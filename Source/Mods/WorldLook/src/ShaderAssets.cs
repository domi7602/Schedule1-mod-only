using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using S1Mods.Shared;
using UnityEngine;

namespace WorldLook;

/// <summary>Owns the embedded water bundle; scene-owned material clones borrow its shader.</summary>
internal static class ShaderAssets
{
    private const string ResourceName = "WorldLook.Shaders.bundle";
    private const int MaximumBytes = 64 * 1024 * 1024;
    private static readonly Dictionary<string, Shader> Shaders = new(StringComparer.Ordinal);
    private static readonly HashSet<string> FailedAssets = new(StringComparer.Ordinal);
    private static AssetBundle? _bundle;
    private static bool _attempted;
    internal static string BundleHash { get; private set; } = "not-loaded";
    internal static string LoadRoute { get; private set; } = "not-loaded";

    public static Shader? LoadShader(string assetPath, Action<string> log)
    {
        if (Shaders.TryGetValue(assetPath, out var cached)) return cached;
        if (FailedAssets.Contains(assetPath)) return null;
        if (!EnsureLoaded(log)) return null;
        try
        {
            var shader = _bundle!.LoadAsset<Shader>(assetPath);
            if (shader == null || !shader.isSupported)
                throw new InvalidDataException("Shader missing or unsupported: " + assetPath);
            Shaders.Add(assetPath, shader);
            log("[assets] Loaded shader " + shader.name);
            return shader;
        }
        catch (Exception ex)
        {
            FailedAssets.Add(assetPath);
            log("[assets] " + ex.GetType().Name + ": " + ex.Message);
            return null;
        }
    }

    private static bool EnsureLoaded(Action<string> log)
    {
        if (_bundle != null) return true;
        if (_attempted) return false;
        _attempted = true;
        try
        {
            using Stream stream = typeof(ShaderAssets).Assembly.GetManifestResourceStream(ResourceName)
                ?? throw new FileNotFoundException("Embedded water bundle missing; rebuild WorldLook.");
            if (stream.Length < 16 || stream.Length > MaximumBytes)
                throw new InvalidDataException("Embedded shader bundle has an invalid size.");
            using var buffer = new MemoryStream((int)stream.Length);
            stream.CopyTo(buffer);
            byte[] bytes = buffer.ToArray();
            BundleHash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
            _bundle = TryLoadFromMemory(bytes, log);
            if (_bundle != null) LoadRoute = "embedded-memory";
            else
            {
                string directory = SafeStorage.GetUserDataPath("WorldLook", "ShaderCache");
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, BundleHash + ".bundle");
                bool matches = false;
                if (File.Exists(path))
                {
                    byte[] old = File.ReadAllBytes(path);
                    matches = old.Length == bytes.Length &&
                        Convert.ToHexString(SHA256.HashData(old)).Equals(BundleHash, StringComparison.OrdinalIgnoreCase);
                }
                if (!matches)
                {
                    string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
                    try
                    {
                        File.WriteAllBytes(temporary, bytes);
                        if (File.Exists(path)) File.Replace(temporary, path, null);
                        else File.Move(temporary, path);
                    }
                    finally { try { if (File.Exists(temporary)) File.Delete(temporary); } catch { } }
                }
                _bundle = AssetBundle.LoadFromFile(path);
                LoadRoute = "embedded-cache";
            }
            if (_bundle == null) throw new InvalidDataException("Unity rejected the embedded bundle.");
            log($"[assets] route={LoadRoute}, bytes={bytes.Length}, sha256={BundleHash}");
            return true;
        }
        catch (Exception ex)
        {
            LoadRoute = "failed";
            log("[assets] Water shader unavailable; native recipe retained: " + ex.GetType().Name + ": " + ex.Message);
            return false;
        }
    }

    private static AssetBundle? TryLoadFromMemory(byte[] bytes, Action<string> log)
    {
        foreach (MethodInfo method in typeof(AssetBundle).GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            if (method.Name != "LoadFromMemory" || method.ReturnType != typeof(AssetBundle)) continue;
            var parameters = method.GetParameters();
            if (parameters.Length != 1 && parameters.Length != 2) continue;
            if (parameters.Length == 2 && parameters[1].ParameterType != typeof(uint)) continue;
            object? nativeBytes = CreateByteArgument(parameters[0].ParameterType, bytes);
            if (nativeBytes == null) continue;
            try
            {
                object?[] arguments = parameters.Length == 1
                    ? new object?[] { nativeBytes }
                    : new object?[] { nativeBytes, 0u };
                var loaded = method.Invoke(null, arguments) as AssetBundle;
                GC.KeepAlive(nativeBytes);
                if (loaded != null) return loaded;
            }
            catch (Exception ex)
            {
                log("[assets] Memory loader unavailable; using embedded cache: " + (ex.InnerException ?? ex).Message);
                return null;
            }
        }
        return null;
    }

    private static object? CreateByteArgument(Type type, byte[] bytes)
    {
        if (type == typeof(byte[])) return bytes;
        if (!type.IsGenericType || type.GetGenericArguments().Length != 1 ||
            type.GetGenericArguments()[0] != typeof(byte) ||
            type.GetGenericTypeDefinition().FullName != "Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1") return null;
        try
        {
            var constructor = type.GetConstructor(new[] { typeof(byte[]) });
            if (constructor != null) return constructor.Invoke(new object[] { bytes });
            foreach (var conversion in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var p = conversion.GetParameters();
                if (conversion.Name == "op_Implicit" && conversion.ReturnType == type &&
                    p.Length == 1 && p[0].ParameterType == typeof(byte[]))
                    return conversion.Invoke(null, new object[] { bytes });
            }
        }
        catch { }
        return null;
    }

    public static void Dispose()
    {
        Shaders.Clear();
        FailedAssets.Clear();
        // Retained material consumers may still borrow the shader after a failed restore.
        try { if (_bundle != null) _bundle.Unload(false); } catch { }
        _bundle = null;
        _attempted = false;
    }
}

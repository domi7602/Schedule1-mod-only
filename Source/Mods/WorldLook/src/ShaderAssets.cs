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
    private static readonly Dictionary<string, CachedShader> Shaders = new(StringComparer.Ordinal);
    private static AssetBundle? _bundle;

    // Called once per explicit ReferenceWaterRuntime initialization, never from its frame loop.
    // A failed request may retry on the next enable/load; managed wrappers are not native roots.
    public static Shader? LoadShader(string assetPath, Action<string> log)
    {
        if (Shaders.TryGetValue(assetPath, out var cached))
        {
            if (IsSupported(cached.Value)) return cached.Value;
            cached.ReleaseRetention();
            Shaders.Remove(assetPath);
        }
        if (!EnsureLoaded(log)) return null;
        CachedShader? candidate = null;
        try
        {
            var shader = _bundle!.LoadAsset<Shader>(assetPath);
            if (!IsSupported(shader))
                throw new InvalidDataException("Shader missing or unsupported: " + assetPath);
            candidate = new CachedShader(shader!);
            candidate.Retain();
            Shaders.Add(assetPath, candidate);
            return shader;
        }
        catch (Exception ex)
        {
            candidate?.ReleaseRetention();
            log("[assets] " + ex.GetType().Name + ": " + ex.Message);
            return null;
        }
    }

    private static bool IsSupported(Shader? shader)
    {
        try { return WaterShader.WaterEnvironment.IsUsable(shader) && shader!.isSupported; }
        catch { return false; }
    }

    private static bool EnsureLoaded(Action<string> log)
    {
        if (WaterShader.WaterEnvironment.IsUsable(_bundle)) return true;
        _bundle = null;
        try
        {
            using Stream stream = typeof(ShaderAssets).Assembly.GetManifestResourceStream(ResourceName)
                ?? throw new FileNotFoundException("Embedded water bundle missing; rebuild WorldLook.");
            if (stream.Length < 16 || stream.Length > MaximumBytes)
                throw new InvalidDataException("Embedded shader bundle has an invalid size.");
            using var buffer = new MemoryStream((int)stream.Length);
            stream.CopyTo(buffer);
            byte[] bytes = buffer.ToArray();
            _bundle = TryLoadFromMemory(bytes, log);
            if (!WaterShader.WaterEnvironment.IsUsable(_bundle))
            {
                string bundleHash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
                string directory = SafeStorage.GetUserDataPath("WorldLook", "ShaderCache");
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, bundleHash + ".bundle");
                bool matches = false;
                if (File.Exists(path))
                {
                    byte[] old = File.ReadAllBytes(path);
                    matches = old.Length == bytes.Length &&
                        Convert.ToHexString(SHA256.HashData(old)).Equals(bundleHash, StringComparison.OrdinalIgnoreCase);
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
            }
            if (!WaterShader.WaterEnvironment.IsUsable(_bundle))
                throw new InvalidDataException("Unity rejected the embedded bundle.");
            return true;
        }
        catch (Exception ex)
        {
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
                if (WaterShader.WaterEnvironment.IsUsable(loaded)) return loaded;
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
        foreach (var shader in Shaders.Values) shader.ReleaseRetention();
        Shaders.Clear();
        // Retained material consumers may still borrow the shader after a failed restore.
        try { if (WaterShader.WaterEnvironment.IsUsable(_bundle)) _bundle!.Unload(false); } catch { }
        _bundle = null;
    }

    private sealed class CachedShader
    {
        internal Shader Value { get; }
        private bool _addedRetention;

        internal CachedShader(Shader value) { Value = value; }

        internal void Retain()
        {
            HideFlags flags = Value.hideFlags;
            if ((flags & HideFlags.DontUnloadUnusedAsset) != 0) return;
            // Own only this bit; scene teardown may leave no native material using the asset.
            _addedRetention = true;
            Value.hideFlags = flags | HideFlags.DontUnloadUnusedAsset;
            if ((Value.hideFlags & HideFlags.DontUnloadUnusedAsset) == 0)
                throw new InvalidDataException("Unity did not retain the embedded water shader.");
        }

        internal void ReleaseRetention()
        {
            if (!_addedRetention || !WaterShader.WaterEnvironment.IsUsable(Value)) return;
            try
            {
                Value.hideFlags &= ~HideFlags.DontUnloadUnusedAsset;
                _addedRetention = false;
            }
            catch { /* Never destroy an asset that a retained material may still borrow. */ }
        }
    }
}

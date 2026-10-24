using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// These are the small, explicitly implemented services used by the supplied
// WorldLook sources. They do not depend on an unprovided S1Mods.Shared assembly.
namespace S1Mods.Shared;

public sealed class ModLogger
{
    private readonly string _name;
    public ModLogger(string name) => _name = name;
    public void Info(string message) => MelonLogger.Msg($"[{_name}] {message}");
    public void Warn(string message, Exception? error = null)
        => MelonLogger.Warning($"[{_name}] {message}" +
            (error == null ? "" : $" ({error.GetType().Name}: {error.Message})"));
}

internal static class NetworkGuard
{
    public static bool IsInMainScene
    {
        get
        {
            try
            {
                var scene = SceneManager.GetSceneByName("Main");
                return scene.IsValid() && scene.isLoaded;
            }
            catch { return false; }
        }
    }

    public static bool IsAlive(UnityEngine.Object? value)
    {
        try
        {
            return !ReferenceEquals(value, null) && !value.WasCollected &&
                value.Pointer != IntPtr.Zero && value != null;
        }
        catch { return false; }
    }
}

internal static class HotkeyManager
{
    public static bool IsInputFieldFocused()
    {
        try
        {
            var current = EventSystem.current;
            if (current == null) return false;
            var selected = current.currentSelectedGameObject;
            return selected != null && (selected.GetComponent<InputField>() != null ||
                selected.GetComponent<TMP_InputField>() != null);
        }
        catch { return true; } // An uncertain input state must not consume a shortcut.
    }
}

internal static class SafeStorage
{
    internal static string UserDataDirectory
    {
        get
        {
            string data = Application.dataPath;
            string game = Directory.GetParent(data)?.FullName
                ?? throw new IOException("Cannot determine the game's data directory.");
            return Path.Combine(game, "UserData");
        }
    }

    public static string GetUserDataPath(string directory, string file)
    {
        if (directory.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
            file.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
            directory == ".." || file == "..")
            throw new ArgumentException("A storage component is not a simple filename.");
        return Path.Combine(UserDataDirectory, directory, file);
    }

    public static bool SaveTextAtomic(string path, string text, ModLogger log)
    {
        string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(temporary, text, new UTF8Encoding(false));
            if (File.Exists(path)) File.Replace(temporary, path, path + ".bak");
            else File.Move(temporary, path);
            return true;
        }
        catch (Exception ex)
        {
            log.Warn("Atomic file write failed; the previous file was retained", ex);
            return false;
        }
        finally
        {
            try { if (File.Exists(temporary)) File.Delete(temporary); } catch { }
        }
    }
}

internal static class ModConfig<T> where T : class, new()
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        IncludeFields = true
    };
    private static string? _path;
    private static ModLogger? _log;
    private static bool _writeAllowed;
    public static T Instance { get; private set; } = new();

    public static void Initialize(string name, ModLogger log)
    {
        if (_path != null) return;
        _log = log;
        _path = SafeStorage.GetUserDataPath("WorldLook", name + ".json");
        try
        {
            // Reading directly distinguishes a confirmed absent file from access
            // failures. File.Exists alone would also return false on some errors.
            string json = File.ReadAllText(_path);
            Instance = JsonSerializer.Deserialize<T>(json, Options)
                ?? throw new InvalidDataException("The configuration contains null.");
            _writeAllowed = true;
            log.Info("Configuration loaded: " + _path);
        }
        catch (FileNotFoundException)
        {
            Instance = new T();
            _writeAllowed = true;
            Flush();
        }
        catch (DirectoryNotFoundException)
        {
            Instance = new T();
            _writeAllowed = true;
            Flush();
        }
        catch (Exception ex)
        {
            Instance = new T();
            _writeAllowed = false;
            log.Warn("Configuration unreadable; session defaults are used and the file will not be overwritten", ex);
        }
    }

    public static void Flush()
    {
        if (!_writeAllowed || _path == null || _log == null) return;
        try { SafeStorage.SaveTextAtomic(_path, JsonSerializer.Serialize(Instance, Options), _log); }
        catch (Exception ex) { _log.Warn("Configuration could not be serialized", ex); }
    }
}

// GameObject resolver copied verbatim from the workspace Shared library
// (Source/Mods/Shared/src/GameObjectResolver.cs, 2026-10-21) because this standalone
// package does not reference S1Mods.Shared.dll.
public static class GameObjectResolver
{
    /// <summary>
    /// Called by SceneGate on scene changes to invalidate any cached UI references.
    /// </summary>
    public static void InvalidateCache()
    {
        // Hook for future UI reference caches
    }

    /// <summary>
    /// Recursively searches for a component of type T below root.
    /// When hintName is provided, only the component with a matching name is returned (or null if not found).
    /// When hintName is empty, the first found component is returned.
    /// </summary>
    public static T? FindComponentDeep<T>(GameObject? root, string hintName = "", ModLogger? log = null) where T : Component
    {
        if (!IsAlive(root))
            return null;

        var components = root!.GetComponentsInChildren<T>(true);
        if (components == null || components.Length == 0)
        {
            if (!string.IsNullOrEmpty(hintName))
                log?.Warn($"GameObjectResolver: No component '{typeof(T).Name}' found below '{root.name}'.");
            return null;
        }

        if (string.IsNullOrEmpty(hintName))
            return components[0];

        foreach (var comp in components)
        {
            if (!IsAlive(comp)) continue;
            if (comp.gameObject.name.IndexOf(hintName, StringComparison.OrdinalIgnoreCase) >= 0)
                return comp;
        }

        // With explicit hintName, do not blindly guess the first child!
        log?.Warn($"GameObjectResolver: No component '{typeof(T).Name}' with name '{hintName}' found below '{root.name}'.");
        return null;
    }

    /// <summary>
    /// Searches for a child GameObject by name or partial name (recursively).
    /// </summary>
    public static GameObject? FindChildDeep(GameObject? root, string childName, bool exactMatch = false)
    {
        if (!IsAlive(root) || string.IsNullOrEmpty(childName))
            return null;

        return FindChildRecursive(root!.transform, childName, exactMatch)?.gameObject;
    }

    /// <summary>
    /// Searches via a flexible path (e.g. "Phone/Apps/Canvas") with tolerance for
    /// missing intermediate nodes.
    /// </summary>
    public static GameObject? FindByPathFuzzy(GameObject? root, string path)
    {
        if (!IsAlive(root) || string.IsNullOrEmpty(path))
            return null;

        string[] segments = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        Transform? current = root!.transform;
        int matched = 0;

        foreach (string segment in segments)
        {
            if (current == null)
                return null;

            Transform? next = FindChildRecursive(current, segment, false);
            if (next == null)
                return null;

            current = next;
            matched++;
        }

        if (matched == 0)
            return null;

        return current?.gameObject;
    }

    private static Transform? FindChildRecursive(Transform parent, string name, bool exactMatch)
    {
        if (!IsAlive(parent)) return null;
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (!IsAlive(child))
                continue;

            bool matches = exactMatch
                ? string.Equals(child.name, name, StringComparison.OrdinalIgnoreCase)
                : child.name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0;

            if (matches)
                return child;

            Transform? foundInSubtree = FindChildRecursive(child, name, exactMatch);
            if (foundInSubtree != null)
                return foundInSubtree;
        }

        return null;
    }

    /// <summary>IL2CPP liveness: managed wrappers survive scene unload while native objects are dead.</summary>
    private static bool IsAlive(UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected && (UnityEngine.Object)obj != null; }
        catch { return false; }
    }
}

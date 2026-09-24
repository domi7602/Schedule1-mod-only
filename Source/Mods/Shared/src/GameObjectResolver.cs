using System;
using UnityEngine;

namespace S1Mods.Shared;

/// <summary>
/// Resilient search and resolution of GameObjects and UI components.
/// Prevents NullReferenceExceptions when developers move, rename, or
/// restructure GameObject hierarchies in game updates.
/// </summary>
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
        if (root == null || string.IsNullOrEmpty(childName))
            return null;

        return FindChildRecursive(root.transform, childName, exactMatch)?.gameObject;
    }

    /// <summary>
    /// Searches via a flexible path (e.g. "Phone/Apps/Canvas") with tolerance for
    /// missing intermediate nodes.
    /// </summary>
    public static GameObject? FindByPathFuzzy(GameObject? root, string path)
    {
        if (root == null || string.IsNullOrEmpty(path))
            return null;

        string[] segments = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        Transform? current = root.transform;
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

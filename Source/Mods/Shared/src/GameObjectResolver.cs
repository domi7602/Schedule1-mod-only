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

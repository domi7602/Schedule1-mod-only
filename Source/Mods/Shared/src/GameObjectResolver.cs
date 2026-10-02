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

    /// <summary>IL2CPP liveness: managed wrappers survive scene unload while native objects are dead.</summary>
    private static bool IsAlive(UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected && (UnityEngine.Object)obj != null; }
        catch { return false; }
    }
}

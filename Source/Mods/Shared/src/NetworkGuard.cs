using System;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace S1Mods.Shared;

/// <summary>
/// Small guards for multiplayer/scene uncertainties. Intentionally without game-type references,
/// so Shared stays stable against framework changes (v0.4.6).
/// Exception: <see cref="NetworkGuard.IsHostOrSingleplayer"/> (partial file
/// NetworkGuard.Host.cs) references Il2CppFishNet — intentionally split out so that
/// this file stays free of game-type references.
/// </summary>
public static partial class NetworkGuard
{
    /// <summary>The name of the primary gameplay scene (default: "Main").</summary>
    public static string MainSceneName
    {
        get => SceneGate.MainSceneName;
        set => SceneGate.MainSceneName = value;
    }

    /// <summary>True, when the main gameplay scene ("Main") is loaded and active.</summary>
    public static bool IsInMainScene => SceneGate.IsInMainScene;

    /// <summary>
    /// Executes the action encapsulated via SafeInvoker only in the Main scene.
    /// Prevents frame crashes from unhandled exceptions during gameplay.
    /// </summary>
    public static bool InGame(Action action, ModLogger? log = null)
    {
        if (action == null || !IsInMainScene)
            return false;

        return SafeInvoker.Execute(action, log, "NetworkGuard.InGame");
    }

    /// <summary>Checks whether a UnityEngine.Object reference is still valid (IL2CPP-safe).</summary>
    public static bool IsAlive(UnityEngine.Object? obj)
    {
        // Bug-Audit 2026-09-12: the overload (UnityEngine.Object) check was missing here,
        // inconsistent with GameObjectResolver.IsAlive / ShopListingSync / Shared internals.
        // A native reference whose wrapper is alive but the underlying object was destroyed
        // (Pointer != Zero but (Object)obj == null) used to slip through.
        if (obj == null) return false;
        try
        {
            if (obj.Pointer == IntPtr.Zero || obj.WasCollected) return false;
            if ((UnityEngine.Object)obj == null) return false;
            return true;
        }
        catch { return false; }
    }

    public static void LogError(ModLogger log, string context, Exception ex)
    {
        log.Error($"{context}: {ex.InnerException?.Message ?? ex.Message}");
    }
}

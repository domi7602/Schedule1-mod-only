using System;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace S1Mods.Shared;

/// <summary>
/// Kleine Guards für Multiplayer/Scene-Unsicherheiten. Bewusst ohne Game-Type-Referenzen,
/// damit Shared gegen Framework-Änderungen (v0.4.6) stabil bleibt.
/// Ausnahme: <see cref="NetworkGuard.IsHostOrSingleplayer"/> (Partial-Datei
/// NetworkGuard.Host.cs) referenziert Il2CppFishNet — bewusst ausgegliedert, damit
/// diese Datei game-typ-frei bleibt.
/// </summary>
public static partial class NetworkGuard
{
    /// <summary>Der Name der primären Gameplay-Szene (Standard: "Main").</summary>
    public static string MainSceneName
    {
        get => SceneGate.MainSceneName;
        set => SceneGate.MainSceneName = value;
    }

    /// <summary>True, wenn die Haupt-Gameplay-Scene ("Main") geladen und aktiv ist.</summary>
    public static bool IsInMainScene => SceneGate.IsInMainScene;

    /// <summary>
    /// Führt action gekapselt via SafeInvoker nur in der Main-Scene aus.
    /// Verhindert Frame-Crashes bei unhandled Exceptions während des Gameplays.
    /// </summary>
    public static bool InGame(Action action, ModLogger? log = null)
    {
        if (action == null || !IsInMainScene)
            return false;

        return SafeInvoker.Execute(action, log, "NetworkGuard.InGame");
    }

    /// <summary>Prüft, ob eine UnityEngine.Object-Referenz noch gültig ist (IL2CPP-sicher).</summary>
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

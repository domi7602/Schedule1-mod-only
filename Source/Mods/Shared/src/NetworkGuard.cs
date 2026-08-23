using System;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace S1Mods.Shared;

/// <summary>
/// Kleine Guards für Multiplayer/Scene-Unsicherheiten. Bewusst ohne Game-Type-Referenzen,
/// damit Shared gegen Framework-Änderungen (v0.4.6) stabil bleibt.
/// </summary>
public static class NetworkGuard
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
        return obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected;
    }

    public static void LogError(ModLogger log, string context, Exception ex)
    {
        log.Error($"{context}: {ex.InnerException?.Message ?? ex.Message}");
    }
}

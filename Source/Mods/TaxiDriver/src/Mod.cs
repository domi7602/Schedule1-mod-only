using System;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(TaxiDriver.Mod), "TaxiDriver", "0.8.2", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace TaxiDriver;

public class Mod : MelonMod
{
    /// <summary>Shared logger — prefixes every line with [TaxiDriver].</summary>
    internal static ModLogger Log { get; } = new("TaxiDriver");

    public override void OnInitializeMelon()
    {
        TaxiLog.Load();

        // IL2CPP registration: the TaxiApp search field's typing guard (phoneapp
        // Key Rule 5). The public IntPtr constructor is mandatory.
        try { ClassInjector.RegisterTypeInIl2Cpp<TaxiAppInputFocus>(); }
        catch (Exception ex) { Log.Warn($"Failed to register TaxiAppInputFocus: {ex.Message}"); }

        Log.Info("initialised — Stage 1 spike (autonomous drive / NPC seat / player enter-exit) + Stage 2 visual swap (taxi.glb via S1MAPI.Gltf.GltfLoader) + Stage 3 driver ride (vanilla NPC drives, player rides along; seat proof = OccupantNPCs slot + root-to-seat distance) + Stage 3b taxi stand (every ParkingLot is dumped on scene load, the taxi spawns on the fixed stand and drives TO the player when called from the Taxi app) + Stage 3c passenger ride (ride locks + destination picker) + Stage 4 (the GAME's own patrol driver cloned onto the taxi: runtime VehiclePatrolRoute + VehiclePatrolBehaviour, with the mod's Navigate dispatch as fallback) + Stage 3d (destination catalog from the game's own deal locations / lot entries, drop-off rule, STOP despawns the taxi, progress watchdog, ride heartbeat).");

        // Idempotent: unsubscribe first so a re-initialised mod never double-hooks.
        MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
        MelonEvents.OnUpdate.Subscribe(SpikeRunner.Update);
        Log.Info("SpikeRunner.Update subscribed to MelonEvents.OnUpdate.");

        // Stage 3c: the passenger-ride input + trunk locks (PatchGuard — a failed
        // patch is non-fatal and named in the log; the ride still works without it).
        RideLocks.Apply();

        // Close experiment (transparent frame): armed once, reverts cleanly.
        TaxiCloseExperiment.Enable();

        // Paket A (2026-09-29): the destination picker must NOT survive a save
        // reload (Dominik: "das Taxi merkt sich die letzten punkte, nachdem man das
        // game reloaded"). OnSaveInfoLoaded is the workspace's canonical save-load
        // boundary (fires after save parsing, before scene build).
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
        Log.Info("GameLifecycle.OnSaveInfoLoaded hooked — the destination picker is cleared on every save load.");

        Log.Info("Console: `taxi help` (both `taxi` and `/taxi` are registered) — but the MelonLoader console is log-only, so every taxi command is output-only. Hotkeys are the control surface (F1-F4 and F6-F12): F1 road target A, F2 road target B, F3 full run to road target, F4 visual swap on/off toggle (applies to the next spawn), F6 full run, F7 probe, F8 trace, F9 diag, F10 go2 (settings=null), F11 run without NPC, F12 Navigate on a vanilla vehicle. Hotkeys only fire in the gameplay scene (menu scenes keep their own keys).");
    }

    /// <summary>
    /// Stale-state guard for save switches: navigation polling, the F6 automation and
    /// a pending respawn are cleared when a scene loads and the spike vehicle is gone.
    /// </summary>
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        SpikeRunner.OnSceneLoaded(sceneName);
    }

    public override void OnApplicationQuit()
    {
        Teardown("quit");
    }

    /// <summary>Review minor: teardown must not live only in OnApplicationQuit.</summary>
    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        Teardown("deinit");
    }

    /// <summary>
    /// Paket A: every save load starts with a clean destination picker — the stale
    /// static picker state was the "taxi drives to the last checkpoint after reload" bug.
    /// </summary>
    private static void OnSaveInfoLoaded()
    {
        SpikeState.ResetPicker();
        Log.Info("[picker] save loaded — destination selection cleared (pick a place before the next ride).");
    }

    private static void Teardown(string reason)
    {
        MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
        SpikeRunner.Shutdown();
        Log.Info($"SpikeRunner.Update unsubscribed from MelonEvents.OnUpdate ({reason}).");
    }
}

// ---------------------------------------------------------------------------
// Merged from TaxiLog.cs (2026-10-02) — mod-local log split (quiet by default).
// ---------------------------------------------------------------------------

/// <summary>
/// Mod-local log split (2026-10-02). Info/Warn/Error stay unconditional; the automatic
/// diagnostic stream (navigation polls, ride heartbeat, one-shot dumps) goes through
/// <see cref="Verbose"/> and is quiet unless <c>UserData/TaxiDriver/log.json</c> sets
/// <c>VerboseLogging=true</c>. The file is written on first run so it can be hand-edited;
/// on-demand output (hotkeys, <c>taxi</c> diagnostics) is not affected either way.
/// </summary>
internal static class TaxiLog
{
    private sealed class LogConfig
    {
        /// <summary>Re-enables the full diagnostic stream (nav poll, heartbeat, dumps).</summary>
        public bool VerboseLogging { get; set; }
    }

    private static bool _loaded;
    private static LogConfig? _config;

    /// <summary>True when <c>log.json</c> enables the diagnostic stream.</summary>
    internal static bool VerboseEnabled { get; private set; }

    private static string FilePath => SafeStorage.GetUserDataPath("TaxiDriver", "log.json");

    /// <summary>Loads <c>log.json</c> once (idempotent); missing file = quiet + default written.</summary>
    internal static void Load()
    {
        if (_loaded)
            return;

        _loaded = true;
        try
        {
            _config = SafeStorage.LoadSafe(FilePath, new LogConfig(), Mod.Log);
            VerboseEnabled = _config.VerboseLogging;
            if (!System.IO.File.Exists(FilePath))
            {
                SafeStorage.SaveAtomic(FilePath, _config, Mod.Log);
                Mod.Log.Info($"[log] quiet by default - {FilePath} written (set VerboseLogging=true for the full diagnostic stream).");
            }
        }
        catch (Exception ex)
        {
            _config = null;
            Mod.Log.Warn($"[log] log.json load failed ({ex.Message}) - verbose logging stays off.");
        }
    }

    /// <summary>Diagnostic-only line: printed when <c>log.json</c> enables verbose logging.</summary>
    internal static void Verbose(string msg)
    {
        if (VerboseEnabled)
            Mod.Log.Info(msg);
    }
}

// ---------------------------------------------------------------------------
// InputFocus guard for the TaxiApp search field (phoneapp Key Rule 5).
// ---------------------------------------------------------------------------

/// <summary>
/// InputFocus guard (phoneapp Key Rule 5) for the TaxiApp destination search:
/// while the InputField is focused, Controls.IsTyping suppresses player
/// movement (WASD) and the taxi hotkeys (SpikeRunner checks IsTyping).
/// Registered via ClassInjector in Mod.OnInitializeMelon - the public IntPtr
/// constructor is mandatory (without it AddComponent crashes the IL2CPP
/// bridge). Mirrors MessagesPlusInputFocus.
/// </summary>
[RegisterTypeInIl2Cpp]
internal sealed class TaxiAppInputFocus : MonoBehaviour
{
    public TaxiAppInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField searchInput = null!;

    private bool _lastTyping;

    private void Update()
    {
        // IL2CPP: isFocused is a proxy read and can throw on a collected
        // wrapper (teardown frames) - never latch Controls.IsTyping on doubt.
        bool typing = false;
        try { typing = searchInput != null && S1Mods.Shared.NetworkGuard.IsAlive(searchInput) && searchInput.isFocused; }
        catch { typing = false; }
        if (typing != _lastTyping)
        {
            _lastTyping = typing;
            S1API.Input.Controls.IsTyping = typing;
        }
    }

    private void OnDisable()
    {
        _lastTyping = false;
        S1API.Input.Controls.IsTyping = false;
    }
}

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

        Log.Info("initialized — TaxiApp controls the service. CALL TAXI uses CallTaxi → TickAutoRun (stand spawn, taxi_driver boarding, pickup navigation); destination selection and STOP remain in the app. Vanilla vehicle AI and taxi.glb rendering remain enabled.");

        // Idempotent: unsubscribe first so a re-initialised mod never double-hooks.
        MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
        MelonEvents.OnUpdate.Subscribe(SpikeRunner.Update);
        Log.Info("[taxi-runner] update loop subscribed.");

        // Passenger rides fail closed unless every required input/trunk patch installs.
        RideLocks.Apply();

        // Beta 8 exposes OnPreLoad and OnPreSceneChange on the installed API.
        // Clear the picker before a save switch and cancel deferred work before
        // scene changes; the live scene callback handles the post-unload verdict.
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnPreSceneChange += OnPreSceneChange;
        Log.Info("GameLifecycle.OnPreLoad and OnPreSceneChange hooked; picker, pending work and ride state are cleared at lifecycle boundaries.");

        Log.Info("TaxiDriver service is controlled through the smartphone app. TaxiDriver installs no keyboard bindings; vanilla E entry/exit and Escape phone-close remain available. `taxi help` lists diagnostic commands.");
    }

    /// <summary>
    /// Stale-state guard for save switches: navigation polling, pickup automation and
    /// a pending respawn are cleared when a scene loads and the tracked taxi is gone.
    /// </summary>
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        SpikeRunner.OnSceneLoaded(sceneName);
    }

    public override void OnApplicationQuit()
    {
        UnsubscribeLifecycle();
        Teardown("quit");
    }

    /// <summary>Review minor: teardown must not live only in OnApplicationQuit.</summary>
    public override void OnDeinitializeMelon()
    {
        UnsubscribeLifecycle();
        Teardown("deinit");
    }

    private static void OnPreLoad()
    {
        bool taxiTracked = !ReferenceEquals(SpikeState.Vehicle, null) ||
                           SpikeState.VehicleOwnership.TrackedPointer != IntPtr.Zero;
        if (taxiTracked && !SpikeCommands.Cleanup("save/load boundary", explicitRetry: false))
            Log.Warn("[taxi-recovery] save/load cleanup is not confirmed; tracked taxi handle is retained and another spawn is blocked.");
        SpikeState.ResetPicker();
        Log.Info("[picker] save load starting — destination selection cleared.");
    }

    private static void OnPreSceneChange()
    {
        SpikeRunner.OnPreSceneChange();
    }

    private static void UnsubscribeLifecycle()
    {
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnPreSceneChange -= OnPreSceneChange;
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
/// on-demand output (TaxiApp and <c>taxi</c> diagnostics) is not affected either way.
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
/// movement (WASD) while typing; TaxiDriver itself installs no key bindings.
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

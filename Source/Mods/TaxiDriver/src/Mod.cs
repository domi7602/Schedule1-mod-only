// ---------------------------------------------------------------------------
// Mod.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   Mod.cs                         -> section Mod.cs
//   TaxiTuning.cs                  -> section TaxiTuning.cs
//
// ---------------------------------------------------------------------------

using MelonLoader;

[assembly: MelonInfo(typeof(TaxiDriver.Mod), "TaxiDriver", "1.0.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace TaxiDriver
{
    // ==== TaxiDriver section: Mod.cs ====
    using System;
    using Il2CppInterop.Runtime.Injection;
    using MelonLoader;
    using S1API.Lifecycle;
    using S1Mods.Shared;
    using UnityEngine;
    using UnityEngine.UI;


    public class Mod : MelonMod
    {
        /// <summary>Shared logger — prefixes every line with [TaxiDriver].</summary>
        internal static ModLogger Log { get; } = new("TaxiDriver");

        /// <summary>Session gate: callbacks captured from an earlier initialization cannot restart.</summary>
        internal static TaxiLifecycle Lifecycle { get; } = new();

        public override void OnInitializeMelon()
        {
            if (!Lifecycle.Initialize())
            {
                Log.Warn("[taxi-lifecycle] initialization refused while shutdown cleanup is running.");
                return;
            }
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
            NativeActuatorPatch.Apply();
            CruiseSpeedPatch.Apply();
            // Scope private flags at parameterless consumers; never detour the
            // GetWheelSweepCollision out-parameter method that failed in 0.8.11.
            TrafficLightPatch.Apply();

            // Beta 8 exposes OnPreLoad and OnPreSceneChange on the installed API.
            // Clear the picker before a save switch and cancel deferred work before
            // scene changes; the live scene callback handles the post-unload verdict.
            // Defensive removal first: a partial initialization must never double-hook.
            GameLifecycle.OnPreLoad -= OnPreLoad;
            GameLifecycle.OnPreSceneChange -= OnPreSceneChange;
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
            if (!Lifecycle.AcceptingWork)
                return;
            bool taxiTracked = !ReferenceEquals(SpikeState.Vehicle, null) ||
                               SpikeState.VehicleOwnership.TrackedPointer != IntPtr.Zero;
            if (taxiTracked && !SpikeCommands.Cleanup("save/load boundary", explicitRetry: false))
                Log.Warn("[taxi-recovery] save/load cleanup is not confirmed; tracked taxi handle is retained and another spawn is blocked.");
            SpikeState.ResetPicker();
            SpikeState.ClearServiceStatus();
            Log.Info("[picker] save load starting — destination selection cleared.");
        }

        private static void OnPreSceneChange()
        {
            if (!Lifecycle.AcceptingWork)
                return;
            SpikeRunner.OnPreSceneChange();
        }

        private static void UnsubscribeLifecycle()
        {
            GameLifecycle.OnPreLoad -= OnPreLoad;
            GameLifecycle.OnPreSceneChange -= OnPreSceneChange;
        }

        private static void Teardown(string reason)
        {
            (string Label, Action Run)[] cleanup =
            {
                ("quiesce updates and deferred work", () =>
                {
                    MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
                    SpikeRunner.Shutdown();
                }),
                ("settle the fare meter once", () => FareMeter.Stop($"session teardown ({reason})")),
                ("stop patrol", () => TaxiAI.StopPatrol($"session teardown ({reason})")),
                ("stop owned taxi navigation/actions", () => SpikeCommands.StopVehicleForShutdown($"session teardown ({reason})")),
                ("restore owned trunk", RideLocks.UnlockTrunk),
                ("release passenger ride state", SpikeState.ResetRide),
                ("reset managed drive state", SpikeRunner.ResetDrivingState),
                ("finalize owned navigation callbacks", () => { SpikeState.NextNavOrder(); }),
            };
            (string Label, Action Run)[] finalization =
            {
                ("remove actuator hook", () => { NativeActuatorPatch.Remove(); }),
                ("remove cruise hook", () => { CruiseSpeedPatch.Remove(); }),
                ("remove traffic-light hook", () => { TrafficLightPatch.Remove(); }),
                ("remove ride protection hooks", () => { RideLocks.Remove(); }),
                ("remove diagnostic hooks", () => { SpikeTrace.Remove(); }),
            };
            Lifecycle.Shutdown(cleanup, finalization, SpikeCommands.ReportCleanupFailure);
            Log.Info($"[taxi-lifecycle] teardown complete ({reason}) — work quiesced, meter settled once, owned taxi retained; hooks removed last.");
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
    // ==== end TaxiDriver section: Mod.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: TaxiTuning.cs ====
    using System;
    using Il2CppScheduleOne.Vehicles;
    using Il2CppScheduleOne.Vehicles.AI;
    using S1Mods.Shared;


    /// <summary>
    /// Driving-agent tuning (Paket 1, review 2026-10-02). <c>VehicleAgent.StuckTimeThreshold</c>
    /// is an INSTANCE field on the agent (verified in the installed interop assembly:
    /// <c>Il2CppScheduleOne.Vehicles.AI.VehicleAgent.StuckTimeThreshold</c> with a public
    /// setter) — so this changes only the taxi's own agent, never any other vehicle in the
    /// game, and needs no global restore (the agent dies with the vehicle).
    ///
    /// Config: <c>UserData/TaxiDriver/tuning.json</c> (SafeStorage, hand-editable).
    /// </summary>
    internal static class TaxiTuning
    {
        /// <summary>Hand-editable driving tuning (public properties: System.Text.Json round-trip).</summary>
        public sealed class TuningConfig
        {
            /// <summary>
            /// Seconds the game's own agent tolerates a stuck vehicle before it un-sticks it
            /// (vanilla default 15). 0 = leave the game's value untouched.
            /// </summary>
            public float StuckTimeThresholdSeconds { get; set; } = 6f;
        }

        private static TuningConfig? _config;
        private static bool _configLoaded;

        /// <summary>The sidecar path (also logged on first load).</summary>
        internal static string FilePath => SafeStorage.GetUserDataPath("TaxiDriver", "tuning.json");

        private static TuningConfig Config
        {
            get
            {
                if (_configLoaded)
                    return _config!;

                _configLoaded = true;
                _config = SafeStorage.LoadSafe(FilePath, new TuningConfig(), Mod.Log);
                if (!System.IO.File.Exists(FilePath))
                {
                    SafeStorage.SaveAtomic(FilePath, _config, Mod.Log);
                    Mod.Log.Info(
                        $"[tune] first run — default tuning written to {FilePath} " +
                        $"(StuckTimeThresholdSeconds={_config!.StuckTimeThresholdSeconds:0.###}, 0 = game default).");
                }

                if (float.IsNaN(_config.StuckTimeThresholdSeconds) || float.IsInfinity(_config.StuckTimeThresholdSeconds) ||
                    _config.StuckTimeThresholdSeconds < 0f)
                    _config.StuckTimeThresholdSeconds = 6f;
                Mod.Log.Info(
                    $"[tune] loaded {FilePath}: StuckTimeThresholdSeconds={_config.StuckTimeThresholdSeconds:0.###} (0 = game default).");
                return _config;
            }
        }

        /// <summary>
        /// Applies the tuning to a freshly spawned taxi agent. Instance fields only — call
        /// once per spawn (the agent belongs to the vehicle instance).
        /// </summary>
        internal static void Apply(LandVehicle veh)
        {
            float want = Config.StuckTimeThresholdSeconds;
            if (want <= 0f)
            {
                Mod.Log.Info("[tune] StuckTimeThreshold left at the game default (config = 0).");
                return;
            }

            try
            {
                VehicleAgent? agent = veh.Agent;
                if (agent == null)
                {
                    Mod.Log.Warn("[tune] no VehicleAgent on the spawned taxi — tuning skipped.");
                    return;
                }

                float before = agent.StuckTimeThreshold;
                if (Math.Abs(before - want) < 0.01f)
                {
                    Mod.Log.Info($"[tune] StuckTimeThreshold already {before:0.###} s.");
                    return;
                }

                agent.StuckTimeThreshold = want;
                float applied = agent.StuckTimeThreshold;
                Mod.Log.Info(
                    $"[tune] taxi agent StuckTimeThreshold {before:0.###} -> {want:0.###} s " +
                    $"(readback {applied:0.###}; this vehicle's agent only — the game un-sticks earlier).");
                if (Math.Abs(applied - want) >= 0.01f)
                    Mod.Log.Warn("[tune] readback differs — the game may overwrite StuckTimeThreshold (in-game check open).");
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[tune] applying StuckTimeThreshold failed ({ex.GetType().Name}: {ex.Message}) — game default stays.");
            }
        }
    }
    // ==== end TaxiDriver section: TaxiTuning.cs ====
}

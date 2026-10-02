using System;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Per-frame driver for the spike: deferred respawns (freeze guard), navigation
/// polling (0.5 s heartbeat) and the F6 one-key automation (spawn → npc → go 40)
/// scheduled on <see cref="Time.unscaledTime"/>. Hooked into
/// <c>MelonEvents.OnUpdate</c> by <see cref="Mod"/>.
/// </summary>
internal static class SpikeRunner
{
    private const float HeartbeatInterval = 0.5f;
    private const float NavigationTimeoutSeconds = 90f;
    private const float AutoStepDelaySeconds = 1f;
    private const float AutoRideDistance = 40f;

    /// <summary>Fresh measurement window granted to the settings=null retry (review M2).</summary>
    private const float RetryGraceSeconds = 6f;

    // ------------------------------------------- Stage 3d progress supervision

    /// <summary>Motion (metres) that counts as the reverse-manoeuvre effect check.</summary>
    private const float ProgressEpsilonMeters = 0.5f;

    /// <summary>
    /// Seconds without progress before a recovery attempt fires. Was 1.5 s, which is
    /// shorter than a normal red-light/pedestrian wait of the game's own obstacle
    /// braking - the watchdog then took the wheel from a driver that was just waiting
    /// (2026-10-01 review: "verwirrte" Fahr-KI). A real wedge is still caught after 5 s.
    /// </summary>
    private const float StuckWindowSeconds = 5f;
    private const float StaticObstacleStuckSeconds = 2.5f;
    private const float StartupStuckSeconds = 3f;

    // ------------------------- Paket 1 (review 2026-10-02): stuck hardening -------

    /// <summary>
    /// Fixed start deadline after a dispatch (review point 1) - extended only by a real
    /// pause, never by navCalc and never by a duplicate destination pick.
    /// </summary>
    private const float DriveSelfTestSeconds = 8f;

    /// <summary>Sustained speed that proves a real drive start - a single physics or collision impulse must not.</summary>
    private const float DriveStartSustainKmh = 2.5f;
    private const float DriveStartSustainSeconds = 0.5f;

    /// <summary>Net approach (start distance - best distance, NOT summed jitter) that proves a drive start.</summary>
    private const float DriveStartApproachMeters = 4f;

    /// <summary>Cap on one uninterrupted path-calculation phase before it counts as a stall (review point 4).</summary>
    private const float NavCalcGraceSeconds = 8f;

    /// <summary>
    /// Running-stall criteria (review point 2): progress needs the AVERAGE speed over
    /// the window or a net approach - a 0.5 km/h crawl can reach neither before the
    /// window expires. The 2 m approach alternative is deliberately generous
    /// (1.44 km/h direct approach).
    /// </summary>
    private const float ProgressAvgKmh = 5f;
    private const float ProgressApproachMeters = 2f;

    /// <summary>Second reverse attempt length (the first is <see cref="ReverseSeconds"/>).</summary>
    private const float ReverseSecondsLong = 3f;

    /// <summary>Free-spot rescues allowed per ride - never a blind teleport loop.</summary>
    private const int MaxFreeSpotRescuesPerRide = 2;

    /// <summary>Idle guard: speed above this while no valid order runs counts as ghost motion.</summary>
    private const float IdleGuardSpeedKmh = 0.5f;
    private const float IdleGuardSeconds = 1f;
    private const int IdleGuardMaxAttempts = 3;

    /// <summary>Exit debounce (review 2026-10-02): the seat flag has been seen to flap for a frame.</summary>
    private const float PlayerOutDebounceSeconds = 0.3f;

    private static bool _startupArmed;
    private static float _startupAt;
    private static bool _recoveryOwnsNavigation;

    private static bool _selfTestActive;
    private static int _selfTestOrder = -1;
    private static float _selfTestAt;

    private static int _proofOrder = -1;
    private static bool _driveStartConfirmed;
    private static float _proofStartDistance;
    private static float _proofBestDistance;
    private static float _proofSpeedSince;

    private static float _navCalcSince;

    private static float _windowStartDistance;
    private static float _windowBestDistance;

    private static int _reverseAttempt;
    private static Vector3 _reverseOrigin;
    private static int _freeSpotRescues;

    private static float _playerOutSince;
    private static float _idleMovingSince;
    private static int _idleStopAttempts;

    internal static void ArmStartupRecovery(LandVehicle veh)
    {
        _startupArmed = true;
        _startupAt = Time.unscaledTime;
    }

    /// <summary>Patrol dispatch: the 8 s self-test owns the start, the startup rescue stays out (review point 1).</summary>
    internal static void DisarmStartupRecovery()
    {
        _startupArmed = false;
    }

    /// <summary>Arms the fixed start deadline for a patrol dispatch (order-bound).</summary>
    internal static void ArmPatrolSelfTest(int order)
    {
        _selfTestActive = true;
        _selfTestOrder = order;
        _selfTestAt = Time.unscaledTime;
    }

    /// <summary>
    /// Fresh progress window for a dispatch: position, start distance and the best
    /// (smallest) distance seen. All of them reset TOGETHER - a stale "best" would make
    /// the approach test pass instantly and blind the watchdog (review point 2).
    /// </summary>
    internal static void ArmProgressWindow(LandVehicle veh)
    {
        Vector3 position = veh.transform.position;
        SpikeState.ProgressFrom = position;
        SpikeState.ProgressWindowStart = Time.unscaledTime;
        _windowStartDistance = Vector3.Distance(position, SpikeState.NavTarget);
        _windowBestDistance = _windowStartDistance;
    }

    /// <summary>Callback factory for a fresh navigation order (review point 4).</summary>
    internal static Action<VehicleAgent.ENavigationResult> NavigationCallbackFor(int order) =>
        result => OnNavigationResult(order, result);

    /// <summary>
    /// Review 2026-10-02, point 3: the central stop. Invalidates the current order FIRST,
    /// then releases the patrol, stops reverse + navigation, parks the car and clears
    /// every private drive flag. Used by exit, arrival, give-up and STOP; the idle guard
    /// verifies the standstill afterwards.
    /// </summary>
    internal static void StopDriving(string reason)
    {
        SpikeState.NextNavOrder(); // (1) invalidate every in-flight order
        TaxiAI.StopPatrol(reason); // (2) release the game's driver

        LandVehicle? veh = SpikeState.Vehicle;
        if (veh != null && veh.Pointer != IntPtr.Zero)
        {
            try
            {
                veh.Agent?.StopReversing();
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[drive] StopReversing() failed ({reason}): {ex.Message}");
            }

            SpikeCommands.ParkCar(veh, reason);
        }

        SpikeState.ResetNavigation();
        _startupArmed = false;
        _selfTestActive = false;
        _selfTestOrder = -1;
        _proofOrder = -1;
        _driveStartConfirmed = false;
        _proofSpeedSince = 0f;
        _navCalcSince = 0f;
        _reverseAttempt = 0;
        _freeSpotRescues = 0;
        _idleMovingSince = 0f;
        _idleStopAttempts = 0;
        TaxiLog.Verbose($"[drive] stopped ({reason}) — order invalidated, patrol released, navigation off, car parked.");
    }

    /// <summary>Free space (metres) required behind the car before recovery #1 may reverse.</summary>
    private const float ReverseClearanceMeters = 3.5f;

    /// <summary>Length of the reverse manoeuvre in recovery #1 (a wedged car backs up).</summary>
    private const float ReverseSeconds = 1.5f;

    /// <summary>Stage 3d heartbeat interval while a ride runs / the player is seated.</summary>
    private const float RideHeartbeatSeconds = 1f;

    /// <summary>
    /// Distance to the target that counts as "arrived" instead of "stopped short".
    /// Review M1-4: raised from 5 m to 10 m because the proven complete run
    /// (callback=Complete) still ends ~8.3 m from the raw target with
    /// <c>endAtRoad = true</c> — the non-callback verdict must not call that a
    /// failure.
    /// </summary>
    private const float ArrivalThresholdMeters = 10f;

    /// <summary>Stage 4: speed at or below this counts as "standing" while the game's patrol driver owns the ride.</summary>
    private const float PatrolStandingKmh = 1f;

    /// <summary>
    /// Stage 4: arrival is accepted this far out when the game's patrol stands on its
    /// last waypoint (a patrol route ends on its own tolerance, not on ours).
    /// </summary>
    private const float PatrolLooseArrivalMeters = 25f;

    private static bool _updateErrorLogged;

    // ------------------------------------------------- parking-lot dump (Stage 3b)

    /// <summary>Delay between the scene-load callback and the first ParkingLot dump (lots may initialise after the callback).</summary>
    private const float LotDumpFirstDelaySeconds = 3f;

    /// <summary>Delay between two dump attempts while no lot has been found yet.</summary>
    private const float LotDumpRetryDelaySeconds = 7f;

    /// <summary>Give up after this many empty attempts (a menu scene legitimately has no lots).</summary>
    private const int LotDumpMaxAttempts = 4;

    private static bool _lotDumpPending;
    private static float _lotDumpAt;
    private static int _lotDumpAttempt;

    internal static void Update()
    {
        try
        {
            if (Time.timeScale == 0f)
                ShiftPauseDeadlines();

            TickPendingSpawn();
            TickLotDump();
            HandleHotkey();
            TickAutoRun();
            TickRide();
            TickIdleGuard();
            RoadKeeper.Tick();
            TickSettleLog();
            TickNavigationPolling();
            TickHeartbeat();

            // Re-arm the one-shot error report: a healthy pass proves the previous
            // fault is gone, so the next distinct exception is logged again (review M8).
            _updateErrorLogged = false;
        }
        catch (Exception ex)
        {
            // Never spam the log from a per-frame callback.
            if (!_updateErrorLogged)
            {
                _updateErrorLogged = true;
                Mod.Log.Error($"SpikeRunner.Update threw (further occurrences are suppressed until one clean tick): {ex}");
            }
        }
    }

    /// <summary>
    /// Review 2026-10-02: extends every drive deadline by exactly one unscaled frame
    /// while the game is paused, so no recovery fires in a pause and the remaining time
    /// after resuming is exactly what was left before.
    /// </summary>
    private static void ShiftPauseDeadlines()
    {
        float dt = Time.unscaledDeltaTime;
        if (SpikeState.NavReDispatchAt > 0f)
            SpikeState.NavReDispatchAt += dt;
        if (SpikeState.NavStartTime > 0f)
            SpikeState.NavStartTime += dt;
        if (_startupAt > 0f)
            _startupAt += dt;
        if (_selfTestAt > 0f)
            _selfTestAt += dt;
        if (_navCalcSince > 0f)
            _navCalcSince += dt;
        if (_proofSpeedSince > 0f)
            _proofSpeedSince += dt;
        if (_playerOutSince > 0f)
            _playerOutSince += dt;
        if (_idleMovingSince > 0f)
            _idleMovingSince += dt;
        if (SpikeState.ProgressWindowStart > 0f)
            SpikeState.ProgressWindowStart = Time.unscaledTime; // the window must not age through a pause
    }

    // ------------------------------------------- drive-start proof (review 2026-10-02)

    /// <summary>
    /// Proves a REAL drive start for the current order (review point 1): the speed must
    /// hold for <see cref="DriveStartSustainSeconds"/> (a single physics/collision
    /// impulse must not disarm anything), or the car must have approached the target by
    /// <see cref="DriveStartApproachMeters"/> net (start distance - BEST distance, not
    /// the sum of jitter). Shared by the startup rescue and the patrol self-test.
    /// </summary>
    private static void UpdateDriveStartProof(LandVehicle veh, Vector3 position, float distance, float now)
    {
        if (_proofOrder != SpikeState.NavOrder)
        {
            _proofOrder = SpikeState.NavOrder;
            _driveStartConfirmed = false;
            _proofStartDistance = distance;
            _proofBestDistance = distance;
            _proofSpeedSince = 0f;
        }

        if (_driveStartConfirmed)
            return;

        float speed = 0f;
        try
        {
            speed = Mathf.Abs(veh.Speed_Kmh);
        }
        catch (Exception)
        {
            // cosmetic only
        }

        if (speed >= DriveStartSustainKmh)
        {
            if (_proofSpeedSince <= 0f)
                _proofSpeedSince = now;
            else if (now - _proofSpeedSince >= DriveStartSustainSeconds)
            {
                _driveStartConfirmed = true;
                TaxiLog.Verbose(
                    $"[drive] start confirmed by speed ({TaxiDestinations.Num(speed)} km/h held {DriveStartSustainSeconds:0.0} s, order {SpikeState.NavOrder}).");
                return;
            }
        }
        else
        {
            _proofSpeedSince = 0f;
        }

        if (distance < _proofBestDistance)
            _proofBestDistance = distance;
        if (_proofStartDistance - _proofBestDistance >= DriveStartApproachMeters)
        {
            _driveStartConfirmed = true;
            TaxiLog.Verbose(
                $"[drive] start confirmed by approach ({TaxiDestinations.Num(_proofStartDistance - _proofBestDistance)} m net toward the target, order {SpikeState.NavOrder}).");
        }
    }

    /// <summary>
    /// Running-stall progress check (review point 2): the window only resets on an
    /// average speed of <see cref="ProgressAvgKmh"/> over the window or
    /// <see cref="ProgressApproachMeters"/> of net approach. Returns false while a
    /// freshly opened window has not evaluated yet.
    /// </summary>
    private static bool EvaluateProgress(Vector3 position, float distance, float now)
    {
        if (SpikeState.ProgressWindowStart <= 0f)
        {
            SpikeState.ProgressFrom = position;
            SpikeState.ProgressWindowStart = now;
            _windowStartDistance = distance;
            _windowBestDistance = distance;
            return false; // a fresh window never counts as progress (and never as a stall)
        }

        _windowBestDistance = Mathf.Min(_windowBestDistance, distance);
        float window = now - SpikeState.ProgressWindowStart;
        float moved = Vector3.Distance(position, SpikeState.ProgressFrom);

        bool fastEnough = window > 0.05f && moved / window * 3.6f >= ProgressAvgKmh;
        bool approached = _windowStartDistance - _windowBestDistance >= ProgressApproachMeters;
        if (!fastEnough && !approached)
            return false;

        SpikeState.ProgressFrom = position;
        SpikeState.ProgressWindowStart = now;
        _windowStartDistance = distance;
        _windowBestDistance = distance;
        return true;
    }

    /// <summary>
    /// Free-spot rescue with a per-ride budget (review 2026-10-02): used when the rear
    /// is blocked or a reverse provably did not move the car. Never a blind teleport -
    /// <see cref="RoadKeeper.TryRescueStartup"/> checks the oriented box, ground and
    /// occupancy first.
    /// </summary>
    private static bool TryFreeSpotRescue(LandVehicle veh, VehicleAgent agent, string context)
    {
        if (_freeSpotRescues >= MaxFreeSpotRescuesPerRide)
        {
            Mod.Log.Warn(
                $"[patrol] {context}: free-spot rescue budget exhausted ({_freeSpotRescues}/{MaxFreeSpotRescuesPerRide}) — not relocating again.");
            return false;
        }

        if (!RoadKeeper.TryRescueStartup(veh, agent, context))
            return false;

        _freeSpotRescues++;
        return true;
    }

    /// <summary>
    /// Review point 4 forensics: the same agent is read twice back-to-back in one line
    /// (pointers + order included) — settles whether the [nav]/[hb] flag mismatch is
    /// temporal or tied to different instances.
    /// </summary>
    private static void LogAgentProbe(string context)
    {
        LandVehicle? veh = SpikeState.Vehicle;
        try
        {
            VehicleAgent? agent = veh == null ? null : veh.Agent;
            if (veh == null || agent == null)
            {
                Mod.Log.Warn($"[probe] {context}: no live vehicle/agent (veh={(veh == null ? "null" : "ok")}).");
                return;
            }

            bool a1 = agent.AutoDriving;
            bool n1 = agent.NavigationCalculationInProgress;
            bool a2 = agent.AutoDriving;
            bool n2 = agent.NavigationCalculationInProgress;

            float speed = -1f;
            try
            {
                speed = veh.Speed_Kmh;
            }
            catch (Exception)
            {
                // cosmetic only
            }

            Mod.Log.Warn(
                $"[probe] {context}: order={SpikeState.NavOrder} frame={Time.frameCount} " +
                $"vehPtr=0x{veh.Pointer.ToInt64():X} agentPtr=0x{agent.Pointer.ToInt64():X} " +
                $"autoDriving1={a1} navCalc1={n1} autoDriving2={a2} navCalc2={n2} " +
                $"speed={TaxiDestinations.Num(speed)} km/h pos={SpikeCommands.Fmt(veh.transform.position)} " +
                $"stuck={agent.GetIsStuck()} ahead={DescribeAhead(veh)}");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[probe] {context}: agent read failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Stall forensics (review 2026-10-02): names the first colliders in front of the
    /// bumper (object + layer), so the next stall log says WHAT it hit. The game's own
    /// <c>VehicleAgent.GetForwardObstacle</c> is private on this build — this is the
    /// equivalent probe, not a behavioural gate.
    /// </summary>
    private static string DescribeAhead(LandVehicle veh)
    {
        try
        {
            Vector3 origin = veh.transform.position + Vector3.up * 0.8f + veh.transform.forward * 2.2f;
            RaycastHit[] hits = Physics.RaycastAll(origin, veh.transform.forward, 3f, ~0, QueryTriggerInteraction.Ignore);
            if (hits == null || hits.Length == 0)
                return "clear";

            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            var names = new System.Collections.Generic.List<string>(2);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null || hit.transform == null)
                    continue;
                if (hit.transform.IsChildOf(veh.transform))
                    continue;

                names.Add($"'{hit.collider.name}'(layer {hit.collider.gameObject.layer}, {hit.distance:0.0} m)");
                if (names.Count >= 2)
                    break;
            }
            return names.Count == 0 ? "clear (own colliders ignored)" : string.Join(", ", names);
        }
        catch (Exception ex)
        {
            return $"<probe failed: {ex.Message}>";
        }
    }

    /// <summary>
    /// Review 2026-10-02, point 5: the idle guard lives on "no valid drive/recovery
    /// order" - the individual flags are plausibility checks, not the definition. It
    /// also runs at pickup-arrived and waiting-for-destination; a new dispatch leaves it
    /// by definition. A player driving the taxi solo is intent, never a ghost.
    /// </summary>
    private static void TickIdleGuard()
    {
        if (Time.timeScale == 0f)
            return;

        LandVehicle? veh = SpikeState.Vehicle;
        if (veh == null || veh.Pointer == IntPtr.Zero)
        {
            _idleMovingSince = 0f;
            _idleStopAttempts = 0;
            return;
        }

        bool validOrder = SpikeState.PollingActive || TaxiAI.Active || SpikeState.NavReDispatchAt > 0f ||
                          SpikeState.AutoRunning || _recoveryOwnsNavigation || _selfTestActive ||
                          SpikeState.PendingSpawnCode != null;
        if (validOrder)
        {
            _idleMovingSince = 0f;
            _idleStopAttempts = 0;
            return;
        }

        // The player alone at the wheel (no NPC driver) is intent, never a ghost.
        if (SpikeCommands.SafePlayerInVehicle(veh) && !SpikeCommands.DriverAtWheel(veh))
        {
            _idleMovingSince = 0f;
            _idleStopAttempts = 0;
            return;
        }

        float speed;
        try
        {
            speed = Mathf.Abs(veh.Speed_Kmh);
        }
        catch (Exception)
        {
            return;
        }

        if (speed < IdleGuardSpeedKmh)
        {
            _idleMovingSince = 0f;
            _idleStopAttempts = 0; // standing = the stop worked; fresh budget next time
            return;
        }

        float now = Time.unscaledTime;
        if (_idleMovingSince <= 0f)
        {
            _idleMovingSince = now;
            return;
        }

        if (now - _idleMovingSince < IdleGuardSeconds)
            return;

        _idleMovingSince = now;
        _idleStopAttempts++;
        Mod.Log.Warn(
            $"[guard] the taxi is moving without an active order (speed={TaxiDestinations.Num(speed)} km/h, " +
            $"attempt {_idleStopAttempts}/{IdleGuardMaxAttempts}, destination='{SpikeState.RideDestinationName}') — stopping it.");
        LogAgentProbe("idle guard");
        try
        {
            veh.Agent?.StopNavigating();
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[guard] StopNavigating() failed: {ex.Message}");
        }

        try
        {
            veh.BrakesApplied = true;
            veh.HandbrakeApplied = true;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[guard] setting the brakes failed: {ex.Message}");
        }

        if (_idleStopAttempts >= IdleGuardMaxAttempts)
        {
            Mod.Log.Error("[guard] the taxi keeps moving without an order — press STOP to despawn it (the state is not being stopped silently).");
            _idleStopAttempts = 0;
        }
    }

    // -------------------------------------------------------- deferred spawn

    /// <summary>
    /// Runs a spawn that <see cref="SpikeCommands"/> deferred out of the frame that
    /// destroyed the previous vehicle (freeze guard: destroy and respawn must never
    /// happen in the same frame).
    /// </summary>
    private static void TickPendingSpawn()
    {
        if (SpikeState.PendingSpawnCode == null)
            return;

        if (Time.unscaledTime < SpikeState.PendingSpawnAt || Time.frameCount <= SpikeState.PendingSpawnFrame)
            return;

        string code = SpikeState.PendingSpawnCode;
        SpikeState.CancelPendingSpawn();

        Mod.Log.Info($"[freeze-guard] deferred spawn of '{code}' executing on frame {Time.frameCount} — destroy and respawn are now separated.");
        if (!SpikeCommands.SpawnVehicle(code))
            Mod.Log.Error($"[freeze-guard] deferred spawn of '{code}' failed — see the error above (`taxi spawn` can retry once cleanup succeeded).");
    }

    // --------------------------------------------------------------- hotkey

    /// <summary>
    /// F1–F12 dispatcher — the only in-game control surface, because the
    /// MelonLoader console is log-only (it has no input field, so every `taxi`
    /// command is output-only and the spike is driven from here). Keys are
    /// checked in a fixed order and are ignored while a text field is focused
    /// (<c>S1API.Input.Controls.IsTyping</c>), so typing is never hijacked.
    /// </summary>
    private static void HandleHotkey()
    {
        // A focused text field owns the keystroke — do not hijack typing.
        if (S1API.Input.Controls.IsTyping)
            return;

        // Menu scenes own their keys (e.g. MoreSaveSlots binds F2/R on the save
        // screens); every taxi hotkey only makes sense in the gameplay scene.
        if (!S1Mods.Shared.SceneGate.IsInMainScene)
            return;

        if (Input.GetKeyDown(KeyCode.F6))
        {
            HandleF6();
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            // Console is unreachable in-game (MelonLoader window is log-only),
            // so the diagnostic commands live on hotkeys.
            Mod.Log.Info("F7 → taxi probe.");
            SpikeCommands.Probe();
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            // Single source of truth: SpikeTrace.Logging is exactly what the console
            // path `taxi trace on|off` flips, so the key can never drift from it.
            bool enable = !SpikeTrace.Logging;
            Mod.Log.Info($"F8 → taxi trace {(enable ? "on" : "off")} (Harmony on VehicleAgent.Navigate, NavigationUtility.CalculatePath, NavigationCalculationCallback, StopNavigating).");
            SpikeCommands.Trace(enable ? "on" : "off");
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            LandVehicle? rideVeh = SpikeState.Vehicle;
            if (rideVeh == null)
            {
                Mod.Log.Info("F9 ignored — no spike vehicle (spawn first).");
            }
            else if (rideVeh.LocalPlayerIsInVehicle)
            {
                Mod.Log.Info("F9 → taxi out.");
                SpikeCommands.Out();
            }
            else
            {
                Mod.Log.Info("F9 → taxi ride.");
                SpikeCommands.Ride();
            }
        }
        /// <summary>F10 — `taxi go2`: Navigate with settings=null (A/B against `taxi go`).</summary>
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            if (!TryBeginGo("F10"))
                return;

            // Every vanilla dispatch runs with settings=null — round 4's null
            // retry never got a valid measurement (no callback + 0.5s grace bug).
            Mod.Log.Info("F10 → taxi go2 (Navigate with settings=null, snapped target, callback).");
            SpikeCommands.Go(40f, useSettings: false);
        }

        /// <summary>F11 — full run spawn → go WITHOUT the npc step (occupant hypothesis).</summary>
        else if (Input.GetKeyDown(KeyCode.F11))
        {
            if (!TryBeginGo("F11"))
                return;

            // Occupant hypothesis: maybe an NPC in the driver seat makes Navigate
            // fail immediately. F11 runs spawn → go WITHOUT the npc step.
            Mod.Log.Info("F11 pressed — spawn → go WITHOUT npc (occupant hypothesis).");
            SpikeState.AutoRunning = true;
            SpikeState.AutoStep = 1;
            SpikeState.AutoNextAt = Time.unscaledTime;
            SpikeState.AutoCode = SpikeState.LastVehicleCode;
            SpikeState.SkipNpcStep = true;
            // Review M1-2: never inherit an absolute target from an earlier F3 run.
            SpikeState.AutoTarget = null;
            // Stage 3b: never inherit the F5 stand spawn either.
            SpikeState.StandSpawnPosition = null;
            SpikeState.AutoToPlayer = false;
        }

        /// <summary>F12 — Navigate on a VANILLA (not spawned) vehicle — vehicle-vs-caller A/B.</summary>
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            if (!TryBeginGo("F12"))
                return;

            // Vehicle-vs-caller A/B: dispatch Navigate on a VANILLA vehicle we
            // did not spawn. Complete => our spawn is the problem; Failed =>
            // the mod-side caller context is the problem.
            if (SpikeState.Vehicle != null)
            {
                Mod.Log.Info("F12: cleaning up the spike vehicle first.");
                SpikeCommands.Cleanup();
            }

            Il2CppScheduleOne.Vehicles.LandVehicle? ext = SpikeCommands.FindVanillaVehicle();
            if (ext == null)
            {
                Mod.Log.Error("F12 ignored — no vanilla vehicle found (VehicleManager.AllVehicles empty).");
                return;
            }

            SpikeState.Vehicle = ext;
            Mod.Log.Info($"F12 → Navigate on VANILLA vehicle pos={SpikeCommands.Fmt(ext.transform.position)} (NOT spawned by this mod) — caller unchanged.");
            SpikeCommands.Go(40f);
        }

        /// <summary>F1 — `taxi go` to the vanilla-proven road target (-131.4, -4.0, 51.9).</summary>
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!TryBeginGo("F1"))
                return;

            // Round 11: (-131.4,-4,51.9) is a vanilla Navigate LOCATION that
            // returned NavCalc(result=Success) in round 10 — if OUR dispatch
            // succeeds there too, the blind forward*40 target was the culprit.
            Mod.Log.Info("F1 → Go to vanilla-proven road target (-131.4, -4.0, 51.9).");
            SpikeCommands.Go(0f, useSettings: true, absoluteTarget: SpikeCommands.RideTargetRoadA);
        }

        /// <summary>F2 — `taxi go` to the second proven road target (-17.1, 0.0, 13.4).</summary>
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!TryBeginGo("F2"))
                return;

            // Second proven road point (returned Success twice in round 10).
            Mod.Log.Info("F2 → Go to second proven road target (-17.1, 0.0, 13.4).");
            SpikeCommands.Go(0f, useSettings: true, absoluteTarget: SpikeCommands.RideTargetRoadB);
        }

        /// <summary>F3 — full run spawn → npc → go against the proven road target.</summary>
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            if (!TryBeginGo("F3"))
                return;

            // Round 12: full spike (spawn + npc) against the proven road target
            // — the only configuration that has never been measured with the
            // heartbeat polls (Success + NPC at the wheel).
            Mod.Log.Info("F3 pressed — spawn → npc → go to vanilla road target (-131.4, -4.0, 51.9).");
            SpikeState.AutoRunning = true;
            SpikeState.AutoStep = 1;
            SpikeState.AutoNextAt = Time.unscaledTime;
            SpikeState.AutoCode = SpikeState.LastVehicleCode;
            SpikeState.SkipNpcStep = false;
            SpikeState.AutoTarget = SpikeCommands.RideTargetRoadA;
            // Stage 3b: F3 keeps its own road target — no stand, no player target.
            SpikeState.StandSpawnPosition = null;
            SpikeState.AutoToPlayer = false;
        }

        /// <summary>
        /// F4 — `taxi visual on|off` toggle (review M1-1): the console cannot be
        /// typed into, so the Stage-2 switch needs a key next to F8's trace toggle.
        /// Deliberately NOT behind <see cref="TryBeginGo"/>: the key never starts a
        /// run, it only decides what the NEXT spawn looks like, so it may be pressed
        /// at any time (during a run too — an existing vehicle keeps its visuals).
        /// </summary>
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            // Single source of truth: SpikeState.VisualSwapEnabled is exactly what
            // `taxi visual on|off` writes, so key and command can never drift.
            SpikeState.VisualSwapEnabled = !SpikeState.VisualSwapEnabled;
            Mod.Log.Info(
                $"F4 → visual swap {(SpikeState.VisualSwapEnabled ? "ON" : "OFF")} (applies to next spawn).");
        }

        /// <summary>
        /// F5 — Stage 3b call-taxi flow: the taxi starts at the FIXED taxi stand
        /// (a vanilla ParkingLot spot, Dominik: "das Auto nur via den Taxi-Fahrer
        /// spawnen … einen fixen Punkt, wo er losfährt — Parkplatz") and drives TO
        /// the player: spawn at the stand → nearest NPC boards → navigate to a road
        /// point near the player (see <see cref="RoadTarget"/>). F9 then puts the
        /// local player aboard. The flow itself lives in
        /// <see cref="SpikeCommands.CallTaxi"/> — single source of truth shared with
        /// the Taxi phone app.
        /// </summary>
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            SpikeCommands.CallTaxi("F5");
        }
    }

    /// <summary>
    /// Shared gate for every caller that starts a run (the run-starting hotkeys
    /// and <see cref="SpikeCommands.CallTaxi"/>): while an automatic run is
    /// in progress or a deferred respawn is still pending, the request is ignored
    /// (with a reason) instead of interrupting the run — the same rule the
    /// F3/F6/F11/F12 blocks used to repeat verbatim.
    /// </summary>
    /// <param name="key">The pressed key / caller name, used verbatim in the ignore message.</param>
    /// <returns><c>true</c> when the caller may start its run.</returns>
    internal static bool TryBeginGo(string key)
    {
        if (SpikeState.AutoRunning)
        {
            Mod.Log.Info($"{key} ignored — run in progress (the automatic spike run is already running).");
            return false;
        }

        if (SpikeState.PendingSpawnCode != null)
        {
            Mod.Log.Info($"{key} ignored — respawn pending (a deferred spawn still has to run).");
            return false;
        }

        return true;
    }

    /// <summary>
    /// F6 — the one-key full run: <c>taxi spawn</c> → +1 s <c>taxi npc</c> →
    /// +1 s <c>taxi go 40</c>, scheduled on <see cref="Time.unscaledTime"/> by
    /// <see cref="TickAutoRun"/>.
    /// </summary>
    private static void HandleF6()
    {
        if (!TryBeginGo("F6"))
            return;

        Mod.Log.Info("F6 pressed — starting the full spike: spawn → (1s) npc → (1s) go 40.");
        SpikeState.AutoRunning = true;
        SpikeState.AutoStep = 1;
        SpikeState.AutoNextAt = Time.unscaledTime;
        SpikeState.AutoCode = SpikeState.LastVehicleCode;
        SpikeState.SkipNpcStep = false;
        SpikeState.AutoTarget = null;
        // Stage 3b: F6 is the player-side debug flow — never inherit the F5 stand.
        SpikeState.StandSpawnPosition = null;
        SpikeState.AutoToPlayer = false;
    }

    // ------------------------------------------------------------- auto run

    /// <summary>
    /// Tiny time kernel: one step per due tick, one try/catch per step, abort with a
    /// plain-language message on the first failure. No coroutines, no MonoBehaviours.
    /// </summary>
    private static void TickAutoRun()
    {
        if (!SpikeState.AutoRunning)
            return;

        if (Time.unscaledTime < SpikeState.AutoNextAt)
            return;

        // Step 1 of a deferred-respawn run cannot proceed while the previous vehicle
        // is still being torn down — wait for the pending spawn instead of failing.
        if (SpikeState.AutoStep == 1 && SpikeState.PendingSpawnCode != null)
            return;

        int step = SpikeState.AutoStep;
        try
        {
            if (step == 1)
            {
                // Spawn's guarded cleanup runs SpikeState.Reset() → ResetAutoRun(),
                // which flips AutoRunning off mid-run (Run B died exactly here) and —
                // since review M1-2 — also clears AutoTarget. Keep both: an F3 run
                // that had to clean up an old vehicle must not lose its road target.
                bool autoTarget = SpikeState.AutoTarget.HasValue;
                Vector3 roadTarget = SpikeState.AutoTarget.GetValueOrDefault();
                // Stage 3b: ResetAutoRun clears the call-taxi flag in exactly the
                // same cleanup, so it has to be captured/restored like AutoTarget.
                bool autoToPlayer = SpikeState.AutoToPlayer;

                if (!SpikeCommands.Spawn(SpikeState.AutoCode))
                    throw new InvalidOperationException("taxi spawn reported a failure (see the error above).");

                // Re-arm the automation now that the fresh vehicle exists.
                SpikeState.AutoRunning = true;
                SpikeState.AutoCode = SpikeState.LastVehicleCode;
                SpikeState.AutoStep = 2;
                SpikeState.AutoNextAt = Time.unscaledTime + AutoStepDelaySeconds;
                if (autoTarget)
                    SpikeState.AutoTarget = roadTarget;
                SpikeState.AutoToPlayer = autoToPlayer;
                Mod.Log.Info($"[F6] step 1/3 done (spawn{(autoToPlayer ? " at the taxi stand" : string.Empty)}) — next: npc.");
            }
            else if (step == 2 && SpikeState.SkipNpcStep)
            {
                Mod.Log.Info($"[F11] npc step SKIPPED (no occupant test) — frame={Time.frameCount}.");
                SpikeState.AutoStep = 3;
                SpikeState.AutoNextAt = Time.unscaledTime + AutoStepDelaySeconds;
            }
            else if (step == 2)
            {
                // First log line of the step (review Testbefund 2): if the game freezes
                // here again, this line tells us whether the step ever started at all.
                Mod.Log.Info($"[F6] step 2/3 starting (npc) — frame={Time.frameCount} t={Time.unscaledTime:F1}s.");
                if (!SpikeCommands.Npc())
                    throw new InvalidOperationException("taxi npc reported a failure (see the error above).");

                SpikeState.AutoStep = 3;
                SpikeState.AutoNextAt = Time.unscaledTime + AutoStepDelaySeconds;
                Mod.Log.Info("[F6] step 2/3 done (npc) — next: go 40.");
            }
            else if (step == 3)
            {
                // Stage 3b: the F5 run does not drive to a fixed road target, it
                // drives to a road point resolved around the PLAYER (the central
                // engineering problem — see RoadTarget for the cascade).
                bool toPlayer = SpikeState.AutoToPlayer;
                Vector3? target = SpikeState.AutoTarget;
                string label = target.HasValue ? "absolute" : "40";

                if (toPlayer)
                {
                    target = SpikeCommands.PlayerRoadTarget();
                    if (!target.HasValue)
                        throw new InvalidOperationException("no driveable road target near the player (see the [target] log above).");
                    label = "to player";
                }

                if (!SpikeCommands.Go(target.HasValue ? 0f : AutoRideDistance, useSettings: true, absoluteTarget: target, toPlayer: toPlayer))
                    throw new InvalidOperationException("taxi go reported a failure (see the error above).");

                SpikeState.AutoRunning = false;
                SpikeState.AutoStep = 0;
                Mod.Log.Info($"[F6] step 3/3 done (go {label}) — navigation polling is now reporting progress.");
            }
            else
            {
                Mod.Log.Warn($"[F6] unknown auto step {step} — stopping the automation.");
                SpikeState.ResetAutoRun();
            }
        }
        catch (Exception ex)
        {
            SpikeState.ResetAutoRun();
            Mod.Log.Error($"[F6] automatic spike run aborted at step {step}: {ex.Message}");
        }
    }

    // ------------------------------------------------------------- ride kernel

    /// <summary>
    /// Stage 3c ride kernel (per frame, next to <see cref="TickAutoRun"/>): board
    /// detection (<c>RideAwaitingBoard</c> + the game's own E-enter / F9 →
    /// <see cref="SpikeCommands.StartRide"/>), the per-frame passenger seat fix, exit
    /// detection (→ <see cref="SpikeCommands.EndRide"/>, re-armed for the next board) and
    /// the arrival verdict (navigation finished → brakes + handbrake, "Arrived — press E to
    /// exit").
    /// </summary>
    internal static void TickRide()
    {
        LandVehicle? veh = SpikeState.Vehicle;

        // Board detection: the pickup reached the player (or the last ride ended with an
        // exit) and the player just got in (the game's own E-enter or F9) — start the ride.
        // Paket E: rapid E in/out churn waits out the debounce before a new ride starts.
        if (SpikeState.RideAwaitingBoard && veh != null && veh.LocalPlayerIsInVehicle &&
            Time.unscaledTime - SpikeState.LastExitAt >= SpikeCommands.ExitDebounceSeconds)
        {
            SpikeCommands.StartRide("ride-kernel");
            return;
        }

        if (!SpikeState.RideActive)
            return;

        if (veh == null)
        {
            SpikeCommands.EndRide("vehicle gone");
            return;
        }

        // Exit detection: covers the game's own E-exit and `taxi out` / F9.
        // Review 2026-10-02: the seat flag has been seen to flap for a single frame - a
        // spurious "player exited" ended a ride the player never left. The exit now only
        // counts after PlayerOutDebounceSeconds of continuous absence; a re-entry inside
        // the window cancels it.
        if (!veh.LocalPlayerIsInVehicle)
        {
            float now = Time.unscaledTime;
            if (_playerOutSince <= 0f)
            {
                _playerOutSince = now;
                return;
            }

            if (now - _playerOutSince < PlayerOutDebounceSeconds)
                return;

            _playerOutSince = 0f;
            SpikeState.LastExitAt = now;
            SpikeCommands.EndRide("player exited", rearm: true);
            // Paket E: the game placed the player somewhere — verify there is ground
            // (the "fell under the map on exit" report). Covers the native E path.
            SpikeCommands.SafeExitGroundSnap(veh);
            return;
        }

        _playerOutSince = 0f;

        // The seat trap can re-assert (a game seat update moves the player back onto the
        // driver seat) — fix it every frame while riding.
        SpikeCommands.EnsurePassengerSeat(veh);

        // Paket G (2026-09-29): the driver belongs at the wheel every frame too — a
        // driverless taxi never reaches its destination ("der Fahrer ist ausgestiegen").
        SpikeCommands.EnsureDriverSeat(veh);

        // Paket A (2026-09-29): waiting for the first destination pick — no arrival
        // verdict yet (without this gate "riding but not dispatched" would read as
        // ARRIVED instantly). The drive starts via ReRoute from the picker/`taxi to`.
        if (SpikeState.RideAwaitingDestination)
            return;

        // The meter: whole dollars per moving in-game minute (0 km/h is free).
        FareMeter.Tick(veh);

        // Arrival: the ride navigation finished (callback or polling terminal) and nothing
        // is still in flight — hold the taxi in place until the player exits.
        if (!SpikeState.RideArrived && !SpikeState.PollingActive)
        {
            SpikeState.RideArrived = true;
            try
            {
                veh.BrakesApplied = true;
                veh.HandbrakeApplied = true;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[ride] brakes/handbrake on arrival failed: {ex.Message}");
            }

            float dist = Vector3.Distance(veh.transform.position, SpikeState.NavTarget);
            FareMeter.Stop("arrived");
            // Honest verdict (bug1, 2026-09-29): the ride used to announce ARRIVED even
            // 152.9 m off when the callback came back Failed - RideArrived only means
            // "driving done", but every consumer read it as "arrived". Any end beyond the
            // threshold (or with a Failed callback) now counts as "cannot get there".
            if (dist > ArrivalThresholdMeters || SpikeState.NavGaveUp ||
                string.Equals(SpikeState.NavCallbackResult, "Failed", StringComparison.OrdinalIgnoreCase))
            {
                SpikeState.NavGaveUp = true;
            }
            bool gaveUp = SpikeState.NavGaveUp;
            string verdict = dist <= ArrivalThresholdMeters
                ? $"within the {ArrivalThresholdMeters:F0} m arrival threshold"
                : $"BEYOND the {ArrivalThresholdMeters:F0} m arrival threshold (navigation ended early)";
            Mod.Log.Info(
                $"[ride] {(gaveUp ? "GAVE UP at" : "ARRIVED at")} {SpikeState.RideDestinationName} — {dist:F1} m from the target " +
                $"{SpikeCommands.Fmt(SpikeState.NavTarget)} ({verdict}, callback={SpikeState.NavCallbackResult ?? "-"}); " +
                (gaveUp
                    ? "the driver could not reach it — press E to get out, or STOP to despawn the taxi."
                    : "brakes + handbrake set — press E to exit."));
        }
    }

    // ---------------------------------------------------------- settle check

    /// <summary>
    /// One-shot [settle] diagnostic ~2 s after a spawn (armed by
    /// <see cref="SpikeCommands.SpawnVehicle"/>): proves the boundingBox ground-snap fix
    /// holds (the root Y must not drift — a drift means the physics pop is back) and
    /// reports the box + GLB ground gaps.
    /// </summary>
    internal static void TickSettleLog()
    {
        if (SpikeState.SettleCheckAt <= 0f || Time.unscaledTime < SpikeState.SettleCheckAt)
            return;

        LandVehicle? veh = SpikeState.SettleCheckVehicle;
        SpikeState.SettleCheckAt = 0f; // one-shot
        if (veh == null || veh.Pointer == IntPtr.Zero)
            return;

        float rootY = veh.transform.position.y;
        float boxMinY = float.NaN;
        if (SpikeCommands.TryGetVehicleBoxWorldBounds(veh, out Vector3 boxMin, out _))
            boxMinY = boxMin.y;
        float glbMinY = TaxiVisual.CurrentGlbMinY();

        TaxiLog.Verbose(
            $"[settle] rootY={SpikeCommands.FmtY(rootY)} " +
            $"(snap placed {SpikeCommands.FmtY(SpikeState.SettleCheckRootY)}, drift {SpikeCommands.FmtY(rootY - SpikeState.SettleCheckRootY)} m; " +
            $"raw spawn {SpikeCommands.FmtY(SpikeState.SettleCheckSpawnY)}) " +
            $"boxMinY={SpikeCommands.FmtY(boxMinY)} vs hitY {SpikeCommands.FmtY(SpikeState.SettleCheckHitY)} " +
            $"(ground gap {SpikeCommands.FmtY(boxMinY - SpikeState.SettleCheckHitY)} m) glbMinY={SpikeCommands.FmtY(glbMinY)}.");
    }

    // ------------------------------------------- Stage 3d recovery + heartbeat

    /// <summary>
    /// Recovery #1/#2/terminal for a car that is not making progress. #1 backs a
    /// wedged car out (it drove and then stopped) or re-dispatches a car that never
    /// started; #2 drops the player off at a different lot entry near the goal;
    /// after that the navigation gives up with a clear message instead of 90 s of
    /// silence.
    /// </summary>
    private static void RecoverStuck(VehicleAgent agent, LandVehicle veh, Vector3 position, float distance, float elapsed, bool autoDriving)
    {
        SpikeState.StuckRecoveries++;

        // Paket G (2026-09-29): a driverless car never makes progress — re-boarding the
        // driver beats burning recovery #1/#2 on navigation re-dispatches into the void.
        if (SpikeCommands.EnsureDriverSeat(veh))
        {
            SpikeState.StuckRecoveries--;
            ArmProgressWindow(veh);
            return;
        }

        float speed = -1f;
        try
        {
            speed = veh.Speed_Kmh;
        }
        catch (Exception)
        {
            // cosmetic only
        }

        if (SpikeState.StuckRecoveries == 1)
        {
            Mod.Log.Warn(
                $"[patrol] navigation made no progress for {TaxiDestinations.Num(Time.unscaledTime - SpikeState.ProgressWindowStart)} s " +
                $"({TaxiDestinations.Num(distance)} m to the target, AutoDriving={autoDriving} navCalc=false " +
                $"speed={TaxiDestinations.Num(speed)} km/h, {TaxiDestinations.Num(elapsed)} s into the run) — " +
                $"recovery #1: reverse {TaxiDestinations.Num(ReverseSeconds)} s if rear clear, then re-dispatch.");

            ArmProgressWindow(veh);

            // AutoDriving is not evidence of motion: a newly spawned car can be wedged too.
            if (!TryStartReverse(agent, veh, Time.unscaledTime, "navigation recovery"))
            {
                if (TryFreeSpotRescue(veh, agent, "navigation recovery"))
                    ReDispatch(agent, SpikeState.NavTarget, "recovery #1 (free-spot rescue)");
                else
                    ReDispatch(agent, SpikeState.NavTarget, "recovery #1 (no reverse)");
            }
            return;
        }

        if (SpikeState.StuckRecoveries == 2)
        {
            Vector3 goal = SpikeState.RideGoal ?? SpikeState.NavTarget;
            if (TaxiDestinations.TryFallbackDropOff(goal, SpikeState.NavTarget, out Vector3 fallback, out string lot))
            {
                Mod.Log.Warn(
                    $"[nav] recovery #2: the agent cannot reach {SpikeCommands.Fmt(SpikeState.NavTarget)} — " +
                    $"dropping off at lot '{lot}' {SpikeCommands.Fmt(fallback)} instead " +
                    $"({TaxiDestinations.Num(Vector3.Distance(goal, fallback))} m from the goal).");
                SpikeState.RideDropOff = "lot entry (fallback after a failed approach)";
                SpikeState.RideDropOffLot = lot;
                SpikeState.RideDestination = fallback;
                ArmProgressWindow(veh);
                ReDispatch(agent, fallback, "recovery #2 (fallback drop-off)");
                return;
            }

            Mod.Log.Warn("[nav] recovery #2: no alternative lot entry near the goal — one more dispatch.");
        }

        // Stage 4: the re-dispatch budget is the GAME's own number.
        // MAX_CONSECUTIVE_PATHING_FAILURES means exactly "how many failed path
        // calculations in a row before giving up", so the ladder may re-dispatch that
        // many times before the terminal branch fires.
        int cap = Math.Max(2, TaxiAI.PathingFailureCap);
        if (SpikeState.StuckRecoveries <= cap)
        {
            Mod.Log.Warn(
                $"[nav] recovery #{SpikeState.StuckRecoveries}: re-dispatching again — the game's own supervision " +
                $"tolerates {cap} consecutive pathing failures.");
            ArmProgressWindow(veh);
            ReDispatch(agent, SpikeState.NavTarget, $"recovery #{SpikeState.StuckRecoveries}");
            return;
        }

        GiveUpNavigation(agent, position, distance, elapsed);
    }

    /// <summary>
    /// True when a short ray behind the car (at bumper height, from the rear axle
    /// line) hits no solid. Triggers are ignored; the car's own colliders are skipped
    /// by starting the ray past the rear bumper.
    /// </summary>
    private static bool RearIsClear(LandVehicle veh)
    {
        return !RoadKeeper.HasObstacle(veh, veh.transform.position, veh.transform.rotation,
            rear: true, distance: ReverseClearanceMeters);
    }

    private static bool TryStartReverse(VehicleAgent agent, LandVehicle veh, float now, string reason)
    {
        if (!RearIsClear(veh))
        {
            Mod.Log.Warn($"[patrol] {reason}: rear blocked; no reverse.");
            return false;
        }
        // Keep the agent's path alive for StartReverse; do not StopNavigating here.
        _recoveryOwnsNavigation = true;
        try
        {
            // Deactivate can eject the NPC. Re-seat now, not on the next TickRide,
            // where boarding could interrupt an already running reverse manoeuvre.
            SpikeCommands.EnsureDriverSeat(veh);
            veh.BrakesApplied = false;
            veh.HandbrakeApplied = false;

            // Review 2026-10-02: the second attempt in the same episode reverses longer
            // (the first 1.5 s often only unsticks the wheels but not the bumper).
            _reverseOrigin = veh.transform.position;
            _reverseAttempt = Math.Min(_reverseAttempt + 1, 2);
            float seconds = _reverseAttempt >= 2 ? ReverseSecondsLong : ReverseSeconds;

            agent.StartReverse();
            SpikeState.NavReDispatchAt = now + seconds;
            SpikeState.NavReDispatchTarget = SpikeState.NavTarget;
            SpikeState.PollingActive = true;
            Mod.Log.Warn($"[patrol] {reason}: reversing {seconds:0.0}s (attempt {_reverseAttempt}), rear clear; then redispatch to {SpikeCommands.Fmt(SpikeState.NavTarget)}.");
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[patrol] {reason}: reverse failed: {ex.Message}");
            return false;
        }
        finally
        {
            _recoveryOwnsNavigation = false;
        }
    }

    private static bool TickStartupRecovery(VehicleAgent agent, LandVehicle veh, Vector3 position, float distance, bool navCalc)
    {
        if (!_startupArmed || distance <= ArrivalThresholdMeters)
            return false;

        // Review 2026-10-02: neither an impulse nor a few cm of crawl may disarm the
        // startup protection any more - only a CONFIRMED drive start does (shared proof).
        if (_driveStartConfirmed)
        {
            _startupArmed = false;
            return false;
        }

        float now = Time.unscaledTime;

        // A path calculation is not a wedge - but only up to the fixed grace cap
        // (review point 4): a calculation that never finishes must not mask the rescue.
        if (navCalc && _navCalcSince > 0f && now - _navCalcSince < NavCalcGraceSeconds)
            return false;

        if (now - _startupAt < StartupStuckSeconds)
            return false;

        _startupArmed = false; // one startup rescue per dispatch, not a teleport loop
        _recoveryOwnsNavigation = true;
        try { TaxiAI.StopPatrol("startup stuck"); }
        finally { _recoveryOwnsNavigation = false; }
        if (SpikeCommands.EnsureDriverSeat(veh))
        {
            _startupArmed = true;
            _startupAt = Time.unscaledTime;
            return true;
        }

        SpikeState.StuckRecoveries++;
        LogAgentProbe("startup stuck");
        Mod.Log.Warn(
            $"[patrol] startup stuck: no confirmed drive start after {TaxiDestinations.Num(now - _startupAt)} s " +
            "(position/speed below the start markers) — reverse first, free-spot rescue otherwise.");
        if (TryStartReverse(agent, veh, Time.unscaledTime, "startup stuck"))
            return true;
        if (TryFreeSpotRescue(veh, agent, "startup stuck"))
        {
            ReDispatch(agent, SpikeState.NavTarget, "startup stuck (free-spot rescue)");
            return true;
        }
        ReDispatch(agent, SpikeState.NavTarget, "startup stuck (no reverse, no free spot)");
        return true;
    }

    /// <summary>
    /// Stage 4 supervision while the GAME's patrol behaviour drives: arrival (at the
    /// resolved point, or standing on its last waypoint near it) and stall handling.
    /// A stall does not fight the game's behaviour — it releases it and hands the ride
    /// back to the mod's own <c>Navigate()</c> dispatch, so a broken clone degrades to
    /// the proven path instead of freezing the ride.
    /// </summary>
    private static void TickPatrolSupervision(LandVehicle veh, Vector3 position, float distance, bool navCalc)
    {
        float now = Time.unscaledTime;
        float speed = -1f;
        try
        {
            speed = veh.Speed_Kmh;
        }
        catch (Exception)
        {
            // cosmetic only
        }

        int waypoint = TaxiAI.CurrentWaypoint;
        int waypoints = TaxiAI.WaypointCount;
        int last = Math.Max(0, waypoints - 1);

        bool atLastWaypoint = waypoints > 0 && waypoint >= last;
        bool nearTarget = distance <= ArrivalThresholdMeters;
        bool standingNear = atLastWaypoint && speed <= PatrolStandingKmh && distance <= PatrolLooseArrivalMeters;

        if (nearTarget || standingNear)
        {
            Mod.Log.Info(
                $"[patrol] ARRIVED at {SpikeState.RideDestinationName} — waypoint {waypoint}/{last}, " +
                $"{TaxiDestinations.Num(distance)} m to the target, speed={TaxiDestinations.Num(speed)} km/h " +
                $"({(nearTarget ? $"within the {ArrivalThresholdMeters:F0} m threshold" : "standing on the last waypoint")}).");
            _selfTestActive = false;
            TaxiAI.StopPatrol("arrived");
            SpikeState.PollingActive = false;
            return;
        }

        // ---- fixed start deadline (review 2026-10-02, point 1) --------------------
        // Until a drive start is CONFIRMED (shared proof: sustained speed or real net
        // approach), the 8 s self-test owns the ride; navCalc and stall ladder stay out.
        if (_selfTestActive)
        {
            if (_selfTestOrder != SpikeState.NavOrder)
            {
                _selfTestActive = false; // a new dispatch superseded this ride start
            }
            else if (_driveStartConfirmed)
            {
                _selfTestActive = false;
                Mod.Log.Info("[patrol] the game's driver passed the start self-test — the stall watchdog takes over.");
            }
            else if (now - _selfTestAt >= DriveSelfTestSeconds)
            {
                _selfTestActive = false;
                Mod.Log.Warn(
                    $"[patrol] start self-test failed — no confirmed drive start within {DriveSelfTestSeconds:0} s " +
                    $"(waypoint {waypoint}/{last}, {TaxiDestinations.Num(distance)} m to the target, " +
                    $"speed={TaxiDestinations.Num(speed)} km/h, navCalc={navCalc}) — handing the ride to the mod's Navigate dispatch.");
                LogAgentProbe("start self-test failed");
                _recoveryOwnsNavigation = true;
                try { TaxiAI.StopPatrol("start self-test failed"); }
                finally { _recoveryOwnsNavigation = false; }
                ReDispatch(veh.Agent, SpikeState.NavTarget, "patrol start self-test fallback");
                return;
            }
        }

        // ---- running stall watchdog (review point 2) ------------------------------
        // Progress needs an average speed over the window or a net approach; a crawl
        // can no longer reset the window, and navCalc only excuses its grace cap.
        if (EvaluateProgress(position, distance, now))
        {
            if (SpikeState.StuckRecoveries > 0)
                TaxiLog.Verbose($"[patrol] progress resumed — the car is moving again after recovery #{SpikeState.StuckRecoveries}.");
            SpikeState.StuckRecoveries = 0;
            _reverseAttempt = 0;
            return;
        }

        if (navCalc && _navCalcSince > 0f && now - _navCalcSince < NavCalcGraceSeconds)
            return; // calculation still inside its grace cap — not a stall yet

        bool staticObstacle = RoadKeeper.HasObstacle(veh, position, veh.transform.rotation, staticOnly: true);
        float stallWindow = staticObstacle ? StaticObstacleStuckSeconds : StuckWindowSeconds;
        if (now - SpikeState.ProgressWindowStart < stallWindow)
            return;

        SpikeState.StuckRecoveries++;
        Mod.Log.Warn(
            $"[patrol] the game's own driver made no progress for {TaxiDestinations.Num(stallWindow)} s " +
            $"(staticObstacle={staticObstacle}, navCalc={navCalc}) " +
            $"(waypoint {waypoint}/{last}, {TaxiDestinations.Num(distance)} m to the target, " +
            $"speed={TaxiDestinations.Num(speed)} km/h) — releasing it and handing the ride to the mod's dispatch.");
        LogAgentProbe("patrol stall");
        _recoveryOwnsNavigation = true;
        try { TaxiAI.StopPatrol("stalled"); }
        finally { _recoveryOwnsNavigation = false; }
        ArmProgressWindow(veh);
        if (!TryStartReverse(veh.Agent, veh, now, "patrol stall"))
        {
            if (TryFreeSpotRescue(veh, veh.Agent, "patrol stall"))
                ReDispatch(veh.Agent, SpikeState.NavTarget, "patrol stall (free-spot rescue)");
            else
                ReDispatch(veh.Agent, SpikeState.NavTarget, "patrol stall (no reverse) -> mod dispatch");
        }
    }

    /// <summary>Finishes the reverse manoeuvre and re-dispatches the same route.</summary>
    private static void TickReDispatch(VehicleAgent agent)
    {
        Vector3 target = SpikeState.NavReDispatchTarget;
        SpikeState.NavReDispatchAt = 0f;

        try
        {
            agent.StopReversing();
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[nav] StopReversing() failed: {ex.Message}");
        }

        // Review 2026-10-02: a reverse that provably did not move the car escalates —
        // a second (longer) reverse while possible, then the free-spot rescue.
        LandVehicle? veh = SpikeState.Vehicle;
        bool moved = veh == null ||
                     Vector3.Distance(veh.transform.position, _reverseOrigin) >= ProgressEpsilonMeters;
        if (!moved && veh != null)
        {
            Mod.Log.Warn($"[patrol] reverse attempt {_reverseAttempt} did not move the car — escalating.");
            if (_reverseAttempt < 2 && TryStartReverse(agent, veh, Time.unscaledTime, "reverse ineffective"))
                return;
            if (TryFreeSpotRescue(veh, agent, "reverse ineffective"))
            {
                _reverseAttempt = 0;
                ReDispatch(agent, target, "post-reverse free-spot rescue");
                return;
            }
        }

        _reverseAttempt = 0;
        TaxiLog.Verbose("[patrol] reverse manoeuvre complete — re-dispatching the route.");
        ReDispatch(agent, target, "post-reverse");
    }

    /// <summary>Fresh <c>Navigate</c> dispatch with settings=null and a new measurement window.</summary>
    internal static void ReDispatch(VehicleAgent agent, Vector3 target, string why)
    {
        SpikeState.PollingActive = true;
        SpikeState.NavTarget = target;
        SpikeState.NavStartTime = Time.unscaledTime;
        SpikeState.NavEverAutoDriving = false;
        SpikeState.NavCallbackResult = null;
        SpikeState.NavRetried = true;

        // Invalidate first, dispatch second (review 2026-10-02, point 2): the fresh
        // order owns the callback from here on; a late result of the old order is stale.
        int order = SpikeState.NextNavOrder();

        // The watchdog window must REALLY restart here - the log line below has
        // promised "fresh measurement window" since v0.6.0. Bug evidence (Latest.log
        // 2026-09-29 06:17): TickReDispatch's post-reverse path left ProgressWindowStart
        // stale, so recovery #2 ("cannot reach") fired 0.5 s after the re-dispatch and
        // dumped the player 24 m from the goal. ArmProgressWindow resets position,
        // window, start distance and best distance together (review point 2).
        try
        {
            LandVehicle? v = SpikeState.Vehicle;
            if (v != null)
                ArmProgressWindow(v);
        }
        catch (Exception)
        {
            // the window reset alone already fixes the premature give-up
        }

        // Release the brakes, THEN dispatch. The car is deliberately NOT rotated toward
        // the target any more: that forced yaw ignored the road direction and the
        // collision geometry around the car, so a re-dispatch could turn it nose-first
        // into a lamp/wall/curb ("fährt gegen Objekte" - the same bug 0.4.0 removed
        // from Go(); ReDispatch had kept the old snap). The agent picks its own heading.
        try
        {
            LandVehicle? av = SpikeState.Vehicle;
            if (av != null)
            {
                av.BrakesApplied = false;
                av.HandbrakeApplied = false;
            }
        }
        catch (Exception)
        {
            // dispatching unaimed still beats not dispatching
        }

        try
        {
            agent.Navigate(target, null, NavigationCallbackFor(order));
            TaxiLog.Verbose($"[nav] {why}: Navigate(settings=null) dispatched (order {order}) to {SpikeCommands.Fmt(target)} — fresh measurement window.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[nav] {why}: Navigate(settings=null) failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Terminal of the recovery ladder — never leaves the player in a taxi that
    /// silently does nothing: polling off, brakes on, and an actionable message.
    /// </summary>
    private static void GiveUpNavigation(VehicleAgent agent, Vector3 position, float distance, float elapsed)
    {
        SpikeState.PollingActive = false;
        SpikeState.NavGaveUp = true;
        SpikeState.NextNavOrder(); // invalidate in-flight orders (review point 4)

        Mod.Log.Error(
            $"[nav] GAVE UP after {TaxiDestinations.Num(elapsed)} s since the last re-dispatch and {SpikeState.StuckRecoveries} recovery attempt(s) — " +
            $"the taxi did not move ({TaxiDestinations.Num(distance)} m left to {SpikeCommands.Fmt(SpikeState.NavTarget)}, " +
            $"stuck at {SpikeCommands.Fmt(position)}). Press STOP to despawn the taxi, or E/F9 (`taxi out`) to get out.");

        try
        {
            agent.StopNavigating();
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[nav] StopNavigating() during give-up failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Stage 3d heartbeat (1 s) while a ride runs or the player sits in the taxi.
    /// Purpose: the 2026-09-27 report ("after cancelling, the player was frozen
    /// while the game ran") could not be told apart from a real main-thread hang,
    /// because a hang and a stuck state both look like "no more log lines". With a
    /// heartbeat the answer is unambiguous: heartbeat stops = the main thread is
    /// blocked; heartbeat continues = only the state is wrong.
    /// </summary>
    private static void TickHeartbeat()
    {
        LandVehicle? veh = SpikeState.Vehicle;
        bool riding = SpikeState.RideActive || SpikeState.RideBoarded || SpikeState.RidePassengerMode;

        bool seated = false;
        try
        {
            seated = veh != null && veh.Pointer != IntPtr.Zero && veh.LocalPlayerIsInVehicle;
        }
        catch (Exception)
        {
            // a dead handle counts as "not seated" — the heartbeat is diagnostics only
        }

        if (!riding && !seated)
            return;

        if (Time.unscaledTime - SpikeState.RideHeartbeatAt < RideHeartbeatSeconds)
            return;

        SpikeState.RideHeartbeatAt = Time.unscaledTime;

        string agentState = "-";
        try
        {
            VehicleAgent? agent = veh == null ? null : veh.Agent;
            if (agent != null)
                agentState = $"autoDriving={agent.AutoDriving} navCalc={agent.NavigationCalculationInProgress} " +
                             $"reversing={agent.IsReversing} stuck={agent.GetIsStuck()}";
        }
        catch (Exception ex)
        {
            agentState = $"<agent read failed: {ex.Message}>";
        }

        Vector3 position = Vector3.zero;
        float speed = -1f;
        try
        {
            if (veh != null)
            {
                position = veh.transform.position;
                speed = veh.Speed_Kmh;
            }
        }
        catch (Exception)
        {
            // cosmetic only
        }

        TaxiLog.Verbose(
            $"[hb] frame={Time.frameCount} riding={riding} seated={seated} polling={SpikeState.PollingActive} " +
            $"{agentState} speed={TaxiDestinations.Num(speed)} km/h pos={TaxiDestinations.Fmt(position)} " +
            $"target={SpikeCommands.Fmt(SpikeState.NavTarget)} dist={TaxiDestinations.Num(Vector3.Distance(position, SpikeState.NavTarget))} m " +
            $"recoveries={SpikeState.StuckRecoveries} destination='{SpikeState.RideDestinationName}' dropOff='{SpikeState.RideDropOff}'");
    }

    // ---------------------------------------------------------- nav callback

    /// <summary>
    /// Invoked by <c>VehicleAgent.Navigate</c> through the interop
    /// <c>NavigationCallback</c> conversion — <c>Failed</c> / <c>Complete</c> /
    /// <c>Stopped</c> is the authoritative end of a run (review M7), so polling stops
    /// as soon as a result arrives. Order-bound (review 2026-10-02, point 4): a result
    /// for a superseded order changes nothing.
    /// </summary>
    internal static void OnNavigationResult(int order, VehicleAgent.ENavigationResult result)
    {
        if (order != SpikeState.NavOrder)
        {
            TaxiLog.Verbose($"[nav] stale callback order={order} (current={SpikeState.NavOrder}) ignored — no state changed.");
            return;
        }

        if (_recoveryOwnsNavigation || SpikeState.NavReDispatchAt > 0f)
        {
            TaxiLog.Verbose($"[patrol] old navigation callback {result} ignored during controlled recovery.");
            return;
        }
        string text = result.ToString();
        SpikeState.NavCallbackResult = text;

        float elapsed = SpikeState.NavStartTime > 0f ? Time.unscaledTime - SpikeState.NavStartTime : -1f;
        TaxiLog.Verbose(
            $"[nav] callback result={text} after {elapsed:F1}s (frame={Time.frameCount}, order={order}) — " +
            "authoritative completion signal, navigation polling stops.");
        SpikeState.PollingActive = false;

        // Stage 3b: the F5 run ends HERE — the taxi left the stand and reached the
        // road point next to the player (the local player then boards with E or F9,
        // which starts the Stage-3c passenger ride via the ride kernel).
        if (SpikeState.NavToPlayer && result == VehicleAgent.ENavigationResult.Complete)
        {
            LandVehicle? arrivedVeh = SpikeState.Vehicle;
            float dist = arrivedVeh == null ? -1f : Vector3.Distance(arrivedVeh.transform.position, SpikeState.NavTarget);
            SpikeState.RideAwaitingBoard = true;
            Mod.Log.Info(
                $"[F5] taxi arrived at player (callback=Complete after {elapsed:F1}s, " +
                $"{dist:F1} m from the resolved target {SpikeCommands.Fmt(SpikeState.NavTarget)}) — board with E or F9.");
            // Review point 3: a finished pickup is final — order invalidated, car parked,
            // only then the boarding gate (never a passenger-ride teardown).
            SpikeCommands.CompletePickup(arrivedVeh, "callback=Complete");
        }
        else if (SpikeState.NavToPlayer)
        {
            Mod.Log.Warn($"[F5] call-taxi run ended WITHOUT arriving at the player (callback={text} after {elapsed:F1}s) — see the [target] lines for the candidate that was dispatched.");
            SpikeCommands.CompletePickup(SpikeState.Vehicle, $"callback={text}");
        }
    }

    // ---------------------------------------------------------- nav polling

    /// <summary>
    /// 0.5 s heartbeat while <see cref="SpikeState.PollingActive"/>: logs
    /// AutoDriving / navCalc / distance / graph / stuck / speed / callback,
    /// fires the one <c>settings = null</c> retry, applies the 90 s timeout and
    /// prints the terminal verdict (ARRIVED within
    /// <see cref="ArrivalThresholdMeters"/> vs. STOPPED SHORT).
    /// </summary>
    private static void TickNavigationPolling()
    {
        if (!SpikeState.PollingActive)
            return;

        // Pauses are handled once per frame in ShiftPauseDeadlines (review 2026-10-02):
        // no recovery runs during a pause and every deadline keeps its remaining time.
        if (Time.timeScale == 0f)
            return;
        LandVehicle? veh = SpikeState.Vehicle;
        if (veh == null)
        {
            Mod.Log.Warn("[nav] SpikeState.Vehicle became null — stopping navigation polling.");
            SpikeState.PollingActive = false;
            return;
        }

        VehicleAgent? agent = veh.Agent;
        if (agent == null)
        {
            Mod.Log.Warn("[nav] LandVehicle.Agent became null — stopping navigation polling.");
            SpikeState.PollingActive = false;
            return;
        }

        float now = Time.unscaledTime;
        float elapsed = now - SpikeState.NavStartTime;
        if (now - SpikeState.LastHeartbeat < HeartbeatInterval)
            return;

        SpikeState.LastHeartbeat = now;

        try
        {
            bool autoDriving = agent.AutoDriving;
            bool navCalc;
            try
            {
                navCalc = agent.NavigationCalculationInProgress;
            }
            catch (Exception)
            {
                navCalc = false;
            }

            if (autoDriving)
                SpikeState.NavEverAutoDriving = true;

            // Review 2026-10-02, point 4: one cap per uninterrupted calculation phase.
            // The phase starts with the first navCalc=true and ends the moment it clears;
            // the cap never extends the stall timer, it only bounds the navCalc excuse.
            if (navCalc)
            {
                if (_navCalcSince <= 0f)
                    _navCalcSince = now;
            }
            else
            {
                _navCalcSince = 0f;
            }

            Vector3 position = veh.transform.position;
            float distance = Vector3.Distance(position, SpikeState.NavTarget);
            UpdateDriveStartProof(veh, position, distance, now);

            // A pending post-reverse re-dispatch runs before anything else.
            if (SpikeState.NavReDispatchAt > 0f)
            {
                if (now < SpikeState.NavReDispatchAt)
                    return;

                TickReDispatch(agent);
                return;
            }

            if (TickStartupRecovery(agent, veh, position, distance, navCalc))
                return;
            TaxiLog.Verbose(
                $"[nav t={elapsed:F1}s] order={SpikeState.NavOrder} AutoDriving={autoDriving} navCalc={navCalc} " +
                $"target={SpikeCommands.Fmt(SpikeState.NavTarget)} " +
                $"vehicle={SpikeCommands.Fmt(position)} distToTarget={distance:F1}m " +
                $"onVehicleGraph={agent.IsOnVehicleGraph()} stuck={agent.GetIsStuck()} " +
                $"reversing={agent.IsReversing} " +
                $"speed={veh.Speed_Kmh:F1} km/h callback={SpikeState.NavCallbackResult ?? "-"}");

            // One timeout for every shape of hang: AutoDriving still true, but also a
            // path calculation that never finishes (review: the 90 s limit used to
            // apply only inside the autoDriving branch).
            if (elapsed >= NavigationTimeoutSeconds)
            {
                SpikeState.PollingActive = false;
                SpikeState.NextNavOrder(); // stale callbacks must not act afterwards
                TaxiAI.StopPatrol("navigation timeout");
                Mod.Log.Warn(
                    $"[nav] timeout after {elapsed:F1}s (limit {NavigationTimeoutSeconds:F0}s, AutoDriving={autoDriving} navCalc={navCalc} " +
                    $"everAutoDriving={SpikeState.NavEverAutoDriving} nullRetryFired={SpikeState.NavRetried} callback={SpikeState.NavCallbackResult ?? "-"} ) — " +
                    "stopping navigation and polling.");
                try
                {
                    agent.StopNavigating();
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"VehicleAgent.StopNavigating() during timeout cleanup failed: {ex.Message}");
                }

                return;
            }

            // ---- Stage 4: the GAME's own driver leads (the patrol clone) --------
            // Dominik: "können wir nicht die KI klonen, vom Spiel z.B. die wo das
            // Polizeifahrzeug steuert". While the game's VehiclePatrolBehaviour owns
            // the wheel the mod must NOT fight it with Navigate()/StartReverse() — it
            // only supervises (arrival, stall → hand the ride back, timeout).
            if (TaxiAI.Active)
            {
                TickPatrolSupervision(veh, position, distance, navCalc);
                return;
            }

            // ---- Stage 3d progress supervision ---------------------------------
            // "AutoDriving" is NOT proof of motion. Observed 2026-09-27 (ride to
            // ROAD A): AutoDriving=True, navCalc=False, speed=0.0 km/h, constant
            // distance for 9.6 s — the old `if (autoDriving) return;` skipped the
            // retry AND the terminal branch, so only the 90 s timeout could end it.
            // Measure movement instead of trusting flags. Since review 2026-10-02 the
            // bar is an average speed or a net approach — a crawl cannot reset it.
            if (EvaluateProgress(position, distance, now))
            {
                if (SpikeState.StuckRecoveries > 0)
                    TaxiLog.Verbose($"[nav] progress resumed — the car is moving again after recovery #{SpikeState.StuckRecoveries}.");
                SpikeState.StuckRecoveries = 0;
                _reverseAttempt = 0;
            }
            else if (now - SpikeState.ProgressWindowStart >= (RoadKeeper.HasObstacle(veh, position, veh.transform.rotation, staticOnly: true)
                         ? StaticObstacleStuckSeconds : StuckWindowSeconds)
                     && !(navCalc && _navCalcSince > 0f && now - _navCalcSince < NavCalcGraceSeconds))
            {
                RecoverStuck(agent, veh, position, distance, elapsed, autoDriving);
                return;
            }

            if (autoDriving)
                return;

            // Not driving right now:
            if (navCalc)
                return; // path calculation still running — keep waiting.

            if (!SpikeState.NavRetried && !SpikeState.NavEverAutoDriving && elapsed >= 3f)
            {
                // Nothing drove and no calculation is pending → one A/B retry with
                // NavigationSettings=null (flag mismatch would show up here).
                SpikeState.NavRetried = true;
                SpikeState.NavRetryAt = now;
                int retryOrder = SpikeState.NextNavOrder();
                TaxiLog.Verbose(
                    $"[nav] retry #1: Navigate(target, settings=null, callback) at t={elapsed:F1}s " +
                    $"(order {retryOrder}) (nothing drove with NavigationSettings, no calculation pending) — grace window {RetryGraceSeconds:F0}s starts now.");
                try
                {
                    agent.Navigate(SpikeState.NavTarget, null, NavigationCallbackFor(retryOrder));
                }
                catch (Exception ex)
                {
                    Mod.Log.Error($"[nav] retry Navigate(target, null, callback) threw: {ex.Message}");
                }

                return;
            }

            if (!SpikeState.NavEverAutoDriving && elapsed < 3f)
                return; // give the first dispatch its 3 s before deciding.

            // Review M2: the settings=null retry used to get exactly one heartbeat
            // before the terminal branch fired — keep its own measurement window open.
            if (SpikeState.NavRetried && SpikeState.NavRetryAt > 0f && now - SpikeState.NavRetryAt < RetryGraceSeconds)
                return;

            // Terminal: either the vehicle arrived/stopped after driving, or it
            // never drove at all (including a failed settings=null retry).
            SpikeState.PollingActive = false;
            bool arrivedWithin = distance <= ArrivalThresholdMeters;
            if (arrivedWithin && SpikeState.NavToPlayer)
            {
                // Polling-terminal fallback (no callback ever arrived): the pickup is
                // reached — arm the Stage-3c ride board like OnNavigationResult does.
                SpikeState.RideAwaitingBoard = true;
            }
            string verdict = arrivedWithin
                ? $"ARRIVED (within the {ArrivalThresholdMeters:F0} m threshold){(SpikeState.NavToPlayer ? " — taxi arrived at player" : string.Empty)}"
                : $"STOPPED SHORT ({distance:F1} m from the target — mid-route stop, failed path or an early terminal){(SpikeState.NavToPlayer ? " (call-taxi to the player)" : string.Empty)}";
            TaxiLog.Verbose(
                $"[nav] AutoDriving false and no calculation pending after {elapsed:F1}s — {verdict}; " +
                $"target={SpikeCommands.Fmt(SpikeState.NavTarget)} " +
                $"vehicle={SpikeCommands.Fmt(position)} " +
                $"finalDistance={distance:F1}m (startDistance={SpikeState.NavStartDistance:F1}m, " +
                $"start={SpikeCommands.Fmt(SpikeState.NavStartPosition)}) " +
                $"everAutoDriving={SpikeState.NavEverAutoDriving} nullRetryFired={SpikeState.NavRetried} " +
                $"callback={SpikeState.NavCallbackResult ?? "-"}.");
            if (SpikeState.NavToPlayer)
                SpikeCommands.CompletePickup(veh, arrivedWithin ? "polling terminal (arrived)" : "polling terminal (stopped short)");
        }
        catch (Exception ex)
        {
            SpikeState.PollingActive = false;
            Mod.Log.Error($"[nav] polling stopped — a native VehicleAgent/LandVehicle getter threw: {ex.Message}");
        }
    }

    // ---------------------------------------------------- parking-lot dump

    /// <summary>
    /// Runs the scheduled <see cref="TaxiStand.DumpLots"/> (Stage 3b): one pass per
    /// due tick, retried while no lot has been found, because a scene can report
    /// loaded before its ParkingLot objects exist. The dump is what turns the live
    /// lots into <see cref="TaxiStand.StandCoordinate"/>.
    /// </summary>
    private static void TickLotDump()
    {
        if (!_lotDumpPending || Time.unscaledTime < _lotDumpAt)
            return;

        _lotDumpAttempt++;
        bool found;
        try
        {
            found = TaxiStand.DumpLots($"scene-load dump (attempt {_lotDumpAttempt}/{LotDumpMaxAttempts})");
        }
        catch (Exception ex)
        {
            found = false;
            Mod.Log.Error($"[lots] dump threw: {ex.Message}");
        }

        if (found || _lotDumpAttempt >= LotDumpMaxAttempts)
        {
            _lotDumpPending = false;
            if (!found)
                Mod.Log.Warn($"[lots] no ParkingLot found after {_lotDumpAttempt} attempt(s) — the stand has to fall back to the configured coordinate.");
        }
        else
        {
            _lotDumpAt = Time.unscaledTime + LotDumpRetryDelaySeconds;
        }
    }

    // ------------------------------------------------------------ scene load

    /// <summary>
    /// Stale-state guard for save switches: when a scene (re)loads and the spike
    /// vehicle is gone, every derived reference is cleared. An additive scene load
    /// with the vehicle still alive changes nothing (the PotScanner lesson: never
    /// reset on every scene load).
    /// </summary>
    internal static void OnSceneLoaded(string sceneName)
    {
        // Stage 3b: every scene load re-arms the ParkingLot dump. It is scheduled
        // instead of run inline because scene objects may finish initialising after
        // this callback — the dump is the source of the taxi-stand coordinates.
        _lotDumpPending = true;
        _lotDumpAttempt = 0;
        _lotDumpAt = Time.unscaledTime + LotDumpFirstDelaySeconds;
        TaxiLog.Verbose($"[lots] ParkingLot dump scheduled {LotDumpFirstDelaySeconds:F1}s after scene '{sceneName}' loaded.");

        // Paket A (2026-09-29): leaving the gameplay scene clears the destination
        // picker too (belt and braces next to GameLifecycle.OnSaveInfoLoaded — a
        // reload always passes through a non-Main scene).
        if (!string.Equals(sceneName, "Main", StringComparison.OrdinalIgnoreCase))
        {
            Mod.Log.Info($"[picker] scene '{sceneName}' (left the gameplay scene) — destination selection cleared.");
            SpikeState.ResetPicker();
        }

        LandVehicle? veh = SpikeState.Vehicle;
        bool vehicleGone = veh == null; // Unity fake-null also covers destroyed objects
        bool hasState = SpikeState.PollingActive || SpikeState.AutoRunning ||
                        SpikeState.DriverNpc != null || SpikeState.PendingSpawnCode != null ||
                        SpikeState.LastVehicleCode != null;

        if (!vehicleGone || !hasState)
            return;

        Mod.Log.Info(
            $"[scene '{sceneName}'] spike vehicle is gone — clearing navigation polling, F6 automation, " +
            "pending respawn and NPC state (stale-state guard).");
        SpikeState.Reset();
    }

    /// <summary>Used by the mod teardown so a quit never leaves the runner half-alive.</summary>
    internal static void Shutdown()
    {
        SpikeState.PollingActive = false;
        SpikeState.CancelPendingSpawn();
        SpikeState.ResetAutoRun();
    }
}

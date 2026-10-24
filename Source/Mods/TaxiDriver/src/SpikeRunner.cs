using System;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TaxiDriver;

/// <summary>
/// Per-frame driver for TaxiApp pickup automation, deferred respawns (freeze
/// guard), navigation polling (0.5 s heartbeat) and ride supervision. Hooked into
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

    /// <summary>Speed drop that proves a stop action worked (anything less = the brakes do nothing).</summary>
    private const float IdleSlowEpsilonKmh = 0.5f;

    /// <summary>Quiet interval once the brake action is proven ineffective (log-proven roll case).</summary>
    private const float IdleQuietSeconds = 30f;

    /// <summary>Exit debounce (review 2026-10-02): the seat flag has been seen to flap for a frame.</summary>
    private const float PlayerOutDebounceSeconds = 0.3f;

    private static bool _startupArmed;
    private static float _startupAt;
    private static bool _recoveryOwnsNavigation;

    /// <summary>Unscaled time at which the current reverse window started (telemetry).</summary>
    private static float _reverseStartedAt;

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
    private static Vector3 _reverseForward;
    private static float _reverseRearCheckAt;
    private static int _freeSpotRescues;

    private static float _playerOutSince;
    private static float _idleMovingSince;
    private static int _idleStopAttempts;
    private static bool _idleIneffective;
    private static float _idleLastSpeed;
    private static float _idleNextLogAt;

    private static Vector3 _operationProgressTarget;
    private static readonly RecoveryRecurrencePolicy RecoveryRecurrence = new();
    private static bool _localRecoveryProgress;
    private static bool _trafficWaiting;
    private static readonly TrafficWaitBudget TrafficWait = new();

    // Fixed goal and best approach survive patrol handoff, reverse and redispatch.
    private static void ObserveNavigationProgress()
    {
        _trafficWaiting = false;
        if (Time.timeScale == 0f || !SpikeState.PollingActive) return;
        try
        {
            LandVehicle? veh = SpikeState.Vehicle;
            if (veh == null) return;
            VehicleAgent? agent = veh.Agent;
            if (agent == null) return;
            bool trafficBlocked = TrafficLightPatch.HoldRecovery(veh);
            bool withinArrival = NavigationArrivalPolicy.IsWithinArrival(
                Vector3.Distance(veh.transform.position, SpikeState.NavTarget));
            float dt = TrafficWait.Observe(trafficBlocked, Time.unscaledDeltaTime, withinArrival);
            _trafficWaiting = dt > 0f;
            if (_trafficWaiting)
            {
                // A positively classified traffic stop is neither progress nor a failed
                // manoeuvre. Exclude only observed wait time; never refill the budget.
                SpikeState.NavigationRecovery.ShiftPause(dt);
                SpikeState.ProgressWindowStart += dt;
                if (_selfTestAt > 0f) _selfTestAt += dt;
                if (_startupAt > 0f) _startupAt += dt;
                if (SpikeState.NavStartTime > 0f) SpikeState.NavStartTime += dt;
                return;
            }
            bool eligible = !SpikeState.Reverse.Active && !agent.IsReversing && !_recoveryOwnsNavigation && !TaxiAI.NavigationBlocked;
            Vector3 position = veh.transform.position;
            SpikeState.NavigationRecovery.ObserveProgress(Time.unscaledTime,
                Vector3.Distance(position, _operationProgressTarget), eligible);
            // Signed speed plus a positive native drive request exclude reverse and
            // unpowered coasting. This affects recovery history only, never steering.
            bool eligibleForward = eligible && (agent.AutoDriving || TaxiAI.Active) &&
                veh.Speed_Kmh >= DriveStartSustainKmh && veh.currentThrottle > 0f;
            bool progressed = RecoveryRecurrence.ObserveProgress(
                Vector3.Distance(position, _operationProgressTarget), eligibleForward);
            _localRecoveryProgress |= progressed;
        }
        catch (Exception)
        {
            // A failed observation buys no time, and cannot skip terminal supervision.
        }
    }

    internal static void ObserveArtificialPlacement(Vector3 position)
    {
        SpikeState.NavigationRecovery.ObserveProgress(Time.unscaledTime,
            Vector3.Distance(position, _operationProgressTarget), false);
        RecoveryRecurrence.ObserveProgress(Vector3.Distance(position, _operationProgressTarget), false);
        _localRecoveryProgress = false;
    }

    /// <summary>Only a genuinely new pickup/ride starts a new budget; redispatch never does.</summary>
    internal static void BeginNavigationOperation()
    {
        _trafficWaiting = false;
        TrafficWait.Reset();
        NavigationTerminalPolicy.Begin(ref SpikeState.NavGaveUp, ref SpikeState.RideArrived,
            ref SpikeState.NavCallbackResult);
        // A genuinely new operation: new identity (separate from the changing NavOrder),
        // a fresh budget, a fresh explicit lifecycle and a cleared manoeuvre history.
        SpikeState.NextOperationId();
        SpikeState.Operation.Begin();
        SpikeState.Reverse.End();
        SpikeState.NavigationRecovery.Begin(Time.unscaledTime, NavigationTimeoutSeconds);
        _operationProgressTarget = SpikeState.NavTarget;
        LandVehicle? veh = SpikeState.Vehicle;
        if (veh != null) ObserveArtificialPlacement(veh.transform.position);
        RecoveryRecurrence.Begin(veh == null ? float.PositiveInfinity :
            Vector3.Distance(veh.transform.position, _operationProgressTarget));
        _localRecoveryProgress = false;
        _freeSpotRescues = 0;
        _reverseAttempt = 0;
    }

    private static bool StopIfNavigationExpired(VehicleAgent agent, LandVehicle? veh, float now)
    {
        if (_trafficWaiting) return false;
        if (!SpikeState.NavigationRecovery.HasExpired(now)) return false;
        Mod.Log.Warn($"[nav] no meaningful approach for {NavigationTimeoutSeconds:F0}s excluding pauses — stopping reverse, patrol and navigation.");
        Vector3 position = Vector3.zero;
        try { if (veh != null) position = veh.transform.position; }
        catch { /* a failed position read must not prevent the terminal stop */ }
        GiveUpNavigation(agent, position, Vector3.Distance(position, SpikeState.NavTarget), now - SpikeState.NavStartTime);
        return true;
    }

    /// <summary>
    /// Spends one bounded operation recovery episode. <paramref name="maneuverPlanned"/> says
    /// whether the caller is about to attempt an actual local manoeuvre (reverse/rescue): only
    /// then does the spatial recurrence history advance. A pure control hand-off after a failed
    /// start self-test spends budget but records no failed manoeuvre, so the first real stall at
    /// the same place still reaches a normal reverse (brief §6.2).
    /// </summary>
    /// <returns><c>true</c> when the caller may attempt its manoeuvre; <c>false</c> when this
    /// call already handled the situation (recurring rescue or terminal give-up).</returns>
    private static bool TryBeginRecovery(VehicleAgent agent, LandVehicle veh, bool maneuverPlanned)
    {
        if (_trafficWaiting) return false;
        float now = Time.unscaledTime;
        if (StopIfNavigationExpired(agent, veh, now)) return false;
        if (TaxiAI.NavigationBlocked)
        {
            TaxiAI.StopPatrol("pending release");
            if (TaxiAI.NavigationBlocked) return false;
        }
        _localRecoveryProgress = false;
        if (SpikeState.NavigationRecovery.TrySpend(now, TaxiAI.PathingFailureCap))
        {
            if (!maneuverPlanned)
                return true; // budget spent; no physical manoeuvre failed at this position

            Vector3 stall = veh.transform.position;
            if (!RecoveryRecurrence.RegisterStall(stall.x, stall.z)) return true;

            // Same area, no new forward approach: do not repeat the reverse ladder.
            // Both successful and refused rescue keep the existing finite budgets.
            Mod.Log.Warn($"[nav] recurring stall at {SpikeCommands.Fmt(stall)} within {RecoveryRecurrencePolicy.StallRadiusMeters:F0} m without new forward approach — checked free-spot rescue instead of reverse.");
            _recoveryOwnsNavigation = true;
            try
            {
                TaxiAI.StopPatrol("recurring stall");
                SpikeCommands.EnsureDriverSeat(veh);
                if (!SpikeState.PollingActive) return false;
                bool rescued = TryFreeSpotRescue(veh, agent, "recurring stall");
                ReDispatch(agent, SpikeState.NavTarget,
                    rescued ? "recurring stall (free-spot rescue)" : "recurring stall (no safe free spot)");
            }
            finally { _recoveryOwnsNavigation = false; }
            return false; // handled; no caller may fall through to another reverse
        }

        Vector3 position = veh.transform.position;
        float distance = Vector3.Distance(position, SpikeState.NavTarget);
        Mod.Log.Warn("[nav] operation recovery budget exhausted — one terminal graph rescue, then give up.");
        if (!TryTeleportRecovery(veh, agent, position, distance))
            GiveUpNavigation(agent, position, distance, now - SpikeState.NavStartTime);
        return false;
    }

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
        Mod.Lifecycle.Guard<VehicleAgent.ENavigationResult>(result => OnNavigationResult(order, result));

    /// <summary>
    /// Review 2026-10-02, point 3: the central stop. Invalidates the current order FIRST,
    /// then releases the patrol, stops reverse + navigation, parks the car and clears
    /// every private drive flag. Used by exit, arrival, give-up and STOP; the idle guard
    /// verifies the standstill afterwards.
    /// </summary>
    internal static void StopDriving(string reason)
    {
        // Managed reverse intent FIRST, then invalidate every in-flight order (brief §7.4): a
        // throwing cleanup step must never leave the order (or the reverse intent) re-armed.
        SpikeState.Reverse.End();
        SpikeState.NavReDispatchAt = 0f;
        SpikeState.Operation.Terminate("stop driving", failed: false);
        SpikeState.NextNavOrder(); // (1) invalidate every in-flight order
        CleanupGuard.Run("StopDriving StopPatrol", () => TaxiAI.StopPatrol(reason), SpikeCommands.ReportCleanupFailure);

        LandVehicle? veh = SpikeState.Vehicle;
        bool ownedAlive = veh != null && veh.Pointer != IntPtr.Zero && SpikeCommands.IsOwnedTaxi(veh);
        // New native stop/park input needs current authority, the exact own live taxi and no
        // actual player driver (brief §7.4); a player takeover gets managed cancellation only.
        bool nativeOk = ownedAlive && ReverseSupervisionPolicy.NativeCleanupPermitted(
            SpikeCommands.HasAuthority(), ownedAlive, PlayerDriver(veh));
        if (nativeOk)
        {
            CleanupGuard.Run("StopDriving StopReversing", () => veh!.Agent?.StopReversing(), SpikeCommands.ReportCleanupFailure);
            CleanupGuard.Run("StopDriving park", () => SpikeCommands.ParkCar(veh!, reason), SpikeCommands.ReportCleanupFailure);
        }
        else if (ownedAlive)
        {
            Mod.Log.Warn("[drive] stop: no current authority/ownership or a player is at the wheel — managed cancellation only, no new native input.");
        }

        ResetDrivingState();
        TaxiLog.Verbose($"[drive] stopped ({reason}) — order invalidated, patrol released, navigation off, car parked.");
    }

    /// <summary>Managed-only drive teardown, also safe after native taxi death.</summary>
    internal static void ResetDrivingState()
    {
        _trafficWaiting = false;
        TrafficWait.Reset();
        // A drive ending does not end the visual model's lifetime. Keep its wheel
        // rest pose/angle cache while the parked taxi can be reused for another ride.
        SpikeState.ResetNavigation();
        SpikeState.Reverse.End();
        RecoveryRecurrence.Begin(float.PositiveInfinity);
        _localRecoveryProgress = false;
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
        _idleIneffective = false;
        _idleLastSpeed = 0f;
        _idleNextLogAt = 0f;
        _recoveryOwnsNavigation = false;
        _playerOutSince = 0f;
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
    internal const float ArrivalThresholdMeters = NavigationArrivalPolicy.ArrivalMeters;

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

    private static bool MainSceneLoaded()
    {
        try { return SceneManager.GetSceneByName("Main").isLoaded; }
        catch { return false; }
    }

    internal static void Update()
    {
        if (!Mod.Lifecycle.AcceptingWork)
        {
            // Lifecycle gone: clear the managed reverse/intent even though the full update path
            // is skipped, and never issue native input (brief §7.3/D14).
            SpikeState.Reverse.End();
            SpikeState.NavReDispatchAt = 0f;
            return;
        }
        try
        {
            if (Time.timeScale == 0f)
                ShiftPauseDeadlines();

            SpikeCommands.ReconcileDeadTaxi();
            SpikeCommands.TickDestroyVerification();
            if (!MainSceneLoaded())
            {
                // No scene, no safe native access: still clear any managed reverse (brief §7.3/D14).
                if (SpikeState.Reverse.Active || SpikeState.NavReDispatchAt > 0f)
                    RevokeReverse("update: gameplay scene not loaded");
                TickLotDump();
                return;
            }
            TickPendingSpawn();
            TickLotDump();

            LandVehicle? trackedVehicle = SpikeState.Vehicle;
            if (!ReferenceEquals(trackedVehicle, null) && !SpikeCommands.IsOwnedTaxi(trackedVehicle))
            {
                if (SpikeState.AutoRunning)
                    SpikeState.ResetAutoRun();
                // A foreign/uncertain handle: revoke the managed reverse, never touch the vehicle (D14).
                if (SpikeState.Reverse.Active || SpikeState.NavReDispatchAt > 0f)
                    RevokeReverse("update: foreign/uncertain tracked vehicle");
                return; // no polling, ride, recovery or visual work touches a foreign/uncertain handle
            }

            TickAutoRun();
            ObserveNavigationProgress();
            // Supervise the bounded reverse independently of PollingActive and BEFORE ride
            // finalization: a polling getter error or a lost handle must not leave the managed
            // reverse request (or the meter verdict) stale (brief §7.3).
            TickReverseSupervision();
            // Drain a queued navigation callback BEFORE ride finalization (brief §8.1): all native
            // work happens here on the normal Update path, never inside the native callback.
            TickNavigationCallback();
            // Confirm a pending arrival (brief §8.2): a current Complete is only evidence until a
            // readable, confirmed halt inside the target radius is observed. Runs independently of
            // PollingActive and BEFORE ride finalization so an accepted arrival completes the
            // operation exactly once.
            TickArrivalConfirmation();
            TickRide();
            // Visual-only: imported wheel nodes follow the native wheel state of the
            // tracked taxi. Never writes to physics; unresolvable links stay static.
            TaxiVisual.TickWheels();
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
            // An exception anywhere in the tick must not leave the managed reverse intent stale
            // (brief §7.3/D14): clear it here, then report once per clean tick.
            SpikeState.Reverse.End();
            SpikeState.NavReDispatchAt = 0f;
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
        SpikeState.NavigationRecovery.ShiftPause(dt);
        SpikeState.Reverse.ShiftPause(dt);
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
        if (SpikeState.NavRetryAt > 0f)
            SpikeState.NavRetryAt += dt;
        if (SpikeState.AutoNextAt > 0f)
            SpikeState.AutoNextAt += dt;
        if (SpikeState.PendingSpawnAt > 0f)
            SpikeState.PendingSpawnAt += dt;
        if (SpikeState.SettleCheckAt > 0f)
            SpikeState.SettleCheckAt += dt;
        if (SpikeState.LastExitAt > 0f)
            SpikeState.LastExitAt += dt;
        if (SpikeState.RideHeartbeatAt > 0f)
            SpikeState.RideHeartbeatAt += dt;
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
        if (TaxiAI.NavigationBlocked) return false;
        if (!SpikeState.PollingActive)
            return false;
        // Never relocate the car under a player who took the wheel (brief §7.4).
        if (PlayerDriver(veh))
        {
            Mod.Log.Warn($"[patrol] {context}: a player is at the wheel — no free-spot relocation.");
            return false;
        }
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

            // Roll diagnosis (log-proven 2026-10-03: constant -2.5 km/h rolling
            // with brakes set). Reversing/brakes/pitch decide slope vs stuck gear.
            string reversing = "n/a";
            string brakeState = "n/a";
            string pitch = "n/a";
            try { reversing = agent.IsReversing.ToString(); } catch { /* cosmetic */ }
            try { brakeState = $"brakes={veh.BrakesApplied} hand={veh.HandbrakeApplied}"; } catch { /* cosmetic */ }
            try { pitch = TaxiDestinations.Num(veh.transform.forward.y); } catch { /* cosmetic */ }

            Mod.Log.Warn(
                $"[probe] {context}: order={SpikeState.NavOrder} frame={Time.frameCount} " +
                $"vehPtr=0x{veh.Pointer.ToInt64():X} agentPtr=0x{agent.Pointer.ToInt64():X} " +
                $"autoDriving1={a1} navCalc1={n1} autoDriving2={a2} navCalc2={n2} " +
                $"reversing={reversing} {brakeState} pitchFwdY={pitch} " +
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
    /// Sampled drive state for recovery decisions; not actuator/physics proof.
    /// ApplyThrottle diagnostics record the final per-consumption inputs separately.
    /// </summary>
    private static string DescribeDriveState(LandVehicle veh, VehicleAgent agent)
    {
        try
        {
            return $" (autoDriving={agent.AutoDriving} agentRev={agent.IsReversing} vehRev={veh.IsReversing} " +
                   $"thr={TaxiDestinations.Num(veh.currentThrottle)} brakes={veh.BrakesApplied} hand={veh.HandbrakeApplied} " +
                   $"speed={TaxiDestinations.Num(veh.Speed_Kmh)} km/h yaw={veh.transform.eulerAngles.y:0.0} " +
                   $"steer={veh.CurrentSteerAngle:0.0} targetSteerNormalized={agent.targetSteerAngle_Normalized:0.00})";
        }
        catch (Exception ex)
        {
            return $" (drive state read failed: {ex.Message})";
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

        bool validOrder = SpikeState.PollingActive || TaxiAI.Active || SpikeState.Reverse.Active ||
                          SpikeState.AutoRunning || _recoveryOwnsNavigation || _selfTestActive ||
                          SpikeState.PendingSpawnCode != null;
        if (validOrder)
        {
            _idleMovingSince = 0f;
            _idleStopAttempts = 0;
            _idleIneffective = false;
            _idleLastSpeed = 0f;
            return;
        }

        // The player alone at the wheel (no NPC driver) is intent, never a ghost.
        if (SpikeCommands.SafePlayerInVehicle(veh) && !SpikeCommands.DriverAtWheel(veh))
        {
            _idleMovingSince = 0f;
            _idleStopAttempts = 0;
            _idleIneffective = false;
            _idleLastSpeed = 0f;
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
            if (_idleIneffective)
                Mod.Log.Info("[guard] recovered — the taxi stands still again.");
            _idleMovingSince = 0f;
            _idleStopAttempts = 0; // standing = the stop worked; fresh budget next time
            _idleIneffective = false;
            _idleLastSpeed = 0f;
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

        // Effectiveness latch (log-proven 2026-10-03: 12 s rolling at -2.5 km/h
        // while the guard re-applied the same useless brakes every second). If
        // the last action did not slow the car, warn once with a handoff and go
        // quiet — repeating it helps nobody. No auto-despawn, ever: destroying
        // under a seated player stays forbidden.
        bool slowing = _idleLastSpeed <= 0f || speed < _idleLastSpeed - IdleSlowEpsilonKmh;
        _idleLastSpeed = speed;
        if (!slowing && !_idleIneffective)
        {
            _idleIneffective = true;
            _idleNextLogAt = now;
            Mod.Log.Warn(
                $"[guard] brake action ineffective — the taxi keeps rolling at {TaxiDestinations.Num(speed)} km/h " +
                $"destination='{SpikeState.RideDestinationName}': press STOP to despawn it or E to get out. Further reminders are throttled.");
        }

        bool loud = !_idleIneffective || now >= _idleNextLogAt;
        if (loud)
        {
            if (_idleIneffective)
                _idleNextLogAt = now + IdleQuietSeconds;
            Mod.Log.Warn(
                $"[guard] the taxi is moving without an active order (speed={TaxiDestinations.Num(speed)} km/h, " +
                $"attempt {_idleStopAttempts}/{IdleGuardMaxAttempts}, destination='{SpikeState.RideDestinationName}') — stopping it.");
            LogAgentProbe("idle guard");
        }
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
            NativeActuatorPatch.RequestPark(veh);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[guard] setting the brakes failed: {ex.Message}");
        }

        if (_idleStopAttempts >= IdleGuardMaxAttempts && !_idleIneffective)
        {
            Mod.Log.Error("[guard] the taxi keeps moving without an order — press STOP to despawn it (the state is not being stopped silently).");
            _idleStopAttempts = 0;
        }
    }

    // Final native inputs are selected at ApplyThrottle, never generic render Update.
    internal static bool HasActuatorOrder => SpikeState.PollingActive || TaxiAI.Active ||
        SpikeState.Reverse.Active || SpikeState.AutoRunning || _recoveryOwnsNavigation || _selfTestActive;

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

        VehicleOwnershipLedger ownership = SpikeState.VehicleOwnership;
        if (ownership.DestroyPending)
            return; // observe only; never race a pending native destroy
        if (ownership.DestroyUnknown)
        {
            Mod.Log.Error("[taxi-spawn] deferred spawn cancelled: the previous destroy result is unknown; retained taxi reference blocks duplicates.");
            SpikeState.CancelPendingSpawn();
            return;
        }
        if (ownership.TrackedPointer != IntPtr.Zero || !ReferenceEquals(SpikeState.Vehicle, null))
            return; // a live/invalid reference must be resolved before another vehicle is spawned

        if (Time.unscaledTime < SpikeState.PendingSpawnAt || Time.frameCount <= SpikeState.PendingSpawnFrame)
            return;

        string code = SpikeState.PendingSpawnCode;
        SpikeState.CancelPendingSpawn();

        Mod.Log.Info($"[freeze-guard] deferred spawn of '{code}' executing on frame {Time.frameCount} — destroy and respawn are now separated.");
        if (!SpikeCommands.SpawnVehicle(code))
            Mod.Log.Error($"[freeze-guard] deferred spawn of '{code}' failed — see the error above (`taxi spawn` can retry once cleanup succeeded).");
    }

    /// <summary>
    /// Shared gate for TaxiApp and diagnostic callers: while an automatic run is
    /// in progress or a deferred respawn is still pending, the request is refused
    /// rather than interrupting the current flow.
    /// </summary>
    /// <param name="caller">The caller name, used in the ignore message.</param>
    /// <returns><c>true</c> when the caller may start its run.</returns>
    internal static bool TryBeginGo(string caller)
    {
        if (!Mod.Lifecycle.AcceptingWork)
            return false; // session teardown: no service work may start after quiesce
        if (SpikeState.AutoRunning)
        {
            Mod.Log.Info($"[taxi-call] {caller} ignored — an automatic taxi run is already running.");
            return false;
        }

        if (SpikeState.PendingSpawnCode != null)
        {
            Mod.Log.Info($"[taxi-call] {caller} ignored — a deferred taxi spawn is still pending.");
            return false;
        }

        return true;
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

        // No vehicle-dependent step of a deferred-respawn run may proceed while
        // the previous vehicle is still being torn down — wait for the pending
        // spawn instead of failing (time margins alone prove no readiness on
        // hitching frames).
        if (SpikeState.AutoStep >= 1 && SpikeState.PendingSpawnCode != null)
            return;

        int step = SpikeState.AutoStep;
        try
        {
            if (step == 1)
            {
                // Spawn's guarded cleanup runs SpikeState.Reset() → ResetAutoRun(),
                // which can flip AutoRunning off mid-run and also clear AutoTarget.
                // Keep both values so a cleanup-and-respawn run retains its target.
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
                Mod.Log.Info($"[taxi-call] step 1/3 done (spawn{(autoToPlayer ? " at the taxi stand" : string.Empty)}) — next: driver.");
            }
            else if (step == 2 && SpikeState.SkipNpcStep)
            {
                Mod.Log.Info($"[taxi-call] diagnostic NPC step skipped — frame={Time.frameCount}.");
                SpikeState.AutoStep = 3;
                SpikeState.AutoNextAt = Time.unscaledTime + AutoStepDelaySeconds;
            }
            else if (step == 2)
            {
                // First log line of the step (review Testbefund 2): if the game freezes
                // here again, this line tells us whether the step ever started at all.
                Mod.Log.Info($"[taxi-call] step 2/3 starting (driver) — frame={Time.frameCount} t={Time.unscaledTime:F1}s.");
                if (!SpikeCommands.Npc())
                    throw new InvalidOperationException("taxi npc reported a failure (see the error above).");

                SpikeState.AutoStep = 3;
                SpikeState.AutoNextAt = Time.unscaledTime + AutoStepDelaySeconds;
                Mod.Log.Info("[taxi-call] step 2/3 done (driver) — next: navigate to player.");
            }
            else if (step == 3)
            {
                // TaxiApp pickup does not drive to a fixed road target, it
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
                Mod.Log.Info($"[taxi-drive] pickup navigation started ({label}) — progress supervision is active.");
            }
            else
            {
                Mod.Log.Warn($"[taxi-call] unknown automatic step {step} — stopping the flow.");
                SpikeState.ResetAutoRun();
            }
        }
        catch (Exception ex)
        {
            string message = SpikeState.ServiceStatus.Length > 0
                ? SpikeState.ServiceStatus
                : $"Taxi dispatch failed at step {step}; please try again later.";
            SpikeState.ResetAutoRun();
            bool cleaned = SpikeCommands.Cleanup("automatic pickup failed", explicitRetry: false);
            SpikeState.SetServiceStatus(cleaned
                ? message
                : message + " Taxi cleanup is pending or uncertain — use STOP and check the log.");
            Mod.Log.Error($"[taxi-call] automatic pickup flow aborted at step {step}: {ex.Message}; cleanupConfirmed={cleaned}");
        }
    }

    // ------------------------------------------------------------- ride kernel

    /// <summary>
    /// Stage 3c ride kernel (per frame, next to <see cref="TickAutoRun"/>): board
    /// detection (<c>RideAwaitingBoard</c> + the game's own E-enter →
    /// <see cref="SpikeCommands.StartRide"/>), the per-frame passenger seat fix, exit
    /// detection (→ <see cref="SpikeCommands.EndRide"/>, re-armed for the next board) and
    /// the arrival verdict (navigation finished → brakes + handbrake, "Arrived — press E to
    /// exit").
    /// </summary>
    internal static void TickRide()
    {
        LandVehicle? veh = SpikeState.Vehicle;
        if (!ReferenceEquals(veh, null) &&
            (!SpikeCommands.IsOwnedTaxi(veh) || !SpikeCommands.HasAuthority()))
            return;

        // Finding 11: settle a provisional boarding first (confirm or fail
        // loudly) — never blocks the ride kernel below.
        SpikeCommands.TickSeatVerify();

        // Board detection: the pickup reached the player (or the last ride ended with an
        // exit) and the player just got in (the game's own E-enter) — start the ride.
        // A manual board (no pickup, no rearm) starts one too while the NPC holds
        // the wheel — but never while a pickup or an automation run owns the car.
        // Paket E: rapid E in/out churn waits out the debounce before a new ride starts.
        bool manualBoard = veh != null && veh.LocalPlayerIsInVehicle &&
            !SpikeState.RideActive && !SpikeState.NavToPlayer && !SpikeState.AutoRunning &&
            SpikeCommands.DriverAtWheel(veh);
        if ((SpikeState.RideAwaitingBoard || manualBoard) && veh != null && veh.LocalPlayerIsInVehicle &&
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

        // Exit detection: covers the game's own E-exit and `taxi out`.
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
        if (!SpikeCommands.EnsurePassengerSeat(veh))
            return;

        // Paket G (2026-09-29): the driver belongs at the wheel every frame too — a
        // driverless taxi never reaches its destination ("der Fahrer ist ausgestiegen").
        SpikeCommands.EnsureDriverSeat(veh);
        if (!SpikeState.RideActive)
            return; // a refused driver re-entry ended/parked the ride

        // Paket A (2026-09-29): waiting for the first destination pick — no arrival
        // verdict yet (without this gate "riding but not dispatched" would read as
        // ARRIVED instantly). The drive starts via ReRoute from the picker/`taxi to`.
        if (SpikeState.RideAwaitingDestination)
            return;

        // The meter: whole dollars per moving in-game minute (0 km/h is free).
        FareMeter.Tick(veh);

        // Arrival: the OPERATION reached an explicit terminal state (brief §6.1/§8.1) — never
        // merely inferred from a native PollingActive flip — and nothing is still in flight, so
        // hold the taxi in place until the player exits.
        if (!SpikeState.RideArrived && SpikeState.Operation.Terminal)
        {
            float dist = Vector3.Distance(veh.transform.position, SpikeState.NavTarget);
            // Final arrival needs target proximity AND a readable, confirmed halt (brief §8.2):
            // distance alone must not complete a taxi that is still moving or reversing, and an
            // unreadable speed (the -1 substitute) is never a standstill. A taxi inside the
            // radius but not yet provably stopped keeps the meter and the active trip open.
            bool speedReadable = TryReadSignedSpeed(veh, out float signedSpeed);
            bool reversing = SpikeState.Reverse.Active;
            if (!TryReadReversing(veh, out bool nativeReversing)) reversing = true;
            else reversing |= nativeReversing;
            if (!NavigationArrivalPolicy.IsConfirmedArrival(dist, speedReadable, signedSpeed, reversing) &&
                dist <= ArrivalThresholdMeters)
            {
                TaxiLog.Verbose(
                    $"[ride] within the {ArrivalThresholdMeters:F0} m radius but not provably stopped " +
                    $"(readable={speedReadable}, speed={TaxiDestinations.Num(signedSpeed)} km/h, reversing={reversing}) — holding the arrival verdict.");
                return;
            }

            SpikeState.RideArrived = true;
            // Store the completed-destination logical identity (name+kind+goal+route) so only an
            // EXACT re-dispatch may later be skipped (brief §8.2); a merely nearby fresh target is
            // a current order.
            if (!SpikeState.Operation.Failed)
            {
                SpikeState.CompletedDestinationName = SpikeState.RideDestinationName;
                SpikeState.CompletedDestinationKind = SpikeState.RideDestinationKind;
                SpikeState.CompletedGoal = SpikeState.RideGoal;
                SpikeState.CompletedArrival = SpikeState.ActiveTripPoint;
            }
            try
            {
                NativeActuatorPatch.RequestPark(veh);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[ride] brakes/handbrake on arrival failed: {ex.Message}");
            }

            // Honest verdict (bug1, 2026-09-29): the ride used to announce ARRIVED even
            // 152.9 m off when the callback came back Failed - RideArrived only means
            // "driving done", but every consumer read it as "arrived". Any end beyond the
            // threshold (or with a Failed callback) now counts as "cannot get there".
            string endReason = NavigationEndReason.ForRide(SpikeState.NavGaveUp, dist,
                ArrivalThresholdMeters, SpikeState.NavCallbackResult);
            if (endReason != "arrived")
            {
                SpikeState.NavGaveUp = true;
            }
            bool gaveUp = SpikeState.NavGaveUp;
            // Resolve the verdict first; isolate meter IO without reporting arrival on failure.
            CleanupGuard.Run("FareMeter.Stop(ride end)", () => FareMeter.Stop(endReason), SpikeCommands.ReportCleanupFailure);
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
        // Never re-seat a driver or reverse under a player who took the wheel (brief §6.3/§7.4).
        if (PlayerDriver(veh))
        {
            Mod.Log.Warn("[nav] recovery refused: a player is at the wheel — no autonomous driver/reverse action.");
            return;
        }
        if (!TryBeginRecovery(agent, veh, maneuverPlanned: true)) return;
        SpikeState.StuckRecoveries++;

        // Paket G (2026-09-29): a driverless car never makes progress — re-boarding the
        // driver beats burning recovery #1/#2 on navigation re-dispatches into the void.
        if (SpikeCommands.EnsureDriverSeat(veh))
        {
            if (!SpikeState.PollingActive) return;
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

        // Later stalls back the car out before re-dispatching: a wedge against an obstacle
        // is exactly what reverse fixes, and recovery #1 was the only place that reversed.
        // The operation budget was committed above, before any reverse/rescue return.
        ArmProgressWindow(veh);
        if (TryStartReverse(agent, veh, Time.unscaledTime, $"recovery #{SpikeState.StuckRecoveries}"))
            return;

        Mod.Log.Warn($"[nav] recovery #{SpikeState.StuckRecoveries}: re-dispatching within the operation budget.");
        ReDispatch(agent, SpikeState.NavTarget, $"recovery #{SpikeState.StuckRecoveries}");
    }

    /// <summary>
    /// True when a short ray behind the car (at bumper height, from the rear axle
    /// line) hits no solid. Triggers are ignored; the car's own colliders are skipped
    /// by starting the ray past the rear bumper.
    /// <para>
    /// LIMITATION (review D2): this is a single coarse rear ray, not a full-body sweep. It is a
    /// fail-closed obstacle probe used for the bounded reverse grant — it does NOT prove the whole
    /// car body is clear, and it is deliberately not claimed as full body safety. A blocked or
    /// unreadable probe only ever REVOKES the grant.
    /// </para>
    /// </summary>
    private static bool RearIsClear(LandVehicle veh)
    {
        return !RoadKeeper.HasObstacle(veh, veh.transform.position, veh.transform.rotation,
            rear: true, distance: ReverseClearanceMeters);
    }

    private static bool TryStartReverse(VehicleAgent agent, LandVehicle veh, float now, string reason)
    {
        if (TaxiAI.NavigationBlocked) return false;
        // Start conditions (brief §6.3): a current ACTIVE operation, live host authority, the exact
        // owned living taxi, a healthy consumer hook and no player at the wheel.
        if (!SpikeState.PollingActive || !SpikeState.Operation.Active || !SpikeCommands.HasAuthority() ||
            !SpikeCommands.IsOwnedTaxi(veh))
        {
            Mod.Log.Warn($"[reverse] {reason}: refused — no current operation, host authority or owned taxi.");
            return false;
        }
        if (!NativeActuatorPatch.Healthy)
        {
            Mod.Log.Warn($"[reverse] {reason}: refused — the actuator consumer hook is unhealthy.");
            return false;
        }
        // A real player at the wheel always prevents new autonomous input. Checked BEFORE any
        // driver re-seat so the mod never mutates a player-occupied seat (brief §6.3/§7.4).
        if (PlayerDriver(veh))
        {
            Mod.Log.Warn($"[reverse] {reason}: player at the wheel — no autonomous reverse.");
            return false;
        }
        // EnsureDriverSeat true means it HANDLED a missing/aborted driver, NOT that one is ready:
        // re-verify the intended NPC actually holds THIS wheel (and no player appeared) before reversing.
        if (SpikeCommands.EnsureDriverSeat(veh))
        {
            if (!SpikeState.PollingActive) return false; // driver safety stopped this recovery
            if (PlayerDriver(veh) || !SpikeCommands.DriverAtWheel(veh))
            {
                Mod.Log.Warn($"[reverse] {reason}: driver not confirmed at the wheel after recovery — no reverse.");
                return false;
            }
        }
        // DriverAtWheel true is not sufficient if a player driver is present simultaneously.
        if (PlayerDriver(veh) || !SpikeCommands.DriverAtWheel(veh))
        {
            Mod.Log.Warn($"[reverse] {reason}: the intended driver does not exclusively hold the wheel — no reverse.");
            return false;
        }
        if (!RearIsClear(veh))
        {
            Mod.Log.Warn($"[reverse] {reason}: rear blocked; no reverse.");
            return false;
        }

        // Consume the attempt and capture operation/order/pointer/origin/direction BEFORE the
        // native call, so an error or a reentrant callback cannot buy a free retry (brief §6.2).
        _reverseOrigin = veh.transform.position;
        _reverseForward = veh.transform.forward;
        _reverseAttempt = Math.Min(_reverseAttempt + 1, 2);
        float seconds = _reverseAttempt >= 2 ? ReverseSecondsLong : ReverseSeconds;
        int order = SpikeState.NavOrder;
        long operation = SpikeState.OperationId;
        IntPtr pointer = veh.Pointer;
        Vector3 reverseDir = -veh.transform.forward;

        _recoveryOwnsNavigation = true;
        try
        {
            // Deactivate can eject the NPC — re-seat is handled above. Never leave the
            // car parked while a reverse is requested.
            NativeActuatorPatch.ReleasePark(veh);
            // The native request window (1.5 s / 3 s) is captured IN the policy so the consumer
            // enforces the SMALLER of it and the overall cap (brief §7.3).
            SpikeState.Reverse.Begin(operation, order, ToNumerics(_reverseOrigin), ToNumerics(reverseDir), now, seconds);
            _reverseStartedAt = now;
            _reverseRearCheckAt = now;
            SpikeState.NavReDispatchAt = now + seconds;
            SpikeState.NavReDispatchTarget = SpikeState.NavTarget;
            SpikeState.PollingActive = true;

            agent.StartReverse();

            // Re-check AFTER the native call AND after any boarding side effect: the operation,
            // order, vehicle pointer, polling, lifecycle and driver must be unchanged (brief §6.3).
            if (!SpikeState.Reverse.Matches(operation, order) || !SpikeState.PollingActive ||
                !SpikeState.Operation.Active || !ReferenceEquals(SpikeState.Vehicle, veh) ||
                veh.Pointer != pointer || PlayerDriver(veh) || !SpikeCommands.DriverAtWheel(veh))
            {
                Mod.Log.Warn($"[reverse] {reason}: native StartReverse was superseded by STOP/reroute/error/driver change — revoking.");
                RevokeReverse("post-StartReverse superseded");
                return false;
            }
            Mod.Log.Warn(
                $"[reverse] {reason}: requested {seconds:0.0}s (attempt {_reverseAttempt}, op {operation}, order {order}), rear clear; then redispatch to {SpikeCommands.Fmt(SpikeState.NavTarget)}" +
                $"{DescribeDriveState(veh, agent)}.");
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[reverse] {reason}: StartReverse failed: {ex.Message}");
            RevokeReverse("StartReverse threw");
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

        // A player who took the wheel gets no autonomous startup rescue (brief §7.4).
        if (PlayerDriver(veh))
            return true;

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

        if (!TryBeginRecovery(agent, veh, maneuverPlanned: true)) return true;
        _startupArmed = false; // one startup rescue per dispatch, not a teleport loop
        _recoveryOwnsNavigation = true;
        try { TaxiAI.StopPatrol("startup stuck"); }
        finally { _recoveryOwnsNavigation = false; }
        if (SpikeCommands.EnsureDriverSeat(veh))
        {
            if (!SpikeState.PollingActive) return true;
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
        // Readability is separate from value (brief §8.2): a failed read keeps the -1
        // substitute, which must never be mistaken for a standstill.
        bool speedReadable = TryReadSignedSpeed(veh, out float speed);
        bool reversing = SpikeState.Reverse.Active;
        // An unreadable native reverse flag fails CLOSED (brief §8.2): it never proves a halt.
        if (!TryReadReversing(veh, out bool nativeReversing))
            reversing = true;
        else
            reversing |= nativeReversing;

        int waypoint = TaxiAI.CurrentWaypoint;
        int waypoints = TaxiAI.WaypointCount;
        int last = Math.Max(0, waypoints - 1);

        bool atLastWaypoint = waypoints > 0 && waypoint >= last;
        bool nearTarget = NavigationArrivalPolicy.IsWithinArrival(distance);

        // The single supervised arrival decision (brief §8.2): proximity OR the last waypoint,
        // but ALWAYS a readable, confirmed halt with no reverse, plus current-operation evidence
        // (a real drive start in THIS operation) — a car parked inside the radius from before
        // must not be completed by proximity alone.
        bool arrivalConfirmed = SupervisedArrivalPolicy.IsConfirmedArrival(
            currentRunComplete: ArrivalConfirmationPolicy.IsCurrent(
                SpikeState.NavCallbackArrivalPending, SpikeState.NavCallbackArrivalOrder,
                SpikeState.NavCallbackArrivalOperation, SpikeState.NavOrder, SpikeState.OperationId),
            operationProgress: _driveStartConfirmed,
            distance: distance, speedReadable: speedReadable, signedSpeed: speed, reversing: reversing);

        if (arrivalConfirmed)
        {
            Mod.Log.Info(
                $"[patrol] ARRIVED at {SpikeState.RideDestinationName} — waypoint {waypoint}/{last}, " +
                $"{TaxiDestinations.Num(distance)} m to the target, speed={TaxiDestinations.Num(speed)} km/h " +
                $"({(nearTarget ? $"within the {ArrivalThresholdMeters:F0} m threshold" : "standing on the last waypoint")}).");
            _selfTestActive = false;
            TaxiAI.StopPatrol("arrived");
            SpikeState.PollingActive = false;
            SpikeState.Operation.Terminate("patrol arrived", failed: false);
            return;
        }

        // Inside the radius but not yet provably stopped (or no current-operation evidence):
        // hold the verdict rather than completing a moving/parked-from-before taxi.
        if (!reversing && nearTarget && !arrivalConfirmed)
        {
            TaxiLog.Verbose(
                $"[patrol] within the {ArrivalThresholdMeters:F0} m radius but not a confirmed arrival " +
                $"(readable={speedReadable}, speed={TaxiDestinations.Num(speed)} km/h, reversing={reversing}, " +
                $"progress={_driveStartConfirmed}) — holding the verdict.");
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
                if (!TryBeginRecovery(veh.Agent, veh, maneuverPlanned: false)) return;
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

        // Matching unconfirmed starts retain ownership through the full fixed grace.
        // Arrival and the operation deadline are checked before this gate; drive-start
        // proof is sampled by polling before patrol supervision, even while waiting.
        if (PatrolStartPolicy.ShouldWait(_selfTestActive, _selfTestOrder, SpikeState.NavOrder,
            _driveStartConfirmed, now - _selfTestAt, DriveSelfTestSeconds))
            return;

        // ---- running stall watchdog (review point 2) ------------------------------
        // Progress needs an average speed over the window or a net approach; a crawl
        // can no longer reset the window, and navCalc only excuses its grace cap.
        if (EvaluateProgress(position, distance, now))
        {
            if (_localRecoveryProgress)
            {
                _localRecoveryProgress = false;
                if (SpikeState.StuckRecoveries > 0)
                    TaxiLog.Verbose($"[patrol] fresh forward approach >= {RecoveryRecurrencePolicy.ForwardApproachMeters:F0} m — clearing local recovery history.");
                SpikeState.StuckRecoveries = 0;
                _reverseAttempt = 0;
            }
            return;
        }

        if (navCalc && _navCalcSince > 0f && now - _navCalcSince < NavCalcGraceSeconds)
            return; // calculation still inside its grace cap — not a stall yet

        bool staticObstacle = RoadKeeper.HasObstacle(veh, position, veh.transform.rotation, staticOnly: true);
        float stallWindow = staticObstacle ? StaticObstacleStuckSeconds : StuckWindowSeconds;
        if (now - SpikeState.ProgressWindowStart < stallWindow)
            return;

        if (!TryBeginRecovery(veh.Agent, veh, maneuverPlanned: true)) return;
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
        LandVehicle? veh = SpikeState.Vehicle;
        // Capture native reverse/steering/speed BEFORE our own cancellation.
        string driveState = veh == null ? string.Empty : DescribeDriveState(veh, agent);
        // Capture the DIRECTED measurement and the front clearance. The forward gate uses the
        // CURRENT directed net, not a best-ever high-water mark a retrace could fake (brief §7.1).
        float netReverse = veh == null ? 0f : SpikeState.Reverse.DirectedNetReverseMeters(ToNumerics(veh.transform.position));
        bool moved = SpikeState.Reverse.MovedConfirmed;
        bool frontBlocked = veh != null &&
            RoadKeeper.HasObstacle(veh, veh.transform.position, veh.transform.rotation);
        bool forwardOk = veh != null && SpikeState.Reverse.CanDispatchForward(netReverse, frontBlocked);
        long operation = SpikeState.OperationId;
        IntPtr pointer = veh == null ? IntPtr.Zero : veh.Pointer;

        // Revoke the managed reverse intent FIRST and enter the bounded transition (D15): the
        // consumer can no longer authorize reverse input while we stop natively (brief §7.4/§7.5).
        SpikeState.Reverse.BeginTransition();
        SpikeState.NavReDispatchAt = 0f;
        // Invalidate the old native callback identity BEFORE the native stop, so a synchronous
        // Stopped callback of the old order cannot act on the new transition (brief §7.4/§8.1).
        SpikeState.NextNavOrder();

        // Controlled end of the reverse (brief §7.4/§7.5): the managed intent is revoked FIRST
        // (above), then the native stop and the independent fallback cleanup run as isolated,
        // re-validated steps. A returned StopReversing is only AUTHORIZATION for the next checked
        // transition, never proof of a physical halt.
        LandVehicle? stopVeh = veh;
        IntPtr stopPointer = pointer;
        long stopOperation = operation;
        ReverseStopTransitionResult transition;
        try
        {
            transition = ReverseStopTransitionPolicy.Run(
                stopPermitted: () => stopVeh != null && stopVeh.Pointer != IntPtr.Zero && stopPointer == stopVeh.Pointer &&
                    ReverseSupervisionPolicy.NativeCleanupPermitted(
                        SpikeCommands.HasAuthority(), SpikeCommands.IsOwnedTaxi(stopVeh), PlayerDriver(stopVeh)),
                // The captured, ownership-validated agent is the target of the stop.
                stopNative: () => { agent.StopReversing(); },
                stillCurrentAndPermitted: () =>
                    SpikeState.OperationId == stopOperation && SpikeState.Operation.Active &&
                    ReferenceEquals(SpikeState.Vehicle, stopVeh) &&
                    stopVeh != null && stopVeh.Pointer == stopPointer &&
                    ReverseSupervisionPolicy.NativeCleanupPermitted(
                        SpikeCommands.HasAuthority(), SpikeCommands.IsOwnedTaxi(stopVeh), PlayerDriver(stopVeh)),
                fallbackStopNavigating: () => stopVeh!.Agent?.StopNavigating(),
                fallbackPark: () => NativeActuatorPatch.RequestPark(stopVeh!),
                onStepError: ex => SpikeCommands.ReportCleanupFailure("reverse stop transition", ex));
        }
        finally
        {
            SpikeState.Reverse.End();
        }

        // Recheck the captured OPERATION and vehicle after the native stop: STOP, a reroute or a
        // driver change may have replaced the manoeuvre (brief §6.3). No stale stack redispatch.
        if (SpikeState.OperationId != operation || !ReferenceEquals(SpikeState.Vehicle, veh) ||
            (veh != null && veh.Pointer != pointer))
        {
            Mod.Log.Warn("[reverse] redispatch abandoned: the operation or vehicle was replaced during the native stop.");
            return;
        }
        if (!SpikeState.PollingActive || SpikeState.Operation.Terminal)
            return;
        if (veh == null)
            return;

        // A native stop that was not permitted, threw, or was followed by a STOP/reroute or an
        // authority/player change authorizes NO follow-up (brief §7.4/§7.5): no retry, no rescue,
        // no forward dispatch. Terminalize THIS operation with a clear failed-stop reason; the
        // meter settles exactly once through the existing terminal.
        if (!transition.FollowupAllowed)
        {
            Mod.Log.Warn(
                $"[reverse] the native reverse stop did not complete safely " +
                $"(permitted={transition.StopPermitted}, issued={transition.StopIssued}, threw={transition.StopThrew}, " +
                $"superseded={transition.Superseded}) — no retry, rescue or forward dispatch; ending the operation{driveState}.");
            SpikeState.PollingActive = false;
            SpikeState.Operation.Terminate("reverse stop failed", failed: true);
            return;
        }

        float displacement = Vector3.Distance(veh.transform.position, _reverseOrigin);
        float reverseSeconds = Time.unscaledTime - _reverseStartedAt;

        // No SUSTAINED DIRECTED reverse motion: lateral slip, height change or jitter is not
        // escape. Bounded retry (rear re-checked inside TryStartReverse), then a checked rescue,
        // then the terminal abort — NEVER a blind forward dispatch (brief §7.1/§7.5).
        if (!moved)
        {
            Mod.Log.Warn(
                $"[reverse] attempt {_reverseAttempt} produced no sustained directed reverse " +
                $"(moved {TaxiDestinations.Num(displacement)} m in {TaxiDestinations.Num(reverseSeconds)} s 3D, " +
                $"{TaxiDestinations.Num(netReverse)} m net along the reverse axis){driveState} — escalating, no forward dispatch.");
            if (_reverseAttempt < 2 && TryStartReverse(agent, veh, Time.unscaledTime, "reverse ineffective"))
                return;
            if (TryFreeSpotRescue(veh, agent, "reverse ineffective"))
            {
                ReDispatch(agent, target, "post-reverse free-spot rescue");
                return;
            }
            GiveUpNavigation(agent, veh.transform.position,
                Vector3.Distance(veh.transform.position, target), Time.unscaledTime - SpikeState.NavStartTime);
            return;
        }

        // Directed motion confirmed, but the front is still blocked (or the CURRENT reverse is not
        // meaningful): 0.6 m plus a blocked front is not escape (brief §7.1/§7.5). Bounded retry,
        // then a checked rescue, then a terminal abort — never an unverified forward ping-pong.
        if (!forwardOk)
        {
            Mod.Log.Warn(
                $"[reverse] backed {TaxiDestinations.Num(netReverse)} m but the front is still blocked — " +
                $"escape is unproven; no unverified forward dispatch (bounded retry){driveState}.");
            if (_reverseAttempt < 2 && TryStartReverse(agent, veh, Time.unscaledTime, "front still blocked"))
                return;
            if (TryFreeSpotRescue(veh, agent, "front still blocked"))
            {
                ReDispatch(agent, target, "post-reverse free-spot rescue (front blocked)");
                return;
            }
            GiveUpNavigation(agent, veh.transform.position,
                Vector3.Distance(veh.transform.position, target), Time.unscaledTime - SpikeState.NavStartTime);
            return;
        }

        // Displacement proves actuation, not escape. Keep the attempt history until genuinely
        // new forward approach or a new operation proves recovery.
        Mod.Log.Info(
            $"[reverse] directed reverse {TaxiDestinations.Num(netReverse)} m confirmed in {TaxiDestinations.Num(reverseSeconds)} s, front clear" +
            $"{driveState} — re-dispatching the route.");
        ReDispatch(agent, target, "post-reverse");
    }

    /// <summary>Fresh <c>Navigate</c> dispatch with settings=null and a new measurement window.</summary>
    internal static void ReDispatch(VehicleAgent agent, Vector3 target, string why)
    {
        if (TaxiAI.NavigationBlocked) return;
        if (!SpikeState.PollingActive)
            return; // a driver safety abort cannot be undone by an older recovery stack
        // Never issue a fresh autonomous navigation against a player who took the wheel (brief §7.4).
        LandVehicle? pd = SpikeState.Vehicle;
        if (pd != null && PlayerDriver(pd))
        {
            Mod.Log.Warn($"[nav] {why}: a player is at the wheel — no autonomous re-dispatch.");
            return;
        }
        float now = Time.unscaledTime;
        if (StopIfNavigationExpired(agent, SpikeState.Vehicle, now)) return;
        SpikeState.PollingActive = true;
        SpikeState.NavTarget = target;
        SpikeState.NavStartTime = now;
        SpikeState.NavEverAutoDriving = false;
        SpikeState.NavCallbackResult = null;
        SpikeState.NavRetried = true;
        SpikeState.NavRetryAt = now;

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
                NativeActuatorPatch.ReleasePark(av);
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
    /// Terminal relocation: the re-dispatch ladder failed and the car is still
    /// stuck (typically no meaningful forward progress left). Relocates the car
    /// onto the live route polyline (RoadKeeper.TryRescueStartup — supported free
    /// spot, ground snap, oriented-box clearance, occupancy check, road-aligned
    /// rotation), then re-dispatches. Last resort before giving up. A car on the
    /// vehicle graph can still be unable to reach its route (2026-10-15 log:
    /// give-up 106.7 m from target while on graph), so graph state never vetoes
    /// the attempt; only the spent one-shot rescue budget does (TerminalRescuePolicy).
    /// </summary>
    private static bool TryTeleportRecovery(LandVehicle veh, VehicleAgent agent, Vector3 position, float distance)
    {
        if (TaxiAI.NavigationBlocked) return false;
        bool lastRescueAvailable = SpikeState.NavigationRecovery.TryLastRescue(Time.unscaledTime);
        bool onGraph = false;
        try
        {
            onGraph = agent.IsOnVehicleGraph();
        }
        catch (Exception)
        {
            // unknown — try the rescue anyway
        }

        if (TerminalRescuePolicy.Decide(onGraph, lastRescueAvailable) == TerminalRescuePolicy.Action.GiveUp)
        {
            Mod.Log.Warn(
                $"[nav] recovery: rescue budget spent — giving up at {SpikeCommands.Fmt(position)} " +
                $"({TaxiDestinations.Num(distance)} m to target).");
            return false;
        }

        Mod.Log.Warn(
            $"[nav] recovery: car is {(onGraph ? "on the vehicle graph but stuck" : "OFF the vehicle graph")} at {SpikeCommands.Fmt(position)} " +
            $"({TaxiDestinations.Num(distance)} m to target) — attempting teleport recovery.");

        if (!RoadKeeper.TryRescueStartup(veh, agent, "teleport recovery"))
        {
            Mod.Log.Warn("[nav] recovery: teleport recovery failed — no safe spot found on the route.");
            return false;
        }

        Mod.Log.Warn("[nav] recovery: teleport recovery succeeded — re-dispatching from the new position.");
        ArmProgressWindow(veh);
        ReDispatch(agent, SpikeState.NavTarget, "teleport recovery");
        return true;
    }

    /// <summary>
    /// Terminal of the recovery ladder — never leaves the player in a taxi that
    /// silently does nothing: polling off, brakes on, and an actionable message.
    /// </summary>
    private static void GiveUpNavigation(VehicleAgent agent, Vector3 position, float distance, float elapsed)
    {
        SpikeState.PollingActive = false;
        SpikeState.NavGaveUp = true;
        // Explicit terminal verdict (brief §6.1/§8.1): the operation is finished, never inferred
        // from a native flag.
        SpikeState.Operation.Terminate("navigation gave up", failed: true);
        SpikeState.NextNavOrder(); // invalidate in-flight orders (review point 4)
        SpikeState.Reverse.End();  // revoke the managed reverse intent first (brief §7.4)
        SpikeState.NavReDispatchAt = 0f; // a pending reverse must not re-dispatch after the give-up
        CleanupGuard.Run("StopPatrol(give up)", () => TaxiAI.StopPatrol("navigation gave up"), SpikeCommands.ReportCleanupFailure);

        Mod.Log.Error(
            $"[nav] GAVE UP after {TaxiDestinations.Num(elapsed)} s into the current dispatch and {SpikeState.NavigationRecovery.Attempts} operation recovery episode(s) — " +
            $"navigation ended before reaching the target ({TaxiDestinations.Num(distance)} m left to {SpikeCommands.Fmt(SpikeState.NavTarget)}, " +
            $"position {SpikeCommands.Fmt(position)}). Press STOP to despawn the taxi, or E (`taxi out`) to get out.");

        // Bug 2026-10-03: the car kept rolling backwards after the give-up. A reverse left
        // engaged keeps driving after StopNavigating, so the reverse gear is released first.
        // New native input is only issued for the exact own live taxi with no actual player
        // driver (brief §7.4); a player takeover gets managed cancellation only. Each step is
        // independently guarded so a throw in the first never skips the rest.
        LandVehicle? veh = SpikeState.Vehicle;
        bool ownedAlive = veh != null && veh.Pointer != IntPtr.Zero && SpikeCommands.IsOwnedTaxi(veh);
        bool nativeOk = ownedAlive && ReverseSupervisionPolicy.NativeCleanupPermitted(
            SpikeCommands.HasAuthority(), ownedAlive, PlayerDriver(veh));
        if (!nativeOk)
        {
            Mod.Log.Warn("[nav] give-up: no current authority/ownership or a player is at the wheel — managed cancellation only, no new native input.");
            return;
        }

        CleanupGuard.Run("give-up StopReversing", () =>
        {
            veh!.Agent?.StopReversing();
        }, SpikeCommands.ReportCleanupFailure);
        CleanupGuard.Run("give-up StopNavigating", () =>
        {
            veh!.Agent?.StopNavigating();
        }, SpikeCommands.ReportCleanupFailure);
        CleanupGuard.Run("give-up park", () =>
        {
            NativeActuatorPatch.RequestPark(veh!);
        }, SpikeCommands.ReportCleanupFailure);
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
        if (!NavigationCallbackPolicy.ShouldAcceptResult(
            order, SpikeState.NavOrder, SpikeState.PollingActive))
        {
            if (order != SpikeState.NavOrder)
                TaxiLog.Verbose($"[nav] stale callback order={order} (current={SpikeState.NavOrder}) ignored — no state changed.");
            else
                TaxiLog.Verbose($"[nav] callback order={order} ignored because navigation is no longer active.");
            return;
        }

        if (_recoveryOwnsNavigation || SpikeState.Reverse.Active)
        {
            TaxiLog.Verbose($"[patrol] old navigation callback {result} ignored during controlled recovery.");
            return;
        }
        string text = result.ToString();
        SpikeState.NavCallbackResult = text;
        // Queue the result for the supervisor; NEVER do native work inside the native callback
        // (brief §8.1): a synchronous Stopped callback from our own StopNavigating must not recurse
        // into another stop/dispatch. The queue is order+operation aware (review D12/D16).
        SpikeState.NavCallbackQueuedOrder = order;
        SpikeState.NavCallbackQueuedResult = text;
        SpikeState.NavCallbackQueuedOperation = SpikeState.OperationId;
        TaxiLog.Verbose(
            $"[nav] callback result={text} (frame={Time.frameCount}, order={order}, op={SpikeState.OperationId}) queued for supervision.");
    }

    /// <summary>
    /// Drains a queued navigation callback BEFORE ride finalization (brief §8.1). All native work
    /// happens here on the normal Update path — never inside the native callback. A stale entry
    /// (new order/operation, or navigation no longer active) is dropped without touching state.
    /// </summary>
    private static void TickNavigationCallback()
    {
        string? result = SpikeState.NavCallbackQueuedResult;
        if (result == null) return;
        int order = SpikeState.NavCallbackQueuedOrder;
        long operation = SpikeState.NavCallbackQueuedOperation;
        SpikeState.NavCallbackQueuedResult = null;
        SpikeState.NavCallbackQueuedOrder = -1;

        if (order != SpikeState.NavOrder || operation != SpikeState.OperationId ||
            !SpikeState.PollingActive || !SpikeState.Operation.Active)
        {
            TaxiLog.Verbose($"[nav] stale queued callback result={result} dropped — no state changed.");
            return;
        }

        float elapsed = SpikeState.NavStartTime > 0f ? Time.unscaledTime - SpikeState.NavStartTime : -1f;
        NavigationOutcomeAction outcome = NavigationCallbackOutcomePolicy.Decide(
            result, SpikeState.NavToPlayer, SpikeState.PollingActive, _recoveryOwnsNavigation);
        switch (outcome)
        {
            case NavigationOutcomeAction.Ignore:
                TaxiLog.Verbose($"[nav] queued callback result={result} ignored (controlled transition or no-op).");
                return;

            case NavigationOutcomeAction.BoundedRecovery:
                // One bounded recovery decision; the ride and the meter stay alive (brief §8.1).
                SpikeState.Operation.EnterRecovery();
                SpikeState.NavCallbackRecoveryPending = true;
                SpikeState.NavCallbackRecoveryOrder = order;
                SpikeState.NavCallbackRecoveryOperation = operation;
                TaxiLog.Verbose(
                    $"[nav] callback result={result} after {elapsed:F1}s (order={order}) — recoverable failure handed to supervision (no premature ride end).");
                return;

            case NavigationOutcomeAction.ExplicitFailure:
                SpikeState.PollingActive = false;
                SpikeState.NavGaveUp = true;
                SpikeState.Operation.Terminate("navigation stopped unexpectedly", failed: true);
                Mod.Log.Warn($"[nav] unexpected Stopped callback after {elapsed:F1}s (order={order}) — explicit failure, no automatic restart.");
                return;

            case NavigationOutcomeAction.ConfirmArrival:
                // A current Complete is EVIDENCE for this operation's arrival, never the arrival
                // itself (brief §8.2): record it and keep the operation ACTIVE so supervision can
                // confirm a readable, confirmed halt inside the target radius first.
                RecordPendingArrival(order, operation);
                TaxiLog.Verbose($"[nav] callback result=Complete after {elapsed:F1}s (order={order}) — arrival evidence recorded; awaiting a confirmed halt.");
                return;

            default: // Finalize — a pickup Complete keeps its own (now supervised) arrival handling.
                // The pickup Complete is the SAME arrival evidence, never an immediate board (brief
                // §8.2): the player is only reached once the taxi provably stops at the target, so
                // keep the operation active and let supervision confirm the halt.
                RecordPendingArrival(order, operation);
                TaxiLog.Verbose($"[nav] callback result={result} after {elapsed:F1}s (order={order}) — arrival evidence recorded; awaiting a confirmed halt.");
                return;
        }
    }

    /// <summary>
    /// Records a current navigation <c>Complete</c> as bounded arrival EVIDENCE for one order and
    /// operation (brief §8.2). It does NOT publish a terminal state, arm the board or stop the
    /// meter: only a confirmed halt inside the target radius does (see
    /// <see cref="TickArrivalConfirmation"/>).
    /// </summary>
    private static void RecordPendingArrival(int order, long operation)
    {
        SpikeState.NavCallbackArrivalPending = true;
        SpikeState.NavCallbackArrivalOrder = order;
        SpikeState.NavCallbackArrivalOperation = operation;
        SpikeState.NavCallbackArrivalAt = Time.unscaledTime;
    }

    // ---------------------------------------------------------- nav polling

    /// <summary>
    /// 0.5 s heartbeat while <see cref="SpikeState.PollingActive"/>: logs
    /// AutoDriving / navCalc / distance / graph / stuck / speed / callback,
    /// fires the one <c>settings = null</c> retry, applies the no-progress deadline and
    /// prints the terminal verdict (ARRIVED within
    /// <see cref="ArrivalThresholdMeters"/> vs. STOPPED SHORT).
    /// </summary>
    private static void TickNavigationPolling()
    {
        if (_trafficWaiting) return;
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
            RevokeReverse("polling: vehicle became null");
            SpikeState.PollingActive = false;
            SpikeState.Operation.Terminate("vehicle lost", failed: true);
            return;
        }

        VehicleAgent? agent = veh.Agent;
        if (agent == null)
        {
            Mod.Log.Warn("[nav] LandVehicle.Agent became null — stopping navigation polling.");
            RevokeReverse("polling: agent became null");
            SpikeState.PollingActive = false;
            SpikeState.Operation.Terminate("agent lost", failed: true);
            return;
        }

        float now = Time.unscaledTime;
        float elapsed = now - SpikeState.NavStartTime;
        if (StopIfNavigationExpired(agent, veh, now)) return;
        if (TaxiAI.NavigationBlocked)
        {
            TaxiAI.StopPatrol("pending polling release");
            if (TaxiAI.NavigationBlocked) return;
        }
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

            // A current recoverable navigation failure handed over by the callback: run exactly
            // ONE bounded recovery decision, keeping the ride and the meter alive (brief §8.1).
            // The mailbox is order/operation aware (review D12): a stale entry is dropped.
            if (SpikeState.NavCallbackRecoveryPending)
            {
                if (SpikeState.NavCallbackRecoveryOrder != SpikeState.NavOrder ||
                    SpikeState.NavCallbackRecoveryOperation != SpikeState.OperationId ||
                    !SpikeState.Operation.Active)
                {
                    SpikeState.NavCallbackRecoveryPending = false;
                    SpikeState.NavCallbackRecoveryOrder = -1;
                    SpikeState.NavCallbackRecoveryOperation = -1;
                    TaxiLog.Verbose("[nav] stale recovery decision dropped (order/operation changed) — no state changed.");
                }
                else
                {
                    SpikeState.NavCallbackRecoveryPending = false;
                    SpikeState.NavCallbackRecoveryOrder = -1;
                    SpikeState.NavCallbackRecoveryOperation = -1;
                    RecoverStuck(agent, veh, position, distance, elapsed, autoDriving);
                    return;
                }
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
                if (_localRecoveryProgress)
                {
                    _localRecoveryProgress = false;
                    if (SpikeState.StuckRecoveries > 0)
                        TaxiLog.Verbose($"[nav] fresh forward approach >= {RecoveryRecurrencePolicy.ForwardApproachMeters:F0} m — clearing local recovery history.");
                    SpikeState.StuckRecoveries = 0;
                    _reverseAttempt = 0;
                }
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
            bool arrivedWithin = distance <= ArrivalThresholdMeters;
            if (arrivedWithin)
            {
                // Never claim success while the taxi is still moving or reversing (brief §8.2):
                // hold the terminal verdict until a readable, confirmed halt is observed.
                bool readable = TryReadSignedSpeed(veh, out float signedSpeed);
                bool reversing = SpikeState.Reverse.Active;
                if (!TryReadReversing(veh, out bool nativeReversing)) reversing = true;
                else reversing |= nativeReversing;
                if (!NavigationArrivalPolicy.IsConfirmedHalt(readable, signedSpeed, reversing))
                {
                    TaxiLog.Verbose(
                        $"[nav] within the {ArrivalThresholdMeters:F0} m radius but not a confirmed halt " +
                        $"(readable={readable}, speed={TaxiDestinations.Num(signedSpeed)} km/h, reversing={reversing}) — holding the terminal verdict.");
                    return;
                }
            }
            SpikeState.PollingActive = false;
            SpikeState.Operation.Terminate(arrivedWithin ? "arrived" : "stopped short", failed: !arrivedWithin);
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
            // A single getter error must not become an inferred terminal next frame (review D13):
            // revoke the managed reverse NOW and queue ONE bounded recovery. The operation budget
            // bounds repeated errors; with no budget left it fails explicitly.
            RevokeReverse("polling getter error");
            if (!SpikeState.Operation.Terminal &&
                SpikeState.NavigationRecovery.TrySpend(Time.unscaledTime, TaxiAI.PathingFailureCap))
            {
                SpikeState.NavCallbackRecoveryPending = true;
                SpikeState.NavCallbackRecoveryOrder = SpikeState.NavOrder;
                SpikeState.NavCallbackRecoveryOperation = SpikeState.OperationId;
                Mod.Log.Error($"[nav] a native getter threw; reverse revoked and ONE bounded recovery queued: {ex.Message}");
            }
            else
            {
                SpikeState.PollingActive = false;
                SpikeState.Operation.Terminate("polling getter failure", failed: true);
                Mod.Log.Error($"[nav] repeated native getter failures with no recovery budget left — explicit failure: {ex.Message}");
            }
        }
    }

    // -------------------------------------------------- arrival confirmation (F2)

    /// <summary>
    /// Supervises a pending arrival (brief §8.2) independently of <see cref="SpikeState.PollingActive"/>:
    /// a current navigation <c>Complete</c> is only EVIDENCE — the operation stays ACTIVE until the
    /// taxi is a readable, confirmed halt inside the exact target radius with no native or managed
    /// reverse. The evidence is order/operation scoped and bounded, so it never completes a newer
    /// target and never hangs the operation. The pickup board/park is armed only on an ACCEPTED
    /// arrival.
    /// </summary>
    private static void TickArrivalConfirmation()
    {
        if (Time.timeScale == 0f)
            return;
        if (!SpikeState.NavCallbackArrivalPending)
            return;
        // The game's own patrol driver owns its own arrival (TickPatrolSupervision, which reads the
        // same pending evidence): never steal a patrol arrival away from its StopPatrol hand-off.
        if (TaxiAI.Active)
            return;

        try
        {
            int order = SpikeState.NavCallbackArrivalOrder;
            long operation = SpikeState.NavCallbackArrivalOperation;
            bool currentOrder = order == SpikeState.NavOrder;
            bool currentOperation = operation == SpikeState.OperationId;
            bool operationActive = SpikeState.Operation.Active;

            // A superseded or already finished operation is dropped without any native access.
            if (!currentOrder || !currentOperation || !operationActive)
            {
                ClearPendingArrival();
                TaxiLog.Verbose("[nav] pending arrival evidence dropped (order/operation changed) — no state changed.");
                return;
            }

            LandVehicle? veh = SpikeState.Vehicle;
            if (veh == null || veh.Pointer == IntPtr.Zero)
                return;

            float pendingSeconds = SpikeState.NavCallbackArrivalAt > 0f
                ? Time.unscaledTime - SpikeState.NavCallbackArrivalAt
                : 0f;
            float distance = Vector3.Distance(veh.transform.position, SpikeState.NavTarget);
            bool speedReadable = TryReadSignedSpeed(veh, out float signedSpeed);
            bool reversing = SpikeState.Reverse.Active;
            if (!TryReadReversing(veh, out bool nativeReversing)) reversing = true;
            else reversing |= nativeReversing;

            ArrivalGateOutcome outcome = ArrivalConfirmationPolicy.Evaluate(
                pending: true, currentOrder: currentOrder, currentOperation: currentOperation,
                operationActive: operationActive, pendingSeconds: pendingSeconds,
                distance: distance, speedReadable: speedReadable, signedSpeed: signedSpeed, reversing: reversing);

            if (outcome == ArrivalGateOutcome.Expire)
            {
                ClearPendingArrival();
                TaxiLog.Verbose(
                    $"[nav] pending arrival evidence expired after {TaxiDestinations.Num(pendingSeconds)} s " +
                    $"(readable={speedReadable}, speed={TaxiDestinations.Num(signedSpeed)} km/h, reversing={reversing}, " +
                    $"{TaxiDestinations.Num(distance)} m to the target) — the operation stays active for the normal deadline/recovery.");
                return;
            }
            if (outcome != ArrivalGateOutcome.Accept)
                return; // still supervising — never an early board/Arrived or fare stop

            // ACCEPTED: the confirmed halt at the target completes THIS operation exactly once.
            ClearPendingArrival();
            SpikeState.PollingActive = false;
            SpikeState.Operation.Terminate("arrival confirmed", failed: false);
            Mod.Log.Info(
                $"[nav] arrival confirmed — readable halt {TaxiDestinations.Num(signedSpeed)} km/h, " +
                $"{TaxiDestinations.Num(distance)} m from {SpikeCommands.Fmt(SpikeState.NavTarget)}.");
            if (SpikeState.NavToPlayer)
            {
                SpikeState.RideAwaitingBoard = true;
                Mod.Log.Info($"[taxi-call] taxi arrived at player ({TaxiDestinations.Num(distance)} m from the resolved target) — board with E.");
                SpikeCommands.CompletePickup(veh, "arrival confirmed");
            }
        }
        catch (Exception ex)
        {
            // A getter failure must not leave a phantom pending arrival: drop it and let the normal
            // deadline/recovery bound the operation (brief §8.2).
            ClearPendingArrival();
            Mod.Log.Warn($"[nav] pending arrival confirmation failed: {ex.Message}");
        }
    }

    /// <summary>Clears the bounded pending-arrival evidence (idempotent).</summary>
    private static void ClearPendingArrival()
    {
        SpikeState.NavCallbackArrivalPending = false;
        SpikeState.NavCallbackArrivalOrder = -1;
        SpikeState.NavCallbackArrivalOperation = -1;
        SpikeState.NavCallbackArrivalAt = 0f;
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
        // picker too (belt and braces next to GameLifecycle.OnPreLoad — a
        // reload always passes through a non-Main scene).
        if (!string.Equals(sceneName, "Main", StringComparison.OrdinalIgnoreCase))
        {
            Mod.Log.Info($"[picker] scene '{sceneName}' (left the gameplay scene) — destination selection cleared.");
            SpikeState.ResetPicker();
        }

        if (!MainSceneLoaded())
            SpikeCommands.AbortForSceneUnload($"scene '{sceneName}'");

        LandVehicle? veh = SpikeState.Vehicle;
        bool vehicleGone = veh == null; // Unity fake-null also covers destroyed objects
        bool hasState = SpikeState.PollingActive || SpikeState.AutoRunning ||
                        SpikeState.DriverNpc != null || SpikeState.PendingSpawnCode != null ||
                        SpikeState.LastVehicleCode != null ||
                        SpikeState.VehicleOwnership.TrackedPointer != IntPtr.Zero;

        if (!vehicleGone || !hasState)
            return;

        Mod.Log.Info(
            $"[scene '{sceneName}'] tracked taxi is gone — clearing navigation polling, automatic pickup state, " +
            "pending respawn and NPC state (stale-state guard).");
        // Package 4: the trunk list holds scene objects — forget it with the
        // rest (UnlockTrunk is per-entry exception-safe and ends with Clear).
        // Only here, where the vehicle is proven gone; a live additive load
        // above keeps its valid lock.
        RideLocks.UnlockTrunk();
        if (!SpikeState.Reset())
            Mod.Log.Error($"[scene '{sceneName}'] state reset refused; a live or unresolved vehicle reference remains tracked.");
    }

    /// <summary>Scene transition hook: cancel queued pickup work before the active scene changes.</summary>
    internal static void OnPreSceneChange()
    {
        // A scene reset invalidates the GLB and every cached native wheel handle; sever
        // them now (managed-only, touches no Unity object) so no later tick can write to
        // a transform from the unloaded scene.
        TaxiVisual.ReleaseWheels();
        SpikeState.CancelPendingSpawn();
        SpikeState.ResetAutoRun();
        SpikeState.StandSpawnPosition = null;
        SpikeState.ClearServiceStatus();
    }

    /// <summary>Used by the mod teardown so a quit never leaves the runner half-alive.</summary>
    internal static void Shutdown()
    {
        TaxiVisual.ReleaseWheels();
        // Quiesce first: invalidate every live order, then every deferred dispatcher.
        // This method deliberately retains the vehicle, ownership, driver and fare proof.
        SpikeState.NextNavOrder();
        SpikeState.PollingActive = false;
        SpikeState.NavToPlayer = false;
        SpikeState.Reverse.End();
        SpikeState.NavReDispatchAt = 0f;
        SpikeState.NavReDispatchTarget = Vector3.zero;
        SpikeState.CancelPendingSpawn();
        SpikeState.ResetAutoRun();
        SpikeState.StandSpawnPosition = null;
        SpikeState.SeatVerifyNpc = null;
        SpikeState.SeatVerifyVeh = null;
        SpikeState.SeatVerifyUntil = 0f;
        SpikeState.SettleCheckAt = 0f;
        SpikeState.SettleCheckVehicle = null;
        _lotDumpPending = false;
        _lotDumpAt = 0f;
        _lotDumpAttempt = 0;
        SpikeState.ClearServiceStatus();
    }

    // ------------------------------------- reverse manoeuvre supervision (brief §7.3)

    /// <summary>
    /// Supervises the bounded reverse manoeuvre independently of <see cref="SpikeState.PollingActive"/>
    /// and BEFORE ride finalization: a polling getter error, a lost scene or an invalid handle
    /// must still end the managed reverse request instead of leaving it stale. Any failure while
    /// supervising revokes the reverse rather than claiming a successful escape.
    /// </summary>
    private static void TickReverseSupervision()
    {
        if (Time.timeScale == 0f)
            return;
        if (!SpikeState.Reverse.Active && SpikeState.NavReDispatchAt <= 0f)
            return;

        try
        {
            // Lifecycle + scene gate (D5/D14): a torn-down lifecycle or an unloaded scene must
            // still clear the managed reverse instead of leaving it stale.
            if (!Mod.Lifecycle.AcceptingWork || !MainSceneLoaded())
            {
                RevokeReverse("supervisor: lifecycle/scene unavailable");
                return;
            }

            LandVehicle? veh = SpikeState.Vehicle;
            VehicleAgent? agent = veh == null ? null : veh.Agent;
            bool ownedAlive = veh != null && veh.Pointer != IntPtr.Zero && SpikeCommands.IsOwnedTaxi(veh);
            if (!ownedAlive || agent == null || !SpikeCommands.HasAuthority())
            {
                RevokeReverse("supervisor: vehicle/host/ownership/agent lost");
                return;
            }

            // A failed actuator must not yield a claimed successful escape/redispatch (D5).
            if (!NativeActuatorPatch.Healthy)
            {
                RevokeReverse("supervisor: actuator unhealthy");
                return;
            }
            // The intended NPC must still exclusively hold THIS wheel and no player may be at it (D5).
            if (PlayerDriver(veh) || !SpikeCommands.DriverAtWheel(veh!))
            {
                RevokeReverse("supervisor: driver/player changed at the wheel");
                return;
            }

            float now = Time.unscaledTime;
            if (SpikeState.Reverse.Active)
            {
                bool readable = TryReadSignedSpeed(veh!, out float signedSpeed);
                float forwardDot = ForwardDot(veh!);
                SpikeState.Reverse.Observe(ToNumerics(veh!.transform.position), readable, signedSpeed, now, forwardDot);

                // Continuous rear clearance during the active reverse (D2): a newly arriving
                // obstacle or an unreadable probe ends the grant promptly, at a bounded cadence.
                if (ReverseSupervisionPolicy.ShouldCheckRear(_reverseRearCheckAt, now))
                {
                    _reverseRearCheckAt = now;
                    if (!RearIsClear(veh))
                    {
                        Mod.Log.Warn("[reverse] the rear is no longer clear during the active reverse — revoking the request.");
                        RevokeReverse("rear blocked during reverse");
                        return;
                    }
                }

                if (SpikeState.Reverse.Expired(now))
                {
                    Mod.Log.Warn(
                        $"[reverse] manoeuvre request window/overall cap ({ReverseManoeuvrePolicy.OverallCapSeconds:0.0}s) reached — ending the request.");
                    TickReDispatch(agent);
                    return;
                }
            }

            if (SpikeState.NavReDispatchAt > 0f && now >= SpikeState.NavReDispatchAt)
                TickReDispatch(agent);
        }
        catch (Exception ex)
        {
            RevokeReverse($"supervisor getter/update failure: {ex.Message}");
        }
    }

    /// <summary>
    /// Revokes the managed reverse intent FIRST, invalidates the order, then attempts native
    /// cleanup in independent steps (brief §7.4). New native stop input requires current authority,
    /// the exact own live taxi and no actual player driver; a local PASSENGER never suppresses the
    /// cleanup of an NPC-driven reverse. On authority loss, a foreign/dead vehicle or a player
    /// takeover only the managed intent is revoked — no new native park/mutation.
    /// </summary>
    private static void RevokeReverse(string reason)
    {
        bool hadReverse = SpikeState.Reverse.Active || SpikeState.NavReDispatchAt > 0f;
        // Managed intent first: revoke and invalidate the order so no later native step can re-arm it.
        SpikeState.Reverse.End();
        SpikeState.NavReDispatchAt = 0f;
        SpikeState.NextNavOrder();
        if (!hadReverse) return;

        LandVehicle? veh = SpikeState.Vehicle;
        bool ownedAlive = veh != null && veh.Pointer != IntPtr.Zero && SpikeCommands.IsOwnedTaxi(veh);
        bool nativeOk = ownedAlive && ReverseSupervisionPolicy.NativeCleanupPermitted(
            SpikeCommands.HasAuthority(), ownedAlive, PlayerDriver(veh));

        if (!nativeOk)
        {
            Mod.Log.Warn($"[reverse] revoked the managed reverse request (managed-only, no native input): {reason}.");
            return;
        }

        // Independent cleanup steps: a throw in the first must not skip the rest (brief §7.4).
        CleanupGuard.Run("reverse revoke (StopReversing)", () => veh!.Agent?.StopReversing(), SpikeCommands.ReportCleanupFailure);
        CleanupGuard.Run("reverse revoke (StopNavigating)", () => veh!.Agent?.StopNavigating(), SpikeCommands.ReportCleanupFailure);
        CleanupGuard.Run("reverse revoke (park)", () => NativeActuatorPatch.RequestPark(veh!), SpikeCommands.ReportCleanupFailure);
        Mod.Log.Warn($"[reverse] revoked the managed reverse request and stopped the native reverse: {reason}.");
    }

    private static System.Numerics.Vector3 ToNumerics(Vector3 v) => new(v.x, v.y, v.z);

    /// <summary>Reads the signed speed and reports readability separately (brief §8.2): a
    /// failed read never proves a standstill, while a valid negative value (including -1) is a
    /// real speed and is not a read failure.</summary>
    private static bool TryReadSignedSpeed(LandVehicle veh, out float signedSpeed)
    {
        signedSpeed = 0f;
        try
        {
            signedSpeed = veh.Speed_Kmh;
            return NavigationArrivalPolicy.IsReadableSpeed(signedSpeed);
        }
        catch (Exception)
        {
            signedSpeed = 0f;
            return false;
        }
    }

    /// <summary>
    /// Reads the native reverse flag; an unreadable flag fails CLOSED (treated as reversing) so a
    /// getter error can never prove a standstill (brief §8.2).
    /// </summary>
    private static bool TryReadReversing(LandVehicle veh, out bool reversing)
    {
        reversing = true;
        try
        {
            reversing = veh.IsReversing;
            return true;
        }
        catch (Exception)
        {
            reversing = true; // unknown is never "not reversing"
            return false;
        }
    }

    /// <summary>Raw native player-driver reference; an unreadable value fails closed (counts as a player).</summary>
    private static bool PlayerDriver(LandVehicle? veh)
    {
        if (veh == null) return true;
        try { return !ReferenceEquals(veh.DriverPlayer, null); }
        catch (Exception) { return true; }
    }

    /// <summary>Dot of the car's current horizontal forward with the captured reverse forward (brief §7.1).</summary>
    private static float ForwardDot(LandVehicle veh)
    {
        try
        {
            Vector3 f = veh.transform.forward;
            Vector3 flat = new(f.x, 0f, f.z);
            Vector3 captured = _reverseForward;
            Vector3 capturedFlat = new(captured.x, 0f, captured.z);
            if (flat.sqrMagnitude < 1e-6f || capturedFlat.sqrMagnitude < 1e-6f) return 0f;
            return Vector3.Dot(flat.normalized, capturedFlat.normalized);
        }
        catch (Exception) { return 0f; }
    }
}

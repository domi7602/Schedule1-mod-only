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
    private const float NavigationTimeoutSeconds = 45f;
    private const float AutoStepDelaySeconds = 1f;
    private const float AutoRideDistance = 40f;

    /// <summary>Fresh measurement window granted to the settings=null retry (review M2).</summary>
    private const float RetryGraceSeconds = 6f;

    /// <summary>
    /// Distance to the target that counts as "arrived" instead of "stopped short".
    /// Review M1-4: raised from 5 m to 10 m because the proven complete run
    /// (callback=Complete) still ends ~8.3 m from the raw target with
    /// <c>endAtRoad = true</c> — the non-callback verdict must not call that a
    /// failure.
    /// </summary>
    private const float ArrivalThresholdMeters = 10f;

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
            TickPendingSpawn();
            TickLotDump();
            HandleHotkey();
            TickAutoRun();
            TickNavigationPolling();

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
    /// F6–F16 dispatcher — the only in-game control surface, because the
    /// MelonLoader console is log-only (it has no input field, so every `taxi`
    /// command is output-only and the spike is driven from here). Keys are
    /// checked in ascending order and are ignored while a text field is focused
    /// (<c>S1API.Input.Controls.IsTyping</c>), so typing is never hijacked.
    /// </summary>
    private static void HandleHotkey()
    {
        // A focused text field owns the keystroke — do not hijack typing.
        if (S1API.Input.Controls.IsTyping)
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
            // Review M1-2: never inherit an absolute target from an earlier F15 run.
            SpikeState.AutoTarget = null;
            // Stage 3b: never inherit the F17 stand spawn either.
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

        /// <summary>F13 — `taxi go` to the vanilla-proven road target (-131.4, -4.0, 51.9).</summary>
        else if (Input.GetKeyDown(KeyCode.F13))
        {
            if (!TryBeginGo("F13"))
                return;

            // Round 11: (-131.4,-4,51.9) is a vanilla Navigate LOCATION that
            // returned NavCalc(result=Success) in round 10 — if OUR dispatch
            // succeeds there too, the blind forward*40 target was the culprit.
            Mod.Log.Info("F13 → Go to vanilla-proven road target (-131.4, -4.0, 51.9).");
            SpikeCommands.Go(0f, useSettings: true, absoluteTarget: new Vector3(-131.4f, -4.0f, 51.9f));
        }

        /// <summary>F14 — `taxi go` to the second proven road target (-17.1, 0.0, 13.4).</summary>
        else if (Input.GetKeyDown(KeyCode.F14))
        {
            if (!TryBeginGo("F14"))
                return;

            // Second proven road point (returned Success twice in round 10).
            Mod.Log.Info("F14 → Go to vanilla-proven road target (-17.1, 0.0, 13.4).");
            SpikeCommands.Go(0f, useSettings: true, absoluteTarget: new Vector3(-17.1f, 0.0f, 13.4f));
        }

        /// <summary>F15 — full run spawn → npc → go against the proven road target.</summary>
        else if (Input.GetKeyDown(KeyCode.F15))
        {
            if (!TryBeginGo("F15"))
                return;

            // Round 12: full spike (spawn + npc) against the proven road target
            // — the only configuration that has never been measured with the
            // heartbeat polls (Success + NPC at the wheel).
            Mod.Log.Info("F15 pressed — spawn → npc → go to vanilla road target (-131.4, -4.0, 51.9).");
            SpikeState.AutoRunning = true;
            SpikeState.AutoStep = 1;
            SpikeState.AutoNextAt = Time.unscaledTime;
            SpikeState.AutoCode = SpikeState.LastVehicleCode;
            SpikeState.SkipNpcStep = false;
            SpikeState.AutoTarget = new Vector3(-131.4f, -4.0f, 51.9f);
            // Stage 3b: F15 keeps its own road target — no stand, no player target.
            SpikeState.StandSpawnPosition = null;
            SpikeState.AutoToPlayer = false;
        }

        /// <summary>
        /// F16 — `taxi visual on|off` toggle (review M1-1): the console cannot be
        /// typed into, so the Stage-2 switch needs a key next to F8's trace toggle.
        /// Deliberately NOT behind <see cref="TryBeginGo"/>: the key never starts a
        /// run, it only decides what the NEXT spawn looks like, so it may be pressed
        /// at any time (during a run too — an existing vehicle keeps its visuals).
        /// </summary>
        else if (Input.GetKeyDown(KeyCode.F16))
        {
            // Single source of truth: SpikeState.VisualSwapEnabled is exactly what
            // `taxi visual on|off` writes, so key and command can never drift.
            SpikeState.VisualSwapEnabled = !SpikeState.VisualSwapEnabled;
            Mod.Log.Info(
                $"F16 → visual swap {(SpikeState.VisualSwapEnabled ? "ON" : "OFF")} (applies to next spawn).");
        }

        /// <summary>
        /// F17 — Stage 3b call-taxi flow: the taxi starts at the FIXED taxi stand
        /// (a vanilla ParkingLot spot, Dominik: "das Auto nur via den Taxi-Fahrer
        /// spawnen … einen fixen Punkt, wo er losfährt — Parkplatz") and drives TO
        /// the player: spawn at the stand → nearest NPC boards → navigate to a road
        /// point near the player (see <see cref="RoadTarget"/>). F9 then puts the
        /// local player aboard.
        /// </summary>
        else if (Input.GetKeyDown(KeyCode.F17))
        {
            if (!TryBeginGo("F17"))
                return;

            // Arms SpikeState.StandSpawnPosition (one-shot) — refuses to start when
            // no stand can be resolved (see the [stand] log for the reason).
            if (!SpikeCommands.PrepareStand("F17"))
                return;

            Mod.Log.Info("F17 pressed — call-taxi: spawn at the taxi stand → +1s npc → +1s navigate to the player.");
            SpikeState.AutoRunning = true;
            SpikeState.AutoStep = 1;
            SpikeState.AutoNextAt = Time.unscaledTime;
            SpikeState.AutoCode = SpikeState.LastVehicleCode;
            SpikeState.SkipNpcStep = false;
            SpikeState.AutoTarget = null;
            SpikeState.AutoToPlayer = true;
        }
    }

    /// <summary>
    /// Shared gate for every hotkey that starts a run: while an automatic run is
    /// in progress or a deferred respawn is still pending, the key is ignored
    /// (with a reason) instead of interrupting the run — the same rule the
    /// F6/F11/F12/F15 blocks used to repeat verbatim.
    /// </summary>
    /// <param name="key">The pressed key, used verbatim in the ignore message.</param>
    /// <returns><c>true</c> when the caller may start its run.</returns>
    private static bool TryBeginGo(string key)
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
        // Stage 3b: F6 is the player-side debug flow — never inherit the F17 stand.
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
                // since review M1-2 — also clears AutoTarget. Keep both: an F15 run
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
                // Stage 3b: the F17 run does not drive to a fixed road target, it
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

    // ---------------------------------------------------------- nav callback

    /// <summary>
    /// Invoked by <c>VehicleAgent.Navigate</c> through the interop
    /// <c>NavigationCallback</c> conversion — <c>Failed</c> / <c>Complete</c> /
    /// <c>Stopped</c> is the authoritative end of a run (review M7), so polling stops
    /// as soon as a result arrives.
    /// </summary>
    internal static void OnNavigationResult(VehicleAgent.ENavigationResult result)
    {
        string text = result.ToString();
        SpikeState.NavCallbackResult = text;

        float elapsed = SpikeState.NavStartTime > 0f ? Time.unscaledTime - SpikeState.NavStartTime : -1f;
        Mod.Log.Info(
            $"[nav] callback result={text} after {elapsed:F1}s (frame={Time.frameCount}) — " +
            "authoritative completion signal, navigation polling stops.");
        SpikeState.PollingActive = false;

        // Stage 3b: the F17 run ends HERE — the taxi left the stand and reached the
        // road point next to the player (the local player then boards with F9).
        if (SpikeState.NavToPlayer && result == VehicleAgent.ENavigationResult.Complete)
        {
            LandVehicle? arrivedVeh = SpikeState.Vehicle;
            float dist = arrivedVeh == null ? -1f : Vector3.Distance(arrivedVeh.transform.position, SpikeState.NavTarget);
            Mod.Log.Info(
                $"[F17] taxi arrived at player (callback=Complete after {elapsed:F1}s, " +
                $"{dist:F1} m from the resolved target {SpikeCommands.Fmt(SpikeState.NavTarget)}) — press F9 to board.");
        }
        else if (SpikeState.NavToPlayer)
        {
            Mod.Log.Warn($"[F17] call-taxi run ended WITHOUT arriving at the player (callback={text} after {elapsed:F1}s) — see the [target] lines for the candidate that was dispatched.");
        }
    }

    // ---------------------------------------------------------- nav polling

    /// <summary>
    /// 0.5 s heartbeat while <see cref="SpikeState.PollingActive"/>: logs
    /// AutoDriving / navCalc / distance / graph / stuck / speed / callback,
    /// fires the one <c>settings = null</c> retry, applies the 45 s timeout and
    /// prints the terminal verdict (ARRIVED within
    /// <see cref="ArrivalThresholdMeters"/> vs. STOPPED SHORT).
    /// </summary>
    private static void TickNavigationPolling()
    {
        if (!SpikeState.PollingActive)
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

            Vector3 position = veh.transform.position;
            float distance = Vector3.Distance(position, SpikeState.NavTarget);
            Mod.Log.Info(
                $"[nav t={elapsed:F1}s] AutoDriving={autoDriving} navCalc={navCalc} " +
                $"target={SpikeCommands.Fmt(SpikeState.NavTarget)} " +
                $"vehicle={SpikeCommands.Fmt(position)} distToTarget={distance:F1}m " +
                $"onVehicleGraph={agent.IsOnVehicleGraph()} stuck={agent.GetIsStuck()} " +
                $"reversing={agent.IsReversing} " +
                $"speed={veh.Speed_Kmh:F1} km/h callback={SpikeState.NavCallbackResult ?? "-"}");

            // One timeout for every shape of hang: AutoDriving still true, but also a
            // path calculation that never finishes (review: the 45 s limit used to
            // apply only inside the autoDriving branch).
            if (elapsed >= NavigationTimeoutSeconds)
            {
                SpikeState.PollingActive = false;
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
                Mod.Log.Info(
                    $"[nav] retry #1: Navigate(target, settings=null, callback) at t={elapsed:F1}s " +
                    $"(nothing drove with NavigationSettings, no calculation pending) — grace window {RetryGraceSeconds:F0}s starts now.");
                try
                {
                    agent.Navigate(SpikeState.NavTarget, null, SpikeState.NavCallback);
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
            string verdict = distance <= ArrivalThresholdMeters
                ? $"ARRIVED (within the {ArrivalThresholdMeters:F0} m threshold){(SpikeState.NavToPlayer ? " — taxi arrived at player" : string.Empty)}"
                : $"STOPPED SHORT ({distance:F1} m from the target — mid-route stop, failed path or an early terminal){(SpikeState.NavToPlayer ? " (call-taxi to the player)" : string.Empty)}";
            Mod.Log.Info(
                $"[nav] AutoDriving false and no calculation pending after {elapsed:F1}s — {verdict}; " +
                $"target={SpikeCommands.Fmt(SpikeState.NavTarget)} " +
                $"vehicle={SpikeCommands.Fmt(position)} " +
                $"finalDistance={distance:F1}m (startDistance={SpikeState.NavStartDistance:F1}m, " +
                $"start={SpikeCommands.Fmt(SpikeState.NavStartPosition)}) " +
                $"everAutoDriving={SpikeState.NavEverAutoDriving} nullRetryFired={SpikeState.NavRetried} " +
                $"callback={SpikeState.NavCallbackResult ?? "-"}.");
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
        Mod.Log.Info($"[lots] ParkingLot dump scheduled {LotDumpFirstDelaySeconds:F1}s after scene '{sceneName}' loaded.");

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

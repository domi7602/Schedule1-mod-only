using System;
using System.Collections.Generic;
using System.Globalization;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.NPCs.Behaviour;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using MelonLoader;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Stage 4 — "clone the police AI" (Dominik, 2026-09-27): "können wir nicht die KI
/// klonen, vom Spiel z.B. die wo das Polizeifahrzeug steuert".
///
/// The finding that made this possible: our taxi already drives with the game's own
/// autopilot (<c>VehicleAgent</c> — the same component every police car uses), so what
/// was missing is not a driver but the <b>supervision</b> the game puts on top of it.
/// That supervision is <c>VehiclePatrolBehaviour</c>, and it is fully reachable from a
/// mod:
///
/// <list type="bullet">
/// <item><c>VehiclePatrolRoute</c> is a plain object (public ctor): a name plus a
/// <c>Il2CppReferenceArray&lt;Transform&gt;</c> of waypoints — buildable at runtime,
/// no scene asset required.</item>
/// <item><c>SetRoute(route)</c>, <c>Activate()</c> and <c>StartPatrol()</c> are all
/// public on the proxy, as is <c>IsAsCloseAsPossible(Vector3, out Vector3)</c> — the
/// game's own "can I get there / where would I end up" check.</item>
/// <item>The numbers the game's supervision uses are readable statics:
/// <c>MAX_CONSECUTIVE_PATHING_FAILURES</c>, <c>PROGRESSION_THRESHOLD</c> (patrol) and
/// <c>TIME_STATIONARY_TO_EXIT</c>, <c>STATIONARY_THRESHOLD</c>,
/// <c>CLOSE_ENOUGH_THRESHOLD</c>, <c>EXIT_VEHICLE_MAX_SPEED</c>,
/// <c>RECENT_VISIBILITY_THRESHOLD</c>, <c>UPDATE_FREQUENCY</c> (pursuit).</item>
/// </list>
///
/// The pursuit behaviour is NOT usable for us: it hangs off a <c>Player</c> target and
/// needs vision events — for "drive to a place" the patrol is the right one.
///
/// Everything here is best-effort and reversible: <see cref="StartPatrol"/> returns
/// <c>false</c> on any failure (having cleaned up after itself) and the caller falls
/// back to the mod's own <c>Navigate()</c> dispatch. The mod's polling keeps
/// supervising either way, so the ride kernel's arrival verdict is unchanged.
/// </summary>
internal static class TaxiAI
{
    /// <summary>The GAME's own driver for our taxi, alive only while a ride drives.</summary>
    private static VehiclePatrolBehaviour? Patrol;
    private static bool PatrolOwned;
    private static IntPtr LastDestroyedPatrol;
    private static int LastDestroyedPatrolFrame = -1;

    private static float _lastDeactivateWarnAt;
    private static string _lastDeactivateMessage = string.Empty;
    private static int _deactivateSuppressed;

    /// <summary>The runtime-built route handed to the game's behaviour.</summary>
    private static VehiclePatrolRoute? Route;

    /// <summary>Waypoint objects created for the route (the game wants Transforms).</summary>
    private static readonly List<GameObject> WaypointObjects = new();

    private static string RouteLabel = string.Empty;
    private static bool ConstantsAdopted;

    /// <summary>
    /// How many consecutive pathing failures the GAME tolerates before its patrol gives
    /// up. Until <see cref="AdoptConstants"/> has read it, the mod's own default (2, the
    /// recovery ladder's shape) applies.
    /// </summary>
    internal static int PathingFailureCap { get; private set; } = 2;

    /// <summary>True while the game's behaviour is attached and driving our taxi.</summary>
    internal static bool Active
    {
        get
        {
            try
            {
                return Patrol != null && Patrol.Pointer != IntPtr.Zero;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>Waypoint the game's behaviour is currently heading for (-1 = unknown).</summary>
    internal static int CurrentWaypoint
    {
        get
        {
            try
            {
                return Patrol == null ? -1 : Patrol.CurrentWaypoint;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }

    /// <summary>Number of waypoints in the route handed to the game (0 = no route).</summary>
    internal static int WaypointCount
    {
        get
        {
            try
            {
                return Route?.Waypoints == null ? 0 : Route.Waypoints.Length;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

    /// <summary>Human-readable label of the active route.</summary>
    internal static string Label => RouteLabel;

    // ------------------------------------------------------- A: the game's own numbers

    /// <summary>
    /// Reads the numbers the game's own driving supervision runs on and adopts the ONE
    /// whose meaning is unambiguous: <c>MAX_CONSECUTIVE_PATHING_FAILURES</c> is literally
    /// "how many failed path calculations in a row before giving up", which is what the
    /// mod's recovery ladder needs. The rest is dumped by <see cref="DumpConstants"/>
    /// but not acted on — their units (e.g. whether <c>STATIONARY_THRESHOLD</c> is km/h
    /// or m/s) would have to be guessed, and guessing is what produced the bad
    /// dispatches in the first place.
    /// </summary>
    internal static void AdoptConstants(string caller)
    {
        if (ConstantsAdopted)
            return;

        ConstantsAdopted = true;
        try
        {
            float cap = VehiclePatrolBehaviour.MAX_CONSECUTIVE_PATHING_FAILURES;
            int clamped = Mathf.Clamp((int)Math.Round(cap), 1, 4);
            PathingFailureCap = clamped;
            TaxiLog.Verbose(
                $"[ai] {caller}: adopted the game's supervision number — MAX_CONSECUTIVE_PATHING_FAILURES=" +
                $"{Num(cap)} — the ride's recovery ladder now re-dispatches up to {clamped} times before giving up.");
            // The MelonLoader console is log-only (no typing): the full dumps only arrive
            // when log.json enables verbose logging; otherwise the ride stays quiet.
            if (TaxiLog.VerboseEnabled)
            {
                DumpConstants(caller);
                DumpDriveModel(caller);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn(
                $"[ai] {caller}: could not read MAX_CONSECUTIVE_PATHING_FAILURES " +
                $"({ex.GetType().Name}: {ex.Message}) — keeping the mod's default ({PathingFailureCap}).");
        }
    }

    /// <summary>Dumps every readable number of the game's driving supervision into the log.</summary>
    internal static void DumpConstants(string caller)
    {
        Mod.Log.Info($"[ai] {caller}: the game's own driving supervision numbers (read live, not guessed):");
        Mod.Log.Info($"[ai]   patrol  MAX_CONSECUTIVE_PATHING_FAILURES = {Read(() => VehiclePatrolBehaviour.MAX_CONSECUTIVE_PATHING_FAILURES)}");
        Mod.Log.Info($"[ai]   patrol  PROGRESSION_THRESHOLD            = {Read(() => VehiclePatrolBehaviour.PROGRESSION_THRESHOLD)}");
        Mod.Log.Info($"[ai]   pursuit RECENT_VISIBILITY_THRESHOLD     = {Read(() => VehiclePursuitBehaviour.RECENT_VISIBILITY_THRESHOLD)}");
        Mod.Log.Info($"[ai]   pursuit CLOSE_ENOUGH_THRESHOLD          = {Read(() => VehiclePursuitBehaviour.CLOSE_ENOUGH_THRESHOLD)}");
        Mod.Log.Info($"[ai]   pursuit EXIT_VEHICLE_MAX_SPEED          = {Read(() => VehiclePursuitBehaviour.EXIT_VEHICLE_MAX_SPEED)}");
        Mod.Log.Info($"[ai]   pursuit UPDATE_FREQUENCY                = {Read(() => VehiclePursuitBehaviour.UPDATE_FREQUENCY)}");
        Mod.Log.Info($"[ai]   pursuit STATIONARY_THRESHOLD            = {Read(() => VehiclePursuitBehaviour.STATIONARY_THRESHOLD)}");
        Mod.Log.Info($"[ai]   pursuit TIME_STATIONARY_TO_EXIT         = {Read(() => VehiclePursuitBehaviour.TIME_STATIONARY_TO_EXIT)}");
        Mod.Log.Info(
            $"[ai]   patrol  RepathDistanceThresholdMap is an INSTANCE AnimationCurve (pursuit-only) — " +
            "no static read; the patrol routes around unreachable waypoints via IsAsCloseAsPossible instead.");
        Mod.Log.Info($"[ai] TODO (the game's own driving model, not implemented): " +
                     "StartReverse/StopReversing, GetForwardObstacle, UpdateSpeedReduction, RefreshSpeedZone, OverrideMaxSteerAngle.");
    }

    /// <summary>
    /// Dumps the GAME's public drive model of the live VehicleAgent (v0.4.7f6 API:
    /// obstacle sensors/ranges, turn-speed reduction, steering PID, path tolerance,
    /// stuck thresholds, kinematic mode). These are TUNABLE - the tuning round uses
    /// these exact numbers instead of guessed ones.
    /// </summary>
    internal static void DumpDriveModel(string caller)
    {
        try
        {
            LandVehicle? v = SpikeState.Vehicle;
            VehicleAgent? agent = v == null ? null : v.Agent;
            if (agent == null)
            {
                Mod.Log.Info($"[ai] {caller}: no VehicleAgent yet - drive-model dump skipped.");
                return;
            }

            Mod.Log.Info($"[ai] {caller}: VehicleAgent drive model (live values, tunable):");
            Mod.Log.Info($"[ai]   obstacles  OBSTACLE_MIN_RANGE={Num(VehicleAgent.ObstacleMinRange)} OBSTACLE_MAX_RANGE={Num(VehicleAgent.ObstacleMaxRange)} (sensors FL/FM/FR/RR/RL)");
            Mod.Log.Info($"[ai]   path       MaxDistanceFromPath={Num(VehicleAgent.MaxDistanceFromPath)} WhenReversing={Num(VehicleAgent.MaxDistanceFromPathWhenReversing)}");
            Mod.Log.Info($"[ai]   steering   Steer_P={Num(VehicleAgent.Steer_P)} I={Num(VehicleAgent.Steer_I)} D={Num(VehicleAgent.Steer_D)} FollowRate={Num(agent.steerTargetFollowRate)} MaxAngleOverride={Num(VehicleAgent.MaxSteerAngleOverride)}");
            Mod.Log.Info($"[ai]   throttle   P={Num(VehicleAgent.Throttle_P)} I={Num(VehicleAgent.Throttle_I)} D={Num(VehicleAgent.Throttle_D)} UnmarkedSpeed={Num(VehicleAgent.UnmarkedSpeed)} ReverseSpeed={Num(VehicleAgent.ReverseSpeed)}");
            Mod.Log.Info($"[ai]   turns      minRange={Num(agent.turnSpeedReductionMinRange)} maxRange={Num(agent.turnSpeedReductionMaxRange)} divisor={Num(agent.turnSpeedReductionDivisor)} minTurningSpeed={Num(agent.minTurningSpeed)}");
            Mod.Log.Info($"[ai]   stuck      StuckTimeThreshold={Num(agent.StuckTimeThreshold)} StuckSamples={agent.StuckSamples} StuckDistanceThreshold={Num(agent.StuckDistanceThreshold)}");
            Mod.Log.Info($"[ai]   kinematic  SpeedMultiplier={Num(VehicleAgent.KinematicModeSpeedMultiplier)} RotationSpeed={Num(VehicleAgent.KinematicModeRotationSpeed)} (vanilla traffic drives kinematic = crash-free rails)");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[ai] {caller}: drive-model dump failed: {ex.Message}");
        }
    }

    private static string Read(Func<float> read)
    {
        try
        {
            return Num(read());
        }
        catch (Exception ex)
        {
            return $"<unavailable: {ex.GetType().Name}>";
        }
    }

    private static string Num(float value) => value.ToString("0.###", CultureInfo.InvariantCulture);

    // ------------------------------------------------------------- B: the patrol clone

    /// <summary>
    /// Hands the ride to the GAME's own driver: builds a <c>VehiclePatrolRoute</c> with
    /// two runtime waypoints (where the car is, where it should be) and attaches
    /// <c>VehiclePatrolBehaviour</c> to the taxi driver, then
    /// <c>SetRoute</c>/<c>Activate</c>/<c>StartPatrol</c> — the same order the police
    /// chain uses (see <c>PoliceOfficer.StartVehiclePatrol</c>).
    ///
    /// Every step is logged BEFORE it runs: if the game hangs inside one of them, the
    /// last line in the log names the step (the lesson from the earlier freeze reports).
    /// </summary>
    internal static bool StartPatrol(NPC? driver, LandVehicle? veh, Vector3 arrival, string label, out Vector3 resolvedTarget)
    {
        resolvedTarget = arrival;
        StopPatrol("restart");

        if (driver == null || veh == null)
        {
            Mod.Log.Warn("[patrol] the game's patrol driver needs a driver NPC in the vehicle — none given.");
            return false;
        }

        AdoptConstants("ride");

        try
        {
            Vector3 from = veh.transform.position;

            // Attach/reuse the behaviour FIRST (review 2026-10-02, point 8): the game's
            // own reachability check needs the instance, and its answer decides the ONE
            // point that route, NavTarget and drop-off all use afterwards.
            Patrol = driver.GetComponent<VehiclePatrolBehaviour>();
            // Unity Destroy is deferred until frame end. Never reuse a component
            // already scheduled for destruction during a same-frame route change.
            if (Patrol != null && LastDestroyedPatrolFrame == Time.frameCount &&
                Patrol.Pointer == LastDestroyedPatrol)
                Patrol = null;
            PatrolOwned = false;
            if (Patrol != null)
            {
                TaxiLog.Verbose("[patrol] the taxi driver already carries a VehiclePatrolBehaviour — reusing it.");
            }
            else
            {
                Patrol = driver.gameObject.AddComponent<VehiclePatrolBehaviour>();
                PatrolOwned = true;
                TaxiLog.Verbose("[patrol] VehiclePatrolBehaviour attached to the taxi driver (runtime AddComponent) — next: Vehicle, SetRoute, Activate, StartPatrol.");
            }

            // Behaviour-framework wiring (bug3, 2026-09-29): VehiclePatrolBehaviour is an
            // NPCs.Behaviour.Behaviour, and the native Activate/StartPatrol expects its
            // owner wiring (Npc + beh). A runtime AddComponent on our custom taxi_driver
            // left that wiring null -> the NullReferenceException that threw every clone
            // attempt back to the mod's Navigate fallback. Wire best-effort.
            try
            {
                // Npc is not settable on this build - the beh wiring below carries the owner.
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[patrol] Npc wiring failed: {ex.Message}");
            }
            try
            {
                NPCBehaviour beh = driver.GetComponent<NPCBehaviour>();
                if (beh == null)
                    beh = driver.gameObject.AddComponent<NPCBehaviour>();
                Patrol.beh = beh;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[patrol] beh wiring failed: {ex.Message}");
            }

            Patrol.Vehicle = veh;
            Patrol.aggressiveDrivingEnabled = false;

            // Resolve the dispatch target BEFORE the route is built (review point 8) —
            // the same point goes into the route, NavTarget and the drop-off note.
            resolvedTarget = ResolveDispatchTarget(arrival, label);

            // The game's route wants Transform objects, not vectors: two throwaway
            // waypoint objects, kept alive in WaypointObjects until StopPatrol.
            GameObject start = new GameObject("TaxiPatrol_Waypoint_0");
            start.transform.position = from;
            GameObject goal = new GameObject("TaxiPatrol_Waypoint_1");
            goal.transform.position = resolvedTarget;
            WaypointObjects.Add(start);
            WaypointObjects.Add(goal);

            RouteLabel = label;
            Route = new VehiclePatrolRoute();
            Route.RouteName = $"Taxi - {label}";
            Route.Waypoints = new Il2CppReferenceArray<Transform>(new[] { start.transform, goal.transform });
            Route.StartWaypointIndex = 0;

            TaxiLog.Verbose(
                $"[patrol] route '{Route.RouteName}': {Route.Waypoints.Length} waypoints " +
                $"{TaxiDestinations.Fmt(from)} -> {TaxiDestinations.Fmt(resolvedTarget)}.");

            Patrol.SetRoute(Route);
            TaxiLog.Verbose($"[patrol] SetRoute done (CurrentWaypoint={Patrol.CurrentWaypoint}).");

            Patrol.enabled = true;
            Patrol.Activate();
            TaxiLog.Verbose("[patrol] Activate done - StartPatrol next.");

            Patrol.StartPatrol();
            TaxiLog.Verbose(
                $"[patrol] StartPatrol done (CurrentWaypoint={Patrol.CurrentWaypoint}, isDriving={Patrol.isDriving}) " +
                "— THE GAME DRIVES NOW, the mod only supervises.");

            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error(
                $"[patrol] cloning the game's patrol driver failed ({ex.GetType().Name}: {ex.Message}) — " +
                "falling back to the mod's own Navigate() dispatch.");
            resolvedTarget = arrival;
            StopPatrol("start failed");
            return false;
        }
    }

    /// <summary>
    /// Review 2026-10-02, point 8: resolves the ONE dispatch target before the route is
    /// built. <c>IsAsCloseAsPossible</c> stays advisory (its semantics are not fully
    /// proven): a meaningful delta (≥ 0.5 m) rebinds to the game's closest reachable
    /// point; a zero-delta "unreachable" verdict keeps the exact point (logged), with
    /// the 8 s start self-test covering the ride.
    /// </summary>
    private static Vector3 ResolveDispatchTarget(Vector3 arrival, string label)
    {
        if (Patrol == null)
            return arrival;

        const float rebindMeters = 0.5f;
        try
        {
            bool reachable = Patrol.IsAsCloseAsPossible(arrival, out Vector3 closest);
            float delta = Vector3.Distance(arrival, closest);

            if (!reachable && delta >= rebindMeters)
            {
                Mod.Log.Warn(
                    $"[ai] '{label}': the goal is not reachable — aiming at the game's closest reachable point " +
                    $"{TaxiDestinations.Fmt(closest)} ({TaxiDestinations.Num(delta)} m from the wanted point).");
                return closest;
            }

            if (!reachable)
            {
                Mod.Log.Warn(
                    $"[ai] '{label}': the game reports the goal unreachable although its closest point matches it " +
                    $"(delta {TaxiDestinations.Num(delta)} m) — keeping the exact point; the start self-test covers the ride.");
                return arrival;
            }

            TaxiLog.Verbose(
                $"[ai] the game's own arrival check for '{label}': reachable (route ends {TaxiDestinations.Num(delta)} m from the wanted point).");
            return arrival;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[ai] the game's arrival check threw ({ex.GetType().Name}: {ex.Message}) — keeping the wanted point.");
            return arrival;
        }
    }

    /// <summary>
    /// Releases the game's behaviour and removes the runtime waypoints. Safe to call
    /// when nothing is attached (the common case: no ride running).
    /// </summary>
    internal static void StopPatrol(string reason)
    {
        bool had = Patrol != null || WaypointObjects.Count > 0;

        if (Patrol != null)
        {
            TaxiLog.Verbose($"[patrol] releasing the game's patrol driver ({reason}).");
            try
            {
                // Paket F (2026-09-29): Deactivate() NREs on an already-torn-down
                // behaviour (one Warn per ride end in the log) — only deactivate a live
                // one, and keep any residual noise at Debug level (Destroy below still
                // removes only mod-owned behaviours).
                if (Patrol.enabled || Patrol.isActiveAndEnabled)
                    Patrol.Deactivate();
            }
            catch (Exception ex)
            {
                ReportDeactivateFailure(ex);
                try { Patrol.enabled = false; }
                catch (Exception disableEx) { Mod.Log.Warn($"[patrol] disabling failed: {disableEx.Message}"); }
            }

            try
            {
                if (PatrolOwned)
                {
                    LastDestroyedPatrol = Patrol.Pointer;
                    LastDestroyedPatrolFrame = Time.frameCount;
                    UnityEngine.Object.Destroy(Patrol);
                    TaxiLog.Verbose("[patrol] removed mod-owned AddComponent behaviour.");
                }
                else
                {
                    TaxiLog.Verbose("[patrol] prefab behaviour preserved (not created by this mod).");
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[patrol] removing the behaviour failed: {ex.Message}");
            }
        }

        Patrol = null;
        PatrolOwned = false;
        Route = null;
        RouteLabel = string.Empty;

        foreach (GameObject go in WaypointObjects)
        {
            if (go == null)
                continue;

            try
            {
                UnityEngine.Object.Destroy(go);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[patrol] removing a waypoint failed: {ex.Message}");
            }
        }

        WaypointObjects.Clear();

        if (had && !string.Equals(reason, "restart", StringComparison.Ordinal))
            TaxiLog.Verbose($"[patrol] released ({reason}).");
    }

    /// <summary>
    /// Review 2026-10-02: the first occurrence of a Deactivate() failure is logged with
    /// the full exception and a wiring snapshot (a quieter log alone would repair
    /// nothing); repeats of the SAME error are throttled to one report per 30 s, and a
    /// DIFFERENT error always logs immediately.
    /// </summary>
    private static void ReportDeactivateFailure(Exception ex)
    {
        string message = $"{ex.GetType().Name}: {ex.Message}";
        float now = Time.unscaledTime;
        bool sameAsLast = string.Equals(message, _lastDeactivateMessage, StringComparison.Ordinal);
        if (sameAsLast && now - _lastDeactivateWarnAt < 30f)
        {
            _deactivateSuppressed++;
            return;
        }

        string suppressed = _deactivateSuppressed > 0
            ? $" ({_deactivateSuppressed} repeats suppressed since the last report)"
            : string.Empty;
        _deactivateSuppressed = 0;
        _lastDeactivateWarnAt = now;
        _lastDeactivateMessage = message;

        string snapshot;
        try
        {
            snapshot = Patrol == null
                ? "behaviour gone"
                : $"enabled={Patrol.enabled} activeAndEnabled={Patrol.isActiveAndEnabled} " +
                  $"vehicle={(Patrol.Vehicle == null ? "null" : "set")} " +
                  $"route={(Patrol.Route == null ? "null" : "set")} " +
                  $"wp={Patrol.CurrentWaypoint} isDriving={Patrol.isDriving} " +
                  $"beh={(Patrol.beh == null ? "null" : "set")} ptr=0x{Patrol.Pointer.ToInt64():X}";
        }
        catch (Exception snapshotEx)
        {
            snapshot = $"<snapshot failed: {snapshotEx.Message}>";
        }

        Mod.Log.Warn(
            $"[patrol] Deactivate() failed ({message}){suppressed} — wiring snapshot: {snapshot}; " +
            $"disabling + removing the behaviour.\n{ex}");
    }
}

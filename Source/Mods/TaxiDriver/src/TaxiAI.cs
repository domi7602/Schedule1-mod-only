using System;
using System.Collections.Generic;
using System.Globalization;
using Il2CppFishNet.Object;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.NPCs.Behaviour;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Stage 4 — "clone the police AI" (Dominik, 2026-09-27): "können wir nicht die KI
/// klonen, vom Spiel z.B. die wo das Polizeifahrzeug steuert".
///
/// The finding that made this possible: our taxi already drives with the game's own
/// autopilot (<c>VehicleAgent</c> — the same component every police car uses), so what
/// was missing is not a driver but the <b>supervision</b> the game puts on top of it.
/// That supervision is <c>VehiclePatrolBehaviour</c>. It can only be borrowed when the
/// driver's prefab already supplies an idle, initialized and registered instance:
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
/// <see cref="StartPatrol"/> returns false when unavailable. Direct Navigate remains
/// the fallback, except while a previously borrowed patrol has an unconfirmed release:
/// its original state stays retained and replacement navigation is blocked.
/// </summary>
internal static class TaxiAI
{
    /// <summary>The GAME's own driver for our taxi, alive only while a ride drives.</summary>
    private static VehiclePatrolBehaviour? Patrol;
    private static PatrolLease? Lease;

    private sealed class PatrolLease
    {
        internal readonly NPC Driver;
        internal readonly NPCBehaviour Manager;
        internal readonly VehiclePatrolBehaviour Patrol;
        internal readonly LandVehicle Vehicle;
        internal readonly VehiclePatrolRoute? OriginalRoute;
        internal readonly LandVehicle? OriginalVehicle;
        internal readonly bool OriginalEnabled, OriginalAggressive;
        internal VehiclePatrolRoute? AssignedRoute;
        internal bool ActivationStarted, DeactivationRequested, Restoring, ReleaseWarning;
        internal readonly PatrolReleaseGate Release = new();

        internal PatrolLease(NPC driver, NPCBehaviour manager, VehiclePatrolBehaviour patrol, LandVehicle vehicle)
        {
            Driver = driver;
            Manager = manager;
            Patrol = patrol;
            Vehicle = vehicle;
            OriginalRoute = patrol.Route;
            OriginalVehicle = patrol.Vehicle;
            OriginalEnabled = patrol.enabled;
            OriginalAggressive = patrol.aggressiveDrivingEnabled;
        }
    }

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
                return Lease != null && !Lease.Release.Requested && Lease.ActivationStarted && NetworkGuard.IsAlive(Patrol);
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
    internal static bool NavigationBlocked => Lease != null && Lease.Release.Requested;

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
    /// two runtime waypoints (where the car is, where it should be) and leases an
    /// already registered <c>VehiclePatrolBehaviour</c> from the driver, then
    /// <c>SetRoute</c>/<c>Activate</c>/<c>StartPatrol</c> — the same order the police
    /// chain uses (see <c>PoliceOfficer.StartVehiclePatrol</c>).
    ///
    /// Every step is logged BEFORE it runs: if the game hangs inside one of them, the
    /// last line in the log names the step (the lesson from the earlier freeze reports).
    /// </summary>
    internal static bool StartPatrol(NPC? driver, LandVehicle? veh, Vector3 arrival, string label, out Vector3 resolvedTarget)
    {
        resolvedTarget = arrival;
        PatrolLease? lease = null;
        try
        {
            if (!NetworkGuard.IsAlive(driver) || !SpikeCommands.IsOwnedTaxi(veh) ||
                !SpikeCommands.HasAuthority() || !NetworkGuard.IsAlive(SpikeState.DriverNpc) ||
                SpikeState.DriverNpc!.Pointer != driver!.Pointer || veh!.DriverPlayer != null)
            {
                StopPatrol("patrol precondition changed");
                Mod.Log.Warn("[patrol] owned driver/authority unavailable — no patrol activated.");
                return false;
            }
            StopPatrol("restart");
            if (Lease != null) return false;
            AdoptConstants("ride");
            Vector3 from = veh.transform.position;

            // Only lease an already registered, idle prefab behaviour. Assigning beh
            // on a runtime-added NetworkBehaviour is not FishNet initialization.
            NPCBehaviour manager = driver.Behaviour;
            var candidate = NetworkGuard.IsAlive(manager)
                ? manager.GetComponentInChildren<VehiclePatrolBehaviour>(true) : null;
            if (!BindingValid(driver, manager, candidate, requireIdle: true))
            {
                Mod.Log.Info("[patrol] no idle, registered patrol on the driver's existing controller — using direct Navigate; no runtime network components added.");
                return false;
            }
            var patrol = candidate!;
            lease = new PatrolLease(driver, manager, patrol, veh);
            Lease = lease; // publish cleanup state before the first native write
            Patrol = patrol;
            patrol.Vehicle = veh;
            patrol.aggressiveDrivingEnabled = false;

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
            lease.AssignedRoute = Route;
            Route.RouteName = $"Taxi - {label}";
            Route.Waypoints = new Il2CppReferenceArray<Transform>(new[] { start.transform, goal.transform });
            Route.StartWaypointIndex = 0;

            TaxiLog.Verbose(
                $"[patrol] route '{Route.RouteName}': {Route.Waypoints.Length} waypoints " +
                $"{TaxiDestinations.Fmt(from)} -> {TaxiDestinations.Fmt(resolvedTarget)}.");

            patrol.SetRoute(Route);
            if (!ReferenceEquals(Lease, lease)) return false;
            TaxiLog.Verbose($"[patrol] SetRoute done (CurrentWaypoint={patrol.CurrentWaypoint}).");

            patrol.enabled = true;
            lease.ActivationStarted = true; // a partial activation also needs cleanup
            patrol.Activate();
            if (!ReferenceEquals(Lease, lease)) return false;
            TaxiLog.Verbose("[patrol] Activate done - StartPatrol next.");

            patrol.StartPatrol();
            if (!ReferenceEquals(Lease, lease)) return false;
            TaxiLog.Verbose(
                $"[patrol] StartPatrol done (CurrentWaypoint={patrol.CurrentWaypoint}, isDriving={patrol.isDriving}) " +
                "— THE GAME DRIVES NOW, the mod only supervises.");

            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error(
                $"[patrol] cloning the game's patrol driver failed ({ex.GetType().Name}: {ex.Message}) — " +
                "direct Navigate is allowed only after a confirmed patrol release.");
            resolvedTarget = arrival;
            if (lease == null && Lease != null) StopPatrol("start failed before lease");
            if (ReferenceEquals(Lease, lease)) StopPatrol("start failed");
            return false;
        }
    }

    /// <summary>No registration repair, component creation or network initialization occurs here.</summary>
    private static bool BindingValid(NPC driver, NPCBehaviour manager, VehiclePatrolBehaviour? patrol, bool requireIdle = false)
    {
        if (!NetworkGuard.IsAlive(driver) || !NetworkGuard.IsAlive(manager) || !NetworkGuard.IsAlive(patrol)) return false;
        if (!NetworkGuard.IsAlive(manager.Npc) || !NetworkGuard.IsAlive(patrol!.beh)) return false;
        var network = driver._networkObjectCache;
        if (!NetworkGuard.IsAlive(network) || !NetworkGuard.IsAlive(manager._networkObjectCache) ||
            !NetworkGuard.IsAlive(patrol._networkObjectCache)) return false;
        bool sameOwner = driver.Behaviour != null && driver.Behaviour.Pointer == manager.Pointer &&
            manager.Npc.Pointer == driver.Pointer && patrol.beh.Pointer == manager.Pointer;
        bool sameNetwork = network.Pointer == manager._networkObjectCache.Pointer &&
            network.Pointer == patrol._networkObjectCache.Pointer;
        var stack = manager.behaviourStack;
        int index = patrol.BehaviourIndex;
        bool registered = stack != null && index >= 0 && index < stack.Count &&
            stack[index] != null && stack[index].Pointer == patrol.Pointer &&
            Registered(manager) && Registered(patrol);
        return PatrolBindingPolicy.CanUse(sameOwner, sameNetwork, registered,
            manager.IsServerInitialized && patrol.IsServerInitialized,
            !requireIdle || (!patrol.Active && !patrol.isDriving));
    }

    private static bool Registered(NetworkBehaviour behaviour)
    {
        var network = behaviour._networkObjectCache;
        if (!NetworkGuard.IsAlive(network)) return false;
        var registered = network.NetworkBehaviours;
        int index = behaviour.ComponentIndex;
        return registered != null && index >= 0 && index < registered.Length &&
            registered[index] != null && registered[index].Pointer == behaviour.Pointer;
    }

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
        var lease = Lease;
        if (lease != null)
        {
            if (!lease.Release.TryBegin()) return;
            bool restored = false;
            try { restored = ReleaseLease(lease); }
            catch (Exception ex) { CleanupFailure("patrol lease", ex); }
            finally { lease.Release.End(restored); }
            if (!restored)
            {
                if (!lease.ReleaseWarning)
                {
                    lease.ReleaseWarning = true;
                    Mod.Log.Warn("[patrol] lease release unconfirmed — originals retained; new navigation blocked until a safe stop/retry.");
                }
                return;
            }
            if (!ReferenceEquals(Lease, lease)) return;
        }
        Lease = null;
        Patrol = null;
        Route = null;
        RouteLabel = string.Empty;
        var waypoints = WaypointObjects.ToArray();
        WaypointObjects.Clear();


        foreach (GameObject go in waypoints)
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

        if (had && !string.Equals(reason, "restart", StringComparison.Ordinal))
            TaxiLog.Verbose($"[patrol] released ({reason}).");
    }

    private static bool ReleaseLease(PatrolLease lease)
    {
        var patrol = lease.Patrol;
        if (!NetworkGuard.IsAlive(patrol) || !NetworkGuard.IsAlive(lease.Driver)) return true;
        if (!BindingValid(lease.Driver, lease.Manager, patrol) || !SpikeCommands.IsOwnedTaxi(lease.Vehicle) ||
            !SpikeCommands.HasAuthority() || !ReferenceEquals(Lease, lease)) return false;
        var currentVehicle = patrol.Vehicle;
        bool assignedVehicle = currentVehicle != null && currentVehicle.Pointer == lease.Vehicle.Pointer;
        bool originalVehicle = currentVehicle?.Pointer == lease.OriginalVehicle?.Pointer;
        if (lease.Restoring || !lease.ActivationStarted)
        {
            if (!assignedVehicle && !originalVehicle) return false;
        }
        else if (!assignedVehicle && !(lease.DeactivationRequested && currentVehicle == null)) return false;
        if (patrol.Route != null && patrol.Route.Pointer != lease.AssignedRoute?.Pointer &&
            patrol.Route.Pointer != lease.OriginalRoute?.Pointer) return false;
        // Quiesce only the borrowed component; never brake/seat a player takeover.
        patrol.enabled = false;
        if (lease.Vehicle.DriverPlayer != null) return false;
        if (lease.ActivationStarted && (patrol.Active || patrol.isDriving))
        {
            if (lease.DeactivationRequested) return false;
            lease.DeactivationRequested = true; // never replay unknown partial native work
            if (!CleanupGuard.Run("patrol deactivate", () => patrol.Deactivate(), CleanupFailure)) return false;
        }
        if (!ReferenceEquals(Lease, lease) || !BindingValid(lease.Driver, lease.Manager, patrol) || patrol.Active || patrol.isDriving) return false;
        if (patrol.Route != null && patrol.Route.Pointer != lease.AssignedRoute?.Pointer &&
            patrol.Route.Pointer != lease.OriginalRoute?.Pointer) return false;
        if (!lease.Restoring && lease.ActivationStarted && patrol.Vehicle != null && patrol.Vehicle.Pointer != lease.Vehicle.Pointer) return false;
        lease.Restoring = true;
        bool route = CleanupGuard.Run("patrol route", () => patrol.Route = lease.OriginalRoute!, CleanupFailure);
        bool vehicle = CleanupGuard.Run("patrol vehicle", () => patrol.Vehicle = lease.OriginalVehicle!, CleanupFailure);
        bool aggression = CleanupGuard.Run("patrol aggression", () => patrol.aggressiveDrivingEnabled = lease.OriginalAggressive, CleanupFailure);
        if (!route || !vehicle || !aggression) return false;
        return CleanupGuard.Run("patrol enabled", () => patrol.enabled = lease.OriginalEnabled, CleanupFailure);
    }

    private static void CleanupFailure(string step, Exception ex)
        => Mod.Log.Warn($"[patrol] {step} cleanup failed: {ex.Message}");

}

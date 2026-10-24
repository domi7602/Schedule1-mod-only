// ---------------------------------------------------------------------------
// DrivingPatches.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   Driving/CruiseSpeedPatch.cs    -> section CruiseSpeedPatch.cs
//   Driving/NativeActuatorPatch.cs -> section NativeActuatorPatch.cs
//   Traffic/TrafficLightPatch.cs   -> section TrafficLightPatch.cs
//
// ---------------------------------------------------------------------------

namespace TaxiDriver
{
    // ==== TaxiDriver section: CruiseSpeedPatch.cs ====
    using System;
    using HarmonyLib;
    using Il2CppScheduleOne.Vehicles.AI;
    using S1Mods.Shared;


    /// <summary>
    /// Changes only the freshly calculated forward base target. In installed f12 native code,
    /// UpdateSpeed (RVA 0xAD86A0) calculates zone/unmarked speed; InfrequentUpdate invokes it
    /// BEFORE UpdateSteering, UpdateSweep and UpdateSpeedReduction (0xAD55A6). The latter
    /// still applies native turn/obstacle/destination reductions to this base. LateUpdate
    /// passes targetSpeed directly alongside LandVehicle.Speed_Kmh to the throttle PID
    /// (0xAD61AB..0xAD61E0): targetSpeed is km/h, NOT metres/second.
    /// No DriveFlags object is mutated or replaced, including potentially shared flags.
    /// Native patrol resets and redispatches are handled by the next native UpdateSpeed call.
    /// </summary>
    internal static class CruiseSpeedPatch
    {
        private static HarmonyLib.Harmony? _harmony;
        private static bool _enabled;
        private static bool _errorLogged;
        private static int _loggedOrder = -1;

        internal static void Apply()
        {
            if (_enabled)
                return;
            try
            {
                _harmony ??= new HarmonyLib.Harmony("com.taxidriver.cruise");
                _enabled = PatchGuard.TryPatch(
                    _harmony,
                    typeof(VehicleAgent),
                    nameof(VehicleAgent.UpdateSpeed),
                    postfix: new HarmonyMethod(typeof(CruiseSpeedPatch), nameof(UpdateSpeedPostfix)),
                    log: Mod.Log);
                if (_enabled)
                    Mod.Log.Info("[cruise] owned autonomous taxi forward base target=50 km/h; native turn, obstacle, destination braking and reverse remain enabled. Achieved speed requires gameplay verification.");
                else
                    Mod.Log.Warn("[cruise] UpdateSpeed hook unavailable; native cruising speed retained.");
            }
            catch (Exception ex)
            {
                _enabled = false;
                Mod.Log.Warn($"[cruise] hook installation failed; native speed retained: {ex.Message}");
            }
        }

        private static void UpdateSpeedPostfix(VehicleAgent __instance)
        {
            if (!_enabled)
                return;
            try
            {
                if (!SpikeCommands.HasAuthority() || __instance == null)
                    return;
                var veh = __instance.vehicle;
                if (!SpikeCommands.IsOwnedTaxi(veh))
                    return;
                var flags = __instance.Flags;
                if (flags == null)
                    return;
                float nativeBase = __instance.targetSpeed;
                float target = CruiseSpeedPolicy.BaseTargetKmh(
                    nativeBase,
                    ownedTaxi: true,
                    activeOrder: SpikeState.PollingActive || TaxiAI.Active,
                    autoDriving: __instance.AutoDriving,
                    playerDriving: veh.DriverPlayer != null,
                    reversing: __instance.IsReversing || SpikeState.Reverse.Active,
                    nativeOverride: flags.OverrideSpeed);
                if (target == nativeBase || !float.IsFinite(target))
                    return;
                __instance.targetSpeed = target;
                if (_loggedOrder != SpikeState.NavOrder)
                {
                    _loggedOrder = SpikeState.NavOrder;
                    Mod.Log.Info($"[cruise] order={SpikeState.NavOrder} nativeBase={nativeBase:0.0} targetBase={__instance.targetSpeed:0.0} km/h actual={veh.Speed_Kmh:0.0} km/h; native speed reductions run next.");
                }
            }
            catch (Exception ex)
            {
                if (_errorLogged)
                    return;
                _errorLogged = true;
                try { Mod.Log.Warn($"[cruise] target update skipped on unreadable state: {ex.Message}"); }
                catch { /* Do not let logging break the native driving path. */ }
            }
        }

        internal static void Remove()
        {
            _enabled = false;
            try { _harmony?.UnpatchSelf(); }
            catch (Exception ex) { Mod.Log.Warn($"[cruise] unpatch failed; disabled hook remains inert: {ex.Message}"); }
            finally
            {
                _harmony = null;
                _errorLogged = false;
                _loggedOrder = -1;
            }
        }
    }
    // ==== end TaxiDriver section: CruiseSpeedPatch.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NativeActuatorPatch.cs ====
    using System;
    using System.Linq;
    using System.Text;
    using HarmonyLib;
    using Il2CppScheduleOne.Vehicles;
    using S1Mods.Shared;
    using UnityEngine;


    /// <summary>
    /// Installed f12: FixedUpdate calls virtual slot 61 / ApplyThrottle (RVA 0xAB2EE0).
    /// currentThrottle and handbrakeOverride are inputs; overrideControls selects the
    /// latter. Render Update replaces currentThrottle and LateUpdate clears the request.
    /// Supply inputs ONLY for this native consumption, then restore them even on error.
    /// No steering, yaw, native reverse-output flags or wheel torques are written here.
    /// </summary>
    internal static class NativeActuatorPatch
    {
        private const string HarmonyId = "com.taxidriver.actuator";
        private static HarmonyLib.Harmony? _harmony;
        private static bool _attempted;
        internal static bool Healthy { get; private set; }
        private static IntPtr _parkPointer;
        private static IntPtr _diagnosticPointer;
        private static NativeActuatorMode _lastMode;
        private static float _nextReadback;
        private static bool _errorLogged;

        internal static void Apply()
        {
            if (_attempted) return;
            _attempted = true;
            try
            {
                _harmony = new HarmonyLib.Harmony(HarmonyId);
                var target = AccessTools.DeclaredMethod(typeof(LandVehicle), "ApplyThrottle", Type.EmptyTypes);
                bool patched = PatchGuard.TryPatch(_harmony, target,
                    prefix: new HarmonyMethod(typeof(NativeActuatorPatch), nameof(ApplyThrottlePrefix)) { priority = Priority.Last },
                    postfix: new HarmonyMethod(typeof(NativeActuatorPatch), nameof(ApplyThrottlePostfix)) { priority = Priority.First },
                    finalizer: new HarmonyMethod(typeof(NativeActuatorPatch), nameof(ApplyThrottleFinalizer)), log: Mod.Log);
                var info = target == null ? null : HarmonyLib.Harmony.GetPatchInfo(target);
                Healthy = patched && info != null &&
                    info.Prefixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(ApplyThrottlePrefix)) &&
                    info.Postfixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(ApplyThrottlePostfix)) &&
                    info.Finalizers.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(ApplyThrottleFinalizer));
                Mod.Log.Info($"[actuator] ApplyThrottle prefix/postfix/finalizer healthy={Healthy}; exact-owned recovery/park native inputs only. Native hook execution and physics remain unverified.");
            }
            catch (Exception ex) { Fail(ex); }
            if (!Healthy)
                Mod.Log.Error("[actuator] required consumer hook unavailable; CALL TAXI and passenger rides remain disabled.");
        }

        internal static void RequestPark(LandVehicle veh)
        {
            if (SpikeCommands.HasAuthority() && SpikeCommands.IsOwnedTaxi(veh))
                _parkPointer = veh.Pointer;
        }

        internal static void ReleasePark(LandVehicle veh)
        {
            if (veh.Pointer == _parkPointer) _parkPointer = IntPtr.Zero;
        }

        internal static void ResetIntent()
        {
            _parkPointer = IntPtr.Zero;
            _diagnosticPointer = IntPtr.Zero;
            _lastMode = NativeActuatorMode.None;
            _nextReadback = 0f;
        }

        private sealed class CallState
        {
            internal readonly LandVehicle Vehicle;
            internal NativeActuatorScope? Scope;
            internal readonly NativeActuatorMode Mode;
            internal readonly bool Diagnose;
            internal bool Completed;
            internal CallState(LandVehicle vehicle, NativeActuatorMode mode, bool diagnose)
                => (Vehicle, Mode, Diagnose) = (vehicle, mode, diagnose);
        }

        private static void ApplyThrottlePrefix(LandVehicle __instance, out CallState? __state)
        {
            __state = null;
            if (!Healthy) return;
            try
            {
                if (!SpikeCommands.HasAuthority() || Time.timeScale == 0f) return;
                var tracked = SpikeState.Vehicle;
                bool owned = __instance != null && tracked != null && __instance.Pointer != IntPtr.Zero &&
                             __instance.Pointer == tracked.Pointer && SpikeCommands.IsOwnedTaxi(__instance);
                if (!owned) return;
                bool active = SpikeRunner.HasActuatorOrder;
                bool playerDriver = __instance!.DriverPlayer != null;
                // Explicit park intent is pointer-scoped and discarded when control changes.
                if (active || playerDriver) ReleasePark(__instance);
                bool parkIntent = _parkPointer == __instance.Pointer;
                var mode = NativeActuatorPolicy.Choose(SpikeCommands.HasAuthority(), owned,
                    Time.timeScale == 0f, playerDriver,
                    SpikeState.Reverse.IsCurrentFor(SpikeState.OperationId, SpikeState.NavOrder, Time.unscaledTime),
                    active, parkIntent);
                bool diagnose = _diagnosticPointer != __instance.Pointer || _lastMode != mode ||
                                Time.unscaledTime >= _nextReadback;
                _diagnosticPointer = __instance.Pointer;
                _lastMode = mode;
                if (diagnose) _nextReadback = Time.unscaledTime + 5f;
                if (mode == NativeActuatorMode.None)
                {
                    if (diagnose) Readback(__instance, mode, "native-pass-through");
                    return;
                }
                if (diagnose) Readback(__instance, mode, "prefix-before");
                // Allocate/store Harmony state BEFORE any input mutation.
                __state = new CallState(__instance, mode, diagnose);
                __state.Scope = NativeActuatorScope.Begin(
                    () => (__instance.currentThrottle, __instance.handbrakeOverride, __instance.overrideControls),
                    x => __instance.currentThrottle = x, x => __instance.handbrakeOverride = x,
                    x => __instance.overrideControls = x, mode == NativeActuatorMode.Reverse);
                if (diagnose) Readback(__instance, mode, "prefix-consume");
            }
            catch (Exception ex)
            {
                Complete(__state, "prefix-error");
                Fail(ex);
            }
        }

        private static void ApplyThrottlePostfix(CallState? __state) => Complete(__state, "postfix-consumed");

        private static Exception? ApplyThrottleFinalizer(Exception? __exception, CallState? __state)
        {
            Complete(__state, __exception == null ? "finalizer" : "finalizer-exception");
            if (__exception != null && __state != null) Fail(__exception);
            return __exception; // never suppress the native/other patch exception
        }

        private static void Complete(CallState? state, string phase)
        {
            if (state == null || state.Completed || state.Scope == null) return;
            state.Completed = true;
            try
            {
                if (state.Diagnose) Readback(state.Vehicle, state.Mode, phase);
            }
            finally
            {
                // Do NOT re-evaluate ownership, pause, mode, host or Healthy here: the
                // exact vehicle snapshot from prefix must always get its restoration.
                bool restored = false;
                try { state.Scope?.Dispose(); restored = true; }
                catch (Exception ex) { Fail(ex); }
                if (state.Diagnose) Readback(state.Vehicle, state.Mode, restored ? "restored" : "restoration-failed");
            }
        }

        private static void Readback(LandVehicle veh, NativeActuatorMode mode, string phase)
        {
            try
            {
                var agent = veh.Agent;
                var wheels = new StringBuilder();
                if (veh.wheels != null)
                {
                    for (int i = 0; i < Math.Min(veh.wheels.Count, 4); i++)
                    {
                        var collider = veh.wheels[i]?.wheelCollider;
                        if (collider != null)
                            wheels.Append($" w{i}=(ground={collider.isGrounded},motor={collider.motorTorque:0.0},brake={collider.brakeTorque:0.0})");
                    }
                }
                Mod.Log.Info($"[actuator] frame={Time.frameCount} ptr={veh.Pointer} mode={mode} phase={phase} " +
                    $"currentThrottle={veh.currentThrottle:0.00} throttleOverride={veh.throttleOverride:0.00} " +
                    $"overrideControls={veh.overrideControls} handbrakeOverride={veh.handbrakeOverride} " +
                    $"hand={veh.HandbrakeApplied} brakes={veh.BrakesApplied} vehRev={veh.IsReversing} agentRev={agent?.IsReversing} " +
                    $"speed={veh.Speed_Kmh:0.0} yaw={veh.transform.eulerAngles.y:0.0} steer={veh.CurrentSteerAngle:0.0} " +
                    $"targetSteerNormalized={agent?.targetSteerAngle_Normalized:0.00} simulated={veh.IsPhysicallySimulated} kinematic={veh.Rb?.isKinematic}{wheels}");
            }
            catch { /* Diagnostics are read-only and may not prevent restoration. */ }
        }

        private static void Fail(Exception ex)
        {
            Healthy = false;
            if (_errorLogged) return;
            _errorLogged = true;
            try { Mod.Log.Error($"[actuator] disabled on unreadable/failed control state: {ex.GetType().Name}: {ex.Message}; new service is refused."); }
            catch { }
        }

        internal static void Remove()
        {
            Healthy = false;
            ResetIntent();
            try { _harmony?.UnpatchSelf(); }
            catch (Exception ex) { Fail(ex); }
            finally { _harmony = null; _attempted = false; _errorLogged = false; }
        }
    }
    // ==== end TaxiDriver section: NativeActuatorPatch.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: TrafficLightPatch.cs ====
    using System;
    using System.Linq;
    using HarmonyLib;
    using Il2CppScheduleOne.Vehicles.AI;
    using S1Mods.Shared;


    /// <summary>
    /// Scope private flags around the two parameterless native physics consumers.
    /// Never detour GetWheelSweepCollision: its native out-parameter bridge failed live.
    /// Kinematic raycast driving is unsupported and retains vanilla obstacle handling.
    /// </summary>
    internal static class TrafficLightPatch
    {
        private const string HarmonyId = "com.taxidriver.traffic-lights";
        private static HarmonyLib.Harmony? _harmony;
        private static bool _attempted, _healthy, _errorLogged;
        private static int _loggedOrder = -1, _waitOrder = -1;
        private static IntPtr _waitVehicle;
        private static float _nextWaitProbe;
        private static bool _waiting, _waitLogged;
        private static bool _probeDisabled;

        internal static void Apply()
        {
            if (_attempted) return;
            _attempted = true;
            try
            {
                _harmony = new HarmonyLib.Harmony(HarmonyId);
                bool sweep = PatchConsumer(nameof(VehicleAgent.UpdateSweep));
                bool speed = PatchConsumer(nameof(VehicleAgent.UpdateSpeedReduction));
                _healthy = sweep && speed;
                Mod.Log.Info($"[traffic-lights] parameterless physics consumers healthy={_healthy}; exact owned autonomous taxi only; kinematic driving retains vanilla. Native crossing requires gameplay verification.");
            }
            catch (Exception ex) { Fail(ex); }
            if (!_healthy)
            {
                try { _harmony?.UnpatchSelf(); } catch { }
                Mod.Log.Warn("[traffic-lights] complete restoration hook unavailable; native behavior retained.");
            }
        }

        private static bool PatchConsumer(string name)
        {
            var target = AccessTools.DeclaredMethod(typeof(VehicleAgent), name);
            if (target == null || target.GetParameters().Length != 0 || target.ReturnType != typeof(void)) return false;
            bool patched = PatchGuard.TryPatch(_harmony!, target,
                prefix: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepPrefix)) { priority = Priority.Last },
                postfix: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepPostfix)) { priority = Priority.First },
                finalizer: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepFinalizer)), log: Mod.Log);
            var info = HarmonyLib.Harmony.GetPatchInfo(target);
            return patched && info != null &&
                info.Prefixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepPrefix)) &&
                info.Postfixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepPostfix)) &&
                info.Finalizers.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepFinalizer));
        }

        private sealed class CallState
        {
            internal PrivateFlagsScope<DriveFlags>? Scope;
            internal VehicleAgent Agent = null!;
            internal IntPtr OriginalPointer;
            internal bool Diagnose;
        }

        private static bool Eligible(VehicleAgent __instance)
        {
            if (!_healthy || !Mod.Lifecycle.AcceptingWork || !NetworkGuard.IsInMainScene ||
                !NetworkGuard.IsAlive(__instance)) return false;
            var veh = __instance.vehicle;
            if (!SpikeCommands.IsOwnedTaxi(veh)) return false;
            bool exact = veh.Agent != null && veh.Agent.Pointer == __instance.Pointer;
            bool eligible = TrafficLightPolicy.ShouldIgnore(SpikeCommands.HasAuthority(), exact,
                SpikeRunner.HasActuatorOrder, __instance.AutoDriving, veh.DriverPlayer != null);
            var body = veh.Rb;
            bool physical = NetworkGuard.IsAlive(body) && !body.isKinematic && veh.IsPhysicallySimulated;
            return TrafficLightPolicy.CanBypass(eligible, physical,
                SpikeState.Reverse.Active || __instance.IsReversing || veh.IsReversing, UnityEngine.Time.timeScale == 0f);
        }

        private static void SweepPrefix(VehicleAgent __instance, out CallState? __state)
        {
            __state = null;
            try
            {
                if (!Eligible(__instance)) return;
                var original = __instance.Flags;
                if (original == null || original.Pointer == IntPtr.Zero || original.WasCollected) return;
                var state = new CallState
                {
                    Agent = __instance,
                    OriginalPointer = original.Pointer,
                    Diagnose = _loggedOrder != SpikeState.NavOrder
                };
                __state = state;
                BeginScope(state, original);
                if (state.Diagnose)
                {
                    _loggedOrder = SpikeState.NavOrder;
                    Breadcrumb(state, "consume-private-copy");
                }
            }
            catch (Exception ex) { Complete(__state); Fail(ex); }
        }

        private static void BeginScope(CallState state, DriveFlags original)
        {
            PrivateFlagsScope<DriveFlags>.Begin(() => original, x => state.Agent.Flags = x,
                CopyFlags, (original, copy) => original.Pointer == copy.Pointer,
                scope => state.Scope = scope);
        }

        private static DriveFlags CopyFlags(DriveFlags original) => new DriveFlags
        {
            OverrideSpeed = original.OverrideSpeed,
            OverriddenSpeed = original.OverriddenSpeed,
            OverriddenReverseSpeed = original.OverriddenReverseSpeed,
            SpeedLimitMultiplier = original.SpeedLimitMultiplier,
            IgnoreTrafficLights = true,
            UseRoads = original.UseRoads,
            StuckDetection = original.StuckDetection,
            ObstacleMode = original.ObstacleMode,
            AutoBrakeAtDestination = original.AutoBrakeAtDestination,
            TurnBasedSpeedReduction = original.TurnBasedSpeedReduction
        };

        // A slow, stationary-only diagnostic uses normal managed-to-native calls, not
        // detours on the by-ref method. Compare native hits before/after only the flag.
        internal static bool HoldRecovery(Il2CppScheduleOne.Vehicles.LandVehicle veh)
        {
            try
            {
                if (_probeDisabled) return false;
                var agent = veh.Agent;
                if (!Eligible(agent) || !float.IsFinite(veh.Speed_Kmh) || System.Math.Abs(veh.Speed_Kmh) > 1f)
                { _waiting = false; _waitLogged = false; return false; }
                float now = UnityEngine.Time.unscaledTime;
                if (_waitOrder != SpikeState.NavOrder || _waitVehicle != veh.Pointer)
                {
                    _waiting = false;
                    _waitOrder = SpikeState.NavOrder;
                    _waitVehicle = veh.Pointer;
                    _nextWaitProbe = 0f;
                }
                if (now < _nextWaitProbe) return _waiting;
                _nextWaitProbe = now + 0.25f;
                _waiting = ProbeTrafficWait(agent);
                if (_waiting && !_waitLogged)
                    Mod.Log.Info($"[traffic-lights] order={_waitOrder} native sweeps identify an exclusive traffic-light stop; bounded recovery allowance evaluated separately.");
                _waitLogged = _waiting;
                return _waiting;
            }
            catch (Exception ex)
            {
                _waiting = false;
                _probeDisabled = true;
                try { Mod.Log.Warn($"[traffic-lights] optional wait probe disabled: {ex.Message}; functional bypass unchanged unless flag restoration failed."); }
                catch { }
                return false;
            }
        }

        private static bool ProbeTrafficWait(VehicleAgent agent)
        {
            bool traffic = false, generic = false;
            bool readable = ReadSweep(agent, VehicleAgent.ESweepType.FL, ref traffic, ref generic) &&
                ReadSweep(agent, VehicleAgent.ESweepType.FR, ref traffic, ref generic);
            if (!readable || !traffic || generic) return false;
            CallState? state = null;
            bool filteredHit = true;
            try
            {
                var original = agent.Flags;
                if (original == null || original.Pointer == IntPtr.Zero || original.WasCollected) return false;
                state = new CallState { Agent = agent, OriginalPointer = original.Pointer };
                BeginScope(state, original);
                bool filteredTraffic = false, filteredGeneric = false;
                readable = ReadSweep(agent, VehicleAgent.ESweepType.FL, ref filteredTraffic, ref filteredGeneric) &&
                    ReadSweep(agent, VehicleAgent.ESweepType.FR, ref filteredTraffic, ref filteredGeneric);
                filteredHit = filteredTraffic || filteredGeneric;
            }
            finally
            {
                Complete(state);
                // Mirror postfix + finalizer cleanup for the direct probe as well:
                // a failed first disposal retains Scope and permits one safe retry.
                Complete(state);
            }
            return _healthy && TrafficLightPolicy.IsTrafficWait(readable, true, traffic, generic, filteredHit);
        }

        private static bool ReadSweep(VehicleAgent agent, VehicleAgent.ESweepType side, ref bool traffic, ref bool generic)
        {
            bool hit = agent.GetWheelSweepCollision(side, agent.vehicle.CurrentSteerAngle, false,
                out float distance, out UnityEngine.RaycastHit result);
            if (!hit) return true;
            if (!float.IsFinite(distance) || distance < 0f || !NetworkGuard.IsAlive(result.collider)) return false;
            var obstacle = result.collider.GetComponent<Il2CppScheduleOne.Vehicles.VehicleObstacle>();
            bool light = NetworkGuard.IsAlive(obstacle) &&
                obstacle.type == Il2CppScheduleOne.Vehicles.VehicleObstacle.EObstacleType.TrafficLight;
            traffic |= light;
            generic |= !light;
            return true;
        }

        private static void SweepPostfix(CallState? __state) => Complete(__state);
        private static Exception? SweepFinalizer(Exception? __exception, CallState? __state)
        {
            Complete(__state);
            return __exception;
        }

        private static void Complete(CallState? state)
        {
            if (state?.Scope == null) return;
            try
            {
                state.Scope.Dispose();
                state.Scope = null;
                if (state.Diagnose) Breadcrumb(state, "restored-original");
            }
            catch (Exception ex) { Fail(ex); }
        }

        private static void Breadcrumb(CallState state, string phase)
        {
            try
            {
                var flags = state.Agent.Flags;
                Mod.Log.Info($"[traffic-lights] order={SpikeState.NavOrder} phase={phase} originalFlags={state.OriginalPointer} currentFlags={flags?.Pointer} ignore={flags?.IgnoreTrafficLights}; only native TrafficLight hits excluded.");
            }
            catch { }
        }

        private static void Fail(Exception ex)
        {
            _healthy = false;
            _waiting = false;
            if (_errorLogged) return;
            _errorLogged = true;
            try { Mod.Log.Warn($"[traffic-lights] disabled on unreadable state: {ex.Message}; native behavior retained when restoration succeeds."); }
            catch { }
        }

        internal static void Remove()
        {
            _healthy = false;
            _waiting = false;
            _waitVehicle = IntPtr.Zero;
            _waitOrder = -1;
            _nextWaitProbe = 0f;
            _waitLogged = false;
            _probeDisabled = false;
            try { _harmony?.UnpatchSelf(); }
            catch (Exception ex) { Fail(ex); }
            finally { _harmony = null; _attempted = false; _errorLogged = false; _loggedOrder = -1; }
        }
    }
    // ==== end TaxiDriver section: TrafficLightPatch.cs ====
}

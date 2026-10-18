using System;
using System.Linq;
using HarmonyLib;
using Il2CppScheduleOne.Vehicles.AI;
using S1Mods.Shared;

namespace TaxiDriver;

/// <summary>
/// Installed f12 GetWheelSweepCollision (RVA 0xAD42D0): at 0xAD4C03..55
/// reads agent+0x30 / Flags+0x20; true skips ONLY VehicleObstacle.type==1
/// (TrafficLight) to the next hit. Generic hits still follow native filtering.
/// Both UpdateSweep and UpdateSpeedReduction call this synchronous consumer.
/// Swap a physically private copied flags object here, not at dispatch (patrol
/// resets flags) and not the speed result (would bypass native safety reductions).
/// </summary>
internal static class TrafficLightPatch
{
    private const string HarmonyId = "com.taxidriver.traffic-lights";
    private static HarmonyLib.Harmony? _harmony;
    private static bool _attempted;
    private static bool _healthy;
    private static bool _errorLogged;
    private static int _loggedOrder = -1;

    internal static void Apply()
    {
        if (_attempted) return;
        _attempted = true;
        try
        {
            _harmony = new HarmonyLib.Harmony(HarmonyId);
            var target = AccessTools.DeclaredMethod(typeof(VehicleAgent), nameof(VehicleAgent.GetWheelSweepCollision));
            bool patched = PatchGuard.TryPatch(_harmony, target,
                prefix: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepPrefix)) { priority = Priority.Last },
                postfix: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepPostfix)) { priority = Priority.First },
                finalizer: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepFinalizer)), log: Mod.Log);
            var info = target == null ? null : HarmonyLib.Harmony.GetPatchInfo(target);
            _healthy = patched && info != null &&
                info.Prefixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepPrefix)) &&
                info.Postfixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepPostfix)) &&
                info.Finalizers.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepFinalizer));
            Mod.Log.Info($"[traffic-lights] GetWheelSweepCollision scoped private flags hook healthy={_healthy}; owned autonomous host taxi only; generic obstacles/native braking retained. Native execution/gameplay unverified.");
        }
        catch (Exception ex) { Fail(ex); }
        if (!_healthy)
        {
            try { _harmony?.UnpatchSelf(); } catch { }
            Mod.Log.Warn("[traffic-lights] complete restoration hook unavailable; native traffic-light behavior retained.");
        }
    }

    private sealed class CallState
    {
        internal PrivateFlagsScope<DriveFlags>? Scope;
        internal VehicleAgent Agent = null!;
        internal IntPtr OriginalPointer;
        internal bool Diagnose;
    }

    private static void SweepPrefix(VehicleAgent __instance, out CallState? __state)
    {
        __state = null;
        if (!_healthy) return;
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected ||
                !SpikeCommands.HasAuthority()) return;
            var veh = __instance.vehicle;
            if (!SpikeCommands.IsOwnedTaxi(veh)) return;
            bool exact = veh.Agent != null && veh.Agent.Pointer == __instance.Pointer;
            if (!TrafficLightPolicy.ShouldIgnore(SpikeCommands.HasAuthority(), exact,
                    SpikeRunner.HasActuatorOrder, __instance.AutoDriving, veh.DriverPlayer != null)) return;
            var original = __instance.Flags;
            if (original == null || original.Pointer == IntPtr.Zero || original.WasCollected) return;
            var state = new CallState
            {
                Agent = __instance,
                OriginalPointer = original.Pointer,
                Diagnose = _loggedOrder != SpikeState.NavOrder
            };
            __state = state; // publish before any reference write, including partial failure
            PrivateFlagsScope<DriveFlags>.Begin(() => original, x => __instance.Flags = x,
                CopyFlags, (original, copy) => original.Pointer == copy.Pointer,
                scope => state.Scope = scope);
            if (state.Diagnose)
            {
                _loggedOrder = SpikeState.NavOrder;
                Breadcrumb(state, "consume-private-copy");
            }
        }
        catch (Exception ex)
        {
            Complete(__state);
            Fail(ex);
        }
    }

    private static DriveFlags CopyFlags(DriveFlags original)
    {
        // Do not call ResetFlags or touch original.IgnoreTrafficLights. Every other
        // installed native field is copied verbatim, including unusual settings.
        var copy = new DriveFlags
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
        return copy;
    }

    private static void SweepPostfix(CallState? __state) => Complete(__state);

    private static Exception? SweepFinalizer(Exception? __exception, CallState? __state)
    {
        Complete(__state);
        return __exception; // never suppress native/other-mod exceptions
    }

    private static void Complete(CallState? state)
    {
        if (state?.Scope == null) return;
        try
        {
            // No current authority/ownership/control gate: restore the exact prefix
            // snapshot even after STOP, redispatch, player takeover or native error.
            state.Scope.Dispose();
            state.Scope = null; // only after success; finalizer retries a failed write
            if (state.Diagnose) Breadcrumb(state, "restored-original");
        }
        catch (Exception ex) { Fail(ex); }
    }

    private static void Breadcrumb(CallState state, string phase)
    {
        try
        {
            var flags = state.Agent.Flags;
            Mod.Log.Info($"[traffic-lights] order={SpikeState.NavOrder} agent={state.Agent.Pointer} phase={phase} originalFlags={state.OriginalPointer} currentFlags={flags?.Pointer} ignore={flags?.IgnoreTrafficLights}; only native TrafficLight hits excluded.");
        }
        catch { /* Diagnostic reads must never prevent snapshot restoration. */ }
    }

    private static void Fail(Exception ex)
    {
        _healthy = false;
        if (_errorLogged) return;
        _errorLogged = true;
        try { Mod.Log.Warn($"[traffic-lights] disabled on unsafe/unreadable flags: {ex.Message}; native behavior retained when restoration succeeds."); }
        catch { }
    }

    internal static void Remove()
    {
        _healthy = false;
        try { _harmony?.UnpatchSelf(); }
        catch (Exception ex) { Fail(ex); }
        finally { _harmony = null; _attempted = false; _errorLogged = false; _loggedOrder = -1; }
        // No persistent agent/flags cache or order intent to survive teardown.
    }
}

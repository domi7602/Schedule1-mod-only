using System;
using System.Linq;
using System.Text;
using HarmonyLib;
using Il2CppScheduleOne.Vehicles;
using S1Mods.Shared;
using UnityEngine;

namespace TaxiDriver;

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
                Time.timeScale == 0f, playerDriver, SpikeState.NavReDispatchAt > 0f, active, parkIntent);
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

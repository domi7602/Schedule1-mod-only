using System;
using HarmonyLib;
using Il2CppScheduleOne.Vehicles.AI;
using S1Mods.Shared;

namespace TaxiDriver;

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
                reversing: __instance.IsReversing || SpikeState.NavReDispatchAt > 0f,
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

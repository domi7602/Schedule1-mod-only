using System;
using System.Collections.Generic;
using HarmonyLib;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.Vehicles;
using S1Mods.Shared;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Stage 3c ride locks for a passenger ride (the player has NO car inputs and the
/// trunk stays shut while riding):
/// <list type="bullet">
/// <item>
/// Input gate: PatchGuard prefixes on the PLAYER car-input path
/// (<c>LandVehicle.UpdateThrottle</c>/<c>UpdateSteerAngle</c>, fed by
/// <c>GameInput.VehicleDriveAxis</c>, and <c>GameInput.OnVehicleHandbrake</c>)
/// that skip the original when the player rides our vehicle without
/// <c>overrideControls</c>. The NPC drives through <c>VehicleAgent</c>'s OWN
/// <c>UpdateSpeed</c>/<c>UpdateSteering</c> (+ steerPID/throttlePID) — those
/// methods are not patched, so the gate provably cannot touch the NPC drive path.
/// </item>
/// <item>
/// Trunk gate: PatchGuard prefixes on <c>StorageDoorAnimation.Open</c> and
/// <c>SetIsOpen(true)</c> for the ride vehicle's <c>Trunk</c>, plus the
/// trunk/Storage child <c>InteractableObject</c> components switched off (the
/// vehicle's own entry interactable stays ON — the game's own E-enter is the
/// boarding path). <c>SetIsOpen(false)</c> is never blocked (closing is fine).
/// </item>
/// </list>
/// Every Harmony patch in this mod goes through <see cref="PatchGuard.TryPatch"/>;
/// state and gating all read <see cref="SpikeState"/> (single source of truth).
/// </summary>
internal static class RideLocks
{
    private static HarmonyLib.Harmony? _harmony;
    private static bool _applyAttempted;
    private static bool _inputGateErrorLogged;
    private static bool _trunkGateErrorLogged;
    private static RideProtectionPatch _installed;

    /// <summary>Trunk/Storage interactable components disabled for the current ride.</summary>
    private static readonly List<InteractableObject> TrunkInteractables = new();

    /// <summary>True only when every required input and trunk patch was installed.</summary>
    internal static bool Patched => RideProtectionPolicy.IsComplete(_installed);

    internal static RideProtectionPatch Installed => _installed;

    /// <summary>
    /// Installs the ride-lock patches once (called from <c>Mod.OnInitializeMelon</c>).
    /// A partial patch set is non-fatal to mod loading, but passenger rides are
    /// disabled because one successful patch is not complete input protection.
    /// </summary>
    internal static bool Apply()
    {
        if (_applyAttempted)
            return Patched;
        _applyAttempted = true;
        try
        {
            _harmony ??= new HarmonyLib.Harmony("com.taxidriver.ridelocks");

            bool throttle = PatchGuard.TryPatch(
                _harmony,
                typeof(LandVehicle),
                nameof(LandVehicle.UpdateThrottle),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(ThrottlePrefix)),
                log: Mod.Log);
            if (throttle) _installed |= RideProtectionPatch.Throttle;
            bool steer = PatchGuard.TryPatch(
                _harmony,
                typeof(LandVehicle),
                nameof(LandVehicle.UpdateSteerAngle),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(SteerPrefix)),
                log: Mod.Log);
            if (steer) _installed |= RideProtectionPatch.Steering;
            bool handbrake = PatchGuard.TryPatch(
                _harmony,
                typeof(GameInput),
                "OnVehicleHandbrake",
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(HandbrakePrefix)),
                log: Mod.Log);
            if (handbrake) _installed |= RideProtectionPatch.Handbrake;
            bool trunkOpen = PatchGuard.TryPatch(
                _harmony,
                typeof(StorageDoorAnimation),
                nameof(StorageDoorAnimation.Open),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(TrunkOpenPrefix)),
                log: Mod.Log);
            if (trunkOpen) _installed |= RideProtectionPatch.TrunkOpen;
            bool trunkSet = PatchGuard.TryPatch(
                _harmony,
                typeof(StorageDoorAnimation),
                nameof(StorageDoorAnimation.SetIsOpen),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(TrunkSetIsOpenPrefix)),
                log: Mod.Log);
            if (trunkSet) _installed |= RideProtectionPatch.TrunkSetOpen;

            if (Patched)
            {
                Mod.Log.Info(
                    $"[ride] complete input/trunk protection installed (throttle={throttle} steer={steer} handbrake={handbrake} " +
                    $"trunkOpen={trunkOpen} trunkSetIsOpen={trunkSet}) — the player-input gate only " +
                    "skips LandVehicle.UpdateThrottle/UpdateSteerAngle + GameInput.OnVehicleHandbrake, " +
                    "the VehicleAgent drive path (UpdateSpeed/UpdateSteering) is untouched.");
                return true;
            }

            Mod.Log.Warn(
                $"[ride] protection INCOMPLETE (installed={_installed}, missing={RideProtectionPolicy.Missing(_installed)}) — " +
                "CALL TAXI and passenger rides are disabled. See the failed signatures above.");
            return false;
        }
        catch (Exception ex)
        {
            Mod.Log.Error(
                $"[ride] RideLocks.Apply threw: {ex.GetType().Name} — {ex.Message}; installed={_installed}, " +
                $"missing={RideProtectionPolicy.Missing(_installed)}. Passenger rides remain disabled.");
            return false;
        }
    }

    /// <summary>
    /// True while the player rides <paramref name="veh"/> and the vehicle is NOT in
    /// agent-override mode — i.e. exactly when the PLAYER car-input path would move
    /// the ride vehicle. When <c>overrideControls</c> is set the vehicle reads
    /// <c>throttleOverride</c>/<c>steerOverride</c> anyway, and the NPC drive path
    /// never runs through the patched methods at all.
    /// </summary>
    private static bool PlayerInputBlocked(LandVehicle? veh)
    {
        bool passengerRideMode = SpikeState.RidePassengerMode;
        if (!passengerRideMode)
            return false;

        try
        {
            LandVehicle? tracked = SpikeState.Vehicle;
            bool ownedRideVehicleVerified = veh != null
                && tracked != null
                && SpikeCommands.IsOwnedTaxi(veh)
                && veh.Pointer == tracked.Pointer;
            bool overrideControls = veh != null && veh.overrideControls;
            return RideProtectionPolicy.ShouldBlockPlayerInput(
                passengerRideMode,
                ownedRideVehicleVerified,
                overrideControls);
        }
        catch (Exception ex)
        {
            if (!_inputGateErrorLogged)
            {
                _inputGateErrorLogged = true;
                try { Mod.Log.Error($"[ride] player-input gate failed closed: {ex.GetType().Name} — {ex.Message}"); }
                catch { /* Harmony prefixes must never let logging reopen the input path. */ }
            }
            return RideProtectionPolicy.ShouldBlockPlayerInput(
                passengerRideMode,
                ownedRideVehicleVerified: false,
                overrideControls: false);
        }
    }

    /// <summary>Blocks the ride trunk and any door whose identity cannot be verified during a passenger ride.</summary>
    private static bool TrunkBlocked(StorageDoorAnimation? door)
    {
        bool passengerRideMode = SpikeState.RidePassengerMode;
        if (!passengerRideMode)
            return false;

        try
        {
            LandVehicle? tracked = SpikeState.Vehicle;
            StorageDoorAnimation? trunk = tracked?.Trunk;
            bool ownedTrunkVerified = tracked != null
                && SpikeCommands.IsOwnedTaxi(tracked)
                && trunk != null
                && trunk.Pointer != IntPtr.Zero;
            bool doorIdentityVerified = door != null && door.Pointer != IntPtr.Zero;
            bool isRideTrunk = ownedTrunkVerified
                && doorIdentityVerified
                && door!.Pointer == trunk!.Pointer;
            return RideProtectionPolicy.ShouldBlockTrunkOpen(
                passengerRideMode,
                ownedTrunkVerified,
                doorIdentityVerified,
                isRideTrunk);
        }
        catch (Exception ex)
        {
            if (!_trunkGateErrorLogged)
            {
                _trunkGateErrorLogged = true;
                try { Mod.Log.Error($"[ride] trunk-open gate failed closed: {ex.GetType().Name} — {ex.Message}"); }
                catch { /* Harmony prefixes must never let logging reopen the trunk. */ }
            }
            return RideProtectionPolicy.ShouldBlockTrunkOpen(
                passengerRideMode,
                ownedTrunkVerified: false,
                doorIdentityVerified: false,
                isRideTrunk: false);
        }
    }

    // ---- Harmony prefixes (PatchGuard.TryPatch targets) ----

    private static bool ThrottlePrefix(LandVehicle __instance) => !PlayerInputBlocked(__instance);

    private static bool SteerPrefix(LandVehicle __instance) => !PlayerInputBlocked(__instance);

    private static bool HandbrakePrefix() =>
        !RideProtectionPolicy.ShouldBlockHandbrake(SpikeState.RidePassengerMode);

    private static bool TrunkOpenPrefix(StorageDoorAnimation __instance) => !TrunkBlocked(__instance);

    private static bool TrunkSetIsOpenPrefix(StorageDoorAnimation __instance, bool __0) => !(__0 && TrunkBlocked(__instance));

    /// <summary>
    /// Disables the trunk/Storage child <c>InteractableObject</c> components of the
    /// ride vehicle (so the "open trunk" prompt never appears). The vehicle's own
    /// entry interactable (<c>LandVehicle.intObj</c>) is explicitly kept alive —
    /// the game's own E-enter is the boarding path. Idempotent; paired with
    /// <see cref="UnlockTrunk"/>.
    /// </summary>
    internal static void LockTrunk(LandVehicle veh)
    {
        if (!SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh))
        {
            Mod.Log.Warn("[ride] trunk lock refused: vehicle is not a live TaxiDriver-owned taxi on the host.");
            return;
        }
        PruneDeadTrunkEntries();
        if (TrunkInteractables.Count > 0)
            return;
        try
        {
            Transform? trunkRoot = null;
            Transform? storageRoot = null;
            try
            {
                StorageDoorAnimation? trunk = veh.Trunk;
                if (trunk != null && trunk.Pointer != IntPtr.Zero)
                    trunkRoot = trunk.transform;
            }
            catch (Exception ex) { Mod.Log.Warn($"[ride] Trunk read failed: {ex.Message}"); }

            try
            {
                StorageEntity? storage = veh.Storage;
                if (storage != null && storage.Pointer != IntPtr.Zero)
                    storageRoot = storage.transform;
            }
            catch (Exception ex) { Mod.Log.Warn($"[ride] Storage read failed: {ex.Message}"); }

            InteractableObject? entry = null;
            try { entry = veh.intObj; } catch { /* dead object — fall through */ }

            InteractableObject[] all;
            try
            {
                all = veh.gameObject.GetComponentsInChildren<InteractableObject>(true);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[ride] trunk interactable scan failed: {ex.Message}");
                return;
            }

            foreach (InteractableObject? io in all)
            {
                if (io == null || io.Pointer == IntPtr.Zero)
                    continue;
                if (entry != null && io.Pointer == entry.Pointer)
                    continue; // the game's own vehicle-entry interactable must stay ON

                bool underTrunk = trunkRoot != null && io.transform.IsChildOf(trunkRoot);
                bool underStorage = storageRoot != null && io.transform.IsChildOf(storageRoot);
                string name = string.Empty;
                try { name = io.gameObject.name; } catch { /* name-less dead object */ }
                bool namedTrunk = name.Contains("Trunk", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("Storage", StringComparison.OrdinalIgnoreCase);
                if (!underTrunk && !underStorage && !namedTrunk)
                    continue;

                try
                {
                    if (!io.enabled)
                        continue;
                    io.enabled = false;
                    TrunkInteractables.Add(io);
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"[ride] disabling interactable '{name}' failed: {ex.Message}");
                }
            }

            Mod.Log.Info(
                $"[ride] trunk locked: {TrunkInteractables.Count} InteractableObject component(s) disabled " +
                "under Trunk/Storage (door gate on StorageDoorAnimation.Open/SetIsOpen(true) is armed); " +
                "the vehicle-entry interactable stays enabled.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[ride] trunk lock failed ({ex.Message}) — the StorageDoorAnimation gate still blocks opens.");
        }
    }

    /// <summary>
    /// Drops entries whose object died with a scene unload (a bare count check
    /// would mistake them for a live lock and skip the new one). Anything
    /// unreadable is dropped, never trusted.
    /// </summary>
    private static void PruneDeadTrunkEntries()
    {
        for (int i = TrunkInteractables.Count - 1; i >= 0; i--)
        {
            bool dead;
            try
            {
                dead = TrunkInteractables[i] == null;
            }
            catch
            {
                dead = true;
            }
            if (dead)
                TrunkInteractables.RemoveAt(i);
        }
    }

    /// <summary>Re-enables every interactable <see cref="LockTrunk"/> disabled. Idempotent.</summary>
    internal static void UnlockTrunk()
    {
        if (TrunkInteractables.Count == 0)
            return;
        int restored = 0;
        foreach (InteractableObject? io in TrunkInteractables)
        {
            try
            {
                if (io != null && io.Pointer != IntPtr.Zero && !io.enabled)
                {
                    io.enabled = true;
                    restored++;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[ride] restoring a trunk interactable failed: {ex.Message}");
            }
        }
        TrunkInteractables.Clear();
        Mod.Log.Info($"[ride] trunk unlocked: {restored} InteractableObject component(s) re-enabled.");
    }
}

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

    /// <summary>Trunk/Storage interactable components disabled for the current ride.</summary>
    private static readonly List<InteractableObject> TrunkInteractables = new();

    internal static bool Patched { get; private set; }

    /// <summary>
    /// Installs the ride-lock patches once (called from <c>Mod.OnInitializeMelon</c>).
    /// A failed patch is non-fatal: the ride still works, only its lock is absent
    /// and the log says so (PatchGuard prints the failed signature).
    /// </summary>
    internal static bool Apply()
    {
        if (Patched)
            return true;
        try
        {
            _harmony ??= new HarmonyLib.Harmony("com.taxidriver.ridelocks");

            bool throttle = PatchGuard.TryPatch(
                _harmony,
                typeof(LandVehicle),
                nameof(LandVehicle.UpdateThrottle),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(ThrottlePrefix)),
                log: Mod.Log);
            bool steer = PatchGuard.TryPatch(
                _harmony,
                typeof(LandVehicle),
                nameof(LandVehicle.UpdateSteerAngle),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(SteerPrefix)),
                log: Mod.Log);
            bool handbrake = PatchGuard.TryPatch(
                _harmony,
                typeof(GameInput),
                "OnVehicleHandbrake",
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(HandbrakePrefix)),
                log: Mod.Log);
            bool trunkOpen = PatchGuard.TryPatch(
                _harmony,
                typeof(StorageDoorAnimation),
                nameof(StorageDoorAnimation.Open),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(TrunkOpenPrefix)),
                log: Mod.Log);
            bool trunkSet = PatchGuard.TryPatch(
                _harmony,
                typeof(StorageDoorAnimation),
                nameof(StorageDoorAnimation.SetIsOpen),
                prefix: new HarmonyMethod(typeof(RideLocks), nameof(TrunkSetIsOpenPrefix)),
                log: Mod.Log);

            Patched = throttle || steer || handbrake || trunkOpen || trunkSet;
            if (Patched)
            {
                Mod.Log.Info(
                    $"RideLocks patched (throttle={throttle} steer={steer} handbrake={handbrake} " +
                    $"trunkOpen={trunkOpen} trunkSetIsOpen={trunkSet}) — the player-input gate only " +
                    "skips LandVehicle.UpdateThrottle/UpdateSteerAngle + GameInput.OnVehicleHandbrake, " +
                    "the VehicleAgent drive path (UpdateSpeed/UpdateSteering) is untouched.");
                return true;
            }

            Mod.Log.Warn(
                "RideLocks patch failed (all 5 signatures unknown) — the passenger ride runs WITHOUT " +
                "input/trunk locks. See the failed signatures above.");
            return false;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"RideLocks.Apply threw: {ex.GetType().Name} — {ex.Message}");
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
        try
        {
            return SpikeState.RidePassengerMode
                && veh != null
                && SpikeState.Vehicle != null
                && veh.Pointer == SpikeState.Vehicle.Pointer
                && !veh.overrideControls;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>True while <paramref name="door"/> is the ride vehicle's trunk during a ride.</summary>
    private static bool TrunkBlocked(StorageDoorAnimation? door)
    {
        try
        {
            return SpikeState.RidePassengerMode
                && door != null
                && SpikeState.Vehicle != null
                && SpikeState.Vehicle.Trunk != null
                && door.Pointer == SpikeState.Vehicle.Trunk.Pointer;
        }
        catch
        {
            return false;
        }
    }

    // ---- Harmony prefixes (PatchGuard.TryPatch targets) ----

    private static bool ThrottlePrefix(LandVehicle __instance) => !PlayerInputBlocked(__instance);

    private static bool SteerPrefix(LandVehicle __instance) => !PlayerInputBlocked(__instance);

    private static bool HandbrakePrefix() => !PlayerInputBlocked(SpikeState.Vehicle);

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

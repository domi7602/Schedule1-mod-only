using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using SnackVendor.Config;
using SnackVendor.Items;
using SnackVendor.World;
using SnackVendor.Persistence;
using UnityEngine;
using ConfigInstance = S1Mods.Shared.ModConfig<SnackVendor.Config.SnackVendorConfig>;

// (MelonInfo/MelonGame attributes intentionally absent — our MelonMod registration is
// auto-discovered by MelonLoader's [MelonInfo] attribute scanning on the Mod class; we
// put them inline next to the class so the asmversion travels together with the class.
[assembly: MelonInfo(typeof(SnackVendor.Mod), "SnackVendor", "0.0.9", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace SnackVendor;

/// <summary>
/// SnackVendor — platzierbare Vending-Maschine, die der Spieler selbst mit
/// Zutaten (Gas-Markt) befüllt. NPCs kaufen daraus zum Vanilla-Marktpreis,
/// der Erlös geht als Cash an den Spieler. Siehe docs/SPEC.md.
/// </summary>
public sealed class Mod : MelonMod
{
    public static readonly ModLogger Log = new("SnackVendor");

    public static SnackVendorConfig CurrentConfig => ConfigInstance.Instance;

    public override void OnInitializeMelon()
    {
        try
        {
            ModConfig<SnackVendorConfig>.Initialize("SnackVendor", Log);
            // First-write default values to disk so the user has a stable
            // starting point.
            ModConfig<SnackVendorConfig>.Save();
            Log.Info("SnackVendor initialized (config live).");
        }
        catch (Exception ex)
        {
            Log.Warn("config init failed; using defaults", ex);
        }

        // 1. Register IL2CPP types for our MonoBehaviours (vanilla-side hooks).
        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<SnackVendorController>();
            ClassInjector.RegisterTypeInIl2Cpp<SnackVendorMarker>();
            ClassInjector.RegisterTypeInIl2Cpp<World.Outdoor.SnackVendorOutdoorInteractable>();
            Log.Info("Registered SnackVendorController + SnackVendorMarker + SnackVendorOutdoorInteractable in IL2CPP.");
        }
        catch (Exception ex)
        {
            Log.Error("IL2CPP type registration failed", ex);
        }

        // 2. Apply Harmony patches (Audit 0.0.3: explicit PatchGuard.TryPatch
        // calls instead of PatchClassProcessor so signature drift shows up
        // in PatchGuard.Report instead of silently leaving the station dead).
        try
        {
            var harmony = HarmonyInstance;

            // BuildableItem.Start postfix → controller setup on placement
            PatchGuard.TryPatch(
                harmony,
                original: AccessTools.Method(typeof(Il2CppScheduleOne.EntityFramework.BuildableItem), "Start"),
                postfix: new HarmonyMethod(typeof(BuildableItem_Start_Patch), nameof(BuildableItem_Start_Patch.Postfix)),
                log: Log);

            // VendingMachine.SendPurchase prefix → marker-gated stock decrement + cash credit
            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.ObjectScripts.VendingMachine),
                methodName: "SendPurchase",
                prefix: new HarmonyMethod(typeof(VendingMachinePatches), nameof(VendingMachinePatches.SendPurchase_Prefix)),
                log: Log);

            // VendingMachine.PurchaseRoutine prefix → suppress the vanilla cuke-dispense anim
            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.ObjectScripts.VendingMachine),
                methodName: "PurchaseRoutine",
                prefix: new HarmonyMethod(typeof(VendingMachinePatches), nameof(VendingMachinePatches.PurchaseRoutine_Prefix)),
                log: Log);

            // VendingMachine.DropItem prefix → no cuke on the floor
            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.ObjectScripts.VendingMachine),
                methodName: "DropItem",
                prefix: new HarmonyMethod(typeof(VendingMachinePatches), nameof(VendingMachinePatches.DropItem_Prefix)),
                log: Log);

            // VendingMachine.DropCash prefix → no prefab cash-spit (we credit directly)
            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.ObjectScripts.VendingMachine),
                methodName: "DropCash",
                prefix: new HarmonyMethod(typeof(VendingMachinePatches), nameof(VendingMachinePatches.DropCash_Prefix)),
                log: Log);

            // VendingMachine.Interacted prefix → deposit/extract panel instead
            // of the vanilla pay-UI on OUR machines (vanilla machines untouched).
            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.ObjectScripts.VendingMachine),
                methodName: "Interacted",
                prefix: new HarmonyMethod(typeof(VendingMachinePatches), nameof(VendingMachinePatches.Interacted_Prefix)),
                log: Log);

            // NPCSignal_UseVendingMachine.Purchase prefix → capture the buying
            // NPC so SendPurchase can credit the ingredient into NPC.Inventory.
            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachine),
                methodName: "Purchase",
                prefix: new HarmonyMethod(typeof(NPCSignalPatches), nameof(NPCSignalPatches.Purchase_Prefix)),
                log: Log);

            // --- Standalone Outdoor Building Patches ---
            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.Building.BuildUpdate_Grid),
                methodName: "CheckIntersections",
                postfix: new HarmonyMethod(typeof(World.Outdoor.SnackVendorBuildPatches.BuildUpdate_Grid_CheckIntersections_Patch), nameof(World.Outdoor.SnackVendorBuildPatches.BuildUpdate_Grid_CheckIntersections_Patch.Postfix)),
                log: Log);

            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.Building.BuildUpdate_Grid),
                methodName: "Place",
                prefix: new HarmonyMethod(typeof(World.Outdoor.SnackVendorBuildPatches.BuildUpdate_Grid_Place_Patch), nameof(World.Outdoor.SnackVendorBuildPatches.BuildUpdate_Grid_Place_Patch.Prefix)),
                log: Log);

            PatchGuard.TryPatch(
                harmony,
                original: AccessTools.Method(typeof(Il2CppScheduleOne.EntityFramework.BuildableItem), "Start"),
                prefix: new HarmonyMethod(typeof(World.Outdoor.SnackVendorGuardPatches.BuildableItem_Start_Guard), nameof(World.Outdoor.SnackVendorGuardPatches.BuildableItem_Start_Guard.Prefix)),
                log: Log);

            PatchGuard.TryPatch(
                harmony,
                original: AccessTools.Method(typeof(Il2CppScheduleOne.EntityFramework.BuildableItem), nameof(Il2CppScheduleOne.EntityFramework.BuildableItem.SetCulled)),
                prefix: new HarmonyMethod(typeof(World.Outdoor.SnackVendorGuardPatches.BuildableItem_SetCulled_Guard), nameof(World.Outdoor.SnackVendorGuardPatches.BuildableItem_SetCulled_Guard.Prefix)),
                log: Log);

            PatchGuard.TryPatch(
                harmony,
                targetType: typeof(Il2CppScheduleOne.EntityFramework.GridItem),
                methodName: "Destroy",
                prefix: new HarmonyMethod(typeof(World.Outdoor.SnackVendorGuardPatches.GridItem_Destroy_Guard), nameof(World.Outdoor.SnackVendorGuardPatches.GridItem_Destroy_Guard.Prefix)),
                log: Log);

            PatchGuard.Report(Log);
        }
        catch (Exception ex)
        {
            Log.Error("Harmony patch failed", ex);
        }

        // 3. Subscribe to S1API Lifecycle events.
        try
        {
            GameLifecycle.OnSaveInfoLoaded += SnackVendorItemFactory.RegisterItem;
            GameLifecycle.OnSaveInfoLoaded += SnackVendorItemFactory.InjectHardwareStoreListing;
            GameLifecycle.OnLoadComplete += SnackVendorItemFactory.InjectHardwareStoreListing;

            // Storage slot-resolution: S1API exposes the current save slot via
            // PlayerData events; if not, our store stays in UnknownSlot and
            // Save/Load become no-ops rather than writing to the wrong file.
            GameLifecycle.OnLoadComplete += ResolveAndStoreSlot;
            GameLifecycle.OnLoadComplete += World.Outdoor.SnackVendorOutdoorManager.LoadAndSpawnOutdoorStations;
            GameLifecycle.OnSaveComplete += PersistAllStationSlots;
            GameLifecycle.OnSaveComplete += World.Outdoor.SnackVendorOutdoorManager.SaveOutdoorStations;
            GameLifecycle.OnPreLoad += ResetStoredSlot;
            GameLifecycle.OnPreLoad += World.Outdoor.SnackVendorOutdoorManager.ResetState;
        }
        catch (Exception ex)
        {
            Log.Warn("lifecycle subscribe failed", ex);
        }
    }

    public override void OnDeinitializeMelon()
    {
        try
        {
            GameLifecycle.OnSaveInfoLoaded -= SnackVendorItemFactory.RegisterItem;
            GameLifecycle.OnSaveInfoLoaded -= SnackVendorItemFactory.InjectHardwareStoreListing;
            GameLifecycle.OnLoadComplete -= SnackVendorItemFactory.InjectHardwareStoreListing;
            GameLifecycle.OnLoadComplete -= ResolveAndStoreSlot;
            GameLifecycle.OnLoadComplete -= World.Outdoor.SnackVendorOutdoorManager.LoadAndSpawnOutdoorStations;
            GameLifecycle.OnSaveComplete -= PersistAllStationSlots;
            GameLifecycle.OnSaveComplete -= World.Outdoor.SnackVendorOutdoorManager.SaveOutdoorStations;
            GameLifecycle.OnPreLoad -= ResetStoredSlot;
            GameLifecycle.OnPreLoad -= World.Outdoor.SnackVendorOutdoorManager.ResetState;
        }
        catch (Exception ex)
        {
            Log.Warn("lifecycle unsubscribe failed", ex);
        }

        // Audit 0.0.3: free the ghost-prefab Texture/Sprite/GameObject/Material
        // caches so they don't outlive the mod on hot-reload or app quit.
        BuildOrLoadGhostPrefab.Dispose();
    }

    /// <summary>On scene-load, re-register the item (catch resets from patch-days).</summary>
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        // Scene switch: drop panel state + stale NPC-capture entries.
        try
        {
            World.SnackVendorPanel.Close();
            World.NPCSignalPatches.ClearPending();
        }
        catch (Exception ex)
        {
            Log.Warn("scene-reset failed", ex);
        }

        if (sceneName == "Main")
        {
            try
            {
                SnackVendorItemFactory.RegisterItem();
                SnackVendorItemFactory.InjectHardwareStoreListing();
            }
            catch (Exception ex)
            {
                Log.Warn("scene-hook OnSceneWasLoaded failed", ex);
            }
        }
    }

    /// <summary>Panel housekeeping (auto-close on distance/scene/dead station).</summary>
    public override void OnUpdate()
    {
        try { World.SnackVendorPanel.Update(); }
        catch { /* never let the panel tick break the game loop */ }
    }

    /// <summary>IMGUI draw for hover prompt and deposit/extract panel.</summary>
    public override void OnGUI()
    {
        try
        {
            World.SnackVendorController.DrawPrompt();
            World.SnackVendorPanel.Draw();
        }
        catch { /* a GUI exception must never escape into MelonLoader */ }
    }

    private void ResolveAndStoreSlot()
    {
        // Sonde: S1Mods.Shared.SaveSlots (Konsolidierung 2026-09-15) — probiert
        // PersistentSingleton/LoadManager.Instance/Singleton (früher zwei Blöcke hier).
        int slot = SaveSlots.GetActiveSlotNumber();
        if (slot >= 0)
        {
            SnackVendor.Persistence.SnackVendorStore.OnSaveSlotResolved(slot);
            Log.Info($"Resolved and stored save slot: {slot}");
        }
    }

    private void ResetStoredSlot() { SnackVendor.Persistence.SnackVendorStore.OnSaveSlotReset(); }
    private void PersistAllStationSlots()
    {
        // Iterate live controllers and ask each to flush its slots, then
        // prune sidecar entries whose GUID has no live controller anymore
        // (v0.0.6: stations packed up via the street-item flow leave an
        // orphan entry behind — on the next save they are dropped, so a
        // packed-up station cannot "respawn" its stock on a later GUID).
        try
        {
            if (!SnackVendorStore.IsSlotResolved) return;
            var live = UnityEngine.Object.FindObjectsOfType<SnackVendorController>();
            var liveGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var c in live)
            {
                if (c == null || c.Pointer == IntPtr.Zero) continue;
                try { c.PersistSlotsToDisk(); }
                catch (Exception ex) { Log.Warn($"save-flush for {c.InstanceGuid}", ex); }
                if (!string.IsNullOrEmpty(c.InstanceGuid)) liveGuids.Add(c.InstanceGuid);
            }

            try
            {
                var all = SnackVendorStore.Load();
                if (all?.Stations != null && all.Stations.Count > 0)
                {
                    bool removed = false;
                    for (int i = all.Stations.Count - 1; i >= 0; i--)
                    {
                        var guid = all.Stations[i]?.InstanceGuid;
                        if (string.IsNullOrEmpty(guid) || !liveGuids.Contains(guid))
                        {
                            all.Stations.RemoveAt(i);
                            removed = true;
                        }
                    }
                    if (removed) SnackVendorStore.Save(all);
                }
            }
            catch (Exception ex) { Log.Warn("sidecar orphan-prune failed", ex); }
        }
        catch (Exception ex)
        {
            Log.Warn("save-flush failed", ex);
        }
    }
}

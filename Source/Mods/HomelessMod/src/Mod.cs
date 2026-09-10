using System;
using HarmonyLib;
using HomelessMod.Building;
using HomelessMod.Config;
using HomelessMod.Items;
using HomelessMod.Quests;
using Il2CppInterop.Runtime.Injection;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(HomelessMod.Mod), "HomelessMod", "0.1.6", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace HomelessMod;

public sealed class Mod : MelonMod
{
    public static readonly ModLogger Log = new("HomelessMod");
    private static readonly HomelessConfig _fallbackConfig = new();
    private static bool _isConfigInitialized = false;
    private float _nextCashCheckTime = 0f;
    private bool _streetItemsLoaded;

    public static HomelessConfig CurrentConfig
    {
        get
        {
            if (!_isConfigInitialized)
            {
                try
                {
                    ModConfig<HomelessConfig>.Initialize("HomelessMod");
                    _isConfigInitialized = true;
                }
                catch
                {
                    return _fallbackConfig;
                }
            }
            try
            {
                return ModConfig<HomelessConfig>.Instance;
            }
            catch
            {
                return _fallbackConfig;
            }
        }
    }

    public override void OnInitializeMelon()
    {
        try
        {
            ModConfig<HomelessConfig>.Initialize("HomelessMod");
            _isConfigInitialized = true;
        }
        catch (Exception ex)
        {
            Log.Warn($"Early config init: {ex.Message}");
        }

        Log.Info($"Initializing Homeless Mod v0.1.3 (Everywhere Building: {CurrentConfig.EnableEverywhereBuilding}, Snap: {CurrentConfig.OutdoorGridSnapSize}m)");

        // 1. Register IL2CPP Types
        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<SleepingBagInteractable>();
            ClassInjector.RegisterTypeInIl2Cpp<OutdoorItemInteractable>();
            ClassInjector.RegisterTypeInIl2Cpp<HomelessInputFocus>();
            Log.Info("Registered SleepingBagInteractable, OutdoorItemInteractable & HomelessInputFocus in IL2CPP runtime.");
            HomelessInputFocus.EnsureAttached(this);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to register custom types in IL2CPP runtime: {ex.Message}");
        }

        // 2. Apply Harmony Patches safely
        ApplyHarmonyPatches();

        // 3. Subscribe to S1API Lifecycle events
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        GameLifecycle.OnSaveComplete += OnSaveComplete;

        // Review-fix 2026-09-09 (v0.1.5): wake-confirmed quest credit — subscribe once here.
        S1API.GameTime.TimeManager.OnSleepEnd += HomelessQuestManager.OnSleepEnded;
    }

    public override void OnDeinitializeMelon()
    {
        // Gatekeeper-fix 2026-08-30 L8: unpatch Harmony patches on unload.
        try { HarmonyInstance.UnpatchSelf(); } catch (Exception ex) { Log.Debug($"Harmony unpatch failed: {ex.Message}"); }
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        GameLifecycle.OnSaveComplete -= OnSaveComplete;
        S1API.GameTime.TimeManager.OnSleepEnd -= HomelessQuestManager.OnSleepEnded;
        StreetPropertyManager.ResetState();
        HomelessQuestManager.ResetState();
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        // Symmetric cleanup: if the active gameplay scene is unloaded, drop references
        // to destroyed world objects so a fresh load can repopulate them correctly.
        if (IsGameplayScene(sceneName))
        {
            StreetPropertyManager.ResetForSceneUnload();
            HomelessQuestManager.ResetForSceneUnload(); // Review-fix 2026-09-09 (v0.1.5): keep slot on same-slot reload
            _streetItemsLoaded = false;
        }
    }

    private static bool IsGameplayScene(string sceneName)
    {
        return !string.IsNullOrEmpty(sceneName) && sceneName == "Main";
    }

    public override void OnUpdate()
    {
        // Gatekeeper-fix 2026-08-30 L3: skip polling when quests disabled
        if (!CurrentConfig.EnableHomelessQuests) return;
        // Periodic check for quest cash goals (every 2.5s)
        if (Time.unscaledTime >= _nextCashCheckTime)
        {
            _nextCashCheckTime = Time.unscaledTime + 2.5f;
            HomelessQuestManager.CheckCashProgress();
        }
    }

    private void ApplyHarmonyPatches()
    {
        try
        {
            // BuildUpdate_Grid CheckIntersections
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(BuildUpdate_Grid),
                nameof(BuildUpdate_Grid.CheckIntersections),
                postfix: new HarmonyMethod(typeof(BuildingPatches.BuildUpdate_Grid_CheckIntersections_Patch), nameof(BuildingPatches.BuildUpdate_Grid_CheckIntersections_Patch.Postfix)),
                log: Log);

            // BuildUpdate_Grid Place
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(BuildUpdate_Grid),
                nameof(BuildUpdate_Grid.Place),
                prefix: new HarmonyMethod(typeof(BuildingPatches.BuildUpdate_Grid_Place_Patch), nameof(BuildingPatches.BuildUpdate_Grid_Place_Patch.Prefix)),
                log: Log);

            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Il2CppScheduleOne.EntityFramework.GridItem),
                nameof(Il2CppScheduleOne.EntityFramework.GridItem.Destroy),
                prefix: new HarmonyMethod(typeof(BuildingPatches.GridItem_Destroy_Patch), nameof(BuildingPatches.GridItem_Destroy_Patch.Prefix)),
                log: Log);

            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Il2CppScheduleOne.EntityFramework.BuildableItem),
                nameof(Il2CppScheduleOne.EntityFramework.BuildableItem.Destroy),
                prefix: new HarmonyMethod(typeof(BuildingPatches.BuildableItem_Destroy_Patch), nameof(BuildingPatches.BuildableItem_Destroy_Patch.Prefix)),
                log: Log);

            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Il2CppScheduleOne.EntityFramework.BuildableItem),
                nameof(Il2CppScheduleOne.EntityFramework.BuildableItem.SetCulled),
                prefix: new HarmonyMethod(typeof(BuildingPatches.BuildableItem_SetCulled_Patch), nameof(BuildingPatches.BuildableItem_SetCulled_Patch.Prefix)),
                log: Log);

            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Il2CppScheduleOne.EntityFramework.BuildableItem),
                "Start",
                prefix: new HarmonyMethod(typeof(BuildingPatches.BuildableItem_Start_Patch), nameof(BuildingPatches.BuildableItem_Start_Patch.Prefix)),
                log: Log);
        }
        catch (Exception ex)
        {
            Log.Error($"Error applying harmony patches: {ex.Message}");
        }
    }

    private void OnPreLoad()
    {
        // Gatekeeper-fix 2026-08-29: cache slot resolution REMOVED from OnPreLoad because
        // loadMgr.ActiveSaveInfo is NOT yet guaranteed to reflect the new slot here (S1API
        // GameLifecycle ordering: OnPreLoad fires before the save-info swap in some paths).
        // Compare-only here, cache-only in OnSaveInfoLoaded where the new slot is authoritative.
        // Slot-switch detection: read the slot that is about to be loaded BEFORE we reset anything.
        // OnPreLoad fires both for real save-slot switches (New Game / Continue on different slot)
        // AND for same-slot scene reloads (Menu -> Game). We must NOT kill placed street items
        // on a same-slot reload, or the player's progress disappears every time they leave the menu.
        int oldSlot = StreetPropertyManager.LastKnownSlotNumber;
        int newSlot = ResolveActiveSaveSlotNumber();
        int itemsBefore = StreetPropertyManager.ActiveStreetItemCount;
        bool isFirstLoad = (oldSlot == -2);
        // Both oldSlot and newSlot may be -1 here on the very first PreLoad before any save is
        // ever resolved — that's fine, isFirstLoad (oldSlot == -2) still triggers a clean reset.
        bool isSlotSwitch = isFirstLoad || (oldSlot != newSlot);

        if (isSlotSwitch)
        {
            string branch = "ResetState(full-wipe)";
            Log.Info($"[OnPreLoad] oldSlot={oldSlot} newSlot={newSlot} isSlotSwitch=True isFirstLoad={isFirstLoad} itemsBefore={itemsBefore} branch={branch}");
            StreetPropertyManager.ResetState();
        }
        else
        {
            string branch = "ResetForSceneUnload(keep-slot)";
            Log.Info($"[OnPreLoad] oldSlot={oldSlot} newSlot={newSlot} isSlotSwitch=False itemsBefore={itemsBefore} branch={branch}");
            StreetPropertyManager.ResetForSceneUnload();
        }

        HomelessQuestManager.ResetState();
        _streetItemsLoaded = false;
    }

    private static int ResolveActiveSaveSlotNumber()
    {
        try
        {
            var loadMgr = PersistentSingleton<LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
            {
                var saveInfo = loadMgr.ActiveSaveInfo;
                if (saveInfo != null && saveInfo.Pointer != IntPtr.Zero && !saveInfo.WasCollected)
                {
                    return saveInfo.SaveSlotNumber; // -1 = no slot / main menu
                }
            }
        }
        catch (Exception ex)
        {
            Log.Debug($"ResolveActiveSaveSlotNumber fallback: {ex.Message}");
        }
        return -1;
    }

    private void OnSaveInfoLoaded()
    {
        SleepingBagItemFactory.RegisterItem();

        // Diagnostic: report the active slot at save-info time so we can correlate
        // this hook with the OnPreLoad branch that just ran.
        int currentSlot = ResolveActiveSaveSlotNumber();
        Log.Info($"[OnSaveInfoLoaded] currentSlot={currentSlot}");

        // Gatekeeper-fix 2026-08-29: cache the now-authoritative slot here (OnPreLoad no longer
        // caches, because loadMgr.ActiveSaveInfo may be stale there). ActiveSaveInfo is
        // guaranteed to reflect the new slot by the time OnSaveInfoLoaded fires.
        if (currentSlot != -1)
        {
            StreetPropertyManager.CacheSlotNumber(currentSlot);
        }
    }

    private void OnLoadComplete()
    {
        SleepingBagItemFactory.RegisterItem();

        // OnLoadComplete can fire more than once per save-load cycle; only
        // restore street items and inject the store listing on the first pass.
        if (!_streetItemsLoaded)
        {
            _streetItemsLoaded = true;
            StreetPropertyManager.LoadAndSpawnStreetItems();
            SleepingBagItemFactory.InjectHardwareStoreListing();
        }

        // Diagnostic: report which JSON path was actually loaded from and how many items came back.
        // GetSaveFilePath() logs its own line above; we add the item-count here.
        try
        {
            string savePath = StreetPropertyManager.GetSaveFilePath();
            int itemsAfter = StreetPropertyManager.ActiveStreetItemCount;
            bool fileExists = System.IO.File.Exists(savePath);
            Log.Info($"[OnLoadComplete] resolvedSavePath='{System.IO.Path.GetFileName(savePath)}' jsonExists={fileExists} itemsLoaded={itemsAfter}");
        }
        catch (Exception ex)
        {
            Log.Debug($"OnLoadComplete diagnostic failed: {ex.Message}");
        }

        HomelessQuestManager.InitializeQuests();
        Log.Info("Homeless Mod fully activated in gameplay scene.");
    }

    private void OnSaveComplete()
    {
        StreetPropertyManager.SaveStreetItems();
        // Fix 2026-09-10: flush quest completions on the same save hook as the street-item
        // store instead of writing quest_progress_{slot}.json mid-gameplay on every trigger.
        HomelessQuestManager.FlushCompletedState();
    }
}

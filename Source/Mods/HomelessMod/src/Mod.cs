using System;
using HarmonyLib;
using HomelessMod.Building;
using HomelessMod.Config;
using HomelessMod.Items;
using HomelessMod.Quests;
using Il2CppInterop.Runtime.Injection;
using Il2CppScheduleOne.Building;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(HomelessMod.Mod), "HomelessMod", "0.1.1", "Dominik")]
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

        Log.Info($"Initializing Homeless Mod v0.1.1 (Everywhere Building: {CurrentConfig.EnableEverywhereBuilding}, Snap: {CurrentConfig.OutdoorGridSnapSize}m)");

        // 1. Register IL2CPP Types
        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<SleepingBagInteractable>();
            ClassInjector.RegisterTypeInIl2Cpp<OutdoorItemInteractable>();
            Log.Info("Registered SleepingBagInteractable & OutdoorItemInteractable in IL2CPP runtime.");
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
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        GameLifecycle.OnSaveComplete -= OnSaveComplete;
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
            HomelessQuestManager.ResetState();
            _streetItemsLoaded = false;
        }
    }

    private static bool IsGameplayScene(string sceneName)
    {
        return !string.IsNullOrEmpty(sceneName) && sceneName == "Main";
    }

    public override void OnUpdate()
    {
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
        StreetPropertyManager.ResetState();
        HomelessQuestManager.ResetState();
        _streetItemsLoaded = false;
    }

    private void OnSaveInfoLoaded()
    {
        SleepingBagItemFactory.RegisterItem();
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

        HomelessQuestManager.InitializeQuests();
        Log.Info("Homeless Mod fully activated in gameplay scene.");
    }

    private void OnSaveComplete()
    {
        StreetPropertyManager.SaveStreetItems();
    }
}

using System;
using AutoPackagingStation.Config;
using AutoPackagingStation.Entities;
using AutoPackagingStation.Items;
using AutoPackagingStation.Persistence;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.EntityFramework;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(AutoPackagingStation.Mod), "AutoPackagingStation", "0.4.7", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace AutoPackagingStation;

public sealed class Mod : MelonMod
{
    public static readonly ModLogger Log = new("AutoPackagingStation");
    private static readonly AutoPackConfig _fallbackConfig = new();
    private static bool _isConfigInitialized = false;

    public static AutoPackConfig CurrentConfig
    {
        get
        {
            if (!_isConfigInitialized)
            {
                try
                {
                    ModConfig<AutoPackConfig>.Initialize("AutoPackagingStation");
                    _isConfigInitialized = true;
                }
                catch
                {
                    return _fallbackConfig;
                }
            }
            try
            {
                return ModConfig<AutoPackConfig>.Instance;
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
            ModConfig<AutoPackConfig>.Initialize("AutoPackagingStation");
            _isConfigInitialized = true;
        }
        catch (Exception ex)
        {
            Log.Warn($"Early config init: {ex.Message}");
        }

        Log.Info($"Initializing AutoPackagingStation v{Info.Version} (Timer: {CurrentConfig.PackagingDurationSeconds}s, FreshnessBonus: {CurrentConfig.FreshnessBonusMultiplier:P0} upgrade chance)");

        // 1. Register IL2CPP Types (Critic Pillar 1: No direct inheritance of IL2CPP classes, pure MonoBehaviour with IntPtr ctor)
        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<AutoPackStationController>();
            Log.Info("Registered AutoPackStationController in IL2CPP runtime.");
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to register custom IL2CPP types: {ex.Message}");
        }

        // 2. Apply Harmony Patches safely via PatchGuard
        ApplyHarmonyPatches();

        // 1b. Validate the loaded configuration (bug-audit 2026-10-13, BUG-019).
        try
        {
            CurrentConfig.SanitizeInPlace(msg => Log.Warn(msg));
        }
        catch (Exception ex)
        {
            Log.Warn($"Config sanitization failed: {ex.Message}");
        }

        // 3. Subscribe to S1API Lifecycle Events (Critic Pillar 3: Persistence exclusively on GameLifecycle.OnSaveComplete)
        GameLifecycle.OnPreLoad += AutoPackStore.OnPreLoad;
        // Bug-audit 2026-10-13 (BUG-012): S1API documents OnPreLoad as THE point for item
        // registration — before any saved item data is loaded. The previous registration
        // (OnSaveInfoLoaded / Main scene) could run after the game tried to restore a
        // saved custom station. RegisterItem is idempotent and the later hooks stay as
        // fallbacks for registry resets.
        GameLifecycle.OnPreLoad += AutoPackagingItemFactory.RegisterItem;
        // 2026-10-14 (Setup-Hook feuert nicht): zweiter Auslösungspfad — S1API feuert
        // BuildEvents.OnGridItemCreated mit ItemId + GameObject, sobald S1API das GridItem
        // erzeugt. Robust gegen Start-Reihenfolge (ItemInstance dort gesetzt) und gegen
        // abweichende Prefab-Pfade (StoredItem/Equippable-Komposition).
        S1API.Building.BuildEvents.OnGridItemCreated += AutoPackBuildEventHandlers.OnGridItemCreated;
        GameLifecycle.OnSaveInfoLoaded += AutoPackagingItemFactory.OnSaveInfoLoaded;
        GameLifecycle.OnSaveInfoLoaded += AutoPackagingItemFactory.InjectHardwareStoreListing;
        GameLifecycle.OnLoadComplete += AutoPackStore.OnLoadComplete;
        // Bug-audit 2026-10-13 (BUG-002): deliver pending refunds after the store loaded them.
        GameLifecycle.OnLoadComplete += AutoPackStationController.DeliverPendingRefunds;
        GameLifecycle.OnLoadComplete += AutoPackagingItemFactory.InjectHardwareStoreListing;
        GameLifecycle.OnSaveComplete += AutoPackStore.OnSaveComplete;
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnPreLoad -= AutoPackStore.OnPreLoad;
        GameLifecycle.OnPreLoad -= AutoPackagingItemFactory.RegisterItem;
        S1API.Building.BuildEvents.OnGridItemCreated -= AutoPackBuildEventHandlers.OnGridItemCreated;
        GameLifecycle.OnSaveInfoLoaded -= AutoPackagingItemFactory.OnSaveInfoLoaded;
        GameLifecycle.OnSaveInfoLoaded -= AutoPackagingItemFactory.InjectHardwareStoreListing;
        GameLifecycle.OnLoadComplete -= AutoPackStore.OnLoadComplete;
        GameLifecycle.OnLoadComplete -= AutoPackStationController.DeliverPendingRefunds;
        GameLifecycle.OnLoadComplete -= AutoPackagingItemFactory.InjectHardwareStoreListing;
        GameLifecycle.OnSaveComplete -= AutoPackStore.OnSaveComplete;
        try { AutoPackStore.OnPreLoad(); }
        finally { AutoPackagingItemFactory.ShutdownPrefabCache(); }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main")
        {
            AutoPackagingItemFactory.RegisterItem();
        }
    }
    private void ApplyHarmonyPatches()
    {
        var harmony = HarmonyInstance;

        // Hook BuildableItem.Start to setup placed station visuals
        PatchGuard.TryPatch(
            harmony,
            original: AccessTools.Method(typeof(BuildableItem), "Start"),
            prefix: null,
            postfix: new HarmonyMethod(typeof(BuildableItem_Start_Patch), nameof(BuildableItem_Start_Patch.Postfix)),
            transpiler: null,
            finalizer: null,
            log: Log
        );

        PatchGuard.Report(Log);
    }
}

public static class BuildableItem_Start_Patch
{
    public static void Postfix(BuildableItem __instance)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.gameObject == null || __instance.gameObject.Pointer == IntPtr.Zero) return;
            var inst = __instance.ItemInstance;
            if (inst != null && inst.Pointer != IntPtr.Zero && inst.Definition != null && inst.Definition.ID == Mod.CurrentConfig.StationItemId)
            {
                AutoPackagingItemFactory.SetupPlacedStation(__instance.gameObject);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"BuildableItem_Start_Patch error: {ex.Message}");
        }
    }
}

/// <summary>
/// 2026-10-14 (Setup-Hook feuert nicht): S1API-Build-Event als zweiter Auslösungspfad.
/// SetupPlacedStation ist idempotent (Controller/Visuals/Interaktion werden nur einmal
/// angelegt), daher ist die Doppel-Auslösung mit dem BuildableItem.Start-Postfix harmlos.
/// </summary>
public static class AutoPackBuildEventHandlers
{
    public static void OnGridItemCreated(S1API.Building.BuildEventArgs args)
    {
        try
        {
            if (args == null) return;
            if (!string.Equals(args.ItemId, Mod.CurrentConfig.StationItemId, StringComparison.OrdinalIgnoreCase)) return;
            var go = args.GameObject;
            if (go == null) return;
            Mod.Log.Info($"BuildEvents.OnGridItemCreated: setting up '{args.ItemId}' on '{go.name}'.");
            AutoPackagingItemFactory.SetupPlacedStation(go);
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"OnGridItemCreatedSetup error: {ex.Message}");
        }
    }
}


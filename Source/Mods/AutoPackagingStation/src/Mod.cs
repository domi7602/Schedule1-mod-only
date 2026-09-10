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

[assembly: MelonInfo(typeof(AutoPackagingStation.Mod), "AutoPackagingStation", "0.2.2", "Dominik")]
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

        var asmVer = typeof(Mod).Assembly.GetName().Version?.ToString(3) ?? "0.2.0";
        Log.Info($"Initializing AutoPackagingStation v{asmVer} (Timer: {CurrentConfig.PackagingDurationSeconds}s, FreshnessBonus: +{CurrentConfig.FreshnessBonusMultiplier:P0})");

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

        // 3. Subscribe to S1API Lifecycle Events (Critic Pillar 3: Persistence exclusively on GameLifecycle.OnSaveComplete)
        GameLifecycle.OnPreLoad += AutoPackStore.OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded += AutoPackagingItemFactory.OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete += AutoPackStore.OnLoadComplete;
        GameLifecycle.OnLoadComplete += AutoPackagingItemFactory.InjectHardwareStoreListing;
        GameLifecycle.OnSaveComplete += AutoPackStore.OnSaveComplete;
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnPreLoad -= AutoPackStore.OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded -= AutoPackagingItemFactory.OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete -= AutoPackStore.OnLoadComplete;
        GameLifecycle.OnLoadComplete -= AutoPackagingItemFactory.InjectHardwareStoreListing;
        GameLifecycle.OnSaveComplete -= AutoPackStore.OnSaveComplete;
        AutoPackStore.OnPreLoad();
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


using System;
using MelonLoader;
using PotScanner.Services;
using PotScanner.Utils;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;
// v0.4.0: hash plugin-shim. `using Hash.Api;` is REQUIRED (not `Hash.Api.HashCommands` fully
// qualified) because MelonBase has a string property called `Hash` that wins over the
// namespace lookup in MelonMod subclasses — see HashCommands.cs doc-comment Z.25.
using Hash.Api;

[assembly: MelonInfo(typeof(PotScanner.Mod), Constants.ModName, Constants.ModVersion, Constants.ModAuthor)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PotScanner;

public class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg($"{Constants.ModName} {Constants.ModVersion} initialized.");
        LoggerInstance.Msg($" Waiting for gameplay scene ('{Constants.GameplaySceneName}') to start scanning.");

        // v0.3.0: persisted settings (currently: AutoWaterEnabled). ModConfig<T>.Initialize
        // creates a MelonPreferences category under the hood; Instance auto-loads.
        ModConfig<PotScannerConfig>.Initialize("PotScanner");

        // v0.3.0: Auto-Water feature — polling-based, runs in PotTracker.Tick() every 2s.
        // Initialize/Shutdown are no-ops for the polling variant; left in for API stability
        // (future Harmony variant could reuse them). Toggle state restored from MelonPreferences
        // below — AutoWaterService.WaterTick() reads IsEnabled on every tick.
        AutoWaterService.Initialize();

        // Restore persisted Auto-Water state.
        bool autoWater = ModConfig<PotScannerConfig>.Instance.AutoWaterEnabled;
        AutoWaterService.SetEnabled(autoWater);
        LoggerInstance.Msg($" Auto-Water restored: {(autoWater ? "ENABLED" : "DISABLED")} (polling-mode).");

        // React to runtime changes (e.g. from console commands or future settings UI).
        ModConfig<PotScannerConfig>.OnChanged += OnConfigChanged;

        // v0.2.1: subscribe to S1API's GameLifecycle hooks. OnSaveInfoLoaded fires AFTER
        // the save-file is parsed but BEFORE the scene loads — perfect time to refresh
        // Property.OwnedProperties (which was empty at OnGameplaySceneLoaded in v0.2.0).
        // OnLoadComplete fires after the scene is fully built — ideal for the final UI refresh.
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete += OnLoadComplete;

        // v0.4.0: register `pot` with hash's command list. This is listing-only — execution
        // happens via S1API's BaseConsoleCommand system in ConsoleBridge.cs. The Add() call
        // is safe even when hash.dll is not deployed (the shim's runtime lookup of
        // "Hash.Bridge.HashBridge, Hash" returns null and queues the call for later replay).
        HashCommands.Add(
            word: "pot",
            description: "PotScanner: list, summary, stats, water-all, auto [on|off], refresh, help",
            example: "pot summary");
    }

    public override void OnApplicationQuit()
    {
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        ModConfig<PotScannerConfig>.OnChanged -= OnConfigChanged;
        AutoWaterService.Shutdown();
    }

    public override void OnUpdate()
    {
        PotTracker.Instance.Tick(Time.deltaTime);
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        LoggerInstance.Msg($"Scene loaded: '{sceneName}' (#{buildIndex})");
        AutoWaterService.ResetCache();
        if (IsGameplayScene(sceneName))
            PotTracker.Instance.OnGameplaySceneLoaded();
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        LoggerInstance.Msg($"Scene unloaded: '{sceneName}' (#{buildIndex})");
        AutoWaterService.ResetCache();
        if (IsGameplayScene(sceneName))
        {
            PotScannerApp.TearDownForSceneUnload();
            PotTracker.Instance.OnSceneUnloaded();
        }
    }

    /// <summary>
    /// S1API hook: fires after the save file's metadata is parsed but before the
    /// scene is loaded. Property.OwnedProperties is populated by this point, so we
    /// refresh the property cache eagerly. Replaces v0.2.0's 25s-retry mechanism.
    /// </summary>
    private void OnSaveInfoLoaded()
    {
        try
        {
            PotTracker.Instance.OnSaveInfoLoaded();
        }
        catch (Exception ex)
        {
            LoggerInstance.Warning($" OnSaveInfoLoaded handler failed: {ex.Message}");
        }
    }

    /// <summary>
    /// S1API hook: fires after the save load completes and the scene is fully built.
    /// Triggers an immediate refresh so the PhoneApp shows the final state without
    /// waiting for the next 2s tick.
    /// </summary>
    private void OnLoadComplete()
    {
        try
        {
            PotTracker.Instance.RefreshNow();
        }
        catch (Exception ex)
        {
            LoggerInstance.Warning($" OnLoadComplete handler failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Single source of truth for "is this scene the one where pots live?".
    /// Load and Unload use the same predicate so an additive scene (menu, interior,
    /// UI overlay) can never accidentally tear down the tracker.
    /// </summary>
    private static bool IsGameplayScene(string sceneName)
    {
        return string.Equals(sceneName, Constants.GameplaySceneName, StringComparison.Ordinal);
    }

    /// <summary>
    /// v0.3.0: reacts to ModConfig changes (e.g. from console / future settings UI).
    /// Currently only the AutoWaterEnabled toggle is wired up.
    /// </summary>
    private void OnConfigChanged(string propertyName, object? value)
    {
        if (propertyName == nameof(PotScannerConfig.AutoWaterEnabled) && value is bool b)
        {
            AutoWaterService.SetEnabled(b);
        }
    }
}

using System;
using System.IO;
using System.Reflection;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using UnityEngine;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(ProfitTracker.ProfitTrackerMod), "Profit Tracker", "1.1.0", "Schedule1Mods")]
[assembly: MelonGame("TVGS", "Schedule I")]
[assembly: MelonGame("TVGS", "Schedule 1")]

namespace ProfitTracker;

public class ProfitTrackerMod : MelonMod
{
    public static ProfitTrackerMod? Instance { get; private set; }

    private const float SampleInterval = 0.25f;

    private ModLogger _log = null!;
    private ProfitTrackerState _state = null!;
    private ProfitTrackerHUD _hud = null!;

    private float _nextSample;
    private string _activeSaveKey = "default";
    private bool _hasValidEconomy;
    private float _lastNetWorth;
    private float _lastLiquid;
    private int _lastElapsedDays = -1;

    public override void OnInitializeMelon()
    {
        Instance = this;
        _log = new ModLogger("ProfitTracker");
        _state = new ProfitTrackerState(_log);
        _hud = new ProfitTrackerHUD(_log);

        Directory.CreateDirectory(ProfitTrackerState.StateDirectory);
        ModConfig<ProfitTrackerConfig>.Initialize("ProfitTracker");

        RegisterHashCommands();

        _log.Info($"Profit Tracker v1.1.0 initialized. Hotkey: {ModConfig<ProfitTrackerConfig>.Instance.ToggleHotkey}.");
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        ProfitTrackerFont.ResetCache();
        _hud.EnsureHUD(ModConfig<ProfitTrackerConfig>.Instance);
        _hud.ApplyAnchorAndPosition(ModConfig<ProfitTrackerConfig>.Instance);
        _hud.SetVisible(false);
    }

    public override void OnUpdate()
    {
        var config = ModConfig<ProfitTrackerConfig>.Instance;

        // 1. Hotkey Toggle with Input-Focus Protection
        if (Input.GetKeyDown(config.ToggleHotkey))
        {
            if (!HotkeyManager.IsInputFieldFocused())
            {
                ToggleHUD();
            }
        }

        // 2. Mouse Dragging
        _hud.HandleMouseDrag(config, (newX, newY) =>
        {
            config.PositionX = newX;
            config.PositionY = newY;
            ModConfig<ProfitTrackerConfig>.Save();
        });

        // 3. Economy Polling
        if (Time.unscaledTime >= _nextSample)
        {
            _nextSample = Time.unscaledTime + SampleInterval;
            SampleEconomy();
        }
    }

    public override void OnApplicationQuit()
    {
        _state?.SaveCurrent();
    }

    public override void OnDeinitializeMelon()
    {
        _state?.SaveCurrent();
        _hud?.Destroy();
    }

    public void ToggleHUD()
    {
        var config = ModConfig<ProfitTrackerConfig>.Instance;
        bool next = !config.Visible;
        ModConfig<ProfitTrackerConfig>.SetAndSave("Visible", next);
        UpdateHUDVisibility();
    }

    public void ApplyHUDConfig()
    {
        var config = ModConfig<ProfitTrackerConfig>.Instance;
        _hud.ApplyAnchorAndPosition(config);
        UpdateHUDVisibility();
    }

    public void ResetBaselines()
    {
        if (_hasValidEconomy && _lastElapsedDays >= 0)
        {
            _state.ResetBaselines(_lastElapsedDays, _lastNetWorth, _lastLiquid);
            _log.Info($"Reset baselines for day {_lastElapsedDays}. Net worth: ${_lastNetWorth:N0}, Liquid: ${_lastLiquid:N0}.");
            SampleEconomy();
        }
        else
        {
            _log.Warn("Cannot reset baselines: No active game session or economy state loaded.");
        }
    }

    public void PrintStatusToConsole()
    {
        var config = ModConfig<ProfitTrackerConfig>.Instance;
        var (todayDelta, weekDelta, sessionDelta) = _state.GetDeltas(_lastElapsedDays, _lastNetWorth, _lastLiquid, config.TrackInventoryNetWorth);

        _log.Info("=== ProfitTracker Live Status ===");
        _log.Info($"  Save Slot:     {_activeSaveKey}");
        _log.Info($"  In-Game Day:   Day {_lastElapsedDays}");
        _log.Info($"  Liquid (Cash): ${_lastLiquid:N0}");
        _log.Info($"  Total Net:     ${_lastNetWorth:N0}");
        _log.Info($"  Mode:          {(config.TrackInventoryNetWorth ? "Total Net Worth (Honest)" : "Liquid Cash Only")}");
        _log.Info($"  Today Profit:  {ProfitTrackerHUD.FormatDelta(todayDelta)}");
        _log.Info($"  Week (7-Day):  {ProfitTrackerHUD.FormatDelta(weekDelta)}");
        _log.Info($"  Session:       {ProfitTrackerHUD.FormatDelta(sessionDelta)}");
        _log.Info($"  HUD Visible:   {config.Visible}");
    }

    private void SampleEconomy()
    {
        if (!IsGameLoaded() || !TryGetLiquidMoney(out float liquid) || !TryGetElapsedDays(out int days))
        {
            _hasValidEconomy = false;
            UpdateHUDVisibility();
            return;
        }

        float netWorth = TryGetTotalNetWorth(liquid);

        string saveKey = DetectSaveKey();
        if (saveKey != _activeSaveKey)
        {
            _state.SaveCurrent();
            _activeSaveKey = saveKey;
            _state.LoadForSave(saveKey);
        }

        var config = ModConfig<ProfitTrackerConfig>.Instance;
        _state.UpdateEconomy(days, netWorth, liquid, config.TrackInventoryNetWorth);

        _lastElapsedDays = days;
        _lastNetWorth = netWorth;
        _lastLiquid = liquid;
        _hasValidEconomy = true;

        var (todayDelta, weekDelta, sessionDelta) = _state.GetDeltas(days, netWorth, liquid, config.TrackInventoryNetWorth);

        _hud.EnsureHUD(config);
        _hud.UpdateValues(todayDelta, weekDelta, sessionDelta, config.TrackInventoryNetWorth, config);
        UpdateHUDVisibility();
    }

    private void UpdateHUDVisibility()
    {
        var config = ModConfig<ProfitTrackerConfig>.Instance;
        bool shouldShow = config.Visible && _hasValidEconomy;
        _hud.SetVisible(shouldShow);
    }

    private static bool IsGameLoaded()
    {
        try
        {
            if (LoadManager.Instance != null && (UnityEngine.Object)LoadManager.Instance != null)
                return LoadManager.Instance.IsGameLoaded;

            return MoneyManager.Instance != null && (UnityEngine.Object)MoneyManager.Instance != null;
        }
        catch
        {
            return false;
        }
    }

    private static bool TryGetLiquidMoney(out float liquid)
    {
        liquid = 0f;
        if (MoneyManager.Instance == null || (UnityEngine.Object)MoneyManager.Instance == null)
            return false;

        try
        {
            float cash = MoneyManager.Instance.cashBalance;
            float bank = MoneyManager.Instance.onlineBalance;
            liquid = cash + bank;
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to read liquid money: {ex.Message}");
            return false;
        }
    }

    private static float TryGetTotalNetWorth(float fallbackLiquid)
    {
        if (MoneyManager.Instance == null || (UnityEngine.Object)MoneyManager.Instance == null)
            return fallbackLiquid;

        try
        {
            float worth = MoneyManager.Instance.GetNetWorth();
            return worth > 0.01f ? worth : fallbackLiquid;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to calculate Total Net Worth: {ex.Message}");
            return fallbackLiquid;
        }
    }

    private static bool TryGetElapsedDays(out int days)
    {
        days = 0;
        if (TimeManager.Instance == null || (UnityEngine.Object)TimeManager.Instance == null)
            return false;

        try
        {
            days = TimeManager.Instance.ElapsedDays;
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to read elapsed days: {ex.Message}");
            return false;
        }
    }

    private static string DetectSaveKey()
    {
        try
        {
            if (SaveManager.Instance != null && (UnityEngine.Object)SaveManager.Instance != null)
            {
                string path = SaveManager.Instance.PlayersSavePath;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    string folder = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    if (!string.IsNullOrWhiteSpace(folder))
                        return folder;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Save key detection fallback: {ex.Message}");
        }

        return "default";
    }

    private void RegisterHashCommands()
    {
        try
        {
            Type? bridgeType = Type.GetType("Hash.Bridge.HashBridge, Hash");
            if (bridgeType != null)
            {
                MethodInfo? declareMethod = bridgeType.GetMethod("Declare", BindingFlags.Public | BindingFlags.Static);
                if (declareMethod != null)
                {
                    declareMethod.Invoke(null, new object[] { "profit", "ProfitTracker economy & HUD commands (status, reset, toggle, mode, pos, scale, help)", "profit status", "ProfitTracker" });
                    _log.Info("Registered 'profit' command with hash terminal via HashBridge.");
                    return;
                }
            }

            Type? hashType = Type.GetType("Hash.Api.HashCommands, Hash") ?? Type.GetType("Hash.HashCommands, Hash");
            if (hashType != null)
            {
                MethodInfo? addMethod = hashType.GetMethod("Add", BindingFlags.Public | BindingFlags.Static);
                if (addMethod != null)
                {
                    addMethod.Invoke(null, new object[] { "profit", "ProfitTracker economy & HUD commands (status, reset, toggle, mode, pos, scale, help)" });
                    _log.Info("Registered 'profit' command with hash terminal.");
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"Hash terminal integration skipped: {ex.Message}");
        }
    }
}

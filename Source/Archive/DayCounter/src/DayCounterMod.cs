using System;
using System.Reflection;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using UnityEngine;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(DayCounter.DayCounterMod), "DayCounter", "1.0.1", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]
[assembly: MelonGame("TVGS", "Schedule 1")]

namespace DayCounter;

public class DayCounterMod : MelonMod
{
    public static DayCounterMod? Instance { get; private set; }

    private const float SampleInterval = 0.20f;

    private ModLogger _log = null!;
    private DayCounterHUD _hud = null!;
    private float _nextSample;

    public override void OnInitializeMelon()
    {
        Instance = this;
        _log = new ModLogger("DayCounter");
        _hud = new DayCounterHUD(_log);

        ModConfig<DayCounterConfig>.Initialize("DayCounter");
        RegisterHashCommands();

        _log.Info($"DayCounter v1.0.1 initialized. Hotkey: {ModConfig<DayCounterConfig>.Instance.ToggleHotkey}.");
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        DayCounterFont.ResetCache();
        var config = ModConfig<DayCounterConfig>.Instance;
        _hud.EnsureHUD(config);
        _hud.RefreshFont();
        _hud.ApplyAnchorAndPosition(config);

        bool isGameplay = sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase);
        _hud.SetVisible(isGameplay && config.Visible);
    }

    public override void OnUpdate()
    {
        var config = ModConfig<DayCounterConfig>.Instance;

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
            ModConfig<DayCounterConfig>.Save();
        });

        // 3. Time Polling
        if (Time.unscaledTime >= _nextSample)
        {
            _nextSample = Time.unscaledTime + SampleInterval;
            UpdateOverlay();
        }
    }

    public override void OnDeinitializeMelon()
    {
        _hud?.Destroy();
        Instance = null;
    }

    public void ToggleHUD()
    {
        var config = ModConfig<DayCounterConfig>.Instance;
        bool next = !config.Visible;
        ModConfig<DayCounterConfig>.SetAndSave("Visible", next);
        UpdateHUDVisibility();
    }

    public void ApplyHUDConfig()
    {
        var config = ModConfig<DayCounterConfig>.Instance;
        _hud.ApplyAnchorAndPosition(config);
        UpdateHUDVisibility();
    }

    public void PrintStatusToConsole()
    {
        var config = ModConfig<DayCounterConfig>.Instance;
        bool isLoaded = IsGameLoaded();
        int days = 0;
        EDay dayOfWeek = EDay.Monday;
        int time = 0;

        if (isLoaded && TimeManager.Instance != null && (UnityEngine.Object)TimeManager.Instance != null)
        {
            try
            {
                days = TimeManager.Instance.ElapsedDays;
                dayOfWeek = TimeManager.Instance.CurrentDay;
                time = TimeManager.Instance.CurrentTime;
            }
            catch { }
        }

        int rawHours = time / 100;
        int rawMins = time % 100;

        _log.Info("=== DayCounter Live Status ===");
        _log.Info($"  In-Game Day:   Day {days}");
        _log.Info($"  Day of Week:   {dayOfWeek}");
        _log.Info($"  Current Time:  {rawHours:D2}:{rawMins:D2}");
        _log.Info($"  HUD Visible:   {config.Visible}");
        _log.Info($"  Layout Mode:   {(config.CompactMode ? "Compact Pill (1-Row)" : "Standard Badge (2-Row)")}");
        _log.Info($"  Time Format:   {(config.TimeFormat24H ? "24-Hour" : "12-Hour AM/PM")}");
        _log.Info($"  Anchor / Pos:  {config.Anchor} ({config.PositionX:F1}, {config.PositionY:F1})");
        _log.Info($"  Scale / Alpha: {config.Scale:F2} / {config.BackgroundOpacity:F2}");
        _log.Info($"  Hotkey:        {config.ToggleHotkey}");
    }

    private void UpdateOverlay()
    {
        if (!IsGameLoaded() || TimeManager.Instance == null || (UnityEngine.Object)TimeManager.Instance == null)
        {
            _hud.SetVisible(false);
            return;
        }

        var config = ModConfig<DayCounterConfig>.Instance;
        if (!config.Visible)
        {
            _hud.SetVisible(false);
            return;
        }

        if (_hud.CheckShouldHide(config))
        {
            _hud.SetVisible(false);
            return;
        }

        _hud.SetVisible(true);

        try
        {
            int elapsedDays = TimeManager.Instance.ElapsedDays;
            EDay dayOfWeek = TimeManager.Instance.CurrentDay;
            int currentTime = TimeManager.Instance.CurrentTime;
            _hud.UpdateValues(elapsedDays, dayOfWeek, currentTime, config);
        }
        catch (Exception ex)
        {
            _log.Warn($"Error updating DayCounter values: {ex.Message}");
        }
    }

    private void UpdateHUDVisibility()
    {
        var config = ModConfig<DayCounterConfig>.Instance;
        bool shouldShow = config.Visible && IsGameLoaded();
        if (shouldShow && _hud.CheckShouldHide(config))
            shouldShow = false;

        _hud.SetVisible(shouldShow);
    }

    private static bool IsGameLoaded()
    {
        try
        {
            if (LoadManager.Instance != null && (UnityEngine.Object)LoadManager.Instance != null)
                return LoadManager.Instance.IsGameLoaded;

            return TimeManager.Instance != null && (UnityEngine.Object)TimeManager.Instance != null;
        }
        catch
        {
            return false;
        }
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
                    declareMethod.Invoke(null, new object[] { "daycounter", "DayCounter HUD overlay commands (status, toggle, compact, format, anchor, pos, scale, opacity, reset, help)", "daycounter status", "DayCounter" });
                    declareMethod.Invoke(null, new object[] { "day", "DayCounter shorthand commands (status, toggle, compact, format, pos, scale, reset)", "day status", "DayCounter" });
                    _log.Info("Registered 'daycounter' and 'day' commands with hash terminal via HashBridge.");
                    return;
                }
            }

            Type? hashType = Type.GetType("Hash.Api.HashCommands, Hash") ?? Type.GetType("Hash.HashCommands, Hash");
            if (hashType != null)
            {
                MethodInfo? addMethod = hashType.GetMethod("Add", BindingFlags.Public | BindingFlags.Static);
                if (addMethod != null)
                {
                    addMethod.Invoke(null, new object[] { "daycounter", "DayCounter HUD overlay commands (status, toggle, compact, format, anchor, pos, scale, reset)" });
                    addMethod.Invoke(null, new object[] { "day", "DayCounter shorthand commands" });
                    _log.Info("Registered 'daycounter' and 'day' commands with hash terminal.");
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"Hash terminal integration skipped: {ex.Message}");
        }
    }
}

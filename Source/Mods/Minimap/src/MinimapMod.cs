using System;
using System.IO;
using System.Text.Json;
using MelonLoader;
using S1API.Console;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(Minimap.MinimapMod), "Minimap", "1.0.2", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace Minimap;

public sealed class MinimapMod : MelonMod
{
    public static MinimapMod? Instance { get; private set; }

    private readonly ModLogger _log = new("Minimap");
    private readonly MinimapHUD _hud;
    public MinimapConfig Config { get; private set; } = new();

    private string _configPath = "";
    private KeyCode _toggleKeyCode = KeyCode.M;
    private KeyCode _zoomInKeyCode = KeyCode.RightBracket;
    private KeyCode _zoomOutKeyCode = KeyCode.LeftBracket;

    public MinimapMod()
    {
        _hud = new MinimapHUD(_log);
    }

    public override void OnInitializeMelon()
    {
        Instance = this;
        _log.Info("Initializing Minimap & Unified HUD Mod v1.0.2...");

        // Setup SafeStorage config directory
        string userDir = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "Minimap");
        Directory.CreateDirectory(userDir);
        _configPath = Path.Combine(userDir, "config.json");

        LoadConfig();
        ParseKeyCodes();

        _log.Info("Minimap initialized. Console commands 'minimap' and 'map' ready.");
        TryRegisterHashPlugin();
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        if (IsGameplayScene(sceneName))
        {
            _hud.EnsureHUD(Config);
            _hud.ApplyLayout(Config);
            _hud.SetHUDActive(Config.MinimapVisible);
            _hud.InvalidateBlipCache();
        }
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (IsGameplayScene(sceneName))
        {
            // Persist debounced transient changes (e.g. zoom level) instead of saving per keypress.
            SaveConfig();
            _hud.SetHUDActive(false);
            _hud.InvalidateMapSprite();
            MinimapFont.ResetCache();
        }
    }

    public override void OnUpdate()
    {
        if (!_hud.IsCreated)
            return;

        // Hotkey Toggle (respects input field focus)
        if (!HotkeyManager.IsInputFieldFocused())
        {
            if (Input.GetKeyDown(_toggleKeyCode))
            {
                ToggleMinimap();
            }

            if (Input.GetKeyDown(_zoomInKeyCode) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                _hud.AdjustZoom(Config, 0.25f);
            }
            else if (Input.GetKeyDown(_zoomOutKeyCode) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                _hud.AdjustZoom(Config, -0.25f);
            }
        }

        _hud.Update(Config, Time.time);
    }

    public void ToggleMinimap()
    {
        _hud.ToggleVisibility(Config);
        SaveConfig();
    }

    public void SaveAndApply()
    {
        SaveConfig();
        _hud.ApplyLayout(Config);
    }

    public void PrintStatusToConsole()
    {
        MelonLogger.Msg("--------------------------------------------------");
        MelonLogger.Msg("  Minimap & Unified HUD — Live Status");
        MelonLogger.Msg("--------------------------------------------------");
        MelonLogger.Msg($"  Visible:        {Config.MinimapVisible}");
        MelonLogger.Msg($"  Shape:          {Config.Shape}");
        MelonLogger.Msg($"  Rotation Mode:  {Config.RotationMode}");
        MelonLogger.Msg($"  Zoom Level:     {Config.ZoomLevel:F2}x");
        MelonLogger.Msg($"  Map Size:       {Config.MapSize:F0}px");
        MelonLogger.Msg($"  DayCounter:     {Config.DayMode}");
        MelonLogger.Msg($"  Anchor:         {(Config.HasCustomPosition ? $"Custom ({Config.CustomPosX:F0}, {Config.CustomPosY:F0})" : Config.AnchorPosition.ToString())}");
        MelonLogger.Msg($"  Blips:          Police={Config.ShowPoliceBlips}, Deals={Config.ShowActiveDeals}, Potential={Config.ShowPotentialCustomers}, Customers={Config.ShowKnownCustomers}, Props={Config.ShowPropertyBlips}, Shops={Config.ShowShopBlips}, Quests={Config.ShowQuestBlips}, Vehicles={Config.ShowVehicleBlips}");
        MelonLogger.Msg($"  Clamping:       {Config.EnableEdgeClamping}");
        MelonLogger.Msg("--------------------------------------------------");
    }

    private void LoadConfig()
    {
        try
        {
            var loaded = SafeStorage.LoadSafe<MinimapConfig>(_configPath, new MinimapConfig(), _log);
            if (loaded != null)
            {
                Config = loaded;
                _log.Info("Loaded configuration from config.json.");
                return;
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to read config.json, using defaults: {ex.Message}");
        }

        SaveConfig();
    }

    public void SaveConfig()
    {
        try
        {
            SafeStorage.SaveAtomic(_configPath, Config, _log);
        }
        catch (Exception ex)
        {
            _log.Error($"Error saving config: {ex.Message}");
        }
    }

    private void ParseKeyCodes()
    {
        if (Enum.TryParse<KeyCode>(Config.ToggleKey, true, out var k)) _toggleKeyCode = k;
        if (Enum.TryParse<KeyCode>(Config.ZoomInKey, true, out var inK)) _zoomInKeyCode = inK;
        if (Enum.TryParse<KeyCode>(Config.ZoomOutKey, true, out var outK)) _zoomOutKeyCode = outK;
    }

    private static bool IsGameplayScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return false;
        return sceneName == "Main";
    }

    private void TryRegisterHashPlugin()
    {
        try
        {
            var hashType = Type.GetType("Hash.Api.HashCommands, Hash");
            if (hashType == null)
            {
                return;
            }

            var availableProp = hashType.GetProperty("Available", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (availableProp == null || !(bool)(availableProp.GetValue(null) ?? false))
            {
                return;
            }

            var addMethod = hashType.GetMethod("Add", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (addMethod == null)
            {
                _log.Warn("Hash.Api.HashCommands.Available but no Add() method found.");
                return;
            }

            addMethod.Invoke(null, new object[]
            {
                "minimap",
                "Minimap & Unified HUD: toggle, shape <circle|rounded|square>, rot <follow|north>, zoom <val>, size <val>, day <header|footer|off|standalone>, pos <corner|reset>, blips <type> <on|off>, status, help",
                "minimap status"
            });

            addMethod.Invoke(null, new object[]
            {
                "map",
                "Alias for 'minimap' command suite.",
                "map status"
            });

            _log.Info("Hash bridge registered: 'minimap' and 'map' commands listed in terminal.");
        }
        catch (Exception ex)
        {
            _log.Warn($"Hash bridge registration failed: {ex.Message}");
        }
    }
}

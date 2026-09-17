using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using MelonLoader;
using S1API.Console;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(Minimap.MinimapMod), "Minimap", "2.0.2", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace Minimap;

public sealed class MinimapMod : MelonMod
{
    public static MinimapMod? Instance { get; private set; }

    private readonly ModLogger _log = new("Minimap");
    private readonly MinimapHUD _hud;
    public MinimapConfig Config { get; private set; } = new();
    public MinimapWaypoints Waypoints => _hud.Waypoints;

    private string _configPath = "";
    private KeyCode _toggleKeyCode = KeyCode.M;
    private KeyCode _zoomInKeyCode = KeyCode.RightBracket;
    private KeyCode _zoomOutKeyCode = KeyCode.LeftBracket;

    // ── M6: per-category blip palette (hex), persisted inside config.json ──
    private readonly System.Collections.Generic.Dictionary<string, string> _blipColors = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Police",     "#338CF2" },
        { "Dealer",     "#A44DE3" },
        { "Waypoint",   "#E14BFF" },
        { "Deal",       "#FAC40F" },
        { "Potential",  "#1ABD9C" },
        { "Customer",   "#2ECC70" },
        { "Property",   "#F29C12" },
        { "Shop",       "#E84D3D" },
        { "Quest",      "#E64C3C" },
        { "Vehicle",    "#3399DB" },
    };

    /// <summary>M6: current hex for a blip category key (falls back to white).</summary>
    public string GetBlipColorHex(string key)
        => _blipColors.TryGetValue(key, out var hex) ? hex : "#FFFFFF";

    /// <summary>M6: set a category color from a hex string. Invalid hex is ignored.</summary>
    public bool SetBlipColor(string key, string hex)
    {
        if (string.IsNullOrWhiteSpace(hex)) return false;
        if (!UnityEngine.ColorUtility.TryParseHtmlString(hex, out _)) return false;
        if (!_blipColors.ContainsKey(key)) return false;
        _blipColors[key] = hex;
        return true;
    }

    /// <summary>M6: live-apply the palette to the blip renderer.</summary>
    public void ApplyBlipPalette()
    {
        try { _hud.ApplyBlipPalette(_blipColors); } catch { }
    }

    /// <summary>M6: hex lookup for the settings app (public wrapper).</summary>
    public string GetBlipColor(string key) => GetBlipColorHex(key);

    public MinimapMod()
    {
        _hud = new MinimapHUD(_log);
    }

    public override void OnInitializeMelon()
    {
        Instance = this;
        string version = GetType().Assembly.GetCustomAttribute<MelonInfoAttribute>()?.Version ?? "?";
        _log.Info($"Initializing Minimap & Unified HUD Mod v{version}...");

        // Setup SafeStorage config directory
        string userDir = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "Minimap");
        Directory.CreateDirectory(userDir);
        _configPath = Path.Combine(userDir, "config.json");

        LoadConfig();
        LoadBlipPalette();
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
            // M4: load waypoints for the (possibly new) save slot.
            try { _hud.Waypoints.Load(); } catch { }
        }
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (IsGameplayScene(sceneName))
        {
            // Persist debounced transient changes (e.g. zoom level) instead of saving per keypress.
            SaveConfig();
            // M4: flush pending waypoint writes before teardown.
            try { _hud.Waypoints.FlushIfDirty(); } catch { }
            _hud.SetHUDActive(false);
            _hud.InvalidateMapSprite();
            MinimapFont.ResetCache();
        }
    }

    public override void OnUpdate()
    {
        if (!_hud.IsCreated)
            return;

        // Self-heal (2026-09-13): the DontDestroyOnLoad canvas gets silently
        // deactivated when the game does a data-only reload (arrest/bust, save
        // reload, cutscene) — OnSceneWasUnloaded deactivates it and no matching
        // OnSceneWasInitialized always follows. Re-activate while we are on Main
        // and the user wants the minimap visible.
        try
        {
            if (!_hud.IsActive && Config.MinimapVisible && NetworkGuard.IsInMainScene)
                _hud.SetHUDActive(true);
            else if (_hud.IsActive && !Config.MinimapVisible)
                _hud.SetHUDActive(false);
        }
        catch { }

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
        _hud.SetHUDActive(Config.MinimapVisible);
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

    /// <summary>M6: persist palette changes into config.json (sidecar of blip colors).</summary>
    public void SaveBlipPalette()
    {
        try
        {
            string palettePath = SafeStorage.GetUserDataPath("Minimap", "blip_colors.json");
            SafeStorage.SaveTextAtomic(palettePath,
                System.Text.Json.JsonSerializer.Serialize(_blipColors,
                    new System.Text.Json.JsonSerializerOptions { WriteIndented = true }), _log);
        }
        catch (Exception ex)
        {
            _log.Warn($"Blip palette save failed: {ex.Message}");
        }
    }

    private void LoadBlipPalette()
    {
        try
        {
            string palettePath = SafeStorage.GetUserDataPath("Minimap", "blip_colors.json");
            string json = SafeStorage.LoadTextSafe(palettePath, "", null);
            if (string.IsNullOrWhiteSpace(json)) return;
            var loaded = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (loaded == null) return;
            foreach (var kv in loaded)
            {
                if (_blipColors.ContainsKey(kv.Key)) _blipColors[kv.Key] = kv.Value;
            }
        }
        catch { }
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

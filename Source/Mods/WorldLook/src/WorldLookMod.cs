using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(WorldLook.WorldLookMod), "WorldLook", "0.3.0-dev.2", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace WorldLook;

/// <summary>
/// One coordinator owns the mild native-light adjustment and clear water. The game
/// owns its sky, clouds and celestial bodies. F7 changes only this session's state.
/// </summary>
public sealed class WorldLookMod : MelonMod
{
    private readonly GoldenHour.Mod _lighting = new();
    private readonly WaterShader.WaterShaderMod _water = new();
    private readonly ModLogger _log = new("WorldLook");
    private WorldLookSettings _settings = new();
    private string _configPath = "unavailable";
    private bool _sessionEnabled;
    private bool _lightingInitialized;
    private bool _waterInitialized;
    private bool _lightingFaulted;
    private bool _waterFaulted;
    private bool _inputErrorLogged;
    private bool _closed;

    public override void OnInitializeMelon()
    {
        try
        {
            _configPath = SafeStorage.GetUserDataPath("WorldLook", "WorldLook.json");
            _settings = WorldLookSettings.Load(() => File.ReadAllText(_configPath),
                json => SafeStorage.SaveTextAtomic(_configPath, json, _log), message => _log.Warn(message));
        }
        catch (Exception ex)
        {
            _settings = new WorldLookSettings { Enabled = false };
            _log.Warn("Profile unavailable; look disabled for this session", ex);
        }
        _sessionEnabled = _settings.Enabled;
        try
        {
            _lighting.OnInitializeMelon();
            _lightingInitialized = true;
            _lighting.SetEnabled(_sessionEnabled && _settings.LightingEnabled, _settings.Warmth);
        }
        catch (Exception ex) { LightingFault("initialization", ex); }
        _log.Info($"v{Info.Version} initialized. Native sky; mild light warmth={_settings.Warmth:0.00}; water={_settings.WaterEnabled}. Ctrl+Shift+F7: look on/off; Ctrl+Shift+F9: targeted status. Profile: {_configPath}");
    }

    public override void OnLateInitializeMelon()
    {
        if (_closed) return;
        try
        {
            _water.SetEnabled(_sessionEnabled && _settings.WaterEnabled);
            _water.OnLateInitializeMelon();
            _waterInitialized = true;
        }
        catch (Exception ex) { WaterFault("initialization", ex); }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (_closed || !_waterInitialized) return;
        try { _water.OnSceneWasLoaded(buildIndex, sceneName); }
        catch (Exception ex) { WaterFault("scene load", ex); }
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (_closed) return;
        try { _lighting.OnSceneWasUnloaded(buildIndex, sceneName); }
        catch (Exception ex) { LightingFault("scene unload", ex); }
        try { _water.OnSceneWasUnloaded(buildIndex, sceneName); }
        catch (Exception ex) { WaterFault("scene unload", ex); }
    }

    public override void OnUpdate()
    {
        if (_closed) return;
        HandleShortcuts();
        if (_lightingInitialized && !_lightingFaulted)
        {
            try { _lighting.OnUpdate(); }
            catch (Exception ex) { LightingFault("update", ex); }
        }
        if (_waterInitialized && !_waterFaulted)
        {
            try { _water.OnUpdate(); }
            catch (Exception ex) { WaterFault("update", ex); }
        }
    }

    public override void OnLateUpdate()
    {
        if (_closed || !_lightingInitialized || _lightingFaulted) return;
        try { _lighting.OnLateUpdate(); }
        catch (Exception ex) { LightingFault("late update", ex); }
    }

    private void HandleShortcuts()
    {
        try
        {
            if (!Application.isFocused || HotkeyManager.IsInputFieldFocused()) return;
            bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (!control || !shift) return;
            if (Input.GetKeyDown(KeyCode.F7) && NetworkGuard.IsInMainScene)
            {
                _sessionEnabled = !_sessionEnabled;
                if (_lightingInitialized)
                {
                    try
                    {
                        _lighting.SetEnabled(_sessionEnabled && _settings.LightingEnabled, _settings.Warmth);
                        _lightingFaulted = false;
                    }
                    catch (Exception ex) { LightingFault("toggle", ex); }
                }
                if (_waterInitialized)
                {
                    try
                    {
                        _water.SetEnabled(_sessionEnabled && _settings.WaterEnabled);
                        _waterFaulted = false;
                    }
                    catch (Exception ex) { WaterFault("toggle", ex); }
                }
                _log.Info("Look requested " + (_sessionEnabled ? "ON" : "OFF") +
                    $" (session only; lightingFault={_lightingFaulted}, waterFault={_waterFaulted}).");
            }
            if (Input.GetKeyDown(KeyCode.F9)) WriteTargetedStatus();
        }
        catch (Exception ex)
        {
            if (_inputErrorLogged) return;
            _inputErrorLogged = true;
            _log.Warn("Shortcut handling unavailable", ex);
        }
    }

    private void WriteTargetedStatus()
    {
        try
        {
            var report = new Dictionary<string, object?>
            {
                ["capturedUtc"] = DateTime.UtcNow.ToString("O"),
                ["version"] = Info.Version,
                ["lookRequested"] = _sessionEnabled,
                ["mainLoaded"] = NetworkGuard.IsInMainScene,
                ["skyMode"] = "native",
                ["nativeSkybox"] = DescribeNativeSkybox(),
                ["configPath"] = _configPath,
                ["lightingFaulted"] = _lightingFaulted,
                ["waterFaulted"] = _waterFaulted,
                ["lighting"] = _lighting.GetDiagnosticsSnapshot(),
                ["water"] = _water.GetDiagnosticsSnapshot(),
                ["shaderBundleHash"] = ShaderAssets.BundleHash,
                ["shaderLoadRoute"] = ShaderAssets.LoadRoute,
                ["measurementNote"] = "Targeted state only; no scene scan, GPU pixel or performance measurement."
            };
            string json = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
            string path = SafeStorage.GetUserDataPath("WorldLook", "status.json");
            if (SafeStorage.SaveTextAtomic(path, json, _log)) _log.Info("Targeted status written: " + path);
        }
        catch (Exception ex) { _log.Warn("Targeted status unavailable", ex); }
    }

    private static string DescribeNativeSkybox()
    {
        var material = RenderSettings.skybox;
        if (!NetworkGuard.IsAlive(material)) return "unavailable";
        var shader = material.shader;
        return material.name + " / " + (NetworkGuard.IsAlive(shader) ? shader.name : "shader unavailable");
    }

    private void LightingFault(string phase, Exception ex)
    {
        if (!_lightingFaulted) _log.Warn("Lighting " + phase + " failed; module paused until an explicit toggle", ex);
        _lightingFaulted = true;
        try { _lighting.SetEnabled(false, _settings.Warmth); } catch { }
    }

    private void WaterFault(string phase, Exception ex)
    {
        if (!_waterFaulted) _log.Warn("Water " + phase + " failed; module paused until an explicit toggle", ex);
        _waterFaulted = true;
        try { _water.SetEnabled(false); } catch { }
    }

    private void Shutdown()
    {
        if (_closed) return;
        _closed = true;
        try { _lighting.OnDeinitializeMelon(); } catch (Exception ex) { _log.Warn("Lighting cleanup incomplete", ex); }
        try { _water.OnDeinitializeMelon(); } catch (Exception ex) { _log.Warn("Water cleanup incomplete", ex); }
        try { ShaderAssets.Dispose(); } catch (Exception ex) { _log.Warn("Shader bundle cleanup incomplete", ex); }
    }

    public override void OnApplicationQuit() => Shutdown();
    public override void OnDeinitializeMelon() => Shutdown();
}

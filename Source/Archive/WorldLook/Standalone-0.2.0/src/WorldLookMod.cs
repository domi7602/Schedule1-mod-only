using System;
using System.Collections.Generic;
using System.Text.Json;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(WorldLook.WorldLookMod), "WorldLook", "0.2.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace WorldLook;

internal sealed class WorldLookConfig
{
    public bool Enabled { get; set; } = true;
    public bool WaterEnabled { get; set; } = true;
    public CloudSkySettings Clouds { get; set; } = new();
}

/// <summary>
/// One host owns the sky, its daylight adaptation, water and manual diagnostics.
/// The retained GoldenHour sources are not initialized: two independent writers
/// must not both tint the same sun and ambient baseline.
/// </summary>
public sealed class WorldLookMod : MelonMod
{
    private readonly GraphicsProbe.Mod _probe = new();
    private readonly WaterShader.WaterShaderMod _water = new();
    private readonly CloudSkyRuntime _sky = new(message => MelonLogger.Msg(message));
    private readonly ModLogger _log = new("WorldLook");
    private WorldLookConfig _config = new();
    private bool _sessionEnabled;
    private bool _lateInitialized;
    private bool _closed;
    private bool _inputErrorLogged;
    private bool _skyFaulted;

    public override void OnInitializeMelon()
    {
        try
        {
            ModConfig<WorldLookConfig>.Initialize("WorldLook", _log);
            _config = ModConfig<WorldLookConfig>.Instance;
            _config.Clouds ??= new CloudSkySettings();
        }
        catch (Exception ex)
        {
            _log.Warn("Configuration initialization failed; session defaults are active", ex);
            _config = new WorldLookConfig();
        }
        _sessionEnabled = _config.Enabled;
        _probe.OnInitializeMelon(Info.Version);
        _log.Info($"v{Info.Version} initialized. Ctrl+Shift+F7: complete look on/off; Ctrl+Shift+F8: full probe; Ctrl+Shift+F9: targeted status. New look is a prototype requiring in-game validation.");
    }

    public override void OnLateInitializeMelon()
    {
        _lateInitialized = true;
        _water.SetEnabled(_sessionEnabled && _config.WaterEnabled);
        _water.OnLateInitializeMelon();
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        => _probe.OnSceneWasInitialized(buildIndex, sceneName);

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        _probe.OnSceneWasLoaded(buildIndex, sceneName);
        _water.OnSceneWasLoaded(buildIndex, sceneName);
        if (sceneName == "Main") _skyFaulted = false;
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        _probe.OnSceneWasUnloaded(buildIndex, sceneName);
        _water.OnSceneWasUnloaded(buildIndex, sceneName);
        if (sceneName == "Main") _sky.Reset();
    }

    public override void OnUpdate()
    {
        if (_closed) return;
        HandleShortcuts();
        _probe.OnUpdate();
        if (_lateInitialized) _water.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        if (_closed || !_lateInitialized || _skyFaulted) return;
        try
        {
            if (_sessionEnabled && NetworkGuard.IsInMainScene && _config.Clouds.Enabled)
                _sky.Tick(_config.Clouds);
            else
                _sky.Reset();
        }
        catch (Exception ex)
        {
            _skyFaulted = true;
            try { _sky.Reset(); } catch { }
            _log.Warn("Sky module stopped after an error; water/probe remain available. Toggle the look or reload Main to retry", ex);
        }
    }

    private void HandleShortcuts()
    {
        try
        {
            if (!Application.isFocused || HotkeyManager.IsInputFieldFocused()) return;
            bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (!control || !shift) return;
            if (Input.GetKeyDown(KeyCode.F7))
            {
                _sessionEnabled = !_sessionEnabled;
                _skyFaulted = false;
                if (!_sessionEnabled) _sky.Reset();
                _water.SetEnabled(_sessionEnabled && _config.WaterEnabled);
                _log.Info("Complete look " + (_sessionEnabled ? "ON" : "OFF") + " (session toggle; config unchanged).");
            }
            if (Input.GetKeyDown(KeyCode.F9)) WriteTargetedStatus();
        }
        catch (Exception ex)
        {
            if (!_inputErrorLogged)
            {
                _inputErrorLogged = true;
                _log.Warn("Shortcut handling unavailable", ex);
            }
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
                ["lookEnabled"] = _sessionEnabled,
                ["mainLoaded"] = NetworkGuard.IsInMainScene,
                ["skyApplied"] = _sky.IsApplied,
                ["skyAppliedMaterials"] = _sky.AppliedMaterialCount,
                ["skyPendingRestores"] = _sky.PendingRestoreCount,
                ["skyFaulted"] = _skyFaulted,
                ["skyStatus"] = _sky.DescribeState(),
                ["shaderBundleHash"] = ShaderAssets.BundleHash,
                ["shaderLoadRoute"] = ShaderAssets.LoadRoute,
                ["water"] = _water.GetDiagnosticsSnapshot(),
                ["measurementNote"] = "Environment-only water: no owned reflection camera or object-reflection render. Legacy reflection counters are zero. Input requests do not prove current-frame texture contents."
            };
            string json = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
            string path = SafeStorage.GetUserDataPath("WorldLook", "status.json");
            if (SafeStorage.SaveTextAtomic(path, json, _log)) _log.Info("Targeted status written: " + path);
        }
        catch (Exception ex) { _log.Warn("Targeted status could not be written", ex); }
    }

    private void Shutdown()
    {
        if (_closed) return;
        _closed = true;
        try { _probe.OnDeinitializeMelon(); } catch (Exception ex) { _log.Warn("Probe shutdown failed", ex); }
        try { _water.OnDeinitializeMelon(); } catch (Exception ex) { _log.Warn("Water shutdown failed", ex); }
        try { _sky.Dispose(); } catch (Exception ex) { _log.Warn("Sky shutdown failed", ex); }
        ShaderAssets.Dispose();
    }

    public override void OnApplicationQuit() => Shutdown();
    public override void OnDeinitializeMelon() => Shutdown();
}

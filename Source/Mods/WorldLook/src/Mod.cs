using System;
using System.Collections.Generic;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.GameTime;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoldenHour;

/// <summary>
/// Native lighting bias coordinated by WorldLook. This module owns neither a hotkey
/// nor configuration. Desired state survives loads; native color leases do not carry
/// into a later scene (only unresolved, restore-only leases can remain quarantined).
/// </summary>
public sealed class Mod
{
    private readonly ModLogger _log = new("GoldenHour");
    private readonly SceneTargets _targets;
    private bool _initialized;
    private bool _desiredEnabled;
    private bool _loading;
    private bool _active;
    private double _warmth;
    private double _appliedStrength;
    private int? _sceneHandle;
    private int? _hhmm;
    private int? _minutes;
    private int _readableChannels;
    private string? _lastError;
    private int _errors;
    private double _nowSeconds;

    public Mod() => _targets = new SceneTargets(_log);

    public void OnInitializeMelon()
    {
        if (_initialized) return;
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnPreSceneChange += OnPreLoad;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        _loading = false;
        _initialized = true;
    }

    public void SetEnabled(bool enabled, double warmth)
    {
        _desiredEnabled = enabled;
        _warmth = BlendPolicy.ClampStrength(warmth);
        if (_initialized && !enabled) ReleaseWorld();
    }

    public void OnUpdate()
    {
        if (!_initialized) return;
        try
        {
            _nowSeconds = Time.unscaledTime;
            if (!TryGetMainScene(out Scene scene) || (_sceneHandle.HasValue && _sceneHandle != scene.handle))
                ReleaseWorld();
            _targets.RetryReleases(_nowSeconds);
        }
        catch (Exception ex) { FailFrame(ex); }
    }

    public void OnLateUpdate()
    {
        if (!_initialized) return;
        try
        {
            _nowSeconds = Time.unscaledTime;
            if (!_desiredEnabled || _loading || !TryGetMainScene(out Scene scene))
            {
                if (_active || _sceneHandle.HasValue) ReleaseWorld();
                return;
            }
            if (_sceneHandle.HasValue && _sceneHandle != scene.handle) ReleaseWorld();
            _sceneHandle = scene.handle;

            _hhmm = null;
            _minutes = null;
            bool readableTime = TryReadMinutes(out int hhmm, out int minutes);
            if (hhmm >= 0) _hhmm = hhmm;
            if (readableTime) _minutes = minutes;
            // An unreadable/malformed clock releases any old tint; it never means noon
            // or a full-strength fallback. Native lighting remains the baseline.
            double weight = _minutes.HasValue ? DaylightPolicy.Weight(_minutes.Value) : 0.0;
            _appliedStrength = _warmth * weight;
            _readableChannels = _targets.Apply(scene, _appliedStrength, _nowSeconds);
            _active = _readableChannels > 0;
        }
        catch (Exception ex) { FailFrame(ex); }
    }

    public void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (!_initialized || !string.Equals(sceneName, "Main", StringComparison.Ordinal)) return;
        ReleaseWorld();
    }

    public void OnDeinitializeMelon()
    {
        if (!_initialized) return;
        _initialized = false;
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnPreSceneChange -= OnPreLoad;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        ReleaseWorld();
        if (_targets.PendingRestores > 0)
            _log.Warn($"lighting shutdown has {_targets.PendingRestores} unresolved color restores.");
    }

    /// <summary>Cached lifecycle state for wiring tests; reading it does not resolve native resources.</summary>
    public Dictionary<string, object?> GetDiagnosticsSnapshot() => new()
    {
        ["desiredEnabled"] = _desiredEnabled,
        ["initialized"] = _initialized,
        ["active"] = _active,
        ["loading"] = _loading,
        ["sceneHandle"] = _sceneHandle,
        ["timeHhmm"] = _hhmm,
        ["minutesSinceMidnight"] = _minutes,
        ["timeReadable"] = _minutes.HasValue,
        ["warmth"] = _warmth,
        ["appliedStrength"] = _appliedStrength,
        ["readableChannels"] = _readableChannels,
        ["pendingRestores"] = _targets.PendingRestores,
        ["errors"] = _errors + _targets.ErrorCount,
        ["lastError"] = _lastError ?? _targets.LastError,
    };

    private void OnPreLoad()
    {
        if (!_initialized) return;
        _loading = true;
        ReleaseWorld();
    }

    private void OnLoadComplete()
    {
        if (_initialized) _loading = false;
    }

    private static bool TryGetMainScene(out Scene scene)
    {
        scene = SceneManager.GetActiveScene();
        return scene.IsValid() && scene.isLoaded && scene.name == "Main";
    }

    private static bool TryReadMinutes(out int hhmm, out int minutes)
    {
        hhmm = -1;
        minutes = -1;
        try
        {
            TimeManager? time = NetworkSingleton<TimeManager>.Instance;
            if (!NetworkGuard.IsAlive(time)) return false;
            hhmm = time!.CurrentTime;
            return DaylightPolicy.TryHhmmToMinutes(hhmm, out minutes);
        }
        catch { return false; }
    }

    private void ReleaseWorld()
    {
        _active = false;
        _sceneHandle = null;
        _appliedStrength = 0.0;
        _readableChannels = 0;
        _hhmm = null;
        _minutes = null;
        _targets.RestoreAll(_nowSeconds);
    }

    private void FailFrame(Exception ex)
    {
        if (_lastError != ex.Message)
        {
            _lastError = ex.Message;
            _errors++;
            _log.Warn("lighting frame failed", ex);
        }
        ReleaseWorld();
    }
}

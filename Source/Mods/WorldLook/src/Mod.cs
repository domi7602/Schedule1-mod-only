using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.GameTime;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoldenHour;

/// <summary>
/// Golden-hour look bias (client-side, cosmetic). Ctrl+Shift+F7 toggles a warm
/// multiplicative bias over sky sun glow, sun light color, ambient Trilight colors,
/// fog color and the GlobalVolume ColorAdjustments colorFilter. On activation a baseline
/// is recorded per parameter and then rebased onto the game's own drifting values (see
/// BaselinePolicy); toggle-off, scene exit and shutdown restore the latest adopted game
/// value for every written key.
/// The applied strength is the configured strength scaled by the in-game clock
/// (DaylightPolicy: full at 06:00 and 18:00, zero at noon and at night), so a constant
/// warm multiply cannot paint noon gold. An unreadable clock releases the bias instead
/// of painting full gold.
/// Every activation also writes a one-shot DIAG dump (camera post-processing, volume
/// flags, sky/render settings) to the MelonLoader log for the in-game verify.
/// </summary>
public sealed class Mod
{
    private const KeyCode ToggleKey = KeyCode.F7;

    private readonly TogglePolicy _toggle = new();
    private readonly SnapshotStore _store = new();
    private ReapplyPolicy _reapply = new(1.0);
    private SceneTargets _targets = new();
    private ModLogger _log = new("GoldenHour");
    private bool _initialized;
    private string _lastSceneName = "";
    private double _lastAppliedStrength = double.NaN;
    private int _lastLoggedWeightBucket = int.MinValue;
    private bool _timeUnreadLogged;

    public void OnInitializeMelon()
    {
        _log = new ModLogger("GoldenHour");
        ModConfig<GoldenHourConfig>.Initialize("GoldenHour", _log);
        _reapply = new ReapplyPolicy(ModConfig<GoldenHourConfig>.Instance.ReapplyIntervalSeconds);
        _initialized = true;
        _log.Info("initialized — Ctrl+Shift+F7 toggles the golden-hour look (Main scene only).");
    }

    public void OnUpdate()
    {
        if (!_initialized) return;

        LogSceneChangeOnce();

        if (!NetworkGuard.IsInMainScene)
        {
            HandleSceneExit("left Main scene");
            return;
        }

        if (HotkeyFired()) OnToggleRequested();

        if (!ModConfig<GoldenHourConfig>.Instance.ApplyEveryLateUpdate
            && _toggle.IsActive
            && _reapply.IsDue(Time.unscaledTime))
        {
            ApplyNow(reapply: true);
        }
    }

    public void OnLateUpdate()
    {
        // Default path (0.1.4): the game's day/night grading rewrites these values
        // every frame (measured 2026-10-17), so the bias must be re-applied per frame
        // or it loses every frame race and never shows on screen.
        if (_initialized
            && ModConfig<GoldenHourConfig>.Instance.ApplyEveryLateUpdate
            && _toggle.IsActive
            && NetworkGuard.IsInMainScene)
        {
            ApplyNow(reapply: true);
        }
    }

    private void LogSceneChangeOnce()
    {
        try
        {
            Scene scene = SceneManager.GetActiveScene();
            string name = scene.IsValid() ? scene.name : "<invalid>";
            if (name != _lastSceneName)
            {
                _lastSceneName = name;
                _log.Info($"scene '{name}' (Main guard: {NetworkGuard.IsInMainScene}).");
            }
        }
        catch
        {
            // Best-effort breadcrumb only; a scene query can throw during teardown.
        }
    }

    public void OnApplicationQuit()
    {
        HandleSceneExit("game shutting down");
        ModConfig<GoldenHourConfig>.Flush();
    }

    public void OnDeinitializeMelon()
    {
        // Runtime unload path (MelonLoader disable/unload): restore before teardown.
        HandleSceneExit("mod deinitialized");
    }

    private bool HotkeyFired()
    {
        try
        {
            if (!Application.isFocused) return false;
            if (HotkeyManager.IsInputFieldFocused()) return false;
            bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            return ctrl && shift && Input.GetKeyDown(ToggleKey);
        }
        catch
        {
            return false;
        }
    }

    private void OnToggleRequested()
    {
        try
        {
            if (_toggle.Toggle())
            {
                _store.Clear();
                _targets.InvalidateRuntimeCache();
                if (!_targets.Snapshot(_store, _log))
                {
                    _log.Warn("no parameters readable — activation aborted (fail-soft).");
                    _toggle.Toggle();
                }
                else
                {
                    _toggle.MarkSnapshotTaken();
                    _targets.LogDiagnostics(_log);
                    ApplyNow(reapply: false);
                }
            }
            else
            {
                Deactivate();
            }
        }
        catch (Exception ex)
        {
            FailSoft(ex);
        }
    }

    private void ApplyNow(bool reapply)
    {
        try
        {
            GoldenHourConfig config = ModConfig<GoldenHourConfig>.Instance;
            double configured = config.Strength;
            bool diagnostic = config.DiagnosticStrongTint;

            // The clock decides when the warm bias is visible at all. DiagnosticStrongTint
            // is the fixed "is the URP path alive" probe and deliberately ignores the clock.
            double weight = 1.0;
            int minutes = -1;
            if (!diagnostic)
            {
                if (TryReadMinutes(out minutes))
                {
                    weight = DaylightPolicy.Weight(minutes);
                }
                else
                {
                    // Fail soft: an unreadable clock must not paint full gold.
                    weight = 0.0;
                    if (!_timeUnreadLogged)
                    {
                        _timeUnreadLogged = true;
                        _log.Warn("in-game time unreadable — warm bias released until the clock is available.");
                    }
                }
            }

            double strength = configured * weight;
            if (!double.IsFinite(strength) || strength < 0.0) strength = 0.0;
            else if (strength > 1.0) strength = 1.0;

            int applied = _targets.Apply(_store, strength, diagnostic, _log, _lastAppliedStrength);
            _lastAppliedStrength = strength;
            _reapply.MarkApplied(Time.unscaledTime);
            LogDaylight(reapply, applied, strength, weight, minutes);
        }
        catch (Exception ex)
        {
            FailSoft(ex);
        }
    }

    private void LogDaylight(bool reapply, int applied, double strength, double weight, int minutes)
    {
        // Bucket the weight so the per-frame path logs on a visible change only.
        int bucket = (int)Math.Round(weight * 20.0);
        if (!reapply || bucket != _lastLoggedWeightBucket)
        {
            _lastLoggedWeightBucket = bucket;
            if (!reapply)
            {
                _log.Info($"golden hour ON — bias applied to {applied}/7 parameters (strength {strength:0.00}, daylight {weight:0.00}, minutes {minutes}).");
            }
            else
            {
                _log.Info($"daylight weight {weight:0.00} at {minutes} min — applied strength {strength:0.00}.");
            }
        }
    }

    private static bool TryReadMinutes(out int minutes)
    {
        minutes = -1;
        try
        {
            if (!NetworkGuard.IsAlive(NetworkSingleton<TimeManager>.Instance)) return false;
            int current = NetworkSingleton<TimeManager>.Instance.CurrentTime;
            if (current < 0 || current >= 1440) return false;
            minutes = current;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void Deactivate()
    {
        try
        {
            int restored = _targets.Restore(_store, _log);
            _store.Clear();
            _lastAppliedStrength = double.NaN;
            _lastLoggedWeightBucket = int.MinValue;
            _log.Info($"golden hour OFF — restored {restored} parameters to originals.");
        }
        catch (Exception ex)
        {
            FailSoft(ex);
        }
    }

    private void HandleSceneExit(string reason)
    {
        if (!_toggle.IsActive) return;

        _log.Warn("active while " + reason + " — restoring.");
        try
        {
            if (NetworkGuard.IsInMainScene)
            {
                _targets.Restore(_store, _log);
            }
            else
            {
                _targets.RestoreAssetLevel(_store, _log);
            }
        }
        catch (Exception ex)
        {
            _log.Warn("restore during scene exit failed", ex);
        }
        _store.Clear();
        _lastAppliedStrength = double.NaN;
        _lastLoggedWeightBucket = int.MinValue;
        _toggle.Toggle();
        _targets.InvalidateRuntimeCache();
    }

    private void FailSoft(Exception ex)
    {
        _log.Warn("golden hour disabled after error", ex);
        try
        {
            _targets.Restore(_store, _log);
        }
        catch (Exception restoreError)
        {
            _log.Warn("restore after error failed", restoreError);
        }
        _store.Clear();
        _lastAppliedStrength = double.NaN;
        _lastLoggedWeightBucket = int.MinValue;
        if (_toggle.IsActive) _toggle.Toggle();
    }
}

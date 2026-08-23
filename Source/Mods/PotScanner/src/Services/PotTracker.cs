using System;
using System.Collections.Generic;
using Il2CppScheduleOne.Growing;
using MelonLoader;
using PotScanner.Utils;
using S1API.Property;
using UnityEngine;

namespace PotScanner.Services;

/// <summary>
/// Singleton service that periodically scans the scene for placed plant pots
/// (GrowContainer instances) and tracks their state (water/soil/growth/plant).
///
/// Discovery uses FindObjectsByType&lt;GrowContainer&gt; (Unity 2022.3 API).
/// Plant access uses GetComponentInChildren&lt;Plant&gt; — Plant is a child MonoBehaviour
/// of the pot, not a field on GrowContainer.
/// Property assignment uses BaseProperty.IsPointInside(pos) (3.1.7 verified public abstract).
///
/// Tracking uses IntPtr (GrowContainer.Pointer) instead of GCHandle because IL2CPP
/// wrapper GC behavior is unreliable — the managed wrapper can be collected while
/// the native object lives, or vice versa.
/// </summary>
public sealed class PotTracker
{
    public static PotTracker Instance { get; } = new();

    public IReadOnlyList<PotInfo> Pots => _pots;

    /// <summary>Fired after every completed Refresh() (every 2s while active). UI subscribes here.</summary>
    public event Action? OnPotsScanned;

    private readonly List<PotInfo> _pots = new();
    private readonly List<IntPtr> _seen = new(64);
    private readonly List<PropertyWrapper> _propertyCache = new();
    private readonly HashSet<string> _ownedPropertyCodes = new();

    private float _elapsed;
    private float _postLoadTimer;
    private int _postLoadScansRemaining;
    private bool _active;
    private int _scanCount;

    public int TotalScans => _scanCount;

    /// <summary>
    /// Called by Mod when S1API fires GameLifecycle.OnSaveInfoLoaded.
    /// The save's metadata has been parsed; Property.OwnedProperties is now populated.
    /// We refresh the property cache eagerly so the very first scene-load sees owned state.
    /// Replaces v0.2.0's 25s retry mechanism.
    /// </summary>
    public void OnSaveInfoLoaded()
    {
        RefreshPropertyCache();
        MelonLogger.Msg($" OnSaveInfoLoaded: owned set = [{string.Join(",", _ownedPropertyCodes)}]");
    }

    /// <summary>Called by Mod on gameplay scene-load. Re-scans immediately.</summary>
    public void OnGameplaySceneLoaded()
    {
        _active = true;
        _elapsed = 0f;
        _postLoadTimer = 0f;
        _postLoadScansRemaining = 6;
        _pots.Clear();
        RefreshPropertyCache();
        Refresh();
        MelonLogger.Msg($" Tracking started; {_pots.Count} pot(s) found (fast post-load active).");
    }

    /// <summary>
    /// Called by Mod on gameplay scene-unload. Drops all IntPtrs (they'd be invalid
    /// after the gameplay scene is gone) and the property cache, and notifies the UI
    /// so any open PhoneApp clears immediately instead of waiting for the next scan.
    /// Defensive: multiple calls are no-ops.
    /// </summary>
    public void OnSceneUnloaded()
    {
        if (!_active) return;

        _active = false;
        _elapsed = 0f;
        _pots.Clear();
        _propertyCache.Clear();
        _ownedPropertyCodes.Clear();

        MelonLogger.Msg(" Tracking stopped; cached pots cleared.");
        NotifyPotsScanned();
    }

    /// <summary>Called by Mod every frame. Triggers refresh at PotRefreshIntervalSec cadence.</summary>
    public void Tick(float dt)
    {
        if (!_active) return;

        if (_postLoadScansRemaining > 0)
        {
            _postLoadTimer += dt;
            if (_postLoadTimer >= 0.3f)
            {
                _postLoadTimer = 0f;
                _postLoadScansRemaining--;
                Refresh();
            }
            return;
        }

        _elapsed += dt;
        if (_elapsed < Constants.PotRefreshIntervalSec) return;
        _elapsed = 0f;
        Refresh();
    }

    /// <summary>Force next Tick to refresh (used by optional MinPass-Hook in iter 2).</summary>
    public void MarkDirty()
    {
        _elapsed = Constants.PotRefreshIntervalSec;
    }

    /// <summary>
    /// Force an immediate scan + fire <see cref="OnPotsScanned"/> synchronously.
    /// Used by WaterAllService so the UI updates within the same frame as the click,
    /// without waiting up to 2s for the next Tick.
    /// </summary>
    public void RefreshNow()
    {
        if (!_active) return;
        RefreshPropertyCache();
        Refresh();
    }

    private void RefreshPropertyCache()
    {
        _propertyCache.Clear();
        _ownedPropertyCodes.Clear();
        try
        {
            // v0.2.1: simple, clean. S1API's GameLifecycle.OnSaveInfoLoaded fires before
            // this method is called, so PropertyManager returns correct owned data
            // (v0.2.0 needed a 25s retry because this was called from OnGameplaySceneLoaded).
            var all = PropertyManager.GetAllProperties();
            if (all != null)
            {
                foreach (var p in all)
                {
                    if (p == null || p.Equals(null)) continue;
                    var code = p.PropertyCode;
                    if (p.IsOwned && !string.IsNullOrEmpty(code))
                    {
                        _ownedPropertyCodes.Add(code);
                    }
                    _propertyCache.Add(p);
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($" PropertyManager failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Plan C fallback: read native _IsOwned_k__BackingField via reflection.
    /// Only triggers when save-load is done (NativeProperty.OwnedProperties populated)
    /// but S1API's PropertyWrapper.IsOwned still returns false. Belt-and-suspenders.
    /// </summary>
    private void RefreshPropertyCacheViaReflection(List<PropertyWrapper>? allWrappers)
    {
        // REMOVED in v0.2.1 — the S1API OnSaveInfoLoaded hook makes this unnecessary.
    }

    private void Refresh()
    {
        _seen.Clear();
        _scanCount++;

        GrowContainer[] containers;
        try
        {
            containers = UnityEngine.Object.FindObjectsByType<GrowContainer>(FindObjectsSortMode.None);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($" FindObjectsByType<GrowContainer> failed: {ex.Message}");
            return;
        }

        foreach (var c in containers)
        {
            if (c == null || c.Equals(null)) continue;
            IntPtr ptr;
            try { ptr = c.Pointer; }
            catch { continue; }
            if (ptr == IntPtr.Zero) continue;

            _seen.Add(ptr);

            var info = FindByPtr(ptr);
            if (info == null)
            {
                info = new PotInfo { NativePtr = ptr };
                _pots.Add(info);
            }
            UpdateInfo(info, c);
        }

        // Remove pots that disappeared from the scene
        _pots.RemoveAll(p => !_seen.Contains(p.NativePtr));

        // Auto-Water tick if enabled (reuses discovered containers)
        if (AutoWaterService.IsEnabled)
        {
            AutoWaterService.WaterTick(_pots, containers);
        }

        // Notify UI subscribers (PotScannerApp listens here)
        NotifyPotsScanned();
    }

    private void NotifyPotsScanned()
    {
        try { OnPotsScanned?.Invoke(); }
        catch (Exception ex)
        {
            MelonLogger.Warning($" OnPotsScanned subscriber threw: {ex.Message}");
        }
    }

    private void UpdateInfo(PotInfo info, GrowContainer c)
    {
        // Read public getters — verified 2026-08-04 via ilspycmd
        try
        {
            info.WaterPercent = c.NormalizedMoistureAmount;
            info.SoilPercent = c.NormalizedSoilAmount;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($" Read failed on pot @ {info.NativePtr:X}: {ex.Message}");
            return;
        }

        // Position snapshot (struct copy)
        try
        {
            var t = c.transform;
            if (t != null) info.WorldPosition = t.position;
        }
        catch { /* transform destroyed */ }

        // Plant is a child MonoBehaviour of the pot (NOT a field on GrowContainer)
        Plant? plant = null;
        try { plant = c.GetComponentInChildren<Plant>(); }
        catch { /* ok, no plant */ }

        if (plant != null)
        {
            try
            {
                info.GrowthPercent = plant.NormalizedGrowthProgress;
                info.IsFullyGrown = plant.IsFullyGrown;
                info.Quality = plant.QualityLevel;
                var seedDef = plant.SeedDefinition;
                info.PlantName = seedDef != null ? (seedDef.Name ?? "(unnamed)") : "(unnamed)";
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($" Plant read failed: {ex.Message}");
            }
        }
        else
        {
            info.GrowthPercent = 0f;
            info.IsFullyGrown = false;
            info.Quality = 0f;
            info.PlantName = string.Empty;
        }

        // Property lookup
        var (code, name) = FindPropertyAt(info.WorldPosition);
        info.PropertyCode = code;
        info.PropertyName = name;
        info.IsOwnedProperty = !string.IsNullOrEmpty(code) && _ownedPropertyCodes.Contains(code);
        info.LastScanned = DateTime.UtcNow;
    }

    private (string code, string name) FindPropertyAt(Vector3 pos)
    {
        for (int i = 0; i < _propertyCache.Count; i++)
        {
            var p = _propertyCache[i];
            if (p == null || p.Equals(null)) continue;
            try
            {
                if (p.IsPointInside(pos))
                {
                    return (p.PropertyCode ?? Constants.UnknownPropertyCode,
                            p.PropertyName ?? string.Empty);
                }
            }
            catch { /* skip */ }
        }
        return (Constants.UnknownPropertyCode, string.Empty);
    }

    public PotInfo? FindByPtr(IntPtr ptr)
    {
        for (int i = 0; i < _pots.Count; i++)
        {
            if (_pots[i].NativePtr == ptr) return _pots[i];
        }
        return null;
    }
}

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

    /// <summary>Fired after every completed Refresh() (2s cadence while the app is open or Auto-Water is on, else 10s). UI subscribes here.</summary>
    public event Action? OnPotsScanned;

    private readonly List<PotInfo> _pots = new();
    private readonly HashSet<IntPtr> _seen = new(64);
    private readonly List<PropertyWrapper> _propertyCache = new();
    private readonly HashSet<string> _ownedPropertyCodes = new();

    private float _elapsed;
    private float _postLoadTimer;
    private int _postLoadScansRemaining;
    private bool _active;

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

    /// <summary>Called by Mod every frame. Scans every 2s while the phone app is open or Auto-Water is
    /// enabled; otherwise the cadence relaxes to PotIdleRefreshIntervalSec (nothing consumes the data).</summary>
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
        float interval = (AutoWaterService.IsEnabled || PotScannerApp.IsAppOpen)
            ? Constants.PotRefreshIntervalSec
            : Constants.PotIdleRefreshIntervalSec;
        if (_elapsed < interval) return;
        _elapsed = 0f;
        Refresh();
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

    private void Refresh()
    {
        _seen.Clear();

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

        // Remove pots that disappeared from the scene (linear in-place compaction, order preserved)
        int write = 0;
        for (int read = 0; read < _pots.Count; read++)
        {
            if (_seen.Contains(_pots[read].NativePtr))
                _pots[write++] = _pots[read];
        }
        if (write < _pots.Count)
            _pots.RemoveRange(write, _pots.Count - write);

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
        // Fix (Bug-Audit 2026-09-10): a single throwing subscriber starved all later ones —
        // invoke each handler in its own try/catch instead of one around the multicast call.
        var handlers = OnPotsScanned?.GetInvocationList();
        if (handlers == null) return;
        foreach (var handler in handlers)
        {
            try { ((Action)handler)(); }
            catch (Exception ex)
            {
                MelonLogger.Warning($" OnPotsScanned subscriber threw: {ex.Message}");
            }
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

        // Fix 4.1 (Bug-Audit 2026-09-02): pots placed outdoors via HomelessMod's street
        // system live under "StreetNomad_WorldRoot" and have no property code (always
        // Constants.UnknownPropertyCode), so the old check marked them permanently
        // foreign and WaterAll/AutoWater refused to water them.
        bool isStreetPot = false;
        if (code == Constants.UnknownPropertyCode)
        {
            try
            {
                var t = c.transform;
                while (t != null && t.Pointer != IntPtr.Zero)
                {
                    if (t.name == "StreetNomad_WorldRoot") { isStreetPot = true; break; }
                    t = t.parent;
                }
            }
            catch { /* transform destroyed mid-scan */ }
        }

        info.IsOwnedProperty = isStreetPot ||
            (!string.IsNullOrEmpty(code) && code != Constants.UnknownPropertyCode && _ownedPropertyCodes.Contains(code));
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

/// <summary>
/// Read-only snapshot of a placed pot in the world. Holds an IntPtr (not a managed reference)
/// to the underlying IL2CPP GrowContainer wrapper, so we can detect destroyed objects via
/// pointer comparison instead of weak GC handles (IL2CPP wrapper GC behavior is unreliable).
/// </summary>
public sealed class PotInfo
{
    /// <summary>Native pointer of the GrowContainer IL2CPP wrapper. Survives scenes; valid until destroyed.</summary>
    public IntPtr NativePtr { get; set; }

    /// <summary>Property code (e.g. "barn", "bungalow") the pot sits in. "unknown" if not inside any owned property.</summary>
    public string PropertyCode { get; set; } = string.Empty;

    /// <summary>Property display name (e.g. "Barn").</summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// True iff the pot sits on a property the player currently owns. Set by PotTracker
    /// against PropertyManager.GetOwnedProperties() — independent of the display's
    /// Owned→All-fallback. Used by WaterAllService to filter targets.
    /// </summary>
    public bool IsOwnedProperty { get; set; }

    /// <summary>World position snapshot (struct copy — safe, no Unity reference held).</summary>
    public Vector3 WorldPosition { get; set; }

    /// <summary>Water level normalized 0..1 (from GrowContainer.NormalizedMoistureAmount).</summary>
    public float WaterPercent { get; set; }

    /// <summary>Soil level normalized 0..1.</summary>
    public float SoilPercent { get; set; }

    /// <summary>Plant growth normalized 0..1. 0 if no plant in pot.</summary>
    public float GrowthPercent { get; set; }

    /// <summary>Plant display name (SeedDefinition.Name). Empty if no plant.</summary>
    public string PlantName { get; set; } = string.Empty;

    /// <summary>Plant quality (0..1). 0 if no plant.</summary>
    public float Quality { get; set; }

    /// <summary>Whether the plant is fully grown.</summary>
    public bool IsFullyGrown { get; set; }

    /// <summary>Last scan timestamp (UTC).</summary>
    public DateTime LastScanned { get; set; }
}

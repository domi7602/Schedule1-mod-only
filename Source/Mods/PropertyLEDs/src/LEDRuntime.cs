using System;
using System.Collections.Generic;
using System.Diagnostics;
using PropertyLEDs.Core;
using S1Mods.Shared;

namespace PropertyLEDs;

/// <summary>Single main-thread facade shared by the phone app and explicit diagnostics.</summary>
internal static class LEDRuntime
{
    private static readonly Stopwatch Clock = Stopwatch.StartNew();
    private static readonly PropertyRegistry Registry = new();
    private static readonly RoomLayoutRegistry Layouts = new();
    private static readonly LEDLightingController Lighting = new();
    private static LEDSettingsStore? _store;
    private static bool _loaded;
    // S1API Property/Business expose no purchase event, and the owned lists can still be empty when
    // OnLoadComplete fires. Retry a bounded number of times instead of a per-frame or scene-wide scan.
    private const int MaxOwnershipRetries = 30;
    private const double OwnershipRetrySeconds = 1.0;
    private static int _ownershipRetries;
    private static double _nextOwnershipRetryAt;

    internal static bool Ready => _loaded && _store != null && _store.SlotMatches;
    internal static int Revision { get; private set; }
    internal static IReadOnlyList<PropertyEntry> Properties => Ready ? Registry.Entries : Array.Empty<PropertyEntry>();
    internal static bool ReadOnly => _store?.ReadOnly ?? true;
    internal static int LoadedSlot => _store?.LoadedSlot ?? -1;
    internal static string? SettingsPath => _store?.CapturedPath;
    internal static int RenderedRoomCount => Lighting.RoomCount;
    internal static bool HasPreview => Lighting.HasPreview;
    internal static string ShaderName => Lighting.ShaderName;
    internal static int RealtimeLightCount => Lighting.RealtimeLightCount;

    internal static void Initialize()
    {
        _store ??= LEDSettingsStore.Create(() => Clock.Elapsed.TotalSeconds);
    }

    internal static IReadOnlyList<RoomLayout> GetRooms(string propertyId)
        => Ready && Registry.Find(propertyId) != null ? Layouts.GetRooms(propertyId) : Array.Empty<RoomLayout>();

    /// <summary>Catalog room count by id for read-only audits (no ownership requirement).</summary>
    internal static int CatalogRoomCount(string propertyId) => Layouts.GetRooms(propertyId).Count;

    /// <summary>All catalog property ids for read-only audits.</summary>
    internal static IReadOnlyList<string> CatalogPropertyIds() => Layouts.GetPropertyIds();

    internal static LEDSettings GetSettings(string propertyId, string? roomId = null)
        => ValidTarget(propertyId, roomId) && _store != null
            ? _store.GetEffective(propertyId, roomId) : new LEDSettings();

    internal static bool HasOverride(string propertyId, string? roomId)
        => ValidTarget(propertyId, roomId) && !string.IsNullOrWhiteSpace(roomId) && _store is { ReadOnly: false } &&
            _store.HasOverride(propertyId, roomId!);

    internal static void Apply(string propertyId, string? roomId, LEDSettings settings)
    {
        if (settings == null || !ValidTarget(propertyId, roomId) || _store == null || !_store.CanEdit) return;
        // The store owns slot/debounce bookkeeping; a rejected edit never dirties the file.
        bool changed = roomId == null
            ? _store.SetGlobal(propertyId, settings)
            : _store.SetRoom(propertyId, roomId, settings);
        if (!changed) return;
        // Global edits clear all room overrides in core, so recolor EVERY room for that property.
        Lighting.Apply(propertyId, roomId, GetSettings);
        Revision++;
    }

    internal static void UsePropertyColor(string propertyId, string? roomId)
    {
        if (!ValidTarget(propertyId, roomId) || string.IsNullOrWhiteSpace(roomId) || _store == null || !_store.CanEdit) return;
        if (!_store.UseProperty(propertyId, roomId!)) return;
        Lighting.Apply(propertyId, roomId, GetSettings);
        Revision++;
    }

    internal static void Refresh()
    {
        if (!Ready) return;
        Registry.Refresh();
        Lighting.Rebuild(Registry, Layouts, GetSettings);
        Revision++;
        // S1API Property/Business expose NO purchase event. Refresh is deliberate (app/manual),
        // not a native global delegate patch or a per-frame property search.
    }

    internal static void ReloadLayouts()
    {
        if (!Ready) return;
        Layouts.Reload();
        Lighting.Rebuild(Registry, Layouts, GetSettings);
        Revision++;
    }

    internal static void PreviewLayout(RoomLayout layout)
    {
        if (!Ready || layout == null) return;
        var property = Registry.Find(layout.PropertyId);
        if (property == null)
        {
            Mod.Log.Warn("LED preview skipped: property is unknown, not owned, ambiguous or no longer live.");
            return;
        }
        // External unverified coordinates are accepted ONLY through this explicit path.
        Lighting.Preview(property, layout);
        Revision++;
    }

    internal static void ClearPreview()
    {
        Lighting.ClearPreview();
        Revision++;
    }

    internal static void LoadComplete()
    {
        Initialize();
        Reset();
        if (!_store!.Load())
        {
            Mod.Log.Warn("LED runtime waiting for a valid save slot; no settings file or room objects created.");
            return;
        }
        _loaded = true;
        Registry.Refresh();
        // The owned list may still be empty here; arm the bounded retry until it fills.
        _ownershipRetries = 0;
        _nextOwnershipRetryAt = Clock.Elapsed.TotalSeconds + OwnershipRetrySeconds;
        Layouts.Reload();
        Lighting.Rebuild(Registry, Layouts, GetSettings);
        Revision++;
        if (Ready) LEDDiagnostics.EnsureBridge();
        Mod.Log.Info($"LED runtime ready for slot {_store.LoadedSlot}: {Registry.Entries.Count} owned properties/businesses.");
    }

    internal static void Tick()
    {
        // No scene/property/material scans: only a managed pending-write/debounce check plus a
        // bounded ownership retry while the game's own owned lists are still empty.
        if (!_loaded) return;
        _store?.Flush(false);
        if (_ownershipRetries >= MaxOwnershipRetries || Registry.Entries.Count > 0) return;
        double now = Clock.Elapsed.TotalSeconds;
        if (now < _nextOwnershipRetryAt) return;
        _ownershipRetries++;
        _nextOwnershipRetryAt = now + OwnershipRetrySeconds;
        if (!NetworkGuard.IsInMainScene) return;
        Registry.Refresh();
        if (Registry.Entries.Count == 0)
        {
            if (_ownershipRetries == MaxOwnershipRetries)
                Mod.Log.Warn($"Owned property list stayed empty after {MaxOwnershipRetries} s; LEDs stay off until an explicit refresh.");
            return;
        }
        Lighting.Rebuild(Registry, Layouts, GetSettings);
        Revision++;
        Mod.Log.Info($"Ownership data arrived after {_ownershipRetries} s: {Registry.Entries.Count} owned properties/businesses.");
    }

    internal static void Flush() => _store?.Flush();

    internal static void Reset()
    {
        _ownershipRetries = MaxOwnershipRetries; // no retries until the next load arms them
        _nextOwnershipRetryAt = 0;
        // Captured-slot flush occurs BEFORE resetting scene references or the loaded state.
        _store?.Reset();
        _loaded = false;
        LEDDiagnostics.Reset();
        Lighting.Dispose();
        Registry.Clear();
        Layouts.Clear();
        Revision++;
    }

    private static bool ValidTarget(string? propertyId, string? roomId)
        => Ready && !string.IsNullOrWhiteSpace(propertyId) && Registry.Find(propertyId) != null &&
            (roomId == null || (!string.IsNullOrWhiteSpace(roomId) && Layouts.Contains(propertyId, roomId)));
}

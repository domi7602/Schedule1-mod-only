using System;
using System.Collections.Generic;

namespace PropertyLEDs.Core;

/// <summary>
/// Owns the phone app's property selection. The first resolution after a load may adopt the
/// game's exact current property identity; every later resolution keeps the shown or manually
/// picked property, so closing and reopening the app never jumps back to the first entry.
/// Exact property codes only: no positions, distances or nearest-match guesses.
/// </summary>
public sealed class PropertySelection
{
    private bool _adoptCurrent = true;

    /// <summary>Property code currently shown, or null before the first resolution.</summary>
    public string? ShownId { get; private set; }

    /// <summary>True while the next resolution may adopt the exact current property.</summary>
    public bool CanAdoptCurrent => _adoptCurrent && string.IsNullOrEmpty(ShownId);

    /// <summary>
    /// Keeps a still-owned shown id, otherwise adopts the exact current property (first resolution
    /// after a load only), otherwise falls back to the first owned property.
    /// </summary>
    public string? Resolve(IReadOnlyList<string>? ownedIds, string? currentPropertyId)
    {
        // An empty list is not a decision: keep adoption armed for when the owned data arrives.
        if (ownedIds == null || ownedIds.Count == 0)
        {
            ShownId = null;
            return null;
        }
        if (!string.IsNullOrEmpty(ShownId) && Contains(ownedIds, ShownId!))
        {
            _adoptCurrent = false;
            return ShownId;
        }
        bool adopt = _adoptCurrent;
        _adoptCurrent = false;
        if (adopt && !string.IsNullOrEmpty(currentPropertyId) && Contains(ownedIds, currentPropertyId!))
        {
            ShownId = currentPropertyId;
            return ShownId;
        }
        ShownId = ownedIds[0];
        return ShownId;
    }

    /// <summary>Explicit user choice; later resolutions keep it until the next load.</summary>
    public void Select(string propertyId)
    {
        if (string.IsNullOrWhiteSpace(propertyId)) return;
        ShownId = propertyId;
        _adoptCurrent = false;
    }

    /// <summary>Load/scene boundary: the next resolution may adopt the newly loaded property.</summary>
    public void Invalidate()
    {
        ShownId = null;
        _adoptCurrent = true;
    }

    private static bool Contains(IReadOnlyList<string> ids, string id)
    {
        for (int i = 0; i < ids.Count; i++)
            if (string.Equals(ids[i], id, StringComparison.Ordinal)) return true;
        return false;
    }
}

/// <summary>Power-card text names the selected property alongside its configured state.</summary>
public static class PropertyStatus
{
    public static string Label(string? displayName, bool enabled)
    {
        string state = enabled ? "LEDs enabled" : "LEDs disabled";
        return string.IsNullOrWhiteSpace(displayName) ? state : state + ": " + displayName!.Trim();
    }

    public static string Detail(bool enabled)
        => enabled ? "Property LEDs are currently on" : "Property LEDs are currently off";
}

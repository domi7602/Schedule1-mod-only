using System;
using UnityEngine;

namespace PotScanner.Services;

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

    /// <summary>One-line summary for log/UI display.</summary>
    public string Summary
    {
        get
        {
            var plantPart = string.IsNullOrEmpty(PlantName)
                ? "(empty)"
                : $"{PlantName} G:{(GrowthPercent * 100):0}% Q:{(Quality * 100):0}";
            return $"W:{(WaterPercent * 100):0}% S:{(SoilPercent * 100):0}% | {plantPart}";
        }
    }
}

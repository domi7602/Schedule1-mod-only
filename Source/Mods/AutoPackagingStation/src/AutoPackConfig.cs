using System;

namespace AutoPackagingStation.Config;

/// <summary>
/// Configuration settings for the Auto-Packaging Station mod.
/// </summary>
public class AutoPackConfig
{
    /// <summary>
    /// Unique item ID for the Auto-Packaging Station in the game registry.
    /// </summary>
    public string StationItemId { get; set; } = "autopackagingstation";

    /// <summary>
    /// Display name shown in UI, shops, and tooltips.
    /// </summary>
    public string StationDisplayName { get; set; } = "Industrial Auto-Packaging Station";

    /// <summary>
    /// Base purchase price in the hardware store.
    /// </summary>
    public float StationPurchasePrice { get; set; } = 3500f;

    /// <summary>
    /// Duration in seconds required to complete one packaging cycle (default 2.0s).
    /// </summary>
    public float PackagingDurationSeconds { get; set; } = 2.0f;

    /// <summary>
    /// Quality multiplier bonus applied to packaged goods (+5% freshness bonus = 0.05).
    /// </summary>
    public float FreshnessBonusMultiplier { get; set; } = 0.05f;

    /// <summary>
    /// Enable pneumatic piston hiss and mechanical compressor sound effects during packaging.
    /// </summary>
    public bool EnablePneumaticSFX { get; set; } = true;

    /// <summary>
    /// Visual UV scrolling speed for the animated conveyor belt surface.
    /// </summary>
    public float ConveyorScrollSpeed { get; set; } = 0.5f;

    /// <summary>
    /// Show illuminated multi-state status LEDs (Green/Amber/Red/Cyan) on the station.
    /// </summary>
    public bool ShowStatusLeds { get; set; } = true;

    /// <summary>
    /// Maximum units packaged per automated cycle batch.
    /// </summary>
    public int MaxBatchSize { get; set; } = 10;

    /// <summary>
    /// Show interactive 3D HUD / proximity status overlay when near the station.
    /// </summary>
    public bool EnableProximityHUD { get; set; } = false;

    /// <summary>
    /// Maximum player interaction distance in meters.
    /// </summary>
    public float InteractionRange { get; set; } = 3.5f;
}

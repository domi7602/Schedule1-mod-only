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
    /// Quality bonus applied to packaged goods. Bug-audit 2026-10-13 (BUG-010):
    /// the old "quality value * (1 + bonus)" math never crossed a discrete tier
    /// boundary, so the bonus did nothing. The mechanic is now chance-based:
    /// every successful packaging cycle rolls once and with probability = this
    /// multiplier the output quality rises by one tier (capped at Heavenly).
    /// 0.05 = 5% chance per cycle. Valid range [0.0, 1.0].
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
    /// Maximum player interaction distance in meters.
    /// </summary>
    public float InteractionRange { get; set; } = 3.5f;

    /// <summary>
    /// Bug-audit 2026-10-13 (BUG-019): validate every loaded value through
    /// ConfigPolicy (non-finite -> default, out-of-range -> clamped, empty
    /// StationItemId -> default) and report each correction via the callback.
    /// Pure — no Unity/S1API dependencies, covered by AutoPackagingStation.Tests.
    /// </summary>
    public void SanitizeInPlace(Action<string>? report = null)
    {
        var sanitized = ConfigPolicy.Sanitize(new ConfigValues(
            PackagingDurationSeconds,
            FreshnessBonusMultiplier,
            MaxBatchSize,
            StationPurchasePrice,
            InteractionRange,
            ConveyorScrollSpeed,
            StationItemId));

        PackagingDurationSeconds = sanitized.PackagingDurationSeconds;
        FreshnessBonusMultiplier = sanitized.FreshnessBonusMultiplier;
        MaxBatchSize = sanitized.MaxBatchSize;
        StationPurchasePrice = sanitized.StationPurchasePrice;
        InteractionRange = sanitized.InteractionRange;
        ConveyorScrollSpeed = sanitized.ConveyorScrollSpeed;
        StationItemId = sanitized.StationItemId;

        if (report != null)
        {
            for (int i = 0; i < sanitized.Corrections.Count; i++)
            {
                report($"AutoPackConfig corrected: {sanitized.Corrections[i]}");
            }
        }
    }
}

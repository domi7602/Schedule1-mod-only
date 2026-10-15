using System;
using System.Collections.Generic;

namespace AutoPackagingStation;

/// <summary>
/// Pure decision policies for AutoPackagingStation. No Unity, no S1API, no
/// MelonLoader — linked into AutoPackagingStation.Tests so every rule below is
/// covered by unit tests. These are models of runtime decisions; they do not
/// prove IL2CPP slot or inventory behaviour.
/// </summary>
public static class RegistrationPolicy
{
    public static bool CanReuse(bool sameDefinition, bool samePrefab, bool ghostPrepared) =>
        sameDefinition && samePrefab && ghostPrepared;
}

public static class FootprintPolicy
{
    public static bool IsCompleteFourByFour(IReadOnlyList<(int X, int Y)> cells)
    {
        if (cells.Count != 16) return false;
        var unique = new HashSet<(int X, int Y)>();
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;
        foreach (var cell in cells)
        {
            if (!unique.Add(cell)) return false;
            minX = Math.Min(minX, cell.X);
            minY = Math.Min(minY, cell.Y);
            maxX = Math.Max(maxX, cell.X);
            maxY = Math.Max(maxY, cell.Y);
        }
        return (long)maxX - minX == 3 && (long)maxY - minY == 3;
    }
}

public static class RestorePolicy
{
    /// <summary>
    /// Decide what a restore pass may do to one native slot.
    /// Rules (bug-audit 2026-10-13, BUG-001):
    ///  - A filled native slot ALWAYS wins — it is the source of truth. Saved or
    ///    runtime data must never overwrite or clear it (prevents the wipe that
    ///    occurred when Start() ran RestoreNativeSlots() after the staged restore
    ///    had already consumed the runtime copies).
    ///  - A record with items fills an EMPTY native slot (JSON as fallback source
    ///    for saves where the vanilla pipeline did not restore slot contents).
    ///  - An absent/empty record is NOT an instruction to clear anything.
    /// </summary>
    public static RestoreAction Decide(bool recordHasItem, bool nativeHasItem)
    {
        if (nativeHasItem) return RestoreAction.KeepNative;
        return recordHasItem ? RestoreAction.WriteFromRecord : RestoreAction.Skip;
    }
}

public enum RestoreAction
{
    /// <summary>Native slot is empty and the record carries items: write the record.</summary>
    WriteFromRecord,
    /// <summary>Native slot is filled: leave it alone — it is the source of truth.</summary>
    KeepNative,
    /// <summary>Nothing in the record and nothing in the slot: no-op.</summary>
    Skip
}

/// <summary>
/// Chance-based freshness upgrade (bug-audit 2026-10-13, BUG-010).
/// The old "quality value * 1.05" math never crossed a tier boundary for any of
/// the five discrete tiers (0.21/0.3675/0.5775/0.84/0.9975), so the advertised
/// +5% bonus did literally nothing. The visible mechanic is now: every
/// successful packaging cycle rolls once; with probability = configured
/// multiplier the OUTPUT quality rises by exactly one tier (capped at Heavenly).
/// </summary>
public static class QualityBonusPolicy
{
    public const int MaxTier = 4; // Heavenly

    /// <summary>
    /// Returns the output tier after the freshness roll.
    /// tier is clamped into [0, 4]; multiplier is clamped into [0, 1] (non-finite
    /// = 0); upgrade happens when roll01 &lt; multiplier, so a roll of exactly the
    /// threshold does not upgrade.
    /// </summary>
    public static int ApplyFreshnessUpgrade(int tier, float bonusMultiplier, float roll01)
    {
        // Garbage tier data is clamped but never receives a bonus.
        if (tier < 0 || tier > MaxTier) return tier < 0 ? 0 : MaxTier;
        int clampedTier = tier;

        float p = bonusMultiplier;
        if (float.IsNaN(p) || float.IsInfinity(p)) p = 0f;
        if (p < 0f) p = 0f;
        if (p > 1f) p = 1f;

        if (roll01 < p && clampedTier < MaxTier) return clampedTier + 1;
        return clampedTier;
    }
}

/// <summary>Plain value bundle consumed by <see cref="ConfigPolicy.Sanitize"/>.</summary>
public sealed record ConfigValues(
    float PackagingDurationSeconds,
    float FreshnessBonusMultiplier,
    int MaxBatchSize,
    float StationPurchasePrice,
    float InteractionRange,
    float ConveyorScrollSpeed,
    string StationItemId)
{
    /// <summary>Human-readable notes for every value Sanitize corrected.</summary>
    public List<string> Corrections { get; init; } = new();
}

/// <summary>
/// Config validation (bug-audit 2026-10-13, BUG-019). Rules:
///  - non-finite values fall back to the documented default,
///  - out-of-range values are clamped to the nearest bound,
///  - empty item IDs fall back to the default ID,
///  - every correction is reported so it shows up in the log.
/// </summary>
public static class ConfigPolicy
{
    public const float DefaultPackagingDuration = 2.0f;
    public const float MinPackagingDuration = 0.2f;
    public const float MaxPackagingDuration = 60f;
    public const float DefaultFreshnessBonus = 0.05f;
    public const int DefaultMaxBatchSize = 10;
    public const int MaxBatchSizeUpper = 20;
    public const float DefaultStationPurchasePrice = 3500f;
    public const float DefaultInteractionRange = 3.5f;
    public const float MinInteractionRange = 1f;
    public const float MaxInteractionRange = 5f;
    public const float DefaultConveyorScrollSpeed = 0.5f;
    public const float MaxConveyorScrollSpeed = 10f;
    public const string DefaultStationItemId = "autopackagingstation";

    public static ConfigValues Sanitize(ConfigValues input)
    {
        var corrections = new List<string>();

        float duration = SanitizeFloat(input.PackagingDurationSeconds, DefaultPackagingDuration,
            MinPackagingDuration, MaxPackagingDuration, "PackagingDurationSeconds", corrections);
        float freshness = SanitizeFloat(input.FreshnessBonusMultiplier, 0f,
            0f, 1f, "FreshnessBonusMultiplier", corrections);
        float price = SanitizeFloat(input.StationPurchasePrice, DefaultStationPurchasePrice,
            0f, float.MaxValue, "StationPurchasePrice", corrections);
        float range = SanitizeFloat(input.InteractionRange, DefaultInteractionRange,
            MinInteractionRange, MaxInteractionRange, "InteractionRange", corrections);
        float scroll = SanitizeFloat(input.ConveyorScrollSpeed, DefaultConveyorScrollSpeed,
            0f, MaxConveyorScrollSpeed, "ConveyorScrollSpeed", corrections);

        int batch = input.MaxBatchSize;
        if (batch < 1)
        {
            corrections.Add($"MaxBatchSize: {batch} -> 1");
            batch = 1;
        }
        else if (batch > MaxBatchSizeUpper)
        {
            corrections.Add($"MaxBatchSize: {batch} -> {MaxBatchSizeUpper}");
            batch = MaxBatchSizeUpper;
        }

        string itemId = input.StationItemId ?? string.Empty;
        if (string.IsNullOrWhiteSpace(itemId))
        {
            corrections.Add($"StationItemId: '{input.StationItemId}' -> '{DefaultStationItemId}'");
            itemId = DefaultStationItemId;
        }

        return new ConfigValues(duration, freshness, batch, price, range, scroll, itemId)
        {
            Corrections = corrections
        };
    }

    private static float SanitizeFloat(float value, float fallback, float min, float max,
        string field, List<string> corrections)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            corrections.Add($"{field}: non-finite -> {fallback}");
            return fallback;
        }
        if (value < min)
        {
            corrections.Add($"{field}: {value} -> {min} (clamped)");
            return min;
        }
        if (value > max)
        {
            corrections.Add($"{field}: {value} -> {max} (clamped)");
            return max;
        }
        return value;
    }
}

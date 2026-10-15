using System;
using Xunit;

namespace AutoPackagingStation.Tests;

/// <summary>
/// RED→GREEN contracts for the pure policy layer (AutoPackPolicies.cs).
/// These are models of the runtime decisions — they prove the decision tables,
/// not Unity/IL2CPP behaviour.
/// </summary>
public class PolicyTests
{
    // ================================================================
    // RestorePolicy — BUG-001: saved items must never be wiped at Start()
    // ================================================================

    [Theory]
    [InlineData(true, false, RestoreAction.WriteFromRecord)]   // record has items, native slot empty -> fill
    [InlineData(true, true, RestoreAction.KeepNative)]         // native slot filled -> native is the source of truth
    [InlineData(false, true, RestoreAction.KeepNative)]        // empty record must NEVER clear a filled native slot
    [InlineData(false, false, RestoreAction.Skip)]             // nothing anywhere -> nothing to do
    public void RestorePolicy_Decide_NeverWipesNativeSlots(bool recordHasItem, bool nativeHasItem, RestoreAction expected)
    {
        Assert.Equal(expected, RestorePolicy.Decide(recordHasItem, nativeHasItem));
    }

    [Fact]
    public void RestorePolicy_Decide_EmptyRuntimeDataNeverClearsFilledSlots()
    {
        // The BUG-001 wipe sequence: first restore writes slots + drops the runtime
        // copy, then Start() runs RestoreNativeSlots() with an empty record. The old
        // code cleared the slots here. The policy must say KeepNative.
        Assert.Equal(RestoreAction.KeepNative, RestorePolicy.Decide(recordHasItem: false, nativeHasItem: true));
    }

    // ================================================================
    // QualityBonusPolicy — BUG-010: a +5% tier-math bonus is a no-op on the
    // 5 discrete tiers; the visible mechanic is a chance-based tier upgrade.
    // ================================================================

    [Theory]
    [InlineData(2, 0.05f, 0.049f, 3)]   // lucky roll: one tier up
    [InlineData(2, 0.05f, 0.05f, 2)]    // roll == threshold: no upgrade
    [InlineData(2, 0.05f, 0.90f, 2)]    // unlucky roll: unchanged
    [InlineData(4, 1.00f, 0.50f, 4)]    // capped at Heavenly
    [InlineData(0, 1.00f, 0.99f, 1)]    // full multiplier always upgrades
    public void QualityBonusPolicy_ApplyFreshnessUpgrade_ChanceBasedOneTier(int tier, float multiplier, float roll, int expected)
    {
        Assert.Equal(expected, QualityBonusPolicy.ApplyFreshnessUpgrade(tier, multiplier, roll));
    }

    [Theory]
    [InlineData(2, -1.0f, 0.0f, 2)]       // negative multiplier -> no upgrade
    [InlineData(2, 2.0f, 0.999f, 3)]      // multiplier clamped to 1.0 -> always upgrades
    [InlineData(2, float.NaN, 0.0f, 2)]   // non-finite multiplier -> no upgrade
    [InlineData(-3, 1.0f, 0.0f, 0)]       // bad tier clamps into range first
    public void QualityBonusPolicy_ApplyFreshnessUpgrade_SanitizesBadInput(int tier, float multiplier, float roll, int expected)
    {
        Assert.Equal(expected, QualityBonusPolicy.ApplyFreshnessUpgrade(tier, multiplier, roll));
    }

    // ================================================================
    // ConfigPolicy — BUG-019: loaded config values must be validated before use.
    // ================================================================

    [Fact]
    public void ConfigPolicy_Sanitize_ReplacesInvalidValuesWithDefaults()
    {
        var values = new ConfigValues(
            PackagingDurationSeconds: float.NaN,
            FreshnessBonusMultiplier: -5f,
            MaxBatchSize: 0,
            StationPurchasePrice: -100f,
            InteractionRange: float.PositiveInfinity,
            ConveyorScrollSpeed: float.PositiveInfinity,
            StationItemId: "   ");

        var result = ConfigPolicy.Sanitize(values);

        Assert.Equal(ConfigPolicy.DefaultPackagingDuration, result.PackagingDurationSeconds);
        Assert.Equal(0f, result.FreshnessBonusMultiplier);
        Assert.Equal(1, result.MaxBatchSize);
        Assert.Equal(0f, result.StationPurchasePrice);
        Assert.Equal(ConfigPolicy.DefaultInteractionRange, result.InteractionRange);
        Assert.Equal(ConfigPolicy.DefaultConveyorScrollSpeed, result.ConveyorScrollSpeed);
        Assert.False(string.IsNullOrWhiteSpace(result.StationItemId));
        Assert.True(result.Corrections.Count >= 6, "every corrected value must be reported");
    }

    [Fact]
    public void ConfigPolicy_Sanitize_KeepsValidValuesUntouched()
    {
        var values = new ConfigValues(
            PackagingDurationSeconds: 2.0f,
            FreshnessBonusMultiplier: 0.05f,
            MaxBatchSize: 10,
            StationPurchasePrice: 3500f,
            InteractionRange: 3.5f,
            ConveyorScrollSpeed: 0.5f,
            StationItemId: "autopackagingstation");

        var result = ConfigPolicy.Sanitize(values);

        Assert.Equal(2.0f, result.PackagingDurationSeconds);
        Assert.Equal(0.05f, result.FreshnessBonusMultiplier);
        Assert.Equal(10, result.MaxBatchSize);
        Assert.Equal(3500f, result.StationPurchasePrice);
        Assert.Equal(3.5f, result.InteractionRange);
        Assert.Equal(0.5f, result.ConveyorScrollSpeed);
        Assert.Equal("autopackagingstation", result.StationItemId);
        Assert.Empty(result.Corrections);
    }

    [Fact]
    public void ConfigPolicy_Sanitize_ClampsRangesInsteadOfRejecting()
    {
        var values = new ConfigValues(
            PackagingDurationSeconds: 0.01f,   // below minimum -> clamped up
            FreshnessBonusMultiplier: 1.5f,    // above 1 -> clamped to 1
            MaxBatchSize: 500,                 // above 20 -> clamped to 20
            StationPurchasePrice: 1e9f,
            InteractionRange: 0.1f,            // below 1 -> clamped up
            ConveyorScrollSpeed: 1000f,
            StationItemId: "autopackagingstation");

        var result = ConfigPolicy.Sanitize(values);

        Assert.Equal(0.2f, result.PackagingDurationSeconds);
        Assert.Equal(1.0f, result.FreshnessBonusMultiplier);
        Assert.Equal(20, result.MaxBatchSize);
        Assert.Equal(1f, result.InteractionRange);
        Assert.Equal(10f, result.ConveyorScrollSpeed);
    }
}

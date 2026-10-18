using Xunit;

namespace TaxiDriver.Tests;

public class RoadSnapSafetyPolicyTests
{
    [Theory]
    [InlineData(5f, 0f, false, false)]
    [InlineData(5.01f, 0f, false, true)]
    [InlineData(3f, 10f, true, false)]
    [InlineData(3.01f, 2.01f, false, true)]
    [InlineData(4f, 0f, true, true)]
    [InlineData(float.NaN, 0f, true, false)]
    public void HardMarginAndDitchRulesRemainXzBased(float lateral, float drop, bool steep, bool expected)
    {
        Assert.Equal(expected, RoadSnapSafetyPolicy.NeedsHardCorrection(lateral, drop, steep));
    }

    [Theory]
    [InlineData(false, false, false, false, false, 0f, true)]
    [InlineData(true, false, false, false, false, 0f, false)]
    [InlineData(false, true, false, false, false, 0f, false)]
    [InlineData(false, false, true, false, false, 0f, false)]
    [InlineData(false, false, false, true, false, 0f, false)]
    [InlineData(false, false, false, false, true, 0f, false)]
    [InlineData(false, false, false, false, false, 40f, true)]
    [InlineData(false, false, false, false, false, 40.1f, false)]
    [InlineData(false, false, false, false, false, float.NaN, false)]
    public void UnsafeSupportIsRejected(bool trigger, bool dynamic, bool character, bool vehicle,
        bool self, float slope, bool expected)
    {
        Assert.Equal(expected, RoadSnapSafetyPolicy.IsSafeSupport(trigger, dynamic, character, vehicle, self, slope));
    }

    [Theory]
    [InlineData(10f, 9.25f, 20f, 19.4f, 9.9f)]
    [InlineData(10f, 10.75f, -2f, -2.6f, 11.4f)]
    [InlineData(10f, 10f, 2f, 2.5f, 9.55f)]
    public void GroundPlacementPreservesMeasuredRootToBottomClearance(float projectedY, float groundY,
        float rootY, float bottomY, float expected)
    {
        Assert.True(RoadSnapSafetyPolicy.TryRootHeight(projectedY, groundY, rootY, bottomY,
            supported: true, free: true, out float result));
        Assert.Equal(expected, result, 4);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void MissingGroundOrBlockedCandidateIsRejected(bool supported, bool free)
    {
        Assert.False(RoadSnapSafetyPolicy.TryRootHeight(10f, 10f, 20f, 19f, supported, free, out _));
    }

    [Theory]
    [InlineData(8f)]
    [InlineData(12f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void UnrelatedFloorsRoofsOrInvalidGroundAreRejected(float groundY)
    {
        Assert.False(RoadSnapSafetyPolicy.TryRootHeight(10f, groundY, 20f, 19f, true, true, out _));
    }
}

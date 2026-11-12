using System.Numerics;
using Xunit;

namespace TaxiDriver.Tests;

public class DetourClearancePolicyTests
{
    private static readonly Vector3 Min = new(-1, -1, -1);
    private static readonly Vector3 Max = new(1, 1, 1);

    [Theory]
    [InlineData(-25f, 25f)]
    [InlineData(25f, -25f)]
    [InlineData(-float.MaxValue, float.MaxValue)]
    public void SparsePathSegmentCrossesBlockerEvenWithBothEndpointsOutside(float fromX, float toX)
    {
        Assert.True(DetourClearancePolicy.CrossesBlocker(new(fromX, 0, 0), new(toX, 0, 0), Min, Max));
    }

    [Theory]
    [InlineData(1f, true)]
    [InlineData(1.01f, false)]
    [InlineData(6f, false)]
    public void BoundaryCountsAsBlockedAndOverpassClears(float height, bool blocked)
    {
        Assert.Equal(blocked, DetourClearancePolicy.CrossesBlocker(new(-5, height, 0), new(5, height, 0), Min, Max));
    }

    [Fact]
    public void CrossingDifferentSlabsAtDifferentTimesDoesNotBlock()
    {
        Assert.False(DetourClearancePolicy.CrossesBlocker(new(-4, 0, 0), new(0, 0, 4), Min, Max));
    }

    [Fact]
    public void SegmentEndingOnCornerCountsAsBlocked()
    {
        Assert.True(DetourClearancePolicy.CrossesBlocker(new(2, 2, 2), Max, Min, Max));
    }

    [Fact]
    public void ZeroLengthSegmentOnlyBlocksInsideOrOnTheBox()
    {
        Assert.True(DetourClearancePolicy.CrossesBlocker(Vector3.Zero, Vector3.Zero, Min, Max));
        Assert.False(DetourClearancePolicy.CrossesBlocker(new(2, 0, 0), new(2, 0, 0), Min, Max));
    }

    [Fact]
    public void UnreadablePositionsOrInvertedBoundsAreBlocked()
    {
        Assert.True(DetourClearancePolicy.CrossesBlocker(new(float.NaN, 0, 0), Vector3.Zero, Min, Max));
        Assert.True(DetourClearancePolicy.CrossesBlocker(Vector3.Zero, new(0, 0, float.PositiveInfinity), Min, Max));
        Assert.True(DetourClearancePolicy.CrossesBlocker(Vector3.Zero, Vector3.Zero, Min, new(1, float.NegativeInfinity, 1)));
        Assert.True(DetourClearancePolicy.CrossesBlocker(Vector3.Zero, Vector3.Zero, new(0, 2, 0), Max));
    }
}

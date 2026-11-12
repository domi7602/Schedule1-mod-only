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

    [Theory]
    [InlineData(-4f, false)]
    [InlineData(1f, false)]
    [InlineData(4f, true)]
    public void FreePoseBeforeBlockerIsRejectedButRemainingRouteBeyondItIsClear(float candidateX, bool clear)
    {
        Vector3[] route = { new(-10, 0, 0), new(10, 0, 0), new(10, 0, 10) };
        Assert.Equal(clear, DetourClearancePolicy.RemainingCorridorClears(route,
            new(candidateX, .6f, 0), new(100, -1, 100), new(102, 1, 102), Min, Max));
    }

    [Fact]
    public void LaterOriginalBlockerStillRejectsAPoseBeyondTheNewBlocker()
    {
        Vector3[] route = { new(-10, 0, 0), new(10, 0, 0), new(10, 0, 10) };
        Assert.False(DetourClearancePolicy.RemainingCorridorClears(route, new(4, .6f, 0),
            new(9, -1, 4), new(11, 1, 6), Min, Max));
    }

    [Fact]
    public void RemainingCorridorFollowsBendsRatherThanAWorldAxis()
    {
        Vector3[] route = { new(-4, 0, 0), new(4, 0, 0), new(4, 0, 10), new(-4, 0, 10) };
        Assert.True(DetourClearancePolicy.RemainingCorridorClears(route, new(4, .6f, 5),
            new(100, -1, 100), new(102, 1, 102), Min, Max));
    }

    [Fact]
    public void ConnectionFromSupportedRootToRouteMustAlsoClearBothBounds()
    {
        Vector3[] route = { new(0, 0, 0), new(10, 0, 0) };
        Assert.False(DetourClearancePolicy.RemainingCorridorClears(route, new(4, 3, 0),
            new(3, 1, -1), new(5, 2, 1), new(100, -1, 100), new(102, 1, 102)));
    }

    [Fact]
    public void DistantCandidateCannotBorrowAnotherRoadsClearSuffix()
    {
        Vector3[] route = { new(-10, 0, 0), new(10, 0, 0) };
        Assert.False(DetourClearancePolicy.RemainingCorridorClears(route, new(4, .6f, 1),
            new(100, -1, 100), new(102, 1, 102), Min, Max));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(4097)]
    public void MissingOrOversizedCorridorIsRejected(int count)
    {
        Assert.False(DetourClearancePolicy.RemainingCorridorClears(new Vector3[count], new(4, .6f, 0),
            new(100, -1, 100), new(102, 1, 102), Min, Max));
    }

    [Fact]
    public void UnreadablePrefixAndInvalidBoundsCannotPassThroughProjection()
    {
        Vector3[] route = { new(float.NaN, 0, 0), new(-10, 0, 0), new(10, 0, 0) };
        Assert.False(DetourClearancePolicy.RemainingCorridorClears(route, new(4, .6f, 0),
            new(100, -1, 100), new(102, 1, 102), Min, Max));
        route[0] = new(-20, 0, 0);
        Assert.False(DetourClearancePolicy.RemainingCorridorClears(route, new(4, .6f, 0), Max, Min, Min, Max));
    }
}

using System.Numerics;
using Xunit;

namespace TaxiDriver.Tests;

public class RoadProjectionPolicyTests
{
    [Theory]
    [InlineData(0f, 0f)]
    [InlineData(float.NaN, 10f)]
    [InlineData(0f, float.PositiveInfinity)]
    [InlineData(0f, float.MaxValue)]
    public void DegenerateOrInvalidSegmentsAreRejected(float aX, float bX)
    {
        Assert.False(RoadProjectionPolicy.TryProject(new(aX, 0, 0), new(bX, 5, 0),
            Vector3.One, out _, out _, out _));
    }

    [Fact]
    public void InvalidVehiclePositionIsRejected()
    {
        Assert.False(RoadProjectionPolicy.TryProject(Vector3.Zero, Vector3.UnitX,
            new(0, float.NaN, 0), out _, out _, out _));
    }

    [Theory]
    [InlineData(2f, 12f, 5f, 7f)]
    [InlineData(12f, 2f, 5f, 7f)]
    [InlineData(2f, 12f, -5f, 2f)]
    [InlineData(2f, 12f, 15f, 12f)]
    [InlineData(12f, 2f, -5f, 12f)]
    [InlineData(12f, 2f, 15f, 2f)]
    public void XzProjectionInterpolatesOriginalSegmentHeight(float startY, float endY, float x, float expectedY)
    {
        Assert.True(RoadProjectionPolicy.TryProject(new(0, startY, 0), new(10, endY, 0),
            new(x, -100, 6), out var point, out var direction, out var lateral));
        Assert.Equal(expectedY, point.Y, 5);
        Assert.Equal(0, direction.Y);
        Assert.Equal(Vector3.UnitX, direction);
        Assert.True(lateral >= 6f);
    }
}

using System;
using System.Numerics;
using Xunit;

namespace TaxiDriver.Tests;

public class RoadPlacementBoundsTests
{
    [Theory]
    [InlineData(0f)]
    [InlineData(0.8f)]
    [InlineData(1.57f)]
    public void MeasuredCornersIncludeFullFootprintAtProposedHeading(float yaw)
    {
        var corners = Corners();
        Assert.True(RoadSnapSafetyPolicy.TryPlacementBounds(corners, Quaternion.Identity,
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, yaw), out var min, out var max));
        float expectedX = Math.Abs(MathF.Cos(yaw)) + 2 * Math.Abs(MathF.Sin(yaw));
        Assert.Equal(-expectedX, min.X, 4);
        Assert.Equal(expectedX, max.X, 4);
        Assert.Equal(-0.6f, min.Y, 4);
        Assert.Equal(0.6f, max.Y, 4);
    }

    [Fact]
    public void CurrentRollDoesNotInflateUprightBottomClearance()
    {
        var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 0.7f);
        var corners = Corners();
        for (int i = 0; i < corners.Length; i++)
            corners[i] = Vector3.Transform(corners[i], rotation);
        Assert.True(RoadSnapSafetyPolicy.TryPlacementBounds(corners, rotation, Quaternion.Identity, out var min, out _));
        Assert.Equal(-0.6f, min.Y, 4);
    }

    [Fact]
    public void MissingInvalidOrDegenerateBoundsAreRejected()
    {
        Assert.False(RoadSnapSafetyPolicy.TryPlacementBounds(Array.Empty<Vector3>(), Quaternion.Identity,
            Quaternion.Identity, out _, out _));
        var corners = Corners();
        corners[0] = new(float.NaN, 0, 0);
        Assert.False(RoadSnapSafetyPolicy.TryPlacementBounds(corners, Quaternion.Identity,
            Quaternion.Identity, out _, out _));
        Assert.False(RoadSnapSafetyPolicy.TryPlacementBounds(new Vector3[8], Quaternion.Identity,
            Quaternion.Identity, out _, out _));
        Assert.False(RoadSnapSafetyPolicy.TryPlacementBounds(Corners(), default,
            Quaternion.Identity, out _, out _));
    }

    private static Vector3[] Corners()
    {
        var corners = new Vector3[8];
        for (int i = 0; i < 8; i++)
            corners[i] = new((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -0.6f : 0.6f, (i & 4) == 0 ? -2 : 2);
        return corners;
    }
}

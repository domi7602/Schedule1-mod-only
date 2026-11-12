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
    public void RootAlignedBoundsKeepTheMeasuredCenterAndExcludeEmptyDiagonalCorners()
    {
        var center = new Vector3(0.45f, 0.1f, -0.2f);
        var current = Quaternion.CreateFromYawPitchRoll(0.3f, -0.2f, 0.4f);
        var proposed = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 4f);
        var corners = Corners();
        for (int i = 0; i < corners.Length; i++)
            corners[i] = Vector3.Transform(corners[i] + center, current);

        Assert.True(RoadSnapSafetyPolicy.TryPlacementBounds(corners, current,
            Quaternion.Identity, out var rootMin, out var rootMax));
        var half = (rootMax - rootMin) * 0.5f;
        Assert.True(Vector3.Distance(center, rootMin + half) < 0.0001f);
        Assert.True(Vector3.Distance(new Vector3(1f, 0.6f, 2f), half) < 0.0001f);

        Assert.True(RoadSnapSafetyPolicy.TryPlacementBounds(corners, current, proposed,
            out _, out var worldMax));
        var emptyWorldCorner = new Vector3(worldMax.X - 0.05f, center.Y, worldMax.Z - 0.05f);
        var boxSpace = Vector3.Transform(emptyWorldCorner, Quaternion.Inverse(proposed)) - center;
        Assert.True(Math.Abs(boxSpace.Z) > half.Z);
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

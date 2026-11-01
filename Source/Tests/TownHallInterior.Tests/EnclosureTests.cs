using TownHallInterior;
using Xunit;

// Geometric rays through the actual production plans; no Unity/URP simulation.
public sealed class EnclosureTests
{
    [Theory]
    [InlineData(54.95, 4.515, 31)]
    [InlineData(54.75, 4.86, 31)]
    [InlineData(54.75, 4.6, 30.02)]
    [InlineData(54.75, 4.6, 31.98)]
    public void ReproducedHeaderSightlinesMeetOwnedGeometry(double x, double y, double z)
    {
        // Same previously-red rays as the independently executed native-fixture forensic repro.
        Assert.Contains(Visible(), b => Hit(b, new[] { 52.8, 3.1, 31 }, new[] { x - 52.8, y - 3.1, z - 31 }) is <= 2);
    }

    [Fact]
    public void NewSkinKeepsPortalAndStairVoidVisuallyClear()
    {
        var skin = FurniturePlan.CreateShell();
        Assert.All(skin, b => Assert.False(b.Solid));
        Assert.Equal(skin.Count, skin.Select(b => b.Name).Distinct().Count());
        // The threshold is the finished walking surface, so the free volume starts above it.
        foreach (var b in Visible())
            Assert.False(Overlap(b, new[] { 54.18, PublicEntrance.FloorTop + .001, 30.04 }, new[] { 54.95, PublicEntrance.HeadLiningBottom, 31.96 }), "portal: " + b.Name);
        foreach (var b in skin)
            Assert.False(Overlap(b, new[] { 60.7, 4.4, 24 }, new[] { 62.1, 7.25, 29.6 }), "stair void: " + b.Name);
    }

    [Fact]
    public void UpperPerimeterClosesHorizontalViewsAtMultipleHeights()
    {
        var visible = Visible().ToArray();
        foreach (double y in new[] { 4.85, 6.3, 7.95 })
            for (int degree = 0; degree < 360; degree++)
            {
                double angle = degree * Math.PI / 180;
                Assert.Contains(visible, b => Hit(b, new[] { 58.7, y, 31 }, new[] { Math.Cos(angle), 0, Math.Sin(angle) }) is <= 30);
            }
    }

    [Fact]
    public void JambFacesAreNotCoplanarWithExposedWallOrUpperFloorFaces()
    {
        var face = PublicEntrance.CreateInterior().Single(b => b.Name == "inner-west-south");
        foreach (var reveal in FurniturePlan.CreateShell().Where(b => b.Name.StartsWith("reveal-")))
        {
            Assert.InRange(reveal.X + reveal.Width / 2 - (face.X + face.Width / 2), .0005f, .002f);
            Assert.InRange(RoomPlan.Upper - (reveal.Y + reveal.Height / 2), .0005f, .002f);
        }
        // Coplanar Z-overlap between reveal and floor-rim must not occur (Z-fighting guard).
        var shell = FurniturePlan.CreateShell();
        foreach (var reveal in shell.Where(b => b.Name.StartsWith("reveal-")))
        {
            var south = reveal.Name.EndsWith("south");
            var rimName = south ? "floor-rim-west-south" : "floor-rim-west-north";
            var rim = shell.Single(b => b.Name == rimName);
            // Their Z-ranges must not share a boundary (at least 1 cm gap).
            float revealZMax = reveal.Z + reveal.Depth / 2;
            float rimZMax = rim.Z + rim.Depth / 2;
            float revealZMin = reveal.Z - reveal.Depth / 2;
            float rimZMin = rim.Z - rim.Depth / 2;
            Assert.True(Math.Abs(revealZMax - rimZMax) > .01f || Math.Abs(revealZMin - rimZMin) > .01f,
                $"Coplanar Z-overlap: {reveal.Name} [{revealZMin:F3}..{revealZMax:F3}] vs {rim.Name} [{rimZMin:F3}..{rimZMax:F3}]");
        }
    }

    [Fact]
    public void EnclosureStylesUseOwnedPaintsWithConsumerGatedCleanup()
    {
        // Static runtime wiring check; native Material failures/unload still need owner evidence.
        var repo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var source = File.ReadAllText(Path.Combine(repo, "Source/Mods/TownHallInterior/src/VanillaFurnishings.cs"));
        Assert.Contains("renderer.sharedMaterial = _paints[box.Style]", source);
        Assert.Contains("foreach (var paint in _paints)", source);
        Assert.Contains("UnityEngine.Object.Destroy(paint); }, Ready)", source);
        Assert.Contains("_paints.Add(paint)", source);
        Assert.Contains("paint.color = color", source);
        Assert.True(source.IndexOf("_paints.Add(paint)", StringComparison.Ordinal) < source.IndexOf("paint.color = color", StringComparison.Ordinal));
        Assert.All(FurniturePlan.CreateShell(), b => Assert.InRange(b.Style, 0, 2));
    }

    [Fact]
    public void LightingPlanPreservesReceptionLightsAndIlluminatesTheClosedUpperFloor()
    {
        var method = typeof(FurniturePlan).GetMethod("CreateLights");
        Assert.NotNull(method);
        var rows = System.Text.Json.JsonSerializer.SerializeToElement(method.Invoke(null, null)).EnumerateArray().ToArray();
        Assert.Equal(8, rows.Length);
        var ground = rows.Where(r => Math.Abs(r.GetProperty("Y").GetDouble() - 4.15) < 1e-3).ToArray();
        var upper = rows.Where(r => Math.Abs(r.GetProperty("Y").GetDouble() - 7.8) < 1e-3).ToArray();
        Assert.Equal(4, ground.Length); Assert.Equal(4, upper.Length);
        Assert.Equal(new[] { 24d, 28.5, 33.5, 38 }, ground.Select(r => r.GetProperty("Z").GetDouble()));
        foreach (var row in ground.Concat(upper))
        {
            double y = row.GetProperty("Y").GetDouble();
            Assert.Equal(y < RoomPlan.Upper ? 4.15 : 7.8, y, 4);
            Assert.Equal(58.7, row.GetProperty("X").GetDouble(), 4);
            Assert.Equal(5, row.GetProperty("Range").GetDouble());
            Assert.Equal(1.1, row.GetProperty("Intensity").GetDouble(), 4);
        }
        var repo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var runtime = File.ReadAllText(Path.Combine(repo, "Source/Mods/TownHallInterior/src/VanillaFurnishings.cs"));
        Assert.Contains("FurniturePlan.CreateLights()", runtime); // Wiring guard, not native lighting acceptance.
    }

    [Theory]
    [InlineData(54.975)]
    [InlineData(62.425)]
    public void GroundFloorReachesTheInsetSideWalls(double x)
    {
        var hit = Visible().Select(b => Hit(b, new[] { x, 2.0, 31 }, new[] { 0d, -1, 0 })).Where(t => t.HasValue).Min();
        Assert.NotNull(hit);
        Assert.Equal(PublicEntrance.FloorTop, 2.0 - hit.Value, 4);
    }

    [Theory]
    [InlineData("west-lintel", -1, 0)]
    [InlineData("east", 1, 0)]
    [InlineData("south", 0, -1)]
    [InlineData("north", 0, 1)]
    public void UpperWallSkinsMeetExistingPerimeterCollisionRatherThanHidingItFarBehindThem(string wallName, double dx, double dz)
    {
        double[] origin = { 58.7, 6.3, 31 }, direction = { dx, 0, dz };
        var support = Hit(RoomPlan.Create().Single(b => b.Name == wallName), origin, direction)!.Value;
        var visible = Visible().Select(b => Hit(b, origin, direction)).Where(t => t.HasValue).Min();
        Assert.NotNull(visible);
        Assert.InRange(visible.Value, support - .025, support + .001);
    }

    [Theory]
    [InlineData(61.4, 4, 27)]
    [InlineData(58.7, 6.3, 31)]
    public void UpperCeilingClosesTheSkyAboveRoomAndStairwell(double x, double y, double z)
    {
        var visible = Visible().Select(b => Hit(b, new[] { x, y, z }, new[] { 0d, 1, 0 })).Where(t => t.HasValue).Min();
        Assert.NotNull(visible);
        Assert.Equal(8.05, y + visible.Value, 4); // Chosen design underside, not a native measurement.
    }

    [Fact]
    public void VisibleUpperFloorCoversTheFullExistingSlabsIncludingTheirOuterRims()
    {
        // Test floor coverage even beneath the perimeter skins, not a camera embedded in a wall.
        var visible = Visible().Where(b => b.Y + b.Height / 2 <= RoomPlan.Upper + .0001f).ToArray();
        foreach (var slab in RoomPlan.Create().Where(b => b.Name.StartsWith("upper-")))
            for (double x = slab.X - slab.Width / 2 + .05; x < slab.X + slab.Width / 2; x += .2)
                for (double z = slab.Z - slab.Depth / 2 + .05; z < slab.Z + slab.Depth / 2; z += .2)
                {
                    var hit = visible.Select(b => Hit(b, new[] { x, 5.5, z }, new[] { 0d, -1, 0 })).Where(t => t.HasValue).Min();
                    Assert.True(hit.HasValue, $"Unrendered support at x={x}, z={z}");
                    Assert.Equal(RoomPlan.Upper, 5.5 - hit.Value, 4);
                }
    }

    [Theory]
    [InlineData(3, 30.02)]
    [InlineData(3, 31.98)]
    [InlineData(4.3, 30.02)]
    [InlineData(4.3, 31.98)]
    public void DoorJambReturnsCloseTheGapNextToTheInsetWalls(double y, double z)
    {
        Assert.Contains(Visible(), b => Hit(b, new[] { 55.5, y, z }, new[] { -1d, 0, 0 }) is <= 1.2);
    }

    [Fact]
    public void HeaderJoinsFrameToCeilingWithoutProjectingAboveTheUpperFloor()
    {
        var header = PublicEntrance.CreateInterior().Single(b => b.Name == "inner-lintel");
        Assert.Equal(PublicEntrance.LiningFrontX, header.X - header.Width / 2, 4);
        Assert.Equal(54.95, header.X + header.Width / 2, 4);
        Assert.Equal(PublicEntrance.HeadLiningBottom, header.Y - header.Height / 2, 4);
        Assert.Equal(RoomPlan.Upper, header.Y + header.Height / 2, 4);
        Assert.Equal(30.04, header.Z - header.Depth / 2, 4);
        Assert.Equal(31.96, header.Z + header.Depth / 2, 4);
    }

    [Fact]
    public void OwnedRevealLiningsSpanTheFullWallDepthToTheFacadePlane()
    {
        // A native facade cut follows triangle edges, so the rendered hole is not the ideal
        // rectangle. Linings that stopped at the old 54.34 frame depth left an upward sight
        // line between their outer edge and the leaf plane; they must overlap the leaf plane.
        var linings = Visible().Where(b => b.Name.StartsWith("reveal-") || b.Name == "inner-lintel").ToArray();
        Assert.Equal(3, linings.Length);
        Assert.All(linings, b => Assert.Equal(PublicEntrance.LiningFrontX, b.X - b.Width / 2, 4));
    }

    [Theory]
    [InlineData(30.30)]
    [InlineData(31.00)]
    [InlineData(31.70)]
    public void UpwardSightOverTheLeavesMeetsTheOwnedHeaderBeforeTheFacadePlane(double z)
    {
        // Owner screenshot 2026-10-29 18:20: a sky band above the leaves. This ray passed
        // under the former 0.6 m header and cleared the leaf top (4.55) before the wall.
        var origin = new[] { 55.5, 3.5, z };
        var direction = new[] { -1d, 0.84, 0 };
        Assert.Contains(Visible(), b => Hit(b, origin, direction) is <= 1.3);
    }

    [Fact]
    public void EntranceThresholdFinishesTheRecessBetweenTheRevealLinings()
    {
        var threshold = PublicEntrance.CreateInterior().Single(b => b.Name == "public-threshold");
        var shell = FurniturePlan.CreateShell();
        var south = shell.Single(b => b.Name == "reveal-south");
        var north = shell.Single(b => b.Name == "reveal-north");
        Assert.True(threshold.Solid);
        Assert.Equal(0, threshold.Style); // Same paint as the hall floor, not a separate sill.
        Assert.Equal(PublicEntrance.FloorTop, threshold.Y + threshold.Height / 2, 4);
        Assert.True(threshold.Y - threshold.Height / 2 <= south.Y - south.Height / 2,
            "Threshold underside must stay buried below the reveal linings");
        Assert.Equal(south.Z + south.Depth / 2, threshold.Z - threshold.Depth / 2, 4);
        Assert.Equal(north.Z - north.Depth / 2, threshold.Z + threshold.Depth / 2, 4);
        Assert.True(threshold.X - threshold.Width / 2 >= 54.16f, "Threshold must stay clear of the sliding leaves");
        Assert.True(threshold.X - threshold.Width / 2 <= 54.35f, "Threshold must reach the facade plane");
        Assert.True(threshold.X + threshold.Width / 2 >= 54.95f, "Threshold must meet the floor edge");
    }

    [Theory]
    [InlineData(30.1)]
    [InlineData(30.5)]
    [InlineData(31.0)]
    [InlineData(31.5)]
    [InlineData(31.9)]
    public void OwnedThresholdClosesTheRecessFloorAcrossTheOpening(double z)
    {
        // Reproduces the owner screenshot column: this used to reach the native terrain at ~1.40.
        var hit = Visible().Select(b => Hit(b, new[] { 54.65, 1.65, z }, new[] { 0d, -1, 0 })).Where(t => t.HasValue).Min();
        Assert.NotNull(hit);
        Assert.Equal(PublicEntrance.FloorTop, 1.65 - hit.Value, 4);
    }

    [Fact]
    public void VisibleUpperFloorMeetsTheExistingSupportInsteadOfFloatingFeet()
    {
        var slab = RoomPlan.Create().Single(b => b.Name == "upper-west");
        var top = slab.Y + slab.Height / 2;
        var hit = Visible().Select(b => Hit(b, new[] { 58.7, 5.5, 31 }, new[] { 0d, -1, 0 })).Where(t => t.HasValue).Min();
        Assert.NotNull(hit);
        Assert.Equal(top, 5.5 - hit.Value, 4);
        Assert.All(FurniturePlan.CreateShell().Where(b => b.Name.StartsWith("ceiling-")), b =>
        {
            Assert.Equal(4.4, b.Y - b.Height / 2, 4);
            Assert.False(b.Solid);
        });
    }

    [Theory]
    [InlineData(55.035, 3, 23, -.04, -1)]
    [InlineData(62.365, 3, 23, .04, -1)]
    [InlineData(55.035, 3, 39, -.04, 1)]
    [InlineData(62.365, 3, 39, .04, 1)]
    [InlineData(55.035, 4.3, 23, -.04, -1)]
    [InlineData(62.365, 4.3, 23, .04, -1)]
    [InlineData(55.035, 4.3, 39, -.04, 1)]
    [InlineData(62.365, 4.3, 39, .04, 1)]
    public void CornerAndCrownRaysCannotSeeThroughTheOwnedShell(double x, double y, double z, double dx, double dz)
    {
        Assert.Contains(Visible(), b => Hit(b, new[] { x, y, z }, new[] { dx, 0, dz }) is <= 3);
    }

    private static IEnumerable<RoomBox> Visible() => PublicEntrance.CreateInterior().Concat(FurniturePlan.CreateShell());

    private static bool Overlap(RoomBox b, double[] lo, double[] hi)
    {
        double[] center = { b.X, b.Y, b.Z }, size = { b.Width, b.Height, b.Depth };
        return Enumerable.Range(0, 3).All(i => Math.Min(center[i] + size[i] / 2, hi[i]) - Math.Max(center[i] - size[i] / 2, lo[i]) > 1e-5);
    }

    private static double? Hit(RoomBox b, double[] origin, double[] direction)
    {
        double[] center = { b.X, b.Y, b.Z }, size = { b.Width, b.Height, b.Depth };
        double enter = double.NegativeInfinity, leave = double.PositiveInfinity;
        for (int i = 0; i < 3; i++)
        {
            double lo = center[i] - size[i] / 2, hi = center[i] + size[i] / 2;
            if (Math.Abs(direction[i]) < 1e-10)
            {
                if (origin[i] < lo - 1e-6 || origin[i] > hi + 1e-6) return null;
            }
            else
            {
                double a = (lo - origin[i]) / direction[i], c = (hi - origin[i]) / direction[i];
                enter = Math.Max(enter, Math.Min(a, c)); leave = Math.Min(leave, Math.Max(a, c));
                if (leave < enter - 1e-6) return null;
            }
        }
        return leave < 1e-7 ? null : Math.Max(0, enter);
    }
}

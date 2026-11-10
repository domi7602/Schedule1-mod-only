using TownHallInterior;
using Xunit;

public sealed class RoomPlanTests
{
    [Fact]
    public void SceneEntryActuallyRequestsTheRuntimeBuilder()
    {
        var repo = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var source = System.IO.File.ReadAllText(System.IO.Path.Combine(repo, "Source/Mods/TownHallInterior/src/Mod.cs"));
        Assert.Contains("RoomBuilder.Build", source);
    }

    [Fact]
    public void RuntimeGeometryBuilderExists()
    {
        Assert.NotNull(typeof(DoorChoice).Assembly.GetType("TownHallInterior.RoomBuilder"));
    }

    [Fact]
    public void BuilderAddsPlanAndCleansPartialRootOnFailure()
    {
        var sink = new RecordingSink();
        Assert.Same(sink, RoomBuilder.Build(() => sink));
        Assert.Equal(RoomPlan.Create().Count, sink.Boxes.Count);
        var failed = new RecordingSink { FailAt = 5 };
        Assert.Throws<InvalidOperationException>(() => RoomBuilder.Build(() => failed));
        Assert.True(failed.Disposed);
    }

    [Fact]
    public void StairsHaveTwentyEqualStepsReachingUpperFloor()
    {
        var steps = RoomPlan.Create().Where(b => b.Name.StartsWith("step-")).ToArray();
        Assert.Equal(20, steps.Length);
        Assert.Equal(RoomPlan.Ground, steps[0].Y - steps[0].Height / 2, 3);
        for (int i = 0; i < steps.Length; i++)
        {
            Assert.Equal(RoomPlan.Ground + (i + 1) * RoomPlan.StairRise, steps[i].Y + steps[i].Height / 2, 3);
            Assert.Equal(RoomPlan.StairTread, steps[i].Depth, 3);
            if (i > 0) Assert.Equal(steps[i - 1].Z + steps[i - 1].Depth / 2, steps[i].Z - steps[i].Depth / 2, 3);
        }
        Assert.Equal(RoomPlan.Upper, steps[^1].Y + steps[^1].Height / 2, 3);
    }

    [Fact]
    public void FloorsProvideExpectedHeadroomAndVoid()
    {
        var boxes = RoomPlan.Create();
        Assert.Equal(3.25f, RoomPlan.Upper - RoomPlan.Slab - RoomPlan.Ground, 3);
        var slabs = boxes.Where(b => b.Name.StartsWith("upper-")).ToArray();
        Assert.Equal(4, slabs.Length);
        foreach (var slab in slabs)
        {
            float x1 = slab.X - slab.Width / 2, x2 = slab.X + slab.Width / 2;
            float z1 = slab.Z - slab.Depth / 2, z2 = slab.Z + slab.Depth / 2;
            Assert.False(x1 < 62.099f && x2 > 60.701f && z1 < 29.599f && z2 > 25.501f);
        }
    }

    [Fact]
    public void PerimeterDoesNotFillTheDoorOpening()
    {
        var walls = RoomPlan.Create().Where(b => b.Name.StartsWith("west-")).ToArray();
        Assert.Equal(3, walls.Length);
        foreach (var b in walls)
            Assert.False(b.Z - b.Depth / 2 < 31.524f && b.Z + b.Depth / 2 > 30.476f && b.Y - b.Height / 2 < 3.349f);
    }

    [Fact]
    public void HiddenPerimeterLintelMustNotBlockTheRaisedVisiblePortal()
    {
        var lintel = RoomPlan.Create().Single(b => b.Name == "west-lintel");
        Assert.True(lintel.Solid);
        Assert.True(lintel.Y - lintel.Height / 2 >= FacadePortal.Maximum[1] - 0.00001,
            $"Hidden west-lintel starts at {lintel.Y - lintel.Height / 2:F2}, below portal top {FacadePortal.Maximum[1]:F2}");
    }

    [Theory]
    [InlineData(false, 1.85, 0.35)]
    [InlineData(true, 1.85, 0.35)]
    [InlineData(false, 1.95, 0.40)]
    [InlineData(true, 1.95, 0.40)]
    public void StandingCapsuleClearsTheEntireEntranceCollisionShell(bool exiting, double height, double radius)
    {
        // Test the geometry actually used by both builders, including hidden
        // slabs/lintels and floor contact. Native approach/stepping are not simulated.
        var interior = PublicEntrance.CreateInterior();
        var floor = interior.Single(b => b.Name == "public-floor");
        double foot = floor.Y + floor.Height / 2;
        var obstacles = RoomPlan.Create().Concat(interior)
            // A box whose top is at or below the walking plane is support, not an obstruction.
            .Where(b => b.Solid && b.Y + b.Height / 2 > foot + 1e-4)
            .ToArray();
        for (int i = 0; i <= 600; i++)
        {
            double x = exiting ? 60.0 - i * 0.01 : 54.0 + i * 0.01;
            foreach (double z in new[] { 30.5, 31.0, 31.5 })
                foreach (var box in obstacles)
                    Assert.False(CapsuleIntersects(box, x, foot, z, height, radius),
                        $"{box.Name} blocks {(exiting ? "exit" : "entry")} at x={x:F2}, z={z:F2}, foot={foot:F2}, height={height:F2}");
        }
    }

    [Fact]
    public void OldHiddenLintelReproducesStandingBlockWhileAShorterEnvelopeFits()
    {
        // Independent old-coordinate fixture; the short envelope is illustrative,
        // not a measurement of the game's crouched CharacterController.
        var old = new RoomBox("old-west-lintel", 54.45f, (3.35f + 8.25f) / 2, 31f,
            0.2f, 8.25f - 3.35f, 1.92f, true, 1);
        Assert.True(CapsuleIntersects(old, 54.45, PublicEntrance.FloorTop, 31.0, 1.85, 0.35));
        Assert.False(CapsuleIntersects(old, 54.45, PublicEntrance.FloorTop, 31.0, 1.30, 0.35));
    }

    // Player prefab stress envelope and a larger hypothetical envelope. The prefab data is
    // sharedassets0.assets:Player/CharacterController (height 1.85, radius .35, skin .04,
    // identity local scale); runtime resizing/scaling is unknown. Inflate by .05 for margin.
    // These are discrete center-line poses, not a Unity movement/controller simulation.
    [Theory]
    [InlineData(1.95, 0.40)]
    [InlineData(2.50, 0.40)]
    public void CapsuleClearsOverheadSlabsAndGuardsAlongTheStairFlight(double height, double radius)
    {
        var boxes = RoomPlan.Create();
        // The north box is the top landing; its leading face must be evaluated as a
        // step-up, not as an overhead obstruction. Controller stepping remains unverified.
        var slabs = boxes.Where(b => (b.Name.StartsWith("upper-") && b.Name != "upper-north") || b.Name.StartsWith("guard-")).ToArray();
        for (int i = 0; i < 20; i++)
            for (int j = 0; j <= 28; j++)
            {
                double z = 24 + i * 0.28 + j * 0.01;
                double foot = RoomPlan.Ground + (i + 1) * RoomPlan.StairRise;
                foreach (var slab in slabs)
                    Assert.False(CapsuleIntersects(slab, 61.4, foot, z, height, radius),
                        $"{slab.Name} intersects stress capsule at step {i}, z={z:F3}");
            }
    }

    private static bool CapsuleIntersects(RoomBox b, double x, double foot, double z, double height, double radius)
    {
        static double Gap(double a, double lo, double hi) => Math.Max(0, Math.Max(lo - a, a - hi));
        double dx = Gap(x, b.X - b.Width / 2, b.X + b.Width / 2);
        double dz = Gap(z, b.Z - b.Depth / 2, b.Z + b.Depth / 2);
        double y1 = foot + radius, y2 = foot + height - radius;
        double dy = Math.Max(0, Math.Max(b.Y - b.Height / 2 - y2, y1 - (b.Y + b.Height / 2)));
        return dx * dx + dy * dy + dz * dz < radius * radius - 1e-8;
    }

    [Fact]
    public void AllExposedEdgesOfTheStairVoidHaveGuardsButTheTopLandingIsOpen()
    {
        var boxes = RoomPlan.Create();
        var guards = boxes.Where(b => b.Name.StartsWith("guard-") && b.Solid).ToArray();
        bool Covers(double x, double z) => guards.Any(b =>
            x >= b.X - b.Width / 2 - 0.001 && x <= b.X + b.Width / 2 + 0.001 &&
            z >= b.Z - b.Depth / 2 - 0.001 && z <= b.Z + b.Depth / 2 + 0.001 &&
            b.Y - b.Height / 2 <= RoomPlan.Upper + 0.001 && b.Y + b.Height / 2 >= RoomPlan.Upper + 0.95);
        for (int i = 0; i <= 56; i++)
        {
            double z = 24 + i * 0.1;
            Assert.True(Covers(60.7, z), $"west edge at {z}");
            Assert.True(Covers(62.1, z), $"east edge at {z}");
        }
        for (int i = 0; i <= 14; i++) Assert.True(Covers(60.7 + i * 0.1, 24), "south edge");
        Assert.False(Covers(61.4, 29.6), "landing must remain open");
    }

    [Fact]
    public void UpperSlabsPartitionTheInsetFloorExceptForTheStairVoid()
    {
        var slabs = RoomPlan.Create().Where(b => b.Name.StartsWith("upper-")).ToArray();
        // Cell centers sampled across the inset floor; no overlapping interiors or holes.
        for (int xi = 0; xi < 860; xi++)
            for (int zi = 0; zi < 2060; zi++)
            {
                double x = 54.55 + (xi + .5) * .01, z = 20.7 + (zi + .5) * .01;
                int count = slabs.Count(b => x > b.X - b.Width / 2 && x < b.X + b.Width / 2 && z > b.Z - b.Depth / 2 && z < b.Z + b.Depth / 2);
                bool hole = x > 60.7 && x < 62.1 && z > 24 && z < 29.6;
                Assert.True(count == (hole ? 0 : 1), $"bad coverage at {x:F3}, {z:F3}: {count}");
            }
    }

    [Fact]
    public void GeometryIsWithinShellAndEntranceAndLandingsAreClear()
    {
        var boxes = RoomPlan.Create();
        foreach (var b in boxes)
        {
            Assert.InRange(b.X - b.Width / 2, 54.349f, 63.351f);
            Assert.InRange(b.X + b.Width / 2, 54.349f, 63.351f);
            Assert.InRange(b.Z - b.Depth / 2, 20.499f, 41.501f);
            Assert.InRange(b.Z + b.Depth / 2, 20.499f, 41.501f);
            Assert.InRange(b.Y - b.Height / 2, RoomPlan.Ground - .001f, 8.251f);
            Assert.InRange(b.Y + b.Height / 2, RoomPlan.Ground - .001f, 8.251f);
        }
        // Straight entrance corridor, then a lower and upper landing cylinder. Floor,
        // steps and slab supports are not obstructions; their support is tested separately.
        var obstacles = boxes.Where(b => b.Solid && !b.Name.StartsWith("upper-") && !b.Name.StartsWith("step-")).ToArray();
        foreach (var b in obstacles)
        {
            for (double x = 54.0; x <= 60.0; x += .05)
                Assert.False(CapsuleIntersects(b, x, RoomPlan.Ground, 31.0, 1.85, .35), $"entrance: {b.Name}");
            for (double z = 22.5; z <= 23.6; z += .05)
                Assert.False(CapsuleIntersects(b, 61.4, RoomPlan.Ground, z, 1.85, .35), $"lower approach: {b.Name}");
            for (double z = 30.0; z <= 31.5; z += .05)
                Assert.False(CapsuleIntersects(b, 61.4, RoomPlan.Upper, z, 1.85, .35), $"upper landing: {b.Name}");
        }
    }

    [Fact]
    public void StairsMeetLandingWithoutSolidSlabOverlap()
    {
        var boxes = RoomPlan.Create();
        var slabs = boxes.Where(b => b.Name.StartsWith("upper-")).ToArray();
        var steps = boxes.Where(b => b.Name.StartsWith("step-")).ToArray();
        static bool Overlap(double lo1, double hi1, double lo2, double hi2) => Math.Min(hi1, hi2) - Math.Max(lo1, lo2) > .0001;
        foreach (var step in steps)
            foreach (var slab in slabs)
            {
                bool overlap = Overlap(step.X - step.Width / 2, step.X + step.Width / 2, slab.X - slab.Width / 2, slab.X + slab.Width / 2)
                    && Overlap(step.Y - step.Height / 2, step.Y + step.Height / 2, slab.Y - slab.Height / 2, slab.Y + slab.Height / 2)
                    && Overlap(step.Z - step.Depth / 2, step.Z + step.Depth / 2, slab.Z - slab.Depth / 2, slab.Z + slab.Depth / 2);
                Assert.False(overlap, $"{step.Name} overlaps {slab.Name}");
            }
        var landing = slabs.Single(b => b.Name == "upper-north");
        Assert.Equal(steps[^1].Z + steps[^1].Depth / 2, landing.Z - landing.Depth / 2, 3);
        Assert.Equal(steps[^1].Y + steps[^1].Height / 2, landing.Y + landing.Height / 2, 3);
        Assert.InRange(landing.Depth, 1.4f, float.MaxValue);
    }

    private sealed class RecordingSink : IRoomSink
    {
        public readonly List<RoomBox> Boxes = new();
        public int FailAt = int.MaxValue;
        public bool Disposed;
        public void Add(RoomBox box) { if (Boxes.Count == FailAt) throw new InvalidOperationException(); Boxes.Add(box); }
        public void Dispose() => Disposed = true;
    }

    [Fact]
    public void BothFloorsHaveTilesAndTheStairVoidDoesNot()
    {
        var ground = HallGridPlan.Tiles(new[] { HallGridPlan.Ground }, null, HallGridPlan.TileSize);
        var upper = HallGridPlan.Tiles(HallGridPlan.Upper, HallGridPlan.StairVoid, HallGridPlan.TileSize);
        Assert.InRange(ground.Count, 400, 700);
        Assert.InRange(upper.Count, 400, 800);
        Assert.Contains(ground, t => Math.Abs(t.WorldX - 58.5f) < 0.01f && Math.Abs(t.WorldZ - 31f) < 0.01f);
        Assert.Contains(upper, t => Math.Abs(t.WorldX - 60f) < 0.01f && Math.Abs(t.WorldZ - 22f) < 0.01f);
        Assert.DoesNotContain(upper, t => HallGridPlan.Overlaps(t.WorldX, t.WorldZ, HallGridPlan.TileSize, HallGridPlan.StairVoid));
    }

    [Fact]
    public void RoomHasAConcreteGeometryPlan()
    {
        Assert.NotNull(typeof(DoorChoice).Assembly.GetType("TownHallInterior.RoomPlan"));
    }
}

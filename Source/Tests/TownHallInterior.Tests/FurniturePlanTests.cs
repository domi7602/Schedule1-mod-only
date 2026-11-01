using TownHallInterior;
using Xunit;

public sealed class FurniturePlanTests
{
    [Fact]
    public void CeilingMeetsInnerWallFacesInsteadOfLeavingFloorSizedEdgeGaps()
    {
        var skin = PublicEntrance.CreateInterior();
        var west = skin.Single(b => b.Name == "inner-west-south");
        var east = skin.Single(b => b.Name == "inner-east");
        var ceilings = FurniturePlan.CreateShell().Where(b => b.Name.StartsWith("ceiling-")).ToArray();
        Assert.Equal(west.X + west.Width / 2, ceilings.Min(b => b.X - b.Width / 2), 4);
        Assert.Equal(east.X - east.Width / 2, ceilings.Max(b => b.X + b.Width / 2), 4);
    }

    [Fact]
    public void ExactClockHandsMayChangePoseButFrameAndOtherStaticDonorsStayBounded()
    {
        var check = typeof(FurniturePlan).GetMethod("RequiresStableBounds");
        Assert.NotNull(check);
        var doc = Native();
        foreach (var a in doc.Assets)
            foreach (var p in a.Parts)
            {
                var hand = a.Key == "clock" && (p.SourcePath.EndsWith("/Min/MinuteHand") || p.SourcePath.EndsWith("/Hour/HourHand"));
                if (hand) Assert.True(p.IsStaticBatch); // The pinned hands ARE statically batched.
                var expected = a.Key == "door-frame" || (p.IsStaticBatch && !hand);
                Assert.Equal(expected, Assert.IsType<bool>(check.Invoke(null, new object[] { a.Key, p })));
            }
    }

    [Fact]
    public void PrimitiveFurnitureIsReplacedRatherThanOverlaid()
    {
        Assert.DoesNotContain(RoomPlan.Create(), b => b.Name is "reception-counter" or "office-desk" or "meeting-table");
    }

    [Fact]
    public void RuntimeWiringKeepsOptionalFurnitureFailureOutOfTheAcceptedEntrancePath()
    {
        // Source wiring guard only; native Unity execution remains an owner check.
        var repo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var host = File.ReadAllText(Path.Combine(repo, "Source/Mods/TownHallInterior/src/PublicHallHost.cs"));
        Assert.Contains("host.Furnish();", host);
        Assert.True(host.IndexOf("room.Publish();", StringComparison.Ordinal) < host.IndexOf("host.Furnish();", StringComparison.Ordinal));
        var runtime = File.ReadAllText(Path.Combine(repo, "Source/Mods/TownHallInterior/src/VanillaFurnishings.cs"));
        Assert.DoesNotContain("Instantiate(", runtime);
        Assert.DoesNotContain("DestroyImmediate", runtime);
        Assert.Contains("lightmapIndex = -1", runtime);
        Assert.Contains("FurniturePlan.Create(document)", runtime);
        Assert.Contains("FurniturePlan.CreateShell()", runtime);
        Assert.Contains("CleanupJournal", runtime);
    }

    [Fact]
    public void EveryChairSitsInAKneeSpaceAndFacesThatDesk()
    {
        // Desk asset: 1.0 x 2.0 m, closed modesty panel on local -X, open knee space on local +X
        // (10 vertices at knee height against 240 on the panel side). Yaw maps local +X to
        // (cos yaw, -sin yaw); the same convention holds for the chair facing.
        var create = typeof(FurniturePlan).GetMethod("Create");
        Assert.NotNull(create);
        var rows = System.Text.Json.JsonSerializer.SerializeToElement(create.Invoke(null, new object[] { Native() }))
            .EnumerateArray().ToArray();
        var chairs = rows.Where(r => r.GetProperty("Key").GetString() == "chair").ToArray();
        var desks = rows.Where(r => r.GetProperty("Key").GetString() == "desk").ToArray();
        Assert.NotEmpty(desks);

        foreach (var c in chairs)
        {
            double cx = c.GetProperty("X").GetDouble(), cy = c.GetProperty("Y").GetDouble(), cz = c.GetProperty("Z").GetDouble();
            double cyaw = c.GetProperty("Yaw").GetDouble() * Math.PI / 180;
            double fx = Math.Cos(cyaw), fz = -Math.Sin(cyaw);

            // A chair belongs to every desk of its storey whose open side it stands in front of.
            var proximity = desks
                .Where(d => Math.Abs(d.GetProperty("Y").GetDouble() - cy) < 1e-4)
                .Select(d =>
                {
                    double dx = d.GetProperty("X").GetDouble(), dz = d.GetProperty("Z").GetDouble();
                    double yaw = d.GetProperty("Yaw").GetDouble() * Math.PI / 180;
                    double ox = cx - dx, oz = cz - dz, len = Math.Sqrt(ox * ox + oz * oz);
                    return new { Name = d.GetProperty("Name").GetString(), Dx = dx, Dz = dz, Len = len,
                                 Knee = (Math.Cos(yaw) * ox - Math.Sin(yaw) * oz) / len };
                })
                .ToArray();
            // Free-standing seating (waiting area) has no desk nearby and is not part of this rule.
            if (!proximity.Any(x => x.Len < 3.0)) continue;
            var seated = proximity.Where(x => x.Len < 3.0 && x.Knee > 0.5).OrderBy(x => x.Len).ToArray();
            Assert.True(seated.Length > 0,
                c.GetProperty("Name").GetString() + " stands at no desk's knee space (yaw " + c.GetProperty("Yaw").GetDouble() + ")");

            var desk = seated[0];
            double dot = (fx * (desk.Dx - cx) + fz * (desk.Dz - cz)) / desk.Len;
            Assert.True(dot > 0.35,
                c.GetProperty("Name").GetString() + " faces away from " + desk.Name
                + " (dot " + dot.ToString("F2") + ", yaw " + c.GetProperty("Yaw").GetDouble() + ")");
        }
    }

    [Fact]
    public void NativeLayoutFitsRoomSupportsDesktopItemsAndKeepsWalkwaysClear()
    {
        var doc = Native();
        Assert.Equal(8, doc.Assets.Length);
        Assert.Equal(6259, doc.Assets.Sum(a => a.Parts.Sum(p => p.Vertices.Length)));
        var create = typeof(FurniturePlan).GetMethod("Create");
        Assert.NotNull(create);
        var plan = create.Invoke(null, new object[] { doc });
        var rows = System.Text.Json.JsonSerializer.SerializeToElement(plan).EnumerateArray().ToArray();
        Assert.Equal(55, rows.Length); // 20 earth storey + 35 upper storey
        Assert.Equal(rows.Length, rows.Select(r => r.GetProperty("Name").GetString()).Distinct().Count());
        Assert.Equal(11, rows.Count(r => r.GetProperty("Key").GetString() == "chair"));
        // Waiting zone: benches replace the eight loose chairs; two scaled plants frame the corners.
        Assert.Equal(5, rows.Count(r => r.GetProperty("Key").GetString() == "plant" && r.GetProperty("Scale").GetDouble() != 1d));
        var placed = new List<(string Name, string Key, bool Solid, double[] Min, double[] Max)>();
        foreach (var r in rows)
        {
            var key = r.GetProperty("Key").GetString()!;
            var a = FurniturePlan.PrepareAsset(doc.Assets.Single(a => a.Key == key));
            double x = r.GetProperty("X").GetDouble(), y = r.GetProperty("Y").GetDouble(), z = r.GetProperty("Z").GetDouble();
            double yaw = r.GetProperty("Yaw").GetDouble() * Math.PI / 180, c = Math.Cos(yaw), s = Math.Sin(yaw);
            var vertices = a.Parts.SelectMany(p => p.Vertices).Select(v => new[] { x + c * v[0] + s * v[2], y + v[1], z - s * v[0] + c * v[2] }).ToArray();
            var lo = Enumerable.Range(0, 3).Select(i => vertices.Min(v => v[i])).ToArray();
            var hi = Enumerable.Range(0, 3).Select(i => vertices.Max(v => v[i])).ToArray();
            var solid = r.GetProperty("Solid").GetBoolean();
            if (key != "door-frame")
            {
                Assert.InRange(lo[0], 54.55, 63.15); Assert.InRange(hi[0], 54.55, 63.15); // upper skin extents
                Assert.InRange(lo[2], 20.7, 41.3); Assert.InRange(hi[2], 20.7, 41.3);
                Assert.InRange(lo[1], PublicEntrance.FloorTop - .0001, 8.0); Assert.True(hi[1] < 8.0);
            }
            if (solid) Assert.True(
                Math.Abs(y - PublicEntrance.FloorTop) < 1e-4 || Math.Abs(y - RoomPlan.Upper) < 1e-4,
                r.GetProperty("Name").GetString() + " must stand on a storey floor");
            placed.Add((r.GetProperty("Name").GetString()!, key, solid, lo, hi));
        }
        bool Overlap(double[] a, double[] b, double[] c, double[] d) => Enumerable.Range(0, 3).All(i => Math.Min(b[i], d[i]) - Math.Max(a[i], c[i]) > .0001);
        foreach (var a in placed.Where(p => p.Solid))
        {
            Assert.False(Overlap(a.Min, a.Max, new[] { 54d, 1.54, 30.1 }, new[] { 60d, 3.6, 31.9 }), a.Name + " entrance");
            Assert.False(Overlap(a.Min, a.Max, new[] { 60.25, 1.54, 22 }, new[] { 62.45, 3.6, 29.9 }), a.Name + " stairs");
        }
        for (int i = 0; i < placed.Count; i++)
            for (int j = i + 1; j < placed.Count; j++)
                Assert.False(Overlap(placed[i].Min, placed[i].Max, placed[j].Min, placed[j].Max), placed[i].Name + " intersects " + placed[j].Name);
        // Only desktop items must stand on a desk; the two framed corner plants stand on the floor.
        // Desktop items must stand on a desk; floor plants stand on a storey floor.
        foreach (var item in placed.Where(p => p.Key is "computer" or "plant" &&
            p.Min[1] > PublicEntrance.FloorTop + .5 && Math.Abs(p.Min[1] - RoomPlan.Upper) > 1e-3))
            Assert.Contains(placed, table => table.Key == "desk" && Math.Abs(item.Min[1] - table.Max[1]) < .0001 &&
                item.Min[0] >= table.Min[0] - .0001 && item.Max[0] <= table.Max[0] + .0001 &&
                item.Min[2] >= table.Min[2] - .0001 && item.Max[2] <= table.Max[2] + .0001);
        var export = Environment.GetEnvironmentVariable("TOWNHALL_PREVIEW_DIR");
        if (!string.IsNullOrWhiteSpace(export))
        {
            Directory.CreateDirectory(export);
            File.WriteAllText(Path.Combine(export, "layout.json"), System.Text.Json.JsonSerializer.Serialize(new
            {
                assets = doc.Assets.Select(FurniturePlan.PrepareAsset),
                placements = plan,
                shell = FurniturePlan.CreateShell(),
                originalShell = PublicEntrance.CreateInterior(),
                room = RoomPlan.Create()
            }));
        }
    }

    private static FurnitureDocument Native()
    {
        using var stream = typeof(RoomPlan).Assembly.GetManifestResourceStream("TownHallInterior.NativeFurniture")!;
        using var bytes = new MemoryStream(); stream.CopyTo(bytes);
        return FurniturePlan.Read(bytes.ToArray());
    }

    [Fact]
    public void NativeFrameIsRaisedAndWidenedWithoutMutatingSourceAttributes()
    {
        var original = Native().Assets.Single(a => a.Key == "door-frame");
        var prepare = typeof(FurniturePlan).GetMethod("PrepareAsset");
        Assert.NotNull(prepare);
        var changed = Assert.IsType<FurnitureAsset>(prepare.Invoke(null, new object[] { original }));
        Assert.True(Covers(original, 2.15, 0), "Original frame header must reproduce the visual obstruction");
        for (double y = .1; y < PublicEntrance.PortalTop - 1.41 - .01; y += .025)
            for (double z = -.95; z < .95; z += .025)
                Assert.False(Covers(changed, y, z), $"Frame blocks y={y}, z={z}");
        Assert.InRange(changed.BoundsMax[1] + 1.41, 4.74, 4.76);
        for (int p = 0; p < original.Parts.Length; p++)
            for (int v = 0; v < original.Parts[p].Vertices.Length; v++)
                Assert.Equal(original.Parts[p].Vertices[v].Skip(3), changed.Parts[p].Vertices[v].Skip(3));
        Assert.Equal(original, Assert.Single(Native().Assets, a => a.Key == "door-frame"), new AssetGeometryComparer());
    }

    private sealed class AssetGeometryComparer : IEqualityComparer<FurnitureAsset>
    {
        public bool Equals(FurnitureAsset? a, FurnitureAsset? b) => System.Text.Json.JsonSerializer.Serialize(a) == System.Text.Json.JsonSerializer.Serialize(b);
        public int GetHashCode(FurnitureAsset a) => a.Key.GetHashCode();
    }

    private static bool Covers(FurnitureAsset a, double y, double z)
    {
        foreach (var p in a.Parts)
            foreach (var indices in p.Submeshes)
                for (int i = 0; i < indices.Length; i += 3)
                {
                    var v = p.Vertices[indices[i]]; var w = p.Vertices[indices[i + 1]]; var q = p.Vertices[indices[i + 2]];
                    double cross(double ay, double az, double by, double bz) => ay * bz - az * by;
                    var area = cross(w[1] - v[1], w[2] - v[2], q[1] - v[1], q[2] - v[2]);
                    if (Math.Abs(area) < 1e-10) continue;
                    var b = cross(y - v[1], z - v[2], q[1] - v[1], q[2] - v[2]) / area;
                    var c = cross(w[1] - v[1], w[2] - v[2], y - v[1], z - v[2]) / area;
                    if (b >= -1e-8 && c >= -1e-8 && b + c <= 1 + 1e-8) return true;
                }
        return false;
    }

    // Synthetic triangle tests the parser boundary, NOT native asset fidelity.
    private static FurnitureDocument Triangle()
    {
        double[] V(double x, double y, double z) => new[] { x, y, z, 0d, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0 };
        return new FurnitureDocument
        {
            Schema = 1,
            LevelSha256 = FacadePortal.LevelSha256,
            Assets = new[] {
            new FurnitureAsset { Key = "synthetic", BoundsMin = new[] { -.5, 0, -.5 }, BoundsMax = new[] { .5, 1, .5 },
                Parts = new[] { new FurniturePart { SourcePath = "Synthetic/Test", MeshName = "Test", SourceVertexCount = 3,
                    SourceBoundsMin = new[] { -.5, 0, -.5 }, SourceBoundsMax = new[] { .5, 1, .5 },
                    MaterialNames = new[] { "Test" }, Vertices = new[] { V(-.5,0,-.5), V(.5,0,-.5), V(-.5,1,.5) },
                    Submeshes = new[] { new[] { 0, 1, 2 } } } } } }
        };
    }

    [Theory]
    [InlineData("asset-null")]
    [InlineData("duplicate")]
    [InlineData("parts-null")]
    [InlineData("indices")]
    [InlineData("vertex")]
    [InlineData("materials")]
    [InlineData("bounds")]
    [InlineData("source-path")]
    [InlineData("source-bounds")]
    public void UntrustedGeometryIsRejected(string fault)
    {
        var d = Triangle(); var a = d.Assets[0]; var p = a.Parts[0];
        Assert.Single(FurniturePlan.Read(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(d)).Assets);
        switch (fault)
        {
            case "asset-null": d.Assets[0] = null!; break;
            case "duplicate": d = d with { Assets = new[] { a, a } }; break;
            case "parts-null": d.Assets[0] = a with { Parts = null! }; break;
            case "indices": a.Parts[0] = p with { Submeshes = new[] { new[] { 0, 1, 99 } } }; break;
            case "vertex": p.Vertices[0] = new double[13]; break;
            case "materials": a.Parts[0] = p with { MaterialNames = Array.Empty<string>() }; break;
            case "bounds": d.Assets[0] = a with { BoundsMax = new[] { 100d, 1, .5 } }; break;
            case "source-path": a.Parts[0] = p with { SourcePath = "../Other" }; break;
            case "source-bounds": a.Parts[0] = p with { SourceBoundsMin = Array.Empty<double>() }; break;
        }
        Assert.Throws<InvalidOperationException>(() => FurniturePlan.Read(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(d)));
    }

    [Fact]
    public void AssetReaderRejectsUnknownVersionsAndMissingGeometryBeforeAllocatingUnityObjects()
    {
        var read = typeof(FurniturePlan).GetMethod("Read");
        Assert.NotNull(read);
        foreach (var payload in new[] { "{}", "{\"schema\":1,\"levelSha256\":\"wrong\",\"assets\":[]}",
            "{\"schema\":1,\"levelSha256\":\"" + FacadePortal.LevelSha256 + "\",\"assets\":[]}" })
        {
            var failure = Assert.Throws<System.Reflection.TargetInvocationException>(() => read.Invoke(null, new object[] { System.Text.Encoding.UTF8.GetBytes(payload) }));
            Assert.IsType<InvalidOperationException>(failure.InnerException);
        }
    }

    [Fact]
    public void FurnishingPlanProvidesAnInsetCeilingWithoutChangingAcceptedEntranceCollision()
    {
        var type = typeof(RoomPlan).Assembly.GetType("TownHallInterior.FurniturePlan");
        Assert.NotNull(type);
        var shell = Assert.IsAssignableFrom<IReadOnlyList<RoomBox>>(type.GetMethod("CreateShell")!.Invoke(null, null));
        var panels = shell.Where(b => b.Name.StartsWith("ceiling-")).ToArray();
        Assert.Equal(4, panels.Length);
        Assert.All(shell, b => Assert.False(b.Solid));
        for (double x = 55.05; x < 62.4; x += .1)
            for (double z = 21.65; z < 40.4; z += .1)
            {
                var hole = x > 60.7 && x < 62.1 && z > 24 && z < 29.6;
                Assert.Equal(hole ? 0 : 1, panels.Count(b => x > b.X - b.Width / 2 && x < b.X + b.Width / 2 && z > b.Z - b.Depth / 2 && z < b.Z + b.Depth / 2));
            }
        Assert.All(panels, b => Assert.InRange(b.Y - b.Height / 2, 4.39f, 4.49f));
        // The accepted collision envelope remains in the existing geometry tests.
        Assert.Equal(8, PublicEntrance.CreateInterior().Count);
    }
}

using TownHallInterior;
using Xunit;

public sealed class FacadePortalTests
{
    [Fact]
    public void ThroughWallTriangleIsSplitAroundThePortalAndNotSimplyDeleted()
    {
        var original = Wall();
        var cut = FacadePortal.Cut(original);
        Assert.False(Covers(cut, 2.4, 31.0), "The brick wall still covers the doorway");
        Assert.True(Covers(cut, 2.4, 29.0), "Wall beside the doorway was lost");
        Assert.True(Covers(cut, 4.8, 31.0), "Wall above the raised doorway was lost");
        Assert.Equal(6 * 8 - (FacadePortal.Maximum[1] - FacadePortal.Minimum[1]) * (31.96 - 30.04), Area(cut), 6);
        Assert.True(Covers(original, 2.4, 31.0), "Shared/native input was mutated");
    }

    [Fact]
    public void PortalAndCollisionShellUseTheSameDesignHeight()
    {
        Assert.Equal(PublicEntrance.PortalTop, FacadePortal.Maximum[1]);
        Assert.Equal(1.53, PublicEntrance.FloorTop, 5); // Owned floor, not measured approach.
    }

    [Fact]
    public void InnerLintelAndLeavesCoverTheRaisedPortalInsteadOfLoweringTheDoorway()
    {
        var interior = PublicEntrance.CreateInterior();
        Assert.Equal(8, interior.Count); // floor split around the basement stair
        Assert.All(interior, b => Assert.True(b.Solid));
        var lintel = interior.Single(b => b.Name == "inner-lintel");
        // The visible head lining now hangs one centimetre below the portal top and overlaps the
        // leaf top, because the native cut overshoots and left a sky sliver over the leaves.
        Assert.Equal(PublicEntrance.HeadLiningBottom, lintel.Y - lintel.Height / 2, 4);
        Assert.Equal(PublicEntrance.HeadLiningDrop,
            PublicEntrance.LeafCenterY + PublicEntrance.LeafHeight / 2 - (lintel.Y - lintel.Height / 2), 3);
        Assert.Equal(PublicEntrance.LeafCenterY + PublicEntrance.LeafHeight / 2, FacadePortal.Maximum[1], 5);
        Assert.True(PublicEntrance.LeafCenterY - PublicEntrance.LeafHeight / 2 <= FacadePortal.Minimum[1]);
        // Separate source wiring guard: this does not execute the Unity adapter.
        var repo = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var source = System.IO.File.ReadAllText(System.IO.Path.Combine(repo, "Source/Mods/TownHallInterior/src/PublicHallHost.cs"));
        Assert.Contains("PublicEntrance.CreateInterior()", source);
        Assert.Contains("new Vector3(box.X, box.Y, box.Z)", source);
        Assert.Contains("new Vector3(box.Width, box.Height, box.Depth)", source);
        Assert.Contains("foreach (var part in PublicEntrance.LeafParts)", source);
        Assert.Equal(2, System.Text.RegularExpressions.Regex.Matches(source, @"PublicEntrance\.LeafCenterY").Count);
    }

    private static FacadeMesh Wall() => new()
    {
        Vertices = new[] { Vertex(0, 27), Vertex(6, 27), Vertex(6, 35), Vertex(0, 35) },
        Submeshes = new[] { new[] { 0, 2, 1, 0, 3, 2 } },
    };

    [Fact]
    public void CutPreservesUvChannelsWindingAndMaterialSlots()
    {
        var source = Wall();
        var cut = FacadePortal.Cut(source);
        foreach (var v in cut.Vertices)
        {
            Assert.Equal(v[1] / 6, v[10], 10);
            Assert.Equal(v[2] / 8, v[11], 10);
            Assert.Equal(v[1] / 12, v[12], 10);
            Assert.Equal(v[2] / 16, v[13], 10);
            Assert.Equal(-1, v[9]);
        }
        foreach (var sub in cut.Submeshes)
            for (int i = 0; i < sub.Length; i += 3)
            {
                var a = cut.Vertices[sub[i]]; var b = cut.Vertices[sub[i + 1]]; var c = cut.Vertices[sub[i + 2]];
                Assert.True((b[1] - a[1]) * (c[2] - a[2]) - (b[2] - a[2]) * (c[1] - a[1]) < 0);
            }
        Assert.Equal(source.Submeshes.Length, cut.Submeshes.Length);
        Assert.Equal(Area(cut), Area(FacadePortal.Cut(cut)), 8);
    }

    [Fact]
    public void AnUnrelatedPlaneAndConcreteThresholdAreUntouched()
    {
        var source = Wall();
        var far = source with { Vertices = source.Vertices.Select(v => { var c = (double[])v.Clone(); c[0] = 60; return c; }).ToArray() };
        Assert.Equal(far.Submeshes[0], FacadePortal.Cut(far).Submeshes[0]);
        var threshold = source with { Vertices = source.Vertices.Select(v => { var c = (double[])v.Clone(); c[1] *= 0.2; return c; }).ToArray() };
        Assert.Equal(threshold.Submeshes[0], FacadePortal.Cut(threshold).Submeshes[0]);
    }

    [Fact]
    public void InvalidPayloadsAreRejectedBeforeGeometryAllocation()
    {
        var badIndex = Wall() with { Submeshes = new[] { new[] { 0, 1, 90 } } };
        Assert.Throws<InvalidOperationException>(() => FacadePortal.Cut(badIndex));
        var badVertices = Wall().Vertices;
        badVertices[0][0] = double.NaN;
        Assert.Throws<InvalidOperationException>(() => FacadePortal.Cut(Wall() with { Vertices = badVertices }));
    }

    [Fact]
    public void FixtureLoadingRequiresTheExactThreeAttributedContributors()
    {
        var invalid = new FacadeDocument { Schema = 1, Sources = new[] { Wall() } };
        Assert.Throws<InvalidOperationException>(() => FacadePortal.ValidateDocument(invalid));
    }

    [Fact]
    public void RuntimePreparesFacadeBeforeMutatingCollidersAndPublishesBeforeActivatingRoom()
    {
        // Wiring guard, not a Unity/IL2CPP execution test.
        var repo = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var source = System.IO.File.ReadAllText(System.IO.Path.Combine(repo, "Source/Mods/TownHallInterior/src/PublicHallHost.cs"));
        int prepare = source.IndexOf("NativeFacadeSwap.Prepare", StringComparison.Ordinal);
        int collider = source.IndexOf("host.SwapColliders()", StringComparison.Ordinal);
        int publish = source.IndexOf("host._facade.Publish()", StringComparison.Ordinal);
        int activate = source.IndexOf("room.Publish()", StringComparison.Ordinal);
        Assert.True(prepare >= 0 && prepare < collider && collider < publish && publish < activate,
            "The scene still only hides Door, or publishes before the complete replacement is ready");
    }

    private static double[] Vertex(double y, double z) =>
        new[] { 54.35, y, z, -1.0, 0, 0, 0, 0, 1, -1, y / 6, z / 8, y / 12, z / 16 };

    private static bool Covers(FacadeMesh mesh, double y, double z)
    {
        foreach (var sub in mesh.Submeshes)
            for (int i = 0; i < sub.Length; i += 3)
            {
                var a = mesh.Vertices[sub[i]]; var b = mesh.Vertices[sub[i + 1]]; var c = mesh.Vertices[sub[i + 2]];
                if (new[] { a, b, c }.Any(v => v[0] < 54.18 || v[0] > 54.56)) continue;
                double projectedArea = (b[1] - a[1]) * (c[2] - a[2]) - (b[2] - a[2]) * (c[1] - a[1]);
                if (Math.Abs(projectedArea) < 1e-10) continue;
                static double Cross(double ay, double az, double by, double bz, double y, double z) =>
                    (by - ay) * (z - az) - (bz - az) * (y - ay);
                var s1 = Cross(a[1], a[2], b[1], b[2], y, z);
                var s2 = Cross(b[1], b[2], c[1], c[2], y, z);
                var s3 = Cross(c[1], c[2], a[1], a[2], y, z);
                if ((s1 >= -1e-8 && s2 >= -1e-8 && s3 >= -1e-8) ||
                    (s1 <= 1e-8 && s2 <= 1e-8 && s3 <= 1e-8)) return true;
            }
        return false;
    }

    [Fact]
    public void AllThreeRealF12WallContributionsHaveAClearPortalAfterCutting()
    {
        using var stream = typeof(FacadePortalTests).Assembly.GetManifestResourceStream("TownHallInterior.NativeFacade")!;
        using var bytes = new System.IO.MemoryStream();
        stream.CopyTo(bytes);
        var document = FacadePortal.Read(bytes.ToArray());
        Assert.Equal(new[] { "townhall/Walls", "Cube", "townhall/Cube" }, document.Sources.Select(s => s.Path));
        foreach (var source in document.Sources)
        {
            var before = System.Text.Json.JsonSerializer.Serialize(source);
            Assert.True(Covers(source, 2.4, 31), source.Path + " is not the native blocking plane");
            var cut = FacadePortal.Cut(source);
            for (int yi = 0; yi < 8; yi++)
                for (int zi = 0; zi < 7; zi++)
                    Assert.False(Covers(cut, 1.6 + yi * 0.4, 30.15 + zi * 0.28), source.Path + " still covers the portal");
            Assert.True(Covers(cut, 2.4, 29), source.Path + " lost the neighboring facade");
            Assert.Equal(Covers(source, 4.8, 31), Covers(cut, 4.8, 31));
            Assert.Equal(source.Submeshes.Length, cut.Submeshes.Length);
            Assert.Equal(source.MaterialNames, cut.MaterialNames);
            Assert.Equal(before, System.Text.Json.JsonSerializer.Serialize(source));
        }
    }

    private static double Area(FacadeMesh mesh)
    {
        double sum = 0;
        foreach (var sub in mesh.Submeshes)
            for (int i = 0; i < sub.Length; i += 3)
            {
                var a = mesh.Vertices[sub[i]]; var b = mesh.Vertices[sub[i + 1]]; var c = mesh.Vertices[sub[i + 2]];
                sum += Math.Abs((b[1] - a[1]) * (c[2] - a[2]) - (b[2] - a[2]) * (c[1] - a[1])) / 2;
            }
        return sum;
    }

    [Fact]
    public void EntranceHasAGeometricCutRatherThanOnlyHiddenDoorRenderers()
    {
        // The old build hides Door, but never cuts any of the independent wall meshes.
        Assert.NotNull(typeof(PublicEntrance).Assembly.GetType("TownHallInterior.FacadePortal"));
    }
}

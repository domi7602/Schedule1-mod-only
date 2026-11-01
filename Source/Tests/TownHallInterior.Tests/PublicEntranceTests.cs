using TownHallInterior;
using Xunit;

public sealed class PublicEntranceTests
{
    [Theory]
    [InlineData(58.7f, 1.6f, 31f)]
    [InlineData(61.2f, 1.6f, 35.65f)]
    [InlineData(55.7f, 1.6f, 24f)]
    [InlineData(56.8f, 1.6f, 39.4f)]
    public void MovingAwayInsideLetsTheLeavesClose(float x, float y, float z)
    {
        Assert.False(PublicEntrance.WantsDoorOpen(x, y, z));
        Assert.Equal(0f, PublicEntrance.MoveLeaf(1f, PublicEntrance.WantsDoorOpen(x, y, z), PublicEntrance.SlideSeconds));
    }

    [Theory]
    [InlineData(52.5f, 1.6f, 31f)]
    [InlineData(55.8f, 1.6f, 31f)]
    [InlineData(54.12f, 1.6f, 30.2f)]
    [InlineData(54.12f, 3.5f, 31.8f)]
    public void BothApproachesAndOccupiedThresholdKeepTheDoorOpen(float x, float y, float z)
    {
        Assert.True(PublicEntrance.WantsDoorOpen(x, y, z));
        Assert.Equal(1f, PublicEntrance.MoveLeaf(1f, PublicEntrance.WantsDoorOpen(x, y, z), 2f));
    }

    [Theory]
    [InlineData(48f, 1.6f, 31f)]
    [InlineData(54.12f, 5f, 31f)]
    [InlineData(54.12f, 1.6f, 35f)]
    [InlineData(float.NaN, 1.6f, 31f)]
    [InlineData(54.12f, float.PositiveInfinity, 31f)]
    public void RemoteUpperFloorAndInvalidPositionsDoNotOpenTheDoor(float x, float y, float z) =>
        Assert.False(PublicEntrance.WantsDoorOpen(x, y, z));

    [Fact]
    public void ProximityDecisionHasOneTestablePolicyUsedByTheRuntime()
    {
        var method = typeof(PublicEntrance).GetMethod("WantsDoorOpen");
        Assert.NotNull(method);
        Assert.True(Assert.IsType<bool>(method.Invoke(null, new object[] { 52.5f, 1.6f, 31f })));
        Assert.True(Assert.IsType<bool>(method.Invoke(null, new object[] { 55.8f, 1.6f, 31f })));
        var repo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var host = File.ReadAllText(Path.Combine(repo, "Source/Mods/TownHallInterior/src/PublicHallHost.cs"));
        Assert.Contains("PublicEntrance.WantsDoorOpen(p.x, p.y, p.z)", host);
    }

    [Fact]
    public void ColliderSwapStaysClosedUntilIdentityMatchesAndVisualPublicationIsStillRefused()
    {
        var matched = NativeSceneSwap.Inspect(new NativeSceneObservation(
            NativeSceneSwap.BuildingPath, NativeSceneSwap.BuildingGuid, NativeSceneSwap.Candidates, 1));
        Assert.True(matched.IdentityAndGeometryMatch);
        Assert.False(matched.CanPublish);
        Assert.True(PublicEntrance.ColliderSwapAllowed(matched));
        Assert.False(PublicEntrance.ColliderSwapAllowed(new NativeSceneInspection(false, false, "no")));
    }

    [Fact]
    public void ClosedLeavesSealTheRevealOnBothSides()
    {
        Assert.True(PublicEntrance.ClosedLeavesSealReveal());
        Assert.Equal(PublicEntrance.RevealSouthZ - PublicEntrance.LeafOverlap, PublicEntrance.LeafMinZ(0f), 4);
        Assert.Equal(PublicEntrance.RevealNorthZ + PublicEntrance.LeafOverlap, PublicEntrance.LeafMaxZ(0f), 4);
    }

    [Fact]
    public void ClosedLeavesMeetExactlyAtThePortalCentre()
    {
        var southEdge = PublicEntrance.LeafSouthZ(0f) + PublicEntrance.LeafWidth / 2f;
        var northEdge = PublicEntrance.LeafNorthZ(0f) - PublicEntrance.LeafWidth / 2f;
        Assert.Equal(southEdge, northEdge, 4);
        Assert.Equal((PublicEntrance.RevealSouthZ + PublicEntrance.RevealNorthZ) / 2f, southEdge, 4);
    }

    [Fact]
    public void OpenLeavesClearTheRevealForTheAcceptedWalkway()
    {
        Assert.True(PublicEntrance.OpenLeavesClearReveal());
        Assert.True(PublicEntrance.LeafSouthZ(1f) + PublicEntrance.LeafWidth / 2f <= PublicEntrance.RevealSouthZ);
        Assert.True(PublicEntrance.LeafNorthZ(1f) - PublicEntrance.LeafWidth / 2f >= PublicEntrance.RevealNorthZ);
    }

    [Fact]
    public void RuntimeLeavesUseTheSharedLeafGeometryInsteadOfLiterals()
    {
        var repo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var host = File.ReadAllText(Path.Combine(repo, "Source/Mods/TownHallInterior/src/PublicHallHost.cs"));
        Assert.Contains("PublicEntrance.LeafSouthZ(_open)", host);
        Assert.Contains("PublicEntrance.LeafNorthZ(_open)", host);
    }

    [Fact]
    public void LeafPartFacesNeverShareAPlaneSoTheBlackFrameCannotFlicker()
    {
        // Owner report 2026-10-29 18:36: the black head frame flickered. Rails, stiles and the
        // panel must therefore never put two visible faces on the same plane.
        var parts = PublicEntrance.LeafParts;
        Assert.Equal(5, parts.Count);
        // Leaves are cosmetic replacements for the hidden native door; a collider would block
        // NPC residents because the proximity sensor only tracks the local player.
        Assert.DoesNotContain(parts, p => p.Solid);
        for (int i = 0; i < parts.Count; i++)
            for (int j = i + 1; j < parts.Count; j++)
            {
                var a = parts[i];
                var b = parts[j];
                Assert.False(Coplanar(a, b), $"Coplanar visible faces: {a.Name} / {b.Name}");
            }

        static (float Min, float Max) Span(float offset, float size) => (offset - size / 2, offset + size / 2);
        static bool SharesPlane((float Min, float Max) a, (float Min, float Max) b) =>
            Math.Abs(a.Min - b.Min) < 1e-4f || Math.Abs(a.Min - b.Max) < 1e-4f ||
            Math.Abs(a.Max - b.Min) < 1e-4f || Math.Abs(a.Max - b.Max) < 1e-4f;
        static bool Overlaps((float Min, float Max) a, (float Min, float Max) b) =>
            Math.Min(a.Max, b.Max) - Math.Max(a.Min, b.Min) > 1e-4f;
        static bool Coplanar(PublicEntrance.LeafPart a, PublicEntrance.LeafPart b)
        {
            (float, float) ax = Span(0, a.ThicknessX), bx = Span(0, b.ThicknessX);
            (float, float) ay = Span(a.OffsetY, a.Height), by = Span(b.OffsetY, b.Height);
            (float, float) az = Span(a.OffsetZ, a.WidthZ), bz = Span(b.OffsetZ, b.WidthZ);
            return (SharesPlane(ax, bx) && Overlaps(ay, by) && Overlaps(az, bz)) ||
                   (SharesPlane(ay, by) && Overlaps(ax, bx) && Overlaps(az, bz)) ||
                   (SharesPlane(az, bz) && Overlaps(ax, bx) && Overlaps(ay, by));
        }
    }

    [Fact]
    public void RuntimeNeverTurnsOffNativeDoorUsabilitySoNpcSchedulesKeepWorking()
    {
        // Owner session 2026-10-29 20:29: NPCEvent_StayInBuilding.OnActiveTick threw every
        // 0.5 s. Suppression must stay limited to the player prompt; `Usable` gates the door
        // itself, so an NPC that lives in the building cannot enter once it is cleared.
        var repo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        var host = File.ReadAllText(Path.Combine(repo, "Source/Mods/TownHallInterior/src/PublicHallHost.cs"));
        Assert.DoesNotContain(".Usable =", host);
        // NPCs despawn while inside; a knock is what brings them back. Suppressing the knock
        // therefore removed the owner's only way to reach the clerk.
        Assert.DoesNotContain(".CanKnock =", host);
        Assert.Contains("HideNativeDoorSurface", host);
    }

    [Fact]
    public void LeavesSlideOpenAndShutWithoutOvershoot()
    {
        Assert.Equal(0f, PublicEntrance.MoveLeaf(0f, false, 1f));
        var half = PublicEntrance.MoveLeaf(0f, true, PublicEntrance.SlideSeconds / 2f);
        Assert.Equal(0.5f, half, 3);
        Assert.Equal(1f, PublicEntrance.MoveLeaf(0.9f, true, 1f));
        Assert.Equal(0f, PublicEntrance.MoveLeaf(0.1f, false, 1f));
        Assert.Equal(0f, PublicEntrance.MoveLeaf(float.NaN, false, 0.1f));
    }
}

// Tests the linked production preflight, not live Unity geometry.
using System;
using System.Linq;
using TownHallInterior;
using Xunit;

public sealed class NativeSceneSwapTests
{
    // Independent extracted data, not a copy of the production manifest at runtime.
    private static NativeSceneObservation Correct() => new(
        "Map/Hyland Point/Region_Downtown/TownCenter/TownHall", "c747a750-98cb-47fc-9c40-cf5912acc390",
        new[] {
            new NativeColliderObservation("Map/Hyland Point/Region_Downtown/TownCenter/TownHall/townhall/Walls", "BoxCollider", 54.35f,1.25f,20.5f,63.35f,8.25f,41.5f,true,false,1),
            new NativeColliderObservation("Map/Hyland Point/Region_Downtown/TownCenter/TownHall/Cube", "BoxCollider", 54.4f,.65f,20.6f,63.3f,9.65f,41.4f,true,false,1),
            new NativeColliderObservation("Map/Hyland Point/Region_Downtown/TownCenter/TownHall/townhall/UpperBrickTrim", "BoxCollider", 54.3f,4.6f,20.45f,63.4f,4.9f,41.55f,true,false,1),
            new NativeColliderObservation("Map/Hyland Point/Region_Downtown/TownCenter/TownHall/townhall/LowerBrickTrim", "BoxCollider", 54.3f,1.1f,20.45f,63.4f,1.4f,41.55f,true,false,1),
            new NativeColliderObservation("Map/Hyland Point/Region_Downtown/TownCenter/TownHall/townhall/Door/Quad", "MeshCollider", 54.192f,1.092f,30.039f,54.192f,3.950f,31.961f,true,false,1),
        }, 1);

    [Fact]
    public void KnownGeometryIsIdentifiedButPublicationStillRefuses()
    {
        var result = NativeSceneSwap.Inspect(Correct());
        Assert.True(result.IdentityAndGeometryMatch, result.Reason);
        Assert.False(result.CanPublish);
        Assert.Contains("exterior", result.Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void WrongBuildingIdentityIsRejected()
    {
        var obs = Correct();
        Assert.False(NativeSceneSwap.Inspect(obs with { BuildingGuid = "wrong" }).IdentityAndGeometryMatch);
        Assert.False(NativeSceneSwap.Inspect(obs with { BuildingPath = "Other/TownHall" }).IdentityAndGeometryMatch);
        Assert.False(NativeSceneSwap.Inspect(obs with { BuildingCount = 2 }).IdentityAndGeometryMatch);
    }

    [Fact]
    public void MissingOrDuplicateColliderIsRejected()
    {
        var obs = Correct();
        Assert.False(NativeSceneSwap.Inspect(obs with { Colliders = obs.Colliders.Skip(1).ToArray() }).IdentityAndGeometryMatch);
        Assert.False(NativeSceneSwap.Inspect(obs with { Colliders = obs.Colliders.Concat(new[] { obs.Colliders[0] }).ToArray() }).IdentityAndGeometryMatch);
        var bad = obs.Colliders.ToArray();
        bad[0] = bad[0] with { ComponentCount = 2 };
        Assert.False(NativeSceneSwap.Inspect(obs with { Colliders = bad }).IdentityAndGeometryMatch);
    }

    [Fact]
    public void TriggerWrongKindOrAlteredBoundsAreRejected()
    {
        var obs = Correct();
        var items = obs.Colliders.ToArray();
        items[0] = items[0] with { IsTrigger = true };
        Assert.False(NativeSceneSwap.Inspect(obs with { Colliders = items }).IdentityAndGeometryMatch);
        items[0] = items[0] with { IsTrigger = false, Kind = "MeshCollider" };
        Assert.False(NativeSceneSwap.Inspect(obs with { Colliders = items }).IdentityAndGeometryMatch);
        items[0] = items[0] with { Kind = "BoxCollider", MaxX = items[0].MaxX + 0.2f };
        Assert.False(NativeSceneSwap.Inspect(obs with { Colliders = items }).IdentityAndGeometryMatch);
        items[0] = items[0] with { MaxX = float.NaN };
        Assert.False(NativeSceneSwap.Inspect(obs with { Colliders = items }).IdentityAndGeometryMatch);
    }

    [Fact]
    public void MissingEnabledStateIsNotAReasonToMutateOrPublish()
    {
        var obs = Correct();
        var items = obs.Colliders.ToArray();
        items[0] = items[0] with { Enabled = false };
        var result = NativeSceneSwap.Inspect(obs with { Colliders = items });
        Assert.True(result.IdentityAndGeometryMatch);
        Assert.False(result.CanPublish);
    }
}


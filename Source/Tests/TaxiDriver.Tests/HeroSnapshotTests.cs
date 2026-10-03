using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Tests for <see cref="HeroSnapshot"/> (finding 7). The old bit-packed int
/// signature wrapped its 5 fare bits every $32, freezing the fare display;
/// the typed snapshot must distinguish every boundary fare, re-render on
/// destination/override change, and stay put on identical state.
/// </summary>
public class HeroSnapshotTests
{
    private static HeroSnapshot Base(int fare = 0, string destination = "none selected") =>
        new(false, false, false, false, false, false, false, false, false, false, false,
            destination, fare, string.Empty);

    [Fact]
    public void BoundaryFares_ArePairwiseDistinct()
    {
        int[] fares = { 0, 1, 15, 16, 31, 32, 33, 63, 64, 128, 192 };
        HeroSnapshot[] snapshots = fares.Select(f => Base(f)).ToArray();
        for (int i = 0; i < snapshots.Length; i++)
            for (int j = i + 1; j < snapshots.Length; j++)
                Assert.NotEqual(snapshots[i], snapshots[j]);
    }

    [Fact]
    public void IdenticalSnapshots_AreEqual() =>
        Assert.Equal(Base(5, "Motel"), Base(5, "Motel"));

    [Fact]
    public void DestinationChange_Rerenders() =>
        Assert.NotEqual(Base(5, "Motel"), Base(5, "Barn"));

    [Fact]
    public void SameDestinationNewFare_Rerenders() =>
        Assert.NotEqual(Base(5, "Motel"), Base(6, "Motel"));

    [Fact]
    public void OverrideChange_Rerenders() =>
        Assert.NotEqual(Base() with { Override = "Taxi ordered." }, Base());
}

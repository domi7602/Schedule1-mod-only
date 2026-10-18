using Xunit;
namespace TaxiDriver.Tests;

public class DestinationSelectionTests
{
    [Fact]
    public void RepeatedSameLogicalPickWhileDrivingIsNoOp()
    {
        var a = new DestinationSelection("A", "property", new DestinationPoint(10, 0, 0));
        var p = new DestinationPoint(20, 0, 0);
        Assert.Equal(DestinationPickMode.NoOp,
            DestinationSelectionPolicy.Select(a, p, p, "lot", "Lot", a, p, "lot", "Lot", true, true, false).Mode);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    public void GiveUpOrNonDrivingPickAllowsFreshDispatch(bool riding, bool driving, bool gaveUp)
    {
        var a = new DestinationSelection("A", "property", new DestinationPoint(10, 0, 0));
        var p = new DestinationPoint(20, 0, 0);
        Assert.Equal(DestinationPickMode.Dispatch,
            DestinationSelectionPolicy.Select(a, p, p, "lot", "Lot", a, p, "lot", "Lot", riding, driving, gaveUp).Mode);
    }

    [Fact]
    public void SharedResolverIdentityRetainsTheActualReachableOrFallbackRoute()
    {
        var a = new DestinationSelection("A", "property", new DestinationPoint(10, 0, 0));
        var b = a with { Name = "B", Goal = new DestinationPoint(30, 0, 0) };
        var wanted = new DestinationPoint(20, 0, 0);
        var actual = new DestinationPoint(25, 0, 0);
        var result = DestinationSelectionPolicy.Select(a, wanted, actual, "reachable point", "Fallback", b,
            wanted, "lot entry", "Original", true, true, false);
        Assert.Equal(DestinationPickMode.MetadataOnly, result.Mode);
        Assert.Equal(actual, result.RoutePoint);
        Assert.Equal("reachable point", result.DropOff);
        Assert.Equal("Fallback", result.Lot);
        Assert.Equal(b, result.Selection);
        Assert.Equal("B", result.ActiveName);
        Assert.Equal(DestinationPickMode.NoOp, DestinationSelectionPolicy.Select(b, wanted, actual,
            result.DropOff, result.Lot, b, wanted, "lot entry", "Original", true, true, false).Mode);
    }

    [Fact]
    public void NearbyButDifferentPhysicalDropOffNeedsDispatch()
    {
        var a = new DestinationSelection("A", "property", new DestinationPoint(10, 0, 0));
        var p = new DestinationPoint(20, 0, 0);
        Assert.Equal(DestinationPickMode.Dispatch, DestinationSelectionPolicy.Select(a, p, p, "lot", "Lot",
            a with { Name = "B" }, p with { X = 20.5f }, "lot", "Lot", true, true, false).Mode);
    }

    [Fact]
    public void SamePhysicalPointRefreshesDropOffMetadataOnDifferentPick()
    {
        var a = new DestinationSelection("A", "property", new DestinationPoint(10, 0, 0));
        var p = new DestinationPoint(20, 0, 0);
        var result = DestinationSelectionPolicy.Select(a, p, p, "old", "Old", a with { Name = "B" },
            p, "new", "New", true, true, false);
        Assert.Equal("new", result.DropOff);
        Assert.Equal("New", result.Lot);
    }

    [Fact]
    public void SameNameDifferentKindOrGoalIsNotTheSelectedLogicalDestination()
    {
        var a = new DestinationSelection("A", "property", new DestinationPoint(10, 0, 0));
        Assert.True(DestinationSelectionPolicy.IsSelected(a, a));
        Assert.False(DestinationSelectionPolicy.IsSelected(a, a with { Kind = "deal" }));
        Assert.False(DestinationSelectionPolicy.IsSelected(a, a with { Goal = new DestinationPoint(10.2f, 0, 0) }));
        Assert.False(DestinationSelectionPolicy.IsSelected(null, a));
    }

    [Fact]
    public void SharedDropOffChangesLogicalGoalAndActiveNameWithoutDispatch()
    {
        var a = new DestinationSelection("A", "property", new DestinationPoint(10, 0, 0));
        var b = new DestinationSelection("B", "deal", new DestinationPoint(30, 0, 0));
        var point = new DestinationPoint(20, 0, 0);
        var result = DestinationSelectionPolicy.Select(a, point, point, "lot entry", "Lot", b, point, "lot entry", "Lot", true, true, false);
        Assert.Equal(DestinationPickMode.MetadataOnly, result.Mode);
        Assert.Equal(b, result.Selection);
        Assert.Equal("B", result.ActiveName);
        Assert.Equal(point, result.RoutePoint);
    }
}

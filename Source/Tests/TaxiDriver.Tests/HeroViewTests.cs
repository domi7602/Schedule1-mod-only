using Xunit;
namespace TaxiDriver.Tests;

public class HeroViewTests
{
    private static HeroSnapshot Riding => new(true, false, false, false, true, false, false,
        false, false, true, false, "A", 12, "", "lot entry", "A", true);
    [Fact]
    public void ExistingIdleBoardingAndWaitingInstructionsArePreserved()
    {
        var idle = Riding with { RideActive = false, Picked = false, Vehicle = false };
        Assert.Equal("No destination picked", HeroView.Build(idle).Value);
        Assert.Equal("No destination picked", HeroView.Build(idle with { Vehicle = true }).Value);
        Assert.Equal("Or pick a destination below", HeroView.Build(idle with { AwaitingBoard = true }).Value);
        Assert.Equal("Pick a destination below", HeroView.Build(Riding with { AwaitingDestination = true }).Meta);
    }

    [Fact]
    public void ActiveTripPresenceOnlyChangeRefreshesFallbackTitle()
    {
        var before = Riding with { ActiveTripName = "B", HasActiveTrip = false };
        var after = before with { HasActiveTrip = true };
        Assert.NotEqual(before, after);
        Assert.Equal("RIDING TO A", HeroView.Build(before).Title);
        Assert.Equal("RIDING TO B", HeroView.Build(after).Title);
    }

    [Fact]
    public void DropOffOnlyChangeInvalidatesSnapshotAndChangesRenderedMeta()
    {
        var before = Riding;
        var after = before with { DropOff = "reachable point (game's arrival check)" };
        Assert.NotEqual(before, after);
        Assert.NotEqual(HeroView.Build(before).Meta, HeroView.Build(after).Meta);
        Assert.Equal("Drop-off: reachable point", HeroView.Build(after).Meta);
    }
    [Fact]
    public void ActiveTripOnlyChangeInvalidatesSnapshotAndChangesTitle()
    {
        var before = Riding;
        var after = before with { ActiveTripName = "B" };
        Assert.NotEqual(before, after);
        Assert.Equal("RIDING TO B", HeroView.Build(after).Title);
    }
}

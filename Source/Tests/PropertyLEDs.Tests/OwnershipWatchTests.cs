using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

public sealed class OwnershipWatchTests
{
    [Fact]
    public void PartialOwnedListDoesNotEndTheProbeWindow()
    {
        Assert.True(OwnershipWatch.ShouldProbe(0, knownCount: 1, now: 2, nextProbeAt: 1));
        Assert.True(OwnershipWatch.ShouldProbe(3, knownCount: 2, now: 5, nextProbeAt: 4));
    }

    [Fact]
    public void ProbeStopsAtTheAttemptCapAndBeforeTheInterval()
    {
        Assert.False(OwnershipWatch.ShouldProbe(OwnershipWatch.MaxAttempts, knownCount: 0, now: 100, nextProbeAt: 1));
        Assert.False(OwnershipWatch.ShouldProbe(1, knownCount: 0, now: 0.5, nextProbeAt: 1));
    }

    [Fact]
    public void UnchangedIdsDoNotCountAsANewOwnedSet()
    {
        Assert.False(OwnershipWatch.IdsChanged(new[] { "rv", "motelroom" }, new[] { "motelroom", "rv" }));
        Assert.False(OwnershipWatch.IdsChanged(Array.Empty<string>(), Array.Empty<string>()));
    }

    [Fact]
    public void ALaterApartmentChangesTheOwnedSet()
    {
        Assert.True(OwnershipWatch.IdsChanged(new[] { "rv" }, new[] { "motelroom", "rv" }));
        Assert.True(OwnershipWatch.IdsChanged(null, new[] { "motelroom" }));
        Assert.True(OwnershipWatch.IdsChanged(new[] { "motelroom" }, null));
    }

    [Fact]
    public void EmptyFollowUpReadDoesNotReplaceAKnownOwnedSet()
    {
        Assert.False(OwnershipWatch.ShouldCommit(new[] { "motelroom", "rv" }, Array.Empty<string>()));
        Assert.False(OwnershipWatch.ShouldCommit(new[] { "motelroom" }, null));
        Assert.True(OwnershipWatch.ShouldCommit(new[] { "rv" }, new[] { "motelroom", "rv" }));
        Assert.False(OwnershipWatch.ShouldCommit(new[] { "rv", "motelroom" }, new[] { "motelroom", "rv" }));
    }
}

using System;
using TaxiDriver;
using Xunit;
namespace TaxiDriver.Tests;

public class PassengerCleanupPolicyTests
{
    private static readonly IntPtr Taxi = new(123);
    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    public void RepeatedStopWhileBlockedSpendsNoAttempts(int state)
    {
        PlayerOccupancy occupancy = (PlayerOccupancy)state;
        var ledger = new VehicleOwnershipLedger(); Assert.True(ledger.TryRegister(Taxi));
        for (int i = 0; i < 8; i++)
            Assert.False(PassengerCleanupPolicy.TryBeginDestroy(ledger, Taxi, true, true, occupancy));
        Assert.Equal(0, ledger.DestroyAttempts);
        Assert.Equal(VehicleOwnershipKind.Owned, ledger.Classify(Taxi, true));
        Assert.False(ledger.DestroyPending); Assert.False(ledger.DestroyUnknown);
        // Passenger exits (or unreadable observation becomes readable): first real
        // native request is still available; only independently confirmed death clears.
        Assert.True(PassengerCleanupPolicy.TryBeginDestroy(ledger, Taxi, true, true, PlayerOccupancy.Empty));
        Assert.Equal(1, ledger.DestroyAttempts);
        Assert.True(ledger.ResolveDestroy(Taxi, false));
        Assert.Equal(IntPtr.Zero, ledger.TrackedPointer);
    }
    [Fact]
    public void BlockedUnknownDestroyRetainsTheOneExplicitRetry()
    {
        var l = new VehicleOwnershipLedger(); l.TryRegister(Taxi);
        Assert.True(l.TryBeginDestroy(Taxi, true, false)); l.ResolveDestroy(Taxi, true);
        Assert.False(PassengerCleanupPolicy.TryBeginDestroy(l, Taxi, true, true, PlayerOccupancy.Occupied));
        Assert.Equal(1, l.DestroyAttempts); Assert.True(l.DestroyUnknown);
        Assert.False(PassengerCleanupPolicy.TryBeginDestroy(l, Taxi, true, false, PlayerOccupancy.Empty));
        Assert.True(PassengerCleanupPolicy.TryBeginDestroy(l, Taxi, true, true, PlayerOccupancy.Empty));
        l.ResolveDestroy(Taxi, true);
        Assert.False(PassengerCleanupPolicy.TryBeginDestroy(l, Taxi, true, true, PlayerOccupancy.Empty));
        Assert.Equal(2, l.DestroyAttempts);
    }
    [Theory]
    [InlineData(false, false, 2)]
    [InlineData(false, true, 0)]
    [InlineData(true, false, 0)]
    [InlineData(true, true, 0)]
    public void AggregateBitNeverIdentifiesARemotePlayer(bool localInside, bool aggregateOccupied, int expected) =>
        Assert.Equal((PlayerOccupancy)expected, PassengerCleanupPolicy.FinalizeEmptyPlayerProof(localInside, aggregateOccupied));

    [Theory]
    [InlineData(true, false, 0, 123, 2)]
    [InlineData(true, true, 123, 123, 1)]
    [InlineData(true, true, 456, 123, 2)]
    [InlineData(false, false, 0, 123, 0)]
    [InlineData(true, true, 0, 123, 0)]
    [InlineData(true, false, 0, 0, 0)]
    public void CurrentVehicleUsesNativeNetworkObjectIdentity(bool readable, bool present, long current, long target, int expected) =>
        Assert.Equal((PlayerOccupancy)expected, PassengerCleanupPolicy.ObserveCurrentVehicle(readable, present, new IntPtr(current), new IntPtr(target)));

    [Fact]
    public void EmptyDoesNotBypassForeignOrPendingGuards()
    {
        var l = new VehicleOwnershipLedger(); l.TryRegister(Taxi);
        Assert.False(PassengerCleanupPolicy.TryBeginDestroy(l, new IntPtr(456), true, true, PlayerOccupancy.Empty));
        Assert.True(PassengerCleanupPolicy.TryBeginDestroy(l, Taxi, true, true, PlayerOccupancy.Empty));
        Assert.False(PassengerCleanupPolicy.TryBeginDestroy(l, Taxi, true, true, PlayerOccupancy.Empty));
        Assert.Equal(1, l.DestroyAttempts); Assert.True(l.DestroyPending);
    }
}

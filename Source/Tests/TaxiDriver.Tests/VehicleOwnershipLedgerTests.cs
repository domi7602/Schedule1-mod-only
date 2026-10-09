using System;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public sealed class VehicleOwnershipLedgerTests
{
    private static IntPtr Ptr(long value) => new(value);

    [Fact]
    public void Register_RejectsZeroAndASecondVehicle()
    {
        var ledger = new VehicleOwnershipLedger();

        Assert.False(ledger.TryRegister(IntPtr.Zero));
        Assert.True(ledger.TryRegister(Ptr(0x101)));
        Assert.False(ledger.TryRegister(Ptr(0x202)));
        Assert.Equal(Ptr(0x101), ledger.TrackedPointer);
    }

    [Fact]
    public void Classify_DistinguishesOwnedForeignInvalidAndUntracked()
    {
        var ledger = new VehicleOwnershipLedger();
        Assert.Equal(VehicleOwnershipKind.Foreign, ledger.Classify(Ptr(0x202), isAlive: true));
        Assert.Equal(VehicleOwnershipKind.None, ledger.Classify(IntPtr.Zero, isAlive: false));

        Assert.True(ledger.TryRegister(Ptr(0x101)));
        Assert.Equal(VehicleOwnershipKind.Owned, ledger.Classify(Ptr(0x101), isAlive: true));
        Assert.Equal(VehicleOwnershipKind.Foreign, ledger.Classify(Ptr(0x202), isAlive: true));
        Assert.Equal(VehicleOwnershipKind.Invalid, ledger.Classify(Ptr(0x101), isAlive: false));
        Assert.Equal(VehicleOwnershipKind.Invalid, ledger.Classify(IntPtr.Zero, isAlive: false));
    }

    [Fact]
    public void ForeignOrInvalidVehicle_CannotBeMutatedOrDestroyed()
    {
        var ledger = new VehicleOwnershipLedger();
        Assert.True(ledger.TryRegister(Ptr(0x101)));

        Assert.False(ledger.CanMutate(Ptr(0x202), isAlive: true));
        Assert.False(ledger.TryBeginDestroy(Ptr(0x202), isAlive: true, explicitRetry: true));
        Assert.False(ledger.CanMutate(Ptr(0x101), isAlive: false));
        Assert.False(ledger.TryBeginDestroy(Ptr(0x101), isAlive: false, explicitRetry: true));
        Assert.Equal(Ptr(0x101), ledger.TrackedPointer);
        Assert.Equal(0, ledger.DestroyAttempts);
    }

    [Fact]
    public void PendingDestroy_RetainsOwnershipAndBlocksAnotherDestroyCall()
    {
        var ledger = new VehicleOwnershipLedger();
        IntPtr taxi = Ptr(0x101);
        Assert.True(ledger.TryRegister(taxi));

        Assert.True(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: false));
        Assert.Equal(VehicleOwnershipKind.DestroyPending, ledger.Classify(taxi, isAlive: true));
        Assert.False(ledger.CanMutate(taxi, isAlive: true));
        Assert.False(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: true));
        Assert.Equal(taxi, ledger.TrackedPointer);
        Assert.Equal(1, ledger.DestroyAttempts);
    }

    [Fact]
    public void AmbiguousDestroy_RetainsOwnershipAndAllowsOnlyOneExplicitRetry()
    {
        var ledger = new VehicleOwnershipLedger();
        IntPtr taxi = Ptr(0x101);
        Assert.True(ledger.TryRegister(taxi));
        Assert.True(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: false));
        Assert.False(ledger.ResolveDestroy(taxi, isAlive: true));

        Assert.Equal(VehicleOwnershipKind.DestroyUnknown, ledger.Classify(taxi, isAlive: true));
        Assert.Equal(taxi, ledger.TrackedPointer);
        Assert.False(ledger.CanMutate(taxi, isAlive: true));
        Assert.False(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: false));
        Assert.True(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: true));
        Assert.False(ledger.ResolveDestroy(taxi, isAlive: true));
        Assert.False(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: true));
        Assert.Equal(taxi, ledger.TrackedPointer);
        Assert.Equal(2, ledger.DestroyAttempts);
    }

    [Fact]
    public void ConfirmedDestroy_ClearsOwnershipOnlyAfterTheVehicleIsDead()
    {
        var ledger = new VehicleOwnershipLedger();
        IntPtr taxi = Ptr(0x101);
        Assert.True(ledger.TryRegister(taxi));
        Assert.True(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: false));

        Assert.False(ledger.ResolveDestroy(taxi, isAlive: true));
        Assert.Equal(taxi, ledger.TrackedPointer);
        Assert.True(ledger.TryBeginDestroy(taxi, isAlive: true, explicitRetry: true));
        Assert.True(ledger.ResolveDestroy(taxi, isAlive: false));
        Assert.Equal(IntPtr.Zero, ledger.TrackedPointer);
        Assert.Equal(VehicleOwnershipKind.None, ledger.Classify(IntPtr.Zero, isAlive: false));
    }
}

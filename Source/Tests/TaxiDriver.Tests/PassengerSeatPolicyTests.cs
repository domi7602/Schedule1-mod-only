using System;
using TaxiDriver;
using Xunit;
namespace TaxiDriver.Tests;

public class PassengerSeatPolicyTests
{
    private static readonly IntPtr Local = new(11);
    private static PassengerSeatSnapshot Seat(bool driver = false, long occupant = 0, bool readable = true, bool? occupied = null) =>
        new(driver, readable, occupant != 0, new IntPtr(occupant), occupied ?? occupant != 0);
    [Fact] public void SkipsForeignSeatAndSelectsVacancy() => Assert.Equal(1, PassengerSeatPolicy.Select(2, i => i == 0 ? Seat(occupant: 22) : Seat(), Local));
    [Fact] public void FullForeignCarHasNoSeat() => Assert.Equal(-1, PassengerSeatPolicy.Select(3, _ => Seat(occupant: 22), Local));
    [Fact] public void LocalNativeIdentityIsPreferredOverEarlierVacancy() => Assert.Equal(1, PassengerSeatPolicy.Select(2, i => i == 0 ? Seat() : Seat(occupant: 11), Local));
    [Fact] public void DriverSeatIsNeverPassenger() => Assert.Equal(-1, PassengerSeatPolicy.Select(1, _ => Seat(driver: true), Local));
    [Fact] public void UnreadableSeatCannotBeSelected() => Assert.Equal(-1, PassengerSeatPolicy.Select(1, _ => Seat(readable: false), Local));
    [Fact] public void NullOccupantWithNativeOccupiedCannotBeSelected() => Assert.False(PassengerSeatPolicy.CanUse(Seat(occupied: true), Local));
    [Fact] public void PresentZeroPointerIsUnknownNotVacant() => Assert.False(PassengerSeatPolicy.CanUse(new(false, true, true, IntPtr.Zero, false), Local));
    [Fact] public void MissingLocalIdentityFailsClosed() => Assert.Equal(-1, PassengerSeatPolicy.Select(1, _ => Seat(), IntPtr.Zero));
    [Fact] public void NativeIdentityNotManagedWrapperIdentity() => Assert.True(PassengerSeatPolicy.CanUse(Seat(occupant: 11), new IntPtr(11)));
    [Fact] public void ReadFailureDoesNotHideLaterSafeSeat() => Assert.Equal(1, PassengerSeatPolicy.Select(2, i => i == 0 ? Seat(readable: false) : Seat(), Local));

    [Theory]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    [InlineData(true, true, false)]
    public void DriverReservationRequiresRawAbsenceAndNativeVacancy(bool occupantPresent, bool isOccupied, bool expected) =>
        Assert.Equal(expected, PassengerSeatPolicy.CanReserveDriverSeat(occupantPresent, isOccupied));

    [Fact]
    public void DestroyedForeignProxyCannotBeOverwrittenEvenWhenUnityAndNativeReportVacancy()
    {
        UnityNullProxy? occupant = new();
        Assert.True(occupant == null);
        bool isOccupied = occupant != null; // Models the native getter's Unity-null check.
        Assert.False(isOccupied);
        Assert.False(PassengerSeatPolicy.CanReserveDriverSeat(!ReferenceEquals(occupant, null), isOccupied));
    }

    private sealed class UnityNullProxy
    {
        public static bool operator ==(UnityNullProxy? left, UnityNullProxy? right) => true;
        public static bool operator !=(UnityNullProxy? left, UnityNullProxy? right) => false;
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
    }
}

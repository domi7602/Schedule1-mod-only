using System;
using Xunit;
namespace TaxiDriver.Tests;

public class MultiplayerSeatWiringTests
{
    [Fact]
    public void BoardingNeverReservesAForeignDriverOrEntersAFullCar()
    {
        string s = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string ride = DriverSafetySourceContractTests.Section(s, "internal static bool Ride()", "internal const float ExitDebounceSeconds");
        Assert.Contains("FindSeat(veh, driver: false) == null", ride);
        string reservation = DriverSafetySourceContractTests.Section(ride,
            "driverSeat = FindSeat(veh, driver: true);", "reserved = true;");
        Assert.Contains("if (driverSeat == null)", reservation);
        Assert.Contains("Player? driverOccupant = driverSeat.Occupant;", reservation);
        Assert.Contains("bool driverOccupantPresent = !ReferenceEquals(driverOccupant, null);", reservation);
        Assert.Contains("if (!PassengerSeatPolicy.CanReserveDriverSeat(driverOccupantPresent, driverSeat.isOccupied))", reservation);
        Assert.True(reservation.IndexOf("RefusePassengerRide", reservation.IndexOf("CanReserveDriverSeat", StringComparison.Ordinal), StringComparison.Ordinal) <
                    reservation.IndexOf("driverSeat.Occupant = local", StringComparison.Ordinal));
        Assert.DoesNotContain("driverSeat.Occupant != null", reservation);
        Assert.Contains("occupant.Pointer == local.Pointer", ride);
        Assert.Contains("if (!EnsurePassengerSeat(veh))", ride);
    }

    [Fact]
    public void SeatSelectionAndEveryRideCallerFailClosed()
    {
        string s = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string find = DriverSafetySourceContractTests.Section(s, "internal static VehicleSeat? FindSeat", "Per-frame passenger seat fix");
        Assert.Contains("PassengerSeatPolicy.Select", find);
        Assert.DoesNotContain("fallback", find);
        string ensure = DriverSafetySourceContractTests.Section(s, "internal static bool EnsurePassengerSeat", "private static string SafeDriverPlayerName");
        Assert.Contains("RefusePassengerRide(veh", ensure);
        Assert.True(ensure.IndexOf("passengerSeat == null", StringComparison.Ordinal) < ensure.IndexOf("driverSeat.Occupant = null", StringComparison.Ordinal));
        Assert.Contains("PassengerSeatPolicy.CanUse", ensure);
        Assert.Contains("driverOccupant.Pointer == local.Pointer", ensure);
        Assert.Contains("bool driverPresent = !ReferenceEquals(driver, null)", ensure);
        Assert.Contains("!moved.OccupantPresent", ensure);
        Assert.Contains("!ReferenceEquals(veh.DriverPlayer, null)", ensure);
        string start = DriverSafetySourceContractTests.Section(s, "internal static bool StartRide(string caller)", "internal static void EndRide");
        Assert.Contains("if (!EnsurePassengerSeat(veh))", start);
        Assert.True(start.IndexOf("if (!EnsurePassengerSeat(veh))", StringComparison.Ordinal) < start.IndexOf("if (SpikeState.RideActive)", StringComparison.Ordinal));
        string drive = DriverSafetySourceContractTests.Section(s, "private static bool StartRideDrive", "internal static bool Stop()");
        Assert.Contains("!EnsurePassengerSeat(veh)", drive);
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.Contains("if (!SpikeCommands.EnsurePassengerSeat(veh))", runner);
    }
}

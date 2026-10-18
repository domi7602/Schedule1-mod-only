using System;
using Xunit;
namespace TaxiDriver.Tests;
public class PassengerCleanupWiringTests
{
    [Fact]
    public void AllPlayerProofPrecedesBudgetAndNativeCall()
    {
        string s = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string c = DriverSafetySourceContractTests.Section(s, "internal static bool Cleanup(string reason", "internal static bool ReconcileDeadTaxi()");
        Assert.Contains("PassengerCleanupPolicy.TryBeginDestroy", c);
        Assert.Contains("ReadPlayerOccupancy(veh)", c);
        Assert.DoesNotContain("VehicleOwnership.TryBeginDestroy(", c);
        Assert.True(c.IndexOf("ReadPlayerOccupancy(veh)", StringComparison.Ordinal) < c.IndexOf("veh.DestroyVehicle()", StringComparison.Ordinal));
        Assert.Contains("Waiting for passengers", c);
        string app = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        Assert.Contains("SpikeState.ServiceStatusRevision != statusBeforeStop", app);
        Assert.Contains("SetStatusOverride(SpikeState.ServiceStatus)", app);
        string read = DriverSafetySourceContractTests.Section(s, "private static PlayerOccupancy ReadPlayerOccupancy", "private static bool ClearNpcOccupants");
        Assert.Contains("veh.Seats", read);
        Assert.Contains("veh.OccupantPlayers", read);
        Assert.Contains("Player.PlayerList", read);
        Assert.Contains("player.CurrentVehicle", read);
        Assert.Contains("PassengerCleanupPolicy.ObserveCurrentVehicle", read);
        Assert.Contains("current!.Pointer : IntPtr.Zero, networkObject.Pointer", read);
        Assert.Contains("veh.IsOccupied", read);
        Assert.Contains("PlayerOccupancy.Unknown", read);
        Assert.DoesNotContain("ExitVehicle(", read);
    }
}

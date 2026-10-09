using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public sealed class RideProtectionPolicyTests
{
    [Fact]
    public void NoOrPartialPatchSet_IsNeverCompleteProtection()
    {
        Assert.False(RideProtectionPolicy.IsComplete(RideProtectionPatch.None));
        Assert.False(RideProtectionPolicy.IsComplete(RideProtectionPatch.Throttle));
        Assert.False(RideProtectionPolicy.IsComplete(
            RideProtectionPatch.Throttle | RideProtectionPatch.Steering | RideProtectionPatch.Handbrake));
    }

    [Fact]
    public void EveryRequiredPatchMustBePresent()
    {
        RideProtectionPatch[] required =
        {
            RideProtectionPatch.Throttle,
            RideProtectionPatch.Steering,
            RideProtectionPatch.Handbrake,
            RideProtectionPatch.TrunkOpen,
            RideProtectionPatch.TrunkSetOpen,
        };

        foreach (RideProtectionPatch missing in required)
            Assert.False(RideProtectionPolicy.IsComplete(RideProtectionPolicy.Required & ~missing));
    }

    [Fact]
    public void CompleteProtectionRequiresAllDrivingAndTrunkGates()
    {
        Assert.True(RideProtectionPolicy.IsComplete(RideProtectionPolicy.Required));
        Assert.Equal(RideProtectionPolicy.Required,
            RideProtectionPolicy.Missing(RideProtectionPatch.None));
        Assert.Equal(RideProtectionPatch.None,
            RideProtectionPolicy.Missing(RideProtectionPolicy.Required));
    }

    [Fact]
    public void NoPassengerRide_DoesNotBlockVehicleInput()
    {
        Assert.False(RideProtectionPolicy.ShouldBlockPlayerInput(
            passengerRideMode: false,
            ownedRideVehicleVerified: false,
            overrideControls: false));
    }

    [Fact]
    public void PassengerRide_FailsClosedWhenVehicleIdentityIsUnverified()
    {
        Assert.True(RideProtectionPolicy.ShouldBlockPlayerInput(
            passengerRideMode: true,
            ownedRideVehicleVerified: false,
            overrideControls: false));
    }

    [Fact]
    public void VerifiedRide_BlocksPlayerInputButLeavesAgentOverrideUntouched()
    {
        Assert.True(RideProtectionPolicy.ShouldBlockPlayerInput(
            passengerRideMode: true,
            ownedRideVehicleVerified: true,
            overrideControls: false));
        Assert.False(RideProtectionPolicy.ShouldBlockPlayerInput(
            passengerRideMode: true,
            ownedRideVehicleVerified: true,
            overrideControls: true));
    }

    [Fact]
    public void NoPassengerRide_DoesNotBlockHandbrake()
    {
        Assert.False(RideProtectionPolicy.ShouldBlockHandbrake(passengerRideMode: false));
    }

    [Fact]
    public void PassengerRide_BlocksHandbrakeEvenWhenAgentOverrideIsActive()
    {
        Assert.True(RideProtectionPolicy.ShouldBlockHandbrake(passengerRideMode: true));
    }

    [Fact]
    public void NoPassengerRide_DoesNotBlockTrunkOpen()
    {
        Assert.False(RideProtectionPolicy.ShouldBlockTrunkOpen(
            passengerRideMode: false,
            ownedTrunkVerified: false,
            doorIdentityVerified: false,
            isRideTrunk: false));
    }

    [Fact]
    public void PassengerRide_BlocksTheRideTrunkAndUnverifiedDoorIdentity()
    {
        Assert.True(RideProtectionPolicy.ShouldBlockTrunkOpen(
            passengerRideMode: true,
            ownedTrunkVerified: true,
            doorIdentityVerified: true,
            isRideTrunk: true));
        Assert.True(RideProtectionPolicy.ShouldBlockTrunkOpen(
            passengerRideMode: true,
            ownedTrunkVerified: false,
            doorIdentityVerified: false,
            isRideTrunk: false));
    }

    [Fact]
    public void PassengerRide_AllowsOtherVerifiedStorageDoors()
    {
        Assert.False(RideProtectionPolicy.ShouldBlockTrunkOpen(
            passengerRideMode: true,
            ownedTrunkVerified: true,
            doorIdentityVerified: true,
            isRideTrunk: false));
    }
}

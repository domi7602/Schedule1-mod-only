using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Requests, not derived flags; physics acceptance remains open.</summary>
public class BrakeHoldPolicyTests
{
    [Fact]
    public void IntendedPark_RequestsHandbrakeEveryConsumption()
        => Assert.True(BrakeHoldPolicy.ShouldRequestPark(false, false, true));

    [Fact]
    public void ActiveOrder_IsNeverFought()
        => Assert.False(BrakeHoldPolicy.ShouldRequestPark(true, false, true));

    [Fact]
    public void PlayerDriver_IsUntouched()
        => Assert.False(BrakeHoldPolicy.ShouldRequestPark(false, true, true));

    [Fact]
    public void NoParkIntent_DoesNotTakeControl()
        => Assert.False(BrakeHoldPolicy.ShouldRequestPark(false, false, false));
}

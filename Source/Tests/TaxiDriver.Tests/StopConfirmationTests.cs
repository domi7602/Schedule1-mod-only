using Xunit;

namespace TaxiDriver.Tests;

public class StopConfirmationTests
{
    [Fact]
    public void TimeMovesBackwards_RequiresANewFirstPress()
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(true, 1, 100f));
        Assert.False(confirmation.Press(true, 1, 99f));
    }

    [Fact]
    public void ExpiredWindow_RequiresANewFirstPress()
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(true, 1, 100f));
        Assert.False(confirmation.Press(true, 1, 103f));
        Assert.True(confirmation.Press(true, 1, 104f));
    }

    [Fact]
    public void NoActiveRide_StopsWithoutConfirmation()
    {
        Assert.True(new StopConfirmation().Press(false, 0, 100f));
    }

    [Fact]
    public void SameVisibleRide_KeepsTheConfirmation()
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(true, 1, 100f));
        Assert.False(confirmation.Synchronize(true, true, 1));
        Assert.True(confirmation.Press(true, 1, 101f));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(-1f)]
    public void InvalidTime_NeverConfirmsDestruction(float now)
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(true, 1, 100f));
        Assert.False(confirmation.Press(true, 1, now));
        Assert.False(confirmation.Press(true, 1, 101f));
    }

    [Fact]
    public void NewRide_CannotUseThePreviousRidesConfirmation()
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(true, 1, 100f));
        Assert.False(confirmation.Press(true, 2, 101f));
        Assert.True(confirmation.Press(true, 2, 102f));
    }

    [Fact]
    public void RideEnds_DiscardsTheArmedConfirmation()
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(true, 1, 100f));
        Assert.True(confirmation.Synchronize(true, false, 1));
        Assert.False(confirmation.Press(true, 2, 101f));
    }

    [Fact]
    public void PhoneOrAppHidden_DiscardsTheArmedConfirmation()
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(true, 1, 100f));
        Assert.True(confirmation.Synchronize(appOpen: false, rideActive: true, rideId: 1));
        Assert.False(confirmation.Synchronize(appOpen: true, rideActive: true, rideId: 1));
        Assert.False(confirmation.Press(true, 1, 101f));
    }

    [Fact]
    public void ActiveRide_RequiresTwoPressesWithinThreeSeconds()
    {
        var confirmation = new StopConfirmation();
        Assert.False(confirmation.Press(rideActive: true, rideId: 1, now: 100f));
        Assert.True(confirmation.Press(rideActive: true, rideId: 1, now: 102f));
        Assert.False(confirmation.Press(rideActive: true, rideId: 1, now: 102f));
    }
}

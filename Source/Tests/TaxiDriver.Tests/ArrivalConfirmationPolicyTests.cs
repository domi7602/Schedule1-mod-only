using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Executable regressions for the arrival verdict (brief §8.2): proximity alone, a
/// failed speed read or an active reverse must not complete a taxi; a valid negative
/// speed is reverse motion, not a standstill.
/// </summary>
public class ArrivalConfirmationPolicyTests
{
    [Fact]
    public void FailedSpeedRead_NeverConfirmsAHalt()
    {
        // Review D8: readability is a SEPARATE flag from the value. A finite sample — including the
        // valid -1 km/h a real reverse can produce — is readable; only the reader's own success
        // flag says the read worked. The old "-1 means the read failed" sentinel is gone.
        Assert.True(NavigationArrivalPolicy.IsReadableSpeed(-1f));
        Assert.False(NavigationArrivalPolicy.IsConfirmedHalt(false, -1f, false)); // read failed -> never a halt
        Assert.False(NavigationArrivalPolicy.IsConfirmedArrival(2f, false, -1f, false));
        // A readable -1 km/h is inside the standing band and, with no reverse, is a genuine halt.
        Assert.True(NavigationArrivalPolicy.IsConfirmedHalt(true, -1f, false));
    }

    [Fact]
    public void NonFiniteSpeed_IsNotReadable()
    {
        Assert.False(NavigationArrivalPolicy.IsReadableSpeed(float.NaN));
        Assert.False(NavigationArrivalPolicy.IsReadableSpeed(float.PositiveInfinity));
        Assert.False(NavigationArrivalPolicy.IsConfirmedArrival(2f, true, float.NaN, false));
    }

    [Theory]
    [InlineData(0f, false, true)]
    [InlineData(0.5f, false, true)]
    [InlineData(-0.5f, false, true)] // a valid small negative speed can be a halt
    [InlineData(-4f, false, false)]  // a valid large negative speed is reverse motion
    [InlineData(4f, false, false)]   // still moving forward
    [InlineData(0f, true, false)]    // actively reversing -> never a halt
    public void ConfirmedHalt_UsesReadableValueAndReverseState(float speed, bool reversing, bool expected)
    {
        Assert.Equal(expected, NavigationArrivalPolicy.IsConfirmedHalt(true, speed, reversing));
    }

    [Fact]
    public void ProximityWhileMovingOrReversing_IsNotArrival()
    {
        Assert.True(NavigationArrivalPolicy.IsWithinArrival(3f));
        Assert.False(NavigationArrivalPolicy.IsConfirmedArrival(3f, true, 12f, false)); // driving in the radius
        Assert.False(NavigationArrivalPolicy.IsConfirmedArrival(3f, true, -3f, true));  // reversing in the radius
        Assert.True(NavigationArrivalPolicy.IsConfirmedArrival(3f, true, 0.2f, false));
    }

    [Fact]
    public void OutOfRadius_IsNeverArrivalEvenWhenStopped()
    {
        Assert.False(NavigationArrivalPolicy.IsConfirmedArrival(25f, true, 0f, false));
    }
}

/// <summary>
/// A fresh, DIFFERENT nearby target must drive; only the exact same completed logical
/// target with a readable confirmed stop may be skipped (brief §8.2).
/// </summary>
public class RideSatisfactionPolicyTests
{
    [Fact]
    public void DifferentNearbyTarget_IsNotSatisfiedByAPriorParkedState()
    {
        Assert.False(RideSatisfactionPolicy.IsAlreadySatisfied(
            sameCompletedTarget: false, callbackComplete: true, gaveUp: false,
            distance: 3f, speedReadable: true, signedSpeed: 0f, reversing: false));
    }

    [Fact]
    public void SameCompletedTargetWithConfirmedStop_IsSatisfied()
    {
        Assert.True(RideSatisfactionPolicy.IsAlreadySatisfied(
            sameCompletedTarget: true, callbackComplete: true, gaveUp: false,
            distance: 3f, speedReadable: true, signedSpeed: 0f, reversing: false));
    }

    [Fact]
    public void SameTargetStillMovingOrAfterGiveUp_IsNotSatisfied()
    {
        Assert.False(RideSatisfactionPolicy.IsAlreadySatisfied(
            true, true, false, 3f, true, 12f, false)); // still moving
        Assert.False(RideSatisfactionPolicy.IsAlreadySatisfied(
            true, true, false, 3f, false, -1f, false)); // unreadable speed
        Assert.False(RideSatisfactionPolicy.IsAlreadySatisfied(
            true, true, true, 3f, true, 0f, false)); // a give-up is never satisfied
        Assert.False(RideSatisfactionPolicy.IsAlreadySatisfied(
            true, false, false, 3f, true, 0f, false)); // no completed callback
    }
}

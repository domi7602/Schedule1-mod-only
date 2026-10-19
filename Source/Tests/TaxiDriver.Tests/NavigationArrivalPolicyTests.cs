using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class NavigationArrivalPolicyTests
{
    [Theory]
    [InlineData(9f, 0f)]
    [InlineData(9f, 12f)]
    [InlineData(9f, -12f)]
    [InlineData(15f, 0f)]
    [InlineData(15f, 12f)]
    [InlineData(15f, -12f)]
    [InlineData(24f, 0f)]
    [InlineData(24f, 12f)]
    [InlineData(24f, -12f)]
    [InlineData(26f, 0f)]
    [InlineData(26f, 12f)]
    [InlineData(26f, -12f)]
    [InlineData(10f, 0f)]
    public void PatrolAndRide_UseTheSameTenMetreBoundary(float distance, float signedSpeed)
    {
        bool within = distance <= 10f;
        bool standing = NavigationArrivalPolicy.IsStandingAtArrival(true, distance, signedSpeed);
        Assert.Equal(within && System.Math.Abs(signedSpeed) <= 1f, standing);
        Assert.Equal(within, NavigationArrivalPolicy.IsWithinArrival(distance) || standing);
        Assert.Equal(within ? "arrived" : "stopped short",
            NavigationEndReason.ForRide(false, distance, NavigationArrivalPolicy.ArrivalMeters, "Complete"));
    }

    [Theory]
    [InlineData(-1f, true)]
    [InlineData(1f, true)]
    [InlineData(-12f, false)]
    [InlineData(12f, false)]
    public void Standing_UsesAbsoluteSignedSpeed(float speed, bool expected)
    {
        Assert.Equal(expected, NavigationArrivalPolicy.IsStandingAtArrival(true, 9f, speed));
        Assert.False(NavigationArrivalPolicy.IsStandingAtArrival(false, 9f, speed));
    }

    [Fact]
    public void RuntimePatrol_UsesSharedPolicyAndHasNoLooseArrival()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.Contains("ArrivalThresholdMeters = NavigationArrivalPolicy.ArrivalMeters", runner);
        Assert.Contains("NavigationArrivalPolicy.IsWithinArrival(distance)", runner);
        // Review D1: the single supervised arrival decision replaced the loose proximity check.
        Assert.Contains("SupervisedArrivalPolicy.IsConfirmedArrival(", runner);
        Assert.DoesNotContain("PatrolLooseArrivalMeters", runner);
    }
}

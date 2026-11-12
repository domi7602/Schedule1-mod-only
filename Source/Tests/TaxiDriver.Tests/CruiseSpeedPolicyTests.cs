using System.Reflection;
using Xunit;

namespace TaxiDriver.Tests;

public class CruiseSpeedPolicyTests
{
    [Theory]
    [InlineData(false, true, true, false, false, false)]
    [InlineData(true, false, true, false, false, false)]
    [InlineData(true, true, false, false, false, false)]
    [InlineData(true, true, true, true, false, false)]
    [InlineData(true, true, true, false, true, false)]
    [InlineData(true, true, true, false, false, true)]
    public void OutsideOwnedAutonomousForwardOrder_PreservesNativeTarget(
        bool owned, bool active, bool auto, bool player, bool reverse, bool nativeOverride)
    {
        Assert.Equal(29f, CruiseSpeedPolicy.BaseTargetKmh(29f, owned, active, auto, player, reverse, nativeOverride));
        Assert.Equal(50f, CruiseSpeedPolicy.BaseTargetKmh(50f, owned, active, auto, player, reverse, nativeOverride, 3.9f));
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-10f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void StoppedReverseOrInvalidNativeTarget_IsNeverPromoted(float nativeTarget)
    {
        Assert.Equal(nativeTarget, CruiseSpeedPolicy.BaseTargetKmh(nativeTarget, true, true, true, false, false, false));
    }

    [Theory]
    [InlineData(1f)]
    [InlineData(25f)]
    [InlineData(29f)]
    [InlineData(80f)]
    public void EligibleBaseTarget_IsBoundedAt50RegardlessOfRoadZone(float nativeTarget)
    {
        Assert.Equal(50f, CruiseSpeedPolicy.BaseTargetKmh(nativeTarget, true, true, true, false, false, false));
    }

    [Fact]
    public void RepeatedNativeResets_ReapplyWithoutAccumulatingSpeed()
    {
        float target = 29f;
        for (int i = 0; i < 100; i++)
        {
            target = CruiseSpeedPolicy.BaseTargetKmh(target, true, true, true, false, false, false);
            Assert.Equal(50f, target);
        }
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(3.9f)] // Native Complete in the recorded Bungalow overshoot.
    [InlineData(6.8f)]
    [InlineData(10f)]
    public void InsideArrivalRadius_UsesLowApproachSpeed(float distance)
    {
        Assert.Equal(6f, CruiseSpeedPolicy.BaseTargetKmh(29f, true, true, true, false, false, false, distance));
    }

    [Theory]
    [InlineData(20f, 20.6f, 20.7f)]
    [InlineData(40f, 34.6f, 34.7f)]
    [InlineData(60f, 44.4f, 44.5f)]
    [InlineData(75f, 50f, 50f)]
    [InlineData(200f, 50f, 50f)]
    public void ApproachSpeed_RisesWithBrakingDistanceUpToCruise(float distance, float lower, float upper)
    {
        Assert.InRange(CruiseSpeedPolicy.LimitApproachKmh(50f, distance), lower, upper);
    }

    [Theory]
    [InlineData(-1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void UnknownApproachDistance_PreservesCruise(float distance)
    {
        Assert.Equal(50f, CruiseSpeedPolicy.LimitApproachKmh(50f, distance));
    }

    [Theory]
    [InlineData(4f)]
    [InlineData(0f)]
    [InlineData(-7f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void ApproachLimit_NeverRaisesLowerOrInvalidInput(float targetKmh)
    {
        Assert.Equal(targetKmh, CruiseSpeedPolicy.LimitApproachKmh(targetKmh, 3.9f));
    }

    [Fact]
    public void OwnedActiveForwardTaxi_Uses50KmhBaseTarget()
    {
        // Reflection keeps the first RED executable before the pure source exists.
        var policy = typeof(CruiseSpeedPolicyTests).Assembly.GetType("TaxiDriver.CruiseSpeedPolicy");
        Assert.NotNull(policy);
        var method = policy.GetMethod("BaseTargetKmh", BindingFlags.Static | BindingFlags.NonPublic);
        Assert.NotNull(method);
        Assert.Equal(50f, method.Invoke(null, new object[] { 29f, true, true, true, false, false, false, float.NaN }));
    }
}

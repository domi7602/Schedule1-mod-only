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

    [Fact]
    public void OwnedActiveForwardTaxi_Uses50KmhBaseTarget()
    {
        // Reflection keeps the first RED executable before the pure source exists.
        var policy = typeof(CruiseSpeedPolicyTests).Assembly.GetType("TaxiDriver.CruiseSpeedPolicy");
        Assert.NotNull(policy);
        var method = policy.GetMethod("BaseTargetKmh", BindingFlags.Static | BindingFlags.NonPublic);
        Assert.NotNull(method);
        Assert.Equal(50f, method.Invoke(null, new object[] { 29f, true, true, true, false, false, false }));
    }
}

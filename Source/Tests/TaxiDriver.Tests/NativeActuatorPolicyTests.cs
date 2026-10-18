using System;
using Xunit;

namespace TaxiDriver.Tests;

public class NativeActuatorPolicyTests
{
    [Fact]
    public void ParkedPassenger_RequestsParkButDispatchAndCleanupReleaseIt()
    {
        Assert.Equal(NativeActuatorMode.Park,
            NativeActuatorPolicy.Choose(true, true, false, false, false, false, true));
        Assert.Equal(NativeActuatorMode.None,
            NativeActuatorPolicy.Choose(true, true, false, false, false, true, true));
        Assert.Equal(NativeActuatorMode.None,
            NativeActuatorPolicy.Choose(true, true, false, false, false, false, false));
        Assert.Equal(NativeActuatorMode.None,
            NativeActuatorPolicy.Choose(true, false, false, false, false, false, true));
        Assert.Equal(NativeActuatorMode.Reverse,
            NativeActuatorPolicy.Choose(true, true, false, false, true, true, true));
    }

    [Theory]
    [InlineData(false, true, false, false)]
    [InlineData(true, false, false, false)]
    [InlineData(true, true, true, false)]
    [InlineData(true, true, false, true)]
    public void UnsafeOwnershipHostPauseOrPlayerDriver_IsUntouched(bool host, bool owned, bool paused, bool player)
    {
        Assert.Equal(NativeActuatorMode.None,
            NativeActuatorPolicy.Choose(host, owned, paused, player, true, true, true));
    }

    [Fact]
    public void OwnedSupervisedWindow_SelectsReverseAtConsumer()
    {
        Assert.Equal(NativeActuatorMode.Reverse,
            NativeActuatorPolicy.Choose(true, true, false, false, true, true, false));
    }
}

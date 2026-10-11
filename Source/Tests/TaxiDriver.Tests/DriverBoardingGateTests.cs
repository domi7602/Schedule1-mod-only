using System;
using Xunit;

namespace TaxiDriver.Tests;

public class DriverBoardingGateTests
{
    [Fact]
    public void AttemptedEntry_BlocksTheSameNativeNpcForFifteenSeconds()
    {
        var gate = new DriverBoardingGate();
        var own = new IntPtr(1);
        Assert.True(gate.TryBeginEntry(own, own, 100f));
        Assert.False(gate.TryBeginEntry(new IntPtr(1), own, 100f));
        Assert.False(gate.TryBeginEntry(own, own, 114.99f));
        Assert.True(gate.TryBeginEntry(own, own, 115f));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(-1f)]
    public void InvalidTime_FailsClosedWithoutPoisoningTheNextAttempt(float now)
    {
        var gate = new DriverBoardingGate();
        var own = new IntPtr(1);
        Assert.False(gate.TryBeginEntry(own, own, now));
        Assert.True(gate.TryBeginEntry(own, own, 100f));
    }

    [Fact]
    public void RejectedAttempts_DoNotExtendTheExistingCooldown()
    {
        var gate = new DriverBoardingGate();
        var own = new IntPtr(1);
        Assert.True(gate.TryBeginEntry(own, own, 100f));
        Assert.False(gate.TryBeginEntry(own, own, 114f));
        Assert.Equal(1f, gate.RemainingSeconds(own, 114f));
        Assert.True(gate.TryBeginEntry(own, own, 115f));
    }

    [Fact]
    public void MissingIdentity_DoesNotConsumeAnEntryAttempt()
    {
        var gate = new DriverBoardingGate();
        Assert.False(gate.TryBeginEntry(IntPtr.Zero, new IntPtr(1), 100f));
        Assert.False(gate.TryBeginEntry(new IntPtr(1), IntPtr.Zero, 100f));
        Assert.True(gate.TryBeginEntry(new IntPtr(1), new IntPtr(1), 100f));
    }

    [Fact]
    public void ForeignNpc_CannotBeginAnEntry()
    {
        var gate = new DriverBoardingGate();
        Assert.False(gate.TryBeginEntry(new IntPtr(2), new IntPtr(1), 100f));
        Assert.True(gate.TryBeginEntry(new IntPtr(1), new IntPtr(1), 100f));
    }
}

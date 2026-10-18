using System;
using System.Collections.Generic;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class DeadTaxiCleanupTests
{
    [Fact]
    public void Cleanup_InvalidatesFirstAndStopsMeterBeforeRuntimeReset()
    {
        var steps = new List<string>();
        Assert.True(DeadTaxiCleanup.Run(
            () => steps.Add("invalidate"), () => steps.Add("meter"),
            () => steps.Add("trunk"), () => steps.Add("patrol"),
            () => steps.Add("runtime"), (_, _) => Assert.Fail("Unexpected failure")));
        Assert.Equal(new[] { "invalidate", "meter", "trunk", "patrol", "runtime" }, steps);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ThrowingStep_DoesNotSkipLaterCleanupAndRetainsRetryOwnership(int throwingStep)
    {
        var steps = new List<int>();
        var ownership = new VehicleOwnershipLedger();
        var pointer = new IntPtr(123);
        ownership.TryRegister(pointer);
        int failures = 0;
        Action Step(int index) => () =>
        {
            steps.Add(index);
            if (index == throwingStep) throw new InvalidOperationException("native/config failure");
        };
        bool cleaned = DeadTaxiCleanup.Run(Step(0), Step(1), Step(2), Step(3), Step(4), (_, _) => failures++);
        if (cleaned) ownership.ClearAfterConfirmedDeath(pointer, false);
        Assert.False(cleaned);
        Assert.Equal(new[] { 0, 1, 2, 3, 4 }, steps);
        Assert.Equal(1, failures);
        Assert.Equal(pointer, ownership.TrackedPointer);
    }

    [Fact]
    public void ThrowingReporter_DoesNotSkipTheRemainingSteps()
    {
        int completed = 0;
        Assert.False(DeadTaxiCleanup.Run(() => { },
            () => throw new Exception("meter"), () => completed++, () => completed++, () => completed++,
            (_, _) => throw new Exception("logger")));
        Assert.Equal(3, completed);
    }

    [Fact]
    public void Retry_DoesNotRepeatConsumedMeterSettlement()
    {
        bool active = true;
        int stops = 0;
        Action meter = () =>
        {
            if (!active) return;
            active = false; // FareMeter.Stop commits this before config/reporting IO.
            stops++;
            throw new Exception("report/config");
        };
        Assert.False(DeadTaxiCleanup.Run(() => { }, meter, () => { }, () => { }, () => { }, (_, _) => { }));
        Assert.True(DeadTaxiCleanup.Run(() => { }, meter, () => { }, () => { }, () => { }, (_, _) => { }));
        Assert.Equal(1, stops);
    }
}

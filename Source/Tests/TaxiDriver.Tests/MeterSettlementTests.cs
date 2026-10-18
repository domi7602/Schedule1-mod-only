using System;
using Xunit;

namespace TaxiDriver.Tests;

public class MeterSettlementTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void StopClaimsRunningMeterBeforeCallbackAndNeverRetriesUnknownOutcome(bool throws)
    {
        bool running = true;
        int calls = 0;
        Action report = () =>
        {
            calls++;
            if (throws) throw new InvalidOperationException("notification/config failed");
        };

        if (throws)
            Assert.Throws<InvalidOperationException>(() => MeterSettlement.Stop(ref running, report));
        else
            MeterSettlement.Stop(ref running, report);

        // The claim is consumed before any report IO, so an unknown/failed report
        // cannot be retried, re-notified or resubmitted by a later cleanup pass.
        Assert.False(running);
        MeterSettlement.Stop(ref running, () => calls++);
        Assert.Equal(1, calls);

        // A genuinely new ride owns a fresh meter and settles exactly once again.
        running = true;
        MeterSettlement.Stop(ref running, () => calls++);
        Assert.False(running);
        Assert.Equal(2, calls);
    }

    [Fact]
    public void StopOnAnAlreadyIdleMeterRunsNoReport()
    {
        bool running = false;
        int calls = 0;
        MeterSettlement.Stop(ref running, () => calls++);
        Assert.False(running);
        Assert.Equal(0, calls);
    }
}

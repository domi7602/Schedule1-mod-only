using System;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Regression tests for the numeric hardening (2026-10-03: validate before state
/// mutation, no partial ledger progression on failure). Before the fix every
/// test here was skipped (each failed against the FareLedger/FareClock behavior
/// by design); they are active since the fix landed. The former quirk pins
/// (FareNumericCharacterizationTests: overflowing checked() threw every tick and
/// left billed minutes stale, NaN poisoned the accumulator, a non-finite rate
/// result was accepted) were removed together with the fixed behavior.
/// </summary>
public class FareNumericTargetTests
{
    private static FareLedger.TickInput Moving(
        float delta = 0.2f,
        float speed = 30f,
        float clockRate = 1f,
        int rate = 1) => new()
        {
            RideValid = true,
            RawDeltaSeconds = delta,
            ClockAvailable = true,
            ClockRate = clockRate,
            ConfigEnabled = true,
            DollarsPerMinute = rate,
            SpeedReadFailed = false,
            SpeedKmh = speed,
            MovingThresholdKmh = 3f,
        };

    [Fact]
    public void NT01_MaxValidInputs_SaturateWithoutOverflowOrRepeatedBilling()
    {
        // The accepted maximum rate and largest finite clock rate must saturate
        // at the fare cap. The next tick must not issue the same payment again.
        var ledger = new FareLedger();
        FareLedger.TickInput extreme = Moving(
            delta: 0.5f,
            clockRate: float.MaxValue,
            rate: FareConfigRules.MaximumDollarsPerMinute);

        ledger.Tick(extreme); // first moving frame only arms the meter
        FareLedger.TickOutcome second = ledger.Tick(extreme);

        Assert.True(second.Counted);
        Assert.Equal(16_000_000, second.DueDollars);
        Assert.Equal((int)Math.Floor(ledger.MovingMinutes), ledger.BilledMinutes);

        FareLedger.TickOutcome third = ledger.Tick(extreme);
        Assert.True(third.Counted);
        Assert.Equal(0, third.DueDollars);
        Assert.Equal(ledger.BilledMinutes, third.WholeMinutes);
    }

    [Fact]
    public void NT02_NonFiniteRawDelta_IsRejected_NoStateChange()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving()); // arming
        ledger.Tick(Moving()); // counted, 0.2 minutes

        FareLedger.TickOutcome bad = ledger.Tick(Moving(delta: float.NaN));

        Assert.False(bad.Counted);
        Assert.Equal(0, bad.DueDollars);
        Assert.Equal(0.2d, ledger.MovingMinutes, 3); // unchanged
        Assert.Equal(0, ledger.BilledMinutes);
    }

    [Fact]
    public void NT03_NonFiniteClockRate_IsRejected_NoStateChange()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving()); // arming
        ledger.Tick(Moving()); // counted, 0.2 minutes

        FareLedger.TickOutcome bad = ledger.Tick(Moving(clockRate: float.NaN));

        Assert.False(bad.Counted);
        Assert.Equal(0, bad.DueDollars);
        Assert.Equal(0.2d, ledger.MovingMinutes, 3); // unchanged
    }

    [Fact]
    public void NT04_TryComputeRate_RejectsANonFiniteResult()
    {
        Assert.False(FareClock.TryComputeRate(1e-30f, 1e10f, out float rate));
        Assert.Equal(0f, rate);
    }
}

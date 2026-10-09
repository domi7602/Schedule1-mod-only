using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public sealed class FareNumericSafetyTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(17)]
    public void ValidDollarRate_IsKept(int value)
    {
        int normalized = FareConfigRules.NormalizeDollarsPerMinute(value, out bool changed);

        Assert.Equal(value, normalized);
        Assert.False(changed);
    }

    [Fact]
    public void MaximumSafeDollarRate_IsKept()
    {
        int normalized = FareConfigRules.NormalizeDollarsPerMinute(
            FareConfigRules.MaximumDollarsPerMinute, out bool changed);

        Assert.Equal(FareConfigRules.MaximumDollarsPerMinute, normalized);
        Assert.False(changed);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NonPositiveDollarRate_ResetsToDefault(int value)
    {
        int normalized = FareConfigRules.NormalizeDollarsPerMinute(value, out bool changed);

        Assert.Equal(FareConfigRules.DefaultDollarsPerMinute, normalized);
        Assert.True(changed);
    }

    [Fact]
    public void RateAboveMaximum_ResetsToDefault()
    {
        int normalized = FareConfigRules.NormalizeDollarsPerMinute(
            FareConfigRules.MaximumDollarsPerMinute + 1, out bool changed);

        Assert.Equal(FareConfigRules.DefaultDollarsPerMinute, normalized);
        Assert.True(changed);
    }

    [Fact]
    public void HugeClockJump_SaturatesAtMaximumFareWithoutOverflowOrRepeatCharge()
    {
        var ledger = new FareLedger();
        var input = new FareLedger.TickInput
        {
            RideValid = true,
            RawDeltaSeconds = 0.5f,
            ClockAvailable = true,
            ClockRate = float.MaxValue,
            ConfigEnabled = true,
            DollarsPerMinute = FareConfigRules.MaximumDollarsPerMinute,
            SpeedKmh = 10f,
            MovingThresholdKmh = 3f,
        };

        Assert.True(ledger.Tick(input).ArmedNow);
        FareLedger.TickOutcome charged = ledger.Tick(input);
        int maxWholeMinutes = FareConfigRules.MaximumFareDollars / FareConfigRules.MaximumDollarsPerMinute;
        int expectedMaxCharge = maxWholeMinutes * FareConfigRules.MaximumDollarsPerMinute;

        Assert.True(charged.Counted);
        Assert.Equal(expectedMaxCharge, charged.DueDollars);
        Assert.Equal(maxWholeMinutes, charged.WholeMinutes);
        Assert.Equal(maxWholeMinutes, ledger.BilledMinutes);
        Assert.Equal(0, ledger.Tick(input).DueDollars);
    }
}

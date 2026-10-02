using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Pins the pure game-clock conversion (TimeManager CycleDuration /
/// TimeSpeedMultiplier → in-game minutes per scaled second).
/// </summary>
public class FareClockTests
{
    [Fact]
    public void TwentyFourMinuteDay_AtNormalSpeed_IsOneGameMinutePerSecond()
    {
        Assert.True(FareClock.TryComputeRate(24f, 1f, out float rate));
        Assert.Equal(1f, rate, 3);
    }

    [Fact]
    public void SpeedMultiplier_ScalesTheRate()
    {
        Assert.True(FareClock.TryComputeRate(24f, 2f, out float rate));
        Assert.Equal(2f, rate, 3);
    }

    [Theory]
    [InlineData(0f, 1f)]
    [InlineData(-1f, 1f)]
    [InlineData(float.NaN, 1f)]
    [InlineData(float.PositiveInfinity, 1f)]
    [InlineData(24f, 0f)]
    [InlineData(24f, -1f)]
    [InlineData(24f, float.NaN)]
    [InlineData(24f, float.PositiveInfinity)]
    public void InvalidCycleOrSpeed_ReturnsFalse(float cycle, float speed)
    {
        Assert.False(FareClock.TryComputeRate(cycle, speed, out float rate));
        Assert.Equal(0f, rate);
    }
}

/// <summary>Pins the fare.json threshold validation/migration branches.</summary>
public class FareConfigRulesTests
{
    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(0f)]
    [InlineData(0.05f)]
    public void DegenerateThresholds_AreInvalid(float value)
    {
        Assert.True(FareConfigRules.IsInvalidThreshold(value));
    }

    [Theory]
    [InlineData(0.1f)]
    [InlineData(0.6f)]
    [InlineData(3f)]
    public void ValidThresholds_AreNotInvalid(float value)
    {
        Assert.False(FareConfigRules.IsInvalidThreshold(value));
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.1f)]
    [InlineData(0.5f)]
    public void LegacyCreepThreshold_IsDetected(float value)
    {
        Assert.True(FareConfigRules.IsLegacyThreshold(value));
    }

    [Theory]
    [InlineData(0.6f)]
    [InlineData(3f)]
    public void CurrentThresholds_AreNotLegacy(float value)
    {
        Assert.False(FareConfigRules.IsLegacyThreshold(value));
    }

    [Fact]
    public void DefaultThreshold_IsThreeKmh()
    {
        Assert.Equal(3f, FareConfigRules.DefaultThresholdKmh);
    }
}

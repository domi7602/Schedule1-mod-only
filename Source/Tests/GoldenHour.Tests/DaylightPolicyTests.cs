using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class DaylightPolicyTests
{
    [Theory]
    [InlineData(600, 360, 1.0)]
    [InlineData(1200, 720, 0.0)]
    [InlineData(1800, 1080, 1.0)]
    [InlineData(2359, 1439, 0.0)]
    [InlineData(0, 0, 0.0)]
    [InlineData(1954, 1194, 0.0)]
    [InlineData(1715, 1035, 0.5)]
    [InlineData(59, 59, 0.0)]
    public void Native_HHMM_is_converted_before_weighting(int hhmm, int expectedMinutes, double weight)
    {
        Assert.True(DaylightPolicy.TryHhmmToMinutes(hhmm, out int minutes));
        Assert.Equal(expectedMinutes, minutes);
        Assert.Equal(weight, DaylightPolicy.Weight(minutes), 6);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(60)]
    [InlineData(660)]
    [InlineData(1260)]
    [InlineData(2399)]
    [InlineData(2400)]
    [InlineData(int.MaxValue)]
    public void Malformed_HHMM_never_becomes_a_warm_fallback(int hhmm)
    {
        Assert.False(DaylightPolicy.TryHhmmToMinutes(hhmm, out int minutes));
        Assert.Equal(-1, minutes);
        Assert.Equal(0.0, DaylightPolicy.Weight(minutes));
    }

    [Theory]
    [InlineData(12 * 60)]          // noon
    [InlineData(12 * 60 + 22)]     // the owner's 12:22 shot
    [InlineData(0)]                // midnight
    [InlineData(23 * 60 + 41)]     // deep night, measured GraphicsProbe sample
    [InlineData(16 * 60 + 30)]     // edge of the evening lobe
    [InlineData(-1)]
    [InlineData(1440)]
    public void Weight_is_zero_outside_the_golden_lobes(int minutes)
    {
        Assert.Equal(0.0, DaylightPolicy.Weight(minutes), 6);
    }

    [Theory]
    [InlineData(6 * 60)]
    [InlineData(18 * 60)]
    public void Weight_is_full_at_the_morning_and_evening_peaks(int minutes)
    {
        Assert.Equal(1.0, DaylightPolicy.Weight(minutes), 6);
    }

    [Fact]
    public void Weight_is_half_forty_five_minutes_off_the_evening_peak()
    {
        // 17:15 is 45 minutes before 18:00, halfway across the 90-minute lobe.
        Assert.Equal(0.5, DaylightPolicy.Weight(17 * 60 + 15), 6);
    }

    [Fact]
    public void Lobes_do_not_add_so_a_wide_mistake_cannot_paint_noon()
    {
        Assert.True(DaylightPolicy.Weight(12 * 60) <= 1.0);
        Assert.Equal(0.0, DaylightPolicy.Weight(12 * 60), 6);
    }
}

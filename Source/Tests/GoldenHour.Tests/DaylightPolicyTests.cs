using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class DaylightPolicyTests
{
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

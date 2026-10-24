using S1MCPServer.Utils;
using Xunit;

namespace S1MCP.Tests;

/// <summary>
/// The native <c>TimeManager.CurrentTime</c> is HHMM, not minutes since midnight. These tests
/// pin the conversion both ways plus the input validation the command handler relies on.
/// </summary>
public class TimeOfDayTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(30, 30)]
    [InlineData(740, 460)]
    [InlineData(1200, 720)]
    [InlineData(1140, 700)]
    [InlineData(1954, 1194)]
    [InlineData(2359, 1439)]
    public void HhmmConvertsToRealMinutes(int hhmm, int expectedMinutes)
    {
        Assert.True(TimeOfDay.TryHhmmToMinutes(hhmm, out int minutes));
        Assert.Equal(expectedMinutes, minutes);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2400)]
    [InlineData(2460)]
    [InlineData(1960)]
    [InlineData(9999)]
    public void InvalidHhmmIsRejected(int hhmm)
    {
        Assert.False(TimeOfDay.IsValidHhmm(hhmm));
        Assert.False(TimeOfDay.TryHhmmToMinutes(hhmm, out _));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(30, 30)]
    [InlineData(460, 740)]
    [InlineData(720, 1200)]
    [InlineData(1194, 1954)]
    [InlineData(1439, 2359)]
    public void MinutesConvertToHhmm(int minutes, int expectedHhmm)
    {
        Assert.True(TimeOfDay.TryMinutesToHhmm(minutes, out int hhmm));
        Assert.Equal(expectedHhmm, hhmm);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1440)]
    [InlineData(5000)]
    public void OutOfRangeMinutesAreRejected(int minutes)
    {
        Assert.False(TimeOfDay.TryMinutesToHhmm(minutes, out _));
    }

    [Fact]
    public void MinutesToHhmmWrapsAcrossMidnight()
    {
        Assert.Equal(2359, TimeOfDay.MinutesToHhmm(-1));
        Assert.Equal(0, TimeOfDay.MinutesToHhmm(1440));
        Assert.Equal(1200, TimeOfDay.MinutesToHhmm(2160));
    }

    [Theory]
    [InlineData(0.0, 0)]
    [InlineData(0.5, 30)]
    [InlineData(7.0, 700)]
    [InlineData(12.0, 1200)]
    [InlineData(19.9, 1954)]
    [InlineData(24.0, 0)]
    public void DecimalHoursConvertToHhmm(double hours, int expectedHhmm)
    {
        Assert.True(TimeOfDay.TryHoursToHhmm(hours, out int hhmm));
        Assert.Equal(expectedHhmm, hhmm);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(24.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void InvalidHoursAreRejected(double hours)
    {
        Assert.False(TimeOfDay.TryHoursToHhmm(hours, out _));
    }

    [Theory]
    [InlineData("1954", 1954)]
    [InlineData("19:54", 1954)]
    [InlineData("9:54", 954)]
    [InlineData("07:40", 740)]
    [InlineData(" 2359 ", 2359)]
    public void TextHhmmIsParsed(string text, int expected)
    {
        Assert.True(TimeOfDay.TryParseHhmmText(text, out int hhmm));
        Assert.Equal(expected, hhmm);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("1960")]
    [InlineData("24:00")]
    [InlineData("abc")]
    [InlineData("19:5x")]
    public void InvalidTextHhmmIsRejected(string? text)
    {
        Assert.False(TimeOfDay.TryParseHhmmText(text, out _));
    }

    [Fact]
    public void HhmmAndMinutesRoundTripForEveryValidValue()
    {
        for (int hhmm = 0; hhmm <= 2359; hhmm++)
        {
            if (!TimeOfDay.IsValidHhmm(hhmm))
                continue;

            Assert.True(TimeOfDay.TryHhmmToMinutes(hhmm, out int minutes));
            Assert.InRange(minutes, 0, 1439);
            Assert.True(TimeOfDay.TryMinutesToHhmm(minutes, out int back));
            Assert.Equal(hhmm, back);
        }
    }

    [Theory]
    [InlineData(1954, "19:54")]
    [InlineData(740, "07:40")]
    [InlineData(0, "00:00")]
    [InlineData(2359, "23:59")]
    public void DisplayUses24HourClock(int hhmm, string expected)
    {
        Assert.Equal(expected, TimeOfDay.Format(hhmm));
    }

    [Fact]
    public void DisplayDoesNotInventAValueForAMalformedClock()
    {
        Assert.Equal("2460", TimeOfDay.Format(2460));
    }
}

using BusinessIncome.Core;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>Centralized pure payout-window planner: fails closed, overflow-safe.</summary>
public class FinancialWindowPlannerTests
{
    [Fact]
    public void PastDay_AlwaysBypassesWindow()
    {
        var d = PayoutWindowPlanner.Plan(payoutHour: 18, currentTime24h: 0, timeKnown: false, isPastDay: true);
        Assert.True(d.InWindow);
    }

    [Fact]
    public void MidnightHour_AlwaysInWindow()
    {
        var d = PayoutWindowPlanner.Plan(0, currentTime24h: 0, timeKnown: false, isPastDay: false);
        Assert.True(d.InWindow);
    }

    [Theory]
    [InlineData(1800)] // start
    [InlineData(1859)] // end
    [InlineData(1830)] // middle
    public void InsideWindow_IsInWindow(int now)
    {
        Assert.True(PayoutWindowPlanner.Plan(18, now, true, false).InWindow);
    }

    [Theory]
    [InlineData(1759)]
    [InlineData(1900)]
    public void OutsideWindow_IsOut(int now)
    {
        Assert.False(PayoutWindowPlanner.Plan(18, now, true, false).InWindow);
    }

    [Fact]
    public void UnknownTime_FailsClosed()
    {
        Assert.False(PayoutWindowPlanner.Plan(18, 0, timeKnown: false, isPastDay: false).InWindow);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(24)]
    [InlineData(int.MaxValue)]
    public void InvalidHour_FailsClosed(int hour)
    {
        Assert.False(PayoutWindowPlanner.Plan(hour, 0, true, false).InWindow);
    }

    [Fact]
    public void Hour23_DoesNotOverflow()
    {
        // 23*100 = 2300, end 2359 - must never wrap negative.
        Assert.True(PayoutWindowPlanner.Plan(23, 2359, true, false).InWindow);
        Assert.False(PayoutWindowPlanner.Plan(23, 0, true, false).InWindow);
    }
}

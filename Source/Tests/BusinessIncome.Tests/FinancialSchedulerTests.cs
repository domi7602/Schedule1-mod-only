using System.Linq;
using BusinessIncome.Core;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>Catch-up planner: oldest-to-newest, capped, overflow-safe, fail-closed.</summary>
public class FinancialSchedulerTests
{
    [Fact]
    public void NothingToCatchUp_WhenCurrentNotAhead()
    {
        var plan = PayoutScheduler.PlanCatchup(lastPaidDay: 10, currentDay: 10, maxCatchupDays: 30, hasBusinesses: true, stateDurable: true);
        Assert.Empty(plan.Days);
    }

    [Fact]
    public void FullBacklog_OldestToNewest()
    {
        var plan = PayoutScheduler.PlanCatchup(3, 6, 30, true, true);
        Assert.Equal(new[] { 4, 5, 6 }, plan.Days.ToArray());
        Assert.False(plan.StoppedEarly);
    }

    [Fact]
    public void ExceedsCap_PaysOnlyLastCapDays_AndStopsEarly()
    {
        var plan = PayoutScheduler.PlanCatchup(0, 100, 5, true, true);
        Assert.Equal(new[] { 96, 97, 98, 99, 100 }, plan.Days.ToArray());
        Assert.True(plan.StoppedEarly);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-7, 1)]
    [InlineData(99999, 365)]
    public void CapIsClamped(int input, int expected)
    {
        Assert.Equal(expected, PayoutScheduler.ClampCap(input));
    }

    [Fact] // Defect 8: no businesses must still yield the durable terminal days, oldest first
    public void NoBusinesses_ReturnsTerminalDays_OldestFirst()
    {
        var plan = PayoutScheduler.PlanCatchup(0, 10, 30, hasBusinesses: false, stateDurable: true);
        Assert.Equal(Enumerable.Range(1, 10).ToArray(), plan.Days.ToArray());
        // Runtime must execute these days through the durable terminal commit path.
        Assert.False(plan.TerminalSkip);
    }

    [Fact] // Defect 8: a no-business backlog larger than the cap is capped too
    public void NoBusinesses_HugeBacklog_IsCapped()
    {
        var plan = PayoutScheduler.PlanCatchup(0, 1000, 5, hasBusinesses: false, stateDurable: true);
        Assert.Equal(new[] { 996, 997, 998, 999, 1000 }, plan.Days.ToArray());
        // Runtime must execute these days through the durable terminal commit path.
        Assert.False(plan.TerminalSkip);
    }

    [Fact] // Defect 8: a non-durable state still stops a no-business backlog
    public void NoBusinesses_NonDurableState_Stops()
    {
        var plan = PayoutScheduler.PlanCatchup(0, 10, 30, hasBusinesses: false, stateDurable: false);
        Assert.Empty(plan.Days);
        Assert.True(plan.StoppedEarly);
        Assert.False(plan.TerminalSkip);
    }

    [Fact]
    public void NonDurableState_StopsImmediately()
    {
        var plan = PayoutScheduler.PlanCatchup(0, 10, 30, true, stateDurable: false);
        Assert.Empty(plan.Days);
        Assert.True(plan.StoppedEarly);
    }

    [Fact]
    public void InvalidLastPaid_StopsImmediately()
    {
        var plan = PayoutScheduler.PlanCatchup(lastPaidDay: -5, currentDay: 10, maxCatchupDays: 30, hasBusinesses: true, stateDurable: true);
        Assert.Empty(plan.Days);
        Assert.True(plan.StoppedEarly);
    }

    [Fact]
    public void HugeCurrentDay_IsOverflowSafeAndCapped()
    {
        var plan = PayoutScheduler.PlanCatchup(0, int.MaxValue, 30, true, true);
        Assert.True(plan.Days.Count <= 31);
        Assert.Equal(int.MaxValue, plan.Days[^1]);
        Assert.All(plan.Days, d => Assert.True(d > 0));
    }

    [Fact]
    public void HugeBacklogWithMinCap_ProducesSingleDay()
    {
        var plan = PayoutScheduler.PlanCatchup(0, int.MaxValue, 1, true, true);
        Assert.Single(plan.Days);
        Assert.Equal(int.MaxValue, plan.Days[0]);
    }
}

using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class NavigationRecoveryBudgetTests
{
    [Fact]
    public void RecoveryEpisodes_StopAtTheCapEvenWhenEveryReverseSucceeds()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0f, 90f);
        Assert.True(budget.TrySpend(1f, 2));
        Assert.True(budget.TrySpend(2f, 2));
        Assert.False(budget.TrySpend(3f, 2));
    }

    [Fact]
    public void RepeatedDispatchesAndProgress_DoNotRenewTheDeadline()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(10f, 90f);
        Assert.True(budget.TrySpend(20f, 10));
        Assert.True(budget.TrySpend(80f, 10));
        Assert.False(budget.HasExpired(99f));
        Assert.True(budget.HasExpired(100f));
        Assert.False(budget.TrySpend(100f, 10));
    }

    [Fact]
    public void TerminalRescue_IsOneShotAndCannotRunAfterTheDeadline()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0f, 90f);
        Assert.True(budget.TryLastRescue(5f));
        Assert.False(budget.TryLastRescue(6f));
        budget.Begin(0f, 90f);
        Assert.False(budget.TryLastRescue(90f));
    }

    [Fact]
    public void NewOperation_ClearsOldAttemptsAndLastRescue()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0f, 90f);
        budget.TrySpend(1f, 2);
        budget.TrySpend(2f, 2);
        budget.TryLastRescue(3f);
        budget.Begin(200f, 90f);
        Assert.True(budget.TrySpend(201f, 2));
        Assert.True(budget.TryLastRescue(202f));
        Assert.False(budget.HasExpired(289f));
        Assert.True(budget.HasExpired(290f));
    }

    [Fact]
    public void Reset_DisarmsTheBudgetUntilANewOperation()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0f, 90f);
        budget.Reset();
        Assert.False(budget.HasExpired(100f));
        Assert.False(budget.TrySpend(1f, 2));
        Assert.False(budget.TryLastRescue(1f));
    }

    [Fact]
    public void Pause_PreservesRemainingTimeIncludingAnOperationStartedAtZero()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0f, 90f);
        budget.ShiftPause(5f);
        Assert.False(budget.HasExpired(94f));
        Assert.True(budget.HasExpired(95f));
    }
}

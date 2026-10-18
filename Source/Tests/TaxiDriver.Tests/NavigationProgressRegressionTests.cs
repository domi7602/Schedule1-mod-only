using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class NavigationProgressRegressionTests
{
    private static void Observe(NavigationRecoveryBudget budget, float now, float distance, bool eligible = true) =>
        budget.ObserveProgress(now, distance, eligible);

    [Fact]
    public void Runtime_ObservesBeforeExpiryAndExcludesArtificialPlacement()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.Contains("ObserveNavigationProgress();", runner);
        Assert.Contains("_operationProgressTarget = SpikeState.NavTarget;", runner);
        Assert.Contains("NavReDispatchAt <= 0f && !agent.IsReversing", runner);
        string road = DriverSafetySourceContractTests.ReadSource("RoadKeeper.cs");
        Assert.Contains("SpikeRunner.ObserveArtificialPlacement(position);", road);
    }

    [Fact]
    public void FailureReason_ReachesMeterAfterVerdict()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.DoesNotContain("FareMeter.Stop(\"arrived\")", runner);
        Assert.Contains("NavigationEndReason.ForRide", runner);
        Assert.Contains("FareMeter.Stop(endReason)", runner);
        Assert.DoesNotContain("the taxi did not move (", runner);
        Assert.Contains("NavigationRecovery.Attempts", runner);
    }

    [Theory]
    [InlineData(false, 82f, null, "stopped short")]
    [InlineData(true, 2f, null, "navigation gave up")]
    [InlineData(false, 2f, "Failed", "navigation failed")]
    [InlineData(false, 2f, "Stopped", "navigation stopped")]
    [InlineData(false, 2f, "Complete", "arrived")]
    public void MeterEndReason_IsNotArrivalOnFailure(bool gaveUp, float distance, string? callback, string expected)
    {
        string actual = NavigationEndReason.ForRide(gaveUp, distance, 10f, callback);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RepeatedRedispatchReverseAndOscillation_CannotRenewOrReplenishBudget()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        for (int t = 1; t <= 180; t++)
        {
            // Dispatch windows may restart; operation observations and budget do not.
            Observe(budget, t, t % 2 == 0 ? 250 : 249, t % 3 != 0);
            if (t <= 2) Assert.True(budget.TrySpend(t, 2));
            else Assert.False(budget.TrySpend(t, 2));
        }
        Assert.True(budget.HasExpired(90));
        Assert.Equal(2, budget.Attempts);
    }

    [Fact]
    public void ArtificialSnapAndForwardReturn_CannotFarmFreshApproach()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        for (int t = 1; t < 90; t++)
        {
            Observe(budget, t, 100, false); // snap toward goal
            Observe(budget, t, 250, false); // rescue back out
            Observe(budget, t, 100);        // forward retraces covered ground
        }
        Assert.True(budget.HasExpired(90));
    }

    [Fact]
    public void ReverseTowardGoal_CannotRenewEvenWhenItCoversNewGround()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        for (int t = 1; t < 90; t++) Observe(budget, t, 250 - t, false);
        Assert.True(budget.HasExpired(90));
    }

    [Fact]
    public void GenuineProgressThenPauseAndNewOperation_ResetOnlyAtOperationBoundary()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        Observe(budget, 80, 240);
        budget.ShiftPause(100);
        Assert.False(budget.HasExpired(269));
        Assert.True(budget.HasExpired(270));
        budget.Begin(300, 90);
        Observe(budget, 300, 50);
        Observe(budget, 350, 40);
        Assert.False(budget.HasExpired(439));
        Assert.True(budget.HasExpired(440));
    }

    [Fact]
    public void LateProgress_CannotReviveAnExpiredOperation()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        Observe(budget, 90, 100);
        Assert.True(budget.HasExpired(90));
    }

    [Fact]
    public void GenuineProgress_DoesNotReplenishRecoveryEpisodes()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        Assert.True(budget.TrySpend(1, 2));
        Assert.True(budget.TrySpend(2, 2));
        Observe(budget, 80, 100);
        Assert.False(budget.HasExpired(120));
        Assert.False(budget.TrySpend(120, 2));
        Assert.True(budget.TryLastRescue(120));
        Assert.False(budget.TryLastRescue(121));
    }

    [Theory]
    [InlineData(0.1f)] // RoadKeeper retreat after each genuine metre
    [InlineData(0f)]   // no-advance artificial observation
    [InlineData(5f)]   // reverse away from the goal
    [InlineData(-0.25f)] // artificial new closest approach between genuine metres
    public void FractionalGenuineProgress_SurvivesInterleavedArtificialObservations(float artificialOffset)
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        float closest = 250;
        for (int t = 1; t < 90; t++)
        {
            float genuineDistance = closest - 1f;
            Observe(budget, t, genuineDistance);
            Observe(budget, t, genuineDistance + artificialOffset, false);
            closest = System.Math.Min(genuineDistance, genuineDistance + artificialOffset);
        }
        Assert.False(budget.HasExpired(90));
        Assert.False(budget.HasExpired(177));
        Assert.True(budget.HasExpired(178)); // last two-metre genuine credit at 88s
    }

    [Fact]
    public void ArtificialClosestApproach_IsNotCreditedNowOrByTheNextEligibleSample()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 250);
        Observe(budget, 80, 249); // one metre of pending genuine credit
        Observe(budget, 81, 240, false);
        Assert.True(budget.HasExpired(90));
        Observe(budget, 82, 240); // eligible sample must not credit the artificial nine metres
        Assert.True(budget.HasExpired(90));
        Observe(budget, 83, 239); // second genuine metre completes the retained credit
        Assert.False(budget.HasExpired(90));
        Assert.False(budget.HasExpired(172));
        Assert.True(budget.HasExpired(173));
    }

    [Fact]
    public void BarnRoute_GenuineProgressBeyond90SecondsKeepsDriving()
    {
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0, 90);
        Observe(budget, 0, 251);
        for (int t = 1; t <= 150; t++)
        {
            Observe(budget, t, 251 - t);
            Assert.False(budget.HasExpired(t), $"Moving route terminated at {t}s.");
        }
    }
}

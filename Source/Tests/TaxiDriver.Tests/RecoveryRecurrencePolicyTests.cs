using Xunit;

namespace TaxiDriver.Tests;

public class RecoveryRecurrencePolicyTests
{
    private static RecoveryRecurrencePolicy Stalled()
    {
        var policy = new RecoveryRecurrencePolicy();
        policy.Begin(100f);
        Assert.False(policy.RegisterStall(0f, 0f));
        return policy;
    }

    [Theory]
    [InlineData(0f, true)]
    [InlineData(3.2f, true)]
    [InlineData(6f, true)]
    [InlineData(6.01f, false)]
    [InlineData(100f, false)]
    public void Recurrence_UsesExplicitSpatialRadius(float x, bool recurring)
    {
        Assert.Equal(recurring, Stalled().RegisterStall(x, 0f));
    }

    [Fact]
    public void RecurringAnchor_DoesNotWalkAcrossUnrelatedStallLocations()
    {
        var policy = Stalled();
        Assert.True(policy.RegisterStall(5f, 0f));
        Assert.False(policy.RegisterStall(10f, 0f));
        Assert.True(policy.RegisterStall(11f, 0f));
    }

    [Fact]
    public void ReverseAwayThenForwardToSameGround_DoesNotClearRecurrence()
    {
        var policy = Stalled();
        for (int i = 0; i < 8; i++)
        {
            Assert.False(policy.ObserveProgress(103.2f, false));
            Assert.False(policy.ObserveProgress(100f, true));
            Assert.True(policy.RegisterStall(3.2f, 0f));
        }
    }

    [Fact]
    public void ArtificialOrCoastingApproach_CannotBeCreditedOnNextNaturalSample()
    {
        var policy = Stalled();
        Assert.False(policy.ObserveProgress(80f, false));
        Assert.False(policy.ObserveProgress(80f, true));
        Assert.False(policy.ObserveProgress(77f, true));
        Assert.True(policy.RegisterStall(0f, 0f));
        Assert.True(policy.ObserveProgress(73f, true));
        Assert.False(policy.RegisterStall(0f, 0f));
    }

    [Fact]
    public void FractionalNaturalApproach_SurvivesArtificialNewGroundWithinSameStall()
    {
        var policy = Stalled();
        Assert.False(policy.ObserveProgress(98f, true));
        Assert.False(policy.ObserveProgress(90f, false));
        Assert.False(policy.ObserveProgress(90f, true));
        Assert.True(policy.ObserveProgress(88f, true));
        Assert.False(policy.RegisterStall(0f, 0f));
    }

    [Fact]
    public void NewOperation_ClearsPriorStallAndHighWater()
    {
        var policy = Stalled();
        policy.ObserveProgress(50f, false);
        policy.Begin(100f);
        Assert.False(policy.RegisterStall(0f, 0f));
        Assert.True(policy.ObserveProgress(96f, true));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(-1f)]
    public void InvalidApproach_DoesNotClearStall(float distance)
    {
        var policy = Stalled();
        Assert.False(policy.ObserveProgress(distance, true));
        Assert.True(policy.RegisterStall(0f, 0f));
    }

    [Fact]
    public void Recurrence_DoesNotRenewDeadlineOrRecoveryBudget()
    {
        var policy = Stalled();
        var budget = new NavigationRecoveryBudget();
        budget.Begin(0f, 90f);
        budget.ObserveProgress(0f, 100f, false);
        for (int i = 0; i < 4; i++)
        {
            Assert.True(budget.TrySpend(i * 10f, 4));
            Assert.True(policy.RegisterStall(3f, 0f));
            policy.ObserveProgress(100f, true);
            budget.ObserveProgress(i * 10f, 100f, true);
        }
        Assert.False(budget.TrySpend(40f, 4));
        Assert.True(budget.HasExpired(90f));
    }

    [Fact]
    public void PartialApproachBeforeStall_CannotMakeTinyPostReverseMotionAnEscape()
    {
        var policy = new RecoveryRecurrencePolicy();
        policy.Begin(100f);
        Assert.False(policy.ObserveProgress(96.1f, true));
        Assert.False(policy.RegisterStall(0f, 0f));
        Assert.False(policy.ObserveProgress(96f, false));
        Assert.False(policy.ObserveProgress(95.8f, true));
        Assert.True(policy.RegisterStall(3f, 0f));
    }

    [Fact]
    public void NewForwardApproach_EndsNearbyRecurrence()
    {
        var policy = new RecoveryRecurrencePolicy();
        policy.Begin(100f);
        Assert.False(policy.RegisterStall(0f, 0f));
        Assert.True(policy.ObserveProgress(96f, true));
        Assert.False(policy.RegisterStall(0f, 0f));
    }
}

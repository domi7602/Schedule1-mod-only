using System.Numerics;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Executable regressions for the explicit operation/terminal model (review D13), the single
/// supervised arrival decision (review D1) and the logical destination identity (review D1/§8.2).
/// The runtime uses exactly these policies; a native PollingActive flip must never publish an
/// intermediate terminal state and a fresh nearby target must never be treated as a completed one.
/// </summary>
public class NavigationOperationPolicyTests
{
    [Fact]
    public void RecoverableFailure_DoesNotPublishATerminalAndKeepsTheOperationAlive()
    {
        var op = new NavigationOperationPolicy();
        op.Begin();
        op.EnterRecovery();
        Assert.True(op.Active);
        Assert.True(op.InRecovery);
        Assert.False(op.Terminal);          // no intermediate published terminal on a recoverable path
        Assert.False(op.Failed);
        op.Recover();
        Assert.True(op.Active);
        Assert.False(op.InRecovery);
        Assert.False(op.Terminal);
    }

    [Fact]
    public void Terminal_IsExplicitAndIdempotent_NeverReopenedByALaterFlip()
    {
        var op = new NavigationOperationPolicy();
        op.Begin();
        op.Terminate("navigation complete", failed: false);
        Assert.True(op.Terminal);
        Assert.Equal("navigation complete", op.TerminalReason);
        Assert.False(op.Failed);
        Assert.False(op.Active);            // a terminal operation is not "active"
        op.Terminate("a later flip", failed: true);
        Assert.Equal("navigation complete", op.TerminalReason); // first verdict wins
        Assert.False(op.Failed);
    }

    [Fact]
    public void FailedTerminal_IsDistinctFromAControlledStop()
    {
        var op = new NavigationOperationPolicy();
        op.Begin();
        op.Terminate("navigation stopped unexpectedly", failed: true);
        Assert.True(op.Terminal);
        Assert.True(op.Failed);
    }

    [Fact]
    public void EachGenuineNewOrder_OpensANewGeneration_AndResetClearsEverything()
    {
        var op = new NavigationOperationPolicy();
        op.Begin();
        long first = op.Generation;
        op.Terminate("stop driving", failed: false);
        op.Begin();
        Assert.True(op.Generation > first);
        Assert.True(op.Active);
        Assert.False(op.Terminal);
        op.Reset();
        Assert.False(op.Active);
        Assert.False(op.Terminal);
        Assert.Equal(string.Empty, op.TerminalReason);
    }
}

/// <summary>Review D1: the single supervised arrival decision every runtime path shares.</summary>
public class SupervisedArrivalPolicyTests
{
    [Fact]
    public void FreshNearbyTargetWithNoCurrentOperationEvidence_IsNotAnArrival()
    {
        // The taxi happened to START inside the radius and is parked — but there is no current
        // operation evidence (no native Complete, no real drive start in THIS operation).
        Assert.False(SupervisedArrivalPolicy.IsConfirmedArrival(
            currentRunComplete: false, operationProgress: false,
            distance: 4f, speedReadable: true, signedSpeed: 0f, reversing: false));
    }

    [Fact]
    public void CurrentRunCompleteOrRealOperationProgress_AllowsTheArrival()
    {
        Assert.True(SupervisedArrivalPolicy.IsConfirmedArrival(
            currentRunComplete: true, operationProgress: false,
            distance: 4f, speedReadable: true, signedSpeed: 0f, reversing: false));
        Assert.True(SupervisedArrivalPolicy.IsConfirmedArrival(
            currentRunComplete: false, operationProgress: true,
            distance: 4f, speedReadable: true, signedSpeed: 0f, reversing: false));
    }

    [Fact]
    public void MovingReversingOrUnreadable_IsNeverAnArrival_EvenWithEvidence()
    {
        Assert.False(SupervisedArrivalPolicy.IsConfirmedArrival(true, true, 4f, true, 12f, false));  // moving
        Assert.False(SupervisedArrivalPolicy.IsConfirmedArrival(true, true, 4f, true, 0f, true));    // reversing
        Assert.False(SupervisedArrivalPolicy.IsConfirmedArrival(true, true, 4f, false, -1f, false)); // unreadable read
        Assert.False(SupervisedArrivalPolicy.IsConfirmedArrival(true, true, 40f, true, 0f, false));  // out of radius
    }
}

/// <summary>Review D1/§8.2: only the EXACT same completed logical destination may be skipped.</summary>
public class DestinationIdentityPolicyTests
{
    private static readonly Vector3 Goal = new(12f, 0f, -3f);

    [Fact]
    public void ExactSameNameKindAndGoal_IsTheSameLogicalDestination()
    {
        Assert.True(DestinationIdentityPolicy.SameLogicalDestination(
            "Oscar's Store", "store", Goal, "Oscar's Store", "store", Goal));
    }

    [Fact]
    public void ANearbyButDifferentTarget_IsNotTheSameLogicalDestination()
    {
        // The old ghost: <= 1 m from the previous NavTarget. 0.4 m away is a DIFFERENT order.
        Assert.False(DestinationIdentityPolicy.SameLogicalDestination(
            "Oscar's Store", "store", Goal, "Oscar's Store", "store", new Vector3(Goal.X + 0.4f, Goal.Y, Goal.Z)));
        Assert.False(DestinationIdentityPolicy.SameLogicalDestination(
            "Oscar's Store", "store", Goal, "Gas Station", "store", Goal));
        Assert.False(DestinationIdentityPolicy.SameLogicalDestination(
            "Oscar's Store", "store", Goal, "Oscar's Store", "house", Goal));
    }

    [Fact]
    public void NonFiniteGoals_AreNeverTheSameLogicalDestination()
    {
        Assert.False(DestinationIdentityPolicy.SameLogicalDestination(
            "A", "store", new Vector3(float.NaN, 0f, 0f), "A", "store", new Vector3(float.NaN, 0f, 0f)));
    }
}

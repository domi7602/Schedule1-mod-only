using System;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// F2 — a current navigation <c>Complete</c> is EVIDENCE for this operation's arrival, never the
/// arrival itself (brief §8.2). The pickup and the ride are supervised to a readable, confirmed
/// halt inside the target radius with no native/managed reverse before the operation is allowed to
/// complete; the pending evidence is bounded and order/operation scoped. These drive the SAME
/// runtime gate SpikeRunner calls.
/// </summary>
public class ArrivalConfirmationGateTests
{
    private const float Window = 3f;

    private static ArrivalGateOutcome Evaluate(
        bool pending = true,
        bool currentOrder = true,
        bool currentOperation = true,
        bool operationActive = true,
        float pendingSeconds = 0.5f,
        float distance = 3f,
        bool speedReadable = true,
        float signedSpeed = 0f,
        bool reversing = false) =>
        ArrivalConfirmationPolicy.Evaluate(pending, currentOrder, currentOperation, operationActive,
            pendingSeconds, distance, speedReadable, signedSpeed, reversing);

    [Fact]
    public void NoPendingEvidence_IsNotPending()
    {
        Assert.Equal(ArrivalGateOutcome.NotPending, Evaluate(pending: false));
    }

    [Fact]
    public void NormalComplete_StoppedNear_IsAccepted()
    {
        Assert.Equal(ArrivalGateOutcome.Accept, Evaluate());
    }

    [Fact]
    public void MovingPickupComplete_IsHeld()
    {
        // A pickup that is still rolling inside the radius is not "arrived at the player".
        Assert.Equal(ArrivalGateOutcome.Pending, Evaluate(distance: 4f, speedReadable: true, signedSpeed: 22f));
    }

    [Fact]
    public void MovingRideComplete_IsHeld()
    {
        Assert.Equal(ArrivalGateOutcome.Pending, Evaluate(distance: 2f, speedReadable: true, signedSpeed: -14f, reversing: true));
    }

    [Fact]
    public void UnreadableSpeed_IsNeverAHalt()
    {
        // A failed speed read keeps the -1 substitute; readability is a separate fact.
        Assert.Equal(ArrivalGateOutcome.Pending, Evaluate(speedReadable: false, signedSpeed: -1f));
    }

    [Fact]
    public void ManagedOrNativeReverse_IsNeverArrival()
    {
        Assert.Equal(ArrivalGateOutcome.Pending, Evaluate(reversing: true, signedSpeed: 0f));
    }

    [Fact]
    public void StoppedFar_IsNotAccepted()
    {
        Assert.Equal(ArrivalGateOutcome.Pending, Evaluate(distance: 25f));
    }

    [Fact]
    public void BoundedPending_ExpiresWithoutAConfirmedHalt()
    {
        Assert.Equal(ArrivalGateOutcome.Expire, Evaluate(pendingSeconds: Window, distance: 4f, signedSpeed: 18f));
        Assert.Equal(ArrivalGateOutcome.Expire, Evaluate(pendingSeconds: Window + 1f, distance: 25f));
    }

    [Fact]
    public void OldCompleteForANewOperation_IsDropped()
    {
        // The evidence is scoped to the order AND the operation generation: a Complete of a
        // superseded order/operation must never complete the new target.
        Assert.Equal(ArrivalGateOutcome.Expire, Evaluate(currentOrder: false));
        Assert.Equal(ArrivalGateOutcome.Expire, Evaluate(currentOperation: false));
        Assert.Equal(ArrivalGateOutcome.Expire, Evaluate(operationActive: false));
    }

    [Fact]
    public void PendingSecondsAreBoundedByAFiniteWindow()
    {
        Assert.InRange(ArrivalConfirmationPolicy.SettleWindowSeconds, 0.5f, 10f);
    }

    [Fact]
    public void CurrentScopedEvidence_MatchesOnlyTheExactOrderAndOperation()
    {
        Assert.True(ArrivalConfirmationPolicy.IsCurrent(true, 7, 3, 7, 3));
        Assert.False(ArrivalConfirmationPolicy.IsCurrent(true, 7, 3, 8, 3));
        Assert.False(ArrivalConfirmationPolicy.IsCurrent(true, 7, 3, 7, 4));
        Assert.False(ArrivalConfirmationPolicy.IsCurrent(false, 7, 3, 7, 3));
    }

    // ---------------------------------------------------------------- runtime wiring (F2)

    private static string Runner => DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");

    private static string Drain =>
        DriverSafetySourceContractTests.Section(Runner, "private static void TickNavigationCallback", "private static void RecordPendingArrival");

    [Fact]
    public void CurrentComplete_RecordsEvidenceInsteadOfPublishingTerminal()
    {
        // A current Complete must record arrival evidence for THIS order/operation and must not
        // immediately publish a terminal state, arm the board or complete the pickup (brief §8.2).
        Assert.Contains("RecordPendingArrival(order, operation);", Drain);
        Assert.DoesNotContain("SpikeCommands.CompletePickup(", Drain);
        Assert.DoesNotContain("Operation.Terminate(\"navigation complete\"", Drain);
        // The evidence carries the exact order AND operation, so a stale Complete is dropped.
        string record = DriverSafetySourceContractTests.Section(Runner, "private static void RecordPendingArrival", "private static void TickArrivalConfirmation");
        Assert.Contains("SpikeState.NavCallbackArrivalPending = true;", record);
        Assert.Contains("SpikeState.NavCallbackArrivalOrder = order;", record);
        Assert.Contains("SpikeState.NavCallbackArrivalOperation = operation;", record);
    }

    [Fact]
    public void ArrivalConfirmation_RunsEachFrameAndAcceptsOnlyAConfirmedHalt()
    {
        string update = DriverSafetySourceContractTests.Section(Runner, "internal static void Update()", "private static void ShiftPauseDeadlines");
        Assert.Contains("TickArrivalConfirmation();", update);

        string gate = DriverSafetySourceContractTests.Section(Runner, "private static void TickArrivalConfirmation", "private static void TickLotDump");
        Assert.Contains("ArrivalConfirmationPolicy.Evaluate(", gate);
        Assert.Contains("ArrivalGateOutcome.Accept", gate);
        Assert.Contains("SpikeState.Operation.Terminate(", gate);
        // The pickup board/park is only armed on an ACCEPTED arrival.
        Assert.Contains("SpikeState.RideAwaitingBoard = true;", gate);
        int accept = gate.IndexOf("ArrivalGateOutcome.Accept", StringComparison.Ordinal);
        int board = gate.IndexOf("SpikeState.RideAwaitingBoard = true;", StringComparison.Ordinal);
        Assert.True(accept >= 0 && board > accept, "the board must be armed only after an accepted arrival");
    }

    [Fact]
    public void PickupComplete_DoesNotCompletePickupInsideTheCallbackDrain()
    {
        // The drain may not immediately CompletePickup on a Complete; that only happens on the
        // confirmed arrival (brief §8.2).
        Assert.DoesNotContain("SpikeCommands.CompletePickup(", Drain);
    }
}

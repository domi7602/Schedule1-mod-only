using System;
using System.Collections.Generic;
using BusinessIncome.Core;
using BusinessIncome.Models;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>
/// The money-safety heart: ordering, fail-closed gating, marker discipline, re-entrancy,
/// and the "request accepted != confirmed settlement" contract.
/// </summary>
public class PersistenceSettlementTests
{
    private const string Slot = "slot_1";
    private const string StatePath = "payout_state_slot_1.json";
    private const string PendingPath = "payout_pending_slot_1.json";

    private static readonly List<string> Ids = new() { "laundromat", "car_wash" };

    private sealed class Harness
    {
        public FakeStorage Storage = new();
        public FakeBank Bank = new();
        public PayoutSettlementEngine Engine = null!;
        public string OriginalStateJson = "";

        public static Harness New(int lastPaid = 5, bool resolveLoad = true, bool withSlot = true)
        {
            var h = new Harness();
            var state = new PayoutState
            {
                SchemaVersion = PayoutCodec.CurrentSchemaVersion,
                SaveIdentity = Slot,
                LastPaidElapsedDay = lastPaid,
                LastPaidDayByBusiness = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            };
            h.OriginalStateJson = PayoutCodec.EncodeState(state);
            h.Storage.Seed(StatePath, h.OriginalStateJson);

            h.Engine = new PayoutSettlementEngine(h.Storage, h.Bank);
            if (withSlot) h.Engine.SetActiveSlot(Slot);
            if (resolveLoad) h.Engine.ResolveOnLoadComplete(withSlot ? Slot : null);
            h.Engine.LoadState(StatePath);
            h.Engine.LoadPending(PendingPath);
            return h;
        }

        public PayoutResult Book(int day, float net, bool force = false) =>
            Engine.TryBook(day, net, Ids, StatePath, PendingPath, force);

        public PayoutState PersistedState() =>
            PayoutCodec.DecodeState(Storage, StatePath, Slot).State!;
    }

    // --- authority / readiness gating -----------------------------------------------

    [Fact]
    public void NoSlotAuthority_BlocksWithNoWrites()
    {
        var h = Harness.New(withSlot: false);
        var r = h.Book(6, 100f, force: true);

        Assert.Equal(PayoutOutcome.NotAuthoritative, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.False(h.Storage.Has(PendingPath));
        Assert.Equal(h.OriginalStateJson, h.Storage.Peek(StatePath));
    }

    [Fact]
    public void ReadinessNotResolved_Blocks()
    {
        var h = Harness.New(resolveLoad: false);
        var r = h.Book(6, 100f);

        Assert.Equal(PayoutOutcome.NotReady, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
    }

    [Fact]
    public void ResetForLoad_ClearsSlot_SoNoWriteDuringTransition()
    {
        var h = Harness.New();
        h.Engine.ResetForLoad();

        Assert.Equal(SlotAuthority.Unknown, h.Engine.Authority);
        Assert.Equal(LoadReadiness.NotResolved, h.Engine.Readiness);
        var r = h.Book(6, 100f, force: true);
        Assert.Equal(PayoutOutcome.NotAuthoritative, r.Outcome);
        Assert.False(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void CorruptState_BlocksMutation()
    {
        var h = Harness.New();
        h.Storage.Seed(StatePath, "{ broken");
        h.Engine.LoadState(StatePath);

        var r = h.Book(6, 100f);
        Assert.Equal(PayoutOutcome.BlockedStateCorrupt, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
    }

    // --- pending marker discipline ---------------------------------------------------

    [Fact]
    public void ValidPending_BlocksEvenForced()
    {
        var h = Harness.New();
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(new PendingPayoutState { Day = 7, Amount = 90f, BusinessCount = 2 }));
        h.Engine.LoadPending(PendingPath);
        string marker = h.Storage.Peek(PendingPath)!;

        var r = h.Book(8, 100f, force: true);

        Assert.Equal(PayoutOutcome.BlockedPending, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.Equal(marker, h.Storage.Peek(PendingPath)); // never overwritten
    }

    [Fact]
    public void CorruptPending_BlocksEvenForced()
    {
        var h = Harness.New();
        h.Storage.Seed(PendingPath, "{ broken");
        h.Engine.LoadPending(PendingPath);

        var r = h.Book(8, 100f, force: true);
        Assert.Equal(PayoutOutcome.BlockedPending, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
    }

    [Fact]
    public void AlreadyPaid_WithoutForce()
    {
        var h = Harness.New(lastPaid: 5);
        var r = h.Book(5, 100f);
        Assert.Equal(PayoutOutcome.AlreadyPaid, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
    }

    // --- terminal days ---------------------------------------------------------------

    [Fact]
    public void NoBusinesses_TerminalPersisted_NoBank()
    {
        var h = Harness.New();
        var r = h.Engine.TryBook(6, 0f, Array.Empty<string>(), StatePath, PendingPath, false);

        Assert.Equal(PayoutOutcome.NoBusinesses, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.False(h.Storage.Has(PendingPath));
        Assert.Equal(6, h.PersistedState().LastPaidElapsedDay);
    }

    [Fact]
    public void ZeroNet_TerminalPersisted_NoBank()
    {
        var h = Harness.New();
        var r = h.Book(6, 0f);
        Assert.Equal(PayoutOutcome.ZeroNet, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.Equal(6, h.PersistedState().LastPaidElapsedDay);
    }

    [Fact]
    public void TerminalPersistFailure_SignalsStateNotPersisted()
    {
        var h = Harness.New();
        h.Storage.FailWrite.Add(StatePath);
        var r = h.Engine.TryBook(6, 0f, Array.Empty<string>(), StatePath, PendingPath, false);
        Assert.Equal(PayoutOutcome.StateNotPersisted, r.Outcome);
    }

    // --- bank readiness --------------------------------------------------------------

    [Fact]
    public void BankNotReady_BlocksBeforeMarker()
    {
        var h = Harness.New();
        h.Bank.Ready = false;
        var r = h.Book(6, 100f);

        Assert.Equal(PayoutOutcome.NotReady, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.False(h.Storage.Has(PendingPath));
        Assert.Equal(h.OriginalStateJson, h.Storage.Peek(StatePath));
    }

    // --- marker write failure --------------------------------------------------------

    [Fact]
    public void MarkerWriteFailure_BlocksBank_AndRollsBackExactly()
    {
        var h = Harness.New();
        h.Storage.FailWrite.Add(PendingPath);

        var r = h.Book(6, 100f);

        Assert.Equal(PayoutOutcome.BlockedMarkerWrite, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.Equal(h.OriginalStateJson, h.Storage.Peek(StatePath)); // no bank-side persistence
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);          // rolled back in memory
        Assert.False(h.Engine.State!.LastPaidDayByBusiness.ContainsKey("laundromat"));
    }

    // --- accepted path ---------------------------------------------------------------

    [Fact]
    public void Accepted_CommitsStates_ClearsMarker()
    {
        var h = Harness.New();
        var r = h.Book(6, 123.5f);

        Assert.Equal(PayoutOutcome.Requested, r.Outcome);
        Assert.True(r.BankInvoked);
        Assert.False(r.Succeeded == false && r.Outcome != PayoutOutcome.Requested);
        Assert.Equal(1, h.Bank.Calls);
        Assert.False(h.Storage.Has(PendingPath)); // cleared after commit
        var persisted = h.PersistedState();
        Assert.Equal(6, persisted.LastPaidElapsedDay);
        Assert.Equal(6, persisted.LastPaidDayByBusiness["laundromat"]);
        Assert.Equal(6, persisted.LastPaidDayByBusiness["car_wash"]);
    }

    [Fact]
    public void Accepted_ButStateCommitFails_MarkerKept()
    {
        var h = Harness.New();
        h.Storage.FailWrite.Add(StatePath);

        var r = h.Book(6, 100f);

        Assert.Equal(PayoutOutcome.BookedStateNotSaved, r.Outcome);
        Assert.Equal(1, h.Bank.Calls);
        Assert.True(h.Storage.Has(PendingPath)); // kept for explicit resolution
    }

    [Fact]
    public void BankRejects_Cleanly_RollsBackAndClearsMarker()
    {
        var h = Harness.New();
        h.Bank.Handler = (_, _, _) => false;

        var r = h.Book(6, 100f);

        Assert.Equal(PayoutOutcome.Skipped, r.Outcome);
        Assert.Equal(1, h.Bank.Calls);
        Assert.False(h.Storage.Has(PendingPath));
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);
        Assert.False(h.Engine.State!.LastPaidDayByBusiness.ContainsKey("laundromat"));
    }

    [Fact]
    public void BankThrowsAfterInvocation_OutcomeUnknown_MarkerKept_RetryBlocked()
    {
        var h = Harness.New();
        h.Bank.Handler = (_, _, _) => throw new InvalidOperationException("mutate-then-throw");

        var r = h.Book(6, 100f);

        Assert.Equal(PayoutOutcome.UnknownOutcome, r.Outcome);
        Assert.True(r.BankInvoked);
        Assert.True(h.Storage.Has(PendingPath));  // marker survives -> no silent retry
        Assert.Equal(1, h.Bank.Calls);

        // A retry must be blocked by the surviving marker.
        var again = h.Book(6, 100f, force: true);
        Assert.Equal(PayoutOutcome.BlockedPending, again.Outcome);
        Assert.Equal(1, h.Bank.Calls);
    }

    [Fact]
    public void ReentrantBooking_IsRejected()
    {
        var h = Harness.New();
        PayoutResult inner = default;
        h.Bank.Handler = (_, _, _) =>
        {
            inner = h.Engine.TryBook(7, 100f, Ids, StatePath, PendingPath, true);
            return true;
        };

        var outer = h.Book(6, 100f);

        Assert.Equal(PayoutOutcome.Requested, outer.Outcome);
        Assert.Equal(PayoutOutcome.Reentrant, inner.Outcome);
        Assert.Equal(1, h.Bank.Calls);
    }

    [Fact]
    public void CommitOnly_RequiresAuthority()
    {
        var h = Harness.New(withSlot: false);
        var r = h.Engine.CommitOnly(6, Ids, StatePath);
        Assert.Equal(PayoutOutcome.NotAuthoritative, r.Outcome);
        Assert.Equal(h.OriginalStateJson, h.Storage.Peek(StatePath));
    }

    [Fact]
    public void CommitOnly_PersistsForward_AndReportsFailure()
    {
        var h = Harness.New(lastPaid: 5);
        Assert.Equal(PayoutOutcome.Requested, h.Engine.CommitOnly(7, Array.Empty<string>(), StatePath).Outcome);
        Assert.Equal(7, h.PersistedState().LastPaidElapsedDay);

        h.Storage.FailWrite.Add(StatePath);
        Assert.Equal(PayoutOutcome.StateNotPersisted, h.Engine.CommitOnly(8, Array.Empty<string>(), StatePath).Outcome);
    }

    [Fact]
    public void CommitOnly_BlockedByUnresolvedPendingMarker()
    {
        var h = Harness.New(lastPaid: 5);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(new PendingPayoutState { Day = 7, Amount = 90f, BusinessCount = 2 }));
        h.Engine.LoadPending(PendingPath);

        var r = h.Engine.CommitOnly(9, Array.Empty<string>(), StatePath);

        Assert.Equal(PayoutOutcome.BlockedPending, r.Outcome);
        Assert.Equal(h.OriginalStateJson, h.Storage.Peek(StatePath)); // state never advances past an unresolved payout day
    }

    // --- preview ---------------------------------------------------------------------

    [Fact]
    public void Preview_RequiresNothing_AndWritesNothing()
    {
        var storage = new FakeStorage();
        var engine = new PayoutSettlementEngine(storage, bank: null); // no slot, no readiness, no bank

        var r = engine.Preview(9, 250f, 3);

        Assert.Equal(PayoutOutcome.DryRun, r.Outcome);
        Assert.Equal(250f, r.Net);
        Assert.Equal(0, storage.FileCount);
        Assert.Equal(0, storage.WriteCount);
    }

    // --- confirm / resolve -----------------------------------------------------------

    [Fact]
    public void ConfirmPending_CommitsForwardThenClears()
    {
        var h = Harness.New(lastPaid: 5);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(new PendingPayoutState { Day = 8, Amount = 200f, BusinessCount = 2 }));
        h.Engine.LoadPending(PendingPath);

        var r = h.Engine.ConfirmPending(StatePath, PendingPath);

        Assert.Equal(PayoutOutcome.PendingConfirmed, r.Outcome);
        Assert.Equal(8, h.PersistedState().LastPaidElapsedDay);
        Assert.False(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void ConfirmPending_NeverMovesStateBackwards()
    {
        var h = Harness.New(lastPaid: 12);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(new PendingPayoutState { Day = 4, Amount = 10f, BusinessCount = 1 }));
        h.Engine.LoadPending(PendingPath);

        var r = h.Engine.ConfirmPending(StatePath, PendingPath);

        Assert.Equal(PayoutOutcome.PendingConfirmed, r.Outcome);
        Assert.Equal(12, h.PersistedState().LastPaidElapsedDay); // stale marker: no regression
        Assert.False(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void ConfirmPending_CommitFailure_KeepsMarker()
    {
        var h = Harness.New();
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(new PendingPayoutState { Day = 8, Amount = 200f, BusinessCount = 2 }));
        h.Engine.LoadPending(PendingPath);
        h.Storage.FailWrite.Add(StatePath);

        var r = h.Engine.ConfirmPending(StatePath, PendingPath);

        Assert.Equal(PayoutOutcome.StateNotPersisted, r.Outcome);
        Assert.True(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void ConfirmPending_NoMarker_IsSkipped_NeverAutoClears()
    {
        var h = Harness.New();
        var r = h.Engine.ConfirmPending(StatePath, PendingPath);
        Assert.Equal(PayoutOutcome.Skipped, r.Outcome);
        Assert.False(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void ResolvePending_RestoresSnapshotFromSameSession()
    {
        var h = Harness.New(lastPaid: 5);
        h.Bank.Handler = (_, _, _) => throw new InvalidOperationException("boom");

        // UnknownOutcome leaves the in-memory mark applied AND the marker on disk.
        Assert.Equal(PayoutOutcome.UnknownOutcome, h.Book(9, 100f).Outcome);
        Assert.Equal(9, h.Engine.State!.LastPaidElapsedDay);

        var r = h.Engine.ResolvePending(PendingPath);

        Assert.Equal(PayoutOutcome.PendingResolved, r.Outcome);
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);   // pre-call snapshot restored
        Assert.False(h.Engine.State!.LastPaidDayByBusiness.ContainsKey("laundromat"));
        Assert.False(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void ResolvePending_WithoutSnapshot_StillClearsMarker()
    {
        var h = Harness.New(lastPaid: 5);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(new PendingPayoutState { Day = 9, Amount = 100f, BusinessCount = 2 }));
        h.Engine.LoadPending(PendingPath);

        var r = h.Engine.ResolvePending(PendingPath);

        Assert.Equal(PayoutOutcome.PendingResolved, r.Outcome);
        Assert.False(h.Storage.Has(PendingPath));
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);
    }

    [Fact]
    public void ResolvePending_NoMarker_IsSkipped()
    {
        var h = Harness.New();
        var r = h.Engine.ResolvePending(PendingPath);
        Assert.Equal(PayoutOutcome.Skipped, r.Outcome);
    }
}

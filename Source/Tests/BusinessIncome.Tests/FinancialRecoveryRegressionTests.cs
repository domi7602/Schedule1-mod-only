using System;
using System.Collections.Generic;
using BusinessIncome.Core;
using BusinessIncome.Models;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>
/// Regression coverage for the parent-reported financial-integration defects. Every test
/// names the defect it pins. The block marked "target" fails against the pre-fix behaviour
/// and must stay green; the single characterization test documents the missing-state gap
/// that the explicit Initialize operation (added in the same change) closes.
/// </summary>
public class FinancialRecoveryRegressionTests
{
    private const string Slot = "slot_1";
    private const string StatePath = "payout_state_slot_1.json";
    private const string PendingPath = "payout_pending_slot_1.json";

    private static readonly string[] TwoBiz = { "a", "b" };

    private static PayoutState State(int lastPaid) => new()
    {
        SchemaVersion = PayoutCodec.CurrentSchemaVersion,
        SaveIdentity = Slot,
        LastPaidElapsedDay = lastPaid,
        LastPaidDayByBusiness = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    };

    private static PendingPayoutState Marker(int day) => new()
    {
        Day = day,
        Amount = 100f,
        BusinessCount = 2,
        StartedUtc = "t"
    };

    private sealed class Harness
    {
        public readonly FakeStorage Storage = new();
        public readonly FakeBank Bank = new();
        public readonly PayoutSettlementEngine Engine;

        public Harness(int lastPaid = -1)
        {
            Engine = new PayoutSettlementEngine(Storage, Bank);
            Engine.SetActiveSlot(Slot);
            Engine.ResolveOnLoadComplete(Slot);
            if (lastPaid >= 0) Storage.Seed(StatePath, PayoutCodec.EncodeState(State(lastPaid)));
            Engine.LoadState(StatePath);
            Engine.LoadPending(PendingPath);
        }

        public PayoutState? Persisted() => PayoutCodec.DecodeState(Storage, StatePath, Slot).State;
        public int PersistedDay => Persisted()?.LastPaidElapsedDay ?? int.MinValue;
    }

    // --- defect 1: a failed state write must never advance memory --------------------

    [Fact]
    public void TerminalCommit_WriteFailure_DoesNotAdvanceMemory()
    {
        var h = new Harness(lastPaid: 5);
        h.Storage.FailWrite.Add(StatePath);

        var r = h.Engine.TryBook(6, 0f, Array.Empty<string>(), StatePath, PendingPath, force: false);

        Assert.Equal(PayoutOutcome.StateNotPersisted, r.Outcome);
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);
        Assert.Equal(5, h.PersistedDay);
    }

    [Fact]
    public void CommitOnly_WriteFailure_DoesNotAdvanceMemory()
    {
        var h = new Harness(lastPaid: 5);
        h.Storage.FailWrite.Add(StatePath);

        var r = h.Engine.CommitOnly(8, Array.Empty<string>(), StatePath);

        Assert.Equal(PayoutOutcome.StateNotPersisted, r.Outcome);
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);
        Assert.Equal(5, h.PersistedDay);
    }

    [Fact] // characterization: pins the missing-state gap that Initialize closes
    public void MissingState_ForwardCommitIsNotReady()
    {
        var h = new Harness(lastPaid: -1);
        Assert.Null(h.Engine.State);

        var r = h.Engine.CommitOnly(6, Array.Empty<string>(), StatePath);

        Assert.Equal(PayoutOutcome.NotReady, r.Outcome);
        Assert.False(h.Storage.Has(StatePath));
    }

    [Fact]
    public void Initialize_SeedsGenuinelyMissingState_Durably()
    {
        var h = new Harness(lastPaid: -1);

        var r = h.Engine.Initialize(4, StatePath);

        Assert.Equal(PayoutOutcome.Requested, r.Outcome);
        Assert.Equal(4, h.Engine.State!.LastPaidElapsedDay);
        Assert.Equal(4, h.Engine.State.InitializationDay);
        Assert.Equal(4, h.PersistedDay);
        Assert.Equal(Slot, h.Persisted()!.SaveIdentity);
    }

    [Fact]
    public void Initialize_WriteFailure_DoesNotSeedMemory()
    {
        var h = new Harness(lastPaid: -1);
        h.Storage.FailWrite.Add(StatePath);

        var r = h.Engine.Initialize(4, StatePath);

        Assert.NotEqual(PayoutOutcome.Requested, r.Outcome);
        Assert.Null(h.Engine.State);
        Assert.False(h.Storage.Has(StatePath));
    }

    [Fact]
    public void Initialize_RefusesWhenStateAlreadyPresent()
    {
        var h = new Harness(lastPaid: 5);

        var r = h.Engine.Initialize(4, StatePath);

        Assert.NotEqual(PayoutOutcome.Requested, r.Outcome);
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);
        Assert.Equal(5, h.PersistedDay);
    }

    [Fact]
    public void Initialize_BlockedByPendingMarker_NoSeed()
    {
        var h = new Harness(lastPaid: -1);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(Marker(3)));
        h.Engine.LoadPending(PendingPath);

        var r = h.Engine.Initialize(4, StatePath);

        Assert.Equal(PayoutOutcome.BlockedPending, r.Outcome);
        Assert.Null(h.Engine.State);
        Assert.False(h.Storage.Has(StatePath));
    }

    // --- defect 2: backup recovery / structural validation must fail closed ----------

    [Fact]
    public void RecoveredFromBackup_IsNotUsable_AndBlocksMutation()
    {
        var s = new FakeStorage();
        s.Seed(StatePath, "{ broken");
        s.Seed(StatePath + ".bak", PayoutCodec.EncodeState(State(7)));

        var r = PayoutCodec.DecodeState(s, StatePath, Slot);

        Assert.Equal(StateStatus.RecoveredFromBackup, r.Status);
        Assert.False(r.Usable);
        Assert.True(r.BlocksMutation);
    }

    [Fact]
    public void RecoveredFromBackup_BlocksAutomaticMutation_NoBankCall()
    {
        var h = new Harness(lastPaid: -1);
        h.Storage.Seed(StatePath, "{ broken");
        h.Storage.Seed(StatePath + ".bak", PayoutCodec.EncodeState(State(7)));
        h.Engine.LoadState(StatePath);

        var r = h.Engine.TryBook(8, 100f, TwoBiz, StatePath, PendingPath, force: true);

        Assert.Equal(PayoutOutcome.BlockedStateCorrupt, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.False(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void MissingMain_UnreadableBackup_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(StatePath + ".bak", "x");
        s.FailRead.Add(StatePath + ".bak");

        var r = PayoutCodec.DecodeState(s, StatePath, Slot);

        Assert.Equal(StateStatus.Corrupt, r.Status);
        Assert.True(r.BlocksMutation);
    }

    [Fact]
    public void MissingMain_TmpOrphan_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(StatePath + ".tmp", "{\"SchemaVersion\":1}");

        var r = PayoutCodec.DecodeState(s, StatePath, Slot);

        Assert.Equal(StateStatus.Corrupt, r.Status);
    }

    [Fact]
    public void EmptyObjectState_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(StatePath, "{}");

        var r = PayoutCodec.DecodeState(s, StatePath, Slot);

        Assert.Equal(StateStatus.Corrupt, r.Status);
    }

    [Fact]
    public void PartialState_MissingDictionary_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(StatePath, "{\"SchemaVersion\":1,\"LastPaidElapsedDay\":2,\"SaveIdentity\":\"slot_1\"}");

        var r = PayoutCodec.DecodeState(s, StatePath, Slot);

        Assert.Equal(StateStatus.Corrupt, r.Status);
    }

    [Fact] // legacy compatibility: empty identity with a real payload is still Ok
    public void EmptyIdentityState_WithRealPayload_IsOk()
    {
        var s = new FakeStorage();
        var legacy = State(3);
        legacy.SaveIdentity = "";
        s.Seed(StatePath, PayoutCodec.EncodeState(legacy));

        var r = PayoutCodec.DecodeState(s, StatePath, Slot);

        Assert.Equal(StateStatus.Ok, r.Status);
        Assert.Equal(3, r.State!.LastPaidElapsedDay);
    }

    [Fact]
    public void EmptyObjectPending_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(PendingPath, "{}");

        var r = PayoutCodec.DecodePending(s, PendingPath);

        Assert.Equal(PendingStatus.Corrupt, r.Status);
        Assert.True(r.BlocksBooking);
    }

    [Fact]
    public void PartialPending_MissingCount_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(PendingPath, "{\"Day\":2,\"Amount\":5}");

        var r = PayoutCodec.DecodePending(s, PendingPath);

        Assert.Equal(PendingStatus.Corrupt, r.Status);
    }

    [Fact]
    public void MissingMain_UnreadablePendingBackup_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(PendingPath + ".bak", "x");
        s.FailRead.Add(PendingPath + ".bak");

        var r = PayoutCodec.DecodePending(s, PendingPath);

        Assert.Equal(PendingStatus.Corrupt, r.Status);
        Assert.True(r.BlocksBooking);
    }

    [Fact]
    public void MissingMain_TmpOrphanPending_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(PendingPath + ".tmp", "{}");

        var r = PayoutCodec.DecodePending(s, PendingPath);

        Assert.Equal(PendingStatus.Corrupt, r.Status);
    }

    // --- defect 3: marker clearing must stop on a failed artifact delete -------------

    [Fact]
    public void ClearMarker_FailsWhenBakUndeletable_StopsBeforeMain()
    {
        var s = new FakeStorage();
        s.Seed(PendingPath, "m");
        s.Seed(PendingPath + ".bak", "b");
        s.FailDelete.Add(PendingPath + ".bak");

        Assert.False(PayoutCodec.ClearMarker(s, PendingPath));
        Assert.True(s.Has(PendingPath));
        Assert.DoesNotContain(PendingPath, s.DeleteOrder);
    }

    [Fact]
    public void ClearMarker_FailsWhenTmpUndeletable_StopsBeforeMain()
    {
        var s = new FakeStorage();
        s.Seed(PendingPath, "m");
        s.Seed(PendingPath + ".tmp", "t");
        s.FailDelete.Add(PendingPath + ".tmp");

        Assert.False(PayoutCodec.ClearMarker(s, PendingPath));
        Assert.True(s.Has(PendingPath));
    }

    [Fact] // reports false while ANY artifact survives
    public void ClearMarker_ReportsFalseWhenBackupSurvives()
    {
        var s = new FakeStorage();
        s.Seed(PendingPath, "m");
        s.Seed(PendingPath + ".bak", "b");
        s.FailDelete.Add(PendingPath + ".bak");

        Assert.False(PayoutCodec.ClearMarker(s, PendingPath));
        Assert.True(s.Has(PendingPath + ".bak"));
    }

    // --- defect 4: a rejected request whose marker survives must stay blocked --------

    [Fact]
    public void BankRejects_MarkerClearFails_StaysBlocked()
    {
        var h = new Harness(lastPaid: 5);
        h.Bank.Handler = (_, _, _) => false;
        h.Storage.FailDelete.Add(PendingPath);

        var first = h.Engine.TryBook(6, 100f, TwoBiz, StatePath, PendingPath, force: true);
        Assert.False(first.BankInvoked);
        Assert.True(h.Storage.Has(PendingPath));

        var second = h.Engine.TryBook(6, 100f, TwoBiz, StatePath, PendingPath, force: true);
        Assert.Equal(PayoutOutcome.BlockedPending, second.Outcome);
        Assert.Equal(1, h.Bank.Calls);
    }

    // --- defect 5: resolve/confirm must not claim success on a failed clear ----------

    [Fact]
    public void ConfirmPending_ClearFails_DoesNotClaimSuccess()
    {
        var h = new Harness(lastPaid: 5);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(Marker(8)));
        h.Engine.LoadPending(PendingPath);
        h.Storage.FailDelete.Add(PendingPath);

        var r = h.Engine.ConfirmPending(StatePath, PendingPath);

        Assert.NotEqual(PayoutOutcome.PendingConfirmed, r.Outcome);
        Assert.True(h.Storage.Has(PendingPath));
        Assert.True(h.Engine.PendingLoad.BlocksBooking);
    }

    [Fact]
    public void ResolvePending_ClearFails_DoesNotClaimSuccess()
    {
        var h = new Harness(lastPaid: 5);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(Marker(9)));
        h.Engine.LoadPending(PendingPath);
        h.Storage.FailDelete.Add(PendingPath);

        var r = h.Engine.ResolvePending(PendingPath);

        Assert.NotEqual(PayoutOutcome.PendingResolved, r.Outcome);
        Assert.True(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void ResolvePending_BlockedByCorruptState()
    {
        var h = new Harness(lastPaid: -1);
        h.Storage.Seed(StatePath, "{ broken");
        h.Engine.LoadState(StatePath);
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(Marker(9)));
        h.Engine.LoadPending(PendingPath);

        var r = h.Engine.ResolvePending(PendingPath);

        Assert.Equal(PayoutOutcome.BlockedStateCorrupt, r.Outcome);
        Assert.True(h.Storage.Has(PendingPath));
    }

    [Fact]
    public void ResetForLoad_ClearsStateAndSnapshot()
    {
        var h = new Harness(lastPaid: 5);
        h.Bank.Handler = (_, _, _) => throw new InvalidOperationException("kaboom");
        Assert.Equal(PayoutOutcome.UnknownOutcome,
            h.Engine.TryBook(9, 100f, TwoBiz, StatePath, PendingPath, force: true).Outcome);
        Assert.NotNull(h.Engine.LastSnapshot);

        h.Engine.ResetForLoad();

        Assert.Null(h.Engine.State);
        Assert.Null(h.Engine.LastSnapshot);
        Assert.Equal(StateStatus.Missing, h.Engine.StateLoad.Status);
        Assert.Equal(PendingStatus.Missing, h.Engine.PendingLoad.Status);
        Assert.Equal(SlotAuthority.Unknown, h.Engine.Authority);
    }

    [Fact]
    public void Resolve_StaleSnapshotNotTiedToCurrentMarker_IsNotApplied()
    {
        var h = new Harness(lastPaid: 5);
        h.Bank.Handler = (_, _, _) => throw new InvalidOperationException("kaboom");
        Assert.Equal(PayoutOutcome.UnknownOutcome,
            h.Engine.TryBook(9, 100f, TwoBiz, StatePath, PendingPath, force: true).Outcome);

        // A DIFFERENT marker is now on disk than the one the snapshot was armed for.
        h.Storage.Seed(PendingPath, PayoutCodec.EncodePending(Marker(11)));
        h.Engine.LoadPending(PendingPath);

        var r = h.Engine.ResolvePending(PendingPath);

        Assert.Equal(PayoutOutcome.PendingResolved, r.Outcome);
        Assert.Equal(9, h.Engine.State!.LastPaidElapsedDay);
    }

    [Fact]
    public void Accepted_DurableCommit_MarkerClearFailed_Resolve_DoesNotRollBack()
    {
        var h = new Harness(lastPaid: 5);
        h.Storage.FailDelete.Add(PendingPath);

        var book = h.Engine.TryBook(9, 100f, TwoBiz, StatePath, PendingPath, force: true);
        Assert.Equal(PayoutOutcome.Requested, book.Outcome);
        Assert.True(h.Storage.Has(PendingPath));
        Assert.Equal(9, h.PersistedDay);

        h.Storage.FailDelete.Remove(PendingPath);
        var r = h.Engine.ResolvePending(PendingPath);

        Assert.Equal(PayoutOutcome.PendingResolved, r.Outcome);
        Assert.Equal(9, h.Engine.State!.LastPaidElapsedDay);
        Assert.Equal(9, h.PersistedDay);
    }

    // --- defect 10: invalid inputs fail closed --------------------------------------

    [Fact]
    public void NegativeDay_Rejected_NoWriteNoBank()
    {
        var h = new Harness(lastPaid: 5);

        var r = h.Engine.TryBook(-1, 100f, TwoBiz, StatePath, PendingPath, force: true);

        Assert.NotEqual(PayoutOutcome.Requested, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.False(h.Storage.Has(PendingPath));
        Assert.Equal(5, h.PersistedDay);
    }

    [Fact]
    public void CommitOnly_NegativeDay_Rejected()
    {
        var h = new Harness(lastPaid: 5);

        var r = h.Engine.CommitOnly(-3, Array.Empty<string>(), StatePath);

        Assert.NotEqual(PayoutOutcome.Requested, r.Outcome);
        Assert.Equal(5, h.PersistedDay);
        Assert.Equal(5, h.Engine.State!.LastPaidElapsedDay);
    }

    [Fact]
    public void NonFiniteNet_FailsClosed_NoTerminalMark()
    {
        var h = new Harness(lastPaid: 5);

        var r = h.Engine.TryBook(6, float.NaN, TwoBiz, StatePath, PendingPath, force: true);

        Assert.NotEqual(PayoutOutcome.NoBusinesses, r.Outcome);
        Assert.NotEqual(PayoutOutcome.ZeroNet, r.Outcome);
        Assert.Equal(0, h.Bank.Calls);
        Assert.Equal(5, h.PersistedDay);
    }
}

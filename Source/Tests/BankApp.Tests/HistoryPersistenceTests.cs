using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.Logic;
using BankApp.Models;
using Xunit;

namespace BankApp.Tests;

/// <summary>
/// Proves the receipt policy itself. A failed history write used to look saved,
/// and the next path lookup could retarget the receipt. These tests fail that
/// outcome without a source-text search. They never move money.
/// </summary>
public sealed class HistoryPersistenceTests
{
    private const string Slot1 = "slot_1";
    private const string Slot2 = "slot_2";
    private const string Path1 = "/userdata/BankApp/bank_slot_1.json";
    private const string Path2 = "/userdata/BankApp/bank_slot_2.json";

    [Fact]
    public void Save_WhenWriteSucceeds_ClearsFailureAndPublishesTheBoundFile()
    {
        var io = new FakeHistoryIo();
        var store = Bind(io, Slot2, Path2);
        BankState state = Receipt("dep-1", 40f);

        Assert.True(store.Save(Slot2, state));

        Assert.Null(store.StatusFor(Slot2));
        Assert.False(store.HasPending(Slot2));
        Assert.Equal("dep-1", io.Files[Path2].Transactions[0].Id);
        Assert.Equal(40f, io.Files[Path2].WeeklyDepositedAmount);
        Assert.Equal(Path2, io.Attempts.Single().Path);
    }

    [Fact]
    public void Save_WhenWriteFails_KeepsTheReceiptAndDoesNotClaimSuccess()
    {
        var io = new FakeHistoryIo { FailAll = true };
        var store = Bind(io, Slot2, Path2);
        BankState state = Receipt("keep-me", 25f);

        Assert.False(store.Save(Slot2, state));

        Assert.Equal(HistoryPersistence.FailureStatus, store.StatusFor(Slot2));
        Assert.Equal(Path2, store.PathFor(Slot2));
        Assert.False(io.Files.ContainsKey(Path2));
        Assert.Equal("keep-me", state.Transactions[0].Id);
        Assert.Equal(25f, state.WeeklyDepositedAmount);
        Assert.Equal(1, io.WritesTo(Path2));
        Assert.Equal(0, io.WritesTo(Path1));
    }

    [Fact]
    public void FailedSave_ThenSuccess_WritesTheSameReceiptToTheSamePath()
    {
        var io = new FakeHistoryIo { FailAll = true };
        var store = Bind(io, Slot2, Path2);
        BankState state = Receipt("retry", 10f);

        Assert.False(store.Save(Slot2, state));
        io.FailAll = false;
        Assert.True(store.Save(Slot2, state));

        Assert.Null(store.StatusFor(Slot2));
        Assert.Equal("retry", io.Files[Path2].Transactions[0].Id);
        Assert.Equal(10f, io.Files[Path2].WeeklyDepositedAmount);
        Assert.Equal(2, io.WritesTo(Path2));
    }

    [Fact]
    public void FailedSave_IsNotRetargetedWhenTheCallerPassesAnotherPath()
    {
        var io = new FakeHistoryIo { FailAll = true };
        var store = Bind(io, Slot2, Path2);
        Assert.False(store.Save(Slot2, Receipt("bound", 5f)));

        store.Bind(Slot2, "/userdata/BankApp/bank_slot_9.json");
        io.FailAll = false;
        Assert.True(store.Save(Slot2, Receipt("bound", 5f)));

        Assert.Equal(Path2, io.Attempts.Last().Path);
        Assert.False(io.Files.ContainsKey("/userdata/BankApp/bank_slot_9.json"));
        Assert.Equal("bound", io.Files[Path2].Transactions[0].Id);
    }

    [Fact]
    public void SlotChange_AfterFailedSave_DoesNotWriteTheOldReceiptIntoTheNewSlot()
    {
        var io = new FakeHistoryIo { Fail = path => path == Path2 };
        var store = Bind(io, Slot2, Path2);
        BankState oldState = Receipt("slot-two", 80f);
        Assert.False(store.Save(Slot2, oldState));

        store.LeaveSlot(Slot2, oldState);
        store.Bind(Slot1, Path1);
        Assert.True(store.Save(Slot1, Receipt("slot-one", 1f)));

        Assert.Equal(HistoryPersistence.FailureStatus, store.StatusFor(Slot2));
        Assert.Null(store.StatusFor(Slot1));
        Assert.Equal("slot-one", io.Files[Path1].Transactions[0].Id);
        Assert.False(io.Files.ContainsKey(Path2));
        Assert.DoesNotContain(io.Attempts, attempt => attempt.Path == Path1 && attempt.Id == "slot-two");
        Assert.Equal("slot-two", io.Attempts.Last(attempt => attempt.Path == Path2).Id);
    }

    [Fact]
    public void LaterFlush_WritesTheRetainedSlotAndLeavesTheOtherSlotUntouched()
    {
        var io = new FakeHistoryIo { Fail = path => path == Path2 };
        var store = Bind(io, Slot2, Path2);
        BankState oldState = Receipt("keep", 15f);
        Assert.False(store.Save(Slot2, oldState));
        store.LeaveSlot(Slot2, oldState);
        store.Bind(Slot1, Path1);
        Assert.True(store.Save(Slot1, Receipt("other", 2f)));

        io.Fail = null;
        store.FlushOnce(Slot1, Receipt("other", 2f));

        Assert.Null(store.StatusFor(Slot2));
        Assert.Null(store.StatusFor(Slot1));
        Assert.Equal("keep", io.Files[Path2].Transactions[0].Id);
        Assert.Equal(15f, io.Files[Path2].WeeklyDepositedAmount);
        Assert.Equal("other", io.Files[Path1].Transactions[0].Id);
    }

    [Fact]
    public void ReturningToTheFailedSlot_RestoresTheReceiptInsteadOfDisk()
    {
        var io = new FakeHistoryIo { FailAll = true };
        var store = Bind(io, Slot2, Path2);
        Assert.False(store.Save(Slot2, Receipt("unsaved", 33f)));
        store.LeaveSlot(Slot2, Receipt("unsaved", 33f));

        Assert.True(store.TryRestore(Slot2, out BankState restored));
        Assert.Equal("unsaved", restored.Transactions[0].Id);
        Assert.Equal(33f, restored.WeeklyDepositedAmount);
        Assert.Equal(Path2, store.PathFor(Slot2));
    }

    [Fact]
    public void LeavingASlot_FlushesOnce_NotAgainOnTheNextLeave()
    {
        var io = new FakeHistoryIo { FailAll = true };
        var store = Bind(io, Slot2, Path2);
        BankState state = Receipt("frozen", 7f);
        Assert.False(store.Save(Slot2, state));

        store.LeaveSlot(Slot2, state);
        int afterLeave = io.WritesTo(Path2);
        store.LeaveSlot(Slot2, state);
        store.LeaveSlot(Slot1, Receipt("other", 1f));

        Assert.Equal(2, afterLeave);
        Assert.Equal(2, io.WritesTo(Path2));
        Assert.Equal(0, io.WritesTo(Path1));
    }

    [Fact]
    public void FlushOnce_OnACleanSlot_DoesNotWriteOrInventAFailure()
    {
        var io = new FakeHistoryIo { FailAll = true };
        var store = Bind(io, Slot2, Path2);

        store.FlushOnce(Slot2, Receipt("clean", 1f));

        Assert.Empty(io.Attempts);
        Assert.Null(store.StatusFor(Slot2));
        Assert.False(store.HasPending(Slot2));
    }

    [Fact]
    public void UnboundSave_WritesNowhere()
    {
        var io = new FakeHistoryIo();
        var store = new HistoryPersistence((path, state) => io.Write(path, state));

        Assert.False(store.Save(Slot2, Receipt("nowhere", 9f)));

        Assert.Empty(io.Attempts);
        Assert.Empty(io.Files);
        Assert.Null(store.StatusFor(Slot2));
    }

    [Fact]
    public void WriterException_IsAFailedSaveAndKeepsTheReceipt()
    {
        var io = new FakeHistoryIo { Throw = true };
        var store = Bind(io, Slot2, Path2);

        Assert.False(store.Save(Slot2, Receipt("thrown", 4f)));

        Assert.Equal(HistoryPersistence.FailureStatus, store.StatusFor(Slot2));
        Assert.False(io.Files.ContainsKey(Path2));
        Assert.True(store.TryRestore(Slot2, out BankState restored));
        Assert.Equal("thrown", restored.Transactions[0].Id);
    }

    private static HistoryPersistence Bind(FakeHistoryIo io, string slot, string path)
    {
        var store = new HistoryPersistence((file, state) => io.Write(file, state), io.LogError, io.LogInfo);
        store.Bind(slot, path);
        return store;
    }

    private static BankState Receipt(string id, float weekly)
    {
        return new BankState
        {
            WeeklyDepositedAmount = weekly,
            LastRecordedWeek = 3,
            Transactions = new List<BankTransaction>
            {
                new()
                {
                    Id = id,
                    Amount = 12f,
                    Type = TransactionType.Deposit,
                    Description = "Mobile Cash Deposit",
                    Gross = 12f,
                    Fee = 0f
                }
            }
        };
    }

    private sealed class FakeHistoryIo
    {
        public bool FailAll { get; set; }
        public bool Throw { get; set; }
        public Func<string, bool>? Fail { get; set; }
        public Dictionary<string, BankState> Files { get; } = new(StringComparer.Ordinal);
        public List<Attempt> Attempts { get; } = new();
        public List<string> ErrorLog { get; } = new();

        public bool Write(string path, BankState state)
        {
            Attempts.Add(new Attempt(path, state.Transactions.Count == 0 ? string.Empty : state.Transactions[0].Id));
            if (Throw)
                throw new IOException("locked");
            if (FailAll || (Fail != null && Fail(path)))
                return false;
            Files[path] = state;
            return true;
        }

        public int WritesTo(string path) => Attempts.Count(attempt => attempt.Path == path);

        public void LogError(string message) => ErrorLog.Add(message);

        public void LogInfo(string message)
        {
        }
    }

    private readonly record struct Attempt(string Path, string Id);
}

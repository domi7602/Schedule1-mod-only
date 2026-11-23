using System;
using System.Collections.Generic;
using BankApp.Models;

namespace BankApp.Logic;

/// <summary>
/// Slot-bound save policy for the bank receipt file. Unity-free so a failed write,
/// a later success and a slot change can be tested without the game.
///
/// The cash and bank movement is not this type's job. A snapshot stays on the path
/// captured for its slot. Save reports success only when the writer returns true.
/// A later slot change cannot retarget that snapshot. Automatic retries happen once
/// when the slot is left, not on a frame tick.
/// </summary>
public sealed class HistoryPersistence
{
    public const string FailureStatus = "History not saved. The transfer already happened.";

    private readonly Func<string, BankState, bool> _write;
    private readonly Action<string>? _logError;
    private readonly Action<string>? _logInfo;
    private readonly Dictionary<string, string> _paths = new(StringComparer.Ordinal);
    private readonly List<Pending> _pending = new();

    public HistoryPersistence(
        Func<string, BankState, bool> write,
        Action<string>? logError = null,
        Action<string>? logInfo = null)
    {
        _write = write ?? throw new ArgumentNullException(nameof(write));
        _logError = logError;
        _logInfo = logInfo;
    }

    public string? PathFor(string slot)
        => !string.IsNullOrWhiteSpace(slot) && _paths.TryGetValue(slot, out string? path) ? path : null;

    public bool HasPending(string slot)
    {
        if (string.IsNullOrWhiteSpace(slot))
            return false;
        for (int i = 0; i < _pending.Count; i++)
        {
            if (string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                return true;
        }

        return false;
    }

    public string? StatusFor(string? slot)
        => HasPending(slot ?? string.Empty) ? FailureStatus : null;

    /// <summary>
    /// Remembers <paramref name="path"/> for <paramref name="slot"/>. A slot that already
    /// has an unsaved snapshot keeps the path captured for that attempt.
    /// </summary>
    public void Bind(string slot, string path)
    {
        if (string.IsNullOrWhiteSpace(slot) || string.IsNullOrWhiteSpace(path))
            return;
        if (HasPending(slot) && _paths.ContainsKey(slot))
            return;
        _paths[slot] = path;
    }

    /// <summary>
    /// Writes <paramref name="state"/> only to the path already captured for
    /// <paramref name="slot"/>. A missing path is a failure, not a write to another file.
    /// </summary>
    public bool Save(string slot, BankState state)
    {
        state ??= new BankState();
        if (string.IsNullOrWhiteSpace(slot) || !_paths.TryGetValue(slot, out string? path))
        {
            _logError?.Invoke(
                "History save skipped: slot '" + slot
                + "' has no bound path. History kept; it will not be written to another slot.");
            return false;
        }

        if (Write(path, state, slot))
        {
            RemovePending(slot);
            return true;
        }

        Upsert(slot, path, state, resetAttempt: true);
        return false;
    }

    /// <summary>
    /// One explicit pass over receipts that are already pending. A clean slot is not
    /// written, so closing the phone cannot invent a failure. Not a frame retry.
    /// </summary>
    public void FlushOnce(string? slot, BankState? state)
    {
        if (!string.IsNullOrWhiteSpace(slot)
            && state != null
            && HasPending(slot)
            && _paths.TryGetValue(slot, out string? path))
        {
            Upsert(slot, path, state, resetAttempt: true);
        }

        TryFlush(skipAlreadyAttempted: false);
    }

    /// <summary>
    /// Updates a snapshot that is already pending, then tries each pending file once.
    /// A clean slot is not written. A later leave does not write that file again.
    /// </summary>
    public void LeaveSlot(string slot, BankState? state)
    {
        if (!string.IsNullOrWhiteSpace(slot) && state != null && HasPending(slot) && _paths.TryGetValue(slot, out string? path))
            Upsert(slot, path, state, resetAttempt: false);

        TryFlush(skipAlreadyAttempted: true);
    }

    public bool TryRestore(string slot, out BankState state)
    {
        for (int i = 0; i < _pending.Count; i++)
        {
            if (!string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                continue;
            state = _pending[i].State;
            return true;
        }

        state = new BankState();
        return false;
    }

    private bool Write(string path, BankState state, string slot)
    {
        bool ok;
        try
        {
            ok = _write(path, Clone(state));
        }
        catch (Exception ex)
        {
            _logError?.Invoke(
                "History save failed for slot '" + slot + "' at '" + path + "': " + ex.Message
                + " History kept; it will not be written to another slot.");
            return false;
        }

        if (ok)
        {
            int count = state.Transactions?.Count ?? 0;
            _logInfo?.Invoke("Saved bank history for slot '" + slot + "' (" + count + " transactions).");
            return true;
        }

        _logError?.Invoke(
            "History save failed for slot '" + slot + "' at '" + path
            + "'. History kept; it will not be written to another slot. The transfer already happened.");
        return false;
    }

    private void Upsert(string slot, string path, BankState state, bool resetAttempt)
    {
        for (int i = 0; i < _pending.Count; i++)
        {
            if (!string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                continue;
            if (!string.IsNullOrEmpty(path))
                _pending[i].Path = path;
            _pending[i].State = state;
            if (resetAttempt)
                _pending[i].AutoAttempted = false;
            return;
        }

        if (string.IsNullOrEmpty(path))
            return;
        _pending.Add(new Pending(slot, path, state));
    }

    private void RemovePending(string slot)
    {
        for (int i = _pending.Count - 1; i >= 0; i--)
        {
            if (string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                _pending.RemoveAt(i);
        }
    }

    private void TryFlush(bool skipAlreadyAttempted)
    {
        for (int i = _pending.Count - 1; i >= 0; i--)
        {
            Pending item = _pending[i];
            if (skipAlreadyAttempted && item.AutoAttempted)
                continue;
            if (skipAlreadyAttempted)
                item.AutoAttempted = true;
            if (Write(item.Path, item.State, item.Slot))
                _pending.RemoveAt(i);
        }
    }

    private static BankState Clone(BankState state)
    {
        var copy = new BankState
        {
            WeeklyDepositedAmount = state.WeeklyDepositedAmount,
            LastRecordedWeek = state.LastRecordedWeek,
            Transactions = new List<BankTransaction>()
        };
        if (state.Transactions == null)
            return copy;

        for (int i = 0; i < state.Transactions.Count; i++)
        {
            BankTransaction? tx = state.Transactions[i];
            if (tx == null)
                continue;
            copy.Transactions.Add(new BankTransaction
            {
                Id = tx.Id,
                InGameDay = tx.InGameDay,
                InGameTime = tx.InGameTime ?? string.Empty,
                Amount = tx.Amount,
                Type = tx.Type,
                Description = tx.Description ?? string.Empty,
                BalanceAfter = tx.BalanceAfter,
                TimestampEpoch = tx.TimestampEpoch,
                Gross = tx.Gross,
                Fee = tx.Fee
            });
        }

        return copy;
    }

    private sealed class Pending
    {
        public Pending(string slot, string path, BankState state)
        {
            Slot = slot;
            Path = path;
            State = state;
        }

        public string Slot { get; }
        public string Path { get; set; }
        public BankState State { get; set; }
        public bool AutoAttempted { get; set; }
    }
}

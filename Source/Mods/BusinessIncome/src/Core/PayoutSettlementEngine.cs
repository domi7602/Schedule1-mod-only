using System;
using System.Collections.Generic;
using BusinessIncome.Models;

namespace BusinessIncome.Core;

/// <summary>
/// Pure, seam-driven settlement orchestrator. Encodes the money-safety invariants:
/// <list type="bullet">
/// <item>mutations require a known active slot AND resolved load readiness;</item>
/// <item>a valid or corrupt pending marker blocks ALL new automatic/manual force booking;</item>
/// <item>the write-ahead marker must be written durably BEFORE the bank call;</item>
/// <item>any exception after the bank API was invoked leaves the marker in place and
/// blocks retry (outcome UNKNOWN);</item>
/// <item>a clean return is "request accepted", never claimed as confirmed settlement;</item>
/// <item>the marker is cleared only after a successful forward state commit;</item>
/// <item>re-entrant booking attempts are rejected.</item>
/// </list>
/// </summary>
public sealed class PayoutSettlementEngine
{
    private readonly IPayoutStorage _storage;
    private readonly IPayoutBank? _bank;
    private readonly PayoutLog? _log;

    private bool _booking;
    private PayoutState? _state;
    private StateLoadResult _stateLoad = new(StateStatus.Missing, null, "not loaded");
    private PendingLoadResult _pendingLoad = new(PendingStatus.Missing, null, "not loaded");
    private PayoutSnapshot? _snapshot;
    private string? _snapshotSlot;
    private PendingPayoutState? _snapshotMarker;

    public PayoutSettlementEngine(IPayoutStorage storage, IPayoutBank? bank = null, PayoutLog? log = null)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _bank = bank;
        _log = log;
    }

    public LoadReadiness Readiness { get; private set; } = LoadReadiness.NotResolved;
    public string? ActiveSlot { get; private set; }
    public SlotAuthority Authority => string.IsNullOrEmpty(ActiveSlot) ? SlotAuthority.Unknown : SlotAuthority.Known;
    public bool IsBooking => _booking;
    public PayoutState? State => _state;
    public StateLoadResult StateLoad => _stateLoad;
    public PendingLoadResult PendingLoad => _pendingLoad;
    public PayoutSnapshot? LastSnapshot => _snapshot;

    /// <summary>
    /// Readiness resets at PreLoad and at scene unload; a mutation is blocked until resolved.
    /// The active slot is also cleared so a transition can never write to the previous slot.
    /// </summary>
    public void ResetForLoad()
    {
        Readiness = LoadReadiness.NotResolved;
        ActiveSlot = null;
        // A slot transition must never carry state, cache or a rollback snapshot across.
        _state = null;
        _stateLoad = new StateLoadResult(StateStatus.Missing, null, "reset for load");
        _pendingLoad = new PendingLoadResult(PendingStatus.Missing, null, "reset for load");
        _snapshot = null;
        _snapshotSlot = null;
        _snapshotMarker = null;
    }

    /// <summary>Explicitly resolves the active slot without touching readiness.</summary>
    public void SetActiveSlot(string? slot)
    {
        if (!string.IsNullOrEmpty(slot)) ActiveSlot = slot;
    }

    /// <summary>
    /// Resolves readiness on LoadComplete and re-checks the pending marker here regardless of
    /// SaveInfoLoaded. Sets the slot when provided.
    /// </summary>
    public void ResolveOnLoadComplete(string? slot)
    {
        if (!string.IsNullOrEmpty(slot)) ActiveSlot = slot;
        Readiness = LoadReadiness.Resolved;
    }

    public StateLoadResult LoadState(string path)
    {
        if (Authority == SlotAuthority.Unknown)
        {
            _stateLoad = new StateLoadResult(StateStatus.Missing, null, "active slot unknown — state not read");
            _state = null;
            return _stateLoad;
        }
        _stateLoad = PayoutCodec.DecodeState(_storage, path, ActiveSlot!);
        _state = _stateLoad.State;
        return _stateLoad;
    }

    public PendingLoadResult LoadPending(string path)
    {
        if (Authority == SlotAuthority.Unknown)
        {
            _pendingLoad = new PendingLoadResult(PendingStatus.Missing, null, "active slot unknown — marker not read");
            return _pendingLoad;
        }
        _pendingLoad = PayoutCodec.DecodePending(_storage, path);
        return _pendingLoad;
    }

    /// <summary>
    /// Forces the engine into a fail-closed mutation-blocked state (e.g. an ambiguous legacy
    /// migration was detected). No seed and no bank call may happen until the operator fixes
    /// the on-disk data.
    /// </summary>
    public void BlockMutations(string reason)
    {
        _state = null;
        _stateLoad = new StateLoadResult(StateStatus.Corrupt, null, reason);
    }

    /// <summary>
    /// Explicit initialization of GENUINELY MISSING state only. Guards authority, resolved
    /// readiness, any blocking state and any pending marker, then materializes a fresh seed
    /// in memory ONLY after the durable write succeeds. An existing state is never overwritten
    /// and a failed write never advances memory.
    /// </summary>
    public PayoutResult Initialize(int day, string statePath)
    {
        if (_booking)
            return new PayoutResult(PayoutOutcome.Reentrant, day, 0f, 0, "re-entrant operation rejected");

        _booking = true;
        try
        {
            if (Authority == SlotAuthority.Unknown)
                return new PayoutResult(PayoutOutcome.NotAuthoritative, day, 0f, 0, "active slot unknown — no write");
            if (Readiness != LoadReadiness.Resolved)
                return new PayoutResult(PayoutOutcome.NotReady, day, 0f, 0, "load readiness not resolved");
            if (_stateLoad.BlocksMutation || _stateLoad.Status == StateStatus.RecoveredFromBackup)
                return new PayoutResult(PayoutOutcome.BlockedStateCorrupt, day, 0f, 0, _stateLoad.Detail);
            if (_pendingLoad.BlocksBooking)
                return new PayoutResult(PayoutOutcome.BlockedPending, day, 0f, 0, "pending payout unresolved — no seed");
            if (day < 0)
                return new PayoutResult(PayoutOutcome.InvalidInput, day, 0f, 0, "invalid negative day");
            // Only a genuinely missing state may be seeded; anything else is used as-is.
            if (_stateLoad.Status != StateStatus.Missing)
                return new PayoutResult(PayoutOutcome.Skipped, day, 0f, 0, "state already present — no fresh seed");

            var fresh = new PayoutState
            {
                SchemaVersion = PayoutCodec.CurrentSchemaVersion,
                SaveIdentity = ActiveSlot ?? "",
                LastPaidElapsedDay = day,
                InitializationDay = day,
                LastPaidDayByBusiness = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            };
            if (!_storage.WriteAtomic(statePath, PayoutCodec.EncodeState(fresh)))
                return new PayoutResult(PayoutOutcome.StateNotPersisted, day, 0f, 0, "initialization not persisted — no seed");

            // Durable commit reached: only now does memory advance.
            _state = fresh;
            _stateLoad = new StateLoadResult(StateStatus.Ok, fresh, "initialized");
            return new PayoutResult(PayoutOutcome.Requested, day, 0f, 0, "fresh state initialized");
        }
        finally
        {
            _booking = false;
        }
    }

    /// <summary>Dry-run preview: no bank call, no storage write, no host/readiness requirement.</summary>
    public PayoutResult Preview(int day, float net, int businessCount)
        => new(PayoutOutcome.DryRun, day, net, businessCount, "dry-run preview only");

    /// <summary>
    /// Attempts a real booking. See class remarks for the ordering guarantees.
    /// </summary>
    public PayoutResult TryBook(
        int day,
        float net,
        IReadOnlyList<string> businessIds,
        string statePath,
        string pendingPath,
        bool force)
    {
        if (_booking)
            return new PayoutResult(PayoutOutcome.Reentrant, day, net, businessIds.Count, "re-entrant booking rejected");

        _booking = true;
        try
        {
            int count = businessIds.Count;

            if (Authority == SlotAuthority.Unknown)
                return new PayoutResult(PayoutOutcome.NotAuthoritative, day, net, count, "active slot unknown — no write");
            if (Readiness != LoadReadiness.Resolved)
                return new PayoutResult(PayoutOutcome.NotReady, day, net, count, "load readiness not resolved");
            if (_stateLoad.BlocksMutation)
                return new PayoutResult(PayoutOutcome.BlockedStateCorrupt, day, net, count, _stateLoad.Detail);
            if (_state == null)
                return new PayoutResult(PayoutOutcome.NotReady, day, net, count, "state not loaded");

            // Fail closed on invalid inputs BEFORE any mutation or bank call.
            if (day < 0)
                return new PayoutResult(PayoutOutcome.InvalidInput, day, net, count, "invalid negative day");
            if (!float.IsFinite(net))
                return new PayoutResult(PayoutOutcome.InvalidInput, day, net, count, "non-finite net amount");

            // A valid, recovered or corrupt pending marker blocks every new booking, forced
            // included, and is never overwritten.
            if (_pendingLoad.BlocksBooking)
            {
                string markerDesc = _pendingLoad.Marker != null ? $"day {_pendingLoad.Marker.Day}" : "unreadable";
                return new PayoutResult(PayoutOutcome.BlockedPending, day, net, count,
                    $"pending payout ({markerDesc}) unresolved");
            }

            if (!force && _state.LastPaidElapsedDay >= day)
                return new PayoutResult(PayoutOutcome.AlreadyPaid, day, net, count, "already paid");

            // Zero/no-business days are terminal: persist forward so catch-up cannot loop.
            if (count == 0)
                return CommitTerminal(PayoutOutcome.NoBusinesses, day, net, businessIds, statePath);
            if (!float.IsFinite(net) || net <= 0f)
                return CommitTerminal(PayoutOutcome.ZeroNet, day, net, businessIds, statePath);

            if (_bank == null || !_bank.IsReady())
                return new PayoutResult(PayoutOutcome.NotReady, day, net, count, "money manager not ready — no bank call");

            // Snapshot BEFORE any bank call so a pre-call failure can be rolled back exactly.
            PayoutSnapshot snap = PayoutLedger.CaptureAndMark(_state, day, businessIds);

            // Write the marker durably BEFORE the bank call; a failed marker write blocks the bank.
            var marker = new PendingPayoutState
            {
                Day = day,
                Amount = net,
                BusinessCount = count,
                StartedUtc = DateTime.UtcNow.ToString("o")
            };
            if (!_storage.WriteAtomic(pendingPath, PayoutCodec.EncodePending(marker)))
            {
                PayoutLedger.TryRollbackExact(_state, snap);
                return new PayoutResult(PayoutOutcome.BlockedMarkerWrite, day, net, count, "marker write failed — bank blocked");
            }
            _pendingLoad = new PendingLoadResult(PendingStatus.Valid, marker, "marker written");
            _snapshot = snap;
            _snapshotSlot = ActiveSlot;
            _snapshotMarker = marker;

            bool accepted;
            try
            {
                accepted = _bank.RequestTransfer("Business Revenue", net,
                    $"Daily Revenue ({count} businesses): +${net:0}");
            }
            catch (Exception ex)
            {
                // Mutate-then-throw: the outcome is UNKNOWN. Keep marker AND state; never retry.
                _log?.Invoke($"[Payout] bank call threw after invocation — outcome UNKNOWN, marker kept: {ex.Message}");
                return new PayoutResult(PayoutOutcome.UnknownOutcome, day, net, count, "bank call threw — outcome unknown, retry blocked");
            }

            if (!accepted)
            {
                // Clean "not accepted" (e.g. missing manager): no mutation happened.
                // Roll back exactly, then clear the marker — but stay BLOCKED if the clear fails.
                PayoutLedger.TryRollbackExact(_state, snap);
                bool rejectedCleared = PayoutCodec.ClearMarker(_storage, pendingPath);
                _snapshot = null;
                _snapshotSlot = null;
                _snapshotMarker = null;
                if (!rejectedCleared)
                {
                    _pendingLoad = new PendingLoadResult(PendingStatus.Valid, marker, "bank rejected but marker clear failed — kept");
                    return new PayoutResult(PayoutOutcome.MarkerClearFailed, day, net, count, "bank rejected; marker not cleared — stays blocked");
                }
                _pendingLoad = new PendingLoadResult(PendingStatus.Missing, null, "bank rejected request; marker cleared");
                return new PayoutResult(PayoutOutcome.Skipped, day, net, count, "bank did not accept the request");
            }

            // Request accepted. Persist the forward state; on failure keep the marker (the
            // snapshot stays armed so a later resolve can roll the un-durable memory back).
            if (!_storage.WriteAtomic(statePath, PayoutCodec.EncodeState(_state)))
            {
                return new PayoutResult(PayoutOutcome.BookedStateNotSaved, day, net, count,
                    "request accepted but state not saved — marker kept, resolve with 'biz pending'");
            }

            // The day is now durable. Nothing may roll it back in memory: drop the snapshot
            // BEFORE touching the marker so a failed clear can never resurrect an old day.
            _snapshot = null;
            _snapshotSlot = null;
            _snapshotMarker = null;

            bool cleared = PayoutCodec.ClearMarker(_storage, pendingPath);
            _pendingLoad = cleared
                ? new PendingLoadResult(PendingStatus.Missing, null, "committed; marker cleared")
                : new PendingLoadResult(PendingStatus.Valid, marker, "committed but marker clear failed");
            _log?.Invoke($"[Payout] day {day}: request accepted, state committed{(cleared ? "" : " (marker clear FAILED — kept)")}.");
            return new PayoutResult(PayoutOutcome.Requested, day, net, count,
                cleared ? "request accepted; state committed" : "request accepted; state committed; marker clear failed");
        }
        finally
        {
            _booking = false;
        }
    }

    private PayoutResult CommitTerminal(
        PayoutOutcome successOutcome,
        int day,
        float net,
        IReadOnlyList<string> businessIds,
        string statePath)
    {
        // Persist forward so catch-up cannot loop — but NEVER advance memory on a failed write.
        PayoutSnapshot snap = PayoutLedger.CaptureAndMark(_state!, day, businessIds);
        if (!_storage.WriteAtomic(statePath, PayoutCodec.EncodeState(_state!)))
        {
            PayoutLedger.TryRollbackExact(_state!, snap);
            return new PayoutResult(PayoutOutcome.StateNotPersisted, day, net, businessIds.Count, "terminal day not persisted — memory rolled back");
        }
        return new PayoutResult(successOutcome, day, net, businessIds.Count, "terminal day persisted");
    }

    /// <summary>
    /// Forward-only state commit with NO bank call (fresh-state seed, catch-up cap, terminal
    /// day). Still requires authority + resolved readiness, and the persistence result is
    /// checked by the caller.
    /// </summary>
    public PayoutResult CommitOnly(int day, IReadOnlyList<string> businessIds, string statePath)
    {
        if (_booking) return new PayoutResult(PayoutOutcome.Reentrant, day, 0f, businessIds.Count, "re-entrant");
        _booking = true;
        try
        {
            if (Authority == SlotAuthority.Unknown)
                return new PayoutResult(PayoutOutcome.NotAuthoritative, day, 0f, businessIds.Count, "active slot unknown — no write");
            if (Readiness != LoadReadiness.Resolved)
                return new PayoutResult(PayoutOutcome.NotReady, day, 0f, businessIds.Count, "load readiness not resolved");
            if (_stateLoad.BlocksMutation)
                return new PayoutResult(PayoutOutcome.BlockedStateCorrupt, day, 0f, businessIds.Count, _stateLoad.Detail);
            if (_state == null)
                return new PayoutResult(PayoutOutcome.NotReady, day, 0f, businessIds.Count, "state not loaded");

            if (day < 0)
                return new PayoutResult(PayoutOutcome.InvalidInput, day, 0f, businessIds.Count, "invalid negative day");

            // A terminal/cap commit is still a state mutation: it must not advance past a day
            // whose payout is unresolved. Any valid/recovered/corrupt marker blocks it.
            if (_pendingLoad.BlocksBooking)
            {
                string markerDesc = _pendingLoad.Marker != null ? $"day {_pendingLoad.Marker.Day}" : "unreadable";
                return new PayoutResult(PayoutOutcome.BlockedPending, day, 0f, businessIds.Count, $"pending payout ({markerDesc}) unresolved");
            }

            // Never advance memory on a failed write: capture, mark, and roll back exactly.
            PayoutSnapshot snap = PayoutLedger.CaptureAndMark(_state, day, businessIds);
            if (!_storage.WriteAtomic(statePath, PayoutCodec.EncodeState(_state)))
            {
                PayoutLedger.TryRollbackExact(_state, snap);
                return new PayoutResult(PayoutOutcome.StateNotPersisted, day, 0f, businessIds.Count, "state not persisted — memory rolled back");
            }
            return new PayoutResult(PayoutOutcome.Requested, day, 0f, businessIds.Count, "state committed (no bank)");
        }
        finally { _booking = false; }
    }

    /// <summary>
    /// User confirms the pending payout was received. Commits the state forward and clears
    /// the marker ONLY after a successful commit; a stale/unknown marker is not auto-cleared.
    /// </summary>
    public PayoutResult ConfirmPending(string statePath, string pendingPath)
    {
        if (_booking) return new PayoutResult(PayoutOutcome.Reentrant, -1, 0f, 0, "re-entrant");
        _booking = true;
        try
        {
            if (Authority == SlotAuthority.Unknown)
                return new PayoutResult(PayoutOutcome.NotAuthoritative, -1, 0f, 0, "active slot unknown");
            if (Readiness != LoadReadiness.Resolved)
                return new PayoutResult(PayoutOutcome.NotReady, -1, 0f, 0, "load readiness not resolved");
            if (_stateLoad.BlocksMutation)
                return new PayoutResult(PayoutOutcome.BlockedStateCorrupt, -1, 0f, 0, _stateLoad.Detail);

            PendingPayoutState? marker = _pendingLoad.Marker;
            if (_pendingLoad.Status == PendingStatus.Missing || marker == null)
                return new PayoutResult(PayoutOutcome.Skipped, -1, 0f, 0, "no pending marker");
            if (_state == null)
                return new PayoutResult(PayoutOutcome.NotReady, -1, 0f, 0, "state not loaded");

            int target = Math.Max(_state.LastPaidElapsedDay, marker.Day);
            PayoutLedger.MarkPaid(_state, target, Array.Empty<string>());
            if (!_storage.WriteAtomic(statePath, PayoutCodec.EncodeState(_state)))
                return new PayoutResult(PayoutOutcome.StateNotPersisted, marker.Day, marker.Amount, marker.BusinessCount, "forward commit failed — marker kept");

            // Durable commit reached: the old pre-call snapshot can never be applied afterwards.
            _snapshot = null;
            _snapshotSlot = null;
            _snapshotMarker = null;

            bool cleared = PayoutCodec.ClearMarker(_storage, pendingPath);
            if (!cleared)
            {
                _pendingLoad = new PendingLoadResult(PendingStatus.Valid, marker, "confirmed but clear failed — kept");
                return new PayoutResult(PayoutOutcome.MarkerClearFailed, target, marker.Amount, marker.BusinessCount, "confirmed but marker not cleared — stays blocked");
            }
            _pendingLoad = new PendingLoadResult(PendingStatus.Missing, null, "confirmed; marker cleared");
            return new PayoutResult(PayoutOutcome.PendingConfirmed, target, marker.Amount, marker.BusinessCount, "confirmed and cleared");
        }
        finally { _booking = false; }
    }

    /// <summary>
    /// Explicit user choice that the money never arrived: restore the pre-call snapshot when
    /// it is still exactly matching in this session, then clear the marker so it can pay again.
    /// </summary>
    public PayoutResult ResolvePending(string pendingPath)
    {
        if (_booking) return new PayoutResult(PayoutOutcome.Reentrant, -1, 0f, 0, "re-entrant");
        _booking = true;
        try
        {
            if (Authority == SlotAuthority.Unknown)
                return new PayoutResult(PayoutOutcome.NotAuthoritative, -1, 0f, 0, "active slot unknown");
            if (Readiness != LoadReadiness.Resolved)
                return new PayoutResult(PayoutOutcome.NotReady, -1, 0f, 0, "load readiness not resolved");
            // Resolve also mutates state on rollback; a corrupt/unusable state blocks it.
            if (_stateLoad.BlocksMutation)
                return new PayoutResult(PayoutOutcome.BlockedStateCorrupt, -1, 0f, 0, _stateLoad.Detail);

            PendingPayoutState? marker = _pendingLoad.Marker;
            if (_pendingLoad.Status == PendingStatus.Missing || marker == null)
                return new PayoutResult(PayoutOutcome.Skipped, -1, 0f, 0, "no pending marker");

            // The snapshot may only be applied when it belongs to THIS session, THIS slot AND
            // THIS exact pending marker. A snapshot from an earlier, already-superseded marker
            // must never roll back the day (monotonicity across slot reloads).
            bool restored = false;
            if (_state != null && _snapshot != null
                && string.Equals(_snapshotSlot, ActiveSlot, StringComparison.OrdinalIgnoreCase)
                && _snapshotMarker != null && SameMarker(_snapshotMarker, marker))
            {
                restored = PayoutLedger.TryRollbackExact(_state, _snapshot);
            }
            _snapshot = null;
            _snapshotSlot = null;
            _snapshotMarker = null;

            bool cleared = PayoutCodec.ClearMarker(_storage, pendingPath);
            if (!cleared)
            {
                _pendingLoad = new PendingLoadResult(PendingStatus.Valid, marker, "resolved but clear failed — kept");
                return new PayoutResult(PayoutOutcome.MarkerClearFailed, marker.Day, marker.Amount, marker.BusinessCount, "resolve failed to clear marker — stays blocked");
            }
            _pendingLoad = new PendingLoadResult(PendingStatus.Missing, null, "resolved; marker cleared");
            return new PayoutResult(PayoutOutcome.PendingResolved, marker.Day, marker.Amount, marker.BusinessCount,
                restored ? "snapshot restored; marker cleared" : "marker cleared (no matching snapshot restored)");
        }
        finally { _booking = false; }
    }

    /// <summary>Identity of a write-ahead marker: same day, same business count, same start time.</summary>
    private static bool SameMarker(PendingPayoutState a, PendingPayoutState b) =>
        a.Day == b.Day && a.BusinessCount == b.BusinessCount
        && string.Equals(a.StartedUtc, b.StartedUtc, StringComparison.Ordinal);
}

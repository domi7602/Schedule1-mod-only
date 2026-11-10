using System;
using System.Collections.Generic;
using System.Linq;

namespace TownHallInterior;

/// <summary>
/// Status of the laundering terminal in the upper floor office.
/// </summary>
public enum LaunderingTerminalState
{
    LockedUnowned,
    Ready,
    Validating,
    CapacityExceeded,
    InsufficientCash,
    Depositing,
    Processing,
    Completed,
    Refunding,
    Refunded,
    Faulted
}

/// <summary>
/// Immutable receipt of a laundering deposit or settlement.
/// </summary>
public sealed record LaunderingOperationReceipt(
    string OperationId,
    float Amount,
    DateTime TimestampUtc,
    bool Success,
    string? StatusMessage = null,
    bool RequiresReconciliation = false);

/// <summary>
/// Port abstraction for interacting with engine laundering mechanics and cash drawers.
/// </summary>
public interface ILaunderingPort
{
    float GetLaunderCapacity();
    float GetCurrentLaunderTotal();
    float GetPlayerCash();
    MoneyMutationOutcome DeductCash(float amount);
    MoneyMutationOutcome RefundCash(float amount);
    bool StartNativeLaundering(float amount);
    MoneyMutationOutcome PayoutCleanFunds(float amount, string label, string note);
}

/// <summary>
/// Coordinates laundering operations from the Town Hall administrative terminal,
/// ensuring capacity limits, ownership gates, and atomic compensating rollbacks.
/// </summary>
public sealed class BusinessLaunderingManager
{
    private readonly object _sync = new();
    private readonly BusinessPurchaseManager _purchaseManager;
    private readonly float _capacity;
    private readonly int _durationMinutes;
    private readonly List<ActiveLaunderingOperationRecord> _activeOperations = new();
    private LaunderingTerminalState _state = LaunderingTerminalState.Ready;
    private LaunderingOperationReceipt? _unresolvedDeposit;
    private bool _isMutating;

    public const float DefaultCapacity = 20_000f;
    public const int DefaultDurationMinutes = 1440;
    public const int MaxPayoutAttempts = 3;

    public BusinessLaunderingManager(
        BusinessPurchaseManager purchaseManager,
        float capacity = DefaultCapacity,
        int durationMinutes = DefaultDurationMinutes)
    {
        ArgumentNullException.ThrowIfNull(purchaseManager);
        if (float.IsNaN(capacity) || float.IsInfinity(capacity) || capacity <= 0f)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be a positive finite amount.");
        if (durationMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationMinutes), "Duration must be positive.");

        _purchaseManager = purchaseManager;
        _capacity = capacity;
        _durationMinutes = durationMinutes;
    }

    public float Capacity => _capacity;
    public int DurationMinutes => _durationMinutes;
    public bool IsMutating
    {
        get
        {
            lock (_sync) return _isMutating;
        }
    }

    public LaunderingTerminalState State
    {
        get
        {
            lock (_sync) return _state;
        }
    }

    public float PendingAmount
    {
        get
        {
            lock (_sync)
            {
                float sum = 0f;
                for (int i = 0; i < _activeOperations.Count; i++)
                    sum += _activeOperations[i].Amount;
                return sum;
            }
        }
    }

    public int RemainingMinutes
    {
        get
        {
            lock (_sync)
            {
                return _activeOperations.Count == 0 ? 0 : _activeOperations.Min(op => op.RemainingMinutes);
            }
        }
    }

    public IReadOnlyList<ActiveLaunderingOperationRecord> GetActiveOperationsSnapshot()
    {
        lock (_sync)
        {
            return _activeOperations.Select(CloneOperation).ToList().AsReadOnly();
        }
    }

    public void RestoreActiveOperations(IEnumerable<ActiveLaunderingOperationRecord>? records, ILaunderingPort? port = null)
    {
        lock (_sync)
        {
            if (_isMutating)
                throw new InvalidOperationException("Cannot restore jobs during a wallet mutation.");

            var restored = new List<ActiveLaunderingOperationRecord>();
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var record in records ?? Array.Empty<ActiveLaunderingOperationRecord>())
            {
                if (record == null || string.IsNullOrWhiteSpace(record.OperationId) || !ids.Add(record.OperationId) ||
                    !float.IsFinite(record.Amount) || record.Amount <= 0f ||
                    record.TotalDurationMinutes <= 0 || record.RemainingMinutes < 0 ||
                    record.RemainingMinutes > record.TotalDurationMinutes ||
                    record.PayoutAttempts < 0 || record.PayoutAttempts > MaxPayoutAttempts ||
                    (record.PayoutAttempts > 0 && record.RemainingMinutes != 0) ||
                    (record.PayoutUnresolved && record.PayoutAttempts == 0))
                {
                    throw new ArgumentException("Invalid saved laundering job.", nameof(records));
                }

                restored.Add(CloneOperation(record));
            }

            foreach (var job in restored)
            {
                if (port?.StartNativeLaundering(job.Amount) == false)
                    throw new InvalidOperationException("Could not restore the local laundering reservation.");
            }

            _activeOperations.Clear();
            _activeOperations.AddRange(restored);
            _unresolvedDeposit = null;
            _state = restored.Any(op => op.PayoutUnresolved || op.PayoutAttempts >= MaxPayoutAttempts)
                ? LaunderingTerminalState.Faulted
                : restored.Count > 0 ? LaunderingTerminalState.Processing : LaunderingTerminalState.Ready;
        }
    }

    public float GetAvailableCapacity(ILaunderingPort port)
    {
        ArgumentNullException.ThrowIfNull(port);
        float current = port.GetCurrentLaunderTotal();
        float cap = port.GetLaunderCapacity();
        if (!float.IsFinite(cap) || cap <= 0f) cap = _capacity;
        return float.IsFinite(current) && current >= 0f ? MathF.Max(0f, cap - current) : 0f;
    }

    public LaunderingOperationReceipt TryStartLaundering(
        ConstructionContext? context,
        ILaunderingPort port,
        float amount,
        string targetOrg = "TownHallTest",
        string targetSlot = "SaveGame_2")
    {
        ArgumentNullException.ThrowIfNull(port);

        lock (_sync)
        {
            string opId = Guid.NewGuid().ToString("N");

            if (_isMutating)
                return new LaunderingOperationReceipt(opId, amount, DateTime.UtcNow, false, "blocked:transaction-in-progress");

            if (_unresolvedDeposit != null)
                return _unresolvedDeposit;

            if (!_purchaseManager.IsOwned)
            {
                _state = LaunderingTerminalState.LockedUnowned;
                return new LaunderingOperationReceipt(
                    opId, amount, DateTime.UtcNow, false, "locked:unowned");
            }

            string? gateError = ConstructionGate.Check(context, targetOrg, targetSlot);
            if (gateError != null)
            {
                return new LaunderingOperationReceipt(
                    opId, amount, DateTime.UtcNow, false, $"skipped:{gateError}");
            }

            if (!float.IsFinite(amount) || amount <= 0f)
            {
                return new LaunderingOperationReceipt(
                    opId, amount, DateTime.UtcNow, false, "invalid-amount");
            }

            _isMutating = true;
            try
            {
                return StartDeposit(port, amount, opId);
            }
            finally
            {
                _isMutating = false;
            }
        }
    }

    private LaunderingOperationReceipt StartDeposit(ILaunderingPort port, float amount, string opId)
    {
        _state = LaunderingTerminalState.Validating;
        if (amount > GetAvailableCapacity(port))
        {
            _state = LaunderingTerminalState.CapacityExceeded;
            return new LaunderingOperationReceipt(opId, amount, DateTime.UtcNow, false, "capacity-exceeded");
        }

        float playerCash = port.GetPlayerCash();
        if (!float.IsFinite(playerCash) || playerCash < amount)
        {
            _state = LaunderingTerminalState.InsufficientCash;
            return new LaunderingOperationReceipt(opId, amount, DateTime.UtcNow, false, "insufficient-cash");
        }

        _state = LaunderingTerminalState.Depositing;
        MoneyMutationOutcome deduction;
        try
        {
            deduction = port.DeductCash(amount);
        }
        catch
        {
            deduction = MoneyMutationOutcome.Unknown;
        }
        if (deduction == MoneyMutationOutcome.NotApplied)
        {
            _state = LaunderingTerminalState.Faulted;
            return new LaunderingOperationReceipt(
                opId, amount, DateTime.UtcNow, false, "cash-deduct-failed");
        }
        if (deduction != MoneyMutationOutcome.Applied)
            return HoldDeposit(opId, amount, "cash-deduct-unresolved");

        bool started;
        try
        {
            started = port.StartNativeLaundering(amount);
        }
        catch (Exception ex)
        {
            return CompensateRefund(port, opId, amount, $"native-exception:{ex.Message}");
        }
        if (!started)
            return CompensateRefund(port, opId, amount, "native-start-failed");

        _activeOperations.Add(new ActiveLaunderingOperationRecord
        {
            OperationId = opId,
            Amount = amount,
            RemainingMinutes = _durationMinutes,
            TotalDurationMinutes = _durationMinutes
        });
        _state = LaunderingTerminalState.Processing;
        return new LaunderingOperationReceipt(opId, amount, DateTime.UtcNow, true, "laundering-started");
    }

    public void AdvanceMinutes(int elapsedMinutes)
    {
        if (elapsedMinutes <= 0)
            return;

        lock (_sync)
        {
            if (_isMutating)
                throw new InvalidOperationException("Cannot advance jobs during a wallet mutation.");

            foreach (var operation in _activeOperations)
                operation.RemainingMinutes = Math.Max(0, operation.RemainingMinutes - elapsedMinutes);
        }
    }

    /// <summary>Attempts each due job once for this game-clock or lifecycle pulse.</summary>
    public bool TrySettleDue(
        ILaunderingPort port,
        TownHallBusinessData? stats,
        out float completedAmount)
    {
        ArgumentNullException.ThrowIfNull(port);
        completedAmount = 0f;

        lock (_sync)
        {
            if (_isMutating || _unresolvedDeposit != null || _activeOperations.Count == 0 || !_purchaseManager.IsOwned)
                return false;

            _isMutating = true;
            try
            {
                for (int i = _activeOperations.Count - 1; i >= 0; i--)
                {
                    var operation = _activeOperations[i];
                    if (operation.RemainingMinutes != 0 || operation.PayoutUnresolved || operation.PayoutAttempts >= MaxPayoutAttempts)
                        continue;

                    operation.PayoutAttempts++;
                    MoneyMutationOutcome payout;
                    try
                    {
                        payout = port.PayoutCleanFunds(
                            operation.Amount, "Town Hall Laundering Payout", "Clean revenue from administrative business");
                    }
                    catch
                    {
                        payout = MoneyMutationOutcome.Unknown;
                    }

                    if (payout != MoneyMutationOutcome.Applied)
                    {
                        operation.PayoutUnresolved = payout != MoneyMutationOutcome.NotApplied;
                        continue;
                    }

                    // Mutations are serialized and reentry is blocked, so the paid job is still at i.
                    _activeOperations.RemoveAt(i);
                    completedAmount += operation.Amount;
                    if (stats != null)
                    {
                        stats.TotalLaundered += operation.Amount;
                        stats.CompletedLaunderingOperations++;
                    }
                }

                _state = _activeOperations.Any(op => op.PayoutAttempts > 0)
                    ? LaunderingTerminalState.Faulted
                    : _activeOperations.Count > 0 ? LaunderingTerminalState.Processing : LaunderingTerminalState.Completed;

                if (stats != null)
                    stats.ActiveOperations = _activeOperations.Select(CloneOperation).ToList();

                return completedAmount > 0f;
            }
            finally
            {
                _isMutating = false;
            }
        }
    }

    public void SyncStatsActiveOperations(TownHallBusinessData stats)
    {
        ArgumentNullException.ThrowIfNull(stats);
        lock (_sync)
        {
            stats.ActiveOperations = _activeOperations.Select(CloneOperation).ToList();
        }
    }

    private static ActiveLaunderingOperationRecord CloneOperation(ActiveLaunderingOperationRecord operation) =>
        new()
        {
            OperationId = operation.OperationId,
            Amount = operation.Amount,
            RemainingMinutes = operation.RemainingMinutes,
            TotalDurationMinutes = operation.TotalDurationMinutes,
            PayoutAttempts = operation.PayoutAttempts,
            PayoutUnresolved = operation.PayoutUnresolved
        };

    public void Reset()
    {
        lock (_sync)
        {
            if (_isMutating)
                throw new InvalidOperationException("Cannot reset jobs during a wallet mutation.");

            _state = LaunderingTerminalState.Ready;
            _activeOperations.Clear();
            _unresolvedDeposit = null;
        }
    }

    private LaunderingOperationReceipt CompensateRefund(
        ILaunderingPort port,
        string opId,
        float amount,
        string failureReason)
    {
        _state = LaunderingTerminalState.Refunding;
        MoneyMutationOutcome refund;

        try
        {
            refund = port.RefundCash(amount);
        }
        catch
        {
            refund = MoneyMutationOutcome.Unknown;
        }

        if (refund == MoneyMutationOutcome.Applied)
        {
            _state = LaunderingTerminalState.Refunded;
            return new LaunderingOperationReceipt(
                opId, amount, DateTime.UtcNow, false, $"refunded:{failureReason}");
        }

        return HoldDeposit(opId, amount, $"critical-refund-failed:{failureReason}");
    }

    private LaunderingOperationReceipt HoldDeposit(string operationId, float amount, string reason)
    {
        _state = LaunderingTerminalState.Faulted;
        _unresolvedDeposit = new LaunderingOperationReceipt(
            operationId, amount, DateTime.UtcNow, false, reason, RequiresReconciliation: true);
        return _unresolvedDeposit;
    }
}

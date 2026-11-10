using System;

namespace TownHallInterior;

/// <summary>
/// Supported payment channels for purchasing the TownHall business.
/// </summary>
public enum PurchasePaymentMode
{
    Bank,
    Cash,
    Auto
}

/// <summary>The observed effect of a wallet mutation, including an ambiguous dispatch.</summary>
public enum MoneyMutationOutcome
{
    NotApplied,
    Applied,
    Unknown
}

/// <summary>
/// Transaction state machine for property and business acquisition.
/// </summary>
public enum PurchaseState
{
    Unowned,
    Validating,
    InsufficientFunds,
    DeductingFunds,
    TransferringOwnership,
    Completed,
    Refunding,
    Refunded,
    Faulted
}

/// <summary>
/// Immutable receipt of a purchase or refund attempt.
/// </summary>
public sealed record PurchaseReceipt(
    string TransactionId,
    float Amount,
    PurchasePaymentMode ResolvedMode,
    DateTime TimestampUtc,
    bool Success,
    string? StatusMessage = null,
    bool RequiresReconciliation = false);

/// <summary>
/// Port abstraction for economic operations (bank transfers and physical cash).
/// </summary>
public interface IPurchasePaymentPort
{
    float GetOnlineBalance();
    float GetCashBalance();
    MoneyMutationOutcome DeductBank(float amount, string label, string note);
    MoneyMutationOutcome DeductCash(float amount);
    MoneyMutationOutcome RefundBank(float amount, string label, string note);
    MoneyMutationOutcome RefundCash(float amount);
}

/// <summary>
/// Coordinates the atomic purchase of the TownHall business, strictly following
/// the schedule1-economy compensating transaction rules.
/// </summary>
public sealed class BusinessPurchaseManager
{
    private readonly object _sync = new();
    private readonly float _price;
    private PurchaseState _state = PurchaseState.Unowned;
    private bool _isOwned;
    private bool _isMutating;
    private PurchaseReceipt? _lastReceipt;

    public const float DefaultTownHallPrice = 250_000f;

    public BusinessPurchaseManager(float price = DefaultTownHallPrice, bool initiallyOwned = false)
    {
        if (float.IsNaN(price) || float.IsInfinity(price) || price <= 0f)
            throw new ArgumentOutOfRangeException(nameof(price), "Purchase price must be a positive finite amount.");

        _price = price;
        _isOwned = initiallyOwned;
        _state = initiallyOwned ? PurchaseState.Completed : PurchaseState.Unowned;
    }

    public float Price => _price;
    public bool IsOwned
    {
        get
        {
            lock (_sync) return _isOwned;
        }
    }

    public PurchaseState State
    {
        get
        {
            lock (_sync) return _state;
        }
    }

    public bool IsMutating
    {
        get
        {
            lock (_sync) return _isMutating;
        }
    }

    public PurchaseReceipt? LastReceipt
    {
        get
        {
            lock (_sync) return _lastReceipt;
        }
    }

    public PurchaseReceipt TryPurchase(
        ConstructionContext? context,
        IPurchasePaymentPort paymentPort,
        IBusinessWorldPort worldPort,
        WorldRegistrationIdentity identity,
        PurchasePaymentMode requestedMode = PurchasePaymentMode.Auto)
    {
        ArgumentNullException.ThrowIfNull(paymentPort);
        ArgumentNullException.ThrowIfNull(worldPort);
        ArgumentNullException.ThrowIfNull(identity);

        lock (_sync)
        {
            string txId = Guid.NewGuid().ToString("N");
            if (_isMutating)
                return new PurchaseReceipt(txId, 0f, requestedMode, DateTime.UtcNow, false, "blocked:transaction-in-progress");

            if (_lastReceipt is { RequiresReconciliation: true } pendingReceipt)
                return pendingReceipt;

            if (_isOwned)
            {
                return RecordReceipt(new PurchaseReceipt(
                    txId, 0f, requestedMode, DateTime.UtcNow, false, "already-owned"));
            }

            string? gateError = ConstructionGate.Check(context, identity.TargetOrganisation, identity.TargetSlot);
            if (gateError != null)
            {
                return RecordReceipt(new PurchaseReceipt(
                    txId, 0f, requestedMode, DateTime.UtcNow, false, $"skipped:{gateError}"));
            }

            _isMutating = true;
            try
            {
                return TryPurchaseCore(paymentPort, worldPort, identity, requestedMode, txId);
            }
            finally
            {
                _isMutating = false;
            }
        }
    }

    private PurchaseReceipt TryPurchaseCore(
        IPurchasePaymentPort paymentPort,
        IBusinessWorldPort worldPort,
        WorldRegistrationIdentity identity,
        PurchasePaymentMode requestedMode,
        string txId)
    {
        _state = PurchaseState.Validating;
        float bankBalance;
        float cashBalance;
        try
        {
            bankBalance = paymentPort.GetOnlineBalance();
            cashBalance = paymentPort.GetCashBalance();
        }
        catch
        {
            _state = PurchaseState.Faulted;
            return RecordReceipt(new PurchaseReceipt(txId, _price, requestedMode, DateTime.UtcNow, false, "balance-unavailable"));
        }

        bool bankCanPay = float.IsFinite(bankBalance) && bankBalance >= _price;
        bool cashCanPay = float.IsFinite(cashBalance) && cashBalance >= _price;
        var resolvedMode = requestedMode == PurchasePaymentMode.Auto
            ? (bankCanPay ? PurchasePaymentMode.Bank : PurchasePaymentMode.Cash)
            : requestedMode;
        bool canPay = resolvedMode switch
        {
            PurchasePaymentMode.Bank => bankCanPay,
            PurchasePaymentMode.Cash => cashCanPay,
            _ => false
        };
        if (!canPay)
        {
            _state = PurchaseState.InsufficientFunds;
            return RecordReceipt(new PurchaseReceipt(
                txId, _price, requestedMode, DateTime.UtcNow, false, "insufficient-funds"));
        }

        _state = PurchaseState.DeductingFunds;
        MoneyMutationOutcome deduction;
        try
        {
            deduction = resolvedMode == PurchasePaymentMode.Bank
                ? paymentPort.DeductBank(_price, "Town Hall Purchase", "Real Estate Acquisition")
                : paymentPort.DeductCash(_price);
        }
        catch
        {
            deduction = MoneyMutationOutcome.Unknown;
        }

        if (deduction != MoneyMutationOutcome.Applied)
        {
            _state = PurchaseState.Faulted;
            bool requiresReconciliation = deduction != MoneyMutationOutcome.NotApplied;
            return RecordReceipt(new PurchaseReceipt(
                txId, _price, resolvedMode, DateTime.UtcNow, false,
                requiresReconciliation ? "payment-unresolved" : "payment-declined", requiresReconciliation));
        }

        _state = PurchaseState.TransferringOwnership;
        bool registered;
        try
        {
            registered = worldPort.RegisterInWorld(identity, isOwned: true);
        }
        catch (Exception ex)
        {
            return CompensateRefund(paymentPort, txId, resolvedMode, $"registration-exception:{ex.Message}");
        }

        if (!registered)
            return CompensateRefund(paymentPort, txId, resolvedMode, "registration-failed");

        _isOwned = true;
        _state = PurchaseState.Completed;
        return RecordReceipt(new PurchaseReceipt(
            txId, _price, resolvedMode, DateTime.UtcNow, true, "purchase-completed"));
    }

    private PurchaseReceipt CompensateRefund(
        IPurchasePaymentPort paymentPort,
        string txId,
        PurchasePaymentMode mode,
        string failureReason)
    {
        _state = PurchaseState.Refunding;
        MoneyMutationOutcome refund;

        try
        {
            refund = mode == PurchasePaymentMode.Bank
                ? paymentPort.RefundBank(_price, "Town Hall Refund", "Rollback of failed purchase")
                : paymentPort.RefundCash(_price);
        }
        catch
        {
            refund = MoneyMutationOutcome.Unknown;
        }

        if (refund == MoneyMutationOutcome.Applied)
        {
            _state = PurchaseState.Refunded;
            return RecordReceipt(new PurchaseReceipt(
                txId, _price, mode, DateTime.UtcNow, false, $"refunded:{failureReason}"));
        }

        _state = PurchaseState.Faulted;
        return RecordReceipt(new PurchaseReceipt(
            txId, _price, mode, DateTime.UtcNow, false, $"critical-refund-failed:{failureReason}", RequiresReconciliation: true));
    }

    private PurchaseReceipt RecordReceipt(PurchaseReceipt receipt)
    {
        _lastReceipt = receipt;
        return receipt;
    }

    public void RestoreOwnership(bool isOwned)
    {
        lock (_sync)
        {
            if (_isMutating)
                throw new InvalidOperationException("Cannot restore ownership during a purchase.");

            _isOwned = isOwned;
            _state = isOwned ? PurchaseState.Completed : PurchaseState.Unowned;
            _lastReceipt = null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Levelling;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using PocketShop.Config;
using S1Mods.Shared;
using UnityEngine;
using EPaymentType = Il2CppScheduleOne.UI.Shop.ShopInterface.EPaymentType;

namespace PocketShop.Services;

public enum BuyResult
{
    Success,
    NotEnoughCash,
    NotEnoughBank,
    NotEnoughFunds,
    NoInventorySpace,
    StockEmpty,
    InvalidQuantity,
    DefinitionNull,
    LevelLocked,
    ShopUnavailable,
    InvalidPrice,
    PriceChanged,
    MultiplayerUnavailable,
    Busy,
    /// <summary>The bank payment was sent; the deferred confirmation is still running.</summary>
    PaymentPending,
    TransactionUncertain,
    Error
}

public enum PurchasePhase
{
    Rejected,
    Preparing,
    Prepared,
    PaymentStarted,
    /// <summary>Bank payment sent, awaiting the deferred server-side application.</summary>
    PaymentPending,
    PaymentConfirmed,
    DeliveryStarted,
    DeliveryObserved,
    StockUpdateStarted,
    RefundStarted,
    Partial,
    Committed,
    Uncertain
}

/// <summary>Detailed outcome used by the UI and status messages.</summary>
public sealed class PurchaseResultData
{
    public BuyResult Result { get; set; }
    public PurchasePhase Phase { get; set; }
    public PaymentMode PaymentModeUsed { get; set; }
    public float TotalPaid { get; set; }
    public bool NetPaidConfirmed { get; set; }
    public float UnitPriceWithFee { get; set; }
    public int Quantity { get; set; }
    public int DeliveredCount { get; set; }
    public ItemPOCO Item { get; set; } = null!;
    public string Message { get; set; } = string.Empty;

    public bool IsSuccess => Result == BuyResult.Success;
    public bool IsUncertain => Result == BuyResult.TransactionUncertain;
    public bool IsPending => Result == BuyResult.PaymentPending;
}

/// <summary>
/// Validates live purchase data, then performs one guarded transaction. Vanilla IL2CPP exposes
/// independent money, inventory, and stock mutations rather than a verified atomic phone-purchase API.
///
/// Payment confirmation (v0.3.3): <c>ChangeCashBalance</c> applies synchronously and is confirmed
/// by an immediate exact balance-delta read-back. <c>CreateOnlineTransaction</c> is a FishNet
/// ServerRpc — the caller only sends the packet, the balance changes when the server processes it
/// on a later network tick — so the bank path is confirmed DEFERRED: the purchase waits (bounded)
/// for the exact balance delta or for the applied-transaction hook, and only then delivers.
/// </summary>
public static class PurchaseService
{
    public const int UnlimitedStockSentinel = PurchaseRules.UnlimitedStockSentinel;

    /// <summary>Bounded wait for the server to apply a sent bank transaction (realtime seconds).</summary>
    public const float BankConfirmationWindowSeconds = 5f;

    /// <summary>How long a timed-out bank transaction is watched for a late application.</summary>
    public const float UncertainReconciliationWindowSeconds = 60f;

    private static int _purchaseInProgress;
    private static int _purchaseToken;
    private static int _purchasePending;
    private static int _transactionUncertain;
    private static int _reconciliationActive;

    private static readonly object _pendingGate = new();
    private static PendingBankPayment? _pendingPayment;
    private static readonly Queue<PurchaseResultData> _completedPurchases = new();

    /// <summary>Set once a transaction could not be reconciled; purchases stay locked (fail-closed).</summary>
    public static bool HasUncertainTransaction => Volatile.Read(ref _transactionUncertain) != 0;

    /// <summary>True while a sent bank payment is awaiting its deferred confirmation.</summary>
    public static bool IsPurchasePending => Volatile.Read(ref _purchasePending) != 0;

    public static bool IsSingleplayerPurchaseContext => NetworkGuard.IsInMainScene && IsSingleplayerSession();
    public static bool IsPurchaseAvailableInCurrentSession =>
        IsSingleplayerPurchaseContext && !HasUncertainTransaction && !IsPurchasePending;

    /// <summary>
    /// Dequeues the result of a deferred (bank) purchase that finished after its call returned.
    /// The app drains this queue while the phone app is open; results are never lost silently.
    /// </summary>
    public static bool TryDequeueCompletedPurchase(out PurchaseResultData result)
    {
        lock (_completedPurchases)
        {
            if (_completedPurchases.Count == 0)
            {
                result = null!;
                return false;
            }
            result = _completedPurchases.Dequeue();
            return true;
        }
    }

    private static void QueueCompletedPurchase(PurchaseResultData result)
    {
        lock (_completedPurchases)
        {
            _completedPurchases.Enqueue(result);
        }
    }

    public static bool TryCalculatePricing(float livePrice, int quantity, out PurchasePricing pricing)
    {
        return PurchaseRules.TryCalculatePricing(
            livePrice,
            PocketShopConfig.ServiceFeePercentStatic,
            PocketShopConfig.DeliveryFeeFlatStatic,
            quantity,
            out pricing);
    }

    /// <summary>Compatibility wrapper; invalid pricing is returned as NaN and must not be charged.</summary>
    public static void CalculatePricing(ItemPOCO item, int qty, out float perUnit, out float subtotal, out float deliveryFee, out float total)
    {
        if (item != null && TryCalculatePricing(item.Price, qty, out var pricing))
        {
            perUnit = pricing.UnitPrice;
            subtotal = pricing.Subtotal;
            deliveryFee = pricing.DeliveryFee;
            total = pricing.Total;
            return;
        }

        perUnit = subtotal = deliveryFee = total = float.NaN;
    }

    public static string FormatMoney(float amount)
    {
        return float.IsFinite(amount)
            ? amount.ToString("0.################", CultureInfo.InvariantCulture)
            : "—";
    }

    /// <summary>Resolves the vanilla Cash/Online/PreferCash/PreferOnline rule.</summary>
    public static bool CanAfford(float total, EPaymentType shopPaymentType, out PaymentMode effectiveMode)
    {
        effectiveMode = PaymentMode.Bank;
        if (!float.IsFinite(total) || total < 0f || !TryGetPaymentPreference(shopPaymentType, out var preference))
            return false;

        MoneyManager? money;
        try { money = MoneyManager.Instance; }
        catch { return false; }
        if (!NetworkGuard.IsAlive(money)) return false;

        float cash;
        float online;
        try
        {
            cash = money!.cashBalance;
            online = money.sync___get_value_onlineBalance();
        }
        catch
        {
            return false;
        }

        bool canPay = PurchaseRules.TryResolvePayment(preference, total, cash, online, out var account);
        effectiveMode = account == PaymentAccount.Cash ? PaymentMode.Cash : PaymentMode.Bank;
        return canPay;
    }

    /// <summary>Legacy entry point retained for source compatibility; it cannot safely buy without a displayed total.</summary>
    public static PurchaseResultData Buy(ItemPOCO item)
    {
        return ConfirmationRequired(item, 1, PaymentMode.Bank);
    }

    /// <summary>Legacy signature retained, but fail-closed because it carries no confirmed card price.</summary>
    public static PurchaseResultData BuyWithQuantity(ItemPOCO item, int quantity, PaymentMode mode = PaymentMode.Bank)
    {
        return ConfirmationRequired(item, quantity, mode);
    }

    private static PurchaseResultData ConfirmationRequired(ItemPOCO item, int quantity, PaymentMode mode)
    {
        var result = new PurchaseResultData
        {
            Result = BuyResult.PriceChanged,
            Item = item!,
            Quantity = quantity,
            PaymentModeUsed = mode,
            Phase = PurchasePhase.Rejected
        };
        return Reject(result, BuyResult.PriceChanged,
            "This legacy purchase call has no confirmed displayed total. Use the BUY button on the current PocketShop item card.");
    }

    /// <summary>
    /// Buys only at the exact total and payment mode last shown by the item card. Price or payment
    /// changes require the player to review the refreshed card before another click can commit.
    ///
    /// Cash: fully synchronous (payment, delivery, refunds). Bank: the payment is sent and the
    /// method returns <see cref="BuyResult.PaymentPending"/>; confirmation and delivery continue
    /// in a coroutine and the final result is dequeued via <see cref="TryDequeueCompletedPurchase"/>.
    /// </summary>
    public static PurchaseResultData BuyWithQuantity(ItemPOCO item, int quantity, float confirmedTotal, PaymentMode confirmedMode)
    {
        var result = new PurchaseResultData
        {
            Result = BuyResult.Error,
            Item = item,
            Quantity = quantity,
            PaymentModeUsed = confirmedMode,
            Phase = PurchasePhase.Preparing
        };

        if (Interlocked.CompareExchange(ref _purchaseInProgress, 1, 0) != 0)
            return Reject(result, BuyResult.Busy, "A purchase is already being processed.");
        int purchaseToken = Interlocked.Increment(ref _purchaseToken);

        bool handoffToDeferredConfirmation = false;
        bool nativeMutationAttempted = false;
        try
        {
            if (HasUncertainTransaction)
                return MarkUncertain(result,
                    "A previous purchase could not be reconciled. PocketShop is paused for this game session; check inventory and balances before restarting.",
                    string.Empty);
            if (!NetworkGuard.IsInMainScene)
                return Reject(result, BuyResult.ShopUnavailable, "Purchases are only available in the active gameplay scene.");
            if (!IsSingleplayerSession())
                return Reject(result, BuyResult.MultiplayerUnavailable,
                    "Purchasing is unavailable in multiplayer (including host sessions) until a verified synchronized purchase path exists.");

            if (item == null)
                return Reject(result, BuyResult.DefinitionNull, "The selected item is no longer available.");
            if (quantity < 1)
                return Reject(result, BuyResult.InvalidQuantity, "Invalid quantity.");

            if (!ShopCatalog.TryResolveLiveOffer(item, out var offer, out string liveFailure))
                return Reject(result, BuyResult.ShopUnavailable, liveFailure);

            if (!offer.Gate.IsKnown)
                return Reject(result, BuyResult.ShopUnavailable, "Shop availability could not be verified. No purchase was made.");
            if (offer.Gate.IsLocked)
                return Reject(result, BuyResult.ShopUnavailable, "This shop is locked.");
            if (!offer.Gate.IsOpen)
            {
                string hours = offer.Gate.HoursText;
                return Reject(result, BuyResult.ShopUnavailable,
                    string.IsNullOrEmpty(hours) ? "This shop is closed." : $"This shop is closed ({hours}).");
            }

            if (!TryCheckLevelRequirement(offer.Definition, out string levelFailure))
                return Reject(result, BuyResult.LevelLocked, levelFailure);

            if (offer.Stock.Kind == StockKind.Unknown || offer.Stock.Kind == StockKind.NotOffered)
                return Reject(result, BuyResult.ShopUnavailable, "The current listing state could not be verified.");
            if (offer.Stock.Kind == StockKind.LimitedEmpty)
                return Reject(result, BuyResult.StockEmpty, $"'{item.Name}' is out of stock.");
            if (offer.Stock.Kind == StockKind.Unlimited && quantity > 99)
                return Reject(result, BuyResult.InvalidQuantity, "Unlimited-stock purchases are capped at 99 items.");
            if (!offer.Stock.CanBuy(quantity))
            {
                string message = offer.Stock.Kind == StockKind.LimitedAvailable
                    ? $"Only {offer.Stock.Quantity}x in stock."
                    : "The requested quantity is not available.";
                return Reject(result, BuyResult.InvalidQuantity, message);
            }

            if (!TryCalculatePricing(offer.Price, quantity, out var pricing))
                return Reject(result, BuyResult.InvalidPrice, "The live item price or configured fees are invalid.");

            result.UnitPriceWithFee = pricing.UnitPrice;
            if (!float.IsFinite(confirmedTotal) || confirmedTotal < 0f || !confirmedTotal.Equals(pricing.Total))
                return Reject(result, BuyResult.PriceChanged,
                    $"Price changed to ${FormatMoney(pricing.Total)}. Review the updated total and press BUY again.");

            if (!CanAfford(pricing.Total, offer.PaymentType, out PaymentMode effectiveMode))
            {
                if (!IsSupportedPaymentType(offer.PaymentType))
                    return Reject(result, BuyResult.ShopUnavailable, "The shop payment method could not be verified.");

                var moneyForMessage = MoneyManager.Instance;
                if (effectiveMode == PaymentMode.Cash)
                {
                    float cash = TryReadBalance(moneyForMessage, PaymentMode.Cash, out float cashBalance) ? cashBalance : 0f;
                    return Reject(result, BuyResult.NotEnoughCash,
                        $"Not enough cash. Need ${FormatMoney(pricing.Total)} (have ${FormatMoney(cash)}).");
                }

                float bank = TryReadBalance(moneyForMessage, PaymentMode.Bank, out float bankBalance) ? bankBalance : 0f;
                return Reject(result, BuyResult.NotEnoughBank,
                    $"Not enough card funds. Need ${FormatMoney(pricing.Total)} (have ${FormatMoney(bank)}).");
            }

            if (effectiveMode != confirmedMode)
                return Reject(result, BuyResult.PriceChanged,
                    $"Payment method changed to {(effectiveMode == PaymentMode.Cash ? "Cash" : "Card")}. Review the card and press BUY again.");
            result.PaymentModeUsed = effectiveMode;

            var money = MoneyManager.Instance;
            var inventory = PlayerInventory.Instance;
            if (!NetworkGuard.IsAlive(money) || !NetworkGuard.IsAlive(inventory))
                return Reject(result, BuyResult.Error, "Money or inventory is unavailable.");

            ItemInstance? probe;
            try { probe = offer.Definition.GetDefaultInstance(1); }
            catch (Exception ex) { return Reject(result, BuyResult.DefinitionNull, $"Could not prepare item: {ex.Message}"); }
            if (!IsInteropObjectAlive(probe))
                return Reject(result, BuyResult.DefinitionNull, "Could not prepare the item instance.");

            bool fits;
            try { fits = inventory!.CanItemFitInInventory(probe!, quantity); }
            catch (Exception ex) { return Reject(result, BuyResult.NoInventorySpace, $"Inventory capacity could not be verified: {ex.Message}"); }
            if (!fits)
                return Reject(result, BuyResult.NoInventorySpace, $"Inventory full. Cannot fit {quantity}x '{item.Name}'.");

            var instances = new List<ItemInstance>(quantity) { probe! };
            for (int i = 1; i < quantity; i++)
            {
                ItemInstance? instance;
                try { instance = offer.Definition.GetDefaultInstance(1); }
                catch (Exception ex) { return Reject(result, BuyResult.DefinitionNull, $"Could not prepare item instances: {ex.Message}"); }
                if (!IsInteropObjectAlive(instance))
                    return Reject(result, BuyResult.DefinitionNull, "Could not prepare all item instances.");
                instances.Add(instance!);
            }

            uint inventoryBefore;
            try { inventoryBefore = inventory!.GetAmountOfItem(offer.Definition.ID); }
            catch (Exception ex)
            { return Reject(result, BuyResult.Error, $"Inventory state could not be verified before payment: {ex.Message}"); }

            result.Phase = PurchasePhase.Prepared;
            if (pricing.Total > 0f)
            {
                result.Phase = PurchasePhase.PaymentStarted;
                nativeMutationAttempted = true;

                if (effectiveMode == PaymentMode.Cash)
                {
                    if (!TryApplyCashDelta(money!, -pricing.Total, out string cashFailure, out float cashBefore, out float cashAfter))
                    {
                        return MarkUncertain(result,
                            "Payment state is unclear; no items were transferred. Check the balance before retrying.",
                            BuildCashDiagnostics(offer, item, pricing, cashBefore, cashAfter, cashFailure));
                    }
                    result.TotalPaid = pricing.Total;
                    result.Phase = PurchasePhase.PaymentConfirmed;
                }
                else
                {
                    // Bank: CreateOnlineTransaction is a FishNet ServerRpc. The balance changes
                    // when the server processes the packet (a later network tick), so confirmation
                    // is deferred — never read back synchronously and treat "not yet applied" as failure.
                    float bankBefore;
                    try { bankBefore = ReadBalance(money!, PaymentMode.Bank); }
                    catch (Exception ex)
                    { return Reject(result, BuyResult.Error, $"The bank balance could not be read before payment: {ex.Message}"); }
                    if (!float.IsFinite(bankBefore))
                        return Reject(result, BuyResult.Error, "The bank balance was not a finite value before payment.");

                    var payment = new PendingBankPayment
                    {
                        Title = BankTransactionObserver.OrderTitle,
                        Amount = -pricing.Total,
                        Before = bankBefore,
                        SentAt = Time.realtimeSinceStartup,
                        ShopCode = item.ShopCode ?? string.Empty,
                        ShopName = item.ShopName ?? string.Empty,
                        ItemId = item.ItemId ?? string.Empty,
                        ItemName = item.Name ?? string.Empty,
                        Rule = offer.PaymentType.ToString(),
                        UnitPrice = pricing.UnitPrice,
                        ServiceFeePerUnit = pricing.UnitPrice - offer.Price,
                        DeliveryFee = pricing.DeliveryFee,
                        Total = pricing.Total
                    };

                    try
                    {
                        money!.CreateOnlineTransaction(payment.Title, payment.Amount, 1f, payment.Title);
                    }
                    catch (Exception ex)
                    {
                        return MarkUncertain(result,
                            "The bank payment call failed while sending; no items were transferred. Check the balance before retrying.",
                            BuildBankDiagnostics(payment, float.NaN, $"send-failed: {ex.Message}"));
                    }

                    try
                    {
                        MelonCoroutines.Start(DeferredBankPurchaseRoutine(
                            result, payment, offer, effectiveMode, instances, inventoryBefore, pricing, quantity, item, purchaseToken));
                    }
                    catch (Exception ex)
                    {
                        // The payment was already sent; without the confirmation routine the state
                        // cannot be resolved automatically — fail-closed, keep the lock.
                        return MarkUncertain(result,
                            "The card payment was sent but its confirmation could not be started. Do not retry until you check the bank balance.",
                            BuildBankDiagnostics(payment, float.NaN, $"coroutine-start-failed: {ex.Message}"));
                    }

                    // Ownership of the single-flight guard and the pending state passes to the coroutine.
                    handoffToDeferredConfirmation = true;
                    lock (_pendingGate) { _pendingPayment = payment; }
                    SetPurchasePending(true);
                    result.Result = BuyResult.PaymentPending;
                    result.Phase = PurchasePhase.PaymentPending;
                    result.NetPaidConfirmed = false;
                    result.Message = $"Confirming card payment of ${FormatMoney(pricing.Total)}...";
                    return result;
                }
            }
            else
            {
                result.Phase = PurchasePhase.PaymentConfirmed;
            }

            return CompleteCashDelivery(result, offer, effectiveMode, instances, inventoryBefore, pricing, quantity, item, money!);
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PocketShop] transaction exception at {result.Phase}: {ex.Message}");
            if (nativeMutationAttempted)
                return MarkUncertain(result, $"Transaction stopped at {result.Phase}; no automatic retry or extra money change was made. Check inventory and balance. {ex.Message}", string.Empty);
            return Reject(result, BuyResult.Error, $"Purchase was cancelled before any native mutation: {ex.Message}");
        }
        finally
        {
            if (!handoffToDeferredConfirmation)
                Interlocked.Exchange(ref _purchaseInProgress, 0);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────────────
    // Deferred bank confirmation (ServerRpc → applied on a later server tick)
    // ─────────────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Waits (bounded) for the sent bank transaction to apply, then delivers. The routine owns the
    /// single-flight guard until it finishes; its final result is queued for the UI.
    /// </summary>
    private static IEnumerator DeferredBankPurchaseRoutine(
        PurchaseResultData result, PendingBankPayment payment, LiveShopOffer offer, PaymentMode mode,
        List<ItemInstance> instances, uint inventoryBefore, PurchasePricing pricing, int quantity, ItemPOCO item, int token)
    {
        // Hand-off separation: the caller publishes the pending state before any work happens here.
        yield return null;

        bool confirmed = false;
        string confirmationSource = string.Empty;
        float lastObserved = float.NaN;

        while (true)
        {
            if (!NetworkGuard.IsInMainScene)
            {
                // The gameplay scene tore down before the payment could be observed. The pending
                // RPC cannot apply anymore after the teardown; the lock is released at the scene
                // boundary (ResetForSceneReload) instead of leaking into the next session.
                MelonLogger.Warning($"[PocketShop] Deferred card payment was not confirmed before the scene changed. {BuildBankDiagnostics(payment, lastObserved, "scene-teardown")}");
                CompletePendingPurchase(payment, token);
                yield break;
            }

            try
            {
                var observation = ObserveBankPayment(payment, out float observed, out string source);
                if (float.IsFinite(observed)) lastObserved = observed;
                if (observation == BalanceObservation.Confirmed)
                {
                    confirmed = true;
                    confirmationSource = source;
                    break;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[PocketShop] Bank balance could not be read while confirming a payment: {ex.Message}");
            }

            if (PurchaseRules.IsConfirmationExpired(payment.SentAt, Time.realtimeSinceStartup, BankConfirmationWindowSeconds))
                break;
            yield return null;
        }

        if (!confirmed)
        {
            MarkUncertain(result,
                $"The card payment was sent but could not be confirmed within {BankConfirmationWindowSeconds:0.#}s. No items were transferred. Do not retry until you check the bank balance.",
                BuildBankDiagnostics(payment, lastObserved, "timeout"));
            QueueCompletedPurchase(result);
            StartUncertainReconciliation(payment);
            CompletePendingPurchase(payment, token);
            yield break;
        }

        result.Phase = PurchasePhase.PaymentConfirmed;
        result.TotalPaid = pricing.Total;
        string diagnostics = BuildBankDiagnostics(payment, lastObserved, confirmationSource);

        // Phase 2 — delivery, stock, refund decision/send. No yields in here: the compiler forbids
        // `yield return` inside a try/catch, so this phase runs as a plain helper that may throw.
        ConfirmedPhaseOutcome? outcome = null;
        string phaseError = string.Empty;
        try
        {
            outcome = RunConfirmedPhase(result, offer, instances, inventoryBefore, pricing, quantity, item);
        }
        catch (Exception ex)
        {
            phaseError = ex.Message;
        }

        if (outcome == null)
        {
            MelonLogger.Error($"[PocketShop] Deferred card purchase failed after confirmation: {phaseError}");
            MarkUncertain(result,
                $"The payment was confirmed, but the purchase stopped during delivery: {phaseError}. Do not retry until you check inventory and balance.",
                diagnostics);
            QueueCompletedPurchase(result);
            CompletePendingPurchase(payment, token);
            yield break;
        }

        if (outcome.Delivery.Uncertain)
        {
            MarkUncertain(result, outcome.Delivery.UncertainMessage, diagnostics);
            QueueCompletedPurchase(result);
            CompletePendingPurchase(payment, token);
            yield break;
        }

        // Phase 3 — confirm the refund (another ServerRpc → later tick). Yields are allowed here.
        bool refundConfirmed = true;
        if (outcome.Refund > 0f)
        {
            if (outcome.RefundSent)
            {
                refundConfirmed = false;
                float refundDeadline = outcome.RefundSentAt + BankConfirmationWindowSeconds;
                while (Time.realtimeSinceStartup < refundDeadline)
                {
                    if (!NetworkGuard.IsInMainScene) break;
                    if (BankTransactionObserver.IsAvailable
                        && BankTransactionObserver.TryFindApplied(BankTransactionObserver.RefundTitle, outcome.Refund, outcome.RefundSentAt, out _))
                    {
                        refundConfirmed = true;
                        break;
                    }
                    if (float.IsFinite(outcome.RefundBefore))
                    {
                        try
                        {
                            float after = ReadBalance(MoneyManager.Instance, PaymentMode.Bank);
                            if (PurchaseRules.ClassifyBalanceObservation(outcome.RefundBefore, after, outcome.Refund) == BalanceObservation.Confirmed)
                            {
                                refundConfirmed = true;
                                break;
                            }
                        }
                        catch { }
                    }
                    yield return null;
                }

                if (refundConfirmed)
                {
                    result.TotalPaid = pricing.Total - outcome.Refund;
                    result.NetPaidConfirmed = true;
                }
                else
                {
                    MelonLogger.Error($"[PocketShop] Card refund could not be confirmed for '{item.Name}' (${FormatMoney(outcome.Refund)}). It was not retried. {diagnostics}");
                }
            }
            else
            {
                refundConfirmed = false;
            }
        }

        // Phase 4 — finalize (no yields).
        FinalizeDeliveryOutcome(result, outcome.Delivery, pricing, mode, item, outcome.StockConfirmed, outcome.StockFailure,
            outcome.Refund, refundConfirmed, diagnostics);
        QueueCompletedPurchase(result);
        CompletePendingPurchase(payment, token);
    }

    /// <summary>Result of the post-confirmation phase (delivery + stock + refund decision/send).</summary>
    private sealed class ConfirmedPhaseOutcome
    {
        public DeliveryPhaseResult Delivery = new();
        public bool StockConfirmed = true;
        public string StockFailure = string.Empty;
        public float Refund;
        public bool RefundSent;
        public float RefundSentAt;
        public float RefundBefore = float.NaN;
    }

    /// <summary>
    /// Delivery, stock update and refund decision for a confirmed bank payment. Never yields;
    /// the caller turns a thrown exception into an uncertain outcome.
    /// </summary>
    private static ConfirmedPhaseOutcome RunConfirmedPhase(
        PurchaseResultData result, LiveShopOffer offer, List<ItemInstance> instances,
        uint inventoryBefore, PurchasePricing pricing, int quantity, ItemPOCO item)
    {
        var outcome = new ConfirmedPhaseOutcome();
        outcome.Delivery = RunDeliveryPhase(result, offer, instances, inventoryBefore, quantity, item);
        if (outcome.Delivery.Uncertain)
            return outcome;

        result.Phase = PurchasePhase.StockUpdateStarted;
        outcome.StockConfirmed = TryUpdateStock(offer, outcome.Delivery.DeliveredCount, out outcome.StockFailure);

        if (outcome.Delivery.DeliveredCount < quantity)
        {
            if (!PurchaseRules.TryCalculateRefund(pricing.UnitPrice, pricing.DeliveryFee, quantity, outcome.Delivery.DeliveredCount, out outcome.Refund))
            {
                outcome.Delivery.Uncertain = true;
                outcome.Delivery.UncertainMessage = "A safe partial refund could not be calculated; transaction state is unclear.";
                return outcome;
            }

            if (outcome.Refund > 0f)
            {
                result.Phase = PurchasePhase.RefundStarted;
                try
                {
                    var money = MoneyManager.Instance;
                    if (!NetworkGuard.IsAlive(money))
                        throw new InvalidOperationException("The money manager is unavailable.");
                    try { outcome.RefundBefore = ReadBalance(money!, PaymentMode.Bank); } catch { outcome.RefundBefore = float.NaN; }
                    outcome.RefundSentAt = Time.realtimeSinceStartup;
                    money!.CreateOnlineTransaction(BankTransactionObserver.RefundTitle, outcome.Refund, 1f, BankTransactionObserver.RefundTitle);
                    outcome.RefundSent = true;
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[PocketShop] Card refund could not be sent for '{item.Name}': {ex.Message}");
                }
            }
            else
            {
                result.TotalPaid = pricing.Total;
                result.NetPaidConfirmed = true;
            }
            result.Phase = PurchasePhase.Partial;
        }

        return outcome;
    }

    /// <summary>
    /// One confirmation observation: exact balance delta first, then the applied-transaction hook.
    /// "Unchanged" is a valid intermediate state — the caller re-checks until its bounded window ends.
    /// </summary>
    private static BalanceObservation ObserveBankPayment(PendingBankPayment payment, out float observed, out string source)
    {
        observed = float.NaN;
        source = string.Empty;

        var money = MoneyManager.Instance;
        if (NetworkGuard.IsAlive(money))
        {
            try
            {
                observed = ReadBalance(money!, PaymentMode.Bank);
                var observation = PurchaseRules.ClassifyBalanceObservation(payment.Before, observed, payment.Amount);
                if (observation == BalanceObservation.Confirmed)
                {
                    source = "balance";
                    return BalanceObservation.Confirmed;
                }
                if (observation == BalanceObservation.Mismatch)
                    return BalanceObservation.Mismatch;
            }
            catch
            {
                observed = float.NaN;
            }
        }

        if (BankTransactionObserver.IsAvailable
            && BankTransactionObserver.TryFindApplied(payment.Title, payment.Amount, payment.SentAt, out _))
        {
            source = "hook";
            return BalanceObservation.Confirmed;
        }

        return BalanceObservation.Unchanged;
    }

    /// <summary>
    /// Watches a timed-out bank transaction for a late application (bounded). If the exact
    /// transaction is observed, the money moved without a delivery — it is refunded with the
    /// same deferred confirmation. Hook-only: only an exact record of our own transaction is
    /// strong enough evidence; a coincidental balance delta is not.
    /// </summary>
    private static void StartUncertainReconciliation(PendingBankPayment payment)
    {
        if (!BankTransactionObserver.IsAvailable)
            return;
        if (Interlocked.CompareExchange(ref _reconciliationActive, 1, 0) != 0)
            return;
        try
        {
            MelonCoroutines.Start(ReconcileUncertainBankPayment(payment));
        }
        catch (Exception ex)
        {
            Interlocked.Exchange(ref _reconciliationActive, 0);
            MelonLogger.Warning($"[PocketShop] Reconciliation watcher could not start: {ex.Message}");
        }
    }

    private static IEnumerator ReconcileUncertainBankPayment(PendingBankPayment payment)
    {
        try
        {
            bool refundAttempted = false;
            float deadline = Time.realtimeSinceStartup + UncertainReconciliationWindowSeconds;

            while (Time.realtimeSinceStartup < deadline)
            {
                if (!NetworkGuard.IsInMainScene)
                    yield break;

                if (!refundAttempted
                    && BankTransactionObserver.TryFindApplied(payment.Title, payment.Amount, payment.SentAt, out _))
                {
                    refundAttempted = true;
                    float refund = -payment.Amount;
                    if (!float.IsFinite(refund) || refund <= 0f)
                        yield break;

                    bool refundSent = false;
                    float refundSentAt = 0f;
                    try
                    {
                        var money = MoneyManager.Instance;
                        if (!NetworkGuard.IsAlive(money))
                            throw new InvalidOperationException("The money manager is unavailable.");
                        refundSentAt = Time.realtimeSinceStartup;
                        money!.CreateOnlineTransaction(BankTransactionObserver.RefundTitle, refund, 1f, BankTransactionObserver.RefundTitle);
                        refundSent = true;
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[PocketShop] Late card charge refund could not be sent: {ex.Message}");
                    }

                    if (refundSent)
                    {
                        bool refundConfirmed = false;
                        float refundDeadline = refundSentAt + BankConfirmationWindowSeconds;
                        while (Time.realtimeSinceStartup < refundDeadline)
                        {
                            if (!NetworkGuard.IsInMainScene) break;
                            if (BankTransactionObserver.TryFindApplied(BankTransactionObserver.RefundTitle, refund, refundSentAt, out _))
                            {
                                refundConfirmed = true;
                                break;
                            }
                            yield return null;
                        }

                        if (refundConfirmed)
                        {
                            ClearUncertain(
                                $"A late card charge of ${FormatMoney(refund)} was detected and refunded; the purchase lock was lifted.");
                        }
                        else
                        {
                            MelonLogger.Error($"[PocketShop] Late card charge was detected but its refund could not be confirmed. Lock stays. {BuildBankDiagnostics(payment, float.NaN, "late-charge-refund-unconfirmed")}");
                        }
                    }
                    yield break;
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
        finally
        {
            Interlocked.Exchange(ref _reconciliationActive, 0);
        }
    }

    private static void ClearUncertain(string reason)
    {
        Interlocked.Exchange(ref _transactionUncertain, 0);
        MelonLogger.Msg($"[PocketShop] {reason}");
    }

    /// <summary>
    /// Releases the single-flight guard and the pending state. Only releases the guard when the
    /// caller still owns the current purchase token (a scene reset invalidates stale routines).
    /// </summary>
    private static void CompletePendingPurchase(PendingBankPayment payment, int token)
    {
        lock (_pendingGate)
        {
            if (ReferenceEquals(_pendingPayment, payment))
                _pendingPayment = null;
        }
        SetPurchasePending(false);
        if (Volatile.Read(ref _purchaseToken) == token)
            Interlocked.Exchange(ref _purchaseInProgress, 0);
    }

    private static void SetPurchasePending(bool pending)
    {
        Interlocked.Exchange(ref _purchasePending, pending ? 1 : 0);
    }

    /// <summary>
    /// Session boundary (Main scene unloaded): the previous transaction can no longer apply,
    /// so the lock and any pending state are dropped deliberately — never per app open.
    /// </summary>
    public static void ResetForSceneReload()
    {
        bool wasUncertain = Volatile.Read(ref _transactionUncertain) != 0;
        Interlocked.Exchange(ref _transactionUncertain, 0);
        if (wasUncertain)
            MelonLogger.Warning("[PocketShop] Transaction lock cleared at the scene/session boundary; the previous transaction can no longer apply.");

        Interlocked.Increment(ref _purchaseToken);
        Interlocked.Exchange(ref _purchaseInProgress, 0);
        SetPurchasePending(false);
        lock (_pendingGate) { _pendingPayment = null; }
        lock (_completedPurchases) { _completedPurchases.Clear(); }
    }

    // ─────────────────────────────────────────────────────────────────────────────────────
    // Delivery / stock / refund phases
    // ─────────────────────────────────────────────────────────────────────────────────────

    private sealed class DeliveryPhaseResult
    {
        public int DeliveredCount;
        public bool Uncertain;
        public string UncertainMessage = string.Empty;
    }

    /// <summary>Cash path: payment applied synchronously — deliver, update stock, refund synchronously, finalize.</summary>
    private static PurchaseResultData CompleteCashDelivery(
        PurchaseResultData result, LiveShopOffer offer, PaymentMode mode, List<ItemInstance> instances,
        uint inventoryBefore, PurchasePricing pricing, int quantity, ItemPOCO item, MoneyManager money)
    {
        var delivery = RunDeliveryPhase(result, offer, instances, inventoryBefore, quantity, item);
        if (delivery.Uncertain)
            return MarkUncertain(result, delivery.UncertainMessage, BuildCashDiagnostics(offer, item, pricing, float.NaN, float.NaN, "delivery-measurement-failed"));

        result.Phase = PurchasePhase.StockUpdateStarted;
        bool stockConfirmed = TryUpdateStock(offer, delivery.DeliveredCount, out string stockFailure);

        float refund = 0f;
        bool refundConfirmed = true;
        if (delivery.DeliveredCount < quantity)
        {
            if (!PurchaseRules.TryCalculateRefund(pricing.UnitPrice, pricing.DeliveryFee, quantity, delivery.DeliveredCount, out refund))
                return MarkUncertain(result, "A safe partial refund could not be calculated; transaction state is unclear.",
                    BuildCashDiagnostics(offer, item, pricing, float.NaN, float.NaN, "refund-calculation-failed"));

            if (refund > 0f)
            {
                result.Phase = PurchasePhase.RefundStarted;
                if (TryApplyCashDelta(money, refund, out string refundFailure, out _, out _))
                {
                    result.TotalPaid = pricing.Total - refund;
                    result.NetPaidConfirmed = true;
                }
                else
                {
                    refundConfirmed = false;
                    MelonLogger.Error($"[PocketShop] Cash refund could not be confirmed for '{item.Name}': {refundFailure}");
                }
            }
            else
            {
                result.TotalPaid = pricing.Total;
                result.NetPaidConfirmed = true;
            }
            result.Phase = PurchasePhase.Partial;
        }

        return FinalizeDeliveryOutcome(result, delivery, pricing, mode, item, stockConfirmed, stockFailure, refund, refundConfirmed,
            BuildCashDiagnostics(offer, item, pricing, float.NaN, float.NaN, "sync"));
    }

    /// <summary>
    /// Transfers the prepared item instances, measures the inventory delta, and updates the
    /// delivered count. Never moves money; refunds are the caller's responsibility.
    /// </summary>
    private static DeliveryPhaseResult RunDeliveryPhase(
        PurchaseResultData result, LiveShopOffer offer, List<ItemInstance> instances,
        uint inventoryBefore, int quantity, ItemPOCO item)
    {
        var delivery = new DeliveryPhaseResult();
        result.Phase = PurchasePhase.DeliveryStarted;

        int completedCalls = 0;
        string deliveryError = string.Empty;
        for (int i = 0; i < instances.Count; i++)
        {
            try
            {
                PlayerInventory.Instance.AddItemToInventory(instances[i]);
                completedCalls++;
            }
            catch (Exception ex)
            {
                deliveryError = ex.Message;
                break;
            }
        }

        uint inventoryAfter;
        try { inventoryAfter = PlayerInventory.Instance.GetAmountOfItem(offer.Definition.ID); }
        catch (Exception ex)
        {
            delivery.Uncertain = true;
            delivery.UncertainMessage = $"Payment was confirmed, but delivery could not be measured. Do not retry until inventory and balance are checked. {ex.Message}";
            return delivery;
        }

        if (inventoryAfter < inventoryBefore)
        {
            delivery.Uncertain = true;
            delivery.UncertainMessage = "Inventory count moved backwards during delivery; purchase outcome is unclear.";
            return delivery;
        }

        ulong deliveredAmount = (ulong)inventoryAfter - inventoryBefore;
        if (deliveredAmount > (ulong)quantity || deliveredAmount > int.MaxValue)
        {
            delivery.Uncertain = true;
            delivery.UncertainMessage = "Inventory change exceeded the requested quantity; purchase outcome is unclear.";
            return delivery;
        }

        delivery.DeliveredCount = (int)deliveredAmount;
        result.DeliveredCount = delivery.DeliveredCount;
        result.Phase = PurchasePhase.DeliveryObserved;
        result.NetPaidConfirmed = delivery.DeliveredCount == quantity;
        if (deliveryError.Length > 0 || completedCalls != quantity)
            MelonLogger.Warning($"[PocketShop] delivery stopped after {completedCalls}/{quantity} calls for '{item.Name}': {deliveryError}");

        return delivery;
    }

    private static PurchaseResultData FinalizeDeliveryOutcome(
        PurchaseResultData result, DeliveryPhaseResult delivery, PurchasePricing pricing, PaymentMode mode,
        ItemPOCO item, bool stockConfirmed, string stockFailure, float refund, bool refundConfirmed, string diagnostics)
    {
        if (!stockConfirmed || !refundConfirmed)
        {
            string details = !stockConfirmed ? $" Stock change unconfirmed: {stockFailure}" : string.Empty;
            if (!refundConfirmed) details += " Refund change unconfirmed; it was not retried.";
            return MarkUncertain(result,
                $"Payment and {delivery.DeliveredCount}/{result.Quantity} delivered item(s) were observed, but the transaction did not fully reconcile. Do not retry until you check inventory and balance.{details}",
                diagnostics);
        }

        if (delivery.DeliveredCount == result.Quantity)
        {
            result.Result = BuyResult.Success;
            result.Phase = PurchasePhase.Committed;
            result.TotalPaid = pricing.Total;
            result.Message = $"Bought {result.Quantity}x '{item.Name}' for ${FormatMoney(pricing.Total)} ({(mode == PaymentMode.Cash ? "Cash" : "Card")}).";
            PlayPurchaseSuccessSafely();
            // Normal successful purchases stay silent in the log (only failures/warnings log).
            return result;
        }

        result.Result = BuyResult.NoInventorySpace;
        result.Message = delivery.DeliveredCount == 0
            ? $"No items were delivered. Refunded ${FormatMoney(refund)}."
            : $"Delivered {delivery.DeliveredCount}/{result.Quantity}x '{item.Name}'. Refunded ${FormatMoney(refund)} for undelivered items.";
        PlayPurchaseDeniedSafely();
        return result;
    }

    public static bool IsLevelRequirementSatisfied(StorableItemDefinition definition, out string message)
    {
        return TryCheckLevelRequirement(definition, out message);
    }

    private static bool TryCheckLevelRequirement(StorableItemDefinition definition, out string message)
    {
        message = string.Empty;
        if (!PocketShopConfig.EnforceLevelRequirementsStatic) return true;
        try
        {
            if (!definition.RequiresLevelToPurchase || definition.IsUnlocked) return true;
            string rank = FullRank.GetString(definition.RequiredRank);
            message = string.IsNullOrEmpty(rank) ? "This item is locked by its level requirement." : $"This item is locked (requires {rank}).";
            return false;
        }
        catch
        {
            message = "The item level requirement could not be verified.";
            return false;
        }
    }

    private static bool TryUpdateStock(LiveShopOffer offer, int deliveredCount, out string failure)
    {
        failure = string.Empty;
        var listing = offer.Listing;
        if (!IsInteropObjectAlive(listing))
        {
            failure = "The listing reference was destroyed.";
            return false;
        }

        try
        {
            bool limitedNow = listing.LimitedStock;
            int stockNow = listing.CurrentStock;
            if (deliveredCount == 0)
            {
                if (offer.Stock.Kind == StockKind.Unlimited)
                    return !limitedNow || Fail("Stock mode changed while no items were delivered.", out failure);
                return limitedNow && stockNow == offer.Stock.Quantity
                    || Fail("Stock changed despite zero confirmed delivery.", out failure);
            }

            if (!limitedNow)
            {
                if (offer.Stock.Kind == StockKind.Unlimited) return true;
                failure = "The listing changed from limited to unlimited stock.";
                return false;
            }
            if (offer.Stock.Kind != StockKind.LimitedAvailable)
            {
                failure = "The original limited stock could not be verified.";
                return false;
            }

            int original = offer.Stock.Quantity;
            int expected = original - deliveredCount;
            if (expected < 0)
            {
                failure = "Confirmed delivery exceeds the original stock snapshot.";
                return false;
            }

            // A native inventory call or another vanilla path may already have updated stock.
            // Accept only the exact expected result; never decrement twice.
            if (stockNow == expected)
            {
                UpdateItemStockSnapshot(offer, expected);
                return true;
            }
            if (stockNow != original)
            {
                failure = $"Stock changed from {original} to {stockNow} outside the expected purchase delta.";
                return false;
            }

            string setStockError = string.Empty;
            try { listing.SetStock(expected, true); }
            catch (Exception ex) { setStockError = ex.Message; }

            if (!IsInteropObjectAlive(listing) || !listing.LimitedStock || listing.CurrentStock != expected)
            {
                failure = setStockError.Length == 0
                    ? "The live listing did not confirm the expected stock value."
                    : $"SetStock threw and the expected value was not observed: {setStockError}";
                return false;
            }

            if (setStockError.Length > 0)
                MelonLogger.Warning($"SetStock threw after the expected stock value was observed: {setStockError}");
            UpdateItemStockSnapshot(offer, expected);
            return true;
        }
        catch (Exception ex)
        {
            failure = $"Live stock could not be read back: {ex.Message}";
            return false;
        }
    }

    private static bool Fail(string message, out string failure)
    {
        failure = message;
        return false;
    }

    private static void UpdateItemStockSnapshot(LiveShopOffer offer, int stock)
    {
        var state = PurchaseRules.ResolveStock(true, true, limitedStock: true, currentStock: stock);
        // The item POCO is display-only; updating it prevents the visible badge from lagging.
        // The next purchase still re-reads the listing.
        var items = ShopCatalog.Items;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item == null || !IsInteropObjectAlive(item.SourceListing)
                || item.SourceListing.Pointer != offer.Listing.Pointer) continue;
            item.Stock = state;
            item.CurrentStock = stock;
            item.IsInStock = state.CanBuy(1);
        }
        // Keep the directory tile's item counter in sync without a full catalog re-read.
        ShopCatalog.RefreshShopItemCount(offer.Shop);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────
    // Money helpers
    // ─────────────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Cash changes apply synchronously (no RPC), so they are confirmed by an immediate
    /// exact balance-delta read-back. Bank changes must NOT use this helper.
    /// </summary>
    private static bool TryApplyCashDelta(MoneyManager money, float delta, out string failure, out float before, out float after)
    {
        failure = string.Empty;
        before = float.NaN;
        after = float.NaN;

        if (!NetworkGuard.IsAlive(money))
        {
            failure = "The cash account is unavailable.";
            return false;
        }

        try { before = money.cashBalance; }
        catch (Exception ex)
        {
            failure = $"Cash balance could not be read before the money change: {ex.Message}";
            return false;
        }
        if (!float.IsFinite(before))
        {
            failure = "Cash balance was not finite before the money change.";
            return false;
        }

        Exception? mutationException = null;
        try { money.ChangeCashBalance(delta, true, false); }
        catch (Exception ex) { mutationException = ex; }

        try { after = money.cashBalance; }
        catch (Exception ex)
        {
            failure = $"The money method {(mutationException == null ? "returned" : "threw")}, but the cash balance read-back failed: {ex.Message}";
            return false;
        }

        if (!PurchaseRules.BalanceDeltaMatches(before, after, delta))
        {
            failure = mutationException == null
                ? "The money method returned, but the observed cash delta did not match."
                : $"The money method threw and the observed cash delta did not match: {mutationException.Message}";
            return false;
        }

        if (mutationException != null)
            MelonLogger.Warning($"The money method threw after the expected cash delta was observed: {mutationException.Message}");
        return true;
    }

    private static float ReadBalance(MoneyManager money, PaymentMode mode)
    {
        return mode == PaymentMode.Cash ? money.cashBalance : money.sync___get_value_onlineBalance();
    }

    private static bool TryReadBalance(MoneyManager? money, PaymentMode mode, out float balance)
    {
        balance = 0f;
        if (!NetworkGuard.IsAlive(money)) return false;
        try
        {
            balance = ReadBalance(money!, mode);
            return float.IsFinite(balance) && balance >= 0f;
        }
        catch
        {
            return false;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────────────
    // Session / diagnostics
    // ─────────────────────────────────────────────────────────────────────────────────────

    private static bool TryGetPaymentPreference(EPaymentType paymentType, out PaymentPreference preference)
    {
        switch (paymentType)
        {
            case EPaymentType.Cash:
                preference = PaymentPreference.Cash;
                return true;
            case EPaymentType.Online:
                preference = PaymentPreference.Online;
                return true;
            case EPaymentType.PreferCash:
                preference = PaymentPreference.PreferCash;
                return true;
            case EPaymentType.PreferOnline:
                preference = PaymentPreference.PreferOnline;
                return true;
            default:
                preference = PaymentPreference.Online;
                return false;
        }
    }

    public static bool IsSupportedPaymentType(EPaymentType paymentType)
    {
        return TryGetPaymentPreference(paymentType, out _);
    }

    /// <summary>
    /// True only in an offline singleplayer session.
    /// Layer 1 (workspace standard, fail-closed) refuses every multiplayer client.
    /// Layer 2 refuses an active lobby session as well — host included — because no proven
    /// synchronized purchase path exists; a lobby is only created when hosting or joining.
    /// A missing or unreadable lobby object is singleplayer: the read error must not block
    /// the primary use case (clients are already covered by layer 1).
    /// </summary>
    private static bool IsSingleplayerSession()
    {
        try
        {
            if (!NetworkGuard.IsHostOrSingleplayer()) return false;
        }
        catch
        {
            return false;
        }

        try
        {
            var lobby = Singleton<Lobby>.Instance;
            if (lobby == null || lobby.Pointer == IntPtr.Zero || lobby.WasCollected) return true;
            return !lobby.IsInLobby;
        }
        catch
        {
            return true;
        }
    }

    private static bool IsInteropObjectAlive(ItemInstance? instance)
    {
        if (instance == null) return false;
        try { return instance.Pointer != IntPtr.Zero && !instance.WasCollected; }
        catch { return false; }
    }

    private static bool IsInteropObjectAlive(ShopListing? listing)
    {
        if (listing == null) return false;
        try { return listing.Pointer != IntPtr.Zero && !listing.WasCollected; }
        catch { return false; }
    }

    private static void PlayPurchaseDeniedSafely()
    {
        try { SoundService.PlayPurchaseDenied(); }
        catch (Exception ex) { MelonLogger.Warning($"[PocketShop] Denied sound failed: {ex.Message}"); }
    }

    private static void PlayPurchaseSuccessSafely()
    {
        try { SoundService.PlayPurchaseSuccess(); }
        catch (Exception ex) { MelonLogger.Warning($"[PocketShop] Success sound failed after commit: {ex.Message}"); }
    }

    private static PurchaseResultData Reject(PurchaseResultData result, BuyResult code, string message)
    {
        result.Result = code;
        result.Phase = PurchasePhase.Rejected;
        result.Message = message;
        PlayPurchaseDeniedSafely();
        return result;
    }

    private static PurchaseResultData MarkUncertain(PurchaseResultData result, string message, string diagnostics)
    {
        Interlocked.Exchange(ref _transactionUncertain, 1);
        result.Result = BuyResult.TransactionUncertain;
        result.Phase = PurchasePhase.Uncertain;
        result.Message = message;
        MelonLogger.Error($"[PocketShop] Transaction uncertain: {message}");
        if (!string.IsNullOrEmpty(diagnostics))
            MelonLogger.Error($"[PocketShop] Transaction diagnostics: {diagnostics}");
        PlayPurchaseDeniedSafely();
        return result;
    }

    /// <summary>Diagnostics for a bank transaction (shop, item, rule, mode, fees, totals, delta, balances, confirmation).</summary>
    private static string BuildBankDiagnostics(PendingBankPayment payment, float observedAfter, string confirmationResult)
    {
        return $"shop='{payment.ShopName}' [{payment.ShopCode}] item='{payment.ItemName}' [{payment.ItemId}] "
            + $"rule={payment.Rule} mode=Card unit=${FormatMoney(payment.UnitPrice)} serviceFeePerUnit=${FormatMoney(payment.ServiceFeePerUnit)} "
            + $"deliveryFee=${FormatMoney(payment.DeliveryFee)} total=${FormatMoney(payment.Total)} expectedDelta={FormatMoney(payment.Amount)} "
            + $"balanceBefore=${FormatMoney(payment.Before)} balanceAfter=${FormatMoney(observedAfter)} confirmation={confirmationResult}";
    }

    /// <summary>Diagnostics for a cash transaction at a failure site.</summary>
    private static string BuildCashDiagnostics(LiveShopOffer offer, ItemPOCO item, PurchasePricing pricing, float before, float after, string confirmationResult)
    {
        return $"shop='{offer.Shop.ShopName}' [{item.ShopCode}] item='{item.Name}' [{item.ItemId}] "
            + $"rule={offer.PaymentType} mode=Cash unit=${FormatMoney(pricing.UnitPrice)} deliveryFee=${FormatMoney(pricing.DeliveryFee)} "
            + $"total=${FormatMoney(pricing.Total)} expectedDelta={FormatMoney(-pricing.Total)} "
            + $"balanceBefore=${FormatMoney(before)} balanceAfter=${FormatMoney(after)} confirmation={confirmationResult}";
    }

    /// <summary>State of one sent bank transaction while its deferred confirmation runs.</summary>
    private sealed class PendingBankPayment
    {
        public string Title = string.Empty;
        public float Amount;          // signed delta: negative charge, positive refund
        public float Before;          // bank balance snapshot before the RPC was sent
        public float SentAt;          // Time.realtimeSinceStartup of the send
        public string ShopCode = string.Empty;
        public string ShopName = string.Empty;
        public string ItemId = string.Empty;
        public string ItemName = string.Empty;
        public string Rule = string.Empty;
        public float UnitPrice;
        public float ServiceFeePerUnit;
        public float DeliveryFee;
        public float Total;
    }
}

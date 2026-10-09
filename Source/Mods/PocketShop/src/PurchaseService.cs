using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Il2CppFishNet;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Levelling;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using PocketShop.Config;
using S1Mods.Shared;
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
    TransactionUncertain,
    Error
}

public enum PurchasePhase
{
    Rejected,
    Preparing,
    Prepared,
    PaymentStarted,
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
}

/// <summary>
/// Validates live purchase data, then performs one guarded transaction. Vanilla IL2CPP exposes
/// independent money, inventory, and stock mutations rather than a verified atomic phone-purchase API.
/// </summary>
public static class PurchaseService
{
    public const int UnlimitedStockSentinel = PurchaseRules.UnlimitedStockSentinel;
    private static int _purchaseInProgress;
    private static int _transactionUncertain;

    public static bool HasUncertainTransaction => Volatile.Read(ref _transactionUncertain) != 0;
    public static bool IsSingleplayerPurchaseContext => NetworkGuard.IsInMainScene && IsSingleplayerSession();
    public static bool IsPurchaseAvailableInCurrentSession => IsSingleplayerPurchaseContext && !HasUncertainTransaction;

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

        bool nativeMutationAttempted = false;
        try
        {
            if (HasUncertainTransaction)
                return MarkUncertain(result,
                    "A previous purchase could not be reconciled. PocketShop is paused for this game session; check inventory and balances before restarting.");
            if (!NetworkGuard.IsInMainScene)
                return Reject(result, BuyResult.ShopUnavailable, "Purchases are only available in the active gameplay scene.");
            if (!IsSingleplayerSession())
                return Reject(result, BuyResult.MultiplayerUnavailable,
                    "Purchasing is unavailable in multiplayer until a verified host-authoritative purchase path is available.");

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
                if (!TryApplyMoneyDelta(money!, effectiveMode, -pricing.Total, out string paymentFailure))
                {
                    return MarkUncertain(result,
                        $"Payment state is unclear; no items were transferred. Check the balance before retrying. {paymentFailure}");
                }
                result.TotalPaid = pricing.Total;
            }
            result.Phase = PurchasePhase.PaymentConfirmed;

            result.Phase = PurchasePhase.DeliveryStarted;
            nativeMutationAttempted = true;
            int completedCalls = 0;
            string deliveryError = string.Empty;
            for (int i = 0; i < instances.Count; i++)
            {
                try
                {
                    inventory!.AddItemToInventory(instances[i]);
                    completedCalls++;
                }
                catch (Exception ex)
                {
                    deliveryError = ex.Message;
                    break;
                }
            }

            uint inventoryAfter;
            try { inventoryAfter = inventory!.GetAmountOfItem(offer.Definition.ID); }
            catch (Exception ex)
            {
                return MarkUncertain(result,
                    $"Payment was confirmed, but delivery could not be measured. Do not retry until inventory and balance are checked. {ex.Message}");
            }

            if (inventoryAfter < inventoryBefore)
                return MarkUncertain(result, "Inventory count moved backwards during delivery; purchase outcome is unclear.");

            ulong deliveredAmount = (ulong)inventoryAfter - inventoryBefore;
            if (deliveredAmount > (ulong)quantity || deliveredAmount > int.MaxValue)
                return MarkUncertain(result, "Inventory change exceeded the requested quantity; purchase outcome is unclear.");

            int deliveredCount = (int)deliveredAmount;
            result.DeliveredCount = deliveredCount;
            result.Phase = PurchasePhase.DeliveryObserved;
            result.NetPaidConfirmed = deliveredCount == quantity;
            if (deliveryError.Length > 0 || completedCalls != quantity)
                MelonLogger.Warning($"PocketShop delivery stopped after {completedCalls}/{quantity} calls for '{item.Name}': {deliveryError}");

            result.Phase = PurchasePhase.StockUpdateStarted;
            bool stockConfirmed = TryUpdateStock(offer, deliveredCount, out string stockFailure);
            float refund = 0f;
            bool refundConfirmed = true;
            if (deliveredCount < quantity)
            {
                if (!PurchaseRules.TryCalculateRefund(pricing.UnitPrice, pricing.DeliveryFee, quantity, deliveredCount, out refund))
                    return MarkUncertain(result, "A safe partial refund could not be calculated; transaction state is unclear.");

                if (refund > 0f)
                {
                    result.Phase = PurchasePhase.RefundStarted;
                    if (TryApplyMoneyDelta(money!, effectiveMode, refund, out string refundFailure))
                    {
                        result.TotalPaid = pricing.Total - refund;
                        result.NetPaidConfirmed = true;
                    }
                    else
                    {
                        refundConfirmed = false;
                        MelonLogger.Error($"PocketShop refund could not be confirmed for '{item.Name}': {refundFailure}");
                    }
                }
                else
                {
                    result.TotalPaid = pricing.Total;
                    result.NetPaidConfirmed = true;
                }
                result.Phase = PurchasePhase.Partial;
            }

            if (!stockConfirmed || !refundConfirmed)
            {
                string details = !stockConfirmed ? $" Stock change unconfirmed: {stockFailure}" : string.Empty;
                if (!refundConfirmed) details += " Refund change unconfirmed; it was not retried.";
                return MarkUncertain(result,
                    $"Payment and {deliveredCount}/{quantity} delivered item(s) were observed, but the transaction did not fully reconcile. Do not retry until you check inventory and balance.{details}");
            }

            if (deliveredCount == quantity)
            {
                result.Result = BuyResult.Success;
                result.Phase = PurchasePhase.Committed;
                result.TotalPaid = pricing.Total;
                result.Message = $"Bought {quantity}x '{item.Name}' for ${FormatMoney(pricing.Total)} ({(effectiveMode == PaymentMode.Cash ? "Cash" : "Card")}).";
                PlayPurchaseSuccessSafely();
                MelonLogger.Msg($"[PocketShop] {result.Message} (unit ${FormatMoney(pricing.UnitPrice)})");
                return result;
            }

            result.Result = BuyResult.NoInventorySpace;
            result.Message = deliveredCount == 0
                ? $"No items were delivered. Refunded ${FormatMoney(refund)}."
                : $"Delivered {deliveredCount}/{quantity}x '{item.Name}'. Refunded ${FormatMoney(refund)} for undelivered items.";
            PlayPurchaseDeniedSafely();
            return result;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PocketShop transaction exception at {result.Phase}: {ex.Message}");
            if (nativeMutationAttempted)
                return MarkUncertain(result, $"Transaction stopped at {result.Phase}; no automatic retry or extra money change was made. Check inventory and balance. {ex.Message}");
            return Reject(result, BuyResult.Error, $"Purchase was cancelled before any native mutation: {ex.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _purchaseInProgress, 0);
        }
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
    }

    private static bool TryApplyMoneyDelta(MoneyManager money, PaymentMode mode, float delta, out string failure)
    {
        failure = string.Empty;
        if (!NetworkGuard.IsAlive(money) || (mode != PaymentMode.Cash && mode != PaymentMode.Bank))
        {
            failure = "The selected money account is unavailable.";
            return false;
        }

        float before;
        try { before = ReadBalance(money, mode); }
        catch (Exception ex)
        {
            failure = $"Balance could not be read before the money change: {ex.Message}";
            return false;
        }
        if (!float.IsFinite(before))
        {
            failure = "Balance was not finite before the money change.";
            return false;
        }

        Exception? mutationException = null;
        try
        {
            if (mode == PaymentMode.Cash)
            {
                money.ChangeCashBalance(delta, true, false);
            }
            else
            {
                string title = delta < 0f ? "PocketShop Order" : "PocketShop Refund";
                string note = delta < 0f ? "PocketShop Order" : "PocketShop Order Refund";
                money.CreateOnlineTransaction(title, delta, 1f, note);
            }
        }
        catch (Exception ex)
        {
            mutationException = ex;
        }

        float after;
        try { after = ReadBalance(money, mode); }
        catch (Exception ex)
        {
            failure = $"Money method {(mutationException == null ? "returned" : "threw")}, but balance read-back failed: {ex.Message}";
            return false;
        }

        if (!PurchaseRules.BalanceDeltaMatches(before, after, delta))
        {
            failure = mutationException == null
                ? "Money method returned, but the observed balance delta did not match."
                : $"Money method threw and the observed balance delta did not match: {mutationException.Message}";
            return false;
        }

        if (mutationException != null)
            MelonLogger.Warning($"Money method threw after the expected balance delta was observed: {mutationException.Message}");
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

    private static bool IsSingleplayerSession()
    {
        try
        {
            var networkManager = InstanceFinder.NetworkManager;
            return networkManager == null
                || networkManager.Pointer == IntPtr.Zero
                || networkManager.WasCollected
                || (UnityEngine.Object)networkManager == null;
        }
        catch
        {
            // An unresolvable network state is not permission to mutate shared economy state.
            return false;
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

    private static PurchaseResultData MarkUncertain(PurchaseResultData result, string message)
    {
        Interlocked.Exchange(ref _transactionUncertain, 1);
        result.Result = BuyResult.TransactionUncertain;
        result.Phase = PurchasePhase.Uncertain;
        result.Message = message;
        MelonLogger.Error($"[PocketShop] Transaction uncertain: {message}");
        PlayPurchaseDeniedSafely();
        return result;
    }
}

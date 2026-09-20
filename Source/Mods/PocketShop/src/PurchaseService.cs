using System;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using PocketShop.Config;
using S1Mods.Shared;
using EPaymentType = Il2CppScheduleOne.UI.Shop.ShopInterface.EPaymentType;

namespace PocketShop.Services;

/// <summary>
/// Outcome of a purchase attempt.
/// </summary>
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
    Error
}

/// <summary>
/// Detailed result package from a purchase operation, used by UI &amp; Toast notifications.
/// </summary>
public sealed class PurchaseResultData
{
    public BuyResult Result { get; set; }
    public PaymentMode PaymentModeUsed { get; set; }
    public float TotalPaid { get; set; }
    public float UnitPriceWithFee { get; set; }
    public int Quantity { get; set; }
    public ItemPOCO Item { get; set; } = null!;
    public string Message { get; set; } = string.Empty;

    public bool IsSuccess => Result == BuyResult.Success;
}

public static class PurchaseService
{
    /// <summary>
    /// Sentinel value for items with unlimited stock. Used consistently across
    /// PurchaseService and QuantitySelector (max=99) so they always agree.
    /// </summary>
    public const int UnlimitedStockSentinel = -1;

    public static PurchaseResultData Buy(ItemPOCO item)
    {
        return BuyWithQuantity(item, 1, PocketShopConfig.PaymentModeStatic);
    }

    /// <summary>
    /// Calculates the full pricing details for an item purchase.
    /// </summary>
    public static void CalculatePricing(ItemPOCO item, int qty, out float perUnit, out float subtotal, out float deliveryFee, out float total)
    {
        float feePercent = PocketShopConfig.ServiceFeePercentStatic;
        perUnit = item.Price * (1f + feePercent / 100f);
        subtotal = perUnit * qty;
        deliveryFee = PocketShopConfig.DeliveryFeeFlatStatic;
        total = subtotal + deliveryFee;
    }

    /// <summary>
    /// Checks whether the player can afford the given total under the shop's vanilla payment rule.
    /// v0.3.1: Black-market shops charge Cash, clean/legal shops charge by card (Online).
    /// PreferCash/PreferOnline mean the shop accepts both — check in that order of preference.
    /// </summary>
    public static bool CanAfford(float total, EPaymentType shopPaymentType, out PaymentMode effectiveMode)
    {
        var money = MoneyManager.Instance;
        effectiveMode = PaymentMode.Bank;
        if (!NetworkGuard.IsAlive(money)) return false;

        switch (shopPaymentType)
        {
            case EPaymentType.Cash:
                effectiveMode = PaymentMode.Cash;
                return money.cashBalance >= total;

            case EPaymentType.PreferCash:
                // Both accepted, cash preferred
                if (money.cashBalance >= total) { effectiveMode = PaymentMode.Cash; return true; }
                if (money.onlineBalance >= total) { effectiveMode = PaymentMode.Bank; return true; }
                effectiveMode = PaymentMode.Cash;
                return false;

            case EPaymentType.PreferOnline:
                // Both accepted, card preferred
                if (money.onlineBalance >= total) { effectiveMode = PaymentMode.Bank; return true; }
                if (money.cashBalance >= total) { effectiveMode = PaymentMode.Cash; return true; }
                effectiveMode = PaymentMode.Bank;
                return false;

            default: // EPaymentType.Online (legal/card-only)
                effectiveMode = PaymentMode.Bank;
                return money.onlineBalance >= total;
        }
    }

    /// <summary>
    /// Buys <paramref name="qty"/> units of <paramref name="item"/>.
    /// v0.3.0: Bank-card only (Schedule I charges legal shops via card). The
    /// <paramref name="mode"/> parameter is kept for source compatibility and ignored.
    /// Decrements vanilla stock, adds items to inventory, and triggers audio/logging.
    /// </summary>
    public static PurchaseResultData BuyWithQuantity(ItemPOCO item, int qty, PaymentMode mode = PaymentMode.Bank)
    {
        var result = new PurchaseResultData
        {
            Item = item,
            Quantity = qty,
            PaymentModeUsed = mode
        };

        if (item == null || item.Definition == null)
        {
            result.Result = BuyResult.DefinitionNull;
            result.Message = "Item definition not found.";
            return result;
        }

        if (qty < 1)
        {
            result.Result = BuyResult.InvalidQuantity;
            result.Message = "Invalid quantity.";
            return result;
        }

        if (PocketShopConfig.EnforceLevelRequirementsStatic && !item.IsAvailableToPlayer)
        {
            result.Result = BuyResult.LevelLocked;
            string rankNotice = !string.IsNullOrEmpty(item.RequiredRankString) ? $" (Requires {item.RequiredRankString})" : "";
            result.Message = $"Item is locked{rankNotice}.";
            return result;
        }

        if (!NetworkGuard.IsInMainScene)
        {
            result.Result = BuyResult.Error;
            result.Message = "Cannot purchase outside main scene.";
            return result;
        }

        var money = MoneyManager.Instance;
        if (!NetworkGuard.IsAlive(money))
        {
            result.Result = BuyResult.DefinitionNull;
            result.Message = "Money system unavailable.";
            return result;
        }

        // Bug-Audit 2026-09-12: the POCO's CurrentStock is a snapshot from the last
        // ShopCatalog.Refresh (only fired on App-Open). Reading it here means co-op
        // clients can oversell — another player has already drained the live listing.
        // Prefer the live listing's stock when available; fall back to the POCO only
        // when the native reference is missing/collected.
        int liveStock = item.CurrentStock;
        bool useLive = false;
        try
        {
            if (item.SourceListing != null && item.SourceListing.Pointer != IntPtr.Zero && !item.SourceListing.WasCollected)
            {
                liveStock = item.SourceListing.CurrentStock;
                useLive = true;
            }
        }
        catch { }
        bool liveInStock;
        try
        {
            liveInStock = (useLive && item.SourceListing != null)
                ? item.SourceListing.IsInStock
                : item.IsInStock;
        }
        catch { liveInStock = item.IsInStock; }

        int availableStock = !liveInStock ? 0 : (liveStock > 0 ? liveStock : UnlimitedStockSentinel);
        if (availableStock == 0)
        {
            result.Result = BuyResult.StockEmpty;
            result.Message = $"'{item.Name}' is out of stock!";
            return result;
        }

        if (availableStock != UnlimitedStockSentinel && qty > availableStock)
        {
            result.Result = BuyResult.InvalidQuantity;
            result.Message = $"Only {availableStock}x in stock.";
            return result;
        }

        CalculatePricing(item, qty, out float perUnit, out _, out float deliveryFee, out float total);
        result.UnitPriceWithFee = perUnit;
        result.TotalPaid = total;

        if (!CanAfford(total, item.ShopPaymentType, out PaymentMode effectiveMode))
        {
            if (effectiveMode == PaymentMode.Cash)
            {
                result.Result = BuyResult.NotEnoughCash;
                result.Message = $"Not enough Cash! Need ${total:F0} (have ${money.cashBalance:F0})";
            }
            else
            {
                result.Result = BuyResult.NotEnoughBank;
                result.Message = $"Not enough card funds! Need ${total:F0} (card balance ${money.onlineBalance:F0})";
            }
            SoundService.PlayPurchaseDenied();
            return result;
        }

        result.PaymentModeUsed = effectiveMode;

        var firstInstance = item.Definition.GetDefaultInstance(1);
        if (firstInstance == null)
        {
            result.Result = BuyResult.DefinitionNull;
            result.Message = "Failed to create item instance.";
            return result;
        }

        var inventory = PlayerInventory.Instance;
        if (!NetworkGuard.IsAlive(inventory))
        {
            result.Result = BuyResult.NoInventorySpace;
            result.Message = "Player inventory not found.";
            return result;
        }

        if (!inventory.CanItemFitInInventory(firstInstance, qty))
        {
            result.Result = BuyResult.NoInventorySpace;
            result.Message = $"Inventory full! Cannot fit {qty}x '{item.Name}'.";
            SoundService.PlayPurchaseDenied();
            return result;
        }

        // Reuse probe instance to save one allocation (quantity probe already created)
        var probeInstance = firstInstance;
        bool paymentExecuted = false;
        try
        {
            // Pre-create ALL instances BEFORE payment so a partial failure cannot
            // leave the player charged for items that were never delivered.
            var instances = new System.Collections.Generic.List<ItemInstance>();
            // Reuse probe as first instance
            instances.Add(probeInstance);
            for (int i = 1; i < qty; i++)
            {
                var inst = item.Definition.GetDefaultInstance(1);
                if (inst == null)
                {
                    result.Result = BuyResult.DefinitionNull;
                    result.Message = "Failed to create item instance.";
                    return result;
                }
                instances.Add(inst);
            }

            // Execute payment — v0.3.1: follow the shop's vanilla payment rule
            // (Black Market = Cash via ChangeCashBalance, clean shops = Card via
            // CreateOnlineTransaction; Prefer* variants resolved by CanAfford).
            // v0.3.2: visualizeChange=true so cash payments show the vanilla HUD popup
            // (matching in-world cash spending — the player sees "-$X" like at a dealer).
            try
            {
                if (effectiveMode == PaymentMode.Cash)
                {
                    money.ChangeCashBalance(-total, true, false);
                }
                else
                {
                    string note = $"PocketShop: {item.Name} x{qty}";
                    money.CreateOnlineTransaction(note, -total, 1, "PocketShop Order");
                }
                paymentExecuted = true;
            }
            catch (Exception paymentEx)
            {
                // Payment never happened — do NOT refund (would double money).
                MelonLogger.Error($"Payment failed for '{item.Name}': {paymentEx.Message}");
                result.Result = BuyResult.Error;
                result.Message = $"Payment failed: {paymentEx.Message}";
                SoundService.PlayPurchaseDenied();
                return result;
            }

            // Transfer items to player inventory
            int deliveredCount = 0;
            try
            {
                for (int i = 0; i < qty; i++)
                {
                    inventory.AddItemToInventory(instances[i]);
                    deliveredCount++;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Inventory transfer failed for '{item.Name}': {ex.Message}");
                // Partial delivery: 0..deliveredCount-1 reached the player, the rest did not.
                // Refund only the undelivered remainder so delivered items stay paid for.
                // Units: perUnit ($/unit) * undelivered (units) + deliveryFee ($) = $.
                int undelivered = qty - deliveredCount;
                float refund = perUnit * undelivered + (deliveredCount == 0 ? deliveryFee : 0f);
                // Rollback payment so the player is never charged for undelivered items.
                // Refunds keep visualizeChange=true as well — a visible "+$X" confirms the rollback.
                try
                {
                    if (effectiveMode == PaymentMode.Cash)
                    {
                        money.ChangeCashBalance(refund, true, false);
                    }
                    else
                    {
                        money.CreateOnlineTransaction("PocketShop Refund", refund, 1, "PocketShop Order Rollback");
                    }
                    // Decrement vendor stock for the items that were actually delivered.
                    if (deliveredCount > 0 && availableStock != UnlimitedStockSentinel && item.SourceListing != null)
                    {
                        int newStock = System.Math.Max(0, item.SourceListing.CurrentStock - deliveredCount);
                        item.SourceListing.SetStock(newStock, true);
                        item.CurrentStock = newStock;
                        item.IsInStock = newStock > 0;
                    }
                    result.TotalPaid = total - refund;
                    result.Quantity = deliveredCount;
                    result.Result = BuyResult.NoInventorySpace;
                    if (deliveredCount > 0)
                    {
                        result.Message = $"Inventory full! Delivered {deliveredCount}x '{item.Name}', refunded ${refund:F0} for {undelivered}x undelivered.";
                    }
                    else
                    {
                        result.Message = $"Inventory full! Cannot fit {qty}x '{item.Name}'. Payment refunded.";
                    }
                    SoundService.PlayPurchaseDenied();
                    return result;
                }
                catch (Exception refundEx)
                {
                    MelonLogger.Error($"Refund failed after inventory error: {refundEx.Message}");
                    result.Result = BuyResult.Error;
                    result.Message = $"Transaction error (refund attempted): {refundEx.Message}";
                    SoundService.PlayPurchaseDenied();
                    return result;
                }
            }

            // Decrement vendor stock (isolated: stock update failure must NEVER trigger a refund for already delivered items)
            if (availableStock != UnlimitedStockSentinel && item.SourceListing != null)
            {
                try
                {
                    if (item.SourceListing.Pointer != IntPtr.Zero && !item.SourceListing.WasCollected)
                    {
                        int newStock = System.Math.Max(0, item.SourceListing.CurrentStock - qty);
                        item.SourceListing.SetStock(newStock, true);
                        item.CurrentStock = newStock;
                        item.IsInStock = newStock > 0;
                    }
                }
                catch (Exception stockEx)
                {
                    MelonLogger.Warning($"Failed to update vendor stock: {stockEx.Message}");
                }
            }

            result.Result = BuyResult.Success;
            string payLabel = effectiveMode == PaymentMode.Cash ? "Cash" : "Card";
            result.Message = $"Bought {qty}x '{item.Name}' for ${total:F0} ({payLabel})";

            SoundService.PlayPurchaseSuccess();
            MelonLogger.Msg($"{result.Message} (Unit: ${perUnit:F2})");
            return result;
        }
        catch (Exception ex)
        {
            // Outer gap: if payment already executed but exception happened between payment and transfer (or during pre-create after payment), refund
            if (paymentExecuted)
            {
                try
                {
                    if (effectiveMode == PaymentMode.Cash) money.ChangeCashBalance(total, true, false);
                    else money.CreateOnlineTransaction("PocketShop Refund", total, 1, "PocketShop Order Rollback (outer)");
                    MelonLogger.Msg($"Outer purchase exception — payment refunded: {ex.Message}");
                }
                catch (Exception refundEx) { MelonLogger.Error($"Outer refund failed: {refundEx.Message}"); }
            }
            else
            {
                MelonLogger.Error($"Purchase failed for '{item.Name}' (no payment executed): {ex.Message}");
            }
            result.Result = BuyResult.Error;
            result.Message = $"Transaction error: {ex.Message}";
            SoundService.PlayPurchaseDenied();
            return result;
        }
    }
}

using System;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using PocketShop.Config;
using S1Mods.Shared;

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
    /// Checks whether the player can afford the given total under the active payment mode.
    /// </summary>
    public static bool CanAfford(float total, PaymentMode mode, out PaymentMode effectiveMode)
    {
        var money = MoneyManager.Instance;
        effectiveMode = mode;
        if (money == null) return false;

        float cash = money.cashBalance;
        float bank = money.onlineBalance;

        if (mode == PaymentMode.Cash)
        {
            effectiveMode = PaymentMode.Cash;
            return cash >= total;
        }

        if (mode == PaymentMode.Bank)
        {
            effectiveMode = PaymentMode.Bank;
            return bank >= total;
        }

        // Auto mode: prefer Cash, fallback to Bank
        if (cash >= total)
        {
            effectiveMode = PaymentMode.Cash;
            return true;
        }

        if (bank >= total)
        {
            effectiveMode = PaymentMode.Bank;
            return true;
        }

        effectiveMode = PaymentMode.Auto;
        return false;
    }

    /// <summary>
    /// Buys <paramref name="qty"/> units of <paramref name="item"/> using the specified <paramref name="mode"/>.
    /// Supports Cash, Bank (online transaction), and Auto (Cash first, Bank fallback).
    /// Decrements vanilla stock, adds items to inventory, and triggers audio/logging.
    /// </summary>
    public static PurchaseResultData BuyWithQuantity(ItemPOCO item, int qty, PaymentMode mode = PaymentMode.Auto)
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

        var money = MoneyManager.Instance;
        if (money == null)
        {
            result.Result = BuyResult.DefinitionNull;
            result.Message = "Money system unavailable.";
            return result;
        }

        int availableStock = !item.IsInStock ? 0 : (item.CurrentStock > 0 ? item.CurrentStock : UnlimitedStockSentinel);
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

        if (!CanAfford(total, mode, out PaymentMode effectiveMode))
        {
            if (mode == PaymentMode.Cash)
            {
                result.Result = BuyResult.NotEnoughCash;
                result.Message = $"Not enough Cash! Need ${total:F0} (have ${money.cashBalance:F0})";
            }
            else if (mode == PaymentMode.Bank)
            {
                result.Result = BuyResult.NotEnoughBank;
                result.Message = $"Not enough Bank funds! Need ${total:F0} (have ${money.onlineBalance:F0})";
            }
            else
            {
                result.Result = BuyResult.NotEnoughFunds;
                result.Message = $"Insufficient funds! Need ${total:F0} (Cash ${money.cashBalance:F0}, Bank ${money.onlineBalance:F0})";
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
        if (inventory == null)
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

            // Execute payment (visualizeChange = false to disable large screen cash animation)
            try
            {
                if (effectiveMode == PaymentMode.Cash)
                {
                    money.ChangeCashBalance(-total, false, false);
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
                try
                {
                    if (effectiveMode == PaymentMode.Cash)
                    {
                        money.ChangeCashBalance(refund, false, false);
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

            // Decrement vendor stock
            if (availableStock != UnlimitedStockSentinel && item.SourceListing != null)
            {
                int newStock = System.Math.Max(0, item.SourceListing.CurrentStock - qty);
                item.SourceListing.SetStock(newStock, true);
                item.CurrentStock = newStock;
                item.IsInStock = newStock > 0;
            }

            result.Result = BuyResult.Success;
            string payLabel = effectiveMode == PaymentMode.Cash ? "Cash" : "Bank";
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
                    if (effectiveMode == PaymentMode.Cash) money.ChangeCashBalance(total, false, false);
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

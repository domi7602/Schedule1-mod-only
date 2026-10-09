using System;

namespace PocketShop.Services;

public enum StockKind
{
    Unknown,
    NotOffered,
    Unlimited,
    LimitedAvailable,
    LimitedEmpty
}

public readonly record struct StockState(StockKind Kind, int Quantity, int QuantityLimit)
{
    public bool CanBuy(int quantity)
    {
        if (quantity < 1) return false;
        return Kind switch
        {
            StockKind.Unlimited => true,
            StockKind.LimitedAvailable => quantity <= Quantity,
            _ => false
        };
    }
}

public readonly record struct PurchasePricing(float UnitPrice, float Subtotal, float DeliveryFee, float Total);

public enum PaymentPreference
{
    Cash,
    Online,
    PreferCash,
    PreferOnline
}

public enum PaymentAccount
{
    Cash,
    Bank
}

/// <summary>Pure validation shared by the live purchase path and UI stock/price presentation.</summary>
public static class PurchaseRules
{
    public const int UnlimitedStockSentinel = -1;

    public static StockState ResolveStock(bool listingValid, bool shouldShow, bool limitedStock, int currentStock)
    {
        if (!listingValid) return new StockState(StockKind.Unknown, 0, 0);
        if (!shouldShow) return new StockState(StockKind.NotOffered, 0, 0);
        if (!limitedStock) return new StockState(StockKind.Unlimited, UnlimitedStockSentinel, UnlimitedStockSentinel);
        if (currentStock < 0) return new StockState(StockKind.Unknown, 0, 0);
        if (currentStock == 0) return new StockState(StockKind.LimitedEmpty, 0, 0);
        return new StockState(StockKind.LimitedAvailable, currentStock, currentStock);
    }

    /// <summary>
    /// Uses the existing float formula (price * (1 + percent / 100), then quantity + flat delivery)
    /// without adding a rounding rule. Rejects invalid inputs and non-representable results.
    /// </summary>
    public static bool TryCalculatePricing(float price, float serviceFeePercent, float deliveryFee, int quantity, out PurchasePricing pricing)
    {
        pricing = default;
        if (quantity < 1 || !IsFiniteNonNegative(price) || !IsFiniteNonNegative(serviceFeePercent)
            || !IsFiniteNonNegative(deliveryFee))
        {
            return false;
        }

        float multiplier = 1f + serviceFeePercent / 100f;
        if (!IsFiniteNonNegative(multiplier)) return false;

        float unitPrice = price * multiplier;
        if (!IsFiniteNonNegative(unitPrice)) return false;

        float subtotal = unitPrice * quantity;
        if (!IsFiniteNonNegative(subtotal)) return false;

        float total = subtotal + deliveryFee;
        if (!IsFiniteNonNegative(total)) return false;

        pricing = new PurchasePricing(unitPrice, subtotal, deliveryFee, total);
        return true;
    }

    public static bool TryCalculateRefund(float unitPrice, float deliveryFee, int quantity, int deliveredCount, out float refund)
    {
        refund = 0f;
        if (!IsFiniteNonNegative(unitPrice) || !IsFiniteNonNegative(deliveryFee)
            || quantity < 1 || deliveredCount < 0 || deliveredCount > quantity)
        {
            return false;
        }

        int undelivered = quantity - deliveredCount;
        refund = unitPrice * undelivered + (deliveredCount == 0 ? deliveryFee : 0f);
        if (!IsFiniteNonNegative(refund))
        {
            refund = 0f;
            return false;
        }
        return true;
    }

    public static bool TryResolvePayment(
        PaymentPreference preference, float total, float cashBalance, float bankBalance, out PaymentAccount account)
    {
        account = preference == PaymentPreference.Online || preference == PaymentPreference.PreferOnline
            ? PaymentAccount.Bank
            : PaymentAccount.Cash;
        if (!IsFiniteNonNegative(total) || !IsFiniteNonNegative(cashBalance) || !IsFiniteNonNegative(bankBalance))
            return false;

        switch (preference)
        {
            case PaymentPreference.Cash:
                account = PaymentAccount.Cash;
                return cashBalance >= total;
            case PaymentPreference.Online:
                account = PaymentAccount.Bank;
                return bankBalance >= total;
            case PaymentPreference.PreferCash:
                if (cashBalance >= total) { account = PaymentAccount.Cash; return true; }
                if (bankBalance >= total) { account = PaymentAccount.Bank; return true; }
                account = PaymentAccount.Cash;
                return false;
            case PaymentPreference.PreferOnline:
                if (bankBalance >= total) { account = PaymentAccount.Bank; return true; }
                if (cashBalance >= total) { account = PaymentAccount.Cash; return true; }
                account = PaymentAccount.Bank;
                return false;
            default:
                account = PaymentAccount.Bank;
                return false;
        }
    }

    private static bool IsFiniteNonNegative(float value) => float.IsFinite(value) && value >= 0f;

    /// <summary>Checks that the exposed float balance changed by the exact representable amount.</summary>
    public static bool BalanceDeltaMatches(float before, float after, float expectedDelta)
    {
        if (!float.IsFinite(before) || !float.IsFinite(after) || !float.IsFinite(expectedDelta)) return false;
        if (expectedDelta == 0f) return after.Equals(before);

        float expectedAfter = before + expectedDelta;
        if (!float.IsFinite(expectedAfter) || expectedAfter.Equals(before)) return false;
        return after.Equals(expectedAfter)
            && (expectedDelta < 0f ? after < before : after > before);
    }

}

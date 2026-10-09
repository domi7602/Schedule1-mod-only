using PocketShop.Services;
using Xunit;

namespace PocketShop.Tests;

public sealed class PurchaseRulesTests
{
    [Fact]
    public void UnlimitedListingRemainsUnlimitedWhenStockCounterIsZeroOrNegative()
    {
        var zero = PurchaseRules.ResolveStock(listingValid: true, shouldShow: true, limitedStock: false, currentStock: 0);
        var negative = PurchaseRules.ResolveStock(listingValid: true, shouldShow: true, limitedStock: false, currentStock: -8);

        Assert.Equal(StockKind.Unlimited, zero.Kind);
        Assert.Equal(StockKind.Unlimited, negative.Kind);
        Assert.Equal(PurchaseRules.UnlimitedStockSentinel, zero.QuantityLimit);
    }

    [Fact]
    public void LimitedListingWithZeroStockCannotBePurchased()
    {
        var stock = PurchaseRules.ResolveStock(listingValid: true, shouldShow: true, limitedStock: true, currentStock: 0);

        Assert.Equal(StockKind.LimitedEmpty, stock.Kind);
        Assert.False(stock.CanBuy(1));
        Assert.Equal(0, stock.QuantityLimit);
    }

    [Fact]
    public void LimitedPositiveStockCapsPurchaseQuantity()
    {
        var stock = PurchaseRules.ResolveStock(listingValid: true, shouldShow: true, limitedStock: true, currentStock: 10);

        Assert.Equal(StockKind.LimitedAvailable, stock.Kind);
        Assert.True(stock.CanBuy(10));
        Assert.False(stock.CanBuy(11));
        Assert.Equal(10, stock.QuantityLimit);
    }

    [Fact]
    public void InvalidListingNeverUsesItsStaleStockSnapshot()
    {
        var stock = PurchaseRules.ResolveStock(listingValid: false, shouldShow: true, limitedStock: false, currentStock: 100);

        Assert.Equal(StockKind.Unknown, stock.Kind);
        Assert.False(stock.CanBuy(1));
    }

    [Fact]
    public void ListingThatVanillaWouldHideIsNotPurchasable()
    {
        var stock = PurchaseRules.ResolveStock(listingValid: true, shouldShow: false, limitedStock: false, currentStock: 0);

        Assert.Equal(StockKind.NotOffered, stock.Kind);
        Assert.False(stock.CanBuy(1));
    }

    [Fact]
    public void LimitedNegativeStockIsUnknownRatherThanUnlimited()
    {
        var stock = PurchaseRules.ResolveStock(listingValid: true, shouldShow: true, limitedStock: true, currentStock: -1);

        Assert.Equal(StockKind.Unknown, stock.Kind);
        Assert.False(stock.CanBuy(1));
    }

    [Fact]
    public void PricingUsesTheExistingFloatFormulaAndFees()
    {
        Assert.True(PurchaseRules.TryCalculatePricing(10f, 10f, 2f, 3, out var pricing));

        Assert.Equal(11f, pricing.UnitPrice);
        Assert.Equal(33f, pricing.Subtotal);
        Assert.Equal(2f, pricing.DeliveryFee);
        Assert.Equal(35f, pricing.Total);
    }

    [Theory]
    [MemberData(nameof(InvalidPricingInputs))]
    public void PricingRejectsInvalidValues(float price, float serviceFeePercent, float deliveryFee, int quantity)
    {
        Assert.False(PurchaseRules.TryCalculatePricing(price, serviceFeePercent, deliveryFee, quantity, out _));
    }

    public static IEnumerable<object[]> InvalidPricingInputs()
    {
        yield return new object[] { -1f, 0f, 0f, 1 };
        yield return new object[] { float.NaN, 0f, 0f, 1 };
        yield return new object[] { float.PositiveInfinity, 0f, 0f, 1 };
        yield return new object[] { 1f, -1f, 0f, 1 };
        yield return new object[] { 1f, float.NaN, 0f, 1 };
        yield return new object[] { 1f, 0f, -1f, 1 };
        yield return new object[] { 1f, 0f, float.PositiveInfinity, 1 };
        yield return new object[] { 1f, 0f, 0f, 0 };
    }

    [Fact]
    public void PricingRejectsFloatOverflow()
    {
        Assert.False(PurchaseRules.TryCalculatePricing(float.MaxValue, 0f, 0f, 2, out _));
    }

    [Fact]
    public void PartialDeliveryRefundsOnlyUndeliveredUnitsAndWaivesDeliveryOnlyIfNothingArrived()
    {
        Assert.True(PurchaseRules.TryCalculateRefund(11f, 5f, 4, 1, out float partialRefund));
        Assert.Equal(33f, partialRefund);

        Assert.True(PurchaseRules.TryCalculateRefund(11f, 5f, 4, 0, out float fullRefund));
        Assert.Equal(49f, fullRefund);
    }

    [Fact]
    public void RefundRejectsUnverifiableDeliveryCounts()
    {
        Assert.False(PurchaseRules.TryCalculateRefund(11f, 5f, 4, -1, out _));
        Assert.False(PurchaseRules.TryCalculateRefund(11f, 5f, 4, 5, out _));
    }

    [Theory]
    [InlineData(PaymentPreference.Cash, 100f, 10f, 50f, true, PaymentAccount.Cash)]
    [InlineData(PaymentPreference.Online, 10f, 100f, 50f, true, PaymentAccount.Bank)]
    [InlineData(PaymentPreference.PreferCash, 100f, 100f, 50f, true, PaymentAccount.Cash)]
    [InlineData(PaymentPreference.PreferCash, 10f, 100f, 50f, true, PaymentAccount.Bank)]
    [InlineData(PaymentPreference.PreferCash, 10f, 20f, 50f, false, PaymentAccount.Cash)]
    [InlineData(PaymentPreference.PreferOnline, 100f, 100f, 50f, true, PaymentAccount.Bank)]
    [InlineData(PaymentPreference.PreferOnline, 100f, 10f, 50f, true, PaymentAccount.Cash)]
    [InlineData(PaymentPreference.PreferOnline, 10f, 20f, 50f, false, PaymentAccount.Bank)]
    public void PaymentPreferencePreservesVanillaAccountSelection(
        PaymentPreference preference, float cash, float bank, float total, bool expectedCanPay, PaymentAccount expectedAccount)
    {
        bool canPay = PurchaseRules.TryResolvePayment(preference, total, cash, bank, out var account);

        Assert.Equal(expectedCanPay, canPay);
        Assert.Equal(expectedAccount, account);
    }

    [Fact]
    public void PaymentPreferenceRejectsNonFiniteBalancesAndTotals()
    {
        Assert.False(PurchaseRules.TryResolvePayment(PaymentPreference.Cash, float.NaN, 10f, 10f, out _));
        Assert.False(PurchaseRules.TryResolvePayment(PaymentPreference.Online, 1f, 10f, float.PositiveInfinity, out _));
    }

    [Fact]
    public void BalanceDeltaMustMatchTheExpectedChargeOrRefund()
    {
        Assert.True(PurchaseRules.BalanceDeltaMatches(100f, 90f, -10f));
        Assert.True(PurchaseRules.BalanceDeltaMatches(90f, 100f, 10f));
        Assert.False(PurchaseRules.BalanceDeltaMatches(100f, 100f, -10f));
        Assert.False(PurchaseRules.BalanceDeltaMatches(100f, 80f, -10f));
    }

    [Fact]
    public void BalanceDeltaRejectsAChargeThatFloatCannotRepresent()
    {
        Assert.False(PurchaseRules.BalanceDeltaMatches(1_000_000_000f, 1_000_000_000f, -10f));
    }

    [Fact]
    public void BalanceDeltaRejectsNonFiniteObservations()
    {
        Assert.False(PurchaseRules.BalanceDeltaMatches(float.NaN, 1f, -1f));
        Assert.False(PurchaseRules.BalanceDeltaMatches(1f, float.PositiveInfinity, -1f));
        Assert.False(PurchaseRules.BalanceDeltaMatches(1f, 0f, float.NegativeInfinity));
    }
}

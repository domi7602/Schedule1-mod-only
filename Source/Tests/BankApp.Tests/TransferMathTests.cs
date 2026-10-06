using BankApp.Logic;
using Xunit;

namespace BankApp.Tests;

public class TransferMathTests
{
    // ---- fee semantics (preserved float formula: amount * (pct/100)) ----

    [Fact]
    public void ComputeFee_UsesPreservedFloatFormula()
    {
        Assert.Equal(100f, TransferMath.ComputeFee(5000f, 2f), 4);
        Assert.Equal(50f, TransferMath.ComputeFee(2500f, 2f), 4);
        Assert.Equal(0f, TransferMath.ComputeFee(2500f, 0f), 4);
    }

    [Fact]
    public void ComputeFee_HandlesNonFiniteAndNonPositive()
    {
        Assert.Equal(0f, TransferMath.ComputeFee(float.NaN, 5f), 4);
        Assert.Equal(0f, TransferMath.ComputeFee(float.PositiveInfinity, 5f), 4);
        Assert.Equal(0f, TransferMath.ComputeFee(-10f, 5f), 4);
    }

    [Theory]
    [InlineData(-1f, 0f)]
    [InlineData(0f, 0f)]
    [InlineData(2f, 2f)]
    [InlineData(10f, 10f)]
    [InlineData(99f, 10f)]
    [InlineData(float.NaN, 0f)]
    [InlineData(float.PositiveInfinity, 0f)]
    public void SanitizeFeePercent_ClampsToSupportedRange(float input, float expected)
    {
        Assert.Equal(expected, TransferMath.SanitizeFeePercent(input), 4);
    }

    // ---- deposit quote ----

    [Fact]
    public void DepositQuote_SplitsFeeAndCreditsNet()
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Deposit, 5000f, cashOnHand: 10000f, onlineBalance: 12450f,
            feePercent: 2f, limitEnabled: true, remainingLimit: 10000f);

        Assert.True(quote.IsValid);
        Assert.Equal(5000f, quote.Gross, 4);      // cash taken from the player
        Assert.Equal(100f, quote.Fee, 4);
        Assert.Equal(4900f, quote.Net, 4);        // credited to the bank
        Assert.Equal(5000f, quote.Debit, 4);
        Assert.Equal(17350f, quote.BalanceAfter, 4);
    }

    [Fact]
    public void DepositQuote_ZeroFeeIsPassthrough()
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Deposit, 1000f, cashOnHand: 5000f, onlineBalance: 100f,
            feePercent: 0f, limitEnabled: false, remainingLimit: 0f);

        Assert.True(quote.IsValid);
        Assert.Equal(0f, quote.Fee, 4);
        Assert.Equal(1000f, quote.Net, 4);
        Assert.Equal(1100f, quote.BalanceAfter, 4);
    }

    // ---- withdrawal quote ----

    [Fact]
    public void WithdrawQuote_AddsFeeOnTopOfBankDebit()
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Withdraw, 2500f, cashOnHand: 0f, onlineBalance: 12450f,
            feePercent: 2f, limitEnabled: false, remainingLimit: 0f);

        Assert.True(quote.IsValid);
        Assert.Equal(2500f, quote.Gross, 4);
        Assert.Equal(50f, quote.Fee, 4);
        Assert.Equal(2500f, quote.Net, 4);        // cash the player receives
        Assert.Equal(2550f, quote.Debit, 4);      // total pulled from the bank
        Assert.Equal(9900f, quote.BalanceAfter, 4);
    }

    [Fact]
    public void WithdrawQuote_ZeroFeeDebitsExactlyTheGross()
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Withdraw, 1000f, cashOnHand: 0f, onlineBalance: 5000f,
            feePercent: 0f, limitEnabled: false, remainingLimit: 0f);

        Assert.True(quote.IsValid);
        Assert.Equal(0f, quote.Fee, 4);
        Assert.Equal(1000f, quote.Debit, 4);
        Assert.Equal(4000f, quote.BalanceAfter, 4);
    }

    // ---- invalid inputs ----

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void NonFiniteAmount_IsRejected(float amount)
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Deposit, amount, 1000f, 1000f, 2f, false, 0f);

        Assert.False(quote.IsValid);
        Assert.Equal(TransferMath.InvalidAmountMessage, quote.ErrorMessage);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-5f)]
    public void NonPositiveAmount_IsRejected(float amount)
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Withdraw, amount, 0f, 1000f, 2f, false, 0f);

        Assert.False(quote.IsValid);
        Assert.Equal(TransferMath.SelectAmountMessage, quote.ErrorMessage);
    }

    [Fact]
    public void Deposit_OverCashOnHand_IsRejected()
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Deposit, 1000f, cashOnHand: 500f, onlineBalance: 0f,
            feePercent: 0f, limitEnabled: false, remainingLimit: 0f);

        Assert.False(quote.IsValid);
        Assert.Contains("cash", quote.ErrorMessage, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Deposit_OverWeeklyLimit_IsRejected_AndAllowedWhenLimitDisabled()
    {
        TransferQuote blocked = TransferMath.ComputeQuote(
            TransferDirection.Deposit, 5000f, cashOnHand: 10000f, onlineBalance: 0f,
            feePercent: 0f, limitEnabled: true, remainingLimit: 3000f);
        Assert.False(blocked.IsValid);
        Assert.Contains("Weekly ATM limit", blocked.ErrorMessage, System.StringComparison.OrdinalIgnoreCase);

        TransferQuote allowed = TransferMath.ComputeQuote(
            TransferDirection.Deposit, 5000f, cashOnHand: 10000f, onlineBalance: 0f,
            feePercent: 0f, limitEnabled: false, remainingLimit: 0f);
        Assert.True(allowed.IsValid);
    }

    [Fact]
    public void Withdraw_OverBankFunds_IncludingFee_IsRejected()
    {
        // 1000 gross + 2% = 1020 required, only 1000 available.
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Withdraw, 1000f, cashOnHand: 0f, onlineBalance: 1000f,
            feePercent: 2f, limitEnabled: false, remainingLimit: 0f);

        Assert.False(quote.IsValid);
        Assert.Contains("Insufficient bank funds", quote.ErrorMessage, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void NonFiniteBalance_IsRejected()
    {
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Deposit, 100f, cashOnHand: float.NaN, onlineBalance: 0f,
            feePercent: 0f, limitEnabled: false, remainingLimit: 0f);

        Assert.False(quote.IsValid);
        Assert.Equal(TransferMath.BalanceUnavailableMessage, quote.ErrorMessage);
    }

    // ---- MAX buttons ----

    [Fact]
    public void MaxDeposit_IsCappedByCashAndLimit()
    {
        Assert.Equal(500f, TransferMath.MaxDeposit(500f, 3000f, true), 4);
        Assert.Equal(3000f, TransferMath.MaxDeposit(5000f, 3000f, true), 4);
        Assert.Equal(5000f, TransferMath.MaxDeposit(5000f, 0f, false), 4);
        Assert.Equal(0f, TransferMath.MaxDeposit(float.NaN, 3000f, true), 4);
    }

    [Fact]
    public void MaxWithdraw_WithoutFee_IsTheWholeBalance()
    {
        Assert.Equal(12450f, TransferMath.MaxWithdraw(12450f, 0f), 4);
    }

    [Fact]
    public void MaxWithdraw_WithFee_DividesByOnePlusRate()
    {
        float max = TransferMath.MaxWithdraw(1000f, 2f);
        Assert.Equal(980.3922f, max, 2);
    }

    [Theory]
    [InlineData(100f, 0.5f)]
    [InlineData(1000f, 1f)]
    [InlineData(12450f, 2f)]
    [InlineData(999.99f, 10f)]
    [InlineData(0.1f, 5f)]
    public void MaxWithdraw_NeverExceedsBalanceOnceFeeAdded(float balance, float feePercent)
    {
        float max = TransferMath.MaxWithdraw(balance, feePercent);

        Assert.True(max >= 0f);
        Assert.True(max <= balance);
        float required = max + TransferMath.ComputeFee(max, feePercent);
        Assert.True(required <= balance, $"max {max} + fee requires {required} > balance {balance}");
    }

    [Fact]
    public void MaxWithdraw_NonFiniteOrEmpty_IsZero()
    {
        Assert.Equal(0f, TransferMath.MaxWithdraw(float.NaN, 2f), 4);
        Assert.Equal(0f, TransferMath.MaxWithdraw(0f, 2f), 4);
        Assert.Equal(0f, TransferMath.MaxWithdraw(-5f, 2f), 4);
    }

    // ---- relative presets ----

    [Theory]
    [InlineData(0.25f, 4000f, 1000f)]
    [InlineData(0.5f, 4000f, 2000f)]
    [InlineData(1f, 4000f, 4000f)]
    [InlineData(2f, 4000f, 4000f)]   // clamped
    [InlineData(0.25f, 0f, 0f)]
    [InlineData(0f, 4000f, 0f)]
    [InlineData(float.NaN, 4000f, 0f)]
    public void PercentageOfMax_ClampsAndGuards(float fraction, float max, float expected)
    {
        Assert.Equal(expected, TransferMath.PercentageOfMax(fraction, max), 4);
    }

    // ---- fixed set-amount presets ----

    [Fact]
    public void AmountPresets_AreTheFixedSetAmounts()
    {
        Assert.Equal(new[] { 100f, 500f, 1000f, 2500f, 5000f, 10000f }, TransferMath.AmountPresets);
    }

    [Fact]
    public void RelativePresets_Are25And50()
    {
        Assert.Equal(new[] { 0.25f, 0.5f }, TransferMath.RelativePresets);
    }

    // ---- guarded direct input ----

    [Theory]
    [InlineData("1234.56", true, 1234.56f)]
    [InlineData("$500", true, 500f)]
    [InlineData("1,000", true, 1000f)]
    [InlineData("  2500 ", true, 2500f)]
    [InlineData("", false, 0f)]
    [InlineData("abc", false, 0f)]
    [InlineData("-5", false, 0f)]
    [InlineData("0", false, 0f)]
    [InlineData("NaN", false, 0f)]
    [InlineData("Infinity", false, 0f)]
    public void TryParseAmount_GuardsInput(string text, bool expectedOk, float expectedValue)
    {
        bool ok = TransferMath.TryParseAmount(text, out float value);

        Assert.Equal(expectedOk, ok);
        if (expectedOk) Assert.Equal(expectedValue, value, 4);
    }

    // ---- input/display formatting (round-trip invariant) ----

    [Fact]
    public void FormatAmountInput_IsRoundTripInvariant_WithoutGrouping()
    {
        float[] values =
        {
            100f, 500f, 1000f, 2500f, 5000f, 10000f,       // fixed presets
            1234.56f, 0.1f, 999.99f,
            TransferMath.MaxWithdraw(1000f, 2f),            // 980.3922: fractional MAX
            TransferMath.PercentageOfMax(0.25f, TransferMath.MaxWithdraw(1000f, 2f)),
            TransferMath.PercentageOfMax(0.5f, 12450f),
        };

        foreach (float value in values)
        {
            string text = TransferMath.FormatAmountInput(value);
            Assert.DoesNotContain(",", text, System.StringComparison.Ordinal);

            Assert.True(TransferMath.TryParseAmount(text, out float parsed),
                $"'{text}' (from {value}) must parse back");
            Assert.Equal(value, parsed); // exact float equality: the invariant the UI relies on
        }
    }

    [Fact]
    public void FormatAmountInput_EmptyForZeroAndNonFinite()
    {
        Assert.Equal(string.Empty, TransferMath.FormatAmountInput(0f));
        Assert.Equal(string.Empty, TransferMath.FormatAmountInput(-5f));
        Assert.Equal(string.Empty, TransferMath.FormatAmountInput(float.NaN));
        Assert.Equal(string.Empty, TransferMath.FormatAmountInput(float.PositiveInfinity));
    }

    [Fact]
    public void FormatMoney_ShowsGroupedWholeUnitsWhenNoCents()
    {
        Assert.Equal("$ 5,000", TransferMath.FormatMoney(5000f));
        Assert.Equal("$ 0", TransferMath.FormatMoney(0f));
        Assert.Equal("$ -2,550", TransferMath.FormatMoney(-2550f));
    }

    [Fact]
    public void FormatMoney_ShowsCentsOnlyWhenFractional()
    {
        Assert.Equal("$ 1,000.50", TransferMath.FormatMoney(1000.5f));
        Assert.Equal("$ 980.39", TransferMath.FormatMoney(TransferMath.MaxWithdraw(1000f, 2f)));
        Assert.Equal("$ -2,550.25", TransferMath.FormatMoney(-2550.25f));
    }

    // ---- weekly reset countdown ----

    [Theory]
    [InlineData(1, 6)]
    [InlineData(6, 1)]
    [InlineData(7, 7)]
    [InlineData(8, 6)]
    [InlineData(13, 1)]
    [InlineData(14, 7)]
    [InlineData(21, 7)]
    public void DaysUntilWeeklyReset_Is7MinusDayMod7(int day, int expected)
    {
        Assert.Equal(expected, TransferMath.DaysUntilWeeklyReset(day));
    }
}

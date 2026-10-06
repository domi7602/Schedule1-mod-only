using System;
using System.Globalization;

namespace BankApp.Logic;

/// <summary>Direction of a mobile cash move. Kept Unity-free so the seam is unit-testable.</summary>
public enum TransferDirection
{
    Deposit,
    Withdraw
}

/// <summary>
/// Immutable result of <see cref="TransferMath.ComputeQuote"/>.
/// <para>
/// A deposit moves cash into the bank: <see cref="Net"/> is the amount credited to the bank
/// (gross minus fee) and <see cref="Debit"/> is the cash taken from the player (the gross).
/// A withdrawal moves bank money to cash: <see cref="Net"/> is the cash the player receives
/// (the gross) and <see cref="Debit"/> is the total pulled from the bank (gross plus fee).
/// </para>
/// </summary>
public readonly struct TransferQuote
{
    public bool IsValid { get; }
    public string ErrorMessage { get; }
    public float Gross { get; }
    public float Fee { get; }
    public float Net { get; }
    public float Debit { get; }
    public float BalanceAfter { get; }

    public TransferQuote(bool isValid, string errorMessage, float gross, float fee, float net, float debit, float balanceAfter)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage ?? string.Empty;
        Gross = gross;
        Fee = fee;
        Net = net;
        Debit = debit;
        BalanceAfter = balanceAfter;
    }
}

/// <summary>
/// The single Unity-free source of truth for every money figure BankApp shows or applies:
/// the live preview, the MAX button and <c>BankService</c>'s execution revalidation all call
/// here, so a quote the UI accepts cannot be recomputed differently at execution time.
/// <para>
/// Fee semantics are preserved verbatim from the previous in-service implementation:
/// <c>fee = amount * (clampedPercent / 100f)</c> with the percentage clamped to [0, 10].
/// No rounding is invented — figures stay float, exactly as the money engine already moves them.
/// </para>
/// </summary>
public static class TransferMath
{
    public const float MaxServiceFeePercent = 10f;

    /// <summary>Fixed "set amount" presets, in ascending order (golden: $100 … $10,000).</summary>
    public static readonly float[] AmountPresets = { 100f, 500f, 1000f, 2500f, 5000f, 10000f };

    /// <summary>Relative presets expressed as a fraction of the currently allowed maximum.</summary>
    public static readonly float[] RelativePresets = { 0.25f, 0.5f };

    public const string InvalidAmountMessage = "Enter a valid amount.";
    public const string SelectAmountMessage = "Select an amount first.";
    public const string BalanceUnavailableMessage = "Balance unavailable.";

    public static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

    /// <summary>Clamps the configured service fee to the supported [0, 10] range; non-finite input falls back to 0.</summary>
    public static float SanitizeFeePercent(float percent)
    {
        if (!IsFinite(percent)) return 0f;
        if (percent < 0f) return 0f;
        if (percent > MaxServiceFeePercent) return MaxServiceFeePercent;
        return percent;
    }

    /// <summary>Fee on a gross amount using the preserved float formula. Zero for non-positive/non-finite amounts.</summary>
    public static float ComputeFee(float amount, float feePercent)
    {
        if (!IsFinite(amount) || amount <= 0f) return 0f;
        return amount * (SanitizeFeePercent(feePercent) / 100f);
    }

    /// <summary>Largest deposit the player can make: cash on hand, capped by the weekly limit when enabled.</summary>
    public static float MaxDeposit(float cashOnHand, float remainingLimit, bool limitEnabled)
    {
        if (!IsFinite(cashOnHand) || cashOnHand <= 0f) return 0f;
        if (!limitEnabled) return cashOnHand;
        float cap = IsFinite(remainingLimit) ? Math.Max(0f, remainingLimit) : 0f;
        return Math.Max(0f, Math.Min(cashOnHand, cap));
    }

    /// <summary>
    /// Largest withdrawal that still fits the bank balance once the fee is added on top.
    /// Solves <c>A + fee(A) &lt;= balance</c>: without a fee the whole balance can be withdrawn.
    /// Float rounding can leave the naive <c>balance / (1 + p)</c> a few ULPs above the limit,
    /// so the candidate is walked down until the required total provably fits (never invents rounding).
    /// </summary>
    public static float MaxWithdraw(float onlineBalance, float feePercent)
    {
        if (!IsFinite(onlineBalance) || onlineBalance <= 0f) return 0f;
        float p = SanitizeFeePercent(feePercent) / 100f;
        if (p <= 0f) return onlineBalance;

        float candidate = onlineBalance / (1f + p);
        int guard = 0;
        while (guard < 16 && candidate > 0f && candidate + ComputeFee(candidate, feePercent) > onlineBalance)
        {
            candidate = PreviousFloat(candidate);
            guard++;
        }
        return candidate < 0f ? 0f : candidate;
    }

    /// <summary>Fraction of the allowed maximum, clamped to [0, max]; any invalid input yields 0.</summary>
    public static float PercentageOfMax(float fraction, float allowedMax)
    {
        if (!IsFinite(fraction) || fraction <= 0f) return 0f;
        if (!IsFinite(allowedMax) || allowedMax <= 0f) return 0f;
        if (fraction > 1f) fraction = 1f;
        return allowedMax * fraction;
    }

    /// <summary>
    /// Validates a transfer against a set of live balances and produces every preview figure.
    /// The same call is used by the UI preview and by BankService's execution revalidation.
    /// </summary>
    public static TransferQuote ComputeQuote(
        TransferDirection direction,
        float amount,
        float cashOnHand,
        float onlineBalance,
        float feePercent,
        bool limitEnabled,
        float remainingLimit)
    {
        if (!IsFinite(amount)) return Invalid(InvalidAmountMessage, amount);
        if (amount <= 0f) return Invalid(SelectAmountMessage, amount);

        float fee = ComputeFee(amount, feePercent);

        if (direction == TransferDirection.Deposit)
        {
            if (!IsFinite(cashOnHand) || !IsFinite(onlineBalance)) return Invalid(BalanceUnavailableMessage, amount);
            if (cashOnHand < amount)
                return Invalid($"Insufficient cash on hand ($ {cashOnHand:N0} available).", amount);

            if (limitEnabled)
            {
                float remaining = IsFinite(remainingLimit) ? Math.Max(0f, remainingLimit) : 0f;
                if (amount > remaining)
                    return Invalid($"Weekly ATM limit exceeded (Max remaining: ${remaining:N0}).", amount);
            }

            float net = amount - fee;
            float balanceAfter = onlineBalance + net;
            if (!IsFinite(balanceAfter)) return Invalid(BalanceUnavailableMessage, amount);
            return new TransferQuote(true, string.Empty, amount, fee, net, amount, balanceAfter);
        }

        if (!IsFinite(onlineBalance)) return Invalid(BalanceUnavailableMessage, amount);
        float debit = amount + fee;
        if (onlineBalance < debit)
            return Invalid($"Insufficient bank funds (${onlineBalance:N0} available, ${debit:N0} required).", amount);

        return new TransferQuote(true, string.Empty, amount, fee, amount, debit, onlineBalance - debit);
    }

    /// <summary>
    /// Guarded parse for the optional direct-input field: strips currency decoration, accepts
    /// invariant or current culture, and rejects non-finite/non-positive results.
    /// </summary>
    public static bool TryParseAmount(string? text, out float amount)
    {
        amount = 0f;
        if (string.IsNullOrWhiteSpace(text)) return false;

        string cleaned = text.Trim().Replace("$", string.Empty).Replace(",", string.Empty).Replace(" ", string.Empty);
        if (cleaned.Length == 0) return false;

        if (!float.TryParse(cleaned, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed))
        {
            if (!float.TryParse(cleaned, NumberStyles.Float, CultureInfo.CurrentCulture, out parsed)) return false;
        }

        if (!IsFinite(parsed) || parsed <= 0f) return false;
        amount = parsed;
        return true;
    }

    /// <summary>
    /// Round-trip-safe text for the editable amount field: invariant culture, no group
    /// separators, so whatever the player sees parses back to exactly the stored float
    /// (verified by <see cref="TryParseAmount"/> in tests). Empty for non-positive input.
    /// </summary>
    public static string FormatAmountInput(float value)
    {
        if (!IsFinite(value) || value <= 0f) return string.Empty;
        return value.ToString("R", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Display-only money text: grouped whole units, with cents shown only when the value
    /// actually carries a fractional part. Never used for the editable input (see
    /// <see cref="FormatAmountInput"/>), which must stay parse-exact.
    /// </summary>
    public static string FormatMoney(float value)
    {
        if (!IsFinite(value)) return "$ 0";

        float rounded = MathF.Round(value, 2);
        bool hasCents = MathF.Abs(rounded - MathF.Round(rounded)) > 0.0001f;
        return hasCents
            ? "$ " + rounded.ToString("N2", CultureInfo.InvariantCulture)
            : "$ " + rounded.ToString("N0", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Days until the weekly ATM limit resets: <c>7 - (day % 7)</c> on the in-game day counter
    /// (day 7 / 14 / 21 report a full week). Negative day wrap is handled defensively.
    /// </summary>
    public static int DaysUntilWeeklyReset(int currentDay)
    {
        int mod = ((currentDay % 7) + 7) % 7;
        return 7 - mod;
    }

    private static TransferQuote Invalid(string message, float gross) =>
        new(false, message, gross, 0f, 0f, 0f, 0f);

    /// <summary>One ULP below a positive float (positive floats are monotonic in their bit pattern).</summary>
    private static float PreviousFloat(float value)
    {
        if (!(value > 0f)) return 0f;
        int bits = BitConverter.SingleToInt32Bits(value);
        bits -= 1;
        return BitConverter.Int32BitsToSingle(bits);
    }
}

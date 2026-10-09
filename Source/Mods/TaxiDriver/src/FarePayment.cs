using System;

namespace TaxiDriver;

/// <summary>
/// Production settlement seam used by <see cref="FareMeter"/>. The money calls
/// remain behind a small Unity-free wallet interface so cash/bank splitting,
/// payment status, exception handling and no-retry behavior are unit-testable.
///
/// Preserved semantics (characterized in FarePaymentCharacterizationTests):
/// <list type="bullet">
/// <item>Billing before payment, no retry: the ledger consumes the minutes
/// first (FareLedger.cs:109-119); one Execute call makes at most one attempt
/// per payment part.</item>
/// <item>A clean wallet return is NOT a payment confirmation: S1API.Money
/// silently no-ops when MoneyManager.Instance is null (Money.cs:43,57). The
/// outcome reports "call returned", never "money moved".</item>
/// <item>A thrown wallet call is NOT "uncharged": its outcome is UNKNOWN
/// (S1API fires OnBalanceChanged after the manager call, Money.cs:44,58; the
/// native MoneyManager methods are IL2CPP stubs, so mutate-then-throw is not
/// disproven). A thrown part is never auto-classified and never retried.</item>
/// <item>Since the TD-01/TD-02/TD-04 fixes (2026-10-03): a failed cash read
/// aborts WITHOUT any money call (fail closed, the amount is unpaid), only
/// clean-returned parts are counted as payment calls returned (not balance-
/// verified), a thrown part is UNKNOWN and never retried, and a client makes
/// no payment call (calculated fare only). The outer catch boundary still covers the wallet calls AND the
/// caller's record/log hooks, exactly like the original try.</item>
/// </list>
/// </summary>
internal static class FarePayment
{
    /// <summary>
    /// Cash/bank split of FareMeter.cs:258-259: clamp cash at 0, cap at the due
    /// amount, truncate to whole dollars. Production (IL2CPPMELON, the mod build)
    /// runs the EXACT original expression, so the special-value/Unity-Math quirks
    /// cannot change; the test build (no IL2CPPMELON) runs a comparison-operator
    /// mirror whose semantics are pinned by the P09 characterization. Mirror
    /// equivalence to Unity is therefore not load-bearing for production
    /// (seam-mapping.md section 4).
    /// </summary>
    internal static int SplitCash(float cash, int dollars)
    {
#if IL2CPPMELON
        return (int)UnityEngine.Mathf.Min(UnityEngine.Mathf.Max(cash, 0f), dollars);
#else
        float clamped = cash > 0f ? cash : 0f; // mirrors Mathf.Max(cash, 0f)
        float capped = clamped < dollars ? clamped : dollars; // mirrors Mathf.Min(x, dollars)
        return (int)capped;
#endif
    }

    /// <summary>
    /// One settlement attempt for <paramref name="dollars"/> whole dollars: host
    /// gate, cash read with its inner guard, split, cash call, bank call, counter,
    /// summary log - one outer catch around all of it (same boundary as the
    /// original FareMeter.Charge). The outcome reports returned/unknown/unpaid
    /// parts instead of a blanket skip: the hooks
    /// run at the exact positions of the original statements, and a thrown part is
    /// never classified as "not charged" and never retried.
    /// </summary>
    internal static PaymentOutcome Execute(
        int dollars,
        IGameWallet wallet,
        bool isHost,
        Action<int>? recordCharge = null,
        Action? clientWarnOnce = null,
        Action? chargeSummaryLog = null)
    {
        var outcome = new PaymentOutcome
        {
            DueDollars = dollars,
            CalculatedDollars = dollars,
        };

        try
        {
            if (!isHost)
            {
                // FareMeter.cs:238-242 (the warn-once state stays in the adapter).
                clientWarnOnce?.Invoke();
                // TD-04: a client makes no payment call; only the calculated fare
                // may be shown without host authority.
                outcome.NotHost = true;
                return outcome;
            }

            float cash;
            try
            {
                cash = wallet.GetCashBalance(); // FareMeter.cs:251
            }
            catch (Exception ex)
            {
                // TD-02: fail closed - an unreadable balance selects NO payment
                // method; nothing is touched and the amount stays unpaid.
                outcome.CashReadFailed = true;
                outcome.CashReadError = $"{ex.GetType().Name}: {ex.Message}";
                outcome.UnpaidDollars = dollars;
                return outcome;
            }

            int fromCash = SplitCash(cash, dollars); // FareMeter.cs:258
            int fromBank = dollars - fromCash; // FareMeter.cs:259
            outcome.CashPart = fromCash;
            outcome.BankPart = fromBank;

            if (fromCash > 0)
            {
                outcome.CashStatus = PaymentPartStatus.Attempted;
                wallet.ChangeCashBalance(-fromCash, true, false); // FareMeter.cs:262
                outcome.CashStatus = PaymentPartStatus.CallReturned;
            }

            if (fromBank > 0)
            {
                outcome.BankStatus = PaymentPartStatus.Attempted;
                wallet.CreateOnlineTransaction("Taxi fare", -fromBank, 1, "Taxi"); // FareMeter.cs:265
                outcome.BankStatus = PaymentPartStatus.CallReturned;
            }

            recordCharge?.Invoke(dollars); // FareMeter.cs:267 (full amount: every part returned)
            outcome.CountedDollars = dollars;
            outcome.ChargeRecorded = true;
            chargeSummaryLog?.Invoke(); // FareMeter.cs:270-276
        }
        catch (Exception ex)
        {
            outcome.Caught = true;
            outcome.CaughtType = ex.GetType().Name;
            outcome.CaughtMessage = ex.Message;

            // A part whose call was in flight when the exception escaped did not
            // report back: outcome UNKNOWN (mutate-then-throw is not disproven),
            // never "not charged".
            if (outcome.CashStatus == PaymentPartStatus.Attempted)
                outcome.CashStatus = PaymentPartStatus.CallThrewUnknown;
            if (outcome.BankStatus == PaymentPartStatus.Attempted)
                outcome.BankStatus = PaymentPartStatus.CallThrewUnknown;

            if (!outcome.ChargeRecorded)
            {
                // TD-01 partial accounting: record only clean-returned parts as
                // API returns (never balance-confirmed); a thrown part is UNKNOWN,
                // a never-attempted part unpaid.
                int returned =
                    (outcome.CashStatus == PaymentPartStatus.CallReturned ? outcome.CashPart : 0) +
                    (outcome.BankStatus == PaymentPartStatus.CallReturned ? outcome.BankPart : 0);
                outcome.CountedDollars = returned;
                if (returned > 0)
                    recordCharge?.Invoke(returned);
                outcome.UnknownDollars =
                    (outcome.CashStatus == PaymentPartStatus.CallThrewUnknown ? outcome.CashPart : 0) +
                    (outcome.BankStatus == PaymentPartStatus.CallThrewUnknown ? outcome.BankPart : 0);
                outcome.UnpaidDollars = dollars - returned - outcome.UnknownDollars;
            }
        }

        return outcome;
    }
}

/// <summary>Per-part state of one settlement attempt. There is deliberately no
/// "Confirmed" and no "Unpaid" state for a thrown call: a clean return is only
/// "call returned", a throw is "outcome unknown".</summary>
internal enum PaymentPartStatus
{
    /// <summary>No call was made (part not due, or the flow stopped earlier).</summary>
    NotAttempted,

    /// <summary>The call is in flight (transient, only observed mid-Execute).</summary>
    Attempted,

    /// <summary>The call returned cleanly. NOT proof that money moved.</summary>
    CallReturned,

    /// <summary>The call threw. NOT proof that money did not move; never retried.</summary>
    CallThrewUnknown,
}

/// <summary>
/// The three money effects FareMeter uses (S1API.Money.Money signatures
/// mirrored 1:1, Money.cs:41/54/74). The interface makes NO stronger success
/// promise than S1API: a clean return means "call returned", not "money
/// moved", and a thrown call leaves the outcome unknown.
/// </summary>
internal interface IGameWallet
{
    /// <summary>Mirrors S1API Money.GetCashBalance (Money.cs:74-77).</summary>
    float GetCashBalance();

    /// <summary>Mirrors S1API Money.ChangeCashBalance (Money.cs:41-45).</summary>
    void ChangeCashBalance(float delta, bool visualizeChange, bool playCashSound);

    /// <summary>Mirrors S1API Money.CreateOnlineTransaction (Money.cs:54-59).</summary>
    void CreateOnlineTransaction(string transactionName, float unitAmount, float quantity, string transactionNote);
}

/// <summary>
/// Observable result of one <see cref="FarePayment.Execute"/> attempt. Records
/// what happened (calls returned or threw, parts attempted, counter hook value)
/// and never classifies a thrown call as "uncharged" or a clean call as
/// "confirmed".
/// </summary>
internal sealed class PaymentOutcome
{
    /// <summary>Dollars requested for this attempt.</summary>
    public int DueDollars;

    /// <summary>Calculated fare shown to the player; separate from payment-call outcomes.</summary>
    public int CalculatedDollars;

    /// <summary>Split part attempted via ChangeCashBalance.</summary>
    public int CashPart;

    /// <summary>Split part attempted via CreateOnlineTransaction.</summary>
    public int BankPart;

    /// <summary>Payment-call-returned dollars passed to the record callback; not proof of balance movement.</summary>
    public int CountedDollars;

    /// <summary>The client path was taken (no wallet call).</summary>
    public bool NotHost;

    /// <summary>GetCashBalance threw; settlement stopped before any money call and the due amount remains unpaid.</summary>
    public bool CashReadFailed;

    /// <summary>The outer catch engaged; a wallet call or hook threw.</summary>
    public bool Caught;

    /// <summary>Exception type name for the caller's log (FareMeter.cs:280 shape).</summary>
    public string? CaughtType;

    /// <summary>Exception message for the caller's log.</summary>
    public string? CaughtMessage;

    /// <summary>Exception of a failed cash read ("Type: Message") for the caller's log.</summary>
    public string? CashReadError;

    /// <summary>Parts whose money call threw: outcome UNKNOWN, never "not charged", never retried.</summary>
    public int UnknownDollars;

    /// <summary>Parts never attempted (fail-closed abort): not charged.</summary>
    public int UnpaidDollars;

    /// <summary>True once the payment-call-returned amount was handed to recordCharge (guards double counting).</summary>
    public bool ChargeRecorded;

    /// <summary>State of the cash part (see <see cref="PaymentPartStatus"/>).</summary>
    public PaymentPartStatus CashStatus;

    /// <summary>State of the bank part (see <see cref="PaymentPartStatus"/>).</summary>
    public PaymentPartStatus BankStatus;
}

/// <summary>
/// Per-ride summary of settlement outcomes. A clean void API return is tracked
/// separately from a thrown/unknown call and is never described as verified money
/// movement. Totals are bounded by the fare ledger's lifetime cap.
/// </summary>
internal sealed class FarePaymentStatusLedger
{
    internal int ReturnedDollars { get; private set; }
    internal int UnknownDollars { get; private set; }
    internal int UnpaidDollars { get; private set; }
    internal bool ClientOnly { get; private set; }

    internal void Reset()
    {
        ReturnedDollars = 0;
        UnknownDollars = 0;
        UnpaidDollars = 0;
        ClientOnly = false;
    }

    /// <summary>Records the amount whose payment calls returned cleanly, without claiming a balance change.</summary>
    internal void RecordReturned(int dollars) =>
        ReturnedDollars = SaturatingAdd(ReturnedDollars, dollars);

    /// <summary>Records one completed payment outcome, including returned parts for unit-test callers.</summary>
    internal void Record(PaymentOutcome outcome)
    {
        RecordReturned(outcome.CountedDollars);
        RecordNonReturned(outcome);
    }

    /// <summary>Records only outcome metadata when returned parts were already recorded by the production callback.</summary>
    internal void RecordNonReturned(PaymentOutcome outcome)
    {
        UnknownDollars = SaturatingAdd(UnknownDollars, outcome.UnknownDollars);
        UnpaidDollars = SaturatingAdd(UnpaidDollars, outcome.UnpaidDollars);
        ClientOnly |= outcome.NotHost;
    }

    internal void RecordUnknown(int dollars) =>
        UnknownDollars = SaturatingAdd(UnknownDollars, dollars);

    internal void RecordUnpaid(int dollars) =>
        UnpaidDollars = SaturatingAdd(UnpaidDollars, dollars);

    internal string SummaryText()
    {
        if (ClientOnly && ReturnedDollars == 0 && UnknownDollars == 0 && UnpaidDollars == 0)
            return "no payment attempted on this client; host authority required";

        string settlement =
            $"payment calls returned ${ReturnedDollars} (balance movement unverified), " +
            $"unknown ${UnknownDollars}, unpaid ${UnpaidDollars}";
        return ClientOnly
            ? $"payment skipped on this client (host authority); {settlement}"
            : settlement;
    }

    private static int SaturatingAdd(int current, int amount)
    {
        if (amount <= 0)
            return current;
        return (int)Math.Min((long)current + amount, FareConfigRules.MaximumFareDollars);
    }
}

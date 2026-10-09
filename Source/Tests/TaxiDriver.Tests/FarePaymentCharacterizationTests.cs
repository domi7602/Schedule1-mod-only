using System;
using System.Collections.Generic;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Characterization tests: pin the FareMeter.Charge money semantics as
/// implemented by the FarePayment seam. P04-P08 were updated on 2026-10-03 when
/// the TD-01/TD-02/TD-04 fixes landed (per the review rule: the expectations of
/// quirk-documenting tests change at fix time); P01-P03 and P09-P11 are
/// unchanged from the pre-fix characterization. A fake wallet proves how a
/// modeled scenario is HANDLED, not that the scenario occurs in the game.
/// </summary>
public class FarePaymentCharacterizationTests
{
    [Fact]
    public void P01_FullCash_PaysCashOnly_CountsOnce()
    {
        var wallet = new FakeWallet { CashBalance = 100f };
        var counted = new List<int>();

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true, recordCharge: counted.Add);

        Assert.Equal(5, outcome.CashPart);
        Assert.Equal(0, outcome.BankPart);
        Assert.Equal(1, wallet.ChangeCalls);
        Assert.Equal(-5f, wallet.LastChangeDelta);
        Assert.Equal(0, wallet.TransactionCalls);
        Assert.Equal(5, outcome.CountedDollars);
        Assert.Equal(new[] { 5 }, counted);
        Assert.False(outcome.Caught);
        Assert.Equal(PaymentPartStatus.CallReturned, outcome.CashStatus);
        Assert.Equal(PaymentPartStatus.NotAttempted, outcome.BankStatus);
    }

    [Fact]
    public void P02_Partial_KeepsCallOrderAndPayload()
    {
        var wallet = new FakeWallet { CashBalance = 2f };

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true);

        // Order and payload are load-bearing: cash first, bank second (FareMeter.cs:261-265).
        Assert.Equal(new[] { "get", "change", "transaction" }, wallet.CallOrder);
        Assert.Equal(-2f, wallet.LastChangeDelta);
        Assert.Equal("Taxi fare", wallet.LastTransactionName);
        Assert.Equal(-3f, wallet.LastUnitAmount);
        Assert.Equal(1f, wallet.LastQuantity);
        Assert.Equal("Taxi", wallet.LastNote);
        Assert.Equal(2, outcome.CashPart);
        Assert.Equal(3, outcome.BankPart);
        Assert.Equal(5, outcome.CountedDollars);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-10f)]
    public void P03_EmptyOrNegativeCash_GoesAllToBank(float cash)
    {
        var wallet = new FakeWallet { CashBalance = cash };

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true);

        Assert.Equal(0, outcome.CashPart);
        Assert.Equal(5, outcome.BankPart);
        Assert.Equal(0, wallet.ChangeCalls);
        Assert.Equal(1, wallet.TransactionCalls);
        Assert.Equal(PaymentPartStatus.NotAttempted, outcome.CashStatus);
        Assert.Equal(PaymentPartStatus.CallReturned, outcome.BankStatus);
    }

    [Fact]
    public void P04_CashReadThrows_FailsClosed_NothingCharged()
    {
        // TD-02 (fixed 2026-10-03): an unreadable balance selects NO payment
        // method speculatively - nothing is touched, the amount stays unpaid and
        // the read error is reported.
        var wallet = new FakeWallet { ThrowOnGet = true };

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true);

        Assert.True(outcome.CashReadFailed);
        Assert.Equal("InvalidOperationException: fake: cash read failed", outcome.CashReadError);
        Assert.Equal(1, wallet.GetCalls);
        Assert.Equal(0, wallet.ChangeCalls);
        Assert.Equal(0, wallet.TransactionCalls);
        Assert.Equal(0, outcome.CountedDollars);
        Assert.Equal(5, outcome.UnpaidDollars);
        Assert.Equal(0, outcome.UnknownDollars);
        Assert.False(outcome.Caught);
        Assert.Equal(PaymentPartStatus.NotAttempted, outcome.CashStatus);
        Assert.Equal(PaymentPartStatus.NotAttempted, outcome.BankStatus);
    }

    [Fact]
    public void P05_BankThrowsAfterMutating_CashPartKept_BankPartUnknown()
    {
        // TD-01 (fixed 2026-10-03): confirmed partial payments are counted; the
        // thrown bank part is UNKNOWN (never "not charged") and exactly one
        // attempt is made (the ledger consumed the minutes - FareLedgerTests
        // Billing_CommitsBeforePayment covers the re-bill side).
        var wallet = new FakeWallet { CashBalance = 2f, TransactionThrow = FakeWallet.ThrowMode.AfterMutation };
        var counted = new List<int>();

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true, recordCharge: counted.Add);

        Assert.True(outcome.Caught);
        Assert.Equal("InvalidOperationException", outcome.CaughtType);
        Assert.Equal(2, outcome.CountedDollars); // the confirmed cash part
        Assert.Equal(new[] { 2 }, counted);
        Assert.Equal(3, outcome.UnknownDollars); // the thrown bank part
        Assert.Equal(0, outcome.UnpaidDollars);
        Assert.Equal(0f, wallet.CashBalance); // 2 - 2: the cash mutation happened
        Assert.Equal(97f, wallet.BankBalance); // 100 - 3: mutated before the throw
        Assert.Equal(1, wallet.ChangeCalls);
        Assert.Equal(1, wallet.TransactionCalls); // exactly one attempt, no retry
        Assert.Equal(PaymentPartStatus.CallReturned, outcome.CashStatus);
        Assert.Equal(PaymentPartStatus.CallThrewUnknown, outcome.BankStatus);
    }

    [Fact]
    public void P06_MutateThenThrowOnCash_NothingConfirmed_CashPartUnknown()
    {
        // TD-01 family (fixed 2026-10-03): the cash call mutated and threw; the
        // outer catch aborts before the bank call. The part is UNKNOWN (the fake
        // shows the money moved), nothing is counted, nothing is retried.
        var wallet = new FakeWallet { CashBalance = 10f, ChangeThrow = FakeWallet.ThrowMode.AfterMutation };
        var counted = new List<int>();

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true, recordCharge: counted.Add);

        Assert.True(outcome.Caught);
        Assert.Equal(0, outcome.CountedDollars); // nothing confirmed
        Assert.Empty(counted);
        Assert.Equal(5, outcome.UnknownDollars); // the thrown cash part
        Assert.Equal(0, outcome.UnpaidDollars);
        Assert.Equal(5f, wallet.CashBalance); // 10 - 5: mutated before the throw
        Assert.Equal(1, wallet.ChangeCalls);
        Assert.Equal(0, wallet.TransactionCalls);
        Assert.Equal(PaymentPartStatus.CallThrewUnknown, outcome.CashStatus);
        Assert.Equal(PaymentPartStatus.NotAttempted, outcome.BankStatus);
    }

    [Fact]
    public void P07_SilentNoOp_ReturnStillCounts_NothingMoves()
    {
        // A clean return is not proof of payment (S1API no-ops when
        // MoneyManager.Instance is null, Money.cs:43/57). CountedDollars means
        // only that the void payment call returned; FareMeter reports balance
        // movement as unverified and never retries a thrown/unknown part.
        var wallet = new FakeWallet { CashBalance = 0f, SilentNoOp = true };

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true);

        Assert.False(outcome.Caught);
        Assert.Equal(5, outcome.CountedDollars);
        Assert.Equal(1, wallet.TransactionCalls);
        Assert.Equal(100f, wallet.BankBalance); // unchanged: silent no-op
        Assert.Equal(PaymentPartStatus.CallReturned, outcome.BankStatus); // never "Confirmed"
    }

    [Fact]
    public void P08_Client_MakesNoWalletCall_AndClaimsNoCharge()
    {
        // TD-04 (fixed 2026-10-03): the client path performs no money call and
        // records no returned amount; it may show only the calculated fare.
        var wallet = new FakeWallet();
        var counted = new List<int>();
        int warns = 0;

        PaymentOutcome outcome = FarePayment.Execute(
            5, wallet, isHost: false, recordCharge: counted.Add, clientWarnOnce: () => warns++);

        Assert.Equal(0, wallet.GetCalls);
        Assert.Equal(0, wallet.ChangeCalls);
        Assert.Equal(0, wallet.TransactionCalls);
        Assert.True(outcome.NotHost);
        Assert.Equal(0, outcome.CountedDollars);
        Assert.Equal(5, outcome.CalculatedDollars);
        Assert.Empty(counted);
        Assert.Equal(1, warns);
    }

    [Theory]
    [InlineData(100f, 5, 5)]
    [InlineData(5f, 5, 5)]
    [InlineData(5.9f, 5, 5)]
    [InlineData(2.9f, 5, 2)]
    [InlineData(0f, 5, 0)]
    [InlineData(-1f, 5, 0)]
    [InlineData(float.PositiveInfinity, 5, 5)]
    [InlineData(float.NegativeInfinity, 5, 0)]
    [InlineData(float.NaN, 5, 0)]
    public void P09_Split_SpecialValues(float cash, int dollars, int expectedCash)
    {
        // Pins the TEST BUILD's split semantics (comparison-operator mirror).
        // Production runs the original Mathf expression verbatim (IL2CPPMELON
        // branch), so the mirror's Unity equivalence is not load-bearing.
        Assert.Equal(expectedCash, FarePayment.SplitCash(cash, dollars));
    }

    [Fact]
    public void P10_SummaryLogThrows_AfterCounting_IsStillCounted()
    {
        // Kept from the pre-fix characterization: the summary log runs inside the
        // outer try after the counter (FareMeter.cs:267 before 270-276), so a
        // log/config failure there leaves the counted amount intact and is
        // reported as a POST-PAYMENT failure (the old blanket "fare skipped"
        // wording is gone with TD-01).
        var wallet = new FakeWallet { CashBalance = 100f };
        var counted = new List<int>();

        PaymentOutcome outcome = FarePayment.Execute(
            5, wallet, isHost: true,
            recordCharge: counted.Add,
            chargeSummaryLog: () => throw new InvalidOperationException("fake: summary log failed"));

        Assert.True(outcome.Caught);
        Assert.Equal("InvalidOperationException", outcome.CaughtType);
        Assert.Equal(5, outcome.CountedDollars); // counted BEFORE the log failed
        Assert.Equal(new[] { 5 }, counted);
        Assert.Equal(PaymentPartStatus.CallReturned, outcome.CashStatus); // money call completed
    }

    [Fact]
    public void P11_ClientWarnThrows_AbortsTheClientPathBeforeCounting()
    {
        // Kept from the pre-fix characterization: the client warn block sits
        // inside the outer try before any counting (FareMeter.cs:238-244 order);
        // a throw there aborts the path entirely.
        var wallet = new FakeWallet();
        var counted = new List<int>();

        PaymentOutcome outcome = FarePayment.Execute(
            5, wallet, isHost: false,
            recordCharge: counted.Add,
            clientWarnOnce: () => throw new InvalidOperationException("fake: warn failed"));

        Assert.True(outcome.Caught);
        Assert.Equal(0, outcome.CountedDollars);
        Assert.Empty(counted);
        Assert.False(outcome.NotHost);
        Assert.Equal(0, wallet.ChangeCalls);
        Assert.Equal(0, wallet.TransactionCalls);
    }
}

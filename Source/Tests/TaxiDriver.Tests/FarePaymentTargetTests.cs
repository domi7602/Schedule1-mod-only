using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Regression tests for the payment semantics fixed with TD-01/TD-02/TD-04
/// (2026-10-03). Before the fixes every test here was skipped (each failed
/// against the extracted behavior by design); they are active since the fixes
/// landed. A fake wallet proves how a modeled scenario is HANDLED, not that the
/// scenario occurs in the game.
/// </summary>
public class FarePaymentTargetTests
{
    [Fact]
    public void TT01_CashReadFailure_FailsClosed_NoMoneyCall_NoCharge()
    {
        // TD-02: without a valid cash balance no payment method is selected
        // speculatively; the amount is reported unpaid, not banked.
        var wallet = new FakeWallet { ThrowOnGet = true };

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true);

        Assert.True(outcome.CashReadFailed);
        Assert.Equal(0, wallet.ChangeCalls);
        Assert.Equal(0, wallet.TransactionCalls);
        Assert.Equal(0, outcome.CountedDollars);
        Assert.Equal(5, outcome.UnpaidDollars);
        Assert.Equal(PaymentPartStatus.NotAttempted, outcome.CashStatus);
        Assert.Equal(PaymentPartStatus.NotAttempted, outcome.BankStatus);
    }

    [Fact]
    public void TT02_BankThrow_CountsOnlyTheConfirmedCashPart_BankStaysUnknown()
    {
        // TD-01: confirmed partial payments are counted; the thrown part is
        // UNKNOWN (never "not charged"), named in the log, and never retried.
        var wallet = new FakeWallet { CashBalance = 2f, TransactionThrow = FakeWallet.ThrowMode.AfterMutation };

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true);

        Assert.Equal(2, outcome.CashPart);
        Assert.Equal(2, outcome.CountedDollars);
        Assert.Equal(3, outcome.UnknownDollars);
        Assert.Equal(0, outcome.UnpaidDollars);
        Assert.Equal(PaymentPartStatus.CallReturned, outcome.CashStatus);
        Assert.Equal(PaymentPartStatus.CallThrewUnknown, outcome.BankStatus);
        Assert.Equal(1, wallet.ChangeCalls);
        Assert.Equal(1, wallet.TransactionCalls); // no retry of the unknown part
    }

    [Fact]
    public void TT03_MutateThenThrow_NeverClassifiedAsUnpaid()
    {
        // TD-01 design rule: a thrown call with visible mutation is UNKNOWN,
        // not "safely unpaid" and not "confirmed"; exactly one attempt.
        var wallet = new FakeWallet { CashBalance = 10f, ChangeThrow = FakeWallet.ThrowMode.AfterMutation };

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: true);

        Assert.Equal(5f, wallet.CashBalance); // mutation is visible in the fake
        Assert.Equal(PaymentPartStatus.CallThrewUnknown, outcome.CashStatus);
        Assert.Equal(5, outcome.UnknownDollars);
        Assert.Equal(0, outcome.UnpaidDollars); // never "uncharged"
        Assert.Equal(0, outcome.CountedDollars); // never "charged" either
        Assert.Equal(1, wallet.ChangeCalls); // exactly one attempt, no retry
        Assert.Equal(0, wallet.TransactionCalls);
    }

    [Fact]
    public void TT04_Client_NeverClaimsCharged()
    {
        // TD-04: a client reports a calculated fare, never a confirmed charge.
        var wallet = new FakeWallet();

        PaymentOutcome outcome = FarePayment.Execute(5, wallet, isHost: false);

        Assert.True(outcome.NotHost);
        Assert.Equal(0, outcome.CountedDollars);
        Assert.Equal(5, outcome.CalculatedDollars);
        Assert.Equal(0, wallet.ChangeCalls);
        Assert.Equal(0, wallet.TransactionCalls);
    }
}

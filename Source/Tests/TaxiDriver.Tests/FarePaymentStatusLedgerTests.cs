using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public sealed class FarePaymentStatusLedgerTests
{
    [Fact]
    public void CleanMoneyCallReturn_IsTrackedAsReturnedButNotVerified()
    {
        var ledger = new FarePaymentStatusLedger();
        ledger.Record(new PaymentOutcome
        {
            DueDollars = 5,
            CountedDollars = 5,
            CashPart = 5,
            CashStatus = PaymentPartStatus.CallReturned,
            ChargeRecorded = true,
        });

        Assert.Equal(5, ledger.ReturnedDollars);
        Assert.Equal(0, ledger.UnknownDollars);
        Assert.Equal(0, ledger.UnpaidDollars);
        Assert.False(ledger.ClientOnly);
        Assert.Equal("payment calls returned $5 (balance movement unverified), unknown $0, unpaid $0", ledger.SummaryText());
    }

    [Fact]
    public void ThrownAndUnattemptedParts_AreAggregatedSeparately()
    {
        var ledger = new FarePaymentStatusLedger();
        ledger.Record(new PaymentOutcome
        {
            DueDollars = 5,
            CountedDollars = 2,
            CashPart = 2,
            BankPart = 3,
            CashStatus = PaymentPartStatus.CallReturned,
            BankStatus = PaymentPartStatus.CallThrewUnknown,
            UnknownDollars = 3,
            ChargeRecorded = true,
            Caught = true,
        });
        ledger.RecordUnpaid(5);

        Assert.Equal(2, ledger.ReturnedDollars);
        Assert.Equal(3, ledger.UnknownDollars);
        Assert.Equal(5, ledger.UnpaidDollars);
        Assert.Equal("payment calls returned $2 (balance movement unverified), unknown $3, unpaid $5", ledger.SummaryText());
    }

    [Fact]
    public void NonHostOutcome_RecordsClientOnlyWithoutClaimingPayment()
    {
        var ledger = new FarePaymentStatusLedger();
        ledger.Record(new PaymentOutcome
        {
            DueDollars = 5,
            CalculatedDollars = 5,
            NotHost = true,
        });

        Assert.True(ledger.ClientOnly);
        Assert.Equal(0, ledger.ReturnedDollars);
        Assert.Equal(0, ledger.UnknownDollars);
        Assert.Equal(0, ledger.UnpaidDollars);
        Assert.Equal("no payment attempted on this client; host authority required", ledger.SummaryText());
    }

    [Fact]
    public void ClientAuthoritySkip_DoesNotHideEarlierSettlementResults()
    {
        var ledger = new FarePaymentStatusLedger();
        ledger.Record(new PaymentOutcome { CountedDollars = 3, ChargeRecorded = true });
        ledger.Record(new PaymentOutcome { NotHost = true });

        Assert.Equal(
            "payment skipped on this client (host authority); payment calls returned $3 (balance movement unverified), unknown $0, unpaid $0",
            ledger.SummaryText());
    }

    [Fact]
    public void Reset_ClearsPaymentStatusForTheNextRide()
    {
        var ledger = new FarePaymentStatusLedger();
        ledger.RecordUnpaid(5);
        ledger.Record(new PaymentOutcome { CountedDollars = 2, ChargeRecorded = true });

        ledger.Reset();

        Assert.Equal(0, ledger.ReturnedDollars);
        Assert.Equal(0, ledger.UnknownDollars);
        Assert.Equal(0, ledger.UnpaidDollars);
        Assert.False(ledger.ClientOnly);
    }
}

using System.Text.Json;
using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

// Delegate seam only: these tests do not call or prove the native cash API.
public class RewardSettlementTests
{
    private static BountyContract Contract() => new() { Status = EBountyStatus.Active, RewardCash = 300, AwaitingDrop = true };

    [Fact]
    public void SuccessMarksCompletedBeforeAnyLaterJournalOrConsumptionWork()
    {
        var c = Contract();
        int calls = 0;
        Assert.Equal(RewardAttempt.Paid, BountyRewardSettlement.TryPay(c, cash => { Assert.Equal(300f, cash); calls++; }));
        Assert.Equal(EBountyStatus.Completed, c.Status);
        Assert.False(c.RewardPaymentUncertain);
        Assert.False(c.AwaitingDrop);
        Assert.Equal(RewardAttempt.Rejected, BountyRewardSettlement.TryPay(c, _ => calls++));
        Assert.Equal(1, calls);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ExceptionPreservesClaimAndBlocksRetryRegardlessOfWhetherSimulatedCashMoved(bool moved)
    {
        var c = Contract();
        int calls = 0;
        float simulatedCash = 0;
        Assert.Equal(RewardAttempt.Uncertain, BountyRewardSettlement.TryPay(c, cash =>
        {
            calls++;
            if (moved) simulatedCash += cash;
            throw new IOException("injected native-call exception");
        }));
        Assert.True(c.RewardPaymentUncertain);
        Assert.False(BountyDeadlinePolicy.IsEligible(c, 0));
        Assert.Equal(EBountyStatus.Active, c.Status);
        Assert.True(c.AwaitingDrop);
        Assert.Equal(RewardAttempt.Rejected, BountyRewardSettlement.TryPay(c, _ => calls++));
        Assert.Equal(1, calls);
        Assert.Equal(moved ? 300f : 0f, simulatedCash);
        var restored = JsonSerializer.Deserialize<BountyContract>(JsonSerializer.Serialize(c, new JsonSerializerOptions { IncludeFields = true }), new JsonSerializerOptions { IncludeFields = true })!;
        Assert.True(restored.RewardPaymentUncertain);
        Assert.Equal(RewardAttempt.Rejected, BountyRewardSettlement.TryPay(restored, _ => calls++));
    }

    [Fact]
    public void ReentrantPaymentIsBlockedBeforeCallingTheProvider()
    {
        var c = Contract();
        int calls = 0;
        Assert.Equal(RewardAttempt.Paid, BountyRewardSettlement.TryPay(c, _ =>
        {
            calls++;
            Assert.Equal(RewardAttempt.Rejected, BountyRewardSettlement.TryPay(c, _ => calls++));
        }));
        Assert.Equal(1, calls);
    }

    [Fact]
    public void FailureOnOneClaimDoesNotBlockAnotherClaim()
    {
        var first = Contract();
        var next = Contract();
        Assert.Equal(RewardAttempt.Uncertain, BountyRewardSettlement.TryPay(first, _ => throw new IOException()));
        Assert.Equal(RewardAttempt.Paid, BountyRewardSettlement.TryPay(next, _ => { }));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(-1f)]
    [InlineData(0f)]
    public void InvalidCashNeverReachesProvider(float cash)
    {
        var c = Contract();
        c.RewardCash = cash;
        Assert.Equal(RewardAttempt.Rejected, BountyRewardSettlement.TryPay(c, _ => Assert.Fail("invalid payout")));
        Assert.False(c.RewardPaymentUncertain);
    }
}

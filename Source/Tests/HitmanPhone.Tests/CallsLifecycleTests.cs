using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

// ---------------------------------------------------------------------------
// HP-10 — only a CONFIRMED delivery consumes the slot; failed deliveries stay
//          retryable within a bounded attempt budget.
// ---------------------------------------------------------------------------

public class BountyOfferDeliveryTests
{
    [Theory]
    [InlineData(BountyOfferDelivery.Delivered, true)]
    [InlineData(BountyOfferDelivery.NoCaller, false)]
    [InlineData(BountyOfferDelivery.SendFailed, false)]
    [InlineData(BountyOfferDelivery.Blocked, false)]
    public void ConsumesSlot_OnlyOnConfirmedDelivery(BountyOfferDelivery result, bool consumes)
        => Assert.Equal(consumes, BountyOfferDispatchPolicy.ConsumesSlot(result));

    [Fact]
    public void DeferFailedDispatch_KeepsSlotRetryableUntilBudgetIsUsedUp()
    {
        var state = new BountyCallScheduleState();
        var slot = new BountyCallSlot { Day = 6, MinuteOfDay = 500, CallerIndex = 0 };
        state.Slots.Add(slot);
        var rng = new Random(3);

        for (int i = 1; i < BountyCallSchedulerConstants.MaxDeliveryAttempts; i++)
        {
            Assert.True(BountyCallSchedulePlan.DeferFailedDispatch(slot, state, 6, 500, 20, rng));
            Assert.False(slot.Dispatched);
            Assert.False(slot.Cancelled);
            Assert.Equal(i, slot.DispatchAttempts);
        }

        // Budget exhausted -> give up (cancel), never counted as delivered.
        Assert.False(BountyCallSchedulePlan.DeferFailedDispatch(slot, state, 6, 500, 20, rng));
        Assert.True(slot.Cancelled);
        Assert.False(slot.Dispatched);
    }

    [Fact]
    public void ConfirmDispatch_StampsActualTimeAndConsumesOnce()
    {
        var slot = new BountyCallSlot { Day = 6, MinuteOfDay = 500, CallerIndex = 0 };

        BountyCallSchedulePlan.ConfirmDispatch(slot, 6, 505);

        Assert.True(slot.Dispatched);
        Assert.Equal(6 * 1440L + 505, slot.DispatchedMinSum);
    }
}

// ---------------------------------------------------------------------------
// HP-15 — a later, longer caller lock is never shortened by a shorter one.
// ---------------------------------------------------------------------------

public class BountyCallerCooldownTests
{
    [Fact]
    public void MergeEnd_KeepsTheLongerExistingCooldown()
    {
        // 14-day death lock (day 10 -> 24) followed by a 3-day decline lock.
        Assert.Equal(24, BountyCallerCooldown.MergeEnd(10, 24, 3));
    }

    [Fact]
    public void MergeEnd_ExtendsWithALongerNewCooldown()
    {
        // 3-day decline lock (day 10 -> 13) followed by a 14-day death lock.
        Assert.Equal(24, BountyCallerCooldown.MergeEnd(10, 13, 14));
    }

    [Fact]
    public void MergeEnd_UsesNewWhenNoneExists()
    {
        Assert.Equal(13, BountyCallerCooldown.MergeEnd(10, 0, 3));
    }

    [Fact]
    public void MergeEnd_AvailabilityIsExactlyAtExpiry()
    {
        int until = BountyCallerCooldown.MergeEnd(10, 0, 3);
        Assert.Equal(13, until);
        // IsCallerOnCooldown uses `day < until`, so the caller is available again
        // exactly on the expiry day.
        Assert.False(13 < until);
        Assert.True(12 < until);
    }
}

// ---------------------------------------------------------------------------
// R-04 — one open offer at a time on the shared caller thread; stale offers can
//        never resolve and non-offer messages preserve the decision path.
// ---------------------------------------------------------------------------

public class BountyOfferPolicyTests
{
    private static BountyOffer Open() => new()
    {
        OfferId = "o1",
        CallerIndex = 0,
        TargetNpcId = "npc_a"
    };

    [Fact]
    public void CanResolve_OnlyWhileOpen()
    {
        Assert.True(BountyOfferPolicy.CanResolve(Open()));
        Assert.False(BountyOfferPolicy.CanResolve(null));
    }

    [Fact]
    public void Supersede_InvalidatesAnOpenPredecessorOnce()
    {
        var offer = Open();

        Assert.True(BountyOfferPolicy.Supersede(offer));
        Assert.Equal(BountyOfferStatus.Superseded, offer.Status);
        Assert.False(BountyOfferPolicy.CanResolve(offer));
        // Already resolved -> nothing left to supersede.
        Assert.False(BountyOfferPolicy.Supersede(offer));
        Assert.False(BountyOfferPolicy.Supersede(null));
    }

    [Fact]
    public void SupersededOffer_CanNoLongerBeResolved_NoStaleAccept()
    {
        var oldOffer = Open();
        var newOffer = Open();

        BountyOfferPolicy.Supersede(oldOffer);

        Assert.False(BountyOfferPolicy.CanResolve(oldOffer));
        Assert.True(BountyOfferPolicy.CanResolve(newOffer));
    }

    [Fact]
    public void ResolvedOffer_IsSingleUse()
    {
        var offer = Open();
        offer.Status = BountyOfferStatus.Accepted;
        Assert.False(BountyOfferPolicy.CanResolve(offer));
    }

    [Fact]
    public void MustPreserveOpenOfferResponses_TrueOnlyWhileOpen()
    {
        var offer = Open();

        Assert.True(BountyOfferPolicy.MustPreserveOpenOfferResponses(offer));
        BountyOfferPolicy.Supersede(offer);
        Assert.False(BountyOfferPolicy.MustPreserveOpenOfferResponses(offer));
        Assert.False(BountyOfferPolicy.MustPreserveOpenOfferResponses(null));
    }
}

// ---------------------------------------------------------------------------
// R-05 — the target is re-resolved and re-checked (alive AND conscious) at
//        accept time; pre-accept eliminations are not credited.
// ---------------------------------------------------------------------------

public class BountyAcceptPolicyTests
{
    [Fact]
    public void IsEligibleTarget_RequiresAliveAndConscious()
    {
        Assert.True(BountyAcceptPolicy.IsEligibleTarget(true, false, false));
        Assert.False(BountyAcceptPolicy.IsEligibleTarget(true, true, false));   // dead
        Assert.False(BountyAcceptPolicy.IsEligibleTarget(true, false, true));   // knocked out before accept
        Assert.False(BountyAcceptPolicy.IsEligibleTarget(false, false, false)); // not resolvable
    }

    [Fact]
    public void Decide_PrioritisesStaleSaveThenTargetState()
    {
        Assert.Equal(BountyAcceptDecision.StaleSave, BountyAcceptPolicy.Decide(false, true, true));
        Assert.Equal(BountyAcceptDecision.TargetGone, BountyAcceptPolicy.Decide(true, false, false));
        Assert.Equal(BountyAcceptDecision.TargetNotEligible, BountyAcceptPolicy.Decide(true, true, false));
        Assert.Equal(BountyAcceptDecision.Accept, BountyAcceptPolicy.Decide(true, true, true));
    }
}

using System;
using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

// ---------------------------------------------------------------------------
// HP-11 — call-plan persistence fingerprint (persist even without a due call,
//          never rewrite an unchanged plan).
// ---------------------------------------------------------------------------

public class BountyCallPlanPersistenceTests
{
    [Fact]
    public void PlanFingerprint_ChangesWhenThePlanIsGenerated()
    {
        var state = new BountyCallScheduleState();
        long before = BountyCallSchedulePlan.PlanFingerprint(state);

        BountyCallSchedulePlan.EnsurePlannedThrough(state, 10, 800, 17, 3, new Random(1234));

        Assert.True(state.Slots.Count > 0);
        Assert.NotEqual(before, BountyCallSchedulePlan.PlanFingerprint(state));
    }

    [Fact]
    public void PlanFingerprint_IsStableWhenNothingChanges()
    {
        var state = new BountyCallScheduleState();
        var rng = new Random(99);
        BountyCallSchedulePlan.EnsurePlannedThrough(state, 10, 800, 17, 3, rng);
        long first = BountyCallSchedulePlan.PlanFingerprint(state);

        // A second identical pass (same day/horizon) must not mutate the plan, so
        // the scheduler can skip the checkpoint write.
        BountyCallSchedulePlan.EnsurePlannedThrough(state, 10, 800, 17, 3, rng);

        Assert.Equal(first, BountyCallSchedulePlan.PlanFingerprint(state));
    }

    [Fact]
    public void PlanFingerprint_ChangesOnSilentCancellation()
    {
        var state = new BountyCallScheduleState();
        state.Slots.Add(new BountyCallSlot { Day = 5, MinuteOfDay = 500, CallerIndex = 0 });
        long before = BountyCallSchedulePlan.PlanFingerprint(state);

        // PeekDue cancels a slot missed beyond the grace window (mutates the plan).
        Assert.Null(BountyCallSchedulePlan.PeekDue(state, 5, 500 + 181));

        Assert.True(state.Slots[0].Cancelled);
        Assert.NotEqual(before, BountyCallSchedulePlan.PlanFingerprint(state));
    }

    [Fact]
    public void PlanFingerprint_ChangesOnReschedule()
    {
        var state = new BountyCallScheduleState();
        var slot = new BountyCallSlot { Day = 6, MinuteOfDay = 470, CallerIndex = 0 };
        state.Slots.Add(slot);
        long before = BountyCallSchedulePlan.PlanFingerprint(state);

        BountyCallSchedulePlan.RescheduleToFreeSlot(slot, state, 6, 470, 14, new Random(5));

        Assert.NotEqual(before, BountyCallSchedulePlan.PlanFingerprint(state));
    }
}

// ---------------------------------------------------------------------------
// HP-12 — real per-day quota and minimum gap between ACTUAL deliveries.
// ---------------------------------------------------------------------------

public class BountyCallQuotaGapTests
{
    private static BountyCallScheduleState With(params (int day, int minute, bool dispatched)[] slots)
    {
        var state = new BountyCallScheduleState();
        foreach (var (day, minute, dispatched) in slots)
        {
            state.Slots.Add(new BountyCallSlot
            {
                Day = day,
                MinuteOfDay = minute,
                CallerIndex = 0,
                Dispatched = dispatched
            });
        }
        return state;
    }

    private static int LiveSlotsOnDay(BountyCallScheduleState state, int day)
    {
        int n = 0;
        foreach (var s in state.Slots)
            if (!s.Cancelled && s.Day == day) n++;
        return n;
    }

    [Fact]
    public void DispatchedCountOnDay_CountsOnlyActualDeliveries()
    {
        var state = With((6, 480, true), (6, 720, false), (7, 480, true));

        Assert.Equal(1, BountyCallSchedulePlan.DispatchedCountOnDay(state, 6));
        Assert.Equal(1, BountyCallSchedulePlan.DispatchedCountOnDay(state, 7));
    }

    [Fact]
    public void CanDispatchNow_EnforcesDailyQuota()
    {
        var state = With((6, 480, true), (6, 900, true));

        Assert.False(BountyCallSchedulePlan.CanDispatchNow(state, 6, 1200, out string reason));
        Assert.Contains("quota", reason);
    }

    [Fact]
    public void CanDispatchNow_EnforcesMinimumGapBetweenRealDeliveries()
    {
        var state = With((6, 480, true));

        Assert.False(BountyCallSchedulePlan.CanDispatchNow(state, 6, 480 + 209, out _));
        Assert.True(BountyCallSchedulePlan.CanDispatchNow(state, 6, 480 + 210, out _));
    }

    [Fact]
    public void CanDispatchNow_UsesActualDeliveryTimeNotPlannedTime()
    {
        var slot = new BountyCallSlot { Day = 6, MinuteOfDay = 480, CallerIndex = 0 };
        var state = new BountyCallScheduleState();
        state.Slots.Add(slot);

        // Delivered late at 10:00 (600), not the planned 08:00 (480).
        BountyCallSchedulePlan.ConfirmDispatch(slot, 6, 600);

        Assert.Equal(600, BountyCallSchedulePlan.LastDispatchMinuteOnDay(state, 6));
        Assert.False(BountyCallSchedulePlan.CanDispatchNow(state, 6, 700, out _));
        Assert.True(BountyCallSchedulePlan.CanDispatchNow(state, 6, 810, out _));
    }

    [Fact]
    public void IsFreeSlot_RejectsWithinGapOrOverQuota()
    {
        var state = With((6, 480, false), (6, 720, false));

        Assert.False(BountyCallSchedulePlan.IsFreeSlot(state, 6, 500, null));  // within 210 of 480
        Assert.False(BountyCallSchedulePlan.IsFreeSlot(state, 6, 1000, null)); // quota already full
        Assert.True(BountyCallSchedulePlan.IsFreeSlot(state, 7, 600, null));   // free day
    }

    [Fact]
    public void RescheduleToFreeSlot_NeverPilesUpOnAFullDay()
    {
        // Day 6 already holds 08:00 + 12:00 (quota full). A blocked slot must move
        // to another day, never become a third same-day call.
        var state = With((6, 480, false), (6, 720, false));
        var blocked = new BountyCallSlot { Day = 6, MinuteOfDay = 470, CallerIndex = 0 };
        state.Slots.Add(blocked);

        bool moved = BountyCallSchedulePlan.RescheduleToFreeSlot(blocked, state, 6, 470, 14, new Random(5));

        Assert.True(moved);
        Assert.True(blocked.Day > 6);
        Assert.Equal(1, LiveSlotsOnDay(state, blocked.Day));
        Assert.True(BountyCallSchedulePlan.IsFreeSlot(state, blocked.Day, blocked.MinuteOfDay, blocked));
    }

    [Fact]
    public void RescheduleToFreeSlot_KeepsMinimumGapWhenSameDayHasRoom()
    {
        var state = With((6, 480, false));
        var blocked = new BountyCallSlot { Day = 6, MinuteOfDay = 600, CallerIndex = 0 };
        state.Slots.Add(blocked);

        Assert.True(BountyCallSchedulePlan.RescheduleToFreeSlot(blocked, state, 6, 600, 14, new Random(7)));

        Assert.Equal(6, blocked.Day);
        Assert.True(blocked.MinuteOfDay > 600);
        Assert.True(Math.Abs(blocked.MinuteOfDay - 480)
                    >= BountyCallSchedulerConstants.MinGapMinutesBetweenSameDayCalls);
    }

    [Fact]
    public void RescheduleToFreeSlot_CancelsWhenNoRoomInsideHorizon()
    {
        var state = new BountyCallScheduleState();
        var blocked = new BountyCallSlot { Day = 6, MinuteOfDay = 400, CallerIndex = 0 };
        state.Slots.Add(blocked);

        // Horizon ends today (exclusive bound == today) -> nothing can be placed.
        Assert.False(BountyCallSchedulePlan.RescheduleToFreeSlot(blocked, state, 6, 1400, 6, new Random(1)));
        Assert.True(blocked.Cancelled);
    }

    [Fact]
    public void GeneratedPlan_RespectsQuotaAndGapWithinADay()
    {
        var state = new BountyCallScheduleState();
        BountyCallSchedulePlan.EnsurePlannedThrough(state, 10, 800, 24, 5, new Random(2026));

        for (int day = 10; day < 24; day++)
        {
            int count = 0;
            var minutes = new System.Collections.Generic.List<int>();
            foreach (var s in state.Slots)
            {
                if (s.Day != day) continue;
                count++;
                minutes.Add(s.MinuteOfDay);
            }
            Assert.True(count <= BountyCallSchedulerConstants.MaxOffersPerDay);
            minutes.Sort();
            for (int i = 1; i < minutes.Count; i++)
            {
                Assert.True(minutes[i] - minutes[i - 1]
                            >= BountyCallSchedulerConstants.MinGapMinutesBetweenSameDayCalls);
            }
        }
    }
}

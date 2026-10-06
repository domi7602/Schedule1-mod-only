using System;
using System.Collections.Generic;
using HitmanPhone.Persistence;

namespace HitmanPhone.Bounty;

/// <summary>
/// Pure generator/reconciler for the save-level caller schedule (package D).
///
/// The plan is generated ahead (irregular cadence: quiet days, single-call
/// days, occasional double-call days), persisted in the save, and consumed
/// exactly once per slot. Reloading or sleeping never re-rolls or replays
/// calls: missed slots are cancelled silently instead of firing as a
/// avalanche on the next tick. Deterministic per injected <see cref="Random"/>
/// so unit tests can pin the behaviour.
/// </summary>
public static class BountyCallSchedulePlan
{
    /// <summary>Earliest minute-of-day a call may ring (07:00).</summary>
    public const int EarliestMinuteOfDay = 7 * 60;

    /// <summary>Latest minute-of-day a call may ring (23:00, exclusive).</summary>
    public const int LatestMinuteOfDay = 23 * 60;

    /// <summary>Chance (percent) a plannable day stays quiet (before the forced-call rule).</summary>
    public const int NoCallChancePercent = 45;

    /// <summary>Chance (percent) of a single-call day; remainder = double-call day.</summary>
    public const int SingleCallChancePercent = 35;

    /// <summary>A today-slot must lie at least this many minutes in the future to be generated.</summary>
    public const int MinLeadMinutesToday = 45;

    /// <summary>
    /// A slot whose ring time lies further in the past than this is treated as
    /// a missed call (e.g. the player slept through it) and cancelled silently.
    /// </summary>
    public const int MissedGraceMinutes = 180;

    /// <summary>
    /// Extends <paramref name="state"/> with slots up to (excluding)
    /// <paramref name="throughDayExclusive"/>. Past undispatched slots are
    /// cancelled — offline time never replays as a call lawine. A fresh plan
    /// starts at <paramref name="today"/> (no backfill).
    /// </summary>
    public static void EnsurePlannedThrough(
        BountyCallScheduleState state, int today, int nowMinuteOfDay,
        int throughDayExclusive, int callerCount, Random rng)
    {
        if (state == null) return;
        if (state.Slots == null) state.Slots = new List<BountyCallSlot>();
        if (rng == null) throw new ArgumentNullException(nameof(rng));

        // Missed slots vanish silently (no replay after sleep/load/offline).
        for (int i = 0; i < state.Slots.Count; i++)
        {
            var s = state.Slots[i];
            if (!s.Dispatched && !s.Cancelled && s.Day < today) s.Cancelled = true;
        }

        if (state.GeneratedThroughDay < today)
            state.GeneratedThroughDay = today; // fresh/reset plan: start from "now"

        for (int day = state.GeneratedThroughDay; day < throughDayExclusive; day++)
        {
            GenerateDay(state, day, today, nowMinuteOfDay, callerCount, rng);
            state.GeneratedThroughDay = day + 1;
        }
    }

    /// <summary>
    /// Peeks the next due slot (oldest by time) for (today, nowMinuteOfDay)
    /// without consuming it. Stale slots (missed ring time) are cancelled
    /// inside. The scheduler confirms via <see cref="ConfirmDispatch"/> or
    /// pushes the slot via <see cref="Reschedule"/> when the offer is blocked.
    /// At most one slot is due per call — same-day double calls spread over
    /// successive ticks.
    /// </summary>
    public static BountyCallSlot PeekDue(BountyCallScheduleState state, int today, int nowMinuteOfDay)
    {
        if (state?.Slots == null) return null;
        BountyCallSlot best = null;
        for (int i = 0; i < state.Slots.Count; i++)
        {
            var s = state.Slots[i];
            if (s.Dispatched || s.Cancelled) continue;
            if (s.Day != today) continue;
            // Slept/paused through the ring time → missed call, never replayed.
            if (s.MinuteOfDay < nowMinuteOfDay - MissedGraceMinutes) { s.Cancelled = true; continue; }
            if (s.MinuteOfDay > nowMinuteOfDay) continue;
            if (best == null || s.MinuteOfDay < best.MinuteOfDay) best = s;
        }
        return best;
    }

    /// <summary>Marks the slot as dispatched (consumed exactly once).</summary>
    public static void ConfirmDispatch(BountyCallSlot slot)
    {
        if (slot != null) slot.Dispatched = true;
    }

    /// <summary>
    /// Pushes a blocked slot (no target / caller busy / contract cap reached) a
    /// few in-game hours out — blocked offers are deferred, never lost.
    /// </summary>
    public static void Reschedule(BountyCallSlot slot, int today, int nowMinuteOfDay, Random rng)
    {
        if (slot == null) return;
        if (rng == null) throw new ArgumentNullException(nameof(rng));
        int when = nowMinuteOfDay + rng.Next(120, 300);
        if (when + 5 >= LatestMinuteOfDay)
        {
            slot.Day = today + 1;
            slot.MinuteOfDay = rng.Next(EarliestMinuteOfDay, EarliestMinuteOfDay + 180);
        }
        else
        {
            slot.Day = today;
            slot.MinuteOfDay = when;
        }
    }

    private static void GenerateDay(
        BountyCallScheduleState state, int day, int today, int nowMinuteOfDay,
        int callerCount, Random rng)
    {
        if (callerCount <= 0) return;

        // Forced-call rule: never stay silent longer than
        // MaxIdleDaysBeforeCall days in a row.
        int lastCallDay = LastPlannedCallDay(state);
        int roll = rng.Next(100);
        int calls;
        if (day - lastCallDay > BountyCallSchedulerConstants.MaxIdleDaysBeforeCall)
            calls = 1; // forced — silence budget used up
        else if (roll < NoCallChancePercent)
            calls = 0;
        else if (roll < NoCallChancePercent + SingleCallChancePercent)
            calls = 1;
        else
            calls = 2;

        if (calls > BountyCallSchedulerConstants.MaxOffersPerDay)
            calls = BountyCallSchedulerConstants.MaxOffersPerDay;

        int firstMinute = -1;
        for (int n = 0; n < calls; n++)
        {
            int minute = SampleMinute(rng, firstMinute, day == today, nowMinuteOfDay);
            if (minute < 0) break;
            state.Slots.Add(new BountyCallSlot
            {
                Day = day,
                MinuteOfDay = minute,
                CallerIndex = rng.Next(callerCount)
            });
            if (firstMinute < 0) firstMinute = minute;
        }
    }

    /// <summary>
    /// Sample a ring time inside the active hours, at least
    /// MinGapMinutesBetweenSameDayCalls after <paramref name="firstMinute"/>.
    /// Returns -1 when no room is left. Today-slots must lie in the future.
    /// </summary>
    private static int SampleMinute(Random rng, int firstMinute, bool isToday, int nowMinuteOfDay)
    {
        int lo = EarliestMinuteOfDay;
        int hi = LatestMinuteOfDay;
        if (firstMinute >= 0)
            lo = firstMinute + BountyCallSchedulerConstants.MinGapMinutesBetweenSameDayCalls;
        if (isToday)
            lo = Math.Max(lo, nowMinuteOfDay + MinLeadMinutesToday);
        if (lo >= hi) return -1;
        return rng.Next(lo, hi);
    }

    /// <summary>Day of the newest slot that still counts as a call (not cancelled), or a very negative sentinel.</summary>
    private static int LastPlannedCallDay(BountyCallScheduleState state)
    {
        int last = int.MinValue / 4;
        for (int i = 0; i < state.Slots.Count; i++)
        {
            var s = state.Slots[i];
            if (s.Cancelled) continue;
            if (s.Day > last) last = s.Day;
        }
        return last;
    }
}

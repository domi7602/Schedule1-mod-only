// ---------------------------------------------------------------------------
// Scheduling/BountyCallPlan.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Scheduling/BountyCallSchedulerConstants.cs -> section Scheduling/BountyCallSchedulerConstants.cs
//   Scheduling/BountyBudget.cs           -> section Scheduling/BountyBudget.cs
//   Scheduling/BountyCallSchedulePlan.cs -> section Scheduling/BountyCallSchedulePlan.cs
//
// ---------------------------------------------------------------------------

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Scheduling/BountyCallSchedulerConstants.cs ====

    /// <summary>
    /// Public constants used by the caller scheduler. Kept here to share values
    /// between <see cref="BountyCallScheduler"/> and <see cref="BountyConversationRouter"/>
    /// without exposing them as fields on the scheduler itself (which would clutter
    /// its API for callers that only care about <c>Tick()</c>).
    /// </summary>
    public static class BountyCallSchedulerConstants
    {
        /// <summary>Cooldown applied to a caller when the player declines the offer.</summary>
        public const int CallerCooldownDaysAfterDecline = 3;

        /// <summary>Cooldown applied to a caller when the player dies during the contract.</summary>
        public const int CallerCooldownDaysAfterPlayerDeath = 14;

        /// <summary>How many in-game days the player has to deliver the photo before the contract expires.</summary>
        public const int ContractDeadlineDays = 3;

        // ------------------------------------------------------------------
        // Call-plan cadence (package D). All times are in-game minutes/days and
        // live in the persisted save-level plan — see BountyCallSchedulePlan.
        // ------------------------------------------------------------------

        /// <summary>Hard cap of offers dispatched per in-game day.</summary>
        public const int MaxOffersPerDay = 2;

        /// <summary>Minimum in-game minutes between two calls on the same day.</summary>
        public const int MinGapMinutesBetweenSameDayCalls = 210;

        /// <summary>Longest stretch of silence: after this many idle days a call is forced.</summary>
        public const int MaxIdleDaysBeforeCall = 2;

        /// <summary>Chance (percent) that a second call joins an already planned call day.</summary>
        public const int DoubleCallChancePercent = 25;

        /// <summary>Quiet hours: no calls scheduled in [QuietHourStart, QuietHourEnd).</summary>
        public const int QuietHourStart = 1;

        /// <summary>End of the quiet window (inclusive-exclusive, see QuietHourStart).</summary>
        public const int QuietHourEnd = 7;

        /// <summary>The plan is always generated this many days ahead of the current day.</summary>
        public const int PlanLeadDays = 7;

        /// <summary>Hard cap on simultaneously active bounty contracts.</summary>
        public const int MaxActiveContracts = 3;
    }
    // ==== end HitmanPhone section: Scheduling/BountyCallSchedulerConstants.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Scheduling/BountyBudget.cs ====
    using System;


    /// <summary>
    /// Budget-derived bounty terms (package B): reward, tier, and post-photo drop
    /// window computed from the target's weekly customer budget. Pure static math
    /// (no Unity, no game types) so Source/Tests/HitmanPhone.Tests can cover it.
    ///
    /// The weekly budget is the midpoint of the game's CustomerData
    /// MinWeeklySpend/MaxWeeklySpend for the target (0 = unknown, e.g. legacy
    /// contracts or NPCs without customer data).
    /// </summary>
    public static class BountyBudget
    {
        public const string TierLow = "low";
        public const string TierMid = "mid";
        public const string TierHigh = "high";

        /// <summary>Weekly-spend at or below this is "low" tier ($/week).</summary>
        public const float LowTierMaxWeeklySpend = 400f;

        /// <summary>Weekly-spend at or above this is "high" tier ($/week).</summary>
        public const float HighTierMinWeeklySpend = 900f;

        /// <summary>Reward = weekly budget × this factor.</summary>
        public const float RewardPerWeekFactor = 0.35f;

        public const float MinReward = 150f;
        public const float MaxReward = 2500f;

        /// <summary>Reward granularity, so prices stay round.</summary>
        public const float RewardRounding = 50f;

        // Agreed deadline table (post-photo drop window): low = 4h, mid = 3h,
        // high = 2h. Unknown budget behaves like mid.
        public const int LowTierWindowMinutes = 4 * 60;
        public const int MidTierWindowMinutes = 3 * 60;
        public const int HighTierWindowMinutes = 2 * 60;
        public const int UnknownTierWindowMinutes = MidTierWindowMinutes;

        /// <summary>
        /// Classify the target budget at offer time. "" = unknown/legacy (the
        /// caller must then keep legacy reward and deadline semantics).
        /// </summary>
        public static string TierFor(float weeklySpend)
        {
            if (weeklySpend <= 0f) return string.Empty;
            if (weeklySpend <= LowTierMaxWeeklySpend) return TierLow;
            if (weeklySpend >= HighTierMinWeeklySpend) return TierHigh;
            return TierMid;
        }

        public static int WindowMinutesFor(string? tier) => tier switch
        {
            TierLow => LowTierWindowMinutes,
            TierHigh => HighTierWindowMinutes,
            TierMid => MidTierWindowMinutes,
            _ => UnknownTierWindowMinutes
        };

        public static int WindowHoursFor(string? tier) => WindowMinutesFor(tier) / 60;

        /// <summary>
        /// Budget-based reward (rounded to <see cref="RewardRounding"/>). Unknown
        /// budget (≤ 0) falls back to the legacy 200–500 roll so test/force offers
        /// and pre-budget contracts keep their old behaviour.
        /// </summary>
        public static float RewardForBudget(float weeklySpend, Random rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (weeklySpend <= 0f) return LegacyReward(rng);
            float raw = weeklySpend * RewardPerWeekFactor;
            float clamped = Math.Clamp(raw, MinReward, MaxReward);
            return MathF.Round(clamped / RewardRounding) * RewardRounding;
        }

        /// <summary>Legacy reward band kept for unknown budgets: 200–500 cash, endpoints included.</summary>
        public static float LegacyReward(Random rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            return rng.Next(200, 501);
        }
    }
    // ==== end HitmanPhone section: Scheduling/BountyBudget.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Scheduling/BountyCallSchedulePlan.cs ====
    using System;
    using System.Collections.Generic;
    using HitmanPhone.Persistence;


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
    // ==== end HitmanPhone section: Scheduling/BountyCallSchedulePlan.cs ====
}

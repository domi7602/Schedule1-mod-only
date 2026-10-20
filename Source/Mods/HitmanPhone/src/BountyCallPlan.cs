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

        /// <summary>
        /// HP-10: how often a single planned slot may fail to deliver before it is
        /// given up (bounded retry — a transient caller/send failure never burns
        /// the call permanently, an unfixable one never loops forever).
        /// </summary>
        public const int MaxDeliveryAttempts = 5;
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
        public static BountyCallSlot? PeekDue(BountyCallScheduleState state, int today, int nowMinuteOfDay)
        {
            if (state?.Slots == null) return null;
            BountyCallSlot? best = null;
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

        // ------------------------------------------------------------------
        // HP-11 — plan change detection (persist even when nothing is due) and
        // HP-12 — real per-day quota / minimum-gap enforcement on top of the
        // plan, both derived from the ACTUAL deliveries recorded on each slot
        // (DispatchedMinSum). Pure, so Source/Tests/HitmanPhone.Tests covers it.
        // ------------------------------------------------------------------

        private static long MinSum(int day, int minuteOfDay) => day * 1440L + minuteOfDay;

        /// <summary>
        /// HP-11: cheap content fingerprint of the plan. The scheduler compares it
        /// before/after a planning pass and checkpoints when it changed — even when
        /// no offer became due — so generation, horizon extension, silent
        /// cancellation and reschedule survive a reload instead of being re-rolled.
        /// A tick that mutates nothing yields the same value (no rewrite).
        /// </summary>
        public static long PlanFingerprint(BountyCallScheduleState state)
        {
            unchecked
            {
                long h = 1469598103934665603L; // FNV-1a offset basis
                void Mix(long v) { h ^= v; h *= 1099511628211L; }
                if (state == null) return h;
                Mix(state.GeneratedThroughDay);
                if (state.Slots != null)
                {
                    Mix(state.Slots.Count);
                    for (int i = 0; i < state.Slots.Count; i++)
                    {
                        var s = state.Slots[i];
                        if (s == null) { Mix(-1); continue; }
                        Mix(s.Day);
                        Mix(s.MinuteOfDay);
                        Mix(s.CallerIndex);
                        Mix(s.Dispatched ? 1 : 0);
                        Mix(s.Cancelled ? 1 : 0);
                        Mix(s.DispatchedMinSum);
                        Mix(s.DispatchAttempts);
                    }
                }
                return h;
            }
        }

        /// <summary>Day of a slot's ACTUAL delivery (falls back to the planned day for legacy slots).</summary>
        private static int DeliveryDay(BountyCallSlot s)
            => s.DispatchedMinSum > 0 ? (int)(s.DispatchedMinSum / 1440L) : s.Day;

        /// <summary>Minute-of-day of a slot's ACTUAL delivery (falls back to the planned minute).</summary>
        private static int DeliveryMinute(BountyCallSlot s)
            => s.DispatchedMinSum > 0 ? (int)(s.DispatchedMinSum % 1440L) : s.MinuteOfDay;

        /// <summary>HP-12: how many offers were ACTUALLY delivered on <paramref name="day"/>.</summary>
        public static int DispatchedCountOnDay(BountyCallScheduleState state, int day)
        {
            if (state?.Slots == null) return 0;
            int n = 0;
            for (int i = 0; i < state.Slots.Count; i++)
            {
                var s = state.Slots[i];
                if (s == null || !s.Dispatched || s.Cancelled) continue;
                if (DeliveryDay(s) == day) n++;
            }
            return n;
        }

        /// <summary>HP-12: minute-of-day of the latest ACTUAL delivery on <paramref name="day"/>, or -1.</summary>
        public static int LastDispatchMinuteOnDay(BountyCallScheduleState state, int day)
        {
            if (state?.Slots == null) return -1;
            int last = -1;
            for (int i = 0; i < state.Slots.Count; i++)
            {
                var s = state.Slots[i];
                if (s == null || !s.Dispatched || s.Cancelled) continue;
                if (DeliveryDay(s) != day) continue;
                int m = DeliveryMinute(s);
                if (m > last) last = m;
            }
            return last;
        }

        /// <summary>
        /// HP-12: whether another offer may ACTUALLY be delivered right now,
        /// enforcing the daily quota and the minimum gap between real deliveries.
        /// Deferred/rescheduled slots that never reached the player never count.
        /// </summary>
        public static bool CanDispatchNow(
            BountyCallScheduleState state, int day, int nowMinuteOfDay, out string reason)
        {
            reason = string.Empty;
            if (state?.Slots == null) return true;
            if (DispatchedCountOnDay(state, day) >= BountyCallSchedulerConstants.MaxOffersPerDay)
            {
                reason = "daily offer quota reached";
                return false;
            }
            int last = LastDispatchMinuteOnDay(state, day);
            if (last >= 0 && nowMinuteOfDay - last < BountyCallSchedulerConstants.MinGapMinutesBetweenSameDayCalls)
            {
                reason = "minimum gap since the last delivery not reached";
                return false;
            }
            return true;
        }

        /// <summary>
        /// HP-12: true when (day, minuteOfDay) can host <paramref name="exclude"/>
        /// without breaking the per-day quota or the same-day minimum gap against
        /// every other live slot of the WHOLE plan.
        /// </summary>
        public static bool IsFreeSlot(
            BountyCallScheduleState state, int day, int minuteOfDay, BountyCallSlot? exclude)
        {
            if (state?.Slots == null) return true;
            int count = 0;
            for (int i = 0; i < state.Slots.Count; i++)
            {
                var s = state.Slots[i];
                if (s == null || s.Cancelled) continue;
                if (ReferenceEquals(s, exclude)) continue;
                if (s.Day != day) continue;
                count++;
                // A dispatched slot is spaced against its ACTUAL delivery time, a
                // planned one against its planned time.
                int occupiedMinute = s.Dispatched ? DeliveryMinute(s) : s.MinuteOfDay;
                if (Math.Abs(occupiedMinute - minuteOfDay) <
                    BountyCallSchedulerConstants.MinGapMinutesBetweenSameDayCalls)
                    return false;
            }
            return count < BountyCallSchedulerConstants.MaxOffersPerDay;
        }

        /// <summary>
        /// HP-12: move a blocked slot to the earliest free appointment across the
        /// whole plan (respecting quota + minimum gap); cancels the slot when no
        /// room exists inside the horizon. Blocked offers are deferred, never
        /// piled up — sleeping / day change / blockades can never cause an
        /// offer avalanche.
        /// </summary>
        public static bool RescheduleToFreeSlot(
            BountyCallSlot slot, BountyCallScheduleState state,
            int today, int nowMinuteOfDay, int throughDayExclusive, Random rng)
        {
            if (slot == null) return false;
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            for (int day = today; day < throughDayExclusive; day++)
            {
                int lo = EarliestMinuteOfDay;
                if (day == today) lo = Math.Max(lo, nowMinuteOfDay + MinLeadMinutesToday);
                int hi = LatestMinuteOfDay;
                if (lo >= hi) continue;

                // Deterministic scan first (stable, RNG-independent), then a random
                // probe so the reschedule does not always land on the same minute.
                for (int minute = lo; minute < hi; minute += 15)
                {
                    if (IsFreeSlot(state, day, minute, slot))
                    {
                        slot.Day = day;
                        slot.MinuteOfDay = minute;
                        slot.Cancelled = false;
                        return true;
                    }
                }
                for (int attempt = 0; attempt < 8; attempt++)
                {
                    int minute = rng.Next(lo, hi);
                    if (IsFreeSlot(state, day, minute, slot))
                    {
                        slot.Day = day;
                        slot.MinuteOfDay = minute;
                        slot.Cancelled = false;
                        return true;
                    }
                }
            }
            slot.Cancelled = true;
            return false;
        }

        /// <summary>
        /// HP-10: defer a FAILED delivery attempt. The slot stays retryable and is
        /// never consumed, until the bounded attempt budget is used up.
        /// </summary>
        public static bool DeferFailedDispatch(
            BountyCallSlot slot, BountyCallScheduleState state,
            int today, int nowMinuteOfDay, int throughDayExclusive, Random rng)
        {
            if (slot == null) return false;
            slot.DispatchAttempts++;
            if (slot.DispatchAttempts >= BountyCallSchedulerConstants.MaxDeliveryAttempts)
            {
                slot.Cancelled = true;
                return false;
            }
            return RescheduleToFreeSlot(slot, state, today, nowMinuteOfDay, throughDayExclusive, rng);
        }

        /// <summary>
        /// HP-10: confirm a CONFIRMED delivery — stamp the ACTUAL delivery time
        /// (the basis for the HP-12 quota/gap enforcement) and consume the slot
        /// exactly once. The planned Day/MinuteOfDay stay untouched.
        /// </summary>
        public static void ConfirmDispatch(BountyCallSlot slot, int day, int minuteOfDay)
        {
            if (slot == null) return;
            slot.Dispatched = true;
            slot.DispatchedMinSum = MinSum(day, minuteOfDay);
        }
    }
    // ==== end HitmanPhone section: Scheduling/BountyCallSchedulePlan.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Scheduling/BountyCallLifecycle.cs ====
    using System;
    using System.Collections.Generic;
    using HitmanPhone.Persistence;

    /// <summary>
    /// HP-15: pure caller-cooldown merge. A later, longer cooldown must never be
    /// shortened by a subsequent shorter one (e.g. a 14-day player-death lock
    /// followed by a 3-day decline lock keeps the 14 days).
    /// </summary>
    public static class BountyCallerCooldown
    {
        /// <summary>
        /// Later of the existing cooldown end and (currentDay + days). Returns the
        /// merged end day; callers persist the merged value.
        /// </summary>
        public static int MergeEnd(int currentDay, int existingUntil, int days)
            => Math.Max(existingUntil, currentDay + days);
    }

    /// <summary>Lifecycle status of a single caller offer on the shared thread (R-04).</summary>
    public enum BountyOfferStatus
    {
        Open = 0,
        Accepted = 1,
        Declined = 2,
        Superseded = 3
    }

    /// <summary>
    /// R-04: one caller offer with a stable identity. Exactly one offer is OPEN at
    /// a time on the shared caller thread; a newer offer supersedes an unanswered
    /// predecessor so its stale Accept/Decline buttons can never resolve a deal
    /// that no longer exists. Pure (no game types) so it is unit-testable.
    /// </summary>
    public sealed class BountyOffer
    {
        public string OfferId = "";
        public int CallerIndex;
        public string TargetNpcId = "";
        public BountyOfferStatus Status = BountyOfferStatus.Open;

        public bool IsOpen => Status == BountyOfferStatus.Open;
    }

    /// <summary>Pure decision rules for the shared-thread offer lifecycle (R-04).</summary>
    public static class BountyOfferPolicy
    {
        /// <summary>Single-use: only an OPEN offer may be resolved.</summary>
        public static bool CanResolve(BountyOffer? offer) => offer != null && offer.IsOpen;

        /// <summary>
        /// Supersede a still-open predecessor when a new offer is posted. Returns
        /// true when the predecessor was actually invalidated (so the caller can
        /// log/observe the displacement).
        /// </summary>
        public static bool Supersede(BountyOffer? previous)
        {
            if (previous == null || !previous.IsOpen) return false;
            previous.Status = BountyOfferStatus.Superseded;
            return true;
        }

        /// <summary>
        /// A message that is not itself a new offer (e.g. the drop-window warning)
        /// must NOT be posted bare while an offer is open — that replaces the
        /// decision buttons and strands the pending deal. Returns true when the
        /// open offer's responses must be re-attached.
        /// </summary>
        public static bool MustPreserveOpenOfferResponses(BountyOffer? current)
            => current != null && current.IsOpen;
    }

    /// <summary>Outcome of evaluating a captured offer against the live world (R-05).</summary>
    public enum BountyAcceptDecision
    {
        Accept = 0,
        StaleSave = 1,
        TargetGone = 2,
        TargetNotEligible = 3
    }

    /// <summary>
    /// R-05: pure accept-time validity rules. An elimination that happened BEFORE
    /// the accept — or that a different actor caused — must not be credited to a
    /// fresh contract, so the target is re-resolved by stable id and its
    /// eligibility (alive AND conscious) re-checked at accept time.
    /// </summary>
    public static class BountyAcceptPolicy
    {
        public static bool IsEligibleTarget(bool resolved, bool isDead, bool isUnconscious)
            => resolved && !isDead && !isUnconscious;

        public static BountyAcceptDecision Decide(bool saveMatches, bool targetResolved, bool targetEligible)
        {
            if (!saveMatches) return BountyAcceptDecision.StaleSave;
            if (!targetResolved) return BountyAcceptDecision.TargetGone;
            if (!targetEligible) return BountyAcceptDecision.TargetNotEligible;
            return BountyAcceptDecision.Accept;
        }
    }

    /// <summary>Result of trying to deliver an offer to the player (HP-10).</summary>
    public enum BountyOfferDelivery
    {
        Delivered = 0,
        NoCaller = 1,
        SendFailed = 2,
        Blocked = 3
    }

    /// <summary>
    /// HP-10: pure dispatch bookkeeping. Only a CONFIRMED delivery may consume the
    /// planned slot and count as a successful call; every other outcome leaves the
    /// slot retryable.
    /// </summary>
    public static class BountyOfferDispatchPolicy
    {
        public static bool ConsumesSlot(BountyOfferDelivery result)
            => result == BountyOfferDelivery.Delivered;
    }
}

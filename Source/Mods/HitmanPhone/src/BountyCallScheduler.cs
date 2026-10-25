using System;
using System.Threading;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.DeadDrops;
using S1Mods.Shared;

#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase D — Caller scheduling &amp; conversation opening (plan-based since the
/// budget/dead-drop update).
///
/// The scheduler runs on each <see cref="Mod.OnUpdate"/> tick (real-time
/// throttled). Instead of the old once-per-day 60% roll it consumes a
/// PERSISTED save-level call plan (<see cref="BountyCallSchedulePlan"/> /
/// <see cref="BountyCallScheduleState"/>): irregular cadence with quiet days,
/// single-call days and occasional double-call days, generated a week ahead in
/// in-game time. Missed slots are cancelled silently (never a lawine after
/// sleep/load); blocked slots are deferred, not lost.
///
/// Every offer carries its rolled terms structurally
/// (<see cref="BountyOfferTerms"/>): budget-based reward (target's weekly
/// customer spend), fixed dead drop and the post-photo drop window — all named
/// in the offer text BEFORE the player accepts.
///
/// Cooldown model (Phase D-Spec §3):
///   • Per-caller cooldown = 3 in-game days after a Decline or expiry.
///   • Per-caller cooldown = 14 in-game days after a player death in a contract.
/// </summary>
public static class BountyCallScheduler
{
    private static int _offersMade;
    private static int _offersSkippedCooldown;
    private static int _offersDeferred;
    private static int _noTargetNotice;
    private static long _lastTickRealtime;

    /// <summary>Total successful caller-rings across the save lifetime.</summary>
    public static int OffersMade => Volatile.Read(ref _offersMade);
    /// <summary>Times a due call had to wait because every caller was on cooldown.</summary>
    public static int OffersSkippedCooldown => Volatile.Read(ref _offersSkippedCooldown);
    /// <summary>Times a due call was deferred (contract cap / no target).</summary>
    public static int OffersDeferred => Volatile.Read(ref _offersDeferred);

    private static readonly Random Rng = new();

    /// <summary>Real-time throttle: the plan is consulted at most this often.</summary>
    private const long TickThrottleMs = 1500;

    /// <summary>
    /// Call this from <see cref="Mod.OnUpdate"/>. Cheap: internally throttled,
    /// then peeks the persisted plan for exactly one due slot per pass.
    /// </summary>
    public static void Tick()
    {
        var save = Mod.Instance?.Save;
        if (save == null) return;
        long nowMs = Environment.TickCount64;
        if (nowMs - _lastTickRealtime < TickThrottleMs) return;
        _lastTickRealtime = nowMs;

        // Singleplayer-only readiness gate (singleplayer + loaded save + main
        // scene). Multiplayer is unsupported and explicitly blocked here, never
        // run as a host-shared service (R-02 product decision).
        if (!BountyCallGate.IsReady)
        {
            if (BountyCallGate.IsMultiplayerSession) BountyConversationRouter.NotifyMultiplayerUnsupportedOnce();
            return;
        }

        // R-03: ONE consistent time sample (day + minute from the same reading) —
        // never a cached day combined with a fresh clock.
        if (!HitmanPhoneTime.TryGetSnapshot(out int day, out int nowMin, out _)) return;
        if (day < 0) return;

        if (save.CallSchedule == null) save.CallSchedule = new BountyCallScheduleState();
        var plan = save.CallSchedule;
        int horizon = day + BountyCallSchedulerConstants.PlanLeadDays;

        // HP-11: fingerprint the plan before the pass so a mutation without a due
        // call (generation, horizon extension, silent cancellation) is still
        // checkpointed and never re-rolled after a reload.
        long fingerprintBefore = BountyCallSchedulePlan.PlanFingerprint(plan);

        BountyCallSchedulePlan.EnsurePlannedThrough(
            plan, day, nowMin, horizon, BountyDialogTemplates.CallerPool.Count, Rng);

        var slot = BountyCallSchedulePlan.PeekDue(plan, day, nowMin);
        if (slot == null)
        {
            PersistIfPlanChanged(plan, fingerprintBefore);
            return;
        }

        // HP-12: the plan caps offers per day and their spacing at GENERATION
        // time; a rescheduled or backlogged slot must additionally be checked
        // against the REAL deliveries before it is allowed to ring.
        if (!BountyCallSchedulePlan.CanDispatchNow(plan, day, nowMin, out string dispatchBlocked))
        {
            BountyCallSchedulePlan.RescheduleToFreeSlot(slot, plan, day, nowMin, horizon, Rng);
            Interlocked.Increment(ref _offersDeferred);
            BountyPersistence.PersistCurrent();
            Mod.Log.Debug($"BountyCallScheduler: call deferred — {dispatchBlocked} (day={day}).");
            return;
        }

        // Contract cap: the planned call is deferred, not lost.
        if (save.Active.Count >= BountyCallSchedulerConstants.MaxActiveContracts)
        {
            BountyCallSchedulePlan.RescheduleToFreeSlot(slot, plan, day, nowMin, horizon, Rng);
            Interlocked.Increment(ref _offersDeferred);
            BountyPersistence.PersistCurrent();
            Mod.Log.Debug($"BountyCallScheduler: call deferred (active cap {BountyCallSchedulerConstants.MaxActiveContracts}).");
            return;
        }

        // Caller availability: the slot's caller first, then any off-cooldown one.
        if (!TryPickCaller(save, day, slot.CallerIndex, out int chosenIndex))
        {
            BountyCallSchedulePlan.RescheduleToFreeSlot(slot, plan, day, nowMin, horizon, Rng);
            Interlocked.Increment(ref _offersSkippedCooldown);
            BountyPersistence.PersistCurrent();
            Mod.Log.Debug($"BountyCallScheduler: call deferred — all callers on cooldown for day={day}.");
            return;
        }

        // Pick a target NPC. An empty network is often transient (dealers not
        // recruited, customer registry still loading). Moving the slot here used
        // to park a due call days later on the first empty tick, so no SMS was
        // ever written. Leave it due and retry until the missed-call grace
        // cancels it. Quota, cooldown and delivery failure still reschedule.
        var target = TargetSelector.PickRandom();
        if (target == null)
        {
            if (Interlocked.Exchange(ref _noTargetNotice, 1) == 0)
            {
                Mod.Log.Info("[BountyScheduler] offer waiting — no living customer in the player's own network yet. The due slot stays and retries.");
            }
            return;
        }
        Interlocked.Exchange(ref _noTargetNotice, 0);

        // Audit H2 pattern: roll the terms ONCE and pass them structurally into
        // the offer AND the accept handler — the body is display-only.
        var terms = BuildTerms(save, target);
        string callerName = BountyDialogTemplates.GetCallerName(chosenIndex);
        string style = BountyDialogTemplates.StyleForCallerIndex(chosenIndex);
        // Audit M7 pattern: show the formatted display name, not the raw NPC id.
        string body = BountyDialogTemplates.BuildForStyle(
            style, callerName, BountyQuest.FormatName(target.ID), TargetSelector.GetRegionName(target),
            terms.RewardCash, terms.DropName, BountyBudget.WindowHoursFor(terms.RewardTier));

        // HP-10: only a CONFIRMED delivery consumes the slot and counts as a call.
        // A missing caller wrapper or a caught SendTextMessage failure leaves the
        // slot retryable instead of burning the call forever.
        var delivery = BountyConversationRouter.SendOffer(save, chosenIndex, target, body, terms);
        if (!BountyOfferDispatchPolicy.ConsumesSlot(delivery))
        {
            BountyCallSchedulePlan.DeferFailedDispatch(slot, plan, day, nowMin, horizon, Rng);
            Interlocked.Increment(ref _offersDeferred);
            BountyPersistence.PersistCurrent();
            Mod.Log.Warn($"[BountyScheduler] offer delivery failed ({delivery}) on day {day} — slot kept retryable.");
            return;
        }

        BountyCallSchedulePlan.ConfirmDispatch(slot, day, nowMin);
        Interlocked.Increment(ref _offersMade);
        BountyPersistence.PersistCurrent();
        Mod.Log.Info($"[BountyScheduler] Day {day} {HitmanPhoneTime.To24HourTime(nowMin):D4}: " +
                     $"'{callerName}' offering bounty on '{target.ID}' ({style}, ${terms.RewardCash:N0}, " +
                     $"drop '{terms.DropName}', {BountyBudget.WindowHoursFor(terms.RewardTier)}h window).");
    }

    /// <summary>
    /// HP-11: checkpoint the plan when this pass mutated it without producing a
    /// due call. An unchanged tick leaves the fingerprint identical and writes
    /// nothing.
    /// </summary>
    private static void PersistIfPlanChanged(BountyCallScheduleState plan, long fingerprintBefore)
    {
        if (BountyCallSchedulePlan.PlanFingerprint(plan) == fingerprintBefore) return;
        BountyPersistence.PersistCurrent();
        Mod.Log.Debug("BountyCallScheduler: call plan changed without a due call — checkpointed.");
    }

    /// <summary>
    /// Roll the offer terms for this target: reward/tier/window from the
    /// target's weekly customer budget, plus a fixed dead drop (preferring one
    /// different from <see cref="BountySaveData.LastUsedDropId"/>). Unknown
    /// budgets (0) keep full legacy semantics: legacy reward, no tier, no
    /// post-photo drop window.
    /// </summary>
    private static BountyOfferTerms BuildTerms(BountySaveData save, S1NPC target)
    {
        float weekly = TargetSelector.TryGetWeeklySpend(target);
        var terms = new BountyOfferTerms { TargetWeeklySpend = weekly };
        if (weekly > 0f)
        {
            terms.RewardTier = BountyBudget.TierFor(weekly);
            terms.RewardCash = BountyBudget.RewardForBudget(weekly, Rng);
            terms.DropWindowMinutes = BountyBudget.WindowMinutesFor(terms.RewardTier);
        }
        else
        {
            terms.RewardTier = string.Empty;
            terms.RewardCash = BountyBudget.LegacyReward(Rng);
            terms.DropWindowMinutes = 0;
        }
        var drop = PickDeadDrop(save);
        if (drop != null)
        {
            terms.DropId = drop.GUID;
            terms.DropName = drop.Name;
        }
        return terms;
    }

    /// <summary>
    /// Random dead drop, preferring one different from the most recently used
    /// drop (variety). Falls back to the full pool of one. Null when no drop
    /// exists yet (the offer then keeps the legacy "any drop" terms).
    /// </summary>
    private static DeadDropInstance? PickDeadDrop(BountySaveData save)
    {
        try
        {
            var all = DeadDropManager.All;
            if (all == null || all.Length == 0) return null;
            var pool = all;
            if (!string.IsNullOrEmpty(save.LastUsedDropId) && all.Length > 1)
            {
                var filtered = Array.FindAll(all,
                    d => !string.Equals(d.GUID, save.LastUsedDropId, StringComparison.OrdinalIgnoreCase));
                if (filtered.Length > 0) pool = filtered;
            }
            return pool[Rng.Next(pool.Length)];
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"PickDeadDrop failed: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// The slot's caller is preferred (they scheduled the call); otherwise any
    /// caller whose cooldown elapsed. False → every caller busy.
    /// </summary>
    private static bool TryPickCaller(BountySaveData save, int day, int preferredIndex, out int chosenIndex)
    {
        chosenIndex = -1;
        var pool = BountyDialogTemplates.CallerPool;
        if (pool.Count == 0) return false;
        if (preferredIndex >= 0 && preferredIndex < pool.Count &&
            !IsCallerOnCooldown(save, day, preferredIndex))
        {
            chosenIndex = preferredIndex;
            return true;
        }
        int availableCount = 0;
        Span<int> available = stackalloc int[pool.Count];
        for (int i = 0; i < pool.Count; i++)
        {
            if (IsCallerOnCooldown(save, day, i)) continue;
            available[availableCount++] = i;
        }
        if (availableCount == 0) return false;
        chosenIndex = available[Rng.Next(availableCount)];
        return true;
    }

    private static bool IsCallerOnCooldown(BountySaveData save, int day, int callerIndex)
    {
        // Same key derivation as CooldownCaller / OnAccept.
        string id = $"caller_{BountyDialogTemplates.GetCallerName(callerIndex).ToLowerInvariant()}";
        return save.CallerCooldowns.TryGetValue(id, out int until) && day < until;
    }

    /// <summary>
    /// Mark a caller as on cooldown after decline / player death / expiry.
    /// Called by the response handlers in BountyConversationRouter.
    /// </summary>
    public static void CooldownCaller(int callerIndex, int days)
    {
        if (Mod.Instance == null || Mod.Instance.Save == null) return;
        string id = $"caller_{BountyDialogTemplates.GetCallerName(callerIndex).ToLowerInvariant()}";
        int day = HitmanPhoneTime.CurrentDay();
        if (day < 0) return; // invalid time sample — never write a bogus cooldown
        var cooldowns = Mod.Instance.Save.CallerCooldowns;
        int existing = cooldowns.TryGetValue(id, out int until) ? until : int.MinValue;
        // HP-15: keep the LATER of the existing and the new cooldown end — a short
        // decline lock must never shorten an already-running 14-day death lock.
        int merged = BountyCallerCooldown.MergeEnd(day, existing, days);
        cooldowns[id] = merged;
        // Audit (2026-09-10): cooldowns must survive a game restart (was RAM-only) —
        // every other state transition persists; PersistCurrent is null-guarded and
        // exception-swallowed, so this is safe/reentrant on all caller paths.
        BountyPersistence.PersistCurrent();
        Mod.Log.Debug($"Caller cooldown applied: {id} until day {merged} " +
                      $"(previous {(existing == int.MinValue ? "none" : existing.ToString())}, +{days}d).");
    }

    /// <summary>
    /// Test-only overload: force-fire a bounty call now with a chosen caller/target.
    /// Useful for manual verification (e.g. /bounty_force console command).
    /// </summary>
    public static void ForceOfferNow(int callerIndex, string targetNpcId)
    {
        if (Mod.Instance?.Save == null) return;
        try
        {
            var target = HitmanPhone.Persistence.TargetResolveHelper.FindById(targetNpcId);
            if (target == null) { Mod.Log.Warn($"ForceOffer: target '{targetNpcId}' not found."); return; }
#if DEBUG
            // Test scaffolding (DEBUG-only): the eligibility guard below blocks every
            // force-offer on early-game saves (no recruited dealers yet), which makes
            // QA of SendOffer/contract flow impossible. Release builds keep the guard.
            bool bypassEligibility = true;
#else
            bool bypassEligibility = false;
#endif
            if (!bypassEligibility && !TargetSelector.IsOwnCustomer(target))
            {
                Mod.Log.Warn($"ForceOffer: target '{targetNpcId}' is not an assigned customer of one of the player's recruited dealers.");
                return;
            }
            var terms = BuildTerms(Mod.Instance.Save, target);
            string callerName = BountyDialogTemplates.GetCallerName(callerIndex);
            string style = BountyDialogTemplates.StyleForCallerIndex(callerIndex);
            string body = BountyDialogTemplates.BuildForStyle(
                style, callerName, BountyQuest.FormatName(target.ID), TargetSelector.GetRegionName(target),
                terms.RewardCash, terms.DropName, BountyBudget.WindowHoursFor(terms.RewardTier));
            var delivery = BountyConversationRouter.SendOffer(Mod.Instance.Save, callerIndex, target, body, terms);
            Mod.Log.Info($"[BountyScheduler] Force-offer: '{callerName}' on '{target.ID}' " +
                         $"(${terms.RewardCash:N0}, drop '{terms.DropName}') — delivery={delivery}.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"ForceOfferNow failed: {ex}");
        }
    }

    /// <summary>
    /// Legacy hook kept for <see cref="SaveStateGuard.TriggerFullReload"/>: with
    /// the persisted call plan there is no unsaved day latch any more — plan
    /// state lives in the save and switches with it. Still resets the real-time
    /// throttle so a fresh save gets a prompt plan pass.
    /// </summary>
    public static void ResetDayLatch()
    {
        _lastTickRealtime = 0;
    }
}

/// <summary>
/// Singleplayer-only readiness gate for the whole caller / offer lifecycle
/// (HP-10/11/12/15, R-04/05). Delegates to the central
/// <see cref="SaveStateGuard.IsReady"/> gate (singleplayer + loaded save +
/// checkpoint not saving + main scene) so the caller system runs under exactly
/// the same conditions as the rest of the mod and is explicitly blocked in
/// multiplayer — never run as a host-shared service.
/// </summary>
internal static class BountyCallGate
{
    /// <summary>True when the caller system may run (singleplayer + save loaded).</summary>
    public static bool IsReady => SaveStateGuard.IsReady;

    /// <summary>True when the block is caused by a real multiplayer session.</summary>
    public static bool IsMultiplayerSession => !SaveStateGuard.IsSingleplayer;
}

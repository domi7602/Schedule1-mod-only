using System;
using System.Threading;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Messaging;

#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase D — Caller scheduling &amp; conversation opening.
///
/// The scheduler runs each <see cref="Mod.OnUpdate"/> tick. Once per in-game day
/// it picks:
///   • a random caller from <see cref="BountyDialogTemplates.CallerPool"/> whose
///     cooldown (per <see cref="BountySaveData.CallerCooldowns"/>) has elapsed,
///   • a random eligible target from <see cref="TargetSelector"/>,
///   • a caller style based on caller index,
/// and opens an in-game <see cref="Response"/> conversation via
/// <see cref="BountyConversationRouter"/> so the player sees the offer in the
/// phone's Messages app.
///
/// This deliberately does NOT push a fake phone call into <c>PayPhone.QueuedCall</c>
/// — instead we send the offer through Messages, which is the modern interface
/// the player uses for bounties, and avoids RPC plumbing for the phone system.
///
/// Cooldown model (Phase D-Spec §3):
///   • Per-caller cooldown = 3 in-game days after a Decline or expiry.
///   • Per-caller cooldown = 14 in-game days after a player death in a contract.
/// </summary>
public static class BountyCallScheduler
{
    private static int _lastFiredDay = -1;
    private static int _offersMade;
    private static int _offersSkippedCooldown;

    /// <summary>Total successful caller-rings across the save lifetime.</summary>
    public static int OffersMade => Volatile.Read(ref _offersMade);
    /// <summary>Times we didn't fire because every caller was on cooldown.</summary>
    public static int OffersSkippedCooldown => Volatile.Read(ref _offersSkippedCooldown);

    private static readonly Random Rng = new();

    /// <summary>
    /// Call this from <see cref="Mod.OnUpdate"/> at low frequency (e.g. once per
    /// ~5 in-game minutes). The scheduler decides whether to ring the player
    /// today based on cooldowns.
    /// </summary>
    public static void Tick()
    {
        if (Mod.Instance == null || Mod.Instance.Save == null) return;
        int day = HitmanPhoneTime.CurrentDay();
        if (day == _lastFiredDay) return; // already fired today
        if (day <= 0) return; // pre-game

        // One roll per in-game day: 60% chance to fire (one roll per day, never
        // re-rolled within the same day).
        bool fire = Rng.Next(100) < 60;
        if (!fire)
        {
            _lastFiredDay = day;
            return;
        }

        // Audit M4 (2026-09-01): the class doc promises a random caller, but the
        // old loop always took the FIRST caller off cooldown (near-always Ghost,
        // and style always "cold"). Collect the available ones and roll.
        var save = Mod.Instance.Save;
        int availableCount = 0;
        Span<int> available = stackalloc int[BountyDialogTemplates.CallerPool.Count];
        for (int i = 0; i < BountyDialogTemplates.CallerPool.Count; i++)
        {
            string id = $"caller_{BountyDialogTemplates.CallerPool[i].ToLowerInvariant()}";
            if (save.CallerCooldowns.TryGetValue(id, out int until) && day < until) continue;
            available[availableCount++] = i;
        }
        if (availableCount == 0)
        {
            Interlocked.Increment(ref _offersSkippedCooldown);
            _lastFiredDay = day;
            Mod.Log.Debug($"BountyCallScheduler.Tick: all callers on cooldown for day={day}.");
            return;
        }
        int chosenIndex = available[Rng.Next(availableCount)];

        // Pick a target NPC.
        var target = TargetSelector.PickRandom();
        if (target == null)
        {
            _lastFiredDay = day;
            Mod.Log.Debug($"BountyCallScheduler.Tick: no eligible target for day={day}.");
            return;
        }

        string callerName = BountyDialogTemplates.GetCallerName(chosenIndex);
        string style = BountyDialogTemplates.StyleForCallerIndex(chosenIndex);
        // Audit H2 (2026-09-01): roll the reward ONCE and pass it structurally
        // into the offer AND the accept handler — parsing it back out of the
        // body text was culture-fragile and always fell back to $10k.
        float reward = RewardFor(style);
        // Audit M7 (2026-09-01): show the formatted display name, not the raw
        // NPC id ("Ludwig Meyer" instead of "ludwig_meyer").
        string body = BountyDialogTemplates.BuildForStyle(
            style, callerName, BountyQuest.FormatName(target.ID), "Hyland Point", reward);

        // Send via the player's existing MessagesApp conversation pipeline.
        // We send through a hidden NPC helper so the contract is bound cleanly.
        BountyConversationRouter.SendOffer(save, chosenIndex, target, body, reward);

        Interlocked.Increment(ref _offersMade);
        _lastFiredDay = day;
        Mod.Log.Info($"[BountyScheduler] Day {day}: '{callerName}' offering bounty on '{target.ID}' ({style}, ${reward:N0}).");
    }

    /// <summary>
    /// Returns the requested fixed reward range for every caller style:
    /// 200–500 cash, both endpoints included.
    /// </summary>
    private static float RewardFor(string style)
    {
        return Rng.Next(200, 501);
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
        Mod.Instance.Save.CallerCooldowns[id] = day + days;
        Mod.Log.Debug($"Caller cooldown applied: {id} until day {day + days}.");
    }

    /// <summary>
    /// Test-only overload: force-fire a bounty call now with a chosen caller/target.
    /// Useful for manual verification (e.g. /bounty_force console command in
    /// Phase L).
    /// </summary>
    public static void ForceOfferNow(int callerIndex, string targetNpcId)
    {
        if (Mod.Instance?.Save == null) return;
        try
        {
            var target = HitmanPhone.Persistence.TargetResolveHelper.FindById(targetNpcId);
            if (target == null) { Mod.Log.Warn($"ForceOffer: target '{targetNpcId}' not found."); return; }
            if (!TargetSelector.IsOwnCustomer(target))
            {
                Mod.Log.Warn($"ForceOffer: target '{targetNpcId}' is not an assigned customer of one of the player's recruited dealers.");
                return;
            }
            string callerName = BountyDialogTemplates.GetCallerName(callerIndex);
            string style = BountyDialogTemplates.StyleForCallerIndex(callerIndex);
            float reward = RewardFor(style);
            string body = BountyDialogTemplates.BuildForStyle(
                style, callerName, BountyQuest.FormatName(target.ID), "Hyland Point", reward);
            BountyConversationRouter.SendOffer(Mod.Instance.Save, callerIndex, target, body, reward);
            Mod.Log.Info($"[BountyScheduler] Force-offer: '{callerName}' on '{target.ID}'.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"ForceOfferNow failed: {ex}");
        }
    }

    /// <summary>
    /// Audit M5 (2026-09-01): the day latch must not survive a slot switch —
    /// otherwise the newly loaded save gets no offer on its current day.
    /// Called from SaveStateGuard.TriggerFullReload.
    /// </summary>
    public static void ResetDayLatch()
    {
        _lastFiredDay = -1;
    }
}

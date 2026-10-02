using System;
using System.Collections.Generic;
using System.Threading;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Messaging;
using S1Mods.Shared;

using S1NPC = Il2CppScheduleOne.NPCs.NPC;

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
        if (day < 0 || !NetworkGuard.IsInMainScene) return;

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
        // Audit (2026-09-10): cooldowns must survive a game restart (was RAM-only) —
        // every other state transition persists; PersistCurrent is null-guarded and
        // exception-swallowed, so this is safe/reentrant on all caller paths.
        BountyPersistence.PersistCurrent();
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

// ---------------------------------------------------------------------------
// Merged from BountyCallSchedulerConstants.cs (2026-10-02) — shared constants.
// ---------------------------------------------------------------------------

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
}

// ---------------------------------------------------------------------------
// Merged from BountyDialogTemplates.cs (2026-10-02) — caller styles + copy.
// ---------------------------------------------------------------------------

/// <summary>
/// Static catalog of the three caller styles the bounty workflow uses.
/// Cold (business), Threatening (pressure), Desperate (vendetta).
/// Each style produces slightly different narrative tone in the message body,
/// but mechanically the caller pays the same and rolls the same target pool.
/// </summary>
public static class BountyDialogTemplates
{
    public const string StyleCold = "cold";
    public const string StyleThreatening = "threatening";
    public const string StyleDesperate = "desperate";

    public const string AcceptLabel = "Take the job";
    public const string AcceptText = "I'll do it.";
    public const string DeclineLabel = "Decline";
    public const string DeclineText = "Not interested.";
    public const string MoreInfoLabel = "Tell me more";
    public const string MoreInfoText = "Who is the target?";

    /// <summary>
    /// Build the body text for a bounty offer shown to the player.
    /// Caller is asked to provide caller name (anonymised) and the target npc
    /// display name; reward is shown explicitly because the worker-stage is
    /// post-cartel and the player has to weigh reward vs. police heat.
    /// NOTE (audit H2, 2026-09-01): the reward travels structurally to the
    /// accept handler — the body is display-only, never parsed back.
    /// </summary>
    public static string BuildColdOffer(string caller, string target, string region, float r)
        => $"[{caller}]: I have a job for you. Clean.\n\n" +
           $"Target <b>{target}</b> in {region}. {r:N0} on confirmation. No questions.";

    public static string BuildThreateningOffer(string caller, string target, string region, float r)
        => $"[{caller}]: Listen. {region} has been rough on me.\n\n" +
           $"Target <b>{target}</b>. I want them gone. {r:N0} when they're gone. Don't test me.";

    public static string BuildDesperateOffer(string caller, string target, string region, float r)
        => $"[{caller}]: Please. <b>{target}</b> took something from me. " +
           "I can't do this myself. They hang out in " + region + ".\n" +
           $"I have {r:N0} for you if you do it. Please.";

    /// <summary>
    /// Style rotation: pick one in stable order — caller index → style.
    /// Caller index 0 = cold, 1 = threatening, 2 = desperate, then wrap.
    /// </summary>
    public static string StyleForCallerIndex(int callerIndex)
    {
        return (callerIndex % 3) switch
        {
            0 => StyleCold,
            1 => StyleThreatening,
            _ => StyleDesperate
        };
    }

    /// <summary>
    /// Pick the message-builder for the chosen style.
    /// </summary>
    public static string BuildForStyle(string style, string caller, string target, string region, float r)
    {
        return style switch
        {
            StyleThreatening => BuildThreateningOffer(caller, target, region, r),
            StyleDesperate => BuildDesperateOffer(caller, target, region, r),
            _ => BuildColdOffer(caller, target, region, r)
        };
    }

    /// <summary>
    /// Names for the anonymous caller pool. These are read by the player as
    /// unknown persons — the underlying NPC identity is not exposed because the
    /// conversation lives on a generic NPC shell (Phase D does not yet bind
    /// callers to specific NPCs; the conversation is sent on the player's
    /// behalf via MessagingManager).
    /// </summary>
    public static readonly IReadOnlyList<string> CallerPool = new[]
    {
        "Ghost",        // index 0
        "Jackal",       // index 1
        "Magpie",       // index 2
        "Viper",        // index 3
        "Crow"          // index 4
    };

    public static string GetCallerName(int index) =>
        CallerPool[((index % CallerPool.Count) + CallerPool.Count) % CallerPool.Count];

    /// <summary>
    /// Reconstruct a caller index from the CallerId string "caller_ghost" → 0 etc.
    /// Shared by the expiry service, the receipt service and the death watchdog
    /// (previously duplicated three times). Returns -1 when the id is unknown.
    /// </summary>
    internal static int ResolveCallerIndex(string callerId)
    {
        if (string.IsNullOrEmpty(callerId)) return -1;
        if (!callerId.StartsWith("caller_")) return -1;
        string name = callerId.Substring("caller_".Length);
        for (int i = 0; i < CallerPool.Count; i++)
        {
            if (string.Equals(name, GetCallerName(i), StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }
}

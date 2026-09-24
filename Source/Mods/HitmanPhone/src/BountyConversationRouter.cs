// Il2Cppmscorlib is referenced with the "il2cpp" extern alias (Directory.Build.props) —
// C# requires extern alias declarations before any other element at file scope.
extern alias il2cpp;
using System;
using System.Collections.Generic;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Messaging;
using S1Mods.Shared;

#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1Messaging = Il2CppScheduleOne.Messaging;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
using S1Messaging = ScheduleOne.Messaging;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase D — Conversation Router.
///
/// Routes a generated bounty offer to a placeholder NPC and attaches
/// Accept / Decline / More-Info <see cref="Response"/>s. The player's response
/// either activates a new <see cref="BountyContract"/> (Accept) or applies a
/// cooldown to the caller (Decline).
///
/// NPC-base is abstract in S1API, so we cannot <c>new NPC(nativeNpc)</c>. Instead
/// we look up the live <c>S1API.Entities.NPC.All</c> wrapper for the target's id
/// and call <see cref="S1API.Entities.NPC.SendTextMessage"/> on it.
/// </summary>
public static class BountyConversationRouter
{
    /// <summary>
    /// Stable id used by the S1API Response wrapper. We just keep it as a unique key
    /// for the conversation so the Player can't trigger the wrong step.
    /// </summary>
    private static string NewContractId()
    {
        return $"hitman-{Guid.NewGuid():N}".Substring(0, 18);
    }

    /// <summary>
    /// Send the bounty offer to the target NPC via its S1API wrapper's conversation.
    /// The conversation surfaces in the phone's Messages app under the target's
    /// thread — the player sees the message from "Ghost"/"Jackal"/etc. even though
    /// it is hosted on the target's conversation.
    /// </summary>
    public static void SendOffer(BountySaveData save, int callerIndex, S1NPC target, string body, float rewardCash)
    {
        if (target == null) return;

        // Built up-front (pure, no side effects) so both the S1API host path and
        // the native fallback tier below can reuse the same response set.
        var responses = BuildResponses(save, callerIndex, target, body, rewardCash);
        var hostWrapper = GetCallerWrapper();
#if DEBUG
        // Beta 0.4.7f6 / S1API 3.2.1-beta.2: HitmanCallerNPC never instantiates
        // ("no framework data object", upstream issue #309), so its wrapper is
        // absent from NPC.All. DEBUG-only fallback chain; Release stays caller-only:
        //   tier 1: NPC.Get(target.ID) materializes the LAZY base-game wrapper
        //           (NPC.All only contains wrappers somebody already requested)
        //           and hosts the offer on the TARGET's thread — the design the
        //           class doc above describes.
        //   tier 2: native MSGConversation on the target, bypassing S1API wrappers.
        // The whole chain is guarded: a throwing tier must never bubble past the
        // scheduler's OnUpdate catch — fall through to the caller-only warn below.
        if (hostWrapper == null)
        {
            try
            {
                hostWrapper = S1API.Entities.NPC.Get(target.ID);
                if (hostWrapper != null)
                {
                    Mod.Log.Warn($"SendOffer[DEBUG]: caller wrapper missing — tier1 hosted on target wrapper '{target.ID}'.");
                }
                else if (SendOfferViaNativeConversation(target, body, responses, responseDelay: 1f))
                {
                    return;
                }
                else
                {
                    Mod.Log.Warn($"SendOffer[DEBUG]: all hosts failed. {DescribeWrapperIds()}");
                }
            }
            catch (Exception ex)
            {
                hostWrapper = null;
                Mod.Log.Warn($"SendOffer[DEBUG]: host fallback failed — {ex.Message}");
            }
        }
#endif
        if (hostWrapper == null)
        {
            Mod.Log.Warn($"SendOffer: no S1API wrapper found for HitmanCallerNPC ('{HitmanCallerNPC.NPC_ID}').");
            return;
        }

        try
        {
            hostWrapper.SendTextMessage(body, responses, responseDelay: 1f, network: true);
            Mod.Log.Info($"[BountyRouter] Sent offer regarding '{target.ID}' from caller {callerIndex}.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SendTextMessage threw for HitmanCallerNPC: {ex}");
        }
    }

    /// <summary>
    /// Find the live S1API.NPC wrapper for the virtual Hitman Caller NPC.
    /// </summary>
    private static S1API.Entities.NPC? GetCallerWrapper()
    {
        try
        {
            foreach (var w in S1API.Entities.NPC.All)
            {
                if (w == null) continue;
                if (w.ID == HitmanCallerNPC.NPC_ID) return w;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"GetCallerWrapper enumeration failed: {ex.Message}");
        }
        return null;
    }

#if DEBUG
    /// <summary>
    /// Diagnostics for the DEBUG host fallback: what S1API's wrapper registry
    /// actually holds. Logged on the total-failure path only — the previous
    /// fallback logged on success only, which is why the 12:39 test runs left no
    /// evidence of why they failed.
    /// </summary>
    private static string DescribeWrapperIds()
    {
        try
        {
            var ids = new List<string>();
            foreach (var w in S1API.Entities.NPC.All)
            {
                if (w == null) { ids.Add("<null>"); continue; }
                try { ids.Add(w.ID); } catch (Exception ex) { ids.Add($"<id-threw:{ex.GetType().Name}>"); }
            }
            return $"NPC.All count={ids.Count}: [{string.Join(", ", ids)}]";
        }
        catch (Exception ex)
        {
            return $"NPC.All enumeration failed: {ex.Message}";
        }
    }

    /// <summary>
    /// Tier-2 DEBUG fallback: deliver the offer through the target's NATIVE
    /// MSGConversation, bypassing the S1API wrapper layer entirely. Mirrors
    /// S1API's own NPC.SendTextMessage (ThirdParty/S1API/S1API/Entities/NPC.cs:3193)
    /// so sender type, notify and network flags behave identically.
    /// </summary>
    private static bool SendOfferViaNativeConversation(S1NPC target, string body, Response[] responses, float responseDelay)
    {
        try
        {
            var conv = target.MSGConversation;
            if (conv == null)
            {
                Mod.Log.Warn($"SendOfferNative: '{target.ID}' has no MSGConversation.");
                return false;
            }

            conv.SendMessage(
                new S1Messaging.Message(
                    body,
                    S1Messaging.Message.ESenderType.Other,
                    true,
                    UnityEngine.Random.Range(int.MinValue, int.MaxValue)),
                notify: true,
                network: true);

            if (responses != null && responses.Length > 0)
            {
                var nativeResponses = new il2cpp::Il2CppSystem.Collections.Generic.List<S1Messaging.Response>();
                foreach (var r in responses)
                {
                    // 2-arg ctor == explicit (null, false): callback=null, disableDefaultResponseBehaviour=false.
                    var native = new S1Messaging.Response(r.Text, r.Label);
                    // Same System.Action -> Il2CppSystem.Action assignment S1API's
                    // Response.OnTriggered setter performs
                    // (ThirdParty/S1API/S1API/Messaging/Response.cs:37).
                    native.callback = r.OnTriggered;
                    nativeResponses.Add(native);
                }
                conv.ShowResponses(nativeResponses, responseDelay, true);
            }

            Mod.Log.Warn($"SendOfferNative: sent via native conversation of '{target.ID}'.");
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"SendOfferNative failed for '{target.ID}': {ex}");
            return false;
        }
    }
#endif

    /// <summary>
    /// Wire three Response buttons: Accept, Decline, MoreInfo.
    /// Accept creates a BountyContract in Active state, Decline sets a cooldown
    /// for the caller, MoreInfo sends a follow-up message with extra context.
    /// Audit H2 (2026-09-01): the reward is passed STRUCTURALLY — parsing it
    /// back out of the message body broke on culture-formatted numbers
    /// (de-DE "18.000" parsed as 18) and the style templates carry no "$" at
    /// all, so the parser always fell back to $10k.
    /// </summary>
    private static Response[] BuildResponses(BountySaveData save, int callerIndex, S1NPC target, string body, float rewardCash)
    {
        var accept = new Response
        {
            Label = BountyDialogTemplates.AcceptLabel,
            Text = BountyDialogTemplates.AcceptText,
            OnTriggered = () => OnAccept(save, callerIndex, target, body, rewardCash)
        };

        var decline = new Response
        {
            Label = BountyDialogTemplates.DeclineLabel,
            Text = BountyDialogTemplates.DeclineText,
            OnTriggered = () => OnDecline(save, callerIndex)
        };

        var info = new Response
        {
            Label = BountyDialogTemplates.MoreInfoLabel,
            Text = BountyDialogTemplates.MoreInfoText,
            OnTriggered = () => OnMoreInfo(save, callerIndex, target, body, rewardCash)
        };

        return new[] { accept, decline, info };
    }

    /// <summary>
    /// IL2CPP-safe liveness check for the captured offer target. The Accept
    /// closure can fire after a scene change that destroyed the native NPC —
    /// a managed null check alone misses that and <c>target.ID</c> throws.
    /// Mirrors the Pointer/WasCollected pattern in
    /// <see cref="BountyReceiptService"/>.
    /// </summary>
    private static bool IsTargetAlive(S1NPC? target)
    {
#if (IL2CPPMELON)
        return target != null && target.Pointer != System.IntPtr.Zero && !target.WasCollected;
#else
        return target != null;
#endif
    }

    private static void OnAccept(BountySaveData capturedSave, int callerIndex, S1NPC target, string body, float rewardCash)
    {
        try
        {
            // Audit H3 (2026-09-01): the captured `save` object can be stale by
            // the time the player clicks — a full reload / slot switch replaces
            // Mod.Instance.Save, and a contract added to the old object was
            // silently lost by the next PersistCurrent. Resolve at click time
            // and void the offer when the save object changed.
            // v0.2.1 (Bug 3): voiding is no longer silent — the caller tells the
            // player the offer is off (SendOfferExpiredNotice).
            var save = Mod.Instance?.Save;
            if (save == null)
            {
                Mod.Log.Warn("OnAccept: no save state — offer voided.");
                SendOfferExpiredNotice(callerIndex);
                return;
            }
            if (!ReferenceEquals(save, capturedSave))
            {
                Mod.Log.Warn("OnAccept: save state changed since the offer (reload/slot switch) — stale offer voided.");
                SendOfferExpiredNotice(callerIndex);
                return;
            }
            if (!IsTargetAlive(target))
            {
                Mod.Log.Warn("OnAccept: bounty target is no longer alive (scene change?) — offer voided.");
                SendOfferExpiredNotice(callerIndex);
                return;
            }

            string id = NewContractId();
            int now = HitmanPhoneTime.CurrentDay();

            var contract = new BountyContract
            {
                Id = id,
                CallerId = $"caller_{BountyDialogTemplates.GetCallerName(callerIndex).ToLowerInvariant()}",
                CallerStyle = BountyDialogTemplates.StyleForCallerIndex(callerIndex),
                TargetNpcId = target.ID,
                TargetNpcInstanceId = target.GetInstanceID(),
                TargetNpcName = BountyQuest.FormatName(target.ID),
                RewardCash = rewardCash,
                OfferedAtDay = now,
                DeadlineDay = now + BountyCallSchedulerConstants.ContractDeadlineDays,
                Status = EBountyStatus.Active,
                RequiredDropId = null
            };

            save.Active.RemoveAll(c => c.Status == EBountyStatus.Offered && c.CallerId == contract.CallerId);
            save.Active.Add(contract);
            BountyPersistence.PersistCurrent(); // v0.1.3: contract must survive a game restart (was RAM-only)

            // Phase G: raise heat. Server-side authority flows through PlayerCrimeData's
            // RPC wrappers in S1API — exactly what the engine uses natively.
            BountyHeatService.OnBountyAccepted(contract);

            // Phase H: register the bounty with the player's journal. The quest
            // appears under the existing JournalApp via S1API auto-registration.
            BountyJournalBridge.RegisterBountyQuest(contract);

            try
            {
                var host = GetCallerWrapper();
#if DEBUG
                // Same missing-caller workaround as SendOffer (tier 1).
                if (host == null)
                    host = S1API.Entities.NPC.Get(target.ID);
#endif
                if (host != null)
                {
                    // Audit L7 (2026-09-01): derive from the deadline constant instead
                    // of a hardcoded "Three days." that can drift from the config.
                    host.SendTextMessage(
                        $"[{BountyDialogTemplates.GetCallerName(callerIndex)}]: Understood. " +
                        $"{BountyCallSchedulerConstants.ContractDeadlineDays} days. Don't disappoint me.",
                        network: true);
                }
            }
            catch
            {
                // best-effort confirmation message
            }
            Mod.Log.Info($"[BountyRouter] Accepted: id={id} target={target.ID} reward=${rewardCash} deadline={contract.DeadlineDay}.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnAccept exception: {ex}");
        }
    }

    private static void OnDecline(BountySaveData capturedSave, int callerIndex)
    {
        try
        {
            // Same stale-save guard as OnAccept (audit H3): CooldownCaller writes
            // Mod.Instance.Save, so a decline on a stale offer would cool down the
            // caller in the WRONG (new) save. Void it like the other responses.
            var save = Mod.Instance?.Save;
            if (save == null || !ReferenceEquals(save, capturedSave))
            {
                Mod.Log.Warn("OnDecline: save state changed since the offer — stale offer voided.");
                SendOfferExpiredNotice(callerIndex);
                return;
            }
            BountyCallScheduler.CooldownCaller(callerIndex, BountyCallSchedulerConstants.CallerCooldownDaysAfterDecline);
            Mod.Log.Info($"[BountyRouter] Caller {callerIndex} declined; cooldown {BountyCallSchedulerConstants.CallerCooldownDaysAfterDecline}d.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnDecline exception: {ex}");
        }
    }

    private static void OnMoreInfo(BountySaveData capturedSave, int callerIndex, S1NPC target, string body, float rewardCash)
    {
        try
        {
            // Same stale-save guard as OnAccept (audit H3): the re-attached
            // responses must bind the CURRENT save object, not the one from
            // offer time.
            var save = Mod.Instance?.Save;
            if (save == null || !ReferenceEquals(save, capturedSave))
            {
                Mod.Log.Warn("OnMoreInfo: save state changed since the offer — stale offer voided.");
                SendOfferExpiredNotice(callerIndex);
                return;
            }
            string caller = BountyDialogTemplates.GetCallerName(callerIndex);
            // v0.2.1 (Bug 2): show the ACTUAL rolled reward the offer carries
            // (rewardCash), not a static per-caller range — the old
            // The MoreInfo message shows the actual rolled reward, so it cannot
            // contradict the 200–500 amount already stored in the contract.
            // v0.2.1 (Bug 1): derive the window from the deadline constant
            // instead of a hardcoded "Three days window." (same drift L7
            // already fixed in OnAccept).
            string followUp =
                $"[{caller}]: Target is {BountyQuest.FormatName(target.ID)}. " +
                $"Pay {rewardCash:N0}, negotiable for clean work. " +
                $"Photo of the body, any dead-drop. " +
                $"{BountyCallSchedulerConstants.ContractDeadlineDays} days window.";
            var host = GetCallerWrapper();
#if DEBUG
            // Same missing-caller workaround as SendOffer (tier 1): keep the
            // follow-up on the same thread the offer was hosted on.
            if (host == null)
                host = S1API.Entities.NPC.Get(target.ID);
#endif
            if (host == null)
            {
                Mod.Log.Warn("OnMoreInfo: no caller wrapper; cannot send follow-up.");
                return;
            }
            // v0.1.5 BUGFIX: the follow-up used to be sent WITHOUT responses, which
            // replaced the offer's Accept/Decline buttons — after asking
            // "Who is the target?" the player could neither accept nor decline.
            // Re-attach the full response set so the decision stays available.
            host.SendTextMessage(followUp, BuildResponses(save, callerIndex, target, body, rewardCash),
                responseDelay: 1f, network: true);
            Mod.Log.Info($"[BountyRouter] More-info sent for '{target.ID}' (responses re-attached).");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnMoreInfo exception: {ex}");
        }
    }

    /// <summary>
    /// v0.2.1 (Bug 3): the stale-save guard used to void the offer SILENTLY —
    /// the Accept/Decline buttons stayed visible after a reload, but clicking
    /// them did nothing (only a log line). Now the caller sends a short
    /// follow-up telling the player the offer is off, so a dead button is
    /// never mistaken for a broken mod. Best-effort: if the wrapper is gone
    /// too (fresh scene), only the log line remains.
    ///
    /// Deliberately NO tier-1/tier-2 host fallback here (unlike SendOffer):
    /// by the time an offer is voided the conversation may be gone with the
    /// scene, and re-hosting the notice on the TARGET's thread would put a
    /// "deal is off" message into a conversation that never carried the
    /// offer in this session. Wrong-thread noise is worse than a skipped
    /// cosmetic notice — the player already sees the dead buttons.
    /// </summary>
    private static void SendOfferExpiredNotice(int callerIndex)
    {
        try
        {
            var host = GetCallerWrapper();
            if (host == null)
            {
                Mod.Log.Warn("SendOfferExpiredNotice: no caller wrapper — notice skipped.");
                return;
            }
            string caller = BountyDialogTemplates.GetCallerName(callerIndex);
            host.SendTextMessage(
                $"[{caller}]: Forget it. The deal is off.",
                network: true);
            Mod.Log.Info($"[BountyRouter] Offer-expired notice sent (caller {callerIndex}).");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"SendOfferExpiredNotice failed: {ex.Message}");
        }
    }
}

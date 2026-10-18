using System;
using System.Threading;
using HitmanPhone.Persistence;
using MelonLoader;
using S1Mods.Shared;

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase I — Auto-Expire.
///
/// Iterates over every <see cref="BountyContract"/> with
/// <see cref="EBountyStatus.Active"/> once per save-day. Contracts whose
/// <see cref="BountyContract.DeadlineDay"/> is in the past are flipped to
/// <see cref="EBountyStatus.Expired"/>, the corresponding Journal quest is
/// marked Expired, the heat is escalated to NonLethal, and the caller is
/// placed on cooldown for the standard decline period (3 days).
///
/// Skill Rule #4: 0-Allocation Polling. We throttle to one check per save day
/// to keep this cheap — the engine itself doesn't need to iterate a contract
/// list every frame.
/// </summary>
public static class BountyExpiryService
{
    private static long _lastTickRealtime;
    private const long TickThrottleMs = 2000;
    /// <summary>Warn the player once when the drop window has this many minutes left.</summary>
    private const int DropWarningThresholdMinutes = 30;
    private static int _expirationsDispatched;
    private static int _lastExpiredContracts;

    public static int ExpirationsDispatched => Volatile.Read(ref _expirationsDispatched);
    public static int LastExpiredContracts => _lastExpiredContracts;

    /// <summary>
    /// Called from <see cref="Mod.OnUpdate"/>. Throttled to once per save-day
    /// (we trigger only when <see cref="HitmanPhoneTime.CurrentDay"/> changes).
    /// </summary>
    public static void Tick()
    {
        var save = Mod.Instance?.Save;
        if (save == null) return;

        // Budget/dead-drop update: the post-drop window runs on MINUTE
        // precision, so the old once-per-day latch is gone — a 2s real-time
        // throttle keeps the ≤-few-contract loop negligible ("0-Allocation
        // Polling" rule still holds: no allocations in the steady state).
        long nowMs = Environment.TickCount64;
        if (nowMs - _lastTickRealtime < TickThrottleMs) return;
        _lastTickRealtime = nowMs;

        int day = HitmanPhoneTime.CurrentDay();
        if (day < 0 || !NetworkGuard.IsInMainScene) return;

        int batched = 0;
        // Audit M6 (2026-09-01): expired contracts now LEAVE Active (moved to
        // History, matching the completion path) — backwards iteration because
        // RemoveAt mutates the list mid-loop. Until this fix Active grew forever.
        for (int i = save.Active.Count - 1; i >= 0; i--)
        {
            var c = save.Active[i];
            if (c.Status != EBountyStatus.Active) continue;
            if (c.DeadlineDay <= 0) continue;

            // Two-phase deadlines (budget/dead-drop update):
            //   1. Execution phase — the polaroid is not in confirmed player
            //      possession yet: the classic day-based DeadlineDay applies
            //      (kill + delivery).
            //   2. Drop phase — the first confirmed receipt of the polaroid
            //      started the short budget-scaled drop window (minute
            //      precision); it replaces the execution deadline from that
            //      moment on.
            if (c.DropDueStarted)
            {
                long remaining = HitmanPhoneTime.MinutesUntil(c.DropDueMinSum);
                if (remaining > 0)
                {
                    if (!c.DropWarningSent && remaining <= DropWarningThresholdMinutes)
                    {
                        c.DropWarningSent = true;
                        BountyPersistence.PersistCurrent();
                        WarnDropWindow(c, (int)remaining);
                    }
                    continue;
                }
                // Drop window missed → same failure transition as a missed
                // execution deadline (below).
            }
            else
            {
                // Boundary: DeadlineDay = OfferedAtDay + ContractDeadlineDays and the quest
                // journal promises exactly that many days ("Time limit: {Deadline-Offered}").
                // Expire when the deadline day is REACHED (>=), not the day after (>).
                if (day < c.DeadlineDay) continue;
            }
            {
                c.Status = EBountyStatus.Expired;
                save.Active.RemoveAt(i);
                save.History.Add(c);
                batched++;
                BountyPersistence.PersistCurrent(); // v0.1.3: expiry must survive a game restart

                // Heat escalation: matches OnBountyFailed path. Standard 3d caller
                // cooldown, not the 14d player-death variant.
                try { BountyHeatService.OnBountyFailed("expired"); }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"OnBountyFailed during expiry: {ex.Message}");
                }
                int callerIdx = ResolveCallerIndex(c.CallerId);
                if (callerIdx >= 0)
                {
                    BountyCallScheduler.CooldownCaller(callerIdx,
                        BountyCallSchedulerConstants.CallerCooldownDaysAfterDecline);
                }

                // Journal: mark Expired.
                try { BountyJournalBridge.FailQuest(c.Id, "expired"); }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"Journal FailQuest during expiry: {ex.Message}");
                }

                BountyService.RemovePolaroidFromInventory(c, "expired");
                BountyService.RemovePolaroidFromDeadDrops(c, "expired");
            }
        }

        _lastExpiredContracts = batched;
        if (batched > 0)
        {
            Volatile.Write(ref _expirationsDispatched,
                Volatile.Read(ref _expirationsDispatched) + batched);
            Mod.Log.Info($"[ExpiryService] Day {day}: {batched} bounty contract(s) expired.");
        }
    }

    /// <summary>
    /// One-shot caller SMS shortly before the assigned drop window closes.
    /// A warning is never worth breaking the expiry transition (all wrapped).
    /// </summary>
    private static void WarnDropWindow(BountyContract c, int remainingMinutes)
    {
        try
        {
            BountyConversationRouter.SendDropWindowWarning(
                ResolveCallerIndex(c.CallerId), remainingMinutes, c.AssignedDropName);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Drop-window warning failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Reconstruct caller index from "caller_ghost" → 0 etc.
    /// </summary>
    private static int ResolveCallerIndex(string callerId)
    {
        if (string.IsNullOrEmpty(callerId)) return -1;
        if (!callerId.StartsWith("caller_")) return -1;
        string name = callerId.Substring("caller_".Length);
        for (int i = 0; i < BountyDialogTemplates.CallerPool.Count; i++)
        {
            if (string.Equals(name, BountyDialogTemplates.GetCallerName(i),
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Audit M5 (2026-09-01): the day latch must not survive a slot switch —
    /// otherwise the newly loaded save skips its expiry check on day one.
    /// Called from SaveStateGuard.TriggerFullReload.
    /// </summary>
    public static void ResetDayLatch()
    {
        _lastTickRealtime = 0;
    }
}

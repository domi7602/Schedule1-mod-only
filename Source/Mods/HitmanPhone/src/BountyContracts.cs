// ---------------------------------------------------------------------------
// Contracts/BountyContracts.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Contracts/BountyExpiryService.cs     -> section Contracts/BountyExpiryService.cs
//   Contracts/BountyHeatService.cs       -> section Contracts/BountyHeatService.cs
//   Contracts/PlayerDeathWatchdog.cs     -> section Contracts/PlayerDeathWatchdog.cs
//
// ---------------------------------------------------------------------------

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Contracts/BountyExpiryService.cs ====
    using System;
    using System.Threading;
    using HitmanPhone.Persistence;
    using MelonLoader;
    using S1Mods.Shared;


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

            if (!SaveStateGuard.IsReady || !HitmanPhoneTime.TryGetSnapshot(out int day, out _, out long now)) return;

            int batched = 0;
            // Audit M6 (2026-09-01): expired contracts now LEAVE Active (moved to
            // History, matching the completion path) — backwards iteration because
            // RemoveAt mutates the list mid-loop. Until this fix Active grew forever.
            for (int i = save.Active.Count - 1; i >= 0; i--)
            {
                var c = save.Active[i];
                if (c.Status != EBountyStatus.Active || c.RewardPaymentUncertain) continue;
                if (BountyDeadlinePolicy.IsEligible(c, now))
                {
                    long remaining = BountyDeadlinePolicy.DueMinSum(c) - now;
                    if (c.DropDueStarted && !c.DropWarningSent && remaining > 0 && remaining <= DropWarningThresholdMinutes)
                    {
                        c.DropWarningSent = true;
                        BountyPersistence.PersistCurrent();
                        WarnDropWindow(c, (int)remaining);
                    }
                    continue;
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
    // ==== end HitmanPhone section: Contracts/BountyExpiryService.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Contracts/BountyHeatService.cs ====
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using HitmanPhone.Persistence;
    using MelonLoader;
    using S1API.Law;
    using S1API.Entities;
    using S1Mods.Shared;

#if (IL2CPPMELON)
    using S1PlayerOfficer = Il2CppScheduleOne.Police.PoliceOfficer;
#elif MONOMELON
    using S1PlayerOfficer = ScheduleOne.Police.PoliceOfficer;
#endif


    /// <summary>
    /// Phase G — Heat orchestration.
    ///
    /// We bridge the Bounty workflow with the existing Police system via S1API's
    /// <see cref="PlayerCrimeData"/>. We never modify the engine directly — we
    /// raise or lower PursuitLevel through the S1API wrapper, which routes through
    /// the same RPC the engine uses internally.
    ///
    /// Effects timeline:
    ///   • Pre-Pursuit (Accept Bounty → set PursuitLevel=Investigating):
    ///     Player is now "wanted"; officer VisionEvents will escalate naturally.
    ///   • On-Kill (BountyService.OnNpcDied matched):
    ///     Set the player's pursuit immediately to Lethal, as requested.
    ///   • On-Timeout / Player-Death:
    ///     Set PursuitLevel=NonLethal for <see cref="FailPursuitDays"/> in-game days.
    ///   • On-Cancel (Player eats the offer decline cooldown):
    ///     Drop pursuit back to None.
    ///
    /// Skill reference: <c>schedule1-game-systems/references/12-Heat-Pursuit-Law.md</c>.
    /// </summary>
    public static class BountyHeatService
    {
        /// <summary>Days the player stays at NonLethal pursuit after a failed bounty.</summary>
        public const int FailPursuitDays = 2;

        private const string GraceElapsedKey = "hitman_grace_until_";

        private static int _pursuitSetActive;
        private static int _pursuitSetLethal;
        private static int _pursuitSetFail;
        private static int _pursuitCleared;

        /// <summary>
        /// Live count of Active contracts, computed from the save on demand.
        /// Audit M9 (2026-09-01): the old increment/decrement counter drifted
        /// negative (early returns before increment, decrements for contracts from
        /// earlier sessions, slot-switch resets). The save list is the only truth.
        /// </summary>
        public static int ActiveBounties
        {
            get
            {
                var save = Mod.Instance?.Save;
                if (save == null) return 0;
                int n = 0;
                for (int i = 0; i < save.Active.Count; i++)
                    if (save.Active[i].Status == EBountyStatus.Active) n++;
                return n;
            }
        }
        public static int PursuitSetActive => Volatile.Read(ref _pursuitSetActive);
        public static int PursuitSetLethal => Volatile.Read(ref _pursuitSetLethal);
        public static int PursuitSetFail => Volatile.Read(ref _pursuitSetFail);
        public static int PursuitCleared => Volatile.Read(ref _pursuitCleared);

        /// <summary>
        /// Reset internal runtime counters when a save slot switch is detected.
        /// Public for testability and so SaveStateGuard can wipe stale counters
        /// between slot switches.
        /// </summary>
        public static void ResetActiveCounters()
        {
            Volatile.Write(ref _pursuitSetActive, 0);
            Volatile.Write(ref _pursuitSetLethal, 0);
            Volatile.Write(ref _pursuitSetFail, 0);
            Volatile.Write(ref _pursuitCleared, 0);
        }

        /// <summary>
        /// Audit M1: old saves carry realtimeSinceStartup-based grace keys in
        /// HeatState. They are never read again — purge them so the save file
        /// shrinks back. Called from SaveStateGuard.OnLoadComplete.
        /// </summary>
        public static void PurgePersistedHeatKeys()
        {
            var save = Mod.Instance?.Save;
            if (save?.HeatState == null || save.HeatState.Count == 0) return;
            List<string> doomed = new();
            foreach (var kvp in save.HeatState)
            {
                if (kvp.Key.StartsWith(GraceElapsedKey, StringComparison.Ordinal)) doomed.Add(kvp.Key);
            }
            for (int i = 0; i < doomed.Count; i++) save.HeatState.Remove(doomed[i]);
            if (doomed.Count > 0)
            {
                Mod.Log.Info($"[Heat] Purged {doomed.Count} obsolete persisted grace key(s) from the save.");
            }
        }

        /// <summary>
        /// Called when the player accepts a bounty contract. We mark the active bounties
        /// count and push the player to <see cref="PursuitLevel.Investigating"/>.
        /// Idempotent across multiple concurrent bounties — we don't escalate beyond
        /// Investigating unless the player keeps failing.
        /// </summary>
        public static void OnBountyAccepted(BountyContract contract)
        {
            var local = GetLocalPlayer();
            if (local == null) return;
            Interlocked.Increment(ref _pursuitSetActive);
            try
            {
                bool awaitingDrop = false;
                var save = Mod.Instance?.Save;
                if (save != null)
                    foreach (var active in save.Active)
                        if (active.Status == EBountyStatus.Active && active.AwaitingDrop) awaitingDrop = true;
                var level = (PursuitLevel)BountyRuntimeRules.AcceptedPursuit(
                    (int)local.CrimeData.CurrentPursuitLevel, awaitingDrop);
                local.CrimeData.SetPursuitLevel(level);
                Mod.Log.Info($"[Heat] Accepted bounty #{contract.Id} on '{contract.TargetNpcId}'; pursuit retained/escalated to {level}.");
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"OnBountyAccepted failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Called by the BountyService when a kill matched a bounty. The requested
        /// behavior is immediate maximum pursuit: the player's pursuit becomes
        /// <see cref="PursuitLevel.Lethal"/> at the moment of the matched kill.
        /// </summary>
        public static void OnBountyKilled(BountyContract contract)
        {
            try
            {
                var local = GetLocalPlayer();
                if (local == null) return;

                local.CrimeData.SetPursuitLevel(PursuitLevel.Lethal);
                Interlocked.Increment(ref _pursuitSetLethal);
                Mod.Log.Info($"[Heat] Bounty #{contract.Id} matched on kill; PursuitLevel set to Lethal.");
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"OnBountyKilled failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when a bounty expires (timeout), or the player dies during a bounty,
        /// or the offer was declined but pursuit had already been raised.
        /// Escalates to <see cref="PursuitLevel.NonLethal"/>.
        /// </summary>
        public static void OnBountyFailed(string reason)
        {
            try
            {
                var local = GetLocalPlayer();
                if (local == null) return;
                // Only escalate if the player still has at least one live bounty
                if (ActiveBounties > 0)
                {
                    // Player has a parallel bounty still in flight — keep at Investigating.
                    return;
                }
                Interlocked.Increment(ref _pursuitSetFail);
                local.CrimeData.SetPursuitLevel(PursuitLevel.NonLethal);
                Mod.Log.Info($"[Heat] Bounty failed ({reason}); pursuit escalated to NonLethal.");
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"OnBountyFailed failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Called after the polaroid is deposited and the payout has cleared. The
        /// bounty is now Completed; we drop the pursuit level all the way to None so
        /// the player isn't permanently wanted.
        /// </summary>
        public static void OnBountyCompleted(BountyContract contract)
        {
            try
            {
                var local = GetLocalPlayer();
                if (local == null) return;
                if (ActiveBounties <= 0)
                {
                    Interlocked.Increment(ref _pursuitCleared);
                    local.CrimeData.SetPursuitLevel(PursuitLevel.None);
                    Mod.Log.Info($"[Heat] Bounty #{contract.Id} complete; pursuit dropped to None.");
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"OnBountyCompleted failed: {ex.Message}");
            }
        }

        private static Player? GetLocalPlayer()
        {
            try { return Player.Local; }
            catch (Exception ex)
            {
                Mod.Log.Warn($"GetLocalPlayer failed: {ex.Message}");
                return null;
            }
        }
    }

    // ==== end HitmanPhone section: Contracts/BountyHeatService.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Contracts/PlayerDeathWatchdog.cs ====
    using System;
    using System.Threading;
    using HitmanPhone.Persistence;
    using MelonLoader;
    using S1API.Entities;
    using S1Mods.Shared;


    /// <summary>
    /// Phase H — Player-Death Watchdog.
    ///
    /// Polls <see cref="Player.IsDead"/> once per second and, on the rising edge
    /// (alive → dead transition), iterates over every active bounty contract and
    /// marks it Failed. This prevents the player from being able to keep a contract
    /// active indefinitely by repeatedly falling unconscious without our knowledge.
    ///
    /// Skill Rule #4: 0-Allocation Polling. We <c>Interlocked.Increment</c> a counter
    /// and only run the full check every ~60 frames. No LINQ, no foreach on hot path.
    /// </summary>
    public static class PlayerDeathWatchdog
    {
        private static long _nextDeathPoll;
        private static bool? _wasAlive;

        private static int _deathsDetected;
        private static int _failuresDispatched;

        public static int DeathsDetected => Volatile.Read(ref _deathsDetected);
        public static int FailuresDispatched => Volatile.Read(ref _failuresDispatched);

        /// <summary>
        /// Called from <see cref="Mod.OnUpdate"/>. Throttles to one full check per second.
        /// </summary>
        public static void Tick()
        {
            if (!SaveStateGuard.IsReady) return;
            long now = Environment.TickCount64;
            if (now < _nextDeathPoll) return;
            _nextDeathPoll = now + 1000;

            Player? local = null;
            try { local = Player.Local; }
            catch { return; }
            if (local == null) return;

            bool alive = false;
            try { alive = !local.IsDead; }
            catch
            {
                // S1API wrapper may be in transition during scene load; skip this tick.
                return;
            }

            bool? wasAlive = _wasAlive;
            _wasAlive = alive;

            // Rising-edge detection: `wasAlive=true, alive=false` is the transition
            // (alive → dead) — the player just died.
            if (wasAlive == true && !alive)
            {
                Interlocked.Increment(ref _deathsDetected);
                DispatchAllActiveToFailed("player_died");
            }
        }

        /// <summary>
        /// Iterate every <see cref="EBountyStatus.Active"/> contract and mark it Failed.
        /// Each one triggers the heat escalation path and the caller cooldowns.
        /// </summary>
        public static void DispatchAllActiveToFailed(string reason)
        {
            if (!SaveStateGuard.IsReady) return;
            var save = Mod.Instance?.Save;
            if (save == null) return;
            int dispatched = 0;
            // Audit M6 (2026-09-01): forfeited contracts leave Active (moved to
            // History, matching the completion/expiry paths) — backwards iteration
            // because RemoveAt mutates the list mid-loop.
            for (int i = save.Active.Count - 1; i >= 0; i--)
            {
                var c = save.Active[i];
                if (c.Status != EBountyStatus.Active) continue;
                if (c.RewardPaymentUncertain) continue; // retain unresolved cash claim/photo for manual review
                c.Status = EBountyStatus.Forfeited; // distinguishes from timeout / cancelled
                save.Active.RemoveAt(i);
                save.History.Add(c);
                Interlocked.Increment(ref _failuresDispatched);
                BountyPersistence.PersistCurrent(); // v0.1.3: forfeit must survive a game restart

                // Heat: escalate. Caller: 14-day cooldown.
                try
                {
                    BountyHeatService.OnBountyFailed(reason);
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"OnBountyFailed during death dispatch: {ex.Message}");
                }
                int callerIdx = ResolveCallerIndex(c.CallerId);
                if (callerIdx >= 0)
                {
                    BountyCallScheduler.CooldownCaller(callerIdx,
                        BountyCallSchedulerConstants.CallerCooldownDaysAfterPlayerDeath);
                }

                // Journal: mark forfeited. "forfeited" is treated as Failed by the bridge.
                try { BountyJournalBridge.FailQuest(c.Id, reason: "forfeited"); }
                catch { /* best-effort */ }

                BountyService.RemovePolaroidFromInventory(c, "forfeited");
                BountyService.RemovePolaroidFromDeadDrops(c, "forfeited");

                dispatched++;
            }
            if (dispatched > 0)
            {
                Mod.Log.Info($"[DeathWatchdog] Player died with {dispatched} active bounty " +
                             $"contract(s); marked Forfeited with 14d caller cooldown.");
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
        /// Reset the watchdog (call this after save-load to avoid spurious death triggers
        /// if the player respawned).
        /// </summary>
        public static void Reset()
        {
            _wasAlive = null; // first readable observation establishes the loaded baseline
            _nextDeathPoll = 0;
        }
    }
    // ==== end HitmanPhone section: Contracts/PlayerDeathWatchdog.cs ====
}

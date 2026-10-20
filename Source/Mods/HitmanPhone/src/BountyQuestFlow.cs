// ---------------------------------------------------------------------------
// Contracts/BountyQuestFlow.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Contracts/BountyQuest.cs             -> section Contracts/BountyQuest.cs
//   Contracts/BountyService.cs           -> section Contracts/BountyService.cs
//
// ---------------------------------------------------------------------------

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Contracts/BountyQuest.cs ====
    using System;
    using HitmanPhone.Persistence;
    using S1API.Quests;
    using S1API.Quests.Constants;
    using UnityEngine;

#if (IL2CPPMELON)
    using S1Quest = Il2CppScheduleOne.Quests.Quest;
#elif MONOMELON
    using S1Quest = ScheduleOne.Quests.Quest;
#endif


    /// <summary>
    /// Phase H — S1API Quest subclass for bounty contracts.
    ///
    /// One quest is created per accepted bounty. Quests use the existing in-game
    /// <c>JournalApp</c> via S1API auto-registration; the player sees a new quest
    /// entry each time they accept a caller's offer.
    ///
    /// BUGFIX v0.2.7 (2026-09-14): Restore restored BountyQuest wrappers render the
    /// vanilla UI snapshot "Hitman Contract" instead of "Hitman Contract: <NPC>"
    /// for quests whose contracts were already persisted when the game was last
    /// saved. Root cause: the vanilla `S1Quest._title` privateField is the UI-
    /// rendering source (set in `Quest.Create` ctor = our `Title` property) and
    /// is never re-snapshotted after the S1API constructor, so when `InitContractId`
    /// arrives later (post-rebind) the UI cache stays on the empty-state snapshot.
    /// v0.2.6's `TryRefreshDisplayTitle` reflection-write to `s1q.title` updates
    /// the field but does not fire the internal `_onTitleChanged` Unity event.
    /// Fix: hook `GameLifecycle.OnSaveStart` and, for every BountyQuest wrapper
    /// whose Title property returns a non-generic value, call `InitializeQuest(
    /// title, desc, entries, guid)` which re-runs the vanilla UI-snapshot path
    /// the ctor used. `BountyJournalBridge.OnPostLoadComplete` syncs the same for
    /// restored wrappers right after the rebind pass.
    ///
    /// Skill reference: <c>schedule1-game-systems/references/03-Dialogue-Quest.md</c>.
    /// </summary>
    public class BountyQuest : Quest
    {
        private string? _contractId;

        public void InitContractId(string id)
        {
            _contractId = id;
            // v0.2.7: re-snapshot the vanilla UI title now that we KNOW the contract.
            // Earlier returns the empty-state "Hitman Contract" stayed on the journal
            // because the setter on the internal field never fired the UI event.
            SyncDisplayTitle();
        }

        /// <summary>Refresh S1Quest.title, description, entry title, and HUD UI.</summary>
        public void SyncDisplayTitle()
        {
            try
            {
                string newTitle = Title;
                string newDesc = Description;
                var s1q = typeof(Quest).GetField("S1Quest",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic)?.GetValue(this) as S1Quest;
                if (s1q == null || s1q.Pointer == System.IntPtr.Zero) return;

                s1q.title = newTitle;
                s1q.Description = newDesc;

                var c = GetContract();
                if (c != null)
                {
                    string targetName = FormatName(c.TargetNpcName ?? c.TargetNpcId);
                    string entryTitle = $"Eliminate {targetName} and collect the polaroid";

                    if (QuestEntries.Count > 0 && QuestEntries[0] != null)
                    {
                        QuestEntries[0].Title = entryTitle;
                    }
                }

                s1q.UpdateHUDUI();
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"[BountyQuest] SyncDisplayTitle failed: {ex.Message}");
            }
        }

        private BountyContract? GetContract()
        {
            if (Mod.Instance.Save == null) return null;

            string? targetId = _contractId ?? BountyJournalBridge.CreatingContractId;
            if (string.IsNullOrEmpty(targetId)) return null;

            for (int i = 0; i < Mod.Instance.Save.Active.Count; i++)
            {
                if (Mod.Instance.Save.Active[i].Id == targetId)
                    return Mod.Instance.Save.Active[i];
            }
            return null;
        }

        public static string FormatName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            var words = id.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            return string.Join(" ", words);
        }

        protected override string Title
        {
            get
            {
                var c = GetContract();
                if (c == null) return "Hitman Contract";
                return $"Hitman Contract: {FormatName(c.TargetNpcName ?? c.TargetNpcId)}";
            }
        }

        protected override string Description
        {
            get
            {
                var c = GetContract();
                if (c == null) return "Eliminate the target.";
                // Budget/dead-drop update: the drop is fixed per contract and the
                // deadline is two-phase (execution window, then the short post-photo
                // drop window). Both are shown in in-game time terms.
                string dropLine = string.IsNullOrEmpty(c.AssignedDropName)
                    ? "Dead drop: any"
                    : $"Dead drop: {c.AssignedDropName}";
                if (c.DropWindowMinutes > 0 && !c.DropDueStarted)
                    dropLine += $" - deliver within {c.DropWindowMinutes / 60}h after the photo reaches you";
                string phaseLine;
                if (c.DropDueStarted)
                {
                    long left = HitmanPhoneTime.MinutesUntil(c.DropDueMinSum);
                    phaseLine = $"Drop window: {left} min left (in-game)";
                }
                else
                {
                    int days = c.DeadlineDay - c.OfferedAtDay;
                    if (days < 0) days = 0;
                    phaseLine = $"Time limit: {days} days (in-game)";
                }
                return $"Caller: {c.CallerId.Replace("caller_", "").ToUpper()}\n" +
                       $"Reward: ${c.RewardCash:N0}\n" +
                       dropLine + "\n" +
                       phaseLine;
            }
        }

        protected override void OnCreated()
        {
            base.OnCreated();
            var c = GetContract();
            string targetName = c != null ? FormatName(c.TargetNpcName ?? c.TargetNpcId) : "the target";
            AddEntry($"Eliminate {targetName} and collect the polaroid");
            AddEntry("Drop the polaroid at the assigned dead drop");
        }

        /// <summary>
        /// Override icon to use our Polaroid sprite. If the icon can't be loaded,
        /// we leave QuestIcon = null and the S1API base falls back to the contacts icon.
        /// </summary>
        protected override Sprite? QuestIcon
        {
            get
            {
                try
                {
                    // The polaroid sprite is loaded once at registry time. We re-use the
                    // cached sprite rather than hit disk again per quest.
                    return HitmanPhone.Items.BountyEvidenceItemRegistry.IconSprite;
                }
                catch
                {
                    return null;
                }
            }
        }

        protected override bool AutoBegin => true;

        /// <summary>
        /// Push a state change to the wrapper. This is the runtime-equivalent of the
        /// in-built <c>Complete()</c> / <c>Fail()</c>/ <c>Expire()</c> methods but routed
        /// through the wrapper so the journal re-renders consistently.
        /// </summary>
        public void SetJournalState(QuestState state)
        {
            try
            {
                switch (state)
                {
                    case QuestState.Completed: Complete(); break;
                    case QuestState.Failed: Fail(); break;
                    case QuestState.Expired: Expire(); break;
                    case QuestState.Cancelled: Cancel(); break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"BountyQuest.SetJournalState({state}) failed: {ex.Message}");
            }
        }
    }
    // ==== end HitmanPhone section: Contracts/BountyQuest.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Contracts/BountyService.cs ====
    using System;
    using System.Threading;
    using HitmanPhone.Items;
    using HitmanPhone.Persistence;
    using MelonLoader;
    using S1API.Entities;
    using S1Mods.Shared;
    using UnityEngine;

#if (IL2CPPMELON)
    using S1NPC = Il2CppScheduleOne.NPCs.NPC;
    using S1PlayerInventory = Il2CppScheduleOne.PlayerScripts.PlayerInventory;
    using S1DeadDrop = Il2CppScheduleOne.Economy.DeadDrop;
#elif MONOMELON
    using S1NPC = ScheduleOne.NPCs.NPC;
    using S1PlayerInventory = ScheduleOne.PlayerScripts.PlayerInventory;
    using S1DeadDrop = ScheduleOne.Economy.DeadDrop;
#endif


    /// <summary>
    /// Phase C — Kill-Detection &amp; Polaroid Spawn.
    ///
    /// On every <see cref="S1NPC"/>.OnDie() (Harmony-Postfix), we check whether any
    /// active bounty matches the dying NPC. If yes:
    ///   • Spawn a polaroid instance pinned to that NPC's instance id
    ///   • Hand it to the local player's <see cref="S1PlayerInventory"/> via the
    ///     player's MonoBehaviour component.
    ///
    /// Pitfalls (Spec §3):
    ///   • OnDie may fire more than once (network re-sync) — guarded by Contract.State.
    ///   • OnDie may never fire if the NPC was destroyed via engine reset instead of damage.
    /// </summary>
    public static class BountyService
    {
        private static int _deathsObserved;
        private static int _matchesFound;
        private static int _polaroidsSpawned;
        private static int _earlyOuts;
        private static float _lastSpawnFailWarn = -999f;
        private static readonly System.Collections.Generic.HashSet<string> _recentlyHandledNpcIds = new();
        private static float _lastClearTime = -999f;

        public static int DeathsObserved => Volatile.Read(ref _deathsObserved);
        public static int MatchesFound => Volatile.Read(ref _matchesFound);
        public static int PolaroidsSpawned => Volatile.Read(ref _polaroidsSpawned);
        public static int EarlyOuts => Volatile.Read(ref _earlyOuts);

        public static void OnNpcDied(S1NPC npc, bool fatal = true)
        {
            Interlocked.Increment(ref _deathsObserved);

            if (npc == null) return;
            if (!HitmanPhone.SaveStateGuard.IsReady) return;
            if (Mod.Instance == null || Mod.Instance.Save == null) return;

            string npcId = npc.ID ?? string.Empty;

            // HP-09: a fatal death of an already-known bounty target upgrades the
            // recorded knockout to a confirmed death, independent of photo delivery (the
            // contract may already be completed). Runs BEFORE the 5s debounce so a kill
            // shortly after the KO is not swallowed. No second photo, no new drop
            // window, no reward — it only corrects the tracker record.
            if (fatal && !string.IsNullOrEmpty(npcId))
            {
                TryUpgradeKillToDeath(npcId);
            }

            if (Time.time - _lastClearTime > 5f)
            {
                _recentlyHandledNpcIds.Clear();
                _lastClearTime = Time.time;
            }

            if (!string.IsNullOrEmpty(npcId) && _recentlyHandledNpcIds.Contains(npcId))
            {
                return;
            }

            var save = Mod.Instance.Save;

            // HP-13: recognition needs a valid in-game time snapshot; an invalid read
            // must never be treated as minute zero.
            if (!HitmanPhoneTime.TryGetSnapshot(out _, out _, out long now))
            {
                Mod.Log.Warn("[Bounty] in-game time unavailable — elimination not recognized this tick.");
                return;
            }

            // Find an Active contract whose TargetNpcId matches this NPC's ID, OR whose
            // already-cached TargetNpcInstanceId matches the runtime InstanceID.
            // A contract whose elimination is already recognized (AwaitingDrop) is never
            // recognized a second time — re-recognizing it after a load is exactly what
            // produced duplicate photos and a repeated Lethal escalation (HP-05).
            BountyContract? match = null;
            for (int i = 0; i < save.Active.Count; i++)
            {
                var c = save.Active[i];
                if (c.Status != EBountyStatus.Active) continue;
                if (c.EvidenceSpawned || c.AwaitingDrop) continue;
                // HP-13: a target eliminated after its execution deadline must not be
                // recognized — it must not spawn a photo nor open a fresh drop window.
                if (!BountyDeadlinePolicy.IsEligible(c, now)) continue;
                if (!string.IsNullOrEmpty(c.TargetNpcId) && c.TargetNpcId == npc.ID)
                {
                    match = c;
                    // Keep the cached instance id session-current: Unity re-rolls
                    // InstanceIDs on every game start, so a value persisted from a
                    // previous session is dead weight for the dead-drop match.
                    c.TargetNpcInstanceId = npc.GetInstanceID();
                    break;
                }
                if (c.TargetNpcInstanceId != 0 && c.TargetNpcInstanceId == npc.GetInstanceID())
                {
                    match = c; break;
                }
            }

            if (match == null)
            {
                Interlocked.Increment(ref _earlyOuts);
                // v0.1.3: this used to be a silent exit — a kill with no waiting
                // contract left no trace in the log and was undiagnosable.
                // Debug level: every unrelated NPC death in a busy city would spam Info.
                Mod.Log.Debug($"[Bounty] NPC '{npc.ID}' died — no active contract targets it " +
                             $"(active={save.Active.Count}); no polaroid spawned.");
                return;
            }

            Interlocked.Increment(ref _matchesFound);

            // Recognition: the elimination is a FACT from here on, independent of whether
            // the polaroid physically fits right now (HP-04). Persist the recognized
            // state, record the takedown and start quest/heat progress exactly once —
            // the old code returned before all of this whenever the inventory was full.
            match.AwaitingDrop = true;
            RecordKillEvent(match, npcId, fatal);
            BountyJournalBridge.AdvanceToDeadDrop(match.Id);
            BountyHeatService.OnBountyKilled(match);

            // Delivery is a separate step. If the photo does not fit, leave a pending
            // delivery that is serviced later WITHOUT a second attack and WITHOUT a
            // second heat escalation (HP-04/HP-05).
            if (!TryDeliverEvidence(match, npcId))
            {
                match.PendingEvidenceDelivery = true;
                Mod.Log.Warn($"[Bounty#{match.Id}] elimination recognized for '{npc.ID}' (fatal={fatal}) but the " +
                             "polaroid cannot be handed over yet — delivery deferred, no second attack needed.");
            }
            else
            {
                Mod.Log.Info($"[Bounty#{match.Id}] Polaroid spawned for target '{npc.ID}' " +
                             $"(instance={npc.GetInstanceID()}); awaiting dead-drop validation.");
            }

            BountyPersistence.PersistCurrent(); // evidence state must survive a game restart
        }

        /// <summary>
        /// Hand the recognized contract's polaroid to the player. Deliberately does NOT
        /// record the kill, start heat, or advance the journal — those belong to
        /// recognition, so a deferred/repair delivery can never re-trigger kill heat
        /// (HP-04/HP-05). Starts the post-photo drop window only on an ACTUAL hand-over.
        /// Returns false when the photo cannot be created or does not fit right now.
        /// </summary>
        private static bool TryDeliverEvidence(BountyContract contract, string npcId)
        {
            if (contract == null) return false;

            var polaroid = BountyEvidenceItemRegistry.Spawn(contract.EvidenceToken);
            if (polaroid == null)
            {
                // Audit M8 (2026-09-01): the target watchdog re-calls this every 1.5s
                // while the registry is broken — throttle the warn to avoid log spam.
                if (Time.realtimeSinceStartup - _lastSpawnFailWarn > 30f)
                {
                    _lastSpawnFailWarn = Time.realtimeSinceStartup;
                    Mod.Log.Warn("Polaroid definition not registered; cannot deliver evidence for " +
                                 $"target='{npcId}' (contract #{contract.Id}).");
                }
                return false;
            }

            try
            {
                var inv = S1PlayerInventory.Instance;
                if (inv == null || inv.Pointer == IntPtr.Zero || inv.WasCollected)
                {
                    Mod.Log.Warn("PlayerInventory.Instance is unavailable; cannot add polaroid.");
                    return false;
                }
                if (!inv.CanItemFitInInventory(polaroid, 1))
                {
                    return false; // caller defers the delivery — the elimination is not lost
                }
                inv.AddItemToInventory(polaroid);
                if (!string.IsNullOrEmpty(npcId))
                {
                    _recentlyHandledNpcIds.Add(npcId);
                }
                contract.EvidenceSpawned = true;
                contract.PendingEvidenceDelivery = false;
                // The short drop window starts at the first ACTUAL hand-over, never at
                // recognition or accept time (HP-04).
                EnsureDropWindowStarted(contract);
                Interlocked.Increment(ref _polaroidsSpawned);
                return true;
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"Adding polaroid to inventory failed: {ex}");
                return false;
            }
        }

        /// <summary>
        /// HP-04/HP-05: service deferred/repair deliveries. Called from the target
        /// watchdog tick and safe to call at any time. No attack, no kill record, no
        /// heat escalation, no new drop window beyond the one the hand-over starts.
        /// </summary>
        public static void TryDeliverPendingEvidence()
        {
            if (!HitmanPhone.SaveStateGuard.IsReady) return;
            var save = Mod.Instance?.Save;
            if (save == null) return;
            if (!HitmanPhoneTime.TryGetSnapshot(out _, out _, out long now)) return;
            if (_evidenceReconciliationPending) ReconcileEvidenceOnLoad();

            for (int i = 0; i < save.Active.Count; i++)
            {
                var c = save.Active[i];
                if (c.Status != EBountyStatus.Active) continue;
                if (!c.PendingEvidenceDelivery || c.EvidenceSpawned) continue;
                // Duplicate guard: if the photo is actually present after all (e.g. the
                // inventory finished restoring after the load-time reconciliation ran),
                // do NOT issue a repair copy — just clear the pending flag.
                var presence = GetEvidencePresence(c);
                if (presence == EvidencePresence.Unknown) continue;
                if (presence == EvidencePresence.Present)
                {
                    c.EvidenceSpawned = true;
                    c.PendingEvidenceDelivery = false;
                    BountyPersistence.PersistCurrent();
                    continue;
                }
                // Never deliver for a contract that has run out of time; the expiry
                // poller retires it instead.
                if (!BountyDeadlinePolicy.IsEligible(c, now)) continue;
                if (TryDeliverEvidence(c, c.TargetNpcId))
                {
                    BountyPersistence.PersistCurrent();
                    Mod.Log.Info($"[Bounty#{c.Id}] deferred polaroid delivered after space became available " +
                                 "(no second attack, no new heat).");
                }
            }
        }

        /// <summary>
        /// HP-09: upgrade an existing knockout record for a known bounty target to a
        /// confirmed death. Independent of photo delivery and of the contract's own
        /// status (it may already be Completed/Expired) — the record itself proves the
        /// target was a bounty elimination, so unrelated NPCs stay excluded.
        /// Idempotent: a record that is already Died=true is left untouched. Never
        /// spawns a photo, starts a drop window, or pays a reward.
        /// </summary>
        public static bool TryUpgradeKillToDeath(string npcId)
        {
            if (string.IsNullOrEmpty(npcId)) return false;
            var save = Mod.Instance?.Save;
            if (save?.KillEvents == null) return false;
            if (!HitmanPhoneTime.TryGetSnapshot(out int day, out _, out long sum)) return false;

            for (int i = 0; i < save.KillEvents.Count; i++)
            {
                var e = save.KillEvents[i];
                if (e == null || !string.Equals(e.TargetNpcId, npcId, StringComparison.OrdinalIgnoreCase)) continue;
                if (e.Died) return false; // already confirmed — upgrade exactly once
                e.Died = true;
                e.Day = day;
                e.MinuteSum = sum;
                BountyPersistence.PersistCurrent();
                Mod.Log.Info($"[Bounty] Tracker upgraded '{npcId}' to confirmed death (post-KO).");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tracker record (kill tracker update): exactly ONE historical record per
        /// NPC per save — several contracts on the same head must never count as
        /// several kills. A confirmed fatal death upgrades an earlier KO record.
        /// </summary>
        private static void RecordKillEvent(BountyContract match, string npcId, bool fatal)
        {
            try
            {
                var save = Mod.Instance?.Save;
                if (save == null) return;
                string id = string.IsNullOrEmpty(npcId) ? match.TargetNpcId : npcId;
                if (string.IsNullOrEmpty(id)) return;
                for (int i = 0; i < save.KillEvents.Count; i++)
                {
                    var e = save.KillEvents[i];
                    if (!string.Equals(e.TargetNpcId, id, StringComparison.OrdinalIgnoreCase)) continue;
                    if (fatal && !e.Died)
                    {
                        e.Died = true;
                        e.Day = HitmanPhoneTime.CurrentDay();
                        e.MinuteSum = HitmanPhoneTime.CurrentMinuteSum();
                        e.ContractId = match.Id;
                    }
                    return;
                }
                save.KillEvents.Add(new BountyKillRecord
                {
                    TargetNpcId = id,
                    TargetName = match.TargetNpcName,
                    Day = HitmanPhoneTime.CurrentDay(),
                    MinuteSum = HitmanPhoneTime.CurrentMinuteSum(),
                    Died = fatal,
                    ContractId = match.Id
                });
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"RecordKillEvent failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes only the evidence belonging to a contract when it ends without
        /// payout. New photos are matched by persistent evidence token; old-format
        /// photos are matched only through an active legacy contract's preserved id.
        /// Idempotent: no match is a no-op. Never throws; a failed removal only logs a warning.
        /// </summary>
        public static void RemovePolaroidFromInventory(BountyContract c, string reason)
        {
            try
            {
                if (c == null ||
                    (c.EvidenceToken == 0 && c.LegacyTargetInstanceId == 0 && c.TargetNpcInstanceId == 0)) return;
                // Host-only: a client-side ClearStoredInstance does not replicate (see BountyReceiptService).
                if (!NetworkGuard.IsHostOrSingleplayer()) return;
                var save = Mod.Instance?.Save;
                if (save == null) return;

                if (HasOtherActiveContractOnTarget(save, c)) return;

                var inv = S1PlayerInventory.Instance;
                if (inv == null || inv.Pointer == IntPtr.Zero || inv.WasCollected) return;

                var slots = inv.GetAllInventorySlots();
                if (slots == null) return;
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null) continue;
                    var inst = slot.ItemInstance;
                    if (inst == null || !BountyReceiptService.MatchesEvidenceForContract(c, inst)) continue;

                    slot.ClearStoredInstance();
                    Mod.Log.Info($"[Bounty#{c.Id}] Polaroid removed from inventory ({reason}).");
                    return;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[Bounty#{c?.Id}] Polaroid removal failed ({reason}); item may remain in inventory: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes the contract's polaroid from registered dead-drop storage when the
        /// contract ends without payout. A polaroid deposited before expiry/forfeit
        /// would otherwise stay in the drop and could still pay out a dead contract.
        /// Same guards as <see cref="RemovePolaroidFromInventory"/>. Idempotent: no
        /// match is a no-op. Never throws; a failed removal only logs a warning.
        /// </summary>
        public static void RemovePolaroidFromDeadDrops(BountyContract c, string reason)
        {
            try
            {
                if (c == null ||
                    (c.EvidenceToken == 0 && c.LegacyTargetInstanceId == 0 && c.TargetNpcInstanceId == 0)) return;
                if (!NetworkGuard.IsHostOrSingleplayer()) return;
                var save = Mod.Instance?.Save;
                if (save == null) return;
                if (HasOtherActiveContractOnTarget(save, c)) return;

                BountyReceiptService.RemoveDeadDropPolaroids(c, reason);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[Bounty#{c?.Id}] Dead-drop polaroid removal failed ({reason}); item may remain in a drop: {ex.Message}");
            }
        }

        /// <summary>
        /// One polaroid can cover several contracts on the same head: the polaroid
        /// stays while another active contract still waits for that target.
        /// </summary>
        private static bool HasOtherActiveContractOnTarget(BountySaveData save, BountyContract c)
        {
            for (int i = 0; i < save.Active.Count; i++)
            {
                var other = save.Active[i];
                if (other != c && other.Status == EBountyStatus.Active &&
                    string.Equals(other.TargetNpcId, c.TargetNpcId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// First confirmed receipt of the polaroid (in the player's inventory or in
        /// any dead drop) starts the short post-photo drop window — the countdown
        /// begins at receipt, never at accept. Idempotent. Legacy contracts without
        /// an offer-time window (DropWindowMinutes ≤ 0) keep their old single
        /// deadline: no retroactive short window is imposed on old saves.
        ///
        /// HP-13: a receipt after the execution deadline must NOT open a fresh valid
        /// window — the contract's fate is decided by the shared deadline rule, not by
        /// the hand-over. Uses a valid in-game time snapshot; an invalid read never
        /// starts a window from "minute zero".
        /// </summary>
        public static void EnsureDropWindowStarted(BountyContract c)
        {
            if (c == null || c.DropDueStarted) return;
            if (c.DropWindowMinutes <= 0) return;
            if (!HitmanPhoneTime.TryGetSnapshot(out _, out _, out long now))
            {
                Mod.Log.Warn($"[Bounty#{c.Id}] drop window not started: in-game time unavailable.");
                return;
            }
            if (!BountyDeadlinePolicy.IsExecutionPhaseEligible(c, now)) return;
            c.DropDueStarted = true;
            c.DropDueMinSum = now + c.DropWindowMinutes;
            Mod.Log.Info($"[Bounty#{c.Id}] Drop window started: {c.DropWindowMinutes} min " +
                         $"(due min-sum {c.DropDueMinSum}, drop '{c.AssignedDropName}').");
        }

        /// <summary>
        /// HP-05: reconcile the persisted evidence state with the ACTUAL world after a
        /// confirmed load. The old implementation blindly cleared EvidenceSpawned for
        /// every active contract, so a still-present photo could be spawned a second
        /// time and re-escalate the pursuit on every reload.
        ///
        /// New rule, per active contract that carries any evidence state:
        ///   • a polaroid for this contract exists (inventory or any dead drop) → keep
        ///     EvidenceSpawned set; no duplicate, no new pursuit.
        ///   • no polaroid exists but the elimination was recognized (AwaitingDrop) →
        ///     mark a pending delivery; the photo is re-issued WITHOUT a new kill and
        ///     WITHOUT new heat.
        ///   • otherwise reset the transient flag.
        ///
        /// Call from the confirmed load-complete path (SaveStateGuard).
        /// </summary>
        private static bool _evidenceReconciliationPending;

        public static void ReconcileEvidenceOnLoad()
        {
            var save = Mod.Instance?.Save;
            if (save == null) return;

            int present = 0, repair = 0, reset = 0, unknown = 0;
            for (int i = 0; i < save.Active.Count; i++)
            {
                var c = save.Active[i];
                if (c.Status != EBountyStatus.Active) continue;
                if (!c.EvidenceSpawned && !c.AwaitingDrop && !c.PendingEvidenceDelivery) continue;

                var presence = GetEvidencePresence(c);
                if (presence == EvidencePresence.Unknown)
                {
                    unknown++;
                    continue; // incomplete/ambiguous is not proof that the photo is lost
                }
                if (presence == EvidencePresence.Present)
                {
                    c.EvidenceSpawned = true;
                    c.PendingEvidenceDelivery = false;
                    present++;
                }
                else if (c.AwaitingDrop)
                {
                    // Elimination recognized but the photo is genuinely gone: re-issue
                    // through the pending-delivery path (no new kill, no new heat).
                    c.EvidenceSpawned = false;
                    c.PendingEvidenceDelivery = true;
                    repair++;
                }
                else
                {
                    c.EvidenceSpawned = false;
                    c.PendingEvidenceDelivery = false;
                    reset++;
                }
            }

            _evidenceReconciliationPending = unknown > 0;
            if (present > 0 || repair > 0 || reset > 0)
            {
                Mod.Log.Info($"[Bounty] Evidence reconciled after load: {present} present, " +
                             $"{repair} pending re-delivery, {reset} reset.");
            }
        }

        /// <summary>
        /// Back-compat alias for the old load hook name. It no longer performs a blanket
        /// EvidenceSpawned reset — it reconciles against real evidence (HP-05).
        /// </summary>
        public static void ResetEvidenceFlags() => ReconcileEvidenceOnLoad();

        /// <summary>
        /// True when a polaroid for this contract is physically present in the player's
        /// inventory or in any registered dead drop. Uses the same token/legacy matching
        /// as the payout, so a photo the payout could find is never treated as lost.
        /// </summary>
        private static EvidencePresence GetEvidencePresence(BountyContract c)
        {
            var result = EvidencePresence.Unknown;
            try
            {
                var inv = S1PlayerInventory.Instance;
                if (inv != null && inv.Pointer != IntPtr.Zero && !inv.WasCollected)
                {
                    var slots = inv.GetAllInventorySlots();
                    if (slots != null)
                    {
                        result = EvidencePresence.Missing;
                        for (int i = 0; i < slots.Count; i++)
                        {
                            var slot = slots[i];
                            if (slot == null) { result = EvidencePresence.Unknown; continue; }
                            var inst = slot.ItemInstance;
                            if (inst != null) result = BountyEvidenceMatcher.MergePresence(result, BountyReceiptService.ObserveEvidenceForContract(c, inst));
                            if (result == EvidencePresence.Present) return result;
                        }
                    }
                }

                var drops = S1DeadDrop.DeadDrops;
                if (drops != null && drops.Count > 0)
                {
                    for (int d = 0; d < drops.Count; d++)
                    {
                        var drop = drops[d];
                        if (drop == null || drop.Pointer == IntPtr.Zero || drop.WasCollected) { result = EvidencePresence.Unknown; continue; }
                        var entity = drop.Storage;
                        if (entity == null || entity.Pointer == IntPtr.Zero || entity.WasCollected) { result = EvidencePresence.Unknown; continue; }
                        var slots = entity.ItemSlots;
                        if (slots == null) { result = EvidencePresence.Unknown; continue; }
                        for (int i = 0; i < slots.Count; i++)
                        {
                            var slot = slots[i];
                            if (slot == null) { result = EvidencePresence.Unknown; continue; }
                            var inst = slot.ItemInstance;
                            if (inst != null) result = BountyEvidenceMatcher.MergePresence(result, BountyReceiptService.ObserveEvidenceForContract(c, inst));
                            if (result == EvidencePresence.Present) return result;
                        }
                    }
                }
                else result = EvidencePresence.Unknown;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[Bounty#{c.Id}] evidence presence check failed: {ex.Message}");
                return EvidencePresence.Unknown;
            }
            return result;
        }

        /// <summary>
        /// Migrates pre-token contracts and clears transient Unity instance IDs after
        /// a load. Awaiting legacy contracts retain their old photo ID for matching;
        /// new evidence uses its persistent token, so IDs cannot collide across sessions.
        /// Also reconciles the persisted evidence state against the real world (HP-05).
        /// Call from the confirmed load-complete path (SaveStateGuard).
        /// </summary>
        public static void InvalidatePersistedInstanceIds()
        {
            if (Mod.Instance?.Save == null) return;
            int migrated = BountyEvidenceMatcher.MigrateLegacyContracts(Mod.Instance.Save);
            if (migrated > 0)
            {
                Mod.Log.Info($"[Bounty] Migrated {migrated} saved evidence field(s) after load; old photos remain bound only to their active contracts.");
                BountyPersistence.PersistCurrent();
            }
            ReconcileEvidenceOnLoad();
        }
    }
    // ==== end HitmanPhone section: Contracts/BountyService.cs ====
}

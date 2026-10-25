// ---------------------------------------------------------------------------
// Tracking/BountyTracking.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Tracking/BountyTargetWatchdog.cs     -> section Tracking/BountyTargetWatchdog.cs
//   Tracking/NPCDeathPatch.cs            -> section Tracking/NPCDeathPatch.cs
//   Tracking/TargetSelector.cs           -> section Tracking/TargetSelector.cs
//
// ---------------------------------------------------------------------------

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Tracking/BountyTargetWatchdog.cs ====
    using S1Mods.Shared;
    using HitmanPhone.Persistence;
    using System;
    using UnityEngine;
#if (IL2CPPMELON)
    using S1NPCManager = Il2CppScheduleOne.NPCs.NPCManager;
#elif MONOMELON
    using S1NPCManager = ScheduleOne.NPCs.NPCManager;
#endif


    /// <summary>
    /// Phase C Alternative - Polls active targets for IsConscious == false.
    /// This completely bypasses the need to guess if the engine used OnDie, 
    /// KnockOut, or SetConscious, making it 100% resilient to game updates.
    /// </summary>
    public static class BountyTargetWatchdog
    {
        private static float _nextTick;

        public static void Tick()
        {
            if (Time.time < _nextTick) return;
            _nextTick = Time.time + 1.5f; // Poll every 1.5s (very cheap, 0-allocation)

            // Singleplayer-only + confirmed-load gate.
            if (!HitmanPhone.SaveStateGuard.IsReady) return;
            if (Mod.Instance?.Save == null) return;

            // HP-04: service deferred/repair photo deliveries first. This is independent
            // of the target's state, so a full inventory at the kill no longer needs a
            // second attack (or a still-unconscious target) to get the photo.
            BountyService.TryDeliverPendingEvidence();

            if (!HitmanPhoneTime.TryGetSnapshot(out _, out _, out long now)) return;

            for (int i = 0; i < Mod.Instance.Save.Active.Count; i++)
            {
                var c = Mod.Instance.Save.Active[i];

                if (c.Status != EBountyStatus.Active) continue;

                try
                {
                    var npc = S1NPCManager.GetNPC(c.TargetNpcId);
                    if (npc == null) continue;

                    // Bug-Audit 2026-09-12: `!npc.IsConscious` fired on KO too. NPCHealth.IsDead
                    // distinguishes the two via the game's own state.
                    bool dead = false;
                    bool down = false;
                    var health = npc.Health;
                    if (health != null && health.Pointer != IntPtr.Zero && !health.WasCollected)
                    {
                        dead = health.IsDead;
                        down = health.IsKnockedOut || !npc.IsConscious;
                    }

                    // HP-05: a contract whose elimination is already recognized must never
                    // be re-recognized — re-recognition after a load produced a duplicate
                    // photo with the same token AND a repeated Lethal escalation. A later
                    // fatal death is still recorded (HP-09), but only the tracker record:
                    // never a second photo, drop window or reward.
                    if (c.EvidenceSpawned || c.AwaitingDrop || c.PendingEvidenceDelivery)
                    {
                        if (dead) BountyService.TryUpgradeKillToDeath(c.TargetNpcId);
                        continue;
                    }

                    if (!down) continue;

                    // HP-13: never recognize an elimination past the execution deadline —
                    // a late knockout must not open a fresh valid drop window.
                    if (!BountyDeadlinePolicy.IsEligible(c, now)) continue;

                    // Trigger the existing polaroid logic. Fatal = the game's own
                    // Health.IsDead; a knockout/unconsciousness records
                    // "ausgeschaltet" only (tracker semantics).
                    BountyService.OnNpcDied(npc, fatal: dead);
                }
                catch { }
            }
        }
    }
    // ==== end HitmanPhone section: Tracking/BountyTargetWatchdog.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Tracking/NPCDeathPatch.cs ====
    using System;
    using System.Reflection;
    using HarmonyLib;
    using S1Mods.Shared;

#if (IL2CPPMELON)
    using S1NPC = Il2CppScheduleOne.NPCs.NPC;
    using S1NPCHealth = Il2CppScheduleOne.NPCs.NPCHealth;
#elif MONOMELON
    using S1NPC = ScheduleOne.NPCs.NPC;
    using S1NPCHealth = ScheduleOne.NPCs.NPCHealth;
#endif


    /// <summary>
    /// Phase C — Harmony-Postfix on <see cref="S1NPC"/>.OnDie().
    ///
    /// We do not throw inside the patch. If anything goes wrong (game version drift,
    /// nulled instance, save corruption) PatchGuard logs and swallows the exception
    /// so the game remains stable — Skill Rule #3 ("Never let a Harmony patch throw").
    ///
    /// Actual NPC.OnDie signature (from stub decompile):
    ///   protected virtual void OnDie();
    /// We patch with <c>HarmonyPrefix</c> semantics (Harmony actually routes any
    /// declared Patch type — Prefix/Postfix/Finalizer — to the corresponding slot).
    /// We use Postfix here so we don't disturb the engine's death logic.
    /// </summary>
    [HarmonyPatch]
    internal static class NPCDeathPatch
    {
        /// <summary>
        /// Harmony requires static <c>TargetMethod()</c> returning the MethodInfo
        /// to patch because the stub decompile hides the runtime signature. We find
        /// OnDie via reflection (parameterless instance method on S1NPC).
        /// </summary>
        private static MethodBase? TargetMethod()
        {
            try
            {
                return typeof(S1NPC).GetMethod(
                    "OnDie",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    binder: null,
                    types: Type.EmptyTypes,
                    modifiers: null);
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"NPCDeathPatch.TargetMethod failed: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Postfix runs after the engine's OnDie. We do our work in BountyService.OnNpcDied
        /// so the patch body itself stays thin and PatchGuard-friendly.
        /// Audit (2026-09-10): internal (was private) so Mod.cs can bind it via an
        /// explicit PatchGuard.TryPatch HarmonyMethod instead of blind PatchAll.
        /// </summary>
        [HarmonyPostfix]
        internal static void Postfix(S1NPC __instance)
        {
            try
            {
                BountyService.OnNpcDied(__instance);
            }
            catch (Exception ex)
            {
                // Skill Rule #3: never let a Harmony patch throw. Log it and continue.
                Mod.Log.Error($"NPCDeathPatch.Postfix swallowed exception: {ex}");
            }
        }

        /// <summary>
        /// Phase C2 postfix on <see cref="S1NPCHealth"/>.Die() — the state transition behind
        /// the IsDead flag. Covers death paths whose NPC.OnDie event is never raised
        /// (e.g. finishing a knocked-out target, drowning): if the game marks the health
        /// component dead, the transition fires here and the bounty resolves.
        /// BountyService.OnNpcDied is idempotent per contract (EvidenceSpawned latch), so
        /// this can safely also run for normal deaths (OnDie + Die both fire) without
        /// producing duplicate polaroids.
        /// </summary>
        [HarmonyPostfix]
        internal static void PostfixHealthDie(S1NPCHealth __instance)
        {
            try
            {
                if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;
                // 0.4.7 drift: NPCHealth.npc field removed (now _npc, private). Resolve the NPC
                // via GetComponent on the same GameObject — the pattern S1API itself uses for
                // NPCHealth since 3.2.0/0.4.6, so it compiles and runs on both game versions.
                var npc = __instance.GetComponent<S1NPC>();
                if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) return;
                // Tracker semantics: Health.Die = game-confirmed death.
                BountyService.OnNpcDied(npc, fatal: true);
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"NPCDeathPatch.PostfixHealthDie swallowed exception: {ex}");
            }
        }

        /// <summary>
        /// Phase C3 postfix on <see cref="S1NPCHealth"/>.KnockOut() — in Schedule I, combat
        /// (punches, melee weapons) knocks the NPC out (IsKnockedOut = true) rather than killing
        /// them outright. Patching KnockOut allows bounty targets to be eliminated on takedown
        /// without forcing the player to drag the body into water to trigger Die().
        /// </summary>
        [HarmonyPostfix]
        internal static void PostfixHealthKnockOut(S1NPCHealth __instance)
        {
            try
            {
                if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;
                // 0.4.7 drift: same as PostfixHealthDie — NPCHealth.npc is gone; GetComponent instead.
                var npc = __instance.GetComponent<S1NPC>();
                if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) return;
                // Tracker semantics: a knockout is "ausgeschaltet", NOT confirmed dead.
                BountyService.OnNpcDied(npc, fatal: false);
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"NPCDeathPatch.PostfixHealthKnockOut swallowed exception: {ex}");
            }
        }
    }
    // ==== end HitmanPhone section: Tracking/NPCDeathPatch.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Tracking/TargetSelector.cs ====
    using System;
    using System.Collections.Generic;
    using HitmanPhone.Persistence;

#if (IL2CPPMELON)
    using S1NPC = Il2CppScheduleOne.NPCs.NPC;
    using S1Dealer = Il2CppScheduleOne.Economy.Dealer;
    using S1Customer = Il2CppScheduleOne.Economy.Customer;
#elif MONOMELON
    using S1NPC = ScheduleOne.NPCs.NPC;
    using S1Dealer = ScheduleOne.Economy.Dealer;
    using S1Customer = ScheduleOne.Economy.Customer;
#endif


    /// <summary>
    /// Selects bounty targets from the player's actual deal network.
    ///
    /// A target is eligible when it is one of the player's unlocked customers
    /// (<c>Customer.UnlockedCustomers</c>) or assigned to a recruited dealer in
    /// <c>Dealer.AllPlayerDealers</c>. Direct customers count: a save with no
    /// recruited dealer still has a network. The old implementation used a
    /// hard-coded list of generic NPC IDs and could therefore select unrelated
    /// civilians; that list is not restored.
    /// </summary>
    public static class TargetSelector
    {
        /// <summary>
        /// NPC ids that are never valid Hitman targets, even if a broken/old save
        /// happens to expose one through a dealer assignment.
        /// </summary>
        private static readonly HashSet<string> ExcludedIds = new(StringComparer.OrdinalIgnoreCase)
        {
            // Dealers
            "benji_coleman", "brad_crosby", "jane_lucero", "leo_rivers", "molly_presley", "wei_long",
            // Suppliers
            "albert_hoover", "phil_wentworth", "salvador_moreno", "shirley_watts",
            // Specials
            "uncle_nelson", "igor_romanovich", "oscar_holland", "stan_carney",
            // Police officers
            "officer_bailey", "officer_cooper", "officer_green", "officer_howard",
            "officer_jackson", "officer_lee", "officer_lopez", "officer_murphy", "officer_oakley",
            // Mercedes (S1API test NPC id)
            "mercedes"
        };

        /// <summary>
        /// Picks a random living customer from the player's own network.
        /// Returns null when that network has no eligible customer.
        /// </summary>
        public static S1NPC? PickRandom()
        {
            var candidates = CollectOwnCustomers();
            if (candidates.Count == 0) return null;
            return candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }

        /// <summary>
        /// Verifies that a target belongs to the player's current deal network.
        /// Used by the test/force-offer command as well as the normal selector so
        /// that the manual path cannot bypass the target restriction.
        /// </summary>
        public static bool IsOwnCustomer(S1NPC? target)
        {
            if (!IsEligible(target)) return false;

            string targetId;
            try
            {
                targetId = target!.ID;
            }
            catch
            {
                return false;
            }

            try
            {
                if (IsUnlockedCustomer(targetId)) return true;
                return IsAssignedToRecruitedDealer(targetId);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"TargetSelector.IsOwnCustomer failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Mean weekly customer spend for this target (budget/tier basis). Returns
        /// 0 when unknown; the contract then falls back to legacy reward math.
        /// </summary>
        public static float TryGetWeeklySpend(S1NPC? target)
        {
            if (target == null || target.WasCollected) return 0f;
            try
            {
                var customer = target.GetComponent<S1Customer>();
                if (customer == null || customer.WasCollected) return 0f;
                var data = customer.CustomerData;
                if (data == null || data.WasCollected) return 0f;
                return (data.MinWeeklySpend + data.MaxWeeklySpend) / 2f;
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"TargetSelector.TryGetWeeklySpend failed: {ex.Message}");
                return 0f;
            }
        }

        /// <summary>Offer-text location used when a target exposes no valid region.</summary>
        public const string FallbackRegionName = "Hyland Point";

        /// <summary>
        /// Designated region of the target (e.g. "Docks") for the offer text. The NPC
        /// region is the home designation, not a live position. Falls back to
        /// <see cref="FallbackRegionName"/> when unreadable or out of range.
        /// </summary>
        public static string GetRegionName(S1NPC? target)
        {
            if (target == null || target.WasCollected) return FallbackRegionName;
            try
            {
                int value = (int)target.Region;
                if (Enum.IsDefined(typeof(S1API.Map.Region), value))
                    return ((S1API.Map.Region)value).ToString();
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"TargetSelector.GetRegionName failed: {ex.Message}");
            }
            return FallbackRegionName;
        }

        private static List<S1NPC> CollectOwnCustomers()
        {
            var candidates = new List<S1NPC>();
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // A missing dealer registry must not hide the player's own customers.
            try
            {
                CollectUnlockedCustomers(candidates, seenIds);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"TargetSelector.CollectOwnCustomers unlocked scan failed: {ex.Message}");
            }

            try
            {
                CollectDealerCustomers(candidates, seenIds);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"TargetSelector.CollectOwnCustomers dealer scan failed: {ex.Message}");
            }

            return candidates;
        }

        /// <summary>
        /// Player's own unlocked customers. Confirmed on the f12 assembly as
        /// <c>Customer.UnlockedCustomers</c> with <c>Customer.NPC</c>.
        /// </summary>
        private static void CollectUnlockedCustomers(List<S1NPC> candidates, HashSet<string> seenIds)
        {
            var unlocked = S1Customer.UnlockedCustomers;
            if (unlocked == null) return;

            // IL2CPP collections must be indexed, not enumerated with foreach.
            for (int i = 0; i < unlocked.Count; i++)
            {
                var customer = unlocked[i];
                if (customer == null || customer.WasCollected) continue;
                TryAddCandidate(candidates, seenIds, customer.NPC);
            }
        }

        private static void CollectDealerCustomers(List<S1NPC> candidates, HashSet<string> seenIds)
        {
            var dealers = S1Dealer.AllPlayerDealers;
            if (dealers == null) return;

            for (int dealerIndex = 0; dealerIndex < dealers.Count; dealerIndex++)
            {
                var dealer = dealers[dealerIndex];
                if (dealer == null || !dealer.IsRecruited) continue;

                var assignedCustomers = dealer.AssignedCustomers;
                if (assignedCustomers == null) continue;

                for (int customerIndex = 0; customerIndex < assignedCustomers.Count; customerIndex++)
                {
                    TryAddCandidate(candidates, seenIds, assignedCustomers[customerIndex]?.NPC);
                }
            }
        }

        private static void TryAddCandidate(List<S1NPC> candidates, HashSet<string> seenIds, S1NPC? npc)
        {
            if (!IsEligible(npc)) return;
            string id = npc!.ID;
            if (seenIds.Add(id)) candidates.Add(npc);
        }

        private static bool IsUnlockedCustomer(string targetId)
        {
            var unlocked = S1Customer.UnlockedCustomers;
            if (unlocked == null) return false;

            for (int i = 0; i < unlocked.Count; i++)
            {
                var customer = unlocked[i];
                if (customer == null || customer.WasCollected) continue;
                if (MatchesId(customer.NPC, targetId)) return true;
            }

            return false;
        }

        private static bool IsAssignedToRecruitedDealer(string targetId)
        {
            var dealers = S1Dealer.AllPlayerDealers;
            if (dealers == null) return false;

            for (int dealerIndex = 0; dealerIndex < dealers.Count; dealerIndex++)
            {
                var dealer = dealers[dealerIndex];
                if (dealer == null || !dealer.IsRecruited) continue;

                var assignedCustomers = dealer.AssignedCustomers;
                if (assignedCustomers == null) continue;

                for (int customerIndex = 0; customerIndex < assignedCustomers.Count; customerIndex++)
                {
                    if (MatchesId(assignedCustomers[customerIndex]?.NPC, targetId)) return true;
                }
            }

            return false;
        }

        private static bool MatchesId(S1NPC? npc, string targetId)
        {
            if (npc == null || npc.WasCollected) return false;
            try
            {
                return string.Equals(npc.ID, targetId, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsEligible(S1NPC? npc)
        {
            if (npc == null) return false;

            try
            {
                if (!npc.IsConscious) return false;
                if (string.IsNullOrEmpty(npc.ID)) return false;
                if (ExcludedIds.Contains(npc.ID)) return false;

                var save = Mod.Instance?.Save;
                if (save != null)
                {
                    for (int i = 0; i < save.Active.Count; i++)
                    {
                        if (save.Active[i].Status == EBountyStatus.Active &&
                            string.Equals(save.Active[i].TargetNpcId, npc.ID, StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                // NPC may be destroyed while the assignment list is being read.
                return false;
            }
        }
    }
    // ==== end HitmanPhone section: Tracking/TargetSelector.cs ====
}

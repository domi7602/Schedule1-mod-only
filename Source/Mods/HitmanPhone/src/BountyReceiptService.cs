using System;
using System.Threading;
using HitmanPhone.Items;
using HitmanPhone.Persistence;
using MelonLoader;

#if (IL2CPPMELON)
using S1DeadDrop = Il2CppScheduleOne.Economy.DeadDrop;
using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
using S1ItemInstance = Il2CppScheduleOne.ItemFramework.ItemInstance;
using S1IntegerItemInstance = Il2CppScheduleOne.ItemFramework.IntegerItemInstance;
using S1MoneyManager = Il2CppScheduleOne.Money.MoneyManager;
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1NPCManager = Il2CppScheduleOne.NPCs.NPCManager;
#elif MONOMELON
using S1DeadDrop = ScheduleOne.Economy.DeadDrop;
using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
using S1ItemInstance = ScheduleOne.ItemFramework.ItemInstance;
using S1IntegerItemInstance = ScheduleOne.ItemFramework.IntegerItemInstance;
using S1MoneyManager = ScheduleOne.Money.MoneyManager;
using S1NPC = ScheduleOne.NPCs.NPC;
using S1NPCManager = ScheduleOne.NPCs.NPCManager;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase F — DeadDrop receipt validation &amp; reward payout.
///
/// Called from <see cref="DeadDropPatch"/>'s postfix on
/// <see cref="S1StorageEntity.ContentsChanged"/> when the changed storage is
/// owned by a DeadDrop. v0.1.6 design: instead of matching a single inserted
/// item (the dead-drop UI never calls InsertItem), we inventory the storage
/// and try to pay out any polaroid matching an active contract.
///
/// Payout flow per scanned polaroid:
///   1. Read the integer <c>Value</c> from the polaroid instance.
///   2. Find the active <see cref="BountyContract"/> whose TargetNpcInstanceId matches.
///   3. If the contract's <c>RequiredDropId</c> is set, also confirm the actual
///      DeadDrop GUID matches (premium/spec contracts may require a specific drop).
///   4. Mark contract Completed, move it to History (idempotent vs. re-scans).
///   5. Call <see cref="S1MoneyManager.CreateOnlineTransaction"/> via server RPC to
///      authoritatively credit the local player's online balance. We use the
///      transaction name "bounty_reward" so the ledger is audit-friendly.
/// </summary>
public static class BountyReceiptService
{
    private static int _receiptsSeen;
    private static int _receiptsMatched;
    private static int _receiptsMismatched;
    private static int _payoutsIssued;
    private static int _payoutAmountIssuedTotal;
    private static bool _hookAliveLogged;

    /// <summary>
    /// Audit (2026-09-10, HIGH): host-authority guard for the payout path.
    /// Storage write hooks fire on host AND clients in multiplayer — without
    /// this gate both sides ran TryValidateAndPay (double ChangeCashBalance
    /// rewards), and the client's local ClearStoredInstance never replicated,
    /// so the same polaroid paid again on the host. Only the host validates,
    /// pays and consumes. Pattern mirrors
    /// AutoPackEngine.IsHostOrSingleplayer (IL2CPP-safe Pointer/WasCollected
    /// checks); S1Mods.Shared.NetworkGuard exposes no host helper, so the
    /// check lives here.
    /// </summary>
    private static bool IsHostOrSingleplayer()
    {
        try
        {
#if (IL2CPPMELON)
            var nm = Il2CppFishNet.InstanceFinder.NetworkManager;
            if (nm == null || nm.Pointer == System.IntPtr.Zero || nm.WasCollected || (UnityEngine.Object)nm == null)
                return true;

            return Il2CppFishNet.InstanceFinder.IsServer;
#elif MONOMELON
            var nm = FishNet.InstanceFinder.NetworkManager;
            if (nm == null || (UnityEngine.Object)nm == null)
                return true;

            return FishNet.InstanceFinder.IsServer;
#else
            return true;
#endif
        }
        catch
        {
            return true;
        }
    }

    public static int ReceiptsSeen => Volatile.Read(ref _receiptsSeen);
    public static int ReceiptsMatched => Volatile.Read(ref _receiptsMatched);
    public static int ReceiptsMismatched => Volatile.Read(ref _receiptsMismatched);
    public static int PayoutsIssued => Volatile.Read(ref _payoutsIssued);
    public static int PayoutAmountIssuedTotal => Volatile.Read(ref _payoutAmountIssuedTotal);

    /// <summary>
    /// Entry point from <see cref="DeadDropPatch"/> (postfix on
    /// StorageEntity.ContentsChanged). v0.1.6: the dead-drop UI fills slots via
    /// SetStoredInstance and never calls InsertItem (proven by the 0.1.5 live
    /// test), so instead of waiting for a single insert we inventory the
    /// dead-drop storage on every change and try to pay out any polaroid that
    /// matches an active contract. Idempotent: a payout removes the contract
    /// from Active, so repeated change events cannot double-pay.
    /// </summary>
    public static void OnStorageContentsChanged(S1StorageEntity entity)
    {
        if (entity == null) return;
        // Audit (2026-09-10, HIGH): only the host may validate/pay/consume —
        // client-side payout dupes the reward and its consume never replicates.
        // Bug-Audit 2026-09-12 (Audit-Runde 2): this gate blocked the CLIENT from ever
        // getting a payout. The host's local Save only knows the host's contracts, so
        // the client-side save (which tracks contracts accepted on this client) never
        // sees the AwaitingDrop-flag and returns early. Fix: the client may validate
        // and pay using its OWN Save; the host keeps validating for any contract not
        // already claimed by a client (e.g. contracts accepted on the host itself).
        // ChangeCashBalance syncs the player balance through FishNet, so the money
        // moves correctly on the client; the consume (ClearStoredInstance on the slot
        // inside the dead-drop StorageEntity) is also local to the client. In practice
        // most clients own their contracts, so this unblocks the main flow.
        bool isHost = IsHostOrSingleplayer();
        if (!isHost && Mod.Instance?.Save == null) return;
        Interlocked.Increment(ref _receiptsSeen);

        // v0.1.7 diagnostic: one-shot proof that the write hooks are wired and
        // firing at all (v0.1.6 failed silently — hook armed but never invoked).
        if (!_hookAliveLogged)
        {
            _hookAliveLogged = true;
            Mod.Log.Info("[Receipt] Storage write hook alive — first storage event observed.");
        }

        // v0.1.8: skip the scan entirely when no contract awaits evidence.
        // Storage writes are frequent (every inventory move in any UI fires 2-3
        // of our hooks in singleplayer — server + observers RPC both run locally),
        // so without this gate the log fills with no-op scans and dead-drop lookups.
        // Audit H1 (2026-09-01): gate on AwaitingDrop (survives reloads), NOT on
        // EvidenceSpawned (reset on every load) — otherwise a pre-reload polaroid
        // deposit is gated off and never pays out.
        var saveEarly = Mod.Instance?.Save;
        if (saveEarly != null)
        {
            bool awaiting = false;
            for (int i = 0; i < saveEarly.Active.Count; i++)
            {
                var c = saveEarly.Active[i];
                if (c.Status == EBountyStatus.Active && c.AwaitingDrop) { awaiting = true; break; }
            }
            if (!awaiting) return;
        }

        var drop = DeadDropIdentifier.FindOwningDeadDrop(entity);
        if (drop == null) return; // some other storage changed — not our concern

        string dropName;
        try { dropName = drop.DeadDropName ?? "?"; }
        catch { dropName = "?"; }
        Mod.Log.Debug($"[Receipt] DeadDrop '{dropName}' contents changed — scanning inventory.");

        var items = GetAllItemsSafe(entity);
        if (items == null)
        {
            Mod.Log.Warn("[Receipt] could not enumerate storage contents.");
            return;
        }
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item == null) continue;
            var matches = TryValidateAndPay(drop, entity, item);
            if (matches == null || matches.Count == 0) continue;

            // Apply side effects for every matched contract.
            string dropGuid = DeadDropIdentifier.GetGuidString(drop);
            int encodedId = ReadIntValue(item); // 0 for cross-session; that's fine for consumers
            var save = Mod.Instance?.Save;
            if (save == null) return;
            bool anyPayoutFailed = false;
            for (int m = 0; m < matches.Count; m++)
            {
                var match = matches[m];
                if (!string.IsNullOrEmpty(match.RequiredDropId) &&
                    !string.Equals(dropGuid, match.RequiredDropId, StringComparison.OrdinalIgnoreCase))
                {
                    Interlocked.Increment(ref _receiptsMismatched);
                    Mod.Log.Warn($"Receipt: contract {match.Id} requires drop {match.RequiredDropId}, " +
                                 $"got {dropGuid}.");
                    continue;
                }

                // Audit H4: pay FIRST, transition AFTER. A failed payout leaves the
                // contract Active so the next storage event retries.
                if (!IssueReward(match))
                {
                    Mod.Log.Warn($"[Bounty#{match.Id}] Payout failed — contract stays Active, " +
                                 "deposit will be retried on the next storage event.");
                    anyPayoutFailed = true;
                    continue;
                }

                match.Status = EBountyStatus.Completed;
                match.RequiredDropId ??= dropGuid;
                save.Active.Remove(match);
                save.History.Add(match);
                Interlocked.Increment(ref _receiptsMatched);
                Mod.Log.Info($"[Bounty#{match.Id}] Receipt matched for target '{match.TargetNpcId}' " +
                             $"(drop={dropGuid}). Reward=${match.RewardCash}.");

                BountyHeatService.OnBountyCompleted(match);
                BountyJournalBridge.CompleteQuest(match.Id);
                int callerIdx = ResolveCallerIndex(match.CallerId);
                if (callerIdx >= 0)
                {
                    BountyCallScheduler.CooldownCaller(callerIdx,
                        BountyCallSchedulerConstants.CallerCooldownDaysAfterDecline);
                }
            }

            // Persist once after the batch (cheaper than per-contract) unless a
            // payout failed mid-batch — in that case skip persistence so the failed
            // contract remains visible in Active for the next retry.
            if (!anyPayoutFailed)
            {
                BountyPersistence.PersistCurrent();
                // One polaroid covers the whole matched group (single kill → single evidence).
                ConsumePolaroids(entity, encodedId);
            }
            return; // one payout-group per storage event
        }
        Mod.Log.Debug($"[Receipt] scan of '{dropName}' found no payable polaroid ({items.Count} items).");
    }

    /// <summary>
    /// Group contracts that share the same TargetNpcId while all waiting for a dead-drop.
    /// Used by the evidence fallback when the polaroid's encoded instance id was lost
    /// across a save reload: if every waiting contract points at the same NPC, one
    /// deposited polaroid pays out all of them (the kill produced one body, multiple
    /// bounties on the same head collapse into a single piece of evidence). Contracts on
    /// different NPCs stay untouched. Returns null on an ambiguous or empty group.
    /// </summary>
    private static System.Collections.Generic.List<BountyContract>? GroupSameTargetAwaiting(
        HitmanPhone.Persistence.BountySaveData save, int excludeCount)
    {
        string? sharedNpcId = null;
        var group = new System.Collections.Generic.List<BountyContract>();
        for (int i = 0; i < save.Active.Count; i++)
        {
            var c = save.Active[i];
            if (c.Status != EBountyStatus.Active || !c.AwaitingDrop) continue;
            if (string.IsNullOrEmpty(c.TargetNpcId)) continue;
            if (sharedNpcId == null) sharedNpcId = c.TargetNpcId;
            else if (!string.Equals(sharedNpcId, c.TargetNpcId, StringComparison.OrdinalIgnoreCase))
            {
                Mod.Log.Warn($"Receipt: {save.Active.Count} contracts awaiting dead-drop " +
                             "with mixed TargetNpcId — cross-session polaroid is ambiguous, refused.");
                return null;
            }
            group.Add(c);
        }
        return group.Count > 0 ? group : null;
    }

    /// <summary>
    /// Enumerate storage contents defensively. The wrapper type of GetAllItems()
    /// resists static naming (CS0234 despite Il2Cppmscorlib being referenced),
    /// so we duck-type via reflection: Count/Length property + get_Item indexer.
    /// A mismatch logs the real type name instead of crashing the postfix.
    /// </summary>
    private static System.Collections.Generic.IList<S1ItemInstance>? GetAllItemsSafe(S1StorageEntity entity)
    {
        try
        {
            var raw = entity.GetAllItems();
            if (raw == null) return null;

            var countProp = raw.GetType().GetProperty("Count") ?? raw.GetType().GetProperty("Length");
            if (countProp == null)
            {
                Mod.Log.Warn($"[Receipt] GetAllItems type exposes no Count/Length: {raw.GetType().FullName}.");
                return null;
            }
            int count = Convert.ToInt32(countProp.GetValue(raw));

            var getItem = raw.GetType().GetMethod("get_Item");
            if (getItem == null)
            {
                Mod.Log.Warn($"[Receipt] GetAllItems type exposes no indexer: {raw.GetType().FullName}.");
                return null;
            }

            var result = new System.Collections.Generic.List<S1ItemInstance>(count);
            for (int i = 0; i < count; i++)
            {
                var item = getItem.Invoke(raw, new object?[] { i }) as S1ItemInstance;
                result.Add(item);
            }
            return result;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Receipt] GetAllItems failed: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Validate one scanned item against the active contracts and pay out on a
    /// match. Returns the list of contracts that were paid (empty list = miss).
    ///
    /// Match strategy:
    ///   - exact: polaroid Value matches a contract's TargetNpcInstanceId → that one.
    ///   - fallback: Value==0 (cross-session loss) → all awaiting contracts on the
    ///     same TargetNpcId. Mixed NPC ids are refused (ambiguous) and logged.
    ///
    /// The caller is responsible for issuing side effects (IssueReward, journal
    /// completion, heat drop, cooldown) per contract and for consuming ONE polaroid
    /// for the whole matched group (a single piece of evidence covers all bounties
    /// on the same head — the kill produced one body).
    /// </summary>
    public static System.Collections.Generic.List<BountyContract>? TryValidateAndPay(
        S1DeadDrop drop, S1StorageEntity entity, S1ItemInstance item)
    {
        if (drop == null || entity == null || item == null) return null;
        if (!IsPolaroid(item)) return null;

        if (Mod.Instance?.Save == null) return null;
        var save = Mod.Instance.Save;

        // Read the polaroid's encoded target id (Unity instance id of the NPC).
        // v0.1.2: Value==0 no longer aborts — a save reload can lose the value,
        // and the single-awaiting-contract fallback below is safe to try.
        // v0.2.5 (bug-ludwig-double-2026-09-13): the old fallback refused when >1
        // contract was awaiting, but Crow can offer multiple bounties on the same
        // NPC across days. We collapse them onto the polaroid only if every waiting
        // contract targets the same NPC id.
        int targetInstanceId = ReadIntValue(item);
        if (targetInstanceId == 0)
        {
            Mod.Log.Warn("Receipt: polaroid Value=0 (lost across save reload?) — relying on evidence fallback.");
        }

        BountyContract? match = null;

        // Exact match: encoded instance id still alive.
        if (targetInstanceId != 0)
        {
            for (int i = 0; i < save.Active.Count; i++)
            {
                var c = save.Active[i];
                if (c.Status != EBountyStatus.Active) continue;
                if (c.TargetNpcInstanceId != 0 && c.TargetNpcInstanceId == targetInstanceId)
                {
                    match = c; break;
                }
            }

            // Live resolve by id (instance id cached but live re-roll diverges).
            if (match == null)
            {
                for (int i = 0; i < save.Active.Count; i++)
                {
                    var c = save.Active[i];
                    if (c.Status != EBountyStatus.Active) continue;
                    if (string.IsNullOrEmpty(c.TargetNpcId)) continue;
                    try
                    {
                        var live = S1NPCManager.GetNPC(c.TargetNpcId);
                        if (live != null && live.GetInstanceID() == targetInstanceId)
                        {
                            match = c; break;
                        }
                    }
                    catch { /* ignore */ }
                }
            }
        }

        // Session-stable fallback (BUGFIX 2026-08-31, refined 2026-09-13):
        // Unity InstanceIDs are re-rolled on every game start, so a polaroid saved
        // in a previous session carries a dead ID. If every Active contract that's
        // awaiting its dead-drop targets the same NPC, the polaroid pays out all
        // of them at once — Crow can stack multiple bounties on one head, but
        // they collapse onto the single piece of evidence the kill produced.
        if (match == null)
        {
            var group = GroupSameTargetAwaiting(save, excludeCount: 0);
            if (group == null)
            {
                Interlocked.Increment(ref _receiptsMismatched);
                Mod.Log.Warn($"Receipt: no active bounty for target instance id {targetInstanceId}.");
                return null;
            }
            Mod.Log.Info($"Receipt: matched via evidence fallback (cross-session polaroid) " +
                         $"→ {group.Count} contract(s) on '{group[0].TargetNpcId}'.");
            return group;
        }

        return new System.Collections.Generic.List<BountyContract> { match };
    }

    /// <summary>
    /// Hand the contract's <c>RewardCash</c> to the player as physical cash via
    /// <see cref="S1API.Money.Money.ChangeCashBalance(float, bool, bool)"/>.
    ///
    /// v0.1.9: bounty pays DIRTY CASH, not an online transfer (Dominik: "Kopfgeld
    /// ist im echten Leben dreckiges Geld"). Fitting side effect: cash leaves no
    /// transaction record in the banking ledger — no paper trail. The 0.1.8
    /// CreateOnlineTransaction path (typed S1API call) is kept in git history.
    ///
    /// Audit H4 (2026-09-01): returns true only on a confirmed payout so the
    /// caller can hold the contract transition until the money actually moved.
    /// </summary>
    private static bool IssueReward(BountyContract contract)
    {
        try
        {
            if (S1MoneyManager.Instance == null)
            {
                Mod.Log.Warn("MoneyManager.Instance is null (no gameplay MoneyManager yet?) — payout skipped.");
                return false;
            }

            S1API.Money.Money.ChangeCashBalance(
                contract.RewardCash, visualizeChange: true, playCashSound: true);

            Interlocked.Increment(ref _payoutsIssued);
            Interlocked.Add(ref _payoutAmountIssuedTotal, (int)Math.Round(contract.RewardCash));
            Mod.Log.Info($"[Bounty#{contract.Id}] Payout ${contract.RewardCash} in CASH via ChangeCashBalance " +
                         "(dirty money — no ledger record).");
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"IssueReward failed: {ex}");
            return false;
        }
    }

    /// <summary>
    /// v0.1.8: evidence is consumed on payout via <c>ItemSlot.ClearStoredInstance()</c>
    /// (the same soft-removal path S1API's <c>RemoveAllOfDefinition</c> uses; no world
    /// item spawns). Before this, the polaroid stayed in the drop forever and every
    /// later storage write re-scanned it (live log 2026-09-01: repeat scans finding
    /// "no payable polaroid (2 items)").
    /// Scoped to a SINGLE slot: prefer the polaroid encoding this contract's target
    /// instance id; fall back to the first polaroid (Value==0 cross-session case).
    /// Never clears the whole drop — a second awaiting contract's evidence survives.
    /// </summary>
    private static void ConsumePolaroids(S1StorageEntity entity, int targetInstanceId)
    {
        try
        {
            var slots = entity.ItemSlots;
            if (slots == null) return;
            int matchedIdx = -1;
            int firstPolaroidIdx = -1;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot == null) continue;
                var inst = slot.ItemInstance;
                if (inst == null) continue;
                if (!IsPolaroid(inst)) continue;
                if (firstPolaroidIdx < 0) firstPolaroidIdx = i;
                if (targetInstanceId != 0)
                {
                    try
                    {
                        if (ReadIntValue(inst) == targetInstanceId) { matchedIdx = i; break; }
                    }
                    catch { }
                }
            }
            int consumeIdx = matchedIdx >= 0 ? matchedIdx : firstPolaroidIdx;
            if (consumeIdx >= 0)
            {
                slots[consumeIdx].ClearStoredInstance();
                Mod.Log.Info(matchedIdx >= 0
                    ? $"[Receipt] Consumed polaroid for target instance {targetInstanceId} — evidence destroyed."
                    : "[Receipt] Consumed 1 polaroid (no encoded id match — cross-session fallback) — evidence destroyed.");
            }
            else
            {
                Mod.Log.Warn("[Receipt] payout done but no polaroid slot found to consume — it stays in the drop.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"ConsumePolaroids failed (polaroid stays in drop): {ex.Message}");
        }
    }

    /// <summary>
    /// Reflection-based check: does the inserted item have a Definition whose ID
    /// matches our polaroid id? ItemInstance does not expose a Definition directly
    /// in the stub, but the property <c>Definition</c> lives on the base class.
    /// </summary>
    private static bool IsPolaroid(S1ItemInstance item)
    {
        try
        {
            var def = item.GetType().GetProperty("Definition",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);
            object? definition = def?.GetValue(item);
            if (definition == null) return false;
            var idProp = definition.GetType().GetProperty("ID",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);
            object? id = idProp?.GetValue(definition);
            if (id is string s)
            {
                if (string.Equals(s, BountyEvidenceItemRegistry.ItemId, StringComparison.OrdinalIgnoreCase))
                    return true;
                Mod.Log.Debug($"[Receipt] Item definition id '{s}' does not match polaroid '{BountyEvidenceItemRegistry.ItemId}'.");
            }
            return false;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"IsPolaroid reflection failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>Read <c>IntegerItemInstance.Value</c> via the typed property.</summary>
    private static int ReadIntValue(S1ItemInstance item)
    {
        try
        {
            if (item is S1IntegerItemInstance integer)
            {
                return integer.Value;
            }
            // Fallback: reflection on .Value (less common but safe).
            var prop = item.GetType().GetProperty("Value",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);
            object? v = prop?.GetValue(item);
            return v is int i ? i : 0;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"ReadIntValue failed: {ex.Message}");
            return 0;
        }
    }

    /// <summary>
    /// Reconstruct a caller index from the CallerId string "caller_ghost" → 0 etc.
    /// Returns -1 when the id is not one of our known callers.
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
}

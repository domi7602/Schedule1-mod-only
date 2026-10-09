using System;
using System.Threading;
using HitmanPhone.Items;
using HitmanPhone.Persistence;
using MelonLoader;
using S1Mods.Shared;

#if (IL2CPPMELON)
using S1DeadDrop = Il2CppScheduleOne.Economy.DeadDrop;
using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
using S1ItemInstance = Il2CppScheduleOne.ItemFramework.ItemInstance;
using S1IntegerItemInstance = Il2CppScheduleOne.ItemFramework.IntegerItemInstance;
using S1MoneyManager = Il2CppScheduleOne.Money.MoneyManager;
#elif MONOMELON
using S1DeadDrop = ScheduleOne.Economy.DeadDrop;
using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
using S1ItemInstance = ScheduleOne.ItemFramework.ItemInstance;
using S1IntegerItemInstance = ScheduleOne.ItemFramework.IntegerItemInstance;
using S1MoneyManager = ScheduleOne.Money.MoneyManager;
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
///   1. Read the integer <c>Value</c> and identify the item format.
///   2. Match v2 photos by persistent evidence token; match legacy photos only
///      to active migrated contracts that retain their original evidence ID.
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
    /// pays and consumes. Consolidated 2026-09-15 in
    /// S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer — unified to fail-closed
    /// (previously fail-open here): on an authority exception without own save
    /// the payout is now aborted instead of blindly continuing.
    /// The MONOMELON branch was removed — the workspace build is exclusively IL2CPP.
    /// </summary>
    private static bool IsHostOrSingleplayer() => NetworkGuard.IsHostOrSingleplayer();

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
            int encodedId = ReadIntValue(item);
            bool isLegacyItem = IsLegacyPolaroid(item);
            var save = Mod.Instance?.Save;
            if (save == null) return;
            bool anyPayoutFailed = false;
            bool anyPayout = false;
            for (int m = 0; m < matches.Count; m++)
            {
                var match = matches[m];
                // First confirmed sighting of the polaroid anywhere (inventory or
                // this drop) starts the post-photo drop window (two-phase
                // deadlines). Idempotent, legacy contracts unaffected.
                BountyService.EnsureDropWindowStarted(match);
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
                save.LastUsedDropId = dropGuid; // next offer avoids this drop (variety)
                save.Active.Remove(match);
                save.History.Add(match);
                anyPayout = true;
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
            if (anyPayout && !anyPayoutFailed)
            {
                BountyPersistence.PersistCurrent();
                // One polaroid covers the whole matched group (single kill → single evidence).
                ConsumePolaroids(entity, encodedId, isLegacyItem);
            }
            else if (!anyPayout)
            {
                // Every matched contract was rejected (wrong assigned drop): the
                // polaroid STAYS where it is — no payout AND no evidence loss
                // (plan constraint; the player can move it to the assigned drop).
                Mod.Log.Info($"[Receipt] polaroid kept in '{dropName}' — matched contract(s) require their assigned drop.");
            }
            return; // one payout-group per storage event
        }
        Mod.Log.Debug($"[Receipt] scan of '{dropName}' found no payable polaroid ({items.Count} items).");
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

        // The item definition identifies its format: v2 values are durable tokens;
        // legacy values are old session-specific Unity IDs (or zero after data loss).
        int encodedValue = ReadIntValue(item);
        bool isLegacyItem = IsLegacyPolaroid(item);
        var matches = BountyEvidenceMatcher.Match(save, encodedValue, isLegacyItem);
        if (matches == null || matches.Count == 0)
        {
            Interlocked.Increment(ref _receiptsMismatched);
            Mod.Log.Warn($"Receipt: no active contract matches {(isLegacyItem ? "legacy instance id" : "evidence token")} {encodedValue}.");
            return null;
        }
        if (isLegacyItem && encodedValue == 0)
        {
            Mod.Log.Warn("Receipt: accepting a zero-value legacy polaroid only for its migrated active contract.");
        }
        return matches;
    }

    /// <summary>
    /// Hand the contract's <c>RewardCash</c> to the player as physical cash via
    /// <see cref="S1API.Money.Money.ChangeCashBalance(float, bool, bool)"/>.
    ///
    /// v0.1.9: bounty pays DIRTY CASH, not an online transfer (Dominik: "Kopfgeld
    /// ist im echten Leben dreckiges Geld" — bounty money is dirty money in real
    /// life). Fitting side effect: cash leaves no
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

    /// <summary>Consumes only the exact photo that was just validated.</summary>
    private static void ConsumePolaroids(S1StorageEntity entity, int encodedValue, bool isLegacyItem)
    {
        try
        {
            var slots = entity.ItemSlots;
            if (slots == null) return;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot == null) continue;
                var inst = slot.ItemInstance;
                if (inst == null || !IsPolaroid(inst)) continue;
                if (IsLegacyPolaroid(inst) != isLegacyItem) continue;
                if (ReadIntValue(inst) != encodedValue) continue;

                slot.ClearStoredInstance();
                Mod.Log.Info($"[Receipt] Consumed polaroid evidence value {encodedValue}.");
                return;
            }
            Mod.Log.Warn("[Receipt] payout done but the validated polaroid slot was not found to consume.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"ConsumePolaroids failed (polaroid stays in drop): {ex.Message}");
        }
    }

    /// <summary>
    /// Removes every polaroid encoding this contract's target instance id from all
    /// registered dead drops. Polaroids of other targets are never touched.
    /// Callers own the host and shared-target guards (BountyService.RemovePolaroidFromDeadDrops).
    /// </summary>
    internal static void RemoveDeadDropPolaroids(BountyContract c, string reason)
    {
        var drops = S1DeadDrop.DeadDrops;
        if (drops == null) return;
        int removed = 0;
        for (int d = 0; d < drops.Count; d++)
        {
            var drop = drops[d];
            if (drop == null || drop.Pointer == IntPtr.Zero || drop.WasCollected) continue;
            var entity = drop.Storage;
            if (entity == null || entity.Pointer == IntPtr.Zero || entity.WasCollected) continue;
            var slots = entity.ItemSlots;
            if (slots == null) continue;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot == null) continue;
                var inst = slot.ItemInstance;
                if (inst == null || !MatchesEvidenceForContract(c, inst)) continue;
                slot.ClearStoredInstance();
                removed++;
            }
        }
        if (removed > 0)
        {
            Mod.Log.Info($"[Bounty#{c.Id}] Removed {removed} polaroid(s) from dead-drop storage ({reason}).");
        }
    }

    /// <summary>
    /// Reflection-based check: does the inserted item have a Definition whose ID
    /// matches our polaroid id? ItemInstance does not expose a Definition directly
    /// in the stub, but the property <c>Definition</c> lives on the base class.
    /// </summary>
    internal static bool IsPolaroid(S1ItemInstance item)
    {
        string? id = ReadDefinitionId(item);
        bool recognized = string.Equals(id, BountyEvidenceItemRegistry.ItemId, StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(id, BountyEvidenceItemRegistry.LegacyItemId, StringComparison.OrdinalIgnoreCase);
        if (!recognized && !string.IsNullOrEmpty(id))
            Mod.Log.Debug($"[Receipt] Item definition id '{id}' is not a bounty polaroid.");
        return recognized;
    }

    internal static bool IsLegacyPolaroid(S1ItemInstance item) =>
        string.Equals(ReadDefinitionId(item), BountyEvidenceItemRegistry.LegacyItemId, StringComparison.OrdinalIgnoreCase);

    internal static bool MatchesEvidenceForContract(BountyContract contract, S1ItemInstance item)
    {
        if (contract == null || item == null || !IsPolaroid(item)) return false;
        int value = ReadIntValue(item);
        if (IsLegacyPolaroid(item))
        {
            int legacyId = contract.LegacyTargetInstanceId;
            if (legacyId == 0 && contract.EvidenceToken <= 0)
                legacyId = contract.TargetNpcInstanceId;
            return legacyId != 0 && value == legacyId;
        }
        return contract.EvidenceToken > 0 && value == contract.EvidenceToken;
    }

    private static string? ReadDefinitionId(S1ItemInstance item)
    {
        try
        {
            var def = item.GetType().GetProperty("Definition",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);
            object? definition = def?.GetValue(item);
            if (definition == null) return null;
            var idProp = definition.GetType().GetProperty("ID",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);
            return idProp?.GetValue(definition) as string;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Polaroid definition lookup failed: {ex.Message}");
            return null;
        }
    }

    /// <summary>Read <c>IntegerItemInstance.Value</c> via the typed property.</summary>
    internal static int ReadIntValue(S1ItemInstance item)
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

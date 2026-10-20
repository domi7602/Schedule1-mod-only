using System;
using System.Reflection;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Quests;

#if (IL2CPPMELON)
using S1Quest = Il2CppScheduleOne.Quests.Quest;
#elif MONOMELON
using S1Quest = ScheduleOne.Quests.Quest;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase H — Journal Adapter.
///
/// The actual <see cref="S1Quest"/> transition is exposed by the base S1API Quest
/// via Complete()/Fail()/Expire() — we use it directly without wrapping it in
/// QuestWrapper.
///
/// v0.3.0 (HP-06 / HP-07): resolution and cleanup were rebuilt around a stable,
/// deterministic quest GUID, verified against the actual S1API 3.2.1 source:
///   • QuestManager.CreateQuest&lt;T&gt;(guid) IGNORES its guid argument.
///   • QuestManager.GetQuestByGuid(guid) matches S1Quest.StaticGUID.
///   • The game persists each quest's StaticGUID and QuestPatches restores it on
///     load (active quests only), so StaticGUID is the durable quest identity.
/// The bridge therefore derives a GUID deterministically from the contract id
/// (no new save field), writes it onto the quest at creation, and resolves by
/// GUID first. Resolution order:
///   1. session dictionary (instances we created this session — the in-session
///      tie-breaker when two contracts share one target NPC),
///   2. QuestManager.GetQuestByGuid(deterministic guid) — restored quests,
///   3. GetQuestByName(reconstructed title) — legacy quests created before the
///      GUID binding existed; the occupancy guard (IsQuestBoundToOtherContract)
///      stops a same-target sibling from stealing the owner's quest,
///   4. fresh creation (rebind pass only).
/// An unbound quest is NOT adopted and NOT treated as an orphan: a wrong binding
/// or a fabricated completion is worse than none (HP-07). The session dictionary
/// is cleared on every save load (ResetSessionCache).
///
/// HP-06: objectives are addressed structurally (index 0 elimination, index 1
/// dead drop), never by a display string — the first objective embeds the target
/// name and cannot be matched by a fixed literal.
///
/// Skill reference: <c>schedule1-game-systems/references/03-Dialogue-Quest.md</c>.
/// </summary>
public static class BountyJournalBridge
{
    internal static string? CreatingContractId;

    private const long RebindRetryIntervalMs = 500;
    private const int MaxRebindAttempts = 8;
    private static int _rebindAttemptsRemaining;
    private static long _nextRebindAt;

    /// <summary>
    /// v0.2.6: true only while RebindActiveQuests iterates. Gates the fresh-quest
    /// fallback (documented resolution step 4) so lookups from gameplay paths
    /// (CompleteQuest/AdvanceToDeadDrop/...) never CREATE quests as a side effect.
    /// </summary>
    private static bool _inRebindPass;

    /// <summary>Session-scoped quest instances keyed by contract id. Cleared on save load.</summary>
    private static readonly System.Collections.Generic.Dictionary<string, Quest> SessionQuests = new();

    /// <summary>
    /// v0.2.6 occupancy check: true when the quest instance is already bound to a
    /// DIFFERENT contract. Only HitmanPhone calls InitContractId, so the session
    /// dictionary is the authoritative binding record — no reflection needed.
    /// </summary>
    private static bool IsQuestBoundToOtherContract(Quest quest, string contractId)
    {
        foreach (var kv in SessionQuests)
        {
            if (ReferenceEquals(kv.Value, quest) && kv.Key != contractId) return true;
        }
        return false;
    }

    /// <summary>True when the quest instance is bound to ANY contract this session.</summary>
    private static bool IsQuestBoundToAnyContract(Quest quest)
    {
        foreach (var kv in SessionQuests)
        {
            if (ReferenceEquals(kv.Value, quest)) return true;
        }
        return false;
    }

    /// <summary>
    /// v0.2.6: enumerate every live managed BountyQuest wrapper via the internal
    /// static QuestManager.Quests list (plain managed List&lt;Quest&gt; — reflection
    /// is safe and cheap; called only during the bounded rebind window).
    /// Returns an empty list on any reflection failure — callers then fall back
    /// to fresh quest registration.
    /// </summary>
    private static System.Collections.Generic.List<BountyQuest> EnumerateLiveBountyQuests()
    {
        var result = new System.Collections.Generic.List<BountyQuest>();
        try
        {
            var field = typeof(QuestManager).GetField("Quests",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public);
            if (field == null) return result;
            if (field.GetValue(null) is not System.Collections.IEnumerable raw) return result;
            foreach (var o in raw)
            {
                if (o is BountyQuest bq) result.Add(bq);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] EnumerateLiveBountyQuests failed: {ex.Message}");
        }
        return result;
    }

    /// <summary>
    /// Stable, deterministic quest GUID for a contract. S1API 3.2.1's
    /// QuestManager.CreateQuest ignores the string it is handed (verified against
    /// the 3.2.1 source) and GetQuestByGuid matches S1Quest.StaticGUID, which the
    /// game persists per quest and QuestPatches restores on load. Deriving a
    /// well-formed GUID from the contract id and writing it onto the quest at
    /// creation makes the binding both stable across sessions and resolvable by
    /// GUID — no new save field, no display-text match.
    /// </summary>
    internal static string ComputeContractQuestGuid(string contractId)
    {
        // FNV-1a (two lanes) -> 16 deterministic bytes -> a valid GUID string.
        const ulong prime = 0x100000001B3UL;
        ulong a = 0xCBF29CE484222325UL;
        ulong b = 0x84222325CBF29CE4UL;
        string key = "HitmanPhone.Bounty.Quest|" + (contractId ?? string.Empty);
        for (int i = 0; i < key.Length; i++)
        {
            a = (a ^ key[i]) * prime;
            b = (b ^ (key[i] + 0x9E3779B97F4A7C15UL)) * prime;
        }
        var bytes = new byte[16];
        for (int i = 0; i < 8; i++) bytes[i] = (byte)(a >> (8 * i));
        for (int i = 0; i < 8; i++) bytes[8 + i] = (byte)(b >> (8 * i));
        return new Guid(bytes).ToString();
    }

    /// <summary>The native S1Quest behind an S1API wrapper (internal field → reflection).</summary>
    private static object? GetNativeQuest(Quest wrapper)
    {
        try
        {
            return typeof(Quest).GetField("S1Quest",
                BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(wrapper);
        }
        catch { return null; }
    }

    /// <summary>Write the deterministic GUID onto the quest so save/load keeps the binding.</summary>
    private static bool TryBindPersistentGuid(Quest wrapper, string guid)
    {
        try
        {
            var native = GetNativeQuest(wrapper);
            if (native == null) return false;
            var t = native.GetType();
            var prop = t.GetProperty("StaticGUID", BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite) { prop.SetValue(native, guid); return true; }
            var field = t.GetField("StaticGUID",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) { field.SetValue(native, guid); return true; }
            Mod.Log.Warn("[Journal] Quest StaticGUID is not writable; GUID binding unavailable.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] TryBindPersistentGuid failed: {ex.Message}");
        }
        return false;
    }

    /// <summary>Read the persisted quest GUID (null when the quest carries none).</summary>
    private static string? TryReadPersistentGuid(Quest wrapper)
    {
        try
        {
            var native = GetNativeQuest(wrapper);
            if (native == null) return null;
            var t = native.GetType();
            var prop = t.GetProperty("StaticGUID", BindingFlags.Public | BindingFlags.Instance);
            if (prop != null) return prop.GetValue(native) as string;
            var field = t.GetField("StaticGUID",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) return field.GetValue(native) as string;
        }
        catch { /* best-effort */ }
        return null;
    }

    /// <summary>
    /// v0.2.6: the vanilla quest title shown in the journal is a SNAPSHOT taken
    /// in the S1API Quest constructor — InitContractId only personalises the
    /// managed Title getter, so an adopted quest would keep rendering the
    /// generic "Hitman Contract" forever. Rewrite the snapshot after adoption.
    /// (S1Quest is internal to S1API → reflection; best-effort, cosmetic only.)
    /// </summary>
    private static void TryRefreshDisplayTitle(Quest wrapper, BountyContract contract)
    {
        // v0.2.7: replaced by BountyQuest.SyncDisplayTitle() — calls into the quest
        // so the public Title setter / InitializeQuest path triggers the UI event.
        // Kept as a no-op stub because BountyJournalBridge is the single point that
        // *prompts* a refresh; keeping the call sites explicit avoids future drift.
        if (wrapper is BountyQuest bq) { try { bq.SyncDisplayTitle(); } catch { /* best-effort */ } }
    }

    /// <summary>Called from SaveStateGuard.OnLoadComplete — restored quests are new objects.</summary>
    public static void ResetSessionCache()
    {
        SessionQuests.Clear();
        Mod.Log.Info("[Journal] Session quest cache cleared (save load).");
    }

    /// <summary>
    /// Rebind all active contracts to the freshly restored journal quest objects.
    /// The contract data survives a save reload, but BountyQuest._contractId is
    /// runtime-only and the old quest instances are destroyed with the scene.
    /// </summary>
    public static bool RebindActiveQuests()
    {
        var save = Mod.Instance?.Save;
        if (save == null) return false;

        int rebound = 0;
        int activeCount = 0;
        _inRebindPass = true;
        try
        {
            for (int i = 0; i < save.Active.Count; i++)
            {
                var contract = save.Active[i];
                if (contract.Status != EBountyStatus.Active) continue;
                activeCount++;

                var quest = ResolveQuest(contract.Id);
                if (quest != null)
                {
                    rebound++;
                    // HP-07: rebuild the whole journal phase from the contract — a
                    // contract in its delivery phase must not reappear with the
                    // default (execution) objectives.
                    ApplyJournalPhase(quest, IsDropPhase(contract));
                    Mod.Log.Info($"[Journal] Rebound active contract '{contract.Id}' " +
                                 $"to target '{contract.TargetNpcName ?? contract.TargetNpcId}'.");
                }
                else
                {
                    // v0.2.6 log damping: during the deferred retry window only the
                    // first and the final attempt log per-contract warns — the old
                    // behaviour repeated the full 5-line block every 500 ms.
                    if (_rebindAttemptsRemaining <= 1 || _rebindAttemptsRemaining >= MaxRebindAttempts)
                    {
                        Mod.Log.Warn($"[Journal] Could not rebind active contract '{contract.Id}' " +
                                     $"for target '{contract.TargetNpcName ?? contract.TargetNpcId}'.");
                    }
                }
            }
        }
        finally
        {
            _inRebindPass = false;
        }

        Mod.Log.Info($"[Journal] Active quest rebind complete: {rebound} of {activeCount} active contract(s).");
        bool complete = rebound == activeCount;
        if (complete)
        {
            _rebindAttemptsRemaining = 0;
            CleanupOrphanBountyQuests();
        }
        return complete;
    }

    /// <summary>
    /// Bring restored/stale bounty quests back in line with the contract record.
    ///
    /// HP-07: an unbound quest is NOT automatically an orphan, and a quest must
    /// never be reported as a successful completion that never happened. Only a
    /// quest whose persistent GUID maps to a real contract is touched, and it then
    /// receives exactly that contract's TRUE terminal state. A quest that maps to
    /// no contract is left alone — the explicit '/hitman_cleanup' command handles
    /// genuine leftovers.
    /// </summary>
    public static void CleanupOrphanBountyQuests()
    {
        try
        {
            var liveQuests = EnumerateLiveBountyQuests();
            for (int i = 0; i < liveQuests.Count; i++)
            {
                var q = liveQuests[i];
                if (q == null || IsQuestBoundToAnyContract(q)) continue;
                var contract = FindContractByQuestGuid(q);
                if (contract != null) ApplyContractState(q, contract);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] CleanupOrphanBountyQuests failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Apply a mapped contract's TRUE terminal state to its restored quest. Never
    /// invents a success: an Active/Offered contract is owned by the rebind pass,
    /// not by cleanup.
    /// </summary>
    private static void ApplyContractState(Quest quest, BountyContract contract)
    {
        try
        {
            switch (contract.Status)
            {
                case EBountyStatus.Completed:
                    Mod.Log.Info($"[Journal] Restored quest matches completed contract '{contract.Id}'.");
                    quest.Complete();
                    break;
                case EBountyStatus.Failed:
                    quest.Fail();
                    break;
                case EBountyStatus.Expired:
                    quest.Expire();
                    break;
                case EBountyStatus.Forfeited:
                    quest.Cancel();
                    break;
                default:
                    break;   // Active/Offered: the rebind pass owns these
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"[Journal] ApplyContractState failed: {ex.Message}");
        }
    }

    /// <summary>Reverse lookup: the contract whose deterministic GUID this quest carries.</summary>
    private static BountyContract? FindContractByQuestGuid(Quest quest)
    {
        string? guid = TryReadPersistentGuid(quest);
        if (string.IsNullOrEmpty(guid)) return null;
        var save = Mod.Instance?.Save;
        if (save == null) return null;
        for (int i = 0; i < save.Active.Count; i++)
            if (save.Active[i] != null && ComputeContractQuestGuid(save.Active[i].Id) == guid) return save.Active[i];
        for (int i = 0; i < save.History.Count; i++)
            if (save.History[i] != null && ComputeContractQuestGuid(save.History[i].Id) == guid) return save.History[i];
        return null;
    }

    /// <summary>
    /// Quest restoration can finish after GameLifecycle.OnLoadComplete. Schedule
    /// a small bounded retry window instead of polling every frame forever.
    /// </summary>
    public static void ScheduleRebindRetries()
    {
        _rebindAttemptsRemaining = MaxRebindAttempts;
        _nextRebindAt = 0;
        Mod.Log.Info($"[Journal] Quest rebind deferred: up to {MaxRebindAttempts} retries.");
    }

    /// <summary>Called from Mod.OnUpdate; cheap and inactive outside a retry window.</summary>
    public static void TickRebind()
    {
        if (_rebindAttemptsRemaining <= 0) return;
        long now = Environment.TickCount64;
        if (now < _nextRebindAt) return;

        if (RebindActiveQuests())
        {
            _rebindAttemptsRemaining = 0;
            Mod.Log.Info("[Journal] Deferred quest rebind succeeded.");
            return;
        }

        _rebindAttemptsRemaining--;
        _nextRebindAt = now + RebindRetryIntervalMs;
        if (_rebindAttemptsRemaining == 0)
        {
            Mod.Log.Warn("[Journal] Deferred quest rebind exhausted; cleaning up any remaining orphan quests.");
            CleanupOrphanBountyQuests();
        }
    }

    /// <summary>Reconstruct the quest title exactly like BountyQuest.Title does.</summary>
    private static string BuildTitle(BountyContract contract)
        => $"Hitman Contract: {BountyQuest.FormatName(contract.TargetNpcName ?? contract.TargetNpcId)}";

    /// <summary>Find a contract (active or history) by id, for title reconstruction.</summary>
    private static BountyContract? FindContract(string contractId)
    {
        var save = Mod.Instance?.Save;
        if (save == null) return null;
        for (int i = 0; i < save.Active.Count; i++)
            if (save.Active[i].Id == contractId) return save.Active[i];
        for (int i = 0; i < save.History.Count; i++)
            if (save.History[i].Id == contractId) return save.History[i];
        return null;
    }

    /// <summary>
    /// Register a new bounty quest with the player's journal. Idempotent per contract id.
    /// </summary>
    public static Quest? RegisterBountyQuest(BountyContract contract)
    {
        if (contract == null) return null;
        var existing = ResolveQuest(contract.Id);
        if (existing != null) return existing;
        return CreateQuestForContract(contract);
    }

    /// <summary>
    /// v0.2.6: raw creation WITHOUT the ResolveQuest dedupe. The same-target
    /// group path in ResolveQuest must call THIS for its fresh registrations —
    /// routing through RegisterBountyQuest re-enters ResolveQuest for the owner
    /// (still unbound at that point) and recurses infinitely when no restored
    /// quest exists for the group (quit-without-save after accepting 2+ offers).
    /// </summary>
    private static Quest? CreateQuestForContract(BountyContract contract)
    {
        try
        {
            CreatingContractId = contract.Id;
            // QuestManager.CreateQuest<T>(guid) registers a mod quest but IGNORES the
            // guid argument in S1API 3.2.1 — the real identity is S1Quest.StaticGUID,
            // which we bind below (see class comment).
            var q = QuestManager.CreateQuest<BountyQuest>(contract.Id);
            CreatingContractId = null;

            if (q == null)
            {
                Mod.Log.Warn($"QuestManager.CreateQuest<BountyQuest> returned null for '{contract.Id}'.");
                return null;
            }

            ((BountyQuest)q).InitContractId(contract.Id);
            // Deterministic persistent GUID — the string handed to CreateQuest is
            // ignored by S1API 3.2.1, so this is what GetQuestByGuid resolves after a
            // reload (QuestPatches restores StaticGUID together with the quest).
            TryBindPersistentGuid(q, ComputeContractQuestGuid(contract.Id));
            SessionQuests[contract.Id] = q;
            Mod.Log.Info($"[Journal] Registered bounty quest for '{contract.TargetNpcId}'.");
            return q;
        }
        catch (Exception ex)
        {
            CreatingContractId = null;
            Mod.Log.Error($"RegisterBountyQuest failed: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Fresh lookup of the live quest instance for a contract. NEVER hold the
    /// returned reference across save loads.
    /// </summary>
    private static Quest? ResolveQuest(string contractId)
    {
        if (string.IsNullOrEmpty(contractId)) return null;

        // 1) Session instance we created ourselves — authoritative in-session.
        if (SessionQuests.TryGetValue(contractId, out var sessionQuest)) return sessionQuest;

        var contract = FindContract(contractId);
        if (contract == null)
        {
            Mod.Log.Warn($"[Journal] ResolveQuest: no contract '{contractId}' in save (Active/History).");
            return null;
        }

        // 2) Stable deterministic GUID binding. QuestManager.GetQuestByGuid matches
        //    S1Quest.StaticGUID (S1API 3.2.1); we wrote that GUID at creation, so the
        //    same lookup resolves the restored quest after a save load — with no
        //    dependence on a display title.
        try
        {
            if (QuestManager.GetQuestByGuid(ComputeContractQuestGuid(contractId)) is BountyQuest byGuid)
            {
                if (IsQuestBoundToOtherContract(byGuid, contractId))
                {
                    Mod.Log.Debug($"[Journal] GUID quest for '{contractId}' already bound elsewhere — leaving it alone.");
                }
                else
                {
                    byGuid.InitContractId(contractId);
                    SessionQuests[contractId] = byGuid;
                    Mod.Log.Info($"[Journal] Bound contract '{contractId}' via persistent quest GUID.");
                    return byGuid;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] ResolveQuest(guid) failed: {ex.Message}");
        }

        // 3) Title lookup fallback — legacy quests created before the GUID binding
        //    existed, or a restored quest the game titled before we adopted it.
        //    Occupancy guard: in a same-target group the owner's quest personalises
        //    its title the moment it is adopted, and every sibling reconstructs the
        //    SAME title; without this guard a sibling would steal the owner's quest
        //    (the M3 wrong-binding bug through the back door).
        string title = BuildTitle(contract);
        try
        {
            if (QuestManager.GetQuestByName(title) is BountyQuest byTitle)
            {
                if (IsQuestBoundToOtherContract(byTitle, contractId))
                {
                    Mod.Log.Debug($"[Journal] Title quest '{title}' already bound to another contract — " +
                                  "leaving it alone (same-target group sibling).");
                }
                else
                {
                    byTitle.InitContractId(contractId);
                    SessionQuests[contractId] = byTitle;
                    Mod.Log.Info($"[Journal] Adopted live quest by title '{title}'.");
                    return byTitle;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] ResolveQuest(title='{title}') failed: {ex.Message}");
        }

        // 4) Fresh creation (rebind pass only). An unbound quest is NOT orphaned, so
        //    we never adopt an arbitrary BountyQuest here (HP-07) — a wrong binding
        //    is worse than none.
        if (_inRebindPass)
        {
            var fresh = CreateQuestForContract(contract);
            if (fresh != null)
            {
                Mod.Log.Info($"[Journal] Fresh quest registered for unbound contract '{contractId}'.");
                return fresh;
            }
        }

        return null;
    }

    /// <summary>
    /// Mark the bounty quest Complete (polaroid dropped, reward cleared).
    /// </summary>
    public static void CompleteQuest(string contractId)
    {
        var q = ResolveQuest(contractId);
        if (q == null)
        {
            Mod.Log.Warn($"[Journal] CompleteQuest: no live quest for '{contractId}'.");
            return;
        }
        try
        {
            q.Complete();
            Mod.Log.Info($"[Journal] Bounty '{contractId}' → Completed.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"CompleteQuest '{contractId}' failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Mark the bounty quest Failed or Expired (player death or 3-day timeout).
    /// </summary>
    public static void FailQuest(string contractId, string reason)
    {
        var q = ResolveQuest(contractId);
        if (q == null)
        {
            Mod.Log.Warn($"[Journal] FailQuest: no live quest for '{contractId}'.");
            return;
        }
        try
        {
            if (reason == "expired") q.Expire();
            else q.Fail();
            Mod.Log.Info($"[Journal] Bounty '{contractId}' → {reason}.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"FailQuest '{contractId}' failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Cancel a quest — used when a caller drops off cooldown after revocation.
    /// </summary>
    public static void CancelQuest(string contractId)
    {
        var q = ResolveQuest(contractId);
        if (q == null)
        {
            Mod.Log.Warn($"[Journal] CancelQuest: no live quest for '{contractId}'.");
            return;
        }
        try
        {
            q.Cancel();
            Mod.Log.Info($"[Journal] Bounty '{contractId}' → Cancelled.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"CancelQuest '{contractId}' failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Advance the quest to the second objective after picking up the polaroid.
    /// </summary>
    public static void AdvanceToDeadDrop(string contractId)
    {
        var q = ResolveQuest(contractId);
        if (q == null)
        {
            Mod.Log.Warn($"[Journal] AdvanceToDeadDrop: no live quest for '{contractId}'.");
            return;
        }
        ApplyJournalPhase(q, dropPhase: true);
        Mod.Log.Info($"[Journal] Bounty '{contractId}' → Objective advanced to Dead-Drop.");
    }

    /// <summary>True once the polaroid was received: the contract is in its delivery phase.</summary>
    private static bool IsDropPhase(BountyContract contract) =>
        contract.AwaitingDrop || contract.DropDueStarted;

    /// <summary>
    /// Apply the journal objective state from the CONTRACT phase (HP-06 / HP-07).
    ///
    /// Objectives are addressed STRUCTURALLY — BountyQuest.OnCreated adds the
    /// elimination objective first (index 0) and the dead-drop objective second
    /// (index 1). The first objective embeds the target name ("Eliminate &lt;NPC&gt;
    /// and collect the polaroid"), so an exact display-string comparison can never
    /// match; the old code left the first step open while starting the second.
    /// The contract is authoritative: after a load the whole journal state is
    /// reconstructed from it rather than trusted from a stale UI snapshot.
    /// </summary>
    private static void ApplyJournalPhase(Quest quest, bool dropPhase)
    {
        try
        {
            if (quest.QuestEntries == null || quest.QuestEntries.Count < 2) return;
            var eliminate = quest.QuestEntries[0];
            var drop = quest.QuestEntries[1];
            if (eliminate == null || drop == null) return;

            if (dropPhase)
            {
                if (eliminate.State != S1API.Quests.Constants.QuestState.Completed) eliminate.Complete();
                if (drop.State == S1API.Quests.Constants.QuestState.Inactive) drop.Begin();
            }
            else if (eliminate.State == S1API.Quests.Constants.QuestState.Inactive)
            {
                eliminate.Begin();
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] ApplyJournalPhase failed: {ex.Message}");
        }
    }
}

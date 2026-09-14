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
/// BUGFIX v0.1.2 (2026-08-31): v0.1.1 resolved quests via
/// QuestManager.GetQuestByGuid(contract.Id) — but S1API treats the string passed
/// to CreateQuest as an INTERNAL ID only; it is not the lookup key. Proven
/// pattern (HomelessQuestline.cs, Gatekeeper-fix H1/L7): CreateQuest takes the
/// internal id while lookups resolve by quest TITLE. So resolution is now:
///   1. session dictionary (instances we created this session — also the
///      tie-breaker when two contracts share one target NPC),
///   2. GetQuestByName(reconstructed title) for restored quests,
///   3. GetQuestByName("Hitman Contract") — a restored quest loses its
///      transient contract id, so its dynamic Title renders the generic form
///      until we re-adopt (InitContractId) it,
///   4. re-create via RegisterBountyQuest as last resort.
/// The session dictionary is cleared on every save load (ResetSessionCache)
/// because restored quest instances are new objects.
///
/// BUGFIX v0.2.6 (2026-09-14): same-target contract groups (supported since
/// the v0.2.5 group payout) made the audit-M3 fail-safe refuse EVERY rebind:
/// with 2+ restored contracts the generic-title adoption was blocked entirely
/// and the retry window spammed warnings on every game start. Resolution is
/// now group-aware: when ALL unbound active contracts share one TargetNpcId
/// the OLDEST adopts the restored generic quest and the siblings get FRESH
/// quests (RegisterBountyQuest); mixed-target groups stay refused (genuinely
/// ambiguous — a wrong binding is worse than none) with damped logging. A
/// session-dictionary occupancy guard (IsQuestBoundToOtherContract) keeps
/// siblings from stealing the owner's freshly adopted quest via the title
/// lookup — the M3 wrong-binding bug through the back door.
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

    /// <summary>Unbound active contracts (no SessionQuests binding), oldest first.</summary>
    private static System.Collections.Generic.List<BountyContract> CollectUnboundActiveContracts()
    {
        var list = new System.Collections.Generic.List<BountyContract>();
        var save = Mod.Instance?.Save;
        if (save == null) return list;
        for (int i = 0; i < save.Active.Count; i++)
        {
            var c = save.Active[i];
            if (c.Status != EBountyStatus.Active) continue;
            if (!SessionQuests.ContainsKey(c.Id)) list.Add(c);
        }
        return list;
    }

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
    /// A restored BountyQuest renders the generic title ("Hitman Contract") exactly
    /// while its _contractId is unset — and only HitmanPhone sets it via
    /// InitContractId, so "not in the session dictionary" IS "generic/unowned".
    /// (The title itself is unreachable: protected on the S1API base, and the
    /// underlying S1Quest field is internal to the S1API assembly.)
    /// </summary>
    private static bool IsGenericBountyQuest(Quest quest) => !IsQuestBoundToAnyContract(quest);

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
    /// v0.2.6: the vanilla quest title shown in the journal is a SNAPSHOT taken
    /// in the S1API Quest constructor — InitContractId only personalises the
    /// managed Title getter, so an adopted quest would keep rendering the
    /// generic "Hitman Contract" forever. Rewrite the snapshot after adoption.
    /// (S1Quest is internal to S1API → reflection; best-effort, cosmetic only.)
    /// </summary>
    private static void TryRefreshDisplayTitle(Quest wrapper, BountyContract contract)
    {
        try
        {
            var s1q = typeof(Quest).GetField("S1Quest",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)?.GetValue(wrapper) as S1Quest;
            if (s1q == null || s1q.Pointer == System.IntPtr.Zero) return;
            s1q.title = BuildTitle(contract);
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"[Journal] Display title refresh failed: {ex.Message}");
        }
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
        if (complete) _rebindAttemptsRemaining = 0;
        return complete;
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
            Mod.Log.Warn("[Journal] Deferred quest rebind exhausted; no target binding was changed further.");
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
            // QuestManager.CreateQuest<T>(guid) registers a mod quest. The string is
            // an internal id — lookups must go by TITLE (see class comment).
            var q = QuestManager.CreateQuest<BountyQuest>(contract.Id);
            CreatingContractId = null;

            if (q == null)
            {
                Mod.Log.Warn($"QuestManager.CreateQuest<BountyQuest> returned null for '{contract.Id}'.");
                return null;
            }

            ((BountyQuest)q).InitContractId(contract.Id);
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

        // 2) Title lookup — S1API resolves quests by TITLE (HomelessMod pattern).
        //    v0.2.6: occupancy guard — in a same-target group the owner's quest
        //    personalises its title the moment it is adopted, and every sibling
        //    reconstructs the SAME title. Without this guard a sibling would
        //    steal (InitContractId) the owner's freshly adopted quest and
        //    re-create the exact M3 wrong-binding bug through the back door.
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

        // 3) Generic title — a restored quest lost its transient contract id and
        //    renders "Hitman Contract" until re-adopted.
        //    Audit M3 (2026-09-01): with 2+ restored quests all rendering the
        //    generic title, GetQuestByName returns ONE instance for EVERY
        //    contract — completing contract A completed B's journal entry too.
        //    v0.2.6 (2026-09-14): same-target groups are LEGAL (see v0.2.5 group
        //    payout) and must not brick the whole rebind. Resolution: when ALL
        //    unbound active contracts share one TargetNpcId, the OLDEST unbound
        //    contract adopts the restored generic quest (it is the only persisted
        //    quest — younger siblings were never saved) and every sibling gets a
        //    FRESH quest via RegisterBountyQuest. Mixed-target groups stay
        //    refused — a wrong binding (A completing B's entry) is worse than none.
        try
        {
            var unbound = CollectUnboundActiveContracts();
            if (unbound.Count > 1)
            {
                bool sameTarget = true;
                for (int i = 1; i < unbound.Count; i++)
                {
                    if (!string.Equals(unbound[i].TargetNpcId, unbound[0].TargetNpcId,
                            System.StringComparison.OrdinalIgnoreCase))
                    {
                        sameTarget = false;
                        break;
                    }
                }

                if (!sameTarget)
                {
                    // Genuinely ambiguous: mixed targets, generic titles only.
                    // Same damping as RebindActiveQuests: initial pass (0) and
                    // first/last retry only — not every 500 ms tick.
                    if (_rebindAttemptsRemaining <= 1 || _rebindAttemptsRemaining >= MaxRebindAttempts)
                    {
                        Mod.Log.Warn("[Journal] Mixed-target restored contracts without a bound quest — " +
                                     "generic-title adoption refused (fail-safe, audit M3).");
                    }
                    return null;
                }

                // Same-target group. Only the OLDEST member (the group owner) runs
                // the adoption below — any other member adopting the shared quest
                // would re-create the exact M3 wrong-binding bug.
                bool isGroupOwner = unbound[0].Id == contractId; // unbound[0] = oldest (Active list order)
                if (!isGroupOwner)
                {
                    Mod.Log.Debug($"[Journal] Same-target group on '{unbound[0].TargetNpcId}': '{contractId}' " +
                                  "is handled by the group owner's rebind pass.");
                    return null;
                }

                // Adopt as many restored generic quests as exist (GetQuestByName
                // returns only the FIRST match — a second restored instance would
                // otherwise linger as a zombie journal entry forever). Managed
                // QuestManager.Quests is a plain List<Quest>; reflection is safe.
                int adopted = 0;
                var liveQuests = EnumerateLiveBountyQuests();
                for (int q = 0; q < liveQuests.Count; q++)
                {
                    var candidate = liveQuests[q];
                    if (!IsGenericBountyQuest(candidate)) continue;
                    if (IsQuestBoundToOtherContract(candidate, contractId)) continue;

                    if (adopted < unbound.Count)
                    {
                        candidate.InitContractId(unbound[adopted].Id);
                        SessionQuests[unbound[adopted].Id] = candidate;
                        TryRefreshDisplayTitle(candidate, unbound[adopted]);
                        adopted++;
                        Mod.Log.Info($"[Journal] Re-adopted generic-titled quest for '{unbound[adopted - 1].Id}' " +
                                     $"(same-target group, member {adopted}/{unbound.Count}).");
                    }
                }

                // Contracts without a restored quest get FRESH journal entries.
                // CreateQuestForContract deliberately skips the ResolveQuest
                // dedupe — RegisterBountyQuest would re-enter ResolveQuest for
                // the still-unbound owner and recurse infinitely when the group
                // has no restored quest at all (quit-without-save case).
                for (int s = unbound.Count - 1; s >= 0; s--)
                {
                    if (SessionQuests.ContainsKey(unbound[s].Id)) continue;
                    var fresh = CreateQuestForContract(unbound[s]);
                    if (fresh != null)
                    {
                        Mod.Log.Info($"[Journal] Fresh quest registered for same-target member '{unbound[s].Id}'.");
                    }
                }

                return SessionQuests.TryGetValue(contractId, out var own) ? own : null;
            }
            else
            {
                if (QuestManager.GetQuestByName("Hitman Contract") is BountyQuest genericSolo
                    && !IsQuestBoundToOtherContract(genericSolo, contractId))
                {
                    genericSolo.InitContractId(contractId);
                    SessionQuests[contractId] = genericSolo;
                    TryRefreshDisplayTitle(genericSolo, contract);
                    Mod.Log.Info($"[Journal] Re-adopted generic-titled quest for '{contractId}'.");
                    return genericSolo;
                }

                // v0.2.6: documented step 4, now actually implemented — a contract
                // with NO restorable quest (accepted after the last game save)
                // gets a FRESH journal entry. Gated on the rebind pass so pure
                // lookups (CompleteQuest etc.) never create quests as a side effect.
                if (_inRebindPass)
                {
                    var freshSole = CreateQuestForContract(contract);
                    if (freshSole != null)
                    {
                        Mod.Log.Info($"[Journal] Fresh quest registered for unbound solo contract '{contractId}' " +
                                     "(no restored quest found).");
                        return freshSole;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] ResolveQuest(generic) failed: {ex.Message}");
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
        try
        {
            if (q.QuestEntries == null) return;
            for (int i = 0; i < q.QuestEntries.Count; i++)
            {
                var entry = q.QuestEntries[i];
                if (entry == null) continue;
                try
                {
                    if (entry.Title == "Eliminate the target and collect the polaroid" && entry.State != S1API.Quests.Constants.QuestState.Completed)
                    {
                        entry.Complete();
                    }
                    else if (entry.Title == "Drop the polaroid at any dead-drop" && entry.State != S1API.Quests.Constants.QuestState.Completed)
                    {
                        entry.Begin();
                    }
                }
                catch { }
            }
            Mod.Log.Info($"[Journal] Bounty '{contractId}' → Objective advanced to Dead-Drop.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"AdvanceToDeadDrop '{contractId}' failed: {ex.Message}");
        }
    }
}

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
/// Skill reference: <c>schedule1-game-systems/references/03-Dialogue-Quest.md</c>.
/// </summary>
public static class BountyJournalBridge
{
    internal static string? CreatingContractId;

    private const long RebindRetryIntervalMs = 500;
    private const int MaxRebindAttempts = 8;
    private static int _rebindAttemptsRemaining;
    private static long _nextRebindAt;

    /// <summary>Session-scoped quest instances keyed by contract id. Cleared on save load.</summary>
    private static readonly System.Collections.Generic.Dictionary<string, Quest> SessionQuests = new();

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
                Mod.Log.Warn($"[Journal] Could not rebind active contract '{contract.Id}' " +
                             $"for target '{contract.TargetNpcName ?? contract.TargetNpcId}'.");
            }
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
        string title = BuildTitle(contract);
        try
        {
            if (QuestManager.GetQuestByName(title) is BountyQuest byTitle)
            {
                byTitle.InitContractId(contractId);
                SessionQuests[contractId] = byTitle;
                Mod.Log.Info($"[Journal] Adopted live quest by title '{title}'.");
                return byTitle;
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
        //    Fail-safe now: adopt only when exactly ONE active contract still
        //    lacks a session quest; otherwise leave unbound (cosmetic loss, no
        //    wrong binding).
        try
        {
            if (CountActiveContractsWithoutSessionQuest() > 1)
            {
                Mod.Log.Warn("[Journal] Multiple restored contracts without a bound quest — " +
                             "generic-title adoption refused (fail-safe, audit M3).");
                return null;
            }
            if (QuestManager.GetQuestByName("Hitman Contract") is BountyQuest generic)
            {
                generic.InitContractId(contractId);
                SessionQuests[contractId] = generic;
                Mod.Log.Info($"[Journal] Re-adopted generic-titled quest for '{contractId}'.");
                return generic;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[Journal] ResolveQuest(generic) failed: {ex.Message}");
        }

        return null;
    }

    /// <summary>Audit M3 helper: active contracts with no session quest binding yet.</summary>
    private static int CountActiveContractsWithoutSessionQuest()
    {
        var save = Mod.Instance?.Save;
        if (save == null) return 0;
        int n = 0;
        for (int i = 0; i < save.Active.Count; i++)
        {
            var c = save.Active[i];
            if (c.Status != EBountyStatus.Active) continue;
            if (!SessionQuests.ContainsKey(c.Id)) n++;
        }
        return n;
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

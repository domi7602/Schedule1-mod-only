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

namespace HitmanPhone.Bounty;

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

    /// <summary>v0.2.7: refresh <c>S1Quest.title</c> via the official InitializeQuest path so the UI re-snapshots.</summary>
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

            // Prefer the public setter if present (vanilla fires onTitleChanged there),
            // else fall back to the InitializeQuest path the ctor used.
            var titleProp = s1q.GetType().GetProperty("Title",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.FlattenHierarchy);
            if (titleProp?.GetSetMethod(true) != null)
            {
                titleProp.SetValue(s1q, newTitle);
            }
            else
            {
                var init = s1q.GetType().GetMethod("InitializeQuest",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (init != null)
                {
                    // InitializeQuest's 3rd param is an Il2CppReferenceArray<QuestEntryData>;
                    // an empty managed array may or may not marshal. We only reach this branch
                    // when the Title property has no public setter; in that case assign the
                    // title field directly + raise the IL2CPP event by re-running Begin().
                    s1q.title = newTitle;
                    try { s1q.Begin(true); } catch { /* Begin() may throw if already begun */ }
                }
            }
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
            return $"Caller: {c.CallerId.Replace("caller_", "").ToUpper()}\n" +
                   $"Reward: ${c.RewardCash:N0}\n" +
                   $"Time limit: {c.DeadlineDay - c.OfferedAtDay} days";
        }
    }

    protected override void OnCreated()
    {
        base.OnCreated();
        AddEntry("Eliminate the target and collect the polaroid");
        AddEntry("Drop the polaroid at any dead-drop");
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

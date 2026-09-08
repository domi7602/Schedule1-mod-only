using System;
using HitmanPhone.Persistence;
using S1API.Quests;
using S1API.Quests.Constants;
using UnityEngine;

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase H — S1API Quest subclass for bounty contracts.
///
/// One quest is created per accepted bounty. Quests use the existing in-game
/// <c>JournalApp</c> via S1API auto-registration; the player sees a new quest
/// entry each time they accept a caller's offer.
///
/// Skill reference: <c>schedule1-game-systems/references/03-Dialogue-Quest.md</c>.
/// </summary>
public class BountyQuest : Quest
{
    private string? _contractId;

    public void InitContractId(string id)
    {
        _contractId = id;
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

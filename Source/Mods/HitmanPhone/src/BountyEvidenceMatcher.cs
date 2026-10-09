using System;
using System.Collections.Generic;
using HitmanPhone.Persistence;

namespace HitmanPhone.Bounty;

/// <summary>
/// Matches polaroids to the contract that created them. New polaroids use a
/// persistent token; old item IDs are accepted only when an active legacy
/// contract still records that evidence.
/// </summary>
public static class BountyEvidenceMatcher
{
    public static List<BountyContract>? Match(
        BountySaveData save,
        int encodedValue,
        bool isLegacyItem)
    {
        if (save == null) return null;

        for (int i = 0; i < save.Active.Count; i++)
        {
            var contract = save.Active[i];
            if (contract.Status != EBountyStatus.Active || !contract.AwaitingDrop) continue;

            bool matches = isLegacyItem
                ? encodedValue != 0 && contract.LegacyTargetInstanceId == encodedValue
                : encodedValue != 0 && contract.EvidenceToken == encodedValue;
            if (matches) return new List<BountyContract> { contract };
        }

        // Value=0 is only safe for a pre-token contract that was already waiting
        // for evidence when its old save was migrated. New contracts never use
        // this ambiguous fallback.
        if (isLegacyItem && encodedValue == 0)
        {
            var legacyGroup = new List<BountyContract>();
            string? sharedTarget = null;
            for (int i = 0; i < save.Active.Count; i++)
            {
                var contract = save.Active[i];
                if (contract.Status != EBountyStatus.Active ||
                    !contract.AwaitingDrop ||
                    !contract.AllowLegacyZeroEvidence)
                    continue;

                if (string.IsNullOrWhiteSpace(contract.TargetNpcId)) return null;
                if (sharedTarget == null) sharedTarget = contract.TargetNpcId;
                else if (!string.Equals(sharedTarget, contract.TargetNpcId, StringComparison.OrdinalIgnoreCase))
                    return null;
                legacyGroup.Add(contract);
            }
            return legacyGroup.Count > 0 ? legacyGroup : null;
        }

        return null;
    }

    public static int AllocateEvidenceToken(BountySaveData save)
    {
        if (save == null) throw new ArgumentNullException(nameof(save));
        var used = new HashSet<int>();
        AddTokens(save.Active, used);
        AddTokens(save.History, used);

        int candidate = save.NextEvidenceToken > 0 ? save.NextEvidenceToken : 1;
        while (used.Contains(candidate))
        {
            if (candidate == int.MaxValue)
                throw new InvalidOperationException("Evidence token range exhausted; refusing to reuse a token.");
            candidate++;
        }

        save.NextEvidenceToken = candidate == int.MaxValue ? int.MaxValue : candidate + 1;
        return candidate;
    }

    /// <summary>
    /// Assigns durable tokens to pre-token active contracts, preserves old photo
    /// IDs only for contracts already awaiting evidence, and clears transient
    /// Unity instance IDs so they cannot collide with another session's NPCs.
    /// </summary>
    public static int MigrateLegacyContracts(BountySaveData save)
    {
        if (save == null) throw new ArgumentNullException(nameof(save));
        int changed = 0;
        for (int i = 0; i < save.Active.Count; i++)
        {
            var contract = save.Active[i];
            if (contract.Status != EBountyStatus.Active) continue;

            bool legacyContract = contract.EvidenceToken <= 0;
            if (legacyContract)
            {
                contract.EvidenceToken = AllocateEvidenceToken(save);
                changed++;
                if (contract.AwaitingDrop && !contract.AllowLegacyZeroEvidence)
                {
                    contract.AllowLegacyZeroEvidence = true;
                    changed++;
                }
            }

            if (legacyContract && contract.AwaitingDrop &&
                contract.LegacyTargetInstanceId == 0 &&
                contract.TargetNpcInstanceId != 0)
            {
                contract.LegacyTargetInstanceId = contract.TargetNpcInstanceId;
                changed++;
            }

            if (contract.TargetNpcInstanceId != 0)
            {
                contract.TargetNpcInstanceId = 0;
                changed++;
            }
        }
        return changed;
    }

    private static void AddTokens(List<BountyContract> contracts, HashSet<int> used)
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            int token = contracts[i].EvidenceToken;
            if (token > 0) used.Add(token);
        }
    }
}

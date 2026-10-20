using System;
using System.Collections.Generic;
using HitmanPhone.Persistence;

namespace HitmanPhone.Bounty;

public enum EvidencePresence { Unknown, Missing, Present }

public enum RewardAttempt { Rejected, Paid, Uncertain }

public static class BountyRewardSettlement
{
    public static RewardAttempt TryPay(BountyContract contract, Action<float> pay)
    {
        if (contract.Status != EBountyStatus.Active || contract.RewardPaymentUncertain ||
            !float.IsFinite(contract.RewardCash) || contract.RewardCash <= 0 || pay == null)
            return RewardAttempt.Rejected;
        contract.RewardPaymentUncertain = true; // close reentrant/native-exception retry first
        try { pay(contract.RewardCash); }
        catch { return RewardAttempt.Uncertain; }
        contract.Status = EBountyStatus.Completed;
        contract.AwaitingDrop = false;
        contract.RewardPaymentUncertain = false;
        return RewardAttempt.Paid;
    }
}

/// <summary>
/// Matches polaroids to the contract that created them. New polaroids use a
/// persistent token; old item IDs are accepted only when an active legacy
/// contract still records that evidence.
/// </summary>
public static class BountyEvidenceMatcher
{
    /// <summary>
    /// Identity-only match (no deadline filter). Kept for callers that already
    /// gate on <see cref="BountyDeadlinePolicy"/> themselves and for the
    /// pre-existing unit contracts of this type.
    /// </summary>
    public static List<BountyContract>? Match(
        BountySaveData save,
        int encodedValue,
        bool isLegacyItem)
        => Match(save, encodedValue, isLegacyItem, hasNow: false, nowMinSum: 0);

    /// <summary>
    /// HP-13: match AND require the contract to still be inside its deadline at
    /// <paramref name="nowMinSum"/>. The payout path uses this overload so a photo
    /// handed in after the governing deadline is never paid.
    /// </summary>
    public static List<BountyContract>? Match(
        BountySaveData save,
        int encodedValue,
        bool isLegacyItem,
        long nowMinSum)
        => Match(save, encodedValue, isLegacyItem, hasNow: true, nowMinSum: nowMinSum);

    private static List<BountyContract>? Match(
        BountySaveData save,
        int encodedValue,
        bool isLegacyItem,
        bool hasNow,
        long nowMinSum)
    {
        if (save == null) return null;

        for (int i = 0; i < save.Active.Count; i++)
        {
            var contract = save.Active[i];
            if (contract.Status != EBountyStatus.Active || !contract.AwaitingDrop) continue;
            if (hasNow && !BountyDeadlinePolicy.IsEligible(contract, nowMinSum)) continue;

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
                if (hasNow && !BountyDeadlinePolicy.IsEligible(contract, nowMinSum)) continue;

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

    public static EvidencePresence ObserveForContract(BountySaveData save, BountyContract contract, int value, bool legacy)
    {
        if (!legacy)
            return value > 0 && value == contract.EvidenceToken ? EvidencePresence.Present : EvidencePresence.Missing;
        if (value != 0)
        {
            int id = contract.LegacyTargetInstanceId;
            if (id == 0 && contract.EvidenceToken <= 0) id = contract.TargetNpcInstanceId;
            return id != 0 && id == value ? EvidencePresence.Present : EvidencePresence.Missing;
        }
        if (!contract.AwaitingDrop || !contract.AllowLegacyZeroEvidence) return EvidencePresence.Missing;
        if (string.IsNullOrWhiteSpace(contract.TargetNpcId)) return EvidencePresence.Unknown;
        foreach (var candidate in save.Active)
        {
            if (candidate.Status != EBountyStatus.Active || !candidate.AwaitingDrop || !candidate.AllowLegacyZeroEvidence) continue;
            if (!string.Equals(candidate.TargetNpcId, contract.TargetNpcId, StringComparison.OrdinalIgnoreCase))
                return EvidencePresence.Unknown;
        }
        // Also usable by terminal cleanup after this contract has left Active.
        return EvidencePresence.Present;
    }

    public static EvidencePresence MergePresence(EvidencePresence left, EvidencePresence right)
        => left == EvidencePresence.Present || right == EvidencePresence.Present ? EvidencePresence.Present
            : left == EvidencePresence.Unknown || right == EvidencePresence.Unknown ? EvidencePresence.Unknown
            : EvidencePresence.Missing;

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

/// <summary>
/// HP-13 — the single shared deadline decision.
///
/// One rule for the elimination gate, the receipt/payout gate and the expiry
/// poller: a contract is ELIGIBLE while the governing deadline has not yet been
/// reached. The comparison is strict (<c>now &lt; due</c>) — exactly at and after the
/// due point the contract is no longer eligible and may be retired by the poller.
/// The poller therefore still warns/displays/cleans up, but no longer decides
/// validity on its own; the same rule is applied immediately before an
/// elimination is recognized and immediately before a payout is issued.
///
/// Deadlines are absolute in-game minute sums (<c>day * 1440 + minute-of-day</c>),
/// the game's own <c>GameDateTime.GetMinSum()</c> convention — never realtime.
/// </summary>
public static class BountyDeadlinePolicy
{
    /// <summary>Which of the two deadlines currently governs the contract.</summary>
    public enum Phase
    {
        /// <summary>Day-based execution window (kill + delivery) — no photo received yet.</summary>
        Execution,
        /// <summary>Minute-precision post-photo drop window — started at first confirmed receipt.</summary>
        Drop,
    }

    public static Phase PhaseFor(BountyContract? contract)
        => contract != null && contract.DropDueStarted ? Phase.Drop : Phase.Execution;

    /// <summary>
    /// Absolute minute-sum at which the governing deadline is reached, or 0 when
    /// the contract has no deadline at all (legacy contract).
    /// </summary>
    public static long DueMinSum(BountyContract? contract)
    {
        if (contract == null) return 0;
        if (contract.DropDueStarted && contract.DropDueMinSum > 0) return contract.DropDueMinSum;
        if (contract.DeadlineDay > 0) return contract.DeadlineDay * 1440L;
        return 0;
    }

    /// <summary>
    /// Execution-phase check only (ignores a started drop window). Used where the
    /// question is "may the execution window still be acted on?" — e.g. before a
    /// late delivery is allowed to open a fresh drop window.
    /// </summary>
    public static bool IsExecutionPhaseEligible(BountyContract? contract, long nowMinSum)
    {
        if (contract == null || contract.RewardPaymentUncertain || nowMinSum < 0) return false;
        if (contract.DeadlineDay <= 0) return true; // legacy: no execution deadline
        return nowMinSum < contract.DeadlineDay * 1440L;
    }

    /// <summary>
    /// The governing deadline decision. The drop window wins once it has started;
    /// otherwise the day-based execution deadline applies.
    /// </summary>
    public static bool IsEligible(BountyContract? contract, long nowMinSum)
    {
        if (contract == null || contract.RewardPaymentUncertain || nowMinSum < 0) return false;
        if (contract.DropDueStarted && contract.DropDueMinSum > 0)
            return nowMinSum < contract.DropDueMinSum;
        return IsExecutionPhaseEligible(contract, nowMinSum);
    }
}

/// <summary>Per-contract verdict for one scanned polaroid.</summary>
public enum DepositDecision
{
    Pay,
    WrongDrop,
    Expired,
}

/// <summary>One polaroid observed in a dead-drop storage during a scan.</summary>
public sealed class DepositScanItem
{
    public int ItemIndex;
    public int EncodedValue;
    public bool IsLegacy;
    public List<BountyContract> Matched = new();
}

/// <summary>Per-item plan: which contracts to pay and whether the photo is consumed.</summary>
public sealed class DepositItemPlan
{
    public int ItemIndex;
    public int EncodedValue;
    public bool IsLegacy;
    /// <summary>Contracts this photo must pay (deduplicated across the whole scan).</summary>
    public List<BountyContract> Pay = new();
    /// <summary>
    /// True only when every contract this photo matched was payable. A rejected
    /// contract (wrong drop / expired) keeps the photo in the container.
    /// </summary>
    public bool Consume;
}

/// <summary>Whole-scan plan for one storage-changed event.</summary>
public sealed class DepositScanPlan
{
    public List<DepositItemPlan> Items = new();
    public List<BountyContract> WrongDrop = new();
    public List<BountyContract> Expired = new();
}

/// <summary>
/// HP-03 / HP-13 — the pure deposit-scan decision, split out of the Unity receipt
/// hook so the "continue past a rejected photo" behaviour is regression-testable.
///
/// The old scan returned after the FIRST matching photo; a photo bound to another
/// contract's assigned drop therefore blocked every later, valid deposit behind it
/// in the same container. This planner instead walks the whole container: a
/// rejected photo (wrong assigned drop, or past its deadline) is recorded and the
/// scan continues, so later valid evidence is still paid — and the rejected photo
/// is left untouched.
/// </summary>
public static class BountyDepositPolicy
{
    public static DepositDecision Decide(BountyContract? contract, string? actualDropGuid, long nowMinSum)
    {
        if (contract == null) return DepositDecision.Expired;
        if (!BountyDeadlinePolicy.IsEligible(contract, nowMinSum)) return DepositDecision.Expired;
        if (!string.IsNullOrEmpty(contract.RequiredDropId) &&
            !string.Equals(actualDropGuid, contract.RequiredDropId, StringComparison.OrdinalIgnoreCase))
            return DepositDecision.WrongDrop;
        return DepositDecision.Pay;
    }

    /// <summary>
    /// Plan a complete container scan. Never stops at the first rejection; each
    /// photo is decided independently. A contract is planned at most once across
    /// the whole scan (a second, duplicate photo of the same token must not pay a
    /// second time). A photo is only consumed when every contract it matched was
    /// payable — a rejected contract keeps its photo where it is.
    /// </summary>
    public static DepositScanPlan PlanScan(
        IReadOnlyList<DepositScanItem>? items,
        string? actualDropGuid,
        long nowMinSum)
    {
        var plan = new DepositScanPlan();
        if (items == null) return plan;

        var planned = new HashSet<BountyContract>();
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item == null || item.Matched == null || item.Matched.Count == 0) continue;

            var itemPlan = new DepositItemPlan
            {
                ItemIndex = item.ItemIndex,
                EncodedValue = item.EncodedValue,
                IsLegacy = item.IsLegacy,
            };
            bool blocked = false;

            for (int m = 0; m < item.Matched.Count; m++)
            {
                var contract = item.Matched[m];
                if (contract == null) continue;
                if (planned.Contains(contract)) continue; // duplicate photo of an already-planned token

                switch (Decide(contract, actualDropGuid, nowMinSum))
                {
                    case DepositDecision.Pay:
                        itemPlan.Pay.Add(contract);
                        planned.Add(contract);
                        break;
                    case DepositDecision.WrongDrop:
                        plan.WrongDrop.Add(contract);
                        blocked = true;
                        break;
                    default:
                        plan.Expired.Add(contract);
                        blocked = true;
                        break;
                }
            }

            itemPlan.Consume = !blocked && itemPlan.Pay.Count > 0;
            if (itemPlan.Pay.Count > 0) plan.Items.Add(itemPlan);
        }

        return plan;
    }
}

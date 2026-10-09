using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

public class BountyEvidenceMatcherTests
{
    private static BountyContract Contract(
        string id,
        string targetId,
        int evidenceToken,
        int legacyInstanceId = 0,
        bool allowLegacyZero = false,
        EBountyStatus status = EBountyStatus.Active,
        bool awaitingDrop = true) => new()
        {
            Id = id,
            TargetNpcId = targetId,
            EvidenceToken = evidenceToken,
            LegacyTargetInstanceId = legacyInstanceId,
            AllowLegacyZeroEvidence = allowLegacyZero,
            Status = status,
            AwaitingDrop = awaitingDrop
        };

    [Fact]
    public void Match_NewEvidenceToken_OnlyMatchesItsActiveContract()
    {
        var active = Contract("new", "npc_a", evidenceToken: 1001);
        var save = new BountySaveData();
        save.Active.Add(active);
        save.Active.Add(Contract("other", "npc_b", evidenceToken: 1002));

        var result = BountyEvidenceMatcher.Match(save, encodedValue: 1001, isLegacyItem: false);

        Assert.NotNull(result);
        Assert.Single(result!);
        Assert.Same(active, result[0]);
    }

    [Fact]
    public void Match_LegacyPhoto_MatchesOnlyAnActiveContractThatOwnedItsOldId()
    {
        var active = Contract("active-old", "npc_a", evidenceToken: 1001, legacyInstanceId: 2468);
        var save = new BountySaveData();
        save.Active.Add(active);

        var result = BountyEvidenceMatcher.Match(save, encodedValue: 2468, isLegacyItem: true);

        Assert.NotNull(result);
        Assert.Single(result!);
        Assert.Same(active, result[0]);
    }

    [Fact]
    public void Match_ExpiredContractsEvidence_CannotPayNewContractForSameTarget()
    {
        var newContract = Contract("new", "npc_a", evidenceToken: 1002);
        var expired = Contract("expired", "npc_a", evidenceToken: 1001,
            legacyInstanceId: 2468, status: EBountyStatus.Expired);
        var save = new BountySaveData();
        save.Active.Add(newContract);
        save.History.Add(expired);

        var result = BountyEvidenceMatcher.Match(save, encodedValue: 2468, isLegacyItem: true);

        Assert.Null(result);
    }

    [Fact]
    public void Match_ZeroValueLegacyPhoto_OnlyMatchesMigratedLegacyContract()
    {
        var legacy = Contract("legacy", "npc_a", evidenceToken: 1001, allowLegacyZero: true);
        var modern = Contract("modern", "npc_b", evidenceToken: 1002);
        var save = new BountySaveData();
        save.Active.Add(legacy);
        save.Active.Add(modern);

        var result = BountyEvidenceMatcher.Match(save, encodedValue: 0, isLegacyItem: true);

        Assert.NotNull(result);
        Assert.Single(result!);
        Assert.Same(legacy, result[0]);
    }

    [Fact]
    public void Match_ZeroValueLegacyPhoto_DoesNotMatchModernContract()
    {
        var save = new BountySaveData();
        save.Active.Add(Contract("modern", "npc_a", evidenceToken: 1002));

        Assert.Null(BountyEvidenceMatcher.Match(save, encodedValue: 0, isLegacyItem: true));
    }

    [Fact]
    public void AllocateEvidenceToken_IsPositiveAndUniqueAcrossActiveAndHistory()
    {
        var save = new BountySaveData();
        save.Active.Add(Contract("active", "npc_a", evidenceToken: 1));
        save.History.Add(Contract("old", "npc_b", evidenceToken: 2, status: EBountyStatus.Completed));

        int allocated = BountyEvidenceMatcher.AllocateEvidenceToken(save);

        Assert.Equal(3, allocated);
        Assert.Equal(4, save.NextEvidenceToken);
    }

    [Fact]
    public void MigrateLegacyContract_PreservesEvidenceIdentityOnlyWhenPhotoIsAwaiting()
    {
        var awaiting = new BountyContract
        {
            Id = "awaiting",
            TargetNpcId = "npc_a",
            TargetNpcInstanceId = 2468,
            Status = EBountyStatus.Active,
            AwaitingDrop = true
        };
        var notAwaiting = new BountyContract
        {
            Id = "not-awaiting",
            TargetNpcId = "npc_b",
            TargetNpcInstanceId = 1357,
            Status = EBountyStatus.Active,
            AwaitingDrop = false
        };
        var save = new BountySaveData();
        save.Active.Add(awaiting);
        save.Active.Add(notAwaiting);

        BountyEvidenceMatcher.MigrateLegacyContracts(save);

        Assert.Equal(2468, awaiting.LegacyTargetInstanceId);
        Assert.True(awaiting.AllowLegacyZeroEvidence);
        Assert.Equal(0, awaiting.TargetNpcInstanceId);
        Assert.True(awaiting.EvidenceToken > 0);
        Assert.Equal(0, notAwaiting.LegacyTargetInstanceId);
        Assert.False(notAwaiting.AllowLegacyZeroEvidence);
        Assert.Equal(0, notAwaiting.TargetNpcInstanceId);
        Assert.True(notAwaiting.EvidenceToken > 0);
        Assert.NotEqual(awaiting.EvidenceToken, notAwaiting.EvidenceToken);
    }

    [Fact]
    public void MigrateLegacyContract_DoesNotTreatNewTokenEvidenceAsLegacyPhoto()
    {
        var modern = new BountyContract
        {
            Id = "modern",
            TargetNpcId = "npc_a",
            TargetNpcInstanceId = 2468,
            EvidenceToken = 1001,
            Status = EBountyStatus.Active,
            AwaitingDrop = true
        };
        var save = new BountySaveData();
        save.Active.Add(modern);

        BountyEvidenceMatcher.MigrateLegacyContracts(save);

        Assert.Equal(0, modern.LegacyTargetInstanceId);
        Assert.Equal(1001, modern.EvidenceToken);
        Assert.Equal(0, modern.TargetNpcInstanceId);
    }

    [Fact]
    public void AllocateEvidenceToken_WhenMaximumIsUsed_DoesNotWrapToOldTokens()
    {
        var save = new BountySaveData { NextEvidenceToken = int.MaxValue };
        save.History.Add(Contract("old", "npc_a", evidenceToken: int.MaxValue,
            status: EBountyStatus.Completed));

        Assert.Throws<InvalidOperationException>(
            () => BountyEvidenceMatcher.AllocateEvidenceToken(save));
        Assert.Equal(int.MaxValue, save.NextEvidenceToken);
    }
}

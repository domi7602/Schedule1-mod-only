using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

public class EvidencePresenceTests
{
    private static BountyContract Legacy(string target, int token) => new()
    {
        Id = "c" + token,
        TargetNpcId = target,
        EvidenceToken = token,
        Status = EBountyStatus.Active,
        AwaitingDrop = true,
        AllowLegacyZeroEvidence = true
    };

    [Fact]
    public void ExistingLegacyZeroPhotoIsPresentForAnUnambiguousTargetGroup()
    {
        var save = new BountySaveData();
        var first = Legacy("a", 1);
        var second = Legacy("a", 2);
        save.Active.AddRange(new[] { first, second });
        Assert.Contains(first, BountyEvidenceMatcher.Match(save, 0, true)!);
        Assert.Equal(EvidencePresence.Present, BountyEvidenceMatcher.ObserveForContract(save, first, 0, true));
        Assert.Equal(EvidencePresence.Present, BountyEvidenceMatcher.ObserveForContract(save, second, 0, true));
    }

    [Fact]
    public void AmbiguousLegacyZeroPhotoIsUnknownNotProofThatEvidenceIsMissing()
    {
        var save = new BountySaveData();
        var a = Legacy("a", 1);
        var b = Legacy("b", 2);
        save.Active.AddRange(new[] { a, b });
        Assert.Null(BountyEvidenceMatcher.Match(save, 0, true));
        Assert.Equal(EvidencePresence.Unknown, BountyEvidenceMatcher.ObserveForContract(save, a, 0, true));
        Assert.Equal(EvidencePresence.Unknown, BountyEvidenceMatcher.ObserveForContract(save, b, 0, true));
    }

    [Fact]
    public void LegacyZeroCleanupKeepsUnrelatedActiveTargetEvidence()
    {
        var save = new BountySaveData();
        var expired = Legacy("a", 1);
        expired.Status = EBountyStatus.Expired;
        save.Active.Add(Legacy("b", 2));
        Assert.NotEqual(EvidencePresence.Present, BountyEvidenceMatcher.ObserveForContract(save, expired, 0, true));
        save.Active.Clear();
        Assert.Equal(EvidencePresence.Present, BountyEvidenceMatcher.ObserveForContract(save, expired, 0, true));
    }

    [Theory]
    [InlineData(7, false, EvidencePresence.Present)]
    [InlineData(0, false, EvidencePresence.Missing)]
    [InlineData(9, false, EvidencePresence.Missing)]
    [InlineData(99, true, EvidencePresence.Present)]
    [InlineData(0, true, EvidencePresence.Missing)]
    public void NewAndEncodedLegacyEvidenceUseExactIdentity(int value, bool legacy, EvidencePresence expected)
    {
        var save = new BountySaveData();
        var c = new BountyContract { EvidenceToken = 7, LegacyTargetInstanceId = 99 };
        Assert.Equal(expected, BountyEvidenceMatcher.ObserveForContract(save, c, value, legacy));
    }

    [Theory]
    [InlineData(EvidencePresence.Missing, EvidencePresence.Unknown, EvidencePresence.Unknown)]
    [InlineData(EvidencePresence.Unknown, EvidencePresence.Missing, EvidencePresence.Unknown)]
    [InlineData(EvidencePresence.Present, EvidencePresence.Unknown, EvidencePresence.Present)]
    [InlineData(EvidencePresence.Unknown, EvidencePresence.Present, EvidencePresence.Present)]
    [InlineData(EvidencePresence.Missing, EvidencePresence.Missing, EvidencePresence.Missing)]
    public void IncompleteScanCannotEstablishAbsence(EvidencePresence left, EvidencePresence right, EvidencePresence expected)
        => Assert.Equal(expected, BountyEvidenceMatcher.MergePresence(left, right));
}

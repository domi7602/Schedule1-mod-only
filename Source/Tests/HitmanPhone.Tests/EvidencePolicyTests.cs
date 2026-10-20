using System.Text.Json;
using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

// ---------------------------------------------------------------------------
// HP-13: the shared deadline rule (BountyDeadlinePolicy).
//
// Boundary contract, applied identically by the elimination gate, the payout
// gate and the expiry poller: a contract is ELIGIBLE strictly before its
// governing due point (now < due). At and after the due point it is not.
// Deadlines are absolute in-game minute sums: day * 1440 + minute-of-day.
// ---------------------------------------------------------------------------

public class BountyDeadlinePolicyTests
{
    private const long Day = 1440L;

    private static BountyContract Execution(int deadlineDay) => new()
    {
        Id = "c",
        TargetNpcId = "npc_a",
        Status = EBountyStatus.Active,
        AwaitingDrop = false,
        DeadlineDay = deadlineDay,
    };

    private static BountyContract DropPhase(long dueMinSum, int deadlineDay = 0) => new()
    {
        Id = "c",
        TargetNpcId = "npc_a",
        Status = EBountyStatus.Active,
        AwaitingDrop = true,
        DeadlineDay = deadlineDay,
        DropDueStarted = true,
        DropDueMinSum = dueMinSum,
        DropWindowMinutes = 120,
    };

    [Fact]
    public void IsEligible_ExecutionPhase_BeforeAtAndAfterDeadline()
    {
        var c = Execution(deadlineDay: 7);
        long due = 7 * Day;

        Assert.True(BountyDeadlinePolicy.IsEligible(c, due - 1));   // just before
        Assert.False(BountyDeadlinePolicy.IsEligible(c, due));      // exactly at -> expired
        Assert.False(BountyDeadlinePolicy.IsEligible(c, due + 1));  // after
        Assert.Equal(BountyDeadlinePolicy.Phase.Execution, BountyDeadlinePolicy.PhaseFor(c));
        Assert.Equal(due, BountyDeadlinePolicy.DueMinSum(c));
    }

    [Fact]
    public void IsEligible_DropWindowWinsOnceStarted()
    {
        var c = DropPhase(dueMinSum: 5 * Day + 600, deadlineDay: 9);

        Assert.Equal(BountyDeadlinePolicy.Phase.Drop, BountyDeadlinePolicy.PhaseFor(c));
        Assert.True(BountyDeadlinePolicy.IsEligible(c, 5 * Day + 599));
        Assert.False(BountyDeadlinePolicy.IsEligible(c, 5 * Day + 600)); // strict boundary
        // The execution deadline (day 9) is irrelevant while the drop window governs.
        Assert.False(BountyDeadlinePolicy.IsEligible(c, 5 * Day + 601));
    }

    [Fact]
    public void IsEligible_DropStartedWithoutDueSum_FallsBackToExecutionDeadline()
    {
        var c = DropPhase(dueMinSum: 0, deadlineDay: 7);
        long due = 7 * Day;

        Assert.True(BountyDeadlinePolicy.IsEligible(c, due - 1));
        Assert.False(BountyDeadlinePolicy.IsEligible(c, due));
    }

    [Fact]
    public void IsEligible_LegacyContractWithoutDeadline_StaysEligible()
    {
        var c = Execution(deadlineDay: 0);
        Assert.True(BountyDeadlinePolicy.IsEligible(c, 100000 * Day));
        Assert.Equal(0, BountyDeadlinePolicy.DueMinSum(c));
    }

    [Fact]
    public void IsEligible_NullContract_IsNeverEligible()
    {
        Assert.False(BountyDeadlinePolicy.IsEligible(null, 0));
        Assert.False(BountyDeadlinePolicy.IsExecutionPhaseEligible(null, 0));
        Assert.Equal(0, BountyDeadlinePolicy.DueMinSum(null));
    }

    [Fact]
    public void IsExecutionPhaseEligible_IgnoresAStartedDropWindow()
    {
        // HP-13: a late hand-over must not open a fresh window. The execution check
        // is the gate for "may the execution window still be acted on?".
        var c = DropPhase(dueMinSum: 5 * Day + 10, deadlineDay: 7);

        Assert.True(BountyDeadlinePolicy.IsExecutionPhaseEligible(c, 6 * Day));
        Assert.False(BountyDeadlinePolicy.IsExecutionPhaseEligible(c, 7 * Day)); // late
    }
}

// ---------------------------------------------------------------------------
// HP-03 / HP-13: the pure deposit-scan plan (BountyDepositPolicy).
//
// The old scan returned after the FIRST matching photo, so a photo bound to
// another contract's assigned drop blocked every valid deposit behind it in the
// same container. PlanScan must walk the whole container.
// ---------------------------------------------------------------------------

public class BountyDepositPolicyTests
{
    private static BountyContract Active(
        string id,
        string target = "npc_a",
        string? requiredDrop = null,
        int deadlineDay = 0,
        bool dropStarted = false,
        long dropDueMinSum = 0) => new()
        {
            Id = id,
            TargetNpcId = target,
            Status = EBountyStatus.Active,
            AwaitingDrop = true,
            EvidenceToken = 1000 + id.Length,
            RequiredDropId = requiredDrop ?? "",
            DeadlineDay = deadlineDay,
            DropDueStarted = dropStarted,
            DropDueMinSum = dropDueMinSum,
        };

    private static DepositScanItem Item(int index, int value, params BountyContract[] matched) => new()
    {
        ItemIndex = index,
        EncodedValue = value,
        IsLegacy = false,
        Matched = new List<BountyContract>(matched),
    };

    [Fact]
    public void PlanScan_WrongDropPhotoFirst_DoesNotBlockLaterValidPhoto()
    {
        var contractA = Active("a", requiredDrop: "drop_X"); // photo dropped in Y
        var contractB = Active("b", requiredDrop: "drop_Y"); // photo correctly in Y
        var items = new List<DepositScanItem>
        {
            Item(0, 1001, contractA),
            Item(1, 1002, contractB),
        };

        var plan = BountyDepositPolicy.PlanScan(items, "drop_Y", nowMinSum: 0);

        // B is still paid even though A (earlier slot) is rejected.
        Assert.Single(plan.Items);
        Assert.Equal(1, plan.Items[0].ItemIndex);
        Assert.Same(contractB, Assert.Single(plan.Items[0].Pay));
        Assert.True(plan.Items[0].Consume);

        // A is rejected and its photo is left untouched (no consume).
        Assert.Same(contractA, Assert.Single(plan.WrongDrop));
        Assert.Empty(plan.Expired);
    }

    [Fact]
    public void PlanScan_TwoValidPhotos_BothPaidAndConsumed()
    {
        var contractA = Active("a", requiredDrop: "drop_Y");
        var contractB = Active("b", requiredDrop: "drop_Y");
        var items = new List<DepositScanItem>
        {
            Item(0, 1001, contractA),
            Item(1, 1002, contractB),
        };

        var plan = BountyDepositPolicy.PlanScan(items, "drop_Y", nowMinSum: 0);

        Assert.Equal(2, plan.Items.Count);
        Assert.Same(contractA, Assert.Single(plan.Items[0].Pay));
        Assert.Same(contractB, Assert.Single(plan.Items[1].Pay));
        Assert.All(plan.Items, i => Assert.True(i.Consume));
        Assert.Empty(plan.WrongDrop);
    }

    [Fact]
    public void PlanScan_ExpiredContract_IsNotPaidAndDoesNotBlockLaterValid()
    {
        var expired = Active("expired", requiredDrop: "drop_Y", deadlineDay: 5);
        var valid = Active("valid", requiredDrop: "drop_Y", deadlineDay: 9);
        var items = new List<DepositScanItem>
        {
            Item(0, 1001, expired),
            Item(1, 1002, valid),
        };

        var plan = BountyDepositPolicy.PlanScan(items, "drop_Y", nowMinSum: 5 * 1440L);

        Assert.Single(plan.Items);
        Assert.Same(valid, Assert.Single(plan.Items[0].Pay));
        Assert.Same(expired, Assert.Single(plan.Expired));
        Assert.Empty(plan.WrongDrop);
    }

    [Fact]
    public void PlanScan_DuplicatePhotoForSameToken_PaysExactlyOnce()
    {
        var contract = Active("a", requiredDrop: "drop_Y");
        var items = new List<DepositScanItem>
        {
            Item(0, 1001, contract),
            Item(1, 1001, contract), // stray second copy of the same evidence
        };

        var plan = BountyDepositPolicy.PlanScan(items, "drop_Y", nowMinSum: 0);

        Assert.Single(plan.Items);
        Assert.Equal(0, plan.Items[0].ItemIndex);
        Assert.Same(contract, Assert.Single(plan.Items[0].Pay));
    }

    [Fact]
    public void PlanScan_MixedWrongAndRightMatchesOnOnePhoto_DoesNotConsume()
    {
        // One photo covering two contracts on the same head where only one of them
        // belongs to this drop: keep the photo so the other contract can still be paid.
        var wrong = Active("wrong", requiredDrop: "drop_X");
        var right = Active("right", requiredDrop: "drop_Y");
        var items = new List<DepositScanItem> { Item(0, 1001, wrong, right) };

        var plan = BountyDepositPolicy.PlanScan(items, "drop_Y", nowMinSum: 0);

        Assert.Single(plan.Items);
        Assert.Same(right, Assert.Single(plan.Items[0].Pay));
        Assert.False(plan.Items[0].Consume); // rejected member keeps the photo in place
        Assert.Same(wrong, Assert.Single(plan.WrongDrop));
    }

    [Fact]
    public void PlanScan_ContractWithoutAssignedDrop_PaysAtAnyDrop()
    {
        var anyDrop = Active("any", requiredDrop: null);
        var plan = BountyDepositPolicy.PlanScan(
            new List<DepositScanItem> { Item(0, 1001, anyDrop) }, "drop_Z", nowMinSum: 0);

        Assert.Same(anyDrop, Assert.Single(plan.Items[0].Pay));
        Assert.True(plan.Items[0].Consume);
    }

    [Fact]
    public void PlanScan_NullOrEmptyInput_ReturnsEmptyPlan()
    {
        Assert.Empty(BountyDepositPolicy.PlanScan(null, "drop_Y", 0).Items);

        var noMatches = new List<DepositScanItem> { Item(0, 1) };
        Assert.Empty(BountyDepositPolicy.PlanScan(noMatches, "drop_Y", 0).Items);
    }

    [Fact]
    public void Decide_ReportsPayWrongDropOrExpired()
    {
        var ok = Active("ok", requiredDrop: "drop_Y");
        Assert.Equal(DepositDecision.Pay, BountyDepositPolicy.Decide(ok, "drop_Y", 0));
        Assert.Equal(DepositDecision.WrongDrop, BountyDepositPolicy.Decide(ok, "drop_X", 0));

        var late = Active("late", requiredDrop: "drop_Y", deadlineDay: 3);
        Assert.Equal(DepositDecision.Expired, BountyDepositPolicy.Decide(late, "drop_Y", 3 * 1440L));
    }
}

// ---------------------------------------------------------------------------
// HP-13 at the matcher boundary: the payout path must not match a contract
// whose deadline has passed, while the identity-only overload stays unchanged.
// ---------------------------------------------------------------------------

public class BountyEvidenceMatcherDeadlineTests
{
    private static BountyContract Awaiting(string id, int deadlineDay, bool dropStarted = false, long dropDue = 0) => new()
    {
        Id = id,
        TargetNpcId = "npc_a",
        EvidenceToken = 4242,
        Status = EBountyStatus.Active,
        AwaitingDrop = true,
        DeadlineDay = deadlineDay,
        DropDueStarted = dropStarted,
        DropDueMinSum = dropDue,
    };

    [Fact]
    public void Match_WithNow_ExcludesContractPastItsExecutionDeadline()
    {
        var c = Awaiting("c", deadlineDay: 5);
        var save = new BountySaveData();
        save.Active.Add(c);

        Assert.NotNull(BountyEvidenceMatcher.Match(save, 4242, isLegacyItem: false, nowMinSum: 4 * 1440L));
        Assert.Null(BountyEvidenceMatcher.Match(save, 4242, isLegacyItem: false, nowMinSum: 5 * 1440L));
        Assert.Null(BountyEvidenceMatcher.Match(save, 4242, isLegacyItem: false, nowMinSum: 6 * 1440L));
    }

    [Fact]
    public void Match_WithNow_ExcludesContractPastItsDropWindow()
    {
        var c = Awaiting("c", deadlineDay: 9, dropStarted: true, dropDue: 6 * 1440L + 120);
        var save = new BountySaveData();
        save.Active.Add(c);

        Assert.NotNull(BountyEvidenceMatcher.Match(save, 4242, false, 6 * 1440L + 119));
        Assert.Null(BountyEvidenceMatcher.Match(save, 4242, false, 6 * 1440L + 120));
    }

    [Fact]
    public void Match_IdentityOnlyOverload_StillIgnoresTheDeadline()
    {
        // Backward compatibility: the 3-arg overload is the pure identity matcher.
        var c = Awaiting("c", deadlineDay: 1);
        var save = new BountySaveData();
        save.Active.Add(c);

        Assert.NotNull(BountyEvidenceMatcher.Match(save, 4242, isLegacyItem: false));
    }
}

// ---------------------------------------------------------------------------
// HP-04: the narrow elimination/delivery field must survive serialization.
// ---------------------------------------------------------------------------

public class BountyEvidencePersistenceTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        IncludeFields = true,
        WriteIndented = true
    };

    [Fact]
    public void PendingEvidenceDelivery_RoundTrips()
    {
        var save = new BountySaveData { SaveIdentity = "slot3" };
        save.Active.Add(new BountyContract
        {
            Id = "c1",
            TargetNpcId = "npc_a",
            EvidenceToken = 7,
            Status = EBountyStatus.Active,
            AwaitingDrop = true,
            PendingEvidenceDelivery = true,
        });

        var json = JsonSerializer.Serialize(save, Options);
        var back = JsonSerializer.Deserialize<BountySaveData>(json, Options);

        Assert.NotNull(back);
        var c = Assert.Single(back!.Active);
        Assert.True(c.AwaitingDrop);
        Assert.True(c.PendingEvidenceDelivery);
    }

    [Fact]
    public void PendingEvidenceDelivery_DefaultsToFalseForOldSaves()
    {
        const string oldJson = """{ "Id": "c1", "AwaitingDrop": true }""";
        var c = JsonSerializer.Deserialize<BountyContract>(oldJson, Options);
        Assert.NotNull(c);
        Assert.True(c!.AwaitingDrop);
        Assert.False(c.PendingEvidenceDelivery);
    }
}

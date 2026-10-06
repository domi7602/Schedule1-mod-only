using System.Text.Json;
using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

// ---------------------------------------------------------------------------
// Package B: budget-derived reward / tier / drop-window math.
// ---------------------------------------------------------------------------

public class BountyBudgetTests
{
    [Theory]
    [InlineData(200f, BountyBudget.TierLow)]
    [InlineData(400f, BountyBudget.TierLow)]
    [InlineData(401f, BountyBudget.TierMid)]
    [InlineData(899f, BountyBudget.TierMid)]
    [InlineData(900f, BountyBudget.TierHigh)]
    [InlineData(5000f, BountyBudget.TierHigh)]
    public void TierFor_FollowsBudgetThresholds(float weekly, string expected)
        => Assert.Equal(expected, BountyBudget.TierFor(weekly));

    [Theory]
    [InlineData(BountyBudget.TierLow, 240)]
    [InlineData(BountyBudget.TierMid, 180)]
    [InlineData(BountyBudget.TierHigh, 120)]
    public void WindowMinutesFor_MapsTiers(string tier, int minutes)
        => Assert.Equal(minutes, BountyBudget.WindowMinutesFor(tier));

    [Fact]
    public void RewardForBudget_ScalesWithWeeklySpendAndClamps()
    {
        var rng = new Random(42);
        // 300/wk -> factor 0.35 -> 105 -> clamped up to MinReward.
        Assert.Equal(BountyBudget.MinReward, BountyBudget.RewardForBudget(300f, rng));
        // 2000/wk -> 700 -> in range, rounded to RewardRounding steps.
        float mid = BountyBudget.RewardForBudget(2000f, rng);
        Assert.InRange(mid, BountyBudget.MinReward, BountyBudget.MaxReward);
        Assert.Equal(0f, mid % BountyBudget.RewardRounding, 2);
        // 50000/wk -> clamped at MaxReward.
        Assert.Equal(BountyBudget.MaxReward, BountyBudget.RewardForBudget(50000f, rng));
    }

    [Fact]
    public void LegacyReward_StaysInsideOldBounds()
    {
        var rng = new Random(7);
        for (int i = 0; i < 50; i++)
        {
            Assert.InRange(BountyBudget.LegacyReward(rng), 200f, 500f);
        }
    }
}

// ---------------------------------------------------------------------------
// Package D: persisted call-plan behaviour (irregular, no replay, single-use).
// ---------------------------------------------------------------------------

public class BountyCallSchedulePlanTests
{
    [Fact]
    public void EnsurePlannedThrough_GeneratesSlotsInsideCallWindow()
    {
        var state = new BountyCallScheduleState();
        var rng = new Random(1234);
        // (state, today, nowMinuteOfDay, throughDay, callerCount, rng) — throughDay
        // is an exclusive generation bound.
        BountyCallSchedulePlan.EnsurePlannedThrough(state, 10, 800, 17, 3, rng);

        Assert.True(state.Slots.Count > 0);
        foreach (var s in state.Slots)
        {
            Assert.InRange(s.Day, 10, 16);
            Assert.InRange(s.MinuteOfDay,
                BountyCallSchedulePlan.EarliestMinuteOfDay,
                BountyCallSchedulePlan.LatestMinuteOfDay);
            Assert.InRange(s.CallerIndex, 0, 2);
        }
    }

    [Fact]
    public void PeekDue_DoesNotReplayAfterDispatch()
    {
        var state = new BountyCallScheduleState();

        // Force exactly one due slot at minute 600 today.
        state.Slots.Add(new BountyCallSlot { Day = 5, MinuteOfDay = 600, CallerIndex = 1 });

        var due = BountyCallSchedulePlan.PeekDue(state, 5, 620);
        Assert.NotNull(due);
        BountyCallSchedulePlan.ConfirmDispatch(due!);

        var again = BountyCallSchedulePlan.PeekDue(state, 5, 625);
        Assert.Null(again); // dispatched slots are never handed out twice
    }

    [Fact]
    public void PeekDue_CancelsSlotsMissedBeyondGrace()
    {
        var state = new BountyCallScheduleState();
        state.Slots.Add(new BountyCallSlot { Day = 5, MinuteOfDay = 500, CallerIndex = 0 });

        // 181 minutes later: beyond MissedGraceMinutes -> silently cancelled.
        var due = BountyCallSchedulePlan.PeekDue(state, 5, 500 + 181);
        Assert.Null(due);
        Assert.True(state.Slots[0].Cancelled);
        Assert.False(state.Slots[0].Dispatched);
    }

    [Fact]
    public void PeekDue_SkipsFutureSlots()
    {
        var state = new BountyCallScheduleState();
        state.Slots.Add(new BountyCallSlot { Day = 5, MinuteOfDay = 1000, CallerIndex = 0 });
        Assert.Null(BountyCallSchedulePlan.PeekDue(state, 5, 990));
    }

    [Fact]
    public void Reschedule_PushesSlotOutAtLeastTwoHours()
    {
        var state = new BountyCallScheduleState();
        var slot = new BountyCallSlot { Day = 5, MinuteOfDay = 500, CallerIndex = 0 };
        state.Slots.Add(slot);
        var rng = new Random(3);
        BountyCallSchedulePlan.Reschedule(slot, 5, 700, rng);
        Assert.True(slot.Cancelled || slot.MinuteOfDay > 700 || slot.Day > 5);
    }
}

// ---------------------------------------------------------------------------
// Package E: tracker semantics (dedupe, kill vs knockout, contract state).
// ---------------------------------------------------------------------------

public class BountyTrackerServiceTests
{
    private static BountyContract Contract(string id, string npcId, string name, EBountyStatus status) => new()
    {
        Id = id,
        TargetNpcId = npcId,
        TargetNpcName = name,
        Status = status
    };

    [Fact]
    public void Build_DedupesMultipleContractsOnTheSameTarget()
    {
        var save = new BountySaveData();
        save.Active.Add(Contract("c1", "ludwig_meyer", "Ludwig Meyer", EBountyStatus.Active));
        save.History.Add(Contract("c2", "ludwig_meyer", "Ludwig Meyer", EBountyStatus.Completed));
        save.KillEvents.Add(new BountyKillRecord
        {
            TargetNpcId = "ludwig_meyer",
            TargetName = "Ludwig Meyer",
            Day = 3,
            MinuteSum = 3 * 1440 + 600,
            Died = true,
            ContractId = "c2"
        });

        var snap = BountyTrackerService.Build(save);
        Assert.Single(snap.Rows); // two contracts, one record -> one row
        var row = snap.Rows[0];
        Assert.True(row.Died);
        Assert.True(row.ContractCompleted);
        Assert.True(row.ContractActive);
    }

    [Fact]
    public void Build_KeepsKnockoutDistinctFromConfirmedDeath()
    {
        var save = new BountySaveData();
        save.KillEvents.Add(new BountyKillRecord
        {
            TargetNpcId = "peter_smith",
            TargetName = "Peter Smith",
            Day = 2,
            MinuteSum = 2 * 1440 + 500,
            Died = false,
            ContractId = "c1"
        });

        var snap = BountyTrackerService.Build(save);
        Assert.Single(snap.Rows);
        Assert.False(snap.Rows[0].Died); // knockout only -> "Out of action"
        Assert.Equal(1, snap.OnlyOut);
        Assert.Equal(0, snap.ConfirmedDead);
    }

    [Fact]
    public void Build_UpgradesRecordWhenDeathIsConfirmedLater()
    {
        var save = new BountySaveData();
        save.KillEvents.Add(new BountyKillRecord
        {
            TargetNpcId = "peter_smith",
            TargetName = "Peter Smith",
            Day = 2,
            MinuteSum = 2 * 1440 + 500,
            Died = false,
            ContractId = "c1"
        });
        save.KillEvents.Add(new BountyKillRecord
        {
            TargetNpcId = "peter_smith",
            TargetName = "Peter Smith",
            Day = 2,
            MinuteSum = 2 * 1440 + 540,
            Died = true,
            ContractId = "c1"
        });

        var snap = BountyTrackerService.Build(save);
        Assert.Single(snap.Rows);
        Assert.True(snap.Rows[0].Died); // death wins over the earlier knockout
        Assert.Equal(1, snap.ConfirmedDead);
        Assert.Equal(0, snap.OnlyOut);
    }

    [Fact]
    public void Build_CountsContractCompletionSeparatelyFromKills()
    {
        var save = new BountySaveData();
        // Completed contract WITHOUT a kill record: possible via knockout polaroid.
        save.History.Add(Contract("c1", "anna_klein", "Anna Klein", EBountyStatus.Completed));

        var snap = BountyTrackerService.Build(save);
        Assert.Single(snap.Rows);
        Assert.False(snap.Rows[0].Died); // contract done != proof of death
        Assert.True(snap.Rows[0].ContractCompleted);
        Assert.Equal(1, snap.ContractsCompleted);
        Assert.Equal(0, snap.ConfirmedDead);
    }

    [Fact]
    public void FormatEventTime_RendersInGameClock()
        => Assert.Equal("Day 2 \u00b7 08:20", BountyTrackerService.FormatEventTime(2, 2 * 1440 + 500));
}

// ---------------------------------------------------------------------------
// Save compatibility: old save shapes must load with LEGACY semantics.
// ---------------------------------------------------------------------------

public class BountySaveCompatTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        IncludeFields = true,
        WriteIndented = true
    };

    [Fact]
    public void OldSaveJson_LoadsWithLegacyDefaults()
    {
        // Shape of a pre-budget/dead-drop save (v0.1.x era): no new fields at all.
        const string oldJson = """
        {
          "SaveIdentity": "slot1",
          "Active": [
            {
              "Id": "c1", "CallerId": "caller_viktor", "CallerStyle": "cold",
              "TargetNpcId": "ludwig_meyer", "TargetNpcName": "Ludwig Meyer",
              "RewardCash": 250.0, "OfferedAtDay": 4, "DeadlineDay": 7,
              "Status": 1, "EvidenceSpawned": true
            }
          ],
          "History": [], "CallerCooldowns": {}, "HeatState": {}
        }
        """;

        var save = JsonSerializer.Deserialize<BountySaveData>(oldJson, Options);
        Assert.NotNull(save);
        var c = Assert.Single(save!.Active);

        // Backward compatibility rules (never accidental "failed" meanings):
        Assert.False(c.DropDueStarted);        // no invented drop deadline
        Assert.Equal(0L, c.DropDueMinSum);
        Assert.Equal(0, c.DropWindowMinutes);  // legacy: no post-photo window
        Assert.Equal(0f, c.TargetWeeklySpend); // unknown budget -> legacy reward path
        Assert.True(string.IsNullOrEmpty(c.RewardTier));
        Assert.True(string.IsNullOrEmpty(c.AssignedDropName));
        Assert.NotNull(save.KillEvents);       // empty, not null
        Assert.Empty(save.KillEvents);
        Assert.NotNull(save.CallSchedule);     // lazily generated on the next tick
    }

    [Fact]
    public void LegacyRewardOnlineProperty_PopulatesRewardCash()
    {
        const string json = """{ "Id": "c1", "RewardOnline": 450.0 }""";
        var c = JsonSerializer.Deserialize<BountyContract>(json, Options);
        Assert.NotNull(c);
        Assert.Equal(450f, c!.RewardCash); // write-only shim feeds the new field
    }

    [Fact]
    public void RoundTrip_PreservesBudgetAndDropFields()
    {
        var save = new BountySaveData { SaveIdentity = "slot2" };
        save.Active.Add(new BountyContract
        {
            Id = "c7",
            TargetNpcId = "anna_klein",
            TargetNpcName = "Anna Klein",
            RewardCash = 700f,
            TargetWeeklySpend = 2000f,
            RewardTier = BountyBudget.TierHigh,
            RequiredDropId = "drop_hyland_02",
            AssignedDropName = "Hyland Point (2)",
            DropDueMinSum = 9 * 1440 + 830,
            DropDueStarted = true,
            DropWindowMinutes = 120
        });

        var json = JsonSerializer.Serialize(save, Options);
        var back = JsonSerializer.Deserialize<BountySaveData>(json, Options);
        Assert.NotNull(back);
        var c = Assert.Single(back!.Active);
        Assert.Equal(2000f, c.TargetWeeklySpend);
        Assert.Equal(BountyBudget.TierHigh, c.RewardTier);
        Assert.Equal("drop_hyland_02", c.RequiredDropId);
        Assert.Equal("Hyland Point (2)", c.AssignedDropName);
        Assert.Equal(9 * 1440 + 830, c.DropDueMinSum);
        Assert.True(c.DropDueStarted);
        Assert.Equal(120, c.DropWindowMinutes);
    }
}

using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

// ---------------------------------------------------------------------------
// HP-08 (tracker semantics) + R07 (tracker render retry) regression suite.
//
// Regression-first: these tests pin the behaviour the repaired sources must
// have. On the pre-repair sources the HP-08 cases fail (a contract-only row is
// counted as an elimination and two completed contracts collapse to one) and
// the R07 gate does not exist yet.
// ---------------------------------------------------------------------------

public class JournalTrackerServiceTests
{
    private static BountyContract Contract(string id, string npcId, string name, EBountyStatus status) => new()
    {
        Id = id,
        TargetNpcId = npcId,
        TargetNpcName = name,
        Status = status
    };

    private static BountyKillRecord Event(string npcId, string name, int day, long minuteSum, bool died) => new()
    {
        TargetNpcId = npcId,
        TargetName = name,
        Day = day,
        MinuteSum = minuteSum,
        Died = died,
        ContractId = "c-" + npcId
    };

    // ---- HP-08: a contract alone is NOT an elimination ---------------------

    [Fact]
    public void Build_UntouchedActiveContract_RecordsNoElimination()
    {
        var save = new BountySaveData();
        save.Active.Add(Contract("c1", "ludwig_meyer", "Ludwig Meyer", EBountyStatus.Active));

        var snap = BountyTrackerService.Build(save);

        var row = Assert.Single(snap.Rows);
        Assert.False(row.HasElimination);   // no event was ever recorded
        Assert.False(row.Died);
        Assert.True(row.ContractActive);
        Assert.Equal(0, snap.OnlyOut);        // "Out of action" must not be invented
        Assert.Equal(0, snap.ConfirmedDead);
    }

    [Fact]
    public void Build_UntouchedExpiredContract_RecordsNoElimination()
    {
        var save = new BountySaveData();
        save.History.Add(Contract("c1", "anna_klein", "Anna Klein", EBountyStatus.Expired));

        var snap = BountyTrackerService.Build(save);

        var row = Assert.Single(snap.Rows);
        Assert.False(row.HasElimination);
        Assert.Equal(0, snap.OnlyOut);
        Assert.Equal(0, snap.ConfirmedDead);
    }

    // ---- HP-08: three-way split (no event / KO / confirmed dead) -----------

    [Fact]
    public void Build_DistinguishesNoEventKnockoutAndConfirmedDeath()
    {
        var save = new BountySaveData();
        // Contract only — no event recorded for this target.
        save.Active.Add(Contract("c-none", "npc_none", "No Event", EBountyStatus.Active));
        // Knockout event recorded (Died == false).
        save.KillEvents.Add(Event("npc_ko", "Knock Out", day: 2, minuteSum: 2 * 1440 + 500, died: false));
        // Confirmed death recorded (Died == true).
        save.KillEvents.Add(Event("npc_dead", "Confirmed Dead", day: 3, minuteSum: 3 * 1440 + 600, died: true));

        var snap = BountyTrackerService.Build(save);

        Assert.Equal(3, snap.Rows.Count);
        Assert.Equal(1, snap.OnlyOut);        // exactly the KO row
        Assert.Equal(1, snap.ConfirmedDead);  // exactly the death row

        var none = snap.Rows.Find(r => r.TargetNpcId == "npc_none")!;
        var ko = snap.Rows.Find(r => r.TargetNpcId == "npc_ko")!;
        var dead = snap.Rows.Find(r => r.TargetNpcId == "npc_dead")!;
        Assert.False(none.HasElimination);
        Assert.True(ko.HasElimination);
        Assert.False(ko.Died);
        Assert.True(dead.HasElimination);
        Assert.True(dead.Died);
    }

    // ---- HP-08: contract count is per contract, not per row ----------------

    [Fact]
    public void Build_TwoCompletedContractsSameTarget_CountsTwoContractCompletions()
    {
        var save = new BountySaveData();
        save.History.Add(Contract("c1", "ludwig_meyer", "Ludwig Meyer", EBountyStatus.Completed));
        save.History.Add(Contract("c2", "ludwig_meyer", "Ludwig Meyer", EBountyStatus.Completed));

        var snap = BountyTrackerService.Build(save);

        Assert.Single(snap.Rows);                  // one unique target row
        Assert.Equal(2, snap.ContractsCompleted);  // but two completed contracts
        Assert.Equal(0, snap.OnlyOut);
        Assert.Equal(0, snap.ConfirmedDead);
    }

    [Fact]
    public void Build_ContractCountSeparateFromUniqueTargets()
    {
        var save = new BountySaveData();
        save.History.Add(Contract("c1", "npc_a", "A", EBountyStatus.Completed));
        save.History.Add(Contract("c2", "npc_b", "B", EBountyStatus.Completed));
        save.History.Add(Contract("c3", "npc_a", "A", EBountyStatus.Completed));

        var snap = BountyTrackerService.Build(save);

        Assert.Equal(2, snap.Rows.Count);          // two unique targets
        Assert.Equal(3, snap.ContractsCompleted);  // three completed contracts
    }
}

// ---------------------------------------------------------------------------
// R07 — the tracker render key must be committed ONLY after a successful
// rebuild, so a transient UI failure with unchanged data is retried instead of
// being masked until the app is reopened.
// ---------------------------------------------------------------------------

public class JournalTrackerRenderGateTests
{
    [Fact]
    public void NeedsRebuild_RepeatsUntilKeyCommitted()
    {
        var gate = new TrackerRenderGate();

        Assert.True(gate.NeedsRebuild("k1"));
        Assert.True(gate.NeedsRebuild("k1")); // nothing committed yet -> still pending

        gate.Commit("k1");
        Assert.False(gate.NeedsRebuild("k1")); // unchanged data -> no rebuild
        Assert.True(gate.NeedsRebuild("k2"));  // changed data -> rebuild
    }

    [Fact]
    public void FailedRebuild_KeepsKeyPending()
    {
        var gate = new TrackerRenderGate();
        string key = "k1";

        Assert.True(gate.NeedsRebuild(key));
        // A failed rebuild throws before Commit -> the gate stays pending and the
        // next tick retries even though the underlying data is unchanged.
        Assert.True(gate.NeedsRebuild(key));
    }

    [Fact]
    public void Reset_ForcesNextRebuild()
    {
        var gate = new TrackerRenderGate();
        gate.Commit("k1");
        Assert.False(gate.NeedsRebuild("k1"));

        gate.Reset();
        Assert.True(gate.NeedsRebuild("k1")); // e.g. app reopened
    }
}

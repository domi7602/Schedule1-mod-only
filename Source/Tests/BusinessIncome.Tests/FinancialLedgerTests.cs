using System.Collections.Generic;
using BusinessIncome.Core;
using BusinessIncome.Models;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>Monotone counters and exact-snapshot rollback.</summary>
public class FinancialLedgerTests
{
    private static PayoutState NewState(int lastPaid = -1, params (string id, int day)[] per)
    {
        var s = new PayoutState { SchemaVersion = 1, LastPaidElapsedDay = lastPaid, SaveIdentity = "slot_1" };
        foreach (var (id, day) in per) s.LastPaidDayByBusiness[id] = day;
        return s;
    }

    [Fact]
    public void MarkPaid_GlobalNeverDecreases()
    {
        var s = NewState(lastPaid: 10);
        PayoutLedger.MarkPaid(s, 5, new List<string>());
        Assert.Equal(10, s.LastPaidElapsedDay);
    }

    [Fact]
    public void MarkPaid_PerBusinessNeverDecreases()
    {
        var s = NewState(lastPaid: 0, ("a", 9));
        PayoutLedger.MarkPaid(s, 4, new List<string> { "a", "b" });
        Assert.Equal(9, s.LastPaidDayByBusiness["a"]);
        Assert.Equal(4, s.LastPaidDayByBusiness["b"]);
    }

    [Fact]
    public void MarkPaid_EmptyIdsAreIgnored()
    {
        var s = NewState(lastPaid: 0);
        PayoutLedger.MarkPaid(s, 3, new List<string> { "", null! });
        Assert.Empty(s.LastPaidDayByBusiness);
    }

    [Fact]
    public void CaptureAndMark_RecordsExpectedValues()
    {
        var s = NewState(lastPaid: 4, ("a", 2));
        var snap = PayoutLedger.CaptureAndMark(s, 7, new List<string> { "a", "b" });

        Assert.Equal(7, s.LastPaidElapsedDay);
        Assert.Equal(7, s.LastPaidDayByBusiness["a"]);
        Assert.Equal(7, s.LastPaidDayByBusiness["b"]);
        Assert.Equal(7, snap.ExpectedLastPaid);
        Assert.Equal(4, snap.PrevLastPaid);
        Assert.Equal(2, snap.PrevPerBusiness["a"]);
        Assert.Null(snap.PrevPerBusiness["b"]);
    }

    [Fact]
    public void RollbackExact_RestoresWhenMatching()
    {
        var s = NewState(lastPaid: 4, ("a", 2));
        var snap = PayoutLedger.CaptureAndMark(s, 7, new List<string> { "a", "b" });

        Assert.True(PayoutLedger.TryRollbackExact(s, snap));
        Assert.Equal(4, s.LastPaidElapsedDay);
        Assert.Equal(2, s.LastPaidDayByBusiness["a"]);
        Assert.False(s.LastPaidDayByBusiness.ContainsKey("b"));
    }

    [Fact]
    public void RollbackExact_RefusesOnGlobalDivergence()
    {
        var s = NewState(lastPaid: 4);
        var snap = PayoutLedger.CaptureAndMark(s, 7, new List<string>());
        s.LastPaidElapsedDay = 8; // diverged

        Assert.False(PayoutLedger.TryRollbackExact(s, snap));
        Assert.Equal(8, s.LastPaidElapsedDay); // untouched
    }

    [Fact]
    public void RollbackExact_RefusesOnPerBusinessDivergence()
    {
        var s = NewState(lastPaid: 4);
        var snap = PayoutLedger.CaptureAndMark(s, 7, new List<string> { "a" });
        s.LastPaidDayByBusiness["a"] = 9; // diverged

        Assert.False(PayoutLedger.TryRollbackExact(s, snap));
        Assert.Equal(9, s.LastPaidDayByBusiness["a"]);
    }

    [Fact]
    public void RollbackExact_RefusesWhenMarkedBusinessMissing()
    {
        var s = NewState(lastPaid: 4);
        var snap = PayoutLedger.CaptureAndMark(s, 7, new List<string> { "a" });
        s.LastPaidDayByBusiness.Remove("a");

        Assert.False(PayoutLedger.TryRollbackExact(s, snap));
    }

    [Fact] // Defect 10: "exact" also covers the untouched identity field
    public void RollbackExact_RefusesOnIdentityDivergence()
    {
        var s = NewState(lastPaid: 4);
        var snap = PayoutLedger.CaptureAndMark(s, 7, new List<string>());
        s.SaveIdentity = "some_other_slot"; // unrelated mutation after the snapshot

        Assert.False(PayoutLedger.TryRollbackExact(s, snap));
        Assert.Equal(7, s.LastPaidElapsedDay);
        Assert.Equal("some_other_slot", s.SaveIdentity);
    }

    [Fact] // Defect 10: "exact" also covers the untouched timestamp field
    public void RollbackExact_RefusesOnTimestampDivergence()
    {
        var s = NewState(lastPaid: 4);
        var snap = PayoutLedger.CaptureAndMark(s, 7, new List<string>());
        s.LastPayoutTimestamp = "modified-externally";

        Assert.False(PayoutLedger.TryRollbackExact(s, snap));
        Assert.Equal(7, s.LastPaidElapsedDay);
        Assert.Equal("modified-externally", s.LastPayoutTimestamp);
    }
}

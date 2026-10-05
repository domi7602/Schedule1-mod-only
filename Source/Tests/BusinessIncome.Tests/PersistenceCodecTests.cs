using System.Collections.Generic;
using BusinessIncome.Core;
using BusinessIncome.Models;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>
/// Codec/validation: fail-closed statuses (missing/corrupt/schema/identity/null-dictionary),
/// backup recovery, and robust marker clearing order.
/// </summary>
public class PersistenceCodecTests
{
    private const string Path = "state.json";

    private static PayoutState Valid(string identity = "slot_1", int lastPaid = 3) => new()
    {
        SchemaVersion = PayoutCodec.CurrentSchemaVersion,
        SaveIdentity = identity,
        LastPaidElapsedDay = lastPaid,
        LastPaidDayByBusiness = new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase) { ["a"] = 2 }
    };

    [Fact]
    public void MissingFile_IsMissing()
    {
        var s = new FakeStorage();
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.Missing, r.Status);
        Assert.False(r.BlocksMutation);
    }

    [Fact]
    public void ValidFile_IsOk()
    {
        var s = new FakeStorage();
        s.Seed(Path, PayoutCodec.EncodeState(Valid()));
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.Ok, r.Status);
        Assert.Equal(3, r.State!.LastPaidElapsedDay);
    }

    [Fact]
    public void CorruptJson_IsCorrupt_NeverSilentlySeeded()
    {
        var s = new FakeStorage();
        s.Seed(Path, "{ this is not json");
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.Corrupt, r.Status);
        Assert.True(r.BlocksMutation);
        Assert.Null(r.State);
    }

    [Fact]
    public void SchemaMismatch_IsDistinctStatus()
    {
        var s = new FakeStorage();
        var bad = Valid();
        bad.SchemaVersion = 99;
        s.Seed(Path, PayoutCodec.EncodeState(bad));
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.SchemaMismatch, r.Status);
        Assert.True(r.BlocksMutation);
    }

    [Fact]
    public void IdentityMismatch_IsDistinctStatus()
    {
        var s = new FakeStorage();
        s.Seed(Path, PayoutCodec.EncodeState(Valid(identity: "other_slot")));
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.IdentityMismatch, r.Status);
        Assert.True(r.BlocksMutation);
    }

    [Fact]
    public void NullDictionary_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(Path, "{\"SchemaVersion\":1,\"LastPaidElapsedDay\":2,\"SaveIdentity\":\"slot_1\",\"LastPaidDayByBusiness\":null}");
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.Corrupt, r.Status);
    }

    [Fact]
    public void StronglyNegativeDay_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(Path, PayoutCodec.EncodeState(Valid(lastPaid: -50)));
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.Corrupt, r.Status);
    }

    [Fact]
    public void UnreadableFile_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(Path, "whatever");
        s.FailRead.Add(Path);
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.Corrupt, r.Status);
    }

    [Fact]
    public void CorruptMain_ValidBackup_Recovers()
    {
        var s = new FakeStorage();
        s.Seed(Path, "{ broken");
        s.Seed(Path + ".bak", PayoutCodec.EncodeState(Valid(lastPaid: 7)));
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.RecoveredFromBackup, r.Status);
        Assert.Equal(7, r.State!.LastPaidElapsedDay);
    }

    [Fact]
    public void CorruptMain_CorruptBackup_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(Path, "{ broken");
        s.Seed(Path + ".bak", "{ also broken");
        var r = PayoutCodec.DecodeState(s, Path, "slot_1");
        Assert.Equal(StateStatus.Corrupt, r.Status);
        Assert.Null(r.State);
    }

    // --- pending marker -------------------------------------------------------------

    private const string Pend = "pending.json";

    [Fact]
    public void PendingMissing_IsMissing_DoesNotBlock()
    {
        var r = PayoutCodec.DecodePending(new FakeStorage(), Pend);
        Assert.Equal(PendingStatus.Missing, r.Status);
        Assert.False(r.BlocksBooking);
    }

    [Fact]
    public void PendingValid_Blocks()
    {
        var s = new FakeStorage();
        s.Seed(Pend, PayoutCodec.EncodePending(new PendingPayoutState { Day = 7, Amount = 120f, BusinessCount = 2 }));
        var r = PayoutCodec.DecodePending(s, Pend);
        Assert.Equal(PendingStatus.Valid, r.Status);
        Assert.True(r.BlocksBooking);
        Assert.Equal(7, r.Marker!.Day);
    }

    [Fact]
    public void PendingCorrupt_Blocks()
    {
        var s = new FakeStorage();
        s.Seed(Pend, "{ broken");
        var r = PayoutCodec.DecodePending(s, Pend);
        Assert.Equal(PendingStatus.Corrupt, r.Status);
        Assert.True(r.BlocksBooking);
    }

    [Fact]
    public void PendingCorruptMain_ValidBackup_RecoversAndBlocks()
    {
        var s = new FakeStorage();
        s.Seed(Pend, "{ broken");
        s.Seed(Pend + ".bak", PayoutCodec.EncodePending(new PendingPayoutState { Day = 4, Amount = 50f, BusinessCount = 1 }));
        var r = PayoutCodec.DecodePending(s, Pend);
        Assert.Equal(PendingStatus.Backup, r.Status);
        Assert.True(r.BlocksBooking);
        Assert.Equal(4, r.Marker!.Day);
    }

    [Fact]
    public void PendingNegativeDay_IsCorrupt()
    {
        var s = new FakeStorage();
        s.Seed(Pend, "{\"Day\":-9,\"Amount\":1,\"BusinessCount\":1,\"StartedUtc\":\"x\"}");
        var r = PayoutCodec.DecodePending(s, Pend);
        Assert.Equal(PendingStatus.Corrupt, r.Status);
    }

    // --- marker clearing ------------------------------------------------------------

    [Fact]
    public void ClearMarker_RemovesBakAndTmpFirst_ThenMain()
    {
        var s = new FakeStorage();
        s.Seed(Pend, "m");
        s.Seed(Pend + ".bak", "b");
        s.Seed(Pend + ".tmp", "t");

        Assert.True(PayoutCodec.ClearMarker(s, Pend));
        Assert.Equal(new[] { Pend + ".bak", Pend + ".tmp", Pend }, s.DeleteOrder.ToArray());
        Assert.False(s.Has(Pend));
        Assert.False(s.Has(Pend + ".bak"));
        Assert.False(s.Has(Pend + ".tmp"));
    }

    [Fact]
    public void ClearMarker_FailsWhenMainUndeletable()
    {
        var s = new FakeStorage();
        s.Seed(Pend, "m");
        s.FailDelete.Add(Pend);

        Assert.False(PayoutCodec.ClearMarker(s, Pend));
        Assert.True(s.Has(Pend)); // caller must fail closed
    }
}

using System;
using System.Collections.Generic;
using BusinessIncome.Core;
using BusinessIncome.Models;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>
/// Pre-schema (v0) payout state - a file with NO SchemaVersion but an otherwise valid required
/// shape, as written by builds that predate the schema marker. Such a file must load as Ok
/// (no permanent block, no forced fresh seed) and must be upgraded in place with the original
/// bytes backed up FIRST. Everything genuinely wrong - bad JSON, missing required fields,
/// invalid values, a present-but-wrong schema, a foreign identity - keeps failing closed and is
/// never rewritten.
/// </summary>
public class PersistenceSchemaMigrationTests
{
    private const string StatePath = "payout_state_slot_1.json";
    private const string PendingPath = "payout_pending_slot_1.json";
    private const string Slot = "slot_1";

    /// <summary>The real pre-schema shape: no SchemaVersion, no InitializationDay, no timestamp.</summary>
    private const string PreSchema =
        """
        {
          "LastPaidElapsedDay": 6,
          "SaveIdentity": "slot_1",
          "LastPaidDayByBusiness": { "nightclub": 5 }
        }
        """;

    private static FakeStorage WithPreSchema(string json = PreSchema)
    {
        var s = new FakeStorage();
        s.Seed(StatePath, json);
        return s;
    }

    // --- decode tolerance (the block that must never come back) ---------------------

    [Fact]
    public void PreSchemaState_DecodesAsOk_Usable_WithoutWriting()
    {
        var s = WithPreSchema();
        var r = PayoutCodec.DecodeState(s, StatePath, Slot);

        Assert.Equal(StateStatus.Ok, r.Status);
        Assert.True(r.Usable);
        Assert.False(r.BlocksMutation);
        Assert.Contains("pre-schema", r.Detail);
        Assert.Equal(6, r.State!.LastPaidElapsedDay);
        Assert.Equal(Slot, r.State.SaveIdentity);
        Assert.Equal(PayoutCodec.CurrentSchemaVersion, r.State.SchemaVersion);
        Assert.Equal(-1, r.State.InitializationDay);
        Assert.Equal(5, r.State.LastPaidDayByBusiness["nightclub"]);
        Assert.Equal(0, s.WriteCount); // decoding never touches disk
    }

    [Fact]
    public void PreSchemaState_BookingIsNotBlocked()
    {
        var s = WithPreSchema();
        var bank = new FakeBank();
        var engine = new PayoutSettlementEngine(s, bank);
        engine.SetActiveSlot(Slot);
        engine.ResolveOnLoadComplete(Slot);
        engine.LoadState(StatePath);
        engine.LoadPending(PendingPath);

        Assert.False(engine.StateLoad.BlocksMutation);

        var r = engine.TryBook(7, 100f, new[] { "nightclub", "laundry" }, StatePath, PendingPath, force: false);

        Assert.Equal(PayoutOutcome.Requested, r.Outcome);
        Assert.Equal(1, bank.Calls);
        Assert.Equal(7, engine.State!.LastPaidElapsedDay);
    }

    [Fact]
    public void PreSchemaState_IsNeverTreatedAsMissing_SoNoFreshSeedOverwritesIt()
    {
        var s = WithPreSchema();
        var engine = new PayoutSettlementEngine(s, new FakeBank());
        engine.SetActiveSlot(Slot);
        engine.ResolveOnLoadComplete(Slot);
        engine.LoadState(StatePath);
        engine.LoadPending(PendingPath);

        var r = engine.Initialize(4, StatePath);

        Assert.Equal(PayoutOutcome.Skipped, r.Outcome);
        Assert.Equal(0, s.WriteCount);
        Assert.Equal(PreSchema, s.Peek(StatePath));
    }

    // --- what still fails closed ----------------------------------------------------

    [Fact]
    public void PreSchemaState_WithoutDictionary_IsCorrupt()
    {
        var s = WithPreSchema("""{ "LastPaidElapsedDay": 6, "SaveIdentity": "slot_1" }""");
        var r = PayoutCodec.DecodeState(s, StatePath, Slot);
        Assert.Equal(StateStatus.Corrupt, r.Status);
        Assert.True(r.BlocksMutation);
    }

    [Fact]
    public void PreSchemaState_WithInvalidDay_IsCorrupt()
    {
        var s = WithPreSchema("""{ "LastPaidElapsedDay": -50, "SaveIdentity": "slot_1", "LastPaidDayByBusiness": {} }""");
        var r = PayoutCodec.DecodeState(s, StatePath, Slot);
        Assert.Equal(StateStatus.Corrupt, r.Status);
        Assert.True(r.BlocksMutation);
    }

    [Fact]
    public void PreSchemaState_WithForeignIdentity_IsIdentityMismatch()
    {
        var s = WithPreSchema(PreSchema.Replace("slot_1", "slot_2"));
        var r = PayoutCodec.DecodeState(s, StatePath, Slot);
        Assert.Equal(StateStatus.IdentityMismatch, r.Status);
        Assert.True(r.BlocksMutation);
        Assert.Null(r.State);
    }

    [Fact]
    public void PresentButWrongSchema_IsStillSchemaMismatch()
    {
        var s = WithPreSchema("""{ "SchemaVersion": 99, "LastPaidElapsedDay": 6, "SaveIdentity": "slot_1", "LastPaidDayByBusiness": {} }""");
        var r = PayoutCodec.DecodeState(s, StatePath, Slot);
        Assert.Equal(StateStatus.SchemaMismatch, r.Status);
        Assert.True(r.BlocksMutation);
    }

    // --- the upgrade itself ---------------------------------------------------------

    [Fact]
    public void Migrate_WritesBackupFirst_ThenMigratedState()
    {
        var s = WithPreSchema();

        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);

        Assert.Equal(SchemaMigrationStatus.Migrated, r.Status);
        Assert.Equal(new[] { StatePath + ".bak", StatePath }, s.WriteOrder.ToArray());
        Assert.Equal(PreSchema, s.Peek(StatePath + ".bak")); // original bytes survive first

        var after = PayoutCodec.DecodeState(s, StatePath, Slot);
        Assert.Equal(StateStatus.Ok, after.Status);
        Assert.Equal("ok", after.Detail); // canonical now, upgrade no longer pending
        Assert.Equal(PayoutCodec.CurrentSchemaVersion, after.State!.SchemaVersion);
        Assert.Equal(6, after.State.LastPaidElapsedDay);
        Assert.Equal(Slot, after.State.SaveIdentity);
        Assert.Equal(-1, after.State.InitializationDay);
        Assert.Equal(5, after.State.LastPaidDayByBusiness["nightclub"]);
    }

    [Fact]
    public void Migrate_IsIdempotent_SecondRunWritesNothing()
    {
        var s = WithPreSchema();
        Assert.Equal(SchemaMigrationStatus.Migrated, PayoutCodec.MigrateLegacySchema(s, StatePath, Slot).Status);
        int writesAfterFirst = s.WriteCount;

        var second = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);

        Assert.Equal(SchemaMigrationStatus.NotNeeded, second.Status);
        Assert.Equal(writesAfterFirst, s.WriteCount);
    }

    [Fact]
    public void Migrate_MissingFile_DoesNotWrite()
    {
        var s = new FakeStorage();
        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);
        Assert.Equal(SchemaMigrationStatus.MissingFile, r.Status);
        Assert.Equal(0, s.WriteCount);
    }

    [Fact]
    public void Migrate_CorruptContent_IsNeverRewritten()
    {
        var s = WithPreSchema("{ this is not json");
        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);
        Assert.Equal(SchemaMigrationStatus.NotLegacy, r.Status);
        Assert.Equal(0, s.WriteCount);
        Assert.Equal("{ this is not json", s.Peek(StatePath));
    }

    [Fact]
    public void Migrate_ForeignIdentity_IsNeverRewritten()
    {
        var s = WithPreSchema(PreSchema.Replace("slot_1", "slot_2"));
        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);
        Assert.Equal(SchemaMigrationStatus.IdentityMismatch, r.Status);
        Assert.Equal(0, s.WriteCount);
        Assert.Equal(PreSchema.Replace("slot_1", "slot_2"), s.Peek(StatePath));
    }

    [Fact]
    public void Migrate_UnknownPresentSchema_IsNeverRewritten()
    {
        const string unsupported = """{ "SchemaVersion": 99, "LastPaidElapsedDay": 6, "SaveIdentity": "slot_1", "LastPaidDayByBusiness": {} }""";
        var s = WithPreSchema(unsupported);
        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);
        Assert.Equal(SchemaMigrationStatus.NotNeeded, r.Status);
        Assert.Equal(0, s.WriteCount);
        Assert.Equal(unsupported, s.Peek(StatePath));
    }

    [Fact]
    public void Migrate_BackupWriteFails_OriginalLeftInPlace()
    {
        var s = WithPreSchema();
        s.FailWrite.Add(StatePath + ".bak");

        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);

        Assert.Equal(SchemaMigrationStatus.WriteFailed, r.Status);
        Assert.Equal(PreSchema, s.Peek(StatePath));
        Assert.False(s.Has(StatePath + ".bak"));
    }

    [Fact]
    public void Migrate_StateWriteFails_OriginalAndBackupLeftInPlace()
    {
        var s = WithPreSchema();
        s.FailWrite.Add(StatePath);

        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);

        Assert.Equal(SchemaMigrationStatus.WriteFailed, r.Status);
        Assert.Equal(PreSchema, s.Peek(StatePath));
        Assert.Equal(PreSchema, s.Peek(StatePath + ".bak"));
        // The still-pre-schema main file keeps loading as Ok - a failed upgrade never blocks.
        Assert.Equal(StateStatus.Ok, PayoutCodec.DecodeState(s, StatePath, Slot).Status);
    }

    [Fact]
    public void Migrate_LegacyStateWithEmptyIdentity_KeepsIdentityUntouched()
    {
        var s = WithPreSchema(PreSchema.Replace("\"slot_1\"", "\"\""));

        var r = PayoutCodec.MigrateLegacySchema(s, StatePath, Slot);

        Assert.Equal(SchemaMigrationStatus.Migrated, r.Status);
        var after = PayoutCodec.DecodeState(s, StatePath, Slot);
        Assert.Equal(StateStatus.Ok, after.Status);
        Assert.Equal("", after.State!.SaveIdentity);
        Assert.Equal(PayoutCodec.CurrentSchemaVersion, after.State.SchemaVersion);
    }
}

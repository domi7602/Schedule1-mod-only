using System;
using System.IO;
using BusinessIncome.Config;
using BusinessIncome.Core;
using BusinessIncome.Models;
using BusinessIncome.Services;
using S1Mods.Shared;
using Xunit;

namespace BusinessIncome.ConfigIo.Tests;

public sealed class RuntimePayoutIoTests : IDisposable
{
    private readonly string _root;
    private readonly string _previousRoot;
    private readonly string _slot;

    public RuntimePayoutIoTests()
    {
        _previousRoot = MelonLoader.Utils.MelonEnvironment.UserDataDirectory;
        _root = Path.Combine(Environment.GetEnvironmentVariable("TMPDIR") ?? Path.GetTempPath(), "bi-runtime-" + Guid.NewGuid().ToString("N"));
        MelonLoader.Utils.MelonEnvironment.UserDataDirectory = _root;
        SaveSlots.Active = new TestSaveInfo { SlotNumber = Random.Shared.Next(1000, int.MaxValue) };
        _slot = "slot_" + SaveSlots.Active.SlotNumber;
        NetworkGuard.Host = true;
        BusinessResolver.Readable = true;
        BusinessResolver.Owned.Clear();
        S1API.Money.Money.Requests = 0;
        PayoutStateStore.Reset();
    }

    public void Dispose()
    {
        PayoutStateStore.Reset();
        MelonLoader.Utils.MelonEnvironment.UserDataDirectory = _previousRoot;
        if (Directory.Exists(_root)) Directory.Delete(_root, true);
    }

    private void Write(string path, string text)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, text);
    }

    private string ValidState(int day = 3) => PayoutCodec.EncodeState(new PayoutState { SaveIdentity = _slot, LastPaidElapsedDay = day });

    [Fact]
    public void RecoveredLegacyBackup_CannotBeAutomaticallyAdopted()
    {
        string legacy = PayoutPaths.LegacyStateCandidates[0];
        Write(legacy, "{ broken main");
        Write(legacy + ".bak", ValidState());
        PayoutStateStore.LoadForActiveSlot();
        Assert.True(PayoutStateStore.StateLoad.BlocksMutation);
        Assert.False(File.Exists(PayoutPaths.State(_slot)));
        PayoutStateStore.LoadForActiveSlot();
        Assert.True(PayoutStateStore.StateLoad.BlocksMutation);
    }

    [Fact]
    public void TargetOrphanArtifact_CannotBeOverwrittenByMigration()
    {
        Write(PayoutPaths.State(_slot) + ".tmp", "unresolved interrupted write");
        Write(PayoutPaths.LegacyStateCandidates[0], ValidState());
        PayoutStateStore.LoadForActiveSlot();
        Assert.True(PayoutStateStore.StateLoad.BlocksMutation);
        Assert.False(File.Exists(PayoutPaths.State(_slot)));
        Assert.Equal("unresolved interrupted write", File.ReadAllText(PayoutPaths.State(_slot) + ".tmp"));
    }

    [Fact]
    public void ZeroBusinessPayout_StillHonorsAlreadyPaidGuard()
    {
        Write(PayoutPaths.State(_slot), ValidState());
        PayoutStateStore.LoadForActiveSlot();
        string before = File.ReadAllText(PayoutPaths.State(_slot));
        var result = IncomeEngine.ExecuteDailyPayout(3, new BusinessIncomeConfig());
        Assert.Equal(PayoutOutcome.AlreadyPaid, result.Outcome);
        Assert.Equal(before, File.ReadAllText(PayoutPaths.State(_slot)));
        Assert.False(File.Exists(PayoutPaths.State(_slot) + ".bak"));
    }

    [Fact]
    public void NoBusinessCatchup_ActuallyCommitsEveryPlannedDay()
    {
        Write(PayoutPaths.State(_slot), ValidState(0));
        PayoutStateStore.LoadForActiveSlot();
        var plan = PayoutScheduler.PlanCatchup(0, 4, 10, false, true);
        // The same branch contract used by Mod.CheckCatchupPayout.
        if (!plan.TerminalSkip)
            foreach (int day in plan.Days)
                Assert.Equal(PayoutOutcome.NoBusinesses, IncomeEngine.ExecuteDailyPayout(day, new BusinessIncomeConfig()).Outcome);
        Assert.Equal(4, PayoutStateStore.GetState().LastPaidElapsedDay);
        PayoutStateStore.Reset();
        PayoutStateStore.LoadForActiveSlot();
        Assert.Equal(4, PayoutStateStore.GetState().LastPaidElapsedDay);
        Assert.Equal(0, S1API.Money.Money.Requests);
    }

    [Fact]
    public void DryRunOnClient_WritesNothing()
    {
        NetworkGuard.Host = false;
        var result = IncomeEngine.ExecuteDailyPayout(5, new BusinessIncomeConfig(), isDryRun: true);
        Assert.Equal(PayoutOutcome.DryRun, result.Outcome);
        Assert.False(Directory.Exists(PayoutPaths.Directory));
        Assert.Equal(0, S1API.Money.Money.Requests);
    }

    // --- pre-schema (v0) payout state ------------------------------------------------

    private string PreSchemaState() =>
        $$"""
        {
          "LastPaidElapsedDay": 6,
          "SaveIdentity": "{{_slot}}",
          "LastPaidDayByBusiness": { "nightclub": 5 }
        }
        """;

    [Fact]
    public void PreSchemaState_LoadsUnblocked_AndIsUpgradedInPlace_WithBackup()
    {
        string statePath = PayoutPaths.State(_slot);
        string preSchema = PreSchemaState();
        Write(statePath, preSchema);

        PayoutStateStore.LoadForActiveSlot();

        // The missing schema marker must not block anything - no forced fresh seed either.
        Assert.Equal(StateStatus.Ok, PayoutStateStore.StateLoad.Status);
        Assert.False(PayoutStateStore.StateLoad.BlocksMutation);
        Assert.True(PayoutStateStore.IsReady);
        Assert.Equal(6, PayoutStateStore.GetState().LastPaidElapsedDay);

        // On disk: current schema written, the original bytes preserved as backup first.
        string upgraded = File.ReadAllText(statePath);
        Assert.Contains("\"SchemaVersion\": " + PayoutCodec.CurrentSchemaVersion, upgraded);
        Assert.Contains("\"InitializationDay\": -1", upgraded);
        Assert.Equal(preSchema, File.ReadAllText(statePath + ".bak"));
        Assert.False(File.Exists(statePath + ".tmp"));

        // A fresh load reads the upgraded file as an ordinary current-schema state.
        PayoutStateStore.Reset();
        PayoutStateStore.LoadForActiveSlot();
        Assert.Equal(StateStatus.Ok, PayoutStateStore.StateLoad.Status);
        Assert.DoesNotContain("pre-schema", PayoutStateStore.StateLoad.Detail);

        Assert.Contains(Mod.Log.Messages,
            m => m.StartsWith("INFO") && m.Contains("upgraded pre-schema payout state"));
    }

    [Fact]
    public void PreSchemaState_UpgradeWriteFailure_NeverBlocksTheState()
    {
        string statePath = PayoutPaths.State(_slot);
        string preSchema = PreSchemaState();
        Write(statePath, preSchema);
        // The backup target is occupied by a directory, so the backup write cannot succeed.
        Directory.CreateDirectory(statePath + ".bak");

        PayoutStateStore.LoadForActiveSlot();

        // The upgrade failed, but the state itself is valid: payouts stay unblocked and the
        // next load simply retries the upgrade instead of failing closed forever.
        Assert.Equal(StateStatus.Ok, PayoutStateStore.StateLoad.Status);
        Assert.False(PayoutStateStore.StateLoad.BlocksMutation);
        Assert.Equal(preSchema, File.ReadAllText(statePath));
        Assert.Contains(Mod.Log.Messages,
            m => m.StartsWith("WARN") && m.Contains("pre-schema payout state upgrade"));
    }
}

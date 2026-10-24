using System;
using System.Collections.Generic;
using System.IO;
using LightControl.Core;

namespace LightControl.Core.Tests;

/// <summary>
/// LC12 persistence-safety regression checks. Pure policy/JSON classification plus one
/// supplementary source contract; the adapter's real file-read path is exercised by the
/// executable harness documented in docs/persistence-repair-evidence.md.
///
/// Registration for the console host (parent integration, Program.cs):
///     foreach (var (name, run) in PersistenceRepairChecks.Cases()) tests.Add((name, run));
/// Cases() is self-contained: no new dependency, no file-system writes.
/// </summary>
internal static class PersistenceRepairChecks
{
    private const string SaveId = "slot-0";
    private const string Lamp =
        "{\"identity\":{\"kind\":2,\"scheme\":\"native-guid\",\"schemeVersion\":1,\"value\":\"9f1b6c2e-1f4a-4c3b-8a2d-2f6e5d4c3b1a\"}," +
        "\"colorMode\":4,\"intensityPercent\":75}";

    /// <summary>Every LC12 case, ready to append to the host's test table.</summary>
    public static (string Name, Action Run)[] Cases() => new (string, Action)[]
    {
        ("LC12-01 Proven absence of main and backup confirms missing data", ProvenAbsence),
        ("LC12-02 Unreadable main blocks even when a valid backup exists", UnreadableMainBlocks),
        ("LC12-03 A readable valid main is loaded from the main file", ValidMainLoads),
        ("LC12-04 Unknown schema main blocks even with a valid backup", UnknownSchemaMainBlocks),
        ("LC12-05 Foreign save-id main blocks even with a valid backup", ForeignMainBlocks),
        ("LC12-06 Corrupt main recovers from an independently validated backup", CorruptMainRecovers),
        ("LC12-07 Corrupt main with a foreign-save backup blocks", CorruptMainForeignBackup),
        ("LC12-08 Corrupt main with an unknown-schema backup blocks", CorruptMainUnsupportedBackup),
        ("LC12-09 Corrupt main with no backup blocks as invalid JSON", CorruptMainNoBackup),
        ("LC12-10 Corrupt main with an unreadable backup blocks", CorruptMainUnreadableBackup),
        ("LC12-11 Absent main with an unreadable backup blocks", AbsentMainUnreadableBackup),
        ("LC12-12 Absent main with a corrupt backup blocks as invalid JSON", AbsentMainCorruptBackup),
        ("LC12-13 Inspect never mutates or applies data by itself", InspectIsPure),
        ("LC12-14 The adapter reads outcomes and never consults File.Exists", AdapterUsesReadOutcomes),
        ("LC12-15 A later explicit read failure blocks an already-ready gate", LaterReadFailureBlocks),
    };

    // ------------------------------------------------------------------ cases

    private static void ProvenAbsence()
    {
        var plan = Plan(Absent(), Absent());
        Equal(PersistenceLoadAction.ConfirmMissing, plan.Action);
        Equal(PersistenceSource.None, plan.Source);
        Equal<string?>(null, plan.Json);
    }

    private static void UnreadableMainBlocks()
    {
        // A valid backup must never stand in for a main file whose bytes are unknown.
        var plan = Plan(Unreadable(), Present(ValidJson(SaveId)));
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.InvalidState, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
        Equal<string?>(null, plan.Json);
        Contains(plan.Detail, "could not be read");
    }

    private static void ValidMainLoads()
    {
        var plan = Plan(Present(ValidJson(SaveId, Lamp)), Absent());
        Equal(PersistenceLoadAction.LoadJson, plan.Action);
        Equal(PersistenceSource.Main, plan.Source);
        Equal(true, plan.Json is not null);
    }

    private static void UnknownSchemaMainBlocks()
    {
        // Intact but newer data: the backup is a stale copy and must not replace it.
        var plan = Plan(Present("{\"schemaVersion\":99,\"saveId\":\"slot-0\",\"lamps\":[]}"), Present(ValidJson(SaveId, Lamp)));
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.UnsupportedSchema, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
        Contains(plan.Detail, "retained");
    }

    private static void ForeignMainBlocks()
    {
        var plan = Plan(Present(ValidJson("slot-7", Lamp)), Present(ValidJson(SaveId, Lamp)));
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.WrongSave, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
    }

    private static void CorruptMainRecovers()
    {
        var plan = Plan(Present("{not json"), Present(ValidJson(SaveId, Lamp)));
        Equal(PersistenceLoadAction.LoadJson, plan.Action);
        Equal(PersistenceSource.Backup, plan.Source);
        Equal(PersistenceError.None, plan.Error);
        Contains(plan.Detail, "backup");
        // The planned JSON is exactly the backup text and is accepted by the runtime seam.
        Equal(ValidJson(SaveId, Lamp), plan.Json);
        WithStore(persistence =>
        {
            Equal(true, persistence.LoadJson(plan.Json!).Success);
            Equal(PersistenceGate.Ready, persistence.Gate);
        });
    }

    private static void CorruptMainForeignBackup()
    {
        var plan = Plan(Present("{not json"), Present(ValidJson("slot-7", Lamp)));
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.WrongSave, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
    }

    private static void CorruptMainUnsupportedBackup()
    {
        var plan = Plan(Present("{not json"), Present("{\"schemaVersion\":99,\"saveId\":\"slot-0\",\"lamps\":[]}"));
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.UnsupportedSchema, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
    }

    private static void CorruptMainNoBackup()
    {
        var plan = Plan(Present("{not json"), Absent());
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.InvalidJson, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
    }

    private static void CorruptMainUnreadableBackup()
    {
        var plan = Plan(Present("{not json"), Unreadable());
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.InvalidState, plan.Error);
        Contains(plan.Detail, "backup");
        Equal(PersistenceSource.None, plan.Source);
    }

    private static void AbsentMainUnreadableBackup()
    {
        var plan = Plan(Absent(), Unreadable());
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.InvalidState, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
    }

    private static void AbsentMainCorruptBackup()
    {
        var plan = Plan(Absent(), Present("{not json"));
        Equal(PersistenceLoadAction.ReadFailure, plan.Action);
        Equal(PersistenceError.InvalidJson, plan.Error);
        Equal(PersistenceSource.None, plan.Source);
    }

    private static void InspectIsPure()
    {
        WithStore(persistence =>
        {
            var valid = JsonStatePersistence.Inspect(ValidJson(SaveId, Lamp), SaveId);
            Equal(PersistenceDataStatus.Valid, valid.Status);
            Equal(1, valid.States.Count);
            Equal(PersistenceGate.NotLoaded, persistence.Gate);

            Equal(PersistenceDataStatus.UnsupportedSchema,
                JsonStatePersistence.Inspect("{\"schemaVersion\":2,\"saveId\":\"slot-0\",\"lamps\":[]}", SaveId).Status);
            Equal(PersistenceDataStatus.WrongSave,
                JsonStatePersistence.Inspect(ValidJson("slot-3"), SaveId).Status);
            Equal(PersistenceDataStatus.CorruptJson,
                JsonStatePersistence.Inspect("{not json", SaveId).Status);
            Equal(PersistenceDataStatus.InvalidState,
                JsonStatePersistence.Inspect(ValidJson(SaveId, Lamp, Lamp), SaveId).Status);
            Equal(PersistenceGate.NotLoaded, persistence.Gate);
        });
    }

    private static void AdapterUsesReadOutcomes()
    {
        string source = ReadSource("LightPersistence.cs");
        // File.Exists reports false for unreadable/unreachable paths, which the old adapter
        // misread as "missing data". The load decision must come from read outcomes only.
        Equal(false, source.Contains("File.Exists(", StringComparison.Ordinal));
        Equal(true, source.Contains("PersistenceSlotPolicy.PlanLoad", StringComparison.Ordinal));
        Equal(true, source.Contains("LastLoadResult", StringComparison.Ordinal));
        Equal(true, source.Contains("LoadedFromBackup", StringComparison.Ordinal));
        // A recovered session must not rotate the validated backup away with the old main text.
        Equal(true, source.Contains("PreserveBackup", StringComparison.Ordinal));
        Equal(true, source.Contains("EnumerateFileSystemEntries", StringComparison.Ordinal));
    }

    /// <summary>
    /// Regression guard for the existing C62 contract: an explicit storage read failure reported
    /// after a successful load must still block, because writing over storage that just failed a
    /// read risks the only good copy.
    /// </summary>
    private static void LaterReadFailureBlocks()
    {
        WithStore(persistence =>
        {
            Equal(true, persistence.ConfirmDataMissing().Success);
            Equal(PersistenceGate.Ready, persistence.Gate);
            var failure = persistence.ReportReadFailure();
            Equal(false, failure.Success);
            Equal(PersistenceGate.Blocked, persistence.Gate);
            Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
        });
    }

    // ------------------------------------------------------------------ helpers

    private static PersistenceLoadPlan Plan(PersistenceReadOutcome main, PersistenceReadOutcome backup)
        => PersistenceSlotPolicy.PlanLoad(main, backup, json => JsonStatePersistence.Inspect(json, SaveId));

    private static PersistenceReadOutcome Absent() => PersistenceReadOutcome.Absent("Proven absent: parent directory was listed.");
    private static PersistenceReadOutcome Unreadable() => PersistenceReadOutcome.Unreadable("IOException: the file is locked.");
    private static PersistenceReadOutcome Present(string json) => PersistenceReadOutcome.Present(json);

    private static string ValidJson(string saveId, params string[] lamps)
        => $"{{\"schemaVersion\":1,\"saveId\":\"{saveId}\",\"lamps\":[{string.Join(",", lamps)}]}}";

    private static void WithStore(Action<JsonStatePersistence> action)
    {
        var store = new LightStateStore();
        store.BeginSession(Guid.Parse("78aca78a-3f15-45e4-9c66-352a65a2a407"), SaveId, SessionMode.Host);
        action(new JsonStatePersistence(store));
    }

    private static string ReadSource(string name)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            string file = Path.Combine(dir.FullName, "Source", "Mods", "LightControl", "src", name);
            if (File.Exists(file)) return File.ReadAllText(file);
            dir = dir.Parent;
        }
        string? root = Environment.GetEnvironmentVariable("LC12_REPO");
        if (!string.IsNullOrEmpty(root))
        {
            string file = Path.Combine(root, "Source", "Mods", "LightControl", "src", name);
            if (File.Exists(file)) return File.ReadAllText(file);
        }
        throw new InvalidOperationException($"Repository source not found: {name}");
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
    }

    private static void Throws<TException>(Action action) where TException : Exception
    {
        try { action(); }
        catch (TException) { return; }
        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }

    private static void Contains(string text, string expected)
    {
        if (!text.Contains(expected, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Expected '{expected}' in: {text}");
    }
}

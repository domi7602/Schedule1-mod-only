using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

// Runs the production subtype/save orchestration against an explicit offline port.
// Fake completion evidence does not establish the native runtime contract.
public sealed class BusinessSaveIntegrationRegressionTests
{
    private static ConstructionContext World => new(true, true, true, true,
        Path.Combine(Path.GetTempPath(), "TownHallSaveIntegration", "SaveGame_2"),
        Path.Combine(Path.GetTempPath(), "TownHallSaveIntegration", "SaveGame_2"), "TownHallTest");

    [Fact]
    public void Repeated_folder_inspection_preserves_diagnostics_without_starting_a_save()
    {
        var port = new Port();
        for (int read = 0; read < 3; read++) Assert.Equal(0, port.ReadSaveState().Started);
        Assert.Equal(3, port.Callbacks.Calls("get_ShouldSaveUnderFolder"));
        Assert.False(port.Callbacks.SaveCallbackObserved);

        var result = new BusinessSubtypeTrial().Run(World, port, probeSave: true);

        Assert.Equal("observed", result.Status);
        Assert.True(result.CleanupComplete);
        Assert.Equal(1, port.SaveCalls);
        Assert.Equal(1, port.Callbacks.Calls("GetSaveString"));
        Assert.Equal(1, port.Callbacks.Calls("WriteData"));
        Assert.True(port.Callbacks.Calls("get_ShouldSaveUnderFolder") > 3);
        Assert.True(port.Callbacks.SaveCallbackObserved);
    }

    [Theory]
    [InlineData("GetSaveString")]
    [InlineData("WriteData")]
    public void Actual_save_callback_before_probe_blocks_registration_and_cleanup(string callback)
    {
        var port = new Port();
        port.Callbacks.Record(callback);

        var result = new BusinessSubtypeTrial().Run(World, port, probeSave: true);

        Assert.Equal("failed", result.Status);
        Assert.NotNull(result.Save);
        Assert.Contains("manager-save-preflight-invalid", result.Save!.Findings);
        Assert.False(result.CleanupComplete);
        Assert.Equal(0, port.RegisterCalls);
        Assert.Equal(0, port.SaveCalls);
        Assert.Equal(0, port.RemoveCalls);
        Assert.Equal(0, port.CleanupCalls);
    }

    [Fact]
    public void Non_save_diagnostics_remain_counted_without_save_callback_evidence()
    {
        var ledger = new BusinessSaveCallbackLedger();
        string[] diagnostics = { "get_ShouldSaveUnderFolder", "Awake", "ShouldSave", "CanBePurchased" };
        foreach (string diagnostic in diagnostics) ledger.Record(diagnostic);

        Assert.False(ledger.SaveCallbackObserved);
        foreach (string diagnostic in diagnostics) Assert.Equal(1, ledger.Calls(diagnostic));
        Assert.Equal(diagnostics.Length, ledger.Snapshot().Count);
    }

    [Theory]
    [InlineData("GetSaveString")]
    [InlineData("WriteData")]
    public void Save_callback_evidence_remains_latched_through_later_inspection(string callback)
    {
        var ledger = new BusinessSaveCallbackLedger();
        ledger.Record(callback);
        ledger.Record("get_ShouldSaveUnderFolder");
        ledger.Record("get_ShouldSaveUnderFolder");

        Assert.True(ledger.SaveCallbackObserved);
        Assert.Equal(1, ledger.Calls(callback));
        Assert.Equal(2, ledger.Calls("get_ShouldSaveUnderFolder"));
    }

    [Fact]
    public void Confirmed_save_is_removed_before_cleanup_and_cannot_be_repeated()
    {
        var port = new Port();
        var trial = new BusinessSubtypeTrial();

        var result = trial.Run(World, port, probeSave: true);

        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.NotNull(result.Save);
        Assert.True(result.Save!.SaveAttempted);
        Assert.True(result.Save.SaveReturned);
        Assert.True(result.Save.CompletionConfirmed);
        Assert.True(result.Save.SafeToDestroy);
        Assert.True(result.CleanupComplete);
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(1, port.SaveCalls);
        Assert.Equal(1, port.RemoveCalls);
        Assert.Equal(1, port.CleanupCalls);
        Assert.Equal(0, port.State.OwnEntries);
        Assert.True(port.Events.IndexOf("remove") < port.Events.IndexOf("cleanup"));
        Assert.Contains("read", port.Events.Skip(port.Events.IndexOf("remove") + 1)
            .Take(port.Events.IndexOf("cleanup") - port.Events.IndexOf("remove") - 1));

        int calls = port.Events.Count;
        var again = trial.Run(World, port, probeSave: true);
        Assert.Equal("skipped", again.Status);
        Assert.Contains("attempt-already-consumed", again.Findings);
        Assert.Equal(calls, port.Events.Count);
    }

    [Theory]
    [InlineData("registration-throws", 0, 0)]
    [InlineData("save-throws", 1, 0)]
    [InlineData("completion-missing", 1, 0)]
    [InlineData("context-loss", 1, 0)]
    [InlineData("removal-throws", 1, 1)]
    [InlineData("removal-readback", 1, 1)]
    public void Possible_manager_effect_retains_participant_until_removal_is_confirmed(
        string failure, int saves, int removals)
    {
        var port = new Port { Failure = failure };
        var trial = new BusinessSubtypeTrial();

        var result = trial.Run(World, port, () => port.ContextCurrent, probeSave: true);

        Assert.Equal("failed", result.Status);
        Assert.NotNull(result.Save);
        Assert.True(result.Save!.RegistrationAttempted);
        Assert.False(result.Save.SafeToDestroy);
        Assert.False(result.CleanupComplete);
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(saves, port.SaveCalls);
        Assert.Equal(removals, port.RemoveCalls);
        Assert.Equal(0, port.CleanupCalls);
        Assert.False(port.IsClean);
        Assert.Equal(1, port.State.OwnEntries);
        Assert.DoesNotContain("cleanup", port.Events);

        int calls = port.Events.Count;
        port.ContextCurrent = true;
        Assert.Equal("skipped", trial.Run(World, port, probeSave: true).Status);
        Assert.Equal(calls, port.Events.Count);
    }

    [Fact]
    public void Unverified_contract_can_clean_a_readable_unregistered_object()
    {
        var port = new Port { Failure = "unverified" };

        var result = new BusinessSubtypeTrial().Run(World, port, probeSave: true);

        Assert.Equal("failed", result.Status);
        Assert.True(result.CleanupComplete);
        Assert.Equal(0, port.RegisterCalls);
        Assert.Equal(0, port.SaveCalls);
        Assert.Equal(0, port.RemoveCalls);
        Assert.Equal(1, port.CleanupCalls);
        Assert.Equal(0, port.State.OwnEntries);
        Assert.True(result.Save == null || !result.Save.RegistrationAttempted);
    }

    [Theory]
    [InlineData("busy")]
    [InlineData("first-read-throws")]
    [InlineData("second-read-throws")]
    [InlineData("own-reference")]
    [InlineData("started-before-probe")]
    [InlineData("completed-before-probe")]
    [InlineData("active-callback")]
    [InlineData("pending-request")]
    public void Uncertain_preflight_retains_object_without_registration_or_save(string failure)
    {
        var port = new Port { Failure = failure };

        var result = new BusinessSubtypeTrial().Run(World, port, probeSave: true);

        Assert.Equal("failed", result.Status);
        Assert.False(result.CleanupComplete);
        Assert.Equal(0, port.RegisterCalls);
        Assert.Equal(0, port.SaveCalls);
        Assert.Equal(0, port.RemoveCalls);
        Assert.Equal(0, port.CleanupCalls);
        Assert.DoesNotContain("cleanup", port.Events);
        Assert.True(result.Save == null || !result.Save.RegistrationAttempted);
    }

    [Fact]
    public void Context_change_after_confirmed_removal_still_prevents_native_cleanup()
    {
        var port = new Port { Failure = "context-loss-after-removal" };

        var result = new BusinessSubtypeTrial().Run(World, port, () => port.ContextCurrent, probeSave: true);

        Assert.Equal("failed", result.Status);
        Assert.NotNull(result.Save);
        Assert.True(result.Save!.SafeToDestroy);
        Assert.True(result.Save.CompletionConfirmed);
        Assert.False(result.CleanupComplete);
        Assert.Equal(1, port.RemoveCalls);
        Assert.Equal(0, port.State.OwnEntries);
        Assert.Equal(0, port.CleanupCalls);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void Save_probe_rejects_main_slot_without_any_port_access()
    {
        string mainSlot = Path.Combine(Path.GetTempPath(), "TownHallSaveIntegration", "SaveGame_1");
        var wrongWorld = World with { ActiveSavePath = mainSlot, LoadedSavePath = mainSlot };
        var port = new Port();
        var trial = new BusinessSubtypeTrial();

        var result = trial.Run(wrongWorld, port, probeSave: true);

        Assert.Equal("skipped", result.Status);
        Assert.False(result.AttemptConsumed);
        Assert.Empty(port.Events);
        Assert.False(trial.AttemptConsumed);
    }

    private sealed class Port : IBusinessSubtypePort, IBusinessSavePort
    {
        private static readonly string[] RegistryNames =
        {
            "properties", "properties-owned", "properties-unowned", "businesses", "businesses-owned",
            "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests"
        };
        private int _reads;
        private bool _completionEntry;
        public readonly List<string> Events = new();
        public readonly BusinessSaveCallbackLedger Callbacks = new();
        public string Failure = "";
        public bool ContextCurrent = true;
        public ManagerSaveState State = new(World, 17, 101, 202, 1000,
            false, 0, 0, 0, 0, 0, "foreign-A;foreign-B", CompletionContractVerified: true);
        public int RegisterCalls, SaveCalls, RemoveCalls, CleanupCalls;
        public bool IsClean { get; private set; }

        public SubtypeSnapshot Capture()
        {
            Events.Add("capture");
            if (Failure == "context-loss-after-removal" && RemoveCalls > 0) ContextCurrent = false;
            var registries = RegistryNames.ToDictionary(name => name, _ => "baseline");
            registries["saveables"] = State.OwnEntries == 0 ? "baseline" : "baseline-plus-participant";
            registries["completed-saveables"] = _completionEntry ? "participant" : "baseline";
            return new(registries, new Dictionary<string, string> { ["foreign-class"] = "unchanged" });
        }
        public void RegisterType() => Events.Add("register-type");
        public void Prepare() => Events.Add("prepare");
        public SubtypeReadback Inspect()
        {
            Events.Add("inspect");
            return new(true, true, true, true, true, 0);
        }
        public SubtypeDispatch InvokeThroughNativeBase()
        {
            Events.Add("dispatch");
            return new(false, false, 1, 1);
        }
        public void Cleanup()
        {
            Events.Add("cleanup");
            CleanupCalls++;
            IsClean = true;
        }
        public ManagerSaveState ReadSaveState()
        {
            Events.Add("read");
            _reads++;
            if ((Failure == "first-read-throws" && _reads == 1) ||
                (Failure == "second-read-throws" && _reads == 2))
                throw new InvalidOperationException("unreadable-manager");
            // Same observation order as NativeSubtypePort: existing callback evidence
            // is read first, and the contract inspection then invokes the folder getter.
            if (Callbacks.SaveCallbackObserved) State = State with { Started = 1 };
            Callbacks.Record("get_ShouldSaveUnderFolder");
            State = State with
            {
                Frame = State.Frame + 1,
                CompletionContractVerified = Failure != "unverified",
                IsSaving = Failure == "busy",
                OwnEntries = Failure == "own-reference" ? 1 : State.OwnEntries,
                Started = Failure == "started-before-probe" ? 1 : State.Started,
                Completed = Failure == "completed-before-probe" ? 1 : State.Completed,
                ActiveCallbacks = Failure == "active-callback" ? 1 : 0,
                PendingRequests = Failure == "pending-request" ? 1 : 0
            };
            return State;
        }
        public void RegisterSaveable()
        {
            Events.Add("register");
            RegisterCalls++;
            State = State with { OwnEntries = 1 };
            if (Failure == "registration-throws") throw new InvalidOperationException("partial-registration");
        }
        public void SaveOnce()
        {
            Events.Add("save");
            SaveCalls++;
            Callbacks.Record("GetSaveString");
            if (Failure == "save-throws") throw new InvalidOperationException("unknown-save-effect");
            Callbacks.Record("WriteData");
            Callbacks.Record("get_ShouldSaveUnderFolder");
            if (Failure != "completion-missing")
            {
                State = State with { Completed = 1 };
                _completionEntry = true;
            }
            if (Failure == "context-loss") ContextCurrent = false;
        }
        public void RemoveSaveable()
        {
            Events.Add("remove");
            RemoveCalls++;
            if (Failure == "removal-throws") throw new InvalidOperationException("unknown-removal-effect");
            if (Failure == "removal-readback") return;
            State = State with { OwnEntries = 0 };
            _completionEntry = false;
        }
    }
}

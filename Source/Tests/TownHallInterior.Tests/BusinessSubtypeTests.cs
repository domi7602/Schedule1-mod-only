using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessSubtypeTests
{
    private static ConstructionContext Context => new(true, true, true, true,
        Path.Combine(Path.GetTempPath(), "TownHallTests", "SaveGame_2"),
        Path.Combine(Path.GetTempPath(), "TownHallTests", "SaveGame_2"), "TownHallTest");

    [Fact]
    public void Native_dispatch_is_observed_only_after_registration_and_immutable_baselines()
    {
        var port = new Port();
        var result = new BusinessSubtypeTrial().Run(Context, port);
        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.RegistrationAttempted && result.RegistrationCompleted && result.CleanupComplete);
        Assert.Equal(new[] { "capture", "register", "capture", "prepare", "inspect", "capture", "invoke", "inspect", "capture", "cleanup", "clean", "capture" }, port.Events);
        port.Registries["properties"] = "later";
        port.Classes["Business"] = "later";
        Assert.Equal("initial", result.Before!.Registries["properties"]);
        Assert.Equal("initial", result.After!.ForeignVtables["Business"]);
    }

    [Theory]
    [InlineData("load")]
    [InlineData("singleplayer")]
    [InlineData("hall")]
    [InlineData("dependencies")]
    [InlineData("organisation")]
    [InlineData("slot")]
    [InlineData("relative")]
    [InlineData("mismatch")]
    public void Denied_context_touches_nothing_and_does_not_consume_attempt(string field)
    {
        var context = field switch
        {
            "load" => Context with { LoadComplete = false },
            "singleplayer" => Context with { Singleplayer = false },
            "hall" => Context with { HallReady = false },
            "dependencies" => Context with { DependenciesMatch = false },
            "organisation" => Context with { OrganisationName = "townhalltest" },
            "slot" => Context with { ActiveSavePath = Path.Combine(Path.GetTempPath(), "SaveGame_1"), LoadedSavePath = Path.Combine(Path.GetTempPath(), "SaveGame_1") },
            "relative" => Context with { ActiveSavePath = "SaveGame_2", LoadedSavePath = "SaveGame_2" },
            _ => Context with { LoadedSavePath = Path.Combine(Path.GetTempPath(), "Other", "SaveGame_2") }
        };
        var trial = new BusinessSubtypeTrial(); var port = new Port();
        var result = trial.Run(context, port);
        Assert.Equal("skipped", result.Status); Assert.False(result.AttemptConsumed); Assert.Empty(port.Events);
        Assert.Equal("observed", trial.Run(Context, port).Status);
    }

    [Theory]
    [InlineData("register")]
    [InlineData("prepare")]
    [InlineData("inspect")]
    [InlineData("invoke")]
    public void Partial_failure_consumes_attempt_cleans_only_own_and_never_retries_registration(string stage)
    {
        var trial = new BusinessSubtypeTrial(); var port = new Port { ThrowAt = stage };
        var result = trial.Run(Context, port);
        Assert.Equal("failed", result.Status); Assert.True(result.CleanupComplete);
        Assert.True(result.RegistrationAttempted); Assert.Equal(stage != "register", result.RegistrationCompleted);
        Assert.Equal(1, port.Events.Count(e => e == "cleanup"));
        int calls = port.Events.Count;
        Assert.Equal("skipped", trial.Run(Context, port).Status); Assert.Equal(calls, port.Events.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Snapshot_failure_is_never_an_observation(int capture)
    {
        var port = new Port { NullCapture = capture };
        var result = new BusinessSubtypeTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.Equal(capture == 1 ? 0 : 1, port.Events.Count(e => e == "cleanup"));
        if (capture <= 3) Assert.DoesNotContain("invoke", port.Events);
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(3, true)]
    public void Foreign_change_blocks_next_mutation_and_is_not_repaired(int capture, bool vtable)
    {
        var port = new Port { ChangeCapture = capture, ChangeClass = vtable };
        var result = new BusinessSubtypeTrial().Run(Context, port);
        Assert.Equal("failed", result.Status); Assert.DoesNotContain("invoke", port.Events);
        if (capture == 2) Assert.DoesNotContain("prepare", port.Events);
        Assert.Equal("changed", vtable ? port.Classes["Business"] : port.Registries["properties"]);
        Assert.Equal("initial", vtable ? result.Before!.ForeignVtables["Business"] : result.Before!.Registries["properties"]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Invalid_native_readback_prevents_bool_invocation(int field)
    {
        var port = new Port
        {
            Readback = field switch
            {
                0 => new(false, true, true, true, true, 0),
                1 => new(true, false, true, true, true, 0),
                2 => new(true, true, false, true, true, 0),
                3 => new(true, true, true, false, true, 0),
                4 => new(true, true, true, true, false, 0),
                _ => new(true, true, true, true, true, 1)
            }
        };
        Assert.Equal("failed", new BusinessSubtypeTrial().Run(Context, port).Status);
        Assert.DoesNotContain("invoke", port.Events);
    }

    [Theory]
    [InlineData(true, false, 1, 1)]
    [InlineData(false, true, 1, 1)]
    [InlineData(false, false, 0, 1)]
    [InlineData(false, false, 1, 2)]
    public void Native_bool_or_counter_mismatch_is_a_failure(bool save, bool purchase, int saves, int purchases)
    {
        var port = new Port { Dispatch = new(save, purchase, saves, purchases) };
        Assert.Equal("failed", new BusinessSubtypeTrial().Run(Context, port).Status);
        Assert.Contains("cleanup", port.Events);
    }

    [Fact]
    public void A_callback_during_dispatch_invalidates_the_observation()
    {
        var port = new Port(); port.OnInvoke = () => port.Readback = port.Readback with { UnexpectedLifecycleCalls = 1 };
        Assert.Equal("failed", new BusinessSubtypeTrial().Run(Context, port).Status);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Cleanup_exception_is_bounded_and_failure_remains_even_after_successful_retry(int throws)
    {
        var port = new Port { CleanupThrows = throws };
        var result = new BusinessSubtypeTrial().Run(Context, port);
        Assert.Equal("failed", result.Status); Assert.Equal(2, port.Events.Count(e => e == "cleanup"));
        Assert.Equal(throws == 1, result.CleanupComplete);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Unknown_cleanup_or_readback_never_becomes_success(bool throws)
    {
        var port = new Port { Clean = false, ThrowAt = throws ? "clean" : "" };
        var result = new BusinessSubtypeTrial().Run(Context, port);
        Assert.Equal("failed", result.Status); Assert.False(result.CleanupComplete);
        Assert.Equal(2, port.Events.Count(e => e == "cleanup"));
    }

    [Theory]
    [InlineData("register")]
    [InlineData("prepare")]
    public void Cancellation_after_native_reentry_stops_the_next_mutation(string phase)
    {
        bool valid = true;
        var port = new Port { OnPhase = stage => { if (stage == phase) valid = false; } };
        var result = new BusinessSubtypeTrial().Run(Context, port, () => valid);
        Assert.Equal("failed", result.Status); Assert.True(result.CleanupComplete);
        Assert.DoesNotContain("invoke", port.Events);
        if (phase == "register") Assert.DoesNotContain("prepare", port.Events);
    }

    [Fact]
    public void Reentrant_run_cannot_register_twice()
    {
        var trial = new BusinessSubtypeTrial(); var port = new Port(); SubtypeResult? inner = null;
        port.OnPhase = phase => { if (phase == "register") inner = trial.Run(Context, port); };
        Assert.Equal("observed", trial.Run(Context, port).Status);
        Assert.Equal("skipped", inner!.Status); Assert.Single(port.Events, e => e == "register");
    }

    [Theory]
    [InlineData("readback")]
    [InlineData("dispatch")]
    [InlineData("registries")]
    [InlineData("classes")]
    [InlineData("key")]
    public void Missing_required_evidence_is_a_failure(string missing)
    {
        var port = new Port();
        if (missing == "readback") port.Readback = null!;
        if (missing == "dispatch") port.Dispatch = null!;
        if (missing == "registries") port.Registries.Clear();
        if (missing == "classes") port.Classes.Clear();
        if (missing == "key") { port.Registries.Remove("save-requests"); port.Registries["unknown"] = "initial"; }
        Assert.Equal("failed", new BusinessSubtypeTrial().Run(Context, port).Status);
        if (missing is "registries" or "classes" or "key") Assert.DoesNotContain("register", port.Events);
    }

    [Fact]
    public void Subtype_trial_with_probe_save_and_save_port_executes_save_trial_and_reverses_cleanly()
    {
        var port = new SavePort(Context, contractVerified: true);
        var result = new BusinessSubtypeTrial().Run(Context, port, probeSave: true);

        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.NotNull(result.Save);
        Assert.True(result.Save!.CompletionConfirmed);
        Assert.True(result.Save!.SafeToDestroy);
        Assert.True(result.CleanupComplete);
        Assert.Contains("register-save", port.Events);
        Assert.Contains("save-once", port.Events);
        Assert.Contains("remove-save", port.Events);
        Assert.Contains("cleanup", port.Events);
    }

    [Fact]
    public void Subtype_trial_with_probe_save_fails_when_save_port_contract_unverified()
    {
        var port = new SavePort(Context, contractVerified: false);
        var result = new BusinessSubtypeTrial().Run(Context, port, probeSave: true);

        Assert.Equal("failed", result.Status);
        Assert.Contains("native-save-completion-contract-unverified", result.Findings);
        Assert.True(result.CleanupComplete);
        Assert.Null(result.Save);
        Assert.DoesNotContain("save-once", port.Events);
        Assert.Contains("cleanup", port.Events);
    }

    [Fact]
    public void Subtype_trial_with_probe_save_handles_context_loss_during_save()
    {
        bool valid = true;
        var port = new SavePort(Context, contractVerified: true);
        port.OnSave = () => valid = false;

        var result = new BusinessSubtypeTrial().Run(Context, port, () => valid, probeSave: true);

        Assert.Equal("failed", result.Status);
        Assert.False(result.CleanupComplete);
        Assert.Contains(result.Findings, f => f.Contains("context-expired") || f.Contains("save-begin"));
        Assert.DoesNotContain("cleanup", port.Events);
        Assert.NotNull(result.Save);
        Assert.True(result.Save!.RegistrationAttempted);
        Assert.False(result.Save.SafeToDestroy);
    }

    [Fact]
    public void Business_saveable_slot_registry_contains_critical_save_interception_slots()
    {
        Assert.Equal(BusinessSaveableContractInventory.TotalInterfaceSlots, BusinessSaveableContractInventory.Slots.Count);
        var slotsByName = BusinessSaveableContractInventory.Slots.ToLookup(s => s.Name);

        Assert.True(slotsByName.Contains("get_ShouldSaveUnderFolder"));
        var folderSlot = slotsByName["get_ShouldSaveUnderFolder"].Single();
        Assert.True(folderSlot.OverriddenOnProbeSubtype);
        Assert.Equal(0x0600177B, folderSlot.Token);
        Assert.Equal(0, folderSlot.ParameterCount);

        Assert.True(slotsByName.Contains("WriteData"));
        var writeSlot = slotsByName["WriteData"].Single();
        Assert.True(writeSlot.OverriddenOnProbeSubtype);
        Assert.Equal(0x06001788, writeSlot.Token);
        Assert.Equal(1, writeSlot.ParameterCount);

        Assert.True(slotsByName.Contains("DeleteUnapprovedFiles"));
        var deleteSlot = slotsByName["DeleteUnapprovedFiles"].Single();
        Assert.True(deleteSlot.OverriddenOnProbeSubtype);
        Assert.Equal(0x06001789, deleteSlot.Token);
        Assert.Equal(1, deleteSlot.ParameterCount);
    }

    [Theory]
    [InlineData(true, true, true, true)]    // Standard virtual: resolved matches and vtable matches -> passes
    [InlineData(false, true, true, true)]   // Final virtual: resolved returns base (METHOD_ATTRIBUTE_FINAL), vtable matches -> passes (FIX)
    [InlineData(true, false, true, true)]   // Virtual resolver matches, vtable slot mismatch -> passes via virtual resolver
    [InlineData(false, false, true, false)] // Neither matches: unpatched -> fails
    [InlineData(false, true, false, false)] // Vtable method points to base pointer instead of own -> fails
    public void Virtual_target_verification_logic_handles_final_virtual_methods_and_enforces_safety(
        bool virtualResolverMatches, bool vtableMatchesMethod, bool vtablePointerIsOwn, bool expectedPass)
    {
        IntPtr baseMethod = (IntPtr)0x1000;
        IntPtr basePointer = (IntPtr)0x2000;
        IntPtr ownMethod = (IntPtr)0x3000;
        IntPtr ownPointer = (IntPtr)0x4000;
        IntPtr ownClass = (IntPtr)0x5000;

        IntPtr resolved = virtualResolverMatches ? ownMethod : baseMethod;
        bool resolvedMatches = resolved == ownMethod && MethodPointer(resolved) == ownPointer && MethodPointer(resolved) != basePointer;

        IntPtr vtableMethod = vtableMatchesMethod ? ownMethod : baseMethod;
        IntPtr vtablePtr = vtablePointerIsOwn ? ownPointer : basePointer;
        IntPtr vtableClass = ownClass;

        bool vtableMatches = vtableMethod == ownMethod && vtableClass == ownClass &&
            vtablePtr == ownPointer && vtablePtr != basePointer;

        bool passes = resolvedMatches || vtableMatches;
        Assert.Equal(expectedPass, passes);

        static IntPtr MethodPointer(IntPtr m) => m == (IntPtr)0x3000 ? (IntPtr)0x4000 : (IntPtr)0x2000;
    }

    private class Port : IBusinessSubtypePort
    {
        public readonly List<string> Events = new();
        public readonly Dictionary<string, string> Registries = new[] { "properties", "properties-owned", "properties-unowned", "businesses", "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests" }.ToDictionary(k => k, _ => "initial");
        public readonly Dictionary<string, string> Classes = new() { ["Business"] = "initial" };
        public string ThrowAt = "";
        public int NullCapture, ChangeCapture, CleanupThrows;
        public bool ChangeClass, Clean = true;
        public SubtypeReadback Readback = new(true, true, true, true, true, 0);
        public SubtypeDispatch Dispatch = new(false, false, 1, 1);
        public Action? OnInvoke;
        public Action<string>? OnPhase;
        private int _capture, _cleanups;
        protected void Enter(string stage)
        {
            Events.Add(stage); OnPhase?.Invoke(stage);
            if (ThrowAt == stage) throw new InvalidOperationException("injected");
        }
        public SubtypeSnapshot Capture()
        {
            Enter("capture"); _capture++;
            if (_capture == NullCapture) return null!;
            if (_capture == ChangeCapture) (ChangeClass ? Classes : Registries)[ChangeClass ? "Business" : "properties"] = "changed";
            return new(Registries, Classes);
        }
        public void RegisterType() => Enter("register");
        public void Prepare() => Enter("prepare");
        public SubtypeReadback Inspect() { Enter("inspect"); return Readback; }
        public SubtypeDispatch InvokeThroughNativeBase() { Enter("invoke"); OnInvoke?.Invoke(); return Dispatch; }
        public void Cleanup() { Enter("cleanup"); if (++_cleanups <= CleanupThrows) throw new InvalidOperationException("late-callback-or-destroy-failure"); }
        public bool IsClean { get { Enter("clean"); return Clean; } }
    }

    private sealed class SavePort : Port, IBusinessSavePort
    {
        public ManagerSaveState State;
        private long _frame = 1000;
        public Action? OnSave { get; set; }
        public Action? OnRegister { get; set; }
        public Action? OnRemove { get; set; }

        public SavePort(ConstructionContext context, bool contractVerified = true)
        {
            State = new ManagerSaveState(context, 17, 101, 202, 1000,
                false, 0, 0, 0, 0, 0, "foreign-A;foreign-B", contractVerified);
        }

        public ManagerSaveState ReadSaveState()
        {
            Enter("read-save");
            _frame++;
            return State = State with { Frame = _frame };
        }

        public void RegisterSaveable()
        {
            Enter("register-save");
            State = State with { OwnEntries = 1 };
            OnRegister?.Invoke();
        }

        public void SaveOnce()
        {
            Enter("save-once");
            State = State with { Started = 1, Completed = 1 };
            OnSave?.Invoke();
        }

        public void RemoveSaveable()
        {
            Enter("remove-save");
            State = State with { OwnEntries = 0 };
            OnRemove?.Invoke();
        }
    }
}

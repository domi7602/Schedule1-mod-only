using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

// Independent offline attacks against the policy model. No game objects, native
// methods, directory enumeration or save-file operations are used by these tests.
public sealed class BusinessManagerAuditTests
{
    private static string SlotPath(string profile = "profile-a", string slot = "SaveGame_2") =>
        Path.Combine(Path.GetTempPath(), "TownHallBusinessManagerAudit", profile, slot);

    private static ConstructionContext World(string organisation = "TownHallTest", string slot = "SaveGame_2") =>
        new(true, true, true, true, SlotPath(slot: slot), SlotPath(slot: slot), organisation);

    private static ManagerSaveState Initial() =>
        new(World(), 4, 101, 201, 100, false, 0, 0, 0, 0, 0, "foreign-a", true);

    [Theory]
    [InlineData("Dominik", "SaveGame_1")]
    [InlineData("TownHallTest", "SaveGame_1")]
    [InlineData("OtherWorld", "SaveGame_2")]
    [InlineData("townhalltest", "SaveGame_2")]
    [InlineData("TownHallTest", "savegame_2")]
    [InlineData("TownHallTest", "SaveGame_2/../SaveGame_1")]
    [InlineData("", "SaveGame_2")]
    [InlineData("TownHallTest", "")]
    [InlineData(null, "SaveGame_2")]
    [InlineData("TownHallTest", null)]
    public void Explicit_save_arguments_cannot_redirect_even_an_otherwise_allowed_world(string? organisation, string? slot)
    {
        Assert.NotNull(ConstructionGate.CheckSave(World(), organisation!, slot!));
        Assert.NotNull(ConstructionGate.CheckSave(World(organisation ?? "", slot ?? ""), organisation!, slot!));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Legacy_save_is_blocked_before_every_port_member_and_context_callback(bool explicitTarget)
    {
        var port = new UntouchableSubtypePort();
        int contextCalls = 0;
        var trial = new BusinessSubtypeTrial();
        var result = explicitTarget
            ? trial.Run(World(), port, () => { contextCalls++; return true; }, organisation: "TownHallTest", slot: "SaveGame_2", probeSave: true)
            : trial.Run(World(), port, () => { contextCalls++; return true; }, probeSave: true);

        Assert.Equal("blocked", result.Status);
        Assert.Contains("native-save-completion-contract-unverified", result.Findings);
        Assert.False(result.AttemptConsumed);
        Assert.False(result.RegistrationAttempted);
        Assert.Null(result.Before);
        Assert.Null(result.Manager);
        Assert.Equal(0, port.Accesses);
        Assert.Equal(0, contextCalls);
    }

    [Fact]
    public void Unverified_contract_blocks_offline_model_before_any_mutation()
    {
        var port = new AuditPort { State = Initial() with { CompletionContractVerified = false } };
        var trial = new BusinessManagerSaveTrial();
        var result = trial.Begin(port, 0);

        Assert.Equal("blocked", result.Status);
        Assert.False(result.AttemptConsumed);
        Assert.False(result.RegistrationAttempted);
        Assert.False(result.SaveAttempted);
        Assert.Equal(1, port.Reads);
        Assert.Equal((0, 0, 0), (port.Registers, port.Saves, port.Removes));
    }

    [Theory]
    [InlineData("manager-zero")]
    [InlineData("list-zero")]
    [InlineData("generation-zero")]
    [InlineData("negative-frame")]
    [InlineData("busy")]
    [InlineData("queued")]
    [InlineData("already-owned")]
    [InlineData("callback-active")]
    [InlineData("stale-start")]
    [InlineData("stale-complete")]
    [InlineData("foreign-unreadable")]
    [InlineData("other-slot")]
    [InlineData("different-paths")]
    public void Invalid_preflight_never_mutates_a_port(string cause)
    {
        var state = Initial();
        state = cause switch
        {
            "manager-zero" => state with { Manager = 0 },
            "list-zero" => state with { Saveables = 0 },
            "generation-zero" => state with { Generation = 0 },
            "negative-frame" => state with { Frame = -1 },
            "busy" => state with { IsSaving = true },
            "queued" => state with { PendingRequests = 1 },
            "already-owned" => state with { OwnEntries = 1 },
            "callback-active" => state with { ActiveCallbacks = 1 },
            "stale-start" => state with { Started = 1 },
            "stale-complete" => state with { Completed = 1 },
            "foreign-unreadable" => state with { ForeignSaveables = "" },
            "other-slot" => state with { Context = World(slot: "SaveGame_1") },
            "different-paths" => state with { Context = World() with { LoadedSavePath = SlotPath("profile-b") } },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        var port = new AuditPort { State = state };
        var result = new BusinessManagerSaveTrial().Begin(port, 0);

        Assert.Contains(result.Status, new[] { "skipped", "blocked" });
        Assert.False(result.AttemptConsumed);
        if (cause is "busy" or "already-owned") Assert.False(result.SafeToDestroy);
        Assert.Equal((0, 0, 0), (port.Registers, port.Saves, port.Removes));
    }

    [Fact]
    public void Failed_initial_read_never_certifies_safe_destruction_of_unknown_references()
    {
        var port = new AuditPort { OnRead = (_, _) => throw new IOException("initial-reference-state-unreadable") };
        var trial = new BusinessManagerSaveTrial();

        var result = trial.Begin(port, 0);

        Assert.Equal("failed", result.Status);
        Assert.False(result.SafeToDestroy);
        Assert.False(result.AttemptConsumed);
        Assert.Null(result.Before);
        Assert.Equal((0, 0, 0), (port.Registers, port.Saves, port.Removes));
    }

    [Theory]
    [InlineData("busy")]
    [InlineData("already-owned")]
    [InlineData("active-callback")]
    [InlineData("queued-reference")]
    public void Unknown_preexisting_references_never_certify_safe_destruction(string cause)
    {
        var state = Initial();
        state = cause switch
        {
            "busy" => state with { IsSaving = true },
            "already-owned" => state with { OwnEntries = 1 },
            "active-callback" => state with { ActiveCallbacks = 1 },
            "queued-reference" => state with { PendingRequests = 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        var port = new AuditPort { State = state };

        var result = new BusinessManagerSaveTrial().Begin(port, 0);

        Assert.Equal("blocked", result.Status);
        Assert.False(result.SafeToDestroy);
        Assert.False(result.AttemptConsumed);
        Assert.Equal((0, 0, 0), (port.Registers, port.Saves, port.Removes));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void Invalid_initial_clock_never_mutates(double now)
    {
        var port = new AuditPort();
        var result = new BusinessManagerSaveTrial().Begin(port, now);
        Assert.Equal("blocked", result.Status);
        Assert.Equal((0, 0, 0), (port.Registers, port.Saves, port.Removes));
    }

    [Theory]
    [InlineData("manager")]
    [InlineData("list")]
    [InlineData("generation")]
    [InlineData("other-profile")]
    [InlineData("loaded-path")]
    [InlineData("active-path")]
    [InlineData("organisation")]
    [InlineData("foreign-list")]
    [InlineData("contract-revoked")]
    [InlineData("frame-backwards")]
    [InlineData("negative-requests")]
    [InlineData("negative-callbacks")]
    [InlineData("missing-own-entry")]
    [InlineData("duplicate-own-entry")]
    [InlineData("duplicate-start")]
    [InlineData("duplicate-complete")]
    [InlineData("complete-without-start")]
    public void Changed_or_malformed_observation_quarantines_without_removal_or_retry(string cause)
    {
        var port = new AuditPort();
        var trial = Started(port);
        Complete(port, 101);
        port.State = cause switch
        {
            "manager" => port.State with { Manager = 102 },
            "list" => port.State with { Saveables = 202 },
            "generation" => port.State with { Generation = 5 },
            "other-profile" => port.State with { Context = World() with { ActiveSavePath = SlotPath("profile-b"), LoadedSavePath = SlotPath("profile-b") } },
            "loaded-path" => port.State with { Context = World() with { LoadedSavePath = SlotPath("profile-b") } },
            "active-path" => port.State with { Context = World() with { ActiveSavePath = SlotPath("profile-b") } },
            "organisation" => port.State with { Context = World("OtherWorld") },
            "foreign-list" => port.State with { ForeignSaveables = "foreign-b" },
            "contract-revoked" => port.State with { CompletionContractVerified = false },
            "frame-backwards" => port.State with { Frame = 99 },
            "negative-requests" => port.State with { PendingRequests = -1 },
            "negative-callbacks" => port.State with { ActiveCallbacks = -1 },
            "missing-own-entry" => port.State with { OwnEntries = 0 },
            "duplicate-own-entry" => port.State with { OwnEntries = 2 },
            "duplicate-start" => port.State with { Started = 2 },
            "duplicate-complete" => port.State with { Started = 2, Completed = 2 },
            "complete-without-start" => port.State with { Started = 0 },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };

        Retained(trial.Tick(1), port);
        NoRetry(trial, port);
    }

    [Fact]
    public void Equivalent_normalized_absolute_paths_preserve_the_frozen_identity()
    {
        var port = new AuditPort();
        var trial = Started(port);
        string root = Path.GetDirectoryName(SlotPath())!;
        string equivalent = Path.Combine(root, "..", "profile-a", ".", "SaveGame_2") + Path.DirectorySeparatorChar;
        port.State = port.State with { Context = World() with { ActiveSavePath = equivalent, LoadedSavePath = equivalent }, Frame = 101 };

        Assert.Equal("pending", trial.Tick(1).Status);
        Complete(port, 102);
        trial.Tick(2);
        port.State = port.State with { Frame = 103 };
        Assert.Equal("observed", trial.Tick(3).Status);
        Assert.Equal((1, 1, 1), (port.Registers, port.Saves, port.Removes));
    }

    [Theory]
    [InlineData("idle-without-events")]
    [InlineData("start-without-complete")]
    [InlineData("still-saving")]
    [InlineData("queue-not-empty")]
    [InlineData("callback-active")]
    public void Incomplete_observation_retains_registration(string cause)
    {
        var port = new AuditPort();
        var trial = Started(port);
        Complete(port, 101);
        port.State = cause switch
        {
            "idle-without-events" => port.State with { Started = 0, Completed = 0 },
            "start-without-complete" => port.State with { Completed = 0 },
            "still-saving" => port.State with { IsSaving = true },
            "queue-not-empty" => port.State with { PendingRequests = 1 },
            "callback-active" => port.State with { ActiveCallbacks = 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        var result = trial.Tick(1);

        Assert.Equal("pending", result.Status);
        Assert.False(result.SafeToDestroy);
        Assert.False(result.CompletionConfirmed);
        Assert.Equal(0, port.Removes);
        Assert.Equal(1, port.State.OwnEntries);
    }

    [Fact]
    public void Synchronous_completion_cannot_remove_in_the_dispatch_frame()
    {
        var port = new AuditPort { OnSave = p => Complete(p, 100) };
        var trial = Started(port);
        Assert.Equal("pending", trial.Tick(1).Status);
        Assert.Equal(0, port.Removes);
        port.State = port.State with { Frame = 101 };

        Assert.Equal("observed", trial.Tick(2).Status);
        Assert.Equal((1, 1, 1), (port.Registers, port.Saves, port.Removes));
    }

    [Fact]
    public void First_completion_observation_requires_a_later_frame_before_removal()
    {
        var port = new AuditPort();
        var trial = Started(port);
        // The completion edge was not observed in an earlier frame. Merely being
        // later than SaveOnce's dispatch does not establish a post-completion frame.
        Complete(port, 101);
        var first = trial.Tick(1);

        Assert.Equal(0, port.Removes);
        Assert.Equal("pending", first.Status);
        Assert.False(first.SafeToDestroy);
        Assert.Equal("pending", trial.Tick(2).Status);
        port.State = port.State with { Frame = 102 };
        Assert.Equal("observed", trial.Tick(3).Status);
        Assert.Equal((1, 1, 1), (port.Registers, port.Saves, port.Removes));
    }

    [Theory]
    [InlineData("start")]
    [InlineData("complete")]
    [InlineData("frame")]
    public void Monotonic_observations_cannot_roll_back(string counter)
    {
        var port = new AuditPort();
        var trial = Started(port);
        port.State = counter == "complete"
            ? port.State with { Started = 1, Completed = 1 }
            : port.State with { Frame = 105, Started = 1, IsSaving = true };
        Assert.Equal("pending", trial.Tick(1).Status);
        port.State = counter switch
        {
            "start" => port.State with { Frame = 106, Started = 0 },
            "complete" => port.State with { Frame = 101, Completed = 0 },
            "frame" => port.State with { Frame = 104 },
            _ => throw new ArgumentOutOfRangeException(nameof(counter))
        };

        Retained(trial.Tick(2), port);
        NoRetry(trial, port);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void Revocation_during_readbacks_never_authorizes_save_or_safe_destruction(int revokeOnRead)
    {
        bool current = true;
        var port = new AuditPort
        {
            OnRead = (_, number) => { if (number == revokeOnRead) current = false; }
        };
        var trial = new BusinessManagerSaveTrial();
        var result = trial.Begin(port, 0, () => current);
        if (revokeOnRead > 2)
        {
            Complete(port, 100);
            if (revokeOnRead == 4)
            {
                // Read 3 observes completion, but same-frame cleanup is forbidden.
                Assert.Equal("pending", trial.Tick(0.5).Status);
                // The next read is itself revoked; no removal may take place.
            }
            port.State = port.State with { Frame = 101 };
            result = trial.Tick(1);
        }

        Retained(result, port, revokeOnRead == 2 ? 0 : 1);
        Assert.Contains(result.Findings, value => value.Contains("context-expired", StringComparison.Ordinal));
        NoRetry(trial, port, revokeOnRead == 2 ? 0 : 1);
    }

    [Fact]
    public void Revocation_in_post_removal_snapshot_prevents_safe_to_destroy_and_retry()
    {
        bool current = true;
        bool removed = false;
        var port = new AuditPort
        {
            OnRemove = _ => removed = true,
            OnRead = (_, _) => { if (removed) current = false; }
        };
        var trial = new BusinessManagerSaveTrial();
        trial.Begin(port, 0, () => current);
        Complete(port, 100);
        trial.Tick(0.5);
        port.State = port.State with { Frame = 101 };

        var result = trial.Tick(1);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.SafeToDestroy);
        Assert.Equal(1, port.Removes);
        NoRetry(trial, port, removals: 1);
    }

    [Fact]
    public void Revocation_at_the_last_context_gate_prevents_safe_to_destroy()
    {
        bool removed = false;
        int checksAfterRemoval = 0;
        var port = new AuditPort { OnRemove = _ => removed = true };
        var trial = new BusinessManagerSaveTrial();
        trial.Begin(port, 0, () => !removed || ++checksAfterRemoval < 2);
        Complete(port, 100);
        trial.Tick(0.5);
        port.State = port.State with { Frame = 101 };

        var result = trial.Tick(1);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.SafeToDestroy);
        Assert.Equal(1, port.Removes);
        NoRetry(trial, port, removals: 1);
    }

    [Theory]
    [InlineData("register")]
    [InlineData("save")]
    [InlineData("remove")]
    [InlineData("pending-read")]
    [InlineData("removal-read")]
    public void Exceptions_after_possible_effects_never_trigger_speculative_cleanup_or_retry(string phase)
    {
        bool removed = false;
        var port = new AuditPort
        {
            OnRegister = _ => { if (phase == "register") throw new IOException("injected-register-after-effect"); },
            OnSave = _ => { if (phase == "save") throw new IOException("injected-save-after-effect"); },
            OnRemove = _ => { removed = true; if (phase == "remove") throw new IOException("injected-remove-after-effect"); },
            OnRead = (_, read) =>
            {
                if ((phase == "pending-read" && read == 3) || (phase == "removal-read" && removed))
                    throw new IOException("injected-unreadable-after-effect");
            }
        };
        var trial = new BusinessManagerSaveTrial();
        var result = trial.Begin(port, 0);
        if (phase is not "register" and not "save")
        {
            Complete(port, 100);
            result = trial.Tick(0.5);
            if (phase != "pending-read")
            {
                port.State = port.State with { Frame = 101 };
                result = trial.Tick(1);
            }
        }

        Assert.Equal("quarantined", result.Status);
        Assert.False(result.SafeToDestroy);
        int saves = phase == "register" ? 0 : 1;
        int removals = phase is "remove" or "removal-read" ? 1 : 0;
        Assert.Equal((1, saves, removals), (port.Registers, port.Saves, port.Removes));
        NoRetry(trial, port, saves, removals);
    }

    [Theory]
    [InlineData("register")]
    [InlineData("save")]
    [InlineData("read")]
    [InlineData("remove")]
    public void Reentry_never_touches_another_port_or_repeats_save_or_removal(string phase)
    {
        var trial = new BusinessManagerSaveTrial();
        var intruder = new AuditPort();
        int reentries = 0;
        void Reenter()
        {
            reentries++;
            Assert.False(trial.Begin(intruder, 0).SafeToDestroy);
            Assert.False(trial.Tick(0).SafeToDestroy);
        }
        var port = new AuditPort
        {
            OnRegister = _ => { if (phase == "register") Reenter(); },
            OnSave = _ => { if (phase == "save") Reenter(); },
            OnRead = (_, read) => { if (phase == "read" && read >= 3) Reenter(); },
            OnRemove = _ => { if (phase == "remove") Reenter(); }
        };
        trial.Begin(port, 0);
        Complete(port, 100);
        trial.Tick(0.5);
        port.State = port.State with { Frame = 101 };
        var result = trial.Tick(1);

        Assert.True(reentries > 0);
        Assert.Equal("observed", result.Status);
        Assert.True(result.SafeToDestroy);
        Assert.Equal(0, intruder.Reads);
        Assert.Equal((1, 1, 1), (port.Registers, port.Saves, port.Removes));
        NoRetry(trial, port, removals: 1);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(BusinessManagerSaveTrial.TimeoutSeconds)]
    public void Invalid_or_expired_clock_retains_the_unknown_operation(double now)
    {
        var port = new AuditPort();
        var trial = Started(port);
        Retained(trial.Tick(now), port);
        NoRetry(trial, port);
    }

    private static BusinessManagerSaveTrial Started(AuditPort port)
    {
        var trial = new BusinessManagerSaveTrial();
        var result = trial.Begin(port, 0);
        Assert.Equal("pending", result.Status);
        Assert.True(result.SaveAttempted);
        Assert.True(result.SaveReturned);
        Assert.False(result.SafeToDestroy);
        Assert.Equal((1, 1, 0), (port.Registers, port.Saves, port.Removes));
        return trial;
    }

    private static void Complete(AuditPort port, long frame) =>
        port.State = port.State with { Frame = frame, Started = 1, Completed = 1, IsSaving = false, PendingRequests = 0, ActiveCallbacks = 0 };

    private static void Retained(ManagerSaveResult result, AuditPort port, int saves = 1)
    {
        Assert.Equal("quarantined", result.Status);
        Assert.True(result.AttemptConsumed);
        Assert.True(result.RegistrationAttempted);
        Assert.False(result.SafeToDestroy);
        Assert.Equal((1, saves, 0), (port.Registers, port.Saves, port.Removes));
    }

    private static void NoRetry(BusinessManagerSaveTrial trial, AuditPort port, int saves = 1, int removals = 0)
    {
        var replacement = new AuditPort();
        trial.Begin(replacement, 2);
        trial.Tick(1000);
        trial.Tick(1001);
        Assert.Equal(0, replacement.Reads);
        Assert.Equal((1, saves, removals), (port.Registers, port.Saves, port.Removes));
    }

    private sealed class AuditPort : IBusinessSavePort
    {
        public ManagerSaveState State = Initial();
        public int Reads, Registers, Saves, Removes;
        public Action<AuditPort, int>? OnRead;
        public Action<AuditPort>? OnRegister, OnSave, OnRemove;

        public ManagerSaveState ReadSaveState()
        {
            int read = ++Reads;
            var snapshot = State;
            OnRead?.Invoke(this, read);
            return snapshot;
        }

        public void RegisterSaveable()
        {
            Registers++;
            State = State with { OwnEntries = 1 };
            OnRegister?.Invoke(this);
        }

        public void SaveOnce()
        {
            Saves++;
            OnSave?.Invoke(this);
        }

        public void RemoveSaveable()
        {
            Removes++;
            State = State with { OwnEntries = 0 };
            OnRemove?.Invoke(this);
        }
    }

    private sealed class UntouchableSubtypePort : IBusinessSubtypePort, IBusinessManagerPort
    {
        public int Accesses;
        private Exception Touched() { Accesses++; return new InvalidOperationException("legacy-save-touched-port"); }
        public SubtypeSnapshot Capture() => throw Touched();
        public void RegisterType() => throw Touched();
        public void Prepare() => throw Touched();
        public SubtypeReadback Inspect() => throw Touched();
        public SubtypeDispatch InvokeThroughNativeBase() => throw Touched();
        public ManagerRegistration RegisterOnce() => throw Touched();
        public void Cleanup() => throw Touched();
        public bool IsClean => throw Touched();
    }
}

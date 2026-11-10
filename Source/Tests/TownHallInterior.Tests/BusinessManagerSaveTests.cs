using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

// Offline model only. The fake's explicit verification flag is not native completion evidence.
public sealed class BusinessManagerSaveTests
{
    private const double BeganAt = 100;
    private const long InitialFrame = 1000;
    private const string TestSlot = "SaveGame_2";

    // These paths are values, not fixtures on disk. No save directory is opened or created.
    private static string SlotPath(string slot = TestSlot, string parent = "profile-a") =>
        Path.Combine(Path.GetTempPath(), "TownHallManagerSaveTests", parent, slot);

    private static ConstructionContext World => new(true, true, true, true,
        SlotPath(), SlotPath(), "TownHallTest");

    private static ManagerSaveState Ready => new(World, 17, 101, 202, InitialFrame,
        false, 0, 0, 0, 0, 0, "foreign-A;foreign-B", CompletionContractVerified: true);

    [Fact]
    public void Unverified_completion_contract_blocks_before_every_mutation()
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port { State = Ready with { CompletionContractVerified = false } };

        var result = trial.Begin(port, BeganAt);

        Assert.Equal("blocked", result.Status);
        Assert.Contains("native-save-completion-contract-unverified", result.Findings);
        AssertNotStarted(result);
        AssertNoMutations(port);
        Assert.Equal(new[] { "read" }, port.Events);
        port.State = CompletedState(port);
        Assert.Equal("blocked", trial.Tick(BeganAt + BusinessManagerSaveTrial.TimeoutSeconds).Status);
        Assert.Equal(new[] { "read" }, port.Events);
    }

    [Theory]
    [InlineData("Dominik-slot-1")]
    [InlineData("Dominik-slot-2")]
    [InlineData("other-world")]
    [InlineData("TownHallTest-slot-1")]
    [InlineData("organisation-case")]
    [InlineData("missing-context")]
    [InlineData("load")]
    [InlineData("lobby")]
    [InlineData("hall")]
    [InlineData("dependencies")]
    [InlineData("different-absolute-paths")]
    [InlineData("missing-active")]
    [InlineData("missing-loaded")]
    [InlineData("relative-active")]
    [InlineData("relative-loaded")]
    public void Foreign_or_unconfirmed_world_cannot_start_a_save(string cause)
    {
        var context = cause switch
        {
            "Dominik-slot-1" => World with { OrganisationName = "Dominik", ActiveSavePath = SlotPath("SaveGame_1"), LoadedSavePath = SlotPath("SaveGame_1") },
            "Dominik-slot-2" => World with { OrganisationName = "Dominik" },
            "other-world" => World with { OrganisationName = "OtherOrganisation", ActiveSavePath = SlotPath("SaveGame_9"), LoadedSavePath = SlotPath("SaveGame_9") },
            "TownHallTest-slot-1" => World with { ActiveSavePath = SlotPath("SaveGame_1"), LoadedSavePath = SlotPath("SaveGame_1") },
            "organisation-case" => World with { OrganisationName = "townhalltest" },
            "missing-context" => null!,
            "load" => World with { LoadComplete = false },
            "lobby" => World with { Singleplayer = false },
            "hall" => World with { HallReady = false },
            "dependencies" => World with { DependenciesMatch = false },
            "different-absolute-paths" => World with { LoadedSavePath = SlotPath(parent: "profile-b") },
            "missing-active" => World with { ActiveSavePath = null },
            "missing-loaded" => World with { LoadedSavePath = null },
            "relative-active" => World with { ActiveSavePath = TestSlot },
            "relative-loaded" => World with { LoadedSavePath = TestSlot },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        var trial = new BusinessManagerSaveTrial();
        var port = new Port { State = Ready with { Context = context } };

        var result = trial.Begin(port, BeganAt);

        Assert.Equal("skipped", result.Status);
        AssertNotStarted(result);
        AssertNoMutations(port);
        Assert.Equal(new[] { "read" }, port.Events);
        trial.Tick(BeganAt + 1);
        Assert.Equal(new[] { "read" }, port.Events);
    }

    [Theory]
    [InlineData("manager")]
    [InlineData("list")]
    [InlineData("generation-zero")]
    [InlineData("generation-negative")]
    [InlineData("frame")]
    [InlineData("saving")]
    [InlineData("queue")]
    [InlineData("negative-queue")]
    [InlineData("own-entry")]
    [InlineData("negative-own-entry")]
    [InlineData("callback")]
    [InlineData("negative-callback")]
    [InlineData("started")]
    [InlineData("completed")]
    [InlineData("foreign-empty")]
    [InlineData("foreign-missing")]
    public void Invalid_or_busy_preflight_cannot_register(string cause)
    {
        var state = cause switch
        {
            "manager" => Ready with { Manager = 0 },
            "list" => Ready with { Saveables = 0 },
            "generation-zero" => Ready with { Generation = 0 },
            "generation-negative" => Ready with { Generation = -1 },
            "frame" => Ready with { Frame = -1 },
            "saving" => Ready with { IsSaving = true },
            "queue" => Ready with { PendingRequests = 1 },
            "negative-queue" => Ready with { PendingRequests = -1 },
            "own-entry" => Ready with { OwnEntries = 1 },
            "negative-own-entry" => Ready with { OwnEntries = -1 },
            "callback" => Ready with { ActiveCallbacks = 1 },
            "negative-callback" => Ready with { ActiveCallbacks = -1 },
            "started" => Ready with { Started = 1 },
            "completed" => Ready with { Completed = 1 },
            "foreign-empty" => Ready with { ForeignSaveables = "" },
            "foreign-missing" => Ready with { ForeignSaveables = null! },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        var port = new Port { State = state };
        var result = new BusinessManagerSaveTrial().Begin(port, BeganAt);

        Assert.Equal("blocked", result.Status);
        Assert.Contains("manager-save-preflight-invalid", result.Findings);
        AssertNotStarted(result);
        AssertNoMutations(port);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(-0.1)]
    public void Invalid_begin_clock_cannot_register(double now)
    {
        var port = new Port();
        var result = new BusinessManagerSaveTrial().Begin(port, now);
        Assert.Equal("blocked", result.Status);
        AssertNotStarted(result);
        AssertNoMutations(port);
    }

    [Fact]
    public void One_save_is_retained_until_verified_completion_and_removed_exactly_once()
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        var original = port.State;
        var pending = trial.Begin(port, BeganAt);
        AssertPending(pending, port);
        Assert.Equal(new[] { "read", "register", "read", "save" }, port.Events);

        port.State = port.State with { Frame = InitialFrame + 1, Started = 1, IsSaving = true };
        AssertPending(trial.Tick(BeganAt + 1), port);
        port.State = CompletedState(port);
        var completion = ObserveCompletion(trial, port, BeganAt + 2);
        var sameFrame = trial.Tick(BeganAt + 2.5);
        AssertPending(sameFrame, port);
        Assert.Equal(completion.CompletionObservedFrame, sameFrame.CompletionObservedFrame);
        port.State = CompletedState(port);
        var result = trial.Tick(BeganAt + 3);

        AssertObserved(result, port);
        Assert.Equal(new[] { "read", "register", "read", "save", "read", "read", "read", "read", "remove", "read" }, port.Events);
        Assert.Equal(InitialFrame, result.DispatchFrame);
        Assert.Equal(InitialFrame + 2, result.CompletionObservedFrame);
        Assert.Equal(completion.CompletionObservedFrame, result.CompletionObservedFrame);
        Assert.Equal(original, result.Before);
        Assert.Equal(0, pending.Before!.OwnEntries);
        Assert.Equal(1, pending.Last!.OwnEntries);
        Assert.Equal(0, pending.Last.Completed);
        Assert.Empty(pending.Findings);
        AssertTerminalWithoutMoreCalls(trial, port);
        var otherPort = new Port();
        Assert.Equal("observed", trial.Begin(otherPort, BeganAt + 4).Status);
        Assert.Empty(otherPort.Events);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void Idle_and_save_return_without_both_events_remain_pending(int started, int completed)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = port.State with { Frame = InitialFrame + 1, Started = started, Completed = completed };
        AssertPending(trial.Tick(BeganAt + 1), port);
        AssertPending(trial.Tick(BeganAt + 2), port);
    }

    [Theory]
    [InlineData("same-frame")]
    [InlineData("saving")]
    [InlineData("queue")]
    [InlineData("callback")]
    public void Completion_also_requires_later_frame_idle_queue_and_no_active_callbacks(string cause)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        AssertPending(trial.Begin(port, BeganAt), port);
        var complete = CompletedState(port);
        port.State = cause switch
        {
            "same-frame" => complete with { Frame = InitialFrame },
            "saving" => complete with { IsSaving = true },
            "queue" => complete with { PendingRequests = 1 },
            "callback" => complete with { ActiveCallbacks = 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };

        ObserveCompletion(trial, port, BeganAt + 1);
        port.State = CompletedState(port);
        AssertObserved(trial.Tick(BeganAt + 2), port);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Fact]
    public void Later_frame_is_relative_to_save_dispatch_not_the_earlier_preflight()
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port { OnRegister = p => p.State = p.State with { Frame = InitialFrame + 5 } };
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = CompletedState(port) with { Frame = InitialFrame + 5 };
        var completion = ObserveCompletion(trial, port, BeganAt + 1);
        Assert.Equal(InitialFrame + 5, completion.DispatchFrame);
        Assert.Equal(InitialFrame + 5, completion.CompletionObservedFrame);
        port.State = port.State with { Frame = InitialFrame + 6 };
        AssertObserved(trial.Tick(BeganAt + 2), port);
    }

    [Theory]
    [InlineData("saving")]
    [InlineData("queue")]
    [InlineData("missing-entry")]
    [InlineData("duplicate-entry")]
    [InlineData("callback")]
    [InlineData("started")]
    [InlineData("completed")]
    public void Invalid_registration_readback_prevents_save_and_is_not_repaired(string cause)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port
        {
            OnRegister = p => p.State = cause switch
            {
                "saving" => p.State with { IsSaving = true },
                "queue" => p.State with { PendingRequests = 1 },
                "missing-entry" => p.State with { OwnEntries = 0 },
                "duplicate-entry" => p.State with { OwnEntries = 2 },
                "callback" => p.State with { ActiveCallbacks = 1 },
                "started" => p.State with { Started = 1 },
                "completed" => p.State with { Completed = 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(cause))
            }
        };
        var result = trial.Begin(port, BeganAt);
        AssertQuarantined(result);
        Assert.False(result.SaveAttempted);
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(0, port.SaveCalls);
        Assert.Equal(0, port.RemoveCalls);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(2, 0)]
    [InlineData(1, -1)]
    [InlineData(1, 2)]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    [InlineData(2, 2)]
    public void Missing_start_duplicate_or_out_of_order_events_quarantine_without_removal(int started, int completed)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = CompletedState(port) with { Started = started, Completed = completed };
        AssertQuarantined(trial.Tick(BeganAt + 1));
        Assert.Equal(0, port.RemoveCalls);
        Assert.Equal(1, port.State.OwnEntries);
        port.State = CompletedState(port);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData("started")]
    [InlineData("completed")]
    [InlineData("frame")]
    public void Previously_observed_events_and_frame_cannot_regress(string cause)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = port.State with
        {
            Frame = InitialFrame + 5,
            Started = 1,
            Completed = cause == "completed" ? 1 : 0,
            PendingRequests = 1
        };
        AssertPending(trial.Tick(BeganAt + 1), port);
        port.State = cause switch
        {
            "started" => port.State with { Frame = InitialFrame + 6, Started = 0 },
            "completed" => port.State with { Frame = InitialFrame + 6, Completed = 0 },
            "frame" => CompletedState(port) with { Frame = InitialFrame + 4 },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        AssertQuarantined(trial.Tick(BeganAt + 2));
        Assert.Equal(0, port.RemoveCalls);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Missing_completion_times_out_keeps_registration_and_never_releases_after_late_completion(int started)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = port.State with { Frame = InitialFrame + 1, Started = started };
        AssertPending(trial.Tick(BeganAt + BusinessManagerSaveTrial.TimeoutSeconds - 0.001), port);

        var result = trial.Tick(BeganAt + BusinessManagerSaveTrial.TimeoutSeconds);
        AssertQuarantined(result);
        Assert.Contains(result.Findings, finding => finding.Contains("TimeoutException:manager-save-completion-unconfirmed"));
        Assert.False(result.CompletionConfirmed);
        Assert.Equal(1, port.State.OwnEntries);
        Assert.Equal(0, port.RemoveCalls);
        port.State = CompletedState(port);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Deadline_precedes_release_even_when_completion_is_available_on_the_first_late_tick(int delay)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = CompletedState(port);
        var result = trial.Tick(BeganAt + BusinessManagerSaveTrial.TimeoutSeconds + delay);
        AssertQuarantined(result);
        Assert.False(result.CompletionConfirmed);
        Assert.Equal(1, port.State.OwnEntries);
        Assert.Equal(0, port.RemoveCalls);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(BeganAt - 1)]
    [InlineData(BeganAt + 1)]
    public void Nonfinite_or_reversed_clock_quarantines_the_pending_save(double now)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        AssertPending(trial.Begin(port, BeganAt), port);
        AssertPending(trial.Tick(BeganAt + 2), port);
        AssertQuarantined(trial.Tick(now));
        Assert.Equal(1, port.State.OwnEntries);
        Assert.Equal(0, port.RemoveCalls);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    public static IEnumerable<object[]> IdentityChanges =>
        from phase in new[] { "registration", "pending", "removal" }
        from field in new[] { "manager", "list", "generation", "parent-path", "foreign", "contract", "organisation", "slot", "load", "lobby", "hall", "dependencies", "relative-path", "frame" }
        select new object[] { phase, field };

    [Theory]
    [MemberData(nameof(IdentityChanges))]
    public void Frozen_manager_list_generation_target_and_foreign_identity_cannot_change(string phase, string field)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        ManagerSaveResult result;
        if (phase == "registration")
        {
            port.OnRegister = p => p.State = ChangedIdentity(p.State, field);
            result = trial.Begin(port, BeganAt);
        }
        else
        {
            AssertPending(trial.Begin(port, BeganAt), port);
            port.State = CompletedState(port);
            if (phase == "pending")
            {
                port.State = ChangedIdentity(port.State, field);
                result = trial.Tick(BeganAt + 1);
            }
            else
            {
                port.OnRemove = p => p.State = ChangedIdentity(p.State, field);
                ObserveCompletion(trial, port, BeganAt + 1);
                port.State = CompletedState(port);
                result = trial.Tick(BeganAt + 2);
            }
        }

        AssertQuarantined(result);
        Assert.Contains(result.Findings, finding => finding.Contains("manager-save-identity-changed"));
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(phase == "registration" ? 0 : 1, port.SaveCalls);
        Assert.Equal(phase == "removal" ? 1 : 0, port.RemoveCalls);
        Assert.Equal(phase == "removal", result.CompletionConfirmed);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Fact]
    public void Canonically_equal_absolute_targets_keep_the_same_frozen_identity()
    {
        var context = World with
        {
            ActiveSavePath = SlotPath() + Path.DirectorySeparatorChar,
            LoadedSavePath = Path.Combine(SlotPath(), "..", TestSlot)
        };
        var trial = new BusinessManagerSaveTrial();
        var port = new Port { State = Ready with { Context = context } };
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = CompletedState(port) with { Context = World };
        ObserveCompletion(trial, port, BeganAt + 1);
        port.State = CompletedState(port);
        AssertObserved(trial.Tick(BeganAt + 2), port);
    }

    [Theory]
    [InlineData("saving")]
    [InlineData("queue")]
    [InlineData("own-entry")]
    [InlineData("callback")]
    [InlineData("started-missing")]
    [InlineData("completed-missing")]
    [InlineData("completed-duplicate")]
    [InlineData("frame-regression")]
    public void Removal_requires_clean_stable_readback_and_never_claims_safe_destruction_on_uncertainty(string cause)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port
        {
            OnRemove = p => p.State = cause switch
            {
                "saving" => p.State with { IsSaving = true },
                "queue" => p.State with { PendingRequests = 1 },
                "own-entry" => p.State with { OwnEntries = 1 },
                "callback" => p.State with { ActiveCallbacks = 1 },
                "started-missing" => p.State with { Started = 0 },
                "completed-missing" => p.State with { Completed = 0 },
                "completed-duplicate" => p.State with { Completed = 2 },
                "frame-regression" => p.State with { Frame = InitialFrame },
                _ => throw new ArgumentOutOfRangeException(nameof(cause))
            }
        };
        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = CompletedState(port);
        ObserveCompletion(trial, port, BeganAt + 1);
        port.State = CompletedState(port);
        var result = trial.Tick(BeganAt + 2);
        AssertQuarantined(result);
        Assert.True(result.CompletionConfirmed);
        Assert.Equal(1, port.RemoveCalls);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData("register", false)]
    [InlineData("register", true)]
    [InlineData("save", false)]
    [InlineData("save", true)]
    [InlineData("remove", false)]
    [InlineData("remove", true)]
    public void Mutation_exceptions_before_or_after_effect_never_retry_or_speculatively_remove(string operation, bool afterEffect)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port { ThrowOperation = operation, ThrowAfterEffect = afterEffect };
        var result = trial.Begin(port, BeganAt);
        if (operation == "remove")
        {
            AssertPending(result, port);
            port.State = CompletedState(port);
            ObserveCompletion(trial, port, BeganAt + 1);
            port.State = CompletedState(port);
            result = trial.Tick(BeganAt + 2);
        }

        AssertQuarantined(result);
        Assert.Contains(result.Findings, finding => finding.Contains("IOException:injected-" + operation));
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(operation == "register" ? 0 : 1, port.SaveCalls);
        Assert.Equal(operation == "remove" ? 1 : 0, port.RemoveCalls);
        Assert.Equal(operation != "register", result.SaveAttempted);
        Assert.Equal(operation == "remove", result.SaveReturned);
        Assert.Equal(operation == "remove", result.CompletionConfirmed);
        port.ThrowOperation = null;
        port.State = CompletedState(port);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData("preflight", false)]
    [InlineData("preflight", true)]
    [InlineData("registration", false)]
    [InlineData("registration", true)]
    [InlineData("pending", false)]
    [InlineData("pending", true)]
    [InlineData("removal", false)]
    [InlineData("removal", true)]
    public void Unreadable_state_is_terminal_and_cannot_be_retried_into_a_new_save(string phase, bool returnsNull)
    {
        int failedRead = phase switch { "preflight" => 1, "registration" => 2, "pending" => 3, "removal" => 5, _ => throw new ArgumentOutOfRangeException(nameof(phase)) };
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        if (returnsNull) port.Observe = p => p.ReadCalls == failedRead ? null! : p.State;
        else port.ThrowRead = failedRead;

        var result = trial.Begin(port, BeganAt);
        if (phase is "pending" or "removal")
        {
            AssertPending(result, port);
            if (phase == "removal")
            {
                port.State = CompletedState(port);
                ObserveCompletion(trial, port, BeganAt + 1);
                port.State = CompletedState(port);
                result = trial.Tick(BeganAt + 2);
            }
            else result = trial.Tick(BeganAt + 1);
        }

        if (phase == "preflight")
        {
            Assert.Equal("failed", result.Status);
            AssertNoMutations(port);
            Assert.False(result.SafeToDestroy);
            Assert.False(result.AttemptConsumed);
        }
        else AssertQuarantined(result);
        Assert.NotEmpty(result.Findings);
        Assert.Equal(phase == "preflight" ? 0 : 1, port.RegisterCalls);
        Assert.Equal(phase is "preflight" or "registration" ? 0 : 1, port.SaveCalls);
        Assert.Equal(phase == "removal" ? 1 : 0, port.RemoveCalls);

        port.ThrowRead = 0;
        port.Observe = null;
        port.State = phase == "preflight" ? Ready : CompletedState(port);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Theory]
    [InlineData("preflight-read")]
    [InlineData("register")]
    [InlineData("registered-read")]
    [InlineData("save")]
    [InlineData("poll-read")]
    [InlineData("remove")]
    [InlineData("removed-read")]
    public void Context_revocation_during_each_port_reentry_is_observed_before_return_or_next_mutation(string phase)
    {
        bool current = true;
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        OnPhase(port, phase, () => current = false);
        var result = trial.Begin(port, BeganAt, () => current);
        if (phase is "poll-read" or "remove" or "removed-read")
        {
            AssertPending(result, port);
            port.State = CompletedState(port);
            if (phase is "remove" or "removed-read")
            {
                ObserveCompletion(trial, port, BeganAt + 1);
                port.State = CompletedState(port);
                result = trial.Tick(BeganAt + 2);
            }
            else result = trial.Tick(BeganAt + 1);
        }

        if (phase == "preflight-read")
        {
            Assert.Equal("skipped", result.Status);
            AssertNotStarted(result);
            AssertNoMutations(port);
        }
        else
        {
            AssertQuarantined(result);
            Assert.Equal(1, port.RegisterCalls);
            Assert.Equal(phase is "register" or "registered-read" ? 0 : 1, port.SaveCalls);
            Assert.Equal(phase is "remove" or "removed-read" ? 1 : 0, port.RemoveCalls);
            AssertTerminalWithoutMoreCalls(trial, port);
        }
        Assert.Contains(result.Findings, finding => finding.Contains("context-expired"));
    }

    public static IEnumerable<object[]> Reentries =>
        from phase in new[] { "preflight-read", "register", "registered-read", "save", "poll-read", "remove", "removed-read" }
        from action in new[] { "begin", "tick" }
        select new object[] { phase, action };

    [Theory]
    [MemberData(nameof(Reentries))]
    public void Reentrant_begin_or_tick_cannot_duplicate_save_or_advance_outer_operation(string phase, string action)
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        var foreignPort = new Port();
        ManagerSaveResult? nested = null;
        OnPhase(port, phase, () => nested = action == "begin"
            ? trial.Begin(foreignPort, BeganAt + 0.5)
            : trial.Tick(BeganAt + 0.5));

        AssertPending(trial.Begin(port, BeganAt), port);
        port.State = CompletedState(port);
        ObserveCompletion(trial, port, BeganAt + 1);
        port.State = CompletedState(port);
        AssertObserved(trial.Tick(BeganAt + 2), port);
        Assert.NotNull(nested);
        Assert.False(nested.SafeToDestroy);
        Assert.Empty(foreignPort.Events);
        Assert.Equal(new[] { "read", "register", "read", "save", "read", "read", "remove", "read" }, port.Events);
        AssertTerminalWithoutMoreCalls(trial, port);
    }

    [Fact]
    public void Result_records_and_findings_are_detached_from_future_fake_observations_and_callers()
    {
        var trial = new BusinessManagerSaveTrial();
        var port = new Port();
        var pending = trial.Begin(port, BeganAt);
        AssertPending(pending, port);
        port.State = CompletedState(port) with { Manager = 999 };
        var failed = trial.Tick(BeganAt + 1);
        AssertQuarantined(failed);
        failed.Findings[0] = "caller-overwrite";
        Assert.DoesNotContain("caller-overwrite", trial.Result.Findings);
        Assert.Equal(101L, pending.Before!.Manager);
        Assert.Equal(101L, pending.Last!.Manager);
        Assert.Equal(0, pending.Before.OwnEntries);
        Assert.Equal(1, pending.Last.OwnEntries);
        Assert.Equal(0, pending.Last.Completed);
        Assert.Empty(pending.Findings);
        Assert.Equal("pending", pending.Status);
        Assert.Equal(InitialFrame, pending.DispatchFrame);
        Assert.Null(pending.CompletionObservedFrame);
    }

    private static ManagerSaveState CompletedState(Port port) => port.State with
    {
        Frame = Math.Max(InitialFrame, port.State.Frame) + 1,
        IsSaving = false,
        PendingRequests = 0,
        ActiveCallbacks = 0,
        Started = 1,
        Completed = 1
    };

    private static ManagerSaveState ChangedIdentity(ManagerSaveState state, string field) => field switch
    {
        "manager" => state with { Manager = 999 },
        "list" => state with { Saveables = 999 },
        "generation" => state with { Generation = state.Generation + 1 },
        "parent-path" => state with { Context = state.Context with { ActiveSavePath = SlotPath(parent: "profile-b"), LoadedSavePath = SlotPath(parent: "profile-b") } },
        "foreign" => state with { ForeignSaveables = "foreign-A;different-same-count-entry" },
        "contract" => state with { CompletionContractVerified = false },
        "organisation" => state with { Context = state.Context with { OrganisationName = "Dominik" } },
        "slot" => state with { Context = state.Context with { ActiveSavePath = SlotPath("SaveGame_1"), LoadedSavePath = SlotPath("SaveGame_1") } },
        "load" => state with { Context = state.Context with { LoadComplete = false } },
        "lobby" => state with { Context = state.Context with { Singleplayer = false } },
        "hall" => state with { Context = state.Context with { HallReady = false } },
        "dependencies" => state with { Context = state.Context with { DependenciesMatch = false } },
        "relative-path" => state with { Context = state.Context with { ActiveSavePath = TestSlot, LoadedSavePath = TestSlot } },
        "frame" => state with { Frame = InitialFrame - 1 },
        _ => throw new ArgumentOutOfRangeException(nameof(field))
    };

    private static void OnPhase(Port port, string phase, Action callback)
    {
        int read = phase switch { "preflight-read" => 1, "registered-read" => 2, "poll-read" => 3, "removed-read" => 5, _ => 0 };
        if (read != 0)
        {
            port.Observe = p =>
            {
                var snapshot = p.State;
                if (p.ReadCalls == read) callback();
                return snapshot;
            };
        }
        else if (phase == "register") port.OnRegister = _ => callback();
        else if (phase == "save") port.OnSave = _ => callback();
        else if (phase == "remove") port.OnRemove = _ => callback();
        else throw new ArgumentOutOfRangeException(nameof(phase));
    }

    private static void AssertNoMutations(Port port)
    {
        Assert.Equal(0, port.RegisterCalls);
        Assert.Equal(0, port.SaveCalls);
        Assert.Equal(0, port.RemoveCalls);
    }

    private static void AssertNotStarted(ManagerSaveResult result)
    {
        Assert.False(result.AttemptConsumed);
        Assert.False(result.RegistrationAttempted);
        Assert.False(result.SaveAttempted);
        Assert.False(result.SaveReturned);
        Assert.False(result.CompletionConfirmed);
        Assert.False(result.SafeToDestroy);
    }

    private static void AssertPending(ManagerSaveResult result, Port port)
    {
        Assert.Equal("pending", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.AttemptConsumed);
        Assert.True(result.RegistrationAttempted);
        Assert.True(result.SaveAttempted);
        Assert.True(result.SaveReturned);
        Assert.False(result.CompletionConfirmed);
        Assert.False(result.SafeToDestroy);
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(1, port.SaveCalls);
        Assert.Equal(0, port.RemoveCalls);
        Assert.Equal(1, port.State.OwnEntries);
    }

    private static ManagerSaveResult ObserveCompletion(BusinessManagerSaveTrial trial, Port port, double now)
    {
        long frame = port.State.Frame;
        Assert.Equal(1, port.State.Started);
        Assert.Equal(1, port.State.Completed);
        var result = trial.Tick(now);
        AssertPending(result, port);
        Assert.Equal(frame, result.CompletionObservedFrame);
        return result;
    }

    private static void AssertObserved(ManagerSaveResult result, Port port)
    {
        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.AttemptConsumed);
        Assert.True(result.RegistrationAttempted);
        Assert.True(result.SaveAttempted);
        Assert.True(result.SaveReturned);
        Assert.True(result.CompletionConfirmed);
        Assert.True(result.SafeToDestroy);
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(1, port.SaveCalls);
        Assert.Equal(1, port.RemoveCalls);
        Assert.Equal(0, port.State.OwnEntries);
        Assert.Equal(0, result.Last!.OwnEntries);
        Assert.NotNull(result.DispatchFrame);
        Assert.NotNull(result.CompletionObservedFrame);
        Assert.True(result.Last.Frame > result.DispatchFrame.Value);
        Assert.True(result.Last.Frame > result.CompletionObservedFrame.Value);
    }

    private static void AssertQuarantined(ManagerSaveResult result)
    {
        Assert.Equal("quarantined", result.Status);
        Assert.NotEmpty(result.Findings);
        Assert.True(result.AttemptConsumed);
        Assert.True(result.RegistrationAttempted);
        Assert.False(result.SafeToDestroy);
    }

    private static void AssertTerminalWithoutMoreCalls(BusinessManagerSaveTrial trial, Port port)
    {
        var result = trial.Result;
        var events = port.Events.ToArray();
        var tick = trial.Tick(BeganAt + BusinessManagerSaveTrial.TimeoutSeconds + 10);
        var begin = trial.Begin(port, BeganAt + BusinessManagerSaveTrial.TimeoutSeconds + 11);
        Assert.Equal(events, port.Events);
        Assert.Equal(result.Status, tick.Status);
        Assert.Equal(result.Status, begin.Status);
        Assert.Equal(result.SafeToDestroy, trial.Result.SafeToDestroy);
    }

    private sealed class Port : IBusinessSavePort
    {
        public ManagerSaveState State = Ready;
        public readonly List<string> Events = new();
        public int ReadCalls, RegisterCalls, SaveCalls, RemoveCalls;
        public int ThrowRead;
        public string? ThrowOperation;
        public bool ThrowAfterEffect;
        public Func<Port, ManagerSaveState>? Observe;
        public Action<Port>? OnRegister, OnSave, OnRemove;

        public ManagerSaveState ReadSaveState()
        {
            Events.Add("read");
            ReadCalls++;
            if (ReadCalls == ThrowRead) throw new IOException("injected-read");
            return Observe == null ? State : Observe(this);
        }

        public void RegisterSaveable()
        {
            Events.Add("register");
            RegisterCalls++;
            ThrowIfRequested("register", false);
            State = State with { OwnEntries = 1 };
            OnRegister?.Invoke(this);
            ThrowIfRequested("register", true);
        }

        public void SaveOnce()
        {
            Events.Add("save");
            SaveCalls++;
            ThrowIfRequested("save", false);
            OnSave?.Invoke(this);
            ThrowIfRequested("save", true);
        }

        public void RemoveSaveable()
        {
            Events.Add("remove");
            RemoveCalls++;
            ThrowIfRequested("remove", false);
            State = State with { OwnEntries = 0 };
            OnRemove?.Invoke(this);
            ThrowIfRequested("remove", true);
        }

        private void ThrowIfRequested(string operation, bool afterEffect)
        {
            if (ThrowOperation == operation && ThrowAfterEffect == afterEffect)
                throw new IOException("injected-" + operation);
        }
    }
}

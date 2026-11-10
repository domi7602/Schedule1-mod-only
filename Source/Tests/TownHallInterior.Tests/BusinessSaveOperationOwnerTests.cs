using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

// Synthetic messages only: no callback body, game object, save file or native port is exercised.
public sealed class BusinessSaveOperationOwnerTests
{
    private const double BeganAt = 100;
    private static readonly BusinessSaveModelEffect[] Effects = Enum.GetValues<BusinessSaveModelEffect>();

    // Path values, not fixtures. Nothing creates or opens these directories.
    private static string SlotPath(string slot = "SaveGame_2", string profile = "profile-a") =>
        Path.Combine(Path.GetTempPath(), "TownHallSaveOwnerModelTests", profile, slot);

    private static BusinessSaveModelBinding Candidate => new(
        new ConstructionContext(true, true, true, true, SlotPath(), SlotPath(), "TownHallTest"),
        17, "load-101", "save-202", "saveables-303", "base-404", "completed-505", "requests-606", "participant-707");

    private sealed class Scenario
    {
        private long _sequence;
        public BusinessSaveProcessModel Process { get; } = new();
        public BusinessSaveOperationOwner Owner { get; }
        public BusinessSaveModelBinding Binding { get; }
        public Scenario()
        {
            Owner = Process.CreateOwner();
            Assert.True(Owner.TryBind(Candidate, BeganAt));
            Binding = Owner.Snapshot.Binding!;
        }
        public BusinessSaveModelStamp Stamp(double now = BeganAt) =>
            new(Owner.Snapshot.Key!.Value, Binding, ++_sequence, now);
        public void Complete(BusinessSaveModelEffect effect)
        {
            Assert.True(Owner.TryRecordIntent(effect, Stamp()));
            Assert.True(Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, Stamp()));
            Assert.True(Owner.RecordReadback(effect, true, Stamp()));
        }
        public void Before(BusinessSaveModelEffect effect)
        {
            foreach (var previous in Effects.TakeWhile(value => value != effect)) Complete(previous);
        }
    }

    private static void AssertNoNativeAuthority(BusinessSaveModelSnapshot snapshot)
    {
        Assert.True(snapshot.SimulationOnly);
        Assert.False(snapshot.NativeSaveWriteAllowed);
        Assert.False(snapshot.SafeToUnregister);
        Assert.False(snapshot.SafeToDestroy);
        Assert.Equal("unknown", snapshot.ReferenceProof);
        Assert.Equal("unknown", snapshot.NativeWriteProof);
        Assert.Equal("unknown", snapshot.ModDataProof);
    }

    private static void AssertNoIntents(BusinessSaveModelSnapshot snapshot) =>
        Assert.All(snapshot.Effects.Values, effect => Assert.Equal(new(false, false, false, false), effect));

    private static void OnOtherThread(Action action)
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            try { action(); }
            catch (Exception error) { failure = error; }
        });
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(5)), "Synthetic worker did not finish.");
        Assert.Null(failure);
    }

    [Fact]
    public void Empty_model_has_no_authority_or_publicly_resettable_owner()
    {
        var process = new BusinessSaveProcessModel();
        var snapshot = process.CreateOwner().Snapshot;
        Assert.NotEqual(Guid.Empty, process.ProcessId);
        Assert.Equal(process.ProcessId, snapshot.ProcessId);
        Assert.Equal("empty", snapshot.Status);
        Assert.False(snapshot.SlotConsumed);
        Assert.False(snapshot.Revoked);
        Assert.Null(snapshot.Key);
        Assert.Null(snapshot.Binding);
        Assert.Null(snapshot.LastTime);
        Assert.Equal(0, snapshot.LastSequence);
        Assert.Equal(6, snapshot.Effects.Count);
        Assert.Empty(snapshot.Diagnostics);
        Assert.Empty(typeof(BusinessSaveOperationOwner).GetConstructors());
        AssertNoIntents(snapshot);
        AssertNoNativeAuthority(snapshot);
    }

    [Theory]
    [InlineData("binding")]
    [InlineData("context")]
    [InlineData("load")]
    [InlineData("singleplayer")]
    [InlineData("hall")]
    [InlineData("dependencies")]
    [InlineData("world")]
    [InlineData("world-case")]
    [InlineData("slot")]
    [InlineData("relative")]
    [InlineData("missing-path")]
    [InlineData("different-path")]
    [InlineData("long-path")]
    [InlineData("epoch-zero")]
    [InlineData("epoch-negative")]
    [InlineData("load-manager")]
    [InlineData("save-manager")]
    [InlineData("saveables")]
    [InlineData("base-saveables")]
    [InlineData("completed")]
    [InlineData("requests")]
    [InlineData("participant")]
    [InlineData("label-control")]
    [InlineData("label-length")]
    public void Invalid_candidate_consumes_no_slot_and_valid_candidate_can_still_bind(string cause)
    {
        var binding = Candidate;
        var context = binding.Context;
        BusinessSaveModelBinding? invalid = cause switch
        {
            "binding" => null,
            "context" => binding with { Context = null! },
            "load" => binding with { Context = context with { LoadComplete = false } },
            "singleplayer" => binding with { Context = context with { Singleplayer = false } },
            "hall" => binding with { Context = context with { HallReady = false } },
            "dependencies" => binding with { Context = context with { DependenciesMatch = false } },
            "world" => binding with { Context = context with { OrganisationName = "Dominik" } },
            "world-case" => binding with { Context = context with { OrganisationName = "townhalltest" } },
            "slot" => binding with { Context = context with { ActiveSavePath = SlotPath("SaveGame_1"), LoadedSavePath = SlotPath("SaveGame_1") } },
            "relative" => binding with { Context = context with { ActiveSavePath = "SaveGame_2" } },
            "missing-path" => binding with { Context = context with { LoadedSavePath = null } },
            "different-path" => binding with { Context = context with { LoadedSavePath = SlotPath(profile: "profile-b") } },
            "long-path" => binding with { Context = context with { ActiveSavePath = new string('x', 32768) } },
            "epoch-zero" => binding with { Epoch = 0 },
            "epoch-negative" => binding with { Epoch = -1 },
            "load-manager" => binding with { LoadManager = "" },
            "save-manager" => binding with { SaveManager = null! },
            "saveables" => binding with { Saveables = " " },
            "base-saveables" => binding with { BaseSaveables = "" },
            "completed" => binding with { Completed = "" },
            "requests" => binding with { Requests = "" },
            "participant" => binding with { Participant = "" },
            "label-control" => binding with { Participant = "participant\n707" },
            "label-length" => binding with { LoadManager = new string('a', 161) },
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        var owner = new BusinessSaveProcessModel().CreateOwner();
        Assert.False(owner.TryBind(invalid, BeganAt));
        Assert.False(owner.Snapshot.SlotConsumed);
        Assert.Null(owner.Snapshot.Binding);
        AssertNoIntents(owner.Snapshot);
        Assert.True(owner.TryBind(binding, BeganAt));
        AssertNoNativeAuthority(owner.Snapshot);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Invalid_bind_time_never_reserves_a_slot(double time)
    {
        var owner = new BusinessSaveProcessModel().CreateOwner();
        Assert.False(owner.TryBind(Candidate, time));
        Assert.False(owner.Snapshot.SlotConsumed);
        AssertNoIntents(owner.Snapshot);
        AssertNoNativeAuthority(owner.Snapshot);
    }

    [Fact]
    public void Binding_is_canonical_frozen_data_and_does_not_open_a_save()
    {
        var candidate = Candidate;
        var alternate = Path.Combine(Path.GetDirectoryName(SlotPath())!, "alias", "..", "SaveGame_2") + Path.DirectorySeparatorChar;
        candidate = candidate with { Context = candidate.Context with { ActiveSavePath = alternate } };
        var process = new BusinessSaveProcessModel();
        var owner = process.CreateOwner();
        Assert.True(owner.TryBind(candidate, BeganAt));
        var frozen = owner.Snapshot;
        Assert.Equal(new BusinessSaveModelKey(process.ProcessId, 1), frozen.Key);
        Assert.Equal(Path.GetFullPath(SlotPath()), frozen.Binding!.Context.ActiveSavePath);
        Assert.Equal(Path.GetFullPath(SlotPath()), frozen.Binding.Context.LoadedSavePath);
        candidate = candidate with { Participant = "replacement", Context = candidate.Context with { OrganisationName = "Dominik" } };
        Assert.NotEqual(candidate, owner.Snapshot.Binding);
        Assert.Equal("participant-707", owner.Snapshot.Binding!.Participant);
        var equivalent = frozen.Binding with { Context = frozen.Binding.Context with { ActiveSavePath = alternate } };
        Assert.True(owner.ObserveContext(equivalent, new(frozen.Key!.Value, frozen.Binding, 1, BeganAt)));
        Assert.False(owner.Snapshot.Revoked);
    }

    [Fact]
    public void All_facades_share_operation_and_tombstone_but_separate_models_are_separate_simulations()
    {
        var scenario = new Scenario();
        var second = scenario.Process.CreateOwner();
        Assert.Equal(scenario.Owner.Snapshot.Key, second.Snapshot.Key);
        second.Revoke("leaving");
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Process.CreateOwner().TryBind(Candidate, BeganAt));
        Assert.Equal(scenario.Binding, scenario.Owner.Snapshot.Binding);
        Assert.True(scenario.Owner.Snapshot.SlotConsumed);
        var other = new BusinessSaveProcessModel();
        Assert.NotEqual(scenario.Process.ProcessId, other.ProcessId);
        Assert.True(other.CreateOwner().TryBind(Candidate, BeganAt));
    }

    [Fact]
    public void Early_revocation_also_survives_owner_recreation_without_consuming_a_binding_slot()
    {
        var process = new BusinessSaveProcessModel();
        process.CreateOwner().Revoke("stop-before-bind");
        var replacement = process.CreateOwner();
        Assert.False(replacement.TryBind(Candidate, BeganAt));
        Assert.Equal("quarantined", replacement.Snapshot.Status);
        Assert.False(replacement.Snapshot.SlotConsumed);
        Assert.Equal("stop-before-bind", replacement.Snapshot.FirstRevocationReason);
        AssertNoIntents(replacement.Snapshot);
    }

    [Theory]
    [InlineData("epoch")]
    [InlineData("load-manager")]
    [InlineData("save-manager")]
    [InlineData("saveables")]
    [InlineData("base-saveables")]
    [InlineData("completed")]
    [InlineData("requests")]
    [InlineData("participant")]
    [InlineData("world-lost")]
    [InlineData("context-lost")]
    public void Same_slot_reload_or_replacement_cannot_retarget_frozen_operation(string changed)
    {
        var scenario = new Scenario();
        var old = scenario.Binding;
        BusinessSaveModelBinding? current = changed switch
        {
            "epoch" => old with { Epoch = old.Epoch + 1 },
            "load-manager" => old with { LoadManager = "load-new" },
            "save-manager" => old with { SaveManager = "save-new" },
            "saveables" => old with { Saveables = "saveables-new" },
            "base-saveables" => old with { BaseSaveables = "base-new" },
            "completed" => old with { Completed = "completed-new" },
            "requests" => old with { Requests = "requests-new" },
            "participant" => old with { Participant = "participant-new" },
            "world-lost" => old with { Context = old.Context with { LoadComplete = false } },
            "context-lost" => null,
            _ => throw new ArgumentOutOfRangeException(nameof(changed))
        };
        Assert.False(scenario.Owner.ObserveContext(current, scenario.Stamp()));
        Assert.Equal(old, scenario.Owner.Snapshot.Binding);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.TryBind(current, BeganAt));
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, scenario.Stamp()));
        AssertNoIntents(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("process")]
    [InlineData("operation")]
    [InlineData("binding")]
    [InlineData("null")]
    public void Message_requires_the_exact_bound_operation(string mismatch)
    {
        var scenario = new Scenario();
        var stamp = scenario.Stamp();
        BusinessSaveModelStamp invalid = mismatch switch
        {
            "process" => stamp with { Key = new(Guid.NewGuid(), 1) },
            "operation" => stamp with { Key = stamp.Key with { Operation = 2 } },
            "binding" => stamp with { Binding = stamp.Binding with { Epoch = 18 } },
            "null" => null!,
            _ => throw new ArgumentOutOfRangeException(nameof(mismatch))
        };
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, invalid));
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.Equal(scenario.Binding, scenario.Owner.Snapshot.Binding);
        AssertNoIntents(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Successful_synthetic_ledger_does_not_grant_any_native_permission_or_proof()
    {
        var scenario = new Scenario();
        foreach (var effect in Effects) scenario.Complete(effect);
        var snapshot = scenario.Owner.Snapshot;
        Assert.Equal("bound-data-only", snapshot.Status);
        Assert.False(snapshot.Revoked);
        Assert.True(snapshot.SlotConsumed);
        Assert.All(snapshot.Effects.Values, effect => Assert.Equal(new(true, true, true, false), effect));
        AssertNoNativeAuthority(snapshot);
        foreach (var name in new[] { "SimulationOnly", "NativeSaveWriteAllowed", "SafeToUnregister", "SafeToDestroy", "ReferenceProof", "NativeWriteProof", "ModDataProof" })
            Assert.Null(typeof(BusinessSaveModelSnapshot).GetProperty(name)!.SetMethod);
        Assert.False(scenario.Process.CreateOwner().TryBind(Candidate, BeganAt));
    }

    [Theory]
    [InlineData(BusinessSaveModelEffect.Construction)]
    [InlineData(BusinessSaveModelEffect.Registration)]
    [InlineData(BusinessSaveModelEffect.Save)]
    [InlineData(BusinessSaveModelEffect.Removal)]
    [InlineData(BusinessSaveModelEffect.Destroy)]
    public void Each_effect_requires_all_predecessors_instead_of_just_idle_or_a_completion_hint(BusinessSaveModelEffect effect)
    {
        var scenario = new Scenario();
        Assert.True(scenario.Owner.ObserveTelemetry("Complete; idle; queue empty; later frame", scenario.Stamp()));
        Assert.False(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        AssertNoIntents(scenario.Owner.Snapshot);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Return_without_confirmed_readback_does_not_admit_the_next_effect(bool returned)
    {
        var scenario = new Scenario();
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, scenario.Stamp()));
        if (returned) Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.TypeRegistration, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration].Intent);
        Assert.False(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Construction].Intent);
    }

    [Theory]
    [InlineData(BusinessSaveModelEffect.TypeRegistration)]
    [InlineData(BusinessSaveModelEffect.Construction)]
    [InlineData(BusinessSaveModelEffect.Registration)]
    [InlineData(BusinessSaveModelEffect.Save)]
    [InlineData(BusinessSaveModelEffect.Removal)]
    [InlineData(BusinessSaveModelEffect.Destroy)]
    public void Failed_effect_preserves_possible_partial_action_and_prevents_blind_repeat(BusinessSaveModelEffect effect)
    {
        var scenario = new Scenario();
        scenario.Before(effect);
        Assert.True(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Failed, scenario.Stamp()));
        Assert.Equal(new(true, false, false, true), scenario.Owner.Snapshot.Effects[effect]);
        Assert.False(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        Assert.Equal(new(true, false, false, true), scenario.Owner.Snapshot.Effects[effect]);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("outcome-without-intent")]
    [InlineData("readback-without-intent")]
    [InlineData("readback-before-return")]
    [InlineData("duplicate-intent")]
    [InlineData("duplicate-return")]
    [InlineData("duplicate-readback")]
    [InlineData("undefined-intent")]
    [InlineData("undefined-effect-outcome")]
    [InlineData("undefined-outcome")]
    [InlineData("undefined-readback")]
    public void Invalid_or_duplicate_effect_messages_revoke_without_retry(string cause)
    {
        var scenario = new Scenario();
        var owner = scenario.Owner;
        var effect = BusinessSaveModelEffect.TypeRegistration;
        if (cause is "readback-before-return" or "duplicate-intent" or "duplicate-return" or "duplicate-readback" or "undefined-outcome")
            Assert.True(owner.TryRecordIntent(effect, scenario.Stamp()));
        if (cause is "duplicate-return" or "duplicate-readback")
            Assert.True(owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        if (cause == "duplicate-readback") Assert.True(owner.RecordReadback(effect, true, scenario.Stamp()));
        bool accepted = cause switch
        {
            "outcome-without-intent" or "duplicate-return" => owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, scenario.Stamp()),
            "readback-without-intent" or "readback-before-return" or "duplicate-readback" => owner.RecordReadback(effect, true, scenario.Stamp()),
            "duplicate-intent" => owner.TryRecordIntent(effect, scenario.Stamp()),
            "undefined-intent" => owner.TryRecordIntent((BusinessSaveModelEffect)999, scenario.Stamp()),
            "undefined-effect-outcome" => owner.RecordOutcome((BusinessSaveModelEffect)999, BusinessSaveModelOutcome.Returned, scenario.Stamp()),
            "undefined-outcome" => owner.RecordOutcome(effect, (BusinessSaveModelOutcome)999, scenario.Stamp()),
            "undefined-readback" => owner.RecordReadback((BusinessSaveModelEffect)999, true, scenario.Stamp()),
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        Assert.False(accepted);
        Assert.True(owner.Snapshot.Revoked);
        Assert.Equal(6, owner.Snapshot.Effects.Count);
        AssertNoNativeAuthority(owner.Snapshot);
    }

    [Fact]
    public void Revocation_before_intent_suppresses_it_but_revocation_after_admission_keeps_the_intent()
    {
        var first = new Scenario();
        OnOtherThread(() => first.Owner.Revoke("worker-first"));
        Assert.False(first.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, first.Stamp()));
        AssertNoIntents(first.Owner.Snapshot);

        var second = new Scenario();
        Assert.True(second.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, second.Stamp()));
        OnOtherThread(() => second.Owner.Revoke("worker-after-admission"));
        Assert.True(second.Owner.RecordOutcome(BusinessSaveModelEffect.TypeRegistration, BusinessSaveModelOutcome.Returned, second.Stamp()));
        Assert.True(second.Owner.RecordReadback(BusinessSaveModelEffect.TypeRegistration, true, second.Stamp()));
        Assert.Equal(new(true, true, true, false), second.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration]);
        Assert.Equal("quarantined", second.Owner.Snapshot.Status);
        Assert.False(second.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, second.Stamp()));
        AssertNoNativeAuthority(second.Owner.Snapshot);
    }

    [Fact]
    public void Reentrant_style_revocation_inside_an_entered_callback_is_not_undone_by_late_return()
    {
        var scenario = new Scenario();
        scenario.Before(BusinessSaveModelEffect.Save);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Save, scenario.Stamp()));
        long ticket = scenario.Owner.EnterCallback(scenario.Stamp());
        Assert.True(ticket > 0);
        scenario.Owner.Revoke("world-exit-during-callback");
        Assert.True(scenario.Owner.ExitCallback(ticket, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Save, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Save, true, scenario.Stamp()));
        Assert.Equal("quarantined", scenario.Owner.Snapshot.Status);
        Assert.Equal("world-exit-during-callback", scenario.Owner.Snapshot.FirstRevocationReason);
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Removal, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Failure_after_return_does_not_erase_return_or_prior_confirmed_readback(bool confirmed)
    {
        var scenario = new Scenario();
        var effect = BusinessSaveModelEffect.TypeRegistration;
        Assert.True(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        if (confirmed) Assert.True(scenario.Owner.RecordReadback(effect, true, scenario.Stamp()));
        Assert.True(confirmed
            ? scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Failed, scenario.Stamp())
            : scenario.Owner.RecordReadback(effect, false, scenario.Stamp()));
        Assert.Equal(new(true, true, confirmed, true), scenario.Owner.Snapshot.Effects[effect]);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
    }

    [Fact]
    public void Foreign_save_observation_consumes_no_own_save_intent_and_cannot_become_a_completion_proof()
    {
        var scenario = new Scenario();
        scenario.Before(BusinessSaveModelEffect.Save);
        Assert.True(scenario.Owner.ObserveUnexpectedSave(scenario.Stamp()));
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Save, scenario.Stamp()));
        Assert.False(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Save].Intent);
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Registration].Intent);
        Assert.True(scenario.Owner.ObserveTelemetry("Complete from foreign/autosave; queued=0; Completed still holds participant", scenario.Stamp()));
        Assert.False(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Removal].Intent);
        Assert.Equal("quarantined", scenario.Owner.Snapshot.Status);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Repeated_parameterless_telemetry_cannot_invent_returns_readbacks_or_a_native_operation_token()
    {
        var scenario = new Scenario();
        for (int index = 0; index < 12; index++)
            Assert.True(scenario.Owner.ObserveTelemetry("Complete before Start; idle; queue empty; later frame", scenario.Stamp()));
        AssertNoIntents(scenario.Owner.Snapshot);
        Assert.False(scenario.Owner.Snapshot.Revoked);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Nested_callback_tickets_can_be_closed_in_finally_without_claiming_native_quiescence()
    {
        var scenario = new Scenario();
        long outer = scenario.Owner.EnterCallback(scenario.Stamp());
        Assert.True(outer > 0);
        try
        {
            long inner = scenario.Owner.EnterCallback(scenario.Stamp());
            Assert.NotEqual(outer, inner);
            Assert.Equal(2, scenario.Owner.Snapshot.ActiveCallbacks);
            try { throw new InvalidOperationException("synthetic-body-failure"); }
            catch (InvalidOperationException) { scenario.Owner.Revoke("synthetic-body-failure"); }
            finally { Assert.True(scenario.Owner.ExitCallback(inner, scenario.Stamp())); }
        }
        finally { Assert.True(scenario.Owner.ExitCallback(outer, scenario.Stamp())); }
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.False(scenario.Owner.Snapshot.CallbacksUncertain);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Active_callback_blocks_a_new_intent_instead_of_becoming_an_implicit_effect_entry()
    {
        var scenario = new Scenario();
        long ticket = scenario.Owner.EnterCallback(scenario.Stamp());
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, scenario.Stamp()));
        AssertNoIntents(scenario.Owner.Snapshot);
        Assert.True(scenario.Owner.ExitCallback(ticket, scenario.Stamp()));
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Constructor_style_callback_before_binding_closes_admission_and_marks_uncertainty()
    {
        var process = new BusinessSaveProcessModel();
        var owner = process.CreateOwner();
        Assert.Equal(0, owner.EnterCallback(new(new(process.ProcessId, 1), Candidate, 1, BeganAt)));
        Assert.True(owner.Snapshot.CallbacksUncertain);
        Assert.True(owner.Snapshot.Revoked);
        Assert.False(process.CreateOwner().TryBind(Candidate, BeganAt));
        Assert.False(owner.Snapshot.SlotConsumed);
        AssertNoIntents(owner.Snapshot);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Unknown_or_duplicate_callback_exit_never_decrements_a_different_live_ticket(bool duplicate)
    {
        var scenario = new Scenario();
        long outer = scenario.Owner.EnterCallback(scenario.Stamp());
        long inner = scenario.Owner.EnterCallback(scenario.Stamp());
        if (duplicate) Assert.True(scenario.Owner.ExitCallback(inner, scenario.Stamp()));
        Assert.False(scenario.Owner.ExitCallback(duplicate ? inner : 99999, scenario.Stamp()));
        Assert.Equal(duplicate ? 1 : 2, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.True(scenario.Owner.ExitCallback(outer, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Revoked);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Callback_capacity_is_bounded_even_when_late_entries_arrive_after_quarantine()
    {
        var scenario = new Scenario();
        var tickets = new List<long>();
        for (int index = 0; index < BusinessSaveProcessModel.DiagnosticCapacity; index++)
        {
            long ticket = scenario.Owner.EnterCallback(scenario.Stamp());
            Assert.True(ticket > 0);
            tickets.Add(ticket);
        }
        Assert.Equal(64, tickets.Distinct().Count());
        Assert.Equal(0, scenario.Owner.EnterCallback(scenario.Stamp()));
        Assert.Equal(64, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        foreach (long ticket in tickets) Assert.True(scenario.Owner.ExitCallback(ticket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("intent")]
    [InlineData("outcome")]
    [InlineData("readback")]
    [InlineData("enter")]
    [InlineData("exit")]
    [InlineData("telemetry")]
    [InlineData("context")]
    [InlineData("unexpected-save")]
    [InlineData("tick")]
    public void Off_thread_mutations_revoke_without_admitting_an_effect(string method)
    {
        var scenario = new Scenario();
        var stamp = scenario.Stamp();
        OnOtherThread(() =>
        {
            switch (method)
            {
                case "intent": Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, stamp)); break;
                case "outcome": Assert.False(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.TypeRegistration, BusinessSaveModelOutcome.Returned, stamp)); break;
                case "readback": Assert.False(scenario.Owner.RecordReadback(BusinessSaveModelEffect.TypeRegistration, true, stamp)); break;
                case "enter": Assert.Equal(0, scenario.Owner.EnterCallback(stamp)); break;
                case "exit": Assert.False(scenario.Owner.ExitCallback(1, stamp)); break;
                case "telemetry": Assert.False(scenario.Owner.ObserveTelemetry("Complete", stamp)); break;
                case "context": Assert.False(scenario.Owner.ObserveContext(scenario.Binding, stamp)); break;
                case "unexpected-save": Assert.False(scenario.Owner.ObserveUnexpectedSave(stamp)); break;
                case "tick": Assert.False(scenario.Owner.Tick(BeganAt)); break;
                default: throw new ArgumentOutOfRangeException(nameof(method));
            }
        });
        Assert.True(scenario.Owner.Snapshot.Revoked);
        if (method is "enter" or "exit") Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        AssertNoIntents(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Facade_creation_on_another_thread_does_not_change_the_models_admission_thread()
    {
        var process = new BusinessSaveProcessModel();
        OnOtherThread(() => Assert.False(process.CreateOwner().TryBind(Candidate, BeganAt)));
        Assert.True(process.CreateOwner().Snapshot.Revoked);
        Assert.False(process.CreateOwner().TryBind(Candidate, BeganAt));
        Assert.False(process.CreateOwner().Snapshot.SlotConsumed);
    }

    [Fact]
    public void Snapshot_and_immediate_revocation_are_allowed_from_another_thread()
    {
        var scenario = new Scenario();
        OnOtherThread(() =>
        {
            Assert.Equal(scenario.Binding, scenario.Owner.Snapshot.Binding);
            Assert.False(scenario.Owner.Snapshot.Revoked);
            scenario.Owner.Revoke(new string('r', 10000));
            Assert.True(scenario.Owner.Snapshot.Revoked);
        });
        string first = scenario.Owner.Snapshot.FirstRevocationReason!;
        Assert.InRange(first.Length, 1, 160);
        scenario.Owner.Revoke("later-cause");
        Assert.Equal(first, scenario.Owner.Snapshot.FirstRevocationReason);
        Assert.False(scenario.Owner.Tick(BeganAt));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    public void Nonadvancing_or_invalid_sequence_latches_incomplete_diagnosis(long sequence)
    {
        var scenario = new Scenario();
        var first = scenario.Stamp();
        Assert.True(scenario.Owner.ObserveTelemetry("first", first));
        Assert.False(scenario.Owner.ObserveTelemetry("duplicate-or-backwards", first with { Sequence = sequence }));
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.DiagnosticsIncomplete);
        Assert.Equal(1, scenario.Owner.Snapshot.LastSequence);
        AssertNoIntents(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Sequence_exhaustion_never_wraps_into_a_new_message_or_operation()
    {
        var scenario = new Scenario();
        var stamp = scenario.Stamp() with { Sequence = long.MaxValue };
        scenario.Owner.ObserveTelemetry("last representable sequence", stamp);
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, stamp with { Sequence = long.MinValue }));
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.DiagnosticsIncomplete);
        Assert.InRange(scenario.Owner.Snapshot.LastSequence, 0, long.MaxValue);
        AssertNoIntents(scenario.Owner.Snapshot);
        Assert.False(scenario.Process.CreateOwner().TryBind(Candidate, BeganAt));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(99)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Invalid_or_backwards_observation_time_revokes_without_effect(double time)
    {
        var scenario = new Scenario();
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, scenario.Stamp(time)));
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.DiagnosticsIncomplete);
        AssertNoIntents(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Timeout_is_diagnostic_quarantine_not_cancellation_cleanup_or_permission()
    {
        var scenario = new Scenario();
        Assert.True(scenario.Owner.Tick(BeganAt + BusinessSaveProcessModel.TimeoutSeconds - 0.001));
        Assert.False(scenario.Owner.Tick(BeganAt + BusinessSaveProcessModel.TimeoutSeconds));
        Assert.Equal("quarantined", scenario.Owner.Snapshot.Status);
        Assert.True(scenario.Owner.Snapshot.SlotConsumed);
        Assert.True(scenario.Owner.ObserveTelemetry("late completion", scenario.Stamp(BeganAt + 61)));
        AssertNoIntents(scenario.Owner.Snapshot);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Observation_at_timeout_records_late_partial_effect_without_reviving_operation()
    {
        var scenario = new Scenario();
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.TypeRegistration, BusinessSaveModelOutcome.Returned, scenario.Stamp(BeganAt + 60)));
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.Equal(new(true, true, false, false), scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration]);
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp(BeganAt + 60)));
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Ring_overflow_preserves_first_cause_and_bounded_latest_diagnostics_not_full_payloads()
    {
        var scenario = new Scenario();
        Assert.True(scenario.Owner.ObserveTelemetry(new string('p', 10000), scenario.Stamp() with { Source = new string('s', 10000) }));
        scenario.Owner.Revoke("first-cause");
        for (int index = 0; index < 200; index++)
            Assert.True(scenario.Owner.ObserveTelemetry("late-" + index, scenario.Stamp()));
        var snapshot = scenario.Owner.Snapshot;
        Assert.InRange(snapshot.Diagnostics.Count, 1, BusinessSaveProcessModel.DiagnosticCapacity);
        Assert.True(snapshot.DiagnosticsIncomplete);
        Assert.True(snapshot.Revoked);
        Assert.Equal("first-cause", snapshot.FirstRevocationReason);
        Assert.Equal(201, snapshot.LastSequence);
        Assert.All(snapshot.Diagnostics, diagnostic =>
        {
            Assert.InRange(diagnostic.Kind.Length, 0, BusinessSaveProcessModel.MaxTextLength);
            Assert.InRange(diagnostic.Detail.Length, 0, BusinessSaveProcessModel.MaxTextLength);
        });
        AssertNoNativeAuthority(snapshot);
    }

    [Fact]
    public void Overflow_alone_revokes_without_forgetting_possible_effects()
    {
        var scenario = new Scenario();
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, scenario.Stamp()));
        for (int index = 0; index < 80; index++) scenario.Owner.ObserveTelemetry("uncorrelated", scenario.Stamp());
        Assert.True(scenario.Owner.Snapshot.DiagnosticsIncomplete);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration].Intent);
        Assert.False(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration].Returned);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData(BusinessSaveModelEffect.TypeRegistration)]
    [InlineData(BusinessSaveModelEffect.Construction)]
    [InlineData(BusinessSaveModelEffect.Registration)]
    [InlineData(BusinessSaveModelEffect.Save)]
    [InlineData(BusinessSaveModelEffect.Removal)]
    [InlineData(BusinessSaveModelEffect.Destroy)]
    public void Failed_readback_preserves_each_effects_unknown_result_without_automatic_compensation(BusinessSaveModelEffect effect)
    {
        var scenario = new Scenario();
        scenario.Before(effect);
        Assert.True(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(effect, false, scenario.Stamp()));
        Assert.Equal(new(true, true, false, true), scenario.Owner.Snapshot.Effects[effect]);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        foreach (var later in Effects.SkipWhile(value => value != effect).Skip(1))
            Assert.Equal(new(false, false, false, false), scenario.Owner.Snapshot.Effects[later]);
        Assert.False(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData(BusinessSaveModelEffect.TypeRegistration)]
    [InlineData(BusinessSaveModelEffect.Construction)]
    [InlineData(BusinessSaveModelEffect.Registration)]
    [InlineData(BusinessSaveModelEffect.Save)]
    [InlineData(BusinessSaveModelEffect.Removal)]
    [InlineData(BusinessSaveModelEffect.Destroy)]
    public void Late_return_after_failed_outcome_preserves_both_and_duplicate_failure_cannot_retry(BusinessSaveModelEffect effect)
    {
        var scenario = new Scenario();
        scenario.Before(effect);
        Assert.True(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Failed, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(effect, true, scenario.Stamp()));
        Assert.Equal(new(true, true, true, true), scenario.Owner.Snapshot.Effects[effect]);
        Assert.False(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Failed, scenario.Stamp()));
        Assert.Equal(new(true, true, true, true), scenario.Owner.Snapshot.Effects[effect]);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Failed_readback_and_failed_outcome_are_separately_recordable_observations()
    {
        var scenario = new Scenario();
        var effect = BusinessSaveModelEffect.TypeRegistration;
        Assert.True(scenario.Owner.TryRecordIntent(effect, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(effect, false, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Failed, scenario.Stamp()));
        Assert.Equal(new(true, true, false, true), scenario.Owner.Snapshot.Effects[effect]);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Invalid_callback_stamp_preserves_live_tickets_and_marks_uncertainty(bool exit)
    {
        var scenario = new Scenario();
        long ticket = scenario.Owner.EnterCallback(scenario.Stamp());
        var wrong = scenario.Stamp() with { Key = new(Guid.NewGuid(), 1) };
        if (exit) Assert.False(scenario.Owner.ExitCallback(ticket, wrong));
        else Assert.Equal(0, scenario.Owner.EnterCallback(wrong));
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.ExitCallback(ticket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
    }

    [Fact]
    public void Bind_during_an_existing_synthetic_callback_cannot_create_a_reentrant_second_operation()
    {
        var scenario = new Scenario();
        long ticket = scenario.Owner.EnterCallback(scenario.Stamp());
        var key = scenario.Owner.Snapshot.Key;
        Assert.False(scenario.Process.CreateOwner().TryBind(Candidate with { Epoch = 18 }, BeganAt));
        Assert.Equal(key, scenario.Owner.Snapshot.Key);
        Assert.Equal(scenario.Binding, scenario.Owner.Snapshot.Binding);
        Assert.True(scenario.Owner.ExitCallback(ticket, scenario.Stamp()));
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(99)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void Invalid_tick_clock_is_also_a_diagnostic_gap_not_an_expiry_reset(double time)
    {
        var scenario = new Scenario();
        Assert.False(scenario.Owner.Tick(time));
        Assert.True(scenario.Owner.Snapshot.DiagnosticsIncomplete);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.Tick(BeganAt));
        AssertNoIntents(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Finite_zero_time_and_maximum_label_length_are_valid_model_boundaries()
    {
        string label = new('a', BusinessSaveProcessModel.MaxTextLength);
        var binding = Candidate with
        {
            Epoch = long.MaxValue,
            LoadManager = label,
            SaveManager = label,
            Saveables = label,
            BaseSaveables = label,
            Completed = label,
            Requests = label,
            Participant = label
        };
        var owner = new BusinessSaveProcessModel().CreateOwner();
        Assert.True(owner.TryBind(binding, 0));
        var snapshot = owner.Snapshot;
        Assert.True(owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, new(snapshot.Key!.Value, snapshot.Binding!, 1, 0)));
        AssertNoNativeAuthority(owner.Snapshot);
    }

    [Fact]
    public void Snapshots_are_independent_read_only_copies_not_live_mutable_ledgers()
    {
        var scenario = new Scenario();
        var old = scenario.Owner.Snapshot;
        int oldDiagnosticCount = old.Diagnostics.Count;
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        scenario.Owner.Revoke("stop");
        Assert.False(old.Revoked);
        Assert.Equal("bound-data-only", old.Status);
        Assert.Equal(oldDiagnosticCount, old.Diagnostics.Count);
        AssertNoIntents(old);
        if (old.Effects is IDictionary<BusinessSaveModelEffect, BusinessSaveModelEffectState> effects)
            Assert.Throws<NotSupportedException>(() => effects[BusinessSaveModelEffect.TypeRegistration] = new(true, true, true, true));
        if (old.Diagnostics is IList<BusinessSaveModelDiagnostic> diagnostics)
            Assert.Throws<NotSupportedException>(() => diagnostics.Add(new(999, "forged", "proof")));
        Assert.Equal(new(true, true, true, false), scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration]);
        Assert.Equal("stop", scenario.Owner.Snapshot.FirstRevocationReason);
        AssertNoNativeAuthority(old);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Armed_constructor_callback_binds_participant_before_add_component_return_and_admits_bound_callbacks()
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
        Assert.Null(scenario.Owner.Snapshot.ConstructedParticipant);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.ConstructionArmed);
        Assert.Null(scenario.Owner.Snapshot.ConstructedParticipant);

        long ctorTicket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
        Assert.True(ctorTicket > 0);
        Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
        Assert.Equal(scenario.Binding.Participant, scenario.Owner.Snapshot.ConstructedParticipant);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.ExitCallback(ctorTicket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);

        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Construction, true, scenario.Stamp()));
        Assert.False(scenario.Owner.Snapshot.Revoked);

        long shouldSaveTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "ShouldSave", scenario.Stamp());
        Assert.True(shouldSaveTicket > 0);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.ExitCallback(shouldSaveTicket, scenario.Stamp()));

        long initTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "InitializeSaveable", scenario.Stamp());
        Assert.True(initTicket > 0);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.ExitCallback(initTicket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.False(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.Snapshot.CallbacksUncertain);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Late_participant_callback_after_revocation_still_tracks_ticket_for_finally_exit_without_reopening_admission()
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        long ctorTicket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
        Assert.True(scenario.Owner.ExitCallback(ctorTicket, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Construction, true, scenario.Stamp()));

        scenario.Owner.Revoke("world-exit");
        long lateTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "OnDestroy", scenario.Stamp());
        Assert.True(lateTicket > 0);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.ExitCallback(lateTicket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.Equal("quarantined", scenario.Owner.Snapshot.Status);
        Assert.Equal("world-exit", scenario.Owner.Snapshot.FirstRevocationReason);
        Assert.False(scenario.Owner.Snapshot.CallbacksUncertain);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("before-construction-intent")]
    [InlineData("after-construction-returned")]
    [InlineData("duplicate-arm")]
    [InlineData("after-constructor-bound")]
    [InlineData("active-callback")]
    public void Invalid_construction_arming_revokes_without_granting_constructor_window(string cause)
    {
        var scenario = new Scenario();
        if (cause != "before-construction-intent")
        {
            scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
            Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        }
        if (cause == "after-construction-returned")
            Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        if (cause == "duplicate-arm")
            Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        if (cause == "after-constructor-bound")
        {
            Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
            long ticket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
            Assert.True(scenario.Owner.ExitCallback(ticket, scenario.Stamp()));
        }
        long active = 0;
        if (cause == "active-callback")
        {
            active = scenario.Owner.EnterCallback(scenario.Stamp());
            Assert.True(active > 0);
        }

        Assert.False(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        if (active > 0) Assert.True(scenario.Owner.ExitCallback(active, scenario.Stamp()));
        Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.Equal("construction-arm-invalid", scenario.Owner.Snapshot.FirstRevocationReason);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Armed_construction_returning_without_constructor_callback_revokes_and_marks_callbacks_uncertain()
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        Assert.False(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
        Assert.Null(scenario.Owner.Snapshot.ConstructedParticipant);
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Construction].Returned);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.Equal("construction-returned-without-constructor-bind", scenario.Owner.Snapshot.FirstRevocationReason);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("revoke")]
    [InlineData("unexpected-save")]
    public void Revocation_while_construction_is_armed_immediately_disarms_and_rejects_late_constructor_or_unbound_return(string trigger)
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.ConstructionArmed);

        if (trigger == "revoke") scenario.Owner.Revoke("world-exit-while-armed");
        else Assert.True(scenario.Owner.ObserveUnexpectedSave(scenario.Stamp()));

        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
        Assert.Null(scenario.Owner.Snapshot.ConstructedParticipant);
        Assert.Equal(0, scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.False(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Construction].Returned);
        Assert.Equal(trigger == "revoke" ? "world-exit-while-armed" : "unexpected-save", scenario.Owner.Snapshot.FirstRevocationReason);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("unarmed")]
    [InlineData("wrong-participant")]
    [InlineData("empty-participant")]
    [InlineData("control-participant")]
    [InlineData("second-constructor")]
    public void Invalid_constructor_callback_revokes_and_never_binds_a_second_or_foreign_instance(string cause)
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        if (cause != "unarmed")
            Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        if (cause == "second-constructor")
        {
            long first = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
            Assert.True(first > 0);
            Assert.True(scenario.Owner.ExitCallback(first, scenario.Stamp()));
        }
        string? candidate = cause switch
        {
            "wrong-participant" => "foreign-participant-999",
            "empty-participant" => "",
            "control-participant" => "participant\0bad",
            _ => scenario.Binding.Participant
        };
        Assert.Equal(0, scenario.Owner.EnterConstructorCallback(candidate, scenario.Stamp()));
        Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
        if (cause != "second-constructor") Assert.Null(scenario.Owner.Snapshot.ConstructedParticipant);
        else Assert.Equal(scenario.Binding.Participant, scenario.Owner.Snapshot.ConstructedParticipant);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.Equal("constructor-callback-invalid", scenario.Owner.Snapshot.FirstRevocationReason);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("before-constructor")]
    [InlineData("while-armed")]
    [InlineData("wrong-participant")]
    [InlineData("empty-callback")]
    [InlineData("control-callback")]
    public void Invalid_participant_callback_revokes_and_marks_uncertainty(string cause)
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        if (cause == "while-armed")
            Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        else if (cause != "before-constructor")
        {
            Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
            long ctorTicket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
            Assert.True(scenario.Owner.ExitCallback(ctorTicket, scenario.Stamp()));
        }
        string? participant = cause == "wrong-participant" ? "foreign-participant-999" : scenario.Binding.Participant;
        string? callback = cause switch
        {
            "empty-callback" => " ",
            "control-callback" => "ShouldSave\n",
            _ => "ShouldSave"
        };
        Assert.Equal(0, scenario.Owner.EnterParticipantCallback(participant, callback, scenario.Stamp()));
        Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.Equal("participant-callback-invalid", scenario.Owner.Snapshot.FirstRevocationReason);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Theory]
    [InlineData("arm")]
    [InlineData("enter-constructor")]
    [InlineData("enter-participant")]
    public void Off_thread_construction_and_participant_callbacks_revoke_without_binding(string method)
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        if (method != "arm")
            Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        if (method == "enter-participant")
        {
            long ctorTicket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
            Assert.True(scenario.Owner.ExitCallback(ctorTicket, scenario.Stamp()));
        }
        var stamp = scenario.Stamp();
        OnOtherThread(() =>
        {
            switch (method)
            {
                case "arm": Assert.False(scenario.Owner.TryArmParticipantConstruction(stamp)); break;
                case "enter-constructor": Assert.Equal(0, scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, stamp)); break;
                case "enter-participant": Assert.Equal(0, scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "ShouldSave", stamp)); break;
                default: throw new ArgumentOutOfRangeException(nameof(method));
            }
        });
        Assert.True(scenario.Owner.Snapshot.Revoked);
        if (method != "arm") Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        if (method == "enter-constructor")
        {
            Assert.False(scenario.Owner.Snapshot.ConstructionArmed);
            Assert.Null(scenario.Owner.Snapshot.ConstructedParticipant);
        }
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Saveable_contract_inventory_matches_all_23_native_slots_and_disassembly_metadata()
    {
        var slots = BusinessSaveableContractInventory.Slots;
        Assert.Equal(BusinessSaveableContractInventory.TotalInterfaceSlots, slots.Count);
        Assert.Equal(23, BusinessSaveableContractInventory.InterfaceOffsetOnBusiness);
        Assert.Equal(16, BusinessSaveableContractInventory.ExpectedProbeSubtypeBoundVirtualSlots);

        for (int index = 0; index < slots.Count; index++)
        {
            var slot = slots[index];
            Assert.Equal(index, slot.Slot);
            Assert.Equal(BusinessSaveableContractInventory.InterfaceOffsetOnBusiness + index, slot.ClassVtableSlot);
            Assert.Equal(0x06001778 + index, slot.Token);
            Assert.False(string.IsNullOrWhiteSpace(slot.Name));
            Assert.False(string.IsNullOrWhiteSpace(slot.DispatchHazard));
            if (index < 12)
            {
                Assert.False(slot.HasDefaultInterfaceBody);
                Assert.Equal("0x0", slot.DefaultRva);
                Assert.False(slot.DefaultPerformsDiskIo);
                Assert.True(slot.ImplementedByPropertyOrBusiness);
            }
            else
            {
                Assert.True(slot.HasDefaultInterfaceBody);
                Assert.NotEqual("0x0", slot.DefaultRva);
            }
        }

        Assert.Equal(new[] { "get_ShouldSaveUnderFolder", "InitializeSaveable", "GetSaveString", "WriteData", "DeleteUnapprovedFiles" },
            slots.Where(slot => slot.OverriddenOnProbeSubtype).Select(slot => slot.Name).ToArray());
        Assert.Equal(new[] { 12, 13, 15, 16, 18, 19, 20, 21, 22 },
            slots.Where(slot => slot.DefaultPerformsDiskIo).Select(slot => slot.Slot).ToArray());
        Assert.Equal("0x582F00", slots[15].DefaultRva);
        Assert.Equal("0x583840", slots[18].DefaultRva);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("subtype-slots")]
    [InlineData("interface-slots")]
    [InlineData("saveables-deadlock")]
    [InlineData("extra-files-null")]
    [InlineData("extra-folders-null")]
    [InlineData("save-folder-null")]
    [InlineData("completesave-unoverridden")]
    [InlineData("container-folder-unoverridden")]
    [InlineData("whole-world-save")]
    [InlineData("admissible-with-base-driver")]
    [InlineData("admissible-with-business-driver")]
    [InlineData("admissible-unregistered")]
    public void Saveable_contract_evaluator_identifies_every_native_dispatch_and_completion_hazard(string scenarioCase)
    {
        var baseline = new BusinessSaveableEvaluationInput(
            RegisteredInSaveables: true,
            RegisteredInBaseSaveables: true,
            RegisteredInBusinessOrPropertyList: false,
            LocalExtraFilesInitialized: true,
            LocalExtraFoldersInitialized: true,
            SaveFolderNameInitialized: true,
            ShouldSaveUnderFolder: false,
            CompleteSaveOverridden: false,
            GetContainerFolderOverridden: false,
            WholeWorldSaveRequested: false,
            BoundSubtypeVirtualSlotCount: BusinessSaveableContractInventory.ExpectedProbeSubtypeBoundVirtualSlots,
            VerifiedInterfaceSlotCount: BusinessSaveableContractInventory.TotalInterfaceSlots);

        BusinessSaveableEvaluationInput? input = scenarioCase switch
        {
            "null" => null,
            "subtype-slots" => baseline with { BoundSubtypeVirtualSlotCount = 12 },
            "interface-slots" => baseline with { VerifiedInterfaceSlotCount = 1 },
            "saveables-deadlock" => baseline with { RegisteredInBaseSaveables = false, RegisteredInBusinessOrPropertyList = false },
            "extra-files-null" => baseline with { LocalExtraFilesInitialized = false },
            "extra-folders-null" => baseline with { LocalExtraFoldersInitialized = false },
            "save-folder-null" => baseline with { SaveFolderNameInitialized = false },
            "completesave-unoverridden" => baseline with { ShouldSaveUnderFolder = true, CompleteSaveOverridden = false, GetContainerFolderOverridden = true },
            "container-folder-unoverridden" => baseline with { ShouldSaveUnderFolder = true, CompleteSaveOverridden = true, GetContainerFolderOverridden = false },
            "whole-world-save" => baseline with { WholeWorldSaveRequested = true },
            "admissible-with-base-driver" => baseline,
            "admissible-with-business-driver" => baseline with { RegisteredInBaseSaveables = false, RegisteredInBusinessOrPropertyList = true },
            "admissible-unregistered" => baseline with { RegisteredInSaveables = false, RegisteredInBaseSaveables = false },
            _ => throw new ArgumentOutOfRangeException(nameof(scenarioCase))
        };

        var result = BusinessSaveableContractInventory.Evaluate(input);
        Assert.False(result.NativeSaveWriteAllowed);
        if (scenarioCase.StartsWith("admissible-", StringComparison.Ordinal))
        {
            Assert.True(result.Admissible);
            Assert.Empty(result.Findings);
            return;
        }

        Assert.False(result.Admissible);
        string expectedFinding = scenarioCase switch
        {
            "null" => "evaluation-input-missing",
            "subtype-slots" => "subtype-virtual-slot-binding-incomplete",
            "interface-slots" => "interface-slot-verification-incomplete",
            "saveables-deadlock" => "saveables-without-driver-deadlocks-saveroutine",
            "extra-files-null" or "extra-folders-null" => "uninitialized-extra-lists-nre-in-completesave",
            "save-folder-null" => "uninitialized-save-folder-name-in-local-path",
            "completesave-unoverridden" or "container-folder-unoverridden" => "default-completesave-creates-container-folder-and-deletes-unapproved-entries",
            "whole-world-save" => "whole-world-save-writes-foreign-saveables",
            _ => throw new ArgumentOutOfRangeException(nameof(scenarioCase))
        };
        Assert.Equal(new[] { expectedFinding }, result.Findings);
    }

    [Fact]
    public void ShouldSaveUnderFolder_slot_contract_and_hazard_evaluation_bypasses_io()
    {
        var slots = BusinessSaveableContractInventory.Slots;
        var slot3 = slots[3];
        Assert.Equal(3, slot3.Slot);
        Assert.Equal(26, slot3.ClassVtableSlot);
        Assert.Equal("get_ShouldSaveUnderFolder", slot3.Name);
        Assert.Equal(0x0600177B, slot3.Token);
        Assert.Equal(0, slot3.ParameterCount);
        Assert.False(slot3.HasDefaultInterfaceBody);
        Assert.Equal("0x0", slot3.DefaultRva);
        Assert.False(slot3.DefaultPerformsDiskIo);
        Assert.True(slot3.ImplementedByPropertyOrBusiness);
        Assert.True(slot3.OverriddenOnProbeSubtype);
        Assert.Contains("bypass", slot3.DispatchHazard, StringComparison.OrdinalIgnoreCase);

        var baseline = new BusinessSaveableEvaluationInput(
            RegisteredInSaveables: true,
            RegisteredInBaseSaveables: true,
            RegisteredInBusinessOrPropertyList: false,
            LocalExtraFilesInitialized: true,
            LocalExtraFoldersInitialized: true,
            SaveFolderNameInitialized: true,
            ShouldSaveUnderFolder: false,
            CompleteSaveOverridden: false,
            GetContainerFolderOverridden: false,
            WholeWorldSaveRequested: false,
            BoundSubtypeVirtualSlotCount: 16,
            VerifiedInterfaceSlotCount: 23);

        var evalBypassed = BusinessSaveableContractInventory.Evaluate(baseline);
        Assert.True(evalBypassed.Admissible);
        Assert.Empty(evalBypassed.Findings);

        var evalHazard = BusinessSaveableContractInventory.Evaluate(baseline with { ShouldSaveUnderFolder = true });
        Assert.False(evalHazard.Admissible);
        Assert.Contains("default-completesave-creates-container-folder-and-deletes-unapproved-entries", evalHazard.Findings);

        var evalOldSlots = BusinessSaveableContractInventory.Evaluate(baseline with { BoundSubtypeVirtualSlotCount = 15 });
        Assert.False(evalOldSlots.Admissible);
        Assert.Contains("subtype-virtual-slot-binding-incomplete", evalOldSlots.Findings);
    }

    [Fact]
    public void Participant_callback_tracking_tracks_get_ShouldSaveUnderFolder_lifecycle()
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        long ctorTicket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
        Assert.True(scenario.Owner.ExitCallback(ctorTicket, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Construction, true, scenario.Stamp()));

        long ticket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "get_ShouldSaveUnderFolder", scenario.Stamp());
        Assert.True(ticket > 0);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.False(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.Snapshot.CallbacksUncertain);

        Assert.True(scenario.Owner.ExitCallback(ticket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.False(scenario.Owner.Snapshot.Revoked);

        long badTicket = scenario.Owner.EnterParticipantCallback("foreign-participant-x", "get_ShouldSaveUnderFolder", scenario.Stamp());
        Assert.Equal(0, badTicket);
        Assert.True(scenario.Owner.Snapshot.Revoked);
        Assert.True(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.Equal("participant-callback-invalid", scenario.Owner.Snapshot.FirstRevocationReason);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Full_non_io_save_completion_flow_executes_participant_callbacks_and_readback()
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        long ctorTicket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
        Assert.True(scenario.Owner.ExitCallback(ctorTicket, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Construction, true, scenario.Stamp()));

        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Registration, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Registration, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Registration, true, scenario.Stamp()));

        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Save, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Save].Intent);

        long folderTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "get_ShouldSaveUnderFolder", scenario.Stamp());
        Assert.True(folderTicket > 0);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.ExitCallback(folderTicket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);

        long writeTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "WriteData", scenario.Stamp());
        Assert.True(writeTicket > 0);
        Assert.Equal(1, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.True(scenario.Owner.ExitCallback(writeTicket, scenario.Stamp()));
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);

        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Save, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Save].Returned);

        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Save, true, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Save].ReadbackConfirmed);
        Assert.False(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.Snapshot.CallbacksUncertain);
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Reversal_hygiene_removes_from_saveables_and_completed_saveables_monotonically()
    {
        var scenario = new Scenario();
        scenario.Complete(BusinessSaveModelEffect.TypeRegistration);
        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Construction, scenario.Stamp()));
        Assert.True(scenario.Owner.TryArmParticipantConstruction(scenario.Stamp()));
        long ctorTicket = scenario.Owner.EnterConstructorCallback(scenario.Binding.Participant, scenario.Stamp());
        Assert.True(scenario.Owner.ExitCallback(ctorTicket, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Construction, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Construction, true, scenario.Stamp()));

        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Registration, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Registration, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Registration, true, scenario.Stamp()));

        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Save, scenario.Stamp()));
        long folderTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "get_ShouldSaveUnderFolder", scenario.Stamp());
        Assert.True(scenario.Owner.ExitCallback(folderTicket, scenario.Stamp()));
        long writeTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "WriteData", scenario.Stamp());
        Assert.True(scenario.Owner.ExitCallback(writeTicket, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Save, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Save, true, scenario.Stamp()));

        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Removal, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Removal, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Removal, true, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Removal].ReadbackConfirmed);

        Assert.True(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Destroy, scenario.Stamp()));
        long destroyTicket = scenario.Owner.EnterParticipantCallback(scenario.Binding.Participant, "OnDestroy", scenario.Stamp());
        Assert.True(destroyTicket > 0);
        Assert.True(scenario.Owner.ExitCallback(destroyTicket, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordOutcome(BusinessSaveModelEffect.Destroy, BusinessSaveModelOutcome.Returned, scenario.Stamp()));
        Assert.True(scenario.Owner.RecordReadback(BusinessSaveModelEffect.Destroy, true, scenario.Stamp()));
        Assert.True(scenario.Owner.Snapshot.Effects[BusinessSaveModelEffect.Destroy].ReadbackConfirmed);

        Assert.False(scenario.Owner.Snapshot.Revoked);
        Assert.False(scenario.Owner.Snapshot.CallbacksUncertain);
        Assert.Equal(0, scenario.Owner.Snapshot.ActiveCallbacks);
        Assert.All(scenario.Owner.Snapshot.Effects.Values, effect =>
        {
            Assert.True(effect.Intent);
            Assert.True(effect.Returned);
            Assert.True(effect.ReadbackConfirmed);
            Assert.False(effect.Failure);
        });

        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Save, scenario.Stamp()));
        Assert.False(scenario.Owner.TryRecordIntent(BusinessSaveModelEffect.Registration, scenario.Stamp()));
        AssertNoNativeAuthority(scenario.Owner.Snapshot);
    }

    [Fact]
    public void Monotonic_frame_progression_advances_monotonically_across_same_frame_reads()
    {
        long lastFrame = -1;
        long Next(long unityFrame)
        {
            while (true)
            {
                long last = Volatile.Read(ref lastFrame);
                long target = Math.Max(unityFrame, last + 1);
                if (Interlocked.CompareExchange(ref lastFrame, target, last) == last)
                    return target;
            }
        }

        // Multiple reads within the same Unity frame (e.g., 5000) must strictly increment
        Assert.Equal(5000, Next(5000));
        Assert.Equal(5001, Next(5000));
        Assert.Equal(5002, Next(5000));

        // When Unity frame advances to 5010, next read catches up to 5010 and then increments
        Assert.Equal(5010, Next(5010));
        Assert.Equal(5011, Next(5010));
        Assert.Equal(5012, Next(5010));
    }
}

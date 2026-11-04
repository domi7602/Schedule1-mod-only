using System.Reflection;
using System.Reflection.Emit;
using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessTypeResolutionTests
{
    private const int Thread = 7;
    private const long NativeClass = 42;
    private const string Qualified = "Il2CppFishNet.Connection.NetworkConnection, Il2CppFishNet.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
    private static SubtypeTypeNamePolicy Policy(bool alreadyResolvable = false) => new(Thread, NativeClass, Qualified, alreadyResolvable);

    [Fact]
    public void Captured_networkconnection_lookup_fails_with_installed_algorithm_and_resolves_with_scoped_fix()
    {
        // Game-free fixture preserves the exact captured native/proxy names and assembly split.
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Il2CppFishNet.Runtime"), AssemblyBuilderAccess.Run);
        var proxy = assembly.DefineDynamicModule("proxy").DefineType("Il2CppFishNet.Connection.NetworkConnection", TypeAttributes.Public).CreateType()!;
        Type? Resolve(string name) => Type.GetType(name,
            requested => requested.Name == assembly.GetName().Name ? assembly : null,
            (found, type, ignoreCase) => found?.GetType(type, false, ignoreCase), false);
        Type InstalledLookup(string raw) => Resolve(raw) ?? Resolve("Il2Cpp" + raw) ?? throw new NullReferenceException("Couldn't find System.Type for Il2Cpp type: " + raw);
        Assert.Throws<NullReferenceException>(() => InstalledLookup(SubtypeTypeNamePolicy.NativeName));
        var policy = new SubtypeTypeNamePolicy(Thread, NativeClass, proxy.AssemblyQualifiedName!, false);
        policy.Activate();
        bool entered = policy.Enter(Thread, true);
        Assert.Equal(proxy, Resolve(policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName)));
        policy.Exit(Thread, entered); policy.Deactivate();
        Assert.Throws<NullReferenceException>(() => InstalledLookup(policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName)));
        Assert.Equal(1, policy.MappingHits);
    }

    [Theory]
    [InlineData("inactive")]
    [InlineData("no-registration")]
    [InlineData("thread")]
    [InlineData("class")]
    [InlineData("known")]
    public void Resolution_does_not_escape_its_validated_binding(string reason)
    {
        var policy = Policy(reason == "known");
        if (reason != "inactive") policy.Activate();
        if (reason != "no-registration") policy.Enter(Thread, true);
        Assert.Equal(SubtypeTypeNamePolicy.NativeName, policy.Rewrite(reason == "thread" ? 8 : Thread,
            reason == "class" ? 43 : NativeClass, SubtypeTypeNamePolicy.NativeName));
        Assert.Equal(0, policy.MappingHits);
    }

    [Theory]
    [InlineData("FishNet.Object.NetworkObject, FishNet.Runtime")]
    [InlineData("FishNet.Connection.NetworkConnection, Other.Runtime")]
    [InlineData("FishNet.Connection.NetworkConnection, FishNet.Runtime, Version=0.0.0.0")]
    [InlineData("Il2CppFishNet.Connection.NetworkConnection, Il2CppFishNet.Runtime")]
    [InlineData("System.Collections.Generic.List`1")]
    [InlineData("ScheduleOne.Money.MoneyManager+FloatContainer, Assembly-CSharp")]
    public void Unknown_already_rewritten_generic_and_nested_names_are_left_to_original_resolver(string raw)
    {
        var policy = Policy(); policy.Activate(); policy.Enter(Thread, true);
        Assert.Equal(raw, policy.Rewrite(Thread, NativeClass, raw)); Assert.Equal(0, policy.MappingHits);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Nested_registration_cannot_inherit_own_scope_and_invalidates_attempt(bool nestedOwn)
    {
        var policy = Policy(); policy.Activate(); bool outer = policy.Enter(Thread, true);
        bool inner = policy.Enter(Thread, nestedOwn);
        Assert.Equal(SubtypeTypeNamePolicy.NativeName, policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName));
        policy.Exit(Thread, inner); policy.Exit(Thread, outer);
        Assert.True(policy.Violated); Assert.Equal(0, policy.Depth); Assert.Equal(0, policy.MappingHits);
    }

    [Fact]
    public void Other_threads_never_change_registration_depth()
    {
        var policy = Policy(); policy.Activate(); bool outer = policy.Enter(Thread, true);
        bool other = policy.Enter(8, false); policy.Exit(8, other);
        Assert.False(other); Assert.Equal(1, policy.Depth); Assert.False(policy.Violated);
        Assert.Equal(Qualified, policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName)); policy.Exit(Thread, outer);
    }

    [Fact]
    public void Closed_scope_cannot_be_reused_or_rewrite()
    {
        var policy = Policy(); policy.Activate(); policy.Deactivate();
        Assert.Throws<InvalidOperationException>(policy.Activate);
        Assert.False(policy.Enter(Thread, true));
        Assert.Equal(SubtypeTypeNamePolicy.NativeName, policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName));
    }

    [Fact]
    public void An_existing_valid_prefixed_fallback_needs_no_mapping_hit()
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Il2CppFishNet.Runtime"), AssemblyBuilderAccess.Run);
        var proxy = assembly.DefineDynamicModule("proxy").DefineType("Il2CppFishNet.Connection.NetworkConnection", TypeAttributes.Public).CreateType()!;
        Type? Resolve(string name) => Type.GetType(name,
            requested => requested.Name is "FishNet.Runtime" or "Il2CppFishNet.Runtime" ? assembly : null,
            (found, type, ignoreCase) => found?.GetType(type, false, ignoreCase), false);
        Assert.Null(Resolve(SubtypeTypeNamePolicy.NativeName));
        Assert.Equal(proxy, Resolve("Il2Cpp" + SubtypeTypeNamePolicy.NativeName));
        var policy = new SubtypeTypeNamePolicy(Thread, NativeClass, proxy.AssemblyQualifiedName!, true);
        var lease = new SubtypeTypeResolutionLease();
        lease.Run(policy, new Port(), () =>
        {
            bool entered = policy.Enter(Thread, true);
            Assert.Equal(SubtypeTypeNamePolicy.NativeName, policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName));
            policy.Exit(Thread, entered);
        });
        Assert.Equal(0, lease.Outcome!.MappingHits); Assert.True(lease.Outcome.Restored);
    }

    [Fact]
    public void Foreign_reentrant_registration_leaves_names_unchanged_and_fails_the_lease()
    {
        var policy = Policy(); var port = new Port(); var lease = new SubtypeTypeResolutionLease();
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () =>
        {
            bool own = policy.Enter(Thread, true); bool foreign = policy.Enter(Thread, false);
            Assert.Equal(SubtypeTypeNamePolicy.NativeName, policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName));
            policy.Exit(Thread, foreign); policy.Exit(Thread, own);
        }));
        Assert.True(lease.Outcome!.ScopeViolated); Assert.True(lease.Outcome.Restored); Assert.False(policy.Active);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("read-fails")]
    public void Own_patch_presence_readback_failure_retains_an_outcome(string failure)
    {
        var policy = Policy(); var port = new Port { PresenceThrows = failure == "read-fails", Own = failure == "missing" };
        var lease = new SubtypeTypeResolutionLease();
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () => throw new Exception("must-not-run")));
        Assert.NotNull(lease.Outcome); Assert.False(lease.Outcome!.Restored); Assert.False(policy.Active);
        Assert.Equal(0, port.InstallCalls); Assert.Equal(0, port.RemoveCalls);
    }

    [Fact]
    public void Successful_lease_measures_real_hits_and_restores_frozen_foreign_snapshot()
    {
        var policy = Policy(); var port = new Port(); var lease = new SubtypeTypeResolutionLease();
        lease.Run(policy, port, () => Register(policy));
        Assert.True(lease.Outcome!.Installed && lease.Outcome.Restored); Assert.False(lease.Outcome.ScopeActive);
        Assert.Equal(1, lease.Outcome.MappingHits); Assert.Empty(lease.Outcome.Findings);
        port.Foreign[0] = "later"; Assert.Equal("foreign-owner|method|priority|before|after", lease.Outcome.Before![0]);
        Assert.Equal(lease.Outcome.Before, lease.Outcome.After); Assert.Equal(1, port.RemoveCalls);
    }

    [Theory]
    [InlineData("install")]
    [InlineData("install-readback")]
    [InlineData("registration")]
    [InlineData("during-snapshot")]
    [InlineData("after-snapshot")]
    public void Partial_failures_remove_own_hooks_without_repeating_registration(string phase)
    {
        var policy = Policy(); var port = new Port { ThrowAt = phase }; var lease = new SubtypeTypeResolutionLease(); int calls = 0;
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () =>
        {
            calls++; Register(policy, phase == "registration");
        }));
        Assert.False(policy.Active); Assert.False(port.Own); Assert.Equal(1, port.RemoveCalls);
        Assert.True(calls <= 1); Assert.NotEmpty(lease.Outcome!.Findings);
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () => calls++));
        Assert.Equal(1, port.RemoveCalls);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Removal_exceptions_are_bounded_and_never_turn_attempt_into_success(int failures)
    {
        var policy = Policy(); var port = new Port { RemoveFailures = failures }; var lease = new SubtypeTypeResolutionLease();
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () => Register(policy)));
        Assert.Equal(2, port.RemoveCalls); Assert.False(policy.Active);
        Assert.Equal(failures == 1, lease.Outcome!.Restored); Assert.NotEmpty(lease.Outcome.Findings);
        Assert.Equal(SubtypeTypeNamePolicy.NativeName, policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName));
    }

    [Fact]
    public void Unremoved_hook_is_inert_and_explicitly_unrestored()
    {
        var policy = Policy(); var port = new Port { KeepOwn = true }; var lease = new SubtypeTypeResolutionLease();
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () => Register(policy)));
        Assert.True(port.Own); Assert.False(policy.Active); Assert.False(lease.Outcome!.Restored); Assert.Equal(2, port.RemoveCalls);
    }

    [Theory]
    [InlineData("before-snapshot")]
    [InlineData("null-snapshot")]
    [InlineData("preexisting")]
    public void Denied_patch_baseline_never_touches_owners_or_registration(string reason)
    {
        var policy = Policy(); var port = new Port { ThrowAt = reason, Own = reason == "preexisting" }; var lease = new SubtypeTypeResolutionLease();
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () => throw new Exception("must-not-run")));
        Assert.Equal(0, port.InstallCalls); Assert.Equal(0, port.RemoveCalls); Assert.False(policy.Active);
    }

    [Fact]
    public void Foreign_patch_change_blocks_registration_and_is_not_rolled_back()
    {
        var policy = Policy(); var port = new Port { ChangeForeign = true }; var lease = new SubtypeTypeResolutionLease();
        Assert.Throws<InvalidOperationException>(() => lease.Run(policy, port, () => throw new Exception("must-not-run")));
        Assert.Equal("foreign-changed", port.Foreign[0]); Assert.False(lease.Outcome!.Restored); Assert.False(port.Own);
    }

    [Fact]
    public void Missing_mapping_hit_or_missing_finalizer_cannot_be_an_observation()
    {
        foreach (bool missingExit in new[] { false, true })
        {
            var policy = Policy(); var lease = new SubtypeTypeResolutionLease();
            Assert.Throws<InvalidOperationException>(() => lease.Run(policy, new Port(), () =>
            {
                bool entered = policy.Enter(Thread, true);
                if (!missingExit) policy.Exit(Thread, entered);
            }));
            Assert.False(policy.Active); Assert.True(lease.Outcome!.Restored);
        }
    }

    private static void Register(SubtypeTypeNamePolicy policy, bool fail = false)
    {
        bool entered = policy.Enter(Thread, true);
        try
        {
            Assert.Equal(Qualified, policy.Rewrite(Thread, NativeClass, SubtypeTypeNamePolicy.NativeName));
            if (fail) throw new NullReferenceException("registration-partial-failure");
        }
        finally { policy.Exit(Thread, entered); }
    }
    private sealed class Port : ITypeResolutionPatchPort
    {
        public readonly List<string> Foreign = new() { "foreign-owner|method|priority|before|after" };
        public string ThrowAt = "";
        public bool Own, KeepOwn, ChangeForeign, PresenceThrows;
        public int InstallCalls, RemoveCalls, RemoveFailures;
        private int _captures;
        public IReadOnlyList<string> CaptureForeignPatches()
        {
            _captures++;
            if ((_captures == 1 && ThrowAt == "before-snapshot") || (_captures == 2 && ThrowAt == "during-snapshot") ||
                (_captures == 3 && ThrowAt == "after-snapshot")) throw new IOException("snapshot-unreadable");
            return ThrowAt == "null-snapshot" ? null! : Foreign;
        }
        public void Install()
        {
            InstallCalls++; Own = true;
            if (ChangeForeign) Foreign[0] = "foreign-changed";
            if (ThrowAt == "install") throw new InvalidOperationException("partially-installed");
        }
        public void Remove()
        {
            RemoveCalls++;
            if (RemoveCalls <= RemoveFailures) throw new IOException("remove-failed");
            if (!KeepOwn) Own = false;
        }
        public bool AllOwnPatchesPresent => ThrowAt != "install-readback" && Own;
        public bool AnyOwnPatchPresent => PresenceThrows ? throw new IOException("patch-presence-unreadable") : Own;
    }
}

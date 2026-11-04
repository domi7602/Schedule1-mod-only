using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessInterfaceTests
{
    private static ConstructionContext Context => new(true, true, true, true,
        Path.Combine(Path.GetTempPath(), "TownHallTests", "SaveGame_2"),
        Path.Combine(Path.GetTempPath(), "TownHallTests", "SaveGame_2"), "TownHallTest");

    private static InterfaceDispatchTarget Target => new(
        new(100, 200, "InitializeSaveable", 10, 0, "System.Void", true, true, false, 0x06001782),
        200, true, 23, 1, 300, 300, true, 1, 1, 33, 33, 64, 64, 400, 400, 500, 600,
        new(500, 300, "InitializeSaveable", ushort.MaxValue, 0, "System.Void", true, false, false), 600, 500, 600, true, true);

    [Fact]
    public void One_interface_call_has_separate_stages_and_immutable_snapshots()
    {
        var port = new Port();
        var trial = new BusinessSubtypeTrial();
        var result = trial.Run(Context, port, probeInterface: true);
        Assert.Equal("observed", result.Status);
        Assert.True(result.CleanupComplete);
        Assert.Equal(1, port.InterfaceCalls);
        Assert.Equal(0, result.Interface!.Before!.State.InitializeSaveableCalls);
        Assert.Equal(1, result.Interface.After!.State.InitializeSaveableCalls);
        Assert.Equal(1, result.Interface.After.SaveCalls);
        Assert.Equal(1, result.Interface.After.PurchaseCalls);
        Assert.True(result.Interface.InvocationAttempted);
        port.Registries["properties"] = "later";
        Assert.Equal("initial", result.Interface.BeforeSnapshot!.Registries["properties"]);
        Assert.Equal("initial", result.Interface.AfterSnapshot!.Registries["properties"]);
        int operations = port.Events.Count;
        var skipped = trial.Run(Context, port, probeInterface: true);
        Assert.Equal("skipped", skipped.Status);
        Assert.Null(skipped.Interface);
        Assert.False(skipped.CleanupComplete);
        Assert.Equal(operations, port.Events.Count);
    }

    [Fact]
    public void Injected_method_without_virtual_flag_is_accepted_when_pointer_matches()
    {
        var port = new Port
        {
            Mapping = Target with
            {
                Resolved = Target.Resolved with { Virtual = false, Abstract = false, Static = false, Slot = ushort.MaxValue, Token = 0 }
            }
        };
        var result = new BusinessSubtypeTrial().Run(Context, port, probeInterface: true);
        Assert.Equal("observed", result.Status);
        Assert.Equal(1, port.InterfaceCalls);
        Assert.False(result.Interface!.Before!.Target.Resolved.Virtual);
        Assert.Equal(ushort.MaxValue, result.Interface.Before.Target.Resolved.Slot);
    }

    [Fact]
    public void Original_subtype_mode_does_not_touch_interface_and_requires_zero_callbacks()
    {
        var port = new Port();
        Assert.Equal("observed", new BusinessSubtypeTrial().Run(Context, port).Status);
        Assert.DoesNotContain("interface-inspect", port.Events);
        Assert.Equal(0, port.InterfaceCalls);
        port = new Port { InitializerCalls = 1 };
        Assert.Equal("failed", new BusinessSubtypeTrial().Run(Context, port).Status);
        Assert.Equal(0, port.BoolCalls);
    }

    [Theory]
    [InlineData("method-null", "interface-method-missing")]
    [InlineData("interface-null", "interface-class-zero")]
    [InlineData("interface-class", "interface-method-owner")]
    [InlineData("not-interface", "interface-flag")]
    [InlineData("name", "interface-method-name")]
    [InlineData("token", "interface-method-token")]
    [InlineData("parameters", "interface-method-parameters")]
    [InlineData("return", "interface-method-return")]
    [InlineData("virtual", "interface-method-virtual")]
    [InlineData("abstract", "interface-method-abstract")]
    [InlineData("static", "interface-method-static")]
    [InlineData("method-count", "interface-method-count")]
    [InlineData("method-duplicate", "interface-method-matches")]
    [InlineData("slot-negative", "interface-method-slot")]
    [InlineData("slot-sentinel", "interface-method-slot")]
    [InlineData("slot-bound", "interface-method-slot")]
    [InlineData("own-null", "interface-own-class-zero")]
    [InlineData("object-class", "interface-object-class")]
    [InlineData("interfaces", "interface-inherited-interfaces")]
    [InlineData("base-offset-missing", "interface-offset-matches")]
    [InlineData("own-offset-duplicate", "interface-offset-matches")]
    [InlineData("offset-negative", "interface-offset")]
    [InlineData("offset-mismatch", "interface-offset")]
    [InlineData("offset-overflow", "interface-vtable-slot")]
    [InlineData("base-vtable-bound", "interface-vtable-slot")]
    [InlineData("own-vtable-bound", "interface-vtable-slot")]
    [InlineData("base-target", "interface-base-target")]
    [InlineData("own-target", "interface-own-target")]
    [InlineData("own-pointer", "interface-own-pointer")]
    [InlineData("resolved-method", "interface-resolved-method")]
    [InlineData("resolved-class", "interface-resolved-class")]
    [InlineData("resolved-parameters", "interface-resolved-signature")]
    [InlineData("resolved-return", "interface-resolved-return")]
    [InlineData("resolved-abstract", "interface-resolved-flags")]
    [InlineData("resolved-pointer", "interface-resolved-pointer")]
    [InlineData("expected-null", "interface-expected-own-pointer")]
    [InlineData("base-not-assignable", "interface-not-assignable")]
    [InlineData("own-not-assignable", "interface-not-assignable")]
    public void Invalid_interface_mapping_fails_before_any_interface_call(string fault, string finding)
    {
        var t = Target;
        t = fault switch
        {
            "method-null" => t with { Method = null! },
            "interface-null" => t with { InterfaceClass = 0 },
            "interface-class" => t with { Method = t.Method with { DeclaringClass = 9 } },
            "not-interface" => t with { IsInterface = false },
            "name" => t with { Method = t.Method with { Name = "ShouldSave" } },
            "token" => t with { Method = t.Method with { Token = 0x06001420 } },
            "parameters" => t with { Method = t.Method with { ParameterCount = 1 } },
            "return" => t with { Method = t.Method with { ReturnType = "System.Boolean" } },
            "virtual" => t with { Method = t.Method with { Virtual = false } },
            "abstract" => t with { Method = t.Method with { Abstract = false } },
            "static" => t with { Method = t.Method with { Static = true } },
            "method-count" => t with { InterfaceMethodCount = 0 },
            "method-duplicate" => t with { InterfaceMethodMatches = 2 },
            "slot-negative" => t with { Method = t.Method with { Slot = -1 } },
            "slot-sentinel" => t with { Method = t.Method with { Slot = ushort.MaxValue } },
            "slot-bound" => t with { Method = t.Method with { Slot = 23 } },
            "own-null" => t with { OwnClass = 0, ObjectClass = 0 },
            "object-class" => t with { ObjectClass = 9 },
            "interfaces" => t with { InheritedInterfacesPreserved = false },
            "base-offset-missing" => t with { BaseOffsetMatches = 0 },
            "own-offset-duplicate" => t with { OwnOffsetMatches = 2 },
            "offset-negative" => t with { BaseOffset = -1, OwnOffset = -1 },
            "offset-mismatch" => t with { OwnOffset = 34 },
            "offset-overflow" => t with { BaseOffset = int.MaxValue, OwnOffset = int.MaxValue },
            "base-vtable-bound" => t with { BaseVtableCount = 43 },
            "own-vtable-bound" => t with { OwnVtableCount = 43 },
            "base-target" => t with { BaseSlotMethod = 9 },
            "own-target" => t with { OwnSlotMethod = 9 },
            "own-pointer" => t with { OwnSlotPointer = 9 },
            "resolved-method" => t with { Resolved = t.Resolved with { Method = 9 } },
            "resolved-class" => t with { Resolved = t.Resolved with { DeclaringClass = 9 } },
            "resolved-parameters" => t with { Resolved = t.Resolved with { ParameterCount = 1 } },
            "resolved-return" => t with { Resolved = t.Resolved with { ReturnType = "System.Boolean" } },
            "resolved-abstract" => t with { Resolved = t.Resolved with { Abstract = true } },
            "resolved-pointer" => t with { ResolvedPointer = 9 },
            "base-not-assignable" => t with { BaseAssignable = false },
            "own-not-assignable" => t with { OwnAssignable = false },
            _ => t with { ExpectedOwnPointer = 0 }
        };
        var port = new Port { Mapping = t };
        var result = new BusinessSubtypeTrial().Run(Context, port, probeInterface: true);
        Assert.Equal("failed", result.Status);
        Assert.Contains(finding, result.Findings[0]);
        Assert.NotNull(result.Interface!.Before);
        Assert.Equal(t, result.Interface.Before.Target);
        Assert.False(result.Interface.InvocationAttempted);
        Assert.Equal(0, port.InterfaceCalls);
        Assert.Equal(1, port.Cleanups);
    }

    [Theory]
    [InlineData(0, 1, 1, 0)]
    [InlineData(2, 1, 1, 0)]
    [InlineData(1, 2, 1, 0)]
    [InlineData(1, 1, 2, 0)]
    [InlineData(1, 1, 1, 1)]
    public void Callback_delta_and_property_counters_are_checked_after_the_single_marked_call(int initializers, int saves, int purchases, int lifecycle)
    {
        var port = new Port { AddedInitializers = initializers, SavesAfter = saves, PurchasesAfter = purchases, LifecycleAfter = lifecycle };
        var result = new BusinessSubtypeTrial().Run(Context, port, probeInterface: true);
        Assert.Equal("failed", result.Status);
        Assert.Equal(1, port.InterfaceCalls);
        Assert.True(result.CleanupComplete);
    }

    [Theory]
    [InlineData("early-init")]
    [InlineData("preflight-init")]
    [InlineData("null-preflight")]
    [InlineData("null-after")]
    [InlineData("changed-target")]
    [InlineData("invoke-throws")]
    [InlineData("cleanup-callback")]
    [InlineData("unclean")]
    public void Failures_preserve_attempt_and_cleanup_bound(string fault)
    {
        var trial = new BusinessSubtypeTrial();
        var port = new Port();
        if (fault == "early-init") port.InitializerCalls = 1;
        if (fault == "preflight-init") port.OnInterfaceInspect = n => { if (n == 1) port.InitializerCalls = 1; };
        if (fault == "null-preflight") port.NullInterface = 1;
        if (fault == "null-after") port.NullInterface = 2;
        if (fault == "changed-target") port.OnInterfaceInspect = n => { if (n == 2) port.Mapping = port.Mapping with { Method = port.Mapping.Method with { Method = 101 } }; };
        if (fault == "invoke-throws") port.ThrowInvoke = true;
        if (fault == "cleanup-callback") port.CleanupThrows = true;
        if (fault == "unclean") port.Clean = false;
        var result = trial.Run(Context, port, probeInterface: true);
        Assert.Equal("failed", result.Status);
        Assert.True(result.AttemptConsumed);
        Assert.Equal(fault is "cleanup-callback" or "unclean" ? 2 : 1, port.Cleanups);
        int operations = port.Events.Count;
        Assert.Equal("skipped", trial.Run(Context, port, probeInterface: true).Status);
        Assert.Equal(operations, port.Events.Count);
    }

    [Theory]
    [InlineData(5, false)]
    [InlineData(5, true)]
    [InlineData(6, false)]
    [InlineData(6, true)]
    public void Interface_stage_foreign_change_is_detected_without_rollback(int snapshot, bool vtable)
    {
        var port = new Port { ChangeCapture = snapshot, ChangeVtable = vtable };
        var result = new BusinessSubtypeTrial().Run(Context, port, probeInterface: true);
        Assert.Equal("failed", result.Status);
        Assert.Equal(snapshot == 5 ? 0 : 1, port.InterfaceCalls);
        Assert.Equal("changed", vtable ? port.Vtables["ISaveable"] : port.Registries["properties"]);
    }

    [Fact]
    public void Context_expiry_after_preflight_prevents_invocation()
    {
        bool valid = true;
        var port = new Port { OnInterfaceInspect = _ => valid = false };
        Assert.Equal("failed", new BusinessSubtypeTrial().Run(Context, port, () => valid, true).Status);
        Assert.Equal(0, port.InterfaceCalls);
        Assert.Equal(1, port.Cleanups);
    }

    [Fact]
    public void Denied_context_does_not_touch_port_or_consume_interface_attempt()
    {
        var trial = new BusinessSubtypeTrial();
        var port = new Port();
        Assert.Equal("skipped", trial.Run(Context with { OrganisationName = "Other" }, port, probeInterface: true).Status);
        Assert.Empty(port.Events);
        Assert.Equal("observed", trial.Run(Context, port, probeInterface: true).Status);
    }

    private sealed class Port : IBusinessSubtypePort, IBusinessInterfacePort
    {
        public readonly List<string> Events = new();
        public readonly Dictionary<string, string> Registries = new[] { "properties", "properties-owned", "properties-unowned", "businesses", "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests" }.ToDictionary(k => k, _ => "initial");
        public readonly Dictionary<string, string> Vtables = new() { ["ISaveable"] = "initial" };
        public InterfaceDispatchTarget Mapping = Target;
        public int InterfaceCalls, BoolCalls, InitializerCalls, Cleanups, NullInterface, ChangeCapture;
        public int AddedInitializers = 1, SavesAfter = 1, PurchasesAfter = 1, LifecycleAfter;
        public bool ThrowInvoke, CleanupThrows, ChangeVtable, Clean = true;
        public Action<int>? OnInterfaceInspect;
        private int _captures, _interfaceReads;
        public SubtypeSnapshot Capture()
        {
            Events.Add("capture");
            if (++_captures == ChangeCapture) (ChangeVtable ? Vtables : Registries)[ChangeVtable ? "ISaveable" : "properties"] = "changed";
            return new(Registries, Vtables);
        }
        public void RegisterType() => Events.Add("register");
        public void Prepare() => Events.Add("prepare");
        public SubtypeReadback Inspect()
        {
            Events.Add("inspect");
            return new(true, true, true, true, true, InterfaceCalls == 0 ? 0 : LifecycleAfter, InitializerCalls);
        }
        public SubtypeDispatch InvokeThroughNativeBase() { Events.Add("bools"); BoolCalls++; return new(false, false, 1, 1); }
        public InterfaceReadback InspectInterface()
        {
            Events.Add("interface-inspect");
            _interfaceReads++;
            OnInterfaceInspect?.Invoke(_interfaceReads);
            if (_interfaceReads == NullInterface) return null!;
            return new(Mapping, Inspect(), InterfaceCalls == 0 ? 1 : SavesAfter, InterfaceCalls == 0 ? 1 : PurchasesAfter);
        }
        public void InvokeThroughNativeInterface()
        {
            Events.Add("interface-invoke"); InterfaceCalls++; InitializerCalls += AddedInitializers;
            if (ThrowInvoke) throw new InvalidOperationException("callback-then-throw");
        }
        public void Cleanup() { Events.Add("cleanup"); Cleanups++; if (CleanupThrows) throw new InvalidOperationException("late-extra-callback"); }
        public bool IsClean => Clean;
    }
}

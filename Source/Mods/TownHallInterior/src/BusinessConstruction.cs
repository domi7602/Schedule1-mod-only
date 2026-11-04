#if TOWNHALL_BUSINESS_ACTIVATION
#error TownHall activation probe withdrawn after native shared-code detour crash. See docs/business-activation-probe.md. Use the verified inactive construction candidate until isolated instance dispatch is proven.
#endif
#if TOWNHALL_BUSINESS_CONSTRUCTION || TOWNHALL_BUSINESS_SUBTYPE
extern alias il2cpp;
#endif
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace TownHallInterior;

public sealed record SubtypeSnapshot(IReadOnlyDictionary<string, string> Registries,
    IReadOnlyDictionary<string, string> ForeignVtables);
public sealed record SubtypeReadback(bool Inactive, bool ExactComponents, bool Unowned,
    bool OwnNativeClass, bool OwnVirtualTargets, int UnexpectedLifecycleCalls, int InitializeSaveableCalls = 0);
public sealed record SubtypeDispatch(bool ShouldSave, bool CanBePurchased, int SaveCalls, int PurchaseCalls);
public sealed record InterfaceMethodEvidence(long Method, long DeclaringClass, string Name, int Slot,
    int ParameterCount, string ReturnType, bool Virtual, bool Abstract, bool Static, uint Token = 0);
public sealed record InterfaceDispatchTarget(InterfaceMethodEvidence Method, long InterfaceClass, bool IsInterface,
    int InterfaceMethodCount, int InterfaceMethodMatches, long OwnClass, long ObjectClass, bool InheritedInterfacesPreserved,
    int BaseOffsetMatches, int OwnOffsetMatches, int BaseOffset, int OwnOffset, int BaseVtableCount, int OwnVtableCount,
    long BaseSlotMethod, long ExpectedBaseMethod, long OwnSlotMethod, long OwnSlotPointer,
    InterfaceMethodEvidence Resolved, long ResolvedPointer, long ExpectedOwnMethod, long ExpectedOwnPointer,
    bool BaseAssignable = false, bool OwnAssignable = false);
public sealed record InterfaceReadback(InterfaceDispatchTarget Target, SubtypeReadback State, int SaveCalls, int PurchaseCalls);
public interface IBusinessInterfacePort
{
    InterfaceReadback InspectInterface();
    void InvokeThroughNativeInterface();
}
public sealed record InterfaceProbeResult(InterfaceReadback? Before, InterfaceReadback? After,
    SubtypeSnapshot? BeforeSnapshot, SubtypeSnapshot? AfterSnapshot, bool InvocationAttempted);
public static class BusinessInterfaceGate
{
    public static void Require(InterfaceReadback? value, int expectedCalls)
    {
        string? mismatch = Mismatch(value, expectedCalls);
        if (mismatch != null) throw new InvalidOperationException(mismatch);
    }
    public static void RequireMetadata(InterfaceReadback? value, int expectedCalls)
    {
        string? mismatch = MetadataMismatch(value, expectedCalls);
        if (mismatch != null) throw new InvalidOperationException(mismatch);
    }
    public static string? Mismatch(InterfaceReadback? value, int expectedCalls)
    {
        string? metadata = MetadataMismatch(value, expectedCalls);
        if (metadata != null) return metadata;
        return value!.Target.BaseAssignable && value.Target.OwnAssignable ? null : "interface-not-assignable";
    }
    // Names the first rejected field. Assignability is intentionally excluded: callers set those flags only after metadata matches.
    public static string? MetadataMismatch(InterfaceReadback? value, int expectedCalls)
    {
        if (value?.Target is not { } t) return "interface-readback-missing";
        if (t.Method is not { } m) return "interface-method-missing";
        if (t.Resolved is not { } r) return "interface-resolved-missing";
        if (value.State is not { } s) return "interface-state-missing";
        if (t.InterfaceClass == 0) return "interface-class-zero";
        if (!t.IsInterface) return "interface-flag";
        if (m.Method == 0) return "interface-method-zero";
        if (m.DeclaringClass != t.InterfaceClass) return "interface-method-owner";
        if (m.Name != "InitializeSaveable") return "interface-method-name";
        if (m.Token != 0x06001782) return "interface-method-token";
        if (m.ParameterCount != 0) return "interface-method-parameters";
        if (m.ReturnType != "System.Void") return "interface-method-return";
        if (!m.Virtual) return "interface-method-virtual";
        if (!m.Abstract) return "interface-method-abstract";
        if (m.Static) return "interface-method-static";
        if (t.InterfaceMethodCount != 23) return "interface-method-count";
        if (t.InterfaceMethodMatches != 1) return "interface-method-matches";
        if (m.Slot < 0 || m.Slot >= t.InterfaceMethodCount) return "interface-method-slot";
        if (t.OwnClass == 0) return "interface-own-class-zero";
        if (t.ObjectClass != t.OwnClass) return "interface-object-class";
        if (!t.InheritedInterfacesPreserved) return "interface-inherited-interfaces";
        if (t.BaseOffsetMatches != 1 || t.OwnOffsetMatches != 1) return "interface-offset-matches";
        if (t.BaseOffset < 0 || t.OwnOffset != t.BaseOffset) return "interface-offset";
        if (t.BaseVtableCount is <= 0 or > 4096 || t.OwnVtableCount is <= 0 or > 4096) return "interface-vtable-count";
        if ((long)t.OwnOffset + m.Slot >= t.OwnVtableCount || (long)t.BaseOffset + m.Slot >= t.BaseVtableCount) return "interface-vtable-slot";
        if (t.ExpectedBaseMethod == 0 || t.BaseSlotMethod != t.ExpectedBaseMethod) return "interface-base-target";
        if (t.ExpectedOwnMethod == 0 || t.OwnSlotMethod != t.ExpectedOwnMethod) return "interface-own-target";
        if (t.ExpectedOwnPointer == 0) return "interface-expected-own-pointer";
        if (t.OwnSlotPointer != t.ExpectedOwnPointer) return "interface-own-pointer";
        if (r.Method != t.ExpectedOwnMethod) return "interface-resolved-method";
        if (r.DeclaringClass != t.OwnClass) return "interface-resolved-class";
        if (r.Name != "InitializeSaveable" || r.ParameterCount != 0) return "interface-resolved-signature";
        if (r.ReturnType != "System.Void") return "interface-resolved-return";
        // Injected MethodInfo is public+hide-by-sig, slot 65535 and token 0. It is not flagged virtual;
        // the copied vtable slot and matching method pointer are the dispatch proof.
        if (r.Abstract || r.Static) return "interface-resolved-flags";
        if (t.ResolvedPointer != t.ExpectedOwnPointer) return "interface-resolved-pointer";
        if (!s.Inactive || !s.ExactComponents || !s.Unowned || !s.OwnNativeClass || !s.OwnVirtualTargets) return "interface-state";
        if (s.UnexpectedLifecycleCalls != 0) return "interface-unexpected-lifecycle";
        if (s.InitializeSaveableCalls != expectedCalls) return "interface-init-count";
        if (value.SaveCalls != 1 || value.PurchaseCalls != 1) return "interface-property-counters";
        return null;
    }
}
public interface IBusinessSubtypePort
{
    SubtypeSnapshot Capture();
    void RegisterType();
    void Prepare();
    SubtypeReadback Inspect();
    SubtypeDispatch InvokeThroughNativeBase();
    void Cleanup();
    bool IsClean { get; }
}
public sealed record ManagerRegistration(int SaveablesBefore, int Added, int Remaining, bool Removed, bool SaveCalled,
    int WriteCalls = 0, int DeleteCalls = 0, int GetSaveStringCalls = 0, bool SavingAfter = false);
public interface IBusinessManagerPort
{
    ManagerRegistration RegisterOnce();
    ManagerRegistration SaveRegistered();
}
public sealed record SubtypeResult(string Status, string[] Findings, bool AttemptConsumed,
    bool RegistrationAttempted, bool RegistrationCompleted, bool CleanupComplete,
    SubtypeSnapshot? Before, SubtypeSnapshot? Registered, SubtypeSnapshot? Constructed,
    SubtypeSnapshot? Dispatched, SubtypeSnapshot? After, SubtypeReadback? Readback, SubtypeDispatch? Dispatch,
    InterfaceProbeResult? Interface = null, ManagerRegistration? Manager = null);

// Exactly one process-lifetime instance. Type registration cannot be undone, including
// partial failure; cleanup confirms only ownership of the transient Unity object.
public sealed class BusinessSubtypeTrial
{
    private static readonly string[] RegistryNames = { "properties", "properties-owned", "properties-unowned",
        "businesses", "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests" };
    private int _attempted;
    public bool AttemptConsumed => Volatile.Read(ref _attempted) != 0;
    public SubtypeResult Run(ConstructionContext context, IBusinessSubtypePort port, Func<bool>? contextStillValid = null, bool probeInterface = false, bool probeManager = false, string organisation = "TownHallTest", string slot = "SaveGame_2", bool probeSave = false)
    {
        ArgumentNullException.ThrowIfNull(port);
        if ((probeManager || probeSave) && probeInterface) throw new InvalidOperationException("manager-and-interface");
        if (probeSave && probeManager) throw new InvalidOperationException("save-and-register");
        string? denial = ConstructionGate.Check(context, organisation, slot);
        if (denial != null || Interlocked.CompareExchange(ref _attempted, 1, 0) != 0)
            return new("skipped", new[] { denial ?? "attempt-already-consumed" }, AttemptConsumed,
                false, false, false, null, null, null, null, null, null, null);
        var findings = new List<string>();
        SubtypeSnapshot? before = null, registered = null, constructed = null, dispatched = null, after = null;
        SubtypeReadback? readback = null;
        SubtypeDispatch? dispatch = null;
        InterfaceReadback? interfaceBefore = null, interfaceAfter = null;
        SubtypeSnapshot? interfaceBeforeSnapshot = null, interfaceAfterSnapshot = null;
        bool interfaceAttempted = false;
        ManagerRegistration? manager = null;
        bool registrationAttempted = false, registrationCompleted = false, clean = false;
        string phase = "baseline";
        void Fail(string value) { if (findings.Count < 16) findings.Add(value); }
        void RequireContext() { if (contextStillValid?.Invoke() == false) throw new InvalidOperationException("context-expired"); }
        void RequireUnchanged(SubtypeSnapshot snapshot)
        {
            if (before == null || !Equal(before.Registries, snapshot.Registries) ||
                !Equal(before.ForeignVtables, snapshot.ForeignVtables))
                throw new InvalidOperationException("foreign-state-changed");
        }
        try
        {
            before = Freeze(port.Capture());
            phase = "register-type"; RequireContext(); registrationAttempted = true;
            port.RegisterType(); registrationCompleted = true;
            phase = "registered-readback"; registered = Freeze(port.Capture()); RequireUnchanged(registered);
            phase = "prepare"; RequireContext(); port.Prepare();
            phase = "inspect"; readback = port.Inspect();
            if (readback == null || !readback.Inactive || !readback.ExactComponents || !readback.Unowned ||
                !readback.OwnNativeClass || !readback.OwnVirtualTargets || readback.UnexpectedLifecycleCalls != 0 || readback.InitializeSaveableCalls != 0)
                throw new InvalidOperationException("subtype-readback-invalid");
            phase = "constructed-readback"; constructed = Freeze(port.Capture()); RequireUnchanged(constructed);
            phase = "native-dispatch"; RequireContext(); dispatch = port.InvokeThroughNativeBase();
            if (dispatch == null || dispatch.ShouldSave || dispatch.CanBePurchased || dispatch.SaveCalls != 1 || dispatch.PurchaseCalls != 1)
                throw new InvalidOperationException("native-dispatch-invalid");
            phase = "dispatched-inspect"; var finalInspection = port.Inspect();
            if (finalInspection != readback) throw new InvalidOperationException("subtype-changed-during-dispatch");
            phase = "dispatched-readback"; dispatched = Freeze(port.Capture()); RequireUnchanged(dispatched);
            if (probeSave || probeManager)
            {
                phase = probeSave ? "manager-save" : "manager-register"; RequireContext();
                var nativeManager = port as IBusinessManagerPort ?? throw new InvalidOperationException("manager-port-missing");
                manager = probeSave ? nativeManager.SaveRegistered() : nativeManager.RegisterOnce();
                if (manager == null || manager.SaveCalled != probeSave || manager.SavingAfter || manager.Added != 1 || manager.Remaining != 0 || !manager.Removed)
                    throw new InvalidOperationException(probeSave ? "manager-save-not-settled" : "manager-registration-not-reversed");
                phase = "manager-readback"; var afterManager = Freeze(port.Capture()); RequireUnchanged(afterManager);
            }
            if (probeInterface)
            {
                phase = "interface-preflight"; RequireContext();
                var nativeInterface = port as IBusinessInterfacePort ?? throw new InvalidOperationException("interface-port-missing");
                interfaceBefore = nativeInterface.InspectInterface(); BusinessInterfaceGate.Require(interfaceBefore, 0);
                if (interfaceBefore.State != readback) throw new InvalidOperationException("interface-preflight-state-changed");
                interfaceBeforeSnapshot = Freeze(port.Capture()); RequireUnchanged(interfaceBeforeSnapshot);
                phase = "interface-dispatch"; RequireContext(); interfaceAttempted = true;
                nativeInterface.InvokeThroughNativeInterface();
                phase = "interface-readback"; RequireContext(); interfaceAfter = nativeInterface.InspectInterface();
                BusinessInterfaceGate.Require(interfaceAfter, 1);
                if (interfaceAfter.Target != interfaceBefore.Target || interfaceAfter.State != (readback with { InitializeSaveableCalls = 1 }))
                    throw new InvalidOperationException("interface-target-or-state-changed");
                interfaceAfterSnapshot = Freeze(port.Capture()); RequireUnchanged(interfaceAfterSnapshot);
            }
        }
        catch (Exception ex) { Fail(phase + ":" + Describe(ex)); }
        finally
        {
            if (registrationAttempted)
            {
                for (int attempt = 1; attempt <= 2 && !clean; attempt++)
                {
                    try { port.Cleanup(); clean = port.IsClean; if (!clean) Fail("cleanup-incomplete:" + attempt); }
                    catch (Exception ex) { Fail("cleanup:" + Describe(ex)); }
                }
            }
            else clean = true;
            try { after = Freeze(port.Capture()); RequireUnchanged(after); }
            catch (Exception ex) { Fail("after-readback:" + Describe(ex)); }
        }
        return new(findings.Count == 0 && clean ? "observed" : "failed", findings.ToArray(), true,
            registrationAttempted, registrationCompleted, clean, before, registered, constructed,
            dispatched, after, readback, dispatch, probeInterface ? new(interfaceBefore, interfaceAfter,
                interfaceBeforeSnapshot, interfaceAfterSnapshot, interfaceAttempted) : null, manager);
    }

    private static SubtypeSnapshot Freeze(SubtypeSnapshot source)
    {
        if (source == null || source.Registries == null || !RegistryNames.All(source.Registries.ContainsKey))
            throw new InvalidOperationException("snapshot-missing-required-registries");
        return new(Copy(source.Registries, 10, 10), Copy(source.ForeignVtables, 1, 256));
    }
    private static IReadOnlyDictionary<string, string> Copy(IReadOnlyDictionary<string, string> source, int min, int max)
    {
        if (source == null || source.Count < min || source.Count > max) throw new InvalidOperationException("snapshot-size");
        var copy = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var entry in source)
        {
            if (string.IsNullOrEmpty(entry.Key) || string.IsNullOrEmpty(entry.Value)) throw new InvalidOperationException("snapshot-entry");
            copy.Add(entry.Key, entry.Value);
        }
        return new ReadOnlyDictionary<string, string>(copy);
    }
    private static bool Equal(IReadOnlyDictionary<string, string> left, IReadOnlyDictionary<string, string> right) =>
        left.Count == right.Count && left.All(p => right.TryGetValue(p.Key, out string? value) && value == p.Value);
    private static string Describe(Exception error)
    {
        string message = error.Message.Replace('\r', ' ').Replace('\n', ' ');
        return error.GetType().Name + ":" + message[..Math.Min(message.Length, 160)];
    }
}

// Names are exact, not a general FishNet namespace/assembly fallback.
public sealed class SubtypeTypeNamePolicy
{
    public const string NativeName = "FishNet.Connection.NetworkConnection, FishNet.Runtime";
    public const string ProxyTypeName = "Il2CppFishNet.Connection.NetworkConnection";
    public const string ProxyAssemblyName = "Il2CppFishNet.Runtime";
    private readonly int _thread;
    private readonly long _nativeClass;
    private readonly string _qualifiedProxy;
    private readonly bool _alreadyResolvable;
    private readonly Stack<bool> _registrations = new();
    private bool _closed;
    public bool Active { get; private set; }
    public bool Violated { get; private set; }
    public int MappingHits { get; private set; }
    public int OwnRegistrations { get; private set; }
    public int Depth => _registrations.Count;
    public bool MappingNeeded => !_alreadyResolvable;

    public SubtypeTypeNamePolicy(int thread, long nativeClass, string qualifiedProxy, bool alreadyResolvable)
    {
        string[] name = qualifiedProxy?.Split(',', 2) ?? Array.Empty<string>();
        if (thread <= 0 || nativeClass == 0 || name.Length != 2 || name[0] != ProxyTypeName ||
            new System.Reflection.AssemblyName(name[1].Trim()).Name != ProxyAssemblyName)
            throw new ArgumentException("type-resolution-binding-invalid");
        _thread = thread; _nativeClass = nativeClass; _qualifiedProxy = qualifiedProxy!; _alreadyResolvable = alreadyResolvable;
    }
    public void Activate()
    {
        if (_closed || Active) throw new InvalidOperationException("type-resolution-scope-reused");
        Active = true;
    }
    public bool Enter(int thread, bool ownType)
    {
        if (!Active || thread != _thread) return false;
        if (_registrations.Count >= 16) { Violated = true; throw new InvalidOperationException("registration-depth-over-budget"); }
        if (_registrations.Count != 0 || !ownType) Violated = true;
        _registrations.Push(ownType);
        if (ownType) OwnRegistrations++;
        return true;
    }
    public void Exit(int thread, bool entered)
    {
        if (!entered) return;
        if (thread != _thread || _registrations.Count == 0) { Violated = true; return; }
        _registrations.Pop();
    }
    public string Rewrite(int thread, long nativeClass, string original)
    {
        if (!CanRewrite(thread, original) || nativeClass != _nativeClass) return original;
        if (MappingHits >= 256) { Violated = true; throw new InvalidOperationException("type-resolution-hit-budget"); }
        MappingHits++;
        return _qualifiedProxy;
    }
    public bool CanRewrite(int thread, string original) => Active && !Violated && thread == _thread &&
        _registrations.Count == 1 && _registrations.Peek() && !_alreadyResolvable && original == NativeName;
    public void Deactivate()
    {
        if (_registrations.Count != 0) Violated = true;
        Active = false; _closed = true;
    }
}

public interface ITypeResolutionPatchPort
{
    IReadOnlyList<string> CaptureForeignPatches();
    void Install();
    void Remove();
    bool AllOwnPatchesPresent { get; }
    bool AnyOwnPatchPresent { get; }
}
public sealed record TypeResolutionOutcome(bool InstallAttempted, bool Installed, bool Restored,
    int MappingHits, int OwnRegistrations, int RemainingDepth, bool ScopeViolated, bool ScopeActive,
    IReadOnlyList<string>? Before, IReadOnlyList<string>? After, string[] Findings);

// A failed registration is never repeated; only owner-scoped patch removal is retried.
public sealed class SubtypeTypeResolutionLease
{
    private int _used;
    public TypeResolutionOutcome? Outcome { get; private set; }
    public void Run(SubtypeTypeNamePolicy policy, ITypeResolutionPatchPort port, Action register)
    {
        ArgumentNullException.ThrowIfNull(policy);
        ArgumentNullException.ThrowIfNull(port);
        ArgumentNullException.ThrowIfNull(register);
        if (Interlocked.Exchange(ref _used, 1) != 0) throw new InvalidOperationException("type-resolution-lease-reused");
        var findings = new List<string>();
        IReadOnlyList<string>? before = null, after = null;
        bool attempted = false, installed = false, removed = false, restored = false;
        Exception? cause = null;
        string phase = "before-patches";
        void Fail(string value) { if (findings.Count < 16) findings.Add(value); }
        try
        {
            before = FreezePatches(port.CaptureForeignPatches());
            if (port.AnyOwnPatchPresent) throw new InvalidOperationException("type-resolution-owner-already-present");
            phase = "install"; attempted = true; port.Install();
            installed = port.AllOwnPatchesPresent;
            if (!installed || !before.SequenceEqual(FreezePatches(port.CaptureForeignPatches())))
                throw new InvalidOperationException("type-resolution-install-readback");
            policy.Activate(); phase = "registration"; register();
            if (policy.Violated || policy.Depth != 0 || policy.OwnRegistrations != 1 || (policy.MappingNeeded && policy.MappingHits == 0))
                throw new InvalidOperationException("type-resolution-invocation-unconfirmed");
        }
        catch (Exception ex) { cause = ex; Fail(phase + ":" + ex.GetType().Name + ":" + ex.Message[..Math.Min(160, ex.Message.Length)]); }
        finally
        {
            policy.Deactivate(); // Any surviving managed hook is inert before removal begins.
            if (attempted)
            {
                for (int i = 0; i < 2 && !removed; i++)
                {
                    try { port.Remove(); removed = !port.AnyOwnPatchPresent; if (!removed) Fail("type-resolution-patch-remains"); }
                    catch (Exception ex) { cause ??= ex; Fail("remove:" + ex.GetType().Name); }
                }
                try
                {
                    after = FreezePatches(port.CaptureForeignPatches());
                    restored = removed && before != null && before.SequenceEqual(after);
                    if (!restored) Fail("type-resolution-restoration-unconfirmed");
                }
                catch (Exception ex) { cause ??= ex; Fail("after-patches:" + ex.GetType().Name); }
            }
            else
            {
                try { restored = before != null && !port.AnyOwnPatchPresent; }
                catch (Exception ex) { cause ??= ex; Fail("unmodified-patches:" + ex.GetType().Name); }
            }
            Outcome = new(attempted, installed, restored, policy.MappingHits, policy.OwnRegistrations,
                policy.Depth, policy.Violated, policy.Active, before, after, findings.ToArray());
        }
        if (findings.Count != 0 || !restored) throw new InvalidOperationException("type-resolution-failed:" + string.Join("|", findings), cause);
    }
    private static IReadOnlyList<string> FreezePatches(IReadOnlyList<string> values)
    {
        if (values == null || values.Count > 256 || values.Any(v => string.IsNullOrEmpty(v) || v.Length > 4096))
            throw new InvalidOperationException("patch-snapshot-unreadable");
        return Array.AsReadOnly(values.ToArray());
    }
}

public sealed record ConstructionContext(bool LoadComplete, bool Singleplayer, bool HallReady,
    bool DependenciesMatch, string? ActiveSavePath, string? LoadedSavePath, string? OrganisationName);

public static class ConstructionGate
{
    public static string? Check(ConstructionContext? context, string organisation = "TownHallTest", string slot = "SaveGame_2")
    {
        if (context == null) return "context-missing";
        if (!context.LoadComplete) return "load-incomplete";
        if (!context.Singleplayer) return "singleplayer-unconfirmed";
        if (!context.HallReady) return "hall-unready";
        if (!context.DependenciesMatch) return "dependencies-mismatch";
        if (string.IsNullOrEmpty(organisation) || string.IsNullOrEmpty(slot)) return "expected-world-missing";
        if (!string.Equals(context.OrganisationName, organisation, StringComparison.Ordinal))
            return "test-world-mismatch";
        try
        {
            if (string.IsNullOrWhiteSpace(context.ActiveSavePath) ||
                string.IsNullOrWhiteSpace(context.LoadedSavePath) ||
                !Path.IsPathFullyQualified(context.ActiveSavePath) ||
                !Path.IsPathFullyQualified(context.LoadedSavePath)) return "save-path-unconfirmed";
            string active = Path.TrimEndingDirectorySeparator(Path.GetFullPath(context.ActiveSavePath));
            string loaded = Path.TrimEndingDirectorySeparator(Path.GetFullPath(context.LoadedSavePath));
            if (!string.Equals(active, loaded, StringComparison.OrdinalIgnoreCase)) return "save-path-mismatch";
            return string.Equals(Path.GetFileName(active), slot, StringComparison.OrdinalIgnoreCase)
                ? null : "test-slot-mismatch";
        }
        catch (Exception ex) when (ex is ArgumentException or IOException or NotSupportedException)
        {
            return "save-path-invalid";
        }
    }
}

public sealed record ConstructionReadback(bool Inactive, bool ReferencesBound, bool Unowned,
    bool NoForeignComponents, string PropertyCode);

// The port owns only the unpublished shell. Capture methods must never repair registries.
public interface IBusinessConstructionPort
{
    IReadOnlyDictionary<string, string> CaptureRegistries();
    void Prepare();
    ConstructionReadback Inspect();
    string Serialize();
    void Cleanup();
    bool IsClean { get; }
}

public sealed record ConstructionResult(string Status, string[] Findings, string? SerializedJson,
    IReadOnlyDictionary<string, string>? Before, IReadOnlyDictionary<string, string>? During,
    IReadOnlyDictionary<string, string>? After, bool CleanupComplete, bool AttemptConsumed);

// One process-lifetime instance is retained by the runtime. Nothing here publishes or activates
// a native object. An uncertain attempt is terminal, including a failed prerequisite read.
public sealed class BusinessConstructionTrial
{
    public const string PropertyCode = "townhallinterior_townhall";
    public const int MaximumJsonLength = 16384;
    private int _attempted;
    public bool AttemptConsumed => Volatile.Read(ref _attempted) != 0;

    public ConstructionResult Run(ConstructionContext context, IBusinessConstructionPort port)
    {
        ArgumentNullException.ThrowIfNull(port);
        string? denial = ConstructionGate.Check(context);
        if (denial != null)
            return new("skipped", new[] { denial }, null, null, null, null, false, AttemptConsumed);
        if (Interlocked.CompareExchange(ref _attempted, 1, 0) != 0)
            return new("skipped", new[] { "attempt-already-consumed" }, null, null, null, null, false, true);

        var findings = new List<string>();
        IReadOnlyDictionary<string, string>? before = null, during = null, after = null;
        string? serialized = null;
        bool prepareAttempted = false, clean = false;
        string phase = "before-registries";
        void Fail(string reason) { if (findings.Count < 16) findings.Add(reason); }
        try
        {
            before = Snapshot(port.CaptureRegistries());
            phase = "prepare";
            prepareAttempted = true;
            port.Prepare();
            phase = "inspect";
            var readback = port.Inspect();
            if (readback == null) throw new InvalidOperationException("readback-missing");
            if (!readback.Inactive) Fail("shell-active");
            if (!readback.ReferencesBound) Fail("references-unbound");
            if (!readback.Unowned) Fail("shell-owned");
            if (!readback.NoForeignComponents) Fail("foreign-components");
            if (!string.Equals(readback.PropertyCode, PropertyCode, StringComparison.Ordinal)) Fail("code-mismatch");
            phase = "during-registries";
            during = Snapshot(port.CaptureRegistries());
            if (!Equal(before, during)) Fail("registries-changed-during-construction");
            if (findings.Count == 0)
            {
                phase = "serialize";
                string json = port.Serialize();
                if (string.IsNullOrEmpty(json) || json.Length > MaximumJsonLength) Fail("serialized-json-size");
                else
                {
                    serialized = json;
                    string? serializationError = ValidateJson(json);
                    if (serializationError != null) Fail(serializationError);
                }
            }
        }
        catch (Exception ex) { Fail(phase + ":" + ex.GetType().Name); }
        finally
        {
            if (prepareAttempted)
            {
                clean = CleanupOwned(port, Fail);
            }
            else clean = true; // No native creation was attempted.
            try
            {
                after = Snapshot(port.CaptureRegistries());
                if (before == null || !Equal(before, after)) Fail("registries-not-confirmed-after-cleanup");
            }
            catch (Exception ex) { Fail("after-registries:" + ex.GetType().Name); }
        }
        return new(findings.Count == 0 && clean ? "observed" : "failed", findings.ToArray(), serialized,
            before, during, after, clean, true);
    }

    // Shared by the trial and shutdown: an unreadable removal state must never escape
    // the optional construction feature or prevent the independent hall cleanup.
    public static bool CleanupOwned(IBusinessConstructionPort port, Action<string>? onFailure = null)
    {
        for (int attempt = 1; attempt <= 2; attempt++)
        {
            try
            {
                port.Cleanup();
                if (port.IsClean) return true;
                onFailure?.Invoke("cleanup-incomplete:" + attempt);
            }
            catch (Exception ex) { onFailure?.Invoke("cleanup:" + ex.GetType().Name); }
        }
        return false;
    }

    private static IReadOnlyDictionary<string, string> Snapshot(IReadOnlyDictionary<string, string> values)
    {
        if (values == null || values.Count == 0) throw new InvalidOperationException("registries-unreadable");
        var copy = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var pair in values)
        {
            if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                throw new InvalidOperationException("registry-entry-unreadable");
            copy.Add(pair.Key, pair.Value);
        }
        return new ReadOnlyDictionary<string, string>(copy);
    }

    private static bool Equal(IReadOnlyDictionary<string, string> left, IReadOnlyDictionary<string, string> right)
    {
        if (left.Count != right.Count) return false;
        foreach (var pair in left)
            if (!right.TryGetValue(pair.Key, out string? value) ||
                !string.Equals(pair.Value, value, StringComparison.Ordinal)) return false;
        return true;
    }

    private static string? ValidateJson(string json)
    {
        using var document = JsonDocument.Parse(json, new JsonDocumentOptions { MaxDepth = 16 });
        var root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Object) return "serialized-json-not-object";
        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in root.EnumerateObject())
            if (!keys.Add(property.Name)) return "serialized-json-duplicate-key";
        if (!root.TryGetProperty("PropertyCode", out var code) || code.ValueKind != JsonValueKind.String ||
            !string.Equals(code.GetString(), PropertyCode, StringComparison.Ordinal)) return "serialized-code-mismatch";
        if (!root.TryGetProperty("IsOwned", out var owned) || owned.ValueKind != JsonValueKind.False)
            return "serialized-ownership-mismatch";
        if (!root.TryGetProperty("LaunderingOperations", out var operations) ||
            operations.ValueKind != JsonValueKind.Array || operations.GetArrayLength() != 0)
            return "serialized-operations-mismatch";
        return null;
    }
}

// Pure activation lease with immutable reads and explicit generation handoff.
public sealed record RegistrationSnapshot(IReadOnlyDictionary<string, string> ForeignFingerprints,
    IReadOnlyDictionary<string, int> OwnedCounts, bool RootAlive, bool RootActive,
    bool ReferencesBound, bool Unowned);

public interface IBusinessActivationPort
{
    RegistrationSnapshot Capture();
    void Prepare();
    void Activate();
    void Register();
    void Cleanup();
    bool IsClean { get; }
}

public sealed record ActivationResult(string Status, string[] Findings, RegistrationSnapshot? Before,
    RegistrationSnapshot? Current, RegistrationSnapshot? After, bool CleanupComplete, long Generation);

public sealed class BusinessActivationLease
{
    private readonly IReadOnlyDictionary<string, int> _expected;
    private readonly List<string> _findings = new();
    private IBusinessActivationPort? _port;
    private RegistrationSnapshot? _before, _current, _after;
    private bool _attempted, _prepared, _closed, _clean, _uncertain;
    private int _busy, _reentered, _beginFrame;
    public long Generation { get; }
    public string State { get; private set; } = "new";
    public bool CanStartNextGeneration => Volatile.Read(ref _busy) == 0 &&
        (!_attempted || (_closed && _clean && !_uncertain));

    public BusinessActivationLease(long generation, IReadOnlyDictionary<string, int> expectedOwnedCounts)
    {
        if (generation < 1) throw new ArgumentOutOfRangeException(nameof(generation));
        ArgumentNullException.ThrowIfNull(expectedOwnedCounts);
        if (expectedOwnedCounts.Count == 0) throw new ArgumentException("Expected registries are required.", nameof(expectedOwnedCounts));
        var copy = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var pair in expectedOwnedCounts)
        {
            if (string.IsNullOrEmpty(pair.Key) || pair.Value < 0) throw new ArgumentException("Invalid expected registry.", nameof(expectedOwnedCounts));
            copy.Add(pair.Key, pair.Value);
        }
        _expected = new ReadOnlyDictionary<string, int>(copy);
        Generation = generation;
    }

    public ActivationResult Begin(ConstructionContext context, IBusinessActivationPort port, int frame)
    {
        ArgumentNullException.ThrowIfNull(port);
        if (!Enter()) return Result("skipped", "reentrant-call");
        string phase = "baseline";
        try
        {
            if (_attempted || _closed) return Result("skipped", "attempt-already-consumed");
            string? denial = ConstructionGate.Check(context);
            if (denial != null || frame < 0) return Result("skipped", denial ?? "invalid-frame");
            _attempted = true; _port = port; _beginFrame = frame;
            _before = Capture();
            if (!Empty(_before)) throw new InvalidOperationException("Owned state exists before construction.");
            phase = "prepare";
            _prepared = true; port.Prepare(); CheckReentry();
            _current = Capture();
            RequireForeignBaseline(_current);
            if (!_current.RootAlive || _current.RootActive || !_current.ReferencesBound || !_current.Unowned || !Counts(_current, false))
                throw new InvalidOperationException("Staging readback failed.");
            phase = "activate";
            port.Activate(); CheckReentry();
            phase = "register";
            port.Register(); CheckReentry();
            phase = "entered-readback";
            _current = Capture();
            RequireForeignBaseline(_current);
            if (!_current.RootAlive || !_current.RootActive || !_current.ReferencesBound || !_current.Unowned || !Counts(_current, true))
                throw new InvalidOperationException("Activated root readback failed.");
            State = "entered";
        }
        catch (Exception ex) { Record(phase + ":" + ex.GetType().Name); Abort(); }
        finally { Volatile.Write(ref _busy, 0); }
        return Result(State);
    }

    public ActivationResult Complete(int frame)
    {
        if (!Enter()) return Result("skipped", "reentrant-call");
        try
        {
            if (State != "entered") return Result(State == "active" ? "active" : "skipped", State == "active" ? null : "not-entered");
            if (frame <= _beginFrame) return Result("entered");
            _current = Capture();
            RequireForeignBaseline(_current);
            if (!_current.RootAlive || !_current.RootActive || !_current.ReferencesBound || !_current.Unowned || !Counts(_current, true))
                throw new InvalidOperationException("Registration readback failed.");
            State = "active";
        }
        catch (Exception ex) { Record("complete:" + ex.GetType().Name); Abort(); }
        finally { Volatile.Write(ref _busy, 0); }
        return Result(State);
    }

    public ActivationResult Fail(string reason)
    {
        if (!Enter()) return Result("skipped", "reentrant-call");
        try { Record(string.IsNullOrEmpty(reason) ? "runtime-failure" : reason.Length <= 160 ? reason : reason.Substring(0, 160)); Abort(); }
        finally { Volatile.Write(ref _busy, 0); }
        return Result(State);
    }

    public ActivationResult Leave()
    {
        if (!Enter()) return Result("skipped", "reentrant-call");
        try
        {
            if (!_closed) CleanupCore();
            State = _findings.Count == 0 && _clean && !_uncertain ? "closed" : "failed";
        }
        finally { Volatile.Write(ref _busy, 0); }
        return Result(State);
    }

    private void Abort()
    {
        if (!_closed)
        {
            if (_prepared) CleanupCore();
            else { _closed = true; _clean = !_attempted; _uncertain = _attempted; }
        }
        State = "failed";
    }

    private void CleanupCore()
    {
        if (!_prepared || _port == null) { _closed = true; _clean = !_attempted; return; }
        RegistrationSnapshot? boundary = null;
        try { boundary = Capture(); }
        catch (Exception ex) { Record("cleanup-before:" + ex.GetType().Name); _uncertain = true; }
        for (int attempt = 1; attempt <= 2 && !_clean; attempt++)
        {
            try
            {
                _port.Cleanup();
                _clean = _port.IsClean;
                if (!_clean) Record("cleanup-incomplete:" + attempt);
            }
            catch (Exception ex) { Record("cleanup:" + ex.GetType().Name); }
        }
        try
        {
            _after = Capture();
            if (boundary == null || !SameForeign(boundary, _after) || !Empty(_after))
            { Record("cleanup-state-unconfirmed"); _uncertain = true; }
        }
        catch (Exception ex) { Record("cleanup-after:" + ex.GetType().Name); _uncertain = true; }
        if (!_clean) _uncertain = true;
        _closed = true;
    }

    private RegistrationSnapshot Capture()
    {
        var source = _port!.Capture(); CheckReentry();
        if (source == null || source.ForeignFingerprints == null || source.ForeignFingerprints.Count == 0 || source.OwnedCounts == null)
            throw new InvalidOperationException("Registry snapshot is unreadable.");
        var foreign = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var pair in source.ForeignFingerprints)
        {
            if (string.IsNullOrEmpty(pair.Key) || pair.Value == null) throw new InvalidOperationException("Registry fingerprint is unreadable.");
            foreign.Add(pair.Key, pair.Value);
        }
        var owned = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var pair in source.OwnedCounts)
        {
            if (string.IsNullOrEmpty(pair.Key) || pair.Value < 0) throw new InvalidOperationException("Owned registry count is unreadable.");
            owned.Add(pair.Key, pair.Value);
        }
        if (owned.Count != _expected.Count) throw new InvalidOperationException("Registry count scope differs.");
        foreach (string key in _expected.Keys)
            if (!owned.ContainsKey(key) || !foreign.ContainsKey(key)) throw new InvalidOperationException("Registry scope is incomplete.");
        return source with { ForeignFingerprints = new ReadOnlyDictionary<string, string>(foreign), OwnedCounts = new ReadOnlyDictionary<string, int>(owned) };
    }

    private void RequireForeignBaseline(RegistrationSnapshot current)
    {
        if (_before == null || !SameForeign(_before, current))
        { _uncertain = true; throw new InvalidOperationException("Foreign registrations changed."); }
    }

    private bool Empty(RegistrationSnapshot snapshot) => !snapshot.RootAlive && !snapshot.RootActive && Counts(snapshot, false);
    private bool Counts(RegistrationSnapshot snapshot, bool registered)
    {
        foreach (var pair in _expected)
            if (snapshot.OwnedCounts[pair.Key] != (registered ? pair.Value : 0)) return false;
        return true;
    }

    private static bool SameForeign(RegistrationSnapshot left, RegistrationSnapshot right)
    {
        if (left.ForeignFingerprints.Count != right.ForeignFingerprints.Count) return false;
        foreach (var pair in left.ForeignFingerprints)
            if (!right.ForeignFingerprints.TryGetValue(pair.Key, out string? value) || !string.Equals(value, pair.Value, StringComparison.Ordinal)) return false;
        return true;
    }

    private bool Enter()
    {
        if (Interlocked.CompareExchange(ref _busy, 1, 0) != 0) { Volatile.Write(ref _reentered, 1); return false; }
        Volatile.Write(ref _reentered, 0); return true;
    }
    private void CheckReentry() { if (Volatile.Read(ref _reentered) != 0) throw new InvalidOperationException("Reentrant lease call."); }
    private void Record(string reason) { if (_findings.Count < 24) _findings.Add(reason); }
    private ActivationResult Result(string status, string? extra = null)
    {
        var findings = new List<string>(_findings);
        if (extra != null && findings.Count < 24) findings.Add(extra);
        return new(status, findings.ToArray(), _before, _current, _after, _closed && _clean && !_uncertain, Generation);
    }
}

#if TOWNHALL_BUSINESS_CONSTRUCTION
// Native construction experiment. Never activates, registers, loads or writes a business.
internal static class BusinessConstructionRuntime
{
    private const string Owner = "TownHallInterior.BusinessConstruction";
    private const string NativeHash = "346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8";
    private const string FishNetHash = "0acb5c70078f5be6e88574be17b675657fc50898eb3dd264a3b98a54545e24ec";
    private const string ApiHash = "d751a3cb66003f7a616eb9f8054daf34da0d9d7f23416a5005a586738a680852";
    private static readonly BusinessConstructionTrial Trial = new();
    private static readonly System.Collections.Generic.List<System.Reflection.MethodInfo> Targets = new();
    private static HarmonyLib.Harmony? _harmony;
    private static System.Func<bool>? _hallReady;
    private static NativeConstructionPort? _port;
    private static bool _ready, _loadComplete;
    private static float _due = -1;
    private static int _thread, _unexpectedLifecycle;
    private static string _dllHash = "", _reportRoot = "";

    internal static void Start(System.Func<bool> hallReady)
    {
        _thread = System.Environment.CurrentManagedThreadId;
        _hallReady = hallReady;
        try
        {
            _dllHash = HashFile(typeof(Mod).Assembly.Location);
            if (HashFile(typeof(Il2CppFishNet.Object.NetworkObject).Assembly.Location) != FishNetHash ||
                HashFile(typeof(Il2CppScheduleOne.Property.Business).Assembly.Location) != NativeHash ||
                HashFile(typeof(S1API.Lifecycle.GameLifecycle).Assembly.Location) != ApiHash)
                throw new System.InvalidOperationException("dependency-hash-mismatch");
            _reportRoot = System.IO.Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory,
                "TownHallInterior", "BusinessConstruction");
            System.IO.Directory.CreateDirectory(_reportRoot);
            _harmony = new HarmonyLib.Harmony(Owner);
            Guard(typeof(Il2CppScheduleOne.Property.Business), "Awake");
            Guard(typeof(Il2CppScheduleOne.Property.Business), "Start");
            Guard(typeof(Il2CppScheduleOne.Property.Property), "Awake");
            Guard(typeof(Il2CppScheduleOne.Property.Property), "Start");
            Guard(typeof(Il2CppScheduleOne.Property.Property), "InitializeSaveable");
            Guard(typeof(Il2CppFishNet.Object.NetworkObject), "Awake");
            Guard(typeof(Il2CppFishNet.Object.NetworkObject), "Start");
            _ready = true;
            S1API.Lifecycle.GameLifecycle.OnLoadComplete += Loaded;
            S1API.Lifecycle.GameLifecycle.OnPreSceneChange += Leaving;
            MelonLoader.MelonLogger.Msg("TownHall construction: inactive native shell test armed for TownHallTest / SaveGame_2 only.");
        }
        catch (System.Exception ex)
        {
            _ready = false;
            MelonLoader.MelonLogger.Warning("TownHall construction unavailable: " + ex.GetType().Name + ": " + ex.Message);
            try { _harmony?.UnpatchSelf(); } catch { /* No construction can run while unready. */ }
        }
    }

    private static void Guard(System.Type type, string methodName)
    {
        var method = type.GetMethod(methodName, System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.DeclaredOnly, null, System.Type.EmptyTypes, null);
        if (method == null || method.ReturnType != typeof(void))
            throw new System.MissingMethodException(type.FullName, methodName);
        Targets.Add(method);
        _harmony!.Patch(method, prefix: new HarmonyLib.HarmonyMethod(typeof(BusinessConstructionRuntime), nameof(BlockStagedLifecycle)));
        if (HarmonyLib.Harmony.GetPatchInfo(method)?.Owners.Contains(Owner) != true)
            throw new System.InvalidOperationException("construction-guard-owner-missing");
    }

    private static bool BlockStagedLifecycle(UnityEngine.Component __instance)
    {
        // Root identity is captured before AddComponent; a foreign object always passes.
        if (_port == null || !_port.Owns(__instance)) return true;
        System.Threading.Interlocked.Increment(ref _unexpectedLifecycle);
        return false;
    }

    private static void Loaded()
    {
        if (!_ready || System.Environment.CurrentManagedThreadId != _thread) return;
        _loadComplete = true;
        _due = UnityEngine.Time.realtimeSinceStartup + 6;
    }

    private static void Leaving() { _loadComplete = false; _due = -1; }

    internal static void Tick()
    {
        if (!_ready || !_loadComplete || _due < 0 || UnityEngine.Time.realtimeSinceStartup < _due) return;
        _due = -1;
        try
        {
            if (System.Environment.CurrentManagedThreadId != _thread) throw new System.InvalidOperationException("wrong-thread");
            var load = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
            var info = load?.ActiveSaveInfo;
            var lobby = Il2CppScheduleOne.DevUtilities.Singleton<Il2CppScheduleOne.Networking.Lobby>.Instance;
            bool singleplayer = S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer() && S1Mods.Shared.NetworkGuard.IsAlive(lobby) &&
                !lobby.IsInLobby && Il2CppScheduleOne.PlayerScripts.Player.PlayerList?.Count == 1;
            bool guards = Targets.Count == 7 && Targets.All(t => HarmonyLib.Harmony.GetPatchInfo(t)?.Owners.Contains(Owner) == true);
            var context = new ConstructionContext(_loadComplete && S1Mods.Shared.NetworkGuard.IsInMainScene,
                singleplayer, _hallReady?.Invoke() == true, guards,
                info?.SavePath, load?.LoadedGameFolderPath, info?.OrganisationName);
            _port ??= new NativeConstructionPort(() => _unexpectedLifecycle == 0);
            var result = Trial.Run(context, _port);
            var report = new
            {
                Schema = 1, Mode = "inactive-native-construction", CapturedUtc = System.DateTime.UtcNow,
                AssemblySha256 = _dllHash, NativeProxySha256 = NativeHash, S1ApiSha256 = ApiHash, FishNetSha256 = FishNetHash,
                Target = "TownHallTest / SaveGame_2", WorldPublicationAllowed = false,
                NativeSaveWriteAllowed = false, UnexpectedLifecycleCalls = _unexpectedLifecycle,
                GuardOwnersPresent = guards, Result = result,
                Limitation = "Inactive construction and serialization only. No Awake/Start, activation, registration, station, native save/load or gameplay acceptance. Registry fingerprints cover the listed collections, not every possible native effect."
            };
            string path = System.IO.Path.Combine(_reportRoot, System.DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffZ") + ".json");
            System.IO.File.WriteAllText(path + ".tmp", System.Text.Json.JsonSerializer.Serialize(report,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            System.IO.File.Move(path + ".tmp", path, true);
            MelonLoader.MelonLogger.Msg("TownHall construction " + result.Status + ": " + path);
        }
        catch (System.Exception ex)
        {
            _ready = false;
            MelonLoader.MelonLogger.Warning("TownHall construction stopped: " + ex.GetType().Name);
            TryCleanup();
        }
    }

    internal static void Stop()
    {
        Leaving(); _ready = false;
        S1API.Lifecycle.GameLifecycle.OnLoadComplete -= Loaded;
        S1API.Lifecycle.GameLifecycle.OnPreSceneChange -= Leaving;
        bool clean = TryCleanup();
        // Keep the scoped guards if cleanup could not establish native absence.
        if (!clean) return;
        try { _harmony?.UnpatchSelf(); }
        catch (System.Exception ex) { MelonLoader.MelonLogger.Warning("TownHall construction unpatch: " + ex.GetType().Name); }
    }

    private static bool TryCleanup() => _port == null || BusinessConstructionTrial.CleanupOwned(_port);

    private static string HashFile(string path) => System.Convert.ToHexString(
        System.Security.Cryptography.SHA256.HashData(System.IO.File.ReadAllBytes(path))).ToLowerInvariant();
}

internal sealed partial class NativeConstructionPort : IBusinessConstructionPort
{
    private const string Code = "townhallinterior_townhall";
    private readonly System.Func<bool> _lifecycleUntouched;
    private UnityEngine.GameObject? _root;
    private Il2CppScheduleOne.Property.Business? _business;
    private Il2CppFishNet.Object.NetworkObject? _network;
    private Il2CppScheduleOne.Property.PropertyContentsContainer? _container;
    private UnityEngine.RectTransform? _ui;
    private UnityEngine.BoxCollider? _bounds;
    private Il2CppScheduleOne.Persistence.Loaders.PropertyLoader? _propertyLoader;
    private Il2CppScheduleOne.Persistence.Loaders.BusinessLoader? _businessLoader;
    private readonly System.Collections.Generic.List<UnityEngine.Transform> _points = new();
    private readonly System.Collections.Generic.List<UnityEngine.GameObject> _ownedObjects = new();
    private System.IntPtr _rootPointer;
    private System.IntPtr _businessNativePointer, _networkNativePointer;

    internal NativeConstructionPort(System.Func<bool> lifecycleUntouched) => _lifecycleUntouched = lifecycleUntouched;
    internal bool Owns(UnityEngine.Component component)
    {
        if (ReferenceEquals(component, null)) return false;
        try
        {
            // Native identities remain usable while Unity destroys the component wrapper.
            var pointer = component.Pointer;
            if (pointer != IntPtr.Zero && (pointer == _businessNativePointer || pointer == _networkNativePointer)) return true;
            return _rootPointer != IntPtr.Zero && component.gameObject.Pointer == _rootPointer;
        }
        catch { return false; }
    }

    public bool IsClean
    {
        get
        {
            foreach (var owned in _ownedObjects)
                if (!Destroyed(owned)) return false;
            return true;
        }
    }

    public System.Collections.Generic.IReadOnlyDictionary<string, string> CaptureRegistries()
    {
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save) || save.IsSaving)
            throw new System.InvalidOperationException("save-manager-unavailable-or-saving");
        return new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["properties"] = Fingerprint(Il2CppScheduleOne.Property.Property.Properties),
            ["properties-owned"] = Fingerprint(Il2CppScheduleOne.Property.Property.OwnedProperties),
            ["properties-unowned"] = Fingerprint(Il2CppScheduleOne.Property.Property.UnownedProperties),
            ["businesses"] = Fingerprint(Il2CppScheduleOne.Property.Business.Businesses),
            ["businesses-owned"] = Fingerprint(Il2CppScheduleOne.Property.Business.OwnedBusinesses),
            ["businesses-unowned"] = Fingerprint(Il2CppScheduleOne.Property.Business.UnownedBusinesses),
            ["saveables"] = Fingerprint(save.Saveables), ["base-saveables"] = Fingerprint(save.BaseSaveables),
            ["completed-saveables"] = Fingerprint(save.CompletedSaveables), ["save-requests"] = Fingerprint(save.QueuedSaveRequests),
            ["staged-lifecycle"] = _lifecycleUntouched() ? "uninvoked" : "unexpected-call-blocked"
        };
    }

    private static string Fingerprint<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        if (list == null || list.WasCollected || list.Count > 8192)
            throw new System.InvalidOperationException("registry-unreadable-or-over-budget");
        int count = list.Count;
        var values = new System.Text.StringBuilder(list.Pointer.ToInt64().ToString("X"));
        for (int i = 0; i < count; i++) values.Append('|').Append(list[i]?.Pointer.ToInt64().ToString("X") ?? "null");
        if (count != list.Count) throw new System.InvalidOperationException("registry-changed-during-read");
        return count + ":" + System.Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(values.ToString()))).ToLowerInvariant();
    }

    public void Prepare()
    {
        if (_rootPointer != System.IntPtr.Zero) throw new System.InvalidOperationException("construction-already-attempted");
        var properties = UnityEngine.Resources.FindObjectsOfTypeAll<Il2CppScheduleOne.Property.Property>();
        if (properties.Length > 512) throw new System.InvalidOperationException("property-scan-over-budget");
        foreach (var p in properties)
            if (p != null && p.propertyCode == Code) throw new System.InvalidOperationException("business-code-collision");
        _root = new UnityEngine.GameObject("TownHallInterior.InactiveBusinessTrial");
        _ownedObjects.Add(_root);
        _rootPointer = _root.Pointer;
        _root.SetActive(false);
        _root.transform.position = new UnityEngine.Vector3(58.85f, 1.25f, 31);
        _network = _root.AddComponent<Il2CppFishNet.Object.NetworkObject>();
        _networkNativePointer = _network.Pointer;
        var b = _business = _root.AddComponent<Il2CppScheduleOne.Property.Business>();
        _businessNativePointer = b.Pointer;
        _network._networkBehaviours = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<Il2CppFishNet.Object.NetworkBehaviour>(
            new Il2CppFishNet.Object.NetworkBehaviour[] { b });
        b._networkObjectCache = _network;
        b._componentIndexCache = 0;
        b.propertyCode = Code; b.propertyName = "Hyland Town Hall";
        b.OwnedByDefault = false; b._IsOwned_k__BackingField = false; b.IsOwnedVariable = "";
        b.Price = 0; b.EmployeeCapacity = 0; b.LaunderCapacity = 0;
        b.ContentCullingEnabled = false; b._hasBeenLoaded = false; b._HasChanged_k__BackingField = false;
        b.onThisPropertyAcquired = new UnityEngine.Events.UnityEvent();
        b._Employees_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Employees.Employee>();
        b.BuildableItems = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.EntityFramework.BuildableItem>();
        b.Configurables = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Management.IConfigurable>();
        b.Grids = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Tiles.Grid>();
        b.Switches = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Misc.ModularSwitch>();
        b.Toggleables = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Interaction.InteractableToggleable>();
        b.LaunderingOperations = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Property.LaunderingOperation>();
        b._LocalExtraFiles_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        b._LocalExtraFolders_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        b.savedObjectPaths = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        b.savedEmployeePaths = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        b.ObjectsToCull = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<UnityEngine.GameObject>(0);
        b.LoadingDocks = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<Il2CppScheduleOne.Delivery.LoadingDock>(0);
        b.SpawnPoint = Point("Spawn", new UnityEngine.Vector3(53.4f, 1.25f, 31));
        b.InteriorSpawnPoint = Point("InteriorSpawn", new UnityEngine.Vector3(56, 1.25f, 31));
        b.NPCSpawnPoint = Point("NPCSpawn", new UnityEngine.Vector3(53.4f, 1.25f, 32));
        var idle = Point("Idle", new UnityEngine.Vector3(57, 4.75f, 31));
        b.EmployeeIdlePoints = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<UnityEngine.Transform>(new[] { idle });
        var boundsObject = Child("Bounds");
        _bounds = boundsObject.AddComponent<UnityEngine.BoxCollider>();
        _bounds.isTrigger = true; _bounds.center = new UnityEngine.Vector3(0, 3.5f, 0);
        _bounds.size = new UnityEngine.Vector3(8.5f, 7, 20.5f);
        b.BoundingBox = boundsObject;
        b.propertyBoundsColliders = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<UnityEngine.BoxCollider>(new[] { _bounds });
        _container = Child("Contents").AddComponent<Il2CppScheduleOne.Property.PropertyContentsContainer>();
        _container._Property_k__BackingField = b; b._Container_k__BackingField = _container;
        _ui = Child("WorldspaceUI").AddComponent<UnityEngine.RectTransform>();
        b._WorldspaceUIContainer_k__BackingField = _ui;
        _propertyLoader = new Il2CppScheduleOne.Persistence.Loaders.PropertyLoader();
        _businessLoader = new Il2CppScheduleOne.Persistence.Loaders.BusinessLoader();
        ((Il2CppScheduleOne.Property.Property)b).loader = _propertyLoader;
        b.loader = _businessLoader;
    }

    private UnityEngine.GameObject Child(string name)
    {
        var child = new UnityEngine.GameObject(name);
        _ownedObjects.Add(child); // Retain even if parenting fails; cleanup must cover orphaned children.
        child.SetActive(false);
        child.transform.SetParent(_root!.transform, false);
        return child;
    }

    private UnityEngine.Transform Point(string name, UnityEngine.Vector3 position)
    {
        var point = Child(name).transform; point.position = position; _points.Add(point); return point;
    }

    public ConstructionReadback Inspect()
    {
        var b = _business ?? throw new System.InvalidOperationException("business-missing");
        bool inactive = _root != null && !_root.activeSelf && !_root.activeInHierarchy && _lifecycleUntouched();
        bool refs = _network != null && b._networkObjectCache?.Pointer == _network.Pointer && b._componentIndexCache == 0 &&
            _network._networkBehaviours?.Length == 1 && _network._networkBehaviours[0]?.Pointer == b.Pointer &&
            b.SpawnPoint?.Pointer == _points[0].Pointer && b.InteriorSpawnPoint?.Pointer == _points[1].Pointer &&
            b.NPCSpawnPoint?.Pointer == _points[2].Pointer && b.EmployeeIdlePoints?.Length == 1 &&
            b.EmployeeIdlePoints[0]?.Pointer == _points[3].Pointer && b.BoundingBox?.Pointer == _bounds?.gameObject.Pointer &&
            b.propertyBoundsColliders?.Length == 1 && b.propertyBoundsColliders[0]?.Pointer == _bounds?.Pointer &&
            b.Container?.Pointer == _container?.Pointer && _container?.Property?.Pointer == b.Pointer &&
            b.WorldspaceUIContainer?.Pointer == _ui?.Pointer && ((Il2CppScheduleOne.Property.Property)b).loader?.Pointer == _propertyLoader?.Pointer &&
            b.loader?.Pointer == _businessLoader?.Pointer && b.onThisPropertyAcquired != null &&
            b.Employees?.Count == 0 && b.BuildableItems?.Count == 0 && b.Configurables?.Count == 0 &&
            b.Grids?.Count == 0 && b.Switches?.Count == 0 && b.Toggleables?.Count == 0 && b.LaunderingOperations?.Count == 0 &&
            b.LocalExtraFiles?.Count == 0 && b.LocalExtraFolders?.Count == 0 && b.savedObjectPaths?.Count == 0 &&
            b.savedEmployeePaths?.Count == 0 && b.ObjectsToCull?.Length == 0 && b.LoadingDocks?.Length == 0 &&
            !b.ContentCullingEnabled && !b._hasBeenLoaded && b.propertyName == "Hyland Town Hall";
        bool own = true;
        var components = _root!.GetComponentsInChildren<UnityEngine.Component>(true);
        if (components.Length > 32) own = false;
        foreach (var component in components)
        {
            if (component == null) { own = false; continue; }
            if (component.TryCast<UnityEngine.Transform>() != null) continue;
            var pointer = component.Pointer;
            if (pointer != b.Pointer && pointer != _network?.Pointer && pointer != _bounds?.Pointer && pointer != _container?.Pointer) own = false;
        }
        return new ConstructionReadback(inactive, refs, !b.IsOwned && !b.OwnedByDefault && b.LaunderingOperations?.Count == 0, own, b.PropertyCode);
    }

    public string Serialize()
    {
        if (!Inspect().Inactive) throw new System.InvalidOperationException("construction-not-inactive");
        return _business!.GetSaveString();
    }

    public void Cleanup()
    {
        for (int i = _ownedObjects.Count - 1; i >= 0; i--)
            if (!Destroyed(_ownedObjects[i])) UnityEngine.Object.DestroyImmediate(_ownedObjects[i]);
        if (!IsClean) throw new System.InvalidOperationException("owned-root-still-present");
        _root = null; _business = null; _network = null; _container = null; _ui = null; _bounds = null;
        _points.Clear(); _ownedObjects.Clear(); _propertyLoader = null; _businessLoader = null; _rootPointer = IntPtr.Zero;
        _businessNativePointer = IntPtr.Zero; _networkNativePointer = IntPtr.Zero;
    }

    private static bool Destroyed(UnityEngine.GameObject owned)
    {
        if (owned.Pointer == IntPtr.Zero || owned.WasCollected)
            throw new InvalidOperationException("owned-object-state-unreadable");
        return (UnityEngine.Object)owned == null;
    }
}

#if TOWNHALL_BUSINESS_ACTIVATION
// Controlled diagnostic publication. Native lifecycle bodies stay suppressed; the lease
// is the sole owner of registration. Saving retires the lease before native enumeration.
internal static class BusinessActivationRuntime
{
    private const string Owner = "TownHallInterior.BusinessActivation";
    private const string NativeHash = "346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8";
    private const string ApiHash = "d751a3cb66003f7a616eb9f8054daf34da0d9d7f23416a5005a586738a680852";
    private const string FishNetHash = "0acb5c70078f5be6e88574be17b675657fc50898eb3dd264a3b98a54545e24ec";
    private static readonly Dictionary<string, int> Expected = new(StringComparer.Ordinal)
    {
        ["properties"] = 1, ["properties-owned"] = 0, ["properties-unowned"] = 1,
        ["businesses"] = 1, ["businesses-owned"] = 0, ["businesses-unowned"] = 1,
        ["saveables"] = 0, ["base-saveables"] = 0, ["completed-saveables"] = 0, ["save-requests"] = 0
    };
    private static readonly List<System.Reflection.MethodInfo> Targets = new();
    private static readonly Dictionary<string, int> Calls = new(StringComparer.Ordinal);
    private static readonly List<object> History = new();
    private static readonly string[] RequiredCalls = { "Business.Awake", "Business.Start", "NetworkObject.Awake", "NetworkObject.Start" };
    private static HarmonyLib.Harmony? _harmony;
    private static Func<bool>? _hallReady;
    private static NativeConstructionPort? _port;
    private static BusinessActivationLease? _lease;
    private static ActivationResult? _result;
    private static ConstructionContext? _context;
    private static bool _ready, _loaded, _loadTicket, _ending, _quarantined;
    private static int _thread;
    private static long _generation;
    private static float _due = -1, _completeDue, _monitorDue;
    private static string _assemblyHash = "", _directory = "", _run = "", _phase = "idle", _critical = "";

    internal static void Start(Func<bool> hallReady)
    {
        _thread = Environment.CurrentManagedThreadId; _hallReady = hallReady;
        try
        {
            _assemblyHash = Hash(typeof(Mod).Assembly.Location);
            if (Hash(typeof(Il2CppScheduleOne.Property.Business).Assembly.Location) != NativeHash ||
                Hash(typeof(S1API.Lifecycle.GameLifecycle).Assembly.Location) != ApiHash ||
                Hash(typeof(Il2CppFishNet.Object.NetworkObject).Assembly.Location) != FishNetHash)
                throw new InvalidOperationException("activation-dependency-hash-mismatch");
            _run = DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffZ") + "-" + Guid.NewGuid().ToString("N");
            _directory = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "TownHallInterior", "BusinessActivation");
            Directory.CreateDirectory(_directory);
            _harmony = new HarmonyLib.Harmony(Owner);
            var property = typeof(Il2CppScheduleOne.Property.Property);
            var business = typeof(Il2CppScheduleOne.Property.Business);
            var network = typeof(Il2CppFishNet.Object.NetworkObject);
            foreach (var type in new[] { property, business })
                foreach (string name in new[] { "Awake", "Start", "OnDestroy" }) Patch(type, name, nameof(BlockLifecycle), typeof(void));
            foreach (string name in new[] { "Awake", "Start", "OnDisable", "OnDestroy" }) Patch(network, name, nameof(BlockLifecycle), typeof(void));
            Patch(property, "FixedUpdate", nameof(BlockLifecycle), typeof(void));
            foreach (string name in new[] { "InitializeSaveable", "SetOwned", "SetOwned_Server", "ReceiveOwned_Networked", "RecieveOwned",
                "RpcLogic___SetOwned_Server_2166136261", "RpcLogic___ReceiveOwned_Networked_2166136261",
                "NetworkInitialize___Early", "NetworkInitialize__Late", "NetworkInitializeIfDisabled" })
                Patch(property, name, nameof(BlockOperation), typeof(void));
            foreach (string name in new[] { "RecieveOwned", "MinPass", "MinsPass", "TimeSkipped", "StartLaunderingOperation",
                "ReceiveLaunderingOperation", "CompleteOperation", "RpcLogic___StartLaunderingOperation_1481775633",
                "RpcLogic___ReceiveLaunderingOperation_1001022388", "NetworkInitialize___Early", "NetworkInitialize__Late", "NetworkInitializeIfDisabled" })
                Patch(business, name, nameof(BlockOperation), typeof(void));
            Patch(property, "ShouldSave", nameof(BlockEligibility), typeof(bool));
            Patch(property, "CanBePurchased", nameof(BlockEligibility), typeof(bool));
            var save = typeof(Il2CppScheduleOne.Persistence.SaveManager);
            var saves = Declared(save).Where(m => m.Name == "Save").ToArray();
            if (saves.Length != 2 || !saves.Any(m => m.GetParameters().Length == 0) ||
                !saves.Any(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(string) })))
                throw new MissingMethodException("SaveManager.Save overload contract changed");
            foreach (var method in saves) Patch(method, nameof(BeforeSave), typeof(void));
            Patch(save, "RegisterSaveable", nameof(BlockSaveable), typeof(void));
            Patch(save, "QueueSaveRequest", nameof(BlockSaveRequest), typeof(void));
            _ready = OwnersPresent();
            if (!_ready) throw new InvalidOperationException("activation-guard-owner-missing");
            S1API.Lifecycle.GameLifecycle.OnPreLoad += PreLoad;
            S1API.Lifecycle.GameLifecycle.OnLoadComplete += Loaded;
            S1API.Lifecycle.GameLifecycle.OnPreSceneChange += Leaving;
            MelonLoader.MelonLogger.Msg("TownHall activation: controlled registration armed for TownHallTest / SaveGame_2; native initialization and persistence remain blocked.");
        }
        catch (Exception ex)
        {
            _ready = false;
            MelonLoader.MelonLogger.Warning("TownHall activation unavailable: " + ex);
            try { _harmony?.UnpatchSelf(); } catch { }
        }
    }

    private static System.Reflection.MethodInfo[] Declared(Type type) => type.GetMethods(System.Reflection.BindingFlags.Public |
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
    private static void Patch(Type type, string name, string prefix, Type returns)
    {
        var methods = Declared(type).Where(m => m.Name == name).ToArray();
        if (methods.Length != 1) throw new MissingMethodException(type.FullName, name);
        Patch(methods[0], prefix, returns);
    }
    private static void Patch(System.Reflection.MethodInfo method, string prefix, Type returns)
    {
        if (method.ReturnType != returns) throw new InvalidOperationException("guard-return-type:" + method.Name);
        Targets.Add(method);
        _harmony!.Patch(method, prefix: new HarmonyLib.HarmonyMethod(typeof(BusinessActivationRuntime), prefix) { priority = HarmonyLib.Priority.First });
        if (HarmonyLib.Harmony.GetPatchInfo(method)?.Owners.Contains(Owner) != true)
            throw new InvalidOperationException("activation-guard-owner-missing:" + method.Name);
    }
    private static bool OwnersPresent() => Targets.Count > 0 && Targets.All(m => HarmonyLib.Harmony.GetPatchInfo(m)?.Owners.Contains(Owner) == true);
    private static string Key(System.Reflection.MethodBase method) => method.DeclaringType!.Name + "." + method.Name;
    private static void Count(string key) { Calls.TryGetValue(key, out int count); Calls[key] = Math.Min(1000000, count + 1); }
    private static bool BlockLifecycle(UnityEngine.Component __instance, System.Reflection.MethodBase __originalMethod)
    {
        if (_port?.Owns(__instance) != true) return true;
        string key = Key(__originalMethod); Count(key);
        bool allowed = __originalMethod.Name == "FixedUpdate" ||
            (_phase == "activating" && __originalMethod.Name == "Awake") ||
            ((_phase == "entered" || _phase == "active") && __originalMethod.Name == "Start") ||
            (_phase == "cleanup" && (__originalMethod.Name == "OnDestroy" || __originalMethod.Name == "OnDisable"));
        if (!allowed || (RequiredCalls.Contains(key) && Calls[key] > 1)) _critical = "unexpected-lifecycle:" + key;
        return false;
    }
    private static bool BlockOperation(UnityEngine.Component __instance, System.Reflection.MethodBase __originalMethod)
    {
        if (_port?.Owns(__instance) != true) return true;
        _critical = "native-operation-blocked:" + Key(__originalMethod); Count(Key(__originalMethod)); return false;
    }
    private static bool BlockEligibility(UnityEngine.Component __instance, ref bool __result, System.Reflection.MethodBase __originalMethod)
    {
        if (_port?.Owns(__instance) != true) return true;
        Count(Key(__originalMethod)); __result = false; return false;
    }
    private static bool BlockSaveable(Il2CppScheduleOne.Persistence.ISaveable __0)
    {
        if (_port?.OwnsSaveable(__0) != true) return true;
        _critical = "register-saveable-blocked"; Count(_critical); return false;
    }
    private static bool BlockSaveRequest(Il2CppScheduleOne.Persistence.SaveRequest __0)
    {
        if (__0 == null || _port?.OwnsSaveable(__0.Saveable) != true) return true;
        _critical = "queue-save-request-blocked"; Count(_critical); return false;
    }
    private static bool BeforeSave()
    {
        // No native-object return method is patched. Both void Save entry points retire the
        // lease permanently for this generation before native managers can enumerate it.
        if (_lease == null || (_lease.CanStartNextGeneration && _result?.Status != "entered" && _result?.Status != "active")) return true;
        bool clean = End("native-save-boundary");
        if (!clean) MelonLoader.MelonLogger.Error("TownHall activation: native save blocked because own diagnostic cleanup is unconfirmed. Restart from the recorded checkpoint after inspection.");
        return clean;
    }
    private static void PreLoad()
    {
        End("pre-load"); _loaded = false; _due = -1; _loadTicket = true;
    }
    private static void Leaving() { End("pre-scene-change"); _loaded = false; _due = -1; _loadTicket = false; }
    internal static void SceneUnloaded() => Leaving();
    private static void Loaded()
    {
        if (!_ready || _quarantined || !_loadTicket) return;
        _loadTicket = false;
        if (_lease != null && !_lease.CanStartNextGeneration) { _quarantined = true; return; }
        _generation++; _loaded = true; _due = UnityEngine.Time.realtimeSinceStartup + 6;
        _lease = null; _port = null; _result = null; _context = null; _critical = ""; _phase = "waiting";
        Calls.Clear(); History.Clear();
    }
    private static ConstructionContext Context()
    {
        var load = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
        var info = load?.ActiveSaveInfo;
        var lobby = Il2CppScheduleOne.DevUtilities.Singleton<Il2CppScheduleOne.Networking.Lobby>.Instance;
        bool single = S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer() && S1Mods.Shared.NetworkGuard.IsAlive(lobby) &&
            !lobby.IsInLobby && Il2CppScheduleOne.PlayerScripts.Player.PlayerList?.Count == 1;
        return new ConstructionContext(_loaded && S1Mods.Shared.NetworkGuard.IsInMainScene, single, _hallReady?.Invoke() == true,
            OwnersPresent() && SaveIdle(), info?.SavePath, load?.LoadedGameFolderPath, info?.OrganisationName);
    }
    private static bool SaveIdle()
    {
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        return S1Mods.Shared.NetworkGuard.IsAlive(save) && !save.IsSaving;
    }
    internal static void Tick()
    {
        if (!_ready || _quarantined || !_loaded || Environment.CurrentManagedThreadId != _thread) return;
        try
        {
            float now = UnityEngine.Time.realtimeSinceStartup;
            if (_due >= 0 && now >= _due)
            {
                _due = -1; _context = Context(); _port = new NativeConstructionPort(() => true);
                _lease = new BusinessActivationLease(_generation, Expected); _phase = "activating";
                _result = _lease.Begin(_context, _port, UnityEngine.Time.frameCount);
                AcceptResult(); _completeDue = now + 1; _monitorDue = now + 2;
                if (!string.IsNullOrEmpty(_critical)) Fail(_critical); else Record("begin");
            }
            if (_lease == null || _result == null || (_result.Status != "entered" && _result.Status != "active")) return;
            if (!string.IsNullOrEmpty(_critical)) { Fail(_critical); return; }
            if (_result.Status == "entered" && now >= _completeDue)
            {
                string? missing = RequiredCalls.FirstOrDefault(key => !Calls.TryGetValue(key, out int count) || count != 1);
                if (missing != null) { Fail("lifecycle-sighting-missing:" + missing); return; }
                if (!Calls.ContainsKey("Property.ShouldSave") || !Calls.ContainsKey("Property.CanBePurchased"))
                { Fail("eligibility-guard-sighting-missing"); return; }
                if (!SameContext()) { Fail("world-context-changed"); return; }
                _result = _lease.Complete(UnityEngine.Time.frameCount); AcceptResult();
                Record("following-frame-check");
            }
            if (_result.Status == "active" && now >= _monitorDue)
            {
                _monitorDue = now + 1;
                if (!SameContext()) { Fail("world-context-changed"); return; }
                var read = ((IBusinessActivationPort)_port!).Capture();
                if (!read.RootAlive || !read.RootActive || !read.ReferencesBound || !read.Unowned ||
                    Expected.Any(e => !read.OwnedCounts.TryGetValue(e.Key, out int count) || count != e.Value))
                    Fail("active-readback-changed");
            }
        }
        catch (Exception ex) { Fail("runtime:" + ex.GetType().Name + ":" + ex.Message); }
    }
    private static bool SameContext()
    {
        var current = Context();
        return ConstructionGate.Check(current) == null && _context != null &&
            string.Equals(Path.GetFullPath(current.ActiveSavePath!), Path.GetFullPath(_context.ActiveSavePath!), StringComparison.OrdinalIgnoreCase);
    }
    private static void AcceptResult()
    {
        _phase = _result!.Status;
        if (_result.Status == "failed" && _lease?.CanStartNextGeneration != true) _quarantined = true;
    }
    internal static string EnterCleanupPhase()
    {
        string previous = _phase; _phase = "cleanup"; return previous;
    }
    internal static void RestoreCleanupPhase(string previous) => _phase = previous;
    private static void Fail(string reason)
    {
        _phase = "cleanup";
        try { if (_lease != null) _result = _lease.Fail(reason); }
        catch (Exception ex) { _quarantined = true; MelonLoader.MelonLogger.Error("TownHall activation failure cleanup: " + ex); }
        _phase = "failed"; _quarantined |= _lease == null || !_lease.CanStartNextGeneration;
        MelonLoader.MelonLogger.Warning("TownHall activation failed: " + reason); Record(reason);
    }
    private static bool End(string reason)
    {
        if (_lease == null) return true;
        if (_ending || Environment.CurrentManagedThreadId != _thread) { _quarantined = true; return false; }
        _ending = true; _phase = "cleanup";
        try
        {
            _result = _lease.Leave(); _phase = _result.Status;
            _quarantined |= !_lease.CanStartNextGeneration;
            Record(reason); return _lease.CanStartNextGeneration;
        }
        catch (Exception ex) { _quarantined = true; MelonLoader.MelonLogger.Error("TownHall activation cleanup: " + ex); return false; }
        finally { _ending = false; }
    }
    private static void Record(string reason)
    {
        if (string.IsNullOrEmpty(_directory) || _generation == 0) return;
        try
        {
            if (History.Count >= 16) { _quarantined = true; throw new InvalidOperationException("activation-history-limit"); }
            History.Add(new { CapturedUtc = DateTime.UtcNow, Reason = reason, Result = _result });
            var report = new
            {
                Schema = 1, Mode = "controlled-native-activation", RunId = _run, Generation = _generation,
                CapturedUtc = DateTime.UtcNow, AssemblySha256 = _assemblyHash, NativeProxySha256 = NativeHash,
                S1ApiSha256 = ApiHash, FishNetSha256 = FishNetHash, Context = _context,
                NativeLifecycleBodiesAllowed = false, NativeSaveWriteAllowed = false, PurchaseAllowed = false,
                LaunderingAllowed = false, WorldPublicationAllowed = true, Quarantined = _quarantined,
                Guards = Targets.Select(m => new { Method = Key(m), OwnerPresent = HarmonyLib.Harmony.GetPatchInfo(m)?.Owners.Contains(Owner) == true }),
                BlockedCalls = new Dictionary<string, int>(Calls), CriticalBoundary = _critical,
                Result = _result, Transitions = History.ToArray(),
                Limitation = "Own active shell and explicit four-list registration only; native Awake/Start bodies suppressed. No FishNet spawn, station, native persistence or purchase acceptance. Standard Save entry points retire this generation; arbitrary direct manager WriteData calls are not intercepted. Unconfirmed cleanup blocks native Save; other mods' save postfixes are outside this contract. Time/Money subscriptions and unlisted native registries are not inventoried."
            };
            string path = Path.Combine(_directory, _run + "-g" + _generation.ToString("D4") + ".json");
            File.WriteAllText(path + ".tmp", JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(path + ".tmp", path, true);
            MelonLoader.MelonLogger.Msg("TownHall activation " + (_result?.Status ?? "failed") + " (" + reason + "): " + path);
        }
        catch (Exception ex)
        {
            _quarantined = true;
            MelonLoader.MelonLogger.Error("TownHall activation report failed: " + ex);
            // No further publication without durable diagnostics. Avoid recursive report writes.
            if (!_ending && _lease != null)
            {
                _ending = true; _phase = "cleanup";
                try { _result = _lease.Fail("report-write-failed"); } catch { }
                finally { _ending = false; }
            }
        }
    }
    internal static void Stop()
    {
        _ready = false; _loaded = false; _due = -1;
        S1API.Lifecycle.GameLifecycle.OnPreLoad -= PreLoad;
        S1API.Lifecycle.GameLifecycle.OnLoadComplete -= Loaded;
        S1API.Lifecycle.GameLifecycle.OnPreSceneChange -= Leaving;
        if (End("shutdown"))
            try { _harmony?.UnpatchSelf(); } catch (Exception ex) { MelonLoader.MelonLogger.Warning("TownHall activation unpatch: " + ex); }
    }
    private static string Hash(string path) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
}
#endif

#if TOWNHALL_BUSINESS_ACTIVATION
// Activation uses the existing owned shell; this interface exclusively owns registration.
internal sealed partial class NativeConstructionPort : IBusinessActivationPort
{
    private const int ActivationRegistryLimit = 8192;
    // Retain after destruction: post-cleanup verification must still identify stale entries.
    private System.IntPtr _activationBusinessPointer;

    internal bool OwnsSaveable(Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase? value) =>
        _rootPointer != System.IntPtr.Zero && _activationBusinessPointer != System.IntPtr.Zero && !ReferenceEquals(value, null) &&
        value.Pointer == _activationBusinessPointer;

    void IBusinessActivationPort.Prepare()
    {
        try { EnsureActivationSaveIdle(); Prepare(); }
        finally
        {
            if (!ReferenceEquals(_business, null)) _activationBusinessPointer = _business.Pointer;
        }
    }

    public RegistrationSnapshot Capture()
    {
        var foreign = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal);
        var owned = new System.Collections.Generic.Dictionary<string, int>(System.StringComparer.Ordinal);
        CaptureActivationRegistries(foreign, owned);
        bool alive = !ReferenceEquals(_root, null) && !Destroyed(_root);
        if (!alive) return new(foreign, owned, false, false, false, false);
        var readback = Inspect();
        bool unspawned = _network != null && !_network.IsSpawned;
        bool refs = readback.ReferencesBound && readback.NoForeignComponents && unspawned &&
            string.Equals(readback.PropertyCode, Code, System.StringComparison.Ordinal) &&
            !ReferenceEquals(_business, null) && _business.Pointer == _activationBusinessPointer;
        bool active = _root!.activeSelf && _root.activeInHierarchy && _business != null &&
            _business.enabled && unspawned;
        return new(foreign, owned, true, active, refs, readback.Unowned);
    }

    void IBusinessActivationPort.Activate()
    {
        var before = Capture();
        if (!before.RootAlive || before.RootActive || !before.ReferencesBound || !before.Unowned ||
            before.OwnedCounts.Values.Any(count => count != 0))
            throw new System.InvalidOperationException("activation-staging-state-invalid");
        EnsureActivationSaveIdle();
        _root!.SetActive(true);
        // Exercise the primitive native-return guards; the runtime also requires sightings.
        if (_business!.ShouldSave() || _business.CanBePurchased())
            throw new System.InvalidOperationException("activation-eligibility-guard-not-effective");
    }

    void IBusinessActivationPort.Register()
    {
        var before = Capture();
        if (!before.RootAlive || !before.RootActive || !before.ReferencesBound || !before.Unowned ||
            before.OwnedCounts.Values.Any(count => count != 0))
            throw new System.InvalidOperationException("activation-registration-state-invalid");
        var business = _business ?? throw new System.InvalidOperationException("activation-business-missing");
        EnsureActivationSaveIdle();
        // Each successful Add is independently recoverable by pointer-based Cleanup.
        Il2CppScheduleOne.Property.Property.Properties.Add(business);
        Il2CppScheduleOne.Property.Property.UnownedProperties.Add(business);
        Il2CppScheduleOne.Property.Business.Businesses.Add(business);
        Il2CppScheduleOne.Property.Business.UnownedBusinesses.Add(business);
    }

    void IBusinessActivationPort.Cleanup()
    {
        string previous = BusinessActivationRuntime.EnterCleanupPhase();
        try { DetachAndDestroy(); }
        finally { BusinessActivationRuntime.RestoreCleanupPhase(previous); }
    }

    private void DetachAndDestroy()
    {
        var failures = new System.Collections.Generic.List<System.Exception>();
        void Remove(System.Action action)
        {
            try { action(); }
            catch (System.Exception ex) { failures.Add(ex); }
        }
        Remove(() => RemoveActivationEntries(Il2CppScheduleOne.Property.Property.Properties));
        Remove(() => RemoveActivationEntries(Il2CppScheduleOne.Property.Property.OwnedProperties));
        Remove(() => RemoveActivationEntries(Il2CppScheduleOne.Property.Property.UnownedProperties));
        Remove(() => RemoveActivationEntries(Il2CppScheduleOne.Property.Business.Businesses));
        Remove(() => RemoveActivationEntries(Il2CppScheduleOne.Property.Business.OwnedBusinesses));
        Remove(() => RemoveActivationEntries(Il2CppScheduleOne.Property.Business.UnownedBusinesses));
        Remove(() =>
        {
            var save = ActivationSaveManager();
            Remove(() => RemoveActivationEntries(save.Saveables));
            Remove(() => RemoveActivationEntries(save.BaseSaveables));
            Remove(() => RemoveActivationEntries(save.CompletedSaveables));
            Remove(() => RemoveActivationEntries(save.QueuedSaveRequests,
                request => request.Saveable?.Pointer ?? System.IntPtr.Zero));
        });
        // Do not destroy an object while an unreadable registry may still reference it.
        if (failures.Count != 0) throw new System.AggregateException("activation-detach-incomplete", failures);
        var foreign = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal);
        var owned = new System.Collections.Generic.Dictionary<string, int>(System.StringComparer.Ordinal);
        CaptureActivationRegistries(foreign, owned);
        if (owned.Values.Any(count => count != 0))
            throw new System.InvalidOperationException("activation-owned-registration-remains");
        Cleanup();
    }

    bool IBusinessActivationPort.IsClean
    {
        get
        {
            // The shell check covers every owned child, including failed-parenting orphans.
            if (!IsClean) return false;
            var snapshot = Capture();
            return !snapshot.RootAlive && !snapshot.RootActive &&
                snapshot.OwnedCounts.Values.All(count => count == 0);
        }
    }

    private static Il2CppScheduleOne.Persistence.SaveManager ActivationSaveManager()
    {
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save))
            throw new System.InvalidOperationException("activation-save-manager-unavailable");
        return save;
    }

    private void CaptureActivationRegistries(System.Collections.Generic.Dictionary<string, string> foreign,
        System.Collections.Generic.Dictionary<string, int> owned)
    {
        var save = ActivationSaveManager();
        SnapshotActivationList("properties", Il2CppScheduleOne.Property.Property.Properties, foreign, owned);
        SnapshotActivationList("properties-owned", Il2CppScheduleOne.Property.Property.OwnedProperties, foreign, owned);
        SnapshotActivationList("properties-unowned", Il2CppScheduleOne.Property.Property.UnownedProperties, foreign, owned);
        SnapshotActivationList("businesses", Il2CppScheduleOne.Property.Business.Businesses, foreign, owned);
        SnapshotActivationList("businesses-owned", Il2CppScheduleOne.Property.Business.OwnedBusinesses, foreign, owned);
        SnapshotActivationList("businesses-unowned", Il2CppScheduleOne.Property.Business.UnownedBusinesses, foreign, owned);
        SnapshotActivationList("saveables", save.Saveables, foreign, owned);
        SnapshotActivationList("base-saveables", save.BaseSaveables, foreign, owned);
        SnapshotActivationList("completed-saveables", save.CompletedSaveables, foreign, owned);
        SnapshotActivationList("save-requests", save.QueuedSaveRequests, foreign, owned,
            request => request.Saveable?.Pointer ?? System.IntPtr.Zero);
    }

    private static void EnsureActivationSaveIdle()
    {
        if (ActivationSaveManager().IsSaving)
            throw new System.InvalidOperationException("activation-save-in-progress");
    }

    private void SnapshotActivationList<T>(string key, il2cpp::Il2CppSystem.Collections.Generic.List<T> list,
        System.Collections.Generic.Dictionary<string, string> foreign,
        System.Collections.Generic.Dictionary<string, int> owned,
        System.Func<T, System.IntPtr>? ownerPointer = null)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        int count = ActivationListCount(list);
        int ownCount = 0, foreignCount = 0;
        var values = new System.Text.StringBuilder(list.Pointer.ToInt64().ToString("X"));
        for (int i = 0; i < count; i++)
        {
            T? item = list[i];
            if (!ReferenceEquals(item, null) && item.WasCollected)
                throw new System.InvalidOperationException("activation-registry-item-unreadable:" + key);
            var pointer = ReferenceEquals(item, null) ? System.IntPtr.Zero : item!.Pointer;
            var target = ReferenceEquals(item, null) ? System.IntPtr.Zero : ownerPointer == null ? pointer : ownerPointer(item!);
            if (_activationBusinessPointer != System.IntPtr.Zero && target == _activationBusinessPointer) { ownCount++; continue; }
            foreignCount++;
            values.Append('|').Append(ReferenceEquals(item, null) ? "null" : pointer.ToInt64().ToString("X"));
            // Queue records also preserve their foreign saveable target, not just request identity.
            if (ownerPointer != null) values.Append('>').Append(target.ToInt64().ToString("X"));
        }
        if (count != list.Count) throw new System.InvalidOperationException("activation-registry-changed-during-read:" + key);
        foreign.Add(key, foreignCount + ":" + System.Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(values.ToString()))).ToLowerInvariant());
        owned.Add(key, ownCount);
    }

    private void RemoveActivationEntries<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list,
        System.Func<T, System.IntPtr>? ownerPointer = null)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        int expectedCount = ActivationListCount(list);
        for (int i = expectedCount - 1; i >= 0; i--)
        {
            if (list.Count != expectedCount) throw new System.InvalidOperationException("activation-registry-changed-during-detach");
            T? item = list[i];
            if (ReferenceEquals(item, null)) continue;
            if (item.WasCollected) throw new System.InvalidOperationException("activation-registry-item-unreadable");
            var target = ownerPointer == null ? item.Pointer : ownerPointer(item);
            if (_activationBusinessPointer == System.IntPtr.Zero || target != _activationBusinessPointer) continue;
            list.RemoveAt(i);
            expectedCount--;
        }
        if (list.Count != expectedCount) throw new System.InvalidOperationException("activation-registry-detach-count-mismatch");
    }

    private static int ActivationListCount<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        if (ReferenceEquals(list, null) || list.WasCollected || list.Pointer == System.IntPtr.Zero)
            throw new System.InvalidOperationException("activation-registry-unreadable");
        int count = list.Count;
        if (count < 0 || count > ActivationRegistryLimit)
            throw new System.InvalidOperationException("activation-registry-over-budget");
        return count;
    }
}

#endif
#endif

#if TOWNHALL_BUSINESS_SUBTYPE
// Intentionally no RegisterTypeInIl2Cpp attribute: registration belongs to the gated trial.
public sealed class ProbeBusinessSubtype : Il2CppScheduleOne.Property.Business
{
    private readonly Dictionary<string, int> _calls = new(StringComparer.Ordinal);
    public ProbeBusinessSubtype(IntPtr pointer) : base(pointer) { }
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    private void Count(string name) => _calls[name] = _calls.TryGetValue(name, out int count) ? count + 1 : 1;
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal Dictionary<string, int> Counters() => new(_calls, StringComparer.Ordinal);
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal int Calls(string name) => _calls.TryGetValue(name, out int count) ? count : 0;
    public override bool ShouldSave() { Count(nameof(ShouldSave)); return false; }
    public override bool CanBePurchased() { Count(nameof(CanBePurchased)); return false; }
    public override void Awake() => Count(nameof(Awake));
    public override void Start() => Count(nameof(Start));
    public override void OnDestroy() => Count(nameof(OnDestroy));
    public override void FixedUpdate() => Count(nameof(FixedUpdate));
    public override void InitializeSaveable() => Count(nameof(InitializeSaveable));
    public override string GetSaveString() { Count(nameof(GetSaveString)); return ""; }
    public override il2cpp::Il2CppSystem.Collections.Generic.List<string> WriteData(string parentFolderPath)
    {
        Count(nameof(WriteData));
        return new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
    }
    public override void DeleteUnapprovedFiles(string parentFolderPath) => Count(nameof(DeleteUnapprovedFiles));
    public override void NetworkInitialize___Early() => Count(nameof(NetworkInitialize___Early));
    public override void NetworkInitialize__Late() => Count(nameof(NetworkInitialize__Late));
    public override void NetworkInitializeIfDisabled() => Count(nameof(NetworkInitializeIfDisabled));
    public override void Reset() => Count(nameof(Reset));
    public override void OnValidate() => Count(nameof(OnValidate));
}

internal static class BusinessSubtypeRuntime
{
#if TOWNHALL_BUSINESS_MANAGER
    private const bool ProbeInterface = false, ProbeManager = false, ProbeSave = true;
    private const string ExpectedOrganisation = "Dominik", ExpectedSlot = "SaveGame_1";
    private const string ProbeMode = "inactive-saveable-save", ReportFolder = "BusinessManager";
#elif TOWNHALL_BUSINESS_INTERFACE
    private const bool ProbeInterface = true, ProbeManager = false, ProbeSave = false;
    private const string ExpectedOrganisation = "TownHallTest", ExpectedSlot = "SaveGame_2";
    private const string ProbeMode = "inactive-own-business-interface-dispatch", ReportFolder = "BusinessInterface";
#else
    private const bool ProbeInterface = false, ProbeManager = false, ProbeSave = false;
    private const string ExpectedOrganisation = "TownHallTest", ExpectedSlot = "SaveGame_2";
    private const string ProbeMode = "inactive-own-business-native-dispatch", ReportFolder = "BusinessSubtype";
#endif
    internal const string NativeHash = "346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8";
    internal const string FishNetHash = "0acb5c70078f5be6e88574be17b675657fc50898eb3dd264a3b98a54545e24ec";
    internal const string ApiHash = "d751a3cb66003f7a616eb9f8054daf34da0d9d7f23416a5005a586738a680852";
    internal const string InteropHash = "3878ce0ba0c60385f76f2ba37e07ad0ee3649d019703aec8695db3402abeedd7";
    internal const string HarmonyHash = "6c898933b52149e8bcf6722305c7e4d94add47c9e53b364096f91721624c0877";
    private static readonly BusinessSubtypeTrial Trial = new();
    private static Func<bool>? _hallReady;
    private static bool _ready, _loaded;
    private static int _thread;
    private static long _generation;
    private static NativeSubtypePort? _port;
    private static float _due = -1;
    private static string _directory = "", _assemblyHash = "";

    internal static void Start(Func<bool> hallReady)
    {
        _thread = Environment.CurrentManagedThreadId; _hallReady = hallReady;
        try
        {
            _assemblyHash = Hash(typeof(Mod).Assembly.Location);
            if (Hash(typeof(Il2CppScheduleOne.Property.Business).Assembly.Location) != NativeHash ||
                Hash(typeof(Il2CppFishNet.Object.NetworkBehaviour).Assembly.Location) != FishNetHash ||
                Hash(typeof(S1API.Lifecycle.GameLifecycle).Assembly.Location) != ApiHash ||
                Hash(typeof(Il2CppInterop.Runtime.Injection.ClassInjector).Assembly.Location) != InteropHash ||
                Hash(typeof(HarmonyLib.Harmony).Assembly.Location) != HarmonyHash)
                throw new InvalidOperationException("subtype-dependency-hash-mismatch");
            _directory = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "TownHallInterior", ReportFolder);
            Directory.CreateDirectory(_directory);
            S1API.Lifecycle.GameLifecycle.OnLoadComplete += Loaded;
            S1API.Lifecycle.GameLifecycle.OnPreSceneChange += Leaving;
            S1API.Lifecycle.GameLifecycle.OnPreLoad += Leaving;
            _ready = true;
            MelonLoader.MelonLogger.Msg(ProbeSave
                ? "TownHall save probe armed for Dominik / SaveGame_1 only; one register, two Save() calls, own writes do not call base."
                : "TownHall subtype: gated inactive native-dispatch test armed; no game-method detours.");
        }
        catch (Exception ex) { _ready = false; MelonLoader.MelonLogger.Warning("TownHall subtype unavailable: " + ex.GetType().Name); }
    }
    private static void Loaded()
    {
        if (!_ready || Environment.CurrentManagedThreadId != _thread) return;
        _generation++; _loaded = true; _due = UnityEngine.Time.realtimeSinceStartup + 6;
    }
    internal static void Leaving() { _generation++; _loaded = false; _due = -1; }
    internal static void Tick()
    {
        if (!_ready || !_loaded || _due < 0 || UnityEngine.Time.realtimeSinceStartup < _due) return;
        _due = -1;
        try
        {
            if (Environment.CurrentManagedThreadId != _thread) throw new InvalidOperationException("wrong-thread");
            var load = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
            var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
            var info = load?.ActiveSaveInfo;
            var lobby = Il2CppScheduleOne.DevUtilities.Singleton<Il2CppScheduleOne.Networking.Lobby>.Instance;
            bool singleplayer = S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer() && S1Mods.Shared.NetworkGuard.IsAlive(lobby) &&
                !lobby.IsInLobby && Il2CppScheduleOne.PlayerScripts.Player.PlayerList?.Count == 1;
            var context = new ConstructionContext(_loaded && S1Mods.Shared.NetworkGuard.IsInMainScene &&
                S1Mods.Shared.NetworkGuard.IsAlive(load) && S1Mods.Shared.NetworkGuard.IsAlive(save) && !save.IsSaving,
                singleplayer, _hallReady?.Invoke() == true, _ready, info?.SavePath, load?.LoadedGameFolderPath, info?.OrganisationName);
            long generation = _generation;
            bool ContextValid() => _ready && _loaded && generation == _generation &&
                Environment.CurrentManagedThreadId == _thread && ConstructionGate.Check(CurrentContext(), ExpectedOrganisation, ExpectedSlot) == null;
            var port = _port ?? new NativeSubtypePort(ContextValid, ProbeInterface);
            if (ConstructionGate.Check(context, ExpectedOrganisation, ExpectedSlot) == null) _port = port; // Retain unknown cleanup handles, never retry them.
            var result = Trial.Run(context, port, ContextValid, ProbeInterface, ProbeManager, ExpectedOrganisation, ExpectedSlot, ProbeSave);
            var report = new
            {
                Schema = 2, Mode = ProbeMode, CapturedUtc = DateTime.UtcNow,
                AssemblySha256 = _assemblyHash, NativeProxySha256 = NativeHash, FishNetSha256 = FishNetHash,
                S1ApiSha256 = ApiHash, InteropRuntimeSha256 = InteropHash, HarmonySha256 = HarmonyHash, Target = ExpectedOrganisation + " / " + ExpectedSlot,
                WorldPublicationAllowed = false, NativeSaveWriteAllowed = ProbeSave, GameMethodDetoursAdded = false,
                Context = context, Generation = generation, Result = result, Native = port.Details,
                NativeEvidenceFromThisAttempt = result.RegistrationAttempted,
                Limitation = "Type registration persists until process exit. CleanupComplete covers the own Unity root and acquired Business component. " +
                    "Own callback counters and class/vtable snapshots are bounded observations, not a proof of every native side effect. " +
                    (ProbeSave ? "The added route registers one inactive object, calls Save() once, removes it, then calls Save() again if the first call has returned and saving is idle. Own GetSaveString, WriteData and DeleteUnapprovedFiles do not call base. " :
                    ProbeInterface ? "The added route is only ISaveable.InitializeSaveable to the own no-base callback. Other interface methods and native manager call paths remain untested. " :
                    "Dispatch proves the Property proxy/class virtual route only; ISaveable/interface dispatch and native manager call paths remain untested. ") +
                    "A skipped result retains historical Native details; it performs no new cleanup or readback. " +
                    (ProbeSave ? "No activation, purchase, spawn or gameplay acceptance. A successful report is not proof the reload restored a business. " :
                    "No activation, registry publication, original lifecycle calls, serialization, purchase, save, spawn or gameplay acceptance.")
            };
            string path = Path.Combine(_directory, DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffZ") + ".json");
            File.WriteAllText(path + ".tmp", JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(path + ".tmp", path, true);
            MelonLoader.MelonLogger.Msg("TownHall subtype " + result.Status + ": " + path);
        }
        catch (Exception ex) { _ready = false; MelonLoader.MelonLogger.Warning("TownHall subtype stopped: " + ex.GetType().Name); }
    }
    internal static void Stop()
    {
        Leaving(); _ready = false;
        S1API.Lifecycle.GameLifecycle.OnLoadComplete -= Loaded;
        S1API.Lifecycle.GameLifecycle.OnPreSceneChange -= Leaving;
        S1API.Lifecycle.GameLifecycle.OnPreLoad -= Leaving;
    }
    private static ConstructionContext CurrentContext()
    {
        var load = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        var info = load?.ActiveSaveInfo;
        var lobby = Il2CppScheduleOne.DevUtilities.Singleton<Il2CppScheduleOne.Networking.Lobby>.Instance;
        return new(_loaded && S1Mods.Shared.NetworkGuard.IsInMainScene && S1Mods.Shared.NetworkGuard.IsAlive(load) &&
            S1Mods.Shared.NetworkGuard.IsAlive(save) && !save.IsSaving,
            S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer() && S1Mods.Shared.NetworkGuard.IsAlive(lobby) && !lobby.IsInLobby &&
            Il2CppScheduleOne.PlayerScripts.Player.PlayerList?.Count == 1, _hallReady?.Invoke() == true, _ready,
            info?.SavePath, load?.LoadedGameFolderPath, info?.OrganisationName);
    }
    private static string Hash(string path) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
}

internal sealed unsafe class ManagedSubtypeTypeResolution : ITypeResolutionPatchPort
{
    private const string Owner = "TownHallInterior.BusinessSubtype.TypeResolution";
    private static ManagedSubtypeTypeResolution? _current;
    private static readonly Type PointerType = typeof(Il2CppInterop.Runtime.Runtime.Il2CppTypeStruct).MakePointerType();
    private readonly System.Reflection.MethodInfo _nameHelper, _systemResolver, _registrar, _namePostfix, _entryPrefix, _entryFinalizer;
    private readonly HarmonyLib.Harmony _harmony = new(Owner);
    private readonly SubtypeTypeResolutionLease _lease = new();
    private readonly List<object> _inventory = new();
    private SubtypeTypeNamePolicy? _policy;
    private IntPtr _networkClass, _networkType;
    private bool _originalResolved, _postScopeReadback, _preflightComplete;
    private string? _postScopeFailure, _qualifiedProxy;
    public object Details => new { Mapping = SubtypeTypeNamePolicy.NativeName, NativeClass = _networkClass.ToInt64().ToString("X"),
        ProxyQualifiedName = _qualifiedProxy, ManagedPatchOwner = Owner, ManagedTargets = new[] { _nameHelper.ToString(), _registrar.ToString() },
        ParameterInventory = _inventory.ToArray(), PreflightComplete = _preflightComplete, OriginalResolverAlreadyResolves = _originalResolved,
        PostScopeOriginalResolutionConfirmed = _postScopeReadback, PostScopeFailure = _postScopeFailure, Outcome = _lease.Outcome };

    internal ManagedSubtypeTypeResolution()
    {
        var injector = typeof(Il2CppInterop.Runtime.Injection.ClassInjector);
        _nameHelper = injector.GetMethod("GetIl2CppTypeFullName", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic,
            null, new[] { PointerType }, null) ?? throw new MissingMethodException("ClassInjector.GetIl2CppTypeFullName");
        _systemResolver = injector.GetMethod("SystemTypeFromIl2CppType", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic,
            null, new[] { PointerType }, null) ?? throw new MissingMethodException("ClassInjector.SystemTypeFromIl2CppType");
        _registrar = injector.GetMethod("RegisterTypeInIl2Cpp", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
            null, new[] { typeof(Type), typeof(Il2CppInterop.Runtime.Injection.RegisterTypeOptions) }, null)
            ?? throw new MissingMethodException("ClassInjector.RegisterTypeInIl2Cpp(Type, options)");
        if (_nameHelper.ReturnType != typeof(string) || _systemResolver.ReturnType != typeof(Type) || _registrar.ReturnType != typeof(void) ||
            _nameHelper.GetMethodBody() == null || _systemResolver.GetMethodBody() == null || _registrar.GetMethodBody() == null || _registrar.IsGenericMethod)
            throw new InvalidOperationException("type-resolution-managed-contract-mismatch");
        _namePostfix = Callback(nameof(RewriteName)); _entryPrefix = Callback(nameof(Entering)); _entryFinalizer = Callback(nameof(Exited));
    }
    internal void Register(IntPtr businessClass, IntPtr networkClass, Action register)
    {
        Preflight(businessClass, networkClass);
        _preflightComplete = true;
        if (Interlocked.CompareExchange(ref _current, this, null) != null)
            throw new InvalidOperationException("type-resolution-concurrent-scope");
        Exception? failure = null;
        try { _lease.Run(_policy!, this, register); }
        catch (Exception ex) { failure = ex; }
        finally { Interlocked.CompareExchange(ref _current, null, this); }
        try
        {
            _postScopeReadback = RawName(_networkType) == SubtypeTypeNamePolicy.NativeName &&
                (ResolveOriginalRoute() != null) == _originalResolved;
            if (!_postScopeReadback) throw new InvalidOperationException("unscoped-type-resolution-changed");
        }
        catch (Exception ex)
        {
            _postScopeFailure = ex.GetType().Name + ":" + ex.Message[..Math.Min(160, ex.Message.Length)];
            failure = failure == null ? ex : new AggregateException(failure, ex);
        }
        if (failure != null) System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
    }
    private void Preflight(IntPtr businessClass, IntPtr networkClass)
    {
        var proxy = typeof(Il2CppFishNet.Connection.NetworkConnection);
        string qualified = proxy.AssemblyQualifiedName ?? throw new InvalidOperationException("proxy-qualified-name-missing");
        _qualifiedProxy = qualified;
        if (businessClass == IntPtr.Zero || networkClass == IntPtr.Zero || proxy.FullName != SubtypeTypeNamePolicy.ProxyTypeName ||
            proxy.Assembly != typeof(Il2CppFishNet.Object.NetworkBehaviour).Assembly || proxy.Assembly.GetName().Name != SubtypeTypeNamePolicy.ProxyAssemblyName ||
            Type.GetType(qualified, false) != proxy)
            throw new InvalidOperationException("type-resolution-proxy-identity-mismatch");
        _networkClass = networkClass;
        _networkType = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_type(networkClass);
        if (_networkType == IntPtr.Zero || RawName(_networkType) != SubtypeTypeNamePolicy.NativeName ||
            Il2CppInterop.Runtime.IL2CPP.il2cpp_class_from_type(_networkType) != networkClass)
            throw new InvalidOperationException("type-resolution-native-identity-mismatch");
        Type? original = ResolveOriginalRoute();
        _originalResolved = original != null;
        var native = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)businessClass);
        if (native.VtableCount is 0 or > 128 || native.VTable == IntPtr.Zero) throw new InvalidOperationException("type-resolution-vtable-budget");
        var slots = (Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)native.VTable;
        int matchingParameters = 0;
        for (int slot = 0; slot < native.VtableCount; slot++)
        {
            if (slots[slot].method == null) throw new InvalidOperationException("type-resolution-method-missing");
            var method = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap(slots[slot].method);
            if (method.ParametersCount > 16) throw new InvalidOperationException("type-resolution-parameter-budget");
            for (int parameter = 0; parameter < method.ParametersCount; parameter++)
            {
                if (_inventory.Count >= 256) throw new InvalidOperationException("type-resolution-inventory-budget");
                var type = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap(method.Parameters, parameter).ParameterType;
                string raw = RawName((IntPtr)type);
                _inventory.Add(new { Slot = slot, Method = System.Runtime.InteropServices.Marshal.PtrToStringUTF8(method.Name), Parameter = parameter, NativeName = raw });
                if (!raw.EndsWith(", FishNet.Runtime", StringComparison.Ordinal)) continue;
                if (raw != SubtypeTypeNamePolicy.NativeName || Il2CppInterop.Runtime.IL2CPP.il2cpp_class_from_type((IntPtr)type) != networkClass)
                    throw new InvalidOperationException("type-resolution-unapproved-fishnet-signature");
                matchingParameters++;
            }
        }
        if (matchingParameters == 0) throw new InvalidOperationException("type-resolution-expected-signature-absent");
        _policy = new(Environment.CurrentManagedThreadId, networkClass.ToInt64(), qualified, _originalResolved);
    }
    private string RawName(IntPtr nativeType) => (string?)_nameHelper.Invoke(null,
        new[] { System.Reflection.Pointer.Box((void*)nativeType, PointerType) }) ?? throw new InvalidOperationException("native-type-name-missing");
    private Type? ResolveOriginalRoute()
    {
        var proxy = typeof(Il2CppFishNet.Connection.NetworkConnection);
        Type? raw = Type.GetType(SubtypeTypeNamePolicy.NativeName, false);
        Type? prefixed = Type.GetType("Il2Cpp" + SubtypeTypeNamePolicy.NativeName, false);
        if ((raw != null && raw != proxy) || (prefixed != null && prefixed != proxy))
            throw new InvalidOperationException("type-resolution-original-resolves-foreign");
        try
        {
            var resolved = _systemResolver.Invoke(null, new[] { System.Reflection.Pointer.Box((void*)_networkType, PointerType) });
            if (!ReferenceEquals(resolved, proxy)) throw new InvalidOperationException("original-resolver-type-mismatch");
            return proxy;
        }
        catch (System.Reflection.TargetInvocationException ex) when (ex.InnerException is NullReferenceException failure &&
            failure.Message == "Couldn't find System.Type for Il2Cpp type: " + SubtypeTypeNamePolicy.NativeName && raw == null && prefixed == null)
        {
            return null; // Expected installed failure, outside any synthetic registration scope.
        }
    }
    private static System.Reflection.MethodInfo Callback(string name) => typeof(ManagedSubtypeTypeResolution).GetMethod(name,
        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new MissingMethodException(name);
    private static void Entering(Type __0, out SubtypeTypeNamePolicy? __state)
    {
        __state = null;
        var policy = _current?._policy;
        if (policy?.Enter(Environment.CurrentManagedThreadId, __0 == typeof(ProbeBusinessSubtype)) == true) __state = policy;
    }
    private static Exception? Exited(Exception? __exception, SubtypeTypeNamePolicy? __state)
    {
        __state?.Exit(Environment.CurrentManagedThreadId, true);
        return __exception;
    }
    private static void RewriteName(Il2CppInterop.Runtime.Runtime.Il2CppTypeStruct* __0, ref string __result)
    {
        var policy = _current?._policy;
        if (__0 == null || policy?.CanRewrite(Environment.CurrentManagedThreadId, __result) != true) return;
        __result = policy.Rewrite(Environment.CurrentManagedThreadId,
            Il2CppInterop.Runtime.IL2CPP.il2cpp_class_from_type((IntPtr)__0).ToInt64(), __result);
    }
    public IReadOnlyList<string> CaptureForeignPatches()
    {
        var result = new List<string>();
        foreach (var target in new[] { _nameHelper, _registrar })
        {
            var info = HarmonyLib.Harmony.GetPatchInfo(target);
            if (info == null) continue;
            void Add(string kind, IEnumerable<HarmonyLib.Patch> patches)
            {
                foreach (var patch in patches.Where(p => p.owner != Owner))
                    result.Add(JsonSerializer.Serialize(new { Target = target.Name, Kind = kind, Owner = patch.owner,
                        Method = patch.PatchMethod.Module.ModuleVersionId + ":" + patch.PatchMethod.MetadataToken,
                        Priority = patch.priority, Index = patch.index, Before = patch.before, After = patch.after }));
            }
            Add("prefix", info.Prefixes); Add("postfix", info.Postfixes); Add("transpiler", info.Transpilers); Add("finalizer", info.Finalizers);
        }
        return result.OrderBy(v => v, StringComparer.Ordinal).ToArray();
    }
    public void Install()
    {
        _harmony.Patch(_registrar, prefix: new HarmonyLib.HarmonyMethod(_entryPrefix), finalizer: new HarmonyLib.HarmonyMethod(_entryFinalizer));
        _harmony.Patch(_nameHelper, postfix: new HarmonyLib.HarmonyMethod(_namePostfix));
    }
    public void Remove()
    {
        var failures = new List<Exception>();
        foreach (var target in new[] { _nameHelper, _registrar })
        {
            try { _harmony.Unpatch(target, HarmonyLib.HarmonyPatchType.All, Owner); }
            catch (Exception ex) { failures.Add(ex); }
        }
        if (failures.Count != 0) throw new AggregateException("type-resolution-unpatch", failures);
    }
    public bool AnyOwnPatchPresent => new[] { _nameHelper, _registrar }.Any(t => HarmonyLib.Harmony.GetPatchInfo(t)?.Owners.Contains(Owner) == true);
    public bool AllOwnPatchesPresent =>
        HarmonyLib.Harmony.GetPatchInfo(_nameHelper)?.Postfixes.Count(p => p.owner == Owner && p.PatchMethod == _namePostfix) == 1 &&
        HarmonyLib.Harmony.GetPatchInfo(_registrar)?.Prefixes.Count(p => p.owner == Owner && p.PatchMethod == _entryPrefix) == 1 &&
        HarmonyLib.Harmony.GetPatchInfo(_registrar)?.Finalizers.Count(p => p.owner == Owner && p.PatchMethod == _entryFinalizer) == 1;
}

internal sealed unsafe class NativeSubtypePort : IBusinessSubtypePort, IBusinessInterfacePort, IBusinessManagerPort
{
    private readonly Func<bool> _contextValid;
    private readonly bool _probeInterface;
    internal NativeSubtypePort(Func<bool> contextValid, bool probeInterface = false) { _contextValid = contextValid; _probeInterface = probeInterface; }
    private void RequireContext() { if (!_contextValid()) throw new InvalidOperationException("context-expired"); }
    private UnityEngine.GameObject? _root;
    private ProbeBusinessSubtype? _business;
    private IntPtr _rootPointer, _businessPointer, _ownClass;
    private bool _registrationReturned, _registered, _destroyed, _cleanupVerified;
    private Dictionary<string, int>? _beforeCleanup;
    private readonly List<object> _components = new();
    private readonly List<object> _targets = new();
    private readonly Dictionary<string, IntPtr> _baseMethods = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IntPtr> _ownMethods = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IntPtr> _ownMethodPointers = new(StringComparer.Ordinal);
    private readonly List<object> _interfaceStages = new();
    private int _expectedInterfaceCallbacks;
    private bool _interfaceAttempted;
    private ManagedSubtypeTypeResolution? _resolution;
    public object Details => new
    {
        RootPointer = Hex(_rootPointer), BusinessPointer = Hex(_businessPointer), OwnClassPointer = Hex(_ownClass),
        ClassInjectorReturned = _registrationReturned, TypeSlotsValidated = _registered, TypeRegistrationReversible = false,
        RootDestroyed = _destroyed, CleanupVerified = _cleanupVerified,
        Components = _components.ToArray(), NativeTargets = _targets.ToArray(), LifecycleBeforeCleanup = _beforeCleanup,
        CountersAfterCleanup = _business?.Counters(), TypeResolution = _resolution?.Details,
        InterfaceEnabled = _probeInterface, InterfaceInvocationAttempted = _interfaceAttempted,
        InterfaceStages = _interfaceStages.ToArray(), ExpectedInterfaceCallbacks = _expectedInterfaceCallbacks
    };
    public SubtypeSnapshot Capture()
    {
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save) || save.IsSaving) throw new InvalidOperationException("saving-or-unavailable");
        // Run normal proxy static initialization before the first foreign baseline.
        var classes = new HashSet<IntPtr>
        {
            Class<Il2CppScheduleOne.Property.Property>(), Class<Il2CppScheduleOne.Property.Business>(),
            Class<Il2CppFishNet.Object.NetworkBehaviour>(), Class<UnityEngine.MonoBehaviour>(),
            Class<UnityEngine.Behaviour>(), Class<UnityEngine.Component>(), Class<UnityEngine.Object>(),
            Class<Il2CppFishNet.Connection.NetworkConnection>()
        };
        if (_probeInterface) classes.Add(Class<Il2CppScheduleOne.Persistence.ISaveable>());
        var registries = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["properties"] = Fingerprint(Il2CppScheduleOne.Property.Property.Properties, classes),
            ["properties-owned"] = Fingerprint(Il2CppScheduleOne.Property.Property.OwnedProperties, classes),
            ["properties-unowned"] = Fingerprint(Il2CppScheduleOne.Property.Property.UnownedProperties, classes),
            ["businesses"] = Fingerprint(Il2CppScheduleOne.Property.Business.Businesses, classes),
            ["businesses-owned"] = Fingerprint(Il2CppScheduleOne.Property.Business.OwnedBusinesses, classes),
            ["businesses-unowned"] = Fingerprint(Il2CppScheduleOne.Property.Business.UnownedBusinesses, classes),
            ["saveables"] = Fingerprint(save.Saveables), ["base-saveables"] = Fingerprint(save.BaseSaveables),
            ["completed-saveables"] = Fingerprint(save.CompletedSaveables), ["save-requests"] = Fingerprint(save.QueuedSaveRequests)
        };
        if (classes.Count > 256) throw new InvalidOperationException("foreign-class-over-budget");
        return new(registries, classes.ToDictionary(Hex, VtableFingerprint, StringComparer.Ordinal));
    }
    public void RegisterType()
    {
        RequireContext();
        if (_registered || _ownClass != IntPtr.Zero) throw new InvalidOperationException("registration-repeated");
        _resolution = new ManagedSubtypeTypeResolution();
        _resolution.Register(Class<Il2CppScheduleOne.Property.Business>(), Class<Il2CppFishNet.Connection.NetworkConnection>(), () =>
        {
            RequireContext();
            Il2CppInterop.Runtime.Injection.ClassInjector.RegisterTypeInIl2Cpp<ProbeBusinessSubtype>(
                new Il2CppInterop.Runtime.Injection.RegisterTypeOptions { Interfaces = Array.Empty<Type>(), LogSuccess = false });
            _registrationReturned = true;
        });
        _ownClass = Il2CppInterop.Runtime.Il2CppClassPointerStore<ProbeBusinessSubtype>.NativeClassPtr;
        if (_ownClass == IntPtr.Zero || Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_parent(_ownClass) != Class<Il2CppScheduleOne.Property.Business>())
            throw new InvalidOperationException("native-parent-mismatch");
        Bind<Il2CppScheduleOne.Property.Property>("ShouldSave", "CanBePurchased", "FixedUpdate", "InitializeSaveable");
        Bind<Il2CppScheduleOne.Property.Business>("Awake", "Start", "OnDestroy", "NetworkInitialize___Early", "NetworkInitialize__Late", "NetworkInitializeIfDisabled");
        Bind<Il2CppFishNet.Object.NetworkBehaviour>("Reset", "OnValidate");
        _registered = true;
    }
    private void Bind<T>(params string[] names) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        var own = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)_ownClass);
        foreach (string name in names)
        {
            IntPtr source = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_method_from_name(Class<T>(), name, 0);
            if (source == IntPtr.Zero) throw new MissingMethodException(typeof(T).FullName, name);
            var original = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)source);
            if (original.Slot >= own.VtableCount || own.VTable == IntPtr.Zero) throw new InvalidOperationException("base-slot-invalid");
            var slot = ((Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)own.VTable)[original.Slot];
            IntPtr candidate = (IntPtr)slot.method;
            if (candidate == IntPtr.Zero || Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(candidate) != _ownClass ||
                slot.methodPtr == IntPtr.Zero || slot.methodPtr == original.MethodPointer || MethodPointer(candidate) != slot.methodPtr)
                throw new InvalidOperationException("own-slot-mismatch:" + name);
            _baseMethods.Add(name, source); _ownMethods.Add(name, candidate); _ownMethodPointers.Add(name, slot.methodPtr);
            _targets.Add(new { Name = name, BaseMethod = Hex(source), BasePointer = Hex(original.MethodPointer),
                BaseSlot = original.Slot, OwnMethod = Hex(candidate), OwnPointer = Hex(slot.methodPtr), DeclaringClass = Hex(_ownClass) });
        }
    }
    public void Prepare()
    {
        RequireContext();
        if (!_registered || _root != null) throw new InvalidOperationException("prepare-state");
        _root = new UnityEngine.GameObject("TownHallInterior.InactiveOwnSubtypeTrial");
        _rootPointer = _root.Pointer; // Record ownership before any following operation can fail.
        _root.SetActive(false);
        if (_root.activeSelf || _root.activeInHierarchy) throw new InvalidOperationException("root-not-inactive");
        RequireContext();
        _business = _root.AddComponent<ProbeBusinessSubtype>();
        _businessPointer = _business.Pointer;
        _business.propertyCode = BusinessConstructionTrial.PropertyCode;
    }
    public SubtypeReadback Inspect()
    {
        if (_root == null || _business == null || _destroyed || _business.WasCollected) throw new InvalidOperationException("own-object-missing");
        var components = _root.GetComponents<UnityEngine.Component>();
        if (components.Length > 16) throw new InvalidOperationException("components-over-budget");
        _components.Clear();
        int transforms = 0, businesses = 0;
        foreach (var component in components)
        {
            if (component == null || component.WasCollected) throw new InvalidOperationException("component-unreadable");
            IntPtr nativeClass = Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(component.Pointer);
            _components.Add(new { Type = component.GetIl2CppType().FullName, Pointer = Hex(component.Pointer), NativeClass = Hex(nativeClass) });
            if (nativeClass == Class<UnityEngine.Transform>()) transforms++;
            if (nativeClass == _ownClass && component.Pointer == _businessPointer) businesses++;
        }
        bool targets = _baseMethods.Count == 12;
        foreach (var entry in _baseMethods)
        {
            IntPtr resolved = Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_virtual_method(_businessPointer, entry.Value);
            targets &= resolved == _ownMethods[entry.Key] && Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(resolved) == _ownClass &&
                MethodPointer(resolved) == _ownMethodPointers[entry.Key] && MethodPointer(resolved) != MethodPointer(entry.Value);
        }
        int lifecycle = _business.Counters().Where(p => p.Key != "ShouldSave" && p.Key != "CanBePurchased").Sum(p => p.Value) - _expectedInterfaceCallbacks;
        return new(!_root.activeSelf && !_root.activeInHierarchy, components.Length == 2 && transforms == 1 && businesses == 1 && _root.transform.childCount == 0,
            !_business.IsOwned && _business.propertyCode == BusinessConstructionTrial.PropertyCode,
            Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(_businessPointer) == _ownClass, targets, lifecycle, _business.Calls("InitializeSaveable"));
    }
    public SubtypeDispatch InvokeThroughNativeBase()
    {
        RequireContext();
        if (_business == null || !Inspect().OwnVirtualTargets) throw new InvalidOperationException("dispatch-not-confirmed");
        if (_business.Calls("ShouldSave") != 0 || _business.Calls("CanBePurchased") != 0) throw new InvalidOperationException("dispatch-already-observed");
        // These concrete fresh base proxies call object_get_virtual_method + runtime_invoke.
        // A managed cast of _business would be insufficient to demonstrate native dispatch.
        bool save = new Il2CppScheduleOne.Property.Property(_businessPointer).ShouldSave();
        RequireContext();
        bool purchase = new Il2CppScheduleOne.Property.Property(_businessPointer).CanBePurchased();
        return new(save, purchase, _business.Calls("ShouldSave"), _business.Calls("CanBePurchased"));
    }
    private InterfaceReadback Reject(string finding, InterfaceReadback readback)
    {
        _interfaceStages.Add(new { Stage = "rejected", Finding = finding, Readback = readback });
        return readback;
    }
    public InterfaceReadback InspectInterface()
    {
        RequireContext();
        if (!_probeInterface || _business == null) throw new InvalidOperationException("interface-mode-or-instance-missing");
        var state = Inspect();
        IntPtr interfaceClass = Class<Il2CppScheduleOne.Persistence.ISaveable>();
        IntPtr baseClass = Class<Il2CppScheduleOne.Property.Business>();
        var nativeInterface = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)interfaceClass);
        if (nativeInterface.MethodCount is 0 or > 256 || nativeInterface.Methods == null) throw new InvalidOperationException("interface-method-table-unreadable");
        IntPtr method = Il2CppInterop.Runtime.IL2CPP.GetIl2CppMethodByToken(interfaceClass, 0x06001782);
        if (method == IntPtr.Zero || method != Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_method_from_name(interfaceClass, "InitializeSaveable", 0))
            throw new InvalidOperationException("interface-method-identity-mismatch");
        var proxyField = typeof(Il2CppScheduleOne.Persistence.ISaveable).GetField("NativeMethodInfoPtr_InitializeSaveable_Public_Abstract_Virtual_New_Void_0",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (proxyField?.FieldType != typeof(IntPtr) || proxyField.GetValue(null) is not IntPtr proxyMethod || proxyMethod != method)
            throw new InvalidOperationException("interface-proxy-method-mismatch");
        var evidence = MethodEvidence(method);
        int matches = 0;
        for (int i = 0; i < nativeInterface.MethodCount; i++) if ((IntPtr)nativeInterface.Methods[i] == method) matches++;
        var inherited = InterfaceLayout(baseClass, interfaceClass);
        var own = InterfaceLayout(_ownClass, interfaceClass);
        if (inherited.Matches != 1 || own.Matches != 1 || inherited.Offset != own.Offset || own.Offset < 0 ||
            evidence.Slot < 0 || evidence.Slot >= nativeInterface.MethodCount)
            throw new InvalidOperationException("interface-offset-or-slot-invalid");
        var baseType = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)baseClass);
        var ownType = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)_ownClass);
        long slot = (long)own.Offset + evidence.Slot;
        if (slot < 0 || slot >= baseType.VtableCount || slot >= ownType.VtableCount ||
            baseType.VtableCount > 4096 || ownType.VtableCount > 4096 || baseType.VTable == IntPtr.Zero || ownType.VTable == IntPtr.Zero)
            throw new InvalidOperationException("interface-vtable-bounds");
        var baseEntry = ((Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)baseType.VTable)[slot];
        var ownEntry = ((Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)ownType.VTable)[slot];
        if ((IntPtr)baseEntry.method != _baseMethods["InitializeSaveable"] || (IntPtr)ownEntry.method != _ownMethods["InitializeSaveable"] ||
            ownEntry.methodPtr != _ownMethodPointers["InitializeSaveable"] || MethodEvidence(_baseMethods["InitializeSaveable"]).Token != 0x06001420)
            throw new InvalidOperationException("interface-slot-target-mismatch");
        var target = new InterfaceDispatchTarget(evidence, interfaceClass.ToInt64(),
            (Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_flags(interfaceClass) & 0x20) != 0,
            nativeInterface.MethodCount, matches, _ownClass.ToInt64(), Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(_businessPointer).ToInt64(),
            inherited.Interfaces.SequenceEqual(own.Interfaces), inherited.Matches, own.Matches, inherited.Offset, own.Offset,
            baseType.VtableCount, ownType.VtableCount, ((IntPtr)baseEntry.method).ToInt64(), _baseMethods["InitializeSaveable"].ToInt64(),
            ((IntPtr)ownEntry.method).ToInt64(), ownEntry.methodPtr.ToInt64(), MethodEvidence((IntPtr)ownEntry.method), MethodPointer((IntPtr)ownEntry.method).ToInt64(),
            _ownMethods["InitializeSaveable"].ToInt64(), _ownMethodPointers["InitializeSaveable"].ToInt64());
        var beforeResolver = new InterfaceReadback(target, state, _business.Calls("ShouldSave"), _business.Calls("CanBePurchased"));
        string? rejected = BusinessInterfaceGate.MetadataMismatch(beforeResolver, _expectedInterfaceCallbacks);
        if (rejected != null) return Reject(rejected, beforeResolver);
        RequireContext();
        target = target with
        {
            BaseAssignable = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_is_assignable_from(interfaceClass, baseClass),
            OwnAssignable = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_is_assignable_from(interfaceClass, _ownClass)
        };
        beforeResolver = beforeResolver with { Target = target };
        if (!target.BaseAssignable || !target.OwnAssignable) return Reject("interface-not-assignable", beforeResolver);
        RequireContext(); // Never hand an unvalidated interface/offset/target to the native resolver.
        IntPtr resolved = Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_virtual_method(_businessPointer, method);
        var resolvedReadback = beforeResolver with { Target = target with { Resolved = MethodEvidence(resolved), ResolvedPointer = MethodPointer(resolved).ToInt64() } };
        if (resolved != _ownMethods["InitializeSaveable"]) return Reject("interface-resolver-target-mismatch", resolvedReadback);
        return resolvedReadback;
    }
    public void InvokeThroughNativeInterface()
    {
        RequireContext();
        if (_interfaceAttempted) throw new InvalidOperationException("interface-attempt-already-consumed");
        var before = InspectInterface(); BusinessInterfaceGate.Require(before, 0);
        _interfaceStages.Add(new { Stage = "before-call", Readback = before });
        RequireContext();
        _interfaceAttempted = true;
        // This fresh concrete interface proxy performs native virtual resolution, then runtime_invoke.
        // The original InitializeSaveable body is never called by our no-base override.
        try { new Il2CppScheduleOne.Persistence.ISaveable(_businessPointer).InitializeSaveable(); }
        finally
        {
            _expectedInterfaceCallbacks = 1;
            _interfaceStages.Add(new { Stage = "call-exited", Counters = _business?.Counters() });
        }
        RequireContext();
        var after = InspectInterface(); BusinessInterfaceGate.Require(after, 1);
        _interfaceStages.Add(new { Stage = "after-call", Readback = after });
        if (after.Target != before.Target) throw new InvalidOperationException("interface-target-changed");
    }
    private static InterfaceMethodEvidence MethodEvidence(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero) throw new InvalidOperationException("interface-method-null");
        var method = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)pointer);
        int flags = (int)method.Flags;
        if (method.ReturnType == null) throw new InvalidOperationException("interface-return-type-null");
        return new(pointer.ToInt64(), Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(pointer).ToInt64(),
            Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_name_(pointer) ?? throw new InvalidOperationException("interface-method-name-null"), method.Slot, method.ParametersCount,
            Il2CppInterop.Runtime.IL2CPP.il2cpp_type_get_name_((IntPtr)method.ReturnType) ?? throw new InvalidOperationException("interface-return-name-null"),
            (flags & 0x40) != 0, (flags & 0x400) != 0, (flags & 0x10) != 0, method.Token);
    }
    private static (int Matches, int Offset, long[] Interfaces) InterfaceLayout(IntPtr pointer, IntPtr expected)
    {
        if (pointer == IntPtr.Zero) throw new InvalidOperationException("interface-owner-class-null");
        var type = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)pointer);
        if (type.InterfaceOffsetsCount > 256 || type.InterfaceCount > 256 ||
            (type.InterfaceOffsetsCount > 0 && type.InterfaceOffsets == null) || (type.InterfaceCount > 0 && type.ImplementedInterfaces == null))
            throw new InvalidOperationException("interface-layout-unreadable");
        int matches = 0, offset = -1;
        for (int i = 0; i < type.InterfaceOffsetsCount; i++)
            if ((IntPtr)type.InterfaceOffsets[i].interfaceType == expected) { matches++; offset = type.InterfaceOffsets[i].offset; }
        var interfaces = new long[type.InterfaceCount];
        for (int i = 0; i < interfaces.Length; i++)
        {
            interfaces[i] = ((IntPtr)type.ImplementedInterfaces[i]).ToInt64();
            if (interfaces[i] == 0) throw new InvalidOperationException("implemented-interface-null");
        }
        return (matches, offset, interfaces);
    }
    public void Cleanup()
    {
        if (ReferenceEquals(_root, null) || _cleanupVerified) return;
        _beforeCleanup ??= _business?.Counters();
        if (_root.Pointer != _rootPointer || _root.WasCollected) throw new InvalidOperationException("cleanup-identity-unreadable");
        if (!_destroyed && (UnityEngine.Object)_root != null) UnityEngine.Object.DestroyImmediate(_root);
        if ((UnityEngine.Object)_root! != null) throw new InvalidOperationException("cleanup-native-root-still-present");
        _destroyed = true;
        if (!ReferenceEquals(_business, null))
        {
            if (_business!.WasCollected || _business.Pointer != _businessPointer || (UnityEngine.Object)_business != null)
                throw new InvalidOperationException("cleanup-component-absence-unconfirmed");
            var after = _business!.Counters();
            foreach (var entry in after)
            {
                int previous = _beforeCleanup?.GetValueOrDefault(entry.Key) ?? 0;
                if (entry.Key == "OnDestroy" ? entry.Value - previous is < 0 or > 1 : entry.Value != previous)
                    throw new InvalidOperationException("cleanup-unexpected-callback:" + entry.Key);
            }
        }
        _cleanupVerified = true;
    }
    public bool IsClean => ReferenceEquals(_root, null) || (_destroyed && _cleanupVerified && !_root.WasCollected &&
        _root.Pointer == _rootPointer && (UnityEngine.Object)_root == null);
    public ManagerRegistration RegisterOnce()
    {
#if !TOWNHALL_BUSINESS_MANAGER
        throw new InvalidOperationException("manager-probe-disabled");
#else
        RequireContext();
        if (_business == null || _businessPointer == IntPtr.Zero) throw new InvalidOperationException("manager-instance-missing");
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save) || save.IsSaving) throw new InvalidOperationException("saving-or-unavailable");
        var list = save.Saveables ?? throw new InvalidOperationException("saveables-missing");
        int before = list.Count;
        if (CountPointer(list, _businessPointer) != 0) throw new InvalidOperationException("already-registered");
        save.RegisterSaveable(new Il2CppScheduleOne.Persistence.ISaveable(_businessPointer));
        RequireContext();
        if (save.IsSaving) throw new InvalidOperationException("save-started-during-register");
        int added = CountPointer(list, _businessPointer);
        bool removed = added == 1 && RemovePointer(list, _businessPointer);
        int remaining = CountPointer(list, _businessPointer);
        if (save.IsSaving) throw new InvalidOperationException("save-started-during-removal");
        return new(before, added, remaining, removed && remaining == 0 && list.Count == before, false);
#endif
    }
    public ManagerRegistration SaveRegistered()
    {
#if !TOWNHALL_BUSINESS_MANAGER
        throw new InvalidOperationException("manager-probe-disabled");
#else
        RequireContext();
        if (_business == null || _businessPointer == IntPtr.Zero) throw new InvalidOperationException("manager-instance-missing");
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save) || save.IsSaving) throw new InvalidOperationException("saving-or-unavailable");
        var list = save.Saveables ?? throw new InvalidOperationException("saveables-missing");
        int before = list.Count;
        if (CountPointer(list, _businessPointer) != 0) throw new InvalidOperationException("already-registered");
        int writesBefore = _business.Calls("WriteData");
        int deletesBefore = _business.Calls("DeleteUnapprovedFiles");
        int stringsBefore = _business.Calls("GetSaveString");
        bool saved = false;
        try
        {
            save.RegisterSaveable(new Il2CppScheduleOne.Persistence.ISaveable(_businessPointer));
            if (save.IsSaving) throw new InvalidOperationException("save-started-during-register");
            save.Save();
            saved = true;
        }
        finally
        {
            if (CountPointer(list, _businessPointer) != 0) RemovePointer(list, _businessPointer);
        }
        if (!saved) throw new InvalidOperationException("save-did-not-return");
        if (!save.IsSaving) save.Save();
        int remaining = CountPointer(list, _businessPointer);
        return new(before, 1, remaining, remaining == 0 && list.Count == before, true,
            _business.Calls("WriteData") - writesBefore, _business.Calls("DeleteUnapprovedFiles") - deletesBefore,
            _business.Calls("GetSaveString") - stringsBefore, save.IsSaving);
#endif
    }
    private static int CountPointer<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list, IntPtr pointer)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        int found = 0;
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if (item != null && item.Pointer == pointer) found++;
        }
        return found;
    }
    private static bool RemovePointer<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list, IntPtr pointer)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            if (item != null && item.Pointer == pointer) return list.Remove(item);
        }
        return false;
    }

    private static IntPtr Class<T>() where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        IntPtr pointer = Il2CppInterop.Runtime.Il2CppClassPointerStore<T>.NativeClassPtr;
        if (pointer == IntPtr.Zero) throw new InvalidOperationException("native-class-missing");
        return pointer;
    }
    private static IntPtr MethodPointer(IntPtr method) => method == IntPtr.Zero ? throw new InvalidOperationException("method-missing") :
        Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)method).MethodPointer;
    private static string VtableFingerprint(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero) throw new InvalidOperationException("foreign-class-missing");
        var type = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)pointer);
        if (type.VtableCount > 4096 || (type.VtableCount > 0 && type.VTable == IntPtr.Zero)) throw new InvalidOperationException("vtable-unreadable");
        var value = new System.Text.StringBuilder(Hex(pointer)).Append('|').Append(Hex(type.VTable))
            .Append('|').Append(Hex(Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_parent(pointer)));
        var slots = (Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)type.VTable;
        for (int i = 0; i < type.VtableCount; i++)
        {
            IntPtr method = (IntPtr)slots[i].method;
            value.Append('|').Append(Hex(slots[i].methodPtr)).Append(':').Append(Hex(method)).Append(':')
                .Append(method == IntPtr.Zero ? "null" : Hex(MethodPointer(method)));
        }
        if (type.InterfaceCount > 256 || type.InterfaceOffsetsCount > 256 || type.MethodCount > 4096 ||
            (type.InterfaceCount > 0 && type.ImplementedInterfaces == null) ||
            (type.InterfaceOffsetsCount > 0 && type.InterfaceOffsets == null) || (type.MethodCount > 0 && type.Methods == null))
            throw new InvalidOperationException("foreign-interface-metadata-unreadable");
        value.Append("|interfaces:").Append(type.InterfaceCount).Append(':').Append(Hex((IntPtr)type.ImplementedInterfaces));
        for (int i = 0; i < type.InterfaceCount; i++) value.Append(':').Append(Hex((IntPtr)type.ImplementedInterfaces[i]));
        value.Append("|offsets:").Append(type.InterfaceOffsetsCount).Append(':').Append(Hex((IntPtr)type.InterfaceOffsets));
        for (int i = 0; i < type.InterfaceOffsetsCount; i++) value.Append(':').Append(Hex((IntPtr)type.InterfaceOffsets[i].interfaceType))
            .Append('@').Append(type.InterfaceOffsets[i].offset);
        value.Append("|methods:").Append(type.MethodCount).Append(':').Append(Hex((IntPtr)type.Methods));
        for (int i = 0; i < type.MethodCount; i++)
        {
            IntPtr method = (IntPtr)type.Methods[i];
            if (method == IntPtr.Zero) throw new InvalidOperationException("foreign-method-null");
            var info = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)method);
            value.Append(':').Append(Hex(method)).Append('@').Append(Hex(info.MethodPointer)).Append('/').Append(info.Slot).Append('/').Append(info.Token);
        }
        return type.VtableCount + ":" + Digest(value.ToString());
    }
    private static string Fingerprint<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list, HashSet<IntPtr>? classes = null)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        if (list == null || list.WasCollected || list.Count < 0 || list.Count > 8192) throw new InvalidOperationException("registry-unreadable");
        int count = list.Count;
        var value = new System.Text.StringBuilder(Hex(list.Pointer));
        for (int i = 0; i < count; i++)
        {
            var item = list[i];
            if (item != null && item.WasCollected) throw new InvalidOperationException("registry-entry-unreadable");
            value.Append('|').Append(item == null ? "null" : Hex(item.Pointer));
            if (item != null && classes != null) classes.Add(Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(item.Pointer));
        }
        if (list.Count != count) throw new InvalidOperationException("registry-changed-during-read");
        return count + ":" + Digest(value.ToString());
    }
    private static string Digest(string value) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
    private static string Hex(IntPtr pointer) => pointer.ToInt64().ToString("X");
}
#endif

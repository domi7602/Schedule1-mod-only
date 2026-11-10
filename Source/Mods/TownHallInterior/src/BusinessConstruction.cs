#if TOWNHALL_BUSINESS_ACTIVATION
#error TownHall activation probe withdrawn after native shared-code detour crash. See docs/business-activation-probe.md. Use the verified inactive construction candidate until isolated instance dispatch is proven.
#endif
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE && (TOWNHALL_BUSINESS_MANAGER || TOWNHALL_BUSINESS_INTERFACE || TOWNHALL_BUSINESS_CONSTRUCTION)
#error Active subtype lifecycle must be built alone; it cannot run a manager, interface or legacy construction probe.
#endif
#if TOWNHALL_BUSINESS_NETWORK_CONFLICT
#error Select either the active Business shell or the NetworkObject binding probe, not both.
#endif
#if TOWNHALL_BUSINESS_DATA_CONFLICT
#error Business data must be built alone; do not combine it with another probe mode.
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

public sealed record ActiveSubtypeState(ConstructionContext Context, long Generation, long UnityFrame,
    long Root, long Business, long OwnClass, long Manager, bool ActiveSelf, bool ActiveInHierarchy,
    bool ExactComponents, bool Unowned, bool OwnVirtualTargets, bool OwnUnityMessages,
    bool ReferencesAbsent, bool SaveIdle, IReadOnlyDictionary<string, int> Counters, NetworkSubtypeState? Network = null,
    BusinessDataState? Data = null);
public sealed record NetworkSubtypeState(long Network, long OwnClass, long BusinessNetworkObject,
    int BusinessComponentIndex, long BehaviourArray, int BehaviourCount, long FirstBehaviour,
    bool OwnNativeClass, bool OwnVirtualTargets, bool OwnUnityMessages, bool Unspawned,
    bool Detached, bool ReferencesAbsent, IReadOnlyDictionary<string, int> Counters);
public interface IBusinessActiveSubtypePort : IBusinessSubtypePort
{
    ActiveSubtypeState ReadActiveState();
    void SetActive(bool active);
    IReadOnlyDictionary<string, int> ReadFinalCounters();
}
public interface IBusinessNetworkSubtypePort : IBusinessActiveSubtypePort
{
    IReadOnlyDictionary<string, int> ReadFinalNetworkCounters();
}
public sealed record BusinessDataState(long ContainerRoot, long Container, long ContainerProperty,
    long BoundsRoot, long BoundsCollider, long BusinessLoader, long PropertyLoader,
    bool OwnedHierarchy, bool NeutralDefaults, bool QueryTargetsVerified,
    IReadOnlyDictionary<string, long> Collections, IReadOnlyDictionary<string, int> CollectionCounts);
public sealed record BusinessDataQuery(long Business, long Container, long BoundsCollider, long WrapperBusiness,
    float CurrentLaunderTotal, float AppliedLaunderLimit, bool Inside, bool Outside);
public interface IBusinessDataPort : IBusinessNetworkSubtypePort
{
    BusinessDataQuery ReadNativeDataQueries();
    bool DataCleanupVerified { get; }
}
public sealed record ActiveSubtypeResult(string Status, string[] Findings, bool AttemptConsumed,
    bool ActivationAttempted, bool ActivationReturned, bool DeactivationAttempted,
    bool CleanupAttempted, bool CleanupComplete, ActiveSubtypeState? Before,
    ActiveSubtypeState? Activated, ActiveSubtypeState? Last,
    SubtypeSnapshot? BeforeSnapshot, SubtypeSnapshot? AfterSnapshot,
    bool DataQueryAttempted = false, BusinessDataQuery? DataQuery = null);

// This owner is wired only to an unregistered, no-base shell. It does not certify a
// writing Business or a world-exit drain. Uncertain effects consume the attempt and
// retain the port; later callbacks cannot cause a retry or target the next world.
public sealed class BusinessActiveSubtypeTrial
{
    public const double TimeoutSeconds = 5;
    private static readonly string[] RegistryNames = { "properties", "properties-owned", "properties-unowned",
        "businesses", "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests" };
    private static readonly string[] LifecycleNames = { "Awake", "Start", "FixedUpdate", "OnEnable", "OnDisable", "OnDestroy" };
    private static readonly string[] DataCollections = { "laundering", "employees", "buildables", "configurables", "grids", "switches", "toggleables",
        "saved-objects", "saved-employees", "extra-files", "extra-folders", "bounds", "objects-to-cull", "employee-idle", "loading-docks" };
    private static readonly string[] DataRegistries = { "data-time-listeners", "data-money-listeners", "data-property-listeners", "data-business-listeners" };
    private readonly object _sync = new();
    private readonly bool _requireNetwork, _requireData;
    private bool _dataQueryAttempted;
    private BusinessDataQuery? _dataQuery;
    private readonly List<string> _findings = new();
    private IBusinessActiveSubtypePort? _port;
    private Func<bool>? _contextCurrent;
    private ConstructionContext? _context;
    private ActiveSubtypeState? _before, _activated, _last, _readFloor;
    private SubtypeSnapshot? _beforeSnapshot, _afterSnapshot;
    private string _status = "idle";
    private int _used, _revoked, _thread;
    private bool _busy, _terminal, _activationAttempted, _activationReturned, _deactivationAttempted, _cleanupAttempted, _clean;
    private double _startedAt, _lastTime;
    public BusinessActiveSubtypeTrial(bool requireNetwork = false, bool requireData = false)
    {
        _requireNetwork = requireNetwork || requireData;
        _requireData = requireData;
    }
    public bool AttemptConsumed => Volatile.Read(ref _used) != 0;
    public bool Pending { get { lock (_sync) return AttemptConsumed && !_terminal; } }
    public ActiveSubtypeResult Result
    {
        get
        {
            lock (_sync) return new(_status, _findings.ToArray(), AttemptConsumed,
                _activationAttempted, _activationReturned, _deactivationAttempted, _cleanupAttempted,
                _clean, _before, _activated, _last, _beforeSnapshot, _afterSnapshot, _dataQueryAttempted, _dataQuery);
        }
    }
    public ActiveSubtypeResult Begin(ConstructionContext context, IBusinessActiveSubtypePort port,
        Func<bool> contextStillValid, double now)
    {
        ArgumentNullException.ThrowIfNull(port);
        ArgumentNullException.ThrowIfNull(contextStillValid);
        lock (_sync)
        {
            if (_busy) { Revoke("reentrant-begin"); return Result; }
            if (AttemptConsumed) return Result;
            string? denial = ConstructionGate.Check(context);
            if (_requireNetwork && port is not IBusinessNetworkSubtypePort) denial ??= "network-port-required";
            if (_requireData && port is not IBusinessDataPort) denial ??= "data-port-required";
            if (denial != null || !double.IsFinite(now) || now < 0)
            {
                _status = "skipped"; Add(denial ?? "invalid-clock"); return Result;
            }
            Interlocked.Exchange(ref _used, 1);
            _thread = Environment.CurrentManagedThreadId; _port = port; _contextCurrent = contextStillValid;
            _context = context; _startedAt = _lastTime = now; _findings.Clear(); _busy = true;
            try
            {
                RequireCurrent(); _beforeSnapshot = Freeze(port.Capture());
                RequireCurrent(); port.RegisterType();
                RequireCurrent(); RequireUnchanged(port.Capture());
                RequireCurrent(); port.Prepare();
                RequireCurrent(); var readback = port.Inspect();
                if (readback == null || !readback.Inactive || !readback.ExactComponents || !readback.Unowned ||
                    !readback.OwnNativeClass || !readback.OwnVirtualTargets || readback.UnexpectedLifecycleCalls != 0 ||
                    readback.InitializeSaveableCalls != 0) throw new InvalidOperationException("active-preparation-invalid");
                RequireCurrent(); var dispatch = port.InvokeThroughNativeBase();
                if (dispatch == null || dispatch.ShouldSave || dispatch.CanBePurchased || dispatch.SaveCalls != 1 || dispatch.PurchaseCalls != 1)
                    throw new InvalidOperationException("active-native-dispatch-invalid");
                _before = ReadCurrent(); RequireInactive(_before); RequireCounters(_before.Counters, false);
                if (LifecycleNames.Any(n => Count(_before.Counters, n) != 0)) throw new InvalidOperationException("lifecycle-before-activation");
                if (_requireNetwork && LifecycleNames.Any(n => Count(_before.Network!.Counters, n) != 0)) throw new InvalidOperationException("network-lifecycle-before-activation");
                if (_requireData)
                {
                    RequireCurrent(); RequireUnchanged(port.Capture());
                    var queryBefore = ReadCurrent(); RequireInactive(queryBefore);
                    RequireSameCounters(_before.Counters, queryBefore.Counters); RequireSameNetworkCounters(_before, queryBefore);
                    RequireCurrent(); _dataQueryAttempted = true;
                    var query = ((IBusinessDataPort)port).ReadNativeDataQueries();
                    RequireCurrent();
                    var queryAfter = ReadCurrent(); RequireInactive(queryAfter);
                    RequireSameCounters(queryBefore.Counters, queryAfter.Counters); RequireSameNetworkCounters(queryBefore, queryAfter);
                    RequireCurrent(); RequireUnchanged(port.Capture());
                    if (query == null || query.Business != queryAfter.Business || query.WrapperBusiness != queryAfter.Business ||
                        query.Container != queryAfter.Data!.Container || query.BoundsCollider != queryAfter.Data.BoundsCollider ||
                        !float.IsFinite(query.CurrentLaunderTotal) || !float.IsFinite(query.AppliedLaunderLimit) ||
                        query.CurrentLaunderTotal != 0 || query.AppliedLaunderLimit != 0 || !query.Inside || query.Outside)
                        throw new InvalidOperationException("data-query-unconfirmed");
                    _dataQuery = query;
                }
                RequireCurrent(); RequireUnchanged(port.Capture());
                var immediate = ReadCurrent(); RequireInactive(immediate); RequireSameCounters(_before.Counters, immediate.Counters);
                RequireSameNetworkCounters(_before, immediate);
                RequireCurrent(); _activationAttempted = true; port.SetActive(true); _activationReturned = true;
                _activated = _last = ReadCurrent(); RequireActive(_activated); RequireCounters(_activated.Counters, false);
                RequireCurrent(); RequireUnchanged(port.Capture()); _status = "waiting";
            }
            catch (Exception ex) { Quarantine(ex.Message); }
            finally { _busy = false; }
            return Result;
        }
    }
    public ActiveSubtypeResult Tick(double now)
    {
        if (AttemptConsumed && Environment.CurrentManagedThreadId != _thread) Interlocked.Exchange(ref _revoked, 1);
        lock (_sync)
        {
            if (_busy) { Revoke("reentrant-tick"); return Result; }
            if (!AttemptConsumed || _terminal) return Result;
            _busy = true;
            try
            {
                RequireCurrent();
                if (!double.IsFinite(now) || now < _lastTime) throw new InvalidOperationException("clock-regressed-or-invalid");
                _lastTime = now;
                var previous = _last!;
                _last = ReadCurrent(); RequireActive(_last); RequireCounters(_last.Counters, false);
                if (_last.UnityFrame < previous.UnityFrame) throw new InvalidOperationException("unity-frame-regressed");
                foreach (string name in LifecycleNames)
                    if (Count(_last.Counters, name) < Count(previous.Counters, name)) throw new InvalidOperationException("callback-count-regressed");
                RequireCurrent(); RequireUnchanged(_port!.Capture());
                bool observed = _last.UnityFrame > _activated!.UnityFrame && Count(_last.Counters, "Awake") == 1 &&
                    Count(_last.Counters, "Start") == 1 && Count(_last.Counters, "OnEnable") == 1 && Count(_last.Counters, "FixedUpdate") > 0;
                if (_requireNetwork) observed &= Count(_last.Network!.Counters, "Awake") == 1 && Count(_last.Network.Counters, "Start") == 1 &&
                    Count(_last.Network.Counters, "OnEnable") == 1 && Count(_last.Network.Counters, "FixedUpdate") > 0;
                if (now - _startedAt >= TimeoutSeconds) { Add("lifecycle-timeout"); Retire(false); }
                else if (observed) Retire(true);
            }
            catch (Exception ex) { Quarantine(ex.Message); }
            finally { _busy = false; }
            return Result;
        }
    }
    public ActiveSubtypeResult Leave()
    {
        if (!AttemptConsumed) return Result;
        Interlocked.Exchange(ref _revoked, 1);
        lock (_sync)
        {
            if (AttemptConsumed && !_terminal)
            {
                Add("world-leaving-or-stopped");
                if (!_busy) Quarantine("active-cleanup-deferred");
            }
            return Result;
        }
    }
    private void Retire(bool observed)
    {
        var active = ReadCurrent(); RequireActive(active); RequireCounters(active.Counters, false);
        RequireCurrent(); RequireUnchanged(_port!.Capture());
        active = ReadCurrent(); RequireActive(active); RequireCounters(active.Counters, false);
        RequireCurrent(); _deactivationAttempted = true; _port.SetActive(false);
        _last = ReadCurrent(); RequireInactive(_last); RequireCounters(_last.Counters, true);
        RequireSameCounters(active.Counters, _last.Counters, "OnDisable", 1);
        RequireSameNetworkCounters(active, _last, "OnDisable", 1);
        var beforeCleanup = Freeze(_port.Capture()); RequireUnchanged(beforeCleanup);
        var immediate = ReadCurrent(); RequireInactive(immediate); RequireSameCounters(_last.Counters, immediate.Counters);
        RequireSameNetworkCounters(_last, immediate);
        RequireCurrent(); _cleanupAttempted = true; _port.Cleanup();
        RequireCurrent(); _clean = _port.IsClean;
        if (!_clean) throw new InvalidOperationException("active-cleanup-unconfirmed");
        if (_requireData)
        {
            RequireCurrent(); bool dataClean = ((IBusinessDataPort)_port).DataCleanupVerified; RequireCurrent();
            if (!dataClean) throw new InvalidOperationException("data-cleanup-unconfirmed");
        }
        var final = FreezeCounters(_port.ReadFinalCounters());
        RequireCurrent();
        RequireSameCounters(immediate.Counters, final, "OnDestroy", Count(immediate.Counters, "Awake") == 1 ? 1 : 0);
        if (_requireNetwork)
        {
            var networkFinal = FreezeCounters(((IBusinessNetworkSubtypePort)_port).ReadFinalNetworkCounters());
            RequireCurrent();
            RequireSameCounters(immediate.Network!.Counters, networkFinal, "OnDestroy", Count(immediate.Network.Counters, "Awake") == 1 ? 1 : 0);
        }
        RequireCurrent(); _afterSnapshot = Freeze(_port.Capture());
        if (!Equal(beforeCleanup, _afterSnapshot)) throw new InvalidOperationException("foreign-state-changed-during-cleanup");
        RequireCurrent(); _terminal = true; _status = observed ? "observed" : "failed";
    }
    private ActiveSubtypeState ReadCurrent()
    {
        RequireCurrent();
        var state = _port!.ReadActiveState() ?? throw new InvalidOperationException("active-state-missing");
        state = state with
        {
            Counters = FreezeCounters(state.Counters),
            Network = state.Network == null ? null :
            state.Network with { Counters = FreezeCounters(state.Network.Counters) },
            Data = FreezeData(state.Data)
        };
        RequireCurrent();
        if (ConstructionGate.Check(state.Context) != null || _context == null ||
            !SamePath(_context.ActiveSavePath, state.Context.ActiveSavePath) || !SamePath(_context.LoadedSavePath, state.Context.LoadedSavePath) ||
            state.Generation <= 0 || state.UnityFrame < 0 || state.Root == 0 || state.Business == 0 || state.OwnClass == 0 || state.Manager == 0 ||
            !state.ExactComponents || !state.Unowned || !state.OwnVirtualTargets || !state.OwnUnityMessages || !state.ReferencesAbsent || !state.SaveIdle)
            throw new InvalidOperationException("active-boundary-unconfirmed");
        if (_before != null && (state.Generation != _before.Generation || state.Root != _before.Root || state.Business != _before.Business ||
            state.OwnClass != _before.OwnClass || state.Manager != _before.Manager || state.UnityFrame < _before.UnityFrame))
            throw new InvalidOperationException("active-identity-changed");
        RequireNetworkState(state);
        RequireDataState(state);
        if (_readFloor != null && (state.UnityFrame < _readFloor.UnityFrame ||
            _readFloor.Counters.Any(p => Count(state.Counters, p.Key) < p.Value)))
            throw new InvalidOperationException("active-observation-regressed");
        _readFloor = state;
        return state;
    }
    private void RequireCurrent()
    {
        if (Volatile.Read(ref _revoked) != 0 || Environment.CurrentManagedThreadId != _thread ||
            _contextCurrent?.Invoke() != true || Volatile.Read(ref _revoked) != 0)
            throw new InvalidOperationException("active-context-expired");
    }
    private static bool SamePath(string? a, string? b) => a != null && b != null &&
        string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(a)), Path.TrimEndingDirectorySeparator(Path.GetFullPath(b)), StringComparison.OrdinalIgnoreCase);
    private static void RequireActive(ActiveSubtypeState state)
    {
        if (!state.ActiveSelf || !state.ActiveInHierarchy) throw new InvalidOperationException("active-root-not-active");
    }
    private static void RequireInactive(ActiveSubtypeState state)
    {
        if (state.ActiveSelf || state.ActiveInHierarchy) throw new InvalidOperationException("active-root-not-inactive");
    }
    private static int Count(IReadOnlyDictionary<string, int> counters, string name) => counters.TryGetValue(name, out int count) ? count : 0;
    private static void RequireCounters(IReadOnlyDictionary<string, int> counters, bool disabled)
    {
        if (Count(counters, "ShouldSave") != 1 || Count(counters, "CanBePurchased") != 1 ||
            Count(counters, "Awake") > 1 || Count(counters, "Start") > 1 || Count(counters, "OnEnable") > 1 ||
            Count(counters, "OnDisable") != (disabled ? 1 : 0) || Count(counters, "OnDestroy") != 0 ||
            counters.Any(p => p.Value != 0 && p.Key != "ShouldSave" && p.Key != "CanBePurchased" && !LifecycleNames.Contains(p.Key)))
            throw new InvalidOperationException("active-unexpected-callback");
    }
    private void RequireNetworkState(ActiveSubtypeState state)
    {
        var network = state.Network;
        if (!_requireNetwork)
        {
            if (network != null) throw new InvalidOperationException("unexpected-network-participant");
            return;
        }
        if (network == null || network.Network == 0 || network.OwnClass == 0 || network.BehaviourArray == 0 ||
            network.BusinessNetworkObject != network.Network || network.BusinessComponentIndex != 0 ||
            network.BehaviourCount != 1 || network.FirstBehaviour != state.Business || !network.OwnNativeClass ||
            !network.OwnVirtualTargets || !network.OwnUnityMessages || !network.Unspawned || !network.Detached || !network.ReferencesAbsent)
            throw new InvalidOperationException("network-binding-unconfirmed");
        if (new[] { state.Root, state.Business, state.OwnClass, state.Manager, network.Network, network.OwnClass, network.BehaviourArray }.Distinct().Count() != 7)
            throw new InvalidOperationException("network-native-identity-alias");
        if (_before?.Network is { } initial && (network.Network != initial.Network || network.OwnClass != initial.OwnClass ||
            network.BehaviourArray != initial.BehaviourArray)) throw new InvalidOperationException("network-identity-changed");
        var calls = network.Counters;
        if (Count(calls, "Awake") > 1 || Count(calls, "Start") > 1 || Count(calls, "OnEnable") > 1 || Count(calls, "OnDestroy") != 0 ||
            Count(calls, "OnDisable") != (_deactivationAttempted ? 1 : 0) || calls.Any(p => p.Value != 0 && !LifecycleNames.Contains(p.Key)))
            throw new InvalidOperationException("network-unexpected-callback");
        if (_readFloor?.Network is { } prior && prior.Counters.Any(p => Count(calls, p.Key) < p.Value))
            throw new InvalidOperationException("network-callback-regressed");
    }
    private void RequireSameNetworkCounters(ActiveSubtypeState before, ActiveSubtypeState after, string? changed = null, int delta = 0)
    {
        if (_requireNetwork) RequireSameCounters(before.Network!.Counters, after.Network!.Counters, changed, delta);
    }
    private static BusinessDataState? FreezeData(BusinessDataState? data)
    {
        if (data == null) return null;
        if (data.Collections == null || data.CollectionCounts == null || data.Collections.Count != DataCollections.Length ||
            data.CollectionCounts.Count != DataCollections.Length || !DataCollections.All(data.Collections.ContainsKey) ||
            !DataCollections.All(data.CollectionCounts.ContainsKey) || data.Collections.Values.Any(p => p <= 0) ||
            data.CollectionCounts.Any(p => p.Value != (p.Key == "bounds" ? 1 : 0)))
            throw new InvalidOperationException("data-collections-unconfirmed");
        return data with
        {
            Collections = new ReadOnlyDictionary<string, long>(new Dictionary<string, long>(data.Collections, StringComparer.Ordinal)),
            CollectionCounts = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>(data.CollectionCounts, StringComparer.Ordinal))
        };
    }
    private void RequireDataState(ActiveSubtypeState state)
    {
        var data = state.Data;
        if (!_requireData)
        {
            if (data != null) throw new InvalidOperationException("unexpected-data-participant");
            return;
        }
        if (data == null || data.ContainerProperty != state.Business || !data.OwnedHierarchy || !data.NeutralDefaults || !data.QueryTargetsVerified)
            throw new InvalidOperationException("data-state-unconfirmed");
        var resources = new[] { data.ContainerRoot, data.Container, data.BoundsRoot, data.BoundsCollider, data.BusinessLoader, data.PropertyLoader };
        var identities = resources.Concat(data.Collections.Values).Concat(new[] { state.Root, state.Business, state.OwnClass, state.Manager,
            state.Network!.Network, state.Network.OwnClass, state.Network.BehaviourArray }).ToArray();
        if (identities.Any(p => p <= 0) || identities.Distinct().Count() != identities.Length)
            throw new InvalidOperationException("data-native-identity-alias");
        if ((_before?.Data ?? _readFloor?.Data) is { } initial &&
            (data.ContainerRoot != initial.ContainerRoot || data.Container != initial.Container || data.BoundsRoot != initial.BoundsRoot ||
             data.BoundsCollider != initial.BoundsCollider || data.BusinessLoader != initial.BusinessLoader || data.PropertyLoader != initial.PropertyLoader ||
             data.Collections.Any(p => !initial.Collections.TryGetValue(p.Key, out long prior) || prior != p.Value)))
            throw new InvalidOperationException("data-identity-changed");
    }
    private static void RequireSameCounters(IReadOnlyDictionary<string, int> before, IReadOnlyDictionary<string, int> after, string? changed = null, int delta = 0)
    {
        foreach (string name in before.Keys.Union(after.Keys))
            if (Count(after, name) != Count(before, name) + (name == changed ? delta : 0)) throw new InvalidOperationException("active-cleanup-callback-mismatch:" + name);
        if (changed != null && Count(after, changed) != Count(before, changed) + delta) throw new InvalidOperationException("active-cleanup-callback-missing:" + changed);
    }
    private static IReadOnlyDictionary<string, int> FreezeCounters(IReadOnlyDictionary<string, int> values)
    {
        if (values == null || values.Count > 32 || values.Any(p => string.IsNullOrEmpty(p.Key) || p.Key.Length > 64 || p.Value < 0 || p.Value > 10000))
            throw new InvalidOperationException("active-counters-unreadable");
        return new ReadOnlyDictionary<string, int>(new Dictionary<string, int>(values, StringComparer.Ordinal));
    }
    private SubtypeSnapshot Freeze(SubtypeSnapshot source)
    {
        if (source == null || source.Registries == null || source.ForeignVtables == null ||
            (_requireNetwork ? source.Registries.Count is < 10 or > 48 : source.Registries.Count != 10) ||
            !RegistryNames.All(source.Registries.ContainsKey) || source.ForeignVtables.Count is < 1 or > 256 ||
            (_requireData && !DataRegistries.All(source.Registries.ContainsKey)) ||
            source.Registries.Concat(source.ForeignVtables).Any(p => string.IsNullOrEmpty(p.Key) || string.IsNullOrEmpty(p.Value) || p.Key.Length > 128 || p.Value.Length > 4096))
            throw new InvalidOperationException("active-snapshot-unreadable");
        return new(new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(source.Registries, StringComparer.Ordinal)),
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(source.ForeignVtables, StringComparer.Ordinal)));
    }
    private static bool Equal(SubtypeSnapshot a, SubtypeSnapshot b) => Equal(a.Registries, b.Registries) && Equal(a.ForeignVtables, b.ForeignVtables);
    private static bool Equal(IReadOnlyDictionary<string, string> a, IReadOnlyDictionary<string, string> b) => a.Count == b.Count && a.All(p => b.TryGetValue(p.Key, out string? value) && p.Value == value);
    private void RequireUnchanged(SubtypeSnapshot value)
    {
        if (_beforeSnapshot == null || !Equal(_beforeSnapshot, Freeze(value))) throw new InvalidOperationException("active-foreign-state-changed");
    }
    private void Add(string value) { if (_findings.Count < 16 && !_findings.Contains(value)) _findings.Add(value.Length > 180 ? value[..180] : value); }
    private void Revoke(string reason) { Interlocked.Exchange(ref _revoked, 1); Add(reason); }
    private void Quarantine(string reason) { Add(reason); _clean = false; _status = "quarantined"; _terminal = true; }
}

public sealed record ManagerRegistration(int SaveablesBefore, int Added, int Remaining, bool Removed, bool SaveCalled,
    int WriteCalls = 0, int DeleteCalls = 0, int GetSaveStringCalls = 0, bool SavingAfter = false);
public interface IBusinessManagerPort
{
    ManagerRegistration RegisterOnce();
}

public sealed record ManagerSaveState(ConstructionContext Context, long Generation, long Manager,
    long Saveables, long Frame, bool IsSaving, int PendingRequests, int OwnEntries, int ActiveCallbacks,
    int Started, int Completed, string ForeignSaveables, bool CompletionContractVerified);
public interface IBusinessSavePort
{
    ManagerSaveState ReadSaveState();
    void RegisterSaveable();
    void SaveOnce();
    void RemoveSaveable();
}
public sealed record ManagerSaveResult(string Status, string[] Findings, bool AttemptConsumed,
    bool RegistrationAttempted, bool SaveAttempted, bool SaveReturned, bool CompletionConfirmed,
    bool SafeToDestroy, ManagerSaveState? Before, ManagerSaveState? Last,
    long? DispatchFrame = null, long? CompletionObservedFrame = null);

// The folder getter is also read during contract inspection. Preserve its diagnostic
// count without treating that read as evidence of a native save dispatch.
public sealed class BusinessSaveCallbackLedger
{
    private readonly Dictionary<string, int> _calls = new(StringComparer.Ordinal);
    public bool SaveCallbackObserved { get; private set; }
    public void Record(string name)
    {
        _calls[name] = _calls.TryGetValue(name, out int count) ? count + 1 : 1;
        if (name is "GetSaveString" or "WriteData") SaveCallbackObserved = true;
    }
    public int Calls(string name) => _calls.TryGetValue(name, out int count) ? count : 0;
    public Dictionary<string, int> Snapshot() => new(_calls, StringComparer.Ordinal);
}

// An idle flag is not a completion certificate. The native adapter must explicitly
// establish the completion contract before this state machine can register or save.
// An uncertain save retains ownership; it is never retried or cleaned up speculatively.
public sealed class BusinessManagerSaveTrial
{
    public const double TimeoutSeconds = 60;
    private IBusinessSavePort? _port;
    private Func<bool>? _contextCurrent;
    private ManagerSaveState? _before, _last;
    private readonly List<string> _findings = new();
    private bool _used, _registered, _saveAttempted, _saveReturned, _complete, _safe, _terminal, _busy;
    private double _startedAt, _lastTime;
    private long _dispatchFrame, _observedFrame, _completionFrame = -1;
    private int _observedStarts, _observedCompletes;
    private string _status = "skipped";
    public ManagerSaveState? Preflight { get; private set; }
    public ManagerSaveResult Result => new(_status, _findings.ToArray(), _used, _registered,
        _saveAttempted, _saveReturned, _complete, _safe, _before, _last,
        _saveAttempted ? _dispatchFrame : null, _completionFrame >= 0 ? _completionFrame : null);

    public ManagerSaveResult Begin(IBusinessSavePort port, double now, Func<bool>? contextCurrent = null)
    {
        ArgumentNullException.ThrowIfNull(port);
        if (_used || _terminal || _busy) return Result;
        _busy = true;
        try
        {
            Preflight = null; // A failed fresh read must not reuse an earlier clean observation.
            var before = port.ReadSaveState();
            Preflight = before;
            string? denial = ConstructionGate.Check(before.Context);
            if (denial != null) { _status = "skipped"; Add(denial); return Result; }
            if (!before.CompletionContractVerified) { _status = "blocked"; Add("native-save-completion-contract-unverified"); return Result; }
            if (!double.IsFinite(now) || now < 0 || before.Manager == 0 || before.Saveables == 0 || before.Generation <= 0 ||
                before.Frame < 0 || before.IsSaving || before.PendingRequests != 0 || before.OwnEntries != 0 ||
                before.ActiveCallbacks != 0 || before.Started != 0 || before.Completed != 0 || string.IsNullOrEmpty(before.ForeignSaveables))
            { _status = "blocked"; Add("manager-save-preflight-invalid"); return Result; }
            if (contextCurrent?.Invoke() == false) { _status = "skipped"; Add("context-expired"); return Result; }
            _used = true; _port = port; _contextCurrent = contextCurrent; _before = _last = before;
            _findings.Clear(); _startedAt = _lastTime = now; _observedFrame = before.Frame;
            _registered = true; _safe = false; // Registration itself may have a partial effect.
            port.RegisterSaveable();
            _last = port.ReadSaveState();
            RequireIdentity(_last);
            if (_last.IsSaving || _last.PendingRequests != 0 || _last.OwnEntries != 1 || _last.ActiveCallbacks != 0 ||
                _last.Started != 0 || _last.Completed != 0) throw new InvalidOperationException("manager-registration-readback-invalid");
            RequireContext();
            _dispatchFrame = _last.Frame;
            _saveAttempted = true; // Set before native entry, including exceptions with unknown effects.
            port.SaveOnce();
            _saveReturned = true;
            RequireContext();
            _status = "pending";
        }
        catch (Exception ex)
        {
            Add("manager-save:" + Describe(ex));
            _status = _registered ? "quarantined" : "failed";
            _terminal = true; // An unreadable preflight is terminal too, even before a native attempt was consumed.
        }
        finally { _busy = false; }
        return Result;
    }

    public ManagerSaveResult Tick(double now)
    {
        if (!_used || _terminal || _busy) return Result;
        _busy = true;
        try
        {
            if (!double.IsFinite(now) || now < _lastTime) throw new InvalidOperationException("manager-save-clock-invalid");
            _lastTime = now;
            if (now - _startedAt >= TimeoutSeconds) throw new TimeoutException("manager-save-completion-unconfirmed");
            RequireContext();
            _last = _port!.ReadSaveState();
            RequireIdentity(_last);
            if (_last.Started is < 0 or > 1 || _last.Completed is < 0 or > 1 || _last.Completed > _last.Started ||
                _last.OwnEntries != 1 || _last.PendingRequests < 0 || _last.ActiveCallbacks < 0)
                throw new InvalidOperationException("manager-save-observation-invalid");
            if (_last.Completed == 1 && _completionFrame < 0) _completionFrame = _last.Frame;
            if (_last.Started == 1 && _last.Completed == 1 && !_last.IsSaving && _last.PendingRequests == 0 &&
                _last.ActiveCallbacks == 0 && _last.Frame > _dispatchFrame && _last.Frame > _completionFrame)
            {
                RequireContext();
                _complete = true;
                _port.RemoveSaveable(); // Never retry an unknown-effect removal.
                _last = _port.ReadSaveState();
                RequireIdentity(_last);
                if (_last.IsSaving || _last.PendingRequests != 0 || _last.OwnEntries != 0 || _last.ActiveCallbacks != 0 ||
                    _last.Started != 1 || _last.Completed != 1) throw new InvalidOperationException("manager-save-removal-unconfirmed");
                RequireContext();
                _safe = true; _terminal = true; _status = "observed";
            }
        }
        catch (Exception ex) { Add("manager-save:" + Describe(ex)); _status = "quarantined"; _terminal = true; }
        finally { _busy = false; }
        return Result;
    }
    private void RequireContext()
    {
        if (_contextCurrent?.Invoke() == false) throw new InvalidOperationException("context-expired");
    }
    private void RequireIdentity(ManagerSaveState state)
    {
        if (_before == null || ConstructionGate.Check(state.Context) != null ||
            state.Generation != _before.Generation || state.Manager != _before.Manager || state.Saveables != _before.Saveables ||
            state.Frame < _observedFrame || state.Started < _observedStarts || state.Completed < _observedCompletes || !state.CompletionContractVerified ||
            !string.Equals(Normalize(state.Context.ActiveSavePath!), Normalize(_before.Context.ActiveSavePath!), StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(Normalize(state.Context.LoadedSavePath!), Normalize(_before.Context.LoadedSavePath!), StringComparison.OrdinalIgnoreCase) ||
            state.ForeignSaveables != _before.ForeignSaveables)
            throw new InvalidOperationException("manager-save-identity-changed");
        RequireContext();
        _observedFrame = state.Frame; _observedStarts = state.Started; _observedCompletes = state.Completed;
    }
    private static string Normalize(string path) => Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
    private void Add(string value) { if (_findings.Count < 16) _findings.Add(value); }
    private static string Describe(Exception ex) => ex.GetType().Name + ":" + ex.Message.Replace('\r', ' ').Replace('\n', ' ')[..Math.Min(ex.Message.Length, 160)];
}
#region DataOnlySaveOperationOwnership
public readonly record struct BusinessSaveModelKey(Guid ProcessId, long Operation);
public sealed record BusinessSaveModelBinding(ConstructionContext Context, long Epoch,
    string LoadManager, string SaveManager, string Saveables, string BaseSaveables,
    string Completed, string Requests, string Participant);
public sealed record BusinessSaveModelStamp(BusinessSaveModelKey Key,
    BusinessSaveModelBinding Binding, long Sequence, double Time, string Source = "synthetic");
public enum BusinessSaveModelEffect { TypeRegistration, Construction, Registration, Save, Removal, Destroy }
public enum BusinessSaveModelOutcome { Returned, Failed }
public sealed record BusinessSaveModelEffectState(bool Intent, bool Returned, bool ReadbackConfirmed, bool Failure);
public sealed record BusinessSaveModelDiagnostic(long Sequence, string Kind, string Detail);
public sealed record BusinessSaveModelSnapshot(Guid ProcessId, BusinessSaveModelKey? Key,
    BusinessSaveModelBinding? Binding, string Status, bool SlotConsumed, bool Revoked,
    string? FirstRevocationReason, long LastSequence, double? LastTime, int ActiveCallbacks,
    bool CallbacksUncertain, bool DiagnosticsIncomplete,
    IReadOnlyDictionary<BusinessSaveModelEffect, BusinessSaveModelEffectState> Effects,
    IReadOnlyList<BusinessSaveModelDiagnostic> Diagnostics)
{
    public bool ConstructionArmed { get; init; }
    public string? ConstructedParticipant { get; init; }
    public bool SimulationOnly => true;
    public bool NativeSaveWriteAllowed => false;
    public bool SafeToUnregister => false;
    public bool SafeToDestroy => false;
    public string ReferenceProof => "unknown";
    public string NativeWriteProof => "unknown";
    public string ModDataProof => "unknown";
}

// One explicitly artificial process per model, not a singleton or an OS-process budget.
// The private lock orders only synthetic messages against revocation. It does not
// establish a native adapter's entry, callback lifetime, dispatch or last-use contract.
public sealed class BusinessSaveProcessModel
{
    public const int DiagnosticCapacity = 64, MaxTextLength = 160;
    public const double TimeoutSeconds = 60;
    private const int EffectCount = 6, MaximumPathLength = 32767;
    private readonly object _sync = new();
    private readonly int _thread = Environment.CurrentManagedThreadId;
    private readonly BusinessSaveModelEffectState[] _effects = new BusinessSaveModelEffectState[EffectCount];
    private readonly bool[] _failedOutcomes = new bool[EffectCount], _readbacks = new bool[EffectCount];
    private readonly HashSet<long> _callbacks = new();
    private readonly Queue<BusinessSaveModelDiagnostic> _diagnostics = new(DiagnosticCapacity);
    private BusinessSaveModelBinding? _binding;
    private bool _revoked, _callbacksUncertain, _diagnosticsIncomplete, _constructionArmed, _constructionReservationUsed;
    private string? _firstRevocationReason, _constructedParticipant;
    private long _lastSequence, _lastTicket, _diagnosticSequence;
    private double _startedAt;
    private double? _lastTime;
    public Guid ProcessId { get; } = Guid.NewGuid();

    public BusinessSaveProcessModel()
    {
        for (int i = 0; i < EffectCount; i++) _effects[i] = new(false, false, false, false);
    }

    public BusinessSaveOperationOwner CreateOwner()
    {
        lock (_sync)
        {
            RequireModelThread();
            return new(this); // Even an off-thread facade inherits the same revoked tombstone.
        }
    }

    internal BusinessSaveModelSnapshot CaptureSnapshot()
    {
        lock (_sync)
        {
            var effects = new Dictionary<BusinessSaveModelEffect, BusinessSaveModelEffectState>(EffectCount);
            for (int i = 0; i < EffectCount; i++) effects.Add((BusinessSaveModelEffect)i, _effects[i]);
            return new(ProcessId, _binding == null ? null : new BusinessSaveModelKey(ProcessId, 1), _binding,
                _revoked ? "quarantined" : _binding == null ? "empty" : "bound-data-only",
                _binding != null, _revoked, _firstRevocationReason, _lastSequence, _lastTime,
                _callbacks.Count, _callbacksUncertain, _diagnosticsIncomplete,
                new ReadOnlyDictionary<BusinessSaveModelEffect, BusinessSaveModelEffectState>(effects),
                Array.AsReadOnly(_diagnostics.ToArray()))
            {
                ConstructionArmed = _constructionArmed,
                ConstructedParticipant = _constructedParticipant
            };
        }
    }

    internal bool Bind(BusinessSaveModelBinding? candidate, double now)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptClock(now)) return false;
            if (_revoked) return false;
            if (_binding != null) return Reject("slot-already-consumed");
            if (!TryFreezeBinding(candidate, out var frozen, out string denial))
            {
                AddDiagnostic("bind-rejected", denial);
                return false; // No slot or possible effect was consumed by a rejected candidate.
            }
            AddDiagnostic("bind", "synthetic-process-slot-1");
            if (_revoked) return false;
            _binding = frozen;
            _startedAt = now;
            _lastTime = now;
            return true;
        }
    }

    internal bool Intent(BusinessSaveModelEffect effect, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptStamp(stamp)) return false;
            if (!Defined(effect)) return Reject("effect-undefined");
            if (_revoked) return false;
            int index = (int)effect;
            if (_callbacks.Count != 0 || _callbacksUncertain) return Reject("callback-active-or-uncertain");
            if (_effects[index].Intent) return Reject("duplicate-intent");
            for (int i = 0; i < index; i++)
                if (!_effects[i].Returned || !_effects[i].ReadbackConfirmed || _effects[i].Failure)
                    return Reject("effect-intent-order");
            AddDiagnostic("intent", effect + ":" + Source(stamp));
            if (_revoked) return false;
            _effects[index] = _effects[index] with { Intent = true };
            return true; // The only admission transition; all named effects are data bits, never calls.
        }
    }

    internal bool ArmConstruction(BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptStamp(stamp)) return false;
            if (_revoked) return false;
            var construction = _effects[(int)BusinessSaveModelEffect.Construction];
            if (_callbacks.Count != 0 || _callbacksUncertain ||
                !construction.Intent || construction.Returned || construction.ReadbackConfirmed || construction.Failure ||
                _constructionArmed || _constructionReservationUsed || _constructedParticipant != null)
                return Reject("construction-arm-invalid");
            AddDiagnostic("construction-armed", Source(stamp));
            if (_revoked) return false;
            _constructionArmed = true;
            _constructionReservationUsed = true;
            return true;
        }
    }

    internal long EnterConstructor(string? runtimeParticipant, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread(callback: true) || !AcceptStamp(stamp, callback: true))
            {
                _constructionArmed = false;
                return 0;
            }
            if (_revoked || !_constructionArmed || _constructedParticipant != null ||
                _effects[(int)BusinessSaveModelEffect.Construction] is not { Intent: true, Returned: false, Failure: false } ||
                !IdentityLabel(runtimeParticipant) || !string.Equals(runtimeParticipant, _binding!.Participant, StringComparison.Ordinal))
            {
                _constructionArmed = false;
                Reject("constructor-callback-invalid", callback: true);
                return 0;
            }
            if (_callbacks.Count >= DiagnosticCapacity)
            {
                _constructionArmed = false;
                Reject("callback-capacity-exhausted", callback: true);
                return 0;
            }
            if (_lastTicket == long.MaxValue)
            {
                _constructionArmed = false;
                Reject("callback-ticket-exhausted", incomplete: true, callback: true);
                return 0;
            }
            _constructionArmed = false;
            _constructedParticipant = runtimeParticipant;
            long ticket = ++_lastTicket;
            _callbacks.Add(ticket);
            if (ticket == long.MaxValue) Reject("callback-ticket-exhausted", incomplete: true);
            AddDiagnostic("constructor-enter", runtimeParticipant + ":" + Source(stamp));
            return ticket;
        }
    }

    internal long EnterParticipant(string? runtimeParticipant, string? callbackName, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread(callback: true) || !AcceptStamp(stamp, callback: true)) return 0;
            if (_constructedParticipant == null || _constructionArmed ||
                !IdentityLabel(runtimeParticipant) || !string.Equals(runtimeParticipant, _constructedParticipant, StringComparison.Ordinal) ||
                !IdentityLabel(callbackName))
            {
                Reject("participant-callback-invalid", callback: true);
                return 0;
            }
            if (_callbacks.Count >= DiagnosticCapacity)
            {
                Reject("callback-capacity-exhausted", callback: true);
                return 0;
            }
            if (_lastTicket == long.MaxValue)
            {
                Reject("callback-ticket-exhausted", incomplete: true, callback: true);
                return 0;
            }
            long ticket = ++_lastTicket;
            _callbacks.Add(ticket);
            if (ticket == long.MaxValue) Reject("callback-ticket-exhausted", incomplete: true);
            AddDiagnostic("participant-enter", callbackName + ":" + Source(stamp));
            return ticket;
        }
    }

    // Observation returns indicate accepted bookkeeping, not native success or permission.
    // Valid observations for existing intents remain recordable after any revocation.
    internal bool Outcome(BusinessSaveModelEffect effect, BusinessSaveModelOutcome outcome, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptStamp(stamp)) return false;
            if (!Defined(effect)) return Reject("effect-undefined");
            if (outcome is not (BusinessSaveModelOutcome.Returned or BusinessSaveModelOutcome.Failed))
                return Reject("outcome-undefined");
            int index = (int)effect;
            if (!_effects[index].Intent) return Reject("outcome-without-intent");
            if (outcome == BusinessSaveModelOutcome.Returned)
            {
                if (_effects[index].Returned) return Reject("duplicate-outcome");
                _effects[index] = _effects[index] with { Returned = true };
                if (effect == BusinessSaveModelEffect.Construction && (_constructionArmed || (_constructionReservationUsed && _constructedParticipant == null)))
                {
                    _constructionArmed = false;
                    AddDiagnostic("outcome", effect + ":" + outcome + ":" + Source(stamp));
                    return Reject("construction-returned-without-constructor-bind", callback: true);
                }
            }
            else
            {
                // Failure also covers readback errors; outcome failure needs its own once-only mark.
                if (_failedOutcomes[index]) return Reject("duplicate-outcome");
                _failedOutcomes[index] = true;
                if (effect == BusinessSaveModelEffect.Construction) _constructionArmed = false;
                _effects[index] = _effects[index] with { Failure = true };
                RevokeCore("effect-outcome-failed");
            }
            AddDiagnostic("outcome", effect + ":" + outcome + ":" + Source(stamp));
            return true;
        }
    }

    internal bool Readback(BusinessSaveModelEffect effect, bool confirmed, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptStamp(stamp)) return false;
            if (!Defined(effect)) return Reject("effect-undefined");
            int index = (int)effect;
            if (!_effects[index].Intent) return Reject("readback-without-intent");
            if (_readbacks[index]) return Reject("duplicate-readback");
            if (confirmed && !_effects[index].Returned) return Reject("readback-without-return");
            _readbacks[index] = true;
            if (confirmed) _effects[index] = _effects[index] with { ReadbackConfirmed = true };
            else
            {
                _effects[index] = _effects[index] with { Failure = true };
                RevokeCore("effect-readback-failed");
            }
            AddDiagnostic("readback", effect + ":" + (confirmed ? "confirmed:" : "failed:") + Source(stamp));
            return true;
        }
    }

    internal long Enter(BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread(callback: true) || !AcceptStamp(stamp, callback: true)) return 0;
            if (_callbacks.Count >= DiagnosticCapacity)
            {
                Reject("callback-capacity-exhausted", callback: true);
                return 0;
            }
            if (_lastTicket == long.MaxValue)
            {
                Reject("callback-ticket-exhausted", incomplete: true, callback: true);
                return 0;
            }
            long ticket = ++_lastTicket;
            _callbacks.Add(ticket);
            if (ticket == long.MaxValue) Reject("callback-ticket-exhausted", incomplete: true);
            AddDiagnostic("callback-enter", Source(stamp));
            return ticket; // Counting a late message does not reopen admission or prove dispatch safety.
        }
    }

    internal bool Exit(long ticket, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread(callback: true) || !AcceptStamp(stamp, callback: true)) return false;
            if (ticket <= 0 || !_callbacks.Remove(ticket)) return Reject("callback-ticket-untracked", callback: true);
            AddDiagnostic("callback-exit", Source(stamp));
            return true; // Zero tracked callbacks never establishes native quiescence.
        }
    }

    internal bool Telemetry(string detail, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptStamp(stamp)) return false;
            AddDiagnostic("telemetry:" + Source(stamp), detail);
            return true; // Completion, Idle and queue-empty text have no effect/proof authority.
        }
    }

    internal bool ObserveBinding(BusinessSaveModelBinding? current, BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptStamp(stamp)) return false;
            if (!TryFreezeBinding(current, out var frozen, out _) || !SameBinding(_binding!, frozen!))
                return Reject("context-changed-or-lost");
            AddDiagnostic("context-unchanged", Source(stamp));
            return true;
        }
    }

    internal bool UnexpectedSave(BusinessSaveModelStamp stamp)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptStamp(stamp)) return false;
            RevokeCore("unexpected-save");
            return true; // No save is issued and no foreign subscriber or callback is invoked.
        }
    }

    internal void Revoke(string reason)
    {
        lock (_sync) RevokeCore(reason);
    }

    internal bool Tick(double now)
    {
        lock (_sync)
        {
            if (!RequireModelThread() || !AcceptClock(now) || _binding == null || _revoked) return false;
            AddDiagnostic("tick", "synthetic-clock-observation");
            return !_revoked;
        }
    }

    // Helpers below are called only under _sync; none calls outside code or performs IO.
    private bool RequireModelThread(bool callback = false) =>
        Environment.CurrentManagedThreadId == _thread || Reject("off-model-thread", callback: callback);

    private bool AcceptStamp(BusinessSaveModelStamp? stamp, bool callback = false)
    {
        if (_binding == null) return Reject("observation-unbound", callback: callback);
        if (stamp == null) return Reject("stamp-missing", callback: callback);
        if (stamp.Key != new BusinessSaveModelKey(ProcessId, 1)) return Reject("stamp-key-mismatch", callback: callback);
        if (!TryFreezeBinding(stamp.Binding, out var frozen, out _) || !SameBinding(_binding, frozen!))
            return Reject("stamp-binding-mismatch", callback: callback);
        if (stamp.Sequence <= 0 || stamp.Sequence <= _lastSequence)
            return Reject("stamp-sequence-invalid", incomplete: true, callback: callback);
        if (!AcceptClock(stamp.Time))
        {
            if (callback) _callbacksUncertain = true;
            return false;
        }
        _lastSequence = stamp.Sequence;
        // The last representable message is observable but closes new intent admission.
        if (_lastSequence == long.MaxValue) Reject("stamp-sequence-exhausted", incomplete: true);
        return true;
    }

    private bool AcceptClock(double now)
    {
        if (!double.IsFinite(now) || now < 0 || (_lastTime is { } last && now < last))
            return Reject("clock-invalid", incomplete: true);
        if (_binding != null)
        {
            _lastTime = now;
            if (!_revoked && now - _startedAt >= TimeoutSeconds) RevokeCore("diagnostic-timeout");
        }
        return true;
    }

    private bool Reject(string reason, bool incomplete = false, bool callback = false)
    {
        if (incomplete) _diagnosticsIncomplete = true;
        if (callback) _callbacksUncertain = true;
        RevokeCore(reason);
        return false;
    }

    private void RevokeCore(string? reason)
    {
        LatchRevocation(reason);
        AddDiagnostic("revoke", BoundText(reason, "revoked"));
    }

    private void LatchRevocation(string? reason)
    {
        _revoked = true;
        _constructionArmed = false;
        _firstRevocationReason ??= BoundText(reason, "revoked");
    }

    private void AddDiagnostic(string kind, string? detail)
    {
        if (_diagnosticSequence == long.MaxValue)
        {
            _diagnosticsIncomplete = true;
            LatchRevocation("diagnostic-sequence-exhausted");
            return;
        }
        if (_diagnostics.Count == DiagnosticCapacity)
        {
            _diagnosticsIncomplete = true;
            LatchRevocation("diagnostic-ring-overflow");
            _diagnostics.Dequeue();
        }
        _diagnostics.Enqueue(new(++_diagnosticSequence, BoundText(kind, "diagnostic"), BoundText(detail, "unspecified")));
        if (_diagnosticSequence == long.MaxValue)
        {
            _diagnosticsIncomplete = true;
            LatchRevocation("diagnostic-sequence-exhausted");
        }
    }

    private static bool Defined(BusinessSaveModelEffect effect) => (uint)effect < EffectCount;
    private static string Source(BusinessSaveModelStamp stamp) => BoundText(stamp.Source, "unspecified");

    private static string BoundText(string? value, string fallback)
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;
        var chars = new char[Math.Min(value.Length, MaxTextLength)];
        for (int i = 0; i < chars.Length; i++) chars[i] = char.IsControl(value[i]) ? ' ' : value[i];
        return new string(chars);
    }

    private static bool IdentityLabel(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MaxTextLength) return false;
        foreach (char character in value) if (char.IsControl(character)) return false;
        return true;
    }

    private static bool TryPathValue(string? value, out string canonical)
    {
        canonical = "";
        if (string.IsNullOrWhiteSpace(value) || value.Length > MaximumPathLength) return false;
        foreach (char character in value) if (char.IsControl(character)) return false;
        try
        {
            if (!Path.IsPathFullyQualified(value)) return false;
            canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(value));
            return canonical.Length <= MaximumPathLength;
        }
        catch (Exception ex) when (ex is ArgumentException or IOException or NotSupportedException or System.Security.SecurityException)
        {
            return false;
        }
    }

    private static bool TryFreezeBinding(BusinessSaveModelBinding? value, out BusinessSaveModelBinding? frozen, out string denial)
    {
        frozen = null;
        denial = "binding-invalid";
        if (value?.Context is not { } context || value.Epoch <= 0 ||
            !IdentityLabel(value.LoadManager) || !IdentityLabel(value.SaveManager) ||
            !IdentityLabel(value.Saveables) || !IdentityLabel(value.BaseSaveables) ||
            !IdentityLabel(value.Completed) || !IdentityLabel(value.Requests) || !IdentityLabel(value.Participant)) return false;
        if (!TryPathValue(context.ActiveSavePath, out string active) ||
            !TryPathValue(context.LoadedSavePath, out string loaded))
        {
            denial = "binding-path-invalid";
            return false;
        }
        // Canonicalization is limited to path values. Identity labels/epoch are never normalized.
        var canonical = context with { ActiveSavePath = active, LoadedSavePath = loaded };
        string? gate = ConstructionGate.CheckSave(canonical, "TownHallTest", "SaveGame_2");
        if (gate != null) { denial = gate; return false; }
        frozen = value with { Context = canonical };
        return true;
    }

    private static bool SameBinding(BusinessSaveModelBinding left, BusinessSaveModelBinding right) =>
        left.Epoch == right.Epoch && left.LoadManager == right.LoadManager && left.SaveManager == right.SaveManager &&
        left.Saveables == right.Saveables && left.BaseSaveables == right.BaseSaveables &&
        left.Completed == right.Completed && left.Requests == right.Requests && left.Participant == right.Participant &&
        left.Context.LoadComplete == right.Context.LoadComplete && left.Context.Singleplayer == right.Context.Singleplayer &&
        left.Context.HallReady == right.Context.HallReady && left.Context.DependenciesMatch == right.Context.DependenciesMatch &&
        left.Context.OrganisationName == right.Context.OrganisationName &&
        string.Equals(left.Context.ActiveSavePath, right.Context.ActiveSavePath, StringComparison.OrdinalIgnoreCase) &&
        string.Equals(left.Context.LoadedSavePath, right.Context.LoadedSavePath, StringComparison.OrdinalIgnoreCase);
}

public sealed class BusinessSaveOperationOwner
{
    private readonly BusinessSaveProcessModel _model;
    internal BusinessSaveOperationOwner(BusinessSaveProcessModel model) { _model = model; }
    public BusinessSaveModelSnapshot Snapshot => _model.CaptureSnapshot();
    public bool TryBind(BusinessSaveModelBinding? binding, double now) => _model.Bind(binding, now);
    public bool TryRecordIntent(BusinessSaveModelEffect effect, BusinessSaveModelStamp stamp) => _model.Intent(effect, stamp);
    public bool TryArmParticipantConstruction(BusinessSaveModelStamp stamp) => _model.ArmConstruction(stamp);
    public long EnterConstructorCallback(string? runtimeParticipant, BusinessSaveModelStamp stamp) => _model.EnterConstructor(runtimeParticipant, stamp);
    public long EnterParticipantCallback(string? runtimeParticipant, string? callbackName, BusinessSaveModelStamp stamp) =>
        _model.EnterParticipant(runtimeParticipant, callbackName, stamp);
    public bool RecordOutcome(BusinessSaveModelEffect effect, BusinessSaveModelOutcome outcome, BusinessSaveModelStamp stamp) =>
        _model.Outcome(effect, outcome, stamp);
    public bool RecordReadback(BusinessSaveModelEffect effect, bool confirmed, BusinessSaveModelStamp stamp) =>
        _model.Readback(effect, confirmed, stamp);
    public long EnterCallback(BusinessSaveModelStamp stamp) => _model.Enter(stamp);
    public bool ExitCallback(long ticket, BusinessSaveModelStamp stamp) => _model.Exit(ticket, stamp);
    public bool ObserveTelemetry(string detail, BusinessSaveModelStamp stamp) => _model.Telemetry(detail, stamp);
    public bool ObserveContext(BusinessSaveModelBinding? current, BusinessSaveModelStamp stamp) => _model.ObserveBinding(current, stamp);
    public bool ObserveUnexpectedSave(BusinessSaveModelStamp stamp) => _model.UnexpectedSave(stamp);
    public void Revoke(string reason) => _model.Revoke(reason);
    public bool Tick(double now) => _model.Tick(now);
}

public sealed record BusinessSaveableSlotContract(
    int Slot,
    int ClassVtableSlot,
    string Name,
    int Token,
    int ParameterCount,
    bool HasDefaultInterfaceBody,
    string DefaultRva,
    bool DefaultPerformsDiskIo,
    bool ImplementedByPropertyOrBusiness,
    bool OverriddenOnProbeSubtype,
    string DispatchHazard);

public sealed record BusinessSaveableEvaluationInput(
    bool RegisteredInSaveables,
    bool RegisteredInBaseSaveables,
    bool RegisteredInBusinessOrPropertyList,
    bool LocalExtraFilesInitialized,
    bool LocalExtraFoldersInitialized,
    bool SaveFolderNameInitialized,
    bool ShouldSaveUnderFolder,
    bool CompleteSaveOverridden,
    bool GetContainerFolderOverridden,
    bool WholeWorldSaveRequested,
    int BoundSubtypeVirtualSlotCount,
    int VerifiedInterfaceSlotCount);

public sealed record BusinessSaveableEvaluation(
    bool Admissible,
    bool NativeSaveWriteAllowed,
    IReadOnlyList<string> Findings);

public static class BusinessSaveableContractInventory
{
    public const int InterfaceOffsetOnBusiness = 23;
    public const int TotalInterfaceSlots = 23;
    public const int ExpectedProbeSubtypeBoundVirtualSlots = 16;
    public static IReadOnlyList<BusinessSaveableSlotContract> Slots { get; } = Array.AsReadOnly(new BusinessSaveableSlotContract[]
    {
        new(0, 23, "get_SaveFolderName", 0x06001778, 0, false, "0x0", false, true, false, "returns-property-name-null-if-uninitialized"),
        new(1, 24, "get_SaveFileName", 0x06001779, 0, false, "0x0", false, true, false, "returns-business-literal"),
        new(2, 25, "get_Loader", 0x0600177A, 0, false, "0x0", false, true, false, "returns-loader-field"),
        new(3, 26, "get_ShouldSaveUnderFolder", 0x0600177B, 0, false, "0x0", false, true, true, "overridden-false-safely-bypasses-completesave-folder-creation-and-file-deletion"),
        new(4, 27, "get_LocalExtraFiles", 0x0600177C, 0, false, "0x0", false, true, false, "null-backing-field-throws-nre-in-completesave"),
        new(5, 28, "set_LocalExtraFiles", 0x0600177D, 1, false, "0x0", false, true, false, "mutates-extra-files-backing-field"),
        new(6, 29, "get_LocalExtraFolders", 0x0600177E, 0, false, "0x0", false, true, false, "null-backing-field-throws-nre-in-completesave"),
        new(7, 30, "set_LocalExtraFolders", 0x0600177F, 1, false, "0x0", false, true, false, "mutates-extra-folders-backing-field"),
        new(8, 31, "get_HasChanged", 0x06001780, 0, false, "0x0", false, true, false, "reads-has-changed-flag"),
        new(9, 32, "set_HasChanged", 0x06001781, 1, false, "0x0", false, true, false, "mutates-has-changed-flag"),
        new(10, 33, "InitializeSaveable", 0x06001782, 0, false, "0x0", false, true, true, "base-registers-in-savemanager-saveables-overridden-on-probe"),
        new(11, 34, "GetSaveString", 0x06001783, 0, false, "0x0", false, true, true, "empty-return-causes-saverequest-ctor-to-tailcall-completesave-synchronously"),
        new(12, 35, "Save", 0x06001784, 1, true, "0x583A50", true, false, false, "default-method-probes-filesystem-and-allocates-saverequest"),
        new(13, 36, "WriteBaseData", 0x06001785, 2, true, "0x583E70", true, false, false, "default-method-writes-base-file-or-logs-error-then-tailcalls-completesave"),
        new(14, 37, "GetLocalPath", 0x06001786, 1, true, "0x583990", false, false, false, "default-method-combines-save-folder-and-file-names"),
        new(15, 38, "CompleteSave", 0x06001787, 2, true, "0x582F00", true, false, false, "default-method-creates-container-folder-and-deletes-unapproved-files-before-deleteunapprovedfiles"),
        new(16, 39, "WriteData", 0x06001788, 1, true, "0x584200", true, true, true, "base-writes-property-and-business-subfiles-overridden-on-probe"),
        new(17, 40, "DeleteUnapprovedFiles", 0x06001789, 1, true, "0x43FEC0", false, true, true, "called-only-after-default-completesave-folder-deletion-loop"),
        new(18, 41, "GetContainerFolder", 0x0600178A, 1, true, "0x583840", true, false, false, "default-method-calls-directory-createdirectory"),
        new(19, 42, "WriteSubfile", 0x0600178B, 3, true, "0x5846A0", true, false, false, "default-method-writes-subfile-to-disk"),
        new(20, 43, "WriteFolder", 0x0600178C, 2, true, "0x584260", true, false, false, "default-method-creates-folder-on-disk"),
        new(21, 44, "TryLoadFile", 0x0600178D, 3, true, "0x583D80", true, false, false, "default-method-reads-file-from-disk"),
        new(22, 45, "TryLoadFile", 0x0600178E, 3, true, "0x583BB0", true, false, false, "default-method-reads-file-from-disk")
    });

    public static BusinessSaveableEvaluation Evaluate(BusinessSaveableEvaluationInput? input)
    {
        if (input == null) return new(false, false, Array.AsReadOnly(new[] { "evaluation-input-missing" }));
        var findings = new List<string>();
        if (input.BoundSubtypeVirtualSlotCount != ExpectedProbeSubtypeBoundVirtualSlots)
            findings.Add("subtype-virtual-slot-binding-incomplete");
        if (input.VerifiedInterfaceSlotCount != TotalInterfaceSlots)
            findings.Add("interface-slot-verification-incomplete");
        if (input.RegisteredInSaveables && !input.RegisteredInBaseSaveables && !input.RegisteredInBusinessOrPropertyList)
            findings.Add("saveables-without-driver-deadlocks-saveroutine");
        if (!input.LocalExtraFilesInitialized || !input.LocalExtraFoldersInitialized)
            findings.Add("uninitialized-extra-lists-nre-in-completesave");
        if (!input.SaveFolderNameInitialized)
            findings.Add("uninitialized-save-folder-name-in-local-path");
        if (input.ShouldSaveUnderFolder && (!input.CompleteSaveOverridden || !input.GetContainerFolderOverridden))
            findings.Add("default-completesave-creates-container-folder-and-deletes-unapproved-entries");
        if (input.WholeWorldSaveRequested)
            findings.Add("whole-world-save-writes-foreign-saveables");
        return new(findings.Count == 0, false, Array.AsReadOnly(findings.ToArray()));
    }
}
#endregion

public sealed record SubtypeResult(string Status, string[] Findings, bool AttemptConsumed,
    bool RegistrationAttempted, bool RegistrationCompleted, bool CleanupComplete,
    SubtypeSnapshot? Before, SubtypeSnapshot? Registered, SubtypeSnapshot? Constructed,
    SubtypeSnapshot? Dispatched, SubtypeSnapshot? After, SubtypeReadback? Readback, SubtypeDispatch? Dispatch,
    InterfaceProbeResult? Interface = null, ManagerRegistration? Manager = null, ManagerSaveResult? Save = null);

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
        string? denial = probeSave ? ConstructionGate.CheckSave(context, organisation, slot) : ConstructionGate.Check(context, organisation, slot);
        if (denial == null && probeSave && port is not IBusinessSavePort)
            return new("blocked", new[] { "native-save-completion-contract-unverified" }, AttemptConsumed,
                false, false, false, null, null, null, null, null, null, null);
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
        ManagerSaveResult? save = null;
        BusinessManagerSaveTrial? saveTrial = null;
        ManagerSaveState? savePreflight = null;
        bool savePreflightReadStarted = false;
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
        bool CanCleanupSaveParticipant()
        {
            RequireContext();
            if (saveTrial != null)
            {
                save = saveTrial.Result;
                // Registration intent is recorded before native entry: even a throwing
                // registration can leave a reference, without ever attempting Save().
                if (save.RegistrationAttempted || save.SaveAttempted)
                    return save.SafeToDestroy && save.CompletionConfirmed;
            }
            if (!savePreflightReadStarted) return true;
            var state = saveTrial != null ? saveTrial.Preflight : savePreflight;
            // An unregistered/blocked result is not a release certificate when its
            // preflight was unreadable or already showed a referenced participant.
            return state != null && ConstructionGate.CheckSave(state.Context, organisation, slot) == null &&
                state.Manager != 0 && state.Saveables != 0 && state.Generation > 0 && state.Frame >= 0 &&
                !state.IsSaving && state.PendingRequests == 0 && state.OwnEntries == 0 &&
                state.ActiveCallbacks == 0 && state.Started == 0 && state.Completed == 0 &&
                !string.IsNullOrEmpty(state.ForeignSaveables);
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
            if (probeManager)
            {
                phase = "manager-register"; RequireContext();
                var nativeManager = port as IBusinessManagerPort ?? throw new InvalidOperationException("manager-port-missing");
                manager = nativeManager.RegisterOnce();
                if (manager == null || manager.SaveCalled || manager.SavingAfter || manager.Added != 1 || manager.Remaining != 0 || !manager.Removed)
                    throw new InvalidOperationException("manager-registration-not-reversed");
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
            if (probeSave)
            {
                phase = "save-preflight"; RequireContext();
                var nativeSave = port as IBusinessSavePort ?? throw new InvalidOperationException("save-port-missing");
                savePreflightReadStarted = true;
                savePreflight = nativeSave.ReadSaveState();
                if (!savePreflight.CompletionContractVerified)
                {
                    Fail("native-save-completion-contract-unverified");
                    throw new InvalidOperationException("save-contract-unverified");
                }
                phase = "save-begin"; RequireContext();
                saveTrial = new BusinessManagerSaveTrial();
                double now = 0;
                save = saveTrial.Begin(nativeSave, now, contextStillValid);
                if (save.Status is "blocked" or "failed" or "quarantined")
                {
                    foreach (var finding in save.Findings) Fail("save-begin:" + finding);
                    throw new InvalidOperationException("save-begin-" + save.Status);
                }
                if (save.SaveAttempted)
                {
                    phase = "save-tick"; RequireContext();
                    save = saveTrial.Tick(now + 1.0);
                    if (save.Status == "pending" && (!save.SafeToDestroy || !save.CompletionConfirmed))
                        save = saveTrial.Tick(now + 2.0);
                    if (!save.SafeToDestroy || !save.CompletionConfirmed)
                    {
                        foreach (var finding in save.Findings) Fail("save-tick:" + finding);
                        throw new InvalidOperationException("save-completion-unconfirmed");
                    }
                }
                phase = "save-readback"; var afterSave = Freeze(port.Capture()); RequireUnchanged(afterSave);
            }
        }
        catch (Exception ex) { Fail(phase + ":" + Describe(ex)); }
        finally
        {
            if (registrationAttempted)
            {
                for (int attempt = 1; attempt <= 2 && !clean; attempt++)
                {
                    if (probeSave)
                    {
                        try
                        {
                            if (!CanCleanupSaveParticipant()) { Fail("save-cleanup-deferred"); break; }
                        }
                        catch (Exception ex) { Fail("save-cleanup-deferred:" + Describe(ex)); break; }
                    }
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
                interfaceBeforeSnapshot, interfaceAfterSnapshot, interfaceAttempted) : null,
            Manager: manager, Save: save);
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
    public static string? CheckSave(ConstructionContext? context, string organisation = "TownHallTest", string slot = "SaveGame_2") =>
        organisation != "TownHallTest" || slot != "SaveGame_2" ? "save-target-not-allowed" : Check(context);

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
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
// Only the injected class's named Unity messages are replaced. Direct native base
// calls (notably nonvirtual OnDisable/OnDestroy) are not intercepted by this shell.
public sealed class ProbeNetworkSubtype : Il2CppFishNet.Object.NetworkObject
{
    private static int _armedConstructs, _unarmedConstructs;
    private readonly bool _constructedWhileArmed;
    private readonly BusinessSaveCallbackLedger _calls = new();
    public ProbeNetworkSubtype(IntPtr pointer) : base(pointer)
    {
        if (Interlocked.CompareExchange(ref _armedConstructs, 0, 1) == 1) _constructedWhileArmed = true;
        else Interlocked.Increment(ref _unarmedConstructs);
    }
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal static void ArmNextConstruction() => Interlocked.Exchange(ref _armedConstructs, 1);
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal static void DisarmConstruction() => Interlocked.Exchange(ref _armedConstructs, 0);
    internal static int UnarmedConstructs { [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp] get => Volatile.Read(ref _unarmedConstructs); }
    internal bool ConstructedWhileArmed { [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp] get => _constructedWhileArmed; }
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal Dictionary<string, int> Counters() => _calls.Snapshot();
    public override void Awake() => _calls.Record(nameof(Awake));
    public override void Start() => _calls.Record(nameof(Start));
    public void OnEnable() => _calls.Record(nameof(OnEnable));
    public new void OnDisable() => _calls.Record(nameof(OnDisable));
    public new void OnDestroy() => _calls.Record(nameof(OnDestroy));
    public void FixedUpdate() => _calls.Record(nameof(FixedUpdate));
}
#endif
// Intentionally no RegisterTypeInIl2Cpp attribute: registration belongs to the gated trial.
public sealed class ProbeBusinessSubtype : Il2CppScheduleOne.Property.Business
{
    private static int _armedConstructs, _unarmedConstructs;
    private readonly bool _constructedWhileArmed;
    private readonly BusinessSaveCallbackLedger _calls = new();
    public ProbeBusinessSubtype(IntPtr pointer) : base(pointer)
    {
        if (Interlocked.CompareExchange(ref _armedConstructs, 0, 1) == 1) _constructedWhileArmed = true;
        else Interlocked.Increment(ref _unarmedConstructs);
    }
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal static void ArmNextConstruction() => Interlocked.Exchange(ref _armedConstructs, 1);
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal static void DisarmConstruction() => Interlocked.Exchange(ref _armedConstructs, 0);
    internal static int UnarmedConstructs { [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp] get => Volatile.Read(ref _unarmedConstructs); }
    internal bool ConstructedWhileArmed { [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp] get => _constructedWhileArmed; }
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    private void Count(string name) => _calls.Record(name);
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal Dictionary<string, int> Counters() => _calls.Snapshot();
    [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp]
    internal int Calls(string name) => _calls.Calls(name);
    internal bool SaveCallbackObserved { [Il2CppInterop.Runtime.Attributes.HideFromIl2Cpp] get => _calls.SaveCallbackObserved; }
    public override bool ShouldSave() { Count(nameof(ShouldSave)); return false; }
    public override bool CanBePurchased() { Count(nameof(CanBePurchased)); return false; }
    public override bool ShouldSaveUnderFolder
    {
        get
        {
            Count("get_ShouldSaveUnderFolder");
            return false;
        }
    }
    public override void Awake() => Count(nameof(Awake));
    public override void Start() => Count(nameof(Start));
    public override void OnDestroy() => Count(nameof(OnDestroy));
    public override void FixedUpdate() => Count(nameof(FixedUpdate));
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
    public void OnEnable() => Count(nameof(OnEnable));
    public void OnDisable() => Count(nameof(OnDisable));
#endif
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
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
    private const bool ProbeInterface = false, ProbeManager = false, ProbeSave = false;
    private const string ExpectedOrganisation = "TownHallTest", ExpectedSlot = "SaveGame_2";
#if TOWNHALL_BUSINESS_DATA
    private const string ProbeMode = "own-business-data-queries-no-io", ReportFolder = "BusinessData";
    private static readonly BusinessActiveSubtypeTrial ActiveTrial = new(requireData: true);
#elif TOWNHALL_BUSINESS_NETWORK_SUBTYPE
    private const string ProbeMode = "own-network-business-binding-no-io", ReportFolder = "BusinessNetworkSubtype";
    private static readonly BusinessActiveSubtypeTrial ActiveTrial = new(requireNetwork: true);
#else
    private const string ProbeMode = "active-own-business-lifecycle-no-io", ReportFolder = "BusinessActiveSubtype";
    private static readonly BusinessActiveSubtypeTrial ActiveTrial = new();
#endif
    private static ConstructionContext? _activeContext;
    private static long _activeGeneration;
    private static bool _activeReportWritten;
#elif TOWNHALL_BUSINESS_MANAGER
    private const bool ProbeInterface = false, ProbeManager = false, ProbeSave = true;
    private const string ExpectedOrganisation = "TownHallTest", ExpectedSlot = "SaveGame_2";
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
#if TOWNHALL_BUSINESS_DATA
    private const string DataGameAssemblyHash = "61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d";
    private const string DataMetadataHash = "e775c97da277da8eb1a90265478f3b65c9c3d6464aff4f7255d45280a1d1181f";
    private static string? _dataGameAssemblyHash, _dataMetadataHash;
#endif
    private static readonly BusinessSubtypeTrial Trial = new();
    private static Func<bool>? _hallReady;
    private static bool _ready, _loaded;
    private static int _thread;
    private static long _generation;
    internal static long Generation => _generation;
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
#if TOWNHALL_BUSINESS_DATA
            using (var process = System.Diagnostics.Process.GetCurrentProcess())
            {
                var modules = process.Modules.Cast<System.Diagnostics.ProcessModule>()
                    .Where(m => string.Equals(m.ModuleName, "GameAssembly.dll", StringComparison.OrdinalIgnoreCase)).ToArray();
                if (modules.Length != 1) throw new InvalidOperationException("data-native-module-unconfirmed");
                string? nativePath = modules[0].FileName;
                if (string.IsNullOrWhiteSpace(nativePath)) throw new InvalidOperationException("data-native-module-path-unconfirmed");
                _dataGameAssemblyHash = Hash(nativePath);
                _dataMetadataHash = Hash(Path.Combine(Path.GetDirectoryName(nativePath)!, "Schedule I_Data", "il2cpp_data", "Metadata", "global-metadata.dat"));
                if (_dataGameAssemblyHash != DataGameAssemblyHash || _dataMetadataHash != DataMetadataHash)
                    throw new InvalidOperationException("data-native-body-input-hash-mismatch");
            }
#endif
            _directory = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "TownHallInterior", ReportFolder);
            Directory.CreateDirectory(_directory);
            S1API.Lifecycle.GameLifecycle.OnLoadComplete += Loaded;
            S1API.Lifecycle.GameLifecycle.OnPreSceneChange += Leaving;
            S1API.Lifecycle.GameLifecycle.OnPreLoad += Leaving;
            _ready = true;
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
#if TOWNHALL_BUSINESS_DATA
            MelonLoader.MelonLogger.Msg("TownHall business data: own empty data/container/bounds and native queries armed for TownHallTest / SaveGame_2; no native initialization or publication.");
#elif TOWNHALL_BUSINESS_NETWORK_SUBTYPE
            MelonLoader.MelonLogger.Msg("TownHall network subtype: own no-base NetworkObject/Business binding probe armed for TownHallTest / SaveGame_2; no native initialization or publication.");
#else
            MelonLoader.MelonLogger.Msg("TownHall active subtype: own no-IO shell lifecycle probe armed for TownHallTest / SaveGame_2; no registry publication or NetworkObject.");
#endif
#else
            MelonLoader.MelonLogger.Msg(ProbeSave
                ? "TownHall save probe restricted to TownHallTest / SaveGame_2; native writes blocked until the completion contract is verified."
                : "TownHall subtype: gated inactive native-dispatch test armed; no game-method detours.");
#endif
        }
        catch (Exception ex) { _ready = false; MelonLoader.MelonLogger.Warning("TownHall subtype unavailable: " + ex.GetType().Name); }
    }
    private static void Loaded()
    {
        if (!_ready || Environment.CurrentManagedThreadId != _thread) return;
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
        if (_loaded || ActiveTrial.AttemptConsumed) return;
        _activeReportWritten = false;
#endif
        _generation++; _loaded = true; _due = UnityEngine.Time.realtimeSinceStartup + 6;
    }
    internal static void Leaving()
    {
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
        ActiveTrial.Leave();
        if (ActiveTrial.AttemptConsumed && !ActiveTrial.Pending) WriteActiveReport();
#endif
        _generation++; _loaded = false; _due = -1;
    }
    internal static void Tick()
    {
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
        TickActive();
#else
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
                Schema = 3, Mode = ProbeMode, CapturedUtc = DateTime.UtcNow,
                AssemblySha256 = _assemblyHash, NativeProxySha256 = NativeHash, FishNetSha256 = FishNetHash,
                S1ApiSha256 = ApiHash, InteropRuntimeSha256 = InteropHash, HarmonySha256 = HarmonyHash, Target = ExpectedOrganisation + " / " + ExpectedSlot,
                WorldPublicationAllowed = false, NativeSaveWriteAllowed = false, SaveCompletionContractVerified = result.Save?.CompletionConfirmed == true, GameMethodDetoursAdded = false,
                Context = context, Generation = generation, Result = result, Native = port.Details,
                NativeEvidenceFromThisAttempt = result.RegistrationAttempted,
                Limitation = "Type registration persists until process exit. CleanupComplete covers the own Unity root and acquired Business component. " +
                    "Own callback counters and class/vtable snapshots are bounded observations, not a proof of every native side effect. " +
                    (ProbeSave ? "The legacy immediate-removal/double-Save route has been removed. This mode is blocked before registration or a native save until a verified completion adapter exists. The pure completion state machine is offline-tested, not a native completion proof. " :
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
#endif
    }
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
    private static void TickActive()
    {
        if (!_ready) return;
        try
        {
            if (Environment.CurrentManagedThreadId != _thread) throw new InvalidOperationException("wrong-thread");
            if (ActiveTrial.Pending)
            {
                ActiveTrial.Tick(UnityEngine.Time.realtimeSinceStartup);
                if (!ActiveTrial.Pending) WriteActiveReport();
                return;
            }
            if (ActiveTrial.AttemptConsumed || !_loaded || _due < 0 || UnityEngine.Time.realtimeSinceStartup < _due) return;
            _due = -1; _activeContext = CurrentContext(); _activeGeneration = _generation;
            long generation = _generation;
#if TOWNHALL_BUSINESS_DATA
            var originalContext = _activeContext;
            var originalManager = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
            IntPtr originalManagerPointer = S1Mods.Shared.NetworkGuard.IsAlive(originalManager) ? originalManager.Pointer : IntPtr.Zero;
            bool Current()
            {
                if (!_ready || !_loaded || generation != _generation || Environment.CurrentManagedThreadId != _thread ||
                    ConstructionGate.Check(originalContext) != null) return false;
                var current = CurrentContext();
                if (ConstructionGate.Check(current) != null) return false;
                var manager = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
                return S1Mods.Shared.NetworkGuard.IsAlive(manager) && manager.Pointer == originalManagerPointer &&
                    string.Equals(current.OrganisationName, originalContext.OrganisationName, StringComparison.Ordinal) &&
                    string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(current.ActiveSavePath!)), Path.TrimEndingDirectorySeparator(Path.GetFullPath(originalContext.ActiveSavePath!)), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(current.LoadedSavePath!)), Path.TrimEndingDirectorySeparator(Path.GetFullPath(originalContext.LoadedSavePath!)), StringComparison.OrdinalIgnoreCase);
            }
#else
            bool Current() => _ready && _loaded && generation == _generation && Environment.CurrentManagedThreadId == _thread &&
                ConstructionGate.Check(CurrentContext()) == null;
#endif
            _port = new NativeSubtypePort(Current);
            ActiveTrial.Begin(_activeContext, _port, Current, UnityEngine.Time.realtimeSinceStartup);
            if (!ActiveTrial.Pending) WriteActiveReport();
        }
        catch (Exception ex)
        {
            ActiveTrial.Leave(); _ready = false;
            MelonLoader.MelonLogger.Warning("TownHall active subtype stopped: " + ex.GetType().Name);
            WriteActiveReport();
        }
    }
    private static void WriteActiveReport()
    {
        if (_activeReportWritten || _activeContext == null || string.IsNullOrEmpty(_directory)) return;
        try
        {
            var result = ActiveTrial.Result;
            var report = new
            {
                Schema = 1,
                Mode = ProbeMode,
                CapturedUtc = DateTime.UtcNow,
                AssemblySha256 = _assemblyHash,
                NativeProxySha256 = NativeHash,
                FishNetSha256 = FishNetHash,
                S1ApiSha256 = ApiHash,
                InteropRuntimeSha256 = InteropHash,
                HarmonySha256 = HarmonyHash,
                Target = ExpectedOrganisation + " / " + ExpectedSlot,
                Context = _activeContext,
                Generation = _activeGeneration,
                WorldPublicationAllowed = false,
                NativeSaveWriteAllowed = false,
#if TOWNHALL_BUSINESS_DATA
                BusinessDataProbeEnabled = true,
                GameAssemblySha256 = _dataGameAssemblyHash,
                NativeMetadataSha256 = _dataMetadataHash,
#endif
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
                NetworkObjectProbeEnabled = true,
                NetworkObjectConstructionAttempted = _port?.NetworkObjectConstructionAttempted == true,
                NetworkObjectHandleAcquired = _port?.NetworkObjectHandleAcquired == true,
                OriginalNetworkLifecycleAllowed = false,
#else
                NetworkObjectAdded = false,
#endif
                GameMethodDetoursAdded = false,
                FrameSource = "UnityEngine.Time.frameCount; no synthetic advancement",
                Result = result,
                Native = _port?.Details,
                NativeEvidenceFromThisAttempt = result.AttemptConsumed,
                Limitation =
#if TOWNHALL_BUSINESS_DATA
                    "Own diagnostic empty collections, separate loaders, container SetProperty and S1API/native zero laundering and inside/outside queries only. " +
                    "Listener fingerprints read the ActionList backing fields _size/_items/_version; the entry bound follows the observed container capacity (<=4096), and the separate generic Count getter plus node/max-children counts are recorded diagnostically. " +
                    "Native metadata and the cached LINQ selector can initialize; unchanged known registries/listeners do not imply all global state is unchanged. " +
                    "Unity cleanup confirms retained own object absence; GC collection/loader/wrapper handle release does not prove immediate native memory reclamation. " +
                    "No complete Business initialization, production bounds, balancing, nonempty laundering, purchase, Save or publication is established. " +
#elif TOWNHALL_BUSINESS_NETWORK_SUBTYPE
                    "Own no-base NetworkObject and Business cache binding only; no original native initialization, spawn, registry publication, Save, purchase or writing. " +
                    "Own OnDisable/OnDestroy names shadow nonvirtual base methods; direct calls to native base bodies are not intercepted. Known holder scans are bounded, not a proof about every delegate or native reference. " +
#else
                    "Own active no-base shell only; no NetworkObject, registry publication, Save, purchase or writing. " +
#endif
                    "Named-method metadata is a prerequisite, not proof of Unity message delivery; only observed own counters describe this attempt. " +
                    "Type registration persists until process exit. Unknown effects or world leave consume the attempt and retain handles without speculative cleanup. " +
                    "This does not establish native Business initialization, persistence/reload, a world-exit drain, native last-use or code-unload safety."
            };
            string path = Path.Combine(_directory, DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffZ") + ".json");
            File.WriteAllText(path + ".tmp", JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(path + ".tmp", path, true); _activeReportWritten = true;
            MelonLoader.MelonLogger.Msg("TownHall active subtype " + result.Status + ": " + path);
        }
        catch (Exception ex) { MelonLoader.MelonLogger.Warning("TownHall active subtype report unavailable: " + ex.GetType().Name); }
    }
#endif
    internal static void Stop()
    {
        Leaving(); _ready = false;
        S1API.Lifecycle.GameLifecycle.OnLoadComplete -= Loaded;
        S1API.Lifecycle.GameLifecycle.OnPreSceneChange -= Leaving;
        S1API.Lifecycle.GameLifecycle.OnPreLoad -= Leaving;
    }
    internal static ConstructionContext CurrentContext()
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
    public object Details => new
    {
        Mapping = SubtypeTypeNamePolicy.NativeName,
        NativeClass = _networkClass.ToInt64().ToString("X"),
        ProxyQualifiedName = _qualifiedProxy,
        ManagedPatchOwner = Owner,
        ManagedTargets = new[] { _nameHelper.ToString(), _registrar.ToString() },
        ParameterInventory = _inventory.ToArray(),
        PreflightComplete = _preflightComplete,
        OriginalResolverAlreadyResolves = _originalResolved,
        PostScopeOriginalResolutionConfirmed = _postScopeReadback,
        PostScopeFailure = _postScopeFailure,
        Outcome = _lease.Outcome
    };

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
                    result.Add(JsonSerializer.Serialize(new
                    {
                        Target = target.Name,
                        Kind = kind,
                        Owner = patch.owner,
                        Method = patch.PatchMethod.Module.ModuleVersionId + ":" + patch.PatchMethod.MetadataToken,
                        Priority = patch.priority,
                        Index = patch.index,
                        Before = patch.before,
                        After = patch.after
                    }));
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

internal sealed unsafe class NativeSubtypePort : IBusinessSubtypePort, IBusinessInterfacePort, IBusinessManagerPort, IBusinessSavePort
#if TOWNHALL_BUSINESS_DATA
    , IBusinessDataPort
#elif TOWNHALL_BUSINESS_NETWORK_SUBTYPE
    , IBusinessNetworkSubtypePort
#elif TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
    , IBusinessActiveSubtypePort
#endif
{
    private readonly Func<bool> _contextValid;
    private readonly bool _probeInterface;
    internal NativeSubtypePort(Func<bool> contextValid, bool probeInterface = false) { _contextValid = contextValid; _probeInterface = probeInterface; }
    private void RequireContext()
    {
        if (!_contextValid()) throw new InvalidOperationException("context-expired");
#if TOWNHALL_BUSINESS_DATA
        if (_dataWorld != null)
        {
            var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
            if (BusinessSubtypeRuntime.Generation != _dataGeneration || !SameNativeWorld(BusinessSubtypeRuntime.CurrentContext(), _dataWorld) ||
                !S1Mods.Shared.NetworkGuard.IsAlive(save) || save.Pointer != _dataSaveManager || save.IsSaving)
                throw new InvalidOperationException("data-world-or-save-manager-changed");
            var queue = save.QueuedSaveRequests;
            if (ReferenceEquals(queue, null) || queue.WasCollected || queue.Count != 0) throw new InvalidOperationException("data-save-queue-not-idle");
        }
#endif
    }
    private UnityEngine.GameObject? _root;
    private ProbeBusinessSubtype? _business;
    private IntPtr _rootPointer, _businessPointer, _ownClass;
    private bool _registrationReturned, _registered, _destroyed, _cleanupVerified;
#if TOWNHALL_BUSINESS_DATA
    private readonly Dictionary<string, Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase> _dataHandles = new(StringComparer.Ordinal);
    private readonly List<Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase> _dataPendingHandles = new();
    private readonly Dictionary<string, (UnityEngine.Object Handle, IntPtr Pointer)> _dataUnity = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> _dataMethods = new(StringComparer.Ordinal);
    private readonly List<object> _dataCalls = new();
    private SubtypeSnapshot? _dataForeignBaseline;
    private ConstructionContext? _dataWorld;
    private long _dataGeneration;
    private IntPtr _dataSaveManager;
    private S1API.Property.BusinessWrapper? _dataWrapper;
    private UnityEngine.GameObject? _dataContainerRoot, _dataBoundsRoot;
    private Il2CppScheduleOne.Property.PropertyContentsContainer? _dataContainer;
    private UnityEngine.BoxCollider? _dataBounds;
    private BusinessDataState? _dataLastRead;
    private Dictionary<string, string>? _dataListenerDiagnostics;
    private bool _dataPreparationAttempted, _dataPrepared, _dataSetPropertyAttempted, _dataSetPropertyReturned, _dataQueryAttempted, _dataCleanupVerified;
    public bool DataCleanupVerified => _dataCleanupVerified; // Cached only: never reread native objects after context revocation.
#endif
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
    private ProbeNetworkSubtype? _network;
    private IntPtr _networkPointer, _networkClass, _behaviourArrayPointer;
    private bool _networkRegistrationAttempted, _networkRegistrationReturned;
    internal bool NetworkObjectConstructionAttempted { get; private set; }
    internal bool NetworkObjectHandleAcquired => _networkPointer != IntPtr.Zero;
    private readonly Dictionary<string, (IntPtr Method, IntPtr Pointer, IntPtr Invoker, IntPtr Virtual)> _networkUnityMessages = new(StringComparer.Ordinal);
    private readonly List<object> _networkUnityEvidence = new();
    private Dictionary<string, int>? _networkBeforeCleanup;
    private object? _networkBindingDetails;
    private Dictionary<string, string>? _networkHolderBaseline;
    private ActiveSubtypeState? _nativeNetworkBinding;
    private ActiveSubtypeState? _nativeNetworkLastRead;
#endif
    private int _saveStarted, _saveCompleted;
    private long _lastFrame = -1;
    private long NextFrame()
    {
        long currentUnity = UnityEngine.Time.frameCount;
        while (true)
        {
            long last = Volatile.Read(ref _lastFrame);
            long target = Math.Max(currentUnity, last + 1);
            if (Interlocked.CompareExchange(ref _lastFrame, target, last) == last)
                return target;
        }
    }
    private Dictionary<string, int>? _beforeCleanup;
    private readonly List<object> _components = new();
    private readonly List<object> _targets = new();
    private readonly Dictionary<string, IntPtr> _baseMethods = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IntPtr> _ownMethods = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IntPtr> _ownMethodPointers = new(StringComparer.Ordinal);
    private readonly Dictionary<string, int> _baseSlots = new(StringComparer.Ordinal);
    private readonly List<object> _interfaceStages = new();
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
    private static readonly string[] UnityMessageNames = { "Awake", "Start", "FixedUpdate", "OnEnable", "OnDisable", "OnDestroy" };
    private readonly Dictionary<string, (IntPtr Method, IntPtr Pointer, IntPtr Invoker, IntPtr Virtual)> _unityMessages = new(StringComparer.Ordinal);
    private readonly List<object> _unityMessageEvidence = new();
    private bool _activeSetAttempted, _inactiveSetAttempted, _activeCleanupAttempted;
    private long? _lastObservedUnityFrame;
#endif
    private int _expectedInterfaceCallbacks;
    private bool _interfaceAttempted;
    private ManagedSubtypeTypeResolution? _resolution;
    public object Details => new
    {
        RootPointer = Hex(_rootPointer),
        BusinessPointer = Hex(_businessPointer),
        OwnClassPointer = Hex(_ownClass),
        ClassInjectorReturned = _registrationReturned,
        TypeSlotsValidated = _registered,
        TypeRegistrationReversible = false,
        RootDestroyed = _destroyed,
        CleanupVerified = _cleanupVerified,
        Components = _components.ToArray(),
        NativeTargets = _targets.ToArray(),
        LifecycleBeforeCleanup = _beforeCleanup,
        CountersAfterCleanup = _business?.Counters(),
        TypeResolution = _resolution?.Details,
        InterfaceEnabled = _probeInterface,
        InterfaceInvocationAttempted = _interfaceAttempted,
        InterfaceStages = _interfaceStages.ToArray(),
        ExpectedInterfaceCallbacks = _expectedInterfaceCallbacks
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
        ,
        UnityMessages = _unityMessageEvidence.ToArray(),
        LastObservedUnityFrame = _lastObservedUnityFrame
#endif
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        ,
        NetworkPointer = Hex(_networkPointer),
        NetworkClass = Hex(_networkClass),
        BehaviourArray = Hex(_behaviourArrayPointer),
        NetworkTypeRegistrationAttempted = _networkRegistrationAttempted,
        NetworkTypeRegistrationReturned = _networkRegistrationReturned,
        NetworkUnityMessages = _networkUnityEvidence.ToArray(),
        NetworkBinding = _networkBindingDetails,
        NetworkCountersBeforeCleanup = _networkBeforeCleanup,
        NetworkCountersAfterCleanup = _network?.Counters()
#endif
#if TOWNHALL_BUSINESS_DATA
        ,
        DataPreparationAttempted = _dataPreparationAttempted,
        DataPreparationConfirmed = _dataPrepared,
        SetPropertyAttempted = _dataSetPropertyAttempted,
        SetPropertyReturned = _dataSetPropertyReturned,
        DataQueryAttempted = _dataQueryAttempted,
        DataCalls = _dataCalls.ToArray(),
        DataLastRead = _dataLastRead,
        DataNativeMethods = new Dictionary<string, string>(_dataMethods),
        DataCleanupVerified = _dataCleanupVerified,
        DataUnityResources = _dataUnity.ToDictionary(p => p.Key, p => Hex(p.Value.Pointer)),
        DataGcHandlesReleased = _dataCleanupVerified && _dataHandles.Count == 0 && _dataWrapper == null,
        DataUnconfirmedHandlesRetained = _dataPendingHandles.Count,
        DataGcMemoryReclamationConfirmed = false,
        DataListenerDiagnostics = _dataListenerDiagnostics == null ? null : new Dictionary<string, string>(_dataListenerDiagnostics, StringComparer.Ordinal)
#endif
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
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        classes.Add(Class<Il2CppFishNet.Object.NetworkObject>());
#endif
#if TOWNHALL_BUSINESS_DATA
        var dataWorld = BusinessSubtypeRuntime.CurrentContext();
        if (_dataWorld == null)
        { _dataWorld = dataWorld; _dataGeneration = BusinessSubtypeRuntime.Generation; _dataSaveManager = save.Pointer; }
        if (BusinessSubtypeRuntime.Generation != _dataGeneration || !SameNativeWorld(dataWorld, _dataWorld) || save.Pointer != _dataSaveManager)
            throw new InvalidOperationException("data-world-or-save-manager-changed");
        ValidateDataMethods();
        classes.Add(Class<Il2CppScheduleOne.Property.PropertyContentsContainer>());
        classes.Add(Class<Il2CppScheduleOne.Persistence.Loaders.PropertyLoader>());
        classes.Add(Class<Il2CppScheduleOne.Persistence.Loaders.BusinessLoader>());
        classes.Add(Class<UnityEngine.BoxCollider>());
#endif
        var registries = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["properties"] = Fingerprint(Il2CppScheduleOne.Property.Property.Properties, classes),
            ["properties-owned"] = Fingerprint(Il2CppScheduleOne.Property.Property.OwnedProperties, classes),
            ["properties-unowned"] = Fingerprint(Il2CppScheduleOne.Property.Property.UnownedProperties, classes),
            ["businesses"] = Fingerprint(Il2CppScheduleOne.Property.Business.Businesses, classes),
            ["businesses-owned"] = Fingerprint(Il2CppScheduleOne.Property.Business.OwnedBusinesses, classes),
            ["businesses-unowned"] = Fingerprint(Il2CppScheduleOne.Property.Business.UnownedBusinesses, classes),
            ["saveables"] = Fingerprint(save.Saveables),
            ["base-saveables"] = Fingerprint(save.BaseSaveables),
            ["completed-saveables"] = Fingerprint(save.CompletedSaveables),
            ["save-requests"] = Fingerprint(save.QueuedSaveRequests)
        };
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        foreach (var item in CaptureNetworkHolders(out _)) registries.Add(item.Key, item.Value);
#endif
#if TOWNHALL_BUSINESS_DATA
        foreach (var item in CaptureDataListeners()) registries.Add(item.Key, item.Value);
#endif
        if (classes.Count > 256) throw new InvalidOperationException("foreign-class-over-budget");
        var snapshot = new SubtypeSnapshot(registries, classes.ToDictionary(Hex, VtableFingerprint, StringComparer.Ordinal));
#if TOWNHALL_BUSINESS_DATA
        _dataForeignBaseline ??= snapshot;
#endif
        return snapshot;
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
        Bind<Il2CppScheduleOne.Property.Property>("ShouldSave", "CanBePurchased", "FixedUpdate", "InitializeSaveable", "get_ShouldSaveUnderFolder");
        Bind<Il2CppScheduleOne.Property.Property>(1, "DeleteUnapprovedFiles");
        Bind<Il2CppScheduleOne.Property.Business>("Awake", "Start", "OnDestroy", "GetSaveString", "NetworkInitialize___Early", "NetworkInitialize__Late", "NetworkInitializeIfDisabled");
        Bind<Il2CppScheduleOne.Property.Business>(1, "WriteData");
        Bind<Il2CppFishNet.Object.NetworkBehaviour>("Reset", "OnValidate");
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        RequireContext(); _networkRegistrationAttempted = true;
        Il2CppInterop.Runtime.Injection.ClassInjector.RegisterTypeInIl2Cpp<ProbeNetworkSubtype>(
            new Il2CppInterop.Runtime.Injection.RegisterTypeOptions { Interfaces = Array.Empty<Type>(), LogSuccess = false });
        _networkRegistrationReturned = true;
        _networkClass = Il2CppInterop.Runtime.Il2CppClassPointerStore<ProbeNetworkSubtype>.NativeClassPtr;
        if (_networkClass == IntPtr.Zero || Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_parent(_networkClass) != Class<Il2CppFishNet.Object.NetworkObject>())
            throw new InvalidOperationException("network-native-parent-mismatch");
        ValidateNetworkUnityMessages();
#endif
        _registered = true;
    }
    private void Bind<T>(params string[] names) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase => Bind<T>(0, names);
    private void Bind<T>(int parameterCount, params string[] names) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        var own = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)_ownClass);
        foreach (string name in names)
        {
            IntPtr source = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_method_from_name(Class<T>(), name, parameterCount);
            if (source == IntPtr.Zero) throw new MissingMethodException(typeof(T).FullName, name);
            var original = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)source);
            if (original.Slot >= own.VtableCount || own.VTable == IntPtr.Zero) throw new InvalidOperationException("base-slot-invalid");
            var slot = ((Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)own.VTable)[original.Slot];
            IntPtr candidate = (IntPtr)slot.method;
            if (candidate == IntPtr.Zero || Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(candidate) != _ownClass ||
                slot.methodPtr == IntPtr.Zero || slot.methodPtr == original.MethodPointer || MethodPointer(candidate) != slot.methodPtr)
                throw new InvalidOperationException("own-slot-mismatch:" + name);
            _baseMethods.Add(name, source); _ownMethods.Add(name, candidate); _ownMethodPointers.Add(name, slot.methodPtr);
            _baseSlots.Add(name, original.Slot);
            _targets.Add(new
            {
                Name = name,
                BaseMethod = Hex(source),
                BasePointer = Hex(original.MethodPointer),
                BaseSlot = original.Slot,
                OwnMethod = Hex(candidate),
                OwnPointer = Hex(slot.methodPtr),
                DeclaringClass = Hex(_ownClass)
            });
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
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        RequireContext(); ProbeNetworkSubtype.ArmNextConstruction();
        NetworkObjectConstructionAttempted = true;
        try { _network = _root.AddComponent<ProbeNetworkSubtype>(); }
        finally { ProbeNetworkSubtype.DisarmConstruction(); }
        _networkPointer = _network.Pointer;
#endif
        RequireContext();
        ProbeBusinessSubtype.ArmNextConstruction();
        try { _business = _root.AddComponent<ProbeBusinessSubtype>(); }
        finally { ProbeBusinessSubtype.DisarmConstruction(); }
        _businessPointer = _business.Pointer;
        _business.propertyCode = BusinessConstructionTrial.PropertyCode;
        _business.propertyName = "Hyland Town Hall";
        _business._LocalExtraFiles_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        _business._LocalExtraFolders_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        PrepareNetworkBinding();
#endif
#if TOWNHALL_BUSINESS_DATA
        PrepareBusinessData();
#endif
    }
#if TOWNHALL_BUSINESS_DATA
    private T KeepData<T>(string name, T value) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        if (!ReferenceEquals(value, null)) _dataPendingHandles.Add(value); // Managed retention precedes every fallible post-allocation guard.
        RequireContext();
        if (ReferenceEquals(value, null) || value.WasCollected || value.Pointer == IntPtr.Zero ||
            _dataHandles.Values.Any(p => p.Pointer == value.Pointer)) throw new InvalidOperationException("data-allocation-unconfirmed:" + name);
        _dataHandles.Add(name, value); _dataPendingHandles.RemoveAt(_dataPendingHandles.Count - 1); return value;
    }
    private T KeepDataUnity<T>(string name, T value) where T : UnityEngine.Object
    {
        if (!ReferenceEquals(value, null)) _dataPendingHandles.Add(value);
        RequireContext();
        if (value == null || value.WasCollected || value.Pointer == IntPtr.Zero ||
            _dataUnity.Values.Any(p => p.Pointer == value.Pointer)) throw new InvalidOperationException("data-unity-allocation-unconfirmed:" + name);
        _dataUnity.Add(name, (value, value.Pointer)); _dataPendingHandles.RemoveAt(_dataPendingHandles.Count - 1); return value;
    }
    private UnityEngine.GameObject DataChild(string name)
    {
        RequireContext();
        var child = KeepDataUnity(name, new UnityEngine.GameObject("TownHallInterior.Data." + name));
        child.SetActive(false);
        var transform = KeepDataUnity(name + "-transform", child.transform);
        transform.SetParent(_root!.transform, false);
        transform.localPosition = UnityEngine.Vector3.zero; transform.localRotation = UnityEngine.Quaternion.identity;
        transform.localScale = UnityEngine.Vector3.one;
        RequireContext(); return child;
    }
    private void PrepareBusinessData()
    {
        RequireContext();
        if (_dataPreparationAttempted || _business == null || _root == null || _root.activeSelf) throw new InvalidOperationException("data-prepare-state");
        _dataPreparationAttempted = true;
        KeepDataUnity("root-transform", _root.transform);
        var b = _business;
        b.Price = 0; b.EmployeeCapacity = 0; b.LaunderCapacity = 0;
        b._IsOwned_k__BackingField = false; b.OwnedByDefault = false;
        b.ContentCullingEnabled = false; b._hasBeenLoaded = false; b._HasChanged_k__BackingField = false;
        b.LaunderingOperations = KeepData("laundering", new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Property.LaunderingOperation>());
        b._Employees_k__BackingField = KeepData("employees", new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Employees.Employee>());
        b.BuildableItems = KeepData("buildables", new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.EntityFramework.BuildableItem>());
        b.Configurables = KeepData("configurables", new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Management.IConfigurable>());
        b.Grids = KeepData("grids", new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Tiles.Grid>());
        b.Switches = KeepData("switches", new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Misc.ModularSwitch>());
        b.Toggleables = KeepData("toggleables", new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Interaction.InteractableToggleable>());
        KeepData("extra-files", b._LocalExtraFiles_k__BackingField); KeepData("extra-folders", b._LocalExtraFolders_k__BackingField);
        b.savedObjectPaths = KeepData("saved-objects", new il2cpp::Il2CppSystem.Collections.Generic.List<string>());
        b.savedEmployeePaths = KeepData("saved-employees", new il2cpp::Il2CppSystem.Collections.Generic.List<string>());
        b.ObjectsToCull = KeepData("objects-to-cull", new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<UnityEngine.GameObject>(0));
        b.EmployeeIdlePoints = KeepData("employee-idle", new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<UnityEngine.Transform>(0));
        b.LoadingDocks = KeepData("loading-docks", new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<Il2CppScheduleOne.Delivery.LoadingDock>(0));
        b.loader = KeepData("business-loader", new Il2CppScheduleOne.Persistence.Loaders.BusinessLoader());
        new Il2CppScheduleOne.Property.Property(_businessPointer).loader = KeepData("property-loader", new Il2CppScheduleOne.Persistence.Loaders.PropertyLoader());
        _dataContainerRoot = DataChild("container");
        _dataContainer = KeepDataUnity("container-component", _dataContainerRoot.AddComponent<Il2CppScheduleOne.Property.PropertyContentsContainer>());
        _dataBoundsRoot = DataChild("bounds");
        _dataBounds = KeepDataUnity("bounds-component", _dataBoundsRoot.AddComponent<UnityEngine.BoxCollider>());
        _dataBounds.enabled = false; _dataBounds.center = UnityEngine.Vector3.zero; _dataBounds.size = new UnityEngine.Vector3(2, 2, 2);
        b.BoundingBox = _dataBoundsRoot;
        b.propertyBoundsColliders = KeepData("bounds", new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<UnityEngine.BoxCollider>(new[] { _dataBounds }));
        b._Container_k__BackingField = _dataContainer;
        _dataWrapper = new S1API.Property.BusinessWrapper(new Il2CppScheduleOne.Property.Business(_businessPointer));
        RequireDataBoundary(requireBacklink: false);
        _dataSetPropertyAttempted = true;
        _dataCalls.Add(new { Call = "SetProperty", Attempted = true, Returned = false });
        _dataContainer.SetProperty(new Il2CppScheduleOne.Property.Property(_businessPointer));
        _dataSetPropertyReturned = true;
        _dataCalls.Add(new { Call = "SetProperty", Attempted = true, Returned = true, ReciprocalPointersConfirmed = false });
        RequireContext();
        RequireDataBoundary();
        _dataCalls.Add(new { Call = "SetProperty", Attempted = true, Returned = true, ReciprocalPointersConfirmed = true });
        _dataPrepared = true;
    }
    private long ReadOwnData(string name, Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase? current)
    {
        RequireContext();
        if (ReferenceEquals(current, null) || current.WasCollected || !_dataHandles.TryGetValue(name, out var retained) ||
            retained.WasCollected || current.Pointer != retained.Pointer || current.Pointer == IntPtr.Zero)
            throw new InvalidOperationException("data-field-identity-changed:" + name);
        return current.Pointer.ToInt64();
    }
    private bool DataHierarchyMatches()
    {
        RequireContext();
        if (_root == null || _dataContainerRoot == null || _dataBoundsRoot == null || _dataContainer == null || _dataBounds == null || _dataUnity.Count != 7)
            return false;
        foreach (var item in _dataUnity.Values)
            if (item.Handle == null || item.Handle.WasCollected || item.Handle.Pointer != item.Pointer) return false;
        var rootTransform = _root.transform;
        if (rootTransform.Pointer != _dataUnity["root-transform"].Pointer || rootTransform.parent != null || rootTransform.childCount != 2 ||
            rootTransform.position != UnityEngine.Vector3.zero || rootTransform.rotation != UnityEngine.Quaternion.identity || rootTransform.localScale != UnityEngine.Vector3.one)
            return false;
        foreach (var pair in new[] { (Name: "container", Root: _dataContainerRoot, Component: (UnityEngine.Component)_dataContainer),
            (Name: "bounds", Root: _dataBoundsRoot, Component: (UnityEngine.Component)_dataBounds) })
        {
            var t = pair.Root.transform;
            if (pair.Root.Pointer != _dataUnity[pair.Name].Pointer || t.Pointer != _dataUnity[pair.Name + "-transform"].Pointer ||
                t.parent == null || t.parent.Pointer != rootTransform.Pointer || t.childCount != 0 || pair.Root.activeSelf || pair.Root.activeInHierarchy ||
                t.localPosition != UnityEngine.Vector3.zero || t.localRotation != UnityEngine.Quaternion.identity || t.localScale != UnityEngine.Vector3.one ||
                pair.Component.gameObject.Pointer != pair.Root.Pointer) return false;
            var components = pair.Root.GetComponents<UnityEngine.Component>();
            if (components.Length != 2 || components.Any(p => p == null || p.WasCollected) ||
                components.Count(p => p.Pointer == t.Pointer) != 1 || components.Count(p => p.Pointer == pair.Component.Pointer) != 1) return false;
        }
        return Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(_dataContainer.Pointer) == Class<Il2CppScheduleOne.Property.PropertyContentsContainer>() &&
            Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(_dataBounds.Pointer) == Class<UnityEngine.BoxCollider>() &&
            !_dataBounds.enabled && _dataBounds.center == UnityEngine.Vector3.zero && _dataBounds.size == new UnityEngine.Vector3(2, 2, 2);
    }
    private long ReadWrapperBinding()
    {
        RequireContext();
        if (_dataWrapper == null) throw new InvalidOperationException("data-wrapper-missing");
        foreach (var spec in new[] { (Owner: typeof(S1API.Property.BusinessWrapper), Name: "InnerBusiness", Type: typeof(Il2CppScheduleOne.Property.Business)),
            (Owner: typeof(S1API.Property.PropertyWrapper), Name: "InnerProperty", Type: typeof(Il2CppScheduleOne.Property.Property)) })
        {
            var field = spec.Owner.GetField(spec.Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.DeclaredOnly);
            if (field == null || field.DeclaringType != spec.Owner || field.FieldType != spec.Type || !field.IsInitOnly ||
                field.GetValue(_dataWrapper) is not Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase value || value.WasCollected || value.Pointer != _businessPointer)
                throw new InvalidOperationException("data-wrapper-binding-changed:" + spec.Name);
        }
        return _businessPointer.ToInt64();
    }
    private BusinessDataState ReadBusinessData(bool requireBacklink = true)
    {
        RequireContext(); ValidateDataMethods(); ReadWrapperBinding();
        if (_business == null || _business.WasCollected || _business.Pointer != _businessPointer || !DataHierarchyMatches())
            throw new InvalidOperationException("data-owned-hierarchy-changed");
        var b = _business; var p = new Il2CppScheduleOne.Property.Property(_businessPointer);
        var pointers = new Dictionary<string, long>(StringComparer.Ordinal); var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        void List<T>(string name, il2cpp::Il2CppSystem.Collections.Generic.List<T> value)
        { pointers.Add(name, ReadOwnData(name, value)); counts.Add(name, value.Count); }
        void Array<T>(string name, Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<T> value) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
        { pointers.Add(name, ReadOwnData(name, value)); counts.Add(name, value.Length); }
        List("laundering", b.LaunderingOperations); List("employees", p._Employees_k__BackingField); List("buildables", p.BuildableItems);
        List("configurables", p.Configurables); List("grids", p.Grids); List("switches", p.Switches); List("toggleables", p.Toggleables);
        List("extra-files", p._LocalExtraFiles_k__BackingField); List("extra-folders", p._LocalExtraFolders_k__BackingField);
        List("saved-objects", p.savedObjectPaths); List("saved-employees", p.savedEmployeePaths);
        Array("bounds", p.propertyBoundsColliders); Array("objects-to-cull", p.ObjectsToCull); Array("employee-idle", p.EmployeeIdlePoints); Array("loading-docks", p.LoadingDocks);
        if (counts.Any(pair => pair.Value != (pair.Key == "bounds" ? 1 : 0))) throw new InvalidOperationException("data-collection-content-changed");
        long businessLoader = ReadOwnData("business-loader", b.loader), propertyLoader = ReadOwnData("property-loader", p.loader);
        var container = p._Container_k__BackingField; var boxRoot = p.BoundingBox; var box = p.propertyBoundsColliders[0];
        if (container == null || container.WasCollected || container.Pointer != _dataContainer!.Pointer || boxRoot == null || boxRoot.WasCollected ||
            boxRoot.Pointer != _dataBoundsRoot!.Pointer || box == null || box.WasCollected || box.Pointer != _dataBounds!.Pointer)
            throw new InvalidOperationException("data-own-reference-changed");
        var back = _dataContainer._Property_k__BackingField;
        if (requireBacklink ? back == null || back.WasCollected || back.Pointer != _businessPointer : !ReferenceEquals(back, null))
            throw new InvalidOperationException("data-container-backlink-changed");
        bool neutral = !p._IsOwned_k__BackingField && !p.OwnedByDefault && !p.ContentCullingEnabled && !p._hasBeenLoaded && !p._HasChanged_k__BackingField &&
            float.IsFinite(p.Price) && p.Price == 0 && p.EmployeeCapacity == 0 && float.IsFinite(b.LaunderCapacity) && b.LaunderCapacity == 0;
        if (!neutral) throw new InvalidOperationException("data-neutral-defaults-changed");
        var state = new BusinessDataState(_dataContainerRoot!.Pointer.ToInt64(), container.Pointer.ToInt64(), back?.Pointer.ToInt64() ?? 0,
            boxRoot.Pointer.ToInt64(), box.Pointer.ToInt64(), businessLoader, propertyLoader, true, neutral, true,
            new ReadOnlyDictionary<string, long>(pointers), new ReadOnlyDictionary<string, int>(counts));
        RequireContext(); _dataLastRead = state; return state;
    }
    private void RequireDataBoundary(bool requireBacklink = true)
    {
        RequireContext();
        ReadBusinessData(requireBacklink);
        var inspection = Inspect();
        var network = ReadNetworkBinding();
        if (!inspection.ExactComponents || !inspection.Unowned || !inspection.OwnNativeClass || !inspection.OwnVirtualTargets ||
            !network.OwnNativeClass || !network.OwnVirtualTargets || !network.OwnUnityMessages || !network.Unspawned || !network.Detached || !network.ReferencesAbsent)
            throw new InvalidOperationException("data-native-boundary-unconfirmed");
        if (!_dataPrepared && (_root!.activeSelf || _root.activeInHierarchy || _business!.Counters().Any(p => p.Value != 0) || network.Counters.Any(p => p.Value != 0)))
            throw new InvalidOperationException("data-preparation-callback-or-activation-changed");
        var current = Capture(); var baseline = _dataForeignBaseline ?? throw new InvalidOperationException("data-baseline-missing");
        if (!SameDataMap(current.Registries, baseline.Registries) || !SameDataMap(current.ForeignVtables, baseline.ForeignVtables))
            throw new InvalidOperationException("data-foreign-state-changed");
        RequireContext();
    }
    private static bool SameDataMap(IReadOnlyDictionary<string, string> first, IReadOnlyDictionary<string, string> second) =>
        first.Count == second.Count && first.All(p => second.TryGetValue(p.Key, out string? value) && p.Value == value);
    public BusinessDataQuery ReadNativeDataQueries()
    {
        RequireContext();
        if (_dataQueryAttempted || !_dataPrepared || !_dataSetPropertyReturned || _activeSetAttempted) throw new InvalidOperationException("data-query-state");
        _dataQueryAttempted = true;
        T Query<T>(string name, Func<T> call, Func<T, bool> accept)
        {
            RequireDataBoundary();
            var before = ReadActiveState(); RequireNetworkMutationBoundary(before);
            if (before.ActiveSelf || before.ActiveInHierarchy) throw new InvalidOperationException("data-query-active");
            RequireContext(); _dataCalls.Add(new { Call = name, Attempted = true, Returned = false });
            T value = call();
            // Preserve even nonfinite scalar returns without performing native reads or breaking JSON diagnostics.
            string? valueText = Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
            _dataCalls.Add(new { Call = name, Attempted = true, Returned = true, Value = valueText, ValueType = typeof(T).Name, ReadbackConfirmed = false });
            RequireContext();
            RequireDataBoundary(); var after = ReadActiveState(); RequireNetworkMutationBoundary(after);
            if (after.ActiveSelf || after.ActiveInHierarchy || before.Counters.Any(p => after.Counters[p.Key] != p.Value) ||
                before.Network!.Counters.Any(p => after.Network!.Counters[p.Key] != p.Value)) throw new InvalidOperationException("data-query-callback-changed");
            if (!accept(value)) throw new InvalidOperationException("data-query-value-unconfirmed:" + name);
            _dataCalls.Add(new { Call = name, Attempted = true, Returned = true, Value = valueText, ValueType = typeof(T).Name, ReadbackConfirmed = true });
            return value;
        }
        float total = Query("CurrentLaunderTotal", () => _dataWrapper!.CurrentLaunderTotal, value => float.IsFinite(value) && value == 0);
        float limit = Query("AppliedLaunderLimit", () => _dataWrapper!.AppliedLaunderLimit, value => float.IsFinite(value) && value == 0);
        bool inside = Query("IsPointInside(center)", () => _dataWrapper!.IsPointInside(UnityEngine.Vector3.zero), value => value);
        bool outside = Query("IsPointInside(outside)", () => _dataWrapper!.IsPointInside(new UnityEngine.Vector3(4, 0, 0)), value => !value);
        RequireDataBoundary();
        return new(_businessPointer.ToInt64(), _dataContainer!.Pointer.ToInt64(), _dataBounds!.Pointer.ToInt64(), ReadWrapperBinding(), total, limit, inside, outside);
    }
    private void ConfirmDataCleanup()
    {
        RequireContext();
        if (_dataPendingHandles.Count != 0) throw new InvalidOperationException("data-unconfirmed-handles-retained");
        foreach (var pair in _dataUnity)
        {
            RequireContext(); var retained = pair.Value;
            if (retained.Handle.WasCollected || retained.Handle.Pointer != retained.Pointer || retained.Handle != null)
                throw new InvalidOperationException("data-unity-cleanup-unconfirmed:" + pair.Key);
        }
        RequireContext();
        _dataHandles.Clear(); _dataWrapper = null; // Release our GC handles; no assertion about immediate native heap collection.
        _dataCleanupVerified = true;
    }
    private void ValidateDataMethods()
    {
        RequireContext();
        void BindData<T>(string name, uint token, string returns, bool privateMethod, params string[] parameters) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
        {
            IntPtr owner = Class<T>();
            IntPtr pointer = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_method_from_name(owner, name, parameters.Length);
            if (pointer == IntPtr.Zero || Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(pointer) != owner)
                throw new InvalidOperationException("data-method-owner:" + name);
            var info = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)pointer);
            int flags = (int)info.Flags;
            if (info.Token != token || Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_name_(pointer) != name ||
                info.ParametersCount != parameters.Length || (flags & (0x10 | 0x40 | 0x400)) != 0 ||
                (flags & 7) != (privateMethod ? 1 : 6) || info.ReturnType == null ||
                Il2CppInterop.Runtime.IL2CPP.il2cpp_type_get_name_((IntPtr)info.ReturnType) != returns ||
                info.MethodPointer == IntPtr.Zero || info.InvokerMethod == IntPtr.Zero || info.VirtualMethodPointer == IntPtr.Zero)
                throw new InvalidOperationException("data-method-metadata:" + name);
            for (int i = 0; i < parameters.Length; i++)
            {
                var type = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap(info.Parameters, i).ParameterType;
                if (type == null || Il2CppInterop.Runtime.IL2CPP.il2cpp_type_get_name_((IntPtr)type) != parameters[i])
                    throw new InvalidOperationException("data-method-signature:" + name);
            }
            string key = typeof(T).FullName + "." + name;
            string value = string.Join("|", Hex(owner), Hex(pointer), Hex(info.MethodPointer), Hex(info.InvokerMethod), Hex(info.VirtualMethodPointer), token, flags, returns, string.Join(",", parameters));
            if (_dataMethods.TryGetValue(key, out string? previous) && previous != value) throw new InvalidOperationException("data-method-changed:" + name);
            _dataMethods[key] = value;
        }
        BindData<Il2CppScheduleOne.Property.Business>("get_currentLaunderTotal", 0x06001398, "System.Single", false);
        BindData<Il2CppScheduleOne.Property.Business>("get_appliedLaunderLimit", 0x06001399, "System.Single", false);
        BindData<Il2CppScheduleOne.Property.PropertyContentsContainer>("SetProperty", 0x06001475, "System.Void", false, "ScheduleOne.Property.Property");
        BindData<Il2CppScheduleOne.Property.Property>("DoBoundsContainPoint", 0x0600143E, "System.Boolean", false, "UnityEngine.Vector3");
        BindData<Il2CppScheduleOne.Property.Property>("IsPointInsideBox", 0x0600143F, "System.Boolean", true, "UnityEngine.Vector3", "UnityEngine.BoxCollider");
        BindData<Il2CppScheduleOne.Persistence.Loaders.BusinessLoader>(".ctor", 0x06001862, "System.Void", false);
        BindData<Il2CppScheduleOne.Persistence.Loaders.PropertyLoader>(".ctor", 0x06001899, "System.Void", false);
        BindData<Il2CppScheduleOne.Persistence.Loaders.Loader>(".ctor", 0x0600187A, "System.Void", false);
        RequireContext();
    }
    private Dictionary<string, string> CaptureDataListeners()
    {
        RequireContext();
        _dataListenerDiagnostics = new Dictionary<string, string>(StringComparer.Ordinal);
        void Diag(string key, string value) => _dataListenerDiagnostics[key] = value;
        var time = Il2CppScheduleOne.DevUtilities.NetworkSingleton<Il2CppScheduleOne.GameTime.TimeManager>.Instance;
        var money = Il2CppScheduleOne.DevUtilities.NetworkSingleton<Il2CppScheduleOne.Money.MoneyManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(time) || !S1Mods.Shared.NetworkGuard.IsAlive(money))
            throw new InvalidOperationException("data-listener-manager-unavailable");
        var recursion = new HashSet<IntPtr>(); int nodes = 0, maxChildren = 0;
        string Identity(Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase? value)
        {
            RequireContext();
            if (ReferenceEquals(value, null)) return "null";
            if (value.WasCollected || value.Pointer == IntPtr.Zero) throw new InvalidOperationException("data-listener-object-unreadable");
            IntPtr pointer = value.Pointer;
            if (pointer == _businessPointer || pointer == _rootPointer || pointer == _networkPointer ||
                _dataUnity.Values.Any(p => p.Pointer == pointer) || _dataHandles.Values.Any(p => p.Pointer == pointer))
                throw new InvalidOperationException("data-listener-own-reference");
            return Hex(pointer);
        }
        string Delegate(il2cpp::Il2CppSystem.Delegate? value, int depth = 0)
        {
            string identity = Identity(value);
            if (ReferenceEquals(value, null)) return identity;
            if (depth > 8 || ++nodes > 16384 || !recursion.Add(value!.Pointer))
                throw new InvalidOperationException("data-listener-graph-over-budget-or-cycle:" + nodes.ToString(System.Globalization.CultureInfo.InvariantCulture));
            var text = new System.Text.StringBuilder(identity).Append(':').Append(Hex(value.method_ptr)).Append(':').Append(Hex(value.invoke_impl))
                .Append(':').Append(Hex(value.method)).Append(':').Append(Identity(value.m_target));
            var multicast = value.TryCast<il2cpp::Il2CppSystem.MulticastDelegate>();
            if (!ReferenceEquals(multicast, null))
            {
                var children = multicast.delegates;
                text.Append(':').Append(Identity(children));
                if (!ReferenceEquals(children, null))
                {
                    int count = children.Length;
                    if (count > maxChildren) maxChildren = count;
                    if (count < 0 || count > 512) throw new InvalidOperationException("data-listener-array-over-budget:" + count.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    text.Append(':').Append(count);
                    for (int i = 0; i < count; i++) text.Append('|').Append(Delegate(children[i], depth + 1));
                    if (children.Length != count || multicast.delegates?.Pointer != children.Pointer) throw new InvalidOperationException("data-listener-array-changed");
                }
            }
            recursion.Remove(value.Pointer); return text.ToString();
        }
        var minute = time.onMinutePass;
        var minuteText = new System.Text.StringBuilder(Identity(time)).Append('|').Append(Identity(minute));
        if (!ReferenceEquals(minute, null))
        {
            var list = minute.list;
            minuteText.Append('|').Append(Identity(list));
            if (!ReferenceEquals(list, null))
            {
                int size = list._size;
                int version = list._version;
                var items = list._items;
                int capacity = items == null ? -1 : items.Length;
                string countProperty;
                bool countPropertyMatches;
                try
                {
                    int count = list.Count;
                    countProperty = count.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    countPropertyMatches = count == size;
                }
                catch (Exception ex)
                {
                    countProperty = "unreadable:" + ex.GetType().Name + ":" + ex.Message;
                    countPropertyMatches = false;
                }
                string diagnostic = Hex(list.Pointer) + "|class=" + Hex(Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(list.Pointer)) +
                    "|size=" + size.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    "|capacity=" + capacity.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    "|items=" + (items == null ? "null" : Hex(items.Pointer)) +
                    "|version=" + version.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    "|countProperty=" + countProperty + "|countPropertyMatchesSize=" + countPropertyMatches.ToString();
                Diag("minute-list", diagnostic);
                if (items == null || items.WasCollected || items.Pointer == IntPtr.Zero)
                    throw new InvalidOperationException("data-minute-list-items-unreadable:" + size.ToString(System.Globalization.CultureInfo.InvariantCulture));
                // Observed 2026-11-06: size 889, capacity 1024. The old fixed 512-entry guard
                // rejected a valid list. Bound by the container's own capacity and the existing
                // 4096 ceiling instead of an arbitrary entry count.
                if (size < 0 || capacity < size || capacity > 4096)
                    throw new InvalidOperationException("data-minute-list-capacity-invalid:" + capacity.ToString(System.Globalization.CultureInfo.InvariantCulture));
                minuteText.Append('|').Append(size).Append('|').Append(version);
                try
                {
                    for (int i = 0; i < size; i++) minuteText.Append('|').Append(Delegate(items[i]));
                }
                catch (Exception ex)
                {
                    Diag("minute-list", diagnostic + "|nodes=" + nodes.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                        "|maxChildren=" + maxChildren.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                        "|failure=" + ex.GetType().Name + ":" + ex.Message);
                    throw;
                }
                Diag("minute-list", diagnostic + "|nodes=" + nodes.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    "|maxChildren=" + maxChildren.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (list._size != size || list._version != version || list._items?.Pointer != items.Pointer || minute.list?.Pointer != list.Pointer)
                    throw new InvalidOperationException("data-minute-list-changed");
            }
            if (time.onMinutePass?.Pointer != minute.Pointer) throw new InvalidOperationException("data-minute-holder-changed");
        }
        minuteText.Append('|').Append(Delegate(time.onTimeSkip));
        var result = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["data-time-listeners"] = Digest(minuteText.ToString()),
            ["data-money-listeners"] = Digest(Identity(money) + "|" + Delegate(money.onNetworthCalculation)),
            ["data-property-listeners"] = Digest(Delegate(Il2CppScheduleOne.Property.Property.onPropertyAcquired)),
            ["data-business-listeners"] = Digest(Delegate(Il2CppScheduleOne.Property.Business.onOperationStarted) + "|" + Delegate(Il2CppScheduleOne.Property.Business.onOperationFinished))
        };
        RequireContext(); return result;
    }
#endif
    public SubtypeReadback Inspect()
    {
        if (_root == null || _business == null || _destroyed || _business.WasCollected) throw new InvalidOperationException("own-object-missing");
        var components = _root.GetComponents<UnityEngine.Component>();
        if (components.Length > 16) throw new InvalidOperationException("components-over-budget");
        _components.Clear();
        int transforms = 0, businesses = 0;
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        int networks = 0;
#endif
        foreach (var component in components)
        {
            if (component == null || component.WasCollected) throw new InvalidOperationException("component-unreadable");
            IntPtr nativeClass = Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(component.Pointer);
            _components.Add(new { Type = component.GetIl2CppType().FullName, Pointer = Hex(component.Pointer), NativeClass = Hex(nativeClass) });
            if (nativeClass == Class<UnityEngine.Transform>()) transforms++;
            if (nativeClass == _ownClass && component.Pointer == _businessPointer) businesses++;
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
            if (nativeClass == _networkClass && component.Pointer == _networkPointer) networks++;
#endif
        }
        bool targets = _baseMethods.Count == 16;
        var ownWrap = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)_ownClass);
        if (ownWrap.VTable == IntPtr.Zero || ownWrap.VtableCount <= 0) throw new InvalidOperationException("own-vtable-unreadable");
        foreach (var entry in _baseMethods)
        {
            int slotIndex = _baseSlots[entry.Key];
            if (slotIndex < 0 || slotIndex >= ownWrap.VtableCount) throw new InvalidOperationException("base-slot-out-of-range:" + entry.Key);
            var vtableSlot = ((Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)ownWrap.VTable)[slotIndex];
            bool vtableMatches = (IntPtr)vtableSlot.method == _ownMethods[entry.Key] &&
                Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class((IntPtr)vtableSlot.method) == _ownClass &&
                vtableSlot.methodPtr == _ownMethodPointers[entry.Key] &&
                vtableSlot.methodPtr != MethodPointer(entry.Value);

            IntPtr resolved = Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_virtual_method(_businessPointer, entry.Value);
            bool resolvedMatches = resolved == _ownMethods[entry.Key] &&
                Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(resolved) == _ownClass &&
                MethodPointer(resolved) == _ownMethodPointers[entry.Key] &&
                MethodPointer(resolved) != MethodPointer(entry.Value);

            targets &= resolvedMatches || vtableMatches;
        }
        int lifecycle = _business.Counters().Where(p => p.Key != "ShouldSave" && p.Key != "CanBePurchased").Sum(p => p.Value) - _expectedInterfaceCallbacks;
        return new(!_root.activeSelf && !_root.activeInHierarchy,
#if TOWNHALL_BUSINESS_DATA
            components.Length == 3 && transforms == 1 && businesses == 1 && networks == 1 && DataHierarchyMatches(),
#elif TOWNHALL_BUSINESS_NETWORK_SUBTYPE
            components.Length == 3 && transforms == 1 && businesses == 1 && networks == 1 && _root.transform.childCount == 0,
#else
            components.Length == 2 && transforms == 1 && businesses == 1 && _root.transform.childCount == 0,
#endif
            !_business.IsOwned && _business.propertyCode == BusinessConstructionTrial.PropertyCode && _business.ConstructedWhileArmed &&
            ProbeBusinessSubtype.UnarmedConstructs == 0 && _business.propertyName == "Hyland Town Hall" &&
            _business.LocalExtraFiles?.Count == 0 && _business.LocalExtraFolders?.Count == 0,
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
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
    public ActiveSubtypeState ReadActiveState()
    {
        RequireContext();
        var inspection = Inspect();
        if (_root == null || _business == null || _root.Pointer != _rootPointer || _business.Pointer != _businessPointer || _root.WasCollected)
            throw new InvalidOperationException("active-owned-identity-unreadable");
        ValidateUnityMessages();
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save)) throw new InvalidOperationException("active-manager-unavailable");
        bool absent = !OwnReference(Il2CppScheduleOne.Property.Property.Properties) &
            !OwnReference(Il2CppScheduleOne.Property.Property.OwnedProperties) &
            !OwnReference(Il2CppScheduleOne.Property.Property.UnownedProperties) &
            !OwnReference(Il2CppScheduleOne.Property.Business.Businesses) &
            !OwnReference(Il2CppScheduleOne.Property.Business.OwnedBusinesses) &
            !OwnReference(Il2CppScheduleOne.Property.Business.UnownedBusinesses) &
            !OwnReference(save.Saveables) & !OwnReference(save.BaseSaveables) & !OwnReference(save.CompletedSaveables) &
            !OwnReference(save.QueuedSaveRequests, request =>
            {
                var target = request.Saveable;
                if (target != null && target.WasCollected) throw new InvalidOperationException("active-queue-target-unreadable");
                return target?.Pointer ?? IntPtr.Zero;
            });
        _lastObservedUnityFrame = UnityEngine.Time.frameCount;
        var result = new ActiveSubtypeState(BusinessSubtypeRuntime.CurrentContext(), BusinessSubtypeRuntime.Generation,
            _lastObservedUnityFrame.Value, _root.Pointer.ToInt64(), _business.Pointer.ToInt64(), _ownClass.ToInt64(), save.Pointer.ToInt64(),
            _root.activeSelf, _root.activeInHierarchy, inspection.ExactComponents && _business.enabled,
            inspection.Unowned, inspection.OwnNativeClass && inspection.OwnVirtualTargets,
            _unityMessages.Count == UnityMessageNames.Length, absent, !save.IsSaving && save.QueuedSaveRequests.Count == 0, _business.Counters());
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        result = result with { Network = ReadNetworkBinding() };
#if TOWNHALL_BUSINESS_DATA
        result = result with { Data = ReadBusinessData() };
#endif
        if (_nativeNetworkBinding is { } bound && (result.Generation != bound.Generation || result.Root != bound.Root ||
            result.Business != bound.Business || result.OwnClass != bound.OwnClass || result.Manager != bound.Manager ||
            !SameNativeWorld(result.Context, bound.Context))) throw new InvalidOperationException("network-native-world-or-instance-changed");
        if (_nativeNetworkLastRead is { } last && (result.UnityFrame < last.UnityFrame ||
            last.Counters.Any(p => !result.Counters.TryGetValue(p.Key, out int count) || count < p.Value) ||
            last.Network!.Counters.Any(p => !result.Network!.Counters.TryGetValue(p.Key, out int count) || count < p.Value)))
            throw new InvalidOperationException("network-native-observation-regressed");
        _nativeNetworkBinding ??= result;
        _nativeNetworkLastRead = result;
#endif
        RequireContext(); return result;
    }
    public void SetActive(bool active)
    {
        RequireContext(); var state = ReadActiveState();
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        RequireNetworkMutationBoundary(state);
#endif
        if (!state.ExactComponents || !state.Unowned || !state.OwnVirtualTargets || !state.OwnUnityMessages ||
            !state.ReferencesAbsent || !state.SaveIdle) throw new InvalidOperationException("active-mutation-boundary-unconfirmed");
        if (active)
        {
            if (_activeSetAttempted || state.ActiveSelf || state.ActiveInHierarchy) throw new InvalidOperationException("activation-repeated-or-invalid");
            _activeSetAttempted = true;
        }
        else
        {
            if (!_activeSetAttempted || _inactiveSetAttempted || !state.ActiveSelf || !state.ActiveInHierarchy)
                throw new InvalidOperationException("deactivation-repeated-or-invalid");
            _inactiveSetAttempted = true;
        }
#if TOWNHALL_BUSINESS_DATA
        RequireDataBoundary();
#endif
        RequireContext(); _root!.SetActive(active);
    }
    public IReadOnlyDictionary<string, int> ReadFinalCounters() => _business?.Counters() ?? throw new InvalidOperationException("active-counters-missing");
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
    public IReadOnlyDictionary<string, int> ReadFinalNetworkCounters() => _network?.Counters() ?? throw new InvalidOperationException("network-counters-missing");
    private void RequireNetworkMutationBoundary(ActiveSubtypeState state)
    {
        var network = state.Network;
        if (!state.ExactComponents || !state.Unowned || !state.OwnVirtualTargets || !state.OwnUnityMessages ||
            state.Root != _rootPointer.ToInt64() || state.Business != _businessPointer.ToInt64() || state.OwnClass != _ownClass.ToInt64() ||
            !state.ReferencesAbsent || !state.SaveIdle || network == null || network.Network != _networkPointer.ToInt64() || network.OwnClass != _networkClass.ToInt64() ||
            network.BehaviourArray != _behaviourArrayPointer.ToInt64() || network.BusinessNetworkObject != network.Network ||
            network.BusinessComponentIndex != 0 || network.BehaviourCount != 1 || network.FirstBehaviour != _businessPointer.ToInt64() ||
            !network.OwnNativeClass || !network.OwnVirtualTargets || !network.OwnUnityMessages || !network.Unspawned || !network.Detached || !network.ReferencesAbsent)
            throw new InvalidOperationException("network-mutation-boundary-unconfirmed");
        if (!state.Counters.TryGetValue("ShouldSave", out int saveCalls) || saveCalls != 1 ||
            !state.Counters.TryGetValue("CanBePurchased", out int purchaseCalls) || purchaseCalls != 1)
            throw new InvalidOperationException("network-property-dispatch-changed");
        foreach (var calls in new[] { state.Counters, network.Counters })
        {
            foreach (var entry in calls)
            {
                if (entry.Value < 0 || entry.Value > 10000 ||
                    (entry.Key is "Awake" or "Start" or "OnEnable" && entry.Value > 1) ||
                    (entry.Key == "OnDestroy" && entry.Value != 0) ||
                    (entry.Key == "OnDisable" && entry.Value != (_inactiveSetAttempted ? 1 : 0)) ||
                    (!UnityMessageNames.Contains(entry.Key) && entry.Key is not "ShouldSave" and not "CanBePurchased" && entry.Value != 0))
                    throw new InvalidOperationException("network-mutation-callback-unconfirmed");
            }
        }
        if (!_activeSetAttempted && (state.Counters.Any(p => UnityMessageNames.Contains(p.Key) && p.Value != 0) || network.Counters.Any(p => p.Value != 0)))
            throw new InvalidOperationException("network-callback-before-activation");
        if (network.Counters.Any(p => !UnityMessageNames.Contains(p.Key) && p.Value != 0)) throw new InvalidOperationException("network-unexpected-callback");
    }
    private static bool SameNativeWorld(ConstructionContext current, ConstructionContext bound) =>
        ConstructionGate.Check(current) == null && string.Equals(current.OrganisationName, bound.OrganisationName, StringComparison.Ordinal) &&
        string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(current.ActiveSavePath!)), Path.TrimEndingDirectorySeparator(Path.GetFullPath(bound.ActiveSavePath!)), StringComparison.OrdinalIgnoreCase) &&
        string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(current.LoadedSavePath!)), Path.TrimEndingDirectorySeparator(Path.GetFullPath(bound.LoadedSavePath!)), StringComparison.OrdinalIgnoreCase);
    private static bool NullNativeReference(Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase? value) => ReferenceEquals(value, null);
    private void PrepareNetworkBinding()
    {
        RequireContext();
        if (_network == null || _business == null || _network.Pointer != _networkPointer || !_network.ConstructedWhileArmed || ProbeNetworkSubtype.UnarmedConstructs != 0)
            throw new InvalidOperationException("network-construction-unconfirmed");
        _network._ObjectId_k__BackingField = Il2CppFishNet.Object.NetworkObject.UNSET_OBJECTID_VALUE;
        _network._SceneId_k__BackingField = 0;
        _network.State = Il2CppFishNet.Object.NetworkObjectState.Unset;
        _network._isNetworked = false;
        _network._IsNested_k__BackingField = false;
        _network._IsDeinitializing_k__BackingField = false;
        _network._IsServerInitialized_k__BackingField = false;
        _network._IsClientInitialized_k__BackingField = false;
        RequireContext();
        _network._ChildNetworkObjects_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppFishNet.Object.NetworkObject>();
        _network._RuntimeChildNetworkObjects_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppFishNet.Object.NetworkObject>();
        _network.Observers = new Il2CppSystem.Collections.Generic.HashSet<Il2CppFishNet.Connection.NetworkConnection>();
        RequireContext();
        _network._networkBehaviours = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<Il2CppFishNet.Object.NetworkBehaviour>(1);
        _behaviourArrayPointer = _network._networkBehaviours.Pointer;
        _network._networkBehaviours[0] = _business;
        RequireContext();
        _business._networkObjectCache = _network;
        _business._componentIndexCache = 0;
    }
    private NetworkSubtypeState ReadNetworkBinding()
    {
        RequireContext();
        if (_network == null || _network.WasCollected || _network.Pointer != _networkPointer || _business == null)
            throw new InvalidOperationException("network-instance-unreadable");
        ValidateNetworkUnityMessages();
        var behaviours = _network._networkBehaviours;
        if (behaviours == null || behaviours.WasCollected || behaviours.Pointer != _behaviourArrayPointer || behaviours.Length != 1)
            throw new InvalidOperationException("network-behaviour-array-unreadable");
        var first = behaviours[0];
        if (first == null || first.WasCollected) throw new InvalidOperationException("network-behaviour-entry-unreadable");
        var cache = _business._networkObjectCache;
        if (cache == null || cache.WasCollected) throw new InvalidOperationException("business-network-cache-unreadable");
        bool detached = NullNativeReference(_network._ParentNetworkObject_k__BackingField) && NullNativeReference(_network._RuntimeParentNetworkObject_k__BackingField) &&
            NullNativeReference(_network._RuntimeParentTransform_k__BackingField) && !_network._IsNested_k__BackingField &&
            EmptyList(_network._ChildNetworkObjects_k__BackingField) && EmptyList(_network._RuntimeChildNetworkObjects_k__BackingField) &&
            EmptySet(_network.Observers) && NullNativeReference(_network._owner) && NullNativeReference(_network._PredictedSpawner_k__BackingField);
        bool unspawned = _network._ObjectId_k__BackingField == Il2CppFishNet.Object.NetworkObject.UNSET_OBJECTID_VALUE &&
            _network._SceneId_k__BackingField == 0 && _network.State == Il2CppFishNet.Object.NetworkObjectState.Unset && !_network._isNetworked &&
            !_network._IsDeinitializing_k__BackingField && !_network._IsClientInitialized_k__BackingField && !_network._IsServerInitialized_k__BackingField &&
            NullNativeReference(_network._NetworkManager_k__BackingField) && NullNativeReference(_network._ServerManager_k__BackingField) && NullNativeReference(_network._ClientManager_k__BackingField) &&
            NullNativeReference(_network._ObserverManager_k__BackingField) && NullNativeReference(_network._TransportManager_k__BackingField) && NullNativeReference(_network._TimeManager_k__BackingField) &&
            NullNativeReference(_network._SceneManager_k__BackingField) && NullNativeReference(_network._PredictionManager_k__BackingField) && NullNativeReference(_network._RollbackManager_k__BackingField) &&
            NullNativeReference(_network._hashGrid) && NullNativeReference(_network.HashGridEntry) && !_business._initializedOnceServer && !_business._initializedOnceClient &&
            NullNativeReference(_business._transportManagerCache);
        var holders = CaptureNetworkHolders(out bool referencesAbsent);
        _networkBindingDetails = new
        {
            Network = Hex(_networkPointer),
            BusinessNetworkObject = Hex(cache.Pointer),
            BusinessComponentIndex = _business._componentIndexCache,
            BehaviourArray = Hex(behaviours.Pointer),
            BehaviourCount = behaviours.Length,
            FirstBehaviour = Hex(first.Pointer),
            ObjectId = _network._ObjectId_k__BackingField,
            ExpectedUnsetObjectId = Il2CppFishNet.Object.NetworkObject.UNSET_OBJECTID_VALUE,
            SceneId = _network._SceneId_k__BackingField,
            State = _network.State.ToString(),
            IsNetworked = _network._isNetworked,
            IsClientInitialized = _network._IsClientInitialized_k__BackingField,
            IsServerInitialized = _network._IsServerInitialized_k__BackingField,
            Detached = detached,
            Unspawned = unspawned,
            KnownReferencesAbsent = referencesAbsent,
            KnownHolderFingerprints = holders,
            Scope = "Direct known FishNet object/connection registries and empty client cache; not arbitrary delegate closure or native last-use proof"
        };
        return new(_networkPointer.ToInt64(), _networkClass.ToInt64(), cache.Pointer.ToInt64(), _business._componentIndexCache,
            behaviours.Pointer.ToInt64(), behaviours.Length, first.Pointer.ToInt64(),
            Il2CppInterop.Runtime.IL2CPP.il2cpp_object_get_class(_networkPointer) == _networkClass && _network.ConstructedWhileArmed &&
            ProbeNetworkSubtype.UnarmedConstructs == 0 && _network.enabled, NetworkVirtualTargetsMatch(), _networkUnityMessages.Count == UnityMessageNames.Length,
            unspawned, detached, referencesAbsent, _network.Counters());
    }
    private void ValidateNetworkUnityMessages()
    {
        foreach (string name in UnityMessageNames)
        {
            IntPtr pointer = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_method_from_name(_networkClass, name, 0);
            if (pointer == IntPtr.Zero || Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(pointer) != _networkClass)
                throw new InvalidOperationException("network-unity-message-owner:" + name);
            var info = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)pointer);
            if (info.ParametersCount != 0 || ((int)info.Flags & (0x10 | 0x400)) != 0 || info.ReturnType == null ||
                Il2CppInterop.Runtime.IL2CPP.il2cpp_type_get_name_((IntPtr)info.ReturnType) != "System.Void" ||
                Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_name_(pointer) != name ||
                info.MethodPointer == IntPtr.Zero || info.InvokerMethod == IntPtr.Zero || info.VirtualMethodPointer == IntPtr.Zero)
                throw new InvalidOperationException("network-unity-message-signature:" + name);
            var current = (pointer, info.MethodPointer, info.InvokerMethod, info.VirtualMethodPointer);
            if (_networkUnityMessages.TryGetValue(name, out var prior))
            {
                if (prior != current) throw new InvalidOperationException("network-unity-message-changed:" + name);
            }
            else
            {
                if (_activeSetAttempted) throw new InvalidOperationException("network-message-not-bound-before-activation");
                _networkUnityMessages.Add(name, current);
                _networkUnityEvidence.Add(new
                {
                    Name = name,
                    OwnClass = Hex(_networkClass),
                    Method = Hex(pointer),
                    MethodPointer = Hex(info.MethodPointer),
                    InvokerMethod = Hex(info.InvokerMethod),
                    VirtualMethodPointer = Hex(info.VirtualMethodPointer),
                    NonvirtualBaseShadow = name is "OnDisable" or "OnDestroy",
                    NativeDeliveryObservedByCountersOnly = true
                });
            }
        }
        if (!NetworkVirtualTargetsMatch()) throw new InvalidOperationException("network-virtual-targets-mismatch");
    }
    private bool NetworkVirtualTargetsMatch()
    {
        var own = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppClass*)_networkClass);
        if (own.VTable == IntPtr.Zero || own.VtableCount is 0 or > 256) return false;
        foreach (string name in new[] { "Awake", "Start" })
        {
            IntPtr source = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_method_from_name(Class<Il2CppFishNet.Object.NetworkObject>(), name, 0);
            if (source == IntPtr.Zero || !_networkUnityMessages.TryGetValue(name, out var expected)) return false;
            var original = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)source);
            if (original.Slot >= own.VtableCount) return false;
            var slot = ((Il2CppInterop.Runtime.Runtime.VirtualInvokeData*)own.VTable)[original.Slot];
            if ((IntPtr)slot.method != expected.Method || slot.methodPtr != expected.Pointer || slot.methodPtr == original.MethodPointer) return false;
        }
        return true;
    }
    private static bool EmptyList<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase =>
        list != null && !list.WasCollected && list.Pointer != IntPtr.Zero && list.Count == 0;
    private static bool EmptySet<T>(Il2CppSystem.Collections.Generic.HashSet<T> set) =>
        set != null && !set.WasCollected && set.Pointer != IntPtr.Zero && set.Count == 0;
    private Dictionary<string, string> CaptureNetworkHolders(out bool referencesAbsent)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        bool found = false;
        var connections = new Dictionary<IntPtr, Il2CppFishNet.Connection.NetworkConnection>();
        void RequireObject(Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase? value)
        {
            if (value == null || value.WasCollected || value.Pointer == IntPtr.Zero) throw new InvalidOperationException("network-holder-unreadable");
        }
        void Check(Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase? value)
        {
            RequireObject(value);
            found |= (_networkPointer != IntPtr.Zero && value!.Pointer == _networkPointer) || (_businessPointer != IntPtr.Zero && value!.Pointer == _businessPointer);
        }
        void Connection(Il2CppFishNet.Connection.NetworkConnection value)
        {
            RequireObject(value);
            if (connections.Count >= 256 && !connections.ContainsKey(value.Pointer)) throw new InvalidOperationException("network-connections-over-budget");
            connections[value.Pointer] = value;
        }
        void List<T>(string key, il2cpp::Il2CppSystem.Collections.Generic.List<T> list) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
        {
            RequireObject(list);
            int count = list.Count;
            if (count < 0 || count > 8192) throw new InvalidOperationException("network-list-over-budget");
            var text = new System.Text.StringBuilder(Hex(list.Pointer)).Append('|').Append(count);
            for (int i = 0; i < count; i++) { var item = list[i]; Check(item); text.Append('|').Append(Hex(item.Pointer)); }
            if (list.Count != count) throw new InvalidOperationException("network-list-changed");
            result.Add(key, count + ":" + Digest(text.ToString()));
        }
        void Dictionary<TKey, TValue>(string key, il2cpp::Il2CppSystem.Collections.Generic.Dictionary<TKey, TValue> dictionary,
            Func<TKey, string> keyText, Action<TValue>? visit = null) where TValue : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
        {
            RequireObject(dictionary);
            int count = dictionary.Count;
            if (count < 0 || count > 8192) throw new InvalidOperationException("network-dictionary-over-budget");
            var text = new System.Text.StringBuilder(Hex(dictionary.Pointer)).Append('|').Append(count);
            int seen = 0;
            foreach (var entry in dictionary)
            {
                if (++seen > count) throw new InvalidOperationException("network-dictionary-changed");
                Check(entry.Value); visit?.Invoke(entry.Value);
                text.Append('|').Append(keyText(entry.Key)).Append('=').Append(Hex(entry.Value.Pointer));
            }
            if (seen != count || dictionary.Count != count) throw new InvalidOperationException("network-dictionary-changed");
            result.Add(key, count + ":" + Digest(text.ToString()));
        }
        string Set<T>(Il2CppSystem.Collections.Generic.HashSet<T> set) where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
        {
            RequireObject(set);
            int count = set.Count;
            if (count < 0 || count > 8192) throw new InvalidOperationException("network-set-over-budget");
            var pointers = new System.Collections.Generic.List<long>();
            foreach (var item in set) { Check(item); pointers.Add(item.Pointer.ToInt64()); if (pointers.Count > count) throw new InvalidOperationException("network-set-changed"); }
            if (pointers.Count != count || set.Count != count) throw new InvalidOperationException("network-set-changed");
            pointers.Sort();
            return count + ":" + Digest(Hex(set.Pointer) + "|" + string.Join("|", pointers));
        }
        var instances = Il2CppFishNet.Managing.NetworkManager._instances;
        RequireObject(instances);
        if (instances.Count != 1) throw new InvalidOperationException("network-manager-ambiguous");
        var manager = instances[0]; RequireObject(manager);
        if (!S1Mods.Shared.NetworkGuard.IsAlive(manager) || !manager._Initialized_k__BackingField) throw new InvalidOperationException("network-manager-unready");
        var server = manager._ServerManager_k__BackingField;
        var client = manager._ClientManager_k__BackingField;
        RequireObject(server); RequireObject(client);
        var sobs = server._Objects_k__BackingField;
        var cobs = client._Objects_k__BackingField;
        RequireObject(sobs); RequireObject(cobs);
        if (server._NetworkManager_k__BackingField?.Pointer != manager.Pointer || client._NetworkManager_k__BackingField?.Pointer != manager.Pointer ||
            sobs._NetworkManager_k__BackingField?.Pointer != manager.Pointer || cobs._NetworkManager_k__BackingField?.Pointer != manager.Pointer)
            throw new InvalidOperationException("network-manager-binding-mismatch");
        result.Add("network-holders", Digest(string.Join("|", new[] { instances.Pointer, manager.Pointer, server.Pointer, client.Pointer, sobs.Pointer, cobs.Pointer }.Select(Hex))));
        Dictionary("network-server-spawned", sobs.Spawned, key => key.ToString(System.Globalization.CultureInfo.InvariantCulture));
        Dictionary("network-server-scene", sobs.SceneObjects_Internal, key => key.ToString(System.Globalization.CultureInfo.InvariantCulture));
        Dictionary("network-client-spawned", cobs.Spawned, key => key.ToString(System.Globalization.CultureInfo.InvariantCulture));
        Dictionary("network-client-scene", cobs.SceneObjects_Internal, key => key.ToString(System.Globalization.CultureInfo.InvariantCulture));
        Dictionary("network-pending-destroy", sobs._pendingDestroy, key => key.ToString(System.Globalization.CultureInfo.InvariantCulture));
        List("network-spawn-cache", sobs._spawnCache);
        List("network-observer-changed", sobs._observerChangedObjectsCache);
        List("network-timed-observers", sobs._timedNetworkObservers);
        List("network-dirty-sync-vars", sobs._dirtySyncVarBehaviours);
        List("network-dirty-sync-objects", sobs._dirtySyncObjectBehaviours);
        List("network-local-client-spawned", cobs.LocalClientSpawned);
        Dictionary("network-registered-components", manager._registeredComponents, key => key);
        Dictionary("network-server-connections", server.Clients, key => key.ToString(System.Globalization.CultureInfo.InvariantCulture), Connection);
        Dictionary("network-client-connections", client.Clients, key => key.ToString(System.Globalization.CultureInfo.InvariantCulture), Connection);
        Connection(client.Connection);
        var connectionStates = new System.Collections.Generic.List<string>();
        foreach (var connection in connections.Values.OrderBy(c => c.Pointer.ToInt64()))
        {
            if (connection._NetworkManager_k__BackingField?.Pointer != manager.Pointer) throw new InvalidOperationException("network-connection-owner-mismatch");
            string objects = Set(connection.Objects);
            var first = connection._FirstObject_k__BackingField;
            if (first != null) Check(first);
            string lodKey = "network-connection-lod-temporary";
            Dictionary(lodKey, connection.LevelOfDetails, nob => { Check(nob); return Hex(nob.Pointer); });
            string lod = result[lodKey]; result.Remove(lodKey);
            connectionStates.Add(Hex(connection.Pointer) + "|" + objects + "|" + (first == null ? "null" : Hex(first.Pointer)) + "|" + lod);
        }
        result.Add("network-connection-owned", connections.Count + ":" + Digest(string.Join(";", connectionStates)));
        var ids = sobs._objectIdCache; RequireObject(ids);
        if (ids.Count < 0 || ids.Count > 65536) throw new InvalidOperationException("network-id-cache-over-budget");
        int idCount = ids.Count;
        var idValues = ids.ToArray();
        if (ids.Count != idCount || idValues.Length != idCount) throw new InvalidOperationException("network-id-cache-changed");
        result.Add("network-object-id-cache", idCount + ":" + Digest(Hex(ids.Pointer) + "|" + string.Join("|", idValues.ToArray())));
        var cache = cobs._objectCache; RequireObject(cache);
        if (cache._networkManager?.Pointer != manager.Pointer || cache._clientObjects?.Pointer != cobs.Pointer)
            throw new InvalidOperationException("network-client-cache-binding");
        RequireObject(cache.IteratedSpawningObjects); RequireObject(cache._cachedObjects);
        if (cache.IteratedSpawningObjects.Count != 0 || cache._cachedObjects.Count != 0 || !EmptySet(cache._iteratedSpawns) ||
            !EmptySet(cache.ReadSpawningObjects) || !EmptySet(cache._conflictingDespawns))
            throw new InvalidOperationException("network-client-cache-not-readably-empty");
        result.Add("network-client-cache", Digest(string.Join("|", new[] { cache.Pointer, cache.IteratedSpawningObjects.Pointer,
            cache._cachedObjects.Pointer, cache._iteratedSpawns.Pointer, cache.ReadSpawningObjects.Pointer, cache._conflictingDespawns.Pointer }.Select(Hex))));
        if (instances.Count != 1 || instances[0]?.Pointer != manager.Pointer) throw new InvalidOperationException("network-manager-changed");
        if (_networkHolderBaseline != null && (_networkHolderBaseline.Count != result.Count ||
            _networkHolderBaseline.Any(p => !result.TryGetValue(p.Key, out string? value) || value != p.Value)))
            throw new InvalidOperationException("network-holder-identity-or-content-changed");
        _networkHolderBaseline ??= new Dictionary<string, string>(result, StringComparer.Ordinal);
        referencesAbsent = !found;
        return result;
    }
#endif
    private void ValidateUnityMessages()
    {
        foreach (string name in UnityMessageNames)
        {
            IntPtr pointer = Il2CppInterop.Runtime.IL2CPP.il2cpp_class_get_method_from_name(_ownClass, name, 0);
            if (pointer == IntPtr.Zero || Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_class(pointer) != _ownClass)
                throw new InvalidOperationException("unity-message-owner-mismatch:" + name);
            var info = Il2CppInterop.Runtime.Runtime.UnityVersionHandler.Wrap((Il2CppInterop.Runtime.Runtime.Il2CppMethodInfo*)pointer);
            if (info.ParametersCount != 0 || ((int)info.Flags & (0x10 | 0x400)) != 0 || info.ReturnType == null ||
                Il2CppInterop.Runtime.IL2CPP.il2cpp_type_get_name_((IntPtr)info.ReturnType) != "System.Void" ||
                Il2CppInterop.Runtime.IL2CPP.il2cpp_method_get_name_(pointer) != name ||
                info.MethodPointer == IntPtr.Zero || info.InvokerMethod == IntPtr.Zero || info.VirtualMethodPointer == IntPtr.Zero)
                throw new InvalidOperationException("unity-message-signature-or-invoker-invalid:" + name);
            var current = (pointer, info.MethodPointer, info.InvokerMethod, info.VirtualMethodPointer);
            if (_unityMessages.TryGetValue(name, out var prior))
            {
                if (prior != current) throw new InvalidOperationException("unity-message-changed:" + name);
            }
            else
            {
                if (_activeSetAttempted) throw new InvalidOperationException("unity-message-not-bound-before-activation:" + name);
                _unityMessages.Add(name, current);
                _unityMessageEvidence.Add(new
                {
                    Name = name,
                    Method = Hex(pointer),
                    OwnClass = Hex(_ownClass),
                    MethodPointer = Hex(info.MethodPointer),
                    InvokerMethod = Hex(info.InvokerMethod),
                    VirtualMethodPointer = Hex(info.VirtualMethodPointer),
                    Parameters = info.ParametersCount,
                    ReturnType = "System.Void",
                    NativeDeliveryObservedByCountersOnly = true
                });
            }
        }
    }
    private bool OwnReference<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T> list, Func<T, IntPtr>? target = null)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        if (list == null || list.WasCollected || list.Pointer == IntPtr.Zero || list.Count < 0 || list.Count > 8192)
            throw new InvalidOperationException("active-reference-list-unreadable");
        int count = list.Count;
        bool found = false;
        for (int i = 0; i < count; i++)
        {
            var item = list[i];
            if (item == null) continue;
            if (item.WasCollected) throw new InvalidOperationException("active-reference-entry-unreadable");
            found |= (target == null ? item.Pointer : target(item)) == _businessPointer;
        }
        if (list.Count != count) throw new InvalidOperationException("active-reference-list-changed");
        return found;
    }
#endif
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
#if TOWNHALL_BUSINESS_ACTIVE_SUBTYPE
        var activeState = ReadActiveState();
        if (_activeCleanupAttempted || !_inactiveSetAttempted || activeState.ActiveSelf || activeState.ActiveInHierarchy ||
            !activeState.ReferencesAbsent || !activeState.SaveIdle) throw new InvalidOperationException("active-cleanup-boundary-unconfirmed");
        _activeCleanupAttempted = true;
        RequireContext();
#endif
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        RequireNetworkMutationBoundary(activeState);
        _networkBeforeCleanup = _network!.Counters();
#endif
        _beforeCleanup ??= _business?.Counters();
#if TOWNHALL_BUSINESS_DATA
        RequireDataBoundary();
#endif
        if (_root.Pointer != _rootPointer || _root.WasCollected) throw new InvalidOperationException("cleanup-identity-unreadable");
        if (!_destroyed && (UnityEngine.Object)_root != null) UnityEngine.Object.DestroyImmediate(_root);
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        RequireContext();
#endif
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
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        if (ReferenceEquals(_network, null) || _network!.WasCollected || _network.Pointer != _networkPointer || (UnityEngine.Object)_network != null)
            throw new InvalidOperationException("network-cleanup-absence-unconfirmed");
        var networkAfter = _network!.Counters();
        foreach (var entry in networkAfter)
        {
            int prior = _networkBeforeCleanup?.GetValueOrDefault(entry.Key) ?? 0;
            if (entry.Key == "OnDestroy" ? entry.Value - prior is < 0 or > 1 : entry.Value != prior)
                throw new InvalidOperationException("network-cleanup-unexpected-callback:" + entry.Key);
        }
#endif
#if TOWNHALL_BUSINESS_DATA
        ConfirmDataCleanup();
#endif
        _cleanupVerified = true;
    }
    public bool IsClean => ReferenceEquals(_root, null) || (_destroyed && _cleanupVerified && !_root.WasCollected &&
        _root.Pointer == _rootPointer && (UnityEngine.Object)_root == null
#if TOWNHALL_BUSINESS_NETWORK_SUBTYPE
        && !ReferenceEquals(_network, null) && !_network!.WasCollected && _network.Pointer == _networkPointer && (UnityEngine.Object)_network == null
#endif
        );
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
    public ManagerSaveState ReadSaveState()
    {
        RequireContext();
        var context = BusinessSubtypeRuntime.CurrentContext();
        long generation = BusinessSubtypeRuntime.Generation;
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        long managerPtr = (save != null && S1Mods.Shared.NetworkGuard.IsAlive(save)) ? save.Pointer.ToInt64() : 0;
        long saveablesPtr = (save?.Saveables != null && !save.Saveables.WasCollected) ? save.Saveables.Pointer.ToInt64() : 0;
        long frame = NextFrame();
        bool isSaving = save != null && save.IsSaving;
        int pending = save?.QueuedSaveRequests?.Count ?? 0;
        int ownEntries = (_businessPointer != IntPtr.Zero && save?.Saveables != null) ? CountPointer(save.Saveables, _businessPointer) : 0;
        int activeCallbacks = 0;
        if (_saveStarted == 0 && _business?.SaveCallbackObserved == true)
            _saveStarted = 1;
        if (_saveCompleted == 0 && save != null && save.CompletedSaveables != null && _businessPointer != IntPtr.Zero && CountPointer(save.CompletedSaveables, _businessPointer) > 0)
            _saveCompleted = 1;
        int started = _saveStarted;
        int completed = _saveCompleted;
        string foreignSaveables = ForeignSaveables(save?.Saveables);
        bool verified = _registered &&
            _baseMethods.Count == 16 &&
            _business != null &&
            _business.ConstructedWhileArmed &&
            ProbeBusinessSubtype.UnarmedConstructs == 0 &&
            _business.LocalExtraFiles != null &&
            _business.LocalExtraFolders != null &&
            _business.LocalExtraFiles.Count == 0 &&
            _business.LocalExtraFolders.Count == 0 &&
            !_business.ShouldSaveUnderFolder;
        return new ManagerSaveState(context, generation, managerPtr, saveablesPtr, frame,
            isSaving, pending, ownEntries, activeCallbacks, started, completed, foreignSaveables, verified);
    }
    public void RegisterSaveable()
    {
        RequireContext();
        if (_business == null || _businessPointer == IntPtr.Zero) throw new InvalidOperationException("manager-instance-missing");
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save) || save.IsSaving) throw new InvalidOperationException("saving-or-unavailable");
        var list = save.Saveables ?? throw new InvalidOperationException("saveables-missing");
        if (CountPointer(list, _businessPointer) != 0) throw new InvalidOperationException("already-registered");
        save.RegisterSaveable(new Il2CppScheduleOne.Persistence.ISaveable(_businessPointer));
    }
    public void SaveOnce()
    {
        RequireContext();
        if (_business == null || _businessPointer == IntPtr.Zero) throw new InvalidOperationException("manager-instance-missing");
        var load = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
        string? folder = load?.LoadedGameFolderPath;
        if (string.IsNullOrEmpty(folder)) throw new InvalidOperationException("loaded-game-folder-missing");
        _saveStarted = 1;
        new Il2CppScheduleOne.Persistence.ISaveable(_businessPointer).Save(folder);
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (save != null && save.CompletedSaveables != null && CountPointer(save.CompletedSaveables, _businessPointer) > 0)
        {
            _saveCompleted = 1;
        }
    }
    public void RemoveSaveable()
    {
        RequireContext();
        if (_businessPointer == IntPtr.Zero) throw new InvalidOperationException("manager-instance-missing");
        var save = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.SaveManager>.Instance;
        if (!S1Mods.Shared.NetworkGuard.IsAlive(save)) throw new InvalidOperationException("saving-or-unavailable");
        if (save.Saveables != null)
        {
            RemovePointer(save.Saveables, _businessPointer);
        }
        save.ClearCompletedSaveable(new Il2CppScheduleOne.Persistence.ISaveable(_businessPointer));
        if (save.CompletedSaveables != null)
        {
            RemovePointer(save.CompletedSaveables, _businessPointer);
        }
    }
    private string ForeignSaveables(il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Persistence.ISaveable>? list)
    {
        if (list == null || list.WasCollected || list.Count < 0 || list.Count > 8192) return "";
        int count = list.Count;
        int foreign = 0;
        var value = new System.Text.StringBuilder(Hex(list.Pointer));
        for (int i = 0; i < count; i++)
        {
            var item = list[i];
            if (item != null && item.WasCollected) throw new InvalidOperationException("registry-entry-unreadable");
            if (item != null && item.Pointer == _businessPointer) continue;
            foreign++;
            value.Append('|').Append(item == null ? "null" : Hex(item.Pointer));
        }
        if (list.Count != count) throw new InvalidOperationException("registry-changed-during-read");
        return foreign + ":" + Digest(value.ToString());
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

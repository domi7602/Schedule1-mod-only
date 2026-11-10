using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

// Exercises the production owner through an offline port. Native getter bodies,
// Unity resources and real delegate/registry inspection need a separate live trial.
public sealed class BusinessDataTests
{
    private static readonly string[] CollectionNames =
    {
        "laundering", "employees", "buildables", "configurables", "grids", "switches", "toggleables",
        "saved-objects", "saved-employees", "extra-files", "extra-folders", "bounds", "objects-to-cull",
        "employee-idle", "loading-docks"
    };

    private static readonly string[] ListenerNames =
    {
        "data-time-listeners", "data-money-listeners", "data-property-listeners", "data-business-listeners"
    };

    private static ConstructionContext World
    {
        get
        {
            string path = Path.Combine(Path.GetTempPath(), "TownHallBusinessDataTests", "SaveGame_2");
            return new(true, true, true, true, path, path, "TownHallTest");
        }
    }

    [Fact]
    public void InactiveQueriesPrecedeActivationAndAllOwnedDataMustRetireOnce()
    {
        var port = new Port();
        var trial = Begin(port);

        Assert.True(trial.Result.DataQueryAttempted);
        Assert.Equal(port.Query, trial.Result.DataQuery);
        Assert.Single(port.Events, e => e == "query");
        Assert.True(port.Events.IndexOf("read") < port.Events.IndexOf("query"));
        Assert.True(port.Events.IndexOf("query") < port.Events.IndexOf("activate"));
        Assert.False(port.ActiveWhenQueried);
        Assert.False(trial.Result.CleanupAttempted);

        port.AdvanceFrame();
        var result = trial.Tick(0.2);

        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.CleanupComplete);
        Assert.True(port.DataDestroyed);
        Assert.True(port.Events.IndexOf("cleanup") < port.Events.IndexOf("data-clean"));
        Assert.Single(port.Events, e => e == "query");
        Assert.Single(port.Events, e => e == "cleanup");
        Assert.Equal(1, port.BusinessCounters["OnDestroy"]);
        Assert.Equal(1, port.NetworkCounters["OnDestroy"]);
        AssertTerminal(trial, port);
    }

    [Fact]
    public void DataModeRequiresDataPortBeforeAnyNativeWork()
    {
        var port = new Port();
        var trial = new BusinessActiveSubtypeTrial(requireData: true);

        var result = trial.Begin(World, new NetworkOnlyPort(port), () => true, 0);

        Assert.Equal("skipped", result.Status);
        Assert.False(trial.AttemptConsumed);
        Assert.Empty(port.Events);
    }

    [Fact]
    public void DataModeAlsoRequiresTheNetworkParticipant()
    {
        var port = new Port();
        port.State = port.State with { Network = null };
        var trial = new BusinessActiveSubtypeTrial(requireData: true);

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.False(result.DataQueryAttempted);
        Assert.DoesNotContain("query", port.Events);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void ExistingNetworkModeCannotSilentlyCertifyUnexpectedData()
    {
        var port = new Port();
        var trial = new BusinessActiveSubtypeTrial(requireNetwork: true);

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.DoesNotContain("query", port.Events);
        Assert.DoesNotContain("activate", port.Events);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("container-root-zero")]
    [InlineData("container-zero")]
    [InlineData("container-property-zero")]
    [InlineData("container-property-foreign")]
    [InlineData("bounds-root-zero")]
    [InlineData("bounds-collider-zero")]
    [InlineData("business-loader-zero")]
    [InlineData("property-loader-zero")]
    [InlineData("foreign-hierarchy")]
    [InlineData("nonneutral-defaults")]
    [InlineData("unverified-query-targets")]
    [InlineData("container-alias-business")]
    [InlineData("container-root-alias-root")]
    [InlineData("bounds-root-alias-container-root")]
    [InlineData("bounds-collider-alias-container")]
    [InlineData("business-loader-alias-network")]
    [InlineData("property-loader-alias-business-loader")]
    public void IncompleteOrAliasedDataNeverReachesNativeQueries(string invalid)
    {
        var port = new Port();
        var data = port.Data;
        port.State = port.State with
        {
            Data = invalid switch
            {
                "missing" => null,
                "container-root-zero" => data with { ContainerRoot = 0 },
                "container-zero" => data with { Container = 0 },
                "container-property-zero" => data with { ContainerProperty = 0 },
                "container-property-foreign" => data with { ContainerProperty = 999 },
                "bounds-root-zero" => data with { BoundsRoot = 0 },
                "bounds-collider-zero" => data with { BoundsCollider = 0 },
                "business-loader-zero" => data with { BusinessLoader = 0 },
                "property-loader-zero" => data with { PropertyLoader = 0 },
                "foreign-hierarchy" => data with { OwnedHierarchy = false },
                "nonneutral-defaults" => data with { NeutralDefaults = false },
                "unverified-query-targets" => data with { QueryTargetsVerified = false },
                "container-alias-business" => data with { Container = port.State.Business },
                "container-root-alias-root" => data with { ContainerRoot = port.State.Root },
                "bounds-root-alias-container-root" => data with { BoundsRoot = data.ContainerRoot },
                "bounds-collider-alias-container" => data with { BoundsCollider = data.Container },
                "business-loader-alias-network" => data with { BusinessLoader = port.State.Network!.Network },
                _ => data with { PropertyLoader = data.BusinessLoader }
            }
        };

        AssertRejectedBeforeQuery(port);
    }

    [Theory]
    [InlineData("missing-pointer-map")]
    [InlineData("missing-count-map")]
    [InlineData("missing-laundering")]
    [InlineData("missing-bounds-count")]
    [InlineData("extra-pointer")]
    [InlineData("extra-count")]
    [InlineData("zero-list")]
    [InlineData("aliased-lists")]
    [InlineData("list-alias-business")]
    [InlineData("list-alias-container")]
    [InlineData("list-alias-loader")]
    [InlineData("list-alias-behaviour-array")]
    [InlineData("nonempty-laundering")]
    [InlineData("nonempty-employees")]
    [InlineData("negative-count")]
    [InlineData("no-bounds")]
    [InlineData("extra-bounds")]
    public void QueryDependenciesNeedExactOwnedCollectionIdentitiesAndExpectedContents(string invalid)
    {
        var port = new Port();
        switch (invalid)
        {
            case "missing-pointer-map": port.Data = port.Data with { Collections = null! }; break;
            case "missing-count-map": port.Data = port.Data with { CollectionCounts = null! }; break;
            case "missing-laundering": port.Collections.Remove("laundering"); break;
            case "missing-bounds-count": port.Counts.Remove("bounds"); break;
            case "extra-pointer": port.Collections["foreign"] = 999; break;
            case "extra-count": port.Counts["foreign"] = 0; break;
            case "zero-list": port.Collections["employees"] = 0; break;
            case "aliased-lists": port.Collections["employees"] = port.Collections["laundering"]; break;
            case "list-alias-business": port.Collections["employees"] = port.State.Business; break;
            case "list-alias-container": port.Collections["employees"] = port.Data.Container; break;
            case "list-alias-loader": port.Collections["employees"] = port.Data.BusinessLoader; break;
            case "list-alias-behaviour-array": port.Collections["employees"] = port.State.Network!.BehaviourArray; break;
            case "nonempty-laundering": port.Counts["laundering"] = 1; break;
            case "nonempty-employees": port.Counts["employees"] = 1; break;
            case "negative-count": port.Counts["buildables"] = -1; break;
            case "no-bounds": port.Counts["bounds"] = 0; break;
            case "extra-bounds": port.Counts["bounds"] = 2; break;
        }

        AssertRejectedBeforeQuery(port);
    }

    [Theory]
    [InlineData("business")]
    [InlineData("wrapper-business")]
    [InlineData("container")]
    [InlineData("bounds")]
    [InlineData("total-nonzero")]
    [InlineData("total-negative")]
    [InlineData("total-nan")]
    [InlineData("total-infinity")]
    [InlineData("limit-nonzero")]
    [InlineData("limit-negative")]
    [InlineData("limit-nan")]
    [InlineData("limit-infinity")]
    [InlineData("inside-false")]
    [InlineData("outside-true")]
    public void NativeQueryMustDescribeThisBusinessAndBothSpatialControlPoints(string invalid)
    {
        var port = new Port();
        port.Query = invalid switch
        {
            "business" => port.Query with { Business = 999 },
            "wrapper-business" => port.Query with { WrapperBusiness = 999 },
            "container" => port.Query with { Container = 999 },
            "bounds" => port.Query with { BoundsCollider = 999 },
            "total-nonzero" => port.Query with { CurrentLaunderTotal = 1 },
            "total-negative" => port.Query with { CurrentLaunderTotal = -1 },
            "total-nan" => port.Query with { CurrentLaunderTotal = float.NaN },
            "total-infinity" => port.Query with { CurrentLaunderTotal = float.PositiveInfinity },
            "limit-nonzero" => port.Query with { AppliedLaunderLimit = 1 },
            "limit-negative" => port.Query with { AppliedLaunderLimit = -1 },
            "limit-nan" => port.Query with { AppliedLaunderLimit = float.NaN },
            "limit-infinity" => port.Query with { AppliedLaunderLimit = float.NegativeInfinity },
            "inside-false" => port.Query with { Inside = false },
            _ => port.Query with { Outside = true }
        };
        var trial = new BusinessActiveSubtypeTrial(requireData: true);

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.True(result.DataQueryAttempted);
        Assert.Single(port.Events, e => e == "query");
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Fact]
    public void MissingQueryResultIsNotReplacedWithDefaultZeroSuccess()
    {
        var port = new Port { MissingQuery = true };
        var trial = new BusinessActiveSubtypeTrial(requireData: true);

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.True(result.DataQueryAttempted);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("container-root")]
    [InlineData("container")]
    [InlineData("bounds-root")]
    [InlineData("bounds-collider")]
    [InlineData("business-loader")]
    [InlineData("property-loader")]
    [InlineData("collection")]
    public void QueryCannotAuthorizeActivationAfterItsOwnedInputIdentityChanges(string changed)
    {
        var port = new Port();
        port.After = stage => { if (stage == "query") ChangeIdentity(port, changed); };
        var trial = new BusinessActiveSubtypeTrial(requireData: true);

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.True(result.DataQueryAttempted);
        Assert.Single(port.Events, e => e == "query");
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("container")]
    [InlineData("bounds-collider")]
    [InlineData("business-loader")]
    [InlineData("collection")]
    public void LaterDataIdentityChangeDoesNotUseTheOldQueryToDestroyNewObjects(string changed)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        ChangeIdentity(port, changed);

        var result = trial.Tick(0.2);

        Assert.Equal("quarantined", result.Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
        Assert.Single(port.Events, e => e == "query");
        AssertTerminal(trial, port);
    }

    [Fact]
    public void ResultOwnsDataMapsAndQueryEvidenceAfterThePortChanges()
    {
        var port = new Port();
        var trial = Begin(port);
        var saved = trial.Result;
        long original = port.Collections["laundering"];
        var originalQuery = port.Query;
        port.Collections["laundering"] = 999;
        port.Counts["laundering"] = 4;
        port.Query = port.Query with { CurrentLaunderTotal = 42 };

        Assert.Equal(original, saved.Before!.Data!.Collections["laundering"]);
        Assert.Equal(0, saved.Before.Data.CollectionCounts["laundering"]);
        Assert.Equal(originalQuery, saved.DataQuery);
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, long>)saved.Before.Data.Collections)["laundering"] = 55);
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, int>)saved.Before.Data.CollectionCounts)["laundering"] = 55);
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.Equal(999, port.Collections["laundering"]);
        Assert.Equal(4, port.Counts["laundering"]);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("data-time-listeners")]
    [InlineData("data-money-listeners")]
    [InlineData("data-property-listeners")]
    [InlineData("data-business-listeners")]
    public void MissingListenerEvidenceStopsBeforeAnyDataPreparation(string missing)
    {
        var port = new Port();
        port.Registries.Remove(missing);
        var trial = new BusinessActiveSubtypeTrial(requireData: true);

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.DoesNotContain("prepare", port.Events);
        Assert.DoesNotContain("query", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("data-time-listeners", "prepare")]
    [InlineData("data-time-listeners", "query")]
    [InlineData("data-money-listeners", "query")]
    [InlineData("data-property-listeners", "waiting")]
    [InlineData("data-business-listeners", "cleanup")]
    public void ChangedExternalListenersArePreservedAndFailTheTrial(string holder, string phase)
    {
        var port = new Port();
        port.After = stage => { if (stage == phase) port.Registries[holder] = "foreign-new-delegate-same-count"; };
        var trial = new BusinessActiveSubtypeTrial(requireData: true);
        trial.Begin(World, port, () => true, 0);
        if (phase == "waiting") port.Registries[holder] = "foreign-new-delegate-same-count";
        if (trial.Pending) { port.AdvanceFrame(); trial.Tick(0.2); }

        Assert.Equal("quarantined", trial.Result.Status);
        Assert.False(trial.Result.CleanupComplete);
        Assert.Equal("foreign-new-delegate-same-count", port.Registries[holder]);
        if (phase != "cleanup") Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData(false, "InitializeSaveable")]
    [InlineData(false, "GetSaveString")]
    [InlineData(false, "OnEnable")]
    [InlineData(true, "NetworkInitializeIfDisabled")]
    [InlineData(true, "Awake")]
    public void NativeQueryCannotTriggerSaveInitializationOrActivationCallbacks(bool network, string callback)
    {
        var port = new Port();
        port.After = stage =>
        {
            if (stage == "query") (network ? port.NetworkCounters : port.BusinessCounters)[callback] = 1;
        };
        var trial = new BusinessActiveSubtypeTrial(requireData: true);

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.True(result.DataQueryAttempted);
        Assert.Null(result.DataQuery);
        Assert.Single(port.Events, e => e == "query");
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("prepare")]
    [InlineData("query")]
    [InlineData("cleanup")]
    public void UnknownPartialDataEffectsAreNeverRetriedOrSpeculativelyReleased(string stage)
    {
        var port = new Port { ThrowAfter = stage };
        var trial = new BusinessActiveSubtypeTrial(requireData: true);
        bool intentObservedInsideQuery = false;
        port.After = current =>
        {
            if (current == "query") intentObservedInsideQuery = trial.Result.DataQueryAttempted;
        };

        trial.Begin(World, port, () => true, 0);
        if (trial.Pending) { port.AdvanceFrame(); trial.Tick(0.2); }

        Assert.Equal("quarantined", trial.Result.Status);
        Assert.False(trial.Result.CleanupComplete);
        Assert.Single(port.Events, e => e == stage);
        if (stage != "prepare") Assert.True(intentObservedInsideQuery);
        if (stage != "cleanup") Assert.DoesNotContain("cleanup", port.Events);
        port.ThrowAfter = null;
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("query")]
    [InlineData("data-clean")]
    public void ContextLossInsideNewDataCallsStopsEveryLaterPortCall(string phase)
    {
        bool valid = true;
        var port = new Port();
        var trial = new BusinessActiveSubtypeTrial(requireData: true);
        int callsAtLoss = -1;
        port.After = stage =>
        {
            if (stage != phase) return;
            valid = false;
            callsAtLoss = port.Events.Count;
        };

        trial.Begin(World, port, () => valid, 0);
        if (trial.Pending) { port.AdvanceFrame(); trial.Tick(0.2); }

        Assert.True(callsAtLoss > 0);
        Assert.Equal("quarantined", trial.Result.Status);
        Assert.False(trial.Result.CleanupComplete);
        Assert.Equal(callsAtLoss, port.Events.Count);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("leave")]
    [InlineData("tick")]
    [InlineData("begin")]
    public void ReentrantQueryRevocationConsumesTheAttemptWithoutFurtherNativeWork(string entry)
    {
        var port = new Port();
        var other = new Port();
        var trial = new BusinessActiveSubtypeTrial(requireData: true);
        int callsAtRevocation = -1;
        port.After = stage =>
        {
            if (stage != "query") return;
            if (entry == "leave") trial.Leave();
            else if (entry == "tick") trial.Tick(0.1);
            else trial.Begin(World, other, () => true, 0.1);
            callsAtRevocation = port.Events.Count;
        };

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.True(result.DataQueryAttempted);
        Assert.False(result.ActivationAttempted);
        Assert.Empty(other.Events);
        Assert.Equal(callsAtRevocation, port.Events.Count);
        AssertTerminal(trial, port);
    }

    [Fact]
    public void DestroyingOnlyTheRootCannotCertifyOwnedDataCleanup()
    {
        var port = new Port { LeaveDataAlive = true };
        var trial = Begin(port);
        port.AdvanceFrame();

        var result = trial.Tick(0.2);

        Assert.True(port.Destroyed);
        Assert.False(port.DataDestroyed);
        Assert.True(result.CleanupAttempted);
        Assert.False(result.CleanupComplete);
        Assert.Equal("quarantined", result.Status);
        Assert.Single(port.Events, e => e == "cleanup");
        AssertTerminal(trial, port);
    }

    private static void ChangeIdentity(Port port, string changed)
    {
        if (changed == "collection") { port.Collections["laundering"] = 999; return; }
        port.Data = changed switch
        {
            "container-root" => port.Data with { ContainerRoot = 999 },
            "container" => port.Data with { Container = 999 },
            "bounds-root" => port.Data with { BoundsRoot = 999 },
            "bounds-collider" => port.Data with { BoundsCollider = 999 },
            "business-loader" => port.Data with { BusinessLoader = 999 },
            _ => port.Data with { PropertyLoader = 999 }
        };
    }

    private static BusinessActiveSubtypeTrial Begin(Port port)
    {
        var trial = new BusinessActiveSubtypeTrial(requireData: true);
        Assert.Equal("waiting", trial.Begin(World, port, () => true, 0).Status);
        return trial;
    }

    private static void AssertRejectedBeforeQuery(Port port)
    {
        var trial = new BusinessActiveSubtypeTrial(requireData: true);
        var result = trial.Begin(World, port, () => true, 0);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.DataQueryAttempted);
        Assert.DoesNotContain("query", port.Events);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    private static void AssertTerminal(BusinessActiveSubtypeTrial trial, Port port)
    {
        int calls = port.Events.Count;
        var other = new Port();
        trial.Tick(6);
        trial.Leave();
        trial.Begin(World, other, () => true, 7);
        Assert.Equal(calls, port.Events.Count);
        Assert.Empty(other.Events);
        Assert.False(trial.Pending);
    }

    private sealed class Port : IBusinessDataPort
    {
        public readonly List<string> Events = new();
        public readonly Dictionary<string, int> BusinessCounters = new(StringComparer.Ordinal);
        public readonly Dictionary<string, int> NetworkCounters = new(StringComparer.Ordinal);
        public readonly Dictionary<string, long> Collections = new(StringComparer.Ordinal);
        public readonly Dictionary<string, int> Counts = new(StringComparer.Ordinal);
        public readonly Dictionary<string, string> Registries = new(StringComparer.Ordinal);
        public readonly Dictionary<string, string> Vtables = new(StringComparer.Ordinal)
        {
            ["Business"] = "unchanged-business-vtable",
            ["NetworkObject"] = "unchanged-network-vtable"
        };
        public ActiveSubtypeState State;
        public BusinessDataQuery Query;
        public Action<string>? After;
        public string? ThrowAfter;
        public bool Destroyed, DataDestroyed, LeaveDataAlive, MissingQuery, ActiveWhenQueried;
        public BusinessDataState Data { get => State.Data!; set => State = State with { Data = value }; }

        public Port()
        {
            foreach (string name in new[] { "properties", "properties-owned", "properties-unowned", "businesses",
                "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests",
                "network-client", "network-server", "network-observers" }.Concat(ListenerNames))
                Registries[name] = "unchanged:" + name;
            for (int i = 0; i < CollectionNames.Length; i++)
            {
                Collections[CollectionNames[i]] = 401 + i;
                Counts[CollectionNames[i]] = CollectionNames[i] == "bounds" ? 1 : 0;
            }
            var network = new NetworkSubtypeState(201, 203, 201, 0, 205, 1, 102,
                true, true, true, true, true, true, NetworkCounters);
            var data = new BusinessDataState(301, 302, 102, 303, 304, 305, 306,
                true, true, true, Collections, Counts);
            State = new(World, 1, 100, 101, 102, 103, 104,
                false, false, true, true, true, true, true, true, BusinessCounters, network, data);
            Query = new(102, 302, 304, 102, 0, 0, true, false);
        }

        public void AdvanceFrame()
        {
            State = State with { UnityFrame = State.UnityFrame + 1 };
            foreach (var counters in new[] { BusinessCounters, NetworkCounters })
            {
                counters["Start"] = 1;
                counters["FixedUpdate"] = counters.GetValueOrDefault("FixedUpdate") + 1;
            }
        }

        private void Enter(string stage) => Events.Add(stage);
        private void Exit(string stage)
        {
            After?.Invoke(stage);
            if (ThrowAfter == stage) throw new IOException("unknown-effect-after-" + stage);
        }
        public SubtypeSnapshot Capture() { Enter("capture"); Exit("capture"); return new(Registries, Vtables); }
        public void RegisterType() { Enter("register"); Exit("register"); }
        public void Prepare() { Enter("prepare"); Exit("prepare"); }
        public SubtypeReadback Inspect()
        {
            Enter("inspect"); Exit("inspect");
            return new(!State.ActiveSelf && !State.ActiveInHierarchy, true, true, true, true, 0);
        }
        public SubtypeDispatch InvokeThroughNativeBase()
        {
            Enter("invoke");
            BusinessCounters["ShouldSave"] = BusinessCounters.GetValueOrDefault("ShouldSave") + 1;
            BusinessCounters["CanBePurchased"] = BusinessCounters.GetValueOrDefault("CanBePurchased") + 1;
            Exit("invoke");
            return new(false, false, BusinessCounters["ShouldSave"], BusinessCounters["CanBePurchased"]);
        }
        public ActiveSubtypeState ReadActiveState() { Enter("read"); Exit("read"); return State; }
        public BusinessDataQuery ReadNativeDataQueries()
        {
            Enter("query"); ActiveWhenQueried = State.ActiveSelf || State.ActiveInHierarchy;
            var result = Query; Exit("query"); return MissingQuery ? null! : result;
        }
        public void SetActive(bool active)
        {
            string stage = active ? "activate" : "deactivate";
            Enter(stage); State = State with { ActiveSelf = active, ActiveInHierarchy = active };
            foreach (var counters in new[] { BusinessCounters, NetworkCounters })
            {
                if (active)
                {
                    counters["Awake"] = counters.GetValueOrDefault("Awake") + 1;
                    counters["OnEnable"] = counters.GetValueOrDefault("OnEnable") + 1;
                }
                else counters["OnDisable"] = counters.GetValueOrDefault("OnDisable") + 1;
            }
            Exit(stage);
        }
        public void Cleanup()
        {
            Enter("cleanup"); Destroyed = true; DataDestroyed = !LeaveDataAlive;
            BusinessCounters["OnDestroy"] = BusinessCounters.GetValueOrDefault("OnDestroy") + 1;
            NetworkCounters["OnDestroy"] = NetworkCounters.GetValueOrDefault("OnDestroy") + 1;
            Exit("cleanup");
        }
        public bool IsClean { get { Enter("is-clean"); Exit("is-clean"); return Destroyed; } }
        public bool DataCleanupVerified { get { Enter("data-clean"); Exit("data-clean"); return DataDestroyed; } }
        public IReadOnlyDictionary<string, int> ReadFinalCounters()
        {
            Enter("final-business-counters"); Exit("final-business-counters"); return BusinessCounters;
        }
        public IReadOnlyDictionary<string, int> ReadFinalNetworkCounters()
        {
            Enter("final-network-counters"); Exit("final-network-counters"); return NetworkCounters;
        }
    }

    private sealed class NetworkOnlyPort(Port inner) : IBusinessNetworkSubtypePort
    {
        public SubtypeSnapshot Capture() => inner.Capture();
        public void RegisterType() => inner.RegisterType();
        public void Prepare() => inner.Prepare();
        public SubtypeReadback Inspect() => inner.Inspect();
        public SubtypeDispatch InvokeThroughNativeBase() => inner.InvokeThroughNativeBase();
        public void Cleanup() => inner.Cleanup();
        public bool IsClean => inner.IsClean;
        public ActiveSubtypeState ReadActiveState() => inner.ReadActiveState();
        public void SetActive(bool active) => inner.SetActive(active);
        public IReadOnlyDictionary<string, int> ReadFinalCounters() => inner.ReadFinalCounters();
        public IReadOnlyDictionary<string, int> ReadFinalNetworkCounters() => inner.ReadFinalNetworkCounters();
    }
}

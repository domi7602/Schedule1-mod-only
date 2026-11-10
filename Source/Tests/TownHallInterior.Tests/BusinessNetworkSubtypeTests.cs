using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

// Production orchestration with an offline environment. These cases do not execute
// FishNet, Unity, the injected classes, or native registry/ownership inspection.
public sealed class BusinessNetworkSubtypeTests
{
    private static ConstructionContext World
    {
        get
        {
            string path = Path.Combine(Path.GetTempPath(), "TownHallNetworkSubtypeTests", "SaveGame_2");
            return new(true, true, true, true, path, path, "TownHallTest");
        }
    }

    [Fact]
    public void BothParticipantsNeedLaterFrameLifecycleBeforeOneOrderedRetirement()
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame(network: false);

        Assert.Equal("waiting", trial.Tick(0.1).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);

        port.NetworkCounters["Start"] = 1;
        port.NetworkCounters["FixedUpdate"] = 1;
        var result = trial.Tick(0.2);

        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.CleanupComplete);
        Assert.True(result.DeactivationAttempted);
        Assert.True(result.CleanupAttempted);
        Assert.Equal(1, port.NetworkCounters["OnDisable"]);
        Assert.Equal(1, port.NetworkCounters["OnDestroy"]);
        Assert.Single(port.Events, e => e == "activate");
        Assert.Single(port.Events, e => e == "deactivate");
        Assert.Single(port.Events, e => e == "cleanup");
        Assert.True(port.Events.IndexOf("deactivate") < port.Events.IndexOf("cleanup"));
        Assert.True(port.Events.IndexOf("cleanup") < port.Events.IndexOf("final-network-counters"));
        Assert.Equal("unchanged:network-observers", result.AfterSnapshot!.Registries["network-observers"]);
        AssertTerminal(trial, port);
    }

    [Fact]
    public void NetworkCallbacksInSameUnityFrameDoNotProveElapsedFrame()
    {
        var port = new Port();
        var trial = Begin(port);
        port.BusinessCounters["Start"] = port.NetworkCounters["Start"] = 1;
        port.BusinessCounters["FixedUpdate"] = port.NetworkCounters["FixedUpdate"] = 1;

        var result = trial.Tick(0.1);

        Assert.Equal("waiting", result.Status);
        Assert.False(result.DeactivationAttempted);
        Assert.False(result.CleanupAttempted);
    }

    [Fact]
    public void NetworkModeRequiresNetworkPortBeforeAnyNativeAccess()
    {
        var inner = new Port();
        var trial = new BusinessActiveSubtypeTrial(requireNetwork: true);

        var result = trial.Begin(World, new BusinessOnlyPort(inner), () => true, 0);

        Assert.Equal("skipped", result.Status);
        Assert.Contains("network-port-required", result.Findings);
        Assert.False(trial.AttemptConsumed);
        Assert.Empty(inner.Events);
    }

    [Fact]
    public void BusinessOnlyModeCannotSilentlyIgnoreAnUnexpectedNetworkParticipant()
    {
        var port = new Port();
        foreach (string key in port.Registries.Keys.Where(k => k.StartsWith("network-", StringComparison.Ordinal)).ToArray())
            port.Registries.Remove(key);
        var trial = new BusinessActiveSubtypeTrial();

        var result = trial.Begin(World, port, () => true, 0);

        Assert.Equal("quarantined", result.Status);
        Assert.Contains("unexpected-network-participant", result.Findings);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("network-zero")]
    [InlineData("class-zero")]
    [InlineData("array-zero")]
    [InlineData("binding-zero")]
    [InlineData("binding-foreign")]
    [InlineData("negative-index")]
    [InlineData("wrong-index")]
    [InlineData("empty-array")]
    [InlineData("extra-behaviour")]
    [InlineData("first-zero")]
    [InlineData("first-foreign")]
    [InlineData("wrong-native-class")]
    [InlineData("wrong-virtual-target")]
    [InlineData("wrong-unity-message")]
    [InlineData("spawned")]
    [InlineData("attached-or-owned")]
    [InlineData("referenced")]
    [InlineData("network-alias-root")]
    [InlineData("network-alias-business")]
    [InlineData("network-alias-manager")]
    [InlineData("class-alias-business-class")]
    [InlineData("class-alias-network")]
    [InlineData("array-alias-root")]
    [InlineData("array-alias-business")]
    [InlineData("array-alias-network")]
    public void InvalidInitialNetworkBindingNeverActivatesOrDestroys(string invalid)
    {
        var port = new Port();
        var network = port.Network;
        port.State = port.State with
        {
            Network = invalid switch
            {
                "missing" => null,
                "network-zero" => network with { Network = 0, BusinessNetworkObject = 0 },
                "class-zero" => network with { OwnClass = 0 },
                "array-zero" => network with { BehaviourArray = 0 },
                "binding-zero" => network with { BusinessNetworkObject = 0 },
                "binding-foreign" => network with { BusinessNetworkObject = 991 },
                "negative-index" => network with { BusinessComponentIndex = -1 },
                "wrong-index" => network with { BusinessComponentIndex = 1 },
                "empty-array" => network with { BehaviourCount = 0 },
                "extra-behaviour" => network with { BehaviourCount = 2 },
                "first-zero" => network with { FirstBehaviour = 0 },
                "first-foreign" => network with { FirstBehaviour = 992 },
                "wrong-native-class" => network with { OwnNativeClass = false },
                "wrong-virtual-target" => network with { OwnVirtualTargets = false },
                "wrong-unity-message" => network with { OwnUnityMessages = false },
                "spawned" => network with { Unspawned = false },
                "attached-or-owned" => network with { Detached = false },
                "referenced" => network with { ReferencesAbsent = false },
                "network-alias-root" => network with { Network = port.State.Root, BusinessNetworkObject = port.State.Root },
                "network-alias-business" => network with { Network = port.State.Business, BusinessNetworkObject = port.State.Business },
                "network-alias-manager" => network with { Network = port.State.Manager, BusinessNetworkObject = port.State.Manager },
                "class-alias-business-class" => network with { OwnClass = port.State.OwnClass },
                "class-alias-network" => network with { OwnClass = network.Network },
                "array-alias-root" => network with { BehaviourArray = port.State.Root },
                "array-alias-business" => network with { BehaviourArray = port.State.Business },
                "array-alias-network" => network with { BehaviourArray = network.Network },
                _ => throw new ArgumentOutOfRangeException(nameof(invalid))
            }
        };
        var trial = new BusinessActiveSubtypeTrial(requireNetwork: true);

        Assert.Equal("quarantined", trial.Begin(World, port, () => true, 0).Status);
        Assert.True(trial.AttemptConsumed);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("network")]
    [InlineData("class")]
    [InlineData("array")]
    [InlineData("business-link")]
    [InlineData("behaviour")]
    [InlineData("index")]
    public void ChangedNetworkIdentityDuringObservationCannotReleaseEitherParticipant(string changed)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.Network = changed switch
        {
            "network" => port.Network with { Network = 777, BusinessNetworkObject = 777 },
            "class" => port.Network with { OwnClass = 778 },
            "array" => port.Network with { BehaviourArray = 779 },
            "business-link" => port.Network with { BusinessNetworkObject = 780 },
            "behaviour" => port.Network with { FirstBehaviour = 781 },
            _ => port.Network with { BusinessComponentIndex = 1 }
        };

        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("Awake")]
    [InlineData("Start")]
    [InlineData("FixedUpdate")]
    [InlineData("OnEnable")]
    [InlineData("OnDisable")]
    [InlineData("OnDestroy")]
    public void NetworkLifecycleBeforeActivationIsNotAnInactiveBinding(string callback)
    {
        var port = new Port();
        port.NetworkCounters[callback] = 1;
        var trial = new BusinessActiveSubtypeTrial(requireNetwork: true);

        Assert.Equal("quarantined", trial.Begin(World, port, () => true, 0).Status);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("Awake")]
    [InlineData("Start")]
    [InlineData("OnEnable")]
    public void NetworkOneShotLifecycleCannotBeObservedTwice(string callback)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.NetworkCounters[callback] = 2;

        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("NetworkInitializeIfDisabled")]
    [InlineData("Initialize")]
    [InlineData("OnStartServer")]
    [InlineData("OnStartClient")]
    [InlineData("ShouldSave")]
    [InlineData("GetSaveString")]
    [InlineData("OnDisable")]
    [InlineData("OnDestroy")]
    public void NativeInitializationOrOtherUnexpectedNetworkCallbackQuarantines(string callback)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.NetworkCounters[callback] = 1;

        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("negative")]
    [InlineData("oversized-value")]
    [InlineData("empty-name")]
    [InlineData("oversized-name")]
    [InlineData("oversized-map")]
    public void MalformedNetworkCountersNeverBecomeLifecycleEvidence(string invalid)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        if (invalid == "missing") port.Network = port.Network with { Counters = null! };
        else if (invalid == "negative") port.NetworkCounters["FixedUpdate"] = -1;
        else if (invalid == "oversized-value") port.NetworkCounters["FixedUpdate"] = 10001;
        else if (invalid == "empty-name") port.NetworkCounters[""] = 0;
        else if (invalid == "oversized-name") port.NetworkCounters[new string('x', 65)] = 0;
        else for (int i = 0; i < 33; i++) port.NetworkCounters["unknown-" + i] = 0;

        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void ResultSnapshotsOwnFrozenNetworkCountersAndForeignFingerprints()
    {
        var port = new Port();
        var trial = Begin(port);
        var prior = trial.Result;
        port.NetworkCounters["Awake"] = 50;
        port.NetworkCounters["Start"] = 8;
        port.Registries["network-observers"] = "external-change";

        Assert.Equal(0, prior.Before!.Network!.Counters.GetValueOrDefault("Awake"));
        Assert.Equal(1, prior.Activated!.Network!.Counters["Awake"]);
        Assert.Equal(0, prior.Activated.Network.Counters.GetValueOrDefault("Start"));
        Assert.Equal("unchanged:network-observers", prior.BeforeSnapshot!.Registries["network-observers"]);
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, int>)prior.Activated.Network.Counters)["Awake"] = 99);
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.Equal("external-change", port.Registries["network-observers"]);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("Start", 1)]
    [InlineData("Start", 2)]
    [InlineData("Start", 3)]
    [InlineData("Start", 4)]
    [InlineData("Start", 5)]
    [InlineData("FixedUpdate", 1)]
    [InlineData("FixedUpdate", 2)]
    [InlineData("FixedUpdate", 3)]
    [InlineData("FixedUpdate", 4)]
    [InlineData("FixedUpdate", 5)]
    public void EveryFreshReadHonoursTheLatestNetworkCounterFloor(string callback, int readToRegress)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        int reads = 0;
        port.After = stage =>
        {
            if (stage == "read" && ++reads == readToRegress) port.NetworkCounters[callback] = 0;
        };

        var result = trial.Tick(0.2);

        if (readToRegress == 1)
        {
            // Start/FixedUpdate have not yet been observed by the trial in this tick.
            Assert.Equal("waiting", result.Status);
        }
        else Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupAttempted);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void EarlierNetworkTickObservationCannotBeErasedOnTheNextTick()
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.NetworkCounters.Remove("Start");
        Assert.Equal("waiting", trial.Tick(0.1).Status);
        port.NetworkCounters["FixedUpdate"] = 0;

        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TimeoutOnlyRetiresWithFreshNetworkReferenceAbsence(bool referenced)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame(network: false);
        if (referenced) port.Network = port.Network with { ReferencesAbsent = false };

        var result = trial.Tick(BusinessActiveSubtypeTrial.TimeoutSeconds + 0.1);

        Assert.Equal(referenced ? "quarantined" : "failed", result.Status);
        Assert.Equal(!referenced, result.CleanupComplete);
        Assert.Equal(referenced ? 0 : 1, port.Events.Count(e => e == "cleanup"));
        if (!referenced) Assert.Contains("lifecycle-timeout", result.Findings);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("spawned")]
    [InlineData("attached-or-owned")]
    [InlineData("referenced")]
    public void NetworkBecomingPublicDuringDeactivationForbidsDestruction(string changed)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.After = stage =>
        {
            if (stage != "deactivate") return;
            port.Network = changed switch
            {
                "spawned" => port.Network with { Unspawned = false },
                "attached-or-owned" => port.Network with { Detached = false },
                _ => port.Network with { ReferencesAbsent = false }
            };
        };

        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.Single(port.Events, e => e == "deactivate");
        Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("missing-disable")]
    [InlineData("double-disable")]
    [InlineData("missing-destroy")]
    [InlineData("double-destroy")]
    [InlineData("alien-final-callback")]
    public void NetworkRetirementNeedsExactOwnCallbacks(string defect)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.After = stage =>
        {
            if (stage == "deactivate" && defect.EndsWith("disable", StringComparison.Ordinal))
                port.NetworkCounters["OnDisable"] = defect == "missing-disable" ? 0 : 2;
            if (stage == "cleanup" && defect.EndsWith("destroy", StringComparison.Ordinal))
                port.NetworkCounters["OnDestroy"] = defect == "missing-destroy" ? 0 : 2;
            if (stage == "cleanup" && defect == "alien-final-callback") port.NetworkCounters["Initialize"] = 1;
        };

        var result = trial.Tick(0.2);

        Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupComplete);
        Assert.True(port.Events.Count(e => e == "cleanup") <= 1);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("register")]
    [InlineData("prepare")]
    [InlineData("activate")]
    [InlineData("deactivate")]
    [InlineData("cleanup")]
    public void UnknownPartialEffectIsNeverRetriedOrCompensated(string stage)
    {
        var port = new Port { ThrowAfter = stage };
        var trial = new BusinessActiveSubtypeTrial(requireNetwork: true);

        trial.Begin(World, port, () => true, 0);
        if (trial.Pending)
        {
            port.AdvanceFrame();
            trial.Tick(0.2);
        }

        Assert.Equal("quarantined", trial.Result.Status);
        Assert.False(trial.Result.CleanupComplete);
        Assert.Single(port.Events, e => e == stage);
        if (stage != "cleanup") Assert.DoesNotContain("cleanup", port.Events);
        port.ThrowAfter = null;
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("activate", "leave")]
    [InlineData("activate", "tick")]
    [InlineData("activate", "begin")]
    [InlineData("deactivate", "leave")]
    [InlineData("deactivate", "tick")]
    [InlineData("final-business-counters", "leave")]
    [InlineData("final-business-counters", "tick")]
    [InlineData("final-business-counters", "begin")]
    [InlineData("final-network-counters", "leave")]
    [InlineData("final-network-counters", "tick")]
    [InlineData("final-network-counters", "begin")]
    public void ReentrantRevocationStopsAllLaterPortCalls(string phase, string entry)
    {
        var port = new Port();
        var nested = new Port();
        var trial = new BusinessActiveSubtypeTrial(requireNetwork: true);
        int eventsAtRevocation = -1;
        port.After = stage =>
        {
            if (stage != phase) return;
            if (entry == "leave") trial.Leave();
            else if (entry == "tick") trial.Tick(0.3);
            else trial.Begin(World, nested, () => true, 0.3);
            eventsAtRevocation = port.Events.Count;
        };

        trial.Begin(World, port, () => true, 0);
        if (trial.Pending)
        {
            port.AdvanceFrame();
            trial.Tick(0.2);
        }

        Assert.True(eventsAtRevocation > 0);
        Assert.Equal("quarantined", trial.Result.Status);
        Assert.False(trial.Result.CleanupComplete);
        Assert.Empty(nested.Events);
        Assert.Equal(eventsAtRevocation, port.Events.Count);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("final-business-counters")]
    [InlineData("final-network-counters")]
    public void ContextLossDuringManagedFinalCountersCannotContinueToNativeSnapshot(string phase)
    {
        bool valid = true;
        var port = new Port();
        var trial = Begin(port, () => valid);
        port.AdvanceFrame();
        int eventsAtLoss = -1;
        port.After = stage =>
        {
            if (stage != phase) return;
            valid = false;
            eventsAtLoss = port.Events.Count;
        };

        var result = trial.Tick(0.2);

        Assert.Equal("quarantined", result.Status);
        Assert.True(result.CleanupAttempted);
        Assert.False(result.CleanupComplete);
        Assert.Equal(eventsAtLoss, port.Events.Count);
        AssertTerminal(trial, port);
    }

    [Theory]
    [InlineData("network-client", "waiting")]
    [InlineData("network-server", "waiting")]
    [InlineData("network-observers", "cleanup")]
    [InlineData("vtable", "waiting")]
    public void ForeignNetworkChangesArePreservedAndNeverReportedAsSuccessfulCleanup(string holder, string phase)
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        void Change()
        {
            if (holder == "vtable") port.Vtables["NetworkObject"] = "foreign-new-target";
            else port.Registries[holder] = "foreign-new-value";
        }
        if (phase == "waiting") Change();
        else port.After = stage => { if (stage == "cleanup") Change(); };

        var result = trial.Tick(0.2);

        Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupComplete);
        Assert.Equal(holder == "vtable" ? "foreign-new-target" : "foreign-new-value",
            holder == "vtable" ? port.Vtables["NetworkObject"] : port.Registries[holder]);
        if (phase == "waiting") Assert.DoesNotContain("cleanup", port.Events);
        AssertTerminal(trial, port);
    }

    [Fact]
    public void NullNetworkReadAfterActivationCannotReuseEarlierBinding()
    {
        var port = new Port();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.State = port.State with { Network = null };

        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    private static BusinessActiveSubtypeTrial Begin(Port port, Func<bool>? current = null)
    {
        var trial = new BusinessActiveSubtypeTrial(requireNetwork: true);
        Assert.Equal("waiting", trial.Begin(World, port, current ?? (() => true), 0).Status);
        return trial;
    }

    private static void AssertTerminal(BusinessActiveSubtypeTrial trial, Port port)
    {
        int count = port.Events.Count;
        var other = new Port();
        trial.Tick(6);
        trial.Leave();
        trial.Begin(World, other, () => true, 7);
        Assert.Equal(count, port.Events.Count);
        Assert.Empty(other.Events);
        Assert.False(trial.Pending);
    }

    private sealed class Port : IBusinessNetworkSubtypePort
    {
        public readonly List<string> Events = new();
        public readonly Dictionary<string, int> BusinessCounters = new(StringComparer.Ordinal);
        public readonly Dictionary<string, int> NetworkCounters = new(StringComparer.Ordinal);
        public readonly Dictionary<string, string> Registries = new(StringComparer.Ordinal);
        public readonly Dictionary<string, string> Vtables = new(StringComparer.Ordinal)
        {
            ["Business"] = "unchanged-business-vtable",
            ["NetworkObject"] = "unchanged-network-vtable"
        };
        public ActiveSubtypeState State;
        public Action<string>? After;
        public string? ThrowAfter;
        public bool Destroyed;
        public NetworkSubtypeState Network
        {
            get => State.Network!;
            set => State = State with { Network = value };
        }

        public Port()
        {
            foreach (string name in new[] { "properties", "properties-owned", "properties-unowned", "businesses",
                "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests",
                "network-client", "network-server", "network-observers" })
                Registries[name] = "unchanged:" + name;
            var network = new NetworkSubtypeState(201, 203, 201, 0, 205, 1, 102,
                true, true, true, true, true, true, NetworkCounters);
            State = new(World, 1, 100, 101, 102, 103, 104,
                false, false, true, true, true, true, true, true, BusinessCounters, network);
        }

        public void AdvanceFrame(bool network = true)
        {
            State = State with { UnityFrame = State.UnityFrame + 1 };
            BusinessCounters["Start"] = 1;
            BusinessCounters["FixedUpdate"] = BusinessCounters.GetValueOrDefault("FixedUpdate") + 1;
            if (network)
            {
                NetworkCounters["Start"] = 1;
                NetworkCounters["FixedUpdate"] = NetworkCounters.GetValueOrDefault("FixedUpdate") + 1;
            }
        }

        private void Enter(string stage) => Events.Add(stage);
        private void Exit(string stage)
        {
            After?.Invoke(stage);
            if (ThrowAfter == stage) throw new IOException("unknown-effect-after-" + stage);
        }

        public SubtypeSnapshot Capture()
        {
            Enter("capture"); Exit("capture");
            return new(Registries, Vtables);
        }
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
        public ActiveSubtypeState ReadActiveState()
        {
            Enter("read"); Exit("read");
            return State;
        }
        public void SetActive(bool active)
        {
            string stage = active ? "activate" : "deactivate";
            Enter(stage);
            State = State with { ActiveSelf = active, ActiveInHierarchy = active };
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
            Enter("cleanup");
            Destroyed = true;
            BusinessCounters["OnDestroy"] = BusinessCounters.GetValueOrDefault("OnDestroy") + 1;
            NetworkCounters["OnDestroy"] = NetworkCounters.GetValueOrDefault("OnDestroy") + 1;
            Exit("cleanup");
        }
        public bool IsClean
        {
            get { Enter("is-clean"); Exit("is-clean"); return Destroyed; }
        }
        // These two reads expose owned managed dictionaries, not further native calls.
        public IReadOnlyDictionary<string, int> ReadFinalCounters()
        {
            Enter("final-business-counters"); Exit("final-business-counters"); return BusinessCounters;
        }
        public IReadOnlyDictionary<string, int> ReadFinalNetworkCounters()
        {
            Enter("final-network-counters"); Exit("final-network-counters"); return NetworkCounters;
        }
    }

    private sealed class BusinessOnlyPort(Port inner) : IBusinessActiveSubtypePort
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
    }
}

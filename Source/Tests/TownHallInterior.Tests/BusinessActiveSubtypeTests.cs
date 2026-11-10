using TownHallInterior;
using Xunit;

public sealed class BusinessActiveSubtypeTests
{
    private static ConstructionContext Context
    {
        get
        {
            string path = Path.Combine(Path.GetTempPath(), "TownHallActiveSubtypeTests", "SaveGame_2");
            return new(true, true, true, true, path, path, "TownHallTest");
        }
    }

    [Fact]
    public void RealLaterFrameAndOwnLifecycleAreRequiredBeforeDeactivationAndDestroy()
    {
        var port = new FakePort();
        var trial = Begin(port);
        Assert.True(trial.Pending);
        Assert.True(trial.Result.ActivationAttempted);
        Assert.True(trial.Result.ActivationReturned);
        Assert.False(trial.Result.DeactivationAttempted);
        Assert.False(trial.Result.CleanupAttempted);

        port.Counters["Start"] = 1;
        port.Counters["FixedUpdate"] = 1;
        trial.Tick(0.1);
        Assert.True(trial.Pending);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);

        port.AdvanceFrame();
        var result = trial.Tick(0.2);
        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.CleanupComplete);
        Assert.True(result.DeactivationAttempted);
        Assert.True(result.CleanupAttempted);
        Assert.False(trial.Pending);
        Assert.True(port.Events.IndexOf("activate") < port.Events.IndexOf("deactivate"));
        Assert.True(port.Events.IndexOf("deactivate") < port.Events.IndexOf("cleanup"));
        Assert.Equal(1, port.Counters["OnDisable"]);
        Assert.Equal(1, port.Counters["OnDestroy"]);
    }

    [Theory]
    [InlineData("load")]
    [InlineData("singleplayer")]
    [InlineData("hall")]
    [InlineData("dependencies")]
    [InlineData("organisation")]
    [InlineData("loaded-path")]
    public void InitialInvalidContextDoesNotTouchNativePortOrConsumeTheAttempt(string field)
    {
        var port = new FakePort();
        var trial = new BusinessActiveSubtypeTrial();
        var context = field switch
        {
            "load" => Context with { LoadComplete = false },
            "singleplayer" => Context with { Singleplayer = false },
            "hall" => Context with { HallReady = false },
            "dependencies" => Context with { DependenciesMatch = false },
            "organisation" => Context with { OrganisationName = "AnotherWorld" },
            _ => Context with { LoadedSavePath = Path.Combine(Path.GetTempPath(), "Elsewhere", "SaveGame_2") }
        };
        var result = trial.Begin(context, port, () => true, 0);
        Assert.NotEqual("observed", result.Status);
        Assert.False(trial.AttemptConsumed);
        Assert.Empty(port.Events);
        trial.Begin(Context, port, () => true, 0);
        Assert.True(trial.Pending);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(-1d)]
    public void InvalidInitialTimeCannotEnterNativeMutation(double now)
    {
        var port = new FakePort();
        var trial = new BusinessActiveSubtypeTrial();
        Assert.NotEqual("observed", trial.Begin(Context, port, () => true, now).Status);
        Assert.DoesNotContain("register", port.Events);
        Assert.DoesNotContain("prepare", port.Events);
        Assert.DoesNotContain("activate", port.Events);
    }

    [Fact]
    public void ValidSameNamedTestSlotAtDifferentPathCannotReplaceTheInitialWorldBinding()
    {
        var port = new FakePort();
        string other = Path.Combine(Path.GetTempPath(), "AnotherProfile", "SaveGame_2");
        port.State = port.State with
        {
            Context = Context with { ActiveSavePath = other, LoadedSavePath = other }
        };
        var trial = new BusinessActiveSubtypeTrial();
        var result = trial.Begin(Context, port, () => true, 0);
        Assert.Equal("quarantined", result.Status);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("generation-zero")]
    [InlineData("generation-negative")]
    [InlineData("frame-negative")]
    [InlineData("root-zero")]
    [InlineData("business-zero")]
    [InlineData("class-zero")]
    [InlineData("manager-zero")]
    public void InvalidInitialIdentityCannotBeBoundAndActivated(string invalid)
    {
        var port = new FakePort();
        port.State = invalid switch
        {
            "generation-zero" => port.State with { Generation = 0 },
            "generation-negative" => port.State with { Generation = -1 },
            "frame-negative" => port.State with { UnityFrame = -1 },
            "root-zero" => port.State with { Root = 0 },
            "business-zero" => port.State with { Business = 0 },
            "class-zero" => port.State with { OwnClass = 0 },
            _ => port.State with { Manager = 0 }
        };
        var trial = new BusinessActiveSubtypeTrial();
        Assert.Equal("quarantined", trial.Begin(Context, port, () => true, 0).Status);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void DuplicateBeginWhilePendingAndAfterCompletionCannotConstructAgain()
    {
        var port = new FakePort();
        var trial = Begin(port);
        var other = new FakePort();
        trial.Begin(Context, other, () => true, 1);
        Assert.Empty(other.Events);
        Assert.Single(port.Events, e => e == "prepare");
        port.AdvanceFrame();
        Assert.Equal("observed", trial.Tick(0.2).Status);
        int count = port.Events.Count;
        trial.Begin(Context, other, () => true, 2);
        trial.Tick(3);
        trial.Leave();
        Assert.Equal(count, port.Events.Count);
        Assert.Empty(other.Events);
    }

    [Theory]
    [InlineData("Start")]
    [InlineData("FixedUpdate")]
    public void LaterFrameWithoutRequiredCallbackCannotClaimObservation(string missing)
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.Counters.Remove(missing);
        var result = trial.Tick(0.5);
        Assert.NotEqual("observed", result.Status);
        Assert.True(trial.Pending);
        Assert.False(result.CleanupAttempted);
    }

    [Fact]
    public void RegressingUnityFrameQuarantinesRatherThanFabricatingProgress()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.State = port.State with { UnityFrame = 99 };
        port.Counters["Start"] = 1;
        port.Counters["FixedUpdate"] = 1;
        var result = trial.Tick(0.2);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupAttempted);
        Assert.DoesNotContain("deactivate", port.Events);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(-0.1d)]
    public void InvalidTickTimeCannotUseGoodLifecycleCountersToAuthorizeCleanup(double now)
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        Assert.Equal("quarantined", trial.Tick(now).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void TimeoutWithFreshSafeStateCanRetireButCannotPassTheLifecycleProbe()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.State = port.State with { UnityFrame = 101 };
        var result = trial.Tick(BusinessActiveSubtypeTrial.TimeoutSeconds + 0.1);
        Assert.Equal("failed", result.Status);
        Assert.True(result.CleanupComplete);
        Assert.Single(port.Events, e => e == "deactivate");
        Assert.Single(port.Events, e => e == "cleanup");
    }

    [Fact]
    public void TimeoutCannotAuthorizeCleanupWhenReferenceAbsenceIsUnknown()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.State = port.State with { UnityFrame = 101, ReferencesAbsent = false };
        Assert.Equal("quarantined", trial.Tick(BusinessActiveSubtypeTrial.TimeoutSeconds + 0.1).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void FirstObservationArrivingAfterDeadlineIsRetiredAsTimeoutRatherThanAccepted()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        var result = trial.Tick(BusinessActiveSubtypeTrial.TimeoutSeconds + 1);
        Assert.Equal("failed", result.Status);
        Assert.True(result.CleanupComplete);
        Assert.Contains("lifecycle-timeout", result.Findings);
        Assert.Single(port.Events, e => e == "deactivate");
        Assert.Single(port.Events, e => e == "cleanup");
    }

    [Theory]
    [InlineData("frame", 2)]
    [InlineData("frame", 3)]
    [InlineData("Start", 2)]
    [InlineData("Start", 3)]
    [InlineData("FixedUpdate", 2)]
    [InlineData("FixedUpdate", 3)]
    public void FreshRetirementReadCannotRegressTheJustObservedFrameOrCallbacks(string regression, int readToRegress)
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        int reads = 0;
        port.After = name =>
        {
            if (name != "read" || ++reads != readToRegress) return;
            if (regression == "frame") port.State = port.State with { UnityFrame = 100 };
            else port.Counters[regression] = 0;
        };
        var result = trial.Tick(0.2);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupAttempted);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("final-counters")]
    [InlineData("final-snapshot")]
    [InlineData("foreign-change")]
    public void NativeAbsenceAloneCannotPublishCleanupSuccessWhenFinalEvidenceFails(string failure)
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.After = name =>
        {
            if (name != "cleanup") return;
            if (failure == "final-counters") port.ThrowBefore = "final-counters";
            else if (failure == "final-snapshot") port.ThrowBefore = "capture";
            else port.Registries["businesses"] = "changed-during-destruction";
        };
        var result = trial.Tick(0.2);
        Assert.True(port.Destroyed);
        Assert.True(result.CleanupAttempted);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupComplete);
        int count = port.Events.Count;
        port.ThrowBefore = null;
        trial.Tick(1);
        trial.Leave();
        Assert.Equal(count, port.Events.Count);
    }

    [Theory]
    [InlineData("generation")]
    [InlineData("root")]
    [InlineData("business")]
    [InlineData("class")]
    [InlineData("manager")]
    [InlineData("context")]
    [InlineData("references")]
    [InlineData("save")]
    [InlineData("components")]
    [InlineData("ownership")]
    [InlineData("virtual-targets")]
    [InlineData("unity-messages")]
    public void ChangedBindingOrSafetyEvidenceCannotReleaseTheOwnedNativeObjects(string changed)
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.State = changed switch
        {
            "generation" => port.State with { Generation = 2 },
            "root" => port.State with { Root = 201 },
            "business" => port.State with { Business = 202 },
            "class" => port.State with { OwnClass = 203 },
            "manager" => port.State with { Manager = 204 },
            "context" => port.State with { Context = Context with { LoadComplete = false } },
            "references" => port.State with { ReferencesAbsent = false },
            "save" => port.State with { SaveIdle = false },
            "components" => port.State with { ExactComponents = false },
            "ownership" => port.State with { Unowned = false },
            "virtual-targets" => port.State with { OwnVirtualTargets = false },
            _ => port.State with { OwnUnityMessages = false }
        };
        var result = trial.Tick(0.2);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupAttempted);
        Assert.False(result.DeactivationAttempted);
        int count = port.Events.Count;
        trial.Tick(1);
        trial.Leave();
        Assert.Equal(count, port.Events.Count);
    }

    [Theory]
    [InlineData("Awake")]
    [InlineData("Start")]
    [InlineData("OnEnable")]
    public void RepeatedOneShotLifecycleCallbackCannotBeAccepted(string callback)
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.Counters[callback] = 2;
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("GetSaveString")]
    [InlineData("WriteData")]
    [InlineData("InitializeSaveable")]
    [InlineData("DeleteUnapprovedFiles")]
    [InlineData("NetworkInitializeIfDisabled")]
    [InlineData("OnDisable")]
    [InlineData("OnDestroy")]
    [InlineData("UnrecognisedCallback")]
    public void UnexpectedCallbackDuringObservationForbidsFurtherNativeEffects(string callback)
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.Counters[callback] = 1;
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("deactivate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void CounterRegressionCannotEraseAnEarlierAwakeObservation()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.Counters["Awake"] = 0;
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void SnapshotsKeepOriginalCounterAndForeignRegistryEvidence()
    {
        var port = new FakePort();
        var trial = Begin(port);
        var entered = trial.Result;
        var before = entered.Before!;
        var activated = entered.Activated!;
        Assert.Equal(0, before.Counters.GetValueOrDefault("Awake"));
        Assert.Equal(1, activated.Counters["Awake"]);
        port.AdvanceFrame();
        port.Registries["businesses"] = "foreign-changed";
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.Equal(0, before.Counters.GetValueOrDefault("Start"));
        Assert.Equal(0, activated.Counters.GetValueOrDefault("Start"));
        Assert.Equal("foreign:businesses", entered.BeforeSnapshot!.Registries["businesses"]);
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, int>)activated.Counters)["Awake"] = 50);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void ForeignVtableChangeCannotBeHiddenByUnchangedRegistries()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.Vtables["Business"] = "changed-native-pointer";
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("register")]
    [InlineData("prepare")]
    [InlineData("activate")]
    [InlineData("deactivate")]
    [InlineData("cleanup")]
    public void NativeEffectFollowedByExceptionIsNeverBlindlyRepeated(string phase)
    {
        var port = new FakePort { ThrowAfter = phase };
        var trial = new BusinessActiveSubtypeTrial();
        trial.Begin(Context, port, () => true, 0);
        if (trial.Pending)
        {
            port.AdvanceFrame();
            trial.Tick(0.2);
        }
        Assert.Equal("quarantined", trial.Result.Status);
        Assert.False(trial.Result.CleanupComplete);
        Assert.Single(port.Events, e => e == phase);
        if (phase is "register" or "prepare" or "activate" or "deactivate")
            Assert.DoesNotContain("cleanup", port.Events);
        int count = port.Events.Count;
        port.ThrowAfter = null;
        trial.Tick(2);
        trial.Leave();
        trial.Begin(Context, new FakePort(), () => true, 3);
        Assert.Equal(count, port.Events.Count);
    }

    [Theory]
    [InlineData("register")]
    [InlineData("prepare")]
    [InlineData("activate")]
    [InlineData("deactivate")]
    public void ContextRevokedInsideAnEffectPreventsTheNextMutation(string phase)
    {
        bool valid = true;
        var port = new FakePort();
        port.After = name => { if (name == phase) valid = false; };
        var trial = new BusinessActiveSubtypeTrial();
        trial.Begin(Context, port, () => valid, 0);
        if (trial.Pending)
        {
            port.AdvanceFrame();
            trial.Tick(0.2);
        }
        Assert.Equal("quarantined", trial.Result.Status);
        Assert.DoesNotContain("cleanup", port.Events);
        if (phase == "register") Assert.DoesNotContain("prepare", port.Events);
        if (phase == "prepare") Assert.DoesNotContain("activate", port.Events);
        if (phase == "activate") Assert.DoesNotContain("deactivate", port.Events);
    }

    [Theory]
    [InlineData("activate")]
    [InlineData("deactivate")]
    public void ReentrantLeaveCannotDestroyWhileNativeMutationIsOnTheStack(string phase)
    {
        var port = new FakePort();
        var trial = new BusinessActiveSubtypeTrial();
        port.After = name =>
        {
            if (name != phase) return;
            trial.Leave();
            Assert.DoesNotContain("cleanup", port.Events);
        };
        trial.Begin(Context, port, () => true, 0);
        if (trial.Pending)
        {
            port.AdvanceFrame();
            trial.Tick(0.2);
        }
        Assert.Equal("quarantined", trial.Result.Status);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("capture")]
    [InlineData("inspect")]
    [InlineData("invoke")]
    [InlineData("read")]
    public void ContextRevocationDuringReadOrDispatchCannotLeakIntoActivation(string stage)
    {
        bool valid = true;
        var port = new FakePort();
        port.After = name => { if (name == stage) valid = false; };
        var trial = new BusinessActiveSubtypeTrial();
        Assert.NotEqual("observed", trial.Begin(Context, port, () => valid, 0).Status);
        Assert.DoesNotContain("activate", port.Events);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("begin")]
    [InlineData("tick")]
    public void ReentrantEntryDuringActivationCannotStartOrCompleteAnotherNativeOperation(string entry)
    {
        var port = new FakePort();
        var nested = new FakePort();
        var trial = new BusinessActiveSubtypeTrial();
        port.After = name =>
        {
            if (name != "activate") return;
            if (entry == "begin") trial.Begin(Context, nested, () => true, 0);
            else trial.Tick(0.1);
            Assert.Empty(nested.Events);
            Assert.DoesNotContain("deactivate", port.Events);
            Assert.DoesNotContain("cleanup", port.Events);
        };
        Assert.Equal("quarantined", trial.Begin(Context, port, () => true, 0).Status);
        Assert.Single(port.Events, e => e == "activate");
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Fact]
    public void LeavingPendingAttemptRetainsHandlesAndConsumesItPermanently()
    {
        var port = new FakePort();
        var trial = Begin(port);
        int count = port.Events.Count;
        Assert.Equal("quarantined", trial.Leave().Status);
        Assert.Equal(count, port.Events.Count);
        Assert.False(trial.Pending);
        Assert.True(trial.AttemptConsumed);
        port.AdvanceFrame();
        trial.Tick(1);
        trial.Begin(Context, new FakePort(), () => true, 2);
        Assert.Equal(count, port.Events.Count);
        Assert.False(port.Destroyed);
    }

    [Fact]
    public void UnreadableFreshStateDoesNotReuseEarlierSafeReadback()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.AdvanceFrame();
        port.ThrowBefore = "read";
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("cleanup", port.Events);
        Assert.DoesNotContain("deactivate", port.Events);
    }

    [Fact]
    public void ReferencesAppearingDuringDeactivationForbidDestroy()
    {
        var port = new FakePort();
        var trial = Begin(port);
        port.After = name => { if (name == "deactivate") port.State = port.State with { ReferencesAbsent = false }; };
        port.AdvanceFrame();
        Assert.Equal("quarantined", trial.Tick(0.2).Status);
        Assert.DoesNotContain("cleanup", port.Events);
    }

    [Theory]
    [InlineData("missing-disable")]
    [InlineData("double-disable")]
    [InlineData("missing-destroy")]
    [InlineData("double-destroy")]
    [InlineData("false-clean")]
    public void RetirementRequiresExactLifecycleAndNativeAbsenceReadbacks(string defect)
    {
        var port = new FakePort { ReportUnclean = defect == "false-clean" };
        var trial = Begin(port);
        port.After = name =>
        {
            if (name == "deactivate" && defect.EndsWith("disable", StringComparison.Ordinal))
                port.Counters["OnDisable"] = defect == "missing-disable" ? 0 : 2;
            if (name == "cleanup" && defect.EndsWith("destroy", StringComparison.Ordinal))
                port.Counters["OnDestroy"] = defect == "missing-destroy" ? 0 : 2;
        };
        port.AdvanceFrame();
        var result = trial.Tick(0.2);
        Assert.Equal("quarantined", result.Status);
        Assert.False(result.CleanupComplete);
        Assert.True(port.Events.Count(e => e == "cleanup") <= 1);
        int count = port.Events.Count;
        trial.Tick(1);
        trial.Leave();
        Assert.Equal(count, port.Events.Count);
    }

    private static BusinessActiveSubtypeTrial Begin(FakePort port)
    {
        var trial = new BusinessActiveSubtypeTrial();
        var result = trial.Begin(Context, port, () => true, 0);
        Assert.Equal("waiting", result.Status);
        return trial;
    }

    private sealed class FakePort : IBusinessActiveSubtypePort
    {
        public readonly List<string> Events = new();
        public readonly Dictionary<string, int> Counters = new(StringComparer.Ordinal);
        public readonly Dictionary<string, string> Registries = new(StringComparer.Ordinal);
        public readonly Dictionary<string, string> Vtables = new(StringComparer.Ordinal) { ["Business"] = "foreign-vtable" };
        public ActiveSubtypeState State;
        public Action<string>? After;
        public string? ThrowBefore, ThrowAfter;
        public bool Destroyed, ReportUnclean;

        public FakePort()
        {
            foreach (string name in new[] { "properties", "properties-owned", "properties-unowned", "businesses", "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests" })
                Registries[name] = "foreign:" + name;
            State = new(Context, 1, 100, 101, 102, 103, 104, false, false, true, true, true, true, true, true, Counters);
        }

        public void AdvanceFrame()
        {
            State = State with { UnityFrame = State.UnityFrame + 1 };
            Counters["Start"] = 1;
            Counters["FixedUpdate"] = Counters.GetValueOrDefault("FixedUpdate") + 1;
        }

        private void Enter(string name)
        {
            Events.Add(name);
            if (ThrowBefore == name) throw new IOException("unreadable-before-" + name);
        }
        private void Exit(string name)
        {
            After?.Invoke(name);
            if (ThrowAfter == name) throw new IOException("unknown-after-" + name);
        }

        public SubtypeSnapshot Capture()
        {
            Enter("capture");
            Exit("capture");
            return new(Registries, Vtables);
        }
        public void RegisterType() { Enter("register"); Exit("register"); }
        public void Prepare() { Enter("prepare"); Exit("prepare"); }
        public SubtypeReadback Inspect()
        {
            Enter("inspect"); Exit("inspect");
            return new(!State.ActiveSelf && !State.ActiveInHierarchy, State.ExactComponents, State.Unowned, true, State.OwnVirtualTargets, 0);
        }
        public SubtypeDispatch InvokeThroughNativeBase()
        {
            Enter("invoke");
            Counters["ShouldSave"] = Counters.GetValueOrDefault("ShouldSave") + 1;
            Counters["CanBePurchased"] = Counters.GetValueOrDefault("CanBePurchased") + 1;
            Exit("invoke");
            return new(false, false, Counters["ShouldSave"], Counters["CanBePurchased"]);
        }
        public ActiveSubtypeState ReadActiveState()
        {
            Enter("read"); Exit("read");
            return State with { Counters = Counters };
        }
        public void SetActive(bool active)
        {
            string name = active ? "activate" : "deactivate";
            Enter(name);
            State = State with { ActiveSelf = active, ActiveInHierarchy = active };
            if (active)
            {
                Counters["Awake"] = Counters.GetValueOrDefault("Awake") + 1;
                Counters["OnEnable"] = Counters.GetValueOrDefault("OnEnable") + 1;
            }
            else Counters["OnDisable"] = Counters.GetValueOrDefault("OnDisable") + 1;
            Exit(name);
        }
        public void Cleanup()
        {
            Enter("cleanup");
            Destroyed = true;
            Counters["OnDestroy"] = Counters.GetValueOrDefault("OnDestroy") + 1;
            Exit("cleanup");
        }
        public bool IsClean
        {
            get { Enter("is-clean"); Exit("is-clean"); return Destroyed && !ReportUnclean; }
        }
        public IReadOnlyDictionary<string, int> ReadFinalCounters()
        {
            Enter("final-counters"); Exit("final-counters"); return Counters;
        }
    }
}

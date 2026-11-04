using TownHallInterior;
using Xunit;

public sealed class BusinessConstructionTests
{
    private static string SlotPath => Path.Combine(Path.GetTempPath(), "TownHallConstructionTests", "SaveGame_2");
    private static ConstructionContext Context => new(true, true, true, true, SlotPath, SlotPath, "TownHallTest");
    private const string ValidJson = "{\"PropertyCode\":\"townhallinterior_townhall\",\"IsOwned\":false,\"LaunderingOperations\":[]}";

    [Fact]
    public void SuccessfulUnpublishedShellIsSerializedAndCleanedBeforeObservationPasses()
    {
        var port = new FakePort();
        var trial = new BusinessConstructionTrial();
        var result = trial.Run(Context, port);
        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.CleanupComplete);
        Assert.True(result.AttemptConsumed);
        Assert.Equal(ValidJson, result.SerializedJson);
        Assert.Equal(new[] { "capture1", "prepare", "inspect", "capture2", "serialize", "cleanup1", "clean", "capture3" }, port.Calls);
        Assert.Equal("skipped", trial.Run(Context, port).Status);
        Assert.Equal(1, port.Prepares);
    }

    [Fact]
    public void DeniedContextDoesNotConsumeTheOnlyAttemptOrTouchThePort()
    {
        var port = new FakePort();
        var trial = new BusinessConstructionTrial();
        var denied = trial.Run(Context with { LoadComplete = false }, port);
        Assert.Equal("skipped", denied.Status);
        Assert.False(trial.AttemptConsumed);
        Assert.Empty(port.Calls);
        Assert.Equal("observed", trial.Run(Context, port).Status);
    }

    [Theory]
    [InlineData("load")]
    [InlineData("singleplayer")]
    [InlineData("hall")]
    [InlineData("dependencies")]
    [InlineData("organisation")]
    [InlineData("slot")]
    [InlineData("different-world-path")]
    [InlineData("relative")]
    [InlineData("missing")]
    public void UnconfirmedWorldOrPrerequisitesCannotReachConstruction(string cause)
    {
        var context = cause switch
        {
            "load" => Context with { LoadComplete = false },
            "singleplayer" => Context with { Singleplayer = false },
            "hall" => Context with { HallReady = false },
            "dependencies" => Context with { DependenciesMatch = false },
            "organisation" => Context with { OrganisationName = "TownHallTest " },
            "slot" => Context with { ActiveSavePath = SlotPath + "0", LoadedSavePath = SlotPath + "0" },
            "different-world-path" => Context with { LoadedSavePath = Path.Combine(Path.GetTempPath(), "different", "SaveGame_2") },
            "relative" => Context with { ActiveSavePath = "SaveGame_2", LoadedSavePath = "SaveGame_2" },
            _ => Context with { ActiveSavePath = null }
        };
        var port = new FakePort();
        Assert.Equal("skipped", new BusinessConstructionTrial().Run(context, port).Status);
        Assert.Empty(port.Calls);
    }

    [Fact]
    public void CanonicalEquivalentPathsPermitTheConfirmedWorld()
    {
        var context = Context with
        {
            ActiveSavePath = SlotPath + Path.DirectorySeparatorChar,
            LoadedSavePath = Path.Combine(SlotPath, "..", "SaveGame_2")
        };
        Assert.Null(ConstructionGate.Check(context));
        Assert.NotNull(ConstructionGate.Check(null));
    }

    [Theory]
    [InlineData("prepare", 2)]
    [InlineData("inspect", 2)]
    [InlineData("serialize", 3)]
    public void FailureAfterMutationAlwaysCleansAndPreventsAnotherAttempt(string phase, int captures)
    {
        var port = new FakePort { ThrowAt = phase };
        var trial = new BusinessConstructionTrial();
        var result = trial.Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.Contains(result.Findings, x => x.StartsWith(phase + ":"));
        Assert.True(result.CleanupComplete);
        Assert.Equal(captures, port.Captures);
        Assert.Equal(1, port.Cleanups);
        trial.Run(Context, port);
        Assert.Equal(1, port.Prepares);
    }

    [Fact]
    public void BeforeReadFailureCreatesNothingAndIsTerminal()
    {
        var port = new FakePort { ThrowCapture = 1 };
        var trial = new BusinessConstructionTrial();
        var result = trial.Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.Equal(0, port.Prepares);
        Assert.Equal(0, port.Cleanups);
        Assert.Null(result.Before);
        Assert.Contains("before-registries:IOException", result.Findings);
        Assert.Equal("skipped", trial.Run(Context, port).Status);
        Assert.Equal(2, port.Captures);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void UnreadableDuringOrAfterRegistryCannotPass(int capture)
    {
        var port = new FakePort { ThrowCapture = capture };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.True(result.CleanupComplete);
        if (capture == 2) Assert.DoesNotContain("serialize", port.Calls);
    }

    [Theory]
    [InlineData("active")]
    [InlineData("references")]
    [InlineData("owned")]
    [InlineData("foreign")]
    [InlineData("code")]
    public void FailedReadbackPreventsNativeSerialization(string cause)
    {
        var port = new FakePort();
        port.Readback = cause switch
        {
            "active" => port.Readback with { Inactive = false },
            "references" => port.Readback with { ReferencesBound = false },
            "owned" => port.Readback with { Unowned = false },
            "foreign" => port.Readback with { NoForeignComponents = false },
            _ => port.Readback with { PropertyCode = "laundromat" }
        };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.DoesNotContain("serialize", port.Calls);
        Assert.True(result.CleanupComplete);
    }

    [Fact]
    public void RegistryMutationCannotBeHiddenByReturningTheSameMutableDictionary()
    {
        var port = new FakePort();
        port.OnPrepare = () => port.Registry["businesses"] = "foreign,own";
        port.OnCleanup = () => port.Registry["businesses"] = "foreign";
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.Contains("registries-changed-during-construction", result.Findings);
        Assert.DoesNotContain("serialize", port.Calls);
        Assert.Equal("foreign", result.Before!["businesses"]);
        Assert.Equal("foreign,own", result.During!["businesses"]);
        Assert.Equal("foreign", result.After!["businesses"]);
        port.Registry.Clear();
        Assert.Equal("foreign", result.Before["businesses"]);
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, string>)result.Before).Clear());
    }

    [Fact]
    public void SameCountWithDifferentRegistryKeyIsStillAChange()
    {
        var port = new FakePort();
        port.OnPrepare = () => { port.Registry.Clear(); port.Registry.Add("other", "foreign"); };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.Contains("registries-changed-during-construction", result.Findings);
        Assert.Contains("registries-not-confirmed-after-cleanup", result.Findings);
    }

    [Fact]
    public void CleanupSideEffectsAreDetectedAfterOtherwiseValidSerialization()
    {
        var port = new FakePort();
        port.OnCleanup = () => port.Registry["businesses"] = "changed";
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.Contains("registries-not-confirmed-after-cleanup", result.Findings);
        Assert.Equal(ValidJson, result.SerializedJson);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]
    public void CleanupRetriesAreBoundedAndRecoveredFailuresStayVisible(int failures, bool clean)
    {
        var port = new FakePort { CleanupFailures = failures };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.Equal(2, port.Cleanups);
        Assert.Equal(clean, result.CleanupComplete);
        Assert.Contains("cleanup:IOException", result.Findings);
    }

    [Fact]
    public void CleanupReadbackMustConfirmRemoval()
    {
        var port = new FakePort { LeaveAlive = true };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.False(result.CleanupComplete);
        Assert.Equal(2, port.Cleanups);
    }

    [Fact]
    public void UnreadableCleanupStateCannotEscapeTrialOrShutdownCleanup()
    {
        var port = new FakePort { ThrowCleanRead = true };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.False(result.CleanupComplete);
        Assert.Equal(2, port.Cleanups);
        Assert.Contains("cleanup:InvalidOperationException", result.Findings);
        Assert.False(BusinessConstructionTrial.CleanupOwned(port));
        Assert.Equal(4, port.Cleanups);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void LateLifecycleSignalDuringSerializationOrCleanupCannotPass(bool duringCleanup)
    {
        var port = new FakePort();
        port.Registry["staged-lifecycle"] = "uninvoked";
        Action signal = () => port.Registry["staged-lifecycle"] = "unexpected-call-blocked";
        if (duringCleanup) port.OnCleanup = signal; else port.OnSerialize = signal;
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.True(result.CleanupComplete);
        Assert.Contains("registries-not-confirmed-after-cleanup", result.Findings);
    }

    [Theory]
    [InlineData("")]
    [InlineData("{")]
    [InlineData("[]")]
    [InlineData("{\"PropertyCode\":\"wrong\",\"IsOwned\":false,\"LaunderingOperations\":[]}")]
    [InlineData("{\"PropertyCode\":\"townhallinterior_townhall\",\"IsOwned\":true,\"LaunderingOperations\":[]}")]
    [InlineData("{\"PropertyCode\":\"townhallinterior_townhall\",\"IsOwned\":false,\"LaunderingOperations\":[{}]}")]
    [InlineData("{\"PropertyCode\":\"townhallinterior_townhall\",\"IsOwned\":false,\"LaunderingOperations\":null}")]
    [InlineData("{\"PropertyCode\":\"wrong\",\"PropertyCode\":\"townhallinterior_townhall\",\"IsOwned\":false,\"LaunderingOperations\":[]}")]
    public void InvalidNativeSerializationIsReportedWithoutPreventingCleanup(string json)
    {
        var port = new FakePort { Json = json };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Equal("failed", result.Status);
        Assert.True(result.CleanupComplete);
    }

    [Fact]
    public void OversizedNativeJsonIsNotRetainedInTheReport()
    {
        var port = new FakePort { Json = new string('x', BusinessConstructionTrial.MaximumJsonLength + 1) };
        var result = new BusinessConstructionTrial().Run(Context, port);
        Assert.Contains("serialized-json-size", result.Findings);
        Assert.Null(result.SerializedJson);
        Assert.True(result.CleanupComplete);
    }

    [Fact]
    public void ReentrantCaptureCannotStartAnotherConstruction()
    {
        var port = new FakePort();
        var nested = new FakePort();
        var trial = new BusinessConstructionTrial();
        port.OnCapture = () => Assert.Equal("skipped", trial.Run(Context, nested).Status);
        Assert.Equal("observed", trial.Run(Context, port).Status);
        Assert.Empty(nested.Calls);
    }

    private sealed class FakePort : IBusinessConstructionPort
    {
        public readonly List<string> Calls = new();
        public readonly Dictionary<string, string> Registry = new() { ["businesses"] = "foreign" };
        public ConstructionReadback Readback = new(true, true, true, true, BusinessConstructionTrial.PropertyCode);
        public string Json = ValidJson;
        public string? ThrowAt;
        public int ThrowCapture, CleanupFailures, Captures, Prepares, Cleanups;
        public bool LeaveAlive, ThrowCleanRead;
        public Action? OnPrepare, OnCleanup, OnCapture, OnSerialize;
        private bool _alive;
        public IReadOnlyDictionary<string, string> CaptureRegistries()
        {
            Calls.Add("capture" + ++Captures);
            OnCapture?.Invoke();
            if (Captures == ThrowCapture) throw new IOException("private path deliberately not in result");
            return Registry;
        }
        public void Prepare()
        {
            Calls.Add("prepare"); Prepares++; _alive = true;
            OnPrepare?.Invoke();
            if (ThrowAt == "prepare") throw new IOException();
        }
        public ConstructionReadback Inspect()
        {
            Calls.Add("inspect");
            if (ThrowAt == "inspect") throw new IOException();
            return Readback;
        }
        public string Serialize()
        {
            Calls.Add("serialize");
            OnSerialize?.Invoke();
            if (ThrowAt == "serialize") throw new IOException();
            return Json;
        }
        public void Cleanup()
        {
            Calls.Add("cleanup" + ++Cleanups);
            if (Cleanups <= CleanupFailures) throw new IOException();
            OnCleanup?.Invoke();
            if (!LeaveAlive) _alive = false;
        }
        public bool IsClean { get { Calls.Add("clean"); if (ThrowCleanRead) throw new InvalidOperationException(); return !_alive; } }
    }
}

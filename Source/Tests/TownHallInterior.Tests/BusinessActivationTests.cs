using TownHallInterior;
using Xunit;

public sealed class BusinessActivationTests
{
    private static ConstructionContext Context
    {
        get
        {
            string path = Path.Combine(Path.GetTempPath(), "TownHallActivationTests", "SaveGame_2");
            return new(true, true, true, true, path, path, "TownHallTest");
        }
    }

    private static Dictionary<string, int> Expected() => new(StringComparer.Ordinal)
    {
        ["properties"] = 1,
        ["properties-owned"] = 0,
        ["properties-unowned"] = 1,
        ["businesses"] = 1,
        ["businesses-owned"] = 0,
        ["businesses-unowned"] = 1,
        ["saveables"] = 0,
        ["base-saveables"] = 0,
        ["completed-saveables"] = 0,
        ["save-requests"] = 0
    };

    [Fact]
    public void LeaseStaysEnteredUntilFollowingFrameAndActiveUntilExplicitLeave()
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(7, Expected());
        Assert.Equal("entered", lease.Begin(Context, port, 100).Status);
        Assert.False(lease.CanStartNextGeneration);
        Assert.Equal(3, port.Captures);
        Assert.Equal("entered", lease.Complete(100).Status);
        Assert.Equal("entered", lease.Complete(99).Status);
        Assert.Equal(3, port.Captures);
        var complete = lease.Complete(101);
        Assert.Equal("active", complete.Status);
        Assert.Equal(7, complete.Generation);
        Assert.Empty(complete.Findings);
        Assert.True(port.Alive);
        Assert.Equal(0, port.Cleanups);
        Assert.Equal("active", lease.Complete(102).Status);
        Assert.Equal(4, port.Captures);
        var closed = lease.Leave();
        Assert.Equal("closed", closed.Status);
        Assert.True(closed.CleanupComplete);
        Assert.True(lease.CanStartNextGeneration);
        Assert.False(port.Alive);
        Assert.Equal(1, port.Cleanups);
    }

    [Fact]
    public void DeniedLoadContextDoesNotTouchPortOrConsumeAttempt()
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        var denied = lease.Begin(Context with { LoadComplete = false }, port, 1);
        Assert.Equal("skipped", denied.Status);
        Assert.Contains("load-incomplete", denied.Findings);
        Assert.Empty(port.Calls);
        Assert.Equal("entered", lease.Begin(Context, port, 2).Status);
    }

    [Fact]
    public void NegativeFrameCannotCreateNativeState()
    {
        var port = new FakePort();
        Assert.Equal("skipped", new BusinessActivationLease(1, Expected()).Begin(Context, port, -1).Status);
        Assert.Empty(port.Calls);
    }

    [Theory]
    [InlineData("prepare")]
    [InlineData("activate")]
    [InlineData("register")]
    public void PartialFailureCleansOwnResourcesAndNeverRetriesSameGeneration(string phase)
    {
        var port = new FakePort { ThrowAt = phase };
        var lease = new BusinessActivationLease(1, Expected());
        var result = lease.Begin(Context, port, 10);
        Assert.Equal("failed", result.Status);
        Assert.Contains(result.Findings, x => x.StartsWith(phase + ":"));
        Assert.Equal(1, port.Cleanups);
        Assert.False(port.Alive);
        Assert.True(result.CleanupComplete);
        Assert.True(lease.CanStartNextGeneration);
        int count = port.Calls.Count;
        Assert.Equal("skipped", lease.Begin(Context, port, 20).Status);
        Assert.Equal(count, port.Calls.Count);
    }

    [Fact]
    public void NativeReadFailureBeforePrepareDoesNotCreateOrCleanAnUnknownRoot()
    {
        var port = new FakePort { ThrowCapture = 1 };
        var lease = new BusinessActivationLease(1, Expected());
        Assert.Equal("failed", lease.Begin(Context, port, 1).Status);
        Assert.Equal(0, port.Prepares);
        Assert.Equal(0, port.Cleanups);
        Assert.False(lease.CanStartNextGeneration);
        Assert.Equal("skipped", lease.Begin(Context, port, 2).Status);
    }

    [Fact]
    public void PreexistingOwnEntriesAreNotSilentlyAdoptedOrDeleted()
    {
        var port = new FakePort();
        port.Owned["businesses"] = 1;
        var lease = new BusinessActivationLease(1, Expected());
        Assert.Equal("failed", lease.Begin(Context, port, 1).Status);
        Assert.Equal(0, port.Prepares);
        Assert.Equal(0, port.Cleanups);
        Assert.Equal(1, port.Owned["businesses"]);
        Assert.False(lease.CanStartNextGeneration);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void NullSnapshotCannotProduceEnteredOrActiveState(int capture)
    {
        var port = new FakePort { NullCapture = capture };
        var lease = new BusinessActivationLease(1, Expected());
        Assert.Equal("failed", lease.Begin(Context, port, 1).Status);
        Assert.Equal(capture > 1 ? 1 : 0, port.Cleanups);
    }

    [Fact]
    public void InactivePreparationMustNotPublishRegistryEntries()
    {
        var port = new FakePort();
        port.OnPrepare = () => port.Owned["properties"] = 1;
        var result = new BusinessActivationLease(1, Expected()).Begin(Context, port, 1);
        Assert.Equal("failed", result.Status);
        Assert.DoesNotContain("activate", port.Calls);
        Assert.DoesNotContain("register", port.Calls);
        Assert.Equal(1, port.Cleanups);
    }

    [Theory]
    [InlineData("root-active")]
    [InlineData("references")]
    [InlineData("owned")]
    public void IncompleteStagingNeverActivates(string cause)
    {
        var port = new FakePort();
        port.OnPrepare = () =>
        {
            if (cause == "root-active") port.Active = true;
            if (cause == "references") port.References = false;
            if (cause == "owned") port.Unowned = false;
        };
        Assert.Equal("failed", new BusinessActivationLease(1, Expected()).Begin(Context, port, 1).Status);
        Assert.DoesNotContain("activate", port.Calls);
        Assert.Equal(1, port.Cleanups);
    }

    [Fact]
    public void ChangedForeignRegistryDuringBeginFailsEvenIfCleanupPreservesItsNewState()
    {
        var port = new FakePort();
        port.OnActivate = () => port.Foreign["businesses"] = "foreign-changed";
        var lease = new BusinessActivationLease(1, Expected());
        var result = lease.Begin(Context, port, 1);
        Assert.Equal("failed", result.Status);
        Assert.Equal("foreign-changed", port.Foreign["businesses"]);
        Assert.False(lease.CanStartNextGeneration);
        Assert.False(result.CleanupComplete);
        Assert.False(port.Alive);
    }

    [Theory]
    [InlineData("properties", 0)]
    [InlineData("properties", 2)]
    [InlineData("businesses", 0)]
    [InlineData("businesses", 2)]
    [InlineData("properties-owned", 1)]
    [InlineData("businesses-owned", 1)]
    [InlineData("saveables", 1)]
    [InlineData("base-saveables", 1)]
    [InlineData("completed-saveables", 1)]
    [InlineData("save-requests", 1)]
    public void BeginRejectsMissingDuplicateOrForbiddenRegistrationImmediately(string registry, int count)
    {
        var port = new FakePort();
        port.OnRegister = () => port.Owned[registry] = count;
        var lease = new BusinessActivationLease(1, Expected());
        var result = lease.Begin(Context, port, 10);
        Assert.Equal("failed", result.Status);
        Assert.Contains("entered-readback:InvalidOperationException", result.Findings);
        Assert.Equal(count, result.Current!.OwnedCounts[registry]);
        Assert.Equal(1, port.Cleanups);
        Assert.False(port.Alive);
        Assert.True(result.CleanupComplete);
        int calls = port.Calls.Count;
        Assert.Equal("skipped", lease.Complete(11).Status);
        Assert.Equal(calls, port.Calls.Count);
    }

    [Theory]
    [InlineData("businesses", 0)]
    [InlineData("businesses", 2)]
    [InlineData("businesses-owned", 1)]
    [InlineData("saveables", 1)]
    [InlineData("save-requests", 1)]
    public void CompleteRequiresExactOwnedCountsIncludingForbiddenRegistrations(string registry, int count)
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 10);
        port.Owned[registry] = count;
        var result = lease.Complete(11);
        Assert.Equal("failed", result.Status);
        Assert.Equal(count, result.Current!.OwnedCounts[registry]);
        Assert.Equal(1, port.Cleanups);
        Assert.False(port.Alive);
    }

    [Fact]
    public void CompleteCannotHideForeignMutationOrMissingRegistryScope()
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 1);
        port.Foreign.Remove("saveables");
        Assert.Equal("failed", lease.Complete(2).Status);
        Assert.Equal(1, port.Cleanups);
        Assert.False(lease.CanStartNextGeneration);
    }

    [Fact]
    public void SnapshotAndExpectedCountAliasingCannotRewriteEarlierEvidence()
    {
        var expected = Expected();
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, expected);
        expected["saveables"] = 1;
        var entered = lease.Begin(Context, port, 1);
        Assert.Equal(0, entered.Before!.OwnedCounts["businesses"]);
        Assert.Equal(1, entered.Current!.OwnedCounts["businesses"]);
        port.Owned["saveables"] = 1;
        var completed = lease.Complete(2);
        Assert.Equal("failed", completed.Status);
        Assert.Equal(0, entered.Current.OwnedCounts["saveables"]);
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, int>)entered.Current.OwnedCounts).Clear());
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, string>)entered.Before.ForeignFingerprints).Clear());
    }

    [Fact]
    public void LeavePreservesLegitimateForeignChangesSinceActivation()
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 1); lease.Complete(2);
        port.Foreign["businesses"] = "legitimate-later-registration";
        var result = lease.Leave();
        Assert.Equal("closed", result.Status);
        Assert.True(result.CleanupComplete);
        Assert.Equal("legitimate-later-registration", result.After!.ForeignFingerprints["businesses"]);
        Assert.Equal("foreign:businesses", result.Before!.ForeignFingerprints["businesses"]);
        Assert.True(lease.CanStartNextGeneration);
    }

    [Fact]
    public void CleanupMustNotChangeAnyForeignRegistration()
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 1); lease.Complete(2);
        port.OnCleanup = () => port.Foreign["businesses"] = "deleted-foreign";
        var result = lease.Leave();
        Assert.Equal("failed", result.Status);
        Assert.Contains("cleanup-state-unconfirmed", result.Findings);
        Assert.False(lease.CanStartNextGeneration);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public void UnreadableCleanupBoundaryStillCleansOwnRootButPermanentlyBlocksHandoff(int capture)
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 1); lease.Complete(2);
        port.ThrowCapture = capture;
        Assert.Equal("failed", lease.Leave().Status);
        Assert.Equal(1, port.Cleanups);
        Assert.False(port.Alive);
        Assert.False(lease.CanStartNextGeneration);
        port.ThrowCapture = 0;
        Assert.Equal("failed", lease.Leave().Status);
        Assert.Equal(1, port.Cleanups);
        Assert.False(lease.CanStartNextGeneration);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]
    public void CleanupRetriesAtMostTwiceAndPreservesRecoveredErrorEvidence(int failures, bool handoff)
    {
        var port = new FakePort { CleanupFailures = failures };
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 1); lease.Complete(2);
        var result = lease.Leave();
        Assert.Equal("failed", result.Status);
        Assert.Contains("cleanup:IOException", result.Findings);
        Assert.Equal(2, port.Cleanups);
        Assert.Equal(handoff, lease.CanStartNextGeneration);
        lease.Leave();
        Assert.Equal(2, port.Cleanups);
    }

    [Fact]
    public void FalseCleanupReadbackIsTerminalEvenAfterThePortLaterLooksClean()
    {
        var port = new FakePort { LieUnclean = true };
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 1); lease.Complete(2);
        Assert.Equal("failed", lease.Leave().Status);
        Assert.Equal(2, port.Cleanups);
        Assert.False(lease.CanStartNextGeneration);
        port.LieUnclean = false;
        lease.Leave();
        Assert.False(lease.CanStartNextGeneration);
    }

    [Fact]
    public void ReentrantBeginCannotConstructTwiceAndForcesOuterCleanup()
    {
        var port = new FakePort();
        var nested = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        port.OnPrepare = () => Assert.Equal("skipped", lease.Begin(Context, nested, 1).Status);
        Assert.Equal("failed", lease.Begin(Context, port, 1).Status);
        Assert.Empty(nested.Calls);
        Assert.Equal(1, port.Prepares);
        Assert.Equal(1, port.Cleanups);
    }

    [Fact]
    public void ReentrantLeaveNeverDisposesInsideNativeActivateAndQuarantinesTheAttempt()
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        port.OnActivate = () =>
        {
            Assert.Equal("skipped", lease.Leave().Status);
            Assert.Equal(0, port.Cleanups);
        };
        Assert.Equal("failed", lease.Begin(Context, port, 1).Status);
        Assert.Equal(1, port.Cleanups);
        Assert.False(lease.CanStartNextGeneration);
    }

    [Fact]
    public void OldClosedLeaseCannotRemoveNewGenerationAndFailAfterCloseDoesNotMutate()
    {
        var port = new FakePort();
        var old = new BusinessActivationLease(1, Expected());
        old.Begin(Context, port, 1); old.Complete(2); old.Leave();
        Assert.True(old.CanStartNextGeneration);
        var next = new BusinessActivationLease(2, Expected());
        Assert.Equal("entered", next.Begin(Context, port, 3).Status);
        Assert.Equal("active", next.Complete(4).Status);
        int calls = port.Calls.Count;
        old.Leave();
        Assert.Equal("failed", old.Fail("late-old-callback").Status);
        old.Leave();
        Assert.Equal(calls, port.Calls.Count);
        Assert.True(port.Alive);
        Assert.True(port.Active);
        Assert.Equal(1, port.Owned["businesses"]);
    }

    [Fact]
    public void RuntimeTimeoutFailsAndCleansAnEnteredLease()
    {
        var port = new FakePort();
        var lease = new BusinessActivationLease(1, Expected());
        lease.Begin(Context, port, 1);
        Assert.Equal("failed", lease.Fail("start-timeout").Status);
        Assert.False(port.Alive);
        Assert.Equal(1, port.Cleanups);
        Assert.Contains("start-timeout", lease.Leave().Findings);
    }

    private sealed class FakePort : IBusinessActivationPort
    {
        public readonly List<string> Calls = new();
        public readonly Dictionary<string, string> Foreign = Expected().ToDictionary(p => p.Key, p => "foreign:" + p.Key, StringComparer.Ordinal);
        public readonly Dictionary<string, int> Owned = Expected().ToDictionary(p => p.Key, _ => 0, StringComparer.Ordinal);
        public bool Alive, Active, References, Unowned = true, LieUnclean;
        public int Captures, Prepares, Cleanups, ThrowCapture, NullCapture, CleanupFailures;
        public string? ThrowAt;
        public Action? OnPrepare, OnActivate, OnRegister, OnCleanup;
        public RegistrationSnapshot Capture()
        {
            Calls.Add("capture" + ++Captures);
            if (Captures == ThrowCapture) throw new IOException();
            if (Captures == NullCapture) return null!;
            return new(Foreign, Owned, Alive, Active, References, Unowned);
        }
        public void Prepare()
        {
            Calls.Add("prepare"); Prepares++; Alive = true; Active = false; References = true; Unowned = true;
            OnPrepare?.Invoke();
            if (ThrowAt == "prepare") throw new IOException();
        }
        public void Activate()
        {
            Calls.Add("activate"); Active = true; OnActivate?.Invoke();
            if (ThrowAt == "activate") throw new IOException();
        }
        public void Register()
        {
            Calls.Add("register");
            foreach (var pair in Expected()) Owned[pair.Key] = pair.Value;
            OnRegister?.Invoke();
            if (ThrowAt == "register") throw new IOException();
        }
        public void Cleanup()
        {
            Calls.Add("cleanup" + ++Cleanups);
            if (Cleanups <= CleanupFailures) throw new IOException();
            Alive = false; Active = false;
            foreach (string key in Owned.Keys.ToArray()) Owned[key] = 0;
            OnCleanup?.Invoke();
        }
        public bool IsClean => !Alive && !LieUnclean;
    }
}

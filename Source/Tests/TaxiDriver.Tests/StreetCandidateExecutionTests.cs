using System.Reflection;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes resolver/graph adapter, fallback and direct dispatch source with substituted native APIs. No real nearest node or full route is measured.</summary>
public class StreetCandidateExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(Compile);

    [Fact]
    public void ExactStreetGraphIsMaskedAndItsReturnedNodeIsIndependentlyChecked()
    {
        Outcome result = Run("resolve", "exact");
        Assert.True(result.Success);
        Assert.Equal("10,0,0", result.Point);
        Assert.Equal("roadside candidate", result.Rule);
        Assert.Contains("not verified", result.Reason);
        Assert.Equal("2/True/True/False", result.Constraint);
        Assert.Equal(1, result.StreetQueries);
        Assert.Equal(0, result.GeneralQueries);
    }

    [Theory]
    [InlineData("misleading")]
    [InlineData("duplicate")]
    [InlineData("case-only")]
    [InlineData("missing-active")]
    [InlineData("missing-graphs")]
    [InlineData("null-node")]
    [InlineData("wrong-graph")]
    [InlineData("unwalkable")]
    [InlineData("goal-nan")]
    [InlineData("point-nan")]
    [InlineData("point-infinity")]
    [InlineData("distance-overflow")]
    [InlineData("too-far")]
    [InlineData("vertical-too-far")]
    [InlineData("unreadable-graph")]
    [InlineData("nearest-throws")]
    public void InvalidOrUnprovableStreetSelectionFailsClosed(string boundary)
    {
        Outcome result = Run("resolve", boundary);
        Assert.False(result.Success);
        Assert.False(result.ArrivalOk);
        Assert.Equal(0, result.GeneralQueries);
    }

    [Theory]
    [InlineData("boundary", "60,0,0")]
    [InlineData("vertical-boundary", "0,60,0")]
    public void ExistingSixtyMeterBudgetIsInclusiveAndThreeDimensional(string boundary, string point)
    {
        Outcome result = Run("resolve", boundary);
        Assert.True(result.Success);
        Assert.Equal(point, result.Point);
        Assert.Equal(60f, result.Distance);
    }

    [Theory]
    [InlineData("barn", "154,0,18.6")]
    [InlineData("bungalow", "-153,-4,109.3")]
    [InlineData("docks", "-96,-2,-52")]
    public void SyntheticHistoricalShapesSelectTheStreetCandidateNotTheGeneralProjection(string fixture, string expected)
    {
        Outcome result = Run("select", fixture);
        Assert.True(result.Success);
        Assert.Equal(expected, result.Point);
        Assert.Equal(expected, result.Target);
        Assert.Equal(expected, result.Destination);
        Assert.Equal(expected, result.ActivePoint);
        Assert.Equal(0, result.GeneralQueries); // no second General Vehicle Graph projection in Go
        Assert.Equal(1, result.StreetQueries);
        Assert.Equal(1, result.NavigateCalls);
        Assert.Equal("True/True/False", result.Settings);
    }

    [Fact]
    public void NativeFailedCalculationStillQueuesRecoveryAndNeverPublishesArrival()
    {
        Outcome result = Run("select-failed", "exact");
        Assert.Equal("10,0,0", result.Target);
        Assert.True(result.RecoveryPending);
        Assert.False(result.Arrived);
        Assert.False(result.Terminal);
        Assert.Equal("Failed", result.Callback);
    }

    [Fact]
    public void PickupGoRetainsItsExistingGeneralProjectionRatherThanOptingIntoStreetPreservation()
    {
        Outcome result = Run("pickup", "exact");
        Assert.True(result.Success);
        Assert.Equal("1,0,0", result.Target);
        Assert.Equal(1, result.GeneralQueries);
        Assert.Equal(0, result.StreetQueries);
        Assert.Equal("-200,0,0", result.Destination);
        Assert.Equal("Old", result.Name);
    }

    [Theory]
    [InlineData("normal")]
    [InlineData("direct-override")]
    [InlineData("stand")]
    [InlineData("legacy")]
    public void NoDestinationEntryPointCanBypassStrictSelectionOrRelabelTheOldTrip(string entry)
    {
        Outcome result = Run("invalid-" + entry, "misleading");
        Assert.False(result.Success);
        Assert.Equal("Old", result.Name);
        Assert.Equal("-200,0,0", result.Target);
        Assert.Equal("-200,0,0", result.Destination);
        Assert.Equal("-200,0,0", result.ActivePoint);
        Assert.Equal(0, result.NavigateCalls);
        Assert.Equal(0, result.FareStarts);
        Assert.Equal(0, result.Operations);
    }

    [Theory]
    [InlineData("stop")]
    [InlineData("reroute")]
    [InlineData("teardown")]
    public void SelectionDoesNotPublishFromASupersededNativeLookup(string interruption)
    {
        Outcome result = Run("interrupt", interruption);
        Assert.False(result.Success);
        Assert.Equal(interruption == "reroute" ? "New" : "Old", result.Name);
        Assert.Equal(interruption == "reroute" ? "999,0,0" : "-200,0,0", result.Target);
        Assert.Equal(0, result.NavigateCalls);
    }

    [Fact]
    public void DuplicateAndSharedStreetPointPicksDoNotResetFareOrOperation()
    {
        Outcome result = Run("shared", "exact");
        Assert.True(result.Success);
        Assert.Equal("B", result.Name);
        Assert.Equal("B", result.ActiveName);
        Assert.Equal("2,0,0", result.Goal);
        Assert.Equal("10,0,0", result.Resolved);
        Assert.Equal("10,0,0", result.Target);
        Assert.Equal(1, result.NavigateCalls);
        Assert.Equal(1, result.FareStarts);
        Assert.Equal(1, result.Operations);
    }

    [Theory]
    [InlineData("collapse", "25,0,0", "second")]
    [InlineData("rank", "20,0,0", "second")]
    [InlineData("raw-far", "55,0,0", "second")]
    [InlineData("fallback-boundary", "60,0,0", "first")]
    public void FallbackSeparationGoalBudgetAndRankingUseProjectedStreetPoints(string fixture, string point, string lot)
    {
        Outcome result = Run("fallback", fixture);
        Assert.True(result.Success);
        Assert.Equal(point, result.Point);
        Assert.Equal(lot, result.Lot);
        Assert.Equal(0, result.GeneralQueries);
    }

    [Theory]
    [InlineData("projected-far")]
    [InlineData("vertical-far")]
    [InlineData("all-collapse")]
    [InlineData("no-entry")]
    [InlineData("goal-nan")]
    public void UnprovableFallbackIsRefusedInsteadOfReturningRawLotCoordinates(string fixture) =>
        Assert.False(Run("fallback", fixture).Success);

    [Theory]
    [InlineData("completed-logical", 0)]
    [InlineData("completed-physical-as-goal", 1)]
    [InlineData("completed-other-arrival", 1)]
    [InlineData("completed-no-arrival", 1)]
    [InlineData("completed-no-goal", 1)]
    [InlineData("completed-moving", 1)]
    [InlineData("completed-reversing", 1)]
    [InlineData("completed-no-complete", 1)]
    public void CompletedSkipRequiresBothLogicalGoalAndCorrespondingPhysicalArrival(string fixture, int calls)
    {
        Outcome result = Run("completed", fixture);
        Assert.True(result.Success);
        Assert.Equal(calls, result.NavigateCalls);
        Assert.Equal("100,0,0", result.Target);
        Assert.Equal(fixture == "completed-no-goal" ? null : "110,0,0", result.Goal);
    }

    [Fact]
    public void SuccessfulNativePatrolReboundRemainsOutsideTheDirectStreetGuarantee()
    {
        Outcome result = Run("patrol", "exact");
        Assert.True(result.Success);
        Assert.Equal("12,0,0", result.Target);
        Assert.Equal("12,0,0", result.ActivePoint);
        Assert.Equal("10,0,0", result.Resolved);
        Assert.Equal(0, result.NavigateCalls);
        Assert.Equal(0, result.GeneralQueries);
    }

    [Theory]
    [InlineData("outside", false)]
    [InlineData("vertical-outside", false)]
    [InlineData("nonfinite", false)]
    [InlineData("boundary", true)]
    public void RetainedFallbackMustFitTheNewLogicalGoalsThreeDimensionalBudget(string fixture, bool accepted)
    {
        Outcome result = Run("retained", fixture);
        Assert.Equal(accepted, result.Success);
        Assert.Equal(accepted ? "B" : "A", result.Name);
        Assert.Equal(fixture == "nonfinite" ? "NaN,0,0" : "-50,0,0", result.Target);
        Assert.Equal(1, result.NavigateCalls);
        Assert.Equal(1, result.FareStarts);
        Assert.Equal(1, result.Operations);
    }

    [Fact]
    public void StrictFallbackProvenanceSurvivesOrdinaryRideResetWithoutChangingResolverIdentity()
    {
        Outcome result = Run("reuse", "fallback");
        Assert.True(result.Success);
        Assert.Equal("100,0,0", result.Target);
        Assert.Equal("110,0,0", result.Resolved);
        Assert.Equal("100,0,0", result.StreetCandidate);
        Assert.Equal(0, result.GeneralQueries);
    }

    [Theory]
    [InlineData("picker-reset")]
    [InlineData("picker-clear")]
    [InlineData("patrol-rebound")]
    public void StreetProvenanceIsNotKeptForClearedSelectionOrAnUnverifiedPatrolRebound(string fixture) =>
        Assert.Null(Run("provenance", fixture).StreetCandidate);

    [Fact]
    public void UnknownPhysicalPointDoesNotAcquireStreetProvenanceFromANameOrResolverMismatch()
    {
        Outcome result = Run("reuse", "unverified");
        Assert.True(result.Success);
        Assert.Equal("105,0,0", result.Target);
        Assert.Null(result.StreetCandidate);
        Assert.Equal(1, result.GeneralQueries);
    }

    [Theory]
    [InlineData("stop")]
    [InlineData("teardown")]
    public void InitialPassengerExitParkCannotResumeAfterCancellation(string interruption)
    {
        Outcome result = Run("initial-start", "exit|" + interruption);
        Assert.True(result.Interrupted);
        Assert.False(result.Success);
        Assert.Equal(0, result.NavigateCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
    }

    [Fact]
    public void InitialNavigateAndItsSynchronousCallbackObserveTheCoherentPassengerPoint()
    {
        Outcome result = Run("select-failed", "exact");
        Assert.True(result.Success);
        Assert.True(result.CoherentAtNavigate);
        Assert.True(result.RecoveryPending);
    }

    public static IEnumerable<object[]> InitialPreparationCases()
    {
        foreach (string flow in new[] { "initial-start", "initial-go" })
        {
            string[] boundaries = flow == "initial-start"
                ? new[] { "seat", "exit", "release", "simulation", "enable", "settings-constructor", "settings-road", "settings-proximity", "settings-teleport", "position", "diagnostics", "snapshot", "patrol" }
                : new[] { "exit", "release", "simulation", "enable", "settings-constructor", "settings-road", "settings-proximity", "settings-teleport", "position", "sample", "diagnostics", "snapshot" };
            foreach (string boundary in boundaries)
                foreach (string interruption in new[] { "stop", "teardown", "reroute", "authority", "foreign", "player", "vehicle", "agent", "picker", "lifecycle-restart" })
                    yield return new object[] { flow, boundary, interruption };
        }
    }

    [Theory]
    [MemberData(nameof(InitialPreparationCases))]
    public void InitialPreparationRejectsStaleNativeContinuations(string flow, string boundary, string interruption)
    {
        Outcome result = Run(flow, boundary + "|" + interruption);
        Assert.True(result.Interrupted, "The substituted native boundary must actually execute.");
        Assert.False(result.Success);
        Assert.Equal(0, result.NavigateCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
        if (interruption == "reroute")
        {
            Assert.Equal("New", result.Name);
            Assert.Equal("999,0,0", result.Target);
            Assert.Equal("999,0,0", result.ActivePoint);
        }
    }

    [Theory]
    [InlineData("initial-start", "release|order")]
    [InlineData("initial-go", "settings-constructor|order")]
    [InlineData("initial-start", "exit|operation")]
    [InlineData("initial-go", "settings-proximity|operation")]
    [InlineData("initial-start", "simulation|ride")]
    [InlineData("initial-go", "settings-constructor|ride")]
    [InlineData("initial-start", "release|pointer")]
    [InlineData("initial-go", "sample|pointer")]
    [InlineData("initial-start", "seat|dead")]
    [InlineData("initial-go", "exit|dead")]
    public void EachCapturedRequestIdentityIndependentlyRejectsSupersession(string flow, string fixture)
    {
        Outcome result = Run(flow, fixture);
        Assert.True(result.Interrupted);
        Assert.False(result.Success);
        Assert.Equal(0, result.NavigateCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
    }

    [Theory]
    [InlineData("exit|stop")]
    [InlineData("exit|teardown")]
    [InlineData("release|reroute")]
    [InlineData("settings-constructor|reroute")]
    [InlineData("navigate|stop")]
    [InlineData("navigate|reroute")]
    [InlineData("fallback-settings-constructor|reroute")]
    public void RootRideStartCannotCleanUpAReplacementAfterItsOldDispatchWasCancelled(string fixture)
    {
        Outcome result = Run("initial-root", fixture);
        Assert.True(result.Interrupted);
        Assert.False(result.Success);
        Assert.Equal(0, result.EndRideCalls);
        if (fixture.EndsWith("reroute"))
        {
            Assert.Equal("New", result.Name);
            Assert.Equal("999,0,0", result.Target);
        }
    }

    [Theory]
    [InlineData("initial-root", true, 0)]
    [InlineData("initial-root-failure", false, 1)]
    public void RootRideStartStillCompletesOrCleansUpItsOwnRequest(string flow, bool success, int cleanups)
    {
        Outcome result = Run(flow, "none|none");
        Assert.Equal(success, result.Success);
        Assert.Equal(cleanups, result.EndRideCalls);
        Assert.True(result.CoherentAtNavigate);
    }

    [Theory]
    [InlineData("pickup-resolver|reroute")]
    [InlineData("exit|stop")]
    [InlineData("exit|teardown")]
    [InlineData("settings-constructor|reroute")]
    [InlineData("navigate|reroute")]
    [InlineData("navigate|stop")]
    [InlineData("fallback-settings-constructor|reroute")]
    public void AutomaticPickupCannotCleanUpOrPublishAfterItsInitialRequestWasSuperseded(string fixture)
    {
        Outcome result = Run("initial-auto", fixture);
        Assert.True(result.Interrupted);
        Assert.Equal(0, result.AutoCleanups);
        if (fixture.EndsWith("reroute"))
        {
            Assert.Equal("New", result.Name);
            Assert.Equal("999,0,0", result.Target);
        }
    }

    [Fact]
    public void ChangingOnlyThePreselectedPassengerDestinationDoesNotRetargetOrCancelPickup()
    {
        Outcome result = Run("initial-auto", "settings-constructor|selection");
        Assert.Equal(1, result.NavigateCalls);
        Assert.Equal(0, result.AutoCleanups);
        Assert.Equal("1,0,0", result.Target);
        Assert.Equal("Next", result.Name);
        Assert.Equal("2,0,0", result.Destination);
        Assert.Equal("2,0,0", result.StreetCandidate);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("unreadable")]
    public void CaptureUnavailableAutoRefusesOnceInsteadOfRemainingBusyAcrossTicks(string fault)
    {
        Outcome result = Run("capture-auto", "ordinary|" + fault);
        Assert.False(result.AutoRunning);
        Assert.Equal(0, result.AutoStep);
        Assert.NotEmpty(result.ServiceStatus);
        Assert.Equal(0, result.NavigateCalls);
        Assert.True(result.AgentReads <= 3, "Later ticks must not repeat unavailable-agent acquisition.");
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("unreadable")]
    [InlineData("missing-after-pickup")]
    [InlineData("unreadable-after-pickup")]
    public void CaptureUnavailableRootRollsBackItsOwnPhantomRideAndLocks(string fault)
    {
        Outcome result = Run("capture-root", "ordinary|" + fault);
        Assert.False(result.RideActive);
        Assert.False(result.PassengerMode);
        Assert.False(result.TrunkLocked);
        Assert.Null(result.ActivePoint);
        Assert.NotEmpty(result.ServiceStatus);
        Assert.Equal(1, result.EndRideCalls);
        Assert.Equal(0, result.NavigateCalls);
        Assert.False(result.Terminal); // prior pickup terminal must not become a new passenger verdict
    }

    [Theory]
    [InlineData("capture-auto", "stop")]
    [InlineData("capture-auto", "reroute")]
    [InlineData("capture-root", "stop")]
    [InlineData("capture-root", "reroute")]
    public void CaptureUnavailableOriginalCancellationControlsRemainInert(string flow, string interruption)
    {
        Outcome result = Run(flow, "capture|" + interruption + "|missing");
        Assert.True(result.Interrupted);
        Assert.Equal(0, result.AutoCleanups);
        Assert.Equal(0, result.EndRideCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
        Assert.Empty(result.ServiceStatus);
        if (interruption == "reroute") Assert.Equal("999,0,0", result.Target);
    }

    public static IEnumerable<object[]> CaptureInterruptionCases()
    {
        foreach (string flow in new[] { "capture-auto", "capture-root" })
            foreach (string fault in new[] { "missing", "unreadable" })
                foreach (string interruption in new[] { "stop", "reroute", "teardown", "lifecycle-restart", "vehicle", "ride", "order", "operation" })
                    yield return new object[] { flow, fault, interruption };
    }

    [Theory]
    [MemberData(nameof(CaptureInterruptionCases))]
    public void CaptureUnavailableCannotRefuseAnotherRequestAfterNativeAgentAcquisition(string flow, string fault, string interruption)
    {
        string boundary = fault == "missing" ? "capture" : "capture-agent";
        Outcome result = Run(flow, boundary + "|" + interruption + "|" + fault);
        Assert.True(result.Interrupted);
        Assert.Equal(0, result.AutoCleanups);
        Assert.Equal(0, result.EndRideCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
        Assert.Empty(result.ServiceStatus);
        if (interruption == "reroute") Assert.Equal("999,0,0", result.Target);
    }

    [Theory]
    [InlineData("capture-auto", "fallback-unreadable")]
    [InlineData("capture-root", "fallback-unreadable")]
    [InlineData("capture-auto", "vehicle-missing")]
    public void OtherOrdinaryAcquisitionFailuresAlsoEndOnlyTheirOwnPendingWork(string flow, string fault)
    {
        Outcome result = Run(flow, "ordinary|" + fault);
        Assert.False(result.AutoRunning);
        Assert.False(result.RideActive);
        Assert.NotEmpty(result.ServiceStatus);
        Assert.Equal(0, result.NavigateCalls);
    }

    [Theory]
    [InlineData("foreign")]
    [InlineData("authority")]
    [InlineData("player")]
    [InlineData("pointer")]
    public void UnavailableCaptureWithUncertainNativeOwnershipGetsManagedRollbackOnly(string uncertainty)
    {
        Outcome result = Run("capture-root", "capture|" + uncertainty + "|missing");
        Assert.False(result.RideActive);
        Assert.False(result.PassengerMode);
        Assert.NotEmpty(result.ServiceStatus);
        Assert.Equal(0, result.EndRideCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
        Assert.Equal(0, result.NavigateCalls);
    }

    [Fact]
    public void UnavailablePickupReceiptDoesNotBindTheIndependentPassengerSelection()
    {
        Outcome result = Run("capture-auto", "capture|selection|missing");
        Assert.False(result.AutoRunning);
        Assert.NotEmpty(result.ServiceStatus);
        Assert.Equal("Next", result.Name);
        Assert.Equal("2,0,0", result.Destination);
        Assert.Equal(0, result.EndRideCalls);
        Assert.Equal(0, result.AutoCleanups);
    }

    [Fact]
    public void UnavailablePickupCannotRefuseAnAutomationReplacementWithTheSameNavigationIds()
    {
        Outcome result = Run("capture-auto", "capture|auto-replace|missing");
        Assert.True(result.AutoRunning);
        Assert.Equal(0, result.AutoCleanups);
        Assert.Empty(result.ServiceStatus);
    }

    [Fact]
    public void RefusedAutomaticAcquisitionDoesNotBlockAFreshRequestAfterReset()
    {
        Outcome result = Run("capture-auto-retry", "ordinary|missing");
        Assert.False(result.AutoRunning);
        Assert.Equal(1, result.NavigateCalls);
        Assert.Equal(1, result.Operations);
        Assert.Equal(0, result.AutoCleanups);
    }

    [Theory]
    [InlineData("navigate|stop")]
    [InlineData("navigate|teardown")]
    [InlineData("navigate|reroute")]
    [InlineData("fallback-settings-constructor|stop")]
    [InlineData("fallback-settings-road|teardown")]
    [InlineData("fallback-settings-proximity|reroute")]
    [InlineData("fallback-settings-teleport|authority")]
    public void ThrowingNativeCallCannotAuthorizeAnObsoleteCallbacklessRetry(string fixture)
    {
        Outcome result = Run("initial-throw", fixture);
        Assert.True(result.Interrupted);
        Assert.False(result.Success);
        Assert.Equal(1, result.NavigateCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
    }

    [Theory]
    [InlineData("stop")]
    [InlineData("teardown")]
    [InlineData("reroute")]
    public void NativeReturnDoesNotRepublishThePassengerTripAfterCancellation(string interruption)
    {
        Outcome result = Run("initial-start", "navigate|" + interruption);
        Assert.False(result.Success);
        Assert.Equal(1, result.NavigateCalls);
        Assert.Equal(interruption == "reroute" ? "999,0,0" : null, result.ActivePoint);
    }

    [Theory]
    [InlineData("initial-start", "exit|stop-throw")]
    [InlineData("initial-start", "release|teardown-throw")]
    [InlineData("initial-start", "position|reroute-throw")]
    [InlineData("initial-go", "exit|stop-throw")]
    [InlineData("initial-go", "simulation|teardown-throw")]
    [InlineData("initial-go", "enable|authority-throw")]
    [InlineData("initial-go", "sample|reroute-throw")]
    [InlineData("initial-go", "settings-constructor|stop-throw")]
    [InlineData("initial-throw", "fallback-settings-constructor|reroute-throw")]
    [InlineData("initial-throw", "fallback-settings-road|teardown-throw")]
    public void AThrowAfterNativeCancellationNeverReauthorizesTheOldPreparation(string flow, string fixture)
    {
        Outcome result = Run(flow, fixture);
        Assert.True(result.Interrupted);
        Assert.False(result.Success);
        Assert.Equal(flow == "initial-throw" ? 1 : 0, result.NavigateCalls);
        Assert.Equal(0, result.WritesAfterInterruption);
    }

    [Theory]
    [InlineData("inactive")]
    [InlineData("authority")]
    [InlineData("foreign")]
    [InlineData("player")]
    [InlineData("teardown")]
    [InlineData("order-changed")]
    [InlineData("operation-changed")]
    [InlineData("missing-completion-scope")]
    public void StaleCompletedIdentityCannotBypassInitialRequestGuards(string fixture)
    {
        Outcome result = Run("stale-completed", fixture);
        bool fresh = fixture is "inactive" or "order-changed" or "operation-changed" or "missing-completion-scope";
        Assert.Equal(fresh ? 1 : 0, result.NavigateCalls);
        Assert.False(result.Arrived);
        if (!fresh) Assert.False(result.Success);
    }

    private static Outcome Run(string scenario, string fixture) =>
        JsonSerializer.Deserialize<Outcome>((string)Replay.Value.Invoke(null, new object[] { scenario, fixture })!)!;

    private sealed record Outcome(bool Success, bool ArrivalOk, string Point, string Rule, string Reason, float Distance,
        string Target, string? Destination, string? ActivePoint, string? Goal, string? Resolved, string Name, string ActiveName,
        string Lot, int StreetQueries, int GeneralQueries, string Constraint, int NavigateCalls, string Settings,
        int FareStarts, int Operations, bool RecoveryPending, bool Arrived, bool Terminal, string? Callback,
        string? StreetCandidate, bool CoherentAtNavigate, bool Interrupted, int WritesAfterInterruption, int EndRideCalls, int AutoCleanups,
        bool AutoRunning, int AutoStep, bool RideActive, bool PassengerMode, string ServiceStatus, bool TrunkLocked, int AgentReads);

    // Opt-in baseline source replay uses exactly the saved production methods. Normal tests need no .opencode artifacts.
    private static string Read(string file)
    {
        string? root = Environment.GetEnvironmentVariable("TAXI_ENDPOINT_TEST_SOURCE_ROOT");
        return string.IsNullOrWhiteSpace(root) ? DriverSafetySourceContractTests.ReadSource(file) : File.ReadAllText(Path.Combine(root, file));
    }

    private static MethodInfo Compile()
    {
        var commands = CSharpSyntaxTree.ParseText(Read("SpikeCommands.cs")).GetRoot();
        var road = CSharpSyntaxTree.ParseText(Read("RoadNavigation.cs")).GetRoot();
        var destinations = CSharpSyntaxTree.ParseText(Read("TaxiDestinations.cs")).GetRoot();
        var runner = CSharpSyntaxTree.ParseText(Read("SpikeRunner.cs")).GetRoot();
        var state = CSharpSyntaxTree.ParseText(Read("SpikeState.cs")).GetRoot();
        var policies = CSharpSyntaxTree.ParseText(Read("DrivingPolicies.cs")).GetRoot();
        var selection = CSharpSyntaxTree.ParseText(Read("TaxiInteractionPolicies.cs")).GetRoot();
        string[] destinationMethods = { "ResolveArrival", "TryFallbackDropOff", "TryNearestLotEntry", "Usable", "SafeName", "Num", "Fmt" };
        string[] commandMethods = { "Go", "CreateNavigationSettings", "StartRide", "StartRideDrive", "ToDestination", "PickPoint", "ReRoute", "SetRideDestination" };
        string[] runnerMethods = { "NavigationCallbackFor", "OnNavigationResult", "TickNavigationCallback", "RecordPendingArrival", "TickAutoRun" };
        string[] policyTypes = { "NavigationArrivalPolicy", "RideSatisfactionPolicy", "NavigationCallbackPolicy",
            "NavigationCallbackOutcomePolicy", "NavigationOutcomeAction", "NavigationOperationPolicy", "OperationPhase" };
        string[] selectionTypes = { "DestinationPoint", "DestinationSelection", "DestinationPickMode", "DestinationPickResult", "DestinationSelectionPolicy" };
        var roadClass = road.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.Text == "RoadTarget");
        string roadMethods = string.Join("\n", roadClass.Members.OfType<MethodDeclarationSyntax>()
            .Where(m => m.Identifier.Text is "TryStreetDropOff" or "Finite" or "NearestRoadNode" or "SafeGraphName" or "DescribeGraph")
            .Select(m => m.ToFullString()));
        string source = Harness
            .Replace("/* AUTO RECEIPT */", string.Join("\n", runner.DescendantNodes().OfType<FieldDeclarationSyntax>()
                .Where(f => f.Declaration.Variables.Any(v => v.Identifier.Text is "_autoNavigationReceipt" or "_autoNavigationSerial"))
                .Select(f => f.ToFullString())))
            .Replace("/* REQUEST */", string.Join("\n", commands.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(c => c.Identifier.Text == "DriveRequest").Select(c => c.ToFullString())))
            .Replace("/* STATE */", string.Join("\n", state.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.Text is "ResetRide" or "ResetPicker" or "ClearDestination" or "ResetAutoRun" or "SetServiceStatus").Select(m => m.ToFullString())))
            .Replace("/* ROAD */", roadMethods)
            .Replace("/* DESTINATIONS */", string.Join("\n", destinations.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Single(c => c.Identifier.Text == "TaxiDestinations").Members
                .Where(m => m is FieldDeclarationSyntax || m is ClassDeclarationSyntax c && c.Identifier.Text is "Destination" or "Arrival" ||
                    m is MethodDeclarationSyntax method && destinationMethods.Contains(method.Identifier.Text)).Select(m => m.ToFullString())))
            .Replace("/* COMMANDS */", string.Join("\n", commands.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m => commandMethods.Contains(m.Identifier.Text)).Select(m => m.ToFullString())))
            .Replace("/* RUNNER */", string.Join("\n", runner.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m => runnerMethods.Contains(m.Identifier.Text)).Select(m => m.ToFullString())))
            .Replace("/* POLICIES */", string.Join("\n", policies.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
                .Where(t => policyTypes.Contains(t.Identifier.Text)).Select(t => t.ToFullString())))
            .Replace("/* IDENTITY */", policies.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.Text == "DestinationIdentityPolicy").ToFullString())
            .Replace("/* SELECTION */", string.Join("\n", selection.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
                .Where(t => selectionTypes.Contains(t.Identifier.Text)).Select(t => t.ToFullString())));
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator).Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("TaxiStreetCandidateReplay", new[] { CSharpSyntaxTree.ParseText(source) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        Assert.True(result.Success, string.Join("\n", result.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.Entry")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        using System.Linq;
        using System.Collections.Generic;
        using System.Globalization;
        using System.Text.Json;
        using UnityEngine;
        using Il2CppPathfinding;
        using Il2CppScheduleOne.Vehicles.AI;
        using Replay;
        namespace UnityEngine {
        public record struct Vector3(float x, float y, float z) {
            public static Vector3 zero => new(); public static Vector3 forward => new(0,0,1);
            public float sqrMagnitude => x*x+y*y+z*z;
            public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.x+b.x,a.y+b.y,a.z+b.z);
            public static Vector3 operator *(Vector3 a, float b) => new(a.x*b,a.y*b,a.z*b);
            public static float Distance(Vector3 a, Vector3 b) => MathF.Sqrt((a.x-b.x)*(a.x-b.x)+(a.y-b.y)*(a.y-b.y)+(a.z-b.z)*(a.z-b.z));
        }
        public enum FindObjectsSortMode { None }
        public class Object {
            public IntPtr Pointer = new(1); public string name = "object";
            public static T[] FindObjectsByType<T>(FindObjectsSortMode mode) => Native.Lots.Cast<T>().ToArray();
        }
        public class Transform : Object {
            private Vector3 value;
            public Vector3 position { get { Native.At("position"); return value; } set { this.value = value; } }
            public Vector3 forward = Vector3.forward;
        }
        }
        namespace Il2CppPathfinding {
        public class NavGraph {
            public string Name; public uint graphIndex;
            public string name { get { if (Native.UnreadableGraph) throw new Exception("graph unreadable"); return Name; } }
        }
        public class GraphNode { public bool Walkable = true; public uint GraphIndex = 2; }
        public class NNInfo { public GraphNode node = new(); public Vector3 position; }
        public record struct GraphMask(uint Index) { public static GraphMask FromGraph(NavGraph graph) => new(graph.graphIndex); }
        public class NNConstraint { public GraphMask graphMask; public bool constrainWalkability, walkable, distanceXZ, constrainDistance = true; }
        }
        namespace Il2Cpp {
        public class AstarPath {
            public static AstarPath active; public NavGraph[] graphs;
            public NNInfo GetNearest(Vector3 point, NNConstraint constraint) {
                Native.StreetQueries++; Native.Constraint = $"{constraint.graphMask.Index}/{constraint.constrainWalkability}/{constraint.walkable}/{constraint.constrainDistance}";
                Native.Interrupt();
                if (Native.NearestThrows) throw new Exception("nearest threw");
                return new() { position = Native.Project(point), node = Native.Node };
            }
        }
        }
        namespace Il2CppScheduleOne.Vehicles.AI {
        public sealed class NavigationSettings {
            private readonly int id; private bool road, proximity, teleport;
            public NavigationSettings() { id = ++Native.SettingsCount; Native.At(id == 1 ? "settings-constructor" : "fallback-settings-constructor"); }
            public bool endAtRoad { get => road; set { Native.At(id == 1 ? "settings-road" : "fallback-settings-road"); road = value; } }
            public bool ensureProximityToGraph { get => proximity; set { Native.At(id == 1 ? "settings-proximity" : "fallback-settings-proximity"); proximity = value; } }
            public bool teleportToGraphIfCalculationFails { get => teleport; set { Native.At(id == 1 ? "settings-teleport" : "fallback-settings-teleport"); teleport = value; } }
        }
        public class VehicleAgent {
            public IntPtr Pointer = new(2);
            public static string RoadNodesGraphName => "Road Nodes";
            private bool active = true;
            public bool enabled { get => active; set { Native.Write(); Native.At("enable"); active = value; } }
            public bool NavigationCalculationInProgress; public string KinematicMode => "native";
            public enum ENavigationResult { Failed, Complete, Stopped }
            public void Navigate(Vector3 target, NavigationSettings settings, Action<ENavigationResult> callback) {
                Native.Write();
                Native.CoherentAtNavigate &= SpikeState.RideDestination == target && SpikeState.ActiveTripPoint == target;
                Native.NavigateCalls++; Native.Settings = $"{settings.endAtRoad}/{settings.ensureProximityToGraph}/{settings.teleportToGraphIfCalculationFails}";
                if (Native.FailCalculation) callback?.Invoke(ENavigationResult.Failed);
                Native.At("navigate");
                if (Native.ThrowAll || Native.ThrowFirst && Native.NavigateCalls == 1) throw new Exception("native Navigate threw after entry");
            }
        }
        public class ParkingLot : UnityEngine.Object { public Transform EntryPoint, ExitPoint; public Transform transform = new(); }
        public static class NavigationUtility { public static Vector3 SampleVehicleGraph(Vector3 point) { Native.GeneralQueries++; Native.At("sample"); return Native.GeneralPoint; } }
        }
        namespace Replay { using Vector3 = System.Numerics.Vector3; /* IDENTITY */ }
        namespace Replay {
        /* POLICIES */
        /* SELECTION */
        public static class RoadTarget { /* ROAD */ }
        public static class TaxiDestinations { /* DESTINATIONS */ }
        public class NPC {}
        public class LandVehicle {
            public IntPtr Pointer = new(1); public Transform transform = new(); private VehicleAgent agent = new();
            public VehicleAgent Agent { get { Native.AgentReads++; Native.At("capture-agent"); if (Native.CaptureFault == "unreadable") throw new Exception("agent unreadable"); return agent; } set { agent = value; } }
            public LandVehicle gameObject => this;
            public T GetComponent<T>() where T : class { Native.At("capture"); if (Native.CaptureFault == "fallback-unreadable") throw new Exception("fallback unreadable"); return agent as T; }
            public bool isParked, BrakesApplied, HandbrakeApplied, IsPhysicallySimulated = true, IsReversing; public float Speed_Kmh;
            public bool ShouldBePhysicallySimulated() => true; public void UpdatePhysicallySimulated(bool value) { Native.Write(); Native.At("simulation"); IsPhysicallySimulated = value; }
            public void ExitPark(bool value) { Native.Write(); Native.At("exit"); isParked = false; }
            public void ExitVehicle() { Native.Write(); }
        }
        public class Player { public static Player Local = new(); public Transform transform = new(); }
        public static class Time { public static float unscaledTime = 100, timeScale = 1; public static int frameCount; }
        public sealed class Life {
            private bool accepting = true; private long generation;
            public bool AcceptingWork { get => accepting; set { if (value && !accepting) generation++; accepting = value; } }
            public Action<T> Guard<T>(Action<T> callback) { long captured = generation; return value => { if (AcceptingWork && captured == generation) callback(value); }; }
        }
        public static class Mod { public static readonly Life Lifecycle = new(); public static readonly Log Log = new(); }
        public class Log { public void Info(string s) {} public void Warn(string s) {} public void Error(string s) {} }
        public static class TaxiLog { public static void Verbose(string s) {} }
        public static class FareMeter { public static int Starts; public static void Start() { Starts++; } }
        public static class RoadKeeper { public static void Reset() {} }
        public static class NativeActuatorPatch { public static bool Healthy = true; public static void ReleasePark(LandVehicle v) { Native.Write(); Native.At("release"); } }
        public enum RideProtectionPatch { None }
        public static class RideProtectionPolicy { public static RideProtectionPatch Missing(int flags) => RideProtectionPatch.None; }
        public static class RideLocks { public static bool Patched = true; public static int Installed; public static void LockTrunk(LandVehicle v) { Native.TrunkLocked = true; Native.Write(); Native.At("lock"); } }
        public static class SpikeTrace { public static void EnsureRouteDiagnostics() { Native.At("diagnostics"); } public static void RouteSnapshot(VehicleAgent a, string s) { Native.At("snapshot"); } }
        public static class TaxiStand { public static Vector3 StandCoordinate => new(2,0,0); public static string StandName => "stand"; }
        public static class TaxiAI {
            public static bool NavigationBlocked, Active;
            public static bool StartPatrol(NPC npc, LandVehicle v, Vector3 target, string name, out Vector3 resolved) { Native.At("patrol"); resolved = new(12,0,0); return Native.Patrol; }
        }
        public class Reverse { public bool Active; }
        public static class SpikeState {
            public const string NoDestinationName = "none selected";
            public static LandVehicle Vehicle; public static NPC DriverNpc;
            public static Vector3 NavTarget, NavStartPosition, NavReDispatchTarget;
            public static Vector3? RideDestination, ActiveTripPoint, RideGoal, RideResolvedArrival, RideStreetCandidate, CompletedGoal, CompletedArrival;
            public static string RideDestinationName, RideDestinationKind, RideDropOff, RideDropOffLot, ActiveTripName;
            public static string CompletedDestinationName, CompletedDestinationKind, NavCallbackResult, NavCallbackQueuedResult;
            public static bool RideActive, RideBoarded, RidePassengerMode, RideAwaitingDestination, RideArrived, RideDestinationPicked, RideAwaitingBoard;
            public static bool PollingActive, NavToPlayer, NavEverAutoDriving, NavRetried, NavGaveUp, NavCallbackRecoveryPending, NavCallbackArrivalPending;
            public static int NavOrder, StuckRecoveries, NavCallbackQueuedOrder, NavCallbackRecoveryOrder, NavCallbackArrivalOrder;
            public static int CompletedNavOrder;
            public static long CompletedOperation;
            public static long OperationId, NavCallbackQueuedOperation, NavCallbackRecoveryOperation, NavCallbackArrivalOperation, RideSerial;
            public static string PendingSpawnCode, AutoCode, LastVehicleCode = "taxi", ServiceStatus = "";
            public static long ServiceStatusRevision;
            public static bool AutoRunning, AutoToPlayer, SkipNpcStep; public static int AutoStep;
            public static long AutoRunSerial;
            public static float AutoNextAt; public static Vector3? AutoTarget;
            public static void CancelPendingSpawn() { PendingSpawnCode = null; }
            public static float NavStartDistance, NavStartTime, LastHeartbeat, NavRetryAt, NavReDispatchAt, NavCallbackArrivalAt;
            public static readonly Reverse Reverse = new(); internal static readonly NavigationOperationPolicy Operation = new();
            public static int NextNavOrder() => ++NavOrder; public static long NextOperationId() => ++OperationId;
            /* STATE */
        }
        public static class SpikeRunner {
            /* AUTO RECEIPT */
            private const float AutoRideDistance = 40, AutoStepDelaySeconds = 1;
            private static bool _recoveryOwnsNavigation;
            public static int Operations;
            public static void BeginNavigationOperation(Vector3? preparedPosition = null, float? preparedTime = null) { Operations++; SpikeState.NextOperationId(); SpikeState.Operation.Begin(); }
            public static void ArmProgressWindow(LandVehicle v) {} public static void ArmProgressWindow(Vector3 position, float distance, float now) {} public static void ArmStartupRecovery(LandVehicle v, float? preparedTime = null) {}
            public static void DisarmStartupRecovery() {} public static void ArmPatrolSelfTest(int order, float? preparedTime = null) {}
            public static void Drain() => TickNavigationCallback();
            public static void RunAuto() => TickAutoRun();
            /* RUNNER */
        }
        public static class SpikeCommands {
            private static int _earlyBoardStage, _earlyBoardOrder;
            private static LandVehicle _earlyBoardVehicle;
            public static Vector3 RideTargetRoadA => new(3,0,0); public static Vector3 RideTargetRoadB => new(4,0,0);
            public static bool EnsureAuthority() => Native.Authority; public static bool HasAuthority() => Native.Authority;
            public static bool IsOwnedTaxi(LandVehicle v) => Native.Owned && ReferenceEquals(v, SpikeState.Vehicle) && v.Pointer != IntPtr.Zero;
            public static bool RequireOwnedTaxi(LandVehicle v, string why) => IsOwnedTaxi(v);
            public static bool SafeDriverPlayerPresent(LandVehicle v) => Native.PlayerDriver;
            public static bool EnsurePassengerSeat(LandVehicle v) { Native.At("seat"); return true; }
            public static bool DriverAtWheel(LandVehicle v) => true;
            public static bool BoardingNavigationCancelled(LandVehicle v) => true;
            public static bool Spawn(string code) => false; public static bool Npc() => false;
            public static Vector3? PlayerRoadTarget() { Native.At("pickup-resolver"); return new(10,0,0); }
            public static bool Cleanup(string reason, bool explicitRetry = false) { Native.AutoCleanups++; Native.Write(); SpikeState.ResetRide(); SpikeState.NavTarget = Vector3.zero; return true; }
            public static void EndRide(string why, bool stopNavigation = true) { Native.EndRideCalls++; Native.Write(); Native.TrunkLocked = false; SpikeState.ResetRide(); SpikeState.NavTarget = Vector3.zero; SpikeState.Operation.Reset(); }
            public static bool SafePlayerInVehicle(LandVehicle v) => false;
            public static void SafeExitGroundSnap(LandVehicle v) {}
            public static void ParkCar(LandVehicle v, string why) { Native.Write(); }
            public static string codeOf(LandVehicle v) => "taxi";
            public static string SafeId(NPC n) => "taxi_driver";
            public static string SafeDriverPlayerName(LandVehicle v) => "null";
            public static string SafeStateName(LandVehicle v) => "native"; public static void Print(string s) {}
            public static string Fmt(Vector3 point) => FormattableString.Invariant($"{point.x},{point.y},{point.z}");
            public static string Format(Vector3? point) => point.HasValue ? Fmt(point.Value) : null;
            public static System.Numerics.Vector3 ToNumerics(Vector3 point) => new(point.x,point.y,point.z);
            public static bool To(string key, string caller) => false;
            public static bool Drive(Vector3 target) => StartRideDrive(SpikeState.DriverNpc, SpikeState.Vehicle, target, "test");
            /* REQUEST */
            /* COMMANDS */
        }
        public static class Native {
            public static ParkingLot[] Lots = Array.Empty<ParkingLot>(); public static GraphNode Node;
            public static Vector3 StreetPoint, GeneralPoint; public static Func<Vector3,Vector3> Project;
            public static int StreetQueries, GeneralQueries, NavigateCalls; public static string Constraint, Settings, Interruption;
            public static bool UnreadableGraph, NearestThrows, FailCalculation, Patrol;
            public static string CaptureFault = ""; public static int AgentReads; public static bool TrunkLocked;
            public static bool Authority, Owned, PlayerDriver, PrepArmed, Interrupted, CoherentAtNavigate, ThrowFirst, ThrowAll;
            public static int WritesAfterInterruption, SettingsCount, EndRideCalls, AutoCleanups; public static string PrepBoundary, PrepAction;
            public static void Write() { if (Interrupted) WritesAfterInterruption++; }
            public static void At(string boundary) {
                if (!PrepArmed || Interrupted || PrepBoundary != boundary) return;
                Interrupted = true;
                bool throws = PrepAction.EndsWith("-throw");
                string action = throws ? PrepAction[..^6] : PrepAction;
                switch (action) {
                    case "stop":
                    case "teardown":
                        SpikeState.NextNavOrder(); SpikeState.PollingActive = SpikeState.RideActive = false;
                        SpikeState.NavTarget = Vector3.zero; SpikeState.ActiveTripPoint = null; SpikeState.Operation.Terminate("STOP", false);
                        if (action == "teardown") Mod.Lifecycle.AcceptingWork = false;
                        break;
                    case "reroute":
                        SpikeState.NextNavOrder(); SpikeState.OperationId++; SpikeState.RideSerial++;
                        SpikeState.RideDestinationName = SpikeState.ActiveTripName = "New";
                        SpikeState.NavTarget = new(999,0,0); SpikeState.RideDestination = SpikeState.ActiveTripPoint = SpikeState.RideGoal = SpikeState.RideResolvedArrival = SpikeState.NavTarget;
                        SpikeState.RideStreetCandidate = null;
                        break;
                    case "authority": Authority = false; break;
                    case "foreign": Owned = false; break;
                    case "player": PlayerDriver = true; break;
                    case "vehicle": SpikeState.Vehicle = new() { Pointer = new(3) }; break;
                    case "agent": SpikeState.Vehicle.Agent = new() { Pointer = new(4) }; break;
                    case "order": SpikeState.NextNavOrder(); break;
                    case "operation": SpikeState.OperationId++; break;
                    case "ride": SpikeState.RideSerial++; break;
                    case "pointer": SpikeState.Vehicle.Pointer = new(3); break;
                    case "dead": SpikeState.Vehicle.Pointer = IntPtr.Zero; break;
                    case "picker": SpikeState.ResetPicker(); break;
                    case "selection":
                        SpikeState.RideDestinationName = "Next";
                        SpikeState.RideGoal = SpikeState.RideResolvedArrival = SpikeState.RideDestination = SpikeState.RideStreetCandidate = new(2,0,0);
                        break;
                    case "auto-replace": SpikeState.AutoCode = "new-request"; SpikeState.AutoTarget = new(99,0,0); break;
                    case "lifecycle-restart": Mod.Lifecycle.AcceptingWork = false; Mod.Lifecycle.AcceptingWork = true; break;
                }
                if (throws) throw new Exception("native boundary threw after cancellation");
            }
            public static void Interrupt() {
                if (Interruption == "stop") { SpikeState.PollingActive = false; SpikeState.NextNavOrder(); }
                if (Interruption == "reroute") { SpikeState.OperationId++; SpikeState.NextNavOrder(); SpikeState.RideDestinationName = "New"; SpikeState.NavTarget = new(999,0,0); }
                if (Interruption == "teardown") Mod.Lifecycle.AcceptingWork = false;
                Interruption = "";
            }
        }
        public static class Entry {
            public static string Run(string scenario, string fixture) {
                Native.StreetQueries = Native.GeneralQueries = Native.NavigateCalls = 0; Native.Constraint = Native.Settings = Native.Interruption = "";
                Native.UnreadableGraph = Native.NearestThrows = Native.FailCalculation = Native.Patrol = false;
                Native.CaptureFault = ""; Native.AgentReads = 0; Native.TrunkLocked = false;
                Native.Authority = Native.Owned = Native.CoherentAtNavigate = true;
                Native.PlayerDriver = Native.PrepArmed = Native.Interrupted = Native.ThrowFirst = Native.ThrowAll = false;
                Native.SettingsCount = Native.WritesAfterInterruption = Native.EndRideCalls = Native.AutoCleanups = 0; Native.PrepBoundary = Native.PrepAction = "";
                Native.Lots = Array.Empty<ParkingLot>(); Native.Node = new(); Native.StreetPoint = new(10,0,0); Native.GeneralPoint = new(1,0,0);
                Native.Project = point => Native.StreetPoint;
                Il2Cpp.AstarPath.active = new() { graphs = new[] { new NavGraph { Name = "General Vehicle Graph", graphIndex = 0 },
                    new NavGraph { Name = "Misleading Road Preview", graphIndex = 1 }, new NavGraph { Name = "Road Nodes", graphIndex = 2 } } };
                Mod.Lifecycle.AcceptingWork = true; TaxiAI.NavigationBlocked = TaxiAI.Active = false;
                FareMeter.Starts = SpikeRunner.Operations = 0;
                SpikeState.Vehicle = new(); SpikeState.DriverNpc = null; SpikeState.Operation.Reset(); SpikeState.OperationId = 0; SpikeState.NavOrder = 40;
                SpikeState.RideSerial = 1; SpikeState.RideStreetCandidate = null;
                SpikeState.ResetAutoRun(); SpikeState.PendingSpawnCode = null; SpikeState.ServiceStatus = "";
                SpikeState.RideDestination = SpikeState.ActiveTripPoint = SpikeState.RideGoal = SpikeState.RideResolvedArrival = SpikeState.NavTarget = new(-200,0,0);
                SpikeState.RideDestinationName = SpikeState.ActiveTripName = "Old"; SpikeState.RideDestinationKind = "property";
                SpikeState.RideDropOff = "old rule"; SpikeState.RideDropOffLot = "Old lot";
                SpikeState.RideActive = SpikeState.PollingActive = SpikeState.RideDestinationPicked = true;
                SpikeState.RideArrived = SpikeState.RideBoarded = SpikeState.RidePassengerMode = SpikeState.NavGaveUp = SpikeState.NavToPlayer = false;
                SpikeState.CompletedGoal = SpikeState.CompletedArrival = null; SpikeState.CompletedDestinationName = SpikeState.CompletedDestinationKind = "";
                SpikeState.CompletedNavOrder = -1; SpikeState.CompletedOperation = -1;
                SpikeState.NavCallbackResult = SpikeState.NavCallbackQueuedResult = null;
                SpikeState.NavCallbackRecoveryPending = SpikeState.NavCallbackArrivalPending = false;
                var destination = new TaxiDestinations.Destination { Name = "A", Kind = "property", Goal = new(0,0,0) };
                switch (fixture) {
                    case "misleading": Il2Cpp.AstarPath.active.graphs = Il2Cpp.AstarPath.active.graphs.Take(2).ToArray(); break;
                    case "case-only": Il2Cpp.AstarPath.active.graphs[2].Name = "road nodes"; break;
                    case "duplicate": Il2Cpp.AstarPath.active.graphs = Il2Cpp.AstarPath.active.graphs.Append(new NavGraph { Name = "Road Nodes", graphIndex = 3 }).ToArray(); break;
                    case "missing-active": Il2Cpp.AstarPath.active = null; break;
                    case "missing-graphs": Il2Cpp.AstarPath.active.graphs = null; break;
                    case "null-node": Native.Node = null; break;
                    case "wrong-graph": Native.Node.GraphIndex = 0; break;
                    case "unwalkable": Native.Node.Walkable = false; break;
                    case "goal-nan": destination.Goal = new(float.NaN,0,0); break;
                    case "point-nan": Native.StreetPoint = new(float.NaN,0,0); break;
                    case "point-infinity": Native.StreetPoint = new(0,float.PositiveInfinity,0); break;
                    case "distance-overflow": Native.StreetPoint = new(float.MaxValue,0,0); break;
                    case "too-far": Native.StreetPoint = new(60.01f,0,0); break;
                    case "vertical-too-far": Native.StreetPoint = new(0,60.01f,0); break;
                    case "boundary": Native.StreetPoint = new(60,0,0); break;
                    case "vertical-boundary": Native.StreetPoint = new(0,60,0); break;
                    case "unreadable-graph": Native.UnreadableGraph = true; break;
                    case "nearest-throws": Native.NearestThrows = true; break;
                    case "barn": destination.Goal = Native.GeneralPoint = new(172.7f,0,-13.8f); Native.StreetPoint = new(154,0,18.6f); break;
                    case "bungalow": destination.Goal = new(-157.8f,-3.9f,114.4f); Native.GeneralPoint = new(-166.1f,-4,109.3f); Native.StreetPoint = new(-153,-4,109.3f); break;
                    case "docks": destination.Goal = new(-80,-2,-50); Native.GeneralPoint = new(-85.9f,-2.2f,-56.3f); Native.StreetPoint = new(-96,-2,-52); break;
                }
                bool success; TaxiDestinations.Arrival arrival = new(); Vector3 point = default; string lot = "";
                if (scenario == "retained") {
                    SpikeCommands.ToDestination(destination, "first pick");
                    SpikeState.NavTarget = fixture == "nonfinite" ? new(float.NaN,0,0) : new(-50,0,0);
                    SpikeState.RideDestination = SpikeState.ActiveTripPoint = SpikeState.RideStreetCandidate = SpikeState.NavTarget;
                    Vector3 goal = fixture == "boundary" ? new(10,0,0) : fixture == "vertical-outside" ? new(10,1,0) : new(65,0,0);
                    success = SpikeCommands.ToDestination(new() { Name = "B", Kind = "property", Goal = goal }, "shared resolver");
                    point = SpikeState.NavTarget;
                }
                else if (scenario == "reuse") {
                    SpikeState.RideDestinationName = "A"; SpikeState.RideGoal = SpikeState.RideResolvedArrival = new(110,0,0);
                    SpikeState.RideDestination = SpikeState.ActiveTripPoint = new(100,0,0);
                    SpikeState.RideStreetCandidate = fixture == "fallback" ? new(100,0,0) : null;
                    SpikeState.ResetRide(); SpikeState.RideSerial++; SpikeState.RideActive = true;
                    Native.GeneralPoint = new(105,0,0);
                    success = SpikeCommands.Drive(new(100,0,0)); point = SpikeState.NavTarget;
                }
                else if (scenario == "provenance") {
                    SpikeState.RideStreetCandidate = SpikeState.RideDestination;
                    if (fixture == "picker-reset") SpikeState.ResetPicker();
                    else if (fixture == "picker-clear") SpikeState.ClearDestination();
                    else { Native.Patrol = true; SpikeState.DriverNpc = new(); SpikeCommands.ToDestination(destination, "patrol"); }
                    success = true; point = SpikeState.NavTarget;
                }
                else if (scenario.StartsWith("initial-") || scenario.StartsWith("capture-")) {
                    string[] parts = fixture.Split('|'); Native.PrepBoundary = parts[0]; Native.PrepAction = parts[1];
                    SpikeState.Vehicle.isParked = SpikeState.Vehicle.BrakesApplied = SpikeState.Vehicle.HandbrakeApplied = true;
                    if (Native.PrepBoundary == "simulation") SpikeState.Vehicle.IsPhysicallySimulated = false;
                    if (Native.PrepBoundary == "enable") SpikeState.Vehicle.Agent.enabled = false;
                    if (Native.PrepBoundary == "patrol") SpikeState.DriverNpc = new();
                    bool capture = scenario.StartsWith("capture-");
                    bool root = scenario.StartsWith("initial-root") || scenario == "capture-root";
                    bool auto = scenario == "initial-auto" || scenario.StartsWith("capture-auto");
                    Native.FailCalculation = true; Native.ThrowFirst = scenario == "initial-throw" || Native.PrepBoundary.StartsWith("fallback-");
                    Native.ThrowAll = scenario == "initial-root-failure";
                    if (root) {
                        SpikeState.RideActive = false; SpikeState.DriverNpc = new();
                        if (capture && parts[1].EndsWith("after-pickup")) Native.StreetPoint = new(50,0,0);
                        SpikeCommands.ToDestination(destination, "pick before boarding");
                    }
                    if (auto) {
                        SpikeState.RideActive = false; SpikeState.AutoRunning = SpikeState.AutoToPlayer = true;
                        SpikeState.AutoStep = 3; SpikeState.AutoNextAt = 0;
                    }
                    if (capture) {
                        string fault = parts.Length > 2 ? parts[2] : parts[1];
                        if (fault.EndsWith("after-pickup")) {
                            fault = fault[..^13];
                            SpikeState.PollingActive = false; SpikeState.NavTarget = Vector3.zero; SpikeState.ActiveTripPoint = null;
                            SpikeState.Operation.Terminate("prior pickup arrived", false);
                        }
                        Native.CaptureFault = fault;
                        if (fault is "missing" or "fallback-unreadable") SpikeState.Vehicle.Agent = null;
                        if (fault == "vehicle-missing") SpikeState.Vehicle = null;
                        Native.AgentReads = 0;
                    }
                    Native.PrepArmed = true;
                    if (auto) { for (int tick = 0; tick < (capture ? 4 : 1); tick++) SpikeRunner.RunAuto(); success = !Native.Interrupted && !SpikeState.AutoRunning && Native.NavigateCalls > 0; }
                    else success = root ? SpikeCommands.StartRide("root boarding") : scenario == "initial-go" ? SpikeCommands.Go(0, new(10,0,0), false) : SpikeCommands.ToDestination(destination, "initial preparation");
                    if (scenario == "capture-auto-retry") {
                        Native.CaptureFault = ""; SpikeState.Vehicle.Agent = new();
                        SpikeState.ResetAutoRun(); SpikeState.AutoRunning = SpikeState.AutoToPlayer = true; SpikeState.AutoStep = 3;
                        SpikeRunner.RunAuto();
                    }
                    point = SpikeState.NavTarget;
                }
                else if (scenario == "stale-completed") {
                    SpikeState.RideDestinationName = "A";
                    SpikeState.RideGoal = new(110,0,0); SpikeState.RideDestination = SpikeState.RideResolvedArrival = SpikeState.RideStreetCandidate = new(100,0,0);
                    SpikeState.Vehicle.transform.position = Native.GeneralPoint = new(100,0,0);
                    SpikeState.CompletedDestinationName = "A"; SpikeState.CompletedDestinationKind = "property";
                    SpikeState.CompletedGoal = new(110,0,0); SpikeState.CompletedArrival = new(100,0,0); SpikeState.NavCallbackResult = "Complete";
                    SpikeState.CompletedNavOrder = SpikeState.NavOrder; SpikeState.CompletedOperation = SpikeState.OperationId;
                    if (fixture != "inactive") SpikeState.Operation.Terminate("prior arrival", false);
                    if (fixture == "order-changed") SpikeState.NextNavOrder();
                    if (fixture == "operation-changed") SpikeState.OperationId++;
                    if (fixture == "missing-completion-scope") SpikeState.CompletedNavOrder = -1;
                    if (fixture == "authority") Native.Authority = false;
                    if (fixture == "foreign") Native.Owned = false;
                    if (fixture == "player") Native.PlayerDriver = true;
                    if (fixture == "teardown") Mod.Lifecycle.AcceptingWork = false;
                    success = SpikeCommands.Drive(new(100,0,0)); point = SpikeState.NavTarget;
                }
                else if (scenario == "resolve") { success = TaxiDestinations.ResolveArrival(destination, out arrival); point = arrival.Point; }
                else if (scenario == "fallback") {
                    Native.Lots = new[] { new ParkingLot { name = "first", EntryPoint = new() { position = new(15,0,0) } },
                        new ParkingLot { name = "second", EntryPoint = new() { position = new(30,0,0) } } };
                    Native.Project = p => p.x == 15 ? new(10,0,0) : new(25,0,0);
                    if (fixture == "rank") Native.Project = p => p.x == 15 ? new(50,0,0) : new(20,0,0);
                    if (fixture == "raw-far") { Native.Lots[1].EntryPoint.position = new(100,0,0); Native.Project = p => p.x == 15 ? new(70,0,0) : new(55,0,0); }
                    if (fixture == "projected-far") Native.Project = p => new(70,0,0);
                    if (fixture == "vertical-far") Native.Project = p => new(p.x,60,0);
                    if (fixture == "all-collapse") Native.Project = p => new(10,0,0);
                    if (fixture == "no-entry") { foreach (var l in Native.Lots) l.EntryPoint = null; }
                    if (fixture == "fallback-boundary") { Native.Lots = Native.Lots.Take(1).ToArray(); Native.Project = p => new(60,0,0); }
                    success = TaxiDestinations.TryFallbackDropOff(destination.Goal, new(10,0,0), out point, out lot);
                }
                else if (scenario == "pickup") { success = SpikeCommands.Go(0, new(10,0,0), true); point = SpikeState.NavTarget; }
                else if (scenario == "completed") {
                    SpikeState.Vehicle.transform.position = new(100,0,0); Native.GeneralPoint = Native.StreetPoint = new(100,0,0);
                    SpikeState.RideGoal = new(110,0,0); SpikeState.RideDestinationName = "A"; SpikeState.RideDestination = SpikeState.RideResolvedArrival = new(100,0,0);
                    SpikeState.RideStreetCandidate = new(100,0,0); SpikeState.Operation.Terminate("prior arrival", false);
                    SpikeState.CompletedDestinationName = "A"; SpikeState.CompletedDestinationKind = "property";
                    SpikeState.CompletedGoal = new(110,0,0); SpikeState.CompletedArrival = new(100,0,0); SpikeState.NavCallbackResult = "Complete";
                    SpikeState.CompletedNavOrder = SpikeState.NavOrder; SpikeState.CompletedOperation = SpikeState.OperationId;
                    if (fixture == "completed-physical-as-goal") SpikeState.CompletedGoal = new(100,0,0);
                    if (fixture == "completed-other-arrival") SpikeState.CompletedArrival = new(99,0,0);
                    if (fixture == "completed-no-arrival") SpikeState.CompletedArrival = null;
                    if (fixture == "completed-no-goal") { SpikeState.RideGoal = null; SpikeState.CompletedGoal = new(100,0,0); }
                    if (fixture == "completed-moving") SpikeState.Vehicle.Speed_Kmh = 3;
                    if (fixture == "completed-reversing") SpikeState.Vehicle.IsReversing = true;
                    if (fixture == "completed-no-complete") SpikeState.NavCallbackResult = null;
                    success = SpikeCommands.Drive(new(100,0,0)); point = SpikeState.NavTarget;
                }
                else {
                    Native.FailCalculation = scenario == "select-failed";
                    if (scenario == "interrupt") Native.Interruption = fixture;
                    if (scenario == "patrol") { Native.Patrol = true; SpikeState.DriverNpc = new(); }
                    if (scenario == "invalid-stand") success = SpikeCommands.SetRideDestination("STAND", "test");
                    else if (scenario == "invalid-legacy") success = SpikeCommands.SetRideDestination("ROAD A", "test");
                    else success = SpikeCommands.ToDestination(destination, "test", scenario == "invalid-direct-override" ?
                        new TaxiDestinations.Arrival { Ok = true, Point = new(5,0,0), Kind = "untrusted override" } : null);
                    if (scenario == "shared") {
                        SpikeCommands.ToDestination(destination, "duplicate");
                        SpikeCommands.ToDestination(new() { Name = "B", Kind = "property", Goal = new(2,0,0) }, "shared point");
                    }
                    SpikeRunner.Drain(); point = SpikeState.RideResolvedArrival ?? default;
                }
                return JsonSerializer.Serialize(new {
                    Success = success, ArrivalOk = arrival.Ok, Point = SpikeCommands.Fmt(point), Rule = arrival.Kind, Reason = arrival.Reason,
                    Distance = float.IsFinite(arrival.SnapDelta) ? arrival.SnapDelta : -1,
                    Target = SpikeCommands.Fmt(SpikeState.NavTarget), Destination = SpikeCommands.Format(SpikeState.RideDestination), ActivePoint = SpikeCommands.Format(SpikeState.ActiveTripPoint),
                    Goal = SpikeCommands.Format(SpikeState.RideGoal), Resolved = SpikeCommands.Format(SpikeState.RideResolvedArrival), Name = SpikeState.RideDestinationName, ActiveName = SpikeState.ActiveTripName,
                    Lot = lot, StreetQueries = Native.StreetQueries, GeneralQueries = Native.GeneralQueries, Constraint = Native.Constraint,
                    NavigateCalls = Native.NavigateCalls, Settings = Native.Settings, FareStarts = FareMeter.Starts, Operations = SpikeRunner.Operations,
                    RecoveryPending = SpikeState.NavCallbackRecoveryPending, Arrived = SpikeState.RideArrived, Terminal = SpikeState.Operation.Terminal, Callback = SpikeState.NavCallbackResult,
                    StreetCandidate = SpikeCommands.Format(SpikeState.RideStreetCandidate), CoherentAtNavigate = Native.CoherentAtNavigate,
                    Interrupted = Native.Interrupted, WritesAfterInterruption = Native.WritesAfterInterruption, EndRideCalls = Native.EndRideCalls, AutoCleanups = Native.AutoCleanups,
                    AutoRunning = SpikeState.AutoRunning, AutoStep = SpikeState.AutoStep, RideActive = SpikeState.RideActive,
                    PassengerMode = SpikeState.RidePassengerMode, ServiceStatus = SpikeState.ServiceStatus, TrunkLocked = Native.TrunkLocked, AgentReads = Native.AgentReads
                });
            }
        }
        }
        """;
}

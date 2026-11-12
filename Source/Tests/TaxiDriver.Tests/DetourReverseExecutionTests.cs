using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Replays the production reverse start and completion with controlled native boundaries.</summary>
public class DetourReverseExecutionTests
{
    private static readonly Lazy<Type> Replay = new(Compile);

    [Theory]
    [InlineData("first", 1)]
    [InlineData("last-grant", 4)]
    [InlineData("slow-native-retreat", 4)]
    [InlineData("native-retreat-boundary", 4)]
    public void GrantedReverse_KeepsTheObservedBlockerForDetour(string scenario, int attempts)
    {
        var result = Run(scenario);

        Assert.Equal(3f, result.Duration);
        Assert.Equal(1, result.Starts);
        Assert.Equal(1, result.Stops);
        Assert.Equal(1, result.Observations);
        Assert.Equal(417, result.DetourBlocker);
        Assert.Equal(0, result.Redispatches);
        Assert.Equal(attempts, result.RecoveryAttempts);
        Assert.False(result.WitnessRetained);
    }

    [Theory]
    [InlineData("no-blocker")]
    [InlineData("still-moving")]
    [InlineData("forward-at-retreat-speed")]
    [InlineData("negative-speed-without-native-reverse")]
    [InlineData("fast-native-retreat")]
    [InlineData("unreadable-speed")]
    [InlineData("pickup")]
    [InlineData("no-path-guard")]
    [InlineData("different-goal")]
    public void UnprovenBlocker_PreservesOrdinaryReverse(string scenario)
    {
        var result = Run(scenario);

        Assert.Equal(1.5f, result.Duration);
        Assert.Equal(1, result.Starts);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(1, result.Redispatches);
    }

    [Fact]
    public void BlockedRear_GrantsNeitherReverseNorDetour()
    {
        var result = Run("blocked-rear");

        Assert.Equal(0, result.Observations);
        Assert.Equal(0, result.Starts);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(0, result.Redispatches);
        Assert.False(result.WitnessRetained);
    }

    [Theory]
    [InlineData("stop-during-start", 0)]
    [InlineData("reroute-during-stop", 1)]
    public void SupersededReverse_DoesNotDispatchOrRetainEvidence(string scenario, int stops)
    {
        var result = Run(scenario);

        Assert.Equal(1, result.Starts);
        Assert.Equal(stops, result.Stops);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(0, result.Redispatches);
        Assert.False(result.WitnessRetained);
    }

    [Theory]
    [InlineData("order-during-start", 0, 0)]
    [InlineData("order-during-stop", 1, 0)]
    [InlineData("order-during-rescue", 1, 1)]
    public void SameOperationOrderReplacement_CannotInheritTheOldDetourOrLoseItsReverse(
        string scenario, int stops, int rescues)
    {
        var result = Run(scenario);

        Assert.Equal(1, result.Starts);
        Assert.Equal(stops, result.Stops);
        Assert.Equal(rescues, result.Rescues);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(0, result.Redispatches);
        Assert.False(result.WitnessRetained);
        Assert.True(result.ReplacementReverseCurrent);
    }

    [Fact]
    public void BoundedRetry_PreservesEvidenceAfterSensorNoLongerSeesTheBlocker()
    {
        var result = Run("retry");

        Assert.Equal(2, result.Starts);
        Assert.Equal(2, result.Stops);
        Assert.Equal(1, result.Observations);
        Assert.Equal(417, result.DetourBlocker);
        Assert.Equal(0, result.Redispatches);
        Assert.Equal(1, result.RecoveryAttempts);
    }

    [Fact]
    public void CheckedRescue_PreservesEvidenceAfterIneffectiveReverse()
    {
        var result = Run("checked-rescue");

        Assert.Equal(1, result.Starts);
        Assert.Equal(1, result.Stops);
        Assert.Equal(1, result.Rescues);
        Assert.Equal(417, result.DetourBlocker);
        Assert.Equal(0, result.Redispatches);
    }

    [Fact]
    public void RearClearanceEndingAfterConfirmedRetreat_StopsAndChecksTheExactDetour()
    {
        var result = RunRear("confirmed");

        Assert.Equal(1, result.Starts);
        Assert.Equal(1, result.Stops);
        Assert.Equal(0, result.Revocations);
        Assert.Equal(417, result.DetourBlocker);
        Assert.Equal(0, result.Rescues);
        Assert.Equal(0, result.Redispatches);
        Assert.Equal(3, result.Attempts);
        Assert.False(result.ReverseCurrent);
    }

    [Theory]
    [InlineData("missing-witness")]
    [InlineData("wrong-order-witness")]
    [InlineData("wrong-operation-witness")]
    [InlineData("no-directed-motion")]
    [InlineData("unreadable-speed")]
    [InlineData("retraced")]
    [InlineData("blocked-front")]
    [InlineData("throwing-front-probe")]
    public void RearClearanceEndingWithoutExactEscapeProof_RevokesWithoutDispatch(string scenario)
    {
        var result = RunRear(scenario);

        Assert.Equal(1, result.Starts);
        Assert.Equal(0, result.Stops);
        Assert.Equal(1, result.Revocations);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(0, result.Rescues);
        Assert.Equal(0, result.Redispatches);
        Assert.Equal(3, result.Attempts);
        Assert.False(result.ReverseCurrent);
    }

    [Theory]
    [InlineData("order-during-rear-probe")]
    [InlineData("order-during-front-probe")]
    [InlineData("order-during-handoff-front-probe")]
    [InlineData("reroute-during-handoff-front-probe")]
    [InlineData("reroute-and-throw-during-handoff-front-probe")]
    public void RearSupervision_LeavesReplacementOrderUntouchedDuringNativeReads(string scenario)
    {
        var result = RunRear(scenario);

        Assert.Equal(1, result.Starts);
        Assert.Equal(0, result.Stops);
        Assert.Equal(0, result.Revocations);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(0, result.Rescues);
        Assert.Equal(0, result.Redispatches);
        Assert.Equal(3, result.Attempts);
        Assert.True(result.ReverseCurrent);
    }

    [Fact]
    public void CheckedStop_RequiresTheSameWitnessAfterTheSecondFrontProbe()
    {
        var result = RunRear("witness-lost-during-handoff-front-probe");

        Assert.Equal(0, result.Stops);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(0, result.Redispatches);
        Assert.True(result.ReverseCurrent);
    }

    [Fact]
    public void CheckedStop_DoesNotRetryReverseWhenFrontClosesAfterHandoff()
    {
        var result = RunRear("front-closes-after-handoff");

        Assert.Equal(1, result.Starts);
        Assert.Equal(1, result.Stops);
        Assert.Equal(0, result.DetourBlocker);
        Assert.Equal(1, result.Rescues);
        Assert.Equal(0, result.Redispatches);
        Assert.Equal(3, result.Attempts);
    }

    [Theory]
    [InlineData("refused", false)]
    [InlineData("blocked-first", true)]
    public void FirstRefusedVia_UsesOneNewOrderAndWaitsForHaltAgain(string scenario, bool preferDirectEligible)
    {
        var result = RunVia(scenario);

        Assert.True(result.SelectedAlternate);
        Assert.True(result.WaitingForHalt);
        Assert.Equal(1, result.OrderAdvance);
        Assert.True(result.DestinationPreserved);
        Assert.False(result.AlternateRemaining);
        Assert.Equal(preferDirectEligible, result.CheckingBlockedAlternate);
        Assert.Equal(1, result.Dispatches);
        Assert.Equal(1, result.Terminals);
    }

    [Theory]
    [InlineData("calculating", 0)]
    [InlineData("approved", 1)]
    [InlineData("final-leg", 1)]
    [InlineData("unhealthy", 1)]
    [InlineData("leg-expired", 1)]
    [InlineData("operation-expired", 0)]
    [InlineData("stale-order", 0)]
    public void AlternateVia_RequiresAnOwnedFailedFirstCalculation(string scenario, int terminals)
    {
        var result = RunVia(scenario);

        Assert.False(result.SelectedAlternate);
        Assert.Equal(0, result.OrderAdvance);
        Assert.Equal(0, result.Dispatches);
        Assert.Equal(terminals, result.Terminals);
        if (scenario == "approved") Assert.False(result.AlternateRemaining);
    }

    [Theory]
    [InlineData("approved-final-expired", false, 0)]
    [InlineData("approved-final-paused", true, 0)]
    [InlineData("waiting-final-expired", true, 1)]
    [InlineData("unapproved-final-expired", true, 1)]
    public void FinalDetour_DelegatesAnApprovedLegToOrdinaryArrivalBeforeCheckingTheDeadline(
        string scenario, bool ownsTick, int expiryChecks)
    {
        var result = RunVia(scenario);

        Assert.Equal(ownsTick, result.OwnsTick);
        Assert.Equal(expiryChecks, result.ExpiryChecks);
        Assert.Equal(0, result.Dispatches);
    }

    [Theory]
    [InlineData("no-via-last-grant", false, 0)]
    [InlineData("no-via-crosses-blocker", true, 1)]
    [InlineData("no-via-wrong-endpoint", true, 1)]
    public void ConfirmedRetreatWithoutVia_ChecksTheRegularPathAfterHaltWithinTheSameGrant(
        string scenario, bool pathRejected, int terminals)
    {
        var result = RunNoVia(scenario);

        Assert.Equal(4, result.Attempts);
        Assert.True(result.OwnsDirectCheck);
        Assert.True(result.WaitedForHalt);
        Assert.Equal(1, result.ReverseStopParks);
        Assert.Equal(1, result.DetourParks);
        Assert.Equal(1, result.Dispatches);
        Assert.Equal(0, result.Redispatches);
        Assert.Equal(pathRejected, result.PathRejected);
        Assert.Equal(terminals, result.Terminals);
    }

    [Theory]
    [InlineData("no-via-unreadable-halt", true, 1, 1, false)]
    [InlineData("no-via-after-placement", false, 0, 1, false)]
    [InlineData("no-via-stale-order", false, 0, 0, true)]
    public void MissingVia_CannotAuthorizeUncheckedMovement(
        string scenario, bool ownsDirectCheck, int detourParks, int terminals, bool replacementReverse)
    {
        var result = RunNoVia(scenario);

        Assert.Equal(4, result.Attempts);
        Assert.Equal(ownsDirectCheck, result.OwnsDirectCheck);
        Assert.Equal(1, result.ReverseStopParks);
        Assert.Equal(detourParks, result.DetourParks);
        Assert.Equal(terminals, result.Terminals);
        Assert.Equal(replacementReverse, result.ReplacementReverse);
        Assert.Equal(0, result.Dispatches);
        Assert.Equal(0, result.Redispatches);
    }

    [Theory]
    [InlineData("fallback-first", 4, true, false)]
    [InlineData("fallback-second", 5, true, false)]
    [InlineData("fallback-none", 4, false, false)]
    [InlineData("fallback-stale-third", 3, false, true)]
    [InlineData("fallback-stale-fourth", 4, false, true)]
    public void FallbackSearch_IsSymmetricBoundedAndStopsWhenOrderChanges(
        string scenario, int probes, bool hasVia, bool stale)
    {
        var result = RunFallbackSearch(scenario);

        Assert.Equal(probes, result.Probes);
        Assert.Equal(hasVia, result.HasVia);
        Assert.Equal(stale, result.ReplacementReverse);
        Assert.Equal(0, result.Dispatches);
        Assert.Equal(0, result.Terminals);
        Assert.True(result.GeometryCorrect);
        Assert.Equal(!stale, result.OwnsDetour);
    }

    [Theory]
    [InlineData("clear", true)]
    [InlineData("first-via", false)]
    [InlineData("final-leg", false)]
    [InlineData("already-approved", false)]
    [InlineData("wrong-target", false)]
    [InlineData("wrong-callback", false)]
    [InlineData("stale-order", false)]
    [InlineData("wrong-endpoint", false)]
    [InlineData("many-points", false)]
    [InlineData("interior-crosses", false)]
    [InlineData("approach-crosses", false)]
    public void DirectCandidate_NeedsTheExactAlternateAndContinuousLocalClearance(string scenario, bool eligible)
    {
        var result = RunDirectCandidate(scenario);
        Assert.Equal(eligible, result.Preferred);
    }

    [Theory]
    [InlineData("unsafe-smoothing")]
    [InlineData("smoothing-wrong-endpoint")]
    public void DirectPreference_DoesNotAuthorizeAnUnsafeSmoothedPath(string scenario)
    {
        var result = RunDirectCandidate(scenario);

        Assert.True(result.Preferred);
        Assert.True(result.SmoothedPathRejected);
    }

    [Theory]
    [InlineData("clear", false)]
    [InlineData("via-boundary", false)]
    [InlineData("crosses-original", true)]
    [InlineData("crosses-final", true)]
    [InlineData("direct-clear", false)]
    [InlineData("direct-outside-via", false)]
    [InlineData("direct-crosses-original", true)]
    [InlineData("direct-crosses-final", true)]
    public void FinalLegRescue_UsesLastGrantAndChecksBothBlockerBounds(string scenario, bool rejected)
    {
        var result = RunFinalRescue(scenario);

        Assert.True(result.Handled);
        Assert.Equal(4, result.Attempts);
        Assert.Equal(1, result.Parks);
        Assert.Equal(1, result.Rescues);
        Assert.Equal(1, result.PlacementsConsumed);
        Assert.Equal(1, result.Stops);
        Assert.Equal(1, result.Dispatches);
        Assert.Equal(0, result.Terminals);
        Assert.True(result.DestinationPreserved);
        Assert.True(result.WaitedForHalt);
        Assert.Equal(rejected, result.PathRejected);
    }

    [Theory]
    [InlineData("stale-order", false, 3, 0)]
    [InlineData("order-during-blocker-read", true, 3, 0)]
    [InlineData("unreadable-speed", true, 3, 1)]
    [InlineData("still-moving", true, 3, 1)]
    [InlineData("native-reversing", true, 3, 1)]
    [InlineData("outside-via", true, 3, 1)]
    [InlineData("unreadable-via-distance", true, 3, 1)]
    [InlineData("unreadable-blocker", true, 3, 1)]
    [InlineData("same-blocker", true, 3, 1)]
    [InlineData("unhealthy", true, 3, 1)]
    [InlineData("unapproved", true, 3, 1)]
    [InlineData("used", true, 3, 1)]
    [InlineData("exhausted", true, 4, 1)]
    [InlineData("expired", true, 3, 1)]
    [InlineData("direct-stale-order", false, 3, 0)]
    [InlineData("direct-order-during-blocker-read", true, 3, 0)]
    [InlineData("direct-unreadable-speed", true, 3, 1)]
    [InlineData("direct-still-moving", true, 3, 1)]
    [InlineData("direct-native-reversing", true, 3, 1)]
    [InlineData("direct-unreadable-blocker", true, 3, 1)]
    [InlineData("direct-same-blocker", true, 3, 1)]
    [InlineData("direct-unhealthy", true, 3, 1)]
    [InlineData("direct-unapproved", true, 3, 1)]
    [InlineData("direct-used", true, 3, 1)]
    [InlineData("direct-exhausted", true, 4, 1)]
    [InlineData("direct-expired", true, 3, 1)]
    public void FinalLegRescue_InvalidInputsNeverSpendOrActuate(string scenario, bool handled, int attempts, int terminals)
    {
        var result = RunFinalRescue(scenario);

        Assert.Equal(handled, result.Handled);
        Assert.Equal(attempts, result.Attempts);
        Assert.Equal(0, result.Parks);
        Assert.Equal(0, result.Rescues);
        Assert.Equal(0, result.PlacementsConsumed);
        Assert.Equal(0, result.Stops);
        Assert.Equal(0, result.Dispatches);
        Assert.Equal(terminals, result.Terminals);
        Assert.False(result.WaitedForHalt);
    }

    [Theory]
    [InlineData("no-free-spot", 1)]
    [InlineData("only-spots-inside-clearance", 1)]
    [InlineData("order-during-rescue", 0)]
    [InlineData("order-during-placement-return", 0)]
    [InlineData("direct-no-free-spot", 1)]
    [InlineData("direct-only-spots-inside-clearance", 1)]
    [InlineData("direct-order-during-rescue", 0)]
    [InlineData("direct-order-during-placement-return", 0)]
    public void FinalLegRescue_RefusedOrSupersededPlacementNeverDispatches(string scenario, int terminals)
    {
        var result = RunFinalRescue(scenario);

        Assert.True(result.Handled);
        Assert.Equal(4, result.Attempts);
        Assert.Equal(1, result.Parks);
        Assert.Equal(1, result.Rescues);
        Assert.Equal(0, result.PlacementsConsumed);
        Assert.Equal(0, result.Stops);
        Assert.Equal(0, result.Dispatches);
        Assert.Equal(terminals, result.Terminals);
    }

    private static (float Duration, int Starts, int Stops, int Observations, int DetourBlocker,
        int Redispatches, int RecoveryAttempts, int Rescues, bool WitnessRetained, bool ReplacementReverseCurrent)
        Run(string scenario) =>
        ((float, int, int, int, int, int, int, int, bool, bool))Replay.Value.GetMethod("Run")!
            .Invoke(null, new object[] { scenario })!;

    private static (bool SelectedAlternate, bool WaitingForHalt, int OrderAdvance, bool DestinationPreserved,
        bool AlternateRemaining, bool CheckingBlockedAlternate, int Dispatches, int Terminals, bool OwnsTick,
        int ExpiryChecks) RunVia(string scenario) =>
        ((bool, bool, int, bool, bool, bool, int, int, bool, int))Replay.Value.GetMethod("RunVia")!
            .Invoke(null, new object[] { scenario })!;

    private static (int Attempts, int ReverseStopParks, int DetourParks, int Dispatches, int Terminals, int Redispatches,
        bool OwnsDirectCheck, bool WaitedForHalt, bool PathRejected, bool ReplacementReverse) RunNoVia(string scenario) =>
        ((int, int, int, int, int, int, bool, bool, bool, bool))Replay.Value.GetMethod("RunNoVia")!
            .Invoke(null, new object[] { scenario })!;

    private static (bool Preferred, bool SmoothedPathRejected) RunDirectCandidate(string scenario) =>
        ((bool, bool))Replay.Value.GetMethod("RunDirectCandidate")!.Invoke(null, new object[] { scenario })!;

    private static (int Probes, bool HasVia, bool ReplacementReverse, int Dispatches, int Terminals,
        bool GeometryCorrect, bool OwnsDetour) RunFallbackSearch(string scenario) =>
        ((int, bool, bool, int, int, bool, bool))Replay.Value.GetMethod("RunFallbackSearch")!
            .Invoke(null, new object[] { scenario })!;

    private static (int Starts, int Stops, int Revocations, int DetourBlocker, int Rescues, int Redispatches,
        int Attempts, bool ReverseCurrent) RunRear(string scenario) =>
        ((int, int, int, int, int, int, int, bool))Replay.Value.GetMethod("RunRear")!
            .Invoke(null, new object[] { scenario })!;

    private static (bool Handled, int Attempts, int Parks, int Rescues, int Stops, int Dispatches, int Terminals,
        bool DestinationPreserved, bool WaitedForHalt, bool PathRejected, int PlacementsConsumed) RunFinalRescue(string scenario) =>
        ((bool, int, int, int, int, int, int, bool, bool, bool, int))Replay.Value.GetMethod("RunFinalRescue")!
            .Invoke(null, new object[] { scenario })!;

    private static Type Compile()
    {
        var runner = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs")).GetRoot();
        string[] methodNames = { "TryStartReverse", "TickReDispatch", "TakeDetourBlockerForReverse", "ObserveDetourBlocker", "TryStartDetour",
            "CanTryDetour", "SamePoint", "TryReadSignedSpeed", "ToNumerics", "TryReadReversing", "TickDetour",
            "TryUseAlternateVia", "DetourOrderCurrent", "DetourOwnsVehicle", "DetourCallbackCurrent", "RejectDetourPath",
            "CanPreferDirectDetourVia", "DetourPathClears", "BlockerClearance", "TickReverseSupervision",
            "CancelFinalDetourOnStall", "TryFreeSpotRescue" };
        string methods = string.Join("\n", runner.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(method => methodNames.Contains(method.Identifier.Text)).Select(method => method.ToFullString()))
            .Replace("il2cpp::Il2CppSystem", "Il2CppSystem", StringComparison.Ordinal);
        string blocker = runner.DescendantNodes().OfType<RecordDeclarationSyntax>()
            .Single(record => record.Identifier.Text == "StaticBlocker").ToFullString();
        string detour = runner.DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Single(type => type.Identifier.Text == "DetourDrive").ToFullString();
        string witnessLimit = runner.DescendantNodes().OfType<FieldDeclarationSyntax>()
            .Single(field => field.Declaration.Variables.Any(v => v.Identifier.Text == "DetourNativeReverseWitnessKmh"))
            .ToFullString();

        var policies = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs")).GetRoot();
        string[] policyNames = { "NavigationRecoveryBudget", "NavigationArrivalPolicy", "ReverseManoeuvrePolicy",
            "ReversePhase", "ReverseSupervisionPolicy", "ReverseStopTransitionPolicy", "ReverseStopTransitionResult",
            "DetourClearancePolicy", "RoadProjectionPolicy" };
        string policyCode = string.Join("\n", policies.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
            .Where(policy => policyNames.Contains(policy.Identifier.Text)).Select(policy => policy.ToFullString()));
        string source = Harness.Replace("/* POLICIES */", policyCode).Replace("/* TYPES */", blocker + detour + witnessLimit)
            .Replace("/* METHODS */", methods);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("TaxiDetourReverseReplay",
            new[] { CSharpSyntaxTree.ParseText(source) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.SpikeRunner")!;
    }

    private const string Harness = """
        using System;
        using Replay.Policies;

        namespace Replay.Policies {
            using System.Numerics;
            /* POLICIES */
        }

        namespace Il2CppSystem.Collections.Generic {
            public sealed class List<T> : System.Collections.Generic.List<T> { }
        }
        namespace Il2CppPathfinding {
            public sealed class Path {
                public readonly Il2CppSystem.Collections.Generic.List<Replay.Vector3> vectorPath = new();
            }
        }

        namespace Replay {
        public record struct Vector3(float x, float y, float z) {
            public static Vector3 operator -(Vector3 v) => new(-v.x, -v.y, -v.z);
            public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
            public static Vector3 operator -(Vector3 a, Vector3 b) => a + (-b);
            public static Vector3 operator *(Vector3 v, float scalar) => new(v.x * scalar, v.y * scalar, v.z * scalar);
            public static float Distance(Vector3 a, Vector3 b) =>
                new System.Numerics.Vector3(a.x - b.x, a.y - b.y, a.z - b.z).Length();
            public static float Dot(Vector3 a, Vector3 b) => a.x * b.x + a.y * b.y + a.z * b.z;
            public override string ToString() => $"({x}, {y}, {z})";
            public Vector3 normalized {
                get {
                    float length = Distance(this, default);
                    return length > 0f ? this * (1f / length) : default;
                }
            }
        }
        public record struct Bounds(Vector3 min, Vector3 max) {
            public Vector3 center => (min + max) * .5f;
        }
        public static class PathSmoothingUtility {
            public sealed class SmoothedPath {
                public readonly Il2CppSystem.Collections.Generic.List<Vector3> vectorPath = new();
            }
        }
        public sealed class NativeReference { public IntPtr Pointer = new(3); }
        public sealed class Transform {
            public Vector3 position = new(71.1f, .6f, -3.8f);
            public Vector3 forward = new(1f, 0f, 0f);
            public object rotation;
        }
        public sealed class LandVehicle {
            public readonly Transform transform = new();
            public readonly VehicleAgent Agent = new();
            public IntPtr Pointer = new(1);
            public float Speed_Kmh;
            public bool IsReversing;
        }
        public sealed class VehicleAgent {
            public enum ENavigationResult { Complete, Failed }
            public IntPtr Pointer = new(2);
            public readonly NativeReference currentNavigationSettings = new(), storedNavigationCallback = new();
            public Vector3 TargetLocation;
            public bool NavigationCalculationInProgress;
            public int Starts, Stops, NavigationStops;
            public void StartReverse() { Starts++; SpikeRunner.Interrupt("start"); }
            public void StopReversing() { Stops++; SpikeRunner.Interrupt("stop"); }
            public void StopNavigating() { NavigationStops++; }
        }
        public static class Time { public static float unscaledTime, time, timeScale = 1f; }
        public sealed class Life { public bool AcceptingWork = true; }
        public sealed class Logger { public void Info(string text) { } public void Warn(string text) { } }
        public static class Mod { public static readonly Life Lifecycle = new(); public static readonly Logger Log = new(); }
        public sealed class Operation {
            public long Generation = 1;
            public bool Active = true;
            public bool Terminal => !Active;
            public void Terminate(string reason, bool failed) => Active = false;
        }
        public static class SpikeState {
            public static bool PollingActive, RideActive, NavToPlayer;
            public static readonly Operation Operation = new();
            public static long OperationId;
            public static int NavOrder;
            public static LandVehicle Vehicle;
            public static Vector3 NavTarget, NavReDispatchTarget;
            public static Vector3? RideResolvedArrival;
            public static float NavReDispatchAt, NavStartTime;
            public static int NextNavOrder() => ++NavOrder;
            internal static readonly ReverseManoeuvrePolicy Reverse = new();
            internal static readonly NavigationRecoveryBudget NavigationRecovery = new();
        }
        public static class TaxiAI {
            public const int PathingFailureCap = 4;
            public static bool NavigationBlocked;
        }
        public static class SpikeCommands {
            public static bool HasAuthority() => true;
            public static bool IsOwnedTaxi(LandVehicle v) => ReferenceEquals(v, SpikeState.Vehicle);
            public static bool EnsureDriverSeat(LandVehicle v) => false;
            public static bool DriverAtWheel(LandVehicle v) => true;
            public static string Fmt(Vector3 v) => v.ToString();
            public static void ReportCleanupFailure(string operation, Exception error) { }
        }
        public static class TaxiDestinations { public static string Num(float value) => value.ToString(); }
        public static class NativeShortcutPatch { public static bool CanValidateDetour; }
        public static class NativeActuatorPatch {
            public static bool Healthy = true;
            public static int Parks;
            public static void RequestPark(LandVehicle v) { Parks++; }
            public static void ReleasePark(LandVehicle v) { }
        }
        public static class RoadKeeper {
            public static bool HasStaticBlocker;
            public static int Observations, StaticBlockerId = 417;
            public static Bounds StaticBounds;
            public static bool TryReadStaticFrontBlocker(LandVehicle veh, VehicleAgent agent, out int id, out Bounds bounds) {
                Observations++;
                SpikeRunner.Interrupt("blocker-read");
                id = StaticBlockerId;
                bounds = StaticBounds;
                return HasStaticBlocker;
            }
            public static bool HasObstacle(LandVehicle vehicle, Vector3 position, object rotation) =>
                SpikeRunner.FrontProbeBlocked();
            public static bool TryRescueStartup(LandVehicle vehicle, VehicleAgent agent, string reason,
                Func<Vector3, bool> allowPlacement, Func<bool> revalidatePlacement) =>
                SpikeRunner.ProbeRescue(reason, allowPlacement, revalidatePlacement);
        }
        public static class RoadTarget {
            public static bool TryDetourPoint(Vector3 probe, out Vector3 point) => SpikeRunner.ProbeVia(probe, out point);
        }
        public static class SpikeRunner {
            /* TYPES */
            private const float ReverseSeconds = 1.5f, ReverseSecondsLong = 3f;
            private const float ArrivalThresholdMeters = 10f, DetourLegTimeoutSeconds = 25f;
            private const float DetourRearOffset = 20f, DetourSideOffset = 16f, DetourFallbackSideOffset = 12f, DetourAlternateAdvance = 5f;
            private const int MaxFreeSpotRescuesPerRide = 2;
            private static Vector3 _reverseOrigin, _reverseForward, _operationProgressTarget;
            private static int _reverseAttempt, _freeSpotRescues;
            private static float _reverseStartedAt, _reverseRearCheckAt;
            private static bool _recoveryOwnsNavigation;
            private static long _detourUsedOperation;
            private static (StaticBlocker Blocker, long Operation, int Order)? _detourReverse;
            private static DetourDrive _detour;
            private static string scenario;
            private static int redispatches, rescues, viaDispatches, viaTerminals, revocations, frontReads, expiryChecks;
            private static int parksAtViaProbe;
            private static readonly System.Collections.Generic.List<Vector3> viaProbes = new();
            private static bool rearBlocked;
            private static bool PlayerDriver(LandVehicle v) => false;
            private static bool MainSceneLoaded() => true;
            private static float ForwardDot(LandVehicle v) => 1f;
            private static bool RearIsClear(LandVehicle v) {
                if (rearBlocked) Interrupt("rear-probe");
                if (scenario == "front-closes-after-handoff" && frontReads > 1) return true;
                return scenario != "blocked-rear" && !rearBlocked;
            }
            public static bool FrontProbeBlocked() {
                frontReads++;
                Interrupt(frontReads == 1 ? "front-probe" : "handoff-front-probe");
                if (scenario == "throwing-front-probe" || (frontReads == 2 && scenario == "reroute-and-throw-during-handoff-front-probe"))
                    throw new InvalidOperationException("unreadable front probe");
                return scenario == "blocked-front" || (scenario == "front-closes-after-handoff" && frontReads > 1);
            }
            private static string DescribeDriveState(LandVehicle v, VehicleAgent a) => "";
            private static void RevokeReverse(string why) {
                revocations++;
                SpikeState.Reverse.End();
                SpikeState.NavReDispatchAt = 0f;
                _detourReverse = null;
                SpikeState.NextNavOrder();
            }
            public static bool ProbeVia(Vector3 probe, out Vector3 point) {
                if (parksAtViaProbe < 0) parksAtViaProbe = NativeActuatorPatch.Parks;
                viaProbes.Add(probe);
                Interrupt("via-probe");
                if ((scenario == "fallback-stale-third" && viaProbes.Count == 3) ||
                    (scenario == "fallback-stale-fourth" && viaProbes.Count == 4)) {
                    int order = SpikeState.NextNavOrder();
                    SpikeState.Reverse.Begin(SpikeState.OperationId, order, ToNumerics(SpikeState.Vehicle.transform.position),
                        new System.Numerics.Vector3(-1f, 0f, 0f), Time.unscaledTime, 3f);
                }
                bool available = scenario.StartsWith("fallback-", StringComparison.Ordinal)
                    ? (scenario == "fallback-first" && viaProbes.Count >= 3) ||
                      (scenario == "fallback-second" && viaProbes.Count >= 4)
                    : !scenario.StartsWith("no-via-", StringComparison.Ordinal);
                point = available ? probe : default;
                return available;
            }
            public static bool ProbeRescue(string reason, Func<Vector3, bool> allowPlacement, Func<bool> revalidatePlacement) {
                rescues++;
                Interrupt("rescue");
                if (reason != "final detour departure")
                    return scenario is "checked-rescue" or "order-during-rescue" or "no-via-after-placement";
                if (allowPlacement == null || revalidatePlacement == null)
                    throw new InvalidOperationException("missing final-detour placement guards");
                if (!revalidatePlacement()) return false;

                // These lie outside the raw colliders but inside their padded bounds.
                var originalClearance = new Vector3(81f, .6f, -4f);
                var finalClearance = new Vector3(36f, .6f, -7f);
                if (allowPlacement(originalClearance) || allowPlacement(finalClearance) ||
                    allowPlacement(new Vector3(float.NaN, .6f, -7f)))
                    throw new InvalidOperationException("final-detour placement would intersect clearance");
                if (scenario is "no-free-spot" or "only-spots-inside-clearance") return false;
                if (!allowPlacement(new Vector3(70f, .6f, -4f)))
                    throw new InvalidOperationException("clear placement was refused");
                Interrupt("placement-return");
                return true;
            }
            private static void ReDispatch(VehicleAgent a, Vector3 target, string why) { redispatches++; }
            private static bool StopIfNavigationExpired(VehicleAgent a, LandVehicle v, float now) {
                expiryChecks++;
                return SpikeState.NavigationRecovery.HasExpired(now);
            }
            private static void EndDetour(DetourDrive drive, LandVehicle v, VehicleAgent a, string why) {
                if (!DetourOwnsVehicle(drive, v, a)) return;
                viaTerminals++;
                _detour = null;
                SpikeState.Operation.Terminate(why, failed: true);
            }
            private static void DispatchDetourLeg(DetourDrive drive, LandVehicle v, VehicleAgent a) {
                viaDispatches++;
                drive.Order = SpikeState.NextNavOrder();
                drive.Settings = a.currentNavigationSettings.Pointer;
                drive.Callback = a.storedNavigationCallback.Pointer;
                a.TargetLocation = drive.Target;
            }
            private static void GiveUpNavigation(VehicleAgent a, Vector3 p, float distance, float elapsed) {
                viaTerminals++;
                SpikeState.Operation.Terminate("no route", failed: true);
            }
            public static void Interrupt(string stage) {
                if (scenario == "stop-during-start" && stage == "start") {
                    SpikeState.PollingActive = false;
                    SpikeState.Operation.Active = false;
                    SpikeState.NextNavOrder();
                }
                if (scenario == "reroute-during-stop" && stage == "stop") {
                    SpikeState.Reverse.End();
                    SpikeState.OperationId++;
                    SpikeState.Operation.Generation++;
                    SpikeState.NextNavOrder();
                }
                bool reroutedDuringHandoff = stage == "handoff-front-probe" &&
                    scenario is "reroute-during-handoff-front-probe" or "reroute-and-throw-during-handoff-front-probe";
                if (reroutedDuringHandoff) {
                    SpikeState.OperationId++;
                    SpikeState.Operation.Generation++;
                }
                if (reroutedDuringHandoff || scenario == "order-during-" + stage ||
                    (scenario == "no-via-stale-order" && stage == "via-probe")) {
                    int nextOrder = SpikeState.NextNavOrder();
                    SpikeState.Reverse.Begin(SpikeState.OperationId, nextOrder, ToNumerics(SpikeState.Vehicle.transform.position),
                        new System.Numerics.Vector3(-1f, 0f, 0f), Time.unscaledTime, 3f);
                    if (_detourReverse is { } witness)
                        _detourReverse = (witness.Blocker, SpikeState.OperationId, nextOrder);
                }
                if (scenario == "witness-lost-during-handoff-front-probe" && stage == "handoff-front-probe")
                    _detourReverse = null;
            }
            private static void CompleteReverse(LandVehicle vehicle, bool moved) {
                float elapsedAt = SpikeState.NavReDispatchAt;
                if (moved) {
                    var backed = new Vector3(vehicle.transform.position.x - 5f, .6f, -3.8f);
                    SpikeState.Reverse.Observe(ToNumerics(backed), true, -6f, elapsedAt - 1f);
                    SpikeState.Reverse.Observe(ToNumerics(backed), true, -6f, elapsedAt - .5f);
                    vehicle.transform.position = backed;
                }
                Time.unscaledTime = elapsedAt;
                TickReDispatch(vehicle.Agent);
            }
            public static (float, int, int, int, int, int, int, int, bool, bool) Run(string test) {
                scenario = test;
                viaProbes.Clear();
                redispatches = rescues = RoadKeeper.Observations = _freeSpotRescues = 0;
                _detour = null;
                rearBlocked = false;
                RoadKeeper.HasStaticBlocker = test != "no-blocker";
                RoadKeeper.StaticBlockerId = 417;
                RoadKeeper.StaticBounds = new Bounds(new(83f, 0f, -6f), new(88f, 2f, -3f));
                NativeShortcutPatch.CanValidateDetour = test != "no-path-guard";
                NativeActuatorPatch.Healthy = true;
                SpikeState.NavToPlayer = test == "pickup";
                SpikeState.PollingActive = SpikeState.Operation.Active = SpikeState.RideActive = true;
                SpikeState.OperationId = 9;
                SpikeState.NavOrder = 57;
                SpikeState.Vehicle = new LandVehicle();
                _operationProgressTarget = new Vector3(-156f, -4f, 113.7f);
                SpikeState.RideResolvedArrival = test == "different-goal"
                    ? new Vector3(-156f, -4f, 82.3f) : _operationProgressTarget;
                SpikeState.NavTarget = SpikeState.RideResolvedArrival.Value;
                SpikeState.NavReDispatchAt = 0f;
                Time.timeScale = 1f;
                SpikeState.NavStartTime = Time.time = Time.unscaledTime = 100f;
                SpikeState.Reverse.End();
                SpikeState.NavigationRecovery.Begin(100f, 90f);
                int grantedAttempts = test is "last-grant" or "slow-native-retreat" or "native-retreat-boundary" ||
                    test.StartsWith("no-via-", StringComparison.Ordinal) ? 4 : 1;
                for (int i = 0; i < grantedAttempts; i++)
                    SpikeState.NavigationRecovery.TrySpend(100f, 4);
                _detourUsedOperation = -1;
                _detourReverse = null;
                _reverseAttempt = test is "last-grant" or "checked-rescue" or "order-during-rescue" or "no-via-after-placement" ? 1 : 0;
                LandVehicle vehicle = SpikeState.Vehicle;
                vehicle.Speed_Kmh = test switch {
                    "still-moving" => 1.6f,
                    "forward-at-retreat-speed" => 2.2f,
                    "slow-native-retreat" or "negative-speed-without-native-reverse" => -2.2f,
                    "native-retreat-boundary" => -3f,
                    "fast-native-retreat" => -4f,
                    "unreadable-speed" => float.NaN,
                    _ => .1f
                };
                vehicle.IsReversing = test is "slow-native-retreat" or "native-retreat-boundary" or
                    "forward-at-retreat-speed" or "fast-native-retreat" or "unreadable-speed";
                bool started = TryStartReverse(vehicle.Agent, vehicle, 100f, "recovery");
                float duration = SpikeState.NavReDispatchAt - 100f;
                if (started) {
                    RoadKeeper.HasStaticBlocker = false;
                    CompleteReverse(vehicle, test is not ("retry" or "checked-rescue" or "order-during-rescue" or "no-via-after-placement"));
                    if (test == "retry" && vehicle.Agent.Starts == 2) CompleteReverse(vehicle, true);
                }
                return (duration, vehicle.Agent.Starts, vehicle.Agent.Stops, RoadKeeper.Observations, _detour?.BlockerId ?? 0,
                    redispatches, SpikeState.NavigationRecovery.Attempts, rescues, _detourReverse.HasValue,
                    SpikeState.Reverse.IsCurrentFor(SpikeState.OperationId, SpikeState.NavOrder, Time.unscaledTime));
            }

            public static (int, bool, bool, int, int, bool, bool) RunFallbackSearch(string test) {
                NativeActuatorPatch.Parks = viaDispatches = viaTerminals = 0;
                parksAtViaProbe = -1;
                var result = Run(test);
                bool geometry = viaProbes.Count >= 3 &&
                    Vector3.Distance(viaProbes[0], viaProbes[1]) == 32f &&
                    Vector3.Distance(viaProbes[0], viaProbes[2]) == 4f &&
                    (viaProbes.Count < 4 || Vector3.Distance(viaProbes[2], viaProbes[3]) ==
                        (test == "fallback-first" ? 5f : 24f));
                return (viaProbes.Count, _detour?.HasVia == true, result.Item10,
                    viaDispatches, viaTerminals, geometry, _detour != null);
            }

            public static (int, int, int, int, int, int, bool, bool, bool, bool) RunNoVia(string test) {
                NativeActuatorPatch.Parks = viaDispatches = viaTerminals = 0;
                parksAtViaProbe = -1;
                var reverse = Run(test);
                int detourParks = NativeActuatorPatch.Parks - parksAtViaProbe;
                var vehicle = SpikeState.Vehicle;
                var agent = vehicle.Agent;
                var drive = _detour;
                bool ownsDirectCheck = drive is { HasVia: false, FinalLeg: true, WaitingForHalt: true } &&
                    drive.Target == _operationProgressTarget && !drive.AlternateVia.HasValue && drive.BlockerId == 417;
                bool waitedForHalt = false;
                bool pathRejected = false;
                if (drive != null) {
                    if (test == "no-via-unreadable-halt") {
                        vehicle.Speed_Kmh = float.NaN;
                        TickDetour();
                        Time.time += 5f;
                        TickDetour();
                    }
                    else {
                        vehicle.Speed_Kmh = .1f;
                        vehicle.IsReversing = true;
                        waitedForHalt = TickDetour() && drive.WaitingForHalt && viaDispatches == 0;
                        vehicle.IsReversing = false;
                        TickDetour();
                        var path = new PathSmoothingUtility.SmoothedPath();
                        path.vectorPath.Add(vehicle.transform.position);
                        if (test == "no-via-crosses-blocker") path.vectorPath.Add(new Vector3(91f, .6f, -3.8f));
                        path.vectorPath.Add(test == "no-via-wrong-endpoint"
                            ? _operationProgressTarget + new Vector3(20f, 0f, 0f) : _operationProgressTarget);
                        pathRejected = RejectDetourPath(agent, path);
                        if (TickDetour() != pathRejected)
                            throw new InvalidOperationException("approved direct path did not return to ordinary arrival");
                    }
                }
                return (SpikeState.NavigationRecovery.Attempts, parksAtViaProbe, detourParks, viaDispatches, viaTerminals,
                    redispatches, ownsDirectCheck, waitedForHalt, pathRejected, reverse.Item10);
            }

            public static (int, int, int, int, int, int, int, bool) RunRear(string test) {
                scenario = test;
                redispatches = rescues = revocations = frontReads = _freeSpotRescues = 0;
                _detour = null;
                rearBlocked = false;
                RoadKeeper.HasStaticBlocker = true;
                RoadKeeper.StaticBlockerId = 417;
                NativeActuatorPatch.Healthy = NativeShortcutPatch.CanValidateDetour = true;
                SpikeState.PollingActive = SpikeState.Operation.Active = SpikeState.RideActive = true;
                SpikeState.NavToPlayer = false;
                SpikeState.OperationId = 9;
                SpikeState.NavOrder = 57;
                var vehicle = SpikeState.Vehicle = new LandVehicle();
                _operationProgressTarget = new Vector3(-156f, -4f, 113.7f);
                SpikeState.NavTarget = _operationProgressTarget;
                SpikeState.RideResolvedArrival = _operationProgressTarget;
                Time.timeScale = 1f;
                Time.time = Time.unscaledTime = SpikeState.NavStartTime = 100f;
                SpikeState.NavReDispatchAt = 0f;
                SpikeState.Reverse.End();
                SpikeState.NavigationRecovery.Begin(100f, 90f);
                for (int i = 0; i < 3; i++) SpikeState.NavigationRecovery.TrySpend(100f, TaxiAI.PathingFailureCap);
                _detourUsedOperation = -1;
                _detourReverse = null;
                _reverseAttempt = 0;
                var origin = vehicle.transform.position;
                if (!TryStartReverse(vehicle.Agent, vehicle, 100f, "recovery"))
                    throw new InvalidOperationException("expected initial witnessed reverse");

                if (test == "missing-witness") _detourReverse = null;
                if (_detourReverse is { } pending) {
                    if (test == "wrong-order-witness")
                        _detourReverse = (pending.Blocker, pending.Operation, pending.Order - 1);
                    if (test == "wrong-operation-witness")
                        _detourReverse = (pending.Blocker, pending.Operation - 1, pending.Order);
                }

                vehicle.Speed_Kmh = test == "unreadable-speed" ? float.NaN : -6f;
                if (test != "no-directed-motion") vehicle.transform.position = origin - new Vector3(5f, 0f, 0f);
                Time.unscaledTime = 100.4f;
                TickReverseSupervision();
                Time.unscaledTime = 100.8f;
                TickReverseSupervision();

                if (test == "retraced") vehicle.transform.position = origin - new Vector3(.1f, 0f, 0f);
                rearBlocked = true;
                Time.unscaledTime = 101.2f;
                TickReverseSupervision();
                return (vehicle.Agent.Starts, vehicle.Agent.Stops, revocations, _detour?.BlockerId ?? 0, rescues, redispatches,
                    SpikeState.NavigationRecovery.Attempts,
                    SpikeState.Reverse.IsCurrentFor(SpikeState.OperationId, SpikeState.NavOrder, Time.unscaledTime));
            }

            public static (bool, bool, int, bool, bool, bool, int, int, bool, int) RunVia(string test) {
                scenario = test;
                viaDispatches = viaTerminals = expiryChecks = 0;
                NativeActuatorPatch.Healthy = test != "unhealthy";
                NativeShortcutPatch.CanValidateDetour = true;
                Time.timeScale = test == "approved-final-paused" ? 0f : 1f;
                Time.time = Time.unscaledTime = 100f;
                SpikeState.NavigationRecovery.Begin(90f, test.EndsWith("expired") && test != "leg-expired" ? 5f : 90f);
                SpikeState.PollingActive = SpikeState.Operation.Active = SpikeState.RideActive = true;
                SpikeState.NavToPlayer = false;
                SpikeState.OperationId = 2;
                SpikeState.NavOrder = 20;
                var vehicle = SpikeState.Vehicle = new LandVehicle();
                var agent = vehicle.Agent;
                var destination = new Vector3(-156f, -4f, 113.7f);
                var alternate = new Vector3(50f, 0f, -19f);
                var original = new Vector3(50f, 0f, 12f);
                var clearance = new Bounds(new(83f, 0f, -6f), new(88f, 2f, -3f));
                var drive = _detour = new DetourDrive(vehicle, agent, original, alternate, destination,
                    new StaticBlocker(417, vehicle.transform.position, vehicle.transform.forward, clearance)) {
                    Order = SpikeState.NavOrder, WaitingForHalt = test == "waiting-final-expired",
                    Started = test == "leg-expired" ? 74f : 99f,
                    FinalLeg = test == "final-leg" || test.Contains("-final-"),
                    PathApproved = test is "approved-final-expired" or "approved-final-paused" or "waiting-final-expired",
                    Settings = agent.currentNavigationSettings.Pointer, Callback = agent.storedNavigationCallback.Pointer,
                };
                agent.TargetLocation = drive.Target;
                agent.NavigationCalculationInProgress = test == "calculating";
                if (test == "stale-order") SpikeState.NextNavOrder();
                if (test is "approved" or "blocked-first") {
                    var path = new PathSmoothingUtility.SmoothedPath();
                    path.vectorPath.Add(vehicle.transform.position);
                    if (test == "blocked-first") path.vectorPath.Add(new Vector3(91f, .6f, -3.8f));
                    path.vectorPath.Add(original);
                    if (RejectDetourPath(agent, path) != (test == "blocked-first"))
                        throw new Exception("unexpected first via verdict");
                    drive.ViaResult = VehicleAgent.ENavigationResult.Failed;
                }
                int initialOrder = SpikeState.NavOrder;
                bool ownsTick = TickDetour();
                int orderAdvance = SpikeState.NavOrder - initialOrder;
                bool waiting = drive.WaitingForHalt;
                bool checkingBlockedAlternate = drive.CheckingAlternateVia;
                if (test is "refused" or "blocked-first") {
                    vehicle.Speed_Kmh = 1.6f;
                    TickDetour();
                    if (viaDispatches != 0) throw new Exception("retry dispatched before a readable halt");
                    vehicle.Speed_Kmh = .1f;
                    TickDetour();
                    TickDetour();
                }
                return (drive.Via == alternate, waiting, orderAdvance, drive.Destination == destination,
                    drive.AlternateVia.HasValue, checkingBlockedAlternate, viaDispatches, viaTerminals, ownsTick, expiryChecks);
            }

            public static (bool, bool) RunDirectCandidate(string test) {
                var vehicle = SpikeState.Vehicle = new LandVehicle();
                var agent = vehicle.Agent;
                SpikeState.PollingActive = SpikeState.Operation.Active = SpikeState.RideActive = true;
                SpikeState.NavToPlayer = false;
                SpikeState.OperationId = 3;
                SpikeState.NavOrder = 21;
                var via = new Vector3(50f, 0f, 12f);
                var destination = new Vector3(-156f, -4f, 113.7f);
                var clearance = test == "approach-crosses"
                    ? new Bounds(new(65f, 0f, -5f), new(67f, 2f, -3f))
                    : new Bounds(new(83f, 0f, -6f), new(88f, 2f, -3f));
                var drive = _detour = new DetourDrive(vehicle, agent, via, null, destination,
                    new StaticBlocker(417, vehicle.transform.position, vehicle.transform.forward, clearance)) {
                    Order = SpikeState.NavOrder, WaitingForHalt = false,
                    CheckingAlternateVia = test != "first-via", FinalLeg = test == "final-leg",
                    PathApproved = test == "already-approved", Settings = agent.currentNavigationSettings.Pointer,
                    Callback = test == "wrong-callback" ? new IntPtr(99) : agent.storedNavigationCallback.Pointer,
                };
                agent.TargetLocation = drive.Target;
                if (test == "stale-order") SpikeState.NextNavOrder();
                var direct = new Il2CppPathfinding.Path();
                var first = test == "approach-crosses" ? new Vector3(60f, 0f, -3.8f) : vehicle.transform.position;
                var acrossBlocker = new Vector3(91f, .6f, -3.8f);
                direct.vectorPath.Add(first);
                if (test == "interior-crosses") direct.vectorPath.Add(acrossBlocker);
                if (test == "many-points") for (int i = 0; i < 63; i++) direct.vectorPath.Add(first);
                direct.vectorPath.Add(test == "wrong-endpoint" ? via + new Vector3(3f, 0f, 0f) : via);
                bool preferred = CanPreferDirectDetourVia(agent, test == "wrong-target" ? destination : via, direct);
                bool refused = false;
                if (test is "unsafe-smoothing" or "smoothing-wrong-endpoint") {
                    var smoothed = new PathSmoothingUtility.SmoothedPath();
                    smoothed.vectorPath.Add(first);
                    if (test == "unsafe-smoothing") smoothed.vectorPath.Add(acrossBlocker);
                    smoothed.vectorPath.Add(test == "smoothing-wrong-endpoint" ? via + new Vector3(3f, 0f, 0f) : via);
                    refused = RejectDetourPath(agent, smoothed);
                }
                return (preferred, refused);
            }

            public static (bool, int, int, int, int, int, int, bool, bool, bool, int) RunFinalRescue(string test) {
                bool noVia = test.StartsWith("direct-", StringComparison.Ordinal);
                if (noVia) test = test.Substring("direct-".Length);
                scenario = test;
                viaDispatches = viaTerminals = rescues = NativeActuatorPatch.Parks = _freeSpotRescues = 0;
                NativeActuatorPatch.Healthy = test != "unhealthy";
                RoadKeeper.HasStaticBlocker = test != "unreadable-blocker";
                RoadKeeper.StaticBlockerId = test == "same-blocker" ? 417 : 691;
                RoadKeeper.StaticBounds = new Bounds(new(30f, 0f, -9f), new(35f, 2f, -6f));
                Time.timeScale = 1f;
                Time.time = Time.unscaledTime = 100f;
                SpikeState.NavigationRecovery.Begin(100f, test == "expired" ? 1f : 90f);
                for (int i = 0; i < (test == "exhausted" ? 4 : 3); i++)
                    SpikeState.NavigationRecovery.TrySpend(100f, TaxiAI.PathingFailureCap);
                if (test == "expired") Time.time = Time.unscaledTime = 102f;
                SpikeState.PollingActive = SpikeState.Operation.Active = SpikeState.RideActive = true;
                SpikeState.NavToPlayer = false;
                SpikeState.OperationId = 4;
                SpikeState.NavOrder = 22;
                SpikeState.Reverse.End();
                var vehicle = SpikeState.Vehicle = new LandVehicle();
                vehicle.Speed_Kmh = test == "unreadable-speed" ? float.NaN : test == "still-moving" ? 1.6f : .1f;
                vehicle.IsReversing = test == "native-reversing";
                var agent = vehicle.Agent;
                var via = vehicle.transform.position + new Vector3(test switch {
                    "via-boundary" => 10f,
                    "outside-via" => 10.25f,
                    "unreadable-via-distance" => float.NaN,
                    _ => 0f
                }, 0f, 0f);
                var destination = new Vector3(-156f, -4f, 113.7f);
                SpikeState.NavTarget = destination;
                var clearance = new Bounds(new(83f, 0f, -6f), new(88f, 2f, -3f));
                var drive = _detour = new DetourDrive(vehicle, agent, via, null, destination,
                    new StaticBlocker(417, vehicle.transform.position, vehicle.transform.forward, clearance), hasVia: !noVia) {
                    Order = SpikeState.NavOrder, WaitingForHalt = false, FinalLeg = true,
                    FinalRescueUsed = test == "used", PathApproved = test != "unapproved",
                    Settings = agent.currentNavigationSettings.Pointer, Callback = agent.storedNavigationCallback.Pointer,
                };
                agent.TargetLocation = destination;
                if (test == "stale-order") SpikeState.NextNavOrder();

                bool handled = CancelFinalDetourOnStall(vehicle, agent);
                bool waitedForHalt = drive.WaitingForHalt && !drive.PathApproved;
                bool pathRejected = false;
                if (test is "clear" or "via-boundary" or "crosses-original" or "crosses-final" || (noVia && test == "outside-via")) {
                    vehicle.Speed_Kmh = 1.6f;
                    TickDetour();
                    if (viaDispatches != 0) throw new InvalidOperationException("final leg resumed before halt");
                    vehicle.Speed_Kmh = .1f;
                    TickDetour();
                    var path = new PathSmoothingUtility.SmoothedPath();
                    path.vectorPath.Add(vehicle.transform.position);
                    if (test == "crosses-original") path.vectorPath.Add(new Vector3(91f, .6f, -3.8f));
                    if (test == "crosses-final") path.vectorPath.Add(new Vector3(24f, .6f, -8f));
                    path.vectorPath.Add(destination);
                    pathRejected = RejectDetourPath(agent, path);
                }
                return (handled, SpikeState.NavigationRecovery.Attempts, NativeActuatorPatch.Parks, rescues,
                    agent.NavigationStops, viaDispatches, viaTerminals, drive.Destination == destination &&
                    SpikeState.NavTarget == destination && drive.FinalLeg, waitedForHalt, pathRejected, _freeSpotRescues);
            }
            /* METHODS */
        }
        }
        """;
}

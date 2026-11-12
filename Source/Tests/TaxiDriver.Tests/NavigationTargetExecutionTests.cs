using System.Reflection;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes production recovery, redispatch and destination-pick methods; substitutes native boundaries, not route planning.</summary>
public class NavigationTargetExecutionTests
{
    private const string Goal = "110,0,0", Resolved = "100,0,0", Fallback = "80,0,10", Replacement = "70,0,20";
    private static readonly Lazy<MethodInfo> Replay = new(Compile);

    [Fact]
    public void ActualSecondRecoveryPublishesOneCoherentPassengerTargetBeforeNavigate()
    {
        Outcome result = Run("recover");
        AssertCoherent(result, Fallback);
        Assert.Equal(Goal, result.FallbackGoal);
        Assert.Equal(Resolved, result.FallbackCurrent);
        Assert.Equal("roadside candidate (fallback near lot entry)", result.DropOff);
        Assert.Equal("Alternative lot", result.Lot);
        Assert.Equal("A-running", result.ActiveName);
        AssertLogicalIdentity(result);
        AssertBudget(result, attempts: 2);
        Assert.Equal(41, result.Order);
        Assert.Equal("Failed", result.Callback);
    }

    [Fact]
    public void OrdinaryTargetReplacementAlsoUpdatesPassengerRouteWithoutRenamingTheTrip()
    {
        Outcome result = Run("redispatch");
        AssertCoherent(result, Replacement);
        Assert.Equal("original rule", result.DropOff);
        Assert.Equal("Original lot", result.Lot);
        Assert.Equal("A-running", result.ActiveName);
        AssertLogicalIdentity(result);
        AssertBudget(result, attempts: 1);
    }

    [Fact]
    public void PickupRecoveryNeverUsesAPassengerFallbackOrChangesPreselectedPassengerMetadata()
    {
        Outcome result = Run("pickup");
        Assert.Null(result.FallbackGoal);
        Assert.Null(result.FallbackCurrent);
        Assert.Equal("-100,0,0", result.Target);
        Assert.Equal(Resolved, result.Destination);
        Assert.Null(result.ActivePoint);
        Assert.Equal("A-running", result.ActiveName);
        Assert.Equal("original rule", result.DropOff);
        Assert.Equal("Original lot", result.Lot);
        AssertLogicalIdentity(result);
        AssertBudget(result, attempts: 2, progressGoal: "-100,0,0");
        Call call = Assert.Single(result.Calls);
        Assert.Equal("-100,0,0", call.Target);
        Assert.Equal(Resolved, call.Destination);
        Assert.Null(call.ActivePoint);
    }

    [Fact]
    public void DiagnosticRedispatchWithoutAPassengerTripDoesNotPublishPassengerMetadata()
    {
        Outcome result = Run("diagnostic");
        Assert.Equal(Replacement, result.Target);
        Assert.Equal(Resolved, result.Destination);
        Assert.Null(result.ActivePoint);
        Assert.Equal("A-running", result.ActiveName);
        AssertLogicalIdentity(result);
    }

    [Theory]
    [InlineData("recover")]
    [InlineData("redispatch")]
    public void SettingsConstructionFailureDoesNotPublishAnUndispatchedTarget(string scenario)
    {
        Outcome result = Run(scenario, "settings-constructor", "throw");
        Assert.Empty(result.Calls);
        AssertOriginalTarget(result);
        Assert.Contains(result.Errors, text => text.Contains("boundary threw", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("blocked")]
    [InlineData("stop")]
    [InlineData("terminal")]
    [InlineData("teardown")]
    [InlineData("authority")]
    [InlineData("foreign")]
    [InlineData("player")]
    [InlineData("dead")]
    [InlineData("agent")]
    [InlineData("expired")]
    public void RefusedRedispatchCannotPublishTheReplacementTarget(string refusal)
    {
        Outcome result = Run("redispatch", "entry", refusal);
        Assert.Empty(result.Calls);
        AssertOriginalTarget(result);
    }

    [Theory]
    [InlineData("blocked")]
    [InlineData("stop")]
    [InlineData("terminal")]
    [InlineData("teardown")]
    [InlineData("authority")]
    [InlineData("foreign")]
    [InlineData("player")]
    [InlineData("vehicle")]
    [InlineData("pointer")]
    [InlineData("agent")]
    [InlineData("order")]
    public void RefusedFallbackDoesNotPublishItsPointOrDropOffMetadata(string refusal)
    {
        Outcome result = Run("recover", "fallback", refusal);
        Assert.Empty(result.Calls);
        AssertOriginalTarget(result);
        AssertLogicalIdentity(result);
        AssertBudget(result, attempts: 2);
    }

    [Theory]
    [InlineData("seat", "stop")]
    [InlineData("seat", "reroute")]
    [InlineData("seat", "teardown")]
    [InlineData("fallback", "reroute")]
    [InlineData("release", "stop")]
    [InlineData("release", "reroute")]
    [InlineData("release", "teardown")]
    [InlineData("release", "authority")]
    [InlineData("release", "foreign")]
    [InlineData("release", "player")]
    [InlineData("release", "vehicle")]
    [InlineData("release", "pointer")]
    [InlineData("release", "agent")]
    [InlineData("release", "order")]
    [InlineData("settings-constructor", "stop")]
    [InlineData("settings-constructor", "reroute")]
    public void SupersededNativeContinuationCannotAdoptOrDispatchTheOldFallback(string boundary, string interruption)
    {
        Outcome result = Run("recover", boundary, interruption);
        Assert.Empty(result.Calls);
        if (interruption == "reroute")
        {
            Assert.Equal("999,0,0", result.Target);
            Assert.Equal("999,0,0", result.Destination);
            Assert.Equal("999,0,0", result.ActivePoint);
            Assert.Equal("new rule", result.DropOff);
            Assert.Equal("New lot", result.Lot);
        }
        else AssertOriginalTarget(result);
    }

    [Fact]
    public void DuplicatePickAfterFallbackRetainsTheActualRouteAndDoesNotDispatch()
    {
        Outcome result = Run("duplicate");
        AssertCoherent(result, Fallback);
        Assert.Equal(0, result.ReRoutes);
        Assert.Equal(41, result.Order);
        Assert.Equal("A-running", result.ActiveName);
        AssertLogicalIdentity(result);
        AssertBudget(result, attempts: 2);
    }

    [Fact]
    public void MetadataOnlyPickRetainsFallbackAndItsNextRedispatchDoesNotOverwriteTheSelectedName()
    {
        Outcome result = Run("metadata-redispatch");
        AssertCoherent(result, Replacement);
        Assert.Equal("B", result.Name);
        Assert.Equal("B", result.ActiveName);
        Assert.Equal("120,0,0", result.Goal);
        Assert.Equal(Resolved, result.Resolved);
        Assert.Equal("Alternative lot", result.Lot);
        Assert.Equal(0, result.ReRoutes);
        Assert.Equal(42, result.Order);
        Assert.Equal(2, result.Calls.Length);
        Assert.Equal(new[] { "A-running", "B" }, result.Calls.Select(c => c.ActiveName));
        Assert.Equal(2, result.Calls.Select(c => c.SettingsId).Distinct().Count());
        Assert.Equal("Failed", result.Callback); // a late Complete of order 41 was rejected
        AssertBudget(result, attempts: 2);
    }

    [Fact]
    public void NoAlternativeKeepsExistingPassengerTargetAndMetadata()
    {
        Outcome result = Run("no-fallback");
        AssertCoherent(result, Resolved);
        Assert.Equal("original rule", result.DropOff);
        Assert.Equal("Original lot", result.Lot);
        AssertLogicalIdentity(result);
        AssertBudget(result, attempts: 2);
    }

    [Theory]
    [InlineData("missing-goal")]
    [InlineData("clear-picker")]
    public void UnknownLogicalGoalCannotAuthorizeAnAlternateAroundTheMutableNavigationTarget(string scenario)
    {
        Outcome result = Run(scenario);
        Assert.Null(result.FallbackGoal);
        Assert.Equal(Resolved, result.Target);
        Assert.Equal(Resolved, result.ActivePoint);
        Assert.Null(result.Goal);
        if (scenario == "clear-picker")
        {
            Assert.Null(result.Destination);
            Assert.Null(result.Resolved);
            Assert.Equal("none selected", result.Name);
        }
        AssertBudget(result, attempts: 2);
    }

    [Theory]
    [InlineData("fallback")]
    [InlineData("release")]
    public void ClearingThePickerInsideNativeFallbackWorkCannotPublishTheObsoleteAlternative(string boundary)
    {
        Outcome result = Run("recover", boundary, "clear");
        Assert.Empty(result.Calls);
        Assert.Null(result.Goal);
        Assert.Null(result.Destination);
        Assert.Equal(Resolved, result.Target);
        Assert.Equal(Resolved, result.ActivePoint);
        Assert.Equal("none selected", result.Name);
    }

    [Theory]
    [InlineData("stop")]
    [InlineData("authority")]
    [InlineData("player")]
    public void NativeProgressWindowReadCannotRefuseDispatchAfterPublishingItsReplacement(string interruption)
    {
        Outcome result = Run("redispatch", "position", interruption);
        Assert.Empty(result.Calls);
        AssertOriginalTarget(result);
    }

    [Fact]
    public void StrictFallbackAdoptionCarriesItsOwnProvenanceWithoutReplacingResolverIdentity()
    {
        Outcome result = Run("recover");
        Assert.Equal(Fallback, result.StreetCandidate);
        Assert.Equal(Resolved, result.Resolved);
        Assert.Equal(Goal, result.Goal);
    }

    [Fact]
    public void UnverifiedOrdinaryTargetReplacementCannotAcquireStreetProvenance() =>
        Assert.Null(Run("redispatch").StreetCandidate);

    private static void AssertCoherent(Outcome result, string point)
    {
        Assert.Equal(point, result.Target);
        Assert.Equal(point, result.Destination);
        Assert.Equal(point, result.ActivePoint);
        Assert.NotEmpty(result.Calls);
        Assert.All(result.Calls, call =>
        {
            Assert.Equal(call.Target, call.Destination);
            Assert.Equal(call.Target, call.ActivePoint); // observe before the synchronous native callback
            Assert.Equal("True/True/False", call.Settings);
            Assert.Equal(1, call.Operation);
        });
    }

    private static void AssertOriginalTarget(Outcome result)
    {
        Assert.Equal(Resolved, result.Target);
        Assert.Equal(Resolved, result.Destination);
        Assert.Equal(Resolved, result.ActivePoint);
        Assert.Equal("A-running", result.ActiveName);
        Assert.Equal("original rule", result.DropOff);
        Assert.Equal("Original lot", result.Lot);
    }

    private static void AssertLogicalIdentity(Outcome result)
    {
        Assert.Equal(Goal, result.Goal);
        Assert.Equal(Resolved, result.Resolved);
        Assert.Equal("A", result.Name);
        Assert.Equal("property", result.Kind);
    }

    private static void AssertBudget(Outcome result, int attempts, string progressGoal = Resolved)
    {
        Assert.Equal(1, result.Operation);
        Assert.Equal(1, result.Generation);
        Assert.Equal(progressGoal, result.ProgressGoal);
        Assert.Equal(attempts, result.Attempts);
        Assert.False(result.ExpiredBefore);
        Assert.True(result.ExpiredAt);
    }

    private static Outcome Run(string scenario, string boundary = "", string interruption = "") =>
        JsonSerializer.Deserialize<Outcome>((string)Replay.Value.Invoke(null, new object[] { scenario, boundary, interruption })!)!;

    private sealed record Call(string Target, string? Destination, string? ActivePoint, string ActiveName,
        string Settings, int SettingsId, long Operation, int Order);
    private sealed record Outcome(string Target, string? Destination, string? ActivePoint, string? Goal, string? Resolved,
        string Name, string Kind, string ActiveName, string DropOff, string Lot, Call[] Calls, string? FallbackGoal,
        string? FallbackCurrent, int ReRoutes, int Attempts, int Order, long Operation, long Generation,
        string ProgressGoal, bool ExpiredBefore, bool ExpiredAt, string? Callback, string[] Errors, string? StreetCandidate);

    private static MethodInfo Compile()
    {
        string Read(string file)
        {
            string? root = Environment.GetEnvironmentVariable("TAXI_ENDPOINT_TEST_SOURCE_ROOT");
            return string.IsNullOrWhiteSpace(root) ? DriverSafetySourceContractTests.ReadSource(file) : File.ReadAllText(Path.Combine(root, file));
        }
        var runner = CSharpSyntaxTree.ParseText(Read("SpikeRunner.cs")).GetRoot().DescendantNodes()
            .OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.Text == "SpikeRunner");
        var commands = CSharpSyntaxTree.ParseText(Read("SpikeCommands.cs")).GetRoot();
        var policies = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs")).GetRoot();
        var selection = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DestinationSelectionPolicy.cs")).GetRoot();
        string[] methods = { "RecoverStuck", "ReDispatch", "TryBeginRecovery", "BeginNavigationOperation",
            "ObserveArtificialPlacement", "ArmProgressWindow", "StopIfNavigationExpired", "NavigationCallbackFor", "OnNavigationResult",
            "HasCurrentForwardEvidence", "SameAuthorizedTarget" };
        string[] policyTypes = { "NavigationCallbackPolicy", "NavigationRecoveryBudget", "NavigationTerminalPolicy",
            "RecoveryRecurrencePolicy", "NavigationOperationPolicy", "OperationPhase", "TrafficWaitBudget", "NavigationArrivalPolicy",
            "InitialDispatchRecoveryPolicy" };
        string source = Harness
            .Replace("/* RUNNER */", string.Join("\n", runner.Members.OfType<MethodDeclarationSyntax>()
                .Where(m => methods.Contains(m.Identifier.Text)).Select(m => m.ToFullString())))
            // This replay exercises ordinary navigation, without automatic pickup or native detour state.
            .Replace("/* FIELDS */", string.Join("\n", runner.Members.OfType<FieldDeclarationSyntax>()
                .Where(f => !f.Declaration.Variables.Any(v => v.Identifier.Text is "_autoNavigationReceipt" or "_autoNavigationSerial" or
                    "_detourUsedOperation" or "_detourReverse" or "_detour"))
                .Select(f => f.ToFullString())))
            .Replace("/* COMMANDS */", string.Join("\n", commands.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.Text is "CreateNavigationSettings" or "ToDestination" or "PickPoint").Select(m => m.ToFullString())))
            .Replace("/* POLICIES */", string.Join("\n", policies.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
                .Where(t => policyTypes.Contains(t.Identifier.Text)).Select(t => t.ToFullString())))
            .Replace("/* SELECTION */", selection.ToFullString());
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("TaxiNavigationTargetReplay", new[] { CSharpSyntaxTree.ParseText(source) },
            references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        Assert.True(result.Success, string.Join("\n", result.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.SpikeRunner")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text.Json;
        namespace Replay {
        /* POLICIES */
        /* SELECTION */
        public record struct Vector3(float x, float y, float z) {
            public static Vector3 zero => new();
            public static float Distance(Vector3 a, Vector3 b) => MathF.Sqrt((a.x-b.x)*(a.x-b.x)+(a.y-b.y)*(a.y-b.y)+(a.z-b.z)*(a.z-b.z));
        }
        public sealed class Transform {
            private Vector3 value;
            public Vector3 position { get { Boundary.Hit("position"); return value; } set { this.value = value; } }
        }
        public sealed class LandVehicle {
            public IntPtr Pointer = new(1); public Transform transform = new(); public VehicleAgent Agent = new(); public float Speed_Kmh;
        }
        public sealed class NavigationSettings {
            public static int Count; public int Id = ++Count;
            private bool road, proximity = true, teleport = true;
            public NavigationSettings() { Boundary.Hit("settings-constructor"); }
            public bool endAtRoad { get => road; set { road = value; } }
            public bool ensureProximityToGraph { get => proximity; set { proximity = value; } }
            public bool teleportToGraphIfCalculationFails { get => teleport; set { teleport = value; } }
        }
        public sealed class VehicleAgent {
            public IntPtr Pointer = new(2);
            public enum ENavigationResult { Failed, Complete, Stopped }
            public void Navigate(Vector3 target, NavigationSettings settings, Action<ENavigationResult> callback) {
                Native.Calls.Add(new(SpikeCommands.Fmt(target), SpikeCommands.Fmt(SpikeState.RideDestination),
                    SpikeCommands.Fmt(SpikeState.ActiveTripPoint), SpikeState.ActiveTripName,
                    $"{settings.endAtRoad}/{settings.ensureProximityToGraph}/{settings.teleportToGraphIfCalculationFails}",
                    settings.Id, SpikeState.OperationId, SpikeState.NavOrder));
                Native.Callbacks.Add(callback);
                callback?.Invoke(ENavigationResult.Failed);
            }
        }
        public static class Native {
            public sealed record Call(string Target, string Destination, string ActivePoint, string ActiveName,
                string Settings, int SettingsId, long Operation, int Order);
            public static readonly List<Call> Calls = new();
            public static readonly List<Action<VehicleAgent.ENavigationResult>> Callbacks = new();
            public static bool Authority = true, Owned = true, PlayerDriver;
        }
        public static class Boundary {
            public static string Stage = "", Action = ""; public static bool Used;
            public static void Hit(string stage) {
                if (Used || Stage != stage || (stage == "position" && Time.unscaledTime < 110)) return;
                Used = true;
                switch (Action) {
                    case "throw": throw new Exception("boundary threw");
                    case "blocked": TaxiAI.NavigationBlocked = true; break;
                    case "stop": SpikeState.PollingActive = false; SpikeState.Operation.Terminate("STOP", false); SpikeState.NextNavOrder(); break;
                    case "terminal": SpikeState.Operation.Terminate("terminal", false); break;
                    case "teardown": Mod.Lifecycle.AcceptingWork = false; break;
                    case "authority": Native.Authority = false; break;
                    case "foreign": Native.Owned = false; break;
                    case "player": Native.PlayerDriver = true; break;
                    case "dead": SpikeState.Vehicle.Pointer = IntPtr.Zero; break;
                    case "vehicle": SpikeState.Vehicle = new() { Pointer = new(3) }; break;
                    case "pointer": SpikeState.Vehicle.Pointer = new(3); break;
                    case "agent": SpikeState.Vehicle.Agent = new() { Pointer = new(4) }; break;
                    case "order": SpikeState.NextNavOrder(); break;
                    case "expired": Time.unscaledTime = 190; break;
                    case "clear":
                        SpikeState.RideGoal = SpikeState.RideResolvedArrival = SpikeState.RideDestination = SpikeState.RideStreetCandidate = null;
                        SpikeState.RideDestinationPicked = false; SpikeState.RideDestinationName = "none selected";
                        SpikeState.RideDropOff = SpikeState.RideDropOffLot = "";
                        break;
                    case "reroute":
                        SpikeState.OperationId++; SpikeState.NextNavOrder();
                        SpikeState.NavTarget = new(999,0,0); SpikeState.RideDestination = SpikeState.ActiveTripPoint = SpikeState.NavTarget;
                        SpikeState.RideDropOff = "new rule"; SpikeState.RideDropOffLot = "New lot";
                        break;
                }
            }
        }
        public static class Time { public static float unscaledTime; public static int frameCount; }
        public sealed class Life {
            public bool AcceptingWork = true;
            public Action<T> Guard<T>(Action<T> action) => value => { if (AcceptingWork) action(value); };
        }
        public static class Mod { public static readonly Logger Log = new(); public static readonly Life Lifecycle = new(); }
        public sealed class Logger {
            public readonly List<string> Errors = new(); public void Info(string text) {} public void Warn(string text) {}
            public void Error(string text) { Errors.Add(text); }
        }
        public static class TaxiLog { public static void Verbose(string text) {} }
        public static class SpikeTrace { public static void RouteSnapshot(VehicleAgent a, string text) {} }
        public static class NativeActuatorPatch { public static void ReleasePark(LandVehicle v) { Boundary.Hit("release"); } }
        public static class TaxiDestinations {
            public const float LotDropOffRadius = 60f;
            public sealed record Destination(string Name, string Kind, Vector3 Goal);
            public sealed record Arrival(Vector3 Point, string Kind, string LotName);
            public static bool Available = true; public static Vector3? FallbackGoal, FallbackCurrent;
            public static bool TryFallbackDropOff(Vector3 goal, Vector3 current, out Vector3 fallback, out string lot) {
                FallbackGoal = goal; FallbackCurrent = current; Boundary.Hit("fallback");
                fallback = new(80,0,10); lot = "Alternative lot"; return Available;
            }
            public static bool ResolveArrival(Destination d, out Arrival arrival) { arrival = new(new(100,0,0), "lot entry", "Original lot"); return true; }
            public static string Fmt(Vector3 v) => SpikeCommands.Fmt(v);
            public static string Num(float value) => value.ToString();
        }
        public static class TaxiAI {
            public static bool NavigationBlocked, Active; public const int PathingFailureCap = 4;
            public static void StopPatrol(string why) { Active = false; }
        }
        public sealed class Reverse { public bool Active; public void End() { Active = false; } }
        public static class SpikeState {
            public static LandVehicle Vehicle;
            public static Vector3 NavTarget, ProgressFrom;
            public static Vector3? RideDestination, ActiveTripPoint, RideGoal, RideResolvedArrival, RideStreetCandidate;
            public static float NavStartTime, NavRetryAt, ProgressWindowStart;
            public static bool PollingActive, NavEverAutoDriving, NavRetried, NavToPlayer, NavGaveUp, RideArrived;
            public static bool RideDestinationPicked, RideActive, RideBoarded, RidePassengerMode;
            public static int NavOrder, StuckRecoveries, NavCallbackQueuedOrder;
            public static long OperationId, NavCallbackQueuedOperation;
            public static string NavCallbackResult, NavCallbackQueuedResult;
            public static string RideDestinationName, RideDestinationKind, ActiveTripName, RideDropOff, RideDropOffLot;
            public static readonly Reverse Reverse = new();
            internal static readonly NavigationRecoveryBudget NavigationRecovery = new();
            internal static readonly NavigationOperationPolicy Operation = new();
            public static int NextNavOrder() => ++NavOrder; public static long NextOperationId() => ++OperationId;
        }
        public static class SpikeCommands {
            public static int ReRoutes;
            public static bool HasAuthority() => Native.Authority;
            public static bool IsOwnedTaxi(LandVehicle v) => Native.Owned && ReferenceEquals(v, SpikeState.Vehicle) && v.Pointer != IntPtr.Zero;
            public static string Fmt(Vector3 v) => $"{v.x},{v.y},{v.z}";
            public static string Fmt(Vector3? v) => v.HasValue ? Fmt(v.Value) : null;
            public static void Print(string text) {}
            public static bool EnsureDriverSeat(LandVehicle v) { Boundary.Hit("seat"); return false; }
            public static bool ReRoute(string why) { ReRoutes++; return true; }
            /* COMMANDS */
        }
        public static class SpikeRunner {
            /* FIELDS */
            /* RUNNER */
            private static bool PlayerDriver(LandVehicle v) => Native.PlayerDriver;
            private static void ResetSupervisorMotion() {}
            private static bool TryCompleteBeforeRecovery(VehicleAgent a, LandVehicle v, Vector3 p, float d, string trigger, bool logDecision) => false;
            private static bool RecentForwardMotionUnjustifiesStall(bool readable, float speed, bool reversing) => false;
            private static void LogSupervisorDecision(string trigger, string action, float distance, bool readable, float speed, bool recent, int before, int after) {}
            private static void CancelDeferredProbe() {}
            private readonly record struct StaticBlocker;
            private static StaticBlocker? ObserveDetourBlocker(LandVehicle v, VehicleAgent a, Vector3 p) => null;
            private static bool TryStartDetour(LandVehicle v, VehicleAgent a, StaticBlocker blocker) => false;
            private static bool TryQueueDetourReverse(LandVehicle veh, VehicleAgent agent, StaticBlocker hit, float now) => false;
            private static bool CancelFinalDetourOnStall(LandVehicle v, VehicleAgent a) => false;
            private static bool TryFreeSpotRescue(LandVehicle v, VehicleAgent a, string why) => false;
            private static bool TryStartReverse(VehicleAgent a, LandVehicle v, float now, string why) => false;
            private static bool TryTeleportRecovery(LandVehicle v, VehicleAgent a, Vector3 p, float distance) => false;
            private static void GiveUpNavigation(VehicleAgent a, Vector3 p, float distance, float elapsed) {
                SpikeState.PollingActive = false; SpikeState.Operation.Terminate("expired", true); SpikeState.NextNavOrder();
            }
            public static string Run(string scenario, string boundary, string interruption) {
                Native.Calls.Clear(); Native.Callbacks.Clear(); Mod.Log.Errors.Clear();
                Native.Authority = Native.Owned = true; Native.PlayerDriver = false; NavigationSettings.Count = 0;
                Boundary.Stage = boundary; Boundary.Action = interruption; Boundary.Used = false;
                Mod.Lifecycle.AcceptingWork = true; TaxiAI.Active = TaxiAI.NavigationBlocked = false;
                SpikeCommands.ReRoutes = 0; TaxiDestinations.Available = scenario != "no-fallback";
                TaxiDestinations.FallbackGoal = TaxiDestinations.FallbackCurrent = null;
                SpikeState.Vehicle = new(); var vehicle = SpikeState.Vehicle; var agent = vehicle.Agent;
                SpikeState.NavOrder = 40; SpikeState.OperationId = 0; SpikeState.Operation.Reset(); SpikeState.NavigationRecovery.Reset();
                SpikeState.RideDestination = SpikeState.ActiveTripPoint = SpikeState.NavTarget = new(100,0,0);
                SpikeState.RideStreetCandidate = new(100,0,0);
                SpikeState.RideGoal = new(110,0,0); SpikeState.RideResolvedArrival = new(100,0,0);
                SpikeState.RideDestinationName = "A"; SpikeState.RideDestinationKind = "property"; SpikeState.ActiveTripName = "A-running";
                SpikeState.RideDropOff = "original rule"; SpikeState.RideDropOffLot = "Original lot";
                SpikeState.RideDestinationPicked = SpikeState.RideActive = SpikeState.PollingActive = true;
                SpikeState.RideBoarded = SpikeState.RidePassengerMode = SpikeState.NavToPlayer = false;
                SpikeState.NavStartTime = 100; SpikeState.StuckRecoveries = 1;
                SpikeState.NavCallbackResult = SpikeState.NavCallbackQueuedResult = null;
                SpikeState.NavCallbackQueuedOrder = -1; SpikeState.NavCallbackQueuedOperation = -1;
                if (scenario is "pickup" or "diagnostic") {
                    SpikeState.RideActive = false; SpikeState.ActiveTripPoint = null;
                    if (scenario == "pickup") { SpikeState.NavToPlayer = true; SpikeState.NavTarget = new(-100,0,0); }
                }
                if (scenario == "missing-goal") SpikeState.RideGoal = null;
                if (scenario == "clear-picker") {
                    Boundary.Stage = "clear-picker"; Boundary.Action = "clear"; Boundary.Hit("clear-picker");
                }
                _trafficWaiting = _recoveryOwnsNavigation = false;
                Time.unscaledTime = 100; long generation = SpikeState.Operation.Generation;
                BeginNavigationOperation(); SpikeState.NavigationRecovery.TrySpend(100, TaxiAI.PathingFailureCap);
                Time.unscaledTime = 110; Boundary.Hit("entry");
                if (scenario is "redispatch" or "diagnostic") ReDispatch(agent, new(70,0,20), "test replacement");
                else RecoverStuck(agent, vehicle, vehicle.transform.position, 100, 10, true);
                if (scenario is "duplicate" or "metadata-redispatch") {
                    bool metadata = scenario == "metadata-redispatch";
                    SpikeCommands.ToDestination(new(metadata ? "B" : "A", "property", new(metadata ? 120 : 110,0,0)), "test pick");
                    if (metadata) {
                        Time.unscaledTime = 111; ReDispatch(agent, new(70,0,20), "after metadata pick");
                        Native.Callbacks.First().Invoke(VehicleAgent.ENavigationResult.Complete);
                    }
                }
                return JsonSerializer.Serialize(new {
                    Target = SpikeCommands.Fmt(SpikeState.NavTarget), Destination = SpikeCommands.Fmt(SpikeState.RideDestination),
                    ActivePoint = SpikeCommands.Fmt(SpikeState.ActiveTripPoint), Goal = SpikeCommands.Fmt(SpikeState.RideGoal),
                    Resolved = SpikeCommands.Fmt(SpikeState.RideResolvedArrival), Name = SpikeState.RideDestinationName,
                    Kind = SpikeState.RideDestinationKind, ActiveName = SpikeState.ActiveTripName,
                    DropOff = SpikeState.RideDropOff, Lot = SpikeState.RideDropOffLot, Calls = Native.Calls,
                    FallbackGoal = SpikeCommands.Fmt(TaxiDestinations.FallbackGoal), FallbackCurrent = SpikeCommands.Fmt(TaxiDestinations.FallbackCurrent),
                    ReRoutes = SpikeCommands.ReRoutes, Attempts = SpikeState.NavigationRecovery.Attempts,
                    Order = SpikeState.NavOrder, Operation = SpikeState.OperationId, Generation = SpikeState.Operation.Generation - generation,
                    ProgressGoal = SpikeCommands.Fmt(_operationProgressTarget),
                    ExpiredBefore = SpikeState.NavigationRecovery.HasExpired(189.99f), ExpiredAt = SpikeState.NavigationRecovery.HasExpired(190),
                    Callback = SpikeState.NavCallbackResult, Errors = Mod.Log.Errors, StreetCandidate = SpikeCommands.Fmt(SpikeState.RideStreetCandidate)
                });
            }
        }
        }
        """;
}

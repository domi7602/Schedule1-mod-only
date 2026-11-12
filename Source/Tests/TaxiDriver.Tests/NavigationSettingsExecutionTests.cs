using System.Reflection;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes production dispatch, polling and recovery methods with hostile native boundaries; not native pathfinding.</summary>
public class NavigationSettingsExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(Compile);

    [Fact]
    public void InitialDispatchCommitsStateBeforeSynchronousCallback()
    {
        Outcome result = Run("initial");
        Assert.True(result.Success);
        Assert.Single(result.Calls);
        AssertFreshExplicitSettings(result);
        Assert.Equal(41, result.QueuedOrder);
        Assert.Equal(1, result.QueuedOperation);
        Assert.Equal("Failed", result.Callback);
        Assert.True(result.Polling);
        AssertOriginalDeadline(result, attempts: 0);
    }

    [Fact]
    public void CallbacklessFallbackDoesNotReuseSettingsMutatedByThrowingNativeCall()
    {
        Outcome result = Run("fallback");
        Assert.True(result.Success);
        Assert.Equal(2, result.Calls.Length);
        AssertFreshExplicitSettings(result);
        Assert.True(result.Calls[0].HasCallback);
        Assert.False(result.Calls[1].HasCallback);
        Assert.Equal(new[] { 41, 41 }, result.Calls.Select(c => c.Order));
        Assert.Equal("Failed", result.Callback); // synchronous callback was not overwritten by fallback bookkeeping
        Assert.Equal(41, result.QueuedOrder);
        AssertOriginalDeadline(result, attempts: 0);
    }

    [Theory]
    [InlineData("redispatch")]
    [InlineData("redispatch-throw")]
    public void RepeatedRedispatchUsesFreshSettingsAndRejectsOldCallbacksWithoutRenewingOperation(string scenario)
    {
        Outcome result = Run(scenario);
        Assert.Equal(3, result.Calls.Length);
        AssertFreshExplicitSettings(result);
        Assert.Equal(new[] { 41, 42, 43 }, result.Calls.Select(c => c.Order));
        Assert.Equal(43, result.QueuedOrder);
        Assert.Equal("Failed", result.Callback); // oldest Complete was delivered after the newest Failed
        Assert.Equal(102f, result.NavStartTime); // dispatch-local window changes, operation deadline does not
        AssertOriginalDeadline(result, attempts: 2);
    }

    [Fact]
    public void RepeatedProductionRecoveryRemainsCappedAndKeepsExplicitSettings()
    {
        Outcome result = Run("recovery");
        Assert.Equal(4, result.Calls.Length); // initial + three handled recurring rescues; no dispatch past the cap
        AssertFreshExplicitSettings(result);
        Assert.Equal(new[] { 41, 42, 43, 44 }, result.Calls.Select(c => c.Order));
        Assert.True(result.Terminal);
        Assert.False(result.Polling);
        AssertOriginalDeadline(result, attempts: 4);
    }

    [Theory]
    [InlineData("polling")]
    [InlineData("polling-throw")]
    public void RealPollingRetryIsOneTimeFreshAndOrderSafe(string scenario)
    {
        Outcome result = Run(scenario);
        Assert.Equal(2, result.Calls.Length); // includes a second heartbeat inside the unchanged grace window
        AssertFreshExplicitSettings(result);
        Assert.Equal(new[] { 41, 42 }, result.Calls.Select(c => c.Order));
        Assert.Equal(42, result.QueuedOrder);
        Assert.Equal("Failed", result.Callback);
        Assert.True(result.Retried);
        Assert.Equal(103f, result.RetryAt);
        Assert.Equal(100f, result.NavStartTime);
        Assert.True(result.Polling);
        AssertOriginalDeadline(result, attempts: 2);
    }

    [Theory]
    [InlineData("redispatch-expired", 3)]
    [InlineData("polling-expired", 2)]
    public void DispatchWindowsCannotExtendTheOperationDeadline(string scenario, int calls)
    {
        Outcome result = Run(scenario);
        Assert.Equal(calls, result.Calls.Length);
        AssertFreshExplicitSettings(result);
        Assert.True(result.Terminal);
        Assert.False(result.Polling);
        AssertOriginalDeadline(result, attempts: 2);
    }

    [Theory]
    [InlineData("patrol-start")]
    [InlineData("patrol-stall")]
    public void ProductionPatrolHandoffUsesTheSameFreshExplicitContract(string scenario)
    {
        Outcome result = Run(scenario);
        Assert.Equal(2, result.Calls.Length);
        AssertFreshExplicitSettings(result);
        Assert.Equal(new[] { 41, 42 }, result.Calls.Select(c => c.Order));
        Assert.False(result.PatrolActive);
        AssertOriginalDeadline(result, attempts: 3);
    }

    [Theory]
    [InlineData("initial", "constructor")]
    [InlineData("initial", "endAtRoad")]
    [InlineData("initial", "ensureProximityToGraph")]
    [InlineData("initial", "teleportToGraphIfCalculationFails")]
    [InlineData("fallback", "constructor")]
    [InlineData("fallback", "endAtRoad")]
    [InlineData("fallback", "ensureProximityToGraph")]
    [InlineData("fallback", "teleportToGraphIfCalculationFails")]
    [InlineData("redispatch", "constructor")]
    [InlineData("redispatch", "endAtRoad")]
    [InlineData("redispatch", "ensureProximityToGraph")]
    [InlineData("redispatch", "teleportToGraphIfCalculationFails")]
    [InlineData("polling", "constructor")]
    [InlineData("polling", "endAtRoad")]
    [InlineData("polling", "ensureProximityToGraph")]
    [InlineData("polling", "teleportToGraphIfCalculationFails")]
    public void ConstructionOrSetterFailureNeverDispatchesNullOrNativeDefaults(string path, string boundary)
    {
        Outcome result = Run("failure-" + path, boundary);
        Assert.Equal(path == "initial" ? 0 : 1, result.Calls.Length);
        AssertFreshExplicitSettings(result);
        Assert.Equal(path == "initial" ? 1 : 2, result.Constructions);
        Assert.Contains(result.Errors, text => text.Contains("settings-" + boundary, StringComparison.Ordinal));
        if (path is "initial" or "fallback")
        {
            Assert.False(result.Success);
            Assert.False(result.Polling);
        }
        else
        {
            Assert.True(result.Retried);
            Assert.True(result.Terminal); // the unchanged deadline still ends a failed retry
            Assert.False(result.Polling);
            AssertOriginalDeadline(result, attempts: 2);
        }
    }

    [Fact]
    public void AllServiceNavigateCallSitesAreCoveredAndCannotOptIntoNullSettings()
    {
        var roots = new[] { "SpikeCommands.cs", "SpikeRunner.cs" }
            .Select(file => CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource(file)).GetRoot()).ToArray();
        var calls = roots.SelectMany(root => root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            .Where(call => call.Expression is MemberAccessExpressionSyntax member && member.Name.Identifier.Text == "Navigate").ToArray();
        static MethodDeclarationSyntax Caller(InvocationExpressionSyntax call) =>
            call.Ancestors().OfType<MethodDeclarationSyntax>().First();
        var detour = Assert.Single(calls, call => Caller(call).Identifier.Text == "DispatchDetourLeg");
        var ordinary = calls.Where(call => call != detour).ToArray();
        Assert.Equal(4, ordinary.Length);
        Assert.All(ordinary, call =>
        {
            Assert.False(call.ArgumentList.Arguments[1].Expression.IsKind(SyntaxKind.NullLiteralExpression), call.ToString());
            Assert.Contains(Caller(call).Identifier.Text, new[] { "Go", "ReDispatch", "TickNavigationPolling" });
        });
        var detourSettings = Assert.IsType<IdentifierNameSyntax>(detour.ArgumentList.Arguments[1].Expression);
        var settingsDeclaration = Assert.Single(Caller(detour).DescendantNodes().OfType<VariableDeclaratorSyntax>(),
            variable => variable.Identifier.Text == detourSettings.Identifier.Text);
        Assert.Equal("SpikeCommands.CreateNavigationSettings()", settingsDeclaration.Initializer?.Value.ToString());
        var go = roots[0].DescendantNodes().OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == "Go");
        Assert.DoesNotContain(go.ParameterList.Parameters, p => p.Identifier.Text == "useSettings");
    }

    private static void AssertFreshExplicitSettings(Outcome result)
    {
        Assert.All(result.Calls, call =>
        {
            Assert.Equal("True/True/False", call.Settings);
            Assert.True(call.Polling); // bookkeeping precedes the native call and its synchronous callback
            Assert.Equal(1, call.Operation);
        });
        Assert.Equal(result.Calls.Length, result.Calls.Select(c => c.SettingsId).Distinct().Count());
    }

    private static void AssertOriginalDeadline(Outcome result, int attempts)
    {
        Assert.Equal(1, result.Operation);
        Assert.Equal(1, result.Generation);
        Assert.Equal(attempts, result.Attempts);
        Assert.False(result.ExpiredBefore);
        Assert.True(result.ExpiredAt);
    }

    private static Outcome Run(string scenario, string boundary = "") =>
        JsonSerializer.Deserialize<Outcome>((string)Replay.Value.Invoke(null, new object[] { scenario, boundary })!)!;

    private sealed record Call(string Settings, int SettingsId, bool HasCallback, int Order, long Operation, bool Polling);
    private sealed record Outcome(Call[] Calls, bool Success, int Constructions, string[] Errors, int Attempts,
        long Operation, long Generation, int QueuedOrder, long QueuedOperation, string? Callback,
        float NavStartTime, float RetryAt, bool Retried, bool Polling, bool Terminal, bool PatrolActive,
        bool ExpiredBefore, bool ExpiredAt);

    private static MethodInfo Compile()
    {
        var commands = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs")).GetRoot();
        var runner = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs")).GetRoot()
            .DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.Text == "SpikeRunner");
        var policies = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs")).GetRoot();
        string[] runnerMethods = { "BeginNavigationOperation", "ObserveArtificialPlacement", "StopIfNavigationExpired",
            "TryBeginRecovery", "ArmStartupRecovery", "ArmPatrolSelfTest", "ArmProgressWindow", "NavigationCallbackFor",
            "OnNavigationResult", "ReDispatch", "TickNavigationPolling", "TickPatrolSupervision", "GiveUpNavigation", "TryTeleportRecovery" };
        string[] policyTypes = { "NavigationCallbackPolicy", "NavigationRecoveryBudget", "NavigationTerminalPolicy",
            "RecoveryRecurrencePolicy", "NavigationOperationPolicy", "OperationPhase", "TrafficWaitBudget",
            "NavigationArrivalPolicy", "SupervisedArrivalPolicy", "ArrivalConfirmationPolicy", "ArrivalGateOutcome",
            "PatrolStartPolicy", "TerminalRescuePolicy", "ReverseSupervisionPolicy" };
        string source = Harness
            .Replace("/* REQUEST */", string.Join("\n", commands.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(c => c.Identifier.Text == "DriveRequest").Select(c => c.ToFullString())))
            // The baseline has no factory: still compile and execute its real failing dispatches, rather than fail for a missing helper.
            .Replace("/* COMMANDS */", string.Join("\n", commands.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.Text is "Go" or "CreateNavigationSettings").Select(m => m.ToFullString())))
            .Replace("/* RUNNER */", string.Join("\n", runner.Members.OfType<MethodDeclarationSyntax>()
                .Where(m => runnerMethods.Contains(m.Identifier.Text)).Select(m => m.ToFullString())))
            // The replay retains ordinary recovery and substitutes the native detour seam below.
            .Replace("/* FIELDS */", string.Join("\n", runner.Members.OfType<FieldDeclarationSyntax>()
                .Where(f => !f.Declaration.Variables.Any(v => v.Identifier.Text is
                    "_detourUsedOperation" or "_detourReverse" or "_detour"))
                .Select(f => f.ToFullString())))
            .Replace("/* POLICIES */", string.Join("\n", policies.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
                .Where(t => policyTypes.Contains(t.Identifier.Text)).Select(t => t.ToFullString())));
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("TaxiNavigationSettingsReplay", new[] { CSharpSyntaxTree.ParseText(source) },
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
        public struct Vector3 {
            public float x, y, z;
            public static Vector3 zero => new(); public static Vector3 forward => new() { z = 1 };
            public float sqrMagnitude => x*x + y*y + z*z;
            public static Vector3 operator +(Vector3 a, Vector3 b) => new() { x = a.x+b.x, y = a.y+b.y, z = a.z+b.z };
            public static Vector3 operator *(Vector3 a, float b) => new() { x = a.x*b, y = a.y*b, z = a.z*b };
            public static float Distance(Vector3 a, Vector3 b) => MathF.Sqrt((a.x-b.x)*(a.x-b.x) + (a.y-b.y)*(a.y-b.y) + (a.z-b.z)*(a.z-b.z));
        }
        public sealed class Transform { public Vector3 position, forward = Vector3.forward; public object rotation; }
        public sealed class Player { public static Player Local = new(); public Transform transform = new(); }
        public sealed class NPC { }
        public sealed class LandVehicle {
            public IntPtr Pointer = new(1); public Transform transform = new(); public VehicleAgent Agent = new();
            public LandVehicle gameObject => this; public T GetComponent<T>() where T : class => Agent as T;
            public bool isParked, BrakesApplied, HandbrakeApplied, IsPhysicallySimulated = true;
            public float Speed_Kmh; public bool ShouldBePhysicallySimulated() => true;
            public void UpdatePhysicallySimulated(bool value) { IsPhysicallySimulated = value; }
            public void ExitPark(bool value) { isParked = false; }
        }
        public sealed class NavigationSettings {
            public static int Constructions, FailAt; public static string FailBoundary;
            public readonly int Id; private bool road, proximity = true, teleport = true;
            public NavigationSettings() { Id = ++Constructions; Fail("constructor"); }
            private void Fail(string boundary) { if (Id == FailAt && FailBoundary == boundary) throw new Exception("settings-" + boundary); }
            public bool endAtRoad { get => road; set { Fail("endAtRoad"); road = value; } }
            public bool ensureProximityToGraph { get => proximity; set { Fail("ensureProximityToGraph"); proximity = value; } }
            public bool teleportToGraphIfCalculationFails { get => teleport; set { Fail("teleportToGraphIfCalculationFails"); teleport = value; } }
            public void Corrupt() { road = proximity = false; teleport = true; }
        }
        public sealed class VehicleAgent {
            public IntPtr Pointer = new(2);
            public enum ENavigationResult { Failed, Complete, Stopped }
            public bool enabled = true, AutoDriving, NavigationCalculationInProgress, IsReversing;
            public string KinematicMode => "native";
            public bool IsOnVehicleGraph() => true; public bool GetIsStuck() => false;
            public void StopNavigating() { AutoDriving = false; } public void StopReversing() { IsReversing = false; }
            public void Navigate(Vector3 target, NavigationSettings settings, Action<ENavigationResult> callback) => Native.Navigate(settings, callback);
        }
        public static class Native {
            public sealed record Call(string Settings, int SettingsId, bool HasCallback, int Order, long Operation, bool Polling);
            public static readonly List<Call> Calls = new();
            public static readonly List<Action<VehicleAgent.ENavigationResult>> Callbacks = new();
            public static int ThrowAt; public static bool Progress = true;
            public static void Navigate(NavigationSettings settings, Action<VehicleAgent.ENavigationResult> callback) {
                Calls.Add(new(settings == null ? "null" : $"{settings.endAtRoad}/{settings.ensureProximityToGraph}/{settings.teleportToGraphIfCalculationFails}",
                    settings?.Id ?? 0, callback != null, SpikeState.NavOrder, SpikeState.OperationId, SpikeState.PollingActive));
                Callbacks.Add(callback);
                settings?.Corrupt(); // native ownership is hostile even when the call subsequently throws
                callback?.Invoke(VehicleAgent.ENavigationResult.Failed); // synchronous result must see committed bookkeeping
                if (Calls.Count == ThrowAt) throw new Exception("native Navigate threw after mutation/callback");
            }
            public static void DeliverOldComplete() => Callbacks.FirstOrDefault()?.Invoke(VehicleAgent.ENavigationResult.Complete);
        }
        public static class NavigationUtility { public static Vector3 SampleVehicleGraph(Vector3 target) => target; }
        public static class Time { public static float unscaledTime = 100, timeScale = 1; public static int frameCount; }
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
        public static class SpikeTrace { public static void EnsureRouteDiagnostics() {} public static void RouteSnapshot(VehicleAgent agent, string text) {} }
        public static class NativeActuatorPatch { public static void ReleasePark(LandVehicle v) {} public static void RequestPark(LandVehicle v) {} }
        public static class CleanupGuard {
            public static void Run(string name, Action action, Action<string, Exception> report) { try { action(); } catch(Exception ex) { report(name, ex); } }
        }
        public static class RoadKeeper {
            public static void Reset() {}
            public static bool HasObstacle(LandVehicle v, Vector3 p, object r, bool staticOnly) => false;
            public static bool TryRescueStartup(LandVehicle v, VehicleAgent a, string why) => false;
        }
        public static class TaxiDestinations { public static string Num(float value) => value.ToString(); }
        public static class TaxiAI {
            public static bool NavigationBlocked, Active; public const int PathingFailureCap = 4;
            public static int CurrentWaypoint => 0; public static int WaypointCount => 3;
            public static void StopPatrol(string why) { Active = false; }
        }
        public sealed class Reverse { public bool Active; public void End() { Active = false; } }
        public static class SpikeState {
            public static LandVehicle Vehicle;
            public static NPC DriverNpc;
            public static Vector3 NavTarget, NavStartPosition, NavReDispatchTarget, ProgressFrom;
            public static Vector3? RideDestination, ActiveTripPoint, RideGoal, RideResolvedArrival, RideStreetCandidate;
            public static float NavStartDistance, NavStartTime, LastHeartbeat, NavRetryAt, NavReDispatchAt, ProgressWindowStart;
            public static bool PollingActive, NavEverAutoDriving, NavRetried, NavToPlayer, NavGaveUp, RideArrived, RideAwaitingBoard, RideActive, RideDestinationPicked;
            public static int NavOrder, StuckRecoveries, NavCallbackQueuedOrder, NavCallbackRecoveryOrder, NavCallbackArrivalOrder;
            public static long OperationId, NavCallbackQueuedOperation, NavCallbackRecoveryOperation, NavCallbackArrivalOperation, RideSerial;
            public static bool AutoRunning, AutoToPlayer; public static int AutoStep; public static float AutoNextAt;
            public static long AutoRunSerial;
            public static string AutoCode, ServiceStatus; public static Vector3? AutoTarget;
            public static void ResetAutoRun() { AutoRunSerial++; AutoRunning = false; AutoStep = 0; }
            public static void ResetRide() { RideActive = false; ActiveTripPoint = null; }
            public static void SetServiceStatus(string message) { ServiceStatus = message; }
            public static bool NavCallbackRecoveryPending, NavCallbackArrivalPending;
            public static string NavCallbackResult, NavCallbackQueuedResult, RideDropOff, RideDropOffLot, ActiveTripName, RideDestinationKind, RideDestinationName = "test destination";
            public static readonly Reverse Reverse = new();
            internal static readonly NavigationRecoveryBudget NavigationRecovery = new();
            internal static readonly NavigationOperationPolicy Operation = new();
            public static int NextNavOrder() => ++NavOrder; public static long NextOperationId() => ++OperationId;
        }
        public static class SpikeCommands {
            public static bool EnsureAuthority() => true;
            public static bool HasAuthority() => true;
            public static bool IsOwnedTaxi(LandVehicle v) => ReferenceEquals(v, SpikeState.Vehicle);
            public static bool RequireOwnedTaxi(LandVehicle v, string why) => IsOwnedTaxi(v);
            public static bool SafeDriverPlayerPresent(LandVehicle v) => false;
            public static void EndRide(string reason) { SpikeState.ResetRide(); }
            public static string SafeStateName(LandVehicle v) => "native";
            public static string Fmt(Vector3 v) => $"{v.x},{v.y},{v.z}";
            public static void Print(string text) {} public static void EnsureDriverSeat(LandVehicle v) {}
            public static void CompletePickup(LandVehicle v, string why) {}
            public static void ReportCleanupFailure(string name, Exception ex) { Mod.Log.Error(ex.Message); }
            /* REQUEST */
            /* COMMANDS */
        }
        public static class SpikeRunner {
            private sealed class FinalRetreat { }
            /* FIELDS */
            /* RUNNER */
            private static bool PlayerDriver(LandVehicle v) => false;
            private static bool MainSceneLoaded() => true;
            private static void LogAgentProbe(string why) {}
            private static void UpdateDriveStartProof(LandVehicle v, Vector3 p, float d, float now) {}
            private static bool EvaluateProgress(Vector3 p, float distance, float now) => Native.Progress;
            private static bool TickStartupRecovery(VehicleAgent a, LandVehicle v, Vector3 p, float d, bool c) => false;
            private readonly record struct StaticBlocker;
            private static StaticBlocker? ObserveDetourBlocker(LandVehicle v, VehicleAgent a, Vector3 p) => null;
            private static bool TryStartDetour(LandVehicle v, VehicleAgent a, StaticBlocker blocker) => false;
            private static bool TryQueueDetourReverse(LandVehicle veh, VehicleAgent agent, StaticBlocker hit, float now) => false;
            private static bool CancelFinalDetourOnStall(LandVehicle v, VehicleAgent a) => false;
            private static bool TryFreeSpotRescue(LandVehicle v, VehicleAgent a, string why) => true;
            private static bool TryStartReverse(VehicleAgent a, LandVehicle v, float now, string why) => false;
            private static void TickReDispatch(VehicleAgent a) => throw new Exception("unexpected pending reverse");
            private static void RecoverStuck(VehicleAgent a, LandVehicle v, Vector3 p, float d, float elapsed, bool driving, bool budgetAlreadySpent = false) => throw new Exception("unexpected ordinary stall");
            private static void ResetSupervisorMotion() {}
            private static void ObserveSupervisorMotion(LandVehicle v, Vector3 p, float d, float now) {}
            private static bool TryCompleteBeforeRecovery(VehicleAgent a, LandVehicle v, Vector3 p, float d, string trigger, bool logDecision) => false;
            private static bool RecentForwardMotionUnjustifiesStall(bool readable, float speed, bool reversing) => false;
            private static bool HasCurrentForwardEvidence() => false;
            private static void LogSupervisorDecision(string trigger, string action, float distance, bool readable, float speed, bool recent, int before, int after) {}
            private static void CancelDeferredProbe() {}
            private static void RevokeReverse(string why) { SpikeState.Reverse.End(); }
            private static bool TryReadSignedSpeed(LandVehicle v, out float value) { value = v.Speed_Kmh; return true; }
            private static bool TryReadReversing(LandVehicle v, out bool value) { value = v.Agent.IsReversing; return true; }
            public static string Run(string scenario, string boundary) {
                Native.Calls.Clear(); Native.Callbacks.Clear(); Mod.Log.Errors.Clear();
                Native.ThrowAt = scenario.Contains("fallback") ? 1 : scenario.EndsWith("-throw") ? 2 : 0;
                Native.Progress = true;
                NavigationSettings.Constructions = 0; NavigationSettings.FailAt = 0; NavigationSettings.FailBoundary = boundary;
                if (scenario.StartsWith("failure-")) NavigationSettings.FailAt = scenario == "failure-initial" ? 1 : 2;
                Time.unscaledTime = 100; Time.timeScale = 1; Mod.Lifecycle.AcceptingWork = true;
                TaxiAI.Active = TaxiAI.NavigationBlocked = false;
                SpikeState.Vehicle = new(); Player.Local = new(); SpikeState.NavOrder = 40; SpikeState.OperationId = 0;
                SpikeState.Operation.Reset(); SpikeState.NavigationRecovery.Reset(); SpikeState.Reverse.End();
                SpikeState.PollingActive = SpikeState.NavCallbackRecoveryPending = SpikeState.NavCallbackArrivalPending = false;
                SpikeState.NavCallbackResult = SpikeState.NavCallbackQueuedResult = null;
                SpikeState.NavCallbackQueuedOrder = -1; SpikeState.NavCallbackQueuedOperation = -1;
                SpikeState.NavRetried = false; SpikeState.NavRetryAt = 0;
                _trafficWaiting = _recoveryOwnsNavigation = _selfTestActive = _driveStartConfirmed = false;
                _navCalcSince = 0;
                long generation = SpikeState.Operation.Generation;
                bool success = SpikeCommands.Go(0f, absoluteTarget: new Vector3 { x = 100 }, toPlayer: false);
                var veh = SpikeState.Vehicle; var agent = veh.Agent;
                if (scenario.Contains("redispatch") || scenario.Contains("polling") || scenario.StartsWith("patrol-")) {
                    SpikeState.NavigationRecovery.TrySpend(100, TaxiAI.PathingFailureCap);
                    SpikeState.NavigationRecovery.TrySpend(100, TaxiAI.PathingFailureCap);
                }
                if (scenario.StartsWith("redispatch") || scenario == "failure-redispatch") {
                    Time.unscaledTime = 101; ReDispatch(agent, SpikeState.NavTarget, "test recovery");
                    if (!scenario.StartsWith("failure-")) { Time.unscaledTime = 102; ReDispatch(agent, SpikeState.NavTarget, "repeated recovery"); }
                }
                if (scenario.StartsWith("polling") || scenario == "failure-polling") {
                    Time.unscaledTime = 103; TickNavigationPolling();
                    Time.unscaledTime = 103.5f; TickNavigationPolling();
                }
                if (scenario == "recovery") {
                    for (int i = 1; i <= 5; i++) { Time.unscaledTime = 100+i; TryBeginRecovery(agent, veh, maneuverPlanned: true); }
                }
                if (scenario.StartsWith("patrol-")) {
                    TaxiAI.Active = true;
                    if (scenario == "patrol-start") ArmPatrolSelfTest(SpikeState.NavOrder);
                    else Native.Progress = false;
                    Time.unscaledTime = 108; TickNavigationPolling();
                }
                if (scenario.Contains("redispatch") || scenario.Contains("polling")) Native.DeliverOldComplete();
                if (scenario.EndsWith("-expired") || scenario is "failure-redispatch" or "failure-polling") {
                    Time.unscaledTime = 190;
                    if (scenario.Contains("polling")) TickNavigationPolling();
                    else ReDispatch(agent, SpikeState.NavTarget, "expired recovery");
                }
                return JsonSerializer.Serialize(new {
                    Calls = Native.Calls, Success = success, Constructions = NavigationSettings.Constructions, Errors = Mod.Log.Errors,
                    Attempts = SpikeState.NavigationRecovery.Attempts, Operation = SpikeState.OperationId, Generation = SpikeState.Operation.Generation - generation,
                    QueuedOrder = SpikeState.NavCallbackQueuedOrder, QueuedOperation = SpikeState.NavCallbackQueuedOperation,
                    Callback = SpikeState.NavCallbackResult, NavStartTime = SpikeState.NavStartTime, RetryAt = SpikeState.NavRetryAt,
                    Retried = SpikeState.NavRetried, Polling = SpikeState.PollingActive, Terminal = SpikeState.Operation.Terminal, PatrolActive = TaxiAI.Active,
                    ExpiredBefore = SpikeState.NavigationRecovery.HasExpired(189.99f), ExpiredAt = SpikeState.NavigationRecovery.HasExpired(190)
                });
            }
        }
        }
        """;
}

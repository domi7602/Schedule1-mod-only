using System.Reflection;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Executes the production polling caller and recovery method together with the real
/// arrival, progress and recurrence policies. Unity/native boundaries and time are stubbed.
/// This does not simulate pathfinding and does not prove a physical Barn arrival.
/// </summary>
public class SupervisorArrivalRecoveryTests
{
    private static readonly Lazy<MethodInfo> Replay = new(CompileReplay);

    [Fact]
    public void ResumedForwardMotion_DuringProbeWait_CancelsWithoutEpisodeDispatchOrHistoryReset()
    {
        Snapshot result = Run("resume-during-wait");
        Assert.Equal(result.ScheduledAttempts, result.Attempts);
        Assert.Equal(0, result.Navigates);
        Assert.Equal(0, result.Terminals);
        Assert.False(result.SuccessArrival);
        Assert.False(result.ProbePending);
        Assert.True(result.HistoryRecurring);
        Assert.True(result.LocalProgress);
        Assert.False(result.ExpiredBefore);
        Assert.True(result.ExpiredAt);
        Assert.Equal(2, result.OperationId);
        Assert.Contains(result.Logs, line => line.Contains("trigger=deferred-probe-cancel", StringComparison.Ordinal)
            && line.Contains("recentMotion=True", StringComparison.Ordinal)
            && line.Contains("action=cancel-without-spend", StringComparison.Ordinal));
    }

    [Fact]
    public void MovedBeyondAnchor_DoesNotSpendThenDiscardAndStallAgain()
    {
        Snapshot forward = Run("beyond-anchor-forward");
        Assert.Equal(forward.ScheduledAttempts, forward.Attempts);
        Assert.Equal(0, forward.Navigates);
        Assert.Equal(0, forward.Terminals);
        Assert.False(forward.SuccessArrival);

        Snapshot lost = Run("beyond-anchor-lost-return");
        Assert.Equal(lost.ScheduledAttempts + 1, lost.Attempts);
        Assert.True(lost.Navigates >= 1);
        Assert.Equal(0, lost.Terminals);
        Assert.False(lost.SuccessArrival);
        Assert.False(lost.ProbePending);
        Assert.Equal(2, lost.OperationId);
        Assert.Equal(165.3f, lost.TargetX);
    }

    [Fact]
    public void TangentialRecentForwardSpeed_IsNotAFreshStallWhenLongWindowAverageIsSlow()
    {
        Snapshot result = Run("tangential-recent-speed");
        Assert.Equal(0, result.Attempts);
        Assert.Equal(0, result.Navigates);
        Assert.Equal(0, result.Terminals);
        Assert.False(result.SuccessArrival);
        Assert.False(result.ExpiredBefore);
        Assert.True(result.ExpiredAt);
    }

    [Fact]
    public void GenuineStationaryRefusal_StillGetsOneDelayedProbeAndExistingTerminal()
    {
        Snapshot result = Run("stationary-refusal");
        Assert.Equal(2, result.Attempts);
        Assert.Equal(1, result.Terminals);
        Assert.Equal(0, result.Navigates);
        Assert.Equal(2, result.Rescues);
        Assert.False(result.SuccessArrival);
        Assert.False(result.Polling);
        Assert.NotEqual("Complete", result.Callback);
        Assert.Equal(0, result.Parks);
        Assert.Equal(0, result.Boards);
    }

    [Fact]
    public void DirectNoCallbackArrival_CompletesOnceBeforeRecoveryWhenHaltedInsideRadius()
    {
        Snapshot result = Run("direct-arrival");
        Assert.True(result.SuccessArrival);
        Assert.False(result.Failed);
        Assert.False(result.Polling);
        Assert.Equal(0, result.Attempts);
        Assert.Equal(0, result.Navigates);
        Assert.Equal(0, result.Terminals);
        Assert.Equal(0, result.Parks);
        Assert.Equal(0, result.Boards);
        Assert.NotEqual("Complete", result.Callback);
        Assert.True(result.Stops >= 1);
        Assert.Equal(result.OrderAtStop, result.Order);
        Assert.True(result.OwnedDuringStop);
        Assert.Equal(5, result.Order);
        Assert.Equal(2, result.OperationId);
        Assert.Equal(165.3f, result.TargetX);
    }

    [Fact]
    public void InitiallyParkedInsideRadius_WithoutForwardEvidence_DoesNotComplete()
    {
        Snapshot result = Run("parked-inside");
        Assert.False(result.SuccessArrival);
        Assert.NotEqual("Complete", result.Callback);
        Assert.Equal(0, result.Parks);
        Assert.Equal(0, result.Boards);
    }

    [Fact]
    public void MovingInsideRadius_DoesNotCompleteOrPark()
    {
        Snapshot result = Run("moving-inside");
        Assert.False(result.SuccessArrival);
        Assert.Equal(0, result.Parks);
        Assert.Equal(0, result.Boards);
        Assert.Equal(0, result.Attempts);
        Assert.Equal(0, result.Navigates);
        Assert.NotEqual("Complete", result.Callback);
    }

    [Fact]
    public void SpikeReverseAutoDrivingOrPlacement_DoNotCancelARealStall()
    {
        Snapshot spike = Run("single-spike");
        Assert.Equal(1, spike.Attempts);
        Assert.True(spike.Navigates >= 1);
        Assert.False(spike.SuccessArrival);

        Snapshot reverse = Run("reverse-oscillation");
        Assert.Equal(1, reverse.Attempts);
        Assert.False(reverse.SuccessArrival);

        Snapshot flag = Run("autodriving-flag-only");
        Assert.Equal(1, flag.Attempts);
        Assert.False(flag.SuccessArrival);

        Snapshot placed = Run("artificial-placement");
        Assert.Equal(placed.ScheduledAttempts + 1, placed.Attempts);
        Assert.True(placed.Navigates >= 1);
        Assert.Equal(0, placed.Terminals);
        Assert.False(placed.SuccessArrival);
    }

    [Fact]
    public void PatrolReverseOnlyProof_DoesNotCompleteArrival()
    {
        Snapshot result = Run("patrol-reverse-proof");
        Assert.False(result.SuccessArrival);
        Assert.Equal(0, result.Attempts);
        Assert.Equal(0, result.Parks);
        Assert.Equal(0, result.Boards);
    }

    [Fact]
    public void RecoveryRedispatch_RetainsOperationBudgetGoalAndStreetPoint()
    {
        Snapshot result = Run("recovery-retains");
        Assert.Equal(2, result.OperationId);
        Assert.Equal(1, result.Attempts);
        Assert.Equal(165.3f, result.TargetX);
        Assert.True(result.StreetKept);
        Assert.Equal(165.3f, result.StreetX);
        Assert.False(result.ExpiredBefore);
        Assert.True(result.ExpiredAt);
        Assert.True(result.Navigates >= 1);
        Assert.False(result.SuccessArrival);
    }

    private static Snapshot Run(string scenario) =>
        JsonSerializer.Deserialize<Snapshot>((string)Replay.Value.Invoke(null, new object[] { scenario })!)!;

    private sealed record Snapshot(
        int Attempts, int ScheduledAttempts, bool Terminal, bool Failed, bool Polling, bool SuccessArrival,
        int Navigates, int Rescues, int Terminals, int Stops, int Parks, int Boards,
        long OperationId, int Order, int OrderAtStop, bool OwnedDuringStop, float TargetX, float StreetX,
        bool StreetKept, bool HistoryRecurring, bool LocalProgress, bool ProbePending,
        bool ExpiredBefore, bool ExpiredAt, string? Callback, string[] Logs);

    private static MethodInfo CompileReplay()
    {
        var runner = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs")).GetRoot();
        var runnerClass = runner.DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Single(type => type.Identifier.Text == "SpikeRunner");
        var policies = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs")).GetRoot();
        string[] methodNames =
        {
            "TickNavigationPolling", "TryBeginRecovery", "EvaluateProgress", "UpdateDriveStartProof",
            "ObserveArtificialPlacement", "RecoverStuck", "ArmProgressWindow", "StopIfNavigationExpired",
            "TryReadSignedSpeed", "TryReadReversing", "PlayerDriver", "TickPatrolSupervision", "ReDispatch",
            "ClearPendingArrival", "ObserveSupervisorMotion", "ObserveForwardEvidence", "ObserveRecentMotion",
            "HasCurrentForwardEvidence", "RecentForwardMotionUnjustifiesStall", "TryCompleteBeforeRecovery",
            "LogSupervisorDecision", "CancelDeferredProbe", "SameAuthorizedTarget"
        };
        string[] policyNames =
        {
            "NavigationRecoveryBudget", "RecoveryRecurrencePolicy", "NavigationArrivalPolicy",
            "SupervisedArrivalPolicy", "InitialDispatchRecoveryPolicy", "ArrivalConfirmationPolicy", "ArrivalGateOutcome",
            "NavigationOperationPolicy", "OperationPhase", "NavigationTerminalPolicy", "PatrolStartPolicy", "TrafficWaitBudget"
        };
        string source = Harness
            .Replace("/* FIELDS */", string.Join("\n", runnerClass.Members.OfType<FieldDeclarationSyntax>()
                .Select(field => field.ToFullString())))
            .Replace("/* METHODS */", string.Join("\n", runner.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(method => methodNames.Contains(method.Identifier.Text)).Select(method => method.ToFullString())))
            .Replace("/* POLICIES */", string.Join("\n", policies.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
                .Where(type => policyNames.Contains(type.Identifier.Text)).Select(type => type.ToFullString())));
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("TaxiSupervisorArrivalReplay",
            new[] { CSharpSyntaxTree.ParseText(source) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.SpikeRunner")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        using System.Collections.Generic;
        using System.Reflection;
        using System.Text.Json;
        namespace Replay {
        /* POLICIES */
        public struct Vector3 {
            public float x, y, z;
            public static Vector3 zero => new();
            public static float Distance(Vector3 a, Vector3 b) {
                float dx = a.x - b.x, dy = a.y - b.y, dz = a.z - b.z;
                return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
            }
        }
        public sealed class Transform { public Vector3 position, forward; public object rotation = new(); }
        public sealed class LandVehicle {
            public IntPtr Pointer = new(1);
            public Transform transform = new();
            public VehicleAgent Agent = new();
            public object DriverPlayer;
            public float Speed_Kmh, currentThrottle;
            public bool IsReversing;
        }
        public sealed class NavigationSettings { }
        public sealed class VehicleAgent {
            public IntPtr Pointer = new(2);
            public enum ENavigationResult { Failed, Complete, Stopped }
            public bool AutoDriving, NavigationCalculationInProgress, IsReversing;
            public bool IsOnVehicleGraph() => true;
            public bool GetIsStuck() => false;
            public void StopNavigating() {
                SpikeRunner.SeenStops++;
                SpikeRunner.StopOrder = SpikeState.NavOrder;
                SpikeRunner.OwnedDuringStop = SpikeRunner.RecoveryOwns;
                AutoDriving = false;
            }
            public void Navigate(Vector3 target, NavigationSettings settings, Action<ENavigationResult> callback) {
                SpikeRunner.SeenNavigates++;
            }
        }
        public static class Time { public static float unscaledTime = 100, timeScale = 1, unscaledDeltaTime; public static int frameCount; }
        public static class Mathf { public static float Min(float a, float b) => MathF.Min(a, b); public static float Abs(float v) => MathF.Abs(v); }
        public static class Mod {
            public static readonly Logger Log = new();
            public static readonly Life Lifecycle = new();
        }
        public sealed class Life { public bool AcceptingWork = true; }
        public sealed class Logger {
            public void Info(string text) { SpikeRunner.Logs.Add(text); }
            public void Warn(string text) { SpikeRunner.Logs.Add(text); }
            public void Error(string text) { SpikeRunner.Logs.Add(text); }
        }
        public static class TaxiLog { public static void Verbose(string text) { } }
        public static class TaxiDestinations {
            public static string Num(float value) => value.ToString("0.0");
            public static bool TryFallbackDropOff(Vector3 goal, Vector3 current, out Vector3 fallback, out string lot) {
                fallback = current; lot = ""; return false;
            }
        }
        public static class NativeActuatorPatch {
            public static int Parks;
            public static void ReleasePark(LandVehicle veh) { }
            public static void RequestPark(LandVehicle veh) { Parks++; }
        }
        public static class CleanupGuard {
            public static void Run(string name, Action action, Action<string, Exception> report) {
                try { action(); } catch (Exception ex) { report(name, ex); }
            }
        }
        public static class RoadKeeper {
            public static bool HasObstacle(LandVehicle veh, Vector3 position, object rotation, bool staticOnly = false) => false;
        }
        public static class TaxiAI {
            public static bool NavigationBlocked, Active;
            public const int PathingFailureCap = 4;
            public static int CurrentWaypoint => 0;
            public static int WaypointCount => 1;
            public static int StopPatrols;
            public static void StopPatrol(string why) { StopPatrols++; Active = false; }
        }
        public sealed class ReverseState { public bool Active; public void End() { Active = false; } }
        public static class SpikeState {
            public static LandVehicle Vehicle;
            public static Vector3 NavTarget, NavStartPosition, NavReDispatchTarget, ProgressFrom;
            public static Vector3? RideDestination, RideGoal, RideStreetCandidate, ActiveTripPoint;
            public static float NavStartDistance, NavStartTime, LastHeartbeat, NavRetryAt, NavReDispatchAt, ProgressWindowStart;
            public static bool PollingActive, NavEverAutoDriving, NavRetried, NavToPlayer, NavGaveUp, RideArrived, RideAwaitingBoard, RideActive, RideDestinationPicked;
            public static int NavOrder, StuckRecoveries, NavCallbackQueuedOrder = -1, NavCallbackRecoveryOrder = -1, NavCallbackArrivalOrder = -1;
            public static long OperationId, NavCallbackQueuedOperation = -1, NavCallbackRecoveryOperation = -1, NavCallbackArrivalOperation = -1;
            public static bool NavCallbackRecoveryPending, NavCallbackArrivalPending;
            public static float NavCallbackArrivalAt;
            public static string NavCallbackResult, NavCallbackQueuedResult, RideDropOff, RideDropOffLot, RideDestinationName = "Barn";
            public static readonly ReverseState Reverse = new();
            internal static readonly NavigationRecoveryBudget NavigationRecovery = new();
            internal static readonly NavigationOperationPolicy Operation = new();
            public static int NextNavOrder() => ++NavOrder;
            public static long NextOperationId() => ++OperationId;
        }
        public static class SpikeCommands {
            public sealed class DriveRequest { }
            public static bool HasAuthority() => true;
            public static bool IsOwnedTaxi(LandVehicle veh) => ReferenceEquals(veh, SpikeState.Vehicle);
            public static string Fmt(Vector3 value) => value.x + "," + value.y + "," + value.z;
            public static bool EnsureDriverSeat(LandVehicle veh) => false;
            public static NavigationSettings CreateNavigationSettings() => new();
            public static void CompletePickup(LandVehicle veh, string note) { SpikeRunner.SeenBoards++; NativeActuatorPatch.RequestPark(veh); }
            public static void ReportCleanupFailure(string name, Exception ex) { }
        }
        public static class SpikeRunner {
            /* FIELDS */
            public static readonly List<string> Logs = new();
            public static int SeenNavigates, SeenRescues, SeenTerminals, SeenStops, SeenBoards, StopOrder, ScheduledAttempts;
            public static bool OwnedDuringStop;
            public static bool RecoveryOwns => _recoveryOwnsNavigation;
            private static void GiveUpNavigation(VehicleAgent agent, Vector3 position, float distance, float elapsed) {
                SeenTerminals++;
                SpikeState.PollingActive = false;
                SpikeState.NavGaveUp = true;
                SpikeState.Operation.Terminate("navigation gave up", failed: true);
            }
            private static bool TryTeleportRecovery(LandVehicle veh, VehicleAgent agent, Vector3 position, float distance) => false;
            private static bool TryFreeSpotRescue(LandVehicle veh, VehicleAgent agent, string context) { SeenRescues++; return false; }
            private readonly struct StaticBlocker { }
            private sealed class DetourDrive { }
            private sealed class FinalRetreat { }
            private static StaticBlocker? ObserveDetourBlocker(LandVehicle veh, VehicleAgent agent, Vector3 position) => null;
            private static bool TryStartDetour(LandVehicle veh, VehicleAgent agent, StaticBlocker blocker) => false;
            private static bool TryQueueDetourReverse(LandVehicle veh, VehicleAgent agent, StaticBlocker hit, float now) => false;
            private static bool CancelFinalDetourOnStall(LandVehicle veh, VehicleAgent agent) => false;
            private static bool TryStartReverse(VehicleAgent agent, LandVehicle veh, float now, string reason) => false;
            private static void TickReDispatch(VehicleAgent agent) { SpikeState.NavReDispatchAt = 0f; }
            private static bool TickStartupRecovery(VehicleAgent agent, LandVehicle veh, Vector3 position, float distance, bool navCalc) => false;
            private static void LogAgentProbe(string context) { }
            private static void RevokeReverse(string reason) { SpikeState.Reverse.End(); }
            internal static Action<VehicleAgent.ENavigationResult> NavigationCallbackFor(int order) => result => { };
            private static void ResetOptionalFields() {
                foreach (FieldInfo field in typeof(SpikeRunner).GetFields(BindingFlags.NonPublic | BindingFlags.Static)) {
                    if (field.IsLiteral) continue;
                    if (field.Name.StartsWith("_forward", StringComparison.Ordinal) || field.Name.StartsWith("_recent", StringComparison.Ordinal)
                        || field.Name.StartsWith("_motion", StringComparison.Ordinal) || field.Name.StartsWith("_supervisor", StringComparison.Ordinal))
                        field.SetValue(null, field.FieldType.IsValueType ? Activator.CreateInstance(field.FieldType) : null);
                }
            }
            private static void Boot(Vector3 position, float speed, float throttle, bool autoDriving) {
                Logs.Clear();
                SeenNavigates = SeenRescues = SeenTerminals = SeenStops = SeenBoards = StopOrder = ScheduledAttempts = 0;
                OwnedDuringStop = false;
                NativeActuatorPatch.Parks = 0;
                TaxiAI.StopPatrols = 0;
                TaxiAI.Active = TaxiAI.NavigationBlocked = false;
                Time.unscaledTime = 100f; Time.timeScale = 1f; Time.frameCount = 1;
                Mod.Lifecycle.AcceptingWork = true;
                SpikeState.Vehicle = new LandVehicle();
                SpikeState.Vehicle.transform.position = position;
                SpikeState.Vehicle.Speed_Kmh = speed;
                SpikeState.Vehicle.currentThrottle = throttle;
                SpikeState.Vehicle.Agent.AutoDriving = autoDriving;
                SpikeState.NavTarget = new Vector3 { x = 165.3f, y = 0f, z = 25.1f };
                SpikeState.OperationId = 2;
                SpikeState.NavOrder = 4;
                SpikeState.Operation.Reset();
                SpikeState.Operation.Begin();
                SpikeState.NavigationRecovery.Reset();
                SpikeState.NavigationRecovery.Begin(100f, 90f);
                SpikeState.PollingActive = true;
                SpikeState.NavGaveUp = false;
                SpikeState.NavToPlayer = false;
                SpikeState.RideAwaitingBoard = false;
                SpikeState.RideActive = false;
                SpikeState.RideDestinationPicked = false;
                SpikeState.RideStreetCandidate = null;
                SpikeState.NavCallbackResult = null;
                SpikeState.NavCallbackArrivalPending = false;
                SpikeState.NavCallbackRecoveryPending = false;
                SpikeState.Reverse.Active = false;
                SpikeState.LastHeartbeat = 0f;
                SpikeState.NavStartTime = 100f;
                SpikeState.ProgressWindowStart = 0f;
                SpikeState.StuckRecoveries = 0;
                _trafficWaiting = _recoveryOwnsNavigation = _startupArmed = _selfTestActive = _driveStartConfirmed = false;
                _recurringProbeAt = 0f;
                _recurringProbeUsed = false;
                _localRecoveryProgress = false;
                _proofOrder = -1;
                _navCalcSince = 0f;
                _windowStartDistance = _windowBestDistance = 0f;
                RecoveryRecurrence.Begin(Vector3.Distance(position, SpikeState.NavTarget));
                ResetOptionalFields();
            }
            private static void Place(float x, float speed, float throttle, bool reversing = false) {
                SpikeState.Vehicle.transform.position = new Vector3 { x = x, y = 0f, z = 25.1f };
                SpikeState.Vehicle.Speed_Kmh = speed;
                SpikeState.Vehicle.currentThrottle = throttle;
                SpikeState.Vehicle.IsReversing = reversing;
                SpikeState.Vehicle.Agent.IsReversing = reversing;
            }
            private static void Poll(float now) {
                Time.unscaledTime = now;
                Time.frameCount++;
                TickNavigationPolling();
            }
            private static int ScheduleProbe(float x) {
                Place(x, 0f, 1f);
                RecoveryRecurrence.RegisterStall(x, 25.1f);
                Time.unscaledTime = 100f;
                TryBeginRecovery(SpikeState.Vehicle.Agent, SpikeState.Vehicle, true);
                ScheduledAttempts = SpikeState.NavigationRecovery.Attempts;
                return ScheduledAttempts;
            }
            private static bool HistoryStillRecurring(float x) {
                var policy = RecoveryRecurrence;
                var field = typeof(RecoveryRecurrencePolicy).GetField("_hasStall", BindingFlags.NonPublic | BindingFlags.Instance);
                return field != null && (bool)field.GetValue(policy)!;
            }
            public static string Run(string scenario) {
                float origin = 165.3f - 20f;
                Boot(new Vector3 { x = origin, y = 0f, z = 25.1f }, 0f, 0f, true);
                if (scenario == "resume-during-wait") {
                    Place(160.7f, 0f, 1f);
                    ScheduleProbe(160.7f);
                    _localRecoveryProgress = true;
                    for (int i = 1; i <= 4; i++) {
                        Place(160.7f + i * 0.4f, 12f, 1f);
                        Poll(100.5f + (i - 1) * 0.5f);
                    }
                    Place(162.3f, 12f, 1f);
                    Poll(102.5f);
                }
                else if (scenario == "beyond-anchor-forward") {
                    ScheduleProbe(0f);
                    Place(0f, 12f, 1f);
                    Poll(100.5f);
                    Place(3f, 12f, 1f);
                    Poll(101f);
                    Place(7.2f, 12f, 1f);
                    Poll(102.5f);
                    Poll(103f);
                }
                else if (scenario == "beyond-anchor-lost-return") {
                    ScheduleProbe(0f);
                    SpikeState.ProgressFrom = new Vector3 { x = 0f, y = 0f, z = 25.1f };
                    SpikeState.ProgressWindowStart = 90f;
                    _windowStartDistance = _windowBestDistance = Vector3.Distance(SpikeState.ProgressFrom, SpikeState.NavTarget);
                    Place(7.2f, 0f, 0f);
                    Poll(102.5f);
                    Poll(103f);
                }
                else if (scenario == "tangential-recent-speed") {
                    Place(150f, 12f, 1f);
                    SpikeState.Vehicle.transform.position = new Vector3 { x = 150f, y = 0f, z = 25.1f };
                    Poll(100f);
                    Place(150f, 12f, 1f);
                    SpikeState.Vehicle.transform.position = new Vector3 { x = 150f, y = 1f, z = 25.1f };
                    Poll(100.6f);
                    SpikeState.ProgressFrom = new Vector3 { x = 150f, y = 0f, z = 25.1f };
                    SpikeState.ProgressWindowStart = 90f;
                    _windowStartDistance = _windowBestDistance = Vector3.Distance(SpikeState.Vehicle.transform.position, SpikeState.NavTarget);
                    Poll(101.2f);
                }
                else if (scenario == "stationary-refusal") {
                    Place(160.3f, 0f, 1f);
                    ScheduleProbe(160.3f);
                    Poll(101f);
                    Poll(102.5f);
                }
                else if (scenario == "direct-arrival") {
                    Place(53.3f, 30f, 1f);
                    Poll(100f);
                    Place(65.3f, 30f, 1f);
                    Poll(100.5f);
                    Place(160.3f, 0f, 0f);
                    Poll(101f);
                    Poll(101.5f);
                }
                else if (scenario == "parked-inside") {
                    Place(160.3f, 0f, 0f);
                    Poll(100f);
                    Poll(106f);
                }
                else if (scenario == "moving-inside") {
                    Place(160.3f, 17.4f, 1f);
                    Poll(100f);
                    Poll(100.6f);
                    SpikeState.ProgressFrom = new Vector3 { x = 160.8f, y = 0f, z = 25.1f };
                    SpikeState.ProgressWindowStart = 90f;
                    _windowStartDistance = _windowBestDistance = 5f;
                    Place(160.3f, 17.4f, 1f);
                    Poll(101.2f);
                }
                else if (scenario == "single-spike") {
                    Place(140f, 40f, 1f);
                    SpikeState.ProgressFrom = SpikeState.Vehicle.transform.position;
                    SpikeState.ProgressWindowStart = 90f;
                    _windowStartDistance = _windowBestDistance = Vector3.Distance(SpikeState.ProgressFrom, SpikeState.NavTarget);
                    Poll(100f);
                }
                else if (scenario == "reverse-oscillation") {
                    Place(140f, -8f, 1f, reversing: true);
                    SpikeState.Reverse.Active = true;
                    SpikeState.ProgressFrom = new Vector3 { x = 147f, y = 0f, z = 25.1f };
                    SpikeState.ProgressWindowStart = 90f;
                    _windowStartDistance = _windowBestDistance = Vector3.Distance(SpikeState.Vehicle.transform.position, SpikeState.NavTarget);
                    Poll(100f);
                }
                else if (scenario == "autodriving-flag-only") {
                    Place(140f, 0f, 0f);
                    SpikeState.Vehicle.Agent.AutoDriving = true;
                    SpikeState.ProgressFrom = SpikeState.Vehicle.transform.position;
                    SpikeState.ProgressWindowStart = 90f;
                    _windowStartDistance = _windowBestDistance = Vector3.Distance(SpikeState.ProgressFrom, SpikeState.NavTarget);
                    Poll(100f);
                }
                else if (scenario == "artificial-placement") {
                    ScheduleProbe(0f);
                    SpikeState.ProgressFrom = new Vector3 { x = 0f, y = 0f, z = 25.1f };
                    SpikeState.ProgressWindowStart = 90f;
                    ObserveArtificialPlacement(new Vector3 { x = 8f, y = 0f, z = 25.1f });
                    Place(8f, 0f, 0f);
                    Poll(102.5f);
                    Poll(103f);
                }
                else if (scenario == "patrol-reverse-proof") {
                    TaxiAI.Active = true;
                    _proofOrder = SpikeState.NavOrder;
                    _driveStartConfirmed = true;
                    Place(160.3f, 0f, 0f);
                    SpikeState.LastHeartbeat = 0f;
                    Poll(100f);
                }
                else if (scenario == "recovery-retains") {
                    SpikeState.RideActive = true;
                    SpikeState.RideDestinationPicked = true;
                    SpikeState.RideStreetCandidate = SpikeState.NavTarget;
                    SpikeState.RideGoal = SpikeState.NavTarget;
                    Place(140f, 0f, 1f);
                    SpikeState.ProgressFrom = SpikeState.Vehicle.transform.position;
                    SpikeState.ProgressWindowStart = 90f;
                    _windowStartDistance = _windowBestDistance = Vector3.Distance(SpikeState.ProgressFrom, SpikeState.NavTarget);
                    Poll(100f);
                }
                float street = SpikeState.RideStreetCandidate.HasValue ? SpikeState.RideStreetCandidate.Value.x : -1f;
                return JsonSerializer.Serialize(new {
                    Attempts = SpikeState.NavigationRecovery.Attempts,
                    ScheduledAttempts,
                    Terminal = SpikeState.Operation.Terminal,
                    Failed = SpikeState.Operation.Failed,
                    Polling = SpikeState.PollingActive,
                    SuccessArrival = !SpikeState.PollingActive && SpikeState.Operation.Terminal && !SpikeState.Operation.Failed && !SpikeState.NavGaveUp,
                    Navigates = SeenNavigates,
                    Rescues = SeenRescues,
                    Terminals = SeenTerminals,
                    Stops = SeenStops,
                    Parks = NativeActuatorPatch.Parks,
                    Boards = SeenBoards,
                    OperationId = SpikeState.OperationId,
                    Order = SpikeState.NavOrder,
                    OrderAtStop = StopOrder,
                    OwnedDuringStop,
                    TargetX = SpikeState.NavTarget.x,
                    StreetX = street,
                    StreetKept = SpikeState.RideStreetCandidate.HasValue && MathF.Abs(street - SpikeState.NavTarget.x) < 0.01f,
                    HistoryRecurring = HistoryStillRecurring(160.7f),
                    LocalProgress = _localRecoveryProgress,
                    ProbePending = _recurringProbeAt > 0f,
                    ExpiredBefore = SpikeState.NavigationRecovery.HasExpired(189.99f),
                    ExpiredAt = SpikeState.NavigationRecovery.HasExpired(190f),
                    Callback = SpikeState.NavCallbackResult,
                    Logs = Logs.ToArray()
                });
            }
            /* METHODS */
        }
        }
        """;
}

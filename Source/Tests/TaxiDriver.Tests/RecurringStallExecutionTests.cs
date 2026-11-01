using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Executes the actual runner recovery method, not a transcription of its decision tree.
/// Only Unity/native boundaries are substituted. This does not simulate native pathfinding.
/// </summary>
public class RecurringStallExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(CompileReplay);

    [Fact]
    public void FailedRecurringRescue_DoesNotRedispatchUnchangedAtEachHeartbeat()
    {
        // Docks trace: a second stall has already spent two episodes, then the same
        // location fails free-spot rescue at .5-second intervals with no forward progress.
        int[] result = Run(false);
        Assert.Equal(0, result[0]); // no unchanged Navigate after a refused rescue
        Assert.Equal(0, result[1]); // bounded placement re-probe is pending, not early terminal
        Assert.Equal(3, result[2]); // spent episode retained, not reset or burned to cap
        Assert.Equal(1, result[3]); // one checked rescue
        Assert.Equal(0, result[4]); // no duplicate terminal graph rescue of the same spot
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RefusedProbe_ThenSupportedProbe_PreservesRecoveryOpportunity(bool nativeFailed)
    {
        int[] result = Run(false, ticks: 6, initialAttempts: 1, interrupt: "second-success", nativeFailed: nativeFailed);
        Assert.Equal(new[] { 1, 0, 3, 2, 0 }, result);
    }

    [Fact]
    public void RepeatedRefusal_EndsAfterOneDelayedProbeWithoutRedispatch()
    {
        int[] result = Run(false, ticks: 10);
        Assert.Equal(new[] { 0, 1, 4, 2, 0 }, result);
    }

    [Fact]
    public void SuccessfulRecurringRescue_RedispatchesWithoutRefillingBudget()
    {
        int[] result = Run(true, ticks: 1);
        Assert.Equal(new[] { 1, 0, 3, 1, 0 }, result);
    }

    [Fact]
    public void ExhaustedOperation_StillUsesOnlyOneTerminalRescue()
    {
        int[] result = Run(false, initialAttempts: 4);
        Assert.Equal(new[] { 0, 1, 4, 0, 1 }, result);
    }

    [Theory]
    [InlineData("stop-patrol-stop")]
    [InlineData("stop-patrol-reroute")]
    [InlineData("stop-patrol-teardown")]
    [InlineData("seat-stop")]
    [InlineData("seat-reroute")]
    [InlineData("seat-teardown")]
    [InlineData("rescue-stop")]
    [InlineData("rescue-reroute")]
    [InlineData("rescue-teardown")]
    public void InterruptedRecovery_DoesNotTerminateOrDispatchFromTheOldStack(string interrupt)
    {
        foreach (bool rescued in new[] { false, true })
        {
            int[] result = Run(rescued, ticks: 1, interrupt: interrupt);
            Assert.Equal(0, result[0]);
            Assert.Equal(0, result[1]);
            Assert.Equal(3, result[2]);
            Assert.Equal(interrupt.StartsWith("rescue-") ? 1 : 0, result[3]);
        }
    }

    [Theory]
    [InlineData("wait-stop")]
    [InlineData("wait-reroute")]
    [InlineData("wait-teardown")]
    public void DelayedProbe_RejectsSupersededIdentity(string interrupt)
    {
        Assert.Equal(new[] { 0, 0, 3, 1, 0 }, Run(false, ticks: 6, interrupt: interrupt));
    }

    [Fact]
    public void DelayedProbe_CannotExtendOperationDeadline()
    {
        Assert.Equal(new[] { 0, 1, 3, 1, 0 }, Run(false, ticks: 6, interrupt: "deadline"));
    }

    private static int[] Run(bool rescue, int ticks = 2, int initialAttempts = 2, string interrupt = "", bool nativeFailed = false) =>
        (int[])Replay.Value.Invoke(null, new object[] { rescue, ticks, initialAttempts, interrupt, nativeFailed })!;

    private static MethodInfo CompileReplay()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        var tree = CSharpSyntaxTree.ParseText(runner);
        string method = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Single(m => m.Identifier.Text == "TryBeginRecovery").ToFullString();
        string policies = DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs");
        var policyTree = CSharpSyntaxTree.ParseText(policies);
        string classes = string.Join("\n", policyTree.GetRoot().DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(c => c.Identifier.Text is "NavigationRecoveryBudget" or "RecoveryRecurrencePolicy")
            .Select(c => c.ToFullString()));
        string source = Harness.Replace("/* RECOVERY_METHOD */", method).Replace("/* POLICIES */", classes);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("TaxiRecurringStallReplay",
            new[] { CSharpSyntaxTree.ParseText(source) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.SpikeRunner")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        namespace Replay {
        /* POLICIES */
        public struct Vector3 {
            public float x, z;
            public static Vector3 zero => new();
            public static float Distance(Vector3 a, Vector3 b) => 201.6f;
        }
        public sealed class Transform { public Vector3 position; }
        public sealed class LandVehicle {
            public readonly Transform transform = new();
            public IntPtr Pointer = new(1);
        }
        public sealed class VehicleAgent { }
        public static class Time { public static float unscaledTime; }
        public static class Mod {
            public static readonly Logger Log = new();
            public static readonly Life Lifecycle = new();
        }
        public sealed class Life { public bool AcceptingWork = true; }
        public sealed class Logger { public void Warn(string text) { } }
        public sealed class OperationState { public bool Active = true; public bool Terminal => !Active; }
        public sealed class ReverseState { public bool Active; }
        public static class SpikeState {
            public static bool PollingActive;
            public static long OperationId;
            public static int NavOrder;
            public static LandVehicle Vehicle = new();
            public static Vector3 NavTarget;
            public static float NavStartTime;
            public static string NavCallbackResult;
            public static readonly OperationState Operation = new();
            internal static readonly NavigationRecoveryBudget NavigationRecovery = new();
            public static readonly ReverseState Reverse = new();
        }
        public static class TaxiAI {
            public static bool NavigationBlocked;
            public const int PathingFailureCap = 4;
            public static void StopPatrol(string why) { SpikeRunner.InterruptAt("stop-patrol"); }
        }
        public static class SpikeCommands {
            public static string Fmt(Vector3 v) => "(-144.1, -3.3, 127.0)";
            public static void EnsureDriverSeat(LandVehicle v) { SpikeRunner.InterruptAt("seat"); }
            public static bool HasAuthority() => true;
            public static bool IsOwnedTaxi(LandVehicle v) => ReferenceEquals(v, SpikeState.Vehicle);
        }
        public static class SpikeRunner {
            private static bool _trafficWaiting, _localRecoveryProgress, _recoveryOwnsNavigation;
            private static bool _supervisorSampleReadable, _supervisorSampleReversing;
            private static float _supervisorSampleSpeed;
            private static float _recurringProbeAt;
            private static bool _recurringProbeUsed;
            private static long _recurringProbeOperation;
            private static int _recurringProbeOrder;
            private static IntPtr _recurringProbePointer;
            private static readonly RecoveryRecurrencePolicy RecoveryRecurrence = new();
            private static int dispatched, terminal, rescues, terminalRescues;
            private static bool rescued;
            private static string interruption = "";
            private static bool PlayerDriver(LandVehicle v) => false;
            private static bool TryCompleteBeforeRecovery(VehicleAgent a, LandVehicle v, Vector3 p, float d, string trigger, bool logDecision) => false;
            private static bool RecentForwardMotionUnjustifiesStall(bool readable, float speed, bool reversing) => false;
            private static void LogSupervisorDecision(string trigger, string action, float distance, bool readable, float speed, bool recent, int before, int after) {}
            private static void CancelDeferredProbe() { _recurringProbeAt = 0f; }
            private static bool MainSceneLoaded() => true;
            private static bool StopIfNavigationExpired(VehicleAgent a, LandVehicle v, float now) {
                if (!SpikeState.NavigationRecovery.HasExpired(now)) return false;
                GiveUpNavigation(a, v.transform.position, 201.6f, now);
                return true;
            }
            private static bool TryFreeSpotRescue(LandVehicle v, VehicleAgent a, string why) {
                rescues++;
                InterruptAt("rescue");
                return rescued || (interruption == "second-success" && rescues == 2);
            }
            private static void ReDispatch(VehicleAgent a, Vector3 target, string why) { dispatched++; }
            private static bool TryTeleportRecovery(LandVehicle v, VehicleAgent a, Vector3 p, float distance) {
                terminalRescues++;
                SpikeState.NavigationRecovery.TryLastRescue(Time.unscaledTime);
                return false;
            }
            private static void GiveUpNavigation(VehicleAgent a, Vector3 p, float distance, float elapsed) {
                terminal++;
                SpikeState.PollingActive = false;
                SpikeState.Operation.Active = false;
            }
            public static void InterruptAt(string boundary) {
                if (interruption == boundary + "-stop") {
                    SpikeState.PollingActive = false;
                    SpikeState.Operation.Active = false;
                    SpikeState.NavOrder++;
                }
                if (interruption == boundary + "-reroute") {
                    SpikeState.OperationId++;
                    SpikeState.NavOrder++;
                }
                if (interruption == boundary + "-teardown") Mod.Lifecycle.AcceptingWork = false;
            }
            public static int[] Run(bool success, int ticks, int attempts, string interrupt, bool nativeFailed) {
                dispatched = terminal = rescues = terminalRescues = 0;
                _recurringProbeAt = 0f;
                _recurringProbeUsed = false;
                rescued = success;
                interruption = interrupt;
                _trafficWaiting = _localRecoveryProgress = _recoveryOwnsNavigation = false;
                Mod.Lifecycle.AcceptingWork = true;
                SpikeState.Vehicle = new();
                SpikeState.PollingActive = SpikeState.Operation.Active = true;
                SpikeState.OperationId = 5;
                SpikeState.NavOrder = 26;
                SpikeState.NavCallbackResult = nativeFailed ? "Failed" : null;
                SpikeState.NavigationRecovery.Begin(100f, interrupt == "deadline" ? 2f : 90f);
                for (int i = 0; i < attempts; i++) SpikeState.NavigationRecovery.TrySpend(100f, 4);
                RecoveryRecurrence.Begin(201.6f);
                RecoveryRecurrence.RegisterStall(0f, 0f);
                for (int i = 0; i < ticks && SpikeState.PollingActive; i++) {
                    Time.unscaledTime = 100.5f + i * .5f;
                    if (i == 5) {
                        InterruptAt("wait");
                        if (interrupt.StartsWith("wait-")) Time.unscaledTime = 200f;
                    }
                    TryBeginRecovery(new VehicleAgent(), SpikeState.Vehicle, true);
                }
                return new[] { dispatched, terminal, SpikeState.NavigationRecovery.Attempts, rescues, terminalRescues };
            }
            /* RECOVERY_METHOD */
        }
        }
        """;
}

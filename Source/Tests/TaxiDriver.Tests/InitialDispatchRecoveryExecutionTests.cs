using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes production recovery/evidence gates with native boundaries replaced by counters; not a physics replay.</summary>
public class InitialDispatchRecoveryExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(Compile);

    [Theory]
    [InlineData(true, false, false, false, 0, true)] // owner: successful navigation enabled, no physical start
    [InlineData(false, false, false, false, 0, true)]
    [InlineData(true, true, false, false, 0, false)]
    [InlineData(false, true, false, false, 0, false)] // redispatch reset the native flag, not operation evidence
    [InlineData(true, true, true, false, 0, true)] // previous operation cannot prove this trip drove
    [InlineData(true, true, false, true, 0, true)] // evidence belongs to a different target
    [InlineData(true, false, false, false, 1, false)] // no endless no-drive retry
    public void FirstRecovery_UsesCurrentPhysicalEvidenceNotNativeEnabledFlag(
        bool autoSeen, bool confirmed, bool staleOperation, bool staleTarget, int recoveries, bool redispatch)
    {
        int[] actual = (int[])Replay.Value.Invoke(null,
            new object[] { autoSeen, confirmed, staleOperation, staleTarget, recoveries, "", false })!;
        Assert.Equal(redispatch ? 1 : 0, actual[0]);
        Assert.Equal(redispatch ? 0 : 1, actual[1]);
        Assert.Equal(1, actual[2]); // one recovery grant, not a renewed operation/budget
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SecondLocalRecovery_UsesTheLastOperationGrantForAConfirmedBlocker(bool budgetAlreadySpent)
    {
        // Earlier roadside recovery has spent three episodes when the car reaches the Sedan.
        // This dispatch's second local recovery must not replace Bungalow with the parking lot.
        int[] actual = RunLastEpisode("blocker", budgetAlreadySpent);

        Assert.Equal(new[] { 0, 0, budgetAlreadySpent ? 0 : 1, 1, 0, 4 }, actual);
    }

    [Theory]
    [InlineData("no-blocker", 0)]
    [InlineData("blocked-rear", 1)]
    public void SecondLocalRecovery_PreservesFallbackWhenDetourCannotStart(string scenario, int probes)
    {
        Assert.Equal(new[] { 1, 0, 1, probes, 1, 4 }, RunLastEpisode(scenario, false));
    }

    [Theory]
    [InlineData("observe-reroute", 0)]
    [InlineData("queue-reroute", 1)]
    public void SecondLocalRecovery_AbandonsTheOldFallbackAfterReentrantReroute(string scenario, int probes)
    {
        Assert.Equal(new[] { 0, 0, 1, probes, 0, 4 }, RunLastEpisode(scenario, false));
    }

    private static int[] RunLastEpisode(string scenario, bool budgetAlreadySpent) =>
        (int[])Replay.Value.Invoke(null, new object[] { true, true, false, false, 1, scenario, budgetAlreadySpent })!;

    private static MethodInfo Compile()
    {
        var runner = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs")).GetRoot();
        string[] methods = { "RecoverStuck", "HasCurrentForwardEvidence", "SameAuthorizedTarget" };
        string code = string.Join("\n", runner.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(m => methods.Contains(m.Identifier.Text)).Select(m => m.ToFullString()));
        string policies = string.Join("\n", CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs"))
            .GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Where(c => c.Identifier.Text is "InitialDispatchRecoveryPolicy" or "NavigationRecoveryBudget")
            .Select(c => c.ToFullString()));
        string source = Harness.Replace("/* POLICIES */", policies).Replace("/* METHODS */", code);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("TaxiInitialRecoveryEvidence",
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
        public record struct Vector3(float x, float y, float z) {
            public static float Distance(Vector3 a, Vector3 b) => 329.7f;
        }
        public class Transform { public Vector3 position = new(160.9f, .7f, 22.8f); }
        public class VehicleAgent { public IntPtr Pointer = new(2); }
        public class LandVehicle {
            public IntPtr Pointer = new(1); public VehicleAgent Agent = new();
            public Transform transform = new(); public float Speed_Kmh;
        }
        public class OperationState { public bool Active = true; }
        public static class SpikeState {
            public static bool PollingActive = true, NavEverAutoDriving, RideActive, NavToPlayer;
            public static readonly OperationState Operation = new();
            public static long OperationId = 3; public static int NavOrder = 7, StuckRecoveries;
            public static LandVehicle Vehicle = new();
            public static Vector3 NavTarget = new(-156, -4, 113.7f);
            public static Vector3? RideGoal;
            public static float ProgressWindowStart = 100;
            public static string NavCallbackResult;
            internal static readonly NavigationRecoveryBudget RecoveryBudget = new();
        }
        public static class Time { public static float unscaledTime = 102.5f; }
        public class Life { public bool AcceptingWork = true; }
        public class Logger { public void Warn(string text) {} }
        public static class Mod { public static readonly Life Lifecycle = new(); public static readonly Logger Log = new(); }
        public static class SpikeCommands {
            public static bool HasAuthority() => true;
            public static bool IsOwnedTaxi(LandVehicle v) => ReferenceEquals(v, SpikeState.Vehicle);
            public static bool EnsureDriverSeat(LandVehicle v) => false;
            public static string Fmt(Vector3 v) => v.ToString();
        }
        public static class TaxiDestinations {
            public static int FallbackLookups;
            public static string Num(float v) => v.ToString();
            public static bool TryFallbackDropOff(Vector3 g, Vector3 t, out Vector3 f, out string lot) {
                FallbackLookups++;
                f = new(-156, -4, 82.3f);
                lot = "Parking lot";
                return true;
            }
        }
        public static class SpikeRunner {
            private const float ReverseSeconds = 1.5f;
            private readonly struct StaticBlocker { }
            private static long _forwardEvidenceOperation;
            private static Vector3 _forwardEvidenceTarget;
            private static bool _forwardEvidenceConfirmed;
            private static int reverses, dispatches, grants, detourProbes;
            private static string detourScenario;
            private static bool PlayerDriver(LandVehicle v) => false;
            private static bool TryBeginRecovery(VehicleAgent a, LandVehicle v, bool maneuverPlanned) {
                grants++;
                return SpikeState.RecoveryBudget.TrySpend(Time.unscaledTime, 4);
            }
            private static StaticBlocker? ObserveDetourBlocker(LandVehicle v, VehicleAgent a, Vector3 position) {
                if (detourScenario == "observe-reroute") SpikeState.NavOrder++;
                return detourScenario == "no-blocker" ? null : new StaticBlocker();
            }
            private static bool TryQueueDetourReverse(LandVehicle v, VehicleAgent a, StaticBlocker hit, float now) {
                detourProbes++;
                if (detourScenario == "queue-reroute") SpikeState.NavOrder++;
                return detourScenario == "blocker";
            }
            private static void ArmProgressWindow(LandVehicle v) {}
            private static bool TryStartReverse(VehicleAgent a, LandVehicle v, float now, string reason) { reverses++; return true; }
            private static bool TryFreeSpotRescue(LandVehicle v, VehicleAgent a, string reason) => false;
            private static void ReDispatch(VehicleAgent a, Vector3 t, string why, string fallbackLot = null) { dispatches++; }
            public static int[] Run(bool autoSeen, bool confirmed, bool staleOperation, bool staleTarget, int recoveries,
                string scenario, bool budgetAlreadySpent) {
                dispatches = reverses = grants = detourProbes = TaxiDestinations.FallbackLookups = 0;
                detourScenario = scenario;
                SpikeState.OperationId = 3;
                SpikeState.NavOrder = 7;
                SpikeState.NavEverAutoDriving = autoSeen;
                SpikeState.StuckRecoveries = recoveries;
                SpikeState.Vehicle = new();
                SpikeState.RideActive = scenario.Length > 0;
                SpikeState.RideGoal = SpikeState.RideActive ? SpikeState.NavTarget : null;
                SpikeState.RecoveryBudget.Begin(100f, 90f);
                if (SpikeState.RideActive) {
                    int earlierEpisodes = budgetAlreadySpent ? 4 : 3;
                    for (int i = 0; i < earlierEpisodes; i++) SpikeState.RecoveryBudget.TrySpend(100f, 4);
                }
                _forwardEvidenceConfirmed = confirmed;
                _forwardEvidenceOperation = staleOperation ? 2 : SpikeState.OperationId;
                _forwardEvidenceTarget = staleTarget ? new(0, 0, 0) : SpikeState.NavTarget;
                var v = SpikeState.Vehicle;
                RecoverStuck(v.Agent, v, v.transform.position, 329.7f, 2.5f, autoSeen, budgetAlreadySpent);
                return new[] { dispatches, reverses, grants, detourProbes, TaxiDestinations.FallbackLookups,
                    SpikeState.RecoveryBudget.Attempts };
            }
            /* METHODS */
        }
        }
        """;
}

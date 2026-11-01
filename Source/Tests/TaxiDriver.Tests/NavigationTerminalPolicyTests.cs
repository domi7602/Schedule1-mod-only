using System;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class NavigationTerminalPolicyTests
{
    // Compile the actual polling-terminal block with native reads substituted. This catches
    // the Barn 37.1m/27.2kmh regression without copying the supervisor decision tree.
    [Theory]
    [InlineData(37.1f, true, 27.2f, false, false, false)]
    [InlineData(37.1f, false, 0f, false, false, false)]
    [InlineData(37.1f, true, float.NaN, false, false, false)]
    [InlineData(37.1f, true, 0f, true, false, false)]
    [InlineData(37.1f, true, 0f, false, true, false)]
    [InlineData(5f, true, 0f, false, true, false)]
    [InlineData(5f, true, 27.2f, false, false, false)]
    [InlineData(37.1f, true, 0f, false, false, true)]
    [InlineData(5f, true, 0f, false, false, true)]
    public void PollingTerminal_RequiresHaltAndDefersPendingArrival(float distance, bool readable,
        float speed, bool reversing, bool pending, bool terminal)
    {
        var result = (string)TerminalReplay.Value.Invoke(null, new object[] { distance, readable, speed, reversing, pending })!;
        Assert.Equal(terminal ? (distance <= 10f ? "arrived" : "stopped short") : "active", result);
    }

    [Fact]
    public void NativeRouteDiagnostics_AreOwnedBoundedReadOnlyAndExposeResolvedEvidence()
    {
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string diagnostics = commands.Substring(commands.IndexOf("internal static class SpikeTrace", StringComparison.Ordinal));
        Assert.Contains("RouteLineLimit = 96", diagnostics);
        Assert.Contains("SpikeCommands.IsOwnedTaxi", diagnostics);
        Assert.Contains("__instance.Pointer != road.Pointer", diagnostics);
        Assert.Contains("__instance.Pointer != general.Pointer", diagnostics);
        foreach (string payload in new[] { "currentNavigationSettings", "TargetLocation", "vectorPath", "errorLog", "pathID", "Speed_Kmh", "targetSpeed" })
            Assert.Contains(payload, diagnostics);
        Assert.Contains("nameof(RouteResultPostfix)", diagnostics);
        Assert.Contains("nameof(RouteNavigatePostfix)", diagnostics);
        Assert.Contains("requested={Fmt(requestedTarget)} sampled={sampledTarget}", commands);
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.Contains("SpikeTrace.RouteSnapshot", runner);
        string polling = DriverSafetySourceContractTests.Section(runner, "private static void TickNavigationPolling()", "private static void TickArrivalConfirmation()");
        Assert.True(polling.IndexOf("StopIfNavigationExpired(", StringComparison.Ordinal) < polling.IndexOf("// Terminal:", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void PendingOrUnknownMotion_StillFailsAtProductionDeadline(bool pending)
    {
        var method = TerminalReplay.Value.DeclaringType!.GetMethod("RunDeadline")!;
        Assert.Equal("navigation gave up", method.Invoke(null, new object[] { pending }));
    }

    private static readonly Lazy<System.Reflection.MethodInfo> TerminalReplay = new(() =>
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string block = DriverSafetySourceContractTests.Section(runner,
            "            // Terminal: either", "        catch (Exception ex)\n        {\n            // A single getter error");
        // Section contains the closing brace of the polling try, reused as Run's closing brace.
        string source = """
            using System;
            namespace TaxiDriver {
            public static class Replay {
                public static string Run(float d, bool r, float s, bool rev, bool pending) {
                    SpikeState.Operation.Result = "active";
                    Execute(d,r,s,rev,pending);
                    return SpikeState.Operation.Result;
                }
                public static string RunDeadline(bool pending) {
                    SpikeState.NavigationRecovery.Begin(0,90);
                    if (Run(37.1f,false,0,false,pending)!="active") return "early terminal";
                    if (StopIfNavigationExpired(null,null,89.9f)) return "early deadline";
                    StopIfNavigationExpired(null,null,90f);
                    return SpikeState.Operation.Result;
                }
                const float NavigationTimeoutSeconds=90;
                static bool _trafficWaiting=false;
                static void GiveUpNavigation(VehicleAgent a, Vector3 p,float d,float t) => SpikeState.Operation.Terminate("navigation gave up",true);
                /* DEADLINE */
                static void Execute(float distance, bool readableInput, float speedInput, bool reverseInput, bool pending) {
                    var veh = new Vehicle { Readable=readableInput, Speed=speedInput, Reversing=reverseInput };
                    SpikeState.NavCallbackArrivalPending=pending;
                    SpikeState.NavCallbackArrivalOrder=SpikeState.NavOrder;
                    SpikeState.NavCallbackArrivalOperation=SpikeState.OperationId;
                    float elapsed=1; int position=0;
                    /* BLOCK */
                const float ArrivalThresholdMeters=10f;
                static bool TryReadSignedSpeed(Vehicle v, out float speed) { speed=v.Speed; return v.Readable; }
                static bool TryReadReversing(Vehicle v, out bool rev) { rev=v.Reversing; return true; }
            }
            public class Vehicle { public bool Readable,Reversing; public float Speed; }
            public class VehicleAgent {}
            public class LandVehicle { public Transform transform=new(); }
            public class Transform { public Vector3 position; }
            public struct Vector3 { public static Vector3 zero=>new(); public static float Distance(Vector3 p,int target)=>37.1f; }
            public static class Mod { public static Logger Log=new(); }
            public class Logger { public void Warn(string s) {} }
            public class Operation { public string Result="active"; public bool Active=>Result=="active";
                public void Terminate(string why,bool failed) { Result=why; } }
            public class Reverse { public bool Active; }
            public static class SpikeState {
                public static Operation Operation=new(); public static Reverse Reverse=new();
                internal static NavigationRecoveryBudget NavigationRecovery=new(); public static float NavStartTime;
                public static bool PollingActive=true,NavToPlayer,RideAwaitingBoard,NavEverAutoDriving,NavRetried,NavCallbackArrivalPending;
                public static int NavOrder=1,NavCallbackArrivalOrder,NavTarget,NavStartPosition;
                public static long OperationId=1,NavCallbackArrivalOperation;
                public static float NavStartDistance; public static string NavCallbackResult="Complete";
            }
            public static class SpikeCommands { public static string Fmt(int p)=>"point"; public static void CompletePickup(Vehicle v,string why) {} }
            public static class TaxiDestinations { public static string Num(float v)=>v.ToString(); }
            public static class TaxiLog { public static void Verbose(string s) {} }
            /* POLICY */
            }
            """;
        var policyTree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs"));
        string policy = string.Join("\n", System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(System.Linq.Enumerable.OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>(
            policyTree.GetRoot().DescendantNodes()), c => c.Identifier.Text is "NavigationArrivalPolicy" or "NavigationRecoveryBudget"), c => c.ToFullString()));
        var runnerTree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(runner);
        string deadline = System.Linq.Enumerable.Single(System.Linq.Enumerable.OfType<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>(
            runnerTree.GetRoot().DescendantNodes()), m => m.Identifier.Text == "StopIfNavigationExpired").ToFullString();
        var refs = System.Linq.Enumerable.Select(((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(System.IO.Path.PathSeparator),
            p => Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(p));
        var compile = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create("PollingTerminalReplay",
            new[] { Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(source.Replace("/* BLOCK */", block).Replace("/* POLICY */", policy).Replace("/* DEADLINE */", deadline)) }, refs,
            new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary));
        using var stream = new System.IO.MemoryStream();
        var emit = compile.Emit(stream);
        Assert.True(emit.Success, string.Join("\n", emit.Diagnostics));
        return System.Reflection.Assembly.Load(stream.ToArray()).GetType("TaxiDriver.Replay")!.GetMethod("Run")!;
    });

    [Theory]
    [InlineData(9f, "Failed")]
    [InlineData(10f, "Stopped")]
    [InlineData(9f, "Complete")]
    public void GiveUpThenNewDirectOperation_CompleteWithinTenMetresArrives(float distance, string oldCallback)
    {
        bool gaveUp = true, arrived = true;
        string? callback = oldCallback;
        Assert.Equal("navigation gave up", NavigationEndReason.ForRide(gaveUp, distance, 10f, callback));
        NavigationTerminalPolicy.Begin(ref gaveUp, ref arrived, ref callback);
        Assert.False(gaveUp);
        Assert.False(arrived);
        Assert.Null(callback);
        callback = "Complete";
        Assert.Equal("arrived", NavigationEndReason.ForRide(gaveUp, distance,
            NavigationArrivalPolicy.ArrivalMeters, callback));
    }

    [Fact]
    public void Runtime_NewDirectAndPatrolOperationsResetBeforeCompletionConsumers()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string begin = DriverSafetySourceContractTests.Section(runner,
            "internal static void BeginNavigationOperation(", "private static bool StopIfNavigationExpired");
        Assert.Contains("NavigationTerminalPolicy.Begin(ref SpikeState.NavGaveUp, ref SpikeState.RideArrived,", begin);
        Assert.Contains("ref SpikeState.NavCallbackResult)", begin);
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string go = DriverSafetySourceContractTests.Section(commands,
            "internal static bool Go(", "internal static void Pois()");
        int beginAt = go.IndexOf("SpikeRunner.BeginNavigationOperation(startPosition, dispatchTime);", StringComparison.Ordinal);
        Assert.True(beginAt >= 0 && beginAt < go.IndexOf("agent.Navigate(target, settings,", StringComparison.Ordinal));
        string start = DriverSafetySourceContractTests.Section(commands,
            "private static bool StartRideDrive(", "internal static bool Stop()");
        Assert.Contains("SpikeRunner.BeginNavigationOperation(startPosition, dispatchTime);", start);
        Assert.Contains("return Go(0f, absoluteTarget: destination, toPlayer: false, preserveStreetTarget: streetCandidate, preparedRequest: request);", start);
        Assert.Equal(2, commands.Split("SpikeRunner.BeginNavigationOperation(").Length - 1);
        // Order invalidation is also used by recovery, terminal stop and teardown.
        string state = DriverSafetySourceContractTests.ReadSource("SpikeState.cs");
        Assert.Contains("internal static int NextNavOrder() => ++NavOrder;", state);
    }

    [Fact]
    public void RecoveryRetainsRecurrenceGlobalFourCapAndNinetySecondDeadline()
    {
        var budget = new NavigationRecoveryBudget();
        var recurrence = new RecoveryRecurrencePolicy();
        budget.Begin(0, 90);
        budget.ObserveProgress(0, 100, false);
        recurrence.Begin(100);
        Assert.False(recurrence.RegisterStall(0, 0));
        for (int dispatch = 1; dispatch <= 4; dispatch++)
        {
            Assert.True(budget.TrySpend(dispatch * 8, 4));
            Assert.True(recurrence.RegisterStall(1, 1));
            budget.ObserveProgress(dispatch * 8, 100, false);
        }
        Assert.Equal(4, budget.Attempts);
        Assert.False(budget.TrySpend(40, 4));
        Assert.False(budget.HasExpired(89.9f));
        Assert.True(budget.HasExpired(90));
        Assert.False(budget.TryLastRescue(90));

        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string recovery = DriverSafetySourceContractTests.Section(runner,
            "internal static void ReDispatch", "private static bool TryTeleportRecovery");
        Assert.DoesNotContain("BeginNavigationOperation(", recovery);
        Assert.DoesNotContain("NavigationTerminalPolicy.Begin(", recovery);
        Assert.DoesNotContain("NavigationRecovery.Begin(", recovery);
        Assert.DoesNotContain("RecoveryRecurrence.Begin(", recovery);
        Assert.Contains("StopIfNavigationExpired(", recovery);
        Assert.Contains("NavigationTimeoutSeconds = 90f", runner);
        Assert.Contains("NavigationRecovery.TrySpend(now, TaxiAI.PathingFailureCap)", runner);
        string ai = DriverSafetySourceContractTests.ReadSource("TaxiAI.cs");
        Assert.Contains("Mathf.Clamp((int)Math.Round(cap), 1, 4)", ai);
        Assert.Contains("PathingFailureCap = clamped;", ai);
    }
}

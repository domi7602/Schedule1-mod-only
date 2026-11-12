using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes the production speed hook with substituted native state; no Unity braking claim.</summary>
public class CruiseApproachDistanceExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(Compile);

    [Theory]
    [InlineData("pickup", 2.8f, 6f)] // Recorded Complete at 18.08 km/h, still requesting 50.
    [InlineData("pickup", 10f, 6f)]
    [InlineData("pickup", 200f, 50f)]
    [InlineData("passenger", 2.8f, 6f)]
    [InlineData("passenger", 200f, 50f)]
    public void CurrentStop_AppliesTheSameApproachLimitForPickupAndPassenger(
        string scenario, float distance, float expected)
    {
        var result = Run(scenario, distance);
        Assert.Equal(expected, result.Target);
        Assert.Equal(1, result.Writes);
    }

    [Theory]
    [InlineData("pickup-calculating")]
    [InlineData("pickup-wrong-native-target")]
    [InlineData("passenger-calculating")]
    [InlineData("passenger-wrong-native-target")]
    [InlineData("passenger-no-trip-point")]
    [InlineData("passenger-wrong-trip-point")]
    [InlineData("passenger-awaiting-destination")]
    [InlineData("passenger-no-ride")]
    [InlineData("passenger-via")]
    public void UnconfirmedOrIntermediateStop_DoesNotBorrowTheNearbyFinalStop(string scenario)
    {
        // The ordinary cruise target remains; native speed reductions are a later native step.
        Assert.Equal(50f, Run(scenario).Target);
    }

    [Theory]
    [InlineData("pickup-inactive")]
    [InlineData("pickup-not-polling")]
    [InlineData("pickup-no-authority")]
    [InlineData("pickup-foreign")]
    [InlineData("pickup-player-driving")]
    [InlineData("pickup-reversing")]
    [InlineData("pickup-managed-reverse")]
    [InlineData("pickup-native-override")]
    [InlineData("pickup-not-driving")]
    public void IneligiblePickup_PreservesNativeTargetWithoutWriting(string scenario)
    {
        var result = Run(scenario);
        Assert.Equal(25f, result.Target);
        Assert.Equal(0, result.Writes);
    }

    [Theory]
    [InlineData("pickup-new-order")]
    [InlineData("pickup-new-operation")]
    [InlineData("pickup-new-vehicle")]
    public void ReplacementDuringTargetRead_DoesNotReceiveTheOldApproachLimit(string scenario)
    {
        var result = Run(scenario);
        Assert.Equal(25f, result.Target);
        Assert.Equal(0, result.Writes);
    }

    private static (float Target, int Writes) Run(string scenario, float distance = 2.8f) =>
        ((float, int))Replay.Value.Invoke(null, new object[] { scenario, distance })!;

    private static MethodInfo Compile()
    {
        var patch = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("CruiseSpeedPatch.cs"))
            .GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Single(type => type.Identifier.Text == "CruiseSpeedPatch");
        string[] methodNames = { "UpdateSpeedPostfix", "GetApproachDistance" };
        string methods = string.Join("\n", patch.Members.OfType<MethodDeclarationSyntax>()
            .Where(method => methodNames.Contains(method.Identifier.Text)).Select(method => method.ToFullString()));
        var policySource = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs"))
            .GetRoot();
        string[] policyNames = { "CruiseSpeedPolicy", "NavigationArrivalPolicy" };
        string policies = string.Join("\n", policySource.DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Where(type => policyNames.Contains(type.Identifier.Text)).Select(type => type.ToFullString()));
        string source = Harness.Replace("/* METHODS */", methods).Replace("/* POLICIES */", policies);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("TaxiCruiseApproachReplay",
            new[] { CSharpSyntaxTree.ParseText(source) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.CruiseSpeedPatch")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        using System.Numerics;
        namespace Replay {
            /* POLICIES */
            sealed class Transform { public Vector3 position; }
            sealed class LandVehicle {
                public IntPtr Pointer = new(1);
                public object DriverPlayer;
                public float Speed_Kmh = 18.08f;
                public Transform transform = new();
            }
            sealed class DriveFlags { public bool OverrideSpeed; }
            sealed class VehicleAgent {
                public LandVehicle vehicle = new();
                public DriveFlags Flags = new();
                public bool AutoDriving = true, IsReversing, NavigationCalculationInProgress;
                public int Writes;
                private float speed = 25f;
                public float targetSpeed { get => speed; set { speed = value; Writes++; } }
                public Vector3 NativeTarget;
                public Action ReadTarget;
                public Vector3 TargetLocation {
                    get { var action = ReadTarget; ReadTarget = null; action?.Invoke(); return NativeTarget; }
                }
            }
            sealed class ActiveState { public bool Active = true; }
            static class SpikeState {
                public static ActiveState Operation = new(), Reverse = new();
                public static int NavOrder;
                public static long OperationId;
                public static bool PollingActive, RideActive, NavToPlayer, RideAwaitingDestination;
                public static Vector3? ActiveTripPoint;
                public static Vector3 NavTarget;
                public static LandVehicle Vehicle;
            }
            static class TaxiAI { public static bool Active; }
            static class SpikeCommands {
                public static bool Authority, Owned;
                public static bool HasAuthority() => Authority;
                public static bool IsOwnedTaxi(LandVehicle vehicle) => Owned;
            }
            sealed class Logger { public void Info(string text) { } public void Warn(string text) { } }
            static class Mod { public static Logger Log = new(); }
            public static class CruiseSpeedPatch {
                private static bool _enabled = true, _errorLogged;
                private static int _loggedOrder = -1, _loggedApproachOrder = -1;
                /* METHODS */
                public static (float Target, int Writes) Run(string scenario, float distance) {
                    _enabled = true; _errorLogged = false;
                    _loggedOrder = _loggedApproachOrder = -1;
                    var agent = new VehicleAgent { NativeTarget = new Vector3(distance, 0, 0) };
                    SpikeCommands.Authority = SpikeCommands.Owned = true;
                    TaxiAI.Active = false;
                    SpikeState.Operation = new ActiveState();
                    SpikeState.Reverse = new ActiveState { Active = false };
                    SpikeState.NavOrder = 4; SpikeState.OperationId = 2;
                    SpikeState.PollingActive = true;
                    SpikeState.NavToPlayer = scenario.StartsWith("pickup");
                    SpikeState.RideActive = !SpikeState.NavToPlayer;
                    SpikeState.RideAwaitingDestination = false;
                    SpikeState.NavTarget = agent.NativeTarget;
                    SpikeState.ActiveTripPoint = SpikeState.NavToPlayer ? null : agent.NativeTarget;
                    SpikeState.Vehicle = agent.vehicle;
                    if (scenario.EndsWith("-calculating")) agent.NavigationCalculationInProgress = true;
                    if (scenario.EndsWith("-wrong-native-target")) agent.NativeTarget += Vector3.UnitX;
                    switch (scenario) {
                        case "pickup-inactive":
                            SpikeState.Operation.Active = false; SpikeState.PollingActive = false; break;
                        case "pickup-not-polling": SpikeState.PollingActive = false; break;
                        case "pickup-no-authority": SpikeCommands.Authority = false; break;
                        case "pickup-foreign": SpikeCommands.Owned = false; break;
                        case "pickup-player-driving": agent.vehicle.DriverPlayer = new object(); break;
                        case "pickup-reversing": agent.IsReversing = true; break;
                        case "pickup-managed-reverse": SpikeState.Reverse.Active = true; break;
                        case "pickup-native-override": agent.Flags.OverrideSpeed = true; break;
                        case "pickup-not-driving": agent.AutoDriving = false; break;
                        case "passenger-no-trip-point": SpikeState.ActiveTripPoint = null; break;
                        case "passenger-wrong-trip-point": SpikeState.ActiveTripPoint += Vector3.UnitX; break;
                        case "passenger-awaiting-destination": SpikeState.RideAwaitingDestination = true; break;
                        case "passenger-no-ride": SpikeState.RideActive = false; break;
                        case "passenger-via": agent.NativeTarget = new Vector3(100, 0, 0); break;
                        case "pickup-new-order": agent.ReadTarget = () => SpikeState.NavOrder++; break;
                        case "pickup-new-operation": agent.ReadTarget = () => SpikeState.OperationId++; break;
                        case "pickup-new-vehicle": agent.ReadTarget = () =>
                            SpikeState.Vehicle = new LandVehicle { Pointer = new IntPtr(2) }; break;
                    }
                    UpdateSpeedPostfix(agent);
                    return (agent.targetSpeed, agent.Writes);
                }
            }
        }
        """;
}

using System.Reflection;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Runs actual Harmony hook methods around a substituted consumer; never a Unity brake/physics proof.</summary>
public class NativeActuatorReadbackExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(Compile);

    [Fact]
    public void ParkToDrive_ObservesConsumedOutputWithoutWritingNoneInputs()
    {
        Outcome result = Run("startup");
        Assert.Equal(0, result.Writes);
        Assert.True(result.Healthy);
        Assert.Contains(result.Reads, r => r.Mode == "None" && r.Hand);
        Assert.Contains(result.Reads, r => r.Mode == "None" && r.Phase == "postfix-consumed" && !r.Hand);
        Assert.Single(result.Reads, r => r.Mode == "None" && r.Phase == "postfix-consumed");
        Assert.False(result.Hand);
    }

    [Fact]
    public void StartupSampling_IsFrequentButBoundedAndRearmsForANewOrder()
    {
        Outcome result = Run("sampling");
        Assert.Equal(0, result.Writes);
        Assert.InRange(result.BeforeExpiry, 4, 8);
        Assert.Equal(result.BeforeExpiry, result.AfterExpiry);
        Assert.Equal(result.AfterExpiry + 1, result.AfterNewOrder);
    }

    [Theory]
    [InlineData("stop")]
    [InlineData("reroute")]
    [InlineData("order")]
    [InlineData("teardown")]
    [InlineData("vehicle")]
    [InlineData("authority")]
    [InlineData("player")]
    public void SupersededNoneConsumption_PublishesNoPostfixFromTheOldCall(string interruption)
    {
        Outcome result = Run(interruption);
        Assert.Equal(0, result.Writes);
        Assert.DoesNotContain(result.Reads, r => r.Mode == "None" && r.Phase == "postfix-consumed");
    }

    [Theory]
    [InlineData("idle")]
    [InlineData("paused")]
    [InlineData("foreign")]
    public void InactiveOrUnauthorizedWindow_DoesNotAddPostfixReads(string scenario)
    {
        Outcome result = Run(scenario);
        Assert.Equal(0, result.Writes);
        Assert.DoesNotContain(result.Reads, r => r.Mode == "None" && r.Phase == "postfix-consumed");
    }

    [Fact]
    public void NativeNoneException_RemainsUnsuppressedAndCannotDisableTheControlHook()
    {
        Outcome result = Run("native-exception");
        Assert.True(result.SameException);
        Assert.True(result.Healthy);
        Assert.Equal(0, result.Writes);
        Assert.DoesNotContain(result.Reads, r => r.Phase == "postfix-consumed");
    }

    [Theory]
    [InlineData("held-reverse-expired")]
    [InlineData("held-reverse-revoked")]
    public void HeldDetour_AllowsOnlyTheCurrentBoundedReverseAtTheActualConsumer(string scenario)
    {
        var result = Run(scenario);
        Assert.True(result.Healthy);
        Assert.Contains(result.Reads, r => r.Mode == "Reverse" && r.Phase == "prefix-consume" && r.Throttle < 0f);
        Assert.Equal(1, result.BeforeExpiry);
        Assert.Equal(result.BeforeExpiry, result.AfterExpiry);
        Assert.NotEqual("Reverse", result.Reads[^1].Mode);
    }

    private static Outcome Run(string scenario) => JsonSerializer.Deserialize<Outcome>(
        (string)Replay.Value.Invoke(null, new object[] { scenario })!)!;

    private sealed record Read(string Mode, string Phase, bool Hand, float Throttle);
    private sealed record Outcome(int Writes, bool Healthy, bool Hand, bool SameException,
        int BeforeExpiry, int AfterExpiry, int AfterNewOrder, Read[] Reads);

    private static MethodInfo Compile()
    {
        var patch = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("NativeActuatorPatch.cs"))
            .GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Single(c => c.Identifier.Text == "NativeActuatorPatch");
        string[] names = { "ApplyThrottlePrefix", "ApplyThrottlePostfix", "ApplyThrottleFinalizer", "Complete",
            "ReleasePark", "ResetIntent", "IsCurrentReadback" };
        string methods = string.Join("\n", patch.Members.OfType<MethodDeclarationSyntax>()
            .Where(m => names.Contains(m.Identifier.Text)).Select(m => m.ToFullString()));
        string fields = string.Join("\n", patch.Members.OfType<FieldDeclarationSyntax>()
            .Where(f => !f.Declaration.Variables.Any(v => v.Identifier.Text == "_harmony"))
            .Select(f => f.ToFullString()));
        string state = patch.Members.OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.Text == "CallState").ToFullString();
        var policyTree = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs")).GetRoot();
        string[] types = { "NativeActuatorMode", "NativeActuatorPolicy", "NativeActuatorScope", "BrakeHoldPolicy", "ReversePedalPolicy",
            "ReverseManoeuvrePolicy", "ReversePhase" };
        string policies = string.Join("\n", policyTree.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
            .Where(t => types.Contains(t.Identifier.Text)).Select(t => t.ToFullString()));
        string source = Harness.Replace("/* POLICIES */", policies).Replace("/* FIELDS */", fields)
            .Replace("/* STATE */", state).Replace("/* METHODS */", methods);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("TaxiReadOnlyConsumerReplay",
            new[] { CSharpSyntaxTree.ParseText(source) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.NativeActuatorPatch")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Numerics;
        using System.Text.Json;
        namespace Replay {
        /* POLICIES */
        public static class Time { public static float unscaledTime, timeScale = 1; public static int frameCount; }
        public sealed class Life { public bool AcceptingWork = true; }
        public static class Mod { public static readonly Life Lifecycle = new(); }
        public class LandVehicle {
            public IntPtr Pointer = new(1); public object DriverPlayer;
            public bool HandbrakeApplied = true;
            private float throttle;
            private bool hand, controls;
            public int Writes;
            public float currentThrottle { get => throttle; set { throttle = value; Writes++; } }
            public bool handbrakeOverride { get => hand; set { hand = value; Writes++; } }
            public bool overrideControls { get => controls; set { controls = value; Writes++; } }
        }
        public sealed class OperationState { public bool Active = true; }
        public static class SpikeState {
            public static LandVehicle Vehicle;
            public static long OperationId;
            public static int NavOrder;
            public static bool PollingActive;
            public static float NavStartTime;
            internal static readonly ReverseManoeuvrePolicy Reverse = new();
            public static readonly OperationState Operation = new();
        }
        public static class SpikeRunner { public static bool HasActuatorOrder; }
        public static class SpikeCommands {
            public static bool Authority = true;
            public static bool HasAuthority() => Authority;
            public static bool IsOwnedTaxi(LandVehicle v) => ReferenceEquals(v, SpikeState.Vehicle);
        }
        public static class NativeActuatorPatch {
            /* FIELDS */
            internal static bool Healthy { get; private set; }
            /* STATE */
            /* METHODS */
            public sealed record Read(string Mode, string Phase, bool Hand, float Throttle);
            private static readonly List<Read> Reads = new();
            private static void Readback(LandVehicle vehicle, NativeActuatorMode mode, string phase) {
                Reads.Add(new(mode.ToString(), phase, vehicle.HandbrakeApplied, vehicle.currentThrottle));
            }
            private static void Fail(Exception error) { Healthy = false; }
            private static void Consume(LandVehicle vehicle) {
                ApplyThrottlePrefix(vehicle, out var state);
                // A substitute native consumer changes its OUTPUT, not the mod's inputs.
                vehicle.HandbrakeApplied = vehicle.overrideControls && vehicle.handbrakeOverride;
                ApplyThrottlePostfix(state);
                ApplyThrottleFinalizer(null, state);
                Time.frameCount++;
            }
            private static int Count() => Reads.Count(r => r.Mode == "None" && r.Phase == "postfix-consumed");
            public static string Run(string scenario) {
                Healthy = true; ResetIntent(); Reads.Clear();
                Mod.Lifecycle.AcceptingWork = SpikeCommands.Authority = true;
                Time.timeScale = 1; Time.unscaledTime = 99; Time.frameCount = 1;
                SpikeState.OperationId = 3; SpikeState.NavOrder = 7; SpikeState.NavStartTime = 100;
                SpikeState.PollingActive = SpikeState.Operation.Active = true;
                SpikeState.Reverse.End(); SpikeRunner.HasActuatorOrder = false;
                var vehicle = new LandVehicle(); SpikeState.Vehicle = vehicle;
                // Real park scope, followed by its input restoration and intent release.
                _parkPointer = vehicle.Pointer;
                Consume(vehicle);
                if (!vehicle.HandbrakeApplied) throw new Exception("park setup did not reach the substitute consumer");
                ReleasePark(vehicle); vehicle.Writes = 0; Reads.Clear();
                SpikeRunner.HasActuatorOrder = true; Time.unscaledTime = 100;
                vehicle.overrideControls = true; vehicle.handbrakeOverride = false; vehicle.currentThrottle = 1;
                vehicle.Writes = 0;
                int beforeExpiry = 0, afterExpiry = 0, afterNewOrder = 0; bool sameException = true;
                if (scenario.StartsWith("held-reverse-", StringComparison.Ordinal)) {
                    SpikeRunner.HasActuatorOrder = false;
                    SpikeState.Reverse.Begin(SpikeState.OperationId, SpikeState.NavOrder, Vector3.Zero, -Vector3.UnitX, 100f, 3f);
                    Consume(vehicle);
                    beforeExpiry = Reads.Count(r => r.Mode == "Reverse" && r.Phase == "postfix-consumed");
                    if (scenario == "held-reverse-revoked") SpikeState.Reverse.End();
                    else Time.unscaledTime = 103f;
                    Consume(vehicle);
                    afterExpiry = Reads.Count(r => r.Mode == "Reverse" && r.Phase == "postfix-consumed");
                } else if (scenario == "sampling") {
                    for (int i = 0; i <= 30; i++) { Time.unscaledTime = 100 + i * .1f; Consume(vehicle); }
                    beforeExpiry = Count();
                    Time.unscaledTime = 104; Consume(vehicle); afterExpiry = Count();
                    SpikeState.NavOrder++; SpikeState.NavStartTime = 110; Time.unscaledTime = 110;
                    Consume(vehicle); afterNewOrder = Count();
                } else {
                    if (scenario == "idle") SpikeRunner.HasActuatorOrder = SpikeState.PollingActive = false;
                    if (scenario == "paused") Time.timeScale = 0;
                    if (scenario == "foreign") SpikeState.Vehicle = new LandVehicle();
                    ApplyThrottlePrefix(vehicle, out var state);
                    switch (scenario) {
                        case "stop": SpikeRunner.HasActuatorOrder = SpikeState.PollingActive = SpikeState.Operation.Active = false; break;
                        case "reroute": SpikeState.OperationId++; break;
                        case "order": SpikeState.NavOrder++; break;
                        case "teardown": Mod.Lifecycle.AcceptingWork = false; break;
                        case "vehicle": SpikeState.Vehicle = new LandVehicle(); break;
                        case "authority": SpikeCommands.Authority = false; break;
                        case "player": vehicle.DriverPlayer = new object(); break;
                    }
                    if (scenario == "native-exception") {
                        var error = new Exception("native consumer failed");
                        sameException = ReferenceEquals(error, ApplyThrottleFinalizer(error, state));
                    } else {
                        vehicle.HandbrakeApplied = vehicle.overrideControls && vehicle.handbrakeOverride;
                        ApplyThrottlePostfix(state); ApplyThrottleFinalizer(null, state);
                    }
                }
                return JsonSerializer.Serialize(new {
                    vehicle.Writes, Healthy, Hand = vehicle.HandbrakeApplied, SameException = sameException,
                    BeforeExpiry = beforeExpiry, AfterExpiry = afterExpiry, AfterNewOrder = afterNewOrder, Reads
                });
            }
        }
        }
        """;
}

using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes the real corridor tick and candidate search with controlled native boundaries.</summary>
public class RoadReplanCorridorExecutionTests
{
    private static readonly Lazy<Type> Replay = new(Compile);

    [Theory]
    [InlineData("same-order", 1, 3f, 0.9f)]
    [InlineData("second-supported-point", 2, 6.9801f, 1.298f)]
    public void NativeReplan_CanUseACheckedPointOnThePreviousThreeDimensionalCorridor(
        string scenario, int probes, float targetZ, float targetY)
    {
        var result = Run(scenario);

        Assert.Equal(1, result.Moves);
        Assert.Equal(1, result.Snaps);
        Assert.Equal(probes, result.Probes);
        Assert.Equal(targetZ, result.TargetZ, precision: 3);
        Assert.Equal(targetY, result.TargetY, precision: 3);
        Assert.True(result.LoggedCachedPlacement);
    }

    [Fact]
    public void NativeReplan_WalksTheRoadAroundACornerAfterAnOccupiedStart()
    {
        var result = Run("corner-after-rejections");

        Assert.Equal(1, result.Moves);
        Assert.Equal(3, result.Probes); // Origin, +4m, +8m. There is no route behind the start.
        Assert.Equal(5.9603f, result.TargetX, precision: 3);
        Assert.Equal(3f, result.TargetZ, precision: 3);
        Assert.Equal(1.496f, result.TargetY, precision: 3);
        Assert.Equal(1f, result.HeadingX, precision: 3);
        Assert.Equal(0f, result.HeadingZ, precision: 3);
    }

    [Theory]
    [InlineData("corner-intermediate", 4, 0f, 2.9901f)]
    [InlineData("corner-later-intermediate", 5, 3.9702f, 3f)]
    public void NativeReplan_ProbesIntermediateRoadPointsAfterTheOriginalOffsetsAreBlocked(
        string scenario, int probes, float targetX, float targetZ)
    {
        var result = Run(scenario);

        Assert.Equal(1, result.Moves);
        Assert.Equal(probes, result.Probes);
        Assert.Equal(targetX, result.TargetX, precision: 3);
        Assert.Equal(targetZ, result.TargetZ, precision: 3);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RoadPlacement_QueriesTheOffsetMeasuredBoxAtTheProposedYaw(bool foreignCollider)
    {
        var result = Replay.Value.GetMethod("MeasureFootprint")!.Invoke(null, new object[] { foreignCollider });

        Assert.True(Assert.IsType<bool>(result));
    }

    [Fact]
    public void RescueWithoutARoute_ReportsTheOriginalFourLocalProbes()
    {
        var result = Replay.Value.GetMethod("RescueWithoutRoute")!.Invoke(null, null);

        Assert.True(Assert.IsType<bool>(result));
    }

    [Theory]
    [InlineData("no-history")]
    [InlineData("no-calculation")]
    [InlineData("too-old")]
    [InlineData("too-far")]
    [InlineData("different-vehicle")]
    [InlineData("different-agent")]
    [InlineData("different-operation")]
    [InlineData("different-generation")]
    [InlineData("different-order")]
    [InlineData("different-target")]
    [InlineData("different-native-target")]
    [InlineData("ready-path")]
    [InlineData("unowned")]
    [InlineData("player-driving")]
    [InlineData("reverse")]
    [InlineData("native-reverse")]
    [InlineData("stopped")]
    [InlineData("reset")]
    [InlineData("soft-margin")]
    public void NativeReplan_RefusesStaleOrInapplicableGeometry(string scenario)
    {
        var result = Run(scenario);

        Assert.Equal(0, result.Moves);
        Assert.Equal(0, result.Probes);
        Assert.Equal(0, result.Snaps);
        Assert.Equal(0, result.Nudges);
    }

    [Theory]
    [InlineData("blocked", 9, true)]
    [InlineData("vertical-break", 2, true)]
    [InlineData("nonfinite-break", 2, true)]
    [InlineData("reroute-during-probe", 1, false)]
    [InlineData("new-path-during-probe", 1, false)]
    public void NativeReplan_RequiresClearPlacementAndRechecksTheOrderAfterTheProbe(string scenario, int probes, bool refused)
    {
        var result = Run(scenario);

        Assert.Equal(probes, result.Probes);
        Assert.Equal(0, result.Moves);
        Assert.Equal(0, result.Snaps);
        Assert.False(result.LoggedCachedPlacement);
        Assert.Equal(refused, result.LoggedCachedRefusal);
    }

    [Theory]
    [InlineData("rescue-excludes-supported-root", 2, 1, 1)]
    [InlineData("rescue-order-changed", 1, 0, 1)]
    [InlineData("rescue-root-recheck-refused", 1, 0, 0)]
    public void Rescue_ContinuesPastAnExcludedCandidateAndRechecksBeforePlacement(
        string scenario, int probes, int moves, int orderChecks)
    {
        var result = Run(scenario);

        Assert.Equal(probes, result.Probes);
        Assert.Equal(moves, result.Moves);
        Assert.Equal(orderChecks, result.OrderChecks);
        if (moves > 0)
            Assert.Equal(3f + 4f / MathF.Sqrt(1.01f), result.TargetZ, precision: 3);
    }

    private static (int Moves, int Snaps, int Probes, int Nudges, float TargetZ, float TargetY,
        bool LoggedCachedPlacement, bool LoggedCachedRefusal, float TargetX, float HeadingX, float HeadingZ, int OrderChecks)
        Run(string scenario) =>
        ((int, int, int, int, float, float, bool, bool, float, float, float, int))
            Replay.Value.GetMethod("Run")!.Invoke(null, new object[] { scenario })!;

    private static Type Compile()
    {
        var tree = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("RoadKeeper.cs")).GetRoot();
        var keeper = tree.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(t => t.Identifier.Text == "RoadKeeper");
        string[] methods = {
            "Reset", "Tick", "CurrentCorridorOrder", "StillReplanning", "TryCorrectDuringReplan",
            "HorizontalDistanceSquared", "TryGetCorridor", "TryGetCachedCorridor", "UpdateClosest",
            "TryStepAlongCorridor", "HardSnap", "TryFindFreeSpot", "AppendRejectedCandidate", "Finite",
            "TryRescueStartup", "IsPlacementFree"
        };
        string members = string.Join("\n", keeper.Members
            .Where(member => member is not MethodDeclarationSyntax method || methods.Contains(method.Identifier.Text))
            .Select(member => member.ToFullString()));
        string policies = string.Join("\n", new[] { "RoadProjectionPolicy.cs", "RoadSnapSafetyPolicy.cs" }
            .Select(name => CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource(name)).GetRoot()
                .DescendantNodes().OfType<ClassDeclarationSyntax>().Single().ToFullString()));
        string source = Harness.Replace("/* MEMBERS */", members).Replace("/* POLICIES */", policies);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("TaxiRoadReplanReplay",
            new[] { CSharpSyntaxTree.ParseText(source) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay.RoadKeeper")!;
    }

    private const string Harness = """
        #nullable enable
        using System;
        using System.Collections.Generic;
        using System.Text;
        using Replay.Policies;

        namespace Replay.Policies {
            using System.Numerics;
            /* POLICIES */
        }

        namespace Replay {
            internal record struct Vector3(float x, float y, float z) {
                internal static Vector3 forward => new(0f, 0f, 1f);
                internal float sqrMagnitude => x*x + y*y + z*z;
                internal void Normalize() { float length = MathF.Sqrt(sqrMagnitude); this = this * (1f / length); }
                internal static float Distance(Vector3 a, Vector3 b) => MathF.Sqrt((a - b).sqrMagnitude);
                public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
                public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
                public static Vector3 operator *(Vector3 a, float b) => new(a.x * b, a.y * b, a.z * b);
                internal static Vector3 up => new(0f, 1f, 0f);
            }
            internal record struct Quaternion(Vector3 Heading) {
                internal static Quaternion identity => default;
                internal static Quaternion LookRotation(Vector3 direction, Vector3 up) => new(direction);
                internal static Quaternion Euler(float x, float y, float z) => default;
                public static Quaternion operator *(Quaternion a, Quaternion b) => a;
                public static Vector3 operator *(Quaternion rotation, Vector3 point) {
                    var heading = rotation.Heading;
                    return heading.sqrMagnitude == 0f ? point : new(
                        heading.z * point.x + heading.x * point.z,
                        point.y,
                        heading.z * point.z - heading.x * point.x);
                }
            }
            internal static class Mathf { internal static float Sqrt(float value) => MathF.Sqrt(value); }
            internal static class Time { internal static float timeScale = 1f, unscaledTime; }
            internal sealed class Transform {
                internal Vector3 position;
                internal Transform? parent;
                internal Vector3 forward => Vector3.forward;
                internal Quaternion rotation => default;
                internal bool IsChildOf(Transform root) => this == root || parent == root;
            }
            internal sealed class Collider {
                internal IntPtr Pointer = new(8);
                internal bool WasCollected;
                internal readonly Transform transform = new();
            }
            internal enum QueryTriggerInteraction { Ignore }
            internal static class Physics {
                internal static Collider[] Hits = Array.Empty<Collider>();
                internal static Vector3 Center, Half;
                internal static Quaternion Rotation;
                internal static void SyncTransforms() { }
                internal static Collider[] OverlapBox(Vector3 center, Vector3 half, Quaternion rotation,
                    int mask, QueryTriggerInteraction query) {
                    if (mask != ~0 || query != QueryTriggerInteraction.Ignore) throw new InvalidOperationException();
                    Center = center;
                    Half = half;
                    Rotation = rotation;
                    return Hits;
                }
            }
            internal sealed class Path { internal readonly List<Vector3> vectorPath = new(); }
            internal sealed class VehicleAgent {
                internal IntPtr Pointer = new(2);
                internal Vector3 TargetLocation = new(50f, 0f, 100f);
                internal Path? path;
                internal bool AutoDriving = true, IsReversing, NavigationCalculationInProgress;
            }
            internal sealed class LandVehicle {
                internal IntPtr Pointer = new(1);
                internal VehicleAgent? Agent = new();
                internal object? DriverPlayer;
                internal readonly Transform transform = new();
            }
            internal static class SpikeState {
                internal static LandVehicle? Vehicle;
                internal static Vector3 NavTarget = new(50f, 0f, 100f);
                internal static bool PollingActive = true;
                internal static long OperationId = 3;
                internal static int NavOrder = 4;
                internal static readonly OperationState Operation = new();
                internal static readonly ReverseState Reverse = new();
            }
            internal sealed class OperationState { internal bool Active = true; internal long Generation = 5; }
            internal sealed class ReverseState { internal bool Active; }
            internal static class TaxiAI { internal static bool Active, NavigationBlocked; }
            internal static class SpikeCommands {
                internal static bool Owned = true;
                internal static bool HasAuthority() => true;
                internal static bool IsOwnedTaxi(LandVehicle vehicle) => Owned;
                internal static string Fmt(Vector3 value) => value.ToString();
            }
            internal static class TaxiDestinations { internal static string Num(float value) => value.ToString(); }
            internal sealed class Logger {
                internal bool LoggedCachedPlacement, LoggedCachedRefusal, LoggedNoRouteRefusal;
                internal void Info(string message) { }
                internal void Warn(string message) {
                    LoggedNoRouteRefusal |= message.Contains("local fallback without a path") &&
                        message.Contains("FootprintOverlap (road/curb)");
                    if (!message.Contains("using the previous corridor during native replan")) return;
                    LoggedCachedPlacement |= message.Contains("HARD SNAP");
                    LoggedCachedRefusal |= message.Contains("snap-back skipped");
                }
            }
            internal static class Mod {
                internal static readonly LifecycleState Lifecycle = new();
                internal static readonly Logger Log = new();
            }
            internal sealed class LifecycleState { internal bool AcceptingWork = true; }

            internal static class RoadKeeper {
                /* MEMBERS */

                private static string _scenario = "";
                private static int _moves, _probes, _nudges, _orderChecks;
                private static Vector3 _target, _heading;

                private static bool SteepGroundBelow(Vector3 position) => false;
                private static void SoftNudge(LandVehicle vehicle, Vector3 closest, float lateral) => _nudges++;
                private static void ApplyPosition(LandVehicle vehicle, Vector3 position, Quaternion rotation, bool zeroVelocity) {
                    if (!zeroVelocity) throw new InvalidOperationException("cached placement must stop the vehicle");
                    _moves++;
                    _target = position;
                    _heading = rotation.Heading;
                    vehicle.transform.position = position;
                }
                private static bool TryGetPlacementBounds(LandVehicle vehicle, out Vector3 min, out Vector3 max,
                    bool rootAligned = true) {
                    min = new(-1f, -0.6f, -2f);
                    max = new(1f, 0.6f, 2f);
                    return true;
                }
                private static string DescribeOverlap(Collider hit) => "road/curb";
                private static bool HasObstacle(LandVehicle vehicle, Vector3 spot, Quaternion rotation, bool rear = false)
                    => false;
                private static bool TrySupportedSpot(LandVehicle vehicle, Vector3 projected, Quaternion rotation,
                    out Vector3 spot, out SnapRejection rejection, out string? firstOverlap, bool describeOverlap) {
                    _probes++;
                    if (_scenario == "reroute-during-probe") SpikeState.NavOrder++;
                    if (_scenario == "new-path-during-probe") vehicle.Agent!.path = new();
                    bool blocked = _scenario == "blocked" || _scenario.EndsWith("-break") ||
                        _scenario == "second-supported-point" && _probes == 1 ||
                        _scenario == "corner-after-rejections" && _probes <= 2 ||
                        _scenario == "corner-intermediate" && _probes <= 3 ||
                        _scenario == "corner-later-intermediate" && _probes <= 4;
                    firstOverlap = blocked && describeOverlap ? "road/curb" : null;
                    rejection = blocked ? SnapRejection.FootprintOverlap : SnapRejection.None;
                    spot = new(projected.x, projected.y + 0.6f, projected.z);
                    return !blocked;
                }

                public static (int, int, int, int, float, float, bool, bool, float, float, float, int) Run(string scenario) {
                    _scenario = scenario;
                    _moves = _probes = _nudges = _orderChecks = 0;
                    _target = _heading = default;
                    Physics.Hits = Array.Empty<Collider>();
                    SpikeState.Vehicle = new();
                    SpikeState.Operation.Active = SpikeState.PollingActive = SpikeCommands.Owned = true;
                    SpikeState.Reverse.Active = false;
                    SpikeState.NavOrder = 4;
                    SpikeState.OperationId = 3;
                    SpikeState.Operation.Generation = 5;
                    SpikeState.NavTarget = new(50f, 0f, 100f);
                    Mod.Log.LoggedCachedPlacement = Mod.Log.LoggedCachedRefusal = false;
                    Time.unscaledTime = 10f;
                    Reset();
                    _nextBlockedLogAt = 20f; // A recent live-route warning must not mask a cached refusal.
                    _nextCachedBlockedLogAt = 0f;
                    var vehicle = SpikeState.Vehicle!;
                    var agent = vehicle.Agent!;
                    var livePath = new Path();
                    bool corner = scenario.StartsWith("corner-") || scenario.EndsWith("-break");
                    if (corner) {
                        livePath.vectorPath.Add(new(0f, 0f, 0f));
                        livePath.vectorPath.Add(new(0f, 0.3f, 3f));
                        livePath.vectorPath.Add(scenario switch {
                            "vertical-break" => new(0f, 3.3f, 3f),
                            "nonfinite-break" => new(0f, float.NaN, 3f),
                            _ => new(0f, 0.3f, 3f)
                        });
                        livePath.vectorPath.Add(new(10f, 1.3f, 3f));
                    } else {
                        for (int z = -20; z <= 20; z++) livePath.vectorPath.Add(new(0f, z * 0.1f, z));
                    }
                    agent.path = livePath;
                    if (scenario.StartsWith("rescue-")) {
                        vehicle.transform.position = new(2f, 0.6f, 3f);
                        int placementChecks = 0;
                        TryRescueStartup(vehicle, agent,
                            allowPlacement: position => {
                                placementChecks++;
                                return scenario switch {
                                    "rescue-excludes-supported-root" => position.y > 1f,
                                    "rescue-root-recheck-refused" => placementChecks == 1,
                                    _ => true
                                };
                            },
                            revalidatePlacement: () => {
                                _orderChecks++;
                                return scenario != "rescue-order-changed";
                            });
                        return Result();
                    }
                    vehicle.transform.position = new(0f, 0.6f, 0f);
                    if (scenario != "no-history") Tick();

                    agent.path = null;
                    agent.NavigationCalculationInProgress = scenario != "no-calculation";
                    // A managed snapshot must still work after its native route is gone.
                    livePath.vectorPath.Clear();
                    Time.unscaledTime += scenario == "too-old" ? 1.01f : 0.3f;
                    vehicle.transform.position = scenario switch {
                        "too-far" => new(5.7f, -2.3f, 20f),
                        "soft-margin" => new(4f, 0.6f, 3f),
                        _ => corner ? new(-6f, -2.3f, 1f) : new(5.7f, -2.3f, 3f)
                    };
                    switch (scenario) {
                        case "different-vehicle": vehicle.Pointer = new(9); break;
                        case "different-agent": agent.Pointer = new(9); break;
                        case "different-operation": SpikeState.OperationId++; break;
                        case "different-generation": SpikeState.Operation.Generation++; break;
                        case "different-order": SpikeState.NavOrder++; break;
                        case "different-target": SpikeState.NavTarget = new(51f, 0f, 100f); break;
                        case "different-native-target": agent.TargetLocation = new(51f, 0f, 100f); break;
                        case "ready-path": agent.path = new(); break;
                        case "unowned": SpikeCommands.Owned = false; break;
                        case "player-driving": vehicle.DriverPlayer = new object(); break;
                        case "reverse": SpikeState.Reverse.Active = true; break;
                        case "native-reverse": agent.IsReversing = true; break;
                        case "stopped": SpikeState.PollingActive = false; break;
                        case "reset": Reset(); break;
                    }
                    Tick();
                    if (scenario == "same-order") {
                        vehicle.transform.position = new(5.7f, -2.3f, 3f);
                        Time.unscaledTime += 0.3f;
                        Tick();
                    }
                    return Result();
                }

                private static (int, int, int, int, float, float, bool, bool, float, float, float, int) Result() =>
                    (_moves, HardSnaps, _probes, _nudges, _target.z, _target.y, Mod.Log.LoggedCachedPlacement,
                     Mod.Log.LoggedCachedRefusal, _target.x, _heading.x, _heading.z, _orderChecks);

                public static bool MeasureFootprint(bool foreignCollider) {
                    var vehicle = new LandVehicle();
                    var self = new Collider();
                    self.transform.parent = vehicle.transform;
                    Physics.Hits = foreignCollider ? new[] { self, new Collider() } : new[] { self };
                    float diagonal = MathF.Sqrt(0.5f);
                    var proposed = Quaternion.LookRotation(new(diagonal, 0f, diagonal), Vector3.up);
                    bool free = IsPlacementFree(vehicle, new(8f, 4f, 5f), proposed,
                        new(-0.5f, -0.5f, -1f), new(1.5f, 0.7f, 3f),
                        out SnapRejection rejection, out string? overlap, describeOverlap: true);
                    return free != foreignCollider &&
                        rejection == (foreignCollider ? SnapRejection.FootprintOverlap : SnapRejection.None) &&
                        overlap == (foreignCollider ? "road/curb" : null) &&
                        Physics.Rotation == proposed &&
                        Vector3.Distance(Physics.Half, new(1f, 0.6f, 2f)) < 0.0001f &&
                        Vector3.Distance(Physics.Center, new(8f + 1.5f * diagonal, 4.1f, 5f + 0.5f * diagonal)) < 0.0001f;
                }

                public static bool RescueWithoutRoute() {
                    _scenario = "blocked";
                    _probes = 0;
                    Mod.Log.LoggedNoRouteRefusal = false;
                    var vehicle = new LandVehicle();
                    bool rescued = TryRescueStartup(vehicle, vehicle.Agent!);
                    return !rescued && _probes == 4 && Mod.Log.LoggedNoRouteRefusal;
                }
            }
        }
        """;
}

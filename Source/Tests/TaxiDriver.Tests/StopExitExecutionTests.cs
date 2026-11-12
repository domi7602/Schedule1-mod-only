using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes the STOP entry point and actual local-player exit/recovery block
/// with substituted native boundaries, destruction and ground probes.</summary>
public class StopExitExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(CompileReplay);

    [Fact]
    public void StopOutsideTaxi_DoesNotRecoverPlayerToTaxi()
    {
        Assert.Equal(new[] { 0, 0, 1, 0 }, Run("outside"));
    }

    [Theory]
    [InlineData("already-exited", 0, 0)]
    [InlineData("other-vehicle", 0, 0)]
    [InlineData("stale-flag-other-vehicle", 0, 0)]
    [InlineData("unreadable-before", 0, 0)]
    [InlineData("invalid-network", 0, 0)]
    [InlineData("foreign-taxi", 0, 0)]
    [InlineData("passenger", 1, 1)]
    [InlineData("exit-noop", 1, 0)]
    [InlineData("exit-throws", 1, 0)]
    [InlineData("exit-clears-flag-only", 1, 0)]
    [InlineData("exit-enters-other", 1, 0)]
    [InlineData("exit-replaces-player", 1, 0)]
    [InlineData("unreadable-after", 1, 0)]
    public void StopRecoversOnlyExactPassengerAfterVerifiedExit(string scenario, int exits, int snaps)
    {
        int[] result = Run(scenario);
        Assert.Equal(exits, result[0]);
        Assert.Equal(snaps, result[1]);
    }

    [Theory]
    [InlineData("phone-covers-passenger", 1, 1, 1, 1)]
    [InlineData("phone-covers-outside", 0, 0, 1, 0)]
    [InlineData("other-screen", 0, 0, 0, 0)]
    [InlineData("other-modal", 0, 0, 0, 0)]
    [InlineData("other-state-under-phone", 0, 0, 0, 0)]
    [InlineData("wrong-parent", 0, 0, 0, 0)]
    [InlineData("moving-backward", 0, 0, 0, 0)]
    [InlineData("reversing", 0, 0, 0, 0)]
    [InlineData("still-driving", 0, 0, 0, 0)]
    [InlineData("close-noop", 0, 0, 0, 1)]
    [InlineData("order-changed-during-close", 0, 0, 0, 1)]
    [InlineData("player-changed-during-close", 0, 0, 0, 1)]
    [InlineData("order-changed-during-exit", 1, 0, 0, 1)]
    [InlineData("non-stop-cleanup", 1, 0, 0, 0)]
    public void StopClosesOnlyTheCoveringPhoneForAnExactStoppedPassenger(
        string scenario, int exits, int snaps, int ok, int closes)
    {
        Assert.Equal(new[] { exits, snaps, ok, closes }, Run(scenario));
    }

    [Theory]
    [InlineData("halt", 1, 1, 1, 1)]
    [InlineData("repeat-stop", 1, 1, 1, 1)]
    [InlineData("repeat-stop-timeout", 0, 0, 0, 0)]
    [InlineData("manual-exit", 1, 0, 0, 0)]
    [InlineData("paused", 1, 1, 1, 1)]
    [InlineData("still-driving", 1, 1, 1, 1)]
    [InlineData("timeout", 0, 0, 0, 0)]
    [InlineData("order", 0, 0, 0, 0)]
    [InlineData("operation", 0, 0, 0, 0)]
    [InlineData("player", 0, 0, 0, 0)]
    [InlineData("network", 0, 0, 0, 0)]
    [InlineData("taxi", 0, 0, 0, 0)]
    [InlineData("scene", 0, 0, 0, 0)]
    [InlineData("scene-reloaded", 0, 0, 0, 0)]
    [InlineData("authority", 0, 0, 0, 0)]
    [InlineData("driver-takeover", 0, 0, 0, 0)]
    public void ConfirmedStopContinuesOnceAfterAnExactNativeHalt(
        string scenario, int completions, int exits, int snaps, int closes)
    {
        int[] result = (int[])Replay.Value.DeclaringType!.GetMethod("RunDeferred")!
            .Invoke(null, new object[] { scenario })!;
        Assert.Equal(new[] { 1, 1, completions, exits, snaps, closes }, result);
    }

    private static int[] Run(string scenario) =>
        (int[])Replay.Value.Invoke(null, new object[] { scenario })!;

    private static MethodInfo CompileReplay()
    {
        var tree = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs"));
        var spikeCommands = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Single(c => c.Identifier.Text == "SpikeCommands");
        var cleanup = spikeCommands.Members.OfType<MethodDeclarationSyntax>()
            .Single(m => m.Identifier.Text == "FinishCleanup");
        var deferredMembers = new HashSet<string> {
            "Stop", "Cleanup", "TryExposeStopExitState", "TryReadStopHalt",
            "StopExitWaitSeconds", "PendingStopExit", "_pendingStopExit", "ReportStopBraking",
            "CancelPendingStopExit", "IsStopExitCurrent", "TryDeferStopExit",
            "IsStopExitPassengerCurrent", "TickPendingStopExit"
        };
        string stopCode = string.Join("\n", spikeCommands.Members.Where(member => member switch
        {
            MethodDeclarationSyntax method => deferredMembers.Contains(method.Identifier.Text),
            RecordDeclarationSyntax record => deferredMembers.Contains(record.Identifier.Text),
            FieldDeclarationSyntax field => field.Declaration.Variables.Any(v => deferredMembers.Contains(v.Identifier.Text)),
            _ => false
        }));
        // Keep the actual statements up to the independent navigation-stop/destruction stage.
        var vehicleBlock = cleanup.Body!.Statements.OfType<IfStatementSyntax>()
            .Single(s => s.Condition.ToString() == "veh != null");
        var statements = ((BlockSyntax)vehicleBlock.Statement).Statements
            .TakeWhile(s => !s.DescendantNodes().OfType<VariableDeclarationSyntax>()
                .Any(v => v.Type.ToString() == "VehicleAgent?"));
        string source = Harness.Replace("/* EXIT_BLOCK */", string.Join("\n", statements))
            .Replace("/* STOP_CODE */", stopCode);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("StopExitReplay", new[] { CSharpSyntaxTree.ParseText(source) },
            references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        using System.Collections.Generic;
        public class NativeObject {
            public IntPtr Pointer = new(1);
            public bool WasCollected;
        }
        public sealed class Player : NativeObject {
            public static Player Local;
            public NativeObject Current;
            public bool ThrowRead;
            public NativeObject CurrentVehicle {
                get { if (ThrowRead) throw new Exception("unreadable"); return Current; }
                set { Current = value; }
            }
        }
        public sealed class MonoState : NativeObject {
            public MonoStateMachine _defaultParent;
        }
        public sealed class MonoStateMachine : NativeObject {
            public List<MonoState> Stack = new();
            public MonoState Peek() => Stack.Count == 0 ? null : Stack[^1];
        }
        public sealed class GameplayMenu : NativeObject {
            public enum EGameplayScreen { Phone, Character }
            public static GameplayMenu Instance;
            public static bool InstanceExists => Instance != null;
            public EGameplayScreen CurrentScreen;
            public MonoState State;
            public bool IsOpen = true;
            public void Close() {
                Replay.MenuCloses++;
                if (Replay.Scenario == "close-noop") return;
                State._defaultParent.Stack.RemoveAt(State._defaultParent.Stack.Count - 1);
                IsOpen = false;
                if (Replay.Scenario == "order-changed-during-close") SpikeState.NavOrder++;
                if (Replay.Scenario == "player-changed-during-close") Player.Local = new() { Pointer = new(9) };
            }
        }
        public sealed class VehicleAgent {
            public bool AutoDriving;
        }
        public sealed class LandVehicle : NativeObject {
            public NativeObject NetworkObject = new() { Pointer = new(2) };
            public MonoState State;
            public VehicleAgent Agent = new();
            public float Speed_Kmh;
            public bool IsReversing;
            public bool LocalPlayerIsInVehicle;
            public Player DriverPlayer;
            public void RemoveNPCOccupant(NPC npc) { }
            public void ExitVehicle() {
                Replay.Exits++;
                if (State._defaultParent.Peek().Pointer != State.Pointer) return;
                if (Replay.Scenario == "exit-noop") return;
                LocalPlayerIsInVehicle = false;
                if (Replay.Scenario == "exit-clears-flag-only") return;
                Player.Local.CurrentVehicle = null;
                if (Replay.Scenario == "order-changed-during-exit") SpikeState.NavOrder++;
                if (Replay.Scenario == "exit-throws") throw new Exception("exit failed after mutation");
                if (Replay.Scenario == "exit-enters-other") Player.Local.CurrentVehicle = new() { Pointer = new(3) };
                if (Replay.Scenario == "exit-replaces-player") Player.Local = new() { Pointer = new(9) };
                if (Replay.Scenario == "unreadable-after") Player.Local.ThrowRead = true;
            }
        }
        public sealed class NPC { public void ExitVehicle() { } }
        public enum VehicleOwnershipKind { None, Invalid, Foreign, Owned, DestroyPending, DestroyUnknown }
        public sealed class VehicleOwnershipLedger {
            public IntPtr TrackedPointer;
            public int DestroyAttempts;
            public const int MaxDestroyAttempts = 2;
            public bool DestroyPending;
            public bool DestroyUnknown;
        }
        public sealed class Logger { public void Error(string s) { } public void Warn(string s) { } }
        public sealed class Lifecycle { public bool AcceptingWork = true; }
        public static class Mod {
            public static Logger Log = new();
            public static Lifecycle Lifecycle = new();
        }
        public static class Time {
            public static float time;
            public static float unscaledTime;
            public static float timeScale;
        }
        public struct Vector3 { public static Vector3 zero; }
        public static class SpikeState {
            public static LandVehicle Vehicle;
            public static int NavOrder;
            public static long OperationId;
            public static long ServiceStatusRevision;
            public static bool RideActive;
            public static bool PollingActive;
            public static bool NavToPlayer;
            public static float NavReDispatchAt;
            public static Vector3 NavReDispatchTarget;
            public static Vector3? StandSpawnPosition;
            public static NPC DriverNpc;
            public static VehicleOwnershipLedger VehicleOwnership = new();
            public static void SetServiceStatus(string s) { ServiceStatusRevision++; }
            public static void CancelPendingSpawn() { }
            public static void ResetAutoRun() { }
            public static bool Reset() => true;
            public static int NextNavOrder() => ++NavOrder;
        }
        public static class SceneGate { public static bool IsInMainScene = true; }
        public static class NavigationArrivalPolicy {
            public static bool IsConfirmedHalt(bool readable, float speed, bool reversing) =>
                readable && !reversing && float.IsFinite(speed) && Math.Abs(speed) <= 1f;
        }
        public static class Replay {
            public static int Exits, Snaps, MenuCloses, Completions, RideEnds;
            public static bool Authority;
            public static string Scenario;
            private static void Print(string s) { }
            private static bool RefuseService(string s) { SpikeState.SetServiceStatus(s); return false; }
            private static bool HasAuthority() => Authority;
            private static bool SafePlayerInVehicle(LandVehicle v) => v.LocalPlayerIsInVehicle;
            private static bool IsOwnedTaxi(LandVehicle v) => Scenario != "foreign-taxi";
            private static VehicleOwnershipKind ClassifyTaxiVehicle(LandVehicle v) => VehicleOwnershipKind.Owned;
            private static bool ReconcileDeadTaxi() => false;
            private static bool SafeInVehicle(NPC npc) => false;
            private static void EndRide(string reason) {
                RideEnds++;
                SpikeState.RideActive = false;
                SpikeState.NextNavOrder();
            }
            private static void SafeExitGroundSnap(LandVehicle v) { Snaps++; }
            /* STOP_CODE */
            private static bool FinishCleanup(bool ok, bool explicitRetry, PendingStopExit pending = null) {
                Completions++;
                return ok && ExitBlock(SpikeState.Vehicle, explicitRetry, pending);
            }
            private static bool ExitBlock(LandVehicle veh, bool explicitRetry, PendingStopExit pending = null) {
                bool ok = true;
                /* EXIT_BLOCK */
                return ok;
            }
            private static LandVehicle Setup(string scenario) {
                Scenario = scenario;
                Exits = Snaps = MenuCloses = Completions = RideEnds = 0;
                Authority = true;
                SceneGate.IsInMainScene = true;
                Time.time = Time.unscaledTime = 100f;
                Time.timeScale = 1f;
                CancelPendingStopExit();
                Player.Local = new();
                var parent = new MonoStateMachine { Pointer = new(10) };
                var veh = new LandVehicle { State = new MonoState { Pointer = new(11), _defaultParent = parent } };
                SpikeState.Vehicle = veh;
                SpikeState.NavOrder = 100;
                parent.Stack.Add(veh.State);
                GameplayMenu.Instance = null;
                bool phoneOpen = scenario is "phone-covers-passenger" or "phone-covers-outside" or "other-screen" or
                    "other-modal" or "other-state-under-phone" or "wrong-parent" or "moving-backward" or "reversing" or
                    "still-driving" or "close-noop" or "order-changed-during-close" or "player-changed-during-close" or
                    "order-changed-during-exit" or "non-stop-cleanup" || scenario.StartsWith("deferred:");
                if (phoneOpen) {
                    var menuState = new MonoState { Pointer = new(12), _defaultParent = parent };
                    GameplayMenu.Instance = new() { State = menuState };
                    if (scenario == "other-screen") GameplayMenu.Instance.CurrentScreen = GameplayMenu.EGameplayScreen.Character;
                    if (scenario == "wrong-parent") menuState._defaultParent = new() { Pointer = new(13) };
                    if (scenario == "other-state-under-phone") parent.Stack.Add(new() { Pointer = new(14) });
                    parent.Stack.Add(menuState);
                    if (scenario == "other-modal") parent.Stack.Add(new() { Pointer = new(15) });
                    if (scenario == "moving-backward") veh.Speed_Kmh = -5f;
                    if (scenario == "reversing") veh.IsReversing = true;
                    if (scenario == "still-driving") veh.Agent.AutoDriving = true;
                }
                bool passenger = scenario != "outside" && scenario != "already-exited" &&
                    scenario != "other-vehicle" && scenario != "phone-covers-outside";
                veh.LocalPlayerIsInVehicle = passenger;
                // Distinct wrappers for the same native network object: identity is not CLR reference equality.
                Player.Local.Current = passenger ? new() { Pointer = new(2) } : null;
                if (scenario.Contains("other-vehicle")) Player.Local.Current = new() { Pointer = new(3) };
                if (scenario == "invalid-network") veh.NetworkObject.Pointer = IntPtr.Zero;
                if (scenario == "unreadable-before") Player.Local.ThrowRead = true;
                return veh;
            }
            public static int[] Run(string scenario) {
                bool ok = ExitBlock(Setup(scenario), explicitRetry: scenario != "non-stop-cleanup");
                return new[] { Exits, Snaps, ok ? 1 : 0, MenuCloses };
            }
            public static int[] RunDeferred(string scenario) {
                var veh = Setup("deferred:" + scenario);
                SpikeState.RideActive = SpikeState.PollingActive = SpikeState.NavToPlayer = true;
                SpikeState.NavReDispatchAt = 123f;
                int oldOrder = SpikeState.NavOrder;
                veh.Speed_Kmh = 31.5f;
                bool stoppedImmediately = Stop();
                bool cancelled = !stoppedImmediately && !SpikeState.RideActive && !SpikeState.PollingActive &&
                    !SpikeState.NavToPlayer && SpikeState.NavReDispatchAt == 0f && SpikeState.NavOrder != oldOrder &&
                    TickPendingStopExit() && Exits == 0 && MenuCloses == 0 && Completions == 0;

                Time.time += 2f;
                Time.unscaledTime += 2f;
                if (scenario is "repeat-stop" or "repeat-stop-timeout") Stop();
                if (scenario is "timeout" or "repeat-stop-timeout") {
                    Time.time += 2f;
                    Time.unscaledTime += 2f;
                }
                if (scenario == "order") SpikeState.NextNavOrder();
                if (scenario == "operation") SpikeState.OperationId++;
                if (scenario == "player") Player.Local.Pointer = new(9);
                if (scenario == "network") veh.NetworkObject.Pointer = new(9);
                if (scenario == "taxi") SpikeState.Vehicle = new();
                if (scenario == "scene") SceneGate.IsInMainScene = false;
                if (scenario == "scene-reloaded") CancelPendingStopExit();
                if (scenario == "authority") Authority = false;
                if (scenario == "driver-takeover") veh.DriverPlayer = Player.Local;
                if (scenario == "manual-exit") {
                    veh.LocalPlayerIsInVehicle = false;
                    Player.Local.CurrentVehicle = null;
                }
                veh.Speed_Kmh = 0.2f;
                if (scenario == "paused") {
                    Time.timeScale = 0f;
                    Time.unscaledTime += 10f;
                    cancelled &= TickPendingStopExit() && Exits == 0 && MenuCloses == 0 && Completions == 0;
                    Time.timeScale = 1f;
                }
                if (scenario == "still-driving") {
                    veh.Agent.AutoDriving = true;
                    cancelled &= TickPendingStopExit() && Exits == 0 && MenuCloses == 0 && Completions == 0;
                    veh.Agent.AutoDriving = false;
                }
                TickPendingStopExit();
                TickPendingStopExit();
                return new[] { cancelled ? 1 : 0, RideEnds, Completions, Exits, Snaps, MenuCloses };
            }
        }
        """;
}

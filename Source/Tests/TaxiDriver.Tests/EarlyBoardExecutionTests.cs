using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

// Executes the production TickRide boarding prefix and early-board transition; native boundaries are substituted.
public class EarlyBoardExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(Compile);
    [Fact]
    public void UpdateHandlesEarlyBoardBeforeOldPickupAndCallbacks()
    {
        var root = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs")).GetRoot();
        string update = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == "Update").ToString();
        int board = update.IndexOf("SpikeCommands.TickEarlyBoard()", StringComparison.Ordinal);
        Assert.True(board >= 0 && board < update.IndexOf("TickAutoRun();", StringComparison.Ordinal));
        Assert.True(board < update.IndexOf("TickNavigationCallback();", StringComparison.Ordinal));
    }

    [Fact]
    public void PickupBoardingCancelsOldControlsBeforeStartingRide()
    {
        Assert.Equal("cancel,seat,stop,driver,seat,start", Run("pickup"));
    }
    [Fact]
    public void SelectedDestinationDispatchesInsteadOfResumingPickup() =>
        Assert.Equal("cancel,seat,stop,driver,seat,start,drive", Run("selected"));

    [Theory]
    [InlineData("repeat", "cancel,seat,stop,driver,seat,start")]
    [InlineData("confirmed", "cancel,seat,stop,driver,seat,start")]
    [InlineData("expired", "cancel,seat,stop,driver,end")]
    public void BoardingContinuationIsBoundedAndIdempotent(string scenario, string expected) => Assert.Equal(expected, Run(scenario));

    [Theory]
    [InlineData("foreign")]
    [InlineData("client")]
    [InlineData("other-vehicle")]
    [InlineData("outside")]
    [InlineData("debounce")]
    [InlineData("stopped")]
    public void UnconfirmedOrCancelledPickupDoesNotTakeOver(string scenario) => Assert.Equal("", Run(scenario));

    [Fact]
    public void ProvisionalDriverDoesNotStartOnFollowingTick() =>
        Assert.Equal("cancel,seat,stop,driver", Run("provisional"));

    [Fact]
    public void ReplacementOrderDuringSeatMoveIsNotStopped() => Assert.Equal("cancel,seat", Run("replacement"));

    [Fact]
    public void SeatRefusalDoesNotStartOrRecoverDriver() => Assert.Equal("cancel,seat", Run("seat-failed"));

    [Theory]
    [InlineData("cancel")]
    [InlineData("seat")]
    [InlineData("stop")]
    [InlineData("driver")]
    public void LifecycleChangeAtNativeBoundaryStopsContinuation(string boundary)
    {
        string result = Run("lifecycle-" + boundary);
        Assert.DoesNotContain("start", result);
        Assert.Equal(boundary, result.Split(',').Last());
    }

    [Theory]
    [InlineData("cancel")]
    [InlineData("seat")]
    [InlineData("stop")]
    [InlineData("driver")]
    public void PauseAtNativeBoundaryDefersAndResumesHandoff(string boundary)
    {
        string result = Run("pause-" + boundary);
        Assert.EndsWith("seat,start", result);
    }

    [Theory]
    [InlineData("throw-stop")]
    [InlineData("ineffective-stop")]
    [InlineData("pending-calculation")]
    [InlineData("unreadable-stop")]
    [InlineData("ineffective-reverse")]
    public void UnconfirmedNativeCancellationBlocksAllFollowingTicks(string scenario)
    {
        string result = Run(scenario);
        Assert.DoesNotContain("driver", result);
        Assert.DoesNotContain("start", result);
        Assert.Contains("park", result);
        Assert.Contains("error", result);
    }

    [Theory]
    [InlineData("pause-start-seat")]
    [InlineData("lifecycle-start-seat")]
    [InlineData("normal-pause-seat")]
    [InlineData("normal-lifecycle-seat")]
    public void StartRideRechecksNativeSeatBoundary(string scenario)
    {
        string result = Run(scenario);
        if (scenario.Contains("pause")) Assert.EndsWith("seat,start", result);
        else Assert.DoesNotContain("start", result);
    }

    [Fact]
    public void ProvisionalNextFrameRechecksNativeCancellation() =>
        Assert.DoesNotContain("start", Run("provisional-reactivated"));

    [Fact]
    public void SuccessfulHandoffKeepsActiveRideIdempotency() =>
        Assert.DoesNotContain("error", Run("duplicate-active"));

    [Fact]
    public void DriverBoundaryCannotLeaveUnsafeProvisionalWait() =>
        Assert.Contains("error", Run("provisional-driver-reactivated"));

    private static string Run(string scenario) => (string)Replay.Value.Invoke(null, new object[] { scenario })!;
    private static MethodInfo Compile()
    {
        var runner = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs")).GetRoot();
        var tick = runner.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == "TickRide");
        var prefix = tick.Body!.Statements.TakeWhile(s => s is not IfStatementSyntax i || i.Condition.ToString() != "!SpikeState.RideActive");
        var commands = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs")).GetRoot();
        var transition = commands.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(m => m.Identifier.Text == "TickEarlyBoard" || m.Identifier.Text == "BoardingNavigationCancelled").Select(m => m.ToString()).Concat(commands.DescendantNodes().OfType<FieldDeclarationSyntax>().Where(f => f.ToString().Contains("_earlyBoard")).Select(f => f.ToString()));
        var start = commands.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == "StartRide");
        string stop = string.Join("\n", runner.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(m => m.Identifier.Text == "StopDriving"));
        string park = string.Join("\n", commands.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(m => m.Identifier.Text == "ParkCar"));
        string source = Harness.Replace("/* STOP */", stop).Replace("/* PARK */", park).Replace("/* TICK */", string.Join("\n", prefix)).Replace("/* TRANSITION */", string.Join("\n", transition)).Replace("/* START */", start.ToString());
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator).Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("EarlyBoardReplay", new[] { CSharpSyntaxTree.ParseText(source) }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        Assert.True(result.Success, string.Join("\n", result.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay")!.GetMethod("Run")!;
    }
    private const string Harness = """
        using System;
        using System.Collections.Generic;
        public class Native { public IntPtr Pointer = new(1); public bool WasCollected; }
        public class Player : Native { public static Player Local; public Native CurrentVehicle; }
        public class Agent : Native {
            public bool AutoDriving = true, NavigationCalculationInProgress, IsReversing;
            public void StopNavigating() { if (Replay.Scenario == "throw-stop") throw new Exception("stop failed"); if (Replay.Scenario != "ineffective-stop") AutoDriving = false; }
            public void StopReversing() { if (Replay.Scenario != "ineffective-reverse") IsReversing = false; }
        }
        public class LandVehicle : Native { public bool LocalPlayerIsInVehicle = true; public Native NetworkObject = new(); private Agent agent = new(); public Agent Agent { get { if (Replay.Scenario == "unreadable-stop") throw new Exception("unreadable"); return agent; } } public Player DriverPlayer; public void ExitVehicle() {} }
        public class Reverse { public void End() {} }
        public class Operation { public void Terminate(string reason, bool failed) {} }
        public static class TaxiAI { public static void StopPatrol(string reason) {} }
        public static class TaxiLog { public static void Verbose(string s) {} }
        public static class ReverseSupervisionPolicy { public static bool NativeCleanupPermitted(bool authority, bool owned, bool player) => authority && owned && !player; }
        public static class CleanupGuard { public static void Run(string name, Action action, Action<string, Exception> report) { try { action(); } catch(Exception ex) { report(name, ex); } } }
        public class NPC {}
        public struct Vector3 {}
        public enum RideProtectionPatch { None }
        public static class RideProtectionPolicy { public static RideProtectionPatch Missing(int p) => RideProtectionPatch.None; }
        public static class RideLocks { public static bool Patched = true; public static int Installed; public static void LockTrunk(LandVehicle v) { Replay.Events.Add("start"); } }
        public static class NativeActuatorPatch { public static bool Healthy = true; public static void RequestPark(LandVehicle v) { if (Replay.IsFailure) Replay.Events.Add("park"); } }
        public static class Time { public static float unscaledTime = 20; public static float timeScale = 1; }
        public static class SpikeState {
            public static int NavOrder; public static float NavReDispatchAt; public static Reverse Reverse = new(); public static Operation Operation = new(); public static void NextNavOrder() { NavOrder++; }
            public static LandVehicle Vehicle;
            public static bool RideActive, NavToPlayer, AutoRunning, RideAwaitingBoard;
            public static bool RideBoarded, RidePassengerMode, RideArrived, RideAwaitingDestination, RideDestinationPicked;
            public static int RideSerial;
            public static Vector3? RideDestination;
            public static string RideDestinationName;
            public static NPC DriverNpc = new();
            public static float LastExitAt;
            public static object SeatVerifyNpc;
            public static void ResetAutoRun() { AutoRunning = false; }
        }
        public class Lifecycle { public bool AcceptingWork = true; }
        public static class Mod { public static Logger Log = new(); public static Lifecycle Lifecycle = new(); }
        public class Logger { public void Warn(string s) {} public void Error(string s) { if (Replay.IsFailure || Replay.Scenario == "duplicate-active" || Replay.Scenario == "provisional-driver-reactivated") Replay.Events.Add("error"); } public void Info(string s) {} }
        public static class SpikeRunner {
            /* STOP */
            public static bool PlayerDriver(LandVehicle v) => v.DriverPlayer != null;
            public static void ResetDrivingState() { Replay.Boundary(Replay.Events.Contains("seat") ? "stop" : "cancel"); SpikeState.NavToPlayer = false; }
            public static void TickRide() { Replay.Tick++; /* TICK */ }
        }
        public static class SpikeCommands {
            public const float ExitDebounceSeconds = 1;
            public static bool IsOwnedTaxi(LandVehicle v) => Replay.Scenario != "foreign";
            public static bool HasAuthority() => Replay.Scenario != "client";
            public static bool DriverAtWheel(LandVehicle v) => Replay.Scenario != "expired" && (Replay.Events.Contains("driver") || Replay.Scenario.StartsWith("normal-")) && SpikeState.SeatVerifyNpc == null;
            public static void TickSeatVerify() { if ((Replay.Scenario == "confirmed" || Replay.Scenario == "expired" || Replay.Scenario == "provisional-reactivated") && Replay.Tick > 1) SpikeState.SeatVerifyNpc = null; }
            public static bool EnsurePassengerSeat(LandVehicle v) { Replay.Boundary("seat"); if (Replay.Events.FindAll(e => e == "seat").Count == 2) Replay.Interrupt("start-seat"); v.DriverPlayer = null; if (Replay.Scenario == "replacement") SpikeState.NavOrder++; return Replay.Scenario != "seat-failed"; }
            public static bool EnsureDriverSeat(LandVehicle v) { Replay.Boundary("driver"); if (Replay.Scenario == "provisional-driver-reactivated") v.Agent.AutoDriving = true; if (Replay.Scenario.StartsWith("provisional") || Replay.Scenario == "confirmed" || Replay.Scenario == "expired") SpikeState.SeatVerifyNpc = new(); return true; }
            public static bool RequireOwnedTaxi(LandVehicle v, string reason) => HasAuthority() && IsOwnedTaxi(v);
            public static void EndRide(string reason, bool stopNavigation = true) { SpikeState.RideActive = SpikeState.RideAwaitingBoard = false; Replay.Events.Add("end"); }
            /* PARK */
            public static void ReportCleanupFailure(string name, Exception ex) {}
            public static bool SafePlayerInVehicle(LandVehicle v) => v.LocalPlayerIsInVehicle;
            public static void SafeExitGroundSnap(LandVehicle v) {}
            public static string codeOf(LandVehicle v) => "taxi";
            public static string SafeId(NPC n) => "taxi_driver";
            public static string Fmt(Vector3 v) => "target";
            public static string SafeDriverPlayerName(LandVehicle v) => "null";
            // Navigation is substituted in this boarding-only host; its real request
            // fences and root failure cleanup execute in StreetCandidateExecutionTests.
            public class DriveRequest {
                public static DriveRequest Capture(LandVehicle v, bool allowPassengerSeatRepair = false) => new();
                public static DriveRequest CreateReceipt(LandVehicle v) => new();
                public bool TryAcquire(bool allowPassengerSeatRepair = false) => true;
                public void RefuseUnavailable(bool passenger) { }
                public bool IsCurrent(bool allowInactive = false) => Mod.Lifecycle.AcceptingWork;
            }
            public static bool StartRideDrive(NPC n, LandVehicle v, Vector3 target, string caller, DriveRequest preparedRequest = null) { Replay.Events.Add("drive"); return true; }
            /* START */
            /* TRANSITION */
        }
        public static class Replay {
            public static List<string> Events = new(); public static string Scenario; public static bool IsFailure => Scenario.Contains("stop") && Scenario != "stopped" || Scenario == "pending-calculation" || Scenario == "ineffective-reverse";
            public static int Tick; public static void Interrupt(string name) { if (Scenario == "lifecycle-" + name || Scenario == "normal-lifecycle-" + name) Mod.Lifecycle.AcceptingWork = false; if (Scenario == "pause-" + name || Scenario == "normal-pause-" + name) Time.timeScale = 0; }
            public static void Boundary(string name) { Events.Add(name); Interrupt(name); }
            public static string Run(string scenario) {
                Scenario = scenario; Tick = 0; Events.Clear(); Mod.Lifecycle.AcceptingWork = true; Time.timeScale = 1;
                SpikeState.Vehicle = new(); Player.Local = new() { CurrentVehicle = new() };
                SpikeState.Vehicle.DriverPlayer = Player.Local;
                if (scenario == "pending-calculation") SpikeState.Vehicle.Agent.NavigationCalculationInProgress = true;
                if (scenario == "ineffective-reverse") SpikeState.Vehicle.Agent.IsReversing = true;
                SpikeState.NavToPlayer = SpikeState.AutoRunning = true;
                SpikeState.RideActive = SpikeState.RideAwaitingBoard = false;
                SpikeState.RideAwaitingDestination = false;
                SpikeState.RideDestinationPicked = scenario == "selected";
                SpikeState.RideDestination = scenario == "selected" ? new Vector3() : null;
                SpikeState.LastExitAt = scenario == "debounce" ? 20 : 0; SpikeState.SeatVerifyNpc = null;
                if (scenario == "other-vehicle") Player.Local.CurrentVehicle.Pointer = new(2);
                if (scenario == "outside") SpikeState.Vehicle.LocalPlayerIsInVehicle = false;
                if (scenario.StartsWith("normal-")) { SpikeState.NavToPlayer = SpikeState.AutoRunning = false; SpikeState.Vehicle.Agent.AutoDriving = false; SpikeState.Vehicle.DriverPlayer = null; SpikeState.RideAwaitingBoard = true; }
                if (scenario == "stopped") SpikeState.NavToPlayer = SpikeState.AutoRunning = false;
                SpikeRunner.TickRide();
                if (scenario == "provisional" || scenario == "repeat" || scenario == "confirmed" || scenario == "expired") SpikeRunner.TickRide();
                if (scenario == "duplicate-active") { SpikeState.Vehicle.Agent.AutoDriving = true; SpikeCommands.StartRide("duplicate"); }
                if (scenario == "provisional-reactivated") { SpikeState.Vehicle.Agent.AutoDriving = true; SpikeRunner.TickRide(); }
                if (IsFailure) { SpikeRunner.TickRide(); SpikeRunner.TickRide(); }
                if (scenario.Contains("pause-")) { if (Events.Contains("start")) throw new Exception("started while paused"); Scenario = scenario.StartsWith("normal-") ? "normal-resumed" : "resumed"; Time.timeScale = 1; SpikeRunner.TickRide(); }
                if (scenario == "pickup" && (!SpikeState.RideActive || !SpikeState.RideAwaitingDestination || SpikeState.AutoRunning || SpikeState.NavToPlayer)) throw new Exception("no-destination ride must wait without pickup controls");
                if (scenario == "selected" && (!SpikeState.RideActive || SpikeState.RideAwaitingDestination)) throw new Exception("selected ride must dispatch");
                return string.Join(",", Events);
            }
        }
        """;
}

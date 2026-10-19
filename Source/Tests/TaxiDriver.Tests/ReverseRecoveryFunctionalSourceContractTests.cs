using System;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Runtime wiring guards for the reverse-recovery repair (brief §6–§8). Unity/IL2CPP
/// execution is NOT covered; these pin the corrected runtime wiring that the pure
/// policies cannot prove on their own.
/// </summary>
public class ReverseRecoveryFunctionalSourceContractTests
{
    private static string Runner => DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");

    private static string Section(string start, string end) =>
        DriverSafetySourceContractTests.Section(Runner, start, end);

    [Fact]
    public void SelfTestHandoff_SpendsBudgetWithoutRecordingAManoeuvreStall()
    {
        string patrol = Section("private static void TickPatrolSupervision", "private static void TickReDispatch");
        // The 8 s start self-test hand-off is NOT a failed reverse attempt.
        Assert.Contains("TryBeginRecovery(veh.Agent, veh, maneuverPlanned: false)", patrol);
        string recovery = Section("private static bool TryBeginRecovery", "internal static void ArmStartupRecovery");
        // The spatial recurrence history is only advanced for a planned manoeuvre.
        Assert.Contains("maneuverPlanned", recovery);
        Assert.Contains("if (!maneuverPlanned)", recovery);
    }

    [Fact]
    public void RealStallPaths_PlanAManoeuvreAndKeepTheOperationBudget()
    {
        string stuck = Section("private static void RecoverStuck", "private static bool RearIsClear");
        Assert.Contains("TryBeginRecovery(agent, veh, maneuverPlanned: true)", stuck);
        string startup = Section("private static bool TickStartupRecovery", "private static void TickPatrolSupervision");
        Assert.Contains("TryBeginRecovery(agent, veh, maneuverPlanned: true)", startup);
        string patrol = Section("private static void TickPatrolSupervision", "private static void TickReDispatch");
        Assert.Contains("TryBeginRecovery(veh.Agent, veh, maneuverPlanned: true)", patrol);
    }

    [Fact]
    public void Redispatch_UsesDirectedReverseNotArbitraryDisplacement()
    {
        string redispatch = Section("private static void TickReDispatch", "internal static void ReDispatch");
        Assert.Contains("DirectedNetReverseMeters", redispatch);
        Assert.Contains("MovedConfirmed", redispatch);
        Assert.DoesNotContain("movedMeters >= ProgressEpsilonMeters", redispatch);
        // Review D4/D15: the managed reverse intent is revoked (Transition, consumer denied) and
        // the old native callback identity invalidated BEFORE the native stop, so a synchronous
        // Stopped callback of the old order cannot act on the new transition.
        int transition = redispatch.IndexOf("SpikeState.Reverse.BeginTransition()", StringComparison.Ordinal);
        int invalidate = redispatch.IndexOf("SpikeState.NextNavOrder()", StringComparison.Ordinal);
        int stop = redispatch.IndexOf("StopReversing()", StringComparison.Ordinal);
        Assert.True(transition >= 0 && stop > transition, "The controlled transition must begin before the native stop.");
        Assert.True(invalidate >= 0 && stop > invalidate, "The old native callback identity must be invalidated before the native stop.");
        Assert.Contains("SpikeState.Reverse.End()", redispatch);
    }

    [Fact]
    public void Redispatch_GatesForwardOnTheFrontClearance()
    {
        string redispatch = Section("private static void TickReDispatch", "internal static void ReDispatch");
        Assert.Contains("CanDispatchForward", redispatch);
        Assert.Contains("HasObstacle", redispatch);
    }

    [Fact]
    public void ReverseStart_ConsumesTheAttemptBeforeTheNativeCallAndRechecksAfter()
    {
        string reverse = Section("private static bool TryStartReverse", "private static bool TickStartupRecovery");
        Assert.Contains("agent.StartReverse();", reverse);
        Assert.Contains("SpikeState.Reverse.Begin(", reverse);
        // Operation + order captured before the native call, matched after it.
        Assert.Contains("SpikeState.OperationId", reverse);
        Assert.Contains("SpikeState.Reverse.Matches(", reverse);
        // Health/authority/driver gates.
        Assert.Contains("NativeActuatorPatch.Healthy", reverse);
        Assert.Contains("DriverAtWheel", reverse);
        Assert.Contains("if (!SpikeState.PollingActive)", reverse);
        int begin = reverse.IndexOf("SpikeState.Reverse.Begin(", StringComparison.Ordinal);
        int start = reverse.IndexOf("agent.StartReverse();", StringComparison.Ordinal);
        Assert.True(begin >= 0 && start > begin, "The manoeuvre must be captured before StartReverse.");
    }

    [Fact]
    public void Supervisor_IsIndependentOfPollingAndRunsBeforeRideFinalization()
    {
        string update = Section("internal static void Update()", "private static void ShiftPauseDeadlines");
        int supervisor = update.IndexOf("TickReverseSupervision();", StringComparison.Ordinal);
        int ride = update.IndexOf("TickRide();", StringComparison.Ordinal);
        Assert.True(supervisor >= 0 && ride > supervisor, "Reverse supervision must run before TickRide finalization.");
        string sup = Section("private static void TickReverseSupervision", "private static void RevokeReverse");
        // Not gated on PollingActive; revokes on lost handle/host/ownership.
        Assert.DoesNotContain("if (!SpikeState.PollingActive)", sup);
        Assert.Contains("RevokeReverse(", sup);
    }

    [Fact]
    public void AllStaleReverseReaders_UseTheBoundedPhaseInsteadOfTheTimestamp()
    {
        // HasActuatorOrder
        string order = Section("internal static bool HasActuatorOrder", "// -------------------------------------------------------- deferred spawn");
        Assert.Contains("SpikeState.Reverse.Active", order);
        Assert.DoesNotContain("NavReDispatchAt > 0f", order);

        // Idle guard
        string idle = Section("private static void TickIdleGuard()", "// Final native inputs are selected");
        Assert.Contains("SpikeState.Reverse.Active", idle);
        Assert.DoesNotContain("NavReDispatchAt > 0f", idle);

        // Navigation callback
        string callback = Section("internal static void OnNavigationResult", "// ---------------------------------------------------------- nav polling");
        Assert.Contains("SpikeState.Reverse.Active", callback);
        Assert.DoesNotContain("NavReDispatchAt > 0f", callback);

        // Progress eligibility
        string observe = Section("private static void ObserveNavigationProgress()", "internal static void ObserveArtificialPlacement");
        Assert.Contains("SpikeState.Reverse.Active", observe);
        Assert.DoesNotContain("NavReDispatchAt <= 0f", observe);

        // Cruise speed consumer
        string cruise = DriverSafetySourceContractTests.ReadSource("CruiseSpeedPatch.cs");
        Assert.Contains("SpikeState.Reverse.Active", cruise);
        Assert.DoesNotContain("SpikeState.NavReDispatchAt > 0f", cruise);

        // RoadKeeper road-keeping skip and the app's "driving" status flags
        string road = DriverSafetySourceContractTests.ReadSource("RoadKeeper.cs");
        Assert.Contains("SpikeState.Reverse.Active", road);
        Assert.DoesNotContain("NavReDispatchAt > 0f", road);
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        Assert.DoesNotContain("SpikeState.NavReDispatchAt > 0f", commands);
    }

    [Fact]
    public void ActuatorConsumer_IndependentlyRefusesExpiredOrStaleReverse()
    {
        string patch = DriverSafetySourceContractTests.ReadSource("NativeActuatorPatch.cs");
        Assert.Contains("SpikeState.Reverse.IsCurrentFor(", patch);
        Assert.Contains("SpikeState.OperationId", patch);
        Assert.Contains("SpikeState.NavOrder", patch);
        Assert.DoesNotContain("reverseWindow: SpikeState.NavReDispatchAt > 0f", patch);
    }

    [Fact]
    public void OperationIdentity_IsSeparateFromTheChangingNavOrder()
    {
        string state = DriverSafetySourceContractTests.ReadSource("SpikeState.cs");
        Assert.Contains("internal static long OperationId;", state);
        Assert.Contains("NextOperationId()", state);
        Assert.Contains("readonly ReverseManoeuvrePolicy Reverse", state);
        string begin = Section("internal static void BeginNavigationOperation()", "private static bool StopIfNavigationExpired");
        Assert.Contains("SpikeState.NextOperationId()", begin);
    }

    [Fact]
    public void ArrivalVerdict_RequiresAReadableConfirmedHalt()
    {
        string ride = Section("internal static void TickRide()", "// ---------------------------------------------------------- settle check");
        Assert.Contains("NavigationArrivalPolicy.IsConfirmedArrival", ride);
        string patrol = Section("private static void TickPatrolSupervision", "private static void TickReDispatch");
        // Review D1: the patrol arrival requires a readable, finite speed, a confirmed stop and no
        // managed/native reverse — a moving or unreadable taxi near the target never completes.
        Assert.Contains("SupervisedArrivalPolicy.IsConfirmedArrival", patrol);
        Assert.Contains("NavigationArrivalPolicy.IsWithinArrival", patrol);
        Assert.Contains("TryReadReversing", patrol);
        Assert.Contains("operationProgress: _driveStartConfirmed", patrol);
        Assert.DoesNotContain("if (!reversing && (nearTarget || standingNear))", patrol);
        Assert.DoesNotContain("speedReadable && !reversing", patrol);
        string callback = Section("internal static void OnNavigationResult", "// ---------------------------------------------------------- nav polling");
        Assert.Contains("NavigationCallbackOutcomePolicy.Decide(", callback);
        Assert.Contains("SpikeState.NavCallbackRecoveryPending = true;", callback);
        string polling = Section("private static void TickNavigationPolling()", "private static void TickLotDump");
        Assert.Contains("if (SpikeState.NavCallbackRecoveryPending)", polling);
        Assert.Contains("RecoverStuck(agent, veh, position, distance, elapsed, autoDriving);", polling);

        string drive = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string startDrive = DriverSafetySourceContractTests.Section(drive, "private static bool StartRideDrive", "internal static bool Stop()");
        Assert.Contains("RideSatisfactionPolicy.IsAlreadySatisfied", startDrive);
    }

    [Fact]
    public void Pause_ShiftsTheReverseCapToo()
    {
        string pause = Section("private static void ShiftPauseDeadlines()", "private static void UpdateDriveStartProof");
        Assert.Contains("SpikeState.Reverse.ShiftPause(dt)", pause);
    }

    [Fact]
    public void StopAndTeardown_RevokeTheManagedReverseIntent()
    {
        string reset = Section("internal static void ResetDrivingState()", "/// <summary>Free space");
        Assert.Contains("SpikeState.Reverse.End()", reset);
        string giveup = Section("private static void GiveUpNavigation", "private static void TickHeartbeat");
        Assert.Contains("SpikeState.Reverse.End()", giveup);
    }

    // ---------------------------------------------------------------- review D1–D16 wiring

    [Fact]
    public void CallbackIsQueuedNotExecuted_AndTheDrainRunsBeforeRideFinalization()
    {
        // D12/D16: the native callback only QUEUES; every native action happens in the drain, and
        // the drain runs before TickRide so no recursion (stop -> callback -> stop) is possible.
        string callback = Section("internal static void OnNavigationResult", "private static void TickNavigationCallback");
        Assert.Contains("SpikeState.NavCallbackQueuedResult = text;", callback);
        Assert.Contains("SpikeState.NavCallbackQueuedOrder = order;", callback);
        Assert.Contains("SpikeState.NavCallbackQueuedOperation = SpikeState.OperationId;", callback);
        Assert.DoesNotContain("SpikeCommands.CompletePickup(", callback);
        Assert.DoesNotContain("StopReversing()", callback);
        Assert.DoesNotContain("StopNavigating()", callback);

        string drain = Section("private static void TickNavigationCallback", "private static void TickLotDump");
        Assert.Contains("NavigationCallbackOutcomePolicy.Decide(", drain);
        Assert.Contains("if (order != SpikeState.NavOrder || operation != SpikeState.OperationId ||", drain);
        Assert.Contains("stale queued callback", drain);

        string update = Section("internal static void Update()", "private static void ShiftPauseDeadlines");
        int drainAt = update.IndexOf("TickNavigationCallback();", StringComparison.Ordinal);
        int ride = update.IndexOf("TickRide();", StringComparison.Ordinal);
        Assert.True(drainAt >= 0 && ride > drainAt, "The queued callback must be drained before ride finalization.");
    }

    [Fact]
    public void UnexpectedStopped_IsAnExplicitFailure_NotARestart()
    {
        string drain = Section("private static void TickNavigationCallback", "private static void TickLotDump");
        Assert.Contains("NavigationOutcomeAction.ExplicitFailure", drain);
        Assert.Contains("SpikeState.Operation.Terminate(\"navigation stopped unexpectedly\", failed: true);", drain);
        Assert.Contains("NavigationOutcomeAction.BoundedRecovery", drain);
        Assert.Contains("SpikeState.Operation.EnterRecovery();", drain);
        Assert.Contains("NavigationOutcomeAction.ConfirmArrival", drain);
        // The stale mailbox is order/operation aware (D12), never a bare bool.
        Assert.Contains("SpikeState.NavCallbackRecoveryOrder", drain);
        Assert.Contains("SpikeState.NavCallbackRecoveryOperation", drain);
    }

    [Fact]
    public void RevokeReverse_RevokesManagedIntentFirstAndRunsIndependentCleanupSteps()
    {
        string revoke = Section("private static void RevokeReverse", "private static System.Numerics.Vector3 ToNumerics");
        // D3: a local PASSENGER never suppresses the NPC reverse cleanup — only a real player
        // driver does, and an unreadable driver reference fails closed.
        Assert.Contains("ReverseSupervisionPolicy.NativeCleanupPermitted", revoke);
        Assert.DoesNotContain("SafePlayerInVehicle", revoke);
        Assert.Contains("SpikeState.Reverse.End()", revoke);
        Assert.Contains("SpikeState.NavReDispatchAt = 0f;", revoke);
        Assert.Contains("SpikeState.NextNavOrder();", revoke);
        // D3/§7.4: independent cleanup steps — a throwing first step must not skip the rest.
        Assert.Contains("CleanupGuard.Run(\"reverse revoke (StopReversing)\"", revoke);
        Assert.Contains("CleanupGuard.Run(\"reverse revoke (StopNavigating)\"", revoke);
        Assert.Contains("CleanupGuard.Run(\"reverse revoke (park)\"", revoke);
    }

    [Fact]
    public void Supervisor_ReChecksRearContinuouslyAndFailsClosedOnLifecycleLoss()
    {
        string sup = Section("private static void TickReverseSupervision", "private static void RevokeReverse");
        // D2: bounded continuous rear clearance during the active reverse.
        Assert.Contains("ReverseSupervisionPolicy.ShouldCheckRear(_reverseRearCheckAt, now)", sup);
        Assert.Contains("if (!RearIsClear(veh))", sup);
        // D5/D14: lifecycle/scene, authority, ownership, actuator health and the driver are
        // re-checked every tick and any failure revokes the managed reverse.
        Assert.Contains("!Mod.Lifecycle.AcceptingWork || !MainSceneLoaded()", sup);
        Assert.Contains("!NativeActuatorPatch.Healthy", sup);
        Assert.Contains("SpikeCommands.HasAuthority()", sup);
        Assert.Contains("SpikeCommands.IsOwnedTaxi(veh)", sup);
        Assert.Contains("PlayerDriver(veh) || !SpikeCommands.DriverAtWheel(veh!)", sup);
        Assert.Contains("RevokeReverse($\"supervisor getter/update failure", sup);
    }

    [Fact]
    public void UpdateEarlyExitsAndOuterCatch_ClearTheManagedReverse()
    {
        string update = Section("internal static void Update()", "private static void ShiftPauseDeadlines");
        Assert.Contains("RevokeReverse(\"update: gameplay scene not loaded\")", update);
        Assert.Contains("RevokeReverse(\"update: foreign/uncertain tracked vehicle\")", update);
        // The !AcceptingWork early exit and the outer catch clear it without native input.
        Assert.Contains("SpikeState.Reverse.End();", update);
        Assert.Contains("SpikeState.NavReDispatchAt = 0f;", update);
    }

    [Fact]
    public void AutonomousPaths_RefuseNewInputUnderAPlayerDriver()
    {
        string redispatch = Section("internal static void ReDispatch", "private static bool TryTeleportRecovery");
        Assert.Contains("PlayerDriver(pd)", redispatch);
        string rescue = Section("private static bool TryFreeSpotRescue", "private static void LogAgentProbe");
        Assert.Contains("PlayerDriver(veh)", rescue);
        string stuck = Section("private static void RecoverStuck", "private static bool RearIsClear");
        Assert.Contains("PlayerDriver(veh)", stuck);
        string startup = Section("private static bool TickStartupRecovery", "private static void TickPatrolSupervision");
        Assert.Contains("PlayerDriver(veh)", startup);
    }

    [Fact]
    public void EnsureDriverSeat_RefusesToReseatWhileAPlayerDrives()
    {
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string retention = DriverSafetySourceContractTests.Section(commands, "internal static bool EnsureDriverSeat", "internal static VehicleSeat? FindSeat");
        // D3: never mutate a player-occupied seat — checked BEFORE any re-seat, failing closed.
        Assert.Contains("SafeDriverPlayerPresent(veh)", retention);
        int guard = retention.IndexOf("SafeDriverPlayerPresent(veh)", StringComparison.Ordinal);
        int seat = retention.IndexOf("FindSeat", StringComparison.Ordinal);
        Assert.True(guard >= 0, "EnsureDriverSeat must guard against a player driver.");
        Assert.True(seat < 0 || guard < seat, "The player-driver guard must precede the re-seat work.");
    }

    [Fact]
    public void TickRide_FinalizesOnlyOnTheExplicitOperationTerminal()
    {
        string ride = Section("internal static void TickRide()", "// ---------------------------------------------------------- settle check");
        // D13: never finalize on a native PollingActive flip.
        Assert.Contains("!SpikeState.RideArrived && SpikeState.Operation.Terminal", ride);
        Assert.DoesNotContain("!SpikeState.RideArrived && !SpikeState.PollingActive", ride);
        // D1/§8.2: the completed-destination identity is stored only on a genuine arrival.
        Assert.Contains("SpikeState.CompletedDestinationName = SpikeState.RideDestinationName;", ride);
        Assert.Contains("SpikeState.CompletedGoal = SpikeState.RideGoal;", ride);
    }
}

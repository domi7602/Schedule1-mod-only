using System;
using Xunit;

namespace TaxiDriver.Tests;

// Unity-free policies are exercised separately; these are runtime wiring guards only.
public class RecoveryLoopSourceContractTests
{
    private static string Section(string start, string end) => DriverSafetySourceContractTests.Section(
        DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs"), start, end);

    [Fact]
    public void StuckRecovery_CommitsOperationBudgetBeforeReverseOrRescue()
    {
        string recovery = Section("private static void RecoverStuck", "private static bool RearIsClear");
        int budget = recovery.IndexOf("TryBeginRecovery(", StringComparison.Ordinal);
        int reverse = recovery.IndexOf("TryStartReverse(", StringComparison.Ordinal);
        Assert.True(budget >= 0 && budget < reverse, "Recovery must spend the operation budget before any reverse/rescue branch can return.");
    }

    [Fact]
    public void Deadline_IsCheckedBeforeDeferredReverseAndStartupRecovery()
    {
        string polling = Section("private static void TickNavigationPolling()", "// ---- Stage 4: the GAME's own driver leads");
        int deadline = polling.IndexOf("StopIfNavigationExpired(", StringComparison.Ordinal);
        int deferred = polling.IndexOf("if (SpikeState.NavReDispatchAt > 0f)", StringComparison.Ordinal);
        Assert.True(deadline >= 0 && deadline < deferred, "Operation deadline must be checked even while reverse/deferred dispatch is pending.");
    }

    [Fact]
    public void StartupAndPatrolRecovery_UseTheSameOperationBudget()
    {
        string startup = Section("private static bool TickStartupRecovery", "private static void TickPatrolSupervision");
        Assert.Contains("if (!TryBeginRecovery(agent, veh, maneuverPlanned: true)) return true;", startup);
        string patrol = Section("private static void TickPatrolSupervision", "private static void TickReDispatch");
        Assert.Contains("if (!TryBeginRecovery(veh.Agent, veh, maneuverPlanned: true)) return;", patrol);
        Assert.Equal(2, patrol.Split("TryBeginRecovery(veh.Agent, veh,").Length - 1);
    }

    [Fact]
    public void NewOrdersAndTeardown_ArmAndResetTheDeadlineWhileRedispatchKeepsIt()
    {
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        Assert.Equal(2, commands.Split("SpikeRunner.BeginNavigationOperation();").Length - 1);
        string begin = Section("internal static void BeginNavigationOperation()", "private static bool StopIfNavigationExpired");
        Assert.Contains("NavigationRecovery.Begin(Time.unscaledTime, NavigationTimeoutSeconds)", begin);
        Assert.Contains("_freeSpotRescues = 0;", begin);
        string state = DriverSafetySourceContractTests.ReadSource("SpikeState.cs");
        string reset = DriverSafetySourceContractTests.Section(state, "internal static void ResetNavigation()", "internal static void ResetAutoRun()");
        Assert.Contains("NavigationRecovery.Reset()", reset);
        string pause = Section("private static void ShiftPauseDeadlines()", "private static void UpdateDriveStartProof");
        Assert.Contains("NavigationRecovery.ShiftPause(dt)", pause);
        string teleport = Section("private static bool TryTeleportRecovery", "private static void GiveUpNavigation");
        Assert.Contains("NavigationRecovery.TryLastRescue(Time.unscaledTime)", teleport);
    }

    [Fact]
    public void Redispatch_DoesNotRestartTheOperationBudget()
    {
        string redispatch = Section("internal static void ReDispatch", "private static bool TryTeleportRecovery");
        Assert.Contains("StopIfNavigationExpired(", redispatch);
        Assert.DoesNotContain("NavigationRecovery.Begin(", redispatch);
    }

    [Fact]
    public void TerminalRelocation_IsPolicyGuardedAndNeverSkippedBecauseOfGraphState()
    {
        string teleport = Section("private static bool TryTeleportRecovery", "private static void GiveUpNavigation");
        Assert.Contains("TerminalRescuePolicy.Decide(onGraph, lastRescueAvailable)", teleport);
        Assert.Contains("RoadKeeper.TryRescueStartup(veh, agent, \"teleport recovery\")", teleport);
        Assert.DoesNotContain("no teleport needed, giving up", teleport);
    }
}

using System;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class PatrolStartPolicyTests
{
    [Theory]
    [InlineData(2.5f, true)]
    [InlineData(5f, true)]
    [InlineData(7.9f, true)]
    [InlineData(8f, true)]
    [InlineData(2.5f, false)]
    [InlineData(5f, false)]
    [InlineData(7.9f, false)]
    [InlineData(8f, false)]
    public void MatchingUnconfirmedStart_OwnsGraceRegardlessOfObstacle(float elapsed, bool obstacle)
    {
        bool waiting = PatrolStartPolicy.ShouldWait(true, 7, 7, false, elapsed, 8f);
        Assert.Equal(elapsed < 8f, waiting);
        // Even when the obstacle's shorter watchdog is due, the grace gate owns it.
        float ordinaryWindow = obstacle ? 2.5f : 5f;
        bool ordinaryRecoveryAllowed = !waiting && elapsed >= ordinaryWindow;
        Assert.Equal(elapsed >= 8f, ordinaryRecoveryAllowed);
    }

    [Theory]
    [InlineData(2.5f)]
    [InlineData(5f)]
    [InlineData(7.9f)]
    [InlineData(8f)]
    public void ConfirmedStart_TransfersImmediatelyToWatchdog(float elapsed)
    {
        Assert.False(PatrolStartPolicy.ShouldWait(true, 7, 7, true, elapsed, 8f));
    }

    [Fact]
    public void SupersededOrInactiveSelfTest_CannotSuppressWatchdog()
    {
        Assert.False(PatrolStartPolicy.ShouldWait(true, 6, 7, false, 2.5f, 8f));
        Assert.False(PatrolStartPolicy.ShouldWait(false, 7, 7, false, 2.5f, 8f));
    }

    [Fact]
    public void Runtime_WaitReturnsBeforeOrdinaryProgressAndObstacleWatchdog()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string patrol = DriverSafetySourceContractTests.Section(runner,
            "private static void TickPatrolSupervision", "private static void TickReDispatch");
        int wait = patrol.IndexOf("PatrolStartPolicy.ShouldWait(", StringComparison.Ordinal);
        int watchdog = patrol.IndexOf("if (EvaluateProgress(", StringComparison.Ordinal);
        Assert.True(wait >= 0 && wait < watchdog, "Unconfirmed start must return before the ordinary stall watchdog.");
        Assert.Contains("return;", patrol.Substring(wait, watchdog - wait));
        Assert.True(patrol.IndexOf("if (nearTarget || standingNear)", StringComparison.Ordinal) < wait);
        string polling = DriverSafetySourceContractTests.Section(runner,
            "private static void TickNavigationPolling()", "// ---- Stage 4: the GAME's own driver leads");
        Assert.True(polling.IndexOf("StopIfNavigationExpired(", StringComparison.Ordinal) <
            polling.IndexOf("UpdateDriveStartProof(", StringComparison.Ordinal));
        Assert.Contains("UpdateDriveStartProof(veh, position, distance, now);", polling);
        Assert.Contains("else if (_driveStartConfirmed)", patrol);
        Assert.Contains("else if (now - _selfTestAt >= DriveSelfTestSeconds)", patrol);
    }
}

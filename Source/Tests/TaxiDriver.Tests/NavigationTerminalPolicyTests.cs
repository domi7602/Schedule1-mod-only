using System;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class NavigationTerminalPolicyTests
{
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
            "internal static void BeginNavigationOperation()", "private static bool StopIfNavigationExpired");
        Assert.Contains("NavigationTerminalPolicy.Begin(ref SpikeState.NavGaveUp, ref SpikeState.RideArrived,", begin);
        Assert.Contains("ref SpikeState.NavCallbackResult)", begin);
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string go = DriverSafetySourceContractTests.Section(commands,
            "internal static bool Go(", "internal static void Pois()");
        Assert.True(go.IndexOf("SpikeRunner.BeginNavigationOperation();", StringComparison.Ordinal) <
            go.IndexOf("agent.Navigate(target, settings,", StringComparison.Ordinal));
        string start = DriverSafetySourceContractTests.Section(commands,
            "private static bool StartRideDrive(", "internal static bool Stop()");
        Assert.Contains("SpikeRunner.BeginNavigationOperation();", start);
        Assert.Contains("bool dispatched = Go(0f, useSettings: true, absoluteTarget: destination, toPlayer: false);", start);
        Assert.Equal(2, commands.Split("SpikeRunner.BeginNavigationOperation();").Length - 1);
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

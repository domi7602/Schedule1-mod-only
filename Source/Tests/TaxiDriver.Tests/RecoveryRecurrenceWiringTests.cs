using Xunit;

namespace TaxiDriver.Tests;

public class RecoveryRecurrenceWiringTests
{
    private static string Source => DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");

    [Fact]
    public void LocalResets_RequireOperationPersistentForwardProgress()
    {
        Assert.Contains("RecoveryRecurrence.ObserveProgress(", Source);
        Assert.Contains("veh.Speed_Kmh >= DriveStartSustainKmh && veh.currentThrottle > 0f", Source);
        Assert.Equal(2, Source.Split("if (_localRecoveryProgress)").Length - 1);
        string reverse = DriverSafetySourceContractTests.Section(Source,
            "private static void TickReDispatch", "internal static void ReDispatch");
        Assert.DoesNotContain("_reverseAttempt = 0;", reverse);
        string placement = DriverSafetySourceContractTests.Section(Source,
            "internal static void ObserveArtificialPlacement", "internal static void BeginNavigationOperation");
        Assert.Contains("RecoveryRecurrence.ObserveProgress(", placement);
        Assert.Contains("false", placement);
    }

    [Fact]
    public void NewOperationAndTeardown_ResetRecurrenceButRedispatchDoesNot()
    {
        string begin = DriverSafetySourceContractTests.Section(Source,
            "internal static void BeginNavigationOperation", "private static bool StopIfNavigationExpired");
        Assert.Contains("RecoveryRecurrence.Begin(", begin);
        Assert.Contains("_localRecoveryProgress = false;", begin);
        string reset = DriverSafetySourceContractTests.Section(Source,
            "internal static void ResetDrivingState", "private const float ReverseClearanceMeters");
        Assert.Contains("RecoveryRecurrence.Begin(", reset);
        string redispatch = DriverSafetySourceContractTests.Section(Source,
            "internal static void ReDispatch", "private static bool TryTeleportRecovery");
        Assert.DoesNotContain("RecoveryRecurrence.Begin(", redispatch);
        Assert.DoesNotContain("_reverseAttempt = 0;", redispatch);
    }

    [Fact]
    public void NearbyRecurringStall_SpendsBudgetThenUsesCheckedRescueBeforeReverse()
    {
        string recovery = DriverSafetySourceContractTests.Section(Source,
            "private static bool TryBeginRecovery", "internal static void ArmStartupRecovery");
        Assert.Contains("RecoveryRecurrence.RegisterStall(", recovery);
        Assert.Contains("TryFreeSpotRescue(veh, agent, \"recurring stall\")", recovery);
        Assert.True(recovery.IndexOf("NavigationRecovery.TrySpend(") < recovery.IndexOf("RecoveryRecurrence.RegisterStall("));
        Assert.Contains("return false;", recovery);
        int branch = recovery.IndexOf("TaxiAI.StopPatrol(\"recurring stall\")");
        int seat = recovery.IndexOf("SpikeCommands.EnsureDriverSeat(veh);", branch);
        int abort = recovery.IndexOf("if (!SpikeState.PollingActive) return false;", branch);
        int rescue = recovery.IndexOf("TryFreeSpotRescue(", branch);
        Assert.True(branch >= 0 && seat > branch && abort > seat && rescue > abort,
            "Patrol release may eject the driver; re-seat and honor a safety abort before relocating.");
    }
}

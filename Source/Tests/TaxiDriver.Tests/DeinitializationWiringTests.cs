using Xunit;

namespace TaxiDriver.Tests;

public class DeinitializationWiringTests
{
    [Fact]
    public void ModUsesOrderedPolicyAndRemovesEveryHookOnlyInFinalization()
    {
        string source = DriverSafetySourceContractTests.ReadSource("Mod.cs");
        string teardown = DriverSafetySourceContractTests.Section(source, "private static void Teardown(string reason)", "// ---------------------------------------------------------------------------");
        Assert.Contains("Lifecycle.Shutdown(", teardown);
        string[] phases = { "SpikeRunner.Shutdown", "FareMeter.Stop", "TaxiAI.StopPatrol", "StopVehicleForShutdown",
            "RideLocks.UnlockTrunk", "SpikeState.ResetRide", "ResetDrivingState", "NativeActuatorPatch.Remove", "CruiseSpeedPatch.Remove", "TrafficLightPatch.Remove", "RideLocks.Remove", "SpikeTrace.Remove" };
        int previous = -1;
        foreach (string phase in phases)
        {
            int at = teardown.IndexOf(phase, StringComparison.Ordinal);
            Assert.True(at > previous, $"Missing/out-of-order phase: {phase}");
            previous = at;
        }
        Assert.DoesNotContain("SpikeCommands.Cleanup(", teardown);
        Assert.DoesNotContain("SpikeState.Reset()", teardown);
        Assert.Contains("if (!Lifecycle.Initialize())", source);
        Assert.Contains("initialization refused", source);
        Assert.Contains("GameLifecycle.OnPreLoad -= OnPreLoad;", source);
    }

    [Fact]
    public void RunnerQuiescesAllDeferredActionsAndNativeCallbacksAreGenerationBound()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.Contains("Mod.Lifecycle.Guard<VehicleAgent.ENavigationResult>", source);
        Assert.Contains("if (!Mod.Lifecycle.AcceptingWork)", source);
        string shutdown = source.Substring(source.IndexOf("internal static void Shutdown()", StringComparison.Ordinal));
        foreach (string required in new[] { "NextNavOrder", "NavToPlayer = false", "NavReDispatchAt = 0f",
            "CancelPendingSpawn", "ResetAutoRun", "StandSpawnPosition = null", "SeatVerifyNpc = null", "SeatVerifyVeh = null",
            "SettleCheckAt = 0f", "SettleCheckVehicle = null", "_lotDumpPending = false" })
            Assert.Contains(required, shutdown);
        Assert.DoesNotContain("VehicleOwnership", shutdown);
        Assert.DoesNotContain("DriverNpc = null", shutdown);
        string beginGo = DriverSafetySourceContractTests.Section(source, "internal static bool TryBeginGo(string caller)", "// ------------------------------------------------------------- auto run");
        Assert.Contains("if (!Mod.Lifecycle.AcceptingWork)", beginGo);
    }

    [Fact]
    public void ShutdownVehicleStopIsAuthorityAndExactOwnershipGatedWithoutDespawnOrSeatMutation()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string stop = DriverSafetySourceContractTests.Section(source, "internal static void StopVehicleForShutdown(string reason)", "internal static bool Cleanup(");
        Assert.Contains("HasAuthority()", stop);
        Assert.Contains("IsOwnedTaxi(veh)", stop);
        Assert.Contains("StopReversing", stop);
        Assert.Contains("StopNavigating", stop);
        Assert.DoesNotContain("DestroyVehicle", stop);
        Assert.DoesNotContain("ExitVehicle", stop);
        Assert.DoesNotContain("Occupant =", stop);
        Assert.DoesNotContain("VehicleOwnership.Reset", stop);
    }

    [Fact]
    public void FareStopUsesConsumeBeforeReportingPolicyWithoutChargingOrResettingLedger()
    {
        string source = DriverSafetySourceContractTests.ReadSource("FareMeter.cs");
        string stop = DriverSafetySourceContractTests.Section(source, "internal static void Stop(string reason)", "internal static void PrintStatus()");
        Assert.Contains("MeterSettlement.Stop(ref _active", stop);
        Assert.DoesNotContain("Charge(", stop);
        Assert.DoesNotContain(".Reset()", stop);
        Assert.Contains("if (!Mod.Lifecycle.AcceptingWork)", source);
    }

    [Fact]
    public void AppCallbacksUseGenerationGuardWithoutReplacingListenerLeaseCleanup()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        Assert.Contains("Mod.Lifecycle.Guard<bool>(_ => Update())", source);
        Assert.Contains("Mod.Lifecycle.Guard<bool>(_ => callback())", source);
        Assert.Contains("Mod.Lifecycle.Guard(callback)", source);
        Assert.Contains("lease.Release(); EventHelper.RemoveListener", source);
        Assert.Contains("new InputField.OnChangeEvent()", source);
    }
}

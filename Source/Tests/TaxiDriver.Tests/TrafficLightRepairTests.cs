using Xunit;

namespace TaxiDriver.Tests;

public class TrafficLightRepairTests
{
    [Theory]
    [InlineData(true, true, false, false, true)]
    [InlineData(false, true, false, false, false)]
    [InlineData(true, false, false, false, false)]
    [InlineData(true, true, true, false, false)]
    [InlineData(true, true, false, true, false)]
    public void BypassRequiresSupportedForwardPhysics(bool eligible, bool physical,
        bool reversing, bool paused, bool expected)
        => Assert.Equal(expected, TrafficLightPolicy.CanBypass(eligible, physical, reversing, paused));

    [Theory]
    [InlineData(true, true, true, false, false, true)]
    [InlineData(false, true, true, false, false, false)]
    [InlineData(true, false, true, false, false, false)]
    [InlineData(true, true, false, false, false, false)]
    [InlineData(true, true, true, true, false, false)]
    [InlineData(true, true, true, false, true, false)]
    public void OnlyReadableExclusiveTrafficBlockCanHoldRecovery(bool readable, bool stopped,
        bool traffic, bool generic, bool filteredHit, bool expected)
        => Assert.Equal(expected, TrafficLightPolicy.IsTrafficWait(readable, stopped, traffic, generic, filteredHit));

    [Fact]
    public void NativeOutParameterConsumerIsNeverDetoured()
    {
        string mod = DriverSafetySourceContractTests.ReadSource("Mod.cs");
        Assert.Contains(mod.Split('\n'), line => line.Trim() == "TrafficLightPatch.Apply();");
        string patch = DriverSafetySourceContractTests.ReadSource("TrafficLightPatch.cs");
        Assert.Contains("nameof(VehicleAgent.UpdateSweep)", patch);
        Assert.Contains("nameof(VehicleAgent.UpdateSpeedReduction)", patch);
        Assert.DoesNotContain("DeclaredMethod(typeof(VehicleAgent), nameof(VehicleAgent.GetWheelSweepCollision))", patch);
        Assert.Contains("TrafficLightPolicy.CanBypass", patch);
        Assert.Contains("IsPhysicallySimulated", patch);
        Assert.Contains("isKinematic", patch);
        Assert.Contains("GetParameters().Length != 0", patch);
        Assert.DoesNotContain("PatchConsumer(nameof(VehicleAgent.GetWheelSweepCollision))", patch);
        string install = DriverSafetySourceContractTests.Section(patch,
            "internal static void Apply(", "private static bool Eligible(");
        Assert.DoesNotContain("GetWheelSweepCollision", install);
        Assert.DoesNotContain("AccessTools.Method", patch);
        Assert.DoesNotContain("\"GetWheelSweepCollision\"", patch);
        Assert.DoesNotContain("nameof(VehicleAgent.GetWheelSweepCollision)", patch);
        Assert.Equal(2, System.Text.RegularExpressions.Regex.Matches(install,
            @"PatchConsumer\(nameof\(VehicleAgent\.\w+\)\)").Count);
    }

    [Fact]
    public void ExplicitTrafficWaitIsObservedBeforeRecoveryBudgetAndShiftsOnlyItsTime()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.Contains("TrafficLightPatch.HoldRecovery(veh)", runner);
        string recovery = DriverSafetySourceContractTests.Section(runner,
            "private static bool TryBeginRecovery(", "internal static void ArmStartupRecovery(");
        int hold = recovery.IndexOf("if (_trafficWaiting) return false;", System.StringComparison.Ordinal);
        int spend = recovery.IndexOf(".TrySpend(", System.StringComparison.Ordinal);
        Assert.True(hold >= 0 && spend > hold);
        Assert.Contains("SpikeState.NavigationRecovery.ShiftPause(dt);", runner);
        Assert.Contains("_trafficWaiting = false;", runner);
    }

    [Fact]
    public void WaitAllowanceIsFiniteAcrossRepeatedStopsAndRedispatch()
    {
        var budget = new TrafficWaitBudget();
        Assert.Equal(20f, budget.Observe(true, 20f, false));
        Assert.Equal(0f, budget.Observe(false, 10f, false));
        Assert.Equal(10f, budget.Observe(true, 25f, false));
        Assert.Equal(TrafficWaitBudget.MaxSeconds, budget.HeldSeconds);
        Assert.Equal(0f, budget.Observe(true, 1f, false));
        budget.Reset();
        Assert.Equal(1f, budget.Observe(true, 1f, false));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(-1f)]
    [InlineData(0f)]
    public void InvalidTimeNeverBuysExemption(float seconds)
    {
        var budget = new TrafficWaitBudget();
        Assert.Equal(0f, budget.Observe(true, seconds, false));
        Assert.Equal(0f, budget.HeldSeconds);
    }

    [Fact]
    public void ArrivalAlwaysRunsEvenWithAConfirmedTrafficHit()
    {
        var budget = new TrafficWaitBudget();
        Assert.Equal(0f, budget.Observe(true, 1f, true));
        Assert.Equal(0f, budget.HeldSeconds);
        Assert.Equal(1f, budget.Observe(true, 1f, false));
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.Contains("TrafficWait.Observe(trafficBlocked, Time.unscaledDeltaTime, withinArrival)", runner);
        Assert.Contains("NavigationArrivalPolicy.IsWithinArrival", runner);
    }

    [Fact]
    public void HoldShiftsProgressWindowWithoutManufacturingANewSpeedOrigin()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string observation = DriverSafetySourceContractTests.Section(runner,
            "private static void ObserveNavigationProgress()", "internal static void ObserveArtificialPlacement(");
        Assert.Contains("SpikeState.ProgressWindowStart += dt;", observation);
        Assert.DoesNotContain("SpikeState.ProgressWindowStart = Time.unscaledTime", observation);
        string recovery = DriverSafetySourceContractTests.Section(runner,
            "private static bool TryBeginRecovery(", "internal static void ArmStartupRecovery(");
        Assert.DoesNotContain("TrafficWait.Reset", recovery);
        Assert.Equal(2, System.Text.RegularExpressions.Regex.Matches(runner, @"\bTrafficWait\.Reset\(").Count);
        string redispatch = DriverSafetySourceContractTests.Section(runner,
            "internal static void ReDispatch(", "private static bool TryTeleportRecovery(");
        Assert.DoesNotContain("TrafficWait.Reset", redispatch);
        Assert.DoesNotContain("BeginNavigationOperation(", redispatch);
    }

    [Fact]
    public void OptionalProbeFailureDoesNotDisableFunctionalBypass()
    {
        string patch = DriverSafetySourceContractTests.ReadSource("TrafficLightPatch.cs");
        string hold = DriverSafetySourceContractTests.Section(patch,
            "internal static bool HoldRecovery(", "private static bool ProbeTrafficWait(");
        Assert.DoesNotContain("Fail(ex)", hold);
        Assert.Contains("_probeDisabled = true;", hold);
        string probe = DriverSafetySourceContractTests.Section(patch,
            "private static bool ProbeTrafficWait(", "private static bool ReadSweep(");
        Assert.DoesNotContain("SweepPrefix(agent", probe);
        Assert.Contains("BeginScope(state, original);", probe);
    }
}

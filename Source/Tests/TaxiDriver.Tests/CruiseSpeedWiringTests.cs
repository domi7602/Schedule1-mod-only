using System;
using System.IO;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Source wiring contracts, not a substitute for IL2CPP gameplay validation.</summary>
public class CruiseSpeedWiringTests
{
    [Fact]
    public void Initialization_InstallsCruiseHookAndTeardownRemovesIt()
    {
        string mod = DriverSafetySourceContractTests.ReadSource("Mod.cs");
        Assert.Contains("CruiseSpeedPatch.Apply();", mod);
        string teardown = DriverSafetySourceContractTests.Section(mod,
            "private static void Teardown(string reason)", "// ---------------------------------------------------------------------------");
        Assert.Contains("CruiseSpeedPatch.Remove();", teardown);
    }

    [Fact]
    public void CruiseHook_AdjustsOnlyFreshNativeBaseTargetThroughPostfix()
    {
        string modPath = DriverSafetySourceContractTests.ReadSource("Mod.cs");
        // The absent hook fails an assertion, never a missing-source compiler error.
        Assert.Contains("CruiseSpeedPatch.Apply();", modPath);
        string source = DriverSafetySourceContractTests.ReadSource("CruiseSpeedPatch.cs");
        Assert.Contains("PatchGuard.TryPatch(", source);
        Assert.Contains("typeof(VehicleAgent)", source);
        Assert.Contains("nameof(VehicleAgent.UpdateSpeed)", source);
        Assert.Contains("postfix: new HarmonyMethod", source);
        Assert.DoesNotContain("prefix: new HarmonyMethod", source);
        Assert.Contains("SpikeCommands.HasAuthority()", source);
        Assert.Contains("SpikeCommands.IsOwnedTaxi(veh)", source);
        Assert.Contains("SpikeState.PollingActive || TaxiAI.Active", source);
        Assert.Contains("__instance.AutoDriving", source);
        Assert.Contains("veh.DriverPlayer != null", source);
        Assert.Contains("__instance.IsReversing || SpikeState.Reverse.Active", source);
        Assert.Contains("flags.OverrideSpeed", source);
        Assert.Contains("CruiseSpeedPolicy.BaseTargetKmh(", source);
        Assert.Contains("__instance.targetSpeed = target;", source);
        Assert.Contains("UnpatchSelf()", source);
        Assert.Contains("[cruise]", source);
    }

    [Fact]
    public void CruiseHook_NeverWritesSharedFlagsOrDrivingActuators()
    {
        Assert.Contains("CruiseSpeedPatch.Apply();", DriverSafetySourceContractTests.ReadSource("Mod.cs"));
        string source = DriverSafetySourceContractTests.ReadSource("CruiseSpeedPatch.cs");
        string[] forbidden = { "SpeedLimitMultiplier =", "OverrideSpeed =", "OverriddenSpeed =",
            "OverriddenReverseSpeed =", "UnmarkedSpeed =", "ReverseSpeed =", "Flags =",
            "TurnBasedSpeedReduction =", "AutoBrakeAtDestination =", "ObstacleMode =",
            "throttleOverride =", "velocity =", "StartReverse(", "StopReversing(", "UpdateSpeedReduction)" };
        foreach (string token in forbidden)
            Assert.DoesNotContain(token, source);
    }
}

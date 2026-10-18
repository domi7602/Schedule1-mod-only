using Xunit;

namespace TaxiDriver.Tests;

// Source contracts only: native execution and driving still require gameplay verification.
public class TrafficLightWiringTests
{
    [Fact]
    public void ProvenConsumerHasScopedOwnedHookAndCleanup()
    {
        string mod = DriverSafetySourceContractTests.ReadSource("Mod.cs");
        string[] modLines = mod.Split('\n');
        Assert.DoesNotContain(modLines, line =>
            line.TrimStart().StartsWith("TrafficLightPatch.Apply();"));
        Assert.Contains("TrafficLightPatch.Remove();", mod);
        string source = DriverSafetySourceContractTests.ReadSource("TrafficLightPatch.cs");
        Assert.Contains("nameof(VehicleAgent.GetWheelSweepCollision)", source);
        Assert.Contains("prefix: new HarmonyMethod", source);
        Assert.Contains("postfix: new HarmonyMethod", source);
        Assert.Contains("finalizer: new HarmonyMethod", source);
        Assert.Contains("info.Finalizers.Any", source);
        Assert.Contains("SpikeCommands.HasAuthority()", source);
        Assert.Contains("SpikeCommands.IsOwnedTaxi(veh)", source);
        Assert.Contains("veh.Agent.Pointer == __instance.Pointer", source);
        Assert.Contains("SpikeRunner.HasActuatorOrder", source);
        Assert.Contains("__instance.AutoDriving", source);
        Assert.Contains("veh.DriverPlayer != null", source);
        Assert.Contains("TrafficLightPolicy.ShouldIgnore", source);
        Assert.Contains("PrivateFlagsScope<DriveFlags>.Begin", source);
        Assert.Contains("original.Pointer == copy.Pointer", source);
        Assert.Contains("return __exception;", source);
        Assert.Contains("UnpatchSelf()", source);
        Assert.Contains("[traffic-lights]", source);
        string[] copied = { "OverrideSpeed", "OverriddenSpeed", "OverriddenReverseSpeed",
            "SpeedLimitMultiplier", "UseRoads", "StuckDetection", "ObstacleMode",
            "AutoBrakeAtDestination", "TurnBasedSpeedReduction" };
        foreach (string field in copied)
            Assert.Contains($"{field} = original.{field}", source);
        Assert.Contains("IgnoreTrafficLights = true", source);
        Assert.DoesNotContain("original.IgnoreTrafficLights =", source);
        Assert.DoesNotContain("__instance.Flags.IgnoreTrafficLights =", source);
        Assert.DoesNotContain("targetSpeed =", source);
        Assert.DoesNotContain("ResetFlags(", source);
        Assert.DoesNotContain("throttleOverride =", source);
        Assert.DoesNotContain("EObstacleMode.IgnoreAll", source);
        Assert.DoesNotContain("StartReverse(", source);
        Assert.DoesNotContain("StopReversing(", source);
    }
}

using Xunit;

namespace TaxiDriver.Tests;

// Supplementary guards: physics cannot execute in this Unity-free test host.
public class RoadSnapWiringTests
{
    private static string Source => DriverSafetySourceContractTests.ReadSource("RoadKeeper.cs");

    [Fact]
    public void CorridorCallsRuntimeUsedProjectionPolicy()
    {
        Assert.Contains("RoadProjectionPolicy.TryProject(", Source);
        Assert.DoesNotContain("Vector3 p = a + ab * t;", Source);
        Assert.Contains("RoadSnapSafetyPolicy.NeedsHardCorrection(", Source);
    }

    [Fact]
    public void SnapRequiresSupportedMeasuredFreePlacementBeforeCountingOrMoving()
    {
        var hard = DriverSafetySourceContractTests.Section(Source, "private static void HardSnap(", "internal static bool TryFindFreeSpot(");
        Assert.Contains("TryFindFreeSpot(", hard);
        Assert.True(hard.IndexOf("TryFindFreeSpot(") < hard.IndexOf("HardSnaps++"));
        Assert.Contains("TrySupportedSpot(", Source);
        Assert.Contains("TryGetPlacementBounds(", Source);
        Assert.Contains("IsPlacementFree(", Source);
        Assert.Contains("RoadSnapSafetyPolicy.TryPlacementBounds(", Source);
        Assert.Contains("RoadSnapSafetyPolicy.TryRootHeight(", Source);
        Assert.Contains("RoadSnapSafetyPolicy.IsSafeSupport(", Source);
        Assert.Contains("GetComponentInParent<Il2CppScheduleOne.Vehicles.LandVehicle>()", Source);
        Assert.DoesNotContain("Physics.queriesHitTriggers =", Source);
    }
}

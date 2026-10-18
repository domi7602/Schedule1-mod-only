using Xunit;
namespace TaxiDriver.Tests;
public class DestinationIdentityWiringTests
{
    [Fact]
    public void PickUsesLogicalPolicyAndMetadataOnlyReturnsBeforeReroute()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string pick = DriverSafetySourceContractTests.Section(source, "internal static bool ToDestination(", "internal static bool ReRoute(");
        Assert.Contains("DestinationSelectionPolicy.Select", pick);
        Assert.Contains("SpikeState.RideGoal = destination.Goal", pick);
        Assert.Contains("SpikeState.ActiveTripName = result.ActiveName", pick);
        Assert.Contains("SpikeState.ActiveTripPoint = routePoint", pick);
        Assert.True(pick.IndexOf("DestinationPickMode.MetadataOnly") < pick.IndexOf("return ReRoute(caller)"));
        Assert.DoesNotContain("NextNavOrder", pick);
        Assert.DoesNotContain("FareMeter", pick);
        Assert.DoesNotContain("BeginNavigationOperation", pick);
    }
}

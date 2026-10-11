using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Wiring contracts for service-feedback lifetime; not a native runtime reproduction.</summary>
public class ServiceFeedbackSourceContractTests
{
    [Fact]
    public void OwnedTaxiCleanup_DoesNotEraseServiceFeedback()
    {
        string state = DriverSafetySourceContractTests.ReadSource("SpikeState.cs");
        string reset = DriverSafetySourceContractTests.Section(state, "internal static bool Reset()", "internal static void ClearServiceStatus");
        Assert.DoesNotContain("ServiceStatus = string.Empty", reset);
    }

    [Fact]
    public void NextOrderAndSessionTeardown_ClearTheFeedback()
    {
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string call = DriverSafetySourceContractTests.Section(commands, "internal static bool CallTaxi(", "internal static Vector3? PlayerRoadTarget()");
        Assert.Contains("ClearServiceStatus", call);

        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string sceneChange = DriverSafetySourceContractTests.Section(runner, "internal static void OnPreSceneChange()", "internal static void Shutdown()");
        Assert.Contains("ClearServiceStatus", sceneChange);

        string mod = DriverSafetySourceContractTests.ReadSource("Mod.cs");
        string preLoad = DriverSafetySourceContractTests.Section(mod, "private static void OnPreLoad()", "private static void OnPreSceneChange()");
        Assert.Contains("ClearServiceStatus", preLoad);
    }

    [Fact]
    public void App_ArbitratesThroughTheSharedFeedbackSeam()
    {
        string app = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        Assert.Contains("ServiceFeedback _feedback = new()", app);
        Assert.Contains("_feedback.Resolve(", app);
    }
}

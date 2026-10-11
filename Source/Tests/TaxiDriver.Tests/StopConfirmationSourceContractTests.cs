using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Source wiring checks; the pure StopConfirmationTests exercise the actual decision logic.</summary>
public class StopConfirmationSourceContractTests
{
    [Fact]
    public void ClosingThePhoneOrApp_ClearsConfirmationWithoutDestroyingUi()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        string close = DriverSafetySourceContractTests.Section(source, "protected override void OnPhoneClosed()", "private void Update()");
        Assert.Contains("ClearStopConfirmation()", close);
        Assert.DoesNotContain("Destroy(", close);
        string update = DriverSafetySourceContractTests.Section(source, "private void Update()", "private void CreateHero(");
        Assert.Contains("_stopConfirmation.Synchronize(open, SpikeState.RideActive, SpikeState.RideSerial)", update);
        Assert.Contains("ClearStopConfirmation();\n            CloseApp()", update);
    }

    [Fact]
    public void RideStart_CreatesANewConfirmationIdentity()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string start = DriverSafetySourceContractTests.Section(source, "internal static bool StartRide(", "internal static void EndRide(");
        Assert.Contains("SpikeState.RideSerial++", start);
    }
}

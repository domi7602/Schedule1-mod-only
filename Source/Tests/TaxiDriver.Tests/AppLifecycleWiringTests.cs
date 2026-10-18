using Xunit;
namespace TaxiDriver.Tests;
public class AppLifecycleWiringTests
{
    [Fact]
    public void EveryAppRegistrationIsOwnedAndRowsReleaseBeforeNativeDestroy()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        Assert.DoesNotContain("ButtonUtils.AddListener", source);
        Assert.DoesNotContain("RemoveAllListeners", source);
        Assert.Contains("OwnSearchListener(_searchInput", source);
        string rebuild = DriverSafetySourceContractTests.Section(source, "private void RebuildDestinationList()", "private bool MatchesFilter(");
        Assert.True(rebuild.IndexOf("_rowSubscriptions.Clear()") >= 0);
        Assert.True(rebuild.IndexOf("_rowSubscriptions.Clear()") < rebuild.IndexOf("UIFactory.ClearChildren"));
        string destroy = DriverSafetySourceContractTests.Section(source, "protected override void OnDestroyed()", "protected override void OnCreatedUI(");
        Assert.True(destroy.IndexOf("_rowSubscriptions.Clear()") < destroy.IndexOf("base.OnDestroyed()"));
        Assert.True(destroy.IndexOf("_appSubscriptions.Clear()") >= 0);
        Assert.True(destroy.IndexOf("_appSubscriptions.Clear()") < destroy.IndexOf("base.OnDestroyed()"));
        Assert.Contains("_destRows.Clear()", destroy);
        Assert.Contains("_updateSubscriptions.Clear()", destroy);
        string created = DriverSafetySourceContractTests.Section(source, "protected override void OnCreated()", "protected override void OnDestroyed()");
        Assert.Contains("LemonAction listener = lease.Invoke", created);
        Assert.Contains("_updateSubscriptions.Add", created);
        Assert.Contains("_mainBG = null", destroy);
    }
    [Fact]
    public void OwnedDelegatesReleaseAppTargetsEvenIfNativeRemovalFails()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        string bridge = DriverSafetySourceContractTests.Section(source, "private void OwnButton(", "internal static class TaxiIcon");
        Assert.Contains("new CallbackLease<bool>", bridge);
        Assert.Contains("new CallbackLease<string>", bridge);
        Assert.Contains("Action listener = lease.Invoke", bridge);
        Assert.Contains("Action<string> listener = lease.Invoke", bridge);
        Assert.Contains("lease.Release();", bridge);
    }

    [Fact]
    public void HighlightUsesLogicalIdentityRatherThanNameOnly()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        string highlight = DriverSafetySourceContractTests.Section(source, "private void RefreshDestinationHighlight()", "private void SetFilter(");
        Assert.Contains("DestinationSelectionPolicy.IsSelected", highlight);
        Assert.Contains("RideDestinationKind", highlight);
        Assert.Contains("RideGoal", highlight);
        Assert.Contains("_destSnapshot.TryGetValue", highlight);
    }

    [Fact]
    public void HeroRendersOnlyFromCompleteSnapshot()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        string refresh = DriverSafetySourceContractTests.Section(source, "private void RefreshHero()", "private void FitHeroTitle()");
        Assert.Contains("SpikeState.RideDropOff", refresh);
        Assert.Contains("SpikeState.ActiveTripName", refresh);
        Assert.Contains("SpikeState.ActiveTripPoint.HasValue", refresh);
        Assert.Contains("HeroView.Build(snapshot)", refresh);
        string render = refresh.Substring(refresh.IndexOf("_heroSnapshot = snapshot"));
        Assert.DoesNotContain("SpikeState.", render);
        Assert.DoesNotContain("FareMeter.", render);
    }
}

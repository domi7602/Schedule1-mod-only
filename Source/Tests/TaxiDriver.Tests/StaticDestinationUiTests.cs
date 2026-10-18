using Xunit;

namespace TaxiDriver.Tests;

// Source contracts only: these do not verify native Unity rendering.
public class StaticDestinationUiTests
{
    [Fact]
    public void DestinationCards_KeepFixedHeightAndInstantSelection()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        string row = DriverSafetySourceContractTests.Section(source,
            "private void MakeDestinationRow(", "private static Color TagColor(");
        Assert.Contains("le.minHeight = UITheme.Dp(44f);", row);
        Assert.Contains("le.flexibleHeight = 0f;", row);
        Assert.Contains("le.layoutPriority = 1;", row);
        Assert.Contains("btn.transition = Selectable.Transition.None;", row);
    }

    [Fact]
    public void DestinationScroll_HasNoInertialMotion()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiApp.cs");
        string list = DriverSafetySourceContractTests.Section(source,
            "private void CreateList(Transform parent)", "private void RebuildDestinationList()");
        Assert.Contains("scroll.inertia = false;", list);
        Assert.Contains("scroll.velocity = Vector2.zero;", list);
        Assert.Contains("scroll.movementType = ScrollRect.MovementType.Clamped;", list);
    }
}

using Xunit;

namespace Minimap.Tests;

public sealed class SettingsOrganizationTests
{
    private static string Source()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        return File.ReadAllText(Path.Combine(dir!.FullName, "Source", "Mods", "Minimap", "src", "MinimapSettingsApp.cs"));
    }

    // Wiring guards only: actual Unity rendering remains a live-game check.
    [Fact]
    public void ScrollRegionIsAnchoredBetweenTabsAndFooter()
    {
        var s = Source();
        Assert.Contains("PinScrollRegion(scrollObj)", s);
        Assert.DoesNotContain("_mainBG.AddComponent<VerticalLayoutGroup>()", s);
    }

    [Fact]
    public void MapAndColorsUseEqualColumnBands()
    {
        var s = Source();
        Assert.Contains("CreateColumnPair(page, \"MapColumns\")", s);
        Assert.Contains("CreateColumnPair(page, \"ColorColumns\")", s);
        Assert.Contains("AddSectionLabel(appearance, \"APPEARANCE\")", s);
        Assert.Contains("AddSectionLabel(behavior, \"BEHAVIOR\")", s);
    }

    [Fact]
    public void RowsStayCompactAndResetIsColorsOnly()
    {
        var s = Source();
        Assert.DoesNotContain("le.flexibleHeight = 1f;", s.Substring(s.IndexOf("private void RegisterStretchable"), 500));
        Assert.Contains("_resetColorsMask.SetActive(index == 2)", s);
        Assert.Contains("ConfigureRowColumns(row)", s);
    }
}

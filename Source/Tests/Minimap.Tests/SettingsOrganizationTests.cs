using Xunit;

namespace Minimap.Tests;

public sealed class SettingsOrganizationTests
{
    private static string Source() => ReadSource("MinimapSettingsApp.cs");

    /// <summary>
    /// Resolves a mod source file by name from the src root, searching the domain
    /// subfolders (src/&lt;Domain&gt;/&lt;File&gt;.cs) as well as the root itself.
    /// </summary>
    private static string ReadSource(string name)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        string root = Path.Combine(dir!.FullName, "Source", "Mods", "Minimap", "src");

        string direct = Path.Combine(root, name);
        if (File.Exists(direct)) return File.ReadAllText(direct);

        string[] matches = Directory.GetFiles(root, name, SearchOption.AllDirectories);
        if (matches.Length == 1) return File.ReadAllText(matches[0]);
        if (matches.Length > 1)
            throw new IOException($"Ambiguous Minimap source name '{name}': {string.Join(", ", matches)}");
        throw new FileNotFoundException("Minimap source file not found", name);
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

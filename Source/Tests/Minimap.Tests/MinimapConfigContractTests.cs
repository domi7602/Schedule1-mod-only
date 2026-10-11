using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Minimap.Tests;

/// <summary>
/// Source configuration guards for the feature surface. These verify that the config
/// contract the minimap relies on still exists; they are not a substitute for in-game
/// rendering tests.
/// </summary>
public sealed class MinimapConfigContractTests
{
    private static string RepoFile(params string[] parts)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null)
        {
            string candidate = Path.Combine(directory.FullName, Path.Combine(parts));
            if (File.Exists(candidate)) return File.ReadAllText(candidate);
            directory = directory.Parent;
        }
        throw new FileNotFoundException($"Run these repository layout guards from a repository build output. Missing: {string.Join("/", parts)}");
    }

    [Fact]
    public void Config_DeclaresNewFeatureToggles()
    {
        string src = RepoFile("Source", "Mods", "Minimap", "src", "MinimapConfig.cs");
        foreach (string member in new[]
                 {
                     "ShowPurchaseChecks", "ShowRegionalLocks",
                     "HideInMenus", "ShowMapBorder", "BorderThickness"
                 })
        {
            Assert.Matches(new Regex($@"public\s+bool\s+{member}\b|public\s+float\s+{member}\b"), src);
        }
    }

    [Fact]
    public void ZoomClamp_UsesSharedFeatureRules()
    {
        string hud = RepoFile("Source", "Mods", "Minimap", "src", "MinimapHUD.cs");
        string commands = RepoFile("Source", "Mods", "Minimap", "src", "MinimapCommands.cs");
        Assert.Contains("MinimapFeatureRules.ClampZoom", hud);
        Assert.Contains("MinimapFeatureRules.ClampZoom", commands);
    }

    [Fact]
    public void BlipScan_UsesSharedMarkerRules()
    {
        string blips = RepoFile("Source", "Mods", "Minimap", "src", "MinimapBlips.cs");
        Assert.Contains("MinimapFeatureRules.ResolveCustomerMarker", blips);
        Assert.DoesNotContain("MinimapFeatureRules.ResolveCustomerLabel", blips);
        Assert.Contains("ResolveCustomerPortrait", blips);
        Assert.Contains("MinimapFeatureRules.ShouldShowPurchaseCheck", blips);
        Assert.Contains("MinimapFeatureRules.ShouldShowRegionLock", blips);
    }

    [Fact]
    public void SettingsApp_UsesTabNavigation()
    {
        string app = RepoFile("Source", "Mods", "Minimap", "src", "MinimapSettingsApp.cs");
        Assert.Contains("BuildMapPage", app);
        Assert.Contains("BuildMarkerPage", app);
        Assert.Contains("BuildColorsPage", app);
        Assert.Contains("BuildAdvancedPage", app);
        Assert.Contains("ShowPage", app);
        Assert.Contains("BuildFooter(_mainBG.transform)", app);
        Assert.DoesNotContain("BuildFooter(_scrollContent)", app);
    }

    [Fact]
    public void SettingsApp_ColourRowsUsePalettePicker()
    {
        string app = RepoFile("Source", "Mods", "Minimap", "src", "MinimapSettingsApp.cs");
        Assert.Contains("MinimapFeatureRules.ColorPalette", app);
        Assert.DoesNotContain("HexInput_", app);
        Assert.DoesNotContain("ApplyBorderColor", app);
    }

    [Fact]
    public void SettingsApp_ZoomRowRemoved()
    {
        string app = RepoFile("Source", "Mods", "Minimap", "src", "MinimapSettingsApp.cs");
        Assert.DoesNotContain("Zoom (0.1-12)", app);
    }
}

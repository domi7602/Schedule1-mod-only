using System;
using System.Linq;
using System.Text.RegularExpressions;
using Minimap;
using Xunit;

namespace Minimap.Tests;

public sealed class MinimapFeatureRulesTests
{
    [Theory]
    [InlineData(-5f, 0.1f)]
    [InlineData(0.1f, 0.1f)]
    [InlineData(1.75f, 1.75f)]
    [InlineData(12f, 12f)]
    [InlineData(20f, 12f)]
    public void Zoom_IsClampedToRequestedRange(float value, float expected)
    {
        Assert.Equal(expected, MinimapFeatureRules.ClampZoom(value), 3);
    }

    [Theory]
    [InlineData(-0.2f, 0f)]
    [InlineData(0.4f, 0.4f)]
    [InlineData(1.4f, 1f)]
    public void Opacity_IsClampedBetweenTransparentAndOpaque(float value, float expected)
    {
        Assert.Equal(expected, MinimapFeatureRules.ClampOpacity(value), 3);
    }

    [Theory]
    [InlineData(-2f, 0f)]
    [InlineData(5f, 5f)]
    [InlineData(20f, 12f)]
    public void BorderThickness_IsClampedToSupportedRange(float value, float expected)
    {
        Assert.Equal(expected, MinimapFeatureRules.ClampBorderThickness(value), 3);
    }

    [Fact]
    public void AwaitingDelivery_UsesAcceptedDealClockBeforePotentialCustomer()
    {
        Assert.Equal(
            MinimapMarkerKind.AcceptedDealClock,
            MinimapFeatureRules.ResolveCustomerMarker(
                isAwaitingDelivery: true,
                isAtDealLocation: false,
                isPotential: true,
                showKnownCustomers: true));
    }

    [Fact]
    public void PotentialCustomer_UsesPurpleHighlightState()
    {
        Assert.Equal(
            MinimapMarkerKind.PotentialCustomer,
            MinimapFeatureRules.ResolveCustomerMarker(
                isAwaitingDelivery: false,
                isAtDealLocation: false,
                isPotential: true,
                showKnownCustomers: true));
    }

    [Fact]
    public void KnownCustomer_IsOmittedWhenKnownCustomerFilterIsOff()
    {
        Assert.Equal(
            MinimapMarkerKind.None,
            MinimapFeatureRules.ResolveCustomerMarker(
                isAwaitingDelivery: false,
                isAtDealLocation: false,
                isPotential: false,
                showKnownCustomers: false));
    }

    [Theory]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(false, false, false)]
    public void PurchaseCheck_OnlyAppearsOnUnownedPurchasableProperty(bool canBePurchased, bool isOwned, bool expected)
    {
        Assert.Equal(expected, MinimapFeatureRules.ShouldShowPurchaseCheck(canBePurchased, isOwned));
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void RegionLockMarker_OnlyAppearsForLockedRegions(bool isUnlocked, bool expected)
    {
        Assert.Equal(expected, MinimapFeatureRules.ShouldShowRegionLock(isUnlocked));
    }

    [Fact]
    public void ColorPalette_IsValidUniqueHexList()
    {
        string[] palette = MinimapFeatureRules.ColorPalette;
        Assert.Equal(16, palette.Length);
        Assert.Equal(palette.Length, palette.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        foreach (string hex in palette)
        {
            Assert.Matches(new Regex("^#[0-9A-F]{6}$"), hex);
        }
    }

    [Theory]
    [InlineData(923, false, "9:23 AM")]
    [InlineData(0, false, "12:00 AM")]
    [InlineData(1200, false, "12:00 PM")]
    [InlineData(1320, false, "1:20 PM")]
    [InlineData(923, true, "09:23")]
    public void Clock_StaysOneTokenWithoutALineBreak(int hhmm, bool use24H, string expected)
    {
        string clock = MinimapFeatureRules.FormatClock(hhmm, use24H);
        Assert.Equal(expected, clock);
        Assert.DoesNotContain("\n", clock);
    }

    [Fact]
    public void DayHeader_KeepsAmPmOnTheClockLine()
    {
        string text = MinimapFeatureRules.FormatDayHeader(2, "Wed", MinimapFeatureRules.FormatClock(923, false),
            showDayNumber: true, showDayName: true, showTime: true);
        Assert.Equal("DAY 2 │ Wed 9:23 AM", text);
        Assert.DoesNotContain("\n", text);
    }

    [Fact]
    public void DayHeader_DoesNotAskVanillaToFormatTwelveHourTime()
    {
        string hud = ReadModSource("MinimapHUD.cs");
        Assert.DoesNotContain("Get12HourTime", hud);
        Assert.Contains("_dayTmp.enableWordWrapping = false;", hud);
        Assert.Contains("config.ShowTime || config.ShortDayName", hud);
    }

    private static string ReadModSource(string name)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        return File.ReadAllText(Path.Combine(dir!.FullName, "Source", "Mods", "Minimap", "src", name));
    }
}

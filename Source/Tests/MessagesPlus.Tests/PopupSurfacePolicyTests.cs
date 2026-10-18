using MessagesPlus;
using Xunit;

namespace MessagesPlus.Tests;

public sealed class PopupSurfacePolicyTests
{
    [Fact]
    public void PopupText_PreservesDarkColoredPricing()
    {
        ThemeRgb green = new(0.20f, 0.35f, 0.20f);
        Assert.False(ThemeColorPolicy.ShouldWrite(false, ThemeRole.PopupText,
            green, default, green, new ThemeRgb(0.95f, 0.95f, 0.95f)));
    }

    [Fact]
    public void PopupText_HealsDifferentNeutralResetShade()
    {
        Assert.True(ThemeColorPolicy.ShouldWrite(true, ThemeRole.PopupText,
            new ThemeRgb(0.10f, 0.10f, 0.10f), new ThemeRgb(0.95f, 0.95f, 0.95f),
            new ThemeRgb(0.35f, 0.35f, 0.35f), new ThemeRgb(0.95f, 0.95f, 0.95f)));
    }

    [Theory]
    [InlineData(false, 0.50f)] // medium neutral response footer, not near-white
    [InlineData(true, 0.80f)] // vanilla can reset to a different neutral shade
    public void PopupSurface_ThemesGrayAndHealsNeutralResets(bool tracked, float gray)
    {
        Assert.True(ThemeColorPolicy.ShouldWrite(tracked, ThemeRole.PopupSurface,
            new ThemeRgb(1, 1, 1), new ThemeRgb(0.08f, 0.09f, 0.12f),
            new ThemeRgb(gray, gray, gray), new ThemeRgb(0.08f, 0.09f, 0.12f)));
    }

    [Theory]
    [InlineData(0.80f, 1.00f, 0.80f)] // pastel price/action green passes the app's permissive white gate
    [InlineData(0.30f, 0.50f, 0.70f)] // clock segment
    [InlineData(0.02f, 0.02f, 0.02f)] // clock hand / existing dark surface
    public void PopupSurface_RejectsMeaningfulColorsAndDarkArt(float r, float g, float b)
    {
        ThemeRgb color = new(r, g, b);
        Assert.False(ThemeColorPolicy.ShouldWrite(false, ThemeRole.PopupSurface,
            color, default, color, new ThemeRgb(0.08f, 0.09f, 0.12f)));
    }
}

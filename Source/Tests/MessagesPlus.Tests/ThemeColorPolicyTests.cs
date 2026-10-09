using MessagesPlus;
using Xunit;

namespace MessagesPlus.Tests;

public sealed class ThemeColorPolicyTests
{
    private static ThemeRgb Rgb(float value) => new(value, value, value);

    [Fact]
    public void FirstIntervention_RequestsTintWhenColorsDiffer()
    {
        Assert.True(ThemeColorPolicy.ShouldWrite(
            hasState: false,
            ThemeRole.PageBackground,
            original: Rgb(0.95f),
            lastTheme: default,
            current: Rgb(0.95f),
            target: Rgb(0.05f)));
    }

    [Fact]
    public void RepeatedCall_DoesNotRewriteThemeColor()
    {
        Assert.False(ThemeColorPolicy.ShouldWrite(
            hasState: true,
            ThemeRole.PageBackground,
            original: Rgb(0.95f),
            lastTheme: Rgb(0.05f),
            current: Rgb(0.05f),
            target: Rgb(0.05f)));
    }

    [Fact]
    public void KnownSurfaceResetToOriginal_IsRethemed()
    {
        Assert.True(ThemeColorPolicy.ShouldWrite(
            hasState: true,
            ThemeRole.InboxRowBackground,
            original: Rgb(0.95f),
            lastTheme: Rgb(0.08f),
            current: Rgb(0.95f),
            target: Rgb(0.08f)));
    }

    [Fact]
    public void ExternalStateColor_IsPreserved()
    {
        Assert.False(ThemeColorPolicy.ShouldWrite(
            hasState: true,
            ThemeRole.InboxRowBackground,
            original: Rgb(0.95f),
            lastTheme: Rgb(0.08f),
            current: new ThemeRgb(0.72f, 0.82f, 0.91f),
            target: Rgb(0.08f)));
    }

    [Fact]
    public void ThemeRoleChange_ReappliesNewTargetFromPriorThemeColor()
    {
        Assert.True(ThemeColorPolicy.ShouldWrite(
            hasState: true,
            ThemeRole.SecondaryText,
            original: Rgb(0.20f),
            lastTheme: Rgb(0.94f),
            current: Rgb(0.94f),
            target: Rgb(0.55f)));
    }

    [Fact]
    public void ProtectedContent_IsNeverRetinted()
    {
        Assert.False(ThemeColorPolicy.ShouldWrite(
            hasState: false,
            ThemeRole.ProtectedContent,
            original: Rgb(0.90f),
            lastTheme: default,
            current: Rgb(0.90f),
            target: Rgb(0.05f)));
    }

    [Fact]
    public void NearEqualThemeColor_IsTreatedAsAlreadyApplied()
    {
        Assert.False(ThemeColorPolicy.ShouldWrite(
            hasState: true,
            ThemeRole.PrimaryText,
            original: Rgb(0.20f),
            lastTheme: Rgb(0.94f),
            current: new ThemeRgb(0.941f, 0.940f, 0.939f),
            target: Rgb(0.94f)));
    }

    [Fact]
    public void StatusIndicatorRole_IsNeverRetinted()
    {
        Assert.False(ThemeColorPolicy.ShouldWrite(
            hasState: false,
            ThemeRole.StatusIndicator,
            original: Rgb(0.90f),
            lastTheme: default,
            current: Rgb(0.90f),
            target: Rgb(0.05f)));
    }

    [Fact]
    public void OutgoingContentRole_IsNeverRetinted()
    {
        Assert.False(ThemeColorPolicy.ShouldWrite(
            hasState: false,
            ThemeRole.OutgoingContent,
            original: Rgb(0.90f),
            lastTheme: default,
            current: Rgb(0.90f),
            target: Rgb(0.05f)));
    }
}

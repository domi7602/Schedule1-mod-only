using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

public sealed class AppLayoutTests
{
    [Theory]
    [InlineData(1f)]
    [InlineData(1.6013333f)]
    [InlineData(2f)]
    public void ExpandedPanelAddsOneSliderAndOneOuterGap(float scale)
    {
        float collapsed = AppLayout.ContentHeight(scale, false);
        float expanded = AppLayout.ContentHeight(scale, true);
        Assert.Equal(AppLayout.ContentHeight(scale), collapsed);
        Assert.Equal(538f * scale, expanded, 2);
        Assert.Equal((AppLayout.SliderHeight + AppLayout.Gap) * scale, expanded - collapsed, 2);
    }

    [Theory]
    [InlineData(750f, 1f)]
    [InlineData(1201f, 1.6013333f)]
    [InlineData(1500f, 2f)]
    public void MinimumCollapsedControlStackFitsOriginalPhoneViewport(float phoneHeight, float scale)
    {
        float height = AppLayout.ContentHeight(scale);
        Assert.Equal(472f * scale, height, 2);
        Assert.True(height <= phoneHeight * 0.885f, $"content {height} exceeds scroll viewport {phoneHeight * 0.885f}");
    }
}

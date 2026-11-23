using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

public sealed class ResponsiveLayoutTests
{
    [Theory]
    [InlineData(750f, 1f)]
    [InlineData(1201f, 1.6013333f)]
    [InlineData(1500f, 2f)]
    public void FullHeightDistributionMatchesSumOfNativeContracts(float phone, float scale)
    {
        float viewport = phone * 0.885f;
        var size = AppLayout.Measure(viewport, scale, false);
        float stretch = size.Selector / (AppLayout.SelectorHeight * scale);
        float sum = size.Selector + 4 * size.Slider + size.Power + 5 * size.Gap +
            2 * AppLayout.Padding * scale * stretch;
        float minimum = AppLayout.ContentHeight(scale);
        Assert.True(minimum <= viewport);
        Assert.InRange(System.Math.Abs(viewport - size.Content), 0, 0.01f);
        Assert.InRange(System.Math.Abs(sum - size.Content), 0, 0.01f);
    }

    [Fact]
    public void SmallViewportOverflowsInsteadOfClippingLowerActions()
    {
        var size = AppLayout.Measure(400, 1.6f, false);
        Assert.Equal(AppLayout.ContentHeight(1.6f), size.Content, 2);
        Assert.True(size.Content > 400);
        Assert.Equal(AppLayout.PowerHeight * 1.6f, size.Power, 2);
    }

    [Fact]
    public void InlineStateAddsOnlyItsRealHeightAndGapAtMinimum()
    {
        var normal = AppLayout.Measure(400, 1, false);
        var state = AppLayout.Measure(400, 1, true);
        Assert.Equal(AppLayout.InlineStateHeight + AppLayout.Gap, state.Content - normal.Content, 2);
        var stretched = AppLayout.Measure(1062.885f, 1.6013333f, true);
        float minimum = AppLayout.ContentHeight(1.6013333f) +
            (AppLayout.InlineStateHeight + AppLayout.Gap) * 1.6013333f;
        if (minimum <= 1062.885f)
            Assert.Equal(1062.885f, stretched.Content, 2);
        else
        {
            Assert.Equal(minimum, stretched.Content, 2);
            Assert.True(stretched.Content > 1062.885f);
        }
    }

    [Fact]
    public void ExtremeViewportBoundsControlGrowthAndGroupGaps()
    {
        var size = AppLayout.Measure(10000, 1, false);
        Assert.Equal(AppLayout.SelectorHeight * 1.5f, size.Selector);
        Assert.Equal(AppLayout.Gap * 3, size.Gap);
        Assert.True(size.Content < 10000); // Do not create enormous cards or a blank filler.
    }

    [Fact]
    public void FirstOpenStaleRectsConvergeAndStableMeasurementsDoNotRebuild()
    {
        Assert.True(AppLayout.ShouldApply(596, 750, -1, -1, true));
        Assert.True(AppLayout.ShouldApply(596, 1062.885f, 596, 750, false));
        Assert.False(AppLayout.ShouldApply(596, 1062.885f, 596, 1062.885f, false));
        Assert.False(AppLayout.ShouldApply(596.1f, 1063, 596, 1062.885f, false));
        Assert.True(AppLayout.ShouldApply(596, 1062.885f, 596, 1062.885f, true));
        Assert.False(AppLayout.ShouldApply(0, 0, -1, -1, true));
        Assert.False(AppLayout.ShouldApply(float.NaN, 1062, -1, -1, true));
    }

    [Fact]
    public void ObservedPhoneUsesFreedFooterHeightAcrossRealControls()
    {
        float scale = 1201f / 750f;
        var size = AppLayout.Measure(1201f * 0.885f, scale, false);
        Assert.Equal(1201f * 0.885f, size.Content, 2);
        Assert.True(size.Selector > AppLayout.SelectorHeight * scale);
        Assert.True(size.Slider > AppLayout.SliderHeight * scale);
        Assert.True(size.Gap > AppLayout.Gap * scale);
    }
}

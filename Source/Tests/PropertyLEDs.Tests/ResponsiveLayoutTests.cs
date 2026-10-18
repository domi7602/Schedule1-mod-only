using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

public sealed class ResponsiveLayoutTests
{
    [Theory]
    [InlineData(750f, 1f, false)]
    [InlineData(750f, 1f, true)]
    [InlineData(1201f, 1.6013333f, false)]
    [InlineData(1201f, 1.6013333f, true)]
    [InlineData(1500f, 2f, false)]
    [InlineData(1500f, 2f, true)]
    public void FullHeightDistributionMatchesSumOfNativeContracts(float phone, float scale, bool expanded)
    {
        float viewport = phone * 0.885f;
        var size = AppLayout.Measure(viewport, scale, expanded, false);
        float stretch = size.Selector / (AppLayout.SelectorHeight * scale);
        float sum = size.Selector + (expanded ? 4 : 3) * size.Slider +
            size.Advanced + size.Power + size.Action + (expanded ? 7 : 6) * size.Gap +
            2 * AppLayout.Padding * scale * stretch;
        float minimum = AppLayout.ContentHeight(scale, expanded);
        if (minimum <= viewport)
            Assert.InRange(System.Math.Abs(viewport - size.Content), 0, 0.01f);
        else
        {
            // Approved Phase-1 constants exceed the scroll region when Advanced is open.
            // Measure must not shrink below the minimum; the existing scroller absorbs it.
            Assert.Equal(minimum, size.Content, 2);
            Assert.True(size.Content > viewport);
            Assert.Equal(1f, stretch, 3);
        }
        Assert.InRange(System.Math.Abs(sum - size.Content), 0, 0.01f);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SmallViewportOverflowsInsteadOfClippingLowerActions(bool expanded)
    {
        var size = AppLayout.Measure(400, 1.6f, expanded, false);
        Assert.Equal(AppLayout.ContentHeight(1.6f, expanded), size.Content, 2);
        Assert.True(size.Content > 400);
        Assert.Equal(AppLayout.ActionHeight * 1.6f, size.Action, 2);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InlineStateAddsOnlyItsRealHeightAndGapAtMinimum(bool expanded)
    {
        var normal = AppLayout.Measure(400, 1, expanded, false);
        var state = AppLayout.Measure(400, 1, expanded, true);
        Assert.Equal(AppLayout.InlineStateHeight + AppLayout.Gap, state.Content - normal.Content, 2);
        var stretched = AppLayout.Measure(1062.885f, 1.6013333f, expanded, true);
        float minimum = AppLayout.ContentHeight(1.6013333f, expanded) +
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
        var size = AppLayout.Measure(10000, 1, false, false);
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
        var size = AppLayout.Measure(1201f * 0.885f, scale, false, false);
        Assert.Equal(1201f * 0.885f, size.Content, 2);
        Assert.True(size.Selector > AppLayout.SelectorHeight * scale);
        Assert.True(size.Slider > AppLayout.SliderHeight * scale);
        Assert.True(size.Gap > AppLayout.Gap * scale);
    }
}

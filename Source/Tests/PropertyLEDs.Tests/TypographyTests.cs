using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

// Pure typography ramp and text-area fit. The app builds every visible label from these
// constants, so a value below the readability floor or a band that cannot hold its text
// fails here before it can ship as unreadable UI.
public sealed class TypographyTests
{
    private const int ReadabilityFloor = 11;

    [Fact]
    public void EveryVisibleLabelMeetsTheReadabilityFloor()
    {
        Assert.True(AppLayout.FontTitle >= ReadabilityFloor);
        Assert.True(AppLayout.FontSubtitle >= ReadabilityFloor);
        Assert.True(AppLayout.FontSelectorCaption >= ReadabilityFloor);
        Assert.True(AppLayout.FontSelectorLabel >= ReadabilityFloor);
        Assert.True(AppLayout.FontSelectorContext >= ReadabilityFloor);
        Assert.True(AppLayout.FontSliderCaption >= ReadabilityFloor);
        Assert.True(AppLayout.FontSliderValue >= ReadabilityFloor);
        Assert.True(AppLayout.FontPowerLabel >= ReadabilityFloor);
        Assert.True(AppLayout.FontPowerDetail >= ReadabilityFloor);
        Assert.True(AppLayout.FontButton >= ReadabilityFloor);
        Assert.True(AppLayout.FontInlineState >= ReadabilityFloor);
    }

    [Fact]
    public void RampRunsFromTitleDownToHelpTextWithoutInversions()
    {
        Assert.True(AppLayout.FontTitle > AppLayout.FontSelectorLabel);
        Assert.True(AppLayout.FontSelectorLabel > AppLayout.FontSliderCaption);
        Assert.Equal(AppLayout.FontSliderCaption, AppLayout.FontSliderValue);
        Assert.Equal(AppLayout.FontSliderValue, AppLayout.FontPowerLabel);
        Assert.True(AppLayout.FontPowerLabel > AppLayout.FontSelectorCaption);
        Assert.True(AppLayout.FontSelectorCaption > AppLayout.FontButton);
        Assert.True(AppLayout.FontButton > AppLayout.FontSelectorContext);
        Assert.True(AppLayout.FontSelectorContext > AppLayout.FontSubtitle);
        Assert.Equal(AppLayout.FontPowerDetail, AppLayout.FontInlineState);
        Assert.True(AppLayout.FontPowerDetail >= AppLayout.FontSubtitle);
    }

    [Fact]
    public void SelectorTextBandsHoldTheirFontsAndStayOrderedInsideTheCard()
    {
        float caption = AppLayout.SelectorCaptionBandTop - AppLayout.SelectorCaptionBandBottom;
        float label = AppLayout.SelectorLabelBandTop - AppLayout.SelectorLabelBandBottom;
        float context = AppLayout.SelectorContextBandTop - AppLayout.SelectorContextBandBottom;
        Assert.True(caption >= AppLayout.FontSelectorCaption);
        Assert.True(label >= AppLayout.FontSelectorLabel);
        Assert.True(context >= AppLayout.FontSelectorContext);
        // Bands stack bottom (context) to top (caption) without overlapping and stay inside the card.
        Assert.True(AppLayout.SelectorContextBandTop <= AppLayout.SelectorLabelBandBottom);
        Assert.True(AppLayout.SelectorLabelBandTop <= AppLayout.SelectorCaptionBandBottom);
        Assert.True(AppLayout.SelectorCaptionBandTop <= AppLayout.SelectorHeight - AppLayout.Padding);
        Assert.True(AppLayout.SelectorContextBandBottom >= 0f);
    }

    [Fact]
    public void HorizontalBandsAreTallerThanTheTextTheyCarry()
    {
        Assert.True(AppLayout.SliderHeadingHeight >= AppLayout.FontSliderCaption);
        Assert.True(AppLayout.InlineStateHeight >= AppLayout.FontInlineState);
        Assert.True(AppLayout.AdvancedHeight >= AppLayout.FontButton);
        Assert.True(AppLayout.ActionHeight >= AppLayout.FontButton);
    }
}

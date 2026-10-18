using Xunit;

namespace PropertyLEDs.Tests;

// Source contracts only: these do not exercise IL2CPP rendering or hit testing.
public sealed class AppLayoutWiringTests
{
    private static string Source()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        return File.ReadAllText(Path.Combine(dir!.FullName, "Source", "Mods", "PropertyLEDs", "src", "PropertyLEDsApp.cs"));
    }

    [Fact]
    public void SaturationTrackRunsWhiteToSelectedHueAndUpdatesOnlyWhenHueChanges()
    {
        var source = Source();
        Assert.Contains("trackImage.sprite = GetSaturationTrack();", source);
        Assert.Contains("UpdateSaturationTrack(_shown.Hue);", source);
        Assert.Contains("Color.HSVToRGB(hue, x / 255f, 1f)", source);
        Assert.Contains("if (!NetworkGuard.IsAlive(_saturationTexture) || _saturationHue == hue) return;", source);
        Assert.Contains("if (index < 2) fill.SetActive(false);", source);
        Assert.Contains("if (index >= 2) slider.fillRect = fill.GetComponent<RectTransform>();", source);
        Assert.Contains("if (NetworkGuard.IsAlive(_saturationSprite)) Object.Destroy(_saturationSprite);", source);
        Assert.Contains("if (NetworkGuard.IsAlive(_saturationTexture)) Object.Destroy(_saturationTexture);", source);
    }

    [Fact]
    public void ColorUsesFullRainbowTrackWithRedEndpointsAndReleasesAssetsOnlyOnDestroy()
    {
        var source = Source();
        Assert.Contains("trackImage.sprite = GetHueTrack();", source);
        Assert.Contains("trackImage.color = Color.white;", source);
        Assert.Contains("if (index < 2) fill.SetActive(false);", source);
        Assert.Contains("if (index >= 2) slider.fillRect = fill.GetComponent<RectTransform>();", source);
        Assert.Contains("Color.HSVToRGB(x / 255f, 1f, 1f)", source);
        Assert.Contains("_hueTexture.wrapMode = TextureWrapMode.Clamp;", source);
        Assert.Contains("_hueTexture.filterMode = FilterMode.Bilinear;", source);
        Assert.Contains("slider.minValue = 0;", source);
        Assert.Contains("slider.maxValue = max;", source);
        Assert.Contains("slider.handleRect = handleRect;", source);
        Assert.Contains("if (NetworkGuard.IsAlive(_hueSprite)) Object.Destroy(_hueSprite);", source);
        Assert.Contains("if (NetworkGuard.IsAlive(_hueTexture)) Object.Destroy(_hueTexture);", source);
        int close = source.IndexOf("protected override void OnPhoneClosed()", StringComparison.Ordinal);
        int destroy = source.IndexOf("protected override void OnDestroyed()", close, StringComparison.Ordinal);
        Assert.DoesNotContain("Object.Destroy", source.Substring(close, destroy - close));
    }

    [Fact]
    public void NormalPageUsesPlainEnglishLabelsAndKeepsHiddenHueValueSlotWithoutDegrees()
    {
        var source = Source();
        Assert.Contains("BuildSlider(content, 0, \"Color\", 1f);", source);
        Assert.Contains("BuildSlider(content, 1, \"Saturation\", 1f);", source);
        Assert.Contains("BuildSlider(content, 3, \"Brightness\", 5f);", source);
        Assert.Contains("if (index == 0) _values[index].gameObject.SetActive(false);", source);
        Assert.Contains("_values[0].text = \"\";", source);
        Assert.DoesNotContain(" deg", source);
        Assert.Contains("_values[1].text = F(_shown.Saturation * 100f) + \"%\";", source);
        Assert.Contains("_values[3].text = F(_shown.Brightness) + \" / 5\";", source);
        Assert.DoesNotContain("var rgb = _shown.ToRgb();", source);
    }

    [Fact]
    public void AdvancedIntensityIsBuiltOnceCollapsedOnOpenAndToggleOnlyRebuildsView()
    {
        var source = Source();
        Assert.Contains("BuildSlider(_advancedPanel.transform, 2, \"Color intensity\", 1f);", source);
        Assert.Contains("Height(_advancedPanel, AppLayout.SliderHeight);", source);
        Assert.Contains("_advancedPanel.SetActive(false);", source);
        Assert.Contains("if (opened) SetAdvancedExpanded(false);", source);
        Assert.Contains("\"Advanced\", ToggleAdvanced", source);
        Assert.Contains("_advancedPanel.SetActive(expanded);", source);
        Assert.Contains("AppLayout.Measure(height, _layoutScale, _advancedExpanded, _status.gameObject.activeSelf)", source);
        Assert.Contains("advanced={_advancedExpanded}", source);
        int start = source.IndexOf("private void ToggleAdvanced()", StringComparison.Ordinal);
        int end = source.IndexOf("private void LogMeasuredLayout", start, StringComparison.Ordinal);
        string toggle = source.Substring(start, end - start);
        Assert.Contains("_layoutDirty = true;", toggle);
        Assert.Contains("LogMeasuredLayout(false);", toggle);
        Assert.DoesNotContain("\n        Edit(", toggle);
        Assert.DoesNotContain("LEDRuntime.Apply", toggle);
        Assert.DoesNotContain("Destroy(", toggle);
        Assert.DoesNotContain("BuildSlider", toggle);
        Assert.Contains("_sliders[2].value = _shown.ColorIntensity;", source);
        Assert.Contains("_values[2].text = F(_shown.ColorIntensity * 100f) + \"%\";", source);
    }

    [Fact]
    public void CompactSizingPolicyIsWiredToNativeControlsAndMeasuredAfterOpen()
    {
        var source = Source();
        Assert.Contains("Height(card, AppLayout.SelectorHeight);", source);
        Assert.Contains("Height(block, AppLayout.SliderHeight);", source);
        Assert.Contains("layout.spacing = UITheme.Dp(AppLayout.Gap);", source);
        Assert.Contains("UITheme.Dp(AppLayout.Padding)", source);
        Assert.Contains("if (open) LogMeasuredLayout(opened);", source);
        Assert.Contains("LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollContent);", source);
        Assert.Contains("Phone layout: viewport=", source);
        Assert.Contains("_scrollViewport = scroll.viewport;", source);
        Assert.Contains("AppLayout.ShouldApply(width, height, _layoutWidth, _layoutHeight, _layoutDirty)", source);
    }

    [Fact]
    public void SliderLabelsAreReadableInsideOneCompactBlockWithoutChangingTrackMechanics()
    {
        var source = Source();
        Assert.Contains("var block = UIFactory.Panel(caption + \"Control\", parent, new Color(0.06f, 0.09f, 0.12f, 0.45f));", source);
        Assert.DoesNotContain("UIFactory.Panel(caption + \"Control\", parent, Color.clear)", source);
        Assert.Contains("Row(block.transform, caption + \"Heading\", AppLayout.SliderHeadingHeight)", source);
        Assert.Contains("Label(\"Caption\", caption, heading.transform, 16)", source);
        Assert.Contains("Label(\"Value\", \"...\", heading.transform, 16, FontStyle.Bold)", source);
        Assert.Contains("UIFactory.Panel(\"Slider\" + index, block.transform, Color.clear)", source);
        Assert.Contains("Height(root, AppLayout.SliderTrackHeight);", source);
        Assert.Contains("layout.spacing = 0;", source);
    }

    [Fact]
    public void SelectorBoundsApplyToFactoryMaskRootAndLongTextCannotBleedIntoArrows()
    {
        var source = Source();
        Assert.Contains("prev.transform.parent.GetComponent<RectTransform>()", source);
        Assert.Contains("following.transform.parent.GetComponent<RectTransform>()", source);
        Assert.Contains("text.horizontalOverflow = HorizontalWrapMode.Wrap;", source);
        Assert.Contains("SelectorBand(heading, 36, 56);", source);
        Assert.Contains("SelectorBand(label, 14, 37);", source);
        Assert.Contains("SelectorBand(context, 1, 15);", source);
        Assert.Contains("UITheme.Dp(bottom - AppLayout.SelectorHeight / 2));", source);
        Assert.DoesNotContain("SelectorBandExtra", source);
        Assert.DoesNotContain("text.gameObject.AddComponent<RectMask2D>()", source);
    }

    [Fact]
    public void SelectorCardContainsCaptionNameAndSeparateContextWithAnchoredArrows()
    {
        var source = Source();
        Assert.DoesNotContain("Caption(content,", source);
        Assert.Contains("Label(name + \"Caption\", caption, card.transform, 14", source);
        Assert.Contains("Label(name + \"Label\", \"...\", card.transform, 18", source);
        Assert.Contains("Label(name + \"Context\", \"\", card.transform, 12", source);
        Assert.Contains("out _propertyLabel, out _propertyContext", source);
        Assert.DoesNotContain("BuildSelector(content, \"Room\"", source);
        Assert.Contains("prev.transform.parent.GetComponent<RectTransform>()", source);
        Assert.Contains("following.transform.parent.GetComponent<RectTransform>()", source);
        Assert.Contains("le.layoutPriority = 10;", source);
        Assert.Contains("le.flexibleHeight = 0;", source);
    }
}

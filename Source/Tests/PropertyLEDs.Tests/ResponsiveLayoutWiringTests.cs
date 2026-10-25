using Xunit;

namespace PropertyLEDs.Tests;

// Source contracts supplement pure sizing tests; no native Unity execution.
public sealed class ResponsiveLayoutWiringTests
{
    /// <summary>
    /// Resolves the shipped app view from the src root, searching the domain subfolders
    /// (src/&lt;Domain&gt;/PropertyLEDsApp.cs) as well as the root itself, so a source
    /// reorganization does not invalidate the contract. The ambiguity guard keeps it pinned
    /// to exactly one file.
    /// </summary>
    private static string Source()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        string root = Path.Combine(dir!.FullName, "Source", "Mods", "PropertyLEDs", "src");

        string direct = Path.Combine(root, "PropertyLEDsApp.cs");
        if (File.Exists(direct)) return File.ReadAllText(direct);

        string[] matches = Directory.GetFiles(root, "PropertyLEDsApp.cs", SearchOption.AllDirectories);
        if (matches.Length == 1) return File.ReadAllText(matches[0]);
        if (matches.Length > 1)
            throw new IOException($"Ambiguous PropertyLEDs source name 'PropertyLEDsApp.cs': {string.Join(", ", matches)}");
        throw new FileNotFoundException("PropertyLEDs source file not found", "PropertyLEDsApp.cs");
    }

    [Fact]
    public void NativeSizingIsAppliedOnlyAfterMeasurementOrStateChanges()
    {
        string s = Source();
        Assert.Contains("AppLayout.ShouldApply(width, height, _layoutWidth, _layoutHeight, _layoutDirty)", s);
        Assert.Contains("AppLayout.Measure(height, _layoutScale, _advancedExpanded, _status.gameObject.activeSelf)", s);
        Assert.Contains("_contentLayout.spacing = sizing.Gap;", s);
        Assert.Contains("ApplyHeight(_selectorSizes[i], sizing.Selector);", s);
        Assert.Contains("ApplyHeight(_sliderSizes[i], sizing.Slider);", s);
        Assert.Contains("ApplyHeight(_trackSizes[i], sizing.Slider - AppLayout.SliderHeadingHeight * _layoutScale);", s);
        Assert.Contains("ApplyHeight(_presetSizes[i], sizing.Action);", s);
        Assert.Contains("ApplyHeight(_advancedPanelSize, sizing.Slider);", s);
        Assert.Contains("fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;", s);
        Assert.Contains("_scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizing.Content);", s);
        Assert.DoesNotContain("Canvas.ForceUpdateCanvases();", s);
        Assert.DoesNotContain("GetComponentsInChildren", s);
        Assert.DoesNotContain("(RectTransform)", s);
        Assert.DoesNotContain("foreach (Transform", s);
        int start = s.IndexOf("private void LogMeasuredLayout", StringComparison.Ordinal);
        int end = s.IndexOf("private void SetInlineState", start, StringComparison.Ordinal);
        string apply = s.Substring(start, end - start);
        Assert.True(apply.IndexOf("AppLayout.ShouldApply", StringComparison.Ordinal) < apply.IndexOf("LayoutRebuilder.ForceRebuildLayoutImmediate", StringComparison.Ordinal));
        Assert.Contains("_layoutDirty = false;", apply);
        Assert.Contains("preview={F(sizing.Preview)}", apply);
    }

    [Fact]
    public void SliderDrivenCrossAxisCannotStretchCircularHandle()
    {
        string s = Source();
        Assert.Contains("Rect(handleAreaRect, 0, 0.54f, 0.84f, 0.54f);", s);
        Assert.Contains("handleAreaRect.sizeDelta = new Vector2(0, UITheme.Dp(23));", s);
        Assert.Contains("handleRect.sizeDelta = new Vector2(UITheme.Dp(23), 0);", s);
        Assert.Contains("handle.GetComponent<Image>().preserveAspect = true;", s);
        Assert.DoesNotContain("Rect(handleArea.GetComponent<RectTransform>(), 0, 0.1f", s);
    }

    [Fact]
    public void PhoneOpensLandscapeLikeTheOtherDashboardApps()
    {
        string s = Source();
        Assert.Contains("EOrientation.Horizontal", s);
        Assert.Contains("LandscapeReferenceHeight", s);
        Assert.DoesNotContain("EOrientation.Vertical", s);
    }

    [Fact]
    public void FooterIsRemovedAndInlineFailuresReserveSpaceOnlyWhenVisible()
    {
        string s = Source();
        Assert.DoesNotContain("DrawIcon(\"Info\"", s);
        Assert.DoesNotContain("verified room layouts", s);
        Assert.DoesNotContain("Label(\"Status\"", s);
        Assert.Contains("0.012f, 0.008f, 0.988f, 0.922f", s);
        Assert.Contains("Label(\"InlineState\"", s);
        Assert.Contains("_status.gameObject.SetActive(visible);", s);
        Assert.Contains("SetInlineState(\"Edit failed: \" + ex.Message)", s);
        Assert.Contains("SetInlineState(\"LED phone refresh failed: \" + ex.Message)", s);
        Assert.Contains("No live property anchor available.", s);
    }

    [Fact]
    public void InlineStateColorFollowsTonePolicy()
    {
        string s = Source();
        Assert.Contains("var tone = AppLayout.InlineColor(message);", s);
        Assert.Contains("_status.color = new Color(tone.R, tone.G, tone.B, tone.A);", s);
    }
}

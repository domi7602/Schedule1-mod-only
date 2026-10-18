using System.Text.RegularExpressions;
using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

// Native orchestration is inspected as source; this is not a Unity pointer/render test.
public sealed class MockupViewTests
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
    public void SolePowerActionIsStatefulToggleWithoutDuplicateListenersOrPresetPowerAction()
    {
        string s = Source();
        Assert.Contains("BuildPowerCard(content);", s);
        Assert.Contains("RoundedPanel(\"PowerState\", parent, new Color(0.08f, 0.13f, 0.17f, 1f));", s);
        Assert.Contains("Edit(s => s.Enabled = !s.Enabled)", s);
        Assert.Single(Regex.Matches(s, "s.Enabled = !s.Enabled").Cast<Match>());
        Assert.Contains("_powerTrack.color = _shown.Enabled ? Accent : Surface;", s);
        Assert.Contains("_powerKnob.anchorMin = _powerKnob.anchorMax", s);
        Assert.DoesNotContain("Turn LEDs OFF", s);
        Assert.DoesNotContain("Turn LEDs ON", s);
        Assert.Contains("EventHelper.RemoveListener(click, button.onClick);", s);
        Assert.Contains("EventHelper.AddListener(click, button.onClick);", s);
        Assert.DoesNotContain("BuildSelector(content, \"Room\"", s);
        Assert.DoesNotContain("Use property color", s);
    }

    [Fact]
    public void MockupUsesDrawnIconsOutlinedCardsRoundHandlesWithoutFooter()
    {
        string s = Source();
        Assert.Contains("CONTROL THE LED LIGHTING FOR YOUR PROPERTIES", s);
        Assert.Contains("DrawIcon(\"Bulb\"", s);
        Assert.Contains("DrawIcon(name == \"Property\" ? \"House\" : \"Layers\"", s);
        Assert.Contains("RoundedPanel(name + \"Selector\"", s);
        Assert.DoesNotContain("RoundedPanel(\"ColorPreview\"", s);
        Assert.DoesNotContain("Room / Scope", s);
        Assert.Contains("handle.GetComponent<Image>().sprite = UISprites.Circle();", s);
        Assert.Contains("DrawIcon(index == 0 ? \"Palette\"", s);
        Assert.DoesNotContain("Saved", s);
        Assert.DoesNotContain("SettingsButton", s);
        Assert.DoesNotMatch(@"[^\u0000-\u007F]", s);
        Assert.DoesNotContain("verified room layouts", s);
        Assert.Contains("_sliders[2].value = _shown.ColorIntensity;", s);
        Assert.Contains("case 0: s.Hue = value; break;", s);
        Assert.Contains("case 1: s.Saturation = value; break;", s);
        Assert.Contains("case 2: s.ColorIntensity = value; break;", s);
        Assert.Contains("case 3: s.Brightness = value; break;", s);
        Assert.Contains("s.EnableForEdit();", s);
    }

    [Fact]
    public void Phase1PaletteUsesDeeperSurfacesAndSofterAccent()
    {
        string s = Source();
        Assert.Contains("new(0.035f, 0.055f, 0.075f, 1f)", s);
        Assert.Contains("new(0.07f, 0.11f, 0.145f, 1f)", s);
        Assert.Contains("new(0.15f, 0.72f, 0.88f, 1f)", s);
        Assert.Contains("new(0.18f, 0.28f, 0.36f, 1f)", s);
        Assert.DoesNotContain("new(0.025f, 0.043f, 0.055f, 1f)", s);
        Assert.DoesNotContain("new(0f, 0.78f, 0.91f, 1f)", s);
    }

    [Fact]
    public void ToggleMaskCannotPaintWhitePlateAndGradientEndsAreRounded()
    {
        var s = Source();
        Assert.Contains("root.GetComponent<Mask>().showMaskGraphic = false;", s);
        Assert.Contains("UISprites.RoundedRectAlpha(x + 0.5f, y + 0.5f, 256, 16, 8)", s);
        Assert.Contains("new Texture2D(256, 16, TextureFormat.RGBA32, false)", s);
        Assert.DoesNotContain("_previewBorder", s);
        Assert.DoesNotContain("_swatch", s);
    }

    [Fact]
    public void InheritanceDisabledStateKeepsReadableTextAndIconInSameState()
    {
        var s = Source();
        Assert.Contains("RoundedPanel(name + \"IconTile\"", s);
        Assert.DoesNotContain("Use property color", s);
        Assert.DoesNotContain("ButtonIcon(inheritLabel, \"Palette\")", s);
        Assert.DoesNotContain("new Color(0.1f, 0.1f, 0.1f", s);
    }

    [Fact]
    public void UnverifiedPlacementWarningSurvivesWithoutNormalFooterStatus()
    {
        var s = Source();
        Assert.DoesNotContain("selectedRoom != null && !selectedRoom.Verified", s);
        Assert.Contains("SetInlineState(_editError", s);
        Assert.DoesNotContain("string geometry =", s);
    }

    [Fact]
    public void MockupSizingFitsCollapsedAndAddsOnlyHiddenControlWhenExpanded()
    {
        Assert.Equal(74f, AppLayout.SelectorHeight);
        Assert.Equal(26f, AppLayout.SliderHeadingHeight);
        Assert.Equal(28f, AppLayout.SliderTrackHeight);
        Assert.Equal(58f, AppLayout.PreviewHeight);
        Assert.Equal(54f, AppLayout.ActionHeight);
        Assert.Equal(68f, AppLayout.PowerHeight);
        Assert.Equal(34f, AppLayout.AdvancedHeight);
        Assert.Equal(12f, AppLayout.Gap);
        Assert.Equal(4f, AppLayout.Padding);
        Assert.Equal(54f, AppLayout.SliderHeight);
        Assert.Equal(472f, AppLayout.ContentHeight(1));
        Assert.Equal(538f, AppLayout.ContentHeight(1, true));
        // 0.1.4 scroll region is 88.5% of the phone, not the retired 80% footer budget.
        Assert.True(AppLayout.ContentHeight(1) <= 750 * 0.885f);
    }
}

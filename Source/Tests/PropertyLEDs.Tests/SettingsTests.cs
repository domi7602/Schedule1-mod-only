using Xunit;
namespace PropertyLEDs.Tests;

public class SettingsTests
{
    [Fact]
    public void Default_settings_are_disabled_neutral_white()
    {
        var type = typeof(SettingsTests).Assembly.GetType("PropertyLEDs.Core.LEDSettings");
        Assert.NotNull(type);
        var value = Activator.CreateInstance(type!);
        Assert.Equal(false, type!.GetProperty("Enabled")!.GetValue(value));
        Assert.Equal(0f, type.GetProperty("Hue")!.GetValue(value));
        Assert.Equal(0f, type.GetProperty("Saturation")!.GetValue(value));
        Assert.Equal(1f, type.GetProperty("ColorIntensity")!.GetValue(value));
        Assert.Equal(1f, type.GetProperty("Brightness")!.GetValue(value));
    }
    [Fact]
    public void White_is_enabled_and_clones_are_independent()
    {
        var method = typeof(Core.LEDSettings).GetMethod("White");
        Assert.NotNull(method);
        var white = (Core.LEDSettings)method!.Invoke(null, null)!;
        Assert.True(white.Enabled);
        Assert.Equal(0, white.Saturation);
        Assert.Equal(1, white.ColorIntensity);
        Assert.Equal(1, white.Brightness);
        var cloneMethod = typeof(Core.LEDSettings).GetMethod("Clone");
        Assert.NotNull(cloneMethod);
        var clone = (Core.LEDSettings)cloneMethod!.Invoke(white, null)!;
        clone.Hue = 0.5f;
        Assert.Equal(0, white.Hue);
    }
    [Theory]
    [InlineData(float.NaN, float.PositiveInfinity, float.NegativeInfinity, float.NaN, 0, 0, 1, 1)]
    [InlineData(-2, 2, -1, 6, 0, 1, 0, 5)]
    [InlineData(2, -1, 2, -1, 1, 0, 1, 0)]
    [InlineData(0.4f, 0.7f, 0.8f, 3f, 0.4f, 0.7f, 0.8f, 3f)]
    public void Normalize_repairs_finite_ranges(float h, float s, float v, float b, float eh, float es, float ev, float eb)
    {
        var settings = new Core.LEDSettings { Hue = h, Saturation = s, ColorIntensity = v, Brightness = b };
        var normalize = typeof(Core.LEDSettings).GetMethod("Normalize");
        Assert.NotNull(normalize);
        normalize!.Invoke(settings, null);
        Assert.Equal(eh, settings.Hue);
        Assert.Equal(es, settings.Saturation);
        Assert.Equal(ev, settings.ColorIntensity);
        Assert.Equal(eb, settings.Brightness);
    }
    [Theory]
    [InlineData(0, 1, 1, 1, 0, 0)]
    [InlineData(1f / 3f, 1, 1, 0, 1, 0)]
    [InlineData(2f / 3f, 1, 1, 0, 0, 1)]
    [InlineData(0.5f, 1, 1, 0, 1, 1)]
    [InlineData(1, 1, 1, 1, 0, 0)]
    [InlineData(0.7f, 0, 0.6f, 0.6f, 0.6f, 0.6f)]
    [InlineData(0, 0.5f, 0.8f, 0.8f, 0.4f, 0.4f)]
    [InlineData(float.NaN, float.NaN, float.NaN, 1, 1, 1)]
    public void Rgb_is_normalized_hsv_independent_of_enabled_and_brightness(float h, float s, float v, float r, float g, float b)
    {
        var settings = new Core.LEDSettings { Hue = h, Saturation = s, ColorIntensity = v, Brightness = 5 };
        var method = typeof(Core.LEDSettings).GetMethod("ToRgb");
        Assert.NotNull(method);
        var rgb = method!.Invoke(settings, null)!;
        foreach (var item in new[] { ("R", r), ("G", g), ("B", b) })
            Assert.InRange((float)rgb.GetType().GetProperty(item.Item1)!.GetValue(rgb)!, item.Item2 - 0.00001f, item.Item2 + 0.00001f);
        Assert.Equal(h, settings.Hue);
    }
    [Fact]
    public void Editing_colour_switches_the_lights_on_but_zero_brightness_never_does()
    {
        var method = typeof(Core.LEDSettings).GetMethod("EnableForEdit");
        Assert.NotNull(method);
        var edited = new Core.LEDSettings { Enabled = false, Brightness = 2 };
        method!.Invoke(edited, null);
        Assert.True(edited.Enabled);
        var dark = new Core.LEDSettings { Enabled = false, Brightness = 0 };
        method.Invoke(dark, null);
        Assert.False(dark.Enabled);
        var notFinite = new Core.LEDSettings { Enabled = false, Brightness = float.NaN };
        method.Invoke(notFinite, null);
        Assert.False(notFinite.Enabled);
        var explicitOn = new Core.LEDSettings { Enabled = true, Brightness = 0 };
        method.Invoke(explicitOn, null);
        Assert.True(explicitOn.Enabled);
    }
    // NEXT TEST
}

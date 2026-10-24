using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class GoldenHourTargetsTests
{
    [Theory]
    [InlineData("sun")]
    [InlineData("skyGlow")]
    [InlineData("ambient")]
    public void All_tints_are_warm_ordered_r_greater_g_greater_b(string key)
    {
        var tint = Tint(key);
        Assert.True(tint.R > tint.G, $"{key}: R ({tint.R}) must exceed G ({tint.G}) for a warm bias");
        Assert.True(tint.G > tint.B, $"{key}: G ({tint.G}) must exceed B ({tint.B}) for a warm bias");
    }

    [Theory]
    [InlineData("sun")]
    [InlineData("skyGlow")]
    [InlineData("ambient")]
    public void All_tints_are_finite_and_bounded(string key)
    {
        var tint = Tint(key);
        Assert.True(tint.IsFinite, $"{key} must be finite");
        Assert.True(tint.R >= 0.90f && tint.G >= 0.90f && tint.B >= 0.90f,
            $"{key}: no channel below 0.90 — a warm shift, not a gold wash");
        Assert.True(tint.R <= 1.25f && tint.G <= 1.25f && tint.B <= 1.25f,
            $"{key}: no channel above 1.25 — bias, not wash-out");
    }

    [Fact]
    public void Accepted_mild_recipe_is_preserved()
    {
        Assert.Equal(new Argb(1.06f, 1.00f, 0.94f, 1f), GoldenHourTargets.SunLightTint);
        Assert.Equal(new Argb(1.04f, 0.97f, 0.90f, 1f), GoldenHourTargets.SkySunGlowTint);
        Assert.Equal(new Argb(1.03f, 0.995f, 0.96f, 1f), GoldenHourTargets.AmbientTint);
        Assert.True(GoldenHourTargets.AmbientTint.R < GoldenHourTargets.SunLightTint.R,
            "ambient must shift less than the sun tint");
    }

    private static Argb Tint(string key) => key switch
    {
        "sun" => GoldenHourTargets.SunLightTint,
        "skyGlow" => GoldenHourTargets.SkySunGlowTint,
        "ambient" => GoldenHourTargets.AmbientTint,
        _ => throw new System.ArgumentException(key),
    };
}

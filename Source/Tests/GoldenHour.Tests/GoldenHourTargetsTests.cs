using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class GoldenHourTargetsTests
{
    [Theory]
    [InlineData("sun")]
    [InlineData("skyGlow")]
    [InlineData("ambient")]
    [InlineData("fog")]
    [InlineData("volume")]
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
    [InlineData("fog")]
    [InlineData("volume")]
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
    public void Fog_bias_is_strongest_ambient_is_gentlest()
    {
        Assert.True(GoldenHourTargets.FogTint.B < GoldenHourTargets.SunLightTint.B,
            "fog blue reduction must be at least as strong as sun");
        Assert.True(GoldenHourTargets.AmbientTint.R < GoldenHourTargets.SunLightTint.R,
            "ambient must shift less than the sun tint");
    }

    [Fact]
    public void Diagnostic_tint_is_finite_and_deliberately_obvious()
    {
        var tint = GoldenHourTargets.DiagnosticVolumeTint;
        Assert.True(tint.IsFinite, "diagnostic tint must be finite");
        Assert.True(tint.R >= 1.5f || tint.B <= 0.35f,
            "diagnostic tint must sit clearly outside the regular bounds so the URP path is unmissable");
        Assert.True(tint.R > GoldenHourTargets.VolumeTint.R,
            "diagnostic tint must be stronger than the production volume tint");
        Assert.True(tint.B < GoldenHourTargets.VolumeTint.B,
            "diagnostic tint must be more colour-shifted than the production volume tint");
    }

    private static Argb Tint(string key) => key switch
    {
        "sun" => GoldenHourTargets.SunLightTint,
        "skyGlow" => GoldenHourTargets.SkySunGlowTint,
        "ambient" => GoldenHourTargets.AmbientTint,
        "fog" => GoldenHourTargets.FogTint,
        "volume" => GoldenHourTargets.VolumeTint,
        _ => throw new System.ArgumentException(key),
    };
}

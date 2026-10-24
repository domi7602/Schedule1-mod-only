using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class BlendPolicyTests
{
    [Theory]
    [InlineData(double.NaN, 0.0)]
    [InlineData(double.PositiveInfinity, 0.0)]
    [InlineData(double.NegativeInfinity, 0.0)]
    [InlineData(-3.0, 0.0)]
    [InlineData(0.4, 0.4)]
    [InlineData(5.0, 1.0)]
    public void Warmth_is_finite_and_clamped(double requested, double expected) =>
        Assert.Equal(expected, BlendPolicy.ClampStrength(requested));

    [Fact]
    public void Finite_HDR_channel_never_overflows_the_native_float()
    {
        var original = new Argb(float.MaxValue, 1f, 1f, 1f);
        Assert.True(BlendPolicy.Blend(original, GoldenHourTargets.SunLightTint, 1.0).IsFinite);
    }

    [Fact]
    public void Blend_at_full_strength_multiplies_original_by_tint()
    {
        var result = BlendPolicy.Blend(new Argb(1f, 1f, 1f, 1f), new Argb(1.15f, 0.97f, 0.72f, 1f), 1.0);
        Assert.Equal(1.15f, result.R, 4);
        Assert.Equal(0.97f, result.G, 4);
        Assert.Equal(0.72f, result.B, 4);
        Assert.Equal(1f, result.A, 4);
    }

    [Fact]
    public void Blend_at_zero_strength_is_identity()
    {
        var original = new Argb(0.3f, 0.6f, 0.9f, 1f);
        var result = BlendPolicy.Blend(original, new Argb(1.15f, 0.97f, 0.72f, 1f), 0.0);
        Assert.Equal(original, result);
    }

    [Fact]
    public void Blend_scales_original_magnitude_and_preserves_alpha()
    {
        var result = BlendPolicy.Blend(new Argb(0.5f, 0.5f, 0.5f, 0.8f), new Argb(1.2f, 1.2f, 1.2f, 0.5f), 1.0);
        Assert.Equal(0.6f, result.R, 4);
        Assert.Equal(0.6f, result.G, 4);
        Assert.Equal(0.6f, result.B, 4);
        Assert.Equal(0.8f, result.A, 4);
    }

    [Fact]
    public void Blend_clamps_strength_into_unit_range()
    {
        var tint = new Argb(1.2f, 1.2f, 1.2f, 1f);
        var over = BlendPolicy.Blend(new Argb(1f, 1f, 1f, 1f), tint, 1.7);
        var under = BlendPolicy.Blend(new Argb(1f, 1f, 1f, 1f), tint, -0.5);
        Assert.Equal(1.2f, over.R, 4);
        Assert.Equal(1f, under.R, 4);
    }

    [Fact]
    public void Blend_interpolates_linearly_between_identity_and_full_bias()
    {
        var original = new Argb(1f, 1f, 1f, 1f);
        var half = BlendPolicy.Blend(original, new Argb(1.2f, 1.2f, 1.2f, 1f), 0.5);
        Assert.Equal(1.1f, half.R, 4);
    }

    [Fact]
    public void Blend_nonfinite_original_channel_falls_back_to_target()
    {
        var tint = new Argb(1.15f, 0.97f, 0.72f, 1f);
        var result = BlendPolicy.Blend(new Argb(float.NaN, 1f, 1f, 1f), tint, 1.0);
        Assert.Equal(1.15f, result.R, 4);
        Assert.Equal(0.97f, result.G, 4);
    }

    [Fact]
    public void Blend_nonfinite_tint_channel_acts_as_neutral_one()
    {
        var tint = new Argb(float.PositiveInfinity, 1f, 1f, 1f);
        var result = BlendPolicy.Blend(new Argb(0.5f, 0.5f, 0.5f, 1f), tint, 1.0);
        Assert.Equal(0.5f, result.R, 4);
        Assert.Equal(0.5f, result.G, 4);
    }

    [Fact]
    public void Blend_result_is_always_finite()
    {
        var result = BlendPolicy.Blend(new Argb(float.NaN, 0f, 0f, 1f), new Argb(1.15f, 0.97f, 0.72f, 1f), 1.0);
        Assert.True(result.IsFinite);
    }
}

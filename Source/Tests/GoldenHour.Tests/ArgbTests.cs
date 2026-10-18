using Xunit;

namespace GoldenHour.Tests;

/// <summary>Value-comparison helper used to skip redundant writes (0.1.1 hardening).</summary>
public class ArgbTests
{
    [Fact]
    public void IsCloseTo_identical_values_true()
    {
        var a = new Argb(0.5f, 0.25f, 0.75f, 1f);
        Assert.True(a.IsCloseTo(a));
    }

    [Fact]
    public void IsCloseTo_within_default_tolerance_true()
    {
        var a = new Argb(0.5f, 0.25f, 0.75f, 1f);
        var b = new Argb(0.5004f, 0.2497f, 0.7503f, 1f);
        Assert.True(a.IsCloseTo(b));
    }

    [Fact]
    public void IsCloseTo_outside_tolerance_false()
    {
        var a = new Argb(0.5f, 0.25f, 0.75f, 1f);
        var b = new Argb(0.51f, 0.25f, 0.75f, 1f);
        Assert.False(a.IsCloseTo(b));
    }

    [Fact]
    public void IsCloseTo_non_finite_on_either_side_false()
    {
        var a = new Argb(0.5f, 0.25f, 0.75f, 1f);
        var nan = new Argb(float.NaN, 0.25f, 0.75f, 1f);
        Assert.False(a.IsCloseTo(nan));
        Assert.False(nan.IsCloseTo(a));
    }

    [Fact]
    public void IsCloseTo_honours_explicit_tolerance()
    {
        var a = new Argb(0.5f, 0.5f, 0.5f, 1f);
        var b = new Argb(0.51f, 0.5f, 0.5f, 1f);
        Assert.True(a.IsCloseTo(b, 0.02f));
        Assert.False(a.IsCloseTo(b, 0.005f));
    }
}

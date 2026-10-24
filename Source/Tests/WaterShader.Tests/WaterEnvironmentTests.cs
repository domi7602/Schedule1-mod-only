using WaterShader;
using Xunit;

namespace WaterShader.Tests;

public sealed class WaterEnvironmentTests
{
    [Theory]
    [InlineData(0f, 0f, 0f, 1f)]
    [InlineData(0.001f, 0.003f, 0.008f, 1f)]
    [InlineData(1.5f, 2.3f, 4f, 0.7f)]
    public void ValidNativeNightAndHdrColorsRemainAuthoritative(float r, float g, float b, float a)
    {
        var native = (r, g, b, a);
        Assert.Equal(native, WaterEnvironmentPolicy.SelectColor(native, (1f, 1f, 1f, 1f))!.Value);
    }

    [Theory]
    [InlineData(float.NaN, 1f, 1f, 1f)]
    [InlineData(1f, float.PositiveInfinity, 1f, 1f)]
    [InlineData(1f, 1f, float.NegativeInfinity, 1f)]
    [InlineData(1f, 1f, 1f, float.NaN)]
    public void AnyNonfiniteNativeChannelUsesTheFiniteAmbientFallback(float r, float g, float b, float a)
    {
        var ambient = (0.01f, 0.02f, 0.03f, 1f);
        Assert.Equal(ambient, WaterEnvironmentPolicy.SelectColor((r, g, b, a), ambient)!.Value);
    }

    [Fact]
    public void MissingNativeUsesAmbientButMissingOrInvalidBothSuppliesNoInventedPalette()
    {
        var ambient = (0f, 0f, 0f, 1f);
        Assert.Equal(ambient, WaterEnvironmentPolicy.SelectColor(null, ambient)!.Value);
        Assert.Null(WaterEnvironmentPolicy.SelectColor(null, null));
        Assert.Null(WaterEnvironmentPolicy.SelectColor((float.NaN, 1f, 1f, 1f), (1f, 1f, float.NaN, 1f)));
    }

    [Theory]
    [InlineData(-1f, 0f)]
    [InlineData(0f, 0f)]
    [InlineData(0.37f, 0.37f)]
    [InlineData(2f, 1f)]
    [InlineData(float.NaN, 0f)]
    [InlineData(float.PositiveInfinity, 0f)]
    public void WindIsAFiniteBoundedNativeWeight(float input, float expected)
        => Assert.Equal(expected, WaterEnvironmentPolicy.WindActivity(input));
}

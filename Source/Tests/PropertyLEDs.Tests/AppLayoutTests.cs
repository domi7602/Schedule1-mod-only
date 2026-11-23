using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

public sealed class AppLayoutTests
{
    [Theory]
    [InlineData(750f, 1f)]
    [InlineData(1201f, 1.6013333f)]
    [InlineData(1500f, 2f)]
    public void DirectFourSliderStackFitsOriginalPhoneViewport(float phoneHeight, float scale)
    {
        float height = AppLayout.ContentHeight(scale);
        Assert.Equal(426f * scale, height, 2);
        Assert.True(height <= phoneHeight * 0.885f, $"content {height} exceeds scroll viewport {phoneHeight * 0.885f}");
    }
}

using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

public sealed class InlineStateTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Unavailable during loading / scene transition.")]
    [InlineData("No live property anchor available.")]
    public void OrdinaryMessagesStayNeutral(string? message)
    {
        var tone = AppLayout.InlineColor(message);
        Assert.Equal(0.80f, tone.R);
        Assert.Equal(0.85f, tone.G);
        Assert.Equal(0.90f, tone.B);
        Assert.Equal(1f, tone.A);
    }

    [Fact]
    public void UnverifiedPlacementIsWarmYellow()
    {
        var tone = AppLayout.InlineColor("Unverified: LEDs not placed");
        Assert.Equal(0.95f, tone.R);
        Assert.Equal(0.78f, tone.G);
        Assert.Equal(0.28f, tone.B);
        Assert.Equal(1f, tone.A);
    }

    [Theory]
    [InlineData("Edit failed: boom")]
    [InlineData("Inheritance edit failed: boom")]
    [InlineData("LED phone refresh failed: boom")]
    public void FailuresAreRed(string message)
    {
        var tone = AppLayout.InlineColor(message);
        Assert.Equal(0.91f, tone.R);
        Assert.Equal(0.29f, tone.G);
        Assert.Equal(0.27f, tone.B);
        Assert.Equal(1f, tone.A);
    }
}

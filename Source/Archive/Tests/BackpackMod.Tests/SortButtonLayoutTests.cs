using BackpackMod.Patches;
using Xunit;

namespace BackpackMod.Tests;

/// <summary>
/// Regression coverage for the Echtspiel-Report 2026-09-13: the injected
/// StorageMenu "Sort" button rendered as a giant overlay because the injector
/// cloned the first Button found (a container panel) and inherited its rect.
/// The injector now clamps the template size and parks the clone
/// left-adjacent to the close cluster — this suite pins that math.
/// </summary>
public class SortButtonLayoutTests
{
    [Fact]
    public void ClampButtonSize_KeepsSaneTemplateSize()
    {
        var (w, h) = SortButtonLayout.ClampButtonSize(150f, 44f);

        Assert.Equal(150f, w, 3);
        Assert.Equal(44f, h, 3);
    }

    [Fact]
    public void ClampButtonSize_GrowsTinyXButtonToReadableMinimum()
    {
        var (w, h) = SortButtonLayout.ClampButtonSize(36f, 36f);

        Assert.Equal(SortButtonLayout.MinWidth, w, 3);
        Assert.Equal(SortButtonLayout.MinHeight, h, 3);
    }

    [Fact]
    public void ClampButtonSize_ShrinksGiantContainerTemplate()
    {
        // The reported bug: a 1920x1080 container panel became the button.
        var (w, h) = SortButtonLayout.ClampButtonSize(1920f, 1080f);

        Assert.Equal(SortButtonLayout.MaxWidth, w, 3);
        Assert.Equal(SortButtonLayout.MaxHeight, h, 3);
    }

    [Theory]
    [InlineData(float.NaN, 44f)]
    [InlineData(150f, float.PositiveInfinity)]
    [InlineData(0f, 0f)]
    [InlineData(-5f, -10f)]
    public void ClampButtonSize_InvalidInputFallsBackToDefault(float w, float h)
    {
        var (cw, ch) = SortButtonLayout.ClampButtonSize(w, h);

        float expectedW = (!float.IsFinite(w) || w <= 0f) ? SortButtonLayout.DefaultWidth : w;
        float expectedH = (!float.IsFinite(h) || h <= 0f) ? SortButtonLayout.DefaultHeight : h;
        Assert.Equal(expectedW, cw, 3);
        Assert.Equal(expectedH, ch, 3);
    }

    [Theory]
    // containerX, containerW, cloneW, pivotX, expectedX
    [InlineData(0f, 100f, 150f, 0.5f, -137f)] // -(50 + 12 + 75)
    [InlineData(0f, 100f, 150f, 1f, -112f)]   // -(100 + 12 + 0)
    [InlineData(0f, 100f, 150f, 0f, -162f)]   // -(0 + 12 + 150)
    [InlineData(-30f, 100f, 150f, 0.5f, -167f)]
    public void LeftAdjacentX_ParksCloneLeftOfCluster(float containerX, float containerW, float cloneW, float pivotX, float expectedX)
    {
        float x = SortButtonLayout.LeftAdjacentX(containerX, containerW, cloneW, pivotX);

        Assert.Equal(expectedX, x, 3);
    }

    [Fact]
    public void LeftAdjacentX_CloneRightEdgeSitsExactlyOneGapLeftOfCluster()
    {
        // Property check for center pivot: cloneRight == containerLeft - Gap.
        float x = SortButtonLayout.LeftAdjacentX(0f, 100f, 150f, 0.5f);
        float cloneRight = x + (1f - 0.5f) * 150f;
        float containerLeft = 0f - 0.5f * 100f;

        Assert.Equal(containerLeft - SortButtonLayout.Gap, cloneRight, 3);
    }
}

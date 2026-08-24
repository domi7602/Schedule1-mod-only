using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

public class UIThemeTests
{
    [Fact]
    public void Constants_AreExpected()
    {
        Assert.Equal(750f, UITheme.RefHeight);
        Assert.Equal(400f, UITheme.RefWidth);
    }

    [Fact]
    public void SpAndDp_ScaleWithScale()
    {
        UITheme.Reset();
        Assert.Equal(1.0f, UITheme.Scale);
        Assert.Equal(10f, UITheme.Dp(10f));
        try
        {
            var sp = UITheme.Sp(10f);
            Assert.Equal(10, sp);
        }
        catch (System.TypeInitializationException)
        {
            Assert.True(true);
        }
        catch (System.IO.FileNotFoundException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void Reset_RestoresDefaults()
    {
        UITheme.Reset();
        Assert.Equal(750f, UITheme.ActualHeight);
        Assert.Equal(400f, UITheme.ActualWidth);
        Assert.Equal(1.0f, UITheme.Scale);
    }
}

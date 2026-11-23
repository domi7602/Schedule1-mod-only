using MoonRocks;
using Xunit;

namespace MoonRocks.Tests;

public sealed class MoonMathTests
{
    [Fact]
    public void LandingPadCenter_IsFlat()
    {
        Assert.Equal(0f, MoonMath.SampleTerrainHeight(0f, 0f));
        Assert.Equal(0f, MoonMath.SampleTerrainHeight(8f, -8f));
        Assert.Equal(0f, MoonMath.SampleTerrainHeight(-15f, 0f));
    }

    [Fact]
    public void OuterTerrain_IsFiniteAndNonFlat()
    {
        float sumAbs = 0f;
        for (int x = -100; x <= 100; x += 20)
        {
            for (int z = -100; z <= 100; z += 20)
            {
                float h = MoonMath.SampleTerrainHeight(x, z);
                Assert.False(float.IsNaN(h));
                Assert.False(float.IsInfinity(h));
                sumAbs += MathF.Abs(h);
            }
        }
        Assert.True(sumAbs > 20f);
    }

    [Fact]
    public void FlightCurves_AreMonotonicAndBounded()
    {
        float prevAscent = -1f;
        float prevDescent = -1f;
        float prevDark = -1f;

        for (int i = 0; i <= 20; i++)
        {
            float t = i / 20f;
            float asc = MoonMath.ComputeAscentProgress(t);
            float desc = MoonMath.ComputeDescentProgress(t);
            float dark = MoonMath.ComputeSpaceDarkness(t);

            Assert.InRange(asc, 0f, 1f);
            Assert.InRange(desc, 0f, 1f);
            Assert.InRange(dark, 0f, 1f);

            Assert.True(asc >= prevAscent);
            Assert.True(desc >= prevDescent);
            Assert.True(dark >= prevDark);

            prevAscent = asc;
            prevDescent = desc;
            prevDark = dark;
        }

        Assert.Equal(0f, MoonMath.ComputeAscentProgress(0f));
        Assert.Equal(1f, MoonMath.ComputeAscentProgress(1f));
        Assert.Equal(0f, MoonMath.ComputeDescentProgress(0f));
        Assert.Equal(1f, MoonMath.ComputeDescentProgress(1f));
        Assert.Equal(0f, MoonMath.ComputeSpaceDarkness(0f));
        Assert.Equal(1f, MoonMath.ComputeSpaceDarkness(1f));
    }
}

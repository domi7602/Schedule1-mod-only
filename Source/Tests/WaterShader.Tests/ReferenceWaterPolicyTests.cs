using WaterShader;
using Xunit;

namespace WaterShader.Tests;

public sealed class ReferenceWaterPolicyTests
{
    [Fact]
    public void ReflectionMatrixMirrorsAcrossHorizontalPlane()
    {
        float[] m = ReferenceWaterPolicy.ReflectionMatrix(5f);
        Assert.Equal(16, m.Length);
        Assert.Equal(1f, m[0]);
        Assert.Equal(-1f, m[5]);
        Assert.Equal(10f, m[7]); // 2 * waterY
        Assert.Equal(1f, m[10]);
        Assert.Equal(1f, m[15]);
        foreach (int i in new[] { 1, 2, 3, 4, 6, 8, 9, 11, 12, 13, 14 })
            Assert.Equal(0f, m[i]);
    }

    [Fact]
    public void ReflectionMatrixOffsetUsesWaterPlane()
    {
        Assert.Equal(0f, ReferenceWaterPolicy.ReflectionMatrix(0f)[7]);
        Assert.Equal(-4f, ReferenceWaterPolicy.ReflectionMatrix(-2f)[7]);
    }

    [Fact]
    public void MirrorPointReflectsAcrossWaterPlane()
    {
        var (x, y, z) = ReferenceWaterPolicy.MirrorPoint(5f, 4f, 9f, -2f);
        Assert.Equal(4f, x);
        Assert.Equal(1f, y); // 2*5 - 9
        Assert.Equal(-2f, z);
    }

    [Fact]
    public void MirrorPointAgreesWithReflectionMatrix()
    {
        const float waterY = 3.5f;
        float[] m = ReferenceWaterPolicy.ReflectionMatrix(waterY);
        var (x, y, z) = ReferenceWaterPolicy.MirrorPoint(waterY, 4f, 9f, -2f);

        float mx = m[0] * 4f + m[1] * 9f + m[2] * -2f + m[3];
        float my = m[4] * 4f + m[5] * 9f + m[6] * -2f + m[7];
        float mz = m[8] * 4f + m[9] * 9f + m[10] * -2f + m[11];

        Assert.Equal(mx, x);
        Assert.Equal(my, y);
        Assert.Equal(mz, z);
    }

    [Fact]
    public void MirrorDirectionFlipsOnlyY()
    {
        var (x, y, z) = ReferenceWaterPolicy.MirrorDirection(1f, -2f, 3f);
        Assert.Equal(1f, x);
        Assert.Equal(2f, y);
        Assert.Equal(3f, z);
    }

    [Theory]
    [InlineData(0, 0, 512, 16, 16)]
    [InlineData(64, 64, 512, 64, 64)]
    [InlineData(4096, 4096, 512, 512, 512)]
    [InlineData(4096, 32, 1024, 1024, 32)]
    [InlineData(100, 100, 0, 100, 100)]
    [InlineData(-8, 2048, 256, 16, 256)]
    public void ReflectionSizeClampsToBudget(int w, int h, int max, int expectedW, int expectedH)
    {
        var (rw, rh) = ReferenceWaterPolicy.ReflectionSize(w, h, max);
        Assert.Equal(expectedW, rw);
        Assert.Equal(expectedH, rh);
    }

    [Theory]
    [InlineData(true, false, false, false, false, true, true, true)]
    [InlineData(false, false, false, false, false, true, true, false)]
    [InlineData(true, true, false, false, false, true, true, false)]
    [InlineData(true, false, true, false, false, true, true, false)]
    [InlineData(true, false, false, true, false, true, true, false)]
    [InlineData(true, false, false, false, true, true, true, false)]
    [InlineData(true, false, false, false, false, false, true, false)]
    [InlineData(true, false, false, false, false, true, false, false)]
    public void ShouldRenderReflectionGatesEveryBlockingCondition(
        bool gate, bool owned, bool reflection, bool overlay, bool preview,
        bool game, bool bound, bool expected)
    {
        Assert.Equal(expected, ReferenceWaterPolicy.ShouldRenderReflection(
            gate, owned, reflection, overlay, preview, game, bound));
    }

    [Theory]
    [InlineData(true, true, true, true)]
    [InlineData(false, true, true, false)]
    [InlineData(true, false, true, false)]
    [InlineData(true, true, false, false)]
    public void CanApplyReferenceRequiresAllPrerequisites(
        bool bundleLoaded, bool shaderSupported, bool hasProperties, bool expected)
    {
        Assert.Equal(expected, ReferenceWaterPolicy.CanApplyReference(
            bundleLoaded, shaderSupported, hasProperties));
    }

    [Fact]
    public void RequiredShaderPropertiesCoverTheReferenceShaderContract()
    {
        string[] names = ReferenceWaterPolicy.RequiredShaderProperties;

        // Exact set: 17 declared shader properties. _ReflectionTex_ST is a
        // generated texture-scale block and _ReflectionVP/_ReflectionAvailable are
        // CBUFFER-only (Material.HasProperty is false for them, measured via
        // BuildReference.ProbeBinding), so none of the three belongs in the
        // HasProperty-gated contract.
        string[] expected =
        {
            "_ShallowTint", "_DeepTint", "_Absorption",
            "_ReflectionTex", "_ReflectionStrength", "_ReflectionSharpness",
            "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength",
            "_SunStrength", "_SunSharpness",
            "_FoamColor", "_FoamDepth", "_FoamStrength",
            "_SkyColor", "_AmbientColor",
        };

        Assert.Equal(expected.Length, names.Length);
        Assert.Equal(expected.OrderBy(n => n), names.OrderBy(n => n));
        Assert.DoesNotContain("_ReflectionVP", names);
        Assert.DoesNotContain("_ReflectionAvailable", names);
        Assert.DoesNotContain("_ReflectionTex_ST", names);
        Assert.DoesNotContain("_WaveHeight", names);
        Assert.DoesNotContain("_ShallowColor", names);
    }

    [Fact]
    public void MissingRequiredPropertiesNamesEveryAbsentContractProperty()
    {
        string[] missing = ReferenceWaterPolicy.MissingRequiredProperties(
            name => name == "_ReflectionTex" || name == "_ShallowTint");

        Assert.DoesNotContain("_ReflectionTex", missing);
        Assert.DoesNotContain("_ShallowTint", missing);
        Assert.Contains("_DeepTint", missing);
        Assert.Contains("_ReflectionStrength", missing);
        Assert.Equal(ReferenceWaterPolicy.RequiredShaderProperties.Length - 2, missing.Length);
    }

    [Fact]
    public void MissingRequiredPropertiesRejectsNullProbe()
    {
        Assert.Throws<ArgumentNullException>(() => ReferenceWaterPolicy.MissingRequiredProperties(null!));
    }

    [Fact]
    public void MissingRequiredPropertiesIsEmptyWhenContractIsComplete()
    {
        Assert.Empty(ReferenceWaterPolicy.MissingRequiredProperties(_ => true));
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    public void ShouldBindReflectionRequiresBoundMaterialAndReadyTexture(
        bool bound, bool textureReady, bool expected)
    {
        Assert.Equal(expected, ReferenceWaterPolicy.ShouldBindReflection(bound, textureReady));
    }

    [Theory]
    [InlineData(1920, 1080, 512, 512, 288)]
    [InlineData(1280, 720, 512, 512, 288)]
    [InlineData(3840, 2160, 512, 512, 288)]
    [InlineData(100, 100, 512, 100, 100)]
    [InlineData(0, 1080, 512, 512, 512)]
    [InlineData(1080, 1920, 512, 288, 512)]
    [InlineData(64, 64, 20, 64, 64)]
    [InlineData(1024, 512, 1024, 1024, 512)]
    public void ReflectionSizeForAspectPreservesFrameShape(
        int sourceW, int sourceH, int max, int expectedW, int expectedH)
    {
        var (w, h) = ReferenceWaterPolicy.ReflectionSizeForAspect(sourceW, sourceH, max);
        Assert.Equal(expectedW, w);
        Assert.Equal(expectedH, h);
    }

    [Fact]
    public void ReflectionSizeForAspectNeverExceedsBudgetOnTheLongSide()
    {
        var (w, h) = ReferenceWaterPolicy.ReflectionSizeForAspect(7680, 4320, 256);
        Assert.Equal(256, Math.Max(w, h));
    }
}

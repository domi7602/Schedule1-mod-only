using System.Text.RegularExpressions;
using WaterShader;
using Xunit;

namespace WaterShader.Tests;

public sealed class ReferenceWaterPolicyTests
{
    // Reflection matrix/camera/RT tests were retired with those unused APIs.
    // The retained contract tests exercise the environment-only shader prerequisites.
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
    public void RequiredPropertiesMatchTheAuthoredShaderContract()
    {
        string shader = ReferenceWaterWiringTests.RepoFile("Source/Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader");
        var declarations = Regex.Matches(shader, @"(?m)^\s*(_\w+)\s*\(\s*""")
            .Select(match => match.Groups[1].Value).OrderBy(name => name);
        Assert.Equal(declarations, ReferenceWaterPolicy.RequiredShaderProperties.OrderBy(name => name));
        Assert.Contains("_SurfaceOpacity", ReferenceWaterPolicy.RequiredShaderProperties);
        Assert.Contains("_WindActivity", ReferenceWaterPolicy.RequiredShaderProperties);
        Assert.DoesNotContain("_ReflectionTex", ReferenceWaterPolicy.RequiredShaderProperties);
    }

    [Fact]
    public void MissingRequiredPropertiesNamesEveryAbsentContractProperty()
    {
        string[] missing = ReferenceWaterPolicy.MissingRequiredProperties(
            name => name == "_SurfaceOpacity" || name == "_ShallowTint");
        Assert.DoesNotContain("_SurfaceOpacity", missing);
        Assert.DoesNotContain("_ShallowTint", missing);
        Assert.Contains("_DeepTint", missing);
        Assert.Contains("_ReflectionStrength", missing);
        Assert.Equal(ReferenceWaterPolicy.RequiredShaderProperties.Length - 2, missing.Length);
    }

    [Fact]
    public void MissingRequiredPropertiesRejectsNullProbe()
        => Assert.Throws<ArgumentNullException>(() => ReferenceWaterPolicy.MissingRequiredProperties(null!));

    [Fact]
    public void MissingRequiredPropertiesIsEmptyWhenContractIsComplete()
        => Assert.Empty(ReferenceWaterPolicy.MissingRequiredProperties(_ => true));
}

using Xunit;

namespace WaterShader.Tests;

/// <summary>Small source-contract supplement; no native/render/GPU execution is claimed.</summary>
public sealed class ReferenceWaterWiringTests
{
    internal static string RepoFile(string path)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        Assert.NotNull(directory);
        return File.ReadAllText(Path.Combine(directory!.FullName, path));
    }

    [Fact]
    public void EnvironmentOnlyPathHasNoOwnedCameraTargetRenderHookOrGlobalWrites()
    {
        string runtime = RepoFile("Source/Mods/WorldLook/src/ReferenceWaterRuntime.cs");
        string shader = RepoFile("Source/Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader");
        foreach (string forbidden in new[] { "new GameObject", "AddComponent<Camera>", "RenderTexture", "RenderSingleCamera", "beginCameraRendering", "Shader.SetGlobal", "FindObjectsOfType", "GetComponentsInChildren" })
            Assert.DoesNotContain(forbidden, runtime);
        Assert.DoesNotContain("_ReflectionTex", shader);
        Assert.DoesNotContain("_ReflectionVP", shader);
        Assert.Contains("WorldLook.ShaderAssets.LoadShader", runtime);
        Assert.Contains("WaterShaderAssignment.AssignConfirmed", runtime);
        Assert.Contains("EnvironmentRefreshSeconds = 0.2", runtime);
        Assert.Contains("InputRequirementsRefreshSeconds = 0.5", runtime);
    }

    [Fact]
    public void EnvironmentReadsNativeSkyAndWeatherWithoutCustomSkyDependency()
    {
        string environment = RepoFile("Source/Mods/WorldLook/src/WaterEnvironment.cs");
        string runtime = RepoFile("Source/Mods/WorldLook/src/ReferenceWaterRuntime.cs");
        Assert.Contains("manager._currentSkyState", environment);
        Assert.Contains("state.SkyUpperColor", environment);
        Assert.Contains("state.SkyMiddleColor", environment);
        Assert.Contains("S1API.Weather.WeatherManager.Current", environment);
        Assert.Contains("weather.Value.Windy", environment);
        Assert.Contains("WaterEnvironmentPolicy.SelectColor", environment);
        Assert.Contains("WaterEnvironment.Read(", runtime);
        Assert.DoesNotContain("WorldLookEnvironment", environment + runtime);
        Assert.DoesNotContain("RenderSettings.skybox =", environment + runtime);
    }

    [Fact]
    public void FilteredSurfaceHighlightStillUsesTheNativeMainLightAndShadowPath()
    {
        // Integration contract only. HLSL compilation and pixel/motion checks belong
        // to the shader build/render verification, not a duplicated CPU noise model.
        string shader = RepoFile("Source/Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader");
        string fragment = shader[shader.IndexOf("half4 frag(", StringComparison.Ordinal)..];
        Assert.Contains("GetMainLight(TransformWorldToShadowCoord(IN.positionWS))", fragment);
        Assert.Contains("SafeWaterDirection(mainLight.direction)", fragment);
        Assert.Contains("SafeWaterDirection(lightDir + viewDir)", fragment);
        Assert.Contains("FilteredSunLobe(normalWS, halfDir, unresolvedVariance)", fragment);
        Assert.Contains("lerp(mainLight.shadowAttenuation, 1.0, GetMainLightShadowFade(IN.positionWS))", fragment);
        Assert.Contains("mainLight.distanceAttenuation * shadow", fragment);
        Assert.Contains("mainLight.color * spec * attenuation * 0.6", fragment);
    }
}

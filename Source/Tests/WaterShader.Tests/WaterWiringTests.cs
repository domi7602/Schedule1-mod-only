using Xunit;

namespace WaterShader.Tests;

public sealed class WaterWiringTests
{
    private static string Source()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        return File.ReadAllText(Path.Combine(directory!.FullName, "Source/Mods/WorldLook/src/Water/WaterShaderMod.cs"));
    }

    [Fact]
    public void RuntimeUsesTargetedLookupAndTestedSessionNotBulkEnumeration()
    {
        string source = Source();
        Assert.DoesNotContain("FindObjectsOfTypeAll", source);
        Assert.DoesNotContain("GetComponentsInChildren", source);
        Assert.DoesNotContain("GameObject.Find", source);
        Assert.Contains("WaterSession<MeshRenderer, Material>", source);
        Assert.Contains("Map.instance", source);
        Assert.Contains("Find(\"StylizedWater2_Ocean\")", source);
        Assert.Contains("scene.name != \"Main\"", source);
        Assert.Contains("Alive(template.shader)", source);
        Assert.DoesNotContain("IndexOf(\"Ocean\"", source);
    }

    [Fact]
    public void RuntimeUsesSharedSlotAndLifecycleCleanup()
    {
        string source = Source();
        Assert.DoesNotContain("renderer.material =", source);
        Assert.Contains("target.sharedMaterial = material", source);
        Assert.Contains("OnSceneWasUnloaded", source);
        Assert.Contains("OnDeinitializeMelon", source);
        Assert.Contains("_session.Stop()", source);
        Assert.Contains("Stopwatch", source);
    }

    [Fact]
    public void RuntimeLogsAppliedRecipeInConfigurationAndDoesNotAddRenderingFeatures()
    {
        string source = Source();
        Assert.Contains("new NativeWaterRuntime(message => LoggerInstance.Msg(message))", source);
        Assert.Contains("_log(WaterLookRecipe.Apply(new NativeWaterLookMaterial(clone)))", source);
        Assert.Single(source.Split('\n'), line => line.Contains("WaterLookRecipe.Apply("));
        foreach (string forbidden in new[] { "SetTexture(", "EnableKeyword(", "DisableKeyword(", "shaderKeywords", "new Camera", "SetInt(" })
            Assert.DoesNotContain(forbidden, source);
    }

    [Fact]
    public void ApprovedClearShallowsRecipeUsesTestedPolicyAndWriteOnlyMaterialAdapter()
    {
        string source = Source();
        Assert.Contains("WaterLookRecipe.Apply(new NativeWaterLookMaterial(clone))", source);
        Assert.Contains("NativeWaterLookMaterial : IWaterLookMaterial", source);
        Assert.Contains("_material.HasProperty(name)", source);
        Assert.Contains("_material.SetFloat(name, value)", source);
        Assert.DoesNotContain("GetColor(", source);
        Assert.DoesNotContain("ReadColorAlpha", source);
        Assert.Contains("_material.SetColor(name, new Color(r, g, b, alpha))", source);
        Assert.DoesNotContain("FloatTuning", source);
        Assert.DoesNotContain("ColorTuning", source);
    }
}

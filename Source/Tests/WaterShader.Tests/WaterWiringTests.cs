using Xunit;

namespace WaterShader.Tests;

public sealed class WaterWiringTests
{
    private static string Source()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        return File.ReadAllText(Path.Combine(directory!.FullName, "Source/Mods/WorldLook/src/WaterShaderMod.cs"));
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
        Assert.Contains("_session.Stop(", source);
        Assert.Contains("Stopwatch", source);
    }

    [Fact]
    public void RuntimeAppliesRecipeInConfigurationAndDoesNotAddRenderingFeatures()
    {
        string source = Source();
        int configurationStart = source.IndexOf("public void ConfigureMaterial(Material clone)", StringComparison.Ordinal);
        Assert.True(configurationStart >= 0, "the material configuration method is gone");
        int configurationEnd = source.IndexOf("private sealed class Log", configurationStart, StringComparison.Ordinal);
        Assert.True(configurationEnd > configurationStart);
        Assert.Contains("WaterLookRecipe.Apply(new NativeWaterLookMaterial(clone));", source[configurationStart..configurationEnd]);
        Assert.Single(source.Split('\n'), line => line.Contains("WaterLookRecipe.Apply("));
        foreach (string forbidden in new[] { "SetTexture(", "EnableKeyword(", "DisableKeyword(", "shaderKeywords", "new Camera", "SetInt(" })
            Assert.DoesNotContain(forbidden, source);
    }

    [Fact]
    public void ReferenceBranchAppliesTheClarityLookBeforeReturning()
    {
        string source = Source();
        Assert.Contains("ReferenceWaterLook.Apply(new NativeWaterLookMaterial(clone))", source);
        int branchStart = source.IndexOf("if (Reference != null && Reference.TryConfigure(clone))", StringComparison.Ordinal);
        Assert.True(branchStart >= 0, "the reference branch is gone");
        int branchReturn = source.IndexOf("return;", branchStart, StringComparison.Ordinal);
        Assert.True(branchReturn > branchStart, "the reference branch must still return early");
        int clarityIndex = source.IndexOf("ReferenceWaterLook.Apply(", StringComparison.Ordinal);
        // Block membership, not file order: the call has to sit INSIDE the reference branch, so
        // moving it down into the vanilla fallback below cannot keep this test green.
        Assert.InRange(clarityIndex, branchStart, branchReturn);
        int vanillaIndex = source.IndexOf("WaterLookRecipe.Apply(", StringComparison.Ordinal);
        Assert.True(vanillaIndex > branchReturn,
            "the vanilla recipe must stay the fallback behind the reference branch");
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

    [Fact]
    public void DisabledModulePumpsCleanupButKeepsReferenceUpdatesEnabledOnly()
    {
        string source = Source();
        int update = source.IndexOf("public void OnUpdate()", StringComparison.Ordinal);
        int tick = source.IndexOf("_session.Tick(now);", update, StringComparison.Ordinal);
        Assert.True(tick > update);
        string beforeTick = source[update..tick];
        Assert.Contains("if (_shutdown || _session == null) return;", beforeTick);
        Assert.DoesNotContain("_enabled", beforeTick);
        Assert.Contains("if (_enabled && _session.State == WaterState.Applied)", source[tick..]);
        Assert.Contains("_session.Stop(_clock.Elapsed.TotalSeconds)", source);
    }

    [Fact]
    public void ShutdownFencesCleanupUpdatesAndExplicitReenable()
    {
        string source = Source();
        int shutdown = source.IndexOf("public void OnDeinitializeMelon()", StringComparison.Ordinal);
        int gate = source.IndexOf("_shutdown = true;", shutdown, StringComparison.Ordinal);
        int stop = source.IndexOf("_session?.Stop(", shutdown, StringComparison.Ordinal);
        Assert.True(gate > shutdown && stop > gate);
        int update = source.IndexOf("public void OnUpdate()", StringComparison.Ordinal);
        int tick = source.IndexOf("_session.Tick(now);", update, StringComparison.Ordinal);
        Assert.Contains("if (_shutdown ||", source[update..tick]);
        int setEnabled = source.IndexOf("internal void SetEnabled(bool enabled)", StringComparison.Ordinal);
        int start = source.IndexOf("_session.Start(", setEnabled, StringComparison.Ordinal);
        Assert.Contains("if (_session == null || _shutdown) return;", source[setEnabled..start]);
    }
}

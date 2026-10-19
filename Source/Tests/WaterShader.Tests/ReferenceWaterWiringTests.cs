using Xunit;

namespace WaterShader.Tests;

/// <summary>
/// Supplementary wiring checks for the Unity-dependent reflection runtime. These
/// prove the safety shapes exist in source; they do not execute the render path.
/// </summary>
public sealed class ReferenceWaterWiringTests
{
    private static string Source()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        return File.ReadAllText(Path.Combine(directory!.FullName, "Source/Mods/WorldLook/src/ReferenceWaterRuntime.cs"));
    }

    [Fact]
    public void ReflectionPathUsesOneOwnedDisabledCameraAndBoundedTarget()
    {
        string source = Source();
        Assert.Contains("_reflectionCamera.enabled = false", source);
        Assert.Contains("new GameObject(\"WaterShader_ReflectionCamera\")", source);
        Assert.Contains("ReferenceWaterPolicy.ReflectionSizeForAspect", source);
        Assert.Contains("MaxReflectionDimension", source);
        Assert.DoesNotContain("GetTemporary", source);
        Assert.DoesNotContain("FindObjectsOfTypeAll", source);
        Assert.DoesNotContain("GetComponentsInChildren", source);
        Assert.DoesNotContain("GameObject.Find", source);
    }

    [Fact]
    public void ReflectionRenderReleasesTheReentrancyGateInFinallyWithoutInvertingCulling()
    {
        string source = Source();
        // The mirrored-pose camera is a proper rotation (LookRotation), so the
        // rasterized winding is already normal. Inverting culling is only the
        // compensation for a reflection-matrix camera, which this construction
        // does not use - doing it here renders the reflection inside-out. The
        // reentrancy gate must still be released in a finally.
        Assert.DoesNotContain("invertCulling", source);
        Assert.Contains("_rendering = true;", source);
        Assert.Contains("finally", source);
        Assert.Contains("_rendering = false;", source);
        Assert.Contains("UniversalRenderPipeline.RenderSingleCamera", source);
    }

    [Fact]
    public void ReentrancyGateResetIsInsideTheFinallyThatAlsoCoversTheNestedRender()
    {
        string source = Source();
        int gateIndex = source.IndexOf("_rendering = true;", StringComparison.Ordinal);
        int tryIndex = source.IndexOf("try", gateIndex, StringComparison.Ordinal);
        int renderIndex = source.IndexOf("UniversalRenderPipeline.RenderSingleCamera", StringComparison.Ordinal);
        int finallyIndex = source.IndexOf("finally", renderIndex, StringComparison.Ordinal);
        int resetIndex = source.IndexOf("_rendering = false;", finallyIndex, StringComparison.Ordinal);
        Assert.True(gateIndex >= 0 && tryIndex > gateIndex && renderIndex > tryIndex,
            "gate must close before the try that wraps the nested render");
        Assert.True(finallyIndex > renderIndex && resetIndex > finallyIndex,
            "the gate reset must sit inside the finally after the nested render");
    }

    [Fact]
    public void RenderHookIsSubscribedUnsubscribedAndHeldForIl2CppLifetime()
    {
        string source = Source();
        Assert.Contains("RenderPipelineManager.add_beginCameraRendering", source);
        Assert.Contains("RenderPipelineManager.remove_beginCameraRendering", source);
        Assert.Contains("DelegateSupport.ConvertDelegate", source);
        Assert.Contains("extern alias il2cpp;", source);
        Assert.Contains("Il2CppSystem.Action<ScriptableRenderContext, Camera>", source);
    }

    [Fact]
    public void ReferencePathIsReversibleAndFailsClosedToTheNativeRecipe()
    {
        string source = Source();
        Assert.Contains("clone.shader = originalShader;", source);
        Assert.Contains("ReferenceWaterPolicy.CanApplyReference", source);
        Assert.Contains("ReferenceWaterPolicy.MissingRequiredProperties", source);
        Assert.Contains("native recipe stays active", source);
        Assert.Contains("public void Dispose()", source);
        Assert.Contains("internal void Release()", source);
    }

    [Fact]
    public void ReflectionBindingUsesBothMaterialAndGlobalPaths()
    {
        string source = Source();
        Assert.Contains("material.SetTexture(\"_ReflectionTex\"", source);
        Assert.Contains("material.SetMatrix(\"_ReflectionVP\"", source);
        Assert.Contains("material.SetFloat(\"_ReflectionAvailable\"", source);
        Assert.Contains("Shader.SetGlobalMatrix(\"_ReflectionVP\"", source);
    }

    [Fact]
    public void ReentrancyGateIsClosedBeforeTheNestedRender()
    {
        string source = Source();
        int gateIndex = source.IndexOf("_rendering = true;", StringComparison.Ordinal);
        int renderIndex = source.IndexOf("UniversalRenderPipeline.RenderSingleCamera", StringComparison.Ordinal);
        Assert.True(gateIndex >= 0, "reentrancy gate assignment missing");
        Assert.True(renderIndex >= 0, "nested render call missing");
        Assert.True(gateIndex < renderIndex, "reentrancy gate must close before the nested render starts");
    }

    [Fact]
    public void ReflectionCallbackFailureIsContainedAndNeverFaultsTheGameRender()
    {
        string source = Source();
        Assert.Contains("reflection frame skipped", source);
        Assert.Contains("catch (Exception ex)", source);
    }
}

using System.Text.RegularExpressions;
using Xunit;

// Supplementary source wiring only: these do not execute Unity or prove native getter safety.
public class RuntimeWiringTests
{
    private static string Source(string name)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Source", "Mods"))) dir = dir.Parent;
        Assert.NotNull(dir);
        string path = Path.Combine(dir!.FullName, "Source", "Mods", "WorldLook", "src", "Probe", name);
        Assert.True(File.Exists(path), "Missing runtime file: " + name);
        return File.ReadAllText(path);
    }

    [Fact]
    public void TopLevelSceneFailureFeedsBoundedDiscoveryErrors()
    {
        string source = Source("RenderingSnapshot.cs");
        Assert.Contains("_report.Read(\"sceneCount\", () => SceneManager.sceneCount, ex => Error(\"scenes\", ex));", source);
        Assert.Contains("catch (Exception ex) { Error(\"scene\", ex);", source);
        int errorStart = source.IndexOf("private void Error(string section, Exception ex)", StringComparison.Ordinal);
        Assert.True(errorStart >= 0);
        string error = source[errorStart..];
        Assert.Contains("DiscoveryTruncated = true;", error);
        Assert.Contains("_errorCount++;", error);
        Assert.Contains("if (_errors.Count < 64)", error);
        Assert.Contains("discoveryTruncated={snapshot.DiscoveryTruncated}", Source("Mod.cs"));
    }

    [Fact]
    public void LogAndReportVersionComeFromMelonIdentity()
    {
        string mod = Source("Mod.cs");
        int initializeStart = mod.IndexOf("public void OnInitializeMelon(string version)", StringComparison.Ordinal);
        int initializeEnd = mod.IndexOf("public void OnSceneWasInitialized(", initializeStart, StringComparison.Ordinal);
        Assert.True(initializeStart >= 0 && initializeEnd > initializeStart);
        Assert.Contains("_version = version;", mod[initializeStart..initializeEnd]);
        Assert.DoesNotContain("_schedule.Request(", mod[initializeStart..initializeEnd]);
        Assert.Contains("new RenderingSnapshot(reason, _version)", mod);
        string snapshot = Source("RenderingSnapshot.cs");
        Assert.Contains("public RenderingSnapshot(string reason, string modVersion)", snapshot);
        Assert.Contains("[\"modVersion\"] = modVersion,", snapshot);
        Assert.DoesNotMatch(@"\[""modVersion""\]\s*=\s*""", snapshot);
    }

    [Fact]
    public void SceneDiscoveryIsExplicitBoundedAndDoesNotUseGlobalObjectSearch()
    {
        string source = Source("RenderingSnapshot.cs");
        Assert.Contains("SceneManager.sceneCount", source);
        Assert.Contains("GetRootGameObjects()", source);
        Assert.Contains("ReportPolicy.MaxNodes", source);
        Assert.Contains("ReportPolicy.MaxHierarchyDepth", source);
        Assert.Contains("scene.IsValid()", source);
        Assert.Contains("scene.isLoaded", source);
        Assert.Contains("node.GetChild(index)", source);
        Assert.Contains("ReportPolicy.MaxChildrenPerNode", source);
        Assert.Contains("ReportPolicy.MaxFrontierCursors", source);
        Assert.DoesNotMatch(@"Resources\.|FindObjects|Camera\.allCameras|GetAllCameras", source);
    }

    [Fact]
    public void CameraAndCandidateDataUseReadOnlySharedReferences()
    {
        string source = Source("ObjectReader.cs");
        Assert.Contains(".sharedMaterials", source);
        Assert.Contains(".sharedMesh", source);
        Assert.Contains(".depthTextureMode", source);
        Assert.Contains(".m_RequiresDepthTextureOption", source);
        Assert.Contains(".sharedProfile", source);
        Assert.Contains("unconfirmed", source);
    }

    [Fact]
    public void UpdateOnlyPollsChordAndScheduleAndFinallyReleasesCapture()
    {
        string source = Source("Mod.cs");
        Assert.Contains("OnSceneWasInitialized", source);
        Assert.Contains("KeyCode.F8", source);
        Assert.Contains("KeyCode.LeftControl", source);
        Assert.Contains("KeyCode.LeftShift", source);
        Assert.Contains("InputFocusAllowsCapture()", source);
        Assert.Contains("finally", source);
        Assert.Contains("_schedule.Complete", source);
        Assert.Contains("_schedule.Shutdown", source);
        Assert.Contains("SaveTextAtomic", source);
    }

    [Fact]
    public void PartialDiscoveryAndMaterialCapsAreExplicitlyMarked()
    {
        string objectSource = Source("ObjectReader.cs");
        Assert.Contains("if (scanned < materials.Length) result.Unreadable = true;", objectSource);
        string discoverySource = Source("RenderingSnapshot.cs");
        int start = discoverySource.IndexOf("private void Error(", StringComparison.Ordinal);
        Assert.Contains("DiscoveryTruncated = true;", discoverySource[start..]);
    }

    [Fact]
    public void PipelineTypeReadsExistingRuntimeFieldWithoutRendererResolution()
    {
        string source = Source("EnvironmentReader.cs");
        Assert.Contains("RenderPipelineManager.s_CurrentPipeline", source);
        Assert.Contains("currentRuntimePipelineType", source);
        Assert.Contains("GraphicsSettings.currentRenderPipeline", source);
        Assert.Contains("QualitySettings.renderPipeline", source);
        Assert.Contains("GraphicsSettings.defaultRenderPipeline", source);
    }

    [Fact]
    public void RuntimeUsesResumablePoliciesAndPrioritizesOneDeduplicatedMainHint()
    {
        string snapshot = Source("RenderingSnapshot.cs");
        Assert.Contains("FairTraversal<Transform>", snapshot);
        Assert.Contains("_walker.Advance()", snapshot);
        Assert.Contains("_scanBudget.TryScan", snapshot);
        Assert.Contains("unconfirmedParticleLineTrailMatches", snapshot);
        Assert.Contains("Camera.main", snapshot);
        Assert.Contains("Camera.main tagged enabled hint", snapshot);
        Assert.Contains("GetInstanceID()", snapshot);
        Assert.Contains("_cameraIds.Add", snapshot);
        Assert.Contains("_roots = null", snapshot);
        Assert.Contains("_components = null", snapshot);
        Assert.DoesNotContain("private void Walk(", snapshot);
        string mod = Source("Mod.cs");
        Assert.Contains("new CooperativeCapture(", mod);
        Assert.Contains("_capture.StepFrame()", mod);
        Assert.Contains("OnSceneWasLoaded", mod);
        Assert.Contains("OnSceneWasUnloaded", mod);
        Assert.Contains("CancelCapture();", mod);
        Assert.Contains("finalizationMilliseconds=", mod);
        Assert.Contains("totalWallMilliseconds=", mod);
        Assert.DoesNotMatch("allCameras|GetAllCameras", snapshot);
    }

    [Fact]
    public void ConstructorDoesNotReadUnityAndSceneHooksCancelBeforeRescheduling()
    {
        string snapshot = Source("RenderingSnapshot.cs");
        int start = snapshot.IndexOf("public RenderingSnapshot(", StringComparison.Ordinal);
        int end = snapshot.IndexOf("public bool Advance()", start, StringComparison.Ordinal);
        string constructor = snapshot[start..end];
        Assert.DoesNotMatch(@"Camera\.main\s*[;,)\]]|EnvironmentReader\.|SceneManager\.", constructor);
        Assert.Contains("captureStartedAtUtc", constructor);
        Assert.Contains("captureEndedAtUtc", snapshot);
        Assert.Contains("NOT an atomic frozen-frame snapshot", snapshot);
        Assert.Contains("_roots.Length, _rootQuota", snapshot);
        Assert.Contains("ReportPolicy.MaxPendingRoots / Math.Max(1, _sceneLimit)", snapshot);
        string mod = Source("Mod.cs");
        foreach (string hook in new[] { "OnSceneWasLoaded", "OnSceneWasUnloaded", "OnSceneWasInitialized" })
        {
            int hookStart = mod.IndexOf("public void " + hook, StringComparison.Ordinal);
            int hookEnd = mod.IndexOf("\n", hookStart, StringComparison.Ordinal);
            string body = mod[hookStart..hookEnd];
            Assert.Contains("CancelCapture();", body);
            Assert.DoesNotContain("_schedule.Request(", body);
        }
        int deinit = mod.IndexOf("public void OnDeinitializeMelon()", StringComparison.Ordinal);
        Assert.Contains("CancelCapture();", mod[deinit..]);
        Assert.Contains("activeWorkMilliseconds", snapshot);
        Assert.Contains("maxObservedStepMilliseconds", snapshot);
    }

    [Fact]
    public void RuntimeHasNoKnownMutatingOrLazyAllocationApis()
    {
        string all = string.Join("\n", new[] { Source("Mod.cs"), Source("RenderingSnapshot.cs"), Source("ObjectReader.cs"), Source("EnvironmentReader.cs") });
        Assert.DoesNotMatch(@"\.materials?\b|FindObjectsOfTypeAll|FindObjectsOfType|HarmonyPatch|\bInstantiate\s*\(|\bDestroy\s*\(|new\s+RenderTexture|\.profile\b|\.scriptableRenderer\b|\bGetRenderer\s*\(", all);
        Assert.DoesNotContain("S1API", all);
    }

    [Fact]
    public void MaterialPropertyIntrospectionIsCappedGatedAndErrorIsolated()
    {
        string reader = Source("ObjectReader.cs");
        Assert.Contains("if (includeProperties) row.Read(\"shaderProperties\", () => ShaderProperties(material));", reader);
        Assert.Contains("IntrospectionPolicy.PlanIndices(total, IntrospectionPolicy.MaxShaderProperties)", reader);
        Assert.Contains("entry.Read(\"name\", () => name = ReportPolicy.Text(shader.GetPropertyName(i)));", reader);
        Assert.Contains("entry.Read(\"kind\", () => shape = IntrospectionPolicy.ShapeOf(shader.GetPropertyType(i).ToString()));", reader);
        Assert.Contains("IntrospectionPolicy.Normalize(material.GetFloat(name))", reader);
        Assert.Contains("IntrospectionPolicy.Normalize(light.shadowStrength)", reader);
    }

    [Fact]
    public void SunAndInGameTimeEvidenceIsGuardedBoundedAndCoherent()
    {
        string env = Source("EnvironmentReader.cs");
        Assert.Contains("ObjectReader.MaterialDetails(RenderSettings.skybox, includeProperties: true)", env);
        Assert.Contains("if (!S1Mods.Shared.NetworkGuard.IsAlive(timeManager)) return null;", env);
        Assert.Contains("int current = timeManager.CurrentTime;", env);
        Assert.Contains("ReportPolicy.Text(Il2CppScheduleOne.GameTime.TimeManager.GetMinutesToDisplayTime(current))", env);
        Assert.Contains("var sun = RenderSettings.sun;", env);
        int guard = env.IndexOf("NetworkGuard.IsAlive(timeManager)", StringComparison.Ordinal);
        int read = env.IndexOf("int current = timeManager.CurrentTime;", StringComparison.Ordinal);
        Assert.True(guard >= 0 && read > guard, "TimeManager reads must come after the IsAlive guard.");
    }
}

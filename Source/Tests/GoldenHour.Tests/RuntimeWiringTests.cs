using Xunit;

// Supplementary source wiring only: these do not execute Unity or prove native getter safety.
// They pin the review findings as source contracts (B1/B2 of the 0.1.0 pre-deploy review).
public class RuntimeWiringTests
{
    private static string Source(string modFolder, string name)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Source", "Mods"))) dir = dir.Parent;
        Assert.NotNull(dir);
        string path = Path.Combine(dir!.FullName, "Source", "Mods", "WorldLook", "src", "Lighting", name);
        Assert.True(File.Exists(path), "Missing runtime file: " + name);
        return File.ReadAllText(path);
    }

    [Fact]
    public void FirstWriteOfLateReadableKeyIsSnapshotBacked_NoCompounding()
    {
        // Review B1: Handle must not blend from an unrecorded current value; the first
        // write of a key that was unreadable at activation records the pristine value.
        string source = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("store.RecordOnce(key, current.Value);", source);
        Assert.Contains("First write of a key that was unreadable at activation", source);
    }

    [Fact]
    public void SceneExitRestoresAssetLevelTargetsIntoRecordedMaterialOnly()
    {
        // Review B2: scene exit must not write per-scene RenderSettings into the
        // destination scene, and _SunShine must go back into the recorded material.
        string targets = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("public int RestoreAssetLevel(SnapshotStore store, ModLogger log)", targets);
        Assert.Contains("private Material? _skyMaterial;", targets);
        Assert.Contains("_skyMaterial ??= mat;", targets);
        Assert.Contains("deliberately skipped", targets);
        int assetLevelStart = targets.IndexOf("public int RestoreAssetLevel", StringComparison.Ordinal);
        int cacheStart = targets.IndexOf("public void InvalidateRuntimeCache", StringComparison.Ordinal);
        Assert.True(assetLevelStart >= 0 && cacheStart > assetLevelStart);
        string assetLevelBody = targets[assetLevelStart..cacheStart];
        Assert.DoesNotContain("ambientSkyColor", assetLevelBody);
        Assert.DoesNotContain("fogColor", assetLevelBody);
        Assert.DoesNotContain("WriteSunColor", assetLevelBody);

        string mod = Source("GoldenHour", "Mod.cs");
        Assert.Contains("_targets.RestoreAssetLevel(_store, _log);", mod);
    }

    [Fact]
    public void RuntimeUnloadRestoresBeforeTeardown()
    {
        // Review N2: OnDeinitializeMelon covers MelonLoader disable/unload while active.
        string mod = Source("GoldenHour", "Mod.cs");
        Assert.Contains("public void OnDeinitializeMelon()", mod);
        Assert.Contains("HandleSceneExit(\"mod deinitialized\");", mod);
    }

    [Fact]
    public void HotkeyAcceptsBothModifierSides()
    {
        // Review N4: right-side Ctrl/Shift must work too (GraphicsProbe precedent).
        string mod = Source("GoldenHour", "Mod.cs");
        Assert.Contains("KeyCode.RightControl", mod);
        Assert.Contains("KeyCode.RightShift", mod);
    }

    // --- 0.1.1 hardening (ChatGPT fix round: accepted parts only) ---------------------

    [Fact]
    public void VolumeWriteForcesComponentAndOverrideState()
    {
        // Writing colorFilter.value is a no-op unless the component is active and the
        // override is set — the likely cause of an invisible tint.
        string targets = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("colorFilter.overrideState = true;", targets);
        Assert.Contains("adjustments.active = true;", targets);
        Assert.Contains("VolumeFlagPolicy.NeedsEnable", targets);
        Assert.Contains("_originalFilterOverride", targets);
        Assert.Contains("_originalVolumeActive", targets);
    }

    [Fact]
    public void VolumeUsesRuntimeProfileNotSharedAsset()
    {
        // volume.profile is a dedicated runtime instance; the shared asset stays untouched.
        string targets = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("volume.profile", targets);
        Assert.DoesNotContain("sharedProfile", targets);
    }

    [Fact]
    public void DiagnosticsAreAvailableForInGameVerify()
    {
        string targets = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("LogDiagnostics", targets);
        Assert.Contains("volumeLayerMask", targets);
        Assert.Contains("renderPostProcessing", targets);
        Assert.Contains("HasProperty", targets);
        string mod = Source("GoldenHour", "Mod.cs");
        Assert.Contains("LogDiagnostics", mod);
    }

    [Fact]
    public void BaselineAdoptsDriftAndRestoreNeverSkips()
    {
        // 0.1.4 revision of the 0.1.1 contract: "baseline replacement weakens
        // first-wins" assumed the game does not rewrite these values. Measured
        // 2026-10-17: it does, continuously — keeping the activation snapshot froze
        // the sun color and a later restore would drag it back hours. Re-basing on
        // the game's drift keeps restore at the latest vanilla value. Restore is
        // still never skipped on external changes.
        string targets = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("store.Update(key, decision.Baseline);", targets);
        Assert.DoesNotContain("changed externally", targets);
    }

    [Fact]
    public void PerFrameApplyIsTheDefault()
    {
        // 0.1.4 revision of the 0.1.3 contract: the "does not rewrite continuously"
        // evidence was wrong (measured 2026-10-17). A once-per-second reapply loses
        // every frame race and the bias never shows on screen. Per-frame apply is
        // now the default; the bounded cadence remains configurable for
        // cost-sensitive setups. DiagnosticStrongTint stays opt-in.
        string config = Source("GoldenHour", "GoldenHourConfig.cs");
        Assert.Contains("ApplyEveryLateUpdate { get; set; } = true;", config);
        Assert.Contains("DiagnosticStrongTint { get; set; } = false;", config);
    }

    [Fact]
    public void VolumeWriteRequiresGameInstantiatedProfile()
    {
        // Review F1: accessing volume.profile instantiates it and would decouple the
        // game's own per-frame colorFilter writes. Only write when the game already
        // instantiated the profile itself.
        string targets = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("HasInstantiatedProfile", targets);
        Assert.Contains("VolumeProfilePolicy.ShouldUseProfile", targets);
        // N-a: an uninstantiated volume must not end the search for a later instantiated one.
        Assert.Contains("sawUninstantiated", targets);
        // Live evidence 2026-10-16: the volume path is @Managers/@EnvironmentFX/GlobalVolume —
        // "EnvironmentFX" is NOT a scene root, so filtering roots on that token alone never
        // matched and the volume key silently stayed unapplied.
        Assert.Contains("IndexOf(\"Managers\"", targets);
        // N-1: a found-but-unusable volume must not be reported as a missing one.
        Assert.Contains("sawVolume", targets);
        Assert.Contains("no GlobalVolume found", targets);
    }

    [Fact]
    public void ClockWeightScalesStrengthAndDoesNotEnableFog()
    {
        // 0.1.5: a constant warm multiply paints noon gold. The clock weight must
        // scale the apply, and a strength change must be visible to BaselinePolicy.
        // Fog stays disabled — enabling it is a visibility change without a measured hook.
        string mod = Source("GoldenHour", "Mod.cs");
        Assert.Contains("DaylightPolicy.Weight(minutes)", mod);
        Assert.Contains("_targets.Apply(_store, strength, diagnostic, _log, _lastAppliedStrength);", mod);
        Assert.DoesNotContain("RenderSettings.fog = true", mod);

        string targets = Source("GoldenHour", "SceneTargets.cs");
        Assert.Contains("BaselinePolicy.Evaluate(original, current.Value, tint, strength, previousStrength);", targets);
        Assert.DoesNotContain("RenderSettings.fog = true", targets);
    }
}

using System;
using System.IO;
using Xunit;

namespace AutoPackagingStation.Tests;

// Source contracts only: these prevent the reproduced late-AddComponent path.
// They do NOT execute Unity/FishNet or establish in-game initialization success.
public sealed class NativeStationWiringTests
{
    private static string FactorySource()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            var path = Path.Combine(dir.FullName, "Source", "Mods", "AutoPackagingStation", "src", "AutoPackagingItemFactory.cs");
            if (File.Exists(path)) return File.ReadAllText(path);
            dir = dir.Parent;
        }
        throw new FileNotFoundException("Repository source not found.");
    }

    private static bool RepoSourceFileExists(string fileName)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Source", "Mods", "AutoPackagingStation", "src", fileName))) return true;
            dir = dir.Parent;
        }
        return false;
    }

    [Fact]
    public void PlacedSetupNeverAddsNativePackagingNetworkBehaviour()
    {
        string source = FactorySource();
        Assert.DoesNotContain("go.AddComponent(Il2CppType.Of<Il2CppScheduleOne.ObjectScripts.PackagingStation>())", source);
    }

    [Fact]
    public void OwnedPrefabIsComposedBeforeDefinitionIsPublished()
    {
        string source = FactorySource();
        int compose = source.IndexOf("AutoPackPrefabComposer.Compose(", StringComparison.Ordinal);
        int publish = source.IndexOf("builder.Build()", StringComparison.Ordinal);
        int assign = source.IndexOf("nativeDefinition.BuiltItem = ownedPrefab;", StringComparison.Ordinal);
        int ghost = source.IndexOf("RegisterPreparedGhost(itemId);", StringComparison.Ordinal);
        Assert.True(compose >= 0 && compose < assign && assign < publish);
        Assert.True(ghost >= 0 && ghost < publish);
    }

    private static string ModSource(string fileName)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            var path = Path.Combine(dir.FullName, "Source", "Mods", "AutoPackagingStation", "src", fileName);
            if (File.Exists(path)) return File.ReadAllText(path);
            dir = dir.Parent;
        }
        throw new FileNotFoundException(fileName);
    }

    [Fact]
    public void StartupVersionUsesReleaseMetadata()
    {
        Assert.Contains("Info.Version", ModSource("Mod.cs"));
    }

    [Fact]
    public void ComposedTemplateHasAReusableLifetimeOwner()
    {
        string composer = ModSource("AutoPackPrefabComposer.cs");
        Assert.Contains("_template = station;", composer);
        int reuse = composer.IndexOf("return _template;", StringComparison.Ordinal);
        int clone = composer.IndexOf("Object.Instantiate(", StringComparison.Ordinal);
        Assert.True(reuse >= 0 && reuse < clone);
        Assert.Contains("AutoPackagingItemFactory.ShutdownPrefabCache();", ModSource("Mod.cs"));
    }

    [Fact]
    public void NativeGhostMaterialMarkerIsAllowedInFootprintGeometry()
    {
        string composer = ModSource("AutoPackPrefabComposer.cs");
        int start = composer.IndexOf("private static void ValidateGeometryOnly(", StringComparison.Ordinal);
        int end = composer.IndexOf("private static void ValidateMovedTile(", start, StringComparison.Ordinal);
        string guard = composer.Substring(start, end - start);
        Assert.Contains("script.TryCast<Il2CppScheduleOne.Building.OverrideGhostMaterial>() != null", guard);
        Assert.Contains("GetComponentsInChildren<Il2CppFishNet.Object.NetworkBehaviour>(true)", guard);
        Assert.Contains("GetComponentsInChildren<Il2CppFishNet.Object.NetworkObject>(true)", guard);
        Assert.Contains("contains non-footprint behaviour", guard);
    }

    [Fact]
    public void NativeInteractionUsesStationReferenceAndChildLookup()
    {
        string source = FactorySource();
        int start = source.IndexOf("public static void SetupPlacedStation(", StringComparison.Ordinal);
        string setup = source.Substring(start);
        Assert.Contains("var interactable = station?.IntObj;", setup);
        Assert.Contains("go.GetComponentInChildren<Il2CppScheduleOne.Interaction.InteractableObject>(true)", setup);
        Assert.Contains("interactable.transform.IsChildOf(go.transform)", setup);
        Assert.DoesNotContain("go.AddComponent(Il2CppType.Of<Il2CppScheduleOne.Interaction.InteractableObject>())", setup);
    }

    [Fact]
    public void PlacedSetupReportsNativeInteractionRetentionWithoutUnverifiedClaims()
    {
        string source = FactorySource();
        int start = source.IndexOf("public static void SetupPlacedStation(", StringComparison.Ordinal);
        int end = source.IndexOf("public static GameObject? SpawnStationAt(", start, StringComparison.Ordinal);
        string setup = source.Substring(start, end - start);
        Assert.Contains("Configured AutoPackagingStation [", setup);
        Assert.Contains("(controller + visuals + native interaction).", setup);
        Assert.DoesNotContain("E-prompt/canvas unverified", setup);
        Assert.DoesNotContain("E-interact wired", setup);
    }

    [Fact]
    public void PlacedSetupDoesNotAddOrOverwriteARootInteractionBlockingCollider()
    {
        string source = FactorySource();
        int start = source.IndexOf("public static void SetupPlacedStation(", StringComparison.Ordinal);
        int end = source.IndexOf("public static GameObject? SpawnStationAt(", start, StringComparison.Ordinal);
        string setup = source.Substring(start, end - start);
        Assert.DoesNotContain("GetComponent<BoxCollider>()", setup);
        Assert.DoesNotContain("Il2CppType.Of<BoxCollider>()", setup);
        Assert.DoesNotContain("box.center =", setup);
        Assert.DoesNotContain("box.size =", setup);
        Assert.DoesNotContain("box.enabled =", setup);
    }

    // 2026-10-15: the collider A/B experiment is resolved (native E/canvas verified in
    // gameplay with the root-collider manipulation removed). The read-only interaction
    // diagnostics were experiment scaffolding and are fully retired - asserted against
    // active code AND comments, so no commented-out remnant can survive.
    [Fact]
    public void InteractionDiagnosticScaffoldingIsFullyRetired()
    {
        Assert.DoesNotContain("AutoPackInteractionDiagnostics", ModSource("Mod.cs"));
        Assert.DoesNotContain("AutoPackInteractionDiagnostics", FactorySource());
        Assert.DoesNotContain("AutoPackInteractionDiagnostics", ModSource("AutoPackStationController.cs"));
        Assert.DoesNotContain("InstallAwakeObserver", ModSource("Mod.cs"));
        Assert.DoesNotContain("AutoPackagingStation.Diagnostics", ModSource("Mod.cs"));
        Assert.DoesNotContain("AutoPackagingStation.Diagnostics", FactorySource());
        Assert.False(RepoSourceFileExists("AutoPackInteractionDiagnostics.cs"));
        Assert.False(RepoSourceFileExists("AutoPackDiagnosticState.cs"));
    }

    [Fact]
    public void DonorCandidatesContainOnlyNativePackagingStations()
    {
        string source = FactorySource();
        int start = source.IndexOf("string[] candidateBases", StringComparison.Ordinal);
        int end = source.IndexOf(';', start);
        string candidates = source.Substring(start, end - start);
        Assert.Contains("\"packagingstation\"", candidates);
        Assert.DoesNotContain("\"cauldron\"", candidates);
        Assert.DoesNotContain("\"brickpress\"", candidates);
        Assert.DoesNotContain("\"storage\"", candidates);
    }
}

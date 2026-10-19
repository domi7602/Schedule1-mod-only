using Xunit;

namespace Minimap.Tests;

/// <summary>Supplementary source wiring checks, not Unity or native execution.</summary>
public sealed class SearchDevelopmentWiringTests
{
    private static string Source(string relativePath)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        return File.ReadAllText(Path.Combine(dir!.FullName, "Source/Mods/Minimap/src", relativePath));
    }

    [Fact]
    public void ObservationRunsBeforeHudCreationGuard()
    {
        string source = Source("MinimapMod.cs");
        int update = source.IndexOf("public override void OnUpdate()", StringComparison.Ordinal);
        int tick = source.IndexOf("_searchDevelopment.Tick();", update, StringComparison.Ordinal);
        int guard = source.IndexOf("if (!_hud.IsCreated)", update, StringComparison.Ordinal);
        Assert.True(tick > update && tick < guard, "Developer observation must not depend on HUD visibility/creation.");
    }

    [Fact]
    public void DeveloperWatchOwnsLoadBoundariesAndDisposesAtDeinitialization()
    {
        string source = Source("MinimapMod.cs");
        Assert.Contains("_searchDevelopment.Initialize();", source);
        Assert.Contains("public override void OnDeinitializeMelon()", source);
        Assert.Contains("_searchDevelopment.Dispose();", source);
    }

    [Fact]
    public void NativeObservationRequiresAnExplicitValidWorldCandidate()
    {
        string source = Source("MinimapSearchDevelopment.cs");
        Assert.Contains("if (!ready || !worldCandidateValid || !_loadCompleted)", source);
        Assert.Contains("!string.IsNullOrWhiteSpace(path)", source);
        Assert.Contains("worldCandidateValid = false;", source);
        Assert.Contains("_gate.TryConsumeAutoStop()", source);
    }

    [Fact]
    public void DeveloperCommandsAreExplicitAndNotPresentedAsPlayableSearch()
    {
        string source = Source("MinimapCommands.cs");
        Assert.Contains("case \"searchdev\":", source);
        Assert.Contains("mod.SearchDevelopmentCommand(args);", source);
    }
}

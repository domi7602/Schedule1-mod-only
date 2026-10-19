using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Minimap.Tests;

/// <summary>
/// Executable behavioral tests for the Unity-free search controller: optional default-off,
/// load/epoch boundaries, unknown observations, accept/cancel/restore rules, the single
/// continuously-valid false-to-true completion signal, and area coverage geometry.
/// </summary>
public sealed class SearchControllerTests
{
    private static SearchArea Area() =>
        new SearchArea("sewer-key", 1, 100f, 0f, 200f, 40f, 12f, 5f);

    private static SearchObservation Obs(
        bool ready,
        bool? collected,
        string world,
        string target,
        bool targetKnown = true,
        float x = 100f,
        float y = 0f,
        float z = 200f) =>
        new(ready, collected, new SearchIdentity(world, target), targetKnown, x, y, z);

    private static SearchController Enabled()
    {
        var controller = new SearchController(Area());
        controller.Enable();
        return controller;
    }

    // ---- optional, default disabled, HUD-independent ----

    [Fact]
    public void DisabledByDefault()
    {
        var controller = new SearchController(Area());
        Assert.False(controller.Enabled);
        Assert.Equal(SearchPhase.Disabled, controller.Snapshot.Phase);
        Assert.False(controller.Snapshot.Accepted);
    }

    [Fact]
    public void ObservationsAreIgnoredWhileDisabled()
    {
        var controller = new SearchController(Area());
        controller.Observe(Obs(true, false, "worldA", "keyA"));
        Assert.Equal(SearchPhase.Disabled, controller.Snapshot.Phase);
    }

    [Fact]
    public void EnableStartsCheckingAndBumpsEpoch()
    {
        var controller = new SearchController(Area());
        Assert.Equal(0, controller.Epoch);
        controller.Enable();
        Assert.True(controller.Enabled);
        Assert.Equal(1, controller.Epoch);
        Assert.Equal(SearchPhase.Checking, controller.Snapshot.Phase);
    }

    [Fact]
    public void DisableAndReenableEachStartANewEpochAndClearAcceptance()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        Assert.Equal(SearchPhase.Searching, controller.Snapshot.Phase);

        controller.Disable();
        Assert.Equal(2, controller.Epoch);
        Assert.False(controller.Enabled);
        Assert.Equal(SearchPhase.Disabled, controller.Snapshot.Phase);

        controller.Enable();
        Assert.Equal(3, controller.Epoch);
        Assert.Equal(SearchPhase.Checking, controller.Snapshot.Phase);
        Assert.False(controller.Snapshot.Accepted);
        Assert.Null(controller.CurrentRecord);
    }

    [Fact]
    public void OnLoadStartsANewEpoch()
    {
        var controller = Enabled();
        int before = controller.Epoch;
        controller.OnLoad();
        Assert.Equal(before + 1, controller.Epoch);
    }

    // ---- accept preconditions ----

    [Fact]
    public void AcceptRequiresReady()
    {
        var controller = Enabled();
        controller.Observe(Obs(false, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.NotReady, controller.Accept());
        Assert.False(controller.Snapshot.Accepted);
    }

    [Fact]
    public void AcceptRequiresKnownIdentity()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, string.Empty, string.Empty));
        Assert.Equal(SearchAcceptStatus.IdentityUnknown, controller.Accept());
    }

    [Theory]
    [InlineData(null)]
    [InlineData(true)]
    public void AcceptRequiresReliableFalseCollected(bool? collected)
    {
        var controller = Enabled();
        controller.Observe(Obs(true, collected, "w", "k"));
        Assert.Equal(SearchAcceptStatus.CollectedNotFalse, controller.Accept());
    }

    [Fact]
    public void AcceptRequiresValidArea()
    {
        var controller = new SearchController(new SearchArea(string.Empty, 1, 0f, 0f, 0f, 10f, 5f, 1f));
        controller.Enable();
        controller.Observe(Obs(true, false, "w", "k", x: 0f, y: 0f, z: 0f));
        Assert.Equal(SearchAcceptStatus.AreaInvalid, controller.Accept());
    }

    [Fact]
    public void AcceptRequiresTargetCoverage()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k", targetKnown: false));
        Assert.Equal(SearchAcceptStatus.TargetNotCovered, controller.Accept());
        controller.Observe(Obs(true, false, "w", "k", x: 0f, y: 0f, z: 0f));
        Assert.Equal(SearchAcceptStatus.TargetNotCovered, controller.Accept());
        Assert.False(controller.Snapshot.Accepted);
    }

    [Fact]
    public void AcceptSucceedsWithAllPreconditions()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        Assert.Equal(SearchPhase.Searching, controller.Snapshot.Phase);
        Assert.True(controller.Snapshot.Accepted);
    }

    [Fact]
    public void AcceptTwiceDoesNotDuplicateRevisionOrWrite()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        long revision = controller.Snapshot.Revision;
        SearchRecord record = controller.CurrentRecord!;

        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        Assert.Equal(revision, controller.Snapshot.Revision);
        Assert.Same(record, controller.CurrentRecord);
    }

    [Fact]
    public void CancelKeepsAreaStableAndReacceptReusesSameIdentity()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        SearchRecord first = controller.CurrentRecord!;

        controller.Cancel();
        SearchRecord cancelled = controller.CurrentRecord!;
        Assert.False(cancelled.Accepted);
        Assert.False(controller.Snapshot.Accepted);
        Assert.Equal(SearchPhase.Idle, controller.Snapshot.Phase);
        Assert.Equal(first.World, cancelled.World);
        Assert.Equal(first.Target, cancelled.Target);
        Assert.Equal(first.AreaId, cancelled.AreaId);
        Assert.Equal(first.AreaVersion, cancelled.AreaVersion);

        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        SearchRecord again = controller.CurrentRecord!;
        Assert.True(again.Accepted);
        Assert.Equal(first.World, again.World);
        Assert.Equal(first.AreaId, again.AreaId);
        Assert.Equal(SearchPhase.Searching, controller.Snapshot.Phase);
    }

    // ---- restore / context switch ----

    [Fact]
    public void RestoreMatchingRecordReopensAcceptedSearch()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.True(controller.Restore(new SearchRecord("w", "k", "sewer-key", 1, accepted: true)));
        Assert.True(controller.Snapshot.Accepted);
        Assert.Equal(SearchPhase.Searching, controller.Snapshot.Phase);
    }

    [Fact]
    public void RestoreCancelledRecordDoesNotAcceptAgain()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.True(controller.Restore(new SearchRecord("w", "k", "sewer-key", 1, accepted: false)));
        Assert.False(controller.Snapshot.Accepted);
        Assert.False(controller.CurrentRecord!.Accepted);
    }

    [Fact]
    public void RestoreNonMatchingRecordChangesNothing()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.False(controller.Restore(new SearchRecord("other", "k", "sewer-key", 1, false)));
        Assert.False(controller.Restore(new SearchRecord("w", "other", "sewer-key", 1, false)));
        Assert.False(controller.Restore(new SearchRecord("w", "k", "other-area", 1, false)));
        Assert.False(controller.Restore(new SearchRecord("w", "k", "sewer-key", 2, false)));
        Assert.False(controller.Snapshot.Accepted);
        Assert.Null(controller.CurrentRecord);
    }

    [Fact]
    public void RestoreBeforeKnownContextReturnsFalse()
    {
        var controller = Enabled();
        Assert.False(controller.Restore(new SearchRecord("w", "k", "sewer-key", 1, false)));
    }

    [Fact]
    public void ContextSwitchClearsVisibleStateAndAcceptanceUntilExplicitRestore()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "worldA", "keyA"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        long revision = controller.Snapshot.Revision;

        controller.Observe(Obs(true, false, "worldB", "keyB"));
        Assert.Equal(SearchPhase.Idle, controller.Snapshot.Phase);
        Assert.False(controller.Snapshot.Accepted);
        Assert.Null(controller.CurrentRecord);
        Assert.True(controller.Snapshot.Revision > revision);

        Assert.True(controller.Restore(new SearchRecord("worldB", "keyB", "sewer-key", 1, false)));
        Assert.False(controller.Snapshot.Accepted);
    }

    [Fact]
    public void UnknownObservationPreservesAcceptedIntentWithoutSuccess()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());

        controller.Observe(SearchObservation.Unknown);
        Assert.True(controller.Snapshot.Accepted);
        Assert.Equal(SearchPhase.Checking, controller.Snapshot.Phase);
        Assert.False(controller.TryConsumeCompletion(out _));
    }

    // ---- collection transition rules ----

    [Fact]
    public void ContinuouslyValidFalseToTrueDuringAcceptedSearchEmitsOneCompletion()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());

        controller.Observe(Obs(true, true, "w", "k"));
        Assert.Equal(SearchPhase.Completed, controller.Snapshot.Phase);
        Assert.True(controller.TryConsumeCompletion(out SearchCompletion completion));
        Assert.Equal("w", completion.World);
        Assert.Equal("k", completion.Target);
        Assert.False(controller.TryConsumeCompletion(out _));
    }

    [Fact]
    public void TemporaryGapResetsTransitionBaseline()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());

        controller.Observe(Obs(true, null, "w", "k"));
        controller.Observe(Obs(true, true, "w", "k"));

        Assert.Equal(SearchPhase.AlreadyFound, controller.Snapshot.Phase);
        Assert.False(controller.TryConsumeCompletion(out _));
    }

    [Fact]
    public void NotReadyGapAlsoResetsTransitionBaseline()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());

        controller.Observe(Obs(false, false, "w", "k"));
        controller.Observe(Obs(true, true, "w", "k"));

        Assert.False(controller.TryConsumeCompletion(out _));
    }

    [Fact]
    public void FirstReliableTrueWithoutAcceptanceIsAlreadyFoundWithoutSuccess()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, true, "w", "k"));
        Assert.Equal(SearchPhase.AlreadyFound, controller.Snapshot.Phase);
        Assert.False(controller.TryConsumeCompletion(out _));
    }

    [Fact]
    public void TransitionWithoutAcceptanceDoesNotComplete()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        controller.Observe(Obs(true, true, "w", "k"));
        Assert.Equal(SearchPhase.AlreadyFound, controller.Snapshot.Phase);
        Assert.False(controller.TryConsumeCompletion(out _));
    }

    [Fact]
    public void FirstReliableTrueAfterLoadIsAlreadyFoundWithoutSuccessEvenSameWorld()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        controller.Observe(Obs(true, true, "w", "k"));
        Assert.True(controller.TryConsumeCompletion(out _));

        controller.OnLoad();
        Assert.Equal(SearchPhase.Checking, controller.Snapshot.Phase);
        Assert.False(controller.Snapshot.Accepted);

        controller.Observe(Obs(true, true, "w", "k"));
        Assert.Equal(SearchPhase.AlreadyFound, controller.Snapshot.Phase);
        Assert.False(controller.TryConsumeCompletion(out _));
    }

    [Fact]
    public void RestoreThenCollectCompletes()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.True(controller.Restore(new SearchRecord("w", "k", "sewer-key", 1, true)));
        controller.Observe(Obs(true, true, "w", "k"));
        Assert.Equal(SearchPhase.Completed, controller.Snapshot.Phase);
        Assert.True(controller.TryConsumeCompletion(out _));
    }

    [Fact]
    public void ProximityAndMissingPickupNeverImplyCollection()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());

        controller.Observe(Obs(true, false, "w", "k", targetKnown: false));
        Assert.Equal(SearchPhase.Checking, controller.Snapshot.Phase);
        Assert.False(controller.TryConsumeCompletion(out _));

        controller.Observe(Obs(true, false, "w", "k", x: 100.1f, y: 0f, z: 200.1f));
        Assert.Equal(SearchPhase.Searching, controller.Snapshot.Phase);
        Assert.False(controller.TryConsumeCompletion(out _));
    }

    // ---- area geometry ----

    [Fact]
    public void AreaContainsUsesFiniteXyzHeightAndRadius()
    {
        SearchArea area = Area();
        Assert.True(area.IsValid);
        Assert.True(area.Contains(100f, 0f, 200f));
        Assert.True(area.Contains(139f, 0f, 200f));
        Assert.False(area.Contains(141f, 0f, 200f));
        Assert.True(area.Contains(100f, 12f, 200f));
        Assert.False(area.Contains(100f, 13f, 200f));
        Assert.False(area.Contains(float.NaN, 0f, 200f));
        Assert.False(area.Contains(100f, 0f, float.PositiveInfinity));
    }

    [Fact]
    public void AreaContainsTargetAppliesInnerMargin()
    {
        SearchArea area = Area();
        Assert.True(area.ContainsTarget(134f, 0f, 200f));
        Assert.False(area.ContainsTarget(136f, 0f, 200f));
        Assert.True(area.Contains(136f, 0f, 200f));
    }

    [Fact]
    public void AreaCenterIsSuppliedAndIndependentOfTarget()
    {
        SearchArea area = Area();
        Assert.Equal(100f, area.CenterX);
        Assert.Equal(0f, area.CenterY);
        Assert.Equal(200f, area.CenterZ);
        Assert.True(area.ContainsTarget(100.5f, 0f, 200.5f));
    }

    [Theory]
    [InlineData("", 1, 0f, 0f, 0f, 10f, 5f, 1f)]
    [InlineData("a", 1, 0f, 0f, 0f, 0f, 5f, 1f)]
    [InlineData("a", 1, 0f, 0f, 0f, float.NaN, 5f, 1f)]
    [InlineData("a", 1, 0f, 0f, 0f, 10f, 5f, 10f)]
    [InlineData("a", 1, 0f, 0f, 0f, 10f, -1f, 1f)]
    public void AreaRejectsInvalidGeometry(
        string id, int version, float cx, float cy, float cz, float radius, float height, float margin)
    {
        var area = new SearchArea(id, version, cx, cy, cz, radius, height, margin);
        Assert.False(area.IsValid);
    }

    // ---- snapshot / record shape ----

    [Fact]
    public void SnapshotRevisionChangesOnlyOnPresentationChange()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        long baseline = controller.Snapshot.Revision;

        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(baseline, controller.Snapshot.Revision);

        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());
        Assert.True(controller.Snapshot.Revision > baseline);
        long searching = controller.Snapshot.Revision;

        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(searching, controller.Snapshot.Revision);

        controller.Observe(SearchObservation.Unknown);
        Assert.True(controller.Snapshot.Revision > searching);
    }

    [Fact]
    public void AcceptedRecordExposesOnlyWorldTargetAreaAndAccepted()
    {
        var controller = Enabled();
        controller.Observe(Obs(true, false, "w", "k"));
        Assert.Equal(SearchAcceptStatus.Accepted, controller.Accept());

        string[] names = typeof(SearchRecord)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(new[] { "Accepted", "AreaId", "AreaVersion", "Target", "World" }, names);
        Assert.True(controller.CurrentRecord!.Accepted);
    }

    // ---- supplementary wiring contract: core stays Unity/HUD-free ----

    [Fact]
    public void CoreSourcesStayUnityAndHudFree()
    {
        string? root = RepoRoot();
        Assert.False(root is null, "Repository root (AGENTS.md) not found for the source contract check.");
        // Flattened layout: the former src/Search/Core sources now sit flat in src/;
        // the search core is exactly the Search*.cs files there.
        string coreDir = Path.Combine(root!, "Source/Mods/Minimap/src");
        string[] files = Directory.GetFiles(coreDir, "Search*.cs");
        Assert.NotEmpty(files);
        foreach (string file in files)
        {
            string text = File.ReadAllText(file);
            Assert.DoesNotContain("UnityEngine", text);
            Assert.DoesNotContain("Il2Cpp", text);
            Assert.DoesNotContain("MinimapHud", text);
            Assert.DoesNotContain("MinimapBlips", text);
        }
    }

    private static string? RepoRoot()
    {
        string? fromEnv = Environment.GetEnvironmentVariable("MINIMAP_REPO_ROOT");
        foreach (string? start in new[] { fromEnv, AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            if (string.IsNullOrEmpty(start))
                continue;
            var dir = new DirectoryInfo(start);
            while (dir is not null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "AGENTS.md")))
                    return dir.FullName;
                dir = dir.Parent;
            }
        }
        return null;
    }
}

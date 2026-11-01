using TownHallInterior;
using Xunit;

public sealed class CleanupJournalTests
{
    [Fact]
    public void FailedLodRestoreDefersOwnedMeshReleaseButNotColliderRestoration()
    {
        var journal = new CleanupJournal();
        bool lodsRestored = false;
        int lodAttempts = 0, colliderRestores = 0, meshReleases = 0;
        journal.Add("LOD", () =>
        {
            if (++lodAttempts == 1) throw new InvalidOperationException("once");
            lodsRestored = true;
        });
        journal.Add("collider", () => colliderRestores++);
        journal.Add("mesh", () => meshReleases++, () => lodsRestored);
        Assert.False(journal.Run((_, _) => { }));
        Assert.Equal(0, meshReleases);
        Assert.Equal(1, colliderRestores);
        Assert.True(journal.Run((_, _) => { }));
        Assert.Equal(1, meshReleases);
        Assert.Equal(1, colliderRestores);
    }

    [Fact]
    public void FailedLodRestoreCannotSkipOriginalFlagsCollidersOrDoorRestoration()
    {
        var journal = new CleanupJournal();
        var restored = new List<string>();
        var failures = new List<string>();
        journal.Add("LOD", () => throw new InvalidOperationException("injected LOD failure"));
        foreach (var name in new[] { "original-flags", "colliders", "door-flags", "interaction", "owned-resources" })
            journal.Add(name, () => restored.Add(name));

        Assert.False(journal.Run((name, _) => failures.Add(name)));
        Assert.Equal(new[] { "original-flags", "colliders", "door-flags", "interaction", "owned-resources" }, restored);
        Assert.Equal(new[] { "LOD" }, failures);
        Assert.Equal(1, journal.PendingCount);
        Assert.False(journal.IsComplete);
    }

    [Fact]
    public void ASecondPassRetriesOnlyFailedStepsAndMarksCompletionAfterRecovery()
    {
        var journal = new CleanupJournal();
        int attempts = 0, colliderRestores = 0;
        journal.Add("LOD", () => { if (++attempts == 1) throw new InvalidOperationException("once"); });
        journal.Add("collider", () => colliderRestores++);
        Assert.False(journal.Run((_, _) => { }));
        Assert.Equal(1, colliderRestores);
        Assert.True(journal.Run((_, _) => { }));
        Assert.True(journal.IsComplete);
        Assert.Equal(2, attempts);
        Assert.Equal(1, colliderRestores);
        Assert.True(journal.Run((_, _) => throw new Exception("not called")));
        Assert.Equal(2, attempts);
    }

    [Fact]
    public void DiagnosticFailureCannotAbortOtherCleanupSteps()
    {
        var journal = new CleanupJournal();
        bool restored = false;
        journal.Add("broken-renderer", () => throw new InvalidOperationException("renderer"));
        journal.Add("another-renderer", () => restored = true);
        Assert.False(journal.Run((_, _) => throw new InvalidOperationException("logger")));
        Assert.True(restored);
        Assert.Equal(1, journal.PendingCount);
    }
}

using Xunit;

namespace Minimap.Tests;

public sealed class SearchControllerRegressionTests
{
    private static SearchObservation Observation(string world, bool? collected, bool ready = true)
        => new(ready, collected, new SearchIdentity(world, "key"), true, 0, 0, 0);

    private static SearchController Started()
    {
        var c = new SearchController(new SearchArea("area", 1, 0, 0, 0, 40, 10, 5));
        c.Enable();
        c.Observe(Observation("A", false));
        c.Accept();
        return c;
    }

    [Fact]
    public void CompletedWorldCannotLeakToastOrCompletedStateIntoAnotherWorld()
    {
        var c = Started();
        c.Observe(Observation("A", true));
        Assert.True(c.Snapshot.HasCompletion);
        c.Observe(Observation("B", false));
        Assert.False(c.TryConsumeCompletion(out _));
        c.Accept();
        c.Observe(Observation("B", true));
        Assert.True(c.TryConsumeCompletion(out var signal));
        Assert.Equal("B", signal.World);
    }

    [Fact]
    public void UnknownReadSuspendsCompletedPresentationAndPendingToast()
    {
        var c = Started();
        c.Observe(Observation("A", true));
        c.Observe(SearchObservation.Unknown);
        Assert.Equal(SearchPhase.Checking, c.Snapshot.Phase);
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void KnownWorldWithUnknownCollectedStateDoesNotShowStaleFound()
    {
        var c = Started();
        c.Observe(Observation("A", true));
        c.Observe(Observation("A", null));
        Assert.Equal(SearchPhase.Checking, c.Snapshot.Phase);
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void FalseRollbackReopensAcceptedSearchAndDropsPriorCompletion()
    {
        var c = Started();
        c.Observe(Observation("A", true));
        c.Observe(Observation("A", false));
        Assert.Equal(SearchPhase.Searching, c.Snapshot.Phase);
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void CancelledIntentStaysCancelledAcrossActualLoadBoundary()
    {
        var c = Started();
        c.Cancel();
        var record = c.CurrentRecord!;
        c.OnLoad();
        c.Observe(Observation("A", false));
        Assert.True(c.Restore(record));
        Assert.Equal(SearchPhase.Idle, c.Snapshot.Phase);
        Assert.False(c.Snapshot.Accepted);
        c.Observe(Observation("A", true));
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void AcceptedIntentReopensOlderUncollectedWorldWithoutStoredCompletedFlag()
    {
        var c = Started();
        var record = c.CurrentRecord!;
        c.Observe(Observation("A", true));
        c.OnLoad();
        c.Observe(Observation("A", false));
        Assert.True(c.Restore(record));
        Assert.Equal(SearchPhase.Searching, c.Snapshot.Phase);
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void RestoreRequiresCurrentReliableWorldNotStaleContext()
    {
        var c = Started();
        var record = c.CurrentRecord!;
        c.Observe(SearchObservation.Unknown);
        Assert.False(c.Restore(record));
    }

    [Fact]
    public void RestoreCannotBypassTargetCoverageOrInvalidArea()
    {
        var c = new SearchController(new SearchArea("area", 1, 0, 0, 0, 40, 10, 5));
        c.Enable();
        c.Observe(new SearchObservation(true, false, new SearchIdentity("A", "key"), true, 100, 0, 0));
        Assert.False(c.Restore(new SearchRecord("A", "key", "area", 1, true)));
    }

    [Fact]
    public void ConsumingCompletionChangesSnapshotRevision()
    {
        var c = Started();
        c.Observe(Observation("A", true));
        var before = c.Snapshot;
        Assert.True(c.TryConsumeCompletion(out _));
        Assert.False(c.Snapshot.HasCompletion);
        Assert.True(c.Snapshot.Revision > before.Revision);
    }

    [Fact]
    public void DisablingRetainsAcceptedIntentButRequiresFreshMatchingObservation()
    {
        var c = Started();
        c.Disable();
        c.Enable();
        Assert.Equal(SearchPhase.Checking, c.Snapshot.Phase);
        c.Observe(Observation("A", false));
        Assert.True(c.Snapshot.Accepted);
        Assert.Equal(SearchPhase.Searching, c.Snapshot.Phase);
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void ReenableCannotRestoreForeignWorldIntent()
    {
        var c = Started();
        c.Disable();
        c.Enable();
        c.Observe(Observation("B", false));
        Assert.False(c.Snapshot.Accepted);
        Assert.Null(c.CurrentRecord);
    }

    [Fact]
    public void CancelledIntentRemainsCancelledAfterDisableAndReenable()
    {
        var c = Started();
        c.Cancel();
        c.Disable();
        c.Enable();
        c.Observe(Observation("A", false));
        Assert.False(c.Snapshot.Accepted);
        Assert.NotNull(c.CurrentRecord);
        Assert.False(c.CurrentRecord!.Accepted);
    }

    [Fact]
    public void MissingUncollectedPickupIsAGapNotACompletionBaseline()
    {
        var c = Started();
        c.Observe(new SearchObservation(true, false, new SearchIdentity("A", "key"), false, 0, 0, 0));
        Assert.Equal(SearchPhase.Checking, c.Snapshot.Phase);
        c.Observe(Observation("A", true));
        Assert.Equal(SearchPhase.AlreadyFound, c.Snapshot.Phase);
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void UnsupportedCoverageIsUnavailableRatherThanAnActiveSearch()
    {
        var c = Started();
        c.Observe(new SearchObservation(true, false, new SearchIdentity("A", "key"), true, 100, 0, 0));
        Assert.Equal(SearchPhase.Unavailable, c.Snapshot.Phase);
        c.Observe(Observation("A", true));
        Assert.False(c.TryConsumeCompletion(out _));
    }

    [Fact]
    public void InvalidGeometryCannotContainAPlayer()
    {
        var invalid = new SearchArea("", 1, 0, 0, 0, 40, 10, 5);
        Assert.False(invalid.Contains(0, 0, 0));
        Assert.False(invalid.ContainsTarget(0, 0, 0));
    }

    [Fact]
    public void FiniteLargeCoordinatesDoNotPassThroughInfinitySquaredComparison()
    {
        var area = new SearchArea("large", 1, float.MaxValue, 0, 0, float.MaxValue / 2, 10, 5);
        Assert.True(area.IsValid);
        Assert.False(area.Contains(-float.MaxValue, 0, 0));
    }
}

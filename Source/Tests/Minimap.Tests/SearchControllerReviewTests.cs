using Xunit;

namespace Minimap.Tests;

public sealed class SearchControllerReviewTests
{
    private static SearchController Started()
    {
        var c = new SearchController(new SearchArea("area", 1, 0, 0, 0, 40, 10, 5));
        c.Enable();
        c.Observe(Observation(false));
        c.Accept();
        return c;
    }

    private static SearchObservation Observation(bool collected)
        => new(true, collected, new SearchIdentity("world", "key"), true, 0, 0, 0);

    [Fact]
    public void RetainedRecordRemainsReadableAcrossLoadAndDisableBoundaries()
    {
        var c = Started();
        var record = c.CurrentRecord;
        c.OnLoad();
        Assert.Null(c.CurrentRecord);
        Assert.Same(record, c.RetainedRecord);
        c.Disable();
        c.Enable();
        Assert.Same(record, c.RetainedRecord);
        Assert.False(c.Snapshot.Accepted);
    }

    [Fact]
    public void PendingIntentCanBeCancelledBeforeReobservation()
    {
        var c = Started();
        c.OnLoad();
        c.Cancel();
        c.Observe(Observation(false));
        Assert.False(c.Snapshot.Accepted);
        Assert.False(c.RetainedRecord!.Accepted);
    }

    [Fact]
    public void CancelAfterObservedCollectionDiscardsPendingCelebrationNotWorldFact()
    {
        var c = Started();
        c.Observe(Observation(true));
        Assert.True(c.Snapshot.HasCompletion);
        c.Cancel();
        Assert.False(c.TryConsumeCompletion(out _));
        Assert.False(c.Snapshot.Accepted);
        Assert.Equal(SearchPhase.AlreadyFound, c.Snapshot.Phase);
        Assert.False(c.RetainedRecord!.Accepted);
    }

    [Fact]
    public void RestoringCancelledRecordClearsAlreadyPendingCelebration()
    {
        var c = Started();
        c.Observe(Observation(true));
        Assert.True(c.Restore(new SearchRecord("world", "key", "area", 1, false)));
        Assert.False(c.TryConsumeCompletion(out _));
        Assert.Equal(SearchPhase.AlreadyFound, c.Snapshot.Phase);
    }

    [Fact]
    public void FailedConsumptionDoesNotExposePreviousSignal()
    {
        var c = Started();
        c.Observe(Observation(true));
        Assert.True(c.TryConsumeCompletion(out _));
        Assert.False(c.TryConsumeCompletion(out var exhausted));
        Assert.Null(exhausted.World);
        Assert.Null(exhausted.Target);
    }
}

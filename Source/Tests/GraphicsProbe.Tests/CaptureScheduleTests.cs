using GraphicsProbe;
using Xunit;

public class CaptureScheduleTests
{
    [Fact]
    public void DelaysAndCoalescesReasonsWithoutStarvingOriginalDeadline()
    {
        var s = new CaptureSchedule(3, 5);
        s.Request(10, "startup");
        s.Request(12, "scene");
        Assert.False(s.TryBegin(12.99, out _));
        Assert.True(s.TryBegin(13, out var reason));
        Assert.Equal("startup,scene", reason);
        Assert.False(s.TryBegin(20, out _));
    }

    [Fact]
    public void RepeatedReasonIsDeduplicatedAndRunningRequestsAreDeferred()
    {
        var s = new CaptureSchedule(2, 5);
        s.Request(0, "manual"); s.Request(1, "manual");
        Assert.True(s.TryBegin(2, out var reason));
        Assert.Equal("manual", reason);
        s.Request(2.1, "scene");
        Assert.False(s.TryBegin(10, out _));
        s.Complete(3);
        Assert.False(s.TryBegin(7.99, out _));
        Assert.True(s.TryBegin(8, out reason));
        Assert.Equal("scene", reason);
    }

    [Fact]
    public void FailedCaptureCompletionDoesNotLatchAndFreshRequestCanRetry()
    {
        var s = new CaptureSchedule(1, 5);
        s.Request(0, "manual"); Assert.True(s.TryBegin(1, out _));
        // Same finally-completion path for a thrown capture and a failed file write.
        s.Complete(2); s.Request(2.1, "manual");
        Assert.False(s.TryBegin(6.99, out _));
        Assert.True(s.TryBegin(7, out _));
    }

    [Fact]
    public void ShutdownCancelsPendingRunningAndFutureWork()
    {
        var s = new CaptureSchedule(1, 5);
        s.Request(0, "startup"); s.Shutdown(); s.Request(100, "manual"); s.Complete(101);
        Assert.False(s.TryBegin(1000, out _));
    }

    [Fact]
    public void CompletionDoesNotCreatePeriodicWork()
    {
        var s = new CaptureSchedule(0, 5);
        s.Request(0, "manual"); Assert.True(s.TryBegin(0, out _)); s.Complete(1);
        Assert.False(s.TryBegin(100000, out _));
    }
}

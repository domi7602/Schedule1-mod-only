using GraphicsProbe;
using Xunit;

public class CooperativeCaptureTests
{
    private sealed class Work : ISteppedCapture
    {
        public int Calls, Releases;
        public int Remaining = 1000;
        public Action? Tick;
        public bool Throw;
        public bool Advance() { Calls++; Tick?.Invoke(); if (Throw) throw new InvalidOperationException("stale"); return --Remaining > 0; }
        public void Dispose() => Releases++;
    }

    [Fact]
    public void HardStepBoundAndCooperativeSlicePreventSynchronousFullCollection()
    {
        double now = 0;
        var work = new Work();
        using var run = new CooperativeCapture(work, () => now, maxSteps: 3);
        run.StepFrame();
        Assert.Equal(3, work.Calls);
        Assert.Equal("running", run.Status);
        Assert.Equal(1, run.Frames);
    }

    [Fact]
    public void SlowNativeLikeCallCanOverrunOneSliceButNoSecondCallRuns()
    {
        double now = 0;
        var work = new Work { Tick = () => now += 7 };
        using var run = new CooperativeCapture(work, () => now);
        run.StepFrame();
        Assert.Equal(1, work.Calls);
        Assert.Equal(7, run.ActiveMilliseconds);
        Assert.Equal(7, run.MaxObservedStepMilliseconds);
    }

    [Fact]
    public void CompletionCancellationFailureAndTimeoutReleaseExactlyOnceAndDoNoIdleWork()
    {
        foreach (string mode in new[] { "complete", "cancel", "failure", "active", "wall" })
        {
            double now = 0;
            var work = new Work { Remaining = mode == "complete" ? 1 : 100, Throw = mode == "failure" };
            using var run = new CooperativeCapture(work, () => now, activeBudget: 4, wallDeadline: 20);
            if (mode == "cancel") run.Cancel();
            if (mode == "wall") now = 21;
            if (mode == "active") work.Tick = () => now += 5;
            run.StepFrame();
            int calls = work.Calls;
            run.StepFrame(); run.Cancel(); run.Dispose();
            Assert.Equal(calls, work.Calls);
            Assert.Equal(1, work.Releases);
            Assert.Equal(mode switch { "complete" => "completed", "cancel" => "cancelled", "failure" => "failed", "active" => "active-budget", _ => "wall-deadline" }, run.Status);
        }
    }

    [Fact]
    public void RepeatedRequestsCoalesceAndCancelledCollectionCanRescheduleWithoutOverlap()
    {
        double now = 0;
        var schedule = new CaptureSchedule(0, 0);
        schedule.Request(0, "manual");
        Assert.True(schedule.TryBegin(0, out _));
        var first = new Work();
        using var active = new CooperativeCapture(first, () => now, maxSteps: 1);
        active.StepFrame();
        schedule.Request(0, "manual"); schedule.Request(0, "manual");
        Assert.False(schedule.TryBegin(10, out _));
        active.Cancel(); schedule.Complete(0);
        schedule.Request(0, "scene-loaded");
        Assert.True(schedule.TryBegin(0, out var reason));
        Assert.Equal("manual,scene-loaded", reason);
        Assert.Equal(1, first.Releases);
        var second = new Work { Remaining = 1 };
        using var next = new CooperativeCapture(second, () => now);
        active.StepFrame(); next.StepFrame(); schedule.Complete(0);
        Assert.Equal(1, first.Calls); Assert.Equal(1, second.Calls);
        Assert.Equal(1, second.Releases);
        Assert.False(schedule.TryBegin(100, out _));
    }

    [Fact]
    public void FrameGapConsumesWallButNotActiveBudget()
    {
        double now = 0;
        var work = new Work { Tick = () => now += 1 };
        using var run = new CooperativeCapture(work, () => now, maxSteps: 1);
        run.StepFrame(); now += 100; run.StepFrame();
        Assert.Equal(2, run.ActiveMilliseconds);
        Assert.Equal(102, run.WallMilliseconds);
        Assert.Equal(2, run.Steps);
    }
}

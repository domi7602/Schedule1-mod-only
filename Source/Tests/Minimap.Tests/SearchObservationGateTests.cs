using Xunit;

namespace Minimap.Tests;

public sealed class SearchObservationGateTests
{
    [Fact]
    public void DisabledByDefaultAndSamplesOnlyAfterExplicitStart()
    {
        var gate = new SearchObservationGate();
        Assert.False(gate.ShouldSample(0));
        gate.Start(10);
        Assert.True(gate.ShouldSample(10));
        Assert.False(gate.ShouldSample(10.1));
        Assert.True(gate.ShouldSample(10.25));
    }

    [Fact]
    public void SameWorldLoadDiscardsPreviousObservationComparison()
    {
        var gate = new SearchObservationGate();
        gate.Start(0);
        Assert.True(gate.ShouldReport("uncollected"));
        Assert.False(gate.ShouldReport("uncollected"));
        gate.ResetEpoch();
        Assert.Equal(1, gate.Epoch);
        Assert.True(gate.ShouldReport("uncollected"));
    }

    [Fact]
    public void WatchExpiresEvenWithNoStateChanges()
    {
        var gate = new SearchObservationGate();
        gate.Start(0);
        Assert.False(gate.ShouldSample(SearchObservationGate.DurationSeconds));
        Assert.False(gate.Enabled);
    }

    [Fact]
    public void MaximumReportsStopTheWatchWithoutUnboundedLogging()
    {
        var gate = new SearchObservationGate();
        gate.Start(0);
        for (int i = 0; i < SearchObservationGate.MaxReports; i++)
            Assert.True(gate.ShouldReport(i.ToString()));
        Assert.False(gate.ShouldReport("overflow"));
        Assert.False(gate.Enabled);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(-1)]
    public void InvalidClockCannotEnableWatch(double now)
    {
        var gate = new SearchObservationGate();
        gate.Start(now);
        Assert.False(gate.Enabled);
    }

    [Fact]
    public void LifecycleBoundariesRespectDeadlineButDoNotCompeteWithSampleCadence()
    {
        var gate = new SearchObservationGate();
        gate.Start(0);
        Assert.True(gate.ShouldSample(0));
        Assert.True(gate.CanObserve(0.1));
        Assert.False(gate.ShouldSample(0.1));
        Assert.False(gate.CanObserve(SearchObservationGate.DurationSeconds));
        Assert.False(gate.Enabled);
    }

    [Fact]
    public void ExpiryDetectedOutsideTickRemainsReportableExactlyOnce()
    {
        var gate = new SearchObservationGate();
        gate.Start(0);
        Assert.False(gate.CanObserve(SearchObservationGate.DurationSeconds));
        Assert.True(gate.TryConsumeAutoStop());
        Assert.False(gate.TryConsumeAutoStop());
    }

    [Fact]
    public void ManualStopOrRestartCannotCarryAnObsoleteAutoStopNotice()
    {
        var gate = new SearchObservationGate();
        gate.Start(0);
        gate.Stop();
        Assert.False(gate.TryConsumeAutoStop());
        gate.Start(0);
        gate.CanObserve(SearchObservationGate.DurationSeconds);
        gate.Start(1);
        Assert.False(gate.TryConsumeAutoStop());
    }

    [Fact]
    public void StopReleasesOldComparisonAndRestartGetsFreshBudget()
    {
        var gate = new SearchObservationGate();
        gate.Start(0);
        Assert.True(gate.ShouldReport("baseline"));
        gate.Stop();
        Assert.False(gate.ShouldReport("late callback"));
        gate.Start(1);
        Assert.Equal(0, gate.Reports);
        Assert.True(gate.ShouldReport("baseline"));
    }
}

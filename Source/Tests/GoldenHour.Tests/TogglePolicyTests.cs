using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class TogglePolicyTests
{
    [Fact]
    public void Starts_idle_without_snapshot()
    {
        var policy = new TogglePolicy();
        Assert.False(policy.IsActive);
        Assert.False(policy.SnapshotNeeded);
    }

    [Fact]
    public void Toggle_from_idle_activates_and_requests_snapshot()
    {
        var policy = new TogglePolicy();
        Assert.True(policy.Toggle());
        Assert.True(policy.IsActive);
        Assert.True(policy.SnapshotNeeded);
    }

    [Fact]
    public void Toggle_from_active_deactivates()
    {
        var policy = new TogglePolicy();
        policy.Toggle();
        Assert.False(policy.Toggle());
        Assert.False(policy.IsActive);
        Assert.False(policy.SnapshotNeeded);
    }

    [Fact]
    public void Re_activation_requests_a_fresh_snapshot_again()
    {
        var policy = new TogglePolicy();
        policy.Toggle();
        policy.MarkSnapshotTaken();
        Assert.False(policy.SnapshotNeeded);
        policy.Toggle(); // off
        policy.Toggle(); // on again
        Assert.True(policy.IsActive);
        Assert.True(policy.SnapshotNeeded);
    }

    [Fact]
    public void Double_toggle_within_active_cycle_is_deactivation_not_reapply()
    {
        var policy = new TogglePolicy();
        policy.Toggle();
        policy.MarkSnapshotTaken();
        Assert.False(policy.Toggle());
    }
}

using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class ReapplyPolicyTests
{
    [Fact]
    public void Is_due_before_any_apply()
    {
        var policy = new ReapplyPolicy(1.0);
        Assert.True(policy.IsDue(100.0));
    }

    [Fact]
    public void Not_due_within_interval()
    {
        var policy = new ReapplyPolicy(1.0);
        policy.MarkApplied(100.0);
        Assert.False(policy.IsDue(100.5));
    }

    [Fact]
    public void Due_after_interval_elapsed()
    {
        var policy = new ReapplyPolicy(1.0);
        policy.MarkApplied(100.0);
        Assert.True(policy.IsDue(101.1));
    }

    [Fact]
    public void Interval_is_clamped_to_minimum_of_zero_point_five_seconds()
    {
        var policy = new ReapplyPolicy(0.01); // spam guard: clamp to 0.5
        policy.MarkApplied(100.0);
        Assert.False(policy.IsDue(100.3));
        Assert.True(policy.IsDue(100.6));
    }
}

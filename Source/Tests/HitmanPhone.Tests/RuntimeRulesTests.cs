using Xunit;

namespace HitmanPhone.Tests;

public class RuntimeRulesTests
{
    [Theory]
    [InlineData(0, false, 1)]
    [InlineData(1, false, 1)]
    [InlineData(2, false, 2)]
    [InlineData(3, false, 3)]
    [InlineData(4, false, 4)]
    [InlineData(0, true, 4)]
    [InlineData(4, true, 4)]
    public void AcceptNeverLowersExistingOrRequiredPursuit(int current, bool drop, int expected)
        => Assert.Equal(expected, BountyRuntimeRules.AcceptedPursuit(current, drop));

    [Fact]
    public void TimeSnapshotRejectsADayTransitionInsteadOfMixingDayAndClock()
    {
        int reads = 0;
        Assert.False(BountyRuntimeRules.TryTimeSnapshot(() => reads++ == 0 ? 3 : 4, () => 0, out _, out _, out _));
    }

    [Fact]
    public void TimeSnapshotReturnsOneConsistentValidMinute()
    {
        Assert.True(BountyRuntimeRules.TryTimeSnapshot(() => 3, () => 123, out int day, out int minute, out long sum));
        Assert.Equal(3, day);
        Assert.Equal(123, minute);
        Assert.Equal(4443L, sum);
    }

    [Fact]
    public void UnreadableClockIsNotRealDayZero()
    {
        Assert.False(BountyRuntimeRules.TryTimeSnapshot(() => throw new InvalidOperationException(), () => 0,
            out int day, out int minute, out long sum));
        Assert.Equal(-1, day);
        Assert.Equal(-1, minute);
        Assert.Equal(-1L, sum);
    }
}

using Xunit;

namespace GoldenHour.Tests;

public class LightingChannelTests
{
    private sealed class Target
    {
        public Argb Value = new(0.6f, 0.7f, 0.8f, 0.9f);
        public bool Alive = true;
        public bool Readable = true;
        public bool FailBefore;
        public bool FailAfter;
        public bool IgnoreWrite;
        public Func<Argb, Argb>? NativeTransform;
        public int Writes;
        public int Reads;
        public readonly List<string> Errors = new();

        public LightingChannel Channel() => new(() => Alive, () =>
        {
            Reads++;
            return Readable ? Value : null;
        }, value =>
        {
            Writes++;
            if (FailBefore) throw new InvalidOperationException("before setter");
            if (!IgnoreWrite) Value = NativeTransform?.Invoke(value) ?? value;
            if (FailAfter) throw new InvalidOperationException("after setter");
        }, GoldenHourTargets.SunLightTint, Errors.Add);
    }

    [Fact]
    public void Native_setter_normalization_is_recorded_instead_of_re_adopting_our_tint()
    {
        var target = new Target
        {
            NativeTransform = color => color with { R = (float)Math.Round(color.R, 4) },
        };
        Argb native = target.Value;
        LightingChannel channel = target.Channel();
        Assert.True(channel.Apply(0.73));
        Argb firstApplied = target.Value;
        for (int frame = 0; frame < 100; frame++) Assert.True(channel.Apply(0.73));
        Assert.Equal(firstApplied, target.Value);
        Assert.True(channel.Release(0));
        Assert.Equal(native, target.Value);
    }

    [Fact]
    public void A_setter_that_normalizes_then_throws_is_restored_from_immediate_readback()
    {
        var target = new Target
        {
            NativeTransform = color => color with { R = (float)Math.Round(color.R, 4) },
            FailAfter = true,
        };
        Argb native = target.Value;
        LightingChannel channel = target.Channel();
        Assert.False(channel.Apply(0.73));
        target.FailAfter = false;
        Assert.True(channel.Release(0));
        Assert.Equal(native, target.Value);
    }

    [Fact]
    public void Repeated_strength_changes_use_the_exact_last_attempt_not_a_global_strength()
    {
        var target = new Target();
        Argb native = target.Value;
        LightingChannel channel = target.Channel();
        foreach (double strength in new[] { 1.0, 0.2, 0.8, 0.0, 0.7, 1.0 })
        {
            for (int frame = 0; frame < 20; frame++) Assert.True(channel.Apply(strength));
            Assert.Equal(BlendPolicy.Blend(native, GoldenHourTargets.SunLightTint, strength), target.Value);
        }
        Assert.True(channel.Release(0));
        Assert.Equal(native, target.Value);
        int writes = target.Writes;
        Assert.True(channel.Release(1));
        Assert.Equal(writes, target.Writes);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void A_throw_before_or_after_setter_keeps_the_correct_restore_obligation(bool after)
    {
        var target = new Target { FailAfter = after, FailBefore = !after };
        Argb native = target.Value;
        LightingChannel channel = target.Channel();
        Assert.False(channel.Apply(1));
        Assert.True(channel.Faulted);
        Assert.Equal(after, channel.HasPendingWrite);
        Assert.False(channel.Apply(0.5));
        Assert.Equal(1, target.Writes);

        target.FailBefore = target.FailAfter = false;
        Assert.True(channel.Release(0));
        Assert.Equal(native, target.Value);
        Assert.Equal(after ? 2 : 1, target.Writes);
    }

    [Fact]
    public void A_restore_that_mutates_then_throws_is_reconciled_without_second_write()
    {
        var target = new Target();
        Argb native = target.Value;
        LightingChannel channel = target.Channel();
        Assert.True(channel.Apply(1));
        target.FailAfter = true;
        Assert.False(channel.Release(0));
        Assert.Equal(native, target.Value);
        Assert.True(channel.Release(1));
        Assert.Equal(2, target.Writes);
    }

    [Fact]
    public void Silent_restore_noop_retains_the_lease_and_caps_native_write_attempts()
    {
        var target = new Target();
        LightingChannel channel = target.Channel();
        Assert.True(channel.Apply(1));
        target.IgnoreWrite = true;
        for (int second = 0; second < 20; second++) Assert.False(channel.Release(second));
        Assert.Equal(1 + LightingChannel.MaximumRestoreAttempts, target.Writes);
        Assert.True(channel.HasPendingWrite);
        Assert.False(channel.Apply(1));

        // Even after the write budget is spent, native drift can discharge the lease.
        target.Value = new Argb(0.4f, 0.5f, 0.6f, 1f);
        Assert.True(channel.Release(21));
        Assert.Equal(4, target.Writes);
    }

    [Fact]
    public void Transient_unreadability_does_not_spend_restore_write_budget()
    {
        var target = new Target();
        Argb native = target.Value;
        LightingChannel channel = target.Channel();
        Assert.True(channel.Apply(1));
        target.Readable = false;
        for (int second = 0; second < 10; second++) Assert.False(channel.Release(second));
        Assert.Equal(1, target.Writes);
        target.Readable = true;
        Assert.True(channel.Release(10));
        Assert.Equal(native, target.Value);
    }

    [Fact]
    public void Retry_is_time_bounded_and_does_not_write_to_a_dead_target()
    {
        var target = new Target();
        LightingChannel channel = target.Channel();
        Assert.True(channel.Apply(1));
        target.FailBefore = true;
        Assert.False(channel.Release(10));
        for (int frame = 0; frame < 50; frame++) Assert.False(channel.Release(10.1));
        Assert.Equal(2, target.Writes);
        target.Alive = false;
        Assert.True(channel.Release(10.2));
        Assert.Equal(2, target.Writes);
    }

    [Fact]
    public void Even_a_tiny_foreign_write_wins_over_restore()
    {
        var target = new Target();
        LightingChannel channel = target.Channel();
        Assert.True(channel.Apply(1));
        target.Value = target.Value with { R = target.Value.R + 0.00001f };
        Argb foreign = target.Value;
        Assert.True(channel.Release(0));
        Assert.Equal(foreign, target.Value);
        Assert.Equal(1, target.Writes);
    }

    [Fact]
    public void A_late_readable_channel_snapshots_before_its_first_write()
    {
        var target = new Target { Readable = false };
        Argb native = target.Value;
        LightingChannel channel = target.Channel();
        Assert.False(channel.Apply(1));
        Assert.False(channel.HasPendingWrite);
        target.Readable = true;
        Assert.True(channel.Apply(1));
        Assert.True(channel.Release(0));
        Assert.Equal(native, target.Value);
    }

    [Fact]
    public void Invalid_native_value_is_not_replaced_with_an_invented_baseline()
    {
        var target = new Target { Value = new Argb(float.NaN, 1f, 1f, 1f) };
        LightingChannel channel = target.Channel();
        Assert.False(channel.Apply(1));
        Assert.True(channel.Release(0));
        Assert.Equal(0, target.Writes);
    }
}

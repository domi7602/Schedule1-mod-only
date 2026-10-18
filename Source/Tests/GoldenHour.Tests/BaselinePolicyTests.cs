using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class BaselinePolicyTests
{
    private static readonly Argb Tint = new(1.15f, 0.97f, 0.72f, 1f);

    [Fact]
    public void Evaluate_first_apply_writes_blended_from_baseline()
    {
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var decision = BaselinePolicy.Evaluate(baseline, baseline, Tint, 1.0);

        Assert.True(decision.Write);
        Assert.Equal(baseline, decision.Baseline);
        Assert.Equal(1.035f, decision.Blended.R, 4);
        Assert.Equal(0.776f, decision.Blended.G, 4);
        Assert.Equal(0.504f, decision.Blended.B, 4);
    }

    [Fact]
    public void Evaluate_adopts_game_drift_as_new_baseline()
    {
        // The game rewrote the value since our last write (day/night drift):
        // the bias must rebase onto the game's current value instead of
        // re-blending the stale activation snapshot (which would freeze it).
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var drifted = new Argb(0.95f, 0.85f, 0.75f, 1f);
        var decision = BaselinePolicy.Evaluate(baseline, drifted, Tint, 1.0);

        Assert.True(decision.Write);
        Assert.Equal(drifted, decision.Baseline);
        Assert.Equal(0.95f * 1.15f, decision.Blended.R, 4);
        Assert.Equal(0.85f * 0.97f, decision.Blended.G, 4);
        Assert.Equal(0.75f * 0.72f, decision.Blended.B, 4);
    }

    [Fact]
    public void Evaluate_keeps_baseline_when_current_is_our_own_write()
    {
        // current == blended: the game did not overwrite us, so the baseline
        // stays and no redundant write happens (no compounding).
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var own = BlendPolicy.Blend(baseline, Tint, 1.0);
        var decision = BaselinePolicy.Evaluate(baseline, own, Tint, 1.0);

        Assert.False(decision.Write);
        Assert.Equal(baseline, decision.Baseline);
    }

    [Fact]
    public void Evaluate_non_finite_current_never_writes()
    {
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var bad = new Argb(float.NaN, 0.8f, 0.7f, 1f);
        var decision = BaselinePolicy.Evaluate(baseline, bad, Tint, 1.0);

        Assert.False(decision.Write);
        Assert.Equal(baseline, decision.Baseline);
    }

    [Fact]
    public void Evaluate_non_finite_tint_never_writes()
    {
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var badTint = new Argb(float.PositiveInfinity, 0.97f, 0.72f, 1f);
        var decision = BaselinePolicy.Evaluate(baseline, baseline, badTint, 1.0);

        Assert.False(decision.Write);
    }

    [Fact]
    public void Evaluate_coincidental_equality_treated_as_own_write_is_benign()
    {
        // If the game happens to write exactly our blended value, the decision is
        // "own write": no write, baseline unchanged. The screen already shows the
        // intended biased value, and the next drift beyond tolerance self-heals.
        // The value is a literal here (not produced by BlendPolicy) to mirror the
        // game independently arriving at it.
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var coincidental = new Argb(1.035f, 0.776f, 0.504f, 1f);
        var decision = BaselinePolicy.Evaluate(baseline, coincidental, Tint, 1.0);

        Assert.False(decision.Write);
        Assert.Equal(baseline, decision.Baseline);
    }

    [Fact]
    public void Evaluate_multi_frame_sequence_never_compounds()
    {
        // The no-compounding invariant across an adopt→write→re-read cycle:
        // frame 1 adopts the game value and writes tint once; frame 2 sees our own
        // write and skips; frame 3 adopts the next game drift and writes tint once
        // again — the game value never gets tinted twice.
        var gameValue = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var baseline = gameValue;

        var first = BaselinePolicy.Evaluate(baseline, gameValue, Tint, 1.0);
        Assert.True(first.Write);
        baseline = first.Baseline;

        var second = BaselinePolicy.Evaluate(baseline, first.Blended, Tint, 1.0);
        Assert.False(second.Write);
        Assert.Equal(baseline, second.Baseline);

        var drifted = new Argb(0.92f, 0.82f, 0.72f, 1f);
        var third = BaselinePolicy.Evaluate(baseline, drifted, Tint, 1.0);
        Assert.True(third.Write);
        Assert.Equal(drifted, third.Baseline);
        Assert.Equal(0.92f * 1.15f, third.Blended.R, 4); // tint applied exactly once
    }

    [Fact]
    public void Evaluate_zero_strength_adopts_drift_and_writes_current()
    {
        // strength 0 = pass-through: the blended value equals the baseline, so a
        // drifted current means the game value is adopted and written unchanged.
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var drifted = new Argb(0.95f, 0.85f, 0.75f, 1f);
        var decision = BaselinePolicy.Evaluate(baseline, drifted, Tint, 0.0);

        Assert.True(decision.Write);
        Assert.Equal(drifted, decision.Baseline);
        Assert.Equal(drifted, decision.Blended);
    }

    [Fact]
    public void Evaluate_strength_drop_releases_own_write_without_adopting_it()
    {
        // A clock-driven strength change must not treat our previous warm write as
        // game drift. Adopting it would freeze the gold at noon and restore a tint.
        var baseline = new Argb(0.9f, 0.8f, 0.7f, 1f);
        var own = BlendPolicy.Blend(baseline, Tint, 1.0);
        var decision = BaselinePolicy.Evaluate(baseline, own, Tint, 0.0, 1.0);

        Assert.True(decision.Write);
        Assert.Equal(baseline, decision.Baseline);
        Assert.Equal(baseline, decision.Blended);
    }
}

namespace GoldenHour;

/// <summary>Outcome of one reapply decision for a single parameter key.</summary>
public readonly struct BaselineDecision
{
    public BaselineDecision(Argb baseline, Argb blended, bool write)
    {
        Baseline = baseline;
        Blended = blended;
        Write = write;
    }

    /// <summary>
    /// The baseline the next reapply should blend from. A strength change writes the new
    /// blend but keeps this baseline. Only adopted game drift replaces it.
    /// </summary>
    public Argb Baseline { get; }

    /// <summary>The value to write (only meaningful when <see cref="Write"/> is true).</summary>
    public Argb Blended { get; }

    /// <summary>True when the value must be written to the scene.</summary>
    public bool Write { get; }
}

/// <summary>
/// Reapply decision for one parameter: the game's day/night cycle rewrites
/// sun/ambient/fog state continuously, so a reapply must rebase onto the game's
/// current value instead of re-blending the stale activation snapshot — otherwise
/// the bias freezes the parameter at its toggle-time value. Own previous writes
/// are detected by value, including the blend at <c>previousStrength</c>, and are
/// never re-adopted. Without that, a clock-driven strength drop freezes the old tint.
/// </summary>
public static class BaselinePolicy
{
    public static BaselineDecision Evaluate(
        Argb baseline, Argb current, Argb tint, double strength, double previousStrength = double.NaN)
    {
        if (!current.IsFinite || !tint.IsFinite) return new BaselineDecision(baseline, default, false);

        if (!double.IsFinite(previousStrength)) previousStrength = strength;

        var blended = BlendPolicy.Blend(baseline, tint, strength);
        if (!blended.IsFinite) return new BaselineDecision(baseline, default, false);

        if (current.IsCloseTo(blended))
        {
            // Still our own write at this strength: keep the baseline, skip the write.
            return new BaselineDecision(baseline, blended, false);
        }

        var previous = BlendPolicy.Blend(baseline, tint, previousStrength);
        if (previous.IsFinite && current.IsCloseTo(previous))
        {
            // Own write at the previous strength. Keep the game baseline and write
            // the new strength. Adopting current would freeze the old tint.
            return new BaselineDecision(baseline, blended, true);
        }

        // The game rewrote the value since our last write (day/night drift):
        // adopt its current value as the new baseline and blend from there.
        var rebased = BlendPolicy.Blend(current, tint, strength);
        if (!rebased.IsFinite) return new BaselineDecision(baseline, default, false);
        return new BaselineDecision(current, rebased, true);
    }
}

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
    /// The latest observed bias-free value, including native drift since our last write.
    /// </summary>
    public Argb Baseline { get; }

    /// <summary>The value to write (only meaningful when <see cref="Write"/> is true).</summary>
    public Argb Blended { get; }

    /// <summary>True when the value must be written to the scene.</summary>
    public bool Write { get; }
}

/// <summary>
/// The native game rewrites lighting continuously. Only an exact match with a value
/// we actually attempted to write belongs to us; even small native drift becomes the
/// new baseline. Inferring ownership from a newly calculated blend or a tolerance
/// would freeze native drift and misidentify unrelated colors as our own.
/// </summary>
public static class BaselinePolicy
{
    public static BaselineDecision Evaluate(
        Argb baseline, Argb current, Argb tint, double strength, Argb? lastWrite = null)
    {
        if (!current.IsFinite || !tint.IsFinite) return new BaselineDecision(baseline, default, false);

        bool ownsCurrent = lastWrite.HasValue && current == lastWrite.Value && baseline.IsFinite;
        Argb adopted = ownsCurrent ? baseline : current;
        Argb blended = BlendPolicy.Blend(adopted, tint, strength);
        return new BaselineDecision(adopted, blended, blended.IsFinite && blended != current);
    }
}

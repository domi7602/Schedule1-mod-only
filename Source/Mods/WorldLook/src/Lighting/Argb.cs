namespace GoldenHour;

/// <summary>Unity-free linear RGBA value (0..1 typical, HDR channels allowed &gt; 1).</summary>
public readonly record struct Argb(float R, float G, float B, float A)
{
    /// <summary>Default per-channel tolerance for "this is the value we already wrote".</summary>
    public const float DefaultTolerance = 0.0005f;

    public static Argb White { get; } = new(1f, 1f, 1f, 1f);

    public bool IsFinite =>
        float.IsFinite(R) && float.IsFinite(G) && float.IsFinite(B) && float.IsFinite(A);

    /// <summary>
    /// Per-channel closeness test (tolerance 0.0005 by default). Used to skip redundant
    /// writes and to detect that a value is still the one we wrote. Non-finite on either
    /// side is never "close" — a NaN must not be treated as unchanged.
    /// </summary>
    public bool IsCloseTo(Argb other, float tolerance = DefaultTolerance) =>
        IsFinite && other.IsFinite &&
        System.Math.Abs(R - other.R) < tolerance &&
        System.Math.Abs(G - other.G) < tolerance &&
        System.Math.Abs(B - other.B) < tolerance &&
        System.Math.Abs(A - other.A) < tolerance;
}

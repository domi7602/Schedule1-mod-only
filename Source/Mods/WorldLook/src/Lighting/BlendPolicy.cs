namespace GoldenHour;

/// <summary>
/// Pure blend math for the golden-hour bias: each channel multiplies the original by a
/// warm tint that fades in with strength (0 = original, 1 = full bias). Non-finite
/// original channels fall back to the tint value; non-finite tint channels act neutral.
/// </summary>
public static class BlendPolicy
{
    public static Argb Blend(Argb original, Argb tint, double strength)
    {
        double s = strength;
        if (!double.IsFinite(s)) s = 0.0;
        if (s < 0.0) s = 0.0; else if (s > 1.0) s = 1.0;

        return new Argb(
            Channel(original.R, tint.R, s),
            Channel(original.G, tint.G, s),
            Channel(original.B, tint.B, s),
            float.IsFinite(original.A) ? original.A : 1f);
    }

    private static float Channel(float original, float tint, double s)
    {
        if (!float.IsFinite(original)) return float.IsFinite(tint) ? tint : 0f;
        if (!float.IsFinite(tint)) return original;
        double mixed = 1.0 + s * ((double)tint - 1.0);
        double scaled = (double)original * mixed;
        if (!double.IsFinite(scaled)) return original;
        return (float)scaled;
    }
}

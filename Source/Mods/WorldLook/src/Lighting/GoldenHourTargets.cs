namespace GoldenHour;

/// <summary>
/// The golden-hour tint table (evidence-based: sun color is constant white across the
/// day cycle, so the warm look is a multiplicative bias — the day/night cycle stays in
/// control of magnitude, the mod only shifts hue). Production channels stay at or
/// above 0.90 and at or below 1.25: a warm shift, not the old gold wash.
/// </summary>
public static class GoldenHourTargets
{
    /// <summary>Sun directional light color bias.</summary>
    public static Argb SunLightTint { get; } = new(1.06f, 1.00f, 0.94f, 1f);

    /// <summary>SkyMat _SunShine HDR glow bias. Slightly warm; this used to be the visible gold.</summary>
    public static Argb SkySunGlowTint { get; } = new(1.04f, 0.97f, 0.90f, 1f);

    /// <summary>Ambient Trilight bias (gentlest — drives scene-wide base lighting).</summary>
    public static Argb AmbientTint { get; } = new(1.03f, 0.995f, 0.96f, 1f);

    /// <summary>Fog color bias. Unused while fog is off; kept only slightly warm.</summary>
    public static Argb FogTint { get; } = new(1.08f, 1.00f, 0.92f, 1f);

    /// <summary>GlobalVolume ColorAdjustments colorFilter bias (global warm grade).</summary>
    public static Argb VolumeTint { get; } = new(1.04f, 0.99f, 0.93f, 1f);

    /// <summary>
    /// Deliberately unmissable volume tint for the diagnostic switch only
    /// (<c>DiagnosticStrongTint</c>). Sits clearly outside the production bounds so a
    /// working URP path is obvious; never used for the normal look.
    /// </summary>
    public static Argb DiagnosticVolumeTint { get; } = new(1.60f, 0.60f, 0.30f, 1f);
}

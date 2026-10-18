namespace GoldenHour;

/// <summary>
/// URP volume visibility rule: a <c>VolumeComponent</c> only contributes when the
/// component is active, and a <c>VolumeParameter</c> only contributes when its
/// <c>overrideState</c> is set. Writing <c>colorFilter.value</c> while either flag is
/// off is a silent no-op — the tint never reaches the screen.
/// </summary>
public static class VolumeFlagPolicy
{
    /// <summary>
    /// True when the component/override flags must be forced so the tint can render.
    /// A zero (or negative) strength means "leave the volume exactly as we found it".
    /// </summary>
    public static bool NeedsEnable(double strength, bool componentActive, bool colorFilterOverride) =>
        strength > 0d && (!componentActive || !colorFilterOverride);
}

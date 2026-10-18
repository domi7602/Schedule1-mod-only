namespace GoldenHour;

/// <summary>
/// URP volume-profile instancing rule. Reading <c>Volume.profile</c> creates a runtime
/// clone (<c>m_InternalProfile</c>) on first access, and from then on the VolumeManager
/// blends from that clone instead of the shared profile asset.
///
/// That is only safe when the game itself already instantiated the profile — in that case
/// the clone IS the live profile the game writes into. If the game never instantiated it,
/// creating the clone would silently decouple the game's own per-frame colorFilter writes
/// (day/night grading) from what is rendered, freezing the grading at the values present
/// when the mod first touched the volume.
///
/// So the rule is: write into the profile only when the game already did the instancing.
/// Otherwise the volume is left completely alone (the other six parameters still apply).
/// </summary>
public static class VolumeProfilePolicy
{
    /// <summary>
    /// True only when the game already instantiated the volume's runtime profile, i.e.
    /// the clone is the profile the game itself is using.
    /// </summary>
    public static bool ShouldUseProfile(bool gameInstantiatedProfile) => gameInstantiatedProfile;
}

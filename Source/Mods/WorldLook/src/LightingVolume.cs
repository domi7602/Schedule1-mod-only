// ---------------------------------------------------------------------------
// Lighting/LightingVolume.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract checks and an independent
// reconstruction proof can still read that file's exact content.
//
//   Lighting/VolumeFlagPolicy.cs       -> section VolumeFlagPolicy.cs
//   Lighting/VolumeProfilePolicy.cs    -> section VolumeProfilePolicy.cs
//   Lighting/SnapshotStore.cs          -> section SnapshotStore.cs
// ---------------------------------------------------------------------------

namespace GoldenHour
{
    // ==== WorldLook section: VolumeFlagPolicy.cs ====

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
    // ==== end WorldLook section: VolumeFlagPolicy.cs ====
}

namespace GoldenHour
{
    // ==== WorldLook section: VolumeProfilePolicy.cs ====

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
    // ==== end WorldLook section: VolumeProfilePolicy.cs ====
}

namespace GoldenHour
{
    // ==== WorldLook section: SnapshotStore.cs ====
    using System.Collections.Generic;


    /// <summary>
    /// Reapply baseline store: records the game's bias-free value per parameter key. The
    /// activation records the first value; the reapply path replaces it via <see cref="Update"/>
    /// whenever the game's drift is adopted (BaselinePolicy), so the stored baseline is always
    /// the latest adopted game value — restore returns exactly that.
    /// </summary>
    public sealed class SnapshotStore
    {
        private readonly Dictionary<string, Argb> _values = new();

        public int Count => _values.Count;
        public IReadOnlyCollection<string> Keys => _values.Keys;

        public void RecordOnce(string key, Argb value)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (!_values.ContainsKey(key)) _values[key] = value;
        }

        public bool TryGet(string key, out Argb value)
        {
            if (key != null && _values.TryGetValue(key, out var stored))
            {
                value = stored;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// Replaces (or sets) a baseline value. Used by the reapply path when the game
        /// overwrote a biased value: the game's current value becomes the new baseline
        /// so the bias follows the day/night drift instead of freezing the first
        /// snapshot. Restore then returns the latest game value, not a stale one.
        /// </summary>
        public void Update(string key, Argb value)
        {
            if (string.IsNullOrEmpty(key)) return;
            _values[key] = value;
        }

        public void Clear() => _values.Clear();
    }
    // ==== end WorldLook section: SnapshotStore.cs ====
}

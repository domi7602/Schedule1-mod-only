// ---------------------------------------------------------------------------
// Lighting/LightingCadence.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract checks and an independent
// reconstruction proof can still read that file's exact content.
//
//   Lighting/ReapplyPolicy.cs          -> section ReapplyPolicy.cs
//   Lighting/TogglePolicy.cs           -> section TogglePolicy.cs
//   Lighting/GoldenHourTargets.cs      -> section GoldenHourTargets.cs
// ---------------------------------------------------------------------------

namespace GoldenHour
{
    // ==== WorldLook section: ReapplyPolicy.cs ====

    /// <summary>Bounded reapply scheduling: while active, applied values are refreshed at
    /// most once per interval (default minimum 0.5 s). This is the cost-saving fallback
    /// cadence — since 0.1.4 the default is per-frame apply (ApplyEveryLateUpdate), because
    /// the game's day/night grading rewrites these values continuously and a once-per-second
    /// reapply loses every frame race.</summary>
    public sealed class ReapplyPolicy
    {
        public const double MinimumIntervalSeconds = 0.5;

        private readonly double _interval;
        private double? _lastApplied;

        public ReapplyPolicy(double intervalSeconds)
        {
            _interval = double.IsFinite(intervalSeconds) && intervalSeconds >= MinimumIntervalSeconds
                ? intervalSeconds
                : MinimumIntervalSeconds;
        }

        public bool IsDue(double nowSeconds) =>
            _lastApplied is null || (double.IsFinite(nowSeconds) && nowSeconds - _lastApplied.Value >= _interval);

        public void MarkApplied(double nowSeconds)
        {
            if (double.IsFinite(nowSeconds)) _lastApplied = nowSeconds;
        }
    }
    // ==== end WorldLook section: ReapplyPolicy.cs ====
}

namespace GoldenHour
{
    // ==== WorldLook section: TogglePolicy.cs ====

    /// <summary>A/B toggle state machine: idle → active (snapshot needed once per
    /// activation) → idle. Snapshot state resets on every activation.</summary>
    public sealed class TogglePolicy
    {
        private bool _active;
        private bool _snapshotTaken;

        public bool IsActive => _active;
        public bool SnapshotNeeded => _active && !_snapshotTaken;

        /// <summary>Flip state. Returns the new IsActive value.</summary>
        public bool Toggle()
        {
            _active = !_active;
            _snapshotTaken = false;
            return _active;
        }

        public void MarkSnapshotTaken() => _snapshotTaken = true;
    }
    // ==== end WorldLook section: TogglePolicy.cs ====
}

namespace GoldenHour
{
    // ==== WorldLook section: GoldenHourTargets.cs ====

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
    // ==== end WorldLook section: GoldenHourTargets.cs ====
}

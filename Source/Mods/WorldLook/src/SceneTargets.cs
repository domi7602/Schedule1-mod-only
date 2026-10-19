using System;
using System.Collections.Generic;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using URP = UnityEngine.Rendering.Universal;

namespace GoldenHour;

/// <summary>
/// Runtime accessors for the seven golden-hour parameters. Every parameter has a
/// baseline in the snapshot store: the activation records what is readable, the first
/// apply of a key that only became readable later records its pristine current value
/// first, and the reapply path rebases onto the game's drifting values (BaselinePolicy +
/// SnapshotStore.Update). Every write is a multiplicative warm bias blended from the
/// current baseline. Missing targets are skipped and reported (deduplicated) —
/// fail-soft by design.
///
/// Asset-level vs scene-level targets: the skybox material and the volume profile are
/// assets that persist across scene loads, so a Main-scene exit restores them (into
/// the recorded material/profile references). Per-scene RenderSettings (ambient, fog,
/// sun) die with the scene and are intentionally NOT written during a scene exit —
/// restoring them into the destination scene would pollute menu/loading state.
///
/// Volume writes (0.1.1) go into the volume's <c>profile</c> runtime instance, never the
/// shared asset, and force the component/override flags first: a <c>colorFilter</c> write
/// while <c>active</c>/<c>overrideState</c> is off is a silent no-op, which is exactly the
/// "toggle does nothing" symptom. Both flags are restored on the way out. The profile
/// instance is only used when the game itself instantiated it (<see cref="VolumeProfilePolicy"/>),
/// because reading <c>volume.profile</c> would otherwise create a clone that takes over
/// rendering and decouples the game's own per-frame grading writes.
/// </summary>
public sealed class SceneTargets
{
    public const string KeySkySunShine = "sky.sunShine";
    public const string KeySunColor = "sun.color";
    public const string KeyAmbientSky = "ambient.sky";
    public const string KeyAmbientEquator = "ambient.equator";
    public const string KeyAmbientGround = "ambient.ground";
    public const string KeyFogColor = "fog.color";
    public const string KeyVolumeColorFilter = "volume.colorFilter";

    public static readonly string[] AllKeys =
    {
        KeySkySunShine, KeySunColor, KeyAmbientSky, KeyAmbientEquator,
        KeyAmbientGround, KeyFogColor, KeyVolumeColorFilter,
    };

    private URP.ColorAdjustments? _colorAdjustments;
    private Volume? _volume;
    private double _nextVolumeSearch;
    private bool _volumeFlagsChanged;
    private bool _originalVolumeActive;
    private bool _originalFilterOverride;
    private bool? _volumeInstantiated;
    private Material? _skyMaterial;
    private Light? _sunLight;
    private readonly HashSet<string> _warnedOnce = new();

    /// <summary>Records current values for every readable key. True if at least one key recorded.</summary>
    public bool Snapshot(SnapshotStore store, ModLogger log)
    {
        int recorded = 0;
        TrySnapshotSky(store, ref recorded, log);
        TrySnapshot(store, KeySunColor, ReadSunColor, ref recorded, log);
        TrySnapshot(store, KeyAmbientSky, () => ReadAmbient(RenderSettings.ambientSkyColor), ref recorded, log);
        TrySnapshot(store, KeyAmbientEquator, () => ReadAmbient(RenderSettings.ambientEquatorColor), ref recorded, log);
        TrySnapshot(store, KeyAmbientGround, () => ReadAmbient(RenderSettings.ambientGroundColor), ref recorded, log);
        TrySnapshot(store, KeyFogColor, () => ColorFrom(RenderSettings.fogColor), ref recorded, log);
        TrySnapshot(store, KeyVolumeColorFilter, ReadVolumeColorFilter, ref recorded, log);
        return recorded > 0;
    }

    /// <summary>Applies the warm bias to every recorded-or-readable key. Returns applied count.</summary>
    public int Apply(
        SnapshotStore store, double strength, bool diagnosticStrongTint, ModLogger log, double previousStrength = double.NaN)
    {
        int applied = 0;
        applied += Handle(store, KeySkySunShine, GoldenHourTargets.SkySunGlowTint, strength, previousStrength, ReadSkySunShine, WriteSkySunShine, log);
        applied += Handle(store, KeySunColor, GoldenHourTargets.SunLightTint, strength, previousStrength, ReadSunColor, WriteSunColor, log);

        if (RenderSettings.ambientMode == AmbientMode.Trilight)
        {
            applied += Handle(store, KeyAmbientSky, GoldenHourTargets.AmbientTint, strength, previousStrength,
                () => ReadAmbient(RenderSettings.ambientSkyColor), v => RenderSettings.ambientSkyColor = v, log);
            applied += Handle(store, KeyAmbientEquator, GoldenHourTargets.AmbientTint, strength, previousStrength,
                () => ReadAmbient(RenderSettings.ambientEquatorColor), v => RenderSettings.ambientEquatorColor = v, log);
            applied += Handle(store, KeyAmbientGround, GoldenHourTargets.AmbientTint, strength, previousStrength,
                () => ReadAmbient(RenderSettings.ambientGroundColor), v => RenderSettings.ambientGroundColor = v, log);
        }
        else
        {
            // Trilight ambient is not in use, so those three keys cannot show the bias.
            WarnOnce(log, $"ambientMode is {RenderSettings.ambientMode}, not Trilight — ambient bias skipped.");
        }

        if (RenderSettings.fog)
        {
            applied += Handle(store, KeyFogColor, GoldenHourTargets.FogTint, strength, previousStrength,
                () => ColorFrom(RenderSettings.fogColor), v => RenderSettings.fogColor = v, log);
        }
        else
        {
            WarnOnce(log, "fog is disabled in this scene — fog color bias skipped.");
        }

        applied += ApplyVolume(store, strength, previousStrength, diagnosticStrongTint, log);
        return applied;
    }

    /// <summary>
    /// Full restore: every recorded key is written back to its snapshotted original,
    /// including per-scene RenderSettings. Wired to toggle-off and to scene-exit/
    /// shutdown/unload while still inside Main. Returns restored count.
    /// </summary>
    public int Restore(SnapshotStore store, ModLogger log)
    {
        int restored = 0;
        RestoreSky(store, ref restored, log);
        RestoreSun(store, ref restored, log);
        RestoreOne(store, KeyAmbientSky, v => RenderSettings.ambientSkyColor = v, ref restored, log);
        RestoreOne(store, KeyAmbientEquator, v => RenderSettings.ambientEquatorColor = v, ref restored, log);
        RestoreOne(store, KeyAmbientGround, v => RenderSettings.ambientGroundColor = v, ref restored, log);
        RestoreOne(store, KeyFogColor, v => RenderSettings.fogColor = v, ref restored, log);
        RestoreVolume(store, ref restored, log);
        return restored;
    }

    /// <summary>
    /// Asset-level restore for a Main-scene exit: writes the recorded `_SunShine` back
    /// into the material instance that was written to (not whatever skybox is current)
    /// and restores the volume profile colorFilter — both are assets that would
    /// otherwise carry the bias across scene loads. Per-scene RenderSettings are
    /// deliberately skipped: they die with the Main scene.
    /// </summary>
    public int RestoreAssetLevel(SnapshotStore store, ModLogger log)
    {
        int restored = 0;
        RestoreSky(store, ref restored, log);
        RestoreVolume(store, ref restored, log);
        return restored;
    }

    /// <summary>
    /// One-shot diagnostic dump for the in-game verify: whether URP post-processing is
    /// even on for the main camera, which volume layer it uses, whether the game
    /// instantiated its own volume profile, and what state the volume/sky/render settings
    /// are actually in. Diagnostics only — it never applies the bias. It does read state,
    /// so it may resolve the volume cache (which can restore previously forced flags).
    /// </summary>
    public void LogDiagnostics(ModLogger log)
    {
        try
        {
            var cam = Camera.main;
            if (cam != null)
            {
                var data = GameObjectResolver.FindComponentDeep<URP.UniversalAdditionalCameraData>(cam.gameObject, "", null);
                if (data != null)
                {
                    log.Info($"DIAG camera '{cam.name}': renderPostProcessing={data.renderPostProcessing}, " +
                             $"volumeLayerMask={data.volumeLayerMask.value}, antialiasing={data.antialiasing}");
                }
                else
                {
                    log.Info($"DIAG camera '{cam.name}': no UniversalAdditionalCameraData component.");
                }
            }
            else
            {
                log.Info("DIAG camera: Camera.main is null.");
            }

            var adjustments = ResolveColorAdjustments(log);
            log.Info($"DIAG volume: HasInstantiatedProfile={(_volumeInstantiated?.ToString() ?? "unknown")}");
            if (adjustments != null)
            {
                log.Info($"DIAG volume: ColorAdjustments active={adjustments.active}, " +
                         $"colorFilter.overrideState={adjustments.colorFilter.overrideState}, " +
                         $"colorFilter={Fmt(adjustments.colorFilter.value)}");
            }

            var sky = RenderSettings.skybox;
            if (sky == null)
            {
                log.Info("DIAG sky: none.");
            }
            else
            {
                bool has = sky.HasProperty("_SunShine");
                log.Info($"DIAG sky '{sky.name}': HasProperty(_SunShine)={has}, " +
                         $"_SunShine={(has ? Fmt(sky.GetColor("_SunShine")) : "n/a")}");
            }

            var sun = RenderSettings.sun;
            string sunDesc = "none";
            if (sun != null && NetworkGuard.IsAlive(sun)) sunDesc = $"'{sun.name}' {Fmt(sun.color)}";
            log.Info($"DIAG render: ambientMode={RenderSettings.ambientMode}, fog={RenderSettings.fog}, sun={sunDesc}");
        }
        catch (Exception ex)
        {
            WarnOnce(log, "diagnostics failed", ex);
        }
    }

    public void InvalidateRuntimeCache()
    {
        _colorAdjustments = null;
        _volume = null;
        _nextVolumeSearch = 0d;
        _volumeFlagsChanged = false;
        _volumeInstantiated = null;
        _skyMaterial = null;
        _sunLight = null;
        _warnedOnce.Clear();
    }

    // --- individual accessors ---------------------------------------------------------

    private Argb? ReadSkySunShine()
    {
        var mat = RenderSettings.skybox;
        if (!NetworkGuard.IsAlive(mat) || !mat.HasProperty("_SunShine")) return null;
        return ColorFrom(mat.GetColor("_SunShine"));
    }

    private void WriteSkySunShine(Color value)
    {
        var mat = RenderSettings.skybox;
        if (!NetworkGuard.IsAlive(mat) || !mat.HasProperty("_SunShine")) return;
        _skyMaterial ??= mat; // remember where we wrote — this is the restore target
        mat.SetColor("_SunShine", value);
    }

    private Argb? ReadSunColor()
    {
        var sun = ResolveSunLight();
        return sun == null ? null : ColorFrom(sun.color);
    }

    private void WriteSunColor(Color value)
    {
        var sun = ResolveSunLight();
        if (sun != null) sun.color = value;
    }

    private Light? ResolveSunLight()
    {
        var sun = RenderSettings.sun;
        if (sun == null || !NetworkGuard.IsAlive(sun)) return null;
        _sunLight ??= sun; // restore target (first written light wins)
        return sun;
    }

    private static Argb? ReadAmbient(Color color) => ColorFrom(color);

    private Argb? ReadVolumeColorFilter()
    {
        var adjustments = ResolveColorAdjustments(null);
        return adjustments == null ? null : ColorFrom(adjustments.colorFilter.value);
    }

    /// <summary>
    /// Volume apply: force the component/override flags when the tint could not render
    /// otherwise, then blend the colorFilter through the normal Handle path.
    /// </summary>
    private int ApplyVolume(
        SnapshotStore store, double strength, double previousStrength, bool diagnosticStrongTint, ModLogger log)
    {
        try
        {
            var adjustments = ResolveColorAdjustments(log);
            if (adjustments == null) return 0;

            if (VolumeFlagPolicy.NeedsEnable(strength, adjustments.active, adjustments.colorFilter.overrideState))
            {
                if (!_volumeFlagsChanged)
                {
                    _originalVolumeActive = adjustments.active;
                    _originalFilterOverride = adjustments.colorFilter.overrideState;
                }
                adjustments.active = true;
                adjustments.colorFilter.overrideState = true;
                _volumeFlagsChanged = true;
            }

            var tint = diagnosticStrongTint ? GoldenHourTargets.DiagnosticVolumeTint : GoldenHourTargets.VolumeTint;
            return Handle(store, KeyVolumeColorFilter, tint, strength, previousStrength,
                () => ColorFrom(adjustments.colorFilter.value),
                v => adjustments.colorFilter.value = v, log);
        }
        catch (Exception ex)
        {
            WarnOnce(log, "volume apply failed", ex);
            return 0;
        }
    }

    private URP.ColorAdjustments? ResolveColorAdjustments(ModLogger? log)
    {
        if (IsUsableVolume()) return _colorAdjustments;

        if (_colorAdjustments != null || _volume != null)
        {
            // The volume we were using is gone or replaced: put its flags back before
            // walking away, then search again.
            RestoreVolumeFlags(log);
            _colorAdjustments = null;
            _volume = null;
        }

        double now = Time.unscaledTime;
        if (now < _nextVolumeSearch) return null; // throttle the scene walk to once per 2 s
        _nextVolumeSearch = now + 2d;

        bool sawUninstantiated = false;
        bool sawVolume = false;
        try
        {
            for (int s = 0; s < SceneManager.sceneCount; s++)
            {
                var scene = SceneManager.GetSceneAt(s);
                if (!scene.isLoaded) continue;
                var roots = scene.GetRootGameObjects();
                if (roots == null) continue;
                for (int r = 0; r < roots.Length; r++)
                {
                    var root = roots[r];
                    if (root == null || !NetworkGuard.IsAlive(root)) continue;
                    // Live-verified 2026-10-16: the volume lives at
                    // @Managers/@EnvironmentFX/GlobalVolume. "EnvironmentFX" is NOT a scene
                    // root, so filtering the roots on that token alone never matched and the
                    // volume key silently stayed unapplied. Accept either token — the walk
                    // stays cheap because only these roots are searched deep.
                    if (root.name.IndexOf("Managers", StringComparison.OrdinalIgnoreCase) < 0 &&
                        root.name.IndexOf("Environment", StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    var volumeGo = GameObjectResolver.FindChildDeep(root, "GlobalVolume", exactMatch: false);
                    if (volumeGo == null) continue;
                    // From here on a GlobalVolume object WAS found, so the final warning must
                    // not claim otherwise (review N-1) — the failure is further down.
                    sawVolume = true;
                    var volume = GameObjectResolver.FindComponentDeep<Volume>(volumeGo, "", null);
                    if (volume == null) continue;

                    // Reading volume.profile INSTANTIATES it (m_InternalProfile) and the
                    // VolumeManager then blends from that clone instead of the shared
                    // asset. That is only safe when the game instantiated it itself — then
                    // the clone is what the game writes into. If it did not, creating the
                    // clone would decouple the game's own per-frame colorFilter writes
                    // (day/night grading) from rendering. Leave the volume alone then.
                    _volumeInstantiated = volume.HasInstantiatedProfile();
                    if (!VolumeProfilePolicy.ShouldUseProfile(_volumeInstantiated.Value))
                    {
                        // A volume without a game-instantiated profile must not end the
                        // search — a later GlobalVolume may have one (review N-a).
                        sawUninstantiated = true;
                        continue;
                    }

                    // The game's own runtime profile instance — writing here never touches
                    // the shared profile asset that ships with the scene.
                    var profile = volume.profile;
                    if (profile == null) continue;
                    var components = profile.components;
                    if (components == null) continue;
                    for (int i = 0; i < components.Count; i++)
                    {
                        var candidate = components[i];
                        var adjustments = candidate?.TryCast<URP.ColorAdjustments>();
                        if (adjustments != null)
                        {
                            _volume = volume;
                            _colorAdjustments = adjustments;
                            return _colorAdjustments;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _colorAdjustments = null;
            _volume = null;
            WarnOnce(log, $"volume lookup failed: {ex.Message}");
            return null;
        }

        if (sawUninstantiated)
        {
            WarnOnce(log, "GlobalVolume uses its shared profile asset (no runtime instance) — " +
                          "volume colorFilter bias skipped so the game's own grading keeps driving it.");
            return null;
        }

        if (sawVolume)
        {
            // A GlobalVolume exists but gave us nothing writable (no Volume component, no
            // profile, or no ColorAdjustments in it). Say that — do not report it as missing.
            WarnOnce(log, "GlobalVolume found but no usable ColorAdjustments on its profile — " +
                          "volume grading skipped.");
            return null;
        }

        WarnOnce(log, "no GlobalVolume found under @Managers/@EnvironmentFX — volume grading skipped.");
        return null;
    }

    /// <summary>
    /// A volume stays usable as long as the object lives and is enabled/global — notably
    /// NOT gated on <c>weight</c>. Gating on weight would drop the cache (and with it the
    /// restore target) while the profile instance still carries our tint.
    /// </summary>
    private bool IsUsableVolume() =>
        _colorAdjustments != null && _volume != null &&
        NetworkGuard.IsAlive(_colorAdjustments) && NetworkGuard.IsAlive(_volume) &&
        _volume.enabled && _volume.isGlobal && _volume.gameObject.activeInHierarchy;

    // --- shared helpers ---------------------------------------------------------------

    private void TrySnapshotSky(SnapshotStore store, ref int recorded, ModLogger log)
    {
        try
        {
            var value = ReadSkySunShine();
            if (value is null || !value.Value.IsFinite) return;
            store.RecordOnce(KeySkySunShine, value.Value);
            recorded++;
        }
        catch (Exception ex)
        {
            WarnOnce(log, $"snapshot {KeySkySunShine} failed", ex);
        }
    }

    private void TrySnapshot(SnapshotStore store, string key, Func<Argb?> read, ref int recorded, ModLogger log)
    {
        try
        {
            var value = read();
            if (value is null || !value.Value.IsFinite) return;
            store.RecordOnce(key, value.Value);
            recorded++;
        }
        catch (Exception ex)
        {
            WarnOnce(log, $"snapshot {key} failed", ex);
        }
    }

    private int Handle(
        SnapshotStore store, string key, Argb tint, double strength, double previousStrength,
        Func<Argb?> read, Action<Color> write, ModLogger log)
    {
        try
        {
            var current = read();
            if (current is null || !current.Value.IsFinite) return 0;
            if (!tint.IsFinite) return 0;
            if (!store.TryGet(key, out var original))
            {
                // First write of a key that was unreadable at activation: record the
                // pristine value so restore can undo every written key and reapplies
                // never blend from an unrecorded value (no compounding).
                store.RecordOnce(key, current.Value);
                original = current.Value;
            }

            // Reapply decision: the day/night cycle rewrites these values, so the
            // bias rebases onto the game's current value (BaselinePolicy adopts the
            // drift) instead of re-blending the stale activation snapshot.
            var decision = BaselinePolicy.Evaluate(original, current.Value, tint, strength, previousStrength);
            if (decision.Write)
            {
                store.Update(key, decision.Baseline);
                write(ColorFrom(decision.Blended));
            }
            return 1;
        }
        catch (Exception ex)
        {
            WarnOnce(log, $"apply {key} failed", ex);
            return 0;
        }
    }

    private int RestoreSky(SnapshotStore store, ref int restored, ModLogger log)
    {
        if (!store.TryGet(KeySkySunShine, out var original) || !original.IsFinite) return restored;
        try
        {
            var value = ColorFrom(original);
            int wrote = 0;

            var cached = _skyMaterial;
            if (cached != null && NetworkGuard.IsAlive(cached) && cached.HasProperty("_SunShine"))
            {
                cached.SetColor("_SunShine", value);
                wrote++;
            }

            var current = RenderSettings.skybox;
            if (current != null && NetworkGuard.IsAlive(current) && current.HasProperty("_SunShine"))
            {
                current.SetColor("_SunShine", value);
                wrote++;
            }

            if (wrote == 0)
            {
                WarnOnce(log, "restore sky.sunShine skipped — no live sky material.");
                return restored;
            }
            restored++;
        }
        catch (Exception ex)
        {
            WarnOnce(log, $"restore {KeySkySunShine} failed", ex);
        }
        return restored;
    }

    private int RestoreSun(SnapshotStore store, ref int restored, ModLogger log)
    {
        if (!store.TryGet(KeySunColor, out var original) || !original.IsFinite) return restored;
        try
        {
            var value = ColorFrom(original);
            int wrote = 0;
            var cached = _sunLight;
            if (cached != null && NetworkGuard.IsAlive(cached))
            {
                cached.color = value;
                wrote++;
            }
            var current = RenderSettings.sun;
            if (current != null && NetworkGuard.IsAlive(current))
            {
                current.color = value;
                wrote++;
            }
            if (wrote == 0)
            {
                WarnOnce(log, "restore sun.color skipped — no live sun light.");
                return restored;
            }
            restored++;
        }
        catch (Exception ex)
        {
            WarnOnce(log, $"restore {KeySunColor} failed", ex);
        }
        return restored;
    }

    private int RestoreVolume(SnapshotStore store, ref int restored, ModLogger log)
    {
        var adjustments = _colorAdjustments;
        if (adjustments != null && NetworkGuard.IsAlive(adjustments) &&
            store.TryGet(KeyVolumeColorFilter, out var original) && original.IsFinite)
        {
            try
            {
                adjustments.colorFilter.value = ColorFrom(original);
                restored++;
            }
            catch (Exception ex)
            {
                WarnOnce(log, $"restore {KeyVolumeColorFilter} failed", ex);
            }
        }
        RestoreVolumeFlags(log);
        return restored;
    }

    /// <summary>Puts the volume's own active/override flags back the way we found them.</summary>
    private void RestoreVolumeFlags(ModLogger? log)
    {
        if (!_volumeFlagsChanged) return;
        try
        {
            var adjustments = _colorAdjustments;
            if (adjustments != null && NetworkGuard.IsAlive(adjustments))
            {
                adjustments.active = _originalVolumeActive;
                adjustments.colorFilter.overrideState = _originalFilterOverride;
            }
        }
        catch (Exception ex)
        {
            WarnOnce(log, "restore volume flags failed", ex);
        }
        _volumeFlagsChanged = false;
    }

    private int RestoreOne(SnapshotStore store, string key, Action<Color> write, ref int restored, ModLogger log)
    {
        try
        {
            if (!store.TryGet(key, out var original) || !original.IsFinite) return restored;
            write(ColorFrom(original));
            restored++;
        }
        catch (Exception ex)
        {
            WarnOnce(log, $"restore {key} failed", ex);
        }
        return restored;
    }

    private void WarnOnce(ModLogger? log, string message, Exception? ex = null)
    {
        if (!_warnedOnce.Add(message)) return;
        if (log == null)
        {
            // No logger available (lookup happens before activation): still make the
            // miss visible in the MelonLoader log.
            MelonLogger.Warning($"[GoldenHour] {message}");
            return;
        }
        if (ex != null) log.Warn(message, ex);
        else log.Warn(message);
    }

    private static string Fmt(Color c) => $"({c.r:0.###}, {c.g:0.###}, {c.b:0.###}, {c.a:0.###})";

    private static Argb ColorFrom(Color c) => new(c.r, c.g, c.b, c.a);
    private static Color ColorFrom(Argb a) => new(a.R, a.G, a.B, a.A);
}

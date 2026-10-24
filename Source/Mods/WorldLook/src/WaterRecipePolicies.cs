// ---------------------------------------------------------------------------
// Water/WaterRecipePolicies.cs
//
// Unity-free water recipes, native-input validation and material ownership.
// The reference path uses environment colors, not a reflection camera. The former
// camera/matrix policies and the standalone sky-only material leases are retired.
//
//   Water/WaterLookRecipe.cs           -> section WaterLookRecipe.cs
//   Water/ReferenceWaterLook.cs        -> section ReferenceWaterLook.cs
//   Water/ReferenceWaterPolicy.cs      -> section ReferenceWaterPolicy.cs
//   Water/WaterSession.cs              -> section WaterSession.cs
// ---------------------------------------------------------------------------

namespace WaterShader
{
    // ==== WorldLook section: WaterLookRecipe.cs ====
    using System.Globalization;
    using System.Text;


    internal interface IWaterLookMaterial
    {
        bool HasProperty(string name);
        void SetFloat(string name, float value);
        void SetColor(string name, float r, float g, float b, float alpha);
    }

    /// <summary>Unity-free material tuning, called only when an owned clone is configured.</summary>
    internal static class WaterLookRecipe
    {
        private static readonly (string name, float value)[] Floats =
        {
            ("_WaveHeight", 0.35f),
            ("_WaveSpeed", 1.1f),
            ("_WaveCount", 4f),
            ("_NormalStrength", 0.18f),
            ("_FoamBaseAmount", 0.25f),
            // Defaults declared by the matching native shader; not physical depth units.
            ("_DepthVertical", 4f),
            ("_DepthHorizontal", 1f),
            ("_RefractionStrength", 0.1f),
            // Approved chromatic suppression candidate, not a claim about the native default.
            ("_RefractionChromaticAberration", 0f),
        };

        internal static string Apply(IWaterLookMaterial material)
        {
            var applied = new StringBuilder("[water-shader] clear-shallows applied:");
            int prefixLength = applied.Length;
            foreach (var (name, value) in Floats)
            {
                if (!material.HasProperty(name)) continue;
                material.SetFloat(name, value);
                applied.Append(' ').Append(name).Append('=').Append(value.ToString(CultureInfo.InvariantCulture));
            }
            if (material.HasProperty("_ShallowColor"))
            {
                // Artistic shallow-opacity candidate; native deep color is intentionally untouched.
                const float alpha = 0.15f;
                material.SetColor("_ShallowColor", 0.025f, 0.055f, 0.060f, alpha);
                applied.Append(" _ShallowColor=(0.025,0.055,0.06,").Append(alpha.ToString(CultureInfo.InvariantCulture)).Append(')');
            }
            if (applied.Length == prefixLength) applied.Append(" none");
            return applied.ToString();
        }
    }
    // ==== end WorldLook section: WaterLookRecipe.cs ====
}

namespace WaterShader
{
    // ==== WorldLook section: ReferenceWaterLook.cs ====
    using System.Globalization;
    using System.Text;


    /// <summary>
    /// Look step for the reference water shader, applied to the owned clone right after the
    /// reference shader is bound. The shader's own declared defaults are the baseline; this
    /// recipe preserves the installed standalone's clear-shallows tuning. Scene-depth
    /// difference is an eye-space thickness approximation. Body opacity and the weak
    /// environment reflection are composed separately; this does not force opaque water.
    /// Sky/horizon colors and wind activity are live native inputs. Deep tint, sun
    /// highlight and shore-foam values use the shader/material settings.
    /// Every write is HasProperty-guarded and Unity-free, so a shader revision that drops a
    /// name degrades to "that value is not applied" instead of faulting the render path.
    /// </summary>
    internal static class ReferenceWaterLook
    {
        private static readonly (string name, float value)[] Floats =
        {
            // Preserve the approved late-dye tuning and environment-only composition.
            ("_Absorption", 0.10f),
            ("_RippleStrength", 0.28f),
            ("_SurfaceOpacity", 0.22f),
            ("_ReflectionStrength", 0.12f),
        };

        private static readonly (string name, float r, float g, float b)[] Colors =
        {
            // Near 0.5, the shader's *2 shallow-transmission gain stays near unity.
            // Its compositor also retains a separate untinted destination contribution.
            ("_ShallowTint", 0.46f, 0.52f, 0.55f),
        };

        internal static string Apply(IWaterLookMaterial material)
        {
            var applied = new StringBuilder("[water-shader] reference clarity applied:");
            int prefixLength = applied.Length;
            foreach (var (name, value) in Floats)
            {
                if (!material.HasProperty(name)) continue;
                material.SetFloat(name, value);
                applied.Append(' ').Append(name).Append('=').Append(value.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var (name, r, g, b) in Colors)
            {
                if (!material.HasProperty(name)) continue;
                material.SetColor(name, r, g, b, 1f);
                applied.Append(' ').Append(name).Append("=(")
                    .Append(r.ToString(CultureInfo.InvariantCulture)).Append(',')
                    .Append(g.ToString(CultureInfo.InvariantCulture)).Append(',')
                    .Append(b.ToString(CultureInfo.InvariantCulture)).Append(')');
            }
            if (applied.Length == prefixLength) applied.Append(" none");
            return applied.ToString();
        }
    }
    // ==== end WorldLook section: ReferenceWaterLook.cs ====
}

namespace WaterShader
{
    // ==== WorldLook section: ReferenceWaterPolicy.cs ====
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Shader contract for the environment-only reference water path.
    /// </summary>
    internal static class ReferenceWaterPolicy
    {
        /// <summary>Shader properties the reference shader contract requires.</summary>
        internal static readonly string[] RequiredShaderProperties =
        {
            "_ShallowTint", "_DeepTint", "_Absorption", "_SurfaceOpacity",
            "_ReflectionStrength",
            "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength", "_WindActivity",
            "_SunStrength", "_SunSharpness",
            "_FoamColor", "_FoamDepth", "_FoamStrength",
            "_SkyColor", "_AmbientColor",
        };

        /// <summary>Decide whether the reference shader may replace the clone's shader.</summary>
        internal static bool CanApplyReference(bool bundleLoaded, bool shaderSupported, bool hasRequiredProperties)
        {
            return bundleLoaded && shaderSupported && hasRequiredProperties;
        }

        /// <summary>List contract property names the supplied probe reports as absent.</summary>
        internal static string[] MissingRequiredProperties(Func<string, bool> hasProperty)
        {
            if (hasProperty == null) throw new ArgumentNullException(nameof(hasProperty));

            var missing = new List<string>();
            foreach (string name in RequiredShaderProperties)
            {
                if (!hasProperty(name)) missing.Add(name);
            }

            return missing.ToArray();
        }

    }
    // ==== end WorldLook section: ReferenceWaterPolicy.cs ====

    internal static class WaterEnvironmentPolicy
    {
        internal static bool IsFinite((float r, float g, float b, float a) color)
            => float.IsFinite(color.r) && float.IsFinite(color.g)
                && float.IsFinite(color.b) && float.IsFinite(color.a);

        // Preserve native HDR/night colors exactly; never normalize or brighten them.
        internal static (float r, float g, float b, float a)? SelectColor(
            (float r, float g, float b, float a)? native,
            (float r, float g, float b, float a)? ambient)
            => native.HasValue && IsFinite(native.Value) ? native
                : ambient.HasValue && IsFinite(ambient.Value) ? ambient : null;

        internal static float WindActivity(float value)
            => float.IsFinite(value) ? Math.Clamp(value, 0f, 1f) : 0f;
    }

    internal static class WaterShaderAssignment
    {
        // Used for both initial binding and rollback. A setter may silently fail or
        // mutate and then throw: neither case proves a successful shader transition.
        internal static void AssignConfirmed<T>(T shader, Func<T, bool> alive,
            Action<T> write, Func<T?> read, Func<T?, T, bool> same) where T : class
        {
            if (shader == null || !alive(shader))
                throw new InvalidOperationException("Water shader candidate is unavailable; refusing an empty shader assignment.");
            write(shader);
            if (!same(read(), shader))
                throw new InvalidOperationException("Water shader assignment was not confirmed.");
        }
    }
}

namespace WaterShader
{
    // ==== WorldLook section: WaterSession.cs ====
    using System;


    internal interface IWaterRuntime<TTarget, TMaterial> where TTarget : class where TMaterial : class
    {
        TTarget? Discover();
        bool TargetAlive(TTarget value);
        bool MaterialAlive(TMaterial value);
        TMaterial? ReadMaterial(TTarget target);
        TMaterial CreateMaterial(TMaterial template);
        void ConfigureMaterial(TMaterial material);
        void WriteMaterial(TTarget target, TMaterial? material);
        bool SameMaterial(TMaterial? left, TMaterial right);
        void DestroyMaterial(TMaterial material);
    }

    internal enum WaterState { Stopped, Waiting, Applied, Yielded, Faulted }

    /// <summary>Unity-free owner and scheduler. Native work stays behind the deadline gate.</summary>
    internal sealed class WaterSession<TTarget, TMaterial> where TTarget : class where TMaterial : class
    {
        internal const double RetrySeconds = 5;
        internal const double HealthSeconds = 1;
        internal const int InactiveCleanupRetryLimit = 3;
        private readonly IWaterRuntime<TTarget, TMaterial> _runtime;
        private TTarget? _target;
        private TMaterial? _original;
        private TMaterial? _owned;
        private bool _active;
        private bool _pendingRelease;
        private double _nextCheck;
        private double _lastNow;
        private int _inactiveCleanupRetries;

        public WaterState State { get; private set; }
        public Exception? LastError { get; private set; }
        public WaterSession(IWaterRuntime<TTarget, TMaterial> runtime) { _runtime = runtime; }

        public void Start(double now)
        {
            if (_active) return;
            _active = true;
            _lastNow = now;
            _inactiveCleanupRetries = 0;
            _nextCheck = now;
            if (!_pendingRelease) State = WaterState.Waiting;
        }

        // Compatibility for explicit callers without a clock; runtime lifecycle
        // calls supply the actual event time rather than the last update time.
        public void Stop() => Stop(_lastNow);

        public void Stop(double now)
        {
            _active = false;
            _lastNow = now;
            _nextCheck = now + RetrySeconds;
            _inactiveCleanupRetries = InactiveCleanupRetryLimit;
            try { Release(); State = WaterState.Stopped; }
            catch (Exception ex) { Fault(ex); }
        }

        // An input/shader update can fail after publication. Keep ownership and let
        // the same bounded cleanup path reconcile it before another allocation.
        public void ReportFailure(Exception error, double now)
        {
            _lastNow = now;
            _nextCheck = now + RetrySeconds;
            Fault(error);
        }

        private void Fault(Exception ex)
        {
            LastError = ex;
            State = WaterState.Faulted;
            _pendingRelease = _target != null || _owned != null;
        }

        private void Release()
        {
            if (_owned != null)
            {
                if (_target != null && _runtime.TargetAlive(_target)
                    && _runtime.SameMaterial(_runtime.ReadMaterial(_target), _owned))
                {
                    if (_original == null || !_runtime.MaterialAlive(_original))
                        throw new InvalidOperationException("Native water material unavailable; retaining the visible owned material instead of writing null.");
                    _runtime.WriteMaterial(_target, _original);
                    if (!_runtime.SameMaterial(_runtime.ReadMaterial(_target), _original))
                        throw new InvalidOperationException("Native water material restoration was not confirmed.");
                }
                // A failed read/restore retains ownership: never destroy a possibly attached clone.
                if (_runtime.MaterialAlive(_owned)) _runtime.DestroyMaterial(_owned);
            }
            _target = null;
            _owned = null;
            _original = null;
            _pendingRelease = false;
            _inactiveCleanupRetries = 0;
            LastError = null;
        }

        public void Tick(double now)
        {
            _lastNow = now;
            if (now < _nextCheck) return;
            // OFF does not discard uncertain ownership. Only retained cleanup can
            // run while inactive, with a finite budget that ticks never replenish.
            if (!_active && (!_pendingRelease || _inactiveCleanupRetries <= 0)) return;
            _nextCheck = now + RetrySeconds; // No tight retries or catch-up after a stalled frame.
            try
            {
                if (_pendingRelease)
                {
                    if (!_active) _inactiveCleanupRetries--;
                    Release();
                    State = _active ? WaterState.Waiting : WaterState.Stopped;
                    return; // Cleanup and new allocation never share a retry slice.
                }
                if (_target != null)
                {
                    if (!_runtime.TargetAlive(_target)
                        || (_owned != null && !_runtime.MaterialAlive(_owned)))
                    {
                        Release();
                        State = WaterState.Waiting;
                        return;
                    }
                    if (_owned != null && !_runtime.SameMaterial(_runtime.ReadMaterial(_target), _owned))
                    {
                        var retained = _target;
                        Release();
                        _target = retained; // Yield until the renderer dies or the scene resets.
                        State = WaterState.Yielded;
                    }
                    _nextCheck = now + HealthSeconds;
                    return;
                }
                var target = _runtime.Discover();
                State = WaterState.Waiting;
                if (target == null || !_runtime.TargetAlive(target)) return;
                var original = _runtime.ReadMaterial(target);
                if (original == null || !_runtime.MaterialAlive(original)) return;
                _target = target;
                _original = original;
                _owned = _runtime.CreateMaterial(original);
                _runtime.ConfigureMaterial(_owned); // Retain even if tuning fails partway through.
                // Capture ownership before the setter: it may mutate and then throw.
                _runtime.WriteMaterial(target, _owned);
                if (!_runtime.SameMaterial(_runtime.ReadMaterial(target), _owned))
                    throw new InvalidOperationException("Water material assignment did not persist.");
                State = WaterState.Applied;
                LastError = null;
                _nextCheck = now + HealthSeconds;
            }
            catch (Exception ex) { Fault(ex); }
        }
    }
    // ==== end WorldLook section: WaterSession.cs ====
}

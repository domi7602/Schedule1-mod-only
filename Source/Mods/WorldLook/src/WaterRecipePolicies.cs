// ---------------------------------------------------------------------------
// Water/WaterRecipePolicies.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract checks and an independent
// reconstruction proof can still read that file's exact content.
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
    /// recipe moves only what makes the water read clearer:
    ///  - lower absorption, so the refracted bottom survives over more real water thickness;
    ///  - a calmer surface, so that bottom is smeared less;
    ///  - a shallow tint whose channels stay near 0.5, so the shader's *2 transmission gain
    ///    does not lift the bottom into a pale cream wash.
    /// The deep tint, the rest of the palette, reflection, sun and the shore foam are
    /// deliberately NOT written. Foam was narrowed in 0.1.1 as a side effect and reverted
    /// in 0.1.2 — the shader's own smoothstep gate, not our value, is what sizes it.
    /// Every write is HasProperty-guarded and Unity-free, so a shader revision that drops a
    /// name degrades to "that value is not applied" instead of faulting the render path.
    /// </summary>
    internal static class ReferenceWaterLook
    {
        private static readonly (string name, float value)[] Floats =
        {
            // Absorption is the dominant depth lever: the shader attenuates the refracted
            // bottom by 1 - exp(-thickness * _Absorption) and raises alpha with the same
            // term. 0.55 -> 0.38 still looked veiled within a couple of metres; 0.22 keeps
            // the bottom readable further out without dropping to the shader minimum.
            ("_Absorption", 0.22f),
            // A calmer surface smears the refracted bottom less.
            ("_RippleStrength", 0.45f),
        };

        private static readonly (string name, float r, float g, float b)[] Colors =
        {
            // Everything seen THROUGH the surface is sceneColor * lerp(shallow, deep, absorb) * 2.0.
            // The 0.1.2 tint (0.50, 0.75, 0.75) therefore gained (1.0, 1.5, 1.5): green and blue
            // lifted sand toward pale cream. Channels near 0.5 keep that gain near the bottom's
            // own colour, with only a small cool bias so it still reads as water. Red stays above
            // the shader default 0.35 so the old teal crush does not come back. Deep tint is not
            // written. The shader still composites a 0.35 alpha floor; this value cannot remove it.
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
    /// Unity-free decisions and math for the reference water reflection path.
    /// Row-major 4x4 arrays keep the reflection math testable without UnityEngine.
    /// The runtime consumes only MirrorPoint/MirrorDirection; ReflectionMatrix is
    /// the matrix those two are derived from and exists to pin that relationship.
    /// </summary>
    internal static class ReferenceWaterPolicy
    {
        /// <summary>Shader properties the reference shader contract requires.</summary>
        internal static readonly string[] RequiredShaderProperties =
        {
            "_ShallowTint", "_DeepTint", "_Absorption",
            "_ReflectionTex", "_ReflectionStrength", "_ReflectionSharpness",
            "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength",
            "_SunStrength", "_SunSharpness",
            "_FoamColor", "_FoamDepth", "_FoamStrength",
            "_SkyColor", "_AmbientColor",
        };

        /// <summary>
        /// Reflection across the horizontal plane y = waterY maps (x, y, z) to
        /// (x, 2*waterY - y, z). Returned as a row-major 4x4.
        /// </summary>
        internal static float[] ReflectionMatrix(float waterY)
        {
            return new[]
            {
                1f, 0f, 0f, 0f,
                0f, -1f, 0f, 2f * waterY,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f,
            };
        }

        /// <summary>Mirror a world point across y = waterY.</summary>
        internal static (float x, float y, float z) MirrorPoint(float waterY, float x, float y, float z)
        {
            return (x, 2f * waterY - y, z);
        }

        /// <summary>Mirror a direction across y = waterY (only the y component flips).</summary>
        internal static (float x, float y, float z) MirrorDirection(float x, float y, float z)
        {
            return (x, -y, z);
        }

        /// <summary>Clamp the reflection render-target size to the supported budget.</summary>
        internal static (int width, int height) ReflectionSize(int requestedWidth, int requestedHeight, int maxDimension)
        {
            int max = Math.Clamp(maxDimension, 128, 1024);
            int width = Math.Clamp(requestedWidth, 16, max);
            int height = Math.Clamp(requestedHeight, 16, max);
            return (width, height);
        }

        /// <summary>Decide whether a camera should trigger the owned reflection render.</summary>
        internal static bool ShouldRenderReflection(
            bool reentrancyGateOpen,
            bool cameraIsOwned,
            bool cameraIsReflection,
            bool cameraIsOverlay,
            bool cameraIsPreview,
            bool cameraIsGame,
            bool materialBound)
        {
            return reentrancyGateOpen
                   && !cameraIsOwned
                   && !cameraIsReflection
                   && !cameraIsOverlay
                   && !cameraIsPreview
                   && cameraIsGame
                   && materialBound;
        }

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

        /// <summary>Decide whether reflection inputs may be bound this frame.</summary>
        internal static bool ShouldBindReflection(bool materialBound, bool reflectionTextureReady)
        {
            return materialBound && reflectionTextureReady;
        }

        /// <summary>
        /// Scale a source resolution down to a bounded budget while preserving its
        /// aspect ratio. Unlike <see cref="ReflectionSize"/> this keeps the shape of
        /// the frame, which a per-axis clamp would distort at high resolutions.
        /// </summary>
        internal static (int width, int height) ReflectionSizeForAspect(int sourceWidth, int sourceHeight, int maxDimension)
        {
            int max = Math.Clamp(maxDimension, 128, 1024);
            if (sourceWidth <= 0 || sourceHeight <= 0) return (max, max);

            int longSide = Math.Max(sourceWidth, sourceHeight);
            float scale = Math.Min(1f, max / (float)longSide);
            int width = Math.Max(16, (int)Math.Round(sourceWidth * scale));
            int height = Math.Max(16, (int)Math.Round(sourceHeight * scale));
            return (width, height);
        }
    }
    // ==== end WorldLook section: ReferenceWaterPolicy.cs ====
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
        private readonly IWaterRuntime<TTarget, TMaterial> _runtime;
        private TTarget? _target;
        private TMaterial? _original;
        private TMaterial? _owned;
        private bool _active;
        private bool _pendingRelease;
        private double _nextCheck;

        public WaterState State { get; private set; }
        public Exception? LastError { get; private set; }
        public WaterSession(IWaterRuntime<TTarget, TMaterial> runtime) { _runtime = runtime; }

        public void Start(double now)
        {
            if (_active) return;
            _active = true;
            _nextCheck = now;
            if (!_pendingRelease) State = WaterState.Waiting;
        }

        public void Stop()
        {
            _active = false;
            try { Release(); State = WaterState.Stopped; }
            catch (Exception ex) { Fault(ex); }
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
                    _runtime.WriteMaterial(_target, _original != null && _runtime.MaterialAlive(_original) ? _original : null);
                    if (_runtime.SameMaterial(_runtime.ReadMaterial(_target), _owned))
                        throw new InvalidOperationException("Water restore did not detach the owned material.");
                }
                // A failed read/restore retains ownership: never destroy a possibly attached clone.
                if (_runtime.MaterialAlive(_owned)) _runtime.DestroyMaterial(_owned);
            }
            _target = null;
            _owned = null;
            _original = null;
            _pendingRelease = false;
            LastError = null;
        }

        public void Tick(double now)
        {
            if (!_active || now < _nextCheck) return;
            _nextCheck = now + RetrySeconds; // No tight retries or catch-up after a stalled frame.
            try
            {
                if (_pendingRelease)
                {
                    Release();
                    State = WaterState.Waiting;
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

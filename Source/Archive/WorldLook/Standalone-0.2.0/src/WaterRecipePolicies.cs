// ---------------------------------------------------------------------------
// Water/WaterRecipePolicies.cs
//
// Consolidated recipe, reflection policy and session ownership code. Section
// markers preserve the original type boundaries. The reference-shader contract
// below tracks this package's shader, including its runtime wind input.
//
//   Water/WaterLookRecipe.cs           -> section WaterLookRecipe.cs
//   Water/ReferenceWaterLook.cs        -> section ReferenceWaterLook.cs
//   Water/ReferenceWaterPolicy.cs      -> section ReferenceWaterPolicy.cs
//   Water/WaterSession.cs              -> section WaterSession.cs
//   Rendering/MaterialSlotLease.cs     -> shared material-slot restoration below
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
    /// recipe preserves the clear-shallows tuning: lower absorption, quieter
    /// normal detail and a near-neutral transmission tint. Scene-depth difference
    /// is an eye-space thickness approximation. Reflection and foam coverage are
    /// composed separately in the shader; this recipe does not force opaque water.
    /// Sky/horizon colors and wind activity are live inputs from ReferenceWaterRuntime.
    /// Deep tint, sun highlight and shore-foam values use the shader/material settings.
    /// Surface opacity is independent of the weak environment-color reflection;
    /// the shader and runtime no longer accept an object-reflection texture.
    /// Every write is HasProperty-guarded and Unity-free, so a shader revision that drops a
    /// name degrades to "that value is not applied" instead of faulting the render path.
    /// </summary>
    internal static class ReferenceWaterLook
    {
        private static readonly (string name, float value)[] Floats =
        {
            // Retain the existing late-dye tuning while changing composition.
            ("_Absorption", 0.10f),
            // Quieter normals reduce distortion of the visible bottom.
            ("_RippleStrength", 0.28f),
            ("_SurfaceOpacity", 0.22f),
            ("_ReflectionStrength", 0.12f),
        };

        private static readonly (string name, float r, float g, float b)[] Colors =
        {
            // The tinted body term uses sceneColor * lerp(shallow, deep, dye) * 2.
            // Near 0.5, the shallow transmission gain stays near unity. The final
            // compositor also retains a separate untinted destination contribution.
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
            "_ShallowTint", "_DeepTint", "_Absorption", "_SurfaceOpacity",
            "_ReflectionStrength",
            "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength", "_WindActivity",
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

namespace WorldLook
{
    using System;
    using System.Collections.Generic;

    internal sealed class MaterialSlotFence
    {
        private readonly HashSet<string> _keys = new(StringComparer.Ordinal);
        private int _world = int.MinValue;
        internal void BeginWorld(int world)
        {
            if (_world == world) return;
            _world = world;
            _keys.Clear();
        }
        internal bool IsBlocked(int world, string key) => world == _world && _keys.Contains(key);
        internal void ObserveForeign(int capturedWorld, string key)
        {
            if (capturedWorld == _world) _keys.Add(key);
        }
    }

    internal static class KnownMaterialConsumers
    {
        internal const int MaximumSlots = 64;
        internal static bool Register<T>(List<T> consumers, T candidate, Func<T, T, bool> same,
            Func<T, bool> isAlive) where T : class
        {
            for (int i = consumers.Count - 1; i >= 0; i--)
            {
                if (ReferenceEquals(consumers[i], candidate)) return false;
                try
                {
                    if (!isAlive(consumers[i])) { consumers.RemoveAt(i); continue; }
                    if (same(consumers[i], candidate)) return false;
                }
                catch
                {
                    // Discovery deduplication is optional. Keep this unknown
                    // consumer for strict cleanup checks, but allow a new one.
                }
            }
            consumers.Add(candidate);
            return true;
        }

        internal static bool Contains<T>(T material, int count, Func<int, T?> read, Func<T?, T, bool> same)
            where T : class
        {
            // An oversized or unreadable known renderer is uncertainty, not a
            // negative alias result. Retain resources instead of truncating.
            if (count < 0 || count > MaximumSlots) throw new InvalidOperationException("Known renderer material slots exceed the cleanup safety bound.");
            for (int i = 0; i < count; i++) if (same(read(i), material)) return true;
            return false;
        }
    }

    internal static class NativeMaterialBridgePolicy
    {
        internal static (T sourceSnapshot, T value) Decide<T>(T previousSource, T previousApplied,
            T observedSource, int cloneCount, Func<int, T> readClone, Func<T, T, bool> equal,
            bool writingBackToSource)
        {
            bool sourceChanged = !equal(observedSource, previousSource);
            T chosen = sourceChanged ? observedSource : previousApplied;
            if (!sourceChanged)
                for (int i = 0; i < cloneCount; i++)
                {
                    T current = readClone(i);
                    if (!equal(current, previousApplied)) chosen = current;
                }
            // A backup used as the native source also receives our own bridge
            // writes. Track that write, so its echo cannot override the next
            // real native change on an owned clone.
            return (writingBackToSource ? chosen : observedSource, chosen);
        }
    }

    // Shared with the sky adapter: Unity-free ownership, alongside WaterSession.
    // Native adapter reads may throw. Unknown is never treated as detached.
    internal interface IMaterialSlotRuntime<T> where T : class
    {
        T? ReadSlot();
        void WriteSlot(T material);
        bool CanRestore { get; }
        bool IsAlive(T material);
        bool IsOwnedShader(T material);
        bool IsNativeShader(T material);
        bool Same(T? left, T right);
        void DestroyOwned(T material);
    }

    internal enum MaterialLeaseState
    {
        Prepared, Applied, RestoredOriginal, RestoredBackup,
        ForeignPreserved, ForeignOwned, Deferred, Released
    }

    /// <summary>
    /// A single native slot, prepared before publication and retained until a read
    /// confirms safe detachment. Shared native backups are owned by the caller.
    /// </summary>
    internal sealed class MaterialSlotLease<T> where T : class
    {
        private readonly IMaterialSlotRuntime<T> _runtime;
        private readonly T _source, _original, _backup;
        private bool _releaseRequested, _released;
        internal T Owned { get; }
        internal MaterialLeaseState State { get; private set; }
        internal string? LastError { get; private set; }
        internal bool IsApplied => State == MaterialLeaseState.Applied && !_releaseRequested;
        internal bool ReleaseRequested => _releaseRequested;
        internal bool Relinquished { get; private set; }

        internal MaterialSlotLease(IMaterialSlotRuntime<T> runtime, T source, T original, T backup, T owned)
        {
            _runtime = runtime; _source = source; _original = original; _backup = backup; Owned = owned;
        }

        internal bool Publish(Action? initializeOwned = null)
        {
            if (Relinquished) return false;
            try
            {
                if (!_runtime.CanRestore || !_runtime.IsAlive(Owned) || !_runtime.IsOwnedShader(Owned)
                    || !_runtime.IsNativeShader(_source) || !_runtime.IsNativeShader(_backup)
                    || _runtime.Same(Owned, _source) || _runtime.Same(Owned, _backup))
                    throw new System.InvalidOperationException("Material candidates are not distinct, valid native/owned materials.");
                if (!_runtime.Same(_runtime.ReadSlot(), _source))
                { PreserveForeign(); return false; }
                initializeOwned?.Invoke();
                if (!_runtime.Same(_runtime.ReadSlot(), _source))
                { PreserveForeign(); return false; }
                // The caller registers this lease before invoking Publish. A setter
                // that changes the slot and then throws therefore remains tracked.
                _runtime.WriteSlot(Owned);
                if (!_runtime.Same(_runtime.ReadSlot(), Owned) || !_runtime.IsOwnedShader(Owned))
                    throw new System.InvalidOperationException("Owned material assignment was not confirmed.");
                State = MaterialLeaseState.Applied;
                LastError = null;
                return true;
            }
            catch (System.Exception ex) { Defer(ex); return false; }
        }

        internal bool ValidateOwnership()
        {
            if (_releaseRequested || _released || Relinquished) return false;
            try
            {
                if (_runtime.Same(_runtime.ReadSlot(), Owned) && _runtime.IsOwnedShader(Owned)) return true;
                PreserveForeign();
                _releaseRequested = true;
                return false;
            }
            catch (System.Exception ex) { _releaseRequested = true; Defer(ex); return false; }
        }

        internal MaterialLeaseState Restore()
        {
            _releaseRequested = true;
            if (_released) return State;
            if (Relinquished) return State = MaterialLeaseState.ForeignPreserved;
            try
            {
                T? current = _runtime.ReadSlot();
                if (!_runtime.Same(current, Owned))
                {
                    if (_runtime.Same(current, _backup) && NativeCandidateUsable(_backup))
                        State = MaterialLeaseState.RestoredBackup;
                    else if (_runtime.Same(current, _original) && NativeCandidateUsable(_original))
                        State = MaterialLeaseState.RestoredOriginal;
                    else PreserveForeign();
                    LastError = null;
                    return State;
                }
                if (!_runtime.IsOwnedShader(Owned)) { PreserveForeign(); return State; }
                if (!_runtime.CanRestore)
                    throw new System.InvalidOperationException("The captured world no longer owns this slot generation.");
                bool restoreOriginal = NativeCandidateUsable(_original);
                T candidate = restoreOriginal ? _original
                    : NativeCandidateUsable(_backup) ? _backup
                    : throw new System.InvalidOperationException("No valid native material is available; retaining the owned material.");
                // Recheck immediately before the write, never seize a foreign/empty slot.
                if (!_runtime.Same(_runtime.ReadSlot(), Owned) || !_runtime.IsOwnedShader(Owned))
                { PreserveForeign(); return State; }
                _runtime.WriteSlot(candidate);
                if (!_runtime.Same(_runtime.ReadSlot(), candidate) || !_runtime.IsNativeShader(candidate))
                    throw new System.InvalidOperationException("Native material restoration was not confirmed.");
                State = restoreOriginal
                    ? MaterialLeaseState.RestoredOriginal : MaterialLeaseState.RestoredBackup;
                LastError = null;
                return State;
            }
            catch (System.Exception ex) { Defer(ex); return State; }
        }

        internal bool ReleaseOwned()
        {
            if (_released) return true;
            try
            {
                if (!_runtime.IsAlive(Owned)) { _released = true; State = MaterialLeaseState.Released; return true; }
                if (!_runtime.IsOwnedShader(Owned))
                {
                    // A later owner changed the shader on this same object. It is
                    // no longer ours to destroy, even if it left the original slot.
                    Relinquished = true;
                    _released = true;
                    State = MaterialLeaseState.ForeignOwned;
                    return true;
                }
                if (_runtime.Same(_runtime.ReadSlot(), Owned))
                    throw new System.InvalidOperationException("The owned material may still be attached.");
                _runtime.DestroyOwned(Owned);
                _released = true;
                State = MaterialLeaseState.Released;
                LastError = null;
                return true;
            }
            catch (System.Exception ex) { Defer(ex); return false; }
        }

        private void PreserveForeign()
        {
            Relinquished = true;
            State = _runtime.IsAlive(Owned) && !_runtime.IsOwnedShader(Owned)
                ? MaterialLeaseState.ForeignOwned : MaterialLeaseState.ForeignPreserved;
        }

        private void Defer(System.Exception ex)
        {
            State = MaterialLeaseState.Deferred;
            LastError = ex.GetType().Name + ": " + ex.Message;
        }

        private bool NativeCandidateUsable(T material)
        {
            // An unreadable restoration candidate may be skipped in favor of
            // our valid backup. Slot/owned-identity reads remain strict: their
            // failures still defer all mutation and destruction.
            try { return _runtime.IsNativeShader(material); }
            catch { return false; }
        }
    }
}

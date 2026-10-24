using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace WaterShader
{
    /// <summary>
    /// Supplies the reference water's live environment colors and input checks.
    /// No extra camera, render target, render callback or global shader writes.
    /// ShaderAssets owns the bundle; WaterSession owns the material clone.
    /// </summary>
    internal sealed class ReferenceWaterRuntime : IDisposable
    {
        private const double EnvironmentRefreshSeconds = 0.2;
        private const double InputRequirementsRefreshSeconds = 0.5;
        private const double WarningIntervalSeconds = 5.0;

        private readonly Action<string> _log;
        private readonly Stopwatch _clock = Stopwatch.StartNew();
        private Shader? _shader;
        private Shader? _nativeShader;
        private Material? _bound;
        private bool _initialized;
        private bool _disposed;
        private double _nextEnvironmentRefresh;
        private double _nextInputRequirementsRefresh;
        private double _nextWarning;
        private bool? _lastLoggedInputRequirements;

        public ReferenceWaterRuntime(Action<string> log) { _log = log; }

        internal bool IsReady { get; private set; }
        internal const string ReflectionMode = "environment-only";
        // Retain the old diagnostic keys, but do not imply that a camera exists.
        internal int RenderedFrames => 0;
        internal double LastReflectionMilliseconds => 0;
        internal bool ReflectionAvailable => false;
        internal int LastReflectionFrame => -1;
        internal int ReflectionWidth => 0;
        internal int ReflectionHeight => 0;
        internal int LastSourceCameraInstanceId { get; private set; }
        internal bool? SceneInputsRequested { get; private set; }
        internal string SceneInputStatus { get; private set; } = "not checked";

        internal bool Initialize()
        {
            if (_disposed) return false;
            if (_initialized) return IsReady;
            _initialized = true;
            _nextEnvironmentRefresh = _nextInputRequirementsRefresh = _nextWarning = 0;
            _lastLoggedInputRequirements = null;
            LastSourceCameraInstanceId = 0;
            SceneInputsRequested = null;
            SceneInputStatus = "not checked";
            try
            {
                _shader = WorldLook.ShaderAssets.LoadShader("Assets/S1Mods/ReferenceWater.shader", _log);
                if (!IsUsable(_shader) || !_shader!.isSupported)
                {
                    _log("[water-shader] embedded reference shader unavailable or unsupported; native recipe stays active");
                    _shader = null;
                    return false;
                }
                IsReady = true;
                _log($"[water-shader] reference ready: shader={_shader.name} reflectionMode={ReflectionMode} ownedCameras=0 bound=false");
                return true;
            }
            catch (Exception ex)
            {
                IsReady = false;
                _shader = null;
                _log($"[water-shader] reference init failed; native recipe stays active: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>Validate before publishing; a stale bundle without _SurfaceOpacity is rejected.</summary>
        internal bool TryConfigure(Material clone, float waterY)
        {
            Shader? shader = _shader;
            if (_disposed || !IsUsable(clone) || !IsReady || !IsUsable(shader)) return false;
            Shader? originalShader = clone.shader;
            try
            {
                if (!CheckSceneInputRequirements(ResolveSourceCamera())) return false;
                clone.shader = shader;
                string[] missing = ReferenceWaterPolicy.MissingRequiredProperties(clone.HasProperty);
                if (!ReferenceWaterPolicy.CanApplyReference(true, shader!.isSupported, missing.Length == 0))
                {
                    clone.shader = originalShader;
                    _log($"[water-shader] reference rejected: supported={shader.isSupported} missing=[{string.Join(", ", missing)}]; native recipe stays active");
                    return false;
                }
                _bound = clone;
                _nativeShader = originalShader;
                UpdateEnvironment(force: true);
                _log($"[water-shader] reference material bound: shader={shader.name} waterY={waterY:F3} properties={ReferenceWaterPolicy.RequiredShaderProperties.Length} reflectionMode={ReflectionMode}");
                return true;
            }
            catch (Exception ex)
            {
                try { clone.shader = originalShader; } catch { }
                if (ReferenceEquals(_bound, clone)) { _bound = null; _nativeShader = null; }
                _log($"[water-shader] reference configure failed; native recipe stays active: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>Called by WaterShaderMod; cadence-bounded reads, never a nested render.</summary>
        internal void Tick()
        {
            if (_disposed || !IsReady || ReferenceEquals(_bound, null)) return;
            if (!IsUsable(_bound)) { _bound = null; _nativeShader = null; return; }
            double now = _clock.Elapsed.TotalSeconds;
            if (now < _nextEnvironmentRefresh && now < _nextInputRequirementsRefresh) return;
            try
            {
                // A later shader change is not ours to rewrite or restore.
                if (!WorldLook.WorldLookEnvironment.SameObject(_bound!.shader, _shader))
                { _bound = null; _nativeShader = null; return; }
                Camera? source = ResolveSourceCamera();
                if (IsUsable(source)) LastSourceCameraInstanceId = source!.GetInstanceID();
                if (now >= _nextInputRequirementsRefresh && !CheckSceneInputRequirements(source))
                { RestoreNativeShaderAfterInputFailure(); return; }
                UpdateEnvironment(force: false);
            }
            catch (Exception ex) { WarnThrottled($"environment tick skipped: {ex.GetType().Name}: {ex.Message}"); }
        }

        private static Camera? ResolveSourceCamera()
        {
            try { Camera main = Camera.main; return IsUsable(main) ? main : null; }
            catch { return null; }
        }

        private bool CheckSceneInputRequirements(Camera? source)
        {
            _nextInputRequirementsRefresh = _clock.Elapsed.TotalSeconds + InputRequirementsRefreshSeconds;
            try
            {
                UniversalRenderPipelineAsset? pipeline = UniversalRenderPipeline.asset;
                if (!IsUsable(pipeline))
                {
                    SceneInputsRequested = false;
                    SceneInputStatus = "active URP asset unavailable";
                }
                else
                {
                    UniversalAdditionalCameraData? data = IsUsable(source)
                        ? source!.GetComponent<UniversalAdditionalCameraData>() : null;
                    CameraOverrideOption depthOption = IsUsable(data)
                        ? data!.requiresDepthOption : CameraOverrideOption.UsePipelineSettings;
                    CameraOverrideOption colorOption = IsUsable(data)
                        ? data!.requiresColorOption : CameraOverrideOption.UsePipelineSettings;
                    bool depth = ResolveInputRequirement(depthOption, pipeline!.supportsCameraDepthTexture);
                    bool color = ResolveInputRequirement(colorOption, pipeline.supportsCameraOpaqueTexture);
                    SceneInputsRequested = depth && color;
                    SceneInputStatus = $"depth={depth} ({depthOption}), opaque={color} ({colorOption}); "
                        + (IsUsable(source) ? "main-camera settings" : "camera not ready; pipeline defaults")
                        + "; request check only, not a current-frame texture-handle validation";
                }
            }
            catch (Exception ex)
            {
                SceneInputsRequested = false;
                SceneInputStatus = $"request check failed: {ex.GetType().Name}: {ex.Message}";
            }
            bool requested = SceneInputsRequested == true;
            if (_lastLoggedInputRequirements != requested)
            {
                _lastLoggedInputRequirements = requested;
                _log($"[water-shader] scene inputs: {SceneInputStatus}"
                    + (requested ? "" : "; reference shader declined, native recipe selected"));
            }
            return requested;
        }

        private static bool ResolveInputRequirement(CameraOverrideOption option, bool pipelineValue)
            => option == CameraOverrideOption.On
               || (option == CameraOverrideOption.UsePipelineSettings && pipelineValue);

        private void RestoreNativeShaderAfterInputFailure()
        {
            Material? material = _bound;
            Shader? nativeShader = _nativeShader;
            _bound = null;
            _nativeShader = null;
            try
            {
                if (!IsUsable(material) || !IsUsable(nativeShader))
                    throw new InvalidOperationException("Owned material or original shader is no longer usable.");
                // Only our exact reference shader may be reverted on this owned clone.
                if (!WorldLook.WorldLookEnvironment.SameObject(material!.shader, _shader)) return;
                material.shader = nativeShader;
                _log(WaterLookRecipe.Apply(new NativeFallbackMaterial(material)));
                _log("[water-shader] native shader restored after input requirements changed; reference can resume on the next enable or scene load");
            }
            catch (Exception ex) { WarnThrottled($"native shader fallback failed: {ex.GetType().Name}: {ex.Message}"); }
        }

        private sealed class NativeFallbackMaterial : IWaterLookMaterial
        {
            private readonly Material _material;
            internal NativeFallbackMaterial(Material material) { _material = material; }
            public bool HasProperty(string name) => _material.HasProperty(name);
            public void SetFloat(string name, float value) => _material.SetFloat(name, value);
            public void SetColor(string name, float r, float g, float b, float alpha)
                => _material.SetColor(name, new Color(r, g, b, alpha));
        }

        private void UpdateEnvironment(bool force)
        {
            double now = _clock.Elapsed.TotalSeconds;
            if (!force && now < _nextEnvironmentRefresh) return;
            _nextEnvironmentRefresh = now + EnvironmentRefreshSeconds;
            if (!IsUsable(_bound)) return;
            try
            {
                var environment = WorldLook.WorldLookEnvironment.Current;
                Color sky, horizon;
                float windActivity = 0f;
                if (environment.IsValid)
                {
                    sky = environment.SkyTopColor;
                    horizon = environment.SkyHorizonColor;
                    windActivity = environment.Windy;
                }
                else if (RenderSettings.ambientMode == AmbientMode.Flat)
                    sky = horizon = RenderSettings.ambientLight;
                else
                {
                    sky = RenderSettings.ambientSkyColor;
                    horizon = RenderSettings.ambientEquatorColor;
                }
                if (IsFinite(sky) && IsFinite(horizon))
                {
                    _bound!.SetColor("_SkyColor", sky);
                    _bound.SetColor("_AmbientColor", horizon);
                }
                _bound!.SetFloat("_WindActivity", float.IsFinite(windActivity) ? Math.Clamp(windActivity, 0f, 1f) : 0f);
            }
            catch (Exception ex) { WarnThrottled($"environment update skipped: {ex.GetType().Name}: {ex.Message}"); }
        }

        private static bool IsFinite(Color value)
            => float.IsFinite(value.r) && float.IsFinite(value.g) && float.IsFinite(value.b) && float.IsFinite(value.a);

        private void WarnThrottled(string message)
        {
            double now = _clock.Elapsed.TotalSeconds;
            if (now < _nextWarning) return;
            _nextWarning = now + WarningIntervalSeconds;
            _log("[water-shader] " + message);
        }

        private static bool IsUsable(UnityEngine.Object? value)
        {
            if (ReferenceEquals(value, null)) return false;
            try { return !value.WasCollected && value.Pointer != IntPtr.Zero && value != null; }
            catch { return false; }
        }

        internal void Release()
        {
            // WaterSession restores/destroys its own clone after this reference is detached.
            _bound = null;
            _nativeShader = null;
            _shader = null;
            IsReady = false;
            _initialized = false;
            _nextEnvironmentRefresh = _nextInputRequirementsRefresh = 0;
            LastSourceCameraInstanceId = 0;
            SceneInputsRequested = null;
            SceneInputStatus = "released";
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Release();
        }
    }
}

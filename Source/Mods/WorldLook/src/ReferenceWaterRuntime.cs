using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace WaterShader
{
    /// <summary>
    /// Supplies live native environment colors and checks camera input requests.
    /// ShaderAssets owns the bundle; WaterSession owns the material clone.
    /// This adapter creates no camera/RT and never renders or writes shader globals.
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
        internal int LastSourceCameraInstanceId { get; private set; }
        internal bool? SceneInputsRequested { get; private set; }
        internal string SceneInputStatus { get; private set; } = "not checked";
        internal string MaterialStatus { get; private set; } = "not configured";
        internal string EnvironmentStatus { get; private set; } = "not checked";

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
                if (!WaterEnvironment.IsUsable(_shader) || !_shader!.isSupported)
                {
                    _shader = null;
                    _log("[water-shader] reference shader unavailable or unsupported; native recipe will be used if binding succeeds");
                    return false;
                }
                IsReady = true;
                _log($"[water-shader] reference ready: shader={_shader.name} reflectionMode={ReflectionMode} ownedCameras=0");
                return true;
            }
            catch (Exception ex)
            {
                IsReady = false;
                _shader = null;
                _log($"[water-shader] reference init failed: {ex.GetType().Name}: {ex.Message}; native recipe will be attempted");
                return false;
            }
        }

        /// <summary>Prepare before publication; a stale shader contract is rejected.</summary>
        internal bool TryConfigure(Material clone, float waterY)
        {
            Detach();
            MaterialStatus = "native recipe pending";
            Shader? shader = _shader;
            if (_disposed || !WaterEnvironment.IsUsable(clone) || !IsReady || !WaterEnvironment.IsUsable(shader)) return false;
            if (!CheckSceneInputRequirements(ResolveSourceCamera())) return false;
            Shader originalShader = clone.shader;
            if (!WaterEnvironment.IsAlive(originalShader))
                throw new InvalidOperationException("Native water shader unavailable before reference configuration.");
            try
            {
                AssignShader(clone, shader!);
                string[] missing = ReferenceWaterPolicy.MissingRequiredProperties(clone.HasProperty);
                if (!ReferenceWaterPolicy.CanApplyReference(true, shader!.isSupported, missing.Length == 0))
                    throw new InvalidOperationException($"Reference shader contract rejected: supported={shader.isSupported} missing=[{string.Join(", ", missing)}]");
                _bound = clone;
                _nativeShader = originalShader;
                if (!UpdateEnvironment())
                    throw new InvalidOperationException("No finite native or ambient environment colors available for initial water configuration.");
                MaterialStatus = "reference prepared";
                _log($"[water-shader] reference material prepared: shader={shader.name} waterY={waterY:F3} properties={ReferenceWaterPolicy.RequiredShaderProperties.Length} reflectionMode={ReflectionMode}");
                return true;
            }
            catch (Exception ex)
            {
                Detach();
                // Do not swallow a failed rollback: WaterSession must retain and clean
                // this candidate, not publish an unconfirmed shader as a native fallback.
                MaterialStatus = "shader rollback pending";
                AssignShader(clone, originalShader);
                MaterialStatus = "native recipe pending";
                _log($"[water-shader] reference configuration rejected, native shader restored: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        internal void MarkNativeRecipeConfigured() => MaterialStatus = "native recipe configured";

        /// <summary>Called only while the session has a confirmed applied material.</summary>
        internal void Tick()
        {
            if (_disposed || !IsReady || ReferenceEquals(_bound, null)) return;
            double now = _clock.Elapsed.TotalSeconds;
            bool environmentDue = now >= _nextEnvironmentRefresh;
            bool inputsDue = now >= _nextInputRequirementsRefresh;
            if (!environmentDue && !inputsDue) return;
            // Deadlines precede native access, including failing proxy reads.
            if (environmentDue) _nextEnvironmentRefresh = now + EnvironmentRefreshSeconds;
            if (inputsDue) _nextInputRequirementsRefresh = now + InputRequirementsRefreshSeconds;
            if (!WaterEnvironment.IsAlive(_bound)) { Detach(); return; }
            if (!WaterEnvironment.SameObject(_bound!.shader, _shader!))
            {
                Detach();
                MaterialStatus = "foreign shader; environment updates detached";
                return;
            }
            if (inputsDue && !CheckSceneInputRequirements(ResolveSourceCamera()))
            {
                RestoreNativeShaderAfterInputFailure();
                return;
            }
            if (environmentDue) UpdateEnvironment();
            MaterialStatus = "reference active";
        }

        private static Camera? ResolveSourceCamera()
        {
            try { Camera main = Camera.main; return WaterEnvironment.IsUsable(main) ? main : null; }
            catch { return null; }
        }

        private bool CheckSceneInputRequirements(Camera? source)
        {
            _nextInputRequirementsRefresh = _clock.Elapsed.TotalSeconds + InputRequirementsRefreshSeconds;
            try
            {
                LastSourceCameraInstanceId = WaterEnvironment.IsUsable(source) ? source!.GetInstanceID() : 0;
                UniversalRenderPipelineAsset? pipeline = UniversalRenderPipeline.asset;
                if (!WaterEnvironment.IsUsable(pipeline))
                {
                    SceneInputsRequested = false;
                    SceneInputStatus = "active URP asset unavailable";
                }
                else
                {
                    UniversalAdditionalCameraData? data = WaterEnvironment.IsUsable(source)
                        ? source!.GetComponent<UniversalAdditionalCameraData>() : null;
                    CameraOverrideOption depthOption = WaterEnvironment.IsUsable(data)
                        ? data!.requiresDepthOption : CameraOverrideOption.UsePipelineSettings;
                    CameraOverrideOption colorOption = WaterEnvironment.IsUsable(data)
                        ? data!.requiresColorOption : CameraOverrideOption.UsePipelineSettings;
                    bool depth = ResolveInputRequirement(depthOption, pipeline!.supportsCameraDepthTexture);
                    bool color = ResolveInputRequirement(colorOption, pipeline.supportsCameraOpaqueTexture);
                    SceneInputsRequested = depth && color;
                    SceneInputStatus = $"depth={depth} ({depthOption}), opaque={color} ({colorOption}); "
                        + (WaterEnvironment.IsUsable(source) ? "main-camera settings" : "camera not ready; pipeline defaults")
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
                    + (requested ? "" : "; native fallback required"));
            }
            return requested;
        }

        private static bool ResolveInputRequirement(CameraOverrideOption option, bool pipelineValue)
            => option == CameraOverrideOption.On
                || (option == CameraOverrideOption.UsePipelineSettings && pipelineValue);

        private static void AssignShader(Material material, Shader shader)
            => WaterShaderAssignment.AssignConfirmed<Shader>(shader, WaterEnvironment.IsAlive,
                value => material.shader = value, () => material.shader, WaterEnvironment.SameObject);

        private void RestoreNativeShaderAfterInputFailure()
        {
            Material? material = _bound;
            Shader? nativeShader = _nativeShader;
            Detach();
            MaterialStatus = "native fallback pending";
            if (!WaterEnvironment.IsAlive(material) || !WaterEnvironment.IsAlive(nativeShader))
                throw new InvalidOperationException("Owned water material or original shader is unavailable for fallback.");
            if (!WaterEnvironment.SameObject(material!.shader, _shader!))
            {
                MaterialStatus = "foreign shader; fallback declined";
                return;
            }
            // Exceptions reach the session owner, which schedules restoration of the
            // original material. Never report a successful fallback before readback.
            AssignShader(material, nativeShader!);
            _log(WaterLookRecipe.Apply(new NativeFallbackMaterial(material)));
            MaterialStatus = "native recipe configured";
            _log("[water-shader] native shader fallback confirmed after input requests changed; reference resumes on the next enable or scene load");
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

        private bool UpdateEnvironment()
        {
            _nextEnvironmentRefresh = _clock.Elapsed.TotalSeconds + EnvironmentRefreshSeconds;
            bool valid = WaterEnvironment.Read(out Color sky, out Color horizon, out float wind, out string source);
            EnvironmentStatus = source;
            if (valid)
            {
                _bound!.SetColor("_SkyColor", sky);
                _bound.SetColor("_AmbientColor", horizon);
            }
            else WarnThrottled("native and ambient colors unavailable; retaining the last finite water environment");
            _bound!.SetFloat("_WindActivity", wind);
            return valid;
        }

        private void WarnThrottled(string message)
        {
            double now = _clock.Elapsed.TotalSeconds;
            if (now < _nextWarning) return;
            _nextWarning = now + WarningIntervalSeconds;
            _log("[water-shader] " + message);
        }

        internal void Detach()
        {
            _bound = null;
            _nativeShader = null;
        }

        internal void Release()
        {
            // WaterSession restores/destroys its clone after the updater is detached.
            Detach();
            _shader = null;
            IsReady = false;
            _initialized = false;
            _nextEnvironmentRefresh = _nextInputRequirementsRefresh = 0;
            LastSourceCameraInstanceId = 0;
            SceneInputsRequested = null;
            SceneInputStatus = "released";
            EnvironmentStatus = "released";
            MaterialStatus = "released";
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Release();
        }
    }
}

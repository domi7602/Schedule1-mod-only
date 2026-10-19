extern alias il2cpp;

using System;
using System.Diagnostics;
using System.IO;
using Il2CppInterop.Runtime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace WaterShader
{
    /// <summary>
    /// Owns the reference water shader bundle, one disabled reflection camera and
    /// one bounded reflection render target. It only re-shades the material clone
    /// the WaterSession already owns; the native material slot stays the session's
    /// business. Every entry point degrades to "native recipe stays active" rather
    /// than throwing into the game's render loop.
    /// </summary>
    internal sealed class ReferenceWaterRuntime : IDisposable
    {
        /// <summary>Long-side budget for the reflection render target.</summary>
        private const int MaxReflectionDimension = 512;

        /// <summary>Fixed offset for the oblique near clip plane.</summary>
        private const float ClipPlaneOffset = 0.07f;

        private readonly Action<string> _log;

        private AssetBundle? _bundle;
        private Shader? _shader;
        private Material? _bound;
        private Camera? _reflectionCamera;
        private GameObject? _reflectionCameraObject;
        private RenderTexture? _reflectionTexture;
        private Camera? _sourceCamera;

        // IL2CPP GC safety: the marshalled delegate must stay rooted on the managed
        // side while the native event holds it (same reason as S1Mods.Shared
        // SceneGate). An inline temporary would be collectable mid-session.
        private il2cpp::Il2CppSystem.Action<ScriptableRenderContext, Camera>? _beginCameraRendering;

        private bool _subscribed;
        private bool _initialized;
        private bool _disposed;
        private bool _rendering;
        private float _waterY;

        public ReferenceWaterRuntime(Action<string> log)
        {
            _log = log;
        }

        /// <summary>True when the bundle loaded and the shader is usable.</summary>
        internal bool IsReady { get; private set; }

        /// <summary>Reflection frames actually rendered since the last init.</summary>
        internal int RenderedFrames { get; private set; }

        /// <summary>Milliseconds spent in the most recent reflection render.</summary>
        internal double LastReflectionMilliseconds { get; private set; }

        /// <summary>
        /// Load the bundle and prepare the owned reflection camera. Idempotent
        /// while healthy; a failure releases everything and leaves the native
        /// recipe as the active path.
        /// </summary>
        internal bool Initialize()
        {
            if (_disposed) return false;
            if (_initialized) return IsReady;
            _initialized = true;
            RenderedFrames = 0;

            try
            {
                string modsDirectory = Path.GetDirectoryName(typeof(ReferenceWaterRuntime).Assembly.Location) ?? ".";
                string bundlePath = Path.Combine(modsDirectory, "s1reference.bundle");
                if (!File.Exists(bundlePath))
                {
                    _log($"[water-shader] reference bundle missing at {bundlePath}; native recipe stays active");
                    return false;
                }

                _bundle = AssetBundle.LoadFromFile(bundlePath);
                if (_bundle == null)
                {
                    _log("[water-shader] reference bundle failed to load; native recipe stays active");
                    return false;
                }

                _shader = _bundle.LoadAsset<Shader>("Assets/S1Mods/ReferenceWater.shader");
                if (_shader == null)
                {
                    _log("[water-shader] reference shader missing in bundle; native recipe stays active");
                    ReleaseResources();
                    return false;
                }

                if (!_shader.isSupported)
                {
                    _log($"[water-shader] reference shader unsupported here ({_shader.name}); native recipe stays active");
                    ReleaseResources();
                    return false;
                }

                _reflectionCameraObject = new GameObject("WaterShader_ReflectionCamera");
                _reflectionCameraObject.hideFlags = HideFlags.HideAndDontSave;
                _reflectionCamera = _reflectionCameraObject.AddComponent<Camera>();
                _reflectionCamera.enabled = false;
                _reflectionCamera.cameraType = CameraType.Reflection;
                _reflectionCamera.clearFlags = CameraClearFlags.SolidColor;
                _reflectionCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);

                _beginCameraRendering = DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action<ScriptableRenderContext, Camera>>(
                    new Action<ScriptableRenderContext, Camera>(HandleBeginCameraRendering));
                RenderPipelineManager.add_beginCameraRendering(_beginCameraRendering);
                _subscribed = true;

                IsReady = true;
                _log($"[water-shader] reference ready: shader={_shader.name} maxReflection={MaxReflectionDimension} bound=false");
                return true;
            }
            catch (Exception ex)
            {
                _log($"[water-shader] reference init failed, native recipe stays active: {ex.GetType().Name}: {ex.Message}");
                ReleaseResources();
                return false;
            }
        }

        /// <summary>
        /// Point the clone at the reference shader. Reversible: on any failure the
        /// clone's original shader is restored so the caller can fall back to the
        /// native recipe on an untouched material.
        /// </summary>
        internal bool TryConfigure(Material clone, float waterY)
        {
            if (_disposed || clone == null) return false;
            if (!IsReady || _shader == null) return false;

            Shader? originalShader = clone.shader;
            try
            {
                clone.shader = _shader;

                string[] missing = ReferenceWaterPolicy.MissingRequiredProperties(clone.HasProperty);
                if (!ReferenceWaterPolicy.CanApplyReference(
                        bundleLoaded: _bundle != null,
                        shaderSupported: _shader.isSupported,
                        hasRequiredProperties: missing.Length == 0))
                {
                    clone.shader = originalShader;
                    _log($"[water-shader] reference rejected: bundle={_bundle != null} supported={_shader.isSupported} missing=[{string.Join(", ", missing)}]; native recipe stays active");
                    return false;
                }

                _bound = clone;
                _waterY = waterY;
                _log($"[water-shader] reference material bound: shader={_shader.name} waterY={waterY:F3} properties={ReferenceWaterPolicy.RequiredShaderProperties.Length}");
                return true;
            }
            catch (Exception ex)
            {
                try { clone.shader = originalShader; } catch { }
                _log($"[water-shader] reference configure failed, native recipe stays active: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        private void HandleBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (_disposed || _rendering) return;

            // A clone destroyed by the session must drop the bind and clear the
            // global reflection inputs, otherwise a stale frame stays latched and
            // a later freshly configured clone samples it instead of the sky
            // fallback. Runs once: the bind is nulled immediately after.
            if (!ReferenceEquals(_bound, null) && !IsUsable(_bound))
            {
                _bound = null;
                try { Shader.SetGlobalFloat("_ReflectionAvailable", 0f); } catch { }
                return;
            }

            if (ReferenceEquals(_bound, null)) return;

            try
            {
                Camera? source = ResolveSourceCamera();
                if (source == null || !ReferenceEquals(camera, source)) return;

                if (!ReferenceWaterPolicy.ShouldRenderReflection(
                        reentrancyGateOpen: !_rendering,
                        cameraIsOwned: IsOwnedCamera(camera),
                        cameraIsReflection: camera.cameraType == CameraType.Reflection,
                        cameraIsOverlay: IsOverlayCamera(camera),
                        cameraIsPreview: camera.cameraType == CameraType.Preview,
                        cameraIsGame: UniversalRenderPipeline.IsGameCamera(camera),
                        materialBound: _bound != null))
                    return;

                RenderReflection(context, source);
            }
            catch (Exception ex)
            {
                // A reflection failure must never break the game's own render.
                _log($"[water-shader] reflection frame skipped: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private void RenderReflection(ScriptableRenderContext context, Camera source)
        {
            if (_reflectionCamera == null) return;
            if (!EnsureReflectionTexture(source)) return;

            long started = Stopwatch.GetTimestamp();

            Vector3 position = source.transform.position;
            var (px, py, pz) = ReferenceWaterPolicy.MirrorPoint(_waterY, position.x, position.y, position.z);

            Vector3 forward = source.transform.forward;
            Vector3 up = source.transform.up;
            var (fx, fy, fz) = ReferenceWaterPolicy.MirrorDirection(forward.x, forward.y, forward.z);
            var (ux, uy, uz) = ReferenceWaterPolicy.MirrorDirection(up.x, up.y, up.z);

            var mirroredForward = new Vector3(fx, fy, fz);
            var mirroredUp = new Vector3(ux, uy, uz);
            if (mirroredForward.sqrMagnitude < 1e-6f || mirroredUp.sqrMagnitude < 1e-6f) return;

            _reflectionCamera.CopyFrom(source);
            _reflectionCamera.transform.SetPositionAndRotation(
                new Vector3(px, py, pz),
                Quaternion.LookRotation(mirroredForward.normalized, mirroredUp.normalized));
            _reflectionCamera.targetTexture = _reflectionTexture;
            _reflectionCamera.cameraType = CameraType.Reflection;
            _reflectionCamera.enabled = false;

            // Oblique near plane: nothing below the water surface may reach the
            // reflection, or it would sample what is under the surface.
            Vector4 clipPlane = CameraSpacePlane(_reflectionCamera, new Vector3(0f, _waterY, 0f), Vector3.up, 1.0f);
            _reflectionCamera.projectionMatrix = _reflectionCamera.CalculateObliqueMatrix(clipPlane);

            _rendering = true; // close the reentrancy gate BEFORE the nested render
            try
            {
                // The mirrored pose above is a proper rotation (LookRotation), so
                // the rasterized winding is already normal here. Flipping the
                // global culling sense is only the compensation for a
                // reflection-matrix camera, which this construction does not use -
                // doing it would render the reflection inside-out.
                UniversalRenderPipeline.RenderSingleCamera(context, _reflectionCamera);
            }
            finally
            {
                // Release the gate even when the nested render threw.
                _rendering = false;
            }

            BindReflectionInputs();
            LastReflectionMilliseconds = (Stopwatch.GetTimestamp() - started) * 1000.0 / Stopwatch.Frequency;
            RenderedFrames++;
        }

        private bool EnsureReflectionTexture(Camera source)
        {
            if (_reflectionTexture != null && _reflectionTexture.IsCreated()) return true;

            try
            {
                ReleaseReflectionTexture();
                var (width, height) = ReferenceWaterPolicy.ReflectionSizeForAspect(
                    source.pixelWidth, source.pixelHeight, MaxReflectionDimension);

                _reflectionTexture = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32)
                {
                    name = "WaterShader_Reflection",
                    useMipMap = false,
                    autoGenerateMips = false,
                };

                if (!_reflectionTexture.Create())
                {
                    _log($"[water-shader] reflection target creation failed ({width}x{height}); frame skipped");
                    ReleaseReflectionTexture();
                    return false;
                }

                _log($"[water-shader] reflection target created: {width}x{height} (source {source.pixelWidth}x{source.pixelHeight}, max {MaxReflectionDimension})");
                return true;
            }
            catch (Exception ex)
            {
                _log($"[water-shader] reflection target failed: {ex.GetType().Name}: {ex.Message}");
                ReleaseReflectionTexture();
                return false;
            }
        }

        private void BindReflectionInputs()
        {
            Material? material = _bound;
            if (!ReferenceWaterPolicy.ShouldBindReflection(
                    materialBound: material != null,
                    reflectionTextureReady: _reflectionTexture != null && _reflectionTexture.IsCreated()))
                return;
            if (material == null || _reflectionTexture == null || _reflectionCamera == null) return;

            Matrix4x4 projection = GL.GetGPUProjectionMatrix(_reflectionCamera.projectionMatrix, true);
            Matrix4x4 viewProjection = projection * _reflectionCamera.worldToCameraMatrix;

            // _ReflectionVP / _ReflectionAvailable are CBUFFER-only uniforms:
            // Material.HasProperty reports false for them (measured by
            // BuildReference.ProbeBinding) while the material sheet round-trips
            // them, so the material path is the primary binding. The global write
            // is a cheap redundancy for the same two names.
            material.SetTexture("_ReflectionTex", _reflectionTexture);
            material.SetMatrix("_ReflectionVP", viewProjection);
            material.SetFloat("_ReflectionAvailable", 1f);
            Shader.SetGlobalMatrix("_ReflectionVP", viewProjection);
            Shader.SetGlobalFloat("_ReflectionAvailable", 1f);
        }

        private static Vector4 CameraSpacePlane(Camera camera, Vector3 position, Vector3 normal, float sideSign)
        {
            Vector3 offsetPosition = position + normal * ClipPlaneOffset;
            Matrix4x4 worldToCamera = camera.worldToCameraMatrix;
            Vector3 cameraPosition = worldToCamera.MultiplyPoint(offsetPosition);
            Vector3 cameraNormal = worldToCamera.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(cameraNormal.x, cameraNormal.y, cameraNormal.z,
                               -Vector3.Dot(cameraPosition, cameraNormal));
        }

        private Camera? ResolveSourceCamera()
        {
            if (IsUsable(_sourceCamera)) return _sourceCamera;
            _sourceCamera = null;
            try
            {
                Camera main = Camera.main;
                if (IsUsable(main)) _sourceCamera = main;
            }
            catch { }

            return _sourceCamera;
        }

        private bool IsOwnedCamera(Camera camera)
        {
            Camera? own = _reflectionCamera;
            if (!IsUsable(own) || !IsUsable(camera)) return false;
            try { return own!.Pointer == camera.Pointer; }
            catch { return false; }
        }

        private static bool IsOverlayCamera(Camera camera)
        {
            try
            {
                UniversalAdditionalCameraData? data = camera.GetComponent<UniversalAdditionalCameraData>();
                return data != null && data.renderType == CameraRenderType.Overlay;
            }
            catch
            {
                return false;
            }
        }

        // Soft liveness for a read-only render path: unlike the session's ownership
        // guard this must never throw into the render loop, so a collected proxy is
        // reported as unusable instead of faulting the caller.
        private static bool IsUsable(UnityEngine.Object? value)
        {
            if (ReferenceEquals(value, null)) return false;
            try
            {
                if (value.WasCollected) return false;
                return value.Pointer != IntPtr.Zero && value != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Release render resources but stay re-initializable.</summary>
        internal void Release()
        {
            Unsubscribe();

            try { Shader.SetGlobalFloat("_ReflectionAvailable", 0f); } catch { }

            Material? material = _bound;
            _bound = null;
            if (material != null)
            {
                try { material.SetFloat("_ReflectionAvailable", 0f); } catch { }
                try { material.SetTexture("_ReflectionTex", null); } catch { }
            }

            ReleaseResources();
            _initialized = false;
            IsReady = false;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Release();
        }

        private void Unsubscribe()
        {
            if (!_subscribed) return;
            _subscribed = false;

            var handler = _beginCameraRendering;
            _beginCameraRendering = null;
            if (handler == null) return;

            try { RenderPipelineManager.remove_beginCameraRendering(handler); }
            catch (Exception ex) { _log($"[water-shader] reflection unsubscribe failed: {ex.GetType().Name}: {ex.Message}"); }
        }

        private void ReleaseResources()
        {
            ReleaseReflectionTexture();

            if (_reflectionCamera != null)
            {
                try { _reflectionCamera.targetTexture = null; } catch { }
                _reflectionCamera = null;
            }

            if (_reflectionCameraObject != null)
            {
                try { UnityEngine.Object.Destroy(_reflectionCameraObject); } catch { }
                _reflectionCameraObject = null;
            }

            _shader = null;

            if (_bundle != null)
            {
                // false: a live clone may still reference the shader while the
                // session restores the native material.
                try { _bundle.Unload(false); } catch { }
                _bundle = null;
            }

            _sourceCamera = null;
        }

        private void ReleaseReflectionTexture()
        {
            RenderTexture? texture = _reflectionTexture;
            _reflectionTexture = null;
            if (texture == null) return;

            try
            {
                if (texture.IsCreated()) texture.Release();
                UnityEngine.Object.Destroy(texture);
            }
            catch { }
        }
    }
}

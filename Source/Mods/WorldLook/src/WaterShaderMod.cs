using System;
using System.Diagnostics;
using System.Collections.Generic;
using Il2CppScheduleOne.Map;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WaterShader
{
    /// <summary>Owns the clear-water material session on one known ocean renderer.</summary>
    public sealed class WaterShaderMod
    {
        private readonly Log LoggerInstance = new();
        private readonly Stopwatch _clock = Stopwatch.StartNew();
        private WaterSession<MeshRenderer, Material>? _session;
        private NativeWaterRuntime? _runtime;
        private ReferenceWaterRuntime? _reference;
        private WaterState _loggedState = WaterState.Stopped;
        private bool _shutdown;
        private bool _enabled = true;

        // Cached state only; on-demand tooling must not discover or clone water resources.
        internal string AutomationState => _session?.State.ToString() ?? "Uninitialized";
        internal bool ReferenceReady => _reference?.IsReady == true;

        internal bool SetDiagnosticMode(int mode)
        {
            if (_shutdown || !_enabled || _session == null || _reference == null)
                throw new InvalidOperationException("reference-water-unavailable");
            try
            {
                return _session.SetDiagnosticMode(mode, _reference.MatchesBoundShader,
                    material => material.HasProperty("_DiagnosticMode"),
                    material => material.GetFloat("_DiagnosticMode"),
                    (material, value) => material.SetFloat("_DiagnosticMode", value),
                    _clock.Elapsed.TotalSeconds);
            }
            catch
            {
                if (_session.State == WaterState.Faulted) _reference.Detach();
                throw;
            }
        }

        // One known direct child only. sharedMaterial is borrowed; never use material,
        // assign, instantiate, or destroy anything on this read-only path.
        internal object ReadMaterialDiagnostic()
        {
            var map = Map.instance;
            if (!WaterEnvironment.IsAlive(map)) throw new InvalidOperationException("map-unavailable");
            var root = map.transform;
            var scene = map.gameObject.scene;
            if (!scene.IsValid() || !scene.isLoaded || scene.name != "Main" || root.name != "Map" || root.parent != null)
                throw new InvalidOperationException("unexpected-map");
            var water = root.Find("StylizedWater2_Ocean");
            if (!WaterEnvironment.IsAlive(water)) throw new InvalidOperationException("ocean-unavailable");
            var renderer = water.GetComponent<MeshRenderer>();
            if (!WaterEnvironment.IsAlive(renderer)) throw new InvalidOperationException("renderer-unavailable");
            var material = _session?.GetAttachedOwnedMaterial()
                ?? throw new InvalidOperationException("owned-water-not-attached");
            if (_reference == null || !_reference.MatchesBoundShader(material)
                || !WaterEnvironment.SameObject(renderer.sharedMaterial, material))
                throw new InvalidOperationException("reference-water-shader-mismatch");
            var shader = material.shader;
            if (!WaterEnvironment.IsAlive(shader)) throw new InvalidOperationException("shader-unavailable");
            var floats = new Dictionary<string, float?>();
            foreach (string name in new[] { "_DiagnosticMode", "_Absorption", "_SurfaceOpacity", "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength", "_WindActivity", "_WaveHeight", "_WaveCount" })
                floats[name] = material.HasProperty(name) ? material.GetFloat(name) : null;
            var colors = new Dictionary<string, float[]?>();
            foreach (string name in new[] { "_ShallowTint", "_DeepTint", "_ShallowColor" })
            {
                if (!material.HasProperty(name)) { colors[name] = null; continue; }
                var color = material.GetColor(name);
                colors[name] = new[] { color.r, color.g, color.b, color.a };
            }
            var bounds = renderer.bounds;
            return new { renderer = water.name, name = material.name, shader = shader.name,
                shaderSupported = shader.isSupported, bounds = new { center = new[] { bounds.center.x, bounds.center.y, bounds.center.z }, size = new[] { bounds.size.x, bounds.size.y, bounds.size.z } }, floats, colors };
        }

        public void OnLateInitializeMelon()
        {
            if (_session != null) return;
            _shutdown = false;
            _reference = new ReferenceWaterRuntime(message => LoggerInstance.Warning(message));
            if (_enabled) _reference.Initialize();
            _runtime = new NativeWaterRuntime();
            _runtime.Reference = _reference;
            _session = new WaterSession<MeshRenderer, Material>(_runtime);
            // Handles initialization into an already loaded world, without searching objects.
            var scene = SceneManager.GetSceneByName("Main");
            if (_enabled && scene.IsValid() && scene.isLoaded) _session.Start(_clock.Elapsed.TotalSeconds);
        }

        public void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (_shutdown || !_enabled || _session == null || sceneName != "Main") return;
            _reference?.Initialize();
            _session.Start(_clock.Elapsed.TotalSeconds);
        }

        public void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (_shutdown || _session == null || sceneName != "Main") return;
            // Detach the environment updater before restoring the native material.
            _reference?.Release();
            _session.Stop(_clock.Elapsed.TotalSeconds);
            LogTransition();
        }

        public void OnUpdate()
        {
            if (_shutdown || _session == null) return;
            double now = _clock.Elapsed.TotalSeconds;
            // Inactive sessions can only finish their bounded pending restoration.
            _session.Tick(now);
            if (_enabled && _session.State == WaterState.Applied)
            {
                try { _reference?.Tick(); }
                catch (Exception ex)
                {
                    _reference?.Detach();
                    _session.ReportFailure(ex, now);
                }
            }
            else _reference?.Detach();
            LogTransition();
        }

        // The host owns F7 and calls this before initialization and on every toggle.
        internal void SetEnabled(bool enabled)
        {
            if (_enabled == enabled) return;
            _enabled = enabled;
            if (_session == null || _shutdown) return;
            if (!enabled)
            {
                _reference?.Release();
                _session.Stop(_clock.Elapsed.TotalSeconds);
            }
            else
            {
                var scene = SceneManager.GetSceneByName("Main");
                if (scene.IsValid() && scene.isLoaded)
                {
                    _reference?.Initialize();
                    _session.Start(_clock.Elapsed.TotalSeconds);
                }
            }
            LogTransition();
        }

        public void OnDeinitializeMelon()
        {
            if (_shutdown) return;
            _shutdown = true;
            _reference?.Dispose();
            _session?.Stop(_clock.Elapsed.TotalSeconds);
            LogTransition();
        }

        private void LogTransition()
        {
            if (_session == null || _session.State == _loggedState) return;
            _loggedState = _session.State;
            if (_loggedState == WaterState.Faulted)
                LoggerInstance.Warning($"[water-shader] fault; ownership retained if cleanup is uncertain: {_session.LastError}");
        }

        private sealed class NativeWaterRuntime : IWaterRuntime<MeshRenderer, Material>
        {
            /// <summary>Optional reference-shader path; null keeps the vanilla recipe.</summary>
            internal ReferenceWaterRuntime? Reference { get; set; }

            public MeshRenderer? Discover()
            {
                // Read the generated static instance field; Map owns the evidenced direct child.
                var map = Map.instance;
                if (!Alive(map)) return null;
                var root = map.transform;
                var scene = map.gameObject.scene;
                if (!scene.IsValid() || !scene.isLoaded || scene.name != "Main"
                    || root.name != "Map" || root.parent != null) return null;
                // A single direct-child name lookup also sees an inactive ocean. No recursive
                // resolver, world enumeration, hierarchy arrays or broad name matching.
                var water = root.Find("StylizedWater2_Ocean");
                if (!Alive(water)) return null;
                var renderer = water.GetComponent<MeshRenderer>();
                if (!Alive(renderer)) return null;
                var template = renderer.sharedMaterial;
                return Alive(template) && Alive(template.shader) ? renderer : null;
            }

            // Keep throwing proxy access in the session's fault boundary. Treating an unreadable
            // live target as destroyed could destroy the material still attached to that target.
            private static bool Alive(UnityEngine.Object? value)
                => WaterEnvironment.IsAlive(value);

            public bool TargetAlive(MeshRenderer value) => Alive(value);
            public bool MaterialAlive(Material value) => Alive(value);
            public Material? ReadMaterial(MeshRenderer target) => target.sharedMaterial;
            public Material CreateMaterial(Material template) => new Material(template);
            public void WriteMaterial(MeshRenderer target, Material? material) => target.sharedMaterial = material;
            public bool SameMaterial(Material? left, Material right)
                => !ReferenceEquals(left, null) && left.Pointer == right.Pointer;
            public void DestroyMaterial(Material material) => UnityEngine.Object.Destroy(material);

            public void ConfigureMaterial(Material clone)
            {
                // A rejected reference contract must confirm shader rollback before
                // returning false. Failed rollback escapes into session cleanup.
                if (Reference != null && Reference.TryConfigure(clone))
                {
                    ReferenceWaterLook.Apply(new NativeWaterLookMaterial(clone));
                    return;
                }
                WaterLookRecipe.Apply(new NativeWaterLookMaterial(clone));
            }
        }

        private sealed class Log
        {
            public void Warning(string message) => MelonLogger.Warning(message);
        }

        private sealed class NativeWaterLookMaterial : IWaterLookMaterial
        {
            private readonly Material _material;

            public NativeWaterLookMaterial(Material material) { _material = material; }
            public bool HasProperty(string name) => _material.HasProperty(name);
            public void SetFloat(string name, float value) => _material.SetFloat(name, value);
            public void SetColor(string name, float r, float g, float b, float alpha)
                => _material.SetColor(name, new Color(r, g, b, alpha));
        }
    }
}

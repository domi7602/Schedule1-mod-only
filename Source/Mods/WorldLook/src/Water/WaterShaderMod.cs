using System;
using System.Diagnostics;
using Il2CppScheduleOne.Map;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WaterShader
{
    /// <summary>Applies a natural-coast vanilla-shader recipe on one known ocean renderer.</summary>
    public sealed class WaterShaderMod
    {
        private readonly Log LoggerInstance = new();
        private readonly Stopwatch _clock = Stopwatch.StartNew();
        private WaterSession<MeshRenderer, Material>? _session;
        private NativeWaterRuntime? _runtime;
        private ReferenceWaterRuntime? _reference;
        private WaterState _loggedState = WaterState.Stopped;
        private bool _shutdown;

        public void OnLateInitializeMelon()
        {
            _shutdown = false;
            _reference = new ReferenceWaterRuntime(message => LoggerInstance.Msg(message));
            _reference.Initialize();
            _runtime = new NativeWaterRuntime(message => LoggerInstance.Msg(message));
            _runtime.Reference = _reference;
            _session = new WaterSession<MeshRenderer, Material>(_runtime);
            // Handles initialization into an already loaded world, without searching objects.
            var scene = SceneManager.GetSceneByName("Main");
            if (scene.IsValid() && scene.isLoaded) _session.Start(_clock.Elapsed.TotalSeconds);
            LoggerInstance.Msg("[water-shader] ready — exact Main/Map/StylizedWater2_Ocean only; no world scans");
        }

        public void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (_shutdown || _session == null || sceneName != "Main") return;
            _reference?.Initialize();
            _session.Start(_clock.Elapsed.TotalSeconds);
        }

        public void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (_shutdown || _session == null || sceneName != "Main") return;
            // Release the reflection path first, so no render callback can touch a
            // material while the session restores the native one.
            _reference?.Release();
            _session.Stop();
            LogTransition();
        }

        public void OnUpdate()
        {
            if (_shutdown || _session == null) return;
            _session.Tick(_clock.Elapsed.TotalSeconds);
            LogTransition();
        }

        public void OnDeinitializeMelon()
        {
            if (_shutdown) return;
            _shutdown = true;
            _reference?.Dispose();
            _session?.Stop();
            LogTransition();
        }

        private void LogTransition()
        {
            if (_session == null || _session.State == _loggedState) return;
            _loggedState = _session.State;
            if (_loggedState == WaterState.Faulted)
                LoggerInstance.Warning($"[water-shader] fault; bounded retry, ownership retained if cleanup is uncertain: {_session.LastError}");
            else
                LoggerInstance.Msg($"[water-shader] {_loggedState}: Main/Map/StylizedWater2_Ocean; last discovery={_runtime!.LastDiscoveryMilliseconds:F3} ms");
        }

        private sealed class NativeWaterRuntime : IWaterRuntime<MeshRenderer, Material>
        {
            private readonly Action<string> _log;
            private float _waterY;
            public double LastDiscoveryMilliseconds { get; private set; }

            /// <summary>Optional reference-shader path; null keeps the vanilla recipe.</summary>
            internal ReferenceWaterRuntime? Reference { get; set; }

            public NativeWaterRuntime(Action<string> log) { _log = log; }

            public MeshRenderer? Discover()
            {
                long started = Stopwatch.GetTimestamp();
                try
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
                    _waterY = renderer.transform.position.y;
                    return Alive(template) && Alive(template.shader) ? renderer : null;
                }
                finally
                {
                    LastDiscoveryMilliseconds = (Stopwatch.GetTimestamp() - started) * 1000.0 / Stopwatch.Frequency;
                }
            }

            // Keep throwing proxy access in the session's fault boundary. Treating an unreadable
            // live target as destroyed could destroy the material still attached to that target.
            private static bool Alive(UnityEngine.Object? value)
            {
                if (ReferenceEquals(value, null)) return false;
                if (value.WasCollected) throw new InvalidOperationException("Collected water proxy; ownership cannot be read safely.");
                return value.Pointer != IntPtr.Zero && value != null;
            }

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
                // Prefer the reference shader when it is loaded and the clone
                // exposes the whole contract. Otherwise fall back to the tested
                // vanilla-shader recipe on the otherwise untouched clone.
                if (Reference != null && Reference.TryConfigure(clone, _waterY))
                {
                    _log("[water-shader] reference material configured; vanilla recipe skipped");
                    return;
                }
                _log(WaterLookRecipe.Apply(new NativeWaterLookMaterial(clone)));
            }
        }

        private sealed class Log
        {
            public void Msg(string message) => MelonLogger.Msg(message);
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

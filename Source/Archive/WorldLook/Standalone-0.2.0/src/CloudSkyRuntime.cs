using System;
using System.Collections.Generic;
using Il2CppScheduleOne.Weather;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace WorldLook;

internal sealed class CloudSkySettings
{
    public bool Enabled { get; set; } = true;
    public float Coverage { get; set; } = .55f;
    public float Density { get; set; } = .75f;
    public float Scale { get; set; } = 1f;
    public float Speed { get; set; } = .35f;
    public float Opacity { get; set; } = .88f;
    public float LightingStrength { get; set; } = .35f;
}

/// <summary>
/// A bounded sky replacement: only RenderSettings.skybox and the native
/// EnvironmentManager -> DayNightController -> _skyRenderer reference are eligible.
/// Native slots are replaced only after their full property contract is checked.
/// Native-shader backups survive a lost borrowed original and repeated toggles.
/// No renderer disabling, scene searches, cloud-controller calls or weather writes.
/// </summary>
internal sealed class CloudSkyRuntime : IDisposable
{
    internal const string NativeShaderName = "Custom/ScheduleOneSky";
    private const string NativeShaderGraphName = "Shader Graphs/Schedule1Sky";
    private static readonly string[] NativeShaderNames = { NativeShaderName, NativeShaderGraphName };
    internal const string ShaderAssetPath = "Assets/S1Mods/WorldLookSky.shader";
    private const string ShaderName = "S1Mods/WorldLookSky";
    private const float UniformInterval = .10f;

    private readonly Action<string> _log;
    private readonly List<MaterialGroup> _groups = new();
    private readonly List<SkyBinding> _bindings = new();
    private readonly HashSet<string> _messages = new();
    private readonly MaterialSlotFence _ownershipFence = new();
    private readonly List<MeshRenderer> _knownRenderers = new();
    private readonly BalancedDaylight _lighting = new();
    private Shader? _shader;
    private bool _disposed;
    private float _nextDiscovery, _nextUniform, _nextLoadAttempt, _lastUniform;
    private float _nextReleaseAttempt;
    private int _worldHandle = int.MinValue;
    private string _lastRestoreStatus = "not attempted";
    private Vector2 _cloudOffset = new(19.3f, 47.1f);

    internal bool IsApplied => AppliedMaterialCount > 0;
    internal int AppliedMaterialCount
    {
        get { int count = 0; foreach (var b in _bindings) if (!b.WantsRelease && b.Lease.IsApplied) count++; return count; }
    }
    internal int PendingRestoreCount
    {
        get { int count = 0; foreach (var b in _bindings) if (b.WantsRelease) count++; return count; }
    }

    internal CloudSkyRuntime(Action<string> log) { _log = log; }

    /// <summary>Host calls on LateUpdate while Main is loaded; disabled is a restore.</summary>
    internal void Tick(CloudSkySettings settings)
    {
        if (_disposed) return;
        if (!settings.Enabled) { Reset(); return; }

        float now = Time.realtimeSinceStartup;
        try
        {
            var main = SceneManager.GetSceneByName("Main");
            if (!main.IsValid() || !main.isLoaded) { Reset(); return; }
            if (_worldHandle != main.handle)
            {
                // F7 is not a world-generation change. Only a new Main scene
                // makes previously relinquished target keys eligible again.
                foreach (var binding in _bindings) binding.WantsRelease = true;
                _worldHandle = main.handle;
                _ownershipFence.BeginWorld(_worldHandle);
                _nextReleaseAttempt = _nextDiscovery = 0f;
            }
            DrainReleases(now);
            if (!EnsureShader(now)) return;
            var manager = WorldLookEnvironment.TryGetManager();
            var controller = WorldLookEnvironment.TryGetController(manager);

            if (now >= _nextDiscovery)
            {
                _nextDiscovery = now + 1f;
                PruneLostBindings();
                DiscoverTargets(controller);
            }
            if (!IsApplied)
            {
                _lighting.Reset();
                WorldLookEnvironment.Reset();
                LogOnce("no-target", "[sky] waiting: no eligible native sky target is owned; sky and daylight bias are inactive.");
                return;
            }

            // Observe before writing: native drift becomes the new baseline,
            // while an unchanged prior WorldLook write never compounds itself.
            var native = _lighting.Observe(controller);
            if (now >= _nextUniform)
            {
                PruneLostBindings();
                if (!IsApplied)
                {
                    _lighting.Reset(); WorldLookEnvironment.Reset(); return;
                }
                float elapsed = _lastUniform > 0f ? Mathf.Clamp(now - _lastUniform, 0f, 1f) : UniformInterval;
                _lastUniform = now;
                _nextUniform = now + UniformInterval;
                foreach (var group in _groups) group.Synchronize();
                var state = WorldLookEnvironment.Read(settings, manager, controller, native, elapsed);
                _cloudOffset += state.WindDirection * (elapsed * .025f *
                    Mathf.Clamp(WorldLookEnvironment.Finite(settings.Speed, .35f), 0f, 3f) * (1f + state.Windy * .6f));
                foreach (var binding in _bindings)
                    if (!binding.WantsRelease && binding.Lease.IsApplied) WriteSky(binding, settings, state);
                LogOnce(state.HasWeather ? "weather-ready" : "weather-fallback",
                    state.HasWeather ? "[sky] native weather weights are available through read-only S1API.Weather.WeatherManager.Current."
                    : "[sky] native weather snapshot is not ready; moderate .30 cloudiness input is used until it becomes available.");
            }

            _lighting.Apply(settings, WorldLookEnvironment.Current);
        }
        catch (Exception ex)
        {
            Reset();
            _shader = null;
            _nextLoadAttempt = now + 10f;
            LogOnce("failure-" + ex.GetType().Name,
                $"[sky] released after {ex.GetType().Name}: {ex.Message}; native targets are restored where still owned.");
        }
    }

    private bool EnsureShader(float now)
    {
        if (WorldLookEnvironment.IsAlive(_shader)) return _shader!.isSupported;
        if (now < _nextLoadAttempt) return false;
        _nextLoadAttempt = now + 10f;
        var shader = ShaderAssets.LoadShader(ShaderAssetPath, _log);
        if (!WorldLookEnvironment.IsAlive(shader) || !shader!.isSupported || shader.name != ShaderName)
        {
            LogOnce("shader-unavailable", "[sky] WorldLookSky shader unavailable, unsupported or unexpected; native sky stays active.");
            return false;
        }
        _shader = shader;
        return true;
    }

    private void DiscoverTargets(DayNightController? controller)
    {
        Material? sky = RenderSettings.skybox;
        TryBind("RenderSettings.skybox", sky, null, -1);

        if (!WorldLookEnvironment.IsAlive(controller))
        {
            LogOnce("controller-unavailable", "[sky] native DayNightController is not ready; only the checked RenderSettings skybox can be bound.");
            return;
        }

        MeshRenderer? renderer = controller!._skyRenderer;
        if (!WorldLookEnvironment.IsAlive(renderer))
        {
            LogOnce("renderer-unavailable", "[sky] DayNightController._skyRenderer is null or unavailable; no other renderer is searched or disabled.");
            return;
        }

        KnownMaterialConsumers.Register(_knownRenderers, renderer!, SameStrict, IsAliveStrict);
        // Register the explicit consumer before reading its slot array. If that
        // read fails, cleanup must retain resources rather than forget the reader.
        var materials = renderer!.sharedMaterials;
        if (materials == null) return;
        // This is one explicit native reference, not a traversal of world renderers.
        int count = Math.Min(materials.Length, 8);
        for (int slot = 0; slot < count; slot++)
        {
            string key = "DayNightController._skyRenderer#" + renderer.GetInstanceID() + "[" + slot + "]";
            TryBind(key, materials[slot], renderer, slot);
        }
        if (materials.Length > count)
            LogOnce("slot-cap", "[sky] native sky renderer exceeds the eight-slot safety limit; remaining slots were not inspected.");
        // An incompatible Shader Graph dome remains untouched. Its mere presence
        // was not evidence that it contributed to the faulty camera image.
    }

    private void TryBind(string key, Material? source, MeshRenderer? renderer, int slot)
    {
        if (_ownershipFence.IsBlocked(_worldHandle, key) || !IsAliveStrict(source)) return;
        foreach (var current in _bindings) if (current.Key == key && current.WorldHandle == _worldHandle) return;
        Shader? originalShader = source!.shader;
        bool nativeShader = IsNativeSkyShader(originalShader);
        if (!nativeShader)
        {
            LogOnce("skip-" + key + "-" + (originalShader?.name ?? "null"),
                $"[sky] skipped {key}: material='{source.name}', shader='{originalShader?.name ?? "none"}'; requires one of: {string.Join(", ", NativeShaderNames)}.");
            return;
        }
        string? missing = null;
        foreach (var property in NativeProperty.Definitions)
            if (!source.HasProperty(property.Name)) { missing = property.Name; break; }
        if (missing != null)
        {
            LogOnce("contract-" + key, $"[sky] skipped {key}: native property {missing} is missing.");
            return;
        }
        LogOnce("contract-ok-" + key,
            $"[sky] contract {key}: {NativeProperty.Definitions.Length}/{NativeProperty.Definitions.Length} native properties present; binding {originalShader!.name}.");

        MaterialGroup? group = null;
        foreach (var candidate in _groups)
            if (candidate.MatchesSource(source)) { group = candidate; break; }

        Material? clone = null;
        SkyBinding? binding = null;
        try
        {
            if (group == null)
            {
                group = new MaterialGroup(source, originalShader!);
                _groups.Add(group);
            }
            group.PrepareBackup();
            // If the native game updated an attached backup while WorldLook was
            // off, adopt those values before writing a new owned clone.
            group.AdoptNativeSource(source);
            clone = new Material(source);
            // Register the allocation in the local variable before any setter.
            clone.name = source.name + " (WorldLook owned sky)";
            clone.hideFlags = HideFlags.HideAndDontSave;
            clone.shader = _shader;
            clone.renderQueue = source.renderQueue;
            foreach (var property in NativeProperty.Definitions)
                if (!clone.HasProperty(property.Name)) throw new InvalidOperationException("WorldLookSky lacks native property " + property.Name);
            if (!clone.HasProperty("_WLCloudCoverage") || !clone.HasProperty("_WLMeshMode"))
                throw new InvalidOperationException("WorldLookSky custom property contract is incomplete");
            clone.SetFloat("_WLMeshMode", slot < 0 ? 0f : 1f);
            binding = new SkyBinding(this, key, group, source, clone, _shader!, renderer, slot, _worldHandle);
            group.Bindings.RemoveAll(b => b.Key == key && !_bindings.Contains(b));
            // Publication may mutate and then throw. Track the lease first.
            _bindings.Add(binding);
            if (!binding.Lease.Publish(() => group.Add(binding)))
                throw new InvalidOperationException("Sky material publication was not confirmed: " + binding.Lease.LastError);
            _nextUniform = 0f;
            _messages.Remove("no-target");
            _log($"[sky] applied {key}: original='{source.name}' #{source.GetInstanceID()}, backup=#{group.Backup!.GetInstanceID()}, owned=#{clone.GetInstanceID()}, shader={ShaderName}; native Sun/Moon/Star/Fog inputs bridged. Native renderers remain untouched.");
        }
        catch
        {
            if (binding != null)
            {
                binding.WantsRelease = true;
                _nextReleaseAttempt = 0f;
                DrainReleases(Time.realtimeSinceStartup);
            }
            else DestroyOwned(clone); // Never published: no native slot could hold this allocation.
            throw;
        }
    }

    private void PruneLostBindings()
    {
        for (int i = _bindings.Count - 1; i >= 0; i--)
        {
            var binding = _bindings[i];
            if (binding.WantsRelease || binding.Lease.ValidateOwnership()) continue;
            binding.WantsRelease = true;
            if (binding.Lease.Relinquished) _ownershipFence.ObserveForeign(binding.WorldHandle, binding.Key);
            LogOnce("lost-" + binding.Key,
                $"[sky] writes suspended at {binding.Key}: {binding.Lease.State}; unreadable ownership is retained, foreign ownership stays fenced until a new Main scene.");
        }
        DrainReleases(Time.realtimeSinceStartup);
    }

    private void WriteSky(SkyBinding binding, CloudSkySettings settings, WorldLookEnvironmentState state)
    {
        Material material = binding.Owned;
        material.SetColor("_WLSkyTop", state.SkyTopColor);
        material.SetColor("_WLSkyHorizon", state.SkyHorizonColor);
        material.SetColor("_WLSkyGround", state.SkyGroundColor);
        material.SetColor("_WLCloudLight", state.CloudLightColor);
        material.SetColor("_WLCloudShadow", state.CloudShadowColor);
        material.SetVector("_WLSunDirection", new Vector4(state.SunDirection.x, state.SunDirection.y, state.SunDirection.z, 0f));
        material.SetVector("_WLMoonDirection", new Vector4(state.MoonDirection.x, state.MoonDirection.y, state.MoonDirection.z, 0f));
        material.SetVector("_WLCloudOffset", new Vector4(_cloudOffset.x, _cloudOffset.y, 0f, 0f));
        material.SetFloat("_WLDaylight", state.Daylight);
        material.SetFloat("_WLDayFraction", state.DayFraction);
        material.SetFloat("_WLSunVisible", state.HasSun ? 1f : 0f);
        material.SetFloat("_WLMoonVisible", state.HasMoon ? 1f : 0f);
        material.SetFloat("_WLCloudCoverage", state.CloudCoverage);
        material.SetFloat("_WLCloudDensity", state.CloudDensity);
        material.SetFloat("_WLCloudScale", Mathf.Clamp(WorldLookEnvironment.Finite(settings.Scale, 1f), .25f, 4f));
        material.SetFloat("_WLCloudOpacity", Mathf.Clamp01(WorldLookEnvironment.Finite(settings.Opacity, .88f)));
        if (!binding.FirstInputsLogged)
        {
            binding.FirstInputsLogged = true;
            _log($"[sky] first-inputs {binding.Key}: skyTop={state.SkyTopColor}, horizon={state.SkyHorizonColor}, cloudLight={state.CloudLightColor}, cloudShadow={state.CloudShadowColor}, dayFraction={state.DayFraction:F4}; CPU material inputs only, not camera-draw verification.");
        }
    }

    internal string DescribeState()
    {
        var state = WorldLookEnvironment.Current;
        string paths = _bindings.Count == 0 ? "none" : string.Join(", ", _bindings.ConvertAll(b => b.Key));
        string actual;
        try { actual = DescribeMaterial(RenderSettings.skybox); }
        catch (Exception ex) { actual = "unreadable: " + ex.GetType().Name; }
        return $"[sky] applied={IsApplied}, materials={AppliedMaterialCount}, targets=[{paths}], " +
            $"weather={(state.HasWeather ? "native S1API" : "fallback .30")}, " +
            $"daylight={state.Daylight:F2}, sunY={state.SunElevation:F3}, coverage={state.CloudCoverage:F2}, " +
            $"density={state.CloudDensity:F2}, skybox={actual}, pendingRestores={PendingRestoreCount}, " +
            $"lastRestore={_lastRestoreStatus}, nativeDome=untouched; material ownership is not camera-draw verification.";
    }

    internal void Reset()
    {
        _lighting.Reset();
        foreach (var binding in _bindings) binding.WantsRelease = true;
        DrainReleases(Time.realtimeSinceStartup);
        _nextDiscovery = _nextUniform = _lastUniform = 0f;
        _cloudOffset = new Vector2(19.3f, 47.1f);
        WorldLookEnvironment.Reset();
    }

    private void DrainReleases(float now)
    {
        if (now < _nextReleaseAttempt) return;
        _nextReleaseAttempt = now + 1f;
        for (int i = _bindings.Count - 1; i >= 0; i--)
        {
            var binding = _bindings[i];
            if (!binding.WantsRelease) continue;
            try
            {
                // Keep native bridge drift in the backup before detaching the clone.
                binding.Group.PrepareRestore();
            }
            catch (Exception ex) { LogOnce("sync-release-" + binding.Key, "[sky] release bridge snapshot unavailable: " + ex.Message); }
            var result = binding.Lease.Restore();
            _lastRestoreStatus = binding.Key + ": " + result;
            if (binding.Lease.Relinquished) _ownershipFence.ObserveForeign(binding.WorldHandle, binding.Key);
            if (result == MaterialLeaseState.Deferred || !binding.Lease.ReleaseOwned())
            {
                LogOnce("defer-" + binding.Key, $"[sky] release deferred at {binding.Key}; resources retained: {binding.Lease.LastError}");
                continue;
            }
            _log($"[sky] release {binding.Key}: {result}; current={DescribeMaterialSafe(binding)}; owned material detached or handed to a later owner, never replaced with null.");
            _bindings.RemoveAt(i);
        }
        CollectUnusedBackups();
    }

    private void CollectUnusedBackups()
    {
        for (int i = _groups.Count - 1; i >= 0; i--)
        {
            var group = _groups[i];
            bool active = false;
            foreach (var binding in _bindings) if (binding.Group == group) { active = true; break; }
            if (active) continue;
            try
            {
                Material? backup = group.Backup;
                if (IsAliveStrict(backup))
                {
                    if (KnownSlotUses(backup!)) continue;
                    // A foreign shader also transfers ownership of a backup.
                    if (group.IsNativeShader(backup!)) UnityEngine.Object.Destroy(backup);
                }
                _groups.RemoveAt(i);
            }
            catch (Exception ex) { LogOnce("backup-retained-" + i, "[sky] native backup retained while a consumer is unreadable: " + ex.Message); }
        }
    }

    private bool KnownSlotUses(Material material)
    {
        if (SameStrict(RenderSettings.skybox, material)) return true;
        // Check ALL slots of each explicit native renderer, including slots that
        // were originally incompatible/unbound. No scene-wide enumeration.
        foreach (var renderer in _knownRenderers)
        {
            if (!IsAliveStrict(renderer)) continue;
            var materials = renderer.sharedMaterials
                ?? throw new InvalidOperationException("Known renderer material array is unreadable.");
            if (KnownMaterialConsumers.Contains(material, materials.Length, i => materials[i], SameStrict)) return true;
        }
        return false;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _nextReleaseAttempt = 0f;
        Reset();
        if (PendingRestoreCount != 0)
            _log($"[sky] shutdown retains {PendingRestoreCount} unresolved material leases; no possibly attached material was destroyed.");
        _shader = null;
    }
    private void LogOnce(string key, string message) { if (_messages.Add(key)) _log(message); }

    private static bool IsNativeSkyShader(Shader? shader)
    {
        if (!WorldLookEnvironment.IsAlive(shader)) return false;
        foreach (string accepted in NativeShaderNames)
            if (shader!.name == accepted) return true;
        return false;
    }
    private static void DestroyOwned(Material? value)
    {
        try { if (WorldLookEnvironment.IsAlive(value)) UnityEngine.Object.Destroy(value); }
        catch { }
    }

    private static bool IsAliveStrict(UnityEngine.Object? value)
    {
        if (ReferenceEquals(value, null)) return false;
        if (value.WasCollected) throw new InvalidOperationException("Collected Unity proxy; material/slot ownership is unreadable.");
        return value.Pointer != IntPtr.Zero && value != null;
    }

    private static bool SameStrict(UnityEngine.Object? left, UnityEngine.Object? right)
        => IsAliveStrict(left) && IsAliveStrict(right) && left!.GetInstanceID() == right!.GetInstanceID();

    private static string DescribeMaterial(Material? material)
        => !IsAliveStrict(material) ? "none" : $"'{material!.name}' #{material.GetInstanceID()} shader='{material.shader?.name ?? "none"}'";

    private static string DescribeMaterialSafe(SkyBinding binding)
    {
        try { return DescribeMaterial(binding.ReadSlot()); }
        catch (Exception ex) { return "unreadable: " + ex.GetType().Name; }
    }

    private sealed class SkyBinding : IMaterialSlotRuntime<Material>
    {
        internal readonly string Key;
        internal readonly MaterialGroup Group;
        internal readonly Material Owned;
        internal readonly MaterialSlotLease<Material> Lease;
        internal bool WantsRelease, FirstInputsLogged;
        private readonly CloudSkyRuntime _owner;
        private readonly Shader _ownedShader;
        private readonly MeshRenderer? _renderer;
        private readonly int _slot, _world;
        internal int WorldHandle => _world;
        internal SkyBinding(CloudSkyRuntime owner, string key, MaterialGroup group, Material source,
            Material owned, Shader ownedShader, MeshRenderer? renderer, int slot, int world)
        {
            _owner = owner; Key = key; Group = group; Owned = owned; _ownedShader = ownedShader;
            _renderer = renderer; _slot = slot; _world = world;
            Lease = new MaterialSlotLease<Material>(this, source, group.Original,
                group.Backup ?? throw new InvalidOperationException("A native backup is required before binding."), owned);
        }

        internal bool OwnsSlot()
            => !WantsRelease && HoldsOwnedMaterial();

        internal bool HoldsOwnedMaterial() => !Lease.Relinquished && SameStrict(ReadSlot(), Owned) && IsOwnedShader(Owned);

        public Material? ReadSlot()
        {
            if (_slot < 0) return RenderSettings.skybox;
            if (!IsAliveStrict(_renderer)) return null; // Confirmed destroyed target, not a caught read error.
            var materials = _renderer!.sharedMaterials;
            return materials != null && _slot < materials.Length ? materials[_slot] : null;
        }

        public bool CanRestore
        {
            get { var main = SceneManager.GetSceneByName("Main"); return main.IsValid() && main.isLoaded && main.handle == _world; }
        }

        public void WriteSlot(Material material)
        {
            if (!CanRestore || !IsAliveStrict(material)) throw new InvalidOperationException("Invalid world or material for sky slot write.");
            if (_slot < 0) { RenderSettings.skybox = material; return; }
            if (!IsAliveStrict(_renderer)) throw new InvalidOperationException("Native sky renderer is no longer alive.");
            var materials = _renderer!.sharedMaterials;
            if (materials == null || _slot >= materials.Length) throw new InvalidOperationException("Native sky material slot is no longer present.");
            materials[_slot] = material;
            _renderer.sharedMaterials = materials;
        }

        public bool IsAlive(Material material) => IsAliveStrict(material);
        public bool IsOwnedShader(Material material) => IsAliveStrict(material) && SameStrict(material.shader, _ownedShader);
        public bool IsNativeShader(Material material) => Group.IsNativeShader(material);
        public bool Same(Material? left, Material right) => SameStrict(left, right);
        public void DestroyOwned(Material material)
        {
            if (_owner.KnownSlotUses(material)) throw new InvalidOperationException("Another known native slot still references this owned material.");
            UnityEngine.Object.Destroy(material);
        }
    }

    /// <summary>
    /// One original can feed the skybox and mesh clones. Native changes on either
    /// original OR active clone are propagated to both. The runtime never scales
    /// the native HDR _SunShine/_MoonShine/_StarShine values in this bridge.
    /// </summary>
    private sealed class MaterialGroup
    {
        internal readonly Material Original;
        internal Material? Backup { get; private set; }
        internal readonly List<SkyBinding> Bindings = new();
        private readonly Shader _originalShader;
        private readonly NativeValue[] _lastSource, _lastApplied;
        private Material _nativeSource;
        private bool _backupPrepared;

        internal MaterialGroup(Material source, Shader shader)
        {
            Original = _nativeSource = source; _originalShader = shader;
            _lastSource = new NativeValue[NativeProperty.Definitions.Length];
            _lastApplied = new NativeValue[_lastSource.Length];
        }

        internal bool MatchesSource(Material material)
            => SameStrict(Backup, material) || SameStrict(Original, material);

        internal bool IsNativeShader(Material material)
            => IsAliveStrict(material) && SameStrict(material.shader, _originalShader);

        internal void PrepareBackup()
        {
            if (_backupPrepared) return;
            // This group is registered before this allocation. Failed setters
            // therefore cannot orphan an untracked native material.
            if (!IsAliveStrict(Backup)) Backup = new Material(Original);
            Material backup = Backup ?? throw new InvalidOperationException("Native backup allocation returned no material.");
            backup.name = Original.name + " (WorldLook native restore backup)";
            backup.hideFlags = HideFlags.HideAndDontSave;
            if (!IsNativeShader(backup)) throw new InvalidOperationException("Native sky backup shader was not retained.");
            _backupPrepared = true;
        }

        internal void AdoptNativeSource(Material source)
        {
            if (!IsNativeShader(source) || !IsAliveStrict(Backup))
                throw new InvalidOperationException("Cannot adopt an unreadable native sky source or backup.");
            _nativeSource = source;
            for (int i = 0; i < _lastSource.Length; i++)
                _lastSource[i] = _lastApplied[i] = NativeProperty.Definitions[i].Read(source);
            foreach (var property in NativeProperty.Definitions)
                property.Write(Backup!, property.Read(source));
        }

        internal void Add(SkyBinding binding)
        {
            Bindings.Add(binding);
            for (int i = 0; i < _lastApplied.Length; i++) NativeProperty.Definitions[i].Write(binding.Owned, _lastApplied[i]);
        }

        internal void Synchronize()
        {
            bool readableSource;
            try { readableSource = IsNativeShader(_nativeSource); }
            catch { readableSource = false; } // Optional native input, never slot-ownership evidence.
            // Resolve ownership once per synchronization, not once per property.
            // Native sharedMaterials reads allocate a returned array on some builds.
            var active = new List<SkyBinding>(Bindings.Count);
            foreach (var binding in Bindings)
                if (binding.CanRestore && binding.HoldsOwnedMaterial()) active.Add(binding);
            if (active.Count == 0) return;
            bool backupWritable = Backup != null && IsNativeShader(Backup);
            bool writingBackToSource = readableSource && backupWritable && SameStrict(_nativeSource, Backup);
            for (int i = 0; i < _lastApplied.Length; i++)
            {
                var property = NativeProperty.Definitions[i];
                NativeValue source = readableSource ? property.Read(_nativeSource) : _lastSource[i];
                var decision = NativeMaterialBridgePolicy.Decide(_lastSource[i], _lastApplied[i], source,
                    active.Count, index => property.Read(active[index].Owned), property.Equal, writingBackToSource);
                foreach (var binding in active) property.Write(binding.Owned, decision.value);
                if (backupWritable) property.Write(Backup!, decision.value);
                _lastSource[i] = decision.sourceSnapshot;
                _lastApplied[i] = decision.value;
            }
        }

        internal void PrepareRestore()
        {
            Synchronize();
            bool stillOwned = false;
            foreach (var binding in Bindings)
                if (binding.CanRestore && binding.HoldsOwnedMaterial()) { stillOwned = true; break; }
            if (!stillOwned || !IsNativeShader(Original)) return;
            // Preserve native bridge updates made on the owned material without
            // copying any WorldLook palette/light bias into the borrowed original.
            for (int i = 0; i < _lastApplied.Length; i++)
                NativeProperty.Definitions[i].Write(Original, _lastApplied[i]);
        }
    }

    private enum NativeKind { Float, Color, Texture }
    private struct NativeValue
    {
        internal float Float;
        internal Color Color;
        internal Texture? Texture;
        internal Vector2 Scale, Offset;
    }
    private sealed class NativeProperty
    {
        internal static readonly NativeProperty[] Definitions =
        {
            new("_SunSizeModifier", NativeKind.Float), new("_SunTexture", NativeKind.Texture),
            new("_SunShine", NativeKind.Color), new("_MoonSizeModifier", NativeKind.Float),
            new("_MoonTexture", NativeKind.Texture), new("_MoonShine", NativeKind.Color),
            new("_StarShine", NativeKind.Color), new("_StarFadeOutStart", NativeKind.Float),
            new("_StarFadeOutEnd", NativeKind.Float), new("_StarFadeInStart", NativeKind.Float),
            new("_StarFadeInEnd", NativeKind.Float), new("_FogInfluence", NativeKind.Float),
        };
        internal readonly string Name;
        private readonly NativeKind _kind;
        private NativeProperty(string name, NativeKind kind) { Name = name; _kind = kind; }
        internal NativeValue Read(Material material)
        {
            if (_kind == NativeKind.Float) return new NativeValue { Float = material.GetFloat(Name) };
            if (_kind == NativeKind.Color) return new NativeValue { Color = material.GetColor(Name) };
            return new NativeValue { Texture = material.GetTexture(Name), Scale = material.GetTextureScale(Name), Offset = material.GetTextureOffset(Name) };
        }
        internal void Write(Material material, NativeValue value)
        {
            if (_kind == NativeKind.Float) material.SetFloat(Name, value.Float);
            else if (_kind == NativeKind.Color) material.SetColor(Name, value.Color);
            else
            {
                material.SetTexture(Name, value.Texture);
                material.SetTextureScale(Name, value.Scale); material.SetTextureOffset(Name, value.Offset);
            }
        }
        internal bool Equal(NativeValue a, NativeValue b)
        {
            if (_kind == NativeKind.Float) return Close(a.Float, b.Float);
            if (_kind == NativeKind.Color) return Close(a.Color, b.Color);
            bool sameTexture = (!WorldLookEnvironment.IsAlive(a.Texture) && !WorldLookEnvironment.IsAlive(b.Texture))
                || WorldLookEnvironment.SameObject(a.Texture, b.Texture);
            return sameTexture && Close(a.Scale.x, b.Scale.x) && Close(a.Scale.y, b.Scale.y)
                && Close(a.Offset.x, b.Offset.x) && Close(a.Offset.y, b.Offset.y);
        }
    }

    private static bool Close(float a, float b) => Math.Abs(a - b) <= .00001f * (1f + Math.Max(Math.Abs(a), Math.Abs(b)));
    private static bool Close(Color a, Color b) => Close(a.r, b.r) && Close(a.g, b.g) && Close(a.b, b.b) && Close(a.a, b.a);

    /// <summary>A small daytime bias; native fog, volumes and weather stay in charge.</summary>
    private sealed class BalancedDaylight
    {
        private Light? _sun;
        private int _scene = int.MinValue;
        private readonly ColorChannel _sunColor = new(), _sky = new(), _equator = new(), _ground = new();
        private readonly FloatChannel _intensity = new(), _shadow = new();

        internal WorldLookNativeLighting Observe(DayNightController? controller)
        {
            Light? sun = null;
            try { if (WorldLookEnvironment.IsAlive(controller)) sun = controller!._sunLight; } catch { }
            if (!WorldLookEnvironment.IsAlive(sun)) sun = RenderSettings.sun;
            if (!WorldLookEnvironment.SameObject(sun, _sun))
            {
                RestoreSun(); _sun = sun;
                _sunColor.Clear(); _intensity.Clear(); _shadow.Clear();
            }
            if (WorldLookEnvironment.IsAlive(_sun))
            {
                _sunColor.Observe(_sun!.color); _intensity.Observe(_sun.intensity); _shadow.Observe(_sun.shadowStrength);
            }

            int scene = SceneManager.GetActiveScene().handle;
            if (scene != _scene) { _sky.Clear(); _equator.Clear(); _ground.Clear(); _scene = scene; }
            _sky.Observe(RenderSettings.ambientSkyColor);
            _equator.Observe(RenderSettings.ambientEquatorColor);
            _ground.Observe(RenderSettings.ambientGroundColor);
            return new WorldLookNativeLighting(_sun, _sunColor.Baseline, _intensity.Baseline, _shadow.Baseline,
                _sky.Baseline, _equator.Baseline, _ground.Baseline);
        }

        internal void Apply(CloudSkySettings settings, WorldLookEnvironmentState state)
        {
            float strength = Mathf.Clamp01(WorldLookEnvironment.Finite(settings.LightingStrength, .35f));
            if (!state.IsValid || strength <= .0001f) { RestoreSun(); RestoreAmbient(); return; }
            float amount = strength * state.Daylight;
            float overcast = WorldLookEnvironment.Smooth(.45f, .95f, state.CloudCoverage);
            if (WorldLookEnvironment.IsAlive(_sun))
            {
                _sunColor.Set(Color.Lerp(_sunColor.Baseline, Neutralize(_sunColor.Baseline, .20f), amount), v => _sun!.color = v);
                _intensity.Set(_intensity.Baseline * (1f - .35f * amount * overcast), v => _sun!.intensity = v);
                _shadow.Set(_shadow.Baseline * (1f - .55f * amount * overcast), v => _sun!.shadowStrength = v);
            }
            if (RenderSettings.ambientMode == AmbientMode.Trilight)
            {
                _sky.Set(AmbientTarget(_sky.Baseline, amount, overcast, .07f), v => RenderSettings.ambientSkyColor = v);
                _equator.Set(AmbientTarget(_equator.Baseline, amount, overcast, .12f), v => RenderSettings.ambientEquatorColor = v);
                _ground.Set(AmbientTarget(_ground.Baseline, amount, overcast, .05f), v => RenderSettings.ambientGroundColor = v);
            }
            else RestoreAmbient();
        }

        private static Color AmbientTarget(Color baseline, float amount, float overcast, float fill)
        {
            Color target = Neutralize(baseline, .16f);
            float multiplier = 1f + fill * overcast;
            target = new Color(target.r * multiplier, target.g * multiplier, target.b * multiplier, baseline.a);
            return Color.Lerp(baseline, target, amount);
        }

        private static Color Neutralize(Color value, float amount)
        {
            float luminance = value.r * .2126f + value.g * .7152f + value.b * .0722f;
            return Color.Lerp(value, new Color(luminance, luminance, luminance, value.a), amount);
        }

        private void RestoreSun()
        {
            try
            {
                if (!WorldLookEnvironment.IsAlive(_sun)) return;
                _sunColor.Restore(_sun!.color, v => _sun.color = v);
                _intensity.Restore(_sun.intensity, v => _sun.intensity = v);
                _shadow.Restore(_sun.shadowStrength, v => _sun.shadowStrength = v);
            }
            catch { }
        }
        private void RestoreAmbient()
        {
            try
            {
                if (SceneManager.GetActiveScene().handle != _scene) return;
                _sky.Restore(RenderSettings.ambientSkyColor, v => RenderSettings.ambientSkyColor = v);
                _equator.Restore(RenderSettings.ambientEquatorColor, v => RenderSettings.ambientEquatorColor = v);
                _ground.Restore(RenderSettings.ambientGroundColor, v => RenderSettings.ambientGroundColor = v);
            }
            catch { }
        }
        internal void Reset()
        {
            RestoreSun(); RestoreAmbient(); _sun = null; _scene = int.MinValue;
            _sunColor.Clear(); _intensity.Clear(); _shadow.Clear(); _sky.Clear(); _equator.Clear(); _ground.Clear();
        }
    }

    private sealed class ColorChannel
    {
        internal Color Baseline;
        private Color _last;
        private bool _owned;
        internal void Observe(Color current) { if (!_owned || !Close(current, _last)) Baseline = current; }
        internal void Set(Color value, Action<Color> write) { write(value); _last = value; _owned = true; }
        internal void Restore(Color current, Action<Color> write) { if (_owned && Close(current, _last)) write(Baseline); _owned = false; }
        internal void Clear() { _owned = false; Baseline = default; }
    }
    private sealed class FloatChannel
    {
        internal float Baseline;
        private float _last;
        private bool _owned;
        internal void Observe(float current) { if (!_owned || !Close(current, _last)) Baseline = current; }
        internal void Set(float value, Action<float> write) { write(value); _last = value; _owned = true; }
        internal void Restore(float current, Action<float> write) { if (_owned && Close(current, _last)) write(Baseline); _owned = false; }
        internal void Clear() { _owned = false; Baseline = default; }
    }
}

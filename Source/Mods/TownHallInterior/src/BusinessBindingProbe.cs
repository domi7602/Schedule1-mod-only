#if TOWNHALL_BUSINESS_PROBE
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppScheduleOne.Persistence.Loaders;
using Il2CppScheduleOne.Property;
using MelonLoader;
using S1API.Lifecycle;
using UnityEngine;
using NativeProperty = Il2CppScheduleOne.Property.Property;

namespace TownHallInterior;

// Diagnostic candidate only. No native setters, creation, ownership, save or laundering calls.
// Harmony ownership proves installation in managed metadata, not invocation by IL2CPP.
internal static class BusinessBindingProbe
{
    private const string Owner = "TownHallInterior.BusinessBindingProbe";
    private static readonly ProbeRecording Recording = new();
    private static ProbeTrace Trace => Recording.Snapshots;
    private static ProbeLoaderTrace LoaderTrace => Recording.Loaders;
    private static readonly ProbeLoadGeneration Lifecycle = new();
    private static readonly List<MethodInfo> Targets = new();
    private static HarmonyLib.Harmony? _harmony;
    private static string _report = "", _reportBase = "", _previousReport = "";
    private static string _assemblyHash = "", _nativeHash = "", _apiHash = "";
    private static int _thread, _offThread;
    private static long _generation => Recording.Generation;
    private static long _call, _sequence;
    private static bool _enabled;
    private static float _settleAt = -1;

    // Public/lifecycle callbacks must never interrupt hall creation or its mandatory cleanup.
    internal static void Start() => Safe(StartCore);
    internal static void Tick() => Safe(TickCore);
    internal static void SceneLoaded(string scene) => Safe(() => SceneLoadedCore(scene));
    internal static void SceneUnloaded(string scene) => Safe(() => SceneUnloadedCore(scene));
    internal static void Stop() => Safe(StopCore);
    private static void PreLoad() => Safe(PreLoadCore);
    private static void Loaded() => Safe(LoadedCore);
    private static void Leaving() => Safe(LeavingCore);

    private static void Safe(Action action)
    {
        try { action(); }
        catch (Exception ex)
        {
            _enabled = false; _settleAt = -1;
            try
            {
                Trace.Append(new ProbeSnapshot("observer-stopped", -1, _generation, false,
                    new[] { ex.GetType().Name }, Array.Empty<ProbeEntity>(), ++_sequence));
                Flush();
            }
            catch { /* A diagnostic failure must not escape into mandatory hall cleanup. */ }
        }
    }

    private static void StartCore()
    {
        _thread = Environment.CurrentManagedThreadId;
        try
        {
            string root = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory,
                "TownHallInterior", "BusinessProbe");
            Directory.CreateDirectory(root);
            _reportBase = Path.Combine(root, DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffZ") + "-" + System.Guid.NewGuid().ToString("N"));
            _report = ReportPath(0);
            _assemblyHash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(typeof(Mod).Assembly.Location))).ToLowerInvariant();
            _nativeHash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(typeof(Business).Assembly.Location))).ToLowerInvariant();
            _apiHash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(typeof(GameLifecycle).Assembly.Location))).ToLowerInvariant();
            _harmony = new HarmonyLib.Harmony(Owner);
            Hook(typeof(LoadManager), "QueueLoadRequest", typeof(LoadRequest));
            Hook(typeof(PropertiesLoader), "Load", typeof(string));
            Hook(typeof(BusinessesLoader), "Load", typeof(string));
            Hook(typeof(BusinessLoader), "Load", typeof(string));
            Hook(typeof(PropertyLoader), "Load", typeof(PropertyData), typeof(string));
            Hook(typeof(Business), "Load", typeof(PropertyData), typeof(string));
            _enabled = true;
            GameLifecycle.OnPreLoad += PreLoad;
            GameLifecycle.OnLoadComplete += Loaded;
            GameLifecycle.OnPreSceneChange += Leaving;
            Mark("probe-start");
        }
        catch (Exception ex)
        {
            Mark("probe-start-failed", ex.GetType().Name);
            RemoveHooks();
        }
        Flush();
    }

    private static void Hook(Type type, string name, params Type[] arguments)
    {
        var method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly,
            null, arguments, null);
        if (method == null || method.ReturnType != typeof(void)) throw new MissingMethodException(type.FullName, name);
        Targets.Add(method);
        _harmony!.Patch(method,
            prefix: new HarmonyMethod(typeof(BusinessBindingProbe), nameof(Entering)),
            postfix: new HarmonyMethod(typeof(BusinessBindingProbe), nameof(Exiting)));
        if (HarmonyLib.Harmony.GetPatchInfo(method)?.Owners.Contains(Owner) != true)
            throw new InvalidOperationException("observer-owner-missing");
    }

    // Void observers cannot suppress the original, and neither arguments nor result are exposed.
    private static void Entering(MethodBase __originalMethod, out long __state)
    {
        __state = 0;
        if (!_enabled || !OnThread()) return;
        try { __state = ++_call; Event("enter", __originalMethod, __state); }
        catch { Interlocked.Exchange(ref _offThread, 1); }
    }

    private static void Exiting(MethodBase __originalMethod, long __state)
    {
        if (!_enabled || !OnThread()) return;
        try { Event("exit", __originalMethod, __state); }
        catch { Interlocked.Exchange(ref _offThread, 1); }
    }

    private static void Event(string phase, MethodBase method, long token)
    {
        // Loader traffic has its own per-load budget and cannot displace inventories.
        // A late exit after rotation has no matching enter in this recording and stays incomplete.
        if (!LoaderTrace.Record(new ProbeLoaderEvent(++_sequence, phase, Time.frameCount,
            _generation, token, MethodName(method))) && LoaderTrace.DroppedEvents == 1)
            Mark("loader-events-truncated", "loader-event-budget-exceeded");
    }

    private static bool OnThread()
    {
        if (Environment.CurrentManagedThreadId == _thread) return true;
        Interlocked.Exchange(ref _offThread, 1);
        return false;
    }

    private static void PreLoadCore()
    {
        if (!_enabled || !OnThread()) return;
        if (!SyncGeneration(Lifecycle.PreLoad())) return;
        _settleAt = -1;
        Mark("pre-load");
    }

    private static void SceneLoadedCore(string scene)
    {
        if (!_enabled || scene != "Main" || !OnThread()) return;
        if (!SyncGeneration(Lifecycle.MainSeen())) return;
        if (!Lifecycle.PreLoadObserved) Mark("main-before-preload");
        Mark("main-scene-loaded");
    }

    private static void LoadedCore()
    {
        if (!_enabled || !OnThread()) return;
        Lifecycle.LoadComplete();
        Capture("load-complete");
        _settleAt = Time.realtimeSinceStartup + 5;
        Flush();
    }

    private static void TickCore()
    {
        if (!_enabled || !OnThread()) return;
        DrainObserverGaps();
        if (!Lifecycle.MainPresent || _settleAt < 0 || Time.realtimeSinceStartup < _settleAt) return;
        _settleAt = -1;
        Capture("settled-five-seconds");
        Flush();
    }

    private static void LeavingCore()
    {
        if (!_enabled || !OnThread()) return;
        Lifecycle.Leaving();
        _settleAt = -1;
        Mark("pre-scene-change"); Flush();
    }

    private static void SceneUnloadedCore(string scene)
    {
        if (!_enabled || scene != "Main" || !OnThread()) return;
        Lifecycle.MainUnloaded(); _settleAt = -1;
        Mark("main-scene-unloaded"); Flush();
    }

    private static bool SyncGeneration(long generation)
    {
        if (_generation == generation) return true;
        string previous = _report;
        if (!Recording.TryAdvance(generation, Flush))
        {
            // Keep the unsaved, bounded old recording. A diagnostic IO failure must not
            // stop the hall lifecycle or silently discard the only available evidence.
            _enabled = false; _settleAt = -1;
            Trace.Append(new ProbeSnapshot("report-rotation-failed", -1, _generation, false,
                new[] { "previous-recording-not-persisted" }, Array.Empty<ProbeEntity>(), ++_sequence));
            Flush();
            return false;
        }
        _previousReport = Path.GetFileName(previous);
        _report = ReportPath(generation);
        Mark("load-recording-start");
        return true;
    }

    private static string ReportPath(long generation) => _reportBase + "-g" + generation.ToString("D4") + ".json";

    private static string MethodName(MethodBase method) => method.DeclaringType?.FullName + "." + method;

    private static void DrainObserverGaps()
    {
        if (Interlocked.Exchange(ref _offThread, 0) != 0)
            Trace.Append(new ProbeSnapshot("observer-gap", -1, _generation, false,
                new[] { "callback-not-captured" }, Array.Empty<ProbeEntity>(), ++_sequence));
    }

    private static void Capture(string phase)
    {
        var entities = new List<ProbeEntity>();
        var errors = new List<string>();
        void Error(string error) { if (errors.Count < 32) errors.Add(error); }
        void Read(string label, Action action)
        {
            try { action(); }
            catch (Exception ex) { Error(label + ":" + ex.GetType().Name); }
        }
        void Add(ProbeEntity entity)
        {
            if (entities.Count < 512) entities.Add(entity);
            else Error("entity-scan-truncated");
        }
        void List<T>(string label, Func<int> count, Func<int, T> at, Action<T> visit)
        {
            Read(label, () =>
            {
                int length = count();
                if (length > 128) Error(label + ":scan-truncated");
                for (int i = 0; i < Math.Min(length, 128); i++)
                {
                    int index = i;
                    Read(label + "[" + index + "]", () => visit(at(index)));
                }
            });
        }
        void Reference(string role, Il2CppObjectBase? target, string parent, Transform? ownerRoot = null)
        {
            var transform = target is Component component ? component.transform :
                target is GameObject gameObject ? gameObject.transform : null;
            string path = "", scene = "unknown", relation = target == null ? "null" : "unknown";
            if (transform != null && transform)
            {
                path = ScenePath(transform);
                scene = transform.gameObject.scene.name;
                if (ownerRoot != null && ownerRoot)
                    relation = transform == ownerRoot || transform.IsChildOf(ownerRoot) ? "internal" : "external";
            }
            Add(new ProbeEntity(role, Id(target), parent, Code: "hierarchy=" + relation + ";scene=" + scene, Path: path));
        }
        void BusinessEntry(Business business)
        {
            if (!business) { Error("business-null"); return; }
            string id = Id(business);
            Add(new ProbeEntity("business", id, Code: business.propertyCode, Path: ScenePath(business.transform)));
            Read("business-state", () => Add(new ProbeEntity("business-state", id,
                Code: $"owned={business._IsOwned_k__BackingField};loaded={business._hasBeenLoaded}")));
            Read("container", () => Reference("container", business._Container_k__BackingField, id, business.transform));
            Read("worldspace-ui", () => Reference("worldspace-ui", business._WorldspaceUIContainer_k__BackingField, id, business.transform));
            Read("spawn", () => Reference("spawn", business.SpawnPoint, id, business.transform));
            Read("interior-spawn", () => Reference("interior-spawn", business.InteriorSpawnPoint, id, business.transform));
            Read("npc-spawn", () => Reference("npc-spawn", business.NPCSpawnPoint, id, business.transform));
            Read("bounds", () => Reference("bounds", business.BoundingBox, id, business.transform));
            Read("employee-idle-points", () =>
            {
                var points = business.EmployeeIdlePoints;
                List("employee-idle-points", () => points.Length, i => points[i],
                    point => Reference("employee-idle-point", point, id, business.transform));
            });
            Read("bounds-colliders", () =>
            {
                var colliders = business.propertyBoundsColliders;
                List("bounds-colliders", () => colliders.Length, i => colliders[i],
                    collider => Reference("bounds-collider", collider, id, business.transform));
            });
            Read("grids", () =>
            {
                var grids = business.Grids;
                List("grids", () => grids.Count, i => grids[i], grid =>
                {
                    if (!grid) { Error("grid-null"); return; }
                    Add(new ProbeEntity("grid", Id(grid), Id(grid._parentProperty), Guid: grid._GUID_k__BackingField.ToString(), Path: ScenePath(grid.transform)));
                    Reference("business-grid-membership", grid, id);
                });
            });
        }
        if (!Lifecycle.MainPresent) Error("main-scene-not-observed");
        if (!Lifecycle.PreLoadObserved) Error("pre-load-event-not-observed");
        if (Lifecycle.Anomaly != null) Error(Lifecycle.Anomaly);
        Read("businesses", () =>
        {
            var businesses = Business.Businesses;
            List("businesses", () => businesses.Count, i => businesses[i], BusinessEntry);
        });
        Read("business-memberships", () =>
        {
            var owned = Business.OwnedBusinesses;
            var unowned = Business.UnownedBusinesses;
            List("owned-businesses", () => owned.Count, i => owned[i], b => Reference("owned-business-membership", b, ""));
            List("unowned-businesses", () => unowned.Count, i => unowned[i], b => Reference("unowned-business-membership", b, ""));
            var properties = NativeProperty.Properties;
            List("properties", () => properties.Count, i => properties[i], p => Reference("property-membership", p, ""));
        });
        Read("stations", () =>
        {
            var stations = Resources.FindObjectsOfTypeAll<LaunderingStation>();
            List("stations", () => stations.Length, i => stations[i], station =>
            {
                if (!station) { Error("station-null"); return; }
                var scene = station.gameObject.scene;
                if (!scene.IsValid() || !scene.isLoaded) return; // Prefab/assets are not live station evidence.
                string id = Id(station);
                Add(new ProbeEntity("station", id, Id(station._ParentProperty_k__BackingField),
                    Guid: station._GUID_k__BackingField.ToString(), Path: ScenePath(station.transform)));
                Read("station-grid", () => Reference("station-grid", station._OwnerGrid_k__BackingField, id));
                Read("station-item", () => Reference("station-item", station._ItemInstance_k__BackingField, id));
                Read("station-interface", () =>
                {
                    var ui = station.Interface;
                    Reference("station-interface", ui, id);
                    if (ui) Reference("interface-business", ui._Business_k__BackingField, Id(ui));
                });
            });
        });
        Read("townhall-anchor", () =>
        {
            var hall = GameObject.Find(NativeSceneSwap.BuildingPath);
            if (!hall) { Error("townhall-anchor-not-found"); return; }
            Add(new ProbeEntity("townhall-anchor", Id(hall), Path: ScenePath(hall.transform)));
        });
        Trace.Append(new ProbeSnapshot(phase, Time.frameCount, _generation, errors.Count == 0, errors, entities, ++_sequence));
    }

    private static string Id(Il2CppObjectBase? value) => value == null ? "null" : "0x" + value.Pointer.ToInt64().ToString("x");

    private static string ScenePath(Transform transform)
    {
        var parts = new List<string>();
        for (int depth = 0; transform && depth < 64; depth++, transform = transform.parent)
            parts.Add(transform.name);
        if (transform) throw new InvalidOperationException("transform-depth-truncated");
        parts.Reverse();
        return string.Join("/", parts);
    }

    private static void Mark(string phase, string? error = null)
    {
        var errors = new List<string>();
        if (error != null) errors.Add(error);
        if (Lifecycle.Anomaly != null) errors.Add(Lifecycle.Anomaly);
        Trace.Append(new ProbeSnapshot(phase, Time.frameCount, _generation, errors.Count == 0,
            errors, Array.Empty<ProbeEntity>(), ++_sequence));
    }

    private static bool Flush()
    {
        if (string.IsNullOrEmpty(_report)) return false;
        try
        {
            // Consume gaps before closing a generation, including gaps arriving since the last Tick.
            DrainObserverGaps();
            var events = LoaderTrace.Events;
            var snapshotAssessment = Trace.Analyze();
            var loaderAssessment = LoaderTrace.Analyze();
            var findings = snapshotAssessment.Findings.Concat(loaderAssessment.Findings).ToArray();
            var report = new
            {
                Schema = 2,
#if TOWNHALL_BUSINESS_ACTIVATION
                Mode = "observation-with-controlled-activation", NativeMutationAllowed = true, WorldPublicationAllowed = true,
#elif TOWNHALL_BUSINESS_SUBTYPE
                Mode = "observation-with-inactive-subtype", NativeMutationAllowed = true, WorldPublicationAllowed = false,
#elif TOWNHALL_BUSINESS_CONSTRUCTION
                Mode = "observation-with-inactive-construction", NativeMutationAllowed = true, WorldPublicationAllowed = false,
#else
                Mode = "observation-only", NativeMutationAllowed = false,
#endif
                RunId = Path.GetFileName(_reportBase), MainGeneration = _generation,
                RecordingKind = _generation == 0 ? "setup" : "load", PreviousReport = _previousReport,
                AssemblySha256 = _assemblyHash, NativeProxySha256 = _nativeHash, S1ApiSha256 = _apiHash,
                CapturedUtc = DateTime.UtcNow, DroppedLoaderEvents = LoaderTrace.DroppedEvents,
                Hooks = Targets.Select(t => new
                {
                    Method = MethodName(t),
                    OwnerPresent = HarmonyLib.Harmony.GetPatchInfo(t)?.Owners.Contains(Owner) == true,
                    RecordedEnters = events.Count(e => e.Method == MethodName(t) && e.Phase == "enter"),
                    RecordedExits = events.Count(e => e.Method == MethodName(t) && e.Phase == "exit")
                }).ToArray(),
                Assessment = new ProbeAssessment(findings.Length == 0 ? "observed" : "incomplete", findings),
                SnapshotAssessment = snapshotAssessment, LoaderAssessment = loaderAssessment,
                HasLoadCompleteInventory = Trace.Snapshots.Any(s => s.Phase == "load-complete"),
                HasSettledInventory = Trace.Snapshots.Any(s => s.Phase == "settled-five-seconds"),
                Trace.Truncated, Trace.Snapshots, LoaderEvents = events,
                Limitation = "Only recorded callbacks are assessed. Zero method sightings do not prove absence of native calls. No creation, early registration, persistence or binding proof."
            };
            File.WriteAllText(_report + ".tmp", JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(_report + ".tmp", _report, true);
            MelonLogger.Msg("TownHall Business probe: " + _report);
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHall Business probe report unavailable: " + ex.GetType().Name);
            return false;
        }
    }

    private static void RemoveHooks()
    {
        _enabled = false;
        try
        {
            _harmony?.UnpatchSelf();
            if (Targets.Any(t => HarmonyLib.Harmony.GetPatchInfo(t)?.Owners.Contains(Owner) == true))
                Mark("unpatch-failed", "observer-owner-still-present");
        }
        catch (Exception ex) { Mark("unpatch-failed", ex.GetType().Name); }
    }

    private static void StopCore()
    {
        GameLifecycle.OnPreLoad -= PreLoad;
        GameLifecycle.OnLoadComplete -= Loaded;
        GameLifecycle.OnPreSceneChange -= Leaving;
        _settleAt = -1;
        RemoveHooks();
        Mark("probe-stop"); Flush();
    }
}
#endif

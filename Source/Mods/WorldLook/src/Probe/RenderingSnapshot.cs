using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace GraphicsProbe;

// Requested-capture-only main-thread work. Construction performs NO Unity reads.
// Values are observations across frames, not an atomic frozen-frame snapshot.
internal sealed class RenderingSnapshot : ISteppedCapture
{
    public BoundedSection<ObservedRecord> Cameras { get; } = new(256);
    public BoundedSection<ObservedRecord> WaterCandidates { get; } = new(128);
    public BoundedSection<ObservedRecord> EffectMatches { get; } = new(ReportPolicy.MaxEffectMatches);
    private readonly BoundedSection<ObservedRecord> _lights = new(256);
    private readonly BoundedSection<ObservedRecord> _probes = new(128);
    private readonly BoundedSection<ObservedRecord> _volumes = new(128);
    private readonly RendererScanBudget _scanBudget = new(ReportPolicy.MaxSurfaceRenderersScanned, ReportPolicy.MaxEffectRenderersScanned);
    private readonly List<string> _errors = new();
    private readonly List<ObservedRecord> _scenes = new();
    private readonly HashSet<int> _cameraIds = new();
    private readonly FairTraversal<Transform> _walker;
    private readonly ObservedRecord _report;
    private Il2CppReferenceArray<GameObject>? _roots;
    private Il2CppArrayBase<Component>? _components;
    private ObservedRecord? _sceneRow;
    private int _phase, _sceneIndex, _sceneLimit, _rootIndex, _rootLimit, _rootQuota;
    private int? _sceneCount;
    private int _componentIndex, _componentLimit;
    private int _renderersEncountered, _rendererScanFailures, _rendererTypeFailures, _rendererHeuristicIncomplete;
    private int _objectsOutsideLoadedScenes, _componentsOmitted, _errorCount, _cameraDuplicates;
    private int _rootAttempts, _rootFailures, _rootLimitOmissions, _rootArraysUnreadable;
    private int _pendingRootsAtRelease, _pendingCursorsAtRelease, _pendingComponentsAtRelease, _pendingRootAdmissionsAtRelease;
    private bool _released;
    public int NodesVisited => _walker.NodesVisited;
    public int RenderersScanned => _scanBudget.SurfaceScanned + _scanBudget.EffectsScanned;
    public bool DiscoveryTruncated { get; private set; }

    public RenderingSnapshot(string reason, string modVersion)
    {
        _walker = new FairTraversal<Transform>(node => node.GetInstanceID(), node => node.childCount,
            (node, index) => node.GetChild(index), VisitNode, Error,
            ReportPolicy.MaxNodes, ReportPolicy.MaxHierarchyDepth, ReportPolicy.MaxChildrenPerNode,
            ReportPolicy.MaxFrontierCursors, ReportPolicy.MaxPendingRoots);
        _report = new ObservedRecord
        {
            ["schemaVersion"] = 2,
            ["modVersion"] = modVersion,
            ["captureReason"] = ReportPolicy.Text(reason),
            ["captureStartedAtUtc"] = DateTime.UtcNow.ToString("O"),
            ["scope"] = "SceneManager-enumerable loaded scenes; inactive descendants and disabled components included. Asset-only and non-enumerated DontDestroyOnLoad objects excluded except the single Camera.main hint.",
            ["limitations"] = new[]
            {
                "Experimental; In-Game-Verify open. Multi-frame observations are NOT an atomic frozen-frame snapshot: objects/settings may change even without scene events.",
                "Camera.main is a tagged enabled hint, not proof of gameplay identity or usable buffers; it supplements rather than replaces inactive scene discovery.",
                "Root admission uses an equal bounded per-scene quota; admitted roots are visited round-robin across scenes before descendants. Parent cursors rotate after one child attempt per level.",
                "Counts describe bounded observed subsets, never world totals. Unknown descendants behind omitted roots/edges/cursors are not counted as known omissions.",
                "Native calls can overrun a cooperative slice; 2ms is NOT a hard frame-time guarantee. Finalization is synchronous and measured separately in the capture log.",
                "Per-scene root, per-object component and shared-material arrays are returned by Unity before item caps apply. No global object/component arrays are requested.",
                "No render/scene/gameplay/save-state mutations, asset extraction or water identification. Particle/line/trail matches have separate scan and retention budgets; non-particle does not prove water."
            },
            ["caps"] = new
            {
                reportUtf8Bytes = ReportPolicy.MaxBytes,
                scenes = ReportPolicy.MaxScenes,
                rootsPerScene = ReportPolicy.MaxRootsPerScene,
                pendingRoots = ReportPolicy.MaxPendingRoots,
                frontierCursors = ReportPolicy.MaxFrontierCursors,
                hierarchyNodes = ReportPolicy.MaxNodes,
                hierarchyDepth = ReportPolicy.MaxHierarchyDepth,
                childrenPerNode = ReportPolicy.MaxChildrenPerNode,
                componentsPerDiscoveryNode = ReportPolicy.MaxComponentsPerNode,
                componentDescriptors = ReportPolicy.MaxComponents,
                sharedMaterialSlots = ReportPolicy.MaxMaterials,
                rendererHeuristicScans = ReportPolicy.MaxRenderersScanned,
                surfaceHeuristicScans = ReportPolicy.MaxSurfaceRenderersScanned,
                particleLineTrailHeuristicScans = ReportPolicy.MaxEffectRenderersScanned,
                surfaceCandidates = WaterCandidates.Limit,
                particleLineTrailMatches = EffectMatches.Limit,
                activeWorkMilliseconds = ReportPolicy.DiscoveryMilliseconds,
                cooperativeSliceMilliseconds = 2,
                stepsPerFrame = 128,
                wallDeadlineMilliseconds = 30000,
                textCharacters = ReportPolicy.MaxText,
                pathCharacters = ReportPolicy.MaxPath,
                errorSamples = 64
            }
        };
    }

    // One initialization phase, root admission, traversal attempt OR component per step.
    public bool Advance()
    {
        if (_released) return false;
        if (_phase == 0)
        {
            _phase++;
            _report.Read("mainCameraHint", () =>
            {
                var camera = Camera.main;
                if (camera == null) return "null: no tagged enabled main hint returned";
                ReadCamera(camera, "Camera.main tagged enabled hint");
                return "observed; not proof of gameplay identity";
            }, ex => Error("Camera.main", ex));
            return true;
        }
        if (_phase == 1) { _phase++; _report.Read("settings", EnvironmentReader.Settings, ex => Error("settings", ex)); return true; }
        if (_phase == 2) { _phase++; _report.Read("environment", EnvironmentReader.Environment, ex => Error("environment", ex)); return true; }
        if (_phase == 3)
        {
            _phase++;
            _report.Read("sceneCount", () => SceneManager.sceneCount, ex => Error("scenes", ex));
            _sceneCount = _report["sceneCount"] is int count ? Math.Max(0, count) : null;
            _sceneLimit = Math.Min(_sceneCount ?? 0, ReportPolicy.MaxScenes);
            _rootQuota = Math.Min(ReportPolicy.MaxRootsPerScene, ReportPolicy.MaxPendingRoots / Math.Max(1, _sceneLimit));
            if (_sceneCount > _sceneLimit) DiscoveryTruncated = true;
            return true;
        }
        if (_phase == 4)
        {
            if (_roots != null && _rootIndex < _rootLimit) { AdmitRoot(); return true; }
            _roots = null;
            if (_sceneIndex < _sceneLimit) { SeedScene(); return true; }
            _walker.FinishSeeding(); _phase++;
            return true;
        }
        if (_components != null && _componentIndex < _componentLimit) { ReadNextComponent(); return true; }
        _components = null;
        return _walker.Advance();
    }

    private void SeedScene()
    {
        int index = _sceneIndex++;
        _sceneRow = new ObservedRecord { ["sceneManagerIndex"] = index, ["rootAdmissionAttempts"] = 0, ["rootsAdmitted"] = 0 };
        _scenes.Add(_sceneRow);
        try
        {
            var scene = SceneManager.GetSceneAt(index);
            _sceneRow.Read("name", () => ReportPolicy.Text(scene.name));
            _sceneRow.Read("handle", () => scene.handle);
            _sceneRow.Read("buildIndex", () => scene.buildIndex);
            _sceneRow.Read("isValid", () => scene.IsValid());
            _sceneRow.Read("isLoaded", () => scene.isLoaded);
            if (!scene.IsValid() || !scene.isLoaded) { _sceneRow["rootsUnavailable"] = "not a valid loaded scene"; DiscoveryTruncated = true; return; }
            _roots = scene.GetRootGameObjects();
            _rootIndex = 0;
            _rootLimit = Math.Min(_roots.Length, _rootQuota);
            _sceneRow["rootsTotalObserved"] = _roots.Length;
            _sceneRow["rootAdmissionLimit"] = _rootLimit;
            _sceneRow["rootAdmissionLimitOmissions"] = _roots.Length - _rootLimit;
            _rootLimitOmissions += _roots.Length - _rootLimit;
            if (_roots.Length > _rootLimit) DiscoveryTruncated = true;
        }
        catch (Exception ex) { Error("scene", ex); _rootArraysUnreadable++; _sceneRow["rootsUnavailable"] = ReportPolicy.Text(ex.GetType().Name + ": " + ex.Message); _roots = null; }
        if (_sceneRow.Errors.Count > 0) Error("scene-fields", new InvalidOperationException("See scene fieldErrors."));
    }

    private void AdmitRoot()
    {
        int index = _rootIndex++;
        _rootAttempts++;
        _sceneRow!["rootAdmissionAttempts"] = (int)_sceneRow["rootAdmissionAttempts"]! + 1;
        try
        {
            var root = _roots![index];
            if (root == null) throw new InvalidOperationException("Null/stale scene root.");
            if (_walker.AddRoot(_sceneIndex - 1, root.transform))
                _sceneRow["rootsAdmitted"] = (int)_sceneRow["rootsAdmitted"]! + 1;
            else DiscoveryTruncated = true;
        }
        catch (Exception ex) { _rootFailures++; Error("scene-root", ex); }
    }

    private void VisitNode(Transform transform)
    {
        // A failed native acquisition/Length read must not publish a new proxy with
        // the previous node's limit. FairTraversal records the thrown visit error.
        _components = null;
        _componentIndex = _componentLimit = 0;
        var go = transform.gameObject;
        var scene = go.scene;
        if (!scene.IsValid() || !scene.isLoaded) { _objectsOutsideLoadedScenes++; DiscoveryTruncated = true; return; }
        var components = go.GetComponents<Component>();
        int count = Math.Max(0, components.Length);
        int limit = Math.Min(count, ReportPolicy.MaxComponentsPerNode);
        _components = components;
        _componentLimit = limit;
        if (limit < count) { _componentsOmitted += count - limit; DiscoveryTruncated = true; }
    }

    private void ReadNextComponent()
    {
        int index = _componentIndex++;
        try
        {
            var component = _components![index];
            if (component == null) return;
            var camera = component.TryCast<Camera>();
            if (camera != null) { ReadCamera(camera, "scene traversal"); return; }
            var renderer = component.TryCast<Renderer>();
            if (renderer != null) { ReadRenderer(renderer); return; }
            var light = component.TryCast<Light>();
            if (light != null) { _lights.Consider(true, () => ObjectReader.LightDetails(light)); return; }
            var reflection = component.TryCast<ReflectionProbe>();
            if (reflection != null) { _probes.Consider(true, () => ObjectReader.ProbeDetails(reflection)); return; }
            var group = component.TryCast<LightProbeGroup>();
            if (group != null) { _probes.Consider(true, () => ObjectReader.ProbeDetails(group)); return; }
            var volume = component.TryCast<Volume>();
            if (volume != null) _volumes.Consider(true, () => ObjectReader.VolumeDetails(volume));
        }
        catch (Exception ex) { Error("component", ex); }
    }

    private void ReadCamera(Camera camera, string discoverySource)
    {
        if (!_cameraIds.Add(camera.GetInstanceID())) { _cameraDuplicates++; return; }
        Cameras.Consider(true, () => ObjectReader.CameraDetails(camera, discoverySource));
    }

    private void ReadRenderer(Renderer renderer)
    {
        _renderersEncountered++;
        string type;
        try { type = ObjectReader.NativeType(renderer); }
        catch (Exception ex) { _rendererTypeFailures++; Error("renderer-type", ex); return; }
        if (!_scanBudget.TryScan(type, out bool effect)) { DiscoveryTruncated = true; return; }
        try
        {
            var clues = ObjectReader.ScanRenderer(renderer);
            if (clues.Unreadable) { _rendererHeuristicIncomplete++; DiscoveryTruncated = true; }
            (effect ? EffectMatches : WaterCandidates).Consider(clues.Evidence.Count > 0,
                () => ObjectReader.WaterCandidate(renderer, clues, effect));
        }
        catch (Exception ex) { _rendererScanFailures++; Error("renderer-heuristics", ex); }
    }

    // Pure report assembly, only after the controller has released native handles.
    public ObservedRecord FinishReport(CooperativeCapture capture)
    {
        if (!_released) throw new InvalidOperationException("Release collection before finalization.");
        if (capture.Status != "completed" && capture.Status != "active-budget" && capture.Status != "wall-deadline")
            throw new InvalidOperationException("Failed/cancelled captures must not replace latest.json.");
        if (capture.Status != "completed" || _walker.StopReason != null || _walker.ChildEdgesOmitted > 0 || _walker.FrontierOmissions > 0)
            DiscoveryTruncated = true;
        foreach (var row in _scenes)
        {
            int index = (int)row["sceneManagerIndex"]!;
            _walker.RootVisitsByScene.TryGetValue(index, out int visited);
            row["rootVisitAttempts"] = visited;
            row["admittedRootsNotVisited"] = (int)row["rootsAdmitted"]! - visited;
        }
        _report["captureEndedAtUtc"] = DateTime.UtcNow.ToString("O");
        _report["scenes"] = new { available = _sceneCount.HasValue, totalObserved = _sceneCount, scanned = _scenes.Count, kept = _scenes.Count, truncated = !_sceneCount.HasValue || _scenes.Count < _sceneCount.Value, items = _scenes };
        _report["cameras"] = Cameras;
        _report["unconfirmedWaterCandidates"] = WaterCandidates;
        _report["unconfirmedParticleLineTrailMatches"] = EffectMatches;
        _report["lights"] = _lights; _report["probes"] = _probes; _report["volumes"] = _volumes;
        _report["rendererInventory"] = new
        {
            encountered = _renderersEncountered,
            scanned = RenderersScanned,
            surfaceScanned = _scanBudget.SurfaceScanned,
            particleLineTrailScanned = _scanBudget.EffectsScanned,
            surfaceScanLimitOmissions = _scanBudget.SurfaceScanOmissions,
            particleLineTrailScanLimitOmissions = _scanBudget.EffectScanOmissions,
            nativeTypeReadFailures = _rendererTypeFailures,
            heuristicReadFailures = _rendererScanFailures,
            incompleteHeuristicReads = _rendererHeuristicIncomplete,
            scanLimitOmissions = _scanBudget.SurfaceScanOmissions + _scanBudget.EffectScanOmissions,
            countsByNativeTypeInEncounteredSubset = _scanBudget.CountsByNativeType,
            interpretation = "Encountered totals cover visited components only. Type failures are unclassified, not charged to either scan budget. All lexical matches unconfirmed; non-particle does not prove water."
        };
        _report["discovery"] = new
        {
            nodesVisited = NodesVisited,
            attempts = _walker.Attempts,
            discoveryTruncated = DiscoveryTruncated,
            stopReason = _walker.StopReason ?? capture.Status,
            objectsOutsideLoadedScenes = _objectsOutsideLoadedScenes,
            componentLimitOmissions = _componentsOmitted,
            childEdgeLimitOmissions = _walker.ChildEdgesOmitted,
            depthBoundaryNodes = _walker.DepthBoundaryNodes,
            frontierChildEdgeOmissions = _walker.FrontierOmissions,
            peakFrontierCursors = _walker.PeakFrontier,
            pendingCursorsAtRelease = _pendingCursorsAtRelease,
            duplicateNodeIdentities = _walker.Duplicates,
            cameraDuplicateIdentities = _cameraDuplicates,
            rootAdmissionAttempts = _rootAttempts,
            rootsAdmitted = _walker.RootsAdmitted,
            rootVisitAttempts = _walker.RootsVisited,
            rootAdmissionFailures = _rootFailures,
            rootAdmissionLimitOmissions = _rootLimitOmissions,
            rootFrontierRejections = _walker.RootsRejected,
            rootArraysUnreadable = _rootArraysUnreadable,
            pendingRootsAtRelease = _pendingRootsAtRelease,
            pendingRootAdmissionsAtRelease = _pendingRootAdmissionsAtRelease,
            pendingComponentsAtRelease = _pendingComponentsAtRelease,
            errors = _errorCount,
            errorSamplesKept = _errors.Count,
            errorsTruncated = _errorCount > _errors.Count,
            errorSamples = _errors
        };
        _report["collectionTiming"] = new
        {
            activeWorkMilliseconds = capture.ActiveMilliseconds,
            wallMilliseconds = capture.WallMilliseconds,
            steps = capture.Steps,
            frames = capture.Frames,
            maxObservedStepMilliseconds = capture.MaxObservedStepMilliseconds,
            status = capture.Status,
            scope = "Sum of timed Advance calls, including Camera.main, initialization, environment and discovery; frame gaps excluded from active work. Wall includes frame gaps. Serialization/write/readback excluded and logged separately; no hard frame-time guarantee."
        };
        return _report;
    }

    public void Dispose()
    {
        if (_released) return;
        _released = true;
        _pendingRootsAtRelease = _walker.PendingRoots;
        _pendingCursorsAtRelease = _walker.PendingCursors;
        _pendingRootAdmissionsAtRelease = _roots == null ? 0 : _rootLimit - _rootIndex;
        _pendingComponentsAtRelease = _components == null ? 0 : _componentLimit - _componentIndex;
        _roots = null; _components = null; _sceneRow = null;
        _cameraIds.Clear(); _walker.Dispose();
    }

    private void Error(string section, Exception ex)
    {
        DiscoveryTruncated = true;
        _errorCount++;
        if (_errors.Count < 64) _errors.Add(ReportPolicy.Text(section + ": " + ex.GetType().Name + ": " + ex.Message));
    }
}


using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NorthIsland;

/// <summary>
/// Disables road barriers, dock walls in the ramp footprint, and invisible boundary colliders
/// blocking access to the Overpass, bridge, and North Island, while keeping references to restore on teardown.
/// Never touches Terrain or general ground/road colliders.
/// </summary>
internal sealed class BarrierRemover
{
    private readonly List<GameObject> _disabledObjects = new();
    private readonly List<Collider> _disabledColliders = new();
    private readonly HashSet<int> _disabledColliderIds = new();
    private readonly List<(BoxCollider box, Vector3 center, Vector3 size)> _resizedBoxes = new();
    private readonly HashSet<int> _resizedBoxIds = new();

    internal int DisabledObjectCount => _disabledObjects.Count;
    internal int DisabledColliderCount => _disabledColliders.Count;

    internal void DisableBlockingObjects()
    {
        _disabledObjects.Clear();
        _disabledColliders.Clear();
        _disabledColliderIds.Clear();
        _resizedBoxes.Clear();
        _resizedBoxIds.Clear();

        DisableNamedBarriers();
        DisableInvisibleWallColliders();
        DisableNativeRampBrokenCollider();

        MelonLogger.Msg($"[NorthIsland] Absperrungen entfernt: {_disabledObjects.Count} Objekte, {_disabledColliders.Count} Boundary-Collider, {_resizedBoxes.Count} angepasste BoxCollider.");
    }

    internal void RestoreAll()
    {
        foreach (var (box, center, size) in _resizedBoxes)
        {
            if (box != null)
            {
                box.center = center;
                box.size = size;
            }
        }
        _resizedBoxes.Clear();
        _resizedBoxIds.Clear();

        foreach (var go in _disabledObjects)
        {
            if (go != null) go.SetActive(true);
        }
        _disabledObjects.Clear();

        foreach (var col in _disabledColliders)
        {
            if (col != null) col.enabled = true;
        }
        _disabledColliders.Clear();
        _disabledColliderIds.Clear();
    }

    private void DisableNamedBarriers()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.IsValid()) return;

        foreach (var root in scene.GetRootGameObjects())
        {
            if (root == null || root.name == NorthIslandWorld.RootName) continue;

            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                if (t == null) continue;

                string name = t.name;
                if (BarrierFilterRules.IsProtectedWorldObject(name)) continue;

                Vector3 p = t.position;

                // 1. All RoadBlocker4_LOD / roadsafetybarrier / Barricade along the Overpass Ramp (z ~ -45.6),
                //    Overpass Segments (z ~ +118.4 & +141.6), and Northtown approach.
                bool isRoadBarrier =
                    name.Contains("RoadBlocker", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("roadsafetybarrier", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Barricade", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Roadblock", StringComparison.OrdinalIgnoreCase);

                if (isRoadBarrier && p.x >= -60f && p.x <= 25f && p.z >= -80f && p.z <= 430f)
                {
                    if (t.gameObject.activeSelf)
                        DisableObject(t.gameObject, $"Barriere '{name}' @ {p}");
                }
            }
        }
    }

    private void DisableInvisibleWallColliders()
    {
        int invisibleLayer = LayerMask.NameToLayer("Invisible");
        if (invisibleLayer < 0) invisibleLayer = 19;

        foreach (var col in UnityEngine.Object.FindObjectsOfType<Collider>(true))
        {
            if (col == null || col.isTrigger) continue;

            string n = col.gameObject.name;
            if (col.transform.root.name == NorthIslandWorld.RootName) continue;

            Bounds b = GetEffectiveColliderBounds(col);
            bool isInvisibleLayer = col.gameObject.layer == invisibleLayer;

            if (BarrierFilterRules.IsInvisibleWallCandidate(
                n,
                isInvisibleLayer,
                b.min.x,
                b.max.x,
                b.min.y,
                b.max.y,
                b.min.z,
                b.max.z))
            {
                // Shrink BoxCollider size to zero as well so OptimizedColliderGroup cannot re-enable the wall later
                var box = col.TryCast<BoxCollider>();
                if (box != null && _resizedBoxIds.Add(box.GetInstanceID()))
                {
                    _resizedBoxes.Add((box, box.center, box.size));
                    box.size = Vector3.zero;
                }

                if (_disabledColliderIds.Add(col.GetInstanceID()))
                {
                    col.enabled = false;
                    _disabledColliders.Add(col);
                }

                MelonLogger.Msg($"[NorthIsland] Unsichtbare Wand deaktiviert: '{n}' layer={col.gameObject.layer} bounds={b.center} size={b.size}");
            }
        }
    }

    private void DisableNativeRampBrokenCollider()
    {
        NormalizeAllOverpassSegmentDeckColliders();
        DisableNativeOverpassRampObstructions();

        DisableSegmentObstructions(
            "Overpass Segment (62)",
            colliderTargetNames: s_merge62ColliderTargets,
            deactivateTargetNames: s_merge62DeactivateTargets,
            disableAllAboveDeckCollidersExceptWestGuard: true);

        foreach (string segName in s_adjacentMedianClearSegments)
        {
            DisableSegmentObstructions(
                segName,
                colliderTargetNames: s_medianBarrierTargets,
                deactivateTargetNames: s_medianBarrierTargets,
                disableAllAboveDeckCollidersExceptWestGuard: false);
        }

        DisableTurnCornerEastFences();
        DisableRoadwayAirspaceObstacles();
        DisableNorthernGapObstacles();
    }

    private static readonly string[] s_merge62ColliderTargets =
    {
        "Block",
        "LOD",
        "Overpass segment LOD1 (EMC Merge)",
        "Divider",
        "concrete_lod1",
        "concrete_lod1 (1)",
        "concrete_lod1 (2)",
        "concrete_lod1 (3)",
        "Cube (1)",
        "Cube (3)",
        "Fence",
        "Fence (1)",
        "Fence (2)",
        "Fence (3)",
        "Fence (4)"
    };

    private static readonly string[] s_merge62DeactivateTargets =
    {
        "Divider",
        "concrete_lod1 (1)",
        "concrete_lod1 (3)",
        "Cube (1)",
        "Cube (3)",
        "Fence",
        "Fence (1)",
        "Fence (2)",
        "Fence (3)",
        "Fence (4)"
    };

    private static readonly string[] s_medianBarrierTargets =
    {
        "Divider",
        "concrete_lod1 (3)",
        "Cube (1)"
    };

    private static readonly string[] s_adjacentMedianClearSegments =
    {
        "Overpass Segment (18)",
        "Overpass Segment (19)",
        "Overpass Segment (20)",
        "Overpass Segment (22)",
        "Overpass Segment (23)",
        "Overpass Segment (30)",
        "Overpass Segment (31)",
        "Overpass Segment (63)",
        "Overpass Segment (64)",
        "Overpass Segment (56)",
        "Overpass Segment (47)",
        "Overpass Segment (55)"
    };

    /// <summary>
    /// Every native Overpass Segment (*) uses an Il2CppScheduleOne.Tools.OptimizedColliderGroup that toggles
    /// colliders based on a 20m player radius, and its only road deck collider is the BoxCollider on
    /// overpass_segment/Concrete (eulerAngles = 270, 90, 0 -> local +Z is world +Y).
    /// By default, Unity auto-sized Concrete's BoxCollider to center=(0, 0, 0.25), size=(10, 14, 0.65),
    /// which spans world y = [9.925 .. 10.575] (including the 0.50m side curbs!) and forms a 50cm-tall solid
    /// wall across the entire 14m roadway above the y = 10.08m asphalt.
    /// Reshaping Concrete's BoxCollider to center=(0, 0, -0.085), size=(10, 14, 0.33) keeps the solid road deck
    /// while placing its world top surface at exact y = 10.080m even when OptimizedColliderGroup re-enables it.
    /// </summary>
    private void NormalizeAllOverpassSegmentDeckColliders()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.IsValid()) return;

        int concreteCount = 0;
        int legCount = 0;

        foreach (var root in scene.GetRootGameObjects())
        {
            if (root == null || root.name == NorthIslandWorld.RootName) continue;

            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                if (t == null) continue;
                string n = t.name;

                if (n.Equals("Concrete", StringComparison.OrdinalIgnoreCase) &&
                    t.parent != null &&
                    t.parent.name.Equals("overpass_segment", StringComparison.OrdinalIgnoreCase))
                {
                    var box = t.GetComponent<BoxCollider>();
                    if (box != null)
                    {
                        _resizedBoxes.Add((box, box.center, box.size));
                        box.center = new Vector3(0f, 0f, -0.085f);
                        box.size = new Vector3(10.0f, 14.0f, 0.33f);
                        concreteCount++;
                    }
                }
                else if (n.Equals("Overpass Leg", StringComparison.OrdinalIgnoreCase))
                {
                    var box = t.GetComponent<BoxCollider>();
                    if (box != null)
                    {
                        _resizedBoxes.Add((box, box.center, box.size));
                        box.center = new Vector3(box.center.x, box.center.y, -0.30f);
                        legCount++;
                    }
                }
                else if (BarrierFilterRules.IsNorthMedianCubeMatch(
                             n,
                             t.parent != null ? t.parent.name : string.Empty,
                             t.position.x,
                             t.position.y,
                             t.position.z))
                {
                    DisableAllCollidersOnTransform(t);
                    if (t.gameObject.activeSelf)
                    {
                        DisableObject(t.gameObject, $"Nord-Mittelstreifen '{n}' @ {t.position}");
                    }
                }
            }
        }

        MelonLogger.Msg($"[NorthIsland] Overpass-Deck-Collider auf Fahrbahnhoehe (y=10.08m) normalisiert: {concreteCount} Concrete-Decks, {legCount} Stuetzpfeiler.");
    }

    private void DisableNativeOverpassRampObstructions()
    {
        var rampRoot = GameObject.Find("Overpass Ramp");
        if (rampRoot == null) return;

        foreach (var t in rampRoot.GetComponentsInChildren<Transform>(true))
        {
            if (t == null) continue;
            string n = t.name;

            // Top 90-degree merge corner fences (x <= -7.5, z <= -80.0) narrow the turn onto Overpass Segment (62) & (20).
            if (t.gameObject.activeSelf && n.StartsWith("FenceContainer", StringComparison.OrdinalIgnoreCase))
            {
                Vector3 p = t.position;
                if (p.z <= -80.0f && p.x <= -7.5f)
                {
                    DisableAllCollidersOnTransform(t);
                    DisableObject(t.gameObject, $"Ramp-Kurvenzaun '{n}' @ {p}");
                    continue;
                }
            }

            // Important: Do not require col.enabled == true because OptimizedColliderGroup disables colliders >20m from player spawn.
            var col = t.GetComponent<Collider>();
            if (col == null) continue;
            Bounds b = col.bounds;

            bool isMainRampMesh =
                n.Equals("Ramp", StringComparison.OrdinalIgnoreCase) ||
                n.StartsWith("RampSupport", StringComparison.OrdinalIgnoreCase) ||
                n.Equals("Block", StringComparison.OrdinalIgnoreCase) ||
                n.Equals("Concrete", StringComparison.OrdinalIgnoreCase) ||
                n.Equals("CurvedBeams", StringComparison.OrdinalIgnoreCase) ||
                n.StartsWith("Crossbeam", StringComparison.OrdinalIgnoreCase) ||
                n.StartsWith("Overpass pole", StringComparison.OrdinalIgnoreCase);

            // Also disable any collider in the top 90-degree curve/throat (z <= -80) except the outer east/south curve rail.
            Vector3 tp = t.position;
            bool isInnerTurnObstacle =
                (col.enabled && b.min.z <= -80.0f && b.max.x <= -7.2f && b.min.z >= -93.6f) ||
                (!col.enabled && tp.z <= -80.0f && tp.x <= -7.2f && tp.z >= -93.6f);

            if (isMainRampMesh || isInnerTurnObstacle)
            {
                DisableAllCollidersOnTransform(t);
                MelonLogger.Msg($"[NorthIsland] Ramp-Collider deaktiviert: '{n}' @ {tp}.");
            }
        }
    }

    private void DisableSegmentObstructions(
        string segmentName,
        string[] colliderTargetNames,
        string[] deactivateTargetNames,
        bool disableAllAboveDeckCollidersExceptWestGuard)
    {
        foreach (var seg in FindSceneObjectsByName(segmentName))
        {
            foreach (var t in seg.GetComponentsInChildren<Transform>(true))
            {
                if (t == null || t == seg.transform) continue;
                string n = t.name;

                // Never disable the normalized flat road deck (Concrete) or support pillars (Overpass Leg).
                bool isNormalizedDeckOrPillar =
                    n.Equals("Concrete", StringComparison.OrdinalIgnoreCase) ||
                    n.Equals("Overpass Leg", StringComparison.OrdinalIgnoreCase);

                if (!isNormalizedDeckOrPillar)
                {
                    // Use Transform world position x <= -24.8f for the west guardrail check because col.bounds is (0,0,0)
                    // when OptimizedColliderGroup has distance-culled (col.enabled = false) the segment at load time.
                    bool keepWestGuard =
                        disableAllAboveDeckCollidersExceptWestGuard &&
                        t.position.x <= -24.8f;

                    if (!keepWestGuard &&
                        (disableAllAboveDeckCollidersExceptWestGuard || MatchesAnyName(n, colliderTargetNames)))
                    {
                        if (DisableAllCollidersOnTransform(t) > 0)
                        {
                            MelonLogger.Msg($"[NorthIsland] Segment-Collider '{n}' auf '{segmentName}' deaktiviert (@ {t.position}).");
                        }
                    }
                }

                if (t.gameObject.activeSelf && MatchesAnyName(n, deactivateTargetNames))
                {
                    DisableAllCollidersOnTransform(t);
                    DisableObject(t.gameObject, $"Segment-Hindernis '{n}' auf '{segmentName}' @ {t.position}");
                }
            }
        }
    }

    private void DisableTurnCornerEastFences()
    {
        // When turning right (north) onto Overpass Segment (20) or left (south) onto Overpass Segment (22),
        // the east-side fence panels immediately adjacent to Overpass Segment (62) (z = -98.5..-79.5, x ~ -11.57)
        // jut into the inner turning radius. Disable both their GameObjects and all 3 BoxColliders per Fence panel.
        DisableEastCornerOnSegment("Overpass Segment (20)", zMin: -84.5f, zMax: -79.5f);
        DisableEastCornerOnSegment("Overpass Segment (22)", zMin: -98.5f, zMax: -93.5f);
    }

    private void DisableEastCornerOnSegment(string segmentName, float zMin, float zMax)
    {
        foreach (var seg in FindSceneObjectsByName(segmentName))
        {
            foreach (var t in seg.GetComponentsInChildren<Transform>(true))
            {
                if (t == null || t == seg.transform) continue;
                string n = t.name;
                Vector3 p = t.position;

                // Cube (3) is the east-wall ShadowCaster (x ~ -11.61) spanning the 10m segment.
                if (n.Equals("Cube (3)", StringComparison.OrdinalIgnoreCase) && p.x > -13.0f)
                {
                    DisableAllCollidersOnTransform(t);
                    if (t.gameObject.activeSelf)
                        DisableObject(t.gameObject, $"Ost-Eck-ShadowCaster '{n}' auf '{segmentName}' @ {p}");
                    continue;
                }

                // Individual 2m east fence panels right next to the ramp mouth (each has 3 BoxColliders in OptimizedColliderGroup).
                if (n.StartsWith("Fence", StringComparison.OrdinalIgnoreCase) &&
                    !n.Contains("billboard", StringComparison.OrdinalIgnoreCase) &&
                    p.x > -13.0f && p.z >= zMin && p.z <= zMax)
                {
                    DisableAllCollidersOnTransform(t);
                    if (t.gameObject.activeSelf)
                        DisableObject(t.gameObject, $"Ost-Eckzaun '{n}' auf '{segmentName}' @ {p}");
                }
            }
        }
    }

    private static List<GameObject> FindSceneObjectsByName(string objectName)
    {
        var matches = new List<GameObject>();
        var scene = SceneManager.GetActiveScene();
        if (!scene.IsValid()) return matches;

        foreach (var root in scene.GetRootGameObjects())
        {
            if (root == null || root.name == NorthIslandWorld.RootName) continue;

            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                if (t != null && t.name.Equals(objectName, StringComparison.OrdinalIgnoreCase))
                    matches.Add(t.gameObject);
            }
        }

        return matches;
    }

    private static Bounds GetEffectiveColliderBounds(Collider col)
    {
        if (col.enabled) return col.bounds;
        var mr = col.GetComponent<MeshRenderer>();
        if (mr != null) return mr.bounds;
        return new Bounds(col.transform.position, new Vector3(1.0f, 1.0f, 1.0f));
    }

    private int DisableAllCollidersOnTransform(Transform t)
    {
        int count = 0;

        foreach (var box in t.GetComponents<BoxCollider>())
        {
            if (box == null) continue;
            if (_resizedBoxIds.Add(box.GetInstanceID()))
            {
                _resizedBoxes.Add((box, box.center, box.size));
                box.size = Vector3.zero;
            }
        }

        foreach (var col in t.GetComponents<Collider>())
        {
            if (col == null) continue;
            if (_disabledColliderIds.Add(col.GetInstanceID()))
            {
                col.enabled = false;
                _disabledColliders.Add(col);
                count++;
            }
        }

        return count;
    }

    private void DisableRoadwayAirspaceObstacles()
    {
        // Spatial safety net: any remaining non-trigger collider from the base map that sticks up above
        // the asphalt deck (y > 10.12) inside the ramp merge throat or across the adjacent Overpass lanes
        // is logged and disabled so turning can never be blocked by a static/merged collider.
        // Never touch Terrain or giant world-spanning bounds.
        foreach (var col in UnityEngine.Object.FindObjectsOfType<Collider>(true))
        {
            if (col == null || col.isTrigger) continue;
            if (col.transform.root.name == NorthIslandWorld.RootName) continue;

            bool hasRb = col.GetComponentInParent<Rigidbody>() != null;
            Bounds b = GetEffectiveColliderBounds(col);

            if (BarrierFilterRules.IsRoadwayAirspaceObstacleMatch(
                col.gameObject.name,
                col.gameObject.layer,
                hasRb,
                b.min.x,
                b.max.x,
                b.min.y,
                b.max.y,
                b.min.z,
                b.max.z))
            {
                var box = col.TryCast<BoxCollider>();
                if (box != null && _resizedBoxIds.Add(box.GetInstanceID()))
                {
                    _resizedBoxes.Add((box, box.center, box.size));
                    box.size = Vector3.zero;
                }

                if (_disabledColliderIds.Add(col.GetInstanceID()))
                {
                    col.enabled = false;
                    _disabledColliders.Add(col);
                }

                MelonLogger.Msg($"[NorthIsland] Fahrbahn-Kollision im Abbiegebereich deaktiviert: '{col.gameObject.name}' (root='{col.transform.root.name}', layer={col.gameObject.layer}, bounds={b.center}, size={b.size})");
            }
        }
    }

    /// <summary>
    /// Removes the transverse construction wall / barrier and any above-deck obstacles at the unfinished
    /// northern Overpass end (x in [-24.8, -12.2], z in [125, 168], y > 10.12m).
    /// Uses MeshRenderer.bounds because OptimizedColliderGroup keeps Collider.bounds at zero >20m from spawn.
    /// </summary>
    private void DisableNorthernGapObstacles()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.IsValid()) return;

        foreach (var root in scene.GetRootGameObjects())
        {
            if (root == null || root.name == NorthIslandWorld.RootName) continue;

            foreach (var mr in root.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (mr == null || !mr.gameObject.activeInHierarchy) continue;

                Bounds b = mr.bounds;
                bool hasRb = mr.GetComponentInParent<Rigidbody>() != null;

                if (!BarrierFilterRules.IsNorthernGapObstacleMatch(
                    mr.gameObject.name,
                    mr.gameObject.layer,
                    hasRb,
                    b.min.x,
                    b.max.x,
                    b.min.y,
                    b.max.y,
                    b.min.z,
                    b.max.z))
                {
                    continue;
                }

                DisableAllCollidersOnTransform(mr.transform);
                if (mr.gameObject.activeSelf)
                    DisableObject(mr.gameObject, $"Nord-Luecken-Hindernis '{mr.gameObject.name}' (bounds={b.center}, size={b.size})");
            }
        }
    }

    private static bool MatchesAnyName(string candidate, string[] targets)
    {
        foreach (string target in targets)
        {
            if (candidate.Equals(target, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private void DisableObject(GameObject go, string reason)
    {
        go.SetActive(false);
        _disabledObjects.Add(go);
        MelonLogger.Msg($"[NorthIsland] Deaktiviert: {reason}");
    }
}

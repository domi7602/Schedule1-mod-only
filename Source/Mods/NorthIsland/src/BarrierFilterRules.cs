using System;

namespace NorthIsland;

/// <summary>
/// Pure C# filtering predicates for BarrierRemover so all spatial bounds, layer guards,
/// and protected object rules can be verified independently of Unity runtime APIs.
/// </summary>
internal static class BarrierFilterRules
{
    /// <summary>
    /// Protects essential world objects (terrain, water, bridges, roads, terrain buildings) from accidental removal.
    /// </summary>
    internal static bool IsProtectedWorldObject(string? name)
    {
        if (string.IsNullOrEmpty(name)) return true;

        return name.Contains("Terrain", StringComparison.OrdinalIgnoreCase) ||
               name.Contains("Overpass", StringComparison.OrdinalIgnoreCase) ||
               name.Contains("Wedge", StringComparison.OrdinalIgnoreCase) ||
               name.Contains("Sidewalk", StringComparison.OrdinalIgnoreCase) ||
               name.Contains("Ground", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Water", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Ocean", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Concrete", StringComparison.OrdinalIgnoreCase) ||
               (name.Contains("Road", StringComparison.OrdinalIgnoreCase) &&
                !name.Contains("RoadBlocker", StringComparison.OrdinalIgnoreCase) &&
                !name.Contains("Roadblock", StringComparison.OrdinalIgnoreCase) &&
                !name.Contains("roadsafetybarrier", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Protects structural meshes and guardrails inside the northern Overpass gap corridor.
    /// </summary>
    internal static bool IsGapStructureEssential(string? name)
    {
        if (string.IsNullOrEmpty(name)) return true;

        return name.Contains("Terrain", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Concrete", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Overpass Leg", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("overpass_segment", StringComparison.OrdinalIgnoreCase) ||
               name.StartsWith("Overpass Segment", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Road", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Road (1)", StringComparison.OrdinalIgnoreCase) ||
               name.StartsWith("Fence", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Identifies North Overpass median blocks while excluding unrelated North Island buildings or props
    /// that happen to carry generic names such as 'Cube (1)'.
    /// </summary>
    internal static bool IsNorthMedianCubeMatch(
        string? name,
        string? parentName,
        float posX,
        float posY,
        float posZ)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (posZ < 130f || posZ > 400f) return false;
        if (posY < 8.5f || posY > 14.0f) return false;

        if (name.Equals("Cube (1)", StringComparison.OrdinalIgnoreCase))
        {
            return !string.IsNullOrEmpty(parentName) &&
                   parentName.Contains("Overpass", StringComparison.OrdinalIgnoreCase) &&
                   MathF.Abs(posX - NorthIslandGeometry.OverpassX) < 1.5f;
        }

        if (MathF.Abs(posX - NorthIslandGeometry.OverpassX) >= 8.0f) return false;

        return name.Equals("Divider", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("concrete_lod1 (3)", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Matches non-essential obstacles standing above the unfinished northern Overpass end (y &gt; 10.12m, z in [125, 168]).
    /// </summary>
    internal static bool IsNorthernGapObstacleMatch(
        string? name,
        int layer,
        bool hasRigidbody,
        float minX,
        float maxX,
        float minY,
        float maxY,
        float minZ,
        float maxZ)
    {
        if (layer == 6 || layer == 24) return false;
        if (hasRigidbody) return false;
        if (IsProtectedWorldObject(name) || IsGapStructureEssential(name)) return false;
        if ((maxX - minX) > 30f || (maxZ - minZ) > 30f) return false;

        return maxX >= -24.8f && minX <= -12.2f &&
               maxZ >= 125.0f && minZ <= 168.0f &&
               maxY > 10.12f && minY < 14.5f;
    }

    /// <summary>
    /// Determines whether an airspace collider blocks the native southern Overpass Ramp / Segment turn area.
    /// Does NOT touch the procedural OverpassBridge gap corridor (z = 136..276) or Northshore Town.
    /// </summary>
    internal static bool IsRoadwayAirspaceObstacleMatch(
        string? name,
        int layer,
        bool hasRigidbody,
        float minX,
        float maxX,
        float minY,
        float maxY,
        float minZ,
        float maxZ)
    {
        if (layer == 6 || layer == 24) return false;
        if (hasRigidbody) return false;
        if (IsProtectedWorldObject(name)) return false;
        if ((maxX - minX) > 60f || (maxZ - minZ) > 60f) return false;
        if (maxY <= 10.12f || minY >= 14.5f) return false;

        bool inNativeRampMouth =
            maxX >= -24.6f && minX <= -7.4f &&
            maxZ >= -93.5f && minZ <= -83.5f;

        bool inOverpassTurnLanes =
            maxX >= -24.6f && minX <= -12.2f &&
            maxZ >= -103.5f && minZ <= -74.5f;

        return inNativeRampMouth || inOverpassTurnLanes;
    }

    /// <summary>
    /// Determines whether a collider belongs to an invisible/explicit boundary wall inside the Overpass or North Island corridor.
    /// </summary>
    internal static bool IsInvisibleWallCandidate(
        string? name,
        bool isInvisibleLayer,
        float minX,
        float maxX,
        float minY,
        float maxY,
        float minZ,
        float maxZ)
    {
        if (IsProtectedWorldObject(name)) return false;

        string n = name ?? string.Empty;
        bool isExplicitBoundary =
            n.Contains("Boundary", StringComparison.OrdinalIgnoreCase) ||
            n.Contains("Border", StringComparison.OrdinalIgnoreCase) ||
            n.Contains("Invisible", StringComparison.OrdinalIgnoreCase) ||
            n.Contains("WorldWall", StringComparison.OrdinalIgnoreCase) ||
            n.Contains("MapEdge", StringComparison.OrdinalIgnoreCase) ||
            n.Contains("MapBound", StringComparison.OrdinalIgnoreCase) ||
            n.Contains("OutOfBounds", StringComparison.OrdinalIgnoreCase) ||
            n.Contains("RoadBlocker", StringComparison.OrdinalIgnoreCase);

        if (!isInvisibleLayer && !isExplicitBoundary) return false;

        bool inRampOrOverpassZone =
            maxX >= -55f && minX <= 20f &&
            maxZ >= -110f && minZ <= 170f &&
            maxY >= -6f && minY <= 40f;

        bool inIslandOrBridgeZone =
            maxX >= -170f && minX <= 225f &&
            maxZ >= 150f && minZ <= 530f &&
            maxY >= -6f && minY <= 40f;

        return inRampOrOverpassZone || inIslandOrBridgeZone;
    }
}

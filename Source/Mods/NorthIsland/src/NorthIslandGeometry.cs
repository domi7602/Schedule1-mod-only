using System;

namespace NorthIsland;

/// <summary>
/// Pure C# geometric and height-sampling helpers for North Island roads, ramps, and approaches.
/// Shared between runtime builders and game-free unit tests.
/// </summary>
internal static class NorthIslandGeometry
{
    internal const float OverpassX = -18.5f;
    internal const float OverpassDeckTopY = 10.08f;
    internal const float OverpassTotalWidth = 14.0f;
    internal const float OverpassRoadWidth = 13.0f;

    internal const float OffRampStartZ = 386.0f;
    internal const float OffRampEndZ = 424.0f;

    internal const float BoulevardMinX = -70.0f;
    internal const float BoulevardMaxX = 105.0f;
    internal const float BoulevardMinZ = 424.0f;
    internal const float BoulevardMaxZ = 434.0f;

    internal const float HighlandStartX = -46.0f;
    internal const float HighlandStartZ = 424.0f;
    internal const float HighlandEndX = -78.0f;
    internal const float HighlandEndZ = 344.0f;
    internal const float HighlandPlateauY = 8.08f;
    internal const float HighlandRoadWidth = 8.4f;
    internal const float HighlandHalfWidth = HighlandRoadWidth * 0.5f;
    internal const float HighlandCurbOffset = HighlandHalfWidth + 0.20f;
    internal const float HighlandCurbWidth = 0.40f;
    internal const float HighlandCurbedTMin = 0.03f;
    internal const float HighlandCurbedTMax = 0.97f;

    internal const float CoastalHouseWestX = -64.5f;
    internal const float CoastalHouseWestZ = 414.2f;
    internal const float CoastalHouseBodyWidth = 9.5f;
    internal const float CoastalHouseBodyDepth = 8.0f;

    // Northshore Boulevard north-boardwalk streetlight poles (shared with the neon string lights).
    internal static readonly float[] NorthLampXs = { -64f, -44f, -29f, -8f, 12f, 32f, 52f, 72f, 92f };

    // Waterfront pier: enters through a guardrail gap at x = 14..20 (clear of the x=12 lamp pole
    // and the x=22 bench), runs north over the coastal shallows to a wider head platform.
    internal const float PierMinX = 14.0f;
    internal const float PierMaxX = 20.0f;
    internal const float PierCenterX = 17.0f;
    internal const float PierStartZ = 436.6f;
    internal const float PierEndZ = 472.0f;
    internal const float PierHeadMinX = 13.0f;
    internal const float PierHeadMaxX = 21.0f;
    internal const float PierHeadMaxZ = 476.5f;
    internal const float PierFreeboard = 0.8f;   // deck top stays this far above the visual sea level
    internal const float PierRailHeight = 1.0f;

    // Visual waterline. The native ocean object's transform/bounds sit at y = -6.50 with a
    // zero-size renderer at load (observed 2026-11-16) and are not the surface the player sees.
    // The heightmap coastline meets the water at about y = 0.
    internal const float VisualSeaLevelY = 0.0f;

    // Lighthouse on a stone islet in open water, northwest of the pier and clear of the
    // bridge channel (x = -25.5..-11.5). z = 496 is ~50 m past the x = -38 shoreline (z ~ 446).
    internal const float LighthouseX = -38.0f;
    internal const float LighthouseZ = 496.0f;
    internal const float LighthouseMoundHalfWidth = 3.5f;
    internal const float LighthouseMoundFreeboard = 2.6f; // mound top above the visual sea level
    internal const int LighthouseNightStartHhmm = 1800;
    internal const int LighthouseDayStartHhmm = 600;
    internal const float LighthouseRevolutionSeconds = 15f;

    // Neon string lights sag between existing north-boardwalk lamp poles.
    internal const float StringLightAttachHeight = 5.45f;
    internal const float StringLightSag = 1.15f;
    internal const float StringLightMinHeadroom = 2.4f;

    /// <summary>
    /// Top surface of the north boardwalk at the pier entrance (flush with Blvd_NorthWalk boxes).
    /// </summary>
    internal static float GetPierWalkTopY() => GetBoulevardSurfaceY(PierCenterX) + 0.10f;

    /// <summary>
    /// Pier deck top elevation for a discovered ocean plane height: never below the boardwalk
    /// and always <see cref="PierFreeboard"/> above the water so waves do not wash over the deck.
    /// </summary>
    internal static float GetPierDeckTopY(float waterY)
        => MathF.Max(GetPierWalkTopY(), waterY + PierFreeboard);

    /// <summary>
    /// Breakwater mound top elevation for a waterline height. Callers pass <see cref="VisualSeaLevelY"/>;
    /// the native ocean object origin is not the visible surface.
    /// </summary>
    internal static float GetLighthouseBaseY(float waterY) => waterY + LighthouseMoundFreeboard;

    /// <summary>
    /// Night is 18:00–05:59 (<paramref name="hhmm"/> is TimeManager.CurrentTime, not elapsed minutes).
    /// An unreadable clock returns false so the beam stays off instead of spinning at noon.
    /// </summary>
    internal static bool TryIsLighthouseNight(int hhmm, out bool night)
    {
        night = false;
        if (hhmm < 0 || hhmm > 2359 || hhmm % 100 >= 60)
            return false;
        night = hhmm >= LighthouseNightStartHhmm || hhmm < LighthouseDayStartHhmm;
        return true;
    }

    /// <summary>
    /// Clips one 5m coast-guardrail segment [x0, x1] against the open pier entrance gap
    /// (PierMinX, PierMaxX). Returns 0..2 visible sub-spans; rails and posts are only built
    /// on the returned spans so the gap stays exactly pier-wide.
    /// </summary>
    internal static (float A, float B)[] ClipCoastRailSpan(float x0, float x1)
    {
        var spans = new System.Collections.Generic.List<(float, float)>(2);
        float leftEnd = MathF.Min(x1, PierMinX);
        if (leftEnd > x0 + 0.001f) spans.Add((x0, leftEnd));
        float rightStart = MathF.Max(x0, PierMaxX);
        if (x1 > rightStart + 0.001f) spans.Add((rightStart, x1));
        return spans.ToArray();
    }

    /// <summary>
    /// Height of a string light at parameter t in [0,1] between attachment heights yA/yB
    /// (parabolic sag, lowest point mid-span).
    /// </summary>
    internal static float GetStringLightY(float yA, float yB, float t)
        => yA + (yB - yA) * t - StringLightSag * 4f * t * (1f - t);

    // Schedule I uses MultiboxPruningBroadphase clamped around [-250, 250] in X/Z and [-50, 50] in Y,
    // which drops dynamic Rigidbody (LandVehicle) collisions north of z = +250. Expand via
    // Physics.RebuildBroadphaseRegions to keep both Hyland Point and North Island inside valid regions.
    internal const float BroadphaseCenterX = 0.0f;
    internal const float BroadphaseCenterY = 25.0f;
    internal const float BroadphaseCenterZ = 175.0f;
    internal const float BroadphaseSizeX = 800.0f;  // X in [-400, +400]
    internal const float BroadphaseSizeY = 200.0f;  // Y in [-75, +125]
    internal const float BroadphaseSizeZ = 950.0f;  // Z in [-300, +650]
    internal const int BroadphaseSubdivisions = 16; // PhysX MBP hard maximum (1..16)

    /// <summary>
    /// Returns true when the world coordinate (<paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>)
    /// lies inside the expanded Physics broadphase volume.
    /// </summary>
    internal static bool ContainsWorldPointInBroadphase(float x, float y, float z)
    {
        float hx = BroadphaseSizeX * 0.5f;
        float hy = BroadphaseSizeY * 0.5f;
        float hz = BroadphaseSizeZ * 0.5f;
        return x >= BroadphaseCenterX - hx && x <= BroadphaseCenterX + hx
            && y >= BroadphaseCenterY - hy && y <= BroadphaseCenterY + hy
            && z >= BroadphaseCenterZ - hz && z <= BroadphaseCenterZ + hz;
    }

    /// <summary>
    /// Returns true if a scene object name/ancestor belongs to Schedule I's InstancingManager proxy objects,
    /// whose shared Mesh bounds are re-centered around local origin for GPU culling.
    /// </summary>
    internal static bool IsInstancedProxyName(string? name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        return name.IndexOf("InstanceWorldObject", StringComparison.OrdinalIgnoreCase) >= 0
            || name.IndexOf("InstancingManager", StringComparison.OrdinalIgnoreCase) >= 0
            || name.IndexOf("InstancedMapTrees", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    /// <summary>
    /// Computes the exact road surface elevation of Northshore Boulevard at longitudinal coordinate <paramref name="x"/>.
    /// Always stays at least 0.16m above underlying terrain across z = 424..434 (including 5m terrain triangle diagonals).
    /// </summary>
    internal static float GetBoulevardSurfaceY(float x)
    {
        float tSouth = NorthIslandTerrainData.SampleHeight(x, 424.0f);
        float tMid = NorthIslandTerrainData.SampleHeight(x, 429.0f);
        float tNorth = NorthIslandTerrainData.SampleHeight(x, 434.0f);
        float baseTerrain = MathF.Max(tSouth, MathF.Max(tMid, tNorth));
        return MathF.Max(0.32f, baseTerrain + 0.16f);
    }

    /// <summary>
    /// Computes the smoothly blended descent elevation of the IslandOffRamp at (<paramref name="x"/>, <paramref name="z"/>).
    /// Blends horizontally toward <see cref="GetBoulevardSurfaceY(float)"/> at z = 424.0m while maintaining
    /// monotonically descending clearance strictly above the hillside terrain across the full ramp width.
    /// </summary>
    internal static float GetOffRampPointY(float x, float z, float startY = OverpassDeckTopY)
    {
        float t = Math.Clamp((z - OffRampStartZ) / (OffRampEndZ - OffRampStartZ), 0f, 1f);
        float cosineT = 0.5f - 0.5f * MathF.Cos(t * MathF.PI);
        float smoothT = 0.55f * cosineT + 0.45f * t;
        float endY = GetBoulevardSurfaceY(x);
        return startY + (endY - startY) * smoothT;
    }

    /// <summary>
    /// Computes the horizontal unit direction (+forward from Boulevard toward West Plateau)
    /// and horizontal unit right vector (+right = West/North-West side) for Highland Drive.
    /// </summary>
    internal static void GetHighlandHorizontalFrame(
        out float dirX,
        out float dirZ,
        out float rightX,
        out float rightZ,
        out float totalHorizLength)
    {
        float dx = HighlandEndX - HighlandStartX;
        float dz = HighlandEndZ - HighlandStartZ;
        totalHorizLength = MathF.Sqrt(dx * dx + dz * dz);
        dirX = dx / totalHorizLength;
        dirZ = dz / totalHorizLength;
        rightX = dirZ;
        rightZ = -dirX;
    }

    /// <summary>
    /// Computes the centerline world point (<paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>)
    /// along Highland Drive for parameter <paramref name="t"/> in [0, 1].
    /// Ensures clearance above sampled terrain and smooth endpoint heights onto Boulevard and West Plateau.
    /// </summary>
    internal static void SampleHighlandCenterline(float t, out float x, out float y, out float z)
    {
        t = Math.Clamp(t, 0f, 1f);
        x = HighlandStartX + (HighlandEndX - HighlandStartX) * t;
        z = HighlandStartZ + (HighlandEndZ - HighlandStartZ) * t;

        float startY = GetBoulevardSurfaceY(HighlandStartX);
        if (t <= 0f)
        {
            y = startY;
            return;
        }

        if (t >= 1f)
        {
            y = HighlandPlateauY;
            return;
        }

        GetHighlandHorizontalFrame(out _, out _, out float rightX, out float rightZ, out _);

        float smoothT = 0.5f - 0.5f * MathF.Cos(t * MathF.PI);
        float curveY = startY + (HighlandPlateauY - startY) * smoothT;

        float tLeft = NorthIslandTerrainData.SampleHeight(x - rightX * HighlandHalfWidth, z - rightZ * HighlandHalfWidth);
        float tCenter = NorthIslandTerrainData.SampleHeight(x, z);
        float tRight = NorthIslandTerrainData.SampleHeight(x + rightX * HighlandHalfWidth, z + rightZ * HighlandHalfWidth);
        float terrainMax = MathF.Max(tLeft, MathF.Max(tCenter, tRight));

        y = MathF.Max(curveY, terrainMax + 0.14f);
    }

    /// <summary>
    /// Computes the intersection X-span half-width of the curbed road profile sliced along constant Z.
    /// </summary>
    internal static float GetHighlandSliceHalfX(float roadHalfWidth = HighlandHalfWidth)
    {
        GetHighlandHorizontalFrame(out float dirX, out float dirZ, out _, out _, out _);
        float absCosZ = MathF.Abs(dirZ);
        return roadHalfWidth / MathF.Max(0.25f, absCosZ);
    }
}

using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace NorthIsland;

/// <summary>
/// Directly continues the native Overpass bridge from the last finished roadway (z = 136..276, Y_blender 141..271)
/// to the North Island Overpass (z = 276..386) and builds a smooth off-ramp (z = 386..424) onto North Island Terrain,
/// plus the smooth collider fix for the southern native Overpass Ramp.
/// Coordinates follow Unity space: ux = bx, uy = bz, uz = -by.
/// </summary>
internal sealed class BridgeBuilder
{
    private const float OverpassX = NorthIslandGeometry.OverpassX;
    private const float OverpassDeckTopY = NorthIslandGeometry.OverpassDeckTopY;
    private const float OverpassTotalWidth = NorthIslandGeometry.OverpassTotalWidth;
    private const float OverpassRoadWidth = NorthIslandGeometry.OverpassRoadWidth;

    private static readonly int[] MissingSegmentBlenderYs =
    {
        141, 151, 161, 171, 181, 191, 201, 211, 221, 231, 241, 251, 261, 271
    };

    private readonly List<Mesh> _createdMeshes = new();

    private Material? _asphaltMat;
    private Material? _concreteMat;
    private Material? _whiteStripeMat;
    private Material? _railMat;

    internal int ProceduralSegmentCount { get; private set; }
    internal bool UsedProceduralFallback { get; private set; }

    internal static readonly Vector3 CityRampStart = new(-3.20f, 0.50f, -43.00f);
    internal static readonly Vector3 IslandArrivalPoint = new(-18.50f, 2.50f, 426.00f);
    internal static readonly Vector3 BridgeMidPoint = new(-18.50f, 11.60f, 221.00f);

    internal void Build(Transform root)
    {
        ProceduralSegmentCount = 0;
        UsedProceduralFallback = false;
        EnsureMaterials();

        BuildOverpassBridgeContinuation(root);
        BuildIslandOffRamp(root);
        EnsureNorthIslandGround(root);
        BuildOriginalOverpassRampColliderFix(root);
    }

    internal void DisposeAssets()
    {
        foreach (var mesh in _createdMeshes)
        {
            if (mesh != null) UnityEngine.Object.Destroy(mesh);
        }
        _createdMeshes.Clear();

        DestroyMaterial(ref _asphaltMat);
        DestroyMaterial(ref _concreteMat);
        DestroyMaterial(ref _whiteStripeMat);
        DestroyMaterial(ref _railMat);
    }

    private static void DestroyMaterial(ref Material? mat)
    {
        if (mat != null)
        {
            UnityEngine.Object.Destroy(mat);
            mat = null;
        }
    }

    /// <summary>
    /// Continues the main 2-lane Overpass straight across the unfinished northern dock end &amp; water gap (z = 136.0..276.0)
    /// to Overpass Segment (56) on North Island (south edge z = 276.0).
    /// Note: Native Overpass Segment meshes in Schedule I are baked into 'Combined Mesh (root: scene)' via Unity
    /// Static Batching, so Object.Instantiate cannot render them at new world positions. Building the 14 segments
    /// (z = 141..271) with dynamic primitives and a single continuous 140m BoxCollider guarantees a visible,
    /// seam-free 2-lane highway bridge at exact y = 10.080m.
    /// </summary>
    private void BuildOverpassBridgeContinuation(Transform root)
    {
        const float gapStartZ = 136.0f;
        const float gapEndZ = 276.0f;
        const float gapLength = gapEndZ - gapStartZ; // 140.0m
        const float gapCenterZ = (gapStartZ + gapEndZ) * 0.5f; // 206.0f

        var bridgeRoot = new GameObject("OverpassBridge_Continuation");
        bridgeRoot.transform.SetParent(root, false);

        // 1. Single continuous 140m road deck collider (top surface at exact y = 10.080f, zero internal seams).
        var deckColObj = new GameObject("OverpassBridge_DeckCollider");
        deckColObj.transform.SetParent(bridgeRoot.transform, false);
        deckColObj.transform.position = new Vector3(OverpassX, OverpassDeckTopY - 0.25f, gapCenterZ);
        var deckBox = deckColObj.AddComponent<BoxCollider>();
        deckBox.size = new Vector3(OverpassTotalWidth, 0.50f, gapLength);

        // 2. Continuous West & East safety guardrail colliders (x = -25.25 and x = -11.75, y = 10.08..11.42).
        var westGuardObj = new GameObject("OverpassBridge_WestGuardCollider");
        westGuardObj.transform.SetParent(bridgeRoot.transform, false);
        westGuardObj.transform.position = new Vector3(OverpassX - 6.75f, OverpassDeckTopY + 0.67f, gapCenterZ);
        var westGuardBox = westGuardObj.AddComponent<BoxCollider>();
        westGuardBox.size = new Vector3(0.50f, 1.34f, gapLength);

        var eastGuardObj = new GameObject("OverpassBridge_EastGuardCollider");
        eastGuardObj.transform.SetParent(bridgeRoot.transform, false);
        eastGuardObj.transform.position = new Vector3(OverpassX + 6.75f, OverpassDeckTopY + 0.67f, gapCenterZ);
        var eastGuardBox = eastGuardObj.AddComponent<BoxCollider>();
        eastGuardBox.size = new Vector3(0.50f, 1.34f, gapLength);

        // 3. Visual 10m segments matching the native Overpass profile (asphalt roadway, concrete slab & curbs,
        //    white lane markings, metal side railings, and twin support pillars into the water).
        foreach (int by in MissingSegmentBlenderYs)
        {
            float uz = by;
            var segRoot = new GameObject($"OverpassBridge_Gap_{by}");
            segRoot.transform.SetParent(bridgeRoot.transform, false);
            segRoot.transform.position = new Vector3(OverpassX, 0f, uz);

            // Concrete structural deck slab (y = 9.45 .. 10.00)
            CreateBox(
                segRoot.transform,
                $"ConcreteSlab_{by}",
                new Vector3(OverpassX, 9.725f, uz),
                new Vector3(OverpassTotalWidth, 0.55f, 10.02f),
                Quaternion.identity,
                _concreteMat,
                withCollider: false);

            // Dark asphalt roadway surface (y = 9.962 .. 10.082, 2mm above native to avoid Z-fighting at overlap)
            CreateBox(
                segRoot.transform,
                $"AsphaltRoad_{by}",
                new Vector3(OverpassX, OverpassDeckTopY - 0.058f, uz),
                new Vector3(OverpassRoadWidth, 0.12f, 10.02f),
                Quaternion.identity,
                _asphaltMat,
                withCollider: false);

            // West & East concrete side curbs (x = -25.25 and x = -11.75, y = 10.08 .. 10.58)
            CreateBox(
                segRoot.transform,
                $"CurbWest_{by}",
                new Vector3(OverpassX - 6.75f, OverpassDeckTopY + 0.25f, uz),
                new Vector3(0.50f, 0.50f, 10.02f),
                Quaternion.identity,
                _concreteMat,
                withCollider: false);

            CreateBox(
                segRoot.transform,
                $"CurbEast_{by}",
                new Vector3(OverpassX + 6.75f, OverpassDeckTopY + 0.25f, uz),
                new Vector3(0.50f, 0.50f, 10.02f),
                Quaternion.identity,
                _concreteMat,
                withCollider: false);

            // West & East metal railings on top of the curbs (matching native Overpass Fence)
            BuildSideRailingVisuals(segRoot.transform, OverpassX - 6.85f, uz, $"RailW_{by}");
            BuildSideRailingVisuals(segRoot.transform, OverpassX + 6.85f, uz, $"RailE_{by}");

            // Center visual median strip & white lane lines (no colliders so lane changes are 100% smooth)
            CreateBox(
                segRoot.transform,
                $"CenterMedianStrip_{by}",
                new Vector3(OverpassX, OverpassDeckTopY + 0.01f, uz),
                new Vector3(0.36f, 0.02f, 10.02f),
                Quaternion.identity,
                _concreteMat,
                withCollider: false);

            // Solid white edge lines for both carriageways
            float[] solidLineOffsets = { -6.15f, -0.35f, 0.35f, 6.15f };
            for (int i = 0; i < solidLineOffsets.Length; i++)
            {
                CreateBox(
                    segRoot.transform,
                    $"EdgeLine_{i}_{by}",
                    new Vector3(OverpassX + solidLineOffsets[i], OverpassDeckTopY + 0.008f, uz),
                    new Vector3(0.12f, 0.01f, 10.02f),
                    Quaternion.identity,
                    _whiteStripeMat,
                    withCollider: false);
            }

            // Dashed white lane divider lines down the center of the West (-3.25) and East (+3.25) carriageways
            float[] dashedLaneOffsets = { -3.25f, 3.25f };
            float[] dashZOffsets = { -2.5f, 2.5f };
            for (int lane = 0; lane < dashedLaneOffsets.Length; lane++)
            {
                for (int d = 0; d < dashZOffsets.Length; d++)
                {
                    CreateBox(
                        segRoot.transform,
                        $"LaneDash_{lane}_{d}_{by}",
                        new Vector3(OverpassX + dashedLaneOffsets[lane], OverpassDeckTopY + 0.008f, uz + dashZOffsets[d]),
                        new Vector3(0.12f, 0.01f, 3.0f),
                        Quaternion.identity,
                        _whiteStripeMat,
                        withCollider: false);
                }
            }

            // Twin concrete support pillars (x = -25.0 and x = -12.0) & crossbeam matching native Overpass Legs
            CreatePillarPair(segRoot.transform, OverpassX, uz, 9.45f, -12.0f, 6.5f);

            ProceduralSegmentCount++;
        }

        MelonLogger.Msg($"[NorthIsland] {ProceduralSegmentCount} Overpass-Brueckensegmente (z=136..276) durchgehend bis zur Nordinsel aufgebaut.");
    }

    private void BuildSideRailingVisuals(Transform parent, float x, float uz, string prefix)
    {
        CreateBox(
            parent,
            $"{prefix}_Top",
            new Vector3(x, OverpassDeckTopY + 1.08f, uz),
            new Vector3(0.08f, 0.06f, 10.02f),
            Quaternion.identity,
            _railMat,
            withCollider: false);

        CreateBox(
            parent,
            $"{prefix}_Mid",
            new Vector3(x, OverpassDeckTopY + 0.78f, uz),
            new Vector3(0.06f, 0.05f, 10.02f),
            Quaternion.identity,
            _railMat,
            withCollider: false);

        for (int p = -4; p <= 4; p += 2)
        {
            CreateBox(
                parent,
                $"{prefix}_Post_{p}",
                new Vector3(x, OverpassDeckTopY + 0.80f, uz + p),
                new Vector3(0.09f, 0.62f, 0.09f),
                Quaternion.identity,
                _railMat,
                withCollider: false);
        }
    }

    /// <summary>
    /// Smoothly connects the north end of Overpass Segment (55) (-18.5, 10.08, 386.0) down onto
    /// Northshore Boulevard (z = 424.0) using a cosine-eased MeshCollider whose transverse edge
    /// follows GetBoulevardSurfaceY(x) pointwise (zero lip across the full 14m width).
    /// </summary>
    private void BuildIslandOffRamp(Transform root)
    {
        const float startZ = NorthIslandGeometry.OffRampStartZ;
        const float endZ = NorthIslandGeometry.OffRampEndZ;
        const float startY = OverpassDeckTopY; // 10.08f
        const int sampleCount = 20; // Every 2.0m from z = 386 to z = 424
        const float halfW = OverpassTotalWidth * 0.5f; // 7.0m

        var rampRoot = new GameObject("IslandOffRamp");
        rampRoot.transform.SetParent(root, false);

        var verts = new Vector3[sampleCount * 2];
        var tris = new int[(sampleCount - 1) * 6];

        float leftX = OverpassX - halfW;
        float rightX = OverpassX + halfW;

        for (int i = 0; i < sampleCount; i++)
        {
            float z = Mathf.Lerp(startZ, endZ, (float)i / (sampleCount - 1));
            float yLeft = NorthIslandGeometry.GetOffRampPointY(leftX, z, startY);
            float yRight = NorthIslandGeometry.GetOffRampPointY(rightX, z, startY);
            verts[i * 2] = new Vector3(leftX, yLeft, z);
            verts[i * 2 + 1] = new Vector3(rightX, yRight, z);
        }

        int ti = 0;
        for (int i = 0; i < sampleCount - 1; i++)
        {
            int bl = i * 2;
            int br = bl + 1;
            int tl = bl + 2;
            int tr = bl + 3;

            // Upward-facing (+Y) winding for z increasing (386 -> 424)
            tris[ti++] = bl;
            tris[ti++] = tl;
            tris[ti++] = br;

            tris[ti++] = br;
            tris[ti++] = tl;
            tris[ti++] = tr;
        }

        var mesh = new Mesh { name = "IslandOffRampSmoothMesh" };
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        _createdMeshes.Add(mesh);

        var colGo = new GameObject("IslandOffRamp_SmoothCollider");
        colGo.transform.SetParent(rampRoot.transform, false);
        var mc = colGo.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        mc.convex = false;

        // Visual segments following the smooth profile (16 steps minimize chord sag so road deck stays cleanly above grass)
        const int visualSteps = 16;
        for (int i = 0; i < visualSteps; i++)
        {
            float z0 = Mathf.Lerp(startZ, endZ, (float)i / visualSteps);
            float z1 = Mathf.Lerp(startZ, endZ, (float)(i + 1) / visualSteps);
            float zMid = (z0 + z1) * 0.5f;
            float y0 = NorthIslandGeometry.GetOffRampPointY(OverpassX, z0, startY);
            float y1 = NorthIslandGeometry.GetOffRampPointY(OverpassX, z1, startY);

            Vector3 p0 = new(OverpassX, y0, z0);
            Vector3 p1 = new(OverpassX, y1, z1);
            Vector3 midTop = (p0 + p1) * 0.5f;
            Vector3 segFwd = (p1 - p0).normalized;

            float yLeftMid = NorthIslandGeometry.GetOffRampPointY(leftX, zMid, startY);
            float yRightMid = NorthIslandGeometry.GetOffRampPointY(rightX, zMid, startY);
            Vector3 transverseRight = new Vector3(OverpassTotalWidth, yRightMid - yLeftMid, 0f).normalized;
            Vector3 segUp = Vector3.Cross(segFwd, transverseRight).normalized;

            Quaternion rot = Quaternion.LookRotation(segFwd, segUp);
            float segLen = Vector3.Distance(p0, p1) + 0.15f;
            Vector3 up = rot * Vector3.up;
            Vector3 right = rot * Vector3.right;

            // Concrete base & asphalt top (visual only, slightly below surface so smooth MeshCollider is flush)
            CreateBox(
                rampRoot.transform,
                $"IslandOffRamp_Slab_{i}",
                midTop - up * 0.28f,
                new Vector3(OverpassTotalWidth, 0.45f, segLen),
                rot,
                _concreteMat,
                withCollider: false);

            CreateBox(
                rampRoot.transform,
                $"IslandOffRamp_Road_{i}",
                midTop - up * 0.05f,
                new Vector3(OverpassRoadWidth, 0.10f, segLen),
                rot,
                _asphaltMat,
                withCollider: false);

            // Side curbs (with colliders so cars stay on the ramp)
            CreateBox(
                rampRoot.transform,
                $"IslandOffRamp_CurbW_{i}",
                midTop - right * 6.75f + up * 0.25f,
                new Vector3(0.50f, 0.50f, segLen),
                rot,
                _concreteMat,
                withCollider: true);

            CreateBox(
                rampRoot.transform,
                $"IslandOffRamp_CurbE_{i}",
                midTop + right * 6.75f + up * 0.25f,
                new Vector3(0.50f, 0.50f, segLen),
                rot,
                _concreteMat,
                withCollider: true);

            if (i % 2 == 0)
            {
                // Dashed white lane lines (every 2nd ~2.4m step matches Overpass rhythm)
                CreateBox(
                    rampRoot.transform,
                    $"IslandOffRamp_DashW_{i}",
                    midTop - right * 3.25f + up * 0.01f,
                    new Vector3(0.12f, 0.01f, segLen * 0.90f),
                    rot,
                    _whiteStripeMat,
                    withCollider: false);

                CreateBox(
                    rampRoot.transform,
                    $"IslandOffRamp_DashE_{i}",
                    midTop + right * 3.25f + up * 0.01f,
                    new Vector3(0.12f, 0.01f, segLen * 0.90f),
                    rot,
                    _whiteStripeMat,
                    withCollider: false);
            }

            if (i % 4 == 2 && midTop.y > 3.5f)
            {
                CreatePillarPair(rampRoot.transform, OverpassX, midTop.z, midTop.y - 0.45f, -5.0f, 6.5f);
            }
        }
    }

    /// <summary>
    /// Builds a curved MeshCollider matching the exact 5m-sampled topography of North Island Terrain
    /// (Schedule_I_Main_FULL_DETAIL.blend -> North Island Terrain.terrain_0_0, X=-160..215, Z=280..520, Y=-10..+8m)
    /// so vehicles follow the hills and coastline naturally instead of clipping through grass or falling out of the world.
    /// </summary>
    private void EnsureNorthIslandGround(Transform root)
    {
        int nx = NorthIslandTerrainData.CountX;
        int nz = NorthIslandTerrainData.CountZ;
        float[] heights = NorthIslandTerrainData.Heights;

        var verts = new Vector3[nx * nz];
        for (int iz = 0; iz < nz; iz++)
        {
            float z = NorthIslandTerrainData.MinZ + iz * NorthIslandTerrainData.Step;
            for (int ix = 0; ix < nx; ix++)
            {
                float x = NorthIslandTerrainData.MinX + ix * NorthIslandTerrainData.Step;
                float y = heights[iz * nx + ix];
                verts[iz * nx + ix] = new Vector3(x, y, z);
            }
        }

        var tris = new int[(nx - 1) * (nz - 1) * 6];
        int ti = 0;
        for (int iz = 0; iz < nz - 1; iz++)
        {
            for (int ix = 0; ix < nx - 1; ix++)
            {
                int v00 = iz * nx + ix;
                int v10 = v00 + 1;
                int v01 = (iz + 1) * nx + ix;
                int v11 = v01 + 1;

                // Upward (+Y) winding
                tris[ti++] = v00;
                tris[ti++] = v01;
                tris[ti++] = v10;

                tris[ti++] = v10;
                tris[ti++] = v01;
                tris[ti++] = v11;
            }
        }

        var mesh = new Mesh { name = "NorthIslandTerrainMesh" };
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        _createdMeshes.Add(mesh);

        var terrainGo = new GameObject("NorthIsland_TerrainMeshCollider");
        terrainGo.transform.SetParent(root, false);
        var mc = terrainGo.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        mc.convex = false;

        MelonLogger.Msg($"[NorthIsland] Gewölbter Nordinsel-Boden-MeshCollider aufgebaut ({nx}x{nz} Raster, {tris.Length / 3} Tris).");
    }

    private void BuildOriginalOverpassRampColliderFix(Transform root)
    {
        // Exact 2m-sampled asphalt surface profile from Schedule_I_Main_FULL_DETAIL.blend (Ramp__R312291 at X=-5.08).
        // Follows the S-curve at the bottom (-46..-54) and top (-74..-82) to the millimeter.
        ReadOnlySpan<(float z, float y)> profile = stackalloc (float z, float y)[]
        {
            (-45.5f, 0.005f),
            (-46.0f, 0.009f),
            (-48.0f, 0.067f),
            (-50.0f, 0.302f),
            (-52.0f, 0.719f),
            (-54.0f, 1.329f),
            (-56.0f, 2.057f),
            (-58.0f, 2.785f),
            (-60.0f, 3.513f),
            (-62.0f, 4.241f),
            (-64.0f, 4.969f),
            (-66.0f, 5.697f),
            (-68.0f, 6.425f),
            (-70.0f, 7.153f),
            (-72.0f, 7.881f),
            (-74.0f, 8.609f),
            (-76.0f, 9.257f),
            (-78.0f, 9.711f),
            (-80.0f, 9.982f),
            (-82.0f, 10.075f),
            (-92.0f, 10.075f),
            (-95.0f, 10.075f)
        };

        const float rampX = -3.20f;
        const float halfW = 4.3f; // 8.6m drivable asphalt width centered on Overpass Ramp (-8.13..+1.72)

        var mesh = new Mesh { name = "NativeRampSmoothMesh" };
        int count = profile.Length;
        var verts = new Vector3[count * 2];
        var tris = new int[(count - 1) * 6];

        for (int i = 0; i < count; i++)
        {
            float z = profile[i].z;
            // Smoothly transition from -0.025f offset on the slope to exact OverpassDeckTopY (10.08f) at the top plateau (z <= -82f)
            // so there is zero vertical step against Overpass Segment (62), (20), and (22).
            float flushY = z <= -82.0f
                ? OverpassDeckTopY
                : z <= -80.0f
                    ? Mathf.Lerp(profile[i].y - 0.025f, OverpassDeckTopY, (-80.0f - z) / 2.0f)
                    : profile[i].y - 0.025f;
            verts[i * 2] = new Vector3(rampX - halfW, flushY, z);
            verts[i * 2 + 1] = new Vector3(rampX + halfW, flushY, z);
        }

        int t = 0;
        for (int i = 0; i < count - 1; i++)
        {
            int bl = i * 2;
            int br = bl + 1;
            int tl = bl + 2;
            int tr = bl + 3;

            // Upward-facing (+Y) winding for z decreasing (-45.5 -> -95.0)
            tris[t++] = bl;
            tris[t++] = br;
            tris[t++] = tl;

            tris[t++] = br;
            tris[t++] = tr;
            tris[t++] = tl;
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        _createdMeshes.Add(mesh);

        var go = new GameObject("NativeRampSmoothCollider");
        go.transform.SetParent(root, false);
        var mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        mc.convex = false;

        // Match OverpassDeckTopY (10.08f) across the entire top curve and Overpass Segment (62) (z = -94.05..-83.95).
        // Keeping z within [-94.05, -83.95] avoids overlapping the sloped NativeRampSmoothCollider north of z = -82.0f.
        var topCurveDeck = new GameObject("NativeRampTopCurveCollider");
        topCurveDeck.transform.SetParent(root, false);
        topCurveDeck.transform.position = new Vector3(-11.75f, OverpassDeckTopY - 0.25f, -89.0f);
        var topBox = topCurveDeck.AddComponent<BoxCollider>();
        topBox.size = new Vector3(26.5f, 0.50f, 10.1f);

        // West-side safety guardrail on Overpass Segment (62) so cars don't fall off the far edge.
        var westGuard = new GameObject("Overpass62_WestGuard");
        westGuard.transform.SetParent(root, false);
        westGuard.transform.position = new Vector3(-25.4f, 10.8f, -89.0f);
        var guardBox = westGuard.AddComponent<BoxCollider>();
        guardBox.size = new Vector3(0.4f, 1.5f, 10.2f);
    }

    private void CreatePillarPair(Transform parent, float cx, float cz, float topY, float bottomY, float halfSpanX)
    {
        float h = topY - bottomY;
        float cy = bottomY + h * 0.5f;
        CreateBox(parent, $"PillarL_{cz:F0}", new Vector3(cx - halfSpanX, cy, cz), new Vector3(0.75f, h, 0.75f), Quaternion.identity, _concreteMat, withCollider: true);
        CreateBox(parent, $"PillarR_{cz:F0}", new Vector3(cx + halfSpanX, cy, cz), new Vector3(0.75f, h, 0.75f), Quaternion.identity, _concreteMat, withCollider: true);
        CreateBox(parent, $"Crossbeam_{cz:F0}", new Vector3(cx, topY - 0.25f, cz), new Vector3(halfSpanX * 2f + 0.7f, 0.50f, 0.60f), Quaternion.identity, _concreteMat, withCollider: true);
    }

    private static GameObject CreateBox(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        Material? mat,
        bool withCollider = true)
        => TownBuilder.CreateBox(parent, name, position, scale, rotation, mat, withCollider);

    private void EnsureMaterials()
    {
        if (_asphaltMat != null) return;
        Shader shader = Shader.Find("Universal Render Pipeline/Lit")
                     ?? Shader.Find("Standard")
                     ?? Shader.Find("Diffuse")
                     ?? Shader.Find("Unlit/Color");

        _asphaltMat = CreateMat(shader, "NI_Asphalt", new Color(0.18f, 0.19f, 0.21f), 0.12f);
        _concreteMat = CreateMat(shader, "NI_Concrete", new Color(0.56f, 0.56f, 0.54f), 0.08f);
        _whiteStripeMat = CreateMat(shader, "NI_WhiteStripe", new Color(0.88f, 0.89f, 0.90f), 0.15f);
        _railMat = CreateMat(shader, "NI_Rail", new Color(0.14f, 0.15f, 0.17f), 0.25f);
    }

    private static Material CreateMat(Shader shader, string name, Color color, float smoothness)
    {
        var m = new Material(shader) { name = name, color = color };
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
        if (m.HasProperty("_Color")) m.SetColor("_Color", color);
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smoothness);
        return m;
    }
}

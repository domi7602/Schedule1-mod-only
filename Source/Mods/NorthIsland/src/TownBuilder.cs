using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace NorthIsland;

/// <summary>
/// Builds the North Island town ("Northshore"):
/// 1. Road network &amp; sidewalks (with full MeshColliders &amp; BoxColliders):
///    - Northshore Boulevard (x = -70..+105, z = 424..434) with seamless off-ramp junction,
///      solid sidewalks, crosswalks &amp; coastal boardwalk railing.
///    - Highland Drive mountain road climbing smoothly from (-46, 424) up to the 8.00m West Plateau (-78, 342).
///    - West Plateau Courtyard &amp; Parking (x = -110..-72, z = 318..344, y = 8.08m).
///    - Town Center Parking Lot (x = +2..+24, z = 414..424, 5 marked stalls).
/// 2. Buildings (all with solid colliders on floors, walls, roofs, steps &amp; props):
///    - Northshore Warehouse Hideout on the 8m West Plateau (-98, 8.08, 330) — walk-in/drive-in 14x18m hall.
///    - Northshore General Store (-33, 415.0), Bayside Diner (+38, 415.0), Harbor Garage (+68, 414.8),
///      and two Coastal Houses (-64.5 and +92).
/// 3. Properly positioned streetlights (outside curbs/sidewalks), benches, trash cans, welcome sign,
///    and real cloned game trees &amp; bushes from the loaded scene (with procedural fallback).
/// </summary>
internal sealed class TownBuilder
{
    internal static readonly Vector3 TownPlazaSpawn = new(-18.50f, 1.60f, 429.00f);
    internal static readonly Vector3 HideoutPlateauSpawn = new(-82.00f, 9.10f, 331.00f);

    private sealed class MeshTemplate
    {
        internal Mesh Mesh = null!;
        internal Material[] Materials = Array.Empty<Material>();
        internal Vector3 BaseScale = Vector3.one;
        internal Quaternion BaseRotation = Quaternion.identity;
        internal float PivotBottomOffsetY;
        internal float TrunkOffsetX;
        internal float TrunkOffsetZ;
        internal float TrunkRadius = 0.55f;
    }

    private readonly List<MeshTemplate> _treeTemplates = new();
    private readonly List<Mesh> _createdMeshes = new();
    private MeshTemplate? _shrubTemplate;

    private Material? _asphaltMat;
    private Material? _sidewalkMat;
    private Material? _curbMat;
    private Material? _whiteLineMat;
    private Material? _yellowLineMat;
    private Material? _brickWallMat;
    private Material? _stuccoWarmMat;
    private Material? _stuccoCoastalMat;
    private Material? _woodMat;
    private Material? _darkMetalMat;
    private Material? _roofSlateMat;
    private Material? _glassMat;
    private Material? _emissiveWarmMat;
    private Material? _emissiveSignMat;
    private Material? _treeTrunkMat;
    private Material? _treeLeavesMat;
    private Material? _awningRedMat;

    internal int BuildingCount { get; private set; }
    internal int StreetlightCount { get; private set; }
    internal int TreeCount { get; private set; }

    internal NorthshoreNeon Neon { get; } = new();
    internal NorthshoreWaterfront Waterfront { get; } = new();

    internal void Build(Transform root)
    {
        BuildingCount = 0;
        StreetlightCount = 0;
        TreeCount = 0;
        _treeTemplates.Clear();
        _shrubTemplate = null;

        EnsureMaterials();
        FindSceneTemplates();

        var townRoot = new GameObject("Northshore_Town");
        townRoot.transform.SetParent(root, false);

        BuildNorthshoreBoulevard(townRoot.transform);
        BuildTownCenterParking(townRoot.transform);
        BuildHighlandDriveAndPlateau(townRoot.transform);
        BuildTownBuildings(townRoot.transform);
        BuildStreetFurnitureAndNature(townRoot.transform);

        // Dependent decor layers (Phase 3): reference the finished town, never reshape it.
        Neon.Build(townRoot.transform);
        Waterfront.Build(townRoot.transform);

        Physics.SyncTransforms();

        MelonLogger.Msg(
            $"[NorthIsland] Stadt 'Northshore' aufgebaut: {BuildingCount} Gebaeude, " +
            $"{StreetlightCount} Strassenlaternen, {TreeCount} Baeume (Spiel-Templates={_treeTemplates.Count}, Busch={(_shrubTemplate != null ? "JA" : "NEIN")}).");
    }

    internal void DisposeAssets()
    {
        foreach (var mesh in _createdMeshes)
        {
            if (mesh != null) UnityEngine.Object.Destroy(mesh);
        }
        _createdMeshes.Clear();

        DestroyMaterial(ref _asphaltMat);
        DestroyMaterial(ref _sidewalkMat);
        DestroyMaterial(ref _curbMat);
        DestroyMaterial(ref _whiteLineMat);
        DestroyMaterial(ref _yellowLineMat);
        DestroyMaterial(ref _brickWallMat);
        DestroyMaterial(ref _stuccoWarmMat);
        DestroyMaterial(ref _stuccoCoastalMat);
        DestroyMaterial(ref _woodMat);
        DestroyMaterial(ref _darkMetalMat);
        DestroyMaterial(ref _roofSlateMat);
        DestroyMaterial(ref _glassMat);
        DestroyMaterial(ref _emissiveWarmMat);
        DestroyMaterial(ref _emissiveSignMat);
        DestroyMaterial(ref _treeTrunkMat);
        DestroyMaterial(ref _treeLeavesMat);
        DestroyMaterial(ref _awningRedMat);

        Neon.DisposeAssets();
        Waterfront.DisposeAssets();
    }

    private static void DestroyMaterial(ref Material? mat)
    {
        if (mat != null)
        {
            UnityEngine.Object.Destroy(mat);
            mat = null;
        }
    }

    // =========================================================================
    // 0. SCENE ASSET TEMPLATE DISCOVERY (REAL GAME TREES & SHRUBS)
    // =========================================================================

    private void FindSceneTemplates()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.IsValid()) return;

        var seenMeshes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var root in scene.GetRootGameObjects())
        {
            if (root == null || root.name == NorthIslandWorld.RootName) continue;

            foreach (var mf in root.GetComponentsInChildren<MeshFilter>(true))
            {
                if (mf == null || mf.sharedMesh == null) continue;
                var mr = mf.GetComponent<MeshRenderer>();
                if (mr == null || mr.isPartOfStaticBatch) continue;

                Mesh mesh = mf.sharedMesh;
                string mName = mesh.name ?? string.Empty;
                if (mName.Contains("Combined Mesh", StringComparison.OrdinalIgnoreCase)) continue;

                Material[] mats = mr.sharedMaterials;
                if (mats == null || mats.Length == 0 || mats[0] == null) continue;

                string goName = mf.gameObject.name ?? string.Empty;
                if (IsInstancedWorldObject(mf.transform, goName))
                    continue;

                // Real non-batched trees (e.g. Pine_L2_LOD0 from Fir L - Variant 2).
                // Skip InstancingManager proxies whose mesh.bounds get zero-centered at Start() and prefer
                // true vertex bounds when readable so non-1.0 prefab scales map into grounded bases.
                Bounds mb = GetAccurateLocalBounds(mesh);
                Vector3 baseScale = SanitizeLossyScale(mf.transform.lossyScale);
                Quaternion baseRot = mf.transform.rotation;
                ComputeWorldBoundsY(mb, baseScale, baseRot, out float worldMinY, out float worldHeight);

                bool isTreeMesh =
                    worldHeight >= 4.0f &&
                    worldHeight <= 45.0f &&
                    (mName.Contains("Pine_", StringComparison.OrdinalIgnoreCase) ||
                     mName.Contains("Fir", StringComparison.OrdinalIgnoreCase) ||
                     mName.Contains("Tree_", StringComparison.OrdinalIgnoreCase) ||
                     goName.Contains("tree_Pine", StringComparison.OrdinalIgnoreCase)) &&
                    !mName.Contains("LOD1", StringComparison.OrdinalIgnoreCase) &&
                    !mName.Contains("LOD2", StringComparison.OrdinalIgnoreCase) &&
                    !mName.Contains("LOD3", StringComparison.OrdinalIgnoreCase);

                if (isTreeMesh && seenMeshes.Add(mName) && _treeTemplates.Count < 4)
                {
                    ComputeTrunkPlacement(
                        mf,
                        mesh,
                        mb,
                        baseScale,
                        baseRot,
                        worldMinY,
                        out float trunkX,
                        out float trunkZ,
                        out float trunkRadius);
                    _treeTemplates.Add(new MeshTemplate
                    {
                        Mesh = mesh,
                        Materials = mats,
                        BaseScale = baseScale,
                        BaseRotation = baseRot,
                        PivotBottomOffsetY = worldMinY,
                        TrunkOffsetX = trunkX,
                        TrunkOffsetZ = trunkZ,
                        TrunkRadius = trunkRadius
                    });
                    MelonLogger.Msg(
                        $"[NorthIsland] Baum-Template [{_treeTemplates.Count}]: mesh='{mName}', go='{goName}', " +
                        $"worldH={worldHeight:F2}, minY={worldMinY:F2}, trunk=({trunkX:F2},{trunkZ:F2},r={trunkRadius:F2}), scale={baseScale}");
                }

                // Real shrub / bush (SGS_Shrub_LOD0)
                if (_shrubTemplate == null &&
                    (mName.Contains("Shrub_LOD0", StringComparison.OrdinalIgnoreCase) ||
                     goName.Equals("SGS_Shrub_LOD0", StringComparison.OrdinalIgnoreCase)))
                {
                    float shrubBoost = 1.15f;
                    Vector3 shrubScale = baseScale * shrubBoost;
                    ComputeWorldBoundsY(mb, shrubScale, baseRot, out float shrubMinY, out _);
                    _shrubTemplate = new MeshTemplate
                    {
                        Mesh = mesh,
                        Materials = mats,
                        BaseScale = shrubScale,
                        BaseRotation = baseRot,
                        PivotBottomOffsetY = shrubMinY
                    };
                }
            }
        }
    }

    // =========================================================================
    // 1. NORTHSHORE BOULEVARD (X = -70 .. +105, Z = 424 .. 434)
    // =========================================================================

    /// <summary>
    /// Computes the exact road surface elevation of Northshore Boulevard at (x, z).
    /// Always stays at least 0.16m above the underlying North Island terrain across z = 424..434
    /// and blends seamlessly with IslandOffRamp across its 14m junction.
    /// </summary>
    internal static float GetBoulevardSurfaceY(float x, float z)
        => NorthIslandGeometry.GetBoulevardSurfaceY(x);

    private void BuildNorthshoreBoulevard(Transform parent)
    {
        const float minX = -70.0f;
        const float maxX = 105.0f;
        const float roadMinZ = 424.0f;
        const float roadMaxZ = 434.0f;
        const float roadCenterZ = 429.0f;
        const float roadWidth = 10.0f;
        const int stepsX = 35; // Every 5.0m from -70 to +105

        var blvdRoot = new GameObject("NorthshoreBoulevard");
        blvdRoot.transform.SetParent(parent, false);

        // 1. Continuous smooth MeshCollider for the asphalt roadway (z = 424.0 .. 434.0)
        var verts = new Vector3[(stepsX + 1) * 2];
        var tris = new int[stepsX * 6];

        for (int i = 0; i <= stepsX; i++)
        {
            float x = Mathf.Lerp(minX, maxX, (float)i / stepsX);
            float y = GetBoulevardSurfaceY(x, roadCenterZ);
            verts[i * 2] = new Vector3(x, y, roadMinZ);
            verts[i * 2 + 1] = new Vector3(x, y, roadMaxZ);
        }

        int ti = 0;
        for (int i = 0; i < stepsX; i++)
        {
            int bl = i * 2;
            int tl = bl + 1;
            int br = bl + 2;
            int tr = bl + 3;

            tris[ti++] = bl;
            tris[ti++] = tl;
            tris[ti++] = br;

            tris[ti++] = br;
            tris[ti++] = tl;
            tris[ti++] = tr;
        }

        var roadMesh = new Mesh { name = "NorthshoreBoulevardMesh" };
        roadMesh.vertices = verts;
        roadMesh.triangles = tris;
        roadMesh.RecalculateNormals();
        roadMesh.RecalculateBounds();
        _createdMeshes.Add(roadMesh);

        var colGo = new GameObject("NorthshoreBoulevard_SmoothCollider");
        colGo.transform.SetParent(blvdRoot.transform, false);
        var mc = colGo.AddComponent<MeshCollider>();
        mc.sharedMesh = roadMesh;
        mc.convex = false;

        // 2. Visual 5m segments + solid sidewalk & guardrail colliders
        for (int i = 0; i < stepsX; i++)
        {
            float x0 = Mathf.Lerp(minX, maxX, (float)i / stepsX);
            float x1 = Mathf.Lerp(minX, maxX, (float)(i + 1) / stepsX);
            float xm = (x0 + x1) * 0.5f;
            float y0 = GetBoulevardSurfaceY(x0, roadCenterZ);
            float y1 = GetBoulevardSurfaceY(x1, roadCenterZ);
            float ym = (y0 + y1) * 0.5f;

            Vector3 p0 = new(x0, y0, roadCenterZ);
            Vector3 p1 = new(x1, y1, roadCenterZ);
            Vector3 dir = (p1 - p0).normalized;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            float segLen = Vector3.Distance(p0, p1) + 0.08f;
            Vector3 up = rot * Vector3.up;

            // Road foundation & asphalt surface (MeshCollider governs top surface)
            // Note: Quaternion.LookRotation(East, Up) already maps local +Z to East (segLen) and local +X to South (width).
            CreateBox(
                blvdRoot.transform,
                $"Blvd_Base_{i}",
                new Vector3(xm, ym - 0.45f, roadCenterZ),
                new Vector3(roadWidth + 5.2f, 0.80f, segLen),
                rot,
                _curbMat,
                withCollider: false);

            CreateBox(
                blvdRoot.transform,
                $"Blvd_Asphalt_{i}",
                new Vector3(xm, ym - 0.045f, roadCenterZ),
                new Vector3(roadWidth, 0.09f, segLen),
                rot,
                _asphaltMat,
                withCollider: false);

            // Center dashed yellow line (skip inside the bridge off-ramp T-junction x = -25.5..-11.5)
            if (xm < -25.5f || xm > -11.5f)
            {
                CreateBox(
                    blvdRoot.transform,
                    $"Blvd_CenterDash_{i}",
                    new Vector3(xm, ym + 0.008f, roadCenterZ) + up * 0.004f,
                    new Vector3(0.14f, 0.01f, segLen * 0.55f),
                    rot,
                    _yellowLineMat,
                    withCollider: false);
            }

            // North white edge line (z = 433.7)
            CreateBox(
                blvdRoot.transform,
                $"Blvd_NorthEdge_{i}",
                new Vector3(xm, ym + 0.008f, roadMaxZ - 0.30f),
                new Vector3(0.12f, 0.01f, segLen),
                rot,
                _whiteLineMat,
                withCollider: false);

            // North Waterfront Boardwalk Sidewalk (z = 434.0 .. 436.6) - WITH COLLIDER!
            CreateBox(
                blvdRoot.transform,
                $"Blvd_NorthWalk_{i}",
                new Vector3(xm, ym + 0.02f, 435.3f),
                new Vector3(2.6f, 0.16f, segLen),
                rot,
                _sidewalkMat,
                withCollider: true);

            // Coastal Safety Guardrail along the northern edge (z = 436.5), clipped around the
            // pier entrance gap (x = 14..20) so the waterfront pier stays reachable on foot.
            foreach (var (a, b) in NorthIslandGeometry.ClipCoastRailSpan(x0, x1))
            {
                float sm = (a + b) * 0.5f;
                float sy = GetBoulevardSurfaceY(sm, roadCenterZ);
                float sLen = (b - a) + 0.08f;
                Vector3 sp0 = new(a, GetBoulevardSurfaceY(a, roadCenterZ), roadCenterZ);
                Vector3 sp1 = new(b, GetBoulevardSurfaceY(b, roadCenterZ), roadCenterZ);
                Quaternion sRot = Quaternion.LookRotation((sp1 - sp0).normalized, Vector3.up);

                CreateBox(
                    blvdRoot.transform,
                    $"Blvd_CoastRailTop_{i}_{a:F0}",
                    new Vector3(sm, sy + 0.96f, 436.5f),
                    new Vector3(0.10f, 0.12f, sLen),
                    sRot,
                    _darkMetalMat,
                    withCollider: true);

                CreateBox(
                    blvdRoot.transform,
                    $"Blvd_CoastRailMid_{i}_{a:F0}",
                    new Vector3(sm, sy + 0.56f, 436.5f),
                    new Vector3(0.08f, 0.06f, sLen),
                    sRot,
                    _darkMetalMat,
                    withCollider: false);

                CreateBox(
                    blvdRoot.transform,
                    $"Blvd_CoastPost_{i}_{a:F0}",
                    new Vector3(sm, sy + 0.52f, 436.5f),
                    new Vector3(0.11f, 0.96f, 0.11f),
                    Quaternion.identity,
                    _darkMetalMat,
                    withCollider: true);
            }

            // South sidewalk (z = 421.4 .. 424.0): leave open ONLY at actual road/driveway entrances
            bool isSouthEntrance =
                (xm >= -52.0f && xm <= -40.0f) ||
                (xm >= -26.0f && xm <= -11.0f) ||
                (xm >= 2.0f && xm <= 24.0f) ||
                (xm >= 60.0f && xm <= 76.0f);

            if (!isSouthEntrance)
            {
                CreateBox(
                    blvdRoot.transform,
                    $"Blvd_SouthEdge_{i}",
                    new Vector3(xm, ym + 0.008f, roadMinZ + 0.30f),
                    new Vector3(0.12f, 0.01f, segLen),
                    rot,
                    _whiteLineMat,
                    withCollider: false);

                CreateBox(
                    blvdRoot.transform,
                    $"Blvd_SouthWalk_{i}",
                    new Vector3(xm, ym + 0.02f, 422.7f),
                    new Vector3(2.6f, 0.16f, segLen),
                    rot,
                    _sidewalkMat,
                    withCollider: true);
            }
        }

        // Smooth sloped end-ramps at both ends of Northshore Boulevard so cars transition cleanly onto grass
        BuildBoulevardEndRamp(blvdRoot.transform, minX, roadCenterZ, roadWidth, isWest: true);
        BuildBoulevardEndRamp(blvdRoot.transform, maxX, roadCenterZ, roadWidth, isWest: false);

        // Crosswalks (Zebrastreifen) at x = -28.0 and x = +26.5
        BuildCrosswalk(blvdRoot.transform, -28.0f, roadCenterZ, roadWidth);
        BuildCrosswalk(blvdRoot.transform, 26.5f, roadCenterZ, roadWidth);

        // Continuous invisible safety barrier collider along North Boardwalk (z = 436.55) so cars
        // cannot jump the rail — split into two boxes around the pier entrance gap (x = 14..20).
        float westGuardWidth = NorthIslandGeometry.PierMinX - minX;
        var northGuardWest = new GameObject("Blvd_NorthCoastGuardCollider_West");
        northGuardWest.transform.SetParent(blvdRoot.transform, false);
        northGuardWest.transform.position = new Vector3(minX + westGuardWidth * 0.5f, 1.40f, 436.55f);
        var ngwBox = northGuardWest.AddComponent<BoxCollider>();
        ngwBox.size = new Vector3(westGuardWidth, 2.2f, 0.50f);

        float eastGuardWidth = maxX - NorthIslandGeometry.PierMaxX;
        var northGuardEast = new GameObject("Blvd_NorthCoastGuardCollider_East");
        northGuardEast.transform.SetParent(blvdRoot.transform, false);
        northGuardEast.transform.position = new Vector3(NorthIslandGeometry.PierMaxX + eastGuardWidth * 0.5f, 1.40f, 436.55f);
        var ngeBox = northGuardEast.AddComponent<BoxCollider>();
        ngeBox.size = new Vector3(eastGuardWidth, 2.2f, 0.50f);
    }

    private void BuildBoulevardEndRamp(Transform parent, float edgeX, float centerZ, float roadWidth, bool isWest)
    {
        const float rampLen = 4.5f;
        float outerX = isWest ? edgeX - rampLen : edgeX + rampLen;
        float yEdge = GetBoulevardSurfaceY(edgeX, centerZ);
        float yOuter = NorthIslandTerrainData.SampleHeight(outerX, centerZ) - 0.06f;

        Vector3 pStart = new(edgeX, yEdge, centerZ);
        Vector3 pEnd = new(outerX, yOuter, centerZ);
        Vector3 mid = (pStart + pEnd) * 0.5f;
        Quaternion rot = Quaternion.LookRotation((pEnd - pStart).normalized, Vector3.up);
        float dist = Vector3.Distance(pStart, pEnd) + 0.15f;

        CreateBox(
            parent,
            isWest ? "Blvd_WestEndRamp" : "Blvd_EastEndRamp",
            mid - (rot * Vector3.up) * 0.12f,
            new Vector3(roadWidth, 0.24f, dist),
            rot,
            _asphaltMat,
            withCollider: true);
    }

    private void BuildCrosswalk(Transform parent, float x, float centerZ, float roadWidth)
    {
        float y = GetBoulevardSurfaceY(x, centerZ) + 0.012f;
        for (float zOff = -roadWidth * 0.42f; zOff <= roadWidth * 0.42f; zOff += 1.1f)
        {
            CreateBox(
                parent,
                $"Crosswalk_{x:F0}_{zOff:F1}",
                new Vector3(x, y, centerZ + zOff),
                new Vector3(2.6f, 0.01f, 0.50f),
                Quaternion.identity,
                _whiteLineMat,
                withCollider: false);
        }
    }

    // =========================================================================
    // 2. TOWN CENTER PARKING LOT (X = +2 .. +24, Z = 414 .. 424)
    // =========================================================================

    private void BuildTownCenterParking(Transform parent)
    {
        var parkRoot = new GameObject("TownCenter_Parking");
        parkRoot.transform.SetParent(parent, false);

        const float minX = 2.0f;
        const float maxX = 24.0f;
        const float minZ = 414.0f;
        const float maxZ = 424.0f;
        float cx = (minX + maxX) * 0.5f; // 13.0f

        float northY = GetBoulevardSurfaceY(cx, 424.0f);
        float southTerrainY = NorthIslandTerrainData.SampleHeight(cx, minZ);
        float southY = Mathf.Max(northY + 0.25f, southTerrainY + 0.14f);

        var verts = new Vector3[]
        {
            new(minX, northY, maxZ),
            new(maxX, northY, maxZ),
            new(minX, southY, minZ),
            new(maxX, southY, minZ)
        };
        var tris = new int[] { 0, 1, 2, 2, 1, 3 };
        var mesh = new Mesh { name = "TownParkingMesh" };
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        _createdMeshes.Add(mesh);

        var colGo = new GameObject("TownParking_Collider");
        colGo.transform.SetParent(parkRoot.transform, false);
        var mc = colGo.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        mc.convex = false;

        Vector3 pNorth = new(cx, northY, maxZ);
        Vector3 pSouth = new(cx, southY, minZ);
        Vector3 mid = (pNorth + pSouth) * 0.5f;
        Quaternion rot = Quaternion.LookRotation((pSouth - pNorth).normalized, Vector3.up);
        float len = Vector3.Distance(pNorth, pSouth) + 0.2f;
        Vector3 up = rot * Vector3.up;

        CreateBox(
            parkRoot.transform,
            "TownParking_Asphalt",
            mid - up * 0.10f,
            new Vector3(maxX - minX, 0.20f, len),
            rot,
            _asphaltMat,
            withCollider: false);

        // Retaining curbs on West, East, and South edges of the parking lot
        CreateBox(parkRoot.transform, "TownParking_CurbW", new Vector3(minX - 0.15f, (northY + southY) * 0.5f + 0.12f, (minZ + maxZ) * 0.5f), new Vector3(0.30f, 0.36f, len), rot, _curbMat, true);
        CreateBox(parkRoot.transform, "TownParking_CurbE", new Vector3(maxX + 0.15f, (northY + southY) * 0.5f + 0.12f, (minZ + maxZ) * 0.5f), new Vector3(0.30f, 0.36f, len), rot, _curbMat, true);
        CreateBox(parkRoot.transform, "TownParking_CurbS", new Vector3(cx, southY + 0.12f, minZ - 0.15f), new Vector3(maxX - minX + 0.6f, 0.36f, 0.30f), Quaternion.identity, _curbMat, true);

        const int stalls = 5;
        float stallWidth = (maxX - minX - 2.0f) / stalls;
        for (int s = 0; s <= stalls; s++)
        {
            float sx = minX + 1.0f + s * stallWidth;
            CreateBox(
                parkRoot.transform,
                $"ParkingLine_{s}",
                new Vector3(sx, Mathf.Lerp(northY, southY, 0.65f), Mathf.Lerp(maxZ, minZ, 0.65f)) + up * 0.01f,
                new Vector3(0.12f, 0.02f, 5.2f),
                rot,
                _whiteLineMat,
                withCollider: false);

            if (s < stalls)
            {
                float stopX = sx + stallWidth * 0.5f;
                CreateBox(
                    parkRoot.transform,
                    $"WheelStop_{s}",
                    new Vector3(stopX, Mathf.Lerp(northY, southY, 0.88f), Mathf.Lerp(maxZ, minZ, 0.88f)) + up * 0.08f,
                    new Vector3(1.8f, 0.14f, 0.22f),
                    rot,
                    _curbMat,
                    withCollider: true);
            }
        }
    }

    // =========================================================================
    // 3. HIGHLAND DRIVE & 8.00M WEST PLATEAU COURTYARD
    // =========================================================================

    private void BuildHighlandDriveAndPlateau(Transform parent)
    {
        var highlandRoot = new GameObject("HighlandDrive_And_Plateau");
        highlandRoot.transform.SetParent(parent, false);

        const int steps = 20;
        const float roadWidth = NorthIslandGeometry.HighlandRoadWidth;
        const float halfW = NorthIslandGeometry.HighlandHalfWidth;
        const int rowCount = steps + 3; // North apron seam + (steps + 1) curbed sections + South apron seam

        NorthIslandGeometry.GetHighlandHorizontalFrame(
            out _,
            out _,
            out float rightX,
            out float rightZ,
            out _);
        Vector3 horizRight = new(rightX, 0f, rightZ);
        float sliceHalfX = NorthIslandGeometry.GetHighlandSliceHalfX();

        var verts = new Vector3[rowCount * 2];
        var tris = new int[(rowCount - 1) * 6];
        var centerPts = new Vector3[steps + 1];

        // Row 0: curb-free seam flush with Northshore Boulevard South Edge (z = 424.0)
        float blvdEastX = NorthIslandGeometry.HighlandStartX + sliceHalfX;
        float blvdWestX = NorthIslandGeometry.HighlandStartX - sliceHalfX;
        verts[0] = new Vector3(blvdEastX, NorthIslandGeometry.GetBoulevardSurfaceY(blvdEastX), NorthIslandGeometry.HighlandStartZ);
        verts[1] = new Vector3(blvdWestX, NorthIslandGeometry.GetBoulevardSurfaceY(blvdWestX), NorthIslandGeometry.HighlandStartZ);

        // Rows 1 .. steps + 1: curbed segments kept within t in [0.03, 0.97] so curbs never enter Boulevard or Courtyard
        for (int i = 0; i <= steps; i++)
        {
            float t = Mathf.Lerp(NorthIslandGeometry.HighlandCurbedTMin, NorthIslandGeometry.HighlandCurbedTMax, (float)i / steps);
            NorthIslandGeometry.SampleHighlandCenterline(t, out float x, out float y, out float z);
            centerPts[i] = new Vector3(x, y, z);
            int row = i + 1;
            verts[row * 2] = centerPts[i] - horizRight * halfW;
            verts[row * 2 + 1] = centerPts[i] + horizRight * halfW;
        }

        // Last row (steps + 2): curb-free seam flush with West Plateau Courtyard North Edge (z = 344.0)
        float platEastX = NorthIslandGeometry.HighlandEndX + sliceHalfX;
        float platWestX = NorthIslandGeometry.HighlandEndX - sliceHalfX;
        int lastRow = steps + 2;
        verts[lastRow * 2] = new Vector3(platEastX, NorthIslandGeometry.HighlandPlateauY, NorthIslandGeometry.HighlandEndZ);
        verts[lastRow * 2 + 1] = new Vector3(platWestX, NorthIslandGeometry.HighlandPlateauY, NorthIslandGeometry.HighlandEndZ);

        int ti = 0;
        for (int i = 0; i < rowCount - 1; i++)
        {
            int bl = i * 2;
            int br = bl + 1;
            int tl = bl + 2;
            int tr = bl + 3;

            tris[ti++] = bl;
            tris[ti++] = tl;
            tris[ti++] = br;

            tris[ti++] = br;
            tris[ti++] = tl;
            tris[ti++] = tr;
        }

        var roadMesh = new Mesh { name = "HighlandDriveMesh" };
        roadMesh.vertices = verts;
        roadMesh.triangles = tris;
        roadMesh.RecalculateNormals();
        if (roadMesh.normals.Length > 0 && roadMesh.normals[0].y < 0f)
        {
            for (int k = 0; k < tris.Length; k += 3)
            {
                (tris[k + 1], tris[k + 2]) = (tris[k + 2], tris[k + 1]);
            }
            roadMesh.triangles = tris;
            roadMesh.RecalculateNormals();
        }
        roadMesh.RecalculateBounds();
        _createdMeshes.Add(roadMesh);

        var colGo = new GameObject("HighlandDrive_SmoothCollider");
        colGo.transform.SetParent(highlandRoot.transform, false);
        var mc = colGo.AddComponent<MeshCollider>();
        mc.sharedMesh = roadMesh;
        mc.convex = false;

        CreateAsphaltQuadVisual(
            highlandRoot.transform,
            "Highland_NorthApron_Asphalt",
            verts[0],
            verts[1],
            verts[2],
            verts[3]);
        CreateAsphaltQuadVisual(
            highlandRoot.transform,
            "Highland_SouthApron_Asphalt",
            verts[(steps + 1) * 2],
            verts[(steps + 1) * 2 + 1],
            verts[lastRow * 2],
            verts[lastRow * 2 + 1]);

        for (int i = 0; i < steps; i++)
        {
            Vector3 p0 = centerPts[i];
            Vector3 p1 = centerPts[i + 1];
            Vector3 mid = (p0 + p1) * 0.5f;
            Quaternion rot = Quaternion.LookRotation((p1 - p0).normalized, Vector3.up);
            float segLen = Vector3.Distance(p0, p1) + 0.12f;
            Vector3 up = rot * Vector3.up;
            Vector3 right = rot * Vector3.right;

            CreateBox(
                highlandRoot.transform,
                $"Highland_Slab_{i}",
                mid - up * 0.32f,
                new Vector3(roadWidth + 0.8f, 0.58f, segLen),
                rot,
                _curbMat,
                withCollider: false);

            CreateBox(
                highlandRoot.transform,
                $"Highland_Asphalt_{i}",
                mid - up * 0.045f,
                new Vector3(roadWidth, 0.09f, segLen),
                rot,
                _asphaltMat,
                withCollider: false);

            CreateBox(
                highlandRoot.transform,
                $"Highland_Dash_{i}",
                mid + up * 0.01f,
                new Vector3(0.12f, 0.01f, segLen * 0.50f),
                rot,
                _yellowLineMat,
                withCollider: false);

            CreateBox(
                highlandRoot.transform,
                $"Highland_CurbL_{i}",
                mid - right * (halfW + 0.20f) + up * 0.14f,
                new Vector3(0.40f, 0.34f, segLen),
                rot,
                _curbMat,
                withCollider: true);

            CreateBox(
                highlandRoot.transform,
                $"Highland_CurbR_{i}",
                mid + right * (halfW + 0.20f) + up * 0.14f,
                new Vector3(0.40f, 0.34f, segLen),
                rot,
                _curbMat,
                withCollider: true);
        }

        // Parametric Streetlights cleanly OUTSIDE the Highland Drive curbs (never on the road!)
        float[] lampTs = { 0.26f, 0.56f, 0.84f };
        for (int l = 0; l < lampTs.Length; l++)
        {
            int idx = Mathf.Clamp(Mathf.RoundToInt(lampTs[l] * steps), 1, steps - 1);
            Vector3 roadPos = centerPts[idx];
            bool eastSide = (l % 2 == 0);
            Vector3 sideOffset = eastSide ? horizRight * (halfW + 0.85f) : -horizRight * (halfW + 0.85f);
            Vector3 lampPos = roadPos + sideOffset;
            lampPos.y = Mathf.Max(roadPos.y - 0.05f, NorthIslandTerrainData.SampleHeight(lampPos.x, lampPos.z));
            Vector3 lookDir = eastSide ? -horizRight : horizRight;
            float yaw = Quaternion.LookRotation(lookDir, Vector3.up).eulerAngles.y;
            BuildStreetlight(highlandRoot.transform, lampPos, yaw, l == 1);
        }

        // B. 8.00m West Plateau Courtyard & Apron (X = -110 .. -72, Z = 318 .. 344, Y = 8.08m)
        const float platMinX = -110.0f;
        const float platMaxX = -72.0f;
        const float platMinZ = 318.0f;
        const float platMaxZ = 344.0f;
        const float platY = 8.08f;

        float platCX = (platMinX + platMaxX) * 0.5f; // -91.0f
        float platCZ = (platMinZ + platMaxZ) * 0.5f; // 331.0f
        float platW = platMaxX - platMinX;           // 38.0f
        float platL = platMaxZ - platMinZ;           // 26.0f

        CreateBox(
            highlandRoot.transform,
            "WestPlateau_CourtyardDeck",
            new Vector3(platCX, platY - 0.15f, platCZ),
            new Vector3(platW, 0.30f, platL),
            Quaternion.identity,
            _asphaltMat,
            withCollider: true);

        CreateBox(
            highlandRoot.transform,
            "WestPlateau_SouthCurb",
            new Vector3(platCX, platY + 0.18f, platMinZ),
            new Vector3(platW, 0.36f, 0.45f),
            Quaternion.identity,
            _curbMat,
            withCollider: true);

        CreateBox(
            highlandRoot.transform,
            "WestPlateau_WestCurb",
            new Vector3(platMinX, platY + 0.18f, platCZ),
            new Vector3(0.45f, 0.36f, platL),
            Quaternion.identity,
            _curbMat,
            withCollider: true);

        CreateBox(
            highlandRoot.transform,
            "WestPlateau_EastCurb",
            new Vector3(platMaxX, platY + 0.18f, platCZ),
            new Vector3(0.45f, 0.36f, platL),
            Quaternion.identity,
            _curbMat,
            withCollider: true);

        for (int p = 0; p <= 3; p++)
        {
            float pz = 322.5f + p * 4.0f;
            CreateBox(
                highlandRoot.transform,
                $"Plateau_ParkLine_{p}",
                new Vector3(-76.5f, platY + 0.01f, pz),
                new Vector3(5.0f, 0.02f, 0.12f),
                Quaternion.identity,
                _whiteLineMat,
                withCollider: false);
        }

        // West Plateau Courtyard streetlights at outer perimeter corners (clear of road entry & parking)
        BuildStreetlight(highlandRoot.transform, new Vector3(-73.0f, platY, 319.2f), 315f, true);
        BuildStreetlight(highlandRoot.transform, new Vector3(-89.0f, platY, 343.2f), 180f, false);
    }

    // =========================================================================
    // 4. TOWN BUILDINGS (SOLID COLLIDERS ON ALL STRUCTURES)
    // =========================================================================

    private void BuildTownBuildings(Transform parent)
    {
        var bldRoot = new GameObject("Town_Buildings");
        bldRoot.transform.SetParent(parent, false);

        BuildHideoutWarehouse(bldRoot.transform, new Vector3(-98.0f, 8.08f, 330.0f));
        BuildGeneralStore(bldRoot.transform, -33.0f, 415.0f);
        BuildBaysideDiner(bldRoot.transform, 38.0f, 415.0f);
        BuildHarborGarage(bldRoot.transform, 68.0f, 414.8f);
        BuildCoastalHouse(bldRoot.transform, "CoastalHouse_West", NorthIslandGeometry.CoastalHouseWestX, NorthIslandGeometry.CoastalHouseWestZ, _stuccoCoastalMat);
        BuildCoastalHouse(bldRoot.transform, "CoastalHouse_East", 92.0f, 414.2f, _stuccoWarmMat);
    }

    private void BuildHideoutWarehouse(Transform parent, Vector3 center)
    {
        var root = new GameObject("Northshore_HideoutWarehouse");
        root.transform.SetParent(parent, false);
        root.transform.position = center;

        const float widthX = 14.0f;
        const float lengthZ = 18.0f;
        const float wallH = 5.2f;
        const float wallT = 0.40f;
        float halfX = widthX * 0.5f;
        float halfZ = lengthZ * 0.5f;

        const float doorWidth = 5.0f;
        const float doorHeight = 3.8f;
        float sideSpanZ = (lengthZ - doorWidth) * 0.5f;

        // Inset FloorSlab inside exterior walls so concrete never Z-fights against exterior red brick faces;
        // add a doorway-width threshold strip flush to the East bay sill.
        CreateBox(root.transform, "FloorSlab", center + new Vector3(0f, 0.03f, 0f), new Vector3(widthX - 0.16f, 0.14f, lengthZ - 0.16f), Quaternion.identity, _sidewalkMat, true);
        CreateBox(root.transform, "DoorThresholdSlab", center + new Vector3(halfX - 0.14f, 0.03f, 0f), new Vector3(0.28f, 0.14f, doorWidth - 0.04f), Quaternion.identity, _sidewalkMat, true);

        // Sink wall bases 0.04m into WestPlateau_CourtyardDeck (8.08m) and butt N/S walls inside E/W walls to avoid coplanar floor flicker
        float wallVisH = wallH + 0.04f;
        float wallCenterY = wallH * 0.5f - 0.02f;
        CreateBox(root.transform, "Wall_West", center + new Vector3(-halfX + wallT * 0.5f, wallCenterY, 0f), new Vector3(wallT, wallVisH, lengthZ), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Wall_North", center + new Vector3(0f, wallCenterY, halfZ - wallT * 0.5f), new Vector3(widthX - wallT * 2f, wallVisH, wallT), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Wall_South", center + new Vector3(0f, wallCenterY, -halfZ + wallT * 0.5f), new Vector3(widthX - wallT * 2f, wallVisH, wallT), Quaternion.identity, _brickWallMat, true);

        CreateBox(root.transform, "Wall_East_SouthWing", center + new Vector3(halfX - wallT * 0.5f, wallCenterY, -halfZ + sideSpanZ * 0.5f), new Vector3(wallT, wallVisH, sideSpanZ), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Wall_East_NorthWing", center + new Vector3(halfX - wallT * 0.5f, wallCenterY, halfZ - sideSpanZ * 0.5f), new Vector3(wallT, wallVisH, sideSpanZ), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Wall_East_Header", center + new Vector3(halfX - wallT * 0.5f, doorHeight + (wallH - doorHeight) * 0.5f, 0f), new Vector3(wallT, wallH - doorHeight, doorWidth), Quaternion.identity, _brickWallMat, true);

        CreateBox(root.transform, "BayFrame_Header", center + new Vector3(halfX + 0.05f, doorHeight + 0.15f, 0f), new Vector3(0.25f, 0.35f, doorWidth + 0.4f), Quaternion.identity, _darkMetalMat, true);
        CreateBox(root.transform, "RoofSlab", center + new Vector3(0f, wallH + 0.18f, 0f), new Vector3(widthX + 0.4f, 0.36f, lengthZ + 0.4f), Quaternion.identity, _roofSlateMat, true);
        CreateBox(root.transform, "EntranceSignBoard", center + new Vector3(halfX + 0.12f, doorHeight + 0.75f, 0f), new Vector3(0.12f, 0.65f, 6.2f), Quaternion.identity, _emissiveSignMat, false);

        // Smooth sloped sill ramp bridging WestPlateau_CourtyardDeck (8.08m) up to Warehouse FloorSlab top (8.18m)
        Vector3 rampBot = center + new Vector3(halfX + 1.10f, 0f, 0f);
        Vector3 rampTop = center + new Vector3(halfX, 0.10f, 0f);
        Vector3 rampMid = (rampBot + rampTop) * 0.5f;
        Quaternion rampRot = Quaternion.LookRotation((rampTop - rampBot).normalized, Vector3.up);
        CreateBox(root.transform, "Warehouse_EntryRamp", rampMid - (rampRot * Vector3.up) * 0.08f, new Vector3(doorWidth - 0.02f, 0.16f, Vector3.Distance(rampBot, rampTop) + 0.15f), rampRot, _sidewalkMat, true);

        // Thick 1.4m flush-exterior anti-tunnel colliders (extend inward so the 5m east bay remains open for driving inside)
        const float wt = 1.40f;
        AddSolidColliderBox(root.transform, "Warehouse_AntiTunnel_W", center + new Vector3(-halfX + wt * 0.5f, wallH * 0.5f, 0f), new Vector3(wt, wallH + 0.8f, lengthZ), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Warehouse_AntiTunnel_N", center + new Vector3(0f, wallH * 0.5f, halfZ - wt * 0.5f), new Vector3(widthX, wallH + 0.8f, wt), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Warehouse_AntiTunnel_S", center + new Vector3(0f, wallH * 0.5f, -halfZ + wt * 0.5f), new Vector3(widthX, wallH + 0.8f, wt), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Warehouse_AntiTunnel_EWingS", center + new Vector3(halfX - wt * 0.5f, wallH * 0.5f, -halfZ + sideSpanZ * 0.5f), new Vector3(wt, wallH + 0.8f, sideSpanZ), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Warehouse_AntiTunnel_EWingN", center + new Vector3(halfX - wt * 0.5f, wallH * 0.5f, halfZ - sideSpanZ * 0.5f), new Vector3(wt, wallH + 0.8f, sideSpanZ), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Warehouse_AntiTunnel_EHeader", center + new Vector3(halfX - wt * 0.5f, doorHeight + (wallH - doorHeight) * 0.5f + 0.2f, 0f), new Vector3(wt, wallH - doorHeight + 0.6f, doorWidth), Quaternion.identity);

        AddWarmPointLight(root.transform, "WarehouseLight_S", center + new Vector3(0f, wallH - 0.5f, -4.5f), 18f, 1.4f);
        AddWarmPointLight(root.transform, "WarehouseLight_N", center + new Vector3(0f, wallH - 0.5f, 4.5f), 18f, 1.4f);

        BuildingCount++;
    }

    private void BuildGeneralStore(Transform parent, float x, float z)
    {
        float northRoadY = GetBoulevardSurfaceY(x, 422.0f);
        float walkTopY = northRoadY + 0.10f;
        float backTerrainY = NorthIslandTerrainData.SampleHeight(x, z - 4.0f);
        float floorY = Mathf.Max(northRoadY, backTerrainY) + 0.10f;

        var root = new GameObject("Northshore_GeneralStore");
        root.transform.SetParent(parent, false);
        Vector3 center = new(x, floorY, z);
        root.transform.position = center;

        const float w = 12.0f;
        const float d = 8.0f;
        const float h = 4.2f;
        float frontWallZ = z + d * 0.5f; // 419.0
        float stepBottomY = northRoadY - 0.60f;

        // Foundation kept strictly under building footprint (never overlapping open entrance stairs or Blvd_SouthWalk)
        CreateBox(root.transform, "Store_Foundation", new Vector3(x, floorY - 0.65f, z), new Vector3(w + 0.8f, 1.30f, d + 0.8f), Quaternion.identity, _sidewalkMat, true);

        // Solid entrance threshold landing from z = 419.0 .. 420.0, then 3 open solid steps climbing from z = 421.4 (walkTopY)
        float landingH = Mathf.Max(0.35f, floorY - stepBottomY);
        CreateBox(root.transform, "Store_Threshold", new Vector3(x, stepBottomY + landingH * 0.5f, frontWallZ + 0.50f), new Vector3(4.5f, landingH, 1.05f), Quaternion.identity, _sidewalkMat, true);

        for (int s = 0; s < 3; s++)
        {
            float stepTopY = Mathf.Lerp(walkTopY, floorY, (s + 1) / 4f);
            float stepH = Mathf.Max(0.25f, stepTopY - stepBottomY);
            float stepZ = 421.4f - (s + 0.5f) * (1.40f / 3f);
            CreateBox(root.transform, $"Store_Step_{s}", new Vector3(x, stepBottomY + stepH * 0.5f, stepZ), new Vector3(4.5f, stepH, 0.54f), Quaternion.identity, _sidewalkMat, true);
        }

        CreateBox(root.transform, "Wall_Back", center + new Vector3(0f, h * 0.5f, -d * 0.5f), new Vector3(w, h, 0.32f), Quaternion.identity, _stuccoWarmMat, true);
        CreateBox(root.transform, "Wall_West", center + new Vector3(-w * 0.5f, h * 0.5f, 0f), new Vector3(0.32f, h, d), Quaternion.identity, _stuccoWarmMat, true);
        CreateBox(root.transform, "Wall_East", center + new Vector3(w * 0.5f, h * 0.5f, 0f), new Vector3(0.32f, h, d), Quaternion.identity, _stuccoWarmMat, true);

        CreateBox(root.transform, "Wall_FrontW", center + new Vector3(-3.55f, h * 0.5f, d * 0.5f), new Vector3(4.9f, h, 0.32f), Quaternion.identity, _stuccoWarmMat, true);
        CreateBox(root.transform, "Wall_FrontE", center + new Vector3(3.55f, h * 0.5f, d * 0.5f), new Vector3(4.9f, h, 0.32f), Quaternion.identity, _stuccoWarmMat, true);
        CreateBox(root.transform, "Wall_FrontHeader", center + new Vector3(0f, 3.35f, d * 0.5f), new Vector3(2.2f, 1.7f, 0.32f), Quaternion.identity, _stuccoWarmMat, true);

        CreateBox(root.transform, "Window_W", center + new Vector3(-3.5f, 1.8f, d * 0.5f + 0.08f), new Vector3(3.2f, 1.8f, 0.08f), Quaternion.identity, _glassMat, true);
        CreateBox(root.transform, "Window_E", center + new Vector3(3.5f, 1.8f, d * 0.5f + 0.08f), new Vector3(3.2f, 1.8f, 0.08f), Quaternion.identity, _glassMat, true);

        CreateBox(root.transform, "Store_Awning", center + new Vector3(0f, 2.85f, d * 0.5f + 0.9f), new Vector3(w - 0.4f, 0.22f, 1.6f), Quaternion.Euler(12f, 0f, 0f), _awningRedMat, true);
        CreateBox(root.transform, "Store_Sign", center + new Vector3(0f, 3.65f, d * 0.5f + 0.18f), new Vector3(6.5f, 0.55f, 0.12f), Quaternion.identity, _emissiveSignMat, false);

        CreateBox(root.transform, "Store_Roof", center + new Vector3(0f, h + 0.18f, 0f), new Vector3(w + 0.5f, 0.36f, d + 0.5f), Quaternion.identity, _roofSlateMat, true);
        CreateBox(root.transform, "Store_Counter", center + new Vector3(0f, 0.55f, -1.8f), new Vector3(5.0f, 1.1f, 0.9f), Quaternion.identity, _woodMat, true);

        // Thick 1.4m flush-exterior anti-tunnel colliders + doorway bollards (1.16m clear walk-in gap for players, blocks cars)
        const float wt = 1.40f;
        AddSolidColliderBox(root.transform, "Store_AntiTunnel_Back", center + new Vector3(0f, h * 0.5f, -d * 0.5f + wt * 0.5f - 0.16f), new Vector3(w + 0.4f, h + 1.4f, wt), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Store_AntiTunnel_W", center + new Vector3(-w * 0.5f + wt * 0.5f - 0.16f, h * 0.5f, 0f), new Vector3(wt, h + 1.4f, d + 0.4f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Store_AntiTunnel_E", center + new Vector3(w * 0.5f - wt * 0.5f + 0.16f, h * 0.5f, 0f), new Vector3(wt, h + 1.4f, d + 0.4f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Store_AntiTunnel_FrontW", center + new Vector3(-3.55f, h * 0.5f, d * 0.5f - wt * 0.5f + 0.16f), new Vector3(4.9f, h + 1.4f, wt), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Store_AntiTunnel_FrontE", center + new Vector3(3.55f, h * 0.5f, d * 0.5f - wt * 0.5f + 0.16f), new Vector3(4.9f, h + 1.4f, wt), Quaternion.identity);
        CreateBox(root.transform, "Store_BollardL", new Vector3(x - 0.70f, floorY + 0.48f, frontWallZ + 0.70f), new Vector3(0.24f, 0.96f, 0.24f), Quaternion.identity, _darkMetalMat, true);
        CreateBox(root.transform, "Store_BollardR", new Vector3(x + 0.70f, floorY + 0.48f, frontWallZ + 0.70f), new Vector3(0.24f, 0.96f, 0.24f), Quaternion.identity, _darkMetalMat, true);

        AddWarmPointLight(root.transform, "Store_Light", center + new Vector3(0f, h - 0.5f, 0f), 14f, 1.2f);
        BuildingCount++;
    }

    private void BuildBaysideDiner(Transform parent, float x, float z)
    {
        float northRoadY = GetBoulevardSurfaceY(x, 422.0f);
        float walkTopY = northRoadY + 0.10f;
        float backTerrainY = NorthIslandTerrainData.SampleHeight(x, z - 3.8f);
        float floorY = Mathf.Max(northRoadY, backTerrainY) + 0.10f;

        var root = new GameObject("Northshore_BaysideDiner");
        root.transform.SetParent(parent, false);
        Vector3 center = new(x, floorY, z);
        root.transform.position = center;

        const float w = 11.0f;
        const float d = 7.5f;
        const float h = 3.9f;
        float frontWallZ = z + d * 0.5f; // 418.75
        float stepBottomY = northRoadY - 0.60f;
        float deckH = Mathf.Max(0.35f, floorY - stepBottomY);

        // Foundation under diner body; flanked patio wings leave central 3.8m doorway stairwell open down to z=421.4
        CreateBox(root.transform, "Diner_Foundation", new Vector3(x, floorY - 0.65f, z), new Vector3(w + 0.8f, 1.30f, d + 0.8f), Quaternion.identity, _sidewalkMat, true);
        CreateBox(root.transform, "Diner_PatioDeckW", new Vector3(x - 3.65f, stepBottomY + deckH * 0.5f, frontWallZ + 1.30f), new Vector3(3.7f, deckH, 2.50f), Quaternion.identity, _sidewalkMat, true);
        CreateBox(root.transform, "Diner_PatioDeckE", new Vector3(x + 3.65f, stepBottomY + deckH * 0.5f, frontWallZ + 1.30f), new Vector3(3.7f, deckH, 2.50f), Quaternion.identity, _sidewalkMat, true);
        CreateBox(root.transform, "Diner_Threshold", new Vector3(x, stepBottomY + deckH * 0.5f, frontWallZ + 0.55f), new Vector3(3.6f, deckH, 1.15f), Quaternion.identity, _sidewalkMat, true);

        for (int s = 0; s < 3; s++)
        {
            float stepTopY = Mathf.Lerp(walkTopY, floorY, (s + 1) / 4f);
            float stepH = Mathf.Max(0.25f, stepTopY - stepBottomY);
            float stepZ = 421.4f - (s + 0.5f) * (1.55f / 3f);
            CreateBox(root.transform, $"Diner_Step_{s}", new Vector3(x, stepBottomY + stepH * 0.5f, stepZ), new Vector3(3.5f, stepH, 0.58f), Quaternion.identity, _sidewalkMat, true);
        }

        CreateBox(root.transform, "Wall_Back", center + new Vector3(0f, h * 0.5f, -d * 0.5f), new Vector3(w, h, 0.30f), Quaternion.identity, _stuccoCoastalMat, true);
        CreateBox(root.transform, "Wall_West", center + new Vector3(-w * 0.5f, h * 0.5f, 0f), new Vector3(0.30f, h, d), Quaternion.identity, _stuccoCoastalMat, true);
        CreateBox(root.transform, "Wall_East", center + new Vector3(w * 0.5f, h * 0.5f, 0f), new Vector3(0.30f, h, d), Quaternion.identity, _stuccoCoastalMat, true);
        CreateBox(root.transform, "Wall_FrontW", center + new Vector3(-3.3f, h * 0.5f, d * 0.5f), new Vector3(4.4f, h, 0.30f), Quaternion.identity, _stuccoCoastalMat, true);
        CreateBox(root.transform, "Wall_FrontE", center + new Vector3(3.3f, h * 0.5f, d * 0.5f), new Vector3(4.4f, h, 0.30f), Quaternion.identity, _stuccoCoastalMat, true);
        CreateBox(root.transform, "Wall_FrontHeader", center + new Vector3(0f, 3.2f, d * 0.5f), new Vector3(2.2f, 1.4f, 0.30f), Quaternion.identity, _stuccoCoastalMat, true);

        CreateBox(root.transform, "Diner_WindowW", center + new Vector3(-3.3f, 1.7f, d * 0.5f + 0.08f), new Vector3(3.0f, 1.6f, 0.08f), Quaternion.identity, _glassMat, true);
        CreateBox(root.transform, "Diner_WindowE", center + new Vector3(3.3f, 1.7f, d * 0.5f + 0.08f), new Vector3(3.0f, 1.6f, 0.08f), Quaternion.identity, _glassMat, true);
        CreateBox(root.transform, "Diner_Roof", center + new Vector3(0f, h + 0.18f, 0.4f), new Vector3(w + 0.8f, 0.35f, d + 1.4f), Quaternion.identity, _roofSlateMat, true);
        CreateBox(root.transform, "Diner_NeonTrim", center + new Vector3(0f, h + 0.22f, d * 0.5f + 1.12f), new Vector3(w + 0.85f, 0.14f, 0.12f), Quaternion.identity, _emissiveWarmMat, false);

        CreateBox(root.transform, "Diner_Bar", center + new Vector3(0f, 0.55f, -1.6f), new Vector3(6.0f, 1.1f, 0.85f), Quaternion.identity, _awningRedMat, true);
        CreateBox(root.transform, "Patio_TableW", center + new Vector3(-3.65f, 0.39f, d * 0.5f + 1.35f), new Vector3(1.4f, 0.78f, 1.0f), Quaternion.identity, _woodMat, true);
        CreateBox(root.transform, "Patio_TableE", center + new Vector3(3.65f, 0.39f, d * 0.5f + 1.35f), new Vector3(1.4f, 0.78f, 1.0f), Quaternion.identity, _woodMat, true);

        // Thick 1.4m flush-exterior anti-tunnel colliders + doorway bollards
        const float wt = 1.40f;
        AddSolidColliderBox(root.transform, "Diner_AntiTunnel_Back", center + new Vector3(0f, h * 0.5f, -d * 0.5f + wt * 0.5f - 0.15f), new Vector3(w + 0.4f, h + 1.4f, wt), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Diner_AntiTunnel_W", center + new Vector3(-w * 0.5f + wt * 0.5f - 0.15f, h * 0.5f, 0f), new Vector3(wt, h + 1.4f, d + 0.4f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Diner_AntiTunnel_E", center + new Vector3(w * 0.5f - wt * 0.5f + 0.15f, h * 0.5f, 0f), new Vector3(wt, h + 1.4f, d + 0.4f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Diner_AntiTunnel_FrontW", center + new Vector3(-3.3f, h * 0.5f, d * 0.5f - wt * 0.5f + 0.15f), new Vector3(4.4f, h + 1.4f, wt), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Diner_AntiTunnel_FrontE", center + new Vector3(3.3f, h * 0.5f, d * 0.5f - wt * 0.5f + 0.15f), new Vector3(4.4f, h + 1.4f, wt), Quaternion.identity);
        CreateBox(root.transform, "Diner_BollardL", new Vector3(x - 0.70f, floorY + 0.48f, frontWallZ + 0.75f), new Vector3(0.24f, 0.96f, 0.24f), Quaternion.identity, _darkMetalMat, true);
        CreateBox(root.transform, "Diner_BollardR", new Vector3(x + 0.70f, floorY + 0.48f, frontWallZ + 0.75f), new Vector3(0.24f, 0.96f, 0.24f), Quaternion.identity, _darkMetalMat, true);

        AddWarmPointLight(root.transform, "Diner_Light", center + new Vector3(0f, h - 0.5f, 0f), 14f, 1.2f);
        BuildingCount++;
    }

    private void BuildHarborGarage(Transform parent, float x, float z)
    {
        float northRoadY = GetBoulevardSurfaceY(x, 424.0f);
        float backTerrainY = NorthIslandTerrainData.SampleHeight(x, z - 4.5f);
        float floorY = Mathf.Max(northRoadY, backTerrainY) + 0.08f;

        var root = new GameObject("Northshore_HarborGarage");
        root.transform.SetParent(parent, false);
        Vector3 center = new(x, floorY, z);
        root.transform.position = center;

        const float w = 14.0f;
        const float d = 9.0f;
        const float h = 4.6f;

        Vector3 pRoad = new(x, northRoadY, 424.0f);
        Vector3 pDoor = new(x, floorY, z + d * 0.5f + 0.30f);
        Vector3 apronMid = (pRoad + pDoor) * 0.5f;
        Quaternion apronRot = Quaternion.LookRotation((pDoor - pRoad).normalized, Vector3.up);
        float apronLen = Vector3.Distance(pRoad, pDoor) + 0.04f;

        float gWallVisH = h + 0.04f;
        float gWallCenterY = h * 0.5f - 0.02f;
        CreateBox(root.transform, "Garage_DriveInApron", apronMid - (apronRot * Vector3.up) * 0.10f, new Vector3(w + 0.6f, 0.20f, apronLen), apronRot, _asphaltMat, true);
        CreateBox(root.transform, "Garage_Floor", center + new Vector3(0f, -0.35f, 0f), new Vector3(w + 0.6f, 0.70f, d + 0.6f), Quaternion.identity, _sidewalkMat, true);
        // Back wall overlaps both side walls so the west bay corner does not show daylight.
        CreateBox(root.transform, "Wall_Back", center + new Vector3(0f, gWallCenterY, -d * 0.5f), new Vector3(w + 0.40f, gWallVisH, 0.40f), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Wall_West", center + new Vector3(-w * 0.5f, gWallCenterY, -0.12f), new Vector3(0.40f, gWallVisH, d + 0.24f), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Wall_East", center + new Vector3(w * 0.5f, gWallCenterY, -0.12f), new Vector3(0.40f, gWallVisH, d + 0.24f), Quaternion.identity, _brickWallMat, true);

        CreateBox(root.transform, "Pillar_West", center + new Vector3(-w * 0.5f + 0.7f, gWallCenterY, d * 0.5f), new Vector3(1.4f, gWallVisH, 0.34f), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Pillar_Center", center + new Vector3(0f, gWallCenterY, d * 0.5f), new Vector3(1.4f, gWallVisH, 0.34f), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Pillar_East", center + new Vector3(w * 0.5f - 0.7f, gWallCenterY, d * 0.5f), new Vector3(1.4f, gWallVisH, 0.34f), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "Wall_Header", center + new Vector3(0f, 3.95f, d * 0.5f), new Vector3(w, 1.3f, 0.34f), Quaternion.identity, _brickWallMat, true);
        CreateBox(root.transform, "RollUpDoor_East", center + new Vector3(3.15f, 1.65f, d * 0.5f), new Vector3(4.9f, 3.3f, 0.16f), Quaternion.identity, _darkMetalMat, true);

        CreateBox(root.transform, "Garage_Roof", center + new Vector3(0f, h + 0.18f, 0f), new Vector3(w + 0.5f, 0.36f, d + 0.5f), Quaternion.identity, _roofSlateMat, true);
        CreateBox(root.transform, "Garage_Workbench", center + new Vector3(-3.0f, 0.50f, -d * 0.5f + 0.9f), new Vector3(4.5f, 1.0f, 0.9f), Quaternion.identity, _awningRedMat, true);

        // Thick 1.4m anti-tunnel colliders on exterior walls + front pillars + closed east bay door block (west bay remains drive-in)
        const float wt = 1.40f;
        AddSolidColliderBox(root.transform, "Garage_AntiTunnel_Back", center + new Vector3(0f, h * 0.5f, -d * 0.5f + wt * 0.5f - 0.17f), new Vector3(w + 0.4f, h + 1.2f, wt), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Garage_AntiTunnel_W", center + new Vector3(-w * 0.5f + wt * 0.5f - 0.17f, h * 0.5f, 0f), new Vector3(wt, h + 1.2f, d + 0.4f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Garage_AntiTunnel_E", center + new Vector3(w * 0.5f - wt * 0.5f + 0.17f, h * 0.5f, 0f), new Vector3(wt, h + 1.2f, d + 0.4f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Garage_AntiTunnel_PillarW", center + new Vector3(-w * 0.5f + 0.7f, h * 0.5f, d * 0.5f - 0.23f), new Vector3(1.4f, h + 1.2f, 0.80f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Garage_AntiTunnel_PillarC", center + new Vector3(0f, h * 0.5f, d * 0.5f - 0.23f), new Vector3(1.4f, h + 1.2f, 0.80f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Garage_AntiTunnel_PillarE", center + new Vector3(w * 0.5f - 0.7f, h * 0.5f, d * 0.5f - 0.23f), new Vector3(1.4f, h + 1.2f, 0.80f), Quaternion.identity);
        AddSolidColliderBox(root.transform, "Garage_ClosedEastBayCollider", center + new Vector3(3.15f, 2.0f, d * 0.5f - 0.90f), new Vector3(5.2f, 4.0f, 1.80f), Quaternion.identity);

        AddWarmPointLight(root.transform, "Garage_Light", center + new Vector3(-3.0f, h - 0.5f, 0f), 15f, 1.3f);
        BuildingCount++;
    }

    private void BuildCoastalHouse(Transform parent, string name, float x, float z, Material? wallMat)
    {
        float northRoadY = GetBoulevardSurfaceY(x, 422.0f);
        float walkTopY = northRoadY + 0.10f;
        float backTerrainY = NorthIslandTerrainData.SampleHeight(x, z - 4.0f);
        float floorY = Mathf.Max(northRoadY, backTerrainY) + 0.15f;

        var root = new GameObject(name);
        root.transform.SetParent(parent, false);
        Vector3 center = new(x, floorY, z);
        root.transform.position = center;

        const float w = NorthIslandGeometry.CoastalHouseBodyWidth;
        const float d = NorthIslandGeometry.CoastalHouseBodyDepth;
        const float h = 6.2f;
        float frontWallZ = z + d * 0.5f; // 418.2
        float stepBottomY = northRoadY - 0.60f;
        float porchH = Mathf.Max(0.35f, floorY - stepBottomY);

        CreateBox(root.transform, "House_Foundation", new Vector3(x, floorY - 0.70f, z), new Vector3(w + 0.8f, 1.40f, d + 0.8f), Quaternion.identity, _sidewalkMat, true);
        CreateBox(root.transform, "House_PorchDeck", new Vector3(x, stepBottomY + porchH * 0.5f, frontWallZ + 1.05f), new Vector3(4.8f, porchH, 2.15f), Quaternion.identity, _sidewalkMat, true);

        for (int s = 0; s < 2; s++)
        {
            float stepTopY = Mathf.Lerp(walkTopY, floorY, (s + 1) / 3f);
            float stepH = Mathf.Max(0.25f, stepTopY - stepBottomY);
            float stepZ = 421.4f - (s + 0.5f) * (1.15f / 2f);
            CreateBox(root.transform, $"House_Step_{s}", new Vector3(x, stepBottomY + stepH * 0.5f, stepZ), new Vector3(2.8f, stepH, 0.64f), Quaternion.identity, _sidewalkMat, true);
        }

        CreateBox(root.transform, "House_Body", center + new Vector3(0f, h * 0.5f, 0f), new Vector3(w, h, d), Quaternion.identity, wallMat, true);
        CreateBox(root.transform, "House_Roof", center + new Vector3(0f, h + 0.25f, 0f), new Vector3(w + 0.8f, 0.50f, d + 0.8f), Quaternion.identity, _roofSlateMat, true);
        AddSolidColliderBox(root.transform, "House_SolidBlockCollider", center + new Vector3(0f, (h - 1.0f) * 0.5f, 0f), new Vector3(w, h + 1.8f, d), Quaternion.identity);

        CreateBox(root.transform, "Porch_Roof", center + new Vector3(0f, 3.1f, d * 0.5f + 1.0f), new Vector3(4.6f, 0.22f, 2.0f), Quaternion.identity, _roofSlateMat, true);
        CreateBox(root.transform, "Porch_ColL", center + new Vector3(-2.0f, 1.55f, d * 0.5f + 1.85f), new Vector3(0.20f, 3.1f, 0.20f), Quaternion.identity, _whiteLineMat, true);
        CreateBox(root.transform, "Porch_ColR", center + new Vector3(2.0f, 1.55f, d * 0.5f + 1.85f), new Vector3(0.20f, 3.1f, 0.20f), Quaternion.identity, _whiteLineMat, true);
        CreateBox(root.transform, "Front_Door", center + new Vector3(0f, 1.15f, d * 0.5f + 0.06f), new Vector3(1.1f, 2.2f, 0.12f), Quaternion.identity, _woodMat, true);

        float[] winXs = { -2.8f, 2.8f };
        foreach (float wx in winXs)
        {
            CreateBox(root.transform, $"Win1_{wx}", center + new Vector3(wx, 1.6f, d * 0.5f + 0.06f), new Vector3(1.6f, 1.3f, 0.08f), Quaternion.identity, _glassMat, false);
            CreateBox(root.transform, $"Win2_{wx}", center + new Vector3(wx, 4.5f, d * 0.5f + 0.06f), new Vector3(1.6f, 1.3f, 0.08f), Quaternion.identity, _glassMat, false);
        }
        CreateBox(root.transform, "Win2_Center", center + new Vector3(0f, 4.5f, d * 0.5f + 0.06f), new Vector3(1.4f, 1.3f, 0.08f), Quaternion.identity, _glassMat, false);

        BuildingCount++;
    }

    // =========================================================================
    // 5. STREETLIGHTS, BENCHES, WELCOME SIGN & REAL CLONED TREES / BUSHES
    // =========================================================================

    private void BuildStreetFurnitureAndNature(Transform parent)
    {
        var propRoot = new GameObject("Town_Props_And_Nature");
        propRoot.transform.SetParent(parent, false);

        // 1. "WELCOME TO NORTHSHORE" landmark sign on the North Boardwalk facing the off-ramp (-18.5, z=436.0)
        float signBaseY = GetBoulevardSurfaceY(-18.5f, 434.0f) + 0.10f;
        CreateBox(propRoot.transform, "WelcomeSign_PostL", new Vector3(-21.2f, signBaseY + 1.4f, 436.0f), new Vector3(0.24f, 2.8f, 0.24f), Quaternion.identity, _woodMat, true);
        CreateBox(propRoot.transform, "WelcomeSign_PostR", new Vector3(-15.8f, signBaseY + 1.4f, 436.0f), new Vector3(0.24f, 2.8f, 0.24f), Quaternion.identity, _woodMat, true);
        CreateBox(propRoot.transform, "WelcomeSign_Board", new Vector3(-18.5f, signBaseY + 2.2f, 436.0f), new Vector3(6.2f, 1.2f, 0.28f), Quaternion.identity, _woodMat, true);
        CreateBox(propRoot.transform, "WelcomeSign_GlowPanel", new Vector3(-18.5f, signBaseY + 2.2f, 435.84f), new Vector3(5.6f, 0.85f, 0.06f), Quaternion.identity, _emissiveSignMat, false);

        // 2. Boulevard Streetlights — strictly on North Boardwalk (z=436.0, facing South) & South Sidewalk (z=421.7, facing North)
        float[] northLampXs = NorthIslandGeometry.NorthLampXs;
        for (int i = 0; i < northLampXs.Length; i++)
        {
            float lx = northLampXs[i];
            float ly = GetBoulevardSurfaceY(lx, 434.0f) + 0.10f;
            BuildStreetlight(propRoot.transform, new Vector3(lx, ly, 436.0f), 180f, i % 2 == 1);
        }

        float[] southLampXs = { -69.0f, -52.6f, -4.0f, 28.0f, 51.0f, 81.5f, 101.0f };
        for (int i = 0; i < southLampXs.Length; i++)
        {
            float lx = southLampXs[i];
            float ly = GetBoulevardSurfaceY(lx, 424.0f) + 0.10f;
            BuildStreetlight(propRoot.transform, new Vector3(lx, ly, 422.4f), 0f, i % 2 == 0);
        }

        // 3. Benches & Trash Cans along the North Waterfront Boardwalk (z = 435.7)
        float[] benchXs = { -50.0f, -36.0f, 2.0f, 22.0f, 42.0f, 62.0f, 82.0f };
        foreach (float bx in benchXs)
        {
            float by = GetBoulevardSurfaceY(bx, 434.0f) + 0.10f;
            BuildBench(propRoot.transform, new Vector3(bx, by, 435.7f), 180f);
            CreateBox(
                propRoot.transform,
                $"TrashCan_{bx:F0}",
                new Vector3(bx + 1.6f, by + 0.38f, 435.7f),
                new Vector3(0.45f, 0.76f, 0.45f),
                Quaternion.identity,
                _darkMetalMat,
                withCollider: true);
        }

        // 4. Real Game Trees (cloned from scene) & Shrubs across the North Island hills & roadside
        ReadOnlySpan<(float x, float z, float scale)> treeCoords = stackalloc (float x, float z, float scale)[]
        {
            (-71f, 403f, 1.15f), (-48f, 396f, 1.05f), (-38f, 402f, 1.25f), (-27f, 404f, 1.10f),
            (-10f, 402f, 1.30f), (-1f, 406f, 1.05f), (27f, 404f, 1.20f), (48f, 402f, 1.15f),
            (54f, 406f, 0.95f), (80f, 404f, 1.25f), (103f, 406f, 1.10f), (110f, 420f, 1.20f),
            (-76f, 420f, 1.15f), (-84f, 385f, 1.35f), (-94f, 372f, 1.20f), (-106f, 360f, 1.30f),
            (-115f, 346f, 1.15f), (-115f, 326f, 1.25f), (-96f, 313f, 1.20f), (-78f, 313f, 1.10f),
            (-58f, 355f, 1.25f), (-42f, 368f, 1.30f), (-30f, 382f, 1.15f), (-6f, 378f, 1.25f),
            (12f, 382f, 1.35f), (28f, 376f, 1.15f), (46f, 380f, 1.25f), (64f, 378f, 1.30f),
            (84f, 382f, 1.20f), (98f, 386f, 1.10f)
        };

        for (int i = 0; i < treeCoords.Length; i++)
        {
            var tc = treeCoords[i];
            float ty = NorthIslandTerrainData.SampleHeight(tc.x, tc.z);
            if (ty < -1.0f) continue;
            BuildTree(propRoot.transform, new Vector3(tc.x, ty, tc.z), tc.scale, i);
        }

        // Roadside & storefront shrubs (cloned from game SGS_Shrub_LOD0 if available, placed in open yard/hillside gaps)
        if (_shrubTemplate != null)
        {
            ReadOnlySpan<(float x, float z)> shrubCoords = stackalloc (float x, float z)[]
            {
                (-73f, 419.5f), (-58.2f, 419.5f), (-40.6f, 419.5f), (30.4f, 419.5f),
                (53f, 419.5f), (83f, 419.5f), (102f, 419.5f),
                (-74f, 316.5f), (-108f, 316.5f), (-108f, 345.0f)
            };
            for (int i = 0; i < shrubCoords.Length; i++)
            {
                var sc = shrubCoords[i];
                float sy = NorthIslandTerrainData.SampleHeight(sc.x, sc.z);
                BuildShrub(propRoot.transform, new Vector3(sc.x, sy, sc.z), i);
            }
        }
    }

    private void BuildStreetlight(Transform parent, Vector3 basePos, float yaw, bool withRealtimeLight)
    {
        var go = new GameObject($"Streetlight_{StreetlightCount}");
        go.transform.SetParent(parent, false);
        go.transform.position = basePos;
        go.transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        Vector3 fwd = go.transform.forward;

        CreateBox(go.transform, "Pole", basePos + new Vector3(0f, 2.8f, 0f), new Vector3(0.16f, 5.6f, 0.16f), go.transform.rotation, _darkMetalMat, false);
        AddSolidColliderBox(go.transform, "Pole_Collider", basePos + new Vector3(0f, 2.8f, 0f), new Vector3(0.42f, 5.6f, 0.42f), go.transform.rotation);
        CreateBox(go.transform, "Arm", basePos + new Vector3(0f, 5.55f, 0f) + fwd * 0.65f, new Vector3(0.10f, 0.10f, 1.4f), go.transform.rotation, _darkMetalMat, false);
        CreateBox(go.transform, "Bulb", basePos + new Vector3(0f, 5.46f, 0f) + fwd * 1.20f, new Vector3(0.32f, 0.12f, 0.45f), go.transform.rotation, _emissiveWarmMat, false);

        if (withRealtimeLight)
        {
            AddWarmPointLight(go.transform, "LampLight", basePos + new Vector3(0f, 5.2f, 0f) + fwd * 1.20f, 22f, 1.15f);
        }

        StreetlightCount++;
    }

    private void BuildBench(Transform parent, Vector3 pos, float yaw)
    {
        var b = new GameObject("Bench");
        b.transform.SetParent(parent, false);
        Quaternion rot = Quaternion.Euler(0f, yaw, 0f);
        Vector3 fwd = rot * Vector3.forward;

        CreateBox(b.transform, "Seat", pos + new Vector3(0f, 0.45f, 0f), new Vector3(1.8f, 0.08f, 0.55f), rot, _woodMat, true);
        CreateBox(b.transform, "Back", pos + new Vector3(0f, 0.80f, 0f) - fwd * 0.24f, new Vector3(1.8f, 0.55f, 0.08f), rot, _woodMat, true);
        CreateBox(b.transform, "LegL", pos + new Vector3(-0.75f, 0.22f, 0f), new Vector3(0.10f, 0.45f, 0.50f), rot, _darkMetalMat, true);
        CreateBox(b.transform, "LegR", pos + new Vector3(0.75f, 0.22f, 0f), new Vector3(0.10f, 0.45f, 0.50f), rot, _darkMetalMat, true);
    }

    private static Vector3 SanitizeLossyScale(Vector3 s)
    {
        float x = MathF.Abs(s.x);
        float y = MathF.Abs(s.y);
        float z = MathF.Abs(s.z);
        return new Vector3(
            x > 0.001f ? x : 1.0f,
            y > 0.001f ? y : 1.0f,
            z > 0.001f ? z : 1.0f);
    }

    private static bool IsInstancedWorldObject(Transform? tr, string goName)
    {
        if (NorthIslandGeometry.IsInstancedProxyName(goName))
            return true;

        Transform? cur = tr;
        int depth = 0;
        while (cur != null && depth++ < 6)
        {
            if (NorthIslandGeometry.IsInstancedProxyName(cur.name))
                return true;
            cur = cur.parent;
        }

        return false;
    }

    private static Bounds GetAccurateLocalBounds(Mesh mesh)
    {
        if (mesh != null && mesh.isReadable)
        {
            var verts = mesh.vertices;
            if (verts != null && verts.Length > 0)
            {
                Vector3 min = verts[0];
                Vector3 max = verts[0];
                for (int i = 1; i < verts.Length; i++)
                {
                    Vector3 v = verts[i];
                    if (v.x < min.x) min.x = v.x;
                    if (v.y < min.y) min.y = v.y;
                    if (v.z < min.z) min.z = v.z;
                    if (v.x > max.x) max.x = v.x;
                    if (v.y > max.y) max.y = v.y;
                    if (v.z > max.z) max.z = v.z;
                }

                var b = new Bounds();
                b.SetMinMax(min, max);
                return b;
            }
        }

        return mesh!.bounds;
    }

    private static void ComputeWorldBoundsY(
        Bounds localBounds,
        Vector3 worldScale,
        Quaternion worldRot,
        out float worldMinY,
        out float worldHeight)
    {
        Vector3 c = localBounds.center;
        Vector3 e = localBounds.extents;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        for (int sx = -1; sx <= 1; sx += 2)
        {
            for (int sy = -1; sy <= 1; sy += 2)
            {
                for (int sz = -1; sz <= 1; sz += 2)
                {
                    Vector3 corner = new(c.x + sx * e.x, c.y + sy * e.y, c.z + sz * e.z);
                    Vector3 rotated = worldRot * Vector3.Scale(corner, worldScale);
                    if (rotated.y < minY) minY = rotated.y;
                    if (rotated.y > maxY) maxY = rotated.y;
                }
            }
        }

        worldMinY = minY;
        worldHeight = maxY - minY;
    }

    private static void ComputeTrunkPlacement(
        MeshFilter mf,
        Mesh mesh,
        Bounds mb,
        Vector3 baseScale,
        Quaternion baseRot,
        float worldMinY,
        out float trunkX,
        out float trunkZ,
        out float trunkRadius)
    {
        var cap = mf.GetComponent<CapsuleCollider>()
                  ?? (mf.transform.parent != null ? mf.transform.parent.GetComponent<CapsuleCollider>() : null);
        if (cap != null)
        {
            Vector3 worldDiff = cap.transform.TransformPoint(cap.center) - mf.transform.position;
            Vector3 capScale = SanitizeLossyScale(cap.transform.lossyScale);
            float scaleXZ = MathF.Max(capScale.x, capScale.z);
            trunkX = worldDiff.x;
            trunkZ = worldDiff.z;
            trunkRadius = Mathf.Max(0.45f, cap.radius * scaleXZ);
            return;
        }

        if (mesh.isReadable)
        {
            try
            {
                var verts = mesh.vertices;
                if (verts != null && verts.Length > 0)
                {
                    float cutoffY = worldMinY + 2.2f;
                    double sx = 0;
                    double sz = 0;
                    int count = 0;
                    for (int i = 0; i < verts.Length; i++)
                    {
                        Vector3 wv = baseRot * Vector3.Scale(verts[i], baseScale);
                        if (wv.y <= cutoffY)
                        {
                            sx += wv.x;
                            sz += wv.z;
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        trunkX = (float)(sx / count);
                        trunkZ = (float)(sz / count);
                        trunkRadius = 0.60f;
                        return;
                    }
                }
            }
            catch
            {
                // Ignore non-readable mesh edge cases and fall back to transformed bounds center.
            }
        }

        Vector3 scaledCenter = baseRot * Vector3.Scale(mb.center, baseScale);
        trunkX = scaledCenter.x;
        trunkZ = scaledCenter.z;
        trunkRadius = 0.60f;
    }

    private void BuildTree(Transform parent, Vector3 basePos, float scale, int idx)
    {
        if (_treeTemplates.Count > 0)
        {
            MeshTemplate tpl = _treeTemplates[idx % _treeTemplates.Count];
            var go = new GameObject($"Northshore_Tree_{idx}");
            go.transform.SetParent(parent, false);
            // Embed trunk base firmly 0.35m into sloped terrain so trees never float
            Vector3 treePos = basePos - new Vector3(0f, 0.35f, 0f);
            Quaternion treeRot = Quaternion.Euler(0f, (idx * 53f) % 360f, 0f);
            go.transform.position = treePos;
            go.transform.rotation = treeRot;
            go.transform.localScale = Vector3.one * scale;

            var visual = new GameObject("TreeMesh");
            visual.transform.SetParent(go.transform, false);
            visual.transform.localPosition = new Vector3(0f, -tpl.PivotBottomOffsetY, 0f);
            visual.transform.localRotation = tpl.BaseRotation;
            visual.transform.localScale = tpl.BaseScale;

            var mf = visual.AddComponent<MeshFilter>();
            mf.sharedMesh = tpl.Mesh;
            var mr = visual.AddComponent<MeshRenderer>();
            mr.sharedMaterials = tpl.Materials;
            mr.lightmapIndex = -1;
            mr.lightProbeUsage = LightProbeUsage.BlendProbes;
            mr.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
            mr.shadowCastingMode = ShadowCastingMode.On;
            mr.receiveShadows = true;

            // Dedicated un-scaled trunk collider holder aligned exactly to the trunk axis in world space
            Vector3 trunkWorldPos = treePos + treeRot * new Vector3(
                tpl.TrunkOffsetX * scale,
                0f,
                tpl.TrunkOffsetZ * scale);

            var colGo = new GameObject($"Northshore_Tree_{idx}_TrunkCol");
            colGo.transform.SetParent(parent, false);
            colGo.transform.position = trunkWorldPos;
            colGo.transform.rotation = Quaternion.identity;
            colGo.transform.localScale = Vector3.one;

            float radius = Mathf.Max(0.45f, tpl.TrunkRadius) * scale;
            var cap = colGo.AddComponent<CapsuleCollider>();
            cap.center = new Vector3(0f, 2.8f, 0f);
            cap.radius = radius;
            cap.height = 7.0f;
            cap.enabled = true;
            cap.isTrigger = false;

            // Anti-tunnel BoxCollider alongside CapsuleCollider so fast vehicles always stop cleanly at the trunk
            float boxWidth = Mathf.Max(0.95f, radius * 1.75f);
            var trunkBox = colGo.AddComponent<BoxCollider>();
            trunkBox.center = new Vector3(0f, 2.8f, 0f);
            trunkBox.size = new Vector3(boxWidth, 6.2f, boxWidth);
            trunkBox.enabled = true;
            trunkBox.isTrigger = false;

            TreeCount++;
            return;
        }

        // Fallback procedural pine if no scene template matched
        var t = new GameObject($"PineTree_{idx}");
        t.transform.SetParent(parent, false);
        t.transform.position = basePos;

        CreateBox(t.transform, "Trunk", basePos + new Vector3(0f, 1.6f * scale, 0f), new Vector3(0.45f * scale, 3.6f * scale, 0.45f * scale), Quaternion.identity, _treeTrunkMat, true);
        CreateBox(t.transform, "Crown_Low", basePos + new Vector3(0f, 3.2f * scale, 0f), new Vector3(2.8f * scale, 1.8f * scale, 2.8f * scale), Quaternion.Euler(0f, 20f, 0f), _treeLeavesMat, false);
        CreateBox(t.transform, "Crown_Mid", basePos + new Vector3(0f, 4.7f * scale, 0f), new Vector3(2.1f * scale, 1.7f * scale, 2.1f * scale), Quaternion.Euler(0f, 65f, 0f), _treeLeavesMat, false);
        CreateBox(t.transform, "Crown_Top", basePos + new Vector3(0f, 6.0f * scale, 0f), new Vector3(1.2f * scale, 1.5f * scale, 1.2f * scale), Quaternion.Euler(0f, 15f, 0f), _treeLeavesMat, false);

        TreeCount++;
    }

    private void BuildShrub(Transform parent, Vector3 basePos, int idx)
    {
        if (_shrubTemplate == null) return;
        var go = new GameObject($"Northshore_Shrub_{idx}");
        go.transform.SetParent(parent, false);
        go.transform.position = basePos - new Vector3(0f, 0.08f, 0f);
        go.transform.rotation = Quaternion.Euler(0f, (idx * 71f) % 360f, 0f);
        go.transform.localScale = Vector3.one;

        var visual = new GameObject("ShrubMesh");
        visual.transform.SetParent(go.transform, false);
        visual.transform.localPosition = new Vector3(0f, -_shrubTemplate.PivotBottomOffsetY, 0f);
        visual.transform.localRotation = _shrubTemplate.BaseRotation;
        visual.transform.localScale = _shrubTemplate.BaseScale;

        var mf = visual.AddComponent<MeshFilter>();
        mf.sharedMesh = _shrubTemplate.Mesh;
        var mr = visual.AddComponent<MeshRenderer>();
        mr.sharedMaterials = _shrubTemplate.Materials;
        mr.lightmapIndex = -1;
        mr.lightProbeUsage = LightProbeUsage.BlendProbes;

        // Solid central stem collider so vehicles and players cannot ghost cleanly through bushes
        AddSolidColliderBox(
            parent,
            $"Northshore_Shrub_{idx}_StemCol",
            basePos + new Vector3(0f, 0.38f, 0f),
            new Vector3(0.55f, 0.76f, 0.55f),
            Quaternion.identity);
    }

    private static void AddWarmPointLight(Transform parent, string name, Vector3 pos, float range, float intensity)
    {
        var lgo = new GameObject(name);
        lgo.transform.SetParent(parent, false);
        lgo.transform.position = pos;
        var light = lgo.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1.0f, 0.90f, 0.72f);
        light.range = range;
        light.intensity = intensity;
        light.shadows = LightShadows.None;
    }

    internal static GameObject CreateBox(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        Material? mat,
        bool withCollider)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = scale;

        // Strip CreatePrimitive's scaled BoxCollider and create a dedicated un-scaled BoxCollider
        // matching BridgeBuilder's proven OverpassBridge_DeckCollider setup.
        var primBox = go.GetComponent<BoxCollider>();
        if (primBox != null)
        {
            UnityEngine.Object.Destroy(primBox);
        }
        else
        {
            var primCol = go.GetComponent<Collider>();
            if (primCol != null) UnityEngine.Object.Destroy(primCol);
        }

        if (withCollider)
        {
            AddSolidColliderBox(parent, $"{name}_Collider", position, scale, rotation);
        }

        if (mat != null)
        {
            var rend = go.GetComponent<MeshRenderer>();
            if (rend != null) rend.sharedMaterial = mat;
        }

        return go;
    }

    private void CreateAsphaltQuadVisual(
        Transform parent,
        string name,
        Vector3 v0,
        Vector3 v1,
        Vector3 v2,
        Vector3 v3)
    {
        Vector3 offset = new(0f, -0.004f, 0f);
        var mesh = new Mesh { name = name + "_Mesh" };
        mesh.vertices = new[] { v0 + offset, v1 + offset, v2 + offset, v3 + offset };
        int[] tris = { 0, 2, 1, 1, 2, 3 };
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        if (mesh.normals.Length > 0 && mesh.normals[0].y < 0f)
        {
            mesh.triangles = new[] { 0, 1, 2, 1, 3, 2 };
            mesh.RecalculateNormals();
        }
        mesh.RecalculateBounds();
        _createdMeshes.Add(mesh);

        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = mesh;
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = _asphaltMat;
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = true;
    }

    internal static BoxCollider AddSolidColliderBox(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 size,
        Quaternion rotation)
    {
        var colGo = new GameObject(name);
        colGo.transform.SetParent(parent, false);
        colGo.transform.position = position;
        colGo.transform.rotation = rotation;
        colGo.transform.localScale = Vector3.one;

        var box = colGo.AddComponent<BoxCollider>();
        box.center = Vector3.zero;
        box.size = size;
        box.enabled = true;
        box.isTrigger = false;
        return box;
    }

    private void EnsureMaterials()
    {
        if (_asphaltMat != null) return;
        Shader shader = Shader.Find("Universal Render Pipeline/Lit")
                     ?? Shader.Find("Standard")
                     ?? Shader.Find("Diffuse")
                     ?? Shader.Find("Unlit/Color");

        Shader unlit = Shader.Find("Universal Render Pipeline/Unlit")
                    ?? Shader.Find("Unlit/Color")
                    ?? shader;

        _asphaltMat = CreateMat(shader, "NI_TownAsphalt", new Color(0.17f, 0.18f, 0.20f), 0.12f);
        _sidewalkMat = CreateMat(shader, "NI_TownSidewalk", new Color(0.64f, 0.64f, 0.61f), 0.10f);
        _curbMat = CreateMat(shader, "NI_TownCurb", new Color(0.52f, 0.52f, 0.50f), 0.08f);
        _whiteLineMat = CreateMat(shader, "NI_TownWhiteLine", new Color(0.90f, 0.91f, 0.92f), 0.15f);
        _yellowLineMat = CreateMat(shader, "NI_TownYellowLine", new Color(0.92f, 0.74f, 0.18f), 0.15f);
        _brickWallMat = CreateMat(shader, "NI_TownBrick", new Color(0.54f, 0.27f, 0.22f), 0.08f);
        _stuccoWarmMat = CreateMat(shader, "NI_TownStuccoWarm", new Color(0.84f, 0.78f, 0.66f), 0.10f);
        _stuccoCoastalMat = CreateMat(shader, "NI_TownStuccoBlue", new Color(0.62f, 0.73f, 0.79f), 0.12f);
        _woodMat = CreateMat(shader, "NI_TownWood", new Color(0.45f, 0.31f, 0.19f), 0.12f);
        _darkMetalMat = CreateMat(shader, "NI_TownDarkMetal", new Color(0.16f, 0.18f, 0.20f), 0.35f);
        _roofSlateMat = CreateMat(shader, "NI_TownRoof", new Color(0.24f, 0.26f, 0.29f), 0.15f);
        _glassMat = CreateMat(shader, "NI_TownGlass", new Color(0.22f, 0.36f, 0.46f), 0.75f);
        _awningRedMat = CreateMat(shader, "NI_TownAwningRed", new Color(0.68f, 0.18f, 0.18f), 0.15f);
        _treeTrunkMat = CreateMat(shader, "NI_TreeTrunk", new Color(0.32f, 0.22f, 0.14f), 0.05f);
        _treeLeavesMat = CreateMat(shader, "NI_TreeLeaves", new Color(0.14f, 0.34f, 0.18f), 0.08f);

        _emissiveWarmMat = CreateMat(unlit, "NI_GlowWarm", new Color(1.0f, 0.92f, 0.70f), 0.5f);
        _emissiveSignMat = CreateMat(unlit, "NI_GlowSign", new Color(0.95f, 0.85f, 0.55f), 0.5f);
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

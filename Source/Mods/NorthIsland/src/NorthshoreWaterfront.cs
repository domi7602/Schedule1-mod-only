using System;
using System.Collections.Generic;
using System.IO;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Map;
using MelonLoader;
using MelonLoader.Utils;
using S1MAPI.Gltf;
using UnityEngine;
using UnityEngine.Rendering;

namespace NorthIsland;

/// <summary>
/// "Northshore Waterfront" decor layer (Phase 3, package B): a walkable wooden pier entering
/// through the coast-guardrail gap at x = 14..20 and running north to a wider head platform,
/// a moored rowboat, and a lighthouse on a stone islet in open water northwest of the pier.
/// Elevations use the visual sea level (heightmap coastline, y = 0). The native ocean object's
/// transform sits below that surface and is logged only. A geometric night beam rotates on the
/// lamp room; an unreadable clock leaves the beam off. Accepted Phase 1 geometry is never
/// reshaped here (the guardrail gap lives in TownBuilder).
/// </summary>
internal sealed class NorthshoreWaterfront
{
    private readonly List<Material> _materials = new();
    private Material? _pierWoodMat;
    private Material? _pilingWoodMat;
    private Material? _stoneMat;
    private Material? _towerWhiteMat;
    private Material? _towerRedMat;
    private Material? _darkMetalMat;
    private Material? _lampGlowMat;
    private Material? _glassMat;
    private Material? _boatHullMat;

    private const string BeamFileName = "lighthouse-beam.glb";

    private GameObject? _beamPivot;
    private GameObject? _beamRoot;
    private Texture2D? _beamTexture;
    private bool _clockFaulted;
    private float _clockCheckIn;

    internal bool IsBuilt { get; private set; }
    internal float DeckTopY { get; private set; }

    internal void Build(Transform townRoot)
    {
        IsBuilt = false;
        _beamPivot = null;
        _beamRoot = null;
        _clockFaulted = false;
        _clockCheckIn = 0f;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            float waterY = NorthIslandGeometry.VisualSeaLevelY;
            float deckTop = NorthIslandGeometry.GetPierDeckTopY(waterY);
            float walkTop = NorthIslandGeometry.GetPierWalkTopY();
            DeckTopY = deckTop;

            EnsureMaterials();

            var pierRoot = new GameObject("Northshore_Pier");
            pierRoot.transform.SetParent(townRoot, false);
            BuildPier(pierRoot.transform, deckTop, walkTop);
            BuildRowboat(pierRoot.transform, waterY);

            var lightRoot = new GameObject("Northshore_Lighthouse");
            lightRoot.transform.SetParent(townRoot, false);
            BuildBreakwaterAndLighthouse(lightRoot.transform, waterY);

            Physics.SyncTransforms();
            IsBuilt = true;
            MelonLogger.Msg(
                $"[NorthIsland:Waterfront] Pier & Leuchtturm aufgebaut in {watch.ElapsedMilliseconds} ms. {DescribeOcean()}; " +
                $"See={waterY:F2}, DeckTop={deckTop:F2} (Boardwalk {walkTop:F2}), " +
                $"Leuchtturm=({NorthIslandGeometry.LighthouseX:F0}, {NorthIslandGeometry.GetLighthouseBaseY(waterY):F2}, {NorthIslandGeometry.LighthouseZ:F0}).");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[NorthIsland:Waterfront] Aufbau fehlgeschlagen (Basistadt bleibt intakt): {ex}");
        }
    }

    internal void Tick(float deltaTime)
    {
        if (!IsBuilt || _beamPivot == null || _beamRoot == null || deltaTime < 0f)
            return;

        _clockCheckIn -= deltaTime;
        if (_clockCheckIn <= 0f && !_clockFaulted)
        {
            _clockCheckIn = 1f;
            try
            {
                var time = NetworkSingleton<TimeManager>.Instance;
                if (time != null && NorthIslandGeometry.TryIsLighthouseNight(time.CurrentTime, out bool night))
                    _beamRoot.SetActive(night);
            }
            catch (Exception ex)
            {
                _clockFaulted = true;
                _beamRoot.SetActive(false);
                MelonLogger.Warning($"[NorthIsland:Waterfront] Uhr unlesbar, Lichtstrahl bleibt aus: {ex.Message}");
            }
        }

        if (_beamRoot.activeSelf)
        {
            _beamPivot.transform.Rotate(
                0f,
                360f / NorthIslandGeometry.LighthouseRevolutionSeconds * deltaTime,
                0f,
                Space.World);
        }
    }

    internal void DisposeAssets()
    {
        foreach (var mat in _materials)
        {
            if (mat != null) UnityEngine.Object.Destroy(mat);
        }
        _materials.Clear();
        _pierWoodMat = _pilingWoodMat = _stoneMat = _towerWhiteMat = _towerRedMat =
            _darkMetalMat = _lampGlowMat = _glassMat = _boatHullMat = null;
        _beamPivot = null;
        _beamRoot = null;
        if (_beamTexture != null)
        {
            UnityEngine.Object.Destroy(_beamTexture);
            _beamTexture = null;
        }
        IsBuilt = false;
    }

    /// <summary>
    /// Diagnostic only. The ocean object origin is not the visible waterline; elevations use
    /// <see cref="NorthIslandGeometry.VisualSeaLevelY"/>.
    /// </summary>
    private static string DescribeOcean()
    {
        try
        {
            var map = Map.instance;
            var ocean = map != null ? map.transform.Find("StylizedWater2_Ocean") : null;
            if (ocean == null)
                return "Ozean-Objekt nicht gefunden (Seehöhe aus Küstenlinie)";
            var renderer = ocean.GetComponent<MeshRenderer>();
            if (renderer == null)
                return $"Ozean position.y={ocean.position.y:F2} (kein Renderer, unbenutzt)";
            Bounds b = renderer.bounds;
            return $"Ozean unbenutzt: position.y={ocean.position.y:F2}, bounds.max.y={b.max.y:F2}, size.y={b.size.y:F2}";
        }
        catch (Exception ex)
        {
            return $"Ozean-Readback fehlgeschlagen ({ex.Message})";
        }
    }

    // =========================================================================
    // PIER (deck, pilings, rails, head platform, entrance threshold & bollards)
    // =========================================================================

    private void BuildPier(Transform root, float deckTop, float walkTop)
    {
        float minX = NorthIslandGeometry.PierMinX;
        float maxX = NorthIslandGeometry.PierMaxX;
        float cx = NorthIslandGeometry.PierCenterX;
        float startZ = NorthIslandGeometry.PierStartZ;
        float endZ = NorthIslandGeometry.PierEndZ;
        float width = maxX - minX;
        float len = endZ - startZ;
        float midZ = (startZ + endZ) * 0.5f;

        // Entrance threshold: gentle ramp from boardwalk top up to the deck (or flush plate).
        float rise = deckTop - walkTop;
        if (rise > 0.02f)
        {
            Vector3 pBot = new(cx, walkTop, 435.9f);
            Vector3 pTop = new(cx, deckTop, 438.1f);
            Vector3 mid = (pBot + pTop) * 0.5f;
            Quaternion rot = Quaternion.LookRotation((pTop - pBot).normalized, Vector3.up);
            TownBuilder.CreateBox(root, "Pier_ThresholdRamp",
                mid - (rot * Vector3.up) * 0.09f,
                new Vector3(width, 0.18f, Vector3.Distance(pBot, pTop) + 0.15f),
                rot, _pierWoodMat, true);
        }
        else
        {
            TownBuilder.CreateBox(root, "Pier_ThresholdPlate",
                new Vector3(cx, deckTop - 0.09f, 436.3f),
                new Vector3(width, 0.18f, 1.4f), Quaternion.identity, _pierWoodMat, true);
        }

        // One continuous deck collider; visual planks on top (no per-plank colliders).
        TownBuilder.AddSolidColliderBox(root, "Pier_DeckCollider",
            new Vector3(cx, deckTop - 0.15f, midZ), new Vector3(width, 0.30f, len), Quaternion.identity);
        for (float z = startZ + 0.5f; z < endZ - 0.2f; z += 1.0f)
        {
            TownBuilder.CreateBox(root, $"Pier_Plank_{z:F0}",
                new Vector3(cx, deckTop - 0.06f, z),
                new Vector3(width - 0.04f, 0.12f, 0.92f), Quaternion.identity, _pierWoodMat, false);
        }
        foreach (float sx in new[] { minX + 0.7f, maxX - 0.7f })
        {
            TownBuilder.CreateBox(root, $"Pier_Stringer_{sx:F1}",
                new Vector3(sx, deckTop - 0.33f, midZ),
                new Vector3(0.24f, 0.34f, len), Quaternion.identity, _pilingWoodMat, false);
        }

        // Pilings down to the sampled seabed (0.5m embedment), both sides every 5m.
        for (float z = 438f; z <= endZ; z += 5f)
        {
            foreach (float px in new[] { minX + 0.3f, maxX - 0.3f })
            {
                float seabed = NorthIslandTerrainData.SampleHeight(px, z) - 0.5f;
                float pTop = deckTop - 0.2f;
                float h = pTop - seabed;
                if (h <= 0.3f) continue;
                CreateCylinder(root, $"Pier_Piling_{px:F0}_{z:F0}",
                    new Vector3(px, seabed + h * 0.5f, z), 0.16f, h, _pilingWoodMat, false);
            }
        }

        // Side rails on the main deck: posts + collidable top rail + visual mid rail.
        float railH = NorthIslandGeometry.PierRailHeight;
        foreach (float rx in new[] { minX + 0.12f, maxX - 0.12f })
        {
            for (float z = startZ + 0.9f; z < endZ - 0.4f; z += 2.4f)
            {
                TownBuilder.CreateBox(root, $"Pier_RailPost_{rx:F1}_{z:F0}",
                    new Vector3(rx, deckTop + railH * 0.5f, z),
                    new Vector3(0.09f, railH, 0.09f), Quaternion.identity, _pilingWoodMat, false);
            }
            TownBuilder.CreateBox(root, $"Pier_TopRail_{rx:F1}",
                new Vector3(rx, deckTop + railH, midZ),
                new Vector3(0.10f, 0.12f, len), Quaternion.identity, _pierWoodMat, true);
            TownBuilder.CreateBox(root, $"Pier_MidRail_{rx:F1}",
                new Vector3(rx, deckTop + railH * 0.55f, midZ),
                new Vector3(0.07f, 0.06f, len), Quaternion.identity, _pierWoodMat, false);
        }

        BuildPierHead(root, deckTop);

        // Gate posts with lanterns at the guardrail gap edges; car-blocking bollard pair
        // leaves a 1.16m clear walking gap (same pattern as the storefront bollards).
        foreach (float gx in new[] { minX, maxX })
        {
            TownBuilder.CreateBox(root, $"Pier_GatePost_{gx:F0}",
                new Vector3(gx, walkTop + 0.58f, 436.5f),
                new Vector3(0.14f, 1.16f, 0.14f), Quaternion.identity, _pilingWoodMat, true);
            TownBuilder.CreateBox(root, $"Pier_GateLantern_{gx:F0}",
                new Vector3(gx, walkTop + 1.26f, 436.5f),
                new Vector3(0.18f, 0.16f, 0.18f), Quaternion.identity, _lampGlowMat, false);
        }
        foreach (float bx in new[] { cx - 0.7f, cx + 0.7f })
        {
            TownBuilder.CreateBox(root, $"Pier_EntryBollard_{bx:F1}",
                new Vector3(bx, deckTop + 0.45f, 438.4f),
                new Vector3(0.24f, 0.90f, 0.24f), Quaternion.identity, _darkMetalMat, true);
        }
    }

    private void BuildPierHead(Transform root, float deckTop)
    {
        float minX = NorthIslandGeometry.PierHeadMinX;
        float maxX = NorthIslandGeometry.PierHeadMaxX;
        float maxZ = NorthIslandGeometry.PierHeadMaxZ;
        float startZ = NorthIslandGeometry.PierEndZ;
        float cx = (minX + maxX) * 0.5f;
        float width = maxX - minX;
        float len = maxZ - startZ;
        float midZ = (startZ + maxZ) * 0.5f;
        float railH = NorthIslandGeometry.PierRailHeight;

        TownBuilder.AddSolidColliderBox(root, "PierHead_DeckCollider",
            new Vector3(cx, deckTop - 0.15f, midZ), new Vector3(width, 0.30f, len), Quaternion.identity);
        for (float z = startZ + 0.4f; z < maxZ - 0.2f; z += 1.0f)
        {
            TownBuilder.CreateBox(root, $"PierHead_Plank_{z:F0}",
                new Vector3(cx, deckTop - 0.06f, z),
                new Vector3(width - 0.04f, 0.12f, 0.92f), Quaternion.identity, _pierWoodMat, false);
        }
        foreach (float px in new[] { minX + 0.4f, maxX - 0.4f })
        {
            foreach (float pz in new[] { startZ + 0.4f, maxZ - 0.4f })
            {
                float seabed = NorthIslandTerrainData.SampleHeight(px, pz) - 0.5f;
                float h = deckTop - 0.2f - seabed;
                if (h <= 0.3f) continue;
                CreateCylinder(root, $"PierHead_Piling_{px:F0}_{pz:F0}",
                    new Vector3(px, seabed + h * 0.5f, pz), 0.18f, h, _pilingWoodMat, false);
            }
        }

        // Perimeter rails (west, east, north) with collidable top rails.
        foreach (float rx in new[] { minX + 0.12f, maxX - 0.12f })
        {
            TownBuilder.CreateBox(root, $"PierHead_TopRail_{rx:F1}",
                new Vector3(rx, deckTop + railH, midZ),
                new Vector3(0.10f, 0.12f, len), Quaternion.identity, _pierWoodMat, true);
            foreach (float pz in new[] { startZ + 0.3f, midZ, maxZ - 0.3f })
            {
                TownBuilder.CreateBox(root, $"PierHead_RailPost_{rx:F1}_{pz:F0}",
                    new Vector3(rx, deckTop + railH * 0.5f, pz),
                    new Vector3(0.09f, railH, 0.09f), Quaternion.identity, _pilingWoodMat, false);
            }
        }
        TownBuilder.CreateBox(root, "PierHead_TopRail_North",
            new Vector3(cx, deckTop + railH, maxZ - 0.12f),
            new Vector3(width, 0.12f, 0.10f), Quaternion.identity, _pierWoodMat, true);

        // Bench facing the open water, lamp post with ONE realtime light, mooring bollards.
        Vector3 benchPos = new(cx, deckTop, maxZ - 1.2f);
        TownBuilder.CreateBox(root, "PierHead_BenchSeat", benchPos + new Vector3(0f, 0.45f, 0f), new Vector3(1.8f, 0.08f, 0.55f), Quaternion.identity, _pierWoodMat, true);
        TownBuilder.CreateBox(root, "PierHead_BenchBack", benchPos + new Vector3(0f, 0.80f, -0.24f), new Vector3(1.8f, 0.55f, 0.08f), Quaternion.identity, _pierWoodMat, true);
        TownBuilder.CreateBox(root, "PierHead_BenchLegL", benchPos + new Vector3(-0.75f, 0.22f, 0f), new Vector3(0.10f, 0.45f, 0.50f), Quaternion.identity, _darkMetalMat, true);
        TownBuilder.CreateBox(root, "PierHead_BenchLegR", benchPos + new Vector3(0.75f, 0.22f, 0f), new Vector3(0.10f, 0.45f, 0.50f), Quaternion.identity, _darkMetalMat, true);

        Vector3 lampPos = new(minX + 0.9f, deckTop, maxZ - 0.9f);
        CreateCylinder(root, "PierHead_LampPole", lampPos + new Vector3(0f, 1.5f, 0f), 0.06f, 3.0f, _darkMetalMat, false);
        TownBuilder.CreateBox(root, "PierHead_LampBulb", lampPos + new Vector3(0f, 3.08f, 0f), new Vector3(0.30f, 0.22f, 0.30f), Quaternion.identity, _lampGlowMat, false);
        AddPointLight(root, "PierHead_LampLight", lampPos + new Vector3(0f, 2.9f, 0f), 20f, 1.2f);

        foreach (float bx in new[] { minX + 0.5f, maxX - 0.5f })
        {
            CreateCylinder(root, $"PierHead_Bollard_{bx:F1}",
                new Vector3(bx, deckTop + 0.25f, maxZ - 0.5f), 0.12f, 0.5f, _darkMetalMat, false);
        }
    }

    // =========================================================================
    // ROWBOAT (static, moored east of the head platform; no collider — open water)
    // =========================================================================

    private void BuildRowboat(Transform root, float waterY)
    {
        var boat = new GameObject("Northshore_Rowboat");
        boat.transform.SetParent(root, false);
        boat.transform.position = new Vector3(NorthIslandGeometry.PierHeadMaxX + 1.3f, waterY + 0.10f, 473.8f);
        boat.transform.rotation = Quaternion.Euler(0f, 8f, 0f);
        Vector3 p = boat.transform.position;

        TownBuilder.CreateBox(boat.transform, "Hull", p + new Vector3(0f, 0.18f, 0f), new Vector3(1.2f, 0.40f, 3.2f), boat.transform.rotation, _boatHullMat, false);
        TownBuilder.CreateBox(boat.transform, "GunwaleW", p + new Vector3(-0.58f, 0.44f, 0f), new Vector3(0.12f, 0.16f, 3.2f), boat.transform.rotation, _pilingWoodMat, false);
        TownBuilder.CreateBox(boat.transform, "GunwaleE", p + new Vector3(0.58f, 0.44f, 0f), new Vector3(0.12f, 0.16f, 3.2f), boat.transform.rotation, _pilingWoodMat, false);
        TownBuilder.CreateBox(boat.transform, "TransomBow", p + new Vector3(0f, 0.44f, 1.56f), new Vector3(1.2f, 0.16f, 0.12f), boat.transform.rotation, _pilingWoodMat, false);
        TownBuilder.CreateBox(boat.transform, "TransomStern", p + new Vector3(0f, 0.44f, -1.56f), new Vector3(1.2f, 0.16f, 0.12f), boat.transform.rotation, _pilingWoodMat, false);
        TownBuilder.CreateBox(boat.transform, "Floor", p + new Vector3(0f, 0.34f, 0f), new Vector3(1.0f, 0.06f, 2.9f), boat.transform.rotation, _pierWoodMat, false);
        TownBuilder.CreateBox(boat.transform, "SeatFore", p + new Vector3(0f, 0.50f, 0.7f), new Vector3(1.0f, 0.08f, 0.35f), boat.transform.rotation, _pierWoodMat, false);
        TownBuilder.CreateBox(boat.transform, "SeatAft", p + new Vector3(0f, 0.50f, -0.7f), new Vector3(1.0f, 0.08f, 0.35f), boat.transform.rotation, _pierWoodMat, false);
    }

    // =========================================================================
    // BREAKWATER MOUND & LIGHTHOUSE (open water northwest of the pier)
    // =========================================================================

    private void BuildBreakwaterAndLighthouse(Transform root, float waterY)
    {
        float cx = NorthIslandGeometry.LighthouseX;
        float cz = NorthIslandGeometry.LighthouseZ;
        float baseY = NorthIslandGeometry.GetLighthouseBaseY(waterY);

        // Stone mound: three rotated courses, top flush with baseY; one solid collider.
        TownBuilder.CreateBox(root, "Mound_Base", new Vector3(cx, baseY - 1.75f, cz), new Vector3(7.4f, 1.8f, 7.4f), Quaternion.Euler(0f, 0f, 0f), _stoneMat, false);
        TownBuilder.CreateBox(root, "Mound_Mid", new Vector3(cx, baseY - 0.95f, cz), new Vector3(6.0f, 1.4f, 6.0f), Quaternion.Euler(0f, 25f, 0f), _stoneMat, false);
        TownBuilder.CreateBox(root, "Mound_Top", new Vector3(cx, baseY - 0.5f, cz), new Vector3(4.6f, 1.0f, 4.6f), Quaternion.Euler(0f, 50f, 0f), _stoneMat, false);
        TownBuilder.AddSolidColliderBox(root, "Mound_Collider", new Vector3(cx, baseY - 1.5f, cz), new Vector3(6.5f, 3.5f, 6.5f), Quaternion.identity);

        // Tapered white tower with two red bands.
        CreateCylinder(root, "Tower_Seg1", new Vector3(cx, baseY + 1.3f, cz), 1.45f, 2.6f, _towerWhiteMat, false);
        CreateCylinder(root, "Tower_Band1", new Vector3(cx, baseY + 2.85f, cz), 1.47f, 0.5f, _towerRedMat, false);
        CreateCylinder(root, "Tower_Seg2", new Vector3(cx, baseY + 4.3f, cz), 1.25f, 2.4f, _towerWhiteMat, false);
        CreateCylinder(root, "Tower_Band2", new Vector3(cx, baseY + 5.75f, cz), 1.27f, 0.5f, _towerRedMat, false);
        CreateCylinder(root, "Tower_Seg3", new Vector3(cx, baseY + 7.1f, cz), 1.05f, 2.2f, _towerWhiteMat, false);
        TownBuilder.AddSolidColliderBox(root, "Tower_Collider", new Vector3(cx, baseY + 4.1f, cz), new Vector3(2.9f, 8.2f, 2.9f), Quaternion.identity);

        // Gallery deck + railing ring.
        CreateCylinder(root, "Gallery_Deck", new Vector3(cx, baseY + 8.31f, cz), 1.75f, 0.22f, _darkMetalMat, false);
        for (int k = 0; k < 8; k++)
        {
            float ang = k * 45f * Mathf.Deg2Rad;
            float px = cx + Mathf.Cos(ang) * 1.6f;
            float pz = cz + Mathf.Sin(ang) * 1.6f;
            TownBuilder.CreateBox(root, $"Gallery_Post_{k}",
                new Vector3(px, baseY + 8.70f, pz),
                new Vector3(0.07f, 0.55f, 0.07f), Quaternion.identity, _darkMetalMat, false);
            float tangent = ang + Mathf.PI * 0.5f;
            TownBuilder.CreateBox(root, $"Gallery_Rail_{k}",
                new Vector3(cx + Mathf.Cos(ang + Mathf.PI / 8f) * 1.6f, baseY + 8.98f, cz + Mathf.Sin(ang + Mathf.PI / 8f) * 1.6f),
                new Vector3(1.25f, 0.07f, 0.07f), Quaternion.Euler(0f, -tangent * Mathf.Rad2Deg, 0f), _darkMetalMat, false);
        }

        // Lamp room is itself unlit. The old opaque glass cube hid the glow core completely.
        float lampY = baseY + 9.05f;
        TownBuilder.CreateBox(root, "LampRoom_Glow", new Vector3(cx, lampY, cz), new Vector3(1.35f, 1.15f, 1.35f), Quaternion.identity, _lampGlowMat, false);
        for (int k = 0; k < 4; k++)
        {
            float ang = (k * 90f + 45f) * Mathf.Deg2Rad;
            TownBuilder.CreateBox(root, $"LampRoom_Post_{k}",
                new Vector3(cx + Mathf.Cos(ang) * 0.78f, lampY, cz + Mathf.Sin(ang) * 0.78f),
                new Vector3(0.08f, 1.25f, 0.08f), Quaternion.identity, _darkMetalMat, false);
        }
        AddPointLight(root, "Lighthouse_Lamp", new Vector3(cx, lampY, cz), 28f, 3.5f);
        BuildNightBeam(root, new Vector3(cx, lampY, cz));

        // Roof cap + spire.
        CreateCylinder(root, "LampRoom_Roof", new Vector3(cx, baseY + 9.72f, cz), 1.85f, 0.16f, _darkMetalMat, false);
        CreateCylinder(root, "Roof_Cap", new Vector3(cx, baseY + 10.0f, cz), 0.7f, 0.5f, _towerRedMat, false);
        CreateCylinder(root, "Spire", new Vector3(cx, baseY + 10.5f, cz), 0.06f, 0.7f, _darkMetalMat, false);
    }

    /// <summary>
    /// Visible night ray. A spotlight alone does not draw a cone in open air, so the ray is
    /// unlit geometry. It stays inactive until the clock confirms night.
    /// </summary>
    private void BuildNightBeam(Transform root, Vector3 lampPos)
    {
        _beamPivot = new GameObject("Lighthouse_BeamPivot");
        _beamPivot.transform.SetParent(root, false);
        _beamPivot.transform.position = lampPos;

        _beamRoot = new GameObject("Lighthouse_Beam");
        _beamRoot.transform.SetParent(_beamPivot.transform, false);
        _beamRoot.SetActive(false);

        if (!TryAttachSoftBeam(_beamRoot.transform))
        {
            const float length = 58f;
            var tilt = Quaternion.Euler(7f, 0f, 0f);
            Vector3 center = lampPos + tilt * new Vector3(0f, -0.4f, length * 0.5f);
            TownBuilder.CreateBox(_beamRoot.transform, "Beam_Ray", center, new Vector3(2.4f, 0.55f, length), tilt, _lampGlowMat, false);
            TownBuilder.CreateBox(_beamRoot.transform, "Beam_Core", center, new Vector3(0.7f, 0.18f, length), tilt, _lampGlowMat, false);
            MelonLogger.Warning("[NorthIsland:Waterfront] Weicher Lichtkegel nicht geladen, Latte bleibt als Ersatz.");
        }

        var spotGo = new GameObject("Lighthouse_BeamSpot");
        spotGo.transform.SetParent(_beamRoot.transform, false);
        spotGo.transform.localRotation = Quaternion.Euler(7f, 0f, 0f);
        var spot = spotGo.AddComponent<Light>();
        spot.type = LightType.Spot;
        spot.color = new Color(1.0f, 0.95f, 0.82f);
        spot.range = 70f;
        spot.intensity = 6f;
        spot.spotAngle = 16f;
        spot.shadows = LightShadows.None;
    }

    /// <summary>
    /// Loads the Blender cone (three crossed, tapered cards). The visible softness comes from
    /// an additive shader the game already has; a missing file or shader keeps the box beam.
    /// </summary>
    private bool TryAttachSoftBeam(Transform beamRoot)
    {
        string? path = ResolveBeamPath();
        if (path == null)
        {
            MelonLogger.Warning("[NorthIsland:Waterfront] lighthouse-beam.glb nicht gefunden.");
            return false;
        }

        Shader? shader = FindAdditiveShader();
        if (shader == null)
        {
            MelonLogger.Warning("[NorthIsland:Waterfront] Kein additives Shader fuer den Lichtkegel.");
            return false;
        }

        GameObject? model;
        try
        {
            model = GltfLoader.LoadFromFile(path, shader);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[NorthIsland:Waterfront] Lichtkegel-Import fehlgeschlagen: {ex.Message}");
            return false;
        }

        if (model == null)
            return false;

        model.name = "LighthouseBeam";
        model.transform.SetParent(beamRoot, false);
        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = Vector3.one;
        model.transform.localRotation = Quaternion.Euler(7f, 180f, 0f);

        foreach (var collider in model.GetComponentsInChildren<Collider>(true))
        {
            if (collider != null)
                UnityEngine.Object.Destroy(collider);
        }

        Texture2D gradient = CreateBeamGradient();
        Material beamMat = CreateAdditiveMaterial(shader, gradient);
        int renderers = 0;
        foreach (var renderer in model.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer == null)
                continue;
            var imported = renderer.sharedMaterial;
            renderer.sharedMaterial = beamMat;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            if (imported != null && imported != beamMat)
                UnityEngine.Object.Destroy(imported);
            renderers++;
        }

        if (renderers == 0)
        {
            UnityEngine.Object.Destroy(model);
            UnityEngine.Object.Destroy(beamMat);
            UnityEngine.Object.Destroy(gradient);
            _beamTexture = null;
            MelonLogger.Warning("[NorthIsland:Waterfront] Lichtkegel ohne Renderer.");
            return false;
        }

        var probe = model.GetComponentInChildren<Renderer>();
        if (probe != null && probe.bounds.size.sqrMagnitude > 1f)
        {
            Vector3 localCenter = beamRoot.InverseTransformPoint(probe.bounds.center);
            if (localCenter.z < 0f)
                model.transform.localRotation = Quaternion.Euler(7f, 0f, 0f);
        }

        MelonLogger.Msg($"[NorthIsland:Waterfront] Weicher Lichtkegel geladen: {path}, shader={shader.name}, renderer={renderers}.");
        return true;
    }

    private static string? ResolveBeamPath()
    {
        string[] candidates =
        {
            Path.Combine(MelonEnvironment.ModsDirectory, "NorthIsland", BeamFileName),
            Path.Combine(MelonEnvironment.UserDataDirectory, "NorthIsland", "assets", BeamFileName),
            Path.Combine(MelonEnvironment.ModsDirectory, "NorthIsland", "assets", BeamFileName),
        };
        foreach (string candidate in candidates)
        {
            if (File.Exists(candidate))
                return candidate;
        }

        return null;
    }

    private static Shader? FindAdditiveShader()
    {
        string[] names =
        {
            "Universal Render Pipeline/Particles/Unlit",
            "Universal Render Pipeline/Unlit",
            "Particles/Additive",
            "Legacy Shaders/Particles/Additive",
            "Mobile/Particles/Additive",
        };
        foreach (string name in names)
        {
            Shader shader = Shader.Find(name);
            if (shader != null)
                return shader;
        }

        return null;
    }

    private Material CreateAdditiveMaterial(Shader shader, Texture2D gradient)
    {
        var mat = new Material(shader) { name = "NI_LighthouseBeam" };
        _materials.Add(mat);
        if (mat.HasProperty("_BaseMap"))
            mat.SetTexture("_BaseMap", gradient);
        if (mat.HasProperty("_MainTex"))
            mat.SetTexture("_MainTex", gradient);
        var color = new Color(1f, 0.93f, 0.72f, 0.85f);
        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color"))
            mat.SetColor("_Color", color);
        if (mat.HasProperty("_Surface"))
            mat.SetFloat("_Surface", 1f);
        if (mat.HasProperty("_Blend"))
            mat.SetFloat("_Blend", 2f);
        if (mat.HasProperty("_ZWrite"))
            mat.SetFloat("_ZWrite", 0f);
        if (mat.HasProperty("_Cull"))
            mat.SetFloat("_Cull", 0f);
        if (mat.HasProperty("_SrcBlend"))
            mat.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
        if (mat.HasProperty("_DstBlend"))
            mat.SetFloat("_DstBlend", (float)BlendMode.One);
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.EnableKeyword("_BLENDMODE_ADD");
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.renderQueue = 3000;
        return mat;
    }

    private Texture2D CreateBeamGradient()
    {
        const int width = 128;
        const int height = 64;
        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false)
        {
            name = "NI_LighthouseBeamGradient",
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear,
        };
        for (int y = 0; y < height; y++)
        {
            float across = MathF.Abs((y + 0.5f) / height - 0.5f) * 2f;
            float edge = Mathf.SmoothStep(1f, 0f, across);
            for (int x = 0; x < width; x++)
            {
                float along = (x + 0.5f) / width;
                float fade = Mathf.SmoothStep(1f, 0f, along);
                fade *= fade;
                tex.SetPixel(x, y, new Color(1f, 0.93f, 0.72f, fade * edge));
            }
        }

        tex.Apply(false, false);
        _beamTexture = tex;
        return tex;
    }

    // =========================================================================
    // HELPERS
    // =========================================================================

    /// <summary>
    /// Unscaled cylinder visual (Unity primitive is radius 0.5 / height 2). The primitive's
    /// own collider is stripped; colliders are dedicated unscaled boxes like TownBuilder's.
    /// The shared primitive mesh is borrowed and never destroyed.
    /// </summary>
    private static GameObject CreateCylinder(Transform parent, string name, Vector3 pos, float radius, float height, Material? mat, bool withCollider)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = pos;
        go.transform.localScale = new Vector3(radius * 2f, height * 0.5f, radius * 2f);

        var primCol = go.GetComponent<Collider>();
        if (primCol != null) UnityEngine.Object.Destroy(primCol);
        if (withCollider)
            TownBuilder.AddSolidColliderBox(parent, $"{name}_Collider", pos, new Vector3(radius * 2f, height, radius * 2f), Quaternion.identity);

        if (mat != null)
        {
            var rend = go.GetComponent<MeshRenderer>();
            if (rend != null) rend.sharedMaterial = mat;
        }
        return go;
    }

    private static void AddPointLight(Transform parent, string name, Vector3 pos, float range, float intensity)
    {
        var lgo = new GameObject(name);
        lgo.transform.SetParent(parent, false);
        lgo.transform.position = pos;
        var light = lgo.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1.0f, 0.93f, 0.78f);
        light.range = range;
        light.intensity = intensity;
        light.shadows = LightShadows.None;
    }

    private void EnsureMaterials()
    {
        if (_pierWoodMat != null) return;

        Shader lit = Shader.Find("Universal Render Pipeline/Lit")
                     ?? Shader.Find("Standard")
                     ?? Shader.Find("Diffuse")
                     ?? Shader.Find("Unlit/Color");
        Shader unlit = Shader.Find("Universal Render Pipeline/Unlit")
                       ?? Shader.Find("Unlit/Color")
                       ?? lit;

        _pierWoodMat = MakeMat(lit, "NI_PierWood", new Color(0.42f, 0.30f, 0.20f), 0.10f);
        _pilingWoodMat = MakeMat(lit, "NI_PilingWood", new Color(0.30f, 0.22f, 0.15f), 0.06f);
        _stoneMat = MakeMat(lit, "NI_BreakwaterStone", new Color(0.45f, 0.46f, 0.47f), 0.05f);
        _towerWhiteMat = MakeMat(lit, "NI_TowerWhite", new Color(0.92f, 0.92f, 0.90f), 0.15f);
        _towerRedMat = MakeMat(lit, "NI_TowerRed", new Color(0.75f, 0.18f, 0.15f), 0.15f);
        _darkMetalMat = MakeMat(lit, "NI_WaterfrontMetal", new Color(0.16f, 0.18f, 0.20f), 0.35f);
        _glassMat = MakeMat(lit, "NI_LampGlass", new Color(0.25f, 0.40f, 0.50f), 0.80f);
        _boatHullMat = MakeMat(lit, "NI_BoatHull", new Color(0.35f, 0.25f, 0.18f), 0.12f);
        _lampGlowMat = MakeMat(unlit, "NI_LampGlow", new Color(1.0f, 0.95f, 0.80f), 0.5f);
    }

    private Material MakeMat(Shader shader, string name, Color color, float smoothness)
    {
        var m = new Material(shader) { name = name, color = color };
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
        if (m.HasProperty("_Color")) m.SetColor("_Color", color);
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smoothness);
        _materials.Add(m);
        return m;
    }
}

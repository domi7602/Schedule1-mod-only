extern alias il2cpp;

using System;
using System.Collections.Generic;
using Il2Cpp;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.Core.Weather;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Weather;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace DowntownWarehouse;

/// <summary>
/// Builds an inward-facing interior shell, doorway reveal, skirting, steel roof beams,
/// and overhead fluorescent lights for the native hardware warehouse.
/// Native Main and Roof meshes are single-sided outward shells and invisible from inside.
/// </summary>
internal static class HallInterior
{
    private const float OuterWest = HallPlan.CenterX - HallPlan.WidthX * 0.5f;   // 108.75
    private const float OuterEast = HallPlan.CenterX + HallPlan.WidthX * 0.5f;   // 122.75
    private const float OuterSouth = HallPlan.CenterZ - HallPlan.LengthZ * 0.5f; // 16.50
    private const float OuterNorth = HallPlan.CenterZ + HallPlan.LengthZ * 0.5f; // 34.50
    private const float FloorY = HallPlan.FloorTopY - 0.02f;                     // 0.08
    private const float EaveY = 6.10f;
    private const float RidgeY = HallPlan.CenterY + HallPlan.HeightY * 0.5f;     // 8.10

    private const float WallInset = 0.08f;
    private const float SkirtInset = 0.115f;
    private const float SkirtTopY = 0.90f;
    private const float DoorMinZ = 21.86f;
    private const float DoorMaxZ = 23.14f;
    private const float DoorTopY = 2.32f;
    private const float JambOuterX = OuterWest - 0.01f;
    private const float WallUvScaleY = 0.085f;
    private const float WallUvScaleH = 0.053f;
    private const float SkirtUvScale = 0.13f;

    private static readonly List<Mesh> OwnedMeshes = new();
    private static readonly List<Material> OwnedMaterials = new();
    private static readonly List<Surface> OwnedSurfaces = new();
    private static GameObject? _root;
    private static GameObject? _surfacesRoot;
    private static GameObject? _savePointGo;
    private static SavePoint? _savePoint;
    private static WeatherEnclosure? _weatherEnclosure;
    private static Il2CppScheduleOne.Property.Property? _surfacesProperty;

    internal static void Prepare(GameObject hall, Il2CppScheduleOne.Property.Property? property = null)
    {
        Reset();
        var root = new GameObject("DowntownWarehouse_Interior");
        root.SetActive(false);
        _root = root;

        try
        {
            var hardwareStore = hall.transform.parent;
            var wallMat = CreateOwnedLitMaterial(new Color(0.75f, 0.77f, 0.79f), 0.02f, 0.18f);
            var ceilingMat = CreateOwnedLitMaterial(new Color(0.64f, 0.67f, 0.70f), 0.05f, 0.20f);
            var skirtMat = FindMaterial(hardwareStore, "Skirting") ?? wallMat;
            var trimMat = FindMaterial(hardwareStore, "DoorFrame (1)/Cube")
                ?? CreateOwnedLitMaterial(new Color(0.14f, 0.16f, 0.19f), 0.1f, 0.3f);
            var bulbMat = CreateOwnedUnlitMaterial(new Color(1.0f, 0.97f, 0.90f));
            var intercomFaceMat = CreateOwnedLitMaterial(new Color(0.78f, 0.80f, 0.82f), 0.1f, 0.35f);
            var saveLedMat = CreateOwnedUnlitMaterial(new Color(0.20f, 0.95f, 0.35f));

            int layer = hall.layer;
            BuildWallsAndCeiling(root.transform, layer, wallMat, ceilingMat);
            BuildDoorJamb(root.transform, layer, trimMat);
            BuildSkirting(root.transform, layer, skirtMat);
            BuildBeamsAndLights(root.transform, layer, trimMat, bulbMat);
            BuildSavePoint(root.transform, layer, trimMat, intercomFaceMat, saveLedMat);
            BuildWeatherEnclosure(root.transform);
            if (property != null)
                BuildWallSurfaces(root.transform, property, layer);
        }
        catch
        {
            Reset();
            throw;
        }
    }

    internal static void Apply()
    {
        if (_root != null)
            _root.SetActive(true);
        RegisterWeatherEnclosure();
        ActivateWallSurfaces();
    }

    internal static void SetSaveVisible(bool owned)
    {
        if (NetworkGuard.IsAlive(_savePointGo))
            _savePointGo!.SetActive(owned);
    }

    internal static void Reset()
    {
        UnregisterWeatherEnclosure();
        _weatherEnclosure = null;
        ResetWallSurfaces();
        DetachSavePointListeners();
        _savePoint = null;
        _savePointGo = null;
        if (NetworkGuard.IsAlive(_root))
        {
            try { _root!.SetActive(false); } catch { }
            try { UnityEngine.Object.Destroy(_root!); } catch { }
        }
        _root = null;
        foreach (var mesh in OwnedMeshes)
        {
            if (NetworkGuard.IsAlive(mesh))
            {
                try { UnityEngine.Object.Destroy(mesh); } catch { }
            }
        }
        foreach (var mat in OwnedMaterials)
        {
            if (NetworkGuard.IsAlive(mat))
            {
                try { UnityEngine.Object.Destroy(mat); } catch { }
            }
        }
        OwnedMeshes.Clear();
        OwnedMaterials.Clear();
    }

    private static void BuildWeatherEnclosure(Transform parent)
    {
        var go = new GameObject("DowntownWarehouse_WeatherEnclosure");
        go.transform.SetParent(parent, false);
        go.transform.position = new Vector3(HallPlan.WeatherCenterX, HallPlan.WeatherCenterY, HallPlan.WeatherCenterZ);
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        var basic = go.AddComponent<BasicEnclosure>();
        basic._center = Vector3.zero;
        basic._size = new Vector3(HallPlan.WeatherSizeX, HallPlan.WeatherSizeY, HallPlan.WeatherSizeZ);
        basic._isBlendZone = false;
        basic._collisionLayers = Physics.DefaultRaycastLayers;
        basic._backRadius = 0f;
        basic._frontRadius = 0f;
        basic._openingObj = null;

        var enclosure = go.AddComponent<WeatherEnclosure>();
        enclosure._enclosures = new il2cpp::Il2CppSystem.Collections.Generic.List<BasicEnclosure>();
        enclosure._enclosures.Add(basic);
        enclosure._stereoPanLimit = 0.5f;
        enclosure._blendChangeSpeed = 5f;
        _weatherEnclosure = enclosure;
    }

    private static void RegisterWeatherEnclosure()
    {
        if (!NetworkGuard.IsAlive(_weatherEnclosure))
            return;
        try
        {
            var env = NetworkSingleton<EnvironmentManager>.Instance;
            if (!NetworkGuard.IsAlive(env))
                return;
            if (ContainsPointer(env!._weatherEnclosures, _weatherEnclosure!))
                return;
            try
            {
                env!.OnEnclosureRegistered(_weatherEnclosure!);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("[DowntownWarehouse] WeatherEnclosure OnEnclosureRegistered: " + ex.Message);
            }

            if (env._weatherEnclosures == null)
                env._weatherEnclosures = new il2cpp::Il2CppSystem.Collections.Generic.List<WeatherEnclosure>();
            if (!ContainsPointer(env._weatherEnclosures, _weatherEnclosure!))
                env._weatherEnclosures.Add(_weatherEnclosure);

            var basic = _weatherEnclosure!.GetComponent<BasicEnclosure>();
            bool inside = basic != null && basic.WithinEnclosure(new Vector3(HallPlan.InteriorSpawnX, HallPlan.FloorTopY + 1.4f, HallPlan.InteriorSpawnZ));
            bool outside = basic != null && basic.WithinEnclosure(new Vector3(HallPlan.SpawnX, HallPlan.FloorTopY + 1.4f, HallPlan.SpawnZ));
            MelonLogger.Msg($"[DowntownWarehouse] WeatherEnclosure bereit (Enclosures={env._weatherEnclosures.Count}, Innen={inside}, Aussen={outside}).");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] WeatherEnclosure Registrierung: " + ex.Message);
        }
    }

    private static void UnregisterWeatherEnclosure()
    {
        if (!NetworkGuard.IsAlive(_weatherEnclosure))
            return;
        try
        {
            var env = NetworkSingleton<EnvironmentManager>.Instance;
            var list = env?._weatherEnclosures;
            if (list == null)
                return;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var entry = list[i];
                if (entry != null && entry.Pointer == _weatherEnclosure!.Pointer)
                    list.RemoveAt(i);
            }
        }
        catch { /* best-effort teardown */ }
    }

    private static bool ContainsPointer(il2cpp::Il2CppSystem.Collections.Generic.List<WeatherEnclosure>? list, WeatherEnclosure target)
    {
        if (list == null)
            return false;
        for (int i = 0; i < list.Count; i++)
        {
            var entry = list[i];
            if (entry != null && entry.Pointer == target.Pointer)
                return true;
        }
        return false;
    }

    private static void BuildWallSurfaces(Transform parent, Il2CppScheduleOne.Property.Property property, int fallbackLayer)
    {
        int surfaceLayer = FindDonorSurfaceLayer(fallbackLayer);
        var root = new GameObject("DowntownWarehouse_Surfaces");
        root.SetActive(false);
        root.transform.SetParent(parent, false);
        _surfacesRoot = root;
        _surfacesProperty = property;
        var surfaceOwner = root.AddComponent<Il2CppScheduleOne.Property.PropertyContentsContainer>();
        surfaceOwner._Property_k__BackingField = property;

        foreach (var spec in HallPlan.WallSurfaces())
        {
            var go = new GameObject("DowntownWarehouse_Surface_" + spec.Name);
            go.transform.SetParent(root.transform, false);
            go.layer = surfaceLayer;
            go.transform.position = new Vector3(
                spec.CenterX - spec.NormalX * 0.005f,
                spec.CenterY,
                spec.CenterZ - spec.NormalZ * 0.005f);
            go.transform.rotation = Quaternion.LookRotation(new Vector3(spec.NormalX, 0f, spec.NormalZ), Vector3.up);

            var box = go.AddComponent<BoxCollider>();
            box.isTrigger = false;
            box.center = Vector3.zero;
            box.size = new Vector3(spec.Width, spec.Height, 0.01f);

            var surface = go.AddComponent<Surface>();
            surface.SurfaceType = Surface.ESurfaceType.Wall;
            surface.ValidFaces = new il2cpp::Il2CppSystem.Collections.Generic.List<Surface.EFace>();
            surface.ValidFaces.Add(Surface.EFace.Front);
            surface.BakedGUID = spec.Guid;
            surface._GUID_k__BackingField = new il2cpp::Il2CppSystem.Guid(spec.Guid);
            surface._ParentProperty_k__BackingField = property;
            OwnedSurfaces.Add(surface);
        }
    }

    private static void ActivateWallSurfaces()
    {
        if (!NetworkGuard.IsAlive(_surfacesRoot))
            return;
        _surfacesRoot!.SetActive(true);
        int activeCount = 0;
        int guidCount = 0;
        foreach (var surface in OwnedSurfaces)
        {
            if (!NetworkGuard.IsAlive(surface))
                continue;
            try
            {
                if (_surfacesProperty != null)
                    surface!._ParentProperty_k__BackingField = _surfacesProperty;
                var guid = new il2cpp::Il2CppSystem.Guid(surface!.BakedGUID);
                if (GUIDManager.GetObject<Surface>(guid) == null)
                    surface.SetGUID(guid);
                if (surface.gameObject.activeInHierarchy)
                    activeCount++;
                if (GUIDManager.GetObject<Surface>(guid) != null)
                    guidCount++;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("[DowntownWarehouse] WallSurface GUID: " + ex.Message);
            }
        }
        MelonLogger.Msg($"[DowntownWarehouse] Wand-Surfaces bereit: {activeCount}/{OwnedSurfaces.Count} aktiv, {guidCount}/{OwnedSurfaces.Count} GUID.");
    }

    private static void ResetWallSurfaces()
    {
        foreach (var surface in OwnedSurfaces)
        {
            if (!NetworkGuard.IsAlive(surface))
                continue;
            try
            {
                var reg = surface!.TryCast<IGUIDRegisterable>();
                if (reg != null)
                    GUIDManager.DeregisterObject(reg);
            }
            catch { }
        }
        OwnedSurfaces.Clear();
        _surfacesProperty = null;
        if (NetworkGuard.IsAlive(_surfacesRoot))
        {
            try { _surfacesRoot!.SetActive(false); } catch { }
            try { UnityEngine.Object.Destroy(_surfacesRoot!); } catch { }
        }
        _surfacesRoot = null;
    }

    private static int FindDonorSurfaceLayer(int fallbackLayer)
    {
        try
        {
            foreach (var surface in UnityEngine.Object.FindObjectsOfType<Surface>())
            {
                if (surface == null || surface.SurfaceType != Surface.ESurfaceType.Wall)
                    continue;
                if (surface.gameObject.name.StartsWith("DowntownWarehouse", StringComparison.Ordinal))
                    continue;
                return surface.gameObject.layer;
            }
        }
        catch { /* fallback below */ }
        return fallbackLayer;
    }

    private static void DetachSavePointListeners()
    {
        var sp = _savePoint;
        if (!NetworkGuard.IsAlive(sp))
            return;
        try
        {
            var manager = PersistentSingleton<SaveManager>.Instance;
            if (!NetworkGuard.IsAlive(manager))
                return;
            manager!.onSaveStart?.RemoveListener((UnityAction)sp!.OnSaveStart);
            manager.onSaveComplete?.RemoveListener((UnityAction)sp!.OnSaveComplete);
        }
        catch { /* best-effort listener cleanup during teardown */ }
    }

    private static void BuildWallsAndCeiling(Transform parent, int layer, Material? wallMat, Material? ceilingMat)
    {
        const float w = OuterWest + WallInset;
        const float e = OuterEast - WallInset;
        const float s = OuterSouth + WallInset;
        const float n = OuterNorth - WallInset;
        const float eave = EaveY - WallInset;
        const float ridge = RidgeY - WallInset;
        const float cz = HallPlan.CenterZ;

        var walls = new MeshBuilder(WallUvScaleY, WallUvScaleH);

        // West wall (+X normal) with doorway cut and gable.
        walls.AddQuad(new(w, FloorY, s), new(w, FloorY, DoorMinZ), new(w, eave, DoorMinZ), new(w, eave, s), Vector3.right);
        walls.AddQuad(new(w, FloorY, DoorMaxZ), new(w, FloorY, n), new(w, eave, n), new(w, eave, DoorMaxZ), Vector3.right);
        walls.AddQuad(new(w, DoorTopY, DoorMinZ), new(w, DoorTopY, DoorMaxZ), new(w, eave, DoorMaxZ), new(w, eave, DoorMinZ), Vector3.right);
        walls.AddTri(new(w, eave, s), new(w, eave, n), new(w, ridge, cz), Vector3.right);

        // East wall (-X normal) + gable.
        walls.AddQuad(new(e, FloorY, s), new(e, eave, s), new(e, eave, n), new(e, FloorY, n), Vector3.left);
        walls.AddTri(new(e, eave, s), new(e, ridge, cz), new(e, eave, n), Vector3.left);

        // South wall (+Z normal) and North wall (-Z normal).
        walls.AddQuad(new(w, FloorY, s), new(w, eave, s), new(e, eave, s), new(e, FloorY, s), Vector3.forward);
        walls.AddQuad(new(w, FloorY, n), new(e, FloorY, n), new(e, eave, n), new(w, eave, n), Vector3.back);

        CreateMeshPart(parent, "Interior_Walls", walls.Build("DowntownWarehouse_InteriorWalls"), layer, wallMat, ShadowCastingMode.TwoSided);

        // Pitched ceiling slopes facing inward/downward.
        var ceiling = new MeshBuilder(WallUvScaleH, WallUvScaleH);
        var southCeilingNormal = new Vector3(0f, -(cz - s), ridge - eave).normalized;
        var northCeilingNormal = new Vector3(0f, -(n - cz), -(ridge - eave)).normalized;
        ceiling.AddQuad(new(w, eave, s), new(w, ridge, cz), new(e, ridge, cz), new(e, eave, s), southCeilingNormal);
        ceiling.AddQuad(new(w, ridge, cz), new(w, eave, n), new(e, eave, n), new(e, ridge, cz), northCeilingNormal);

        CreateMeshPart(parent, "Interior_Ceiling", ceiling.Build("DowntownWarehouse_InteriorCeiling"), layer, ceilingMat, ShadowCastingMode.TwoSided);
    }

    private static void BuildDoorJamb(Transform parent, int layer, Material? mat)
    {
        const float w = OuterWest + WallInset;
        var mb = new MeshBuilder(0.5f, 0.5f);
        mb.AddQuad(new(JambOuterX, FloorY, DoorMinZ), new(w, FloorY, DoorMinZ), new(w, DoorTopY, DoorMinZ), new(JambOuterX, DoorTopY, DoorMinZ), Vector3.forward);
        mb.AddQuad(new(JambOuterX, FloorY, DoorMaxZ), new(JambOuterX, DoorTopY, DoorMaxZ), new(w, DoorTopY, DoorMaxZ), new(w, FloorY, DoorMaxZ), Vector3.back);
        mb.AddQuad(new(JambOuterX, DoorTopY, DoorMinZ), new(w, DoorTopY, DoorMinZ), new(w, DoorTopY, DoorMaxZ), new(JambOuterX, DoorTopY, DoorMaxZ), Vector3.down);
        CreateMeshPart(parent, "Interior_DoorJamb", mb.Build("DowntownWarehouse_DoorJamb"), layer, mat, ShadowCastingMode.On);
    }

    private static void BuildSkirting(Transform parent, int layer, Material? mat)
    {
        const float wWall = OuterWest + WallInset;
        const float eWall = OuterEast - WallInset;
        const float sWall = OuterSouth + WallInset;
        const float nWall = OuterNorth - WallInset;

        const float w = OuterWest + SkirtInset;
        const float e = OuterEast - SkirtInset;
        const float s = OuterSouth + SkirtInset;
        const float n = OuterNorth - SkirtInset;

        var mb = new MeshBuilder(SkirtUvScale, SkirtUvScale);

        void Strip(Vector3 p0, Vector3 p1, Vector3 wall0, Vector3 wall1, Vector3 inward)
        {
            Vector3 up = new(0f, SkirtTopY - FloorY, 0f);
            mb.AddQuad(p0, p1, p1 + up, p0 + up, inward);
            mb.AddQuad(p0 + up, p1 + up, wall1 + up, wall0 + up, Vector3.up);
        }

        // West skirting split around the doorway, with end caps at DoorMinZ / DoorMaxZ.
        Strip(new(w, FloorY, s), new(w, FloorY, DoorMinZ), new(wWall, FloorY, sWall), new(wWall, FloorY, DoorMinZ), Vector3.right);
        Strip(new(w, FloorY, DoorMaxZ), new(w, FloorY, n), new(wWall, FloorY, DoorMaxZ), new(wWall, FloorY, nWall), Vector3.right);
        mb.AddQuad(new(wWall, FloorY, DoorMinZ), new(w, FloorY, DoorMinZ), new(w, SkirtTopY, DoorMinZ), new(wWall, SkirtTopY, DoorMinZ), Vector3.forward);
        mb.AddQuad(new(wWall, FloorY, DoorMaxZ), new(wWall, SkirtTopY, DoorMaxZ), new(w, SkirtTopY, DoorMaxZ), new(w, FloorY, DoorMaxZ), Vector3.back);

        // East, South, North skirting.
        Strip(new(e, FloorY, n), new(e, FloorY, s), new(eWall, FloorY, nWall), new(eWall, FloorY, sWall), Vector3.left);
        Strip(new(e, FloorY, s), new(w, FloorY, s), new(eWall, FloorY, sWall), new(wWall, FloorY, sWall), Vector3.forward);
        Strip(new(w, FloorY, n), new(e, FloorY, n), new(wWall, FloorY, nWall), new(eWall, FloorY, nWall), Vector3.back);

        CreateMeshPart(parent, "Interior_Skirting", mb.Build("DowntownWarehouse_InteriorSkirting"), layer, mat, ShadowCastingMode.On);
    }

    private static void BuildBeamsAndLights(Transform parent, int layer, Material? trimMat, Material? bulbMat)
    {
        float spanX = (OuterEast - WallInset) - (OuterWest + WallInset);
        float eave = EaveY - WallInset;
        float ridge = RidgeY - WallInset;

        // Three horizontal tie beams, vertical wall columns under them, and one ridge beam.
        float colH = eave - FloorY;
        float colY = FloorY + colH * 0.5f;
        foreach (float z in new[] { 20.0f, 25.5f, 31.0f })
        {
            CreateVisualBox(parent, $"Beam_Z{z:0}", new Vector3(HallPlan.CenterX, eave - 0.10f, z), new Vector3(spanX, 0.20f, 0.16f), layer, trimMat);
            CreateVisualBox(parent, $"ColumnWest_Z{z:0}", new Vector3(OuterWest + WallInset + 0.03f, colY, z), new Vector3(0.06f, colH, 0.16f), layer, trimMat);
            CreateVisualBox(parent, $"ColumnEast_Z{z:0}", new Vector3(OuterEast - WallInset - 0.03f, colY, z), new Vector3(0.06f, colH, 0.16f), layer, trimMat);
        }
        CreateVisualBox(parent, "Beam_Ridge", new Vector3(HallPlan.CenterX, ridge - 0.11f, HallPlan.CenterZ), new Vector3(spanX, 0.22f, 0.18f), layer, trimMat);

        // Four suspended fluorescent fixtures (2x2 grid) centered in the bays between beams.
        float[] xs = { 112.25f, 119.25f };
        float[] zs = { 22.75f, 28.25f };
        const float lightY = 5.55f;
        float halfRun = HallPlan.LengthZ * 0.5f;

        int index = 0;
        foreach (float x in xs)
        {
            foreach (float z in zs)
            {
                index++;
                float ceilingAtLightZ = eave + (ridge - eave) * (1f - MathF.Abs(z - HallPlan.CenterZ) / halfRun);
                float rodHeight = MathF.Max(0.2f, ceilingAtLightZ - lightY);
                CreateVisualBox(parent, $"LightHousing_{index}", new Vector3(x, lightY, z), new Vector3(1.50f, 0.07f, 0.22f), layer, trimMat);
                CreateVisualBox(parent, $"LightBulb_{index}", new Vector3(x, lightY - 0.04f, z), new Vector3(1.38f, 0.025f, 0.13f), layer, bulbMat);
                CreateVisualBox(parent, $"LightRodA_{index}", new Vector3(x - 0.55f, lightY + rodHeight * 0.5f, z), new Vector3(0.025f, rodHeight, 0.025f), layer, trimMat);
                CreateVisualBox(parent, $"LightRodB_{index}", new Vector3(x + 0.55f, lightY + rodHeight * 0.5f, z), new Vector3(0.025f, rodHeight, 0.025f), layer, trimMat);

                var lightGo = new GameObject($"InteriorLight_{index}");
                lightGo.transform.SetParent(parent, false);
                lightGo.transform.position = new Vector3(x, lightY - 0.15f, z);
                var light = lightGo.AddComponent<Light>();
                light.type = LightType.Point;
                light.color = new Color(1.0f, 0.96f, 0.89f);
                light.range = 14.5f;
                light.intensity = 1.65f;
                light.shadows = LightShadows.None;
            }
        }
    }

    private static void BuildSavePoint(Transform parent, int layer, Material? trimMat, Material? faceMat, Material? ledMat)
    {
        var donor = FindSavePointDonor();
        if (donor == null)
        {
            MelonLogger.Warning("[DowntownWarehouse] SavePoint-Vorlage fehlt; Savebutton uebersprungen.");
            return;
        }

        const float wallX = OuterWest + WallInset; // 108.83
        const float saveZ = 21.40f;                // left of blue door (DoorMinZ = 21.86)
        const float saveY = 1.45f;

        var saveGo = UnityEngine.Object.Instantiate(donor, parent);
        saveGo.name = "DowntownWarehouse_SavePoint";
        saveGo.transform.position = new Vector3(wallX + 0.03f, saveY, saveZ);
        saveGo.transform.rotation = Quaternion.identity;
        saveGo.transform.localScale = Vector3.one;

        // Static-batched Intercom meshes render at their original world pose; replace with procedural meshes.
        foreach (var mr in saveGo.GetComponentsInChildren<MeshRenderer>(true))
        {
            if (mr == null)
                continue;
            var mesh = mr.GetComponent<MeshFilter>()?.sharedMesh;
            if (mr.isPartOfStaticBatch || mesh == null || mesh.name.StartsWith("Combined", StringComparison.Ordinal))
                mr.enabled = false;
        }

        // Disable any inherited colliders on rotated child transforms and put one clean collider on root.
        foreach (var col in saveGo.GetComponentsInChildren<Collider>(true))
        {
            if (col != null && col.gameObject.Pointer != saveGo.Pointer)
                col.enabled = false;
        }

        var box = saveGo.GetComponent<BoxCollider>() ?? saveGo.AddComponent<BoxCollider>();
        box.isTrigger = false;
        box.center = Vector3.zero;
        box.size = new Vector3(0.12f, 0.38f, 0.24f);
        box.enabled = true;

        var point = saveGo.transform.Find("Point");
        if (point != null)
            point.position = new Vector3(wallX + 0.08f, saveY, saveZ);

        var sp = saveGo.GetComponent<SavePoint>();
        var io = sp?.IntObj ?? saveGo.GetComponent<InteractableObject>();
        if (io != null)
        {
            io.enabled = true;
            if (point != null)
                io.displayLocationPoint = point;
        }
        if (sp != null)
            sp.enabled = true;

        CreateVisualBox(saveGo.transform, "Intercom_Backplate", new Vector3(wallX + 0.01f, saveY, saveZ), new Vector3(0.02f, 0.34f, 0.22f), layer, trimMat);
        CreateVisualBox(saveGo.transform, "Intercom_Body", new Vector3(wallX + 0.03f, saveY, saveZ), new Vector3(0.04f, 0.28f, 0.16f), layer, faceMat);
        CreateVisualBox(saveGo.transform, "Intercom_Speaker", new Vector3(wallX + 0.052f, saveY + 0.05f, saveZ), new Vector3(0.01f, 0.06f, 0.10f), layer, trimMat);
        CreateVisualBox(saveGo.transform, "Intercom_Button", new Vector3(wallX + 0.055f, saveY - 0.06f, saveZ), new Vector3(0.015f, 0.07f, 0.07f), layer, trimMat);
        CreateVisualBox(saveGo.transform, "Intercom_Led", new Vector3(wallX + 0.055f, saveY + 0.105f, saveZ), new Vector3(0.012f, 0.022f, 0.022f), layer, ledMat);

        // Start inactive so SavePoint.Awake() only subscribes to SaveManager once the hall is owned.
        saveGo.SetActive(false);
        _savePoint = sp;
        _savePointGo = saveGo;
    }

    private static GameObject? FindSavePointDonor()
    {
        foreach (string path in new[]
        {
            "@Properties/DocksWarehouse/Container/Intercom Save Point",
            "@Properties/MotelRoom/MotelRoom/Intercom Save Point",
            "@Properties/Sweatshop/Contents/Intercom Save Point",
            "@Properties/Bungalow/Container/Intercom Save Point"
        })
        {
            var found = GameObject.Find(path);
            if (found != null && found.GetComponent<SavePoint>() != null)
                return found;
        }

        return UnityEngine.Object.FindObjectOfType<SavePoint>()?.gameObject;
    }

    private static void CreateMeshPart(Transform parent, string name, Mesh mesh, int layer, Material? mat, ShadowCastingMode shadows)
    {
        OwnedMeshes.Add(mesh);
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.layer = layer;
        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = mat;
        mr.shadowCastingMode = shadows;
        mr.receiveShadows = true;
    }

    private static void CreateVisualBox(Transform parent, string name, Vector3 pos, Vector3 scale, int layer, Material? mat)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = pos;
        go.transform.localScale = scale;
        go.layer = layer;
        var col = go.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
            UnityEngine.Object.Destroy(col);
        }
        var mr = go.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.sharedMaterial = mat;
            mr.shadowCastingMode = ShadowCastingMode.Off;
            mr.receiveShadows = true;
        }
    }

    private static Material? FindMaterial(Transform? parent, string relativePath)
    {
        var t = parent != null ? parent.Find(relativePath) : null;
        return t != null ? t.GetComponent<MeshRenderer>()?.sharedMaterial : null;
    }

    private static Material CreateOwnedLitMaterial(Color color, float metallic, float smoothness)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        var mat = new Material(shader) { color = color };
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
        OwnedMaterials.Add(mat);
        return mat;
    }

    private static Material CreateOwnedUnlitMaterial(Color color)
    {
        var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
        var mat = new Material(shader) { color = color };
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        OwnedMaterials.Add(mat);
        return mat;
    }

    private sealed class MeshBuilder
    {
        private readonly float _uvScaleU;
        private readonly float _uvScaleV;
        private readonly List<Vector3> _verts = new();
        private readonly List<Vector3> _norms = new();
        private readonly List<Vector2> _uvs = new();
        private readonly List<int> _tris = new();

        internal MeshBuilder(float uvScaleU, float uvScaleV)
        {
            _uvScaleU = uvScaleU;
            _uvScaleV = uvScaleV;
        }

        internal void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 expectedNormal)
        {
            AddTri(a, b, c, expectedNormal);
            AddTri(a, c, d, expectedNormal);
        }

        internal void AddTri(Vector3 a, Vector3 b, Vector3 c, Vector3 expectedNormal)
        {
            Vector3 normal = expectedNormal.normalized;
            // Ensure Unity left-handed CW winding matches expectedNormal.
            if (Vector3.Dot(Vector3.Cross(b - a, c - a), normal) < 0f)
                (b, c) = (c, b);

            int baseIdx = _verts.Count;
            _verts.Add(a);
            _verts.Add(b);
            _verts.Add(c);
            _norms.Add(normal);
            _norms.Add(normal);
            _norms.Add(normal);
            _uvs.Add(ProjectUv(a, normal));
            _uvs.Add(ProjectUv(b, normal));
            _uvs.Add(ProjectUv(c, normal));
            _tris.Add(baseIdx);
            _tris.Add(baseIdx + 1);
            _tris.Add(baseIdx + 2);
        }

        private Vector2 ProjectUv(Vector3 p, Vector3 n)
        {
            float ax = MathF.Abs(n.x);
            float ay = MathF.Abs(n.y);
            float az = MathF.Abs(n.z);
            float u, v;
            if (ax >= ay && ax >= az)
            {
                u = p.y - FloorY;
                v = p.z - OuterSouth;
            }
            else if (az >= ax && az >= ay)
            {
                u = p.y - FloorY;
                v = p.x - OuterWest;
            }
            else
            {
                u = p.x - OuterWest;
                v = p.z - OuterSouth;
            }
            return new Vector2(u * _uvScaleU, v * _uvScaleV);
        }

        internal Mesh Build(string name)
        {
            var mesh = new Mesh { name = name };
            mesh.vertices = _verts.ToArray();
            mesh.normals = _norms.ToArray();
            mesh.uv = _uvs.ToArray();
            mesh.triangles = _tris.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            return mesh;
        }
    }
}

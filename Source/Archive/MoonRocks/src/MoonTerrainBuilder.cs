using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MoonRocks;

/// <summary>
/// Builds the playable lunar crater terrain, Landing Base, inward space sky-dome, stars, and Earth-in-sky.
/// </summary>
internal sealed class MoonTerrainBuilder
{
    private const int GridResolution = 110;
    private readonly List<Mesh> _ownedMeshes = new();

    internal GameObject? Root { get; private set; }
    internal Vector3 LandingPadCenter => new(MoonMath.MoonOriginX, MoonMath.MoonOriginY, MoonMath.MoonOriginZ);
    internal Vector3 PlayerArrivalPoint => new(MoonMath.MoonOriginX + 6.5f, MoonMath.MoonOriginY + 0.35f, MoonMath.MoonOriginZ);

    internal void Build(MoonMaterials mats)
    {
        if (Root != null) return;

        Root = new GameObject("MoonRocks_MoonWorld");
        Root.transform.position = LandingPadCenter;

        BuildLunarSurface(Root.transform, mats);
        BuildLandingBase(Root.transform, mats);
        BuildSpaceSky(Root.transform, mats);
        BuildMoonLight(Root.transform);

        // Keep hidden while player is on Earth so no black dome appears in Hyland Point's sky.
        Root.SetActive(false);
    }

    internal void SetActive(bool active)
    {
        if (Root != null && Root.activeSelf != active)
        {
            Root.SetActive(active);
        }
    }

    private void BuildLunarSurface(Transform parent, MoonMaterials mats)
    {
        var go = new GameObject("LunarSurface");
        go.transform.SetParent(parent, false);

        float size = MoonMath.MoonRadius * 2f;
        float half = MoonMath.MoonRadius;
        int n = GridResolution;
        int vertCount = (n + 1) * (n + 1);

        var verts = new Vector3[vertCount];
        var uvs = new Vector2[vertCount];
        var tris = new int[n * n * 6];

        int v = 0;
        for (int z = 0; z <= n; z++)
        {
            float tz = (float)z / n;
            float wz = -half + tz * size;
            for (int x = 0; x <= n; x++)
            {
                float tx = (float)x / n;
                float wx = -half + tx * size;
                float wy = MoonMath.SampleTerrainHeight(wx, wz);
                verts[v] = new Vector3(wx, wy, wz);
                uvs[v] = new Vector2(tx * 16f, tz * 16f);
                v++;
            }
        }

        int ti = 0;
        for (int z = 0; z < n; z++)
        {
            for (int x = 0; x < n; x++)
            {
                int i0 = z * (n + 1) + x;
                int i1 = i0 + 1;
                int i2 = i0 + (n + 1);
                int i3 = i2 + 1;

                tris[ti++] = i0;
                tris[ti++] = i2;
                tris[ti++] = i1;

                tris[ti++] = i1;
                tris[ti++] = i2;
                tris[ti++] = i3;
            }
        }

        var mesh = new Mesh { name = "MoonRocks_LunarTerrain" };
        if (vertCount > 60000) mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        _ownedMeshes.Add(mesh);

        var mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = mats.MoonGround;

        var mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
    }

    private void BuildLandingBase(Transform parent, MoonMaterials mats)
    {
        var baseRoot = new GameObject("LandingBase");
        baseRoot.transform.SetParent(parent, false);

        // Central landing pad slab (slightly above y=0 so it sits cleanly on the flat center).
        CreateBox(baseRoot.transform, "PadSlab", new Vector3(0f, -0.05f, 0f), new Vector3(14f, 0.25f, 14f), mats.ConcretePad, true);
        CreateBox(baseRoot.transform, "PadRing", new Vector3(0f, 0.08f, 0f), new Vector3(9.5f, 0.04f, 9.5f), mats.MetalDark, false);

        // Four glowing corner beacons around the landing pad.
        float corner = 6.3f;
        Vector3[] beacons =
        {
            new(-corner, 0f, -corner),
            new(corner, 0f, -corner),
            new(-corner, 0f, corner),
            new(corner, 0f, corner)
        };
        for (int i = 0; i < beacons.Length; i++)
        {
            CreateBox(baseRoot.transform, $"BeaconPost_{i}", beacons[i] + new Vector3(0f, 0.7f, 0f), new Vector3(0.28f, 1.4f, 0.28f), mats.MetalDark, true);
            CreateBox(baseRoot.transform, $"BeaconLight_{i}", beacons[i] + new Vector3(0f, 1.5f, 0f), new Vector3(0.36f, 0.25f, 0.36f), mats.BeaconGlow, false);
        }

        // Small Lunar Outpost Shelter to the side of the pad.
        var shelter = new GameObject("LunarOutpost");
        shelter.transform.SetParent(baseRoot.transform, false);
        shelter.transform.localPosition = new Vector3(11.5f, 0f, 0f);

        CreateBox(shelter.transform, "Floor", new Vector3(0f, 0.05f, 0f), new Vector3(5.5f, 0.2f, 6.5f), mats.MetalDark, true);
        CreateBox(shelter.transform, "BackWall", new Vector3(2.6f, 1.55f, 0f), new Vector3(0.3f, 2.9f, 6.5f), mats.RocketBody, true);
        CreateBox(shelter.transform, "NorthWall", new Vector3(0f, 1.55f, 3.1f), new Vector3(5.5f, 2.9f, 0.3f), mats.RocketBody, true);
        CreateBox(shelter.transform, "SouthWall", new Vector3(0f, 1.55f, -3.1f), new Vector3(5.5f, 2.9f, 0.3f), mats.RocketBody, true);
        CreateBox(shelter.transform, "Roof", new Vector3(0f, 3.05f, 0f), new Vector3(5.8f, 0.25f, 6.8f), mats.GoldFoil, true);
        CreateBox(shelter.transform, "StripLight", new Vector3(0f, 2.88f, 0f), new Vector3(3.2f, 0.08f, 0.25f), mats.BeaconGlow, false);
    }

    private void BuildSpaceSky(Transform parent, MoonMaterials mats)
    {
        // Inward-facing black sky sphere so the player sees deep space around the moon.
        var domeGo = new GameObject("SpaceDome");
        domeGo.transform.SetParent(parent, false);
        Mesh domeMesh = BuildInwardSphereMesh(700f, 24, 16);
        _ownedMeshes.Add(domeMesh);

        domeGo.AddComponent<MeshFilter>().sharedMesh = domeMesh;
        var domeMr = domeGo.AddComponent<MeshRenderer>();
        domeMr.sharedMaterial = mats.SpaceDome;
        domeMr.shadowCastingMode = ShadowCastingMode.Off;
        domeMr.receiveShadows = false;

        // Deterministic starfield inside the dome.
        var starsRoot = new GameObject("Stars");
        starsRoot.transform.SetParent(parent, false);
        var rng = new System.Random(4242);
        for (int i = 0; i < 260; i++)
        {
            double theta = rng.NextDouble() * Math.PI * 2.0;
            // Upper hemisphere + slightly below horizon.
            double phi = Math.Acos(rng.NextDouble() * 1.15 - 0.15);
            float r = 560f;
            float sx = (float)(r * Math.Sin(phi) * Math.Cos(theta));
            float sy = (float)(r * Math.Cos(phi));
            float sz = (float)(r * Math.Sin(phi) * Math.Sin(theta));

            var star = GameObject.CreatePrimitive(PrimitiveType.Quad);
            star.name = $"Star_{i}";
            star.transform.SetParent(starsRoot.transform, false);
            star.transform.localPosition = new Vector3(sx, sy, sz);
            star.transform.LookAt(parent.position);
            float scale = 0.8f + (float)rng.NextDouble() * 1.8f;
            star.transform.localScale = new Vector3(scale, scale, scale);
            var col = star.GetComponent<Collider>();
            if (col != null) UnityEngine.Object.Destroy(col);
            var mr = star.GetComponent<MeshRenderer>();
            mr.sharedMaterial = mats.StarGlow;
            mr.shadowCastingMode = ShadowCastingMode.Off;
            mr.receiveShadows = false;
        }

        // Large Earth hanging in the lunar sky!
        var earthRoot = new GameObject("EarthInSky");
        earthRoot.transform.SetParent(parent, false);
        earthRoot.transform.localPosition = new Vector3(-220f, 240f, 320f);

        var earthSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        earthSphere.name = "EarthOcean";
        earthSphere.transform.SetParent(earthRoot.transform, false);
        earthSphere.transform.localScale = new Vector3(72f, 72f, 72f);
        var earthCol = earthSphere.GetComponent<Collider>();
        if (earthCol != null) UnityEngine.Object.Destroy(earthCol);
        var earthMr = earthSphere.GetComponent<MeshRenderer>();
        earthMr.sharedMaterial = mats.EarthOcean;
        earthMr.shadowCastingMode = ShadowCastingMode.Off;

        // Visible green continent patches protruding slightly from the ocean sphere.
        Vector3[] continents =
        {
            new Vector3(0.45f, 0.28f, -0.84f).normalized,
            new Vector3(-0.55f, -0.15f, -0.82f).normalized,
            new Vector3(0.10f, -0.58f, -0.80f).normalized
        };
        for (int c = 0; c < continents.Length; c++)
        {
            var patch = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            patch.name = $"EarthContinent_{c}";
            patch.transform.SetParent(earthRoot.transform, false);
            patch.transform.localPosition = continents[c] * 26f;
            patch.transform.localScale = new Vector3(28f, 22f, 22f);
            var pCol = patch.GetComponent<Collider>();
            if (pCol != null) UnityEngine.Object.Destroy(pCol);
            var pMr = patch.GetComponent<MeshRenderer>();
            pMr.sharedMaterial = mats.EarthLand;
            pMr.shadowCastingMode = ShadowCastingMode.Off;
        }
    }

    private static void BuildMoonLight(Transform parent)
    {
        var lightGo = new GameObject("MoonSunLight");
        lightGo.transform.SetParent(parent, false);
        lightGo.transform.localPosition = new Vector3(0f, 40f, 0f);
        lightGo.transform.rotation = Quaternion.Euler(38f, -45f, 0f);

        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(0.92f, 0.94f, 1.0f);
        light.intensity = 1.15f;
        light.shadows = LightShadows.Soft;
    }

    private static Mesh BuildInwardSphereMesh(float radius, int lonSegments, int latSegments)
    {
        var verts = new List<Vector3>();
        var tris = new List<int>();

        for (int lat = 0; lat <= latSegments; lat++)
        {
            float v = (float)lat / latSegments;
            float phi = v * Mathf.PI;
            for (int lon = 0; lon <= lonSegments; lon++)
            {
                float u = (float)lon / lonSegments;
                float theta = u * Mathf.PI * 2f;
                float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = radius * Mathf.Cos(phi);
                float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                verts.Add(new Vector3(x, y, z));
            }
        }

        int stride = lonSegments + 1;
        for (int lat = 0; lat < latSegments; lat++)
        {
            for (int lon = 0; lon < lonSegments; lon++)
            {
                int i0 = lat * stride + lon;
                int i1 = i0 + 1;
                int i2 = i0 + stride;
                int i3 = i2 + 1;

                // Reversed winding so triangles face inward.
                tris.Add(i0);
                tris.Add(i1);
                tris.Add(i2);

                tris.Add(i1);
                tris.Add(i3);
                tris.Add(i2);
            }
        }

        var mesh = new Mesh { name = "MoonRocks_SpaceDome" };
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    private static GameObject CreateBox(Transform parent, string name, Vector3 localPos, Vector3 scale, Material mat, bool withCollider)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPos;
        go.transform.localScale = scale;
        if (!withCollider)
        {
            var col = go.GetComponent<Collider>();
            if (col != null) UnityEngine.Object.Destroy(col);
        }
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        return go;
    }

    internal void Destroy()
    {
        if (Root != null)
        {
            UnityEngine.Object.Destroy(Root);
            Root = null;
        }
        for (int i = 0; i < _ownedMeshes.Count; i++)
        {
            if (_ownedMeshes[i] != null) UnityEngine.Object.Destroy(_ownedMeshes[i]);
        }
        _ownedMeshes.Clear();
    }
}

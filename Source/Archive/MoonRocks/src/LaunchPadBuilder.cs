using System.Collections.Generic;
using UnityEngine;

namespace MoonRocks;

/// <summary>
/// Builds the Earth launch pad (with raycasted ground alignment) and the flyable 3D Rocket with thruster effects.
/// </summary>
internal sealed class LaunchPadBuilder
{
    private readonly List<Mesh> _ownedMeshes = new();
    private readonly List<GameObject> _hiddenFences = new();

    internal GameObject? PadRoot { get; private set; }
    internal GameObject? RocketRoot { get; private set; }
    internal GameObject? ThrusterFlame { get; private set; }
    private GameObject[] _exhaustPuffs = System.Array.Empty<GameObject>();
    internal Vector3 EarthPadCenter { get; private set; }
    internal float EarthPadYaw { get; private set; }
    internal Vector3 EarthArrivalPoint => PadRoot != null
        ? PadRoot.transform.TransformPoint(new Vector3(4.2f, 0.35f, 0f))
        : EarthPadCenter + new Vector3(4.2f, 0.35f, 0f);

    internal void BuildAtExactPosition(Vector3 exactPadCenter, float yawDegrees, MoonMaterials mats)
    {
        if (PadRoot != null) return;
        EarthPadCenter = exactPadCenter;
        EarthPadYaw = yawDegrees;
        BuildInternal(mats);
        OpenDockFencePassage();
    }

    internal void Build(Vector3 candidateXZ, MoonMaterials mats)
    {
        if (PadRoot != null) return;

        float groundY = ResolveGroundHeight(candidateXZ);
        EarthPadCenter = new Vector3(candidateXZ.x, groundY, candidateXZ.z);
        EarthPadYaw = 0f;
        BuildInternal(mats);
    }

    private void BuildInternal(MoonMaterials mats)
    {
        PadRoot = new GameObject("MoonRocks_EarthLaunchPad");
        PadRoot.transform.position = EarthPadCenter;
        PadRoot.transform.rotation = Quaternion.Euler(0f, EarthPadYaw, 0f);

        // Dock extension pier (11.5 x 12.0m concrete platform flush with X=-36.02 quay wall + deep water pillars).
        const float pierW = 11.5f;
        const float pierD = 12.0f;
        CreateBox(PadRoot.transform, "PierDeck", new Vector3(0f, -0.22f, 0f), new Vector3(pierW, 0.45f, pierD), mats.ConcretePad, true);
        float px = pierW * 0.5f - 0.85f;
        float pz = pierD * 0.5f - 0.85f;
        Vector3[] pillars = { new(-px, -4.4f, -pz), new(px, -4.4f, -pz), new(-px, -4.4f, pz), new(px, -4.4f, pz) };
        for (int i = 0; i < pillars.Length; i++)
        {
            CreateBox(PadRoot.transform, $"PierPillar_{i}", pillars[i], new Vector3(0.95f, 8.2f, 0.95f), mats.MetalDark, true);
        }

        // Build matching dark metal railing around -Z (warehouse side), -X (waterfront), +Z (dock side), leaving +X (promenade) open.
        BuildPierRailing(PadRoot.transform, pierW, pierD, mats.MetalDark);

        // Concrete launch deck + blast ring (compact 6.8 x 6.8m footprint on top of the pier).
        CreateBox(PadRoot.transform, "PadDeck", new Vector3(0f, 0.1f, 0f), new Vector3(6.8f, 0.35f, 6.8f), mats.ConcretePad, true);
        CreateBox(PadRoot.transform, "BlastRing", new Vector3(0f, 0.29f, 0f), new Vector3(4.2f, 0.05f, 4.2f), mats.MetalDark, false);

        // Support gantry tower beside the rocket.
        CreateBox(PadRoot.transform, "GantrySpine", new Vector3(-2.55f, 5.0f, 0f), new Vector3(0.7f, 10.0f, 0.7f), mats.MetalDark, true);
        CreateBox(PadRoot.transform, "GantryArm", new Vector3(-1.55f, 7.2f, 0f), new Vector3(2.0f, 0.4f, 0.5f), mats.RocketAccent, true);
        CreateBox(PadRoot.transform, "GantryBeacon", new Vector3(-2.55f, 10.2f, 0f), new Vector3(0.4f, 0.32f, 0.4f), mats.RocketAccent, false);

        // Corner pad beacons.
        float c = 3.0f;
        Vector3[] corners = { new(-c, 0.4f, -c), new(c, 0.4f, -c), new(-c, 0.4f, c), new(c, 0.4f, c) };
        for (int i = 0; i < corners.Length; i++)
        {
            CreateBox(PadRoot.transform, $"PadBeacon_{i}", corners[i], new Vector3(0.32f, 0.35f, 0.32f), mats.BeaconGlow, false);
        }

        BuildRocket(mats);
        PlaceRocketOnEarth();
    }

    internal void PlaceRocketOnEarth()
    {
        if (RocketRoot == null) return;
        RocketRoot.transform.position = EarthPadCenter + new Vector3(0f, 0.32f, 0f);
        RocketRoot.transform.rotation = Quaternion.identity;
        SetThrustersActive(false);
    }

    internal void RepositionPad(Vector3 newWorldPos)
    {
        if (PadRoot == null) return;
        EarthPadCenter = newWorldPos;
        PadRoot.transform.position = EarthPadCenter;
        PlaceRocketOnEarth();
    }

    internal void PlaceRocketOnMoon(Vector3 moonPadCenter)
    {
        if (RocketRoot == null) return;
        RocketRoot.transform.position = moonPadCenter + new Vector3(0f, 0.10f, 0f);
        RocketRoot.transform.rotation = Quaternion.identity;
        SetThrustersActive(false);
    }

    internal void SetThrustersActive(bool active)
    {
        if (ThrusterFlame != null) ThrusterFlame.SetActive(active);
        for (int i = 0; i < _exhaustPuffs.Length; i++)
        {
            if (_exhaustPuffs[i] != null) _exhaustPuffs[i].SetActive(active);
        }
    }

    internal void AnimateThrusters(float elapsed)
    {
        if (ThrusterFlame != null && ThrusterFlame.activeSelf)
        {
            float pulse = 0.92f + Mathf.Sin(elapsed * 42f) * 0.14f;
            ThrusterFlame.transform.localScale = new Vector3(pulse, 0.85f + Mathf.Cos(elapsed * 31f) * 0.2f, pulse);
        }

        for (int i = 0; i < _exhaustPuffs.Length; i++)
        {
            var puff = _exhaustPuffs[i];
            if (puff == null || !puff.activeSelf) continue;
            float phase = (elapsed * 2.4f + i * 0.17f) % 1f;
            float spread = 0.35f + phase * 1.4f;
            float angle = i * 1.15f + elapsed * 3f;
            puff.transform.localPosition = new Vector3(
                Mathf.Cos(angle) * spread * 0.45f,
                -0.6f - phase * 6.5f,
                Mathf.Sin(angle) * spread * 0.45f);
            float scale = Mathf.Lerp(1.2f, 0.25f, phase);
            puff.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void BuildRocket(MoonMaterials mats)
    {
        RocketRoot = new GameObject("MoonRocks_Rocket");

        // Main hull cylinder.
        var hull = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hull.name = "Hull";
        hull.transform.SetParent(RocketRoot.transform, false);
        hull.transform.localPosition = new Vector3(0f, 4.2f, 0f);
        hull.transform.localScale = new Vector3(2.1f, 3.2f, 2.1f);
        hull.GetComponent<MeshRenderer>().sharedMaterial = mats.RocketBody;

        // Red orbital stripe band.
        var stripe = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        stripe.name = "AccentStripe";
        stripe.transform.SetParent(RocketRoot.transform, false);
        stripe.transform.localPosition = new Vector3(0f, 6.2f, 0f);
        stripe.transform.localScale = new Vector3(2.16f, 0.45f, 2.16f);
        DestroyCollider(stripe);
        stripe.GetComponent<MeshRenderer>().sharedMaterial = mats.RocketAccent;

        // Cockpit viewport window.
        var window = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        window.name = "CockpitWindow";
        window.transform.SetParent(RocketRoot.transform, false);
        window.transform.localPosition = new Vector3(0f, 5.5f, 0.92f);
        window.transform.localScale = new Vector3(0.75f, 0.75f, 0.45f);
        DestroyCollider(window);
        window.GetComponent<MeshRenderer>().sharedMaterial = mats.GlassCyan;

        // Nose cone.
        var nose = new GameObject("NoseCone");
        nose.transform.SetParent(RocketRoot.transform, false);
        nose.transform.localPosition = new Vector3(0f, 7.4f, 0f);
        Mesh coneMesh = BuildConeMesh(1.06f, 2.4f, 20);
        _ownedMeshes.Add(coneMesh);
        nose.AddComponent<MeshFilter>().sharedMesh = coneMesh;
        nose.AddComponent<MeshRenderer>().sharedMaterial = mats.RocketAccent;

        // 4 Stabilizer fins + landing legs.
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f;
            var finPivot = new GameObject($"Fin_{i}");
            finPivot.transform.SetParent(RocketRoot.transform, false);
            finPivot.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            CreateBox(finPivot.transform, "FinBlade", new Vector3(1.22f, 1.35f, 0f), new Vector3(0.7f, 1.9f, 0.16f), mats.RocketAccent, true);
            CreateBox(finPivot.transform, "LandingFoot", new Vector3(1.52f, 0.35f, 0f), new Vector3(0.45f, 0.7f, 0.35f), mats.MetalDark, true);
        }

        // Engine bell.
        var bell = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        bell.name = "EngineBell";
        bell.transform.SetParent(RocketRoot.transform, false);
        bell.transform.localPosition = new Vector3(0f, 0.75f, 0f);
        bell.transform.localScale = new Vector3(1.25f, 0.55f, 1.25f);
        DestroyCollider(bell);
        bell.GetComponent<MeshRenderer>().sharedMaterial = mats.MetalDark;

        // Glowing thruster flame cone (toggled on during flight).
        ThrusterFlame = new GameObject("ThrusterFlame");
        ThrusterFlame.transform.SetParent(RocketRoot.transform, false);
        ThrusterFlame.transform.localPosition = new Vector3(0f, 0.25f, 0f);
        ThrusterFlame.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
        Mesh flameMesh = BuildConeMesh(0.75f, 4.2f, 14);
        _ownedMeshes.Add(flameMesh);
        ThrusterFlame.AddComponent<MeshFilter>().sharedMesh = flameMesh;
        var flameMr = ThrusterFlame.AddComponent<MeshRenderer>();
        flameMr.sharedMaterial = mats.FlameGlow;
        flameMr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        ThrusterFlame.SetActive(false);

        // Animated exhaust puffs (URP-safe spheres using owned FlameGlow/RocketBody materials).
        const int puffCount = 8;
        _exhaustPuffs = new GameObject[puffCount];
        for (int i = 0; i < puffCount; i++)
        {
            var puff = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            puff.name = $"ExhaustPuff_{i}";
            puff.transform.SetParent(RocketRoot.transform, false);
            DestroyCollider(puff);
            var mr = puff.GetComponent<MeshRenderer>();
            mr.sharedMaterial = (i % 2 == 0) ? mats.FlameGlow : mats.RocketBody;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            puff.SetActive(false);
            _exhaustPuffs[i] = puff;
        }
    }

    private static float ResolveGroundHeight(Vector3 xz)
    {
        Vector3 rayOrigin = new(xz.x, 220f, xz.z);
        var hits = Physics.RaycastAll(rayOrigin, Vector3.down, 400f);
        if (hits == null || hits.Length == 0) return 1.5f;

        float bestTerrainY = float.MinValue;
        float lowestGroundY = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            var col = hits[i].collider;
            if (col == null) continue;
            if (col.GetType().Name.Contains("Terrain"))
            {
                if (hits[i].point.y > bestTerrainY) bestTerrainY = hits[i].point.y;
                continue;
            }

            string name = col.gameObject.name.ToLowerInvariant();
            if (name.Contains("tree") || name.Contains("pine") || name.Contains("bush") ||
                name.Contains("leaf") || name.Contains("wire") || name.Contains("cable") ||
                name.Contains("pole") || name.Contains("roof") || name.Contains("foliage"))
            {
                continue;
            }

            if (hits[i].point.y > -5f && hits[i].point.y < lowestGroundY)
            {
                lowestGroundY = hits[i].point.y;
            }
        }

        if (bestTerrainY > float.MinValue) return bestTerrainY;
        if (lowestGroundY < float.MaxValue) return lowestGroundY;
        return 1.5f;
    }

    private static Mesh BuildConeMesh(float radius, float height, int segments)
    {
        var verts = new Vector3[segments + 2];
        var tris = new int[segments * 6];

        verts[0] = new Vector3(0f, height, 0f); // Tip
        verts[1] = Vector3.zero;                 // Base center

        for (int i = 0; i < segments; i++)
        {
            float a = (float)i / segments * Mathf.PI * 2f;
            verts[i + 2] = new Vector3(Mathf.Cos(a) * radius, 0f, Mathf.Sin(a) * radius);
        }

        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int cur = i + 2;
            int next = (i + 1) % segments + 2;

            // Side triangle
            tris[ti++] = 0;
            tris[ti++] = next;
            tris[ti++] = cur;

            // Base cap
            tris[ti++] = 1;
            tris[ti++] = cur;
            tris[ti++] = next;
        }

        var mesh = new Mesh { name = "MoonRocks_Cone" };
        mesh.vertices = verts;
        mesh.triangles = tris;
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
        if (!withCollider) DestroyCollider(go);
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        return go;
    }

    private static void DestroyCollider(GameObject go)
    {
        var col = go.GetComponent<Collider>();
        if (col != null) UnityEngine.Object.Destroy(col);
    }

    private static void BuildPierRailing(Transform parent, float pierW, float pierD, Material metalMat)
    {
        var railRoot = new GameObject("PierRailing");
        railRoot.transform.SetParent(parent, false);

        float halfX = pierW * 0.5f - 0.15f;
        float halfZ = pierD * 0.5f - 0.15f;
        const float h = 1.05f;

        // Top & mid rails on -Z (warehouse side), +Z (dock side), and -X (waterfront). +X stays open to promenade.
        CreateBox(railRoot.transform, "RailTop_SideA", new Vector3(0f, h, -halfZ), new Vector3(pierW - 0.25f, 0.08f, 0.09f), metalMat, true);
        CreateBox(railRoot.transform, "RailMid_SideA", new Vector3(0f, h * 0.52f, -halfZ), new Vector3(pierW - 0.25f, 0.06f, 0.06f), metalMat, false);

        CreateBox(railRoot.transform, "RailTop_SideB", new Vector3(0f, h, halfZ), new Vector3(pierW - 0.25f, 0.08f, 0.09f), metalMat, true);
        CreateBox(railRoot.transform, "RailMid_SideB", new Vector3(0f, h * 0.52f, halfZ), new Vector3(pierW - 0.25f, 0.06f, 0.06f), metalMat, false);

        CreateBox(railRoot.transform, "RailTop_Water", new Vector3(-halfX, h, 0f), new Vector3(0.09f, 0.08f, pierD - 0.25f), metalMat, true);
        CreateBox(railRoot.transform, "RailMid_Water", new Vector3(-halfX, h * 0.52f, 0f), new Vector3(0.06f, 0.06f, pierD - 0.25f), metalMat, false);

        // Vertical posts along -Z and +Z sides.
        const int sidePosts = 6;
        for (int i = 0; i <= sidePosts; i++)
        {
            float x = Mathf.Lerp(-halfX, halfX, (float)i / sidePosts);
            CreateBox(railRoot.transform, $"PostA_{i}", new Vector3(x, h * 0.5f, -halfZ), new Vector3(0.10f, h, 0.10f), metalMat, false);
            CreateBox(railRoot.transform, $"PostB_{i}", new Vector3(x, h * 0.5f, halfZ), new Vector3(0.10f, h, 0.10f), metalMat, false);
        }

        // Vertical posts along -X waterfront edge.
        const int waterPosts = 6;
        for (int i = 1; i < waterPosts; i++)
        {
            float z = Mathf.Lerp(-halfZ, halfZ, (float)i / waterPosts);
            CreateBox(railRoot.transform, $"PostW_{i}", new Vector3(-halfX, h * 0.5f, z), new Vector3(0.10f, h, 0.10f), metalMat, false);
        }
    }

    private void OpenDockFencePassage()
    {
        if (PadRoot == null) return;
        Vector3 boxCenter = PadRoot.transform.TransformPoint(new Vector3(5.5f, 0.9f, 0f));
        Vector3 halfExtents = new(2.2f, 2.0f, 6.5f);
        var cols = Physics.OverlapBox(boxCenter, halfExtents, PadRoot.transform.rotation);
        if (cols == null || cols.Length == 0) return;

        for (int i = 0; i < cols.Length; i++)
        {
            var col = cols[i];
            if (col == null) continue;
            var go = col.gameObject;
            if (go == null || go.name.StartsWith("MoonRocks_", System.StringComparison.Ordinal)) continue;

            Transform? parent = go.transform.parent;
            string parentName = parent != null ? parent.name.ToLowerInvariant() : string.Empty;
            Transform? grandParent = parent != null ? parent.parent : null;
            string grandName = grandParent != null ? grandParent.name.ToLowerInvariant() : string.Empty;

            // Never disable our own PierRailing!
            if (parentName.Contains("pierrailing") || grandName.Contains("pierrailing") ||
                go.name.StartsWith("Post", System.StringComparison.Ordinal) && parentName.Contains("railing"))
            {
                continue;
            }

            string objName = go.name.ToLowerInvariant();
            if (grandParent != null && grandName.StartsWith("outdoorfence", System.StringComparison.Ordinal))
            {
                var root = grandParent.gameObject;
                if (root.activeSelf)
                {
                    root.SetActive(false);
                    _hiddenFences.Add(root);
                    Mod.Log($"Zaun-Root komplett geoeffnet: {root.name} @ {root.transform.position}");
                }
                continue;
            }

            if (parentName.Contains("outdoorfence") && parent != null)
            {
                var fenceRoot = parent.gameObject;
                if (fenceRoot.activeSelf)
                {
                    fenceRoot.SetActive(false);
                    _hiddenFences.Add(fenceRoot);
                    Mod.Log($"Zaun-Segment komplett geoeffnet: {fenceRoot.name} @ {fenceRoot.transform.position}");
                }
                continue;
            }

            if (objName.Contains("fence") || objName.Contains("rail") || objName.Contains("barrier") ||
                objName.Contains("guard") || objName.Contains("crossbeam") ||
                parentName.Contains("fence") || parentName.Contains("rail"))
            {
                if (go.activeSelf)
                {
                    go.SetActive(false);
                    _hiddenFences.Add(go);
                    Mod.Log($"Zaun-Durchgang geoeffnet: {go.name} (Parent: {parentName}) @ {go.transform.position}");
                }
            }
        }
    }

    internal void Destroy()
    {
        for (int i = 0; i < _hiddenFences.Count; i++)
        {
            if (_hiddenFences[i] != null) _hiddenFences[i].SetActive(true);
        }
        _hiddenFences.Clear();

        if (PadRoot != null)
        {
            UnityEngine.Object.Destroy(PadRoot);
            PadRoot = null;
        }
        if (RocketRoot != null)
        {
            UnityEngine.Object.Destroy(RocketRoot);
            RocketRoot = null;
        }
        ThrusterFlame = null;
        _exhaustPuffs = System.Array.Empty<GameObject>();
        for (int i = 0; i < _ownedMeshes.Count; i++)
        {
            if (_ownedMeshes[i] != null) UnityEngine.Object.Destroy(_ownedMeshes[i]);
        }
        _ownedMeshes.Clear();
    }
}

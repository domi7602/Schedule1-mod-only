using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

namespace TownHallInterior;

// Pure geometry instances, never clones of NPC, seating, inventory or network objects.
// Prepared only after the host's native scene hash/entrance checks have succeeded.
internal sealed class VanillaFurnishings : IDisposable
{
    private sealed record Part(Mesh Mesh, Material[] Materials, uint RenderingLayerMask);
    private readonly List<Mesh> _meshes = new();
    private readonly List<GameObject> _objects = new();
    private readonly List<MeshFilter> _filters = new();
    private readonly List<Renderer> _paintConsumers = new();
    private readonly List<Material> _paints = new();
    private readonly Dictionary<string, Part[]> _parts = new(StringComparer.Ordinal);
    private GameObject? _root;
    private MeshRenderer? _nativeFrame;
    private bool _frameWasOff;
    private bool _framePublished;
    private CleanupJournal? _cleanup;
    internal bool CleanupComplete { get; private set; }

    internal void PrepareAndPublish(Transform room)
    {
        if (_root != null || _cleanup != null) throw new InvalidOperationException("Furniture owner is not fresh");
        using var stream = typeof(VanillaFurnishings).Assembly.GetManifestResourceStream("TownHallInterior.NativeFurniture")
            ?? throw new InvalidOperationException("Local vanilla furniture extraction missing");
        using var bytes = new MemoryStream(); stream.CopyTo(bytes);
        var document = FurniturePlan.Read(bytes.ToArray());
        var placements = FurniturePlan.Create(document);
        _root = Own(new GameObject("TownHallInterior.VanillaReception"));
        _root.SetActive(false);
        _root.transform.SetParent(room, false);
        var assets = new Dictionary<string, FurnitureAsset>(StringComparer.Ordinal);
        foreach (var original in document.Assets)
        {
            var asset = FurniturePlan.PrepareAsset(original);
            assets.Add(asset.Key, asset);
            var parts = new List<Part>();
            foreach (var part in asset.Parts)
            {
                var source = Resolve(part, asset.Key);
                if (asset.Key == "door-frame")
                {
                    _nativeFrame = source;
                    _frameWasOff = source.forceRenderingOff;
                }
                var materials = new Material[part.MaterialNames.Length];
                for (int i = 0; i < materials.Length; i++) materials[i] = source.sharedMaterials[i];
                var mesh = CreateMesh(asset.Key, part);
                parts.Add(new Part(mesh, materials, source.renderingLayerMask));
            }
            _parts.Add(asset.Key, parts.ToArray());
        }
        foreach (var placement in placements) Place(assets[placement.Key], placement);
        int lightCount = CreateShellAndLights();
        if (_nativeFrame == null) throw new InvalidOperationException("Native visual frame missing");
        // Mark first: publication failure still restores the exact captured renderer.
        _framePublished = true;
        _nativeFrame.forceRenderingOff = true;
        _root.SetActive(true);
        MelonLogger.Msg("TownHallInterior: vanilla reception published: " + placements.Count +
            " model instances, " + _meshes.Count + " owned shared meshes, " + lightCount + " local lights; decorative only.");
    }

    private static MeshRenderer Resolve(FurniturePart part, string key)
    {
        var node = PublicHallHost.Find(part.SourcePath)
            ?? throw new InvalidOperationException("Vanilla donor missing: " + part.SourcePath);
        var renderer = node.GetComponent<MeshRenderer>();
        var mesh = node.GetComponent<MeshFilter>()?.sharedMesh;
        if (renderer == null || mesh == null || mesh.name != part.MeshName ||
            mesh.vertexCount != part.SourceVertexCount || renderer.isPartOfStaticBatch != part.IsStaticBatch ||
            renderer.subMeshStartIndex != part.FirstSubMesh)
            throw new InvalidOperationException("Vanilla donor mesh identity changed: " + part.SourcePath);
        var materials = renderer.sharedMaterials;
        if (materials == null || materials.Length != part.MaterialNames.Length)
            throw new InvalidOperationException("Vanilla donor material count changed: " + part.SourcePath);
        for (int i = 0; i < materials.Length; i++)
            if (materials[i] == null || materials[i].name != part.MaterialNames[i])
                throw new InvalidOperationException("Vanilla donor material changed: " + part.SourcePath);
        // The pinned clock hands are statically batched, but may have a changed pose.
        // Their material/mesh identities still have to match; their transforms are not copied.
        if (FurniturePlan.RequiresStableBounds(key, part))
        {
            var bounds = renderer.bounds;
            for (int axis = 0; axis < 3; axis++)
                if (Math.Abs(bounds.min[axis] - part.SourceBoundsMin[axis]) > .03 ||
                    Math.Abs(bounds.max[axis] - part.SourceBoundsMax[axis]) > .03)
                    throw new InvalidOperationException("Static vanilla donor bounds changed: " + part.SourcePath);
        }
        return renderer;
    }

    private Mesh CreateMesh(string key, FurniturePart part)
    {
        var mesh = new Mesh();
        _meshes.Add(mesh); // Register before any Unity upload can fail.
        mesh.name = "TownHallInterior.Vanilla." + key;
        int count = part.Vertices.Length;
        var positions = new Vector3[count]; var normals = new Vector3[count];
        var tangents = new Vector4[count]; var uv0 = new Vector2[count]; var uv1 = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            var v = part.Vertices[i];
            positions[i] = new Vector3((float)v[0], (float)v[1], (float)v[2]);
            normals[i] = new Vector3((float)v[3], (float)v[4], (float)v[5]);
            tangents[i] = new Vector4((float)v[6], (float)v[7], (float)v[8], (float)v[9]);
            uv0[i] = new Vector2((float)v[10], (float)v[11]);
            uv1[i] = new Vector2((float)v[12], (float)v[13]);
        }
        mesh.vertices = positions; mesh.normals = normals; mesh.tangents = tangents;
        mesh.uv = uv0; mesh.uv2 = uv1; mesh.subMeshCount = part.Submeshes.Length;
        for (int i = 0; i < part.Submeshes.Length; i++) mesh.SetTriangles(part.Submeshes[i], i, false);
        mesh.RecalculateBounds();
        return mesh;
    }

    private void Place(FurnitureAsset asset, FurniturePlacement placement)
    {
        var root = Own(new GameObject(placement.Name));
        root.transform.SetParent(_root!.transform, false);
        root.transform.localPosition = new Vector3(placement.X, placement.Y, placement.Z);
        root.transform.localRotation = Quaternion.Euler(0, placement.Yaw, 0);
        if (placement.Scale != 1f)
            root.transform.localScale = new Vector3(placement.Scale, placement.Scale, placement.Scale);
        foreach (var part in _parts[asset.Key])
        {
            var node = Own(new GameObject(asset.Key));
            node.transform.SetParent(root.transform, false);
            var filter = node.AddComponent<MeshFilter>();
            _filters.Add(filter);
            filter.sharedMesh = part.Mesh;
            var renderer = node.AddComponent<MeshRenderer>();
            renderer.sharedMaterials = part.Materials; // borrowed, never edited or destroyed
            renderer.renderingLayerMask = part.RenderingLayerMask;
            renderer.lightmapIndex = -1; // Relocated furniture must not sample donor lightmaps.
            renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
            renderer.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
            renderer.receiveShadows = true;
            renderer.shadowCastingMode = ShadowCastingMode.On;
        }
        if (placement.Solid)
        {
            var collider = root.AddComponent<BoxCollider>();
            collider.isTrigger = false;
            collider.center = new Vector3(0, (float)asset.BoundsMax[1] / 2, 0);
            collider.size = new Vector3((float)(asset.BoundsMax[0] - asset.BoundsMin[0]),
                (float)asset.BoundsMax[1], (float)(asset.BoundsMax[2] - asset.BoundsMin[2]));
        }
    }

    private int CreateShellAndLights()
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit")
            ?? throw new InvalidOperationException("URP/Lit unavailable for inset ceiling");
        // Floor edges match the accepted hall floor; wall infills match its paint.
        // Ceiling/lamp color and the existing four reception lights remain unchanged.
        foreach (var color in new[]
                 {
                     new Color(.72f, .68f, .60f), new Color(.84f, .82f, .76f), new Color(.78f, .74f, .66f),
                     new Color(.29f, .31f, .34f), // 3: dark built-in joinery, matches the door frame
                     new Color(.36f, .16f, .17f), // 4: muted bordeaux runner
                 })
        {
            var paint = new Material(shader);
            _paints.Add(paint); // Register before the next fallible native setter.
            paint.color = color;
        }
        foreach (var box in FurniturePlan.CreateShell()) DecorativeBox(box);
        foreach (var box in HallFittings.Create()) DecorativeBox(box);
        var lights = FurniturePlan.CreateLights();
        foreach (var planned in lights)
        {
            DecorativeBox(new RoomBox("ceiling-lamp-" + planned.Name, planned.X, planned.Y + .22f, planned.Z, 1.2f, .04f, .35f, false, 1));
            var node = Own(new GameObject("light-" + planned.Name));
            node.transform.SetParent(_root!.transform, false);
            node.transform.localPosition = new Vector3(planned.X, planned.Y, planned.Z);
            var light = node.AddComponent<Light>();
            light.type = LightType.Point; light.color = new Color(1f, .94f, .83f);
            light.range = planned.Range; light.intensity = planned.Intensity; light.shadows = LightShadows.None;
            light.cullingMask = -1; light.renderMode = LightRenderMode.ForcePixel;
        }
        return lights.Count;
    }

    private void DecorativeBox(RoomBox box)
    {
        var node = Own(GameObject.CreatePrimitive(PrimitiveType.Cube));
        node.transform.SetParent(_root!.transform, false); node.name = box.Name;
        node.transform.localPosition = new Vector3(box.X, box.Y, box.Z);
        node.transform.localScale = new Vector3(box.Width, box.Height, box.Depth);
        var collider = node.GetComponent<Collider>();
        // Built-in fittings (counter, benches) must block the player; shell skins must not.
        if (!box.Solid) { collider.enabled = false; UnityEngine.Object.Destroy(collider); }
        var renderer = node.GetComponent<Renderer>();
        _paintConsumers.Add(renderer); renderer.sharedMaterial = _paints[box.Style];
    }

    private GameObject Own(GameObject node) { _objects.Add(node); return node; }

    public void Dispose()
    {
        if (CleanupComplete) return;
        _cleanup ??= CreateCleanup();
        CleanupComplete = _cleanup.Run((step, ex) => MelonLogger.Warning("TownHallInterior furniture cleanup pending (" + step + "): " + ex.Message));
        if (CleanupComplete) { _parts.Clear(); _filters.Clear(); _paintConsumers.Clear(); _paints.Clear(); _meshes.Clear(); _objects.Clear(); _root = null; }
    }

    private CleanupJournal CreateCleanup()
    {
        var cleanup = new CleanupJournal();
        int detached = 0, painted = 0;
        cleanup.Add("furniture visibility", () => { if (_root != null) _root.SetActive(false); });
        cleanup.Add("native visual frame", () =>
        {
            if (_framePublished && _nativeFrame != null && _nativeFrame.forceRenderingOff)
                _nativeFrame.forceRenderingOff = _frameWasOff;
        });
        foreach (var filter in _filters)
            cleanup.Add("mesh consumer", () => { if (filter != null) filter.sharedMesh = null; detached++; });
        foreach (var renderer in _paintConsumers)
            cleanup.Add("paint consumer", () => { if (renderer != null) renderer.sharedMaterial = null; painted++; });
        bool Ready() => detached == _filters.Count && painted == _paintConsumers.Count;
        foreach (var node in _objects)
            cleanup.Add("owned furniture node", () => { if (node != null) UnityEngine.Object.Destroy(node); }, Ready);
        foreach (var mesh in _meshes)
            cleanup.Add("owned furniture mesh", () => { if (mesh != null) UnityEngine.Object.Destroy(mesh); }, Ready);
        foreach (var paint in _paints)
            cleanup.Add("owned enclosure material", () => { if (paint != null) UnityEngine.Object.Destroy(paint); }, Ready);
        return cleanup;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using S1Mods.Shared;
using UnityEngine;

namespace DowntownWarehouse;

/// <summary>Replaces only the two near-LOD contributions crossing the shop doorway.</summary>
internal static class HallSkin
{
    private const string ResourceName = "DowntownWarehouse.main-cut.json";
    private const string ExpectedHash = "DB902D35C113252D5C93A27DAC8F3ACDE42093B9C1E2C704EAEABCF267313681";

    private class CutMesh
    {
        [JsonPropertyName("v")] public float[][] Positions { get; set; } = Array.Empty<float[]>();
        [JsonPropertyName("n")] public float[][] Normals { get; set; } = Array.Empty<float[]>();
        [JsonPropertyName("tangents")] public float[][] Tangents { get; set; } = Array.Empty<float[]>();
        [JsonPropertyName("uv")] public float[][] Uv { get; set; } = Array.Empty<float[]>();
        [JsonPropertyName("t")] public int[] Indices { get; set; } = Array.Empty<int>();
    }

    private sealed class CutFile : CutMesh
    {
        [JsonPropertyName("skirting")] public CutMesh Skirting { get; set; } = null!;
    }

    private sealed record Source(string Part, string MeshName, int VertexCount, int SubMeshCount,
        int SubMesh, uint IndexCount, string Material, Vector3 Min, Vector3 Max, int LodSlots);

    private static readonly Source Main = new("Main", "Combined Mesh (root: scene) 95", 1366, 19,
        4, 36, "hardwarestore_Main mat", new(108.75f, 0.1f, 16.5f), new(122.75f, 8.1f, 34.5f), 2);
    private static readonly Source Skirting = new("Skirting", "Combined Mesh (root: scene) 50", 63653, 507,
        102, 60, "wood_mediumbrown", new(104.89f, 0.1f, 21.64f), new(108.75f, 0.9f, 29.36f), 1);

    private sealed record Swap(Source Source, MeshRenderer Original, MeshRenderer Replacement)
    {
        internal bool Suppressed { get; set; }
    }

    private static readonly List<Mesh> OwnedMeshes = new();
    private static readonly List<Swap> Swaps = new();
    private static GameObject? _root;
    private static MeshCollider? _solidHall;
    private static LODGroup? _lodGroup;
    private static bool _lodsChanged;
    private static bool _colliderDisabled;

    // Preparation changes only inactive objects owned by this mod. Publish and shutdown
    // share one rollback path, including exceptions part way through activation.
    internal static void Prepare(GameObject hall)
    {
        Reset();
        var store = GameObject.Find(HallPlan.StoreRootPath);
        _lodGroup = store != null ? store.GetComponent<LODGroup>() : null;
        if (_lodGroup == null)
            throw new InvalidOperationException("LOD-Gruppe der Halle fehlt.");

        using var stream = typeof(HallSkin).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidDataException("Eingangsgeometrie fehlt.");
        using var sha = SHA256.Create();
        if (Convert.ToHexString(sha.ComputeHash(stream)) != ExpectedHash)
            throw new InvalidDataException("Eingangsgeometrie hat eine andere Prüfsumme.");
        stream.Position = 0;
        var cut = JsonSerializer.Deserialize<CutFile>(stream)
            ?? throw new InvalidDataException("Eingangsgeometrie ist leer.");

        // Positions and normals were baked in world space in the native static batch.
        _root = new GameObject("DowntownWarehouse_Entrance");
        _root.SetActive(false);
        PrepareRenderer(hall, Main, cut);
        var trim = hall.transform.parent.Find(Skirting.Part);
        if (trim == null)
            throw new InvalidOperationException("Sockelleiste am Laden fehlt.");
        PrepareRenderer(trim.gameObject, Skirting, cut.Skirting);
        _solidHall = HallCollision.Prepare(_root, hall, OwnedMeshes);
    }

    private static void PrepareRenderer(GameObject part, Source source, CutMesh cut)
    {
        var renderer = part.GetComponent<MeshRenderer>();
        var mesh = part.GetComponent<MeshFilter>()?.sharedMesh;
        if (renderer == null || mesh == null || !renderer.enabled || renderer.forceRenderingOff ||
            !renderer.isPartOfStaticBatch || mesh.name != source.MeshName ||
            mesh.vertexCount != source.VertexCount || mesh.subMeshCount != source.SubMeshCount ||
            renderer.subMeshStartIndex != source.SubMesh || mesh.GetIndexCount(source.SubMesh) != source.IndexCount ||
            renderer.sharedMaterials.Length != 1 || renderer.sharedMaterial == null ||
            renderer.sharedMaterial.name != source.Material ||
            Vector3.Distance(renderer.bounds.min, source.Min) > 0.04f ||
            Vector3.Distance(renderer.bounds.max, source.Max) > 0.04f)
            throw new InvalidOperationException(source.Part + ": Renderer passt nicht zur vermessenen Halle.");

        var owned = new Mesh { name = "DowntownWarehouse_" + source.Part + "Cut" };
        OwnedMeshes.Add(owned);
        owned.vertices = Array.ConvertAll(cut.Positions, p => new Vector3(p[0], p[1], p[2]));
        owned.normals = Array.ConvertAll(cut.Normals, n => new Vector3(n[0], n[1], n[2]));
        owned.tangents = Array.ConvertAll(cut.Tangents, t => new Vector4(t[0], t[1], t[2], t[3]));
        owned.uv = Array.ConvertAll(cut.Uv, uv => new Vector2(uv[0], uv[1]));
        owned.triangles = cut.Indices;
        owned.RecalculateBounds();

        var child = new GameObject(owned.name);
        child.transform.SetParent(_root!.transform, false);
        child.layer = part.layer;
        child.AddComponent<MeshFilter>().sharedMesh = owned;
        var copy = child.AddComponent<MeshRenderer>();
        copy.sharedMaterial = renderer.sharedMaterial;
        copy.shadowCastingMode = renderer.shadowCastingMode;
        copy.receiveShadows = renderer.receiveShadows;
        copy.lightProbeUsage = renderer.lightProbeUsage;
        copy.reflectionProbeUsage = renderer.reflectionProbeUsage;
        copy.probeAnchor = renderer.probeAnchor;
        copy.lightmapIndex = renderer.lightmapIndex;
        copy.lightmapScaleOffset = renderer.lightmapScaleOffset;
        copy.renderingLayerMask = renderer.renderingLayerMask;
        Swaps.Add(new Swap(source, renderer, copy));
    }

    internal static void Apply()
    {
        ReplaceLodReferences(restore: false);
        foreach (var swap in Swaps)
        {
            swap.Suppressed = true;
            swap.Original.forceRenderingOff = true;
        }
        _colliderDisabled = true;
        _solidHall!.enabled = false;
        _root!.SetActive(true);
    }

    // Work on fresh renderer arrays. Restoration replaces only references to our own
    // renderers, so changes to unrelated LOD slots during gameplay survive teardown.
    private static void ReplaceLodReferences(bool restore)
    {
        if (_lodGroup == null)
            return;
        var levels = _lodGroup.GetLODs();
        var hits = new int[Swaps.Count];
        bool changed = false;
        for (int level = 0; level < levels.Length; level++)
        {
            var lod = levels[level];
            var current = lod.renderers;
            var updated = new Renderer[current.Length];
            for (int i = 0; i < current.Length; i++)
            {
                var entry = current[i];
                updated[i] = entry;
                if (entry == null)
                    continue;
                for (int j = 0; j < Swaps.Count; j++)
                {
                    var swap = Swaps[j];
                    var from = restore ? swap.Replacement : swap.Original;
                    if (from == null || from.Pointer != entry.Pointer)
                        continue;
                    if (!restore && level != 0)
                        throw new InvalidOperationException(swap.Source.Part + ": unerwartete LOD-Stufe.");
                    updated[i] = restore ? swap.Original : swap.Replacement;
                    hits[j]++;
                    changed = true;
                }
            }
            lod.renderers = updated;
            levels[level] = lod;
        }
        if (!restore)
        {
            for (int i = 0; i < Swaps.Count; i++)
                if (hits[i] != Swaps[i].Source.LodSlots)
                    throw new InvalidOperationException(Swaps[i].Source.Part + ": unerwartete LOD-Zuordnung.");
            _lodsChanged = true;
        }
        if (changed)
            _lodGroup.SetLODs(levels);
    }

    internal static void Reset()
    {
        if (NetworkGuard.IsAlive(_root))
        {
            try { _root!.SetActive(false); } catch { }
        }
        if (_colliderDisabled && NetworkGuard.IsAlive(_solidHall))
        {
            try
            {
                if (!_solidHall!.enabled)
                    _solidHall.enabled = true;
            }
            catch { }
        }
        _colliderDisabled = false;
        foreach (var swap in Swaps)
        {
            if (swap.Suppressed && NetworkGuard.IsAlive(swap.Original))
            {
                try
                {
                    if (swap.Original.forceRenderingOff)
                        swap.Original.forceRenderingOff = false;
                }
                catch { }
            }
        }
        if (_lodsChanged && NetworkGuard.IsAlive(_lodGroup))
        {
            try { ReplaceLodReferences(restore: true); } catch { }
        }
        _lodsChanged = false;
        if (NetworkGuard.IsAlive(_root))
        {
            try { UnityEngine.Object.Destroy(_root!); } catch { }
        }
        foreach (var mesh in OwnedMeshes)
        {
            if (NetworkGuard.IsAlive(mesh))
            {
                try { UnityEngine.Object.Destroy(mesh); } catch { }
            }
        }
        OwnedMeshes.Clear();
        Swaps.Clear();
        _solidHall = null;
        _lodGroup = null;
        _root = null;
    }
}

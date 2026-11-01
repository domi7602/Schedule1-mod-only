using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using UnityEngine;

namespace TownHallInterior;

// Only the three verified renderer contributions are replaced. Shared combined meshes
// are NEVER assigned to, read through Mesh.vertices (not readable), or modified.
internal sealed class NativeFacadeSwap : IDisposable
{
    private sealed record Entry(MeshRenderer Original, MeshRenderer Replacement, Mesh OwnedMesh,
        bool Enabled, bool ForcedOff);
    private readonly List<Entry> _entries = new();
    private LODGroup? _group;
    private LOD[]? _originalLods;
    private LOD[]? _replacementLods;
    private bool _published;
    private bool _disposed;
    private CleanupJournal? _cleanup;
    internal bool CleanupComplete => _disposed;

    internal static NativeFacadeSwap Prepare(Transform ownedRoot, GameObject building)
    {
        var swap = new NativeFacadeSwap();
        try
        {
            using (var stream = File.OpenRead(Path.Combine(Application.dataPath, "level1")))
            using (var hash = SHA256.Create())
                if (Convert.ToHexString(hash.ComputeHash(stream)).ToLowerInvariant() != FacadePortal.LevelSha256)
                    throw new InvalidOperationException("Native scene hash changed; facade replacement refused");
            using var resource = typeof(NativeFacadeSwap).Assembly.GetManifestResourceStream("TownHallInterior.NativeFacade")
                ?? throw new InvalidOperationException("Local facade extraction is missing");
            using var bytes = new MemoryStream();
            resource.CopyTo(bytes);
            var document = FacadePortal.Read(bytes.ToArray());
            if (ownedRoot.position != Vector3.zero || ownedRoot.rotation != Quaternion.identity ||
                ownedRoot.lossyScale != Vector3.one)
                throw new InvalidOperationException("Baked world vertices require an identity room root");
            foreach (var source in document.Sources)
                swap.PrepareContributor(ownedRoot, building, source);
            swap.PrepareLods(building);
            return swap;
        }
        catch { swap.Dispose(); throw; }
    }

    private void PrepareContributor(Transform root, GameObject building, FacadeMesh source)
    {
        var node = building.transform.Find(source.Path)
            ?? throw new InvalidOperationException("Facade path missing: " + source.Path);
        var renderer = node.GetComponent<MeshRenderer>();
        var filter = node.GetComponent<MeshFilter>();
        var nativeMesh = filter?.sharedMesh;
        if (renderer == null || nativeMesh == null || !renderer.isPartOfStaticBatch ||
            nativeMesh.name != source.MeshName || nativeMesh.vertexCount != source.SourceVertexCount ||
            renderer.subMeshStartIndex != source.FirstSubMesh || renderer.forceRenderingOff)
            throw new InvalidOperationException("Facade renderer identity changed: " + source.Path);
        var materials = renderer.sharedMaterials;
        if (materials == null || materials.Length != source.MaterialNames.Length)
            throw new InvalidOperationException("Facade material slots changed: " + source.Path);
        for (int i = 0; i < materials.Length; i++)
            if (materials[i] == null || materials[i].name != source.MaterialNames[i])
                throw new InvalidOperationException("Facade material identity changed: " + source.Path);
        var bounds = renderer.bounds;
        for (int axis = 0; axis < 3; axis++)
            if (Math.Abs(bounds.min[axis] - source.BoundsMin[axis]) > 0.03 ||
                Math.Abs(bounds.max[axis] - source.BoundsMax[axis]) > 0.03)
                throw new InvalidOperationException("Facade world bounds changed: " + source.Path);

        var cut = FacadePortal.Cut(source);
        var mesh = new Mesh { name = "TownHallInterior.Portal." + source.Path.Replace('/', '.') };
        GameObject? clone = null;
        try
        {
            var count = cut.Vertices.Length;
            var positions = new Vector3[count]; var normals = new Vector3[count];
            var tangents = new Vector4[count]; var uv0 = new Vector2[count]; var uv1 = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                var v = cut.Vertices[i];
                positions[i] = new Vector3((float)v[0], (float)v[1], (float)v[2]);
                normals[i] = new Vector3((float)v[3], (float)v[4], (float)v[5]);
                tangents[i] = new Vector4((float)v[6], (float)v[7], (float)v[8], (float)v[9]);
                uv0[i] = new Vector2((float)v[10], (float)v[11]);
                uv1[i] = new Vector2((float)v[12], (float)v[13]);
            }
            mesh.vertices = positions; mesh.normals = normals; mesh.tangents = tangents;
            mesh.uv = uv0; mesh.uv2 = uv1;
            mesh.subMeshCount = cut.Submeshes.Length;
            for (int i = 0; i < cut.Submeshes.Length; i++) mesh.SetTriangles(cut.Submeshes[i], i, false);
            mesh.RecalculateBounds();
            clone = new GameObject(mesh.name);
            clone.SetActive(false);
            clone.transform.SetParent(root, false); // identity: vertices are already world-space
            clone.layer = node.gameObject.layer;
            clone.AddComponent<MeshFilter>().sharedMesh = mesh;
            var replacement = clone.AddComponent<MeshRenderer>();
            replacement.sharedMaterials = materials; // borrowed, never mutated
            replacement.shadowCastingMode = renderer.shadowCastingMode;
            replacement.receiveShadows = renderer.receiveShadows;
            replacement.lightmapIndex = renderer.lightmapIndex;
            replacement.lightmapScaleOffset = renderer.lightmapScaleOffset;
            replacement.renderingLayerMask = renderer.renderingLayerMask;
            replacement.lightProbeUsage = renderer.lightProbeUsage;
            replacement.reflectionProbeUsage = renderer.reflectionProbeUsage;
            replacement.probeAnchor = renderer.probeAnchor;
            _entries.Add(new Entry(renderer, replacement, mesh, renderer.enabled, renderer.forceRenderingOff));
        }
        catch
        {
            if (clone != null) UnityEngine.Object.Destroy(clone);
            UnityEngine.Object.Destroy(mesh);
            throw;
        }
    }

    private void PrepareLods(GameObject building)
    {
        var shell = building.transform.Find("townhall");
        _group = shell?.GetComponent<LODGroup>();
        if (_group == null) throw new InvalidOperationException("Native Town Hall LOD group missing");
        _originalLods = CopyLods(_group.GetLODs());
        if (_originalLods.Length != 3) throw new InvalidOperationException("Native LOD count changed");
        _replacementLods = CopyLods(_originalLods);
        var hits = new Dictionary<IntPtr, List<int>>();
        for (int level = 0; level < _replacementLods.Length; level++)
        {
            var lod = _replacementLods[level];
            var renderers = lod.renderers;
            for (int i = 0; i < renderers.Length; i++)
            {
                var current = renderers[i];
                if (current == null) continue;
                foreach (var entry in _entries)
                {
                    if (current.Pointer != entry.Original.Pointer) continue;
                    if (!hits.TryGetValue(current.Pointer, out var levels)) hits[current.Pointer] = levels = new List<int>();
                    levels.Add(level);
                    renderers[i] = entry.Replacement;
                }
            }
        }
        foreach (var entry in _entries)
        {
            var expected = entry.Original.gameObject.name == "Walls" ? new[] { 0, 1 } :
                entry.Original.transform.parent.name == "townhall" ? new[] { 1, 2 } : Array.Empty<int>();
            var actual = hits.TryGetValue(entry.Original.Pointer, out var levels) ? levels.ToArray() : Array.Empty<int>();
            if (!actual.SequenceEqual(expected)) throw new InvalidOperationException("Facade LOD attribution changed");
        }
    }

    internal void Publish()
    {
        if (_disposed || _cleanup != null || _published || _group == null || _replacementLods == null || _originalLods == null)
            throw new InvalidOperationException("Facade swap is not ready");
        if (!SameLods(_group.GetLODs(), _originalLods))
            throw new InvalidOperationException("LOD ownership changed during facade preparation");
        // Mark first so any exception is handled by the normal rollback path.
        _published = true;
        _group.SetLODs(_replacementLods);
        foreach (var entry in _entries)
        {
            entry.Original.forceRenderingOff = true;
            entry.Replacement.enabled = entry.Enabled;
            entry.Replacement.gameObject.SetActive(true);
        }
        MelonLogger.Msg("TownHallInterior: geometric portal published in Walls, root Cube and LOD Cube; shared batches untouched.");
    }

    public void Dispose()
    {
        if (_disposed) return;
        _cleanup ??= CreateCleanup();
        _disposed = _cleanup.Run((step, ex) => MelonLogger.Warning("TownHallInterior facade restore pending (" + step + "): " + ex.Message));
        if (_disposed) _entries.Clear();
    }

    private CleanupJournal CreateCleanup()
    {
        var cleanup = new CleanupJournal();
        bool lodsRestored = !_published;
        foreach (var entry in _entries)
            cleanup.Add("replacement visibility", () =>
            {
                if (entry.Replacement == null) return;
                entry.Replacement.gameObject.SetActive(false);
                entry.Replacement.enabled = false;
            });
        if (_published) cleanup.Add("LOD references", () => { RestoreLods(); lodsRestored = true; });
        foreach (var entry in _entries)
        {
            if (_published) cleanup.Add("original facade visibility", () =>
            {
                if (entry.Original != null && entry.Original.forceRenderingOff)
                    entry.Original.forceRenderingOff = entry.ForcedOff;
            });
            cleanup.Add("owned facade mesh", () =>
            {
                if (entry.Replacement != null)
                {
                    var filter = entry.Replacement.GetComponent<MeshFilter>();
                    if (filter != null) filter.sharedMesh = null;
                    UnityEngine.Object.Destroy(entry.Replacement.gameObject);
                }
                if (entry.OwnedMesh != null) UnityEngine.Object.Destroy(entry.OwnedMesh);
            }, () => lodsRestored); // Keep clone identities alive until the LOD references detach.
        }
        return cleanup;
    }

    private void RestoreLods()
    {
        if (_group == null || _originalLods == null || _replacementLods == null) return;
        var current = _group.GetLODs();
        if (SameLods(current, _originalLods)) return;
        if (SameLods(current, _replacementLods))
        {
            _group.SetLODs(_originalLods);
            return;
        }
        // Preserve foreign LOD edits and retry only our exact references after a partial failure.
        var restored = CopyLods(current);
        bool changed = false;
        foreach (var lod in restored)
            for (int i = 0; i < lod.renderers.Length; i++)
                foreach (var entry in _entries)
                    if (lod.renderers[i] != null && lod.renderers[i].Pointer == entry.Replacement.Pointer)
                    {
                        lod.renderers[i] = entry.Original;
                        changed = true;
                    }
        if (changed) _group.SetLODs(restored);
    }

    private static LOD[] CopyLods(Il2CppReferenceArray<LOD> lods)
    {
        var source = new LOD[lods.Length];
        for (int i = 0; i < source.Length; i++) source[i] = lods[i];
        return CopyLods(source);
    }

    private static LOD[] CopyLods(LOD[] source)
    {
        var copy = new LOD[source.Length];
        for (int i = 0; i < source.Length; i++)
        {
            var renderers = new Renderer[source[i].renderers.Length];
            for (int j = 0; j < renderers.Length; j++) renderers[j] = source[i].renderers[j];
            copy[i] = new LOD
            {
                screenRelativeTransitionHeight = source[i].screenRelativeTransitionHeight,
                fadeTransitionWidth = source[i].fadeTransitionWidth,
                renderers = renderers,
            };
        }
        return copy;
    }

    private static bool SameLods(Il2CppReferenceArray<LOD> actual, LOD[] expected)
    {
        if (actual.Length != expected.Length) return false;
        for (int i = 0; i < actual.Length; i++)
        {
            if (actual[i].screenRelativeTransitionHeight != expected[i].screenRelativeTransitionHeight ||
                actual[i].fadeTransitionWidth != expected[i].fadeTransitionWidth ||
                actual[i].renderers.Length != expected[i].renderers.Length) return false;
            for (int j = 0; j < actual[i].renderers.Length; j++)
                if ((actual[i].renderers[j]?.Pointer ?? IntPtr.Zero) != (expected[i].renderers[j]?.Pointer ?? IntPtr.Zero)) return false;
        }
        return true;
    }
}

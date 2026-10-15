using System;
using System.Collections.Generic;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Tiles;
using UnityEngine;
using Object = UnityEngine.Object;
using NativeDefinition = Il2CppScheduleOne.ItemFramework.BuildableItemDefinition;

namespace AutoPackagingStation.Items;

/// <summary>
/// Keeps the complete packaging network/slot graph, but takes the cauldron's
/// existing footprint as geometry. Never activates a template in the live scene.
/// Failure discards owned copies; neither original donor is modified.
/// </summary>
internal static class AutoPackPrefabComposer
{
    internal const string FootprintRootName = "AutoPack_NativeFootprint";
    private static GameObject? _cache;
    private static BuildableItem? _template;

    internal static BuildableItem Compose(NativeDefinition packaging, NativeDefinition cauldron)
    {
        // This reservation survives publication failures and registry resets.
        // Composition remains bounded to one completed template per mod lifetime.
        if (_template != null && _template.Pointer != IntPtr.Zero && _template.gameObject != null)
            return _template;
        var cache = GetCache();
        GameObject? stationCopy = null;
        GameObject? donorCopy = null;
        bool complete = false;
        try
        {
            var packagingSource = packaging.BuiltItem;
            var cauldronSource = cauldron.BuiltItem;
            if (packagingSource == null || cauldronSource == null)
                throw new InvalidOperationException("Both placed donor prefabs are required.");
            var sourceGrid = cauldronSource.TryCast<GridItem>();
            if (sourceGrid == null) throw new InvalidOperationException("Cauldron is not a GridItem.");
            int originalPackagingTiles = packagingSource.TryCast<GridItem>()?.CoordinateFootprintTilePairs?.Count ?? 0;
            int originalCauldronTiles = sourceGrid.CoordinateFootprintTilePairs?.Count ?? 0;

            stationCopy = Object.Instantiate(packagingSource.gameObject, cache.transform, false);
            donorCopy = Object.Instantiate(cauldronSource.gameObject, cache.transform, false);
            stationCopy.name = "AutoPackagingStation_PackagingNative";
            donorCopy.name = "AutoPack_CauldronFootprintSource";
            stationCopy.transform.localPosition = Vector3.zero;
            stationCopy.transform.localRotation = Quaternion.identity;
            donorCopy.transform.localPosition = Vector3.zero;
            donorCopy.transform.localRotation = Quaternion.identity;

            var station = stationCopy.GetComponent<PackagingStation>();
            var donor = donorCopy.GetComponent<GridItem>();
            if (station == null || donor == null)
                throw new InvalidOperationException("Clones must retain native PackagingStation and GridItem.");
            if (stationCopy.GetComponent<Il2CppFishNet.Object.NetworkObject>() == null)
                throw new InvalidOperationException("Packaging clone has no serialized NetworkObject.");
            var originalNetwork = packagingSource.GetComponentsInChildren<Il2CppFishNet.Object.NetworkBehaviour>(true);
            var clonedNetwork = stationCopy.GetComponentsInChildren<Il2CppFishNet.Object.NetworkBehaviour>(true);
            if (originalNetwork.Length != clonedNetwork.Length)
                throw new InvalidOperationException("Packaging network behaviour layout changed during cloning.");

            var pairs = donor.CoordinateFootprintTilePairs;
            var sourcePairs = sourceGrid.CoordinateFootprintTilePairs;
            var targetPairs = station.CoordinateFootprintTilePairs;
            if (pairs == null || sourcePairs == null || targetPairs == null || pairs.Pointer == sourcePairs.Pointer)
                throw new InvalidOperationException("Footprint list is missing or still aliases the original donor.");
            var originalTargetPairs = packagingSource.TryCast<GridItem>()?.CoordinateFootprintTilePairs;
            if (originalTargetPairs == null || targetPairs.Pointer == originalTargetPairs.Pointer)
                throw new InvalidOperationException("Packaging footprint list was not privately cloned.");

            var cells = new List<(int X, int Y)>();
            var tiles = new HashSet<IntPtr>();
            var roots = new Dictionary<IntPtr, Transform>();
            Vector3 center = Vector3.zero;
            for (int i = 0; i < pairs.Count; i++)
            {
                var pair = pairs[i];
                var tile = pair.footprintTile;
                if (tile == null || pair.coord == null || !tiles.Add(tile.Pointer))
                    throw new InvalidOperationException("Footprint has missing or duplicate tile references.");
                if (tile.X != pair.coord.x || tile.Y != pair.coord.y)
                    throw new InvalidOperationException("Native tile coordinates disagree with the mapping.");
                cells.Add((pair.coord.x, pair.coord.y));
                center += donor.transform.InverseTransformPoint(tile.transform.position);
                CollectTileRoots(tile, donor.transform, roots);
            }
            if (!FootprintPolicy.IsCompleteFourByFour(cells))
                throw new InvalidOperationException($"Cauldron footprint is not a complete native 4x4 mapping ({pairs.Count} pairs).");
            center /= pairs.Count;
            // Translate the entire native grid once to the centered 2m model. Relative
            // tile positions, coordinates, offsets and all corner dependencies stay intact.
            var offset = new Vector3(center.x, 0f, center.z);
            var buildPoint = donor.BuildPoint;
            if (buildPoint == null || station.BuildPoint == null)
                throw new InvalidOperationException("Both donors need a native ground BuildPoint.");
            if (!buildPoint.IsChildOf(donor.transform) || !station.BuildPoint.IsChildOf(station.transform))
                throw new InvalidOperationException("BuildPoint reference was not privately remapped.");
            if (stationCopy.transform.localScale != Vector3.one || donorCopy.transform.localScale != Vector3.one)
                throw new InvalidOperationException("Donor root scaling requires a separate measured transplant.");
            if (float.IsNaN(center.x) || float.IsInfinity(center.x) || float.IsNaN(center.z) || float.IsInfinity(center.z))
                throw new InvalidOperationException("Footprint positions are not finite.");
            Vector3 buildPointPosition = donor.transform.InverseTransformPoint(buildPoint.position) - offset;
            Quaternion buildPointRotation = Quaternion.Inverse(donor.transform.rotation) * buildPoint.rotation;

            foreach (var root in roots.Values) ValidateGeometryOnly(root);
            // Old packaging tiles stay on the owned template only, disabled individually.
            // Never disable a parent that might also carry slot/network references.
            for (int i = 0; i < targetPairs.Count; i++)
            {
                var tile = targetPairs[i].footprintTile;
                if (tile == null || !tile.transform.IsChildOf(station.transform) || tile.transform.Pointer == station.transform.Pointer)
                    throw new InvalidOperationException("Packaging footprint is not owned by its clone.");
                ValidateGeometryOnly(tile.transform);
                tile.gameObject.SetActive(false);
            }

            var footprintRoot = new GameObject(FootprintRootName);
            footprintRoot.transform.SetParent(station.transform, false);
            foreach (var root in roots.Values)
            {
                Vector3 pos = donor.transform.InverseTransformPoint(root.position) - offset;
                Quaternion rot = Quaternion.Inverse(donor.transform.rotation) * root.rotation;
                root.SetParent(footprintRoot.transform, false);
                root.localPosition = pos;
                root.localRotation = rot;
            }
            // Assign the private cloned list wholesale. This does not fabricate tiles,
            // coordinates or native list contents, and preserves native pair ordering.
            station.CoordinateFootprintTilePairs = pairs;
            // OriginFootprint is derived by the native getter from the replaced mapping.
            if (station.OriginFootprint == null || !station.OriginFootprint.transform.IsChildOf(footprintRoot.transform))
                throw new InvalidOperationException("Native origin footprint did not remap into the cloned grid.");
            station.BuildPoint.position = station.transform.TransformPoint(buildPointPosition);
            station.BuildPoint.rotation = station.transform.rotation * buildPointRotation;

            for (int i = 0; i < pairs.Count; i++)
            {
                var tile = pairs[i].footprintTile;
                ValidateMovedTile(tile, footprintRoot.transform);
            }
            if (stationCopy.GetComponentsInChildren<Il2CppFishNet.Object.NetworkBehaviour>(true).Length != originalNetwork.Length)
                throw new InvalidOperationException("Footprint transplant introduced a network behaviour.");
            if (originalPackagingTiles != originalTargetPairs.Count || originalCauldronTiles != sourcePairs.Count)
                throw new InvalidOperationException("Original donor footprint changed.");

            // activeSelf=true, inactiveInHierarchy: native Instantiate consumers receive
            // an active clone, but Awake/Start never run on this persistent template.
            stationCopy.SetActive(true);
            complete = true;
            _template = station;
            Mod.Log.Info($"[autopack-prefab] native PackagingStation preserved; cauldron footprint={pairs.Count} (4x4), networkBehaviours={originalNetwork.Length}, donorTiles unchanged={originalPackagingTiles}/{originalCauldronTiles}, templateActiveInHierarchy={stationCopy.activeInHierarchy}.");
            return station;
        }
        finally
        {
            if (donorCopy != null) Object.Destroy(donorCopy);
            if (!complete && stationCopy != null) Object.Destroy(stationCopy);
        }
    }

    internal static void Shutdown()
    {
        _template = null;
        var cache = _cache;
        _cache = null;
        if (cache != null && cache.Pointer != IntPtr.Zero) Object.Destroy(cache);
    }

    private static GameObject GetCache()
    {
        if (_cache != null && _cache.Pointer != IntPtr.Zero) return _cache;
        _cache = new GameObject("AutoPack_InactivePrefabCache");
        _cache.SetActive(false);
        Object.DontDestroyOnLoad(_cache);
        return _cache;
    }

    private static void CollectTileRoots(FootprintTile tile, Transform donor, Dictionary<IntPtr, Transform> roots)
    {
        CollectRoot(tile.transform, donor, roots);
        var appearance = tile.tileAppearance;
        if (appearance != null)
        {
            CollectRoot(appearance.transform, donor, roots);
            if (appearance.tileMesh != null) CollectRoot(appearance.tileMesh.transform, donor, roots);
        }
        if (tile.tileDetector != null) CollectRoot(tile.tileDetector.transform, donor, roots);
        var corners = tile.Corners;
        if (corners == null) return;
        for (int i = 0; i < corners.Count; i++)
        {
            var corner = corners[i];
            if (corner == null) throw new InvalidOperationException("Native footprint corner reference is missing.");
            CollectRoot(corner.transform, donor, roots);
            if (corner.parentFootprint != null && corner.parentFootprint.Pointer != tile.Pointer)
                throw new InvalidOperationException("Corner retains a foreign footprint reference.");
        }
    }

    private static void CollectRoot(Transform child, Transform donor, Dictionary<IntPtr, Transform> roots)
    {
        if (child.Pointer == donor.Pointer || !child.IsChildOf(donor))
            throw new InvalidOperationException("Footprint dependency points outside the owned donor clone.");
        var root = child;
        while (root.parent != null && root.parent.Pointer != donor.Pointer) root = root.parent;
        roots[root.Pointer] = root;
    }

    private static void ValidateGeometryOnly(Transform root)
    {
        if (root.GetComponentsInChildren<Il2CppFishNet.Object.NetworkBehaviour>(true).Length != 0 ||
            root.GetComponentsInChildren<Il2CppFishNet.Object.NetworkObject>(true).Length != 0)
            throw new InvalidOperationException($"Footprint subtree '{root.name}' contains networking.");
        var scripts = root.GetComponentsInChildren<MonoBehaviour>(true);
        for (int i = 0; i < scripts.Length; i++)
        {
            var script = scripts[i];
            // Vanilla footprint presentation also carries this ghost-material marker.
            // Preserve it, while rejecting networking and unknown machine behaviour.
            if (script.TryCast<FootprintTile>() != null || script.TryCast<TileDetector>() != null ||
                script.TryCast<TileAppearance>() != null || script.TryCast<Il2CppScheduleOne.Building.CornerObstacle>() != null ||
                script.TryCast<Il2CppScheduleOne.Building.OverrideGhostMaterial>() != null)
                continue;
            throw new InvalidOperationException($"Footprint subtree '{root.name}' contains non-footprint behaviour '{script.GetIl2CppType().FullName}'.");
        }
    }

    private static void ValidateMovedTile(FootprintTile tile, Transform footprintRoot)
    {
        if (!tile.transform.IsChildOf(footprintRoot)) throw new InvalidOperationException("Tile did not move into the owned grid.");
        if (tile.tileDetector != null && !tile.tileDetector.transform.IsChildOf(footprintRoot))
            throw new InvalidOperationException("Tile detector did not move into the owned grid.");
        var appearance = tile.tileAppearance;
        if (appearance != null && (!appearance.transform.IsChildOf(footprintRoot) ||
            (appearance.tileMesh != null && !appearance.tileMesh.transform.IsChildOf(footprintRoot))))
            throw new InvalidOperationException("Tile appearance did not move into the owned grid.");
        var corners = tile.Corners;
        if (corners == null) return;
        for (int i = 0; i < corners.Count; i++)
            if (corners[i] == null || !corners[i].transform.IsChildOf(footprintRoot))
                throw new InvalidOperationException("Tile corner did not move into the owned grid.");
    }
}

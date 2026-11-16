using System;
using MelonLoader;
using UnityEngine;

namespace NorthIsland;

/// <summary>
/// Coordinates building and tearing down the North Island bridge connection and barrier removal.
/// </summary>
internal sealed class NorthIslandWorld
{
    internal const string RootName = "NorthIsland_Root";

    private GameObject? _root;
    internal BridgeBuilder Bridge { get; } = new();
    internal TownBuilder Town { get; } = new();
    internal BarrierRemover Barriers { get; } = new();
    internal bool IsBuilt => _root != null;

    internal void Build()
    {
        if (_root != null) Teardown();

        EnsureBroadphaseWorldBounds();

        _root = new GameObject(RootName);
        Bridge.Build(_root.transform);
        Town.Build(_root.transform);
        Barriers.DisableBlockingObjects();
        Physics.SyncTransforms();

        MelonLogger.Msg(
            $"[NorthIsland] Welt bereit: {Bridge.ProceduralSegmentCount} Overpass-Brueckensegmente" +
            (Bridge.UsedProceduralFallback ? " (Fallback aktiv)" : "") +
            $", Northshore Town ({Town.BuildingCount} Gebaeude, {Town.StreetlightCount} Laternen, {Town.TreeCount} Baeume)" +
            $", {Barriers.DisabledObjectCount} Barrieren + {Barriers.DisabledColliderCount} Boundary-Collider entfernt.");
    }

    /// <summary>
    /// Expands Unity/PhysX MultiboxPruningBroadphase regions to include North Island (z &gt; 250)
    /// so dynamic Rigidbody colliders (such as LandVehicle) retain full contact detection off-map.
    /// </summary>
    private static void EnsureBroadphaseWorldBounds()
    {
        var bounds = new Bounds(
            new Vector3(
                NorthIslandGeometry.BroadphaseCenterX,
                NorthIslandGeometry.BroadphaseCenterY,
                NorthIslandGeometry.BroadphaseCenterZ),
            new Vector3(
                NorthIslandGeometry.BroadphaseSizeX,
                NorthIslandGeometry.BroadphaseSizeY,
                NorthIslandGeometry.BroadphaseSizeZ));
        try
        {
            Physics.RebuildBroadphaseRegions(bounds, NorthIslandGeometry.BroadphaseSubdivisions);
            MelonLogger.Msg(
                $"[NorthIsland] Physics-Broadphase erweitert: center={bounds.center}, size={bounds.size}, subdiv={NorthIslandGeometry.BroadphaseSubdivisions}");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[NorthIsland] Physics.RebuildBroadphaseRegions fehlgeschlagen: {ex.Message}");
        }
    }

    internal void Teardown()
    {
        Barriers.RestoreAll();

        if (_root != null)
        {
            UnityEngine.Object.Destroy(_root);
            _root = null;
        }

        Bridge.DisposeAssets();
        Town.DisposeAssets();
    }
}

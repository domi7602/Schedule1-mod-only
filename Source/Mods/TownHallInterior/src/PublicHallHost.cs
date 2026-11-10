using System;
using System.Collections.Generic;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using UnityEngine;

namespace TownHallInterior;

// Builds inactive, then publishes the collider opening and attributed facade replacements.
// Native shared meshes and the NPC building identity are never changed.
internal sealed class PublicHallHost : IDisposable
{
    private readonly UnityRoomSink _sink;
    private readonly List<(Collider Collider, bool Enabled)> _journal = new();
    private readonly List<(Renderer Renderer, bool Enabled, bool ForcedOff)> _renderers = new();
    private readonly List<Material> _materials = new();
    private NativeFacadeSwap? _facade;
    private VanillaFurnishings? _furnishings;
    private Material? _leafPanel;
    private Material? _leafFrame;
    private Material? _casingTrim;
    private Transform? _south;
    private Transform? _north;
    private float _open;
    private bool _disposed;
    private CleanupJournal? _cleanup;
    internal bool CleanupComplete => _disposed;

    private PublicHallHost(UnityRoomSink sink) => _sink = sink;

    internal static PublicHallHost? TryOpen(IRoomSink sink)
    {
        if (sink is not UnityRoomSink room || room.Root == null) return null;
        var host = new PublicHallHost(room);
        try
        {
            var building = Find(NativeSceneSwap.BuildingPath)
                ?? throw new InvalidOperationException("Town Hall building is missing");
            host._facade = NativeFacadeSwap.Prepare(room.Root, building);
            host.CreateLeaves();
            host.Tick(0); // place leaves before the root can become visible/collidable
            if (!host.SwapColliders()) { host.Dispose(); host.Dispose(); return null; }
            host.HideDoorMesh();
            host.HideNativeDoorSurface();
            host._facade.Publish();
            room.Publish();
            host.Furnish();
            HallGrid.TryPublish(room.Root!);
            MelonLogger.Msg("TownHallInterior: hall and geometric facade opening published; native knock retained.");
            return host;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior public hall refused: " + ex.Message);
            host.Dispose(); host.Dispose(); // One bounded retry of failed steps only.
            return null;
        }
    }

    private void Furnish()
    {
        _furnishings = new VanillaFurnishings();
        try { _furnishings.PrepareAndPublish(_sink.Root!); }
        catch (Exception ex)
        {
            // Optional furnishing failure must not close the accepted public entrance.
            try { MelonLogger.Warning("TownHallInterior furniture unavailable; entrance retained: " + ex.Message); }
            catch { }
            _furnishings.Dispose();
            _furnishings.Dispose();
            // Retain incomplete cleanup for the host's scene-exit retry.
        }
    }

    internal void Tick(float deltaSeconds)
    {
        if (_disposed || _cleanup != null || _south == null || _north == null) return;
        _open = PublicEntrance.MoveLeaf(_open, PlayerAtDoor(), deltaSeconds);
        _south.localPosition = new Vector3(PublicEntrance.LeafCenterX, PublicEntrance.LeafCenterY,
            PublicEntrance.LeafSouthZ(_open));
        _north.localPosition = new Vector3(PublicEntrance.LeafCenterX, PublicEntrance.LeafCenterY,
            PublicEntrance.LeafNorthZ(_open));
    }

    public void Dispose()
    {
        if (_disposed) return;
        Restore();
    }

    private bool SwapColliders()
    {
        var building = Find(NativeSceneSwap.BuildingPath);
        var npc = building?.GetComponent<NPCEnterableBuilding>();
        if (building == null || npc == null ||
            !string.Equals(npc.BakedGUID, NativeSceneSwap.BuildingGuid, StringComparison.OrdinalIgnoreCase))
        {
            MelonLogger.Warning("TownHallInterior: Town Hall path or GUID did not match.");
            return false;
        }
        var guidCount = CountGuid(NativeSceneSwap.BuildingGuid);
        if (guidCount != 1)
        {
            MelonLogger.Warning("TownHallInterior: expected one Town Hall GUID, found " + guidCount);
            return false;
        }

        var seen = new List<NativeColliderObservation>();
        foreach (var expected in NativeSceneSwap.Candidates)
        {
            var go = Find(expected.Path);
            var collider = MatchingCollider(go, expected.Kind);
            if (go == null)
            {
                MelonLogger.Warning("TownHallInterior: object missing at " + expected.Path);
                return false;
            }
            if (collider == null)
            {
                MelonLogger.Warning("TownHallInterior: " + expected.Kind + " missing at " + expected.Path);
                return false;
            }
            var bounds = collider.bounds;
            seen.Add(new NativeColliderObservation(expected.Path, expected.Kind,
                bounds.min.x, bounds.min.y, bounds.min.z, bounds.max.x, bounds.max.y, bounds.max.z,
                collider.enabled, collider.isTrigger, ColliderCount(go)));
        }
        var inspection = NativeSceneSwap.Inspect(new NativeSceneObservation(
            NativeSceneSwap.BuildingPath, NativeSceneSwap.BuildingGuid, seen, 1));
        if (!PublicEntrance.ColliderSwapAllowed(inspection))
        {
            MelonLogger.Warning("TownHallInterior: collider identity refused: " + inspection.Reason);
            return false;
        }

        foreach (var path in PublicEntrance.PreserveColliders)
            if (Find(path) == null) return false;
        var door = Find(NativeSceneSwap.BuildingPath + "/StaticDoor");
        if (door == null || door.GetComponent<Il2CppScheduleOne.Doors.StaticDoor>() == null) return false;

        foreach (var path in PublicEntrance.DisableColliders)
        {
            var expected = Expected(path);
            var collider = MatchingCollider(Find(path), expected.Kind);
            if (collider == null) return false;
            _journal.Add((collider, collider.enabled));
            collider.enabled = false;
        }
        return true;
    }

    private void CreateLeaves()
    {
        _leafPanel = Paint(new Color(0.78f, 0.76f, 0.71f));
        _leafFrame = Paint(new Color(0.29f, 0.31f, 0.34f));
        _casingTrim = Paint(new Color(0.42f, 0.44f, 0.47f));
        _south = Leaf("sliding-south");
        _north = Leaf("sliding-north");
        Floor();
        DoorCasing();
    }

    // Each leaf is a panel plus a dark stile/rail border, sized from PublicEntrance.LeafParts so
    // the part faces cannot become coplanar. Only the panel keeps a collider; the trim is
    // decorative and must not add collision beside the walkway.
    private Transform Leaf(string name)
    {
        var root = new GameObject(name).transform;
        root.SetParent(_sink.Root, false);
        foreach (var part in PublicEntrance.LeafParts)
            Part(root, part.Name, new Vector3(0f, part.OffsetY, part.OffsetZ),
                new Vector3(part.ThicknessX, part.Height, part.WidthZ),
                part.Name == "panel" ? _leafPanel : _leafFrame, part.Solid);
        return root;
    }

    // Finished inner jamb around the portal: covers the leaf/reveal joint without
    // narrowing the walkway collision, which stays owned by the accepted portal.
    private void DoorCasing()
    {
        const float face = 54.95f, depth = 0.05f;
        CasingBox("casing-south", 29.90f, PublicEntrance.FloorTop, 30.06f, PublicEntrance.HeadLiningBottom);
        CasingBox("casing-north", 31.94f, PublicEntrance.FloorTop, 32.10f, PublicEntrance.HeadLiningBottom);
        CasingBox("casing-head", 29.90f, PublicEntrance.HeadLiningBottom, 32.10f,
            PublicEntrance.HeadLiningBottom + 0.08f);

        void CasingBox(string name, float z1, float y1, float z2, float y2)
        {
            Part(_sink.Root!, name, new Vector3(face + depth / 2f, (y1 + y2) / 2f, (z1 + z2) / 2f),
                new Vector3(depth, y2 - y1, z2 - z1), _casingTrim, false);
        }
    }

    private void Part(Transform parent, string name, Vector3 localPosition, Vector3 scale,
        Material? material, bool solid)
    {
        var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        try
        {
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = scale;
            var renderer = part.GetComponent<Renderer>();
            if (renderer != null && material != null) renderer.sharedMaterial = material;
            if (!solid) UnityEngine.Object.Destroy(part.GetComponent<Collider>());
        }
        catch
        {
            UnityEngine.Object.Destroy(part);
            throw;
        }
    }

    private void Floor()
    {
        foreach (var box in PublicEntrance.CreateInterior())
        {
            var child = GameObject.CreatePrimitive(PrimitiveType.Cube);
            child.name = box.Name;
            child.transform.SetParent(_sink.Root, false);
            child.transform.localPosition = new Vector3(box.X, box.Y, box.Z);
            child.transform.localScale = new Vector3(box.Width, box.Height, box.Depth);
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null) renderer.sharedMaterial = Paint(box.Style == 0
                ? new Color(0.72f, 0.68f, 0.60f) : new Color(0.78f, 0.74f, 0.66f));
        }
    }

    private Material Paint(Color color)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        var material = new Material(shader);
        material.color = color;
        _materials.Add(material);
        return material;
    }

    private void HideDoorMesh()
    {
        foreach (var suffix in new[] { "/townhall/Door", "/townhall/Door/Glass", "/townhall/Door/Quad" })
        {
            var renderer = Find(NativeSceneSwap.BuildingPath + suffix)?.GetComponent<MeshRenderer>()
                ?? throw new InvalidOperationException("Native door contribution missing: " + suffix);
            _renderers.Add((renderer, renderer.enabled, renderer.forceRenderingOff));
            renderer.forceRenderingOff = true;
        }
        MelonLogger.Msg("TownHallInterior: three exact door contributions hidden (including distant billboard).");
    }

    private void HideNativeDoorSurface()
    {
        // Only the visible contributions are replaced. Knock and usability stay native: NPCs
        // despawn while they are inside, and a knock is what brings them back, so clearing
        // CanKnock removed the owner's only way to reach the clerk (readback 2026-10-29 20:5x:
        // CanKnock=false while Usable=true).
        var doorObject = Find(NativeSceneSwap.BuildingPath + "/StaticDoor");
        if (doorObject == null) return;
        foreach (var renderer in doorObject.GetComponentsInChildren<Renderer>(true))
        {
            _renderers.Add((renderer, renderer.enabled, renderer.forceRenderingOff));
            renderer.enabled = false;
            renderer.forceRenderingOff = true;
        }
        MelonLogger.Msg("TownHallInterior: native door mesh hidden; knock and usability retained.");
    }


    private bool PlayerAtDoor()
    {
        var player = Player.Local;
        if (player == null) return false;
        var p = player.transform.position;
        return PublicEntrance.WantsDoorOpen(p.x, p.y, p.z);
    }

    private void Restore()
    {
        _cleanup ??= CreateCleanup();
        _disposed = _cleanup.Run((step, ex) => MelonLogger.Warning("TownHallInterior hall restore pending (" + step + "): " + ex.Message));
        if (!_disposed) return;
        _journal.Clear();
        _renderers.Clear();
        _materials.Clear();
    }

    private CleanupJournal CreateCleanup()
    {
        var cleanup = new CleanupJournal();
        cleanup.Add("furniture", () =>
        {
            _furnishings?.Dispose();
            if (_furnishings != null && !_furnishings.CleanupComplete)
                throw new InvalidOperationException("Furniture cleanup has pending steps");
            _furnishings = null;
        });
        cleanup.Add("hall grid", HallGrid.Dispose);
        cleanup.Add("facade", () =>
        {
            _facade?.Dispose();
            if (_facade != null && !_facade.CleanupComplete)
                throw new InvalidOperationException("Facade restoration has pending steps");
            _facade = null;
        });
        for (int i = _journal.Count - 1; i >= 0; i--)
        {
            var entry = _journal[i];
            cleanup.Add("native collider", () => { if (entry.Collider != null) entry.Collider.enabled = entry.Enabled; });
        }
        for (int i = _renderers.Count - 1; i >= 0; i--)
        {
            var entry = _renderers[i];
            cleanup.Add("native door renderer", () =>
            {
                if (entry.Renderer == null) return;
                entry.Renderer.forceRenderingOff = entry.ForcedOff;
                entry.Renderer.enabled = entry.Enabled;
            });
        }
        foreach (var material in _materials)
            cleanup.Add("owned hall material", () => { if (material != null) UnityEngine.Object.Destroy(material); });
        return cleanup;
    }

    private static NativeColliderObservation Expected(string path)
    {
        foreach (var candidate in NativeSceneSwap.Candidates)
            if (candidate.Path == path) return candidate;
        throw new InvalidOperationException(path);
    }

    private static Collider? MatchingCollider(GameObject? go, string kind)
    {
        if (go == null) return null;
        Collider? found = kind switch
        {
            "BoxCollider" => go.GetComponent<BoxCollider>(),
            "MeshCollider" => go.GetComponent<MeshCollider>(),
            _ => null,
        };
        return found;
    }

    private static int ColliderCount(GameObject go)
    {
        int count = 0;
        if (go.GetComponent<BoxCollider>() != null) count++;
        if (go.GetComponent<MeshCollider>() != null) count++;
        if (go.GetComponent<CapsuleCollider>() != null) count++;
        if (go.GetComponent<SphereCollider>() != null) count++;
        return count;
    }

    private static int CountGuid(string guid)
    {
        int count = 0;
        foreach (var building in UnityEngine.Object.FindObjectsOfType<NPCEnterableBuilding>())
            if (building != null && string.Equals(building.BakedGUID, guid, StringComparison.OrdinalIgnoreCase))
                count++;
        return count;
    }

    internal static GameObject? Find(string path)
    {
        var parts = path.Split('/');
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        foreach (var root in scene.GetRootGameObjects())
        {
            var found = Search(root.transform, parts);
            if (found != null) return found.gameObject;
        }
        return null;
    }

    private static Transform? Search(Transform node, string[] parts)
    {
        if (node.name == parts[0])
        {
            var rest = Match(node, parts, 0);
            if (rest != null) return rest;
        }
        for (int i = 0; i < node.childCount; i++)
        {
            var found = Search(node.GetChild(i), parts);
            if (found != null) return found;
        }
        return null;
    }

    private static Transform? Match(Transform node, string[] parts, int index)
    {
        if (node.name != parts[index]) return null;
        if (index == parts.Length - 1) return node;
        for (int i = 0; i < node.childCount; i++)
        {
            var found = Match(node.GetChild(i), parts, index + 1);
            if (found != null) return found;
        }
        return null;
    }
}

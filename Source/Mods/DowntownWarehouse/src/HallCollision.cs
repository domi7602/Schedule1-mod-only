using System;
using System.Collections.Generic;
using UnityEngine;

namespace DowntownWarehouse;

/// <summary>Replaces the solid convex Main hull with enclosed wall and roof sections.</summary>
internal static class HallCollision
{
    private const float West = HallPlan.CenterX - HallPlan.WidthX / 2;
    private const float East = HallPlan.CenterX + HallPlan.WidthX / 2;
    private const float South = HallPlan.CenterZ - HallPlan.LengthZ / 2;
    private const float North = HallPlan.CenterZ + HallPlan.LengthZ / 2;
    private const float Floor = HallPlan.FloorTopY;
    private const float Ridge = HallPlan.CenterY + HallPlan.HeightY / 2;
    private const float Eave = 6.1f;
    private const float Thickness = 0.12f;
    private const float DoorMinZ = 21.86f;
    private const float DoorMaxZ = 23.14f;
    private const float DoorTop = 2.32f;

    internal static MeshCollider Prepare(GameObject root, GameObject main, List<Mesh> ownedMeshes)
    {
        if (root.activeSelf || root.transform.position != Vector3.zero
            || root.transform.rotation != Quaternion.identity || root.transform.lossyScale != Vector3.one)
            throw new InvalidOperationException("Kollisionsaufbau benötigt eine inaktive Wurzel im Weltursprung.");

        var native = main.GetComponent<MeshCollider>();
        if (native == null || main.GetComponents<Collider>().Length != 1 || main.layer != 0
            || !native.enabled || !main.activeInHierarchy || native.isTrigger || !native.convex)
            throw new InvalidOperationException("Unerwarteter nativer Main-Kollider.");

        // Main's collider references an unbatched local mesh. The renderer uses a different mesh.
        var mesh = native.sharedMesh;
        if (mesh == null || mesh.name != "Main" || mesh.vertexCount != 20 || mesh.subMeshCount != 1
            || mesh.GetTopology(0) != MeshTopology.Triangles || mesh.GetIndexCount(0) != 36
            || !Matches(mesh.bounds, new(0f, 0f, 1.5f), new(14f, 18f, 8f))
            || !Matches(native.bounds, new(HallPlan.CenterX, HallPlan.CenterY, HallPlan.CenterZ),
                new(HallPlan.WidthX, HallPlan.HeightY, HallPlan.LengthZ)))
            throw new InvalidOperationException("Main-Kollisionsmesh oder Hallenmaße weichen von der f12-Vorlage ab.");

        var material = native.sharedMaterial;
        GameObject Part(string name)
        {
            var part = new GameObject("Collision_" + name);
            part.transform.SetParent(root.transform, false);
            part.layer = main.layer;
            return part;
        }

        void Box(string name, Vector3 min, Vector3 max)
        {
            var box = Part(name).AddComponent<BoxCollider>();
            box.center = (min + max) * 0.5f;
            box.size = max - min;
            box.sharedMaterial = material;
        }

        void Prism(string name, float x, float z, Mesh shape)
        {
            var part = Part(name);
            part.transform.localPosition = new Vector3(x, Eave - Thickness, z);
            var collider = part.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.sharedMesh = shape;
            collider.sharedMaterial = material;
        }

        // All thickness lies inside the original hull. Foundation remains the floor.
        float wallTop = Eave - Thickness;
        Box("WestSouth", new(West, Floor, South), new(West + Thickness, wallTop, DoorMinZ));
        Box("WestNorth", new(West, Floor, DoorMaxZ), new(West + Thickness, wallTop, North));
        Box("WestLintel", new(West, DoorTop, DoorMinZ), new(West + Thickness, wallTop, DoorMaxZ));
        Box("East", new(East - Thickness, Floor, South), new(East, wallTop, North));
        Box("South", new(West, Floor, South), new(East, Eave, South + Thickness));
        Box("North", new(West, Floor, North - Thickness), new(East, Eave, North));

        // Prism profiles use (Z, Y) relative to the eave underside at the southern corner.
        const float halfRun = HallPlan.LengthZ / 2;
        const float rise = Ridge - Eave;
        var gable = MakePrism("Gable", Thickness, ownedMeshes,
            new(0f, 0f), new(HallPlan.LengthZ, 0f), new(halfRun, rise));
        Prism("GableWest", West, South, gable);
        Prism("GableEast", East - Thickness, South, gable);

        // The two walkable top faces meet on the exact original ridge at Y=8.1.
        var southRoof = MakePrism("SouthRoof", HallPlan.WidthX, ownedMeshes,
            new(0f, 0f), new(halfRun, rise), new(halfRun, rise + Thickness), new(0f, Thickness));
        var northRoof = MakePrism("NorthRoof", HallPlan.WidthX, ownedMeshes,
            new(0f, rise), new(halfRun, 0f), new(halfRun, Thickness), new(0f, rise + Thickness));
        Prism("RoofSouth", West, South, southRoof);
        Prism("RoofNorth", West, HallPlan.CenterZ, northRoof);
        return native;
    }

    private static bool Matches(Bounds bounds, Vector3 center, Vector3 size)
    {
        const float tolerance = 0.02f;
        return (bounds.center - center).sqrMagnitude <= tolerance * tolerance
            && (bounds.size - size).sqrMagnitude <= tolerance * tolerance;
    }

    // The convex polygon is counterclockwise in (Z,Y), so its first face points west.
    private static Mesh MakePrism(string name, float width, List<Mesh> ownedMeshes, params Vector2[] profile)
    {
        int count = profile.Length;
        var vertices = new Vector3[count * 2];
        var triangles = new List<int>();
        for (int i = 0; i < count; i++)
        {
            vertices[i] = new(0f, profile[i].y, profile[i].x);
            vertices[i + count] = new(width, profile[i].y, profile[i].x);
            int next = (i + 1) % count;
            triangles.AddRange(new[] { i, i + count, next + count, i, next + count, next });
        }
        for (int i = 1; i < count - 1; i++)
            triangles.AddRange(new[] { 0, i, i + 1, count, count + i + 1, count + i });

        var mesh = new Mesh { name = "DowntownWarehouse_" + name + "Collision" };
        ownedMeshes.Add(mesh);
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        return mesh;
    }
}

using System;
using System.Collections.Generic;
using PropertyLEDs.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace PropertyLEDs;

/// <summary>
/// Turns one validated room layout into a SINGLE owned Mesh (one path-independent draw call per room).
/// No colliders, no primitive GameObjects, no per-segment objects. Geometry decisions live in
/// <see cref="GeometryMath"/> so the beam math stays testable without Unity.
/// </summary>
internal static class LEDGeometryBuilder
{
    // Unity's 16-bit index buffer tops out at 65535 vertices; switch to 32-bit well before that.
    private const int UInt16VertexBudget = 60000;

    /// <summary>
    /// Builds the room's LED beam mesh in anchor-local space. Returns null when the room has no
    /// usable geometry, so callers must treat null as "not renderable".
    /// </summary>
    internal static Mesh? Build(RoomLayout room, bool preview)
    {
        if (room == null || room.Paths == null || room.Paths.Count == 0) return null;
        float width = room.Width;
        var points = new List<Vector3>();
        var indices = new List<int>();
        int usedPaths = 0;
        for (int i = 0; i < room.Paths.Count; i++)
        {
            var path = room.Paths[i];
            if (path == null) continue;
            MeshData data;
            try { data = GeometryMath.GenerateBeams(path, width); }
            catch (Exception ex) { throw new InvalidOperationException($"beam math failed on path {i}: {ex.Message}"); }
            if (data.Vertices.Length == 0 || data.Triangles.Length == 0) continue;
            int offset = points.Count;
            for (int v = 0; v < data.Vertices.Length; v++)
            {
                var point = data.Vertices[v];
                if (point == null) throw new InvalidOperationException($"path {i} produced a null vertex");
                points.Add(new Vector3(point.X, point.Y, point.Z));
            }
            for (int t = 0; t < data.Triangles.Length; t++) indices.Add(data.Triangles[t] + offset);
            usedPaths++;
        }
        if (usedPaths == 0 || points.Count < 3 || indices.Count < 3) return null;

        var mesh = new Mesh { name = preview ? $"PropertyLEDs_Preview_{room.RoomId}" : $"PropertyLEDs_{room.RoomId}" };
        try
        {
            // Arrays (not Il2Cpp collection types) — Il2CppInterop marshals them directly.
            if (points.Count > UInt16VertexBudget) mesh.indexFormat = IndexFormat.UInt32;
            mesh.vertices = points.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
        catch
        {
            UnityEngine.Object.Destroy(mesh);
            throw;
        }
    }
}

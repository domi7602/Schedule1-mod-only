using System;
using System.Collections.Generic;
using System.Numerics;

namespace PropertyLEDs.Core
{
    public sealed class MeshData
    {
        public LEDPoint[] Vertices { get; }
        public int[] Triangles { get; }
        public MeshData(LEDPoint[] vertices, int[] triangles) { Vertices = vertices; Triangles = triangles; }
        public static MeshData Empty => new MeshData(Array.Empty<LEDPoint>(), Array.Empty<int>());
    }

    public static class GeometryMath
    {
        /// <summary>
        /// Builds one square beam per path segment. This geometry matches the accepted reference look:
        /// independent segment beams that extend width/2 past BOTH endpoints, so neighbouring beams
        /// overlap at every corner and the strip stays visibly continuous around it (no miter seam).
        /// Width is the beam's square cross-section side in anchor-local metres.
        /// Invalid inputs return an empty mesh.
        /// </summary>
        public static MeshData GenerateBeams(LEDPath? path, float width)
        {
            if (path == null || !path.Validate(out _) || !float.IsFinite(width) || width < 0.001f || width > 1f)
                return MeshData.Empty;
            int count = path.Points.Count;
            if (count < 2) return MeshData.Empty;
            var centers = new Vector3[count];
            for (int i = 0; i < count; i++) centers[i] = ToVector(path.Points[i]);

            var vertices = new List<LEDPoint>(count * 8);
            var triangles = new List<int>(count * 36);
            float half = width / 2f;
            int edges = path.Closed ? count : count - 1;
            for (int i = 0; i < edges; i++)
            {
                int next = (i + 1) % count;
                var start = centers[i];
                var end = centers[next];
                var axis = end - start;
                // A zero-length segment contributes nothing; validated paths do not contain one.
                if (!float.IsFinite(axis.LengthSquared()) || axis.LengthSquared() < 1e-12f) continue;
                var direction = Vector3.Normalize(axis);
                var side = Vector3.Cross(direction, Vector3.UnitY);
                if (side.LengthSquared() < 1e-8f) side = Vector3.Cross(direction, Vector3.UnitX); // vertical beam
                side = Vector3.Normalize(side);
                var up = Vector3.Normalize(Vector3.Cross(direction, side));
                // Overhang: each beam reaches width/2 past its endpoints -> corner coverage by overlap.
                var a = start - direction * half;
                var b = end + direction * half;
                int v = vertices.Count;
                vertices.Add(ToPoint(a - side * half - up * half)); // 0
                vertices.Add(ToPoint(a + side * half - up * half)); // 1
                vertices.Add(ToPoint(a + side * half + up * half)); // 2
                vertices.Add(ToPoint(a - side * half + up * half)); // 3
                vertices.Add(ToPoint(b - side * half - up * half)); // 4
                vertices.Add(ToPoint(b + side * half - up * half)); // 5
                vertices.Add(ToPoint(b + side * half + up * half)); // 6
                vertices.Add(ToPoint(b - side * half + up * half)); // 7
                // Outward winding: cross(v1-v0, v2-v0) points away from the axis for every face.
                for (int t = 0; t < BoxTriangles.Length; t++) triangles.Add(v + BoxTriangles[t]);
            }
            if (vertices.Count < 8 || triangles.Count < 36) return MeshData.Empty;
            // Float precision at a large anchor-local offset can collapse tiny box edges.
            // Fail closed rather than hand Unity a mesh with zero-area faces.
            for (int i = 0; i < triangles.Count; i += 3)
            {
                var a = ToVector(vertices[triangles[i]]);
                var b = ToVector(vertices[triangles[i + 1]]);
                var c = ToVector(vertices[triangles[i + 2]]);
                var cross = Vector3.Cross(b - a, c - a);
                if (!float.IsFinite(cross.LengthSquared()) || cross.LengthSquared() <= 0f) return MeshData.Empty;
            }
            return new MeshData(vertices.ToArray(), triangles.ToArray());
        }

        // Outward-wound triangle list for one box, in the vertex order used by GenerateBeams.
        private static readonly int[] BoxTriangles =
        {
            0, 2, 1, 0, 3, 2,   // start cap
            4, 5, 6, 4, 6, 7,   // end cap
            0, 1, 5, 0, 5, 4,   // -up
            2, 3, 7, 2, 7, 6,   // +up
            1, 2, 6, 1, 6, 5,   // +side
            3, 0, 4, 3, 4, 7    // -side
        };

        private static Vector3 ToVector(LEDPoint p) => new Vector3(p.X, p.Y, p.Z);
        private static LEDPoint ToPoint(Vector3 v) => new LEDPoint(v.X, v.Y, v.Z);
    }
}

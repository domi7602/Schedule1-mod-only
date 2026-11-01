using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TownHallInterior;

// Local, version-bound extraction of ONLY the three attributed renderer contributions.
// Vertex payload: world XYZ, normal XYZ, tangent XYZW, UV0 XY, UV1 XY.
public sealed record FacadeMesh
{
    public string Path { get; init; } = "";
    public string MeshName { get; init; } = "";
    public int FirstSubMesh { get; init; }
    public int SourceVertexCount { get; init; }
    public double[] BoundsMin { get; init; } = Array.Empty<double>();
    public double[] BoundsMax { get; init; } = Array.Empty<double>();
    public string[] MaterialNames { get; init; } = Array.Empty<string>();
    public double[][] Vertices { get; init; } = Array.Empty<double[]>();
    public int[][] Submeshes { get; init; } = Array.Empty<int[]>();
}

public sealed record FacadeDocument
{
    public int Schema { get; init; }
    public string UnityVersion { get; init; } = "";
    public string LevelSha256 { get; init; } = "";
    public string BuildingGuid { get; init; } = "";
    public FacadeMesh[] Sources { get; init; } = Array.Empty<FacadeMesh>();
}

public static class FacadePortal
{
    public const string LevelSha256 = "8d17438080e7692895a8bb3ce63ca1a3f980bf0f2d5c8972f1ce67ea65cd047f";
    // The visual cut and owned entrance colliders share one top coordinate.
    // A historical player teleport position is not evidence of the approach floor.
    public static readonly double[] Minimum = { 54.18, 1.41, 30.04 };
    public static readonly double[] Maximum = { 54.56, PublicEntrance.PortalTop, 31.96 };

    public static FacadeDocument Read(byte[] bytes)
    {
        if (bytes.Length > 2_000_000) throw new InvalidOperationException("Facade extraction exceeds the byte budget");
        var document = JsonSerializer.Deserialize<FacadeDocument>(bytes,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Facade extraction is empty");
        ValidateDocument(document);
        return document;
    }

    public static void ValidateDocument(FacadeDocument document)
    {
        if (document.Schema != 1 || document.UnityVersion != "2022.3.62f2" ||
            document.LevelSha256 != LevelSha256 || document.BuildingGuid != NativeSceneSwap.BuildingGuid ||
            document.Sources == null || document.Sources.Length != 3)
            throw new InvalidOperationException("Unrecognized facade extraction or game version");
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var source in document.Sources)
        {
            if (source == null || !seen.Add(source.Path)) throw new InvalidOperationException("Duplicate facade target");
            var expected = source.Path switch
            {
                "townhall/Walls" => (Name: "Combined Mesh (root: scene) 23", First: 84, Count: 1, Vertices: 41514),
                "Cube" => (Name: "Combined Mesh (root: scene) 26", First: 127, Count: 1, Vertices: 63485),
                "townhall/Cube" => (Name: "Combined Mesh (root: scene) 25", First: 84, Count: 3, Vertices: 59729),
                _ => throw new InvalidOperationException("Unexpected facade target: " + source.Path),
            };
            ValidateMesh(source);
            if (source.MeshName != expected.Name || source.FirstSubMesh != expected.First ||
                source.SourceVertexCount != expected.Vertices || source.Submeshes.Length != expected.Count ||
                source.MaterialNames == null || source.MaterialNames.Length != expected.Count ||
                source.MaterialNames.Any(string.IsNullOrWhiteSpace) ||
                source.BoundsMin == null || source.BoundsMax == null ||
                source.BoundsMin.Length != 3 || source.BoundsMax.Length != 3)
                throw new InvalidOperationException("Facade contributor metadata mismatch: " + source.Path);
            for (int axis = 0; axis < 3; axis++)
                if (!double.IsFinite(source.BoundsMin[axis]) || !double.IsFinite(source.BoundsMax[axis]) ||
                    Math.Abs(source.Vertices.Min(v => v[axis]) - source.BoundsMin[axis]) > 0.0001 ||
                    Math.Abs(source.Vertices.Max(v => v[axis]) - source.BoundsMax[axis]) > 0.0001)
                    throw new InvalidOperationException("Facade bounds mismatch: " + source.Path);
        }
    }

    public static FacadeMesh Cut(FacadeMesh source)
    {
        ValidateMesh(source);
        var vertices = source.Vertices.Select(v => (double[])v.Clone()).ToList();
        var submeshes = new List<int[]>();
        foreach (var indices in source.Submeshes)
        {
            var output = new List<int>();
            for (int i = 0; i < indices.Length; i += 3)
            {
                var a = source.Vertices[indices[i]];
                var b = source.Vertices[indices[i + 1]];
                var c = source.Vertices[indices[i + 2]];
                var remaining = new List<double[]> { a, b, c };
                var retained = new List<List<double[]>>();
                for (int plane = 0; plane < 6 && remaining.Count >= 3; plane++)
                {
                    int axis = plane / 2;
                    bool minimum = plane % 2 == 0;
                    var (inside, outside) = Split(remaining, axis,
                        minimum ? Minimum[axis] : Maximum[axis], minimum);
                    if (outside.Count >= 3) retained.Add(outside);
                    remaining = inside;
                }
                // Preserve untouched triangles and their original attribute values exactly.
                if (PolygonArea(remaining) < 1e-12)
                {
                    output.Add(indices[i]); output.Add(indices[i + 1]); output.Add(indices[i + 2]);
                    continue;
                }
                foreach (var polygon in retained)
                    for (int j = 1; j < polygon.Count - 1; j++)
                    {
                        if (TriangleArea(polygon[0], polygon[j], polygon[j + 1]) < 1e-12) continue;
                        output.Add(vertices.Count); vertices.Add(polygon[0]);
                        output.Add(vertices.Count); vertices.Add(polygon[j]);
                        output.Add(vertices.Count); vertices.Add(polygon[j + 1]);
                    }
            }
            submeshes.Add(output.ToArray());
        }
        return source with { Vertices = vertices.ToArray(), Submeshes = submeshes.ToArray() };
    }

    private static (List<double[]> Inside, List<double[]> Outside) Split(
        List<double[]> polygon, int axis, double boundary, bool minimum)
    {
        var inside = new List<double[]>();
        var outside = new List<double[]>();
        for (int i = 0; i < polygon.Count; i++)
        {
            var a = polygon[i]; var b = polygon[(i + 1) % polygon.Count];
            double da = minimum ? a[axis] - boundary : boundary - a[axis];
            double db = minimum ? b[axis] - boundary : boundary - b[axis];
            if (da >= 0) inside.Add(a);
            if (da <= 0) outside.Add(a);
            if ((da > 0 && db < 0) || (da < 0 && db > 0))
            {
                var vertex = new double[14];
                double t = da / (da - db);
                for (int channel = 0; channel < vertex.Length; channel++)
                    vertex[channel] = a[channel] + (b[channel] - a[channel]) * t;
                vertex[axis] = boundary;
                inside.Add(vertex); outside.Add(vertex);
            }
        }
        return (inside, outside);
    }

    private static double PolygonArea(List<double[]> polygon)
    {
        double area = 0;
        for (int i = 1; i + 1 < polygon.Count; i++)
            area += TriangleArea(polygon[0], polygon[i], polygon[i + 1]);
        return area;
    }

    private static double TriangleArea(double[] a, double[] b, double[] c)
    {
        double ux = b[0] - a[0], uy = b[1] - a[1], uz = b[2] - a[2];
        double vx = c[0] - a[0], vy = c[1] - a[1], vz = c[2] - a[2];
        double x = uy * vz - uz * vy, y = uz * vx - ux * vz, z = ux * vy - uy * vx;
        return Math.Sqrt(x * x + y * y + z * z) / 2;
    }

    public static void ValidateMesh(FacadeMesh source)
    {
        if (source.Vertices == null || source.Vertices.Length is < 3 or > 10000 ||
            source.Submeshes == null || source.Submeshes.Length is < 1 or > 8)
            throw new InvalidOperationException("Facade mesh is missing or exceeds the budget");
        foreach (var v in source.Vertices)
            if (v == null || v.Length != 14 || v.Any(value => !double.IsFinite(value)))
                throw new InvalidOperationException("Invalid facade vertex payload");
        foreach (var indices in source.Submeshes)
            if (indices == null || indices.Length > 30000 || indices.Length % 3 != 0 ||
                indices.Any(index => index < 0 || index >= source.Vertices.Length))
                throw new InvalidOperationException("Invalid facade triangle indices");
    }
}

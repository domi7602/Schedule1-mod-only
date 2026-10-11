using Xunit;
using PropertyLEDs.Core;
using System.Numerics;
namespace PropertyLEDs.Tests;
public class GeometryTests
{
    internal static LEDPath Open(params LEDPoint[] points) => new() { Points = points.ToList() };
    internal static Vector3 V(LEDPoint p) => new(p.X, p.Y, p.Z);

    [Fact]
    public void Straight_beam_is_a_finite_box_with_overhang_and_outward_triangles()
    {
        var mesh = GeometryMath.GenerateBeams(Open(new(0, 0, 0), new(0, 0, 2)), 0.2f);
        Assert.Equal(8, mesh.Vertices.Length);
        Assert.Equal(36, mesh.Triangles.Length);
        Assert.All(mesh.Vertices, p => Assert.True(float.IsFinite(p.X) && float.IsFinite(p.Y) && float.IsFinite(p.Z)));
        // Overhang: the beam reaches width/2 past both endpoints (0 -> -0.1, 2 -> 2.1 along Z).
        Assert.InRange(mesh.Vertices.Select(p => p.Z).Min(), -0.100001f, -0.099999f);
        Assert.InRange(mesh.Vertices.Select(p => p.Z).Max(), 2.099999f, 2.100001f);
        // Square cross-section of exactly `width`: X and Y each span -0.1 .. 0.1.
        Assert.InRange(mesh.Vertices.Select(p => p.X).Min(), -0.100001f, -0.099999f);
        Assert.InRange(mesh.Vertices.Select(p => p.X).Max(), 0.099999f, 0.100001f);
        Assert.InRange(mesh.Vertices.Select(p => p.Y).Min(), -0.100001f, -0.099999f);
        Assert.InRange(mesh.Vertices.Select(p => p.Y).Max(), 0.099999f, 0.100001f);
        // Outward triangles: for the convex box, every face normal points away from the segment midpoint.
        var mid = new Vector3(0, 0, 1);
        for (int i = 0; i < mesh.Triangles.Length; i += 3)
        {
            var a = V(mesh.Vertices[mesh.Triangles[i]]);
            var b = V(mesh.Vertices[mesh.Triangles[i + 1]]);
            var c = V(mesh.Vertices[mesh.Triangles[i + 2]]);
            var normal = Vector3.Cross(b - a, c - a);
            var center = (a + b + c) / 3;
            Assert.True(normal.LengthSquared() > 1e-12);
            Assert.True(Vector3.Dot(normal, center - mid) > 0);
        }
        AssertClosedManifold(mesh);
    }

    [Fact]
    public void Adjacent_beams_extend_half_width_past_every_corner_so_corners_stay_covered()
    {
        // Closed L-shaped path with the motel room's corner layout.
        var path = Open(new(2.30f, 3.46f, -3.85f), new(2.30f, 3.46f, 2.17f), new(-2.62f, 3.46f, 2.17f),
                        new(-2.62f, 3.46f, -1.43f), new(-0.70f, 3.46f, -1.43f), new(-0.70f, 3.46f, -3.85f));
        path.Closed = true;
        float width = 0.055f;
        var mesh = GeometryMath.GenerateBeams(path, width);
        int count = path.Points.Count;
        Assert.Equal(count * 8, mesh.Vertices.Length);
        for (int i = 0; i < count; i++)
        {
            int next = (i + 1) % count;
            // Beam i's END face reaches width/2 BEYOND the shared corner along beam i's direction;
            // the following beam's START face reaches width/2 beyond the corner back the other way.
            var corner = V(path.Points[next]);
            var dirA = Vector3.Normalize(corner - V(path.Points[i]));
            float endProj = float.MinValue;
            for (int v = i * 8 + 4; v < i * 8 + 8; v++) endProj = MathF.Max(endProj, Vector3.Dot(V(mesh.Vertices[v]), dirA));
            Assert.InRange(endProj, Vector3.Dot(corner, dirA) + width / 2f - 0.00001f, Vector3.Dot(corner, dirA) + width / 2f + 0.00001f);

            var corner2 = V(path.Points[(next + 1) % count]);
            var dirB = Vector3.Normalize(corner2 - corner);
            float startProj = float.MaxValue;
            for (int v = next * 8; v < next * 8 + 4; v++) startProj = MathF.Min(startProj, Vector3.Dot(V(mesh.Vertices[v]), dirB));
            Assert.InRange(startProj, Vector3.Dot(corner, dirB) - width / 2f - 0.00001f, Vector3.Dot(corner, dirB) - width / 2f + 0.00001f);
        }
        AssertClosedManifold(mesh);
    }

    [Fact]
    public void Open_and_closed_paths_emit_one_beam_per_segment()
    {
        var open = Open(new(0, 0, 0), new(1, 0, 0), new(1, 0, 1), new(2, 0, 1));
        Assert.Equal(3 * 8, GeometryMath.GenerateBeams(open, 0.05f).Vertices.Length);
        var closed = Open(new(0, 0, 0), new(1, 0, 0), new(1, 0, 1));
        closed.Closed = true;
        Assert.Equal(3 * 8, GeometryMath.GenerateBeams(closed, 0.05f).Vertices.Length);
    }

    [Fact]
    public void Singular_u_turns_return_empty_instead_of_degenerate_geometry()
    {
        var path = Open(new(0, 0, 0), new(1, 0, 0), new(0.5f, 0, 0));
        Assert.False(path.Validate(out _));
        Assert.Empty(GeometryMath.GenerateBeams(path, 0.055f).Vertices);
    }

    [Fact]
    public void Malformed_paths_and_widths_return_empty_without_throwing()
    {
        foreach (var path in new LEDPath?[] { null, new(), new() { Points = null! }, Open(new(0, 0, 0), new(float.NaN, 0, 0)), Open(new(0, 0, 0), new(0, 0, 0)) })
            Assert.Empty(GeometryMath.GenerateBeams(path, 0.055f).Vertices);
        var valid = Open(new(0, 0, 0), new(1, 0, 0));
        foreach (float width in new[] { float.NaN, float.PositiveInfinity, 0, -1, 2 })
            Assert.Empty(GeometryMath.GenerateBeams(valid, width).Vertices);
        // A vertical beam exercises the fallback side-axis branch and stays a closed box.
        AssertClosedManifold(GeometryMath.GenerateBeams(Open(new(0, 0, 0), new(0, 2, 0)), 0.055f));
    }

    [Fact]
    public void Unrepresentable_float_coordinates_fail_safe_instead_of_emitting_collapsed_triangles()
    {
        var path = Open(new(10000, 10000, 10000), new(9999, 10000, 10000));
        var mesh = GeometryMath.GenerateBeams(path, 0.001f);
        if (mesh.Vertices.Length != 0) AssertClosedManifold(mesh);
    }

    internal static void AssertClosedManifold(MeshData mesh)
    {
        var edges = new Dictionary<(int, int), (int Uses, int Orientation)>();
        for (int i = 0; i < mesh.Triangles.Length; i += 3)
        {
            var tri = mesh.Triangles.Skip(i).Take(3).ToArray();
            Assert.Equal(3, tri.Distinct().Count());
            Assert.All(tri, x => Assert.InRange(x, 0, mesh.Vertices.Length - 1));
            Assert.True(Vector3.Cross(V(mesh.Vertices[tri[1]]) - V(mesh.Vertices[tri[0]]), V(mesh.Vertices[tri[2]]) - V(mesh.Vertices[tri[0]])).LengthSquared() > 1e-14);
            for (int j = 0; j < 3; j++)
            {
                int a = tri[j], b = tri[(j + 1) % 3];
                var key = (Math.Min(a, b), Math.Max(a, b));
                edges.TryGetValue(key, out var previous);
                edges[key] = (previous.Uses + 1, previous.Orientation + (a < b ? 1 : -1));
            }
        }
        Assert.All(edges.Values, x => { Assert.Equal(2, x.Uses); Assert.Equal(0, x.Orientation); });
    }
}

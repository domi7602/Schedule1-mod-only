using System;
using System.Numerics;

namespace TaxiDriver;

/// <summary>XZ parameter/distance, original 3D segment height; no Unity dependency.</summary>
internal static class RoadProjectionPolicy
{
    internal static bool TryProject(Vector3 a, Vector3 b, Vector3 position,
        out Vector3 point, out Vector3 direction, out float lateral)
    {
        point = default;
        direction = default;
        lateral = 0;
        if (!Finite(a) || !Finite(b) || !Finite(position))
            return false;
        Vector3 segment = b - a;
        Vector3 flat = new(segment.X, 0, segment.Z);
        float lengthSquared = flat.LengthSquared();
        if (!float.IsFinite(lengthSquared) || lengthSquared < 1e-6f)
            return false;
        float dot = Vector3.Dot(new(position.X - a.X, 0, position.Z - a.Z), flat);
        if (!float.IsFinite(dot))
            return false;
        float t = Math.Clamp(dot / lengthSquared, 0f, 1f);
        point = a + segment * t;
        direction = Vector3.Normalize(flat);
        lateral = new Vector2(position.X - point.X, position.Z - point.Z).Length();
        return Finite(point) && Finite(direction) && float.IsFinite(lateral);
    }

    internal static bool Finite(Vector3 value) =>
        float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);
}

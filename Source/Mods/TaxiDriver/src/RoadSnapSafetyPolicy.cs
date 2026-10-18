using System;
using System.Numerics;

namespace TaxiDriver;

/// <summary>Fail-closed vertical support and measured chassis/root placement policy.</summary>
internal static class RoadSnapSafetyPolicy
{
    // A short probe around the interpolated road elevation, not a roof-to-basement search.
    internal const float SupportProximityMeters = 0.75f;
    internal const float BottomClearanceMeters = 0.05f;

    internal static bool NeedsHardCorrection(float lateral, float drop, bool steep) =>
        float.IsFinite(lateral) && float.IsFinite(drop) &&
        (lateral > 5f || (lateral > 3f && (drop > 2f || steep)));

    internal static bool IsSafeSupport(bool trigger, bool dynamic, bool character, bool vehicle,
        bool self, float slope) =>
        !trigger && !dynamic && !character && !vehicle && !self &&
        float.IsFinite(slope) && slope >= 0f && slope <= 40f;

    internal static bool TryPlacementBounds(ReadOnlySpan<Vector3> worldRootOffsets, Quaternion currentRotation,
        Quaternion targetRotation, out Vector3 min, out Vector3 max)
    {
        min = new(float.MaxValue);
        max = new(float.MinValue);
        float currentLength = currentRotation.LengthSquared();
        float targetLength = targetRotation.LengthSquared();
        if (worldRootOffsets.Length != 8 || !float.IsFinite(currentLength) || !float.IsFinite(targetLength) ||
            Math.Abs(currentLength - 1f) > 0.001f || Math.Abs(targetLength - 1f) > 0.001f)
            return false;
        Quaternion delta = targetRotation * Quaternion.Inverse(currentRotation);
        foreach (Vector3 corner in worldRootOffsets)
        {
            if (!RoadProjectionPolicy.Finite(corner))
                return false;
            Vector3 offset = Vector3.Transform(corner, delta);
            if (!RoadProjectionPolicy.Finite(offset))
                return false;
            min = Vector3.Min(min, offset);
            max = Vector3.Max(max, offset);
        }
        Vector3 size = max - min;
        return RoadProjectionPolicy.Finite(size) && size.X > 0 && size.Y > 0 && size.Z > 0;
    }

    internal static bool TryRootHeight(float projectedY, float groundY, float rootY, float bottomY,
        bool supported, bool free, out float targetY)
    {
        targetY = 0f;
        if (!supported || !free || !float.IsFinite(projectedY) || !float.IsFinite(groundY) ||
            !float.IsFinite(rootY) || !float.IsFinite(bottomY) ||
            Math.Abs(groundY - projectedY) > SupportProximityMeters)
            return false;
        targetY = groundY + (rootY - bottomY) + BottomClearanceMeters;
        return float.IsFinite(targetY);
    }
}

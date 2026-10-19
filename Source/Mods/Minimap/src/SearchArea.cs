using System;

namespace Minimap;

/// <summary>
/// One fixed, adapter-verified search area. Its center is a curated constant and is never
/// derived from the pickup position. Contains tests a finite point against the vertical
/// height band and the horizontal radius; ContainsTarget additionally keeps the point at
/// least InnerMargin inside the radius so a target must be well covered, not on the edge.
/// </summary>
public sealed class SearchArea
{
    /// <param name="height">Vertical half-height: a point may differ from CenterY by at most +/- this value.</param>
    public SearchArea(
        string id,
        int version,
        float centerX,
        float centerY,
        float centerZ,
        float radius,
        float height,
        float innerMargin)
    {
        Id = id ?? string.Empty;
        Version = version;
        CenterX = centerX;
        CenterY = centerY;
        CenterZ = centerZ;
        Radius = radius;
        Height = height;
        InnerMargin = innerMargin;
    }

    public string Id { get; }

    public int Version { get; }

    public float CenterX { get; }

    public float CenterY { get; }

    public float CenterZ { get; }

    public float Radius { get; }

    /// <summary>Vertical half-height, not the total band thickness; valid Y is CenterY +/- Height.</summary>
    public float Height { get; }

    public float InnerMargin { get; }

    public bool IsValid =>
        Id.Length > 0
        && Version >= 0
        && float.IsFinite(CenterX)
        && float.IsFinite(CenterY)
        && float.IsFinite(CenterZ)
        && float.IsFinite(Radius)
        && Radius > 0f
        && float.IsFinite(Height)
        && Height >= 0f
        && float.IsFinite(InnerMargin)
        && InnerMargin >= 0f
        && InnerMargin < Radius;

    public bool Contains(float x, float y, float z) => ContainsWithin(x, y, z, Radius);

    public bool ContainsTarget(float x, float y, float z) => ContainsWithin(x, y, z, Radius - InnerMargin);

    private bool ContainsWithin(float x, float y, float z, float limit)
    {
        if (!IsValid || !float.IsFinite(x) || !float.IsFinite(y) || !float.IsFinite(z))
            return false;
        if (limit < 0f)
            return false;
        if (Math.Abs((double)y - CenterY) > Height)
            return false;
        double dx = (double)x - CenterX;
        double dz = (double)z - CenterZ;
        return dx * dx + dz * dz <= (double)limit * limit;
    }
}

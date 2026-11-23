using System;

namespace MoonRocks;

/// <summary>
/// Pure math for the Moon terrain heightfield and the Planet-Crafter-style rocket flight curves.
/// Unity-free so it can be unit-tested without game assemblies.
/// </summary>
public static class MoonMath
{
    public const float EarthGravityMultiplier = 1f;
    public const float MoonGravityMultiplier = 0.22f;

    public const float MoonOriginX = 0f;
    public const float MoonOriginY = 3200f;
    public const float MoonOriginZ = 0f;

    public const float MoonRadius = 180f;
    public const float LandingPadFlatRadius = 16f;

    /// <summary>
    /// Computes local moon surface height (Y offset relative to <see cref="MoonOriginY"/>) at local (x, z).
    /// Keeps the central landing pad flat and blends smoothly into craters and rolling lunar ridges.
    /// </summary>
    public static float SampleTerrainHeight(float x, float z)
    {
        float dist = MathF.Sqrt(x * x + z * z);

        // Smooth blend from flat landing pad (0..16m) to full terrain (28m+).
        float padBlend = SmoothStep(LandingPadFlatRadius, LandingPadFlatRadius + 12f, dist);
        if (padBlend <= 0f) return 0f;

        // Rolling lunar dunes / hills.
        float hills =
            MathF.Sin(x * 0.045f) * MathF.Cos(z * 0.042f) * 3.2f +
            MathF.Sin(x * 0.11f + 1.3f) * MathF.Sin(z * 0.095f - 0.7f) * 1.1f;

        // Deterministic craters around the landing zone.
        float craters =
            CraterOffset(x, z, -38f, 34f, 15f, 3.6f) +
            CraterOffset(x, z, 46f, -28f, 19f, 4.4f) +
            CraterOffset(x, z, 32f, 52f, 12f, 2.8f) +
            CraterOffset(x, z, -55f, -44f, 22f, 5.0f) +
            CraterOffset(x, z, 75f, 18f, 16f, 3.4f) +
            CraterOffset(x, z, -18f, -68f, 14f, 3.1f);

        // Gentle rim rise near the outer playable edge so the horizon feels enclosed.
        float rim = SmoothStep(MoonRadius - 25f, MoonRadius, dist) * 7.5f;

        return (hills + craters + rim) * padBlend;
    }

    private static float CraterOffset(float x, float z, float cx, float cz, float radius, float depth)
    {
        float dx = x - cx;
        float dz = z - cz;
        float r = MathF.Sqrt(dx * dx + dz * dz) / radius;
        if (r >= 1.45f) return 0f;

        if (r < 1f)
        {
            // Bowl inside the crater.
            float bowl = (r * r - 1f) * depth;
            // Slight raised rim near r = 1.
            float rim = SmoothStep(0.7f, 1.0f, r) * (depth * 0.28f);
            return bowl + rim;
        }
        else
        {
            // Outer rim falloff from 1.0 to 1.45.
            float outer = 1f - SmoothStep(1.0f, 1.45f, r);
            return outer * (depth * 0.28f);
        }
    }

    /// <summary>
    /// Normalized atmosphere darkness factor during ascent:
    /// 0.0 = Earth atmosphere, 1.0 = deep space / lunar sky.
    /// </summary>
    public static float ComputeSpaceDarkness(float progress01)
    {
        float clamped = Clamp01(progress01);
        return SmoothStep(0.22f, 0.72f, clamped);
    }

    /// <summary>
    /// Computes vertical ease curve for rocket ascent (slow liftoff, accelerating climb).
    /// </summary>
    public static float ComputeAscentProgress(float t01)
    {
        float t = Clamp01(t01);
        // Cubic acceleration for dramatic liftoff.
        return t * t * (0.4f + 0.6f * t);
    }

    /// <summary>
    /// Computes vertical ease curve for rocket descent (fast approach, gentle retro-brake touchdown).
    /// </summary>
    public static float ComputeDescentProgress(float t01)
    {
        float t = Clamp01(t01);
        float inv = 1f - t;
        return 1f - inv * inv * inv;
    }

    public static float SmoothStep(float edge0, float edge1, float x)
    {
        if (edge1 <= edge0) return x >= edge1 ? 1f : 0f;
        float t = Clamp01((x - edge0) / (edge1 - edge0));
        return t * t * (3f - 2f * t);
    }

    public static float Clamp01(float v) => v < 0f ? 0f : (v > 1f ? 1f : v);
}

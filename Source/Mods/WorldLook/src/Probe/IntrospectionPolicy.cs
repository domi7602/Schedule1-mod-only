using System;
using System.Collections.Generic;

namespace GraphicsProbe;

/// <summary>
/// Unity-free policy for bounded material/volume introspection: property read
/// strategy by shader property kind, property-count caps and non-finite value
/// normalization. Wired by ObjectReader/EnvironmentReader in the mod assembly.
/// </summary>
internal static class IntrospectionPolicy
{
    public const int MaxShaderProperties = 64;

    /// <summary>
    /// Maps a UnityEngine.Rendering.ShaderPropertyType name to a read strategy:
    /// "color" (GetColor), "vector" (GetVector), "float" (GetFloat, covers Range),
    /// "texture" (GetTexture identity only) and "unknown" (name kept, value not read).
    /// </summary>
    public static string ShapeOf(string? kind) => kind switch
    {
        "Color" => "color",
        "Vector" => "vector",
        "Float" or "Range" => "float",
        "Texture" => "texture",
        _ => "unknown"
    };

    /// <summary>Keeps the prefix of property indices within the cap.</summary>
    public static IReadOnlyList<int> PlanIndices(int totalCount, int cap)
    {
        int kept = Math.Clamp(cap, 0, Math.Max(0, totalCount));
        var indices = new List<int>(kept);
        for (int i = 0; i < kept; i++) indices.Add(i);
        return indices;
    }

    /// <summary>Finite values pass through; NaN/Infinity become null.</summary>
    public static double? Normalize(double value) => double.IsFinite(value) ? value : null;

    /// <summary>
    /// Normalizes a component tuple. JSON validity at serialization is guaranteed
    /// by ReportPolicy's finite converters; rejecting the whole tuple when one
    /// component is non-finite is a semantic choice: a color/vector with a broken
    /// component is not meaningful evidence, so the record carries a clean null.
    /// Empty or missing tuples are rejected as well.
    /// </summary>
    public static IReadOnlyList<double>? NormalizeComponents(IReadOnlyList<double> components)
    {
        if (components == null || components.Count == 0) return null;
        var kept = new List<double>(components.Count);
        foreach (double component in components)
        {
            if (!double.IsFinite(component)) return null;
            kept.Add(component);
        }
        return kept;
    }
}

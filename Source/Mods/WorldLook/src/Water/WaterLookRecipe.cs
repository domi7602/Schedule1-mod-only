using System.Globalization;
using System.Text;

namespace WaterShader;

internal interface IWaterLookMaterial
{
    bool HasProperty(string name);
    void SetFloat(string name, float value);
    void SetColor(string name, float r, float g, float b, float alpha);
}

/// <summary>Unity-free material tuning, called only when an owned clone is configured.</summary>
internal static class WaterLookRecipe
{
    private static readonly (string name, float value)[] Floats =
    {
        ("_WaveHeight", 0.35f),
        ("_WaveSpeed", 1.1f),
        ("_WaveCount", 4f),
        ("_NormalStrength", 0.18f),
        ("_FoamBaseAmount", 0.25f),
        // Defaults declared by the matching native shader; not physical depth units.
        ("_DepthVertical", 4f),
        ("_DepthHorizontal", 1f),
        ("_RefractionStrength", 0.1f),
        // Approved chromatic suppression candidate, not a claim about the native default.
        ("_RefractionChromaticAberration", 0f),
    };

    internal static string Apply(IWaterLookMaterial material)
    {
        var applied = new StringBuilder("[water-shader] clear-shallows applied:");
        int prefixLength = applied.Length;
        foreach (var (name, value) in Floats)
        {
            if (!material.HasProperty(name)) continue;
            material.SetFloat(name, value);
            applied.Append(' ').Append(name).Append('=').Append(value.ToString(CultureInfo.InvariantCulture));
        }
        if (material.HasProperty("_ShallowColor"))
        {
            // Artistic shallow-opacity candidate; native deep color is intentionally untouched.
            const float alpha = 0.15f;
            material.SetColor("_ShallowColor", 0.025f, 0.055f, 0.060f, alpha);
            applied.Append(" _ShallowColor=(0.025,0.055,0.06,").Append(alpha.ToString(CultureInfo.InvariantCulture)).Append(')');
        }
        if (applied.Length == prefixLength) applied.Append(" none");
        return applied.ToString();
    }
}

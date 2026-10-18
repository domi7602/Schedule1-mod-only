using System.Globalization;
using System.Text;

namespace WaterShader;

/// <summary>
/// Clarity step for the reference water shader, applied to the owned clone right after
/// the reference shader is bound. The shader's own declared defaults are the baseline;
/// this recipe moves only the clarity parameters one mild step toward "clearer" — less
/// absorption over the real water thickness, a slightly calmer surface, and a narrower
/// shore foam band.
/// The palette, reflection and sun values are deliberately NOT written: the owner
/// approved the colour, so this is a clarity change, not a recolour.
/// Every write is HasProperty-guarded and Unity-free, so a shader revision that drops a
/// name degrades to "that value is not applied" instead of faulting the render path.
/// </summary>
internal static class ReferenceWaterLook
{
    private static readonly (string name, float value)[] Floats =
    {
        // Absorption is the dominant clarity lever: the shader attenuates the refracted
        // bottom by 1 - exp(-thickness * _Absorption) over ACTUAL water thickness, so a
        // lower density is what makes the bottom read further out. 0.55 -> 0.38.
        ("_Absorption", 0.38f),
        // A calmer surface smears the refracted bottom less.
        ("_RippleStrength", 0.45f),
        // A narrower and weaker shore foam band keeps the shallow edge readable.
        ("_FoamDepth", 0.28f),
        ("_FoamStrength", 0.68f),
    };

    internal static string Apply(IWaterLookMaterial material)
    {
        var applied = new StringBuilder("[water-shader] reference clarity applied:");
        int prefixLength = applied.Length;
        foreach (var (name, value) in Floats)
        {
            if (!material.HasProperty(name)) continue;
            material.SetFloat(name, value);
            applied.Append(' ').Append(name).Append('=').Append(value.ToString(CultureInfo.InvariantCulture));
        }
        if (applied.Length == prefixLength) applied.Append(" none");
        return applied.ToString();
    }
}

namespace LightControl.Core;

/// <summary>
/// Unity-free navigation for the phone app: the color and intensity sets are the single source of
/// truth for cycling, and the original action resets to the model default. No UI dependency.
/// </summary>
public static class UiSelectionPolicy
{
    public const int ColorCount = 11;
    public const int IntensityCount = 7;

    private static readonly IntensityLevel[] Intensities =
    {
        IntensityLevel.Percent10, IntensityLevel.Percent25, IntensityLevel.Percent50, IntensityLevel.Percent75,
        IntensityLevel.Percent100, IntensityLevel.Percent150, IntensityLevel.Percent200
    };

    /// <summary>Normalizes any index into the half-open range [0, count).</summary>
    public static int WrapIndex(int index, int count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        return ((index % count) + count) % count;
    }

    public static ColorMode NextColor(ColorMode color, int delta = 1)
    {
        LightLimits.EnumValue(color, nameof(color));
        return (ColorMode)WrapIndex((int)color + delta, ColorCount);
    }

    public static IntensityLevel IntensityAt(int index) => Intensities[WrapIndex(index, IntensityCount)];

    public static IntensityLevel NextIntensity(IntensityLevel level, int delta = 1)
    {
        int index = Array.IndexOf(Intensities, level);
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(level));
        return IntensityAt(index + delta);
    }

    public static LampBaseState OriginalReset() => new(ColorMode.Original, IntensityLevel.Percent100);
}

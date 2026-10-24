namespace GoldenHour;

/// <summary>Mild accepted recipe; native intensity, sky rendering and grading stay in charge.</summary>
public static class GoldenHourTargets
{
    public static Argb SunLightTint { get; } = new(1.06f, 1.00f, 0.94f, 1f);
    public static Argb SkySunGlowTint { get; } = new(1.04f, 0.97f, 0.90f, 1f);
    public static Argb AmbientTint { get; } = new(1.03f, 0.995f, 0.96f, 1f);
}

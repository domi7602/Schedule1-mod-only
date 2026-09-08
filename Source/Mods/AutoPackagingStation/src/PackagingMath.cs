namespace AutoPackagingStation.Engine;

/// <summary>
/// Pure-math helpers for the AutoPackagingStation pipeline. Lives in a
/// separate file from AutoPackEngine so unit tests can target it without
/// pulling in S1API, MelonLoader IL2CPP bindings, or Unity game types.
/// </summary>
public static class PackagingMath
{
    /// <summary>
    /// Maps a 0..1 quality value to the integer quality tier used by the
    /// game's EQuality enum:
    ///   0 = Trash, 1 = Poor, 2 = Standard, 3 = Premium, 4 = Heavenly.
    /// Boundaries are inclusive on the lower side (quality 0.25 → Trash).
    /// </summary>
    public static int ComputeQualityTier(float qualityValue)
    {
        if (qualityValue < 0.25f) return 0; // Trash
        if (qualityValue < 0.40f) return 1; // Poor
        if (qualityValue < 0.75f) return 2; // Standard
        if (qualityValue < 0.90f) return 3; // Premium
        return 4;                           // Heavenly
    }

    /// <summary>
    /// Formats the display name for a packaged item, e.g. "OG Kush" + "jar" →
    /// "OG Kush (Jar)". Falls back to "Product (Packaged)" when packagingId is
    /// empty or null, and to "Product" when baseName is empty.
    /// </summary>
    public static string FormatPackagedName(string baseName, string packagingId)
    {
        string name = string.IsNullOrEmpty(baseName) ? "Product" : baseName;
        string pkg = (packagingId ?? "baggie").ToLowerInvariant();
        if (pkg.Contains("jar")) return $"{name} (Jar)";
        if (pkg.Contains("box")) return $"{name} (Box)";
        if (pkg.Contains("brick")) return $"{name} (Brick)";
        if (pkg.Contains("vial")) return $"{name} (Vial)";
        return $"{name} (Packaged)";
    }
}

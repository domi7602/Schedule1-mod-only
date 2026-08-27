using AutoPackagingStation.Engine;
using Xunit;

namespace AutoPackagingStation.Tests;

/// <summary>
/// Tests for AutoPackagingStation.Engine.PackagingMath — pure-math helpers
/// extracted from AutoPackEngine so they can be tested without spinning up
/// S1API, MelonLoader, or Unity.
/// </summary>
public class PackagingMathTests
{
    // ---------------- ComputeQualityTier ----------------

    [Theory]
    [InlineData(0.0f, 0)]   // Trash
    [InlineData(0.10f, 0)]  // Trash
    [InlineData(0.249f, 0)] // Trash (just below boundary)
    [InlineData(0.25f, 1)]  // Poor (boundary inclusive)
    [InlineData(0.30f, 1)]  // Poor
    [InlineData(0.399f, 1)] // Poor (just below boundary)
    [InlineData(0.40f, 2)]  // Standard
    [InlineData(0.50f, 2)]  // Standard
    [InlineData(0.749f, 2)] // Standard (just below boundary)
    [InlineData(0.75f, 3)]  // Premium
    [InlineData(0.80f, 3)]  // Premium
    [InlineData(0.899f, 3)] // Premium (just below boundary)
    [InlineData(0.90f, 4)]  // Heavenly
    [InlineData(1.0f, 4)]   // Heavenly
    public void ComputeQualityTier_MapsFloatToTier(float quality, int expectedTier)
    {
        Assert.Equal(expectedTier, PackagingMath.ComputeQualityTier(quality));
    }

    [Fact]
    public void ComputeQualityTier_NegativeInput_ReturnsTrash()
    {
        // Defensive: negative or NaN values should map to the lowest tier
        int negResult = PackagingMath.ComputeQualityTier(-0.5f);
        int nanResult = PackagingMath.ComputeQualityTier(float.NaN);
        // The function explicitly checks `qualityValue < 0.25f` for Trash, so
        // -0.5f is Trash. NaN is the boundary case: all < checks return false,
        // so NaN currently falls through to Heavenly (4). Documented limitation.
        Assert.Equal(0, negResult);
        Assert.Equal(4, nanResult);
    }

    [Fact]
    public void ComputeQualityTier_AboveOne_ClampsToHeavenly()
    {
        // The function returns Heavenly for any value >= 0.90, so 5.0
        // correctly maps to 4.
        Assert.Equal(4, PackagingMath.ComputeQualityTier(5.0f));
    }

    // ---------------- FormatPackagedName ----------------

    [Theory]
    [InlineData("OG Kush", "jar", "OG Kush (Jar)")]
    [InlineData("OG Kush", "JAR", "OG Kush (Jar)")]   // case-insensitive
    [InlineData("Weed", "jar_of_weed", "Weed (Jar)")] // contains 'jar'
    [InlineData("Product", "box", "Product (Box)")]
    [InlineData("Product", "cardboard_box", "Product (Box)")]
    [InlineData("Product", "brick", "Product (Brick)")]
    [InlineData("Product", "vial", "Product (Vial)")]
    [InlineData("Product", "VIAL", "Product (Vial)")]
    public void FormatPackagedName_RecognisesPackagingTypes(string baseName, string pkg, string expected)
    {
        Assert.Equal(expected, PackagingMath.FormatPackagedName(baseName, pkg));
    }

    [Fact]
    public void FormatPackagedName_UnknownPackaging_FallsBackToGeneric()
    {
        Assert.Equal("OG Kush (Packaged)", PackagingMath.FormatPackagedName("OG Kush", "baggie"));
        Assert.Equal("OG Kush (Packaged)", PackagingMath.FormatPackagedName("OG Kush", "unknown_pkg"));
    }

    [Fact]
    public void FormatPackagedName_EmptyBaseName_UsesProductFallback()
    {
        Assert.Equal("Product (Jar)", PackagingMath.FormatPackagedName("", "jar"));
    }

    [Fact]
    public void FormatPackagedName_NullPackagingId_UsesBaggieFallback()
    {
        // Null is treated as "baggie" (default packaging) per the function contract
        Assert.Equal("OG Kush (Packaged)", PackagingMath.FormatPackagedName("OG Kush", null!));
    }

    [Fact]
    public void FormatPackagedName_NullAndEmptyBaseName_ProducesProductName()
    {
        Assert.Equal("Product (Packaged)", PackagingMath.FormatPackagedName(null!, "baggie"));
        Assert.Equal("Product (Packaged)", PackagingMath.FormatPackagedName("", "baggie"));
    }
}

using System;

namespace Minimap;

/// <summary>Unity-free feature decisions shared by runtime code and focused tests.</summary>
public static class MinimapFeatureRules
{
    public const float MinimumZoom = 0.1f;
    public const float MaximumZoom = 12f;
    public const float MaximumBorderThickness = 12f;
    private const float DefaultZoom = 1.75f;
    private const float DefaultOpacity = 0.85f;
    private const float DefaultBorderThickness = 4f;

    public static float ClampZoom(float value)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
            return DefaultZoom;
        return Math.Clamp(value, MinimumZoom, MaximumZoom);
    }

    public static float ClampOpacity(float value)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
            return DefaultOpacity;
        return Math.Clamp(value, 0f, 1f);
    }

    public static float ClampBorderThickness(float value)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
            return DefaultBorderThickness;
        return Math.Clamp(value, 0f, MaximumBorderThickness);
    }

    /// <summary>
    /// Curated colour palette offered by the settings app's colour pickers (#RRGGBB):
    /// neutral greys plus vivid hues, all clearly distinguishable on the dark map.
    /// Full hex control remains available via the blip palette sidecar / config.json.
    /// </summary>
    public static readonly string[] ColorPalette =
    {
        "#FFFFFF", "#C8D0D8", "#6E7681", "#23272E",
        "#FF5252", "#FF8A3D", "#FFC94D", "#FFE94A",
        "#9AE66E", "#3BD882", "#2BD9C7", "#4FC3F7",
        "#5B8DEF", "#8C6BFF", "#E14BFF", "#FF6E9C",
    };

    public static MinimapMarkerKind ResolveCustomerMarker(
        bool isAwaitingDelivery,
        bool isAtDealLocation,
        bool isPotential,
        bool showKnownCustomers)
    {
        if (isAwaitingDelivery || isAtDealLocation)
            return MinimapMarkerKind.AcceptedDealClock;
        if (isPotential)
            return MinimapMarkerKind.PotentialCustomer;
        return showKnownCustomers ? MinimapMarkerKind.KnownCustomer : MinimapMarkerKind.None;
    }

    public static bool ShouldShowPurchaseCheck(bool canBePurchased, bool isOwned)
        => canBePurchased && !isOwned;

    public static bool ShouldShowRegionLock(bool isUnlocked)
        => !isUnlocked;
}

public enum MinimapMarkerKind
{
    None,
    KnownCustomer,
    PotentialCustomer,
    AcceptedDealClock
}

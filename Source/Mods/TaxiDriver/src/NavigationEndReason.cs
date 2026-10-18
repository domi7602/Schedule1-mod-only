using System;

namespace TaxiDriver;

/// <summary>One truthful terminal verdict shared by ride feedback and fare-end reporting.</summary>
internal static class NavigationEndReason
{
    internal static string ForRide(bool gaveUp, float distance, float arrivalRadius, string? callback)
    {
        if (gaveUp) return "navigation gave up";
        if (string.Equals(callback, "Failed", StringComparison.OrdinalIgnoreCase)) return "navigation failed";
        if (string.Equals(callback, "Stopped", StringComparison.OrdinalIgnoreCase)) return "navigation stopped";
        if (!float.IsFinite(distance) || distance > arrivalRadius) return "stopped short";
        return "arrived";
    }
}

using System;

namespace MessagesPlus;

/// <summary>Semantic roles accepted by the dark-theme whitelist.</summary>
internal enum ThemeRole
{
    PageBackground,
    Header,
    InboxRowBackground,
    IncomingBubble,
    PrimaryText,
    SecondaryText,
    MonochromeIcon,
    ProtectedContent,
    StatusIndicator,
    OutgoingContent
}

/// <summary>RGB-only value used by the theme state policy; alpha is always preserved separately.</summary>
internal readonly record struct ThemeRgb(float R, float G, float B);

/// <summary>
/// Pure decision policy for one tracked graphic. It distinguishes a vanilla reset
/// to the first observed color from a different, potentially legitimate state color.
/// </summary>
internal static class ThemeColorPolicy
{
    private const float ColorEpsilon = 0.002f;

    public static bool ShouldWrite(
        bool hasState,
        ThemeRole role,
        ThemeRgb original,
        ThemeRgb lastTheme,
        ThemeRgb current,
        ThemeRgb target)
    {
        if (role is ThemeRole.ProtectedContent or ThemeRole.StatusIndicator or ThemeRole.OutgoingContent)
            return false;

        if (SameRgb(current, target)) return false;
        if (!hasState) return true;

        // A prior theme color means a semantic-role change (or an interrupted theme
        // assignment), so moving to the new target is intentional.
        if (SameRgb(current, lastTheme)) return true;

        // Vanilla restoring the original surface/text color is the one safe signal
        // that the theme should be re-applied. Other external colors are preserved.
        return SameRgb(current, original);
    }

    private static bool SameRgb(ThemeRgb left, ThemeRgb right) =>
        MathF.Abs(left.R - right.R) <= ColorEpsilon &&
        MathF.Abs(left.G - right.G) <= ColorEpsilon &&
        MathF.Abs(left.B - right.B) <= ColorEpsilon;
}

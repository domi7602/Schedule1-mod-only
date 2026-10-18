using UnityEngine;
using GamePalette = S1Mods.Shared.GamePalette;

namespace BankApp.UI;

/// <summary>
/// BankApp brand color aliases. The shared <see cref="GamePalette"/> is the single source of
/// truth (it already documents BankApp as a consumer); this type only preserves the mod-local
/// names so the brand vocabulary stays readable without duplicating any color value.
/// Scaling/fonts still come from <c>S1Mods.Shared.UITheme</c> (using alias in BankApp.cs).
/// </summary>
public static class BankTheme
{
    public static Color BgDark => GamePalette.Bg;
    public static Color HeaderBg => GamePalette.Header;
    public static Color CardBg => GamePalette.Card;
    public static Color CardBgSecondary => GamePalette.CardAlt;
    public static Color CardBorder => GamePalette.Border;

    public static Color AccentGreen => GamePalette.Green;
    public static Color AccentBlue => GamePalette.Blue;
    public static Color AccentTeal => GamePalette.Teal;
    public static Color AccentOrange => GamePalette.Orange;
    public static Color AccentRed => GamePalette.Red;

    public static Color TextPrimary => GamePalette.TextPrimary;
    public static Color TextMuted => GamePalette.TextMuted;
    public static Color TextDim => GamePalette.TextDim;
}

using UnityEngine;

namespace S1Mods.Shared;

/// <summary>
/// Shared dark palette for S1 phone apps. Values are taken verbatim from BankApp's
/// verified v0.3.0 look (<c>BankApp/src/BankTheme.cs</c>, screenshot-verified 2026-09-09),
/// so every app that uses this palette is guaranteed to match an already-approved surface.
/// <para>
/// Opacity rule: every surface token is fully opaque. The previous generation of phone UIs
/// built its hierarchy from white-alpha "glass" fills (white 4-14%) over a near-black base;
/// that vocabulary does not survive an opaque palette and is deliberately replaced by
/// <b>steps of surface lightness</b> (Bg &lt; Card &lt; CardAlt &lt; CardHover &lt; CardPressed &lt; Border).
/// </para>
/// <para>
/// Accents carry meaning, not decoration: <see cref="Blue"/> = interactive/selected,
/// <see cref="Green"/> = primary action/success, <see cref="Teal"/> = numeric value and
/// progress-bar fill, <see cref="Orange"/> = attention/ready, <see cref="Red"/> = destructive.
/// </para>
/// <para>
/// Ink rule (BankApp precedent, verified 2026-09-09): text sitting on a saturated accent fill
/// uses <see cref="Color.white"/>, not a dark ink — see BankApp's selected tab
/// (<c>_depositTabText.color = isDep ? Color.white : TextMuted</c>) and its teal value numbers.
/// </para>
/// </summary>
public static class GamePalette
{
    // ---- Surfaces (opaque, ascending lightness) ----

    /// <summary>App backdrop.</summary>
    public static readonly Color Bg = new(0.043f, 0.055f, 0.078f, 1f);

    /// <summary>Header/inset band, one step above <see cref="Bg"/>.</summary>
    public static readonly Color Header = new(0.067f, 0.086f, 0.133f, 1f);

    /// <summary>Standard card/row fill.</summary>
    public static readonly Color Card = new(0.082f, 0.114f, 0.165f, 1f);

    /// <summary>Raised card / active-but-neutral control fill.</summary>
    public static readonly Color CardAlt = new(0.110f, 0.149f, 0.220f, 1f);

    /// <summary>Hover step (derived from CardAlt, kept in the same family).</summary>
    public static readonly Color CardHover = new(0.137f, 0.184f, 0.267f, 1f);

    /// <summary>Pressed step (derived, between hover and border).</summary>
    public static readonly Color CardPressed = new(0.165f, 0.220f, 0.314f, 1f);

    /// <summary>Card outline / track / divider. Also the lightest neutral step.</summary>
    public static readonly Color Border = new(0.169f, 0.220f, 0.306f, 1f);

    // ---- Accents ----

    public static readonly Color Green = new(0.180f, 0.627f, 0.263f, 1f);
    public static readonly Color Blue = new(0.220f, 0.545f, 0.992f, 1f);
    public static readonly Color Teal = new(0.161f, 0.796f, 0.725f, 1f);
    public static readonly Color Orange = new(0.941f, 0.533f, 0.243f, 1f);
    public static readonly Color Red = new(0.973f, 0.318f, 0.286f, 1f);

    // ---- Text ----

    /// <summary>Titles and values.</summary>
    public static readonly Color TextPrimary = new(0.941f, 0.965f, 0.988f, 1f);

    /// <summary>Labels, sublabels, badges.</summary>
    public static readonly Color TextMuted = new(0.545f, 0.580f, 0.620f, 1f);

    /// <summary>Disabled text and zero values.</summary>
    public static readonly Color TextDim = new(0.282f, 0.310f, 0.345f, 1f);
}

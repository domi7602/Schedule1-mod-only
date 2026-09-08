using UnityEngine;
using S1Mods.Shared;

namespace BankApp.UI;

public static class UITheme
{
    public const float RefHeight = S1Mods.Shared.UITheme.RefHeight;
    public const float RefWidth = S1Mods.Shared.UITheme.RefWidth;

    public static float ActualWidth => S1Mods.Shared.UITheme.ActualWidth;
    public static float ActualHeight => S1Mods.Shared.UITheme.ActualHeight;
    public static float Scale => S1Mods.Shared.UITheme.Scale;

    public static readonly Color BgDark = new(0.043f, 0.055f, 0.078f, 1f);
    public static readonly Color HeaderBg = new(0.067f, 0.086f, 0.133f, 1f);
    public static readonly Color CardBg = new(0.082f, 0.114f, 0.165f, 1f);
    public static readonly Color CardBgSecondary = new(0.110f, 0.149f, 0.220f, 1f);
    public static readonly Color CardBorder = new(0.169f, 0.220f, 0.306f, 1f);

    public static readonly Color AccentGreen = new(0.180f, 0.627f, 0.263f, 1f);
    public static readonly Color AccentBlue = new(0.220f, 0.545f, 0.992f, 1f);
    public static readonly Color AccentOrange = new(0.941f, 0.533f, 0.243f, 1f);
    public static readonly Color AccentRed = new(0.973f, 0.318f, 0.286f, 1f);

    public static readonly Color TextPrimary = new(0.941f, 0.965f, 0.988f, 1f);
    public static readonly Color TextMuted = new(0.545f, 0.580f, 0.620f, 1f);
    public static readonly Color TextDim = new(0.282f, 0.310f, 0.345f, 1f);

    public static void Initialize(RectTransform containerRt) => S1Mods.Shared.UITheme.InitializeForTextApp(containerRt);

    public static int Sp(float pt) => S1Mods.Shared.UITheme.Sp(pt);

    public static float Dp(float px) => S1Mods.Shared.UITheme.Dp(px);
}

using UnityEngine;

namespace PocketShop.UI;

public static class UITheme
{
    public const float RefHeight = S1Mods.Shared.UITheme.RefHeight;
    public const float RefWidth = S1Mods.Shared.UITheme.RefWidth;

    public static float ActualWidth => S1Mods.Shared.UITheme.ActualWidth;
    public static float ActualHeight => S1Mods.Shared.UITheme.ActualHeight;
    public static float Scale => S1Mods.Shared.UITheme.Scale;

    public static void Initialize(RectTransform containerRt) => S1Mods.Shared.UITheme.InitializeForTextApp(containerRt);

    public static int Sp(float pt) => S1Mods.Shared.UITheme.Sp(pt);

    public static float Dp(float px) => S1Mods.Shared.UITheme.Dp(px);
}

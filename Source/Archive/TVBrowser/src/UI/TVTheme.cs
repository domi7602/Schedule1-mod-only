using System;
using UnityEngine;

namespace TVBrowser.UI;

public static class TVTheme
{
    public const float RefWidth = 1920f;
    public const float RefHeight = 1080f;

    public static float ActualWidth { get; private set; } = RefWidth;
    public static float ActualHeight { get; private set; } = RefHeight;
    public static float Scale { get; private set; } = 1.0f;

    public static void Initialize(RectTransform containerRt)
    {
        Canvas.ForceUpdateCanvases();
        var r = containerRt.rect;
        float w = Mathf.Abs(r.width);
        float h = Mathf.Abs(r.height);

        if (w > 300f && h > 200f)
        {
            ActualWidth = w;
            ActualHeight = h;
            Scale = Mathf.Clamp(ActualHeight / 720f, 1.0f, 2.5f);
        }
        else
        {
            ActualWidth = RefWidth;
            ActualHeight = RefHeight;
            Scale = 1.5f;
        }
    }

    public static int Sp(float pt) => Mathf.RoundToInt(pt * Scale);
    public static float Dp(float px) => px * Scale;
}

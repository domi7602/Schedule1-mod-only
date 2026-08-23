using UnityEngine;

namespace S1Mods.Shared;

public static class UITheme
{
    public const float RefHeight = 750f;
    public const float RefWidth = 400f;

    public static float ActualWidth { get; private set; } = RefWidth;
    public static float ActualHeight { get; private set; } = RefHeight;
    public static float Scale { get; private set; } = 1.0f;

    public static void Initialize(RectTransform containerRt, float refHeight = 750f, float minScale = 0.85f, float maxScale = 2.0f)
    {
        if (containerRt == null)
        {
            ActualHeight = refHeight;
            ActualWidth = RefWidth;
            Scale = 1.0f;
            return;
        }
        try { Canvas.ForceUpdateCanvases(); } catch { }
        var r = containerRt.rect;
        float h = Mathf.Max(r.width, r.height);
        float w = Mathf.Min(r.width, r.height);
        if (h > 200f && w > 100f)
        {
            ActualHeight = h;
            ActualWidth = w;
            Scale = Mathf.Clamp(ActualHeight / refHeight, minScale, maxScale);
        }
        else
        {
            ActualHeight = refHeight;
            ActualWidth = RefWidth;
            Scale = 1.0f;
        }
    }

    public static void InitializeForTextApp(RectTransform containerRt) => Initialize(containerRt, 750f, 0.85f, 2.0f);

    public static void InitializeForDashboard(RectTransform containerRt) => Initialize(containerRt, 900f, 0.75f, 1.20f);

    public static int Sp(float pt) => Mathf.RoundToInt(pt * Scale);

    public static float Dp(float px) => px * Scale;

    public static void Reset()
    {
        ActualWidth = RefWidth;
        ActualHeight = RefHeight;
        Scale = 1.0f;
    }
}

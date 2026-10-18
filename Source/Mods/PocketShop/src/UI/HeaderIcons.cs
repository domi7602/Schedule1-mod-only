using System;
using UnityEngine;

namespace PocketShop.UI;

/// <summary>
/// Small procedural header icons (chevron, close, magnifier) drawn as white alpha sprites
/// at runtime and tinted via <c>Image.color</c> — no emoji glyphs (Arial renders them
/// blank) and no asset bundle needed. Each pixel is supersampled 3x3 so the strokes get
/// smooth edges at the small render sizes.
/// </summary>
internal static class HeaderIcons
{
    private const int Size = 24;
    private static readonly Color32 White = new(255, 255, 255, 255);

    private static Sprite? _chevronLeft;
    private static Sprite? _close;
    private static Sprite? _magnifier;

    /// <summary>"&lt;" back chevron.</summary>
    public static Sprite ChevronLeft()
    {
        if (Alive(_chevronLeft)) return _chevronLeft!;
        var px = Blank();
        Stroke(px, 15.5f, 6f, 9f, 12f, 2.4f);
        Stroke(px, 9f, 12f, 15.5f, 18f, 2.4f);
        _chevronLeft = Make(px);
        return _chevronLeft;
    }

    /// <summary>Clean "X" close glyph.</summary>
    public static Sprite Close()
    {
        if (Alive(_close)) return _close!;
        var px = Blank();
        Stroke(px, 7f, 17f, 17f, 7f, 2.4f);
        Stroke(px, 7f, 7f, 17f, 17f, 2.4f);
        _close = Make(px);
        return _close;
    }

    /// <summary>Magnifier for the search field (glass upper-left, handle lower-right).</summary>
    public static Sprite Magnifier()
    {
        if (Alive(_magnifier)) return _magnifier!;
        var px = Blank();
        CircleOutline(px, 9.5f, 14.5f, 4.2f, 2.2f);
        Stroke(px, 12.8f, 11.2f, 17.5f, 6.5f, 2.6f);
        _magnifier = Make(px);
        return _magnifier;
    }

    // ---- rasterizer -------------------------------------------------------

    private static Color32[] Blank()
    {
        var px = new Color32[Size * Size];
        var clear = new Color32(0, 0, 0, 0);
        for (int i = 0; i < px.Length; i++) px[i] = clear;
        return px;
    }

    private static Sprite Make(Color32[] px)
    {
        var tex = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        tex.SetPixels32(px);
        tex.Apply(false, true);
        return Sprite.Create(tex, new Rect(0, 0, Size, Size), new Vector2(0.5f, 0.5f), 100f);
    }

    private static bool Alive(Sprite? s) =>
        s != null && s.Pointer != IntPtr.Zero && !s.WasCollected;

    /// <summary>3x3 supersampled coverage of a predicate, blended additively (max alpha).</summary>
    private static void Cover(Color32[] px, Func<float, float, bool> inside)
    {
        for (int py = 0; py < Size; py++)
        {
            for (int pxi = 0; pxi < Size; pxi++)
            {
                int hits = 0;
                for (int sy = 0; sy < 3; sy++)
                {
                    for (int sx = 0; sx < 3; sx++)
                    {
                        float x = pxi + (sx + 0.5f) / 3f;
                        float y = py + (sy + 0.5f) / 3f;
                        if (inside(x, y)) hits++;
                    }
                }
                if (hits == 0) continue;
                byte a = (byte)(hits * 255 / 9);
                int i = py * Size + pxi;
                if (a > px[i].a) px[i] = new Color32(White.r, White.g, White.b, a);
            }
        }
    }

    private static void Stroke(Color32[] px, float x0, float y0, float x1, float y1, float thickness)
    {
        float half = thickness * 0.5f;
        Cover(px, (x, y) => DistToSegment(x, y, x0, y0, x1, y1) <= half);
    }

    private static void CircleOutline(Color32[] px, float cx, float cy, float r, float thickness)
    {
        float half = thickness * 0.5f;
        Cover(px, (x, y) =>
        {
            float d = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
            return Mathf.Abs(d - r) <= half;
        });
    }

    private static float DistToSegment(float px, float py, float x0, float y0, float x1, float y1)
    {
        float dx = x1 - x0;
        float dy = y1 - y0;
        float lenSq = dx * dx + dy * dy;
        float t = lenSq <= 0f ? 0f : Mathf.Clamp01(((px - x0) * dx + (py - y0) * dy) / lenSq);
        float cx = x0 + t * dx;
        float cy = y0 + t * dy;
        return Mathf.Sqrt((px - cx) * (px - cx) + (py - cy) * (py - cy));
    }
}

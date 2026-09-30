using System.Collections.Generic;
using UnityEngine;

namespace S1Mods.Shared;

/// <summary>
/// Shared, procedurally generated UI sprites (no bitmap assets) for S1 phone apps.
/// <para>
/// These generators were duplicated verbatim in <c>PotScannerApp.cs</c> and
/// <c>WeatherApp.cs</c> (~150 lines each, four near-identical copies of the same
/// signed-distance rasteriser). This is the single source of truth.
/// </para>
/// <para>
/// All sprites are white texels with an alpha silhouette, so the consuming
/// <see cref="UnityEngine.UI.Image"/> supplies the actual colour (including any alpha).
/// Rounded rects and capsules are 9-sliced: the corner radius on screen equals the
/// <c>radius</c> argument in texture pixels (border = radius), NOT a <c>UITheme.Dp</c>
/// value — pass the radius you want to see at 1x.
/// </para>
/// </summary>
public static class UISprites
{
    private static readonly Dictionary<int, Sprite> RoundedCache = new();
    private static Sprite? _capsuleSprite;
    private static Sprite? _circleSprite;
    private static Sprite? _donutSprite;

    /// <summary>
    /// Anti-aliased rounded rect, 9-sliced. <paramref name="radius"/> is the on-screen corner
    /// radius in texture px; the rasteriser uses a 32 px sprite, so keep it below ~14.
    /// Cached per (size, radius) — the old per-app versions rebuilt a sprite per radius.
    /// </summary>
    public static Sprite Rounded(float radius, int size = 32)
    {
        radius = Mathf.Clamp(radius, 1f, size * 0.45f);
        int key = (size * 1000) + Mathf.RoundToInt(radius * 10f);
        if (RoundedCache.TryGetValue(key, out var cached) && cached != null) return cached;

        var px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float a = RoundedRectAlpha(x + 0.5f, y + 0.5f, size, size, radius);
                px[(y * size) + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }

        var sp = Build("S1Rounded" + key, px, size, size, radius, size, size);
        sp.name = $"S1Rounded{key}";
        RoundedCache[key] = sp;
        return sp;
    }

    /// <summary>Anti-aliased capsule (fully rounded ends), 9-sliced bars/pills/chevrons.</summary>
    public static Sprite Capsule()
    {
        if (_capsuleSprite != null) return _capsuleSprite;

        const int w = 32;
        const int h = 8;
        float radius = h * 0.5f;
        var px = new Color32[w * h];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float a = RoundedRectAlpha(x + 0.5f, y + 0.5f, w, h, radius);
                px[(y * w) + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }

        _capsuleSprite = Build("S1Capsule", px, w, h, radius, w, h);
        return _capsuleSprite;
    }

    /// <summary>Anti-aliased filled circle (status dots, gauge centrepiece parts).</summary>
    public static Sprite Circle()
    {
        if (_circleSprite != null) return _circleSprite;

        const int size = 48;
        const float radius = 23.5f;
        var px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x + 0.5f - (size * 0.5f);
                float dy = y + 0.5f - (size * 0.5f);
                float dist = Mathf.Sqrt((dx * dx) + (dy * dy));
                float a = Mathf.Clamp01(radius - dist + 0.5f);
                px[(y * size) + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }

        _circleSprite = Build("S1Circle", px, size, size, 0f, 0, 0, sliced: false);
        return _circleSprite;
    }

    /// <summary>
    /// Anti-aliased donut ring (outer radius 46 / inner 30 at 96 px). Used with
    /// <c>Image.Type.Filled</c> + <c>Radial360</c> for hero gauges.
    /// </summary>
    public static Sprite Donut()
    {
        if (_donutSprite != null) return _donutSprite;

        const int size = 96;
        const float outer = 46f;
        const float inner = 30f;
        var px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x + 0.5f - (size * 0.5f);
                float dy = y + 0.5f - (size * 0.5f);
                float dist = Mathf.Sqrt((dx * dx) + (dy * dy));
                float aOut = Mathf.Clamp01(outer - dist + 0.5f);
                float aIn = Mathf.Clamp01(dist - inner + 0.5f);
                float a = Mathf.Min(aOut, aIn);
                px[(y * size) + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }

        _donutSprite = Build("S1Donut", px, size, size, 0f, 0, 0, sliced: false);
        return _donutSprite;
    }

    /// <summary>Signed-distance alpha for an anti-aliased rounded rectangle.</summary>
    public static float RoundedRectAlpha(float px, float py, float w, float h, float radius)
    {
        float qx = Mathf.Abs(px - (w * 0.5f)) - ((w * 0.5f) - radius);
        float qy = Mathf.Abs(py - (h * 0.5f)) - ((h * 0.5f) - radius);
        float outside = Mathf.Sqrt((Mathf.Max(qx, 0f) * Mathf.Max(qx, 0f)) + (Mathf.Max(qy, 0f) * Mathf.Max(qy, 0f)));
        float inside = Mathf.Min(Mathf.Max(qx, qy), 0f);
        float dist = outside + inside - radius;
        return Mathf.Clamp01(-dist + 0.5f);
    }

    private static Sprite Build(string name, Color32[] px, int w, int h, float radius, int borderX, int borderY, bool sliced = true)
    {
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.SetPixels32(px);
        tex.Apply(false, true);

        var border = sliced ? new Vector4(radius, radius, radius, radius) : Vector4.zero;
        var sp = Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, border);
        sp.name = name;
        return sp;
    }
}

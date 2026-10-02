using System.Collections.Generic;
using UnityEngine;

namespace S1Mods.Shared;

// ---------------------------------------------------------------------------
// Merged from GamePalette.cs, UITheme.cs and UISprites.cs (2026-10-02).
// The three UI classes keep their names and namespace unchanged.
// ---------------------------------------------------------------------------

/// <summary>
/// Shared dark palette for S1 phone apps. Values are taken verbatim from BankApp's
/// verified v0.3.0 look (formerly <c>BankApp/src/BankTheme.cs</c>, merged away 2026-10-02; screenshot-verified 2026-09-09),
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

public static class UITheme
{
    public const float RefHeight = 750f;
    public const float RefWidth = 400f;

    public static float ActualWidth { get; private set; } = RefWidth;
    public static float ActualHeight { get; private set; } = RefHeight;
    public static float Scale { get; private set; } = 1.0f;

    public static void Initialize(RectTransform containerRt, float refHeight = 750f, float minScale = 0.85f, float maxScale = 2.0f)
    {
        if (refHeight <= 0f)
            refHeight = RefHeight;
        if (!NetworkGuard.IsAlive(containerRt))
        {
            ActualHeight = refHeight;
            ActualWidth = RefWidth;
            Scale = 1.0f;
            return;
        }
        try { Canvas.ForceUpdateCanvases(); } catch { }
        if (!NetworkGuard.IsAlive(containerRt)) return;
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
}

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

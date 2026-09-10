using System.Collections.Generic;
using UnityEngine;

namespace PocketShop.Services;

/// <summary>
/// Provides high-resolution avatars for each shop by rendering crisp,
/// anti-aliased 128x128 themed vector icons (Target for Arms Dealer,
/// Diamond for Boutique, T-Shirt for Clothing, Gas Pump for Gas Mart, etc.).
/// </summary>
public static class NPCPortraitService
{
    private static readonly Dictionary<string, Sprite> _cache = new();

    public static Sprite GetAvatar(string shopCode, string shopName, int size = 128)
    {
        string key = $"{shopCode}_{shopName}_{size}";
        if (_cache.TryGetValue(key, out var cached) && cached != null)
        {
            return cached;
        }

        // Generate crisp themed icon matching the mockup
        var iconSprite = CreateThemedStoreIcon(shopCode, shopName, size);
        _cache[key] = iconSprite;
        return iconSprite;
    }

    private static Sprite CreateThemedStoreIcon(string shopCode, string shopName, int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        var pixels = new Color32[size * size];

        var baseColor = ShopColorScheme.ColorFor(shopCode);
        string lowerCode = (shopCode ?? string.Empty).ToLowerInvariant();
        string lowerName = (shopName ?? string.Empty).ToLowerInvariant();

        // 1. Fill circle background
        int center = size / 2;
        float radius = size * 0.44f;
        float radiusSq = radius * radius;
        float innerRadiusSq = (radius - 2.5f) * (radius - 2.5f);

        Color32 bgC32 = new Color(baseColor.r * 0.85f, baseColor.g * 0.85f, baseColor.b * 0.85f, 0.40f);
        Color32 borderC32 = new Color(1f, 1f, 1f, 0.35f);
        Color32 fgC32 = Color.white;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                float distSq = dx * dx + dy * dy;

                if (distSq <= innerRadiusSq)
                {
                    pixels[y * size + x] = bgC32;
                }
                else if (distSq <= radiusSq)
                {
                    pixels[y * size + x] = borderC32;
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        // 2. Draw specific crisp vector silhouette based on store category
        if (lowerCode.Contains("gas") || lowerName.Contains("gas"))
        {
            DrawGasPump(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("thread") || lowerCode.Contains("cloth") || lowerName.Contains("thread") || lowerName.Contains("cloth"))
        {
            DrawTShirt(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("arm") || lowerCode.Contains("weapon") || lowerName.Contains("arm") || lowerName.Contains("weapon"))
        {
            DrawCrosshair(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("boutique") || lowerCode.Contains("playball") || lowerName.Contains("boutique") || lowerName.Contains("playball"))
        {
            DrawDiamond(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("fungal") || lowerCode.Contains("phil") || lowerName.Contains("fungal") || lowerName.Contains("phil"))
        {
            DrawMushroom(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("hardware") || lowerCode.Contains("hank") || lowerCode.Contains("dan") || lowerName.Contains("hardware"))
        {
            DrawTools(pixels, size, fgC32);
        }
        else
        {
            // Stylized NPC Portrait Silhouette (Head & Shoulders)
            DrawCharacterSilhouette(pixels, size, fgC32);
        }

        tex.SetPixels32(pixels);
        tex.Apply(false, false);

        var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        sprite.name = $"PocketShop_ThemedIcon_{shopCode}";
        return sprite;
    }

    private static void DrawGasPump(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillRect(pixels, size, c - 14, c - 18, 20, 32, fg);
        FillRect(pixels, size, c - 11, c + 2, 14, 8, new Color32(30, 40, 55, 220));
        DrawThickLine(pixels, size, c + 6, c + 8, c + 14, c + 4, 3, fg);
        DrawThickLine(pixels, size, c + 14, c + 4, c + 14, c - 12, 3, fg);
        FillRect(pixels, size, c + 11, c - 16, 6, 6, fg);
    }

    private static void DrawTShirt(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillRect(pixels, size, c - 14, c - 16, 28, 24, fg);
        FillRect(pixels, size, c - 22, c - 2, 10, 10, fg);
        FillRect(pixels, size, c + 12, c - 2, 10, 10, fg);
        FillCircle(pixels, size, c, c + 8, 7, new Color32(0, 0, 0, 0));
    }

    private static void DrawCrosshair(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        DrawRing(pixels, size, c, c, 16, 3, fg);
        DrawRing(pixels, size, c, c, 8, 2, fg);
        FillCircle(pixels, size, c, c, 3, fg);
        FillRect(pixels, size, c - 20, c - 1, 8, 3, fg);
        FillRect(pixels, size, c + 12, c - 1, 8, 3, fg);
        FillRect(pixels, size, c - 1, c - 20, 3, 8, fg);
        FillRect(pixels, size, c - 1, c + 12, 3, 8, fg);
    }

    private static void DrawDiamond(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        int r = 18;
        for (int y = c - r; y <= c + r; y++)
        {
            for (int x = c - r; x <= c + r; x++)
            {
                int dx = Mathf.Abs(x - c);
                int dy = Mathf.Abs(y - c);
                if (dx + dy <= r)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = fg;
                    }
                }
            }
        }
    }

    private static void DrawMushroom(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillRect(pixels, size, c - 6, c - 16, 12, 18, fg);
        for (int y = c - 2; y <= c + 18; y++)
        {
            for (int x = c - 22; x <= c + 22; x++)
            {
                float dx = (x - c) / 22f;
                float dy = (y - c + 2) / 16f;
                if (dx * dx + dy * dy <= 1f && y >= c - 2)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = fg;
                    }
                }
            }
        }
    }

    private static void DrawTools(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        DrawThickLine(pixels, size, c - 14, c - 14, c + 14, c + 14, 5, fg);
        DrawThickLine(pixels, size, c - 14, c + 14, c + 14, c - 14, 5, fg);
        FillRect(pixels, size, c + 10, c + 8, 8, 10, fg);
        FillCircle(pixels, size, c - 14, c - 14, 7, fg);
        FillCircle(pixels, size, c - 14, c - 14, 3, new Color32(0, 0, 0, 0));
    }

    private static void DrawCharacterSilhouette(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillCircle(pixels, size, c, c + 6, 11, fg);
        for (int y = c - 18; y <= c - 1; y++)
        {
            for (int x = c - 20; x <= c + 20; x++)
            {
                float dx = (x - c) / 20f;
                float dy = (y - (c - 18)) / 16f;
                if (dx * dx + (1f - dy) * (1f - dy) <= 1f)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = fg;
                    }
                }
            }
        }
    }

    private static void FillRect(Color32[] pixels, int size, int x, int y, int w, int h, Color32 c)
    {
        for (int py = y; py < y + h; py++)
        {
            for (int px = x; px < x + w; px++)
            {
                if (px >= 0 && px < size && py >= 0 && py < size)
                {
                    pixels[py * size + px] = c;
                }
            }
        }
    }

    private static void FillCircle(Color32[] pixels, int size, int cx, int cy, int r, Color32 c)
    {
        int rSq = r * r;
        for (int y = cy - r; y <= cy + r; y++)
        {
            for (int x = cx - r; x <= cx + r; x++)
            {
                if ((x - cx) * (x - cx) + (y - cy) * (y - cy) <= rSq)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = c;
                    }
                }
            }
        }
    }

    private static void DrawRing(Color32[] pixels, int size, int cx, int cy, int r, int thickness, Color32 c)
    {
        int outerSq = r * r;
        int innerSq = (r - thickness) * (r - thickness);
        for (int y = cy - r; y <= cy + r; y++)
        {
            for (int x = cx - r; x <= cx + r; x++)
            {
                int distSq = (x - cx) * (x - cx) + (y - cy) * (y - cy);
                if (distSq <= outerSq && distSq >= innerSq)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = c;
                    }
                }
            }
        }
    }

    private static void DrawThickLine(Color32[] pixels, int size, int x0, int y0, int x1, int y1, int thickness, Color32 c)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int steps = Mathf.Max(dx, dy);
        if (steps == 0) { FillCircle(pixels, size, x0, y0, thickness / 2, c); return; }

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            int x = Mathf.RoundToInt(Mathf.Lerp(x0, x1, t));
            int y = Mathf.RoundToInt(Mathf.Lerp(y0, y1, t));
            FillCircle(pixels, size, x, y, thickness / 2, c);
        }
    }

    public static void Reset()
    {
        foreach (var s in _cache.Values)
        {
            if (s != null)
            {
                if (s.texture != null) UnityEngine.Object.Destroy(s.texture);
                UnityEngine.Object.Destroy(s);
            }
        }
        _cache.Clear();
    }
}

/// <summary>
/// Deterministic color palette for shop cards matching the reference mockup.
/// </summary>
public static class ShopColorScheme
{
    private static readonly Color[] Palette = new[]
    {
        new Color(0.85f, 0.40f, 0.40f, 1f), // Crimson / Coral (Albert Hoover)
        new Color(0.70f, 0.85f, 0.35f, 1f), // Olive Lime (Dan's Hardware)
        new Color(0.35f, 0.80f, 0.45f, 1f), // Emerald Green (Fungal Phil)
        new Color(0.30f, 0.60f, 0.85f, 1f), // Ocean Blue (Handy Hank)
        new Color(0.45f, 0.50f, 0.85f, 1f), // Slate Indigo (Oscar's Store)
        new Color(0.65f, 0.45f, 0.80f, 1f), // Orchid Purple (Salvador Moreno)
        new Color(0.80f, 0.45f, 0.60f, 1f), // Berry Pink (Shirley Watts)
        new Color(0.88f, 0.58f, 0.28f, 1f), // Orange Ochre (Arms Dealer)
        new Color(0.90f, 0.72f, 0.30f, 1f), // Amber Gold (Playball's Boutique)
        new Color(0.85f, 0.35f, 0.55f, 1f), // Magenta Rose (Thrifty Threads)
        new Color(0.25f, 0.75f, 0.70f, 1f), // Teal (Gas Mart Central)
        new Color(0.30f, 0.70f, 0.80f, 1f), // Cyan Blue (Gas Mart West)
    };

    public static Color ColorFor(string shopCode)
    {
        if (string.IsNullOrEmpty(shopCode)) return Palette[0];
        string lower = shopCode.ToLowerInvariant();
        if (lower.Contains("albert")) return Palette[0];
        if (lower.Contains("dan")) return Palette[1];
        if (lower.Contains("fungal") || lower.Contains("phil")) return Palette[2];
        if (lower.Contains("hank")) return Palette[3];
        if (lower.Contains("oscar")) return Palette[4];
        if (lower.Contains("salvador")) return Palette[5];
        if (lower.Contains("shirley")) return Palette[6];
        if (lower.Contains("arm")) return Palette[7];
        if (lower.Contains("boutique") || lower.Contains("playball")) return Palette[8];
        if (lower.Contains("thread") || lower.Contains("cloth")) return Palette[9];
        if (lower.Contains("central")) return Palette[10];
        if (lower.Contains("west")) return Palette[11];

        uint hash = 2166136261;
        foreach (char c in shopCode) hash = (hash ^ c) * 16777619;
        return Palette[hash % Palette.Length];
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minimap;

public static class MinimapTextures
{
    private static readonly Dictionary<string, Sprite> _circleMaskCache = new();
    private static readonly Dictionary<string, Sprite> _circleBorderCache = new();
    private static readonly Dictionary<string, Sprite> _roundedSquareMaskCache = new();
    private static readonly Dictionary<string, Sprite> _roundedSquareBorderCache = new();
    private static Sprite? _playerArrowSprite;
    private static Sprite? _blipDotSprite;
    private static Sprite? _blipPoliceSprite;
    private static Sprite? _blipDealerSprite;
    private static Sprite? _blipPropertySprite;
    private static Sprite? _blipQuestSprite;

    public static Sprite GetCircleMask(int size = 256)
    {
        string key = $"{size}";
        if (_circleMaskCache.TryGetValue(key, out var cached)) return cached;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float center = size * 0.5f;
        float radius = center - 2f;
        float rSqr = radius * radius;
        float aaSpan = 2f;

        for (int y = 0; y < size; y++)
        {
            float dy = y - center + 0.5f;
            for (int x = 0; x < size; x++)
            {
                float dx = x - center + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((radius - d) / aaSpan);
                byte a = (byte)(alpha * 255f);
                pixels[y * size + x] = new Color32(255, 255, 255, a);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        _circleMaskCache[key] = sprite;
        return sprite;
    }

    public static Sprite GetCircleBorder(int size = 256, float thickness = 5f)
    {
        string key = $"{size}_{thickness}";
        if (_circleBorderCache.TryGetValue(key, out var cached)) return cached;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float center = size * 0.5f;
        float outerR = center - 1.5f;
        float innerR = outerR - thickness;

        for (int y = 0; y < size; y++)
        {
            float dy = y - center + 0.5f;
            for (int x = 0; x < size; x++)
            {
                float dx = x - center + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((outerR - d) / 1.5f) * Mathf.Clamp01((d - innerR) / 1.5f);
                byte a = (byte)(alpha * 255f);
                pixels[y * size + x] = new Color32(255, 255, 255, a);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        _circleBorderCache[key] = sprite;
        return sprite;
    }

    public static Sprite GetRoundedSquareMask(int size = 256, float cornerRadius = 36f)
    {
        string key = $"{size}_{cornerRadius}";
        if (_roundedSquareMaskCache.TryGetValue(key, out var cached)) return cached;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;
        float pad = 2f;
        float innerHalf = half - pad - cornerRadius;

        for (int y = 0; y < size; y++)
        {
            float dy = Mathf.Abs(y - half + 0.5f);
            for (int x = 0; x < size; x++)
            {
                float dx = Mathf.Abs(x - half + 0.5f);
                float d;
                if (dx > innerHalf && dy > innerHalf)
                {
                    float cx = dx - innerHalf;
                    float cy = dy - innerHalf;
                    d = Mathf.Sqrt(cx * cx + cy * cy) - cornerRadius;
                }
                else
                {
                    d = Mathf.Max(dx - (half - pad), dy - (half - pad));
                }

                float alpha = Mathf.Clamp01(-d / 1.5f);
                byte a = (byte)(alpha * 255f);
                pixels[y * size + x] = new Color32(255, 255, 255, a);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        _roundedSquareMaskCache[key] = sprite;
        return sprite;
    }

    public static Sprite GetRoundedSquareBorder(int size = 256, float cornerRadius = 36f, float thickness = 5f)
    {
        string key = $"{size}_{cornerRadius}_{thickness}";
        if (_roundedSquareBorderCache.TryGetValue(key, out var cached)) return cached;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;
        float pad = 2f;
        float innerHalf = half - pad - cornerRadius;

        for (int y = 0; y < size; y++)
        {
            float dy = Mathf.Abs(y - half + 0.5f);
            for (int x = 0; x < size; x++)
            {
                float dx = Mathf.Abs(x - half + 0.5f);
                float d;
                if (dx > innerHalf && dy > innerHalf)
                {
                    float cx = dx - innerHalf;
                    float cy = dy - innerHalf;
                    d = Mathf.Sqrt(cx * cx + cy * cy) - cornerRadius;
                }
                else
                {
                    d = Mathf.Max(dx - (half - pad), dy - (half - pad));
                }

                float outerAlpha = Mathf.Clamp01(-d / 1.5f);
                float innerAlpha = Mathf.Clamp01((d + thickness) / 1.5f);
                float alpha = outerAlpha * innerAlpha;
                byte a = (byte)(alpha * 255f);
                pixels[y * size + x] = new Color32(255, 255, 255, a);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        _roundedSquareBorderCache[key] = sprite;
        return sprite;
    }

    public static Sprite GetPlayerArrow(int size = 64)
    {
        if (_playerArrowSprite != null) return _playerArrowSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        // Draw crisp player chevron arrow pointing UP
        for (int y = 0; y < size; y++)
        {
            float ny = (float)y / size; // 0 (bottom) to 1 (top)
            for (int x = 0; x < size; x++)
            {
                float nx = (float)x / size; // 0 (left) to 1 (right)
                float dCenter = Mathf.Abs(nx - 0.5f);

                // Triangle bounds: tip at (0.5, 0.92), base corners at (0.15, 0.12) and (0.85, 0.12), indent at (0.5, 0.30)
                float sideProgress = (0.92f - ny) / 0.80f;
                float maxWidth = sideProgress * 0.36f;
                float indentY = 0.12f + (0.5f - dCenter) * 0.36f;

                bool inTriangle = ny <= 0.92f && ny >= indentY && dCenter <= maxWidth;
                bool isBorder = inTriangle && (dCenter >= maxWidth - 0.05f || ny <= indentY + 0.06f || ny >= 0.87f);

                if (isBorder)
                {
                    pixels[y * size + x] = new Color32(20, 24, 30, 255); // Dark outline
                }
                else if (inTriangle)
                {
                    pixels[y * size + x] = new Color32(255, 255, 255, 255); // Bright white core
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _playerArrowSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _playerArrowSprite;
    }

    public static Sprite GetBlipDot(int size = 32)
    {
        if (_blipDotSprite != null) return _blipDotSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float center = size * 0.5f;
        float radius = center - 2f;

        for (int y = 0; y < size; y++)
        {
            float dy = y - center + 0.5f;
            for (int x = 0; x < size; x++)
            {
                float dx = x - center + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((radius - d) / 1.2f);
                float borderFactor = Mathf.Clamp01((d - (radius - 3f)) / 1.0f);
                byte r = (byte)Mathf.Lerp(255, 20, borderFactor);
                byte g = (byte)Mathf.Lerp(255, 24, borderFactor);
                byte b = (byte)Mathf.Lerp(255, 30, borderFactor);
                pixels[y * size + x] = new Color32(r, g, b, (byte)(alpha * 255f));
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipDotSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipDotSprite;
    }

    public static Sprite GetBlipPolice(int size = 40)
    {
        if (_blipPoliceSprite != null) return _blipPoliceSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        // Draw shield badge icon
        for (int y = 0; y < size; y++)
        {
            float ny = (float)y / size;
            for (int x = 0; x < size; x++)
            {
                float nx = (float)x / size;
                float dx = Mathf.Abs(nx - 0.5f);

                // Shield math: top flat/curved, bottom tapers to point (0.5, 0.1)
                float maxW = ny > 0.45f ? 0.38f : (0.10f + (ny - 0.10f) / 0.35f * 0.28f);
                bool inShield = ny >= 0.10f && ny <= 0.88f && dx <= maxW;
                bool isBorder = inShield && (dx >= maxW - 0.06f || ny <= 0.16f || ny >= 0.82f);

                if (isBorder)
                {
                    pixels[y * size + x] = new Color32(255, 255, 255, 255); // Crisp white rim
                }
                else if (inShield)
                {
                    pixels[y * size + x] = new Color32(40, 120, 240, 240); // Sapphire police blue
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipPoliceSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipPoliceSprite;
    }

    public static Sprite GetBlipDealer(int size = 40)
    {
        if (_blipDealerSprite != null) return _blipDealerSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float center = size * 0.5f;
        float radius = center - 3f;

        for (int y = 0; y < size; y++)
        {
            float dy = y - center + 0.5f;
            for (int x = 0; x < size; x++)
            {
                float dx = x - center + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((radius - d) / 1.5f);
                bool isBorder = d >= radius - 3.5f;

                if (isBorder)
                {
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255f));
                }
                else if (d < radius)
                {
                    pixels[y * size + x] = new Color32(46, 204, 113, (byte)(alpha * 255f)); // Emerald cash green
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipDealerSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipDealerSprite;
    }

    public static Sprite GetBlipProperty(int size = 40)
    {
        if (_blipPropertySprite != null) return _blipPropertySprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        // Draw house icon (triangle roof + rectangular base)
        for (int y = 0; y < size; y++)
        {
            float ny = (float)y / size;
            for (int x = 0; x < size; x++)
            {
                float nx = (float)x / size;
                float dx = Mathf.Abs(nx - 0.5f);

                bool inRoof = ny >= 0.50f && ny <= 0.88f && dx <= (0.88f - ny) * 0.95f;
                bool inBase = ny >= 0.12f && ny < 0.50f && dx <= 0.32f;
                bool inHouse = inRoof || inBase;

                if (inHouse)
                {
                    bool isBorder = (inRoof && (dx >= (0.88f - ny) * 0.95f - 0.06f || ny >= 0.82f)) ||
                                    (inBase && (dx >= 0.27f || ny <= 0.17f));

                    if (isBorder)
                        pixels[y * size + x] = new Color32(255, 255, 255, 255);
                    else
                        pixels[y * size + x] = new Color32(243, 156, 18, 250); // Amber orange
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipPropertySprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipPropertySprite;
    }

    public static Sprite GetBlipQuest(int size = 40)
    {
        if (_blipQuestSprite != null) return _blipQuestSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        // Draw diamond quest marker
        for (int y = 0; y < size; y++)
        {
            float ny = Mathf.Abs((float)y / size - 0.5f);
            for (int x = 0; x < size; x++)
            {
                float nx = Mathf.Abs((float)x / size - 0.5f);
                float d = nx + ny;
                bool inDiamond = d <= 0.40f;
                bool isBorder = d <= 0.40f && d >= 0.32f;

                if (isBorder)
                    pixels[y * size + x] = new Color32(255, 255, 255, 255);
                else if (inDiamond)
                    pixels[y * size + x] = new Color32(231, 76, 60, 245); // Ruby red / Quest yellow
                else
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipQuestSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipQuestSprite;
    }

    private static Sprite? _blipDealSprite;
    private static Sprite? _blipPotentialSprite;
    private static Sprite? _blipShopSprite;
    private static Sprite? _blipSleepingBagSprite;
    private static Sprite? _blipVehicleSprite;

    public static Sprite GetBlipDeal(int size = 44)
    {
        if (_blipDealSprite != null) return _blipDealSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float center = size * 0.5f;
        float radius = center - 2f;

        // Draw glowing circular deal badge with star center
        for (int y = 0; y < size; y++)
        {
            float dy = y - center + 0.5f;
            for (int x = 0; x < size; x++)
            {
                float dx = x - center + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((radius - d) / 1.5f);
                bool isBorder = d >= radius - 3.5f && d <= radius;

                if (isBorder)
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255f));
                else if (d < radius)
                    pixels[y * size + x] = new Color32(241, 196, 15, (byte)(alpha * 255f)); // Vibrant Sun Yellow / Gold
                else
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipDealSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipDealSprite;
    }

    public static Sprite GetBlipPotential(int size = 40)
    {
        if (_blipPotentialSprite != null) return _blipPotentialSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float center = size * 0.5f;
        float radius = center - 3f;

        // Draw cyan/turquoise flask/sample blip
        for (int y = 0; y < size; y++)
        {
            float dy = y - center + 0.5f;
            for (int x = 0; x < size; x++)
            {
                float dx = x - center + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((radius - d) / 1.5f);
                bool isBorder = d >= radius - 3.0f && d <= radius;

                if (isBorder)
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255f));
                else if (d < radius)
                    pixels[y * size + x] = new Color32(26, 188, 156, (byte)(alpha * 255f)); // Turquoise / Sample Cyan
                else
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipPotentialSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipPotentialSprite;
    }

    public static Sprite GetBlipShop(int size = 40)
    {
        if (_blipShopSprite != null) return _blipShopSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        // Draw shopping bag / store cart icon
        for (int y = 0; y < size; y++)
        {
            float ny = (float)y / size;
            for (int x = 0; x < size; x++)
            {
                float nx = (float)x / size;
                float dx = Mathf.Abs(nx - 0.5f);

                // Body: ny 0.15 to 0.65, dx <= 0.32; Handle: ny 0.65 to 0.85, arc
                bool inBody = ny >= 0.15f && ny <= 0.65f && dx <= 0.32f;
                bool inHandle = ny > 0.65f && ny <= 0.85f && dx <= 0.20f && dx >= 0.12f;
                bool isShape = inBody || inHandle;

                if (isShape)
                {
                    bool isBorder = (inBody && (dx >= 0.26f || ny <= 0.20f || ny >= 0.60f)) ||
                                    (inHandle && (ny >= 0.80f || dx >= 0.17f));

                    if (isBorder)
                        pixels[y * size + x] = new Color32(255, 255, 255, 255);
                    else
                        pixels[y * size + x] = new Color32(155, 89, 182, 245); // Royal Purple
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipShopSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipShopSprite;
    }

    public static Sprite GetBlipSleepingBag(int size = 40)
    {
        if (_blipSleepingBagSprite != null) return _blipSleepingBagSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        // Draw sleeping bag / tent triangle
        for (int y = 0; y < size; y++)
        {
            float ny = (float)y / size;
            for (int x = 0; x < size; x++)
            {
                float nx = (float)x / size;
                float dx = Mathf.Abs(nx - 0.5f);

                bool inTent = ny >= 0.18f && ny <= 0.82f && dx <= (0.82f - ny) * 0.55f;
                if (inTent)
                {
                    bool isBorder = dx >= (0.82f - ny) * 0.55f - 0.06f || ny <= 0.23f || ny >= 0.77f;
                    if (isBorder)
                        pixels[y * size + x] = new Color32(255, 255, 255, 255);
                    else
                        pixels[y * size + x] = new Color32(39, 174, 96, 245); // Camp Green
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipSleepingBagSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipSleepingBagSprite;
    }

    public static Sprite GetBlipVehicle(int size = 40)
    {
        if (_blipVehicleSprite != null) return _blipVehicleSprite;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        // Draw car / vehicle top-down chassis
        for (int y = 0; y < size; y++)
        {
            float ny = (float)y / size;
            for (int x = 0; x < size; x++)
            {
                float nx = (float)x / size;
                float dx = Mathf.Abs(nx - 0.5f);

                bool inCar = ny >= 0.15f && ny <= 0.85f && dx <= 0.28f;
                if (inCar)
                {
                    bool isBorder = dx >= 0.23f || ny <= 0.20f || ny >= 0.80f;
                    if (isBorder)
                        pixels[y * size + x] = new Color32(255, 255, 255, 255);
                    else
                        pixels[y * size + x] = new Color32(52, 152, 219, 245); // Sky Blue
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply();
        _blipVehicleSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _blipVehicleSprite;
    }
}

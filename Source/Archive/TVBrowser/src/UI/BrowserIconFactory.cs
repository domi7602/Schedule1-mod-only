using System;
using UnityEngine;

namespace TVBrowser.UI;

public static class BrowserIconFactory
{
    private static Sprite? _cachedIcon;

    public static Sprite GetOrCreateIcon()
    {
        if (_cachedIcon != null)
        {
            return _cachedIcon;
        }

        const int size = 64;
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
        {
            name = "TVBrowser_Icon",
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        var colors = new Color[size * size];
        var center = new Vector2(size / 2f, size / 2f);
        float radius = (size / 2f) - 3f;

        var bgDark = new Color(0.09f, 0.11f, 0.16f, 1f);
        var accentBlue = new Color(0.18f, 0.52f, 0.95f, 1f);
        var accentRed = new Color(0.92f, 0.22f, 0.22f, 1f);
        var borderCol = new Color(0.28f, 0.35f, 0.48f, 1f);
        var clear = new Color(0f, 0f, 0f, 0f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int index = y * size + x;
                float dist = Vector2.Distance(new Vector2(x, y), center);

                if (dist > radius + 1f)
                {
                    colors[index] = clear;
                }
                else if (dist > radius - 1.5f)
                {
                    float alpha = Mathf.Clamp01(radius + 1f - dist);
                    colors[index] = new Color(borderCol.r, borderCol.g, borderCol.b, alpha);
                }
                else
                {
                    // Gradient fill with top-right blue and bottom-left red tone
                    float t = (float)(x + y) / (size * 2);
                    var grad = Color.Lerp(accentRed * 0.4f, accentBlue * 0.6f, t);
                    var pixel = Color.Lerp(bgDark, grad, 0.55f);

                    // Draw browser top bar (horizontal line)
                    if (y == size - 16 && x > 14 && x < size - 14)
                    {
                        pixel = Color.white * 0.7f;
                    }
                    // Draw 3 dots for browser window
                    if (y == size - 12)
                    {
                        if ((x >= 16 && x <= 18) || (x >= 22 && x <= 24) || (x >= 28 && x <= 30))
                        {
                            pixel = Color.white * 0.9f;
                        }
                    }
                    // Draw a play triangle in the center for YouTube feel
                    float triLeft = 24f;
                    float triRight = 42f;
                    float triMidY = 26f;
                    if (x >= triLeft && x <= triRight)
                    {
                        float halfH = (x - triLeft) * 0.55f;
                        if (Mathf.Abs(y - triMidY) <= halfH)
                        {
                            pixel = Color.white;
                        }
                    }

                    colors[index] = pixel;
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        _cachedIcon = Sprite.Create(
            texture,
            new Rect(0, 0, size, size),
            new Vector2(0.5f, 0.5f),
            100f
        );

        return _cachedIcon;
    }
}

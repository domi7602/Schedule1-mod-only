using UnityEngine;

namespace BusinessIncome.Services;

/// <summary>
/// Notification icon for BusinessIncome HUD messages.
///
/// The vanilla NotificationsManager renders the icon slot as a plain filled
/// rectangle when no sprite is assigned (the "grey box" symptom seen on Taxi
/// before TaxiIcon existed), so every SendNotification call must pass a real
/// sprite. This tile is generated procedurally (green banknote tile with a
/// cream "$") and cached - no external PNG to load, no load warnings in the log.
/// </summary>
internal static class BusinessIcon
{
    private static Sprite? _cachedIconSprite;

    /// <summary>Generates and caches the icon sprite (never returns null).</summary>
    internal static Sprite Get()
    {
        if (_cachedIconSprite != null)
            return _cachedIconSprite;

        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = "BusinessIncome_Icon" };
        var green = new Color32(34, 120, 72, 255);
        var cream = new Color32(240, 240, 235, 255);
        var px = new Color32[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool inDollar =
                    (y >= 44 && y < 50 && x >= 18 && x < 46) ||   // top bar
                    (x >= 18 && x < 24 && y >= 29 && y < 50) ||   // upper-left stroke
                    (y >= 28 && y < 34 && x >= 18 && x < 46) ||   // middle bar
                    (x >= 40 && x < 46 && y >= 14 && y < 34) ||   // lower-right stroke
                    (y >= 14 && y < 20 && x >= 18 && x < 46) ||   // bottom bar
                    (x >= 29 && x < 35 && y >= 6 && y < 58);      // stem
                px[y * size + x] = inDollar ? cream : green;
            }
        }

        tex.SetPixels32(px);
        tex.Apply(false, true);
        _cachedIconSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _cachedIconSprite;
    }
}

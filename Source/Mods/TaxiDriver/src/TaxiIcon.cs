using System;
using System.IO;
using MelonLoader;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// The taxi icon, shared by the phone app and the fare notification: <c>taxi_icon.png</c>
/// from the game's <c>Mods</c> folder (deployed from <c>assets/</c>), with the procedural
/// yellow "T" fallback so a missing or broken file never logs load errors and never
/// renders as an empty white square.
/// </summary>
internal static class TaxiIcon
{
    private static Sprite? _cachedIconSprite;

    /// <summary>Loads and caches the icon (never returns null - the fallback is generated if needed).</summary>
    internal static Sprite Get()
    {
        if (_cachedIconSprite != null)
            return _cachedIconSprite;

        try
        {
            string path = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "taxi_icon.png");
            if (File.Exists(path))
            {
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (ImageConversion.LoadImage(tex, File.ReadAllBytes(path)))
                {
                    tex.name = "TaxiApp_Icon";
                    _cachedIconSprite = Sprite.Create(
                        tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    return _cachedIconSprite;
                }

                MelonLogger.Warning($"[TaxiApp] Icon '{path}' is not a decodable texture — using the procedural fallback.");
            }
            else
            {
                MelonLogger.Warning($"[TaxiApp] Icon '{path}' not found — using the procedural fallback.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[TaxiApp] Icon load failed ({ex.Message}) — using the procedural fallback.");
        }

        _cachedIconSprite = CreateFallbackIconSprite();
        return _cachedIconSprite;
    }

    /// <summary>Procedural yellow "T" icon — used when the PNG cannot be loaded.</summary>
    private static Sprite CreateFallbackIconSprite()
    {
        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = "TaxiApp_Icon_Fallback" };
        var yellow = new Color32(242, 194, 48, 255);
        var dark = new Color32(30, 34, 44, 255);
        var px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool inT = (y >= 42 && y < 52 && x >= 14 && x < 50) ||   // top bar
                           (x >= 27 && x < 37 && y >= 14 && y < 52);    // stem
                px[y * size + x] = inT ? dark : yellow;
            }
        }

        tex.SetPixels32(px);
        tex.Apply(false, true);
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }
}

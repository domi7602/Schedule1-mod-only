using System;
using System.IO;
using System.Reflection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace TownHallInterior;

/// <summary>
/// Provides visual sprites for Town Hall:
/// 1. Realistic architectural exterior photo for the real estate whiteboard card.
/// 2. Procedural classical icon fallback for UI notifications.
/// </summary>
internal static class TownHallIcon
{
    private static Sprite? _cachedListingSprite;
    private static Sprite? _cachedProceduralSprite;

    internal static Sprite Get()
    {
        return GetListingSprite() ?? GetProceduralIcon();
    }

    internal static Sprite? GetListingSprite()
    {
        if (_cachedListingSprite != null) return _cachedListingSprite;

        try
        {
            var asm = Assembly.GetExecutingAssembly();
            using var stream = asm.GetManifestResourceStream("TownHallInterior.TownHallListing");
            if (stream == null) return null;

            byte[] buffer = new byte[stream.Length];
            int read = 0;
            while (read < buffer.Length)
            {
                int r = stream.Read(buffer, read, buffer.Length - read);
                if (r <= 0) break;
                read += r;
            }

            var tex = new Texture2D(512, 384, TextureFormat.RGBA32, false)
            {
                name = "TownHallInterior_ListingTex",
                filterMode = FilterMode.Bilinear
            };

            Il2CppStructArray<byte> il2CppBytes = buffer;
            if (UnityEngine.ImageConversion.LoadImage(tex, il2CppBytes))
            {
                _cachedListingSprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f));
                return _cachedListingSprite;
            }
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"TownHallIcon: Failed to load listing photo: {ex.Message}");
        }

        return null;
    }

    internal static Sprite GetProceduralIcon()
    {
        if (_cachedProceduralSprite != null) return _cachedProceduralSprite;

        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
        {
            name = "TownHallInterior_Icon",
            filterMode = FilterMode.Bilinear
        };

        var navy = new Color32(26, 42, 68, 255);
        var gold = new Color32(212, 175, 55, 255);
        var cream = new Color32(245, 242, 230, 255);
        var pixels = new Color32[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int dx = x - 32;
                int dy = y - 32;
                if ((dx * dx) + (dy * dy) > 30 * 30)
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                    continue;
                }

                bool inRoof = y >= 40 && y <= 52 && Math.Abs(x - 32) <= (52 - y) * 2;
                bool inBase = y >= 12 && y <= 18 && x >= 12 && x <= 52;
                bool inBeam = y >= 37 && y <= 40 && x >= 14 && x <= 50;
                bool inColumn = y >= 18 && y <= 37 && (
                    (x >= 16 && x <= 20) ||
                    (x >= 25 && x <= 28) ||
                    (x >= 36 && x <= 39) ||
                    (x >= 44 && x <= 48));

                if (inRoof || inBase || inBeam)
                    pixels[y * size + x] = gold;
                else if (inColumn)
                    pixels[y * size + x] = cream;
                else
                    pixels[y * size + x] = navy;
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply(false, true);
        _cachedProceduralSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _cachedProceduralSprite;
    }
}

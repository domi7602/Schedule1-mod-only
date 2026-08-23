using System;
using Il2CppScheduleOne.UI;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

namespace DayCounter;

public static class DayCounterFont
{
    private static TMP_FontAsset? _cachedFont;
    private static Material? _cachedMaterial;

    private static readonly string[] PreferredFontNames = new[]
    {
        "Bebas",
        "Oswald",
        "Schedule",
        "Roboto",
        "Liberation"
    };

    public static (TMP_FontAsset font, Material? material) ResolveFont()
    {
        if (_cachedFont != null && _cachedFont.Pointer != IntPtr.Zero && !_cachedFont.WasCollected)
            return (_cachedFont, _cachedMaterial);

        _cachedFont = null;
        _cachedMaterial = null;

        // 1. Try resolving directly from active Vanilla BalanceDisplay
        try
        {
            if (HUD.Instance != null && HUD.Instance.Pointer != IntPtr.Zero && !HUD.Instance.WasCollected)
            {
                var bal = HUD.Instance.OnlineBalanceDisplay;
                if (bal != null && bal.Pointer != IntPtr.Zero && !bal.WasCollected && bal.BalanceLabel != null && bal.BalanceLabel.font != null)
                {
                    _cachedFont = bal.BalanceLabel.font;
                    _cachedMaterial = bal.BalanceLabel.fontSharedMaterial ?? _cachedFont.material;
                    MelonLogger.Msg($"Resolved font from BalanceDisplay: '{_cachedFont.name}'");
                    return (_cachedFont, _cachedMaterial);
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Error resolving font from BalanceDisplay: {ex.Message}");
        }

        // 2. Search scene TextMeshProUGUI instances for preferred fonts
        try
        {
            var tmps = UnityEngine.Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
            if (tmps != null && tmps.Length > 0)
            {
                foreach (var pref in PreferredFontNames)
                {
                    foreach (var tmp in tmps)
                    {
                        if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                        {
                            string fontName = tmp.font.name ?? "";
                            if (fontName.Contains(pref, StringComparison.OrdinalIgnoreCase))
                            {
                                _cachedFont = tmp.font;
                                _cachedMaterial = tmp.fontSharedMaterial ?? _cachedFont.material;
                                MelonLogger.Msg($"Resolved preferred font from Scene TextMeshProUGUI: '{fontName}'");
                                return (_cachedFont, _cachedMaterial);
                            }
                        }
                    }
                }

                // Fallback to first valid scene TMP font
                foreach (var tmp in tmps)
                {
                    if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                    {
                        _cachedFont = tmp.font;
                        _cachedMaterial = tmp.fontSharedMaterial ?? _cachedFont.material;
                        MelonLogger.Msg($"Resolved fallback font from Scene: '{_cachedFont.name}'");
                        return (_cachedFont, _cachedMaterial);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Error scanning scene TMP fonts: {ex.Message}");
        }

        // 3. Fallback from Resources
        try
        {
            var fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
            if (fonts != null && fonts.Count > 0)
            {
                foreach (var pref in PreferredFontNames)
                {
                    foreach (var f in fonts)
                    {
                        if (f != null && (UnityEngine.Object)f != null)
                        {
                            string fontName = f.name ?? "";
                            if (fontName.Contains(pref, StringComparison.OrdinalIgnoreCase))
                            {
                                _cachedFont = f;
                                _cachedMaterial = f.material;
                                MelonLogger.Msg($"Resolved preferred font from Resources: '{fontName}'");
                                return (_cachedFont, _cachedMaterial);
                            }
                        }
                    }
                }

                _cachedFont = fonts[0];
                _cachedMaterial = _cachedFont.material;
                MelonLogger.Msg($"Fallback font from Resources: '{_cachedFont.name}'");
                return (_cachedFont, _cachedMaterial);
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Error scanning Resource fonts: {ex.Message}");
        }

        MelonLogger.Warning("Failed to resolve any TMP font asset.");
        return (null!, null);
    }

    public static void ResetCache()
    {
        _cachedFont = null;
        _cachedMaterial = null;
    }
}

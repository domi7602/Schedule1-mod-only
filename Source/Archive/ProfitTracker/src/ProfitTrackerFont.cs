using System;
using Il2CppScheduleOne.UI;
using Il2CppTMPro;
using UnityEngine;

namespace ProfitTracker;

public static class ProfitTrackerFont
{
    private static TMP_FontAsset? _cachedFont;
    private static Material? _cachedMaterial;

    public static (TMP_FontAsset font, Material? material) ResolveFont()
    {
        if (_cachedFont != null && _cachedFont.Pointer != IntPtr.Zero && !_cachedFont.WasCollected)
            return (_cachedFont, _cachedMaterial);
        _cachedFont = null;
        _cachedMaterial = null;

        // 1. Try resolving from active Vanilla BalanceDisplay
        try
        {
            if (HUD.Instance != null && HUD.Instance.Pointer != IntPtr.Zero && !HUD.Instance.WasCollected)
            {
                var bal = HUD.Instance.OnlineBalanceDisplay;
                if (bal != null && bal.Pointer != IntPtr.Zero && !bal.WasCollected && bal.BalanceLabel != null && bal.BalanceLabel.font != null)
                {
                    _cachedFont = bal.BalanceLabel.font;
                    _cachedMaterial = bal.BalanceLabel.fontSharedMaterial ?? _cachedFont.material;
                    return (_cachedFont, _cachedMaterial);
                }
            }
        }
        catch { }

        // 2. Search scene TextMeshProUGUI instances
        try
        {
            var tmps = UnityEngine.Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
            if (tmps != null && tmps.Length > 0)
            {
                foreach (var tmp in tmps)
                {
                    if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                    {
                        string name = tmp.font.name ?? "";
                        if (name.Contains("Bebas", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("Roboto", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("Oswald", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("Liberation", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("Schedule", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("SDF", StringComparison.OrdinalIgnoreCase))
                        {
                            _cachedFont = tmp.font;
                            _cachedMaterial = tmp.fontSharedMaterial ?? _cachedFont.material;
                            return (_cachedFont, _cachedMaterial);
                        }
                    }
                }

                // Fallback to first available scene tmp font
                foreach (var tmp in tmps)
                {
                    if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                    {
                        _cachedFont = tmp.font;
                        _cachedMaterial = tmp.fontSharedMaterial ?? _cachedFont.material;
                        return (_cachedFont, _cachedMaterial);
                    }
                }
            }
        }
        catch { }

        // 3. Fallback from Resources
        try
        {
            var fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
            if (fonts != null && fonts.Count > 0)
            {
                // Score known fonts
                foreach (var f in fonts)
                {
                    if (f != null && (UnityEngine.Object)f != null)
                    {
                        string name = f.name ?? "";
                        if (name.Contains("Bebas", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("Roboto", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("Oswald", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("Liberation", StringComparison.OrdinalIgnoreCase) ||
                            name.Contains("SDF", StringComparison.OrdinalIgnoreCase))
                        {
                            _cachedFont = f;
                            _cachedMaterial = f.material;
                            return (_cachedFont, _cachedMaterial);
                        }
                    }
                }

                _cachedFont = fonts[0];
                _cachedMaterial = _cachedFont.material;
                return (_cachedFont, _cachedMaterial);
            }
        }
        catch { }

        return (null!, null);
    }

    public static void ResetCache()
    {
        _cachedFont = null;
        _cachedMaterial = null;
    }
}

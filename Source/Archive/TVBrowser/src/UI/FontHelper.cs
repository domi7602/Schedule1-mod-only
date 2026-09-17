using System;
using Il2CppTMPro;
using UnityEngine;

namespace TVBrowser.UI;

public static class FontHelper
{
    private static TMP_FontAsset? _cachedFont;

    public static void InvalidateCache()
    {
        _cachedFont = null;
    }

    public static TMP_FontAsset GetFont(Transform? context = null)
    {
        if (_cachedFont != null && _cachedFont.Pointer != IntPtr.Zero && !_cachedFont.WasCollected)
        {
            return _cachedFont;
        }
        _cachedFont = null;

        // 1. Search in context
        if (context != null)
        {
            var tmps = context.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var tmp in tmps)
            {
                if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                {
                    _cachedFont = tmp.font;
                    return _cachedFont;
                }
            }
        }

        // 2. Search scene objects
        var sceneTmps = UnityEngine.Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        if (sceneTmps != null)
        {
            foreach (var tmp in sceneTmps)
            {
                if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                {
                    _cachedFont = tmp.font;
                    return _cachedFont;
                }
            }
        }

        // 3. Resources fallback
        var fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        if (fonts != null && fonts.Count > 0)
        {
            foreach (var f in fonts)
            {
                if (f != null && f.Pointer != IntPtr.Zero && !f.WasCollected && !string.IsNullOrEmpty(f.name))
                {
                    _cachedFont = f;
                    return _cachedFont;
                }
            }
        }

        return _cachedFont!;
    }
}

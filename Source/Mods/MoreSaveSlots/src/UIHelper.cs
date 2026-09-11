using System;
using Il2CppInterop.Runtime;
using Il2CppTMPro;
using MelonLoader;
using S1API.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreSaveSlots.UI;

public static class UIHelper
{
    private static TMP_FontAsset? _cachedFont;
    private static Material? _cachedFontMaterial;
    private static Sprite? _solidSprite;

    public static Sprite GetSolidSprite()
    {
        if (_solidSprite != null) return _solidSprite;

        Texture2D texture = new Texture2D(4, 4, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[16];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        _solidSprite = Sprite.Create(texture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
        return _solidSprite;
    }

    public static TMP_FontAsset? GetFont(Transform? context = null)
    {
        // Cached fonts die with the scene (IL2CPP wrapper survives) — re-resolve.
        if (IsAlive(_cachedFont) && IsAlive(_cachedFontMaterial)) return _cachedFont;
        _cachedFont = null;
        _cachedFontMaterial = null;

        // 1. Search in context hierarchy (IL2CPP-safe indexed for loops)
        if (context != null)
        {
            var tmps = context.GetComponentsInChildren<TextMeshProUGUI>(true);
            if (tmps != null)
            {
                for (int i = 0; i < tmps.Length; i++)
                {
                    var tmp = tmps[i];
                    if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                    {
                        _cachedFont = tmp.font;
                        _cachedFontMaterial = tmp.fontSharedMaterial ?? tmp.font.material;
                        return _cachedFont;
                    }
                }
            }

            // Search up root
            if (context.root != null)
            {
                var rootTmps = context.root.GetComponentsInChildren<TextMeshProUGUI>(true);
                if (rootTmps != null)
                {
                    for (int i = 0; i < rootTmps.Length; i++)
                    {
                        var tmp = rootTmps[i];
                        if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                        {
                            _cachedFont = tmp.font;
                            _cachedFontMaterial = tmp.fontSharedMaterial ?? tmp.font.material;
                            return _cachedFont;
                        }
                    }
                }
            }
        }

        // 2. Search scene objects
        // Gatekeeper-fix 2026-08-29: FindObjectsOfType<T>() is [Obsolete] in Unity 2022.3+ (CS0618).
        // Migrated to FindObjectsByType with explicit FindObjectsSortMode.None (no allocation, faster).
        var sceneTmps = UnityEngine.Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        if (sceneTmps != null)
        {
            for (int i = 0; i < sceneTmps.Length; i++)
            {
                var tmp = sceneTmps[i];
                if (tmp != null && tmp.Pointer != IntPtr.Zero && !tmp.WasCollected && tmp.font != null)
                {
                    _cachedFont = tmp.font;
                    _cachedFontMaterial = tmp.fontSharedMaterial ?? tmp.font.material;
                    return _cachedFont;
                }
            }
        }

        // 3. Resources fallback — cold-path one-shot: cached via _cachedFont so subsequent calls hit the early return above and avoid re-scan.
        MelonLogger.Msg("[MoreSaveSlots] GetFont: entering Resources fallback (cold-path, one-shot cached via _cachedFont)");
        var fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        if (fonts != null && fonts.Length > 0)
        {
            for (int i = 0; i < fonts.Length; i++)
            {
                var f = fonts[i];
                if (f != null && f.Pointer != IntPtr.Zero && !f.WasCollected && !string.IsNullOrEmpty(f.name))
                {
                    _cachedFont = f;
                    _cachedFontMaterial = f.material;
                    return _cachedFont;
                }
            }
        }

        return null;
    }

    public static TextMeshProUGUI CreateTextMeshPro(Transform parent, string name, string text, float fontSize, FontStyles style, TextAlignmentOptions align, Color color)
    {
        GameObject obj = new GameObject(name, Il2CppType.Of<RectTransform>());
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        var font = GetFont(parent);
        if (font != null)
        {
            tmp.font = font;
            if (_cachedFontMaterial != null)
            {
                tmp.fontSharedMaterial = _cachedFontMaterial;
            }
        }

        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = align;
        tmp.color = color;
        tmp.raycastTarget = false;
        tmp.SetVerticesDirty();
        return tmp;
    }

    public static Button CreateButton(
        Transform parent,
        string name,
        string labelText,
        float width,
        float height,
        Color normalCol,
        Color hoverCol,
        Color pressedCol,
        Color disabledCol,
        Action onClickAction,
        out TextMeshProUGUI labelTmp)
    {
        GameObject btnObj = new GameObject(name, Il2CppType.Of<RectTransform>());
        btnObj.transform.SetParent(parent, false);

        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);

        Image img = btnObj.AddComponent<Image>();
        img.sprite = GetSolidSprite();
        img.type = Image.Type.Simple;
        img.color = normalCol;
        img.raycastTarget = true;

        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = img;

        ColorBlock colors = btn.colors;
        colors.normalColor = normalCol;
        colors.highlightedColor = hoverCol;
        colors.pressedColor = pressedCol;
        colors.selectedColor = hoverCol;
        colors.disabledColor = disabledCol;
        colors.colorMultiplier = 1.0f;
        colors.fadeDuration = 0.08f;
        btn.colors = colors;

        // Use S1API EventHelper for IL2CPP-safe delegate marshalling (AGENTS.md §5)
        EventHelper.AddListener(() =>
        {
            try { onClickAction.Invoke(); }
            catch (Exception ex) { MelonLogger.Error($"[MoreSaveSlots] Error executing click on '{name}': {ex}"); }
        }, btn.onClick);

        labelTmp = CreateTextMeshPro(btnObj.transform, "Label", labelText, 12f, FontStyles.Bold, TextAlignmentOptions.Center, Color.white);
        RectTransform textRt = labelTmp.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        return btn;
    }

    private static bool IsAlive(UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected && (UnityEngine.Object)obj != null; }
        catch { return false; }
    }
}

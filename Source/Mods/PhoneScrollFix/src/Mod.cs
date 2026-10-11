using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(PhoneScrollFix.Mod), "PhoneScrollFix", "2.0.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PhoneScrollFix;

/// <summary>
/// Fixes the white hover plate on phone home-screen app icons.
///
/// Root cause (live-confirmed 2026-10-11, S1MCP + in-mod field watcher):
/// hovering an icon fades in its "Outline" child — a filled white rounded-rect
/// graphic. Two things make it look broken once S1API's scroll patch exists:
///   1. the fill covers the whole cell (184x185) while the icon art is smaller,
///      so the hover reads as a big white plate instead of a highlight;
///   2. the graphic is non-maskable in vanilla, so it escapes the scroll
///      viewport's clipping and keeps rendering full-size over half-scrolled
///      icons (the "white plate over a clipped icon" report).
///
/// Fixes applied per icon (visuals only, no gameplay or config touched):
///   - hover sprite replaced with a slim procedurally generated rounded ring
///     (transparent centre) -> frame instead of plate;
///   - Image.maskable = true -> the highlight is clipped exactly like the art;
///   - frame alpha 0.85 -> soft, vanilla-looking highlight.
/// Plus a stencil-free RectMask2D on the scroll viewport (replaces S1API's
/// stencil Mask, which misrendered the same boundary).
///
/// Ordering-proof: polls until applied, so it runs after S1API's
/// HomeScreenScrollPatch regardless of mod load order; re-checks after scene
/// loads. Removal: delete PhoneScrollFix.dll.
/// </summary>
public class Mod : MelonMod
{
    /// <summary>Colour alpha of the hover frame (multiplies the vanilla fade).</summary>
    private const float HoverFrameAlpha = 0.85f;

    private bool _maskSwapped;
    private float _nextCheckTime;
    private readonly HashSet<string> _framed = new();
    private static Sprite? _ringSprite;

    public override void OnInitializeMelon()
    {
        _maskSwapped = false;
        _nextCheckTime = 0f;
        _framed.Clear();
        MelonLogger.Msg("[PhoneScrollFix] initialized v2.0.0 — waiting for the phone home screen.");
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        _maskSwapped = false;
        _nextCheckTime = 0f;
        _framed.Clear();
    }

    public override void OnUpdate()
    {
        if (Time.time < _nextCheckTime) return;
        _nextCheckTime = Time.time + 2f;
        try
        {
            GameObject home = GameObject.Find("HomeScreen");
            if (home == null) return;

            if (!_maskSwapped)
                _maskSwapped = TrySwapViewportMask(home.transform);

            Transform icons = home.transform.Find("AppIconsScrollView/Viewport/AppIcons");
            if (icons == null) return;
            for (int i = 0; i < icons.childCount; i++)
            {
                Transform child = icons.GetChild(i);
                if (child != null) ApplyHoverFrame(child);
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[PhoneScrollFix] check failed (will retry): {ex.Message}");
        }
    }

    /// <summary>
    /// Replaces S1API's stencil viewport Mask with a RectMask2D — stencil-free
    /// rectangular clipping that does not misrender at the scroll boundary.
    /// </summary>
    private static bool TrySwapViewportMask(Transform home)
    {
        Transform viewport = home.Find("AppIconsScrollView/Viewport");
        if (viewport == null) return false; // scroll patch not active (yet)
        if (viewport.GetComponent<RectMask2D>() != null) return true;
        Mask mask = viewport.GetComponent<Mask>();
        if (mask == null) return true;
        UnityEngine.Object.Destroy(mask);
        Image maskImage = viewport.GetComponent<Image>();
        if (maskImage != null) UnityEngine.Object.Destroy(maskImage);
        viewport.gameObject.AddComponent<RectMask2D>();
        MelonLogger.Msg("[PhoneScrollFix] viewport Mask -> RectMask2D.");
        return true;
    }

    /// <summary>
    /// Turns each icon's hover highlight into a slim frame that clips with the
    /// icon: ring sprite + maskable. Applied once per icon.
    /// </summary>
    private void ApplyHoverFrame(Transform icon)
    {
        string key = icon.gameObject.name + "#" + icon.GetSiblingIndex();
        if (_framed.Contains(key)) return;
        Transform outline = icon.Find("Outline");
        if (outline == null) return;
        Image img = outline.GetComponent<Image>();
        if (img == null) return;
        _framed.Add(key);
        try
        {
            img.sprite = GetRingSprite();
            img.type = Image.Type.Sliced;
            img.pixelsPerUnitMultiplier = 1f;
            img.color = new Color(1f, 1f, 1f, HoverFrameAlpha);
            img.maskable = true;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[PhoneScrollFix] hover frame failed on '{icon.gameObject.name}': {ex.Message}");
        }
    }

    /// <summary>Rounded-rect ring sprite: white 8 px frame on a transparent 96x96 canvas.</summary>
    private static Sprite GetRingSprite()
    {
        if (_ringSprite != null) return _ringSprite;
        const int size = 96;
        const int thickness = 8;
        const float radius = 26f;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        var white = Color.white;
        var clear = new Color(1f, 1f, 1f, 0f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool outer = InRoundedRect(x, y, size, 0f, radius);
                bool inner = InRoundedRect(x, y, size, thickness, Mathf.Max(radius - thickness, 1f));
                tex.SetPixel(x, y, outer && !inner ? white : clear);
            }
        }
        tex.Apply();
        var border = new Vector4(radius + thickness, radius + thickness, radius + thickness, radius + thickness);
        try
        {
            _ringSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
        }
        catch
        {
            _ringSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }
        _ringSprite.name = "PhoneScrollFix_Ring";
        return _ringSprite;
    }

    private static bool InRoundedRect(int x, int y, int size, float inset, float radius)
    {
        float x0 = inset, x1 = size - 1 - inset, y0 = inset, y1 = size - 1 - inset;
        if (x < x0 || x > x1 || y < y0 || y > y1) return false;
        float cx = x < x0 + radius ? x0 + radius : (x > x1 - radius ? x1 - radius : x);
        float cy = y < y0 + radius ? y0 + radius : (y > y1 - radius ? y1 - radius : y);
        float dx = x - cx, dy = y - cy;
        return dx * dx + dy * dy <= radius * radius;
    }
}

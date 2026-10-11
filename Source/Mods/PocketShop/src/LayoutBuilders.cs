using System;
using UnityEngine;
using UnityEngine.UI;
using GamePalette = S1Mods.Shared.GamePalette;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

internal static class AppHeaderBuilder
{
    /// <summary>Shared optical height for every header control (search, back, close).</summary>
    private const float ControlHeightDp = 26f;

    /// <summary>
    /// Builds the app header: [&lt;]  [icon] PocketShop  [ search field ]  [X].
    /// Modern compact style: sentence-case upright title in soft white-blue, muted dark
    /// controls of equal height, a compact dark close button with a subtle red edge.
    /// Returns the search InputField so the app can attach its focus guard; the back
    /// button is returned so the app can hide it on the top-level directory.
    /// </summary>
    public static InputField Build(Transform parent, Action onBack, Action onClose, Action<string> onSearchChanged, out GameObject backButton)
    {
        var header = S1API.UI.UIFactory.Panel("AppHeader", parent, GamePalette.Header);
        var hLE = header.AddComponent<LayoutElement>();
        hLE.minHeight = UITheme.Dp(46f);
        hLE.preferredHeight = UITheme.Dp(46f);
        hLE.flexibleHeight = 0f;

        var hlg = header.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;
        hlg.childAlignment = TextAnchor.MiddleLeft;

        // Back button (left) — same muted style as the close button, no loud color plate.
        backButton = BuildIconButton(header.transform, "<", new Color(0.13f, 0.17f, 0.24f, 1f), new Color(0.78f, 0.83f, 0.90f, 1f), 26f, onBack);

        // Title (no emoji glyphs — Arial renders them as blank boxes)
        BuildTitle(header.transform);

        // Live search
        var search = BuildSearchField(header.transform, onSearchChanged);

        // X close button: compact dark anthracite-red with a subtle red edge.
        BuildIconButton(header.transform, "X", new Color(0.26f, 0.12f, 0.13f, 1f), Color.white, 26f, onClose, redEdge: true);

        return search;
    }

    private static void BuildTitle(Transform parent)
    {
        // Small drawn storefront glyph (cyan) instead of an emoji character.
        var glyphGO = new GameObject("TitleGlyph");
        glyphGO.transform.SetParent(parent, false);
        glyphGO.AddComponent<RectTransform>();
        var gLE = glyphGO.AddComponent<LayoutElement>();
        gLE.preferredWidth = UITheme.Dp(16f);
        gLE.minWidth = UITheme.Dp(16f);
        gLE.preferredHeight = UITheme.Dp(16f);
        gLE.minHeight = UITheme.Dp(16f);
        var gImg = glyphGO.AddComponent<Image>();
        gImg.sprite = GetShopGlyph();
        gImg.preserveAspect = true;
        gImg.raycastTarget = false;

        var titleTxt = S1API.UI.UIFactory.Text("Title", "PocketShop", parent, UITheme.Sp(14), TextAnchor.MiddleLeft, FontStyle.Bold);
        titleTxt.color = new Color(0.84f, 0.93f, 1.00f, 1f);
        titleTxt.raycastTarget = false;
        titleTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
        var titleLE = titleTxt.gameObject.AddComponent<LayoutElement>();
        titleLE.minWidth = UITheme.Dp(86f);
        titleLE.preferredWidth = UITheme.Dp(98f);
    }

    private static Sprite? _shopGlyph;

    /// <summary>Tiny cyan storefront icon (awning + door) drawn at runtime; no emoji glyphs.</summary>
    private static Sprite GetShopGlyph()
    {
        if (_shopGlyph != null && _shopGlyph.Pointer != IntPtr.Zero && !_shopGlyph.WasCollected)
        {
            return _shopGlyph;
        }

        const int s = 24;
        var tex = new Texture2D(s, s, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        var px = new Color32[s * s];
        var clear = new Color32(0, 0, 0, 0);
        for (int i = 0; i < px.Length; i++) px[i] = clear;

        var cyan = new Color32(51, 217, 242, 255);
        var cyanDim = new Color32(36, 140, 165, 255);
        var dark = new Color32(16, 48, 62, 255);

        FillRect(px, s, 4, 8, 16, 11, cyanDim);     // body
        FillRect(px, s, 3, 16, 18, 4, cyan);        // awning
        for (int x = 5; x <= 18; x += 6) FillRect(px, s, x, 16, 3, 4, dark);   // awning stripes
        FillRect(px, s, 10, 8, 5, 7, dark);         // door

        tex.SetPixels32(px);
        tex.Apply(false, true);
        _shopGlyph = Sprite.Create(tex, new Rect(0, 0, s, s), new Vector2(0.5f, 0.5f), 100f);
        return _shopGlyph;
    }

    private static void FillRect(Color32[] pixels, int size, int x, int y, int w, int h, Color32 c)
    {
        for (int py = y; py < y + h; py++)
        {
            for (int px = x; px < x + w; px++)
            {
                if (px >= 0 && px < size && py >= 0 && py < size)
                {
                    pixels[py * size + px] = c;
                }
            }
        }
    }

    /// <summary>
    /// Slim dark search field (capsule surface). The surface keeps raycastTarget = true -
    /// it is the field's click target and the click bubbles up to the InputField on this
    /// object (Rule 17: a decor rule that disables it makes the field dead).
    /// </summary>
    private static InputField BuildSearchField(Transform parent, Action<string> onSearchChanged)
    {
        var go = new GameObject("PocketShop_Search");
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        var le = go.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
        le.minWidth = UITheme.Dp(150f);
        le.preferredHeight = UITheme.Dp(ControlHeightDp);
        le.minHeight = UITheme.Dp(ControlHeightDp);
        le.flexibleHeight = 0f;

        var bg = S1API.UI.UIFactory.Panel("Background", go.transform, new Color(0.07f, 0.11f, 0.19f, 1f), fullAnchor: true);
        var bgImg = bg.GetComponent<Image>();
        if (bgImg != null)
        {
            bgImg.sprite = S1Mods.Shared.UISprites.Capsule();
            bgImg.type = Image.Type.Sliced;
            bgImg.raycastTarget = true;
        }

        // Subtle edge so the field reads as a field, not as a dark hole in the header.
        var border = bg.AddComponent<Outline>();
        border.effectColor = new Color(0.35f, 0.45f, 0.62f, 0.35f);
        border.effectDistance = new Vector2(1f, -1f);

        var field = go.AddComponent<InputField>();
        field.targetGraphic = bgImg;

        var text = S1API.UI.UIFactory.Text("Text", string.Empty, go.transform, UITheme.Sp(13), TextAnchor.MiddleLeft);
        Stretch(text.rectTransform, 12f, 30f);
        text.color = new Color(0.92f, 0.94f, 0.97f, 1f);
        text.supportRichText = false;
        text.raycastTarget = false;

        var placeholder = S1API.UI.UIFactory.Text("Placeholder", "Search...", go.transform, UITheme.Sp(13), TextAnchor.MiddleLeft);
        Stretch(placeholder.rectTransform, 12f, 30f);
        placeholder.color = new Color(0.55f, 0.60f, 0.68f, 1f);
        placeholder.supportRichText = false;
        placeholder.raycastTarget = false;

        field.textComponent = text;
        field.placeholder = placeholder;
        field.caretWidth = 2;
        field.caretColor = new Color(0.92f, 0.94f, 0.97f, 1f);
        field.selectionColor = new Color(0.20f, 0.85f, 0.95f, 0.30f);

        if (onSearchChanged != null)
        {
            try { S1API.Utils.EventHelper.AddListener<string>(value => onSearchChanged(value), field.onValueChanged); }
            catch (Exception ex) { MelonLoader.MelonLogger.Warning($"[PocketShop] search wiring failed: {ex.Message}"); }
        }

        var clear = BuildClearButton(go.transform);
        S1API.Utils.ButtonUtils.AddListener(clear, () => field.text = string.Empty);

        return field;
    }

    /// <summary>Small "x" at the right edge of the search field; clears the query.</summary>
    private static Button BuildClearButton(Transform parent)
    {
        var panel = S1API.UI.UIFactory.Panel("ClearSearch", parent, Color.clear);
        var rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(1f, 0.5f);
        rt.anchorMax = new Vector2(1f, 0.5f);
        rt.pivot = new Vector2(1f, 0.5f);
        rt.sizeDelta = new Vector2(UITheme.Dp(24f), UITheme.Dp(24f));
        rt.anchoredPosition = new Vector2(-UITheme.Dp(4f), 0f);

        var btn = panel.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;

        var txt = S1API.UI.UIFactory.Text("Txt", "x", panel.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        Stretch(txt.rectTransform, 0f, 0f);
        txt.color = GamePalette.TextMuted;
        txt.raycastTarget = false;
        return btn;
    }

    private static void Stretch(RectTransform rt, float leftDp, float rightDp)
    {
        if (rt == null) return;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(UITheme.Dp(leftDp), 0f);
        rt.offsetMax = new Vector2(-UITheme.Dp(rightDp), 0f);
    }

    /// <summary>
    /// Compact square-ish control (back / close). Muted dark plate of the shared control
    /// height; the close button gets a subtle red edge instead of a saturated red fill.
    /// </summary>
    private static GameObject BuildIconButton(Transform parent, string label, Color bg, Color fg, float widthDp, Action onClick, bool redEdge = false)
    {
        var panel = S1API.UI.UIFactory.Panel($"Btn_{label}", parent, bg);
        var btn = panel.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        var le = panel.AddComponent<LayoutElement>();
        le.preferredWidth = UITheme.Dp(widthDp);
        le.minWidth = UITheme.Dp(widthDp);
        le.preferredHeight = UITheme.Dp(ControlHeightDp);
        le.minHeight = UITheme.Dp(ControlHeightDp);
        le.flexibleHeight = 0f;

        if (redEdge)
        {
            var edge = panel.AddComponent<Outline>();
            edge.effectColor = new Color(0.78f, 0.30f, 0.30f, 0.55f);
            edge.effectDistance = new Vector2(1f, -1f);
        }

        var vlg = panel.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = true;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        var txt = S1API.UI.UIFactory.Text("Txt", label, panel.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
        txt.color = fg;
        txt.raycastTarget = false;
        S1API.Utils.ButtonUtils.AddListener(btn, onClick);
        return panel;
    }
}



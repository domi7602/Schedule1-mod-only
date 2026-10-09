using System;
using UnityEngine;
using UnityEngine.UI;
using GamePalette = S1Mods.Shared.GamePalette;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

internal static class AppHeaderBuilder
{
    /// <summary>
    /// Builds the app header: [&lt;]  POCKETSHOP  [ search field ]  [X].
    /// The emoji "shopping bag" glyph was dropped (Arial renders emoji as blanks, so it
    /// showed as an empty gap) and a live search field was added in the same row.
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
        hlg.childForceExpandHeight = true;
        hlg.childAlignment = TextAnchor.MiddleLeft;

        // Back button (left)
        backButton = BuildButton(header.transform, "<", GamePalette.CardAlt, GamePalette.TextPrimary, 36, onBack);

        // Title (no emoji glyph - it rendered as a blank box in Arial)
        var titleTxt = S1API.UI.UIFactory.Text("Title", "POCKETSHOP", header.transform, UITheme.Sp(15), TextAnchor.MiddleLeft, FontStyle.Bold | FontStyle.Italic);
        titleTxt.color = new Color(0.20f, 0.85f, 0.95f, 1f);
        titleTxt.raycastTarget = false;
        titleTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
        var titleLE = titleTxt.gameObject.AddComponent<LayoutElement>();
        titleLE.minWidth = UITheme.Dp(118f);
        titleLE.preferredWidth = UITheme.Dp(150f);

        // Live search
        var search = BuildSearchField(header.transform, onSearchChanged);

        // X close button (palette red)
        BuildButton(header.transform, "X", GamePalette.Red, GamePalette.TextPrimary, 32, onClose);

        return search;
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
        le.preferredHeight = UITheme.Dp(28f);
        le.minHeight = UITheme.Dp(28f);

        var bg = S1API.UI.UIFactory.Panel("Background", go.transform, GamePalette.Card, fullAnchor: true);
        var bgImg = bg.GetComponent<Image>();
        if (bgImg != null)
        {
            bgImg.sprite = S1Mods.Shared.UISprites.Capsule();
            bgImg.type = Image.Type.Sliced;
            bgImg.raycastTarget = true;
        }

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

    private static GameObject BuildButton(Transform parent, string label, Color bg, Color fg, int widthDp, Action onClick)
    {
        var panel = S1API.UI.UIFactory.Panel($"Btn_{label}", parent, bg);
        var btn = panel.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        var le = panel.AddComponent<LayoutElement>();
        le.preferredWidth = UITheme.Dp(widthDp);
        le.minWidth = UITheme.Dp(widthDp);
        le.preferredHeight = UITheme.Dp(26f);
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



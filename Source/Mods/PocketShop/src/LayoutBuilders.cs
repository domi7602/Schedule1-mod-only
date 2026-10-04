using System;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

internal static class AppHeaderBuilder
{
    /// <summary>
    /// Builds the app header: [&lt;]  POCKETSHOP  [ search field ]  [X].
    /// The emoji "shopping bag" glyph was dropped (Arial renders emoji as blanks, so it
    /// showed as an empty gap) and a live search field was added in the same row.
    /// Returns the search InputField so the app can attach its focus guard.
    /// </summary>
    public static InputField Build(Transform parent, Action onBack, Action onClose, Action<string> onSearchChanged)
    {
        var header = S1API.UI.UIFactory.Panel("AppHeader", parent, new Color(0.08f, 0.10f, 0.14f, 1f));
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
        BuildButton(header.transform, "<", new Color(0.18f, 0.21f, 0.28f, 1f), Color.white, 36, onBack);

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

        // X close button (coral red)
        BuildButton(header.transform, "X", new Color(0.85f, 0.28f, 0.35f, 1f), Color.white, 32, onClose);

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

        var bg = S1API.UI.UIFactory.Panel("Background", go.transform, new Color(0.14f, 0.16f, 0.20f, 1f), fullAnchor: true);
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
        Stretch(text.rectTransform, 12f, 10f);
        text.color = new Color(0.92f, 0.94f, 0.97f, 1f);
        text.supportRichText = false;
        text.raycastTarget = false;

        var placeholder = S1API.UI.UIFactory.Text("Placeholder", "Search...", go.transform, UITheme.Sp(13), TextAnchor.MiddleLeft);
        Stretch(placeholder.rectTransform, 12f, 10f);
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

        return field;
    }

    private static void Stretch(RectTransform rt, float leftDp, float rightDp)
    {
        if (rt == null) return;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(UITheme.Dp(leftDp), 0f);
        rt.offsetMax = new Vector2(-UITheme.Dp(rightDp), 0f);
    }

    private static void BuildButton(Transform parent, string label, Color bg, Color fg, int widthDp, Action onClick)
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
    }
}

internal static class FooterBuilder
{
    public static void Build(Transform parent)
    {
        var footer = S1API.UI.UIFactory.Panel("Footer", parent, new Color(0.04f, 0.05f, 0.07f, 1f));
        var le = footer.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(16f);
        le.preferredHeight = UITheme.Dp(16f);
        le.flexibleHeight = 0f;

        var hlg = footer.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.padding = new RectOffset(0, 0, 0, 0);

        var txt = S1API.UI.UIFactory.Text("Version", $"PocketShop v{Mod.Version}", footer.transform, UITheme.Sp(9), TextAnchor.MiddleCenter);
        txt.color = new Color(0.40f, 0.45f, 0.52f, 1f);
        txt.raycastTarget = false;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop.UI;

internal static class AppHeaderBuilder
{
    /// <summary>
    /// Builds the clean app header:
    /// [<] [🛍 PocketShop] [Spacer] [X]
    /// </summary>
    public static void Build(Transform parent, Action onBack, Action onClose)
    {
        var header = S1API.UI.UIFactory.Panel("AppHeader", parent, new Color(0.06f, 0.07f, 0.10f, 1f));
        var hLE = header.AddComponent<LayoutElement>();
        hLE.minHeight = UITheme.Dp(38f);
        hLE.preferredHeight = UITheme.Dp(38f);
        hLE.flexibleHeight = 0f;

        var hlg = header.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(4f), (int)UITheme.Dp(4f));
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;
        hlg.childAlignment = TextAnchor.MiddleLeft;

        // Back button (left)
        BuildButton(header.transform, "<", new Color(0.18f, 0.21f, 0.28f, 1f), Color.white, 34, onBack);

        // Title Group (icon + PocketShop)
        var titleGroup = S1API.UI.UIFactory.Panel("TitleGroup", header.transform, Color.clear);
        var titleLE = titleGroup.AddComponent<LayoutElement>();
        titleLE.minWidth = UITheme.Dp(160f);
        titleLE.preferredWidth = UITheme.Dp(220f);
        titleLE.flexibleWidth = 1f;

        var titleHlg = titleGroup.AddComponent<HorizontalLayoutGroup>();
        titleHlg.spacing = UITheme.Dp(6f);
        titleHlg.childControlWidth = false;
        titleHlg.childControlHeight = true;
        titleHlg.childForceExpandWidth = false;
        titleHlg.childForceExpandHeight = true;
        titleHlg.childAlignment = TextAnchor.MiddleLeft;

        var iconTxt = S1API.UI.UIFactory.Text("Icon", "🛍", titleGroup.transform, UITheme.Sp(17), TextAnchor.MiddleLeft);
        iconTxt.color = new Color(0.20f, 0.85f, 0.95f, 1f);
        iconTxt.raycastTarget = false;

        var titleTxt = S1API.UI.UIFactory.Text("Title", "PocketShop", titleGroup.transform, UITheme.Sp(16), TextAnchor.MiddleLeft, FontStyle.Bold | FontStyle.Italic);
        titleTxt.color = new Color(0.20f, 0.85f, 0.95f, 1f);
        titleTxt.raycastTarget = false;
        titleTxt.horizontalOverflow = HorizontalWrapMode.Overflow;

        // X close button (coral red)
        BuildButton(header.transform, "X", new Color(0.85f, 0.28f, 0.35f, 1f), Color.white, 32, onClose);
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

        var txt = S1API.UI.UIFactory.Text("Version", "PocketShop v0.2.1", footer.transform, UITheme.Sp(9), TextAnchor.MiddleCenter);
        txt.color = new Color(0.40f, 0.45f, 0.52f, 1f);
        txt.raycastTarget = false;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
    }
}

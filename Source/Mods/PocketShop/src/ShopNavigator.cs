using System;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop.UI;

/// <summary>
/// Sub-header cycle: [<] [ShopName] [>]. Fires <see cref="OnShopChanged"/>
/// with the new shop index on left/right click.
/// </summary>
public class ShopNavigator
{
    private readonly Action _onPrev;
    private readonly Action _onNext;
    private Text _shopNameLabel = null!;
    private GameObject? _navGO;
    private int _currentIndex;
    private int _shopCount;

    public ShopNavigator(Action onPrev, Action onNext)
    {
        _onPrev = onPrev;
        _onNext = onNext;
    }

    public GameObject Build(Transform parent)
    {
        _navGO = UIFactory.Panel("ShopNavigator", parent, Color.clear);
        var le = _navGO.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(28f);
        le.preferredHeight = UITheme.Dp(28f);
        le.flexibleHeight = 0f;

        var hlg = _navGO.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(6f);
        hlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), 0, 0);
        hlg.childControlWidth = false;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;
        hlg.childAlignment = TextAnchor.MiddleLeft;

        AddCycleButton(_navGO.transform, "<", _onPrev, widthDp: 28);
        _shopNameLabel = UIFactory.Text("ShopName", "—", _navGO.transform, UITheme.Sp(13), TextAnchor.MiddleLeft, FontStyle.Bold | FontStyle.Italic);
        _shopNameLabel.color = new Color(0.78f, 0.95f, 0.25f, 1f);
        _shopNameLabel.raycastTarget = false;
        _shopNameLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        var nameLE = _shopNameLabel.gameObject.AddComponent<LayoutElement>();
        nameLE.minWidth = UITheme.Dp(100f);
        nameLE.flexibleWidth = 1f;

        AddCycleButton(_navGO.transform, ">", _onNext, widthDp: 28);

        return _navGO;
    }

    public void SetActive(bool active)
    {
        if (_navGO != null)
        {
            _navGO.SetActive(active);
        }
    }

    public void SetShop(int index, int totalShops, string name)
    {
        _currentIndex = index;
        _shopCount = totalShops;
        if (_shopNameLabel != null)
            _shopNameLabel.text = totalShops > 0 ? $"{name.ToUpperInvariant()} ({index + 1}/{totalShops})" : "NO SHOPS";
    }

    private void AddCycleButton(Transform parent, string label, Action onClick, int widthDp)
    {
        var btnGO = UIFactory.Panel($"Btn_{label}", parent, new Color(0.30f, 0.45f, 0.65f, 1f));
        var btn = btnGO.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        var le = btnGO.AddComponent<LayoutElement>();
        le.preferredWidth = UITheme.Dp(widthDp);
        le.minWidth = UITheme.Dp(widthDp);
        le.preferredHeight = UITheme.Dp(24f);
        var vlg = btnGO.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = true;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        var txt = UIFactory.Text("Lbl", label, btnGO.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        txt.color = Color.white;
        txt.raycastTarget = false;
        ButtonUtils.AddListener(btn, onClick);
    }
}

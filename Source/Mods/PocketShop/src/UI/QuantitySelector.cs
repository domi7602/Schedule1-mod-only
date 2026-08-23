using System;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop.UI;

/// <summary>
/// Min/Max-aware quantity selector: [-] [N] [+].
/// Min is always 1. Max is supplied by the caller (Stock or 99 for unlimited).
/// Raises <see cref="OnChanged"/> when the user clicks +/-.
/// </summary>
public class QuantitySelector
{
    public event Action<int>? OnChanged;

    private readonly int _maxStockOrSentinel;
    private int _quantity;
    private Text _valueLabel = null!;

    public int Quantity => _quantity;
    public int Max => _maxStockOrSentinel;
    public bool IsAtMax => _maxStockOrSentinel > 0 && _quantity >= _maxStockOrSentinel;
    public bool IsAtMin => _quantity <= 1;

    public QuantitySelector(int maxStockOrSentinel, int initial = 1)
    {
        _maxStockOrSentinel = maxStockOrSentinel;
        _quantity = Mathf.Clamp(initial, 1, EffectiveMax());
    }

    public int EffectiveMax()
    {
        if (_maxStockOrSentinel == PurchaseService.UnlimitedStockSentinel) return 99;
        if (_maxStockOrSentinel <= 0) return 1;
        return _maxStockOrSentinel;
    }

    public bool IsStockEmpty => _maxStockOrSentinel == 0;

    public GameObject Build(Transform parent)
    {
        var bar = UIFactory.Panel("QtyBar", parent, new Color(0.10f, 0.12f, 0.16f, 0.90f));
        var hlg = bar.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(4f);
        hlg.padding = new RectOffset((int)UITheme.Dp(4f), (int)UITheme.Dp(4f), (int)UITheme.Dp(2f), (int)UITheme.Dp(2f));
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;
        hlg.childAlignment = TextAnchor.MiddleCenter;

        var le = bar.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(24f);
        le.preferredHeight = UITheme.Dp(24f);

        AddButton(bar.transform, "-", () => ChangeBy(-1));

        _valueLabel = UIFactory.Text("Qty", _quantity.ToString(), bar.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        _valueLabel.color = Color.white;
        _valueLabel.raycastTarget = false;
        _valueLabel.horizontalOverflow = HorizontalWrapMode.Overflow;

        AddButton(bar.transform, "+", () => ChangeBy(+1));

        if (IsStockEmpty)
        {
            SetInteractable(bar, false);
        }

        return bar;
    }

    /// <summary>
    /// Clamps the current quantity to the new max (e.g. after a purchase that
    /// reduced stock). If max dropped to 0, quantity stays at 1 and IsStockEmpty flips true.
    /// </summary>
    public void ClampTo(int newMax)
    {
        if (newMax <= 0)
        {
            _quantity = 1;
        }
        else
        {
            int cap = newMax == PurchaseService.UnlimitedStockSentinel ? 99 : newMax;
            _quantity = Mathf.Clamp(_quantity, 1, cap);
        }
        if (_valueLabel != null) _valueLabel.text = _quantity.ToString();
    }

    public void SetQuantity(int newQty)
    {
        int cap = EffectiveMax();
        int clamped = Mathf.Clamp(newQty, 1, cap);
        if (_quantity == clamped) return;
        _quantity = clamped;
        if (_valueLabel != null) _valueLabel.text = _quantity.ToString();
        OnChanged?.Invoke(_quantity);
    }

    public void RefreshDisplay()
    {
        if (_valueLabel != null) _valueLabel.text = _quantity.ToString();
    }

    public void ChangeBy(int delta)
    {
        int newQty = _quantity + delta;
        if (newQty < 1) newQty = 1;
        int cap = EffectiveMax();
        if (newQty > cap) newQty = cap;
        if (newQty == _quantity) return;
        _quantity = newQty;
        if (_valueLabel != null) _valueLabel.text = _quantity.ToString();
        SoundService.PlayButtonClick();
        OnChanged?.Invoke(_quantity);
    }

    private void AddButton(Transform parent, string label, Action onClick)
    {
        var btnGO = UIFactory.Panel($"Btn_{label}", parent, new Color(0.18f, 0.22f, 0.29f, 1f));
        var btn = btnGO.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        var le = btnGO.AddComponent<LayoutElement>();
        le.preferredWidth = UITheme.Dp(24f);
        le.minWidth = UITheme.Dp(24f);
        le.preferredHeight = UITheme.Dp(20f);
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

    private static void SetInteractable(GameObject go, bool interactable)
    {
        foreach (var b in go.GetComponentsInChildren<Button>(true))
        {
            b.interactable = interactable;
        }
    }
}

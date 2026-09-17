using System;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

/// <summary>
/// Min/Max-aware inline quantity selector: [-] [Editable Number InputField] [+].
/// Exactly like vanilla Dan's Hardware (ShopAmountSelector):
/// Players can click the number directly to type any quantity (e.g. 20 or 40).
/// Typing is protected by PocketShopInputFocus (WASD movement locked while focused).
/// </summary>
public class QuantitySelector
{
    public event Action<int>? OnChanged;

    private int _maxStockOrSentinel;
    private int _quantity;
    private InputField _inputField = null!;
    private GameObject? _bar;

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
        _bar = bar;
        var hlg = bar.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(2f);
        hlg.padding = new RectOffset((int)UITheme.Dp(2f), (int)UITheme.Dp(2f), (int)UITheme.Dp(1f), (int)UITheme.Dp(1f));
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;
        hlg.childAlignment = TextAnchor.MiddleCenter;

        var le = bar.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(22f);
        le.preferredHeight = UITheme.Dp(22f);

        // [-] Button
        AddButton(bar.transform, "-", () => ChangeBy(-1));

        // Center: Inline Editable InputField [ N ]
        var inputGO = UIFactory.Panel("QtyInputBox", bar.transform, new Color(0.14f, 0.17f, 0.23f, 1f));
        var inputLE = inputGO.AddComponent<LayoutElement>();
        inputLE.flexibleWidth = 1f;
        inputLE.minWidth = UITheme.Dp(24f);
        inputLE.minHeight = UITheme.Dp(18f);
        inputLE.preferredHeight = UITheme.Dp(18f);

        var text = UIFactory.Text("Text", _quantity.ToString(), inputGO.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
        var textRt = text.rectTransform;
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(2f, 0f);
        textRt.offsetMax = new Vector2(-2f, 0f);
        text.color = Color.white;
        text.supportRichText = false;
        text.raycastTarget = false;

        _inputField = inputGO.AddComponent<InputField>();
        _inputField.contentType = InputField.ContentType.IntegerNumber;
        _inputField.targetGraphic = inputGO.GetComponent<Image>();
        _inputField.textComponent = text;
        _inputField.caretWidth = 2;
        _inputField.caretColor = Color.white;
        _inputField.selectionColor = new Color(0.24f, 0.82f, 0.44f, 0.45f);
        _inputField.text = _quantity.ToString();

        var focus = inputGO.AddComponent<PocketShopInputFocus>();
        focus.inputField = _inputField;

        EventHelper.AddListener<string>(OnInputEndEdit, _inputField.onEndEdit);
        EventHelper.AddListener<string>(OnInputValueChanged, _inputField.onValueChanged);

        // [+] Button
        AddButton(bar.transform, "+", () => ChangeBy(+1));

        if (IsStockEmpty)
        {
            SetInteractable(false);
        }

        return bar;
    }

    private void OnInputValueChanged(string val)
    {
        if (int.TryParse(val, out int parsed) && parsed > 0)
        {
            int cap = EffectiveMax();
            int clamped = Mathf.Clamp(parsed, 1, cap);
            if (_quantity != clamped)
            {
                _quantity = clamped;
                OnChanged?.Invoke(_quantity);
            }
        }
    }

    private void OnInputEndEdit(string val)
    {
        int cap = EffectiveMax();
        int target = 1;
        if (int.TryParse(val, out int parsed))
        {
            target = Mathf.Clamp(parsed, 1, cap);
        }
        _quantity = target;
        if (_inputField != null)
        {
            _inputField.text = _quantity.ToString();
        }
        OnChanged?.Invoke(_quantity);
    }

    public void ClampTo(int newMax)
    {
        _maxStockOrSentinel = newMax;
        int sentinel = PurchaseService.UnlimitedStockSentinel;
        if (newMax == sentinel)
        {
            _quantity = Mathf.Clamp(_quantity, 1, 99);
        }
        else if (newMax <= 0)
        {
            _quantity = 1;
        }
        else
        {
            _quantity = Mathf.Clamp(_quantity, 1, newMax);
        }
        if (_inputField != null) _inputField.text = _quantity.ToString();
    }

    public void SetQuantity(int newQty)
    {
        int cap = EffectiveMax();
        int clamped = Mathf.Clamp(newQty, 1, cap);
        if (_quantity == clamped) return;
        _quantity = clamped;
        if (_inputField != null) _inputField.text = _quantity.ToString();
        OnChanged?.Invoke(_quantity);
    }

    public void RefreshDisplay()
    {
        if (_inputField != null) _inputField.text = _quantity.ToString();
    }

    public void ChangeBy(int delta)
    {
        int newQty = _quantity + delta;
        if (newQty < 1) newQty = 1;
        int cap = EffectiveMax();
        if (newQty > cap) newQty = cap;
        if (newQty == _quantity) return;
        _quantity = newQty;
        if (_inputField != null) _inputField.text = _quantity.ToString();
        SoundService.PlayButtonClick();
        OnChanged?.Invoke(_quantity);
    }

    public void ChangeBySafe(int delta)
    {
        int newQty = _quantity + delta;
        if (newQty < 1) newQty = 1;
        int cap = EffectiveMax();
        if (newQty > cap)
        {
            newQty = cap > 0 ? cap : 1;
        }
        if (newQty == _quantity) return;
        _quantity = newQty;
        if (_inputField != null) _inputField.text = _quantity.ToString();
        SoundService.PlayButtonClick();
        OnChanged?.Invoke(_quantity);
    }

    private void AddButton(Transform parent, string label, Action onClick)
    {
        var btnGO = UIFactory.Panel($"Btn_{label}", parent, new Color(0.18f, 0.22f, 0.29f, 1f));
        var btn = btnGO.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        var le = btnGO.AddComponent<LayoutElement>();
        le.preferredWidth = UITheme.Dp(22f);
        le.minWidth = UITheme.Dp(22f);
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

    public void SetInteractable(bool interactable)
    {
        if (_bar != null)
        {
            foreach (var b in _bar.GetComponentsInChildren<Button>(true))
            {
                b.interactable = interactable;
            }
            if (_inputField != null)
            {
                _inputField.interactable = interactable;
            }
        }
    }
}

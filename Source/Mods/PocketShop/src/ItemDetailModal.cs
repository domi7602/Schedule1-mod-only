using System;
using PocketShop.Config;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop.UI;

/// <summary>
/// Detailed inspection and purchase modal for a single item.
/// Opens when the player taps an item card or icon.
/// Supports quick bulk jumps (+1, +5, +10, MAX) and direct purchase.
/// </summary>
public class ItemDetailModal
{
    public event Action<PurchaseResultData>? OnPurchaseResult;

    private readonly RectTransform _parent;
    private GameObject? _modalRoot;
    private ItemPOCO? _item;
    private QuantitySelector _qty = null!;
    private Text _priceBreakdownLabel = null!;
    private Text _stockLabel = null!;
    private Button _buyButton = null!;
    private Text _buyButtonLabel = null!;
    private Image _buyButtonImage = null!;

    public bool IsOpen => _modalRoot != null && _modalRoot.activeSelf;

    public ItemDetailModal(RectTransform parent)
    {
        _parent = parent;
    }

    public void Show(ItemPOCO item)
    {
        _item = item;
        Build();
        if (_modalRoot != null)
        {
            _modalRoot.transform.SetAsLastSibling();
            _modalRoot.SetActive(true);
            RefreshDisplay();
        }
    }

    public void Hide()
    {
        if (_modalRoot != null)
        {
            _modalRoot.SetActive(false);
        }
    }

    private void Build()
    {
        if (_modalRoot != null)
        {
            UnityEngine.Object.Destroy(_modalRoot);
        }

        if (_item == null) return;

        // Dark backdrop covering the whole phone screen
        _modalRoot = UIFactory.Panel("PocketShop_DetailModal", _parent, new Color(0.02f, 0.03f, 0.05f, 0.82f), fullAnchor: true);
        var backdropBtn = _modalRoot.AddComponent<Button>();
        backdropBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(backdropBtn, Hide);

        // Centered Card Container
        var card = UIFactory.Panel("ModalCard", _modalRoot.transform, new Color(0.09f, 0.11f, 0.15f, 1f));
        var cardRt = (RectTransform)card.transform;
        cardRt.anchorMin = new Vector2(0.5f, 0.5f);
        cardRt.anchorMax = new Vector2(0.5f, 0.5f);
        cardRt.pivot = new Vector2(0.5f, 0.5f);
        cardRt.sizeDelta = new Vector2(UITheme.Dp(360f), UITheme.Dp(320f));

        // Prevent clicking inside the card from triggering backdrop dismiss
        var cardClickShield = card.GetComponent<Image>() ?? card.AddComponent<Image>();
        cardClickShield.color = new Color(0.09f, 0.11f, 0.15f, 1f);

        var cardVlg = card.AddComponent<VerticalLayoutGroup>();
        cardVlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(10f), (int)UITheme.Dp(10f));
        cardVlg.spacing = UITheme.Dp(8f);
        cardVlg.childControlWidth = true;
        cardVlg.childControlHeight = true;
        cardVlg.childForceExpandWidth = true;
        cardVlg.childForceExpandHeight = false;

        // 1. Header Bar: [Category Badge] [Title] [Spacer] [X]
        var header = UIFactory.Panel("Header", card.transform, Color.clear);
        var hLE = header.AddComponent<LayoutElement>();
        hLE.minHeight = UITheme.Dp(28f);
        hLE.preferredHeight = UITheme.Dp(28f);
        var hHlg = header.AddComponent<HorizontalLayoutGroup>();
        hHlg.spacing = UITheme.Dp(6f);
        hHlg.childControlWidth = false;
        hHlg.childControlHeight = true;
        hHlg.childForceExpandWidth = false;
        hHlg.childForceExpandHeight = true;
        hHlg.childAlignment = TextAnchor.MiddleLeft;

        var titleTxt = UIFactory.Text("Title", _item.Name, header.transform, UITheme.Sp(14), TextAnchor.MiddleLeft, FontStyle.Bold | FontStyle.Italic);
        titleTxt.color = Color.white;
        titleTxt.raycastTarget = false;
        var tLE = titleTxt.gameObject.AddComponent<LayoutElement>();
        tLE.minWidth = UITheme.Dp(180f);
        tLE.flexibleWidth = 1f;

        var closeBtnGO = UIFactory.Panel("CloseBtn", header.transform, new Color(0.85f, 0.28f, 0.35f, 1f));
        var closeBtn = closeBtnGO.AddComponent<Button>();
        closeBtn.transition = Selectable.Transition.None;
        var cbLE = closeBtnGO.AddComponent<LayoutElement>();
        cbLE.preferredWidth = UITheme.Dp(24f);
        cbLE.preferredHeight = UITheme.Dp(24f);
        var cbVlg = closeBtnGO.AddComponent<VerticalLayoutGroup>();
        cbVlg.childControlWidth = true;
        cbVlg.childControlHeight = true;
        cbVlg.childForceExpandWidth = true;
        cbVlg.childForceExpandHeight = true;
        cbVlg.childAlignment = TextAnchor.MiddleCenter;
        var cbTxt = UIFactory.Text("X", "X", closeBtnGO.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
        cbTxt.color = Color.white;
        cbTxt.raycastTarget = false;
        ButtonUtils.AddListener(closeBtn, Hide);

        // 2. Middle Info Row: [Large Icon Box] + [Details Column]
        var middleRow = UIFactory.Panel("MiddleRow", card.transform, Color.clear);
        var mLE = middleRow.AddComponent<LayoutElement>();
        mLE.minHeight = UITheme.Dp(100f);
        mLE.preferredHeight = UITheme.Dp(100f);
        var mHlg = middleRow.AddComponent<HorizontalLayoutGroup>();
        mHlg.spacing = UITheme.Dp(12f);
        mHlg.childControlWidth = true;
        mHlg.childControlHeight = true;
        mHlg.childForceExpandWidth = false;
        mHlg.childForceExpandHeight = true;

        // Icon Box
        var iconBox = UIFactory.Panel("IconBox", middleRow.transform, new Color(0.05f, 0.06f, 0.08f, 1f));
        var ibLE = iconBox.AddComponent<LayoutElement>();
        ibLE.preferredWidth = UITheme.Dp(90f);
        ibLE.preferredHeight = UITheme.Dp(90f);
        if (_item.Icon != null)
        {
            var imgGO = new GameObject("IconImg");
            imgGO.transform.SetParent(iconBox.transform, false);
            var irt = imgGO.AddComponent<RectTransform>();
            irt.anchorMin = Vector2.zero;
            irt.anchorMax = Vector2.one;
            irt.offsetMin = new Vector2(UITheme.Dp(4f), UITheme.Dp(4f));
            irt.offsetMax = new Vector2(-UITheme.Dp(4f), -UITheme.Dp(4f));
            var img = imgGO.AddComponent<Image>();
            img.sprite = _item.Icon;
            img.preserveAspect = true;
            img.raycastTarget = false;
        }

        // Details Column
        var detailsCol = UIFactory.Panel("DetailsCol", middleRow.transform, Color.clear);
        var dcLE = detailsCol.AddComponent<LayoutElement>();
        dcLE.flexibleWidth = 1f;
        var dcVlg = detailsCol.AddComponent<VerticalLayoutGroup>();
        dcVlg.spacing = UITheme.Dp(4f);
        dcVlg.childControlWidth = true;
        dcVlg.childControlHeight = true;
        dcVlg.childForceExpandWidth = true;
        dcVlg.childForceExpandHeight = false;
        dcVlg.childAlignment = TextAnchor.UpperLeft;

        var vendorLabel = UIFactory.Text("Vendor", $"🏪  {_item.ShopName}", detailsCol.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        vendorLabel.color = new Color(0.70f, 0.85f, 0.95f, 1f);
        vendorLabel.raycastTarget = false;

        _stockLabel = UIFactory.Text("Stock", $"📦  Stock: {StockText(_item)}", detailsCol.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _stockLabel.color = new Color(0.30f, 0.85f, 0.95f, 1f);
        _stockLabel.raycastTarget = false;

        _priceBreakdownLabel = UIFactory.Text("Breakdown", "", detailsCol.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _priceBreakdownLabel.color = new Color(0.96f, 0.77f, 0.26f, 1f);
        _priceBreakdownLabel.raycastTarget = false;

        // 3. Quick Jump Chips: [+1] [+5] [+10] [MAX]
        var quickRow = UIFactory.Panel("QuickRow", card.transform, Color.clear);
        var qLE = quickRow.AddComponent<LayoutElement>();
        qLE.minHeight = UITheme.Dp(26f);
        qLE.preferredHeight = UITheme.Dp(26f);
        var qHlg = quickRow.AddComponent<HorizontalLayoutGroup>();
        qHlg.spacing = UITheme.Dp(6f);
        qHlg.childControlWidth = true;
        qHlg.childControlHeight = true;
        qHlg.childForceExpandWidth = true;
        qHlg.childForceExpandHeight = true;

        AddQuickChip(quickRow.transform, "1", () => _qty.SetQuantity(1));
        AddQuickChip(quickRow.transform, "+5", () => _qty.ChangeBy(5));
        AddQuickChip(quickRow.transform, "+10", () => _qty.ChangeBy(10));
        AddQuickChip(quickRow.transform, "MAX", () => _qty.SetQuantity(_qty.EffectiveMax()));

        // 4. Fine Quantity Selector: [-] [N] [+]
        int maxStock = ResolveMaxStock(_item);
        _qty = new QuantitySelector(maxStock, 1);
        _qty.Build(card.transform);
        _qty.OnChanged += _ => RefreshDisplay();

        // 5. Buy Button (Full Width)
        var buyPanel = UIFactory.Panel("ModalBuyBtn", card.transform, new Color(0.24f, 0.82f, 0.44f, 1f));
        _buyButtonImage = buyPanel.GetComponent<Image>() ?? buyPanel.AddComponent<Image>();
        var bLE = buyPanel.AddComponent<LayoutElement>();
        bLE.minHeight = UITheme.Dp(36f);
        bLE.preferredHeight = UITheme.Dp(36f);
        var bVlg = buyPanel.AddComponent<VerticalLayoutGroup>();
        bVlg.childControlWidth = true;
        bVlg.childControlHeight = true;
        bVlg.childForceExpandWidth = true;
        bVlg.childForceExpandHeight = true;
        bVlg.childAlignment = TextAnchor.MiddleCenter;

        _buyButtonLabel = UIFactory.Text("BuyLbl", "BUY NOW", buyPanel.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        _buyButtonLabel.color = new Color(0.03f, 0.10f, 0.05f, 1f);
        _buyButtonLabel.raycastTarget = false;

        _buyButton = buyPanel.AddComponent<Button>();
        _buyButton.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(_buyButton, OnBuyClicked);
    }

    private void AddQuickChip(Transform parent, string label, Action onClick)
    {
        var chip = UIFactory.Panel($"Chip_{label}", parent, new Color(0.16f, 0.20f, 0.28f, 1f));
        var btn = chip.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        var le = chip.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(24f);
        le.flexibleWidth = 1f;
        var vlg = chip.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = true;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        var txt = UIFactory.Text("Lbl", label, chip.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
        txt.color = Color.white;
        txt.raycastTarget = false;
        ButtonUtils.AddListener(btn, () =>
        {
            SoundService.PlayButtonClick();
            onClick();
        });
    }

    public void RefreshDisplay()
    {
        if (_item == null || _modalRoot == null || !_modalRoot.activeSelf) return;

        PurchaseService.CalculatePricing(_item, _qty.Quantity, out float perUnit, out float subtotal, out float deliveryFee, out float total);

        float feePercent = PocketShopConfig.ServiceFeePercentStatic;
        if (_priceBreakdownLabel != null)
        {
            _priceBreakdownLabel.text = $"💵  Base ${_item.Price:F0} (+{feePercent:F0}% fee → ${perUnit:F0}/ea)\n" +
                                        $"💳  Total: ${total:F0} for {_qty.Quantity}x";
        }

        if (_stockLabel != null)
        {
            _stockLabel.text = $"📦  Stock: {StockText(_item)}";
        }

        var activeMode = PocketShopConfig.PaymentModeStatic;
        bool canAfford = PurchaseService.CanAfford(total, activeMode, out PaymentMode effective);

        if (_qty.IsStockEmpty)
        {
            _buyButton.interactable = false;
            _buyButtonImage.color = new Color(0.12f, 0.15f, 0.20f, 1f);
            _buyButtonLabel.text = "OUT OF STOCK";
            _buyButtonLabel.color = new Color(0.55f, 0.58f, 0.65f, 1f);
        }
        else if (!canAfford)
        {
            _buyButton.interactable = false;
            _buyButtonImage.color = new Color(0.18f, 0.14f, 0.16f, 1f);
            _buyButtonLabel.text = $"INSUFFICIENT FUNDS (${total:F0})";
            _buyButtonLabel.color = new Color(0.90f, 0.45f, 0.45f, 1f);
        }
        else
        {
            _buyButton.interactable = true;
            _buyButtonImage.color = new Color(0.24f, 0.82f, 0.44f, 1f);
            string payLabel = effective == PaymentMode.Cash ? "CASH" : "BANK";
            _buyButtonLabel.text = $"BUY {_qty.Quantity}x · ${total:F0} ({payLabel})";
            _buyButtonLabel.color = new Color(0.03f, 0.10f, 0.05f, 1f);
        }
    }

    private void OnBuyClicked()
    {
        if (_item == null) return;
        var result = PurchaseService.BuyWithQuantity(_item, _qty.Quantity, PocketShopConfig.PaymentModeStatic);
        OnPurchaseResult?.Invoke(result);

        if (result.IsSuccess)
        {
            _qty.ClampTo(ResolveMaxStock(_item));
            RefreshDisplay();
        }
    }

    private static int ResolveMaxStock(ItemPOCO item)
    {
        if (!item.IsInStock) return 0;
        return item.CurrentStock > 0 ? item.CurrentStock : PurchaseService.UnlimitedStockSentinel;
    }

    private static string StockText(ItemPOCO item)
    {
        if (!item.IsInStock) return "OUT OF STOCK";
        if (item.CurrentStock <= 0) return "Unlimited (∞)";
        return $"{item.CurrentStock} units";
    }

    public void Dispose()
    {
        if (_modalRoot != null)
        {
            UnityEngine.Object.Destroy(_modalRoot);
            _modalRoot = null;
        }
    }
}

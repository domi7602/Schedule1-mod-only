using System;
using PocketShop.Config;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop.UI;

/// <summary>
/// Single item card: Price (top-left) + Stock badge (top-right) + Image (clickable) +
/// Name (clickable) + QuantitySelector + BUY button.
/// Supports multi-payment methods (Cash, Bank, Auto) and opens ItemDetailModal on tap.
/// </summary>
public class ItemCard
{
    public event Action<PurchaseResultData>? OnPurchaseResult;
    public event Action<ItemPOCO>? OnInspectRequested;

    private readonly ItemPOCO _item;
    private QuantitySelector _qty = null!;
    private Button _buyButton = null!;
    private Text _buyLabel = null!;
    private Image _buyPanelImage = null!;
    private Text _priceLabel = null!;
    private GameObject _stockBadge = null!;
    private GameObject _card = null!;

    public ItemCard(ItemPOCO item) { _item = item; }

    public GameObject Build(Transform parent)
    {
        string itemId = _item.Definition != null ? _item.Definition.ID : _item.Name;
        _card = UIFactory.Panel($"Item_{itemId}", parent, new Color(0.09f, 0.10f, 0.14f, 1f));
        var le = _card.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(175f);
        le.preferredHeight = UITheme.Dp(175f);
        le.flexibleWidth = 1f;

        var vlg = _card.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(4f), (int)UITheme.Dp(4f), (int)UITheme.Dp(4f), (int)UITheme.Dp(4f));
        vlg.spacing = UITheme.Dp(3f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childAlignment = TextAnchor.UpperCenter;

        // Top row: Price (left) + Stock badge (right)
        var topRow = UIFactory.Panel("TopRow", _card.transform, Color.clear);
        var topRowLE = topRow.AddComponent<LayoutElement>();
        topRowLE.minHeight = UITheme.Dp(16f);
        topRowLE.preferredHeight = UITheme.Dp(16f);
        var topHlg = topRow.AddComponent<HorizontalLayoutGroup>();
        topHlg.spacing = UITheme.Dp(2f);
        topHlg.childControlWidth = true;
        topHlg.childControlHeight = true;
        topHlg.childForceExpandWidth = true;
        topHlg.childForceExpandHeight = true;
        topHlg.childAlignment = TextAnchor.MiddleLeft;

        _priceLabel = UIFactory.Text("Price", $"${_item.Price:F0}", topRow.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        _priceLabel.color = new Color(0.96f, 0.77f, 0.26f, 1f);
        _priceLabel.raycastTarget = false;

        var priceSpacer = UIFactory.Panel("Spacer", topRow.transform, Color.clear);
        priceSpacer.AddComponent<LayoutElement>().flexibleWidth = 1f;

        _stockBadge = UIFactory.Panel("StockBadge", topRow.transform, new Color(0.12f, 0.16f, 0.22f, 0.90f));
        var badgeLE = _stockBadge.AddComponent<LayoutElement>();
        badgeLE.preferredWidth = UITheme.Dp(32f);
        badgeLE.preferredHeight = UITheme.Dp(14f);
        var badgeVlg = _stockBadge.AddComponent<VerticalLayoutGroup>();
        badgeVlg.childControlWidth = true;
        badgeVlg.childControlHeight = true;
        badgeVlg.childForceExpandWidth = true;
        badgeVlg.childForceExpandHeight = true;
        badgeVlg.childAlignment = TextAnchor.MiddleCenter;
        var badgeTxt = UIFactory.Text("BadgeLbl", StockText(_item), _stockBadge.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        badgeTxt.color = new Color(0.30f, 0.85f, 0.95f, 1f);
        badgeTxt.raycastTarget = false;

        // Image area (center, clickable for inspect)
        var imgPanel = UIFactory.Panel("Image", _card.transform, new Color(0.05f, 0.06f, 0.08f, 1f));
        var imgLE = imgPanel.AddComponent<LayoutElement>();
        imgLE.flexibleHeight = 1f;
        imgLE.minHeight = UITheme.Dp(52f);
        imgLE.preferredHeight = UITheme.Dp(55f);

        var imgBtn = imgPanel.AddComponent<Button>();
        imgBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(imgBtn, OnInspectClicked);

        if (_item.Icon != null)
        {
            var imgGO = new GameObject("Icon");
            imgGO.transform.SetParent(imgPanel.transform, false);
            var imgRt = imgGO.AddComponent<RectTransform>();
            imgRt.anchorMin = Vector2.zero;
            imgRt.anchorMax = Vector2.one;
            imgRt.offsetMin = new Vector2(UITheme.Dp(3f), UITheme.Dp(3f));
            imgRt.offsetMax = new Vector2(-UITheme.Dp(3f), -UITheme.Dp(3f));
            var img = imgGO.AddComponent<Image>();
            img.sprite = _item.Icon;
            img.preserveAspect = true;
            img.raycastTarget = false;
        }

        // Name (clickable for inspect)
        var nameBtnGO = UIFactory.Panel("NameBtn", _card.transform, Color.clear);
        var nameLE = nameBtnGO.AddComponent<LayoutElement>();
        nameLE.minHeight = UITheme.Dp(16f);
        nameLE.preferredHeight = UITheme.Dp(16f);
        var nameVlg = nameBtnGO.AddComponent<VerticalLayoutGroup>();
        nameVlg.childControlWidth = true;
        nameVlg.childControlHeight = true;
        nameVlg.childForceExpandWidth = true;
        nameVlg.childForceExpandHeight = true;
        nameVlg.childAlignment = TextAnchor.MiddleCenter;

        var name = UIFactory.Text("Name", _item.Name, nameBtnGO.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold | FontStyle.Italic);
        name.color = Color.white;
        name.raycastTarget = false;
        name.horizontalOverflow = HorizontalWrapMode.Wrap;

        var nameBtn = nameBtnGO.AddComponent<Button>();
        nameBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(nameBtn, OnInspectClicked);

        // Quantity selector
        int maxStock = ResolveMaxStock(_item);
        _qty = new QuantitySelector(maxStock, 1);
        _qty.Build(_card.transform);
        _qty.OnChanged += _ => RefreshBuyState();

        // Buy button
        var buyPanel = UIFactory.Panel("Buy", _card.transform, new Color(0.24f, 0.82f, 0.44f, 1f));
        _buyPanelImage = buyPanel.GetComponent<Image>() ?? buyPanel.AddComponent<Image>();
        var buyLE = buyPanel.AddComponent<LayoutElement>();
        buyLE.minHeight = UITheme.Dp(24f);
        buyLE.preferredHeight = UITheme.Dp(24f);
        var buyVlg = buyPanel.AddComponent<VerticalLayoutGroup>();
        buyVlg.childControlWidth = true;
        buyVlg.childControlHeight = true;
        buyVlg.childForceExpandWidth = true;
        buyVlg.childForceExpandHeight = true;
        buyVlg.childAlignment = TextAnchor.MiddleCenter;
        _buyLabel = UIFactory.Text("BuyLbl", "BUY", buyPanel.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        _buyLabel.color = new Color(0.03f, 0.10f, 0.05f, 1f);
        _buyLabel.raycastTarget = false;
        _buyButton = buyPanel.AddComponent<Button>();
        _buyButton.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(_buyButton, OnBuyClicked);

        RefreshBuyState();
        return _card;
    }

    /// <summary>
    /// Refreshes BUY-button enabled state and label. Supports Cash, Bank, and Auto modes.
    /// </summary>
    public void RefreshBuyState()
    {
        if (_qty.IsStockEmpty)
        {
            _buyButton.interactable = false;
            _buyPanelImage.color = new Color(0.12f, 0.15f, 0.20f, 1f);
            _buyLabel.text = "OUT";
            _buyLabel.color = new Color(0.55f, 0.58f, 0.65f, 1f);
            return;
        }

        PurchaseService.CalculatePricing(_item, _qty.Quantity, out _, out _, out _, out float total);
        var mode = PocketShopConfig.PaymentModeStatic;
        bool canAfford = PurchaseService.CanAfford(total, mode, out PaymentMode effective);

        if (!canAfford)
        {
            _buyButton.interactable = false;
            _buyPanelImage.color = new Color(0.14f, 0.18f, 0.24f, 1f);
            _buyLabel.text = $"${total:F0}";
            _buyLabel.color = new Color(0.80f, 0.82f, 0.88f, 1f);
        }
        else
        {
            _buyButton.interactable = true;
            _buyPanelImage.color = new Color(0.24f, 0.82f, 0.44f, 1f);
            string payBadge = effective == PaymentMode.Bank ? " [CARD]" : "";
            _buyLabel.text = $"BUY{payBadge}";
            _buyLabel.color = new Color(0.03f, 0.10f, 0.05f, 1f);
        }
    }

    /// <summary>
    /// After a successful purchase, refresh the stock badge and clamp quantity.
    /// </summary>
    public void NotifyStockChanged()
    {
        if (_stockBadge == null) return;
        var badgeTxt = _stockBadge.GetComponentInChildren<Text>();
        if (badgeTxt != null) badgeTxt.text = StockText(_item);

        _qty.ClampTo(ResolveMaxStock(_item));
        RefreshBuyState();
    }

    private static int ResolveMaxStock(ItemPOCO item)
    {
        if (!item.IsInStock) return 0;
        return item.CurrentStock > 0 ? item.CurrentStock : PurchaseService.UnlimitedStockSentinel;
    }

    private static string StockText(ItemPOCO item)
    {
        if (!item.IsInStock) return "OUT";
        if (item.CurrentStock <= 0) return "∞";
        return item.CurrentStock.ToString();
    }

    private void OnInspectClicked()
    {
        SoundService.PlayButtonClick();
        OnInspectRequested?.Invoke(_item);
    }

    private void OnBuyClicked()
    {
        var result = PurchaseService.BuyWithQuantity(_item, _qty.Quantity, PocketShopConfig.PaymentModeStatic);
        OnPurchaseResult?.Invoke(result);

        if (result.IsSuccess)
        {
            NotifyStockChanged();
        }
    }
}

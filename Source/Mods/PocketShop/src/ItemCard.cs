using System;
using System.Globalization;
using PocketShop.Config;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

/// <summary>
/// Single item card: Price (top-left) + Stock badge (top-right) + Image +
/// Name + QuantitySelector + BUY button.
/// Payment follows the shop's vanilla payment type (v0.3.1: black market = cash,
/// clean shops = card), resolved by PurchaseService.
/// </summary>
public class ItemCard
{
    public event Action<PurchaseResultData>? OnPurchaseResult;

    private readonly ItemPOCO _item;
    private QuantitySelector _qty = null!;
    private Button _buyButton = null!;
    private Text _buyLabel = null!;
    private Image _buyPanelImage = null!;
    private Text _priceLabel = null!;
    private GameObject _stockBadge = null!;
    private GameObject _card = null!;
    private float _displayedTotal;
    private PaymentMode _displayedMode;
    private bool _hasDisplayedConfirmation;
    private bool _isBuying;

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
        var badgeTxt = UIFactory.Text("BadgeLbl", StockText(_item, _item.Stock), _stockBadge.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        badgeTxt.color = new Color(0.30f, 0.85f, 0.95f, 1f);
        badgeTxt.raycastTarget = false;

        // Image area (center, visual only)
        var imgPanel = UIFactory.Panel("Image", _card.transform, new Color(0.05f, 0.06f, 0.08f, 1f));
        var imgLE = imgPanel.AddComponent<LayoutElement>();
        imgLE.flexibleHeight = 1f;
        imgLE.minHeight = UITheme.Dp(52f);
        imgLE.preferredHeight = UITheme.Dp(55f);

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

        // Name (visual only)
        var nameTxt = UIFactory.Text("Name", _item.Name, _card.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold | FontStyle.Italic);
        nameTxt.color = Color.white;
        nameTxt.raycastTarget = false;
        nameTxt.horizontalOverflow = HorizontalWrapMode.Wrap;
        var nameLE = nameTxt.gameObject.AddComponent<LayoutElement>();
        nameLE.minHeight = UITheme.Dp(16f);
        nameLE.preferredHeight = UITheme.Dp(16f);

        // Quantity selector
        _qty = new QuantitySelector(_item.Stock, 1);
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
    /// Refreshes BUY-button enabled state and label. Payment follows the shop's vanilla
    /// payment type (v0.3.1). Also checks whether the item is locked due to player level requirements.
    /// </summary>
    public void RefreshBuyState()
    {
        if (_buyButton == null || _buyLabel == null || _buyPanelImage == null || _qty == null) return;
        _hasDisplayedConfirmation = false;

        if (_isBuying)
        {
            SetUnavailable("BUYING", new Color(0.14f, 0.18f, 0.24f, 1f));
            return;
        }

        if (!ShopCatalog.TryResolveLiveOffer(_item, out var offer, out string liveFailure))
        {
            _qty.SetStockState(new StockState(StockKind.Unknown, 0, 0));
            UpdateStockBadge(new StockState(StockKind.Unknown, 0, 0));
            _priceLabel.text = "$—";
            SetUnavailable("UNAVAILABLE", new Color(0.14f, 0.18f, 0.24f, 1f));
            if (!string.IsNullOrEmpty(liveFailure)) MelonLogger.Warning($"[PocketShop] {liveFailure}");
            return;
        }

        _qty.SetStockState(offer.Stock);
        UpdateStockBadge(offer.Stock);
        _priceLabel.text = float.IsFinite(offer.Price) && offer.Price >= 0f
            ? $"${PurchaseService.FormatMoney(offer.Price)}"
            : "$—";

        if (offer.Stock.Kind == StockKind.Unknown || offer.Stock.Kind == StockKind.NotOffered)
        {
            SetUnavailable("VERIFY", new Color(0.14f, 0.18f, 0.24f, 1f));
            _qty.SetInteractable(false);
            return;
        }
        if (offer.Stock.Kind == StockKind.LimitedEmpty)
        {
            SetUnavailable("OUT", new Color(0.12f, 0.15f, 0.20f, 1f));
            _qty.SetInteractable(false);
            return;
        }

        if (!offer.Gate.IsKnown)
        {
            SetUnavailable("VERIFY SHOP", new Color(0.14f, 0.18f, 0.24f, 1f));
            _qty.SetInteractable(false);
            return;
        }
        if (offer.Gate.IsLocked)
        {
            SetUnavailable("SHOP LOCKED", new Color(0.14f, 0.18f, 0.24f, 1f));
            _qty.SetInteractable(false);
            return;
        }
        if (!offer.Gate.IsOpen)
        {
            SetUnavailable("CLOSED", new Color(0.14f, 0.18f, 0.24f, 1f));
            _qty.SetInteractable(false);
            return;
        }

        if (!PurchaseService.IsLevelRequirementSatisfied(offer.Definition, out _))
        {
            SetUnavailable("LOCKED", new Color(0.18f, 0.14f, 0.16f, 1f));
            _buyLabel.color = new Color(0.90f, 0.45f, 0.45f, 1f);
            _qty.SetInteractable(false);
            return;
        }

        if (PurchaseService.HasUncertainTransaction)
        {
            SetUnavailable("VERIFY TX", new Color(0.14f, 0.18f, 0.24f, 1f));
            _qty.SetInteractable(false);
            return;
        }
        if (!PurchaseService.IsSingleplayerPurchaseContext)
        {
            SetUnavailable("MP UNAVAILABLE", new Color(0.14f, 0.18f, 0.24f, 1f));
            _qty.SetInteractable(false);
            return;
        }

        if (!PurchaseService.TryCalculatePricing(offer.Price, _qty.Quantity, out var pricing))
        {
            SetUnavailable("PRICE ERROR", new Color(0.14f, 0.18f, 0.24f, 1f));
            return;
        }

        if (!PurchaseService.IsSupportedPaymentType(offer.PaymentType))
        {
            SetUnavailable("PAYMENT?", new Color(0.14f, 0.18f, 0.24f, 1f));
            _qty.SetInteractable(false);
            return;
        }

        bool canAfford = PurchaseService.CanAfford(pricing.Total, offer.PaymentType, out PaymentMode effectiveMode);
        if (!canAfford)
        {
            SetUnavailable($"NEED ${PurchaseService.FormatMoney(pricing.Total)}", new Color(0.14f, 0.18f, 0.24f, 1f));
            return;
        }

        _qty.SetInteractable(true);
        _displayedTotal = pricing.Total;
        _displayedMode = effectiveMode;
        _hasDisplayedConfirmation = true;
        _buyButton.interactable = true;
        _buyPanelImage.color = new Color(0.24f, 0.82f, 0.44f, 1f);
        string payBadge = effectiveMode == PaymentMode.Cash ? " CASH" : " CARD";
        _buyLabel.text = $"BUY ${PurchaseService.FormatMoney(pricing.Total)}{payBadge}";
        _buyLabel.color = new Color(0.03f, 0.10f, 0.05f, 1f);
    }

    private void SetUnavailable(string label, Color color)
    {
        _buyButton.interactable = false;
        _buyPanelImage.color = color;
        _buyLabel.text = label;
        _buyLabel.color = new Color(0.80f, 0.82f, 0.88f, 1f);
    }

    private void UpdateStockBadge(StockState stock)
    {
        if (_stockBadge == null) return;
        var label = _stockBadge.GetComponentInChildren<Text>();
        if (label != null) label.text = StockText(_item, stock);
    }

    /// <summary>
    /// After a successful purchase, refresh the stock badge and clamp quantity.
    /// </summary>
    public void NotifyStockChanged()
    {
        RefreshBuyState();
    }

    /// <summary>Stable item identifier used by the grid's stock-change dispatcher.</summary>
    public string GetItemIdPublic() => _item?.ItemId ?? string.Empty;

    private static string StockText(ItemPOCO item, StockState stock)
    {
        if (!item.IsAvailableToPlayer) return "LOCKED";
        return stock.Kind switch
        {
            StockKind.Unlimited => "∞",
            StockKind.LimitedAvailable => stock.Quantity.ToString(CultureInfo.InvariantCulture),
            StockKind.LimitedEmpty => "OUT",
            StockKind.NotOffered => "—",
            _ => "?"
        };
    }

    private void OnBuyClicked()
    {
        if (_isBuying || !_hasDisplayedConfirmation) return;
        float confirmedTotal = _displayedTotal;
        PaymentMode confirmedMode = _displayedMode;
        _isBuying = true;
        _buyButton.interactable = false;

        try
        {
            var result = PurchaseService.BuyWithQuantity(_item, _qty.Quantity, confirmedTotal, confirmedMode);
            try { OnPurchaseResult?.Invoke(result); }
            catch (Exception ex) { MelonLogger.Error($"[PocketShop] Purchase result handler failed: {ex.Message}"); }
        }
        finally
        {
            _isBuying = false;
            RefreshBuyState();
        }
    }
}

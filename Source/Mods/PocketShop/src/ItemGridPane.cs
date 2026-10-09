using System;
using System.Collections.Generic;
using PocketShop.Services;
using S1API.UI;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

/// <summary>
/// 5-column grid of ItemCards for the currently active shop. The last row is
/// padded with empty cells. Scrolls when total item count exceeds the viewport.
/// </summary>
public class ItemGridPane
{
    public event Action<PurchaseResultData>? OnPurchaseResult;

    private readonly RectTransform _root;
    private readonly List<ItemCard> _cards = new();
    private GameObject _scrollGO = null!;
    private RectTransform _content = null!;
    private Text _statusText = null!;
    private bool _disposed;
    private const float StatusBarHeight = 22f;
    private static readonly Color StatusOk = new(0.24f, 0.82f, 0.44f, 1f);
    private static readonly Color StatusError = new(0.90f, 0.45f, 0.45f, 1f);

    public ItemGridPane(RectTransform root) { _root = root; Build(); }

    private void Build()
    {
        _scrollGO = new GameObject("Scroll");
        _scrollGO.transform.SetParent(_root, false);
        var sRt = _scrollGO.AddComponent<RectTransform>();
        sRt.anchorMin = Vector2.zero;
        sRt.anchorMax = Vector2.one;
        sRt.offsetMin = new Vector2(0f, StatusBarHeight);
        sRt.offsetMax = Vector2.zero;

        _statusText = S1API.UI.UIFactory.Text("PurchaseStatus", string.Empty, _root, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        var stRt = _statusText.rectTransform;
        stRt.anchorMin = Vector2.zero;
        stRt.anchorMax = new Vector2(1f, 0f);
        stRt.offsetMin = Vector2.zero;
        stRt.offsetMax = new Vector2(0f, StatusBarHeight);
        _statusText.raycastTarget = false;

        var scrollRect = _scrollGO.AddComponent<ScrollRect>();
        scrollRect.scrollSensitivity = 25f;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        var viewportGO = new GameObject("Viewport");
        viewportGO.transform.SetParent(_scrollGO.transform, false);
        var vrt = viewportGO.AddComponent<RectTransform>();
        vrt.anchorMin = Vector2.zero;
        vrt.anchorMax = Vector2.one;
        vrt.offsetMin = Vector2.zero;
        vrt.offsetMax = Vector2.zero;
        var vimg = viewportGO.AddComponent<Image>();
        vimg.color = new Color(0, 0, 0, 0);
        viewportGO.AddComponent<RectMask2D>();
        scrollRect.viewport = vrt;

        var contentGO = new GameObject("Content");
        contentGO.transform.SetParent(viewportGO.transform, false);
        _content = contentGO.AddComponent<RectTransform>();
        _content.anchorMin = new Vector2(0, 1);
        _content.anchorMax = new Vector2(1, 1);
        _content.pivot = new Vector2(0.5f, 1f);
        _content.offsetMin = Vector2.zero;
        _content.offsetMax = Vector2.zero;

        var vlg = contentGO.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(8f);
        vlg.padding = new RectOffset((int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;

        var csf = contentGO.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        scrollRect.content = _content;

        // Defensively unsubscribe first: Build() can run again for a new scene while
        // the old handler is still in the static invocation list (leaked subscription
        // would call RefreshActiveShop on destroyed _content after every Refresh).
        ShopCatalog.OnCatalogChanged -= OnCatalogChangedHandler;
        ShopCatalog.OnCatalogChanged += OnCatalogChangedHandler;
    }

    public void SetActive(bool active)
    {
        if (_disposed || _scrollGO == null) return;
        try
        {
            if (_scrollGO.Pointer != IntPtr.Zero && !_scrollGO.WasCollected) _scrollGO.SetActive(active);
        }
        catch { }
    }

    /// <summary>
    /// Bug-Audit 2026-09-13 (Round 5): callback to refresh the directory pane
    /// when the catalog changes (so the store count badge updates).
    /// </summary>
    public Action? OnCatalogChanged { get; set; }

    private void OnCatalogChangedHandler()
    {
        if (_disposed) return;
        RefreshActiveShop();
        // Bug-Audit 2026-09-13 (Round 5): also refresh the store count badge
        // so the directory pane stays in sync with live catalog changes.
        try { OnCatalogChanged?.Invoke(); } catch { }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        ShopCatalog.OnCatalogChanged -= OnCatalogChangedHandler;
        OnCatalogChanged = null;
        OnPurchaseResult = null;
        _cards.Clear();
        DestroyOwnedObject(_scrollGO);
        if (_statusText != null) DestroyOwnedObject(_statusText.gameObject);
    }

    private static void DestroyOwnedObject(GameObject? target)
    {
        if (target == null) return;
        try
        {
            if (target.Pointer != IntPtr.Zero && !target.WasCollected)
                UnityEngine.Object.Destroy(target);
        }
        catch { }
    }

    public void RefreshActiveShop()
    {
        if (_disposed || _content == null) return;
        try
        {
            if (_content.Pointer == IntPtr.Zero || _content.WasCollected) return;
        }
        catch { return; }
        Clear();
        var currentShopCode = ActiveShopCode;
        if (string.IsNullOrEmpty(currentShopCode)) return;
        var items = ShopCatalog.GetItemsOfShop(currentShopCode);
        if (!string.IsNullOrEmpty(Filter))
        {
            string needle = Filter.Trim().ToLowerInvariant();
            items = items.FindAll(x => x.Name != null && x.Name.ToLowerInvariant().Contains(needle));
        }
        if (items.Count == 0)
        {
            ShowEmptyState(string.IsNullOrEmpty(Filter) ? "No items in stock." : "No matching items.");
            return;
        }
        SortForDisplay(items);
        BuildGrid(items);
    }

    /// <summary>
    /// Vanilla-like display order: purchasable (unlocked) items first, then ascending by the
    /// required rank, then by price and name. Vanilla sorts its shop lists; the raw listing
    /// order looked arbitrary.
    /// </summary>
    private static void SortForDisplay(List<ItemPOCO> items)
    {
        items.Sort((a, b) =>
        {
            int ua = a.IsAvailableToPlayer ? 0 : 1;
            int ub = b.IsAvailableToPlayer ? 0 : 1;
            if (ua != ub) return ua - ub;

            int ra = a.RequiredRankValue;
            int rb = b.RequiredRankValue;
            if (ra != rb) return ra - rb;

            int byPrice = a.Price.CompareTo(b.Price);
            if (byPrice != 0) return byPrice;

            return string.CompareOrdinal(a.Name ?? string.Empty, b.Name ?? string.Empty);
        });
    }

    /// <summary>Currently displayed shop code (set externally by PocketShopApp).</summary>
    public string ActiveShopCode
    {
        get => _activeShopCode;
        set
        {
            _activeShopCode = value;
            ShowStatus(string.Empty, true);
        }
    }

    private string _activeShopCode = string.Empty;

    /// <summary>Shows the outcome of the last purchase below the grid (empty text clears it).</summary>
    public void ShowStatus(string message, bool success)
    {
        if (_statusText == null) return;
        _statusText.text = message ?? string.Empty;
        _statusText.color = success ? StatusOk : StatusError;
    }

    /// <summary>Live search filter (case-insensitive substring on the item name). Empty = no filter.</summary>
    public string Filter { get; set; } = string.Empty;

    /// <summary>Refresh every card's BUY enabled-state (called on Cash/Bank change or payment toggle).</summary>
    public void RefreshAllBuyStates()
    {
        foreach (var card in _cards) card.RefreshBuyState();
    }

    /// <summary>
    /// Bug-Audit 2026-09-12: after a purchase (card or status line) the visible grid card
    /// kept showing the old stock badge and an over-sized MAX chip — HandlePurchaseResult
    /// only re-ran RefreshBuyState. Forward the stock-changed event so each affected card
    /// can re-derive its badge + clamp the QuantitySelector to the new max.
    /// </summary>
    public void NotifyStockChanged(string itemId)
    {
        if (string.IsNullOrEmpty(itemId)) return;
        foreach (var card in _cards)
        {
            if (card == null) continue;
            try
            {
                var cardItem = card.GetItemIdPublic();
                if (!string.IsNullOrEmpty(cardItem) && string.Equals(cardItem, itemId, StringComparison.OrdinalIgnoreCase))
                {
                    card.NotifyStockChanged();
                }
            }
            catch { }
        }
    }

    private void Clear()
    {
        for (int i = _content.childCount - 1; i >= 0; i--)
            UnityEngine.Object.Destroy(_content.GetChild(i).gameObject);
        _cards.Clear();
    }

    private void ShowEmptyState(string msg)
    {
        var placeholder = UIFactory.Text("EmptyState", msg, _content, UITheme.Sp(14), TextAnchor.MiddleCenter);
        placeholder.color = new Color(0.6f, 0.6f, 0.6f, 1f);
    }

    private void BuildGrid(List<ItemPOCO> items)
    {
        const int columns = 5;
        int rows = (items.Count + columns - 1) / columns;
        for (int r = 0; r < rows; r++)
        {
            var rowGO = new GameObject($"Row_{r}");
            rowGO.transform.SetParent(_content, false);
            var rowLE = rowGO.AddComponent<LayoutElement>();
            rowLE.flexibleWidth = 1f;
            rowLE.preferredHeight = UITheme.Dp(175f);
            rowLE.minHeight = UITheme.Dp(175f);

            var rowHlg = rowGO.AddComponent<HorizontalLayoutGroup>();
            rowHlg.spacing = UITheme.Dp(6f);
            rowHlg.padding = new RectOffset(0, 0, 0, 0);
            rowHlg.childControlWidth = true;
            rowHlg.childControlHeight = true;
            rowHlg.childForceExpandWidth = true;
            rowHlg.childForceExpandHeight = false;
            rowHlg.childAlignment = TextAnchor.UpperLeft;

            for (int c = 0; c < columns; c++)
            {
                int idx = r * columns + c;
                if (idx < items.Count)
                {
                    var item = items[idx];
                    var card = new ItemCard(item);
                    _cards.Add(card);
                    card.OnPurchaseResult += res => OnPurchaseResult?.Invoke(res);
                    card.Build(rowGO.transform);
                }
                else
                {
                    var spacer = UIFactory.Panel("Spacer", rowGO.transform, Color.clear);
                    spacer.AddComponent<LayoutElement>().flexibleWidth = 1f;
                }
            }
        }
    }
}

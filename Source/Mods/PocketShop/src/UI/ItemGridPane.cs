using System;
using System.Collections.Generic;
using PocketShop.Services;
using S1API.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop.UI;

/// <summary>
/// 5-column grid of ItemCards for the currently active shop. The last row is
/// padded with empty cells. Scrolls when total item count exceeds the viewport.
/// </summary>
public class ItemGridPane
{
    public event Action<PurchaseResultData>? OnPurchaseResult;
    public event Action<ItemPOCO>? OnInspectRequested;

    private readonly RectTransform _root;
    private readonly List<ItemCard> _cards = new();
    private GameObject _scrollGO = null!;
    private RectTransform _content = null!;

    public ItemGridPane(RectTransform root) { _root = root; Build(); }

    private void Build()
    {
        _scrollGO = new GameObject("Scroll");
        _scrollGO.transform.SetParent(_root, false);
        var sRt = _scrollGO.AddComponent<RectTransform>();
        sRt.anchorMin = Vector2.zero;
        sRt.anchorMax = Vector2.one;
        sRt.offsetMin = Vector2.zero;
        sRt.offsetMax = Vector2.zero;

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

        ShopCatalog.OnCatalogChanged += OnCatalogChangedHandler;
    }

    public void SetActive(bool active)
    {
        if (_scrollGO != null)
        {
            _scrollGO.SetActive(active);
        }
    }

    private void OnCatalogChangedHandler() => RefreshActiveShop();

    public void Dispose()
    {
        ShopCatalog.OnCatalogChanged -= OnCatalogChangedHandler;
        Clear();
    }

    public void RefreshActiveShop()
    {
        if (_content == null) return;
        Clear();
        var currentShopCode = ActiveShopCode;
        if (string.IsNullOrEmpty(currentShopCode)) return;
        var items = ShopCatalog.GetItemsOfShop(currentShopCode);
        if (items.Count == 0)
        {
            ShowEmptyState("No items in stock.");
            return;
        }
        BuildGrid(items);
    }

    /// <summary>Currently displayed shop code (set externally by PocketShopApp).</summary>
    public string ActiveShopCode { get; set; } = string.Empty;

    /// <summary>Refresh every card's BUY enabled-state (called on Cash/Bank change or payment toggle).</summary>
    public void RefreshAllBuyStates()
    {
        foreach (var card in _cards) card.RefreshBuyState();
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
                    card.OnInspectRequested += itm => OnInspectRequested?.Invoke(itm);
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

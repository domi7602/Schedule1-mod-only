using System;
using System.Collections.Generic;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

/// <summary>
/// 4-column x 3-row Store Directory showing colorful store tiles
/// with NPC avatar / icon, shop name, and item count.
/// Matches the reference mockup design.
/// </summary>
public class StoreCatalogPane
{
    public event Action<ShopPOCO>? OnShopSelected;

    private readonly RectTransform _parent;
    private GameObject? _rootPanel;
    private bool _disposed;

    public StoreCatalogPane(RectTransform parent)
    {
        _parent = parent;
    }

    /// <summary>Live search filter over the store tiles (name or code, case-insensitive).</summary>
    public string Filter { get; set; } = string.Empty;

    public void Build()
    {
        if (_disposed) return;
        DestroyOwnedObject(_rootPanel);
        _rootPanel = null;

        _rootPanel = new GameObject("StoreCatalogRoot");
        _rootPanel.transform.SetParent(_parent, false);
        var rRt = _rootPanel.AddComponent<RectTransform>();
        rRt.anchorMin = Vector2.zero;
        rRt.anchorMax = Vector2.one;
        rRt.offsetMin = Vector2.zero;
        rRt.offsetMax = Vector2.zero;

        var scrollRect = _rootPanel.AddComponent<ScrollRect>();
        scrollRect.scrollSensitivity = 25f;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        var viewportGO = new GameObject("Viewport");
        viewportGO.transform.SetParent(_rootPanel.transform, false);
        var vrt = viewportGO.AddComponent<RectTransform>();
        vrt.anchorMin = Vector2.zero;
        vrt.anchorMax = Vector2.one;
        vrt.offsetMin = Vector2.zero;
        vrt.offsetMax = Vector2.zero;
        viewportGO.AddComponent<Image>().color = new Color(0, 0, 0, 0);
        viewportGO.AddComponent<RectMask2D>();
        scrollRect.viewport = vrt;

        // Anchor the content to the top and size it to its rows; this keeps 4 columns and
        // makes every row reachable when the filtered shop list exceeds the viewport.
        var contentGO = new GameObject("Content");
        contentGO.transform.SetParent(viewportGO.transform, false);
        var contentRt = contentGO.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0f, 1f);
        contentRt.anchorMax = new Vector2(1f, 1f);
        contentRt.pivot = new Vector2(0.5f, 1f);
        contentRt.anchoredPosition = Vector2.zero;
        contentRt.sizeDelta = new Vector2(0f, Mathf.Max(vrt.rect.height, UITheme.Dp(105f)));

        var vlg = contentGO.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(6f);
        vlg.padding = new RectOffset((int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        scrollRect.content = contentRt;

        var shops = FilterShops(ShopCatalog.Shops);
        if (shops.Count == 0)
        {
            string emptyMsg = string.IsNullOrEmpty(Filter) ? "No stores registered in town." : "No matching stores.";
            var emptyTxt = UIFactory.Text("EmptyState", emptyMsg, contentGO.transform, UITheme.Sp(14), TextAnchor.MiddleCenter);
            emptyTxt.color = new Color(0.6f, 0.6f, 0.6f, 1f);
            return;
        }

        const int columns = 4;
        int rows = (shops.Count + columns - 1) / columns;
        float minRowHeight = UITheme.Dp(105f);
        float rowSpacing = UITheme.Dp(6f);
        float verticalPadding = UITheme.Dp(12f);
        float viewportHeight = Mathf.Max(0f, vrt.rect.height);
        float rowHeight = Mathf.Max(minRowHeight,
            (viewportHeight - verticalPadding - rowSpacing * Mathf.Max(0, rows - 1)) / rows);
        float contentHeight = verticalPadding + rowSpacing * Mathf.Max(0, rows - 1) + rowHeight * rows;
        contentRt.sizeDelta = new Vector2(0f, Mathf.Max(viewportHeight, contentHeight));

        for (int r = 0; r < rows; r++)
        {
            var rowGO = new GameObject($"StoreRow_{r}");
            rowGO.transform.SetParent(contentGO.transform, false);
            var rowLE = rowGO.AddComponent<LayoutElement>();
            rowLE.minHeight = minRowHeight;
            rowLE.preferredHeight = rowHeight;
            rowLE.flexibleWidth = 1f;
            rowLE.flexibleHeight = 0f;

            var rowHlg = rowGO.AddComponent<HorizontalLayoutGroup>();
            rowHlg.spacing = UITheme.Dp(6f);
            rowHlg.padding = new RectOffset(0, 0, 0, 0);
            rowHlg.childControlWidth = true;
            rowHlg.childControlHeight = true;
            rowHlg.childForceExpandWidth = true;
            rowHlg.childForceExpandHeight = true;

            for (int c = 0; c < columns; c++)
            {
                int idx = r * columns + c;
                if (idx < shops.Count)
                {
                    var shop = shops[idx];
                    BuildStoreCard(rowGO.transform, shop, rowHeight);
                }
                else
                {
                    var spacer = UIFactory.Panel("Spacer", rowGO.transform, Color.clear);
                    spacer.AddComponent<LayoutElement>().flexibleWidth = 1f;
                }
            }
        }
    }

    /// <summary>Live search filter over the store tiles (name or code, case-insensitive).</summary>
    private List<ShopPOCO> FilterShops(IReadOnlyList<ShopPOCO> all)
    {
        var list = new List<ShopPOCO>(all.Count);
        if (string.IsNullOrEmpty(Filter))
        {
            for (int i = 0; i < all.Count; i++) list.Add(all[i]);
            return list;
        }
        string needle = Filter.Trim().ToLowerInvariant();
        for (int i = 0; i < all.Count; i++)
        {
            var s = all[i];
            if (s == null) continue;
            string name = (s.Name ?? string.Empty).ToLowerInvariant();
            string code = (s.ShopCode ?? string.Empty).ToLowerInvariant();
            if (name.Contains(needle) || code.Contains(needle)) list.Add(s);
        }
        return list;
    }

    private void BuildStoreCard(Transform parent, ShopPOCO shop, float rowH)
    {
        bool locked = shop.IsLocked;
        bool gateUnknown = !shop.GateKnown;
        bool closed = shop.GateKnown && !locked && !shop.IsOpen;

        var cardColor = ShopColorScheme.ColorFor(shop.ShopCode);
        // A locked, closed, or unverified shop reads as inactive; unknown is not treated as open.
        bool inactive = locked || closed || gateUnknown;
        if (inactive)
            cardColor = Color.Lerp(cardColor, new Color(0.10f, 0.11f, 0.13f, 1f), 0.62f);

        // Outer Card
        var card = UIFactory.Panel($"StoreCard_{shop.ShopCode}", parent, cardColor);
        var cardLE = card.AddComponent<LayoutElement>();
        cardLE.flexibleWidth = 1f;
        cardLE.preferredHeight = UITheme.Dp(105f);
        cardLE.minHeight = UITheme.Dp(105f);

        var cardVlg = card.AddComponent<VerticalLayoutGroup>();
        cardVlg.spacing = 0f;
        cardVlg.padding = new RectOffset(0, 0, 0, 0);
        cardVlg.childControlWidth = true;
        cardVlg.childControlHeight = true;
        cardVlg.childForceExpandWidth = true;
        cardVlg.childForceExpandHeight = false;

        // Top section: Avatar / Icon (centered)
        var topSection = UIFactory.Panel("AvatarSection", card.transform, Color.clear);
        var topLE = topSection.AddComponent<LayoutElement>();
        topLE.flexibleHeight = 1f;
        topLE.minHeight = UITheme.Dp(60f);

        // Circular backdrop vignette matching vanilla framed avatar circles
        var backdropGO = new GameObject("AvatarBackdrop");
        backdropGO.transform.SetParent(topSection.transform, false);
        var bRt = backdropGO.AddComponent<RectTransform>();
        bRt.anchorMin = new Vector2(0.5f, 0.5f);
        bRt.anchorMax = new Vector2(0.5f, 0.5f);
        bRt.pivot = new Vector2(0.5f, 0.5f);
        bRt.sizeDelta = new Vector2(UITheme.Dp(52f), UITheme.Dp(52f));
        var bImg = backdropGO.AddComponent<Image>();
        bImg.sprite = NPCPortraitService.GetCircleSprite(64);
        bImg.color = new Color(0f, 0f, 0f, 0.30f);
        bImg.raycastTarget = false;

        var avatar = NPCPortraitService.GetAvatar(shop.ShopCode, shop.Name, 128);
        if (avatar != null)
        {
            var avatarGO = new GameObject("AvatarImg");
            avatarGO.transform.SetParent(topSection.transform, false);
            var aRt = avatarGO.AddComponent<RectTransform>();
            aRt.anchorMin = new Vector2(0.5f, 0.5f);
            aRt.anchorMax = new Vector2(0.5f, 0.5f);
            aRt.pivot = new Vector2(0.5f, 0.5f);
            aRt.sizeDelta = new Vector2(UITheme.Dp(50f), UITheme.Dp(50f));

            var img = avatarGO.AddComponent<Image>();
            img.sprite = avatar;
            img.preserveAspect = true;
            img.raycastTarget = false;
        }

        // Bottom section: Dark banner with Store Name & Item count
        var bottomBanner = UIFactory.Panel("BottomBanner", card.transform, new Color(0f, 0f, 0f, 0.45f));
        var bLE = bottomBanner.AddComponent<LayoutElement>();
        bLE.minHeight = UITheme.Dp(42f);
        bLE.preferredHeight = UITheme.Dp(42f);
        bLE.flexibleHeight = 0f;

        var bVlg = bottomBanner.AddComponent<VerticalLayoutGroup>();
        bVlg.spacing = UITheme.Dp(1f);
        bVlg.padding = new RectOffset((int)UITheme.Dp(4f), (int)UITheme.Dp(4f), (int)UITheme.Dp(3f), (int)UITheme.Dp(3f));
        bVlg.childControlWidth = true;
        bVlg.childControlHeight = true;
        bVlg.childForceExpandWidth = true;
        bVlg.childForceExpandHeight = false;
        bVlg.childAlignment = TextAnchor.MiddleCenter;

        var nameTxt = UIFactory.Text("ShopName", shop.Name, bottomBanner.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold | FontStyle.Italic);
        nameTxt.color = Color.white;
        nameTxt.raycastTarget = false;
        nameTxt.horizontalOverflow = HorizontalWrapMode.Wrap;

        string subLabel;
        if (locked) subLabel = "LOCKED";
        else if (gateUnknown) subLabel = "VERIFY";
        else if (closed) subLabel = string.IsNullOrEmpty(shop.HoursText) ? "CLOSED" : $"CLOSED {shop.HoursText}";
        else subLabel = $"{shop.ItemCount} items";

        var countTxt = UIFactory.Text("ItemCount", subLabel, bottomBanner.transform, UITheme.Sp(10), TextAnchor.MiddleCenter);
        countTxt.color = locked || gateUnknown
            ? new Color(0.95f, 0.58f, 0.45f, 1f)
            : new Color(0.90f, 0.92f, 0.95f, 0.90f);
        countTxt.raycastTarget = false;

        // Button overlay for the whole card. A locked shop is not enterable.
        var btn = card.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        if (locked)
        {
            btn.interactable = false;
        }
        else
        {
            ButtonUtils.AddListener(btn, () => OnShopSelected?.Invoke(shop));
        }
    }

    public void SetActive(bool active)
    {
        if (_disposed || !IsOwnedObjectAlive(_rootPanel)) return;
        try { _rootPanel!.SetActive(active); }
        catch { }
    }

    /// <summary>
    /// Refresh the store count badge whenever the catalog changes without keeping callbacks
    /// alive after the pane is disposed.
    /// </summary>
    public void RefreshShopCount()
    {
        if (_disposed || !IsOwnedObjectAlive(_rootPanel)) return;
        try
        {
            if (_rootPanel!.activeSelf) Build();
        }
        catch { }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        OnShopSelected = null;
        var root = _rootPanel;
        _rootPanel = null;
        DestroyOwnedObject(root);
    }

    private static bool IsOwnedObjectAlive(GameObject? target)
    {
        if (target == null) return false;
        try { return target.Pointer != IntPtr.Zero && !target.WasCollected; }
        catch { return false; }
    }

    private static void DestroyOwnedObject(GameObject? target)
    {
        if (!IsOwnedObjectAlive(target)) return;
        try { UnityEngine.Object.Destroy(target); }
        catch { }
    }
}

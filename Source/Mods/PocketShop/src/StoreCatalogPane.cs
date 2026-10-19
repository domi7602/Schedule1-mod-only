using System;
using System.Collections.Generic;
using MelonLoader;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop.UI;

/// <summary>
/// Store Directory: a 4-column rectangle of colorful store tiles, each with an NPC avatar /
/// icon, the shop name and the item count. Matches the reference mockup design.
///
/// Layout is deterministic: every tile is anchored to an equal-width column band of the
/// content rect and placed at an explicitly computed y offset - no HorizontalLayoutGroup
/// is involved. Layout groups cannot produce equal tiles here: LayoutUtility resolves a
/// child's preferred size to the MAX over all ILayoutElement components, so the shop-name
/// Text's measured width always beat LayoutElement overrides and the columns ended up
/// ragged. Anchored bands make every tile exactly the same size and every column edge a
/// straight line, and they self-adapt to width changes without recomputation.
///
/// The tile size itself comes from <see cref="StoreGridLayout"/>: it is sized for three visible
/// rows and never for the catalog size, so the full directory, a search result and a single shop
/// all use one tile size, and a longer catalog overflows into the scroller instead of shrinking
/// the tiles. A last row that is not full is completed with subtle non-interactive placeholder
/// cells, so the grid always reads as a rectangle. Tile heights are recomputed whenever the
/// measured viewport height changes (see <see cref="EnsureLayout"/>): the first measurement can be
/// a stale pre-layout value, and re-applying on change makes the very first open converge to the
/// correct layout within a frame or two.
/// </summary>
public class StoreCatalogPane
{
    private const int Columns = StoreGridLayout.Columns;

    /// <summary>Subtle dark filler for the trailing cells of a partly filled last row.</summary>
    private static readonly Color EmptyCellColor = new Color(0.11f, 0.12f, 0.15f, 0.55f);

    public event Action<ShopPOCO>? OnShopSelected;

    private readonly RectTransform _parent;
    private GameObject? _rootPanel;
    private bool _disposed;

    private RectTransform? _viewportRt;
    private RectTransform? _contentRt;
    private readonly List<GridCell> _cells = new();
    private int _displayedItems;
    private float _appliedViewportHeight = -1f;
    private bool _layoutFinalized;

    /// <summary>One laid-out grid cell: a store tile, or a placeholder completing the last row.</summary>
    private readonly struct GridCell
    {
        public GridCell(RectTransform rt, int row, int col)
        {
            Rt = rt;
            Row = row;
            Col = col;
        }

        public RectTransform Rt { get; }
        public int Row { get; }
        public int Col { get; }
    }

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
        _viewportRt = null;
        _contentRt = null;
        _cells.Clear();
        _displayedItems = 0;
        _appliedViewportHeight = -1f;
        _layoutFinalized = false;

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

        // Content is top-anchored and manually sized (no layout group on purpose): the
        // tiles anchor into equal column bands so the grid stays perfectly aligned and
        // every row remains reachable when the filtered shop list exceeds the viewport.
        var contentGO = new GameObject("Content");
        contentGO.transform.SetParent(viewportGO.transform, false);
        var contentRt = contentGO.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0f, 1f);
        contentRt.anchorMax = new Vector2(1f, 1f);
        contentRt.pivot = new Vector2(0.5f, 1f);
        contentRt.anchoredPosition = Vector2.zero;
        contentRt.sizeDelta = Vector2.zero;
        scrollRect.content = contentRt;

        _viewportRt = vrt;
        _contentRt = contentRt;

        var shops = FilterShops(ShopCatalog.Shops);
        if (shops.Count == 0)
        {
            string emptyMsg = string.IsNullOrEmpty(Filter) ? "No stores registered in town." : "No matching stores.";
            var emptyTxt = UIFactory.Text("EmptyState", emptyMsg, contentGO.transform, UITheme.Sp(14), TextAnchor.MiddleCenter);
            emptyTxt.color = new Color(0.6f, 0.6f, 0.6f, 1f);
            var eRt = emptyTxt.rectTransform;
            eRt.anchorMin = Vector2.zero;
            eRt.anchorMax = Vector2.one;
            eRt.offsetMin = Vector2.zero;
            eRt.offsetMax = Vector2.zero;
            return;
        }

        _displayedItems = shops.Count;

        // Every cell of the rectangle is created: real tiles first, then the trailing placeholder
        // cells that complete the last row. Both go through ConfigureTileRect, so a placeholder is
        // exactly as wide and tall as the tiles beside it.
        int cellCount = StoreGridLayout.CellCount(_displayedItems);
        float minTileHeight = MinTileHeight();
        for (int i = 0; i < cellCount; i++)
        {
            int row = i / Columns;
            int col = i % Columns;
            RectTransform cellRt = i < shops.Count
                ? BuildStoreCard(contentGO.transform, shops[i]).GetComponent<RectTransform>()
                : BuildPlaceholderCell(contentGO.transform, row, col);
            ConfigureTileRect(cellRt, row, col, minTileHeight);
            _cells.Add(new GridCell(cellRt, row, col));
        }

        ApplyTileSizing();
    }

    /// <summary>
    /// Places one tile in its grid cell: x spans an equal-width column band (gap/2 inset on
    /// each side), y is an explicit offset from the content top. Fully determined by
    /// (row, col, rowHeight) — identical for every tile of a row, so the columns form
    /// straight lines and all tiles share one size.
    /// </summary>
    private void ConfigureTileRect(RectTransform rt, int row, int col, float rowHeight)
    {
        float gap = TileGap();
        float pad = ContentPad();
        rt.anchorMin = new Vector2(col / (float)Columns, 1f);
        rt.anchorMax = new Vector2((col + 1) / (float)Columns, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        float yTop = pad + row * (rowHeight + gap);
        rt.offsetMin = new Vector2(gap * 0.5f, -(yTop + rowHeight));
        rt.offsetMax = new Vector2(-gap * 0.5f, -yTop);
    }

    /// <summary>
    /// Computes the tile height for the current viewport and writes it to every cell. The height
    /// comes from the fixed sizing target (three visible rows), never from the catalog size, so the
    /// directory, a filtered search and a one-shop list all render the same tile size. Rows beyond
    /// the target overflow into the scroller. No-op while Unity has not produced a valid viewport
    /// height yet - <see cref="EnsureLayout"/> retries until a real measurement arrives.
    /// </summary>
    private void ApplyTileSizing()
    {
        if (_viewportRt == null || _contentRt == null || _cells.Count == 0) return;
        float viewportHeight = _viewportRt.rect.height;
        if (viewportHeight <= 1f) return;

        float gap = TileGap();
        float pad = ContentPad();
        float tileHeight = StoreGridLayout.TileHeight(viewportHeight, _displayedItems, MinTileHeight(), pad, gap);

        for (int i = 0; i < _cells.Count; i++)
        {
            var cell = _cells[i];
            ConfigureTileRect(cell.Rt, cell.Row, cell.Col, tileHeight);
        }

        _contentRt.sizeDelta = new Vector2(0f,
            StoreGridLayout.ContentHeight(_displayedItems, tileHeight, pad, gap, viewportHeight));

        _appliedViewportHeight = viewportHeight;
        if (!_layoutFinalized)
        {
            _layoutFinalized = true;
            MelonLogger.Msg($"[PocketShop] directory layout applied: viewport={viewportHeight:0.#} rows={StoreGridLayout.RowCount(_displayedItems)} sizingRows={StoreGridLayout.SizingRows(_displayedItems)} tile={tileHeight:0.#} cells={_cells.Count} placeholders={_cells.Count - _displayedItems}");
        }
    }

    /// <summary>
    /// Cheap per-frame check (called by the app). Re-applies the tile sizing whenever the
    /// measured viewport height changes — this covers the first open (where the first
    /// measurement is often a stale pre-layout value) and any later resize. A no-op while
    /// nothing changed.
    /// </summary>
    public void EnsureLayout()
    {
        if (_disposed || !IsOwnedObjectAlive(_rootPanel)) return;
        if (_cells.Count == 0) return;
        try
        {
            float viewportHeight = _viewportRt != null ? _viewportRt.rect.height : 0f;
            if (viewportHeight <= 1f) return;
            if (_layoutFinalized && Mathf.Abs(viewportHeight - _appliedViewportHeight) <= 0.5f) return;
            ApplyTileSizing();
        }
        catch { }
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

    /// <summary>
    /// Subtle filler for the trailing cells of a partly filled last row. It carries no click
    /// handler and does not receive raycasts, so it can never be selected or block a scroll drag -
    /// it only keeps the grid a full rectangle instead of a ragged last row.
    /// </summary>
    private RectTransform BuildPlaceholderCell(Transform parent, int row, int col)
    {
        var go = UIFactory.Panel($"StoreCellEmpty_{row}_{col}", parent, EmptyCellColor);
        var img = go.GetComponent<Image>();
        if (img != null) img.raycastTarget = false;
        return go.GetComponent<RectTransform>();
    }

    private GameObject BuildStoreCard(Transform parent, ShopPOCO shop)
    {
        bool locked = shop.IsLocked;
        bool gateUnknown = !shop.GateKnown;
        bool closed = shop.GateKnown && !locked && !shop.IsOpen;

        var cardColor = ShopColorScheme.ColorFor(shop.ShopCode);
        // A locked, closed, or unverified shop reads as inactive; unknown is not treated as open.
        bool inactive = locked || closed || gateUnknown;
        if (inactive)
            cardColor = Color.Lerp(cardColor, new Color(0.10f, 0.11f, 0.13f, 1f), 0.62f);

        // Outer Card — the rect is owned by ConfigureTileRect (anchored grid cell), so no
        // LayoutElement here; only the internal stack uses a VerticalLayoutGroup.
        var card = UIFactory.Panel($"StoreCard_{shop.ShopCode}", parent, cardColor);

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
        topLE.minHeight = AvatarMinHeight();

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

        // Bottom section: Dark banner with Store Name & Item count. Its height is the same budget
        // StoreGridLayout uses for the minimum usable tile height, so the two-line name band and
        // the count line always fit inside a tile.
        var bottomBanner = UIFactory.Panel("BottomBanner", card.transform, new Color(0f, 0f, 0f, 0.45f));
        var bLE = bottomBanner.AddComponent<LayoutElement>();
        bLE.minHeight = BannerMinHeight();
        bLE.preferredHeight = BannerMinHeight();
        bLE.flexibleHeight = 0f;

        var bVlg = bottomBanner.AddComponent<VerticalLayoutGroup>();
        bVlg.spacing = BannerSpacing();
        bVlg.padding = new RectOffset(BannerPadHorizontal(), BannerPadHorizontal(), BannerPadVertical(), BannerPadVertical());
        bVlg.childControlWidth = true;
        bVlg.childControlHeight = true;
        bVlg.childForceExpandWidth = true;
        bVlg.childForceExpandHeight = false;
        bVlg.childAlignment = TextAnchor.MiddleCenter;

        var nameTxt = UIFactory.Text("ShopName", shop.Name, bottomBanner.transform, NameFontSize(), TextAnchor.MiddleCenter, FontStyle.Bold | FontStyle.Italic);
        nameTxt.color = Color.white;
        nameTxt.raycastTarget = false;
        nameTxt.horizontalOverflow = HorizontalWrapMode.Wrap;
        nameTxt.verticalOverflow = VerticalWrapMode.Truncate;
        // The name band is exactly two lines tall: a longer name wraps into the second line and is
        // truncated there. The stored shop name is rendered verbatim - never rewritten, shortened,
        // re-cased or otherwise "normalized" from a guess about its shape.
        var nameLE = nameTxt.gameObject.AddComponent<LayoutElement>();
        nameLE.minHeight = StoreGridLayout.NameBandHeight(NameFontSize());
        nameLE.preferredHeight = nameLE.minHeight;
        nameLE.flexibleHeight = 0f;
        nameLE.layoutPriority = 1;

        string subLabel;
        if (locked) subLabel = "LOCKED";
        else if (gateUnknown) subLabel = "VERIFY";
        else if (closed) subLabel = string.IsNullOrEmpty(shop.HoursText) ? "CLOSED" : $"CLOSED {shop.HoursText}";
        else subLabel = $"{shop.ItemCount} items";

        var countTxt = UIFactory.Text("ItemCount", subLabel, bottomBanner.transform, CountFontSize(), TextAnchor.MiddleCenter);
        countTxt.color = locked || gateUnknown
            ? new Color(0.95f, 0.58f, 0.45f, 1f)
            : new Color(0.90f, 0.92f, 0.95f, 0.90f);
        countTxt.raycastTarget = false;
        var countLE = countTxt.gameObject.AddComponent<LayoutElement>();
        countLE.minHeight = CountFontSize();
        countLE.preferredHeight = CountFontSize();
        countLE.flexibleHeight = 0f;
        countLE.layoutPriority = 1;

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

        return card;
    }

    private static float TileGap() => UITheme.Dp(6f);

    private static float ContentPad() => UITheme.Dp(6f);

    // Sizing constants shared with StoreGridLayout: the minimum usable tile height is the avatar
    // block plus the banner, and the banner budget is the two-line name band plus the count line
    // plus its insets - so the tile floor always matches what a card actually draws.
    private static int NameFontSize() => UITheme.Sp(11f);

    private static int CountFontSize() => UITheme.Sp(10f);

    private static float AvatarMinHeight() => UITheme.Dp(60f);

    private static int BannerPadHorizontal() => (int)UITheme.Dp(4f);

    private static int BannerPadVertical() => (int)UITheme.Dp(3f);

    private static float BannerSpacing() => UITheme.Dp(1f);

    private static float BannerMinHeight() =>
        StoreGridLayout.BannerMinHeight(NameFontSize(), CountFontSize(), BannerPadVertical(), BannerPadVertical(), BannerSpacing());

    private static float MinTileHeight() => StoreGridLayout.MinTileHeight(AvatarMinHeight(), BannerMinHeight());

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

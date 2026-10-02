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
/// 4-column x 3-row Store Directory showing colorful store tiles
/// with NPC avatar / icon, shop name, and item count.
/// Matches the reference mockup design.
/// </summary>
public class StoreCatalogPane
{
    public event Action<ShopPOCO>? OnShopSelected;

    private readonly RectTransform _parent;
    private GameObject? _rootPanel;

    /// <summary>
    /// Units below which a measured viewport is implausibly small. The phone canvas is
    /// 655×1201 units and the content area is always most of the short side — the stale
    /// first-frame value in the live log (2026-10-02) was 100u vs the real 546u.
    /// </summary>
    private const float MinPlausibleViewportUnits = 300f;

    private bool _rebuildRetryPending;
    private int _rebuildRetries;

    public StoreCatalogPane(RectTransform parent)
    {
        _parent = parent;
    }

    public void Build()
    {
        if (_rootPanel != null)
        {
            UnityEngine.Object.Destroy(_rootPanel);
        }

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

        var contentGO = new GameObject("Content");
        contentGO.transform.SetParent(viewportGO.transform, false);
        var contentRt = contentGO.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0, 1);
        contentRt.anchorMax = new Vector2(1, 1);
        contentRt.pivot = new Vector2(0.5f, 1f);
        contentRt.offsetMin = Vector2.zero;
        contentRt.offsetMax = Vector2.zero;

        var vlg = contentGO.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(6f);
        vlg.padding = new RectOffset((int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var csf = contentGO.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        scrollRect.content = contentRt;

        var shops = ShopCatalog.Shops;
        if (shops.Count == 0)
        {
            var emptyTxt = UIFactory.Text("EmptyState", "No stores registered in town.", contentGO.transform, UITheme.Sp(14), TextAnchor.MiddleCenter);
            emptyTxt.color = new Color(0.6f, 0.6f, 0.6f, 1f);
            return;
        }

        const int columns = 4;
        int rows = (shops.Count + columns - 1) / columns;

        // v0.3.6: exact-fit rows — measure the live viewport and divide it by the
        // row count, so all cards fit on screen with neither clipping nor scroll
        // nor black void. Overflow (many mod-injected shops) falls back to a
        // 90dp floor + scroll.
        float rowH = ComputeFittedRowHeight(rows, vlg);

        for (int r = 0; r < rows; r++)
        {
            var rowGO = new GameObject($"StoreRow_{r}");
            rowGO.transform.SetParent(contentGO.transform, false);
            var rowLE = rowGO.AddComponent<LayoutElement>();
            rowLE.minHeight = rowH;
            rowLE.preferredHeight = rowH;
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
                    BuildStoreCard(rowGO.transform, shop, rowH);
                }
                else
                {
                    var spacer = UIFactory.Panel("Spacer", rowGO.transform, Color.clear);
                    spacer.AddComponent<LayoutElement>().flexibleWidth = 1f;
                }
            }
        }
    }

    private void BuildStoreCard(Transform parent, ShopPOCO shop, float rowH)
    {
        var cardColor = ShopColorScheme.ColorFor(shop.ShopCode);

        // Outer Card
        var card = UIFactory.Panel($"StoreCard_{shop.ShopCode}", parent, cardColor);
        var cardLE = card.AddComponent<LayoutElement>();
        cardLE.flexibleWidth = 1f;
        cardLE.flexibleHeight = 0f;
        cardLE.preferredHeight = rowH;
        cardLE.minHeight = rowH;

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
        topLE.minHeight = UITheme.Dp(65f);

        // Circular backdrop vignette matching vanilla framed avatar circles
        var backdropGO = new GameObject("AvatarBackdrop");
        backdropGO.transform.SetParent(topSection.transform, false);
        var bRt = backdropGO.AddComponent<RectTransform>();
        bRt.anchorMin = new Vector2(0.5f, 0.5f);
        bRt.anchorMax = new Vector2(0.5f, 0.5f);
        bRt.pivot = new Vector2(0.5f, 0.5f);
        bRt.sizeDelta = new Vector2(UITheme.Dp(68f), UITheme.Dp(68f));
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
            aRt.sizeDelta = new Vector2(UITheme.Dp(66f), UITheme.Dp(66f));

            var img = avatarGO.AddComponent<Image>();
            img.sprite = avatar;
            img.preserveAspect = true;
            img.raycastTarget = false;
        }

        // Bottom section: Dark banner with Store Name & Item count
        var bottomBanner = UIFactory.Panel("BottomBanner", card.transform, new Color(0f, 0f, 0f, 0.45f));
        var bLE = bottomBanner.AddComponent<LayoutElement>();
        bLE.minHeight = UITheme.Dp(48f);
        bLE.preferredHeight = UITheme.Dp(48f);
        bLE.flexibleHeight = 0f;

        var bVlg = bottomBanner.AddComponent<VerticalLayoutGroup>();
        bVlg.spacing = UITheme.Dp(1f);
        bVlg.padding = new RectOffset((int)UITheme.Dp(4f), (int)UITheme.Dp(4f), (int)UITheme.Dp(3f), (int)UITheme.Dp(3f));
        bVlg.childControlWidth = true;
        bVlg.childControlHeight = true;
        bVlg.childForceExpandWidth = true;
        bVlg.childForceExpandHeight = false;
        bVlg.childAlignment = TextAnchor.MiddleCenter;

        var nameTxt = UIFactory.Text("ShopName", shop.Name, bottomBanner.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold | FontStyle.Italic);
        nameTxt.color = Color.white;
        nameTxt.raycastTarget = false;
        nameTxt.horizontalOverflow = HorizontalWrapMode.Wrap;

        var countTxt = UIFactory.Text("ItemCount", $"{shop.ItemCount} items", bottomBanner.transform, UITheme.Sp(10), TextAnchor.MiddleCenter);
        countTxt.color = new Color(0.90f, 0.92f, 0.95f, 0.90f);
        countTxt.raycastTarget = false;

        // Button overlay for the whole card
        var btn = card.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(btn, () => OnShopSelected?.Invoke(shop));
    }

    /// <summary>
    /// Divides the live content height exactly by the row count so every store card
    /// fits on screen (v0.3.6 — user request: squeeze into format, no scroll, no clip).
    /// v0.3.8 fix: the content panel gets its height from the ROOT vertical layout,
    /// so the root (not the panel itself) must be rebuilt before measuring —
    /// rebuilding only the panel returns a stale mini value and the floor wins.
    /// v0.3.9 fix: even that was not enough on the FIRST open — the app becomes visible
    /// in the same frame the directory builds, so the freshly activated tree had never
    /// run a layout pass (live log 2026-10-02: viewport=100u instead of 546u; users had
    /// to reopen the app twice). Now the layout is forced from the TOPMOST rect under
    /// the canvas, and a stale result schedules an exact rebuild for the next frame.
    /// Falls back to a 90dp floor (scrolls on overflow) when the viewport cannot be
    /// measured yet, and to the proven 105dp rows when measurement fails entirely.
    /// </summary>
    private float ComputeFittedRowHeight(int rows, VerticalLayoutGroup contentVlg)
    {
        float parentH = 0f;
        bool plausible = false;
        try
        {
            Canvas.ForceUpdateCanvases();

            // The whole chain counts: app container -> _mainBG vertical layout ->
            // content area. Rebuilding a mid-level rect (v0.3.8) left the chain above
            // it stale on the first open, so climb to the top first.
            var top = FindTopmostLayoutRect(_parent);
            if (top != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(top);

            parentH = _parent.rect.height;
            plausible = parentH >= MinPlausibleViewportUnits;
        }
        catch
        {
            plausible = false;
        }

        if (plausible)
        {
            _rebuildRetryPending = false;
            _rebuildRetries = 0;
        }
        else
        {
            // First paint on a stale rect: derive from the phone canvas so the
            // immediate build is at least close, and rebuild exactly next frame.
            _rebuildRetryPending = true;
            float estimate = UITheme.ActualWidth - UITheme.Dp(38f) - UITheme.Dp(30f);
            if (estimate > parentH)
                parentH = estimate;
            MelonLogger.Msg(
                $"[PocketShop] directory viewport stale ({_parent.rect.height:F0}u) — estimate {parentH:F0}u; exact rebuild next frame.");
        }

        if (parentH > 0f && rows > 0)
        {
            float padV = contentVlg.padding.top + contentVlg.padding.bottom;
            float gaps = contentVlg.spacing * (rows - 1);
            float fit = (parentH - padV - gaps) / rows;
            float rowH = Mathf.Max(UITheme.Dp(90f), fit);
            MelonLogger.Msg($"[PocketShop] directory rows={rows} rowH={rowH:F0}px viewport={parentH:F0}px{(plausible ? string.Empty : " (estimated)")}");
            return rowH;
        }
        return UITheme.Dp(105f);
    }

    /// <summary>
    /// Highest RectTransform under the canvas that contains this pane — rebuilding it
    /// runs every nested layout group. Stops at a Canvas (its rect is the render area,
    /// not a layout item) and at a non-RectTransform parent.
    /// </summary>
    private static RectTransform? FindTopmostLayoutRect(RectTransform rect)
    {
        var top = rect;
        for (int i = 0; i < 32; i++)
        {
            var parent = top.parent as RectTransform;
            if (parent == null)
                break;
            if (parent.GetComponent<Canvas>() != null)
                break;
            top = parent;
        }
        return top;
    }

    /// <summary>
    /// v0.3.9: runs from the app's per-frame update while the directory is visible —
    /// rebuilds the rows once with the live viewport after a stale first measurement
    /// (replaces the "reopen the app twice" workaround). Bounded to a few attempts.
    /// </summary>
    public void TickRetry()
    {
        if (!_rebuildRetryPending || _rootPanel == null)
            return;

        float parentH = _parent != null ? _parent.rect.height : 0f;
        bool plausible = parentH >= MinPlausibleViewportUnits;

        _rebuildRetries++;
        if (_rebuildRetries >= 5 || plausible)
        {
            bool rebuild = plausible;
            _rebuildRetryPending = false;
            _rebuildRetries = 0;
            if (rebuild)
            {
                MelonLogger.Msg($"[PocketShop] directory rebuilt with the live viewport ({parentH:F0}px) — no reopen needed.");
                Build();
            }
            else
            {
                MelonLogger.Warning("[PocketShop] directory viewport never became plausible — keeping the estimated rows.");
            }
        }
    }

    public void SetActive(bool active)
    {
        if (_rootPanel != null)
        {
            _rootPanel.SetActive(active);
        }
    }

    /// <summary>
    /// Bug-Audit 2026-09-13 (Round 5): refresh the store count badge whenever the
    /// catalog changes (stock update, new shop registered, etc.). Without this,
    /// the "CHOOSE A SHOP · N STORES" header shows stale counts.
    /// </summary>
    public void RefreshShopCount()
    {
        // Rebuild only if we're currently visible — avoids redundant allocations.
        if (_rootPanel != null && _rootPanel.activeSelf)
        {
            Build();
        }
    }

    public void Dispose()
    {
        if (_rootPanel != null)
        {
            UnityEngine.Object.Destroy(_rootPanel);
            _rootPanel = null;
        }
    }
}

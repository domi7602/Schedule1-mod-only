using System;
using System.Collections.Generic;
using PocketShop.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

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

        for (int r = 0; r < rows; r++)
        {
            var rowGO = new GameObject($"StoreRow_{r}");
            rowGO.transform.SetParent(contentGO.transform, false);
            var rowLE = rowGO.AddComponent<LayoutElement>();
            rowLE.minHeight = UITheme.Dp(105f);
            rowLE.preferredHeight = UITheme.Dp(105f);
            rowLE.flexibleWidth = 1f;

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
                    BuildStoreCard(rowGO.transform, shop);
                }
                else
                {
                    var spacer = UIFactory.Panel("Spacer", rowGO.transform, Color.clear);
                    spacer.AddComponent<LayoutElement>().flexibleWidth = 1f;
                }
            }
        }
    }

    private void BuildStoreCard(Transform parent, ShopPOCO shop)
    {
        var cardColor = ShopColorScheme.ColorFor(shop.ShopCode);

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

        var avatar = NPCPortraitService.GetAvatar(shop.ShopCode, shop.Name, 128);
        if (avatar != null)
        {
            var avatarGO = new GameObject("AvatarImg");
            avatarGO.transform.SetParent(topSection.transform, false);
            var aRt = avatarGO.AddComponent<RectTransform>();
            aRt.anchorMin = new Vector2(0.5f, 0.5f);
            aRt.anchorMax = new Vector2(0.5f, 0.5f);
            aRt.pivot = new Vector2(0.5f, 0.5f);
            aRt.sizeDelta = new Vector2(UITheme.Dp(48f), UITheme.Dp(48f));

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

        var countTxt = UIFactory.Text("ItemCount", $"{shop.ItemCount} items", bottomBanner.transform, UITheme.Sp(9), TextAnchor.MiddleCenter);
        countTxt.color = new Color(0.90f, 0.92f, 0.95f, 0.90f);
        countTxt.raycastTarget = false;

        // Button overlay for the whole card
        var btn = card.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(btn, () => OnShopSelected?.Invoke(shop));
    }

    public void SetActive(bool active)
    {
        if (_rootPanel != null)
        {
            _rootPanel.SetActive(active);
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

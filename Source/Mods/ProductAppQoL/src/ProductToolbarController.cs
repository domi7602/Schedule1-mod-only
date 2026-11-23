using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using Il2CppScheduleOne.Effects;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.UI.Phone.ProductManagerApp;
using MelonLoader;
using S1API.Input;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;
using GameProductManagerApp = Il2CppScheduleOne.UI.Phone.ProductManagerApp.ProductManagerApp;

namespace ProductAppQoL;

/// <summary>
/// Monitors the ProductAppQoL search input focus and manages <see cref="Controls.IsTyping"/>
/// so WASD and game hotkeys do not fire while typing.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class ProductSearchInputFocus : MonoBehaviour
{
    public ProductSearchInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField? searchInput;
    private bool _ownsTyping;

    private void Update()
    {
        bool focused = searchInput != null &&
                       searchInput.gameObject.activeInHierarchy &&
                       searchInput.isFocused;

        if (focused && !_ownsTyping)
        {
            _ownsTyping = true;
            Controls.IsTyping = true;
        }
        else if (!focused && _ownsTyping)
        {
            ReleaseTyping();
        }
    }

    private void OnDisable()
    {
        ReleaseTyping();
    }

    public void ReleaseTyping()
    {
        if (!_ownsTyping) return;
        _ownsTyping = false;
        Controls.IsTyping = false;
    }
}

/// <summary>
/// Adds a fixed search and sort toolbar above the product list scroll view in
/// <see cref="GameProductManagerApp"/>.
/// </summary>
internal sealed class ProductToolbarController
{
    private const float ToolbarHeight = 42f;
    private const float ToolbarGap = 6f;

    private static readonly Color ToolbarBgColor = new(0.11f, 0.13f, 0.16f, 0.94f);
    private static readonly Color InputBgColor = new(0.17f, 0.20f, 0.25f, 1f);
    private static readonly Color ButtonBgColor = new(0.22f, 0.26f, 0.32f, 1f);
    private static readonly Color ClearBgColor = new(0.32f, 0.20f, 0.22f, 1f);

    private readonly GameProductManagerApp _app;
    private InputField? _searchInput;
    private ProductSearchInputFocus? _inputFocus;
    private Button? _clearButton;
    private Text? _sortButtonText;
    private Text? _dirButtonText;

    private ProductSortMode _sortMode = ProductSortMode.Default;
    private bool _descending;
    private string _searchQuery = string.Empty;

    private bool _wasSearchActive;
    private readonly Dictionary<IntPtr, int> _originalOrder = new();
    private readonly Dictionary<IntPtr, bool> _savedExpandedState = new();

    public ProductToolbarController(GameProductManagerApp app)
    {
        _app = app;
    }

    public void Initialize()
    {
        if (_searchInput != null)
            return;

        ScrollRect? scrollRect = FindProductListScrollRect(_app);
        if (scrollRect == null)
        {
            MelonLogger.Warning("[ProductAppQoL] Could not locate product list ScrollRect; search/sort toolbar skipped.");
            return;
        }

        RectTransform? scrollRt = scrollRect.GetComponent<RectTransform>();
        Transform? listParent = scrollRt != null ? scrollRt.parent : null;
        if (scrollRt == null || listParent == null)
            return;

        BuildToolbar(listParent, scrollRt);
        RecordOriginalOrder();
        ApplyFilterAndSort();
    }

    public void OnAppOpenChanged(bool open)
    {
        if (!open)
        {
            _inputFocus?.ReleaseTyping();
            if (_searchInput != null && _searchInput.isFocused)
                _searchInput.DeactivateInputField();
            return;
        }

        if (_searchInput == null)
            Initialize();
        RecordOriginalOrder();
        ApplyFilterAndSort();
    }

    public void Refresh()
    {
        RecordOriginalOrder();
        ApplyFilterAndSort();
    }

    private void BuildToolbar(Transform parent, RectTransform scrollRt)
    {
        float topY = scrollRt.offsetMax.y;
        float leftX = scrollRt.offsetMin.x;
        float rightX = scrollRt.offsetMax.x;

        scrollRt.offsetMax = new Vector2(rightX, topY - (ToolbarHeight + ToolbarGap));

        var barGo = new GameObject("ProductAppQoL_Toolbar");
        barGo.transform.SetParent(parent, false);
        var barRt = barGo.AddComponent<RectTransform>();
        barRt.anchorMin = new Vector2(scrollRt.anchorMin.x, scrollRt.anchorMax.y);
        barRt.anchorMax = new Vector2(scrollRt.anchorMax.x, scrollRt.anchorMax.y);
        barRt.pivot = new Vector2(0.5f, 1f);
        barRt.offsetMin = new Vector2(leftX, topY - ToolbarHeight);
        barRt.offsetMax = new Vector2(rightX, topY);

        var barBg = barGo.AddComponent<Image>();
        barBg.color = ToolbarBgColor;

        var layout = barGo.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(6, 6, 4, 4);
        layout.spacing = 6f;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;

        // 1. Search InputField
        _searchInput = CreateSearchInputField(barGo.transform);
        var searchLe = _searchInput.gameObject.AddComponent<LayoutElement>();
        searchLe.flexibleWidth = 1f;
        searchLe.minWidth = 120f;
        EventHelper.AddListener<string>(OnSearchChanged, _searchInput.onValueChanged);

        _inputFocus = _searchInput.gameObject.AddComponent<ProductSearchInputFocus>();
        _inputFocus.searchInput = _searchInput;

        // 2. Clear ("×") Button
        var clearGo = new GameObject("ClearSearchButton");
        clearGo.transform.SetParent(barGo.transform, false);
        clearGo.AddComponent<RectTransform>();
        var clearImg = clearGo.AddComponent<Image>();
        clearImg.color = ClearBgColor;
        _clearButton = clearGo.AddComponent<Button>();
        _clearButton.targetGraphic = clearImg;
        var clearLe = clearGo.AddComponent<LayoutElement>();
        clearLe.preferredWidth = 30f;
        clearLe.minWidth = 30f;
        CreateButtonLabel(clearGo.transform, "×", 16);
        ButtonUtils.AddListener(_clearButton, OnClearClicked);
        clearGo.SetActive(false);

        // 3. Sort Mode Button
        var sortGo = new GameObject("SortModeButton");
        sortGo.transform.SetParent(barGo.transform, false);
        sortGo.AddComponent<RectTransform>();
        var sortImg = sortGo.AddComponent<Image>();
        sortImg.color = ButtonBgColor;
        var sortBtn = sortGo.AddComponent<Button>();
        sortBtn.targetGraphic = sortImg;
        var sortLe = sortGo.AddComponent<LayoutElement>();
        sortLe.preferredWidth = 104f;
        sortLe.minWidth = 96f;
        _sortButtonText = CreateButtonLabel(sortGo.transform, "Sort: Default", 13);
        ButtonUtils.AddListener(sortBtn, OnSortModeClicked);

        // 4. Sort Direction Button
        var dirGo = new GameObject("SortDirectionButton");
        dirGo.transform.SetParent(barGo.transform, false);
        dirGo.AddComponent<RectTransform>();
        var dirImg = dirGo.AddComponent<Image>();
        dirImg.color = ButtonBgColor;
        var dirBtn = dirGo.AddComponent<Button>();
        dirBtn.targetGraphic = dirImg;
        var dirLe = dirGo.AddComponent<LayoutElement>();
        dirLe.preferredWidth = 32f;
        dirLe.minWidth = 32f;
        _dirButtonText = CreateButtonLabel(dirGo.transform, "—", 15);
        ButtonUtils.AddListener(dirBtn, OnSortDirectionClicked);

        UpdateSortLabels();
    }

    private static InputField CreateSearchInputField(Transform parent)
    {
        var go = new GameObject("SearchInput");
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();

        var bg = go.AddComponent<Image>();
        bg.color = InputBgColor;

        var inputField = go.AddComponent<InputField>();
        inputField.targetGraphic = bg;
        inputField.lineType = InputField.LineType.SingleLine;
        inputField.caretWidth = 2;
        inputField.caretColor = Color.white;
        inputField.selectionColor = new Color(0.18f, 0.65f, 0.98f, 0.45f);

        Font? font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(go.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(8f, 2f);
        textRt.offsetMax = new Vector2(-8f, -2f);
        var text = textGo.AddComponent<Text>();
        text.font = font;
        text.fontSize = 14;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleLeft;
        text.supportRichText = false;
        text.raycastTarget = false;

        var phGo = new GameObject("Placeholder");
        phGo.transform.SetParent(go.transform, false);
        var phRt = phGo.AddComponent<RectTransform>();
        phRt.anchorMin = Vector2.zero;
        phRt.anchorMax = Vector2.one;
        phRt.offsetMin = new Vector2(8f, 2f);
        phRt.offsetMax = new Vector2(-8f, -2f);
        var placeholder = phGo.AddComponent<Text>();
        placeholder.font = font;
        placeholder.fontSize = 14;
        placeholder.fontStyle = FontStyle.Italic;
        placeholder.color = new Color(1f, 1f, 1f, 0.45f);
        placeholder.alignment = TextAnchor.MiddleLeft;
        placeholder.text = "Search name or effect...";
        placeholder.supportRichText = false;
        placeholder.raycastTarget = false;

        inputField.textComponent = text;
        inputField.placeholder = placeholder;
        return inputField;
    }

    private static Text CreateButtonLabel(Transform parent, string label, int fontSize)
    {
        var textGo = new GameObject("Label");
        textGo.transform.SetParent(parent, false);
        var rt = textGo.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(4f, 0f);
        rt.offsetMax = new Vector2(-4f, 0f);

        var text = textGo.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.text = label;
        text.raycastTarget = false;
        return text;
    }

    private void OnSearchChanged(string query)
    {
        _searchQuery = query ?? string.Empty;
        if (_clearButton != null)
            _clearButton.gameObject.SetActive(!string.IsNullOrEmpty(_searchQuery));
        ApplyFilterAndSort();
    }

    private void OnClearClicked()
    {
        if (_searchInput != null)
            _searchInput.text = string.Empty;
        else
            OnSearchChanged(string.Empty);
    }

    private void OnSortModeClicked()
    {
        _sortMode = ProductListLogic.NextMode(_sortMode);
        _descending = ProductListLogic.DefaultDescending(_sortMode);
        UpdateSortLabels();
        ApplyFilterAndSort();
    }

    private void OnSortDirectionClicked()
    {
        if (_sortMode == ProductSortMode.Default)
            return;

        _descending = !_descending;
        UpdateSortLabels();
        ApplyFilterAndSort();
    }

    private void UpdateSortLabels()
    {
        if (_sortButtonText != null)
            _sortButtonText.text = $"Sort: {ProductListLogic.GetSortLabel(_sortMode)}";

        if (_dirButtonText != null)
        {
            _dirButtonText.text = _sortMode == ProductSortMode.Default
                ? "—"
                : (_descending ? "↓" : "↑");
        }
    }

    private void ApplyFilterAndSort()
    {
        bool searchActive = !string.IsNullOrWhiteSpace(_searchQuery);
        List<ProductTypeContainer> containers = CollectContainers(_app);

        if (searchActive && !_wasSearchActive)
        {
            _savedExpandedState.Clear();
            for (int i = 0; i < containers.Count; i++)
            {
                ProductTypeContainer c = containers[i];
                _savedExpandedState[c.Pointer] = c._isExpanded;
            }
        }

        for (int i = 0; i < containers.Count; i++)
            ApplyToContainer(containers[i], searchActive);

        if (!searchActive && _wasSearchActive)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                ProductTypeContainer c = containers[i];
                c.gameObject.SetActive(true);
                if (_savedExpandedState.TryGetValue(c.Pointer, out bool wasExpanded))
                    c.SetDropdown(wasExpanded);
                c.RefreshNoneDisplay();
            }
            _savedExpandedState.Clear();
        }

        _wasSearchActive = searchActive;

        try
        {
            _app.DelayedRebuildLayout();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] DelayedRebuildLayout failed: {ex.Message}");
        }
    }

    private void ApplyToContainer(ProductTypeContainer container, bool searchActive)
    {
        RectTransform? entriesRoot = container._enteries;
        if (entriesRoot == null) return;

        int childCount = entriesRoot.childCount;
        var entryObjects = new List<ProductEntry>(childCount);
        var sortItems = new List<ProductSortItem>(childCount);
        int visibleCount = 0;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = entriesRoot.GetChild(i);
            ProductEntry? entry = child.GetComponent<ProductEntry>();
            if (entry == null) continue;

            if (!_originalOrder.TryGetValue(entry.Pointer, out int origIdx))
            {
                origIdx = i;
                _originalOrder[entry.Pointer] = origIdx;
            }

            ProductSortItem item = BuildSnapshot(entry, origIdx);
            bool matches = ProductListLogic.MatchesSearch(in item, _searchQuery);
            entry.gameObject.SetActive(matches);
            if (matches) visibleCount++;

            entryObjects.Add(entry);
            sortItems.Add(item);
        }

        if (entryObjects.Count > 1)
        {
            int[] ordered = ProductListLogic.SortIndices(sortItems, _sortMode, _descending);
            for (int pos = 0; pos < ordered.Length; pos++)
                entryObjects[ordered[pos]].transform.SetSiblingIndex(pos);
        }

        if (searchActive)
        {
            bool hasMatches = visibleCount > 0;
            container.gameObject.SetActive(hasMatches);
            if (hasMatches)
                container.SetDropdown(true);
        }
        else
        {
            container.gameObject.SetActive(true);
            container.RefreshNoneDisplay();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(entriesRoot);
    }

    private void RecordOriginalOrder()
    {
        List<ProductTypeContainer> containers = CollectContainers(_app);
        for (int c = 0; c < containers.Count; c++)
        {
            RectTransform? entriesRoot = containers[c]._enteries;
            if (entriesRoot == null) continue;

            for (int i = 0; i < entriesRoot.childCount; i++)
            {
                ProductEntry? entry = entriesRoot.GetChild(i).GetComponent<ProductEntry>();
                if (entry != null && !_originalOrder.ContainsKey(entry.Pointer))
                    _originalOrder[entry.Pointer] = i;
            }
        }
    }

    private static ProductSortItem BuildSnapshot(ProductEntry entry, int originalIndex)
    {
        ProductDefinition? def = entry.Definition;
        if (def == null)
            return new ProductSortItem(originalIndex, string.Empty, 0f, 0f, 0f, false, Array.Empty<string>());

        var effects = new List<string>();
        if (def.Properties != null)
        {
            for (int i = 0; i < def.Properties.Count; i++)
            {
                Effect? eff = def.Properties[i];
                if (eff != null && !string.IsNullOrEmpty(eff.Name))
                    effects.Add(eff.Name);
            }
        }

        bool isListed = false;
        try
        {
            var listed = ProductManager.ListedProducts;
            if (listed != null)
                isListed = listed.Contains(def);
        }
        catch { }

        return new ProductSortItem(
            originalIndex,
            def.Name ?? string.Empty,
            def.Price,
            def.MarketValue,
            def.GetAddictiveness(),
            isListed,
            effects);
    }

    private static List<ProductTypeContainer> CollectContainers(GameProductManagerApp app)
    {
        var result = new List<ProductTypeContainer>();
        if (app.FavouritesContainer != null)
            result.Add(app.FavouritesContainer);

        if (app.ProductTypeContainers != null)
        {
            for (int i = 0; i < app.ProductTypeContainers.Count; i++)
            {
                ProductTypeContainer? c = app.ProductTypeContainers[i];
                if (c != null)
                    result.Add(c);
            }
        }

        return result;
    }

    private static ScrollRect? FindProductListScrollRect(GameProductManagerApp app)
    {
        if (app.ProductTypeContainers != null)
        {
            for (int i = 0; i < app.ProductTypeContainers.Count; i++)
            {
                ProductTypeContainer? c = app.ProductTypeContainers[i];
                if (c == null) continue;
                ScrollRect? sr = WalkUpForScrollRect(c.transform);
                if (sr != null) return sr;
            }
        }

        if (app.FavouritesContainer != null)
        {
            ScrollRect? sr = WalkUpForScrollRect(app.FavouritesContainer.transform);
            if (sr != null) return sr;
        }

        return null;
    }

    private static ScrollRect? WalkUpForScrollRect(Transform? current)
    {
        while (current != null)
        {
            ScrollRect? sr = current.GetComponent<ScrollRect>();
            if (sr != null)
                return sr;
            current = current.parent;
        }
        return null;
    }
}

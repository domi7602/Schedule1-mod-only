using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.StationFramework;
using Il2CppScheduleOne.UI.Phone.ProductManagerApp;
using MelonLoader;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;
using GameProductManagerApp = Il2CppScheduleOne.UI.Phone.ProductManagerApp.ProductManagerApp;

namespace ProductAppQoL;

/// <summary>
/// Adds Rename and Delete actions for player-created mixes inside
/// <see cref="ProductAppDetailPanel"/>. Base products remain protected.
/// </summary>
internal sealed class ProductDetailActions
{
    private static readonly Color BarBgColor = new(0.11f, 0.13f, 0.16f, 0.90f);
    private static readonly Color RenameBtnColor = new(0.22f, 0.28f, 0.36f, 1f);
    private static readonly Color DeleteBtnColor = new(0.45f, 0.18f, 0.20f, 1f);
    private static readonly Color ConfirmBtnColor = new(0.68f, 0.16f, 0.18f, 1f);
    private static readonly Color SaveBtnColor = new(0.18f, 0.48f, 0.28f, 1f);
    private static readonly Color CancelBtnColor = new(0.25f, 0.27f, 0.31f, 1f);
    private static readonly Color InputBgColor = new(0.17f, 0.20f, 0.25f, 1f);

    private readonly ProductAppDetailPanel _panel;
    private GameObject? _barRoot;
    private GameObject? _idleRow;
    private GameObject? _renameRow;
    private GameObject? _deleteRow;

    private InputField? _renameInput;
    private ProductSearchInputFocus? _renameFocus;
    private Text? _statusText;

    private ProductDefinition? _currentProduct;

    public ProductDetailActions(ProductAppDetailPanel panel)
    {
        _panel = panel;
    }

    public void Initialize()
    {
        if (_barRoot != null || _panel.Container == null)
            return;

        Transform parent = _panel.Container.transform;

        _barRoot = new GameObject("ProductAppQoL_DetailActions");
        _barRoot.transform.SetParent(parent, false);
        var rootRt = _barRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = new Vector2(0f, 0f);
        rootRt.anchorMax = new Vector2(1f, 0f);
        rootRt.pivot = new Vector2(0.5f, 0f);
        rootRt.offsetMin = new Vector2(10f, 8f);
        rootRt.offsetMax = new Vector2(-10f, 44f);

        var bg = _barRoot.AddComponent<Image>();
        bg.color = BarBgColor;

        BuildIdleRow(_barRoot.transform);
        BuildRenameRow(_barRoot.transform);
        BuildDeleteRow(_barRoot.transform);

        ShowMode(ActionMode.Idle);
        _barRoot.SetActive(false);
    }

    public void OnActiveProductChanged(ProductDefinition? product)
    {
        if (_barRoot == null)
            Initialize();

        _currentProduct = product;
        ReleaseTyping();

        if (_barRoot == null)
            return;

        if (product == null || !IsPlayerCreatedMix(product))
        {
            _barRoot.SetActive(false);
            return;
        }

        _barRoot.SetActive(true);
        if (_statusText != null)
            _statusText.text = string.Empty;
        ShowMode(ActionMode.Idle);
    }

    public void ReleaseTyping()
    {
        _renameFocus?.ReleaseTyping();
        if (_renameInput != null && _renameInput.isFocused)
            _renameInput.DeactivateInputField();
    }

    private enum ActionMode { Idle, Rename, ConfirmDelete }

    private void ShowMode(ActionMode mode)
    {
        if (_idleRow != null) _idleRow.SetActive(mode == ActionMode.Idle);
        if (_renameRow != null) _renameRow.SetActive(mode == ActionMode.Rename);
        if (_deleteRow != null) _deleteRow.SetActive(mode == ActionMode.ConfirmDelete);

        if (mode != ActionMode.Rename)
            ReleaseTyping();
    }

    private void BuildIdleRow(Transform parent)
    {
        _idleRow = CreateRow("IdleRow", parent);

        var renameBtn = CreateButton("RenameButton", _idleRow.transform, "Rename", RenameBtnColor, 84f);
        ButtonUtils.AddListener(renameBtn, OnRenameClicked);

        var deleteBtn = CreateButton("DeleteButton", _idleRow.transform, "Delete", DeleteBtnColor, 84f);
        ButtonUtils.AddListener(deleteBtn, OnDeleteClicked);

        var statusGo = new GameObject("StatusLabel");
        statusGo.transform.SetParent(_idleRow.transform, false);
        statusGo.AddComponent<RectTransform>();
        var le = statusGo.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
        _statusText = statusGo.AddComponent<Text>();
        _statusText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        _statusText.fontSize = 12;
        _statusText.color = new Color(0.85f, 0.85f, 0.85f, 0.8f);
        _statusText.alignment = TextAnchor.MiddleRight;
        _statusText.raycastTarget = false;
    }

    private void BuildRenameRow(Transform parent)
    {
        _renameRow = CreateRow("RenameRow", parent);

        _renameInput = CreateInputField(_renameRow.transform);
        var inputLe = _renameInput.gameObject.AddComponent<LayoutElement>();
        inputLe.flexibleWidth = 1f;
        inputLe.minWidth = 90f;

        _renameFocus = _renameInput.gameObject.AddComponent<ProductSearchInputFocus>();
        _renameFocus.searchInput = _renameInput;

        var saveBtn = CreateButton("SaveRenameButton", _renameRow.transform, "Save", SaveBtnColor, 54f);
        ButtonUtils.AddListener(saveBtn, OnSaveRenameClicked);

        var cancelBtn = CreateButton("CancelRenameButton", _renameRow.transform, "×", CancelBtnColor, 28f);
        ButtonUtils.AddListener(cancelBtn, () => ShowMode(ActionMode.Idle));
    }

    private void BuildDeleteRow(Transform parent)
    {
        _deleteRow = CreateRow("DeleteRow", parent);

        var warnGo = new GameObject("WarningLabel");
        warnGo.transform.SetParent(_deleteRow.transform, false);
        warnGo.AddComponent<RectTransform>();
        var le = warnGo.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
        var warnText = warnGo.AddComponent<Text>();
        warnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        warnText.fontSize = 12;
        warnText.color = new Color(1f, 0.72f, 0.72f, 1f);
        warnText.alignment = TextAnchor.MiddleLeft;
        warnText.text = "Delete mix? (Owned items vanish)";
        warnText.raycastTarget = false;

        var confirmBtn = CreateButton("ConfirmDeleteButton", _deleteRow.transform, "Confirm", ConfirmBtnColor, 68f);
        ButtonUtils.AddListener(confirmBtn, OnConfirmDeleteClicked);

        var cancelBtn = CreateButton("CancelDeleteButton", _deleteRow.transform, "Cancel", CancelBtnColor, 58f);
        ButtonUtils.AddListener(cancelBtn, () => ShowMode(ActionMode.Idle));
    }

    private static GameObject CreateRow(string name, Transform parent)
    {
        var row = new GameObject(name);
        row.transform.SetParent(parent, false);
        var rt = row.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var layout = row.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(6, 6, 4, 4);
        layout.spacing = 6f;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
        return row;
    }

    private static Button CreateButton(string name, Transform parent, string label, Color bgColor, float width)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        var img = go.AddComponent<Image>();
        img.color = bgColor;
        var btn = go.AddComponent<Button>();
        btn.targetGraphic = img;

        var le = go.AddComponent<LayoutElement>();
        le.preferredWidth = width;
        le.minWidth = width;

        var textGo = new GameObject("Label");
        textGo.transform.SetParent(go.transform, false);
        var rt = textGo.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(4f, 0f);
        rt.offsetMax = new Vector2(-4f, 0f);

        var text = textGo.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 13;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.text = label;
        text.raycastTarget = false;
        return btn;
    }

    private static InputField CreateInputField(Transform parent)
    {
        var go = new GameObject("RenameInput");
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        var bg = go.AddComponent<Image>();
        bg.color = InputBgColor;

        var input = go.AddComponent<InputField>();
        input.targetGraphic = bg;
        input.lineType = InputField.LineType.SingleLine;
        input.characterLimit = 40;
        input.caretWidth = 2;
        input.caretColor = Color.white;
        input.selectionColor = new Color(0.18f, 0.65f, 0.98f, 0.45f);

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(go.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(6f, 2f);
        textRt.offsetMax = new Vector2(-6f, -2f);

        var text = textGo.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 13;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleLeft;
        text.supportRichText = false;
        text.raycastTarget = false;

        input.textComponent = text;
        return input;
    }

    private void OnRenameClicked()
    {
        if (_currentProduct == null || _renameInput == null)
            return;

        _renameInput.text = _currentProduct.Name ?? string.Empty;
        ShowMode(ActionMode.Rename);
        _renameInput.ActivateInputField();
    }

    private void OnSaveRenameClicked()
    {
        if (_currentProduct == null || _renameInput == null)
            return;

        string newName = (_renameInput.text ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(newName) || !ProductManager.IsValidProductName(newName))
        {
            if (_statusText != null) _statusText.text = "Invalid name";
            ShowMode(ActionMode.Idle);
            return;
        }

        try
        {
            _currentProduct.Name = newName;
            _currentProduct.HasChanged = true;

            ProductManager? pm = NetworkSingleton<ProductManager>.Instance;
            if (pm != null)
                pm.HasChanged = true;

            if (_panel.NameLabel != null)
                _panel.NameLabel.text = newName;

            ShowMode(ActionMode.Idle);
            if (_statusText != null) _statusText.text = "Renamed";

            Mod.RefreshAllApps();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] Rename failed: {ex.Message}");
        }
    }

    private void OnDeleteClicked()
    {
        if (_currentProduct == null)
            return;
        ShowMode(ActionMode.ConfirmDelete);
    }

    private void OnConfirmDeleteClicked()
    {
        ProductDefinition? target = _currentProduct;
        if (target == null || !IsPlayerCreatedMix(target))
            return;

        try
        {
            ProductManager? pm = NetworkSingleton<ProductManager>.Instance;
            if (pm != null)
            {
                string id = target.ID;
                try { pm.SetProductListed(id, false); } catch { }
                try { pm.SetProductFavourited(id, false); } catch { }

                ProductManager.DiscoveredProducts?.Remove(target);
                ProductManager.ListedProducts?.Remove(target);
                ProductManager.FavouritedProducts?.Remove(target);
                pm.createdProducts?.Remove(target);
                pm.AllProducts?.Remove(target);
                pm.ProductPrices?.Remove(target);

                if (pm.mixRecipes != null)
                {
                    for (int i = pm.mixRecipes.Count - 1; i >= 0; i--)
                    {
                        StationRecipe? recipe = pm.mixRecipes[i];
                        if (recipe?.Product?.Item == target)
                            pm.mixRecipes.RemoveAt(i);
                    }
                }

                pm.HasChanged = true;
            }

            RemoveEntriesFromApp(target);
            _currentProduct = null;
            _panel.SetActiveProduct(null);
            ShowMode(ActionMode.Idle);
            Mod.RefreshAllApps();

            MelonLogger.Msg($"[ProductAppQoL] Deleted player mix '{target.Name}' ({target.ID}).");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] Delete failed: {ex}");
        }
    }

    private static void RemoveEntriesFromApp(ProductDefinition target)
    {
        GameProductManagerApp? app = PlayerSingleton<GameProductManagerApp>.Instance;
        if (app == null) return;

        if (app.selectedEntry != null && app.selectedEntry.Definition == target)
            app.selectedEntry = null;

        if (app.entries != null)
        {
            for (int i = app.entries.Count - 1; i >= 0; i--)
            {
                ProductEntry? entry = app.entries[i];
                if (entry != null && entry.Definition == target)
                {
                    app.entries.RemoveAt(i);
                    UnityEngine.Object.Destroy(entry.gameObject);
                }
            }
        }

        if (app.favouriteEntries != null)
        {
            for (int i = app.favouriteEntries.Count - 1; i >= 0; i--)
            {
                ProductEntry? entry = app.favouriteEntries[i];
                if (entry != null && entry.Definition == target)
                {
                    app.favouriteEntries.RemoveAt(i);
                    UnityEngine.Object.Destroy(entry.gameObject);
                }
            }
        }
    }

    private static bool IsPlayerCreatedMix(ProductDefinition product)
    {
        ProductManager? pm = NetworkSingleton<ProductManager>.Instance;
        if (pm == null || pm.createdProducts == null)
            return false;

        for (int i = 0; i < pm.createdProducts.Count; i++)
        {
            if (pm.createdProducts[i] == product)
                return true;
        }
        return false;
    }
}

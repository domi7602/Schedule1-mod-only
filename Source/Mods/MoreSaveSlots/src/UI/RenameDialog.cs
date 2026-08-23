using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Persistence;
using Il2CppTMPro;
using MelonLoader;
using MoreSaveSlots.Services;
using UnityEngine;
using UnityEngine.UI;

namespace MoreSaveSlots.UI;

public static class RenameDialog
{
    public static bool IsOpen => _dialogRoot != null && _dialogRoot.activeSelf;

    private static GameObject? _dialogRoot;
    private static TextMeshProUGUI? _titleTmp;
    private static TextMeshProUGUI? _subTmp;
    private static TMP_InputField? _inputField;
    private static int _currentSlotIndex = -1;

    private static readonly List<Button> _slotSelectorButtons = new();
    private static readonly List<TextMeshProUGUI> _slotSelectorLabels = new();
    private static readonly List<Image> _slotSelectorImages = new();

    public static void OpenForHoveredOrSelected()
    {
        int slot = PaginationController.GetSelectedOrHoveredSlot();

        if (LoadManager.SaveGames != null)
        {
            if (slot >= 0 && slot < LoadManager.SaveGames.Length && LoadManager.SaveGames[slot] != null)
            {
                Open(slot);
                return;
            }

            // Fallback: If targeted slot is empty, check if there is another populated save on this page
            int pageStart = PaginationController.CurrentPage * PaginationController.SlotsPerPage;
            int pageEnd = Mathf.Min(pageStart + PaginationController.SlotsPerPage, LoadManager.SaveGames.Length);
            for (int s = pageStart; s < pageEnd; s++)
            {
                if (LoadManager.SaveGames[s] != null)
                {
                    PaginationController.SelectedLocalSlot = s - pageStart;
                    Open(s);
                    return;
                }
            }
        }

        Open(slot);
    }

    public static void Open(int slotIndex)
    {
        if (DeleteDialog.IsOpen) DeleteDialog.Close();
        EnsureDialogCreated();

        int targetSlot = slotIndex;
        if (LoadManager.SaveGames != null)
        {
            if (targetSlot < 0 || targetSlot >= LoadManager.SaveGames.Length || LoadManager.SaveGames[targetSlot] == null)
            {
                // If invalid or empty, pick the first populated slot on current page
                int pageStart = PaginationController.CurrentPage * PaginationController.SlotsPerPage;
                int pageEnd = Mathf.Min(pageStart + PaginationController.SlotsPerPage, LoadManager.SaveGames.Length);
                for (int s = pageStart; s < pageEnd; s++)
                {
                    if (LoadManager.SaveGames[s] != null)
                    {
                        targetSlot = s;
                        break;
                    }
                }
            }
        }

        SelectSlot(targetSlot);

        if (_dialogRoot != null)
        {
            _dialogRoot.SetActive(true);
        }
    }

    public static void SelectSlot(int slotIndex)
    {
        _currentSlotIndex = slotIndex;

        SaveInfo? saveInfo = null;
        if (LoadManager.SaveGames != null && slotIndex >= 0 && slotIndex < LoadManager.SaveGames.Length)
        {
            saveInfo = LoadManager.SaveGames[slotIndex];
        }

        if (_titleTmp != null)
        {
            _titleTmp.text = $"RENAME SAVE — SLOT {slotIndex + 1}";
        }

        if (_subTmp != null)
        {
            _subTmp.text = (saveInfo != null)
                ? $"Editing name for Slot {slotIndex + 1}:"
                : $"Slot {slotIndex + 1} (Empty):";
        }

        if (_inputField != null)
        {
            _inputField.text = saveInfo?.OrganisationName ?? "";
            _inputField.Select();
            _inputField.ActivateInputField();
        }

        RefreshSlotSelectorButtons();
    }

    private static void RefreshSlotSelectorButtons()
    {
        int page = PaginationController.CurrentPage;
        int slotsPerPage = PaginationController.SlotsPerPage;

        Color activeCol = new Color(0.18f, 0.55f, 0.32f, 1f);
        Color populatedCol = new Color(0.18f, 0.22f, 0.28f, 0.95f);
        Color emptyCol = new Color(0.10f, 0.11f, 0.14f, 0.40f);

        for (int i = 0; i < _slotSelectorButtons.Count; i++)
        {
            int actualSlot = page * slotsPerPage + i;
            var btn = _slotSelectorButtons[i];
            var lbl = _slotSelectorLabels[i];
            var img = _slotSelectorImages[i];

            if (btn == null || lbl == null || img == null) continue;

            bool hasSave = LoadManager.SaveGames != null
                && actualSlot >= 0
                && actualSlot < LoadManager.SaveGames.Length
                && LoadManager.SaveGames[actualSlot] != null;

            bool isSelected = (actualSlot == _currentSlotIndex);

            if (hasSave)
            {
                lbl.text = isSelected ? $"▶ SLOT {actualSlot + 1}" : $"SLOT {actualSlot + 1}";
                lbl.color = Color.white;
                btn.interactable = true;
                img.color = isSelected ? activeCol : populatedCol;
            }
            else
            {
                lbl.text = $"({actualSlot + 1})";
                lbl.color = new Color(0.5f, 0.5f, 0.55f, 0.6f);
                btn.interactable = false;
                img.color = emptyCol;
            }
        }
    }

    public static void Submit()
    {
        if (!IsOpen || _currentSlotIndex < 0) return;

        string newName = _inputField?.text ?? "";
        if (!string.IsNullOrWhiteSpace(newName))
        {
            SaveRenameService.RenameSave(_currentSlotIndex, newName.Trim());
        }

        Close();
    }

    public static void Close()
    {
        if (_dialogRoot != null)
        {
            UnityEngine.Object.Destroy(_dialogRoot);
            _dialogRoot = null;
        }
        _currentSlotIndex = -1;
    }

    public static void ResetForSceneReload()
    {
        if (_dialogRoot != null)
        {
            UnityEngine.Object.Destroy(_dialogRoot);
            _dialogRoot = null;
        }
        _currentSlotIndex = -1;
    }

    private static void EnsureDialogCreated()
    {
        if (_dialogRoot != null) return;

        // Find root canvas
        Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        if (canvas == null) return;

        // 1. Overlay Root
        _dialogRoot = new GameObject("MoreSaveSlots_RenameModal", Il2CppType.Of<RectTransform>());
        _dialogRoot.transform.SetParent(canvas.transform, false);

        RectTransform rootRt = _dialogRoot.GetComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;

        // Dim backdrop
        Image backdrop = _dialogRoot.AddComponent<Image>();
        backdrop.sprite = UIHelper.GetSolidSprite();
        backdrop.type = Image.Type.Simple;
        backdrop.color = new Color(0f, 0f, 0f, 0.75f);
        backdrop.raycastTarget = true;

        // 2. Dialog Window
        GameObject winObj = new GameObject("DialogWindow", Il2CppType.Of<RectTransform>());
        winObj.transform.SetParent(_dialogRoot.transform, false);

        RectTransform winRt = winObj.GetComponent<RectTransform>();
        winRt.anchorMin = new Vector2(0.5f, 0.5f);
        winRt.anchorMax = new Vector2(0.5f, 0.5f);
        winRt.pivot = new Vector2(0.5f, 0.5f);
        winRt.sizeDelta = new Vector2(500f, 280f);

        Image winBg = winObj.AddComponent<Image>();
        winBg.sprite = UIHelper.GetSolidSprite();
        winBg.type = Image.Type.Simple;
        winBg.color = new Color(0.10f, 0.11f, 0.14f, 0.98f);
        winBg.raycastTarget = true;

        // 3. Title
        _titleTmp = UIHelper.CreateTextMeshPro(winObj.transform, "Title", "RENAME SAVE", 16f, FontStyles.Bold, TextAlignmentOptions.Center, new Color(0.95f, 0.95f, 0.98f, 1f));
        RectTransform titleRt = _titleTmp.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0f, 1f);
        titleRt.anchorMax = new Vector2(1f, 1f);
        titleRt.pivot = new Vector2(0.5f, 1f);
        titleRt.sizeDelta = new Vector2(0f, 32f);
        titleRt.anchoredPosition = new Vector2(0f, -12f);

        // 4. Slot Selector Bar (Horizontal list of slots on current page)
        GameObject selectorObj = new GameObject("SlotSelectorBar", Il2CppType.Of<RectTransform>());
        selectorObj.transform.SetParent(winObj.transform, false);

        RectTransform selectorRt = selectorObj.GetComponent<RectTransform>();
        selectorRt.anchorMin = new Vector2(0.5f, 1f);
        selectorRt.anchorMax = new Vector2(0.5f, 1f);
        selectorRt.pivot = new Vector2(0.5f, 1f);
        selectorRt.sizeDelta = new Vector2(460f, 32f);
        selectorRt.anchoredPosition = new Vector2(0f, -46f);

        HorizontalLayoutGroup selectorLayout = selectorObj.AddComponent<HorizontalLayoutGroup>();
        selectorLayout.childAlignment = TextAnchor.MiddleCenter;
        selectorLayout.spacing = 8f;
        selectorLayout.childControlWidth = false;
        selectorLayout.childControlHeight = false;

        _slotSelectorButtons.Clear();
        _slotSelectorLabels.Clear();
        _slotSelectorImages.Clear();

        for (int i = 0; i < 5; i++)
        {
            int capturedLocalIndex = i;
            Button slotBtn = UIHelper.CreateButton(
                selectorObj.transform,
                $"SlotTab_{i}",
                $"SLOT {i + 1}",
                84f,
                30f,
                new Color(0.18f, 0.22f, 0.28f, 0.95f),
                new Color(0.26f, 0.32f, 0.40f, 1f),
                new Color(0.10f, 0.12f, 0.16f, 1f),
                new Color(0.08f, 0.09f, 0.11f, 0.40f),
                () =>
                {
                    int actual = PaginationController.CurrentPage * 5 + capturedLocalIndex;
                    SelectSlot(actual);
                },
                out TextMeshProUGUI slotLbl
            );

            _slotSelectorButtons.Add(slotBtn);
            _slotSelectorLabels.Add(slotLbl);
            _slotSelectorImages.Add(slotBtn.GetComponent<Image>());
        }

        // 5. Subtitle / Prompt
        _subTmp = UIHelper.CreateTextMeshPro(winObj.transform, "Subtitle", "Enter new organization name:", 12f, FontStyles.Normal, TextAlignmentOptions.Center, new Color(0.68f, 0.68f, 0.72f, 1f));
        RectTransform subRt = _subTmp.GetComponent<RectTransform>();
        subRt.anchorMin = new Vector2(0f, 1f);
        subRt.anchorMax = new Vector2(1f, 1f);
        subRt.pivot = new Vector2(0.5f, 1f);
        subRt.sizeDelta = new Vector2(0f, 22f);
        subRt.anchoredPosition = new Vector2(0f, -86f);

        // 6. Input Field
        GameObject inputObj = new GameObject("InputField", Il2CppType.Of<RectTransform>());
        inputObj.transform.SetParent(winObj.transform, false);
        RectTransform inputRt = inputObj.GetComponent<RectTransform>();
        inputRt.anchorMin = new Vector2(0.5f, 1f);
        inputRt.anchorMax = new Vector2(0.5f, 1f);
        inputRt.pivot = new Vector2(0.5f, 1f);
        inputRt.sizeDelta = new Vector2(430f, 38f);
        inputRt.anchoredPosition = new Vector2(0f, -114f);

        Image inputBg = inputObj.AddComponent<Image>();
        inputBg.sprite = UIHelper.GetSolidSprite();
        inputBg.type = Image.Type.Simple;
        inputBg.color = new Color(0.06f, 0.07f, 0.09f, 1f);
        inputBg.raycastTarget = true;

        // Text Area inside Input
        var inputText = UIHelper.CreateTextMeshPro(inputObj.transform, "Text", "", 14f, FontStyles.Normal, TextAlignmentOptions.Left, Color.white);
        RectTransform textRt = inputText.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(12f, 0f);
        textRt.offsetMax = new Vector2(-12f, 0f);

        _inputField = inputObj.AddComponent<TMP_InputField>();
        _inputField.textComponent = inputText;
        _inputField.fontAsset = UIHelper.GetFont(canvas.transform);
        _inputField.characterLimit = 32;

        // 7. Action Buttons Bar
        GameObject btnBar = new GameObject("ButtonsBar", Il2CppType.Of<RectTransform>());
        btnBar.transform.SetParent(winObj.transform, false);
        RectTransform barRt = btnBar.GetComponent<RectTransform>();
        barRt.anchorMin = new Vector2(0f, 0f);
        barRt.anchorMax = new Vector2(1f, 0f);
        barRt.pivot = new Vector2(0.5f, 0f);
        barRt.sizeDelta = new Vector2(0f, 48f);
        barRt.anchoredPosition = new Vector2(0f, 14f);

        HorizontalLayoutGroup layout = btnBar.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 20f;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        // Cancel Button
        UIHelper.CreateButton(
            btnBar.transform,
            "Btn_Cancel",
            "CANCEL",
            140f,
            36f,
            new Color(0.18f, 0.20f, 0.25f, 0.95f),
            new Color(0.28f, 0.32f, 0.38f, 1f),
            new Color(0.10f, 0.11f, 0.14f, 1f),
            new Color(0.10f, 0.10f, 0.12f, 0.4f),
            Close,
            out _
        );

        // Save Button
        UIHelper.CreateButton(
            btnBar.transform,
            "Btn_Submit",
            "SAVE",
            140f,
            36f,
            new Color(0.14f, 0.42f, 0.24f, 0.95f),
            new Color(0.20f, 0.58f, 0.34f, 1f),
            new Color(0.08f, 0.25f, 0.14f, 1f),
            new Color(0.10f, 0.10f, 0.12f, 0.4f),
            Submit,
            out _
        );
    }
}

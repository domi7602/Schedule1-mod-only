using System;
using HarmonyLib;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.UI.MainMenu;
using Il2CppTMPro;
using MelonLoader;
using MoreSaveSlots.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace MoreSaveSlots.Patches;

[HarmonyPatch(typeof(SaveDisplay))]
public static class SaveDisplay_Patches
{
    [HarmonyPatch(nameof(SaveDisplay.Awake))]
    [HarmonyPrefix]
    public static bool Awake_Prefix(SaveDisplay __instance)
    {
        // Fix 2026-09-11: Vanilla-Awake loopt 0..SAVE_SLOT_COUNT-1 (25) und ruft
        // SetDisplayedSave(i) auf. Slots.Length ist aber 5 -> IndexOutOfRange ab i=5.
        // Darum Vanilla-Awake skippen und paginiert initialisieren (Seite 0).
        if (__instance == null || __instance.Slots == null || __instance.Slots.Length == 0)
        {
            return true;
        }

        try
        {
            int page = PaginationController.CurrentPage;
            int slotsPerPage = PaginationController.SlotsPerPage;

            for (int i = 0; i < __instance.Slots.Length; i++)
            {
                try
                {
                    int actualIndex = page * slotsPerPage + i;
                    SaveInfo? info = null;

                    if (LoadManager.SaveGames != null && actualIndex >= 0 && actualIndex < LoadManager.SaveGames.Length)
                    {
                        info = LoadManager.SaveGames[actualIndex];
                    }

                    __instance.SetDisplayedSave(i, info);
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[MoreSaveSlots] Awake: slot {i} SetDisplayedSave threw, forcing visible anyway: {ex.Message}");
                }

                try
                {
                    var slotRt = __instance.Slots[i];
                    if (slotRt != null)
                        ForceSlotVisible(slotRt, page * slotsPerPage + i + 1);
                }
                catch { }
            }

            PaginationController.UpdateUILabel();
            MelonLogger.Msg($"[MoreSaveSlots] SaveDisplay.Awake intercepted: page {page + 1}, {__instance.Slots.Length} slots shown.");
            return false;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error in SaveDisplay.Awake prefix: {ex}");
            return true;
        }
    }

    [HarmonyPatch(nameof(SaveDisplay.Awake))]
    [HarmonyPostfix]
    public static void Awake_Postfix(SaveDisplay __instance)
    {
        try
        {
            PaginationController.EnsurePaginationBar(__instance);
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error in SaveDisplay.Awake postfix: {ex}");
        }
    }

    [HarmonyPatch(nameof(SaveDisplay.Refresh))]
    [HarmonyPrefix]
    public static bool Refresh_Prefix(SaveDisplay __instance)
    {
        if (__instance == null || __instance.Slots == null || __instance.Slots.Length == 0)
        {
            return true;
        }

        try
        {
            try
            {
                PaginationController.EnsurePaginationBar(__instance);
            }
            catch (Exception exBar)
            {
                MelonLogger.Warning("[MoreSaveSlots] Refresh: EnsurePaginationBar threw, continuing with slot refresh: " + exBar.Message);
            }

            int page = PaginationController.CurrentPage;
            int slotsPerPage = PaginationController.SlotsPerPage;

            // Diag 2026-09-11: ein Zeile pro Refresh — zeigt ob/wann Refresh läuft und was im Array steht.
            try
            {
                string arrInfo = LoadManager.SaveGames == null ? "null" : LoadManager.SaveGames.Length.ToString();
                string firstName = "none";
                int firstIdx = page * slotsPerPage;
                if (LoadManager.SaveGames != null && firstIdx >= 0 && firstIdx < LoadManager.SaveGames.Length
                    && LoadManager.SaveGames[firstIdx] != null)
                {
                    firstName = LoadManager.SaveGames[firstIdx].OrganisationName ?? "unnamed";
                }
                MelonLogger.Msg($"[MoreSaveSlots] Refresh: page {page + 1}, SaveGames={arrInfo}, first='{firstName}'");
            }
            catch { }

            int occupied = 0;
            int empty = 0;
            for (int i = 0; i < __instance.Slots.Length; i++)
            {
                int actualIndex = page * slotsPerPage + i;
                int slotNumber = actualIndex + 1;
                SaveInfo? info = null;

                if (LoadManager.SaveGames != null && actualIndex >= 0 && actualIndex < LoadManager.SaveGames.Length)
                {
                    try { info = LoadManager.SaveGames[actualIndex]; }
                    catch (Exception exRead)
                    {
                        MelonLogger.Warning($"[MoreSaveSlots] Refresh: SaveGames[{actualIndex}] read threw: {exRead.Message}");
                        info = null;
                    }
                }

                if (info != null) occupied++; else empty++;

                // Index-swap guard: vanilla may render SaveGames[localIndex] instead of the
                // passed info. Temporarily place the global info at the local index so both
                // vanilla behaviors (param vs. array lookup) paint the correct save.
                bool swapped = false;
                SaveInfo? stashed = null;
                try
                {
                    if (info != null && LoadManager.SaveGames != null
                        && i >= 0 && i < LoadManager.SaveGames.Length)
                    {
                        try
                        {
                            stashed = LoadManager.SaveGames[i];
                            LoadManager.SaveGames[i] = info;
                            swapped = true;
                        }
                        catch { swapped = false; }
                    }
                    __instance.SetDisplayedSave(i, info);
                }
                catch (Exception exSet)
                {
                    MelonLogger.Warning($"[MoreSaveSlots] Refresh: slot {slotNumber} SetDisplayedSave threw, forcing visible anyway: {exSet.Message}");
                }
                finally
                {
                    if (swapped && LoadManager.SaveGames != null
                        && i >= 0 && i < LoadManager.SaveGames.Length)
                    {
                        try { LoadManager.SaveGames[i] = stashed; } catch { }
                    }
                }

                var slotRt = (RectTransform?)null;
                try { slotRt = __instance.Slots[i]; } catch { }
                if (slotRt != null)
                {
                    // Fix invisibility: vanilla may hide empty slots or hide after pagination — force visible.
                    // Isolated per-slot so one bad card can never blank the remaining slots.
                    try
                    {
                        ForceSlotVisible(slotRt, slotNumber, info != null);
                    }
                    catch (Exception exVis)
                    {
                        MelonLogger.Warning($"[MoreSaveSlots] Refresh: slot {slotNumber} ForceSlotVisible threw: {exVis.Message}");
                    }

                    // Empty slots: vanilla leaves stale card content behind — hide the
                    // details block and show our own placeholder instead.
                    try
                    {
                        UpdateEmptyState(slotRt, slotNumber, info != null);
                    }
                    catch (Exception exEmpty)
                    {
                        MelonLogger.Warning($"[MoreSaveSlots] Refresh: slot {slotNumber} UpdateEmptyState threw: {exEmpty.Message}");
                    }

                    try { UpdateSlotNumberText(slotRt, slotNumber); } catch { }

                    // Update Import/Export buttons on ImportScreen (isolated — never aborts slot loop)
                    var exportBtn = (SaveExportButton?)null;
                    var importBtn = (SaveImportButton?)null;
                    try
                    {
                        exportBtn = slotRt.GetComponentInChildren<SaveExportButton>(true);
                        if (exportBtn != null)
                        {
                            exportBtn.SaveSlotIndex = actualIndex;
                        }

                        importBtn = slotRt.GetComponentInChildren<SaveImportButton>(true);
                        if (importBtn != null)
                        {
                            importBtn.SaveSlotIndex = actualIndex;
                        }
                    }
                    catch { }

                    try
                    {
                        EnsureInlineButtons(slotRt, info, actualIndex, exportBtn != null);
                    }
                    catch (Exception exBtn)
                    {
                        MelonLogger.Warning($"[MoreSaveSlots] Refresh: slot {slotNumber} inline buttons threw, card stays visible: {exBtn.Message}");
                    }
                }
                else
                {
                    try { UpdateSlotNumberText(slotRt, slotNumber); } catch { }
                }
            }

            try { PaginationController.UpdateUILabel(); }
            catch (Exception exLabel)
            {
                MelonLogger.Warning("[MoreSaveSlots] Refresh: UpdateUILabel threw: " + exLabel.Message);
            }
            MelonLogger.Msg($"[MoreSaveSlots] Refresh done: page {page + 1} occupied={occupied} empty={empty}");
            return false; // Skip vanilla 0..4 loop
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error in SaveDisplay.Refresh prefix: {ex}");
            return true;
        }
    }

    private static void EnsureInlineButtons(RectTransform slotRt, SaveInfo? info, int actualIndex, bool hasExportBtn)
    {
                    // Inline Rename + Delete buttons on populated slot cards
                    Transform? container = slotRt.Find("Container") ?? slotRt;
                    if (container != null)
                    {
                        // Rename button
                        string inlineBtnName = "MoreSaveSlots_InlineRenameBtn";
                        Transform existingBtn = container.Find(inlineBtnName);

                        if (info != null)
                        {
                            int captureSlot = actualIndex;
                            if (existingBtn == null)
                            {
                                Button slotRenameBtn = UIHelper.CreateButton(
                                    container,
                                    inlineBtnName,
                                    "EDIT",
                                    44f,
                                    32f,
                                    new Color(0.12f, 0.32f, 0.20f, 0.90f),
                                    new Color(0.18f, 0.48f, 0.30f, 1f),
                                    new Color(0.08f, 0.20f, 0.12f, 1f),
                                    new Color(0.1f, 0.1f, 0.1f, 0.4f),
                                    () => RenameDialog.Open(captureSlot),
                                    out _
                                );

                                RectTransform btnRt = slotRenameBtn.GetComponent<RectTransform>();
                                btnRt.anchorMin = new Vector2(1f, 0.5f);
                                btnRt.anchorMax = new Vector2(1f, 0.5f);
                                btnRt.pivot = new Vector2(1f, 0.5f);
                                float xOffset = hasExportBtn ? -75f : -12f;
                                btnRt.anchoredPosition = new Vector2(xOffset, 0f);
                                slotRenameBtn.transform.SetAsLastSibling();
                            }
                            else
                            {
                                existingBtn.gameObject.SetActive(true);
                                existingBtn.SetAsLastSibling();
                                var btn = existingBtn.GetComponent<Button>();
                                if (btn != null)
                                {
                                    btn.onClick.RemoveAllListeners();
                                    EventHelper.AddListener(() => RenameDialog.Open(captureSlot), btn.onClick);
                                }
                            }
                        }
                        else if (existingBtn != null)
                        {
                            existingBtn.gameObject.SetActive(false);
                        }

                        // Delete button (DEL) — mirrors rename, left of rename
                        string inlineDeleteName = "MoreSaveSlots_InlineDeleteBtn";
                        Transform existingDeleteBtn = container.Find(inlineDeleteName);

                        if (info != null)
                        {
                            int captureDeleteSlot = actualIndex;
                            if (existingDeleteBtn == null)
                            {
                                Button slotDeleteBtn = UIHelper.CreateButton(
                                    container,
                                    inlineDeleteName,
                                    "DEL",
                                    44f,
                                    32f,
                                    new Color(0.45f, 0.12f, 0.12f, 0.90f),
                                    new Color(0.65f, 0.18f, 0.18f, 1f),
                                    new Color(0.28f, 0.08f, 0.08f, 1f),
                                    new Color(0.1f, 0.1f, 0.1f, 0.4f),
                                    () => DeleteDialog.Open(captureDeleteSlot),
                                    out _
                                );

                                RectTransform delRt = slotDeleteBtn.GetComponent<RectTransform>();
                                delRt.anchorMin = new Vector2(1f, 0.5f);
                                delRt.anchorMax = new Vector2(1f, 0.5f);
                                delRt.pivot = new Vector2(1f, 0.5f);
                                float xOffsetDel = hasExportBtn ? -115f : -52f;
                                delRt.anchoredPosition = new Vector2(xOffsetDel, 0f);
                                slotDeleteBtn.transform.SetAsLastSibling();
                                // Keep rename on top — reorder: delete first, rename last
                                if (existingBtn != null) existingBtn.SetAsLastSibling();
                            }
                            else
                            {
                                existingDeleteBtn.gameObject.SetActive(true);
                                existingDeleteBtn.SetAsLastSibling();
                                if (existingBtn != null) existingBtn.SetAsLastSibling();
                                var btn = existingDeleteBtn.GetComponent<Button>();
                                if (btn != null)
                                {
                                    btn.onClick.RemoveAllListeners();
                                    EventHelper.AddListener(() => DeleteDialog.Open(captureDeleteSlot), btn.onClick);
                                }
                            }
                        }
                        else if (existingDeleteBtn != null)
                        {
                            existingDeleteBtn.gameObject.SetActive(false);
                        }
                    }
    }

    internal static void ForceSlotVisible(RectTransform slotRt, int slotNumber, bool? hasSave = null)
    {
        if (slotRt == null) return;
        try { if (!slotRt.gameObject.activeSelf) slotRt.gameObject.SetActive(true); } catch { }
        try { if (slotRt.localScale == Vector3.zero) slotRt.localScale = Vector3.one; } catch { }
        try
        {
            var cg = slotRt.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                if (cg.alpha < 0.9f) cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }
        catch { }
        try
        {
            var parentCg = slotRt.parent?.GetComponent<CanvasGroup>();
            if (parentCg != null && parentCg.alpha < 0.9f) parentCg.alpha = 1f;
        }
        catch { }

        Transform? container = null;
        try
        {
            container = slotRt.Find("Container");
            if (container != null)
            {
                if (!container.gameObject.activeSelf) container.gameObject.SetActive(true);
                var crt = container.GetComponent<RectTransform>();
                if (crt != null && crt.localScale == Vector3.zero) crt.localScale = Vector3.one;
                var ccg = container.GetComponent<CanvasGroup>();
                if (ccg != null && ccg.alpha < 0.9f) ccg.alpha = 1f;
            }
        }
        catch { }
        try
        {
            var infoT = slotRt.Find("Container/Info") ?? slotRt.Find("Info");
            if (infoT != null && !infoT.gameObject.activeSelf) infoT.gameObject.SetActive(true);
        }
        catch { }

        // All TMP / legacy Text / Image children: re-enable + restore alpha so a
        // vanilla-hidden empty card becomes visible again (parent button stays clickable).
        try
        {
            var tmps = slotRt.GetComponentsInChildren<TextMeshProUGUI>(true);
            if (tmps != null)
            {
                for (int i = 0; i < tmps.Length; i++)
                {
                    TextMeshProUGUI? tmp = null;
                    try { tmp = tmps[i]; } catch { continue; }
                    if (tmp == null) continue;
                    try { if (tmp.Pointer == IntPtr.Zero || tmp.WasCollected) continue; } catch { continue; }
                    try
                    {
                        if (!tmp.gameObject.activeSelf) tmp.gameObject.SetActive(true);
                        if (!tmp.enabled) tmp.enabled = true;
                        if (tmp.alpha < 0.9f) tmp.alpha = 1f;
                        var c = tmp.color;
                        if (c.a < 0.9f) { c.a = 1f; tmp.color = c; }
                        tmp.raycastTarget = false;
                        try { tmp.SetVerticesDirty(); } catch { }
                    }
                    catch { }
                }
            }
        }
        catch { }
        try
        {
            var texts = slotRt.GetComponentsInChildren<Text>(true);
            if (texts != null)
            {
                for (int j = 0; j < texts.Length; j++)
                {
                    Text? t = null;
                    try { t = texts[j]; } catch { continue; }
                    if (t == null) continue;
                    try { if (t.Pointer == IntPtr.Zero || t.WasCollected) continue; } catch { continue; }
                    try
                    {
                        if (!t.gameObject.activeSelf) t.gameObject.SetActive(true);
                        if (!t.enabled) t.enabled = true;
                        var c2 = t.color;
                        if (c2.a < 0.9f) { c2.a = 1f; t.color = c2; }
                    }
                    catch { }
                }
            }
        }
        catch { }
        try
        {
            var images = slotRt.GetComponentsInChildren<Image>(true);
            if (images != null)
            {
                for (int k = 0; k < images.Length; k++)
                {
                    Image? img = null;
                    try { img = images[k]; } catch { continue; }
                    if (img == null) continue;
                    try { if (img.Pointer == IntPtr.Zero || img.WasCollected) continue; } catch { continue; }
                    try { if (!img.enabled) img.enabled = true; } catch { }
                }
            }
        }
        catch { }
    }

    private const string EmptyLabelName = "MoreSaveSlots_EmptyLabel";

    private static void UpdateEmptyState(RectTransform slotRt, int slotNumber, bool hasSave)
    {
        Transform scope = slotRt.Find("Container") ?? (Transform)slotRt;
        if (scope == null) return;
        Transform? infoT = slotRt.Find("Container/Info") ?? slotRt.Find("Info");
        Transform? emptyT = null;
        try { emptyT = scope.Find(EmptyLabelName); } catch { }

        if (hasSave)
        {
            try { if (emptyT != null && emptyT.gameObject.activeSelf) emptyT.gameObject.SetActive(false); } catch { }
            try { if (infoT != null && !infoT.gameObject.activeSelf) infoT.gameObject.SetActive(true); } catch { }
            return;
        }

        try { if (infoT != null && infoT.gameObject.activeSelf) infoT.gameObject.SetActive(false); } catch { }

        TextMeshProUGUI? label = null;
        if (emptyT == null)
        {
            try
            {
                label = UIHelper.CreateTextMeshPro(
                    scope, EmptyLabelName, $"SLOT {slotNumber} — EMPTY",
                    15f, FontStyles.Bold, TextAlignmentOptions.Center,
                    new Color(0.55f, 0.58f, 0.62f, 1f));
                var lrt = label.GetComponent<RectTransform>();
                lrt.anchorMin = Vector2.zero;
                lrt.anchorMax = Vector2.one;
                lrt.offsetMin = Vector2.zero;
                lrt.offsetMax = Vector2.zero;
                LayoutElement? le = null;
                try { le = label.gameObject.GetComponent<LayoutElement>(); } catch { }
                if (le == null)
                {
                    try { le = label.gameObject.AddComponent<LayoutElement>(); } catch { }
                }
                if (le != null)
                {
                    try { le.preferredHeight = 36f; le.flexibleWidth = 1f; } catch { }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[MoreSaveSlots] UpdateEmptyState: slot {slotNumber} label create threw: {ex.Message}");
                return;
            }
        }
        else
        {
            try { label = emptyT.GetComponent<TextMeshProUGUI>(); } catch { }
            if (label == null) return;
            try
            {
                if (label.Pointer == IntPtr.Zero || label.WasCollected)
                {
                    try { UnityEngine.Object.Destroy(emptyT.gameObject); } catch { }
                    return;
                }
            }
            catch { return; }
            try
            {
                label.text = $"SLOT {slotNumber} — EMPTY";
                if (!emptyT.gameObject.activeSelf) emptyT.gameObject.SetActive(true);
                if (!label.enabled) label.enabled = true;
                if (label.alpha < 0.9f) label.alpha = 1f;
                emptyT.SetAsLastSibling();
            }
            catch { }
        }
    }

    private static void UpdateSlotNumberText(RectTransform slotRt, int slotNumber)
    {
        Transform? infoTransform = slotRt.Find("Container/Info") ?? slotRt.Find("Info");

        // 1. TextMeshProUGUI (Target only elements outside the Info container) — IL2CPP-safe indexed loop
        var tmpComponents = slotRt.GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int ti = 0; ti < tmpComponents.Length; ti++)
        {
            var tmp = tmpComponents[ti];
            if (tmp == null) continue;
            try { if (tmp.Pointer == IntPtr.Zero || tmp.WasCollected) continue; } catch { continue; }
            if (string.IsNullOrWhiteSpace(tmp.text)) continue;
            if (infoTransform != null && tmp.transform.IsChildOf(infoTransform)) continue;
            string t = tmp.text.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(t, @"^SLOT\s+\d+$"))
            {
                tmp.text = $"SLOT {slotNumber}";
            }
        }

        // 2. Legacy UI.Text — IL2CPP-safe indexed loop
        var textComponents = slotRt.GetComponentsInChildren<Text>(true);
        for (int txi = 0; txi < textComponents.Length; txi++)
        {
            var txt = textComponents[txi];
            if (txt == null) continue;
            try { if (txt.Pointer == IntPtr.Zero || txt.WasCollected) continue; } catch { continue; }
            if (string.IsNullOrWhiteSpace(txt.text)) continue;
            if (infoTransform != null && txt.transform.IsChildOf(infoTransform)) continue;
            string t = txt.text.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(t, @"^SLOT\s+\d+$"))
            {
                txt.text = $"SLOT {slotNumber}";
            }
        }
    }
}

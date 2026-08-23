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
            PaginationController.EnsurePaginationBar(__instance);

            int page = PaginationController.CurrentPage;
            int slotsPerPage = PaginationController.SlotsPerPage;

            for (int i = 0; i < __instance.Slots.Length; i++)
            {
                int actualIndex = page * slotsPerPage + i;
                int slotNumber = actualIndex + 1;
                SaveInfo? info = null;

                if (LoadManager.SaveGames != null && actualIndex >= 0 && actualIndex < LoadManager.SaveGames.Length)
                {
                    info = LoadManager.SaveGames[actualIndex];
                }

                __instance.SetDisplayedSave(i, info);

                var slotRt = __instance.Slots[i];
                if (slotRt != null)
                {
                    UpdateSlotNumberText(slotRt, slotNumber);

                    // Update Import/Export buttons on ImportScreen
                    var exportBtn = slotRt.GetComponentInChildren<SaveExportButton>(true);
                    if (exportBtn != null)
                    {
                        exportBtn.SaveSlotIndex = actualIndex;
                    }

                    var importBtn = slotRt.GetComponentInChildren<SaveImportButton>(true);
                    if (importBtn != null)
                    {
                        importBtn.SaveSlotIndex = actualIndex;
                    }

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
                                    "✏️",
                                    32f,
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
                                float xOffset = (exportBtn != null) ? -75f : -12f;
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

                        // Delete button (🗑️) — mirrors rename, 40px left of rename
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
                                    "🗑️",
                                    32f,
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
                                float xOffsetDel = (exportBtn != null) ? -115f : -52f;
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
            }

            PaginationController.UpdateUILabel();
            return false; // Skip vanilla 0..4 loop
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error in SaveDisplay.Refresh prefix: {ex}");
            return true;
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

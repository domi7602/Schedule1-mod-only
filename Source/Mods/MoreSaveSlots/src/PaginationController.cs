extern alias il2cpp;

using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.UI.MainMenu;
using Il2CppTMPro;
using MelonLoader;
using S1API.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoreSaveSlots.UI;

public static class PaginationController
{
    public static int CurrentPage { get; set; } = 0;
    public static int TotalSlots { get; set; } = 25;
    public const int SlotsPerPage = 5;
    public static int TotalPages => Mathf.Max(1, Mathf.CeilToInt((float)TotalSlots / SlotsPerPage));

    public static int HoveredLocalSlot { get; set; } = -1;
    public static int SelectedLocalSlot { get; set; } = 0;

    private static readonly List<TextMeshProUGUI> ActivePageLabels = new();
    private static readonly List<Button> ActivePrevButtons = new();
    private static readonly List<Button> ActiveNextButtons = new();

    public static int GetSelectedOrHoveredSlot()
    {
        int local = (HoveredLocalSlot >= 0 && HoveredLocalSlot < SlotsPerPage)
            ? HoveredLocalSlot
            : Mathf.Clamp(SelectedLocalSlot, 0, SlotsPerPage - 1);

        return CurrentPage * SlotsPerPage + local;
    }

    public static void NextPage()
    {
        if (CurrentPage < TotalPages - 1)
        {
            CurrentPage++;
            SelectedLocalSlot = 0;
            HoveredLocalSlot = -1;
            MelonLogger.Msg($"[MoreSaveSlots] Navigated to Page {CurrentPage + 1}/{TotalPages} (Slots {CurrentPage * SlotsPerPage + 1}–{Mathf.Min((CurrentPage + 1) * SlotsPerPage, TotalSlots)})");
            RefreshActiveScreen();
        }
    }

    public static void PreviousPage()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            SelectedLocalSlot = 0;
            HoveredLocalSlot = -1;
            MelonLogger.Msg($"[MoreSaveSlots] Navigated to Page {CurrentPage + 1}/{TotalPages} (Slots {CurrentPage * SlotsPerPage + 1}–{Mathf.Min((CurrentPage + 1) * SlotsPerPage, TotalSlots)})");
            RefreshActiveScreen();
        }
    }

    public static void RefreshActiveScreen()
    {
        try
        {
            // Gatekeeper-fix 2026-08-29: FindObjectsOfType<T>() is [Obsolete] in Unity 2022.3+ (CS0618).
            // Migrated to FindObjectsByType with explicit FindObjectsSortMode.None (no allocation, faster).
            var saveDisplays = UnityEngine.Object.FindObjectsByType<SaveDisplay>(FindObjectsSortMode.None);
            if (saveDisplays != null)
            {
                for (int i = 0; i < saveDisplays.Length; i++)
                {
                    var sd = saveDisplays[i];
                    if (sd == null || sd.Pointer == IntPtr.Zero || sd.WasCollected) continue;
                    if (sd.gameObject == null || sd.gameObject.Pointer == IntPtr.Zero) continue;
                    if (!sd.gameObject.activeInHierarchy) continue;
                    sd.Refresh();
                }
            }
            UpdateUILabel();
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error refreshing screen: {ex}");
        }
    }

    public static void EnsurePaginationBar(SaveDisplay saveDisplay)
    {
        if (saveDisplay == null || !saveDisplay.gameObject.activeInHierarchy) return;

        Transform parentContainer = (saveDisplay.Slots != null && saveDisplay.Slots.Length > 0 && saveDisplay.Slots[0] != null)
            ? saveDisplay.Slots[0].parent
            : saveDisplay.transform;

        string barName = "MoreSaveSlots_PaginationBar";
        Transform existing = parentContainer.Find(barName);
        if (existing != null)
        {
            existing.gameObject.SetActive(true);
            UpdateUILabel();
            return;
        }

        // Attach hover tracking to each slot card
        if (saveDisplay.Slots != null)
        {
            for (int i = 0; i < saveDisplay.Slots.Length; i++)
            {
                var slotRt = saveDisplay.Slots[i];
                if (slotRt != null)
                {
                    AttachHoverTracker(slotRt.gameObject, i);
                }
            }
        }

        MelonLogger.Msg($"[MoreSaveSlots] Creating Navigation Bar on '{parentContainer.name}'...");

        // Create Navigation Bar GameObject
        GameObject barObj = new GameObject(barName, Il2CppType.Of<RectTransform>());
        barObj.transform.SetParent(parentContainer, false);

        RectTransform barRt = barObj.GetComponent<RectTransform>();
        barRt.sizeDelta = new Vector2(720f, 44f);
        barRt.pivot = new Vector2(0.5f, 0.5f);

        // Add LayoutElement so VerticalLayoutGroup allocates clean space
        LayoutElement layoutElement = barObj.AddComponent<LayoutElement>();
        layoutElement.minHeight = 44f;
        layoutElement.preferredHeight = 44f;
        layoutElement.flexibleWidth = 1f;

        // Put as last sibling (below slot 5)
        barRt.SetAsLastSibling();

        // Background
        Image bg = barObj.AddComponent<Image>();
        bg.sprite = UIHelper.GetSolidSprite();
        bg.type = Image.Type.Simple;
        bg.color = new Color(0.07f, 0.08f, 0.11f, 0.95f);
        bg.raycastTarget = false;

        // Horizontal Layout Group
        HorizontalLayoutGroup layout = barObj.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 10f;
        layout.padding = new RectOffset(12, 12, 5, 5);
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        Color btnNormal = new Color(0.15f, 0.18f, 0.24f, 0.95f);
        Color btnHover = new Color(0.24f, 0.28f, 0.38f, 1f);
        Color btnPressed = new Color(0.09f, 0.11f, 0.15f, 1f);
        Color btnDisabled = new Color(0.08f, 0.09f, 0.11f, 0.40f);

        // 1. Prev Button
        Button prevBtn = UIHelper.CreateButton(
            barObj.transform,
            "Btn_Prev",
            "◄ PREV",
            95f,
            32f,
            btnNormal,
            btnHover,
            btnPressed,
            btnDisabled,
            PreviousPage,
            out _
        );
        ActivePrevButtons.Add(prevBtn);

        // 2. Page Indicator Text
        var pageTmp = UIHelper.CreateTextMeshPro(
            barObj.transform,
            "PageText",
            GetPageLabelString(),
            13f,
            FontStyles.Bold,
            TextAlignmentOptions.Center,
            new Color(0.95f, 0.96f, 0.98f, 1f)
        );
        RectTransform textRt = pageTmp.GetComponent<RectTransform>();
        textRt.sizeDelta = new Vector2(210f, 32f);
        ActivePageLabels.Add(pageTmp);

        // 3. Next Button
        Button nextBtn = UIHelper.CreateButton(
            barObj.transform,
            "Btn_Next",
            "NEXT ►",
            95f,
            32f,
            btnNormal,
            btnHover,
            btnPressed,
            btnDisabled,
            NextPage,
            out _
        );
        ActiveNextButtons.Add(nextBtn);

        // 4. Rename Button
        Color renameNormal = new Color(0.12f, 0.32f, 0.20f, 0.95f);
        Color renameHover = new Color(0.18f, 0.48f, 0.30f, 1f);
        Color renamePressed = new Color(0.08f, 0.20f, 0.12f, 1f);

        UIHelper.CreateButton(
            barObj.transform,
            "Btn_Rename",
            "✏️ RENAME",
            110f,
            32f,
            renameNormal,
            renameHover,
            renamePressed,
            btnDisabled,
            () => RenameDialog.OpenForHoveredOrSelected(),
            out _
        );

        // 5. Delete Button (mirrors Rename)
        Color deleteNormal = new Color(0.45f, 0.12f, 0.12f, 0.95f);
        Color deleteHover = new Color(0.65f, 0.18f, 0.18f, 1f);
        Color deletePressed = new Color(0.28f, 0.08f, 0.08f, 1f);

        UIHelper.CreateButton(
            barObj.transform,
            "Btn_Delete",
            "🗑️ DELETE",
            110f,
            32f,
            deleteNormal,
            deleteHover,
            deletePressed,
            btnDisabled,
            () => DeleteDialog.OpenForHoveredOrSelected(),
            out _
        );

        UpdateUILabel();
    }

    private static void AttachHoverTracker(GameObject slotObj, int localIndex)
    {
        var trigger = slotObj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slotObj.AddComponent<EventTrigger>();
        }

        if (trigger.triggers == null)
        {
            trigger.triggers = new il2cpp::Il2CppSystem.Collections.Generic.List<EventTrigger.Entry>();
        }
        else
        {
            // Remove only our hover/click entries (stale localIndex closures from a
            // previous attach) — never Clear() the whole list, vanilla may own entries
            // (hover/click handlers) that must survive.
            for (int i = trigger.triggers.Count - 1; i >= 0; i--)
            {
                var existing = trigger.triggers[i];
                if (existing == null) { trigger.triggers.RemoveAt(i); continue; }
                if (existing.eventID == EventTriggerType.PointerEnter
                    || existing.eventID == EventTriggerType.PointerExit
                    || existing.eventID == EventTriggerType.PointerClick)
                    trigger.triggers.RemoveAt(i);
            }
        }

        // IL2CPP-safe subscription via S1API (a direct UnityAction cast faults on
        // the IntPtr boundary per AGENTS.md §5).
        EventHelper.AddEventTrigger(trigger, EventTriggerType.PointerEnter, _ =>
        {
            SelectedLocalSlot = localIndex;
            HoveredLocalSlot = localIndex;
        });
        EventHelper.AddEventTrigger(trigger, EventTriggerType.PointerExit, _ =>
        {
            if (HoveredLocalSlot == localIndex)
            {
                HoveredLocalSlot = -1;
            }
            // Do NOT reset SelectedLocalSlot so moving down to click Rename preserves the selected slot
        });
        EventHelper.AddEventTrigger(trigger, EventTriggerType.PointerClick, _ =>
        {
            SelectedLocalSlot = localIndex;
        });
    }

    public static void UpdateUILabel()
    {
        string labelStr = GetPageLabelString();

        for (int i = ActivePageLabels.Count - 1; i >= 0; i--)
        {
            var label = ActivePageLabels[i];
            if (label == null || label.gameObject == null)
            {
                ActivePageLabels.RemoveAt(i);
                continue;
            }
            label.text = labelStr;
        }

        for (int i = ActivePrevButtons.Count - 1; i >= 0; i--)
        {
            var btn = ActivePrevButtons[i];
            if (btn == null || btn.gameObject == null)
            {
                ActivePrevButtons.RemoveAt(i);
                continue;
            }
            btn.interactable = CurrentPage > 0;
        }

        for (int i = ActiveNextButtons.Count - 1; i >= 0; i--)
        {
            var btn = ActiveNextButtons[i];
            if (btn == null || btn.gameObject == null)
            {
                ActiveNextButtons.RemoveAt(i);
                continue;
            }
            btn.interactable = CurrentPage < TotalPages - 1;
        }
    }

    public static string GetPageLabelString()
    {
        int startSlot = CurrentPage * SlotsPerPage + 1;
        int endSlot = Mathf.Min((CurrentPage + 1) * SlotsPerPage, TotalSlots);
        return $"PAGE {CurrentPage + 1} / {TotalPages}  (SLOTS {startSlot}–{endSlot})";
    }
}

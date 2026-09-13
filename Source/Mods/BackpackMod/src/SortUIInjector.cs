extern alias il2cpp;
using Il2CppScheduleOne.UI;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace BackpackMod.Patches
{
    /// <summary>
    /// B1 (QoL Spec v1.1.0): Injects a "[ Sort ]" button into the vanilla StorageMenu
    /// (world containers) and wires hotkeys for backpack (B-menu already exists) and
    /// player-inventory sorting. The button is cloned from the menu's own close-button
    /// so it inherits vanilla styling, parent canvas and click handling.
    ///
    /// Design rules from Spec §Pitfalls:
    /// - Button creation is idempotent (tracked by GameObject name) — Open fires every time.
    /// - All native touches guarded (WasCollected/IntPtr).
    /// - Sorting our own backpack entity through the vanilla menu is routed to SortBackpack().
    /// - Keyboard fallbacks (Spec §Observability): F6 = sort open container, F7 = sort
    ///   player inventory — guard-gated by HotkeyManager.IsInputFieldFocused.
    /// </summary>
    public static class SortUIInjector
    {
        private const string SortButtonName = "BackpackMod_SortButton";

        private static GameObject? _storageSortButton;
        private static GameObject? _hudSortButton;

        // ─────────────────────────────────────────────────────────────
        // StorageMenu.Open postfix — Harmony calls this after every open
        // ─────────────────────────────────────────────────────────────

        public static void StorageMenu_Open_Postfix(StorageMenu __instance)
        {
            try
            {
                if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;
                EnsureSortButton(__instance);
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"StorageMenu_Open_Postfix failed: {ex.Message}");
            }
        }

        public static void StorageMenu_Close_Postfix(StorageMenu __instance)
        {
            // Menu closed → hide (not destroy) the button; it lives inside the menu's
            // canvas so hiding follows the menu automatically. Defensive re-hide ok.
            try
            {
                if (_storageSortButton != null && _storageSortButton.Pointer != IntPtr.Zero && !_storageSortButton.WasCollected)
                    _storageSortButton.SetActive(false);
            }
            catch { }
        }

        // ─────────────────────────────────────────────────────────────
        // GameplayMenu inventory sort button (button-only, no hotkeys).
        // The vanilla inventory lives in GameplayMenu (screens: Phone, Character);
        // IsOpen + CurrentScreen==Character means the inventory is on screen.
        // Harmony postfixes on OnOpen/OnClose/SetScreen call the sync below.
        // ─────────────────────────────────────────────────────────────

        /// <summary>Harmony postfix target — syncs the HUD button after every menu state change.</summary>
        public static void GameplayMenu_Sync_Postfix(Il2CppScheduleOne.UI.GameplayMenu __instance)
        {
            try
            {
                if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;
                bool invOnScreen = __instance.IsOpen
                    && __instance.CurrentScreen == Il2CppScheduleOne.UI.GameplayMenu.EGameplayScreen.Character;
                HudInventoryVisibilityChanged(invOnScreen);
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"GameplayMenu_Sync_Postfix failed: {ex.Message}");
            }
        }

        /// <summary>Shows/hides (and lazily creates) the inventory sort button.</summary>
        public static void HudInventoryVisibilityChanged(bool inventoryVisible)
        {
            try
            {
                if (inventoryVisible) EnsureHudSortButton();
                if (_hudSortButton != null && _hudSortButton.Pointer != IntPtr.Zero && !_hudSortButton.WasCollected)
                    _hudSortButton.SetActive(inventoryVisible);
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"HudInventoryVisibilityChanged failed: {ex.Message}");
            }
        }

        private static void EnsureHudSortButton()
        {
            // Idempotent recreation after scene switches.
            if (_hudSortButton != null && _hudSortButton.Pointer != IntPtr.Zero && !_hudSortButton.WasCollected)
                return;
            _hudSortButton = null;

            try
            {
                var hud = Il2CppScheduleOne.DevUtilities.Singleton<Il2CppScheduleOne.UI.HUD>.Instance;
                if (hud == null || hud.Pointer == IntPtr.Zero || hud.WasCollected) return;

                var slotContainer = hud.SlotContainer;
                if (slotContainer == null || slotContainer.Pointer == IntPtr.Zero || slotContainer.WasCollected) return;

                // Template: any vanilla button under the HUD canvas (close/sleep prompts etc.).
                Button? template = slotContainer.GetComponentInChildren<Button>();
                if (template == null || template.Pointer == IntPtr.Zero || template.WasCollected)
                {
                    // Fall back: search the whole HUD canvas.
                    var hudCanvas = hud.canvas;
                    if (hudCanvas != null && hudCanvas.Pointer != IntPtr.Zero && !hudCanvas.WasCollected)
                        template = hudCanvas.GetComponentInChildren<Button>();
                }
                if (template == null || template.Pointer == IntPtr.Zero || template.WasCollected)
                {
                    Mod.Log?.Warning("HUD sort button: no template Button found under HUD canvas.");
                    return;
                }

                Transform parent = slotContainer.parent;
                if (parent == null || parent.Pointer == IntPtr.Zero) return;

                var obj = UnityEngine.Object.Instantiate(template.gameObject, parent);
                obj.name = "BackpackMod_HudSortButton";
                _hudSortButton = obj;

                var rt = obj.GetComponent<RectTransform>();
                if (rt != null && rt.Pointer != IntPtr.Zero)
                {
                    // Anchor to the bottom-right corner region above the hotbar.
                    rt.anchorMin = new Vector2(1f, 0f);
                    rt.anchorMax = new Vector2(1f, 0f);
                    rt.pivot = new Vector2(1f, 0f);
                    rt.anchoredPosition = new Vector2(-24f, 150f);
                    rt.sizeDelta = new Vector2(150f, 44f);
                }

                var label = obj.GetComponentInChildren<TextMeshProUGUI>();
                if (label != null && label.Pointer != IntPtr.Zero && !label.WasCollected)
                    label.text = "Sort Inventory";
                else
                {
                    var legacyText = obj.GetComponentInChildren<Text>();
                    if (legacyText != null && legacyText.Pointer != IntPtr.Zero) legacyText.text = "Sort Inventory";
                }

                var btn = obj.GetComponent<Button>();
                if (btn != null && btn.Pointer != IntPtr.Zero && !btn.WasCollected)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(new Action(() =>
                    {
                        try { BackpackInventorySorter.SortPlayerInventory(); }
                        catch (Exception ex) { Mod.Log?.Error($"Inventory sort button failed: {ex.Message}"); }
                    }));
                }

                obj.SetActive(false); // visibility managed by HudInventoryVisibilityChanged
                Mod.Log?.Msg("HUD inventory sort button created.");
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"EnsureHudSortButton failed: {ex.Message}");
            }
        }

        private static void EnsureSortButton(StorageMenu menu)
        {
            // Idempotent: reuse existing button object if it survived.
            if (_storageSortButton != null && _storageSortButton.Pointer != IntPtr.Zero && !_storageSortButton.WasCollected)
            {
                _storageSortButton.SetActive(true);
                return;
            }

            try
            {
                // Template: the menu's own close button container — guaranteed vanilla-styled.
                var closeButtonContainer = menu.CloseButtonContainer;
                if (closeButtonContainer == null || closeButtonContainer.Pointer == IntPtr.Zero || closeButtonContainer.WasCollected)
                {
                    Mod.Log?.Warning("Sort button: StorageMenu.CloseButtonContainer unavailable.");
                    return;
                }

                Transform parent = closeButtonContainer.parent;
                if (parent == null || parent.Pointer == IntPtr.Zero) return;

                // Find a Button component anywhere in the close-button hierarchy to clone.
                Button? templateButton = closeButtonContainer.GetComponentInChildren<Button>();
                if (templateButton == null || templateButton.Pointer == IntPtr.Zero || templateButton.WasCollected)
                {
                    Mod.Log?.Warning("Sort button: no template Button found in CloseButtonContainer.");
                    return;
                }

                var obj = UnityEngine.Object.Instantiate(templateButton.gameObject, parent);
                obj.name = SortButtonName;
                _storageSortButton = obj;

                // Bug-Audit 2026-09-12 (Round 4): the cloned Button's Image child has
                // raycastTarget = true by default (inherited from the template). When the
                // Sort button is anchored at (-24, 150) on the storage canvas it overlaps
                // the slot grid and swallows drag/click events for those slots. Disable
                // the Image's raycastTarget — the Button itself still receives clicks
                // because its RectTransform keeps raycastTarget = true.
                try
                {
                    var img = obj.GetComponentInChildren<Image>(true);
                    if (img != null && img.Pointer != IntPtr.Zero && !img.WasCollected)
                    {
                        img.raycastTarget = false;
                    }
                }
                catch { /* non-fatal */ }

                // Position: nudge left of the close button cluster.
                var rt = obj.GetComponent<RectTransform>();
                if (rt != null && rt.Pointer != IntPtr.Zero)
                {
                    rt.anchoredPosition += new Vector2(-190f, 0f);
                }

                // Label (TMP if present).
                var label = obj.GetComponentInChildren<TextMeshProUGUI>();
                if (label != null && label.Pointer != IntPtr.Zero && !label.WasCollected)
                {
                    label.text = "Sort";
                }
                else
                {
                    var legacyText = obj.GetComponentInChildren<Text>();
                    if (legacyText != null && legacyText.Pointer != IntPtr.Zero) legacyText.text = "Sort";
                }

                // Wire click → sort the currently open container.
                var btn = obj.GetComponent<Button>();
                if (btn != null && btn.Pointer != IntPtr.Zero && !btn.WasCollected)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(new Action(() =>
                    {
                        try { BackpackInventorySorter.SortOpenStorageMenu(); }
                        catch (Exception ex) { Mod.Log?.Error($"Sort button click failed: {ex.Message}"); }
                    }));
                }

                Mod.Log?.Msg("Sort button injected into StorageMenu.");
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"EnsureSortButton failed: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Keyboard fallbacks REMOVED per user decision 2026-09-12:
        // sorting is button-only (StorageMenu "Sort" button + inventory button).
        // ─────────────────────────────────────────────────────────────

        /// <summary>Scene teardown (called from Mod.OnSceneWasUnloaded).</summary>
        public static void ResetForSceneUnload()
        {
            _storageSortButton = null;
        }
    }
}

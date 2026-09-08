using HarmonyLib;
using Il2CppScheduleOne.Clothing;
using Il2CppScheduleOne.UI;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace BackpackMod.Patches
{
    public static class CharacterUIPatch
    {
        // Gatekeeper-fix 2026-08-29: Schedule I's EClothingSlot enum uses index 10 for backpack.
        // If the game ever adds an explicit backpack-slot enum value, re-check this constant.
        private const EClothingSlot BackpackSlotId = (EClothingSlot)10;

        private static GameObject? _backpackSlotObj;
        private static ClothingSlotUI? _backpackSlotUI;

        public static void Open_Postfix(CharacterInterface __instance)
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;

            EnsureSlotCreated(__instance);
            if (_backpackSlotUI != null && _backpackSlotUI.Pointer != IntPtr.Zero && !_backpackSlotUI.WasCollected)
            {
                if (_backpackSlotUI.assignedSlot == null || _backpackSlotUI.assignedSlot.Pointer == IntPtr.Zero || _backpackSlotUI.assignedSlot.WasCollected)
                {
                    var pc = Il2CppScheduleOne.DevUtilities.PlayerSingleton<Il2CppScheduleOne.PlayerScripts.PlayerMovement>.Instance?.GetComponent<Il2CppScheduleOne.PlayerScripts.PlayerClothing>();
                    if (pc != null && pc.Pointer != IntPtr.Zero && !pc.WasCollected && pc.ClothingSlots != null && pc.ClothingSlots.ContainsKey(BackpackSlotId))
                    {
                        _backpackSlotUI.AssignSlot(pc.ClothingSlots[BackpackSlotId]);
                    }
                    else if (PlayerClothingPatch.BackpackSlot != null && PlayerClothingPatch.BackpackSlot.Pointer != IntPtr.Zero && !PlayerClothingPatch.BackpackSlot.WasCollected)
                    {
                        _backpackSlotUI.AssignSlot(PlayerClothingPatch.BackpackSlot);
                    }
                }
                _backpackSlotUI.UpdateUI();
            }

            if (_backpackSlotObj != null && _backpackSlotObj.Pointer != IntPtr.Zero && !_backpackSlotObj.WasCollected)
            {
                _backpackSlotObj.SetActive(true);
            }
        }

        public static void Close_Postfix(CharacterInterface __instance)
        {
            if (_backpackSlotObj != null && _backpackSlotObj.Pointer != IntPtr.Zero && !_backpackSlotObj.WasCollected)
            {
                _backpackSlotObj.SetActive(false);
            }
        }

        public static void LateUpdate_Postfix(CharacterInterface __instance)
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected || _backpackSlotObj == null || _backpackSlotObj.Pointer == IntPtr.Zero || _backpackSlotObj.WasCollected || !_backpackSlotObj.activeSelf) return;


            try
            {
                var slots = __instance.ClothingSlots;
                if (slots != null)
                {
                    ClothingSlotUI? handsSlot = null;
                    foreach (var s in slots)
                    {
                        if (s != null && s.SlotType == EClothingSlot.Hands)
                        {
                            handsSlot = s;
                            break;
                        }
                    }

                    if (handsSlot != null && handsSlot.gameObject != null)
                    {
                        var targetPos = handsSlot.transform.position;
                        _backpackSlotObj.transform.position = targetPos + new Vector3(0f, -80f * (Screen.height / 1080f), 0f);
                    }
                }

                // Mannequin 360-degree inspection rotation
                var charDisplay = Il2CppScheduleOne.DevUtilities.Singleton<CharacterDisplay>.Instance;
                if (charDisplay != null && charDisplay.Pointer != IntPtr.Zero && charDisplay.IsOpen)
                {
                    // Right-click drag rotates mannequin smoothly
                    if (Input.GetMouseButton(1))
                    {
                        float mouseDeltaX = Input.GetAxis("Mouse X");
                        if (Mathf.Abs(mouseDeltaX) > 0.001f)
                        {
                            charDisplay.targetRotation += mouseDeltaX * -180f * Time.unscaledDeltaTime;
                        }
                    }

                    // Fix 3.2 (Bug-Audit 2026-09-02): don't rotate while the player is typing
                    // in a UI text field (e.g. naming fields). Canonical API: Shared HotkeyManager.
                    if (S1Mods.Shared.HotkeyManager.IsInputFieldFocused()) return;

                    // Q / E keys rotate mannequin
                    if (Input.GetKey(KeyCode.Q))
                    {
                        charDisplay.targetRotation += 120f * Time.unscaledDeltaTime;
                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        charDisplay.targetRotation -= 120f * Time.unscaledDeltaTime;
                    }
                }
            }
            catch
            {
            }
        }

        private static void EnsureSlotCreated(CharacterInterface __instance)
        {
            if ((_backpackSlotObj != null && _backpackSlotObj.Pointer != IntPtr.Zero) || __instance == null || __instance.Pointer == IntPtr.Zero) return;

            try
            {
                var slots = __instance.ClothingSlots;
                if (slots == null || slots.Length == 0) return;

                ClothingSlotUI template = slots[0];
                foreach (var s in slots)
                {
                    if (s != null && (s.SlotType == EClothingSlot.Hands || s.SlotType == EClothingSlot.Outerwear))
                    {
                        template = s;
                        break;
                    }
                }

                Transform parent = template.transform.parent;
                _backpackSlotObj = UnityEngine.Object.Instantiate(template.gameObject, parent);
                _backpackSlotObj.name = "Backpack_ClothingSlot";
                _backpackSlotObj.transform.localScale = Vector3.one;

                _backpackSlotUI = _backpackSlotObj.GetComponent<ClothingSlotUI>();
                if (_backpackSlotUI != null)
                {
                    _backpackSlotUI.SlotType = BackpackSlotId;
                    if (_backpackSlotUI.SlotTypeImage != null)
                    {
                        _backpackSlotUI.SlotTypeImage.sprite = BackpackIconGenerator.GetSlotIcon();
                        _backpackSlotUI.SlotTypeImage.color = new Color(1f, 1f, 1f, 0.9f);
                        _backpackSlotUI.SlotTypeImage.enabled = true;
                    }

                    var pc = Il2CppScheduleOne.DevUtilities.PlayerSingleton<Il2CppScheduleOne.PlayerScripts.PlayerMovement>.Instance?.GetComponent<Il2CppScheduleOne.PlayerScripts.PlayerClothing>();
                    if (pc != null && pc.ClothingSlots != null && pc.ClothingSlots.ContainsKey(BackpackSlotId))
                    {
                        _backpackSlotUI.AssignSlot(pc.ClothingSlots[BackpackSlotId]);
                    }
                    else if (PlayerClothingPatch.BackpackSlot != null)
                    {
                        _backpackSlotUI.AssignSlot(PlayerClothingPatch.BackpackSlot);
                    }

                    _backpackSlotUI.UpdateUI();
                }

                Mod.Log?.Msg("Successfully created BackpackSlotUI attached to ClothingSlots parent.");
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Failed to create BackpackSlotUI: {ex.Message}");
            }
        }
    }
}

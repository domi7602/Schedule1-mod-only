using System;
using HarmonyLib;

namespace BackpackMod.Patches
{
    /// <summary>
    /// Guards vanilla ClothingItemUI.UpdateUI against NRE when a custom clothing item
    /// (backpack) is added via AddItemToInventory. Vanilla UpdateUI dereferences
    /// clothing visuals that may be null on cloned definitions — swallow NRE so the
    /// item still lands in inventory and the game stays alive. Logged as warning
    /// for diagnostics; real fix is in BackpackDefinitions to ensure visuals are set.
    /// Gatekeeper-hotfix 2026-08-30 for console `additem backpack_t1` crash.
    /// </summary>
    public static class ClothingItemUIPatch
    {
        public static Exception? Finalizer(Exception? __exception)
        {
            if (__exception is NullReferenceException)
            {
                try { MelonLoader.MelonLogger.Warning($"[BackpackMod][ClothingItemUI] UpdateUI NRE swallowed (backpack item added): {__exception.Message}"); } catch { }
                return null; // swallow — keep game alive, item stays in inventory
            }
            return __exception;
        }
    }
}

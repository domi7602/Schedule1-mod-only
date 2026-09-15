using HarmonyLib;
using Il2CppScheduleOne.Clothing;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using System;

namespace BackpackMod.Patches
{
    public static class PlayerClothingPatch
    {
        // Gatekeeper-fix 2026-08-29: Schedule I's EClothingSlot enum uses index 10 for backpack.
        // If the game ever adds an explicit backpack-slot enum value, re-check this constant.
        private const EClothingSlot BackpackSlotId = (EClothingSlot)10;

        public static ItemSlot? BackpackSlot { get; private set; }

        // Gatekeeper-fix 2026-09-10: the cached slot is an IL2CPP reference to player-scoped state —
        // drop it on scene unload so UpdateVisuals can't dereference a dead object from a previous scene.
        public static void ResetForSceneUnload() => BackpackSlot = null;

        public static void Prefix(PlayerClothing __instance) { }
        public static void Postfix(PlayerClothing __instance)
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;

            try
            {
                if (__instance.ClothingSlots != null)
                {
                    if (__instance.ClothingSlots.ContainsKey(BackpackSlotId))
                    {
                        BackpackSlot = __instance.ClothingSlots[BackpackSlotId];
                        return;
                    }
                    var slot = new ItemSlot(false);
                    slot.AddFilter(new ItemFilter_ClothingSlot(BackpackSlotId));

                    __instance.ClothingSlots.Add(BackpackSlotId, slot);
                    if (__instance.ItemSlots != null)
                    {
                        __instance.ItemSlots.Add(slot);
                    }

                    BackpackSlot = slot;
                    Mod.Log?.Msg("Successfully registered Backpack ItemSlot (Slot 10) in PlayerClothing!");
                }
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error in PlayerClothingPatch: {ex}");
            }
        }

        public static void Awake_Postfix(PlayerClothing __instance) => Postfix(__instance);
    }
}

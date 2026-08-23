using HarmonyLib;
using Il2CppScheduleOne.Clothing;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using System;

namespace BackpackMod.Patches
{
    public static class PlayerClothingPatch
    {
        public static ItemSlot? BackpackSlot { get; private set; }

        public static void Prefix(PlayerClothing __instance) { }
        public static void Postfix(PlayerClothing __instance)
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;

            try
            {
                if (__instance.ClothingSlots != null)
                {
                    if (__instance.ClothingSlots.ContainsKey((EClothingSlot)10))
                    {
                        BackpackSlot = __instance.ClothingSlots[(EClothingSlot)10];
                        return;
                    }
                    var slot = new ItemSlot(false);
                    slot.AddFilter(new ItemFilter_ClothingSlot((EClothingSlot)10));

                    __instance.ClothingSlots.Add((EClothingSlot)10, slot);
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

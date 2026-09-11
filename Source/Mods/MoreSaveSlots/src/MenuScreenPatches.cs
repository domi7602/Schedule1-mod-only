using System;
using HarmonyLib;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.UI.MainMenu;
using MelonLoader;
using MoreSaveSlots.UI;

namespace MoreSaveSlots.Patches;

[HarmonyPatch(typeof(ContinueScreen), nameof(ContinueScreen.LoadGame))]
public static class ContinueScreen_LoadGame_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(ref int index)
    {
        int original = index;
        int mapped = PaginationController.CurrentPage * PaginationController.SlotsPerPage + index;

        if (LoadManager.SaveGames == null || mapped < 0 || mapped >= LoadManager.SaveGames.Length || LoadManager.SaveGames[mapped] == null)
        {
            MelonLogger.Msg($"[MoreSaveSlots] ContinueScreen: Slot {mapped + 1} is empty, ignoring click.");
            return false;
        }

        MelonLogger.Msg($"[MoreSaveSlots] ContinueScreen.LoadGame: mapped slot {original + 1} -> {mapped + 1} (SaveGame_{mapped + 1})");
        index = mapped;
        return true;
    }
}

[HarmonyPatch(typeof(NewGameScreen), nameof(NewGameScreen.SlotSelected))]
public static class NewGameScreen_SlotSelected_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(ref int slotIndex)
    {
        int original = slotIndex;
        int mapped = PaginationController.CurrentPage * PaginationController.SlotsPerPage + slotIndex;
        // Same bounds guard as ContinueScreen above: on a partial last page
        // (TotalSlots % 5 != 0) the trailing local indices map past the array.
        if (mapped < 0 || mapped >= PaginationController.TotalSlots
            || LoadManager.SaveGames == null || mapped >= LoadManager.SaveGames.Length)
        {
            MelonLogger.Msg($"[MoreSaveSlots] NewGameScreen: Slot {mapped + 1} out of range, ignoring click.");
            return false;
        }
        MelonLogger.Msg($"[MoreSaveSlots] NewGameScreen.SlotSelected: mapped slot {original + 1} -> {mapped + 1} (SaveGame_{mapped + 1})");
        slotIndex = mapped;
        return true;
    }
}

[HarmonyPatch(typeof(MenuScreen), nameof(MenuScreen.OnOpen))]
public static class MenuScreen_OnOpen_Patch
{
    [HarmonyPostfix]
    public static void Postfix(MenuScreen __instance)
    {
        if (__instance == null) return;

        if (__instance is ContinueScreen || __instance is NewGameScreen || __instance is ImportScreen)
        {
            var saveDisplay = __instance.GetComponentInChildren<SaveDisplay>(true);
            if (saveDisplay != null)
            {
                PaginationController.EnsurePaginationBar(saveDisplay);
                saveDisplay.Refresh();
            }
        }
    }
}

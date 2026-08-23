using System;
using System.IO;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using MoreSaveSlots.UI;
using UnityEngine;

namespace MoreSaveSlots.Patches;

[HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Awake))]
public static class SaveManager_Awake_Patch
{
    [HarmonyPostfix]
    public static void Postfix(SaveManager __instance)
    {
        try
        {
            SaveManager.SAVE_SLOT_COUNT = PaginationController.TotalSlots;
            MelonLogger.Msg($"[MoreSaveSlots] SaveManager.Awake: SAVE_SLOT_COUNT set to {PaginationController.TotalSlots}.");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error in SaveManager.Awake patch: {ex}");
        }
    }
}

[HarmonyPatch(typeof(LoadManager), nameof(LoadManager.RefreshSaveInfo))]
public static class LoadManager_RefreshSaveInfo_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(LoadManager __instance)
    {
        try
        {
            int totalSlots = PaginationController.TotalSlots;
            SaveManager.SAVE_SLOT_COUNT = totalSlots;

            string savesContainerPath = SaveManager.Instance?.IndividualSavesContainerPath;
            if (string.IsNullOrEmpty(savesContainerPath))
            {
                string playersSavePath = SaveManager.Instance?.PlayersSavePath;
                if (!string.IsNullOrEmpty(playersSavePath))
                {
                    savesContainerPath = playersSavePath;
                }
                else
                {
                    savesContainerPath = Path.Combine(Application.persistentDataPath, "Saves");
                }
            }

            MelonLogger.Msg($"[MoreSaveSlots] Refreshing save info for {totalSlots} slots in '{savesContainerPath}'...");

            var saveArray = new Il2CppReferenceArray<SaveInfo>(totalSlots);
            SaveInfo? newestSave = null;

            for (int i = 0; i < totalSlots; i++)
            {
                int slotNumber = i + 1;
                string slotDirName = $"{SaveManager.SAVE_GAME_PREFIX}{slotNumber}";
                string slotFolderPath = Path.Combine(savesContainerPath, slotDirName);

                SaveInfo? info = null;
                if (Directory.Exists(slotFolderPath))
                {
                    bool loaded = LoadManager.TryLoadSaveInfo(slotFolderPath, i, out info, false);
                    if (loaded && info != null)
                    {
                        if (newestSave == null || info.DateLastPlayed > newestSave.DateLastPlayed)
                        {
                            newestSave = info;
                        }
                    }
                }

                saveArray[i] = info;
            }

            LoadManager.SaveGames = saveArray;
            LoadManager.LastPlayedGame = newestSave;

            MelonLogger.Msg($"[MoreSaveSlots] Scanned {totalSlots} slots successfully. Active saves found.");

            // Notify S1API / listeners
            if (__instance != null && __instance.onSaveInfoLoaded != null)
            {
                __instance.onSaveInfoLoaded.Invoke();
            }

            return false; // Skip vanilla 5-slot scan
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error in LoadManager.RefreshSaveInfo prefix: {ex}");
            return true; // Fallback to vanilla if error
        }
    }
}

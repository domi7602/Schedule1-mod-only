using System;
using System.IO;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using MoreSaveSlots.UI;

namespace MoreSaveSlots.Services;

public static class SaveDeleteService
{
    public static bool DeleteSave(int slotIndex)
    {
        try
        {
            SaveInfo? saveInfo = null;
            if (LoadManager.SaveGames != null && slotIndex >= 0 && slotIndex < LoadManager.SaveGames.Length)
            {
                saveInfo = LoadManager.SaveGames[slotIndex];
            }

            if (saveInfo == null)
            {
                MelonLogger.Warning($"[MoreSaveSlots] Cannot delete save: Slot {slotIndex + 1} is empty or not loaded.");
                return false;
            }

            // Guard: don't delete active in-game save
            try
            {
                var active = LoadManager.Instance?.ActiveSaveInfo;
                if (active != null && active.SaveSlotNumber == (slotIndex + 1))
                {
                    MelonLogger.Warning($"[MoreSaveSlots] Cannot delete active save Slot {slotIndex + 1} while it is loaded in-game. Exit to Menu first.");
                    return false;
                }
            }
            catch { }

            string savePath = saveInfo.SavePath;
            if (string.IsNullOrEmpty(savePath))
            {
                // Fallback via SaveManager container path
                string container = SaveManager.Instance?.IndividualSavesContainerPath;
                if (!string.IsNullOrEmpty(container))
                {
                    string slotDirName = $"{SaveManager.SAVE_GAME_PREFIX}{slotIndex + 1}";
                    savePath = Path.Combine(container, slotDirName);
                }
            }

            if (string.IsNullOrEmpty(savePath) || !Directory.Exists(savePath))
            {
                MelonLogger.Warning($"[MoreSaveSlots] Cannot delete save: Folder '{savePath}' does not exist (slot {slotIndex + 1}).");
                // Still clear in-memory reference
                if (LoadManager.SaveGames != null && slotIndex >= 0 && slotIndex < LoadManager.SaveGames.Length)
                {
                    LoadManager.SaveGames[slotIndex] = null;
                }
                PaginationController.RefreshActiveScreen();
                return false;
            }

            string orgName = saveInfo.OrganisationName ?? $"Slot {slotIndex + 1}";
            MelonLogger.Msg($"[MoreSaveSlots] Deleting save Slot {slotIndex + 1} ('{orgName}') at '{savePath}'...");

            // Delete folder recursively
            Directory.Delete(savePath, true);

            // Also try to delete .bak leftover if exists (e.g., Game.json.bak outside folder — unlikely)
            // Not needed — folder deletion covers it

            // Clear in-memory SaveInfo
            if (LoadManager.SaveGames != null && slotIndex >= 0 && slotIndex < LoadManager.SaveGames.Length)
            {
                LoadManager.SaveGames[slotIndex] = null;
            }

            // Re-evaluate LastPlayedGame
            try
            {
                SaveInfo? newest = null;
                if (LoadManager.SaveGames != null)
                {
                    for (int i = 0; i < LoadManager.SaveGames.Length; i++)
                    {
                        var info = LoadManager.SaveGames[i];
                        if (info != null)
                        {
                            if (newest == null || info.DateLastPlayed > newest.DateLastPlayed)
                                newest = info;
                        }
                    }
                }
                LoadManager.LastPlayedGame = newest;
            }
            catch { }

            MelonLogger.Msg($"[MoreSaveSlots] Successfully deleted Slot {slotIndex + 1}.");

            PaginationController.RefreshActiveScreen();
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error deleting save in slot {slotIndex + 1}: {ex}");
            return false;
        }
    }
}

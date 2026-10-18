using System;
using System.IO;
using System.Text.Json.Nodes;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using MoreSaveSlots.UI;
using S1Mods.Shared;

namespace MoreSaveSlots.Services;

public static class SaveRenameService
{
    public static bool RenameSave(int slotIndex, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            MelonLogger.Warning("[MoreSaveSlots] Cannot rename save: New name is empty.");
            return false;
        }

        newName = newName.Trim();

        try
        {
            SaveInfo? saveInfo = null;
            if (LoadManager.SaveGames != null && slotIndex >= 0 && slotIndex < LoadManager.SaveGames.Length)
            {
                saveInfo = LoadManager.SaveGames[slotIndex];
            }

            if (saveInfo == null)
            {
                MelonLogger.Warning($"[MoreSaveSlots] Cannot rename save: Slot {slotIndex + 1} is empty or not loaded.");
                return false;
            }

            string savePath = saveInfo.SavePath;
            if (string.IsNullOrEmpty(savePath) || !Directory.Exists(savePath))
            {
                MelonLogger.Warning($"[MoreSaveSlots] Cannot rename save: Folder '{savePath}' does not exist.");
                return false;
            }

            string gameJsonPath = Path.Combine(savePath, "Game.json");
            if (!File.Exists(gameJsonPath))
            {
                MelonLogger.Warning($"[MoreSaveSlots] Cannot rename save: 'Game.json' not found in '{savePath}'.");
                return false;
            }

            string json = SafeStorage.LoadTextSafe(gameJsonPath, "");
            if (string.IsNullOrWhiteSpace(json))
            {
                // Fallback to direct read if LoadTextSafe fallback was empty but file exists
                json = File.ReadAllText(gameJsonPath);
            }
            var jsonNode = JsonNode.Parse(json);
            string newJson;
            if (jsonNode is JsonObject jsonObj)
            {
                jsonObj["OrganisationName"] = newName;
                newJson = jsonObj.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            }
            else
            {
                // No textual regex fallback: hand-rolled escaping misses newlines and
                // control chars and can corrupt Game.json. A non-object root has no
                // OrganisationName property to rename anyway — fail loudly instead.
                MelonLogger.Warning("[MoreSaveSlots] Cannot rename save: unexpected Game.json root (not an object).");
                return false;
            }

            // Pre-write backup under its own name (SafeStorage's .bak covers the atomic
            // swap itself; this one survives for manual recovery after a rename).
            try { File.Copy(gameJsonPath, gameJsonPath + ".pre-rename.bak", true); } catch { }

            if (!SafeStorage.SaveTextAtomic(gameJsonPath, newJson))
            {
                MelonLogger.Error($"[MoreSaveSlots] SafeStorage failed to save renamed Game.json atomically.");
                return false;
            }

            // Update in-memory SaveInfo
            saveInfo.OrganisationName = newName;

            // If active in-game session matches this save slot, update GameManager
            if (GameManager.Instance != null && LoadManager.Instance?.ActiveSaveInfo?.SaveSlotNumber == (slotIndex + 1))
            {
                GameManager.Instance.OrganisationName = newName;
            }

            MelonLogger.Msg($"[MoreSaveSlots] Successfully renamed Slot {slotIndex + 1} to '{newName}'.");

            // Refresh UI immediately
            PaginationController.RefreshActiveScreen();
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MoreSaveSlots] Error renaming save in slot {slotIndex + 1}: {ex}");
            return false;
        }
    }
}

using System;
using System.IO;
using System.Linq;
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
    private static string NormalizePath(string path)
    {
        if (string.IsNullOrEmpty(path)) return path;
        try
        {
            string normalized = path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            return Path.GetFullPath(normalized);
        }
        catch
        {
            return path;
        }
    }

    private static bool _hasLoggedFirstScan = false;
    private static int _lastFoundCount = -1;
    private static float _lastLogTime = -1000f;
    // Gatekeeper-fix 2026-08-29: defend against double-fire of onSaveInfoLoaded.
    // Scenario A: another mod also patches RefreshSaveInfo and runs vanilla despite our Prefix returning false.
    // Scenario B: the Invoke() itself throws → current try-block would short-circuit to catch → return true
    //   → vanilla fires its own Invoke. The user-facing event would fire twice.
    // We track the last frame we invoked on, and if we see a second Invoke within the same frame we no-op it.
    private static int _lastInvokeFrame = -1;

    [HarmonyPrefix]
    public static bool Prefix(LoadManager __instance)
    {
        try
        {
            int totalSlots = PaginationController.TotalSlots;
            SaveManager.SAVE_SLOT_COUNT = totalSlots;

            string rawContainer = SaveManager.Instance?.IndividualSavesContainerPath;
            string fallbackPlayers = SaveManager.Instance?.PlayersSavePath;
            string savesContainerPath = rawContainer;

            if (string.IsNullOrEmpty(savesContainerPath))
            {
                if (!string.IsNullOrEmpty(fallbackPlayers))
                {
                    savesContainerPath = fallbackPlayers;
                    MelonLogger.Msg($"[MoreSaveSlots] IndividualSavesContainerPath was null/empty, falling back to PlayersSavePath: '{savesContainerPath}'");
                }
                else
                {
                    savesContainerPath = Path.Combine(Application.persistentDataPath, "Saves");
                    MelonLogger.Msg($"[MoreSaveSlots] Both paths null, fallback to persistentDataPath/Saves: '{savesContainerPath}'");
                }
            }

            string normalizedContainer = NormalizePath(savesContainerPath);
            bool verbose = !_hasLoggedFirstScan || Time.realtimeSinceStartup - _lastLogTime > 30f;
            if (verbose)
                MelonLogger.Msg($"[MoreSaveSlots] Refreshing save info for {totalSlots} slots — raw='{savesContainerPath}' normalized='{normalizedContainer}' (prefix='{SaveManager.SAVE_GAME_PREFIX}')");

            // Diagnostic: only on first scan or when empty, to avoid per-frame IO spam
            if (!_hasLoggedFirstScan)
            {
                try
                {
                    bool containerExists = Directory.Exists(normalizedContainer);
                    if (verbose) MelonLogger.Msg($"[MoreSaveSlots] Container exists={containerExists}: '{normalizedContainer}'");
                    if (containerExists)
                    {
                        var dirs = Directory.GetDirectories(normalizedContainer);
                        if (verbose) MelonLogger.Msg($"[MoreSaveSlots] Found {dirs.Length} subdirs.");
                        if (dirs.Length == 0)
                        {
                            MelonLogger.Warning($"[MoreSaveSlots] Container is empty! Backups in '{Path.Combine(normalizedContainer, "backups")}'");
                        }
                    }
                    else
                    {
                        MelonLogger.Warning($"[MoreSaveSlots] Container does NOT exist: '{normalizedContainer}'");
                        if (!string.IsNullOrEmpty(fallbackPlayers) && Directory.Exists(NormalizePath(fallbackPlayers)))
                        {
                            savesContainerPath = fallbackPlayers;
                            normalizedContainer = NormalizePath(savesContainerPath);
                            MelonLogger.Msg($"[MoreSaveSlots] Switching to PlayersSavePath: '{normalizedContainer}'");
                        }
                        else
                        {
                            string generic = NormalizePath(Path.Combine(Application.persistentDataPath, "Saves"));
                            if (Directory.Exists(generic))
                            {
                                var subDirs = Directory.GetDirectories(generic);
                                if (subDirs.Length == 1 && Directory.Exists(Path.Combine(subDirs[0], "backups")))
                                {
                                    savesContainerPath = subDirs[0];
                                    normalizedContainer = NormalizePath(savesContainerPath);
                                    MelonLogger.Msg($"[MoreSaveSlots] Auto-detected profile: '{normalizedContainer}'");
                                }
                                else
                                {
                                    savesContainerPath = generic;
                                    normalizedContainer = generic;
                                    MelonLogger.Msg($"[MoreSaveSlots] Switching to generic Saves: '{normalizedContainer}'");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[MoreSaveSlots] Diagnostic failed: {ex.Message}");
                }
            }

            var saveArray = new Il2CppReferenceArray<SaveInfo>(totalSlots);
            SaveInfo? newestSave = null;
            int foundCount = 0;

            for (int i = 0; i < totalSlots; i++)
            {
                int slotNumber = i + 1;
                string slotDirName = $"{SaveManager.SAVE_GAME_PREFIX}{slotNumber}";
                string slotFolderPath = Path.Combine(normalizedContainer, slotDirName);
                // Also try normalized version explicitly
                string slotFolderNorm = NormalizePath(slotFolderPath);

                SaveInfo? info = null;
                bool exists = Directory.Exists(slotFolderNorm);
                // Also check raw combined path in case of slash issues
                if (!exists && slotFolderPath != slotFolderNorm)
                    exists = Directory.Exists(slotFolderPath);

                if (exists)
                {
                    string tryPath = Directory.Exists(slotFolderNorm) ? slotFolderNorm : slotFolderPath;
                    try
                    {
                        // Pass the 1-based slot number (matches folder SaveGame_{slotNumber}); vanilla stores
                        // this value verbatim in SaveInfo.SaveSlotNumber, which the rename/delete guards
                        // compare against ActiveSaveInfo.SaveSlotNumber.
                        bool loaded = LoadManager.TryLoadSaveInfo(tryPath, slotNumber, out info, false);
                        if (verbose && loaded && info != null)
                            MelonLogger.Msg($"[MoreSaveSlots] Slot {slotNumber}: '{info.OrganisationName}' v{info.SaveVersion} {info.DateLastPlayed:yyyy-MM-dd HH:mm}");
                        else if (!loaded && _hasLoggedFirstScan == false)
                            MelonLogger.Msg($"[MoreSaveSlots] Slot {slotNumber}: exists but TryLoad failed (requireGameFile=false)");

                        if (loaded && info != null)
                        {
                            foundCount++;
                            if (newestSave == null || info.DateLastPlayed > newestSave.DateLastPlayed)
                                newestSave = info;
                        }
                        else if (exists && !loaded && !_hasLoggedFirstScan)
                        {
                            bool loaded2 = LoadManager.TryLoadSaveInfo(tryPath, slotNumber, out var info2, true);
                            if (loaded2 && info2 != null)
                            {
                                info = info2;
                                foundCount++;
                                if (newestSave == null || info.DateLastPlayed > newestSave.DateLastPlayed)
                                    newestSave = info;
                            }
                            else
                            {
                                try
                                {
                                    var files = Directory.GetFiles(tryPath);
                                    if (files.Length == 0)
                                        MelonLogger.Warning($"[MoreSaveSlots] Slot {slotNumber} folder empty.");
                                    else if (!files.Any(f => Path.GetFileName(f).Equals("Game.json", StringComparison.OrdinalIgnoreCase)))
                                        MelonLogger.Warning($"[MoreSaveSlots] Slot {slotNumber} Game.json missing — corrupted.");
                                }
                                catch (Exception ex2) { if (!_hasLoggedFirstScan) MelonLogger.Warning($"[MoreSaveSlots] List files failed: {ex2.Message}"); }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!_hasLoggedFirstScan) MelonLogger.Warning($"[MoreSaveSlots] Slot {slotNumber}: TryLoad threw: {ex.Message}");
                    }
                }

                saveArray[i] = info;
            }

            LoadManager.SaveGames = saveArray;
            LoadManager.LastPlayedGame = newestSave;

            bool shouldLogSummary = !_hasLoggedFirstScan || foundCount != _lastFoundCount || Time.realtimeSinceStartup - _lastLogTime > 30f;
            if (shouldLogSummary)
            {
                MelonLogger.Msg($"[MoreSaveSlots] Scan complete: {foundCount}/{totalSlots} saves loaded. LastPlayed={(newestSave != null ? $"Slot {newestSave.SaveSlotNumber} '{newestSave.OrganisationName}'" : "none")}");
                if (foundCount == 0 && !_hasLoggedFirstScan)
                {
                    MelonLogger.Warning($"[MoreSaveSlots] No saves found! Check '{normalizedContainer}' for '{SaveManager.SAVE_GAME_PREFIX}N' folders.");
                }
                _lastLogTime = Time.realtimeSinceStartup;
                _lastFoundCount = foundCount;
            }
            _hasLoggedFirstScan = true;

            // Notify S1API / listeners — wrapped in inner try/catch so an exception here
            // does NOT short-circuit to the outer catch (which would return true and let vanilla
            // fire its own onSaveInfoLoaded → double-fire). Same-frame guard protects against
            // external mods also patching RefreshSaveInfo.
            if (__instance != null && __instance.onSaveInfoLoaded != null)
            {
                int currentFrame = Time.frameCount;
                if (currentFrame != _lastInvokeFrame)
                {
                    _lastInvokeFrame = currentFrame;
                    try
                    {
                        __instance.onSaveInfoLoaded.Invoke();
                    }
                    catch (Exception invokeEx)
                    {
                        MelonLogger.Warning($"[MoreSaveSlots] onSaveInfoLoaded.Invoke() threw, continuing without double-fire: {invokeEx.Message}");
                    }
                }
                else
                {
                    MelonLogger.Msg($"[MoreSaveSlots] Suppressed duplicate onSaveInfoLoaded.Invoke() within frame {currentFrame}.");
                }
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BusinessIncome.Models;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using S1Mods.Shared;

namespace BusinessIncome.Services;

/// <summary>
/// Persists and manages the payout state per savegame slot.
/// Protects against double payouts on the same in-game day.
/// </summary>
public static class PayoutStateStore
{
    private static PayoutState? _cachedState;
    private static string _currentSlotSuffix = "default";
    private static string _lastKnownSlot = "default";
    private static int _pendingPrevLastPaid = -1;
    private static Dictionary<string, int?> _pendingPrevPerBusiness = new();

    /// <summary>
    /// Determines the save-slot suffix of the active save file.
    /// </summary>
    public static string GetActiveSlotSuffix()
    {
        string slotSuffix = "default";
        bool resolved = false;
        try
        {
            var loadMgr = PersistentSingleton<LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
            {
                var saveInfo = loadMgr.ActiveSaveInfo;
                if (saveInfo != null && saveInfo.Pointer != IntPtr.Zero && !saveInfo.WasCollected)
                {
                    if (saveInfo.SaveSlotNumber >= 0)
                    {
                        slotSuffix = $"slot_{saveInfo.SaveSlotNumber}";
                        resolved = true;
                    }
                    else if (!string.IsNullOrEmpty(saveInfo.SavePath))
                    {
                        slotSuffix = Path.GetFileName(saveInfo.SavePath);
                        resolved = true;
                    }
                }
            }
        }
        catch { }

        if (resolved)
        {
            _lastKnownSlot = slotSuffix;
        }
        else if (!string.IsNullOrEmpty(_lastKnownSlot))
        {
            return _lastKnownSlot;
        }

        return slotSuffix;
    }

    /// <summary>
    /// Returns the full path to the state file of the current save file.
    /// </summary>
    public static string GetStateFilePath()
    {
        string slotSuffix = GetActiveSlotSuffix();
        _currentSlotSuffix = slotSuffix;
        string dir = SafeStorage.GetUserDataPath("BusinessIncome");
        string path = Path.Combine(dir, $"payout_state_{slotSuffix}.json");
        TryMigrateLegacy(path, slotSuffix);
        return path;
    }

    private static void TryMigrateLegacy(string slotPath, string slotSuffix)
    {
        try
        {
            // Legacy pre-slot file: payout_state.json and payout_state_default.json
            string[] legacyCandidates = new[]
            {
                SafeStorage.GetUserDataPath("BusinessIncome", "payout_state.json"),
                SafeStorage.GetUserDataPath("BusinessIncome", "payout_state_default.json")
            };
            foreach (var legacy in legacyCandidates)
            {
                if (!File.Exists(legacy)) continue;
                if (File.Exists(slotPath))
                {
                    try { File.Delete(legacy); } catch { }
                    continue;
                }
                try { File.Move(legacy, slotPath); } catch { }
                break;
            }
        }
        catch { }
    }

    /// <summary>
    /// Loads the current state for the active slot or creates a new one.
    /// </summary>
    public static PayoutState GetState()
    {
        string slotSuffix = GetActiveSlotSuffix();
        if (_cachedState != null && _currentSlotSuffix == slotSuffix)
        {
            return _cachedState;
        }

        string path = GetStateFilePath();
        PayoutState fallback = new()
        {
            SaveIdentity = slotSuffix,
            LastPaidElapsedDay = -1
        };

        PayoutState state = SafeStorage.LoadSafe<PayoutState>(path, fallback, Mod.Log);

        _cachedState = state;
        _currentSlotSuffix = slotSuffix;
        return state;
    }

    /// <summary>
    /// Checks whether the given in-game day has already been paid out.
    /// </summary>
    public static bool IsDayPaid(int elapsedDay)
    {
        var state = GetState();
        return state.LastPaidElapsedDay >= elapsedDay;
    }

    /// <summary>
    /// Checks whether a specific business has already been paid on that day.
    /// </summary>
    public static bool IsBusinessPaid(string businessId, int elapsedDay)
    {
        var state = GetState();
        if (state.LastPaidDayByBusiness.TryGetValue(businessId, out int lastDay))
        {
            return lastDay >= elapsedDay;
        }
        return false;
    }

    /// <summary>
    /// Marks the day and the given businesses as paid IN MEMORY ONLY (no disk write).
    /// Called BEFORE the bank transaction to prevent double-payouts on retry loops.
    /// Snapshots previous values so Revert can restore them without losing history.
    /// </summary>
    public static void MarkInMemoryPaid(int elapsedDay, IEnumerable<string> paidBusinessIds)
    {
        var state = GetState();
        _pendingPrevLastPaid = state.LastPaidElapsedDay;
        _pendingPrevPerBusiness.Clear();
        var ids = paidBusinessIds.ToList();
        foreach (var id in ids)
        {
            if (state.LastPaidDayByBusiness.TryGetValue(id, out int prev))
                _pendingPrevPerBusiness[id] = prev;
            else
                _pendingPrevPerBusiness[id] = null;
        }

        state.LastPaidElapsedDay = elapsedDay;
        state.SaveIdentity = GetActiveSlotSuffix();
        state.LastPayoutTimestamp = DateTime.UtcNow.ToString("o");

        foreach (var id in ids)
        {
            state.LastPaidDayByBusiness[id] = elapsedDay;
        }
    }

    /// <summary>
    /// Reverts the in-memory paid-marker after a failed transaction so the payout can retry.
    /// Restores the snapshotted previous day/business values instead of wiping to -1.
    /// </summary>
    public static void RevertInMemoryPaid(int elapsedDay, IEnumerable<string> paidBusinessIds)
    {
        var state = GetState();
        if (state.LastPaidElapsedDay == elapsedDay)
        {
            state.LastPaidElapsedDay = _pendingPrevLastPaid;
        }
        foreach (var id in paidBusinessIds)
        {
            if (state.LastPaidDayByBusiness.TryGetValue(id, out int lastDay) && lastDay == elapsedDay)
            {
                if (_pendingPrevPerBusiness.TryGetValue(id, out var prev) && prev.HasValue)
                    state.LastPaidDayByBusiness[id] = prev.Value;
                else
                    state.LastPaidDayByBusiness.Remove(id);
            }
        }
        _pendingPrevPerBusiness.Clear();
    }

    /// <summary>
    /// Marks the day and the given businesses as successfully paid and saves atomically.
    /// </summary>
    public static bool CommitPayout(int elapsedDay, IEnumerable<string> paidBusinessIds)
    {
        try
        {
            var state = GetState();
            state.LastPaidElapsedDay = elapsedDay;
            state.SaveIdentity = GetActiveSlotSuffix();
            state.LastPayoutTimestamp = DateTime.UtcNow.ToString("o");

            foreach (var id in paidBusinessIds)
            {
                state.LastPaidDayByBusiness[id] = elapsedDay;
            }

            string path = GetStateFilePath();
            bool success = SafeStorage.SaveAtomic(path, state, Mod.Log);
            if (success)
            {
                Mod.Log.Info($"PayoutState for day {elapsedDay} saved to {Path.GetFileName(path)}.");
                _pendingPrevPerBusiness.Clear();
            }
            return success;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to save PayoutState: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Resets the in-memory cache (e.g. on scene unload).
    /// </summary>
    public static void Reset(bool keepSlot = false)
    {
        _cachedState = null;
        _currentSlotSuffix = "default";
        _pendingPrevPerBusiness.Clear();
        _pendingPrevLastPaid = -1;
        if (!keepSlot) _lastKnownSlot = "default";
    }

    public static void ResetForSceneUnload()
    {
        Reset(keepSlot: true);
    }
}

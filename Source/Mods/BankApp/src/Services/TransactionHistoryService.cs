using System;
using System.Collections.Generic;
using System.IO;
using BankApp.Models;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using S1Mods.Shared;

namespace BankApp.Services;

/// <summary>
/// Manages save-slot isolated bank history and weekly ATM deposit limits.
/// Stored under UserData/BankApp/bank_slot_{slotId}.json with atomic backup protection.
/// </summary>
public static class TransactionHistoryService
{
    private static BankState? _activeState;
    private static string? _lastResolvedSlot;
    private static string _lastKnownSlot = "default";

    public static event Action? OnHistoryChanged;

    public static string GetSaveSlotSuffix()
    {
        string slotSuffix = "default";
        bool resolved = false;
        try
        {
            var loadMgr = PersistentSingleton<LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero)
            {
                var saveInfo = loadMgr.ActiveSaveInfo;
                if (saveInfo != null && saveInfo.Pointer != IntPtr.Zero)
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
            // No active save info (e.g. additive UI scenes): keep the last known slot
            // so we never fall back to writing the global 'default' file mid-session.
            return _lastKnownSlot;
        }

        return slotSuffix;
    }

    private static string GetFilePath()
    {
        string slot = GetSaveSlotSuffix();
        string dir = SafeStorage.GetUserDataPath("BankApp");
        string path = Path.Combine(dir, $"bank_{slot}.json");
        TryMigrateLegacy(path, slot);
        return path;
    }

    private static void TryMigrateLegacy(string slotPath, string slotSuffix)
    {
        try
        {
            // Legacy global files: bank.json and bank_default.json (pre-slot isolation)
            string[] legacyCandidates = new[]
            {
                SafeStorage.GetUserDataPath("BankApp", "bank.json"),
                SafeStorage.GetUserDataPath("BankApp", "bank_default.json")
            };
            foreach (var legacy in legacyCandidates)
            {
                if (!File.Exists(legacy)) continue;
                if (File.Exists(slotPath))
                {
                    try { File.Delete(legacy); } catch { }
                    continue;
                }
                // Only migrate if legacy file has content
                try { File.Move(legacy, slotPath); } catch { }
                break;
            }
        }
        catch { }
    }

    public static BankState GetActiveState()
    {
        string currentSlot = GetSaveSlotSuffix();
        if (_activeState != null && _lastResolvedSlot == currentSlot)
        {
            return _activeState;
        }

        _lastResolvedSlot = currentSlot;
        string path = GetFilePath();

        _activeState = SafeStorage.LoadSafe<BankState>(path, new BankState(), Mod.Log);
        return _activeState;
    }

    public static void SaveActiveState()
    {
        if (_activeState == null) return;
        string path = GetFilePath();
        SafeStorage.SaveAtomic(path, _activeState, Mod.Log);
        OnHistoryChanged?.Invoke();
    }

    public static IReadOnlyList<BankTransaction> GetTransactions()
    {
        var state = GetActiveState();
        return state.Transactions;
    }

    public static void AddTransaction(BankTransaction transaction)
    {
        var state = GetActiveState();
        state.Transactions.Insert(0, transaction);

        // Keep last 150 transactions
        if (state.Transactions.Count > 150)
        {
            state.Transactions.RemoveRange(150, state.Transactions.Count - 150);
        }

        SaveActiveState();
    }

    public static float GetWeeklyDeposits(int currentWeek)
    {
        var state = GetActiveState();
        if (state.LastRecordedWeek != currentWeek)
        {
            // Read-only check should not mutate disk. Only return 0 for new week;
            // actual reset is done in RecordWeeklyDeposit which is the mutating path.
            return 0f;
        }
        return state.WeeklyDepositedAmount;
    }

    public static void RecordWeeklyDeposit(float amount, int currentWeek)
    {
        var state = GetActiveState();
        if (state.LastRecordedWeek != currentWeek)
        {
            state.LastRecordedWeek = currentWeek;
            state.WeeklyDepositedAmount = 0f;
        }
        state.WeeklyDepositedAmount += amount;
        SaveActiveState();
    }

    public static void ResetCache(bool keepSlot = false)
    {
        _activeState = null;
        _lastResolvedSlot = null;
        if (!keepSlot) _lastKnownSlot = "default";
    }

    public static void ResetForSceneUnload()
    {
        // Keep lastKnownSlot across Main unload so additive UI scenes don't fall back to default
        ResetCache(keepSlot: true);
    }
}

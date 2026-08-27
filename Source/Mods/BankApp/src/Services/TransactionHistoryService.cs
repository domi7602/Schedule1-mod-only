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
                        // H4: Normalize fallback — extract slot number from path, don't use "Save.json"
                        string file = Path.GetFileNameWithoutExtension(saveInfo.SavePath) ?? "";
                        var m = System.Text.RegularExpressions.Regex.Match(file, @"\d+");
                        if (m.Success) slotSuffix = $"slot_{m.Value}";
                        else
                        {
                            string dir = Path.GetFileName(Path.GetDirectoryName(saveInfo.SavePath) ?? "");
                            var m2 = System.Text.RegularExpressions.Regex.Match(dir, @"\d+");
                            slotSuffix = m2.Success ? $"slot_{m2.Value}" : $"slot_{file}";
                        }
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
                try
                {
                    var legacyState = SafeStorage.LoadSafe<BankState>(legacy, null, Mod.Log);
                    if (legacyState != null && legacyState.Transactions != null && legacyState.Transactions.Count > 0)
                    {
                        if (SafeStorage.SaveAtomic(slotPath, legacyState, Mod.Log))
                        {
                            try { File.Delete(legacy); } catch { }
                            Mod.Log?.Info($"Migrated legacy {Path.GetFileName(legacy)} -> {Path.GetFileName(slotPath)}");
                            break;
                        }
                    }
                    // Fallback for empty or load failure: copy raw
                    SafeStorage.EnsureDirectoryForFile(slotPath);
                    File.Copy(legacy, slotPath, true);
                    try { File.Delete(legacy); } catch { }
                    Mod.Log?.Info($"Migrated legacy {Path.GetFileName(legacy)} -> {Path.GetFileName(slotPath)} (copy)");
                    break;
                }
                catch (Exception ex) { Mod.Log?.Warn($"Legacy migrate {Path.GetFileName(legacy)} failed: {ex.Message}"); }
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
        // H3: Do NOT Save here — caller (BankService.DepositCash) will do single SaveAtomic via AddTransaction
        // This prevents double-write desync where weekly amount persisted but transaction not
    }

    internal static void RecordWeeklyDepositAndSave(float amount, int currentWeek)
    {
        RecordWeeklyDeposit(amount, currentWeek);
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

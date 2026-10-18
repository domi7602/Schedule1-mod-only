using System;
using System.IO;
using BusinessIncome.Core;
using S1API.Money;
using S1Mods.Shared;
using S1MoneyManager = Il2CppScheduleOne.Money.MoneyManager;

namespace BusinessIncome.Services;

/// <summary>
/// Real filesystem implementation of the pure <see cref="IPayoutStorage"/> seam.
/// Uses S1Mods.Shared.SafeStorage.SaveTextAtomic so writes keep the shared
/// ".tmp -> .bak -> final" atomic convention that the codec's marker clearing expects.
/// </summary>
public sealed class PayoutRuntimeStorage : IPayoutStorage
{
    public bool Exists(string path)
    {
        try { return File.Exists(path); } catch { return false; }
    }

    public bool BackupExists(string path)
    {
        try { return File.Exists(path + ".bak"); } catch { return false; }
    }

    public string? ReadText(string path)
    {
        try { return File.Exists(path) ? File.ReadAllText(path) : null; }
        catch { return null; }
    }

    public bool WriteAtomic(string path, string content)
    {
        try { return SafeStorage.SaveTextAtomic(path, content, Mod.Log); }
        catch { return false; }
    }

    public bool DeleteFile(string path)
    {
        try
        {
            if (File.Exists(path)) File.Delete(path);
            return !File.Exists(path);
        }
        catch { return false; }
    }
}

/// <summary>
/// Real bank seam over S1API's Money facade. A clean return means the request was
/// accepted by the money manager - it is NOT independent confirmation of settlement.
/// </summary>
public sealed class MoneyPayoutBank : IPayoutBank
{
    public bool IsReady()
    {
        try
        {
            // S1API's Money.GetOnlineBalance() returns 0f - it does NOT throw - when
            // MoneyManager.Instance is null, so calling it is a FALSE-POSITIVE readiness check.
            // Guard the live singleton directly: no manager, no payout.
            var mgr = S1MoneyManager.Instance;
            if (mgr == null || mgr.Pointer == IntPtr.Zero || mgr.WasCollected) return false;
            return true;
        }
        catch { return false; }
    }

    public bool RequestTransfer(string transactionName, float unitAmount, string note)
    {
        Money.CreateOnlineTransaction(transactionName, unitAmount, 1f, note);
        return true;
    }
}

/// <summary>Canonical per-slot payout paths.</summary>
public static class PayoutPaths
{
    public static string Directory => SafeStorage.GetUserDataPath("BusinessIncome");

    public static string State(string slot) => Path.Combine(Directory, $"payout_state_{slot}.json");

    public static string Pending(string slot) => Path.Combine(Directory, $"payout_pending_{slot}.json");

    /// <summary>Legacy (pre-slot) state file candidates - never created, only inspected.</summary>
    public static string[] LegacyStateCandidates => new[]
    {
        SafeStorage.GetUserDataPath("BusinessIncome", "payout_state.json"),
        SafeStorage.GetUserDataPath("BusinessIncome", "payout_state_default.json")
    };
}

using System;
using System.IO;
using System.Security.Cryptography;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;

namespace TownHallInterior;

public sealed class NativePurchasePaymentPort : IPurchasePaymentPort
{
    public float GetOnlineBalance() => TownHallNativeMoney.ReadBalance(bank: true);
    public float GetCashBalance() => TownHallNativeMoney.ReadBalance(bank: false);

    public MoneyMutationOutcome DeductBank(float amount, string label, string note) =>
        TownHallNativeMoney.ChangeBalance(-amount, bank: true, label, note);

    public MoneyMutationOutcome DeductCash(float amount) =>
        TownHallNativeMoney.ChangeBalance(-amount, bank: false, playCashSound: true);

    public MoneyMutationOutcome RefundBank(float amount, string label, string note) =>
        TownHallNativeMoney.ChangeBalance(amount, bank: true, label, note);

    public MoneyMutationOutcome RefundCash(float amount) =>
        TownHallNativeMoney.ChangeBalance(amount, bank: false);
}

internal static class TownHallNativeMoney
{
    private const string AuditedGameAssembly =
        "61829050E56CA180D67BE592E4BFE61EC97D2E9573F720D90A54E318A7E4381D";

    public static bool MatchesAuditedGameAssembly()
    {
        using var nativeCode = File.OpenRead(Path.Combine(Path.GetDirectoryName(Application.dataPath)!, "GameAssembly.dll"));
        using var sha256 = SHA256.Create();
        return Convert.ToHexString(sha256.ComputeHash(nativeCode)) == AuditedGameAssembly;
    }

    public static bool TryReadWallet(out float bank, out float cash)
    {
        bank = cash = 0f;
        try
        {
            var manager = ReadyMoneyManager();
            if (manager == null) return false;
            bank = manager.onlineBalance;
            cash = manager.cashBalance;
            return float.IsFinite(bank) && float.IsFinite(cash);
        }
        catch
        {
            return false;
        }
    }

    public static float ReadBalance(bool bank) =>
        TryReadWallet(out float online, out float cash) ? (bank ? online : cash) : 0f;

    public static MoneyMutationOutcome ChangeBalance(float delta, bool bank,
        string label = "", string note = "", bool playCashSound = false)
    {
        MoneyManager? manager;
        float expected;
        try
        {
            manager = ReadyMoneyManager();
            if (manager == null || !NativeWorldPort.IsLocalSingleplayer() || !float.IsFinite(delta) || delta == 0f)
                return MoneyMutationOutcome.NotApplied;

            float before = bank ? manager.onlineBalance : manager.cashBalance;
            expected = before + delta;
            if (!float.IsFinite(expected) || expected == before || (delta < 0f && expected < 0f) ||
                (!bank && expected > 1_000_000_000f))
                return MoneyMutationOutcome.NotApplied;
        }
        catch
        {
            return MoneyMutationOutcome.NotApplied;
        }

        try
        {
            if (bank)
            {
                // CreateOnlineTransaction uses deferred FishNet RPCs. In the confirmed local
                // host world, execute the f12 receipt body once for the ledger, SyncVar and HUD.
                manager.RpcLogic___ReceiveOnlineTransaction_1419830531(label, delta, 1f, note);
            }
            else
            {
                manager.ChangeCashBalance(delta, visualizeChange: true, playCashSound);
            }
        }
        catch (Exception ex)
        {
            // UI or another mod can throw after the balance changed. Read it back either way.
            MelonLogger.Warning($"TownHallInterior wallet callback failed: {ex.Message}");
        }

        try
        {
            var current = ReadyMoneyManager();
            if (current != null && current.Pointer == manager.Pointer &&
                (bank ? current.onlineBalance == expected && current.syncVar___onlineBalance.GetValue(calledByUser: true) == expected
                    : current.cashBalance == expected))
                return MoneyMutationOutcome.Applied;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallInterior wallet readback failed: {ex.Message}");
        }
        MelonLogger.Warning("TownHallInterior wallet outcome could not be confirmed. The operation is held for reconciliation.");
        return MoneyMutationOutcome.Unknown;
    }

    private static MoneyManager? ReadyMoneyManager()
    {
        var manager = MoneyManager.Instance;
        var inventory = PlayerInventory.Instance;
        return NetworkGuard.IsAlive(manager) && manager!.IsServerInitialized &&
            manager.syncVar___onlineBalance != null &&
            NetworkGuard.IsAlive(inventory) && inventory!.cashInstance != null
            ? manager : null;
    }
}

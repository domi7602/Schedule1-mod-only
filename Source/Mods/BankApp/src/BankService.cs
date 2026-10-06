using System;
using BankApp.Config;
using BankApp.Logic;
using BankApp.Models;
using Il2CppScheduleOne.DevUtilities;
using S1API.GameTime;
using S1Mods.Shared;
using UnityEngine;
using BankApp;

namespace BankApp.Services;

/// <summary>
/// Core banking service executing limit-checked money operations.
/// </summary>
public static class BankService
{
    public const float VanillaWeeklyAtmLimit = 10000f;

    public static float GetOnlineBalance() => EconomyHelper.GetOnlineBalance();
    public static float GetCashBalance() => EconomyHelper.GetCashBalance();
    public static float GetNetWorth() => EconomyHelper.GetNetWorth();

    public static int GetCurrentInGameDay()
    {
        try
        {
            return TimeManager.ElapsedDays;
        }
        catch
        {
            return 1;
        }
    }

    public static int GetCurrentInGameWeek()
    {
        return GetCurrentInGameDay() / 7;
    }

    public static string GetCurrentInGameTimeString()
    {
        try
        {
            return TimeManager.GetFormatted12HourTime();
        }
        catch
        {
            return DateTime.Now.ToString("hh:mm tt");
        }
    }

    public static int GetFreeInventorySlotsCount() => EconomyHelper.GetFreeInventorySlotsCount();

    public static float GetCashInDedicatedCashSlot() => EconomyHelper.GetCashInDedicatedCashSlot();

    public static float GetMaxWithdrawableCash()
    {
        // No inventory-capacity term: the engine cash path is slot-free.
        // MoneyManager keeps the balance as a float plus a single CashInstance
        // (arbitrary SetQuantity — S1API Money.CreateCashInstance relies on it),
        // with no hotbar involvement. The old slot model (freeSlots x
        // cash-StackLimit) collapsed because vanilla cash StackLimit is 1,
        // which wrongly limited withdrawals to ~$1 per free slot.
        // Fee-aware: a withdrawal is debited gross + fee, so the maximum must leave
        // room for the fee (TransferMath.MaxWithdraw solves A + fee(A) <= balance;
        // with no fee it is simply the whole balance).
        float onlineBalance = GetOnlineBalance();
        float feePercent = ModConfig<BankAppConfig>.Instance.ServiceFeePercent;

        return TransferMath.MaxWithdraw(onlineBalance, feePercent);
    }

    public static float GetRemainingWeeklyAtmLimit()
    {
        if (!ModConfig<BankAppConfig>.Instance.RespectVanillaAtmLimit)
        {
            return float.MaxValue;
        }

        int currentWeek = GetCurrentInGameWeek();
        float depositedThisWeek = TransactionHistoryService.GetWeeklyDeposits(currentWeek);
        return Mathf.Max(0f, VanillaWeeklyAtmLimit - depositedThisWeek);
    }

    public static float GetMaxDepositableCash()
    {
        float cashOnHand = GetCashBalance();
        bool limitEnabled = ModConfig<BankAppConfig>.Instance.RespectVanillaAtmLimit;
        float remainingLimit = limitEnabled ? GetRemainingWeeklyAtmLimit() : float.MaxValue;
        return TransferMath.MaxDeposit(cashOnHand, remainingLimit, limitEnabled);
    }

    public static bool DepositCash(float amount, out string errorMessage)
    {
        errorMessage = string.Empty;
        Mod.Log?.Info($"[Bank] Deposit request: amount={amount:0.##}");
        // Bug-Audit 2026-09-12 (Round 3): gate money mutations behind a scene guard.
        // The UI is normally only reachable while the player is on Main, but a leftover
        // hotkey, scene change mid-call, or programmatic invocation could otherwise
        // move money outside of gameplay state. Defense in depth — refuse early with a
        // clean error instead of risking a partial transfer.
        if (!NetworkGuard.IsInMainScene)
        {
            errorMessage = "Bank not available outside the game scene.";
            BankSoundService.PlayError();
            return false;
        }
        if (amount <= 0f)
        {
            errorMessage = "Invalid amount.";
            BankSoundService.PlayError();
            return false;
        }

        var config = ModConfig<BankAppConfig>.Instance;
        bool limitEnabled = config.RespectVanillaAtmLimit;
        float remainingLimit = limitEnabled ? GetRemainingWeeklyAtmLimit() : float.MaxValue;

        // Shared seam: the live UI preview runs the exact same validation, so a quote
        // the button accepted cannot be recomputed differently at execution time.
        // This also rejects non-finite amounts and applies the preserved float fee formula.
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Deposit, amount, GetCashBalance(), GetOnlineBalance(),
            config.ServiceFeePercent, limitEnabled, remainingLimit);

        if (!quote.IsValid)
        {
            errorMessage = quote.ErrorMessage;
            Mod.Log?.Warn($"[Bank] Deposit rejected: {quote.ErrorMessage}");
            BankSoundService.PlayError();
            return false;
        }

        float fee = quote.Fee;
        float netCredited = quote.Net;

        // Step 1: deduct physical cash (must succeed before bank credit)
        try
        {
            EconomyHelper.ChangeCashBalance(-amount, true, false);
        }
        catch (Exception ex)
        {
            errorMessage = "Transaction failed: cash not deducted.";
            BankSoundService.PlayError();
            MelonLoader.MelonLogger.Error($"DepositCash cash deduct failed: {ex.GetType().Name}: {ex.Message}");
            return false;
        }

        // Step 2: credit bank — refund cash only if THIS step fails
        try
        {
            EconomyHelper.CreateOnlineTransaction("ATM Deposit", netCredited, 1f, "Mobile ATM Cash Deposit");
        }
        catch (Exception ex)
        {
            try { EconomyHelper.ChangeCashBalance(+amount, false, false); } catch (Exception ex2) { MelonLoader.MelonLogger.Error($"CRITICAL Deposit refund failed (money at risk): {ex2}"); }
            errorMessage = "Transaction failed: cash refunded.";
            BankSoundService.PlayError();
            MelonLoader.MelonLogger.Error($"DepositCash rollback: {ex.GetType().Name}: {ex.Message}");
            return false;
        }

        int currentDay = GetCurrentInGameDay();
        string timeStr = GetCurrentInGameTimeString();
        float balanceAfter = GetOnlineBalance();

        if (config.RespectVanillaAtmLimit)
        {
            TransactionHistoryService.RecordWeeklyDeposit(amount, GetCurrentInGameWeek());
        }

        TransactionHistoryService.AddTransaction(new BankTransaction
        {
            InGameDay = currentDay,
            InGameTime = timeStr,
            Amount = netCredited,
            Type = TransactionType.Deposit,
            Description = "Mobile Cash Deposit",
            BalanceAfter = balanceAfter,
            Gross = amount,
            Fee = fee
        });

        BankSoundService.PlayCashSuccess();
        Mod.Log?.Info($"[Bank] Deposit ok: -{amount:0.##} cash, +{netCredited:0.##} bank.");
        return true;
    }

    public static bool WithdrawCash(float amount, out string errorMessage)
    {
        errorMessage = string.Empty;
        Mod.Log?.Info($"[Bank] Withdraw request: amount={amount:0.##}");
        // Bug-Audit 2026-09-12 (Round 3): see DepositCash — guard against outside-Main calls.
        if (!NetworkGuard.IsInMainScene)
        {
            Mod.Log?.Warn("[Bank] Withdraw rejected: not in Main scene.");
            errorMessage = "Bank not available outside the game scene.";
            BankSoundService.PlayError();
            return false;
        }
        if (amount <= 0f)
        {
            Mod.Log?.Warn($"[Bank] Withdraw rejected: invalid amount ({amount:0.##}).");
            errorMessage = "Invalid amount.";
            BankSoundService.PlayError();
            return false;
        }

        var config = ModConfig<BankAppConfig>.Instance;

        // Same shared seam as the deposit path and the UI preview.
        TransferQuote quote = TransferMath.ComputeQuote(
            TransferDirection.Withdraw, amount, GetCashBalance(), GetOnlineBalance(),
            config.ServiceFeePercent, false, 0f);

        if (!quote.IsValid)
        {
            errorMessage = quote.ErrorMessage;
            Mod.Log?.Warn($"[Bank] Withdraw rejected: {quote.ErrorMessage}");
            BankSoundService.PlayError();
            return false;
        }

        float fee = quote.Fee;
        float totalDeducted = quote.Debit;

        // NOTE: deliberately no inventory-capacity gate. The engine cash path
        // (MoneyManager.ChangeCashBalance -> cashBalance float + single
        // CashInstance) never touches hotbar slots, so a full inventory
        // cannot destroy withdrawn cash. The old freeSlots x StackLimit
        // model was fiction (vanilla cash StackLimit is 1) and wrongly
        // rejected legit withdrawals with "Inventory full".

        // Step 1: debit bank (must succeed before cash spawn)
        try
        {
            EconomyHelper.CreateOnlineTransaction("ATM Withdrawal", -totalDeducted, 1f, "Mobile ATM Cash Withdrawal");
        }
        catch (Exception ex)
        {
            errorMessage = "Transaction failed: bank not debited.";
            BankSoundService.PlayError();
            MelonLoader.MelonLogger.Error($"WithdrawCash bank debit failed: {ex.GetType().Name}: {ex.Message}");
            return false;
        }

        // Step 2: spawn physical cash — refund bank only if THIS step fails
        try
        {
            EconomyHelper.ChangeCashBalance(+amount, true, false);
        }
        catch (Exception ex)
        {
            try { EconomyHelper.CreateOnlineTransaction("ATM Withdrawal Refund", +totalDeducted, 1f, "Rollback"); } catch (Exception ex2) { MelonLoader.MelonLogger.Error($"CRITICAL Withdraw refund failed (money at risk): {ex2}"); }
            errorMessage = "Transaction failed: bank refunded.";
            BankSoundService.PlayError();
            MelonLoader.MelonLogger.Error($"WithdrawCash rollback: {ex.GetType().Name}: {ex.Message}");
            return false;
        }

        int currentDay = GetCurrentInGameDay();
        string timeStr = GetCurrentInGameTimeString();
        float balanceAfter = GetOnlineBalance();

        TransactionHistoryService.AddTransaction(new BankTransaction
        {
            InGameDay = currentDay,
            InGameTime = timeStr,
            // Actual bank delta for new entries: gross + fee debited (Gross/Fee record the split).
            Amount = -totalDeducted,
            Type = TransactionType.Withdrawal,
            Description = "Mobile Cash Withdrawal",
            BalanceAfter = balanceAfter,
            Gross = amount,
            Fee = fee
        });

        BankSoundService.PlayCashSuccess();
        Mod.Log?.Info($"[Bank] Withdraw ok: -{totalDeducted:0.##} bank, +{amount:0.##} cash.");
        return true;
    }
}

using System;
using BankApp.Config;
using BankApp.Models;
using Il2CppScheduleOne.DevUtilities;
using S1API.GameTime;
using S1Mods.Shared;
using UnityEngine;

namespace BankApp.Services;

/// <summary>
/// Core banking service executing slot-aware and limit-checked money operations.
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
        int maxPerSlot = ModConfig<BankAppConfig>.Instance.MaxCashPerSlot;
        float capacity = EconomyHelper.GetMaxHoldableCashCapacity(maxPerSlot);
        float onlineBalance = GetOnlineBalance();

        return Mathf.Min(onlineBalance, capacity);
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
        float remainingLimit = GetRemainingWeeklyAtmLimit();
        return Mathf.Min(cashOnHand, remainingLimit);
    }

    public static bool DepositCash(float amount, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (amount <= 0f)
        {
            errorMessage = "Invalid amount.";
            BankSoundService.PlayError();
            return false;
        }

        float cashOnHand = GetCashBalance();
        if (cashOnHand < amount)
        {
            errorMessage = $"Insufficient cash on hand ($ {cashOnHand:N0} available).";
            BankSoundService.PlayError();
            return false;
        }

        var config = ModConfig<BankAppConfig>.Instance;
        if (config.RespectVanillaAtmLimit)
        {
            float remainingLimit = GetRemainingWeeklyAtmLimit();
            if (amount > remainingLimit)
            {
                errorMessage = $"Weekly ATM limit exceeded (Max remaining: ${remainingLimit:N0} / ${VanillaWeeklyAtmLimit:N0}).";
                BankSoundService.PlayError();
                return false;
            }
        }

        float feePercent = Mathf.Clamp(config.ServiceFeePercent, 0f, 10f);
        float fee = amount * (feePercent / 100f);
        float netCredited = amount - fee;

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
            try { EconomyHelper.ChangeCashBalance(+amount, false, false); } catch { }
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
            Description = fee > 0f ? $"Mobile Cash Deposit (Fee: ${fee:N0})" : "Mobile Cash Deposit",
            BalanceAfter = balanceAfter
        });

        BankSoundService.PlayCashSuccess();
        return true;
    }

    public static bool WithdrawCash(float amount, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (amount <= 0f)
        {
            errorMessage = "Invalid amount.";
            BankSoundService.PlayError();
            return false;
        }

        float onlineBalance = GetOnlineBalance();
        var config = ModConfig<BankAppConfig>.Instance;
        float feePercent = Mathf.Clamp(config.ServiceFeePercent, 0f, 10f);
        float fee = amount * (feePercent / 100f);
        float totalDeducted = amount + fee;

        if (onlineBalance < totalDeducted)
        {
            errorMessage = $"Insufficient bank funds (${onlineBalance:N0} available, ${totalDeducted:N0} required).";
            BankSoundService.PlayError();
            return false;
        }

        float maxWithdrawable = GetMaxWithdrawableCash();
        if (amount > maxWithdrawable)
        {
            int freeSlots = GetFreeInventorySlotsCount();
            errorMessage = $"Inventory full! Only room for ${maxWithdrawable:N0} ({freeSlots} free slot{(freeSlots == 1 ? "" : "s")}).";
            BankSoundService.PlayError();
            return false;
        }

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
            try { EconomyHelper.CreateOnlineTransaction("ATM Withdrawal Refund", +totalDeducted, 1f, "Rollback"); } catch { }
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
            Amount = -amount,
            Type = TransactionType.Withdrawal,
            Description = fee > 0f ? $"Mobile Cash Withdrawal (Fee: ${fee:N0})" : "Mobile Cash Withdrawal",
            BalanceAfter = balanceAfter
        });

        BankSoundService.PlayCashSuccess();
        return true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using BusinessIncome.Config;
using BusinessIncome.Models;
using Il2CppFishNet;
using Il2CppScheduleOne.UI;
using S1API.GameTime;
using S1API.Money;
using S1Mods.Shared;
using UnityEngine;

namespace BusinessIncome.Services;

/// <summary>
/// Main service orchestrating revenue calculation, payout,
/// host check, transaction execution, and in-game notifications.
/// </summary>
public static class IncomeEngine
{
    /// <summary>
    /// Checks whether the current instance is host/server or running in singleplayer.
    /// </summary>
    public static bool IsHostOrSingleplayer()
    {
        try
        {
            if (InstanceFinder.NetworkManager == null || (UnityEngine.Object)InstanceFinder.NetworkManager == null)
                return true;

            return InstanceFinder.IsServer;
        }
        catch
        {
            return true;
        }
    }

    /// <summary>
    /// Calculates the revenue preview for the given day.
    /// </summary>
    public static (List<BusinessRevenueLine> Lines, float TotalGross, float TotalCosts, float TotalNet) GetDailyRevenuePreview(
        int elapsedDays,
        BusinessIncomeConfig config)
    {
        config.Sanitize();
        var owned = BusinessResolver.GetOwnedBusinesses(config.DisplayNameOverrides);
        if (owned.Count == 0)
        {
            return (new List<BusinessRevenueLine>(), 0f, 0f, 0f);
        }

        bool isWeekend = false;
        try
        {
            var dayOfWeek = S1API.GameTime.TimeManager.CurrentDay;
            isWeekend = dayOfWeek is Day.Saturday or Day.Sunday;
        }
        catch
        {
            // Fallback
        }

        var lines = RevenueCalculator.CalculateAll(owned, elapsedDays, isWeekend, config);
        float totalGross = lines.Sum(l => l.GrossRevenue);
        float totalCosts = lines.Sum(l => l.OperatingCosts);
        float totalNet = lines.Sum(l => l.NetRevenue);

        return (lines, totalGross, totalCosts, totalNet);
    }

    /// <summary>
    /// Executes the daily payout if all conditions (host, idempotency, ownership) are met.
    /// </summary>
    public static bool TryExecuteDailyPayout(
        int elapsedDays,
        BusinessIncomeConfig config,
        bool force = false,
        bool commit = true,
        bool isDryRun = false)
    {
        // 1. Check host authority
        if (!IsHostOrSingleplayer())
        {
            Mod.Log.Debug("Payout skipped: Client instance (only server/host executes payouts).");
            return false;
        }

        // 2. Idempotency check
        if (!force && PayoutStateStore.IsDayPaid(elapsedDays))
        {
            Mod.Log.Debug($"Payout for day {elapsedDays} already executed.");
            return false;
        }

        // 3. Calculate revenue
        var (lines, totalGross, totalCosts, totalNet) = GetDailyRevenuePreview(elapsedDays, config);
        if (lines.Count == 0)
        {
            Mod.Log.Debug($"No owned businesses for day {elapsedDays}.");
            return false;
        }

        if (totalNet <= 0f)
        {
            Mod.Log.Info($"Total revenue for day {elapsedDays} is $0. No transaction executed.");
            if (commit && !isDryRun)
            {
                PayoutStateStore.CommitPayout(elapsedDays, lines.Select(l => l.BusinessId));
            }
            return false;
        }

        if (isDryRun)
        {
            Mod.Log.Info($"[DRY RUN] Day {elapsedDays}: Gross ${totalGross:N2}, Costs ${totalCosts:N2}, Net ${totalNet:N2} ({lines.Count} businesses).");
            return true;
        }

        // 4. Mark as paid IN MEMORY FIRST, then execute the transaction, then persist.
        //    Reihenfolge verhindert Geld-Duplikation: Wenn CommitPayout (Disk) nach einer
        //    erfolgreichen Transaktion fehlschlägt, bleibt der In-Memory-State gesetzt und
        //    der nächste Day-Pass wird übersprungen (kein doppeltes Auszahlen).
        //    Schlägt hingegen die Transaktion fehl, wird der Memory-State zurückgesetzt.
        var businessIds = lines.Select(l => l.BusinessId).ToList();
        if (commit)
        {
            PayoutStateStore.MarkInMemoryPaid(elapsedDays, businessIds);
        }

        try
        {
            string summaryNote = $"Daily Revenue ({lines.Count} businesses): +${totalNet:N0}";
            Money.CreateOnlineTransaction("Business Revenue", totalNet, 1f, summaryNote);
            Mod.Log.Info($"[Payout] Day {elapsedDays}: +${totalNet:N2} booked ({lines.Count} businesses).");
        }
        catch (Exception ex)
        {
            // Transaction failed — revert the in-memory marker so the payout can retry.
            if (commit)
            {
                PayoutStateStore.RevertInMemoryPaid(elapsedDays, businessIds);
            }
            Mod.Log.Error($"Online transaction failed: {ex.Message}");
            return false;
        }

        // 5. Persist state (now safe: transaction already succeeded; a disk failure here
        //    only means the marker is missing on disk, but in-memory state stays set).
        if (commit)
        {
            PayoutStateStore.CommitPayout(elapsedDays, businessIds);
        }

        // 6. Send in-game HUD notification
        if (config.EnableNotifications)
        {
            SendNotification(totalNet, lines.Count, config.PlayCashSound);
        }

        return true;
    }

    /// <summary>
    /// Sends an in-game HUD notification via the NotificationsManager.
    /// </summary>
    private static void SendNotification(float totalNet, int businessCount, bool playSound)
    {
        try
        {
            var notifMgr = NotificationsManager.Instance;
            if (notifMgr != null && (UnityEngine.Object)notifMgr != null)
            {
                string title = "Business Revenue";
                string sub = $"+${totalNet:N0} from {businessCount} {(businessCount == 1 ? "business" : "businesses")}";
                notifMgr.SendNotification(title, sub, null!, 5f, playSound);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"Failed to send notification: {ex.Message}");
        }
    }
}

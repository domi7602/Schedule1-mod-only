using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BusinessIncome.Config;
using BusinessIncome.Models;
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
    /// Konsolidiert 2026-09-15 in S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer
    /// (fail-closed bei Authority-Exceptions).
    /// </summary>
    public static bool IsHostOrSingleplayer() => NetworkGuard.IsHostOrSingleplayer();

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

        // H2: Derive weekend from elapsedDays, not current day — backfill correctness
        bool isWeekend = false;
        try
        {
            int dayIdx = elapsedDays % 7;
            if (dayIdx < 0) dayIdx += 7;
            isWeekend = dayIdx == 5 || dayIdx == 6; // Saturday/Sunday per EDay Monday=0
        }
        catch
        {
            try
            {
                var cur = S1API.GameTime.TimeManager.CurrentDay;
                isWeekend = cur is Day.Saturday or Day.Sunday;
            }
            catch { }
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
                var idsZero = lines.Select(l => l.BusinessId).ToList();
                PayoutStateStore.MarkInMemoryPaid(elapsedDays, idsZero);
                bool ok = PayoutStateStore.CommitPayout(elapsedDays, idsZero);
                if (!ok) PayoutStateStore.RevertInMemoryPaid(elapsedDays, idsZero);
            }
            return false;
        }

        if (isDryRun)
        {
            Mod.Log.Info($"[DRY RUN] Day {elapsedDays}: Gross ${totalGross.ToString("N2", CultureInfo.InvariantCulture)}, Costs ${totalCosts.ToString("N2", CultureInfo.InvariantCulture)}, Net ${totalNet.ToString("N2", CultureInfo.InvariantCulture)} ({lines.Count} businesses).");
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
            // F1: Write-ahead marker BEFORE the transaction. If the game crashes after the
            // bank booked but before CommitPayout below, the marker survives on disk and
            // startup warns instead of silently paying this day again.
            PayoutStateStore.WritePendingMarker(elapsedDays, totalNet, lines.Count);
            PayoutStateStore.MarkInMemoryPaid(elapsedDays, businessIds);
        }

        try
        {
            string summaryNote = $"Daily Revenue ({lines.Count} businesses): +${totalNet.ToString("N0", CultureInfo.InvariantCulture)}";
            Money.CreateOnlineTransaction("Business Revenue", totalNet, 1f, summaryNote);
            Mod.Log.Info($"[Payout] Day {elapsedDays}: +${totalNet.ToString("N2", CultureInfo.InvariantCulture)} booked ({lines.Count} businesses).");
        }
        catch (Exception ex)
        {
            // Transaction failed — revert the in-memory marker so the payout can retry.
            if (commit)
            {
                PayoutStateStore.RevertInMemoryPaid(elapsedDays, businessIds);
                PayoutStateStore.ClearPendingMarker(); // F1: no money moved — safe to clear
            }
            Mod.Log.Error($"Online transaction failed: {ex.Message}");
            return false;
        }

        // 5. Persist state (now safe: transaction already succeeded; a disk failure here
        //    only means the marker is missing on disk, but in-memory state stays set).
        bool committed = true;
        if (commit)
        {
            // F1: the commit result is now checked. On failure the pending marker stays on
            // disk: the money IS booked, but the saved state is not — startup warns and
            // 'biz pending confirm|resolve' resolves it instead of a silent double payout.
            committed = PayoutStateStore.CommitPayout(elapsedDays, businessIds);
            if (committed)
            {
                PayoutStateStore.ClearPendingMarker();
            }
            else
            {
                Mod.Log.Error($"PayoutState for day {elapsedDays} NOT saved (transaction already booked). Pending marker kept — resolve with 'biz pending confirm' (money received) or 'biz pending resolve' (pay again).");
            }
        }

        // 6. Send in-game HUD notification. Only show the success banner when the money is
        //    actually in the bank (committed=true). On commit-with-failed-persist, show a
        //    different message so the player is not told "you got paid" when the state
        //    was not saved. Dry-run (commit=false) keeps the normal revenue preview
        //    notification since nothing was booked.
        if (config.EnableNotifications)
        {
            if (commit && !committed)
            {
                try
                {
                    var notifMgr = NotificationsManager.Instance;
                    if (notifMgr != null && (UnityEngine.Object)notifMgr != null)
                        notifMgr.SendNotification("Business Revenue",
                            $"+${totalNet.ToString("N0", CultureInfo.InvariantCulture)} booked — save FAILED, run 'biz pending confirm|resolve'.",
                            null!, 5f, false);
                }
                catch { }
            }
            else
            {
                SendNotification(totalNet, lines.Count, config.PlayCashSound);
            }
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
                string sub = $"+${totalNet.ToString("N0", CultureInfo.InvariantCulture)} from {businessCount} {(businessCount == 1 ? "business" : "businesses")}";
                notifMgr.SendNotification(title, sub, null!, 5f, playSound);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"Failed to send notification: {ex.Message}");
        }
    }
}

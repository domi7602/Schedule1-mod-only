using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BusinessIncome.Config;
using BusinessIncome.Core;
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
///
/// All money movement is delegated to the pure A-D settlement core via
/// <see cref="PayoutStateStore"/> so the safety ordering (pending marker -> bank request ->
/// forward state commit -> marker clear) is enforced in exactly one place.
/// </summary>
public static class IncomeEngine
{
    /// <summary>
    /// Checks whether the current instance is host/server or running in singleplayer.
    /// Consolidated 2026-09-15 in S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer
    /// (fail-closed on authority exceptions).
    /// </summary>
    public static bool IsHostOrSingleplayer() => NetworkGuard.IsHostOrSingleplayer();

    /// <summary>
    /// Calculates the revenue preview for the given day.
    /// </summary>
    public static (List<BusinessRevenueLine> Lines, float TotalGross, float TotalCosts, float TotalNet) GetDailyRevenuePreview(
        int elapsedDays,
        BusinessIncomeConfig config)
        => TryGetDailyRevenuePreview(elapsedDays, config, out var p)
            ? p
            : (new List<BusinessRevenueLine>(), 0f, 0f, 0f);

    /// <summary>
    /// Fail-closed revenue preview. Returns FALSE when the owned-business read is not
    /// trustworthy (unreadable / partial) — the caller must NOT treat that as "no businesses",
    /// because an unknown read must never terminal-mark a day nor suppress a real payout.
    /// </summary>
    public static bool TryGetDailyRevenuePreview(
        int elapsedDays,
        BusinessIncomeConfig config,
        out (List<BusinessRevenueLine> Lines, float TotalGross, float TotalCosts, float TotalNet) preview)
    {
        config.Sanitize();
        preview = (new List<BusinessRevenueLine>(), 0f, 0f, 0f);

        if (!BusinessResolver.TryGetOwnedBusinesses(config.DisplayNameOverrides, out var owned))
        {
            Mod.Log.Warn("[Payout] owned-business read is not trustworthy (unreadable/partial) — failing closed (NOT treated as 'no businesses').");
            return false;
        }
        if (owned.Count == 0)
        {
            return true;
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

        preview = (lines, totalGross, totalCosts, totalNet);
        return true;
    }

    /// <summary>Whether a payout outcome means the caller must stop any catch-up loop.</summary>
    public static bool StopsCatchup(PayoutOutcome outcome) => outcome is
        PayoutOutcome.NotAuthoritative or
        PayoutOutcome.NotReady or
        PayoutOutcome.BlockedPending or
        PayoutOutcome.BlockedStateCorrupt or
        PayoutOutcome.BlockedMarkerWrite or
        PayoutOutcome.BookedStateNotSaved or
        PayoutOutcome.UnknownOutcome or
        PayoutOutcome.StateNotPersisted or
        PayoutOutcome.MarkerClearFailed or
        PayoutOutcome.InvalidInput or
        PayoutOutcome.Reentrant;

    /// <summary>
    /// Executes the daily payout and returns the explicit settlement outcome. A clean
    /// <see cref="PayoutOutcome.Requested"/> means the bank request was ACCEPTED, not that
    /// settlement is confirmed.
    /// </summary>
    public static PayoutResult ExecuteDailyPayout(
        int elapsedDays,
        BusinessIncomeConfig config,
        bool force = false,
        bool commit = true,
        bool isDryRun = false)
    {
        // 1. Calculate revenue. An untrustworthy owned-business read fails closed and is never
        //    treated as "no businesses".
        if (!TryGetDailyRevenuePreview(elapsedDays, config, out var preview))
            return new PayoutResult(PayoutOutcome.NotReady, elapsedDays, 0f, 0, "owned-business read not trustworthy");
        var (lines, totalGross, totalCosts, totalNet) = preview;

        // 2. Dry run FIRST: a pure preview needs no host authority, no bank call and no write.
        if (isDryRun)
        {
            var dryPreview = PayoutStateStore.Preview(elapsedDays, totalNet, lines.Count);
            Mod.Log.Info($"[DRY RUN] Day {elapsedDays}: Gross ${totalGross.ToString("N2", CultureInfo.InvariantCulture)}, Costs ${totalCosts.ToString("N2", CultureInfo.InvariantCulture)}, Net ${totalNet.ToString("N2", CultureInfo.InvariantCulture)} ({lines.Count} businesses). [{dryPreview.Outcome}]");
            return dryPreview with { Day = elapsedDays, Net = totalNet, BusinessCount = lines.Count };
        }

        // 3. Host authority: only the authoritative instance may mutate money or state.
        if (!IsHostOrSingleplayer())
        {
            Mod.Log.Debug("Payout skipped: Client instance (only server/host executes payouts).");
            return new PayoutResult(PayoutOutcome.NotAuthoritative, elapsedDays, 0f, 0, "not host/singleplayer");
        }

        if (!commit)
        {
            Mod.Log.Debug($"Payout day {elapsedDays}: preview-only (commit:false), nothing booked.");
            return new PayoutResult(PayoutOutcome.Skipped, elapsedDays, totalNet, lines.Count, "preview-only (commit:false)");
        }

        var businessIds = lines.Select(l => l.BusinessId).ToList();

        // All days, including zero/no-business days, use the same guarded settlement
        // path. This preserves idempotency, invalid-input checks and pending locks.
        var result = PayoutStateStore.TryBook(elapsedDays, totalNet, businessIds, force);

        switch (result.Outcome)
        {
            case PayoutOutcome.Requested:
            case PayoutOutcome.BookedStateNotSaved:
                Mod.Log.Info($"[Payout] Day {elapsedDays}: +${totalNet.ToString("N2", CultureInfo.InvariantCulture)} request accepted ({lines.Count} businesses) [{result.Outcome}].");
                break;

            case PayoutOutcome.UnknownOutcome:
                Mod.Log.Error($"[Payout] Day {elapsedDays}: bank call raised AFTER invocation — outcome UNKNOWN. Marker kept and retry blocked. " +
                              "Use 'biz pending confirm' (money received) or 'biz pending resolve' (pay again).");
                break;

            default:
                Mod.Log.Debug($"[Payout] Day {elapsedDays} not booked: {result.Outcome} ({result.Detail}).");
                break;
        }

        if (config.EnableNotifications)
        {
            if (result.Outcome is PayoutOutcome.BookedStateNotSaved or PayoutOutcome.UnknownOutcome)
                SendBlockerNotification($"+${totalNet.ToString("N0", CultureInfo.InvariantCulture)} requested — save NOT confirmed. Run 'biz pending confirm|resolve'.");
            else if (result.Outcome == PayoutOutcome.Requested)
                SendNotification(totalNet, lines.Count, config.PlayCashSound);
            else if (result.Outcome is PayoutOutcome.BlockedPending or PayoutOutcome.BlockedStateCorrupt)
                SendBlockerNotification($"Payout for day {elapsedDays} blocked: {result.Detail}. Run 'biz pending confirm|resolve'.");
        }

        return result;
    }

    /// <summary>Back-compat bool wrapper — true only when the bank request was accepted.</summary>
    public static bool TryExecuteDailyPayout(
        int elapsedDays,
        BusinessIncomeConfig config,
        bool force = false,
        bool commit = true,
        bool isDryRun = false)
        => ExecuteDailyPayout(elapsedDays, config, force, commit, isDryRun).Succeeded;

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

    /// <summary>Warning banner when a payout was blocked or its persistence is unconfirmed.</summary>
    private static void SendBlockerNotification(string message)
    {
        try
        {
            var notifMgr = NotificationsManager.Instance;
            if (notifMgr != null && (UnityEngine.Object)notifMgr != null)
                notifMgr.SendNotification("Business Revenue", message, null!, 5f, false);
        }
        catch { }
    }
}

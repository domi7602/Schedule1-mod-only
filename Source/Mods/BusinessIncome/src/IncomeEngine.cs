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
            if (commit && !isDryRun)
            {
                PayoutStateStore.MarkInMemoryPaid(elapsedDays, Array.Empty<string>());
                bool ok = PayoutStateStore.CommitPayout(elapsedDays, Array.Empty<string>());
                if (!ok) PayoutStateStore.RevertInMemoryPaid(elapsedDays, Array.Empty<string>());
            }
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
        //    Order prevents money duplication: if CommitPayout (disk) fails after a
        //    successful transaction, the in-memory state remains set and the
        //    next day pass is skipped (no double payout).
        //    If the transaction itself fails, the memory state is reverted.
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
                            ResolveNotificationIcon(), 5f, false);
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
    /// The game's own money-notification sprite (MoneyManager.LaunderingNotificationIcon),
    /// resolved lazily once per session. Falls back to a procedural green "$" tile so the
    /// banner never renders as an empty white square.
    /// </summary>
    private static Sprite? _notificationIcon;
    private static bool _notificationIconResolved;

    private static Sprite ResolveNotificationIcon()
    {
        if (_notificationIconResolved)
            return _notificationIcon!;

        _notificationIconResolved = true;
        try
        {
            var money = Il2CppScheduleOne.Money.MoneyManager.Instance;
            if (money == null || (UnityEngine.Object)money == null)
            {
                Mod.Log.Warn("[notify] MoneyManager instance unavailable - using the procedural fallback icon.");
            }
            else
            {
                Sprite? icon = money.LaunderingNotificationIcon;
                if (icon != null && (UnityEngine.Object)icon != null)
                    return _notificationIcon = icon;
                Mod.Log.Warn("[notify] MoneyManager.LaunderingNotificationIcon is not assigned - using the procedural fallback icon.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[notify] reading the money notification icon failed ({ex.Message}) - using the procedural fallback icon.");
        }

        _notificationIcon = CreateFallbackNotificationIcon();
        return _notificationIcon;
    }

    /// <summary>Procedural fallback: deep-green rounded tile with a white "$" (no bitmap asset).</summary>
    private static Sprite CreateFallbackNotificationIcon()
    {
        const int size = 64;
        const float radius = 12f;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = "BusinessIncome_NotifyIcon_Fallback" };
        var tile = new Color32(46, 125, 70, 255);
        var glyph = new Color32(255, 255, 255, 255);
        var px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float cx = Mathf.Clamp(x + 0.5f, radius, size - radius);
                float cy = Mathf.Clamp(y + 0.5f, radius, size - radius);
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(cx, cy));
                float a = Mathf.Clamp01(radius - dist + 0.5f);
                bool inTile = a > 0.5f;
                px[y * size + x] = inTile && IsDollarPixel(x + 0.5f, y + 0.5f)
                    ? glyph
                    : new Color32(tile.r, tile.g, tile.b, (byte)Mathf.RoundToInt(a * 255f));
            }
        }

        tex.SetPixels32(px);
        tex.Apply(false, true);
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    /// <summary>Blocky "$" mask: vertical stem plus five S segments (LCD-style).</summary>
    private static bool IsDollarPixel(float x, float y)
    {
        if (x >= 29f && x < 35f && y >= 6f && y <= 58f) return true;   // stem
        if (y >= 44f && y < 50f && x >= 16f && x < 48f) return true;   // top bar
        if (y >= 38f && y < 44f && x >= 16f && x < 23f) return true;   // upper-left connector
        if (y >= 29f && y < 35f && x >= 16f && x < 48f) return true;   // middle bar
        if (y >= 14f && y < 29f && x >= 42f && x < 48f) return true;   // lower-right connector
        return y >= 8f && y < 14f && x >= 16f && x < 48f;              // bottom bar
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
                notifMgr.SendNotification(title, sub, ResolveNotificationIcon(), 5f, playSound);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"Failed to send notification: {ex.Message}");
        }
    }
}

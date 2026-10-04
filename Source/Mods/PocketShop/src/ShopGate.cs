using System;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using UnityEngine;

namespace PocketShop.Services;

/// <summary>
/// Availability of one vanilla shop inside the PocketShop phone app.
///
/// Two independent vanilla signals are read:
///   - IsLocked: the shop must not be usable yet (black-market rank/quest unlock).
///   - IsOpen:   the shop's building/access zone is currently open (opening hours).
///
/// Every value is a best-effort read of vanilla state; a missing signal degrades to
/// "available" instead of throwing into the catalog refresh. Each resolution is
/// logged at Info so a playtest log can attribute a shop's state.
/// </summary>
public sealed class ShopGate
{
    public bool IsLocked;
    public bool IsOpen = true;
    public bool HasSchedule;
    public int OpenTime = -1;
    public int CloseTime = -1;
    public string Source = "none";

    /// <summary>"18:00-06:00" when the shop has a timed access zone, else empty.</summary>
    public string HoursText
    {
        get
        {
            if (!HasSchedule) return string.Empty;
            return $"{FormatTime(OpenTime)}-{FormatTime(CloseTime)}";
        }
    }

    /// <summary>Accepts both 24h HHMM (1800) and minute-of-day (1080) authored values.</summary>
    private static string FormatTime(int value)
    {
        if (value < 0) return "--:--";
        int h, m;
        if (value >= 100 && (value % 100) < 60) { h = value / 100; m = value % 100; }
        else { h = value / 60; m = value % 60; }
        h = ((h % 24) + 24) % 24;
        return $"{h:00}:{m:00}";
    }
}

/// <summary>
/// Resolves the vanilla availability signals for a shop. Never throws.
/// </summary>
public static class ShopGateResolver
{
    public static ShopGate Resolve(ShopInterface shop, string shopCode, string shopName)
    {
        var gate = new ShopGate();
        if (shop == null) return gate;

        // 1. Dark market (Oscar): rank-gated unlock. Its DarkMarketAccessZone also carries
        //    the opening hours, so reuse it instead of walking the transform tree.
        try
        {
            var dm = DarkMarket.Instance;
            if (dm != null && dm.Pointer != IntPtr.Zero && !dm.WasCollected)
            {
                bool isDarkMarketShop = false;
                try
                {
                    var oscar = dm.Oscar;
                    var oscarShop = (oscar != null && oscar.Pointer != IntPtr.Zero && !oscar.WasCollected)
                        ? oscar.ShopInterface
                        : null;
                    if (oscarShop != null && oscarShop.Pointer == shop.Pointer) isDarkMarketShop = true;
                }
                catch { }
                if (!isDarkMarketShop)
                {
                    try { isDarkMarketShop = shop.transform.IsChildOf(dm.transform); } catch { }
                }

                if (isDarkMarketShop)
                {
                    gate.Source = "DarkMarket";
                    try { gate.IsLocked = !dm.Unlocked; } catch { }
                    try { ApplyZone(gate, dm.AccessZone); } catch { }
                    LogGate(gate, shopCode, shopName);
                    return gate;
                }
            }
        }
        catch { }

        // 2. Generic building access zone (opening hours + any zone-driven open state).
        AccessZone? zone = null;
        try { zone = shop.GetComponentInParent<AccessZone>(true); } catch { }
        if (zone != null)
        {
            gate.Source = "zone:" + ZoneTypeName(zone);
            ApplyZone(gate, zone);
        }

        LogGate(gate, shopCode, shopName);
        return gate;
    }

    private static void ApplyZone(ShopGate gate, AccessZone? zone)
    {
        if (zone == null) return;
        try { gate.IsOpen = zone.IsOpen; } catch { }
        try
        {
            var timed = zone.TryCast<TimedAccessZone>();
            if (timed != null)
            {
                gate.HasSchedule = true;
                gate.OpenTime = timed.OpenTime;
                gate.CloseTime = timed.CloseTime;
            }
        }
        catch { }
    }

    /// <summary>Runtime type name of a zone proxy (GetType() only returns the declared proxy type).</summary>
    private static string ZoneTypeName(AccessZone zone)
    {
        try { if (zone.TryCast<DarkMarketAccessZone>() != null) return "DarkMarketAccessZone"; } catch { }
        try { if (zone.TryCast<TimedAccessZone>() != null) return "TimedAccessZone"; } catch { }
        try { if (zone.TryCast<NPCPresenceAccessZone>() != null) return "NPCPresenceAccessZone"; } catch { }
        return "AccessZone";
    }

    private static void LogGate(ShopGate gate, string code, string name)
    {
        try
        {
            string hours = gate.HasSchedule ? $"{gate.OpenTime}-{gate.CloseTime}" : "none";
            MelonLogger.Msg($"[PocketShop] gate '{code}' ({name}): src={gate.Source} open={gate.IsOpen} locked={gate.IsLocked} hours={hours}");
        }
        catch { }
    }
}

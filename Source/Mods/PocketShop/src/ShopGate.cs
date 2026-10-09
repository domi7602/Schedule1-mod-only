using System;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.UI.Shop;
using Il2CppScheduleOne.GameTime;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;

namespace PocketShop.Services;

/// <summary>Live shop availability. Unknown is deliberately non-purchasable.</summary>
public sealed class ShopGate
{
    public bool IsKnown;
    public bool IsLocked;
    public bool IsOpen;
    public bool HasSchedule;
    public int OpenTime = -1;
    public int CloseTime = -1;
    public string Source = "unknown";

    public bool CanPurchase => IsKnown && !IsLocked && IsOpen;

    public string HoursText => HasSchedule ? $"{FormatTime(OpenTime)}-{FormatTime(CloseTime)}" : string.Empty;

    /// <summary>Game time is HHMM (24-hour); validate and format that one documented representation.</summary>
    private static string FormatTime(int value)
    {
        try
        {
            if (!TimeManager.IsValid24HourTime(value)) return "--:--";
            return $"{value / 100:00}:{value % 100:00}";
        }
        catch
        {
            return "--:--";
        }
    }
}

/// <summary>Resolves vanilla shop and access-zone restrictions without failing open on read errors.</summary>
public static class ShopGateResolver
{
    public static ShopGate Resolve(ShopInterface shop, string shopCode, string shopName)
    {
        var gate = new ShopGate();
        if (!NetworkGuard.IsAlive(shop)) return gate;

        try
        {
            gate.IsOpen = shop.IsOpen;
            gate.Source = "ShopInterface";
        }
        catch
        {
            LogGate(gate, shopCode, shopName);
            return gate;
        }

        AccessZone? zone;
        try
        {
            zone = shop.GetComponentInParent<AccessZone>(true);
        }
        catch
        {
            LogGate(gate, shopCode, shopName);
            return gate;
        }

        DarkMarket? darkMarket;
        try
        {
            darkMarket = DarkMarket.Instance;
        }
        catch
        {
            LogGate(gate, shopCode, shopName);
            return gate;
        }

        bool isDarkMarketShop = false;
        if (NetworkGuard.IsAlive(darkMarket))
        {
            if (!TryIsDarkMarketShop(darkMarket, shop, out isDarkMarketShop))
            {
                LogGate(gate, shopCode, shopName);
                return gate;
            }
        }

        if (isDarkMarketShop)
        {
            gate.Source = "DarkMarket";
            try
            {
                gate.IsLocked = !darkMarket!.Unlocked;
                zone ??= darkMarket.AccessZone;
            }
            catch
            {
                LogGate(gate, shopCode, shopName);
                return gate;
            }
        }

        if (zone != null)
        {
            if (!NetworkGuard.IsAlive(zone))
            {
                LogGate(gate, shopCode, shopName);
                return gate;
            }

            if (!isDarkMarketShop)
            {
                try
                {
                    if (zone.TryCast<DarkMarketAccessZone>() != null)
                    {
                        // The zone itself proves this is a dark-market restriction, but without
                        // its owner we cannot establish the separate unlock requirement.
                        if (!NetworkGuard.IsAlive(darkMarket))
                        {
                            gate.Source = "DarkMarketAccessZone";
                            LogGate(gate, shopCode, shopName);
                            return gate;
                        }

                        gate.IsLocked = !darkMarket!.Unlocked;
                        gate.Source = "DarkMarketAccessZone";
                    }
                    else
                    {
                        gate.Source = "zone:" + ZoneTypeName(zone);
                    }
                }
                catch
                {
                    LogGate(gate, shopCode, shopName);
                    return gate;
                }
            }

            if (!ApplyZone(gate, zone))
            {
                LogGate(gate, shopCode, shopName);
                return gate;
            }
        }

        gate.IsKnown = true;
        LogGate(gate, shopCode, shopName);
        return gate;
    }

    private static bool TryIsDarkMarketShop(DarkMarket darkMarket, ShopInterface shop, out bool isDarkMarketShop)
    {
        isDarkMarketShop = false;
        try
        {
            var oscar = darkMarket.Oscar;
            if (NetworkGuard.IsAlive(oscar))
            {
                var oscarShop = oscar!.ShopInterface;
                if (NetworkGuard.IsAlive(oscarShop))
                {
                    isDarkMarketShop = oscarShop!.Pointer == shop.Pointer;
                    return true;
                }
            }
        }
        catch { }

        try
        {
            isDarkMarketShop = shop.transform.IsChildOf(darkMarket.transform);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool ApplyZone(ShopGate gate, AccessZone zone)
    {
        try
        {
            gate.IsOpen = gate.IsOpen && zone.IsOpen;
            var timed = zone.TryCast<TimedAccessZone>();
            if (timed != null)
            {
                int open = timed.OpenTime;
                int close = timed.CloseTime;
                if (!TimeManager.IsValid24HourTime(open) || !TimeManager.IsValid24HourTime(close)) return false;
                gate.HasSchedule = true;
                gate.OpenTime = open;
                gate.CloseTime = close;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string ZoneTypeName(AccessZone zone)
    {
        try { if (zone.TryCast<TimedAccessZone>() != null) return "TimedAccessZone"; } catch { }
        try { if (zone.TryCast<NPCPresenceAccessZone>() != null) return "NPCPresenceAccessZone"; } catch { }
        return "AccessZone";
    }

    private static void LogGate(ShopGate gate, string code, string name)
    {
        try
        {
            string state = !gate.IsKnown ? "unknown" : gate.IsLocked ? "locked" : gate.IsOpen ? "open" : "closed";
            string hours = gate.HasSchedule ? $"{gate.OpenTime}-{gate.CloseTime}" : "none";
            MelonLogger.Msg($"[PocketShop] gate '{code}' ({name}): src={gate.Source} state={state} hours={hours}");
        }
        catch { }
    }
}

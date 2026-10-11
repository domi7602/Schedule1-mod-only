using System;
using System.Collections.Generic;
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

        // NOTE: ShopInterface.IsOpen is the UI-menu state (is the shop panel open
        // right now), NOT the shop's business hours. A phone shop must be browsable
        // regardless of whether the player is standing in front of the shop with the
        // menu open — so IsOpen is NOT used as the availability signal here.
        // Business hours (if any) come from a TimedAccessZone in the shop's parent
        // chain; when none exists the shop has no schedule and stays browsable.
        gate.IsOpen = true;  // default: no schedule = always browsable from the phone
        gate.Source = "phone";

        AccessZone? zone;
        try
        {
            zone = shop.GetComponentInParent<AccessZone>(true);
        }
        catch
        {
            // GetComponentInParent<AccessZone> can throw in IL2CPP when the parent
            // chain contains a non-MonoBehaviour component or a type mismatch.
            // Fall back to GetComponentsInParent which is more resilient.
            try
            {
                var zones = shop.GetComponentsInParent<AccessZone>(true);
                zone = zones != null && zones.Length > 0 ? zones[0] : null;
            }
            catch
            {
                zone = null;
            }
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

        // 1. Transform check FIRST — the definitive structural answer, no IL2CPP risk.
        try
        {
            isDarkMarketShop = shop.transform.IsChildOf(darkMarket.transform);
            return true;
        }
        catch { }

        // 2. NPC-based fallback: when the transform check is unavailable, Oscar's shop
        //    interface still gives a definite answer — return it instead of discarding the
        //    result (an undetermined value forces VERIFY for every shop). If the fallback
        //    cannot answer either, the gate stays unknown and locked (fail-closed).
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
        return false;
    }

    private static bool ApplyZone(ShopGate gate, AccessZone zone)
    {
        try
        {
            // zone.IsOpen combines time-of-day AND player-presence checks. For the
            // phone app the player is NOT physically inside the shop's zone — the
            // access-zone presence check would always fail and force CLOSED.
            // Evaluate the TIME schedule instead: a TimedAccessZone with a valid
            // window is open if the current game time falls inside it.
            var timed = zone.TryCast<TimedAccessZone>();
            if (timed != null)
            {
                int open = timed.OpenTime;
                int close = timed.CloseTime;
                if (!TimeManager.IsValid24HourTime(open) || !TimeManager.IsValid24HourTime(close)) return false;
                gate.HasSchedule = true;
                gate.OpenTime = open;
                gate.CloseTime = close;
                gate.IsOpen = IsTimeWithinSchedule(open, close);
            }
            else
            {
                // Non-timed zone (e.g. a presence-only zone): trust the base ShopInterface
                // open state — the phone does not require physical presence.
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks whether the current game time falls within [open, close) in HHMM 24-hour
    /// format. Handles midnight wrap (e.g. open=2200, close=0500). Uses TimeManager.
    /// </summary>
    private static bool IsTimeWithinSchedule(int open, int close)
    {
        try
        {
            var tm = TimeManager.Instance;
            if (tm == null) return false;
            int now = tm.CurrentTime;
            if (!TimeManager.IsValid24HourTime(now)) return false;

            if (open == close) return true; // 24h schedule
            if (open < close)
            {
                // Normal window (e.g. 600-1800)
                return now >= open && now < close;
            }
            // Wrap-around window (e.g. 2200-0500)
            return now >= open || now < close;
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

    /// <summary>
    /// Change-only gate log. Resolve() runs on every live offer read (per card, per refresh),
    /// so logging each result produced thousands of identical lines. Only a state change is logged.
    /// </summary>
    private static readonly Dictionary<string, string> _lastLoggedGate = new();

    private static void LogGate(ShopGate gate, string code, string name)
    {
        try
        {
            string state = !gate.IsKnown ? "unknown" : gate.IsLocked ? "locked" : gate.IsOpen ? "open" : "closed";
            string hours = gate.HasSchedule ? $"{gate.OpenTime}-{gate.CloseTime}" : "none";
            string signature = $"src={gate.Source} state={state} hours={hours}";
            if (_lastLoggedGate.TryGetValue(code, out string? previous) && previous == signature)
                return;
            _lastLoggedGate[code] = signature;
            MelonLogger.Msg($"[PocketShop] gate '{code}' ({name}): {signature}");
        }
        catch { }
    }

    /// <summary>Drops the change-detection cache on scene reload so the first resolve per shop logs again.</summary>
    public static void ResetLogState()
    {
        _lastLoggedGate.Clear();
    }
}

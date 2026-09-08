using System;
using System.Collections.Generic;
using Il2CppScheduleOne.Growing;
using Il2CppScheduleOne.Money;
using MelonLoader;
using PotScanner.Utils;
using UnityEngine;

namespace PotScanner.Services;

/// <summary>
/// v0.3.0 (Polling variant): Auto-Water feature. When enabled, dry player-owned pots
/// are watered automatically on each PotTracker tick (every 2s real-time).
///
/// Replaces the original Harmony-Patch on GrowContainer.OnMinPass, which didn't fire
/// reliably in IL2CPP (the C# wrapper OnMinPass() can be bypassed when native code
/// calls il2cpp_runtime_invoke directly — see AGENTS.md §5 IL2CPP-Pitfalls).
/// Polling is the pragmatic fallback: PotTracker.Refresh() already runs every 2s and
/// the scene is small (<100 GrowContainers typical), so the extra scan is negligible.
///
/// Throttled per-pot via a private ptr→realtime dictionary. Default 5s cooldown =
/// max 1 water per pot per ~5 wall-clock seconds. Same cost as WaterAll (50g/pot) +
/// same 30% skip-threshold. Cash-capped per pot (never goes negative).
/// </summary>
public static class AutoWaterService
{
    private static bool _enabled;
    private static int _logCount;
    private const int MaxLogsPerSession = 10;
    /// <summary>Min seconds between auto-water on the same pot. Cheap safety belt — prevents spam if drain is unrealistically fast.</summary>
    private const float PerPotCooldownSec = 5f;

    /// <summary>ptr → Time.realtimeSinceStartup of last auto-water. Per-pot throttle.</summary>
    private static readonly Dictionary<IntPtr, float> _lastWaterByPtr = new();

    public static bool IsEnabled => _enabled;

    /// <summary>Called by Mod in OnInitializeMelon. No-op for polling variant (no Harmony to apply).</summary>
    public static void Initialize() { }

    /// <summary>Called by Mod in OnApplicationQuit. No-op for polling variant (no Harmony to remove).</summary>
    public static void Shutdown() { }

    /// <summary>Clears tracked pointer timestamps on scene transitions to prevent stale memory pointers.</summary>
    public static void ResetCache()
    {
        _lastWaterByPtr.Clear();
    }

    /// <summary>Updates the toggle state. Polling checks IsEnabled on every Tick().</summary>
    public static void SetEnabled(bool enabled)
    {
        if (_enabled == enabled) return;
        _enabled = enabled;
        MelonLogger.Msg($"Auto-Water {(enabled ? "ENABLED" : "DISABLED")} (polling-mode).");
    }

    /// <summary>
    /// Called by PotTracker during Refresh(). Iterates the cached pot list and waters
    /// any dry owned pot (subject to per-pot cooldown + cash gate).
    /// Accepts the already-discovered GrowContainer array to avoid a duplicate scene scan.
    /// </summary>
    public static void WaterTick(IReadOnlyList<PotInfo> pots, GrowContainer[]? containers = null)
    {
        if (!_enabled) return;
        if (pots == null || pots.Count == 0) return;

        // Fix 4.2 (Bug-Audit 2026-09-02): only the host/server may mutate pot moisture in MP.
        // Pattern copied from BusinessIncome.IncomeEngine.IsHostOrSingleplayer (IL2CPP-safe
        // Pointer/WasCollected checks; no NetworkManager in SP => true).
        try
        {
            var nm = Il2CppFishNet.InstanceFinder.NetworkManager;
            if (nm != null && nm.Pointer != IntPtr.Zero && !nm.WasCollected && (UnityEngine.Object)nm != null
                && !Il2CppFishNet.InstanceFinder.IsServer)
                return;
        }
        catch { /* offline / SP: allowed */ }

        float now = Time.realtimeSinceStartup;
        const float cost = Constants.WaterAllCostPerPot;
        const float threshold = Constants.WaterAllSkipThreshold;

        // Fast pre-check: any dry owned pot that needs watering and isn't on cooldown?
        // Skip map building / FindObjectsByType if not — the common case is "all pots moist".
        bool anyDry = false;
        for (int i = 0; i < pots.Count; i++)
        {
            var info = pots[i];
            if (!info.IsOwnedProperty) continue;
            if (info.WaterPercent >= threshold) continue;
            if (_lastWaterByPtr.TryGetValue(info.NativePtr, out var last) &&
                now - last < PerPotCooldownSec) continue;
            anyDry = true;
            break;
        }
        if (!anyDry) return;

        // Resolve MoneyManager
        MoneyManager? money;
        try { money = MoneyManager.Instance; }
        catch { return; }
        if (money == null || money.cashBalance < cost) return;

        // Use supplied containers or fallback to scene scan if null
        if (containers == null)
        {
            try { containers = UnityEngine.Object.FindObjectsByType<GrowContainer>(FindObjectsSortMode.None); }
            catch { return; }
        }
        if (containers == null || containers.Length == 0) return;

        var byPtr = new Dictionary<IntPtr, GrowContainer>(containers.Length);
        for (int i = 0; i < containers.Length; i++)
        {
            var c = containers[i];
            if (c == null || c.Equals(null)) continue;
            try
            {
                var p = c.Pointer;
                if (p != IntPtr.Zero) byPtr[p] = c;
            }
            catch { }
        }

        int wateredThisTick = 0;
        float totalCharged = 0f;

        for (int i = 0; i < pots.Count; i++)
        {
            var info = pots[i];
            if (!info.IsOwnedProperty) continue;
            if (info.WaterPercent >= threshold) continue;
            if (_lastWaterByPtr.TryGetValue(info.NativePtr, out var last) &&
                now - last < PerPotCooldownSec) continue;
            if (money.cashBalance < cost) break;
            if (!byPtr.TryGetValue(info.NativePtr, out var c)) continue;

            try
            {
                float capacity = c.MoistureCapacity;
                if (capacity <= 0f) continue;
                c.SetMoistureAmount(capacity);
                money.ChangeCashBalance(-cost, visualizeChange: false, playCashSound: false);
                _lastWaterByPtr[info.NativePtr] = now;
                info.WaterPercent = c.NormalizedMoistureAmount;
                wateredThisTick++;
                totalCharged += cost;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($" AutoWater failed on pot @ {info.NativePtr:X}: {ex.Message}");
            }
        }

        if (wateredThisTick > 0 && _logCount < MaxLogsPerSession)
        {
            _logCount++;
            MelonLogger.Msg($" [AutoWater] tick: watered {wateredThisTick} pot(s) for ${totalCharged:0} (bal=${money.cashBalance:0})");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppScheduleOne.Growing;
using Il2CppScheduleOne.Money;
using MelonLoader;
using PotScanner.Utils;
using UnityEngine;

namespace PotScanner.Services;

/// <summary>
/// Result of a WaterAll() call. Returned so the UI can show a meaningful message
/// and distinguish "succeeded", "no targets", "insufficient funds", "busy", etc.
/// </summary>
/// <param name="Success">True if all requested pots were watered without error.</param>
/// <param name="Requested">Number of pots that passed the ownership + scene-validity filter.</param>
/// <param name="Watered">How many were actually watered (may be less on per-pot failure).</param>
/// <param name="Charged">Total cash deducted (0 if not reached the charge step).</param>
/// <param name="Message">Human-readable status, suitable for log/toast.</param>
public sealed record WaterAllResult(
    bool Success,
    int Requested,
    int Watered,
    float Charged,
    string Message
);

/// <summary>
/// Bulk-action service: waters every player-owned pot to 100% moisture, charging cash per pot.
/// Implements the 10-step validate-then-charge flow defined in the v0.2.0 spec.
/// </summary>
public static class WaterAllService
{
    private static bool _isWatering;

    /// <summary>True while a WaterAll() call is in progress. Used by the UI to lock the button.</summary>
    public static bool IsWatering => _isWatering;

    /// <summary>
    /// 10-step validate-then-charge flow (v0.2.2 adds skip-when-sufficiently-watered):
    ///   0. HOST GUARD  — only host/server may mutate moisture + cash in MP (clients: silent no-op)
    ///   1. LOCK        — reject re-entry while a previous call is mid-flight
    ///   2. SNAPSHOT    — freeze the current pot list to avoid races with concurrent Refresh
    ///   3. RESOLVE     — single full scan of GrowContainers, build IntPtr → container dictionary
    ///   4. TARGET LIST — filter by IsOwnedProperty + scene-validity + WaterPercent &lt; threshold
    ///   5. VALIDATE    — non-empty targets, sufficient cash (only target count is charged)
    ///   6. CHARGE      — exact-once atomic deduction
    ///   7. WATER       — SetMoistureAmount(capacity) per target, per-pot try/catch
    ///   8. REFRESH     — RefreshNow() for immediate UI feedback
    ///   9. RESULT      — build WaterAllResult with skip counts in Message
    ///  10. UNLOCK      — finally block always releases the lock
    /// </summary>
    public static WaterAllResult WaterAll()
    {
        // 0. HOST GUARD (Fix 4.3): networked GrowContainer moisture + cash must only change on
        // the host. Same guard as AutoWaterService.WaterTick — silent no-op for clients.
        try
        {
            var nm = Il2CppFishNet.InstanceFinder.NetworkManager;
            if (nm != null && nm.Pointer != IntPtr.Zero && !nm.WasCollected && (UnityEngine.Object)nm != null
                && !Il2CppFishNet.InstanceFinder.IsServer)
                return new WaterAllResult(false, 0, 0, 0f, "Client: host only");
        }
        catch { /* offline / SP: allowed */ }

        // 1. LOCK
        if (_isWatering)
            return new WaterAllResult(false, 0, 0, 0f, "Already active");
        _isWatering = true;
        try
        {
            // 2. SNAPSHOT
            var snapshot = PotTracker.Instance.Pots.ToList();
            if (snapshot.Count == 0)
                return new WaterAllResult(false, 0, 0, 0f, "No pots found");

            // 3. RESOLVE — one full scene scan, dictionary for O(1) lookups
            var all = UnityEngine.Object.FindObjectsByType<GrowContainer>(FindObjectsSortMode.None);
            var byPtr = new Dictionary<IntPtr, GrowContainer>(all.Length);
            foreach (var c in all)
            {
                if (c == null || c.Equals(null)) continue;
                var ptr = c.Pointer;
                if (ptr != IntPtr.Zero) byPtr[ptr] = c;
            }

            // 4. TARGET LIST (v0.2.2: skip pots that are already sufficiently watered)
            var targets = new List<(IntPtr ptr, GrowContainer container)>();
            int foreignSkipped = 0;
            int missingFromScene = 0;
            int dryEnoughSkipped = 0;
            float threshold = Constants.WaterAllSkipThreshold;
            foreach (var info in snapshot)
            {
                if (!info.IsOwnedProperty) { foreignSkipped++; continue; }
                if (!byPtr.TryGetValue(info.NativePtr, out var c)) { missingFromScene++; continue; }
                if (info.WaterPercent >= threshold) { dryEnoughSkipped++; continue; }
                targets.Add((info.NativePtr, c));
            }

            // 5. VALIDATE (cost is based on target count, not all owned pots)
            var totalCost = targets.Count * Constants.WaterAllCostPerPot;
            var money = MoneyManager.Instance;
            if (money == null)
                return new WaterAllResult(false, targets.Count, 0, 0f, "MoneyManager not available");

            // 5a. EMPTY-TARGETS: every owned pot is already sufficiently watered
            if (targets.Count == 0)
            {
                var skipMsg = (foreignSkipped, dryEnoughSkipped) switch
                {
                    ( > 0, > 0) => $"No owned pots need water ({dryEnoughSkipped} already moist, {foreignSkipped} unowned)",
                    ( > 0, _) => $"No owned pots ({foreignSkipped} unowned)",
                    (_, > 0) => $"All owned pots are sufficiently watered ({dryEnoughSkipped} already moist, skipped)",
                    _ => "No owned pots"
                };
                return new WaterAllResult(false, 0, 0, 0f, skipMsg);
            }

            if (money.cashBalance < totalCost)
                return new WaterAllResult(false, targets.Count, 0, 0f,
                    $"Not enough cash ({totalCost} needed, {money.cashBalance} available)");

            // 6. CHARGE & 7. WATER — per-pot try/catch, only successfully watered pots are charged
            int watered = 0;
            float totalCharged = 0f;
            foreach (var (_, c) in targets)
            {
                try
                {
                    if (c.NormalizedMoistureAmount < 0.99f)
                    {
                        c.SetMoistureAmount(c.MoistureCapacity);
                        money.ChangeCashBalance(-Constants.WaterAllCostPerPot, visualizeChange: true, playCashSound: false);
                        watered++;
                        totalCharged += Constants.WaterAllCostPerPot;
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($" Failed to water pot @ {c.Pointer:X}: {ex.Message}");
                }
            }

            // 8. REFRESH — immediate UI update (synchronous OnPotsScanned)
            PotTracker.Instance.RefreshNow();

            // 9. RESULT
            var success = watered == targets.Count;
            var baseMsg = success
                ? $"{watered}/{targets.Count} pot(s) watered for {totalCharged:0}g"
                : $"{watered}/{targets.Count} pot(s) watered ({targets.Count - watered} failed) for {totalCharged:0}g";
            var msg = dryEnoughSkipped > 0
                ? $"{baseMsg} ({dryEnoughSkipped} already watered, skipped)"
                : baseMsg;
            return new WaterAllResult(success, targets.Count, watered, totalCharged, msg);
        }
        catch (Exception ex)
        {
            MelonLogger.Error($" WaterAll() crashed: {ex}");
            return new WaterAllResult(false, 0, 0, 0f, $"Error: {ex.Message}");
        }
        finally
        {
            // 10. UNLOCK — always
            _isWatering = false;
        }
    }

    /// <summary>
    /// Compute button label + enabled state without running WaterAll().
    /// Used by the UI to keep the TopBar button in sync on every OnPotsScanned.
    /// v0.2.2: returns count of *thirsty* pots (i.e. those that would actually be watered),
    /// not total owned pots. The button is disabled when no pot actually needs water.
    /// </summary>
    /// <returns>
    /// Tuple: (thirstyCount, totalCost, canEnable, disabledReason).
    /// canEnable is false when no thirsty pot, insufficient cash, or watering in progress.
    /// </returns>
    public static (int thirstyCount, float totalCost, bool canEnable, string disabledReason, float minWater) GetButtonState()
    {
        var pots = PotTracker.Instance.Pots;
        int ownedCount = 0;
        int thirstyCount = 0;
        float threshold = Constants.WaterAllSkipThreshold;
        float minWater = 1.0f;
        for (int i = 0; i < pots.Count; i++)
        {
            var p = pots[i];
            if (!p.IsOwnedProperty) continue;
            ownedCount++;
            if (p.WaterPercent < threshold) thirstyCount++;
            if (p.WaterPercent < minWater) minWater = p.WaterPercent;
        }

        float totalCost = thirstyCount * Constants.WaterAllCostPerPot;

        if (_isWatering) return (thirstyCount, totalCost, false, "Watering…", minWater);
        if (ownedCount == 0) return (thirstyCount, totalCost, false, "No owned pots", minWater);
        if (thirstyCount == 0) return (thirstyCount, totalCost, false, "All pots watered", minWater);
        var bal = MoneyManager.Instance?.cashBalance ?? 0f;
        if (bal < totalCost)
            return (thirstyCount, totalCost, false, $"Not enough cash ({totalCost} needed, {bal} available)", minWater);

        return (thirstyCount, totalCost, true, "", minWater);
    }

    /// <summary>
    /// Waters a single pot by native pointer and charges WaterAllCostPerPot.
    /// Ownership + skip-threshold guards mirror WaterAll() so foreign or already-moist pots are never charged.
    /// </summary>
    public static void WaterSinglePot(IntPtr ptr)
    {
        // Fix 4.3: host guard, same as WaterAll() above — silent no-op for clients.
        try
        {
            var nm = Il2CppFishNet.InstanceFinder.NetworkManager;
            if (nm != null && nm.Pointer != IntPtr.Zero && !nm.WasCollected && (UnityEngine.Object)nm != null
                && !Il2CppFishNet.InstanceFinder.IsServer)
                return;
        }
        catch { /* offline / SP: allowed */ }

        if (ptr == IntPtr.Zero) return;
        var money = MoneyManager.Instance;
        if (money == null || money.cashBalance < Constants.WaterAllCostPerPot) return;

        try
        {
            var all = UnityEngine.Object.FindObjectsByType<GrowContainer>(FindObjectsSortMode.None);
            for (int i = 0; i < all.Length; i++)
            {
                var c = all[i];
                if (c == null || c.Equals(null)) continue;
                if (c.Pointer == ptr)
                {
                    if (c.NormalizedMoistureAmount < 0.99f)
                    {
                        float capacity = c.MoistureCapacity;
                        if (capacity <= 0f) return;

                        // Ownership guard: nur eigene Töpfe (konsistent zu WaterAll).
                        var info = PotTracker.Instance.FindByPtr(ptr);
                        if (info == null || !info.IsOwnedProperty) return;

                        // Skip-Threshold: bereits ausreichend feuchte Töpfe nicht erneut berechnen.
                        if (info.WaterPercent >= Constants.WaterAllSkipThreshold) return;

                        c.SetMoistureAmount(capacity);
                        money.ChangeCashBalance(-Constants.WaterAllCostPerPot, visualizeChange: true, playCashSound: false);
                        PotTracker.Instance.RefreshNow();
                    }
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($" Failed to single-water pot @ {ptr:X}: {ex.Message}");
        }
    }
}

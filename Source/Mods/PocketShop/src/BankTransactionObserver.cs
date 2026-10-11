using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Il2CppScheduleOne.Money;
using S1Mods.Shared;
using UnityEngine;

namespace PocketShop.Services;

/// <summary>
/// Records bank transactions at the exact moment the vanilla MoneyManager applies them
/// on the server (<c>ReceiveOnlineTransaction</c>).
///
/// Why: <c>CreateOnlineTransaction</c> is a FishNet ServerRpc. The caller only sends the
/// packet; the balance changes when the server processes it on a later network tick.
/// A synchronous read-back right after the call therefore cannot see the change — the
/// deferred confirmation needs a deterministic signal that the transaction actually
/// applied, with its exact amount. This observer provides that signal.
///
/// The postfix only records (title, applied amount, timestamp). It never alters,
/// blocks or reorders vanilla behaviour; if the patch cannot be applied the purchase
/// flow falls back to balance-delta observation alone.
/// </summary>
public static class BankTransactionObserver
{
    public const string OrderTitle = "PocketShop Order";
    public const string RefundTitle = "PocketShop Refund";

    private const int HistorySize = 16;

    private static readonly List<(float At, string Title, float Amount)> _applied = new(HistorySize);
    private static bool _patchApplied;

    /// <summary>True when the vanilla hook is installed and can confirm applied transactions.</summary>
    public static bool IsAvailable => _patchApplied;

    public static bool TryApplyPatch(HarmonyLib.Harmony harmony, ModLogger log)
    {
        MethodInfo? postfixMethod = null;
        try
        {
            postfixMethod = typeof(BankTransactionObserver).GetMethod(
                nameof(OnTransactionApplied), BindingFlags.Static | BindingFlags.NonPublic);
        }
        catch (Exception ex)
        {
            log?.Warn($"BankTransactionObserver: postfix lookup failed: {ex.Message}");
        }

        if (postfixMethod == null)
        {
            _patchApplied = false;
            return false;
        }

        // Exact signature pins the overload: private void ReceiveOnlineTransaction(string, float, float, string).
        bool ok = PatchGuard.TryPatch(
            harmony,
            typeof(MoneyManager),
            "ReceiveOnlineTransaction",
            prefix: null,
            postfix: new HarmonyMethod(postfixMethod),
            parameterTypes: new[] { typeof(string), typeof(float), typeof(float), typeof(string) },
            log: log);

        _patchApplied = ok;
        if (ok)
            log?.Info("BankTransactionObserver: ReceiveOnlineTransaction hook active (deferred bank confirmation).");
        else
            log?.Warn("BankTransactionObserver: hook unavailable — bank confirmation falls back to balance observation.");
        return ok;
    }

    /// <summary>
    /// Harmony postfix. Positional arguments (__0..__2) are version-proof: the IL2CPP
    /// proxy exposes the original parameter names, but only the order is guaranteed.
    /// Never throws (a patch must not break the game's money path).
    /// </summary>
    private static void OnTransactionApplied(string __0, float __1, float __2)
    {
        try
        {
            float amount = __1 * __2;
            if (!float.IsFinite(amount)) return;
            lock (_applied)
            {
                _applied.Add((Time.realtimeSinceStartup, __0 ?? string.Empty, amount));
                if (_applied.Count > HistorySize)
                    _applied.RemoveAt(0);
            }
        }
        catch
        {
            // Recording is best-effort; the balance-delta observation remains the fallback.
        }
    }

    /// <summary>
    /// True when a transaction with the same title and exactly the expected applied
    /// amount was observed at or after <paramref name="sentAt"/> (small clock tolerance
    /// for ordering only). Money amounts are compared exactly.
    /// </summary>
    public static bool TryFindApplied(string title, float amount, float sentAt, out float appliedAt)
    {
        appliedAt = 0f;
        lock (_applied)
        {
            for (int i = _applied.Count - 1; i >= 0; i--)
            {
                var entry = _applied[i];
                if (PurchaseRules.MatchesAppliedBankTransaction(title, amount, sentAt, entry.Title, entry.Amount, entry.At))
                {
                    appliedAt = entry.At;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>Drops the recorded history when the gameplay scene tears down (new session).</summary>
    public static void ResetForSceneReload()
    {
        lock (_applied)
        {
            _applied.Clear();
        }
    }
}

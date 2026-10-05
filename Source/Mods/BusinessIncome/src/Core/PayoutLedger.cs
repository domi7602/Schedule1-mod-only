using System;
using System.Collections.Generic;
using BusinessIncome.Models;

namespace BusinessIncome.Core;

/// <summary>
/// Monotone counter bookkeeping and exact-snapshot rollback for the payout state.
/// The global LastPaidElapsedDay and every per-business day only ever move forward;
/// rollback (used only BEFORE a bank call is attempted) happens solely when the
/// in-memory state still matches the snapshot exactly.
/// </summary>
public static class PayoutLedger
{
    /// <summary>Monotone global/per-business mark. Never decreases a counter.</summary>
    public static void MarkPaid(PayoutState state, int day, IEnumerable<string> businessIds)
    {
        state.LastPaidElapsedDay = Math.Max(state.LastPaidElapsedDay, day);
        foreach (string id in businessIds)
        {
            if (string.IsNullOrEmpty(id)) continue;
            if (state.LastPaidDayByBusiness.TryGetValue(id, out int prev))
                state.LastPaidDayByBusiness[id] = Math.Max(prev, day);
            else
                state.LastPaidDayByBusiness[id] = day;
        }
        state.LastPayoutTimestamp = DateTime.UtcNow.ToString("o");
    }

    /// <summary>Prepares a snapshot and applies the monotone forward mark in one step.</summary>
    public static PayoutSnapshot CaptureAndMark(PayoutState state, int day, IReadOnlyList<string> businessIds)
    {
        var snap = new PayoutSnapshot
        {
            PrevLastPaid = state.LastPaidElapsedDay,
            PrevIdentity = state.SaveIdentity,
            PrevTimestamp = state.LastPayoutTimestamp,
            PrevPerBusiness = new Dictionary<string, int?>(StringComparer.OrdinalIgnoreCase),
            ExpectedPerBusiness = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        };

        foreach (string id in businessIds)
        {
            if (string.IsNullOrEmpty(id)) continue;
            snap.PrevPerBusiness[id] = state.LastPaidDayByBusiness.TryGetValue(id, out int prev) ? prev : null;
        }

        MarkPaid(state, day, businessIds);

        snap.ExpectedLastPaid = state.LastPaidElapsedDay;
        snap.ExpectedTimestamp = state.LastPayoutTimestamp;
        foreach (string id in businessIds)
        {
            if (string.IsNullOrEmpty(id)) continue;
            snap.ExpectedPerBusiness[id] = state.LastPaidDayByBusiness[id];
        }
        return snap;
    }

    /// <summary>
    /// Restores the snapshot ONLY when the live state still matches it exactly
    /// (global day, every marked business, identity AND timestamp). Returns false on any
    /// divergence so a concurrent/partial/unrelated mutation is never silently clobbered.
    /// </summary>
    public static bool TryRollbackExact(PayoutState state, PayoutSnapshot snapshot)
    {
        if (state.LastPaidElapsedDay != snapshot.ExpectedLastPaid) return false;
        if (!string.Equals(state.SaveIdentity, snapshot.PrevIdentity, StringComparison.Ordinal)) return false;
        if (!string.Equals(state.LastPayoutTimestamp, snapshot.ExpectedTimestamp, StringComparison.Ordinal)) return false;
        foreach (var kv in snapshot.ExpectedPerBusiness)
        {
            if (!state.LastPaidDayByBusiness.TryGetValue(kv.Key, out int cur) || cur != kv.Value)
                return false;
        }

        state.LastPaidElapsedDay = snapshot.PrevLastPaid;
        state.SaveIdentity = snapshot.PrevIdentity;
        state.LastPayoutTimestamp = snapshot.PrevTimestamp;
        foreach (var kv in snapshot.PrevPerBusiness)
        {
            if (kv.Value.HasValue) state.LastPaidDayByBusiness[kv.Key] = kv.Value.Value;
            else state.LastPaidDayByBusiness.Remove(kv.Key);
        }
        return true;
    }
}

/// <summary>Immutable record of the pre-mark state plus the expected post-mark values.</summary>
public sealed class PayoutSnapshot
{
    public int PrevLastPaid;
    public string PrevIdentity = "";
    public string PrevTimestamp = "";
    public Dictionary<string, int?> PrevPerBusiness { get; init; } = new();
    public int ExpectedLastPaid;
    /// <summary>Timestamp written by the mark; rollback refuses if it has since diverged.</summary>
    public string ExpectedTimestamp = "";
    public Dictionary<string, int> ExpectedPerBusiness { get; init; } = new();
}

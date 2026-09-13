using System;
using System.Collections.Generic;

namespace BusinessIncome.Models;

/// <summary>
/// Persistent payout state for a specific save game (save slot).
/// Prevents duplicate payouts on the same day even after game restart or save reload.
/// </summary>
public sealed class PayoutState
{
    public int SchemaVersion { get; set; } = 1;
    public int LastPaidElapsedDay { get; set; } = -1;
    public string SaveIdentity { get; set; } = "";
    public string LastPayoutTimestamp { get; set; } = "";
    public Dictionary<string, int> LastPaidDayByBusiness { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

/// <summary>
/// Write-ahead marker (F1): written to disk BEFORE the bank transaction and cleared only
/// after the payout state is durably committed. If the game crashes in between, the marker
/// survives and startup warns about a possibly-booked-but-not-committed payout instead of
/// silently paying the day twice.
/// </summary>
public sealed class PendingPayoutState
{
    public int Day { get; set; } = -1;
    public float Amount { get; set; }
    public int BusinessCount { get; set; }
    public string StartedUtc { get; set; } = "";
}

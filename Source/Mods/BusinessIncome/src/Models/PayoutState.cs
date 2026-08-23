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

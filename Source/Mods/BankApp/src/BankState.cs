using System;
using System.Collections.Generic;

namespace BankApp.Models;

/// <summary>
/// Root data structure persisted per save-slot.
/// </summary>
public sealed class BankState
{
    public float WeeklyDepositedAmount { get; set; }
    public int LastRecordedWeek { get; set; } = -1;
    public List<BankTransaction> Transactions { get; set; } = new();
}

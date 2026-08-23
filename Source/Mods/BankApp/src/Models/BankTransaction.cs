using System;

namespace BankApp.Models;

public enum TransactionType
{
    Deposit,
    Withdrawal,
    TransferIn,
    TransferOut,
    Fee
}

/// <summary>
/// A single recorded bank transaction entry.
/// </summary>
public sealed class BankTransaction
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public int InGameDay { get; set; } = 1;
    public string InGameTime { get; set; } = "08:00";
    public float Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public float BalanceAfter { get; set; }
    public long TimestampEpoch { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}

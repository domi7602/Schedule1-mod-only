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
/// <para>
/// <see cref="Amount"/> is the signed change applied to the bank balance for entries written from
/// now on (deposits store the fee-netted credit; withdrawals store the gross+fee debit).
/// Legacy entries written before this field carried a fee breakdown keep their historical value —
/// a legacy withdrawal stored only <c>-gross</c> — so <see cref="Gross"/>/<see cref="Fee"/> stay
/// null for them and no fee is retro-fitted.
/// </para>
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

    /// <summary>Gross amount the player moved, for entries written with a fee breakdown. Null on legacy entries.</summary>
    public float? Gross { get; set; }

    /// <summary>Fee applied to this entry, for entries written with a fee breakdown. Null on legacy entries.</summary>
    public float? Fee { get; set; }

    /// <summary>True when this entry records an explicit gross/fee split (written by the current build).</summary>
    public bool HasFeeBreakdown => Gross.HasValue && Fee.HasValue;
}

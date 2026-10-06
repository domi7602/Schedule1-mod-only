using System.Text.Json;
using BankApp.Models;
using Xunit;

namespace BankApp.Tests;

public class SerializationTests
{
    // System.Text.Json defaults match SafeStorage's options for these members (enums as numbers,
    // deserialization is case-insensitive there but not here — both must round-trip).

    [Fact]
    public void LegacyJson_WithoutFeeFields_DeserializesWithNoBreakdown()
    {
        const string legacyJson =
            "{\"Id\":\"legacy1\",\"InGameDay\":20,\"InGameTime\":\"08:00\",\"Amount\":-2500," +
            "\"Type\":1,\"Description\":\"Cash Withdrawal\",\"BalanceAfter\":5000,\"TimestampEpoch\":123}";

        BankTransaction? tx = JsonSerializer.Deserialize<BankTransaction>(legacyJson);

        Assert.NotNull(tx);
        Assert.False(tx!.HasFeeBreakdown);
        Assert.Null(tx.Gross);
        Assert.Null(tx.Fee);
        Assert.Equal(-2500f, tx.Amount, 4);
    }

    [Fact]
    public void NewEntry_WithFeeFields_RoundTrips()
    {
        var entry = new BankTransaction
        {
            Id = "new1",
            InGameDay = 21,
            InGameTime = "11:48",
            Amount = -2550f,
            Type = TransactionType.Withdrawal,
            Description = "Mobile Cash Withdrawal",
            BalanceAfter = 9900f,
            Gross = 2500f,
            Fee = 50f
        };

        string json = JsonSerializer.Serialize(entry);
        Assert.Contains("\"Gross\"", json);
        Assert.Contains("\"Fee\"", json);

        BankTransaction? back = JsonSerializer.Deserialize<BankTransaction>(json);
        Assert.NotNull(back);
        Assert.True(back!.HasFeeBreakdown);
        Assert.Equal(2500f, back.Gross!.Value, 4);
        Assert.Equal(50f, back.Fee!.Value, 4);
        Assert.Equal(-2550f, back.Amount, 4);
    }
}

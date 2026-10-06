using BankApp.Logic;
using BankApp.Models;
using Xunit;

namespace BankApp.Tests;

public class HistoryAndNavigationTests
{
    private static BankTransaction Tx(int day, float amount, TransactionType type, float? gross = null, float? fee = null) =>
        new()
        {
            InGameDay = day,
            Amount = amount,
            Type = type,
            Description = type.ToString(),
            Gross = gross,
            Fee = fee
        };

    [Fact]
    public void LegacyEntry_HasNoFabricatedFeeBreakdown()
    {
        var legacy = Tx(20, -2500f, TransactionType.Withdrawal); // stores -gross only (pre-change shape)

        Assert.False(legacy.HasFeeBreakdown);
        Assert.Null(legacy.Gross);
        Assert.Null(legacy.Fee);
        Assert.Equal(-2500f, legacy.Amount, 4);
    }

    [Fact]
    public void NewEntry_CarriesGrossAndFee()
    {
        var entry = Tx(21, -2550f, TransactionType.Withdrawal, gross: 2500f, fee: 50f);

        Assert.True(entry.HasFeeBreakdown);
        Assert.Equal(2500f, entry.Gross!.Value, 4);
        Assert.Equal(50f, entry.Fee!.Value, 4);
        Assert.Equal(-2550f, entry.Amount, 4); // actual bank delta
    }

    [Theory]
    [InlineData(27, 27, "TODAY")]
    [InlineData(26, 27, "YESTERDAY")]
    [InlineData(25, 27, "DAY 25")]
    public void LabelFor_UsesRelativeCaptions(int day, int currentDay, string expected)
    {
        Assert.Equal(expected, HistoryGrouping.LabelFor(day, currentDay));
    }

    [Fact]
    public void GroupByDay_PreservesNewestFirstOrderAndSplitsDays()
    {
        var list = new List<BankTransaction>
        {
            Tx(27, 5000f, TransactionType.Deposit),
            Tx(27, -2550f, TransactionType.Withdrawal, 2500f, 50f),
            Tx(26, 3000f, TransactionType.Deposit),
            Tx(25, 10000f, TransactionType.TransferIn),
        };

        var groups = HistoryGrouping.GroupByDay(list, 27);

        Assert.Equal(3, groups.Count);
        Assert.Equal("TODAY", groups[0].Label);
        Assert.Equal(2, groups[0].Items.Count);
        Assert.Equal(5000f, groups[0].Items[0].Amount, 4);   // insertion order preserved
        Assert.Equal(-2550f, groups[0].Items[1].Amount, 4);
        Assert.Equal("YESTERDAY", groups[1].Label);
        Assert.Equal("DAY 25", groups[2].Label);
    }

    [Fact]
    public void GroupByDay_EmptyOrNull_YieldsNoGroups()
    {
        Assert.Empty(HistoryGrouping.GroupByDay(new List<BankTransaction>(), 27));
        Assert.Empty(HistoryGrouping.GroupByDay(null, 27));
    }

    [Fact]
    public void Back_AlwaysReturnsToOverview()
    {
        Assert.Equal(BankTab.Overview, BankNavigation.Back(BankTab.Transaction));
        Assert.Equal(BankTab.Overview, BankNavigation.Back(BankTab.Overview));
        Assert.Equal(BankTab.Overview, BankNavigation.DefaultTab);
    }

    [Fact]
    public void TransactionPane_IsASeparateTab()
    {
        Assert.NotEqual(BankTab.Overview, BankTab.Transaction);
        Assert.Equal(new[] { BankTab.Overview, BankTab.Transaction }, Enum.GetValues<BankTab>());
    }
}

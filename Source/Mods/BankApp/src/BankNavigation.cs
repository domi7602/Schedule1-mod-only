using System.Collections.Generic;
using BankApp.Models;

namespace BankApp.Logic;

/// <summary>One day's slice of the activity feed, newest day first, entries kept in stored order.</summary>
public sealed class HistoryDayGroup
{
    public int Day { get; init; }
    public string Label { get; init; } = string.Empty;
    public List<BankTransaction> Items { get; init; } = new();
}

/// <summary>
/// Unity-free grouping for the Activity tab. The persisted list is already newest-first
/// (TransactionHistoryService inserts at index 0), so grouping purely preserves that order
/// while inserting a day header — no re-sorting that could scramble same-timestamp entries.
/// </summary>
public static class HistoryGrouping
{
    public const string TodayLabel = "TODAY";
    public const string YesterdayLabel = "YESTERDAY";

    /// <summary>Relative day caption: TODAY / YESTERDAY / DAY N.</summary>
    public static string LabelFor(int day, int currentDay)
    {
        if (day >= currentDay) return TodayLabel;
        if (day == currentDay - 1) return YesterdayLabel;
        return $"DAY {day}";
    }

    /// <summary>Groups transactions by in-game day, preserving the incoming (newest-first) order.</summary>
    public static List<HistoryDayGroup> GroupByDay(IReadOnlyList<BankTransaction>? transactions, int currentDay)
    {
        var groups = new List<HistoryDayGroup>();
        if (transactions == null) return groups;

        var indexByDay = new Dictionary<int, int>();
        for (int i = 0; i < transactions.Count; i++)
        {
            BankTransaction tx = transactions[i];
            if (tx == null) continue;

            if (!indexByDay.TryGetValue(tx.InGameDay, out int groupIndex))
            {
                groupIndex = groups.Count;
                indexByDay[tx.InGameDay] = groupIndex;
                groups.Add(new HistoryDayGroup
                {
                    Day = tx.InGameDay,
                    Label = LabelFor(tx.InGameDay, currentDay),
                    Items = new List<BankTransaction>()
                });
            }

            groups[groupIndex].Items.Add(tx);
        }

        return groups;
    }
}

/// <summary>Available banking screens.</summary>
public enum BankTab
{
    Overview,
    Transaction
}

/// <summary>
/// Trivial, testable navigation state for the transaction back button.
/// Kept separate from the Unity view so the back target is covered by a plain unit test.
/// </summary>
public static class BankNavigation
{
    public const BankTab DefaultTab = BankTab.Overview;

    /// <summary>
    /// The back button of the Transaction pane returns to Overview;
    /// Overview itself stays put (Escape/close is handled by the phone, not here).
    /// </summary>
    public static BankTab Back(BankTab current) => BankTab.Overview;
}

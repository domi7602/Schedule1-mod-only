using System.Linq;
using StorageScanner;
using Xunit;

namespace StorageScanner.Tests;

/// <summary>Category filtering + grouping composition of the pure aggregator.</summary>
public class StockAggregatorCategoryTests
{
    private static StorageItem Item(string id, string name, long quantity, string category, string property = "p1")
        => new StorageItem(property, "Prop", "c-" + id + "-" + property, "Chest", id, name, quantity, false, category);

    private static StorageSnapshot Snapshot(params StorageItem[] items)
        => new StorageSnapshot(items, true, "test");

    [Fact]
    public void Summarize_FiltersExactOrdinalCategory()
    {
        var snapshot = Snapshot(
            Item("i1", "Rifle", 5, "Weapons"),
            Item("i2", "Pistol", 3, "weapons"),
            Item("i3", "Bread", 9, "Other"));

        var rows = StockAggregator.Summarize(snapshot, categoryId: "Weapons");

        Assert.Single(rows);
        Assert.Equal("i1", rows[0].ItemId);
    }

    [Fact]
    public void Summarize_NullCategory_ReturnsAllCategories()
    {
        var snapshot = Snapshot(
            Item("i1", "Rifle", 5, "Weapons"),
            Item("i2", "Bread", 9, "Other"));

        var rows = StockAggregator.Summarize(snapshot, categoryId: null);

        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void Summarize_RowCarriesGroupCategory()
    {
        var snapshot = Snapshot(Item("i1", "Rifle", 5, "Weapons"));

        var rows = StockAggregator.Summarize(snapshot);

        Assert.Equal("Weapons", rows[0].CategoryId);
    }

    [Fact]
    public void Summarize_UncategorizedItem_RowCategoryOther()
    {
        var snapshot = new StorageSnapshot(
            new[] { new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Item", 5) },
            true,
            "test");

        var rows = StockAggregator.Summarize(snapshot);

        Assert.Equal("Other", rows[0].CategoryId);
    }

    [Fact]
    public void Summarize_ComposesCategoryPropertySearchAndSort()
    {
        var snapshot = Snapshot(
            Item("i1", "Rifle", 5, "Weapons", "p1"),
            Item("i1", "Rifle", 2, "Weapons", "p2"),
            Item("i2", "Rifle Ammo", 10, "Weapons", "p1"),
            Item("i3", "Ring", 50, "Weapons", "p1"),
            Item("i9", "Rifle", 99, "Other", "p1"),
            Item("i4", "Bread", 100, "Other", "p1"));

        var rows = StockAggregator.Summarize(
            snapshot,
            propertyId: "p1",
            search: "ri",
            sortMode: StockSortMode.QuantityAscending,
            categoryId: "Weapons");

        Assert.Equal(new[] { "i1", "i2", "i3" }, rows.Select(r => r.ItemId).ToArray());
        Assert.All(rows, r => Assert.Equal("Weapons", r.CategoryId));
        Assert.Equal(5, rows.Single(r => r.ItemId == "i1").Quantity);
    }

    [Fact]
    public void Summarize_CachedStack_RetainsCategory_AndFlagsCached()
    {
        StorageItem cached = Item("i1", "Rifle", 5, "Weapons").AsCached();
        var snapshot = new StorageSnapshot(new[] { cached }, false, "cached");

        var rows = StockAggregator.Summarize(snapshot);

        Assert.Equal("Weapons", rows[0].CategoryId);
        Assert.True(rows[0].IncludesCachedData);
    }

    [Fact]
    public void RowsEquivalent_DiffersOnName()
    {
        var left = new[] { new StockRow("i1", "Rifle", 5, 1) };
        var right = new[] { new StockRow("i1", "Rifle Mk2", 5, 1) };

        Assert.False(StockAggregator.RowsEquivalent(left, right));
    }

    [Fact]
    public void RowsEquivalent_DiffersOnCategory()
    {
        var left = new[] { new StockRow("i1", "Rifle", 5, 1, categoryId: "Weapons") };
        var right = new[] { new StockRow("i1", "Rifle", 5, 1, categoryId: "Other") };

        Assert.False(StockAggregator.RowsEquivalent(left, right));
    }

    [Fact]
    public void RowsEquivalent_SameContent_True()
    {
        var left = new[] { new StockRow("i1", "Rifle", 5, 1, includesCachedData: false, categoryId: "Weapons") };
        var right = new[] { new StockRow("i1", "Rifle", 5, 1, includesCachedData: false, categoryId: "Weapons") };

        Assert.True(StockAggregator.RowsEquivalent(left, right));
    }

    [Fact]
    public void StockRow_NoCategoryArgument_FallsBackToOther()
    {
        var row = new StockRow("i1", "Rifle", 5, 1);

        Assert.Equal("Other", row.CategoryId);
    }
}

using System;
using System.Collections.Generic;
using Xunit;

namespace StorageScanner.Tests;

public sealed class StorageScannerCoreTests
{
    [Fact]
    public void Summarize_AggregatesQuantityAndDistinctContainers()
    {
        StorageSnapshot snapshot = Snapshot(
            Item("p1", "Barn", "c1", "Shelf", "coke", "Cocaine", 10),
            Item("p1", "Barn", "c1", "Shelf", "coke", "Cocaine", 5),
            Item("p1", "Barn", "c2", "Safe", "coke", "Cocaine", 20));

        IReadOnlyList<StockRow> rows = StockAggregator.Summarize(snapshot);

        Assert.Single(rows);
        Assert.Equal(35, rows[0].Quantity);
        Assert.Equal(2, rows[0].ContainerCount);
    }

    [Fact]
    public void Summarize_SearchesItemPropertyAndContainerNames()
    {
        StorageSnapshot snapshot = Snapshot(
            Item("p1", "Barn", "c1", "Steel Safe", "coke", "Cocaine", 10),
            Item("p2", "Motel", "c2", "Shelf", "acid", "Acid", 5));

        Assert.Single(StockAggregator.Summarize(snapshot, search: "barn"));
        Assert.Single(StockAggregator.Summarize(snapshot, search: "safe"));
        Assert.Single(StockAggregator.Summarize(snapshot, search: "acid"));
        Assert.Empty(StockAggregator.Summarize(snapshot, search: "warehouse"));
    }

    [Fact]
    public void Summarize_SortsByQuantityAndContainers()
    {
        StorageSnapshot snapshot = Snapshot(
            Item("p1", "Barn", "c1", "A", "a", "Alpha", 5),
            Item("p1", "Barn", "c2", "B", "a", "Alpha", 5),
            Item("p1", "Barn", "c3", "C", "b", "Beta", 100));

        IReadOnlyList<StockRow> byQty = StockAggregator.Summarize(snapshot, sortMode: StockSortMode.QuantityDescending);
        IReadOnlyList<StockRow> byContainers = StockAggregator.Summarize(snapshot, sortMode: StockSortMode.ContainersDescending);

        Assert.Equal("Beta", byQty[0].ItemName);
        Assert.Equal("Alpha", byContainers[0].ItemName);
    }

    [Fact]
    public void GetItemDetails_GroupsByPropertyAndContainer()
    {
        StorageSnapshot snapshot = Snapshot(
            Item("p1", "Barn", "c1", "Shelf", "coke", "Cocaine", 10),
            Item("p1", "Barn", "c1", "Shelf", "coke", "Cocaine", 5),
            Item("p2", "Motel", "c2", "Safe", "coke", "Cocaine", 20));

        ItemDetails? details = StockAggregator.GetItemDetails(snapshot, "coke");

        Assert.NotNull(details);
        Assert.Equal(35, details!.TotalQuantity);
        Assert.Equal(2, details.Locations.Count);
    }

    [Fact]
    public void PropertyCache_ReturnsCachedCopiesAndTimestamp()
    {
        var cache = new PropertyStorageCache();
        DateTimeOffset time = new DateTimeOffset(2026, 10, 4, 20, 0, 0, TimeSpan.Zero);
        StorageItem item = Item("p1", "Barn", "c1", "Shelf", "coke", "Cocaine", 10);

        cache.Store("p1", new[] { item }, 1, time);
        bool found = cache.TryGet("p1", out IReadOnlyList<StorageItem> cached, out int containers, out DateTimeOffset updatedAt);

        Assert.True(found);
        Assert.Single(cached);
        Assert.True(cached[0].IsCached);
        Assert.Equal(1, containers);
        Assert.Equal(time, updatedAt);
    }

    [Fact]
    public void RowsEqual_DetectsMeaningfulChanges()
    {
        StockRow[] before = { new StockRow("a", "Alpha", 10, 1, false) };
        StockRow[] same = { new StockRow("a", "Alpha", 10, 1, false) };
        StockRow[] changed = { new StockRow("a", "Alpha", 11, 1, false) };

        Assert.True(StockAggregator.RowsEqual(before, same));
        Assert.False(StockAggregator.RowsEqual(before, changed));
    }

    private static StorageItem Item(string propertyId, string propertyName, string containerId,
        string containerName, string itemId, string itemName, long quantity) =>
        new StorageItem(propertyId, propertyName, containerId, containerName, itemId, itemName, quantity);

    private static StorageSnapshot Snapshot(params StorageItem[] items) =>
        new StorageSnapshot(items,
            new[] { new PropertyScanState("p1", "Barn", true, DateTimeOffset.UtcNow, 2) },
            true, "ok", 2, 1, DateTimeOffset.UtcNow);
}

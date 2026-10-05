using System;
using System.Linq;
using StorageScanner;
using Xunit;

namespace StorageScanner.Tests;

/// <summary>Category semantics of the pure model: fallback, preservation and cache round-trip.</summary>
public class StorageItemCategoryTests
{
    [Fact]
    public void StorageItem_NoCategoryArgument_FallsBackToOther()
    {
        var item = new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Rifle", 5);

        Assert.Equal("Other", item.CategoryId);
    }

    [Fact]
    public void StorageItem_NullCategory_FallsBackToOther()
    {
        var item = new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Rifle", 5, categoryId: null);

        Assert.Equal("Other", item.CategoryId);
    }

    [Fact]
    public void StorageItem_WhitespaceCategory_FallsBackToOther()
    {
        var item = new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Rifle", 5, categoryId: "   ");

        Assert.Equal("Other", item.CategoryId);
    }

    [Fact]
    public void StorageItem_KeepsProvidedCategory()
    {
        var item = new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Rifle", 5, categoryId: "Weapons");

        Assert.Equal("Weapons", item.CategoryId);
    }

    [Fact]
    public void AsCached_PreservesCategory_AndMarksCached()
    {
        var item = new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Rifle", 5, categoryId: "Weapons");

        StorageItem cached = item.AsCached();

        Assert.True(cached.IsCached);
        Assert.Equal("Weapons", cached.CategoryId);
        Assert.False(item.IsCached);
    }

    [Fact]
    public void AsCached_UncategorizedItem_KeepsOtherFallback()
    {
        var item = new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Rifle", 5);

        StorageItem cached = item.AsCached();

        Assert.Equal("Other", cached.CategoryId);
    }

    [Fact]
    public void PropertyStorageCache_ReplayedItems_KeepCategory()
    {
        var items = new[]
        {
            new StorageItem("p1", "Prop", "c1", "Chest", "i1", "Rifle", 5, categoryId: "Weapons")
        };
        var cache = new PropertyStorageCache("p1", "Prop", items, DateTimeOffset.Now);

        StorageItem[] replayed = cache.Items.Select(x => x.AsCached()).ToArray();

        Assert.Equal("Weapons", replayed[0].CategoryId);
        Assert.True(replayed[0].IsCached);
    }
}

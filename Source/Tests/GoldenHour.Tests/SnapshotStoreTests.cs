using GoldenHour;
using Xunit;

namespace GoldenHour.Tests;

public class SnapshotStoreTests
{
    [Fact]
    public void RecordOnce_stores_value_and_TryGet_returns_it()
    {
        var store = new SnapshotStore();
        store.RecordOnce("sun.color", new Argb(1f, 1f, 1f, 1f));
        Assert.True(store.TryGet("sun.color", out var value));
        Assert.Equal(new Argb(1f, 1f, 1f, 1f), value);
    }

    [Fact]
    public void RecordOnce_is_first_wins_per_key()
    {
        var store = new SnapshotStore();
        store.RecordOnce("fog.color", new Argb(0.1f, 0.2f, 0.3f, 1f));
        store.RecordOnce("fog.color", new Argb(0.9f, 0.9f, 0.9f, 1f));
        Assert.True(store.TryGet("fog.color", out var value));
        Assert.Equal(new Argb(0.1f, 0.2f, 0.3f, 1f), value);
    }

    [Fact]
    public void TryGet_missing_key_returns_false()
    {
        var store = new SnapshotStore();
        Assert.False(store.TryGet("never.recorded", out _));
    }

    [Fact]
    public void Count_and_Keys_expose_the_recorded_set()
    {
        var store = new SnapshotStore();
        store.RecordOnce("a", Argb.White);
        store.RecordOnce("b", Argb.White);
        store.RecordOnce("a", Argb.White);
        Assert.Equal(2, store.Count);
        Assert.Contains("a", store.Keys);
        Assert.Contains("b", store.Keys);
    }

    [Fact]
    public void Update_replaces_a_recorded_value()
    {
        var store = new SnapshotStore();
        store.RecordOnce("sun.color", new Argb(0.1f, 0.2f, 0.3f, 1f));
        store.Update("sun.color", new Argb(0.9f, 0.8f, 0.7f, 1f));
        Assert.True(store.TryGet("sun.color", out var value));
        Assert.Equal(new Argb(0.9f, 0.8f, 0.7f, 1f), value);
    }

    [Fact]
    public void Update_sets_a_value_for_an_unknown_key()
    {
        var store = new SnapshotStore();
        store.Update("ambient.sky", new Argb(0.5f, 0.5f, 0.5f, 1f));
        Assert.True(store.TryGet("ambient.sky", out var value));
        Assert.Equal(new Argb(0.5f, 0.5f, 0.5f, 1f), value);
    }

    [Fact]
    public void Update_ignores_null_or_empty_keys()
    {
        var store = new SnapshotStore();
        store.Update("", new Argb(0.5f, 0.5f, 0.5f, 1f));
        store.Update(null, new Argb(0.5f, 0.5f, 0.5f, 1f));
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void Clear_resets_the_store_for_a_fresh_activation()
    {
        var store = new SnapshotStore();
        store.RecordOnce("a", Argb.White);
        store.Clear();
        Assert.Equal(0, store.Count);
        Assert.False(store.TryGet("a", out _));
    }
}

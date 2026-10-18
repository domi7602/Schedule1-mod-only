using Xunit;

namespace Minimap.Tests;

public sealed class BlipRenderCacheTests
{
    [Fact]
    public void FirstDefaultValueStillRequiresAWrite()
    {
        var cache = new AppliedValue<bool>();
        Assert.True(cache.NeedsUpdate(false));
    }

    [Fact]
    public void SuccessfullyAppliedValueSkipsRepeatedWrites()
    {
        var cache = new AppliedValue<string>();
        cache.Commit("clock");
        Assert.False(cache.NeedsUpdate("clock"));
        Assert.True(cache.NeedsUpdate("check"));
    }

    [Fact]
    public void FailedOrCulledWriteDoesNotAdvanceTheCache()
    {
        var cache = new AppliedValue<int>();
        cache.Commit(10);
        Assert.True(cache.NeedsUpdate(20));
        // No Commit: the setter failed or the slot was culled before application.
        Assert.True(cache.NeedsUpdate(20));
        Assert.False(cache.NeedsUpdate(10));
    }

    [Fact]
    public void PoolReuseAndReopeningCompareAgainstActualAppliedState()
    {
        var cache = new AppliedValue<bool>();
        cache.Commit(true); // portrait shown
        Assert.True(cache.NeedsUpdate(false)); // reused for icon-only marker
        cache.Commit(false);
        Assert.False(cache.NeedsUpdate(false)); // hide/reopen retains child state
        Assert.True(cache.NeedsUpdate(true)); // portrait returns
    }

    [Theory]
    [InlineData(15f, 15f, false)]
    [InlineData(15f, 15.000001f, true)]
    [InlineData(15f, 17f, true)]
    public void PulseDimensionsUseExactRatherThanApproximateEquality(float applied, float next, bool expected)
    {
        var cache = new AppliedValue<float>();
        cache.Commit(applied);
        Assert.Equal(expected, cache.NeedsUpdate(next));
    }

    // Same scalar tuple used by RenderBlips; no Unity stand-ins or native mocks.
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void ProjectionKeyObservesEverySnapshotAndCalibrationInput(int input)
    {
        var key = (1L, 12, (1f, 2f, 3f), (4f, 5f, 6f), 2048f, 2.5f);
        var cache = new AppliedValue<(long, int, (float, float, float), (float, float, float), float, float)>();
        cache.Commit(key);
        Assert.False(cache.NeedsUpdate(key));
        switch (input)
        {
            case 0: key.Item1++; break; // snapshot rebuild, even same count/order
            case 1: key.Item2++; break; // new converter native instance
            case 2: key.Item3.Item1++; break;
            case 3: key.Item3.Item2++; break;
            case 4: key.Item3.Item3++; break;
            case 5: key.Item4.Item1++; break;
            case 6: key.Item4.Item2++; break;
            case 7: key.Item4.Item3++; break;
            case 8: key.Item5++; break;
            case 9: key.Item6++; break;
        }
        Assert.True(cache.NeedsUpdate(key));
        cache.Commit(key);
        Assert.False(cache.NeedsUpdate(key));
    }

    [Fact]
    public void FallbackProjectionIsReplacedWhenConverterArrivesOrDisappears()
    {
        var cache = new AppliedValue<(long, int)>();
        cache.Commit((1L, 0));
        Assert.True(cache.NeedsUpdate((1L, 12)));
        cache.Commit((1L, 12));
        Assert.True(cache.NeedsUpdate((1L, 0)));
    }

    [Fact]
    public void UnchangedSnapshotProjectsEachSlotOnlyOnceThenRebuildRefreshesAllSlots()
    {
        var caches = new AppliedValue<(long, int)>[64];
        int projections = 0;
        for (int frame = 0; frame < 60; frame++)
        {
            var key = (frame < 30 ? 1L : 2L, 12);
            for (int slot = 0; slot < caches.Length; slot++)
            {
                if (!caches[slot].NeedsUpdate(key)) continue;
                projections++;
                caches[slot].Commit(key);
            }
        }
        Assert.Equal(128, projections); // policy workload count, NOT a Unity benchmark
    }

    [Fact]
    public void IndependentPropertiesDoNotInvalidateEachOther()
    {
        var sprite = new AppliedValue<int>();
        var color = new AppliedValue<string>();
        sprite.Commit(7);
        color.Commit("green");
        Assert.False(sprite.NeedsUpdate(7));
        Assert.True(color.NeedsUpdate("purple"));
    }
}

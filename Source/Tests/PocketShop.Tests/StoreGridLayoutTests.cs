using System;
using PocketShop.UI;
using Xunit;

namespace PocketShop.Tests;

/// <summary>
/// Behavior of the Unity-free directory-grid policy: a fixed 4-column rectangle, one stable tile
/// size for every catalog size, three visible rows when they fit, scrolling beyond that, and a
/// two-line store-name band. Arithmetic only - native rendering stays unverified here.
/// </summary>
public sealed class StoreGridLayoutTests
{
    // Viewport from Latest.log 2026-10-18; scale 1.2 is a sizing fixture, not a live measurement.
    // Dp(6) = 7.2; the fixture floor uses avatar 72, fonts 13/12, banner insets 3/3 and gap 1.2.
    private const float Viewport = 533.3f;
    private const float Pad = 7.2f;
    private const float Gap = 7.2f;
    private const float MinTile = 123.7f;
    private const int ShopsInTown = 14;

    private static float TileFor(int itemCount) => StoreGridLayout.TileHeight(Viewport, itemCount, MinTile, Pad, Gap);

    [Fact]
    public void EveryCatalogSizeUsesTheSameTileHeight()
    {
        float single = TileFor(1);
        float full = TileFor(ShopsInTown);
        float huge = TileFor(400);

        AssertClose(full, single, "one shop and the full directory must share one tile size");
        AssertClose(full, huge, "a long catalog must not shrink the tiles");
        AssertClose(TileFor(3), full, "a catalog within the sizing target uses the same tile size");
    }

    [Fact]
    public void SearchResultKeepsTheSameTileSize()
    {
        AssertClose(TileFor(ShopsInTown), TileFor(2), "a filtered result keeps the directory tile size");
    }

    [Fact]
    public void TileHeightFillsExactlyThreeVisibleRows()
    {
        float tile = TileFor(ShopsInTown);
        float threeRows = 3f * tile + 2f * Gap + 2f * Pad;

        AssertClose(Viewport, threeRows, "three sized rows plus gaps and padding must fill the viewport");
    }

    [Fact]
    public void CatalogLargerThanThreeRowsScrollsInsteadOfShrinkingTiles()
    {
        Assert.Equal(3, StoreGridLayout.SizingRows(ShopsInTown));
        Assert.Equal(4, StoreGridLayout.RowCount(ShopsInTown));

        float tile = TileFor(ShopsInTown);
        float content = StoreGridLayout.ContentHeight(ShopsInTown, tile, Pad, Gap, Viewport);

        Assert.True(content > Viewport, $"four rows of tiles must overflow the viewport: content={content}");
        Assert.True(tile >= MinTile, $"the tile stays at or above the minimum usable height: tile={tile}");
    }

    [Fact]
    public void CatalogWithinThreeRowsFillsTheViewportWithoutScrolling()
    {
        float tile = TileFor(3);
        float content = StoreGridLayout.ContentHeight(3, tile, Pad, Gap, Viewport);

        AssertClose(Viewport, content, "a short grid never scrolls, it fills the viewport");
    }

    [Fact]
    public void ShortViewportKeepsTheMinimumUsableTileHeight()
    {
        const float shortViewport = 300f;
        float tile = StoreGridLayout.TileHeight(shortViewport, ShopsInTown, MinTile, Pad, Gap);

        AssertClose(MinTile, tile, "a cramped viewport must not shrink the tiles below usability");
        float content = StoreGridLayout.ContentHeight(ShopsInTown, tile, Pad, Gap, shortViewport);
        Assert.True(content > shortViewport, $"the overflowing rows scroll: content={content}");
    }

    [Fact]
    public void GridIsARectangleWithTrailingPlaceholderCells()
    {
        Assert.Equal(0, StoreGridLayout.PlaceholderCount(0));
        Assert.Equal(3, StoreGridLayout.PlaceholderCount(1));
        Assert.Equal(3, StoreGridLayout.PlaceholderCount(5));
        Assert.Equal(2, StoreGridLayout.PlaceholderCount(ShopsInTown));
        Assert.Equal(0, StoreGridLayout.PlaceholderCount(4));
        Assert.Equal(0, StoreGridLayout.PlaceholderCount(8));

        Assert.Equal(0, StoreGridLayout.CellCount(0));
        Assert.Equal(8, StoreGridLayout.CellCount(5));
        Assert.Equal(16, StoreGridLayout.CellCount(ShopsInTown));

        foreach (int count in new[] { 1, 2, 3, 5, 7, 8, 9, ShopsInTown, 21 })
        {
            Assert.Equal(StoreGridLayout.RowCount(count) * StoreGridLayout.Columns, StoreGridLayout.CellCount(count));
            Assert.Equal(StoreGridLayout.CellCount(count) - count, StoreGridLayout.PlaceholderCount(count));
        }
    }

    [Fact]
    public void PlaceholderCellsAreOnlyTheTrailingCellsOfTheLastRow()
    {
        int cells = StoreGridLayout.CellCount(ShopsInTown);
        int lastRow = StoreGridLayout.RowCount(ShopsInTown) - 1;

        for (int i = 0; i < cells; i++)
        {
            bool placeholder = StoreGridLayout.IsPlaceholderCell(i, ShopsInTown);
            Assert.Equal(i >= ShopsInTown, placeholder);
            if (placeholder) Assert.Equal(lastRow, i / StoreGridLayout.Columns);
        }

        Assert.False(StoreGridLayout.IsPlaceholderCell(cells, ShopsInTown));
        Assert.False(StoreGridLayout.IsPlaceholderCell(-1, ShopsInTown));
        Assert.False(StoreGridLayout.IsPlaceholderCell(0, 0));
    }

    [Fact]
    public void DegenerateInputsFallBackToTheMinimumUsableTileHeight()
    {
        AssertClose(MinTile, StoreGridLayout.TileHeight(float.NaN, ShopsInTown, MinTile, Pad, Gap), "NaN viewport");
        AssertClose(MinTile, StoreGridLayout.TileHeight(0f, ShopsInTown, MinTile, Pad, Gap), "empty viewport");
        AssertClose(MinTile, StoreGridLayout.TileHeight(-5f, ShopsInTown, MinTile, Pad, Gap), "negative viewport");
        AssertClose(MinTile, StoreGridLayout.TileHeight(Viewport, 0, MinTile, Pad, Gap), "no entries");
        AssertClose(MinTile, StoreGridLayout.TileHeight(Viewport, -3, MinTile, Pad, Gap), "negative entries");
        AssertClose(0f, StoreGridLayout.ContentHeight(0, 0f, Pad, Gap, Viewport), "no entries has no grid");
    }

    [Fact]
    public void NonFiniteInsetsAreTreatedAsZero()
    {
        float tile = StoreGridLayout.TileHeight(500f, ShopsInTown, 100f, float.NaN, float.NaN);
        AssertClose(500f / 3f, tile, "unreadable padding and gaps must not poison the tile height");
    }

    [Fact]
    public void StoreNameBandFitsExactlyTwoLines()
    {
        Assert.Equal(2, StoreGridLayout.NameLineLimit);
        Assert.True(StoreGridLayout.NameBandHeight(13f) >= 13f * 2f * 1.2f,
            "two lines need font-metric headroom, not just twice the nominal font size");
        AssertClose(0f, StoreGridLayout.NameBandHeight(0f), "no font size");
        AssertClose(0f, StoreGridLayout.NameBandHeight(float.NaN), "unreadable font size");
    }

    [Fact]
    public void BannerHeightFitsTheTwoLineNameBandAndTheCountLine()
    {
        float banner = StoreGridLayout.BannerMinHeight(13f, 12f, 3f, 3f, 1.2f);

        AssertClose(51.7f, banner, "two-line name band + count line + banner insets");
        Assert.True(banner >= StoreGridLayout.NameBandHeight(13f) + 12f,
            "the banner must hold the full two-line name band plus the count line");
    }

    [Fact]
    public void MinimumUsableTileHeightIsTheAvatarBlockPlusItsBanner()
    {
        float banner = StoreGridLayout.BannerMinHeight(13f, 12f, 3f, 3f, 1.2f);

        AssertClose(MinTile, StoreGridLayout.MinTileHeight(72f, banner), "avatar block Dp(60) + banner");
    }

    private static void AssertClose(float expected, float actual, string because)
    {
        Assert.True(Math.Abs(expected - actual) < 0.01f, $"{because}: expected {expected}, actual {actual}");
    }
}

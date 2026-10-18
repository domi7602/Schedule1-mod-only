using System;

namespace PocketShop.UI;

/// <summary>
/// Pure, Unity-free layout policy for the store directory grid.
///
/// The grid is a rectangle: a fixed number of columns (<see cref="Columns"/>) and only full rows.
/// A catalog whose size is not a multiple of the column count gets trailing placeholder cells
/// (<see cref="PlaceholderCount"/>) which the pane renders as subtle non-interactive fillers, so the
/// last row keeps the rectangle shape instead of ending ragged.
///
/// The tile height is sized for a fixed number of visible rows (<see cref="TargetVisibleRows"/>),
/// never for the catalog size: the full directory, a search result and a one-shop catalog all use
/// the same tile size, and a catalog that needs more rows scrolls instead of shrinking the tiles.
/// </summary>
public static class StoreGridLayout
{
    /// <summary>Fixed column count of the directory grid.</summary>
    public const int Columns = 4;

    /// <summary>Rows the tile height is sized for, so three rows are visible without scrolling.</summary>
    public const int TargetVisibleRows = 3;

    /// <summary>Rendered line limit of the store-name band; longer names truncate, they are never rewritten.</summary>
    public const int NameLineLimit = 2;

    /// <summary>Grid rows needed for the given catalog size (0 when there is nothing to show).</summary>
    public static int RowCount(int itemCount)
    {
        if (itemCount <= 0) return 0;
        return (itemCount + Columns - 1) / Columns;
    }

    /// <summary>Cells of the complete rectangle: every row full, trailing cells are placeholders.</summary>
    public static int CellCount(int itemCount)
    {
        return RowCount(itemCount) * Columns;
    }

    /// <summary>Trailing non-interactive filler cells that complete the last row.</summary>
    public static int PlaceholderCount(int itemCount)
    {
        int items = itemCount > 0 ? itemCount : 0;
        return CellCount(itemCount) - items;
    }

    /// <summary>True for the padded cells that complete the last row; never true for a real item index.</summary>
    public static bool IsPlaceholderCell(int index, int itemCount)
    {
        return index >= 0 && index >= itemCount && index < CellCount(itemCount);
    }

    /// <summary>
    /// Rows the tile height is sized for. The grid keeps one tile size for every catalog size, so the
    /// visible row count never depends on how many entries are registered.
    /// </summary>
    public static int SizingRows(int itemCount)
    {
        return itemCount <= 0 ? 0 : TargetVisibleRows;
    }

    /// <summary>
    /// Tile height for the current viewport: the sizing rows fill the viewport exactly, floored at the
    /// minimum usable tile height. The item count only decides whether there is a grid at all.
    /// </summary>
    public static float TileHeight(float viewportHeight, int itemCount, float minTileHeight, float pad, float gap)
    {
        float floor = Sanitize(minTileHeight);
        int rows = SizingRows(itemCount);
        if (rows <= 0) return floor;
        if (!float.IsFinite(viewportHeight) || viewportHeight <= 0f) return floor;

        float spacing = Sanitize(gap) * (rows - 1);
        float available = viewportHeight - 2f * Sanitize(pad) - spacing;
        float target = available / rows;
        if (!float.IsFinite(target)) return floor;
        return target > floor ? target : floor;
    }

    /// <summary>
    /// Content height of the grid, never shorter than the viewport: more rows than the sizing target
    /// overflow into the scroller instead of compressing the tiles. An empty grid has no content
    /// height at all.
    /// </summary>
    public static float ContentHeight(int itemCount, float tileHeight, float pad, float gap, float viewportHeight)
    {
        float usableViewport = float.IsFinite(viewportHeight) && viewportHeight > 0f ? viewportHeight : 0f;
        int rows = RowCount(itemCount);
        if (rows <= 0) return 0f;

        float grid = 2f * Sanitize(pad) + rows * Sanitize(tileHeight) + Sanitize(gap) * (rows - 1);
        if (!float.IsFinite(grid)) return usableViewport;
        return grid > usableViewport ? grid : usableViewport;
    }

    /// <summary>Height for two name lines with 25% headroom for native font metrics.</summary>
    public static float NameBandHeight(float fontSize)
    {
        if (!float.IsFinite(fontSize) || fontSize <= 0f) return 0f;
        return fontSize * NameLineLimit * 1.25f;
    }

    /// <summary>
    /// Banner height that fits the two-line name band, the item-count line and the banner insets.
    /// The band is fixed, so a longer name wraps into the second line and truncates there.
    /// </summary>
    public static float BannerMinHeight(float nameFontSize, float countFontSize, float padTop, float padBottom, float spacing)
    {
        return NameBandHeight(nameFontSize) + Sanitize(countFontSize) + Sanitize(padTop) + Sanitize(padBottom) + Sanitize(spacing);
    }

    /// <summary>Minimum usable tile height: the avatar block plus its banner.</summary>
    public static float MinTileHeight(float avatarMinHeight, float bannerMinHeight)
    {
        return Sanitize(avatarMinHeight) + Sanitize(bannerMinHeight);
    }

    private static float Sanitize(float value)
    {
        return float.IsFinite(value) && value > 0f ? value : 0f;
    }
}

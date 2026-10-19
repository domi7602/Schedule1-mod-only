using System;
using System.IO;
using Xunit;

namespace PocketShop.Tests;

/// <summary>
/// Wiring contracts for the directory pane. These read the production source and do not execute
/// Unity: they pin the structural decisions that native layout cannot be verified from here - the
/// policy-owned tile sizing, the completed last row and the verbatim two-line store name.
/// </summary>
public sealed class StoreDirectoryLayoutWiringTests
{
    [Fact]
    public void TileSizingComesFromTheFixedTargetRowsAndNotFromTheCatalogSize()
    {
        string source = ReadSource("StoreCatalogPane.cs");

        string sizing = Section(source, "private void ApplyTileSizing()", "public void EnsureLayout()");
        Assert.Contains("StoreGridLayout.TileHeight(", sizing);
        Assert.Contains("StoreGridLayout.ContentHeight(", sizing);
        Assert.DoesNotContain("ShopCatalog.Shops.Count", sizing);
        Assert.DoesNotContain("totalRows", sizing);
        Assert.DoesNotContain("minRowHeight", sizing);

        string build = Section(source, "public void Build()", "private void ConfigureTileRect");
        Assert.DoesNotContain("ShopCatalog.Shops.Count", build);
    }

    [Fact]
    public void LastRowIsCompletedWithNonInteractivePlaceholderCells()
    {
        string source = ReadSource("StoreCatalogPane.cs");

        string build = Section(source, "public void Build()", "private void ConfigureTileRect");
        Assert.Contains("StoreGridLayout.CellCount(", build);
        Assert.Contains("BuildPlaceholderCell(", build);

        string placeholder = Section(source, "private RectTransform BuildPlaceholderCell", "private GameObject BuildStoreCard");
        Assert.Contains("raycastTarget = false", placeholder);
        Assert.DoesNotContain("AddComponent<Button>", placeholder);
        Assert.DoesNotContain("AddListener", placeholder);
    }

    [Fact]
    public void StoreNameIsRenderedVerbatimInATwoLineBand()
    {
        string card = Section(ReadSource("StoreCatalogPane.cs"), "private GameObject BuildStoreCard", "private static float TileGap");

        Assert.Contains("UIFactory.Text(\"ShopName\", shop.Name,", card);
        Assert.Contains("StoreGridLayout.NameBandHeight(", card);
        Assert.Contains("horizontalOverflow = HorizontalWrapMode.Wrap", card);
        Assert.Contains("verticalOverflow = VerticalWrapMode.Truncate", card);
        Assert.DoesNotContain("Substring", card);
        Assert.DoesNotContain("ToUpper", card);
        Assert.DoesNotContain("Normalize", card);
    }

    [Fact]
    public void StoreTileKeepsTheSharedPaletteAndSelectionPath()
    {
        string card = Section(ReadSource("StoreCatalogPane.cs"), "private GameObject BuildStoreCard", "private static float TileGap");

        Assert.Contains("ShopColorScheme.ColorFor(shop.ShopCode)", card);
        Assert.Contains("ButtonUtils.AddListener(btn, () => OnShopSelected?.Invoke(shop))", card);
        Assert.Contains("btn.interactable = false", card);
    }

    [Fact]
    public void TileRectIsResolvedWithoutADirectTransformCast()
    {
        string source = ReadSource("StoreCatalogPane.cs");

        Assert.DoesNotContain("(RectTransform)card.transform", source);
        Assert.Contains("GetComponent<RectTransform>()", source);
    }

    /// <summary>
    /// Resolves a mod source file by name from the src root, searching the domain
    /// subfolders (src/&lt;Domain&gt;/&lt;File&gt;.cs) as well as the root itself. Keeping the
    /// lookup layout-independent means a source reorganization does not invalidate
    /// every contract test that reads a file.
    /// </summary>
    private static string ReadSource(string name)
    {
        for (DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            string root = Path.Combine(dir.FullName, "Source", "Mods", "PocketShop", "src");
            if (!Directory.Exists(root)) continue;

            string direct = Path.Combine(root, name);
            if (File.Exists(direct)) return File.ReadAllText(direct);

            string[] matches = Directory.GetFiles(root, name, SearchOption.AllDirectories);
            if (matches.Length == 1) return File.ReadAllText(matches[0]);
            if (matches.Length > 1)
                throw new IOException($"Ambiguous PocketShop source name '{name}': {string.Join(", ", matches)}");
            throw new FileNotFoundException("PocketShop source file not found", name);
        }
        throw new FileNotFoundException("PocketShop source root not found", name);
    }

    private static string Section(string source, string start, string end)
    {
        int first = source.IndexOf(start, StringComparison.Ordinal);
        Assert.True(first >= 0, $"Missing section: {start}");
        int last = source.IndexOf(end, first + start.Length, StringComparison.Ordinal);
        Assert.True(last > first, $"Missing section end: {end}");
        return source.Substring(first, last - first);
    }
}

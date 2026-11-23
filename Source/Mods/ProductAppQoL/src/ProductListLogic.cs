using System;
using System.Collections.Generic;

namespace ProductAppQoL;

internal enum ProductSortMode
{
    Default = 0,
    Name = 1,
    Price = 2,
    MarketValue = 3,
    Addictiveness = 4,
    Listed = 5
}

internal readonly struct ProductSortItem
{
    public ProductSortItem(
        int originalIndex,
        string name,
        float price,
        float marketValue,
        float addictiveness,
        bool isListed,
        IReadOnlyList<string> effects)
    {
        OriginalIndex = originalIndex;
        Name = name ?? string.Empty;
        Price = price;
        MarketValue = marketValue;
        Addictiveness = addictiveness;
        IsListed = isListed;
        Effects = effects;
    }

    public int OriginalIndex { get; }
    public string Name { get; }
    public float Price { get; }
    public float MarketValue { get; }
    public float Addictiveness { get; }
    public bool IsListed { get; }
    public IReadOnlyList<string> Effects { get; }
}

/// <summary>
/// Pure, game-free search and sorting logic for the Product app list.
/// </summary>
internal static class ProductListLogic
{
    private static readonly ProductSortMode[] SortOrder =
    {
        ProductSortMode.Default,
        ProductSortMode.Price,
        ProductSortMode.Name,
        ProductSortMode.MarketValue,
        ProductSortMode.Addictiveness,
        ProductSortMode.Listed
    };

    public static ProductSortMode NextMode(ProductSortMode current)
    {
        int index = Array.IndexOf(SortOrder, current);
        if (index < 0) return ProductSortMode.Default;
        return SortOrder[(index + 1) % SortOrder.Length];
    }

    public static bool DefaultDescending(ProductSortMode mode) => mode switch
    {
        ProductSortMode.Price => true,
        ProductSortMode.MarketValue => true,
        ProductSortMode.Addictiveness => true,
        ProductSortMode.Listed => true,
        _ => false
    };

    public static string GetSortLabel(ProductSortMode mode) => mode switch
    {
        ProductSortMode.Default => "Default",
        ProductSortMode.Name => "Name",
        ProductSortMode.Price => "Price",
        ProductSortMode.MarketValue => "Value",
        ProductSortMode.Addictiveness => "Addict",
        ProductSortMode.Listed => "Listed",
        _ => "Default"
    };

    /// <summary>
    /// Returns true if all whitespace-separated query tokens match either the product name
    /// or at least one of its effect names (case-insensitive).
    /// </summary>
    public static bool MatchesSearch(in ProductSortItem item, string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return true;

        string[] tokens = query.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int t = 0; t < tokens.Length; t++)
        {
            string token = tokens[t];
            bool matched = item.Name.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;

            if (!matched && item.Effects != null)
            {
                for (int e = 0; e < item.Effects.Count; e++)
                {
                    string effect = item.Effects[e];
                    if (!string.IsNullOrEmpty(effect) &&
                        effect.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matched = true;
                        break;
                    }
                }
            }

            if (!matched)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Returns the item indices ordered by the selected sort mode and direction,
    /// using <see cref="ProductSortItem.OriginalIndex"/> as a deterministic tie-breaker.
    /// </summary>
    public static int[] SortIndices(IReadOnlyList<ProductSortItem> items, ProductSortMode mode, bool descending)
    {
        int count = items.Count;
        int[] indices = new int[count];
        for (int i = 0; i < count; i++)
            indices[i] = i;

        if (count <= 1)
            return indices;

        Array.Sort(indices, (a, b) => CompareItems(items[a], items[b], mode, descending));
        return indices;
    }

    private static int CompareItems(in ProductSortItem a, in ProductSortItem b, ProductSortMode mode, bool descending)
    {
        if (mode == ProductSortMode.Default)
            return a.OriginalIndex.CompareTo(b.OriginalIndex);

        int cmp = mode switch
        {
            ProductSortMode.Name => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase),
            ProductSortMode.Price => a.Price.CompareTo(b.Price),
            ProductSortMode.MarketValue => a.MarketValue.CompareTo(b.MarketValue),
            ProductSortMode.Addictiveness => a.Addictiveness.CompareTo(b.Addictiveness),
            ProductSortMode.Listed => a.IsListed.CompareTo(b.IsListed),
            _ => 0
        };

        if (cmp != 0)
            return descending ? -cmp : cmp;

        // Secondary sort by name for Listed ties, then OriginalIndex for stability.
        if (mode == ProductSortMode.Listed)
        {
            int nameCmp = string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
            if (nameCmp != 0)
                return nameCmp;
        }

        return a.OriginalIndex.CompareTo(b.OriginalIndex);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageScanner
{
    /// <summary>A single stack of items found in one container of one owned property.</summary>
    public sealed class StorageItem
    {
        public string PropertyId { get; }
        public string PropertyName { get; }
        public string ContainerId { get; }
        public string ContainerName { get; }
        public string ItemId { get; }
        public string ItemName { get; }
        public long Quantity { get; }

        /// <summary>Item category used by the category filter; never empty ('Other' when unknown).</summary>
        public string CategoryId { get; }

        /// <summary>True when this stack was replayed from the last-known cache of a culled property.</summary>
        public bool IsCached { get; }

        public StorageItem(string propertyId, string? propertyName, string containerId, string? containerName, string itemId, string? itemName, long quantity, bool isCached = false, string? categoryId = null)
        {
            if (string.IsNullOrWhiteSpace(propertyId))
            {
                throw new ArgumentException("Property ID is required.", nameof(propertyId));
            }
            if (string.IsNullOrWhiteSpace(containerId))
            {
                throw new ArgumentException("Container ID is required.", nameof(containerId));
            }
            if (string.IsNullOrWhiteSpace(itemId))
            {
                throw new ArgumentException("Item ID is required.", nameof(itemId));
            }
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }
            PropertyId = propertyId;
            PropertyName = string.IsNullOrEmpty(propertyName) ? propertyId : propertyName;
            ContainerId = containerId;
            ContainerName = string.IsNullOrEmpty(containerName) ? containerId : containerName;
            ItemId = itemId;
            ItemName = string.IsNullOrEmpty(itemName) ? itemId : itemName;
            Quantity = quantity;
            CategoryId = string.IsNullOrWhiteSpace(categoryId) ? "Other" : categoryId;
            IsCached = isCached;
        }

        /// <summary>Copy of this stack flagged as cached (the stored instance stays unmarked).</summary>
        public StorageItem AsCached() => new StorageItem(PropertyId, PropertyName, ContainerId, ContainerName, ItemId, ItemName, Quantity, true, CategoryId);
    }

    /// <summary>Aggregated total for one item across all filtered containers.</summary>
    public sealed class StockRow
    {
        public string ItemId { get; }
        public string ItemName { get; }
        public long Quantity { get; }
        public int ContainerCount { get; }

        /// <summary>Category of the grouped item (all stacks of an item share one category); never empty.</summary>
        public string CategoryId { get; }

        /// <summary>True when any contributing stack came from cached (culled property) data.</summary>
        public bool IncludesCachedData { get; }

        public StockRow(string itemId, string itemName, long quantity, int containerCount, bool includesCachedData = false, string? categoryId = null)
        {
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            ContainerCount = containerCount;
            IncludesCachedData = includesCachedData;
            CategoryId = string.IsNullOrWhiteSpace(categoryId) ? "Other" : categoryId;
        }
    }

    /// <summary>Immutable result of one scan pass; <see cref="Status"/> explains completeness.</summary>
    public sealed class StorageSnapshot
    {
        public DateTimeOffset CapturedAt { get; }
        public bool IsComplete { get; }
        public string Status { get; }
        public IReadOnlyList<StorageItem> Items { get; }

        /// <summary>Storage containers read live during this pass.</summary>
        public int ContainerCount { get; }

        /// <summary>Owned properties known to this pass.</summary>
        public int PropertyCount { get; }

        /// <summary>Properties that could be read live (not culled).</summary>
        public int LivePropertyCount { get; }

        /// <summary>Culled properties served from the last-known cache.</summary>
        public int CachedPropertyCount { get; }

        public StorageSnapshot(
            IEnumerable<StorageItem> items,
            bool isComplete,
            string? status,
            int containerCount = 0,
            int propertyCount = 0,
            int livePropertyCount = 0,
            int cachedPropertyCount = 0)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            Items = Array.AsReadOnly(items.ToArray());
            CapturedAt = DateTimeOffset.Now;
            IsComplete = isComplete;
            Status = status ?? string.Empty;
            ContainerCount = containerCount;
            PropertyCount = propertyCount;
            LivePropertyCount = livePropertyCount;
            CachedPropertyCount = cachedPropertyCount;
        }
    }

    /// <summary>Last-known item data of one property; used when the game culls its contents.</summary>
    public sealed class PropertyStorageCache
    {
        public string PropertyId { get; }
        public string PropertyName { get; }
        public IReadOnlyList<StorageItem> Items { get; }
        public DateTimeOffset UpdatedAt { get; }

        public PropertyStorageCache(string propertyId, string propertyName, IEnumerable<StorageItem> items, DateTimeOffset updatedAt)
        {
            if (string.IsNullOrEmpty(propertyId))
            {
                throw new ArgumentException("Property ID is required.", nameof(propertyId));
            }
            PropertyId = propertyId;
            PropertyName = string.IsNullOrEmpty(propertyName) ? propertyId : propertyName;
            Items = Array.AsReadOnly((items ?? throw new ArgumentNullException(nameof(items))).ToArray());
            UpdatedAt = updatedAt;
        }
    }

    /// <summary>Read-only source of storage data for the scanner UI.</summary>
    public interface IStorageSource
    {
        /// <summary>
        /// Non-throwing readiness check run before <see cref="Capture"/>. Refreshes the owned-property
        /// cache as a side effect (exactly one refresh per scan cycle); returns false while no owned
        /// property is loaded.
        /// </summary>
        bool IsReadyToScan();

        /// <summary>
        /// Reuses the property cache primed by <see cref="IsReadyToScan"/>() and reads every readable
        /// container once. Must return each physical stack once, using stable item and container IDs.
        /// If not all storage is readable, return IsComplete=false and explain why in
        /// <see cref="StorageSnapshot.Status"/>; stacks replayed from the culled-property cache are
        /// marked with <see cref="StorageItem.IsCached"/>.
        /// </summary>
        StorageSnapshot Capture();
    }

    /// <summary>Aggregated amount of one item in one container, for the item detail page.</summary>
    public sealed class ItemLocationRow
    {
        public string PropertyName { get; }
        public string ContainerName { get; }
        public long Quantity { get; }

        public ItemLocationRow(string propertyName, string containerName, long quantity)
        {
            PropertyName = propertyName;
            ContainerName = containerName;
            Quantity = quantity;
        }
    }

    /// <summary>Everything the detail page shows about one item.</summary>
    public sealed class ItemDetails
    {
        public string ItemId { get; }
        public string ItemName { get; }
        public long TotalQuantity { get; }
        public IReadOnlyList<ItemLocationRow> Locations { get; }

        public ItemDetails(string itemId, string itemName, long totalQuantity, IEnumerable<ItemLocationRow> locations)
        {
            ItemId = itemId;
            ItemName = itemName;
            TotalQuantity = totalQuantity;
            Locations = Array.AsReadOnly((locations ?? throw new ArgumentNullException(nameof(locations))).ToArray());
        }
    }

    /// <summary>Sort orders offered by the item list.</summary>
    public enum StockSortMode
    {
        NameAscending,
        NameDescending,
        QuantityDescending,
        QuantityAscending,
        ContainersDescending,
        ContainersAscending
    }

    /// <summary>Cycles sort modes in the order the sort button walks them.</summary>
    public static class StockSortModeExtensions
    {
        public static StockSortMode Next(this StockSortMode mode)
        {
            switch (mode)
            {
                case StockSortMode.NameAscending:
                    return StockSortMode.NameDescending;
                case StockSortMode.NameDescending:
                    return StockSortMode.QuantityDescending;
                case StockSortMode.QuantityDescending:
                    return StockSortMode.QuantityAscending;
                case StockSortMode.QuantityAscending:
                    return StockSortMode.ContainersDescending;
                case StockSortMode.ContainersDescending:
                    return StockSortMode.ContainersAscending;
                default:
                    return StockSortMode.NameAscending;
            }
        }
    }

    /// <summary>Pure aggregation: filter + group scan items into displayable rows.</summary>
    public static class StockAggregator
    {
        public static IReadOnlyList<StockRow> Summarize(StorageSnapshot snapshot, string? propertyId = null, string? search = null, StockSortMode sortMode = StockSortMode.NameAscending, string? categoryId = null)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }
            IEnumerable<StorageItem> items = snapshot.Items;
            if (propertyId != null)
            {
                items = items.Where(x => string.Equals(x.PropertyId, propertyId, StringComparison.Ordinal));
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                items = items.Where(x => x.ItemName.IndexOf(search!, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            if (categoryId != null)
            {
                items = items.Where(x => string.Equals(x.CategoryId, categoryId, StringComparison.Ordinal));
            }
            StockRow[] rows = items
                .GroupBy(x => x.ItemId, StringComparer.Ordinal)
                .Select(g => new StockRow(
                    g.Key,
                    g.First().ItemName,
                    g.Sum(x => x.Quantity),
                    g.Where(x => x.Quantity > 0)
                        .Select(x => new { x.PropertyId, x.ContainerId })
                        .Distinct()
                        .Count(),
                    g.Any(x => x.IsCached),
                    g.First().CategoryId))
                .ToArray();
            return SortRows(rows, sortMode);
        }

        /// <summary>True when two row lists would render identically (lets the UI skip rebuilds).</summary>
        public static bool RowsEquivalent(IReadOnlyList<StockRow>? left, IReadOnlyList<StockRow>? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (left == null || right == null || left.Count != right.Count)
            {
                return false;
            }
            for (int i = 0; i < left.Count; i++)
            {
                StockRow a = left[i];
                StockRow b = right[i];
                if (!string.Equals(a.ItemId, b.ItemId, StringComparison.Ordinal)
                    || !string.Equals(a.ItemName, b.ItemName, StringComparison.Ordinal)
                    || !string.Equals(a.CategoryId, b.CategoryId, StringComparison.Ordinal)
                    || a.Quantity != b.Quantity
                    || a.ContainerCount != b.ContainerCount
                    || a.IncludesCachedData != b.IncludesCachedData)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Detail view of one item: total plus a per-property/container breakdown; null when the item is not in the snapshot.</summary>
        public static ItemDetails? GetItemDetails(StorageSnapshot snapshot, string itemId)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }
            if (string.IsNullOrEmpty(itemId))
            {
                return null;
            }
            List<StorageItem> stacks = snapshot.Items
                .Where(x => string.Equals(x.ItemId, itemId, StringComparison.Ordinal))
                .ToList();
            if (stacks.Count == 0)
            {
                return null;
            }
            long total = stacks.Sum(x => x.Quantity);
            List<ItemLocationRow> locations = new List<ItemLocationRow>();
            var perContainer = stacks
                .GroupBy(x => new { x.PropertyId, x.ContainerId })
                .Select(g => new
                {
                    First = g.First(),
                    Quantity = g.Sum(x => x.Quantity)
                })
                .Where(x => x.Quantity > 0)
                .ToList();
            foreach (var property in perContainer
                .GroupBy(x => x.First.PropertyId, StringComparer.Ordinal)
                .Select(g => new
                {
                    Name = g.First().First.PropertyName,
                    Total = g.Sum(x => x.Quantity),
                    Containers = g.ToList()
                })
                .OrderByDescending(g => g.Total)
                .ThenBy(g => g.Name, StringComparer.OrdinalIgnoreCase))
            {
                foreach (var row in property.Containers
                    .OrderByDescending(x => x.Quantity)
                    .ThenBy(x => x.First.ContainerName, StringComparer.OrdinalIgnoreCase))
                {
                    locations.Add(new ItemLocationRow(property.Name, row.First.ContainerName, row.Quantity));
                }
            }
            return new ItemDetails(itemId, stacks[0].ItemName, total, locations);
        }

        private static IReadOnlyList<StockRow> SortRows(StockRow[] rows, StockSortMode sortMode)
        {
            IOrderedEnumerable<StockRow> ordered;
            switch (sortMode)
            {
                case StockSortMode.NameDescending:
                    ordered = rows.OrderByDescending(x => x.ItemName, StringComparer.OrdinalIgnoreCase);
                    break;
                case StockSortMode.QuantityDescending:
                    ordered = rows.OrderByDescending(x => x.Quantity);
                    break;
                case StockSortMode.QuantityAscending:
                    ordered = rows.OrderBy(x => x.Quantity);
                    break;
                case StockSortMode.ContainersDescending:
                    ordered = rows.OrderByDescending(x => x.ContainerCount);
                    break;
                case StockSortMode.ContainersAscending:
                    ordered = rows.OrderBy(x => x.ContainerCount);
                    break;
                default:
                    ordered = rows.OrderBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase);
                    break;
            }
            return ordered
                .ThenBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(x => x.ItemId, StringComparer.Ordinal)
                .ToArray();
        }
    }
}

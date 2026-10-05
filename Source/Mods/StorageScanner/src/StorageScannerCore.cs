using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageScanner
{
    public enum StockSortMode
    {
        NameAscending,
        NameDescending,
        QuantityDescending,
        QuantityAscending,
        ContainersDescending,
        ContainersAscending,
    }

    public sealed class StorageItem
    {
        public string PropertyId { get; }
        public string PropertyName { get; }
        public string ContainerId { get; }
        public string ContainerName { get; }
        public string ItemId { get; }
        public string ItemName { get; }
        public long Quantity { get; }
        public bool IsCached { get; }

        public StorageItem(string propertyId, string? propertyName, string containerId, string? containerName,
            string itemId, string? itemName, long quantity, bool isCached = false)
        {
            if (string.IsNullOrWhiteSpace(propertyId)) throw new ArgumentException("Property ID is required.", nameof(propertyId));
            if (string.IsNullOrWhiteSpace(containerId)) throw new ArgumentException("Container ID is required.", nameof(containerId));
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentException("Item ID is required.", nameof(itemId));
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            PropertyId = propertyId;
            PropertyName = string.IsNullOrEmpty(propertyName) ? propertyId : propertyName;
            ContainerId = containerId;
            ContainerName = string.IsNullOrEmpty(containerName) ? containerId : containerName;
            ItemId = itemId;
            ItemName = string.IsNullOrEmpty(itemName) ? itemId : itemName;
            Quantity = quantity;
            IsCached = isCached;
        }

        public StorageItem AsCached() => new StorageItem(
            PropertyId, PropertyName, ContainerId, ContainerName, ItemId, ItemName, Quantity, true);
    }

    public sealed class PropertyScanState
    {
        public string PropertyId { get; }
        public string PropertyName { get; }
        public bool IsLive { get; }
        public DateTimeOffset UpdatedAt { get; }
        public int ContainerCount { get; }

        public PropertyScanState(string propertyId, string? propertyName, bool isLive, DateTimeOffset updatedAt, int containerCount)
        {
            PropertyId = propertyId ?? throw new ArgumentNullException(nameof(propertyId));
            PropertyName = string.IsNullOrEmpty(propertyName) ? propertyId : propertyName;
            IsLive = isLive;
            UpdatedAt = updatedAt;
            ContainerCount = Math.Max(0, containerCount);
        }
    }

    public sealed class StockRow
    {
        public string ItemId { get; }
        public string ItemName { get; }
        public long Quantity { get; }
        public int ContainerCount { get; }
        public bool HasCachedData { get; }

        public StockRow(string itemId, string itemName, long quantity, int containerCount, bool hasCachedData)
        {
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            ContainerCount = containerCount;
            HasCachedData = hasCachedData;
        }
    }

    public sealed class ItemLocationRow
    {
        public string PropertyId { get; }
        public string PropertyName { get; }
        public string ContainerId { get; }
        public string ContainerName { get; }
        public long Quantity { get; }
        public bool IsCached { get; }

        public ItemLocationRow(string propertyId, string propertyName, string containerId, string containerName,
            long quantity, bool isCached)
        {
            PropertyId = propertyId;
            PropertyName = propertyName;
            ContainerId = containerId;
            ContainerName = containerName;
            Quantity = quantity;
            IsCached = isCached;
        }
    }

    public sealed class ItemDetails
    {
        public string ItemId { get; }
        public string ItemName { get; }
        public long TotalQuantity { get; }
        public IReadOnlyList<ItemLocationRow> Locations { get; }

        public ItemDetails(string itemId, string itemName, long totalQuantity, IReadOnlyList<ItemLocationRow> locations)
        {
            ItemId = itemId;
            ItemName = itemName;
            TotalQuantity = totalQuantity;
            Locations = locations;
        }
    }

    public sealed class StorageSnapshot
    {
        public DateTimeOffset CapturedAt { get; }
        public bool IsComplete { get; }
        public string Status { get; }
        public IReadOnlyList<StorageItem> Items { get; }
        public IReadOnlyList<PropertyScanState> Properties { get; }
        public int ContainerCount { get; }
        public long ScanDurationMilliseconds { get; }
        public int LivePropertyCount => Properties.Count(x => x.IsLive);
        public int CachedPropertyCount => Properties.Count(x => !x.IsLive);

        public StorageSnapshot(IEnumerable<StorageItem> items, IEnumerable<PropertyScanState> properties,
            bool isComplete, string? status, int containerCount, long scanDurationMilliseconds,
            DateTimeOffset? capturedAt = null)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            Items = Array.AsReadOnly(items.ToArray());
            Properties = Array.AsReadOnly(properties.ToArray());
            CapturedAt = capturedAt ?? DateTimeOffset.Now;
            IsComplete = isComplete;
            Status = status ?? string.Empty;
            ContainerCount = Math.Max(0, containerCount);
            ScanDurationMilliseconds = Math.Max(0, scanDurationMilliseconds);
        }
    }

    public interface IStorageSource
    {
        bool IsReadyToScan();
        StorageSnapshot Capture();
    }

    public sealed class PropertyStorageCache
    {
        private sealed class Entry
        {
            public StorageItem[] Items = Array.Empty<StorageItem>();
            public int ContainerCount;
            public DateTimeOffset UpdatedAt;
        }

        private readonly Dictionary<string, Entry> _entries = new Dictionary<string, Entry>(StringComparer.Ordinal);

        public void Clear() => _entries.Clear();

        public void Store(string propertyId, IEnumerable<StorageItem> items, int containerCount, DateTimeOffset updatedAt)
        {
            if (string.IsNullOrWhiteSpace(propertyId)) throw new ArgumentException("Property ID is required.", nameof(propertyId));
            if (items == null) throw new ArgumentNullException(nameof(items));

            _entries[propertyId] = new Entry
            {
                Items = items
                    .Where(x => string.Equals(x.PropertyId, propertyId, StringComparison.Ordinal))
                    .Select(x => new StorageItem(x.PropertyId, x.PropertyName, x.ContainerId, x.ContainerName,
                        x.ItemId, x.ItemName, x.Quantity))
                    .ToArray(),
                ContainerCount = Math.Max(0, containerCount),
                UpdatedAt = updatedAt,
            };
        }

        public bool TryGet(string propertyId, out IReadOnlyList<StorageItem> items,
            out int containerCount, out DateTimeOffset updatedAt)
        {
            if (_entries.TryGetValue(propertyId, out Entry? entry))
            {
                items = Array.AsReadOnly(entry.Items.Select(x => x.AsCached()).ToArray());
                containerCount = entry.ContainerCount;
                updatedAt = entry.UpdatedAt;
                return true;
            }

            items = Array.Empty<StorageItem>();
            containerCount = 0;
            updatedAt = default;
            return false;
        }
    }

    public static class StockAggregator
    {
        public static IReadOnlyList<StockRow> Summarize(StorageSnapshot snapshot, string? propertyId = null,
            string? search = null, StockSortMode sortMode = StockSortMode.NameAscending)
        {
            if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

            IEnumerable<StorageItem> items = snapshot.Items;
            if (propertyId != null)
                items = items.Where(x => string.Equals(x.PropertyId, propertyId, StringComparison.Ordinal));

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search!.Trim();
                items = items.Where(x =>
                    x.ItemName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.PropertyName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.ContainerName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            IEnumerable<StockRow> rows = items
                .GroupBy(x => x.ItemId, StringComparer.Ordinal)
                .Select(g => new StockRow(
                    g.Key,
                    g.First().ItemName,
                    g.Sum(x => x.Quantity),
                    g.Where(x => x.Quantity > 0)
                        .Select(x => x.PropertyId + "\u001f" + x.ContainerId)
                        .Distinct(StringComparer.Ordinal)
                        .Count(),
                    g.Any(x => x.IsCached)));

            return ApplySort(rows, sortMode).ToArray();
        }

        public static ItemDetails? GetItemDetails(StorageSnapshot snapshot, string itemId, string? propertyId = null)
        {
            if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));
            if (string.IsNullOrWhiteSpace(itemId)) return null;

            IEnumerable<StorageItem> matches = snapshot.Items
                .Where(x => string.Equals(x.ItemId, itemId, StringComparison.Ordinal));
            if (propertyId != null)
                matches = matches.Where(x => string.Equals(x.PropertyId, propertyId, StringComparison.Ordinal));

            StorageItem[] materialized = matches.ToArray();
            if (materialized.Length == 0) return null;

            ItemLocationRow[] locations = materialized
                .GroupBy(x => new { x.PropertyId, x.PropertyName, x.ContainerId, x.ContainerName })
                .Select(g => new ItemLocationRow(g.Key.PropertyId, g.Key.PropertyName, g.Key.ContainerId,
                    g.Key.ContainerName, g.Sum(x => x.Quantity), g.Any(x => x.IsCached)))
                .OrderBy(x => x.PropertyName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(x => x.ContainerName, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return new ItemDetails(materialized[0].ItemId, materialized[0].ItemName,
                materialized.Sum(x => x.Quantity), locations);
        }

        public static bool RowsEqual(IReadOnlyList<StockRow>? left, IReadOnlyList<StockRow>? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left == null || right == null || left.Count != right.Count) return false;

            for (int i = 0; i < left.Count; i++)
            {
                StockRow a = left[i];
                StockRow b = right[i];
                if (!string.Equals(a.ItemId, b.ItemId, StringComparison.Ordinal) ||
                    !string.Equals(a.ItemName, b.ItemName, StringComparison.Ordinal) ||
                    a.Quantity != b.Quantity || a.ContainerCount != b.ContainerCount ||
                    a.HasCachedData != b.HasCachedData)
                    return false;
            }

            return true;
        }

        private static IOrderedEnumerable<StockRow> ApplySort(IEnumerable<StockRow> rows, StockSortMode mode)
        {
            return mode switch
            {
                StockSortMode.NameDescending => rows.OrderByDescending(x => x.ItemName, StringComparer.OrdinalIgnoreCase),
                StockSortMode.QuantityDescending => rows.OrderByDescending(x => x.Quantity).ThenBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase),
                StockSortMode.QuantityAscending => rows.OrderBy(x => x.Quantity).ThenBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase),
                StockSortMode.ContainersDescending => rows.OrderByDescending(x => x.ContainerCount).ThenBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase),
                StockSortMode.ContainersAscending => rows.OrderBy(x => x.ContainerCount).ThenBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase),
                _ => rows.OrderBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase),
            };
        }
    }
}

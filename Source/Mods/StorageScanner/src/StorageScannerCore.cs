using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageScanner
{
    public sealed class StorageItem
    {
        public string PropertyId { get; }
        public string PropertyName { get; }
        public string ContainerId { get; }
        public string ContainerName { get; }
        public string ItemId { get; }
        public string ItemName { get; }
        public long Quantity { get; }

        public StorageItem(string propertyId, string propertyName, string containerId,
            string containerName, string itemId, string itemName, long quantity)
        {
            if (string.IsNullOrWhiteSpace(propertyId)) throw new ArgumentException("Property ID is required.", nameof(propertyId));
            if (string.IsNullOrWhiteSpace(containerId)) throw new ArgumentException("Container ID is required.", nameof(containerId));
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentException("Item ID is required.", nameof(itemId));
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            PropertyId = propertyId;
            PropertyName = propertyName ?? propertyId;
            ContainerId = containerId;
            ContainerName = containerName ?? containerId;
            ItemId = itemId;
            ItemName = itemName ?? itemId;
            Quantity = quantity;
        }
    }

    public sealed class StockRow
    {
        public string ItemId { get; }
        public string ItemName { get; }
        public long Quantity { get; }
        public int ContainerCount { get; }

        public StockRow(string itemId, string itemName, long quantity, int containerCount)
        {
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            ContainerCount = containerCount;
        }
    }

    public sealed class StorageSnapshot
    {
        public DateTimeOffset CapturedAt { get; }
        public bool IsComplete { get; }
        public string Status { get; }
        public IReadOnlyList<StorageItem> Items { get; }

        public StorageSnapshot(IEnumerable<StorageItem> items, bool isComplete, string status)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            Items = Array.AsReadOnly(items.ToArray());
            CapturedAt = DateTimeOffset.UtcNow;
            IsComplete = isComplete;
            Status = status ?? string.Empty;
        }
    }

    public interface IStorageSource
    {
        // Must return each physical stack once, using stable item and container IDs.
        // If not all shelves are readable, return IsComplete=false and explain why.
        StorageSnapshot Capture();
    }

    public static class StockAggregator
    {
        public static IReadOnlyList<StockRow> Summarize(StorageSnapshot snapshot,
            string propertyId = null, string search = null)
        {
            if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));
            IEnumerable<StorageItem> items = snapshot.Items;
            if (propertyId != null)
                items = items.Where(x => string.Equals(x.PropertyId, propertyId, StringComparison.Ordinal));
            if (!string.IsNullOrWhiteSpace(search))
                items = items.Where(x => x.ItemName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

            return items.GroupBy(x => x.ItemId, StringComparer.Ordinal)
                .Select(g => new StockRow(g.Key, g.First().ItemName,
                    g.Sum(x => x.Quantity),
                    g.Where(x => x.Quantity > 0)
                        .Select(x => Tuple.Create(x.PropertyId, x.ContainerId)).Distinct().Count()))
                .OrderBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
    }
}

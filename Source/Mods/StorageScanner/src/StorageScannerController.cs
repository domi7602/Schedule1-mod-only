using System;
using System.Collections.Generic;

namespace StorageScanner
{
    /// <summary>Which page the scanner UI currently shows.</summary>
    public enum StorageScannerPage
    {
        Inventory,
        ItemDetails
    }

    public enum StorageScanState { Waiting, Ready, Failed }

    /// <summary>Everything the view needs for a single render pass.</summary>
    public sealed class StorageScannerViewState
    {
        public StorageSnapshot Snapshot { get; }
        public IReadOnlyList<StockRow> Rows { get; }
        public string? SelectedPropertyId { get; }

        /// <summary>Selected category filter; null shows all categories.</summary>
        public string? SelectedCategoryId { get; }

        public string Search { get; }
        public StorageScanState ScanState { get; }
        public bool HasScanned { get; }
        public string? ErrorMessage => ScanState == StorageScanState.Failed
            ? "Scan failed. Displayed stock may be outdated." : null;

        public StockSortMode SortMode { get; }
        public StorageScannerPage Page { get; }

        /// <summary>Non-null only while <see cref="Page"/> is <see cref="StorageScannerPage.ItemDetails"/>.</summary>
        public ItemDetails? ItemDetails { get; }

        public StorageScannerViewState(StorageSnapshot snapshot, IReadOnlyList<StockRow> rows, string? selectedPropertyId, StockSortMode sortMode, StorageScannerPage page, ItemDetails? itemDetails, string? selectedCategoryId = null, string? search = null, StorageScanState scanState = StorageScanState.Ready, bool hasScanned = true)
        {
            Snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
            SelectedPropertyId = selectedPropertyId;
            SelectedCategoryId = selectedCategoryId;
            Search = search ?? string.Empty;
            ScanState = scanState;
            HasScanned = hasScanned;
            SortMode = sortMode;
            Page = page;
            ItemDetails = itemDetails;
        }
    }

    /// <summary>UI surface the controller renders into (portrait layout: property selector, search, sort, item rows; item detail page; always shows completeness and capture time).</summary>
    public interface IStorageScannerView
    {
        void Render(StorageScannerViewState state);

    }

    /// <summary>Drives scan + filtering and pushes results to the view.</summary>
    public sealed class StorageScannerController
    {
        private readonly IStorageSource source;
        private readonly IStorageScannerView view;
        private readonly Action<Exception>? onError;
        private StorageSnapshot? snapshot;
        private StorageScanState scanState = StorageScanState.Waiting;
        private string? propertyId;
        private string? categoryId;
        private string? search;
        private StockSortMode sortMode = StockSortMode.NameAscending;
        private StorageScannerPage page = StorageScannerPage.Inventory;
        private string? selectedItemId;

        public StorageScannerController(IStorageSource source, IStorageScannerView view, Action<Exception>? onError = null)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.onError = onError;
        }

        public void Open()
        {
            page = StorageScannerPage.Inventory;
            selectedItemId = null;
            Refresh();
        }

        public void Refresh()
        {
            try
            {
                if (!source.IsReadyToScan())
                {
                    if (scanState != StorageScanState.Failed) scanState = StorageScanState.Waiting;
                    Render();
                    return;
                }
                snapshot = source.Capture() ?? throw new InvalidOperationException("Scanner returned no snapshot.");
                scanState = StorageScanState.Ready;
                Render();
            }
            catch (Exception ex)
            {
                // Never replace a failed scan with a misleading empty inventory, and never
                // leak exception internals into the player-facing message.
                onError?.Invoke(ex);
                scanState = StorageScanState.Failed;
                Render();
            }
        }

        public void SelectProperty(string? id)
        {
            propertyId = id; // null selects all properties.
            Render();
        }

        public void SetSearch(string? value)
        {
            search = value;
            Render();
        }

        public void SelectCategory(string? id)
        {
            categoryId = id; // null selects all categories.
            Render();
        }

        /// <summary>Clears property, category and search filters and re-renders the current snapshot (does not scan).</summary>
        public void ResetFilters()
        {
            propertyId = null;
            categoryId = null;
            search = null;
            Render();
        }

        public void SetSortMode(StockSortMode mode)
        {
            sortMode = mode;
            Render();
        }

        public void SelectItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return;
            }
            selectedItemId = itemId;
            page = StorageScannerPage.ItemDetails;
            Render();
        }

        public void CloseItemDetails()
        {
            selectedItemId = null;
            page = StorageScannerPage.Inventory;
            Render();
        }

        public void ResetForNewSave()
        {
            snapshot = null;
            scanState = StorageScanState.Waiting;
            propertyId = null;
            categoryId = null;
            search = null;
            sortMode = StockSortMode.NameAscending;
            page = StorageScannerPage.Inventory;
            selectedItemId = null;
            Render();
        }

        private void Render()
        {
            StorageSnapshot displayed = snapshot ?? new StorageSnapshot(Array.Empty<StorageItem>(), false, string.Empty);
            IReadOnlyList<StockRow> rows = StockAggregator.Summarize(displayed, propertyId, search, sortMode, categoryId);
            ItemDetails? details = null;
            if (page == StorageScannerPage.ItemDetails && selectedItemId != null)
            {
                details = StockAggregator.GetItemDetails(displayed, selectedItemId);
                if (details == null)
                {
                    // The item vanished from storage while its detail page was open; fall back to the list.
                    selectedItemId = null;
                    page = StorageScannerPage.Inventory;
                }
            }
            view.Render(new StorageScannerViewState(displayed, rows, propertyId, sortMode, page, details, categoryId, search, scanState, snapshot != null));
        }
    }
}

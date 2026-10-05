using System;
using System.Collections.Generic;

namespace StorageScanner
{
    public interface IStorageScannerView
    {
        void RenderInventory(StorageSnapshot snapshot, IReadOnlyList<StockRow> rows,
            string? selectedPropertyId, StockSortMode sortMode);
        void RenderItemDetails(StorageSnapshot snapshot, ItemDetails details, string? selectedPropertyId);
        void ShowError(string message);
    }

    public sealed class StorageScannerController
    {
        private readonly IStorageSource source;
        private readonly IStorageScannerView view;
        private StorageSnapshot? snapshot;
        private string? propertyId;
        private string? search;
        private string? selectedItemId;
        private StockSortMode sortMode = StockSortMode.NameAscending;

        public StorageScannerController(IStorageSource source, IStorageScannerView view)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Open() => Refresh();

        public void Refresh()
        {
            try
            {
                if (!source.IsReadyToScan())
                {
                    view.ShowError("Loading storage...");
                    return;
                }

                snapshot = source.Capture() ?? throw new InvalidOperationException("Scanner returned no snapshot.");
                Render();
            }
            catch (Exception ex)
            {
                view.ShowError("Scan failed; displayed data may be outdated: " + ex.Message);
            }
        }

        public void SelectProperty(string? id)
        {
            propertyId = id;
            selectedItemId = null;
            Render();
        }

        public void SetSearch(string? value)
        {
            search = value;
            selectedItemId = null;
            Render();
        }

        public void CycleSortMode()
        {
            sortMode = sortMode switch
            {
                StockSortMode.NameAscending => StockSortMode.NameDescending,
                StockSortMode.NameDescending => StockSortMode.QuantityDescending,
                StockSortMode.QuantityDescending => StockSortMode.QuantityAscending,
                StockSortMode.QuantityAscending => StockSortMode.ContainersDescending,
                StockSortMode.ContainersDescending => StockSortMode.ContainersAscending,
                _ => StockSortMode.NameAscending,
            };
            Render();
        }

        public void SelectItem(string itemId)
        {
            selectedItemId = itemId;
            Render();
        }

        public void CloseItemDetails()
        {
            selectedItemId = null;
            Render();
        }

        public void ResetForNewSave()
        {
            snapshot = null;
            propertyId = null;
            search = null;
            selectedItemId = null;
            sortMode = StockSortMode.NameAscending;
        }

        private void Render()
        {
            if (snapshot == null) return;

            if (selectedItemId != null)
            {
                ItemDetails? details = StockAggregator.GetItemDetails(snapshot, selectedItemId, propertyId);
                if (details != null)
                {
                    view.RenderItemDetails(snapshot, details, propertyId);
                    return;
                }
                selectedItemId = null;
            }

            view.RenderInventory(snapshot,
                StockAggregator.Summarize(snapshot, propertyId, search, sortMode),
                propertyId,
                sortMode);
        }
    }
}

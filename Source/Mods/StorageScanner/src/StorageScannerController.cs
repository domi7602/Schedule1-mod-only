using System;
using System.Collections.Generic;

namespace StorageScanner
{
    public interface IStorageScannerView
    {
        // Portrait layout: property selector, search, item rows, refresh button.
        // Always show snapshot completeness and capture time.
        void Render(StorageSnapshot snapshot, IReadOnlyList<StockRow> rows, string selectedPropertyId);
        void ShowError(string message);
    }

    public sealed class StorageScannerController
    {
        private readonly IStorageSource source;
        private readonly IStorageScannerView view;
        private StorageSnapshot snapshot;
        private string propertyId;
        private string search;

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
                snapshot = source.Capture() ?? throw new InvalidOperationException("Scanner returned no snapshot.");
                Render();
            }
            catch (Exception ex)
            {
                // Never replace a failed scan with a misleading empty inventory.
                view.ShowError("Scan failed; displayed data may be outdated: " + ex.Message);
            }
        }

        public void SelectProperty(string id)
        {
            propertyId = id; // null selects all properties.
            Render();
        }

        public void SetSearch(string value)
        {
            search = value;
            Render();
        }

        public void ResetForNewSave()
        {
            snapshot = null;
            propertyId = null;
            search = null;
        }

        private void Render()
        {
            if (snapshot == null) return;
            view.Render(snapshot, StockAggregator.Summarize(snapshot, propertyId, search), propertyId);
        }
    }
}

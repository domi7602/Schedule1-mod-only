using System;
using System.Linq;
using StorageScanner;
using Xunit;

namespace StorageScanner.Tests;

/// <summary>Deterministic IStorageSource: counts calls, optional capture failure, fixed snapshot.</summary>
internal sealed class FakeSource : IStorageSource
{
    private readonly StorageSnapshot _snapshot;

    public FakeSource(StorageSnapshot snapshot) => _snapshot = snapshot;

    public bool Ready = true;
    public Exception? ThrowOnCapture;
    public int ReadyCalls;
    public int CaptureCalls;

    public bool IsReadyToScan()
    {
        ReadyCalls++;
        return Ready;
    }

    public StorageSnapshot Capture()
    {
        CaptureCalls++;
        if (ThrowOnCapture != null)
        {
            throw ThrowOnCapture;
        }
        return _snapshot;
    }
}

/// <summary>Records the last rendered state and the last error message.</summary>
internal sealed class FakeView : IStorageScannerView
{
    public StorageScannerViewState? Last;
    public string? LastError;
    public int RenderCalls;

    public void Render(StorageScannerViewState state)
    {
        RenderCalls++;
        Last = state;
        LastError = state.ErrorMessage;
    }

    public void ShowError(string message) => LastError = message;
}

public class StorageScannerControllerTests
{
    private static StorageItem Item(string id, string name, long quantity, string category, string property = "p1")
        => new StorageItem(property, "Prop", "c-" + id + "-" + property, "Chest", id, name, quantity, false, category);

    private static StorageSnapshot Snapshot(params StorageItem[] items)
        => new StorageSnapshot(items, true, "test");

    [Fact]
    public void SelectCategory_FiltersRows_AndExposesSelection()
    {
        var snapshot = Snapshot(Item("i1", "Rifle", 5, "Weapons"), Item("i2", "Bread", 9, "Food"));
        var view = new FakeView();
        var controller = new StorageScannerController(new FakeSource(snapshot), view);
        controller.Refresh();

        controller.SelectCategory("Weapons");

        Assert.Equal("Weapons", view.Last!.SelectedCategoryId);
        Assert.Single(view.Last.Rows);
        Assert.Equal("i1", view.Last.Rows[0].ItemId);
    }

    [Fact]
    public void SelectCategory_Null_RestoresAllCategories()
    {
        var snapshot = Snapshot(Item("i1", "Rifle", 5, "Weapons"), Item("i2", "Bread", 9, "Food"));
        var view = new FakeView();
        var controller = new StorageScannerController(new FakeSource(snapshot), view);
        controller.Refresh();
        controller.SelectCategory("Weapons");

        controller.SelectCategory(null);

        Assert.Null(view.Last!.SelectedCategoryId);
        Assert.Equal(2, view.Last.Rows.Count);
    }

    [Fact]
    public void SelectCategory_CombinesWithPropertyAndSearch()
    {
        var snapshot = Snapshot(
            Item("i1", "Rifle", 5, "Weapons", "p1"),
            Item("i2", "Rifle Ammo", 10, "Weapons", "p2"),
            Item("i3", "Bread", 9, "Food", "p1"));
        var view = new FakeView();
        var controller = new StorageScannerController(new FakeSource(snapshot), view);
        controller.Refresh();
        controller.SelectProperty("p1");
        controller.SelectCategory("Weapons");

        controller.SetSearch("rifle");

        Assert.Single(view.Last!.Rows);
        Assert.Equal("i1", view.Last.Rows[0].ItemId);
        Assert.Equal("Weapons", view.Last.SelectedCategoryId);
        Assert.Equal("p1", view.Last.SelectedPropertyId);
        Assert.Equal("rifle", view.Last.Search);
    }

    [Fact]
    public void ResetForNewSave_ClearsSelectedCategory()
    {
        var snapshot = Snapshot(Item("i1", "Rifle", 5, "Weapons"), Item("i2", "Bread", 9, "Food"));
        var view = new FakeView();
        var controller = new StorageScannerController(new FakeSource(snapshot), view);
        controller.Refresh();
        controller.SelectCategory("Weapons");

        controller.ResetForNewSave();
        controller.Refresh();

        Assert.Null(view.Last!.SelectedCategoryId);
        Assert.Equal(2, view.Last.Rows.Count);
    }

    [Fact]
    public void ResetFilters_ClearsPropertyCategoryAndSearch_WithoutScanning()
    {
        var snapshot = Snapshot(
            Item("i1", "Rifle", 5, "Weapons", "p1"),
            Item("i2", "Bread", 9, "Food", "p2"));
        var source = new FakeSource(snapshot);
        var view = new FakeView();
        var controller = new StorageScannerController(source, view);
        controller.Refresh();
        controller.SelectProperty("p1");
        controller.SelectCategory("Weapons");
        controller.SetSearch("rifle");
        int capturesBefore = source.CaptureCalls;
        int rendersBefore = view.RenderCalls;

        controller.ResetFilters();

        Assert.Equal(capturesBefore, source.CaptureCalls); // no extra scan
        Assert.Equal(rendersBefore + 1, view.RenderCalls); // still rendered once with cleared filters
        Assert.Null(view.Last!.SelectedPropertyId);
        Assert.Null(view.Last.SelectedCategoryId);
        Assert.Equal(string.Empty, view.Last.Search);
        Assert.Equal(2, view.Last.Rows.Count);
    }

    [Fact]
    public void Refresh_PreservesFiltersAndSelectedCategory()
    {
        var snapshot = Snapshot(
            Item("i1", "Rifle", 5, "Weapons", "p1"),
            Item("i2", "Bread", 9, "Food", "p2"));
        var source = new FakeSource(snapshot);
        var view = new FakeView();
        var controller = new StorageScannerController(source, view);
        controller.Refresh();
        controller.SelectProperty("p1");
        controller.SelectCategory("Weapons");
        controller.SetSearch("rifle");
        int capturesBefore = source.CaptureCalls;

        controller.Refresh();

        Assert.Equal(capturesBefore + 1, source.CaptureCalls);
        Assert.Equal("p1", view.Last!.SelectedPropertyId);
        Assert.Equal("Weapons", view.Last.SelectedCategoryId);
        Assert.Equal("rifle", view.Last.Search);
        Assert.Single(view.Last.Rows);
        Assert.Equal("i1", view.Last.Rows[0].ItemId);
    }

    [Fact]
    public void ScanError_InvokesCallback_AndShowsGenericMessage()
    {
        var source = new FakeSource(Snapshot()) { ThrowOnCapture = new InvalidOperationException("secret detail 42") };
        var view = new FakeView();
        Exception? reported = null;
        var controller = new StorageScannerController(source, view, ex => reported = ex);

        controller.Refresh();

        Assert.IsType<InvalidOperationException>(reported);
        Assert.Equal("secret detail 42", reported!.Message);
        Assert.Equal("Scan failed. Displayed stock may be outdated.", view.LastError);
        Assert.DoesNotContain("secret", view.LastError);
    }

    [Fact]
    public void ScanError_WithoutCallback_DoesNotThrow_AndShowsGenericMessage()
    {
        var source = new FakeSource(Snapshot()) { ThrowOnCapture = new Exception("boom") };
        var view = new FakeView();
        var controller = new StorageScannerController(source, view);

        controller.Refresh();

        Assert.Equal("Scan failed. Displayed stock may be outdated.", view.LastError);
    }

    [Theory]
    [InlineData("search")]
    [InlineData("category")]
    [InlineData("property")]
    [InlineData("sort")]
    [InlineData("reset")]
    [InlineData("detail")]
    [InlineData("back")]
    [InlineData("not-ready")]
    public void FailedScan_RemainsVisibleUntilSuccessfulCapture(string action)
    {
        var source = new FakeSource(Snapshot(Item("i1", "Rifle", 5, "Weapons")));
        var view = new FakeView();
        var controller = new StorageScannerController(source, view);
        controller.Refresh();
        source.ThrowOnCapture = new Exception("private failure");
        controller.Refresh();
        switch (action)
        {
            case "search": controller.SetSearch("rifle"); break;
            case "category": controller.SelectCategory("Weapons"); break;
            case "property": controller.SelectProperty("p1"); break;
            case "sort": controller.SetSortMode(StockSortMode.QuantityDescending); break;
            case "reset": controller.ResetFilters(); break;
            case "detail": controller.SelectItem("i1"); break;
            case "back": controller.CloseItemDetails(); break;
            case "not-ready": source.Ready = false; controller.Refresh(); break;
        }
        Assert.Equal(StorageScanState.Failed, view.Last!.ScanState);
        Assert.True(view.Last.HasScanned);
        Assert.Single(view.Last.Snapshot.Items);
        source.Ready = true;
        source.ThrowOnCapture = null;
        controller.Refresh();
        Assert.Equal(StorageScanState.Ready, view.Last.ScanState);
    }

    [Fact]
    public void NewSaveReset_RendersWaitingWithoutPreviousStockOrError()
    {
        var source = new FakeSource(Snapshot(Item("i1", "Rifle", 5, "Weapons")));
        var view = new FakeView();
        var controller = new StorageScannerController(source, view);
        controller.Refresh();
        controller.SetSearch("rifle");
        source.ThrowOnCapture = new Exception("old save error");
        controller.Refresh();
        controller.ResetForNewSave();
        Assert.Equal(StorageScanState.Waiting, view.Last!.ScanState);
        Assert.False(view.Last.HasScanned);
        Assert.Empty(view.Last.Snapshot.Items);
        Assert.Empty(view.Last.Search);
        Assert.Null(view.Last.ErrorMessage);
    }

    [Fact]
    public void NotReady_ShowsLoading_AndDoesNotReportError()
    {
        var source = new FakeSource(Snapshot()) { Ready = false };
        var view = new FakeView();
        bool callbackInvoked = false;
        var controller = new StorageScannerController(source, view, _ => callbackInvoked = true);

        controller.Refresh();

        Assert.False(callbackInvoked);
        Assert.Null(view.LastError);
        Assert.NotNull(view.Last);
        Assert.Equal(StorageScanState.Waiting, view.Last.ScanState);
        Assert.False(view.Last.HasScanned);
        Assert.Empty(view.Last.Rows);
        Assert.Equal(0, source.CaptureCalls);
    }
}

using System;
using Minimap;
using Xunit;

namespace Minimap.Tests;

public sealed class WaypointStoreReviewTests
{
    [Fact]
    public void ReturningToSlotUsesItsNewerPendingSnapshotNotStaleDisk()
    {
        var io = new FakeWaypointIo();
        string? slot = "slot_1";
        io.Files[WaypointTestData.Slot1Path] = WaypointTestData.Serialize(("Old", 1, 2, 3, "#FFFFFF"));
        var store = WaypointTestData.Store(io, () => slot);
        store.Load();
        store.Add("Pending", 4, 5, 6, "#FFFFFF");
        io.FailWrite = _ => true;
        slot = "slot_2";
        store.Load();
        slot = "slot_1";
        store.Load();
        Assert.Equal(2, store.Count);
        Assert.Contains(store.Rows, r => r.Name == "Pending");
        store.Add("Newest", 7, 8, 9, "#FFFFFF");
        io.FailWrite = null;
        Assert.True(store.Save());
        Assert.Contains("Pending", io.Files[WaypointTestData.Slot1Path]);
        Assert.Contains("Newest", io.Files[WaypointTestData.Slot1Path]);
    }

    [Fact]
    public void AddResolvesAndLoadsTheNewSlotBeforeMutating()
    {
        var io = new FakeWaypointIo();
        string? slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);
        store.Load();
        store.Add("A", 1, 2, 3, "#FFFFFF");
        slot = "slot_2";
        io.Files[WaypointTestData.Slot2Path] = WaypointTestData.Serialize(("B", 4, 5, 6, "#FFFFFF"));
        Assert.True(store.Add("NewB", 7, 8, 9, "#FFFFFF"));
        Assert.True(store.Save());
        Assert.DoesNotContain("NewB", io.Files[WaypointTestData.Slot1Path]);
        Assert.Contains("B", io.Files[WaypointTestData.Slot2Path]);
        Assert.Contains("NewB", io.Files[WaypointTestData.Slot2Path]);
        Assert.DoesNotContain("\"A\"", io.Files[WaypointTestData.Slot2Path]);
    }

    [Fact]
    public void ClearAfterContextChangeCannotClearTheOldSlot()
    {
        var io = new FakeWaypointIo();
        string? slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);
        store.Load();
        store.Add("A", 1, 2, 3, "#FFFFFF");
        slot = "slot_2";
        store.Clear();
        store.Save();
        Assert.Contains("A", io.Files[WaypointTestData.Slot1Path]);
        Assert.Empty(MinimapJson.DeserializeWaypoints(io.Files[WaypointTestData.Slot2Path])!);
    }

    [Fact]
    public void UnknownContextRejectsEditsAndHidesPreviousRows()
    {
        var io = new FakeWaypointIo();
        string? slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);
        store.Load();
        store.Add("A", 1, 2, 3, "#FFFFFF");
        slot = null;
        Assert.False(store.Add("Unknown", 4, 5, 6, "#FFFFFF"));
        Assert.False(store.Save());
        Assert.Empty(store.Rows);
        Assert.DoesNotContain("Unknown", io.Files[WaypointTestData.Slot1Path]);
    }

    [Fact]
    public void RecoveredBackupIsReadableButCannotBeRotatedOverByCorruptPrimary()
    {
        var io = new FakeWaypointIo();
        string p = WaypointTestData.Slot1Path;
        io.Files[p] = "broken";
        io.Files[p + ".bak"] = WaypointTestData.Serialize(("Recovered", 1, 2, 3, "#FFFFFF"));
        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();
        Assert.Single(store.Rows);
        Assert.True(store.IsFaulted);
        Assert.False(store.Add("No", 1, 2, 3, "#FFFFFF"));
        Assert.False(store.Remove("Recovered"));
        Assert.False(store.Save());
        Assert.Empty(io.WriteAttempts);
    }

    [Fact]
    public void EmptyObjectRowsDoNotDefeatUsefulBackupRecovery()
    {
        var io = new FakeWaypointIo();
        string p = WaypointTestData.Slot1Path;
        io.Files[p] = "[{}]";
        io.Files[p + ".bak"] = WaypointTestData.Serialize(("Recovered", 1, 2, 3, "#FFFFFF"));
        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();
        Assert.Single(store.Rows);
        Assert.Equal("Recovered", store.Rows[0].Name);
        Assert.True(store.IsFaulted);
    }

    [Fact]
    public void ExistenceProbeFailureLeavesStoreReadOnly()
    {
        var store = new WaypointStore(() => "slot_1", s => s, (_, _) => true,
            _ => throw new System.IO.IOException("probe failed"), _ => "[]");
        store.Load();
        Assert.True(store.IsFaulted);
        Assert.False(store.Add("No", 1, 2, 3, "#FFFFFF"));
    }

    [Fact]
    public void RetryCapacityKeepsTheLastSnapshotHiddenAndRecoverable()
    {
        var io = new FakeWaypointIo { FailWrite = _ => true };
        string? slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);
        for (int i = 1; i <= WaypointStore.MaxPendingWrites + 1; i++)
        {
            slot = "slot_" + i;
            store.Load();
            Assert.True(store.Add("Point" + i, i, 0, 0, "#FFFFFF"));
        }
        int before = store.Revision;
        slot = "slot_" + (WaypointStore.MaxPendingWrites + 2);
        store.Load();
        Assert.True(store.IsFaulted);
        Assert.Empty(store.Rows);
        Assert.True(store.Revision > before); // Unity projection must also stop showing old rows.
        io.FailWrite = null;
        store.Load();
        Assert.False(store.IsFaulted);
        for (int i = 1; i <= WaypointStore.MaxPendingWrites + 1; i++)
            Assert.Contains("Point" + i, io.Files[$"/data/waypoints_slot_{i}.json"]);
    }

    [Fact]
    public void NamesCannotInjectControlCharactersIntoLogsOrLabels()
    {
        Assert.Null(WaypointStore.NormalizeName("Base\nFake line"));
        Assert.Equal("München", WaypointStore.NormalizeName("München"));
    }

    [Fact]
    public void UnboundedLabelsAreRejected()
    {
        Assert.Null(WaypointStore.NormalizeName(new string('x', 4096)));
        Assert.Equal("Normal", WaypointStore.NormalizeName(" Normal "));
    }
}

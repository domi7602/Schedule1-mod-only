using System;
using System.Collections.Generic;
using System.Linq;
using Minimap;
using Xunit;

namespace Minimap.Tests;

/// <summary>
/// In-memory IO double for the waypoint store. Records every write attempt so a test can
/// prove which file a payload targeted, and can inject a per-path write failure to exercise
/// the retry semantics without touching disk.
/// </summary>
internal sealed class FakeWaypointIo
{
    public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
    public readonly List<string> WriteAttempts = new();

    /// <summary>Return true to make a write to that path fail (as SafeStorage.SaveTextAtomic does).</summary>
    public Func<string, bool>? FailWrite;

    public bool Write(string path, string content)
    {
        WriteAttempts.Add(path);
        if (FailWrite != null && FailWrite(path)) return false;
        Files[path] = content;
        return true;
    }

    public bool Exists(string path) => Files.ContainsKey(path);

    public string? Read(string path) => Files.TryGetValue(path, out var value) ? value : null;

    public int WritesTo(string path) =>
        WriteAttempts.Count(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase));
}

internal static class WaypointTestData
{
    public const string Slot1Path = "/data/waypoints_slot_1.json";
    public const string Slot2Path = "/data/waypoints_slot_2.json";

    public static string Serialize(params (string Name, float X, float Y, float Z, string Color)[] rows)
    {
        var list = new List<WaypointFile>(rows.Length);
        foreach (var row in rows)
            list.Add(new WaypointFile { Name = row.Name, X = row.X, Y = row.Y, Z = row.Z, ColorHex = row.Color });
        return MinimapJson.SerializeWaypoints(list);
    }

    public static WaypointStore Store(FakeWaypointIo io, Func<string?> slot) =>
        new(slot, s => $"/data/waypoints_{s}.json", io.Write, io.Exists, io.Read);
}

/// <summary>
/// D1/D4: write-failure and save-slot-context regressions. SaveTextAtomic returns a bool
/// that the old code discarded (dirty cleared + "Saved" logged even on failure, no retry),
/// and the old Load/Save resolved the slot lazily — so a slot switch mid-operation flushed
/// the wrong (already emptied or foreign) data into the wrong file.
/// </summary>
public sealed class WaypointStoreTests
{
    [Fact]
    public void Save_WhenWriteFails_KeepsDirtyAndRetriesTheSameFile()
    {
        var io = new FakeWaypointIo();
        string slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);

        store.Load();
        Assert.True(store.Add("Base", 1, 2, 3, "#E14BFF"));

        io.FailWrite = _ => true;
        Assert.False(store.Save());
        Assert.Equal(1, store.Count);
        Assert.Equal(1, io.WritesTo(WaypointTestData.Slot1Path));
        Assert.False(io.Files.ContainsKey(WaypointTestData.Slot1Path));

        io.FailWrite = null;
        Assert.True(store.Save());
        Assert.Equal(2, io.WritesTo(WaypointTestData.Slot1Path));
        Assert.Contains("Base", io.Files[WaypointTestData.Slot1Path]);
    }

    [Fact]
    public void Load_OnSlotSwitch_FlushesPendingRowsToOldFileAndNeverWipesIt()
    {
        var io = new FakeWaypointIo();
        io.Files[WaypointTestData.Slot1Path] = WaypointTestData.Serialize(("Base", 1, 2, 3, "#E14BFF"));
        io.Files[WaypointTestData.Slot2Path] = WaypointTestData.Serialize(("Docks", 9, 8, 7, "#33AAFF"));

        string slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);
        store.Load();
        Assert.Single(store.Rows);

        Assert.True(store.Add("Extra", 5, 5, 5, "#FFFFFF"));

        slot = "slot_2";
        store.Load();

        // D4: slot_1's pending data is flushed to slot_1, never wiped by the reload.
        Assert.Contains("Base", io.Files[WaypointTestData.Slot1Path]);
        Assert.Contains("Extra", io.Files[WaypointTestData.Slot1Path]);
        // New slot loaded; slot_1's rows were never written into slot_2.
        Assert.Single(store.Rows);
        Assert.Equal("Docks", store.Rows[0].Name);
        Assert.DoesNotContain("Base", io.Files[WaypointTestData.Slot2Path]);
        Assert.DoesNotContain("Extra", io.Files[WaypointTestData.Slot2Path]);
    }

    [Fact]
    public void Save_AfterSlotSwitch_NeverWritesOldRowsIntoNewSlot()
    {
        var io = new FakeWaypointIo();
        io.Files[WaypointTestData.Slot1Path] = WaypointTestData.Serialize(("Base", 1, 2, 3, "#E14BFF"));

        string slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);
        store.Load();
        Assert.True(store.Add("Extra", 5, 5, 5, "#FFFFFF"));

        slot = "slot_2"; // context changed without a reload
        store.Save();

        Assert.Contains("Extra", io.Files[WaypointTestData.Slot1Path]);
        Assert.False(io.Files.ContainsKey(WaypointTestData.Slot2Path));
    }

    [Fact]
    public void FailedSlotFlush_IsRetriedToTheOriginalFileOnly()
    {
        var io = new FakeWaypointIo();
        io.Files[WaypointTestData.Slot1Path] = WaypointTestData.Serialize(("Base", 1, 2, 3, "#E14BFF"));
        io.Files[WaypointTestData.Slot2Path] = WaypointTestData.Serialize(("Docks", 9, 8, 7, "#33AAFF"));

        string slot = "slot_1";
        var store = WaypointTestData.Store(io, () => slot);
        store.Load();
        store.Add("Extra", 5, 5, 5, "#FFFFFF");

        io.FailWrite = p => string.Equals(p, WaypointTestData.Slot1Path, StringComparison.OrdinalIgnoreCase);
        slot = "slot_2";
        store.Load();
        Assert.DoesNotContain("Extra", io.Files[WaypointTestData.Slot1Path]);

        io.FailWrite = null;
        store.Save();

        Assert.Contains("Base", io.Files[WaypointTestData.Slot1Path]);
        Assert.Contains("Extra", io.Files[WaypointTestData.Slot1Path]);
        Assert.DoesNotContain("Extra", io.Files[WaypointTestData.Slot2Path]);
        Assert.DoesNotContain("Base", io.Files[WaypointTestData.Slot2Path]);
    }

    [Fact]
    public void Load_WithUnsavedChangesForSameSlot_DoesNotDiscardThem()
    {
        var io = new FakeWaypointIo();
        io.Files[WaypointTestData.Slot1Path] = WaypointTestData.Serialize(("Base", 1, 2, 3, "#E14BFF"));

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();
        store.Add("Extra", 5, 5, 5, "#FFFFFF");

        io.FailWrite = _ => true; // the persist attempt fails
        Assert.False(store.Save());

        store.Load(); // reload the same slot
        Assert.Equal(2, store.Count);
        Assert.Contains(store.Rows, r => r.Name == "Extra");
    }

    [Fact]
    public void Save_FromUnresolvedSession_DoesNotWriteTheDefaultFile()
    {
        var io = new FakeWaypointIo();
        var unresolved = WaypointTestData.Store(io, () => null); // no active slot
        unresolved.Load();
        unresolved.Add("Base", 1, 2, 3, "#E14BFF");

        Assert.False(unresolved.Save());
        Assert.Empty(io.WriteAttempts);

        // positive control: a session that resolved its slot before the add does persist.
        var resolved = WaypointTestData.Store(io, () => "slot_1");
        resolved.Load();
        resolved.Add("Docks", 1, 2, 3, "#E14BFF");
        Assert.True(resolved.Save());
        Assert.Contains("Docks", io.Files[WaypointTestData.Slot1Path]);
    }
}

using System.Linq;
using Minimap;
using Xunit;

namespace Minimap.Tests;

/// <summary>
/// D3: a readable-but-invalid primary JSON file must not be treated as "no data". The old
/// Load cleared the in-memory list and only consulted the .bak on an IO error, so a corrupt
/// primary silently produced zero waypoints — and the next Save overwrote the corrupt file.
/// Recovery must commit only validated data, never fabricate an empty list, and must not
/// overwrite an unreadable primary silently.
/// </summary>
public sealed class WaypointLoadRecoveryTests
{
    private const string Primary = WaypointTestData.Slot1Path;
    private static readonly string Backup = Primary + ".bak";

    [Fact]
    public void Load_ValidPrimary_UsesPrimaryNotBackup()
    {
        var io = new FakeWaypointIo();
        io.Files[Primary] = WaypointTestData.Serialize(("Base", 1, 2, 3, "#E14BFF"));
        io.Files[Backup] = WaypointTestData.Serialize(("Old", 9, 9, 9, "#000000"));

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Single(store.Rows);
        Assert.Equal("Base", store.Rows[0].Name);
        Assert.False(store.IsFaulted);
    }

    [Fact]
    public void Load_InvalidPrimary_RecoversOnlyValidatedBackup()
    {
        var io = new FakeWaypointIo();
        io.Files[Primary] = "{ this is not valid json";
        io.Files[Backup] = WaypointTestData.Serialize(("Docks", 1, 2, 3, "#33AAFF"));

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Single(store.Rows);
        Assert.Equal("Docks", store.Rows[0].Name);
        Assert.True(store.IsFaulted); // preserve the good backup until explicit repair/clear
    }

    [Fact]
    public void Load_MissingPrimary_RecoversFromBackup()
    {
        var io = new FakeWaypointIo();
        io.Files[Backup] = WaypointTestData.Serialize(("Docks", 1, 2, 3, "#33AAFF"));

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Single(store.Rows);
        Assert.Equal("Docks", store.Rows[0].Name);
        Assert.False(store.IsFaulted);
    }

    [Fact]
    public void Load_MissingEverything_IsFreshStartWithoutFault()
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Empty(store.Rows);
        Assert.False(store.IsFaulted);
        Assert.True(store.Add("Base", 1, 2, 3, "#E14BFF")); // positive control: usable, not faulted
        Assert.True(store.Save());
    }

    [Fact]
    public void Load_InvalidJsonObject_IsNotSilentlyTreatedAsEmpty()
    {
        var io = new FakeWaypointIo();
        io.Files[Primary] = "{}";

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Empty(store.Rows);
        Assert.True(store.IsFaulted);
    }

    [Fact]
    public void Load_InvalidPrimaryAndInvalidBackup_MarksFaultAndRefusesToOverwrite()
    {
        var io = new FakeWaypointIo();
        io.Files[Primary] = "{ broken";
        io.Files[Backup] = "also broken";

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Empty(store.Rows);
        Assert.True(store.IsFaulted);

        store.Add("Base", 1, 2, 3, "#E14BFF");
        Assert.False(store.Save());
        Assert.Empty(io.WriteAttempts);
        Assert.Equal("{ broken", io.Files[Primary]);
    }

    [Fact]
    public void Load_UnreadablePrimary_IsNotOverwrittenSilently()
    {
        // Simulate an IO error reading the primary (e.g. file lock): read throws.
        var failingRead = new FakeWaypointIo();
        failingRead.Files[Primary] = "anything";
        var store = new WaypointStore(
            () => "slot_1",
            s => $"/data/waypoints_{s}.json",
            failingRead.Write,
            failingRead.Exists,
            path => failingRead.Read(path) is null ? throw new System.IO.IOException("locked") : failingRead.Read(path));

        store.Load();

        Assert.True(store.IsFaulted);
        store.Add("Base", 1, 2, 3, "#E14BFF");
        Assert.False(store.Save());
        Assert.Empty(failingRead.WriteAttempts);
    }

    [Fact]
    public void Clear_AfterFault_IsTheRecoverActionThatAllowsAWrite()
    {
        var io = new FakeWaypointIo();
        io.Files[Primary] = "{ broken";

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();
        Assert.True(store.IsFaulted);

        store.Clear();
        Assert.False(store.IsFaulted);
        Assert.True(store.Save());

        // The corrupt primary is replaced by validated (empty) JSON, not by "{}" garbage.
        var reparsed = MinimapJson.DeserializeWaypoints(io.Files[Primary]);
        Assert.NotNull(reparsed);
        Assert.Empty(reparsed!);
    }

    [Fact]
    public void RoundTrip_SaveThenLoadInFreshStore_PreservesRows()
    {
        var io = new FakeWaypointIo();

        var writer = WaypointTestData.Store(io, () => "slot_1");
        writer.Load();
        Assert.True(writer.Add("Base", 1.5f, 2.5f, 3.5f, "#ff0000"));
        Assert.True(writer.Add("Docks", -4f, 0f, 8f, "#33aaff"));
        Assert.True(writer.Save());

        var reader = WaypointTestData.Store(io, () => "slot_1");
        reader.Load();

        Assert.Equal(2, reader.Count);
        Assert.Equal("Base", reader.Rows[0].Name);
        Assert.Equal(1.5f, reader.Rows[0].X, 3);
        Assert.Equal("#FF0000", reader.Rows[0].ColorHex);
        Assert.Equal("Docks", reader.Rows[1].Name);
        Assert.Equal("#33AAFF", reader.Rows[1].ColorHex);
    }
}

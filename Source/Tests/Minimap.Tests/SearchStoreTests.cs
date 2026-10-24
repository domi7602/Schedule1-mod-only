using System;
using System.Collections.Generic;
using System.IO;
using Minimap;
using Xunit;

namespace Minimap.Tests;

/// <summary>
/// Versioned, fail-closed persistence for the optional search intent. These tests exercise the
/// production <see cref="SearchStore"/> against injected file IO, so no game runtime is needed.
/// </summary>
public class SearchStoreTests
{
    private const string Path = "userdata/Minimap/search.json";
    private const string BackupPath = Path + ".bak";

    [Fact]
    public void FreshStoreIsEmptyAndWritable()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);

        store.Load();

        Assert.Equal(0, store.Count);
        Assert.False(store.IsFaulted);
        Assert.False(store.IsDirty);
        Assert.Null(store.Find("world-a"));
    }

    [Fact]
    public void SaveWritesTheCurrentFormatVersion()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true));
        Assert.True(store.Save());

        string json = io.Files[Path];
        Assert.Contains("\"Version\": 1", json);
        Assert.Contains("world-a", json);
        Assert.DoesNotContain("Completed", json);
    }

    [Fact]
    public void RecordRoundTripsThroughSaveAndLoad()
    {
        var io = new FakeIo();
        SearchStore writer = Create(io);
        writer.Load();
        Assert.True(writer.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 2, true)));
        Assert.True(writer.Save());

        SearchStore reader = Create(io);
        reader.Load();

        SearchRecord? record = reader.Find("world-a");
        Assert.NotNull(record);
        Assert.Equal("target-1", record!.Target);
        Assert.Equal("sewer-key", record.AreaId);
        Assert.Equal(2, record.AreaVersion);
        Assert.True(record.Accepted);
    }

    [Fact]
    public void CancelledIntentIsPersistedAsCancelled()
    {
        var io = new FakeIo();
        SearchStore writer = Create(io);
        writer.Load();
        writer.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, false));
        Assert.True(writer.Save());

        SearchStore reader = Create(io);
        reader.Load();

        SearchRecord? record = reader.Find("world-a");
        Assert.NotNull(record);
        Assert.False(record!.Accepted);
    }

    [Theory]
    [InlineData("", "target-1", "area", 1)]
    [InlineData("   ", "target-1", "area", 1)]
    [InlineData("world-a", "", "area", 1)]
    [InlineData("world-a", "target-1", "", 1)]
    [InlineData("world-a", "target-1", "area", -1)]
    public void InvalidRecordsAreRejected(string world, string target, string areaId, int version)
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        Assert.False(store.Remember(new SearchRecord(world, target, areaId, version, true)));
        Assert.Equal(0, store.Count);
        Assert.False(store.IsDirty);
        // Nothing to write is a successful no-op, not an error.
        Assert.True(store.Save());
        Assert.False(io.Files.ContainsKey(Path));
    }

    [Fact]
    public void RememberingTheSameWorldReplacesInsteadOfAppending()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true));
        store.Remember(new SearchRecord("world-a", "target-2", "sewer-key", 1, false));

        Assert.Equal(1, store.Count);
        Assert.Equal("target-2", store.Find("world-a")!.Target);
        Assert.False(store.Find("world-a")!.Accepted);
    }

    [Fact]
    public void IdenticalRecordIsANoOp()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        Assert.True(store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true)));
        Assert.True(store.Save());
        Assert.False(store.IsDirty);

        Assert.True(store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true)));
        Assert.False(store.IsDirty);
    }

    [Fact]
    public void NewestRecordComesFirst()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true));
        store.Remember(new SearchRecord("world-b", "target-2", "sewer-key", 1, true));

        Assert.Equal("world-b", store.Records[0].World);
        Assert.Equal("world-a", store.Records[1].World);
    }

    [Fact]
    public void StoreIsBoundedAndDropsTheOldestWorld()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        for (int i = 0; i < SearchStore.MaxRecords + 3; i++)
            store.Remember(new SearchRecord($"world-{i}", "target", "sewer-key", 1, true));

        Assert.Equal(SearchStore.MaxRecords, store.Count);
        Assert.Null(store.Find("world-0"));
        Assert.Null(store.Find("world-2"));
        Assert.NotNull(store.Find("world-3"));
        Assert.NotNull(store.Find($"world-{SearchStore.MaxRecords + 2}"));
    }

    [Fact]
    public void FailedWriteKeepsTheStoreDirtyAndRetriesTheSamePath()
    {
        var io = new FakeIo { FailWrites = true };
        SearchStore store = Create(io);
        store.Load();
        store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true));

        Assert.False(store.Save());
        Assert.True(store.IsDirty);

        io.FailWrites = false;
        Assert.True(store.Save());
        Assert.False(store.IsDirty);
        Assert.Equal(new[] { Path }, io.Writes);
    }

    [Fact]
    public void SaveWithoutChangesDoesNotTouchTheDisk()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        Assert.True(store.Save());
        Assert.Empty(io.Writes);
    }

    [Fact]
    public void CorruptPrimaryWithoutBackupIsReadOnlyUntilCleared()
    {
        var io = new FakeIo();
        io.Files[Path] = "{ this is not json";
        SearchStore store = Create(io);

        store.Load();

        Assert.True(store.IsFaulted);
        Assert.Equal(0, store.Count);
        Assert.False(store.Save());
        Assert.Equal("{ this is not json", io.Files[Path]);

        Assert.True(store.Clear());
        Assert.False(store.IsFaulted);
        Assert.True(store.Save());
        Assert.Equal(0, store.Count);
        Assert.DoesNotContain("this is not json", io.Files[Path]);
    }

    [Fact]
    public void CorruptPrimaryWithValidBackupServesBackupReadOnly()
    {
        var io = new FakeIo();
        io.Files[Path] = "{ broken";
        io.Files[BackupPath] = ValidDocument("world-a", "target-1", accepted: true);
        SearchStore store = Create(io);

        store.Load();

        Assert.True(store.IsFaulted);
        Assert.Equal(1, store.Count);
        Assert.NotNull(store.Find("world-a"));
        Assert.False(store.Save());
        Assert.Equal("{ broken", io.Files[Path]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(99)]
    public void UnknownFormatVersionIsRejected(int version)
    {
        var io = new FakeIo();
        io.Files[Path] = $$"""
        {
          "Version": {{version}},
          "Records": [ { "World": "world-a", "Target": "target-1", "AreaId": "sewer-key", "AreaVersion": 1, "Accepted": true } ]
        }
        """;
        SearchStore store = Create(io);

        store.Load();

        Assert.True(store.IsFaulted);
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void DuplicateWorldsAreRejected()
    {
        var io = new FakeIo();
        io.Files[Path] = """
        {
          "Version": 1,
          "Records": [
            { "World": "world-a", "Target": "t1", "AreaId": "sewer-key", "AreaVersion": 1, "Accepted": true },
            { "World": "world-a", "Target": "t2", "AreaId": "sewer-key", "AreaVersion": 1, "Accepted": false }
          ]
        }
        """;
        SearchStore store = Create(io);

        store.Load();

        Assert.True(store.IsFaulted);
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void InvalidRowRejectsTheWholeDocument()
    {
        var io = new FakeIo();
        io.Files[Path] = """
        {
          "Version": 1,
          "Records": [
            { "World": "world-a", "Target": "t1", "AreaId": "sewer-key", "AreaVersion": 1, "Accepted": true },
            { "World": "", "Target": "t2", "AreaId": "sewer-key", "AreaVersion": 1, "Accepted": true }
          ]
        }
        """;
        SearchStore store = Create(io);

        store.Load();

        Assert.True(store.IsFaulted);
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void MoreRowsThanTheCapIsRejected()
    {
        var io = new FakeIo();
        var rows = new List<string>();
        for (int i = 0; i <= SearchStore.MaxRecords; i++)
            rows.Add($$"""{ "World": "world-{{i}}", "Target": "t", "AreaId": "sewer-key", "AreaVersion": 1, "Accepted": true }""");
        io.Files[Path] = "{ \"Version\": 1, \"Records\": [ " + string.Join(",", rows) + " ] }";
        SearchStore store = Create(io);

        store.Load();

        Assert.True(store.IsFaulted);
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void OversizedDocumentIsRejected()
    {
        var io = new FakeIo();
        string huge = new string('x', SearchStore.MaxJsonLength + 1);
        io.Files[Path] = huge;
        SearchStore store = Create(io);

        store.Load();

        Assert.True(store.IsFaulted);
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void KeysAreTrimmedOnWriteAndLookup()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        Assert.True(store.Remember(new SearchRecord("  world-a  ", " target-1 ", " sewer-key ", 1, true)));
        Assert.Equal("world-a", store.Find("world-a")!.World);
        Assert.Equal("target-1", store.Find("world-a")!.Target);
        Assert.NotNull(store.Find(" world-a "));
        Assert.Null(store.Find("   "));
        Assert.Null(store.Find(null));
    }

    [Fact]
    public void ForgetRemovesOnlyTheNamedWorld()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();
        store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true));
        store.Remember(new SearchRecord("world-b", "target-2", "sewer-key", 1, true));
        Assert.True(store.Save());
        Assert.False(store.IsDirty);

        Assert.True(store.Forget("world-a"));
        Assert.True(store.IsDirty);
        Assert.Null(store.Find("world-a"));
        Assert.NotNull(store.Find("world-b"));
        Assert.False(store.Forget("world-a"));
        Assert.False(store.Forget(""));
    }

    [Fact]
    public void OversizedKeysAreRejected()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();

        string tooLong = new string('w', SearchStore.MaxWorldLength + 1);
        Assert.False(store.Remember(new SearchRecord(tooLong, "target-1", "sewer-key", 1, true)));
        Assert.Null(store.Find(tooLong));
    }

    [Fact]
    public void RepeatedLoadRebuildsFromDisk()
    {
        var io = new FakeIo();
        SearchStore store = Create(io);
        store.Load();
        store.Remember(new SearchRecord("world-a", "target-1", "sewer-key", 1, true));
        Assert.True(store.Save());

        store.Remember(new SearchRecord("unsaved", "target-9", "sewer-key", 1, true));
        Assert.True(store.IsDirty);

        store.Load();

        Assert.Equal(1, store.Count);
        Assert.Null(store.Find("unsaved"));
        Assert.NotNull(store.Find("world-a"));
        Assert.False(store.IsDirty);
    }

    [Fact]
    public void UnreadableIoDoesNotThrow()
    {
        var io = new FakeIo { ThrowOnExists = true, ThrowOnRead = true };
        SearchStore store = Create(io);

        store.Load();

        Assert.False(store.IsFaulted);
        Assert.Equal(0, store.Count);
    }

    private static SearchStore Create(FakeIo io) => new(
        Path,
        io.Write,
        io.Exists,
        io.Read);

    private static string ValidDocument(string world, string target, bool accepted) => $$"""
    {
      "Version": 1,
      "Records": [
        { "World": "{{world}}", "Target": "{{target}}", "AreaId": "sewer-key", "AreaVersion": 1, "Accepted": {{(accepted ? "true" : "false")}} }
      ]
    }
    """;

    private sealed class FakeIo
    {
        public Dictionary<string, string> Files { get; } = new(StringComparer.OrdinalIgnoreCase);
        public List<string> Writes { get; } = new();
        public bool FailWrites { get; set; }
        public bool ThrowOnExists { get; set; }
        public bool ThrowOnRead { get; set; }

        public bool Exists(string path)
            => ThrowOnExists ? throw new IOException("probe failed") : Files.ContainsKey(path);

        public string? Read(string path)
            => ThrowOnRead ? throw new IOException("read failed") : Files.TryGetValue(path, out string? text) ? text : null;

        public bool Write(string path, string content)
        {
            if (FailWrites)
                return false;
            Files[path] = content;
            Writes.Add(path);
            return true;
        }
    }
}

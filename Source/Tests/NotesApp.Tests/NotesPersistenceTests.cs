using System;
using System.Collections.Generic;
using System.Linq;
using NotesApp;
using Xunit;

namespace NotesApp.Tests;

/// <summary>
/// Proves the save policy itself. The old NotesApp.Save ignored a false writer
/// result, so a failed write looked saved and the next path lookup could retarget
/// the notes. These tests fail that old outcome without a source-text search.
/// </summary>
public sealed class NotesPersistenceTests
{
    private const string Slot1 = "slot_1";
    private const string Slot2 = "slot_2";
    private const string Path1 = "/userdata/NotesApp/notes_slot_1.json";
    private const string Path2 = "/userdata/NotesApp/notes_slot_2.json";

    [Fact]
    public void Save_WhenWriteSucceeds_ClearsFailureAndPublishesTheBoundFile()
    {
        var io = new FakeNotesIo();
        var store = Bind(io, Slot1, Path1);

        Assert.True(store.SaveSlot(Slot1, Notes("Alpha")));

        Assert.False(store.SaveFailed);
        Assert.Null(store.StatusText);
        Assert.Equal("Alpha", io.Files[Path1]);
        Assert.Equal(Path1, io.Attempts.Single().Path);
    }

    [Fact]
    public void Save_WhenWriteFails_KeepsNotesAndDoesNotClaimSuccess()
    {
        var io = new FakeNotesIo { FailAll = true };
        var store = Bind(io, Slot1, Path1);
        var notes = Notes("Keep me");

        Assert.False(store.SaveSlot(Slot1, notes));

        Assert.True(store.SaveFailed);
        Assert.Equal(NotesPersistence.FailureStatus, store.StatusText);
        Assert.Equal(Path1, store.BoundPath);
        Assert.False(io.Files.ContainsKey(Path1));
        Assert.Equal("Keep me", notes[0].Title);
        Assert.Equal(1, io.WritesTo(Path1));
    }

    [Fact]
    public void FailedSave_ThenSuccess_ClearsTheErrorAndWritesTheSameNotes()
    {
        var io = new FakeNotesIo { FailAll = true };
        var store = Bind(io, Slot1, Path1);
        var notes = Notes("Retry");

        Assert.False(store.SaveSlot(Slot1, notes));
        io.FailAll = false;
        Assert.True(store.SaveSlot(Slot1, notes));

        Assert.False(store.SaveFailed);
        Assert.Null(store.StatusText);
        Assert.Equal("Retry", io.Files[Path1]);
        Assert.Equal(2, io.WritesTo(Path1));
    }

    [Fact]
    public void FailedSave_IsNotRetargetedWhenTheCallerPassesAnotherPath()
    {
        var io = new FakeNotesIo { FailAll = true };
        var store = Bind(io, Slot1, Path1);
        Assert.False(store.SaveSlot(Slot1, Notes("Bound")));

        var ignored = store.Prepare(Slot1, "/userdata/NotesApp/notes_slot_9.json", Notes("Bound"));

        Assert.True(ignored.KeepMemory);
        Assert.Equal(Path1, store.PathForSlot(Slot1));
        io.FailAll = false;
        Assert.True(store.SaveSlot(Slot1, Notes("Bound")));
        Assert.Equal(Path1, io.Attempts.Last().Path);
        Assert.False(io.Files.ContainsKey("/userdata/NotesApp/notes_slot_9.json"));
    }

    [Fact]
    public void SlotChange_AfterFailedSave_DoesNotWriteOldNotesIntoTheNewSlot()
    {
        var io = new FakeNotesIo { Fail = path => path == Path1 };
        var store = Bind(io, Slot1, Path1);
        var oldNotes = Notes("Slot one");
        Assert.False(store.SaveSlot(Slot1, oldNotes));

        SlotPreparation moved = store.Prepare(Slot2, Path2, oldNotes);

        Assert.False(moved.KeepMemory);
        Assert.Null(moved.Restore);
        Assert.Equal(Slot2, moved.Slot);
        Assert.DoesNotContain(io.Attempts, attempt => attempt.Path == Path2);
        Assert.False(io.Files.ContainsKey(Path1));
        Assert.True(store.SaveFailed);
        Assert.Equal(NotesPersistence.FailureStatus, store.StatusText);

        store.MarkClean(Slot2, Path2);
        Assert.True(store.SaveSlot(Slot2, Notes("Slot two")));
        Assert.Equal("Slot two", io.Files[Path2]);
        Assert.False(io.Files.ContainsKey(Path1));
        Assert.Equal("Slot one", io.Attempts.Last(attempt => attempt.Path == Path1).Titles);
    }

    [Fact]
    public void LaterSuccess_WritesTheRetainedSlotAndLeavesTheOtherSlotUntouched()
    {
        var io = new FakeNotesIo { Fail = path => path == Path1 };
        var store = Bind(io, Slot1, Path1);
        Assert.False(store.SaveSlot(Slot1, Notes("Keep")));
        store.Prepare(Slot2, Path2, Notes("Keep"));
        store.MarkClean(Slot2, Path2);
        Assert.True(store.SaveSlot(Slot2, Notes("Other")));

        io.Fail = null;
        store.FlushOnce(Slot2, Notes("Other"));

        Assert.False(store.SaveFailed);
        Assert.Null(store.StatusText);
        Assert.Equal("Keep", io.Files[Path1]);
        Assert.Equal("Other", io.Files[Path2]);
    }

    [Fact]
    public void ReturningToTheFailedSlot_RestoresUnsavedNotesInsteadOfDisk()
    {
        var io = new FakeNotesIo { Fail = path => path == Path1 };
        var store = Bind(io, Slot1, Path1);
        Assert.False(store.SaveSlot(Slot1, Notes("Unsaved")));
        store.Prepare(Slot2, Path2, Notes("Unsaved"));

        SlotPreparation back = store.Prepare(Slot1, Path1, Notes("Other view"));

        Assert.False(back.KeepMemory);
        Assert.NotNull(back.Restore);
        Assert.Equal("Unsaved", back.Restore![0].Title);
        Assert.Equal(Path1, back.Path);
        // The failed save plus one leave-flush. Coming back must not write again.
        Assert.Equal(2, io.WritesTo(Path1));
    }

    [Fact]
    public void RepeatedPrepare_DoesNotWriteEveryCall()
    {
        var io = new FakeNotesIo { FailAll = true };
        var store = Bind(io, Slot1, Path1);
        Assert.False(store.SaveSlot(Slot1, Notes("Once")));
        int afterSave = io.Attempts.Count;

        for (int i = 0; i < 25; i++)
        {
            SlotPreparation again = store.Prepare(Slot1, Path1, Notes("Once"));
            Assert.True(again.KeepMemory);
        }

        Assert.Equal(afterSave, io.Attempts.Count);
        Assert.Single(io.ErrorLog);
    }

    [Fact]
    public void LeavingASlot_FlushesOnce_NotAgainOnTheNextPrepare()
    {
        var io = new FakeNotesIo { FailAll = true };
        var store = Bind(io, Slot1, Path1);
        Assert.False(store.SaveSlot(Slot1, Notes("Frozen")));

        store.Prepare(Slot2, Path2, Notes("Frozen"));
        int afterLeave = io.WritesTo(Path1);
        store.Prepare(Slot2, Path2, Notes("Loaded"));
        store.Prepare("slot_3", "/userdata/NotesApp/notes_slot_3.json", Notes("Loaded"));

        // Explicit save plus one automatic flush when the slot is left. Later prepares do not write again.
        Assert.Equal(2, afterLeave);
        Assert.Equal(2, io.WritesTo(Path1));
        Assert.Equal(0, io.WritesTo(Path2));
    }

    [Fact]
    public void EmptyCaller_OnSlotChange_DoesNotEraseTheRetainedSnapshot()
    {
        var io = new FakeNotesIo { FailAll = true };
        var store = Bind(io, Slot1, Path1);
        Assert.False(store.SaveSlot(Slot1, Notes("Survive")));

        store.Prepare(Slot2, Path2, new List<Note>());
        io.FailAll = false;
        store.FlushOnce(Slot2, new List<Note>());

        Assert.Equal("Survive", io.Files[Path1]);
        Assert.False(io.Files.ContainsKey(Path2));
    }

    [Fact]
    public void FreshCaller_ReceivesTheRetainedSnapshotInsteadOfAnEmptyList()
    {
        var io = new FakeNotesIo { FailAll = true };
        var store = Bind(io, Slot1, Path1);
        Assert.False(store.SaveSlot(Slot1, Notes("Survive")));

        SlotPreparation reloaded = store.Prepare(Slot1, Path1, new List<Note>());

        Assert.True(reloaded.KeepMemory);
        Assert.NotNull(reloaded.Restore);
        Assert.Equal("Survive", reloaded.Restore![0].Title);
        Assert.Equal(Path1, reloaded.Path);
        Assert.Equal(1, io.WritesTo(Path1));
    }

    [Fact]
    public void ExplicitEmptySave_IsKeptInsteadOfResurrectingOlderNotes()
    {
        var io = new FakeNotesIo();
        var store = Bind(io, Slot1, Path1);
        Assert.True(store.SaveSlot(Slot1, Notes("Old")));
        io.FailAll = true;
        Assert.False(store.SaveSlot(Slot1, new List<Note>()));

        SlotPreparation again = store.Prepare(Slot1, Path1, new List<Note>());

        Assert.True(again.KeepMemory);
        Assert.Equal("Old", io.Files[Path1]);
        io.FailAll = false;
        Assert.True(store.SaveSlot(Slot1, new List<Note>()));
        Assert.Equal(string.Empty, io.Files[Path1]);
    }

    private static NotesPersistence Bind(FakeNotesIo io, string slot, string path)
    {
        var store = new NotesPersistence(io.Write, io.LogError, io.LogInfo);
        SlotPreparation first = store.Prepare(slot, path, Array.Empty<Note>());
        Assert.False(first.KeepMemory);
        store.MarkClean(slot, path);
        return store;
    }

    private static List<Note> Notes(string title)
        => new() { new Note { Title = title, Text = title + " body", CreatedAt = DateTime.UnixEpoch, UpdatedAt = DateTime.UnixEpoch } };

    private sealed class FakeNotesIo
    {
        public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
        public readonly List<Attempt> Attempts = new();
        public readonly List<string> ErrorLog = new();
        public bool FailAll;
        public Func<string, bool>? Fail;

        public bool Write(string path, IReadOnlyList<Note> notes)
        {
            string titles = string.Join("|", notes.Select(note => note.Title));
            Attempts.Add(new Attempt(path, titles));
            if (FailAll || (Fail != null && Fail(path)))
                return false;
            Files[path] = titles;
            return true;
        }

        public void LogError(string message) => ErrorLog.Add(message);

        public void LogInfo(string message)
        {
        }

        public int WritesTo(string path)
            => Attempts.Count(attempt => string.Equals(attempt.Path, path, StringComparison.OrdinalIgnoreCase));
    }

    private readonly record struct Attempt(string Path, string Titles);
}

using System;
using System.Collections.Generic;

namespace NotesApp;

public sealed class Note
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPinned { get; set; }
}

/// <summary>
/// Slot-bound save policy for NotesApp. Unity-free so a failed write, a later
/// success and a slot change can be tested without the game.
///
/// A snapshot stays on the path captured for its slot. Save reports success only
/// when the writer returns true. A later slot change cannot retarget that snapshot.
/// Automatic retries happen once per leaving a slot, not on a frame tick.
/// </summary>
public sealed class NotesPersistence
{
    public const string FailureStatus = "Save failed. Notes kept for this save slot.";

    private readonly Func<string, IReadOnlyList<Note>, bool> _write;
    private readonly Action<string>? _logError;
    private readonly Action<string>? _logInfo;
    private readonly Dictionary<string, string> _paths = new(StringComparer.Ordinal);
    private readonly List<Pending> _pending = new();

    private string? _slot;
    private string? _path;
    private bool _dirty;
    private bool _failed;

    public NotesPersistence(
        Func<string, IReadOnlyList<Note>, bool> write,
        Action<string>? logError = null,
        Action<string>? logInfo = null)
    {
        _write = write ?? throw new ArgumentNullException(nameof(write));
        _logError = logError;
        _logInfo = logInfo;
    }

    public string? BoundSlot => _slot;
    public string? BoundPath => _path;
    public bool SaveFailed => _failed || _pending.Count > 0;
    public string? StatusText => SaveFailed ? FailureStatus : null;
    public bool HasUnsavedChanges => _dirty || _failed || _pending.Count > 0;
    public int PendingCount => _pending.Count;

    public string? PathForSlot(string slot)
        => _paths.TryGetValue(slot, out string? path) ? path : null;

    /// <summary>
    /// Binds the view to <paramref name="slot"/>. Unsaved notes for the previous
    /// slot are frozen to their original path and flushed once. The caller must
    /// keep its list when <see cref="SlotPreparation.KeepMemory"/> is set.
    /// </summary>
    public SlotPreparation Prepare(string slot, string path, IReadOnlyList<Note> currentNotes)
    {
        if (string.IsNullOrWhiteSpace(slot))
            throw new ArgumentException("A save slot is required.", nameof(slot));
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("A save path is required.", nameof(path));
        currentNotes ??= Array.Empty<Note>();

        RememberPath(slot, path);

        if (string.Equals(slot, _slot, StringComparison.Ordinal))
        {
            if (_dirty || _failed)
            {
                // A new app instance has an empty list. The retained snapshot is authoritative.
                if (TryPeekPending(slot, out List<Note> held))
                    return SlotPreparation.Keep(slot, _path ?? path, held);
                return SlotPreparation.Keep(slot, _path ?? path, currentNotes);
            }

            _path = _paths[slot];
            return SlotPreparation.LoadDisk(slot, _path);
        }

        if (_slot != null && (_dirty || _failed) && _path != null)
        {
            // Do not replace a retained snapshot with an empty list from a freshly created app instance.
            if (!HasPending(_slot) || currentNotes.Count > 0)
                Freeze(_slot, _path, currentNotes);
            TryFlushPending(skipAlreadyAttempted: true);
        }

        _slot = slot;
        _path = _paths[slot];
        _dirty = false;
        _failed = false;

        if (TryDetachPending(slot, out List<Note> restored))
        {
            _dirty = true;
            _failed = true;
            return SlotPreparation.FromPending(slot, _path, restored);
        }

        return SlotPreparation.LoadDisk(slot, _path);
    }

    public void MarkClean(string slot, string path)
    {
        if (string.IsNullOrWhiteSpace(slot) || string.IsNullOrWhiteSpace(path))
            return;
        RememberPath(slot, path);
        if (!string.Equals(slot, _slot, StringComparison.Ordinal))
            return;
        if (_dirty || _failed)
            return;
        _path = _paths[slot];
    }

    /// <summary>
    /// Writes <paramref name="notes"/> only to the path already captured for
    /// <paramref name="slot"/>. A different live slot does not retarget them.
    /// </summary>
    public bool SaveSlot(string slot, IReadOnlyList<Note> notes)
    {
        notes ??= Array.Empty<Note>();
        if (string.IsNullOrWhiteSpace(slot) || !_paths.TryGetValue(slot, out string? path))
        {
            RememberFailure(slot, notes);
            _logError?.Invoke("Notes save skipped: slot '" + slot + "' has no bound path. Notes kept in memory.");
            return false;
        }

        bool ok = Write(path, notes, slot);
        var snapshot = Clone(notes);
        if (ok)
        {
            RemovePending(slot);
            if (string.Equals(slot, _slot, StringComparison.Ordinal))
            {
                _dirty = false;
                _failed = false;
            }
        }
        else
        {
            UpsertPending(slot, path, snapshot, attempted: false);
            if (string.Equals(slot, _slot, StringComparison.Ordinal))
            {
                _dirty = true;
                _failed = true;
            }
        }

        return ok;
    }

    /// <summary>One pass over pending snapshots. Not a frame retry.</summary>
    public void FlushOnce(string viewSlot, IReadOnlyList<Note> viewNotes)
    {
        if ((_dirty || _failed) && !string.IsNullOrWhiteSpace(viewSlot) && _paths.TryGetValue(viewSlot, out string? path))
            UpsertPending(viewSlot, path, Clone(viewNotes), attempted: false);

        TryFlushPending(skipAlreadyAttempted: false);

        if (!string.IsNullOrWhiteSpace(viewSlot)
            && string.Equals(viewSlot, _slot, StringComparison.Ordinal)
            && !HasPending(viewSlot))
        {
            _dirty = false;
            _failed = false;
        }
    }

    private void RememberPath(string slot, string path)
    {
        bool locked = HasPending(slot) || (string.Equals(slot, _slot, StringComparison.Ordinal) && (_dirty || _failed));
        if (locked && _paths.ContainsKey(slot))
            return;
        _paths[slot] = path;
    }

    private void RememberFailure(string? slot, IReadOnlyList<Note> notes)
    {
        _dirty = true;
        _failed = true;
        if (!string.IsNullOrWhiteSpace(slot) && string.Equals(slot, _slot, StringComparison.Ordinal))
            Freeze(slot, _path ?? string.Empty, notes);
    }

    private void Freeze(string slot, string path, IReadOnlyList<Note> notes)
    {
        UpsertPending(slot, path, Clone(notes), attempted: false);
    }

    private void UpsertPending(string slot, string path, List<Note> notes, bool attempted)
    {
        for (int i = 0; i < _pending.Count; i++)
        {
            if (!string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                continue;
            _pending[i].Path = string.IsNullOrEmpty(path) ? _pending[i].Path : path;
            _pending[i].Notes = notes;
            if (!attempted)
                _pending[i].AutoAttempted = false;
            return;
        }

        if (string.IsNullOrEmpty(path))
            return;
        _pending.Add(new Pending(slot, path, notes) { AutoAttempted = attempted });
    }

    private bool TryPeekPending(string slot, out List<Note> notes)
    {
        for (int i = 0; i < _pending.Count; i++)
        {
            if (!string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                continue;
            notes = _pending[i].Notes;
            return true;
        }

        notes = new List<Note>();
        return false;
    }

    private bool TryDetachPending(string slot, out List<Note> notes)
    {
        for (int i = 0; i < _pending.Count; i++)
        {
            if (!string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                continue;
            notes = _pending[i].Notes;
            _pending.RemoveAt(i);
            return true;
        }

        notes = new List<Note>();
        return false;
    }

    private void RemovePending(string slot)
    {
        for (int i = _pending.Count - 1; i >= 0; i--)
        {
            if (string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                _pending.RemoveAt(i);
        }
    }

    private bool HasPending(string slot)
    {
        for (int i = 0; i < _pending.Count; i++)
        {
            if (string.Equals(_pending[i].Slot, slot, StringComparison.Ordinal))
                return true;
        }

        return false;
    }

    private void TryFlushPending(bool skipAlreadyAttempted)
    {
        for (int i = _pending.Count - 1; i >= 0; i--)
        {
            Pending item = _pending[i];
            if (skipAlreadyAttempted && item.AutoAttempted)
                continue;
            if (skipAlreadyAttempted)
                item.AutoAttempted = true;
            if (Write(item.Path, item.Notes, item.Slot))
                _pending.RemoveAt(i);
        }
    }

    private bool Write(string path, IReadOnlyList<Note> notes, string slot)
    {
        var snapshot = Clone(notes);
        bool ok;
        try
        {
            ok = _write(path, snapshot);
        }
        catch (Exception ex)
        {
            _logError?.Invoke("Notes save failed for slot '" + slot + "' at '" + path + "': " + ex.Message);
            return false;
        }

        if (ok)
        {
            _logInfo?.Invoke("Saved " + snapshot.Count + " notes for slot '" + slot + "'.");
            return true;
        }

        _logError?.Invoke(
            "Notes save failed for slot '" + slot + "' at '" + path
            + "'. Notes kept; they will not be written to another slot.");
        return false;
    }

    private static List<Note> Clone(IReadOnlyList<Note> notes)
    {
        var copy = new List<Note>(notes.Count);
        for (int i = 0; i < notes.Count; i++)
        {
            Note? note = notes[i];
            if (note == null)
                continue;
            copy.Add(new Note
            {
                Title = note.Title ?? string.Empty,
                Text = note.Text ?? string.Empty,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt,
                IsPinned = note.IsPinned
            });
        }

        return copy;
    }

    private sealed class Pending
    {
        public Pending(string slot, string path, List<Note> notes)
        {
            Slot = slot;
            Path = path;
            Notes = notes;
        }

        public string Slot { get; }
        public string Path { get; set; }
        public List<Note> Notes { get; set; }
        public bool AutoAttempted { get; set; }
    }
}

public readonly struct SlotPreparation
{
    private SlotPreparation(bool keepMemory, string slot, string path, IReadOnlyList<Note>? restore)
    {
        KeepMemory = keepMemory;
        Slot = slot;
        Path = path;
        Restore = restore;
    }

    public bool KeepMemory { get; }
    public string Slot { get; }
    public string Path { get; }
    public IReadOnlyList<Note>? Restore { get; }

    public static SlotPreparation Keep(string slot, string path, IReadOnlyList<Note> notes)
        => new(true, slot, path, notes);

    public static SlotPreparation LoadDisk(string slot, string path) => new(false, slot, path, null);

    public static SlotPreparation FromPending(string slot, string path, IReadOnlyList<Note> notes)
        => new(false, slot, path, notes);
}

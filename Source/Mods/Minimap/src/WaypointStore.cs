using System;
using System.Collections.Generic;

namespace Minimap;

/// <summary>
/// Unity-free waypoint store: input normalization plus slot-scoped atomic persistence
/// policy. <see cref="MinimapWaypoints"/> supplies the save-slot probe and the real file
/// IO through injected delegates, so failure, retry and slot-switch behaviour can be
/// exercised without the game runtime.
///
/// Persistence contract:
///  - In-memory rows always belong to exactly one slot (<c>_rowsSlot</c>).
///  - When the active slot changes, the rows that belong to the old slot are FROZEN with
///    their ORIGINAL path and flushed there. A failed flush stays pending and is retried to
///    that same file only — rows are never migrated from slot A into slot B.
///  - Save() reports success and clears the dirty flag ONLY when the atomic write returned
///    true; a failed write keeps the data dirty so the next Save retries it.
///  - Load() validates the primary file and the backup before committing anything. A corrupt
///    primary is never overwritten silently, never turned into fabricated (empty) data, and
///    leaves the store read-only until an explicit Clear().
/// </summary>
public sealed class WaypointStore
{
    public const int MaxWaypoints = 16;
    public const int MaxNameLength = 64;
    public const int MaxJsonLength = 131072;
    public const int MaxPendingWrites = 32;
    public const string DefaultColorHex = "#E14BFF";

    /// <summary>Sentinel for "no save slot has been resolved yet". Save() never writes it.</summary>
    private const string UnresolvedSlot = "default";

    private readonly Func<string?> _activeSlot;
    private readonly Func<string, string> _pathForSlot;
    private readonly Func<string, string, bool> _writeAtomic;
    private readonly Func<string, bool> _fileExists;
    private readonly Func<string, string?> _readText;
    private readonly Action<string>? _logInfo;
    private readonly Action<string>? _logWarn;
    private readonly Action<string>? _logError;

    private readonly List<WaypointFile> _rows = new();
    private readonly List<(List<WaypointFile> Rows, string Path)> _pending = new();

    private string _slot = UnresolvedSlot;
    private string _rowsSlot = string.Empty;
    private bool _dirty;
    private bool _activeKnown;
    private bool _faulted;
    private int _revision;

    public WaypointStore(
        Func<string?> activeSlot,
        Func<string, string> pathForSlot,
        Func<string, string, bool> writeAtomic,
        Func<string, bool> fileExists,
        Func<string, string?> readText,
        Action<string>? logInfo = null,
        Action<string>? logWarn = null,
        Action<string>? logError = null)
    {
        _activeSlot = activeSlot ?? throw new ArgumentNullException(nameof(activeSlot));
        _pathForSlot = pathForSlot ?? throw new ArgumentNullException(nameof(pathForSlot));
        _writeAtomic = writeAtomic ?? throw new ArgumentNullException(nameof(writeAtomic));
        _fileExists = fileExists ?? throw new ArgumentNullException(nameof(fileExists));
        _readText = readText ?? throw new ArgumentNullException(nameof(readText));
        _logInfo = logInfo;
        _logWarn = logWarn;
        _logError = logError;
    }

    public int Count => string.Equals(_rowsSlot, _slot, StringComparison.Ordinal) ? _rows.Count : 0;
    public int Revision => _revision;
    public bool IsFaulted => _faulted;
    public IReadOnlyList<WaypointFile> Rows => string.Equals(_rowsSlot, _slot, StringComparison.Ordinal)
        ? _rows : Array.Empty<WaypointFile>();

    // ────────────────────────────────────────────────────────────
    // CRUD — normalization is identical for Add() and Load()
    // ────────────────────────────────────────────────────────────

    public bool Add(string? name, float x, float y, float z, string? colorHex)
        => EnsureContext() && AddCore(name, x, y, z, colorHex, markDirty: true);

    public bool Remove(string? name)
    {
        if (!EnsureContext()) return false;
        string? clean = NormalizeName(name);
        if (clean == null) return false;

        for (int i = 0; i < _rows.Count; i++)
        {
            if (string.Equals(_rows[i].Name, clean, StringComparison.OrdinalIgnoreCase))
            {
                _rows.RemoveAt(i);
                MarkDirty();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes every waypoint. Also clears a load fault: an explicit clear is the user's
    /// recover action for a corrupt store and is what re-enables saving.
    /// </summary>
    public void Clear() => TryClear();

    public bool TryClear()
    {
        if (!EnsureContext(allowFault: true)) return false;
        bool wasFaulted = _faulted;
        _faulted = false;
        if (_rows.Count == 0 && !wasFaulted) return true;
        _rows.Clear();
        MarkDirty();
        return true;
    }

    // ────────────────────────────────────────────────────────────
    // Persistence
    // ────────────────────────────────────────────────────────────

    public bool Save()
    {
        try
        {
            SyncSlot();

            if (_faulted)
            {
                _logWarn?.Invoke("Waypoint save skipped: the waypoint file could not be validated; clear or repair it first.");
                return false;
            }
            if (!_activeKnown || string.Equals(_slot, UnresolvedSlot, StringComparison.Ordinal))
            {
                // Rule 2: never write waypoints_default.json from an unresolved session.
                return false;
            }
            if (!string.Equals(_rowsSlot, _slot, StringComparison.Ordinal))
            {
                // The in-memory rows belong to another slot; writing them here would move
                // slot A's data into slot B.
                return false;
            }

            string path = _pathForSlot(_slot);
            string json = MinimapJson.SerializeWaypoints(SnapshotRows(_rows));
            if (_writeAtomic(path, json))
            {
                _dirty = false;
                _logInfo?.Invoke($"Saved {_rows.Count} waypoints to {path}.");
                return true;
            }

            // Failed write: keep the data dirty so the next Save retries the SAME file.
            _logError?.Invoke($"Waypoint save failed at {path}; will retry.");
            return false;
        }
        catch (Exception ex)
        {
            _logError?.Invoke($"Waypoint save failed: {ex.Message}");
            return false;
        }
    }

    public void Load()
    {
        try
        {
            SyncSlot();
            if (!_activeKnown) return;
            string path = _pathForSlot(_slot);
            for (int i = 0; i < _pending.Count; i++)
            {
                if (!string.Equals(_pending[i].Path, path, StringComparison.OrdinalIgnoreCase)) continue;
                // Failed writes are newer than disk, including after A -> B -> A.
                List<WaypointFile> pendingRows = _pending[i].Rows;
                _pending.RemoveAt(i);
                _rows.Clear();
                _rows.AddRange(SnapshotRows(pendingRows));
                _rowsSlot = _slot;
                _dirty = true;
                _faulted = false;
                _revision++;
                return;
            }

            if (_dirty && string.Equals(_rowsSlot, _slot, StringComparison.Ordinal))
            {
                // Unsaved changes for this very slot: never discard them by reloading
                // (possibly stale) disk data over the top.
                _logWarn?.Invoke("Waypoint reload skipped: unsaved changes for this slot are still pending.");
                return;
            }

            _rows.Clear();
            _rowsSlot = _slot;
            _dirty = false;
            _faulted = false;
            _revision++;

            List<WaypointFile>? loaded = LoadValidated(path, out bool faulted);
            if (loaded != null)
            {
                for (int i = 0; i < loaded.Count; i++)
                {
                    WaypointFile row = loaded[i];
                    if (row == null) continue;
                    AddCore(row.Name, row.X, row.Y, row.Z, row.ColorHex, markDirty: false);
                }
                _rowsSlot = _slot;
                _dirty = false;
                _faulted = faulted;
                _logInfo?.Invoke($"Loaded {_rows.Count} waypoints for slot '{_slot}'.");
            }
            else if (faulted)
            {
                _faulted = true;
                _logWarn?.Invoke("Waypoint file could not be validated; store is read-only until it is cleared or repaired.");
            }
            else
            {
                _logInfo?.Invoke($"No waypoint file for slot '{_slot}' (fresh start).");
            }
            _revision++;
        }
        catch (Exception ex)
        {
            _faulted = true;
            _logError?.Invoke($"Waypoint load failed: {ex.Message}");
        }
    }

    public void FlushIfDirty()
    {
        if (_dirty || _pending.Count > 0) Save();
    }

    // ────────────────────────────────────────────────────────────
    // Slot context
    // ────────────────────────────────────────────────────────────

    private bool EnsureContext(bool allowFault = false)
    {
        try
        {
            SyncSlot();
            if (!_activeKnown) return false;
            if (!string.Equals(_rowsSlot, _slot, StringComparison.Ordinal)) Load();
            return string.Equals(_rowsSlot, _slot, StringComparison.Ordinal) && (allowFault || !_faulted);
        }
        catch (Exception ex)
        {
            _faulted = true;
            _logError?.Invoke($"Waypoint context unavailable: {ex.Message}");
            return false;
        }
    }

    private void SyncSlot()
    {
        TryFlushPending();
        string? current;
        try { current = _activeSlot(); }
        catch { current = null; }
        _activeKnown = current != null && current.StartsWith("slot_", StringComparison.Ordinal)
            && int.TryParse(current.AsSpan(5), System.Globalization.NumberStyles.None,
                System.Globalization.CultureInfo.InvariantCulture, out int number) && number >= 0;
        string target = _activeKnown ? current! : UnresolvedSlot;
        if (string.Equals(target, _slot, StringComparison.Ordinal)
            && (_rowsSlot.Length == 0 || string.Equals(_rowsSlot, target, StringComparison.Ordinal))) return;

        // Bind the new view first. On retry-budget failure old rows stay retained but hidden.
        if (!string.Equals(_slot, target, StringComparison.Ordinal)) _revision++;
        _slot = target;
        if (_dirty && _rowsSlot.Length > 0 && !string.Equals(_rowsSlot, target, StringComparison.Ordinal))
        {
            FreezePending(_rows, _pathForSlot(_rowsSlot));
            TryFlushPending();
        }
        if (!string.Equals(_rowsSlot, target, StringComparison.Ordinal))
        {
            _rows.Clear();
            _rowsSlot = string.Empty;
            _dirty = false;
            _faulted = false;
            _revision++;
        }
    }

    private void FreezePending(List<WaypointFile> rows, string path)
    {
        for (int i = 0; i < _pending.Count; i++)
        {
            if (string.Equals(_pending[i].Path, path, StringComparison.OrdinalIgnoreCase))
            {
                _pending[i] = (SnapshotRows(rows), path);
                return;
            }
        }
        if (_pending.Count >= MaxPendingWrites)
            throw new InvalidOperationException("Waypoint retry budget exhausted; edits are blocked until pending IO succeeds.");
        _pending.Add((SnapshotRows(rows), path));
    }

    private bool TryFlushPending()
    {
        bool allSucceeded = true;
        for (int i = _pending.Count - 1; i >= 0; i--)
        {
            (List<WaypointFile> rows, string path) = _pending[i];
            bool ok;
            try
            {
                ok = _writeAtomic(path, MinimapJson.SerializeWaypoints(rows));
            }
            catch (Exception ex)
            {
                _logError?.Invoke($"Waypoint flush failed at {path}: {ex.Message}");
                ok = false;
            }

            if (ok)
            {
                _pending.RemoveAt(i);
                _logInfo?.Invoke($"Flushed pending waypoints to {path}.");
            }
            else
            {
                allSucceeded = false;
                _logError?.Invoke($"Waypoint flush failed at {path}; will retry the same file.");
            }
        }
        return allSucceeded;
    }

    // ────────────────────────────────────────────────────────────
    // Load validation (D3)
    // ────────────────────────────────────────────────────────────

    private List<WaypointFile>? LoadValidated(string path, out bool faulted)
    {
        faulted = false;
        string backupPath = path + ".bak";
        bool primaryExists = _fileExists(path);
        bool backupExists = _fileExists(backupPath);

        if (primaryExists)
        {
            string? text = TryRead(path);
            if (text != null && TryParseRows(text, out List<WaypointFile> primaryRows))
                return primaryRows;
            _logWarn?.Invoke($"Waypoint file '{path}' is invalid or unreadable; trying the backup.");
        }

        if (backupExists)
        {
            string? text = TryRead(backupPath);
            if (text != null && TryParseRows(text, out List<WaypointFile> backupRows))
            {
                _logWarn?.Invoke($"Recovered waypoints from backup '{backupPath}'.");
                // Do not rotate a known-corrupt primary over the only good backup.
                faulted = primaryExists;
                return backupRows;
            }
            _logWarn?.Invoke($"Waypoint backup '{backupPath}' is invalid or unreadable.");
        }

        // Nothing validated. Preserve whatever is on disk instead of fabricating {} data.
        faulted = primaryExists || backupExists;
        return null;
    }

    private string? TryRead(string path)
    {
        try { return _readText(path); }
        catch (Exception ex)
        {
            _logWarn?.Invoke($"Waypoint read failed at '{path}': {ex.Message}");
            return null;
        }
    }

    private static bool TryParseRows(string text, out List<WaypointFile> rows)
    {
        rows = new List<WaypointFile>();
        if (string.IsNullOrWhiteSpace(text) || text.Length > MaxJsonLength) return false;

        List<WaypointFile>? parsed;
        try { parsed = MinimapJson.DeserializeWaypoints(text); }
        catch { return false; }

        if (parsed == null) return false; // "null" or a non-array root
        if (parsed.Count > 0 && !parsed.Exists(row => row != null && NormalizeName(row.Name) != null
            && IsFinite(row.X) && IsFinite(row.Y) && IsFinite(row.Z))) return false;
        rows = parsed;
        return true;
    }

    // ────────────────────────────────────────────────────────────
    // Normalization / helpers
    // ────────────────────────────────────────────────────────────

    private bool AddCore(string? name, float x, float y, float z, string? colorHex, bool markDirty)
    {
        string? clean = NormalizeName(name);
        if (clean == null) return false;
        if (!IsFinite(x) || !IsFinite(y) || !IsFinite(z)) return false;
        string color = NormalizeColorHex(colorHex);

        // Replace a same-name (trimmed) waypoint instead of duplicating it.
        for (int i = 0; i < _rows.Count; i++)
        {
            if (string.Equals(_rows[i].Name, clean, StringComparison.OrdinalIgnoreCase))
            {
                _rows[i].X = x;
                _rows[i].Y = y;
                _rows[i].Z = z;
                _rows[i].ColorHex = color;
                if (markDirty) MarkDirty();
                return true;
            }
        }

        if (_rows.Count >= MaxWaypoints)
        {
            _logWarn?.Invoke($"Waypoint limit ({MaxWaypoints}) reached — delete one first.");
            return false;
        }

        _rows.Add(new WaypointFile { Name = clean, X = x, Y = y, Z = z, ColorHex = color });
        if (markDirty) MarkDirty();
        return true;
    }

    private void MarkDirty()
    {
        _rowsSlot = _slot;
        _dirty = true;
        _revision++;
    }

    private static List<WaypointFile> SnapshotRows(List<WaypointFile> rows)
    {
        var copy = new List<WaypointFile>(rows.Count);
        for (int i = 0; i < rows.Count; i++)
        {
            WaypointFile row = rows[i];
            copy.Add(new WaypointFile { Name = row.Name, X = row.X, Y = row.Y, Z = row.Z, ColorHex = row.ColorHex });
        }
        return copy;
    }

    public static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

    public static string? NormalizeName(string? raw)
    {
        if (raw == null) return null;
        string trimmed = raw.Trim();
        if (trimmed.Length == 0 || trimmed.Length > MaxNameLength) return null;
        // Reject Unity rich-text markup instead of letting it reach the HUD label / console.
        if (trimmed.IndexOf('<') >= 0 || trimmed.IndexOf('>') >= 0) return null;
        for (int i = 0; i < trimmed.Length; i++)
            if (char.IsControl(trimmed[i])) return null;
        return trimmed;
    }

    /// <summary>
    /// Canonicalizes a colour to a single spelling (<c>#RRGGBB</c>, or <c>#RRGGBBAA</c> when
    /// an alpha byte is present). Accepts 3/6/8 hex digits with or without a leading '#';
    /// anything else (named colours, typos, empty) falls back to the default safely.
    /// </summary>
    public static string NormalizeColorHex(string? raw)
    {
        if (raw == null) return DefaultColorHex;
        string value = raw.Trim();
        if (value.StartsWith("#", StringComparison.Ordinal)) value = value.Substring(1);

        if (value.Length == 3 && IsHex(value))
            return ("#" + new string(new[] { value[0], value[0], value[1], value[1], value[2], value[2] })).ToUpperInvariant();

        if ((value.Length == 6 || value.Length == 8) && IsHex(value))
            return ("#" + value).ToUpperInvariant();

        return DefaultColorHex;
    }

    private static bool IsHex(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            bool isHex = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
            if (!isHex) return false;
        }
        return true;
    }
}

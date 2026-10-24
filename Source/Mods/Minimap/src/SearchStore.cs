using System;
using System.Collections.Generic;

namespace Minimap;

/// <summary>
/// Unity-free persistence for the optional sewer-key search intent. It stores exactly the
/// <see cref="SearchRecord"/> values the controller retains: world, target, area id/version and
/// the accepted flag. There is deliberately no completed/found field - completion stays a world
/// observation, never a second authority.
///
/// Contract:
///  - The file is versioned. A document with an unknown version, an unknown shape, an invalid
///    row, a duplicate world or more rows than the cap is <b>rejected</b>; nothing is migrated,
///    truncated or invented.
///  - A rejected primary is never overwritten. The automatic <c>.bak</c> is tried once; if it
///    validates, its rows are served read-only (a corrupt primary must not rotate over the only
///    good backup). An explicit <see cref="Clear"/> is the user's recovery and re-enables saving.
///  - A failed atomic write keeps the store dirty so the next <see cref="Save"/> retries the same
///    path; success is reported only when the write returned true.
///  - Rows are keyed by world identity (ordinal), newest first, bounded by
///    <see cref="MaxRecords"/>. A world replaced inside the same save slot therefore never
///    matches an older record, because the identity string is the key.
///
/// The production runtime binding (how the adapter derives that identity) is still unimplemented;
/// this type is prepared domain code and is not wired into the mod yet.
/// </summary>
public sealed class SearchStore
{
    public const int FormatVersion = 1;
    public const int MaxRecords = 16;
    public const int MaxJsonLength = 65536;
    public const int MaxWorldLength = 128;
    public const int MaxTargetLength = 128;
    public const int MaxAreaIdLength = 64;

    private readonly string _path;
    private readonly Func<string, string, bool> _writeAtomic;
    private readonly Func<string, bool> _fileExists;
    private readonly Func<string, string?> _readText;
    private readonly Action<string>? _logInfo;
    private readonly Action<string>? _logWarn;
    private readonly Action<string>? _logError;

    private readonly List<SearchRecord> _records = new();
    private bool _faulted;
    private bool _dirty;
    private int _revision;

    public SearchStore(
        string path,
        Func<string, string, bool> writeAtomic,
        Func<string, bool> fileExists,
        Func<string, string?> readText,
        Action<string>? logInfo = null,
        Action<string>? logWarn = null,
        Action<string>? logError = null)
    {
        _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentException("path must not be blank", nameof(path)) : path;
        _writeAtomic = writeAtomic ?? throw new ArgumentNullException(nameof(writeAtomic));
        _fileExists = fileExists ?? throw new ArgumentNullException(nameof(fileExists));
        _readText = readText ?? throw new ArgumentNullException(nameof(readText));
        _logInfo = logInfo;
        _logWarn = logWarn;
        _logError = logError;
    }

    public string Path => _path;

    /// <summary>Rows held in memory, newest first.</summary>
    public IReadOnlyList<SearchRecord> Records => _records;

    public int Count => _records.Count;

    /// <summary>Bumped by every load/clear that replaces the in-memory rows.</summary>
    public int Revision => _revision;

    /// <summary>True when the on-disk data could not be validated; saving stays blocked.</summary>
    public bool IsFaulted => _faulted;

    /// <summary>True when memory differs from the last successful write.</summary>
    public bool IsDirty => _dirty;

    // ────────────────────────────────────────────────────────────
    // Lookup / mutation
    // ────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the stored record for <paramref name="world"/>, or null when the world is blank or
    /// unknown. An unreadable store never answers from fabricated data.
    /// </summary>
    public SearchRecord? Find(string? world)
    {
        string? cleanWorld = NormalizeKey(world, MaxWorldLength);
        if (cleanWorld == null)
            return null;

        for (int i = 0; i < _records.Count; i++)
        {
            if (string.Equals(_records[i].World, cleanWorld, StringComparison.Ordinal))
                return _records[i];
        }
        return null;
    }

    /// <summary>
    /// Stores (or replaces) the intent for its world. Rejects every record that could not be
    /// restored later: blank/oversized world, target or area id, or a negative area version.
    /// </summary>
    public bool Remember(SearchRecord? record)
    {
        if (record == null)
            return false;

        SearchRecord? clean = NormalizeRecord(record);
        if (clean == null)
        {
            _logWarn?.Invoke("Search record rejected: world, target and area id must be non-blank and within the length limits.");
            return false;
        }

        for (int i = 0; i < _records.Count; i++)
        {
            if (!string.Equals(_records[i].World, clean.World, StringComparison.Ordinal))
                continue;

            if (Same(clean, _records[i]))
                return true;

            _records.RemoveAt(i);
            break;
        }

        _records.Insert(0, clean);
        if (_records.Count > MaxRecords)
            _records.RemoveRange(MaxRecords, _records.Count - MaxRecords);
        _dirty = true;
        return true;
    }

    /// <summary>Drops the stored intent for one world. Returns true when a row was removed.</summary>
    public bool Forget(string? world)
    {
        string? cleanWorld = NormalizeKey(world, MaxWorldLength);
        if (cleanWorld == null)
            return false;

        for (int i = 0; i < _records.Count; i++)
        {
            if (!string.Equals(_records[i].World, cleanWorld, StringComparison.Ordinal))
                continue;

            _records.RemoveAt(i);
            _dirty = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Explicit user recovery: clears the rows and the fault, and marks the store dirty so the
    /// next save writes a valid empty document.
    /// </summary>
    public bool Clear()
    {
        bool wasFaulted = _faulted;
        _faulted = false;
        if (_records.Count == 0 && !wasFaulted)
        {
            _dirty = true;
            return true;
        }

        _records.Clear();
        _dirty = true;
        _revision++;
        return true;
    }

    // ────────────────────────────────────────────────────────────
    // Persistence
    // ────────────────────────────────────────────────────────────

    public bool Save()
    {
        try
        {
            if (_faulted)
            {
                _logWarn?.Invoke($"Search save skipped: '{_path}' could not be validated; clear or repair it first.");
                return false;
            }

            if (!_dirty)
                return true;

            string json = MinimapJson.SerializeSearch(new SearchFile
            {
                Version = FormatVersion,
                Records = ToRows(_records)
            });

            if (json.Length > MaxJsonLength)
            {
                _logError?.Invoke($"Search save aborted: the document would be {json.Length} characters (limit {MaxJsonLength}).");
                return false;
            }

            if (_writeAtomic(_path, json))
            {
                _dirty = false;
                _logInfo?.Invoke($"Saved {_records.Count} search record(s) to {_path}.");
                return true;
            }

            _logError?.Invoke($"Search save failed at {_path}; will retry.");
            return false;
        }
        catch (Exception ex)
        {
            _logError?.Invoke($"Search save failed: {ex.Message}");
            return false;
        }
    }

    public void FlushIfDirty()
    {
        if (_dirty)
            Save();
    }

    public void Load()
    {
        try
        {
            _records.Clear();
            _dirty = false;
            _faulted = false;
            _revision++;

            List<SearchRecord>? loaded = LoadValidated(_path, out bool faulted);
            if (loaded != null)
            {
                _records.AddRange(loaded);
                _faulted = faulted;
                _logInfo?.Invoke($"Loaded {_records.Count} search record(s) from {_path}.");
            }
            else if (faulted)
            {
                _faulted = true;
                _logWarn?.Invoke($"Search file '{_path}' could not be validated; the store is read-only until it is cleared or repaired.");
            }
            else
            {
                _logInfo?.Invoke($"No search file at {_path} (fresh start).");
            }
        }
        catch (Exception ex)
        {
            _faulted = true;
            _logError?.Invoke($"Search load failed: {ex.Message}");
        }
    }

    private List<SearchRecord>? LoadValidated(string path, out bool faulted)
    {
        faulted = false;
        string backupPath = path + ".bak";
        bool primaryExists = Exists(path);
        bool backupExists = Exists(backupPath);

        if (primaryExists)
        {
            string? text = TryRead(path);
            if (text != null && TryParse(text, out List<SearchRecord> primaryRows))
                return primaryRows;
            _logWarn?.Invoke($"Search file '{path}' is invalid or unreadable; trying the backup.");
        }

        if (backupExists)
        {
            string? text = TryRead(backupPath);
            if (text != null && TryParse(text, out List<SearchRecord> backupRows))
            {
                _logWarn?.Invoke($"Recovered search records from backup '{backupPath}'.");
                // Never rotate a known-corrupt primary over the only good backup.
                faulted = primaryExists;
                return backupRows;
            }
            _logWarn?.Invoke($"Search backup '{backupPath}' is invalid or unreadable.");
        }

        // Nothing validated: preserve whatever is on disk instead of fabricating empty data.
        faulted = primaryExists || backupExists;
        return null;
    }

    private bool Exists(string path)
    {
        try { return _fileExists(path); }
        catch (Exception ex)
        {
            _logWarn?.Invoke($"Search existence probe failed at '{path}': {ex.Message}");
            return false;
        }
    }

    private string? TryRead(string path)
    {
        try { return _readText(path); }
        catch (Exception ex)
        {
            _logWarn?.Invoke($"Search read failed at '{path}': {ex.Message}");
            return null;
        }
    }

    private static bool TryParse(string text, out List<SearchRecord> rows)
    {
        rows = new List<SearchRecord>();
        if (string.IsNullOrWhiteSpace(text) || text.Length > MaxJsonLength)
            return false;

        SearchFile? file;
        try { file = MinimapJson.DeserializeSearch(text); }
        catch { return false; }

        if (file == null || file.Version != FormatVersion || file.Records == null)
            return false;
        if (file.Records.Count > MaxRecords)
            return false;

        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (int i = 0; i < file.Records.Count; i++)
        {
            SearchRow? row = file.Records[i];
            if (row == null)
                return false;

            SearchRecord? record = NormalizeRecord(
                new SearchRecord(row.World, row.Target, row.AreaId, row.AreaVersion, row.Accepted));
            if (record == null)
                return false;
            if (!seen.Add(record.World))
                return false;

            rows.Add(record);
        }
        return true;
    }

    // ────────────────────────────────────────────────────────────
    // Normalization
    // ────────────────────────────────────────────────────────────

    private static SearchRecord? NormalizeRecord(SearchRecord record)
    {
        string? world = NormalizeKey(record.World, MaxWorldLength);
        string? target = NormalizeKey(record.Target, MaxTargetLength);
        string? areaId = NormalizeKey(record.AreaId, MaxAreaIdLength);
        if (world == null || target == null || areaId == null || record.AreaVersion < 0)
            return null;

        return new SearchRecord(world, target, areaId, record.AreaVersion, record.Accepted);
    }

    private static string? NormalizeKey(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        string trimmed = value.Trim();
        return trimmed.Length > maxLength ? null : trimmed;
    }

    private static bool Same(SearchRecord left, SearchRecord right)
        => string.Equals(left.World, right.World, StringComparison.Ordinal)
           && string.Equals(left.Target, right.Target, StringComparison.Ordinal)
           && string.Equals(left.AreaId, right.AreaId, StringComparison.Ordinal)
           && left.AreaVersion == right.AreaVersion
           && left.Accepted == right.Accepted;

    private static List<SearchRow> ToRows(List<SearchRecord> records)
    {
        var rows = new List<SearchRow>(records.Count);
        for (int i = 0; i < records.Count; i++)
        {
            SearchRecord record = records[i];
            rows.Add(new SearchRow
            {
                World = record.World,
                Target = record.Target,
                AreaId = record.AreaId,
                AreaVersion = record.AreaVersion,
                Accepted = record.Accepted
            });
        }
        return rows;
    }
}

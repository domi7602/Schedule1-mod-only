using System;
using System.IO;
using LightControl.Core;
using S1Mods.Shared;

namespace LightControl;

/// <summary>
/// Slot-scoped disk adapter for the pure JSON data API.
///
/// Safety contract:
/// <list type="bullet">
/// <item>The file path is derived from the active slot number captured at load time only; a
/// negative slot never yields a path and never authorizes a write.</item>
/// <item>Load decisions use read outcomes, not existence booleans. <c>File.Exists</c> reports
/// false for unreadable or unreachable paths, so it cannot prove absence; a missing file is
/// only "confirmed missing" when its parent directory was actually listed.</item>
/// <item>A readable main file with an unknown schema or a foreign save id blocks the write gate
/// even when a valid backup exists: those bytes are intact data that must be retained.</item>
/// <item>A main file that was read but failed lamp-data validation may fall back to the backup,
/// which is validated independently in full (schema, save id, every lamp) before use.</item>
/// <item>Load never repairs, renames or rewrites a file on disk.</item>
/// <item>A session recovered from the backup writes the new main file without rotating the
/// validated backup, because that rotation would copy the corrupt main text over the one good
/// backup. The corrupt main is left untouched on disk until such a save happens.</item>
/// <item>Every save path, including the native slot probe and serialization, is wrapped so an
/// exception can never escape into the game loop.</item>
/// </list>
/// </summary>
internal sealed class LightPersistence
{
    private const string ModName = "LightControl";

    private readonly int _slot;
    private readonly string _saveId;
    private readonly string _path;
    private readonly ModLogger _log;
    private bool _preserveBackup;

    public LightPersistence(int slot, string saveId, ModLogger log)
    {
        _slot = slot;
        _saveId = saveId;
        _log = log;
        _path = SafeStorage.GetUserDataPath(ModName, $"lamps_{saveId}.json");
    }

    /// <summary>
    /// The exact outcome of the last <see cref="Load"/>, including the error category and a human
    /// readable detail. The bool return value stays the primary API for callers.
    /// </summary>
    public PersistenceLoadResult LastLoadResult { get; private set; } =
        new(false, PersistenceError.None, "Lamp data has not been loaded yet.");

    /// <summary>
    /// True while this session's data came from the backup and no save has succeeded since, so the
    /// next save must leave the validated backup in place.
    /// </summary>
    public bool LoadedFromBackup => _preserveBackup;

    /// <summary>Loads the captured slot's lamp data, distinguishing missing from unreadable.</summary>
    public bool Load(JsonStatePersistence persistence)
    {
        if (persistence is null) throw new ArgumentNullException(nameof(persistence));
        try
        {
            var main = ReadOutcome(_path, "The lamp file");
            var backup = PersistenceReadOutcome.NotConsulted;

            // The backup is only read when it could legitimately be used: the main file is
            // provably absent, or it was readable but is not usable lamp data. An unknown schema,
            // a foreign save id or an unreadable main never consults the backup at all.
            if (main.State == PersistenceReadState.Absent)
                backup = ReadOutcome(_path + ".bak", "The lamp backup");
            else if (main.State == PersistenceReadState.Present)
            {
                var status = JsonStatePersistence.Inspect(main.Text ?? string.Empty, _saveId).Status;
                if (status is PersistenceDataStatus.CorruptJson or PersistenceDataStatus.InvalidState)
                    backup = ReadOutcome(_path + ".bak", "The lamp backup");
            }

            var plan = PersistenceSlotPolicy.PlanLoad(main, backup, json => JsonStatePersistence.Inspect(json, _saveId));
            PersistenceLoadResult result;
            switch (plan.Action)
            {
                case PersistenceLoadAction.ConfirmMissing:
                    result = persistence.ConfirmDataMissing();
                    break;
                case PersistenceLoadAction.LoadJson:
                    // The JSON API re-validates against this instance's session and save id.
                    result = persistence.LoadJson(plan.Json!);
                    break;
                default:
                    result = persistence.ReportLoadFailure(plan.Error, plan.Detail);
                    break;
            }

            // Keep the recovery reason in the observable result; the JSON API's generic success
            // detail would otherwise hide that these values came from the backup.
            if (result.Success && plan.Source == PersistenceSource.Backup)
                result = result with { Detail = plan.Detail };
            LastLoadResult = result;
            _preserveBackup |= plan.Source == PersistenceSource.Backup && result.Success;
            if (_preserveBackup)
                _log.Warn("Lamp data was recovered from the validated backup. The backup is kept intact; no file was repaired during load.");
            else if (!result.Success)
                _log.Warn($"Lamp data could not be loaded ({result.Error}): {result.Detail}");
            return result.Success;
        }
        catch (Exception exception)
        {
            // Defensive: a read/parse problem is a blocked write gate, never a crash.
            var result = persistence.ReportLoadFailure(PersistenceError.InvalidState,
                $"Lamp data could not be loaded safely ({exception.GetType().Name}). Writes stay blocked and existing files are kept.");
            LastLoadResult = result;
            // A prior validated recovery still needs its protected first save after a failed retry.
            _log.Warn("Lamp data could not be loaded safely; writes stay blocked and existing files are kept", exception);
            return false;
        }
    }

    /// <summary>
    /// Writes the complete JSON atomically. Refuses when the game reports a different active slot
    /// (a stale slot must never be written), when the write gate is not ready, or on any I/O error.
    /// </summary>
    public bool Save(JsonStatePersistence persistence)
    {
        if (persistence is null) throw new ArgumentNullException(nameof(persistence));
        try
        {
            int active = SaveSlots.GetActiveSlotNumber();
            // Strict guard: the captured slot must always match the active slot, including when
            // the game reports no active slot (-1). A negative slot never authorizes a write.
            if (!PersistenceSlotPolicy.CanWrite(_slot, active))
            {
                _log.Warn($"Active save slot {active} no longer matches the captured slot {_slot}; lamp data was not written.");
                return false;
            }

            string json;
            try { json = persistence.CreateJsonForSave(); }
            catch (Exception ex) { _log.Warn("Lamp data could not be serialized; existing storage is kept", ex); return false; }

            return _preserveBackup
                ? PreserveBackup(json)
                : SafeStorage.SaveTextAtomic(_path, json, _log);
        }
        catch (Exception exception)
        {
            _log.Warn("Lamp data could not be written; existing storage is retained", exception);
            return false;
        }
    }

    /// <summary>
    /// Writes the main file without the normal .bak rotation. Used after a backup recovery: the
    /// current main file is corrupt, so the shared atomic writer would copy that corruption over
    /// the one good backup. The main file is still replaced through a temp file, and any failure
    /// keeps every existing file and returns false.
    /// </summary>
    private bool PreserveBackup(string json)
    {
        string temp = _path + ".tmp";
        try
        {
            string? directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.WriteAllText(temp, json);
            File.Move(temp, _path, true);
            _preserveBackup = false;
            _log.Info("Lamp data was written; the validated backup was kept intact.");
            return true;
        }
        catch (Exception exception)
        {
            try { File.Delete(temp); } catch { /* best-effort cleanup only */ }
            _log.Warn("Lamp data could not be written; the recovered data and the backup are kept", exception);
            return false;
        }
    }

    /// <summary>
    /// Reads a file and proves its absence when, and only when, the parent directory was actually
    /// listed. Any other failure is reported as unreadable so it can never be mistaken for
    /// "missing data" and replaced with empty state.
    /// </summary>
    private static PersistenceReadOutcome ReadOutcome(string path, string label)
    {
        try
        {
            return PersistenceReadOutcome.Present(File.ReadAllText(path));
        }
        catch (FileNotFoundException)
        {
            return ProveAbsence(path, label);
        }
        catch (DirectoryNotFoundException)
        {
            return ProveAbsence(path, label);
        }
        catch (Exception exception)
        {
            return PersistenceReadOutcome.Unreadable(
                $"{label} could not be read ({exception.GetType().Name}: {exception.Message}).");
        }
    }

    /// <summary>
    /// Absence requires a successfully listed ancestor and an absent child at that boundary.
    /// DirectoryNotFound alone is not proof: an unavailable drive also throws it.
    /// </summary>
    private static PersistenceReadOutcome ProveAbsence(string path, string label)
    {
        string? directory = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(directory))
            return PersistenceReadOutcome.Unreadable(
                $"{label} was not found, but its parent directory is unknown, so absence is not proven.");
        string missingChild = Path.GetFullPath(path);
        while (!string.IsNullOrEmpty(directory))
        {
            try
            {
                foreach (string entry in Directory.EnumerateFileSystemEntries(directory))
                    if (string.Equals(Path.GetFullPath(entry), missingChild, StringComparison.OrdinalIgnoreCase))
                        return PersistenceReadOutcome.Unreadable($"{label} or its parent appeared during the read; absence is not proven.");
                return PersistenceReadOutcome.Absent($"{label} is absent beneath a successfully listed storage ancestor.");
            }
            catch (DirectoryNotFoundException)
            {
                missingChild = Path.GetFullPath(directory);
                directory = Path.GetDirectoryName(Path.TrimEndingDirectorySeparator(missingChild));
            }
            catch (Exception exception)
            {
                return PersistenceReadOutcome.Unreadable(
                    $"{label} could not be located safely ({exception.GetType().Name}: {exception.Message}); absence is not proven.");
            }
        }
        return PersistenceReadOutcome.Unreadable($"{label} storage root is unreachable; absence is not proven.");
    }
}

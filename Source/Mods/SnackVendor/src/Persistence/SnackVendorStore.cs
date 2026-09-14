using System;
using System.Collections.Generic;
using System.IO;
using S1Mods.Shared;

namespace SnackVendor.Persistence;

/// <summary>
/// Sidecar-Persistenz für SnackVendor-Stationen (Stock pro Maschine).
/// Pattern: AutoPackStore — slot-spezifische JSON-Datei
/// (`snacks_slot_{n}.json`), ein Slot-Sentinel (`-1`) für nicht
/// aufgelöste Saves, atomic write (.tmp + rename) mit optionalem
/// Backup. Save-Slot muss IMMER vor SaveData-Zugriffen aufgelöst
/// sein (sonst Slot-Sentinel-Hit, der ein Konsolen-Warn loggt
/// statt unbemerkt auf _default zu schreiben — siehe
/// game-mod-persistence Skill, Abschnitt "Save-slot isolation").
/// </summary>
public static class SnackVendorStore
{
    private static readonly ModLogger Log = new("SnackVendor");
    private const string DirName = "SnackVendor";
    private const string FilePattern = "snacks_slot_{0}.json";
    private const string BackupSuffix = ".bak";
    public const int UnknownSlot = -1;

    // Mirrors S1API's pattern: a single SafeStorage helper would be nicer,
    // but the only vanilla-Schedule-I-aware call path for slot resolution
    // is via SaveInfo events. Controller fills _currentSlot when the
    // game fires the relevant lifecycle event. Until then: unknown.
    private static int _currentSlot = UnknownSlot;

    /// <summary>Called by controller when the game reports a save slot.</summary>
    public static void OnSaveSlotResolved(int slot)
    {
        if (slot < 0)
        {
            Log.Warn($"OnSaveSlotResolved called with negative slot {slot} (kept as UnknownSlot).");
            return;
        }
        _currentSlot = slot;
    }

    public static void OnSaveSlotReset()
    {
        _currentSlot = UnknownSlot;
    }

    public static bool IsSlotResolved => _currentSlot >= 0;

    /// <summary>Atomically writes the file for the currently-resolved slot.</summary>
    public static bool Save(SnackVendorSaveFile file)
    {
        if (!IsSlotResolved)
        {
            Log.Warn("Save called without resolved slot — dropping (would write to _default sentinel).");
            return false;
        }

        var path = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, DirName, string.Format(FilePattern, _currentSlot));
        var dir = Path.GetDirectoryName(path);
        if (dir != null) Directory.CreateDirectory(dir);

        // Backup last good copy (crash-window insurance from game-mod-persistence skill).
        if (File.Exists(path))
        {
            try { File.Copy(path, path + BackupSuffix, overwrite: true); }
            catch (Exception ex) { Log.Warn($"backup failed for {path}", ex); }
        }

        var tmp = path + ".tmp";
        try
        {
            // SafeStorage.SaveAtomic writes JSON via its default options
            // (public properties, no converters needed for our DTOs).
            var wrote = SafeStorage.SaveAtomic(path, file, Log);
            if (wrote && File.Exists(tmp))
            {
                // Belt + suspenders: clean up half-written tmp if SaveAtomic
                // left it behind (default impl moves it, but defensive).
                try { File.Delete(tmp); } catch { /* swallow */ }
            }
            return wrote;
        }
        catch (Exception ex)
        {
            Log.Error($"save failed for {path}", ex);
            try { if (File.Exists(tmp)) File.Delete(tmp); } catch { /* swallow */ }
            return false;
        }
    }

    /// <summary>Loads the file for the currently-resolved slot. Missing file -> empty payload.</summary>
    public static SnackVendorSaveFile Load()
    {
        if (!IsSlotResolved)
        {
            Log.Warn($"Load called without resolved slot — returning empty (would read from _default sentinel).");
            return new SnackVendorSaveFile();
        }

        var path = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, DirName, string.Format(FilePattern, _currentSlot));
        if (!File.Exists(path)) return new SnackVendorSaveFile();
        try
        {
            var data = SafeStorage.LoadSafe<SnackVendorSaveFile>(path, new SnackVendorSaveFile(), Log);
            return data ?? new SnackVendorSaveFile();
        }
        catch (Exception ex)
        {
            Log.Error($"load failed for {path}, returning empty", ex);
            return new SnackVendorSaveFile();
        }
    }
}

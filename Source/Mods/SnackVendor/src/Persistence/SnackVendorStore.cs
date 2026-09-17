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
        // 2026-09-16 FIX: SafeStorage.SaveAtomic (-> SaveTextAtomic) macht den
        // .bak-Backup und das .tmp-Rename intern. Der frühere eigene
        // File.Copy-Backup + tmp-Aufräumcode hier war Redundanz: doppelter
        // Backup-Schreibvorgang pro Save bei jedem NPC-Kauf (Hot Path).
        return SafeStorage.SaveAtomic(path, file, Log);
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

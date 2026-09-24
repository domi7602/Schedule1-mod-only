using System;
using System.Collections.Generic;
using System.IO;
using S1Mods.Shared;

namespace SnackVendor.Persistence;

/// <summary>
/// Sidecar persistence for SnackVendor stations (stock per machine).
/// Pattern: AutoPackStore — slot-specific JSON file
/// (`snacks_slot_{n}.json`), a slot sentinel (`-1`) for unresolved
/// saves, atomic write (.tmp + rename) with optional
/// backup. Save slot must ALWAYS be resolved before SaveData accesses
/// (otherwise slot sentinel hit logs a console warning
/// instead of silently writing to _default — see
/// game-mod-persistence skill, section "Save-slot isolation").
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
        // 2026-09-16 FIX: SafeStorage.SaveAtomic (-> SaveTextAtomic) handles the
        // .bak backup and the .tmp rename internally. The previous own
        // File.Copy backup + tmp cleanup code here was redundancy: a duplicate
        // backup write per save on every NPC purchase (hot path).
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

using System;
using System.IO;
using System.Text.RegularExpressions;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;

namespace S1Mods.Shared;

/// <summary>
/// Raw result of the save-slot probe: slot number (>= 0, or -1 for
/// "no slot / main menu") and the save path if available.
/// </summary>
public readonly record struct SaveSlotInfo(int SlotNumber, string? SavePath);

/// <summary>
/// Single source of truth for "which save slot is currently active?"
/// (Consolidated 2026-09-15 from 10+ per-mod copies of the same
/// LoadManager probe pattern). Like NetworkGuard.Host.cs, references
/// game types (Il2CppScheduleOne) — signature drift surfaces at compile time.
/// Filename formats remain the responsibility of each mod: this type only
/// returns number/path/token, never finished suffixes.
/// </summary>
public static class SaveSlots
{
    private static readonly Regex DigitsRx = new(@"\d+", RegexOptions.Compiled);

    /// <summary>
    /// IL2CPP-safe probe on <c>LoadManager.ActiveSaveInfo</c>. Probes in
    /// order <c>PersistentSingleton&lt;LoadManager&gt;.Instance</c>,
    /// <c>LoadManager.Instance</c>, and <c>Singleton&lt;LoadManager&gt;.Instance</c>
    /// (the access paths historically used by various mods) and returns the
    /// first live hit. Returns null when no save is currently loaded or the
    /// interop layer throws (transitions/main menu).
    /// </summary>
    public static SaveSlotInfo? TryGetActiveSaveInfo()
    {
        try
        {
            var info = ReadInfo(PersistentSingleton<LoadManager>.Instance)
                    ?? ReadInfo(LoadManager.Instance)
                    ?? ReadInfo(Singleton<LoadManager>.Instance);
            if (info == null)
                return null;

            string? savePath = null;
            try { savePath = info.SavePath; } catch { }
            return new SaveSlotInfo(info.SaveSlotNumber, savePath);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Active slot number, or -1 when no save is loaded. Never throws;
    /// the >= 0 check prevents slot_-1 files (bug audit 2026-09-12,
    /// previously spread across 16 guard sites in 9 mods).
    /// </summary>
    public static int GetActiveSlotNumber()
    {
        var info = TryGetActiveSaveInfo();
        return info is { SlotNumber: >= 0 } value ? value.SlotNumber : -1;
    }

    /// <summary>
    /// Derives a slot token from a save path (legacy/file-based saves
    /// without a slot number): digits from the filename (without extension),
    /// else digits from the directory name, else the filename without extension.
    /// Null for an empty path. Prefixes ("slot_{...}") are appended by each mod.
    /// </summary>
    public static string? TryExtractSlotTokenFromSavePath(string? savePath)
    {
        if (string.IsNullOrWhiteSpace(savePath))
            return null;

        string file = Path.GetFileNameWithoutExtension(savePath) ?? string.Empty;
        var m = DigitsRx.Match(file);
        if (m.Success)
            return m.Value;

        string dir = Path.GetFileName(Path.GetDirectoryName(savePath) ?? string.Empty);
        var m2 = DigitsRx.Match(dir);
        if (m2.Success)
            return m2.Value;

        return file.Length > 0 ? file : null;
    }

    private static SaveInfo? ReadInfo(LoadManager? loadMgr)
    {
        if (loadMgr == null || loadMgr.Pointer == IntPtr.Zero || loadMgr.WasCollected)
            return null;

        var info = loadMgr.ActiveSaveInfo;
        if (info == null || info.Pointer == IntPtr.Zero || info.WasCollected)
            return null;

        return info;
    }
}

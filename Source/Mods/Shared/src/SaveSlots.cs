using System;
using System.IO;
using System.Text.RegularExpressions;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;

namespace S1Mods.Shared;

/// <summary>
/// Rohergebnis der Save-Slot-Sonde: Slot-Nummer (>= 0, oder -1 für
/// „kein Slot / Main Menu") und der Save-Pfad, falls verfügbar.
/// </summary>
public readonly record struct SaveSlotInfo(int SlotNumber, string? SavePath);

/// <summary>
/// Single Source of Truth für „welcher Save-Slot ist gerade aktiv?"
/// (Konsolidierung 2026-09-15 von ehemals 10+ per-Mod-Kopien desselben
/// LoadManager-Sonde-Musters). Referenziert wie NetworkGuard.Host.cs
/// Game-Types (Il2CppScheduleOne) — Signature-Drift fällt zur Compile-Zeit auf.
/// Die Formate der Dateinamen bleiben Sache der Mods: dieser Typ liefert nur
/// Nummer/Pfad/Token, nie fertige Suffixe.
/// </summary>
public static class SaveSlots
{
    private static readonly Regex DigitsRx = new(@"\d+", RegexOptions.Compiled);

    /// <summary>
    /// IL2CPP-sichere Sonde auf <c>LoadManager.ActiveSaveInfo</c>. Probt der
    /// Reihe nach <c>PersistentSingleton&lt;LoadManager&gt;.Instance</c>,
    /// <c>LoadManager.Instance</c> und <c>Singleton&lt;LoadManager&gt;.Instance</c>
    /// (die historisch in verschiedenen Mods genutzten Zugänge) und liefert den
    /// ersten lebigen Treffer. Null, wenn gerade kein Save geladen ist oder die
    /// Interop-Schicht wirft (Übergänge/Main Menu).
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
    /// Aktive Slot-Nummer, oder -1 wenn kein Save geladen ist. Wirft nie;
    /// der >= 0-Check verhindert slot_-1-Dateien (Bug-Audit 2026-09-12,
    /// vorher in 16 Guard-Stellen quer über 9 Mods verteilt).
    /// </summary>
    public static int GetActiveSlotNumber()
    {
        var info = TryGetActiveSaveInfo();
        return info is { SlotNumber: >= 0 } value ? value.SlotNumber : -1;
    }

    /// <summary>
    /// Leitet aus einem Save-Pfad einen Slot-Token ab (Legacy-/dateibasierte
    /// Saves ohne Slot-Nummer): Ziffern aus dem Dateinamen (ohne Extension),
    /// sonst Ziffern aus dem Ordnernamen, sonst der Dateiname ohne Extension.
    /// Null bei leerem Pfad. Präfixe ("slot_{...}") fügt die jeweilige Mod hinzu.
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

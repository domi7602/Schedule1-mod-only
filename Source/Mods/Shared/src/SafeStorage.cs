using System;
using System.IO;
using System.Text.Json;

namespace S1Mods.Shared;

/// <summary>
/// Resiliente I/O- und Persistenz-Hilfsklasse.
/// Schützt Savegames und Konfigurationen durch atomare Schreiboperationen (Temp-File -> Atomic Replace)
/// und automatisches Backup (.bak) vor Dateikorruption bei Spielabstürzen oder Patch-Änderungen.
/// </summary>
public static class SafeStorage
{
    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    /// <summary>
    /// Liefert den Standard-Pfad unter UserData/<ModName>/<FileName>.
    /// </summary>
    public static string GetUserDataPath(string modName, string fileName = "")
    {
        string baseDir = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, modName);
        if (string.IsNullOrEmpty(fileName))
            return baseDir;
        return Path.Combine(baseDir, fileName);
    }

    /// <summary>
    /// Stellt sicher, dass das angegebene Verzeichnis existiert.
    /// </summary>
    public static void EnsureDirectory(string dirPath)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(dirPath) && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"[SafeStorage] EnsureDirectory failed for '{dirPath}': {ex.Message}");
        }
    }

    /// <summary>
    /// Stellt sicher, dass das übergeordnete Verzeichnis einer Zieldatei existiert.
    /// </summary>
    public static void EnsureDirectoryForFile(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"[SafeStorage] EnsureDirectoryForFile failed for '{filePath}': {ex.Message}");
        }
    }

    /// <summary>
    /// Speichert ein Objekt als JSON atomar ab: Schreibt erst in eine .tmp-Datei,
    /// sichert den bestehenden Stand als .bak und ersetzt dann atomar die Zieldatei.
    /// </summary>
    public static bool SaveAtomic<T>(string filePath, T data, ModLogger? log = null, JsonSerializerOptions? options = null)
    {
        try
        {
            EnsureDirectoryForFile(filePath);
            string json = JsonSerializer.Serialize(data, options ?? DefaultJsonOptions);
            return SaveTextAtomic(filePath, json, log);
        }
        catch (Exception ex)
        {
            log?.Error($"SafeStorage: Serialisierungsfehler für '{Path.GetFileName(filePath)}': {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Speichert Text atomar ab mit .bak-Sicherung.
    /// </summary>
    public static bool SaveTextAtomic(string filePath, string content, ModLogger? log = null)
    {
        try
        {
            EnsureDirectoryForFile(filePath);
            string tempPath = filePath + ".tmp";
            string backupPath = filePath + ".bak";

            File.WriteAllText(tempPath, content);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Copy(filePath, backupPath, true);
                }
                catch (Exception ex)
                {
                    log?.Warn($"SafeStorage: Backup-Kopie '{Path.GetFileName(backupPath)}' fehlgeschlagen: {ex.Message}");
                }
            }

            File.Move(tempPath, filePath, true);
            return true;
        }
        catch (Exception ex)
        {
            log?.Error($"SafeStorage: Schreibfehler bei '{Path.GetFileName(filePath)}': {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Lädt ein JSON-Objekt sicher. Falls die Hauptdatei korrupt oder nicht lesbar ist,
    /// wird versucht, das automatische Backup (.bak) zu laden. Schlägt alles fehl, greift der Fallback.
    /// </summary>
    public static T LoadSafe<T>(string filePath, T fallback, ModLogger? log = null, JsonSerializerOptions? options = null) where T : class
    {
        if (!File.Exists(filePath))
        {
            string backupPath = filePath + ".bak";
            if (File.Exists(backupPath))
            {
                log?.Warn($"SafeStorage: Hauptdatei '{Path.GetFileName(filePath)}' fehlt, lade Backup '{Path.GetFileName(backupPath)}'.");
                return TryDeserialize(backupPath, fallback, log, options);
            }
            return fallback;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                return fallback;

            T? result = JsonSerializer.Deserialize<T>(json, options ?? DefaultJsonOptions);
            return result ?? fallback;
        }
        catch (Exception ex)
        {
            log?.Warn($"SafeStorage: Fehler beim Laden von '{Path.GetFileName(filePath)}' ({ex.Message}) — Versuche Backup-Wiederherstellung.");
            string backupPath = filePath + ".bak";
            if (File.Exists(backupPath))
                return TryDeserialize(backupPath, fallback, log, options);

            return fallback;
        }
    }

    /// <summary>
    /// Liest eine Textdatei sicher ein. Falls fehlend oder unlesbar, greift das .bak-Backup bzw. der Fallback.
    /// </summary>
    public static string LoadTextSafe(string filePath, string fallback = "", ModLogger? log = null)
    {
        if (!File.Exists(filePath))
        {
            string backupPath = filePath + ".bak";
            if (File.Exists(backupPath))
            {
                try { return File.ReadAllText(backupPath); }
                catch (Exception ex)
                {
                    log?.Warn($"SafeStorage: Backup-Lesefehler bei '{Path.GetFileName(backupPath)}': {ex.Message}");
                }
            }
            return fallback;
        }

        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            log?.Warn($"SafeStorage: Text-Lese-Fehler bei '{Path.GetFileName(filePath)}' ({ex.Message}) — Versuche Backup.");
            string backupPath = filePath + ".bak";
            if (File.Exists(backupPath))
            {
                try { return File.ReadAllText(backupPath); }
                catch (Exception exBak)
                {
                    log?.Error($"SafeStorage: Auch Backup-Lesefehler bei '{Path.GetFileName(backupPath)}': {exBak.Message}");
                }
            }
            return fallback;
        }
    }

    private static T TryDeserialize<T>(string path, T fallback, ModLogger? log, JsonSerializerOptions? options) where T : class
    {
        try
        {
            string json = File.ReadAllText(path);
            T? result = JsonSerializer.Deserialize<T>(json, options ?? DefaultJsonOptions);
            return result ?? fallback;
        }
        catch (Exception ex)
        {
            log?.Error($"SafeStorage: Auch Backup '{Path.GetFileName(path)}' unlesbar: {ex.Message}");
            return fallback;
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;

namespace S1Mods.Shared;

/// <summary>
/// Resilient I/O and persistence helper class.
/// Protects savegames and configurations through atomic write operations (temp file -> atomic replace)
/// and automatic backup (.bak) against file corruption from game crashes or patch changes.
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

    private static readonly ConcurrentDictionary<string, object> SaveLocks = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Lock key normalization: "a/b.json" and "a\b.json" (and casing variants) must
    /// share one lock, otherwise two writers take different locks for the same file.
    /// </summary>
    private static string NormalizeLockKey(string filePath)
    {
        try { return Path.GetFullPath(filePath); }
        catch { return filePath.Replace('/', '\\'); }
    }

    private static object GetLock(string filePath) => SaveLocks.GetOrAdd(NormalizeLockKey(filePath), _ => new object());

    /// <summary>
    /// Returns the default path under UserData/<ModName>/<FileName>.
    /// </summary>
    public static string GetUserDataPath(string modName, string fileName = "")
    {
        string baseDir = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, modName);
        if (string.IsNullOrEmpty(fileName))
            return baseDir;
        return Path.Combine(baseDir, fileName);
    }

    /// <summary>
    /// Ensures that the specified directory exists.
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
    /// Ensures that the parent directory of a target file exists.
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
    /// Atomically saves an object as JSON: first writes to a .tmp file,
    /// backs up the existing state as .bak, then atomically replaces the target file.
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
            log?.Error($"SafeStorage: Serialization error for '{Path.GetFileName(filePath)}': {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Atomically saves text with .bak backup.
    /// </summary>
    public static bool SaveTextAtomic(string filePath, string content, ModLogger? log = null)
    {
        try
        {
            EnsureDirectoryForFile(filePath);
            lock (GetLock(filePath))
            {
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
                        log?.Warn($"SafeStorage: Backup copy '{Path.GetFileName(backupPath)}' failed: {ex.Message}");
                    }
                }

                File.Move(tempPath, filePath, true);
                return true;
            }
        }
        catch (Exception ex)
        {
            log?.Error($"SafeStorage: Write error at '{Path.GetFileName(filePath)}': {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Safely loads a JSON object. If the main file is corrupt or unreadable,
    /// tries to load the automatic backup (.bak). If everything fails, returns the fallback.
    /// </summary>
    public static T LoadSafe<T>(string filePath, T fallback, ModLogger? log = null, JsonSerializerOptions? options = null) where T : class
    {
        if (!File.Exists(filePath))
        {
            string backupPath = filePath + ".bak";
            if (File.Exists(backupPath))
            {
                log?.Warn($"SafeStorage: Main file '{Path.GetFileName(filePath)}' missing, loading backup '{Path.GetFileName(backupPath)}'.");
                return TryDeserialize(backupPath, fallback, log, options);
            }
            return fallback;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidDataException("Main file is empty.");

            T? result = JsonSerializer.Deserialize<T>(json, options ?? DefaultJsonOptions);
            return result ?? fallback;
        }
        catch (Exception ex)
        {
            log?.Warn($"SafeStorage: Error loading '{Path.GetFileName(filePath)}' ({ex.Message}) — attempting backup recovery.");
            string backupPath = filePath + ".bak";
            if (File.Exists(backupPath))
                return TryDeserialize(backupPath, fallback, log, options);

            return fallback;
        }
    }

    /// <summary>
    /// Safely reads a text file. If missing or unreadable, the .bak backup or fallback is used.
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
                    log?.Warn($"SafeStorage: Backup read error at '{Path.GetFileName(backupPath)}': {ex.Message}");
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
            log?.Warn($"SafeStorage: Text read error at '{Path.GetFileName(filePath)}' ({ex.Message}) — attempting backup.");
            string backupPath = filePath + ".bak";
            if (File.Exists(backupPath))
            {
                try { return File.ReadAllText(backupPath); }
                catch (Exception exBak)
                {
                    log?.Error($"SafeStorage: Backup read error at '{Path.GetFileName(backupPath)}': {exBak.Message}");
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
            log?.Error($"SafeStorage: Backup '{Path.GetFileName(path)}' unreadable: {ex.Message}");
            return fallback;
        }
    }
}

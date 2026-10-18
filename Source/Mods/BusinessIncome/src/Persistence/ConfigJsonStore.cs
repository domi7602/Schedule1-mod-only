using System;
using System.IO;
using BusinessIncome.Config;
using S1Mods.Shared;

namespace BusinessIncome.Services;

/// <summary>
/// JSON sidecar for config fields that MelonPreferences/TOML cannot map
/// (Dictionary&lt;string,float&gt;, Dictionary&lt;string,string&gt;, List&lt;string&gt;).
/// ModConfig<T> only persists scalar properties - this file secures the
/// complex collections via SafeStorage (atomic + .bak).
/// </summary>
public static class ConfigJsonStore
{
    private static readonly string SavePath = SafeStorage.GetUserDataPath("BusinessIncome", "business_config.json");

    /// <summary>Loads the sidecar into <paramref name="cfg"/> and reports what happened.</summary>
    public static SidecarLoadStatus ApplyToConfig(BusinessIncomeConfig cfg, Action<string>? warn = null)
        => ApplyToConfigFrom(cfg, SavePath, warn);

    /// <summary>Explicit-path variant (used by tests and tooling).</summary>
    public static SidecarLoadStatus ApplyToConfigFrom(BusinessIncomeConfig cfg, string sidecarPath, Action<string>? warn = null)
    {
        if (cfg == null)
        {
            Report(warn, "ConfigJsonStore.ApplyToConfig called with a null config.");
            return SidecarLoadStatus.Unreadable;
        }
        if (sidecarPath == null) throw new ArgumentNullException(nameof(sidecarPath));

        string backupPath = sidecarPath + ".bak";

        bool mainExists = File.Exists(sidecarPath);
        string? mainJson = ReadJsonOrNull(sidecarPath, mainExists, warn);
        bool backupExists = File.Exists(backupPath);
        string? backupJson = ReadJsonOrNull(backupPath, backupExists, warn);

        SidecarLoadStatus status = SidecarLoad.Classify(mainExists, mainJson, backupExists, backupJson, out ConfigSidecarData? data, warn);

        switch (status)
        {
            case SidecarLoadStatus.Missing:
                return status; // nothing on disk - defaults apply silently

            case SidecarLoadStatus.Unreadable:
                Report(warn, "JSON sidecar exists but could not be read; keeping current config.");
                return status;

            case SidecarLoadStatus.UnsupportedSchema:
                Report(warn, $"JSON sidecar declares unsupported schema v{data!.SchemaVersion} (this build supports v{ConfigSidecarData.CurrentSchemaVersion}); not applied, file preserved.");
                return status;

            default:
                data!.ApplyTo(cfg, warn);
                Mod.Log.Info($"Applied JSON sidecar ({(status == SidecarLoadStatus.Backup ? "backup" : "main")}): {cfg.PropertyMultipliers.Count} multipliers, {cfg.DisplayNameOverrides.Count} name overrides, {cfg.WeekendBonusCategories.Count} weekend categories.");
                return status;
        }
    }

    /// <summary>Persists the config's collection values. Returns false on any failure.</summary>
    public static bool Save(BusinessIncomeConfig cfg)
        => Save(cfg, SavePath, false, null);

    /// <summary>Explicit-path variant; refuses to clobber an unsupported schema unless forced.</summary>
    public static bool Save(BusinessIncomeConfig cfg, string sidecarPath, bool forceOverwriteUnsupportedSchema = false, Action<string>? warn = null)
    {
        if (cfg == null)
        {
            Report(warn, "ConfigJsonStore.Save called with a null config.");
            return false;
        }

        if (!forceOverwriteUnsupportedSchema && ExistingSchemaUnsupported(sidecarPath))
        {
            Report(warn, $"Refusing to overwrite '{sidecarPath}': it declares a JSON schema newer than v{ConfigSidecarData.CurrentSchemaVersion}. Pass forceOverwriteUnsupportedSchema: true to override.");
            return false;
        }

        ConfigSidecarData data = ConfigSidecarData.FromConfig(cfg);
        bool ok = SafeStorage.SaveAtomic(sidecarPath, data, Mod.Log, ConfigSidecarData.JsonOptions);
        if (ok)
            Mod.Log.Info($"Saved JSON sidecar to '{sidecarPath}'.");
        else
            Report(warn, $"Failed to write JSON sidecar '{sidecarPath}' (see SafeStorage log).");
        return ok;
    }

    private static bool ExistingSchemaUnsupported(string sidecarPath)
    {
        try
        {
            if (!File.Exists(sidecarPath)) return false;
            string text = File.ReadAllText(sidecarPath);
            if (!ConfigSidecarData.TryParse(text, out ConfigSidecarData? d, out _, null)) return false;
            return d!.HasUnsupportedSchema;
        }
        catch
        {
            return false;
        }
    }

    private static string? ReadJsonOrNull(string path, bool exists, Action<string>? warn)
    {
        if (!exists) return null;
        try
        {
            string text = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text))
            {
                Report(warn, $"JSON sidecar '{Path.GetFileName(path)}' is empty.");
                return null;
            }
            return text;
        }
        catch (Exception ex)
        {
            Report(warn, $"JSON sidecar '{Path.GetFileName(path)}' could not be read: {ex.Message}");
            return null;
        }
    }

    private static void Report(Action<string>? warn, string message)
    {
        warn?.Invoke(message);
        Mod.Log.Warn(message);
    }
}

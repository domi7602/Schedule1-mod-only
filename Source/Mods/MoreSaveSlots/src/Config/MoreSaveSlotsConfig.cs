using System;
using System.IO;
using System.Text.Json;
using MelonLoader.Utils;
using S1Mods.Shared;

namespace MoreSaveSlots.Config;

public class MoreSaveSlotsConfig
{
    public int TotalSlots { get; set; } = 25;
    public int SlotsPerPage { get; set; } = 5;
    public bool EnableKeyboardNavigation { get; set; } = true;
    public bool EnableRenameFeature { get; set; } = true;

    private static readonly string ConfigDir = Path.Combine(MelonEnvironment.UserDataDirectory, "MoreSaveSlots");
    private static readonly string ConfigPath = Path.Combine(ConfigDir, "config.json");

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true
    };

    public static MoreSaveSlotsConfig Load()
    {
        try
        {
            string json = SafeStorage.LoadTextSafe(ConfigPath, "");
            if (!string.IsNullOrWhiteSpace(json))
            {
                var cfg = JsonSerializer.Deserialize<MoreSaveSlotsConfig>(json, JsonOpts);
                if (cfg != null)
                {
                    cfg.Validate();
                    return cfg;
                }
            }
            else if (File.Exists(ConfigPath))
            {
                // File exists but LoadTextSafe returned empty (fallback) — try direct read for diagnostics
                string raw = File.ReadAllText(ConfigPath);
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    var cfg2 = JsonSerializer.Deserialize<MoreSaveSlotsConfig>(raw, JsonOpts);
                    if (cfg2 != null) { cfg2.Validate(); return cfg2; }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"[MoreSaveSlots] Failed to load config: {ex.Message}. Using default configuration.");
        }

        var defaultCfg = new MoreSaveSlotsConfig();
        defaultCfg.Save();
        return defaultCfg;
    }

    public void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(this, JsonOpts);
            bool ok = SafeStorage.SaveTextAtomic(ConfigPath, json);
            if (!ok)
                MelonLoader.MelonLogger.Warning($"[MoreSaveSlots] SafeStorage failed to save config atomically.");
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"[MoreSaveSlots] Failed to save config: {ex.Message}");
        }
    }

    public void Validate()
    {
        if (TotalSlots < 5) TotalSlots = 5;
        if (TotalSlots > 100) TotalSlots = 100;
        if (SlotsPerPage != 5) SlotsPerPage = 5;
    }
}

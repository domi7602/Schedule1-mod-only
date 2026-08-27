using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using S1Mods.Shared;

namespace StackLimitMod;

public class StackLimitConfig
{
    public int StackLimit { get; set; } = 40;
    public bool OverrideNonStackable { get; set; } = true;
    public List<string> ExcludedItemIds { get; set; } = new();
    public bool LogModifications { get; set; } = true;

    private static readonly string ConfigPath = SafeStorage.GetUserDataPath("StackLimitMod", "config.json");

    public static StackLimitConfig Load()
    {
        var cfg = SafeStorage.LoadSafe<StackLimitConfig>(ConfigPath, new StackLimitConfig(), Mod.Log);
        if (cfg == null) cfg = new StackLimitConfig();
        int beforeLimit = cfg.StackLimit;
        cfg.Validate();
        bool fileExists = File.Exists(ConfigPath);
        if (!fileExists)
        {
            cfg.Save();
        }
        else
        {
            // M-6: Heal corrupt file — if LoadSafe returned fallback due to JSON error, overwrite
            try
            {
                string txt = File.ReadAllText(ConfigPath);
                if (string.IsNullOrWhiteSpace(txt) || txt.Trim() == "{}")
                {
                    // empty object may be fallback artifact — check if cfg is default and file invalid
                }
                var test = System.Text.Json.JsonSerializer.Deserialize<StackLimitConfig>(txt);
                if (test == null) throw new InvalidDataException("deserialized null");
            }
            catch
            {
                Mod.Log?.Warn($"Config '{ConfigPath}' was corrupt — healing with defaults.");
                cfg.Save();
            }
            if (beforeLimit != cfg.StackLimit)
            {
                Mod.Log?.Warn($"StackLimit clamped {beforeLimit} -> {cfg.StackLimit} (1..9999)");
            }
        }
        return cfg;
    }

    public void Save()
    {
        Validate();
        SafeStorage.SaveAtomic(ConfigPath, this, Mod.Log);
    }

    public void Validate()
    {
        int orig = StackLimit;
        StackLimit = Math.Clamp(StackLimit, 1, 9999);
        if (orig != StackLimit)
        {
            Mod.Log?.Warn($"StackLimit clamped {orig} -> {StackLimit} (1..9999)");
        }
        ExcludedItemIds ??= new();
        // L-6: Normalize — trim, drop empties, distinct case-insensitive
        var cleaned = ExcludedItemIds.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        if (cleaned.Count != ExcludedItemIds.Count)
        {
            ExcludedItemIds = cleaned;
        }
    }
}

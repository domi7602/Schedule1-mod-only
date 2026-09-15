using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using S1Mods.Shared;

namespace StackLimitMod;

public class StackLimitConfig
{
    public int StackLimit { get; set; } = 40;
    // Bug-Audit 2026-09-12 (Round 3): when true, even items Vanilla ships as
    // non-stackable (Limit=1) get the global StackLimit. The default is kept TRUE
    // for backwards-compatibility with users who already have a config.json, but
    // documented here so new users understand the trade-off (quest items, unique
    // weapons become stackable, which can break certain quests and UI assumptions).
    // Set to false in-game via `stack set overridenonstackable false` if you hit
    // quest- or UI regressions.
    public bool OverrideNonStackable { get; set; } = true;
    /// <summary>
    /// When true (default), stack limits are applied strictly to agriculture and farming items:
    /// soil, seeds, packaging (baggies, jars), additives/fertilizers, mushroom spores/spawns,
    /// and harvested crop products. Weapons, ammunition, clothing, and cash are always protected.
    /// </summary>
    public bool AgricultureOnly { get; set; } = true;
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
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                    PropertyNameCaseInsensitive = true
                };
                var test = System.Text.Json.JsonSerializer.Deserialize<StackLimitConfig>(txt, options);
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

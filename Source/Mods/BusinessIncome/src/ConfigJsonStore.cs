using System;
using BusinessIncome.Config;
using S1Mods.Shared;

namespace BusinessIncome.Services;

/// <summary>
/// JSON-Sidecar für Config-Felder, die MelonPreferences/TOML nicht mappen kann
/// (Dictionary&lt;string,float&gt;, Dictionary&lt;string,string&gt;, List&lt;string&gt;).
/// ModConfig<T> persistiert nur skalare Properties — diese Datei sichert die
/// komplexen Collections über SafeStorage (atomar + .bak).
/// </summary>
public static class ConfigJsonStore
{
    private static readonly string SavePath = SafeStorage.GetUserDataPath("BusinessIncome", "business_config.json");

    public static void ApplyToConfig(BusinessIncomeConfig cfg)
    {
        if (cfg == null) return;
        try
        {
            var stored = SafeStorage.LoadSafe<BusinessIncomeConfig>(SavePath, new BusinessIncomeConfig(), Mod.Log);
            if (stored == null) return;

            if (stored.PropertyMultipliers != null && stored.PropertyMultipliers.Count > 0)
                cfg.PropertyMultipliers = stored.PropertyMultipliers;
            if (stored.DisplayNameOverrides != null && stored.DisplayNameOverrides.Count > 0)
                cfg.DisplayNameOverrides = stored.DisplayNameOverrides;
            if (stored.WeekendBonusCategories != null && stored.WeekendBonusCategories.Count > 0)
                cfg.WeekendBonusCategories = stored.WeekendBonusCategories;

            Mod.Log.Info($"Applied {cfg.PropertyMultipliers.Count} multipliers, {cfg.DisplayNameOverrides.Count} name overrides, {cfg.WeekendBonusCategories.Count} weekend categories from JSON sidecar.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to apply config JSON sidecar: {ex.Message}");
        }
    }

    public static void Save(BusinessIncomeConfig cfg)
    {
        if (cfg == null) return;
        try
        {
            SafeStorage.SaveAtomic(SavePath, cfg, Mod.Log);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to save config JSON sidecar: {ex.Message}");
        }
    }
}

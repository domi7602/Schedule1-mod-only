using System;
using System.Collections.Generic;
using System.IO;
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
        cfg.Validate();
        if (!File.Exists(ConfigPath))
        {
            cfg.Save();
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
        StackLimit = Math.Clamp(StackLimit, 1, 9999);
        ExcludedItemIds ??= new();
    }
}

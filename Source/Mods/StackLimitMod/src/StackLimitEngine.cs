using System;
using System.Collections.Generic;
using HarmonyLib;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.ItemFramework;
using UnityEngine;

namespace StackLimitMod;

public static class StackLimitEngine
{
    private static readonly Dictionary<string, int> _originalLimits = new(StringComparer.OrdinalIgnoreCase);
    private static readonly object _lock = new();
    internal static object _lockProxy => _lock;

    public static int ModifiedItemCount { get; private set; }
    public static int TrackedItemCount
    {
        get
        {
            lock (_lock)
            {
                return _originalLimits.Count;
            }
        }
    }

    public static int GetOriginalLimit(string id, int fallback = 1)
    {
        if (string.IsNullOrEmpty(id)) return fallback;
        lock (_lock)
        {
            return _originalLimits.TryGetValue(id, out int orig) ? orig : fallback;
        }
    }

    public static int ApplyStackLimits(StackLimitConfig config)
    {
        if (config == null) return 0;



        var processedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        int count = 0;

        // 1. Scan loaded BaseItemDefinitions via Resources
        try
        {
            var foundDefs = Resources.FindObjectsOfTypeAll<BaseItemDefinition>();
            if (foundDefs != null)
            {
                for (int i = 0; i < foundDefs.Length; i++)
                {
                    var def = foundDefs[i];
                    if (ApplyToDefinition(def, config, processedIds))
                    {
                        count++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyStackLimits Resources scan encountered exception: {ex.Message}");
        }

        // 2. Scan Registry items
        try
        {
            var registry = Registry.Instance;
            if (registry != null && registry.Pointer != IntPtr.Zero)
            {
                var allItems = registry.GetAllItems();
                if (allItems != null && allItems.Pointer != IntPtr.Zero)
                {
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        var item = allItems[i];
                        if (ApplyToDefinition(item, config, processedIds))
                        {
                            count++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyStackLimits Registry scan encountered exception: {ex.Message}");
        }

        ModifiedItemCount = count;

        if (config.LogModifications)
        {
            Mod.Log?.Info($"StackLimitEngine: Applied stack limit ({config.StackLimit}) to {count} items.");
        }

        return count;
    }

    public static void ApplyToItem(ItemDefinition def, StackLimitConfig config)
    {
        ApplyToDefinition(def, config, null);
    }

    public static void ApplyToItem(BaseItemDefinition def, StackLimitConfig config)
    {
        ApplyToDefinition(def, config, null);
    }

    private static bool ApplyToDefinition(BaseItemDefinition? def, StackLimitConfig config, HashSet<string>? processedIds)
    {
        if (def == null || def.Pointer == IntPtr.Zero) return false;

        string id = def.ID;
        if (string.IsNullOrEmpty(id)) return false;

        if (processedIds != null && !processedIds.Add(id))
            return false;

        int originalLimit;
        lock (_lock)
        {
            if (!_originalLimits.TryGetValue(id, out originalLimit))
            {
                originalLimit = def.StackLimit;
                _originalLimits[id] = originalLimit;
            }
        }

        if (config.ExcludedItemIds != null && config.ExcludedItemIds.Exists(x => string.Equals(x, id, StringComparison.OrdinalIgnoreCase)))
            return false;

        if (!config.OverrideNonStackable && originalLimit == 1)
            return false;

        try
        {
            def.StackLimit = config.StackLimit;
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyToDefinition failed for '{id}': {ex.GetType().Name}: {ex.Message}");
            return false;
        }

        return true;
    }
}

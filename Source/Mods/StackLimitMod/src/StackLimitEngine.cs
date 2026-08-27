using System;
using System.Collections.Generic;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.ItemFramework;
using UnityEngine;

namespace StackLimitMod;

public static class StackLimitEngine
{
    private static readonly Dictionary<string, int> _originalLimits = new(StringComparer.OrdinalIgnoreCase);
    private static readonly object _lock = new();
    private static HashSet<string>? _excludedSet;
    private static int _excludedSetHash = 0;

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

    public static bool IsExcluded(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        var set = _excludedSet;
        if (set != null) return set.Contains(id);
        var cfg = Mod.Config;
        if (cfg?.ExcludedItemIds == null) return false;
        return cfg.ExcludedItemIds.Exists(x => string.Equals(x, id, StringComparison.OrdinalIgnoreCase));
    }

    private static void RebuildExcludedCache(StackLimitConfig config)
    {
        if (config?.ExcludedItemIds == null || config.ExcludedItemIds.Count == 0)
        {
            _excludedSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludedSetHash = 0;
            return;
        }
        int hash = config.ExcludedItemIds.Count;
        for (int i = 0; i < config.ExcludedItemIds.Count; i++)
        {
            var s = config.ExcludedItemIds[i];
            hash = HashCode.Combine(hash, s != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(s.Trim()) : 0);
        }
        if (_excludedSet == null || hash != _excludedSetHash)
        {
            var cleaned = new List<string>();
            for (int i = 0; i < config.ExcludedItemIds.Count; i++)
            {
                var s = config.ExcludedItemIds[i];
                if (string.IsNullOrWhiteSpace(s)) continue;
                cleaned.Add(s.Trim());
            }
            _excludedSet = new HashSet<string>(cleaned, StringComparer.OrdinalIgnoreCase);
            _excludedSetHash = hash;
        }
    }

    public static int ApplyStackLimits(StackLimitConfig config)
    {
        if (config == null) return 0;

        RebuildExcludedCache(config);

        var processedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        int count = 0;
        int resourcesCount = 0;
        int registryCount = 0;

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
                        resourcesCount++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyStackLimits Resources scan encountered exception: {ex}");
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
                            registryCount++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyStackLimits Registry scan encountered exception: {ex}");
        }

        // M-4: Only publish count after both scans; if partial, log warn
        ModifiedItemCount = count;

        if (config.LogModifications)
        {
            if (count == 0 && (resourcesCount == 0 && registryCount == 0))
                Mod.Log?.Debug($"StackLimitEngine: No items modified (limit {config.StackLimit}).");
            else
                Mod.Log?.Info($"StackLimitEngine: Applied stack limit ({config.StackLimit}) to {count} items (Resources:{resourcesCount} Registry:{registryCount}).");
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

        if (IsExcluded(id))
            return false;

        if (!config.OverrideNonStackable && originalLimit == 1)
            return false;

        try
        {
            def.StackLimit = config.StackLimit;
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyToDefinition failed for '{id}': {ex}");
            return false;
        }

        return true;
    }
}

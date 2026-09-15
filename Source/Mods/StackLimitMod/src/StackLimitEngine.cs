using System;
using System.Collections.Generic;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Clothing;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.Equipping.Framework;
using Il2CppScheduleOne.Growing;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.Product.Packaging;
using UnityEngine;

namespace StackLimitMod;

public static class StackLimitEngine
{
    private static readonly Dictionary<string, int> _originalLimits = new(StringComparer.OrdinalIgnoreCase);
    private static readonly object _lock = new();
    private static HashSet<string>? _excludedSet;
    private static int _excludedSetHash = 0;
    private static bool _restoreWarned;

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

    /// <summary>True once a scan has memorized this ID's vanilla limit.</summary>
    public static bool IsOriginalKnown(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        lock (_lock) { return _originalLimits.ContainsKey(id); }
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

    /// <summary>
    /// Cold-path: applies <see cref="StackLimitConfig.StackLimit"/> to every known item definition.
    /// Called once on game load / config change — never per-frame. Safe to do heavier scans here.
    /// </summary>
    /// <remarks>
    /// Dual-scan rationale:
    /// <list type="bullet">
    /// <item><description><c>Resources.FindObjectsOfTypeAll&lt;BaseItemDefinition&gt;</c> — legacy / pre-Registry fallback.
    /// Catches definitions that exist as loaded Unity objects before (or without) Registry registration.</description></item>
    /// <item><description><c>Registry.GetAllItems()</c> — canonical post-load source of truth. After scene load
    /// this should contain every item; when both scans return the same ID, <c>processedIds</c> dedup ensures
    /// the Registry entry is not double-counted (Resources entry wins first-write, Registry is skipped).</description></item>
    /// </list>
    /// If Registry returns items, the Resources scan is purely a compatibility fallback for pre-registered /
    /// mod-added definitions that haven't been registered yet. Order matters: Resources first, then Registry.
    /// </remarks>
    public static int ApplyStackLimits(StackLimitConfig config)
    {
        if (config == null) return 0;

        RebuildExcludedCache(config);

        var processedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        int count = 0;
        int resourcesCount = 0;
        int registryCount = 0;

        // ── Dual scan: Resources (legacy / pre-Registry fallback) → Registry (canonical) ──
        // Resources catches definitions that exist as loaded objects before Registry registration.
        // Registry is authoritative post-load; processedIds dedupes so each ID is patched once.
        // This whole method is cold-path (load / config change only, not per-frame).

        // 1. Scan loaded BaseItemDefinitions via Resources (fallback for pre-Registry defs)
        bool resourcesExecuted = false;
        int resourcesFound = 0;
        try
        {
            var foundDefs = Resources.FindObjectsOfTypeAll<BaseItemDefinition>();
            if (foundDefs != null)
            {
                resourcesExecuted = true;
                resourcesFound = foundDefs.Length;
                for (int i = 0; i < foundDefs.Length; i++)
                {
                    var def = foundDefs[i];
                    if (ApplyToDefinition(def, config, processedIds))
                    {
                        count++;
                        resourcesCount++;
                    }
                }
                if (resourcesFound == 0)
                    Mod.Log?.Debug("StackLimitEngine: Resources scan executed but found 0 BaseItemDefinitions (expected post-load; Registry is canonical).");
                else if (resourcesCount == 0)
                    Mod.Log?.Debug($"StackLimitEngine: Resources scan executed — found {resourcesFound} defs, 0 modified (all excluded/empty/already limited or limit={config.StackLimit}).");
                else
                    Mod.Log?.Debug($"StackLimitEngine: Resources scan executed — found {resourcesFound} defs, modified {resourcesCount} (pre-Registry / legacy).");
            }
            else
            {
                Mod.Log?.Debug("StackLimitEngine: Resources scan skipped — FindObjectsOfTypeAll returned null.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyStackLimits Resources scan encountered exception: {ex}");
            if (!resourcesExecuted)
                Mod.Log?.Debug("StackLimitEngine: Resources scan skipped due to exception (see warn above).");
        }

        // 2. Scan Registry items — canonical post-load source (authoritative)
        // If Registry returns items, Resources above was just the fallback for defs not yet registered.
        // processedIds dedup ensures we don't double-patch the same ID.
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

        // M-4: Only publish count after both scans complete; config-aware logging below.
        // Cold-path note: this publish + log runs once per load/config change, not per-frame.
        ModifiedItemCount = count;

        // Config-aware logging: respect config.LogModifications — silent when disabled.
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

    /// <summary>
    /// Checks if the item definition is a weapon, ammunition, clothing, or tool with runtime state.
    /// Stacking these items causes severe UI corruption (Chinese characters) and ammo reload underflows (-1 ammo).
    /// These items must NEVER have their stack limits increased above vanilla limits.
    /// </summary>
    public static bool IsWeaponOrAmmo(BaseItemDefinition? def)
    {
        if (def == null || def.Pointer == IntPtr.Zero) return false;
        string id = def.ID ?? string.Empty;
        if (IsWeaponOrAmmoId(id)) return true;

        try
        {
            if (def is EquippableItemDefinition) return true;
            if (def is ClothingDefinition) return true;
            if (def is CashDefinition) return true;

            if (def is ItemDefinition itemDef && itemDef.Pointer != IntPtr.Zero)
            {
                var eq = itemDef.Equippable;
                if (eq != null && eq.Pointer != IntPtr.Zero)
                {
                    if (eq is Equippable_RangedWeapon || eq is Equippable_MeleeWeapon)
                        return true;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"IsWeaponOrAmmo type check failed for '{id}': {ex.Message}");
        }

        return false;
    }

    /// <summary>
    /// Fast string-based check for weapon and ammo identifiers.
    /// </summary>
    public static bool IsWeaponOrAmmoId(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        string lower = id.ToLowerInvariant();

        // Direct weapon/ammo keywords
        if (lower.Contains("ammo") || lower.Contains("bullet") || lower.Contains("shell") || lower.Contains("cartridge"))
            return true;
        if (lower.Contains("pistol") || lower.Contains("shotgun") || lower.Contains("rifle") || lower.Contains("smg") ||
            lower.Contains("revolver") || lower.Contains("sniper") || lower.Contains("weapon") || lower.Contains("firearm"))
            return true;
        if (lower.Contains("knife") || lower.Contains("bat") || lower.Contains("crowbar") || lower.Contains("machete") ||
            lower.Contains("taser") || lower.Contains("grenade") || lower.Contains("molotov") || lower.Contains("c4") ||
            lower.Contains("rdx") || lower.Contains("bomb"))
            return true;

        return false;
    }

    /// <summary>
    /// Checks if an item definition belongs to agriculture and farming:
    /// soil, seeds, packaging (baggies, jars), additives/fertilizers, mushroom spores/spawns,
    /// and harvested crop products.
    /// </summary>
    public static bool IsAgricultureItem(BaseItemDefinition? def)
    {
        if (def == null || def.Pointer == IntPtr.Zero) return false;
        string id = def.ID ?? string.Empty;

        // Permanent weapon/ammo veto
        if (IsWeaponOrAmmo(def)) return false;

        try
        {
            if (def is SoilDefinition) return true;
            if (def is SeedDefinition) return true;
            if (def is PackagingDefinition) return true;
            if (def is AdditiveDefinition) return true;
            if (def is ShroomSpawnDefinition) return true;
            if (def is SporeSyringeDefinition) return true;
            if (def is ProductDefinition) return true;
            if (def is QualityItemDefinition) return true;
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"IsAgricultureItem type check failed for '{id}': {ex.Message}");
        }

        // ID / Name heuristics for modded or dynamically injected agricultural items
        return IsAgricultureId(id);
    }

    /// <summary>
    /// Fast ID-based heuristic for agricultural items.
    /// </summary>
    public static bool IsAgricultureId(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        if (IsWeaponOrAmmoId(id)) return false;

        string lower = id.ToLowerInvariant();
        return lower.Contains("seed") ||
               lower.Contains("soil") ||
               lower.Contains("baggie") ||
               lower.Contains("jar") ||
               lower.Contains("fertilizer") ||
               lower.Contains("additive") ||
               lower.Contains("spore") ||
               lower.Contains("spawn") ||
               lower.Contains("weed") ||
               lower.Contains("shroom") ||
               lower.Contains("mushroom") ||
               lower.Contains("packaging") ||
               lower.Contains("bud") ||
               lower.Contains("plantfood") ||
               lower.Contains("cocoir") ||
               lower.Contains("potting");
    }

    /// <summary>
    /// Checks whether an item ID is eligible for stack limit overrides given the current configuration.
    /// </summary>
    public static bool IsEligibleForOverride(string id, BaseItemInstance? instance = null)
    {
        if (string.IsNullOrEmpty(id)) return false;
        if (IsExcluded(id)) return false;
        if (IsWeaponOrAmmoId(id)) return false;

        var cfg = Mod.Config;
        if (cfg == null) return false;

        if (cfg.AgricultureOnly)
        {
            // First check fast ID heuristic
            if (IsAgricultureId(id)) return true;

            // Otherwise resolve definition from Registry if available
            try
            {
                var def = Registry.GetItem(id);
                if (def != null && def.Pointer != IntPtr.Zero)
                {
                    return IsAgricultureItem(def);
                }
            }
            catch { }

            return false;
        }

        return true;
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

        // 1. Permanent protection: Weapons & Ammo MUST NEVER be stacked!
        if (IsWeaponOrAmmo(def))
        {
            try
            {
                if (def.StackLimit != originalLimit)
                    def.StackLimit = originalLimit;
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"ApplyToDefinition weapon-restore failed for '{id}': {ex}");
            }
            return false;
        }

        // 2. Excluded items must behave as if the mod never touched them: restore original limit
        if (IsExcluded(id))
        {
            try
            {
                if (def.StackLimit != originalLimit)
                    def.StackLimit = originalLimit;
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"ApplyToDefinition exclude-restore failed for '{id}': {ex}");
            }
            return false;
        }

        // 3. Agriculture Only mode: if enabled, only agricultural items are modified!
        if (config.AgricultureOnly && !IsAgricultureItem(def))
        {
            try
            {
                if (def.StackLimit != originalLimit)
                    def.StackLimit = originalLimit;
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"ApplyToDefinition non-ag-restore failed for '{id}': {ex}");
            }
            return false;
        }

        // 4. NonStackable guard: if OverrideNonStackable is false and original == 1
        if (!config.OverrideNonStackable && originalLimit == 1)
        {
            try
            {
                if (def.StackLimit != originalLimit)
                    def.StackLimit = originalLimit;
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"ApplyToDefinition restore failed for '{id}': {ex}");
            }
            return false;
        }

        try
        {
            def.StackLimit = config.StackLimit;
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ApplyToDefinition failed for '{id}': {ex.Message}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Bug-Audit 2026-09-12 (Round 3): Restore every previously captured original stack
    /// limit. Called from Mod.OnDeinitializeMelon (and from the `stack restore` console
    /// command) so a disabling / unload / explicit-restore leaves the registry clean.
    /// Without this, mod uninstall / disable would leave overrides on every definition
    /// and survive reloads — Vanilla's <c>StackLimit</c> would never come back.
    /// </summary>
    public static int RestoreAll()
    {
        int restored = 0;
        int failed = 0;
        List<string>? snapshot = null;
        lock (_lock)
        {
            if (_originalLimits.Count == 0) return 0;
            snapshot = new List<string>(_originalLimits.Keys);
        }

        // Audit 2026-09-13: OnDeinitializeMelon runs AFTER the game has torn down the
        // Registry singleton — Registry.GetItem(id) then throws NullReferenceException
        // for every tracked item (~150 log lines of noise at quit). Harden:
        // 1) bail out entirely when the singleton is gone, 2) log only the first failure
        // of any remaining per-item errors.
        try
        {
            var registry = Registry.Instance;
            if (registry == null || registry.Pointer == IntPtr.Zero || registry.WasCollected)
            {
                Mod.Log?.Debug("RestoreAll: Registry unavailable (shutdown) — skipping restore; limits re-apply next launch.");
                return 0;
            }
        }
        catch { return 0; }

        foreach (var id in snapshot)
        {
            int original;
            lock (_lock) { if (!_originalLimits.TryGetValue(id, out original)) continue; }
            try
            {
                var def = Registry.GetItem(id);
                if (def != null && def.Pointer != IntPtr.Zero && !def.WasCollected)
                {
                    def.StackLimit = original;
                    restored++;
                }
            }
            catch (Exception ex)
            {
                failed++;
                if (!_restoreWarned)
                {
                    _restoreWarned = true;
                    Mod.Log?.Warn($"RestoreAll: failed to restore '{id}' ({failed} total so far): {ex.Message}");
                }
            }
        }
        return restored;
    }

    /// <summary>
    /// Forget every captured original value. After this, a re-Apply will start a fresh
    /// capture set. Use for test isolation or full reset.
    /// </summary>
    public static void ForgetAllOriginals()
    {
        lock (_lock) { _originalLimits.Clear(); }
    }
}

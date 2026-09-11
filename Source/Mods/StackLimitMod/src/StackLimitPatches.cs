using System;
using System.Collections.Generic;
using HarmonyLib;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.ItemFramework;

namespace StackLimitMod;

public static class StackLimitPatches
{
    // Hot-path decision cache for get_StackLimit: whether the override applies, keyed by the native
    // object pointer (BaseItemInstance is an Il2CppSystem.Object — no Unity GetInstanceID()). Avoids
    // the IL2CPP ID string marshaling + engine lookups on every call. Postfix runs on the main thread,
    // so a plain Dictionary is sufficient. Pointers are per-session: cleared on scene unload and on
    // 'stack reload' (config change may flip exclusions / OverrideNonStackable).
    // Residual risk: the IL2CPP GC recycles pointers, so a fresh instance can inherit a stale
    // decision until the next clear. MaxEntries bounds both the memory growth and that stale
    // window (worst case a few thousand instances in a long session).
    private static readonly Dictionary<IntPtr, bool> _overrideDecisionCache = new();
    private const int MaxDecisionCacheEntries = 4096;

    public static void ClearDecisionCache()
    {
        _overrideDecisionCache.Clear();
    }

    [HarmonyPostfix]
    public static void Registry_AddToRegistry_Postfix(Registry __instance, ItemDefinition item)
    {
        try
        {
            if (Mod.Config == null) return;
            if (item == null || item.Pointer == IntPtr.Zero) return;
            if (__instance != null && __instance.Pointer == IntPtr.Zero) return;
            StackLimitEngine.ApplyToItem(item, Mod.Config);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Error in Registry_AddToRegistry_Postfix: {ex}");
        }
    }

    [HarmonyPostfix]
    public static void BaseItemInstance_GetStackLimit_Postfix(BaseItemInstance __instance, ref int __result)
    {
        try
        {
            if (Mod.Config == null) return;
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return;

            // Hot-path: consult the decision cache before resolving the ID string (no marshaling on hit).
            IntPtr key = __instance.Pointer;
            if (_overrideDecisionCache.TryGetValue(key, out bool shouldOverride))
            {
                if (shouldOverride) __result = Mod.Config.StackLimit;
                return;
            }

            string id = __instance.ID;
            if (string.IsNullOrEmpty(id)) return;

            bool excluded = StackLimitEngine.IsExcluded(id);
            bool keepOriginal = false;
            // Unknown ID (instance seen before the first definition scan): decide live
            // but do NOT cache — the fallback limit (1) may be wrong, and a cached
            // false would stick until scene unload even after the scan fills in.
            bool known = StackLimitEngine.IsOriginalKnown(id);
            if (!excluded && !Mod.Config.OverrideNonStackable)
            {
                int orig = StackLimitEngine.GetOriginalLimit(id, 1);
                keepOriginal = orig == 1;
            }

            shouldOverride = !excluded && !keepOriginal;
            if (known)
            {
                if (_overrideDecisionCache.Count >= MaxDecisionCacheEntries)
                    _overrideDecisionCache.Clear();
                _overrideDecisionCache[key] = shouldOverride;
            }

            if (shouldOverride) __result = Mod.Config.StackLimit;
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Error in BaseItemInstance_GetStackLimit_Postfix: {ex}");
        }
    }

    // Entfernt: BaseItemDefinition_GetDefaultStackLimit ist ein Field-Accessor und IL2CPP-seitig nicht patchbar.
    // Siehe Mod.ApplyHarmonyPatches — Engine löst das via Scan + Field-Write.
}

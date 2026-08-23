using System;
using HarmonyLib;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.ItemFramework;

namespace StackLimitMod;

public static class StackLimitPatches
{
    [HarmonyPostfix]
    public static void Registry_AddToRegistry_Postfix(ItemDefinition item)
    {
        try
        {
            if (item != null && item.Pointer != IntPtr.Zero && Mod.Config != null)
            {
                StackLimitEngine.ApplyToItem(item, Mod.Config);
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"Error in Registry_AddToRegistry_Postfix: {ex.Message}");
        }
    }

    [HarmonyPostfix]
    public static void BaseItemInstance_GetStackLimit_Postfix(BaseItemInstance __instance, ref int __result)
    {
        try
        {
            if (Mod.Config == null) return;
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return;

            string id = __instance.ID;
            if (string.IsNullOrEmpty(id)) return;

            if (Mod.Config.ExcludedItemIds != null && Mod.Config.ExcludedItemIds.Exists(x => string.Equals(x, id, StringComparison.OrdinalIgnoreCase)))
                return;

            if (!Mod.Config.OverrideNonStackable)
            {
                // If item was never scanned, treat as non-stackable (orig==1) to avoid accidentally stacking unintended items
                // This prevents race where get_StackLimit fires before Registry_AddToRegistry_Postfix snapshot
                int orig;
                lock (StackLimitEngine._lockProxy) { orig = StackLimitEngine.GetOriginalLimit(id, -1); }
                if (orig == -1) orig = 1;
                if (orig == 1) return;
                // Also check fallback __result ==1 as secondary guard
                if (StackLimitEngine.GetOriginalLimit(id, __result) == 1 && __result == 1) return;
            }

            __result = Mod.Config.StackLimit;
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"Error in BaseItemInstance_GetStackLimit_Postfix: {ex.Message}");
        }
    }

    // Entfernt: BaseItemDefinition_GetDefaultStackLimit ist ein Field-Accessor und IL2CPP-seitig nicht patchbar.
    // Siehe Mod.ApplyHarmonyPatches — Engine löst das via Scan + Field-Write.
}

using System;
using HarmonyLib;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.ItemFramework;

namespace StackLimitMod;

public static class StackLimitPatches
{
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

            string id = __instance.ID;
            if (string.IsNullOrEmpty(id)) return;

            // Hot-path: HashSet O(1) no allocation (M-3)
            if (StackLimitEngine.IsExcluded(id)) return;

            if (!Mod.Config.OverrideNonStackable)
            {
                int orig = StackLimitEngine.GetOriginalLimit(id, 1);
                if (orig == 1) return;
            }

            __result = Mod.Config.StackLimit;
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Error in BaseItemInstance_GetStackLimit_Postfix: {ex}");
        }
    }

    // Entfernt: BaseItemDefinition_GetDefaultStackLimit ist ein Field-Accessor und IL2CPP-seitig nicht patchbar.
    // Siehe Mod.ApplyHarmonyPatches — Engine löst das via Scan + Field-Write.
}

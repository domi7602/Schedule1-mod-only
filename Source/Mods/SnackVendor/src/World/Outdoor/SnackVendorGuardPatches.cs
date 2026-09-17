using System;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.Tiles;
using S1Mods.Shared;
using UnityEngine;

namespace SnackVendor.World.Outdoor;

/// <summary>
/// Golden Rule 3 & 7 guards: prevents vanilla property lookups, interior culling,
/// and auto-destruction on outdoor SnackVendor stations.
/// </summary>
public static class SnackVendorGuardPatches
{
    public static class BuildableItem_Start_Guard
    {
        public static bool Prefix(BuildableItem __instance)
        {
            try
            {
                if (__instance != null && __instance.Pointer != IntPtr.Zero && !__instance.WasCollected &&
                    __instance.gameObject != null && __instance.gameObject.Pointer != IntPtr.Zero)
                {
                    if (SnackVendorOutdoorManager.IsOutdoorStation(__instance.gameObject))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"BuildableItem_Start_Guard error: {ex.Message}");
            }
            return true;
        }
    }

    public static class BuildableItem_SetCulled_Guard
    {
        public static bool Prefix(BuildableItem __instance, bool culled)
        {
            try
            {
                if (__instance != null && __instance.Pointer != IntPtr.Zero && !__instance.WasCollected &&
                    __instance.gameObject != null && __instance.gameObject.Pointer != IntPtr.Zero)
                {
                    if (SnackVendorOutdoorManager.IsOutdoorStation(__instance.gameObject))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"BuildableItem_SetCulled_Guard error: {ex.Message}");
            }
            return true;
        }
    }

    public static class GridItem_Destroy_Guard
    {
        public static bool Prefix(GridItem __instance)
        {
            try
            {
                if (__instance != null && __instance.Pointer != IntPtr.Zero && !__instance.WasCollected &&
                    __instance.gameObject != null && __instance.gameObject.Pointer != IntPtr.Zero)
                {
                    if (SnackVendorOutdoorManager.IsOutdoorStation(__instance.gameObject))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"GridItem_Destroy_Guard error: {ex.Message}");
            }
            return true;
        }
    }
}

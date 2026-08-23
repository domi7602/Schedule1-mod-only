using System;
using CustomSkateboard.Config;
using CustomSkateboard.Items;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Skating;

namespace CustomSkateboard.Visuals;

/// <summary>
/// Contains Harmony patch handlers invoked safely via PatchGuard for visuals and anti-gravel physics.
/// </summary>
public static class SkateboardVisualPatches
{
    public static void OnEquipPostfix(Skateboard_Equippable __instance, ItemInstance item)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || item == null || item.Pointer == IntPtr.Zero)
                return;

            if (SkateboardItemFactory.IsCustomItem(item))
            {
                if (__instance.ModelContainer != null)
                {
                    CyberSkateboardVisualizer.ApplyToViewmodel(__instance.ModelContainer.gameObject);
                }
                else
                {
                    CyberSkateboardVisualizer.ApplyToViewmodel(__instance.gameObject);
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Equip patch notice: {ex.Message}");
        }
    }

    public static void OnMountPostfix(Skateboard_Equippable __instance)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return;

            var itemInst = __instance.itemInstance;
            if (SkateboardItemFactory.IsCustomItem(itemInst))
            {
                var activeBoard = __instance.ActiveSkateboard;
                if (activeBoard != null && activeBoard.Pointer != IntPtr.Zero)
                {
                    SkateboardItemFactory.TuneSkateboard(activeBoard, Mod.CurrentConfig);
                    CyberSkateboardVisualizer.ApplyToSkateboard(activeBoard, Mod.CurrentConfig);
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"OnMount patch notice: {ex.Message}");
        }
    }

    public static void OnSkateboardAwakePostfix(Skateboard __instance)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return;

            if (SkateboardItemFactory.IsCustomItem(__instance.Equippable?.itemInstance))
            {
                SkateboardItemFactory.TuneSkateboard(__instance, Mod.CurrentConfig);
                CyberSkateboardVisualizer.ApplyToSkateboard(__instance, Mod.CurrentConfig);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Skateboard.Awake patch notice: {ex.Message}");
        }
    }

    /// <summary>
    /// Prevents gravel, grass, and dirt slowdown by returning 1.0f (maximum smoothness) for the custom skateboard.
    /// </summary>
    public static bool OnGetSurfaceSmoothnessPrefix(Skateboard __instance, ref float __result)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return true;

            if (SkateboardItemFactory.IsCustomItem(__instance.Equippable?.itemInstance) && Mod.CurrentConfig.DisableTerrainSlowdown)
            {
                __result = 1.0f;
                return false;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"GetSurfaceSmoothness prefix notice: {ex.Message}");
        }
        return true;
    }

    /// <summary>
    /// Prevents terrain friction check from flagging the custom board as stuck on rough terrain.
    /// </summary>
    public static bool OnIsOnTerrainPrefix(Skateboard __instance, ref bool __result)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return true;

            if (SkateboardItemFactory.IsCustomItem(__instance.Equippable?.itemInstance) && Mod.CurrentConfig.DisableTerrainSlowdown)
            {
                __result = false;
                return false;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"IsOnTerrain prefix notice: {ex.Message}");
        }
        return true;
    }

    public static void OnWeatherChangePostfix(Skateboard __instance)
    {
        try
        {
            if (SkateboardItemFactory.IsCustomSkateboard(__instance))
            {
                SkateboardItemFactory.TuneSkateboard(__instance, Mod.CurrentConfig, forceRetune: true, logStats: false);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Weather change re-tuning notice: {ex.Message}");
        }
    }
}

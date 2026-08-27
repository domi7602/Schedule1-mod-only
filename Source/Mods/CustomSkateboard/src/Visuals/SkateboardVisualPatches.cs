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

    // Hot-path early-out counters (per-physics-step).
    // These Harmony prefix methods fire on EVERY Skateboard in the scene (player + NPCs),
    // EVERY physics step while any board is mounted. For non-custom boards the cost must be ~0.
    public static long SmoothnessEarlyOuts;
    public static long SmoothnessTunedHits;
    public static long TerrainEarlyOuts;
    public static long TerrainTunedHits;

    /// <summary>
    /// Prevents gravel, grass, and dirt slowdown by returning 1.0f (maximum smoothness) for the custom skateboard.
    ///
    /// Performance: this fires every physics step for every mounted board in the scene.
    /// Early-out via the tuned-instance set so non-custom boards pay only a null-check + HashSet lookup.
    /// </summary>
    public static bool OnGetSurfaceSmoothnessPrefix(Skateboard __instance, ref float __result)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) { return true; }

            // Cache the Unity instance ID once. GetInstanceID() is a cheap native call.
            int instId = __instance.GetInstanceID();
            if (instId == 0 || !SkateboardItemFactory.IsInstanceTuned(instId))
            {
                System.Threading.Interlocked.Increment(ref SmoothnessEarlyOuts);
                return true;
            }

            System.Threading.Interlocked.Increment(ref SmoothnessTunedHits);

            // Only our custom board gets the terrain-smoothing override — and only if user enabled it.
            if (Mod.CurrentConfig.DisableTerrainSlowdown)
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
    /// Same hot-path early-out as GetSurfaceSmoothness.
    /// </summary>
    public static bool OnIsOnTerrainPrefix(Skateboard __instance, ref bool __result)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) { return true; }

            int instId = __instance.GetInstanceID();
            if (instId == 0 || !SkateboardItemFactory.IsInstanceTuned(instId))
            {
                System.Threading.Interlocked.Increment(ref TerrainEarlyOuts);
                return true;
            }

            System.Threading.Interlocked.Increment(ref TerrainTunedHits);

            if (Mod.CurrentConfig.DisableTerrainSlowdown)
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

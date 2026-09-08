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
    // Gatekeeper-fix B10: Interlocked counters are dead code in hotpath + Mod.CurrentConfig per tick does try/catch.
    // Cached flag is set at Tune time; counters are debug-only.
#if DEBUG
    public static long SmoothnessEarlyOuts;
    public static long SmoothnessTunedHits;
    public static long TerrainEarlyOuts;
    public static long TerrainTunedHits;
#endif
    // Gatekeeper-fix B10: cache flag at Tune time to avoid Mod.CurrentConfig try/catch per physics tick.
    /// <summary>
    /// Fix 2026-09-02 (v3): vanilla jump is a CHARGE mechanic —
    /// force = Lerp(JumpForceMin, JumpForce, JumpBuildAmount), where JumpBuildAmount
    /// ramps 0→1 while the jump key is held (over static JumpForceBuildTime).
    /// v1 (write IL2CPP statics) crashed: AccessViolationException — statics live on the
    /// nested compiler display class. v2 (postfix pinning JumpBuildAmount=1 always) caused
    /// rider shaking (vanilla uses the value for pre-jump suspension compression → board
    /// thought it was permanently fully charged) AND the jump still fired weak, because a
    /// postfix runs AFTER vanilla's own FixedUpdate charge/jump logic (race).
    /// v3 = PREFIX (runs before vanilla) + only pin while a charge is already in progress
    /// (JumpBuildAmount > 0): idle = 0 → untouched (no shaking); key tapped → vanilla ramps
    /// 0.02 → prefix snaps to 1 → jump fires at full tuned force in the same tick.
    /// </summary>
    public static void OnFixedUpdatePrefix(Skateboard __instance)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero || __instance.WasCollected) return;

            int instId = __instance.GetInstanceID();
            if (instId == 0 || !SkateboardItemFactory.IsInstanceTuned(instId)) return;

            float build = __instance.JumpBuildAmount;
            if (build > 0f && build < 1f)
            {
                __instance.JumpBuildAmount = 1f;
            }
        }
        catch
        {
            // Never let the prefix kill the physics loop.
        }
    }

    private static bool s_disableTerrainSlowdownCached = true;
    internal static void SetDisableTerrainSlowdownCached(bool v) => s_disableTerrainSlowdownCached = v;

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

using System;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.EntityFramework;
using MelonLoader;
using S1Mods.Shared;

namespace SnackVendor.World;

/// <summary>
/// Hooks BuildableItem.Start (vanilla) so every Snack-Vendor station that
/// gets placed in the world also receives a SnackVendorController, which
/// in turn spawns the vanilla VendingMachine-clone + manages stock.
/// Mirrors AutoPack's BuildableItem_Start_Patch exactly.
///
/// Note (Audit 0.0.3): the [HarmonyPatch] class-attribute and the
/// [HarmonyPatch("Start")] method-attribute are intentionally absent. We
/// register this patch explicitly through PatchGuard.TryPatch in Mod.cs so
/// the PatchGuard statistics cover it (graceful-degradation on signature
/// drift, visible failure-log on game updates).
/// </summary>
public static class BuildableItem_Start_Patch
{
    public static void Postfix(BuildableItem __instance)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return;
            if (__instance.gameObject == null || __instance.gameObject.Pointer == IntPtr.Zero) return;

            var inst = __instance.ItemInstance;
            if (inst == null || inst.Pointer == IntPtr.Zero) return;
            var def = inst.Definition;
            if (def == null || def.Pointer == IntPtr.Zero) return;

            // Filter to *our* item, so we never run on a drying rack.
            string itemId = def.ID ?? string.Empty;
            if (itemId != Mod.CurrentConfig.StationItemId) return;

            var go = __instance.gameObject;
            var existing = go.GetComponent<SnackVendorController>();
            if (existing != null)
            {
                // Re-instantiation can re-fire Start; idempotent setup.
                return;
            }

            var ctrl = go.AddComponent<SnackVendorController>();
            ctrl.SetupAfterPlacement(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"SnackVendor BuildableItem_Start_Patch: {ex.Message}");
        }
    }
}

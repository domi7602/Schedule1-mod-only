using System;
using HarmonyLib;
using Il2CppScheduleOne.UI.Phone.ProductManagerApp;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ProductAppQoL.Mod), "ProductAppQoL", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ProductAppQoL;

/// <summary>
/// Product app quality of life: the asking-price −/+ buttons respond to mouse and touch.
///
/// Vanilla wires those buttons through ScheduleOne.GamepadInput.InputValueRamp (the
/// hold-to-accelerate gamepad adjustment), so plain mouse clicks do nothing — with
/// mouse/keyboard the price can only be typed into the field. This mod ADDS pointer
/// handling next to the untouched vanilla Button: tap = ±$1, hold = repeat with
/// acceleration (the gamepad ramp feel). Gamepad and typed input keep working as
/// before — listeners are added, no vanilla state is replaced.
/// </summary>
public class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        try
        {
            HarmonyInstance.Patch(
                AccessTools.Method(typeof(ProductAppDetailPanel), nameof(ProductAppDetailPanel.Awake)),
                postfix: new HarmonyMethod(typeof(Mod).GetMethod(
                    nameof(AwakePostfix), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] failed to patch ProductAppDetailPanel.Awake: {ex.Message}");
            return;
        }

        MelonLogger.Msg("[ProductAppQoL] Initialized — asking-price −/+ buttons work with mouse (tap = ±$1, hold = repeat).");
    }

    /// <summary>Wires one price button of the freshly awoken detail panel.</summary>
    private static void AwakePostfix(ProductAppDetailPanel __instance)
    {
        try
        {
            Wire(__instance, __instance._addButton, +1f);
            Wire(__instance, __instance._removeButton, -1f);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] wiring failed: {ex.Message}");
        }
    }

    private static void Wire(ProductAppDetailPanel panel, Transform button, float step)
    {
        if (button == null) return;
        GameObject go = button.gameObject;
        if (go.GetComponent<PriceButtonMouseInput>() != null) return;
        PriceButtonMouseInput input = go.AddComponent<PriceButtonMouseInput>();
        input.Setup(panel, button, step);
    }
}

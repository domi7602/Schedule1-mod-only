using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.UI.Phone.ProductManagerApp;
using MelonLoader;
using UnityEngine;
using GameProductManagerApp = Il2CppScheduleOne.UI.Phone.ProductManagerApp.ProductManagerApp;

[assembly: MelonInfo(typeof(ProductAppQoL.Mod), "ProductAppQoL", "0.1.1", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ProductAppQoL;

/// <summary>
/// Product app quality of life:
/// - Asking-price −/+ buttons respond to mouse and touch (tap = ±$1, hold = repeat with acceleration).
/// - Fixed search bar (matches product name or effects) and sort selector (Default, Price, Name, Market Value, Addictiveness, Listed).
/// - Rename and delete actions for player-created mixes in the detail panel.
/// </summary>
public class Mod : MelonMod
{
    private static readonly Dictionary<IntPtr, ProductToolbarController> Controllers = new();
    private static readonly Dictionary<IntPtr, ProductDetailActions> DetailActions = new();

    public override void OnInitializeMelon()
    {
        try
        {
            HarmonyInstance.Patch(
                AccessTools.Method(typeof(ProductAppDetailPanel), nameof(ProductAppDetailPanel.Awake)),
                postfix: new HarmonyMethod(GetPrivateStatic(nameof(DetailAwakePostfix))));

            HarmonyInstance.Patch(
                AccessTools.Method(typeof(ProductAppDetailPanel), nameof(ProductAppDetailPanel.SetActiveProduct)),
                postfix: new HarmonyMethod(GetPrivateStatic(nameof(DetailSetActiveProductPostfix))));

            HarmonyInstance.Patch(
                AccessTools.Method(typeof(GameProductManagerApp), nameof(GameProductManagerApp.Start)),
                postfix: new HarmonyMethod(GetPrivateStatic(nameof(AppStartPostfix))));

            HarmonyInstance.Patch(
                AccessTools.Method(typeof(GameProductManagerApp), nameof(GameProductManagerApp.SetOpen)),
                postfix: new HarmonyMethod(GetPrivateStatic(nameof(AppSetOpenPostfix))));

            HarmonyInstance.Patch(
                AccessTools.Method(typeof(GameProductManagerApp), nameof(GameProductManagerApp.CreateEntry)),
                postfix: new HarmonyMethod(GetPrivateStatic(nameof(AppListChangedPostfix))));

            HarmonyInstance.Patch(
                AccessTools.Method(typeof(GameProductManagerApp), nameof(GameProductManagerApp.CreateFavouriteEntry)),
                postfix: new HarmonyMethod(GetPrivateStatic(nameof(AppListChangedPostfix))));

            HarmonyInstance.Patch(
                AccessTools.Method(typeof(GameProductManagerApp), nameof(GameProductManagerApp.OnProductListedEvent)),
                postfix: new HarmonyMethod(GetPrivateStatic(nameof(AppListChangedPostfix))));
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] failed to apply Harmony patches: {ex.Message}");
            return;
        }

        MelonLogger.Msg("[ProductAppQoL] Initialized — price −/+ mouse input, search bar, sort controls, and mix rename/delete active.");
    }

    internal static void RefreshAllApps()
    {
        foreach (ProductToolbarController controller in Controllers.Values)
            controller.Refresh();
    }

    private static MethodInfo GetPrivateStatic(string name) =>
        typeof(Mod).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)!;

    /// <summary>Wires price buttons and detail actions of the freshly awoken detail panel.</summary>
    private static void DetailAwakePostfix(ProductAppDetailPanel __instance)
    {
        try
        {
            if (__instance == null) return;
            Wire(__instance, __instance._addButton, +1f);
            Wire(__instance, __instance._removeButton, -1f);
            GetOrCreateDetailActions(__instance).Initialize();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] detail wiring failed: {ex}");
        }
    }

    private static void DetailSetActiveProductPostfix(ProductAppDetailPanel __instance, ProductDefinition __0)
    {
        try
        {
            if (__instance == null) return;
            GetOrCreateDetailActions(__instance).OnActiveProductChanged(__0);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] detail action update failed: {ex}");
        }
    }

    private static void AppStartPostfix(GameProductManagerApp __instance)
    {
        try
        {
            if (__instance == null) return;
            ProductToolbarController controller = GetOrCreateController(__instance);
            controller.Initialize();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] toolbar setup failed: {ex}");
        }
    }

    private static void AppSetOpenPostfix(GameProductManagerApp __instance, bool open)
    {
        try
        {
            if (__instance == null) return;
            if (!open)
            {
                foreach (ProductDetailActions actions in DetailActions.Values)
                    actions.ReleaseTyping();
            }

            if (Controllers.TryGetValue(__instance.Pointer, out ProductToolbarController? controller))
                controller.OnAppOpenChanged(open);
            else if (open)
            {
                ProductToolbarController created = GetOrCreateController(__instance);
                created.Initialize();
                created.OnAppOpenChanged(true);
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] SetOpen update failed: {ex}");
        }
    }

    private static void AppListChangedPostfix(GameProductManagerApp __instance)
    {
        try
        {
            if (__instance == null) return;
            if (Controllers.TryGetValue(__instance.Pointer, out ProductToolbarController? controller))
                controller.Refresh();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] list refresh failed: {ex.Message}");
        }
    }

    private static ProductToolbarController GetOrCreateController(GameProductManagerApp app)
    {
        if (!Controllers.TryGetValue(app.Pointer, out ProductToolbarController? controller))
        {
            controller = new ProductToolbarController(app);
            Controllers[app.Pointer] = controller;
        }
        return controller;
    }

    private static ProductDetailActions GetOrCreateDetailActions(ProductAppDetailPanel panel)
    {
        if (!DetailActions.TryGetValue(panel.Pointer, out ProductDetailActions? actions))
        {
            actions = new ProductDetailActions(panel);
            DetailActions[panel.Pointer] = actions;
        }
        return actions;
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

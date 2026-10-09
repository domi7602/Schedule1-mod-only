using System;
using System.Reflection;
using HarmonyLib;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Messaging;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1Mods.Shared;

namespace MessagesPlus;

/// <summary>
/// Preserves the deal-window popup's vanilla appearance whenever it changes state.
/// The popup is excluded from app-wide dark-theme passes, and previously themed
/// colors under its explicit Container root are restored when first registered.
/// </summary>
public static class DealWindowSelectorPatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        // One shared postfix for both overloads below — identical behaviour.
        HarmonyMethod postfix = new(typeof(DealWindowSelectorPatch), nameof(SetIsOpen_Postfix));

        // The app's real open path: SetIsOpen(bool, MSGConversation, Action<EDealWindow>).
        // Found by exact reflected parameter types instead of declaring the
        // IL2CPP delegate in this patch signature; proxy delegate generic aliases
        // vary between generated assemblies.
        MethodInfo? threeArg = FindThreeArgSetIsOpen();
        if (threeArg != null)
        {
            PatchGuard.TryPatch(harmony, threeArg, postfix: postfix, log: log);
        }
        else
        {
            log.Warn("DealWindowSelectorPatch: SetIsOpen(bool, MSGConversation, Action<EDealWindow>) not found — popup will not be registered for vanilla-color preservation.");
        }

        // Defensive: the bool-only overload (no managed callers in 0.4.7f7, but
        // patched so every edge call path is covered as well).
        PatchGuard.TryPatch(
            harmony,
            typeof(DealWindowSelector),
            nameof(DealWindowSelector.SetIsOpen),
            postfix: postfix,
            parameterTypes: new[] { typeof(bool) },
            log: log);
    }

    /// <summary>Locates SetIsOpen(bool, MSGConversation, Action&lt;EDealWindow&gt;) by its shape.</summary>
    private static MethodInfo? FindThreeArgSetIsOpen()
    {
        try
        {
            foreach (MethodInfo m in typeof(DealWindowSelector).GetMethods(
                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name != nameof(DealWindowSelector.SetIsOpen)) continue;
                ParameterInfo[] ps = m.GetParameters();
                if (ps.Length != 3 || ps[0].ParameterType != typeof(bool) ||
                    ps[1].ParameterType != typeof(MSGConversation)) continue;

                Type callbackType = ps[2].ParameterType;
                if (!callbackType.IsGenericType) continue;
                Type[] callbackArguments = callbackType.GetGenericArguments();
                if (callbackArguments.Length == 1 && callbackArguments[0] == typeof(EDealWindow))
                    return m;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"DealWindowSelectorPatch: overload lookup failed: {ex.Message}");
        }

        return null;
    }

    /// <summary>Runs after both open and close calls so reused popup roots stay registered.</summary>
    [HarmonyPostfix]
    public static void SetIsOpen_Postfix(DealWindowSelector __instance, bool __0)
    {
        // "__0" instead of a named parameter — immune to future renames. Register
        // the root on close too: the same native container is commonly reused.
        Refresh(__instance);
    }

    private static void Refresh(DealWindowSelector selector)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            if (selector == null || !NetworkGuard.IsAlive(selector)) return;
            if (!NetworkGuard.IsAlive(selector.Container)) return;

            // Keep the game's own order/deal popup in its native light theme.
            AppTheme.PreserveVanillaSubtree(selector.Container);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"DealWindowSelector theme refresh failed: {ex.Message}");
        }
    }
}

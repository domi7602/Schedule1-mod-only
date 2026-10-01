using System;
using System.Reflection;
using HarmonyLib;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1Mods.Shared;

namespace MessagesPlus;

/// <summary>
/// Immediate dark-theme refresh for the deal-window popup (<see cref="DealWindowSelector"/> —
/// the Morning/Afternoon/Night/LateNight picker that opens from a conversation
/// response). Without this, the popup keeps its vanilla colours until the next
/// 1-second theme tick — the "popup flashes light for a moment" report.
///
/// The postfix runs in the same frame as the open call (before the first render)
/// and force-refreshes the popup's subtree, so colours the game re-sets while
/// opening also end up dark. Light-restore correctness is unaffected: the cached
/// originals of already-themed graphics are never overwritten.
/// </summary>
public static class DealWindowSelectorPatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        // One shared postfix for both overloads below — identical behaviour.
        HarmonyMethod postfix = new(typeof(DealWindowSelectorPatch), nameof(SetIsOpen_Postfix));

        // The app's real open path: SetIsOpen(bool, MSGConversation, Action<EDealWindow>).
        // Found by SHAPE (name + arity + first parameter) instead of naming the
        // IL2CPP delegate type in a parameter list — the proxy delegate type
        // (Il2CppSystem.Action<EDealWindow>) is not referenceable from mod code
        // (CS0305: Il2CppSystem.Action is generated as a 9-arity generic only).
        MethodInfo? threeArg = FindThreeArgSetIsOpen();
        if (threeArg != null)
        {
            PatchGuard.TryPatch(harmony, threeArg, postfix: postfix, log: log);
        }
        else
        {
            log.Warn("DealWindowSelectorPatch: SetIsOpen(bool, MSGConversation, Action<EDealWindow>) not found — popup hook skipped (the 1 s tick stays as fallback).");
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
                if (ps.Length == 3 && ps[0].ParameterType == typeof(bool))
                    return m;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"DealWindowSelectorPatch: overload lookup failed: {ex.Message}");
        }

        return null;
    }

    [HarmonyPostfix]
    public static void SetIsOpen_Postfix(DealWindowSelector __instance, bool __0)
    {
        // "__0" instead of a named parameter — immune to future renames.
        if (!__0) return; // closing — nothing to theme
        Refresh(__instance);
    }

    private static void Refresh(DealWindowSelector selector)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            if (selector == null || !NetworkGuard.IsAlive(selector)) return;

            AppTheme.ApplyToSubtree(selector.gameObject);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"DealWindowSelector theme refresh failed: {ex.Message}");
        }
    }
}

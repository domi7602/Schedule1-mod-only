using System;
using HarmonyLib;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1Mods.Shared;

namespace MessagesPlus;

/// <summary>
/// Harmony patches on the vanilla <see cref="MessagesApp"/> (patch-only mod —
/// MoreSaveSlots-style: we inject into the existing app UI instead of creating
/// a new PhoneApp). Every patch goes through PatchGuard.TryPatch so a game
/// update that renames a method degrades gracefully instead of crashing.
///
/// Patched methods:
///   Start            — inject the MessagesPlus toolbar + confirmation modal (once per app instance).
///   SetOpen(bool)    — re-inject when the app is opened (covers page rebuilds); on close,
///                      reset the modal and the search/filter view (W12 analogue).
///   Loaded()         — re-inject after the game loaded the conversations.
///
/// The v0.1.x CreateConversationUI patch is gone —
/// v0.2.0+ has no per-entry UI at all.
/// </summary>
public static class MessagesAppPatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        PatchGuard.TryPatch(
            harmony,
            typeof(MessagesApp),
            nameof(MessagesApp.Start),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(Start_Postfix)),
            log: log);

        PatchGuard.TryPatch(
            harmony,
            typeof(MessagesApp),
            nameof(MessagesApp.SetOpen),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(SetOpen_Postfix)),
            log: log);

        PatchGuard.TryPatch(
            harmony,
            typeof(MessagesApp),
            "Loaded",
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(Loaded_Postfix)),
            log: log);

        // Conversation switch: theme in the SAME frame the selection runs, so the
        // freshly shown chat surfaces never sit light until the next 1 s tick (the
        // "white artifacts while switching messages" report, 2026-10-03).
        PatchGuard.TryPatch(
            harmony,
            typeof(MessagesApp),
            nameof(MessagesApp.SetCurrentConversation),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(ConversationSwitched_Postfix)),
            log: log);
    }

    /// <summary>
    /// Injects the MessagesPlus UI into the freshly created vanilla app page.
    /// </summary>
    [HarmonyPostfix]
    public static void Start_Postfix(MessagesApp __instance)
    {
        try
        {
            InboxUI.EnsureBuilt(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Start_Postfix failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Re-inject on every app open (cheap: EnsureBuilt is a no-op when the
    /// injected UI is still alive). On close, reset the confirmation modal and
    /// the search/filter view so neither can reappear with stale state (W12).
    /// </summary>
    [HarmonyPostfix]
    public static void SetOpen_Postfix(MessagesApp __instance, bool __0)
    {
        // S6: "__0" instead of a named parameter — immune to future renames of
        // MessagesApp.SetOpen's parameter.
        if (!__0)
        {
            try
            {
                InboxUI.OnAppClosed();
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"SetOpen_Postfix (close) failed: {ex.Message}");
            }
            return;
        }
        try
        {
            InboxUI.EnsureBuilt(__instance);
            InboxUI.RequestOpenRefresh(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"SetOpen_Postfix failed: {ex.Message}");
        }
    }

    /// <summary>
    /// The vanilla app finished loading its conversations from the save — re-inject
    /// the UI in case the page was rebuilt.
    /// </summary>
    [HarmonyPostfix]
    public static void Loaded_Postfix(MessagesApp __instance)
    {
        try
        {
            InboxUI.EnsureBuilt(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Loaded_Postfix failed: {ex.Message}");
        }
    }

    /// <summary>
    /// A conversation was selected: theme immediately (same frame) and keep a few
    /// unthrottled follow-up passes, because the vanilla UI swaps the chat surfaces
    /// in over the following frames (the selection is deferred). Without this the
    /// switch shows vanilla (light) bubbles until the next 1 s tick (2026-10-03).
    /// </summary>
    [HarmonyPostfix]
    public static void ConversationSwitched_Postfix(MessagesApp __instance)
    {
        try
        {
            InboxUI.RequestConversationRefresh(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ConversationSwitched_Postfix failed: {ex.Message}");
        }
    }
}

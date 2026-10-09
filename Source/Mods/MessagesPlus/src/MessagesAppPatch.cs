using System;
using HarmonyLib;
using Il2CppScheduleOne.Messaging;
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
///   MSGConversation.CreateUI/RenderMessage/RenderPlayerMessage
///                    — refresh the affected conversation's known surfaces.
///   CreateResponseUI — register the vanilla response subtree without recoloring it.
///   Start           — inject the MessagesPlus toolbar + confirmation modal (once per app instance).
///   SetOpen(bool)   — re-inject when the app is opened; on close, reset the modal and search/filter view.
///   Loaded()        — one-time legacy restore (idempotent) + re-inject after conversations load.
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
            typeof(MSGConversation),
            nameof(MSGConversation.CreateUI),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(ConversationUI_Postfix)),
            log: log);

        PatchGuard.TryPatch(
            harmony,
            typeof(MSGConversation),
            nameof(MSGConversation.RenderMessage),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(ConversationUI_Postfix)),
            parameterTypes: new[] { typeof(Message) },
            log: log);

        PatchGuard.TryPatch(
            harmony,
            typeof(MSGConversation),
            nameof(MSGConversation.RenderPlayerMessage),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(ConversationUI_Postfix)),
            parameterTypes: new[] { typeof(SendableMessage) },
            log: log);

        PatchGuard.TryPatch(
            harmony,
            typeof(MSGConversation),
            nameof(MSGConversation.CreateResponseUI),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(ResponseUI_Postfix)),
            parameterTypes: new[] { typeof(Response) },
            log: log);

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
    }

    [HarmonyPostfix]
    public static void ConversationUI_Postfix(MSGConversation __instance)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            AppTheme.RefreshConversation(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"ConversationUI_Postfix failed: {ex.Message}");
        }
    }

    [HarmonyPostfix]
    public static void ResponseUI_Postfix(MSGConversation __instance)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            AppTheme.PreserveConversationResponseArea(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"ResponseUI_Postfix failed: {ex.Message}");
        }
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
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"SetOpen_Postfix failed: {ex.Message}");
        }
    }

    /// <summary>
    /// The vanilla app finished loading its conversations from the save — run
    /// the (idempotent) legacy restore now that the conversation lists are
    /// populated (OnSaveInfoLoaded fires earlier and defers), then re-inject
    /// the UI in case the page was rebuilt.
    /// </summary>
    [HarmonyPostfix]
    public static void Loaded_Postfix(MessagesApp __instance)
    {
        try
        {
            LegacyRestore.Run();
            InboxUI.EnsureBuilt(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Loaded_Postfix failed: {ex.Message}");
        }
    }
}

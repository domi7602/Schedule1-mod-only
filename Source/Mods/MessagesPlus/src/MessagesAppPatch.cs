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
///   Start            — inject the toolbar buttons + trash section (once per app instance).
///   SetOpen(bool)    — re-inject/refresh when the app is opened (covers page rebuilds).
///   Loaded()         — re-apply trash/purge state after the game loaded conversations.
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

        PatchGuard.TryPatch(
            harmony,
            typeof(MessagesApp),
            nameof(MessagesApp.CreateConversationUI),
            postfix: new HarmonyMethod(typeof(MessagesAppPatch), nameof(CreateConversationUI_Postfix)),
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
            TrashUI.EnsureBuilt(__instance);
            TrashService.ApplyToConversations();
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Start_Postfix failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Refresh + re-inject on every app open (cheap: EnsureBuilt is a no-op when
    /// the injected UI is still alive). On close, reset the confirmation modal so
    /// a half-finished dialog cannot reappear with a stale pending action.
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
                TrashUI.ResetModal(); // W12: modal must not survive an app close.
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"SetOpen_Postfix (close) failed: {ex.Message}");
            }
            return;
        }
        try
        {
            TrashUI.EnsureBuilt(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"SetOpen_Postfix failed: {ex.Message}");
        }
    }

    /// <summary>
    /// The vanilla app finished loading its conversations from the save —
    /// re-apply trashed (hidden) and purged (removed) state so the trash
    /// survives game reloads.
    /// </summary>
    [HarmonyPostfix]
    public static void Loaded_Postfix(MessagesApp __instance)
    {
        try
        {
            TrashService.ApplyToConversations();
            TrashUI.EnsureBuilt(__instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Loaded_Postfix failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Injects a small per-entry delete button (🗑) into every conversation
    /// row created by the vanilla app. Clicking it moves just that thread
    /// to the trash (same as Clear All but for one conversation).
    /// </summary>
    [HarmonyPostfix]
    public static void CreateConversationUI_Postfix(MessagesApp __instance, MSGConversation c, ref RectTransform entry)
    {
        try
        {
            if (entry == null) return;
            TrashUI.InjectEntryDeleteButton(entry, c, __instance);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CreateConversationUI_Postfix failed: {ex.Message}");
        }
    }
}

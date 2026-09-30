using System;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(MessagesPlus.Mod), "MessagesPlus", "0.4.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MessagesPlus;

/// <summary>
/// MessagesPlus — inbox hygiene for the vanilla MessagesApp: customer-only
/// Clear All + Clear Read, live name search, category filter chips and an
/// unread counter, plus the one-time legacy restore.
///
/// Patch-only mod (plain MelonMod + Harmony, like MoreSaveSlots/StackLimitMod):
/// it enhances the EXISTING in-game Messages app and never registers a new
/// PhoneApp or homescreen icon. See MessagesAppPatch/InboxUI/InboxView/LegacyRestore.
/// </summary>
public class Mod : MelonMod
{
    /// <summary>Shared logger for the mod (wired in <see cref="OnInitializeMelon"/>).</summary>
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Log = new ModLogger("MessagesPlus");

        // 0. IL2CPP registration for the search field's InputFocus guard (Key Rule 5).
        try { ClassInjector.RegisterTypeInIl2Cpp<MessagesPlusInputFocus>(); }
        catch (Exception ex) { Log.Warn($"Failed to register MessagesPlusInputFocus: {ex.Message}"); }

        // 1. Config (Phase 2 toast/sound + Phase 3 background placeholders).
        ModConfig<MessagesPlusConfig>.Initialize("MessagesPlus", Log);

        // 2. Harmony patches on the vanilla MessagesApp (PatchGuard = graceful
        //    degradation if a game update renames a method).
        MessagesAppPatch.ApplyAll(HarmonyInstance, Log);
        PatchGuard.Report(Log);

        // 3. Save-load timing: OnSaveInfoLoaded fires after save parsing but
        //    before scene build (OnGameplaySceneLoaded is too early/late — see
        //    IL2CPP pitfalls, save-load timing). The MessagesApp.Loaded postfix
        //    retries LegacyRestore.Run once the conversation lists are
        //    populated (Run is idempotent).
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;

        Log.Info("MessagesPlus v0.4.0 initialized (search band + category chips + unread counter + ... menu with Clear Read/All + whole-app dark mode + legacy restore).");
    }

    /// <summary>
    /// Single throttled dispatcher (1 s, see InboxUI.Tick) for the injected UI:
    /// re-applies the search/filter view (vanilla can re-show entries on its own
    /// events — the v0.1.x TrashUI W5 lesson) and refreshes the unread counter.
    /// Free when the phone is closed or no view is active.
    /// </summary>
    public override void OnUpdate()
    {
        InboxUI.Tick();
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase))
        {
            // UI references die with the scene — drop them (never Destroy here).
            InboxUI.TearDownForSceneUnload();
            AppTheme.HandleSceneUnload();
        }
    }

    private void OnSaveInfoLoaded()
    {
        try
        {
            LegacyRestore.Run();
        }
        catch (Exception ex)
        {
            Log.Error($"OnSaveInfoLoaded handler failed: {ex.Message}");
        }
    }
}

using System;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(MessagesPlus.Mod), "MessagesPlus", "0.1.1", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MessagesPlus;

/// <summary>
/// MessagesPlus — Phase 1: Clear All + Trash/Restore for the vanilla MessagesApp.
///
/// Patch-only mod (plain MelonMod + Harmony, like MoreSaveSlots/StackLimitMod):
/// it enhances the EXISTING in-game Messages app and never registers a new
/// PhoneApp or homescreen icon. See MessagesAppPatch/TrashUI/TrashService.
/// </summary>
public class Mod : MelonMod
{
    /// <summary>Shared logger for the mod (wired in <see cref="OnInitializeMelon"/>).</summary>
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Log = new ModLogger("MessagesPlus");

        // 1. Config (Phase 3 background colors + Phase 2 toast/sound toggles).
        ModConfig<MessagesPlusConfig>.Initialize("MessagesPlus", Log);

        // 2. Trash service + persistence.
        TrashService.Initialize(Log);

        // 3. Harmony patches on the vanilla MessagesApp (PatchGuard = graceful
        //    degradation if a game update renames a method).
        MessagesAppPatch.ApplyAll(HarmonyInstance, Log);
        PatchGuard.Report(Log);

        // 4. Save-load timing: OnSaveInfoLoaded fires after save parsing but
        //    before scene build (OnGameplaySceneLoaded is too early/late — see
        //    IL2CPP pitfalls, save-load timing).
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;

        // 5. Single static update dispatcher (defensive Unsubscribe-before-
        //    Subscribe; NEVER unsubscribe per phone close — Rule 10 analogue).
        MelonEvents.OnUpdate.Unsubscribe(OnModUpdate);
        MelonEvents.OnUpdate.Subscribe(OnModUpdate);

        Log.Info("MessagesPlus v0.1.1 initialized (Clear All + Trash/Restore for MessagesApp).");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        MelonEvents.OnUpdate.Unsubscribe(OnModUpdate);
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase))
        {
            // UI references die with the scene — drop them (never Destroy here).
            TrashUI.TearDownForSceneUnload();
        }
    }

    private void OnSaveInfoLoaded()
    {
        try
        {
            TrashService.LoadForCurrentSlot();
            TrashService.ApplyToConversations();
        }
        catch (Exception ex)
        {
            Log.Error($"OnSaveInfoLoaded handler failed: {ex.Message}");
        }
    }

    private void OnModUpdate()
    {
        // Throttled UI sync (1s inside TrashUI.Tick) — counter/rows stay fresh
        // even when the vanilla app rebuilds its conversation list.
        TrashUI.Tick();
    }
}

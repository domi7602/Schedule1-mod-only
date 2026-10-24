using HitmanPhone.Bounty;
using HitmanPhone.Items;
using HitmanPhone.Persistence;
using HarmonyLib;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using System;
using System.Reflection;

#if (IL2CPPMELON)
using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
using S1SaveManager = Il2CppScheduleOne.Persistence.SaveManager;
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1NPCHealth = Il2CppScheduleOne.NPCs.NPCHealth;
#elif MONOMELON
using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
using S1SaveManager = ScheduleOne.Persistence.SaveManager;
using S1NPC = ScheduleOne.NPCs.NPC;
using S1NPCHealth = ScheduleOne.NPCs.NPCHealth;
#endif

[assembly: MelonInfo(typeof(HitmanPhone.Mod), "HitmanPhone", "0.3.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace HitmanPhone;

/// <summary>
/// Phase A — Mod-Foundation.
/// Phase B — Polaroid registration.
/// Phase C — NPCDeathPatch.
/// Phase D — Caller scheduling.
/// Phase F — DeadDrop validation + reward payout.
/// Phase G — Heat orchestration (Police Pursuit integration via S1API.Law).
/// Phase H — Journal integration + Player-Death Watchdog.
/// Phase I — Contract auto-expiry (3-day DeadlineDay check per save-day).
/// Phase J — Slot-Load Detection (Skill Rule #16).
/// </summary>
public class Mod : MelonMod
{
    internal static Mod Instance { get; private set; } = null!;
    internal static ModLogger Log { get; } = new("HitmanPhone");

    /// <summary>
    /// The persistence state is exposed as a public set so other mod-internal classes
    /// (SaveStateGuard, BountyReceiptService) can update it on slot switches and
    /// storage events. The Saveable persistence itself is handled via the
    /// <see cref="BountyPersistence"/> static class.
    /// </summary>
    public BountySaveData Save { get; set; } = new();

    public override void OnInitializeMelon()
    {
        Instance = this;
        // v0.1.2: report the real mod version instead of a hardcoded string.
        // v0.1.3: read it from MelonInfo — the assembly version is unrelated
        // (defaults to 1.0.0 and is not touched by the bump script).
        string version = GetType().Assembly.GetCustomAttribute<MelonInfoAttribute>()?.Version ?? "?";
        Log.Info($"initialized (v{version}) — Phase J: slot-switch detection enabled.");

        // Definitions are verified in the Main/pre-load lifecycle, before gameplay.
        // Initialization alone is too early for the native registry/assets.

        // Audit (2026-09-10): explicit PatchGuard patch for S1NPC.OnDie — the old
        // blind PatchAll was all-or-nothing and invisible to PatchGuard stats.
        // parameterTypes: Type.EmptyTypes pins the parameterless overload, same
        // resolution as NPCDeathPatch.TargetMethod; a signature drift now logs
        // a PatchGuard warning instead of silently disabling every patch.
        PatchGuard.TryPatch(HarmonyInstance, typeof(S1NPC), "OnDie",
            prefix: null,
            postfix: new HarmonyMethod(typeof(NPCDeathPatch), nameof(NPCDeathPatch.Postfix)),
            parameterTypes: Type.EmptyTypes,
            log: Log);

        // Phase C2 (2026-09-13): NPCHealth.Die is the actual state transition behind the
        // dead flag (IsDead = true, onDie emitted). NPC.OnDie (the network/damage event)
        // does NOT fire for every death path — finishing a knocked-out target or drowning
        // passes through Health.Die() while NPC.OnDie can be missed. Patching the state
        // transition makes every genuine kill count, incl. "KO'd then finished".
        PatchGuard.TryPatch(HarmonyInstance, typeof(S1NPCHealth), "Die",
            prefix: null,
            postfix: new HarmonyMethod(typeof(NPCDeathPatch), nameof(NPCDeathPatch.PostfixHealthDie)),
            parameterTypes: Type.EmptyTypes,
            log: Log);

        // Phase C3: NPCHealth.KnockOut fires when NPCs lose consciousness from melee/punches.
        PatchGuard.TryPatch(HarmonyInstance, typeof(S1NPCHealth), "KnockOut",
            prefix: null,
            postfix: new HarmonyMethod(typeof(NPCDeathPatch), nameof(NPCDeathPatch.PostfixHealthKnockOut)),
            parameterTypes: Type.EmptyTypes,
            log: Log);

        // v0.1.7: dead-drop storage hooks go through PatchGuard (house standard).
        // v0.1.6 lesson: a hook that silently never fires is indistinguishable from
        // a patch that was never applied — PatchGuard logs found/missing/applied per
        // hook, so the startup log proves the wiring.
        // The dead-drop UI writes slots via the FishNet RPC write paths
        // (SetStoredInstance_Internal / SetItemSlotQuantity_Internal); ContentsChanged
        // never fires for dead drops (no Open(), so UpdateWhileOpen never runs).
        PatchGuard.TryPatch(HarmonyInstance, typeof(S1StorageEntity), "ContentsChanged",
            postfix: new HarmonyMethod(typeof(DeadDropPatch), nameof(DeadDropPatch.PostfixContentsChanged)), log: Log);
        PatchGuard.TryPatch(HarmonyInstance, typeof(S1StorageEntity), "SetStoredInstance_Internal",
            postfix: new HarmonyMethod(typeof(DeadDropPatch), nameof(DeadDropPatch.PostfixSetStoredInstance)), log: Log);
        PatchGuard.TryPatch(HarmonyInstance, typeof(S1StorageEntity), "SetItemSlotQuantity_Internal",
            postfix: new HarmonyMethod(typeof(DeadDropPatch), nameof(DeadDropPatch.PostfixSetItemSlotQuantity)), log: Log);

        PatchGuard.TryPatch(HarmonyInstance, typeof(S1SaveManager), "Save",
            prefix: new HarmonyMethod(typeof(NativeSaveCheckpointPatch), nameof(NativeSaveCheckpointPatch.PrefixDefault)),
            parameterTypes: Type.EmptyTypes, log: Log);
        PatchGuard.TryPatch(HarmonyInstance, typeof(S1SaveManager), "Save",
            prefix: new HarmonyMethod(typeof(NativeSaveCheckpointPatch), nameof(NativeSaveCheckpointPatch.Prefix)),
            parameterTypes: new[] { typeof(string) }, log: Log);
        SaveStateGuard.TrySubscribeLifecycle();
#if DEBUG
        BountyTestCommands.Register();
#endif
    }

    public override void OnDeinitializeMelon()
    {
        SaveStateGuard.TryUnsubscribeLifecycle();
        HarmonyInstance.UnpatchSelf();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        if (string.Equals(sceneName, "Main", StringComparison.OrdinalIgnoreCase))
            BountyEvidenceItemRegistry.OnMainSceneLoaded();
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (string.Equals(sceneName, "Main", StringComparison.OrdinalIgnoreCase))
        {
            SaveStateGuard.Suspend();
            BountyJournalBridge.ResetSessionCache();
            BountyTrackerApp.TearDownForSceneUnload();
        }
    }

    /// <summary>
    /// Phase D — fire the bounty-call scheduler (day-gated).
    /// Phase G — immediate Lethal pursuit after a matched kill.
    /// Phase H — Player-Death Watchdog (1Hz throttled).
    /// Phase I — contract auto-expiry (day-gated).
    /// Each subsystem self-throttles; OnUpdate is cheap.
    /// </summary>
    public override void OnUpdate()
    {
        SaveStateGuard.Tick();
        BountyEvidenceItemRegistry.TickRegistration();
        if (!SaveStateGuard.IsReady)
        {
            if (NetworkGuard.IsInMainScene && !SaveStateGuard.IsSingleplayer)
                BountyConversationRouter.NotifyMultiplayerUnsupportedOnce();
            return;
        }
        try
        {
            BountyCallScheduler.Tick();
            HitmanPhone.Bounty.PlayerDeathWatchdog.Tick();
            BountyExpiryService.Tick();
            BountyTargetWatchdog.Tick();
            BountyJournalBridge.TickRebind();
        }
        catch (Exception ex)
        {
            // Audit L9 (2026-09-01): full exception text (stack included) — a bare
            // ex.Message made tick-loop failures undiagnosable.
            Log.Warn($"Mod.OnUpdate tick exception (swallowed): {ex}");
        }
    }
}

internal static class NativeSaveCheckpointPatch
{
    public static void PrefixDefault()
    {
        try { BountyPersistence.CaptureDefaultNativeSavePath(); }
        catch (Exception ex) { Mod.Log.Error($"[checkpoint] default native save-path capture failed: {ex}"); }
    }

    public static void Prefix(string __0)
    {
        try { BountyPersistence.CaptureNativeSavePath(__0); }
        catch (Exception ex) { Mod.Log.Error($"[checkpoint] native save-path capture failed: {ex}"); }
    }
}

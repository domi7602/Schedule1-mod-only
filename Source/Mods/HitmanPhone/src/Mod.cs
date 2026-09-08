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
#elif MONOMELON
using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
#endif

[assembly: MelonInfo(typeof(HitmanPhone.Mod), "HitmanPhone", "0.2.1", "Dominik")]
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
        Log.Info($"initialisiert (v{version}) — Phase J: slot-switch detection enabled.");

        // Polaroid registration is intentionally lazy on first Spawn() — the native
        // Registry.Instance and asset pipelines aren't fully online at OnInitializeMelon
        // time, so eager registration fails; lazy-then-cache works on every spawn thereafter.

        try
        {
            HarmonyInstance.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            Log.Info("Harmony PatchAll done (NPCDeathPatch via TargetMethod).");
        }
        catch (Exception ex)
        {
            Log.Error($"Harmony patch application failed: {ex}");
        }

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

        SaveStateGuard.TrySubscribeLifecycle();
        BountyTestCommands.Register();
    }

    public override void OnDeinitializeMelon()
    {
        SaveStateGuard.TryUnsubscribeLifecycle();
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

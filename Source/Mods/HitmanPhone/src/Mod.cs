using HitmanPhone.Bounty;
using HitmanPhone.Items;
using HitmanPhone.Persistence;
using HarmonyLib;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using System;
using System.Reflection;
using UnityEngine;

using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1NPCHealth = Il2CppScheduleOne.NPCs.NPCHealth;
using S1Time = Il2CppScheduleOne.GameTime.TimeManager;

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

        // Polaroid registration is intentionally lazy on first Spawn() — the native
        // Registry.Instance and asset pipelines aren't fully online at OnInitializeMelon
        // time, so eager registration fails; lazy-then-cache works on every spawn thereafter.

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

        SaveStateGuard.TrySubscribeLifecycle();
#if DEBUG
        BountyTestCommands.Register();
#endif
    }

    public override void OnDeinitializeMelon()
    {
        SaveStateGuard.TryUnsubscribeLifecycle();
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (string.Equals(sceneName, "Main", StringComparison.OrdinalIgnoreCase))
        {
            BountyJournalBridge.ResetSessionCache();
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
            // Audit L9 (2026-09-01): full exception text (stack included) - a bare
            // ex.Message made tick-loop failures undiagnosable.
            Log.Warn($"Mod.OnUpdate tick exception (swallowed): {ex}");
        }
    }
}

// ---------------------------------------------------------------------------
// Merged from HitmanPhoneTime.cs (2026-10-02) — cached in-game day queries.
// ---------------------------------------------------------------------------

/// <summary>
/// Wrappers around Schedule I's <see cref="S1Time"/> for hitman-phase time
/// queries. Keeps reflection in one place so callers can ask for the current
/// in-game day without learning the stub layout.
/// </summary>
public static class HitmanPhoneTime
{
    // Audit L3 (2026-09-01): CurrentDay is called from every scheduler tick —
    // resolve the member infos ONCE instead of per call (per-frame reflection
    // churn). The IL2CPP stub type never changes within a session.
    private static PropertyInfo? _elapsedDaysProp;
    private static FieldInfo? _elapsedDaysField;
    private static bool _membersResolved;

    // Audit (2026-09-10): CurrentDay is called from every scheduler/expiry tick
    // (i.e. every OnUpdate frame) — cache the resolved day and refresh at most
    // once per second so the hot loop avoids interop reflection + boxing per
    // frame (same _nextTick throttle shape as BountyTargetWatchdog). No
    // OnDayPass-style event exists in this mod to hook instead; day-granular
    // bounty deadlines tolerate a 1s-stale day. First call resolves immediately.
    private static int _cachedDay;
    private static float _nextRefreshRealtime;
    private static bool _hasCachedDay;

    private static void ResolveMembers()
    {
        if (_membersResolved) return;
        _membersResolved = true;
        try
        {
            var type = typeof(S1Time);
            _elapsedDaysProp = type.GetProperty("ElapsedDays",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _elapsedDaysField = type.GetField("_ElapsedDays_k__BackingField",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"HitmanPhoneTime member resolution failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Elapsed in-game days (whole days). Falls back to 0 when TimeManager is
    /// not yet initialised (e.g. before the player enters the gameplay scene).
    /// Throttled: the interop read runs at most once per second, otherwise the
    /// cached value is returned.
    /// </summary>
    public static int CurrentDay()
    {
        try
        {
            float now = Time.realtimeSinceStartup;
            if (_hasCachedDay && now < _nextRefreshRealtime) return _cachedDay;
            int day = ResolveDayUncached();
            _cachedDay = day;
            _hasCachedDay = true;
            _nextRefreshRealtime = now + 1.0f;
            return day;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"HitmanPhoneTime.CurrentDay failed: {ex.Message}");
            return _hasCachedDay ? _cachedDay : 0;
        }
    }

    /// <summary>
    /// Uncached interop read of ElapsedDays (property, else backing field).
    /// </summary>
    private static int ResolveDayUncached()
    {
        try
        {
            var tm = S1Time.Instance;
            if (tm == null) return 0;
            ResolveMembers();
            // ElapsedDays is exposed via property in newer patches; the backing
            // field is the fallback.
            if (_elapsedDaysProp != null && _elapsedDaysProp.CanRead)
            {
                var v = _elapsedDaysProp.GetValue(tm);
                if (v is int d) return d;
            }
            if (_elapsedDaysField != null)
            {
                var v = _elapsedDaysField.GetValue(tm);
                if (v is int d2) return d2;
            }
            return 0;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"HitmanPhoneTime.ResolveDayUncached failed: {ex.Message}");
            return 0;
        }
    }
}

using System;
using System.Threading;
using HitmanPhone.Bounty;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

#if (IL2CPPMELON)
using S1Persistence = Il2CppScheduleOne.Persistence;
#elif MONOMELON
using S1Persistence = ScheduleOne.Persistence;
#endif

namespace HitmanPhone;

/// <summary>
/// Phase J — Slot-Load Detection (Skill Rule #16).
///
/// <c>GameLifecycle.OnPreLoad</c> fires for **both** real save-slot switches
/// AND same-slot Menu→Game scene reloads. A naive
/// <c>Save = BountyPersistence.Load()</c> in <c>OnPreLoad</c> would wipe placed
/// items on every menu-return. We cache the last-known slot number and only
/// trigger a full reload when the slot actually changed.
///
/// Sentinel value <c>-2</c> means "never initialised" — the very first load
/// fires a full reload so that the player sees their previous bounty state on
/// save-game start.
/// </summary>
public static class SaveStateGuard
{
    private const string ActiveSlotKey = "hitman_active_slot";

    /// <summary>Cached last-known save slot. <c>-2</c> = "uninitialised".</summary>
    private static int _lastKnownSlot = -2;

    /// <summary>Count of times a real slot switch was detected.</summary>
    private static int _slotSwitches;
    /// <summary>Count of times a same-slot menu reload was detected.</summary>
    private static int _sameSlotReloads;

    public static int SlotSwitches => Volatile.Read(ref _slotSwitches);
    public static int SameSlotReloads => Volatile.Read(ref _sameSlotReloads);

    /// <summary>
    /// Subscribe to the GameLifecycle hooks. Idempotent.
    /// </summary>
    public static void TrySubscribeLifecycle()
    {
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
        LogSubscriptionsOnce();
    }

    /// <summary>
    /// Unsubscribe — called from Mod.OnDeinitializeMelon.
    /// </summary>
    public static void TryUnsubscribeLifecycle()
    {
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
    }

    private static bool _subscribedLogged;
    private static void LogSubscriptionsOnce()
    {
        if (_subscribedLogged) return;
        _subscribedLogged = true;
        Mod.Log.Debug("SaveStateGuard subscribed to GameLifecycle hooks.");
    }

    /// <summary>
    /// Called by <see cref="GameLifecycle"/> just before load. Resolve the new slot
    /// before it actually loads so we can compare with our cached value.
    /// </summary>
    private static void OnPreLoad()
    {
        int newSlot = ResolveActiveSlotSafe();
        int oldSlot = _lastKnownSlot;

        if (oldSlot == -2)
        {
            // First-time init: do a full reload so the player sees their previous
            // bounty state on save-game start.
            TriggerFullReload(newSlot, reason: "init");
            _lastKnownSlot = newSlot;
            return;
        }

        if (newSlot == -1)
        {
            // Audit L8 (2026-09-01): never cache -1 and never reload on it — a
            // permanently-unready LoadManager would otherwise wedge the guard in
            // "same slot" forever after one init reload. OnSaveInfoLoaded
            // reconciles the cache once the real slot is known.
            Mod.Log.Debug("[SaveStateGuard] LoadManager not ready — preload skipped, waiting for OnSaveInfoLoaded.");
            return;
        }

        if (newSlot != oldSlot)
        {
            // Real slot switch: cache old, do a full reload.
            Mod.Log.Info($"[SaveStateGuard] Slot switch detected: {oldSlot} → {newSlot}. " +
                         $"Full save reload.");
            TriggerFullReload(newSlot, reason: "slot_switch");
            Interlocked.Increment(ref _slotSwitches);
            _lastKnownSlot = newSlot;
        }
        else
        {
            // Same slot, Menu → Game scene reload: don't touch Save state. We just
            // need to reset the death-watchdog so a corpse-load doesn't fire a
            // spurious rising-edge (Phase H).
            Mod.Log.Debug($"[SaveStateGuard] Same-slot reload ({newSlot}) — keeping bounty state.");
            Interlocked.Increment(ref _sameSlotReloads);
            PlayerDeathWatchdog.Reset();
        }
    }

    /// <summary>
    /// Called once the load completed. Used to refresh Save to the new slot's contents.
    /// </summary>
    private static void OnLoadComplete()
    {
        // BUGFIX (2026-08-31): every completed load gets fresh runtime state.
        // The journal bridge's session dictionary must go (restored quests are
        // new objects) and the persisted EvidenceSpawned latch is released so
        // contracts whose polaroid was lost become completable again.
        Bounty.BountyJournalBridge.ResetSessionCache();
        Bounty.BountyService.ResetEvidenceFlags();
        // Audit M2 (2026-09-01): persisted Unity InstanceIDs are dead after a
        // restart and can randomly collide with a DIFFERENT npc's fresh id —
        // zero them so only this session's kills (re)stamp them.
        Bounty.BountyService.InvalidatePersistedInstanceIds();
        // Audit M1 (2026-09-01): drop realtimeSinceStartup grace keys old saves
        // still carry — the grace window is in-memory only now.
        Bounty.BountyHeatService.PurgePersistedHeatKeys();
    }

    /// <summary>
    /// Save-data updated: re-read the file. The engine fires this on real save-info
    /// changes (e.g. user slots into a save) which is the cleanest place to react.
    /// </summary>
    private static void OnSaveInfoLoaded()
    {
        if (Mod.Instance == null) return;
        Mod.Instance.Save = BountyPersistence.Load();
        int slot = ResolveActiveSlotSafe();
        _lastKnownSlot = slot == -1 ? _lastKnownSlot : slot;
    }

    /// <summary>
    /// Force a full bounty-state reload from disk. Called when a real slot switch is
    /// detected so that active contracts from the previous slot don't bleed through.
    /// </summary>
    private static void TriggerFullReload(int newSlot, string reason)
    {
        if (Mod.Instance == null) return;
        var prev = Mod.Instance.Save;
        Mod.Log.Info($"[SaveStateGuard] Full reload ({reason}). " +
                     $"Active contract count before: {prev?.Active.Count ?? -1}.");
        Mod.Instance.Save = BountyPersistence.Load();
        int after = Mod.Instance.Save.Active.Count;
        Mod.Log.Info($"[SaveStateGuard] Reload done. Active contract count after: {after}.");

        // Reset subsystems so we don't carry over runtime state from the previous slot.
        PlayerDeathWatchdog.Reset();
        BountyHeatService.ResetActiveCounters();
        // Audit M5 (2026-09-01): day latches must not survive a slot switch —
        // otherwise the new save gets no offer and no expiry check on day one.
        BountyCallScheduler.ResetDayLatch();
        BountyExpiryService.ResetDayLatch();
    }

    /// <summary>
    /// Ask the LoadManager for the active save slot. Returns -1 if it isn't ready yet.
    /// We swallow all exceptions because the load manager is fussy during transitions.
    /// </summary>
    private static int ResolveActiveSlotSafe()
    {
        try
        {
            var lm = S1Persistence.LoadManager.Instance;
            if (lm == null) return -1;
            var info = lm.ActiveSaveInfo;
            if (info == null) return -1;
            return info.SaveSlotNumber;
        }
        catch
        {
            return -1;
        }
    }
}

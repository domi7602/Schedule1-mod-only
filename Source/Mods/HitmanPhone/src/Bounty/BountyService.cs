using System;
using System.Threading;
using HitmanPhone.Items;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Entities;
using UnityEngine;

#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1PlayerInventory = Il2CppScheduleOne.PlayerScripts.PlayerInventory;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
using S1PlayerInventory = ScheduleOne.PlayerScripts.PlayerInventory;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase C — Kill-Detection &amp; Polaroid Spawn.
///
/// On every <see cref="S1NPC"/>.OnDie() (Harmony-Postfix), we check whether any
/// active bounty matches the dying NPC. If yes:
///   • Spawn a polaroid instance pinned to that NPC's instance id
///   • Hand it to the local player's <see cref="S1PlayerInventory"/> via the
///     player's MonoBehaviour component.
///
/// Pitfalls (Spec §3):
///   • OnDie may fire more than once (network re-sync) — guarded by Contract.State.
///   • OnDie may never fire if the NPC was destroyed via engine reset instead of damage.
/// </summary>
public static class BountyService
{
    private static int _deathsObserved;
    private static int _matchesFound;
    private static int _polaroidsSpawned;
    private static int _earlyOuts;
    private static float _lastSpawnFailWarn = -999f;

    public static int DeathsObserved => Volatile.Read(ref _deathsObserved);
    public static int MatchesFound => Volatile.Read(ref _matchesFound);
    public static int PolaroidsSpawned => Volatile.Read(ref _polaroidsSpawned);
    public static int EarlyOuts => Volatile.Read(ref _earlyOuts);

    public static void OnNpcDied(S1NPC npc)
    {
        Interlocked.Increment(ref _deathsObserved);

        if (npc == null) return;
        if (Mod.Instance == null || Mod.Instance.Save == null) return;

        var save = Mod.Instance.Save;

        // Find an Active contract whose TargetNpcId matches this NPC's ID, OR whose
        // already-cached TargetNpcInstanceId matches the runtime InstanceID.
        BountyContract? match = null;
        for (int i = 0; i < save.Active.Count; i++)
        {
            var c = save.Active[i];
            if (c.Status != EBountyStatus.Active) continue;
            if (c.EvidenceSpawned) continue;
            if (!string.IsNullOrEmpty(c.TargetNpcId) && c.TargetNpcId == npc.ID)
            {
                match = c;
                // Keep the cached instance id session-current: Unity re-rolls
                // InstanceIDs on every game start, so a value persisted from a
                // previous session is dead weight for the dead-drop match.
                c.TargetNpcInstanceId = npc.GetInstanceID();
                break;
            }
            if (c.TargetNpcInstanceId != 0 && c.TargetNpcInstanceId == npc.GetInstanceID())
            {
                match = c; break;
            }
        }

        if (match == null)
        {
            Interlocked.Increment(ref _earlyOuts);
            // v0.1.3: this used to be a silent exit — a kill with no waiting
            // contract left no trace in the log and was undiagnosable.
            Mod.Log.Info($"[Bounty] NPC '{npc.ID}' died — no active contract targets it " +
                         $"(active={save.Active.Count}); no polaroid spawned.");
            return;
        }

        Interlocked.Increment(ref _matchesFound);

        var polaroid = BountyEvidenceItemRegistry.Spawn(npc.GetInstanceID());
        if (polaroid == null)
        {
            // Audit M8 (2026-09-01): the target watchdog re-calls OnNpcDied every
            // 1.5s while the NPC stays unconscious — without a throttle this warn
            // spams the log once per poll when the registry is broken.
            if (Time.realtimeSinceStartup - _lastSpawnFailWarn > 30f)
            {
                _lastSpawnFailWarn = Time.realtimeSinceStartup;
                Mod.Log.Warn($"Polaroid definition not registered; cannot spawn evidence for " +
                             $"target={npc.ID} (instance={npc.GetInstanceID()}).");
            }
            return;
        }

        try
        {
            var inv = S1PlayerInventory.Instance;
            if (inv == null)
            {
                Mod.Log.Warn("PlayerInventory.Instance is null; cannot add polaroid.");
                return;
            }
            inv.AddItemToInventory(polaroid);
            match.EvidenceSpawned = true;
            // Audit H1 (2026-09-01): AwaitingDrop survives save loads — the
            // receipt gate and the cross-session fallback key off THIS flag, not
            // off EvidenceSpawned (which is reset below so a lost polaroid can
            // be re-earned by killing the target again).
            match.AwaitingDrop = true;
            Interlocked.Increment(ref _polaroidsSpawned);
            BountyPersistence.PersistCurrent(); // v0.1.3: evidence state must survive a game restart

            // Advance the quest journal objective
            BountyJournalBridge.AdvanceToDeadDrop(match.Id);

            // Phase G: schedule the post-kill pursuit grace window. We pre-stamp
            // the marker so the moment the player walks toward a dead-drop the
            // engine has already settled on Investigating for at least 60 seconds.
            BountyHeatService.OnBountyKilled(match);

            Mod.Log.Info($"[Bounty#{match.Id}] Polaroid spawned for target '{npc.ID}' " +
                         $"(instance={npc.GetInstanceID()}); awaiting dead-drop validation.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Adding polaroid to inventory failed: {ex}");
        }
    }

    /// <summary>
    /// BUGFIX (2026-08-31): Clear the one-way EvidenceSpawned latch on Active
    /// contracts after every save load. The flag exists to prevent duplicate
    /// polaroids within one session, but persisted across loads it bricked
    /// contracts whose evidence item was lost (inventory full at spawn, item
    /// dropped, older save) — every later kill of the target then silently
    /// early-outed and no polaroid ever spawned again. Fresh session = fresh
    /// chance; a duplicate polaroid is harmless because the dead-drop receipt
    /// completes the contract exactly once.
    ///
    /// Audit H1 (2026-09-01): the receipt gate and the cross-session fallback
    /// no longer read EvidenceSpawned — they read <see cref="BountyContract.AwaitingDrop"/>,
    /// which is NOT reset here, so depositing a pre-reload polaroid still pays out.
    /// Call from SaveStateGuard.OnLoadComplete.
    /// </summary>
    public static void ResetEvidenceFlags()
    {
        if (Mod.Instance?.Save == null) return;
        int reset = 0;
        for (int i = 0; i < Mod.Instance.Save.Active.Count; i++)
        {
            var c = Mod.Instance.Save.Active[i];
            if (c.Status == EBountyStatus.Active && c.EvidenceSpawned)
            {
                c.EvidenceSpawned = false;
                reset++;
            }
        }
        if (reset > 0)
        {
            Mod.Log.Info($"[Bounty] Reset EvidenceSpawned on {reset} active contract(s) after load " +
                         "(evidence item can re-spawn on next target death).");
        }
    }

    /// <summary>
    /// Audit M2 (2026-09-01): Unity InstanceIDs are re-rolled every game start.
    /// A TargetNpcInstanceId persisted by a previous session can randomly equal a
    /// DIFFERENT NPC's fresh instance id, completing the wrong contract off a
    /// stale polaroid. Zero the persisted ids on every load — they are re-stamped
    /// (session-current) the moment this session's target dies, and same-session
    /// receipts keep working via the GetNPC-by-id fallback in the receipt scan.
    /// Call from SaveStateGuard.OnLoadComplete.
    /// </summary>
    public static void InvalidatePersistedInstanceIds()
    {
        if (Mod.Instance?.Save == null) return;
        int cleared = 0;
        for (int i = 0; i < Mod.Instance.Save.Active.Count; i++)
        {
            var c = Mod.Instance.Save.Active[i];
            if (c.Status == EBountyStatus.Active && c.TargetNpcInstanceId != 0)
            {
                c.TargetNpcInstanceId = 0;
                cleared++;
            }
        }
        if (cleared > 0)
        {
            Mod.Log.Info($"[Bounty] Invalidated {cleared} persisted TargetNpcInstanceId(s) after load " +
                         "(stale Unity InstanceIDs must never match across sessions).");
        }
    }
}

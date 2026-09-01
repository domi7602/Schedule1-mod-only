using System;
using System.Collections.Generic;
using System.Threading;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Law;
using S1API.Entities;
using S1Mods.Shared;

#if (IL2CPPMELON)
using S1PlayerOfficer = Il2CppScheduleOne.Police.PoliceOfficer;
#elif MONOMELON
using S1PlayerOfficer = ScheduleOne.Police.PoliceOfficer;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase G — Heat orchestration.
///
/// We bridge the Bounty workflow with the existing Police system via S1API's
/// <see cref="PlayerCrimeData"/>. We never modify the engine directly — we
/// raise or lower PursuitLevel through the S1API wrapper, which routes through
/// the same RPC the engine uses internally.
///
/// Effects timeline:
///   • Pre-Pursuit (Accept Bounty → set PersuitLevel=Investigating):
///     Player is now "wanted"; officer VisionEvents will escalate naturally.
///   • On-Kill (BountyService.OnNpcDied matched):
///     Hold the player's pursuit at Investigating for <see cref="KillGraceSeconds"/>
///     (60s in real time) — the killer gets a window to disappear before
///     escalation. We re-apply Investigating every tick during the grace window.
///   • On-Timeout / Player-Death:
///     Set PursuitLevel=NonLethal for <see cref="FailPursuitDays"/> in-game days.
///   • On-Cancel (Player eats the offer decline cooldown):
///     Drop pursuit back to None.
///
/// Skill reference: <c>schedule1-game-systems/references/12-Heat-Pursuit-Law.md</c>.
/// </summary>
public static class BountyHeatService
{
    /// <summary>Hold the player's pursuit at Investigating for 60 real-time seconds after a kill.</summary>
    public const float KillGraceSeconds = 60f;

    /// <summary>Days the player stays at NonLethal pursuit after a failed bounty.</summary>
    public const int FailPursuitDays = 2;

    private const string GraceElapsedKey = "hitman_grace_until_";

    /// <summary>
    /// Audit M1 (2026-09-01): the grace window lives IN MEMORY ONLY now. It used
    /// to be persisted into Save.HeatState keyed on Time.realtimeSinceStartup —
    /// after an app restart the persisted timestamp was huge, every grace key
    /// stayed "active" for the whole session and the save file grew unboundedly.
    /// A grace window is seconds-scale session state; persisting it was wrong.
    /// </summary>
    private static readonly System.Collections.Generic.Dictionary<string, float> GraceUntil =
        new();

    private static int _pursuitSetActive;
    private static int _pursuitSetGrace;
    private static int _pursuitSetFail;
    private static int _pursuitCleared;

    /// <summary>
    /// Live count of Active contracts, computed from the save on demand.
    /// Audit M9 (2026-09-01): the old increment/decrement counter drifted
    /// negative (early returns before increment, decrements for contracts from
    /// earlier sessions, slot-switch resets). The save list is the only truth.
    /// </summary>
    public static int ActiveBounties
    {
        get
        {
            var save = Mod.Instance?.Save;
            if (save == null) return 0;
            int n = 0;
            for (int i = 0; i < save.Active.Count; i++)
                if (save.Active[i].Status == EBountyStatus.Active) n++;
            return n;
        }
    }
    public static int PursuitSetActive => Volatile.Read(ref _pursuitSetActive);
    public static int PursuitSetGrace => Volatile.Read(ref _pursuitSetGrace);
    public static int PursuitSetFail => Volatile.Read(ref _pursuitSetFail);
    public static int PursuitCleared => Volatile.Read(ref _pursuitCleared);

    /// <summary>
    /// Reset internal runtime counters when a save slot switch is detected.
    /// Public for testability and so SaveStateGuard can wipe stale counters
    /// between slot switches.
    /// </summary>
    public static void ResetActiveCounters()
    {
        GraceUntil.Clear();
        Volatile.Write(ref _pursuitSetActive, 0);
        Volatile.Write(ref _pursuitSetGrace, 0);
        Volatile.Write(ref _pursuitSetFail, 0);
        Volatile.Write(ref _pursuitCleared, 0);
    }

    /// <summary>
    /// Audit M1: old saves carry realtimeSinceStartup-based grace keys in
    /// HeatState. They are never read again — purge them so the save file
    /// shrinks back. Called from SaveStateGuard.OnLoadComplete.
    /// </summary>
    public static void PurgePersistedHeatKeys()
    {
        var save = Mod.Instance?.Save;
        if (save?.HeatState == null || save.HeatState.Count == 0) return;
        List<string> doomed = new();
        foreach (var kvp in save.HeatState)
        {
            if (kvp.Key.StartsWith(GraceElapsedKey, StringComparison.Ordinal)) doomed.Add(kvp.Key);
        }
        for (int i = 0; i < doomed.Count; i++) save.HeatState.Remove(doomed[i]);
        if (doomed.Count > 0)
        {
            Mod.Log.Info($"[Heat] Purged {doomed.Count} stale persisted grace key(s) from the save.");
        }
    }

    /// <summary>
    /// Called when the player accepts a bounty contract. We mark the active bounties
    /// count and push the player to <see cref="PursuitLevel.Investigating"/>.
    /// Idempotent across multiple concurrent bounties — we don't escalate beyond
    /// Investigating unless the player keeps failing.
    /// </summary>
    public static void OnBountyAccepted(BountyContract contract)
    {
        var local = GetLocalPlayer();
        if (local == null) return;
        Interlocked.Increment(ref _pursuitSetActive);
        try
        {
            local.CrimeData.SetPursuitLevel(PursuitLevel.Investigating);
            Mod.Log.Info($"[Heat] Accepted bounty #{contract.Id} on '{contract.TargetNpcId}'. " +
                         $"PursuitLevel set to Investigating.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnBountyAccepted failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Called by the BountyService when a kill matched a bounty. We:
    ///   • record a grace timestamp in the save (Skill §6 "Timed" — in-memory first)
    ///   • schedule a 60s real-time grace period where we re-apply Investigating
    ///   • the player's actual pursuit is held at Investigating (no escalation to
    ///     Arresting/NonLethal while they're processing the body).
    /// </summary>
    public static void OnBountyKilled(BountyContract contract)
    {
        try
        {
            var local = GetLocalPlayer();
            if (local == null) return;

            // FIX: Apply Investigating exactly ONCE at the moment of the kill/KO!
            try { local.CrimeData.SetPursuitLevel(PursuitLevel.Investigating); } catch { }

            // Audit M1 (2026-09-01): in-memory grace window — never persisted.
            float graceUntil = UnityEngine.Time.realtimeSinceStartup + KillGraceSeconds;
            GraceUntil[contract.Id] = graceUntil;
            Interlocked.Increment(ref _pursuitSetGrace);
            Mod.Log.Info($"[Heat] Bounty #{contract.Id} matched on kill; grace until {graceUntil:F1}s.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnBountyKilled failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Called when a bounty expires (timeout), or the player dies during a bounty,
    /// or the offer was declined but pursuit had already been raised.
    /// Escalates to <see cref="PursuitLevel.NonLethal"/>.
    /// </summary>
    public static void OnBountyFailed(string reason)
    {
        try
        {
            var local = GetLocalPlayer();
            if (local == null) return;
            // Only escalate if the player still has at least one live bounty
            if (ActiveBounties > 0)
            {
                // Player has a parallel bounty still in flight — keep at Investigating.
                return;
            }
            Interlocked.Increment(ref _pursuitSetFail);
            local.CrimeData.SetPursuitLevel(PursuitLevel.NonLethal);
            Mod.Log.Info($"[Heat] Bounty failed ({reason}); pursuit escalated to NonLethal.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnBountyFailed failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Called after the polaroid is deposited and the payout has cleared. The
    /// bounty is now Completed; we drop the pursuit level all the way to None so
    /// the player isn't permanently wanted.
    /// </summary>
    public static void OnBountyCompleted(BountyContract contract)
    {
        try
        {
            var local = GetLocalPlayer();
            if (local == null) return;
            if (ActiveBounties <= 0)
            {
                Interlocked.Increment(ref _pursuitCleared);
                local.CrimeData.SetPursuitLevel(PursuitLevel.None);
                Mod.Log.Info($"[Heat] Bounty #{contract.Id} complete; pursuit dropped to None.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnBountyCompleted failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Expires in-memory grace markers. Called from <see cref="Mod.OnUpdate"/>.
    /// Audit M1: the grace window no longer re-applies Investigating per frame
    /// (removed earlier for UI freezes) and is no longer persisted — this tick
    /// only garbage-collects expired markers.
    /// </summary>
    public static void Tick()
    {
        if (GraceUntil.Count == 0) return;
        float now = UnityEngine.Time.realtimeSinceStartup;
        List<string> expired = null;
        foreach (var kvp in GraceUntil)
        {
            if (now >= kvp.Value)
            {
                (expired ??= new List<string>()).Add(kvp.Key);
            }
        }
        if (expired != null)
        {
            for (int i = 0; i < expired.Count; i++) GraceUntil.Remove(expired[i]);
        }
    }

    private static Player? GetLocalPlayer()
    {
        try { return Player.Local; }
        catch (Exception ex)
        {
            Mod.Log.Warn($"GetLocalPlayer failed: {ex.Message}");
            return null;
        }
    }
}


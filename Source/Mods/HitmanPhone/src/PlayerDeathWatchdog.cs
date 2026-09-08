using System;
using System.Threading;
using HitmanPhone.Persistence;
using MelonLoader;
using S1API.Entities;
using S1Mods.Shared;

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase H — Player-Death Watchdog.
///
/// Polls <see cref="Player.IsDead"/> once per second and, on the rising edge
/// (alive → dead transition), iterates over every active bounty contract and
/// marks it Failed. This prevents the player from being able to keep a contract
/// active indefinitely by repeatedly falling unconscious without our knowledge.
///
/// Skill Rule #4: 0-Allocation Polling. We <c>Interlocked.Increment</c> a counter
/// and only run the full check every ~60 frames. No LINQ, no foreach on hot path.
/// </summary>
public static class PlayerDeathWatchdog
{
    private static int _aliveFrameCounter;
    private static bool _wasAlive = true;

    private static int _deathsDetected;
    private static int _failuresDispatched;

    public static int DeathsDetected => Volatile.Read(ref _deathsDetected);
    public static int FailuresDispatched => Volatile.Read(ref _failuresDispatched);

    /// <summary>
    /// Called from <see cref="Mod.OnUpdate"/>. Throttles to one full check per second.
    /// </summary>
    public static void Tick()
    {
        // Throttle: ~60 frames at 60 fps == once per second.
        int fc = Volatile.Read(ref _aliveFrameCounter);
        if (fc < 60) { Interlocked.Increment(ref _aliveFrameCounter); return; }
        Interlocked.Exchange(ref _aliveFrameCounter, 0);

        Player? local = null;
        try { local = Player.Local; }
        catch { return; }
        if (local == null) return;

        bool alive = false;
        try { alive = !local.IsDead; }
        catch
        {
            // S1API wrapper may be in transition during scene load; skip this tick.
            return;
        }

        bool wasAlive = _wasAlive;
        _wasAlive = alive;

        // Rising-edge detection: `wasAlive=true, alive=false` is the transition
        // (alive → dead) — the player just died.
        if (wasAlive && !alive)
        {
            Interlocked.Increment(ref _deathsDetected);
            DispatchAllActiveToFailed("player_died");
        }
    }

    /// <summary>
    /// Iterate every <see cref="EBountyStatus.Active"/> contract and mark it Failed.
    /// Each one triggers the heat escalation path and the caller cooldowns.
    /// </summary>
    public static void DispatchAllActiveToFailed(string reason)
    {
        var save = Mod.Instance?.Save;
        if (save == null) return;
        int dispatched = 0;
        // Audit M6 (2026-09-01): forfeited contracts leave Active (moved to
        // History, matching the completion/expiry paths) — backwards iteration
        // because RemoveAt mutates the list mid-loop.
        for (int i = save.Active.Count - 1; i >= 0; i--)
        {
            var c = save.Active[i];
            if (c.Status != EBountyStatus.Active) continue;
            c.Status = EBountyStatus.Forfeited; // distinguishes from timeout / cancelled
            save.Active.RemoveAt(i);
            save.History.Add(c);
            Interlocked.Increment(ref _failuresDispatched);
            BountyPersistence.PersistCurrent(); // v0.1.3: forfeit must survive a game restart

            // Heat: escalate. Caller: 14-day cooldown.
            try
            {
                BountyHeatService.OnBountyFailed(reason);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"OnBountyFailed during death dispatch: {ex.Message}");
            }
            int callerIdx = ResolveCallerIndex(c.CallerId);
            if (callerIdx >= 0)
            {
                BountyCallScheduler.CooldownCaller(callerIdx,
                    BountyCallSchedulerConstants.CallerCooldownDaysAfterPlayerDeath);
            }

            // Journal: mark forfeited. "forfeited" is treated as Failed by the bridge.
            try { BountyJournalBridge.FailQuest(c.Id, reason: "forfeited"); }
            catch { /* best-effort */ }

            dispatched++;
        }
        if (dispatched > 0)
        {
            Mod.Log.Info($"[DeathWatchdog] Player died with {dispatched} active bounty " +
                         $"contract(s); marked Forfeited with 14d caller cooldown.");
        }
    }

    /// <summary>
    /// Reconstruct caller index from "caller_ghost" → 0 etc.
    /// </summary>
    private static int ResolveCallerIndex(string callerId)
    {
        if (string.IsNullOrEmpty(callerId)) return -1;
        if (!callerId.StartsWith("caller_")) return -1;
        string name = callerId.Substring("caller_".Length);
        for (int i = 0; i < BountyDialogTemplates.CallerPool.Count; i++)
        {
            if (string.Equals(name, BountyDialogTemplates.GetCallerName(i),
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Reset the watchdog (call this after save-load to avoid spurious death triggers
    /// if the player respawned).
    /// </summary>
    public static void Reset()
    {
        _wasAlive = true;
        Volatile.Write(ref _aliveFrameCounter, 0);
    }
}

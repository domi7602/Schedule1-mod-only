using System;
using System.Collections.Generic;

namespace HitmanPhone.Persistence;

/// <summary>
/// Serialized state for the Hitman-Phone mod. Lives in
/// <c>UserData\HitmanPhone\bounties_slot_{n}.json</c> (slot-isolated).
///
/// Lifecycle: BountyContract is created in <c>Offered</c>, transitions to
/// <c>Active</c> when the player accepts, and resolves to <c>Completed</c>,
/// <c>Failed</c>, or <c>Expired</c>. Mirror of Schedule I's
/// <c>Quest : MonoBehaviour</c> state machine (Reference 03 in
/// <c>.agents/skills/schedule1-game-systems/references/</c>).
/// </summary>
[Serializable]
public class BountySaveData
{
    /// <summary>
    /// Identity of the actual game save, not just its slot number. This prevents
    /// a new game started in an existing slot from inheriting old bounty state.
    /// </summary>
    public string SaveIdentity = "";

    public List<BountyContract> Active = new();
    public List<BountyContract> History = new();

    /// <summary>
    /// Per-caller cooldown in in-game days (CooldownDay - CurrentDay &lt; 0 means
    /// the caller can ring again). Key = caller NPC id, Value = the in-game day
    /// the cooldown expires.
    /// </summary>
    public Dictionary<string, int> CallerCooldowns = new();

    /// <summary>
    /// Stable id of the dead drop used by the most recent payout; the next
    /// offer avoids assigning the same drop again (variety).
    /// </summary>
    public string LastUsedDropId = "";

    public int LastSaveDay; // in-game day when BountySaveData was last persisted

    /// <summary>
    /// Durable elimination events (contract kills) for the tracker app. One
    /// record per stable NPC id per save. Old saves simply deserialize this as
    /// empty (or null, see BountyPersistence.Load) and the tracker falls back
    /// to contract history for them.
    /// </summary>
    public List<BountyKillRecord> KillEvents = new();

    /// <summary>
    /// Persisted caller schedule plan (package D) — irregular cadence at save
    /// level, in in-game time. Null/empty on old saves → generated lazily on
    /// the next scheduler tick without replaying missed days.
    /// </summary>
    public BountyCallScheduleState CallSchedule = new();

    /// <summary>
    /// Legacy heat-state dictionary. It is retained only so old saves can have
    /// obsolete grace keys removed during load; the current Lethal-on-kill flow
    /// does not persist transient pursuit state.
    /// </summary>
    public Dictionary<string, string> HeatState = new();
}

[Serializable]
public class BountyContract
{
    public string Id;
    public string CallerId;
    public string CallerStyle; // "cold" | "threatening" | "desperate"
    public string TargetNpcId;
    public int TargetNpcInstanceId; // Unity InstanceID, used as Polaroid `Value`
    public string TargetNpcName;     // cached display name at offer time
    public float RewardCash;

    /// <summary>
    /// Legacy shim: contracts saved by v0.1.0–v0.1.8 stored the reward as
    /// "RewardOnline" (paid via CreateOnlineTransaction). v0.1.9 pays dirty cash
    /// instead. Set-only property — System.Text.Json populates it when reading
    /// old saves but never writes it (no getter), so new saves carry only the
    /// new <see cref="RewardCash"/> name.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("RewardOnline")]
    public float? RewardOnlineLegacy
    {
        set { if (value.HasValue) RewardCash = value.Value; }
    }
    public int OfferedAtDay;
    public int DeadlineDay;
    public EBountyStatus Status;
    public bool EvidenceSpawned; // Tracks if the polaroid has already been handed to the player

    /// <summary>
    /// Session-spanning latch: "a polaroid for this contract exists somewhere
    /// (inventory or dead drop) and the dead-drop receipt must watch for it".
    /// Unlike <see cref="EvidenceSpawned"/> — which is reset on every load so a
    /// lost polaroid can be re-earned — this flag SURVIVES reloads. It drives
    /// the receipt scan gate and the cross-session fallback (2026-09-01 audit H1:
    /// gating on EvidenceSpawned made any reload silently kill the payout).
    /// </summary>
    public bool AwaitingDrop;

    // Optional: which Drop-Position the caller wants the photo deposited at.
    // Null = "any drop". If specified, only that exact drop pays out.
    // Since the budget-terms update the assigned dead drop is written here at
    // OFFER time, so the offer message can name it before the player accepts.
    public string RequiredDropId;

    // ------------------------------------------------------------------
    // Budget-derived terms + fixed drop window (added 2026-10 by the
    // budget/dead-drop/tracker extension). Every field defaults to the legacy
    // semantics below when missing from an old save (System.Text.Json leaves
    // absent fields at their initializer values), so pre-existing contracts
    // keep their exact old conditions.
    // ------------------------------------------------------------------

    /// <summary>
    /// Mean weekly customer spend of the target (Min/MaxWeeklySpend midpoint)
    /// captured at offer time. 0 = unknown (legacy contract or target without
    /// customer data) → legacy reward and deadline semantics stay in effect.
    /// </summary>
    public float TargetWeeklySpend;

    /// <summary>"low" | "mid" | "high" | "" (unknown/legacy). Fixed at offer time.</summary>
    public string RewardTier;

    /// <summary>
    /// Display name of <see cref="RequiredDropId"/>, cached at offer time so
    /// messages and the tracker never need live dead-drop lookups.
    /// </summary>
    public string AssignedDropName;

    /// <summary>
    /// Post-photo drop-window due point as absolute in-game minutes
    /// (day*1440 + minute-of-day). 0 = window not started yet (or legacy
    /// contract that never started one).
    /// </summary>
    public long DropDueMinSum;

    /// <summary>
    /// True once the polaroid was confirmed in the player's possession — the
    /// drop window starts at that first confirmed receipt and is never reset by
    /// later re-pickups. Survives reload like <see cref="AwaitingDrop"/>.
    /// </summary>
    public bool DropDueStarted;

    /// <summary>One-time "window is running out" warning latch.</summary>
    public bool DropWarningSent;

    /// <summary>Minutes granted for the drop window (fixed at offer time; UI countdown base).</summary>
    public int DropWindowMinutes;
}

/// <summary>
/// Durable elimination event for the kill tracker (package E). Recorded when a
/// bounty elimination is observed (polaroid hand-off moment), one entry per
/// stable NPC id per save — several contracts on the same target never
/// double-count a kill.
/// </summary>
[Serializable]
public class BountyKillRecord
{
    public string TargetNpcId;  // stable game NPC id
    public string TargetName;   // display name at kill time
    public int Day;             // in-game day of the elimination
    public long MinuteSum;      // day*1440 + minute-of-day, for ordering
    public bool Died;           // true = game confirmed death (Health.IsDead); false = knockout/unconscious only
    public string ContractId;   // bounty contract that triggered the record
}

/// <summary>
/// One pre-planned caller slot in the save-level call schedule (package D).
/// Slots are generated in advance and consumed exactly once, so reloading or
/// sleeping never re-rolls or replays calls.
/// </summary>
[Serializable]
public class BountyCallSlot
{
    public int Day;
    public int MinuteOfDay;
    public int CallerIndex;
    public bool Dispatched; // offer actually sent
    public bool Cancelled;  // slot skipped (missed while offline / no caller available at generation time)
}

/// <summary>
/// Persisted irregular call plan (save-level). Generated ahead by
/// BountyCallSchedulePlan; never re-rolled per contract.
/// </summary>
[Serializable]
public class BountyCallScheduleState
{
    /// <summary>Plan covers days &lt;= this day (exclusive upper bound for generation).</summary>
    public int GeneratedThroughDay;
    public List<BountyCallSlot> Slots = new();
}

public enum EBountyStatus
{
    Offered,
    Active,
    Completed,
    Failed,
    Expired,
    Forfeited
}

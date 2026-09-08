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

    public int LastSaveDay; // in-game day when BountySaveData was last persisted

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
    public string RequiredDropId;
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

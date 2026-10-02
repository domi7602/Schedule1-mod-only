using System;
using System.Collections.Generic;
using System.IO;
using S1Mods.Shared;
using HitmanPhone.Persistence;

using S1Persistence = Il2CppScheduleOne.Persistence;
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1NPCManager = Il2CppScheduleOne.NPCs.NPCManager;

namespace HitmanPhone.Persistence;

/// <summary>
/// Slot-isolated persistence for Hitman-Phone save data.
///
/// Pattern (Reference: <c>schedule1-persistence</c> skill §3 + §4):
///   • UserData\HitmanPhone\bounties_slot_{n}.json
///   • TryMigrateLegacy() moves a pre-slot file once if a slot file is missing.
///   • SaveLoad lifecycle hooks in Mod.cs call Load() / Save() at the right times.
/// </summary>
public static class BountyPersistence
{
    private const string ModFolder = "HitmanPhone";
    private const string LegacyFileName = "bounties.json";
    private const string SlotFileNamePattern = "bounties_slot_{0}.json";

    private static readonly ModLogger Log = new(ModFolder);

    /// <summary>
    /// CRITICAL FIX (v0.1.9, 2026-09-01): <see cref="BountySaveData"/> consists of
    /// public FIELDS, but System.Text.Json does not serialize fields unless
    /// <c>IncludeFields</c> is set — every save since v0.1.3 wrote the literal
    /// "{}" (proven by inspecting bounties_slot_0.json). A game reload mid-contract
    /// silently wiped all bounty state. These options mirror SafeStorage's defaults
    /// plus IncludeFields, and MUST be passed to BOTH Save and Load.
    /// </summary>
    private static readonly System.Text.Json.JsonSerializerOptions FieldJsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        IncludeFields = true
    };

    private static int _lastKnownSlot = -1;

    /// <summary>Active slot number, or -1 if not currently loaded.
    /// Sonde: S1Mods.Shared.SaveSlots (Konsolidierung 2026-09-15).</summary>
    public static int ActiveSlotNumber
    {
        get
        {
            int slot = SaveSlots.GetActiveSlotNumber();
            if (slot >= 0)
            {
                _lastKnownSlot = slot;
            }
            return _lastKnownSlot;
        }
    }

    /// <summary>Best-effort read of the active save slot suffix. Returns null if slot &lt; 0.</summary>
    public static string? ActiveSlotSuffix
    {
        get
        {
            int slot = ActiveSlotNumber;
            return slot >= 0 ? slot.ToString() : null;
        }
    }

    /// <summary>
    /// Stable identity for the actual vanilla save. SaveSlotNumber alone is not
    /// enough because starting a new game in the same slot reuses that number.
    /// DateCreated distinguishes a replacement save even when the organisation
    /// name and folder are unchanged.
    /// </summary>
    public static string ActiveSaveIdentity
    {
        get
        {
            try
            {
                var info = S1Persistence.LoadManager.Instance?.ActiveSaveInfo;
                if (info == null) return "";

                return string.Join("|",
                    info.SavePath ?? "",
                    info.OrganisationName ?? "",
                    info.DateCreated.ToString("O"));
            }
            catch
            {
                return "";
            }
        }
    }

    /// <summary>Audit L6 (2026-09-01): the legacy migration needs to run exactly once per session.</summary>
    private static bool _migrationChecked;

    public static string? GetSaveFilePath()
    {
        string? slot = ActiveSlotSuffix;
        if (slot == null) return null;
        string path = SafeStorage.GetUserDataPath(ModFolder, string.Format(SlotFileNamePattern, slot));
        if (!_migrationChecked && int.TryParse(slot, out int s) && s >= 0)
        {
            _migrationChecked = true;
            TryMigrateLegacy(path);
        }
        return path;
    }

    public static BountySaveData Load()
    {
        string? path = GetSaveFilePath();
        if (string.IsNullOrEmpty(path))
        {
            Log.Debug("No valid save slot active yet. Returning empty state.");
            return NewStateForActiveSave();
        }
        BountySaveData data = SafeStorage.LoadSafe<BountySaveData>(path, null, Log, FieldJsonOptions);
        if (data == null)
        {
            Log.Info($"No existing save found at '{path}'. Starting with empty state.");
            return NewStateForActiveSave();
        }

        string currentIdentity = ActiveSaveIdentity;
        if (!string.IsNullOrEmpty(currentIdentity) &&
            !string.IsNullOrEmpty(data.SaveIdentity) &&
            !string.Equals(data.SaveIdentity, currentIdentity, StringComparison.Ordinal))
        {
            Log.Info("Save identity changed in the same slot — treating this as a new game " +
                     "and discarding old HitmanPhone contract state.");
            var fresh = NewStateForActiveSave();
            Save(fresh);
            return fresh;
        }

        // One-time migration for state written before SaveIdentity existed.
        if (string.IsNullOrEmpty(data.SaveIdentity) && !string.IsNullOrEmpty(currentIdentity))
        {
            data.SaveIdentity = currentIdentity;
            Save(data);
            Log.Info("Adopted existing bounty state for the current vanilla save identity.");
        }

        Log.Info($"Loaded BountySaveData from '{path}': " +
                 $"{data.Active.Count} active, {data.History.Count} historical contracts.");
        return data;
    }

    private static BountySaveData NewStateForActiveSave()
        => new() { SaveIdentity = ActiveSaveIdentity };

    public static bool Save(BountySaveData data)
    {
        if (data == null)
        {
            Log.Warn("Refusing to save null BountySaveData.");
            return false;
        }
        if (string.IsNullOrEmpty(data.SaveIdentity))
            data.SaveIdentity = ActiveSaveIdentity;
        string? path = GetSaveFilePath();
        if (string.IsNullOrEmpty(path))
        {
            Log.Warn("Cannot save BountySaveData: no active save slot (slot < 0).");
            return false;
        }
        bool ok = SafeStorage.SaveAtomic(path, data, Log, FieldJsonOptions);
        if (ok) Log.Info($"Saved BountySaveData to '{path}'.");
        return ok;
    }

    /// <summary>
    /// BUGFIX (2026-08-31, v0.1.3): write-through persistence. Until now the ONLY
    /// Save() call site was the /hitman_reset test command — accepted contracts,
    /// evidence spawns and completions lived in RAM only, so every game restart
    /// silently wiped them (Active=0 on next load, no polaroid on target kill,
    /// orphaned journal quest). Call after EVERY contract state transition:
    /// accept, evidence spawn, receipt/complete, expiry, forfeit.
    /// </summary>
    public static void PersistCurrent()
    {
        try
        {
            var save = Mod.Instance?.Save;
            if (save == null) return;
            Save(save);
        }
        catch (Exception ex)
        {
            Log.Warn($"PersistCurrent failed: {ex.Message}");
        }
    }

    /// <summary>
    /// One-time migration: if a legacy global file exists and no slot file does,
    /// move it to the active slot. Then delete the legacy file so we never re-migrate.
    /// </summary>
    private static void TryMigrateLegacy(string slotPath)
    {
        string legacy = SafeStorage.GetUserDataPath(ModFolder, LegacyFileName);
        if (!File.Exists(legacy)) return;
        if (File.Exists(slotPath))
        {
            // Slot file exists already — drop the legacy one to avoid future confusion.
            try { File.Delete(legacy); }
            catch (Exception ex) { Log.Warn($"Failed to delete legacy file '{legacy}': {ex.Message}"); }
            return;
        }
        try
        {
            SafeStorage.EnsureDirectoryForFile(slotPath);
            File.Move(legacy, slotPath);
            Log.Info($"Migrated legacy bounty data '{legacy}' → '{slotPath}'.");
        }
        catch (Exception ex)
        {
            Log.Warn($"Failed to migrate legacy bounty data '{legacy}': {ex.Message}");
        }
    }
}

// ---------------------------------------------------------------------------
// Merged from TargetResolveHelper.cs (2026-10-02) — NPC-id lookup wrapper.
// ---------------------------------------------------------------------------

/// <summary>
/// Phase D helper: Look up an NPC by its id, returning the typed native reference
/// or null. Wraps the static <see cref="S1NPCManager.GetNPC(string)"/> so callers
/// (e.g. force-fire path) don't need to know about <c>Il2CppScheduleOne.NPCs</c>.
/// </summary>
public static class TargetResolveHelper
{
    public static S1NPC? FindById(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        try { return S1NPCManager.GetNPC(id); }
        catch { return null; }
    }
}

// ---------------------------------------------------------------------------
// Merged from BountySaveData.cs (2026-10-02) — serialized models + status enum.
// ---------------------------------------------------------------------------

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

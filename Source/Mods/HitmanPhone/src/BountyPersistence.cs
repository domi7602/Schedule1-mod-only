using System;
using System.IO;
using S1Mods.Shared;
using HitmanPhone.Persistence;

#if (IL2CPPMELON)
using S1Persistence = Il2CppScheduleOne.Persistence;
#elif MONOMELON
using S1Persistence = ScheduleOne.Persistence;
#endif

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

    private static string _lastKnownSlot = "-1";

    /// <summary>Best-effort read of the active save slot. Defaults to "-1" before any save loaded.</summary>
    public static string ActiveSlotSuffix
    {
        get
        {
            try
            {
                var info = S1Persistence.LoadManager.Instance?.ActiveSaveInfo;
                if (info != null)
                {
                    _lastKnownSlot = info.SaveSlotNumber.ToString();
                    return _lastKnownSlot;
                }
            }
            catch
            {
                // Save may not be loaded yet (Main Menu); fall through to cached value.
            }
            return _lastKnownSlot;
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

    public static string GetSaveFilePath()
    {
        string slot = ActiveSlotSuffix;
        string path = SafeStorage.GetUserDataPath(ModFolder, string.Format(SlotFileNamePattern, slot));
        if (!_migrationChecked)
        {
            _migrationChecked = true;
            TryMigrateLegacy(path);
        }
        return path;
    }

    public static BountySaveData Load()
    {
        string path = GetSaveFilePath();
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
        string path = GetSaveFilePath();
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

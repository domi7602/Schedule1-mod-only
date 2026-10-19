// ---------------------------------------------------------------------------
// Persistence/BountyPersistence.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Persistence/BountyPersistence.cs     -> section Persistence/BountyPersistence.cs
//   Persistence/TargetResolveHelper.cs   -> section Persistence/TargetResolveHelper.cs
//
// ---------------------------------------------------------------------------

namespace HitmanPhone.Persistence
{
    // ==== HitmanPhone section: Persistence/BountyPersistence.cs ====
    using System;
    using System.IO;
    using S1Mods.Shared;
    using HitmanPhone.Persistence;

#if (IL2CPPMELON)
    using S1Persistence = Il2CppScheduleOne.Persistence;
#elif MONOMELON
    using S1Persistence = ScheduleOne.Persistence;
#endif


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

            NormalizeLoadedState(data);

            Log.Info($"Loaded BountySaveData from '{path}': " +
                     $"{data.Active.Count} active, {data.History.Count} historical contracts.");
            return data;
        }

        /// <summary>
        /// Field-level defaults keep old saves fully compatible (see the budget-terms
        /// block in <see cref="BountyContract"/>), but an explicit JSON null (or a
        /// hand-edited file) must never NRE the scheduler/tracker — normalize here.
        /// </summary>
        private static void NormalizeLoadedState(BountySaveData data)
        {
            data.KillEvents ??= new System.Collections.Generic.List<BountyKillRecord>();
            if (data.CallSchedule == null) data.CallSchedule = new BountyCallScheduleState();
            data.CallSchedule.Slots ??= new System.Collections.Generic.List<BountyCallSlot>();
            data.LastUsedDropId ??= "";
            foreach (var c in data.Active)
            {
                c.AssignedDropName ??= "";
                c.RewardTier ??= "";
            }
            foreach (var c in data.History)
            {
                c.AssignedDropName ??= "";
                c.RewardTier ??= "";
            }
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
    // ==== end HitmanPhone section: Persistence/BountyPersistence.cs ====
}

namespace HitmanPhone.Persistence
{
    // ==== HitmanPhone section: Persistence/TargetResolveHelper.cs ====
#if (IL2CPPMELON)
    using S1NPC = Il2CppScheduleOne.NPCs.NPC;
    using S1NPCManager = Il2CppScheduleOne.NPCs.NPCManager;
#elif MONOMELON
    using S1NPC = ScheduleOne.NPCs.NPC;
    using S1NPCManager = ScheduleOne.NPCs.NPCManager;
#endif


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
    // ==== end HitmanPhone section: Persistence/TargetResolveHelper.cs ====
}

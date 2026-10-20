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

        private static readonly BountyCheckpointSession Session = new();
        private static string _worldPath = "";
        private static string _nativeSavePath = "";
        private static string _baselineJson = "";
        private static string _baselineFingerprint = "";
        public static bool IsSaving => Session.Saving;

        public static void BindContext(int slot, string identity, string worldPath)
        {
            if (slot < 0 || string.IsNullOrWhiteSpace(identity) || string.IsNullOrWhiteSpace(worldPath))
                throw new InvalidOperationException("Unconfirmed HitmanPhone load context.");
            Session.Bind(identity, SafeStorage.GetUserDataPath(ModFolder, string.Format(SlotFileNamePattern, slot)));
            _worldPath = BountyWorldCheckpoint.ResolveDirectory(worldPath);
        }

        public static void ClearContext()
        {
            Session.Bind("", "");
            _worldPath = _nativeSavePath = "";
            _baselineJson = _baselineFingerprint = "";
        }

        /// <summary>Active slot number, or -1 if not currently loaded.
        /// Sonde: S1Mods.Shared.SaveSlots (Konsolidierung 2026-09-15).</summary>
        public static int ActiveSlotNumber => SaveSlots.GetActiveSlotNumber();

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
                    if (info == null || info.SaveSlotNumber < 0 || string.IsNullOrWhiteSpace(info.SavePath) || info.DateCreated.Ticks <= 0) return "";

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

        public static string? GetSaveFilePath() => string.IsNullOrEmpty(Session.Path) ? null : Session.Path;

        private static string CheckpointDirectory => SafeStorage.GetUserDataPath(ModFolder,
            System.IO.Path.Combine("checkpoints", Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(Session.Identity)))));

        private static string CheckpointPath(string fingerprint) => Path.Combine(CheckpointDirectory, fingerprint + ".json");

        public static BountySaveData Load()
        {
            if (string.IsNullOrEmpty(Session.Identity)) throw new InvalidOperationException("No bound bounty context.");
            string fingerprint = BountyWorldCheckpoint.Fingerprint(_worldPath);
            var loaded = LoadCore(fingerprint);
            // Retain the loaded baseline in RAM, so the first explicit save can also
            // archive the older world checkpoint without a write during load/migration.
            _baselineFingerprint = fingerprint;
            loaded.WorldCheckpoint = fingerprint;
            _baselineJson = System.Text.Json.JsonSerializer.Serialize(loaded, FieldJsonOptions);
            return loaded;
        }

        private static BountySaveData LoadCore(string fingerprint)
        {
            string checkpoint = CheckpointPath(fingerprint);
            foreach (string candidate in new[] { checkpoint, Session.Path })
            {
                if (!File.Exists(candidate) && !File.Exists(candidate + ".bak")) continue;
                try
                {
                    var restored = ReadValidated(candidate, fingerprint);
                    Log.Info($"[checkpoint] restored {fingerprint.Substring(0, 12)} for the confirmed world from '{candidate}'.");
                    return restored;
                }
                catch (InvalidDataException ex)
                {
                    Log.Warn($"[checkpoint] candidate not usable for this world: {ex.Message}");
                }
            }
            // Once checkpoint mode exists, an unknown world must never inherit the latest
            // state. A failed native/mod save gap stays visible instead of losing a reward.
            if (Directory.Exists(CheckpointDirectory))
                throw new InvalidDataException("No bounty snapshot matches this world checkpoint. Original data retained.");

            string path = Session.Path;
            if (!File.Exists(path) && !File.Exists(path + ".bak"))
            {
                string legacy = SafeStorage.GetUserDataPath(ModFolder, LegacyFileName);
                if (!File.Exists(legacy)) return new BountySaveData { SaveIdentity = Session.Identity };
                path = legacy; // read-only migration; never move/delete old data
            }
            var data = ReadValidated(path);
            if (!string.IsNullOrEmpty(data.SaveIdentity) && data.SaveIdentity != Session.Identity)
            {
                Log.Warn("[checkpoint] Different save identity: starting fresh in memory; prior file retained until an explicit world save.");
                return new BountySaveData { SaveIdentity = Session.Identity };
            }
            if (!string.IsNullOrEmpty(data.WorldCheckpoint))
                throw new InvalidDataException("Checkpoint archive missing; refusing to apply an unconfirmed bounty state.");
            data.SaveIdentity = Session.Identity;
            Log.Warn("[checkpoint] Legacy bounty data imported in memory. Its old world alignment cannot be proven; no source files changed.");
            return data;
        }

        private static BountySaveData ReadValidated(string path, string? expectedFingerprint = null)
        {
            // Loading never repairs/overwrites sources. A backup/compatibility copy is
            // accepted only if both its world fingerprint and identity are exact.
            Exception? error = null;
            foreach (string candidate in new[] { path, path + ".bak" })
            {
                if (!File.Exists(candidate)) continue;
                try
                {
                    var data = System.Text.Json.JsonSerializer.Deserialize<BountySaveData>(File.ReadAllText(candidate), FieldJsonOptions)
                        ?? throw new InvalidDataException("Null bounty data.");
                    BountySaveValidation.Normalize(data);
                    if (expectedFingerprint != null && !BountySaveValidation.MatchesCheckpoint(data, Session.Identity, expectedFingerprint))
                        throw new InvalidDataException("Bounty identity/checkpoint mismatch.");
                    if (candidate != path && expectedFingerprint == null)
                        throw new InvalidDataException("Legacy backup alignment is unconfirmed; manual recovery required.");
                    return data;
                }
                catch (Exception ex) { error = ex; }
            }
            throw new InvalidDataException($"Cannot safely load bounty data '{path}'. Original and backup retained.", error);
        }

        /// <summary>Gameplay changes remain in RAM until a confirmed native world save.</summary>
        public static void PersistCurrent() => Session.MarkDirty();

        // Kept for DEBUG reset callers: this deliberately does not write through.
        public static bool Save(BountySaveData data)
        {
            if (data == null || data.SaveIdentity != Session.Identity) return false;
            Session.MarkDirty();
            return true;
        }

        public static void PrepareCheckpoint()
        {
            var data = Mod.Instance?.Save ?? throw new InvalidOperationException("No bounty state loaded.");
            if (data.SaveIdentity != Session.Identity || ActiveSaveIdentity != Session.Identity)
                throw new InvalidOperationException("Bounty save identity changed before checkpoint.");
            if (!string.IsNullOrEmpty(_baselineJson) && !File.Exists(CheckpointPath(_baselineFingerprint)) &&
                !SafeStorage.SaveTextAtomic(CheckpointPath(_baselineFingerprint), _baselineJson, Log))
                throw new IOException("Could not preserve the previously loaded bounty checkpoint.");
            if (!HitmanPhoneTime.TryGetSnapshot(out int day, out _, out _))
                throw new InvalidOperationException("No valid game clock at save-start.");
            data.LastSaveDay = day;
            Session.Prepare(System.Text.Json.JsonSerializer.Serialize(data, FieldJsonOptions));
            Log.Info("[checkpoint] save-start snapshot frozen; bounty mutations paused.");
        }

        public static void CaptureDefaultNativeSavePath()
        {
            _nativeSavePath = "";
            var lm = S1Persistence.LoadManager.Instance;
            if (lm == null || !lm.IsGameLoaded || lm.IsLoading || ActiveSaveIdentity != Session.Identity) return;
            CaptureNativeSavePath(lm.LoadedGameFolderPath);
        }

        public static void CaptureNativeSavePath(string path)
        {
            _nativeSavePath = "";
            if (string.IsNullOrEmpty(Session.Identity) || !SaveStateGuard.IsSingleplayer) return;
            var info = S1Persistence.LoadManager.Instance?.ActiveSaveInfo;
            if (info == null || string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(info.SavePath)) return;
            string expected = BountyWorldCheckpoint.ResolveDirectory(info.SavePath);
            string actual = BountyWorldCheckpoint.ResolveDirectory(path);
            if (string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(_worldPath, actual, StringComparison.OrdinalIgnoreCase) && ActiveSaveIdentity == Session.Identity)
            {
                _nativeSavePath = _worldPath;
                Log.Info($"[checkpoint] native save destination verified: loaded='{_worldPath}', info='{info.SavePath}', request='{path}'.");
            }
            else
                Log.Error($"[checkpoint] save destination mismatch: loaded='{_worldPath}', info='{info.SavePath}', request='{path}'; commit refused.");
        }

        public static bool CommitCheckpoint(bool nativeSaveSucceeded)
        {
            return Session.Commit(ActiveSaveIdentity, nativeSaveSucceeded && !string.IsNullOrEmpty(_nativeSavePath), (path, frozen) =>
            {
                string fingerprint = BountyWorldCheckpoint.Fingerprint(_nativeSavePath);
                var data = System.Text.Json.JsonSerializer.Deserialize<BountySaveData>(frozen, FieldJsonOptions)!;
                data.WorldCheckpoint = fingerprint;
                string json = System.Text.Json.JsonSerializer.Serialize(data, FieldJsonOptions);
                // Keep each world checkpoint separately, so rolling the vanilla world back
                // selects its corresponding contract/evidence state instead of the newest one.
                if (!SafeStorage.SaveTextAtomic(CheckpointPath(fingerprint), json, Log)) return false;
                if (!SafeStorage.SaveTextAtomic(path, json, Log)) return false;
                _nativeSavePath = "";
                Log.Info($"[checkpoint] committed {fingerprint.Substring(0, 12)}; native save complete.");
                return true;
            });
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

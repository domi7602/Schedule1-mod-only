// ---------------------------------------------------------------------------
// Evidence/BountyEvidence.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Evidence/BountyEvidenceItemRegistry.cs -> section Evidence/BountyEvidenceItemRegistry.cs
//   Evidence/BountyReceiptService.cs     -> section Evidence/BountyReceiptService.cs
//   Evidence/DeadDropIdentifier.cs       -> section Evidence/DeadDropIdentifier.cs
//   Evidence/DeadDropPatch.cs            -> section Evidence/DeadDropPatch.cs
//
// ---------------------------------------------------------------------------

namespace HitmanPhone.Items
{
    // ==== HitmanPhone section: Evidence/BountyEvidenceItemRegistry.cs ====
    using System;
    using System.IO;
    using System.Reflection;
    using S1Mods.Shared;
    using UnityEngine;

#if (IL2CPPMELON)
    using S1ItemFramework = Il2CppScheduleOne.ItemFramework;
    using S1Registry = Il2CppScheduleOne.Registry;
#elif MONOMELON
    using S1ItemFramework = ScheduleOne.ItemFramework;
    using S1Registry = ScheduleOne.Registry;
#endif


    /// <summary>
    /// Registers the Polaroid evidence item used by the Hitman-Phone bounty workflow.
    ///
    /// We piggyback on Schedule I's <c>IntegerItemDefinition</c> and store a persistent
    /// bounty evidence token in the <c>Value</c> field of the spawned
    /// <c>IntegerItemInstance</c>. Old polaroids used Unity InstanceIDs and remain
    /// recognizable through <see cref="LegacyItemId"/> for save compatibility.
    ///
    /// Caveat: the public surface of <c>StorableItemDefinition</c> in
    /// <c>MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll</c> is stub-only (C++ bodies
    /// live in <c>GameAssembly.dll</c>), so we discover the inherited properties via
    /// reflection at runtime. This means a future game version may rename them; the
    /// registry tries each candidate and logs which one worked.
    /// </summary>
    public static class BountyEvidenceItemRegistry
    {
        public const string ItemId = "bounty_evidence_polaroid_v2";
        public const string LegacyItemId = "bounty_evidence_polaroid";
        public const string ItemName = "Polaroid Evidence";
        public const string ItemDescription =
            "A photo taken at the scene of a bounty. Its persistent evidence token " +
            "links it to an active contract. Drop it at the assigned dead-drop to collect the reward.";

        /// <summary>Cached sprite for the polaroid icon (loaded from disk in <see cref="Register"/>).</summary>
        public static Sprite? IconSprite { get; private set; }

        /// <summary>The current definition used for newly spawned evidence.</summary>
        public static S1ItemFramework.IntegerItemDefinition? Definition { get; private set; }

        /// <summary>Definition needed to deserialize polaroids saved by older versions.</summary>
        public static S1ItemFramework.IntegerItemDefinition? LegacyDefinition { get; private set; }

        public static bool IsRegistered => IsAlive(Definition) && IsAlive(LegacyDefinition);

        /// <summary>
        /// IL2CPP-safe liveness check for the cached Unity objects. A scene change
        /// can destroy the native Definition/Sprite while the managed static stays
        /// non-null — using it then throws. Mirrors the Pointer/WasCollected
        /// pattern in <see cref="Bounty.BountyReceiptService"/>.
        /// </summary>
        private static bool IsAlive(UnityEngine.Object? obj)
        {
#if (IL2CPPMELON)
            return obj != null && obj.Pointer != System.IntPtr.Zero && !obj.WasCollected;
#else
            return obj != null;
#endif
        }

        /// <summary>
        /// Registers the polaroid in the game registry if it has not been registered yet.
        /// Idempotent — calling it twice is a no-op.
        /// </summary>
        public static void Register()
        {
            if (IsRegistered) return;
            Definition = null;
            LegacyDefinition = null;

            if (!IsAlive(IconSprite))
            {
                IconSprite = LoadPolaroidSprite();
                if (IconSprite == null)
                {
                    Mod.Log.Warn("Polaroid sprite could not be loaded; " +
                                 "the item will be created with a fallback (null) icon.");
                }
            }

            try
            {
                Definition = CreateDefinition(ItemId);
                // Keep the old definition registered so saved legacy polaroids can
                // be deserialized after upgrading to the token-based item ID.
                LegacyDefinition = CreateDefinition(LegacyItemId);
                Mod.Log.Info($"Polaroid definitions registered (current='{ItemId}', legacy='{LegacyItemId}', " +
                             $"icon={(IconSprite != null ? "loaded" : "missing")}).");
            }
            catch (Exception ex)
            {
                Definition = null;
                LegacyDefinition = null;
                Mod.Log.Error($"Failed to register Polaroid item definitions: {ex}");
            }
        }

        private static S1ItemFramework.IntegerItemDefinition CreateDefinition(string id)
        {
            var def = ScriptableObject.CreateInstance<S1ItemFramework.IntegerItemDefinition>();
            def.hideFlags = HideFlags.HideAndDontSave;
            def.DefaultValue = 0;

            TrySetReflected(def, "ID", id);
            TrySetReflected(def, "Name", ItemName);
            TrySetReflected(def, "Description", ItemDescription);
            TrySetReflected(def, "StackLimit", 1);
            TrySetReflected(def, "BasePurchasePrice", 0f);
            TrySetReflected(def, "ResellMultiplier", 0f);
            TrySetReflected(def, "Icon", IconSprite);
            TrySetCategory(def, S1API.Items.ItemCategory.Decoration);
            RegisterIntoNativeRegistry(def);
            return def;
        }

        /// <summary>Spawn a version-2 polaroid carrying a persistent evidence token.</summary>
        public static S1ItemFramework.IntegerItemInstance? Spawn(int evidenceToken)
        {
            if (!IsRegistered)
            {
                // First spawn in this session, or a cached definition was destroyed;
                // register both current and legacy definitions before continuing.
                Mod.Log.Debug("Polaroid definitions were unavailable — attempting lazy re-registration.");
                Register();
            }

            if (Definition == null || !IsAlive(Definition))
            {
                Mod.Log.Warn("Polaroid Spawn() failed: Definition is still null or destroyed after re-registration.");
                return null;
            }
            if (evidenceToken <= 0)
            {
                Mod.Log.Error($"Polaroid Spawn() rejected invalid evidence token {evidenceToken}.");
                return null;
            }
            var instance = new S1ItemFramework.IntegerItemInstance(Definition, 1, evidenceToken);
            instance.SetValue(evidenceToken);
            return instance;
        }

        // --- helpers ---

        private static void TrySetReflected(object target, string propName, object value)
        {
            try
            {
                var prop = target.GetType().GetProperty(propName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(target, value);
                    Mod.Log.Debug($"Set {target.GetType().Name}.{propName}={value} (via reflection).");
                    return;
                }
                var field = target.GetType().GetField(propName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    field.SetValue(target, value);
                    Mod.Log.Debug($"Set {target.GetType().Name}.{propName}={value} (via reflection field).");
                    return;
                }
                Mod.Log.Warn($"Polaroid property/field '{propName}' not found on {target.GetType().Name} — skipping.");
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"TrySetReflected failed for {propName}: {ex.Message}");
            }
        }

        private static void TrySetCategory(object def, S1API.Items.ItemCategory category)
        {
            try
            {
                // Try setting Category via reflection. S1API exposes ItemCategory as a managed enum,
                // but the engine uses its own enum — value identity may not match. Cast via ToString
                // would not work; we just send the int value.
                int v = (int)category;
                TrySetReflected(def, "Category", v);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Category assignment skipped: {ex.Message}");
            }
        }

        private static Sprite? LoadPolaroidSprite()
        {
            try
            {
                string modDir = Path.GetDirectoryName(typeof(BountyEvidenceItemRegistry).Assembly.Location)
                                ?? string.Empty;
                string iconPath = Path.Combine(modDir, "polaroid.png");
                if (!File.Exists(iconPath))
                {
                    Mod.Log.Error($"Polaroid PNG missing at '{iconPath}'.");
                    return null;
                }
                byte[] bytes = File.ReadAllBytes(iconPath);
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false);
                tex.hideFlags = HideFlags.HideAndDontSave; // Protect texture from scene unload
                tex.name = "HitmanPhone_PolaroidIcon";
                if (!tex.LoadImage(bytes))
                {
                    Mod.Log.Error($"Texture2D.LoadImage failed for '{iconPath}'.");
                    return null;
                }
                tex.filterMode = FilterMode.Bilinear;
                var sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f),
                    pixelsPerUnit: 100f);
                sprite.hideFlags = HideFlags.HideAndDontSave; // Protect sprite from scene unload
                sprite.name = "HitmanPhone_PolaroidIcon";
                return sprite;
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"Exception while loading polaroid sprite: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Best-effort native registry injection. We try <c>Registry.AddItem</c> / <c>Register</c>
        /// via reflection because the accessor naming varies between patches. If nothing works
        /// the spec still falls back to manual spawning — the item is registered only so
        /// that ID-based lookups in <see cref="S1Registry"/> resolve; direct spawning works
        /// without registration.
        /// </summary>
        private static void RegisterIntoNativeRegistry(S1ItemFramework.IntegerItemDefinition def)
        {
            try
            {
                var registryType = typeof(S1Registry);
                string[] candidates = { "AddToRegistry", "AddItemToRegistry", "Register", "AddItem", "Add" };
                bool added = false;
                foreach (string name in candidates)
                {
                    var mi = registryType.GetMethod(name,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                        binder: null,
                        types: new[] { typeof(S1ItemFramework.ItemDefinition) },
                        modifiers: null);
                    if (mi != null)
                    {
                        object? instance = mi.IsStatic ? null : S1Registry.Instance;
                        if (!mi.IsStatic && instance == null) continue;

                        mi.Invoke(instance, new object[] { def });
                        added = true;
                        Mod.Log.Info($"Polaroid registered via {registryType.Name}.{name}().");
                        break;
                    }
                }
                if (!added)
                {
                    Mod.Log.Warn("No known Registry.Add method found — polaroid will still spawn " +
                                 "directly via IntegerItemInstance(definition, qty, value).");
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Native registry injection failed (non-fatal): {ex.Message}");
            }
        }
    }
    // ==== end HitmanPhone section: Evidence/BountyEvidenceItemRegistry.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Evidence/BountyReceiptService.cs ====
    using System;
    using System.Threading;
    using HitmanPhone.Items;
    using HitmanPhone.Persistence;
    using MelonLoader;
    using S1Mods.Shared;

#if (IL2CPPMELON)
    using S1DeadDrop = Il2CppScheduleOne.Economy.DeadDrop;
    using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
    using S1ItemInstance = Il2CppScheduleOne.ItemFramework.ItemInstance;
    using S1IntegerItemInstance = Il2CppScheduleOne.ItemFramework.IntegerItemInstance;
    using S1MoneyManager = Il2CppScheduleOne.Money.MoneyManager;
#elif MONOMELON
    using S1DeadDrop = ScheduleOne.Economy.DeadDrop;
    using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
    using S1ItemInstance = ScheduleOne.ItemFramework.ItemInstance;
    using S1IntegerItemInstance = ScheduleOne.ItemFramework.IntegerItemInstance;
    using S1MoneyManager = ScheduleOne.Money.MoneyManager;
#endif


    /// <summary>
    /// Phase F — DeadDrop receipt validation &amp; reward payout.
    ///
    /// Called from <see cref="DeadDropPatch"/>'s postfix on
    /// <see cref="S1StorageEntity.ContentsChanged"/> when the changed storage is
    /// owned by a DeadDrop. v0.1.6 design: instead of matching a single inserted
    /// item (the dead-drop UI never calls InsertItem), we inventory the storage
    /// and try to pay out any polaroid matching an active contract.
    ///
    /// Payout flow per scanned polaroid:
    ///   1. Read the integer <c>Value</c> and identify the item format.
    ///   2. Match v2 photos by persistent evidence token; match legacy photos only
    ///      to active migrated contracts that retain their original evidence ID.
    ///   3. If the contract's <c>RequiredDropId</c> is set, also confirm the actual
    ///      DeadDrop GUID matches (premium/spec contracts may require a specific drop).
    ///   4. Mark contract Completed, move it to History (idempotent vs. re-scans).
    ///   5. Call <see cref="S1MoneyManager.CreateOnlineTransaction"/> via server RPC to
    ///      authoritatively credit the local player's online balance. We use the
    ///      transaction name "bounty_reward" so the ledger is audit-friendly.
    /// </summary>
    public static class BountyReceiptService
    {
        private static int _receiptsSeen;
        private static int _receiptsMatched;
        private static int _receiptsMismatched;
        private static int _payoutsIssued;
        private static int _payoutAmountIssuedTotal;
        private static bool _hookAliveLogged;

        /// <summary>
        /// Audit (2026-09-10, HIGH): host-authority guard for the payout path.
        /// Storage write hooks fire on host AND clients in multiplayer — without
        /// this gate both sides ran TryValidateAndPay (double ChangeCashBalance
        /// rewards), and the client's local ClearStoredInstance never replicated,
        /// so the same polaroid paid again on the host. Only the host validates,
        /// pays and consumes. Consolidated 2026-09-15 in
        /// S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer — unified to fail-closed
        /// (previously fail-open here): on an authority exception without own save
        /// the payout is now aborted instead of blindly continuing.
        /// The MONOMELON branch was removed — the workspace build is exclusively IL2CPP.
        /// </summary>
        private static bool IsHostOrSingleplayer() => NetworkGuard.IsHostOrSingleplayer();

        public static int ReceiptsSeen => Volatile.Read(ref _receiptsSeen);
        public static int ReceiptsMatched => Volatile.Read(ref _receiptsMatched);
        public static int ReceiptsMismatched => Volatile.Read(ref _receiptsMismatched);
        public static int PayoutsIssued => Volatile.Read(ref _payoutsIssued);
        public static int PayoutAmountIssuedTotal => Volatile.Read(ref _payoutAmountIssuedTotal);

        /// <summary>
        /// Entry point from <see cref="DeadDropPatch"/> (postfix on
        /// StorageEntity.ContentsChanged). v0.1.6: the dead-drop UI fills slots via
        /// SetStoredInstance and never calls InsertItem (proven by the 0.1.5 live
        /// test), so instead of waiting for a single insert we inventory the
        /// dead-drop storage on every change and try to pay out any polaroid that
        /// matches an active contract. Idempotent: a payout removes the contract
        /// from Active, so repeated change events cannot double-pay.
        /// </summary>
        public static void OnStorageContentsChanged(S1StorageEntity entity)
        {
            if (entity == null) return;
            // Audit (2026-09-10, HIGH): only the host may validate/pay/consume —
            // client-side payout dupes the reward and its consume never replicates.
            // Bug-Audit 2026-09-12 (Audit-Runde 2): this gate blocked the CLIENT from ever
            // getting a payout. The host's local Save only knows the host's contracts, so
            // the client-side save (which tracks contracts accepted on this client) never
            // sees the AwaitingDrop-flag and returns early. Fix: the client may validate
            // and pay using its OWN Save; the host keeps validating for any contract not
            // already claimed by a client (e.g. contracts accepted on the host itself).
            // ChangeCashBalance syncs the player balance through FishNet, so the money
            // moves correctly on the client; the consume (ClearStoredInstance on the slot
            // inside the dead-drop StorageEntity) is also local to the client. In practice
            // most clients own their contracts, so this unblocks the main flow.
            bool isHost = IsHostOrSingleplayer();
            if (!isHost && Mod.Instance?.Save == null) return;
            Interlocked.Increment(ref _receiptsSeen);

            // v0.1.7 diagnostic: one-shot proof that the write hooks are wired and
            // firing at all (v0.1.6 failed silently — hook armed but never invoked).
            if (!_hookAliveLogged)
            {
                _hookAliveLogged = true;
                Mod.Log.Info("[Receipt] Storage write hook alive — first storage event observed.");
            }

            // v0.1.8: skip the scan entirely when no contract awaits evidence.
            // Storage writes are frequent (every inventory move in any UI fires 2-3
            // of our hooks in singleplayer — server + observers RPC both run locally),
            // so without this gate the log fills with no-op scans and dead-drop lookups.
            // Audit H1 (2026-09-01): gate on AwaitingDrop (survives reloads), NOT on
            // EvidenceSpawned (reset on every load) — otherwise a pre-reload polaroid
            // deposit is gated off and never pays out.
            var saveEarly = Mod.Instance?.Save;
            if (saveEarly != null)
            {
                bool awaiting = false;
                for (int i = 0; i < saveEarly.Active.Count; i++)
                {
                    var c = saveEarly.Active[i];
                    if (c.Status == EBountyStatus.Active && c.AwaitingDrop) { awaiting = true; break; }
                }
                if (!awaiting) return;
            }

            var drop = DeadDropIdentifier.FindOwningDeadDrop(entity);
            if (drop == null) return; // some other storage changed — not our concern

            string dropName;
            try { dropName = drop.DeadDropName ?? "?"; }
            catch { dropName = "?"; }
            Mod.Log.Debug($"[Receipt] DeadDrop '{dropName}' contents changed — scanning inventory.");

            var items = GetAllItemsSafe(entity);
            if (items == null)
            {
                Mod.Log.Warn("[Receipt] could not enumerate storage contents.");
                return;
            }
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item == null) continue;
                var matches = TryValidateAndPay(drop, entity, item);
                if (matches == null || matches.Count == 0) continue;

                // Apply side effects for every matched contract.
                string dropGuid = DeadDropIdentifier.GetGuidString(drop);
                int encodedId = ReadIntValue(item);
                bool isLegacyItem = IsLegacyPolaroid(item);
                var save = Mod.Instance?.Save;
                if (save == null) return;
                bool anyPayoutFailed = false;
                bool anyPayout = false;
                for (int m = 0; m < matches.Count; m++)
                {
                    var match = matches[m];
                    // First confirmed sighting of the polaroid anywhere (inventory or
                    // this drop) starts the post-photo drop window (two-phase
                    // deadlines). Idempotent, legacy contracts unaffected.
                    BountyService.EnsureDropWindowStarted(match);
                    if (!string.IsNullOrEmpty(match.RequiredDropId) &&
                        !string.Equals(dropGuid, match.RequiredDropId, StringComparison.OrdinalIgnoreCase))
                    {
                        Interlocked.Increment(ref _receiptsMismatched);
                        Mod.Log.Warn($"Receipt: contract {match.Id} requires drop {match.RequiredDropId}, " +
                                     $"got {dropGuid}.");
                        continue;
                    }

                    // Audit H4: pay FIRST, transition AFTER. A failed payout leaves the
                    // contract Active so the next storage event retries.
                    if (!IssueReward(match))
                    {
                        Mod.Log.Warn($"[Bounty#{match.Id}] Payout failed — contract stays Active, " +
                                     "deposit will be retried on the next storage event.");
                        anyPayoutFailed = true;
                        continue;
                    }

                    match.Status = EBountyStatus.Completed;
                    match.RequiredDropId ??= dropGuid;
                    save.LastUsedDropId = dropGuid; // next offer avoids this drop (variety)
                    save.Active.Remove(match);
                    save.History.Add(match);
                    anyPayout = true;
                    Interlocked.Increment(ref _receiptsMatched);
                    Mod.Log.Info($"[Bounty#{match.Id}] Receipt matched for target '{match.TargetNpcId}' " +
                                 $"(drop={dropGuid}). Reward=${match.RewardCash}.");

                    BountyHeatService.OnBountyCompleted(match);
                    BountyJournalBridge.CompleteQuest(match.Id);
                    int callerIdx = ResolveCallerIndex(match.CallerId);
                    if (callerIdx >= 0)
                    {
                        BountyCallScheduler.CooldownCaller(callerIdx,
                            BountyCallSchedulerConstants.CallerCooldownDaysAfterDecline);
                    }
                }

                // Persist once after the batch (cheaper than per-contract) unless a
                // payout failed mid-batch — in that case skip persistence so the failed
                // contract remains visible in Active for the next retry.
                if (anyPayout && !anyPayoutFailed)
                {
                    BountyPersistence.PersistCurrent();
                    // One polaroid covers the whole matched group (single kill → single evidence).
                    ConsumePolaroids(entity, encodedId, isLegacyItem);
                }
                else if (!anyPayout)
                {
                    // Every matched contract was rejected (wrong assigned drop): the
                    // polaroid STAYS where it is — no payout AND no evidence loss
                    // (plan constraint; the player can move it to the assigned drop).
                    Mod.Log.Info($"[Receipt] polaroid kept in '{dropName}' — matched contract(s) require their assigned drop.");
                }
                return; // one payout-group per storage event
            }
            Mod.Log.Debug($"[Receipt] scan of '{dropName}' found no payable polaroid ({items.Count} items).");
        }

        /// <summary>
        /// Enumerate storage contents defensively. The wrapper type of GetAllItems()
        /// resists static naming (CS0234 despite Il2Cppmscorlib being referenced),
        /// so we duck-type via reflection: Count/Length property + get_Item indexer.
        /// A mismatch logs the real type name instead of crashing the postfix.
        /// </summary>
        private static System.Collections.Generic.IList<S1ItemInstance>? GetAllItemsSafe(S1StorageEntity entity)
        {
            try
            {
                var raw = entity.GetAllItems();
                if (raw == null) return null;

                var countProp = raw.GetType().GetProperty("Count") ?? raw.GetType().GetProperty("Length");
                if (countProp == null)
                {
                    Mod.Log.Warn($"[Receipt] GetAllItems type exposes no Count/Length: {raw.GetType().FullName}.");
                    return null;
                }
                int count = Convert.ToInt32(countProp.GetValue(raw));

                var getItem = raw.GetType().GetMethod("get_Item");
                if (getItem == null)
                {
                    Mod.Log.Warn($"[Receipt] GetAllItems type exposes no indexer: {raw.GetType().FullName}.");
                    return null;
                }

                var result = new System.Collections.Generic.List<S1ItemInstance>(count);
                for (int i = 0; i < count; i++)
                {
                    var item = getItem.Invoke(raw, new object?[] { i }) as S1ItemInstance;
                    result.Add(item);
                }
                return result;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[Receipt] GetAllItems failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Validate one scanned item against the active contracts and pay out on a
        /// match. Returns the list of contracts that were paid (empty list = miss).
        ///
        /// Match strategy:
        ///   - exact: polaroid Value matches a contract's TargetNpcInstanceId → that one.
        ///   - fallback: Value==0 (cross-session loss) → all awaiting contracts on the
        ///     same TargetNpcId. Mixed NPC ids are refused (ambiguous) and logged.
        ///
        /// The caller is responsible for issuing side effects (IssueReward, journal
        /// completion, heat drop, cooldown) per contract and for consuming ONE polaroid
        /// for the whole matched group (a single piece of evidence covers all bounties
        /// on the same head — the kill produced one body).
        /// </summary>
        public static System.Collections.Generic.List<BountyContract>? TryValidateAndPay(
            S1DeadDrop drop, S1StorageEntity entity, S1ItemInstance item)
        {
            if (drop == null || entity == null || item == null) return null;
            if (!IsPolaroid(item)) return null;

            if (Mod.Instance?.Save == null) return null;
            var save = Mod.Instance.Save;

            // The item definition identifies its format: v2 values are durable tokens;
            // legacy values are old session-specific Unity IDs (or zero after data loss).
            int encodedValue = ReadIntValue(item);
            bool isLegacyItem = IsLegacyPolaroid(item);
            var matches = BountyEvidenceMatcher.Match(save, encodedValue, isLegacyItem);
            if (matches == null || matches.Count == 0)
            {
                Interlocked.Increment(ref _receiptsMismatched);
                Mod.Log.Warn($"Receipt: no active contract matches {(isLegacyItem ? "legacy instance id" : "evidence token")} {encodedValue}.");
                return null;
            }
            if (isLegacyItem && encodedValue == 0)
            {
                Mod.Log.Warn("Receipt: accepting a zero-value legacy polaroid only for its migrated active contract.");
            }
            return matches;
        }

        /// <summary>
        /// Hand the contract's <c>RewardCash</c> to the player as physical cash via
        /// <see cref="S1API.Money.Money.ChangeCashBalance(float, bool, bool)"/>.
        ///
        /// v0.1.9: bounty pays DIRTY CASH, not an online transfer (Dominik: "Kopfgeld
        /// ist im echten Leben dreckiges Geld" — bounty money is dirty money in real
        /// life). Fitting side effect: cash leaves no
        /// transaction record in the banking ledger — no paper trail. The 0.1.8
        /// CreateOnlineTransaction path (typed S1API call) is kept in git history.
        ///
        /// Audit H4 (2026-09-01): returns true only on a confirmed payout so the
        /// caller can hold the contract transition until the money actually moved.
        /// </summary>
        private static bool IssueReward(BountyContract contract)
        {
            try
            {
                if (S1MoneyManager.Instance == null)
                {
                    Mod.Log.Warn("MoneyManager.Instance is null (no gameplay MoneyManager yet?) — payout skipped.");
                    return false;
                }

                S1API.Money.Money.ChangeCashBalance(
                    contract.RewardCash, visualizeChange: true, playCashSound: true);

                Interlocked.Increment(ref _payoutsIssued);
                Interlocked.Add(ref _payoutAmountIssuedTotal, (int)Math.Round(contract.RewardCash));
                Mod.Log.Info($"[Bounty#{contract.Id}] Payout ${contract.RewardCash} in CASH via ChangeCashBalance " +
                             "(dirty money — no ledger record).");
                return true;
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"IssueReward failed: {ex}");
                return false;
            }
        }

        /// <summary>Consumes only the exact photo that was just validated.</summary>
        private static void ConsumePolaroids(S1StorageEntity entity, int encodedValue, bool isLegacyItem)
        {
            try
            {
                var slots = entity.ItemSlots;
                if (slots == null) return;
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null) continue;
                    var inst = slot.ItemInstance;
                    if (inst == null || !IsPolaroid(inst)) continue;
                    if (IsLegacyPolaroid(inst) != isLegacyItem) continue;
                    if (ReadIntValue(inst) != encodedValue) continue;

                    slot.ClearStoredInstance();
                    Mod.Log.Info($"[Receipt] Consumed polaroid evidence value {encodedValue}.");
                    return;
                }
                Mod.Log.Warn("[Receipt] payout done but the validated polaroid slot was not found to consume.");
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"ConsumePolaroids failed (polaroid stays in drop): {ex.Message}");
            }
        }

        /// <summary>
        /// Removes every polaroid encoding this contract's target instance id from all
        /// registered dead drops. Polaroids of other targets are never touched.
        /// Callers own the host and shared-target guards (BountyService.RemovePolaroidFromDeadDrops).
        /// </summary>
        internal static void RemoveDeadDropPolaroids(BountyContract c, string reason)
        {
            var drops = S1DeadDrop.DeadDrops;
            if (drops == null) return;
            int removed = 0;
            for (int d = 0; d < drops.Count; d++)
            {
                var drop = drops[d];
                if (drop == null || drop.Pointer == IntPtr.Zero || drop.WasCollected) continue;
                var entity = drop.Storage;
                if (entity == null || entity.Pointer == IntPtr.Zero || entity.WasCollected) continue;
                var slots = entity.ItemSlots;
                if (slots == null) continue;
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null) continue;
                    var inst = slot.ItemInstance;
                    if (inst == null || !MatchesEvidenceForContract(c, inst)) continue;
                    slot.ClearStoredInstance();
                    removed++;
                }
            }
            if (removed > 0)
            {
                Mod.Log.Info($"[Bounty#{c.Id}] Removed {removed} polaroid(s) from dead-drop storage ({reason}).");
            }
        }

        /// <summary>
        /// Reflection-based check: does the inserted item have a Definition whose ID
        /// matches our polaroid id? ItemInstance does not expose a Definition directly
        /// in the stub, but the property <c>Definition</c> lives on the base class.
        /// </summary>
        internal static bool IsPolaroid(S1ItemInstance item)
        {
            string? id = ReadDefinitionId(item);
            bool recognized = string.Equals(id, BountyEvidenceItemRegistry.ItemId, StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(id, BountyEvidenceItemRegistry.LegacyItemId, StringComparison.OrdinalIgnoreCase);
            if (!recognized && !string.IsNullOrEmpty(id))
                Mod.Log.Debug($"[Receipt] Item definition id '{id}' is not a bounty polaroid.");
            return recognized;
        }

        internal static bool IsLegacyPolaroid(S1ItemInstance item) =>
            string.Equals(ReadDefinitionId(item), BountyEvidenceItemRegistry.LegacyItemId, StringComparison.OrdinalIgnoreCase);

        internal static bool MatchesEvidenceForContract(BountyContract contract, S1ItemInstance item)
        {
            if (contract == null || item == null || !IsPolaroid(item)) return false;
            int value = ReadIntValue(item);
            if (IsLegacyPolaroid(item))
            {
                int legacyId = contract.LegacyTargetInstanceId;
                if (legacyId == 0 && contract.EvidenceToken <= 0)
                    legacyId = contract.TargetNpcInstanceId;
                return legacyId != 0 && value == legacyId;
            }
            return contract.EvidenceToken > 0 && value == contract.EvidenceToken;
        }

        private static string? ReadDefinitionId(S1ItemInstance item)
        {
            try
            {
                var def = item.GetType().GetProperty("Definition",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic);
                object? definition = def?.GetValue(item);
                if (definition == null) return null;
                var idProp = definition.GetType().GetProperty("ID",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic);
                return idProp?.GetValue(definition) as string;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Polaroid definition lookup failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>Read <c>IntegerItemInstance.Value</c> via the typed property.</summary>
        internal static int ReadIntValue(S1ItemInstance item)
        {
            try
            {
                if (item is S1IntegerItemInstance integer)
                {
                    return integer.Value;
                }
                // Fallback: reflection on .Value (less common but safe).
                var prop = item.GetType().GetProperty("Value",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic);
                object? v = prop?.GetValue(item);
                return v is int i ? i : 0;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"ReadIntValue failed: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Reconstruct a caller index from the CallerId string "caller_ghost" → 0 etc.
        /// Returns -1 when the id is not one of our known callers.
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
    }
    // ==== end HitmanPhone section: Evidence/BountyReceiptService.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Evidence/DeadDropIdentifier.cs ====
    using System;
    using System.Reflection;

#if (IL2CPPMELON)
    using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
    using S1DeadDrop = Il2CppScheduleOne.Economy.DeadDrop;
    using S1ItemInstance = Il2CppScheduleOne.ItemFramework.ItemInstance;
#elif MONOMELON
    using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
    using S1DeadDrop = ScheduleOne.Economy.DeadDrop;
    using S1ItemInstance = ScheduleOne.ItemFramework.ItemInstance;
#endif


    /// <summary>
    /// Phase F — Receipt Identifier.
    ///
    /// BUGFIX (2026-08-31, v0.1.4): the previous check walked the inheritance chain
    /// assuming <c>Economy.DeadDrop</c> derives from <c>StorageEntity</c>. The
    /// decompiled stub proves the opposite (Archive/Temp_DeadDrop.cs line 16:
    /// <c>DeadDrop : MonoBehaviour</c>) — the drop OWNS a <c>WorldStorageEntity</c>
    /// in its <c>Storage</c> field. The inheritance test therefore returned false
    /// for every object in the game and the payout chain never ran.
    ///
    /// Correct semantics: a storage entity belongs to a dead drop when one of the
    /// registered <see cref="S1DeadDrop.DeadDrops"/> holds exactly that entity.
    /// </summary>
    internal static class DeadDropIdentifier
    {
        /// <summary>
        /// Returns the registered DeadDrop that owns this storage entity, or null.
        /// Comparison via Unity instance id (stable within a session, unlike the
        /// Il2Cpp wrapper references which may differ per access).
        /// </summary>
        public static S1DeadDrop? FindOwningDeadDrop(S1StorageEntity entity)
        {
            if (entity == null || entity.Pointer == IntPtr.Zero || entity.WasCollected) return null;
            try
            {
                var drops = S1DeadDrop.DeadDrops;
                if (drops == null) return null;
                int instanceId = entity.GetInstanceID();
                for (int i = 0; i < drops.Count; i++)
                {
                    try
                    {
                        var dd = drops[i];
                        if (dd == null || dd.Pointer == IntPtr.Zero || dd.WasCollected) continue;
                        var st = dd.Storage;
                        if (st == null || st.Pointer == IntPtr.Zero || st.WasCollected) continue;
                        if (st.GetInstanceID() == instanceId) return dd;
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"FindOwningDeadDrop failed: {ex.Message}");
            }
            return null;
        }

        /// <summary>True if the given storage entity belongs to any registered dead drop.</summary>
        public static bool IsDeadDropStorage(S1StorageEntity entity) => FindOwningDeadDrop(entity) != null;

        /// <summary>
        /// The dead drop's GUID. Lives on the <see cref="S1DeadDrop"/> component itself,
        /// NOT on the storage entity (the old entity-based reflection lookup could
        /// never find it for the same reason as the inheritance bug).
        /// </summary>
        public static string GetGuidString(S1DeadDrop drop)
        {
            if (drop == null) return string.Empty;
            try
            {
                return drop.GUID.ToString();
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"GetGuidString failed: {ex.Message}");
                return string.Empty;
            }
        }
    }
    // ==== end HitmanPhone section: Evidence/DeadDropIdentifier.cs ====
}

namespace HitmanPhone.Bounty
{
    // ==== HitmanPhone section: Evidence/DeadDropPatch.cs ====
    using System;

#if (IL2CPPMELON)
    using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
#elif MONOMELON
    using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
#endif


    /// <summary>
    /// Phase F — Storage write hooks (v0.1.7 design).
    ///
    /// History: v0.1.0–v0.1.5 hooked <c>StorageEntity.InsertItem</c>; the 0.1.5 live
    /// test proved the dead-drop UI never calls it. v0.1.6 hooked
    /// <c>StorageEntity.ContentsChanged()</c>; the 0.1.6 live test proved the game
    /// never invokes it for dead drops either — the method exists (interop dump:
    /// <c>ContentsChanged() : Void</c>), the patch applied cleanly (NPCDeathPatch
    /// from the same PatchAll demonstrably ran), but the dead-drop UI writes slots
    /// through the FishNet RPC chain
    /// (<c>SetStoredInstance → SetStoredInstance_Internal</c> and
    /// <c>SetItemSlotQuantity → SetItemSlotQuantity_Internal</c>) and never raises
    /// ContentsChanged. ContentsChanged is only serviced by UpdateWhileOpen, which
    /// runs solely for storages opened through the regular storage UI — dead drops
    /// use their own UI and never call Open().
    ///
    /// New design: hook the two actual write paths plus the old signal. Every slot
    /// write in the game flows through SetStoredInstance_Internal (full instance
    /// write) or SetItemSlotQuantity_Internal (quantity-only write), regardless of
    /// which UI issued it. The three hooks are registered explicitly via
    /// PatchGuard.TryPatch in Mod.cs (house standard), so a missing method is
    /// logged at startup instead of being silently skipped.
    ///
    /// Idempotency: scanning on each write is safe — a payout removes the contract
    /// from Active, so repeated events cannot double-pay.
    ///
    /// Skill Rule #3: every postfix body is wrapped in try/catch so a thrown
    /// exception never destabilises the engine.
    /// </summary>
    internal static class DeadDropPatch
    {
        public static void PostfixContentsChanged(S1StorageEntity __instance)
            => SafeScan(__instance, "ContentsChanged");

        public static void PostfixSetStoredInstance(S1StorageEntity __instance)
            => SafeScan(__instance, "SetStoredInstance_Internal");

        public static void PostfixSetItemSlotQuantity(S1StorageEntity __instance)
            => SafeScan(__instance, "SetItemSlotQuantity_Internal");

        private static void SafeScan(S1StorageEntity instance, string source)
        {
            try
            {
                BountyReceiptService.OnStorageContentsChanged(instance);
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"DeadDropPatch[{source}] swallowed exception: {ex.Message}");
            }
        }
    }
    // ==== end HitmanPhone section: Evidence/DeadDropPatch.cs ====
}

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

        /// <summary>
        /// True only when both definitions exist AND were verified present inside the
        /// native <see cref="S1Registry"/>. Object liveness alone is NOT proof of
        /// registration (HP-02 part B: the old check reported success for a definition
        /// whose native injection had silently failed, and never retried).
        /// </summary>
        public static bool IsRegistered =>
            IsAlive(Definition) && IsAlive(LegacyDefinition) && _registrationVerified &&
            RegistryHasItem(ItemId) && RegistryHasItem(LegacyItemId);

        /// <summary>Bounded retry budget per registry lifecycle (HP-02: retry transient failures).</summary>
        private const int MaxRegistrationAttempts = 5;

        private static bool _registrationVerified;
        private static int _registrationAttempts;
        private static bool _retryExhaustedLogged;
        private static bool _unverifiedSpawnWarned;
        private static long _nextRegistrationAttempt;

        /// <summary>
        /// HP-02: re-establish registration against the CURRENT registry lifecycle.
        /// The native Registry drops runtime-added items on scene teardown
        /// (<c>RemoveRuntimeItems</c>), so verification must be re-proven per scene and
        /// not cached for the whole session. Wire this to the main-scene-loaded /
        /// pre-load lifecycle in Mod.
        /// </summary>
        public static bool OnMainSceneLoaded()
        {
            InvalidateRegistration();
            return EnsureRegistered();
        }

        public static void InvalidateRegistration()
        {
            _registrationVerified = false;
            _registrationAttempts = 0;
            _retryExhaustedLogged = false;
            _unverifiedSpawnWarned = false;
            _nextRegistrationAttempt = Environment.TickCount64 + 1000;
        }

        public static void TickRegistration()
        {
            if (!S1Mods.Shared.NetworkGuard.IsInMainScene || Environment.TickCount64 < _nextRegistrationAttempt) return;
            _nextRegistrationAttempt = Environment.TickCount64 + 1000;
            if (!IsRegistered) EnsureRegistered(); // bounded by MaxRegistrationAttempts
        }

        /// <summary>
        /// IL2CPP-safe liveness check for the cached Unity objects. A scene change
        /// can destroy the native Definition/Sprite while the managed static stays
        /// non-null — using it then throws. Mirrors the Pointer/WasCollected
        /// pattern in <see cref="Bounty.BountyReceiptService"/>.
        /// </summary>
        private static bool IsAlive(UnityEngine.Object? obj)
        {
            try
            {
#if (IL2CPPMELON)
                return obj != null && obj.Pointer != System.IntPtr.Zero && !obj.WasCollected;
#else
                return obj != null;
#endif
            }
            catch { return false; }
        }

        /// <summary>
        /// Idempotent, VERIFIED registration (HP-02). Returns true only when both
        /// polaroid definitions are present in the native registry. Creation and
        /// registration are separate facts here: a created-but-unregistered definition
        /// keeps <see cref="IsRegistered"/> false and is retried (bounded) on the next
        /// lifecycle call, instead of being cached as a false success.
        /// </summary>
        public static bool EnsureRegistered()
        {
            if (IsRegistered) return true;

            if (_registrationAttempts >= MaxRegistrationAttempts)
            {
                if (!_retryExhaustedLogged)
                {
                    _retryExhaustedLogged = true;
                    Mod.Log.Error($"Polaroid registration unverified after {MaxRegistrationAttempts} attempts; " +
                                  "waiting for the next scene lifecycle before retrying.");
                }
                return false;
            }
            _registrationAttempts++;

            if (!IsAlive(IconSprite))
            {
                IconSprite = LoadPolaroidSprite();
                if (IconSprite == null)
                {
                    Mod.Log.Warn("Polaroid sprite could not be loaded; " +
                                 "the item will be created with a fallback (null) icon.");
                }
            }

            bool currentOk = TryCreateAndRegister(ItemId, out var current);
            // Keep the old definition registered so saved legacy polaroids can be
            // deserialized after upgrading to the token-based item ID.
            bool legacyOk = TryCreateAndRegister(LegacyItemId, out var legacy);
            Definition = current;
            LegacyDefinition = legacy;
            _registrationVerified = currentOk && legacyOk;

            if (_registrationVerified)
            {
                Mod.Log.Info($"Polaroid definitions registered and verified in the native registry " +
                             $"(current='{ItemId}', legacy='{LegacyItemId}', " +
                             $"icon={(IconSprite != null ? "loaded" : "missing")}).");
            }
            else
            {
                Mod.Log.Warn($"Polaroid registration NOT verified (attempt {_registrationAttempts}/{MaxRegistrationAttempts}): " +
                             $"current={currentOk}, legacy={legacyOk}. Retrying on the next lifecycle call.");
            }
            return _registrationVerified;
        }

        /// <summary>
        /// Creates one definition and reports whether it was verified in the native
        /// registry. Existing entries are reused; the cached definition is the same
        /// definition resolved by ID, not an unregistered duplicate template.
        /// </summary>
        private static bool TryCreateAndRegister(string id, out S1ItemFramework.IntegerItemDefinition? def)
        {
            def = null;
            try
            {
                S1ItemFramework.IntegerItemDefinition? Lookup()
                {
                    var registered = S1Registry.GetItem(id);
                    if (!IsAlive(registered)) return null;
#if IL2CPPMELON
                    return registered.TryCast<S1ItemFramework.IntegerItemDefinition>();
#else
                    return registered as S1ItemFramework.IntegerItemDefinition;
#endif
                }
                if (RegistryHasItem(id))
                {
                    def = Lookup();
                    return IsAlive(def);
                }
                var created = ScriptableObject.CreateInstance<S1ItemFramework.IntegerItemDefinition>();
                created.hideFlags = HideFlags.HideAndDontSave;
                created.DefaultValue = 0;

                TrySetReflected(created, "ID", id);
                TrySetReflected(created, "Name", ItemName);
                TrySetReflected(created, "Description", ItemDescription);
                TrySetReflected(created, "StackLimit", 1);
                TrySetReflected(created, "BasePurchasePrice", 0f);
                TrySetReflected(created, "ResellMultiplier", 0f);
                TrySetReflected(created, "Icon", IconSprite);
                TrySetCategory(created, S1API.Items.ItemCategory.Decoration);
                if (!RegisterIntoNativeRegistry(created, id)) return false;
                def = Lookup();
                return IsAlive(def);
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"Failed to create/register Polaroid definition '{id}': {ex}");
                return false;
            }
        }

        /// <summary>Spawn a version-2 polaroid carrying a persistent evidence token.</summary>
        public static S1ItemFramework.IntegerItemInstance? Spawn(int evidenceToken)
        {
            if (!EnsureRegistered())
            {
                if (!_unverifiedSpawnWarned)
                {
                    _unverifiedSpawnWarned = true;
                    Mod.Log.Warn("Polaroid delivery deferred: both item IDs must be verified before spawning savable evidence.");
                }
                return null;
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

        private static void TrySetReflected(object target, string propName, object? value)
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
        /// Native registry injection with VERIFIED success (HP-02). The native
        /// <see cref="S1Registry"/> exposes <c>AddToRegistry(ItemDefinition)</c> and the
        /// static <c>ItemExists(string)</c> / <c>GetItem(string)</c> lookups; we add the
        /// definition and then prove it resolves. A method call that merely did not
        /// throw is NOT success — the old code returned a live object and reported
        /// registration as done. Direct spawning still works without a registry entry,
        /// so a failure here is reported, not fatal.
        /// </summary>
        private static bool RegisterIntoNativeRegistry(S1ItemFramework.ItemDefinition def, string id)
        {
            try
            {
                // Idempotent: if the id already resolves (survived a scene change),
                // never add a second registry entry.
                if (RegistryHasItem(id)) return true;

                var registryType = typeof(S1Registry);
                string[] candidates = { "AddToRegistry", "AddItemToRegistry", "Register", "AddItem", "Add" };
                bool invoked = false;
                foreach (string name in candidates)
                {
                    var mi = registryType.GetMethod(name,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                        binder: null,
                        types: new[] { typeof(S1ItemFramework.ItemDefinition) },
                        modifiers: null);
                    if (mi == null) continue;

                    object? instance = mi.IsStatic ? null : S1Registry.Instance;
                    if (!mi.IsStatic && instance == null) continue;

                    mi.Invoke(instance, new object[] { def });
                    invoked = true;
                    Mod.Log.Debug($"Polaroid '{id}' added via {registryType.Name}.{name}().");
                    break;
                }

                if (!invoked)
                {
                    Mod.Log.Warn($"No known {registryType.Name} add method found for '{id}' — " +
                                 "polaroid will still spawn directly via " +
                                 "IntegerItemInstance(definition, qty, value).");
                    return false;
                }

                if (RegistryHasItem(id)) return true;
                Mod.Log.Warn($"Polaroid '{id}' add call completed but the registry does not resolve it yet.");
                return false;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Native registry injection failed for '{id}' (non-fatal): {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Authoritative registry lookup used as the only acceptable registration
        /// signal: <c>Registry.ItemExists(string)</c>, with a non-generic
        /// <c>Registry.GetItem(string)</c> fallback. Returns false when neither API is
        /// reachable — an unverifiable registration is reported as NOT registered.
        /// </summary>
        private static bool RegistryHasItem(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            try
            {
                var registryType = typeof(S1Registry);
                var exists = registryType.GetMethod("ItemExists",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                    binder: null, types: new[] { typeof(string) }, modifiers: null);
                if (exists != null && exists.ReturnType == typeof(bool))
                    return (bool)(exists.Invoke(null, new object[] { id }) ?? false);

                var get = registryType.GetMethod("GetItem",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                    binder: null, types: new[] { typeof(string) }, modifiers: null);
                if (get != null && !get.IsGenericMethod)
                    return get.Invoke(null, new object[] { id }) != null;
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"Registry lookup for '{id}' failed: {ex.Message}");
            }
            return false;
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
        /// HP-03 re-entrancy latch: consuming a polaroid writes the slot again and
        /// re-fires the storage hooks. The nested scan would re-walk the same container
        /// while our outer scan already covers every item, so the nested event is
        /// dropped instead of being processed twice.
        /// </summary>
        private static int _scanActive;

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
        /// Entry point from <see cref="DeadDropPatch"/> (postfix on the dead-drop
        /// storage write paths). We inventory the whole storage on every change and pay
        /// out every polaroid that matches an active contract.
        ///
        /// HP-03: the scan no longer stops at the first matching photo. A photo bound to
        /// another contract's assigned drop (or one whose deadline has passed) is
        /// recorded and left where it is, while later valid evidence in the same
        /// container is still paid — the old <c>return</c> after the first hit silently
        /// blocked every valid deposit behind it.
        ///
        /// Idempotent: a payout moves the contract from Active to History, so repeated
        /// change events cannot double-pay.
        /// </summary>
        public static void OnStorageContentsChanged(S1StorageEntity entity)
        {
            if (entity == null) return;
            // Singleplayer-only + confirmed-load gate (SaveStateGuard.IsReady includes
            // the SP check). A multiplayer client never reaches the payout path.
            if (!HitmanPhone.SaveStateGuard.IsReady) return;
            // Host-authority guard (see the class note): clients may only pay for
            // contracts in their OWN save; without one the payout is aborted.
            if (!IsHostOrSingleplayer() && Mod.Instance?.Save == null) return;

            // HP-03: drop nested events caused by our own consume write.
            if (Interlocked.CompareExchange(ref _scanActive, 1, 0) != 0) return;
            try
            {
                Interlocked.Increment(ref _receiptsSeen);

                // v0.1.7 diagnostic: one-shot proof that the write hooks are wired.
                if (!_hookAliveLogged)
                {
                    _hookAliveLogged = true;
                    Mod.Log.Info("[Receipt] Storage write hook alive — first storage event observed.");
                }

                // v0.1.8: skip the scan when no contract awaits evidence. Gate on
                // AwaitingDrop (survives reloads), NOT on EvidenceSpawned (audit H1).
                var save = Mod.Instance?.Save;
                if (save != null)
                {
                    bool awaiting = false;
                    for (int i = 0; i < save.Active.Count; i++)
                    {
                        var c = save.Active[i];
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
                if (save == null) return;

                // HP-13: the payout decision needs a valid in-game time snapshot. An
                // invalid read must never be treated as "minute zero" — defer instead.
                if (!HitmanPhoneTime.TryGetSnapshot(out _, out _, out long now))
                {
                    Mod.Log.Warn("[Receipt] in-game time unavailable — payout deferred.");
                    return;
                }

                string dropGuid = DeadDropIdentifier.GetGuidString(drop);

                // Build the pure scan plan: every polaroid in the container decided
                // independently (HP-03 — a rejected photo must not stop the scan).
                var scanItems = new System.Collections.Generic.List<DepositScanItem>(items.Count);
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (item == null) continue;
                    if (!TryReadEvidence(item, out bool evidence, out int value, out bool legacy) || !evidence) continue;
                    var matches = TryValidateAndPay(save, drop, item, now);
                    if (matches == null || matches.Count == 0) continue;
                    scanItems.Add(new DepositScanItem
                    {
                        ItemIndex = i,
                        EncodedValue = value,
                        IsLegacy = legacy,
                        Matched = matches,
                    });
                }

                var plan = BountyDepositPolicy.PlanScan(scanItems, dropGuid, now);
                for (int i = 0; i < plan.WrongDrop.Count; i++)
                {
                    Interlocked.Increment(ref _receiptsMismatched);
                    Mod.Log.Warn($"Receipt: contract {plan.WrongDrop[i].Id} requires drop " +
                                 $"{plan.WrongDrop[i].RequiredDropId}, got {dropGuid}.");
                }
                for (int i = 0; i < plan.Expired.Count; i++)
                {
                    Interlocked.Increment(ref _receiptsMismatched);
                    Mod.Log.Warn($"Receipt: contract {plan.Expired[i].Id} is past its deadline; not paying.");
                }

                if (plan.Items.Count == 0)
                {
                    if (plan.WrongDrop.Count > 0)
                    {
                        // The photo STAYS where it is — no payout AND no evidence loss
                        // (the player can move it to the assigned drop).
                        Mod.Log.Info($"[Receipt] polaroid kept in '{dropName}' — matched contract(s) require their assigned drop.");
                    }
                    else
                    {
                        Mod.Log.Debug($"[Receipt] scan of '{dropName}' found no payable polaroid ({items.Count} items).");
                    }
                    return;
                }

                bool paidAny = false;
                for (int p = 0; p < plan.Items.Count; p++)
                {
                    var itemPlan = plan.Items[p];
                    bool anyPaid = false;
                    bool anyFailed = false;
                    for (int m = 0; m < itemPlan.Pay.Count; m++)
                    {
                        var match = itemPlan.Pay[m];
                        // First confirmed sighting of the polaroid anywhere starts the
                        // post-photo drop window. A late delivery cannot open a fresh
                        // window: EnsureDropWindowStarted checks the execution deadline
                        // before it starts anything (HP-13).
                        BountyService.EnsureDropWindowStarted(match);

                        if (!save.Active.Contains(match) || !HitmanPhoneTime.TryGetSnapshot(out _, out _, out long liveNow) ||
                            !BountyDeadlinePolicy.IsEligible(match, liveNow))
                        {
                            anyFailed = true;
                            continue;
                        }
                        // Native-call uncertainty is retained for manual review. Only
                        // a preflight failure can be retried automatically.
                        if (!IssueReward(match))
                        {
                            Mod.Log.Warn($"[Bounty#{match.Id}] Payout not completed; photo retained. " +
                                         (match.RewardPaymentUncertain ? "Manual balance review required; no automatic retry." : "Preflight can retry on a later storage event."));
                            anyFailed = true;
                            continue;
                        }

                        match.Status = EBountyStatus.Completed;
                        match.RequiredDropId ??= dropGuid;
                        match.PendingEvidenceDelivery = false;
                        save.LastUsedDropId = dropGuid; // next offer avoids this drop (variety)
                        save.Active.Remove(match);
                        save.History.Add(match);
                        anyPaid = true;
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

                    // Consume only the exact photo that paid cleanly; a partial failure
                    // leaves the deposit for the next event (no half-finished booking).
                    if (anyPaid && !anyFailed && itemPlan.Consume)
                    {
                        paidAny = true;
                        ConsumePolaroids(entity, itemPlan.EncodedValue, itemPlan.IsLegacy);
                    }
                }

                // Persist once after the whole batch (dirty-tracking marks the checkpoint).
                if (paidAny) BountyPersistence.PersistCurrent();
            }
            finally
            {
                Interlocked.Exchange(ref _scanActive, 0);
            }
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
                    if (item != null) result.Add(item);
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
        /// Match one scanned item against the active contracts. Returns the list of
        /// contracts this photo belongs to (null/empty = miss).
        ///
        /// Match strategy:
        ///   - exact: the polaroid Value equals a contract's persistent evidence token
        ///     (or, for a legacy photo, its preserved old instance id).
        ///   - fallback: Value==0 (cross-session loss) → all awaiting migrated contracts
        ///     on the same target. Mixed target ids are refused (ambiguous) and logged.
        ///
        /// HP-13: <paramref name="nowMinSum"/> is passed through to the matcher, so a
        /// contract whose governing deadline has passed never matches and is never paid.
        ///
        /// The caller is responsible for issuing side effects (IssueReward, journal
        /// completion, heat drop, cooldown) per contract and for consuming ONE polaroid
        /// for the whole matched group (a single piece of evidence covers all bounties
        /// on the same head — the kill produced one body).
        /// </summary>
        public static System.Collections.Generic.List<BountyContract>? TryValidateAndPay(
            BountySaveData save, S1DeadDrop drop, S1ItemInstance item, long nowMinSum)
        {
            if (save == null || drop == null || item == null) return null;
            if (!TryReadEvidence(item, out bool evidence, out int encodedValue, out bool isLegacyItem) || !evidence) return null;
            var matches = BountyEvidenceMatcher.Match(save, encodedValue, isLegacyItem, nowMinSum);
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
        /// A normal void API return is not native balance proof. An exception may
        /// have happened after a cash mutation: preserve the claim, forbid retry,
        /// and require manual review rather than risking a duplicate payout.
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
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Payout preflight unavailable; no money call attempted: {ex.Message}");
                return false;
            }
            var outcome = BountyRewardSettlement.TryPay(contract,
                amount => S1API.Money.Money.ChangeCashBalance(amount, visualizeChange: true, playCashSound: true));
            BountyPersistence.PersistCurrent();
            if (outcome != RewardAttempt.Paid)
            {
                if (outcome == RewardAttempt.Uncertain)
                    Mod.Log.Error($"[Bounty#{contract.Id}] cash call failed with unknown effect; claim/photo retained. Automatic retry disabled; manual balance review required.");
                return false;
            }
            // The runtime-used settlement seam already marked Completed. A later
            // logging/counter failure must not turn this into a retryable claim.
            try
            {
                Interlocked.Increment(ref _payoutsIssued);
                Interlocked.Add(ref _payoutAmountIssuedTotal, (int)Math.Round(contract.RewardCash));
                Mod.Log.Info($"[Bounty#{contract.Id}] Payout ${contract.RewardCash} in CASH via ChangeCashBalance " +
                             "(dirty money — no ledger record).");
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[Bounty#{contract.Id}] paid; payout observability failed: {ex.Message}");
            }
            return true;
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
                    if (inst == null || !TryReadEvidence(inst, out bool evidence, out int value, out bool legacy) || !evidence) continue;
                    if (legacy != isLegacyItem || value != encodedValue) continue;

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
            return ObserveEvidenceForContract(contract, item) == EvidencePresence.Present;
        }

        internal static EvidencePresence ObserveEvidenceForContract(BountyContract contract, S1ItemInstance item)
        {
            var save = Mod.Instance?.Save;
            if (save == null || contract == null) return EvidencePresence.Unknown;
            if (!TryReadEvidence(item, out bool evidence, out int value, out bool legacy)) return EvidencePresence.Unknown;
            return evidence ? BountyEvidenceMatcher.ObserveForContract(save, contract, value, legacy) : EvidencePresence.Missing;
        }

        internal static bool TryReadEvidence(S1ItemInstance item, out bool evidence, out int value, out bool legacy)
        {
            evidence = false;
            value = 0;
            legacy = false;
            string? id = ReadDefinitionId(item);
            if (string.IsNullOrEmpty(id)) return false;
            legacy = string.Equals(id, BountyEvidenceItemRegistry.LegacyItemId, StringComparison.OrdinalIgnoreCase);
            evidence = legacy || string.Equals(id, BountyEvidenceItemRegistry.ItemId, StringComparison.OrdinalIgnoreCase);
            if (!evidence) return true;
            try
            {
#if IL2CPPMELON
                var integer = item.TryCast<S1IntegerItemInstance>();
#else
                var integer = item as S1IntegerItemInstance;
#endif
                if (integer == null) return false;
                value = integer.Value; // a failed read must never masquerade as legacy zero
                return true;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Evidence value unavailable; scan deferred: {ex.Message}");
                return false;
            }
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

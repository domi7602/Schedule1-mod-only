// ---------------------------------------------------------------------------
// Runtime/BountyRuntime.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Runtime/BountyTestCommands.cs        -> section Runtime/BountyTestCommands.cs
//   Runtime/HitmanPhoneTime.cs           -> section Runtime/HitmanPhoneTime.cs
//   Runtime/SaveStateGuard.cs            -> section Runtime/SaveStateGuard.cs
//
// ---------------------------------------------------------------------------

namespace HitmanPhone
{
    // ==== HitmanPhone section: Runtime/BountyTestCommands.cs ====
    using System.Collections.Generic;
    using HitmanPhone.Bounty;
    using HitmanPhone.Items;
    using HitmanPhone.Persistence;
    using MelonLoader;
    using S1API.Console;
    using S1API.Quests;
    using S1Mods.Shared;

#if (IL2CPPMELON)
    using S1PlayerInventory = Il2CppScheduleOne.PlayerScripts.PlayerInventory;
#elif MONOMELON
    using S1PlayerInventory = ScheduleOne.PlayerScripts.PlayerInventory;
#endif


    /// <summary>
    /// Phase K — Test scaffolding (DEBUG builds only — never registered in Release).
    /// </summary>
#if DEBUG

    /// <summary>Force-fire a bounty call now.</summary>
    internal sealed class HitmanForceOfferCommand : BaseConsoleCommand
    {
        public override string CommandWord => "hitman_force_offer";
        public override string CommandDescription =>
            "Force-fire a bounty call now. Args: [callerIdx=0..4] [targetNpcId].";
        public override string ExampleUsage =>
            "/hitman_force_offer 2 ludwig_meyer";

        public override void ExecuteCommand(List<string> args)
        {
            int callerIdx = 0;
            string targetId = "ludwig_meyer";
            if (args != null && args.Count >= 1 && int.TryParse(args[0], out var c)) callerIdx = c;
            if (args != null && args.Count >= 2 && !string.IsNullOrEmpty(args[1])) targetId = args[1];
            Mod.Log.Info($"[Cmd] Force-offer: caller={callerIdx} target={targetId}");
            BountyCallScheduler.ForceOfferNow(callerIdx, targetId);
        }
    }

    /// <summary>Print current bounty counters.</summary>
    internal sealed class HitmanStatusCommand : BaseConsoleCommand
    {
        public override string CommandWord => "hitman_status";
        public override string CommandDescription =>
            "Print current HitmanPhone counters (Phase A–K observability).";
        public override string ExampleUsage => "/hitman_status";

        public override void ExecuteCommand(List<string> args)
        {
            var log = Mod.Log;
            log.Info("=== Hitman-Phone status ===");
            log.Info($"Active bounties counter: {BountyHeatService.ActiveBounties}");
            log.Info($"NPC deaths observed: {BountyService.DeathsObserved}");
            log.Info($"Poloroid spawns: {BountyService.PolaroidsSpawned}");
            log.Info($"Receipts seen: {BountyReceiptService.ReceiptsSeen}");
            log.Info($"Receipts matched: {BountyReceiptService.ReceiptsMatched}");
            log.Info($"Receipts mismatched: {BountyReceiptService.ReceiptsMismatched}");
            log.Info($"Payouts issued: {BountyReceiptService.PayoutsIssued} (${BountyReceiptService.PayoutAmountIssuedTotal})");
            log.Info($"Offers made: {BountyCallScheduler.OffersMade}");
            log.Info($"Offers skipped (cooldown): {BountyCallScheduler.OffersSkippedCooldown}");
            log.Info($"Heat: active={BountyHeatService.ActiveBounties} " +
                     $"set_active={BountyHeatService.PursuitSetActive} " +
                     $"set_lethal={BountyHeatService.PursuitSetLethal} " +
                     $"set_fail={BountyHeatService.PursuitSetFail} " +
                     $"cleared={BountyHeatService.PursuitCleared}");
            log.Info($"Expirations dispatched (lifetime): {BountyExpiryService.ExpirationsDispatched}");
            log.Info($"Last expired contracts: {BountyExpiryService.LastExpiredContracts}");
            log.Info($"DeathWatchdog: detected={PlayerDeathWatchdog.DeathsDetected} " +
                     $"failures={PlayerDeathWatchdog.FailuresDispatched}");
            log.Info($"SaveStateGuard: slot_switches={SaveStateGuard.SlotSwitches} same_slot_reloads={SaveStateGuard.SameSlotReloads}");
            log.Info("=== End ===");
        }
    }

    /// <summary>Spawn a polaroid pinned to an NPC.</summary>
    internal sealed class HitmanKillSimCommand : BaseConsoleCommand
    {
        public override string CommandWord => "hitman_kill";
        public override string CommandDescription =>
            "Simulate an NPC kill — spawns a polaroid for the given NPC ID.";
        public override string ExampleUsage => "/hitman_kill melissa_wood";

        public override void ExecuteCommand(List<string> args)
        {
            if (args == null || args.Count == 0 || string.IsNullOrEmpty(args[0]))
            {
                Mod.Log.Warn("Usage: /hitman_kill <npcId>");
                return;
            }
            string npcId = args[0];
            var target = HitmanPhone.Persistence.TargetResolveHelper.FindById(npcId);
            if (target == null)
            {
                Mod.Log.Warn($"NPC '{npcId}' not found.");
                return;
            }

            var save = Mod.Instance?.Save;
            BountyContract? contract = null;
            if (save != null)
            {
                for (int i = 0; i < save.Active.Count; i++)
                {
                    var candidate = save.Active[i];
                    if (candidate.Status == EBountyStatus.Active &&
                        string.Equals(candidate.TargetNpcId, npcId, System.StringComparison.OrdinalIgnoreCase))
                    {
                        contract = candidate;
                        break;
                    }
                }
            }
            if (contract == null)
            {
                Mod.Log.Warn($"No active bounty targets '{npcId}'; cannot make redeemable test evidence.");
                return;
            }

            var polaroid = BountyEvidenceItemRegistry.Spawn(contract.EvidenceToken);
            if (polaroid == null)
            {
                Mod.Log.Warn("Polaroid not registered; cannot simulate kill.");
                return;
            }
            var local = S1API.Entities.Player.Local;
            if (local == null) { Mod.Log.Warn("Player.Local null."); return; }
            var inv = ((S1API.Entities.Interfaces.IEntity)local).gameObject
                          .GetComponent<S1PlayerInventory>();
            if (inv == null) { Mod.Log.Warn("PlayerInventory missing."); return; }
            if (!inv.CanItemFitInInventory(polaroid, 1)) { Mod.Log.Warn("Inventory full."); return; }
            inv.AddItemToInventory(polaroid);
            contract.TargetNpcInstanceId = target.GetInstanceID();
            contract.EvidenceSpawned = true;
            contract.AwaitingDrop = true;
            BountyPersistence.PersistCurrent();
            Mod.Log.Info($"[Cmd] Polaroid spawned for NPC '{npcId}' (token={contract.EvidenceToken}).");
        }
    }

    /// <summary>Wipe saved BountySaveData for the current slot.</summary>
    internal sealed class HitmanResetCommand : BaseConsoleCommand
    {
        public override string CommandWord => "hitman_reset";
        public override string CommandDescription =>
            "Wipe BountySaveData for the current slot (cannot be undone).";
        public override string ExampleUsage => "/hitman_reset";

        public override void ExecuteCommand(List<string> args)
        {
            if (Mod.Instance == null || !SaveStateGuard.IsReady) return;
            var reset = BountySaveValidation.CreateReset(Mod.Instance.Save);
            if (!BountyPersistence.Save(reset))
            {
                Mod.Log.Warn("HitmanPhone: reset refused; bound save context changed.");
                return;
            }
            Mod.Instance.Save = reset;
            Mod.Log.Warn("HitmanPhone: reset in memory; use a normal world save to commit this checkpoint.");
        }
    }

    /// <summary>
    /// v0.1.2 — Cancel orphaned generic-titled "Hitman Contract" journal quests.
    /// A restored quest loses its transient contract id and renders the generic
    /// title; when its contract no longer exists (e.g. after /hitman_reset) the
    /// quest would linger in the journal forever. Guard: with active contracts a
    /// generic-titled quest may belong to one of them, so 'confirm' is required.
    /// </summary>
    internal sealed class HitmanCleanupCommand : BaseConsoleCommand
    {
        public override string CommandWord => "hitman_cleanup";
        public override string CommandDescription =>
            "Cancel orphaned generic-titled 'Hitman Contract' journal quests. If active contracts exist, add 'confirm'.";
        public override string ExampleUsage => "/hitman_cleanup confirm";

        public override void ExecuteCommand(List<string> args)
        {
            var save = Mod.Instance?.Save;
            int active = save?.Active.Count ?? 0;
            bool confirmed = args != null && args.Count >= 1 &&
                             string.Equals(args[0], "confirm", System.StringComparison.OrdinalIgnoreCase);
            if (active > 0 && !confirmed)
            {
                Mod.Log.Warn($"[Cleanup] {active} active contract(s) exist — a generic-titled quest may belong to one of them. " +
                             "Re-run with '/hitman_cleanup confirm' to cancel anyway.");
                return;
            }

            var q = QuestManager.GetQuestByName("Hitman Contract");
            if (q == null)
            {
                Mod.Log.Info("[Cleanup] No generic-titled 'Hitman Contract' quest found — nothing to clean.");
                return;
            }
            try
            {
                q.Cancel();
                Mod.Log.Info("[Cleanup] Cancelled generic-titled 'Hitman Contract' quest. Re-run if more orphans exist.");
            }
            catch (System.Exception ex)
            {
                Mod.Log.Warn($"[Cleanup] Cancel failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Wraps another command and exposes it under a slash-prefixed command word.
    /// Needed because the routing uses args[0] verbatim as the registry key: whether
    /// the native console strips the leading '/' before tokenizing is version
    /// dependent, so both spellings are registered and both resolve to one handler.
    /// </summary>
    internal sealed class SlashAliasCommand : BaseConsoleCommand
    {
        private readonly BaseConsoleCommand _inner;

        public SlashAliasCommand(BaseConsoleCommand inner) => _inner = inner;

        public override string CommandWord => "/" + _inner.CommandWord;
        public override string CommandDescription => _inner.CommandDescription;
        public override string ExampleUsage => _inner.ExampleUsage;

        public override void ExecuteCommand(List<string> args) => _inner.ExecuteCommand(args);
    }

    /// <summary>
    /// Aggregator that registers all five commands during Mod.OnInitializeMelon.
    /// </summary>
    public static class BountyTestCommands
    {
        public static void Register()
        {
            // CustomConsoleRegistry exposes Register(BaseConsoleCommand) — we look it
            // up via reflection because the class is `internal static`. We search the
            // already-loaded assembly list first to avoid the cross-ALC loader edge
            // case (the SimpleName validation fails when we pass a name that doesn't
            // match what ALC saw at load time).
            var registryType = TryResolveType("S1API.Console.CustomConsoleRegistry");
            if (registryType == null)
            {
                Mod.Log.Warn("Could not resolve S1API.Console.CustomConsoleRegistry.");
                return;
            }

            var registerMi = registryType.GetMethod("Register",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            if (registerMi == null)
            {
                Mod.Log.Warn("CustomConsoleRegistry.Register method not found.");
                return;
            }

            var commands = new BaseConsoleCommand[]
            {
                new HitmanForceOfferCommand(),
                new HitmanStatusCommand(),
                new HitmanKillSimCommand(),
                new HitmanResetCommand(),
                new HitmanCleanupCommand(),
                new HitmanDumpBudgetsCommand(),
                new HitmanDumpCallPlanCommand(),
            };

            foreach (var cmd in commands)
            {
                try { registerMi.Invoke(null, new object[] { cmd }); } catch (System.Exception ex) { Mod.Log.Warn(ex.Message); }
                // Same handler, slash-prefixed word — covers consoles that keep the '/'.
                try { registerMi.Invoke(null, new object[] { new SlashAliasCommand(cmd) }); } catch (System.Exception ex) { Mod.Log.Warn(ex.Message); }
            }

            Mod.Log.Info("HitmanPhone commands registered: hitman_force_offer, hitman_status, hitman_kill, hitman_reset, hitman_cleanup, hitman_dump_budgets, hitman_dump_callplan (plus '/' aliases).");
        }

        /// <summary>
        /// Resolve a Type via the assembly ALC list, avoiding the cross-ALC loader
        /// edge case where <c>Type.GetType("Foo, BarAssembly")</c> fails when the
        /// resolved assembly's SimpleName doesn't exactly match the requested one.
        /// </summary>
        private static System.Type? TryResolveType(string fullName)
        {
            // First try the cheap path: Type.GetType on already-loaded context.
            var t = System.Type.GetType(fullName);
            if (t != null) return t;
            // Fall back: search all currently-loaded assemblies.
            foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                var found = asm.GetType(fullName);
                if (found != null) return found;
            }
            return null;
        }
        /// <summary>
        /// Print budget-derived bounty terms for active contracts (and optionally one
        /// target id): weekly customer spend, tier, reward math, drop window.
        /// </summary>
        internal sealed class HitmanDumpBudgetsCommand : BaseConsoleCommand
        {
            public override string CommandWord => "hitman_dump_budgets";
            public override string CommandDescription =>
                "Print weekly budget -> tier/reward/window for active bounty targets. Args: [targetNpcId].";
            public override string ExampleUsage => "/hitman_dump_budgets ludwig_meyer";

            public override void ExecuteCommand(List<string> args)
            {
                try
                {
                    var log = Mod.Log;
                    var save = Mod.Instance?.Save;
                    if (save == null) { log.Warn("No save loaded."); return; }
                    log.Info("=== Bounty budgets (weekly customer spend -> tier/reward) ===");
                    if (args != null && args.Count >= 1 && !string.IsNullOrEmpty(args[0]))
                    {
                        var npc = HitmanPhone.Persistence.TargetResolveHelper.FindById(args[0]);
                        if (npc == null) { log.Warn($"Target '{args[0]}' not found."); return; }
                        float weekly = TargetSelector.TryGetWeeklySpend(npc);
                        string tier = BountyBudget.TierFor(weekly);
                        log.Info($"{args[0]}: budget=${weekly:N0}/wk tier={tier} " +
                                 $"window={BountyBudget.WindowMinutesFor(tier)}min " +
                                 $"(reward preview ${BountyBudget.RewardForBudget(weekly, new System.Random()):N0})");
                        return;
                    }
                    if (save.Active.Count == 0) { log.Info("(no active contracts)"); return; }
                    for (int i = 0; i < save.Active.Count; i++)
                    {
                        var c = save.Active[i];
                        log.Info($"[{c.Id}] {c.TargetNpcName}: budget=${c.TargetWeeklySpend:N0}/wk tier={c.RewardTier} " +
                                 $"reward=${c.RewardCash:N0} drop='{c.AssignedDropName}' window={c.DropWindowMinutes}min " +
                                 $"dropDue={(c.DropDueStarted ? $"{HitmanPhoneTime.MinutesUntil(c.DropDueMinSum)}min left" : "not started")}");
                    }
                }
                catch (System.Exception ex) { Mod.Log.Warn($"hitman_dump_budgets failed: {ex.Message}"); }
            }

        }

        /// <summary>Print the persisted call-plan slots and their state (DEBUG).</summary>
        internal sealed class HitmanDumpCallPlanCommand : BaseConsoleCommand
        {
            public override string CommandWord => "hitman_dump_callplan";
            public override string CommandDescription =>
                "Print the persisted caller schedule plan (in-game day/minute slots).";
            public override string ExampleUsage => "/hitman_dump_callplan";

            public override void ExecuteCommand(List<string> args)
            {
                try
                {
                    var log = Mod.Log;
                    var save = Mod.Instance?.Save;
                    if (save == null) { log.Warn("No save loaded."); return; }
                    var plan = save.CallSchedule;
                    if (plan == null) { log.Info("No call plan yet (created on first tick)."); return; }
                    log.Info($"=== Call plan: {plan.GeneratedThroughDay} days generated through ===");
                    for (int i = 0; i < plan.Slots.Count; i++)
                    {
                        var s = plan.Slots[i];
                        log.Info($"slot[{i}] day={s.Day} min={s.MinuteOfDay:D4} caller={s.CallerIndex} dispatched={s.Dispatched} cancelled={s.Cancelled}");
                    }
                }
                catch (System.Exception ex) { Mod.Log.Warn($"hitman_dump_callplan failed: {ex.Message}"); }
            }
        }
    }
#endif
    // ==== end HitmanPhone section: Runtime/BountyTestCommands.cs ====
}

namespace HitmanPhone
{
    // ==== HitmanPhone section: Runtime/HitmanPhoneTime.cs ====
    using System;
    using System.Reflection;
    using MelonLoader;
    using UnityEngine;

#if (IL2CPPMELON)
    using S1Time = Il2CppScheduleOne.GameTime.TimeManager;
#elif MONOMELON
    using S1Time = ScheduleOne.GameTime.TimeManager;
#endif


    /// <summary>
    /// Wrappers around Schedule I's <see cref="S1Time"/> for hitman-phase time
    /// queries. Keeps reflection in one place so callers can ask for the current
    /// in-game day without learning the stub layout.
    /// </summary>
    public static class HitmanPhoneTime
    {
        /// <summary>One main-thread time sample; no cached day is mixed with a fresh clock.</summary>
        public static bool TryGetSnapshot(out int day, out int minute, out long sum)
        {
            day = minute = -1;
            sum = -1;
            try
            {
                var tm = S1Time.Instance;
                if (tm == null || !S1Mods.Shared.NetworkGuard.IsInMainScene) return false;
                return BountyRuntimeRules.TryTimeSnapshot(() => tm.ElapsedDays,
                    () => S1Time.GetMinSumFrom24HourTime(tm.CurrentTime), out day, out minute, out sum);
            }
            catch { return false; }
        }

        public static void Reset() { } // no session-spanning time cache remains
        public static int CurrentDay() => TryGetSnapshot(out int day, out _, out _) ? day : -1;
        public static int CurrentMinuteOfDay() => TryGetSnapshot(out _, out int minute, out _) ? minute : -1;
        public static long MinuteSum(int day, int minuteOfDay) => day * 1440L + minuteOfDay;
        public static long CurrentMinuteSum() => TryGetSnapshot(out _, out _, out long sum) ? sum : -1;

        /// <summary>Minutes remaining until the given absolute minute sum (negative when past).</summary>
        public static long MinutesUntil(long dueMinSum) => dueMinSum - CurrentMinuteSum();

        /// <summary>HHMM int (e.g. 1405) for an absolute minute sum — for display/debug.</summary>
        public static int To24HourTime(long minSum)
        {
            int minutesInDay = (int)(minSum % 1440);
            if (minutesInDay < 0) minutesInDay += 1440;
            return S1Time.Get24HourTimeFromMinSum(minutesInDay);
        }
    }
    // ==== end HitmanPhone section: Runtime/HitmanPhoneTime.cs ====
}

namespace HitmanPhone
{
    // ==== HitmanPhone section: Runtime/SaveStateGuard.cs ====
    using System;
    using System.Threading;
    using HitmanPhone.Bounty;
    using HitmanPhone.Persistence;
    using MelonLoader;
    using S1API.Lifecycle;
    using S1Mods.Shared;

#if (IL2CPPMELON)
    using S1Persistence = Il2CppScheduleOne.Persistence;
#elif MONOMELON
    using S1Persistence = ScheduleOne.Persistence;
#endif


    /// <summary>Only a completed load installs a world-bound bounty context.</summary>
    public static class SaveStateGuard
    {
        private static bool _subscribed;
        private static bool _loaded;
        private static bool _pendingLoad;
        private static long _loadDeadline;
        private static bool _saveFailed;
        private static int _slot = -1;
        public static int SlotSwitches { get; private set; }
        public static int SameSlotReloads { get; private set; }
        public static string StatusMessage { get; private set; } = "Waiting for a loaded singleplayer world.";

        public static bool IsSingleplayer
        {
            get
            {
                try
                {
                    if (!NetworkGuard.IsHostOrSingleplayer()) return false;
#if IL2CPPMELON
                    var lobby = Il2CppScheduleOne.DevUtilities.Singleton<Il2CppScheduleOne.Networking.Lobby>.Instance;
                    if (lobby != null && lobby.IsInLobby) return false;
                    if (Il2CppScheduleOne.PlayerScripts.Player.PlayerList.Count > 1) return false;
#endif
                    return true;
                }
                catch { return false; }
            }
        }

        public static bool IsReady
        {
            get
            {
                if (!IsSingleplayer)
                {
                    StatusMessage = "HitmanPhone is singleplayer only. Multiplayer contracts and payouts are disabled.";
                    return false;
                }
                if (!_loaded || _saveFailed || BountyPersistence.IsSaving || !NetworkGuard.IsInMainScene) return false;
                try
                {
                    var lm = S1Persistence.LoadManager.Instance;
                    return lm != null && lm.IsGameLoaded && !lm.IsLoading &&
                        Mod.Instance.Save.SaveIdentity == BountyPersistence.ActiveSaveIdentity;
                }
                catch { return false; }
            }
        }

        public static void TrySubscribeLifecycle()
        {
            if (_subscribed) return;
            _subscribed = true;
            GameLifecycle.OnPreLoad += OnPreLoad;
            GameLifecycle.OnPreSceneChange += Suspend;
            GameLifecycle.OnLoadComplete += OnLoadComplete;
            GameLifecycle.OnSaveStart += OnSaveStart;
            GameLifecycle.OnSaveComplete += OnSaveComplete;
            // OnSaveInfoLoaded is metadata/menu activity, never a context switch.
        }

        public static void TryUnsubscribeLifecycle()
        {
            GameLifecycle.OnPreLoad -= OnPreLoad;
            GameLifecycle.OnPreSceneChange -= Suspend;
            GameLifecycle.OnLoadComplete -= OnLoadComplete;
            GameLifecycle.OnSaveStart -= OnSaveStart;
            GameLifecycle.OnSaveComplete -= OnSaveComplete;
            _subscribed = false;
            Suspend();
        }

        public static void Tick()
        {
            if (!_pendingLoad) return;
            if (Environment.TickCount64 > _loadDeadline)
            {
                _pendingLoad = false;
                StatusMessage = "HitmanPhone paused: loaded world identity did not become available; see log.";
                Mod.Log.Error("[context] timed out waiting for the completed load context.");
                return;
            }
            try
            {
                var lm = S1Persistence.LoadManager.Instance;
                if (lm != null && lm.IsGameLoaded && !lm.IsLoading && SaveSlots.GetActiveSlotNumber() >= 0)
                    TryInstallContext();
            }
            catch { /* bounded readiness retry, no gameplay mutation */ }
        }

        public static void Suspend()
        {
            _pendingLoad = false;
            _loaded = false;
            _saveFailed = false;
            BountyPersistence.ClearContext();
            BountyJournalBridge.ResetSessionCache();
            HitmanPhoneTime.Reset();
            PlayerDeathWatchdog.Reset();
            BountyCallScheduler.ResetDayLatch();
            BountyConversationRouter.InvalidateOpenOffer();
            BountyExpiryService.ResetDayLatch();
            StatusMessage = "Waiting for the world to finish loading.";
        }

        private static void OnPreLoad()
        {
            Suspend();
            HitmanPhone.Items.BountyEvidenceItemRegistry.InvalidateRegistration();
            HitmanPhone.Items.BountyEvidenceItemRegistry.EnsureRegistered();
        }

        private static void OnLoadComplete()
        {
            if (Mod.Instance == null) return;
            Suspend();
            _pendingLoad = true;
            _loadDeadline = Environment.TickCount64 + 15000;
            TryInstallContext();
        }

        private static void TryInstallContext()
        {
            if (!IsSingleplayer)
            {
                _pendingLoad = false;
                StatusMessage = "HitmanPhone is singleplayer only. Multiplayer contracts and payouts are disabled.";
                Mod.Log.Warn("[context] " + StatusMessage);
                return;
            }
            try
            {
                var lm = S1Persistence.LoadManager.Instance;
                int slot = SaveSlots.GetActiveSlotNumber();
                string identity = BountyPersistence.ActiveSaveIdentity;
                if (lm == null || !lm.IsGameLoaded || lm.IsLoading || slot < 0 || string.IsNullOrEmpty(identity))
                {
                    return; // preserve the original deadline across readiness retries
                }
                _pendingLoad = false;
                string world = lm.LoadedGameFolderPath;
                BountyPersistence.BindContext(slot, identity, world);
                Mod.Instance.Save = BountyPersistence.Load(); // ALWAYS reload, including A -> A
                if (_slot == slot) SameSlotReloads++; else SlotSwitches++;
                _slot = slot;
                _loaded = true;
                StatusMessage = "";
                BountyHeatService.ResetActiveCounters();
                BountyService.InvalidatePersistedInstanceIds();
                BountyHeatService.PurgePersistedHeatKeys();
                if (!BountyJournalBridge.RebindActiveQuests()) BountyJournalBridge.ScheduleRebindRetries();
                Mod.Log.Info($"[context] ready slot={slot}, active={Mod.Instance.Save.Active.Count}, history={Mod.Instance.Save.History.Count}; checkpoint mode.");
            }
            catch (Exception ex)
            {
                _pendingLoad = false;
                _loaded = false;
                StatusMessage = "HitmanPhone paused: load/checkpoint mismatch. Original data retained; see log.";
                Mod.Log.Error($"[context] load refused: {ex}");
            }
        }

        private static void OnSaveStart()
        {
            if (!_loaded || !IsSingleplayer) return;
            try { BountyPersistence.PrepareCheckpoint(); }
            catch (Exception ex)
            {
                _saveFailed = true;
                StatusMessage = "HitmanPhone paused: checkpoint preparation failed; see log.";
                Mod.Log.Error($"[checkpoint] preparation failed: {ex}");
            }
        }

        private static void OnSaveComplete()
        {
            if (!_loaded || !IsSingleplayer || !BountyPersistence.IsSaving) return;
            try
            {
                if (!BountyPersistence.CommitCheckpoint(!S1Persistence.SaveManager.SaveError))
                    throw new InvalidOperationException("Native save or bounty checkpoint write failed.");
                _saveFailed = false;
                StatusMessage = "";
            }
            catch (Exception ex)
            {
                _saveFailed = true;
                StatusMessage = "HitmanPhone paused: checkpoint failed. Do not reload; retry a world save and check the log.";
                Mod.Log.Error($"[checkpoint] NOT committed; frozen state retained: {ex}");
            }
        }
    }
    // ==== end HitmanPhone section: Runtime/SaveStateGuard.cs ====
}

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
            if (Mod.Instance == null) return;
            Mod.Log.Warn("HitmanPhone: wiping BountySaveData for current slot.");
            Mod.Instance.Save = new BountySaveData();
            BountyPersistence.Save(Mod.Instance.Save);
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
        // Audit L3 (2026-09-01): CurrentDay is called from every scheduler tick —
        // resolve the member infos ONCE instead of per call (per-frame reflection
        // churn). The IL2CPP stub type never changes within a session.
        private static PropertyInfo? _elapsedDaysProp;
        private static FieldInfo? _elapsedDaysField;
        private static bool _membersResolved;

        // Audit (2026-09-10): CurrentDay is called from every scheduler/expiry tick
        // (i.e. every OnUpdate frame) — cache the resolved day and refresh at most
        // once per second so the hot loop avoids interop reflection + boxing per
        // frame (same _nextTick throttle shape as BountyTargetWatchdog). No
        // OnDayPass-style event exists in this mod to hook instead; day-granular
        // bounty deadlines tolerate a 1s-stale day. First call resolves immediately.
        private static int _cachedDay;
        private static float _nextRefreshRealtime;
        private static bool _hasCachedDay;

        private static void ResolveMembers()
        {
            if (_membersResolved) return;
            _membersResolved = true;
            try
            {
                var type = typeof(S1Time);
                _elapsedDaysProp = type.GetProperty("ElapsedDays",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                _elapsedDaysField = type.GetField("_ElapsedDays_k__BackingField",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"HitmanPhoneTime member resolution failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Elapsed in-game days (whole days). Falls back to 0 when TimeManager is
        /// not yet initialised (e.g. before the player enters the gameplay scene).
        /// Throttled: the interop read runs at most once per second, otherwise the
        /// cached value is returned.
        /// </summary>
        public static int CurrentDay()
        {
            try
            {
                float now = Time.realtimeSinceStartup;
                if (_hasCachedDay && now < _nextRefreshRealtime) return _cachedDay;
                int day = ResolveDayUncached();
                _cachedDay = day;
                _hasCachedDay = true;
                _nextRefreshRealtime = now + 1.0f;
                return day;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"HitmanPhoneTime.CurrentDay failed: {ex.Message}");
                return _hasCachedDay ? _cachedDay : 0;
            }
        }

        /// <summary>
        /// Uncached interop read of ElapsedDays (property, else backing field).
        /// </summary>
        private static int ResolveDayUncached()
        {
            try
            {
                var tm = S1Time.Instance;
                if (tm == null) return 0;
                ResolveMembers();
                // ElapsedDays is exposed via property in newer patches; the backing
                // field is the fallback.
                if (_elapsedDaysProp != null && _elapsedDaysProp.CanRead)
                {
                    var v = _elapsedDaysProp.GetValue(tm);
                    if (v is int d) return d;
                }
                if (_elapsedDaysField != null)
                {
                    var v = _elapsedDaysField.GetValue(tm);
                    if (v is int d2) return d2;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"HitmanPhoneTime.ResolveDayUncached failed: {ex.Message}");
                return 0;
            }
        }

        // ------------------------------------------------------------------
        // Minute-precision helpers (drop windows + call plan). TimeManager's
        // CurrentTime is the game's 24-hour int (1330 = 13:30); the static
        // GetMinSumFrom24HourTime converts it to minutes-of-day (proven in the
        // 0.4.7 stub decompile).
        // ------------------------------------------------------------------

        /// <summary>Current minute-of-day (0..1399). 0 on failure.</summary>
        public static int CurrentMinuteOfDay()
        {
            try
            {
                var tm = S1Time.Instance;
                if (tm == null) return 0;
                return S1Time.GetMinSumFrom24HourTime(tm.CurrentTime);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"HitmanPhoneTime.CurrentMinuteOfDay failed: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Absolute in-game minute sum (day*1440 + minute-of-day) — the game's own
        /// GameDateTime.GetMinSum() convention. Base for drop-window due points.
        /// </summary>
        public static long MinuteSum(int day, int minuteOfDay) => day * 1440L + minuteOfDay;

        public static long CurrentMinuteSum() => MinuteSum(CurrentDay(), CurrentMinuteOfDay());

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


    /// <summary>
    /// Phase J — Slot-Load Detection (Skill Rule #16).
    ///
    /// <c>GameLifecycle.OnPreLoad</c> fires for **both** real save-slot switches
    /// AND same-slot Menu→Game scene reloads. A naive
    /// <c>Save = BountyPersistence.Load()</c> in <c>OnPreLoad</c> would wipe placed
    /// items on every menu-return. We cache the last-known slot number and only
    /// trigger a full reload when the slot actually changed.
    ///
    /// Sentinel value <c>-2</c> means "never initialised" — the very first load
    /// fires a full reload so that the player sees their previous bounty state on
    /// save-game start.
    /// </summary>
    public static class SaveStateGuard
    {
        private const string ActiveSlotKey = "hitman_active_slot";

        /// <summary>Cached last-known save slot. <c>-2</c> = "uninitialised".</summary>
        private static int _lastKnownSlot = -2;

        private static bool _subscribed;

        /// <summary>Count of times a real slot switch was detected.</summary>
        private static int _slotSwitches;
        /// <summary>Count of times a same-slot menu reload was detected.</summary>
        private static int _sameSlotReloads;

        public static int SlotSwitches => Volatile.Read(ref _slotSwitches);
        public static int SameSlotReloads => Volatile.Read(ref _sameSlotReloads);

        /// <summary>
        /// Subscribe to the GameLifecycle hooks. Idempotent.
        /// </summary>
        public static void TrySubscribeLifecycle()
        {
            if (_subscribed) return;
            _subscribed = true;
            GameLifecycle.OnPreLoad += OnPreLoad;
            GameLifecycle.OnLoadComplete += OnLoadComplete;
            GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
            LogSubscriptionsOnce();
        }

        /// <summary>
        /// Unsubscribe — called from Mod.OnDeinitializeMelon.
        /// </summary>
        public static void TryUnsubscribeLifecycle()
        {
            GameLifecycle.OnPreLoad -= OnPreLoad;
            GameLifecycle.OnLoadComplete -= OnLoadComplete;
            GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
            _subscribed = false;
        }

        private static bool _subscribedLogged;
        private static void LogSubscriptionsOnce()
        {
            if (_subscribedLogged) return;
            _subscribedLogged = true;
            Mod.Log.Debug("SaveStateGuard subscribed to GameLifecycle hooks.");
        }

        /// <summary>
        /// Called by <see cref="GameLifecycle"/> just before load. Resolve the new slot
        /// before it actually loads so we can compare with our cached value.
        /// </summary>
        private static void OnPreLoad()
        {
            int newSlot = ResolveActiveSlotSafe();
            int oldSlot = _lastKnownSlot;

            if (oldSlot == -2)
            {
                // First-time init: do a full reload so the player sees their previous
                // bounty state on save-game start.
                TriggerFullReload(newSlot, reason: "init");
                _lastKnownSlot = newSlot;
                return;
            }

            if (newSlot == -1)
            {
                // Audit L8 (2026-09-01): never cache -1 and never reload on it — a
                // permanently-unready LoadManager would otherwise wedge the guard in
                // "same slot" forever after one init reload. OnSaveInfoLoaded
                // reconciles the cache once the real slot is known.
                Mod.Log.Debug("[SaveStateGuard] LoadManager not ready — preload skipped, waiting for OnSaveInfoLoaded.");
                return;
            }

            if (newSlot != oldSlot)
            {
                // Real slot switch: cache old, do a full reload.
                Mod.Log.Info($"[SaveStateGuard] Slot switch detected: {oldSlot} → {newSlot}. " +
                             $"Full save reload.");
                TriggerFullReload(newSlot, reason: "slot_switch");
                Interlocked.Increment(ref _slotSwitches);
                _lastKnownSlot = newSlot;
            }
            else
            {
                // Same slot, Menu → Game scene reload: don't touch Save state. We just
                // need to reset the death-watchdog so a corpse-load doesn't fire a
                // spurious rising-edge (Phase H).
                Mod.Log.Debug($"[SaveStateGuard] Same-slot reload ({newSlot}) — keeping bounty state.");
                Interlocked.Increment(ref _sameSlotReloads);
                PlayerDeathWatchdog.Reset();
            }
        }

        /// <summary>
        /// Called once the load completed. Used to refresh Save to the new slot's contents.
        /// </summary>
        private static void OnLoadComplete()
        {
            // BUGFIX (2026-08-31): every completed load gets fresh runtime state.
            // The journal bridge's session dictionary must go (restored quests are
            // new objects) and the persisted EvidenceSpawned latch is released so
            // contracts whose polaroid was lost become completable again.
            Bounty.BountyJournalBridge.ResetSessionCache();
            Bounty.BountyService.ResetEvidenceFlags();
            // Old saves used session-only Unity IDs in contracts and photos. Migration
            // preserves those IDs only for active legacy evidence, assigns durable
            // tokens, and clears the transient target IDs to prevent cross-session collisions.
            Bounty.BountyService.InvalidatePersistedInstanceIds();
            // Audit M1 (2026-09-01): drop realtimeSinceStartup grace keys old saves
            // still carry — the grace window is in-memory only now.
            Bounty.BountyHeatService.PurgePersistedHeatKeys();

            // Restored journal quests are new objects and BountyQuest._contractId is
            // runtime-only. Rebind them after the save and journal systems finished
            // loading so the target name/objective is visible again after restart.
            if (!Bounty.BountyJournalBridge.RebindActiveQuests())
                Bounty.BountyJournalBridge.ScheduleRebindRetries();
        }

        /// <summary>
        /// Save-data updated: re-read the file. The engine fires this on real save-info
        /// changes (e.g. user slots into a save) which is the cleanest place to react.
        /// </summary>
        private static void OnSaveInfoLoaded()
        {
            if (Mod.Instance == null) return;
            Mod.Instance.Save = BountyPersistence.Load();
            int slot = ResolveActiveSlotSafe();
            _lastKnownSlot = slot == -1 ? _lastKnownSlot : slot;
        }

        /// <summary>
        /// Force a full bounty-state reload from disk. Called when a real slot switch is
        /// detected so that active contracts from the previous slot don't bleed through.
        /// </summary>
        private static void TriggerFullReload(int newSlot, string reason)
        {
            if (Mod.Instance == null) return;
            var prev = Mod.Instance.Save;
            Mod.Log.Info($"[SaveStateGuard] Full reload ({reason}). " +
                         $"Active contract count before: {prev?.Active.Count ?? -1}.");
            Mod.Instance.Save = BountyPersistence.Load();
            int after = Mod.Instance.Save.Active.Count;
            Mod.Log.Info($"[SaveStateGuard] Reload done. Active contract count after: {after}.");

            // Reset subsystems so we don't carry over runtime state from the previous slot.
            PlayerDeathWatchdog.Reset();
            BountyHeatService.ResetActiveCounters();
            // Audit M5 (2026-09-01): day latches must not survive a slot switch —
            // otherwise the new save gets no offer and no expiry check on day one.
            BountyCallScheduler.ResetDayLatch();
            BountyExpiryService.ResetDayLatch();
        }

        /// <summary>
        /// Ask the LoadManager for the active save slot. Returns -1 if it isn't ready yet.
        /// We swallow all exceptions because the load manager is fussy during transitions.
        /// Sonde: S1Mods.Shared.SaveSlots (Konsolidierung 2026-09-15).
        /// </summary>
        private static int ResolveActiveSlotSafe()
        {
            return SaveSlots.GetActiveSlotNumber();
        }
    }
    // ==== end HitmanPhone section: Runtime/SaveStateGuard.cs ====
}

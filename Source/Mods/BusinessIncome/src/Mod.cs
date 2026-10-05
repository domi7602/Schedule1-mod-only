using System;
using System.Globalization;
using System.Reflection;
using BusinessIncome.Config;
using BusinessIncome.Core;
using BusinessIncome.Services;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(BusinessIncome.Mod), "BusinessIncome", "0.1.6", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BusinessIncome;

public class Mod : MelonMod
{
    public static Mod Instance { get; private set; } = null!;
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Instance = this;
        Log = new ModLogger("BusinessIncome");

        // 1. Initialize config via SafeStorage / ModConfig
        // Dictionary/List properties are persisted via ConfigJsonStore (JSON sidecar), not TOML —
        // declaring them here keeps the ModConfig startup log as Info (not Warning).
        ModConfig<BusinessIncomeConfig>.Initialize("BusinessIncome", Log,
            sidecarManagedProperties: new[] { "PropertyMultipliers", "DisplayNameOverrides", "WeekendBonusCategories" });
        // JSON sidecar for Dictionary/List properties (not TOML-mappable, see ModConfig).
        try
        {
            ConfigJsonStore.ApplyToConfig(ModConfig<BusinessIncomeConfig>.Instance);
        }
        catch (Exception ex)
        {
            Log.Warn($"Config sidecar apply failed: {ex.Message}");
        }

        // 2. Register hash terminal shim (reflective, optional dependency)
        TryRegisterHashPlugin();

        // 3. Subscribe to lifecycle events
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded += OnSaveLoaded;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        try
        {
            S1API.GameTime.TimeManager.OnDayPass += OnDayPass;
            S1API.GameTime.TimeManager.OnHourPass += OnHourPass;
        }
        catch (Exception ex) { Log.Warn($"TimeManager hooks failed (S1API missing?): {ex.Message}"); }

        Log.Info($"BusinessIncome v{Info.Version} initialized. Features: write-ahead pending marker, catch-up cap, visible payout warnings.");
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main")
        {
            PayoutStateStore.ResetForSceneUnload();
            Log.Debug("Main Scene unloaded. PayoutStateStore reset — authority and slot cleared (no stale-slot writes).");
        }
    }

    private void OnPreLoad()
    {
        // Readiness AND authority are invalidated here; the active slot is cleared as well.
        // Carrying a slot across the PreLoad -> LoadComplete window could write into the
        // PREVIOUS save (the old keepSlot:true "last-known slot" behaviour). The slot is
        // re-resolved on LoadComplete.
        PayoutStateStore.Reset();
        Log.Debug("OnPreLoad: PayoutStateStore reset — ready=false, slot cleared.");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded -= OnSaveLoaded;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        try
        {
            S1API.GameTime.TimeManager.OnDayPass -= OnDayPass;
            S1API.GameTime.TimeManager.OnHourPass -= OnHourPass;
        }
        catch { }
    }

    private void OnSaveLoaded()
    {
        // Informational only: the slot is (re-)resolved and state is loaded on LoadComplete,
        // so no state is read or written during this window.
        try
        {
            Log.Info("Save info loaded. Payout state will be resolved on LoadComplete.");
        }
        catch (Exception ex)
        {
            Log.Error($"Error in save-load handler: {ex.Message}");
        }
    }

    private void OnLoadComplete()
    {
        // Scene build is done and owned-business lists are guaranteed populated here. This is
        // the single point where authority + readiness are resolved and the pending marker is
        // inspected — regardless of whether SaveInfoLoaded fired.
        try
        {
            PayoutStateStore.LoadForActiveSlot();
            WarnPendingMarker();
            CheckCatchupPayout();
        }
        catch (Exception ex)
        {
            Log.Warn("OnLoadComplete payout init failed", ex);
        }
    }

    private void OnDayPass()
    {
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;

        // Catch up FIRST and only pay today when catch-up completed safely: a failed or
        // blocked earlier day must never be skipped over by today's payout.
        if (!CheckCatchupPayout())
        {
            Log.Warn("[Payout] OnDayPass: skipping today's payout - catch-up did not complete safely.");
            return;
        }

        if (cfg.PayoutHour == 0)
        {
            int elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
            Log.Debug($"OnDayPass event received (day {elapsedDays}). Executing midnight payout...");
            IncomeEngine.ExecuteDailyPayout(elapsedDays, cfg);
        }
    }

    private void OnHourPass()
    {
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;
        if (cfg.PayoutHour <= 0) return;

        try
        {
            // Catch up FIRST so a failed earlier day is never skipped by the in-window payout.
            if (!CheckCatchupPayout())
            {
                Log.Warn("[Payout] OnHourPass: skipping in-window payout - catch-up did not complete safely.");
                return;
            }

            int elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
            var decision = DecideWindow(cfg.PayoutHour, isPastDay: false);
            if (!decision.InWindow)
            {
                Log.Debug($"[Payout] OnHourPass outside window: {decision.Reason}.");
                return;
            }

            Log.Debug($"[Payout] OnHourPass in window ({decision.Reason}). Executing configured payout for day {elapsedDays}...");
            IncomeEngine.ExecuteDailyPayout(elapsedDays, cfg);
        }
        catch (Exception ex) { Log.Warn("OnHourPass failed", ex); }
    }

    /// <summary>
    /// Shared, fail-closed payout-window decision (centralized pure planner).
    /// </summary>
    private static WindowDecision DecideWindow(int payoutHour, bool isPastDay)
    {
        int now24 = 0;
        bool timeKnown = false;
        try
        {
            now24 = S1API.GameTime.TimeManager.CurrentTime;
            timeKnown = true;
        }
        catch { timeKnown = false; }

        return PayoutWindowPlanner.Plan(payoutHour, now24, timeKnown, isPastDay);
    }

    private void WarnPendingMarker()
    {
        var status = PayoutStateStore.PendingStatus;
        if (status == PendingStatus.Missing) return;

        if (status == PendingStatus.Corrupt)
        {
            Log.Error($"[Payout] pending marker for slot {PayoutStateStore.GetActiveSlotSuffix()} is CORRUPT/unreadable. " +
                      "ALL new automatic and forced payouts are BLOCKED. Back up the state, marker and artifacts, " +
                      "then repair the unreadable marker using verified bank history. Deleting it does NOT accept the payout and can enable a duplicate payout.");
            return;
        }

        var pending = PayoutStateStore.ReadPendingMarker();
        if (pending == null || pending.Day < 0) return;

        string recovered = status == PendingStatus.Backup ? " (recovered from .bak)" : "";
        Log.Warn($"UNCHECKED PAYOUT{recovered}: day {pending.Day} may already be booked (+${pending.Amount.ToString("N2", CultureInfo.InvariantCulture)}, {pending.BusinessCount} businesses, started {pending.StartedUtc}) " +
                 "but its payout state was never saved. Check the in-game bank app for a 'Business Revenue' entry of that day, then run " +
                 "'biz pending confirm' (money received — skip day) or 'biz pending resolve' (money missing — pay again). New payouts are BLOCKED until then.");
    }

    /// <summary>
    /// Runs the catch-up pass and returns true ONLY when it is safe to proceed with today's
    /// in-window payout. Any host/authority/state/marker failure, an unpersisted seed or cap
    /// commit, or a day whose outcome stopped the loop returns false so the caller never books
    /// a later day on top of an unsafe earlier one.
    /// </summary>
    private bool CheckCatchupPayout()
    {
        try
        {
            // Host authority FIRST, before ANY mutation (seed, cap-skip, terminal commit) —
            // clients must never write state even though the engine also gates the bank.
            if (!IncomeEngine.IsHostOrSingleplayer())
            {
                Log.Debug("[Payout] catch-up skipped: not host/singleplayer (fail closed).");
                return false;
            }
            if (!PayoutStateStore.IsAuthoritative)
            {
                Log.Warn("[Payout] catch-up skipped: active slot not resolved (fail closed).");
                return false;
            }
            if (PayoutStateStore.StateLoad.BlocksMutation)
            {
                Log.Error($"[Payout] catch-up skipped: payout state is {PayoutStateStore.StateLoad.Status} for slot {PayoutStateStore.GetActiveSlotSuffix()} ({PayoutStateStore.StateLoad.Detail}). " +
                          "Back up all state/marker artifacts and reconcile with verified bank history before manual repair. Do not delete the ledger to bypass this lock.");
                return false;
            }
            if (PayoutStateStore.PendingStatus != PendingStatus.Missing)
            {
                Log.Warn("[Payout] catch-up skipped: an unresolved pending payout marker exists. Run 'biz pending confirm' or 'biz pending resolve'.");
                return false;
            }

            int elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
            var cfg = ModConfig<BusinessIncomeConfig>.Instance;
            var state = PayoutStateStore.GetState();

            // Fresh state (never seeded): record the seed as INITIALIZATION via the guarded,
            // durable-only Initialize op and stop for this pass. Only genuinely-MISSING state is
            // seeded; a failed write advances nothing. Today's payout is deferred (no windfall).
            if (PayoutStateStore.StateLoad.Status == StateStatus.Missing)
            {
                var seed = PayoutStateStore.Initialize(elapsedDays);
                if (seed.Succeeded)
                    Log.Info($"[Payout] Fresh payout state: day {elapsedDays} seeded as INITIALIZATION (not a paid day); today's payout deferred.");
                else
                    Log.Warn($"[Payout] fresh-state initialization NOT persisted ({seed.Outcome}: {seed.Detail}); catch-up deferred and will retry.");
                return false;
            }

            if (!BusinessResolver.TryGetOwnedBusinesses(cfg.DisplayNameOverrides, out var ownedNow))
            {
                Log.Warn("[Payout] catch-up aborted: owned-business read is not trustworthy (unreadable/partial) — refusing to treat it as 'no businesses'.");
                return false;
            }
            bool hasBusinesses = ownedNow.Count > 0;
            var plan = PayoutScheduler.PlanCatchup(state.LastPaidElapsedDay, elapsedDays, cfg.MaxCatchupDays, hasBusinesses, stateDurable: true);

            if (plan.TerminalSkip || plan.Days.Count == 0)
            {
                Log.Debug($"[Payout] catch-up: {plan.Reason}.");
                return true;
            }

            int planFirst = plan.Days[0];
            int planLast = plan.Days[plan.Days.Count - 1];
            if (plan.StoppedEarly) Log.Warn($"[Payout] {plan.Reason}: days {planFirst}..{planLast}.");
            else Log.Debug($"[Payout] {plan.Reason}: days {planFirst}..{planLast}.");

            // A capped backlog durably advances the state to just before the first planned day
            // so the storm does not repeat on every day-pass. The commit is CHECKED — if it is
            // not durable the whole catch-up aborts (we do not book on top of an unpersisted day).
            if (plan.StoppedEarly)
            {
                var skip = PayoutStateStore.CommitOnly(planFirst - 1, Array.Empty<string>());
                if (!skip.Succeeded)
                {
                    Log.Warn($"[Payout] catch-up aborted: cap-skip commit to day {planFirst - 1} not persisted ({skip.Outcome}: {skip.Detail}).");
                    return false;
                }
            }

            // Oldest -> newest. Stop at the first day whose outcome is unresolved or whose state
            // could not be persisted, so we never book later days on top of an unsafe earlier day.
            foreach (int d in plan.Days)
            {
                if (PayoutStateStore.IsDayPaid(d)) continue;

                bool isPastDay = d < elapsedDays;
                if (!isPastDay)
                {
                    var decision = DecideWindow(cfg.PayoutHour, isPastDay: false);
                    if (!decision.InWindow)
                    {
                        Log.Debug($"[Payout] catch-up: day {d} not in window ({decision.Reason}).");
                        continue;
                    }
                }

                Log.Info($"[Payout] catch-up for day {d} ({(isPastDay ? "backlog" : "current window")})...");
                var r = IncomeEngine.ExecuteDailyPayout(d, cfg);
                if (IncomeEngine.StopsCatchup(r.Outcome))
                {
                    Log.Warn($"[Payout] catch-up stopped at day {d}: {r.Outcome} ({r.Detail}).");
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            // F3: Warn (not Debug — Debug is compiled out in release builds) so payout
            // failures are visible in the MelonLoader log instead of vanishing silently.
            Log.Warn("CheckCatchupPayout failed", ex);
            return false;
        }
    }

    private static void TryRegisterHashPlugin()
    {
        try
        {
            var hashType = Type.GetType("Hash.Api.HashCommands, Hash");
            if (hashType == null) return;

            var availableProp = hashType.GetProperty("Available", BindingFlags.Public | BindingFlags.Static);
            if (availableProp == null || !(bool)(availableProp.GetValue(null) ?? false)) return;

            var addMethod = hashType.GetMethod("Add", BindingFlags.Public | BindingFlags.Static);
            if (addMethod == null) return;

            addMethod.Invoke(null, new object[]
            {
                "biz",
                "business income: stats, trigger, config, set, help",
                "biz stats"
            });
            Log.Info("Hash bridge registered: 'biz' command listed in terminal.");
        }
        catch (Exception ex)
        {
            Log.Debug($"Hash bridge registration skipped: {ex.Message}");
        }
    }
}

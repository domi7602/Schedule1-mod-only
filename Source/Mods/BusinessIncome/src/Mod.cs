using System;
using System.Globalization;
using System.Reflection;
using BusinessIncome.Config;
using BusinessIncome.Services;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(BusinessIncome.Mod), "BusinessIncome", "0.1.5", "Dominik")]
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
        // JSON-Sidecar für Dictionary/List-Properties (nicht TOML-mappable, siehe ModConfig).
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

        Log.Info("BusinessIncome v0.1.3 initialized. Features: write-ahead pending marker, catch-up cap, visible payout warnings.");
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main")
        {
            PayoutStateStore.ResetForSceneUnload();
            Log.Debug("Main Scene unloaded. PayoutStateStore reset (keepSlot).");
        }
    }

    private void OnPreLoad()
    {
        // Audit 2026-09-13 (BIZ-05): keepSlot:true — OnPreLoad fires BEFORE the new save info is
        // parsed. With keepSlot:false the store would fall back to the 'default' slot, and every
        // state access in the PreLoad->SaveInfoLoaded window would read/write *_default.json.
        // BankApp hardened this the same way (keepSlot:true); the slot is re-resolved on load.
        PayoutStateStore.Reset(keepSlot: true);
        Log.Debug("OnPreLoad: PayoutStateStore reset (keepSlot).");
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
        try
        {
            string slot = PayoutStateStore.GetActiveSlotSuffix();
            var state = PayoutStateStore.GetState();
            Log.Info($"Save loaded (Slot: {slot}). Last payout on day {state.LastPaidElapsedDay}.");

            // F1: leftover write-ahead marker = a payout transaction may be booked but its
            // state commit was never confirmed (crash between booking and save). Warn loudly
            // instead of silently paying the day again on the next catch-up.
            var pending = PayoutStateStore.ReadPendingMarker();
            if (pending != null && pending.Day >= 0)
            {
                Log.Warn($"UNCHECKED PAYOUT: day {pending.Day} may already be booked (+${pending.Amount.ToString("N2", CultureInfo.InvariantCulture)}, {pending.BusinessCount} businesses, started {pending.StartedUtc}) but its payout state was never saved. " +
                         "Check the in-game bank app for a 'Business Revenue' entry of that day, then run 'biz pending confirm' (money received — skip day) or 'biz pending resolve' (money missing — pay again).");
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error in save-load handler: {ex.Message}");
        }
    }

    private void OnLoadComplete()
    {
        // Scene build is done, owned business lists are guaranteed populated here.
        CheckCatchupPayout();
    }

    private void OnDayPass()
    {
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;
        if (cfg.PayoutHour == 0)
        {
            int elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
            Log.Debug($"OnDayPass event received (day {elapsedDays}). Executing midnight payout...");
            IncomeEngine.TryExecuteDailyPayout(elapsedDays, cfg);
        }
        CheckCatchupPayout();
    }

    private void OnHourPass()
    {
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;
        if (cfg.PayoutHour > 0)
        {
            // Use the vanilla current-time range check so we don't have to interpret the time encoding ourselves.
            int payoutStart = cfg.PayoutHour == 0 ? 0 : cfg.PayoutHour * 100;
            int payoutEnd = payoutStart + 59;

            try
            {
                if (S1API.GameTime.TimeManager.IsCurrentTimeWithinRange(payoutStart, payoutEnd))
                {
                    int elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
                    Log.Debug($"OnHourPass event in payout window ({payoutStart:D4}-{payoutEnd:D4}). Executing configured payout for day {elapsedDays}...");
                    IncomeEngine.TryExecuteDailyPayout(elapsedDays, cfg);
                }
            }
            catch (Exception ex) { Log.Warn("OnHourPass failed", ex); }
        }
    }

    private void CheckCatchupPayout()
    {
        try
        {
            int elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
            var cfg = ModConfig<BusinessIncomeConfig>.Instance;
            var state = PayoutStateStore.GetState();

            // Fresh state file (LastPaidElapsedDay = -1): seed it to the current day so installing
            // the mod on an old save doesn't book every day since day 0 as windfall. Days after the
            // install date still pay. States with a real last-paid day (>= 0) keep full catch-up.
            if (state.LastPaidElapsedDay < 0)
            {
                state.LastPaidElapsedDay = elapsedDays;
                // Persist the seed — otherwise no state file is ever written and every
                // session re-seeds (biz stats flips "Paid Today" after each restart).
                PayoutStateStore.CommitPayout(elapsedDays, Array.Empty<string>());
                Log.Info($"Fresh payout state: seeded last-paid day to current day {elapsedDays} (skipping save history).");
            }

            int lastPaid = state.LastPaidElapsedDay;

            // F2: cap the catch-up. A huge backlog (corrupted/copied state) would otherwise
            // burst-book every day since day 0. Days beyond the cap are skipped with a
            // warning; state advances so the storm does not repeat every day-pass.
            int cap = Math.Max(1, cfg.MaxCatchupDays);
            int backlog = elapsedDays - lastPaid;
            if (backlog > cap)
            {
                Log.Warn($"Catch-up backlog of {backlog} days exceeds MaxCatchupDays={cap} — paying only the last {cap} days ({elapsedDays - cap + 1}..{elapsedDays}), rest skipped.");
                PayoutStateStore.CommitPayout(elapsedDays - cap, Array.Empty<string>());
                // Audit 2026-09-13 (BIZ-01): re-read after the cap commit — continuing on the
                // stale value made the loop below iterate the full (potentially billions-wide,
                // corrupted-state) backlog even though IsDayPaid skips the middle. Re-reading
                // bounds the loop to MaxCatchupDays iterations.
                lastPaid = PayoutStateStore.GetState().LastPaidElapsedDay;
            }

            // H5: Pay all missed days, not just current (mod disabled, sleep skip)
            // Audit 2026-09-13 (BIZ-01): belt-and-braces — never iterate more than MaxCatchupDays,
            // even if the cap commit failed (e.g. read-only disk).
            int loopStart = Math.Max(lastPaid + 1, elapsedDays - Math.Max(1, cfg.MaxCatchupDays));
            for (int d = loopStart; d <= elapsedDays; d++)
            {
                if (PayoutStateStore.IsDayPaid(d)) continue;

                // For past days, ignore payout window — they were missed.
                // For current day, respect window unless force.
                bool isPastDay = d < elapsedDays;
                if (!isPastDay)
                {
                    int payoutStart = cfg.PayoutHour == 0 ? 0 : cfg.PayoutHour * 100;
                    int payoutEnd = payoutStart + 59;
                    bool withinPayoutWindow = false;
                    try { withinPayoutWindow = S1API.GameTime.TimeManager.IsCurrentTimeWithinRange(payoutStart, payoutEnd); } catch { withinPayoutWindow = true; }
                    if (!withinPayoutWindow)
                    {
                        Log.Debug($"Catch-up: day {d} not in window {payoutStart:D4}-{payoutEnd:D4}, skip current day");
                        continue;
                    }
                }

                Log.Info($"Catch-up payout for day {d} ({(isPastDay ? "backlog" : $"window {cfg.PayoutHour * 100:D4}")})...");
                IncomeEngine.TryExecuteDailyPayout(d, cfg);
            }
        }
        catch (Exception ex)
        {
            // F3: Warn (not Debug — Debug is compiled out in release builds) so payout
            // failures are visible in the MelonLoader log instead of vanishing silently.
            Log.Warn("CheckCatchupPayout failed", ex);
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

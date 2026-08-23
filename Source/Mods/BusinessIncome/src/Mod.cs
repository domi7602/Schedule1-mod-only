using System;
using System.Reflection;
using BusinessIncome.Config;
using BusinessIncome.Services;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(BusinessIncome.Mod), "BusinessIncome", "0.1.0", "Dominik")]
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
        ModConfig<BusinessIncomeConfig>.Initialize("BusinessIncome", Log);
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
        GameLifecycle.OnSaveInfoLoaded += OnSaveLoaded;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        S1API.GameTime.TimeManager.OnDayPass += OnDayPass;
        S1API.GameTime.TimeManager.OnHourPass += OnHourPass;

        Log.Info("BusinessIncome v0.1.0 initialized.");
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main")
        {
            PayoutStateStore.ResetForSceneUnload();
            Log.Debug("Main Scene unloaded. PayoutStateStore reset (keepSlot).");
        }
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnSaveInfoLoaded -= OnSaveLoaded;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        S1API.GameTime.TimeManager.OnDayPass -= OnDayPass;
        S1API.GameTime.TimeManager.OnHourPass -= OnHourPass;
    }

    private void OnSaveLoaded()
    {
        try
        {
            string slot = PayoutStateStore.GetActiveSlotSuffix();
            var state = PayoutStateStore.GetState();
            Log.Info($"Save loaded (Slot: {slot}). Last payout on day {state.LastPaidElapsedDay}.");
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
    }

    private void OnHourPass()
    {
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;
        if (cfg.PayoutHour > 0)
        {
            // Use the vanilla current-time range check so we don't have to interpret the time encoding ourselves.
            int payoutStart = cfg.PayoutHour * 100;
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
            catch { }
        }
    }

    private void CheckCatchupPayout()
    {
        try
        {
            int elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
            var cfg = ModConfig<BusinessIncomeConfig>.Instance;

            // If the payout hour of the current day has already passed and the day was not yet paid,
            // use the vanilla current-time range check so we don't have to interpret the time encoding ourselves.
            int payoutStart = cfg.PayoutHour * 100;
            int payoutEnd = payoutStart + 59;
            bool withinPayoutWindow = S1API.GameTime.TimeManager.IsCurrentTimeWithinRange(payoutStart, payoutEnd);

            if (withinPayoutWindow && !PayoutStateStore.IsDayPaid(elapsedDays))
            {
                Log.Info($"Catch-up payout for day {elapsedDays} (current time in range {payoutStart:D4}-{payoutEnd:D4})...");
                IncomeEngine.TryExecuteDailyPayout(elapsedDays, cfg);
            }
        }
        catch (Exception ex)
        {
            Log.Debug($"CheckCatchupPayout skipped: {ex.Message}");
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

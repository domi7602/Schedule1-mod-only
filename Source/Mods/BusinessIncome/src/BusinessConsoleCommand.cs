using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BusinessIncome.Config;
using BusinessIncome.Services;
using MelonLoader;
using S1API.Console;
using S1Mods.Shared;

namespace BusinessIncome.Console;

public sealed class BusinessConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "biz";

    public override string CommandDescription =>
        "BusinessIncome: stats, trigger [--commit], config, set <key> <val>, help";

    public override string ExampleUsage => "biz stats";

    public override void ExecuteCommand(List<string> args)
    {
        try
        {
            if (args == null || args.Count == 0 || Is(args, 0, "stats", "info", "s"))
            {
                PrintStats();
                return;
            }

            string sub = args[0].ToLowerInvariant();
            switch (sub)
            {
                case "trigger":
                case "pay":
                case "run":
                    ExecuteTrigger(args);
                    return;

                case "config":
                case "cfg":
                    PrintConfig();
                    return;

                case "set":
                    ExecuteSet(args);
                    return;

                case "help":
                case "?":
                case "h":
                    PrintHelp();
                    return;

                default:
                    Print($"<color=#ff6060>Unknown command: '{sub}'. Use 'biz help'.</color>");
                    return;
            }
        }
        catch (Exception ex)
        {
            Print($"<color=#ff6060>Error: {ex.Message}</color>");
            Mod.Log.Error($"Error in biz command: {ex}");
        }
    }

    private void PrintStats()
    {
        int elapsedDays = 0;
        try
        {
            elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
        }
        catch { }

        var config = ModConfig<BusinessIncomeConfig>.Instance;
        var (lines, totalGross, totalCosts, totalNet) = IncomeEngine.GetDailyRevenuePreview(elapsedDays, config);

        bool isPaid = PayoutStateStore.IsDayPaid(elapsedDays);
        string slotSuffix = PayoutStateStore.GetActiveSlotSuffix();
        bool isHost = IncomeEngine.IsHostOrSingleplayer();

        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>==================================================</color>");
        sb.AppendLine($"<color=#60f080>★ Business Income Dashboard</color> (Day {elapsedDays})");
        sb.AppendLine($"<color=#aaaaaa>Save Slot:</color> {slotSuffix}  |  <color=#aaaaaa>Host:</color> {(isHost ? "Yes" : "No")}  |  <color=#aaaaaa>Paid Today:</color> {(isPaid ? "<color=#60f080>YES</color>" : "<color=#ffaa00>NO</color>")}");
        sb.AppendLine("<color=#60f080>--------------------------------------------------</color>");

        if (lines.Count == 0)
        {
            sb.AppendLine("<color=#ffaa00>No owned businesses found.</color>");
            sb.AppendLine("<color=#888888>Buy businesses like Laundromat or Car Wash for passive income.</color>");
        }
        else
        {
            sb.AppendLine(string.Format("{0,-18} {1,5} {2,6} {3,7} {4,8} {5,8}", "Business", "Mult", "Staff", "Var", "Gross", "Net"));
            sb.AppendLine("<color=#444444>--------------------------------------------------</color>");

            foreach (var l in lines)
            {
                string staffStr = l.EmployeeCount > 0 ? $"+{l.EmployeeBonusPercent * 100:0}%" : "-";
                string varStr = $"{l.VarianceFactor * 100:0}%";
                sb.AppendLine(string.Format("{0,-18} {1,5:0.00} {2,6} {3,7} {4,8:C0} {5,8:C0}",
                    Truncate(l.DisplayName, 18),
                    l.Multiplier,
                    staffStr,
                    varStr,
                    l.GrossRevenue,
                    l.NetRevenue));
            }

            sb.AppendLine("<color=#444444>--------------------------------------------------</color>");
            sb.AppendLine(string.Format("{0,-18} {1,5} {2,6} {3,7} {4,8:C0} {5,8:C0}",
                "TOTAL (" + lines.Count + ")",
                "-",
                "-",
                "-",
                totalGross,
                totalNet));
            sb.AppendLine($"<color=#aaaaaa>Operating Costs:</color> -${totalCosts:N2} ({config.OperatingCostRate * 100:0}%)");
        }

        sb.AppendLine("<color=#60f080>==================================================</color>");
        Print(sb.ToString());
    }

    private void ExecuteTrigger(List<string> args)
    {
        bool commit = args.Exists(a => a.Equals("--commit", StringComparison.OrdinalIgnoreCase) || a.Equals("-c", StringComparison.OrdinalIgnoreCase));
        int elapsedDays = 0;
        try
        {
            elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
        }
        catch { }

        var config = ModConfig<BusinessIncomeConfig>.Instance;

        if (!commit)
        {
            Print($"<color=#ffaa00>Running simulation (dry-run) for day {elapsedDays}...</color>");
            IncomeEngine.TryExecuteDailyPayout(elapsedDays, config, force: true, commit: false, isDryRun: true);
            var (_, _, _, totalNet) = IncomeEngine.GetDailyRevenuePreview(elapsedDays, config);
            Print($"<color=#60f080>Simulation complete: net revenue would be +${totalNet:N2}.</color>");
            Print("<color=#888888>Tip: Use 'biz trigger --commit' for a real bank transfer.</color>");
        }
        else
        {
            Print($"<color=#60f080>Executing authoritative bank booking for day {elapsedDays}...</color>");
            bool executed = IncomeEngine.TryExecuteDailyPayout(elapsedDays, config, force: true, commit: true, isDryRun: false);
            if (executed)
            {
                Print($"<color=#60f080>Payout completed successfully and state saved.</color>");
            }
            else
            {
                Print($"<color=#ffaa00>Payout could not be executed (e.g. no businesses or net $0).</color>");
            }
        }
    }

    private void PrintConfig()
    {
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>[BusinessIncome Configuration]</color>");
        sb.AppendLine($"  PayoutHour:              {cfg.PayoutHour} (0 = Midnight / DayPass)");
        sb.AppendLine($"  DefaultBaseIncome:       ${cfg.DefaultBaseIncome:N2}");
        sb.AppendLine($"  OperatingCostRate:       {cfg.OperatingCostRate * 100:0}%");
        sb.AppendLine($"  EmployeeBonusPerWorker:  +{cfg.EmployeeBonusPerWorker * 100:0}% (Max: +{cfg.MaxEmployeeBonus * 100:0}%)");
        sb.AppendLine($"  WeekendBonusRate:        +{cfg.WeekendBonusRate * 100:0}%");
        sb.AppendLine($"  EnableNotifications:     {cfg.EnableNotifications}");
        sb.AppendLine($"  PlayCashSound:           {cfg.PlayCashSound}");
        sb.AppendLine("  Multipliers:");
        foreach (var kvp in cfg.PropertyMultipliers)
        {
            sb.AppendLine($"    - {kvp.Key}: {kvp.Value:0.00}x");
        }
        Print(sb.ToString());
    }

    private void ExecuteSet(List<string> args)
    {
        if (args.Count < 3)
        {
            Print("<color=#ff6060>Usage:</color> biz set <base|hour|costs|notif|sound> <value>");
            return;
        }

        string key = args[1].ToLowerInvariant();
        string val = args[2];
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;

        switch (key)
        {
            case "base":
            case "baseincome":
                if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out float baseVal) && baseVal >= 0f)
                {
                    ModConfig<BusinessIncomeConfig>.SetAndSave("DefaultBaseIncome", baseVal);
                    ConfigJsonStore.Save(ModConfig<BusinessIncomeConfig>.Instance);
                    Print($"<color=#60f080>DefaultBaseIncome set to ${baseVal:N2}.</color>");
                }
                else Print("<color=#ff6060>Invalid number value.</color>");
                return;

            case "hour":
            case "payouthour":
                if (int.TryParse(val, out int hourVal) && hourVal >= 0 && hourVal <= 23)
                {
                    ModConfig<BusinessIncomeConfig>.SetAndSave("PayoutHour", hourVal);
                    ConfigJsonStore.Save(ModConfig<BusinessIncomeConfig>.Instance);
                    Print($"<color=#60f080>PayoutHour set to {hourVal}:00.</color>");
                }
                else Print("<color=#ff6060>Invalid hour (0-23).</color>");
                return;

            case "costs":
            case "costrate":
                if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out float costVal) && costVal >= 0f && costVal <= 1f)
                {
                    ModConfig<BusinessIncomeConfig>.SetAndSave("OperatingCostRate", costVal);
                    ConfigJsonStore.Save(ModConfig<BusinessIncomeConfig>.Instance);
                    Print($"<color=#60f080>OperatingCostRate set to {costVal * 100:0}%.</color>");
                }
                else Print("<color=#ff6060>Invalid rate (0.00 - 1.00).</color>");
                return;

            case "notif":
            case "notifications":
                if (bool.TryParse(val, out bool notifVal))
                {
                    ModConfig<BusinessIncomeConfig>.SetAndSave("EnableNotifications", notifVal);
                    ConfigJsonStore.Save(ModConfig<BusinessIncomeConfig>.Instance);
                    Print($"<color=#60f080>EnableNotifications set to {notifVal}.</color>");
                }
                else Print("<color=#ff6060>Invalid bool value (true/false).</color>");
                return;

            case "sound":
                if (bool.TryParse(val, out bool soundVal))
                {
                    ModConfig<BusinessIncomeConfig>.SetAndSave("PlayCashSound", soundVal);
                    ConfigJsonStore.Save(ModConfig<BusinessIncomeConfig>.Instance);
                    Print($"<color=#60f080>PlayCashSound set to {soundVal}.</color>");
                }
                else Print("<color=#ff6060>Invalid bool value (true/false).</color>");
                return;

            default:
                Print($"<color=#ff6060>Unknown key '{key}'.</color> Allowed: base, hour, costs, notif, sound");
                return;
        }
    }

    private void PrintHelp()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>[BusinessIncome Help]</color>");
        sb.AppendLine("  biz stats                  - Shows revenue preview of all owned businesses");
        sb.AppendLine("  biz trigger                - Simulates a payout (dry-run without booking)");
        sb.AppendLine("  biz trigger --commit       - Executes real payout and saves marker");
        sb.AppendLine("  biz config                 - Shows current configuration");
        sb.AppendLine("  biz set <key> <val>        - Configures settings at runtime");
        sb.AppendLine("  biz help                   - Shows this help");
        Print(sb.ToString());
    }

    private static void Print(string message)
    {
        MelonLogger.Msg(message);
    }

    private static bool Is(List<string> args, int index, params string[] values)
    {
        if (args.Count <= index) return false;
        string actual = args[index].ToLowerInvariant();
        foreach (var v in values)
        {
            if (actual == v) return true;
        }
        return false;
    }

    private static string Truncate(string val, int maxLen)
    {
        if (string.IsNullOrEmpty(val)) return "";
        return val.Length <= maxLen ? val : val[..(maxLen - 2)] + "..";
    }
}

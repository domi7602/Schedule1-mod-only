using System;
using System.Collections.Generic;
using HomelessMod.Building;
using HomelessMod.Quests;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.PlayerScripts;
using S1API.Console;
using UnityEngine;

namespace HomelessMod.Commands;

/// <summary>
/// In-game console commands for the Homeless Mod ("Street Nomad").
/// Usage: `homeless bag`, `homeless start`, `homeless stats`, `homeless help`
/// </summary>
public sealed class HomelessConsoleCommands : BaseConsoleCommand
{
    public override string CommandWord => "homeless";

    public override string CommandDescription => "Homeless Mod commands: bag, start, stats, help";

    public override string ExampleUsage => "homeless bag";

    public override void ExecuteCommand(List<string> args)
    {
        try
        {
            if (args == null || args.Count == 0 || Is(args, 0, "help", "h", "?"))
            {
                PrintHelp();
                return;
            }

            string sub = args[0].ToLowerInvariant();
            switch (sub)
            {
                case "bag":
                case "sleepingbag":
                    GiveSleepingBag();
                    break;

                case "start":
                case "challenge":
                    StartHomelessChallenge();
                    break;

                case "stats":
                case "status":
                case "info":
                    PrintStats();
                    break;

                default:
                    PrintHelp();
                    break;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Homeless console command error: {ex.Message}");
        }
    }

    private void GiveSleepingBag()
    {
        var cfg = Mod.CurrentConfig;
        Mod.Log.Info($"Adding Sleeping Bag ('{cfg.SleepingBagItemId}') to inventory...");
        ConsoleHelper.AddItemToInventory(cfg.SleepingBagItemId, 1);
    }

    private void StartHomelessChallenge()
    {
        Mod.Log.Info("========================================");
        Mod.Log.Info(" STARTING HOMELESS SURVIVAL CHALLENGE!");
        Mod.Log.Info("========================================");

        // 1. Give Sleeping Bag
        ConsoleHelper.AddItemToInventory(Mod.CurrentConfig.SleepingBagItemId, 1);

        // 2. Initialize Quests
        HomelessQuestManager.InitializeQuests();

        Mod.Log.Info("Sleeping bag granted. Quests activated. Good luck on the streets!");
    }

    private void PrintStats()
    {
        var cfg = Mod.CurrentConfig;
        var moneyMgr = NetworkSingleton<MoneyManager>.Instance;
        float cash = moneyMgr != null && moneyMgr.Pointer != IntPtr.Zero ? moneyMgr.cashBalance : 0f;

        Mod.Log.Info("========================================");
        Mod.Log.Info(" HOMELESS MOD ('Street Nomad') STATUS");
        Mod.Log.Info("========================================");
        Mod.Log.Info($" Everywhere Building: {(cfg.EnableEverywhereBuilding ? "ENABLED" : "DISABLED")}");
        Mod.Log.Info($" Grid Snap Size:       {cfg.OutdoorGridSnapSize:F2} m");
        Mod.Log.Info($" Placed Street Items:  {StreetPropertyManager.ActiveStreetObjects.Count}");
        Mod.Log.Info($" Current Cash:         ${cash:N2}");
        Mod.Log.Info($" Sleeping Bag Item ID: {cfg.SleepingBagItemId}");
        Mod.Log.Info("========================================");
    }

    private void PrintHelp()
    {
        Mod.Log.Info("Homeless Mod Commands:");
        Mod.Log.Info("  homeless bag   - Gives a portable sleeping bag to your inventory");
        Mod.Log.Info("  homeless start - Begins the Street Nomad survival challenge");
        Mod.Log.Info("  homeless stats - Displays active outdoor setups and stats");
        Mod.Log.Info("  homeless help  - Shows this help message");
    }

    private static bool Is(List<string> args, int index, params string[] values)
    {
        if (args == null || index >= args.Count) return false;
        string v = args[index];
        foreach (var candidate in values)
        {
            if (v.Equals(candidate, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }
}

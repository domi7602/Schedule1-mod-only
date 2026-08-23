using System;
using System.Collections.Generic;
using System.Globalization;
using MelonLoader;
using S1API.Console;
using S1Mods.Shared;

namespace ProfitTracker;

public sealed class ProfitConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "profit";

    public override string CommandDescription =>
        "ProfitTracker: status, reset, toggle, mode, pos <x> <y>, scale <val>, help";

    public override string ExampleUsage => "profit status";

    public override void ExecuteCommand(List<string> args)
    {
        try
        {
            var mod = ProfitTrackerMod.Instance;
            if (mod == null)
            {
                Print("ProfitTracker mod instance is not running.");
                return;
            }

            if (args == null || args.Count == 0 || Is(args, 0, "status", "info", "s"))
            {
                mod.PrintStatusToConsole();
                return;
            }

            string sub = args[0].ToLowerInvariant();
            switch (sub)
            {
                case "reset":
                case "r":
                    mod.ResetBaselines();
                    Print("<color=#60f080>[ProfitTracker]</color> Baselines for Today and Week have been reset.");
                    return;

                case "toggle":
                case "t":
                    mod.ToggleHUD();
                    Print($"<color=#60f080>[ProfitTracker]</color> HUD is now {(ModConfig<ProfitTrackerConfig>.Instance.Visible ? "VISIBLE" : "HIDDEN")}.");
                    return;

                case "show":
                    ModConfig<ProfitTrackerConfig>.SetAndSave("Visible", true);
                    mod.ApplyHUDConfig();
                    Print("<color=#60f080>[ProfitTracker]</color> HUD is now VISIBLE.");
                    return;

                case "hide":
                    ModConfig<ProfitTrackerConfig>.SetAndSave("Visible", false);
                    mod.ApplyHUDConfig();
                    Print("<color=#60f080>[ProfitTracker]</color> HUD is now HIDDEN.");
                    return;

                case "mode":
                case "m":
                    bool newMode = !ModConfig<ProfitTrackerConfig>.Instance.TrackInventoryNetWorth;
                    ModConfig<ProfitTrackerConfig>.SetAndSave("TrackInventoryNetWorth", newMode);
                    Print($"<color=#60f080>[ProfitTracker]</color> Metric mode switched to: {(newMode ? "Total Net Worth (Betrieb + Ware)" : "Cash / Liquid Only")}.");
                    return;

                case "session":
                    bool showSession = !ModConfig<ProfitTrackerConfig>.Instance.ShowSessionRow;
                    ModConfig<ProfitTrackerConfig>.SetAndSave("ShowSessionRow", showSession);
                    Print($"<color=#60f080>[ProfitTracker]</color> Session profit row: {(showSession ? "ENABLED" : "DISABLED")}.");
                    return;

                case "pos":
                case "position":
                    if (args.Count >= 3 &&
                        float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float px) &&
                        float.TryParse(args[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float py))
                    {
                        ModConfig<ProfitTrackerConfig>.Instance.PositionX = px;
                        ModConfig<ProfitTrackerConfig>.Instance.PositionY = py;
                        ModConfig<ProfitTrackerConfig>.Save();
                        mod.ApplyHUDConfig();
                        Print($"<color=#60f080>[ProfitTracker]</color> Position set to ({px:F1}, {py:F1}).");
                    }
                    else
                    {
                        Print("<color=#f06060>[ProfitTracker]</color> Usage: profit pos <x> <y>");
                    }
                    return;

                case "scale":
                    if (args.Count >= 2 && float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float scale))
                    {
                        ModConfig<ProfitTrackerConfig>.SetAndSave("Scale", Math.Clamp(scale, 0.5f, 2.5f));
                        mod.ApplyHUDConfig();
                        Print($"<color=#60f080>[ProfitTracker]</color> Scale set to {scale:F2}.");
                    }
                    else
                    {
                        Print("<color=#f06060>[ProfitTracker]</color> Usage: profit scale <0.5-2.5>");
                    }
                    return;

                case "help":
                case "h":
                case "?":
                default:
                    PrintHelp();
                    return;
            }
        }
        catch (Exception ex)
        {
            Print($"<color=#f06060>[ProfitTracker Error]</color> {ex.Message}");
        }
    }

    private static void PrintHelp()
    {
        Print("<color=#60f080>=== ProfitTracker Commands ===</color>");
        Print("  <color=#80d0ff>profit status</color>       - Show live profit, net worth, and liquid balances");
        Print("  <color=#80d0ff>profit reset</color>        - Reset Today & rolling Week baselines to current");
        Print("  <color=#80d0ff>profit toggle</color>       - Toggle HUD overlay visibility");
        Print("  <color=#80d0ff>profit mode</color>         - Switch between Total Net Worth and Cash-Only");
        Print("  <color=#80d0ff>profit session</color>      - Toggle 3rd row for Session profit");
        Print("  <color=#80d0ff>profit pos <x> <y></color>  - Set custom anchored HUD position");
        Print("  <color=#80d0ff>profit scale <val></color>  - Adjust HUD scale factor");
    }

    private static bool Is(List<string> args, int index, params string[] options)
    {
        if (index < 0 || index >= args.Count)
            return false;
        string val = args[index];
        foreach (var opt in options)
        {
            if (string.Equals(val, opt, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static void Print(string message)
    {
        MelonLogger.Msg(message);
    }
}

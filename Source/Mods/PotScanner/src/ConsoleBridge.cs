using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MelonLoader;
using PotScanner.Services;
using PotScanner.Utils;
using S1API.Console;
using S1Mods.Shared;

namespace PotScanner;

/// <summary>
/// Terminal bridge — exposes PotScanner as `pot`-subcommands in the in-game console
/// (vanilla grey bar AND the hash phone-app terminal). S1API's BaseConsoleCommand system
/// auto-discovers every subclass in the loaded mod via reflection
/// (S1API.Internal.Console.ConsolePatches.AddCommands on Console.Awake), instantiates it
/// with Activator, and routes "pot ..." lines through ExecuteCommand(args) by patching
/// Console.SubmitCommand. hash's autocomplete/help picks up the word separately via
/// HashCommands.Add() in Mod.OnInitializeMelon.
///
/// Subcommands:
///   pot list         — detailed list of every known pot (property, plant, water%, growth%, ready)
///   pot summary      — compact 1-line overview per property (alias: status)
///   pot stats        — global statistics (total, owned, growing, ready, thirsty, cost)
///   pot water-all    — trigger WaterAll (50g/pot, owned + dry)
///   pot auto         — toggle auto-water
///   pot auto on|off  — set auto-water explicitly
///   pot refresh      — force an immediate pot-scan (otherwise 2s-polling)
///   pot help         — print command list and usage
/// </summary>
public sealed class PotConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "pot";

    public override string CommandDescription =>
        " list, summary, stats, water-all, auto [on|off], refresh, help";

    public override string ExampleUsage => "pot summary";

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
                case "list":
                case "ls":
                case "l":
                    PrintList();
                    return;

                case "summary":
                case "status":
                case "sum":
                case "s":
                    PrintSummary();
                    return;

                case "stats":
                case "stat":
                case "info":
                case "st":
                    PrintStats();
                    return;

                case "water-all":
                case "waterall":
                case "water":
                case "w":
                    PrintWaterAll();
                    return;

                case "auto":
                case "a":
                    HandleAuto(args);
                    return;

                case "refresh":
                case "r":
                    PotTracker.Instance.RefreshNow();
                    MelonLogger.Msg(" Refresh forced.");
                    return;

                default:
                    MelonLogger.Msg($" Unknown subcommand '{args[0]}'. Type `pot help` for available commands.");
                    return;
            }
        }
        catch (Exception ex)
        {
            // A console command must never crash the caller.
            MelonLogger.Warning($" 'pot' command failed: {ex.Message}");
        }
    }

    private void PrintHelp()
    {
        MelonLogger.Msg(
            "commands:\n" +
            "  pot list          list every pot with details (property, plant, water%, growth%, ready)\n" +
            "  pot summary       compact 1-line overview per property (alias: status)\n" +
            "  pot stats         global statistics for all pots, crops, and watering costs\n" +
            "  pot water-all     water every owned dry pot (50g/pot, charges cash)\n" +
            "  pot auto          toggle auto-water (persisted)\n" +
            "  pot auto on|off   set auto-water explicitly\n" +
            "  pot refresh       force a pot-scan (otherwise 2s-polling)\n" +
            "  pot help          print this help");
    }

    private void PrintSummary()
    {
        var pots = PotTracker.Instance.Pots;
        if (pots.Count == 0)
        {
            MelonLogger.Msg(" No pots found in scene.");
            return;
        }

        var byProperty = pots
            .GroupBy(p => string.IsNullOrEmpty(p.PropertyName)
                ? (string.IsNullOrEmpty(p.PropertyCode) ? "(unknown)" : p.PropertyCode)
                : p.PropertyName)
            .OrderBy(g => g.Key);

        var propertySummaries = new List<string>();
        foreach (var group in byProperty)
        {
            int total = group.Count();
            int ready = group.Count(p => p.IsFullyGrown);
            int dry = group.Count(p => p.WaterPercent < Constants.WaterAllSkipThreshold);
            int growing = group.Count(p => !p.IsFullyGrown && !string.IsNullOrEmpty(p.PlantName));
            int empty = group.Count(p => string.IsNullOrEmpty(p.PlantName));
            int moist = group.Count(p => p.WaterPercent >= Constants.WaterAllSkipThreshold);

            var parts = new List<string>();
            if (ready > 0) parts.Add($"{ready} ready");
            if (dry > 0) parts.Add($"{dry} dry");
            if (growing > 0) parts.Add($"{growing} growing");
            if (empty > 0) parts.Add($"{empty} empty");
            if (parts.Count == 0) parts.Add($"{moist} moist");

            string potUnit = total == 1 ? "pot" : "pots";
            propertySummaries.Add($"{group.Key}: {total} {potUnit} ({string.Join(", ", parts)})");
        }

        MelonLogger.Msg($" {string.Join(" | ", propertySummaries)}");
    }

    private void PrintStats()
    {
        var pots = PotTracker.Instance.Pots;
        if (pots.Count == 0)
        {
            MelonLogger.Msg(" No pots found in scene.");
            return;
        }

        int totalPots = pots.Count;
        int ownedPots = pots.Count(p => p.IsOwnedProperty);
        int plantsGrowing = pots.Count(p => !p.IsFullyGrown && !string.IsNullOrEmpty(p.PlantName));
        int readyToHarvest = pots.Count(p => p.IsFullyGrown);
        int thirstyPots = pots.Count(p => p.IsOwnedProperty && p.WaterPercent < Constants.WaterAllSkipThreshold);
        float costToWaterAll = thirstyPots * Constants.WaterAllCostPerPot;
        int emptyPots = pots.Count(p => string.IsNullOrEmpty(p.PlantName));

        int ownedPct = totalPots > 0 ? (ownedPots * 100 / totalPots) : 0;

        MelonLogger.Msg(
            "Statistics:\n" +
            $"  Total pots:          {totalPots}\n" +
            $"  Owned pots:          {ownedPots} ({ownedPct}%)\n" +
            $"  Empty pots:          {emptyPots}\n" +
            $"  Plants growing:      {plantsGrowing}\n" +
            $"  Ready to harvest:    {readyToHarvest}\n" +
            $"  Thirsty pots (owned): {thirstyPots}\n" +
            $"  Total cost to water: {costToWaterAll:0}g ({thirstyPots} x {Constants.WaterAllCostPerPot:0}g)\n" +
            $"  Auto-Water service:  {(AutoWaterService.IsEnabled ? "ENABLED" : "DISABLED")}");
    }

    private void PrintList()
    {
        var pots = PotTracker.Instance.Pots;
        if (pots.Count == 0)
        {
            MelonLogger.Msg(" No pots found in scene.");
            return;
        }

        var sb = new StringBuilder();
        sb.Append(" ").Append(pots.Count).Append(" pots, ")
          .Append(pots.Count(p => p.IsFullyGrown)).Append(" ready, ")
          .Append(pots.Count(p => p.WaterPercent < Constants.WaterAllSkipThreshold)).Append(" dry");

        // Group by property so the eye can find the property it cares about quickly.
        var byProperty = pots
            .GroupBy(p => string.IsNullOrEmpty(p.PropertyName)
                ? (string.IsNullOrEmpty(p.PropertyCode) ? "(unknown)" : p.PropertyCode)
                : p.PropertyName)
            .OrderBy(g => g.Key);

        foreach (var group in byProperty)
        {
            sb.Append("\n  ").Append(group.Key).Append(':');
            foreach (var pot in group.OrderByDescending(p => p.IsFullyGrown)
                                      .ThenByDescending(p => p.GrowthPercent))
            {
                string status = pot.IsFullyGrown
                    ? "ready"
                    : !string.IsNullOrEmpty(pot.PlantName)
                        ? "grow "
                        : "empty";
                string plant = string.IsNullOrEmpty(pot.PlantName) ? "-" : pot.PlantName;
                sb.Append("\n    [").Append(status).Append("] ")
                  .Append(plant.PadRight(18))
                  .Append("  W:").Append(PercentString(pot.WaterPercent))
                  .Append("  G:").Append(PercentString(pot.GrowthPercent))
                  .Append("  S:").Append(PercentString(pot.SoilPercent));
            }
        }

        MelonLogger.Msg(sb.ToString());
    }

    private void PrintWaterAll()
    {
        var result = WaterAllService.WaterAll();
        MelonLogger.Msg($"Water-All: {result.Message}");
    }

    private void HandleAuto(List<string> args)
    {
        bool? target = null;
        if (args.Count >= 2)
        {
            string arg = args[1].ToLowerInvariant();
            if (arg is "on" or "1" or "true" or "yes" or "enable" or "enabled")
                target = true;
            else if (arg is "off" or "0" or "false" or "no" or "disable" or "disabled")
                target = false;
        }

        if (target == null)
        {
            // Toggle.
            target = !AutoWaterService.IsEnabled;
        }

        bool newState = target.Value;
        AutoWaterService.SetEnabled(newState);
        ModConfig<PotScannerConfig>.SetAndSave(nameof(PotScannerConfig.AutoWaterEnabled), newState);
        MelonLogger.Msg($"Auto-Water: {(newState ? "ENABLED" : "DISABLED")}.");
    }

    private static string PercentString(float fraction)
    {
        int pct = (int)Math.Round(Math.Clamp(fraction, 0f, 1f) * 100f);
        // 3-char field keeps the columns aligned in the terminal.
        return pct.ToString().PadLeft(3) + '%';
    }

    private static bool Is(List<string> args, int idx, params string[] options)
    {
        if (args == null || idx >= args.Count) return false;
        string actual = args[idx]?.ToLowerInvariant() ?? string.Empty;
        return options.Any(o => string.Equals(o, actual, StringComparison.OrdinalIgnoreCase));
    }
}

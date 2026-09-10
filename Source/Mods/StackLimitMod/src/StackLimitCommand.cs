using System;
using System.Collections.Generic;
using System.Text;
using MelonLoader;
using S1API.Console;

namespace StackLimitMod;

public sealed class StackLimitCommand : BaseConsoleCommand
{
    public override string CommandWord => "stack";
    public override string CommandDescription => "StackLimit: stats, set <amount>, reload, help";
    public override string ExampleUsage => "stack stats";

    public override void ExecuteCommand(List<string> args)
    {
        Execute(args);
    }

    public static void Execute(List<string> args)
    {
        try
        {
            if (args == null || args.Count == 0 || Is(args, 0, "stats", "info", "s", "status"))
            {
                PrintStats();
                return;
            }

            string sub = args[0].ToLowerInvariant();
            switch (sub)
            {
                case "set":
                    ExecuteSet(args);
                    return;

                case "reload":
                case "r":
                    ExecuteReload();
                    return;

                case "help":
                case "h":
                case "?":
                    PrintHelp();
                    return;

                default:
                    MelonLogger.Msg($"<color=#ff6060>Unknown command: '{sub}'. Use 'stack help'.</color>");
                    return;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Msg($"<color=#ff6060>Error executing stack command: {ex.Message}</color>");
            Mod.Log?.Error($"Error executing stack command: {ex}");
        }
    }

    private static void PrintStats()
    {
        var cfg = Mod.Config;
        if (cfg == null)
        {
            cfg = StackLimitConfig.Load();
            Mod.Config = cfg;
        }
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>==================================================</color>");
        sb.AppendLine("<color=#60f080>★ StackLimitMod Status</color>");
        sb.AppendLine("<color=#60f080>--------------------------------------------------</color>");
        sb.AppendLine($"  <color=#aaaaaa>Current Limit:</color>          {cfg.StackLimit}");
        sb.AppendLine($"  <color=#aaaaaa>Modified Items:</color>         {StackLimitEngine.ModifiedItemCount}");
        sb.AppendLine($"  <color=#aaaaaa>Tracked Items:</color>          {StackLimitEngine.TrackedItemCount}");
        sb.AppendLine($"  <color=#aaaaaa>Override Non-Stackable:</color> {cfg.OverrideNonStackable}");
        sb.AppendLine($"  <color=#aaaaaa>Excluded Item IDs:</color>      {(cfg.ExcludedItemIds.Count > 0 ? string.Join(", ", cfg.ExcludedItemIds) : "(none)")}");
        sb.AppendLine("<color=#60f080>==================================================</color>");
        MelonLogger.Msg(sb.ToString());
    }

    private static void ExecuteSet(List<string> args)
    {
        if (args.Count < 2)
        {
            MelonLogger.Msg("<color=#ff6060>Usage:</color> stack set <amount> (1 - 9999)");
            return;
        }

        if (!int.TryParse(args[1], out int amount) || amount < 1 || amount > 9999)
        {
            MelonLogger.Msg("<color=#ff6060>Invalid amount. Must be an integer between 1 and 9999.</color>");
            return;
        }

        int before = Mod.Config.StackLimit;
        Mod.Config.StackLimit = amount;
        Mod.Config.Validate();
        if (Mod.Config.StackLimit != amount)
        {
            MelonLogger.Msg($"<color=#e67e22>Clamped {amount} -> {Mod.Config.StackLimit} (1..9999)</color>");
        }
        Mod.Config.Save();

        int modified = StackLimitEngine.ApplyStackLimits(Mod.Config);
        MelonLogger.Msg($"<color=#60f080>Stack limit set to {Mod.Config.StackLimit} (was {before}). Applied to {modified} items and saved to config.</color>");
    }

    private static void ExecuteReload()
    {
        Mod.Config = StackLimitConfig.Load();
        StackLimitPatches.ClearDecisionCache();
        int modified = StackLimitEngine.ApplyStackLimits(Mod.Config);
        MelonLogger.Msg($"<color=#60f080>StackLimitMod config reloaded. Applied limit ({Mod.Config.StackLimit}) to {modified} items.</color>");
    }

    private static void PrintHelp()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>[StackLimitMod Help]</color>");
        sb.AppendLine("  stack                     - Shows current stack limit status and modified item count");
        sb.AppendLine("  stack stats               - Shows detailed status and configuration");
        sb.AppendLine("  stack set <1-9999>        - Sets stack limit, saves config, and reapplies immediately");
        sb.AppendLine("  stack reload              - Reloads configuration from disk and reapplies");
        sb.AppendLine("  stack help                - Displays this help menu");
        MelonLogger.Msg(sb.ToString());
    }

    private static bool Is(List<string> args, int index, params string[] values)
    {
        if (args.Count <= index) return false;
        string actual = args[index].ToLowerInvariant();
        for (int i = 0; i < values.Length; i++)
        {
            if (actual == values[i]) return true;
        }
        return false;
    }
}

public sealed class StackLimitAliasCommand : BaseConsoleCommand
{
    public override string CommandWord => "stacklimit";
    public override string CommandDescription => "Alias for 'stack'";
    public override string ExampleUsage => "stacklimit stats";

    public override void ExecuteCommand(List<string> args)
    {
        StackLimitCommand.Execute(args);
    }
}

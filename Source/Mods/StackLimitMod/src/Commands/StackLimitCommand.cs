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

                case "check":
                    ExecuteCheck(args);
                    return;

                case "report":
                    ExecuteReport();
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
        sb.AppendLine($"  <color=#aaaaaa>Agriculture Only:</color>       {cfg.AgricultureOnly} (agriculture + ingredients when true)");
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
            MelonLogger.Msg("<color=#ff6060>Usage:</color> stack set <amount> (1 - 9999) OR stack set ag <true|false>");
            return;
        }

        if (args.Count >= 3 && Is(args, 1, "ag", "agriculture", "agricultureonly"))
        {
            if (bool.TryParse(args[2], out bool agOnly))
            {
                Mod.Config.AgricultureOnly = agOnly;
                Mod.Config.Save();
                StackLimitPatches.ClearDecisionCache();
                int modifiedCount = StackLimitEngine.ApplyStackLimits(Mod.Config);
                MelonLogger.Msg($"<color=#60f080>AgricultureOnly set to {agOnly}. Reapplied limit to {modifiedCount} items.</color>");
                return;
            }
            MelonLogger.Msg("<color=#ff6060>Usage:</color> stack set ag <true|false>");
            return;
        }

        if (!int.TryParse(args[1], out int amount) || amount < 1 || amount > 9999)
        {
            MelonLogger.Msg("<color=#ff6060>Invalid amount. Must be an integer between 1 and 9999, or 'stack set ag <true|false>'.</color>");
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

    /// <summary>
    /// v0.1.5 live diagnosis: evaluates every filter for one item ID and prints the verdict.
    /// Usage: stack check ogkush | stack check baggie
    /// </summary>
    private static void ExecuteCheck(List<string> args)
    {
        if (args.Count < 2 || string.IsNullOrWhiteSpace(args[1]))
        {
            MelonLogger.Msg("<color=#ff6060>Usage:</color> stack check <itemId>");
            return;
        }
        string id = args[1].Trim();
        var cfg = Mod.Config;
        if (cfg == null)
        {
            MelonLogger.Msg("<color=#ff6060>Config not loaded.</color>");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"<color=#60f080>Diagnosis for '{id}':</color>");
        sb.AppendLine($"  <color=#aaaaaa>Weapon/Ammo ID match:</color>   {StackLimitEngine.IsWeaponOrAmmoId(id)}");

        // Resolve the definition (live Registry lookup) and evaluate type checks.
        string typeName = "(not found)";
        int defStackLimit = -1;
        bool defIsAgri = false;
        bool defIsIngredient = false;
        bool defIsWeapon = false;
        try
        {
            var def = Il2CppScheduleOne.Registry.GetItem(id);
            if (def != null && def.Pointer != IntPtr.Zero && !def.WasCollected)
            {
                typeName = def.GetIl2CppType().Name;
                defStackLimit = def.StackLimit;
                defIsAgri = StackLimitEngine.IsAgricultureItem(def);
                defIsIngredient = StackLimitEngine.IsIngredientItem(def);
                defIsWeapon = StackLimitEngine.IsWeaponOrAmmo(def);
            }
        }
        catch (Exception ex)
        {
            typeName = $"error: {ex.Message}";
        }

        bool idIsAgri = StackLimitEngine.IsAgricultureId(id);
        sb.AppendLine($"  <color=#aaaaaa>Definition:</color>            {typeName}");
        sb.AppendLine($"  <color=#aaaaaa>Def StackLimit (live):</color>   {defStackLimit}");
        sb.AppendLine($"  <color=#aaaaaa>Def Weapon/Ammo (type):</color>  {defIsWeapon}");
        sb.AppendLine($"  <color=#aaaaaa>Def Agriculture (type):</color>  {defIsAgri}");
        sb.AppendLine($"  <color=#aaaaaa>Def Ingredient (category):</color> {defIsIngredient}");
        sb.AppendLine($"  <color=#aaaaaa>ID Agriculture match:</color>    {idIsAgri}");
        sb.AppendLine($"  <color=#aaaaaa>Excluded:</color>               {StackLimitEngine.IsExcluded(id)}");
        sb.AppendLine($"  <color=#aaaaaa>Original known (tracked):</color> {StackLimitEngine.IsOriginalKnown(id)} (captured: {StackLimitEngine.GetOriginalLimit(id)})");
        sb.AppendLine($"  <color=#aaaaaa>Eligible for override:</color>   {StackLimitEngine.IsEligibleForOverride(id)}");

        string verdict;
        if (defStackLimit < 0)
            verdict = "<color=#e67e22>Definition NOT FOUND in Registry — item unknown to the game or not yet registered.</color>";
        else if (defIsWeapon)
            verdict = "<color=#e67e22>Protected (weapon/ammo) — intentionally never stacked.</color>";
        else if (StackLimitEngine.IsExcluded(id))
            verdict = "<color=#e67e22>Excluded via ExcludedItemIds in config.json.</color>";
        else if (cfg.AgricultureOnly && !defIsAgri && !defIsIngredient && !idIsAgri)
            verdict = "<color=#e67e22>NOT agriculture/ingredient (AgricultureOnly=true) — raise would be skipped. If this is wrong, extend IsAgricultureItem/IsAgricultureId/IsIngredientItem.</color>";
        else if (defStackLimit == cfg.StackLimit)
            verdict = "<color=#60f080>Definition already carries the target limit — check the INSTANCE path (postfix) if stacking still fails.</color>";
        else
            verdict = "<color=#e67e22>Definition limit differs from target — apply did not reach this item (timing or scan gap).</color>";
        sb.AppendLine($"  <color=#ffffff>Verdict:</color>              {verdict}");
        MelonLogger.Msg(sb.ToString());
    }

    /// <summary>
    /// v0.1.5: prints a summary of the last apply report (per-decision decisions from
    /// apply_report.json) without leaving the game.
    /// </summary>
    private static void ExecuteReport()
    {
        var entries = StackLimitEngine.GetReportSnapshot();
        var sb = new StringBuilder();
        sb.AppendLine($"<color=#60f080>Last apply report: {entries.Count} decision entries (RegistryCount={StackLimitEngine.LastRegistryCount}, full file: UserData/StackLimitMod/apply_report.json)</color>");

        int shown = 0;
        foreach (var e in entries)
        {
            if (e.Decision == "modified") continue; // summary focus: why NOT modified
            sb.AppendLine($"  <color=#aaaaaa>{e.Source}:</color> {e.Id} <color=#888888>({e.TypeName})</color> orig={e.OriginalLimit} → {e.Decision}");
            if (++shown >= 40)
            {
                sb.AppendLine($"  <color=#888888>... {entries.Count - shown} more (see apply_report.json)</color>");
                break;
            }
        }
        MelonLogger.Msg(sb.ToString());
    }

    private static void PrintHelp()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>[StackLimitMod Help]</color>");
        sb.AppendLine("  stack                     - Shows current stack limit status and modified item count");
        sb.AppendLine("  stack stats               - Shows detailed status and configuration");
        sb.AppendLine("  stack set <1-9999>        - Sets stack limit, saves config, and reapplies immediately");
        sb.AppendLine("  stack set ag <true|false> - Toggle Agriculture-Only mode (agriculture + ingredients; protects weapons & ammo)");
        sb.AppendLine("  stack check <itemId>      - Diagnoses why an item is (not) stack-limited");
        sb.AppendLine("  stack report              - Summarizes the last apply (who was skipped and why)");
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

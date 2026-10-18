using System;
using System.Collections.Generic;
using CustomSkateboard.Items;
using S1API.Console;

namespace CustomSkateboard.Commands;

/// <summary>
/// In-game console command for testing and managing the Custom Skateboard.
/// Usage: `skate give`, `skate stats`, `skate help`
/// </summary>
public sealed class SkateboardConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "skate";

    public override string CommandDescription => "CustomSkateboard mod commands: give, stats, help";

    public override string ExampleUsage => "skate give";

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
                case "give":
                case "spawn":
                    GiveSkateboard();
                    break;

                case "stats":
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
            Mod.Log.Error($"Console command error: {ex.Message}");
        }
    }

    private void GiveSkateboard()
    {
        var cfg = Mod.CurrentConfig;
        if (cfg == null)
        {
            Mod.Log.Warn("Config not loaded.");
            return;
        }

        // Gatekeeper-fix B14: console `skate give` without registration check — verify item exists before inventory add.
        var def = SkateboardItemFactory.CustomSkateboardItem;
        if (def == null || def.Pointer == IntPtr.Zero)
        {
            Mod.Log.Warn($"Cannot give '{cfg.SkateboardId}': item not registered yet (base prefab not found or lifecycle not reached OnPreLoad). Try again after loading a save, or check logs for registration errors.");
            return;
        }

        Mod.Log.Info($"Adding '{cfg.SkateboardName}' ({cfg.SkateboardId}) to inventory...");
        ConsoleHelper.AddItemToInventory(cfg.SkateboardId, 1);
    }

    private void PrintStats()
    {
        var cfg = Mod.CurrentConfig;
        if (cfg == null)
        {
            Mod.Log.Warn("Config not loaded.");
            return;
        }

        Mod.Log.Info("========================================");
        Mod.Log.Info($" Skateboard:      {cfg.SkateboardName} ({cfg.SkateboardId})");
        Mod.Log.Info($" Price:            ${cfg.Price:N0}");
        Mod.Log.Info($" Top Speed:        {cfg.TopSpeed_Kmh} km/h");
        Mod.Log.Info($" Push Force:       x{cfg.PushForceMultiplier:F1} (Cooldown: {cfg.PushCooldown:F2}s)");
        Mod.Log.Info($" Turn Force:       x{cfg.TurnForce:F1} (Rate: {cfg.TurnChangeRate:F1}, Boost: {cfg.TurnSpeedBoost:F1}x)");
        Mod.Log.Info($" Jump Force:       x{cfg.JumpForce:F1} (Duration: {cfg.JumpDuration_Min:F2}s-{cfg.JumpDuration_Max:F2}s, Boost: {cfg.JumpForwardBoost:F1}x)");
        Mod.Log.Info($" Air Control:      {(cfg.AirMovementEnabled ? $"Enabled (Force: {cfg.AirMovementForce:F1})" : "Disabled")}");
        Mod.Log.Info($" Anti-Gravel:      {(cfg.DisableTerrainSlowdown ? "Active (100% Smooth)" : "Off")}");
        Mod.Log.Info("========================================");
    }

    private void PrintHelp()
    {
        Mod.Log.Info("CustomSkateboard Commands:");
        Mod.Log.Info("  skate give   - Adds the custom skateboard directly to your inventory");
        Mod.Log.Info("  skate stats  - Displays physics parameters and configuration");
        Mod.Log.Info("  skate help   - Shows this help message");
    }

    private static bool Is(List<string> args, int index, params string[] values)
    {
        if (args == null || index >= args.Count) return false;
        string v = args[index];
        foreach (var candidate in values)
        {
            if (string.Equals(v, candidate, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }
}

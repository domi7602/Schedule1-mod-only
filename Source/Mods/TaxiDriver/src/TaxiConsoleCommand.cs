using System;
using System.Collections.Generic;
using System.Globalization;
using S1API.Console;

namespace TaxiDriver;

/// <summary>
/// In-game console entry point: <c>taxi ...</c> (a leading <c>/</c> works as well —
/// S1API's registry strips it before the lookup).
///
/// Registration is automatic: S1API's <c>ConsolePatches.AddCommands</c> reflects over
/// every loaded assembly that references S1API and instantiates each non-abstract
/// <see cref="BaseConsoleCommand"/> with a public parameterless constructor.
/// </summary>
public sealed class TaxiConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "taxi";

    public override string CommandDescription =>
        "TaxiDriver spike (Stage 1 drive + Stage 2 visual swap + Stage 3 NPC driver ride + Stage 3b taxi stand / call-taxi): help, codes, spawn, npc, ride, out, go, go2, stop, status, cleanup, probe, trace, visual, lots, stand";

    public override string ExampleUsage => "taxi go 40";

    public override void ExecuteCommand(List<string> args)
    {
        string sub = args != null && args.Count > 0 ? args[0] : "help";
        try
        {
            if (args == null || args.Count == 0 || Is(args, 0, "help", "h", "?"))
            {
                SpikeCommands.PrintHelp();
                return;
            }

            switch (sub.ToLowerInvariant())
            {
                case "codes":
                case "list":
                    SpikeCommands.Codes();
                    break;

                case "spawn":
                    SpikeCommands.Spawn(args.Count >= 2 ? args[1] : null);
                    break;

                case "npc":
                case "driver":
                    SpikeCommands.Npc();
                    break;

                case "ride":
                case "in":
                    SpikeCommands.Ride();
                    break;

                case "out":
                case "exit":
                    SpikeCommands.Out();
                    break;

                case "go":
                    SpikeCommands.Go(ReadDistance(args, 40f));
                    break;

                case "go2":
                    SpikeCommands.Go(ReadDistance(args, 40f), useSettings: false);
                    break;

                case "stop":
                    SpikeCommands.Stop();
                    break;

                case "status":
                    SpikeCommands.Status();
                    break;

                case "cleanup":
                case "reset":
                    SpikeCommands.Cleanup();
                    break;

                case "probe":
                    SpikeCommands.Probe();
                    break;

                case "trace":
                    SpikeCommands.Trace(args.Count >= 2 ? args[1] : null);
                    break;

                case "lots":
                    SpikeCommands.Lots();
                    break;

                case "stand":
                    SpikeCommands.PrepareStand("taxi stand");
                    break;

                case "visual":
                    SpikeCommands.Visual(args.Count >= 2 ? args[1] : null, args.Count >= 3 ? args[2] : null);
                    break;

                default:
                    SpikeCommands.Print($"Unknown subcommand '{sub}' — showing the command list.");
                    SpikeCommands.PrintHelp();
                    break;
            }
        }
        catch (Exception ex)
        {
            // S1API logs a warning and lets the vanilla console handle the miss —
            // always name the failing step ourselves so the log stays actionable.
            Mod.Log.Error($"console subcommand 'taxi {sub}' failed: {ex.Message}");
        }
    }

    /// <summary>Reads an optional float argument, defaulting when absent/unparsable.</summary>
    private static float ReadDistance(List<string> args, float fallback)
    {
        if (args == null || args.Count < 2)
            return fallback;

        if (float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed) && parsed > 0f)
            return parsed;

        SpikeCommands.Print($"Could not parse '{args[1]}' as a distance — using {fallback:F0} m.");
        return fallback;
    }

    private static bool Is(List<string> args, int index, params string[] values)
    {
        if (args == null || index >= args.Count)
            return false;

        string value = args[index];
        foreach (string candidate in values)
        {
            if (string.Equals(value, candidate, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}

/// <summary>
/// Same handler under the slash-prefixed word. Whether the native console keeps the
/// leading <c>/</c> before the registry lookup is version dependent (S1API 3.2.0
/// source strips it, the deployed 3.2.1-beta.5 was never observed either way), so —
/// like HitmanPhone — both spellings are registered and both resolve.
/// Auto-discovered by <c>ConsolePatches.AddCommands</c> like every other public
/// <see cref="BaseConsoleCommand"/> with a parameterless constructor.
/// </summary>
public sealed class TaxiSlashCommand : BaseConsoleCommand
{
    private readonly TaxiConsoleCommand _inner = new();

    public override string CommandWord => "/taxi";

    public override string CommandDescription => _inner.CommandDescription;

    public override string ExampleUsage => _inner.ExampleUsage;

    public override void ExecuteCommand(List<string> args) => _inner.ExecuteCommand(args);
}

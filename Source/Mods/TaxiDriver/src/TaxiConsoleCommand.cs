using System;
using System.Collections.Generic;

using S1API.Console;

namespace TaxiDriver;

/// <summary>
/// Read-only diagnostic console entry point: <c>taxi ...</c> (a leading <c>/</c>
/// works as well — S1API's registry strips it before the lookup). Service actions
/// are deliberately left to the TaxiApp.
///
/// Registration is automatic: S1API's <c>ConsolePatches.AddCommands</c> reflects over
/// every loaded assembly that references S1API and instantiates each non-abstract
/// <see cref="BaseConsoleCommand"/> with a public parameterless constructor.
/// </summary>
public sealed class TaxiConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "taxi";

    public override string CommandDescription =>
        "TaxiDriver read-only diagnostics: help, codes, status, diag, probe, trace, lots, pois, fare, ai";

    public override string ExampleUsage => "taxi status";

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

            if (!TaxiConsolePolicy.IsReadOnlyDiagnostic(sub))
            {
                SpikeCommands.Print("Taxi service actions are phone-app-only; no console vehicle, ride, destination or cleanup action was run.");
                return;
            }

            switch (sub.ToLowerInvariant())
            {
                case "codes":
                case "list":
                    SpikeCommands.Codes();
                    break;
                case "status":
                    SpikeCommands.Status();
                    break;
                case "diag":
                    SpikeCommands.Diag();
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
                case "pois":
                case "places":
                case "destinations":
                    SpikeCommands.Pois();
                    break;
                case "fare":
                case "meter":
                    FareMeter.PrintStatus();
                    break;
                case "ai":
                case "ki":
                    TaxiAI.DumpConstants("taxi ai");
                    break;
                default:
                    SpikeCommands.PrintHelp();
                    break;
            }
        }
        catch (Exception ex)
        {
            // S1API logs a warning and lets the vanilla console handle the miss —
            // always name the failing step ourselves so the log stays actionable.
            Mod.Log.Error($"console diagnostic 'taxi {sub}' failed: {ex.Message}");
        }
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
/// Explicit slash-prefixed alias. Both spellings route to the same read-only
/// diagnostic handler, so command normalization does not change the policy.
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

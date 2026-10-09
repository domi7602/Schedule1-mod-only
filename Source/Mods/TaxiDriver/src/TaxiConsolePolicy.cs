using System;
using System.Collections.Generic;

namespace TaxiDriver;

/// <summary>Allow-list for console diagnostics; service actions belong to TaxiApp.</summary>
internal static class TaxiConsolePolicy
{
    private static readonly HashSet<string> ReadOnlyCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "help", "h", "?", "codes", "list", "status", "diag", "probe", "trace",
        "lots", "pois", "places", "destinations", "fare", "meter", "ai", "ki",
    };

    internal static bool IsReadOnlyDiagnostic(string? command) =>
        command != null && ReadOnlyCommands.Contains(command.Trim());
}

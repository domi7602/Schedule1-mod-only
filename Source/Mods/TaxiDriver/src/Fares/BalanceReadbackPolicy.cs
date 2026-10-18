using System;
using System.Globalization;

namespace TaxiDriver;

/// <summary>
/// Formats the observed balance movement around one settlement attempt. The
/// readback is log evidence only: it compares net worth before and after the
/// money calls and never proves causation (other game systems can move money
/// concurrently).
/// </summary>
internal static class BalanceReadbackPolicy
{
    /// <summary>Whole-dollar fares, plus float noise from S1API reads.</summary>
    internal const float ToleranceDollars = 0.51f;

    /// <summary>Describes before/after net worth and the observed delta.</summary>
    internal static string Describe(float netWorthBefore, float netWorthAfter, int chargedDollars)
    {
        float observed = netWorthBefore - netWorthAfter;
        string verdict = Math.Abs(observed - chargedDollars) <= ToleranceDollars
            ? "movement matches the charged amount"
            : "movement does not match the charged amount (concurrent money changes possible; verify manually)";
        return $"net worth {Num(netWorthBefore)} -> {Num(netWorthAfter)} (observed {Num(observed)}, charged ${chargedDollars}) — {verdict}";
    }

    private static string Num(float value) => value.ToString("0.##", CultureInfo.InvariantCulture);
}

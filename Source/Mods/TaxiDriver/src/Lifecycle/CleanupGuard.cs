using System;

namespace TaxiDriver;

/// <summary>
/// Cleanup-safety helper (Cleanup package, 2026-10-03): runs ordered cleanup
/// steps so a throwing step can never abort the steps after it (review rule:
/// "Fehler protokollieren, aber UnlockTrunk und StopDriving weiterhin
/// versuchen"). The step order stays the caller's; every failure is reported
/// through the injected handler and swallowed. Unity-free so the contract is
/// unit-tested (CleanupGuardTests).
/// </summary>
internal static class CleanupGuard
{
    /// <summary>
    /// Runs <paramref name="step"/>; an exception is reported via
    /// <paramref name="onFailure"/> (label + exception) and swallowed so the
    /// remaining steps still run. Returns true when the step completed.
    /// </summary>
    internal static bool Run(string label, Action step, Action<string, Exception> onFailure)
    {
        try
        {
            step();
            return true;
        }
        catch (Exception ex)
        {
            onFailure(label, ex);
            return false;
        }
    }
}

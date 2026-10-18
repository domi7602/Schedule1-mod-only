using System;

namespace TaxiDriver;

/// <summary>Confirmed-death cleanup ordering; no native vehicle access or payment retry.</summary>
internal static class DeadTaxiCleanup
{
    internal static bool Run(Action invalidate, Action stopMeter, Action unlockTrunk,
        Action stopPatrol, Action clearRuntime, Action<string, Exception> report)
    {
        // Reporting is also best-effort: a logging failure must not skip teardown.
        void Report(string label, Exception ex)
        {
            try { report(label, ex); }
            catch { /* preserve cleanup continuation */ }
        }

        bool complete = CleanupGuard.Run("invalidate navigation", invalidate, Report);
        complete &= CleanupGuard.Run("FareMeter.Stop", stopMeter, Report);
        complete &= CleanupGuard.Run("RideLocks.UnlockTrunk", unlockTrunk, Report);
        complete &= CleanupGuard.Run("TaxiAI.StopPatrol", stopPatrol, Report);
        complete &= CleanupGuard.Run("clear dead-taxi runtime", clearRuntime, Report);
        return complete;
    }
}

namespace TaxiDriver;

/// <summary>
/// Terminal-rescue decision: after the operation recovery budget is spent, one
/// guarded relocation may still be attempted before the ride is given up.
/// </summary>
/// <remarks>
/// 2026-10-15 gameplay log: the old wiring gave up whenever the taxi was still
/// on the vehicle graph ("no teleport needed, giving up") and abandoned a ride
/// 106.7 m from its destination while the car sat stuck at (40.1, 0.6, 52.5).
/// Graph membership only says a route exists — not that the car can reach it.
/// The relocation itself stays guarded (RoadKeeper.TryRescueStartup: supported
/// free spot, ground snap, oriented-box clearance, occupancy check).
/// </remarks>
internal static class TerminalRescuePolicy
{
    internal enum Action
    {
        GiveUp,
        AttemptGuardedRelocation,
    }

    /// <summary>
    /// Decides whether the terminal rescue attempts the guarded relocation.
    /// The graph state never vetoes it; only a spent one-shot rescue budget does.
    /// </summary>
    internal static Action Decide(bool onGraph, bool lastRescueAvailable)
    {
        _ = onGraph; // documented no-veto parameter, see remarks
        return lastRescueAvailable ? Action.AttemptGuardedRelocation : Action.GiveUp;
    }
}

namespace TaxiDriver;

/// <summary>Order-bound start ownership; ordinary stalls must not preempt the fixed self-test.</summary>
internal static class PatrolStartPolicy
{
    internal static bool ShouldWait(bool active, int selfTestOrder, int currentOrder,
        bool driveStartConfirmed, float elapsed, float selfTestSeconds) =>
        active && selfTestOrder == currentOrder && !driveStartConfirmed && elapsed < selfTestSeconds;
}

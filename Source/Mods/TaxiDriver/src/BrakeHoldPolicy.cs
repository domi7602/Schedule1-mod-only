namespace TaxiDriver;

/// <summary>Park intent requests native handbrake on every consumption.
/// Previous HandbrakeApplied is an output, never proof the next request is held.</summary>
internal static class BrakeHoldPolicy
{
    internal static bool ShouldRequestPark(bool activeOrder, bool playerDriver, bool parkIntent)
        => !activeOrder && !playerDriver && parkIntent;
}

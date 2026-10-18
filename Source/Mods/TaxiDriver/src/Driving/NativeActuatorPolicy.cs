namespace TaxiDriver;

internal enum NativeActuatorMode { None, Reverse, Park }

/// <summary>Pure selection at the consumer; ownership is the exact live tracked
/// pointer, not vehicle type. A pending supervised window takes precedence over park.</summary>
internal static class NativeActuatorPolicy
{
    internal static NativeActuatorMode Choose(bool host, bool exactOwnedVehicle,
        bool paused, bool playerDriver, bool reverseWindow, bool activeOrder, bool parkIntent)
    {
        if (!host || !exactOwnedVehicle || paused || playerDriver)
            return NativeActuatorMode.None;
        if (reverseWindow) return NativeActuatorMode.Reverse;
        return BrakeHoldPolicy.ShouldRequestPark(activeOrder, playerDriver, parkIntent)
            ? NativeActuatorMode.Park : NativeActuatorMode.None;
    }
}

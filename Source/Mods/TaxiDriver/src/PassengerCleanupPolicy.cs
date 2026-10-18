using System;
namespace TaxiDriver;

internal enum PlayerOccupancy { Unknown, Occupied, Empty }

/// <summary>Only a fresh, complete empty-player observation may consume the existing ownership budget.</summary>
internal static class PassengerCleanupPolicy
{
    // Called only after seats/registered players and independent NPC teardown are
    // verified. The native aggregate bit may lag NPC exit; do not label it remote.
    internal static PlayerOccupancy FinalizeEmptyPlayerProof(bool localInside, bool aggregateOccupied) =>
        localInside || aggregateOccupied ? PlayerOccupancy.Unknown : PlayerOccupancy.Empty;

    internal static PlayerOccupancy ObserveCurrentVehicle(bool readable, bool present,
        IntPtr currentPointer, IntPtr targetNetworkPointer)
    {
        if (!readable || targetNetworkPointer == IntPtr.Zero || (present && currentPointer == IntPtr.Zero))
            return PlayerOccupancy.Unknown;
        return present && currentPointer == targetNetworkPointer ? PlayerOccupancy.Occupied : PlayerOccupancy.Empty;
    }

    internal static bool TryBeginDestroy(VehicleOwnershipLedger ownership, IntPtr pointer,
        bool alive, bool explicitRetry, PlayerOccupancy occupancy) =>
        occupancy == PlayerOccupancy.Empty && ownership.TryBeginDestroy(pointer, alive, explicitRetry);
}

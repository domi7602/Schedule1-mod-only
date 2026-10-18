using System;
namespace TaxiDriver;

/// <summary>Player-only seat observation. Native null and native player identity are distinct from an unreadable proxy.</summary>
internal readonly record struct PassengerSeatSnapshot(
    bool Driver, bool Readable, bool OccupantPresent, IntPtr OccupantPointer, bool IsOccupied);

internal static class PassengerSeatPolicy
{
    internal static bool CanReserveDriverSeat(bool occupantPresent, bool isOccupied) =>
        !occupantPresent && !isOccupied;

    internal static bool CanUse(PassengerSeatSnapshot seat, IntPtr localPointer) =>
        localPointer != IntPtr.Zero && seat.Readable && !seat.Driver &&
        (seat.OccupantPresent
            ? seat.OccupantPointer != IntPtr.Zero && seat.OccupantPointer == localPointer
            : !seat.IsOccupied);

    internal static int Select(int count, Func<int, PassengerSeatSnapshot> read, IntPtr localPointer)
    {
        int vacant = -1;
        for (int i = 0; i < count; i++)
        {
            PassengerSeatSnapshot seat = read(i);
            if (!CanUse(seat, localPointer)) continue;
            if (seat.OccupantPresent) return i;
            if (vacant < 0) vacant = i;
        }
        return vacant;
    }
}

using System;

namespace PoliceTestMode;

internal enum VehicleOccupancy { Unknown, Clear, Player }
internal static class SessionPolicy
{
    public static bool ShouldSuppress(bool requested, bool hooksReady, bool worldReady, bool singleplayer)
        => requested && hooksReady && worldReady && singleplayer;

    // null means unreadable, not vacant. NPC occupancy is deliberately not an input.
    public static VehicleOccupancy ReadVehicleOccupancy(Func<int> seatCount, Func<int, bool?> playerPresent)
    {
        try
        {
            int count = seatCount();
            if (count <= 0 || count > 512) return VehicleOccupancy.Unknown;
            for (int i = 0; i < count; i++)
            {
                bool? present = playerPresent(i);
                if (!present.HasValue) return VehicleOccupancy.Unknown;
                if (present.Value) return VehicleOccupancy.Player;
            }
            return VehicleOccupancy.Clear;
        }
        catch { return VehicleOccupancy.Unknown; }
    }
}

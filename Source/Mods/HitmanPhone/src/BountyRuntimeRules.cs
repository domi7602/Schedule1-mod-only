using System;

namespace HitmanPhone;

/// <summary>Game-independent decisions at the native clock and pursuit boundaries.</summary>
public static class BountyRuntimeRules
{
    public static int AcceptedPursuit(int current, bool awaitingDrop) => Math.Max(current, awaitingDrop ? 4 : 1);
    public static bool TryTimeSnapshot(Func<int> readDay, Func<int> readMinute, out int day, out int minute, out long sum)
    {
        day = minute = -1;
        sum = -1;
        try
        {
            int before = readDay();
            int clock = readMinute();
            int after = readDay();
            if (before < 0 || before != after || clock < 0 || clock >= 1440) return false;
            day = before;
            minute = clock;
            sum = day * 1440L + minute;
            return true;
        }
        catch { return false; }
    }
}

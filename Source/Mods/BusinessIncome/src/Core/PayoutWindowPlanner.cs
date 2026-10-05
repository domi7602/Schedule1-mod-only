using System;

namespace BusinessIncome.Core;

/// <summary>
/// Centralized, pure payout-window planner. Fails closed: an unknown current time for the
/// CURRENT day keeps the day out of the window (never pay on a bad time read). Past days
/// (backlog) bypass the window. Hour math is overflow-safe because the hour is clamped.
/// </summary>
public static class PayoutWindowPlanner
{
    public static WindowDecision Plan(int payoutHour, int currentTime24h, bool timeKnown, bool isPastDay)
    {
        if (isPastDay)
            return new WindowDecision(true, "past-day backlog bypasses the window");

        if (payoutHour == 0)
            return new WindowDecision(true, "midnight/day-pass window");

        if (payoutHour < 0 || payoutHour > 23)
            return new WindowDecision(false, "fail-closed: invalid payout hour");

        if (!timeKnown)
            return new WindowDecision(false, "fail-closed: time probe failed");

        int start = payoutHour * 100; // <= 2300, no overflow
        int end = start + 59;
        bool inWindow = currentTime24h >= start && currentTime24h <= end;
        return inWindow
            ? new WindowDecision(true, $"in window {start:D4}-{end:D4}")
            : new WindowDecision(false, $"outside window {start:D4}-{end:D4}");
    }
}

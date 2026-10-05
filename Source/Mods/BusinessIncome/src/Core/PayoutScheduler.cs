using System;
using System.Collections.Generic;

namespace BusinessIncome.Core;

/// <summary>
/// Pure catch-up planner: produces the candidate days oldest-to-newest and stops the
/// enumeration at the first day that is not durable (the executor must also stop there).
/// Loop size is bounded by the cap and all arithmetic is done in long to be overflow-safe.
/// </summary>
public static class PayoutScheduler
{
    public const int MinCatchupDays = 1;
    public const int MaxCatchupDays = 365;

    public static CatchupPlan PlanCatchup(
        long lastPaidDay,
        long currentDay,
        int maxCatchupDays,
        bool hasBusinesses,
        bool stateDurable)
    {
        if (!stateDurable)
            return new CatchupPlan { Days = Array.Empty<int>(), StoppedEarly = true, Reason = "state not durable" };

        if (lastPaidDay < -1)
            return new CatchupPlan { Days = Array.Empty<int>(), StoppedEarly = true, Reason = "invalid last-paid day" };

        int cap = ClampCap(maxCatchupDays);

        if (currentDay <= lastPaidDay)
            return new CatchupPlan { Days = Array.Empty<int>(), Reason = "nothing to catch up" };

        long windowStart = currentDay - cap + 1;
        if (windowStart < lastPaidDay + 1) windowStart = lastPaidDay + 1;
        if (windowStart < 1) windowStart = 1;

        var days = new List<int>();
        for (long d = windowStart; d <= currentDay; d++)
        {
            if (d > int.MaxValue) break; // overflow-safe guard
            days.Add((int)d);
            if (days.Count > MaxCatchupDays) break; // hard loop cap
        }

        bool capped = (currentDay - lastPaidDay) > cap;

        // "zero/no business terminal if persisted": a saved day with no businesses is
        // terminal. It must STILL return the days so the executor can durably commit them —
        // otherwise the backlog survives and later pays as a windfall once a business is
        // bought. Same oldest-to-newest, capped enumeration as a normal backlog.
        if (!hasBusinesses)
            return new CatchupPlan
            {
                Days = days,
                StoppedEarly = capped,
                // These are executable terminal commits, not an instruction to skip the pass.
                TerminalSkip = false,
                Reason = capped
                    ? $"no owned businesses — terminal days {days[0]}..{days[^1]} (cap {cap})"
                    : "no owned businesses — days treated as terminal"
            };

        return new CatchupPlan
        {
            Days = days,
            StoppedEarly = capped,
            Reason = capped
                ? $"backlog exceeds cap {cap}; paying only the last {days.Count} day(s)"
                : "full backlog within cap"
        };
    }

    public static int ClampCap(int maxCatchupDays)
    {
        if (maxCatchupDays < MinCatchupDays) return MinCatchupDays;
        if (maxCatchupDays > MaxCatchupDays) return MaxCatchupDays;
        return maxCatchupDays;
    }
}

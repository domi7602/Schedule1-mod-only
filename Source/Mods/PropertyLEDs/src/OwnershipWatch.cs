using System;
using System.Collections.Generic;

namespace PropertyLEDs.Core;

/// <summary>
/// Decides when a bounded ownership probe may run. A partial list is not complete:
/// the starter RV can be present before a purchased apartment appears.
/// </summary>
public static class OwnershipWatch
{
    public const int MaxAttempts = 30;
    public const double IntervalSeconds = 1.0;

    /// <summary>
    /// True when another owned-list read is due. <paramref name="knownCount"/> does not
    /// end the window: one early property must not hide a later one.
    /// </summary>
    public static bool ShouldProbe(int attempts, int knownCount, double now, double nextProbeAt)
    {
        _ = knownCount;
        return attempts < MaxAttempts && now >= nextProbeAt;
    }

    /// <summary>Order-insensitive identity change. An unchanged set must not rebuild lights.</summary>
    public static bool IdsChanged(IReadOnlyList<string>? before, IReadOnlyList<string>? after)
    {
        var left = Normalize(before);
        var right = Normalize(after);
        if (left.Count != right.Count) return true;
        foreach (string id in left)
            if (!right.Contains(id)) return true;
        return false;
    }

    /// <summary>
    /// An empty follow-up read is not proof the player owns nothing. Slot 1 on 2026-11-12
    /// had two properties and two LED groups, then a one-second-later read of zero wiped them.
    /// </summary>
    public static bool ShouldCommit(IReadOnlyList<string>? before, IReadOnlyList<string>? after)
    {
        if (Normalize(before).Count > 0 && Normalize(after).Count == 0) return false;
        return IdsChanged(before, after);
    }

    private static HashSet<string> Normalize(IReadOnlyList<string>? ids)
    {
        var set = new HashSet<string>(StringComparer.Ordinal);
        if (ids == null) return set;
        for (int i = 0; i < ids.Count; i++)
            if (!string.IsNullOrWhiteSpace(ids[i])) set.Add(ids[i]);
        return set;
    }
}

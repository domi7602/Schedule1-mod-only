using System;
using System.Collections.Generic;

namespace LightControl.Core;

/// <summary>
/// Unity-free helper that removes duplicate and already-owned native light pointers from a
/// candidate list, so a Unity <c>Light</c> shared by two grow lamps (or listed twice) is never
/// scaled twice. The adapter passes the native pointers; this type holds no Unity reference.
/// </summary>
public static class LightPointerDedupe
{
    /// <summary>
    /// Indices of the candidate pointers to keep: the first occurrence of every distinct nonzero
    /// pointer that is not already owned by another active lamp, in original order.
    /// </summary>
    public static IReadOnlyList<int> SelectUnowned(IReadOnlyList<long> candidatePointers, IReadOnlyCollection<long>? alreadyOwned)
    {
        if (candidatePointers is null) throw new ArgumentNullException(nameof(candidatePointers));
        var seen = new HashSet<long>();
        var keep = new List<int>(candidatePointers.Count);
        for (int index = 0; index < candidatePointers.Count; index++)
        {
            long pointer = candidatePointers[index];
            if (pointer == 0) continue;
            if (alreadyOwned is not null && alreadyOwned.Contains(pointer)) continue;
            if (seen.Add(pointer)) keep.Add(index);
        }
        return keep;
    }
}

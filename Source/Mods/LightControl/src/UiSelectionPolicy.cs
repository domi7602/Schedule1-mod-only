namespace LightControl.Core;

/// <summary>
/// Unity-free navigation for the phone app: the color and intensity sets are the single source of
/// truth for cycling, and the original action resets to the model default. No UI dependency.
/// </summary>
public static class UiSelectionPolicy
{
    public const int ColorCount = 11;
    public const int IntensityCount = 7;

    private static readonly IntensityLevel[] Intensities =
    {
        IntensityLevel.Percent10, IntensityLevel.Percent25, IntensityLevel.Percent50, IntensityLevel.Percent75,
        IntensityLevel.Percent100, IntensityLevel.Percent150, IntensityLevel.Percent200
    };

    /// <summary>Normalizes any index into the half-open range [0, count).</summary>
    public static int WrapIndex(int index, int count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        return ((index % count) + count) % count;
    }

    public static ColorMode NextColor(ColorMode color, int delta = 1)
    {
        LightLimits.EnumValue(color, nameof(color));
        return (ColorMode)WrapIndex((int)color + delta, ColorCount);
    }

    public static IntensityLevel IntensityAt(int index) => Intensities[WrapIndex(index, IntensityCount)];

    public static IntensityLevel NextIntensity(IntensityLevel level, int delta = 1)
    {
        int index = Array.IndexOf(Intensities, level);
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(level));
        return IntensityAt(index + delta);
    }

    public static LampBaseState OriginalReset() => new(ColorMode.Original, IntensityLevel.Percent100);

    public enum LampListFilter { All = 0, Tents = 1, Lamps = 2 }

    /// <summary>A row is visible when it matches the kind chip and, if set, the property chip.</summary>
    public static bool Matches(string? kind, string? propertyCode, LampListFilter filter, string? propertyFilter)
    {
        if (!string.IsNullOrWhiteSpace(propertyFilter)
            && !string.Equals(propertyCode?.Trim(), propertyFilter.Trim(), StringComparison.OrdinalIgnoreCase))
            return false;
        bool tent = string.Equals(kind?.Trim(), "Grow tent", StringComparison.OrdinalIgnoreCase);
        return filter switch
        {
            LampListFilter.Tents => tent,
            LampListFilter.Lamps => !tent,
            _ => true
        };
    }

    /// <summary>Keeps the current lamp when it is still in the filtered list, otherwise the first visible one.</summary>
    public static int KeepOrFirst(int selected, IReadOnlyList<int> visible)
    {
        if (visible == null || visible.Count == 0) return -1;
        for (int i = 0; i < visible.Count; i++)
            if (visible[i] == selected) return selected;
        return visible[0];
    }

    public readonly struct PropertyGroup
    {
        public PropertyGroup(string name, int start, int count)
        {
            Name = name;
            Start = start;
            Count = count;
        }

        public string Name { get; }
        public int Start { get; }
        public int Count { get; }
    }

    /// <summary>
    /// Groups lamps under the exact property code they were first seen with. Display names never merge
    /// two codes, and case is part of the code. Blank names fall back to the code, then "Unknown".
    /// </summary>
    public static void GroupByProperty(
        IReadOnlyList<int> indices,
        IReadOnlyList<string?> codes,
        IReadOnlyList<string?> names,
        List<int> ordered,
        List<PropertyGroup> groups)
    {
        if (ordered == null) throw new ArgumentNullException(nameof(ordered));
        if (groups == null) throw new ArgumentNullException(nameof(groups));
        ordered.Clear();
        groups.Clear();
        if (indices == null || indices.Count == 0) return;

        var seen = new List<string>();
        var display = new Dictionary<string, string>(StringComparer.Ordinal);
        var buckets = new Dictionary<string, List<int>>(StringComparer.Ordinal);
        for (int i = 0; i < indices.Count; i++)
        {
            string code = i < codes.Count && !string.IsNullOrWhiteSpace(codes[i]) ? codes[i]!.Trim() : string.Empty;
            string key = code.Length == 0 ? "\0unknown" : code;
            if (!buckets.TryGetValue(key, out var bucket))
            {
                bucket = new List<int>();
                buckets[key] = bucket;
                seen.Add(key);
                display[key] = code.Length == 0 ? "Unknown" : code;
            }
            if (i < names.Count && !string.IsNullOrWhiteSpace(names[i]))
            {
                string name = names[i]!.Trim();
                if (display[key] == "Unknown" || string.Equals(display[key], code, StringComparison.OrdinalIgnoreCase))
                    display[key] = name;
            }
            bucket.Add(indices[i]);
        }

        for (int i = 0; i < seen.Count; i++)
        {
            var bucket = buckets[seen[i]];
            groups.Add(new PropertyGroup(display[seen[i]], ordered.Count, bucket.Count));
            ordered.AddRange(bucket);
        }
    }

    /// <summary>
    /// Keeps a selected lamp key across inserts and removals of other lamps. A missing key is kept so a
    /// temporarily unloaded lamp is not replaced. Permanent removal clears it. A first open may take the first key.
    /// </summary>
    public static string? ResolveSelection(string? selectedKey, IReadOnlyList<string> visibleKeys, bool permanentlyRemoved)
    {
        if (permanentlyRemoved) return null;
        if (!string.IsNullOrEmpty(selectedKey)) return selectedKey;
        return visibleKeys != null && visibleKeys.Count > 0 ? visibleKeys[0] : null;
    }

    public static bool SelectionVisible(string? selectedKey, IReadOnlyList<string> visibleKeys)
    {
        if (string.IsNullOrEmpty(selectedKey) || visibleKeys == null) return false;
        for (int i = 0; i < visibleKeys.Count; i++)
            if (string.Equals(visibleKeys[i], selectedKey, StringComparison.Ordinal)) return true;
        return false;
    }

    /// <summary>Same lamp identities, ignoring order. Equal counts with different keys do not match.</summary>
    public static bool SameKeySet(IReadOnlyList<string>? left, IReadOnlyList<string>? right)
    {
        if (left == null || right == null || left.Count != right.Count) return false;
        var a = new string[left.Count];
        var b = new string[right.Count];
        for (int i = 0; i < left.Count; i++)
        {
            a[i] = left[i] ?? string.Empty;
            b[i] = right[i] ?? string.Empty;
        }
        Array.Sort(a, StringComparer.Ordinal);
        Array.Sort(b, StringComparer.Ordinal);
        for (int i = 0; i < a.Length; i++)
            if (!string.Equals(a[i], b[i], StringComparison.Ordinal)) return false;
        return true;
    }

    public static bool PreviewMatches(
        Guid shownSession, string? shownSource, string? shownProperty, ColorMode shownColor, IntensityLevel shownIntensity, IReadOnlyList<string> shownKeys,
        Guid currentSession, string? currentSource, string? currentProperty, ColorMode currentColor, IntensityLevel currentIntensity, IReadOnlyList<string> currentKeys) =>
        shownSession == currentSession
        && string.Equals(shownSource, currentSource, StringComparison.Ordinal)
        && string.Equals(shownProperty, currentProperty, StringComparison.Ordinal)
        && shownColor == currentColor
        && shownIntensity == currentIntensity
        && SameKeySet(shownKeys, currentKeys);

    public const double ActionMessageSeconds = 4;

    public static bool ActionMessageVisible(double postedAt, double now) =>
        double.IsFinite(postedAt) && double.IsFinite(now) && now >= postedAt && now - postedAt < ActionMessageSeconds;

    public enum SaveNotice { NoSession, Blocked, WriteFailed, Pending, Saved, Loaded }

    public static SaveNotice ClassifySave(bool hasSession, bool blocked, bool lastWriteFailed, long revision, long savedRevision, bool loaded)
    {
        if (!hasSession) return SaveNotice.NoSession;
        if (blocked) return SaveNotice.Blocked;
        if (lastWriteFailed && revision != savedRevision) return SaveNotice.WriteFailed;
        if (revision != savedRevision) return SaveNotice.Pending;
        return loaded ? SaveNotice.Loaded : SaveNotice.Saved;
    }

    public static string SaveNoticeText(SaveNotice notice) => notice switch
    {
        SaveNotice.NoSession => "Waiting for a loaded save.",
        SaveNotice.Blocked => "Changes are not saved this session.",
        SaveNotice.WriteFailed => "Save failed. Retrying.",
        SaveNotice.Pending => "Save pending.",
        SaveNotice.Saved => "Saved.",
        _ => "Loaded. No unsaved changes."
    };
}

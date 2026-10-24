namespace LightControl.Core;

/// <summary>
/// Unity-free navigation for the phone app: the color and intensity sets are the single source of
/// truth for cycling, and the original action resets to the model default. No UI dependency.
/// </summary>
public static class UiSelectionPolicy
{
    public const int ColorCount = 11;
    public const int IntensityCount = 7;
    public const int PickerColorCount = 10;
    public const float GraphiteReferenceWidth = 1201f;
    public const float GraphiteReferenceHeight = 655f;

    /// <summary>Owns one widget subscription, including the callback identity kept by S1API.</summary>
    public sealed class ClickBinding
    {
        private Action? _target;

        public ClickBinding(Action target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            // Distinct binding targets defeat EventHelper's global delegate-equality deduplication.
            Callback = Invoke;
        }

        public Action Callback { get; }
        private void Invoke() => _target?.Invoke();
        public void Release() => _target = null;
    }

    /// <summary>The single heading scrolls past the first property band, then sticks to the viewport.</summary>
    public static float ColumnHeadingTop(float scrollOffset) =>
        Math.Max(0f, 52f - (float.IsFinite(scrollOffset) ? Math.Max(0f, scrollOffset) : 0f));

    /// <summary>Uniform fit of the approved reference canvas inside this app's measured container.</summary>
    public static float ReferenceCanvasScale(float width, float height)
    {
        if (!float.IsFinite(width) || !float.IsFinite(height) || width <= 0f || height <= 0f) return 0f;
        return Math.Min(width / GraphiteReferenceWidth, height / GraphiteReferenceHeight);
    }

    /// <summary>The ten explicit palette choices, in five-column reading order. Original is an action.</summary>
    public static ColorMode PickerColorAt(int index)
    {
        if (index < 0 || index >= PickerColorCount) throw new ArgumentOutOfRangeException(nameof(index));
        return (ColorMode)((int)ColorMode.White + index);
    }

    public static bool IsPickerColor(ColorMode color) => IsFixedColor(color) || color == ColorMode.Rainbow;

    /// <summary>The displayed brightness bar uses the documented 200-percent maximum.</summary>
    public static float BrightnessFill(IntensityLevel level)
    {
        LightLimits.EnumValue(level, nameof(level));
        return (int)level / 200f;
    }

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

    // LC08-LC10 UI decisions. These are pure so the phone app has one tested place for the
    // selection, mode, copy wording, status and responsive-width rules. The app only paints them.
    public enum EditorAvailability { None = 0, Editable = 1, Unavailable = 2 }

    /// <summary>
    /// A missing key is only a permanent removal once a completed scan no longer knows it. A
    /// temporarily unloaded lamp stays in the known set, and an unknown key during a scan may still
    /// be found, so both retain the selection.
    /// </summary>
    public static bool IsPermanentlyRemoved(string? selectedKey, bool known, bool scanInProgress) =>
        !string.IsNullOrEmpty(selectedKey) && !known && !scanInProgress;

    public static EditorAvailability ClassifyEditor(string? selectedKey, bool known, bool visible)
    {
        if (string.IsNullOrEmpty(selectedKey)) return EditorAvailability.None;
        return known && visible ? EditorAvailability.Editable : EditorAvailability.Unavailable;
    }

    /// <summary>The editor never acts on a lamp that is filtered out, unloaded or unknown.</summary>
    public static bool CanEditSelection(string? selectedKey, bool known, bool visible) =>
        ClassifyEditor(selectedKey, known, visible) == EditorAvailability.Editable;

    public enum LightMode { Original = 0, Fixed = 1, Rainbow = 2 }

    public static LightMode ModeOf(ColorMode color)
    {
        LightLimits.EnumValue(color, nameof(color));
        if (color == ColorMode.Original) return LightMode.Original;
        if (color == ColorMode.Rainbow) return LightMode.Rainbow;
        return LightMode.Fixed;
    }

    public static bool IsFixedColor(ColorMode color) =>
        color >= ColorMode.White && color <= ColorMode.Pink;

    /// <summary>
    /// Fixed mode keeps the current solid color, otherwise the last solid pick, otherwise white.
    /// Original and Rainbow are never remembered as the fixed color.
    /// </summary>
    public static ColorMode FixedColor(ColorMode current, ColorMode remembered)
    {
        if (IsFixedColor(current)) return current;
        return IsFixedColor(remembered) ? remembered : ColorMode.White;
    }

    public static string ColorName(ColorMode color) => color switch
    {
        ColorMode.Original => "Original",
        ColorMode.White => "White",
        ColorMode.Red => "Red",
        ColorMode.Orange => "Orange",
        ColorMode.Yellow => "Yellow",
        ColorMode.Green => "Green",
        ColorMode.Cyan => "Cyan",
        ColorMode.Blue => "Blue",
        ColorMode.Purple => "Purple",
        ColorMode.Pink => "Pink",
        ColorMode.Rainbow => "Rainbow",
        _ => throw new ArgumentOutOfRangeException(nameof(color))
    };

    public static string IntensityLabel(IntensityLevel level)
    {
        LightLimits.EnumValue(level, nameof(level));
        return $"{(int)level}%";
    }

    /// <summary>The copied property count always includes the selected source lamp.</summary>
    public static string PropertyCopyLabel(string? propertyName, int targetCount)
    {
        string name = string.IsNullOrWhiteSpace(propertyName) ? "this property" : propertyName!.Trim();
        return $"Copy to {name} ({Math.Max(0, targetCount)} incl. source)";
    }

    /// <summary>The world copy is explicit: source, source settings and the exact including-source count.</summary>
    public static string WorldCopyConfirmation(
        string sourceLabel, string? propertyName, ColorMode color, IntensityLevel intensity, int targetCount)
    {
        string source = string.IsNullOrWhiteSpace(sourceLabel) ? "the selected lamp" : sourceLabel.Trim();
        string property = string.IsNullOrWhiteSpace(propertyName) ? string.Empty : $" ({propertyName!.Trim()})";
        return $"Copy {ColorName(color)} at {IntensityLabel(intensity)} from {source}{property} to all {Math.Max(0, targetCount)} lamps, including the source?";
    }

    public static string EmptyCatalogText(bool scanInProgress) =>
        scanInProgress ? "Looking for grow lamps..." : "No supported grow lamps found.";

    /// <summary>Never report an unchanged or zero-change result as applied work.</summary>
    public static string ActionMessage(CommandStatus status, int appliedCount) => status switch
    {
        CommandStatus.Applied when appliedCount == 1 => "Applied to 1 lamp.",
        CommandStatus.Applied when appliedCount > 1 => $"Applied to {appliedCount} lamps.",
        CommandStatus.Applied => "No lamps changed.",
        CommandStatus.Unchanged => "Already set.",
        CommandStatus.StaleRevision => "The lamp list changed. That change was not applied; check the targets and try again.",
        _ => "That change was not applied."
    };

    /// <summary>
    /// The status keeps the actual runtime reason before a session exists. Once ready it keeps the
    /// scan state, the unsupported-lamp count and the persistence state as separate facts, even when
    /// the lamp list is empty. No part silently replaces another.
    /// </summary>
    public static string StatusLine(
        bool ready, string? reason, string? persistence, bool scanInProgress, bool scanTruncated, int lampCount, int unsupported,
        int materialConflicts = 0, int pendingRestores = 0)
    {
        if (!ready)
            return string.IsNullOrWhiteSpace(reason) ? "Waiting for a loaded save." : reason!.Trim();

        var parts = new List<string>();
        if (scanInProgress)
            parts.Add(lampCount == 0 ? "Scanning for grow lamps..." : $"Scanning... {lampCount} found so far.");
        else if (lampCount == 0)
            parts.Add("No supported grow lamps found.");
        else
            parts.Add(lampCount == 1 ? "1 light." : $"{lampCount} lights.");
        if (scanTruncated) parts.Add("Scan incomplete; some lights may be missing.");
        if (unsupported > 0) parts.Add($"{unsupported} on without a readable light source; left untouched.");
        if (materialConflicts > 0) parts.Add($"{materialConflicts} material conflicts; foreign surfaces kept.");
        if (pendingRestores > 0) parts.Add($"{pendingRestores} restores pending.");
        if (!string.IsNullOrWhiteSpace(persistence)) parts.Add(persistence!.Trim());
        return string.Join(" ", parts);
    }

    public const float CompactWidth = 360f;

    /// <summary>Below the compact width the list and editor become one pane with a Back action.</summary>
    public static bool UseCompactLayout(float width) =>
        float.IsFinite(width) && width > 0f && width < CompactWidth;

    /// <summary>
    /// Minimum usable modal card width in reference units. Below it the five-tile grid and the
    /// two-button footer stop fitting, so the approved fraction is not allowed to shrink further.
    /// </summary>
    public const float MinModalCardWidth = 300f;

    /// <summary>
    /// Width of one modal card: the approved fraction of the measured container, clamped so it
    /// never exceeds the space available on the phone and never collapses below the usable minimum.
    /// A container narrower than the minimum still wins, so the card can never overflow it.
    /// </summary>
    public static float ModalCardWidth(float rootWidth, float fraction, float minWidth, float availableWidth)
    {
        if (!float.IsFinite(rootWidth) || rootWidth <= 0f) return 0f;
        if (!float.IsFinite(fraction) || fraction <= 0f) return 0f;
        if (!float.IsFinite(availableWidth) || availableWidth <= 0f) return 0f;
        float preferred = rootWidth * fraction;
        float usable = float.IsFinite(minWidth) && minWidth > 0f ? Math.Min(minWidth, availableWidth) : 0f;
        return Math.Max(usable, Math.Min(availableWidth, preferred));
    }

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

    // ---- LC11 compact color picker: staged edit decisions (Unity-free) -------------------
    // These are the decisions behind the full-width list and the color/brightness modals. The
    // phone app only paints them and never applies a pending pick before an explicit SELECT.

    /// <summary>The result of an explicit SELECT on a staged edit.</summary>
    public enum PickerOutcome
    {
        Committed = 0,
        NotOpen = 1,
        NoPick = 2,
        WrongSession = 3,
        WrongSource = 4,
        Unavailable = 5,
        StaleSource = 6,
        AlreadyConsumed = 7
    }

    /// <summary>What a confirmed color pick must apply, bound to the exact lamp it was opened for.</summary>
    public readonly record struct ColorPickerCommit(Guid SessionId, string SourceKey, ColorMode Color);

    /// <summary>The result of validating one modal action against the live runtime at click time.</summary>
    public enum ActionGate
    {
        Allowed = 0,
        NoKey = 1,
        NotReady = 2,
        WrongSession = 3,
        UnknownKey = 4,
        NotVisible = 5
    }

    /// <summary>
    /// A modal action may only reach the runtime while the session that opened it is still the live
    /// session and its exact lamp is still known and visible. Session ids can change while a lamp
    /// keeps the same identity key, so the binding is session plus key, never the key alone.
    /// </summary>
    public static ActionGate ClassifyAction(
        Guid expectedSession, string? expectedKey, bool ready, Guid currentSession, bool known, bool visible)
    {
        if (string.IsNullOrWhiteSpace(expectedKey)) return ActionGate.NoKey;
        if (!ready) return ActionGate.NotReady;
        if (expectedSession == Guid.Empty || expectedSession != currentSession) return ActionGate.WrongSession;
        if (!known) return ActionGate.UnknownKey;
        if (!visible) return ActionGate.NotVisible;
        return ActionGate.Allowed;
    }

    /// <summary>Staged color edit for one lamp: nothing is applied until a validated SELECT.</summary>
    public sealed class ColorPickerState
    {
        private ColorMode _pending = ColorMode.Original;

        public bool IsOpen { get; private set; }
        public Guid SessionId { get; private set; }
        public string SourceKey { get; private set; } = string.Empty;
        public ColorMode SourceColor { get; private set; } = ColorMode.Original;
        public bool HasPendingColor { get; private set; }
        public bool Consumed { get; private set; }
        public bool Stale { get; private set; }

        /// <summary>The staged color. Only meaningful while <see cref="HasPendingColor"/> is true.</summary>
        public ColorMode PendingColor => _pending;

        /// <summary>
        /// Opens the picker for one exact lamp. Original opens without a staged choice; Rainbow
        /// highlights its own tenth tile. Opening never changes the lamp. An open picker cannot be
        /// retargeted by a late callback.
        /// </summary>
        public bool Open(Guid sessionId, string? sourceKey, ColorMode sourceColor)
        {
            if (IsOpen || sessionId == Guid.Empty || string.IsNullOrWhiteSpace(sourceKey)) return false;
            LightLimits.EnumValue(sourceColor, nameof(sourceColor));
            SessionId = sessionId;
            SourceKey = sourceKey!.Trim();
            SourceColor = sourceColor;
            Stale = false;
            Consumed = false;
            HasPendingColor = IsPickerColor(sourceColor);
            _pending = HasPendingColor ? sourceColor : ColorMode.Original;
            IsOpen = true;
            return true;
        }

        /// <summary>Stages one of the nine fixed colors or Rainbow, without applying it.</summary>
        public bool Choose(ColorMode color)
        {
            if (!IsOpen || !IsPickerColor(color)) return false;
            _pending = color;
            HasPendingColor = true;
            Stale = false;
            return true;
        }

        /// <summary>
        /// Validates the staged pick against the live session, the exact source lamp and its readable
        /// state. The claim is consumed before the caller may reach the runtime, so a repeated or
        /// throwing confirmation cannot apply the same pick twice.
        /// </summary>
        public PickerOutcome Confirm(
            Guid sessionId, string? sourceKey, bool known, bool visible, ColorMode sourceColor, out ColorPickerCommit commit)
        {
            commit = default;
            if (Consumed) return PickerOutcome.AlreadyConsumed;
            if (!IsOpen) return PickerOutcome.NotOpen;
            if (SessionId == Guid.Empty || sessionId != SessionId) return PickerOutcome.WrongSession;
            if (!string.Equals(SourceKey, sourceKey?.Trim(), StringComparison.Ordinal)) return PickerOutcome.WrongSource;
            if (!known || !visible) return PickerOutcome.Unavailable;
            if (sourceColor != SourceColor)
            {
                // The lamp changed underneath the modal: refresh visibly and require a re-pick instead
                // of silently overwriting the newer edit.
                SourceColor = sourceColor;
                HasPendingColor = false;
                _pending = ColorMode.Original;
                Stale = true;
                return PickerOutcome.StaleSource;
            }
            if (!HasPendingColor) return PickerOutcome.NoPick;

            commit = new ColorPickerCommit(SessionId, SourceKey, _pending);
            Consumed = true;
            IsOpen = false;
            HasPendingColor = false;
            return PickerOutcome.Committed;
        }

        /// <summary>BACK or an outside click: the staged pick is discarded without applying.</summary>
        public void Cancel() => Reset();

        /// <summary>The app/phone closed: the staged pick is discarded without applying.</summary>
        public void Close() => Reset();

        /// <summary>Closes the modal when a load or slot change replaced the session.</summary>
        public bool CloseIfSessionChanged(Guid sessionId)
        {
            if (!IsOpen || SessionId == sessionId) return false;
            Reset();
            return true;
        }

        private void Reset()
        {
            IsOpen = false;
            HasPendingColor = false;
            Stale = false;
            _pending = ColorMode.Original;
        }
    }

    /// <summary>True only for the seven documented brightness steps.</summary>
    public static bool IsIntensityStep(IntensityLevel level)
    {
        for (int i = 0; i < IntensityCount; i++)
            if (Intensities[i] == level) return true;
        return false;
    }
}

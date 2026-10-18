using System.Collections.Generic;

namespace GoldenHour;

/// <summary>
/// Reapply baseline store: records the game's bias-free value per parameter key. The
/// activation records the first value; the reapply path replaces it via <see cref="Update"/>
/// whenever the game's drift is adopted (BaselinePolicy), so the stored baseline is always
/// the latest adopted game value — restore returns exactly that.
/// </summary>
public sealed class SnapshotStore
{
    private readonly Dictionary<string, Argb> _values = new();

    public int Count => _values.Count;
    public IReadOnlyCollection<string> Keys => _values.Keys;

    public void RecordOnce(string key, Argb value)
    {
        if (string.IsNullOrEmpty(key)) return;
        if (!_values.ContainsKey(key)) _values[key] = value;
    }

    public bool TryGet(string key, out Argb value)
    {
        if (key != null && _values.TryGetValue(key, out var stored))
        {
            value = stored;
            return true;
        }
        value = default;
        return false;
    }

    /// <summary>
    /// Replaces (or sets) a baseline value. Used by the reapply path when the game
    /// overwrote a biased value: the game's current value becomes the new baseline
    /// so the bias follows the day/night drift instead of freezing the first
    /// snapshot. Restore then returns the latest game value, not a stale one.
    /// </summary>
    public void Update(string key, Argb value)
    {
        if (string.IsNullOrEmpty(key)) return;
        _values[key] = value;
    }

    public void Clear() => _values.Clear();
}

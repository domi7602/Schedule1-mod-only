using System.Collections.Generic;

namespace Minimap;

/// <summary>
/// Last successfully applied state of one privately-owned pooled UI property.
/// Commit only after the native setter succeeds; hiding a slot retains child state.
/// Exact managed equality keeps even small pulse/position changes responsive.
/// </summary>
internal struct AppliedValue<T>
{
    private bool _hasValue;
    private T _value;

    public readonly bool NeedsUpdate(T value) =>
        !_hasValue || !EqualityComparer<T>.Default.Equals(_value, value);

    public void Commit(T value)
    {
        _value = value;
        _hasValue = true;
    }
}

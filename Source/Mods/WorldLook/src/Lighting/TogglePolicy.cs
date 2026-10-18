namespace GoldenHour;

/// <summary>A/B toggle state machine: idle → active (snapshot needed once per
/// activation) → idle. Snapshot state resets on every activation.</summary>
public sealed class TogglePolicy
{
    private bool _active;
    private bool _snapshotTaken;

    public bool IsActive => _active;
    public bool SnapshotNeeded => _active && !_snapshotTaken;

    /// <summary>Flip state. Returns the new IsActive value.</summary>
    public bool Toggle()
    {
        _active = !_active;
        _snapshotTaken = false;
        return _active;
    }

    public void MarkSnapshotTaken() => _snapshotTaken = true;
}

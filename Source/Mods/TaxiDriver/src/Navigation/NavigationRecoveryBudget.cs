namespace TaxiDriver;

/// <summary>Operation-wide recovery cap and meaningful-approach deadline, independent of native dispatch orders.</summary>
internal sealed class NavigationRecoveryBudget
{
    private bool _active;
    private float _deadline;
    private int _attempts;
    private bool _lastRescueUsed;
    private float _timeout;
    private float _bestDistance = float.PositiveInfinity;
    private float _progressMark = float.PositiveInfinity;

    // A persistent high-water mark, never a sum of jitter or dispatch-local motion.
    internal void ObserveProgress(float now, float distance, bool eligible)
    {
        if (!_active || HasExpired(now) || !float.IsFinite(distance) || distance < 0f) return;
        if (float.IsPositiveInfinity(_bestDistance))
        {
            _bestDistance = _progressMark = distance;
            return;
        }
        float previousBestDistance = _bestDistance;
        _bestDistance = System.Math.Min(_bestDistance, distance);
        if (!eligible)
        {
            // Consume only new artificial ground; preserve pending genuine progress.
            _progressMark -= previousBestDistance - _bestDistance;
            return;
        }
        if (_progressMark - _bestDistance < 2f) return;
        _progressMark = _bestDistance;
        _deadline = now + _timeout;
    }

    internal int Attempts => _attempts;

    internal void Begin(float now, float timeout)
    {
        _active = true;
        _deadline = now + timeout;
        _timeout = timeout;
        _bestDistance = _progressMark = float.PositiveInfinity;
        _attempts = 0;
        _lastRescueUsed = false;
    }

    internal void Reset()
    {
        _active = false;
        _deadline = 0f;
        _attempts = 0;
        _lastRescueUsed = false;
    }

    internal void ShiftPause(float seconds)
    {
        if (_active) _deadline += seconds;
    }

    internal bool HasExpired(float now) => _active && now >= _deadline;

    internal bool TrySpend(float now, int cap)
    {
        if (!_active || HasExpired(now) || _lastRescueUsed || _attempts >= System.Math.Max(2, cap))
            return false;
        _attempts++;
        return true;
    }

    internal bool TryLastRescue(float now)
    {
        if (!_active || HasExpired(now) || _lastRescueUsed)
            return false;
        _lastRescueUsed = true; // consume before the native attempt, even when it fails
        return true;
    }
}

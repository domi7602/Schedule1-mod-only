namespace LightControl.Core;

/// <summary>
/// Unity-free, monotonic-clock cadence for bounded reconcile scans. It never runs a full scan per
/// frame and never accepts a nonfinite or negative timestamp. The caller supplies the clock.
/// </summary>
public sealed class ScanCadence
{
    private readonly double _intervalSeconds;
    private double _nextAllowed;

    public ScanCadence(double intervalSeconds)
    {
        if (!double.IsFinite(intervalSeconds) || intervalSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(intervalSeconds), "The scan interval must be finite and positive.");
        _intervalSeconds = intervalSeconds;
        _nextAllowed = 0;
    }

    public double IntervalSeconds => _intervalSeconds;

    /// <summary>True at most once per interval; an invalid timestamp is refused and does not consume a slot.</summary>
    public bool ShouldScan(double nowSeconds)
    {
        if (!double.IsFinite(nowSeconds) || nowSeconds < 0) return false;
        if (nowSeconds < _nextAllowed) return false;
        _nextAllowed = nowSeconds + _intervalSeconds;
        return true;
    }

    public void Reset(double nowSeconds)
        => _nextAllowed = double.IsFinite(nowSeconds) && nowSeconds >= 0 ? nowSeconds + _intervalSeconds : 0;
}

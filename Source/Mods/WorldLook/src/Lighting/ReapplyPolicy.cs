namespace GoldenHour;

/// <summary>Bounded reapply scheduling: while active, applied values are refreshed at
/// most once per interval (default minimum 0.5 s). This is the cost-saving fallback
/// cadence — since 0.1.4 the default is per-frame apply (ApplyEveryLateUpdate), because
/// the game's day/night grading rewrites these values continuously and a once-per-second
/// reapply loses every frame race.</summary>
public sealed class ReapplyPolicy
{
    public const double MinimumIntervalSeconds = 0.5;

    private readonly double _interval;
    private double? _lastApplied;

    public ReapplyPolicy(double intervalSeconds)
    {
        _interval = double.IsFinite(intervalSeconds) && intervalSeconds >= MinimumIntervalSeconds
            ? intervalSeconds
            : MinimumIntervalSeconds;
    }

    public bool IsDue(double nowSeconds) =>
        _lastApplied is null || (double.IsFinite(nowSeconds) && nowSeconds - _lastApplied.Value >= _interval);

    public void MarkApplied(double nowSeconds)
    {
        if (double.IsFinite(nowSeconds)) _lastApplied = nowSeconds;
    }
}

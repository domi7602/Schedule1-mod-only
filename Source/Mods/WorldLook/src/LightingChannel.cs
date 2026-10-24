using System;

namespace GoldenHour;

/// <summary>
/// A lease on one color value, not on the borrowed Unity object. Accessors are bound
/// to one exact target. A setter can mutate and then throw, so its attempted value is
/// recorded BEFORE the call. Failed channels stop applying until released.
/// </summary>
public sealed class LightingChannel
{
    public const int MaximumRestoreAttempts = 3;
    public const double RestoreRetrySeconds = 1.0;

    private readonly Func<bool> _isValid;
    private readonly Func<Argb?> _read;
    private readonly Action<Argb> _write;
    private readonly Action<string> _reportError;
    private readonly Argb _tint;
    private Argb _baseline;
    private Argb? _lastWrite;
    private Argb? _lastIntended;
    private int _restoreAttempts;
    private double _nextRestore;

    public LightingChannel(Func<bool> isValid, Func<Argb?> read, Action<Argb> write,
        Argb tint, Action<string> reportError)
    {
        _isValid = isValid;
        _read = read;
        _write = write;
        _tint = tint;
        _reportError = reportError;
    }

    public bool Faulted { get; private set; }
    public bool Releasing { get; private set; }
    public bool HasPendingWrite => _lastWrite.HasValue;

    /// <summary>True means readable/processed, not a measured rendering result.</summary>
    public bool Apply(double strength)
    {
        if (Faulted || Releasing) return false;
        bool attemptedWrite = false;
        try
        {
            if (!_isValid()) return false;
            Argb? current = _read();
            if (!current.HasValue || !current.Value.IsFinite) return false;

            BaselineDecision decision = BaselinePolicy.Evaluate(_baseline, current.Value, _tint, strength, _lastWrite);
            _baseline = decision.Baseline;
            bool unchangedOwnWrite = current == _lastWrite && decision.Blended == _lastIntended;
            if (current != _lastWrite)
            {
                _lastWrite = null;
                _lastIntended = null;
            }
            if (decision.Write && !unchangedOwnWrite)
            {
                _lastWrite = decision.Blended;
                _lastIntended = decision.Blended;
                attemptedWrite = true;
                _write(decision.Blended);
                // Native setters may normalize floats. Record the immediate readback,
                // rather than treating that normalization as fresh native daylight next
                // frame. Keep the attempted value if the setter/readback throws.
                Argb? applied = _read();
                if (!applied.HasValue || !applied.Value.IsFinite)
                    throw new InvalidOperationException("apply readback is unreadable");
                _lastWrite = applied;
            }
            // Neutral or numerically unchanged channels need no later restoration.
            if (_lastWrite == _baseline) _lastWrite = null;
            return true;
        }
        catch (Exception ex)
        {
            if (attemptedWrite)
            {
                // A setter can normalize/mutate before throwing. Reconcile immediately
                // on the same thread if possible, before native per-frame drift resumes.
                try
                {
                    Argb? observed = _read();
                    if (observed.HasValue && observed.Value.IsFinite)
                        _lastWrite = observed.Value == _baseline ? null : observed;
                }
                catch { /* Retain the pre-call attempted value for conservative release. */ }
            }
            Faulted = true;
            _reportError("apply failed: " + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Restores only an unchanged own write. Native/foreign changes win. A failed or
    /// unreadable restore keeps its lease, with at most three spaced write attempts;
    /// a later activation cannot adopt our unresolved tint as a pristine baseline.
    /// Null reads mean temporarily unavailable; invalid identity means the target is gone.
    /// </summary>
    public bool Release(double nowSeconds)
    {
        Releasing = true;
        if (!_lastWrite.HasValue) return true;
        try
        {
            if (!_isValid())
            {
                _lastWrite = null;
                return true;
            }
            if (nowSeconds < _nextRestore) return false;
            _nextRestore = nowSeconds + RestoreRetrySeconds;

            Argb? current = _read();
            if (!current.HasValue || !current.Value.IsFinite)
                throw new InvalidOperationException("owned value is unreadable during restore");

            if (current.Value != _lastWrite.Value || current.Value == _baseline)
            {
                _lastWrite = null;
                return true;
            }

            if (_restoreAttempts >= MaximumRestoreAttempts) return false;
            _restoreAttempts++;
            _write(_baseline);
            // Check even a normally returning setter: a silent no-op is not a release.
            Argb? after = _read();
            if (!after.HasValue || !after.Value.IsFinite || after.Value == _lastWrite.Value)
                throw new InvalidOperationException("restore readback did not confirm release");
            _lastWrite = null;
            return true;
        }
        catch (Exception ex)
        {
            Faulted = true;
            _reportError("restore failed: " + ex.Message);
            return false;
        }
    }
}

using System;

namespace Minimap;

/// <summary>Managed budget for opt-in development observations; never a gameplay completion policy.</summary>
public sealed class SearchObservationGate
{
    public const double DurationSeconds = 600;
    public const double PollSeconds = 0.25;
    public const int MaxReports = 256;
    public bool Enabled { get; private set; }
    public int Epoch { get; private set; }
    public int Reports { get; private set; }
    private double _deadline;
    private double _nextSample;
    private string? _last;
    private bool _autoStopPending;

    public void Start(double now)
    {
        Stop();
        if (!double.IsFinite(now) || now < 0) return;
        _deadline = now + DurationSeconds;
        _nextSample = now;
        Reports = 0;
        Epoch = 0;
        Enabled = true;
    }

    public void Stop()
    {
        Enabled = false;
        _last = null;
        _autoStopPending = false;
    }

    public void ResetEpoch()
    {
        Epoch++;
        _last = null;
    }

    public bool TryConsumeAutoStop()
    {
        bool stopped = _autoStopPending;
        _autoStopPending = false;
        return stopped;
    }

    public bool CanObserve(double now)
    {
        if (!Enabled) return false;
        if (!double.IsFinite(now) || now < 0 || now >= _deadline || Reports >= MaxReports)
        {
            Stop();
            _autoStopPending = true;
            return false;
        }
        return true;
    }

    public bool ShouldSample(double now)
    {
        if (!CanObserve(now)) return false;
        if (now < _nextSample) return false;
        _nextSample = now + PollSeconds;
        return true;
    }

    public bool ShouldReport(string fingerprint)
    {
        if (!Enabled) return false;
        if (Reports >= MaxReports)
        {
            Stop();
            _autoStopPending = true;
            return false;
        }
        if (string.Equals(_last, fingerprint, StringComparison.Ordinal)) return false;
        _last = fingerprint;
        Reports++;
        return true;
    }
}

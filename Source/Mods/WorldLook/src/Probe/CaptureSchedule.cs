using System;
using System.Collections.Generic;

namespace GraphicsProbe;

// Main-thread only. No retained Unity objects, coroutines or background callbacks.
internal sealed class CaptureSchedule
{
    private readonly double _delay;
    private readonly double _cooldown;
    private readonly List<string> _reasons = new();
    private double _due;
    private double _notBefore;
    private bool _running;
    private bool _closed;

    public CaptureSchedule(double delay, double cooldown)
    {
        _delay = Math.Max(0, delay);
        _cooldown = Math.Max(0, cooldown);
    }

    public void Request(double now, string reason)
    {
        if (_closed) return;
        if (_reasons.Count == 0) _due = now + _delay;
        // Callers supply fixed reason labels, not unbounded scene/object names.
        if (!_reasons.Contains(reason) && _reasons.Count < 3) _reasons.Add(reason);
    }

    public bool TryBegin(double now, out string reason)
    {
        reason = "";
        if (_closed || _running || _reasons.Count == 0 || now < Math.Max(_due, _notBefore)) return false;
        reason = string.Join(",", _reasons);
        _reasons.Clear();
        _running = true;
        return true;
    }

    public void Complete(double now)
    {
        if (!_running) return;
        _running = false;
        _notBefore = now + _cooldown;
    }

    public void Shutdown()
    {
        _closed = true;
        _running = false;
        _reasons.Clear();
    }
}

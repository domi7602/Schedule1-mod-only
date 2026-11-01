using System;

namespace PoliceTestMode;

/// <summary>Capture before the first write; retain the original across reapplication.</summary>
internal sealed class BoolLease
{
    private readonly Func<bool> _alive;
    private readonly Func<bool> _read;
    private readonly Action<bool> _write;
    private readonly bool _forced;
    private bool _original;
    private bool _captured;

    public BoolLease(Func<bool> alive, Func<bool> read, Action<bool> write, bool forced)
    {
        _alive = alive;
        _read = read;
        _write = write;
        _forced = forced;
    }

    public void Apply()
    {
        if (!_alive()) return;
        if (!_captured) { _original = _read(); _captured = true; }
        if (_read() != _forced) _write(_forced);
        if (_read() != _forced) throw new InvalidOperationException("Suppression readback mismatch.");
    }

    public bool Restore()
    {
        if (!_captured || !_alive()) { _captured = false; return true; }
        _write(_original);
        if (_read() != _original) return false;
        _captured = false;
        return true;
    }
}

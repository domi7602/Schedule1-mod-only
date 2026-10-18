using System;

namespace TaxiDriver;

/// <summary>Call-scoped final native inputs. Does not write derived output flags.</summary>
internal sealed class NativeActuatorScope : IDisposable
{
    private readonly (float Throttle, bool Handbrake, bool Controls) _saved;
    private readonly Action<float> _throttle;
    private readonly Action<bool> _handbrake;
    private readonly Action<bool> _controls;
    private bool _restored;

    private NativeActuatorScope((float, bool, bool) saved, Action<float> throttle,
        Action<bool> handbrake, Action<bool> controls)
    {
        _saved = saved;
        _throttle = throttle;
        _handbrake = handbrake;
        _controls = controls;
    }

    internal static NativeActuatorScope Begin(Func<(float, bool, bool)> read,
        Action<float> throttle, Action<bool> handbrake, Action<bool> controls, bool reverse)
    {
        var scope = new NativeActuatorScope(read(), throttle, handbrake, controls);
        try
        {
            throttle(ReversePedalPolicy.CurrentThrottleFor(reverse));
            handbrake(!reverse);
            controls(true);
            return scope;
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        if (_restored) return;
        _restored = true;
        // Attempt every saved input even if a destroyed native handle rejects one.
        try { _throttle(_saved.Throttle); }
        finally
        {
            try { _handbrake(_saved.Handbrake); }
            finally { _controls(_saved.Controls); }
        }
    }
}

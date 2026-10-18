using System;

namespace WaterShader;

internal interface IWaterRuntime<TTarget, TMaterial> where TTarget : class where TMaterial : class
{
    TTarget? Discover();
    bool TargetAlive(TTarget value);
    bool MaterialAlive(TMaterial value);
    TMaterial? ReadMaterial(TTarget target);
    TMaterial CreateMaterial(TMaterial template);
    void ConfigureMaterial(TMaterial material);
    void WriteMaterial(TTarget target, TMaterial? material);
    bool SameMaterial(TMaterial? left, TMaterial right);
    void DestroyMaterial(TMaterial material);
}

internal enum WaterState { Stopped, Waiting, Applied, Yielded, Faulted }

/// <summary>Unity-free owner and scheduler. Native work stays behind the deadline gate.</summary>
internal sealed class WaterSession<TTarget, TMaterial> where TTarget : class where TMaterial : class
{
    internal const double RetrySeconds = 5;
    internal const double HealthSeconds = 1;
    private readonly IWaterRuntime<TTarget, TMaterial> _runtime;
    private TTarget? _target;
    private TMaterial? _original;
    private TMaterial? _owned;
    private bool _active;
    private bool _pendingRelease;
    private double _nextCheck;

    public WaterState State { get; private set; }
    public Exception? LastError { get; private set; }
    public WaterSession(IWaterRuntime<TTarget, TMaterial> runtime) { _runtime = runtime; }

    public void Start(double now)
    {
        if (_active) return;
        _active = true;
        _nextCheck = now;
        if (!_pendingRelease) State = WaterState.Waiting;
    }

    public void Stop()
    {
        _active = false;
        try { Release(); State = WaterState.Stopped; }
        catch (Exception ex) { Fault(ex); }
    }

    private void Fault(Exception ex)
    {
        LastError = ex;
        State = WaterState.Faulted;
        _pendingRelease = _target != null || _owned != null;
    }

    private void Release()
    {
        if (_owned != null)
        {
            if (_target != null && _runtime.TargetAlive(_target)
                && _runtime.SameMaterial(_runtime.ReadMaterial(_target), _owned))
            {
                _runtime.WriteMaterial(_target, _original != null && _runtime.MaterialAlive(_original) ? _original : null);
                if (_runtime.SameMaterial(_runtime.ReadMaterial(_target), _owned))
                    throw new InvalidOperationException("Water restore did not detach the owned material.");
            }
            // A failed read/restore retains ownership: never destroy a possibly attached clone.
            if (_runtime.MaterialAlive(_owned)) _runtime.DestroyMaterial(_owned);
        }
        _target = null;
        _owned = null;
        _original = null;
        _pendingRelease = false;
        LastError = null;
    }

    public void Tick(double now)
    {
        if (!_active || now < _nextCheck) return;
        _nextCheck = now + RetrySeconds; // No tight retries or catch-up after a stalled frame.
        try
        {
            if (_pendingRelease)
            {
                Release();
                State = WaterState.Waiting;
                return; // Cleanup and new allocation never share a retry slice.
            }
            if (_target != null)
            {
                if (!_runtime.TargetAlive(_target)
                    || (_owned != null && !_runtime.MaterialAlive(_owned)))
                {
                    Release();
                    State = WaterState.Waiting;
                    return;
                }
                if (_owned != null && !_runtime.SameMaterial(_runtime.ReadMaterial(_target), _owned))
                {
                    var retained = _target;
                    Release();
                    _target = retained; // Yield until the renderer dies or the scene resets.
                    State = WaterState.Yielded;
                }
                _nextCheck = now + HealthSeconds;
                return;
            }
            var target = _runtime.Discover();
            State = WaterState.Waiting;
            if (target == null || !_runtime.TargetAlive(target)) return;
            var original = _runtime.ReadMaterial(target);
            if (original == null || !_runtime.MaterialAlive(original)) return;
            _target = target;
            _original = original;
            _owned = _runtime.CreateMaterial(original);
            _runtime.ConfigureMaterial(_owned); // Retain even if tuning fails partway through.
            // Capture ownership before the setter: it may mutate and then throw.
            _runtime.WriteMaterial(target, _owned);
            if (!_runtime.SameMaterial(_runtime.ReadMaterial(target), _owned))
                throw new InvalidOperationException("Water material assignment did not persist.");
            State = WaterState.Applied;
            LastError = null;
            _nextCheck = now + HealthSeconds;
        }
        catch (Exception ex) { Fault(ex); }
    }
}

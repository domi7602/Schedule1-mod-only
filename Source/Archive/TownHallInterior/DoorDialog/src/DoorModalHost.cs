using System;

namespace TownHallInterior;

// All operations run on the game thread. An implementation must refuse every
// mutation unless it can prove the same scene and exclusive focus ownership.
public interface IDoorModalRuntime
{
    bool IsCurrent { get; }
    bool IsExclusive { get; }
    bool TryOpen(string[] options, Action<int?> selected);
    bool IsClosed { get; }
    bool TryRelease(bool closeOwnedPopup);
}

public sealed class NativeDoorModalHost : IDoorModalHost
{
    private readonly IDoorModalRuntime _runtime;
    private Lease? _active;
    private long _generation;
    private bool _shutdown;
    public NativeDoorModalHost(IDoorModalRuntime runtime) => _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));

    public bool TryOpen(string title, string[] options, Action<int?> result, out IDoorModalLease? lease)
    {
        lease = null;
        if (_shutdown || _active != null || options == null || options.Length != 3 || result == null) return false;
        Lease? current = null;
        try
        {
            if (!_runtime.IsCurrent || !_runtime.IsExclusive) return false;
            current = new Lease(this, ++_generation, result);
            _active = current;
            if (!_runtime.TryOpen(options, current.Select)) return FailedOpen(current, out lease);
            lease = current;
            return true;
        }
        catch
        {
            return current != null && FailedOpen(current, out lease);
        }
    }

    // If partial cleanup is unproven, retain a cancellation-only lease: reporting
    // failure would let the door adapter immediately invoke the native action.
    private bool FailedOpen(Lease current, out IDoorModalLease? lease)
    {
        current.Chosen = true;
        current.Value = null;
        current.RecoveringOpen = true;
        if (Release(current, true)) { lease = null; return false; }
        lease = current;
        return true;
    }

    // Called only by the owner on a later game update, never from the native callback.
    public void Tick()
    {
        var current = _active;
        if (current == null || current.Releasing) return;
        if (_shutdown) { Release(current, true); return; }
        try
        {
            if (!_runtime.IsCurrent) { Abandon(current); return; }
            if (!_runtime.IsExclusive) return;
            if (current.RecoveringOpen)
            {
                var cancelled = current.Callback;
                if (!Release(current, true)) return;
                if (!_shutdown && _runtime.IsCurrent) cancelled(null);
                return;
            }
            if (!_runtime.IsClosed) return;
            var result = current.Chosen ? current.Value : null;
            var callback = current.Callback;
            if (!Release(current, false)) return;
            if (_shutdown || !_runtime.IsCurrent) return;
            callback(result);
        }
        catch { _shutdown = true; Abandon(current); }
    }

    public void Shutdown()
    {
        _shutdown = true;
        var current = _active;
        if (current != null) current.Close();
    }

    private void Abandon(Lease lease)
    {
        if (ReferenceEquals(_active, lease)) { _active = null; ++_generation; }
        lease.Releasing = true;
    }

    private bool Release(Lease lease, bool closePopup)
    {
        if (!ReferenceEquals(_active, lease) || lease.Releasing) return false;
        lease.Releasing = true;
        try
        {
            if (!_runtime.IsCurrent) { Abandon(lease); return false; }
            if (!_runtime.IsExclusive) { lease.Releasing = false; return false; }
            if (!_runtime.TryRelease(closePopup)) { lease.Releasing = false; return false; }
            Abandon(lease);
            return true;
        }
        catch { _shutdown = true; Abandon(lease); return false; }
    }

    private sealed class Lease : IDoorModalLease
    {
        private readonly NativeDoorModalHost _host;
        private readonly long _generation;
        internal readonly Action<int?> Callback;
        internal bool Releasing, Chosen, RecoveringOpen;
        internal int? Value;
        internal Lease(NativeDoorModalHost host, long generation, Action<int?> callback)
        { _host = host; _generation = generation; Callback = callback; }
        internal void Select(int? value)
        {
            if (Releasing || Chosen || _host._generation != _generation || !ReferenceEquals(_host._active, this)) return;
            Chosen = true;
            Value = value;
        }
        public void Close() => _host.Release(this, true);
    }
}

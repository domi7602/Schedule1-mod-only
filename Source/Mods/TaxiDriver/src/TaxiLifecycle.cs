using System;

namespace TaxiDriver;

/// <summary>Unity-free lifetime gate shared by updates, service and deferred callbacks.</summary>
internal sealed class TaxiLifecycle
{
    private bool _stopping;
    private bool _stopped;
    private long _generation;
    internal bool AcceptingWork { get; private set; }

    internal bool Initialize()
    {
        if (_stopping) return false;
        if (AcceptingWork) return true;
        _generation++;
        _stopped = false;
        AcceptingWork = true;
        return true;
    }

    internal Action<T> Guard<T>(Action<T> callback)
    {
        long generation = _generation;
        return value =>
        {
            if (AcceptingWork && generation == _generation) callback(value);
        };
    }

    internal void Shutdown((string Label, Action Run)[] cleanup,
        (string Label, Action Run)[] finalization, Action<string, Exception> report)
    {
        if (_stopping || _stopped) return;
        _stopping = true; // claim once before invoking any externally supplied callback
        AcceptingWork = false;
        try
        {
            foreach (var step in cleanup) Run(step, report);
        }
        finally
        {
            try
            {
                foreach (var step in finalization) Run(step, report);
            }
            finally { _stopping = false; _stopped = true; }
        }
    }

    private static void Run((string Label, Action Run) step, Action<string, Exception> report)
    {
        try { step.Run(); }
        catch (Exception ex)
        {
            try { report(step.Label, ex); }
            catch { /* Logging cannot interrupt cleanup or hook finalization. */ }
        }
    }
}

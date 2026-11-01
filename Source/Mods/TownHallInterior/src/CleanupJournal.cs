using System;
using System.Collections.Generic;

namespace TownHallInterior;

// A restoration stays pending until it returns normally. Unity remains an adapter boundary.
internal sealed class CleanupJournal
{
    private readonly List<(string Name, Action Restore, Func<bool>? Ready)> _pending = new();
    internal bool IsComplete => _pending.Count == 0;
    internal int PendingCount => _pending.Count;

    internal void Add(string name, Action restore, Func<bool>? ready = null) => _pending.Add((name, restore, ready));

    internal bool Run(Action<string, Exception> report)
    {
        for (int i = 0; i < _pending.Count;)
        {
            var step = _pending[i];
            try
            {
                if (step.Ready != null && !step.Ready()) { i++; continue; }
                step.Restore();
                _pending.RemoveAt(i);
            }
            catch (Exception ex)
            {
                i++; // Keep this exact failed action for the next bounded pass.
                try { report(step.Name, ex); }
                catch { /* Diagnostics must not prevent restoration of other native state. */ }
            }
        }
        return IsComplete;
    }
}

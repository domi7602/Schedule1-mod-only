using System;
using System.Collections.Generic;
namespace TaxiDriver;

/// <summary>Owns exact delegate/event pairs, including managed wrappers whose native
/// owner has died. Clear before destroying UI; never consult Unity overloaded null.</summary>
internal sealed class OwnedSubscriptions
{
    private readonly List<Action> _remove = new();
    internal int Count => _remove.Count;

    internal void Add<TCallback, TEvent>(TCallback callback, TEvent evt,
        Action<TCallback, TEvent> subscribe, Action<TCallback, TEvent> unsubscribe,
        Action<TCallback> forget)
    {
        Action remove = () =>
        {
            try { unsubscribe(callback, evt); }
            catch (Exception) { /* Native owner may already be dead. Still release the helper root. */ }
            finally
            {
                try { forget(callback); }
                catch (Exception) { /* One broken helper must not retain this registry or block others. */ }
            }
        };
        try { subscribe(callback, evt); }
        catch
        {
            remove();
            throw;
        }
        _remove.Add(remove);
    }

    internal void Clear()
    {
        var entries = _remove.ToArray();
        _remove.Clear();
        foreach (var remove in entries) remove();
    }
}

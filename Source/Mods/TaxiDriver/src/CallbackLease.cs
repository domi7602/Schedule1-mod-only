using System;
namespace TaxiDriver;

/// <summary>Breaks the native callback -> app graph even when native unsubscribe
/// fails. The forwarding delegate remains inert and no longer owns the app.</summary>
internal sealed class CallbackLease<T>
{
    private Action<T>? _callback;
    internal CallbackLease(Action<T> callback) => _callback = callback;
    internal void Invoke(T value) => _callback?.Invoke(value);
    internal void Invoke() => Invoke(default!);
    internal void Release() => _callback = null;
}

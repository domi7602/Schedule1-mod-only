using System;

namespace TaxiDriver;

/// <summary>Swap a private reference only for synchronous native consumption.</summary>
internal sealed class PrivateFlagsScope<T> : IDisposable where T : class
{
    private readonly T _saved;
    private readonly Action<T> _write;
    private bool _restored;

    private PrivateFlagsScope(T saved, Action<T> write) => (_saved, _write) = (saved, write);

    internal static PrivateFlagsScope<T> Begin(Func<T> read, Action<T> write,
        Func<T, T> copy, Func<T, T, bool> sameNativeObject,
        Action<PrivateFlagsScope<T>>? publish = null)
    {
        T original = read();
        T privateCopy = copy(original);
        if (privateCopy == null || sameNativeObject(original, privateCopy))
            throw new InvalidOperationException("Flags copy must be a physically distinct native object.");
        var scope = new PrivateFlagsScope<T>(original, write);
        publish?.Invoke(scope); // Harmony state exists before any reference write.
        try
        {
            write(privateCopy);
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
        _write(_saved);
        _restored = true;
    }
}

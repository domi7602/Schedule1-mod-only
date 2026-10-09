using System;

namespace TaxiDriver;

/// <summary>Ownership classification for the one vehicle TaxiDriver spawned.</summary>
internal enum VehicleOwnershipKind
{
    None,
    Owned,
    Foreign,
    Invalid,
    DestroyPending,
    DestroyUnknown,
}

/// <summary>
/// Pure ownership/destruction state for the vehicle returned by TaxiDriver's own
/// spawn call. A native pointer is never treated as owned unless it was registered
/// at spawn time. Ambiguous destruction keeps the token and permits at most one
/// explicitly requested retry.
/// </summary>
internal sealed class VehicleOwnershipLedger
{
    internal const int MaxDestroyAttempts = 2;

    private bool _destroyPending;
    private bool _destroyUnknown;

    internal IntPtr TrackedPointer { get; private set; }

    internal int DestroyAttempts { get; private set; }

    internal bool DestroyPending => _destroyPending;

    internal bool DestroyUnknown => _destroyUnknown;

    internal bool TryRegister(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero || TrackedPointer != IntPtr.Zero)
            return false;

        TrackedPointer = pointer;
        _destroyPending = false;
        _destroyUnknown = false;
        DestroyAttempts = 0;
        return true;
    }

    internal VehicleOwnershipKind Classify(IntPtr candidatePointer, bool isAlive)
    {
        if (candidatePointer == IntPtr.Zero || !isAlive)
            return TrackedPointer == IntPtr.Zero && candidatePointer == IntPtr.Zero
                ? VehicleOwnershipKind.None
                : VehicleOwnershipKind.Invalid;

        if (TrackedPointer == IntPtr.Zero || candidatePointer != TrackedPointer)
            return VehicleOwnershipKind.Foreign;

        if (_destroyPending)
            return VehicleOwnershipKind.DestroyPending;
        if (_destroyUnknown)
            return VehicleOwnershipKind.DestroyUnknown;
        return VehicleOwnershipKind.Owned;
    }

    internal bool CanMutate(IntPtr candidatePointer, bool isAlive) =>
        Classify(candidatePointer, isAlive) == VehicleOwnershipKind.Owned;

    internal bool TryBeginDestroy(IntPtr candidatePointer, bool isAlive, bool explicitRetry)
    {
        VehicleOwnershipKind kind = Classify(candidatePointer, isAlive);
        bool firstAttempt = kind == VehicleOwnershipKind.Owned && DestroyAttempts == 0;
        bool boundedManualRetry = kind == VehicleOwnershipKind.DestroyUnknown
            && explicitRetry
            && DestroyAttempts < MaxDestroyAttempts;

        if (!firstAttempt && !boundedManualRetry)
            return false;

        DestroyAttempts++;
        _destroyPending = true;
        _destroyUnknown = false;
        return true;
    }

    /// <summary>
    /// Resolves one pending destroy call. A live object means the result is
    /// ambiguous, not success; its ownership token is deliberately retained.
    /// </summary>
    internal bool ResolveDestroy(IntPtr candidatePointer, bool isAlive)
    {
        if (!_destroyPending || candidatePointer == IntPtr.Zero || candidatePointer != TrackedPointer)
            return false;

        if (isAlive)
        {
            _destroyPending = false;
            _destroyUnknown = true;
            return false;
        }

        Clear();
        return true;
    }

    /// <summary>Turns a timed-out pending request into an ambiguous result without retrying it.</summary>
    internal bool MarkDestroyUnknown(IntPtr candidatePointer)
    {
        if (!_destroyPending || candidatePointer == IntPtr.Zero || candidatePointer != TrackedPointer)
            return false;

        _destroyPending = false;
        _destroyUnknown = true;
        return true;
    }

    /// <summary>Clear the token only after a caller has independently confirmed native death.</summary>
    internal bool ClearAfterConfirmedDeath(IntPtr candidatePointer, bool isAlive)
    {
        if (isAlive || candidatePointer == IntPtr.Zero || candidatePointer != TrackedPointer)
            return false;

        Clear();
        return true;
    }

    /// <summary>Used only by the owning lifecycle after it has confirmed destruction or scene death.</summary>
    internal void ResetAfterConfirmedDeath()
    {
        Clear();
    }

    private void Clear()
    {
        TrackedPointer = IntPtr.Zero;
        _destroyPending = false;
        _destroyUnknown = false;
        DestroyAttempts = 0;
    }
}

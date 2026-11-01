using System;

namespace TownHallInterior;

// Domains whose mutations the native observation layer must report while a
// lease is held. The owner never mutates Typing; a reported typing change
// therefore always indicates foreign activity.
public enum FocusDomain { ScreenStack = 0, Cursor = 1, Movement = 2, Typing = 3 }

// A real, continuously exclusive coordinator contract: a snapshot of
// cursor/movement values is NOT such a coordinator. The coordinator must
// exclude foreign changes to the screen stack as well as cursor and movement,
// continuously, including interleaving identical values.
public interface IExclusiveModalFocus
{
    bool IsHeld { get; }
}

// Maintains one exclusive modal-focus lease per instance. Ownership is proven
// by attributed mutations, never by comparing focus values: the owner announces
// every mutation before performing it, and the native observation layer reports
// every observed change through NotifyChanged. Any unannounced change — or any
// announced mutation whose notification count does not match — poisons the lease
// until Reset, including interleavings that leave identical values behind.
//
// This class is game-free logic. The observation layer (native hooks on screen
// stack, cursor, movement and typing mutations) is NOT part of this build and
// remains unverified. Without a faithful observation layer this class cannot
// prove exclusivity by itself; it only maintains a lease given honest reports.
public sealed class ExclusiveModalFocus : IExclusiveModalFocus
{
    // Bounded leases per instance, mirroring the runtime's retained-payload cap.
    public const int MaxLeases = 128;

    private readonly Func<bool> _baseline;
    private readonly int[] _pending = new int[4];
    private bool _leased;
    private bool _poisoned;
    private int _leaseCount;

    // The baseline must hold exactly once at lease start; it is deliberately
    // not re-evaluated afterwards, because the owner's own mutations change the
    // observed values. Continuous safety comes from attributed mutations.
    // A throwing baseline propagates without changing any state.
    public ExclusiveModalFocus(Func<bool> baseline)
        => _baseline = baseline ?? throw new ArgumentNullException(nameof(baseline));

    // True only while a lease is held and no foreign change was reported.
    public bool IsHeld => _leased && !_poisoned;

    public bool TryBegin()
    {
        if (_leased || _poisoned || _leaseCount >= MaxLeases) return false;
        if (!_baseline()) return false;
        _leased = true;
        _leaseCount++;
        return true;
    }

    // Announces and performs exactly one owned mutation. The observation layer
    // must report it exactly expectedNotifications times: fewer reports leave
    // pending slots that make TryEnd refuse, more reports poison the lease.
    public bool MutateOwned(FocusDomain domain, int expectedNotifications, Action mutation)
    {
        if (mutation == null) throw new ArgumentNullException(nameof(mutation));
        if (!IsHeld) return false;
        if (expectedNotifications < 1) throw new ArgumentOutOfRangeException(nameof(expectedNotifications));
        _pending[Index(domain)] += expectedNotifications;
        try { mutation(); }
        catch { _poisoned = true; throw; }
        return true;
    }

    // Called by the native observation layer for every observed change. Outside
    // a lease foreign activity is normal and ignored. While leased, a change
    // without a pending owned slot is foreign and poisons the lease.
    public void NotifyChanged(FocusDomain domain)
    {
        if (!_leased || _poisoned) return;
        int index = Index(domain);
        if (_pending[index] == 0) { _poisoned = true; return; }
        _pending[index]--;
    }

    // Ends the lease only when every announced mutation was observed and no
    // foreign change happened. Any other state poisons the lease and refuses,
    // because restoring foreign-modified state would be unsafe. Reset remains
    // the authoritative exit for a poisoned lease.
    public bool TryEnd()
    {
        if (!_leased) return false;
        if (_poisoned || HasPending()) { _poisoned = true; return false; }
        _leased = false;
        return true;
    }

    // Authoritative invalidation (scene reload or authority loss). Clears lease
    // and poison state; previously leased state must be treated as foreign from
    // here on.
    public void Reset()
    {
        _leased = false;
        _poisoned = false;
        for (int i = 0; i < _pending.Length; i++) _pending[i] = 0;
    }

    private static int Index(FocusDomain domain)
        => domain >= FocusDomain.ScreenStack && domain <= FocusDomain.Typing ? (int)domain
            : throw new ArgumentOutOfRangeException(nameof(domain));

    private bool HasPending()
    {
        for (int i = 0; i < _pending.Length; i++) if (_pending[i] != 0) return true;
        return false;
    }
}

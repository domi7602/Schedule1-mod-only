using System;

namespace TownHallInterior;

// A selector must call back only after TryShow has succeeded. It owns and releases
// its own focus, and Close must be idempotent. No native listeners are modified.
public interface IDoorSelector
{
    bool TryShow(Action<DoorAction> chosen);
    void Close();
}

// Shared by the Harmony prefix and tests; callbacks borrow the current door.
public sealed class DoorAdapter
{
    private readonly IDoorSelector _selector;
    private readonly DoorChoice _choice = new();
    private int _generation;
    private long _ticket;
    private bool _replaying;
    private object? _replayIdentity;

    public DoorAdapter(IDoorSelector selector) => _selector = selector;

    // Returns true to let the native method run. Failures before a choice is
    // made (foreign door, missing authority, unready selector) pass native.
    // Once ShowChoice was selected, any failure suppresses and poisons the
    // choice until an authoritative reset (generation change or authority
    // loss): a native action whose outcome is unknown is never retried.
    public bool Prefix(bool target, bool singleplayer, int generation, Action open, Action knock,
        Func<bool>? stillValid = null, object? identity = null, bool selectorReady = true)
    {
        if (_generation != generation) Reset(generation);
        // A different door cannot consume the captured native replay token.
        if (_replaying && !Equals(_replayIdentity, identity)) return true;
        if (target && !singleplayer) { Reset(generation); return true; }
        var result = _choice.Interact(target, singleplayer, selectorReady);
        if (result == Intercept.PassNative) return true;
        if (result == Intercept.Suppress) return false;
        long ticket = ++_ticket;
        bool showing = false;
        bool premature = false;
        try
        {
            bool success = _selector.TryShow(action =>
            {
                if (!showing) { premature = true; return; }
                Complete(ticket, generation, action, open, knock, stillValid, identity);
            });
            if (!success || premature) return FailedShow();
            showing = true;
            return false;
        }
        catch { return FailedShow(); }
    }

    // The popup may be partially open or its lease state uncertain: suppress
    // the native action and poison until an authoritative reset.
    private bool FailedShow()
    {
        ++_ticket;
        try { _selector.Close(); } catch { }
        _choice.Poison();
        return false;
    }

    private void Complete(long ticket, int generation, DoorAction action, Action open,
        Action knock, Func<bool>? stillValid, object? identity)
    {
        if (ticket != _ticket || generation != _generation) return;
        ++_ticket;
        var chosen = _choice.Choose(action);
        try { _selector.Close(); }
        catch { _choice.Poison(); return; }
        try
        {
            if (stillValid != null && !stillValid()) { _choice.Reset(); return; }
            if (chosen == DoorAction.Open) { open(); return; }
            if (chosen == DoorAction.Knock && _choice.StartReplay())
            {
                _replaying = true;
                _replayIdentity = identity;
                try { knock(); }
                finally { _replaying = false; _replayIdentity = null; _choice.EndReplay(); }
            }
        }
        // An action that threw may have partially run: its outcome is unknown,
        // so the choice is poisoned instead of being reopened.
        catch { _choice.Poison(); }
    }

    public void Reset(int generation)
    {
        _generation = generation;
        ++_ticket;
        _choice.Reset();
        _replaying = false;
        _replayIdentity = null;
        try { _selector.Close(); } catch { }
    }
}

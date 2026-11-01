namespace TownHallInterior;
public enum DoorAction { None, Open, Knock, Cancel }
public enum Intercept { PassNative, ShowChoice, Suppress }
public sealed class DoorChoice
{
    private bool _pending;
    private bool _replayArmed;
    private bool _replaying;
    private bool _replayPassed;
    private bool _poisoned;

    // Adapter must confirm identity, authority and menu setup before calling.
    // Exactly one intercept is returned per call: PassNative only before a
    // choice is made (foreign door, missing authority or unready selector),
    // ShowChoice only for the single first target interaction, Suppress in
    // every pending/replay/poisoned state.
    public Intercept Interact(bool target, bool singleplayer, bool selectorReady)
    {
        if (!target) return Intercept.PassNative;
        if (_poisoned) return Intercept.Suppress;
        if (_replaying)
        {
            if (_replayPassed) return Intercept.Suppress;
            _replayPassed = true;
            return Intercept.PassNative;
        }
        if (_pending || _replayArmed) return Intercept.Suppress;
        return Begin(target, singleplayer, selectorReady) ? Intercept.ShowChoice : Intercept.PassNative;
    }
    public bool StartReplay()
    {
        if (!_replayArmed || _replaying) return false;
        _replayArmed = false;
        _replaying = true;
        _replayPassed = false;
        return true;
    }
    public void EndReplay() { _replaying = false; _replayPassed = false; }

    // A failure after suppression made the native action outcome unknown.
    // Poison suppresses every further target interaction until Reset; the
    // adapter must never pass the native action again on its own.
    public void Poison()
    {
        _poisoned = true;
        _pending = false;
        _replayArmed = false;
        _replaying = false;
        _replayPassed = false;
    }
    public void Reset()
    {
        _poisoned = false;
        _pending = false;
        _replayArmed = false;
        _replaying = false;
        _replayPassed = false;
    }
    private bool Begin(bool target, bool singleplayer, bool selectorReady)
    {
        if (_pending || _poisoned || !target || !singleplayer || !selectorReady) return false;
        _pending = true;
        return true;
    }
    public DoorAction Choose(DoorAction action)
    {
        if (!_pending) return DoorAction.None;
        _pending = false;
        if (action == DoorAction.Knock) _replayArmed = true;
        return action == DoorAction.Open || action == DoorAction.Knock ? action : DoorAction.None;
    }
}

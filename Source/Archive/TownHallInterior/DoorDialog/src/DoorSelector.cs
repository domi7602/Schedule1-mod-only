using System;

namespace TownHallInterior;

public readonly record struct ModalFocusState(int ManagerId, int PlayerId, int[] Stack,
    bool PopupActive, bool CanMove, bool Typing, bool MouseFreed);

public static class ModalOwnership
{
    // The caller must independently prove the exclusive lease; matching values alone
    // do not prove ownership. Only the experimentally observed baseline is accepted.
    public static bool MayRestore(ModalFocusState baseline, ModalFocusState current, bool exclusiveLease)
    {
        if (!exclusiveLease || baseline.ManagerId == 0 || baseline.PlayerId == 0 ||
            baseline.ManagerId != current.ManagerId || baseline.PlayerId != current.PlayerId ||
            baseline.Stack == null || current.Stack == null || baseline.Stack.Length != current.Stack.Length ||
            baseline.PopupActive || !baseline.CanMove || baseline.Typing || baseline.MouseFreed ||
            current.PopupActive || current.CanMove || current.Typing || !current.MouseFreed) return false;
        for (int i = 0; i < baseline.Stack.Length; i++) if (baseline.Stack[i] != current.Stack[i]) return false;
        return true;
    }
    // Necessary, not sufficient: shared cursor/movement booleans cannot prove
    // that another owner has not acquired an identical state in the meantime.
    public static bool OwnsTop(int[]? before, int[] current, int expectedManager,
        int currentManager, int expectedOwned, int currentTop, int currentOwned)
    {
        if (before == null || current == null || expectedManager == 0 || expectedOwned == 0 ||
            expectedManager != currentManager || currentTop != expectedOwned || currentOwned != expectedOwned ||
            current.Length != before.Length + 1 || current[0] != expectedOwned) return false;
        for (int i = 0; i < before.Length; i++) if (current[i + 1] != before[i]) return false;
        return true;
    }
}

// The host must not mutate foreign windows and must roll back an unreturned
// partial window. A lease closes only its own still-current window and focus.
// Null result denotes the host's native Back/Escape cancellation.
public interface IDoorModalLease { void Close(); }
public interface IDoorModalHost
{
    bool TryOpen(string title, string[] options, Action<int?> result, out IDoorModalLease? lease);
}

// Intentionally not wired to the game until host ownership is verified.
public sealed class DoorSelector : IDoorSelector
{
    private readonly IDoorModalHost _host;
    private IDoorModalLease? _lease;
    private long _ticket;
    private bool _opening;

    public DoorSelector(IDoorModalHost host) => _host = host ?? throw new ArgumentNullException(nameof(host));

    public bool TryShow(Action<DoorAction> chosen)
    {
        if (chosen == null) throw new ArgumentNullException(nameof(chosen));
        if (_lease != null || _opening) return false;
        long ticket = ++_ticket;
        bool accepted = false, premature = false;
        IDoorModalLease? lease = null;
        _opening = true;
        try
        {
            bool opened = _host.TryOpen("Rathaus", new[] { "Tür öffnen", "Anklopfen", "Abbrechen" }, index =>
            {
                if (ticket != _ticket) return;
                if (!accepted) { premature = true; return; }
                ++_ticket;
                var completedLease = _lease;
                _lease = null;
                try { completedLease?.Close(); }
                catch { return; }
                chosen(index switch { 0 => DoorAction.Open, 1 => DoorAction.Knock, _ => DoorAction.Cancel });
            }, out lease);
            if (!opened || lease == null || premature)
            {
                ++_ticket;
                var failedLease = lease;
                lease = null;
                failedLease?.Close();
                return false;
            }
            _lease = lease;
            accepted = true;
            return true;
        }
        catch
        {
            ++_ticket;
            try { lease?.Close(); } catch { }
            return false;
        }
        finally { _opening = false; }
    }

    public void Close()
    {
        ++_ticket;
        var lease = _lease;
        _lease = null;
        lease?.Close();
    }
}

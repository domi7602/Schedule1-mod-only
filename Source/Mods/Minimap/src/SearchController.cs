using System;

namespace Minimap;

/// <summary>Visible search state for the HUD. Derived only from observations and explicit commands.</summary>
public enum SearchPhase
{
    Disabled,
    Checking,
    Unavailable,
    Idle,
    Searching,
    AlreadyFound,
    Completed,
}

/// <summary>Result of an accept attempt. Only Accepted changes state.</summary>
public enum SearchAcceptStatus
{
    Accepted,
    Disabled,
    NotReady,
    IdentityUnknown,
    CollectedNotFalse,
    AreaInvalid,
    TargetNotCovered,
}

/// <summary>Pure, immutable presentation snapshot. Revision changes only when the visible state changes.</summary>
public readonly struct SearchSnapshot
{
    internal SearchSnapshot(long revision, SearchPhase phase, bool accepted, SearchIdentity identity, bool hasCompletion)
    {
        Revision = revision;
        Phase = phase;
        Accepted = accepted;
        Identity = identity;
        HasCompletion = hasCompletion;
    }

    public long Revision { get; }

    public SearchPhase Phase { get; }

    public bool Accepted { get; }

    public SearchIdentity Identity { get; }

    public bool HasCompletion { get; }
}

/// <summary>One-shot, world-generic completion signal; no personal finder attribution.</summary>
public readonly struct SearchCompletion
{
    internal SearchCompletion(string world, string target)
    {
        World = world;
        Target = target;
    }

    public string World { get; }

    public string Target { get; }
}

/// <summary>
/// Unity-free, HUD-independent state machine for the optional sewer-key search.
/// The runtime adapter feeds observations; the controller decides the visible state and
/// emits one world-generic completion signal per continuously observed collection transition,
/// never on a baseline after a load or gap. Signals require a continuously valid
/// false-to-true collection transition while an accepted search is active. It never derives
/// world/target identity and never derives the fixed area from the pickup position.
/// </summary>
public sealed class SearchController
{
    private readonly SearchArea _area;
    private SearchObservation _last = SearchObservation.Unknown;
    private bool _enabled;
    private bool _accepted;
    private bool? _collected;
    private bool? _baseline;
    private bool _completed;
    private bool _completionPending;
    private SearchCompletion _completion;
    private bool _hasContext;
    private SearchIdentity _context;
    private SearchRecord? _record;
    private SearchRecord? _resumeRecord;

    private long _revision;
    private bool _sigEnabled;
    private SearchPhase _sigPhase;
    private bool _sigAccepted;
    private bool _sigCompletionPending;
    private SearchIdentity _sigIdentity;

    public SearchController(SearchArea area) => _area = area ?? throw new ArgumentNullException(nameof(area));

    public SearchArea Area => _area;

    public bool Enabled => _enabled;

    /// <summary>Increments on every load and on disable/re-enable.</summary>
    public int Epoch { get; private set; }

    /// <summary>Presentation revision is not an IO generation; acceptance actions persist RetainedRecord explicitly.</summary>
    public SearchSnapshot Snapshot => new(_revision, CurrentPhase(), _accepted, _context, _completionPending);

    /// <summary>
    /// Record bound to the current observed context. Null during a load is not a deletion
    /// request; persistence must use RetainedRecord and its already-bound world/path.
    /// </summary>
    public SearchRecord? CurrentRecord => _record;

    /// <summary>Immutable personal intent, including intent awaiting a fresh world observation.</summary>
    public SearchRecord? RetainedRecord => _record ?? _resumeRecord;

    public void Enable()
    {
        if (_enabled)
            return;
        _enabled = true;
        NewEpoch();
    }

    public void Disable()
    {
        if (!_enabled)
            return;
        _enabled = false;
        NewEpoch();
    }

    public void OnLoad() => NewEpoch();

    public void Observe(in SearchObservation observation)
    {
        if (!_enabled)
        {
            _last = observation;
            return;
        }

        if (!observation.Identity.IsKnown)
        {
            // Unknown identity: preserve accepted intent in memory, break continuity, no success.
            _last = observation;
            _baseline = null;
            _completionPending = false;
            Refresh();
            return;
        }

        if (_hasContext && !_context.Equals(observation.Identity))
            ClearContext();

        _context = observation.Identity;
        _hasContext = true;
        if (_resumeRecord is not null
            && (!string.Equals(_resumeRecord.World, _context.World, StringComparison.Ordinal)
                || !string.Equals(_resumeRecord.Target, _context.Target, StringComparison.Ordinal)))
            _resumeRecord = null;
        _last = observation;

        if (!observation.Ready || observation.Collected is null
            || (observation.Collected == false && (!observation.TargetKnown || !_area.IsValid
                || !_area.ContainsTarget(observation.TargetX, observation.TargetY, observation.TargetZ))))
        {
            // Gap: break continuity so a later true cannot earn a toast.
            _baseline = null;
            _completionPending = false;
            Refresh();
            return;
        }

        bool collected = observation.Collected.Value;
        if (!collected && _collected == true)
        {
            // A rollback is authoritative even if its load callback was unavailable.
            _completed = false;
            _completionPending = false;
        }
        bool transition = _baseline == false && collected;
        _baseline = collected;
        _collected = collected;

        if (transition && _accepted && !_completed)
        {
            _completed = true;
            _completionPending = true;
            _completion = new SearchCompletion(observation.Identity.World, observation.Identity.Target);
        }

        if (_resumeRecord is not null)
        {
            SearchRecord pending = _resumeRecord;
            if (Restore(pending)) _resumeRecord = null;
        }
        Refresh();
    }

    /// <summary>
    /// Accept the search for the latest observation. Requires readiness, a reliable false
    /// collection state, a known identity, a valid fixed area and a target covered by that
    /// area. Accepting twice is idempotent (no new revision, no new record).
    /// </summary>
    public SearchAcceptStatus Accept()
    {
        if (!_enabled)
            return SearchAcceptStatus.Disabled;

        SearchObservation observation = _last;
        if (!observation.Ready)
            return SearchAcceptStatus.NotReady;
        if (!observation.Identity.IsKnown)
            return SearchAcceptStatus.IdentityUnknown;
        if (observation.Collected != false)
            return SearchAcceptStatus.CollectedNotFalse;
        if (!_area.IsValid)
            return SearchAcceptStatus.AreaInvalid;
        if (!observation.TargetKnown
            || !_area.ContainsTarget(observation.TargetX, observation.TargetY, observation.TargetZ))
        {
            return SearchAcceptStatus.TargetNotCovered;
        }

        if (_accepted && _record is not null)
            return SearchAcceptStatus.Accepted;

        _context = observation.Identity;
        _hasContext = true;
        _accepted = true;
        _record = new SearchRecord(observation.Identity.World, observation.Identity.Target, _area.Id, _area.Version, accepted: true);
        Refresh();
        return SearchAcceptStatus.Accepted;
    }

    /// <summary>Clear the accepted flag but keep the area and identity stable, so a reaccept reuses them.</summary>
    public void Cancel()
    {
        if (!_accepted && RetainedRecord?.Accepted != true && !_completionPending && !_completed)
            return;
        _accepted = false;
        _completed = false;
        _completionPending = false;
        _completion = default;
        if (_record is not null)
            _record = new SearchRecord(_record.World, _record.Target, _record.AreaId, _record.AreaVersion, accepted: false);
        if (_resumeRecord is not null)
            _resumeRecord = new SearchRecord(_resumeRecord.World, _resumeRecord.Target, _resumeRecord.AreaId, _resumeRecord.AreaVersion, accepted: false);
        Refresh();
    }

    /// <summary>
    /// Reopen a persisted accepted search when the record matches the current known world,
    /// target, area id and area version. Preserve a stored cancellation; an older native
    /// save with an uncollected key reopens only a genuinely accepted record.
    /// </summary>
    public bool Restore(SearchRecord record)
    {
        if (!_enabled || record is null)
            return false;
        if (!_hasContext || !_context.IsKnown || !_last.Ready || _last.Collected is null
            || !_last.Identity.Equals(_context) || !_area.IsValid)
            return false;
        if (_last.Collected == false && (!_last.TargetKnown
            || !_area.ContainsTarget(_last.TargetX, _last.TargetY, _last.TargetZ)))
            return false;
        if (!string.Equals(record.World, _context.World, StringComparison.Ordinal))
            return false;
        if (!string.Equals(record.Target, _context.Target, StringComparison.Ordinal))
            return false;
        if (!string.Equals(record.AreaId, _area.Id, StringComparison.Ordinal))
            return false;
        if (record.AreaVersion != _area.Version)
            return false;

        _accepted = record.Accepted;
        _record = new SearchRecord(record.World, record.Target, record.AreaId, record.AreaVersion, record.Accepted);
        _resumeRecord = null;
        if (!record.Accepted)
        {
            _completed = false;
            _completionPending = false;
            _completion = default;
        }
        Refresh();
        return true;
    }

    /// <summary>Explicitly consume the pending completion signal. Returns false when there is none.</summary>
    public bool TryConsumeCompletion(out SearchCompletion completion)
    {
        completion = default;
        if (!_completionPending)
            return false;
        completion = _completion;
        _completionPending = false;
        Refresh();
        return true;
    }

    private void NewEpoch()
    {
        Epoch++;
        SearchRecord? resume = _record ?? _resumeRecord;
        ClearContext();
        _resumeRecord = resume;
        _last = SearchObservation.Unknown;
        _completed = false;
        _completionPending = false;
        _completion = default;
        Refresh();
    }

    private void ClearContext()
    {
        _completed = false;
        _completionPending = false;
        _completion = default;
        _collected = null;
        _baseline = null;
        _accepted = false;
        _record = null;
        _resumeRecord = null;
        _hasContext = false;
        _context = default;
    }

    private SearchPhase CurrentPhase()
    {
        if (!_enabled)
            return SearchPhase.Disabled;
        if (!_last.Ready || !_last.Identity.IsKnown || _last.Collected is null)
            return SearchPhase.Checking;
        if (_last.Collected == false && !_last.TargetKnown)
            return SearchPhase.Checking;
        if (_last.Collected == false && (!_area.IsValid
            || !_area.ContainsTarget(_last.TargetX, _last.TargetY, _last.TargetZ)))
            return SearchPhase.Unavailable;
        if (_collected == true)
            return _completed ? SearchPhase.Completed : SearchPhase.AlreadyFound;
        if (_accepted)
            return SearchPhase.Searching;
        return SearchPhase.Idle;
    }

    private void Refresh()
    {
        SearchPhase phase = CurrentPhase();
        if (phase == _sigPhase
            && _accepted == _sigAccepted
            && _completionPending == _sigCompletionPending
            && _enabled == _sigEnabled
            && _context.Equals(_sigIdentity))
        {
            return;
        }

        _revision++;
        _sigPhase = phase;
        _sigAccepted = _accepted;
        _sigCompletionPending = _completionPending;
        _sigEnabled = _enabled;
        _sigIdentity = _context;
    }
}

using System;

namespace TaxiDriver;

/// <summary>
/// Pure display arbitration between a short-lived local override (a button outcome for a
/// few seconds) and the persistent <see cref="SpikeState.ServiceStatus"/> feedback.
///
/// A newer, non-empty service status supersedes an active override immediately, so an
/// asynchronous failure can never be masked by a stale success message — while an
/// override that is armed afterwards (deliberate UI feedback, or the protected STOP
/// prompt) still shows normally. Empty statuses and unchanged revisions never discard an
/// override. All time inputs are validated fail-closed, so an invalid clock shows the
/// persistent status instead of an override.
/// </summary>
internal sealed class ServiceFeedback
{
    private string _overrideText = string.Empty;
    private float _overrideUntil;
    private long _appliedRevision;

    /// <summary>Arms a local override. An invalid or negative <paramref name="now"/> arms nothing.</summary>
    internal void ArmOverride(string text, float now, float seconds)
    {
        _overrideText = text ?? string.Empty;
        _overrideUntil = _overrideText.Length > 0 && IsValidTime(now)
            ? now + Math.Max(0f, seconds)
            : 0f;
    }

    internal void ClearOverride()
    {
        _overrideText = string.Empty;
        _overrideUntil = 0f;
    }

    /// <summary>True while a local override is armed (regardless of its expiry).</summary>
    internal bool HasOverride => _overrideText.Length > 0;

    /// <summary>
    /// The text to display: the active override while it is armed and unexpired, otherwise
    /// the service status. A new non-empty status (new revision) supersedes an unprotected
    /// override once, marking that revision as consumed.
    /// </summary>
    internal string Resolve(float now, string serviceStatus, long statusRevision, bool overrideProtected)
    {
        if (statusRevision != _appliedRevision)
        {
            _appliedRevision = statusRevision;
            if (!overrideProtected && serviceStatus.Length > 0)
                ClearOverride();
        }

        if (_overrideText.Length > 0 && IsValidTime(now) && now < _overrideUntil)
            return _overrideText;

        return serviceStatus;
    }

    private static bool IsValidTime(float value) => !float.IsNaN(value) && !float.IsInfinity(value) && value >= 0f;
}

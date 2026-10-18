namespace TaxiDriver;

/// <summary>Operation-local stall history; independent of native navigation orders.</summary>
internal sealed class RecoveryRecurrencePolicy
{
    // A reverse can move several metres while leaving the same obstruction unresolved.
    internal const float StallRadiusMeters = 6f;
    internal const float ForwardApproachMeters = 4f;
    private float _bestDistance, _progressMark;
    private bool _hasStall;
    private float _stallX, _stallZ;

    internal void Begin(float distance)
    {
        _hasStall = false;
        _bestDistance = _progressMark = float.IsFinite(distance) && distance >= 0f
            ? distance : float.PositiveInfinity;
    }

    // Fixed operation target, not a redispatch-local route/goal. Ineligible movement
    // consumes only newly acquired ground, so returning forward cannot earn it again.
    internal bool ObserveProgress(float distance, bool eligibleForward)
    {
        if (!float.IsFinite(distance) || distance < 0f) return false;
        if (float.IsPositiveInfinity(_bestDistance))
        {
            _bestDistance = _progressMark = distance;
            return false;
        }
        float previousBest = _bestDistance;
        _bestDistance = System.Math.Min(_bestDistance, distance);
        if (!eligibleForward)
        {
            _progressMark -= previousBest - _bestDistance;
            return false;
        }
        if (_progressMark - _bestDistance < ForwardApproachMeters) return false;
        _progressMark = _bestDistance;
        _hasStall = false;
        return true;
    }

    internal bool RegisterStall(float x, float z)
    {
        if (!float.IsFinite(x) || !float.IsFinite(z)) return false;
        float dx = x - _stallX, dz = z - _stallZ;
        bool recurring = _hasStall && dx * dx + dz * dz <= StallRadiusMeters * StallRadiusMeters;
        // Escape credit must be earned after this stall, not borrowed from the
        // preceding ordinary drive. Keep closest-ever ground across redispatch.
        _progressMark = _bestDistance;
        if (!recurring)
        {
            _stallX = x;
            _stallZ = z;
            _hasStall = true;
        }
        return recurring;
    }
}

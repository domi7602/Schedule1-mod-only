using System;

namespace TaxiDriver;

/// <summary>Two-press destructive action gate; no Unity objects or wall-clock dependencies.</summary>
internal sealed class StopConfirmation
{
    internal const float ConfirmSeconds = 3f;
    private bool _armed;
    private float _until;
    private float _armedAt;
    private long _rideId;

    internal void Reset()
    {
        _armed = false;
        _until = 0f;
        _armedAt = 0f;
        _rideId = 0;
    }

    internal bool Synchronize(bool appOpen, bool rideActive, long rideId)
    {
        if (!_armed || (appOpen && rideActive && rideId == _rideId))
            return false;
        Reset();
        return true;
    }

    internal bool Press(bool rideActive, long rideId, float now)
    {
        if (!float.IsFinite(now) || now < 0f)
        {
            Reset();
            return false;
        }
        if (!rideActive)
        {
            Reset();
            return true;
        }
        if (_armed && rideId == _rideId && now >= _armedAt && now < _until)
        {
            Reset();
            return true;
        }
        _armed = true;
        _rideId = rideId;
        _armedAt = now;
        _until = now + ConfirmSeconds;
        return false;
    }
}

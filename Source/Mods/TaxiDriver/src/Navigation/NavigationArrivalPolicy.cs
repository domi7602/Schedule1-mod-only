using System;

namespace TaxiDriver;

/// <summary>The resolved destination has one arrival radius for patrol and ride verdicts.</summary>
internal static class NavigationArrivalPolicy
{
    internal const float ArrivalMeters = 10f;
    internal const float StandingKmh = 1f;

    internal static bool IsWithinArrival(float distance) =>
        float.IsFinite(distance) && distance >= 0f && distance <= ArrivalMeters;

    internal static bool IsStandingAtArrival(bool atLastWaypoint, float distance, float signedSpeed) =>
        atLastWaypoint && IsWithinArrival(distance) && Math.Abs(signedSpeed) <= StandingKmh;
}

using System;

namespace TaxiDriver;

[Flags]
internal enum RideProtectionPatch
{
    None = 0,
    Throttle = 1 << 0,
    Steering = 1 << 1,
    Handbrake = 1 << 2,
    TrunkOpen = 1 << 3,
    TrunkSetOpen = 1 << 4,
}

/// <summary>Pure fail-closed policy for the patches required before a passenger ride.</summary>
internal static class RideProtectionPolicy
{
    internal const RideProtectionPatch Required =
        RideProtectionPatch.Throttle |
        RideProtectionPatch.Steering |
        RideProtectionPatch.Handbrake |
        RideProtectionPatch.TrunkOpen |
        RideProtectionPatch.TrunkSetOpen;

    internal static bool IsComplete(RideProtectionPatch installed) =>
        (installed & Required) == Required;

    /// <summary>
    /// Fail closed while a passenger ride is active: if the currently controlled
    /// vehicle cannot be proven to be the owned taxi, do not pass player input
    /// through. When the owned taxi's agent override is active, leave that
    /// separate AI-control path untouched.
    /// </summary>
    internal static bool ShouldBlockPlayerInput(
        bool passengerRideMode,
        bool ownedRideVehicleVerified,
        bool overrideControls) =>
        passengerRideMode && (!ownedRideVehicleVerified || !overrideControls);

    /// <summary>The handbrake is a player input path, never an agent-override path.</summary>
    internal static bool ShouldBlockHandbrake(bool passengerRideMode) => passengerRideMode;

    /// <summary>
    /// During a passenger ride, block the known taxi trunk and fail closed if
    /// either the taxi's trunk or the candidate door cannot be identified.
    /// Other storage doors remain usable only when both identities are valid.
    /// </summary>
    internal static bool ShouldBlockTrunkOpen(
        bool passengerRideMode,
        bool ownedTrunkVerified,
        bool doorIdentityVerified,
        bool isRideTrunk) =>
        passengerRideMode && (!ownedTrunkVerified || !doorIdentityVerified || isRideTrunk);

    internal static RideProtectionPatch Missing(RideProtectionPatch installed) =>
        Required & ~installed;
}

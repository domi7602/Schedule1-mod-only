namespace TaxiDriver;

/// <summary>Final currentThrottle request at ApplyThrottle, not a gear output.
/// Ordinary AI/player inputs are restored after the call; no render Update hold.</summary>
internal static class ReversePedalPolicy
{
    internal const float PedalInput = -0.7f;
    internal static float CurrentThrottleFor(bool reverse) => reverse ? PedalInput : 0f;
}

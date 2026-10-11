using System;

namespace TaxiDriver;

/// <summary>Native identity gate for a genuine EnterVehicle attempt; Unity-free for tests.</summary>
internal sealed class DriverBoardingGate
{
    internal const float CooldownSeconds = 15f;
    private IntPtr _lastAttemptedNpc;
    private float _lastAttemptAt;

    internal static bool IsDedicatedDriver(IntPtr candidate, IntPtr dedicated) =>
        candidate != IntPtr.Zero && dedicated != IntPtr.Zero && candidate == dedicated;

    internal float RemainingSeconds(IntPtr candidate, float now) =>
        candidate == _lastAttemptedNpc && candidate != IntPtr.Zero
            ? Math.Max(0f, CooldownSeconds - (now - _lastAttemptAt))
            : 0f;

    /// <summary>Commit BEFORE the native call: even a throwing/failed attempt starts the cooldown.</summary>
    internal bool TryBeginEntry(IntPtr candidate, IntPtr dedicated, float now)
    {
        if (!float.IsFinite(now) || now < 0f || !IsDedicatedDriver(candidate, dedicated) || RemainingSeconds(candidate, now) > 0f)
            return false;
        _lastAttemptedNpc = candidate;
        _lastAttemptAt = now;
        return true;
    }
}

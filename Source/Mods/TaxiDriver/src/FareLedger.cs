using System;

namespace TaxiDriver;

/// <summary>
/// Pure, Unity-free fare accrual state machine, extracted from
/// <c>FareMeter.Tick</c> on 2026-10-02 so the billing rules are unit-testable
/// without the game (<c>Source/Tests/TaxiDriver.Tests</c>). Behavior-preserving:
/// all time values are inputs, all Unity/game access stays in FareMeter, and the
/// returned <see cref="TickOutcome"/> drives the same log lines and payment.
///
/// Rules (unchanged from FareMeter):
/// <list type="bullet">
/// <item>Standing is free; only whole moving in-game minutes are billed.</item>
/// <item>The first moving frame after entering motion does not accrue.</item>
/// <item>Pauses, clock stops and dead speed handles reset the motion latch and accrue nothing.</item>
/// <item>Raw frame deltas are capped at <see cref="MaxDeltaSeconds"/> (hitch protection).</item>
/// <item>Billing is committed before payment so a failed payment is never retried.</item>
/// </list>
/// </summary>
internal sealed class FareLedger
{
    /// <summary>Hitch cap: a freeze (e.g. 2 s) must never bill more than half a second of motion.</summary>
    public const float MaxDeltaSeconds = 0.5f;

    private double _movingMinutes;
    private int _billedMinutes;
    private bool _wasMoving;
    private bool _armed;
    private bool _paused;

    /// <summary>True while the meter is in a paused (timeScale 0) state.</summary>
    public bool Paused => _paused;

    /// <summary>Accumulated moving in-game minutes (not yet floored).</summary>
    public double MovingMinutes => _movingMinutes;

    /// <summary>Whole minutes already billed; a re-tick can never double-charge below this.</summary>
    public int BilledMinutes => _billedMinutes;

    /// <summary>Motion latch from the previous tick (first moving frame is not counted).</summary>
    public bool WasMoving => _wasMoving;

    /// <summary>True once the first confirmed motion of the ride was seen.</summary>
    public bool Armed => _armed;

    private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

    /// <summary>Clears all accrual state for a new ride (FareMeter.Start).</summary>
    public void Reset()
    {
        _movingMinutes = 0;
        _billedMinutes = 0;
        _wasMoving = false;
        _armed = false;
        _paused = false;
    }

    /// <summary>
    /// Advances the accrual state machine by one frame. Mirrors the former
    /// <c>FareMeter.Tick</c> branch order exactly; the caller maps the outcome
    /// to logging and payment.
    /// </summary>
    public TickOutcome Tick(in TickInput input)
    {
        var outcome = new TickOutcome();

        if (input.TimePaused)
        {
            outcome.JustPaused = !_paused;
            _paused = true;
            _wasMoving = false;
            return outcome;
        }

        outcome.JustResumed = _paused;
        _paused = false;

        // Same early-return set as the old code: ride gone, clock unavailable,
        // meter disabled/zero rate, or a dead speed handle are all free and
        // reset the motion latch (the first frame after recovery never accrues).
        if (!input.RideValid || !input.ClockAvailable || !input.ConfigEnabled ||
            input.DollarsPerMinute <= 0 || input.SpeedReadFailed)
        {
            _wasMoving = false;
            return outcome;
        }

        // Numeric hardening (2026-10-03): reject non-finite tick input BEFORE any
        // state mutation - a poisoned accumulator can never be repaired (the old
        // behavior let NaN pass through Math.Clamp and corrupted the ride total).
        if (!IsFinite(input.RawDeltaSeconds) || !IsFinite(input.ClockRate))
            return outcome;

        float dt = Math.Clamp(input.RawDeltaSeconds, 0f, MaxDeltaSeconds) * input.ClockRate;
        if (!IsFinite(dt))
            return outcome;

        bool moving = !float.IsNaN(input.SpeedKmh) && !float.IsInfinity(input.SpeedKmh) &&
                      input.SpeedKmh > 0f && input.SpeedKmh >= input.MovingThresholdKmh;

        if (moving && !_armed)
        {
            _armed = true;
            outcome.ArmedNow = true;
        }

        if (moving != _wasMoving)
            outcome.MovingChangedTo = moving;

        bool countInterval = moving && _wasMoving;
        _wasMoving = moving;
        if (!countInterval)
            return outcome; // standing is free

        _movingMinutes += dt;
        outcome.Counted = true;

        // Floor TIME first (rate=2 must not charge $1 after 0.5 s).
        int wholeMinutes = (int)Math.Floor(_movingMinutes);
        int units = wholeMinutes - _billedMinutes;
        int due = 0;
        if (units > 0)
        {
            // Numeric hardening (2026-10-03): the old checked() threw AFTER the
            // accumulator moved (line 106) and BEFORE the billed update, leaving a
            // half-advanced state that threw on every following tick. Compute in
            // long and clamp instead: the tick completes and the ledger stays
            // consistent (billed == floored moving minutes).
            long rawDue = (long)units * input.DollarsPerMinute;
            due = rawDue > int.MaxValue ? int.MaxValue : (int)rawDue;
        }
        if (due > 0)
        {
            // Mark consumed before the caller touches money: a failed/partially
            // applied payment must never be retried every subsequent frame.
            _billedMinutes = wholeMinutes;
            outcome.DueDollars = due;
        }

        outcome.WholeMinutes = wholeMinutes;
        return outcome;
    }

    /// <summary>Inputs for one frame; everything is derived by the caller from Unity/game state.</summary>
    internal readonly struct TickInput
    {
        /// <summary><c>Time.timeScale == 0f</c>.</summary>
        public bool TimePaused { get; init; }

        /// <summary><c>SpikeState.RideActive &amp;&amp; ReferenceEquals(veh, SpikeState.Vehicle)</c>.</summary>
        public bool RideValid { get; init; }

        /// <summary>Raw frame delta in scaled seconds; capped internally.</summary>
        public float RawDeltaSeconds { get; init; }

        /// <summary>True when the game clock provided a valid rate this frame.</summary>
        public bool ClockAvailable { get; init; }

        /// <summary>Game minutes per scaled second (see <see cref="FareClock.TryComputeRate"/>).</summary>
        public float ClockRate { get; init; }

        /// <summary>fare.json <c>Enabled</c>.</summary>
        public bool ConfigEnabled { get; init; }

        /// <summary>fare.json <c>DollarsPerInGameMinute</c>.</summary>
        public int DollarsPerMinute { get; init; }

        /// <summary>True when reading <c>Speed_Kmh</c> threw (dead handle).</summary>
        public bool SpeedReadFailed { get; init; }

        /// <summary>Absolute speed in km/h; ignored when <see cref="SpeedReadFailed"/>.</summary>
        public float SpeedKmh { get; init; }

        /// <summary>fare.json <c>MovingSpeedThresholdKmh</c>.</summary>
        public float MovingThresholdKmh { get; init; }
    }

    /// <summary>What one tick did; the caller turns this into the same log lines as before.</summary>
    internal struct TickOutcome
    {
        /// <summary>True on the first paused frame only.</summary>
        public bool JustPaused { get; set; }

        /// <summary>True on the first frame after a pause.</summary>
        public bool JustResumed { get; set; }

        /// <summary>True on the first confirmed motion of the ride.</summary>
        public bool ArmedNow { get; set; }

        /// <summary>Set when the moving/standing state flipped; the value is the new state.</summary>
        public bool? MovingChangedTo { get; set; }

        /// <summary>True when the interval was counted (moving follow-up frame).</summary>
        public bool Counted { get; set; }

        /// <summary>Whole dollars due now (0 = nothing to charge).</summary>
        public int DueDollars { get; set; }

        /// <summary>Floored moving minutes after this tick.</summary>
        public int WholeMinutes { get; set; }
    }
}

/// <summary>
/// Threshold validation/migration rules from the fare.json loader
/// (FareMeter.Config). Pure so the invalid/legacy branches stay under test.
/// </summary>
internal static class FareConfigRules
{
    /// <summary>Default moving threshold in km/h.</summary>
    public const float DefaultThresholdKmh = 3f;

    /// <summary>Values at or below this are the legacy creep threshold (&lt;= 0.5 km/h).</summary>
    public const float LegacyThresholdMaxKmh = 0.5f;

    /// <summary>
    /// True for NaN/Infinity or anything below 0.1 km/h — reset to the default
    /// WITHOUT rewriting the file (the legacy migration below handles and logs
    /// real rewrites).
    /// </summary>
    public static bool IsInvalidThreshold(float value) =>
        float.IsNaN(value) || float.IsInfinity(value) || value < 0.1f;

    /// <summary>True for the legacy creep threshold that counted parking-speed crawl as motion.</summary>
    public static bool IsLegacyThreshold(float value) => value <= LegacyThresholdMaxKmh;

    /// <summary>
    /// Package 8: migration decision (legacy first, invalid second), pure so the
    /// matrix stays under test. Legacy (even below 0.1) and invalid values both
    /// repair to the default; <paramref name="changed"/> tells the caller to
    /// rewrite the file so a second load is migration-free.
    /// </summary>
    public static float Migrate(float value, out bool changed, out bool wasLegacy)
    {
        wasLegacy = IsLegacyThreshold(value);
        if (wasLegacy || IsInvalidThreshold(value))
        {
            changed = true;
            return DefaultThresholdKmh;
        }

        changed = false;
        return value;
    }
}

/// <summary>
/// Pure game-clock conversion (TimeManager CycleDuration/TimeSpeedMultiplier →
/// in-game minutes per scaled second). Validation mirrors the former inline
/// checks; a false return makes the caller throw/log exactly as before.
/// </summary>
internal static class FareClock
{
    /// <summary>
    /// <c>1440 / (cycle * 60) * speed</c>; false when either input is
    /// NaN/Infinity/&lt;= 0.
    /// </summary>
    public static bool TryComputeRate(float cycleRealMinutesPerDay, float speedMultiplier, out float rate)
    {
        rate = 0f;
        if (!IsFinitePositive(cycleRealMinutesPerDay) || !IsFinitePositive(speedMultiplier))
            return false;

        rate = 1440f / (cycleRealMinutesPerDay * 60f) * speedMultiplier;
        if (!IsFinitePositive(rate))
        {
            // Numeric hardening (2026-10-03): finite positive inputs can still
            // overflow the computed rate - a useless result is rejected, not used.
            rate = 0f;
            return false;
        }

        return true;
    }

    private static bool IsFinitePositive(float value) =>
        !float.IsNaN(value) && !float.IsInfinity(value) && value > 0f;
}

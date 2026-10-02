using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Tests for the Unity-free fare accrual state machine extracted from
/// FareMeter (2026-10-02). They pin the billing rules of the in-game meter:
/// standing is free, only whole moving minutes are billed, the first moving
/// frame and the first frame after any interruption never accrue, pauses and
/// clock stops are free, and raw deltas are hitch-capped.
/// </summary>
public class FareLedgerTests
{
    private const float DefaultThreshold = 3f;

    private static FareLedger.TickInput Moving(
        float delta = 0.2f,
        float speed = 30f,
        float clockRate = 1f,
        int rate = 1,
        float threshold = DefaultThreshold) => new()
        {
            TimePaused = false,
            RideValid = true,
            RawDeltaSeconds = delta,
            ClockAvailable = true,
            ClockRate = clockRate,
            ConfigEnabled = true,
            DollarsPerMinute = rate,
            SpeedReadFailed = false,
            SpeedKmh = speed,
            MovingThresholdKmh = threshold,
        };

    private static FareLedger.TickInput Standing(float delta = 0.2f) => new()
    {
        RideValid = true,
        RawDeltaSeconds = delta,
        ClockAvailable = true,
        ClockRate = 1f,
        ConfigEnabled = true,
        DollarsPerMinute = 1,
        SpeedKmh = 0f,
        MovingThresholdKmh = DefaultThreshold,
    };

    [Fact]
    public void FreshLedger_IsIdle()
    {
        var ledger = new FareLedger();

        Assert.False(ledger.Paused);
        Assert.False(ledger.WasMoving);
        Assert.False(ledger.Armed);
        Assert.Equal(0d, ledger.MovingMinutes);
        Assert.Equal(0, ledger.BilledMinutes);
    }

    [Fact]
    public void StandingFrames_AreFree()
    {
        var ledger = new FareLedger();

        for (int i = 0; i < 10; i++)
        {
            FareLedger.TickOutcome outcome = ledger.Tick(Standing());

            Assert.Equal(0, outcome.DueDollars);
            Assert.False(outcome.Counted);
        }

        Assert.Equal(0d, ledger.MovingMinutes);
        Assert.False(ledger.Armed);
    }

    [Fact]
    public void FirstMovingFrame_IsNotCounted()
    {
        var ledger = new FareLedger();

        FareLedger.TickOutcome outcome = ledger.Tick(Moving());

        Assert.False(outcome.Counted);
        Assert.True(outcome.ArmedNow);
        Assert.Equal(0d, ledger.MovingMinutes);
    }

    [Fact]
    public void MovingTime_AccruesWholeMinutes_AndChargesPerUnit()
    {
        var ledger = new FareLedger();

        ledger.Tick(Moving(0.2f)); // arming frame, not counted
        int due = 0;
        for (int i = 0; i < 10; i++)
        {
            due += ledger.Tick(Moving(0.2f)).DueDollars;
        }

        // 10 counted frames x 0.2 game minutes = 2.0 -> $2 at $1/unit.
        Assert.Equal(2, due);
        Assert.Equal(2, ledger.BilledMinutes);
        Assert.Equal(2d, ledger.MovingMinutes, 3);
    }

    [Fact]
    public void ThresholdBoundary_CountsAsMoving()
    {
        var ledger = new FareLedger();

        ledger.Tick(Moving(speed: DefaultThreshold));
        FareLedger.TickOutcome second = ledger.Tick(Moving(speed: DefaultThreshold));

        Assert.True(second.Counted);
    }

    [Fact]
    public void BelowThreshold_IsStanding()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving()); // arm the motion latch first

        FareLedger.TickOutcome outcome = ledger.Tick(Moving(speed: 2.99f));

        Assert.False(outcome.Counted);
        Assert.Equal(false, outcome.MovingChangedTo);
        Assert.False(ledger.WasMoving);
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void NonFiniteSpeed_IsStanding(float speed)
    {
        var ledger = new FareLedger();

        ledger.Tick(Moving(speed: speed));
        FareLedger.TickOutcome second = ledger.Tick(Moving(speed: speed));

        Assert.False(second.Counted);
        Assert.False(ledger.Armed);
    }

    [Fact]
    public void DeadSpeedRead_IsFree_AndResetsMotionLatch()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());
        ledger.Tick(Moving()); // now counted once, 0.2 minutes

        var dead = new FareLedger.TickInput
        {
            RideValid = true,
            RawDeltaSeconds = 0.2f,
            ClockAvailable = true,
            ClockRate = 1f,
            ConfigEnabled = true,
            DollarsPerMinute = 1,
            SpeedReadFailed = true,
            SpeedKmh = 30f,
            MovingThresholdKmh = DefaultThreshold,
        };
        FareLedger.TickOutcome failed = ledger.Tick(dead);

        Assert.False(failed.Counted);
        Assert.Equal(0, failed.DueDollars);
        Assert.False(ledger.WasMoving);

        // Recovery frame must not accrue either (latch resets).
        FareLedger.TickOutcome recovered = ledger.Tick(Moving());
        Assert.False(recovered.Counted);
    }

    [Fact]
    public void StoppedIntervals_BreakTheMotionLatch()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());
        ledger.Tick(Moving()); // counted, 0.2 min

        ledger.Tick(Standing()); // latch off, free

        FareLedger.TickOutcome afterStanding = ledger.Tick(Moving());
        Assert.False(afterStanding.Counted);

        FareLedger.TickOutcome next = ledger.Tick(Moving());
        Assert.True(next.Counted);
    }

    [Fact]
    public void Pause_FreezesAndDiscardsPausedTime()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());
        ledger.Tick(Moving()); // counted, 0.2 min

        FareLedger.TickOutcome paused = ledger.Tick(new FareLedger.TickInput { TimePaused = true });
        Assert.True(paused.JustPaused);
        Assert.False(ledger.WasMoving);

        // Long paused frame must not accrue.
        FareLedger.TickOutcome stillPaused = ledger.Tick(new FareLedger.TickInput { TimePaused = true, RawDeltaSeconds = 5f });
        Assert.False(stillPaused.JustPaused);
        Assert.Equal(0, stillPaused.DueDollars);

        FareLedger.TickOutcome resumed = ledger.Tick(Moving());
        Assert.True(resumed.JustResumed);
        Assert.False(resumed.Counted); // first frame after resume is free

        FareLedger.TickOutcome next = ledger.Tick(Moving());
        Assert.True(next.Counted);
        Assert.Equal(0.4d, ledger.MovingMinutes, 3);
    }

    [Fact]
    public void ClockUnavailable_IsFree()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());
        ledger.Tick(Moving()); // counted once

        var input = new FareLedger.TickInput
        {
            RideValid = true,
            RawDeltaSeconds = 0.2f,
            ClockAvailable = false,
            ConfigEnabled = true,
            DollarsPerMinute = 1,
            SpeedKmh = 30f,
            MovingThresholdKmh = DefaultThreshold,
        };
        FareLedger.TickOutcome outcome = ledger.Tick(input);

        Assert.False(outcome.Counted);
        Assert.Equal(0, outcome.DueDollars);
        Assert.False(ledger.WasMoving);
    }

    [Fact]
    public void DisabledConfig_IsFree()
    {
        var ledger = new FareLedger();

        ledger.Tick(Moving());
        var disabled = new FareLedger.TickInput
        {
            RideValid = true,
            RawDeltaSeconds = 0.2f,
            ClockAvailable = true,
            ClockRate = 1f,
            ConfigEnabled = false,
            DollarsPerMinute = 1,
            SpeedKmh = 30f,
            MovingThresholdKmh = DefaultThreshold,
        };
        FareLedger.TickOutcome outcome = ledger.Tick(disabled);

        Assert.False(outcome.Counted);
        Assert.Equal(0d, ledger.MovingMinutes);
    }

    [Fact]
    public void ZeroUnitRate_IsFree()
    {
        var ledger = new FareLedger();

        ledger.Tick(Moving(rate: 0));
        FareLedger.TickOutcome second = ledger.Tick(Moving(rate: 0));

        Assert.False(second.Counted);
        Assert.Equal(0, second.DueDollars);
    }

    [Fact]
    public void HitchDelta_IsCappedAtHalfASecond()
    {
        var ledger = new FareLedger();

        ledger.Tick(Moving(2f)); // arming frame
        ledger.Tick(Moving(2f)); // capped to 0.5 game minutes
        Assert.Equal(0.5d, ledger.MovingMinutes, 3);

        FareLedger.TickOutcome third = ledger.Tick(Moving(2f));
        Assert.Equal(1, third.DueDollars);
        Assert.Equal(1d, ledger.MovingMinutes, 3);
    }

    [Fact]
    public void HigherRate_DoesNotRoundUpBeforeAFullUnit()
    {
        var ledger = new FareLedger();

        ledger.Tick(Moving(rate: 2));
        Assert.Equal(0, ledger.Tick(Moving(rate: 2)).DueDollars); // 0.2
        Assert.Equal(0, ledger.Tick(Moving(rate: 2)).DueDollars); // 0.4
        Assert.Equal(0, ledger.Tick(Moving(rate: 2)).DueDollars); // 0.6
        Assert.Equal(0, ledger.Tick(Moving(rate: 2)).DueDollars); // 0.8

        FareLedger.TickOutcome atOne = ledger.Tick(Moving(rate: 2)); // 1.0
        Assert.Equal(2, atOne.DueDollars);
    }

    [Fact]
    public void Billing_CommitsBeforePayment_SoARetryCannotDoubleCharge()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());

        int total = 0;
        for (int i = 0; i < 5; i++)
        {
            total += ledger.Tick(Moving(clockRate: 2f, rate: 2)).DueDollars; // 5 frames x 0.4 = 2.0 minutes
        }

        Assert.Equal(4, total); // 2 units x $2
        Assert.Equal(2, ledger.BilledMinutes);

        // A further small movement inside the same whole minute must not charge again.
        FareLedger.TickOutcome extra = ledger.Tick(Moving(clockRate: 0.1f));
        Assert.Equal(0, extra.DueDollars);
        Assert.Equal(2, ledger.BilledMinutes);
    }

    [Fact]
    public void NegativeRawDelta_AccruesNothing()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());

        FareLedger.TickOutcome outcome = ledger.Tick(Moving(delta: -1f));

        Assert.True(outcome.Counted);
        Assert.Equal(0, outcome.DueDollars);
        Assert.Equal(0d, ledger.MovingMinutes);
    }

    [Fact]
    public void RideInvalidFrame_IsFree_AndResetsMotionLatch()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());
        ledger.Tick(Moving()); // counted

        var invalid = new FareLedger.TickInput
        {
            RideValid = false,
            RawDeltaSeconds = 0.2f,
            ClockAvailable = true,
            ClockRate = 1f,
            ConfigEnabled = true,
            DollarsPerMinute = 1,
            SpeedKmh = 30f,
            MovingThresholdKmh = DefaultThreshold,
        };
        FareLedger.TickOutcome outcome = ledger.Tick(invalid);

        Assert.False(outcome.Counted);
        Assert.False(ledger.WasMoving);
        Assert.Equal(0.2d, ledger.MovingMinutes, 3);
    }

    [Fact]
    public void ArmedOnce_StaysArmed()
    {
        var ledger = new FareLedger();

        Assert.True(ledger.Tick(Moving()).ArmedNow);
        Assert.False(ledger.Tick(Moving()).ArmedNow);
        Assert.True(ledger.Armed);
    }

    [Fact]
    public void Reset_ClearsAccumulatorAndLatches()
    {
        var ledger = new FareLedger();
        ledger.Tick(Moving());
        ledger.Tick(Moving());
        ledger.Tick(new FareLedger.TickInput { TimePaused = true });

        ledger.Reset();

        Assert.Equal(0d, ledger.MovingMinutes);
        Assert.Equal(0, ledger.BilledMinutes);
        Assert.False(ledger.WasMoving);
        Assert.False(ledger.Armed);
        Assert.False(ledger.Paused);
    }
}

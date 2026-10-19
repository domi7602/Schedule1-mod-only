using System.Numerics;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Executable regressions for the bounded reverse manoeuvre (brief §6.1/§7.1/§7.2/§7.3).
/// The taxi must confirm sustained DIRECTED horizontal reverse movement — an arbitrary
/// 3D displacement, lateral slip, vertical change or jitter is not escape.
/// </summary>
public class ReverseManoeuvrePolicyTests
{
    private static readonly Vector3 Origin = new(10f, 0f, 20f);
    // Reverse direction points backwards (-Z): backing up increases Z.
    private static readonly Vector3 Reverse = new(0f, 0f, 1f);

    private static ReverseManoeuvrePolicy Started(long op = 7, int order = 3, float now = 0f)
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(op, order, Origin, Reverse, now);
        return policy;
    }

    [Fact]
    public void Begin_StartsRequestedAndOwnsTheControls()
    {
        var policy = Started();
        Assert.True(policy.Active);
        Assert.Equal(ReversePhase.Requested, policy.Phase);
        Assert.True(policy.Matches(7, 3));
        Assert.False(policy.MovedConfirmed);
    }

    [Fact]
    public void ArbitraryThreeDimensionalMovement_IsNotDirectedReverseProgress()
    {
        var policy = Started();
        // 0.5 m of pure sideways (X) displacement and 0.5 m of vertical lift:
        // the old code counted either as an escape; the directed axis must not.
        Assert.False(policy.Observe(new Vector3(10.5f, 0f, 20f), true, -0.7f, 0f));
        Assert.False(policy.Observe(new Vector3(10.5f, 0.5f, 20f), true, -0.7f, 0.5f));
        Assert.False(policy.Observe(new Vector3(10.5f, 0.5f, 20f), true, -0.7f, 1.0f));
        Assert.False(policy.MovedConfirmed);
        Assert.Equal(0f, policy.DirectedNetReverseMeters(new Vector3(10.5f, 0.5f, 20f)), 3);
    }

    [Fact]
    public void SustainedDirectedReverse_ConfirmsMovement()
    {
        var policy = Started();
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.4f), true, -0.7f, 0f));
        Assert.True(policy.Observe(new Vector3(10f, 0f, 20.4f), true, -0.7f, 0.4f));
        Assert.True(policy.MovedConfirmed);
        Assert.Equal(ReversePhase.Moving, policy.Phase);
        Assert.Equal(0.4f, policy.DirectedNetReverseMeters(new Vector3(10f, 0f, 20.4f)), 3);
    }

    [Fact]
    public void ReadableSpeedSign_IsNotInterpreted_ThePositionalProofDecides()
    {
        var policy = Started();
        // Review D9: the native speed SIGN semantics are not asserted anywhere. A readable speed
        // plus sustained DIRECTED reverse displacement is the proof; the policy must not guess
        // that a positive value vetoes it (nor that a negative one authorizes it).
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.6f), true, 6f, 0f));
        Assert.True(policy.Observe(new Vector3(10f, 0f, 20.6f), true, 6f, 0.5f));
        Assert.True(policy.MovedConfirmed);
        Assert.Equal(0.6f, policy.DirectedNetReverseMeters(new Vector3(10f, 0f, 20.6f)), 3);
    }

    [Fact]
    public void UnreadableSpeedStillAllowsDirectedMotionButJitterCancelsIt()
    {
        var policy = Started();
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.3f), false, -1f, 0f));
        // Jitter back below the threshold resets the sustain window.
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.05f), false, -1f, 0.2f));
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.3f), false, -1f, 0.4f));
        Assert.False(policy.MovedConfirmed);
    }

    [Fact]
    public void NaNOrUnreadablePosition_DoesNotConfirmMovement()
    {
        var policy = Started();
        Assert.False(policy.Observe(new Vector3(float.NaN, 0f, 21f), true, -0.7f, 0f));
        Assert.False(policy.Observe(new Vector3(float.NaN, 0f, 21f), true, -0.7f, 0.5f));
        Assert.False(policy.MovedConfirmed);
    }

    [Fact]
    public void ZeroReverseDirection_NeverConfirmsMovement()
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(1, 1, Origin, Vector3.Zero, 0f);
        Assert.False(policy.Observe(new Vector3(10f, 0f, 21f), true, -0.7f, 0f));
        Assert.False(policy.Observe(new Vector3(10f, 0f, 21f), true, -0.7f, 0.5f));
        Assert.False(policy.MovedConfirmed);
    }

    [Theory]
    [InlineData(0.6f, true)]  // 0.6 m reverse, front STILL blocked -> no forward dispatch
    [InlineData(0.6f, false)] // 0.6 m reverse, front clear -> dispatch allowed
    [InlineData(0.3f, false)] // below the meaningful-escape distance -> no dispatch
    public void ForwardGate_RequiresMeaningfulReverseAndAClearFront(float reverseMeters, bool frontBlocked)
    {
        var policy = Started();
        var pos = new Vector3(10f, 0f, 20f + reverseMeters);
        policy.Observe(pos, true, -0.7f, 0f);
        policy.Observe(pos, true, -0.7f, 0.4f);
        Assert.True(policy.MovedConfirmed);
        bool expected = reverseMeters >= ReverseManoeuvrePolicy.ForwardEscapeMeters && !frontBlocked;
        // Review D10: the gate consumes the CURRENT directed net (measured on the captured axis),
        // never a best-ever high-water mark a retrace could fake.
        Assert.Equal(expected, policy.CanDispatchForward(policy.CurrentReverseMeters, frontBlocked));
    }

    [Fact]
    public void Retrace_CannotFakeTheForwardGateWithABestEverHighWaterMark()
    {
        var policy = Started();
        // Confirmed 0.7 m of directed reverse (best-ever would be 0.7).
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.7f), true, -0.7f, 0f));
        Assert.True(policy.Observe(new Vector3(10f, 0f, 20.7f), true, -0.7f, 0.4f));
        Assert.True(policy.MovedConfirmed);
        Assert.True(policy.CanDispatchForward(0.7f, frontBlocked: false));
        // The taxi then rolls FORWARD again, back to 0.2 m net along the reverse axis: the CURRENT
        // net must decide, so the forward gate closes again even though 0.7 m was reached once.
        // (Observe still reports the latched confirmation; what matters is the CURRENT measurement.)
        Assert.True(policy.Observe(new Vector3(10f, 0f, 20.2f), true, 0.7f, 0.8f));
        Assert.Equal(0.2f, policy.CurrentReverseMeters, 3);
        Assert.False(policy.CanDispatchForward(policy.CurrentReverseMeters, frontBlocked: false));
    }

    [Fact]
    public void RequestWindow_IsEnforcedIndependentlyOfTheOverallCap()
    {
        // Review D6: the native request window (1.5 s) is a hard deadline on its own, even though
        // the whole-manoeuvre cap (6 s) is larger.
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(7, 3, Origin, Reverse, now: 0f, requestWindowSeconds: 1.5f);
        Assert.True(policy.IsCurrentFor(7, 3, 1.4f));
        Assert.False(policy.IsCurrentFor(7, 3, 1.5f)); // request window closed
        Assert.False(policy.IsCurrentFor(7, 3, 0.1f + ReverseManoeuvrePolicy.OverallCapSeconds)); // overall cap closed
        Assert.True(policy.Active); // expired is not the same as revoked
    }

    [Fact]
    public void NonFiniteTime_FailsClosedForBothTheRequestWindowAndTheCap()
    {
        var policy = Started(now: 0f);
        Assert.False(policy.IsCurrentFor(7, 3, float.NaN));
        Assert.True(policy.Expired(float.NaN));
    }

    [Fact]
    public void UnreadableSpeed_AtTheSustainTransition_NeverConfirmsMovement()
    {
        var policy = Started();
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.4f), true, -0.7f, 0f));   // sustain opens
        // The confirming sample is unreadable -> the movement proof must not latch (review D9).
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.4f), false, -1f, 0.4f));
        Assert.False(policy.MovedConfirmed);
    }

    [Fact]
    public void YawDriftBeyondTheBoundedDot_NeverConfirmsMovement()
    {
        var policy = Started();
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.4f), true, -0.7f, 0f, forwardDot: 1f));
        // The car has spun far enough that the captured straight-reverse axis no longer describes
        // the manoeuvre (review D9): the dot bound rejects the proof.
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.4f), true, -0.7f, 0.4f, forwardDot: 0.2f));
        Assert.False(policy.MovedConfirmed);
    }

    [Fact]
    public void TransitionPhase_AcceptsNoFurtherReverseEvidence()
    {
        var policy = Started();
        policy.BeginTransition();
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.9f), true, -0.7f, 0f));
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.9f), true, -0.7f, 0.5f));
        Assert.False(policy.MovedConfirmed);
        Assert.False(policy.CanDispatchForward(1f, frontBlocked: false));
    }

    [Fact]
    public void Pause_ShiftsTheRequestWindowTooAndIsNeverMotionProof()
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(7, 3, Origin, Reverse, now: 0f, requestWindowSeconds: 1.5f);
        policy.ShiftPause(10f);
        // The request window moved by exactly the paused seconds (review D11): no shortening, no
        // double shift.
        Assert.True(policy.IsCurrentFor(7, 3, 1.4f + 10f));
        Assert.False(policy.IsCurrentFor(7, 3, 1.5f + 10f));
        // A paused window is not a movement proof.
        Assert.False(policy.Observe(new Vector3(10f, 0f, 20.9f), true, -0.7f, 10.0f));
        Assert.False(policy.MovedConfirmed);
    }

    [Fact]
    public void OverallCap_IsNeverRestartedByAPhaseChangeOrRepeatedImpulse()
    {
        var policy = Started(now: 0f);
        float cap = ReverseManoeuvrePolicy.OverallCapSeconds;
        Assert.False(policy.Expired(cap - 0.1f));
        // A confirmed movement / phase change must not move the deadline.
        policy.Observe(new Vector3(10f, 0f, 21f), true, -0.7f, 0f);
        policy.Observe(new Vector3(10f, 0f, 21f), true, -0.7f, 0.4f);
        Assert.True(policy.MovedConfirmed);
        Assert.False(policy.Expired(cap - 0.1f));
        Assert.True(policy.Expired(cap));
    }

    [Fact]
    public void Pause_ShiftsTheCapExactlyOnce()
    {
        var policy = Started(now: 0f);
        float cap = ReverseManoeuvrePolicy.OverallCapSeconds;
        policy.ShiftPause(100f);
        Assert.False(policy.Expired(cap + 99f));
        Assert.True(policy.Expired(cap + 100f));
    }

    [Fact]
    public void ConsumerGate_RefusesWrongOperationOrderTransitionAndExpiry()
    {
        var policy = Started(op: 7, order: 3, now: 0f);
        Assert.True(policy.IsCurrentFor(7, 3, 0f));
        Assert.False(policy.IsCurrentFor(8, 3, 0f)); // a new operation
        Assert.False(policy.IsCurrentFor(7, 4, 0f)); // a re-dispatch / new order
        Assert.False(policy.IsCurrentFor(7, 3, ReverseManoeuvrePolicy.OverallCapSeconds));
        policy.BeginTransition();
        Assert.False(policy.IsCurrentFor(7, 3, 0f)); // transition owns no reverse input
        Assert.True(policy.Active);
    }

    [Fact]
    public void TransitionAndEnd_AreBoundedAndIdempotent()
    {
        var policy = Started();
        Assert.True(policy.BeginTransition());
        Assert.Equal(ReversePhase.Transition, policy.Phase);
        Assert.True(policy.End());
        Assert.False(policy.Active);
        Assert.False(policy.End());          // idempotent
        Assert.False(policy.BeginTransition()); // nothing to transition
    }

    [Fact]
    public void Matches_RejectsADifferentOperationOrOrder()
    {
        var policy = Started(op: 1, order: 1);
        Assert.False(policy.Matches(2, 1));
        Assert.False(policy.Matches(1, 2));
        policy.End();
        Assert.False(policy.Matches(1, 1));
    }
}

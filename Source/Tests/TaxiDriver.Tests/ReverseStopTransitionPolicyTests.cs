using System;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// F1 — the controlled end of a reverse manoeuvre (brief §7.4/§7.5). The native stop and the
/// independent fallback cleanup run as ISOLATED, re-validated steps; a stop that was not
/// permitted, threw, or was superseded by a STOP/reroute authorizes NO follow-up (no retry,
/// no rescue, no forward dispatch). These drive the SAME runtime helper SpikeRunner calls.
/// </summary>
public class ReverseStopTransitionPolicyTests
{
    [Fact]
    public void PermittedIssuedStop_WithCurrentOperation_AllowsFollowup()
    {
        bool stopped = false;
        var result = ReverseStopTransitionPolicy.Run(
            stopPermitted: () => true,
            stopNative: () => stopped = true,
            stillCurrentAndPermitted: () => true,
            fallbackStopNavigating: () => { },
            fallbackPark: () => { },
            onStepError: _ => { });

        Assert.True(stopped, "the permitted native stop must be issued");
        Assert.True(result.StopPermitted);
        Assert.True(result.StopIssued);
        Assert.False(result.StopThrew);
        Assert.False(result.Superseded);
        Assert.True(result.FollowupAllowed);
    }

    [Fact]
    public void StopNotPermitted_DisallowsFollowup_AndIssuesNoNativeStop()
    {
        bool stopped = false;
        var result = ReverseStopTransitionPolicy.Run(
            stopPermitted: () => false,
            stopNative: () => stopped = true,
            stillCurrentAndPermitted: () => true,
            fallbackStopNavigating: () => { },
            fallbackPark: () => { },
            onStepError: _ => { });

        Assert.False(stopped, "no native stop may be issued when it is not permitted");
        Assert.False(result.StopPermitted);
        Assert.False(result.StopIssued);
        Assert.False(result.FollowupAllowed);
    }

    [Fact]
    public void StopThrows_DisallowsFollowup_ButTheIndependentFallbackStillRuns()
    {
        bool navRan = false, parkRan = false;
        var result = ReverseStopTransitionPolicy.Run(
            stopPermitted: () => true,
            stopNative: () => throw new InvalidOperationException("native StopReversing threw"),
            stillCurrentAndPermitted: () => true,
            fallbackStopNavigating: () => navRan = true,
            fallbackPark: () => parkRan = true,
            onStepError: _ => { });

        Assert.True(result.StopThrew);
        Assert.False(result.FollowupAllowed, "a throwing stop authorizes no follow-up");
        Assert.True(navRan, "the independent StopNavigating fallback must still run");
        Assert.True(parkRan, "the independent park fallback must still run");
    }

    [Fact]
    public void FallbackStopNavigatingThrows_ParkStillRuns_AndTheIssuedStopStaysAllowed()
    {
        bool parkRan = false;
        var result = ReverseStopTransitionPolicy.Run(
            stopPermitted: () => true,
            stopNative: () => { },
            stillCurrentAndPermitted: () => true,
            fallbackStopNavigating: () => throw new InvalidOperationException("StopNavigating threw"),
            fallbackPark: () => parkRan = true,
            onStepError: _ => { });

        Assert.True(result.FallbackNavigatingThrew);
        Assert.True(parkRan, "a throwing StopNavigating must not skip the park step");
        Assert.True(result.FollowupAllowed);
    }

    [Fact]
    public void AuthorityRevokedAfterTheStop_DisallowsFollowup_AndRunsNoFallback()
    {
        bool navRan = false, parkRan = false;
        var result = ReverseStopTransitionPolicy.Run(
            stopPermitted: () => true,
            stopNative: () => { },
            stillCurrentAndPermitted: () => false,
            fallbackStopNavigating: () => navRan = true,
            fallbackPark: () => parkRan = true,
            onStepError: _ => { });

        Assert.True(result.Superseded);
        Assert.False(result.FollowupAllowed);
        Assert.False(navRan || parkRan, "no new native input may run once authority/ownership is gone");
    }

    [Fact]
    public void SynchronousCallbackRerouteDuringTheStop_DisallowsFollowup()
    {
        // The native stop synchronously reroutes/STOPs the operation: the captured operation is no
        // longer current, so no follow-up (retry/rescue/forward dispatch) may run.
        bool rerouted = false;
        var result = ReverseStopTransitionPolicy.Run(
            stopPermitted: () => true,
            stopNative: () => rerouted = true,
            stillCurrentAndPermitted: () => !rerouted,
            fallbackStopNavigating: () => { },
            fallbackPark: () => { },
            onStepError: _ => { });

        Assert.True(rerouted);
        Assert.True(result.Superseded);
        Assert.False(result.FollowupAllowed);
    }

    [Fact]
    public void RevalidationThrow_FailsClosedAndDisallowsFollowup()
    {
        var result = ReverseStopTransitionPolicy.Run(
            stopPermitted: () => true,
            stopNative: () => { },
            stillCurrentAndPermitted: () => throw new InvalidOperationException("getter threw"),
            fallbackStopNavigating: () => { },
            fallbackPark: () => { },
            onStepError: _ => { });

        Assert.True(result.Superseded);
        Assert.False(result.FollowupAllowed);
    }

    // ---------------------------------------------------------------- runtime wiring (F1)

    private static string Redispatch =>
        DriverSafetySourceContractTests.Section(DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs"),
            "private static void TickReDispatch", "internal static void ReDispatch");

    [Fact]
    public void TickReDispatch_RunsTheCheckedTransitionAndTerminalizesOnAFailedStop()
    {
        // The runtime must run the SAME helper and must NOT fall through to a new reverse, a
        // rescue or a forward dispatch when the stop was not safely issued (brief §7.4/§7.5).
        Assert.Contains("ReverseStopTransitionPolicy.Run(", Redispatch);
        Assert.Contains("FollowupAllowed", Redispatch);
        Assert.Contains("reverse stop failed", Redispatch);
        Assert.Contains("SpikeState.Operation.Terminate(", Redispatch);
        // The follow-up paths must be gated behind the transition result.
        int gate = Redispatch.IndexOf("FollowupAllowed", StringComparison.Ordinal);
        int retry = Redispatch.IndexOf("TryStartReverse(agent, veh, Time.unscaledTime, \"reverse ineffective\")", StringComparison.Ordinal);
        int forward = Redispatch.IndexOf("ReDispatch(agent, target, \"post-reverse\")", StringComparison.Ordinal);
        Assert.True(gate >= 0 && retry > gate, "the reverse retry must sit behind the checked transition");
        Assert.True(forward > gate, "the forward dispatch must sit behind the checked transition");
    }
}

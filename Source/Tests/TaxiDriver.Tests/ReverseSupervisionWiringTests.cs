using System.Numerics;
using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Executable regressions for the runtime supervision wiring (review D2/D3/D4/D5/D6/D15/D16).
/// These drive the SAME Unity-free policies the runtime calls, in the SAME order the runtime
/// calls them (the NativeActuatorPatch consumer and the callback drain), so a regression in the
/// policy/consumer contract is caught without a Unity host.
/// </summary>
public class ReverseSupervisionWiringTests
{
    private static readonly Vector3 Origin = new(10f, 0f, 20f);
    private static readonly Vector3 Reverse = new(0f, 0f, 1f);

    // ---- D6: the request window and the overall cap are BOTH enforced at the consumer -------

    [Fact]
    public void ExpiredRequestWindow_SelectsNoReverseAtTheConsumer()
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(operationId: 7, navOrder: 3, Origin, Reverse, now: 0f, requestWindowSeconds: 1.5f);

        Assert.Equal(NativeActuatorMode.Reverse,
            ConsumerMode(policy, operationId: 7, navOrder: 3, now: 1.4f));
        Assert.Equal(NativeActuatorMode.None,
            ConsumerMode(policy, operationId: 7, navOrder: 3, now: 1.5f)); // request window closed
        Assert.Equal(NativeActuatorMode.None,
            ConsumerMode(policy, operationId: 7, navOrder: 3, now: ReverseManoeuvrePolicy.OverallCapSeconds));
    }

    [Fact]
    public void StaleOperationOrOrder_SelectsNoReverseAtTheConsumer()
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(7, 3, Origin, Reverse, now: 0f, requestWindowSeconds: 1.5f);

        Assert.Equal(NativeActuatorMode.None, ConsumerMode(policy, operationId: 8, navOrder: 3, now: 0.5f));
        Assert.Equal(NativeActuatorMode.None, ConsumerMode(policy, operationId: 7, navOrder: 4, now: 0.5f));
        Assert.Equal(NativeActuatorMode.Reverse, ConsumerMode(policy, operationId: 7, navOrder: 3, now: 0.5f));
    }

    [Fact]
    public void NonFiniteClock_SelectsNoReverseAtTheConsumer()
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(7, 3, Origin, Reverse, now: 0f, requestWindowSeconds: 1.5f);
        Assert.Equal(NativeActuatorMode.None, ConsumerMode(policy, operationId: 7, navOrder: 3, now: float.NaN));
    }

    // ---- D4/D15: the controlled transition revokes the consumer grant ----------------------

    [Fact]
    public void ControlledTransition_RevokesTheConsumerGrantBeforeAnyNativeStop()
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(7, 3, Origin, Reverse, now: 0f, requestWindowSeconds: 3f);
        Assert.Equal(NativeActuatorMode.Reverse, ConsumerMode(policy, 7, 3, 1f));

        Assert.True(policy.BeginTransition());
        Assert.True(policy.Active);                       // still owned, but no longer granting input
        Assert.Equal(NativeActuatorMode.None, ConsumerMode(policy, 7, 3, 1.1f));
    }

    // ---- D3/D5: no new native input under a player driver, lost host or lost ownership -----

    [Theory]
    [InlineData(false, true, false, false)]  // host authority lost
    [InlineData(true, false, false, false)]  // not the exact owned vehicle
    [InlineData(true, true, true, false)]    // paused
    [InlineData(true, true, false, true)]    // a real player is at the wheel
    public void UnsafeHostOwnershipPauseOrPlayerDriver_SelectsNoReverse(bool host, bool owned, bool paused, bool player)
    {
        var policy = new ReverseManoeuvrePolicy();
        policy.Begin(7, 3, Origin, Reverse, now: 0f, requestWindowSeconds: 3f);
        Assert.Equal(NativeActuatorMode.Reverse, ConsumerMode(policy, 7, 3, 1f)); // the grant exists
        Assert.Equal(NativeActuatorMode.None,
            NativeActuatorPolicy.Choose(host, owned, paused, player,
                reverseWindow: policy.IsCurrentFor(7, 3, 1f), activeOrder: true, parkIntent: true));
    }

    // ---- D3: a local PASSENGER never suppresses the NPC reverse cleanup ---------------------

    [Fact]
    public void NativeCleanupPermitted_LocalPassengerDoesNotSuppressTheNpcReverseCleanup()
    {
        // A passenger is NOT a player driver: the NPC-driven reverse must still be cleaned up.
        Assert.True(ReverseSupervisionPolicy.NativeCleanupPermitted(host: true, exactOwnedAlive: true, playerDriver: false));
        // A real player at the wheel, a lost host or a foreign/dead handle => managed-only revoke.
        Assert.False(ReverseSupervisionPolicy.NativeCleanupPermitted(true, true, playerDriver: true));
        Assert.False(ReverseSupervisionPolicy.NativeCleanupPermitted(host: false, true, false));
        Assert.False(ReverseSupervisionPolicy.NativeCleanupPermitted(true, exactOwnedAlive: false, false));
    }

    // ---- D2: bounded continuous rear re-check cadence during an active reverse --------------

    [Fact]
    public void RearCheckCadence_IsBoundedAndFailsClosedOnAnUnreadableClock()
    {
        Assert.False(ReverseSupervisionPolicy.ShouldCheckRear(lastCheckAt: 10f, now: 10f + ReverseSupervisionPolicy.RearCheckSeconds - 0.01f));
        Assert.True(ReverseSupervisionPolicy.ShouldCheckRear(lastCheckAt: 10f, now: 10f + ReverseSupervisionPolicy.RearCheckSeconds + 0.01f));
        Assert.True(ReverseSupervisionPolicy.ShouldCheckRear(lastCheckAt: 10f, now: float.NaN));      // unreadable clock
        Assert.True(ReverseSupervisionPolicy.ShouldCheckRear(lastCheckAt: float.NaN, now: 10f));
        Assert.True(ReverseSupervisionPolicy.ShouldCheckRear(lastCheckAt: 10f, now: 9f));             // clock went backwards
        Assert.InRange(ReverseSupervisionPolicy.RearCheckSeconds, 0.1f, 0.25f);
    }

    // ---- D16: no recursion stop -> callback -> stop ----------------------------------------

    [Fact]
    public void ReentrantCallbackFromOurOwnStop_IsDroppedByTheInvalidatedOrder()
    {
        // Mirrors the runtime drain: finalizing a pickup parks the car (StopNavigating), which
        // synchronously fires a Stopped callback for the OLD order. The drain invalidates the
        // order BEFORE the native work, so the reentrant entry can never be queued.
        const int order = 41;
        var operation = new NavigationOperationPolicy();
        operation.Begin();

        string? queued = "Complete";
        int queuedOrder = order;
        bool reentrantQueued = false;

        // --- drain (TickNavigationCallback) ---
        Assert.True(NavigationCallbackPolicy.ShouldAcceptResult(queuedOrder, order, pollingActive: true));
        Assert.Equal(NavigationOutcomeAction.Finalize,
            NavigationCallbackOutcomePolicy.Decide(queued, navToPlayer: true, operationValid: operation.Active, controlledStop: false));
        operation.Terminate("navigation complete", failed: false);
        int newOrder = order + 1; // SpikeState.NextNavOrder() — invalidates the old callback identity

        // the native park/stop fires a Stopped callback for the OLD order synchronously:
        if (NavigationCallbackPolicy.ShouldAcceptResult(order, newOrder, pollingActive: true))
            reentrantQueued = true;

        Assert.False(reentrantQueued, "a callback of the invalidated order must never be queued");
    }

    [Fact]
    public void ControlledStopCallback_ProducesNoActionAtAll()
    {
        // While our own transition owns navigation, every callback (incl. Stopped) is a no-op.
        Assert.Equal(NavigationOutcomeAction.Ignore,
            NavigationCallbackOutcomePolicy.Decide("Stopped", navToPlayer: false, operationValid: true, controlledStop: true));
        Assert.Equal(NavigationOutcomeAction.Ignore,
            NavigationCallbackOutcomePolicy.Decide("Complete", navToPlayer: false, operationValid: true, controlledStop: true));
    }

    /// <summary>Mirrors the NativeActuatorPatch consumer selection exactly (D6/D4/D15).</summary>
    private static NativeActuatorMode ConsumerMode(ReverseManoeuvrePolicy policy, long operationId, int navOrder, float now) =>
        NativeActuatorPolicy.Choose(host: true, exactOwnedVehicle: true, paused: false, playerDriver: false,
            reverseWindow: policy.IsCurrentFor(operationId, navOrder, now), activeOrder: true, parkIntent: true);
}

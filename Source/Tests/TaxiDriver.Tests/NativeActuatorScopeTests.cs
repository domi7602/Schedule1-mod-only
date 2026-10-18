using System;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Deterministic input-order model, NOT Unity wheel/physics execution.</summary>
public class NativeActuatorScopeTests
{
    [Fact]
    public void ConsumerException_FinalizerRestoresAndDoesNotSuppressIt()
    {
        float throttle = .23f;
        bool hand = true, controls = false;
        var error = new InvalidOperationException("consumer");
        var thrown = Assert.Throws<InvalidOperationException>((Action)(() =>
        {
            var scope = NativeActuatorScope.Begin(() => (throttle, hand, controls),
                x => throttle = x, x => hand = x, x => controls = x, true);
            try { throw error; }
            finally { scope.Dispose(); } // exceptional consumer path, no postfix
        }));
        Assert.Same(error, thrown);
        Assert.Equal((.23f, true, false), (throttle, hand, controls));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EveryPhysicsConsumption_ReassertsRequestsThenRestoresNativeInputs(bool reverse)
    {
        float throttle = .23f;
        bool hand = false, controls = false;
        bool outputHand = true; // an old output cannot carry a request to the next call
        for (int frame = 0; frame < 8; frame++)
        {
            throttle = .8f; // hostile Agent/vehicle render overwrite
            hand = false; // native LateUpdate clears it every time
            var before = (throttle, hand, controls);
            var scope = NativeActuatorScope.Begin(() => (throttle, hand, controls),
                x => throttle = x, x => hand = x, x => controls = x, reverse);
            // Model the proven input selection, not WheelCollider physics.
            outputHand = controls && hand;
            Assert.Equal(reverse ? -.7f : 0f, throttle);
            Assert.Equal(!reverse, outputHand);
            Assert.True(controls);
            scope.Dispose();
            Assert.Equal(before, (throttle, hand, controls));
        }
    }

    [Fact]
    public void SnapshotReadFailure_PerformsNoWrites()
    {
        int writes = 0;
        Assert.Throws<InvalidOperationException>(() => NativeActuatorScope.Begin(
            () => throw new InvalidOperationException("read"), _ => writes++, _ => writes++, _ => writes++, true));
        Assert.Equal(0, writes);
    }

    [Fact]
    public void NestedConsumerCalls_RestoreTheirOwnSnapshotsInStackOrder()
    {
        float throttle = .23f;
        bool hand = true, controls = false;
        var outer = NativeActuatorScope.Begin(() => (throttle, hand, controls),
            x => throttle = x, x => hand = x, x => controls = x, true);
        var inner = NativeActuatorScope.Begin(() => (throttle, hand, controls),
            x => throttle = x, x => hand = x, x => controls = x, false);
        inner.Dispose();
        Assert.Equal((-.7f, false, true), (throttle, hand, controls));
        outer.Dispose();
        Assert.Equal((.23f, true, false), (throttle, hand, controls));
    }

    [Fact]
    public void UnreadableThrottleOnRestore_DoesNotSkipOtherSavedInputs()
    {
        float throttle = .23f;
        bool hand = true, controls = false, fail = false;
        var scope = NativeActuatorScope.Begin(() => (throttle, hand, controls),
            x => { if (fail) throw new InvalidOperationException("dead input"); throttle = x; },
            x => hand = x, x => controls = x, true);
        fail = true;
        Assert.ThrowsAny<Exception>(() => scope.Dispose());
        Assert.True(hand);
        Assert.False(controls);
        scope.Dispose(); // never repeat partial restoration on a stale handle
    }

    [Fact]
    public void PartialPrefixWriteFailure_RestoresBeforeRethrowing()
    {
        float throttle = .23f;
        bool hand = true, controls = false;
        bool fail = true;
        Assert.Throws<InvalidOperationException>(() => NativeActuatorScope.Begin(
            () => (throttle, hand, controls), x => throttle = x,
            x => { if (fail) { fail = false; throw new InvalidOperationException("write"); } hand = x; },
            x => controls = x, true));
        Assert.Equal((.23f, true, false), (throttle, hand, controls));
    }

    [Fact]
    public void PostfixAndFinalizer_RestoreSnapshotExactlyOnce()
    {
        float throttle = .23f;
        bool hand = true, controls = false;
        int writes = 0;
        var scope = NativeActuatorScope.Begin(() => (throttle, hand, controls),
            x => { throttle = x; writes++; }, x => { hand = x; writes++; },
            x => { controls = x; writes++; }, true);
        scope.Dispose(); // postfix
        Assert.Equal((.23f, true, false), (throttle, hand, controls));
        Assert.Equal(6, writes);
        scope.Dispose(); // Harmony also runs a finalizer on normal completion
        Assert.Equal(6, writes);
    }

    [Fact]
    public void ReverseInput_ReachesConsumerDespiteHostileRenderWrites()
    {
        float throttle = -.7f;
        bool hand = false, controls = true;
        // Agent.LateUpdate / LandVehicle.Update replace the earlier mod pedal.
        throttle = .4f;
        hand = false; // LandVehicle.LateUpdate clears the request.
        using var scope = NativeActuatorScope.Begin(() => (throttle, hand, controls),
            x => throttle = x, x => hand = x, x => controls = x, true);
        Assert.Equal(-.7f, throttle);
        Assert.True(controls);
        Assert.False(hand);
    }
}

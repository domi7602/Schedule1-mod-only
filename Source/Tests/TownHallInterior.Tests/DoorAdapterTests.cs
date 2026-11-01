using System;
using TownHallInterior;
using Xunit;

public sealed class DoorAdapterTests
{
    [Fact]
    public void SelectorFailureAfterChoicePoisonsWithoutExecutingEitherBranch()
    {
        var selector = new TestSelector { Result = false };
        var adapter = new DoorAdapter(selector);
        var opens = 0;
        var knocks = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => opens++, () => knocks++));
        Assert.False(adapter.Prefix(true, true, 1, () => opens++, () => knocks++));
        Assert.Equal(0, opens);
        Assert.Equal(0, knocks);
    }

    [Fact]
    public void UnreadySelectorPassesNativeBeforeAnyChoiceDecision()
    {
        var selector = new TestSelector { Result = true };
        var adapter = new DoorAdapter(selector);
        int opens = 0, knocks = 0;
        Assert.True(adapter.Prefix(true, true, 1, () => opens++, () => knocks++, selectorReady: false));
        Assert.True(adapter.Prefix(true, true, 1, () => opens++, () => knocks++, selectorReady: false));
        Assert.Equal(0, opens);
        Assert.Equal(0, knocks);
        Assert.Null(selector.Chosen);
        Assert.False(adapter.Prefix(true, true, 1, () => { }, () => { }));
    }

    [Fact]
    public void AuthorityLossWhilePendingClosesChoiceAndPassesNative()
    {
        var selector = new TestSelector { Result = true };
        var adapter = new DoorAdapter(selector);
        Assert.False(adapter.Prefix(true, true, 1, () => { }, () => { }));
        Assert.True(adapter.Prefix(true, false, 1, () => { }, () => { }));
        Assert.Null(selector.Chosen);
    }

    [Fact]
    public void KnockReplayPassesOnlyOnceAndPoisonsAfterException()
    {
        var selector = new TestSelector { Result = true };
        var adapter = new DoorAdapter(selector);
        int native = 0;
        bool nested = true;
        Action knock = () =>
        {
            if (adapter.Prefix(true, true, 1, () => { }, () => { })) native++;
            nested = adapter.Prefix(true, true, 1, () => { }, () => { });
            throw new InvalidOperationException();
        };
        Assert.False(adapter.Prefix(true, true, 1, () => { }, knock));
        var callback = selector.Chosen!;
        callback(DoorAction.Knock);
        callback(DoorAction.Knock);
        Assert.Equal(1, native);
        Assert.False(nested);
        Assert.False(adapter.Prefix(true, true, 1, () => { }, knock));
        adapter.Reset(2);
        Assert.False(adapter.Prefix(true, true, 2, () => { }, knock));
        selector.Chosen!(DoorAction.Cancel);
    }

    [Fact]
    public void ReplayTokenCannotBeConsumedByAnotherDoor()
    {
        var selector = new TestSelector { Result = true };
        var adapter = new DoorAdapter(selector);
        var capturedDoor = new object();
        var otherDoor = new object();
        int capturedNative = 0, otherNative = 0;
        Action knock = () =>
        {
            if (adapter.Prefix(true, true, 1, () => { }, () => { }, identity: otherDoor)) otherNative++;
            if (adapter.Prefix(true, true, 1, () => { }, () => { }, identity: capturedDoor)) capturedNative++;
            Assert.False(adapter.Prefix(true, true, 1, () => { }, () => { }, identity: capturedDoor));
        };
        Assert.False(adapter.Prefix(true, true, 1, () => { }, knock, identity: capturedDoor));
        selector.Chosen!(DoorAction.Knock);
        Assert.Equal(1, otherNative);
        Assert.Equal(1, capturedNative);
    }

    [Fact]
    public void ChoiceIsSingleUseAndUnrelatedDoorIsUntouched()
    {
        var selector = new TestSelector { Result = true };
        var adapter = new DoorAdapter(selector);
        int opens = 0, knocks = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => opens++, () => knocks++));
        Assert.True(adapter.Prefix(false, true, 1, () => opens++, () => knocks++));
        Assert.False(adapter.Prefix(true, true, 1, () => opens++, () => knocks++));
        var callback = selector.Chosen!;
        callback(DoorAction.Open);
        callback(DoorAction.Knock);
        Assert.Equal(1, opens);
        Assert.Equal(0, knocks);
    }

    [Fact]
    public void StaleGenerationAndInvalidTargetCancelCallbacks()
    {
        var selector = new TestSelector { Result = true };
        var adapter = new DoorAdapter(selector);
        int opens = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => opens++, () => { }));
        var stale = selector.Chosen!;
        adapter.Reset(2);
        stale(DoorAction.Open);
        Assert.False(adapter.Prefix(true, true, 2, () => opens++, () => { }, () => false));
        selector.Chosen!(DoorAction.Open);
        Assert.Equal(0, opens);
    }

    [Fact]
    public void CancelDoesNotRunEitherAction()
    {
        var selector = new TestSelector { Result = true };
        var adapter = new DoorAdapter(selector);
        int actions = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
        selector.Chosen!(DoorAction.Cancel);
        Assert.Equal(0, actions);
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
    }

    [Fact]
    public void SelectorExceptionPoisonsWithoutActionAndInvalidatesCallback()
    {
        var selector = new TestSelector { Throw = true };
        var adapter = new DoorAdapter(selector);
        int actions = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
        Assert.Equal(0, actions);
        Assert.Null(selector.Chosen);
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
    }

    [Fact]
    public void SynchronousSelectorCallbackPoisonsWithoutAction()
    {
        var selector = new TestSelector { Result = true, Synchronous = true };
        var adapter = new DoorAdapter(selector);
        int actions = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
        Assert.Equal(0, actions);
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
    }

    private sealed class TestSelector : IDoorSelector
    {
        public bool Result;
        public bool Throw;
        public bool Synchronous;
        public Action<DoorAction>? Chosen;
        public bool TryShow(Action<DoorAction> chosen)
        {
            Chosen = chosen;
            if (Synchronous) chosen(DoorAction.Open);
            if (Throw) throw new InvalidOperationException();
            return Result;
        }
        public void Close() { Chosen = null; }
    }
}

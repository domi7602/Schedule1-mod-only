using System;
using TownHallInterior;
using Xunit;

public sealed class DoorModalHostTests
{
    [Fact]
    public void SelectionWaitsUntilNativeCloseAndRelease()
    {
        var runtime = new FakeRuntime();
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("Rathaus", new[] { "open", "knock", "cancel" }, i => { Assert.Equal(1, i); Assert.Equal(1, runtime.Releases); calls++; }, out _));
        runtime.Selected!(1);
        host.Tick();
        Assert.Equal(0, calls);
        runtime.Closed = true;
        host.Tick();
        Assert.Equal(1, calls);
        host.Tick();
        Assert.Equal(1, calls);
    }
    [Fact]
    public void ShutdownRetriesOwnedCleanupWhenExclusiveFocusReturns()
    {
        var runtime = new FakeRuntime();
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out _));
        runtime.Exclusive = false;
        host.Shutdown();
        Assert.Equal(0, runtime.Releases);
        runtime.Exclusive = true;
        host.Tick();
        Assert.Equal(1, runtime.Releases);
        Assert.Equal(0, calls);
    }

    [Fact]
    public void ClosedStateReadExceptionDoesNotPermitAnotherOpen()
    {
        var runtime = new FakeRuntime { ThrowClosed = true };
        var host = new NativeDoorModalHost(runtime);
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => { }, out _));
        host.Tick();
        Assert.False(host.TryOpen("", new[] { "a", "b", "c" }, _ => { }, out _));
    }

    [Fact]
    public void FailedReleasePoisonsHostInsteadOfReopeningWithUnknownState()
    {
        var runtime = new FakeRuntime { ThrowRelease = true };
        var host = new NativeDoorModalHost(runtime);
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => Assert.Fail("callback"), out var lease));
        lease!.Close();
        Assert.False(host.TryOpen("", new[] { "a", "b", "c" }, _ => { }, out _));
    }

    [Fact]
    public void SynchronousCallbackIsDeferredAndDuplicateIsIgnored()
    {
        var runtime = new FakeRuntime { Synchronous = true };
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, i => { Assert.Equal(2, i); calls++; }, out _));
        Assert.Equal(0, calls);
        runtime.Selected!(1);
        runtime.Closed = true;
        host.Tick();
        Assert.Equal(1, calls);
    }

    [Fact]
    public void ForeignFocusBlocksReleaseUntilOwnershipReturns()
    {
        var runtime = new FakeRuntime();
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out _));
        runtime.Selected!(0); runtime.Closed = true; runtime.Exclusive = false;
        host.Tick();
        Assert.Equal(0, runtime.Releases);
        Assert.Equal(0, calls);
        Assert.False(host.TryOpen("", new[] { "a", "b", "c" }, _ => { }, out _));
        runtime.Exclusive = true;
        host.Tick();
        Assert.Equal(1, calls);
    }

    [Fact]
    public void StaleSceneDiscardsOldCallbackWithoutNativeRelease()
    {
        var runtime = new FakeRuntime();
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out var oldLease));
        var old = runtime.Selected!;
        runtime.Current = false;
        host.Tick();
        oldLease!.Close(); old(0);
        Assert.Equal(0, runtime.Releases);
        runtime.Current = true;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out _));
        old(1);
        runtime.Selected!(2); runtime.Closed = true;
        host.Tick();
        Assert.Equal(1, calls);
    }

    [Fact]
    public void SceneChangeDuringReleaseSuppressesCallback()
    {
        var runtime = new FakeRuntime { InvalidateDuringRelease = true };
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out _));
        runtime.Selected!(0); runtime.Closed = true;
        host.Tick();
        Assert.Equal(0, calls);
    }

    [Fact]
    public void UncertainOpenCleanupKeepsLeaseAndDoesNotTriggerNativeFallback()
    {
        var runtime = new FakeRuntime { FailOpen = true, DenyRelease = true };
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out var lease));
        Assert.NotNull(lease);
        Assert.False(host.TryOpen("", new[] { "a", "b", "c" }, _ => { }, out _));
        runtime.DenyRelease = false;
        runtime.Closed = true;
        host.Tick();
        Assert.Equal(1, calls);
    }

    [Fact]
    public void PartiallyOpenedPopupIsRetriedForOwnedCloseBeforeCancellation()
    {
        var runtime = new FakeRuntime { FailOpen = true, DenyRelease = true };
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, i => { Assert.Null(i); calls++; }, out _));
        Assert.Equal(1, runtime.Releases);
        runtime.DenyRelease = false;
        host.Tick();
        Assert.Equal(2, runtime.Releases);
        Assert.True(runtime.LastCloseOwnedPopup);
        Assert.Equal(1, calls);
    }

    [Fact]
    public void PartialOpenRecoveryNeverClosesWhileForeignFocusIsHeld()
    {
        var runtime = new FakeRuntime { FailOpen = true, DenyRelease = true };
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out _));
        runtime.Exclusive = false;
        runtime.DenyRelease = false;
        host.Tick();
        Assert.Equal(1, runtime.Releases);
        Assert.Equal(0, calls);
        runtime.Exclusive = true;
        host.Tick();
        Assert.Equal(2, runtime.Releases);
        Assert.True(runtime.LastCloseOwnedPopup);
        Assert.Equal(1, calls);
        runtime.Selected!(0);
        host.Tick();
        Assert.Equal(1, calls);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void OpenFailureRollsBackAndNeverDeliversSynchronousResult(bool throws)
    {
        var runtime = new FakeRuntime { Synchronous = true, FailOpen = !throws, ThrowOpen = throws };
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.False(host.TryOpen("", new[] { "a", "b", "c" }, _ => calls++, out _));
        Assert.Equal(1, runtime.Releases);
        runtime.Selected!(0); runtime.Closed = true; host.Tick();
        Assert.Equal(0, calls);
    }

    [Fact]
    public void ReentrantCallbackCanOpenAgainWithoutOldLeaseClosingNewOne()
    {
        var runtime = new FakeRuntime();
        var host = new NativeDoorModalHost(runtime);
        IDoorModalLease? next = null;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ =>
            Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, _ => { }, out next)), out var old));
        runtime.Selected!(1); runtime.Closed = true; host.Tick();
        old!.Close();
        Assert.Equal(1, runtime.Releases);
        next!.Close();
        Assert.Equal(2, runtime.Releases);
    }

    [Fact]
    public void CancelledNativeSelectionDoesNotOpenOrKnock()
    {
        var runtime = new FakeRuntime();
        var host = new NativeDoorModalHost(runtime);
        var adapter = new DoorAdapter(new DoorSelector(host));
        int open = 0, knock = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => open++, () => knock++));
        runtime.Selected!(2); runtime.Closed = true;
        host.Tick();
        Assert.Equal(0, open);
        Assert.Equal(0, knock);
    }

    [Fact]
    public void EscapeWithoutSelectionCancelsAfterClose()
    {
        var runtime = new FakeRuntime();
        var host = new NativeDoorModalHost(runtime);
        int calls = 0;
        Assert.True(host.TryOpen("", new[] { "a", "b", "c" }, i => { Assert.Null(i); calls++; }, out _));
        runtime.Closed = true;
        host.Tick();
        Assert.Equal(1, calls);
    }

    private sealed class FakeRuntime : IDoorModalRuntime
    {
        public bool Current = true, Exclusive = true;
        public bool IsCurrent => Current;
        public bool IsExclusive => Exclusive;
        public bool Closed;
        public bool IsClosed => ThrowClosed ? throw new InvalidOperationException() : Closed;
        public Action<int?>? Selected;
        public int Releases;
        public bool ThrowRelease, Synchronous, InvalidateDuringRelease, FailOpen, ThrowOpen, DenyRelease, ThrowClosed, LastCloseOwnedPopup;
        public bool TryOpen(string[] options, Action<int?> selected) { Selected = selected; if (Synchronous) selected(2); if (ThrowOpen) throw new InvalidOperationException(); return !FailOpen; }
        public bool TryRelease(bool closeOwnedPopup) { Releases++; LastCloseOwnedPopup = closeOwnedPopup; if (ThrowRelease) throw new InvalidOperationException(); if (InvalidateDuringRelease) Current = false; return !DenyRelease; }
    }
}

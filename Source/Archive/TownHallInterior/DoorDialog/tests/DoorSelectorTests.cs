using System;
using TownHallInterior;
using Xunit;

public sealed class DoorSelectorTests
{
    [Theory]
    [InlineData(0, DoorAction.Open)]
    [InlineData(1, DoorAction.Knock)]
    [InlineData(2, DoorAction.Cancel)]
    [InlineData(null, DoorAction.Cancel)]
    public void ChoiceIsDeliveredOnceAndOnlyOwnedLeaseCloses(int? selected, DoorAction expected)
    {
        var host = new FakeHost();
        var adapter = new DoorAdapter(new DoorSelector(host));
        int open = 0, knock = 0, native = 0;
        if (adapter.Prefix(true, true, 1, () => open++, () => knock++)) native++;
        Assert.Equal(new[] { "Tür öffnen", "Anklopfen", "Abbrechen" }, host.Options);
        var late = host.Result!;
        late(selected); late(selected);
        Assert.Equal(expected == DoorAction.Open ? 1 : 0, open);
        Assert.Equal(expected == DoorAction.Knock ? 1 : 0, knock);
        Assert.Equal(0, native);
        Assert.Equal(1, host.Lease.CloseCount);
    }

    [Fact]
    public void DirectCallbackIsAtMostOnceEvenIfConsumerDoesNotClose()
    {
        var host = new FakeHost();
        var selector = new DoorSelector(host);
        int calls = 0;
        Assert.True(selector.TryShow(_ => calls++));
        host.Result!(0); host.Result!(1);
        Assert.Equal(1, calls);
        selector.Close();
        Assert.Equal(1, host.Lease.CloseCount);
    }

    [Fact]
    public void ACompletedSelectionAllowsAnotherPopup()
    {
        var host = new FakeHost();
        var selector = new DoorSelector(host);
        Assert.True(selector.TryShow(_ => { }));
        host.Result!(2);
        Assert.True(selector.TryShow(_ => { }));
    }

    [Fact]
    public void BusyForeignWindowIsSuppressedAndPoisonsUntilAuthoritativeReset()
    {
        var host = new FakeHost { Busy = true };
        var adapter = new DoorAdapter(new DoorSelector(host));
        Assert.False(adapter.Prefix(true, true, 1, () => Assert.Fail("open"), () => Assert.Fail("knock")));
        Assert.Equal(0, host.Lease.CloseCount);
        Assert.False(adapter.Prefix(true, true, 1, () => Assert.Fail("open"), () => Assert.Fail("knock")));
        adapter.Reset(2);
        host.Busy = false;
        Assert.False(adapter.Prefix(true, true, 2, () => { }, () => { }));
        host.Result!(2);
    }

    [Fact]
    public void ResetClosesOnceAndIgnoresLateCallbackAfterReload()
    {
        var host = new FakeHost();
        var adapter = new DoorAdapter(new DoorSelector(host));
        int actions = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
        var late = host.Result!;
        adapter.Reset(2); adapter.Reset(2); late(0);
        Assert.Equal(0, actions);
        Assert.Equal(1, host.Lease.CloseCount);
    }

    [Fact]
    public void PrematurePopupSuppressesNativePoisonsAndClosesItsLease()
    {
        var host = new FakeHost { Premature = true };
        var adapter = new DoorAdapter(new DoorSelector(host));
        int actions = 0;
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
        host.Result!(0);
        Assert.Equal(0, actions);
        Assert.Equal(1, host.Lease.CloseCount);
        Assert.False(adapter.Prefix(true, true, 1, () => actions++, () => actions++));
        Assert.Equal(0, actions);
    }

    [Fact]
    public void SetupFailureWithThrowingLeaseCloseStillPoisonsAndSuppresses()
    {
        var host = new FakeHost { Premature = true };
        host.Lease.Throw = true;
        var adapter = new DoorAdapter(new DoorSelector(host));
        Assert.False(adapter.Prefix(true, true, 1, () => { }, () => { }));
        Assert.Equal(1, host.Lease.CloseCount);
    }

    private sealed class FakeHost : IDoorModalHost
    {
        public bool Busy, Premature;
        public Action<int?>? Result;
        public string[]? Options;
        public FakeLease Lease = new();
        public bool TryOpen(string title, string[] options, Action<int?> result, out IDoorModalLease? lease)
        {
            Options = options; Result = result;
            if (Busy) { lease = null; return false; }
            lease = Lease;
            if (Premature) result(0);
            return true;
        }
    }
    private sealed class FakeLease : IDoorModalLease
    {
        public int CloseCount;
        public bool Throw;
        public void Close() { CloseCount++; if (Throw) throw new InvalidOperationException(); }
    }
}

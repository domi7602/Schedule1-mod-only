using System;
using TownHallInterior;
using Xunit;

public sealed class ExclusiveModalFocusTests
{
    [Fact]
    public void BeginRequiresBaselineAndIsSingleUse()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        Assert.True(focus.IsHeld);
        Assert.False(focus.TryBegin());
        Assert.False(new ExclusiveModalFocus(() => false).TryBegin());
    }

    [Fact]
    public void BaselineIsNotReevaluatedWhileLeased()
    {
        bool baseline = true;
        var focus = new ExclusiveModalFocus(() => baseline);
        Assert.True(focus.TryBegin());
        baseline = false;
        Assert.True(focus.IsHeld);
    }

    [Fact]
    public void ForeignChangeWithoutSlotPoisonsUntilReset()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        focus.NotifyChanged(FocusDomain.Cursor);
        Assert.False(focus.IsHeld);
        Assert.False(focus.TryBegin());
        focus.Reset();
        Assert.True(focus.TryBegin());
    }

    [Fact]
    public void InterleavedIdenticalValuesAreDetectedAsExtraMutation()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        // The owner announced one screen-stack change; a foreign close+reopen of
        // the same popup reports two changes even though the values look identical.
        Assert.True(focus.MutateOwned(FocusDomain.ScreenStack, 1,
            () => focus.NotifyChanged(FocusDomain.ScreenStack)));
        focus.NotifyChanged(FocusDomain.ScreenStack);
        Assert.False(focus.IsHeld);
    }

    [Fact]
    public void OwnedMutationWithExactNotificationsEndsCleanly()
    {
        var focus = new ExclusiveModalFocus(() => true);
        int calls = 0;
        Assert.True(focus.TryBegin());
        Assert.True(focus.MutateOwned(FocusDomain.Cursor, 2, () =>
        {
            calls++;
            focus.NotifyChanged(FocusDomain.Cursor);
            focus.NotifyChanged(FocusDomain.Cursor);
        }));
        Assert.True(focus.IsHeld);
        Assert.True(focus.TryEnd());
        Assert.Equal(1, calls);
        Assert.False(focus.TryEnd());
    }

    [Fact]
    public void UnderReportedMutationRefusesEndAndPoisons()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        Assert.True(focus.MutateOwned(FocusDomain.Movement, 2,
            () => focus.NotifyChanged(FocusDomain.Movement)));
        Assert.False(focus.TryEnd());
        Assert.False(focus.IsHeld);
        Assert.False(focus.TryBegin());
    }

    [Fact]
    public void OverReportedMutationPoisonsImmediately()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        Assert.True(focus.MutateOwned(FocusDomain.Cursor, 1, () =>
        {
            focus.NotifyChanged(FocusDomain.Cursor);
            focus.NotifyChanged(FocusDomain.Cursor);
        }));
        Assert.False(focus.IsHeld);
        Assert.False(focus.TryEnd());
    }

    [Fact]
    public void TypingChangeIsAlwaysForeign()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        focus.NotifyChanged(FocusDomain.Typing);
        Assert.False(focus.IsHeld);
    }

    [Fact]
    public void ForeignActivityOutsideLeaseIsIgnored()
    {
        var focus = new ExclusiveModalFocus(() => true);
        focus.NotifyChanged(FocusDomain.Cursor);
        focus.NotifyChanged(FocusDomain.ScreenStack);
        Assert.True(focus.TryBegin());
        Assert.True(focus.IsHeld);
        Assert.True(focus.TryEnd());
    }

    [Fact]
    public void MutationOutsideLeaseIsRefused()
    {
        var focus = new ExclusiveModalFocus(() => true);
        int calls = 0;
        Assert.False(focus.MutateOwned(FocusDomain.Cursor, 1, () => calls++));
        Assert.Equal(0, calls);
    }

    [Fact]
    public void ThrowingMutationPoisonsAndRethrows()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        Assert.Throws<InvalidOperationException>(() =>
            focus.MutateOwned(FocusDomain.Cursor, 1, () => throw new InvalidOperationException()));
        Assert.False(focus.IsHeld);
        Assert.False(focus.TryEnd());
        focus.Reset();
        Assert.True(focus.TryBegin());
    }

    [Fact]
    public void ResetClearsAnActiveLease()
    {
        var focus = new ExclusiveModalFocus(() => true);
        Assert.True(focus.TryBegin());
        focus.Reset();
        Assert.False(focus.IsHeld);
        Assert.False(focus.TryEnd());
        Assert.True(focus.TryBegin());
    }

    [Fact]
    public void LeasesAreBounded()
    {
        var focus = new ExclusiveModalFocus(() => true);
        for (int i = 0; i < ExclusiveModalFocus.MaxLeases; i++)
        {
            Assert.True(focus.TryBegin());
            Assert.True(focus.TryEnd());
        }
        Assert.False(focus.TryBegin());
    }
}

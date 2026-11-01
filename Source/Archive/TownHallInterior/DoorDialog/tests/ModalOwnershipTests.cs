using TownHallInterior;
using Xunit;

public sealed class ModalOwnershipTests
{
    [Fact]
    public void RestoreRequiresOriginalStackPlayerAndExclusiveLease()
    {
        var snapshot = new ModalFocusState(1, 2, new[] { 10, 11 }, false, true, false, false);
        var closed = new ModalFocusState(1, 2, new[] { 10, 11 }, false, false, false, true);
        Assert.True(ModalOwnership.MayRestore(snapshot, closed, true));
        Assert.False(ModalOwnership.MayRestore(snapshot, closed with { PlayerId = 3 }, true));
        Assert.False(ModalOwnership.MayRestore(snapshot, closed with { ManagerId = 3 }, true));
        Assert.False(ModalOwnership.MayRestore(snapshot, closed with { PopupActive = true }, true));
        Assert.False(ModalOwnership.MayRestore(snapshot, closed with { CanMove = true }, true));
        Assert.False(ModalOwnership.MayRestore(snapshot, closed with { Typing = true }, true));
        Assert.False(ModalOwnership.MayRestore(snapshot, closed with { Stack = new[] { 10 } }, true));
        Assert.False(ModalOwnership.MayRestore(snapshot, closed, false));
    }

    [Fact]
    public void PopupMustBeExactTopOverUnchangedStack()
    {
        var baseline = new[] { 10, 11 };
        Assert.True(ModalOwnership.OwnsTop(baseline, new[] { 20, 10, 11 }, 1, 1, 20, 20, 20));
        Assert.False(ModalOwnership.OwnsTop(baseline, new[] { 21, 20, 10, 11 }, 1, 1, 20, 21, 20));
        Assert.False(ModalOwnership.OwnsTop(baseline, new[] { 20, 11, 10 }, 1, 1, 20, 20, 20));
        Assert.False(ModalOwnership.OwnsTop(baseline, new[] { 20, 10, 11 }, 1, 2, 20, 20, 20));
    }
}

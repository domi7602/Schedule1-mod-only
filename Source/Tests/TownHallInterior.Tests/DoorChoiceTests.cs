using TownHallInterior;
using Xunit;

public sealed class DoorChoiceTests
{
    [Fact]
    public void OpenIsConsumedOnceWithoutKnock()
    {
        var c = new DoorChoice();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
        Assert.Equal(DoorAction.Open, c.Choose(DoorAction.Open));
        Assert.Equal(DoorAction.None, c.Choose(DoorAction.Knock));
    }
    [Fact]
    public void NativeReplayMustPassThroughExactlyOnceDuringReentry()
    {
        var c = new DoorChoice();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
        Assert.Equal(DoorAction.Knock, c.Choose(DoorAction.Knock));
        Assert.True(c.StartReplay());
        Assert.Equal(Intercept.PassNative, c.Interact(true, true, true));
        Assert.Equal(Intercept.Suppress, c.Interact(true, true, true));
        c.EndReplay();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
    }
    [Fact]
    public void TeardownDiscardsPendingAndReplayState()
    {
        var c = new DoorChoice();
        c.Interact(true, true, true);
        c.Choose(DoorAction.Knock);
        Assert.True(c.StartReplay());
        c.Reset();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
        Assert.Equal(DoorAction.None, c.Choose(DoorAction.Cancel));
        c.Reset();
        Assert.Equal(DoorAction.None, c.Choose(DoorAction.Knock));
    }
    [Theory]
    [InlineData(false, true, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    public void UnsafeOrUnreadyInteractionPassesVanilla(bool target, bool singleplayer, bool ready)
    {
        var c = new DoorChoice();
        Assert.Equal(Intercept.PassNative, c.Interact(target, singleplayer, ready));
        Assert.Equal(DoorAction.None, c.Choose(DoorAction.Open));
    }
    [Fact]
    public void DoubleInteractionIsSuppressedAndCancelDoesNothing()
    {
        var c = new DoorChoice();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
        Assert.Equal(Intercept.Suppress, c.Interact(true, true, true));
        Assert.Equal(Intercept.PassNative, c.Interact(false, true, true));
        Assert.Equal(DoorAction.None, c.Choose(DoorAction.Cancel));
        Assert.False(c.StartReplay());
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
    }
    [Fact]
    public void PoisonSuppressesEveryTargetInteractionUntilReset()
    {
        var c = new DoorChoice();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
        c.Poison();
        Assert.Equal(Intercept.Suppress, c.Interact(true, true, true));
        Assert.Equal(Intercept.PassNative, c.Interact(false, true, true));
        Assert.Equal(DoorAction.None, c.Choose(DoorAction.Open));
        c.Reset();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
    }
    [Fact]
    public void PoisonClearsPendingAndReplayState()
    {
        var c = new DoorChoice();
        Assert.Equal(Intercept.ShowChoice, c.Interact(true, true, true));
        Assert.Equal(DoorAction.Knock, c.Choose(DoorAction.Knock));
        Assert.True(c.StartReplay());
        c.Poison();
        Assert.False(c.StartReplay());
        Assert.Equal(Intercept.Suppress, c.Interact(true, true, true));
    }
}
